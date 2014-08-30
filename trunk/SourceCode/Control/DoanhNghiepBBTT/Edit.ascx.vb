Imports System.Data
Imports ThanhTraLaoDongModel
Imports System.Text.RegularExpressions
Imports Cls_Common
Imports SecurityService

Partial Class Control_DoanhNghiep_Edit
    Inherits System.Web.UI.UserControl
    Private Shared ReadOnly log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
#Region "Sub and Function "
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            If Not Session("Username") = "" Then
                Dim script As ScriptManager = ScriptManager.GetCurrent(Me.Page)
                If Not script Is Nothing AndAlso script.IsInAsyncPostBack Then
                    ScriptManager.RegisterStartupScript(Me.Page, Me.Page.GetType, "TTLDjs", "ajaxJquery()", True)
                    ScriptManager.RegisterStartupScript(Me.Page, Me.Page.GetType, "TTLDjs1", "ajaxJqueryToolTip()", True)
                Else
                    Me.Page.ClientScript.RegisterStartupScript(Me.Page.GetType(), "TTLDjs", String.Concat("Sys.Application.add_load(function(){", "ajaxJquery()", "});"), True)
                    Me.Page.ClientScript.RegisterStartupScript(Me.Page.GetType(), "TTLDjs1", String.Concat("Sys.Application.add_load(function(){", "ajaxJqueryToolTip()", "});"), True)
                End If

                If Not Request.QueryString("DNId").ToString.Equals("0") Then
                    hidID.Value = Request.QueryString("DNId")
                End If
                Using data As New ThanhTraLaoDongEntities
                    Dim isUser As String
                    Dim UserName As String
                    isUser = Session("IsUser")
                    UserName = Session("UserName")

                    Dim dn = (From q In data.DoanhNghieps
                              Where q.DoanhNghiepId = hidID.Value
                              Select q.NguoiTao).FirstOrDefault()
                    If Not IsNothing(dn) Then
                        If isUser = 3 Or UserName = dn Then
                            LoadType()
                            LoadData()
                            LoadDoanhNghiep()
                        Else
                            Excute_Javascript("AlertboxRedirect('Bạn không có quyền sửa với doanh nghiệp này.','List.aspx');", Me.Page, True)
                        End If
                    Else
                        Excute_Javascript("AlertboxRedirect('Doanh nghiệp này không tồn tại.','List.aspx');", Me.Page, True)
                    End If
                End Using
            Else
                Response.Redirect("../../Login.aspx")
            End If
        End If
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
            'Kiểm tra là thanh tra bộ/sở
            Dim iChkLoaiTT = data.uspCheckLoaiThanhTraByUserId(userID).FirstOrDefault
            If Not IsNothing(iChkLoaiTT) Then
                Dim lstTinh = (From q In data.Tinhs
                           Order By q.TenTinh
                           Select New With {.Value = q.TinhId, .Text = q.TenTinh}).ToList
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
                With ddlHuyen
                    .Items.Clear()
                    .AppendDataBoundItems() = True
                    .Items.Insert(0, New ListItem(Cls_Common.Str_Opt_Chon, 0))
                End With

                ' Load thông tin khu công nghiệp
                Dim lstKhuCongNghiep = (From q In data.KhuCongNghieps
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
            Else
                Dim lstTinh = (From q In data.Users Join p In data.Tinhs On q.TinhTP Equals p.TinhId
                Where (q.UserId = userID)
                            Order By p.TenTinh
                            Select New With {.Value = q.TinhTP, .Text = p.TenTinh}).ToList
                With ddlTinh
                    .Items.Clear()
                    .AppendDataBoundItems() = True
                    .DataTextField = "Text"
                    .DataValueField = "Value"
                    .DataSource = lstTinh
                    .DataBind()
                End With
                '' Load thông tin mặc định cho Huyện
                Dim tinhId = lstTinh(0).Value
                Dim lsthuyen = (From a In data.Huyens Where a.TinhId = tinhId
                                Order By a.TenHuyen
                            Select New With {.Value = a.HuyenId, .Text = a.TenHuyen}).ToList
                With ddlHuyen
                    .Items.Clear()
                    .AppendDataBoundItems() = True
                    .DataTextField = "Text"
                    .DataValueField = "Value"
                    .DataSource = lsthuyen
                    .DataBind()
                    .Items.Insert(0, New ListItem(Cls_Common.Str_Opt_Chon, 0))
                End With
                ' Load thông tin khu công nghiệp
                Dim lstKhuCongNghiep = (From q In data.KhuCongNghieps Where q.TinhId = tinhId
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
            End If

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

    Protected Sub LoadDoanhNghiep()
        Try
         Using data As New ThanhTraLaoDongEntities
            ' Load data by DoanhNghiepID and Bind Data to Control
            Dim d = (From q In data.DoanhNghieps.Include("LoaiHinhDoanhNghiep").Include("LoaiHinhSanXuat").Include("Tinh").Include("Huyen")
                              Where q.DoanhNghiepId = hidID.Value).FirstOrDefault()
            If Not IsNothing(d) Then
                txtTendoanhnghiep.Text = d.TenDoanhNghiep
                txtTenVietTat.Text = d.TenVietTat
                txtCodeNamtldn.Text = IIf(IsNothing(d.NamTLDN), "", d.NamTLDN)
                ddlLoaiHinhDN.SelectedValue = IIf(IsNothing(d.LoaiHinhDNId), 0, d.LoaiHinhDNId)

                txtTrusochinh.Text = d.TruSoChinh
                    ddlKhuCongNghiep.SelectedValue = IIf(IsNothing(d.KhuCongNghiepId), 0, d.KhuCongNghiepId)
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
                chkIsCongDoan.SelectedValue = IIf(IsNothing(d.IsCongDoan) OrElse Not d.IsCongDoan, 0, 1) 'Math.Abs(CInt(d.IsCongDoan))
                txtCodeDienthoai.Text = d.DienThoai
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
                txtSonguoilamnghenguyhiem.Text = IIf(IsNothing(d.SoNguoiLamNgheNguyHiem) OrElse d.SoNguoiLamNgheNguyHiem = 0, 0, FormatNumber(d.SoNguoiLamNgheNguyHiem))
                txtNguoilamCVCoYCNN.Text = IIf(IsNothing(d.SoNguoiLamCongViecYeuCauNghiemNgat) OrElse d.SoNguoiLamCongViecYeuCauNghiemNgat = 0, 0, FormatNumber(d.SoNguoiLamCongViecYeuCauNghiemNgat))

                txtNguoiLienHe.Text = IIf(IsNothing(d.NguoiLienHe) = True, "", d.NguoiLienHe)
                txtCodeDienThoaiLH.Text = IIf(IsNothing(d.DienThoaiLH) = True, "", d.DienThoaiLH)
                txtEmailLH.Text = IIf(IsNothing(d.EmailLH) = True, "", d.EmailLH)
            End If
            End Using
        Catch ex As Exception
            Excute_Javascript("Alertbox('Việc xuất dữ liệu bị lỗi! " & ex.Message & "');", Me.Page, True)
            log4net.Config.XmlConfigurator.Configure()
            log.Error("Error error " & AddTabSpace(1) & Session("Username") & AddTabSpace(1) & "IP:" & GetIPAddress(), ex)
        End Try
    End Sub

    Protected Sub Save()
        Using data As New ThanhTraLaoDongEntities
            Dim p = (From q In data.DoanhNghieps
                    Where q.DoanhNghiepId = hidID.Value).FirstOrDefault()
            If Not IsNothing(p) Then
                Try
                    Dim iLanThayDoi, iSoNguoiLVDH, iSoNguoiLCVYCNN, iSoLaoDongNu As Integer
                    iLanThayDoi = GetNumberByFormat(txtLanthaydoi.Text.Trim())
                    iSoNguoiLVDH = GetNumberByFormat(txtSonguoilamnghenguyhiem.Text.Trim())
                    iSoNguoiLCVYCNN = GetNumberByFormat(txtNguoilamCVCoYCNN.Text.Trim())

                    iSoLaoDongNu = GetNumberByFormat(txtSolaodongnu.Text.Trim())

                    p.TenDoanhNghiep = txtTendoanhnghiep.Text.Trim()
                    p.TenVietTat = txtTenVietTat.Text.Trim()
                    p.DienThoai = txtCodeDienthoai.Text.Trim()
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
                    p.SoChiNhanh = txtSochinhanh.Text.Trim
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
                    p.NguoiLienHe = txtNguoiLienHe.Text.Trim()
                    p.DienThoaiLH = txtCodeDienThoaiLH.Text.Trim()
                    p.EmailLH = txtEmailLH.Text.Trim
                    p.NguoiSua = Session("Username")
                    p.NgaySua = Date.Now
                    p.IsHoanThanh = True 'Đã nhập đầy đủ thông tin DN. Áp dụng cho DN tạo biên bản thanh tra

                    'Cập nhật phiếu nhập nếu có session("EditInfoPhieu")
                    If Not IsNothing(Session("EditInfoPhieu")) Then
                        'Phiếu nhập
                        Dim iPhieuId As Integer = CInt(Session("PhieuId"))
                        Dim pn As PhieuNhapHeader = (From a In data.PhieuNhapHeaders Where a.DoanhNghiepId = hidID.Value And a.PhieuID = iPhieuId Select a).FirstOrDefault
                        pn.SoChiNhanh = p.SoChiNhanh
                        pn.TongSoNhanVien = p.TongSoNhanVien
                        pn.SoLaoDongNu = p.SoLaoDongNu
                        pn.SoNguoiLamNgheNguyHiem = p.SoNguoiLamNgheNguyHiem
                        pn.SoNguoiLamCongViecYeuCauNghiemNgat = p.SoNguoiLamCongViecYeuCauNghiemNgat
                        pn.TongGiaTriSP = p.TongGiaTriSP
                        pn.TongLoiNhuanSauThue = p.TongLoiNhuanSauThue
                        pn.IsCongDoan = p.IsCongDoan
                        pn.NgaySua = Date.Now
                        pn.NguoiSua = Session("Username")
                        pn.NguoiLienHe = p.NguoiLienHe
                        pn.DienThoaiLH = p.DienThoaiLH
                        pn.EmailLH = p.EmailLH
                        data.SaveChanges()

                        Insert_App_Log("Update  Doanhnghiep:" & txtTendoanhnghiep.Text.Trim() & "", Function_Name.DoanhNghiepBBTT, Audit_Type.Edit, Request.ServerVariables("REMOTE_ADDR"), Session("UserName"))
                        If Session("EditInfoPhieu").ToString.Equals("2") Then
                            Session("EditInfoPhieu") = Nothing
                            Excute_Javascript("AlertboxRedirect('Cập nhật dữ liệu thành công.','../../Page/PhieuKiemTra/ThongTinChung.aspx?DNId=" & hidID.Value & "');", Me.Page, True)
                        Else
                            Session("EditInfoPhieu") = Nothing
                            Excute_Javascript("AlertboxRedirect('Cập nhật dữ liệu thành công.','../../Page/BienBanThanhTra/ThongTinChung.aspx?DNId=" & hidID.Value & "');", Me.Page, True)
                        End If

                    Else
                        data.SaveChanges()
                        Insert_App_Log("Update  Doanhnghiep:" & txtTendoanhnghiep.Text.Trim() & "", Function_Name.DoanhNghiepBBTT, Audit_Type.Edit, Request.ServerVariables("REMOTE_ADDR"), Session("UserName"))
                        Excute_Javascript("AlertboxRedirect('Cập nhật dữ liệu thành công.','" & Request.Url().ToString & "');", Me.Page, True)
                    End If


                Catch ex As Exception
                    log4net.Config.XmlConfigurator.Configure()
                    log.Error("Error error " & AddTabSpace(1) & Session("Username") & AddTabSpace(1) & "IP:" & GetIPAddress(), ex)
                    Excute_Javascript("Alertbox('Cập nhật thất bại.');", Me.Page, True)
                End Try
            Else
                Excute_Javascript("AlertboxRedirect('Không tồn tại doanh nghiệp này.','List.aspx');", Me.Page, True)
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
                LoadDoanhNghiep()
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

            ' Load thông tin khu công nghiệp
            Dim lstKhuCongNghiep = Nothing
            If ddlTinh.SelectedValue > 0 Then
                lstKhuCongNghiep = (From q In data.KhuCongNghieps Where q.TinhId = ddlTinh.SelectedValue
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
            Else
                With ddlKhuCongNghiep
                    .Items.Clear()
                    .AppendDataBoundItems() = True
                    .DataTextField = "Text"
                    .DataValueField = "Value"
                    .DataSource = lstKhuCongNghiep
                    .DataBind()
                    .Items.Insert(0, New ListItem(Cls_Common.Str_Opt_Chon, 0))
                End With
            End If
        End Using
    End Sub

#End Region

End Class
