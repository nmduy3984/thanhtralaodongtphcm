Imports System.Data
Imports System.Transactions
Imports Cls_Common
Imports SecurityService
Imports ThanhTraLaoDongModel
Imports Novacode
Imports System.IO
Partial Class Control_Quyetdinhthanhtra_Edit
    Inherits System.Web.UI.UserControl
    Private Shared ReadOnly log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
#Region "Sub and Function "
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Dim script As ScriptManager = ScriptManager.GetCurrent(Me.Page)
            If Not script Is Nothing AndAlso script.IsInAsyncPostBack Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.Page.GetType, "duyjs", "ajaxJquery()", True)
                ScriptManager.RegisterStartupScript(Me.Page, Me.Page.GetType, "duyjs1", "ajaxJqueryToolTip()", True)
            Else
                Me.Page.ClientScript.RegisterStartupScript(Me.Page.GetType(), "duyjs", String.Concat("Sys.Application.add_load(function(){", "ajaxJquery()", "});"), True)
                Me.Page.ClientScript.RegisterStartupScript(Me.Page.GetType(), "duyjs1", String.Concat("Sys.Application.add_load(function(){", "ajaxJqueryToolTip()", "});"), True)
            End If
            If Not Request.QueryString("SoQuyetDinh").ToString.Equals("") Then
                hidID.Value = Request.QueryString("SoQuyetDinh")
                LoadData()
                ShowData()
            End If
        End If
    End Sub
    Protected Sub LoadData()
        Using data As New ThanhTraLaoDongEntities
            'Số quyết định
            Dim userID As Integer = CInt(Session("UserId"))
            Dim isUser As Integer = CInt(Session("IsUser"))
            Dim TinhUser As Tinh = (From t In data.Tinhs Join u In data.Users On t.TinhId Equals u.TinhTP Where u.UserId = userID Select t).SingleOrDefault
            Dim chkTT = data.uspCheckLoaiThanhTraByUserId(userID).FirstOrDefault
            Dim isThanhTra As Integer = IIf(Not IsNothing(chkTT) AndAlso chkTT.UserId > 0, 1, 0) '1: thanh tra bộ; 0: thanh tra sở           
            lblNDSQD.Text = "/" + Now().Year.ToString() + "/" + IIf(isThanhTra = 1 AndAlso isUser = 1, "Hà Nội", TinhUser.KiHieu)
            'Load chức danh
            Dim cd = (From a In data.vChucDanhs Select New With {.Text = a.Name, .Value = a.Id}).ToList
            With ddlChucDanhNguoiKy
                .DataTextField = "Text"
                .DataValueField = "Value"
                .DataSource = cd
                .DataBind()
            End With
            'Địa bàn thanh tra, kiểm tra
            '' Load thông tin Tỉnh theo User login tại đây

            Dim lstTinh = Nothing

            '' Xét xem User đó là loại nào để Load danh sách tỉnh ra
            If isThanhTra > 1 Then
                lstTinh = (From q In data.Tinhs Where Not q.KiHieu.Equals("THANH TRA BO")
                           Order By q.TenTinh
                           Select New With {.Value = q.TinhId, .Text = q.TenTinh})
                With ddlDiaBanTTKT
                    .Items.Clear()
                    .AppendDataBoundItems = True
                    .DataTextField = "Text"
                    .DataValueField = "Value"
                    .DataSource = lstTinh
                    .DataBind()
                    .Items.Insert(0, New ListItem(Cls_Common.Str_Opt_ToanQuoc, "0"))
                End With
            Else
                lstTinh = (From q In data.UsersTinhs Join p In data.Tinhs On q.TinhId Equals p.TinhId
                            Where (q.UserId = userID)
                            Order By p.TenTinh
                            Select New With {.Value = q.TinhId, .Text = p.TenTinh})
                With ddlDiaBanTTKT
                    .Items.Clear()
                    .AppendDataBoundItems = True
                    .DataTextField = "Text"
                    .DataValueField = "Value"
                    .DataSource = lstTinh
                    .DataBind()
                End With
            End If

        End Using
    End Sub
    Protected Sub ShowData()
        Using data As New ThanhTraLaoDongEntities
            Dim p As QuyetDinhThanhTra = (From q In data.QuyetDinhThanhTras Where q.SoQuyetDinh.Equals(hidID.Value) Select q).SingleOrDefault
            If Not p Is Nothing Then
                If p.IsEdited Then
                    txtCodeSoQuyetDinh.Visible = False
                    lblNDSQD.Text = IIf(IsNothing(p.SoQuyetDinh) = True, "", "<b>" + p.SoQuyetDinh + "</b>")
                Else
                    Dim strQD() As String = p.SoQuyetDinh.Split("/")
                    txtCodeSoQuyetDinh.Text = strQD(0)
                    lblNDSQD.Text = "/" & strQD(1) & "/" & strQD(2)
                End If

                ddlLoaiQuyetDinh.SelectedValue = IIf(IsNothing(p.LoaiQuyetDinh) = True, 0, p.LoaiQuyetDinh)
                txtPhamVi.Text = IIf(IsNothing(p.PhamVi) = True, "", p.PhamVi)
                If Not IsNothing(p.CanCuLuat) Then
                    Dim arrCanCuLuat() As String = p.CanCuLuat.Split(Str_Symbol_Group)
                    Dim str = ""
                    For index As Integer = 0 To arrCanCuLuat.Count - 2
                        '' Thêm dữ liệu vào document
                        str += arrCanCuLuat(index) + Environment.NewLine
                    Next
                    str += arrCanCuLuat(arrCanCuLuat.Count - 1)
                    txtCanCuLuat.Text = str
                End If

                If Not IsNothing(p.CanCuQuyetDinh) Then
                    Dim arrCanCuQuyetDinh() As String = p.CanCuQuyetDinh.Split(Str_Symbol_Group)
                    Dim str = ""
                    For index As Integer = 0 To arrCanCuQuyetDinh.Count - 2
                        '' Thêm dữ liệu vào document
                        str += arrCanCuQuyetDinh(index) + Environment.NewLine
                    Next
                    str += arrCanCuQuyetDinh(arrCanCuQuyetDinh.Count - 1)
                    txtCancuquyetdinh.Text = str
                End If
                ddlDiaBanTTKT.SelectedValue = IIf(IsNothing(p.DiaBanTTKT) = True, 0, p.DiaBanTTKT)

                If Not IsNothing(p.ThanhVienDoan) Then
                    Dim arrThanhVienDoan() As String = p.ThanhVienDoan.Split(Str_Symbol_Group)
                    Dim str = ""
                    For index As Integer = 0 To arrThanhVienDoan.Count - 2
                        '' Thêm dữ liệu vào document
                        str += arrThanhVienDoan(index) + Environment.NewLine
                    Next
                    str += arrThanhVienDoan(arrThanhVienDoan.Count - 1)
                    txtThanhVienDoan.Text = str
                End If
                txtTrachNhiemThiHanh.Text = IIf(IsNothing(p.TrachNhiemThiHanh) = True, "", p.TrachNhiemThiHanh)
                ddlChucDanhNguoiKy.SelectedValue = IIf(IsNothing(p.ChucDanhNguoiKy) = True, 0, p.ChucDanhNguoiKy)
                If Not IsNothing(p.NoiNhan) Then
                    Dim arrNoiNhan() As String = p.NoiNhan.Split(Str_Symbol_Group)
                    Dim str = ""
                    For index As Integer = 0 To arrNoiNhan.Count - 2
                        '' Thêm dữ liệu vào document
                        str += arrNoiNhan(index) + Environment.NewLine
                    Next
                    str += arrNoiNhan(arrNoiNhan.Count - 1)
                    txtNoiNhan.Text = str
                End If
                txtNguoiKyQuyetDinh.Text = IIf(IsNothing(p.NguoiKyQuyetDinh) = True, "", p.NguoiKyQuyetDinh)

                'Dùng cho load dữ liệu doanh nghiệp
                Dim dn = (From a In data.QuyetDinhTTDoanhNghieps Join c In data.DoanhNghieps On a.DoanhNghiepId Equals c.DoanhNghiepId
                                                                Where a.QuyetDinhTT.Equals(p.SoQuyetDinh)
                                                                Order By c.ThoiGianLamViec
                                                                Select c).ToList()
                Dim lstDoanhNghiepId = ""
                Dim lstDoanhNghiep = ""
                Dim lstDiaChi = ""
                Dim lstTinh = ""
                Dim lstHuyen = ""
                Dim lstThoiGian = ""
                If dn.Count > 0 Then
                    For i As Integer = 0 To dn.Count - 2
                        lstDoanhNghiepId &= dn(i).DoanhNghiepId & Str_Symbol_Group
                        lstDoanhNghiep &= dn(i).TenDoanhNghiep + Str_Symbol_Group
                        lstDiaChi &= dn(i).TruSoChinh + Str_Symbol_Group
                        lstTinh &= dn(i).TinhId & Str_Symbol_Group
                        lstHuyen &= dn(i).HuyenId & Str_Symbol_Group
                        lstThoiGian &= CType(dn(i).ThoiGianLamViec, Date).ToString("dd/MM/yyyy") + Str_Symbol_Group
                    Next
                    lstDoanhNghiepId &= dn(dn.Count - 1).DoanhNghiepId
                    lstDoanhNghiep &= dn(dn.Count - 1).TenDoanhNghiep
                    lstDiaChi &= dn(dn.Count - 1).TruSoChinh
                    lstTinh &= dn(dn.Count - 1).TinhId
                    lstHuyen &= dn(dn.Count - 1).HuyenId
                    lstThoiGian &= CType(dn(dn.Count - 1).ThoiGianLamViec, Date).ToString("dd/MM/yyyy")
                End If
                hidlstDoanhNghiepId.Value = lstDoanhNghiepId
                hidlstDoanhNghiep.Value = lstDoanhNghiep
                hidlstDiaChi.Value = lstDiaChi
                hidlstTinh.Value = lstTinh
                hidlstHuyen.Value = lstHuyen
                hidlstThoiGian.Value = lstThoiGian

                'Dùng cho load dữ liệu theo user tạo số quyết định này
                Dim us As User = (From a In data.Users Where a.UserName.Equals(p.NguoiTao)).SingleOrDefault
                Dim userID As Integer = us.UserId
                Dim isUser As Integer = us.IsUser
                Dim TinhUser As Tinh = (From t In data.Tinhs Join u In data.Users On t.TinhId Equals u.TinhTP Where u.UserId = userID Select t).SingleOrDefault
                Dim chkTT = data.uspCheckLoaiThanhTraByUserId(userID).FirstOrDefault
                Dim isThanhTra As Integer = IIf(Not IsNothing(chkTT) AndAlso chkTT.UserId > 0, 1, 0) '1: thanh tra bộ; 0: thanh tra sở  
                hidIsThanhTra.Value = isThanhTra
                hidTinhUser.Value = TinhUser.TinhId
                Session("SoQD") = p.SoQuyetDinh
            End If
        End Using
    End Sub
#End Region
#Region "Event for control"
    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Using data As New ThanhTraLaoDongEntities
            Dim p As QuyetDinhThanhTra = (From q In data.QuyetDinhThanhTras Where q.SoQuyetDinh.Equals(hidID.Value)).SingleOrDefault
            Try
                p.LoaiQuyetDinh = CInt(ddlLoaiQuyetDinh.SelectedValue)
                'Phạm vi
                Dim strResult As String = ""
                For Each item In ReadAllLines(txtPhamVi.Text)
                    If Not String.Equals(item, "") Then
                        strResult = strResult & item & Str_Symbol_Group
                    End If
                Next
                'Cắt đi ký tự "Str_Symbol_Group" cuối cùng.
                If strResult.Length > 0 Then
                    p.PhamVi = strResult.Substring(0, strResult.Length - Str_Symbol_Group.Length)
                Else
                    p.PhamVi = Nothing
                End If
                'Căn cứ luật
                strResult = ""
                For Each item In ReadAllLines(txtCanCuLuat.Text.Trim())
                    If Not String.Equals(item, "") Then
                        strResult = strResult & item & Str_Symbol_Group
                    End If
                Next
                'Cắt đi ký tự "Str_Symbol_Group" cuối cùng.
                If strResult.Length > 0 Then
                    p.CanCuLuat = strResult.Substring(0, strResult.Length - Str_Symbol_Group.Length)
                Else
                    p.CanCuLuat = Nothing
                End If
                'Căn cứ quyết định
                strResult = ""
                For Each item In ReadAllLines(txtCancuquyetdinh.Text.Trim())
                    If Not String.Equals(item, "") Then
                        strResult = strResult & item & Str_Symbol_Group
                    End If
                Next
                'Cắt đi ký tự "Str_Symbol_Group" cuối cùng.
                If strResult.Length > 0 Then
                    p.CanCuQuyetDinh = strResult.Substring(0, strResult.Length - Str_Symbol_Group.Length)
                Else
                    p.CanCuQuyetDinh = Nothing
                End If

                p.DiaBanTTKT = CInt(ddlDiaBanTTKT.SelectedValue)
                'Thành viên đoàn
                strResult = ""
                For Each item In ReadAllLines(txtThanhVienDoan.Text.Trim())
                    If Not String.Equals(item, "") Then
                        strResult = strResult & item & Str_Symbol_Group
                    End If
                Next
                'Cắt đi ký tự "Str_Symbol_Group" cuối cùng.
                If strResult.Length > 0 Then
                    p.ThanhVienDoan = strResult.Substring(0, strResult.Length - Str_Symbol_Group.Length)
                Else
                    p.ThanhVienDoan = Nothing
                End If
                'Trách nhiệm thi hành
                strResult = ""
                For Each item In ReadAllLines(txtTrachNhiemThiHanh.Text.Trim())
                    If Not String.Equals(item, "") Then
                        strResult = strResult & item & Str_Symbol_Group
                    End If
                Next
                'Cắt đi ký tự "Str_Symbol_Group" cuối cùng.
                If strResult.Length > 0 Then
                    p.TrachNhiemThiHanh = strResult.Substring(0, strResult.Length - Str_Symbol_Group.Length)
                Else
                    p.TrachNhiemThiHanh = Nothing
                End If
                p.ChucDanhNguoiKy = ddlChucDanhNguoiKy.SelectedValue
                'Nơi nhận
                strResult = ""
                For Each item In ReadAllLines(txtNoiNhan.Text.Trim())
                    If Not String.Equals(item, "") Then
                        strResult = strResult & item & Str_Symbol_Group
                    End If
                Next
                'Cắt đi ký tự "Str_Symbol_Group" cuối cùng.
                If strResult.Length > 0 Then
                    p.NoiNhan = strResult.Substring(0, strResult.Length - Str_Symbol_Group.Length)
                Else
                    p.NoiNhan = Nothing
                End If
                p.NguoiKyQuyetDinh = txtNguoiKyQuyetDinh.Text.Trim()
                p.NgaySua = Now()
                p.NguoiSua = Session("UserName")
                data.SaveChanges()
                'Lưu query Quyết định thanh tra vừa cập nhật
                Dim strInsert = "update QuyetDinhThanhTra set "
                strInsert = strInsert & "SoQuyetDinh = '" & p.SoQuyetDinh & "',LoaiQuyetDinh =" & p.LoaiQuyetDinh & ", PhamVi = '" & p.PhamVi & "', CanCuLuat = '" & p.CanCuLuat & "',CanCuQuyetDinh = '" & p.CanCuQuyetDinh & "',DiaBanTTKT = " & p.DiaBanTTKT
                strInsert = strInsert & ",ThanhVienDoan = '" & p.ThanhVienDoan & "', TrachNhiemThiHanh = '" & p.TrachNhiemThiHanh & "', ChucDanhNguoiKy = " & p.ChucDanhNguoiKy & ", NoiNhan = '" & p.NoiNhan & "', NguoiKyQuyetDinh = '" & p.NguoiKyQuyetDinh & "', NgayTao = '" & p.NgayTao & "', NguoiTao = '" & p.NguoiTao & "' where SoQuyetDinh = ;"
                'Save danh sách doanh nghiệp
                Dim lstDoanhNghiepId() As String = hidlstDoanhNghiepId.Value.Split(Str_Symbol_Group)
                Dim lstDoanhNghiep() As String = hidlstDoanhNghiep.Value.Split(Str_Symbol_Group)
                Dim lstDiaChi() As String = hidlstDiaChi.Value.Split(Str_Symbol_Group)
                Dim lstTinh() As String = hidlstTinh.Value.Split(Str_Symbol_Group)
                Dim lstHuyen() As String = hidlstHuyen.Value.Split(Str_Symbol_Group)
                Dim lstThoiGian() As String = hidlstThoiGian.Value.Split(Str_Symbol_Group)
                For i As Integer = 0 To lstDoanhNghiep.Length - 2
                    Dim dnid As Integer = 0
                    If Not lstDoanhNghiepId(i).Equals("") Then
                        dnid = lstDoanhNghiepId(i)
                    End If
                    Dim dn As DoanhNghiep = (From a In data.DoanhNghieps Where a.DoanhNghiepId = dnid).SingleOrDefault
                    If Not IsNothing(dn) Then
                        With dn
                            .TenDoanhNghiep = lstDoanhNghiep(i)
                            .TruSoChinh = lstDiaChi(i)
                            .TinhId = CInt(lstTinh(i))
                            .HuyenId = CInt(lstHuyen(i))
                            .ThoiGianLamViec = StringToDate(lstThoiGian(i))
                            .NgaySua = Now()
                            .NguoiSua = Session("UserName")
                        End With
                        data.SaveChanges()
                    Else
                        dn = New DoanhNghiep
                        With dn
                            .TenDoanhNghiep = lstDoanhNghiep(i)
                            .TruSoChinh = lstDiaChi(i)
                            .TinhId = CInt(lstTinh(i))
                            .HuyenId = CInt(lstHuyen(i))
                            .ThoiGianLamViec = StringToDate(lstThoiGian(i))
                            .NgayTao = Now()
                            .NguoiTao = Session("UserName")
                        End With
                        data.DoanhNghieps.AddObject(dn)
                        data.SaveChanges()

                        Dim qdtt_dn As New QuyetDinhTTDoanhNghiep
                        With qdtt_dn
                            .QuyetDinhTT = p.SoQuyetDinh
                            .DoanhNghiepId = dn.DoanhNghiepId
                            .IsMoi = True
                        End With
                        data.QuyetDinhTTDoanhNghieps.AddObject(qdtt_dn)
                        data.SaveChanges()
                    End If
                Next
                'Xóa những doanh nghiệp xóa trên lưới load từ database
                Dim lstDNIdDel() As String = hidlstDNIdDel.Value.Split(Str_Symbol_Group)
                If lstDNIdDel.Length > 0 Then
                    For i As Integer = 0 To lstDNIdDel.Length - 2
                        Dim dnid As Integer = CInt(lstDNIdDel(i))
                        'Xóa thông tin doanh nghiệp tại bảng QuyetDinhTTDoanhNghiep và Doanh nghiệp
                        Dim qdttdn As QuyetDinhTTDoanhNghiep = (From a In data.QuyetDinhTTDoanhNghieps Where a.QuyetDinhTT.Equals(p.SoQuyetDinh) And a.DoanhNghiepId = dnid).SingleOrDefault
                        data.QuyetDinhTTDoanhNghieps.DeleteObject(qdttdn)
                        data.SaveChanges()
                        'Kiểm tra doanh nghiệp có trong PhieuNhapHeader?
                        'Nếu không có thì cho xóa
                        Dim pn = (From a In data.PhieuNhapHeaders Where a.DoanhNghiepId = dnid).ToList
                        If pn.Count = 0 Then
                            Dim dn As DoanhNghiep = (From a In data.DoanhNghieps Where a.DoanhNghiepId = dnid).SingleOrDefault
                            data.DoanhNghieps.DeleteObject(dn)
                            data.SaveChanges()
                        End If
                    Next
                End If
                If IsNothing(p.IsEdited) OrElse p.IsEdited = False Then
                    data.ExecuteStoreCommand("update QuyetDinhTTDoanhNghiep set QuyetDinhTT=N'" & txtCodeSoQuyetDinh.Text.Trim & lblNDSQD.Text.Trim & "' where QuyetDinhTT=N'" & hidID.Value & "'")
                    data.ExecuteStoreCommand("update QuyetDinhThanhTra set SoQuyetDinh=N'" & txtCodeSoQuyetDinh.Text.Trim & lblNDSQD.Text.Trim & "' where SoQuyetDinh=N'" & hidID.Value & "'")
                    hidID.Value = txtCodeSoQuyetDinh.Text.Trim & lblNDSQD.Text.Trim
                    Session("SoQD") = txtCodeSoQuyetDinh.Text.Trim & lblNDSQD.Text.Trim
                End If
                Insert_App_Log("Update Quyetdinhthanhtra:" & p.SoQuyetDinh & "", Function_Name.QuyetDinhThanhTra, Audit_Type.Create, Request.ServerVariables("REMOTE_ADDR"), Session("UserName"))
                Excute_Javascript("AlertboxRedirect('Cập nhật dữ liệu thành công.','../../Page/QuyetdinhthanhtraTTS/Edit.aspx?SoQuyetDinh=" & hidID.Value & "');", Me.Page, True)
            Catch ex As Exception
                log4net.Config.XmlConfigurator.Configure()
                log.Error("Error error " & AddTabSpace(1) & Session("Username") & AddTabSpace(1) & "IP:" & GetIPAddress(), ex)
                Excute_Javascript("Alertbox('Cập nhật thất bại.');", Me.Page, True)
            End Try
        End Using
    End Sub

    Protected Sub btnBack_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnBack.Click
        Response.Redirect("List.aspx")
    End Sub
    Protected Sub Button_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnInQuyetDinh.Click, btnInDSDoanhNghiep.Click
        Select Case CType(sender, Control).ID
            Case "btnInQuyetDinh"
                InQuyetDinh()
            Case "btnInDSDoanhNghiep"
                InDSDoanhNghiep()
        End Select
    End Sub
    Protected Sub InQuyetDinh()
        Using sc As New TransactionScope
            Try
                Using data As New ThanhTraLaoDongEntities
                    Dim FolderPath = Server.MapPath("~/Template").ToString
                    Dim attributes As FileAttributes = File.GetAttributes(FolderPath & "\QuyetDinhThanhTra_Template.docx")
                    If (attributes And FileAttributes.ReadOnly) = FileAttributes.ReadOnly Then
                        attributes = attributes And Not FileAttributes.ReadOnly
                        File.SetAttributes(FolderPath & "\QuyetDinhThanhTra_Template.docx", attributes)
                    End If
                    Dim pParagraph As Paragraph
                    Using document As DocX = DocX.Load(FolderPath & "\QuyetDinhThanhTra_Template.docx")
                        ''Get info Quyết định thanh tra
                        Dim soQD = Session("SoQD").ToString
                        Dim qdtt = (From a In data.QuyetDinhThanhTras Where a.SoQuyetDinh.Equals(soQD) Select a).SingleOrDefault
                        'Xử lý cho cách <<thanhtra>>
                        Dim strThanhTra = ""
                        Dim quyetdinh = ""
                        If Not IsNothing(qdtt.LoaiQuyetDinh) Then
                            If qdtt.LoaiQuyetDinh = 1 Then
                                strThanhTra = "Thanh tra"
                                quyetdinh = "Thanh tra việc chấp hành các quy định của " & qdtt.PhamVi & " tại doanh nghiệp"
                            Else
                                strThanhTra = "Kiểm tra"
                                quyetdinh = "Kiểm tra việc chấp hành các quy định của " & qdtt.PhamVi & " tại doanh nghiệp"
                            End If
                        End If
                        '' Phần Cơ quan ban hành
                        'Xác định thanh tra bộ hay thanh tra sở
                        Dim iUserId As Integer = Session("userid")
                        Dim chkTT = data.uspCheckLoaiThanhTraByUserId(iUserId).FirstOrDefault
                        Dim TinhUserLogin As Tinh = (From t In data.Tinhs Join u In data.Users On t.TinhId Equals u.TinhTP Where u.UserId = iUserId Select t).SingleOrDefault
                        Dim DiaBanTT As Tinh = (From t In data.Tinhs Where t.TinhId = qdtt.DiaBanTTKT Select t).SingleOrDefault
                        'Tỉnh nơi nhận theo doanh nghiệp
                        'Tỉnh theo user
                        Dim tinhuser As String = IIf(IsNothing(TinhUserLogin.IsTinh) OrElse Not TinhUserLogin.IsTinh, ("tỉnh " + TinhUserLogin.TenTinh), TinhUserLogin.TenTinh)
                        Dim tinhDiaBanTT As String = IIf(IsNothing(DiaBanTT.IsTinh) OrElse Not DiaBanTT.IsTinh, ("tỉnh " + qdtt.Tinh.TenTinh), qdtt.Tinh.TenTinh)
                        Dim tencoquan As String = ""
                        If Not IsNothing(chkTT) Then 'Là thanh tra bộ
                            document.ReplaceText("<<cqbh1>>", "BỘ LAO ĐỘNG - THƯƠNG BINH VÀ XÃ HỘI")
                            document.ReplaceText("<<cqbh2>>", "THANH TRA BỘ")

                            '<<tencoquan>>
                            Select Case qdtt.ChucDanhNguoiKy
                                Case 1, 2
                                    tencoquan = "CHÁNH THANH TRA BỘ LAO ĐỘNG – THƯƠNG BINH VÀ XÃ HỘI"
                                Case 3, 4
                                    tencoquan = "GIÁM ĐỐC SỞ LAO ĐỘNG – THƯƠNG BINH VÀ XÃ HỘI " & tinhuser.ToUpper
                            End Select
                        Else 'Là thanh tra sở
                            document.ReplaceText("<<cqbh1>>", "SỞ LAO ĐỘNG - THƯƠNG BINH VÀ XÃ HỘI " & tinhuser.ToUpper)

                            '<<tencoquan>>
                            Select Case qdtt.ChucDanhNguoiKy
                                Case 1, 2
                                    tencoquan = "CHÁNH THANH TRA SỞ LAO ĐỘNG – THƯƠNG BINH VÀ XÃ HỘI " & tinhuser.ToUpper
                                    document.ReplaceText("<<cqbh2>>", "THANH TRA SỞ")
                                Case 3, 4
                                    tencoquan = "GIÁM ĐỐC SỞ LAO ĐỘNG – THƯƠNG BINH VÀ XÃ HỘI " & tinhuser.ToUpper
                                    document.ReplaceText("<<cqbh2>>", "")
                            End Select
                        End If
                        'Số quyết định
                        document.ReplaceText("<<SoQD>>", soQD.Split("/")(0))
                        ''tinh
                        Dim IsThanhTra As Integer = IIf(Not IsNothing(chkTT) AndAlso chkTT.UserId > 0, 1, 0) '1: thanh tra bộ; 0: thanh tra sở
                        Dim isUser As Integer = Session("IsUser")
                        document.ReplaceText("<<tinh>>", IIf(IsThanhTra = 1 AndAlso isUser = 1, "Hà Nội", TinhUserLogin.TenTinh) + "," + " ngày   tháng   năm " & Now.Year.ToString())
                        '<<quyetdinh>>
                        document.ReplaceText("<<quyetdinh>>", quyetdinh)
                        '<<tencoquan>>
                        document.ReplaceText("<<tencoquan>>", tencoquan)
                        '' Căn cứ luật
                        pParagraph = (From q In document.Paragraphs
                                        Where q.Text.Contains("<<canculuat>>")).FirstOrDefault()
                        If Not IsNothing(qdtt.CanCuLuat) Then
                            Dim arrCanCuLuat() As String = qdtt.CanCuLuat.Split(Str_Symbol_Group)
                            For index As Integer = 0 To arrCanCuLuat.Count - 1
                                '' Thêm dữ liệu vào document
                                Dim pNew As Paragraph = document.InsertParagraph '' Mặc định sẻ insert vào cuối cùng của văn bản
                                pNew.StyleName = pParagraph.StyleName
                                pNew.Append(arrCanCuLuat(index))
                                pParagraph.InsertParagraphBeforeSelf(pNew)
                                '' tìm ra paragraph cuối cùng vào  xóa đi 
                                Dim pNewEnd = document.Paragraphs.LastOrDefault()
                                pNewEnd.Remove(False)
                            Next
                            pParagraph.Remove(False)
                        Else
                            document.ReplaceText("<<canculuat>>", "")
                        End If
                        '' Căn cứ quyết định
                        pParagraph = (From q In document.Paragraphs
                                        Where q.Text.Contains("<<cancuquyetdinh>>")).FirstOrDefault()
                        If Not IsNothing(qdtt.CanCuQuyetDinh) Then
                            Dim arrCanCuQuyetDinh() As String = qdtt.CanCuQuyetDinh.Split(Str_Symbol_Group)
                            For index As Integer = 0 To arrCanCuQuyetDinh.Count - 1
                                '' Thêm dữ liệu vào document
                                Dim pNew As Paragraph = document.InsertParagraph '' Mặc định sẻ insert vào cuối cùng của văn bản
                                pNew.StyleName = pParagraph.StyleName
                                pNew.Append(arrCanCuQuyetDinh(index))
                                pParagraph.InsertParagraphBeforeSelf(pNew)
                                '' tìm ra paragraph cuối cùng vào  xóa đi 
                                Dim pNewEnd = document.Paragraphs.LastOrDefault()
                                pNewEnd.Remove(False)
                            Next
                            pParagraph.Remove(False)
                        Else
                            document.ReplaceText("<<cancuquyetdinh>>", "")
                        End If
                        '<<dieumot>>
                        document.ReplaceText("<<dieumot>>", strThanhTra & " việc chấp hành các quy định của " & qdtt.PhamVi & " tại một số doanh nghiệp trên địa bàn " & tinhDiaBanTT)
                        '<<thanhtra3>>
                        document.ReplaceText("<<thanhtra3>>", strThanhTra.ToLower)
                        '<<thanhtra4>>
                        document.ReplaceText("<<thanhtra4>>", strThanhTra.ToLower)
                        '<<thanhtra5>>
                        document.ReplaceText("<<thanhtra5>>", strThanhTra.ToLower)
                        'Thành viên đoàn
                        pParagraph = (From q In document.Paragraphs
                                                       Where q.Text.Contains("<<thanhviendoan>>")).FirstOrDefault()
                        If Not IsNothing(qdtt.ThanhVienDoan) Then
                            Dim arrThanhVienDoan() As String = qdtt.ThanhVienDoan.Split(Str_Symbol_Group)
                            For index As Integer = 0 To arrThanhVienDoan.Count - 1
                                '' Thêm dữ liệu vào document
                                Dim pNew As Paragraph = document.InsertParagraph '' Mặc định sẻ insert vào cuối cùng của văn bản
                                pNew.StyleName = pParagraph.StyleName
                                pNew.Append(arrThanhVienDoan(index))
                                pParagraph.InsertParagraphBeforeSelf(pNew)
                                '' tìm ra paragraph cuối cùng vào  xóa đi 
                                Dim pNewEnd = document.Paragraphs.LastOrDefault()
                                pNewEnd.Remove(False)
                            Next
                            pParagraph.Remove(False)
                        Else
                            document.ReplaceText("<<thanhviendoan>>", "")
                        End If
                        '<<thanhtra6>>
                        document.ReplaceText("<<thanhtra6>>", strThanhTra.ToLower)
                        '<<dieu31>>
                        document.ReplaceText("<<dieu31>>", strThanhTra & " việc chấp hành các quy định của " & qdtt.PhamVi & " tại doanh nghiệp các nội dung trong đề cương kèm theo Quyết định này.")
                        ''<<dieu32>>
                        document.ReplaceText("<<dieu32>>", "Qua " & strThanhTra.ToLower & " có đánh giá về việc thực hiện " & qdtt.PhamVi & " tại doanh nghiệp, phát hiện những vi phạm và hướng dẫn doanh nghiệp thực hiện theo đúng pháp luật; kiến nghị hoặc thực hiện các biện pháp xử lý những vi phạm pháp luật, đồng thời có kiến nghị sửa đổi bổ sung chính sách")
                        '<<trachnhiemthanhtra>>
                        document.ReplaceText("<<trachnhiemthanhtra>>", IIf(IsNothing(qdtt.TrachNhiemThiHanh), "", qdtt.TrachNhiemThiHanh & "./."))
                        ''Noi nhân
                        pParagraph = (From q In document.Paragraphs
                                                       Where q.Text.Contains("<<noinhan>>")).FirstOrDefault()
                        If Not IsNothing(qdtt.NoiNhan) Then
                            Dim arrNoiNhan() As String = qdtt.NoiNhan.Split(Str_Symbol_Group)
                            For index As Integer = 0 To arrNoiNhan.Count - 1
                                '' Thêm dữ liệu vào document
                                Dim pNew As Paragraph = document.InsertParagraph '' Mặc định sẻ insert vào cuối cùng của văn bản
                                pNew.StyleName = pParagraph.StyleName
                                pNew.Append(arrNoiNhan(index))
                                pParagraph.InsertParagraphBeforeSelf(pNew)
                                '' tìm ra paragraph cuối cùng vào  xóa đi 
                                Dim pNewEnd = document.Paragraphs.LastOrDefault()
                                pNewEnd.Remove(False)
                            Next
                            pParagraph.Remove(False)
                        Else
                            document.ReplaceText("<<noinhan>>", "")
                        End If
                        '' Chức danh
                        Dim chucdanh As String = ""
                        Select Case qdtt.ChucDanhNguoiKy
                            Case 1
                                chucdanh = "CHÁNH THANH TRA"
                            Case 2
                                chucdanh = "KT.CHÁNH THANH TRA" + Environment.NewLine + "PHÓ CHÁNH THANH TRA"
                            Case 3
                                chucdanh = "GIÁM ĐỐC"
                            Case 4
                                chucdanh = "KT.GIÁM ĐỐC" + Environment.NewLine + "PHÓ GIÁM ĐỐC"
                        End Select
                        document.ReplaceText("<<chucdanh>>", chucdanh)

                        ''Người ký
                        document.ReplaceText("<<nguoiky>>", IIf(IsNothing(qdtt.NguoiKyQuyetDinh), "", qdtt.NguoiKyQuyetDinh))

                        'Reset(chặn lặp lại double danh sách doanh nghiệp)
                        hidID.Value = Request.QueryString("SoQuyetDinh")
                        'Dùng cho load dữ liệu doanh nghiệp
                        Dim dn = (From a In data.QuyetDinhTTDoanhNghieps Join c In data.DoanhNghieps On a.DoanhNghiepId Equals c.DoanhNghiepId
                                                                        Where a.QuyetDinhTT.Equals(qdtt.SoQuyetDinh)
                                                                        Order By c.ThoiGianLamViec
                                                                        Select c).ToList()
                        Dim lstDoanhNghiepId = ""
                        Dim lstDoanhNghiep = ""
                        Dim lstDiaChi = ""
                        Dim lstTinh = ""
                        Dim lstHuyen = ""
                        Dim lstThoiGian = ""
                        If dn.Count > 0 Then
                            For i As Integer = 0 To dn.Count - 2
                                lstDoanhNghiepId &= dn(i).DoanhNghiepId & Str_Symbol_Group
                                lstDoanhNghiep &= dn(i).TenDoanhNghiep + Str_Symbol_Group
                                lstDiaChi &= dn(i).TruSoChinh + Str_Symbol_Group
                                lstTinh &= dn(i).TinhId & Str_Symbol_Group
                                lstHuyen &= dn(i).HuyenId & Str_Symbol_Group
                                lstThoiGian &= CType(dn(i).ThoiGianLamViec, Date).ToString("dd/MM/yyyy") + Str_Symbol_Group
                            Next
                            lstDoanhNghiepId &= dn(dn.Count - 1).DoanhNghiepId
                            lstDoanhNghiep &= dn(dn.Count - 1).TenDoanhNghiep
                            lstDiaChi &= dn(dn.Count - 1).TruSoChinh
                            lstTinh &= dn(dn.Count - 1).TinhId
                            lstHuyen &= dn(dn.Count - 1).HuyenId
                            lstThoiGian &= CType(dn(dn.Count - 1).ThoiGianLamViec, Date).ToString("dd/MM/yyyy")
                        End If
                        hidlstDoanhNghiepId.Value = lstDoanhNghiepId
                        hidlstDoanhNghiep.Value = lstDoanhNghiep
                        hidlstDiaChi.Value = lstDiaChi
                        hidlstTinh.Value = lstTinh
                        hidlstHuyen.Value = lstHuyen
                        hidlstThoiGian.Value = lstThoiGian

                        'Dùng cho load dữ liệu theo user tạo số quyết định này
                        Dim us As User = (From a In data.Users Where a.UserName.Equals(qdtt.NguoiTao)).SingleOrDefault
                        Dim userID As Integer = us.UserId
                        Dim iTinhUser As Tinh = (From t In data.Tinhs Join u In data.Users On t.TinhId Equals u.TinhTP Where u.UserId = userID Select t).SingleOrDefault
                        Dim ichkTT = data.uspCheckLoaiThanhTraByUserId(userID).FirstOrDefault
                        Dim iisThanhTra As Integer = IIf(Not IsNothing(ichkTT) AndAlso ichkTT.UserId > 0, 1, 0) '1: thanh tra bộ; 0: thanh tra sở  
                        hidIsThanhTra.Value = iisThanhTra
                        hidTinhUser.Value = iTinhUser.TinhId
                        Session("SoQD") = qdtt.SoQuyetDinh

                        Dim FileName = "QDTT" & Now.ToString("dd_MM_yyyy_hh_mm") & ".docx"
                        document.SaveAs(Server.MapPath("~/Output").ToString & "\" & FileName)
                        ' Request.Url
                        Dim URL As String = "Output/" & FileName
                        Excute_Javascript("Alertbox('Quyết định thanh tra được xuất thành công.');window.location='../../" + URL + "'; ", Me.Page, True)
                    End Using
                End Using
                sc.Complete()
            Catch ex As Exception
                Excute_Javascript("Alertbox('Việc xuất dữ liệu bị lỗi! " & ex.Message & "');", Me.Page, True)
                sc.Dispose()
            End Try
        End Using
    End Sub
    Protected Sub InDSDoanhNghiep()
        Using sc As New TransactionScope
            Try
                Using data As New ThanhTraLaoDongEntities

                    Dim FolderPath = Server.MapPath("~/Template").ToString
                    Dim attributes As FileAttributes = File.GetAttributes(FolderPath & "\DanhSachDoanhNghiep_Template.docx")
                    If (attributes And FileAttributes.ReadOnly) = FileAttributes.ReadOnly Then
                        attributes = attributes And Not FileAttributes.ReadOnly
                        File.SetAttributes(FolderPath & "\DanhSachDoanhNghiep_Template.docx", attributes)
                    End If
                    Using document As DocX = DocX.Load(FolderPath & "\DanhSachDoanhNghiep_Template.docx")
                        ''Get info Quyết định thanh tra
                        Dim soQD = Session("SoQD").ToString
                        Dim qdtt_dn = (From a In data.QuyetDinhThanhTras Join b In data.QuyetDinhTTDoanhNghieps On a.SoQuyetDinh Equals b.QuyetDinhTT
                                       Join c In data.DoanhNghieps On b.DoanhNghiepId Equals c.DoanhNghiepId
                                       Where a.SoQuyetDinh.Equals(soQD) Order By c.ThoiGianLamViec Select c, a.ChucDanhNguoiKy, a.LoaiQuyetDinh, a.NguoiTao).ToList
                        '' Phần Cơ quan ban hành
                        'Xác định thanh tra bộ hay thanh tra sở
                        Dim tencoquan As String = ""
                        Dim iUserId As Integer = Session("userid")
                        Dim chkTT = data.uspCheckLoaiThanhTraByUserId(iUserId).FirstOrDefault
                        Dim TinhUserLogin As Tinh = (From t In data.Tinhs Join u In data.Users On t.TinhId Equals u.TinhTP Where u.UserId = iUserId Select t).SingleOrDefault
                        'Tỉnh nơi nhận theo doanh nghiệp
                        'Tỉnh theo user
                        Dim tinhuser As String = IIf(IsNothing(TinhUserLogin.IsTinh) OrElse Not TinhUserLogin.IsTinh, ("tỉnh " + TinhUserLogin.TenTinh), TinhUserLogin.TenTinh)
                        If Not IsNothing(chkTT) Then 'Là thanh tra bộ
                            document.ReplaceText("<<cqbh1>>", "BỘ LAO ĐỘNG - THƯƠNG BINH VÀ XÃ HỘI")
                            document.ReplaceText("<<cqbh2>>", "THANH TRA BỘ")
                            '<<tencoquan>>
                            If qdtt_dn.Count > 0 Then
                                Select Case qdtt_dn(0).ChucDanhNguoiKy
                                    Case 1, 2
                                        tencoquan = "Chánh thanh tra Bộ Lao động - Thương binh và Xã hội"
                                    Case Else
                                        tencoquan = "Giám đốc Sở Lao động - Thương binh và Xã hội " + tinhuser
                                End Select
                            End If
                        Else 'Là thanh tra sở
                            document.ReplaceText("<<cqbh1>>", "SỞ LAO ĐỘNG - THƯƠNG BINH VÀ XÃ HỘI " & tinhuser.ToUpper)
                            document.ReplaceText("<<cqbh2>>", "THANH TRA SỞ")
                            '<<tencoquan>>
                            If qdtt_dn.Count > 0 Then
                                Select Case qdtt_dn(0).ChucDanhNguoiKy
                                    Case 1, 2
                                        tencoquan = "Chánh thanh tra Sở Lao động - Thương binh và Xã hội"
                                    Case Else
                                        tencoquan = "Giám đốc Sở Lao động - Thương binh và Xã hội " + tinhuser
                                End Select
                            End If
                        End If

                        '<<tieude>>
                        Dim strTieuDe = ""
                        If qdtt_dn(0).LoaiQuyetDinh = 1 Then
                            strTieuDe = "DANH SÁCH CÁC DOANH NGHIỆP ĐƯỢC THANH TRA"
                        Else
                            strTieuDe = "DANH SÁCH CÁC DOANH NGHIỆP ĐƯỢC KIỂM TRA"
                        End If
                        document.ReplaceText("<<tieude>>", strTieuDe)
                        'Số quyết định
                        document.ReplaceText("<<SoQD>>", soQD.Split("/")(0))
                        '<<tencoquan>>
                        document.ReplaceText("<<tencoquan>>", tencoquan)

                        '' Danh sách doanh nghiệp                        
                        If qdtt_dn.Count > 0 Then
                            'Add a Table to this document.
                            Dim tb As Table = document.AddTable(qdtt_dn.Count + 1, 4)

                            'Add content to this Table.
                            tb.Rows(0).Cells(0).Paragraphs.First().Append("TT").Bold().Alignment = Alignment.center
                            tb.Rows(0).Cells(1).Paragraphs.First().Append("Tên doanh nghiệp").Bold().Alignment = Alignment.center
                            tb.Rows(0).Cells(2).Paragraphs.First().Append("Địa chỉ").Bold().Alignment = Alignment.center
                            tb.Rows(0).Cells(3).Paragraphs.First().Append("Thời gian").Bold().Alignment = Alignment.center
                            tb.Rows(0).Cells(0).Width = 5
                            tb.Rows(0).Cells(1).Width = 320
                            tb.Rows(0).Cells(2).Width = 500
                            tb.Rows(0).Cells(3).Width = 80
                            tb.Rows(0).Cells(1).MarginTop = 5
                            tb.Rows(0).Cells(1).MarginRight = 5
                            tb.Rows(0).Cells(1).MarginBottom = 5
                            tb.Rows(0).Cells(1).MarginLeft = 5
                            For index As Integer = 0 To qdtt_dn.Count - 1
                                tb.Rows(index + 1).Cells(0).Paragraphs.First().Append(index + 1)
                                tb.Rows(index + 1).Cells(1).Paragraphs.First().Append(qdtt_dn(index).c.TenDoanhNghiep)
                                Dim tentinh As String = IIf(IsNothing(qdtt_dn(index).c.Tinh.IsTinh) OrElse Not qdtt_dn(index).c.Tinh.IsTinh, ("tỉnh " + qdtt_dn(index).c.Tinh.TenTinh), qdtt_dn(index).c.Tinh.TenTinh)
                                tb.Rows(index + 1).Cells(2).Paragraphs.First().Append(qdtt_dn(index).c.TruSoChinh + ", " + qdtt_dn(index).c.Huyen.TenHuyen.Trim.Replace("Huyện", "huyện").Replace("Thị xã", "thị xã") + ", " + tentinh)
                                tb.Rows(index + 1).Cells(3).Paragraphs.First().Append(CType(qdtt_dn(index).c.ThoiGianLamViec, Date).ToString("dd/MM/yyyy"))
                                tb.Rows(index + 1).Cells(0).Width = 5
                                tb.Rows(index + 1).Cells(1).Width = 320
                                tb.Rows(index + 1).Cells(2).Width = 500
                                tb.Rows(index + 1).Cells(3).Width = 80
                                tb.Rows(index + 1).Cells(1).MarginTop = 5
                                tb.Rows(index + 1).Cells(1).MarginRight = 5
                                tb.Rows(index + 1).Cells(1).MarginBottom = 5
                                tb.Rows(index + 1).Cells(1).MarginLeft = 5
                            Next
                            'Insert the Table into the document.
                            document.InsertTable(tb)
                            document.InsertParagraph()
                            'Ghi chú
                            Dim tbnote As Table = document.AddTable(3, 1)
                            ' tbnote.SetBorder(TableBorderType.Bottom, 1)
                            tbnote.Rows(0).Cells(0).Paragraphs.First().Append("Ghi chú:").UnderlineStyle(UnderlineStyle.singleLine)
                            tbnote.Rows(1).Cells(0).Paragraphs.First().Append("- Thời gian làm việc: Sáng từ 08 giờ 00, chiều từ 13 giờ 30;").Italic()
                            tbnote.Rows(2).Cells(0).Paragraphs.First().Append("- Nếu thời gian làm việc có thay đổi, Trưởng đoàn sẽ thông báo sau.").Italic()
                            tbnote.Rows(0).Cells(0).Width = 825
                            tbnote.Rows(1).Cells(0).Width = 825
                            tbnote.Rows(2).Cells(0).Width = 825
                            tbnote.SetBorder(TableCellBorderType.Top, New Border(Novacode.BorderStyle.Tcbs_none, BorderSize.one, 1, System.Drawing.Color.White))
                            tbnote.SetBorder(TableCellBorderType.Right, New Border(Novacode.BorderStyle.Tcbs_none, BorderSize.one, 1, System.Drawing.Color.White))
                            tbnote.SetBorder(TableCellBorderType.Left, New Border(Novacode.BorderStyle.Tcbs_none, BorderSize.one, 1, System.Drawing.Color.White))
                            tbnote.SetBorder(TableCellBorderType.Bottom, New Border(Novacode.BorderStyle.Tcbs_none, BorderSize.one, 1, System.Drawing.Color.White))
                            tbnote.SetBorder(TableCellBorderType.InsideH, New Border(Novacode.BorderStyle.Tcbs_none, BorderSize.one, 1, System.Drawing.Color.White))
                            tbnote.SetBorder(TableCellBorderType.InsideV, New Border(Novacode.BorderStyle.Tcbs_none, BorderSize.one, 1, System.Drawing.Color.White))
                            document.InsertTable(tbnote)
                        End If

                        'Reset(chặn lặp lại double danh sách doanh nghiệp)
                        hidID.Value = Request.QueryString("SoQuyetDinh")
                        'Dùng cho load dữ liệu doanh nghiệp
                        Dim dn = (From a In data.QuyetDinhTTDoanhNghieps Join c In data.DoanhNghieps On a.DoanhNghiepId Equals c.DoanhNghiepId
                                                                        Where a.QuyetDinhTT.Equals(hidID.Value)
                                                                        Order By c.ThoiGianLamViec
                                                                        Select c).ToList()
                        Dim lstDoanhNghiepId = ""
                        Dim lstDoanhNghiep = ""
                        Dim lstDiaChi = ""
                        Dim lstTinh = ""
                        Dim lstHuyen = ""
                        Dim lstThoiGian = ""
                        If dn.Count > 0 Then
                            For i As Integer = 0 To dn.Count - 2
                                lstDoanhNghiepId &= dn(i).DoanhNghiepId & Str_Symbol_Group
                                lstDoanhNghiep &= dn(i).TenDoanhNghiep + Str_Symbol_Group
                                lstDiaChi &= dn(i).TruSoChinh + Str_Symbol_Group
                                lstTinh &= dn(i).TinhId & Str_Symbol_Group
                                lstHuyen &= dn(i).HuyenId & Str_Symbol_Group
                                lstThoiGian &= CType(dn(i).ThoiGianLamViec, Date).ToString("dd/MM/yyyy") + Str_Symbol_Group
                            Next
                            lstDoanhNghiepId &= dn(dn.Count - 1).DoanhNghiepId
                            lstDoanhNghiep &= dn(dn.Count - 1).TenDoanhNghiep
                            lstDiaChi &= dn(dn.Count - 1).TruSoChinh
                            lstTinh &= dn(dn.Count - 1).TinhId
                            lstHuyen &= dn(dn.Count - 1).HuyenId
                            lstThoiGian &= CType(dn(dn.Count - 1).ThoiGianLamViec, Date).ToString("dd/MM/yyyy")
                        End If
                        hidlstDoanhNghiepId.Value = lstDoanhNghiepId
                        hidlstDoanhNghiep.Value = lstDoanhNghiep
                        hidlstDiaChi.Value = lstDiaChi
                        hidlstTinh.Value = lstTinh
                        hidlstHuyen.Value = lstHuyen
                        hidlstThoiGian.Value = lstThoiGian

                        'Dùng cho load dữ liệu theo user tạo số quyết định này
                        Dim username = qdtt_dn(0).NguoiTao.Trim
                        Dim us As User = (From a In data.Users Where a.UserName.Equals(username)).SingleOrDefault
                        Dim userID As Integer = us.UserId
                        Dim iTinhUser As Tinh = (From t In data.Tinhs Join u In data.Users On t.TinhId Equals u.TinhTP Where u.UserId = userID Select t).SingleOrDefault
                        Dim ichkTT = data.uspCheckLoaiThanhTraByUserId(userID).FirstOrDefault
                        Dim iisThanhTra As Integer = IIf(Not IsNothing(ichkTT) AndAlso ichkTT.UserId > 0, 1, 0) '1: thanh tra bộ; 0: thanh tra sở  
                        hidIsThanhTra.Value = iisThanhTra
                        hidTinhUser.Value = iTinhUser.TinhId
                        Session("SoQD") = hidID.Value

                        Dim FileName = "DSDoanhNghiep" & Now.ToString("dd_MM_yyyy_hh_mm") & ".docx"
                        document.SaveAs(Server.MapPath("~/Output").ToString & "\" & FileName)
                        ' Request.Url
                        Dim URL As String = "Output/" & FileName
                        Excute_Javascript("Alertbox('Danh sách doanh nghiệp được xuất thành công.');window.location='../../" + URL + "'; ", Me.Page, True)
                    End Using
                End Using
                sc.Complete()
            Catch ex As Exception
                Excute_Javascript("Alertbox('Việc xuất dữ liệu bị lỗi! " & ex.Message & "');", Me.Page, True)
                sc.Dispose()
            End Try
        End Using
    End Sub
#End Region

    
End Class
