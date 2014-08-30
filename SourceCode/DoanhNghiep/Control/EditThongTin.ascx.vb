Imports System.Data
Imports ThanhTraLaoDongModel
Imports System.Text.RegularExpressions
Imports Cls_Common
Imports SecurityService

Partial Class DoanhNghiep_Control_EditThongTin
    Inherits System.Web.UI.UserControl
    Private Shared ReadOnly log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
#Region "Sub and Function "
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Dim script As ScriptManager = ScriptManager.GetCurrent(Me.Page)
            If Not script Is Nothing AndAlso script.IsInAsyncPostBack Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.Page.GetType, "TTLDjs", "ajaxJquery()", True)
            Else
                Me.Page.ClientScript.RegisterStartupScript(Me.Page.GetType(), "TTLDjs", String.Concat("Sys.Application.add_load(function(){", "ajaxJquery()", "});"), True)
            End If
            hidTinhThanhTraSo.Value = Session("TinhThanhTraSo")
            Using data As New ThanhTraLaoDongEntities
                Dim _UserName As String = Session("UserName")
                Dim dn = (From q In data.DoanhNghieps
                          Where q.NguoiTao = _UserName
                          Select q.NguoiTao).FirstOrDefault()
                If Not dn Is Nothing Then
                    hidID.Value = _UserName
                    LoadType()
                    LoadData()
                    'getMenu()
                    LoadDoanhNghiep(_UserName)
                Else
                    LoadInit()

                End If
            End Using
        End If
    End Sub
    Protected Sub LoadInit()
        Using data As New ThanhTraLaoDongEntities
            '' Load thông tin Tỉnh theo User login tại đây
            Dim lstTinh = Nothing
            Dim tinhID As Integer = CInt(hidTinhThanhTraSo.Value)
            lstTinh = (From q In data.Tinhs Where q.TinhId = tinhID Select q Order By q.TenTinh
                        Select New With {.Value = q.TinhId, .Text = q.TenTinh})

            With ddlTinh
                .Items.Clear()
                .AppendDataBoundItems() = True
                .DataTextField = "Text"
                .DataValueField = "Value"
                .DataSource = lstTinh
                .DataBind()
                .Items.Insert(0, New ListItem(Cls_Common.Str_Opt_Chon, 0))
            End With

            ' Load thông tin Loại hình Doanh Nghiệp
            Dim lstLHDN = (From q In data.LoaiHinhDoanhNghieps.Include("LoaiHinhDoanhNghiep").Include("LoaiHinhSanXuat").Include("Tinh").Include("Huyen")
                        Select New With {.Value = q.LoaiHinhDNId, .Text = q.TenLoaiHinhDN})

            With ddlLoaiHinhDN
                .Items.Clear()
                .AppendDataBoundItems() = True
                .DataTextField = "Text"
                .DataValueField = "Value"
                .DataSource = lstLHDN
                .DataBind()
                .Items.Insert(0, New ListItem(Cls_Common.Str_Opt_Chon, 0))
            End With
        End Using
    End Sub
    Protected Sub LoadType()
        Using Data As New ThanhTraLaoDongEntities
            Dim lstLinhVuc = (From q In Data.LoaiHinhSanXuats Where q.ParentID = 0
               Order By q.Title
               Select New With {.Value = q.LoaiHinhSXId, .Text = q.Title})

            For Each a In lstLinhVuc
                Dim itm As New ListItem(a.Text, a.Value)
                'insert cap 2
                ddlLinhVuc.Items.Add(itm)
                Dim lstLinhVucSecond = (From q In Data.LoaiHinhSanXuats Where q.ParentID = a.Value
                      Order By q.Title
                      Select New With {.Value = q.LoaiHinhSXId, .Text = q.Title})

                For Each b In lstLinhVucSecond
                    Dim itmSecond As New ListItem(" --- " & b.Text, b.Value)
                    ddlLinhVuc.Items.Add(itmSecond)
                Next
            Next
            ddlLinhVuc.Items.Insert(0, New ListItem(Cls_Common.Str_Opt_Chon, -1))
        End Using
    End Sub
    Protected Sub LoadData()
        Using data As New ThanhTraLaoDongEntities

            '' Load thông tin Tỉnh theo User login tại đây
            Dim userID As Integer = CInt(Session("UserId"))
            Dim isUser As Integer = CInt(Session("IsUser"))

            Dim lstTinh = Nothing
            Dim tinhID As Integer = CInt(hidTinhThanhTraSo.Value)
            lstTinh = (From q In data.Tinhs Where q.TinhId = tinhID Select q Order By q.TenTinh
                        Select New With {.Value = q.TinhId, .Text = q.TenTinh})

            With ddlTinh
                .Items.Clear()
                .AppendDataBoundItems() = True
                .DataTextField = "Text"
                .DataValueField = "Value"
                .DataSource = lstTinh
                .DataBind()
                .Items.Insert(0, New ListItem(Cls_Common.Str_Opt_Chon, 0))
            End With

            '' Load thông tin mặc định cho Huyện
            Dim lstHuyen = (From q In data.Huyens
                            Where q.TinhId = tinhID
                            Order By q.TenHuyen
                            Select New With {.Value = q.HuyenId, .Text = q.TenHuyen}).ToList()

            With ddlHuyen
                .AppendDataBoundItems() = True
                .DataTextField = "Text"
                .DataValueField = "Value"
                .DataSource = lstHuyen
                .DataBind()
                .Items.Insert(0, New ListItem(Cls_Common.Str_Opt_TatCa, -1))
            End With

            ' Load thông tin khu công nghiệp
            Dim lstKhuCongNghiep = (From q In data.KhuCongNghieps Where q.TinhId = tinhID
                                            Select New With {.Value = q.KhuCongNghiepId, .Text = q.TenKhuCongNghiep}).ToList
            With ddlKhuCongNghiep
                .Items.Clear()
                .AppendDataBoundItems() = True
                .DataTextField = "Text"
                .DataValueField = "Value"
                .DataSource = lstKhuCongNghiep
                .DataBind()
                .Items.Insert(0, New ListItem(Cls_Common.Str_Opt_Chon, 0))
            End With

            ' Load thông tin Loại hình Doanh Nghiệp
            Dim lstLHDN = (From q In data.LoaiHinhDoanhNghieps.Include("LoaiHinhDoanhNghiep").Include("LoaiHinhSanXuat").Include("Tinh").Include("Huyen")
                        Select New With {.Value = q.LoaiHinhDNId, .Text = q.TenLoaiHinhDN})

            With ddlLoaiHinhDN
                .Items.Clear()
                .AppendDataBoundItems() = True
                .DataTextField = "Text"
                .DataValueField = "Value"
                .DataSource = lstLHDN
                .DataBind()
                .Items.Insert(0, New ListItem(Cls_Common.Str_Opt_Chon, 0))
            End With

            '' Clear hết Data những biến đã tạo ra 
            lstTinh = Nothing
            lstLHDN = Nothing
            'lstLHSX = Nothing

        End Using
    End Sub

    Protected Sub LoadDoanhNghiep(ByVal _UserName As String)
        Using data As New ThanhTraLaoDongEntities
            ' Load data by DoanhNghiepID and Bind Data to Control
            Dim d = (From q In data.DoanhNghieps.Include("LoaiHinhDoanhNghiep").Include("LoaiHinhSanXuat").Include("Tinh").Include("Huyen")
                              Where q.NguoiTao = _UserName).FirstOrDefault()
            If Not IsNothing(d) Then
                txtTendoanhnghiep.Text = d.TenDoanhNghiep
                'txtTenVietTat.Text =
                txtCodeNamtldn.Text = d.NamTLDN
                ddlLoaiHinhDN.SelectedValue = d.LoaiHinhDNId
                txtTrusochinh.Text = d.TruSoChinh
                If Not IsNothing(d.KhuCongNghiepId) Then
                    ddlKhuCongNghiep.SelectedValue = d.KhuCongNghiepId
                End If

                ddlTinh.SelectedValue = d.TinhId

                '' Load ra danh sách các huyện ứng với tỉnh đó
                Dim tinhID As Integer = d.TinhId
                ddlHuyen.Items.Clear()
                Dim lstHuyen = (From q In data.Huyens
                            Where q.TinhId = tinhID
                            Select New With {.Value = q.HuyenId, .Text = q.TenHuyen}).ToList()

                With ddlHuyen
                    .AppendDataBoundItems() = True
                    .DataTextField = "Text"
                    .DataValueField = "Value"
                    .DataSource = lstHuyen
                    .DataBind()
                    .Items.Insert(0, New ListItem(Cls_Common.Str_Opt_Chon, 0))
                    .SelectedValue = d.HuyenId
                End With
                lstHuyen = Nothing

                txtSotknganhang.Text = d.SoTKNganHang
                txtTennganhang.Text = d.TenNganHang
                txtUrl.Text = d.Url
                txtEmail.Text = d.Email
                 ddlLinhVuc.SelectedValue = IIf(IsNothing(d.LoaiHinhSXId), "-1", d.LoaiHinhSXId)
                If Not IsNothing(d.IsCongDoan) Then
                    chkIsCongDoan.SelectedValue = Math.Abs(CInt(d.IsCongDoan))
                Else
                    chkIsCongDoan.SelectedValue = 0
                End If

                txtDienthoai.Text = d.DienThoai
                txtFax.Text = d.Fax
                txtSochungnhandkkd.Text = d.SoChungNhanDKKD
                If Not IsNothing(d.NgayChungNhanDKKD) Then
                    txtNgaychungnhandkkd.Text = CType(d.NgayChungNhanDKKD, Date).ToString("dd/MM/yyyy")
                End If
                txtLanthaydoi.Text = FormatNumber(d.LanThayDoi)
                If Not IsNothing(d.NgayThayDoi) Then
                    txtNgaythaydoi.Text = CType(d.NgayThayDoi, Date).ToString("dd/MM/yyyy")
                End If
                txtTongsonhanvien.Text = FormatNumber(d.TongSoNhanVien)
                txtSochinhanh.Text = d.SoChiNhanh
                txtSolaodongnu.Text = FormatNumber(d.SoLaoDongNu)
                txtTonggiatrisp.Text = IIf(IsNothing(d.TongGiaTriSP), 0, d.TongGiaTriSP)
                txtTongLoiNhuanSauThue.Text = IIf(IsNothing(d.TongLoiNhuanSauThue), 0, d.TongLoiNhuanSauThue)
                txtSonguoilamnghenguyhiem.Text = FormatNumber(d.SoNguoiLamNgheNguyHiem)
                txtNguoilamCVCoYCNN.Text = FormatNumber(d.SoNguoiLamCongViecYeuCauNghiemNgat)
                txtNguoiLienHe.Text = IIf(IsNothing(d.NguoiLienHe) = True, "", d.NguoiLienHe)
                txtCodeDienThoaiLH.Text = IIf(IsNothing(d.DienThoaiLH) = True, "", d.DienThoaiLH)
                txtEmailLH.Text = IIf(IsNothing(d.EmailLH) = True, "", d.EmailLH)
            End If
        End Using
    End Sub

    Protected Sub Save()
        Using data As New ThanhTraLaoDongEntities
            Dim p = (From q In data.DoanhNghieps
                    Where q.NguoiTao = hidID.Value).FirstOrDefault()
            Dim iLanThayDoi, iSoNguoiLVDH, iSoNguoiLCVYCNN, iSoLaoDongNu As Integer
            iLanThayDoi = GetNumberByFormat(txtLanthaydoi.Text.Trim())
            iSoNguoiLVDH = GetNumberByFormat(txtSonguoilamnghenguyhiem.Text.Trim())
            iSoNguoiLCVYCNN = GetNumberByFormat(txtNguoilamCVCoYCNN.Text.Trim())
            iSoLaoDongNu = GetNumberByFormat(txtSolaodongnu.Text.Trim())
            If Not IsNothing(p) Then
                Try
                    p.TenDoanhNghiep = txtTendoanhnghiep.Text.Trim()

                    p.DienThoai = txtDienthoai.Text.Trim()
                    p.Fax = txtFax.Text.Trim()
                    p.NamTLDN = txtCodeNamtldn.Text.Trim()
                    p.LoaiHinhDNId = ddlLoaiHinhDN.SelectedValue
                    p.TruSoChinh = txtTrusochinh.Text.Trim()
                    p.HuyenId = ddlHuyen.SelectedValue
                    p.TinhId = ddlTinh.SelectedValue
                    p.KhuCongNghiepId = ddlKhuCongNghiep.SelectedValue
                    p.SoTKNganHang = txtSotknganhang.Text.Trim()
                    p.TenNganHang = txtTennganhang.Text.Trim()
                    p.Url = txtUrl.Text.Trim()
                    p.Email = txtEmail.Text.Trim()
                    If Not ddlLinhVuc.SelectedValue.Equals("-1") Then
                        p.LoaiHinhSXId = ddlLinhVuc.SelectedValue
                    End If
                    p.SoChungNhanDKKD = txtSochungnhandkkd.Text.Trim()
                    p.NgayChungNhanDKKD = StringToDate(txtNgaychungnhandkkd.Text.Trim())
                    p.LanThayDoi = iLanThayDoi
                    p.NgayThayDoi = IIf(txtNgaythaydoi.Text.Trim().Equals("") = True, Nothing, StringToDate(txtNgaythaydoi.Text.Trim()))
                    p.SoChiNhanh = txtSochinhanh.Text.Trim()
                    If txtTongsonhanvien.Text.Trim().Equals("") OrElse txtTongsonhanvien.Text.Trim().Equals("0") Then
                        p.TongSoNhanVien = 0
                    Else
                        p.TongSoNhanVien = txtTongsonhanvien.Text.Trim()
                    End If
                    p.SoNguoiLamNgheNguyHiem = iSoNguoiLVDH
                    p.SoNguoiLamCongViecYeuCauNghiemNgat = iSoNguoiLCVYCNN
                    p.SoLaoDongNu = iSoLaoDongNu
                    If txtTonggiatrisp.Text.Trim().Equals("") OrElse txtTonggiatrisp.Text.Trim().Equals("0") Then
                        p.TongGiaTriSP = 0
                    Else
                        p.TongGiaTriSP = txtTonggiatrisp.Text.Trim()
                    End If
                    If txtTongLoiNhuanSauThue.Text.Trim().Equals("") OrElse txtTongLoiNhuanSauThue.Text.Trim().Equals("0") Then
                        p.TongLoiNhuanSauThue = 0
                    Else
                        p.TongLoiNhuanSauThue = txtTongLoiNhuanSauThue.Text.Trim()
                    End If
                    p.IsCongDoan = False
                    If chkIsCongDoan.SelectedValue <> "" Then
                        p.IsCongDoan = chkIsCongDoan.SelectedValue = "1"
                    End If
                    p.NguoiSua = hidID.Value 'Session("Username")
                    p.NgaySua = Date.Now
                    p.NguoiLienHe = txtNguoiLienHe.Text.Trim()
                    p.DienThoaiLH = txtCodeDienThoaiLH.Text.Trim()
                    p.EmailLH = txtEmailLH.Text.Trim
                    data.SaveChanges()
                    'Insert_App_Log("Insert  Doanhnghiep:" & txtTendoanhnghiep.Text.Trim() & "", Function_Name.DoanhNghiep, Audit_Type.Create, Request.ServerVariables("REMOTE_ADDR"), Session("UserName"))
                    Excute_Javascript("Alertbox('Cập nhật dữ liệu thành công.');", Me.Page, True)

                Catch ex As Exception
                    log4net.Config.XmlConfigurator.Configure()
                    log.Error("Error error " & AddTabSpace(1) & Session("Username") & AddTabSpace(1) & "IP:" & GetIPAddress(), ex)
                    Excute_Javascript("Alertbox('Cập nhật thất bại.');", Me.Page, True)
                End Try
            Else
                Try
                    p = New DoanhNghiep
                    p.TenDoanhNghiep = txtTendoanhnghiep.Text.Trim()
                    p.DienThoai = txtDienthoai.Text.Trim()
                    p.Fax = txtFax.Text.Trim()
                    p.NamTLDN = txtCodeNamtldn.Text.Trim()
                    p.LoaiHinhDNId = ddlLoaiHinhDN.SelectedValue
                    p.TruSoChinh = txtTrusochinh.Text.Trim()
                    p.HuyenId = ddlHuyen.SelectedValue
                    p.TinhId = ddlTinh.SelectedValue
                    p.KhuCongNghiepId = ddlKhuCongNghiep.SelectedValue
                    p.SoTKNganHang = txtSotknganhang.Text.Trim()
                    p.TenNganHang = txtTennganhang.Text.Trim()
                    p.Url = txtUrl.Text.Trim()
                    p.Email = txtEmail.Text.Trim()
                    If Not ddlLinhVuc.SelectedValue.Equals("-1") Then
                        p.LoaiHinhSXId = ddlLinhVuc.SelectedValue
                    End If
                    p.SoChungNhanDKKD = txtSochungnhandkkd.Text.Trim()
                    p.NgayChungNhanDKKD = StringToDate(txtNgaychungnhandkkd.Text.Trim())
                    p.LanThayDoi = iLanThayDoi
                    p.NgayThayDoi = IIf(txtNgaythaydoi.Text.Trim().Equals("") = True, Nothing, StringToDate(txtNgaythaydoi.Text.Trim()))
                    p.SoChiNhanh = txtSochinhanh.Text.Trim()
                    If txtTongsonhanvien.Text.Trim().Equals("") OrElse txtTongsonhanvien.Text.Trim().Equals("0") Then
                        p.TongSoNhanVien = 0
                    Else
                        p.TongSoNhanVien = txtTongsonhanvien.Text.Trim()
                    End If
                    p.SoNguoiLamNgheNguyHiem = iSoNguoiLVDH
                    p.SoNguoiLamCongViecYeuCauNghiemNgat = iSoNguoiLCVYCNN
                    p.SoLaoDongNu = iSoLaoDongNu
                    If txtTonggiatrisp.Text.Trim().Equals("") OrElse txtTonggiatrisp.Text.Trim().Equals("0") Then
                        p.TongGiaTriSP = 0
                    Else
                        p.TongGiaTriSP = txtTonggiatrisp.Text.Trim()
                    End If
                    If txtTongLoiNhuanSauThue.Text.Trim().Equals("") OrElse txtTongLoiNhuanSauThue.Text.Trim().Equals("0") Then
                        p.TongLoiNhuanSauThue = 0
                    Else
                        p.TongLoiNhuanSauThue = txtTongLoiNhuanSauThue.Text.Trim()
                    End If
                    p.IsCongDoan = False
                    If chkIsCongDoan.SelectedValue <> "" Then
                        p.IsCongDoan = chkIsCongDoan.SelectedValue = "1"
                    End If
                    p.NguoiTao = hidID.Value 'Session("Username")
                    p.NgayTao = Date.Now
                    data.DoanhNghieps.AddObject(p)
                    data.SaveChanges()
                    'Insert_App_Log("Insert  Doanhnghiep:" & txtTendoanhnghiep.Text.Trim() & "", Function_Name.DoanhNghiep, Audit_Type.Create, Request.ServerVariables("REMOTE_ADDR"), Session("UserName"))
                    Excute_Javascript("Alertbox('Cập nhật dữ liệu thành công.');", Me.Page, True)
                Catch ex As Exception
                    log4net.Config.XmlConfigurator.Configure()
                    log.Error("Error error " & AddTabSpace(1) & Session("Username") & AddTabSpace(1) & "IP:" & GetIPAddress(), ex)
                    Excute_Javascript("Alertbox('Cập nhật thất bại.');", Me.Page, True)
                End Try
                'Excute_Javascript("Alertbox('Không tồn tại doanh nghiệp này');", Me.Page, True)
            End If
        End Using
    End Sub

#End Region
#Region "Event for control "
    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click, btnReset.Click
        Select Case CType(sender, Control).ID
            Case "btnSave"
                Save()
            Case "btnReset"
                LoadDoanhNghiep(Session("UserName"))
        End Select


    End Sub
    Protected Sub ddlTinh_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlTinh.SelectedIndexChanged
        Dim tinhID As Integer = ddlTinh.SelectedValue
        ddlHuyen.Items.Clear()
        Using data As New ThanhTraLaoDongEntities
            Dim lstHuyen = (From q In data.Huyens
                        Where q.TinhId = tinhID
                        Order By q.TenHuyen
                        Select New With {.Value = q.HuyenId, .Text = q.TenHuyen}).ToList()

            With ddlHuyen
                .AppendDataBoundItems() = True
                .DataTextField = "Text"
                .DataValueField = "Value"
                .DataSource = lstHuyen
                .DataBind()
                .Items.Insert(0, New ListItem(Cls_Common.Str_Opt_Chon, 0))
            End With
            lstHuyen = Nothing
        End Using
    End Sub

    Protected Sub getMenu()
        'ddlLoaiHinhSX.Items.Clear()
        'Using data As New ThanhTraLaoDongEntities
        'Dim p As List(Of LoaiHinhSanXuat) = (From q In Data.LoaiHinhSanXuats Select q).ToList
        'ddlLoaiHinhSX.DataValueField = "LoaiHinhSXId"
        'ddlLoaiHinhSX.DataTextField = "Title"

        'RecursiveFillTree(p, 0)
        'ddlLoaiHinhSX.Items.Insert(0, New ListItem("---Tất cả---", "0"))
        'End Using
    End Sub
    Dim level As Integer = 0
    Private Sub RecursiveFillTree(ByVal dtParent As List(Of LoaiHinhSanXuat), ByVal parentID As Integer)
        'level += 1
        ''on the each call level increment 1
        'Dim appender As New StringBuilder()

        'For j As Integer = 0 To level - 1

        '    appender.Append("&nbsp;&nbsp;&nbsp;&nbsp;")
        'Next
        'If level > 0 Then
        '    appender.Append("|__")
        'End If

        'Using data As New ThanhTraLaoDongEntities
        '    Dim dv As List(Of LoaiHinhSanXuat) = (From q In data.LoaiHinhSanXuats
        '                                            Where q.ParentID = parentID Select q).ToList
        '    Dim i As Integer

        '    If dv.Count > 0 Then
        '        For i = 0 To dv.Count - 1
        '            Dim itm As New ListItem(Server.HtmlDecode(appender.ToString() + dv.Item(i).Title.ToString()), dv.Item(i).LoaiHinhSXId.ToString())
        '            'If Check_Selected(dv.Item(i).LoaiHinhSXId) = False Then
        '            '    itm.Attributes.Add("class", "ItemDisabled")
        '            'Else
        '            '    itm.Attributes.Add("class", "ItemActived")
        '            'End If

        '            ddlLoaiHinhSX.Items.Add(itm)
        '            RecursiveFillTree(dtParent, Integer.Parse(dv.Item(i).LoaiHinhSXId.ToString()))
        '        Next
        '    End If
        'End Using
        'level -= 1
        ''on the each function end level will decrement by 1
    End Sub
     
#End Region

End Class
