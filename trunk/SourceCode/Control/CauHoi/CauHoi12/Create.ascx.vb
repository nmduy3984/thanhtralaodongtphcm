Imports System.Data
Imports ThanhTraLaoDongModel
Imports Cls_Common
Imports SecurityService
Partial Class Control_CauHoi12_Create
    Inherits System.Web.UI.UserControl
    Private Shared ReadOnly log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
    Dim strTenCotCauHoi As String = ""
    Dim strKetLuan As String = ""

#Region "Sub and Function "
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            If Not Session("Username") = "" AndAlso Not IsNothing(Session("phieuid")) Then
                Dim script As ScriptManager = ScriptManager.GetCurrent(Me.Page)
                If Not script Is Nothing AndAlso script.IsInAsyncPostBack Then
                    ScriptManager.RegisterStartupScript(Me.Page, Me.Page.GetType, "duyjs", "ajaxJquery()", True)
                    ScriptManager.RegisterStartupScript(Me.Page, Me.Page.GetType, "duyjs1", "ajaxJqueryToolTip()", True)
                Else
                    Me.Page.ClientScript.RegisterStartupScript(Me.Page.GetType(), "duyjs", String.Concat("Sys.Application.add_load(function(){", "ajaxJquery()", "});"), True)
                    Me.Page.ClientScript.RegisterStartupScript(Me.Page.GetType(), "duyjs1", String.Concat("Sys.Application.add_load(function(){", "ajaxJqueryToolTip()", "});"), True)
                End If

                'Lưu session vào các hidden field cho dễ làm việc
                hidPhieuID.Value = IIf(IsNothing(Session("phieuid")), 0, Session("phieuid"))
                hidIsUser.Value = IIf(IsNothing(Session("IsUser")), 0, Session("IsUser"))
                hidModePhieu.Value = IIf(IsNothing(Session("ModePhieu")), 0, Session("ModePhieu"))
                'Neu ModePhieu la xem chi tiet thì ẩn các button
                'Nếu ModePhieu là chỉnh sửa thì xử lý các button
                If hidModePhieu.Value = ModePhieu.Detail Then
                    btnSave.Visible = False
                    btnReset.Visible = False
                    btnKetThuc.Visible = False
                    btnXemKetLuan.Visible = False
                    btnInBienBan.Visible = False
                    btnInKetLuan.Visible = False
                    btnINBBVP.Visible = False
                    btnInPhieuKienNghi.Visible = False
                Else
                    Using data As New ThanhTraLaoDongEntities
                        Dim pn = (From a In data.PhieuNhapHeaders Where a.PhieuID = hidPhieuID.Value Select a).FirstOrDefault
                        Dim kl = (From a In data.KetLuans Where a.PhieuId = hidPhieuID.Value Select a).ToList
                        If Not IsNothing(pn) AndAlso pn.CauHoiDaTraLoi.Split(";").Length >= 13 Then
                            btnKetThuc.Enabled = True
                        End If
                        btnInPhieuKienNghi.Visible = False
                        If pn.LoaiPhieu = False Then
                            btnXemKetLuan.Enabled = False
                            btnInPhieuKienNghi.Visible = True
                            btnInPhieuKienNghi.Enabled = False
                            btnInBienBan.Visible = False
                            btnInKetLuan.Visible = False
                            btnINBBVP.Visible = False
                        End If
                        If kl.Count > 0 AndAlso pn.LoaiPhieu = False Then
                            btnXemKetLuan.Enabled = True
                            btnInPhieuKienNghi.Enabled = True
                        End If

                        If kl.Count > 0 AndAlso pn.LoaiPhieu = True Then
                            btnXemKetLuan.Enabled = True
                            btnInBienBan.Enabled = True
                            btnInKetLuan.Enabled = True
                            btnINBBVP.Enabled = True
                            btnInPhieuKienNghi.Visible = False
                        End If
                    End Using
                End If

                'Load nội dung cauhoi12
                ShowData()

            ElseIf Not Session("Username") = "" AndAlso Session("phieuid") = "" Then
                If Request.Path.Contains("BienBanThanhTra") Then
                    Response.Redirect("../../Page/BienBanThanhTra/List.aspx")
                Else
                    Response.Redirect("../../Page/PhieuKiemTra/List.aspx")
                End If
            Else
                Response.Redirect("../../Login.aspx")
            End If
        End If
    End Sub
    Protected Sub LoadDataYKienDN()
        Using data As New ThanhTraLaoDongEntities
            Dim phieuNhap = (From q In data.PhieuNhapHeaders Where q.PhieuID = hidPhieuID.Value).FirstOrDefault()
            If Not IsNothing(phieuNhap) Then
                If Not IsNothing(phieuNhap.YKienCuaDN) Then
                    Dim Strykien As String = ""
                    For Each item As String In Strings.Split(phieuNhap.YKienCuaDN, Str_Symbol_Group)
                        Strykien = Strykien + item + Environment.NewLine
                    Next
                    txtYKienDN.Text = Strykien
                End If
            End If
        End Using
    End Sub
    Protected Function Save() As Boolean
        Using data As New ThanhTraLaoDongEntities
            Dim q12 As CauHoi12 = (From q In data.CauHoi12 Where q.PhieuId = hidPhieuID.Value).FirstOrDefault()
            Try
                If Not Session("Username") = "" Then
                    Dim iQ121, iQ1241, iQ1242 As Integer
                    iQ121 = GetNumberByFormat(txtQ121.Text.Trim())
                    iQ1241 = GetNumberByFormat(txtQ1241.Text.Trim())
                    iQ1242 = GetNumberByFormat(txtQ1242.Text.Trim())

                    If q12 Is Nothing Then
                        q12 = New CauHoi12
                        q12.PhieuId = hidPhieuID.Value
                        '12. Lao động nữ và bình đẳng giới
                        q12.Q121 = IIf(iQ121 > 0, iQ121, Nothing) 'Số lao động nữ
                        q12.Q1211 = Nothing
                        If chkQ1211.SelectedValue <> "" Then
                            q12.Q1211 = Not (chkQ1211.SelectedValue = 1) 'Bố trí nữ làm công việc nặng nhọc, độc hại, nguy hiểm
                        End If
                        q12.Q1212 = Nothing
                        If chkQ1212.SelectedValue <> "" Then
                            q12.Q1212 = chkQ1212.SelectedValue = 1 'Giảm giờ làm đối với lao động nữ có thai, con bú
                        End If
                        q12.Q1213 = Nothing
                        If chkQ1213.SelectedValue <> "" Then
                            q12.Q1213 = chkQ1213.SelectedValue = 1 'Bố trí nơi vệ sinh phụ nữ
                        End If
                        'Bình đẳng giới
                        q12.Q122 = Nothing
                        If chkQ122.SelectedValue <> "" Then
                            q12.Q122 = Not (chkQ122.SelectedValue = 1) 'Tuyển dụng và trả công phân biệt nam và nữ
                        End If
                        q12.Q123 = Nothing
                        If chkQ123.SelectedValue <> "" Then
                            q12.Q123 = Not (chkQ123.SelectedValue = 1) 'Đào tạo, bổ nhiệm phân biệt nam và nữ
                        End If
                        q12.Q1241 = IIf(iQ1241 > 0, iQ1241, Nothing) 'Tỉ lệ cán bộ nữ đi học 
                        q12.Q1242 = IIf(iQ1242 > 0, iQ1242, Nothing) 'Tỷ lệ cán bộ quản lí là nữ

                        q12.NguoiTao = Session("Username")
                        q12.NgayTao = Date.Now
                        data.CauHoi12.AddObject(q12)
                        'Luu cau hoi da tra loi
                        Dim pn = (From a In data.PhieuNhapHeaders Where a.PhieuID = hidPhieuID.Value Select a).FirstOrDefault
                        pn.CauHoiDaTraLoi = pn.CauHoiDaTraLoi & "12;"
                        data.SaveChanges()
                        'Bật nút kết thúc
                        If Not IsNothing(pn) AndAlso pn.CauHoiDaTraLoi.Split(";").Length >= 13 Then
                            btnKetThuc.Enabled = True
                        End If

                        If hidIsUser.Value = 1 Then
                            Insert_App_Log("Insert  Cauhoi12: PhieuId - " & hidPhieuID.Value, Function_Name.BienBanThanhTra, Audit_Type.Create, Request.ServerVariables("REMOTE_ADDR"), Session("UserName"))
                        ElseIf hidIsUser.Value = 2 Then
                            Insert_App_Log("Insert  Cauhoi12: PhieuId - " & hidPhieuID.Value, Function_Name.PhieuKiemTra, Audit_Type.Create, Request.ServerVariables("REMOTE_ADDR"), Session("UserName"))
                        End If
                    Else
                        '12. Lao động nữ và bình đẳng giới
                        q12.Q121 = IIf(iQ121 > 0, iQ121, Nothing) 'Số lao động nữ
                        q12.Q1211 = Nothing
                        If chkQ1211.SelectedValue <> "" Then
                            q12.Q1211 = Not (chkQ1211.SelectedValue = 1) 'Bố trí nữ làm công việc nặng nhọc, độc hại, nguy hiểm
                        End If
                        q12.Q1212 = Nothing
                        If chkQ1212.SelectedValue <> "" Then
                            q12.Q1212 = chkQ1212.SelectedValue = 1 'Giảm giờ làm đối với lao động nữ có thai, con bú
                        End If
                        q12.Q1213 = Nothing
                        If chkQ1213.SelectedValue <> "" Then
                            q12.Q1213 = chkQ1213.SelectedValue = 1 'Bố trí nơi vệ sinh phụ nữ
                        End If
                        'Bình đẳng giới
                        q12.Q122 = Nothing
                        If chkQ122.SelectedValue <> "" Then
                            q12.Q122 = Not (chkQ122.SelectedValue = 1) 'Tuyển dụng và trả công phân biệt nam và nữ
                        End If
                        q12.Q123 = Nothing
                        If chkQ123.SelectedValue <> "" Then
                            q12.Q123 = Not (chkQ123.SelectedValue = 1) 'Đào tạo, bổ nhiệm phân biệt nam và nữ
                        End If
                        q12.Q1241 = IIf(iQ1241 > 0, iQ1241, Nothing) 'Tỉ lệ cán bộ nữ đi học 
                        q12.Q1242 = IIf(iQ1242 > 0, iQ1242, Nothing) 'Tỷ lệ cán bộ quản lí là nữ
                        'Luu ngay sua, nguoi sua phieu, ý kiến của doanh nghiệp
                        Dim pn = (From a In data.PhieuNhapHeaders Where a.PhieuID = hidPhieuID.Value Select a).FirstOrDefault
                        If Not IsNothing(pn) Then
                            Dim strResult As String = ""
                            For Each item In ReadAllLines(txtYKienDN.Text)
                                If Not String.Equals(item, "") Then
                                    strResult = strResult & item & Str_Symbol_Group
                                End If
                            Next
                            'Nếu có ý kiến thì thực hiện cắt đi ký tự "Str_Symbol_Group" cuối cùng.
                            If strResult.Length > 0 Then
                                pn.YKienCuaDN = strResult.Substring(0, strResult.Length - Str_Symbol_Group.Length)
                            Else
                                pn.YKienCuaDN = Nothing
                            End If
                            pn.NgaySua = Date.Now
                            pn.NguoiSua = Session("Username")
                        End If

                        data.SaveChanges()
                        If hidIsUser.Value = 1 Then
                            Insert_App_Log("Update  Cauhoi12: PhieuId - " & hidPhieuID.Value, Function_Name.BienBanThanhTra, Audit_Type.Edit, Request.ServerVariables("REMOTE_ADDR"), Session("UserName"))
                        ElseIf hidIsUser.Value = 2 Then
                            Insert_App_Log("Update  Cauhoi12: PhieuId - " & hidPhieuID.Value, Function_Name.PhieuKiemTra, Audit_Type.Edit, Request.ServerVariables("REMOTE_ADDR"), Session("UserName"))
                        End If
                    End If
                    Return True

                Else
                    Response.Redirect("../../Login.aspx")
                    Return False
                End If
            Catch ex As Exception
                log4net.Config.XmlConfigurator.Configure()
                log.Error("Error error " & AddTabSpace(1) & Session("Username") & AddTabSpace(1) & "IP:" & GetIPAddress(), ex)
                Return False
            End Try
        End Using
    End Function
    Protected Sub ShowData()
        Using data As New ThanhTraLaoDongEntities
            Try
                'Tiêu đề doanh nghiệp
                Dim DNId As Integer = IIf(IsNothing(Request.QueryString("DNId")), 0, CInt(Request.QueryString("DNId")))
                Dim dn = (From a In data.DoanhNghieps Where a.DoanhNghiepId = DNId).SingleOrDefault
                If Not IsNothing(dn) Then
                    lblTitleCompany.Text = "TÌNH HÌNH THỰC HIỆN PHÁP LUẬT LAO ĐỘNG CỦA " & dn.TenDoanhNghiep.Trim()
                End If

                'Load SoLaoDongNu
                Dim pn = (From q In data.PhieuNhapHeaders Where q.PhieuID = hidPhieuID.Value Select q).FirstOrDefault
                If Not pn Is Nothing Then
                    txtQ121.Text = FormatNumber(pn.SoLaoDongNu)
                End If
                'Load cauhoi12
                Dim p As CauHoi12 = (From q In data.CauHoi12 Where q.PhieuId = hidPhieuID.Value Select q).FirstOrDefault
                If Not p Is Nothing Then
                    txtQ121.Text = IIf(IsNothing(p.Q121) = True, "", p.Q121)
                    If p.Q1211.HasValue Then
                        chkQ1211.SelectedValue = Math.Abs(CInt(Not p.Q1211))
                    Else
                        chkQ1211.ClearSelection()
                    End If
                    If p.Q1212.HasValue Then
                        chkQ1212.SelectedValue = Math.Abs(CInt(p.Q1212))
                    Else
                        chkQ1212.ClearSelection()
                    End If
                    If p.Q1213.HasValue Then
                        chkQ1213.SelectedValue = Math.Abs(CInt(p.Q1213))
                    Else
                        chkQ1213.ClearSelection()
                    End If
                    If p.Q122.HasValue Then
                        chkQ122.SelectedValue = Math.Abs(CInt(Not p.Q122))
                    Else
                        chkQ122.ClearSelection()
                    End If
                    If p.Q123.HasValue Then
                        chkQ123.SelectedValue = Math.Abs(CInt(Not p.Q123))
                    Else
                        chkQ123.ClearSelection()
                    End If

                    txtQ1241.Text = IIf(IsNothing(p.Q1241) = True, "", p.Q1241)
                    txtQ1242.Text = IIf(IsNothing(p.Q1242) = True, "", p.Q1242)

                End If
                'Load nội dung ý kiến của doanh nghiệp
                LoadDataYKienDN()
            Catch ex As Exception

            End Try

        End Using
    End Sub
    Protected Sub ResetControl()
        'txtQ121.Text = ""
        chkQ1211.ClearSelection()
        chkQ1212.ClearSelection()
        chkQ1213.ClearSelection()
        chkQ122.ClearSelection()
        chkQ123.ClearSelection()
        txtQ1241.Text = ""
        txtQ1242.Text = ""

    End Sub
#End Region
#Region "Event for control "
    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Session("phieuid") = hidPhieuID.Value
        Session("IsUser") = hidIsUser.Value
        Session("ModePhieu") = hidModePhieu.Value

        If Save() Then
            Excute_Javascript("Alertbox('Cập nhật dữ liệu thành công.');", Me.Page, True) 'window.location ='../../Page/Users/List.aspx';
        Else
            Excute_Javascript("Alertbox('Cập nhật thất bại.');", Me.Page, True)
        End If
    End Sub

    Protected Sub btnReset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReset.Click
        Using data As New ThanhTraLaoDongEntities
            Dim q12 = (From p In data.CauHoi12 Where p.PhieuId = hidPhieuID.Value).FirstOrDefault
            If Not IsNothing(q12) Then
                ShowData()
            Else
                ResetControl()
            End If
        End Using
    End Sub

    Protected Sub btnKetThuc_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnKetThuc.Click
        Try
            Session("phieuid") = hidPhieuID.Value
            Session("IsUser") = hidIsUser.Value
            Session("ModePhieu") = hidModePhieu.Value
            If Save() Then
                Using data As New ThanhTraLaoDongEntities
                    Dim pn = (From a In data.PhieuNhapHeaders Where a.PhieuID = hidPhieuID.Value Select a).FirstOrDefault
                    If pn.CauHoiDaTraLoi.Split(";").Length >= 12 Then
                        data.ExecuteStoreCommand("delete ketluan where phieuid=" & hidPhieuID.Value & "; delete kiennghidn where phieuid=" & hidPhieuID.Value)
                        'Lưu ngày thực hiện ra kết luận
                        pn.NgayKetThucPhieu = Now()
                        data.SaveChanges()
                        'Kết luận các mục câu hỏi
                        XulyKetLuanCauHoi1()
                        XulyKetLuanCauHoi2()
                        XulyKetLuanCauHoi3()
                        XulyKetLuanCauHoi4()
                        XulyKetLuanCauHoi5()
                        XulyKetLuanCauHoi6()
                        XulyKetLuanCauHoi7()
                        XulyKetLuanCauHoi8()
                        XulyKetLuanCauHoi9()
                        XulyKetLuanCauHoi10()
                        XulyKetLuanCauHoi11()
                        XulyKetLuanCauHoi12()
                        'Tao kiến nghị doanh nghiệp tự động
                        'XulyRaKienNghiTuDong()
                        'Excute_Javascript("Alertbox('');", Me.Page, True)
                        btnInBienBan.Enabled = True
                        btnInKetLuan.Enabled = True
                        btnINBBVP.Enabled = True
                        btnXemKetLuan.Enabled = True
                        btnInPhieuKienNghi.Enabled = True
                    Else
                        Excute_Javascript("AlertboxRedirect('Bạn chưa trả lời đầy đủ các câu hỏi.','ThongTinChung.aspx?DNId=" & Request.QueryString("DNId") & "');", Me.Page, True)
                    End If
                End Using
            Else
                Excute_Javascript("Alertbox('Tạo kết luận thất bại.');", Me.Page, True)
            End If
        Catch ex As Exception
            log4net.Config.XmlConfigurator.Configure()
            log.Error("Error error " & AddTabSpace(1) & Session("Username") & AddTabSpace(1) & "IP:" & GetIPAddress(), ex)
            Excute_Javascript("Alertbox('Lưu kết luận thất bại.');", Me.Page, True)
        End Try

    End Sub
    Protected Sub XulyRaKienNghiTuDong()
        Using data As New ThanhTraLaoDongEntities
            data.uspInsertKNDNTuDong(hidPhieuID.Value, Session("Username"))
            data.SaveChanges()
        End Using
    End Sub
    Protected Sub XulyKetLuanCauHoi1()
        Using data As New ThanhTraLaoDongEntities
            Dim kl As New ThanhTraLaoDongModel.KetLuan
            Dim kn As New ThanhTraLaoDongModel.KienNghiDN
            'Tao moi ket luan dua vao cauhoi1
            Dim q1 = (From a In data.CauHoi1
                        Where a.PhieuId = hidPhieuID.Value).FirstOrDefault
            If Not IsNothing(q1) Then
                '1.1. Khai trình, báo cáo định kỳ về tuyển dụng, sử dụng lao động với cơ quan quản lý nhà nước về lao động địa phương
                If Not IsNothing(q1.Q11) Then
                    kl = New ThanhTraLaoDongModel.KetLuan
                    If Not q1.Q11 Then
                        'Xử lý kết luận
                        kl.NDKetLuan = "Doanh nghiệp chưa thực hiện báo cáo định kỳ về tình hình tuyển dụng, nhu cầu sử dụng lao động với cơ quan quản lý nhà nước về lao động địa phương;"
                        kl.IsViPham = TypeViPham.ViPham
                        'Xuất kiến nghị
                        kn = New ThanhTraLaoDongModel.KienNghiDN
                        kn.NDKienNghi = "Thực hiện báo cáo định kỳ sáu tháng và hằng năm về tình hình tuyển dụng, nhu cầu sử dụng lao động với cơ quan quản lý nhà nước về lao động địa phương theo"
                        kn.TrichDanId = 1
                        kn.TenBangCauHoi = "Cauhoi1"
                        kn.PhieuId = hidPhieuID.Value
                        data.KienNghiDNs.AddObject(kn)
                    Else
                        kl.NDKetLuan = "Đã thực hiện báo cáo định kỳ về tình hình tuyển dụng, nhu cầu sử dụng lao động với cơ quan quản lý nhà nước về lao động địa phương;"
                        kl.IsViPham = TypeViPham.KhongViPham
                    End If
                    'Xuất kết luận
                    kl.TenCotCauHoi = "Q11"
                    kl.TenBangCauHoi = "CauHoi1"
                    kl.PhieuId = hidPhieuID.Value
                    data.KetLuans.AddObject(kl)
                End If
                '1.2. Báo cáo định kỳ về công tác an toàn vệ sinh lao động với cơ quan quản lý nhà nước về lao động địa phương
                If Not IsNothing(q1.Q12) Then
                    kl = New ThanhTraLaoDongModel.KetLuan
                    If Not q1.Q12 Then
                        'Xử lý kết luận
                        kl.NDKetLuan = "Chưa thực hiện báo cáo định kỳ về công tác an toàn vệ sinh lao động với cơ quan quản lý nhà nước về lao động địa phương;"
                        kl.IsViPham = TypeViPham.ViPham
                        'Xuất kiến nghị
                        kn = New ThanhTraLaoDongModel.KienNghiDN
                        kn.NDKienNghi = "Thực hiện báo cáo định kỳ sáu tháng và hằng năm về công tác an toàn vệ sinh lao động với cơ quan quản lý nhà nước về lao động địa phương theo"
                        kn.TrichDanId = 2
                        kn.TenBangCauHoi = "Cauhoi1"
                        kn.PhieuId = hidPhieuID.Value
                        data.KienNghiDNs.AddObject(kn)
                    Else
                        kl.NDKetLuan = "Đã thực hiện báo cáo công tác an toàn vệ sinh lao động với cơ quan quản lý nhà nước về lao động địa phương;"
                        kl.IsViPham = TypeViPham.KhongViPham
                    End If
                    'Xuất kết luận
                    kl.TenCotCauHoi = "Q12"
                    kl.TenBangCauHoi = "CauHoi1"
                    kl.PhieuId = hidPhieuID.Value
                    data.KetLuans.AddObject(kl)
                End If
                '1.3. Báo cáo định kỳ về tai nạn lao động với cơ quan quản lý nhà nước về lao động địa phương
                If Not IsNothing(q1.Q13) Then
                    kl = New ThanhTraLaoDongModel.KetLuan
                    If Not q1.Q13 Then
                        'Xử lý kết luận
                        kl.NDKetLuan = "Chưa thực hiện báo cáo định kỳ tình hình tai nạn lao động với cơ quan quản lý nhà nước về lao động địa phương;"
                        kl.IsViPham = TypeViPham.ViPham
                        'Xuất kiến nghị
                        kn = New ThanhTraLaoDongModel.KienNghiDN
                        kn.NDKienNghi = "Thực hiện báo cáo định kỳ sáu tháng và hằng năm về tình hình tai nạn lao động với cơ quan quản lý nhà nước về lao động địa phương theo"
                        kn.TrichDanId = 3
                        kn.TenBangCauHoi = "Cauhoi1"
                        kn.PhieuId = hidPhieuID.Value
                        data.KienNghiDNs.AddObject(kn)
                    Else
                        kl.NDKetLuan = "Đã thực hiện báo cáo tình hình tai nạn lao động với cơ quan quản lý nhà nước về lao động địa phương;"
                        kl.IsViPham = TypeViPham.KhongViPham
                    End If
                    'Xuất kết luận
                    kl.TenCotCauHoi = "Q13"
                    kl.TenBangCauHoi = "CauHoi1"
                    kl.PhieuId = hidPhieuID.Value
                    data.KetLuans.AddObject(kl)
                End If
            End If
            data.SaveChanges()
        End Using
    End Sub
    Protected Sub XulyKetLuanCauHoi2()
        Using data As New ThanhTraLaoDongEntities
            Dim kl As New ThanhTraLaoDongModel.KetLuan
            Dim kn As New ThanhTraLaoDongModel.KienNghiDN
            'Tao moi ket luan dua vao cauhoi2
            Dim q2 = (From a In data.CauHoi2
                        Where a.PhieuId = hidPhieuID.Value).FirstOrDefault
            If Not IsNothing(q2) Then
                '2.1. ký hợp đồng đúng loại
                If Not IsNothing(q2.Q21) Then
                    kl = New ThanhTraLaoDongModel.KetLuan
                    Dim sumLDDaKy As Integer = 0
                    Dim KhongKyHopDong As String = ""
                    Dim arrKL As New ArrayList
                    If q2.Q211 > 0 Then
                        arrKL.Add("HĐLĐ không xác định thời hạn: " & String.Format(info, "{0:n0}", q2.Q211) & " (người);")
                        sumLDDaKy += q2.Q211
                    End If
                    If q2.Q212 > 0 Then
                        arrKL.Add("HĐLĐ xác định thời hạn từ 12 tháng đến 36 tháng: " & String.Format(info, "{0:n0}", q2.Q212) & " (người);")
                        sumLDDaKy += q2.Q212
                    End If
                    If q2.Q213 > 0 Then
                        arrKL.Add("HĐLĐ xác định thời hạn từ 3 tháng đến dưới 12 tháng: " & String.Format(info, "{0:n0}", q2.Q213) & " (người);")
                        sumLDDaKy += q2.Q213
                    End If
                    If q2.Q214 > 0 Then
                        arrKL.Add("HĐLĐ mùa vụ dưới 3 tháng: " & String.Format(info, "{0:n0}", q2.Q214) & " (người);")
                        sumLDDaKy += q2.Q214
                    End If
                    If q2.Q215 > 0 Then
                        arrKL.Add("Hợp đồng khoán gọn theo vụ việc: " & String.Format(info, "{0:n0}", q2.Q215) & " (người);")
                        sumLDDaKy += q2.Q215
                    End If
                    If q2.Q216 > 0 Then
                        arrKL.Add("Hợp đồng học nghề, thử việc: " & String.Format(info, "{0:n0}", q2.Q216) & " (người);")
                        sumLDDaKy += q2.Q216
                    End If
                    If q2.Q219 > 0 Then
                        arrKL.Add("Hợp đồng thuê lại lao động của đơn vị khác: " & String.Format(info, "{0:n0}", q2.Q219) & " (người);")
                        sumLDDaKy += q2.Q219
                    End If
                    If q2.Q2110 > 0 Then
                        KhongKyHopDong = " (" & String.Format(info, "{0:n0}", q2.Q2110) & " người không phải ký hợp đồng lao động)"
                    End If
                    If Not q2.Q21 Then
                        Dim sumLDChuaKy As Integer = 0
                        sumLDChuaKy += q2.Q218 - (IIf(IsNothing(q2.Q217), 0, q2.Q217) + IIf(IsNothing(q2.Q2110), 0, q2.Q2110))
                        kl.NDKetLuan = "Tổng số người đang làm việc tại doanh nghiệp là " & String.Format(info, "{0:n0}", q2.Q218) & KhongKyHopDong & ". Doanh nghiệp đã ký hợp đồng với " & String.Format(info, "{0:n0}", sumLDChuaKy) & " người, trong đó: "
                    Else
                        kl.NDKetLuan = "Tổng số người đang làm việc tại doanh nghiệp là " & String.Format(info, "{0:n0}", q2.Q218) & KhongKyHopDong & ". Doanh nghiệp đã ký hợp đồng đúng loại với " & String.Format(info, "{0:n0}", sumLDDaKy) & " người, trong đó: "
                    End If
                    kl.TenCotCauHoi = "Q21"
                    kl.NDKetLuan += "#" & String.Join("#", TryCast(arrKL.ToArray(GetType(String)), String()))
                    kl.IsViPham = TypeViPham.KhongViPham
                    kl.TenBangCauHoi = "CauHoi2"
                    kl.PhieuId = hidPhieuID.Value
                    data.KetLuans.AddObject(kl)
                End If
                ''Chưa ký hợp đồng lao động
                If Not IsNothing(q2.Q217) AndAlso q2.Q217 > 0 Then
                    'Xuất kết luận
                    kl = New ThanhTraLaoDongModel.KetLuan
                    kl.NDKetLuan = "Chưa ký hợp đồng lao động đối với " & String.Format(info, "{0:n0}", q2.Q217) & " (người);"
                    kl.TenCotCauHoi = "Q217"
                    kl.IsViPham = TypeViPham.ViPham
                    kl.TenBangCauHoi = "CauHoi2"
                    kl.PhieuId = hidPhieuID.Value
                    data.KetLuans.AddObject(kl)
                    'Xuất kiến nghị
                    kn = New ThanhTraLaoDongModel.KienNghiDN
                    kn.NDKienNghi = "Thực hiện ký hợp đồng lao động với " & String.Format(info, "{0:n0}", q2.Q217) & " (người) chưa được ký hợp đồng lao động theo"
                    kn.TrichDanId = 4
                    kn.TenBangCauHoi = "Cauhoi2"
                    kn.PhieuId = hidPhieuID.Value
                    data.KienNghiDNs.AddObject(kn)

                End If

                '2.2. Ghi hợp đồng lao động cụ thể về
                If Not IsNothing(q2.Q221) AndAlso q2.Q221 AndAlso Not IsNothing(q2.Q222) AndAlso q2.Q222 AndAlso Not IsNothing(q2.Q223) AndAlso q2.Q223 AndAlso Not IsNothing(q2.Q224) AndAlso q2.Q224 AndAlso Not IsNothing(q2.Q225) AndAlso q2.Q225 AndAlso Not IsNothing(q2.Q226) AndAlso q2.Q226 Then
                    kl.NDKetLuan = "Đã ghi hợp đồng lao động cụ thể theo quy định;"
                    kl.IsViPham = TypeViPham.KhongXet
                    kl.TenBangCauHoi = "CauHoi2"
                    kl.TenCotCauHoi = "Q221;Q222;Q223;Q224;Q225;Q226;Q227;Q228;Q229;Q2210;Q2211"
                    kl.PhieuId = hidPhieuID.Value
                    data.KetLuans.AddObject(kl)
                ElseIf Not IsNothing(q2.Q221) Or Not IsNothing(q2.Q222) Or Not IsNothing(q2.Q223) Or Not IsNothing(q2.Q224) Or Not IsNothing(q2.Q225) Then
                    kl = New ThanhTraLaoDongModel.KetLuan
                    strTenCotCauHoi = ""
                    kl.NDKetLuan = "Thỏa thuận và ghi hợp đồng lao động chưa cụ thể về: "
                    Dim arrKL As New ArrayList
                    If Not IsNothing(q2.Q221) Then
                        If Not q2.Q221 Then
                            arrKL.Add("chức danh nghề, công việc độc hại, nguy hiểm")
                            strTenCotCauHoi = strTenCotCauHoi & "Q221;"
                        End If
                    End If
                    If Not IsNothing(q2.Q222) Then
                        If Not q2.Q222 Then
                            arrKL.Add("công việc và địa điểm làm việc")
                            strTenCotCauHoi = strTenCotCauHoi & "Q222;"
                        End If
                    End If
                    If Not IsNothing(q2.Q223) Then
                        If Not q2.Q223 Then
                            arrKL.Add("thời hạn của hợp đồng")
                            strTenCotCauHoi = strTenCotCauHoi & "Q223;"
                        End If
                    End If
                    If Not IsNothing(q2.Q224) Then
                        If Not q2.Q224 Then
                            arrKL.Add("mức lương")
                            strTenCotCauHoi = strTenCotCauHoi & "Q224;"
                        End If
                    End If
                    If Not IsNothing(q2.Q225) Then
                        If Not q2.Q225 Then
                            arrKL.Add("chế độ nâng lương")
                            strTenCotCauHoi = strTenCotCauHoi & "Q225;"
                        End If
                    End If
                    If Not IsNothing(q2.Q226) Then
                        If Not q2.Q226 Then
                            arrKL.Add("thời giờ làm việc, nghỉ ngơi")
                            strTenCotCauHoi = strTenCotCauHoi & "Q226;"
                        End If
                    End If
                    If Not IsNothing(q2.Q227) Then
                        If Not q2.Q227 Then
                            arrKL.Add("phương tiện bảo vệ cá nhân")
                            strTenCotCauHoi = strTenCotCauHoi & "Q227;"
                        End If
                    End If
                    If Not IsNothing(q2.Q228) Then
                        If Not q2.Q228 Then
                            arrKL.Add("chế độ bảo hiểm xã hội, bảo hiểm y tế")
                            strTenCotCauHoi = strTenCotCauHoi & "Q228;"
                        End If
                    End If
                    If Not IsNothing(q2.Q229) Then
                        If Not q2.Q229 Then
                            arrKL.Add("đào tạo nâng cao trình độ, tay nghề")
                            strTenCotCauHoi = strTenCotCauHoi & "Q229;"
                        End If
                    End If
                    If Not IsNothing(q2.Q2210) Then
                        If Not q2.Q2210 Then
                            arrKL.Add("bí mật công nghệ, bí mật kinh doanh")
                            strTenCotCauHoi = strTenCotCauHoi & "Q2210;"
                        End If
                    End If
                    If arrKL.Count > 0 Then
                        'Xuất kết luận
                        kl.NDKetLuan = kl.NDKetLuan & String.Join("; ", TryCast(arrKL.ToArray(GetType(String)), String())) + ";"
                        kl.IsViPham = TypeViPham.ViPham
                        kl.TenBangCauHoi = "CauHoi2"
                        kl.TenCotCauHoi = strTenCotCauHoi
                        kl.PhieuId = hidPhieuID.Value
                        data.KetLuans.AddObject(kl)
                        'Xuất kiến nghị
                        kn = New ThanhTraLaoDongModel.KienNghiDN
                        kn.NDKienNghi = "Thỏa thuận và ghi hợp đồng lao động cụ thể về " & String.Join(", ", TryCast(arrKL.ToArray(GetType(String)), String())) & " theo"
                        kn.TrichDanId = 5
                        kn.TenBangCauHoi = "Cauhoi2"
                        kn.PhieuId = hidPhieuID.Value
                        data.KienNghiDNs.AddObject(kn)

                    End If
                End If

                'Thỏa thuận trái luật
                If Not IsNothing(q2.Q2211) Then
                    If Not q2.Q2211 Then
                        'Khai báo kết luận
                        kl = New ThanhTraLaoDongModel.KetLuan
                        kl.NDKetLuan = "Hợp đồng lao động có nội dung thỏa thuận trái luật"
                        'Khai báo kiến nghị
                        kn = New ThanhTraLaoDongModel.KienNghiDN
                        kn.NDKienNghi = "Hủy bỏ nội dung thỏa thuận trái luật"
                        If Not String.IsNullOrEmpty(q2.Q22111) Then
                            'Xử lý kết luận
                            kl.NDKetLuan = kl.NDKetLuan & ": " & q2.Q22111 & ";"
                            'Xử lý kiến nghị
                            kn.NDKienNghi += " trong hợp đồng lao động (" & q2.Q22111 & ") theo"
                        Else
                            'Xử lý kết luận
                            kl.NDKetLuan = kl.NDKetLuan & ";"
                            'Xử lý kiến nghị
                            kn.NDKienNghi += " theo"
                        End If
                        'Xuất kết luận
                        kl.IsViPham = TypeViPham.ViPham
                        kl.TenBangCauHoi = "CauHoi2"
                        kl.TenCotCauHoi = "Q2211"
                        kl.PhieuId = hidPhieuID.Value
                        data.KetLuans.AddObject(kl)
                        'Xuất kiến nghị
                        kn.TrichDanId = 6
                        kn.TenBangCauHoi = "Cauhoi2"
                        kn.PhieuId = hidPhieuID.Value
                        data.KienNghiDNs.AddObject(kn)

                    End If
                End If
                '2.3. Học nghề và thử việc
                If Not IsNothing(q2.Q235) AndAlso q2.Q235 > 0 AndAlso Not IsNothing(q2.Q231) AndAlso q2.Q231 AndAlso Not IsNothing(q2.Q233) AndAlso q2.Q233 Then
                    kl = New ThanhTraLaoDongModel.KetLuan
                    'Năm [hiện thời] doanh nghiệp đã tuyển dụng + [số lao động mới tuyển] + không thu phí tuyển dụng, học nghề để làm việc cho doanh nghiệp; không giữ tiền đặt cọc hoặc bản gốc văn bằng của người lao động
                    kl.NDKetLuan = "Năm " & Now.Year.ToString & " doanh nghiệp đã tuyển dụng " & String.Format(info, "{0:n0}", q2.Q235) & " lao động không thu phí tuyển dụng, học nghề để làm việc cho doanh nghiệp; không giữ tiền đặt cọc hoặc bản gốc văn bằng của người lao động;"
                    kl.IsViPham = TypeViPham.KhongViPham
                    kl.TenBangCauHoi = "CauHoi2"
                    kl.TenCotCauHoi = "Q235;Q231;Q233"
                    kl.PhieuId = hidPhieuID.Value
                    data.KetLuans.AddObject(kl)
                ElseIf Not IsNothing(q2.Q235) AndAlso q2.Q235 > 0 AndAlso Not IsNothing(q2.Q231) AndAlso q2.Q231 AndAlso Not IsNothing(q2.Q233) AndAlso Not q2.Q233 Then
                    kl = New ThanhTraLaoDongModel.KetLuan
                    kl.NDKetLuan = "Năm " & Now.Year.ToString & " doanh nghiệp đã tuyển dụng " & String.Format(info, "{0:n0}", q2.Q235) & " lao động không thu phí tuyển dụng, học nghề để làm việc cho doanh nghiệp;"
                    kl.IsViPham = TypeViPham.KhongViPham
                    kl.TenBangCauHoi = "CauHoi2"
                    kl.TenCotCauHoi = "Q235;Q231;Q233"
                    kl.PhieuId = hidPhieuID.Value
                    data.KetLuans.AddObject(kl)
                ElseIf Not IsNothing(q2.Q235) AndAlso q2.Q235 > 0 AndAlso Not IsNothing(q2.Q231) AndAlso Not q2.Q231 AndAlso Not IsNothing(q2.Q233) AndAlso q2.Q233 Then
                    kl = New ThanhTraLaoDongModel.KetLuan
                    kl.NDKetLuan = "Không giữ tiền đặt cọc hoặc bản gốc văn bằng của người lao động;"
                    kl.IsViPham = TypeViPham.KhongViPham
                    kl.TenBangCauHoi = "CauHoi2"
                    kl.TenCotCauHoi = "Q235;Q231;Q233"
                    kl.PhieuId = hidPhieuID.Value
                    data.KetLuans.AddObject(kl)
                End If

                'TH có cả hai: Kế hoạch đào tạo, nâng cao trình độ, kỹ năng nghề; Thực hiện ký kết hợp đồng đào tạo nghề (nếu có)
                If Not IsNothing(q2.Q236) AndAlso q2.Q236 AndAlso Not IsNothing(q2.Q237) AndAlso q2.Q237 Then
                    kl = New ThanhTraLaoDongModel.KetLuan
                    kl.NDKetLuan = "Đã xây dựng kế hoạch đào tạo, nâng cao trình độ, kỹ năng nghề hằng năm; đã thực hiện ký kết hợp đồng đào tạo nghề đối với người lao động;"
                    kl.IsViPham = TypeViPham.KhongViPham
                    kl.TenBangCauHoi = "CauHoi2"
                    kl.TenCotCauHoi = "Q236;Q237"
                    kl.PhieuId = hidPhieuID.Value
                    data.KetLuans.AddObject(kl)
                ElseIf Not IsNothing(q2.Q236) AndAlso q2.Q236 AndAlso Not IsNothing(q2.Q237) AndAlso Not q2.Q237 Then 'Kế hoạch đào tạo, nâng cao trình độ, kỹ năng nghề --> có; Thực hiện ký kết hợp đồng đào tạo nghề (nếu có) -->không
                    kl = New ThanhTraLaoDongModel.KetLuan
                    kl.NDKetLuan = "Đã xây dựng kế hoạch đào tạo, nâng cao trình độ, kỹ năng nghề hằng năm đối với người lao động;"
                    kl.IsViPham = TypeViPham.KhongViPham
                    kl.TenBangCauHoi = "CauHoi2"
                    kl.TenCotCauHoi = "Q236"
                    kl.PhieuId = hidPhieuID.Value
                    data.KetLuans.AddObject(kl)
                ElseIf Not IsNothing(q2.Q236) AndAlso Not q2.Q236 AndAlso Not IsNothing(q2.Q237) AndAlso q2.Q237 Then 'Kế hoạch đào tạo, nâng cao trình độ, kỹ năng nghề --> không; Thực hiện ký kết hợp đồng đào tạo nghề (nếu có) -->có
                    kl = New ThanhTraLaoDongModel.KetLuan
                    kl.NDKetLuan = "Đã thực hiện ký kết hợp đồng đào tạo nghề đối với người lao động;"
                    kl.IsViPham = TypeViPham.KhongViPham
                    kl.TenBangCauHoi = "CauHoi2"
                    kl.TenCotCauHoi = "Q237"
                    kl.PhieuId = hidPhieuID.Value
                    data.KetLuans.AddObject(kl)
                End If
                'TH không cả hai: Kế hoạch đào tạo, nâng cao trình độ, kỹ năng nghề; Thực hiện ký kết hợp đồng đào tạo nghề (nếu có)
                If Not IsNothing(q2.Q236) AndAlso Not q2.Q236 AndAlso Not IsNothing(q2.Q237) AndAlso Not q2.Q237 Then
                    kl = New ThanhTraLaoDongModel.KetLuan
                    kl.NDKetLuan = "Chưa xây dựng kế hoạch đào tạo, nâng cao trình độ, kỹ năng nghề hằng năm; chưa thực hiện ký kết hợp đồng đào tạo nghề đối với người lao động;"
                    kl.IsViPham = TypeViPham.ViPham
                    kl.TenBangCauHoi = "CauHoi2"
                    kl.TenCotCauHoi = "Q236;Q237"
                    kl.PhieuId = hidPhieuID.Value
                    data.KetLuans.AddObject(kl)
                    'Xuất kiến nghị
                    kn = New ThanhTraLaoDongModel.KienNghiDN
                    kn.NDKienNghi = "Xây dựng kế hoạch đào tạo, nâng cao trình độ, kỹ năng nghề hằng năm và thực hiện ký kết hợp đồng đào tạo nghề đối với người lao động theo"
                    kn.TrichDanId = 64
                    kn.TenBangCauHoi = "Cauhoi2"
                    kn.PhieuId = hidPhieuID.Value
                    data.KienNghiDNs.AddObject(kn)
                ElseIf Not IsNothing(q2.Q236) AndAlso q2.Q236 AndAlso Not IsNothing(q2.Q237) AndAlso Not q2.Q237 Then 'Kế hoạch đào tạo, nâng cao trình độ, kỹ năng nghề --> có; Thực hiện ký kết hợp đồng đào tạo nghề (nếu có) -->không
                    kl = New ThanhTraLaoDongModel.KetLuan
                    kl.NDKetLuan = "Chưa thực hiện ký kết hợp đồng đào tạo nghề theo quy định;"
                    kl.IsViPham = TypeViPham.ViPham
                    kl.TenBangCauHoi = "CauHoi2"
                    kl.TenCotCauHoi = "Q237"
                    kl.PhieuId = hidPhieuID.Value
                    data.KetLuans.AddObject(kl)
                    'Xuất kiến nghị
                    kn = New ThanhTraLaoDongModel.KienNghiDN
                    kn.NDKienNghi = "Thực hiện ký kết hợp đồng đào tạo nghề đối với người lao động theo"
                    kn.TrichDanId = 66
                    kn.TenBangCauHoi = "Cauhoi2"
                    kn.PhieuId = hidPhieuID.Value
                    data.KienNghiDNs.AddObject(kn)
                ElseIf Not IsNothing(q2.Q236) AndAlso Not q2.Q236 AndAlso Not IsNothing(q2.Q237) AndAlso q2.Q237 Then 'Kế hoạch đào tạo, nâng cao trình độ, kỹ năng nghề --> không; Thực hiện ký kết hợp đồng đào tạo nghề (nếu có) -->có
                    kl = New ThanhTraLaoDongModel.KetLuan
                    kl.NDKetLuan = "Chưa xây dựng kế hoạch đào tạo, nâng cao trình độ, kỹ năng nghề hằng năm;"
                    kl.IsViPham = TypeViPham.ViPham
                    kl.TenBangCauHoi = "CauHoi2"
                    kl.TenCotCauHoi = "Q236"
                    kl.PhieuId = hidPhieuID.Value
                    data.KetLuans.AddObject(kl)
                    'Xuất kiến nghị
                    kn = New ThanhTraLaoDongModel.KienNghiDN
                    kn.NDKienNghi = "Xây dựng kế hoạch đào tạo, nâng cao trình độ, kỹ năng nghề hằng năm cho người lao động theo"
                    kn.TrichDanId = 65
                    kn.TenBangCauHoi = "Cauhoi2"
                    kn.PhieuId = hidPhieuID.Value
                    data.KienNghiDNs.AddObject(kn)
                End If
                If Not (IsNothing(q2.Q231) Or IsNothing(q2.Q2321) Or IsNothing(q2.Q2322) Or IsNothing(q2.Q2323)) AndAlso q2.Q231 And q2.Q2321 And q2.Q2322 And q2.Q2323 Then
                    kl = New ThanhTraLaoDongModel.KetLuan
                    kl.NDKetLuan = "Áp dụng thời gian thử việc đúng quy định;"
                    kl.IsViPham = TypeViPham.KhongViPham
                    kl.TenBangCauHoi = "CauHoi2"
                    kl.TenCotCauHoi = "Q231;Q2321;Q2322;Q2323"
                    kl.PhieuId = hidPhieuID.Value
                    data.KetLuans.AddObject(kl)
                Else
                    If Not (IsNothing(q2.Q231) OrElse q2.Q231) Then
                        'Xuất kết luận
                        kl = New ThanhTraLaoDongModel.KetLuan
                        kl.NDKetLuan = "Áp dụng thu phí tuyển dụng, học nghề để làm việc cho doanh nghiệp;"
                        kl.IsViPham = TypeViPham.ViPham
                        kl.TenBangCauHoi = "CauHoi2"
                        kl.TenCotCauHoi = "Q231"
                        kl.PhieuId = hidPhieuID.Value
                        data.KetLuans.AddObject(kl)
                        'Xuất kiến nghị
                        kn = New ThanhTraLaoDongModel.KienNghiDN
                        kn.NDKienNghi = "Hủy bỏ quy định thu phí tuyển dụng, học nghề đối với người lao động tuyển vào làm việc cho doanh nghiệp theo"
                        kn.TrichDanId = 7
                        kn.TenBangCauHoi = "Cauhoi2"
                        kn.PhieuId = hidPhieuID.Value
                        data.KienNghiDNs.AddObject(kn)
                    End If
                    Dim arrKL As New ArrayList
                    kl = New ThanhTraLaoDongModel.KetLuan
                    kl.NDKetLuan = "Áp dụng thời gian thử việc "
                    If Not (IsNothing(q2.Q2321) OrElse q2.Q2321) Then
                        arrKL.Add("quá 60 ngày đối với lao động có chức danh nghề cần trình độ từ cao đẳng trở lên")
                        kl.TenCotCauHoi += "Q2321;"
                    End If
                    If Not (IsNothing(q2.Q2322) OrElse q2.Q2322) Then
                        arrKL.Add("quá 30 ngày đối với lao động có chức danh nghề cần trình độ trung cấp")
                        kl.TenCotCauHoi += "Q2322;"
                    End If
                    If Not (IsNothing(q2.Q2323) OrElse q2.Q2323) Then
                        arrKL.Add("quá 6 ngày đối với lao động khác")
                        kl.TenCotCauHoi += "Q2323;"
                    End If
                    If arrKL.Count > 0 Then
                        'Xuất kết luận
                        kl.NDKetLuan += String.Join("; ", TryCast(arrKL.ToArray(GetType(String)), String())) & ";"
                        kl.IsViPham = TypeViPham.ViPham
                        kl.TenBangCauHoi = "CauHoi2"
                        kl.PhieuId = hidPhieuID.Value
                        data.KetLuans.AddObject(kl)
                        'Xuất kiến nghị
                        kn = New ThanhTraLaoDongModel.KienNghiDN
                        kn.NDKienNghi = "Hủy bỏ quy định kéo dài thời gian thử việc trái luật (" & String.Join(", ", TryCast(arrKL.ToArray(GetType(String)), String())) & "); Doanh nghiệp phải áp dụng thời gian thử việc theo"
                        kn.TrichDanId = 8
                        kn.TenBangCauHoi = "Cauhoi2"
                        kn.PhieuId = hidPhieuID.Value
                        data.KienNghiDNs.AddObject(kn)
                    End If
                    If Not (IsNothing(q2.Q233) OrElse q2.Q233) Then
                        'Khai báo kết luận
                        kl = New ThanhTraLaoDongModel.KetLuan
                        kl.NDKetLuan = "Giữ bản gốc văn bằng hoặc tiền đặt cọc của người lao động là trái quy định"
                        'Khai báo kiến nghị
                        kn = New ThanhTraLaoDongModel.KienNghiDN
                        kn.NDKienNghi = "Hủy bỏ quy định giữ bản gốc văn bằng hoặc tiền đặt cọc của người lao động"
                        If Not IsNothing(q2.Q2331) Then
                            'Xử lý kết luận
                            kl.NDKetLuan = kl.NDKetLuan & " đối với " & q2.Q2331 & " trường hợp;"
                            'Xử lý kiến nghị
                            kn.NDKienNghi += ", đồng thời trả lại tiền đặt cọc hoặc văn bằng gốc cho " & q2.Q2331 & " trường hợp theo"
                        Else
                            'Xử lý kết luận
                            kl.NDKetLuan = kl.NDKetLuan & ";"
                            'Xử lý kiến nghị
                            kn.NDKienNghi += " theo"
                        End If
                        'Xuất kết luận
                        kl.IsViPham = TypeViPham.ViPham
                        kl.TenBangCauHoi = "CauHoi2"
                        kl.TenCotCauHoi = "Q233"
                        kl.PhieuId = hidPhieuID.Value
                        data.KetLuans.AddObject(kl)
                        'Xuất kiến nghị
                        kn.TrichDanId = 57
                        kn.TenBangCauHoi = "Cauhoi2"
                        kn.PhieuId = hidPhieuID.Value
                        data.KienNghiDNs.AddObject(kn)
                    End If
                End If

                '2.4. Mất việc làm
                If Not IsNothing(q2.Q241) Then
                    kl = New ThanhTraLaoDongModel.KetLuan
                    If q2.Q241 >= 2 And q2.Q242 Then
                        kl.NDKetLuan = "Trong phạm vi thời gian thanh tra doanh nghiệp có " & String.Format(info, "{0:n0}", q2.Q241) & " lao động bị mất việc làm; đã thực hiện báo cáo cơ quan quản lý nhà nước về lao động địa phương khi cho nhiều người thôi việc;"
                        kl.IsViPham = TypeViPham.KhongViPham
                        kl.TenCotCauHoi = "Q242"
                        kl.TenBangCauHoi = "CauHoi2"
                        kl.PhieuId = hidPhieuID.Value
                        data.KetLuans.AddObject(kl)
                    ElseIf q2.Q241 >= 2 And Not q2.Q242 Then
                        kl.NDKetLuan = "Trong phạm vi thời gian thanh tra doanh nghiệp có " & String.Format(info, "{0:n0}", q2.Q241) & " lao động bị mất việc làm; chưa thực hiện báo cáo cơ quan quản lý nhà nước về lao động địa phương khi cho nhiều người thôi việc;"
                        kl.IsViPham = TypeViPham.ViPham
                        kl.TenCotCauHoi = "Q242"
                        kl.TenBangCauHoi = "CauHoi2"
                        kl.PhieuId = hidPhieuID.Value
                        data.KetLuans.AddObject(kl)
                        'Xuất kiến nghị
                        kn = New ThanhTraLaoDongModel.KienNghiDN
                        kn.NDKienNghi = "Thực hiện báo cáo cơ quan quản lý nhà nước về lao động địa phương khi cho nhiều người thôi việc theo do mất việc làm theo"
                        kn.TrichDanId = 9
                        kn.TenBangCauHoi = "Cauhoi2"
                        kn.PhieuId = hidPhieuID.Value
                        data.KienNghiDNs.AddObject(kn)
                    End If
                End If
                data.SaveChanges()
            End If
        End Using
    End Sub
    Protected Sub XulyKetLuanCauHoi3()
        Using data As New ThanhTraLaoDongEntities
            Dim kl As New ThanhTraLaoDongModel.KetLuan
            Dim kn As New ThanhTraLaoDongModel.KienNghiDN
            'Tao moi ket luan dua vao cauhoi3
            Dim q3 = (From a In data.CauHoi3
                      Where a.PhieuId = hidPhieuID.Value).FirstOrDefault
            If Not IsNothing(q3) Then
                If Not IsNothing(q3.Q31) Then
                    '3.2
                    If Not IsNothing(q3.Q32) AndAlso Not q3.Q32 Then
                        'Xuất kết luận
                        kl = New ThanhTraLaoDongModel.KetLuan
                        kl.NDKetLuan = "Chưa gửi bản Thỏa ước lao động tập thể đến cơ quan quản lý nhà nước về lao động địa phương;"
                        kl.TenCotCauHoi = "Q32"
                        kl.IsViPham = TypeViPham.ViPham
                        kl.TenBangCauHoi = "CauHoi3"
                        kl.PhieuId = hidPhieuID.Value
                        data.KetLuans.AddObject(kl)
                         'Xuất kiến nghị
                        kn = New ThanhTraLaoDongModel.KienNghiDN
                        kn.NDKienNghi = "Gửi bản Thỏa ước lao động tập thể đã ký đến cơ quan quản lý nhà nước về lao động địa phương theo"
                        kn.TrichDanId = 11
                        kn.TenBangCauHoi = "Cauhoi3"
                        kn.PhieuId = hidPhieuID.Value
                        data.KienNghiDNs.AddObject(kn)
                    End If

                    If Not IsNothing(q3.Q34) AndAlso q3.Q34 AndAlso Not IsNothing(q3.Q32) AndAlso q3.Q32 AndAlso String.IsNullOrEmpty(q3.Q33) Then ' tất cả đúng
                        kl = New ThanhTraLaoDongModel.KetLuan
                        kl.NDKetLuan = "Đã thương lượng, ký kết Thỏa ước lao động tập thể đúng quy trình, nội dung phù hợp pháp luật và đã gửi cơ quan quản lý nhà nước về lao động địa phương;"
                        kl.TenCotCauHoi = "Q34;Q32;Q33"
                        kl.IsViPham = TypeViPham.KhongViPham
                        kl.TenBangCauHoi = "CauHoi3"
                        kl.PhieuId = hidPhieuID.Value
                        data.KetLuans.AddObject(kl)
                    ElseIf Not IsNothing(q3.Q34) AndAlso Not q3.Q34 AndAlso Not IsNothing(q3.Q32) AndAlso Not q3.Q32 AndAlso Not IsNothing(q3.Q33) Then 'tất cả sai
                        'Xuất kết luận
                        'Kết luận 1
                        kl = New ThanhTraLaoDongModel.KetLuan
                        kl.NDKetLuan = "Thương lượng, ký kết Thỏa ước lao động tập thể không đúng quy trình: " & IIf(String.IsNullOrEmpty(q3.Q341), "", q3.Q341) & ";"
                        kl.TenCotCauHoi = "Q34"
                        kl.IsViPham = TypeViPham.ViPham
                        kl.TenBangCauHoi = "CauHoi3"
                        kl.PhieuId = hidPhieuID.Value
                        data.KetLuans.AddObject(kl)
                        'Kết luận 2
                        kl = New ThanhTraLaoDongModel.KetLuan
                        kl.NDKetLuan = "Chưa gửi bản Thỏa ước lao động tập thể đến cơ quan quản lý nhà nước địa phương;"
                        kl.TenCotCauHoi = "Q32"
                        kl.IsViPham = TypeViPham.ViPham
                        kl.TenBangCauHoi = "CauHoi3"
                        kl.PhieuId = hidPhieuID.Value
                        data.KetLuans.AddObject(kl)
                        'Kết luận 3
                        kl = New ThanhTraLaoDongModel.KetLuan
                        kl.NDKetLuan = "Thỏa ước lao động tập thể có nội dung không phù hợp: " & q3.Q33 & ";"
                        kl.TenCotCauHoi = "Q33"
                        kl.IsViPham = TypeViPham.ViPham
                        kl.TenBangCauHoi = "CauHoi3"
                        kl.PhieuId = hidPhieuID.Value
                        data.KetLuans.AddObject(kl)

                        'Xuất kiến nghị
                        ''''Kiến nghị 1
                        kn = New ThanhTraLaoDongModel.KienNghiDN
                        kn.NDKienNghi = "Thương lượng, ký kết Thỏa ước lao động tập thể đúng quy trình theo"
                        kn.TrichDanId = 10
                        kn.TenBangCauHoi = "Cauhoi3"
                        kn.PhieuId = hidPhieuID.Value
                        data.KienNghiDNs.AddObject(kn)

                        ''''Kiến nghị 2
                        kn = New ThanhTraLaoDongModel.KienNghiDN
                        kn.NDKienNghi = "Gửi thỏa ước lao động tập thể đến cơ quan quản lý nhà nước về lao động địa phương theo"
                        kn.TrichDanId = 11
                        kn.TenBangCauHoi = "Cauhoi3"
                        kn.PhieuId = hidPhieuID.Value
                        data.KienNghiDNs.AddObject(kn)

                        ''''Kiến nghị 3
                        kn = New ThanhTraLaoDongModel.KienNghiDN
                        kn.NDKienNghi = "Thương lượng lại Thỏa ước lao động thập thể để hủy bỏ những nội dung không phù hợp pháp luật: " & q3.Q33 & " theo"
                        kn.TrichDanId = 14
                        kn.TenBangCauHoi = "Cauhoi3"
                        kn.PhieuId = hidPhieuID.Value
                        data.KienNghiDNs.AddObject(kn)
                    ElseIf Not IsNothing(q3.Q34) AndAlso Not q3.Q34 Then 'Nếu không đúng quy trình
                        'Xuất kết luận
                        kl = New ThanhTraLaoDongModel.KetLuan
                        kl.NDKetLuan = "Thương lượng, ký kết thỏa ước lao động tập thể không đúng quy trình"
                        kl.NDKetLuan += IIf(String.IsNullOrEmpty(q3.Q341), ";", ": " & q3.Q341 & ";")
                        kl.TenCotCauHoi = "Q34"
                        kl.IsViPham = TypeViPham.ViPham
                        kl.TenBangCauHoi = "CauHoi3"
                        kl.PhieuId = hidPhieuID.Value
                        data.KetLuans.AddObject(kl)
                        'Xuất kiến nghị
                        kn = New ThanhTraLaoDongModel.KienNghiDN
                        kn.NDKienNghi = "Thương lượng, ký kết Thỏa ước lao động tập thể đúng quy trình theo"
                        kn.TrichDanId = 13
                        kn.TenBangCauHoi = "Cauhoi3"
                        kn.PhieuId = hidPhieuID.Value
                        data.KienNghiDNs.AddObject(kn)
                    ElseIf Not IsNothing(q3.Q34) AndAlso q3.Q34 AndAlso Not String.IsNullOrEmpty(q3.Q33) Then
                        'Xuất kết luận
                        kl = New ThanhTraLaoDongModel.KetLuan
                        kl.NDKetLuan = "Thỏa ước lao động tập thể có nội dung không phù hợp: " & q3.Q33 & ";"
                        kl.TenCotCauHoi = "Q33"
                        kl.IsViPham = TypeViPham.ViPham
                        kl.TenBangCauHoi = "CauHoi3"
                        kl.PhieuId = hidPhieuID.Value
                        data.KetLuans.AddObject(kl)
                        'Xuất kiến nghị
                        kn = New ThanhTraLaoDongModel.KienNghiDN
                        kn.NDKienNghi = "Thương lượng lại Thỏa ước lao động thập thể để hủy bỏ những nội dung không phù hợp pháp luật (" & q3.Q33 & ") theo"
                        kn.TrichDanId = 14
                        kn.TenBangCauHoi = "Cauhoi3"
                        kn.PhieuId = hidPhieuID.Value
                        data.KienNghiDNs.AddObject(kn)
                    End If
                    '3.5 Thực hiện các nội dung thỏa ước lao động tập thể đã ký
                    If Not IsNothing(q3.Q35) AndAlso Not q3.Q35 Then
                        'Xuất kết luận
                        kl = New ThanhTraLaoDongModel.KetLuan
                        kl.NDKetLuan = "Chưa thực hiện nội dung (" & q3.Q351 & ") trong thỏa ước lao động tập thể đã ký năm " & q3.Q31 & ";"
                        kl.TenCotCauHoi = "Q35"
                        kl.IsViPham = TypeViPham.ViPham
                        kl.TenBangCauHoi = "CauHoi3"
                        kl.PhieuId = hidPhieuID.Value
                        data.KetLuans.AddObject(kl)
                        'Xuất kiến nghị
                        kn = New ThanhTraLaoDongModel.KienNghiDN
                        kn.NDKienNghi = "Thực hiện nội dung (" & q3.Q351 & ") trong thỏa ước lao động tập thể đã ký năm " & q3.Q31 & " theo"
                        kn.TrichDanId = 75
                        kn.TenBangCauHoi = "Cauhoi3"
                        kn.PhieuId = hidPhieuID.Value
                        data.KienNghiDNs.AddObject(kn)
                    End If
                    data.SaveChanges()
                End If
            End If
        End Using
    End Sub
    Protected Sub XulyKetLuanCauHoi4()
        Using data As New ThanhTraLaoDongEntities
            Dim kl As New ThanhTraLaoDongModel.KetLuan
            Dim kn As New ThanhTraLaoDongModel.KienNghiDN
            'Tao moi ket luan dua vao cauhoi4
            Dim q4 As CauHoi4 = (From a In data.CauHoi4
                                 Where a.PhieuId = hidPhieuID.Value
                                 ).FirstOrDefault
            'tham số
            Dim LamThemGioNgayThuong = (From a In data.SYS_PARAMETERS Where a.Name = "LamThemGioNgayThuong" And a.Activated = True Select a.Val).FirstOrDefault
            Dim LamThemGioNgayNghiHangTuan = (From a In data.SYS_PARAMETERS Where a.Name = "LamThemGioNgayNghiHangTuan" And a.Activated = True Select a.Val).FirstOrDefault
            Dim LamThemGioVaoNgayLeTet = (From a In data.SYS_PARAMETERS Where a.Name = "LamThemGioVaoNgayLeTet" And a.Activated = True Select a.Val).FirstOrDefault
            Dim LamThemGioVaoBanDem = (From a In data.SYS_PARAMETERS Where a.Name = "LamThemGioVaoBanDem" And a.Activated = True Select a.Val).FirstOrDefault
            Dim MucTraLuongLamDem = (From a In data.SYS_PARAMETERS Where a.Name = "MucTraLuongLamDem" And a.Activated = True Select a.Val).FirstOrDefault
            Dim LuongTTQuaDaoTao = (From a In data.SYS_PARAMETERS Where a.Name = "LuongTTQuaDaoTao" And a.Activated = True Select a.Val).FirstOrDefault
            Dim MucLuongThuViec = (From a In data.SYS_PARAMETERS Where a.Name = "MucLuongThuViec" And a.Activated = True Select a.Val).FirstOrDefault
            Dim MucLuongNgungViec = (From a In data.SYS_PARAMETERS Where a.Name = "MucLuongNgungViec" And a.Activated = True Select a.Val).FirstOrDefault
            If Not IsNothing(q4) Then
                '4.1. Mức lương tối thiểu đang áp dụng
                Dim dn = (From b In data.DoanhNghieps Join c In data.PhieuNhapHeaders On c.DoanhNghiepId Equals b.DoanhNghiepId
                          Where c.PhieuID = hidPhieuID.Value
                          Select New With {b.LoaiHinhDNId, b.HuyenId, b.Huyen.LuongToiThieuID}).FirstOrDefault

                If Not IsNothing(dn) Then
                    Dim lh = (From a In data.LuongToiThieux
                              Where a.LuongToiThieuID = dn.LuongToiThieuID
                              Select New With {a.MucLuongToiThieu}).FirstOrDefault

                    'KL liệt kê
                    Dim arrKLGen As New ArrayList
                    kl = New ThanhTraLaoDongModel.KetLuan
                    kl.IsViPham = TypeViPham.KhongViPham
                    kl.NDKetLuan = "Doanh nghiệp "
                    If Not IsNothing(q4.Q4111) AndAlso q4.Q4111 > 0 And q4.Q4111 > lh.MucLuongToiThieu Then
                        arrKLGen.Add("áp dụng mức lương tối thiểu là " & String.Format(info, "{0:n0}", q4.Q4111) & " đồng")
                        kl.TenCotCauHoi += "Q4111;"
                    End If
                    If Not IsNothing(q4.Q4114) AndAlso q4.Q4114 >= LuongTTQuaDaoTao Then
                        arrKLGen.Add("trả lương thấp nhất đối với lao động đã qua đào tạo là " & String.Format(info, "{0:n0}", q4.Q4114) & " % lương tối thiểu")
                        kl.TenCotCauHoi += "Q4114;"
                    End If
                    If Not IsNothing(q4.Q4112) AndAlso q4.Q4112 > 0 Then
                        arrKLGen.Add("thu nhập trung bình của người lao động là " & String.Format(info, "{0:n0}", q4.Q4112) & " đồng/người/tháng")
                        kl.TenCotCauHoi += "Q4112;"
                    End If
                    If Not IsNothing(q4.Q4113) AndAlso Not q4.Q4113 Then
                        arrKLGen.Add("đã thanh toán lương đúng hạn cho người lao động")
                        kl.TenCotCauHoi += "Q4113;"
                    End If
                    If arrKLGen.Count > 0 Then
                        kl.NDKetLuan += String.Join(", ", TryCast(arrKLGen.ToArray(GetType(String)), String())) + ";"
                        kl.TenBangCauHoi = "CauHoi4"
                        kl.PhieuId = hidPhieuID.Value
                        data.KetLuans.AddObject(kl)
                    End If
                    'Mức lương tối thiểu với lao động đã qua đào tạo
                    If Not IsNothing(q4.Q4114) AndAlso q4.Q4114 < LuongTTQuaDaoTao Then
                        'Xuất kết luận
                        kl = New ThanhTraLaoDongModel.KetLuan
                        kl.NDKetLuan = "Trả lương thấp nhất với lao động đã qua đào tạo " & String.Format(info, "{0:n0}", q4.Q4114) & "% lương tối thiểu là chưa đủ mức quy định;"
                        kl.IsViPham = TypeViPham.ViPham
                        kl.TenBangCauHoi = "CauHoi4"
                        kl.TenCotCauHoi = "Q4114"
                        kl.PhieuId = hidPhieuID.Value
                        data.KetLuans.AddObject(kl)
                        'Xuất kiến nghị
                        kn = New ThanhTraLaoDongModel.KienNghiDN
                        kn.NDKienNghi = "Trả lương đối với lao động đã qua đào tạo không thấp hơn " & LuongTTQuaDaoTao & "% lương tối thiểu theo"
                        kn.TrichDanId = 18
                        kn.TenBangCauHoi = "Cauhoi4"
                        kn.PhieuId = hidPhieuID.Value
                        data.KienNghiDNs.AddObject(kn)

                    End If
                    'Nợ lương của người lao động check có là vi phạm
                    If Not IsNothing(q4.Q4113) AndAlso q4.Q4113 Then
                        'Xuất kết luận
                        kl = New ThanhTraLaoDongModel.KetLuan
                        kl.NDKetLuan = "Còn nợ lương của người lao động " & String.Format(info, "{0:n0}", q4.Q41131) & " tháng;"
                        kl.IsViPham = TypeViPham.ViPham
                        kl.TenBangCauHoi = "CauHoi4"
                        kl.TenCotCauHoi = "Q4113"
                        kl.PhieuId = hidPhieuID.Value
                        data.KetLuans.AddObject(kl)
                        'Xuất kiến nghị
                        kn = New ThanhTraLaoDongModel.KienNghiDN
                        kn.NDKienNghi = "Thanh toán tiền lương đầy đủ, đúng hạn cho người lao động theo"
                        kn.TrichDanId = 15
                        kn.TenBangCauHoi = "Cauhoi4"
                        kn.PhieuId = hidPhieuID.Value
                        data.KienNghiDNs.AddObject(kn)

                    End If

                    kl = New ThanhTraLaoDongModel.KetLuan
                    If dn.LoaiHinhDNId = Cls_Common.ThamSoSys.LoaiHinhDN And q4.Q4111 < lh.MucLuongToiThieu Then
                        kl.NDKetLuan = "Áp dụng mức lương tối thiểu " & String.Format(info, "{0:n0}", q4.Q4111) & " đồng thấp hơn quy định của nhà nước hiện hành;"
                        kl.TenCotCauHoi = "Q4111;"
                        kl.IsViPham = TypeViPham.ViPham
                        'Xuất kiến nghị
                        kn = New ThanhTraLaoDongModel.KienNghiDN
                        kn.NDKienNghi = "Áp dụng mức lương tối thiểu không thấp hơn quy định của nhà nước hiện hành theo"
                        kn.TrichDanId = 16
                        kn.TenBangCauHoi = "Cauhoi4"
                        kn.PhieuId = hidPhieuID.Value
                        data.KienNghiDNs.AddObject(kn)

                    ElseIf dn.LoaiHinhDNId <> Cls_Common.ThamSoSys.LoaiHinhDN Then
                        '' Lấy mức lương tối thiểu của doanh nghiệp
                        Dim iLuongToiThieuID As Integer = dn.LuongToiThieuID
                        Dim luong = (From c In data.LuongToiThieux
                                     Where c.LuongToiThieuID = iLuongToiThieuID
                                     Select New With {c.MucLuongToiThieu}).FirstOrDefault

                        If q4.Q4111 < luong.MucLuongToiThieu Then
                            kl.NDKetLuan = "Áp dụng mức lương tối thiểu " & String.Format(info, "{0:n0}", q4.Q4111) & " đồng thấp hơn mức lương tối thiểu vùng hiện hành;"
                            kl.TenCotCauHoi = "Q4111;"
                            kl.IsViPham = TypeViPham.ViPham
                            'Xuất kiến nghị
                            kn = New ThanhTraLaoDongModel.KienNghiDN
                            kn.NDKienNghi = "Áp dụng mức lương tối thiểu không thấp hơn quy định của nhà nước hiện hành theo"
                            kn.TrichDanId = 16
                            kn.TenBangCauHoi = "Cauhoi4"
                            kn.PhieuId = hidPhieuID.Value
                            data.KienNghiDNs.AddObject(kn)

                        End If
                    End If
                    If Not IsNothing(kl.NDKetLuan) Then
                        'Xuất kết luận
                        kl.TenBangCauHoi = "CauHoi4"
                        kl.PhieuId = hidPhieuID.Value
                        data.KetLuans.AddObject(kl)
                    End If
                End If

                '4.2;4.3;4.4;4.5 
                '4.2 Thỏa thuận trách nhiệm người lao động phải trả phí mở, duy trì tài khoản nhận lương qua ATM
                If Not IsNothing(q4.Q424) AndAlso q4.Q424 = 2 Then
                    'Xuất kết luận
                    kl = New ThanhTraLaoDongModel.KetLuan
                    kl.NDKetLuan = "Để người lao động trả phí duy trì, giao dịch nhận lương qua tài khoản nhưng không thỏa thuận trước là trái quy định;"
                    kl.IsViPham = TypeViPham.ViPham
                    kl.TenBangCauHoi = "CauHoi4"
                    kl.TenCotCauHoi = "Q424"
                    kl.PhieuId = hidPhieuID.Value
                    data.KetLuans.AddObject(kl)
                    'Xuất kiến nghị
                    kn = New ThanhTraLaoDongModel.KienNghiDN
                    kn.NDKienNghi = "Thỏa thuận rõ với người lao động trách nhiệm trả phí mở và duy trì tài khoản nhận lương qua thẻ ATM (trong hợp đồng lao động hoặc thỏa ước lao động tập thể) theo"
                    kn.TrichDanId = 76
                    kn.TenBangCauHoi = "Cauhoi4"
                    kn.PhieuId = hidPhieuID.Value
                    data.KienNghiDNs.AddObject(kn)
                End If
                'If dn.LoaiHinhDNId <> Cls_Common.ThamSoSys.LoaiHinhDN Then
                'TH: 4.4. Áp dụng thang lương chọn Của nhà nước/tự xây dựng và chọn có mục 4.5
                kl = New ThanhTraLaoDongModel.KetLuan
                Dim arrKLTrue As New ArrayList
                If (Not IsNothing(q4.Q44) AndAlso q4.Q44 = 2) Then
                    arrKLTrue.Add("Đã xây dựng thang lương, bảng lương")
                    kl.TenCotCauHoi += "Q44;"
                End If
                If (Not IsNothing(q4.Q45) AndAlso q4.Q45) Then
                    arrKLTrue.Add("Đã gửi thang lương, bảng lương đến cơ quan quản lý nhà nước về lao động địa phương")
                    kl.TenCotCauHoi += "Q45;"
                End If
                If arrKLTrue.Count > 1 Then
                    kl.NDKetLuan = arrKLTrue(0) & " và " & arrKLTrue(1).ToString().ToLower() & ";"
                    kl.TenBangCauHoi = "CauHoi4"
                    kl.IsViPham = TypeViPham.KhongViPham
                    kl.PhieuId = hidPhieuID.Value
                    data.KetLuans.AddObject(kl)
                ElseIf arrKLTrue.Count = 1 Then
                    kl.NDKetLuan = arrKLTrue(0) & ";"
                    kl.TenBangCauHoi = "CauHoi4"
                    kl.IsViPham = TypeViPham.KhongViPham
                    kl.PhieuId = hidPhieuID.Value
                    data.KetLuans.AddObject(kl)
                End If
                kl = New ThanhTraLaoDongModel.KetLuan
                '4.4. Áp dụng thang lương lương
                If Not IsNothing(q4.Q44) Then
                    Select Case q4.Q44
                        Case 1
                            kl.NDKetLuan = "Áp dụng hệ thống thang lương, bảng lương của nhà nước;"
                            kl.IsViPham = TypeViPham.KhongViPham
                            kl.TenCotCauHoi = "Q44"
                        Case 3
                            If IsNothing(q4.Q43) OrElse (Not IsNothing(q4.Q43) AndAlso q4.Q43) Then
                                'Xuất kết luận
                                kl.NDKetLuan = "Chưa xây dựng thang lương, bảng lương;"
                                kl.IsViPham = TypeViPham.ViPham
                                kl.TenCotCauHoi = "Q44;"
                                'Xuất kiến nghị
                                kn = New ThanhTraLaoDongModel.KienNghiDN
                                kn.NDKienNghi = "Xây dựng hệ thống thang lương, bảng lương và gửi đến cơ quan quản lý nhà nước về lao động địa phương theo"
                                kn.TrichDanId = 17
                                kn.TenBangCauHoi = "Cauhoi4"
                                kn.PhieuId = hidPhieuID.Value
                                data.KienNghiDNs.AddObject(kn)
                            End If
                    End Select
                    If Not IsNothing(kl.NDKetLuan) Then
                        kl.TenBangCauHoi = "CauHoi4"
                        kl.PhieuId = hidPhieuID.Value
                        data.KetLuans.AddObject(kl)
                    End If
                End If
                '4.3 & 4.4 TH: vi phạm tất cả
                If Not IsNothing(q4.Q44) AndAlso q4.Q44 = 3 AndAlso Not IsNothing(q4.Q43) AndAlso Not q4.Q43 Then
                    'Xuất kết luận
                    kl = New ThanhTraLaoDongModel.KetLuan
                    kl.NDKetLuan = "Chưa xây dựng thang lương, bảng lương, định mức lao động;"
                    kl.IsViPham = TypeViPham.ViPham
                    kl.TenBangCauHoi = "CauHoi4"
                    kl.TenCotCauHoi = "Q43;Q44"
                    kl.PhieuId = hidPhieuID.Value
                    data.KetLuans.AddObject(kl)
                    'Xuất kiến nghị
                    kn = New ThanhTraLaoDongModel.KienNghiDN
                    kn.NDKienNghi = "Xây dựng hệ thống thang lương, bảng lương, định mức lao động theo"
                    kn.TrichDanId = 18
                    kn.TenBangCauHoi = "Cauhoi4"
                    kn.PhieuId = hidPhieuID.Value
                    data.KienNghiDNs.AddObject(kn)

                End If
                '4.5. Gửi thang bảng lương với cơ quan quản lí nhà nước về lao động địa phương 
                If Not IsNothing(q4.Q45) Then
                    kl = New ThanhTraLaoDongModel.KetLuan
                    If Not q4.Q45 Then
                        'Xuất kết luận
                        kl.NDKetLuan = "Chưa gửi thang lương, bảng lương đến cơ quan quản lý nhà nước về lao động địa phương;"
                        kl.IsViPham = TypeViPham.ViPham
                        kl.TenBangCauHoi = "CauHoi4"
                        kl.TenCotCauHoi = "Q45"
                        kl.PhieuId = hidPhieuID.Value
                        data.KetLuans.AddObject(kl)
                        'Xuất kiến nghị
                        kn = New ThanhTraLaoDongModel.KienNghiDN
                        kn.NDKienNghi = "Gửi hệ thống thang lương, bảng lương đến cơ quan quản lý nhà nước về lao động địa phương theo"
                        kn.TrichDanId = 17
                        kn.TenBangCauHoi = "Cauhoi4"
                        kn.PhieuId = hidPhieuID.Value
                        data.KienNghiDNs.AddObject(kn)

                    End If
                End If
                'End If

                '.4.3
                If Not IsNothing(q4.Q43) Then
                    If q4.Q43 Then
                        kl = New ThanhTraLaoDongModel.KetLuan
                        kl.NDKetLuan = "Đã xây dựng định mức lao động;"
                        kl.IsViPham = TypeViPham.KhongViPham
                        kl.TenBangCauHoi = "CauHoi4"
                        kl.TenCotCauHoi = "Q43"
                        kl.PhieuId = hidPhieuID.Value
                        data.KetLuans.AddObject(kl)
                    Else
                        If IsNothing(q4.Q44) OrElse (Not IsNothing(q4.Q44) AndAlso q4.Q44 <> 3) Then
                            'Xuất kết luận
                            kl = New ThanhTraLaoDongModel.KetLuan
                            kl.NDKetLuan = "Chưa xây dựng định mức lao động;"
                            kl.IsViPham = TypeViPham.ViPham
                            kl.TenBangCauHoi = "CauHoi4"
                            kl.TenCotCauHoi = "Q44"
                            kl.PhieuId = hidPhieuID.Value
                            data.KetLuans.AddObject(kl)
                            'Xuất kiến nghị
                            kn = New ThanhTraLaoDongModel.KienNghiDN
                            kn.NDKienNghi = "Xây dựng định mức lao động làm căn cứ tính lương cho người lao động theo"
                            kn.TrichDanId = 17
                            kn.TenBangCauHoi = "Cauhoi4"
                            kn.PhieuId = hidPhieuID.Value
                            data.KienNghiDNs.AddObject(kn)

                        End If
                    End If
                End If

                '4.6;4.7;4.10.1
                'TH đúng 4.6. Mức trả lương làm thêm giờ theo/4.10.1 Trả lương ngừng việc/ 4.7. Mức trả lương làm đêm /4.10.4 Phụ cấp độc hại hoặc tính tiền độc hại vào lương:
                Dim arrKLGenTrue As New ArrayList
                kl = New ThanhTraLaoDongModel.KetLuan
                kl.NDKetLuan = "Đã trả lương "
                strTenCotCauHoi = ""
                Dim q5 = (From a In data.CauHoi5 Where a.PhieuId = hidPhieuID.Value).FirstOrDefault
                If Not IsNothing(q5) AndAlso Not IsNothing(q5.Q52) AndAlso q5.Q52 AndAlso _
                    Not IsNothing(q4.Q461) AndAlso q4.Q461 >= LamThemGioNgayThuong AndAlso _
                    Not IsNothing(q4.Q462) AndAlso q4.Q462 >= LamThemGioNgayNghiHangTuan AndAlso _
                    Not IsNothing(q4.Q463) AndAlso q4.Q463 >= LamThemGioVaoNgayLeTet AndAlso _
                    Not IsNothing(q4.Q464) AndAlso q4.Q464 >= LamThemGioVaoBanDem Then
                    arrKLGenTrue.Add("làm thêm giờ")
                    strTenCotCauHoi += "Q461;Q462;Q463;Q464"
                End If

                If Not IsNothing(q4.Q47) AndAlso q4.Q47 >= MucTraLuongLamDem Then
                    arrKLGenTrue.Add("làm đêm")
                    strTenCotCauHoi += "Q47;"
                End If
                If Not IsNothing(q4.Q4101) AndAlso q4.Q4101 >= MucLuongNgungViec Then
                    arrKLGenTrue.Add("ngừng việc")
                    strTenCotCauHoi += "Q4101;"
                End If
                If Not IsNothing(q4.Q41011) AndAlso q4.Q41011 >= MucLuongThuViec Then
                    arrKLGenTrue.Add("lương thử việc")
                    strTenCotCauHoi += "Q41011;"
                End If
                If Not IsNothing(q4.Q4104) AndAlso q4.Q4104 = 1 Then
                    arrKLGenTrue.Add("phụ cấp độc hại")
                    strTenCotCauHoi += "Q4104;"
                End If
                If arrKLGenTrue.Count > 0 Then
                    kl.NDKetLuan += String.Join(", ", TryCast(arrKLGenTrue.ToArray(GetType(String)), String())) + " đúng quy định;"
                    kl.IsViPham = TypeViPham.KhongViPham
                    kl.TenBangCauHoi = "CauHoi4"
                    kl.TenCotCauHoi = strTenCotCauHoi
                    kl.PhieuId = hidPhieuID.Value
                    data.KetLuans.AddObject(kl)
                End If

                'TH sai 4.6. Mức trả lương làm thêm giờ theo
                Dim arrKL As New ArrayList
                Dim arrKN As New ArrayList
                If (Not IsNothing(q4.Q46a) AndAlso q4.Q46a) Or (Not IsNothing(q4.Q46b) AndAlso q4.Q46b) Then
                    kl = New ThanhTraLaoDongModel.KetLuan
                    kl.NDKetLuan = "Trả lương làm thêm giờ với người hưởng "
                    Dim strlink1 As String = ""
                    Dim strlink2 As String = ""
                    Dim strresult As String = ""

                    If Not IsNothing(q4.Q46a) AndAlso q4.Q46a Then
                        strlink1 = "lương thời gian"
                    End If
                    If Not IsNothing(q4.Q46b) AndAlso q4.Q46b Then
                        strlink2 = "lương sản phẩm"
                    End If
                    If strlink1.Length > 0 And strlink2.Length > 0 Then
                        strresult = strlink1 & " và " & strlink2
                    ElseIf strlink1.Length > 0 And strlink2.Length = 0 Then
                        strresult = strlink1
                    ElseIf strlink1.Length = 0 And strlink2.Length > 0 Then
                        strresult = strlink2
                    End If

                    If q4.Q461 < LamThemGioNgayThuong Then
                        arrKL.Add("ngày thường bằng " & q4.Q461 & "%")
                        kl.TenCotCauHoi += "Q461;"
                        arrKN.Add("150% vào ngày thường")
                    End If
                    If q4.Q462 < LamThemGioNgayNghiHangTuan Then
                        arrKL.Add("ngày nghỉ hàng tuần bằng " & q4.Q462 & "%")
                        kl.TenCotCauHoi += "Q462;"
                        arrKN.Add("200% vào ngày nghỉ hàng tuần")
                    End If
                    If q4.Q463 < LamThemGioVaoNgayLeTet Then
                        arrKL.Add("ngày lễ, tết bằng " & q4.Q463 & "%")
                        kl.TenCotCauHoi += "Q463;"
                        arrKN.Add("300% vào ngày lễ")
                    End If
                    If q4.Q464 < LamThemGioVaoBanDem Then
                        arrKL.Add("Trả thêm tiền làm thêm giờ vào ban đêm = " & q4.Q464 & " % so với lương làm thêm giờ vào ban ngày")
                        kl.TenCotCauHoi += "Q464;"
                        arrKN.Add("Trả thêm tiền làm thêm giờ vào ban đêm = " & LamThemGioVaoBanDem & " % so với lương làm thêm giờ vào ban ngày")
                    End If
                    If arrKL.Count > 0 Then
                        'Xuất kết luận
                        kl.NDKetLuan += strresult & " vào " & String.Join(", ", TryCast(arrKL.ToArray(GetType(String)), String())) & " là chưa đủ theo quy định;"
                        kl.IsViPham = TypeViPham.ViPham
                        kl.TenBangCauHoi = "CauHoi4"
                        kl.PhieuId = hidPhieuID.Value
                        data.KetLuans.AddObject(kl)
                        'Xuất kiến nghị
                        kn = New ThanhTraLaoDongModel.KienNghiDN
                        kn.NDKienNghi = "Trả lương làm thêm giờ đủ " & String.Join(", ", TryCast(arrKN.ToArray(GetType(String)), String())) & " theo"
                        kn.TrichDanId = 19
                        kn.TenBangCauHoi = "Cauhoi4"
                        kn.PhieuId = hidPhieuID.Value
                        data.KienNghiDNs.AddObject(kn)

                    End If
                End If
                If q4.Q47 < MucTraLuongLamDem Then
                    'Xuất kết luận
                    kl = New ThanhTraLaoDongModel.KetLuan
                    kl.NDKetLuan = "Trả lương làm ca đêm bằng " & q4.Q47 & "% là chưa đủ theo quy định;"
                    kl.IsViPham = TypeViPham.ViPham
                    kl.TenBangCauHoi = "CauHoi4"
                    kl.TenCotCauHoi = "Q47"
                    kl.PhieuId = hidPhieuID.Value
                    data.KetLuans.AddObject(kl)
                    'Xuất kiến nghị
                    kn = New ThanhTraLaoDongModel.KienNghiDN
                    kn.NDKienNghi = "Trả lương làm ca đêm đủ " & MucTraLuongLamDem & "% theo"
                    kn.TrichDanId = 19
                    kn.TenBangCauHoi = "Cauhoi4"
                    kn.PhieuId = hidPhieuID.Value
                    data.KienNghiDNs.AddObject(kn)

                End If

                '4.10. Trả lương chế độ theo:
                If q4.Q410 > 0 Then
                    kl = New ThanhTraLaoDongModel.KetLuan
                    If q4.Q4101 < MucLuongNgungViec Then
                        'Xuất kết luận
                        kl.NDKetLuan = "Trả lương ngừng việc bằng " & q4.Q4101 & "% lương tối thiểu là chưa đủ theo quy định;"
                        kl.IsViPham = TypeViPham.ViPham
                        kl.TenBangCauHoi = "CauHoi4"
                        kl.TenCotCauHoi = "Q4101"
                        kl.PhieuId = hidPhieuID.Value
                        data.KetLuans.AddObject(kl)
                        'Xuất kiến nghị
                        kn = New ThanhTraLaoDongModel.KienNghiDN
                        kn.NDKienNghi = "Trả lương ngừng việc không thấp hơn mức lương tối thiểu vùng hiện hành theo"
                        kn.TrichDanId = 20
                        kn.TenBangCauHoi = "Cauhoi4"
                        kn.PhieuId = hidPhieuID.Value
                        data.KienNghiDNs.AddObject(kn)

                    End If
                    'Mức lương thử việc
                    If Not IsNothing(q4.Q41011) AndAlso q4.Q41011 < MucLuongThuViec Then
                        'Xuất kết luận
                        kl = New ThanhTraLaoDongModel.KetLuan
                        kl.NDKetLuan = "Trả lương thử việc bằng " & String.Format(info, "{0:n0}", q4.Q41011) & "% lương chính thức của công việc đó là chưa đủ theo quy định;"
                        kl.IsViPham = TypeViPham.ViPham
                        kl.TenBangCauHoi = "CauHoi4"
                        kl.TenCotCauHoi = "Q41011"
                        kl.PhieuId = hidPhieuID.Value
                        data.KetLuans.AddObject(kl)
                        'Xuất kiến nghị
                        kn = New ThanhTraLaoDongModel.KienNghiDN
                        kn.NDKienNghi = "Trả lương thử việc ít nhất bằng " & String.Format(info, "{0:n0}", MucLuongThuViec) & "% mức lương của công việc đó theo"
                        kn.TrichDanId = 61
                        kn.TenBangCauHoi = "Cauhoi4"
                        kn.PhieuId = hidPhieuID.Value
                        data.KienNghiDNs.AddObject(kn)

                    End If
                    Dim q24 = (From q In data.CauHoi2
                             Where q.PhieuId = hidPhieuID.Value
                             Select q.Q241).FirstOrDefault()
                    'TH đúng liệt kê 4.10.2 & 4.10.3
                    arrKL.Clear()
                    kl = New ThanhTraLaoDongModel.KetLuan
                    kl.NDKetLuan = "Đã trả  "
                    If Not IsNothing(q24) AndAlso q24 > 0 Then
                        If Not IsNothing(q4.Q4102) AndAlso q4.Q4102 And q4.Q41021 <= q4.Q41022 AndAlso Not IsNothing(q4.Q41032) AndAlso q4.Q41032 Then
                            arrKL.Add("trợ cấp mất việc cho " & q4.Q41022 & "/" & q4.Q41021 & " người")
                            kl.TenCotCauHoi += "Q4102"
                        End If
                    End If
                    If Not IsNothing(q4.Q4103) AndAlso q4.Q4103 AndAlso q4.Q410312 >= q4.Q410311 AndAlso Not IsNothing(q4.Q41032) AndAlso q4.Q41032 Then
                        arrKL.Add("trợ cấp thôi việc cho " & q4.Q410312 & "/" & q4.Q410311 & " người")
                        kl.TenCotCauHoi += "Q4103"
                    End If
                    If arrKL.Count > 0 Then
                        kl.NDKetLuan += String.Join(", ", TryCast(arrKL.ToArray(GetType(String)), String())) + " đủ điều kiện hưởng;"
                        kl.IsViPham = TypeViPham.KhongViPham
                        kl.TenBangCauHoi = "CauHoi4"
                        kl.PhieuId = hidPhieuID.Value
                        data.KetLuans.AddObject(kl)
                    End If

                    '' Xet neu muc 2.4 co thi xet muc 4.10.2
                    'TH sai trợ cấp mất việc và thôi việc
                    arrKL.Clear()
                    kl = New ThanhTraLaoDongModel.KetLuan
                    Dim bA As Boolean = False
                    Dim bB As Boolean = False
                    Dim iCountA As Integer = 0
                    Dim iCountB As Integer = 0
                    If Not IsNothing(q24) AndAlso q24 > 0 Then
                        '' Xet muc 4.10.2  
                        If Not IsNothing(q4.Q4102) Then
                            kl = New ThanhTraLaoDongModel.KetLuan
                            If q4.Q4102 AndAlso Not IsNothing(q4.Q41021) AndAlso Not IsNothing(q4.Q41022) AndAlso q4.Q41021 > q4.Q41022 Then
                                'Xử lý kết luận
                                kl.NDKetLuan = "Chưa trả trợ cấp mất việc cho " & String.Format(info, "{0:n0}", q4.Q41021 - q4.Q41022) & " người đủ điều kiện hưởng;"
                                kl.IsViPham = TypeViPham.ViPham
                                kl.TenCotCauHoi += "Q4102"
                                kl.TenBangCauHoi = "CauHoi4"
                                kl.PhieuId = hidPhieuID.Value
                                data.KetLuans.AddObject(kl)
                                iCountA += 1
                                'Xuất kiến nghị
                                kn = New ThanhTraLaoDongModel.KienNghiDN
                                kn.NDKienNghi = "Trả trợ cấp mất việc cho " & String.Format(info, "{0:n0}", q4.Q41021 - q4.Q41022) & " người đủ điều kiện hưởng theo"
                                kn.TrichDanId = 59
                                kn.TenBangCauHoi = "Cauhoi4"
                                kn.PhieuId = hidPhieuID.Value
                                data.KienNghiDNs.AddObject(kn)
                            End If
                            If Not q4.Q4102 Then
                                'Xử lý kết luận
                                Dim strtemp As String = "Không trả trợ cấp mất việc cho người lao động"
                                If Not IsNothing(q4.Q41021) Then
                                    strtemp = "Không trả trợ cấp mất việc cho " & q4.Q41021 & " người"
                                End If
                                kl.NDKetLuan = strtemp + " đủ điều kiện hưởng;"
                                kl.IsViPham = TypeViPham.ViPham
                                kl.TenCotCauHoi += "Q4102;"
                                kl.TenBangCauHoi = "CauHoi4"
                                kl.PhieuId = hidPhieuID.Value
                                data.KetLuans.AddObject(kl)
                                iCountB += 1
                                'Xuất kiến nghị
                                kn = New ThanhTraLaoDongModel.KienNghiDN
                                kn.NDKienNghi = "Trả trợ cấp mất việc cho " & q4.Q41021 & " người đủ điều kiện hưởng theo"
                                kn.TrichDanId = 59
                                kn.TenBangCauHoi = "Cauhoi4"
                                kn.PhieuId = hidPhieuID.Value
                                data.KienNghiDNs.AddObject(kn)

                            End If
                        End If
                    End If

                    '' Xet muc 4.10.3
                    Dim strQ4103 As String = ""
                    strKetLuan = ""
                    If Not IsNothing(q4.Q4103) Then
                        If q4.Q4103 And q4.Q410312 < q4.Q410311 Then
                            'Xử lý kết luận
                            kl = New ThanhTraLaoDongModel.KetLuan
                            strKetLuan = "Chưa trả trợ cấp thôi việc cho " & String.Format(info, "{0:n0}", (q4.Q410311 - q4.Q410312)) & " người"
                            strKetLuan = strKetLuan & IIf(IsNothing(q4.Q41033) OrElse q4.Q41033.Length = 0, "", ", (lý do chưa trả: " & q4.Q41033 & ")")
                            'Xử lý kiến nghị
                            kn = New ThanhTraLaoDongModel.KienNghiDN
                            kn.NDKienNghi = "Trả trợ cấp thôi việc cho " & String.Format(info, "{0:n0}", (q4.Q410311 - q4.Q410312)) & " người đủ điều kiện hưởng"

                            If Not IsNothing(q4.Q41032) AndAlso Not q4.Q41032 Then
                                strKetLuan = strKetLuan & "; chưa làm tròn số tháng lẻ khi tính thời gian hưởng trợ cấp theo quy định;"
                                kl.TenCotCauHoi += "Q41032;"
                                kn.NDKienNghi += " và tính tròn số tháng lẻ"
                            End If
                            kl.NDKetLuan = strKetLuan
                            kl.TenCotCauHoi += "Q4103"
                            kl.IsViPham = TypeViPham.ViPham
                            kl.TenBangCauHoi = "CauHoi4"
                            kl.PhieuId = hidPhieuID.Value
                            data.KetLuans.AddObject(kl)
                            iCountA += 1
                            'Xuất kiến nghị
                            kn.NDKienNghi += " theo"
                            kn.TrichDanId = 60
                            kn.TenBangCauHoi = "Cauhoi4"
                            kn.PhieuId = hidPhieuID.Value
                            data.KienNghiDNs.AddObject(kn)

                        End If
                    End If

                    '4.10.4 Phụ cấp độc hại hoặc tính tiền độc hại vào lương:
                    If Not IsNothing(q4.Q4104) AndAlso q4.Q4104 = 0 Then
                        'Xuất kết luận
                        kl = New ThanhTraLaoDongModel.KetLuan
                        kl.TenCotCauHoi = "Q4104"
                        kl.NDKetLuan = "Chưa trả phụ cấp độc hại hoặc tính tiền độc hại vào lương đối với người lao động làm công việc nguy hiểm, độc hại;"
                        kl.IsViPham = TypeViPham.ViPham
                        kl.TenBangCauHoi = "CauHoi4"
                        kl.PhieuId = hidPhieuID.Value
                        data.KetLuans.AddObject(kl)
                        'Xuất kiến nghị
                        kn = New ThanhTraLaoDongModel.KienNghiDN
                        kn.NDKienNghi = "Trả phụ cấp độc hại hoặc tính tiền độc hại vào lương đối với người lao động làm công việc nguy hiểm, độc hại theo"
                        kn.TrichDanId = 69
                        kn.TenBangCauHoi = "Cauhoi4"
                        kn.PhieuId = hidPhieuID.Value
                        data.KienNghiDNs.AddObject(kn)
                    End If
                    If Not IsNothing(q4.Q4104) AndAlso q4.Q4104 = 2 Then
                        'Xuất kết luận
                        kl = New ThanhTraLaoDongModel.KetLuan
                        kl.TenCotCauHoi = "Q4104"
                        kl.NDKetLuan = "Trả phụ cấp độc hại hoặc tính tiền độc hại vào lương chưa đầy đủ đối với người lao động làm các nghề, nặng nhọc, độc hại, nguy hiểm" + IIf(IsNothing(q4.Q41041), "", " (chưa thực hiện với nghề " + q4.Q41041 + ")") + ";"
                        kl.IsViPham = TypeViPham.ViPham
                        kl.TenBangCauHoi = "CauHoi4"
                        kl.PhieuId = hidPhieuID.Value
                        data.KetLuans.AddObject(kl)
                        'Xuất kiến nghị
                        kn = New ThanhTraLaoDongModel.KienNghiDN
                        kn.NDKienNghi = "Thực hiện trả phụ cấp độc hại hoặc tính tiền độc hại vào lương đầy đủ đối với người lao động làm các nghề, nặng nhọc, độc hại, nguy hiểm" + IIf(IsNothing(q4.Q41041), " ", " (bổ sung với nghề " + q4.Q41041 + ") ") + "theo"
                        kn.TrichDanId = 18
                        kn.TenBangCauHoi = "Cauhoi4"
                        kn.PhieuId = hidPhieuID.Value
                        data.KienNghiDNs.AddObject(kn)
                    End If
                    '4.10.5 Trả lương ngày lễ, ngày nghỉ hưởng nguyên lương
                    If Not IsNothing(q4.Q4105) Then
                        If q4.Q4105 = 1 Then
                            'Xuất kết luận
                            kl = New ThanhTraLaoDongModel.KetLuan
                            kl.TenCotCauHoi = "Q4105"
                            kl.NDKetLuan = "Đã trả lương ngày lễ, ngày nghỉ hưởng nguyên lương theo quy định;"
                            kl.IsViPham = TypeViPham.KhongViPham
                            kl.TenBangCauHoi = "CauHoi4"
                            kl.PhieuId = hidPhieuID.Value
                            data.KetLuans.AddObject(kl)
                        ElseIf q4.Q4105 = 0 Then
                            'Xuất kết luận
                            kl = New ThanhTraLaoDongModel.KetLuan
                            kl.TenCotCauHoi = "Q4105"
                            kl.NDKetLuan = "Chưa trả lương ngày lễ, ngày nghỉ có hưởng lương theo quy định;"
                            kl.IsViPham = TypeViPham.ViPham
                            kl.TenBangCauHoi = "CauHoi4"
                            kl.PhieuId = hidPhieuID.Value
                            data.KetLuans.AddObject(kl)
                            'Xuất kiến nghị
                            kn = New ThanhTraLaoDongModel.KienNghiDN
                            kn.NDKienNghi = "Trả lương ngày lễ, ngày nghỉ hưởng nguyên lương theo quy định theo"
                            kn.TrichDanId = 26
                            kn.TenBangCauHoi = "Cauhoi4"
                            kn.PhieuId = hidPhieuID.Value
                            data.KienNghiDNs.AddObject(kn)
                        ElseIf q4.Q4105 = 2 Then
                            'Xuất kết luận
                            kl = New ThanhTraLaoDongModel.KetLuan
                            kl.TenCotCauHoi = "Q4105"
                            kl.NDKetLuan = "Chưa trả lương ngày lễ, ngày nghỉ có hưởng lương đối với lao động hưởng lương sản phẩm;"
                            kl.IsViPham = TypeViPham.ViPham
                            kl.TenBangCauHoi = "CauHoi4"
                            kl.PhieuId = hidPhieuID.Value
                            data.KetLuans.AddObject(kl)
                            'Xuất kiến nghị
                            kn = New ThanhTraLaoDongModel.KienNghiDN
                            kn.NDKienNghi = "Trả lương ngày lễ, ngày nghỉ có hưởng lương đối với lao động hưởng lương sản phẩm theo"
                            kn.TrichDanId = 26
                            kn.TenBangCauHoi = "Cauhoi4"
                            kn.PhieuId = hidPhieuID.Value
                            data.KienNghiDNs.AddObject(kn)
                        End If
                    End If
                End If


                '4.8. Phạt tiền, phạt trừ lương:
                If Not IsNothing(q4.Q48) AndAlso Not q4.Q48 Then
                    'Xuất kết luận
                    kl = New ThanhTraLaoDongModel.KetLuan
                    kl.TenCotCauHoi = "Q48"
                    kl.NDKetLuan = "Áp dụng xử lý kỷ luật lao động bằng hình thức phạt tiền là trái quy định;"
                    kl.IsViPham = TypeViPham.ViPham
                    kl.TenBangCauHoi = "CauHoi4"
                    kl.PhieuId = hidPhieuID.Value
                    data.KetLuans.AddObject(kl)
                    'Xuất kiến nghị
                    kn = New ThanhTraLaoDongModel.KienNghiDN
                    kn.NDKienNghi = "Bãi bỏ xử lý kỷ luật lao động bằng hình thức phạt tiền. Doanh nghiệp phải áp dụng các hình thức kỷ luật lao động theo"
                    kn.TrichDanId = 21
                    kn.TenBangCauHoi = "Cauhoi4"
                    kn.PhieuId = hidPhieuID.Value
                    data.KienNghiDNs.AddObject(kn)

                End If

                '4.9. Công khai thang lương, bảng lương và  quy chế thưởng
                If Not IsNothing(q4.Q49) Then
                    kl = New ThanhTraLaoDongModel.KetLuan
                    If q4.Q49 Then
                        kl.NDKetLuan = "Đã thực hiện công khai thang lương, bảng lương và quy chế lương, thưởng;"
                        kl.IsViPham = TypeViPham.KhongViPham
                    Else
                        kl.NDKetLuan = "Chưa thực hiện công khai thang lương, bảng lương và quy chế lương, thưởng tại nơi làm việc;"
                        kl.IsViPham = TypeViPham.ViPham
                        'Xuất kiến nghị
                        kn = New ThanhTraLaoDongModel.KienNghiDN
                        kn.NDKienNghi = "Thực hiện công khai hệ thống thang lương, bảng lương tại nơi làm việc theo"
                        kn.TrichDanId = 22
                        kn.TenBangCauHoi = "Cauhoi4"
                        kn.PhieuId = hidPhieuID.Value
                        data.KienNghiDNs.AddObject(kn)
                    End If
                    kl.TenCotCauHoi = "Q49"
                    kl.TenBangCauHoi = "CauHoi4"
                    kl.PhieuId = hidPhieuID.Value
                    data.KetLuans.AddObject(kl)
                End If
                '' Lưu lại dữ liệu
                data.SaveChanges()
            End If
        End Using
    End Sub
    Protected Sub XulyKetLuanCauHoi5()
        Using data As New ThanhTraLaoDongEntities
            Dim kl As New ThanhTraLaoDongModel.KetLuan
            Dim kn As New ThanhTraLaoDongModel.KienNghiDN
            'Tao moi ket luan dua vao cauhoi4
            Dim q5 As CauHoi5 = (From a In data.CauHoi5 Where a.PhieuId = hidPhieuID.Value Select a).FirstOrDefault
            'Tham số
            Dim GioLamThemTheoNamNgheBT = (From a In data.SYS_PARAMETERS Where a.Name = "GioLamThemTheoNamNgheBT" And a.Activated = True Select a.Val).FirstOrDefault
            Dim GioLamThemTheoNamNgheKhac = (From a In data.SYS_PARAMETERS Where a.Name = "GioLamThemTheoNamNgheKhac" And a.Activated = True Select a.Val).FirstOrDefault
            Dim GioLamThemTheoNgay = (From a In data.SYS_PARAMETERS Where a.Name = "GioLamThemTheoNgay" And a.Activated = True Select a.Val).FirstOrDefault

            Dim NgayNghiHangNamCVBT = (From a In data.SYS_PARAMETERS Where a.Name = "NgayNghiHangNamCVBT" And a.Activated = True Select a.Val).FirstOrDefault
            Dim NgayNghiHangNamCVDHNN = (From a In data.SYS_PARAMETERS Where a.Name = "NgayNghiHangNamCVDHNN" And a.Activated = True Select a.Val).FirstOrDefault
            Dim NgayNghiHangNamCVDBDHNN = (From a In data.SYS_PARAMETERS Where a.Name = "NgayNghiHangNamCVDBDHNN" And a.Activated = True Select a.Val).FirstOrDefault

            If Not q5 Is Nothing Then
                Dim flag As Boolean = True 'Cho biết tất cả các mục đều đúng hoặc sai?
                Dim arrKN As New ArrayList
                '5.1. Số giờ làm việc
                'Nếu số giờ làm việc / tuần > 48  và số giờ làm việc/ ngày đối với CVNNĐH > 6 
                If (Not IsNothing(q5.Q512) AndAlso q5.Q512 > 48) AndAlso (Not IsNothing(q5.Q515) AndAlso q5.Q515 > 48) AndAlso (Not IsNothing(q5.Q513) AndAlso q5.Q513 > 6) Then
                    flag = False
                    kl = New ThanhTraLaoDongModel.KetLuan
                    kl.NDKetLuan = "Doanh nghiệp tổ chức làm việc " & q5.Q512.ToString.Replace(".", ",") & " giờ/tuần; bố trí người làm công việc đặc biệt nặng nhọc, độc hại làm việc " & q5.Q513.ToString.Replace(".", ",") & " giờ/ngày là quá số giờ quy định;"
                    kl.IsViPham = TypeViPham.ViPham
                    kl.TenCotCauHoi = "Q512;Q513;Q515"
                    kl.TenBangCauHoi = "CauHoi5"
                    kl.PhieuId = hidPhieuID.Value
                    data.KetLuans.AddObject(kl)
                    'Xuất kiến nghị
                    kn = New ThanhTraLaoDongModel.KienNghiDN
                    kn.NDKienNghi = "Tổ chức làm việc không quá 48 giờ/tuần đối với người làm công việc bình thường; không quá 6 giờ/ ngày đối với người làm công việc đặc biệt nặng nhọc, độc hại, nguy hiểm theo"
                    kn.TrichDanId = 84
                    kn.TenBangCauHoi = "Cauhoi5"
                    kn.PhieuId = hidPhieuID.Value
                    data.KienNghiDNs.AddObject(kn)
                Else
                    Dim arrKL As New ArrayList
                    strTenCotCauHoi = ""
                    kl = New ThanhTraLaoDongModel.KetLuan
                    Dim dem = ""
                    If Not IsNothing(q5.Q512) AndAlso q5.Q512 > 48 Then
                        flag = False
                        dem = dem + "A"
                        arrKL.Add(q5.Q512.ToString.Replace(".", ",") & " giờ/tuần với người hưởng lương thời gian")
                        strTenCotCauHoi = strTenCotCauHoi & "Q512;"
                        
                    End If
                    If Not IsNothing(q5.Q515) AndAlso q5.Q515 > 48 Then
                        flag = False
                        dem = dem + "B"
                        arrKL.Add(q5.Q515.ToString.Replace(".", ",") & " giờ/tuần với người hưởng lương thời sản phẩm")
                        strTenCotCauHoi = strTenCotCauHoi & "Q515;"
                        
                    End If
                    If Not IsNothing(q5.Q513) AndAlso q5.Q513 > 6 Then
                        flag = False
                        arrKL.Add(q5.Q513.ToString.Replace(".", ",") & " giờ/ngày với người làm công việc đặc biệt nặng nhọc, độc hại, nguy hiểm")
                        strTenCotCauHoi = strTenCotCauHoi & "Q513;"
                    End If
                    If dem = "A" Then
                        'Xuất kiến nghị
                        kn = New ThanhTraLaoDongModel.KienNghiDN
                        kn.NDKienNghi = "Tổ chức làm việc không quá 48 giờ/tuần đối với người làm công việc bình thường hưởng lương thời gian theo"
                        kn.TrichDanId = 84
                        kn.TenBangCauHoi = "Cauhoi5"
                        kn.PhieuId = hidPhieuID.Value
                        data.KienNghiDNs.AddObject(kn)
                    End If
                    If dem = "B" Then
                        'Xuất kiến nghị
                        kn = New ThanhTraLaoDongModel.KienNghiDN
                        kn.NDKienNghi = "Tổ chức làm việc không quá 48 giờ/tuần đối với người làm công việc bình thường hưởng lương sản phẩm theo"
                        kn.TrichDanId = 84
                        kn.TenBangCauHoi = "Cauhoi5"
                        kn.PhieuId = hidPhieuID.Value
                        data.KienNghiDNs.AddObject(kn)
                    End If
                    If dem = "AB" Then
                        'Xuất kiến nghị
                        kn = New ThanhTraLaoDongModel.KienNghiDN
                        kn.NDKienNghi = "Tổ chức làm việc không quá 48 giờ/tuần đối với người làm công việc bình thường hưởng lương thời gian, không quá 48 giờ/tuần đối với người làm công việc bình thường hưởng lương sản phẩm theo"
                        kn.TrichDanId = 84
                        kn.TenBangCauHoi = "Cauhoi5"
                        kn.PhieuId = hidPhieuID.Value
                        data.KienNghiDNs.AddObject(kn)
                    End If

                    If arrKL.Count > 0 Then
                        'Xuất kết luận
                        kl.NDKetLuan = "Tổ chức làm việc " & String.Join(", ", TryCast(arrKL.ToArray(GetType(String)), String())) + " là quá mức quy định;"
                        kl.IsViPham = TypeViPham.ViPham
                        kl.TenCotCauHoi = strTenCotCauHoi
                        kl.TenBangCauHoi = "CauHoi5"
                        kl.PhieuId = hidPhieuID.Value
                        data.KetLuans.AddObject(kl)
                    End If
                End If

                'Nếu số giờ làm việc / tuần <= 48  và số giờ làm việc/ ngày đối với CVNNĐH > 6 
                If (Not IsNothing(q5.Q512) AndAlso q5.Q512 <= 48) AndAlso (Not IsNothing(q5.Q513) AndAlso q5.Q513 > 6) Then
                    flag = False
                    kl = New ThanhTraLaoDongModel.KetLuan
                    kl.NDKetLuan = "Doanh nghiệp bố trí người làm công việc đặc biệt nặng nhọc, độc hại làm việc " & q5.Q513.ToString.Replace(".", ",") & " giờ/ngày là quá số giờ quy định;"
                    kl.IsViPham = TypeViPham.ViPham
                    kl.TenCotCauHoi = "Q513"
                    kl.TenBangCauHoi = "CauHoi5"
                    kl.PhieuId = hidPhieuID.Value
                    data.KetLuans.AddObject(kl)
                    'Xuất kiến nghị
                    kn = New ThanhTraLaoDongModel.KienNghiDN
                    kn.NDKienNghi = "Tổ chức làm việc không quá 6 giờ/ngày đối với người làm công việc đặc biệt nặng nhọc, độc hại, nguy hiểm theo"
                    kn.TrichDanId = 23
                    kn.TenBangCauHoi = "Cauhoi5"
                    kn.PhieuId = hidPhieuID.Value
                    data.KienNghiDNs.AddObject(kn)
                End If
                'Nếu số giờ làm việc / tuần > 48  và số giờ làm việc/ ngày đối với CVNNĐH <= 6 
                'If (Not IsNothing(q5.Q512) AndAlso q5.Q512 > 48) AndAlso (Not IsNothing(q5.Q513) AndAlso q5.Q513 <= 6) Then
                '    flag = False
                '    kl = New ThanhTraLaoDongModel.KetLuan
                '    kl.NDKetLuan = "Doanh nghiệp tổ chức làm việc " & q5.Q512.ToString.Replace(".", ",") & " giờ/tuần là quá số giờ quy định;"
                '    kl.IsViPham = TypeViPham.ViPham
                '    kl.TenCotCauHoi = "Q512"
                '    kl.TenBangCauHoi = "CauHoi5"
                '    kl.PhieuId = hidPhieuID.Value
                '    data.KetLuans.AddObject(kl)
                '    'Xuất kiến nghị
                '    kn = New ThanhTraLaoDongModel.KienNghiDN
                '    kn.NDKienNghi = "Tổ chức làm việc không quá 48 giờ/tuần đối với người làm công việc bình thường theo"
                '    kn.TrichDanId = 23
                '    kn.TenBangCauHoi = "Cauhoi5"
                '    kn.PhieuId = hidPhieuID.Value
                '    data.KienNghiDNs.AddObject(kn)
                'End If

                '5.2. Làm thêm giờ                
                If Not IsNothing(q5.Q52) Then
                    If q5.Q52 Then
                        'lay lai gio lam toi da theo linh vuc san xuat
                        Dim iCompare As Integer = (From q In data.PhieuNhapHeaders Join h In data.DoanhNghieps On q.DoanhNghiepId Equals h.DoanhNghiepId _
                                                                                Where q.PhieuID = hidPhieuID.Value Select h.LoaiHinhSanXuat.SoGioLamThemToiDaTheoNam).FirstOrDefault
                        If Not IsNothing(q5.Q521) AndAlso Not IsNothing(q5.Q522) Then
                            kl = New ThanhTraLaoDongModel.KetLuan
                            strTenCotCauHoi = ""
                            If q5.Q521 > GioLamThemTheoNgay And q5.Q522 <= iCompare Then 'lĩnh vực giày da, dệt may, thủy sản
                                flag = False
                                kl.NDKetLuan = "Tổ chức làm thêm giờ mức " & q5.Q521 & " giờ/người/ngày là quá số giờ quy định;"
                                kl.IsViPham = TypeViPham.ViPham
                                strTenCotCauHoi = "Q521"
                                'Xuất kiến nghị
                                kn = New ThanhTraLaoDongModel.KienNghiDN
                                kn.NDKienNghi = "Tổ chức làm thêm giờ không quá số giờ trong ngày theo"
                                kn.TrichDanId = 24
                                kn.TenBangCauHoi = "Cauhoi5"
                                kn.PhieuId = hidPhieuID.Value
                                data.KienNghiDNs.AddObject(kn)
                            ElseIf q5.Q521 <= GioLamThemTheoNgay And q5.Q522 > iCompare Then
                                flag = False
                                kl.NDKetLuan = "Tổ chức làm thêm giờ đến " & q5.Q522 & " giờ/người/năm là quá số giờ quy định;"
                                kl.IsViPham = TypeViPham.ViPham
                                strTenCotCauHoi = "Q522"
                                'Xuất kiến nghị
                                kn = New ThanhTraLaoDongModel.KienNghiDN
                                kn.NDKienNghi = "Tổ chức làm thêm giờ không quá số giờ trong năm theo"
                                kn.TrichDanId = 24
                                kn.TenBangCauHoi = "Cauhoi5"
                                kn.PhieuId = hidPhieuID.Value
                                data.KienNghiDNs.AddObject(kn)

                            ElseIf q5.Q521 > GioLamThemTheoNgay And q5.Q522 > iCompare Then
                                flag = False
                                kl.NDKetLuan = "Tổ chức làm thêm giờ mức " & q5.Q521 & " giờ/người/ngày, " & q5.Q522 & " giờ/người/năm là quá số giờ quy định;"
                                kl.IsViPham = TypeViPham.ViPham
                                strTenCotCauHoi = "Q521;Q522"
                                'Xuất kiến nghị
                                kn = New ThanhTraLaoDongModel.KienNghiDN
                                kn.NDKienNghi = "Tổ chức làm thêm giờ không quá số giờ trong ngày, trong năm theo"
                                kn.TrichDanId = 24
                                kn.TenBangCauHoi = "Cauhoi5"
                                kn.PhieuId = hidPhieuID.Value
                                data.KienNghiDNs.AddObject(kn)

                            Else 'không vi phạm
                                kl.NDKetLuan = "Tổ chức làm thêm giờ đúng quy định;"
                                kl.IsViPham = TypeViPham.KhongXet
                                strTenCotCauHoi = "Q521;Q522"
                            End If
                            kl.TenCotCauHoi = strTenCotCauHoi
                            kl.TenBangCauHoi = "CauHoi5"
                            kl.PhieuId = hidPhieuID.Value
                            data.KetLuans.AddObject(kl)
                        End If
                    End If
                End If

                '5.3. Thực hiện ngày nghỉ hàng năm hưởng nguyên lương
                If Not IsNothing(q5.Q53) Then
                    If Not q5.Q53 Then
                        flag = False
                        kl = New ThanhTraLaoDongModel.KetLuan
                        kl.NDKetLuan = "Không thực hiện ngày nghỉ phép hàng năm theo quy định;"
                        kl.IsViPham = TypeViPham.ViPham
                        kl.TenCotCauHoi = "Q53"
                        kl.TenBangCauHoi = "CauHoi5"
                        kl.PhieuId = hidPhieuID.Value
                        data.KetLuans.AddObject(kl)
                        'Xuất kiến nghị
                        kn = New ThanhTraLaoDongModel.KienNghiDN
                        kn.NDKienNghi = "Thực hiện số ngày nghỉ hằng năm hưởng nguyên lương theo"
                        kn.TrichDanId = 25
                        kn.TenBangCauHoi = "Cauhoi5"
                        kn.PhieuId = hidPhieuID.Value
                        data.KienNghiDNs.AddObject(kn)

                    Else 'chọn có và xét từng trường hợp nếu vi phạm
                        If Not IsNothing(q5.Q531) Or Not IsNothing(q5.Q532) Or Not IsNothing(q5.Q533) Then
                            Dim arrKL As New ArrayList
                            arrKN.Clear()
                            strTenCotCauHoi = ""
                            kl = New ThanhTraLaoDongModel.KetLuan
                            If q5.Q531 < NgayNghiHangNamCVBT Then
                                flag = False
                                arrKL.Add(q5.Q531 & " ngày đối với lao động làm công việc bình thường")
                                arrKN.Add(NgayNghiHangNamCVBT & " ngày với người làm công việc bình thường")
                                strTenCotCauHoi = strTenCotCauHoi & "Q531;"
                            End If
                            If q5.Q532 < NgayNghiHangNamCVDHNN Then
                                flag = False
                                arrKL.Add(q5.Q532 & " ngày đối với lao động làm công việc nặng nhọc, độc hại, nguy hiểm")
                                arrKN.Add(NgayNghiHangNamCVDHNN & " ngày với người làm công việc nghề công việc nặng nhọc, độc hại, nguy hiểm")
                                strTenCotCauHoi = strTenCotCauHoi & "Q532;"
                            End If
                            If q5.Q533 < NgayNghiHangNamCVDBDHNN Then
                                flag = False
                                arrKL.Add(q5.Q533 & " ngày đối với lao động làm công việc đặc biệt nặng nhọc, độc hại, nguy hiểm")
                                arrKN.Add(NgayNghiHangNamCVDBDHNN & " ngày với người làm công việc đặc biệt nặng nhọc, độc hại, nguy hiểm")
                                strTenCotCauHoi = strTenCotCauHoi & "Q533;"
                            End If
                            If arrKL.Count > 0 Then
                                'Xuất kết luận
                                kl.NDKetLuan = "Thực hiện số ngày nghỉ hàng năm " & String.Join(", ", TryCast(arrKL.ToArray(GetType(String)), String())) + " là chưa đủ theo quy định;"
                                kl.IsViPham = TypeViPham.ViPham
                                kl.TenCotCauHoi = strTenCotCauHoi
                                kl.TenBangCauHoi = "CauHoi5"
                                kl.PhieuId = hidPhieuID.Value
                                data.KetLuans.AddObject(kl)
                                'Xuất kiến nghị
                                kn = New ThanhTraLaoDongModel.KienNghiDN
                                kn.NDKienNghi = "Thực hiện đủ số ngày nghỉ hằng năm hưởng nguyên lương " & String.Join(", ", TryCast(arrKN.ToArray(GetType(String)), String())) & " theo"
                                kn.TrichDanId = 25
                                kn.TenBangCauHoi = "Cauhoi5"
                                kn.PhieuId = hidPhieuID.Value
                                data.KienNghiDNs.AddObject(kn)

                            End If
                        End If
                        If (Not IsNothing(q5.Q531) And q5.Q531 >= NgayNghiHangNamCVBT) AndAlso (Not IsNothing(q5.Q532) And q5.Q532 >= NgayNghiHangNamCVDHNN) AndAlso (Not IsNothing(q5.Q533) And q5.Q533 >= NgayNghiHangNamCVDBDHNN) Then
                            kl.NDKetLuan = "Đã thực hiện số ngày nghỉ hàng năm hưởng nguyên lương đúng quy định;"
                            kl.IsViPham = TypeViPham.KhongViPham
                            kl.TenCotCauHoi = "Q53"
                            kl.TenBangCauHoi = "CauHoi5"
                            kl.PhieuId = hidPhieuID.Value
                            data.KetLuans.AddObject(kl)
                        End If
                    End If
                End If

                '5.4. Thực hiện nghỉ việc riêng hưởng nguyên lương lương
                Dim arrKL54 As New ArrayList
                arrKN.Clear()
                kl = New ThanhTraLaoDongModel.KetLuan
                kl.NDKetLuan = "Không thực hiện "
                Dim strKL As String = ""
                Dim iSign As Boolean = False
                Dim iCount As Integer = 0
                If Not IsNothing(q5.Q54) AndAlso Not q5.Q54 Then
                    arrKL54.Add("nghỉ ngày lễ")
                    arrKN.Add("nghỉ ngày lễ")
                    kl.TenCotCauHoi += "Q54;"
                    iCount += 1
                End If
                If Not IsNothing(q5.Q55) AndAlso Not q5.Q55 Then
                    arrKL54.Add("nghỉ việc riêng hưởng nguyên lương")
                    arrKN.Add("nghỉ việc riêng hưởng nguyên lương")
                    kl.TenCotCauHoi += "Q55;"
                    iCount += 1
                End If
                If Not IsNothing(q5.Q541) AndAlso Not q5.Q541.Equals("") Then
                    iSign = True
                    arrKL54.Add("thực hiện nghỉ ngày lễ thiếu " & q5.Q541)
                    arrKN.Add("nghỉ đủ số ngày lễ")
                    strKL = "Thực hiện nghỉ ngày lễ thiếu " & q5.Q541
                    kl.TenCotCauHoi += "Q541"
                End If
                If arrKL54.Count > 0 Then
                    'Xuất kết luận
                    If iCount = 0 And iSign Then
                        kl.NDKetLuan = strKL & ";"
                    Else
                        kl.NDKetLuan += String.Join(", ", TryCast(arrKL54.ToArray(GetType(String)), String())) + " theo quy định;"
                    End If
                    kl.IsViPham = TypeViPham.ViPham
                    kl.TenBangCauHoi = "CauHoi5"
                    kl.PhieuId = hidPhieuID.Value
                    data.KetLuans.AddObject(kl)
                    'Xuất kiến nghị
                    kn = New ThanhTraLaoDongModel.KienNghiDN
                    kn.NDKienNghi = "Thực hiện " & String.Join(", ", TryCast(arrKN.ToArray(GetType(String)), String())) + " theo"
                    kn.TrichDanId = 26
                    kn.TenBangCauHoi = "Cauhoi5"
                    kn.PhieuId = hidPhieuID.Value
                    data.KienNghiDNs.AddObject(kn)

                End If

                'Kết luận chung
                If flag = True Then
                    kl = New ThanhTraLaoDongModel.KetLuan
                    kl.NDKetLuan = "Đã thực hiện thời giờ làm việc, thời giờ nghỉ ngơi đúng quy định;"
                    kl.IsViPham = TypeViPham.KhongViPham
                    kl.TenCotCauHoi = "Q51;Q52;Q53;Q54"
                    kl.TenBangCauHoi = "CauHoi5"
                    kl.PhieuId = hidPhieuID.Value
                    data.KetLuans.AddObject(kl)
                End If
                data.SaveChanges()
            End If
        End Using
    End Sub
    Protected Sub XulyKetLuanCauHoi6()
        Using data As New ThanhTraLaoDongEntities
            Dim kl As KetLuan
            Dim kn As New ThanhTraLaoDongModel.KienNghiDN
            'Tao moi ket luan dua vao cauhoi4
            Dim q6 = (From a In data.CauHoi6
                                 Where a.PhieuId = hidPhieuID.Value).FirstOrDefault
            Dim q2 = (From a In data.CauHoi2
                                 Where a.PhieuId = hidPhieuID.Value).FirstOrDefault
            Dim pn = (From a In data.PhieuNhapHeaders Where a.PhieuID = hidPhieuID.Value).FirstOrDefault
            If Not IsNothing(q6) Then
                'TH: Nếu DN chỉ có 2 loại hợp đồng lao động là HĐLĐ không xác định thời hạn +  HĐLĐ xác định thời hạn từ 12 tháng đến 36 tháng và HĐLĐ xác định thời hạn từ 3 tháng đến dưới 12 tháng bằng 0
                If Not IsNothing(q6.Q612) Then
                    If Not IsNothing(q2) Then
                        If q6.Q611 > q6.Q612 AndAlso (IsNothing(q2.Q213) OrElse (Not IsNothing(q2.Q213) AndAlso q2.Q213 = 0)) Then
                            'Xuất kết luận
                            kl = New ThanhTraLaoDongModel.KetLuan
                            kl.NDKetLuan = "Chưa tham gia bảo hiểm xã hội, bảo hiểm thất nghiệp cho " & String.Format(info, "{0:n0}", (q6.Q611 - q6.Q612)) & "/" & String.Format(info, "{0:n0}", q6.Q611) & " người phải tham gia;"
                            kl.IsViPham = TypeViPham.ViPham
                            kl.TenBangCauHoi = "CauHoi6"
                            kl.TenCotCauHoi = "Q611;Q621"
                            kl.PhieuId = hidPhieuID.Value
                            data.KetLuans.AddObject(kl)
                            'Xuất kiến nghị
                            kn = New ThanhTraLaoDongModel.KienNghiDN
                            kn.NDKienNghi = "Tham gia bảo hiểm xã hội, bảo hiểm thất nghiệp cho " & String.Format(info, "{0:n0}", (q6.Q611 - q6.Q612)) & " người phải tham gia theo"
                            kn.TrichDanId = 71
                            kn.TenBangCauHoi = "Cauhoi6"
                            kn.PhieuId = hidPhieuID.Value
                            data.KienNghiDNs.AddObject(kn)

                        ElseIf q6.Q611 = q6.Q612 AndAlso (IsNothing(q2.Q213) OrElse (Not IsNothing(q2.Q213) AndAlso q2.Q213 = 0)) Then
                            kl = New ThanhTraLaoDongModel.KetLuan
                            kl.NDKetLuan = "Đã tham gia bảo hiểm xã hội, bảo hiểm thất nghiệp cho " & String.Format(info, "{0:n0}", q6.Q612) & "/" & String.Format(info, "{0:n0}", q6.Q611) & " người phải tham gia;"
                            kl.IsViPham = TypeViPham.KhongViPham
                            kl.TenBangCauHoi = "CauHoi6"
                            kl.TenCotCauHoi = "Q612;Q622"
                            kl.PhieuId = hidPhieuID.Value
                            data.KetLuans.AddObject(kl)
                        End If
                    End If
                End If

                '6.1 BHXH bắt buộc
                If Not IsNothing(q6.Q612) Then
                    ' Xét số tham gia < số bắt buộc tham giam không ?
                    If q6.Q611 > q6.Q612 AndAlso (Not IsNothing(q2.Q213) And q2.Q213 > 0) Then
                        'Xuất kết luận
                        kl = New ThanhTraLaoDongModel.KetLuan
                        kl.NDKetLuan = "Chưa tham gia bảo hiểm xã hội cho " & String.Format(info, "{0:n0}", (q6.Q611 - q6.Q612)) & "/" & String.Format(info, "{0:n0}", q6.Q611) & " người phải tham gia;"
                        kl.IsViPham = TypeViPham.ViPham
                        kl.TenBangCauHoi = "CauHoi6"
                        kl.TenCotCauHoi = "Q611"
                        kl.PhieuId = hidPhieuID.Value
                        data.KetLuans.AddObject(kl)
                        'Xuất kiến nghị
                        kn = New ThanhTraLaoDongModel.KienNghiDN
                        kn.NDKienNghi = "Tham gia bảo hiểm xã hội cho " & String.Format(info, "{0:n0}", (q6.Q611 - q6.Q612)) & " người phải tham gia theo"
                        kn.TrichDanId = 27
                        kn.TenBangCauHoi = "Cauhoi6"
                        kn.PhieuId = hidPhieuID.Value
                        data.KienNghiDNs.AddObject(kn)

                    ElseIf q6.Q611 <= q6.Q612 AndAlso (Not IsNothing(q2.Q213) And q2.Q213 > 0) Then
                        kl = New ThanhTraLaoDongModel.KetLuan
                        kl.NDKetLuan = "Đã tham gia bảo hiểm xã hội bắt buộc cho " & String.Format(info, "{0:n0}", q6.Q612) & "/" & String.Format(info, "{0:n0}", q6.Q611) & " người phải tham gia;"
                        kl.IsViPham = TypeViPham.KhongViPham
                        kl.TenBangCauHoi = "CauHoi6"
                        kl.TenCotCauHoi = "Q611"
                        kl.PhieuId = hidPhieuID.Value
                        data.KetLuans.AddObject(kl)
                    End If
                End If

                ' 6.2 Bảo hiểm thất nghiệp
                If Not IsNothing(q6.Q622) Then
                    '' Nếu số tham gia nhỏ hơn sô bắt buộc tham gian
                    If q6.Q622 < q6.Q621 AndAlso (Not IsNothing(q2.Q213) And q2.Q213 > 0) Then
                        'Xuất kết luận
                        kl = New ThanhTraLaoDongModel.KetLuan
                        kl.NDKetLuan = "Chưa tham gia bảo hiểm thất nghiệp cho " & String.Format(info, "{0:n0}", q6.Q621 - q6.Q622) & "/" & String.Format(info, "{0:n0}", q6.Q621) & " người phải tham gia;"
                        kl.IsViPham = TypeViPham.ViPham
                        kl.TenBangCauHoi = "CauHoi6"
                        kl.TenCotCauHoi = "Q621"
                        kl.PhieuId = hidPhieuID.Value
                        data.KetLuans.AddObject(kl)
                        'Xuất kiến nghị
                        kn = New ThanhTraLaoDongModel.KienNghiDN
                        kn.NDKienNghi = "Tham gia bảo hiểm thất nghiệp cho " & String.Format(info, "{0:n0}", q6.Q621 - q6.Q622) & " người phải tham gia theo"
                        kn.TrichDanId = 27
                        kn.TenBangCauHoi = "Cauhoi6"
                        kn.PhieuId = hidPhieuID.Value
                        data.KienNghiDNs.AddObject(kn)

                    ElseIf q6.Q622 >= q6.Q621 AndAlso (Not IsNothing(q2.Q213) And q2.Q213 > 0) Then
                        kl = New ThanhTraLaoDongModel.KetLuan
                        kl.NDKetLuan = "Đã tham gia bảo hiểm thất nghiệp cho " & String.Format(info, "{0:n0}", q6.Q622) & "/" & String.Format(info, "{0:n0}", q6.Q621) & " người phải tham gia;"
                        kl.IsViPham = TypeViPham.KhongViPham
                        kl.TenBangCauHoi = "CauHoi6"
                        kl.TenCotCauHoi = "Q621"
                        kl.PhieuId = hidPhieuID.Value
                        data.KetLuans.AddObject(kl)
                    End If

                End If
                If Not IsNothing(q6.Q612) Then
                    ' Xét sô người được cấp (6.3)/ sô người tham gian BHXH (6.2.2)
                    If Not IsNothing(q6.Q63) And Not IsNothing(q6.Q612) Then
                        If q6.Q63 >= q6.Q612 Then
                            kl = New ThanhTraLaoDongModel.KetLuan
                            kl.NDKetLuan = "Đã làm thủ tục cấp sổ bảo hiểm xã hội cho " & String.Format(info, "{0:n0}", q6.Q63) & " người;"
                            kl.IsViPham = TypeViPham.KhongViPham
                            kl.TenBangCauHoi = "CauHoi6"
                            kl.TenCotCauHoi = "Q63"
                            kl.PhieuId = hidPhieuID.Value
                            data.KetLuans.AddObject(kl)
                        End If
                    End If
                End If
                'Số người ký HĐLĐ trên 01 tháng chưa làm thủ tục cấp sổ BHXH
                If Not IsNothing(q6.Q631) And q6.Q631 > 0 Then
                    'Xuất kết luận
                    kl = New ThanhTraLaoDongModel.KetLuan
                    kl.NDKetLuan = "Chưa làm thủ tục tham gia và cấp sổ cho " & String.Format(info, "{0:n0}", q6.Q631) & " người đã ký hợp đồng lao động trên 01 tháng phải tham gia bảo hiểm xã hội bắt buộc;"
                    kl.IsViPham = TypeViPham.ViPham
                    kl.TenBangCauHoi = "CauHoi6"
                    kl.TenCotCauHoi = "Q631"
                    kl.PhieuId = hidPhieuID.Value
                    data.KetLuans.AddObject(kl)
                    'Xuất kiến nghị
                    kn = New ThanhTraLaoDongModel.KienNghiDN
                    kn.NDKienNghi = "Hoàn thành thủ tục tham gia và cấp sổ cho " & String.Format(info, "{0:n0}", (q6.Q612 - q6.Q63)) & " người chưa có sổ bảo hiểm xã hội theo"
                    kn.TrichDanId = 30
                    kn.TenBangCauHoi = "Cauhoi6"
                    kn.PhieuId = hidPhieuID.Value
                    data.KienNghiDNs.AddObject(kn)
                End If
                ' 6.4 Chậm đóng tiền bảo hiểm xã hội
                If Not IsNothing(q6.Q641) Then
                    'Xuất kết luận
                    kl = New ThanhTraLaoDongModel.KetLuan
                    kl.NDKetLuan = "Chậm đóng tiền bảo hiểm xã hội đến tháng " & _
                        IIf(IsNothing(pn.NgayKetThucPhieu), "", CType(pn.NgayKetThucPhieu, Date).Month.ToString) & "/" & _
                        IIf(IsNothing(pn.NgayKetThucPhieu), "", CType(pn.NgayKetThucPhieu, Date).Year.ToString) & _
                        " tính tròn số là " & String.Format(info, "{0:n0}", q6.Q641) & " triệu đồng;"
                    kl.IsViPham = TypeViPham.ViPham
                    kl.TenBangCauHoi = "CauHoi6"
                    kl.TenCotCauHoi = "Q641"
                    kl.PhieuId = hidPhieuID.Value
                    data.KetLuans.AddObject(kl)
                    'Xuất kiến nghị
                    kn = New ThanhTraLaoDongModel.KienNghiDN
                    kn.NDKienNghi = "Nộp ngay số tiền chậm đóng bảo hiểm xã hội đến tháng " & _
                        IIf(IsNothing(pn.NgayKetThucPhieu), "", CType(pn.NgayKetThucPhieu, Date).Month.ToString) & "/" & _
                        IIf(IsNothing(pn.NgayKetThucPhieu), "", CType(pn.NgayKetThucPhieu, Date).Year.ToString) & _
                        " tính tròn số là " & String.Format(info, "{0:n0}", q6.Q641) & " triệu đồng theo"
                    kn.TrichDanId = 29
                    kn.TenBangCauHoi = "Cauhoi6"
                    kn.PhieuId = hidPhieuID.Value
                    data.KienNghiDNs.AddObject(kn)
                Else
                    kl = New ThanhTraLaoDongModel.KetLuan
                    kl.NDKetLuan = "Đã đóng tiền bảo hiểm xã hội đầy đủ;"
                    kl.IsViPham = TypeViPham.KhongViPham
                    kl.TenBangCauHoi = "CauHoi6"
                    kl.TenCotCauHoi = "Q641"
                    kl.PhieuId = hidPhieuID.Value
                    data.KetLuans.AddObject(kl)
                End If
                ''6.5 Số sổ BHXH chưa trả cho người thôi việc
                If Not IsNothing(q6.Q65) Then
                    'Xuất kết luận
                    kl = New ThanhTraLaoDongModel.KetLuan
                    kl.NDKetLuan = "Chưa trả sổ bảo hiểm xã hội cho " & q6.Q65 & " người thôi việc" & IIf(String.IsNullOrEmpty(q6.Q651), ";", ", (lý do " & q6.Q651 & ");")
                    kl.IsViPham = TypeViPham.ViPham
                    kl.TenBangCauHoi = "CauHoi6"
                    kl.TenCotCauHoi = "Q65"
                    kl.PhieuId = hidPhieuID.Value
                    data.KetLuans.AddObject(kl)
                    'Xuất kiến nghị
                    kn = New ThanhTraLaoDongModel.KienNghiDN
                    kn.NDKienNghi = "Trả sổ bảo hiểm xã hội cho " & q6.Q65 & " người thôi việc theo"
                    kn.TrichDanId = 30
                    kn.TenBangCauHoi = "Cauhoi6"
                    kn.PhieuId = hidPhieuID.Value
                    data.KienNghiDNs.AddObject(kn)

                End If
                '6.7. Trả tiền bảo hiểm xã hội vào lương và ngày nghỉ phép: 
                If Not IsNothing(q6.Q67) Then
                    If q6.Q67 = 1 Then
                        kl = New ThanhTraLaoDongModel.KetLuan
                        kl.NDKetLuan = "Đã trả thêm tiền bảo hiểm xã hội, bảo hiểm y tế và tiền ngày nghỉ phép vào lương đối với lao động không phải tham gia bảo hiểm xã hội bắt buộc;"
                        kl.IsViPham = TypeViPham.KhongViPham
                        kl.TenBangCauHoi = "CauHoi6"
                        kl.TenCotCauHoi = "Q67"
                        kl.PhieuId = hidPhieuID.Value
                        data.KetLuans.AddObject(kl)
                    ElseIf q6.Q67 = 0 Then
                        'Xuất kết luận
                        kl = New ThanhTraLaoDongModel.KetLuan
                        kl.NDKetLuan = "Chưa trả thêm cùng với kỳ lương đối với lao động không phải tham gia bảo hiểm xã hội bắt buộc khoản tiền tương đương với đóng bảo hiểm xã hội, bảo hiểm y tế và tiền ngày nghỉ phép hằng năm theo quy định;"
                        kl.IsViPham = TypeViPham.ViPham
                        kl.TenBangCauHoi = "CauHoi6"
                        kl.TenCotCauHoi = "Q67"
                        kl.PhieuId = hidPhieuID.Value
                        data.KetLuans.AddObject(kl)
                        'Xuất kiến nghị
                        kn = New ThanhTraLaoDongModel.KienNghiDN
                        kn.NDKienNghi = "Trả thêm cùng với kỳ lương đối với lao động không phải tham gia bảo hiểm xã hội bắt buộc khoản tiền tương đương với đóng bảo hiểm xã hội, bảo hiểm y tế và tiền ngày nghỉ phép hằng năm theo"
                        kn.TrichDanId = 58
                        kn.TenBangCauHoi = "Cauhoi6"
                        kn.PhieuId = hidPhieuID.Value
                        data.KienNghiDNs.AddObject(kn)

                    End If
                End If
                If Not IsNothing(q6.Q671) AndAlso Not q6.Q671.Equals("") Then
                    'Xuất kết luận
                    kl = New ThanhTraLaoDongModel.KetLuan
                    kl.NDKetLuan = "Trả tiền bảo hiểm xã hội, bảo hiểm y tế, bảo hiểm thất nghiệp và tiền ngày nghỉ phép vào lương đối với lao động không phải tham gia bảo hiểm xã hội bắt buộc chưa đủ mức quy định (" + q6.Q671 + ");"
                    kl.IsViPham = TypeViPham.ViPham
                    kl.TenBangCauHoi = "CauHoi6"
                    kl.TenCotCauHoi = "Q671"
                    kl.PhieuId = hidPhieuID.Value
                    data.KetLuans.AddObject(kl)
                    'Xuất kiến nghị
                    kn = New ThanhTraLaoDongModel.KienNghiDN
                    kn.NDKienNghi = "Trả thêm cùng với kỳ lương đối với lao động không phải tham gia bảo hiểm xã hội bắt buộc khoản tiền tương đương với mức đóng bảo hiểm xã hội, bảo hiểm y tế, bảo hiểm thất nghiệp và tiền ngày nghỉ phép hằng năm theo"
                    kn.TrichDanId = 58
                    kn.TenBangCauHoi = "Cauhoi6"
                    kn.PhieuId = hidPhieuID.Value
                    data.KienNghiDNs.AddObject(kn)

                End If
                '6.8 Làm thủ tục thanh toán các chế độ bảo hiểm xã hội đầy đủ, kịp thời
                If Not IsNothing(q6.Q68) AndAlso Not q6.Q68 Then
                    'Xuất kết luận
                    kl = New ThanhTraLaoDongModel.KetLuan
                    kl.NDKetLuan = "Làm thủ tục thanh toán các chế độ bảo hiểm xã hội cho người lao động chưa đầy đủ, kịp thời (" & q6.Q681 & ") theo quy định;"
                    kl.IsViPham = TypeViPham.ViPham
                    kl.TenBangCauHoi = "CauHoi6"
                    kl.TenCotCauHoi = "Q68"
                    kl.PhieuId = hidPhieuID.Value
                    data.KetLuans.AddObject(kl)
                    'Xuất kiến nghị
                    kn = New ThanhTraLaoDongModel.KienNghiDN
                    kn.NDKienNghi = "Làm thủ tục thanh toán các chế độ bảo hiểm xã hội cho người lao động đầy đủ, kịp thời theo"
                    kn.TrichDanId = 77
                    kn.TenBangCauHoi = "Cauhoi6"
                    kn.PhieuId = hidPhieuID.Value
                    data.KienNghiDNs.AddObject(kn)
                End If
                data.SaveChanges()
            End If
        End Using
    End Sub
    Protected Sub XulyKetLuanCauHoi7()
        Using data As New ThanhTraLaoDongEntities
            Dim kl As New ThanhTraLaoDongModel.KetLuan
            Dim kn As New ThanhTraLaoDongModel.KienNghiDN
            'Tao moi ket luan dua vao cauhoi10
            Dim q7 = (From a In data.CauHoi7
                    Where a.PhieuId = hidPhieuID.Value).FirstOrDefault
            'Lấy thông tin số người ld của doanh nghiệp để xét số lượng cán bộ có đúng hay không?
            Dim pn = (From p In data.PhieuNhapHeaders
                      Where p.PhieuID = hidPhieuID.Value).FirstOrDefault

            If Not IsNothing(q7) Then
                '7.1. Tổ chức bộ máy làm công tác an toàn; 7.2. Phân định trách nhiệm về an toàn vệ sinh lao động
                If Not IsNothing(q7.Q701) AndAlso q7.Q701 > 0 Then
                    If Not IsNothing(q7.Q7011) Or Not IsNothing(q7.Q7012) Or Not IsNothing(q7.Q7013) Or Not IsNothing(q7.Q7014) Then
                        kl = New ThanhTraLaoDongModel.KetLuan
                        kl.NDKetLuan = "Đã bố trí " & q7.Q701 & " người làm công tác an toàn, vệ sinh lao động "
                        Dim arrKL As New ArrayList
                        If Not IsNothing(q7.Q7011) Then
                            If q7.Q7011 > 0 Then
                                arrKL.Add(q7.Q7011 & " cán bộ chuyên trách an toàn")
                            End If
                        End If
                        If Not IsNothing(q7.Q7012) Then
                            If q7.Q7012 > 0 Then
                                arrKL.Add(q7.Q7012 & " cán bộ y tế")
                            End If
                        End If
                        If Not IsNothing(q7.Q7013) Then
                            If q7.Q7013 > 0 Then
                                arrKL.Add(q7.Q7013 & " An toàn viên")
                            End If
                        End If
                        If Not IsNothing(q7.Q7014) Then
                            If q7.Q7014 > 0 Then
                                arrKL.Add("Hội đồng bảo hộ lao động " & q7.Q7014 & " người")
                            End If
                        End If
                        If arrKL.Count > 0 Then
                            kl.NDKetLuan += "(" & String.Join(", ", TryCast(arrKL.ToArray(GetType(String)), String())) + ");"
                        Else
                            kl.NDKetLuan += ";"
                        End If
                        kl.IsViPham = TypeViPham.KhongViPham
                        kl.TenBangCauHoi = "CauHoi7"
                        kl.TenCotCauHoi = "Q7011;Q7012;Q7013;Q7014"
                        kl.PhieuId = hidPhieuID.Value
                        data.KetLuans.AddObject(kl)
                    End If
                    'Xét trường hợp vi phạm
                    'Cán bộ chuyên trách an toàn
                    If Not IsNothing(q7.Q7011) AndAlso q7.Q7011 > 0 Then
                        kl = New ThanhTraLaoDongModel.KetLuan
                        'kl.NDKetLuan = "Đã bố trí " & String.Format(info, "{0:n0}", q7.Q7011) & " cán bộ chuyên trách làm an toàn vệ sinh lao động."
                        kl.IsViPham = IIf(((pn.TongSoNhanVien >= 300 And pn.TongSoNhanVien <= 1000) And q7.Q7011 >= 1) _
                                          Or (pn.TongSoNhanVien > 1000 And q7.Q7011 >= 2) Or (pn.TongSoNhanVien < 300), _
                                          TypeViPham.KhongViPham, TypeViPham.ViPham)
                        If kl.IsViPham = 0 Then
                            'Xuất kết luận
                            kl.NDKetLuan = "Bố trí cán bộ chuyên trách làm an toàn vệ sinh lao động chưa đủ theo quy định;"
                            kl.TenBangCauHoi = "CauHoi7"
                            kl.TenCotCauHoi = "Q7011"
                            kl.PhieuId = hidPhieuID.Value
                            data.KetLuans.AddObject(kl)
                            'Xuất kiến nghị
                            kn = New ThanhTraLaoDongModel.KienNghiDN
                            kn.NDKienNghi = "Bố trí cán bộ chuyên trách làm an toàn vệ sinh lao động đủ theo"
                            kn.TrichDanId = 67
                            kn.TenBangCauHoi = "Cauhoi7"
                            kn.PhieuId = hidPhieuID.Value
                            data.KienNghiDNs.AddObject(kn)

                        End If
                    End If
                    'Cán bộ y tế
                    If Not IsNothing(q7.Q7012) AndAlso q7.Q7012 > 0 Then
                        kl = New ThanhTraLaoDongModel.KetLuan
                        'kl.NDKetLuan = "Đã bố trí " & String.Format(info, "{0:n0}", q7.Q7012) & " cán bộ y tế."
                        kl.IsViPham = IIf((pn.TongSoNhanVien > 500 And q7.Q7012 < 1), TypeViPham.ViPham, TypeViPham.KhongViPham)
                        If kl.IsViPham = 0 Then
                            'Xuất kết luận
                            kl.NDKetLuan = "Chưa bố trí cán bộ y tế;"
                            kl.TenBangCauHoi = "CauHoi7"
                            kl.TenCotCauHoi = "Q7012"
                            kl.PhieuId = hidPhieuID.Value
                            data.KetLuans.AddObject(kl)
                            'Xuất kiến nghị
                            kn = New ThanhTraLaoDongModel.KienNghiDN
                            kn.NDKienNghi = "Bố trí cán bộ y tế đủ theo"
                            kn.TrichDanId = 67
                            kn.TenBangCauHoi = "Cauhoi7"
                            kn.PhieuId = hidPhieuID.Value
                            data.KienNghiDNs.AddObject(kn)

                        End If
                    End If
                    'Hội đồng bảo hộ lao động
                    kl = New ThanhTraLaoDongModel.KetLuan
                    kl.IsViPham = IIf((pn.TongSoNhanVien > 1000 AndAlso pn.IsCongDoan AndAlso Not IsNothing(q7.Q7014) AndAlso q7.Q7014 <= 3), TypeViPham.ViPham, TypeViPham.KhongViPham)
                    If kl.IsViPham = 0 Then
                        'Xuất kết luận
                        kl.NDKetLuan = "Chưa thành lập Hội đồng bảo hộ lao động;"
                        kl.TenBangCauHoi = "CauHoi7"
                        kl.TenCotCauHoi = "Q7014"
                        kl.PhieuId = hidPhieuID.Value
                        data.KetLuans.AddObject(kl)
                        'Xuất kiến nghị
                        kn = New ThanhTraLaoDongModel.KienNghiDN
                        kn.NDKienNghi = "Thành lập Hội đồng bảo hộ lao động theo"
                        kn.TrichDanId = 68
                        kn.TenBangCauHoi = "Cauhoi7"
                        kn.PhieuId = hidPhieuID.Value
                        data.KienNghiDNs.AddObject(kn)

                    End If
                    'Hợp đồng với Tổ chức dịch vụ an toàn lao động
                    If IsNothing(q7.Q7011) OrElse Not IsNothing(q7.Q7011) AndAlso q7.Q7011 = 0 Then
                        If Not IsNothing(q7.Q7015) AndAlso q7.Q7015 Then
                            kl = New ThanhTraLaoDongModel.KetLuan
                            kl.NDKetLuan = "Đã ký hợp đồng với tổ chức dịch vụ an toàn vệ sinh lao động;"
                            kl.IsViPham = TypeViPham.KhongViPham
                            kl.TenBangCauHoi = "CauHoi7"
                            kl.TenCotCauHoi = "Q7015"
                            kl.PhieuId = hidPhieuID.Value
                            data.KetLuans.AddObject(kl)
                        End If
                        If Not IsNothing(q7.Q7015) AndAlso Not q7.Q7015 And q7.Q701 > 1000 Then
                            kl = New ThanhTraLaoDongModel.KetLuan
                            kl.NDKetLuan = "Chưa thành lập bộ phận An toàn vệ sinh lao động hoặc hợp đồng với tổ chức dịch vụ đủ năng lực làm công tác an toàn vệ sinh lao động;"
                            kl.IsViPham = TypeViPham.ViPham
                            kl.TenBangCauHoi = "CauHoi7"
                            kl.TenCotCauHoi = "Q7015"
                            kl.PhieuId = hidPhieuID.Value
                            data.KetLuans.AddObject(kl)
                            'Xuất kiến nghị
                            kn = New ThanhTraLaoDongModel.KienNghiDN
                            kn.NDKienNghi = "Thành lập bộ phận An toàn vệ sinh lao động hoặc hợp đồng với đơn vị dịch vụ đủ năng lực làm công tác an toàn vệ sinh lao động theo"
                            kn.TrichDanId = 31
                            kn.TenBangCauHoi = "Cauhoi7"
                            kn.PhieuId = hidPhieuID.Value
                            data.KienNghiDNs.AddObject(kn)

                        End If
                    End If
                    'Hợp đồng chăm sóc sức khỏe với cơ sở Y tế địa phương
                    If IsNothing(q7.Q7012) OrElse Not IsNothing(q7.Q7012) AndAlso q7.Q7012 = 0 Then
                        If Not IsNothing(q7.Q7016) AndAlso q7.Q7016 Then
                            kl = New ThanhTraLaoDongModel.KetLuan
                            kl.NDKetLuan = "Đã ký hợp đồng chăm sóc sức khỏe với cơ sở Y tế địa phương;"
                            kl.IsViPham = TypeViPham.KhongViPham
                            kl.TenBangCauHoi = "CauHoi7"
                            kl.TenCotCauHoi = "Q7016"
                            kl.PhieuId = hidPhieuID.Value
                            data.KetLuans.AddObject(kl)
                        End If
                        If Not IsNothing(q7.Q7016) AndAlso Not q7.Q7016 And q7.Q701 > 1000 Then
                            kl = New ThanhTraLaoDongModel.KetLuan
                            kl.NDKetLuan = "Chưa thành lập bộ phận Y tế doanh nghiệp hoặc hợp đồng chăm sóc sức khỏe với cơ sở Y tế địa phương;"
                            kl.IsViPham = TypeViPham.ViPham
                            kl.TenBangCauHoi = "CauHoi7"
                            kl.TenCotCauHoi = "Q7016"
                            kl.PhieuId = hidPhieuID.Value
                            data.KetLuans.AddObject(kl)
                            'Xuất kiến nghị
                            kn = New ThanhTraLaoDongModel.KienNghiDN
                            kn.NDKienNghi = "Thành lập bộ phận Y tế hoặc ký hợp đồng chăm sóc sức khỏe với cơ sở Y tế địa phương theo"
                            kn.TrichDanId = 31
                            kn.TenBangCauHoi = "Cauhoi7"
                            kn.PhieuId = hidPhieuID.Value
                            data.KienNghiDNs.AddObject(kn)
                        End If
                    End If
                ElseIf IsNothing(q7.Q701) OrElse (Not IsNothing(q7.Q701) AndAlso q7.Q701 = 0) Then
                    'Xuất kết luận
                    kl = New ThanhTraLaoDongModel.KetLuan
                    kl.NDKetLuan = "Doanh nghiệp chưa bố trí cán bộ làm công tác an toàn, vệ sinh lao động;"
                    kl.IsViPham = TypeViPham.ViPham
                    kl.TenBangCauHoi = "CauHoi7"
                    kl.TenCotCauHoi = "Q701"
                    kl.PhieuId = hidPhieuID.Value
                    data.KetLuans.AddObject(kl)
                    'Xuất kiến nghị
                    kn = New ThanhTraLaoDongModel.KienNghiDN
                    kn.NDKienNghi = "Bố trí đủ cán bộ làm công tác an toàn, vệ sinh lao động theo"
                    kn.TrichDanId = 68
                    kn.TenBangCauHoi = "Cauhoi7"
                    kn.PhieuId = hidPhieuID.Value
                    data.KienNghiDNs.AddObject(kn)
                End If

                '7.2
                If Not IsNothing(q7.Q702) Then
                    'Xuất kết luận
                    kl = New ThanhTraLaoDongModel.KetLuan
                    kl.NDKetLuan = IIf(q7.Q702, "Đã", "Chưa") & " thực hiện phân định trách nhiệm về công tác an toàn - vệ sinh lao động cho các cán bộ quản lý, đến từng bộ phận chuyên môn nghiệp vụ theo quy định;"
                    kl.IsViPham = IIf(q7.Q702, TypeViPham.KhongViPham, TypeViPham.ViPham)
                    kl.TenBangCauHoi = "CauHoi7"
                    kl.TenCotCauHoi = "Q702"
                    kl.PhieuId = hidPhieuID.Value
                    data.KetLuans.AddObject(kl)
                    'Xuất kiến nghị
                    If kl.IsViPham = 0 Then
                        kn = New ThanhTraLaoDongModel.KienNghiDN
                        kn.NDKienNghi = "Thực hiện phân định trách nhiệm về công tác an toàn - vệ sinh lao động cho các cán bộ quản lý, đến từng bộ phận chuyên môn nghiệp vụ theo"
                        kn.TrichDanId = 68
                        kn.TenBangCauHoi = "Cauhoi7"
                        kn.PhieuId = hidPhieuID.Value
                        data.KienNghiDNs.AddObject(kn)
                    End If
                End If
                'Thống kê số người làm công việc độc hại
                If Not IsNothing(q7.Q7021) AndAlso q7.Q7021 = 0 Then
                    'Xuất kết luận
                    kl = New ThanhTraLaoDongModel.KetLuan
                    kl.NDKetLuan = "Chưa thống kê, phân loại và thực hiện các chế độ liên quan đối với người lao động làm các chức danh công việc nặng nhọc độc hại, nguy hiểm;"
                    kl.IsViPham = TypeViPham.ViPham
                    kl.TenBangCauHoi = "CauHoi7"
                    kl.TenCotCauHoi = "Q7021"
                    kl.PhieuId = hidPhieuID.Value
                    data.KetLuans.AddObject(kl)
                    'Xuất kiến nghị
                    kn = New ThanhTraLaoDongModel.KienNghiDN
                    kn.NDKienNghi = "Thống kê, phân loại và thực hiện các chế độ liên quan đối với người lao động làm các công việc nặng nhọc, độc hại, nguy hiểm theo"
                    kn.TrichDanId = 79
                    kn.TenBangCauHoi = "Cauhoi7"
                    kn.PhieuId = hidPhieuID.Value
                    data.KienNghiDNs.AddObject(kn)
                End If
                'Không đầy đủ
                If Not IsNothing(q7.Q7021) AndAlso q7.Q7021 = 2 Then
                    'Xuất kết luận
                    kl = New ThanhTraLaoDongModel.KetLuan
                    kl.NDKetLuan = "Thống kê, phân loại chức danh nghề, công việc, nặng nhọc, độc hại, nguy hiểm để thực hiện các chế độ liên quan chưa đầy đủ;"
                    kl.IsViPham = TypeViPham.ViPham
                    kl.TenBangCauHoi = "CauHoi7"
                    kl.TenCotCauHoi = "Q7021"
                    kl.PhieuId = hidPhieuID.Value
                    data.KetLuans.AddObject(kl)
                    'Xuất kiến nghị
                    kn = New ThanhTraLaoDongModel.KienNghiDN
                    kn.NDKienNghi = "Thống kê, phân loại và thực hiện các chế độ liên quan đầy đủ đối với các nghề, công việc, nặng nhọc, độc hại, nguy hiểm theo"
                    kn.TrichDanId = 79
                    kn.TenBangCauHoi = "Cauhoi7"
                    kn.PhieuId = hidPhieuID.Value
                    data.KienNghiDNs.AddObject(kn)
                End If
                '7.3. Xây dựng kế hoạch Công tác an toàn vệ sinh lao động hàng năm;
                If Not IsNothing(q7.Q703) Then
                    kl = New ThanhTraLaoDongModel.KetLuan
                    Select Case q7.Q703
                        Case 1
                            kl.NDKetLuan = "Đã xây dựng kế hoạch an toàn vệ sinh lao động hàng năm đúng quy định;"
                            kl.IsViPham = TypeViPham.KhongViPham
                            kl.TenCotCauHoi = "Q703"
                        Case 2
                            'Xử lý kết luận
                            kl.NDKetLuan = "Chưa xây dựng kế hoạch an toàn vệ sinh lao động;"
                            kl.IsViPham = TypeViPham.ViPham
                            kl.TenCotCauHoi = "Q703"
                            'Xuất kiến nghị
                            kn = New ThanhTraLaoDongModel.KienNghiDN
                            kn.NDKienNghi = "Xây dựng kế hoạch an toàn vệ sinh lao động theo"
                            kn.TrichDanId = 67
                            kn.TenBangCauHoi = "Cauhoi7"
                            kn.PhieuId = hidPhieuID.Value
                            data.KienNghiDNs.AddObject(kn)
                        Case Else
                            'Xử lý kết luận
                            kl.NDKetLuan = "Kế hoạch an toàn vệ sinh lao động còn thiếu nội dung: " & IIf(String.IsNullOrEmpty(q7.Q7031), ";", q7.Q7031 & ";")
                            kl.IsViPham = TypeViPham.ViPham
                            kl.TenCotCauHoi = "Q703;"
                            'Xuất kiến nghị
                            kn = New ThanhTraLaoDongModel.KienNghiDN
                            kn.NDKienNghi = "Xây dựng kế hoạch an toàn vệ sinh lao động hàng năm đủ nội dung theo"
                            kn.TrichDanId = 68
                            kn.TenBangCauHoi = "Cauhoi7"
                            kn.PhieuId = hidPhieuID.Value
                            data.KienNghiDNs.AddObject(kn)

                    End Select
                    kl.TenBangCauHoi = "CauHoi7"
                    kl.PhieuId = hidPhieuID.Value
                    data.KetLuans.AddObject(kl)
                End If
                '' 7.4. Xây dựng quy trình, biện pháp an toàn
                If Not IsNothing(q7.Q704) Then
                    kl = New ThanhTraLaoDongModel.KetLuan
                    Select Case q7.Q704
                        Case 1
                            kl.NDKetLuan = "Đã xây dựng đầy đủ quy trình, biện pháp an toàn đúng quy định;"
                            kl.IsViPham = TypeViPham.KhongViPham
                            kl.TenCotCauHoi = "Q704"
                        Case 2
                            'Xử lý kết luận
                            kl.NDKetLuan = "Chưa xây dựng quy trình, biện pháp an toàn theo quy định;"
                            kl.IsViPham = TypeViPham.ViPham
                            kl.TenCotCauHoi = "Q704"
                            'Xuất kiến nghị
                            kn = New ThanhTraLaoDongModel.KienNghiDN
                            kn.NDKienNghi = "Xây dựng đầy đủ quy trình làm việc, biện pháp an toàn phù hợp với các máy, thiết bị, công việc có yếu tố nguy hiểm theo"
                            kn.TrichDanId = 32
                            kn.TenBangCauHoi = "Cauhoi7"
                            kn.PhieuId = hidPhieuID.Value
                            data.KienNghiDNs.AddObject(kn)

                        Case Else
                            'Xử lý kết luận
                            kl.NDKetLuan = "Còn thiếu quy trình, biện pháp an toàn " & IIf(String.IsNullOrEmpty(q7.Q7041), ";", q7.Q7041 & ";")
                            kl.IsViPham = TypeViPham.ViPham
                            kl.TenCotCauHoi = "Q704"
                            'Xuất kiến nghị
                            kn = New ThanhTraLaoDongModel.KienNghiDN
                            kn.NDKienNghi = "Xây dựng bổ sung quy trình làm việc, biện pháp an toàn phù hợp với " & IIf(String.IsNullOrEmpty(q7.Q7041), "", q7.Q7041) & " theo"
                            kn.TrichDanId = 72
                            kn.TenBangCauHoi = "Cauhoi7"
                            kn.PhieuId = hidPhieuID.Value
                            data.KienNghiDNs.AddObject(kn)

                    End Select
                    kl.TenBangCauHoi = "CauHoi7"
                    kl.PhieuId = hidPhieuID.Value
                    data.KetLuans.AddObject(kl)
                End If

                '7.5. Kiểm định thiết bị:
                If Not IsNothing(q7.Q7051) And Not IsNothing(q7.Q7052) Then
                    kl = New ThanhTraLaoDongModel.KetLuan
                    kl.NDKetLuan = "Đã kiểm định " & String.Format(info, "{0:n0}", q7.Q7051) & "/" & String.Format(info, "{0:n0}", (q7.Q7051 + q7.Q7052)) & " thiết bị có yêu cầu nghiêm ngặt về an toàn lao động theo quy định;"
                    kl.IsViPham = TypeViPham.KhongViPham
                    kl.TenBangCauHoi = "CauHoi7"
                    kl.TenCotCauHoi = "Q7051"
                    kl.PhieuId = hidPhieuID.Value
                    data.KetLuans.AddObject(kl)
                End If

                '' Số thiết bị chưa kiểm định
                If Not IsNothing(q7.Q7052) Then
                    'Xuất kết luận
                    kl = New ThanhTraLaoDongModel.KetLuan
                    kl.NDKetLuan = "Chưa kiểm định " & String.Format(info, "{0:n0}", q7.Q7052) & " thiết bị có yêu cầu nghiêm ngặt về an toàn lao động theo quy định" & IIf(String.IsNullOrEmpty(q7.Q7053), ";", " (" & q7.Q7053 & ");")
                    kl.IsViPham = TypeViPham.ViPham
                    kl.TenBangCauHoi = "CauHoi7"
                    kl.TenCotCauHoi = "Q7052"
                    kl.PhieuId = hidPhieuID.Value
                    data.KetLuans.AddObject(kl)
                    'Xuất kiến nghị
                    kn = New ThanhTraLaoDongModel.KienNghiDN
                    kn.NDKienNghi = "Kiểm định kỹ thuật an toàn đối với " & String.Format(info, "{0:n0}", q7.Q7052) & " thiết bị có yêu cầu nghiêm ngặt về an toàn lao động theo"
                    kn.TrichDanId = 33
                    kn.TenBangCauHoi = "Cauhoi7"
                    kn.PhieuId = hidPhieuID.Value
                    kn.IsKNPhaiTH = True
                    data.KienNghiDNs.AddObject(kn)
                End If

                '7.6. Huấn luyện an toàn lao động, vệ sinh lao động;7.7. Hồ sơ huấn luyện:;7.8. Nội dung huấn luyện:
                'Huấn luyện cho cán bộ lao động
                If Not IsNothing(q7.Q706) AndAlso q7.Q706 Then
                    If Not IsNothing(q7.Q70611) Then
                        kl = New ThanhTraLaoDongModel.KetLuan
                        kl.NDKetLuan = "Đã có " & String.Format(info, "{0:n0}", q7.Q70611) & " cán bộ quản lý được huấn luyện về an toàn vệ sinh lao động;"
                        kl.IsViPham = TypeViPham.KhongViPham
                        kl.TenBangCauHoi = "CauHoi7"
                        kl.TenCotCauHoi = "Q70611"
                        kl.PhieuId = hidPhieuID.Value
                        data.KetLuans.AddObject(kl)
                    End If

                    '- Cho người lao động:
                    If Not IsNothing(q7.Q706211) AndAlso q7.Q706211 _
                        AndAlso (Not IsNothing(q7.Q706221) AndAlso q7.Q706221) _
                        AndAlso (Not IsNothing(q7.Q706232) AndAlso pn.SoNguoiLamCongViecYeuCauNghiemNgat = q7.Q706232) _
                        AndAlso (Not IsNothing(q7.Q707) AndAlso q7.Q707) _
                        AndAlso (Not IsNothing(q7.Q708) AndAlso q7.Q708) Then
                        kl = New ThanhTraLaoDongModel.KetLuan
                        kl.NDKetLuan = "Đã thực hiện huấn luyện và cấp thẻ an toàn lao động cho người lao động đầy đủ;"
                        kl.IsViPham = TypeViPham.KhongViPham
                        kl.TenBangCauHoi = "CauHoi7"
                        kl.TenCotCauHoi = "Q706211;Q706221;Q706232"
                        kl.PhieuId = hidPhieuID.Value
                        data.KetLuans.AddObject(kl)
                    Else
                        'Liệt kê
                        '7.6. Huấn luyện an toàn lao động, vệ sinh lao động; 
                        kl = New ThanhTraLaoDongModel.KetLuan
                        kl.NDKetLuan = "Tổ chức huấn luyện an toàn vệ sinh lao động cho người lao động "
                        Dim strCombine = ""
                        Dim arrCombine As New ArrayList
                        Dim arrKN As New ArrayList
                        Dim strTenCotCauHoi As String = ""
                        '7.6. Huấn luyện an toàn lao động, vệ sinh lao động;
                        If Not IsNothing(q7.Q706212) AndAlso q7.Q706212 > 0 Then
                            arrCombine.Add("lần đầu thiếu " & String.Format(info, "{0:n0}", q7.Q706212) & " người")
                            strTenCotCauHoi = "Q706212;"
                            arrKN.Add(String.Format(info, "{0:n0}", q7.Q706212) & " người chưa được huấn luyện lần đầu")
                        End If
                        If Not IsNothing(q7.Q706222) AndAlso q7.Q706222 > 0 Then
                            arrCombine.Add("định kỳ thiếu " & String.Format(info, "{0:n0}", q7.Q706222) & " người")
                            strTenCotCauHoi += "Q706222;"
                            arrKN.Add(String.Format(info, "{0:n0}", q7.Q706222) & " người chưa được huấn luyện định kỳ")
                        End If

                        Dim strKL As String = ""
                        If Not IsNothing(q7.Q706232) AndAlso q7.Q706232 > 0 Then
                            If Not IsNothing(pn) Then
                                If Not IsNothing(pn.SoNguoiLamCongViecYeuCauNghiemNgat) AndAlso pn.SoNguoiLamCongViecYeuCauNghiemNgat > q7.Q706232 Then
                                    strKL = "; chưa tổ chức huấn luyện " & String.Format(info, "{0:n0}", pn.SoNguoiLamCongViecYeuCauNghiemNgat - q7.Q706232) & " người làm công việc có yêu cầu nghiêm ngặt về an toàn theo quy định"
                                    strTenCotCauHoi += "Q706232;"
                                    arrKN.Add("Thực hiện huấn luyện cho " & String.Format(info, "{0:n0}", pn.SoNguoiLamCongViecYeuCauNghiemNgat - q7.Q706232) & " người làm công việc về an toàn vệ sinh lao động theo đúng quy định.")

                                    'strKL = "; chưa cấp thẻ an toàn vệ sinh lao động cho " & String.Format(info, "{0:n0}", pn.SoNguoiLamCongViecYeuCauNghiemNgat - q7.Q706232) & " người"
                                    'strTenCotCauHoi += "Q706232;"
                                    'arrKN.Add(String.Format(info, "{0:n0}", q7.Q706232) & " người làm công việc yêu cầu nghiêm ngặt về an toàn lao động chưa cấp thẻ an toàn")
                                End If
                            End If
                        End If

                        If arrCombine.Count > 0 Then
                            'Xuất kết luận
                            kl.IsViPham = TypeViPham.ViPham
                            kl.NDKetLuan += String.Join(", ", TryCast(arrCombine.ToArray(GetType(String)), String())) & IIf(strKL.Length > 1, strKL, "") & ";"
                            kl.TenBangCauHoi = "CauHoi7"
                            kl.TenCotCauHoi = strTenCotCauHoi
                            kl.PhieuId = hidPhieuID.Value
                            data.KetLuans.AddObject(kl)
                            'Xuất kiến nghị
                            kn = New ThanhTraLaoDongModel.KienNghiDN
                            kn.NDKienNghi = "Tổ chức huấn luyện an toàn vệ sinh lao động cho " & String.Join(", ", TryCast(arrKN.ToArray(GetType(String)), String())) & " theo"
                            kn.TrichDanId = 34
                            kn.TenBangCauHoi = "Cauhoi7"
                            kn.PhieuId = hidPhieuID.Value
                            kn.IsKNPhaiTH = True
                            data.KienNghiDNs.AddObject(kn)
                        End If
                    End If
                Else
                    'Xuất kết luận
                    kl = New ThanhTraLaoDongModel.KetLuan
                    kl.IsViPham = TypeViPham.ViPham
                    kl.NDKetLuan = "Không thực hiện huấn luyện an toàn vệ sinh lao động theo quy định;"
                    kl.TenBangCauHoi = "CauHoi7"
                    kl.TenCotCauHoi = "Q706"
                    kl.PhieuId = hidPhieuID.Value
                    data.KetLuans.AddObject(kl)
                    'Xuất kiến nghị
                    kn = New ThanhTraLaoDongModel.KienNghiDN
                    kn.NDKienNghi = "Thực hiện việc huấn luyện an toàn vệ sinh lao động đối với người sử dụng lao động, người làm công tác an toàn vệ sinh lao động và người lao động theo"
                    kn.TrichDanId = 83
                    kn.TenBangCauHoi = "Cauhoi7"
                    kn.PhieuId = hidPhieuID.Value
                    kn.IsKNPhaiTH = True
                    data.KienNghiDNs.AddObject(kn)
                End If
                '7.7. Hồ sơ huấn luyện; 
                If Not IsNothing(q7.Q707) Then
                    kl = New ThanhTraLaoDongModel.KetLuan
                    If Not q7.Q707 Then
                        Dim arrKL As New ArrayList
                        Dim arrKN As New ArrayList
                        kl.NDKetLuan = "Hồ sơ huấn luyện an toàn vệ sinh lao động thiếu: "
                        strTenCotCauHoi = ""
                        If Not IsNothing(q7.Q7071) Then
                            arrKL.Add("tài liệu")
                            strTenCotCauHoi += "Q7071;"
                            arrKN.Add("tài liệu")
                        End If
                        If Not IsNothing(q7.Q7072) Then
                            arrKL.Add("sổ theo dõi")
                            strTenCotCauHoi += "Q7072;"
                            arrKN.Add("sổ theo dõi")
                        End If
                        If Not IsNothing(q7.Q7073) Then
                            arrKL.Add("bài kiểm tra")
                            strTenCotCauHoi += "Q7073;"
                            arrKN.Add("bài kiểm tra")
                        End If
                        If arrKL.Count > 0 Then
                            'Xuất kết luận
                            kl.IsViPham = TypeViPham.ViPham
                            kl.NDKetLuan += String.Join(", ", TryCast(arrKL.ToArray(GetType(String)), String())) + ";"
                            kl.TenBangCauHoi = "CauHoi7"
                            kl.TenCotCauHoi = "Q707;" & strTenCotCauHoi
                            kl.PhieuId = hidPhieuID.Value
                            data.KetLuans.AddObject(kl)
                            'Xuất kiến nghị
                            kn = New ThanhTraLaoDongModel.KienNghiDN
                            kn.NDKienNghi = "Bổ sung " & String.Join(", ", TryCast(arrKN.ToArray(GetType(String)), String())) + " vào hồ sơ huấn luyện và lưu giữ theo"
                            kn.TrichDanId = 34
                            kn.TenBangCauHoi = "Cauhoi7"
                            kn.PhieuId = hidPhieuID.Value
                            data.KienNghiDNs.AddObject(kn)

                        End If
                    End If
                End If

                '7.8. Nội dung huấn luyện
                If Not IsNothing(q7.Q708) Then
                    kl = New ThanhTraLaoDongModel.KetLuan
                    strTenCotCauHoi = "Q708;"
                    If Not q7.Q708 Then
                        Dim arrKL As New ArrayList
                        Dim arrKN As New ArrayList
                        If q7.Q7081 Then
                            arrKL.Add("vệ sinh lao động")
                            strTenCotCauHoi += "Q7081;"
                            arrKN.Add("vệ sinh lao động")
                        End If
                        If q7.Q7082 Then
                            arrKL.Add("cấp cứu tai nạn lao động")
                            strTenCotCauHoi += "Q7082;"
                            arrKN.Add("cấp cứu tai nạn lao động")
                        End If
                        If q7.Q7083 Then
                            arrKL.Add("quy trình, biện pháp an toàn")
                            strTenCotCauHoi += "Q7083;"
                            arrKN.Add("quy trình làm việc, biện pháp an toàn")
                        End If
                        If arrKL.Count > 0 Then
                            'Xuất kết luận
                            kl.NDKetLuan = "Nội dung huấn luyện của doanh nghiệp thiếu phần: " & String.Join(", ", TryCast(arrKL.ToArray(GetType(String)), String())) + ";"
                            kl.IsViPham = TypeViPham.ViPham
                            kl.TenBangCauHoi = "CauHoi7"
                            kl.TenCotCauHoi = strTenCotCauHoi
                            kl.PhieuId = hidPhieuID.Value
                            data.KetLuans.AddObject(kl)
                            'Xuất kiến nghị
                            kn = New ThanhTraLaoDongModel.KienNghiDN
                            kn.NDKienNghi = "Bổ sung nội dung huấn luyện về: " & String.Join(", ", TryCast(arrKN.ToArray(GetType(String)), String())) + " theo"
                            kn.TrichDanId = 34
                            kn.TenBangCauHoi = "Cauhoi7"
                            kn.PhieuId = hidPhieuID.Value
                            data.KienNghiDNs.AddObject(kn)

                        End If
                    End If
                End If

                '7.9. Trang bị phương tiện bảo vệ cá nhân theo danh mục nghề;
                If Not IsNothing(q7.Q709) Then
                    kl = New ThanhTraLaoDongModel.KetLuan
                    If q7.Q709 Then
                        kl.NDKetLuan = "Đã trang bị phương tiện bảo vệ cá nhân đầy đủ;"
                        kl.IsViPham = TypeViPham.KhongViPham
                    Else
                        'Xử lý kết luận
                        kl.NDKetLuan = "Trang bị phương tiện bảo vệ cá nhân còn thiếu " & q7.Q7091 & " cho chức danh nghề " & q7.Q70911 & ";"
                        kl.IsViPham = TypeViPham.ViPham
                        'Xuất kiến nghị
                        kn = New ThanhTraLaoDongModel.KienNghiDN
                        kn.NDKienNghi = "Trang bị bổ sung phương tiện bảo vệ cá nhân còn thiếu (" & q7.Q7091 & " cho chức danh nghề " & q7.Q70911 & ") theo danh mục ban hành kèm theo"
                        kn.TrichDanId = 35
                        kn.TenBangCauHoi = "Cauhoi7"
                        kn.PhieuId = hidPhieuID.Value
                        kn.IsKNPhaiTH = True
                        data.KienNghiDNs.AddObject(kn)
                    End If
                    kl.TenBangCauHoi = "CauHoi7"
                    kl.TenCotCauHoi = "Q709"
                    kl.PhieuId = hidPhieuID.Value
                    data.KetLuans.AddObject(kl)

                End If

                ''7.10. Thực hiện bồi dưỡng cho người lao động làm các công việc độc hại, nguy hiểm và đặc biệt độc hại, nguy hiểm
                Dim checkQ71011 As Boolean = True
                If Not IsNothing(q7.Q71012) Then
                    checkQ71011 = False
                    If q7.Q71012 Then
                        'Xuất kết luận
                        kl = New ThanhTraLaoDongModel.KetLuan
                        kl.NDKetLuan = "Thực hiện chế độ bồi dưỡng bằng tiền đối với lao động làm việc trong điều kiện có yếu tố nguy hiểm, độc hại chưa đúng quy định;"
                        kl.IsViPham = TypeViPham.ViPham
                        kl.TenBangCauHoi = "CauHoi7"
                        kl.TenCotCauHoi = "Q71012"
                        kl.PhieuId = hidPhieuID.Value
                        data.KetLuans.AddObject(kl)
                        'Xuất kiến nghị
                        kn = New ThanhTraLaoDongModel.KienNghiDN
                        kn.NDKienNghi = "Thực hiện chế độ bồi dưỡng bằng hiện vật đối với lao động làm việc trong điều kiện độc hại theo"
                        kn.TrichDanId = 36
                        kn.TenBangCauHoi = "Cauhoi7"
                        kn.PhieuId = hidPhieuID.Value
                        data.KienNghiDNs.AddObject(kn)
                    End If
                End If
                If Not IsNothing(q7.Q71011) Then 'Đầy đủ?
                    kl = New ThanhTraLaoDongModel.KetLuan
                    If q7.Q71011 Then
                        kl.NDKetLuan = "Đã thực hiện chế độ bồi dưỡng bằng hiện vật đối với người lao động làm việc trong điều kiện có yếu tố độc hại, nguy hiểm đầy đủ;"
                        kl.IsViPham = TypeViPham.KhongViPham
                        kl.TenBangCauHoi = "CauHoi7"
                        kl.TenCotCauHoi = "Q71011"
                        kl.PhieuId = hidPhieuID.Value
                        data.KetLuans.AddObject(kl)
                    Else
                        Dim arrKL As New ArrayList
                        Dim arrKN As New ArrayList
                        kl.TenCotCauHoi = "Q71011;"

                        Dim arrTSMucBoiThuong() As String = {"MucBoiDuongLoai1", "MucBoiDuongLoai2", "MucBoiDuongLoai3", "MucBoiDuongLoai4"}
                        Dim ts = (From a In data.SYS_PARAMETERS
                                  Where arrTSMucBoiThuong.Contains(a.Name)
                                  Select a.Name, a.Val).ToList
                        If Not IsNothing(q7.Q710111) AndAlso q7.Q710111 > 0 Then
                            checkQ71011 = False
                            arrKL.Add("mức 1 bằng " & String.Format(info, "{0:n0}", q7.Q710111) & " đồng/người/ca")
                            kl.TenCotCauHoi += "Q710111;"
                            arrKN.Add("mức " & String.Format(info, "{0:n0}", ts.Where(Function(c) c.Name.Equals("MucBoiDuongLoai1")).ToList()(0).Val * 1000) & " đồng/người/ca đối với nhóm nghề có điều kiện lao động loại 1")
                        End If
                        If Not IsNothing(q7.Q710112) AndAlso q7.Q710112 > 0 Then
                            checkQ71011 = False
                            arrKL.Add("mức 2 bằng " & String.Format(info, "{0:n0}", q7.Q710112) & " đồng/người/ca")
                            kl.TenCotCauHoi += "Q710112;"
                            arrKN.Add(String.Format(info, "{0:n0}", ts.Where(Function(c) c.Name.Equals("MucBoiDuongLoai2")).ToList()(0).Val * 1000) & " đồng/người/ca đối với nhóm nghề có điều kiện lao động loại 2")
                        End If
                        If Not IsNothing(q7.Q710113) AndAlso q7.Q710113 > 0 Then
                            checkQ71011 = False
                            arrKL.Add("mức 3 bằng " & String.Format(info, "{0:n0}", q7.Q710113) & " đồng/người/ca")
                            kl.TenCotCauHoi += "Q710113;"
                            arrKN.Add("mức " & String.Format(info, "{0:n0}", ts.Where(Function(c) c.Name.Equals("MucBoiDuongLoai3")).ToList()(0).Val * 1000) & " đồng/người/ca đối với nhóm nghề có điều kiện lao động loại 3")
                        End If
                        If Not IsNothing(q7.Q710114) AndAlso q7.Q710114 > 0 Then
                            checkQ71011 = False
                            arrKL.Add("mức 4 bằng " & String.Format(info, "{0:n0}", q7.Q710114) & " đồng/người/ca")
                            kl.TenCotCauHoi += "Q710114;"
                            arrKN.Add("mức " & String.Format(info, "{0:n0}", ts.Where(Function(c) c.Name.Equals("MucBoiDuongLoai4")).ToList()(0).Val * 1000) & " đồng/người/ca đối với nhóm nghề có điều kiện lao động loại 4")
                        End If

                        If Not IsNothing(q7.Q710115) Then
                            checkQ71011 = False
                            arrKL.Add("chưa thực hiện đối với nghề " & q7.Q710115 & ",")
                            kl.TenCotCauHoi += "Q710115;"
                            arrKN.Add("thực hiện bồi dưỡng bằng hiện vật đối với các nghề " & q7.Q710115)
                        End If
                        If arrKL.Count > 0 Then
                            'Xuất kết luận
                            kl.NDKetLuan = "Thực hiện chế độ bồi dưỡng bằng hiện vật " & String.Join(", ", TryCast(arrKL.ToArray(GetType(String)), String())) & " là chưa đủ theo quy định;"
                            kl.IsViPham = TypeViPham.ViPham
                            kl.TenBangCauHoi = "CauHoi7"
                            kl.PhieuId = hidPhieuID.Value
                            data.KetLuans.AddObject(kl)
                            'Xuất kiến nghị
                            kn = New ThanhTraLaoDongModel.KienNghiDN
                            kn.NDKienNghi = "Thực hiện chế độ bồi dưỡng bằng hiện vật đủ theo " & String.Join(", ", TryCast(arrKN.ToArray(GetType(String)), String())) & " theo"
                            kn.TrichDanId = 37
                            kn.TenBangCauHoi = "Cauhoi7"
                            kn.PhieuId = hidPhieuID.Value
                            data.KienNghiDNs.AddObject(kn)
                        End If
                        'Trường hợp Q71011 check không và các trường khác trong 7.10 có nhập, không check
                        If checkQ71011 Then
                            kl.NDKetLuan = "Chưa thực hiện chế độ bồi dưỡng bằng hiện vật đối với người làm trong điều kiện độc hại;"
                            kl.IsViPham = TypeViPham.ViPham
                            kl.TenBangCauHoi = "CauHoi7"
                            kl.TenCotCauHoi = "Q71011"
                            kl.PhieuId = hidPhieuID.Value
                            data.KetLuans.AddObject(kl)
                            'Xuất kiến nghị
                            kn = New ThanhTraLaoDongModel.KienNghiDN
                            kn.NDKienNghi = "Thực hiện chế độ bồi dưỡng bằng hiện vật đối với người làm trong điều kiện độc hại theo"
                            kn.TrichDanId = 38
                            kn.TenBangCauHoi = "Cauhoi7"
                            kn.PhieuId = hidPhieuID.Value
                            data.KienNghiDNs.AddObject(kn)
                        End If
                    End If
                End If


                '7.11. Tổng số vụ tai nạn lao động
                If Not IsNothing(q7.Q711) Then
                    ''TH đã thực hiện '' Khai báo điều tra; ''Giải quyết chế độ
                    kl = New ThanhTraLaoDongModel.KetLuan
                    kl.NDKetLuan = "Trong phạm vi thời gian thanh tra tại doanh nghiệp xảy ra " & String.Format(info, "{0:n0}", q7.Q711) & " vụ tai nạn lao động, "
                    Dim arrTemp As New ArrayList
                    Dim arrKL0 As New ArrayList
                    If q7.Q7111 > 0 Then
                        arrTemp.Add("làm " & String.Format(info, "{0:n0}", q7.Q7111) & " người chết")
                    End If
                    If (q7.Q7112 + q7.Q7116) > 0 Then
                        arrTemp.Add(String.Format(info, "{0:n0}", q7.Q7112 + q7.Q7116) + " người bị thương")
                    End If
                    If Not IsNothing(q7.Q7113) AndAlso q7.Q7113 > 0 Then
                        arrKL0.Add("đã thực hiện khai báo, điều tra " & String.Format(info, "{0:n0}", q7.Q7113) & " vụ")
                    End If
                    'If Not IsNothing(q7.Q7114) AndAlso q7.Q7114 > 0 Then
                    '    arrKL0.Add("đã thực hiện giải quyết chế độ cho " & String.Format(info, "{0:n0}", q7.Q7114) & " người bị nạn đầy đủ")
                    'End If
                    If arrTemp.Count > 0 Then
                        kl.NDKetLuan += String.Join(", ", TryCast(arrTemp.ToArray(GetType(String)), String())) & "; " & String.Join("; ", TryCast(arrKL0.ToArray(GetType(String)), String())) + ";"
                        kl.IsViPham = TypeViPham.KhongXet
                        kl.TenBangCauHoi = "CauHoi7"
                        kl.TenCotCauHoi = "Q711"
                        kl.PhieuId = hidPhieuID.Value
                        data.KetLuans.AddObject(kl)
                    End If

                    ''TH chưa thực hiện
                    '' Chưa khai báo, điều tra; ''Giải quyết chế độ
                    arrTemp.Clear()
                    kl = New ThanhTraLaoDongModel.KetLuan
                    If Not IsNothing(q7.Q71118) AndAlso q7.Q71118 > 0 Then
                        arrTemp.Add("Chưa thực hiện khai báo, điều tra " & q7.Q71118 & " vụ tai nạn lao động")
                        kl.TenCotCauHoi += "Q71118;"
                        'Xuất kiến nghị
                        kn = New ThanhTraLaoDongModel.KienNghiDN
                        kn.NDKienNghi = "Thực hiện điều tra " & q7.Q71118 & " vụ tai nạn lao động chưa được điều tra theo"
                        kn.TrichDanId = 38
                        kn.TenBangCauHoi = "Cauhoi7"
                        kn.PhieuId = hidPhieuID.Value
                        data.KienNghiDNs.AddObject(kn)

                    End If
                    ''Giải quyết chế độ
                    If IsNothing(q7.Q7114) Then
                        arrTemp.Add("Chưa thực hiện giải quyết chế độ cho người bị tai nạn lao động")
                        kl.TenCotCauHoi += "Q7114"
                        'Xuất kiến nghị
                        kn = New ThanhTraLaoDongModel.KienNghiDN
                        kn.NDKienNghi = "Thực hiện giải quyết chế độ cho người bị tai nạn lao động theo"
                        kn.TrichDanId = 39
                        kn.TenBangCauHoi = "Cauhoi7"
                        kn.PhieuId = hidPhieuID.Value
                        data.KienNghiDNs.AddObject(kn)

                    End If
                    If arrTemp.Count > 0 Then
                        kl.NDKetLuan = String.Join("; ", TryCast(arrTemp.ToArray(GetType(String)), String())) + ";"
                        kl.IsViPham = TypeViPham.ViPham
                        kl.TenBangCauHoi = "CauHoi7"
                        kl.PhieuId = hidPhieuID.Value
                        data.KetLuans.AddObject(kl)
                    End If
                    ''Tiền lương trong thời gian điều trị; Bồi thường, trợ cấp; Bố trí làm việc phù hợp; Thanh toán chi phí Y tế
                    If Not IsNothing(q7.Q7114) AndAlso q7.Q7114 > 0 Then
                        'Tất cả check có
                        If Not IsNothing(q7.Q7119) AndAlso q7.Q7119 AndAlso
                                                Not IsNothing(q7.Q71110) AndAlso q7.Q71110 AndAlso
                                                Not IsNothing(q7.Q71111) AndAlso q7.Q71111 AndAlso
                                                Not IsNothing(q7.Q71112) AndAlso q7.Q71112 Then
                            kl = New ThanhTraLaoDongModel.KetLuan
                            kl.NDKetLuan = "Đã thực hiện điều tra tai nạn lao động và giải quyết chế độ đối với " & String.Format(info, "{0:n0}", q7.Q7114) & " người bị nạn đầy đủ;"
                            kl.IsViPham = TypeViPham.KhongViPham
                            kl.TenBangCauHoi = "CauHoi7"
                            kl.TenCotCauHoi = "Q7119;Q71110;Q71111;Q71112"
                            kl.PhieuId = hidPhieuID.Value
                            data.KetLuans.AddObject(kl)
                        Else 'Ngược lại
                            arrTemp.Clear()
                            kl = New ThanhTraLaoDongModel.KetLuan
                            If Not IsNothing(q7.Q7119) AndAlso Not q7.Q7119 Then
                                arrTemp.Add("tiền lương trong thời gian điều trị")
                                kl.TenCotCauHoi += "Q7119;"
                            End If
                            If Not IsNothing(q7.Q71110) AndAlso Not q7.Q71110 Then
                                arrTemp.Add("bồi thường, trợ cấp")
                                kl.TenCotCauHoi += "Q71110;"
                            End If
                            If Not IsNothing(q7.Q71111) AndAlso Not q7.Q71111 Then
                                arrTemp.Add("bố trí làm việc phù hợp")
                                kl.TenCotCauHoi += "Q71111;"
                            End If
                            If Not IsNothing(q7.Q71112) AndAlso Not q7.Q71112 Then
                                arrTemp.Add("thanh toán chi phí Y tế")
                                kl.TenCotCauHoi += "Q71112;"
                            End If
                            If arrTemp.Count > 0 Then
                                kl.NDKetLuan = "Chưa giải quyết chế độ " & String.Join(", ", TryCast(arrTemp.ToArray(GetType(String)), String())) + " đối với người bị tai nạn lao động theo quy định;"
                                kl.IsViPham = TypeViPham.ViPham
                                kl.TenBangCauHoi = "CauHoi7"
                                kl.PhieuId = hidPhieuID.Value
                                data.KetLuans.AddObject(kl)
                                'Xuất kiến nghị
                                kn = New ThanhTraLaoDongModel.KienNghiDN
                                kn.NDKienNghi = "Giải quyết đầy đủ chế độ " & String.Join("; ", TryCast(arrTemp.ToArray(GetType(String)), String())) & " cho người bị tai nạn lao động theo"
                                kn.TrichDanId = 63
                                kn.TenBangCauHoi = "Cauhoi7"
                                kn.PhieuId = hidPhieuID.Value
                                data.KienNghiDNs.AddObject(kn)

                            End If
                        End If
                    End If


                    '' Xác định rõ nguyên nhân
                    If Not IsNothing(q7.Q7115) Then
                        kl = New ThanhTraLaoDongModel.KetLuan
                        If Not q7.Q7115 Then
                            kl.NDKetLuan = "Việc điều tra tai nạn lao động chưa làm rõ nguyên nhân, trách nhiệm người có lỗi và biện pháp đề phòng;"
                            kl.IsViPham = TypeViPham.ViPham
                            'Xuất kiến nghị
                            kn = New ThanhTraLaoDongModel.KienNghiDN
                            kn.NDKienNghi = "Việc điều tra tai nạn lao động phải làm rõ nguyên nhân, trách nhiệm người có lỗi và biện pháp đề phòng theo"
                            kn.TrichDanId = 38
                            kn.TenBangCauHoi = "Cauhoi7"
                            kn.PhieuId = hidPhieuID.Value
                            data.KienNghiDNs.AddObject(kn)
                        Else
                            kl.NDKetLuan = "Việc điều tra tai nạn lao động đã làm rõ nguyên nhân, trách nhiệm người có lỗi và biện pháp đề phòng;"
                            kl.IsViPham = TypeViPham.KhongViPham
                        End If
                        kl.TenBangCauHoi = "CauHoi7"
                        kl.TenCotCauHoi = "Q7115"
                        kl.PhieuId = hidPhieuID.Value
                        data.KetLuans.AddObject(kl)
                    End If
                End If


                '7.12. Đo đạc, kiểm tra môi trường tại nơi làm việc;
                If pn.SoNguoiLamNgheNguyHiem > 0 Then
                    If (Not IsNothing(q7.Q7121) And (Date.Now.Year - q7.Q7121) > 1) Then
                        'Xuất kết luận
                        kl = New ThanhTraLaoDongModel.KetLuan
                        kl.NDKetLuan = "Từ năm " & q7.Q7121 + 1 & " đến nay doanh nghiệp chưa đo môi trường lao động tại nơi làm việc;"
                        kl.IsViPham = TypeViPham.ViPham
                        kl.TenBangCauHoi = "CauHoi7"
                        kl.TenCotCauHoi = "Q7121"
                        kl.PhieuId = hidPhieuID.Value
                        data.KetLuans.AddObject(kl)
                        'Xuất kiến nghị
                        kn = New ThanhTraLaoDongModel.KienNghiDN
                        kn.NDKienNghi = "Thực hiện đo môi trường lao động hàng năm theo"
                        kn.TrichDanId = 40
                        kn.TenBangCauHoi = "Cauhoi7"
                        kn.PhieuId = hidPhieuID.Value
                        data.KienNghiDNs.AddObject(kn)
                    End If
                    'Nếu có nhập năm và năm khám cách năm hiện thời <=1 năm thì xét đến số chưa khám
                    If (Not IsNothing(q7.Q7121) AndAlso (Date.Now.Year - q7.Q7121) <= 1) Then
                        kl = New ThanhTraLaoDongModel.KetLuan
                        kl.NDKetLuan = "Đã thực hiện đo môi trường lao động trong năm " & q7.Q7121 & ";"
                        kl.IsViPham = TypeViPham.KhongViPham
                        kl.TenBangCauHoi = "CauHoi7"
                        kl.TenCotCauHoi = "Q7121"
                        kl.PhieuId = hidPhieuID.Value
                        data.KetLuans.AddObject(kl)
                    End If
                    If IsNothing(q7.Q7121) Then
                        Dim dn = (From a In data.DoanhNghieps Where a.DoanhNghiepId = pn.DoanhNghiepId Select a.NamTLDN).SingleOrDefault
                        kl = New ThanhTraLaoDongModel.KetLuan
                        kl.NDKetLuan = "Từ năm " & dn & " đến nay doanh nghiệp chưa đo môi trường lao động tại nơi làm việc có yếu tố độc hại;"
                        kl.IsViPham = TypeViPham.ViPham
                        kl.TenBangCauHoi = "CauHoi7"
                        kl.TenCotCauHoi = "Q7121"
                        kl.PhieuId = hidPhieuID.Value
                        data.KetLuans.AddObject(kl)
                        'Xuất kiến nghị
                        kn = New ThanhTraLaoDongModel.KienNghiDN
                        kn.NDKienNghi = "Thực hiện đo môi trường lao động hằng năm theo"
                        kn.TrichDanId = 40
                        kn.TenBangCauHoi = "Cauhoi7"
                        kn.PhieuId = hidPhieuID.Value
                        data.KienNghiDNs.AddObject(kn)
                    End If

                End If

                '7.13. Các biện pháp kỹ thuật nhằm cải thiện điều kiện làm việc
                If Not IsNothing(q7.Q713) Then
                    If q7.Q713 Then
                        kl = New ThanhTraLaoDongModel.KetLuan
                        kl.NDKetLuan = "Doanh nghiệp đã có biện pháp kỹ thuật nhằm cải thiện điều kiện làm việc " & q7.Q7131 & " đối với các yếu tố vượt tiêu chuẩn vệ sinh cho phép: " & q7.Q71222 & ";"
                        kl.IsViPham = TypeViPham.KhongViPham
                        kl.TenBangCauHoi = "CauHoi7"
                        kl.TenCotCauHoi = "Q713"
                        kl.PhieuId = hidPhieuID.Value
                        data.KetLuans.AddObject(kl)
                    Else
                        kl = New ThanhTraLaoDongModel.KetLuan
                        'Xuất kết luận
                        kl.NDKetLuan = "Chưa có biện pháp kỹ thuật nhằm cải thiện điều kiện làm việc " & q7.Q7132 & " đối với các yếu tố vượt tiêu chuẩn vệ sinh cho phép: " & q7.Q71222 & ";"
                        kl.IsViPham = TypeViPham.ViPham
                        kl.TenBangCauHoi = "CauHoi7"
                        kl.TenCotCauHoi = "Q713"
                        kl.PhieuId = hidPhieuID.Value
                        data.KetLuans.AddObject(kl)
                        'Xuất kiến nghị
                        kn = New ThanhTraLaoDongModel.KienNghiDN
                        kn.NDKienNghi = "Thực hiện biện pháp kỹ thuật nhằm cải thiện điều kiện làm việc đối với các yếu tố vượt tiêu chuẩn vệ sinh cho phép: " & q7.Q71222 & " theo"
                        kn.TrichDanId = 62
                        kn.TenBangCauHoi = "Cauhoi7"
                        kn.PhieuId = hidPhieuID.Value
                        data.KienNghiDNs.AddObject(kn)

                    End If
                End If


                '7.14. Khám sức khỏe định kỳ cho người lao động;
                If IsNothing(q7.Q7141) OrElse (Not IsNothing(q7.Q7141) And (Date.Now.Year - q7.Q7141) > 1) Then
                    'Xuất kết luận
                    kl = New ThanhTraLaoDongModel.KetLuan
                    If IsNothing(q7.Q7141) Then
                        kl.NDKetLuan = "Chưa khám sức khỏe định kỳ cho người lao động;"
                    Else
                        kl.NDKetLuan = "Từ năm " & (q7.Q7141 + 1) & " đến nay chưa khám sức khỏe định kỳ cho người lao động;"
                    End If
                    kl.IsViPham = TypeViPham.ViPham
                    kl.TenBangCauHoi = "CauHoi7"
                    kl.TenCotCauHoi = "Q7141"
                    kl.PhieuId = hidPhieuID.Value
                    data.KetLuans.AddObject(kl)
                    'Xuất kiến nghị
                    kn = New ThanhTraLaoDongModel.KienNghiDN
                    kn.NDKienNghi = "Thực hiện khám sức khỏe định kỳ hằng năm đầy đủ cho người lao động theo"
                    kn.TrichDanId = 41
                    kn.TenBangCauHoi = "Cauhoi7"
                    kn.PhieuId = hidPhieuID.Value
                    data.KienNghiDNs.AddObject(kn)

                End If
                'Nếu có nhập năm và năm khám cách năm hiện thời <=1 năm thì xét đến số chưa khám
                If (Not IsNothing(q7.Q7141) AndAlso (Date.Now.Year - q7.Q7141) <= 1) Then
                    'Xét đến số chưa khám
                    If Not IsNothing(q7.Q71422) Then
                        kl = New ThanhTraLaoDongModel.KetLuan
                        Dim percent = (q7.Q71422 / q7.Q71421) * 100
                        If percent > Cls_Common.ThamSoSys.PhanTramKhamSucKhoeDinhKy Then
                            'Xuất kết luận
                            kl.NDKetLuan = "Năm " & q7.Q7141 & " doanh nghiệp hiện còn " & String.Format(info, "{0:n0}", q7.Q71422) & " người chưa được khám sức khỏe định kỳ;"
                            kl.IsViPham = TypeViPham.ViPham
                            kl.TenBangCauHoi = "CauHoi7"
                            kl.TenCotCauHoi = "Q71422"
                            kl.PhieuId = hidPhieuID.Value
                            data.KetLuans.AddObject(kl)
                            'Xuất kiến nghị
                            kn = New ThanhTraLaoDongModel.KienNghiDN
                            kn.NDKienNghi = "Thực hiện khám sức khỏe định kỳ hằng năm đầy đủ cho người lao động theo"
                            kn.TrichDanId = 41
                            kn.TenBangCauHoi = "Cauhoi7"
                            kn.PhieuId = hidPhieuID.Value
                            data.KienNghiDNs.AddObject(kn)

                        Else
                            kl.NDKetLuan = "Đã khám sức khỏe định kỳ năm " & q7.Q7141 & " cho " & String.Format(info, "{0:n0}", q7.Q71421 - q7.Q71422) & " người đạt " & String.Format("{0:#.##}", 100 - percent) & "%;"
                            kl.IsViPham = TypeViPham.KhongViPham
                            kl.TenBangCauHoi = "CauHoi7"
                            kl.TenCotCauHoi = "Q71422"
                            kl.PhieuId = hidPhieuID.Value
                            data.KetLuans.AddObject(kl)
                        End If
                    End If
                End If


                ''hồ sơ quản lý sức khỏe
                If Not IsNothing(q7.Q7143) Then
                    kl = New ThanhTraLaoDongModel.KetLuan
                    If q7.Q7143 Then
                        kl.NDKetLuan = "Đã lập sổ sức khỏe và thực hiện quản lý sức khỏe theo quy định;"
                        kl.IsViPham = TypeViPham.KhongViPham
                    Else
                        'Xử lý kết luận
                        kl.NDKetLuan = "Chưa lập sổ khám sức khỏe định kỳ để thực hiện quản lý sức khỏe theo quy định;"
                        kl.IsViPham = TypeViPham.ViPham
                        'Xuất kiến nghị
                        kn = New ThanhTraLaoDongModel.KienNghiDN
                        kn.NDKienNghi = "Lập sổ khám sức khỏe định kỳ để thực hiện quản lý sức khỏe theo"
                        kn.TrichDanId = 74
                        kn.TenBangCauHoi = "Cauhoi7"
                        kn.PhieuId = hidPhieuID.Value
                        data.KienNghiDNs.AddObject(kn)
                    End If
                    kl.TenBangCauHoi = "CauHoi7"
                    kl.TenCotCauHoi = "Q7143"
                    kl.PhieuId = hidPhieuID.Value
                    data.KetLuans.AddObject(kl)
                End If

                '' hồ sơ khám sức khỏe tuyển dụng đúng quy định
                If Not IsNothing(q7.Q7144) Then
                    kl = New ThanhTraLaoDongModel.KetLuan
                    If q7.Q7144 Then
                        kl.NDKetLuan = "Đã thực hiện khám sức khỏe tuyển dụng và lưu giữ hồ sơ đúng quy định;"
                        kl.IsViPham = TypeViPham.KhongViPham
                    Else
                        'Xử lý kết luận
                        kl.NDKetLuan = "Lao động mùa vụ dưới 3 tháng chưa có giấy chứng nhận sức khỏe theo quy định;"
                        kl.IsViPham = TypeViPham.ViPham
                        'Xuất kiến nghị
                        kn = New ThanhTraLaoDongModel.KienNghiDN
                        kn.NDKienNghi = "Thực hiện khám sức khỏe khi tuyển dụng và quản lý Giấy chứng nhận sức khỏe đối với lao động mùa vụ dưới 3 tháng chưa có giấy chứng nhận sức khỏe theo"
                        kn.TrichDanId = 41
                        kn.TenBangCauHoi = "Cauhoi7"
                        kn.PhieuId = hidPhieuID.Value
                        data.KienNghiDNs.AddObject(kn)
                    End If
                    kl.TenBangCauHoi = "CauHoi7"
                    kl.TenCotCauHoi = "Q7144"
                    kl.PhieuId = hidPhieuID.Value
                    data.KetLuans.AddObject(kl)
                End If

                ''7.15. Khám phát hiện bệnh nghề nghiệp hàng năm cho người lao động
                If Not IsNothing(q7.Q7151) Then
                    kl = New ThanhTraLaoDongModel.KetLuan
                    kl.NDKetLuan = "Đã thực hiện khám bệnh nghề nghiệp cho " & String.Format(info, "{0:n0}", q7.Q7151) & " lao động làm công việc độc hại; "
                    Dim arrKL As New ArrayList
                    If Not IsNothing(q7.Q7152) AndAlso q7.Q7152 > 0 Then
                        arrKL.Add("số người bị bệnh cộng dồn đến nay là " & String.Format(info, "{0:n0}", q7.Q7152))
                    End If
                    If Not IsNothing(q7.Q715211) AndAlso q7.Q715211 > 0 Then
                        arrKL.Add("đã giám định " & String.Format(info, "{0:n0}", q7.Q715211) & " người")
                    End If
                    If Not IsNothing(q7.Q715212) AndAlso q7.Q715212 > 0 Then
                        arrKL.Add("cấp sổ " & String.Format(info, "{0:n0}", q7.Q715212) & " người")
                    End If
                    If Not IsNothing(q7.Q715213) AndAlso q7.Q715213 > 0 Then
                        arrKL.Add("chuyển công việc khác " & String.Format(info, "{0:n0}", q7.Q715213) & " người")
                    End If
                    If arrKL.Count > 0 Then
                        kl.NDKetLuan += String.Join("; ", TryCast(arrKL.ToArray(GetType(String)), String())) & ";"
                    End If
                    kl.IsViPham = TypeViPham.KhongViPham
                    kl.TenBangCauHoi = "CauHoi7"
                    kl.TenCotCauHoi = "Q7151"
                    kl.PhieuId = hidPhieuID.Value
                    data.KetLuans.AddObject(kl)
                Else
                    '' Xét xem có số ld làm công việc đọc hại không ?--> nếu có ==> kết luận vi phạm
                    Dim soNguoi = (From a In data.PhieuNhapHeaders
                                Where a.PhieuID = hidPhieuID.Value
                                Select a.SoNguoiLamNgheNguyHiem).FirstOrDefault
                    Dim namTLDoanhNghiep = (From a In data.DoanhNghieps Join b In data.PhieuNhapHeaders On a.DoanhNghiepId Equals b.DoanhNghiepId
                                Where b.PhieuID = hidPhieuID.Value
                                Select a.NamTLDN).FirstOrDefault
                    If (soNguoi > 0 AndAlso (Date.Now.Year - namTLDoanhNghiep) >= Cls_Common.ThamSoSys.SoSanhNamThanhLapDN _
                        AndAlso ((Not IsNothing(q7.Q71221) AndAlso q7.Q71221 >= 2) OrElse (IsNothing(q7.Q7121)))) Then
                        'Xuất kết luận
                        kl = New ThanhTraLaoDongModel.KetLuan
                        kl.NDKetLuan = "Chưa khám bệnh nghề nghiệp cho những người làm việc trên 5 năm trong số " & String.Format(info, "{0:n0}", soNguoi) & " lao động làm các nghề, công việc độc hại;"
                        kl.IsViPham = TypeViPham.ViPham
                        kl.TenBangCauHoi = "CauHoi7"
                        kl.TenCotCauHoi = "Q7151"
                        kl.PhieuId = hidPhieuID.Value
                        data.KetLuans.AddObject(kl)
                        'Xuất kiến nghị
                        kn = New ThanhTraLaoDongModel.KienNghiDN
                        kn.NDKienNghi = "Thực hiện khám phát hiện bệnh nghề nghiệp cho những người làm việc trên 5 năm trong số " & String.Format(info, "{0:n0}", soNguoi) & " lao động làm các nghề, công việc có yếu tố độc hại theo"
                        If Not IsNothing(q7.Q71222) AndAlso (q7.Q71222.Contains("bụi") Or q7.Q71222.Contains("ồn")) Then
                            Dim arrKL As New ArrayList
                            If q7.Q71222.Contains("bụi") Then
                                arrKL.Add("bệnh bụi phổi-silic")
                            End If
                            If q7.Q71222.Contains("ồn") Then
                                arrKL.Add("bệnh giảm sức nghe do tiếng ồn")
                            End If
                            kn.NDKienNghi = "Thực hiện khám phát hiện " & String.Join(", ", TryCast(arrKL.ToArray(GetType(String)), String())) & " cho những người làm việc trên 5 năm trong số " & String.Format(info, "{0:n0}", soNguoi) & " lao động làm các nghề, công việc có yếu tố độc hại theo"
                        End If
                        kn.TrichDanId = 42
                        kn.TenBangCauHoi = "Cauhoi7"
                        kn.PhieuId = hidPhieuID.Value
                        data.KienNghiDNs.AddObject(kn)
                    End If

                    'If (soNguoi > 0 AndAlso (Date.Now.Year - namTLDoanhNghiep) >= Cls_Common.ThamSoSys.SoSanhNamThanhLapDN _
                    '    AndAlso Not IsNothing(q7.Q71222) AndAlso (q7.Q71222.Contains("bụi") Or q7.Q71222.Contains("ồn"))) Then
                    '    'Xuất kết luận
                    '    kl = New ThanhTraLaoDongModel.KetLuan
                    '    kl.NDKetLuan = "Chưa khám bệnh nghề nghiệp cho " & String.Format(info, "{0:n0}", soNguoi) & " lao động làm các nghề, công việc độc hại;"
                    '    kl.IsViPham = TypeViPham.ViPham
                    '    kl.TenBangCauHoi = "CauHoi7"
                    '    kl.TenCotCauHoi = "Q715"
                    '    kl.PhieuId = hidPhieuID.Value
                    '    data.KetLuans.AddObject(kl)
                    '    'Xuất kiến nghị
                    '    Dim arrKL As New ArrayList
                    '    If q7.Q71222.Contains("bụi") Then
                    '        arrKL.Add("bệnh bụi phổi-silic")
                    '    End If
                    '    If q7.Q71222.Contains("ồn") Then
                    '        arrKL.Add("bệnh giảm sức nghe do tiếng ồn")
                    '    End If
                    '    If arrKL.Count > 0 Then
                    '        kn = New ThanhTraLaoDongModel.KienNghiDN
                    '        kn.NDKienNghi = "Thực hiện khám phát hiện " & String.Join(", ", TryCast(arrKL.ToArray(GetType(String)), String())) & " cho " & String.Format(info, "{0:n0}", soNguoi) & " lao động làm các nghề, công việc có yếu tố độc hại theo"
                    '        kn.TrichDanId = 42
                    '        kn.TenBangCauHoi = "Cauhoi7"
                    '        kn.PhieuId = hidPhieuID.Value
                    '        data.KienNghiDNs.AddObject(kn)
                    '    End If
                    'End If
                End If


                '7.16. Thành lập đội cấp cứu
                If Not IsNothing(q7.Q716) AndAlso Not q7.Q716 AndAlso Not IsNothing(q7.Q7161) AndAlso Not q7.Q7161 Then
                    'Xuất kết luận
                    kl = New ThanhTraLaoDongModel.KetLuan
                    kl.NDKetLuan = "Chưa xây dựng phương án xử lý cấp cứu tai nạn lao động và trang bị phương tiện, túi thuốc tại nơi làm việc theo qui định;"
                    kl.IsViPham = TypeViPham.ViPham
                    kl.TenBangCauHoi = "CauHoi7"
                    kl.TenCotCauHoi = "Q716;Q7161"
                    kl.PhieuId = hidPhieuID.Value
                    data.KetLuans.AddObject(kl)
                    'Xuất kiến nghị
                    kn = New ThanhTraLaoDongModel.KienNghiDN
                    kn.NDKienNghi = "Xây dựng phương án xử lý cấp cứu tai nạn lao động và trang bị phương tiện, túi thuốc tại nơi làm việc theo"
                    kn.TrichDanId = 43
                    kn.TenBangCauHoi = "Cauhoi7"
                    kn.PhieuId = hidPhieuID.Value
                    data.KienNghiDNs.AddObject(kn)

                ElseIf Not IsNothing(q7.Q716) AndAlso q7.Q716 AndAlso Not IsNothing(q7.Q7161) AndAlso q7.Q7161 Then
                    kl = New ThanhTraLaoDongModel.KetLuan
                    kl.NDKetLuan = "Đã thành lập đội cấp cứu và trang bị phương tiện, túi thuốc tại nơi làm việc;"
                    kl.IsViPham = TypeViPham.KhongViPham
                    kl.TenBangCauHoi = "CauHoi7"
                    kl.TenCotCauHoi = "Q716;Q7161"
                    kl.PhieuId = hidPhieuID.Value
                    data.KetLuans.AddObject(kl)
                ElseIf Not IsNothing(q7.Q716) AndAlso Not q7.Q716 AndAlso Not IsNothing(q7.Q7161) AndAlso q7.Q7161 Then
                    'Xuất kết luận
                    kl = New ThanhTraLaoDongModel.KetLuan
                    kl.NDKetLuan = "Chưa xây dựng phương án xử lý cấp cứu tai nạn lao động;"
                    kl.IsViPham = TypeViPham.ViPham
                    kl.TenBangCauHoi = "CauHoi7"
                    kl.TenCotCauHoi = "Q716"
                    kl.PhieuId = hidPhieuID.Value
                    data.KetLuans.AddObject(kl)
                    'Xuất kiến nghị
                    kn = New ThanhTraLaoDongModel.KienNghiDN
                    kn.NDKienNghi = "Xây dựng phương án xử lý cấp cứu tai nạn lao động theo"
                    kn.TrichDanId = 43
                    kn.TenBangCauHoi = "Cauhoi7"
                    kn.PhieuId = hidPhieuID.Value
                    data.KienNghiDNs.AddObject(kn)

                ElseIf Not IsNothing(q7.Q716) AndAlso q7.Q716 AndAlso Not IsNothing(q7.Q7161) AndAlso Not q7.Q7161 Then
                    'Xuất kết luận
                    kl = New ThanhTraLaoDongModel.KetLuan
                    kl.NDKetLuan = "Chưa trang bị phương tiện, túi thuốc tại nơi làm việc;"
                    kl.IsViPham = TypeViPham.ViPham
                    kl.TenBangCauHoi = "CauHoi7"
                    kl.TenCotCauHoi = "Q7161"
                    kl.PhieuId = hidPhieuID.Value
                    data.KetLuans.AddObject(kl)
                    'Xuất kiến nghị
                    kn = New ThanhTraLaoDongModel.KienNghiDN
                    kn.NDKienNghi = "Trang bị phương tiện, túi thuốc tại nơi làm việc theo"
                    kn.TrichDanId = 43
                    kn.TenBangCauHoi = "Cauhoi7"
                    kn.PhieuId = hidPhieuID.Value
                    data.KienNghiDNs.AddObject(kn)

                End If

                '7.17. Kiểm tra thực tế yếu tố nguy hiểm tại nơi làm việc;
                If Not IsNothing(q7.Q717) Then
                    '' Nếu không có thì không xét mục 7.18, ngược lại thì xét
                    If Not q7.Q717 Then
                        kl = New ThanhTraLaoDongModel.KetLuan
                        kl.NDKetLuan = "Đã tự kiểm tra công tác an toàn vệ sinh lao động đúng quy định, nơi làm việc đạt tiêu chuẩn về an toàn vệ sinh lao động;"
                        kl.IsViPham = TypeViPham.KhongViPham
                        kl.TenBangCauHoi = "CauHoi7"
                        kl.TenCotCauHoi = "Q717"
                        kl.PhieuId = hidPhieuID.Value
                        data.KetLuans.AddObject(kl)
                    Else
                        kl = New ThanhTraLaoDongModel.KetLuan
                        Dim arrKL As New ArrayList
                        Dim arrKN As New ArrayList
                        kl.TenCotCauHoi = ""
                        If Not IsNothing(q7.Q7171) And q7.Q7171 Then
                            arrKL.Add("bộ phận chuyển động không bao che")
                            kl.TenCotCauHoi = kl.TenCotCauHoi & "Q7171;"
                            arrKN.Add("bộ phận chuyển động không bao che")
                        End If
                        If Not IsNothing(q7.Q7172) And q7.Q7172 Then
                            arrKL.Add("thiếu lan can, rào ngăn tại nơi nguy hiểm")
                            kl.TenCotCauHoi = kl.TenCotCauHoi & "Q7172;"
                            arrKN.Add("thiếu lan can, rào ngăn tại nơi nguy hiểm")
                        End If
                        If Not IsNothing(q7.Q7173) And q7.Q7173 Then
                            arrKL.Add("thiếu biển báo nơi nguy hiểm")
                            kl.TenCotCauHoi = kl.TenCotCauHoi & "Q7173;"
                            arrKN.Add("thiếu biển báo nơi nguy hiểm")
                        End If
                        If Not IsNothing(q7.Q7177) And q7.Q7177 Then
                            arrKL.Add("thiếu bảng chỉ dẫn an toàn đối với máy, thiết bị đặt tại vị trí dễ đọc, dễ thấy tại nơi làm việc")
                            kl.TenCotCauHoi = kl.TenCotCauHoi & "Q7177;"
                            arrKN.Add("thiếu bảng chỉ dẫn an toàn đối với máy, thiết bị đặt tại vị trí dễ đọc, dễ thấy tại nơi làm việc")
                        End If
                        If Not IsNothing(q7.Q7174) And q7.Q7174 Then
                            arrKL.Add("hộp cầu dao điện hở")
                            kl.TenCotCauHoi = kl.TenCotCauHoi & "Q7174;"
                            arrKN.Add("hộp cầu dao điện hở")
                        End If
                        If Not IsNothing(q7.Q7175) And q7.Q7175 Then
                            arrKL.Add("đấu nối điện không đúng quy cách")
                            kl.TenCotCauHoi = kl.TenCotCauHoi & "Q7175;"
                            arrKN.Add("đấu nối điện không đúng quy cách")
                        End If
                        If Not IsNothing(q7.Q7178) And q7.Q7178 Then
                            arrKL.Add("người lao động không sử dụng phương tiện bảo vệ cá nhân")
                            kl.TenCotCauHoi = kl.TenCotCauHoi & "Q7178;"
                            arrKN.Add("người lao động không sử dụng phương tiện bảo vệ cá nhân")
                        End If
                        If Not String.IsNullOrEmpty(q7.Q7176) Then
                            arrKL.Add(q7.Q7176)
                            kl.TenCotCauHoi = kl.TenCotCauHoi & "Q7176;"
                            arrKN.Add(q7.Q7176)
                        End If
                        If arrKL.Count > 0 Then
                            'Xuất kết luận
                            kl.NDKetLuan = "Tại nơi làm việc còn có yếu tố nguy hiểm: " & String.Join(", ", TryCast(arrKL.ToArray(GetType(String)), String())) & ";"
                            kl.IsViPham = TypeViPham.ViPham
                            kl.TenBangCauHoi = "CauHoi7"
                            kl.PhieuId = hidPhieuID.Value
                            data.KetLuans.AddObject(kl)
                            arrKL.Clear()
                            'Xuất kiến nghị
                            kn = New ThanhTraLaoDongModel.KienNghiDN
                            kn.NDKienNghi = "Xử lý các yếu tố nguy hiểm tại nơi làm việc: " & String.Join(", ", TryCast(arrKN.ToArray(GetType(String)), String())) & " nhằm đảm bảo an toàn lao động theo"
                            kn.TrichDanId = 44
                            kn.TenBangCauHoi = "Cauhoi7"
                            kn.PhieuId = hidPhieuID.Value
                            kn.IsKNPhaiTH = True
                            data.KienNghiDNs.AddObject(kn)
                            arrKN.Clear()
                        End If

                        '' Lưu cho mục 7.18
                        kl = New ThanhTraLaoDongModel.KetLuan
                        kl.TenCotCauHoi = ""
                        Dim strKhac As String = Nothing
                        If Not IsNothing(q7.Q7181) And q7.Q7181 Then
                            arrKL.Add("không thường xuyên")
                            kl.TenCotCauHoi = kl.TenCotCauHoi & "Q7181;"
                            arrKN.Add("thường xuyên")
                        End If
                        If Not IsNothing(q7.Q7182) And q7.Q7182 Then
                            arrKL.Add("không ghi sổ kiểm tra")
                            kl.TenCotCauHoi = kl.TenCotCauHoi & "Q7182;"
                            arrKN.Add("ghi sổ kiểm tra")
                        End If
                        If Not IsNothing(q7.Q7183) And q7.Q7183 Then
                            arrKL.Add("không nghiệm thu an toàn thiết bị trước khi sử dụng")
                            kl.TenCotCauHoi = kl.TenCotCauHoi & "Q7183;"
                            arrKN.Add("nghiệm thu an toàn thiết bị trước khi sử dụng")
                        End If
                        If Not IsNothing(q7.Q7185) And q7.Q7185 Then
                            arrKL.Add("không đo điện trở tiếp đất các thiết bị điện, hệ thống chống sét")
                            kl.TenCotCauHoi = kl.TenCotCauHoi & "Q7185;"
                            arrKN.Add("đo điện trở tiếp đất các thiết bị điện và hệ thống chống sét")
                        End If
                        If Not String.IsNullOrEmpty(q7.Q7184) Then
                            arrKL.Add(q7.Q7184)
                            kl.TenCotCauHoi = kl.TenCotCauHoi & "Q7184;"
                            'arrKN.Add("khắc phục các thiếu sót " & q7.Q7184)
                            strKhac = "khắc phục các thiếu sót " & q7.Q7184
                        End If
                        If arrKL.Count > 0 Then
                            'Xuất kết luận
                            kl.NDKetLuan = "Việc tự kiểm tra công tác an toàn vệ sinh lao động: " & String.Join(", ", TryCast(arrKL.ToArray(GetType(String)), String())) & ";"
                            kl.IsViPham = TypeViPham.ViPham
                            kl.TenBangCauHoi = "CauHoi7"
                            kl.PhieuId = hidPhieuID.Value
                            data.KetLuans.AddObject(kl)
                            'Xuất kiến nghị
                            kn = New ThanhTraLaoDongModel.KienNghiDN
                            kn.NDKienNghi = "Thực hiện tự kiểm tra công tác an toàn vệ sinh lao động: " & String.Join(", ", TryCast(arrKN.ToArray(GetType(String)), String())) & IIf(IsNothing(strKhac), "", "; " & strKhac) & " theo"
                            kn.TrichDanId = 44
                            kn.TenBangCauHoi = "Cauhoi7"
                            kn.PhieuId = hidPhieuID.Value
                            data.KienNghiDNs.AddObject(kn)

                        End If
                    End If
                End If
                ''7.19 Lấy ý kiến đại diện người lao động về công tác an toàn vệ sinh lao động
                If Not IsNothing(q7.Q719) Then
                    If q7.Q719 Then
                        'Xuất kết luận
                        kl = New ThanhTraLaoDongModel.KetLuan
                        kl.NDKetLuan = "Đã lấy ý kiến tổ chức đại diện người lao động khi xây dựng kế hoạch và thực hiện các hoạt động đảm bảo an toàn vệ sinh lao động;"
                        kl.IsViPham = TypeViPham.KhongViPham
                        kl.TenBangCauHoi = "CauHoi7"
                        kl.TenCotCauHoi = "Q719"
                        kl.PhieuId = hidPhieuID.Value
                        data.KetLuans.AddObject(kl)
                    Else
                        'Xuất kết luận
                        kl = New ThanhTraLaoDongModel.KetLuan
                        kl.NDKetLuan = "Không lấy ý kiến tổ chức đại diện người lao động khi xây dựng kế hoạch và thực hiện các hoạt động đảm bảo an toàn vệ sinh lao động theo quy định;"
                        kl.IsViPham = TypeViPham.ViPham
                        kl.TenBangCauHoi = "CauHoi7"
                        kl.TenCotCauHoi = "Q719"
                        kl.PhieuId = hidPhieuID.Value
                        data.KetLuans.AddObject(kl)
                        'Xuất kiến nghị
                        kn = New ThanhTraLaoDongModel.KienNghiDN
                        kn.NDKienNghi = "Lấy ý kiến tổ chức đại diện người lao động khi xây dựng kế hoạch và thực hiện các hoạt động đảm bảo an toàn vệ sinh lao động theo"
                        kn.TrichDanId = 78
                        kn.TenBangCauHoi = "Cauhoi7"
                        kn.PhieuId = hidPhieuID.Value
                        data.KienNghiDNs.AddObject(kn)
                    End If
                End If
                data.SaveChanges()
            End If
        End Using
    End Sub
    Protected Sub XulyKetLuanCauHoi8()
        Using data As New ThanhTraLaoDongEntities
            Dim kl As KetLuan
            Dim kn As KienNghiDN
            'Tao moi ket luan dua vao cauhoi10
            Dim q8 = (From a In data.CauHoi8
                                 Where a.PhieuId = hidPhieuID.Value).FirstOrDefault

            If Not q8 Is Nothing Then
                '8.1. Xây dựng và đăng ký Nội quy lao động
                If Not IsNothing(q8.Q81) AndAlso q8.Q81 Then
                    If Not IsNothing(q8.Q8111) Then
                        If Not IsNothing(q8.Q8112) Then
                            If q8.Q8112 Then '' Đã đằng ký 
                                kl = New ThanhTraLaoDongModel.KetLuan
                                kl.NDKetLuan = "Đã xây dựng nội quy lao động năm " & q8.Q8111 & " và đăng kí với cơ quan quản lý nhà nước về lao động địa phương;"
                                kl.IsViPham = TypeViPham.KhongViPham
                                kl.TenBangCauHoi = "CauHoi8"
                                kl.TenCotCauHoi = "Q8111" & IIf(String.IsNullOrEmpty(q8.Q812), "", "Q812") ' Nếu có nội dung chưa phù hợp pháp luật lao động
                                kl.PhieuId = hidPhieuID.Value
                                data.KetLuans.AddObject(kl)
                            Else '' chưa đăng ký
                                'Xuất kết luận
                                kl = New ThanhTraLaoDongModel.KetLuan
                                kl.NDKetLuan = "Doanh nghiệp đã xây dựng nội quy lao động nhưng chưa đăng ký với cơ quan quản lý nhà nước về lao động địa phương;"
                                kl.IsViPham = TypeViPham.ViPham
                                kl.TenBangCauHoi = "CauHoi8"
                                kl.TenCotCauHoi = "Q8112"
                                kl.PhieuId = hidPhieuID.Value
                                data.KetLuans.AddObject(kl)
                                'Xuất kiến nghị
                                kn = New ThanhTraLaoDongModel.KienNghiDN
                                kn.NDKienNghi = "Đăng ký nội quy lao động với cơ quan quản lý nhà nước về lao động địa phương theo"
                                kn.TrichDanId = 45
                                kn.TenBangCauHoi = "Cauhoi8"
                                kn.PhieuId = hidPhieuID.Value
                                data.KienNghiDNs.AddObject(kn)
                            End If
                        End If
                    Else '' Chưa xây dựng
                        'Xuất kết luận
                        kl = New ThanhTraLaoDongModel.KetLuan
                        kl.NDKetLuan = "Doanh nghiệp chưa xây dựng nội quy lao động;"
                        kl.IsViPham = TypeViPham.ViPham
                        kl.TenBangCauHoi = "CauHoi8"
                        kl.TenCotCauHoi = "Q8111"
                        kl.PhieuId = hidPhieuID.Value
                        data.KetLuans.AddObject(kl)
                        'Xuất kiến nghị
                        kn = New ThanhTraLaoDongModel.KienNghiDN
                        kn.NDKienNghi = "Xây dựng nội quy lao động theo"
                        kn.TrichDanId = 46
                        kn.TenBangCauHoi = "Cauhoi8"
                        kn.PhieuId = hidPhieuID.Value
                        data.KienNghiDNs.AddObject(kn)
                    End If

                    '' Nội dung không phù hợp pháp luật
                    If Not String.IsNullOrEmpty(q8.Q812) Then
                        'Xuất kết luận
                        kl = New ThanhTraLaoDongModel.KetLuan
                        kl.NDKetLuan = "Nội quy lao động có nội dung không phù hợp với pháp luật: " & q8.Q812 & ";"
                        kl.IsViPham = TypeViPham.ViPham
                        kl.TenBangCauHoi = "CauHoi8"
                        kl.TenCotCauHoi = "Q812"
                        kl.PhieuId = hidPhieuID.Value
                        data.KetLuans.AddObject(kl)
                        'Xuất kiến nghị
                        kn = New ThanhTraLaoDongModel.KienNghiDN
                        kn.NDKienNghi = "Bãi bỏ những nội dung không phù hợp với pháp luật trong nội quy lao động: " & q8.Q812 & " theo"
                        kn.TrichDanId = 46
                        kn.TenBangCauHoi = "Cauhoi8"
                        kn.PhieuId = hidPhieuID.Value
                        data.KienNghiDNs.AddObject(kn)
                    End If
                End If
                '8.2, 8.3, 8.4;8.5
                'Nếu có số vụ kỷ luật lao động
                If Not IsNothing(q8.Q82) Then
                    '' Nếu có xử lý quy trình
                    If Not IsNothing(q8.Q851) Then
                        ''Nếu xử lý đúng quy trình
                        If q8.Q851 Then
                            kl = New ThanhTraLaoDongModel.KetLuan
                            kl.NDKetLuan = "Doanh nghiệp xử lý kỷ luật đúng trình tự;"
                            kl.IsViPham = TypeViPham.KhongViPham
                            kl.TenBangCauHoi = "CauHoi8"
                            kl.TenCotCauHoi = "Q851"
                            kl.PhieuId = hidPhieuID.Value
                            data.KetLuans.AddObject(kl)
                        Else
                            ' xử lý sai quy trinh
                            kl = New ThanhTraLaoDongModel.KetLuan
                            Dim arrStr As New ArrayList
                            Dim strTenCotCauHoi As String = ""
                            If Not IsNothing(q8.Q8511) AndAlso q8.Q8511 Then
                                arrStr.Add("không đúng thẩm quyền")
                                strTenCotCauHoi = "Q8511;"
                            End If
                            If Not IsNothing(q8.Q8512) AndAlso q8.Q8512 Then
                                arrStr.Add("quá thời hiệu")
                                strTenCotCauHoi += "Q8512;"
                            End If
                            'If Not IsNothing(q8.Q8513) AndAlso q8.Q8513 Then
                            '    arrStr.Add("áp dụng sai hình thức kỷ luật theo Bộ Luật lao động")
                            '    strTenCotCauHoi += "Q8513;"
                            'End If
                            If Not IsNothing(q8.Q8514) AndAlso q8.Q8514 Then
                                arrStr.Add("không chứng minh được lỗi của người lao động")
                                strTenCotCauHoi += "Q8514;"
                            End If
                            If Not IsNothing(q8.Q8515) AndAlso q8.Q8515 Then
                                arrStr.Add("họp xét kỷ luật không ghi biên bản họp")
                                strTenCotCauHoi += "Q8515;"
                            End If
                            If Not String.IsNullOrEmpty(q8.Q8516) Then
                                arrStr.Add(q8.Q8516)
                                strTenCotCauHoi += "Q8516;"
                            End If
                            'Xuất kết luận
                            kl.NDKetLuan = "Xử lý kỷ luật lao động " + String.Join(", ", TryCast(arrStr.ToArray(GetType(String)), String())) + ";"
                            kl.IsViPham = TypeViPham.ViPham
                            kl.TenBangCauHoi = "CauHoi8"
                            kl.TenCotCauHoi = "Q851;" + strTenCotCauHoi
                            kl.PhieuId = hidPhieuID.Value
                            data.KetLuans.AddObject(kl)
                            'Xuất kiến nghị
                            kn = New ThanhTraLaoDongModel.KienNghiDN
                            kn.NDKienNghi = "Xử lý kỷ luật lao động đúng trình tự theo"
                            kn.TrichDanId = 47
                            kn.TenBangCauHoi = "Cauhoi8"
                            kn.PhieuId = hidPhieuID.Value
                            data.KienNghiDNs.AddObject(kn)
                        End If
                    End If
                    '' Nếu có sa thải
                    If Not IsNothing(q8.Q823) Then
                        '' Xét xem có hoặc không báo cáo vs cơ quan quản lý lao động nhà nước
                        If Not IsNothing(q8.Q83) Then
                            ' Nếu có báo cáo 
                            If q8.Q83 Then
                                kl = New ThanhTraLaoDongModel.KetLuan
                                kl.NDKetLuan = "Đã gửi quyết định sa thải đến cơ quan quản lý nhà nước về lao động địa phương sau khi sa thải lao động;"
                                kl.IsViPham = TypeViPham.KhongViPham
                                kl.TenBangCauHoi = "CauHoi8"
                                kl.TenCotCauHoi = "Q83"
                                kl.PhieuId = hidPhieuID.Value
                                data.KetLuans.AddObject(kl)
                            Else '' Nếu không có báo cáo vs cơ quan quản lý lao động nhà nước
                                'Xuất kết luận
                                kl = New ThanhTraLaoDongModel.KetLuan
                                kl.NDKetLuan = "Chưa gửi quyết định sa thải đến cơ quan quản lý nhà nước về lao động địa phương sau khi sa thải lao động;"
                                kl.IsViPham = TypeViPham.ViPham
                                kl.TenBangCauHoi = "CauHoi8"
                                kl.TenCotCauHoi = "Q83"
                                kl.PhieuId = hidPhieuID.Value
                                data.KetLuans.AddObject(kl)
                                'Xuất kiến nghị
                                kn = New ThanhTraLaoDongModel.KienNghiDN
                                kn.NDKienNghi = "Thực hiện gửi quyết định sa thải kèm theo biên bản xử lý kỷ luật lao động đến cơ quan quản lý nhà nước về lao động địa phương trong vòng 10 ngày sau khi sa thải người lao động theo"
                                kn.TrichDanId = 48
                                kn.TenBangCauHoi = "Cauhoi8"
                                kn.PhieuId = hidPhieuID.Value
                                data.KienNghiDNs.AddObject(kn)
                            End If
                        End If
                    End If

                    'Nếu mục hình thức khác
                    If Not IsNothing(q8.Q824) Then
                        'Xử lý kỷ luật lao động bằng hình thức: liệt kê….. Là trái quy định
                        If Not IsNothing(q8.Q8241) Then
                            'Xuất kết luận
                            kl = New ThanhTraLaoDongModel.KetLuan
                            kl.NDKetLuan = "Xử lý kỷ luật lao động bằng hình thức " & q8.Q8241 & " là trái quy định;"
                            kl.IsViPham = TypeViPham.ViPham
                            kl.TenBangCauHoi = "CauHoi8"
                            kl.TenCotCauHoi = "Q8241"
                            kl.PhieuId = hidPhieuID.Value
                            data.KetLuans.AddObject(kl)
                            'Xuất kiến nghị
                            kn = New ThanhTraLaoDongModel.KienNghiDN
                            kn.NDKienNghi = "Bãi bỏ hình thức xử lý kỷ luật lao động không đúng pháp luật quy định: " & q8.Q8241 & ". Doanh nghiệp phải áp dụng hình thức xử lý kỷ luật lao động theo"
                            kn.TrichDanId = 49
                            kn.TenBangCauHoi = "Cauhoi8"
                            kn.PhieuId = hidPhieuID.Value
                            data.KienNghiDNs.AddObject(kn)
                        End If
                    End If
                Else
                    If Not IsNothing(q8.Q84) AndAlso q8.Q84 > 0 Then
                        '' Nếu có xử lý quy trình
                        If Not IsNothing(q8.Q851) Then
                            ''Nếu xử lý đúng quy trình
                            If Not q8.Q851 Then
                                ' xử lý sai quy trinh
                                kl = New ThanhTraLaoDongModel.KetLuan
                                Dim arrStr As New ArrayList
                                Dim strTenCotCauHoi As String = ""
                                If Not IsNothing(q8.Q8511) AndAlso q8.Q8511 Then
                                    arrStr.Add("không đúng thẩm quyền")
                                    strTenCotCauHoi = "Q8511;"
                                End If
                                If Not IsNothing(q8.Q8512) AndAlso q8.Q8512 Then
                                    arrStr.Add("quá thời hiệu")
                                    strTenCotCauHoi += "Q8512;"
                                End If
                                'If Not IsNothing(q8.Q8513) AndAlso q8.Q8513 Then
                                '    arrStr.Add("áp dụng sai hình thức kỷ luật theo Bộ Luật lao động")
                                '    strTenCotCauHoi += "Q8513;"
                                'End If
                                If Not IsNothing(q8.Q8514) AndAlso q8.Q8514 Then
                                    arrStr.Add("không chứng minh được lỗi của người lao động")
                                    strTenCotCauHoi += "Q8514;"
                                End If
                                If Not IsNothing(q8.Q8515) AndAlso q8.Q8515 Then
                                    arrStr.Add("họp xét kỷ luật không ghi biên bản họp")
                                    strTenCotCauHoi += "Q8515;"
                                End If
                                If Not String.IsNullOrEmpty(q8.Q8516) Then
                                    arrStr.Add(q8.Q8516)
                                    strTenCotCauHoi += "Q8516;"
                                End If
                                kl.NDKetLuan = "Xử lý bồi thường trách nhiệm vật chất: " + String.Join(", ", TryCast(arrStr.ToArray(GetType(String)), String())) + ";"
                                kl.IsViPham = TypeViPham.ViPham
                                kl.TenBangCauHoi = "CauHoi8"
                                kl.TenCotCauHoi = strTenCotCauHoi
                                kl.PhieuId = hidPhieuID.Value
                                data.KetLuans.AddObject(kl)
                            End If
                        End If
                    End If
                End If
                'Luu bang ketluan               
                data.SaveChanges()
            End If
        End Using
    End Sub
    Protected Sub XulyKetLuanCauHoi9()
        Using data As New ThanhTraLaoDongEntities
            Dim kl As KetLuan
            Dim kn As KienNghiDN
            'Tao moi ket luan dua vao cauhoi10
            Dim q9 = (From a In data.CauHoi9
                       Where a.PhieuId = hidPhieuID.Value).FirstOrDefault

            If Not IsNothing(q9) Then
                ' Xét mục 9.1 tranh chấp cá nhân
                If Not IsNothing(q9.Q9111) AndAlso Not IsNothing(q9.Q9112) AndAlso q9.Q9111 > 0 AndAlso q9.Q9112 > 0 Then
                    'Xuất kết luận
                    If q9.Q9111 = q9.Q9112 Then
                        kl = New ThanhTraLaoDongModel.KetLuan
                        kl.NDKetLuan = "Doanh nghiệp xảy ra " & String.Format(info, "{0:n0}", q9.Q9111) & " vụ tranh chấp lao động cá nhân, đã hòa giải thành công " & String.Format(info, "{0:n0}", q9.Q9112) & " vụ;"
                        kl.IsViPham = TypeViPham.KhongViPham
                        kl.TenBangCauHoi = "CauHoi9"
                        kl.TenCotCauHoi = "Q9111"
                        kl.PhieuId = hidPhieuID.Value
                        data.KetLuans.AddObject(kl)
                    ElseIf q9.Q9111 > q9.Q9112 Then
                        kl = New ThanhTraLaoDongModel.KetLuan
                        kl.NDKetLuan = "Doanh nghiệp xảy ra " & String.Format(info, "{0:n0}", q9.Q9111) & " vụ tranh chấp lao động cá nhân, chưa giải quyết xong " & String.Format(info, "{0:n0}", q9.Q9111 - q9.Q9112) & " vụ;"
                        kl.IsViPham = TypeViPham.ViPham
                        kl.TenBangCauHoi = "CauHoi9"
                        kl.TenCotCauHoi = "Q9111"
                        kl.PhieuId = hidPhieuID.Value
                        data.KetLuans.AddObject(kl)
                        'Xuất kiến nghị
                        kn = New ThanhTraLaoDongModel.KienNghiDN
                        kn.NDKienNghi = "Giải quyết dứt điểm " & String.Format(info, "{0:n0}", q9.Q9111 - q9.Q9112) & " vụ tranh chấp lao động cá nhân đúng trình tự và thời hiệu theo"
                        kn.TrichDanId = 81
                        kn.TenBangCauHoi = "Cauhoi9"
                        kn.PhieuId = hidPhieuID.Value
                        data.KienNghiDNs.AddObject(kn)
                    End If
                End If
                ' Xét mục 9.2 Tranh chấp tập thể
                If Not IsNothing(q9.Q92) Then
                    kl = New ThanhTraLaoDongModel.KetLuan
                    Dim arrKL As New ArrayList
                    If Not IsNothing(q9.Q921) Then
                        arrKL.Add("trong đó tranh chấp về lợi ích: " & String.Format(info, "{0:n0}", q9.Q921) & " vụ")
                        kl.TenCotCauHoi += "Q921;"
                    End If
                    If Not IsNothing(q9.Q922) Then
                        arrKL.Add("đình công tự phát: " & String.Format(info, "{0:n0}", q9.Q922) & " vụ")
                        kl.TenCotCauHoi += "Q922;"
                    End If
                    If Not String.IsNullOrEmpty(q9.Q923) Then
                        arrKL.Add("với mục đích đòi: " & q9.Q923)
                        kl.TenCotCauHoi += "Q923;"
                    End If
                    If Not IsNothing(q9.Q924) Then
                        arrKL.Add("người sử dụng lao động đã giải quyết: " & q9.Q924)
                        kl.TenCotCauHoi += "Q924;"
                    End If
                    If arrKL.Count > 0 Then
                        'Xuất kết luận
                        kl.NDKetLuan = "Doanh nghiệp đã xảy ra " & String.Format(info, "{0:n0}", q9.Q92) & " vụ tranh chấp tập thể " & String.Join(", ", TryCast(arrKL.ToArray(GetType(String)), String())) & ";"
                        kl.IsViPham = TypeViPham.ViPham
                        kl.TenBangCauHoi = "CauHoi9"
                        kl.TenCotCauHoi += "Q92"
                        kl.PhieuId = hidPhieuID.Value
                        data.KetLuans.AddObject(kl)
                        'Xuất kiến nghị
                        kn = New ThanhTraLaoDongModel.KienNghiDN
                        kn.NDKienNghi = "Xem xét kết quả giải quyết đình công của cơ quan quản lý nhà nước về lao động địa phương, thực hiện đúng quy định của pháp luật về lao động và thoả ước lao động tập thể, đảm bảo hài hòa về quyền và lợi ích tăng lương đối với " & q9.Q923 & " của người lao động để tránh xảy ra đình công, lãn công tương tự"
                        kn.PhieuId = hidPhieuID.Value
                        data.KienNghiDNs.AddObject(kn)
                    End If
                End If

                ' Xét mục 9.3 Tranh chấp lao động
                If Not IsNothing(q9.Q93) AndAlso Not IsNothing(q9.Q931) AndAlso q9.Q93 > 0 AndAlso q9.Q931 > 0 Then
                    kl = New ThanhTraLaoDongModel.KetLuan
                    If q9.Q93 = q9.Q931 Then
                        Dim arrKL As New ArrayList
                        arrKL.Add("đã giải quyết xong : " & String.Format(info, "{0:n0}", q9.Q931) & " vụ")
                        kl.TenCotCauHoi += "Q931;"
                        If Not String.IsNullOrEmpty(q9.Q932) Then
                            arrKL.Add("nguyên nhân: " & q9.Q932)
                            kl.TenCotCauHoi += "Q932;"
                        End If
                        If Not String.IsNullOrEmpty(q9.Q933) Then
                            arrKL.Add("kết quả giải quyết: " & q9.Q933)
                            kl.TenCotCauHoi += "Q933;"
                        End If
                        If arrKL.Count > 0 Then
                            'Xuất kết luận
                            kl.NDKetLuan = "Doanh nghiệp đã xảy ra " & String.Format(info, "{0:n0}", q9.Q93) & " vụ khiếu nại về lao động " & String.Join(", ", TryCast(arrKL.ToArray(GetType(String)), String())) & ";"
                            kl.IsViPham = TypeViPham.KhongViPham
                            kl.TenBangCauHoi = "CauHoi9"
                            kl.TenCotCauHoi += "Q93"
                            kl.PhieuId = hidPhieuID.Value
                            data.KetLuans.AddObject(kl)
                        End If
                    ElseIf q9.Q93 > q9.Q931 Then
                        'Xuất kết luận
                        kl.NDKetLuan = "Doanh nghiệp đã xảy ra " & String.Format(info, "{0:n0}", q9.Q93) & " vụ khiếu nại về lao động, chưa giải quyết xong " & String.Format(info, "{0:n0}", q9.Q93 - q9.Q931) & " vụ; "
                        If Not String.IsNullOrEmpty(q9.Q932) Then
                            kl.NDKetLuan += "nguyên nhân: " & q9.Q932
                            kl.TenCotCauHoi += "Q932;"
                        End If
                        kl.IsViPham = TypeViPham.ViPham
                        kl.TenBangCauHoi = "CauHoi9"
                        kl.TenCotCauHoi = "Q93"
                        kl.PhieuId = hidPhieuID.Value
                        data.KetLuans.AddObject(kl)
                        'Xuất kiến nghị
                        kn = New ThanhTraLaoDongModel.KienNghiDN
                        kn.NDKienNghi = "Giải quyết dứt điểm " & String.Format(info, "{0:n0}", q9.Q93 - q9.Q931) & " vụ khiếu nại về lao động đúng trình tự theo"
                        kn.KienNghiId = 82
                        kn.TenBangCauHoi = "Cauhoi9"
                        kn.PhieuId = hidPhieuID.Value
                        data.KienNghiDNs.AddObject(kn)
                    End If
                End If

                '' lưu dữ liệu lại
                data.SaveChanges()
            End If
        End Using
    End Sub
    Protected Sub XulyKetLuanCauHoi10()
        Using data As New ThanhTraLaoDongEntities
            Dim kl As New ThanhTraLaoDongModel.KetLuan
            Dim kn As New ThanhTraLaoDongModel.KienNghiDN
            'Tao moi ket luan dua vao cauhoi10
            Dim q10 = (From a In data.CauHoi10
                       Where a.PhieuId = hidPhieuID.Value).FirstOrDefault

            If Not IsNothing(q10) Then
                'Xét số ld nước ngoài có cấp phép 
                If Not IsNothing(q10.Q102) Then
                    If q10.Q102 > 0 Then
                        kl = New ThanhTraLaoDongModel.KetLuan
                        If Not IsNothing(q10.Q101) AndAlso q10.Q101 > 0 AndAlso Not IsNothing(q10.Q105) AndAlso q10.Q105 > 0 Then
                            If Not IsNothing(q10.Q101) AndAlso Not IsNothing(q10.Q102) Then
                                If (q10.Q101 - q10.Q102) > 0 Then
                                    kl.NDKetLuan = "Doanh nghiệp sử dụng " & q10.Q105 & " lao động là người nước ngoài; trong đó có " & q10.Q101 & _
                                            " người thuộc diện phải có giấy phép lao động. "
                                    kl.NDKetLuan += "Đã làm thủ tục cấp giấy phép cho " & q10.Q101 - q10.Q102 & "/" & q10.Q101 & " người phải có giấy phép lao động;"
                                Else
                                    kl.NDKetLuan = "Doanh nghiệp sử dụng " & q10.Q105 & " lao động là người nước ngoài; chưa làm thủ tục cấp giấy phép lao động cho " & q10.Q101 & " người thuộc diện phải có giấy phép lao động;"
                                End If
                            End If
                            kl.IsViPham = TypeViPham.KhongXet
                            kl.TenBangCauHoi = "CauHoi10"
                            kl.TenCotCauHoi = "Q102"
                            kl.PhieuId = hidPhieuID.Value
                            data.KetLuans.AddObject(kl)
                        End If
                    End If
                End If
                'Số người chưa được cấp hoặc gia hạn giấy phép lao động
                If Not IsNothing(q10.Q102) OrElse Not IsNothing(q10.Q106) Then
                    'Xuất kết luận
                    kl = New ThanhTraLaoDongModel.KetLuan
                    kl.NDKetLuan = "Doanh nghiệp sử dụng "
                    kl.NDKetLuan += IIf(IsNothing(q10.Q102), "", String.Format(info, "{0:n0}", q10.Q102) + " người nước ngoài chưa có giấy phép lao động")
                    kl.NDKetLuan += IIf(IsNothing(q10.Q106), "", IIf(IsNothing(q10.Q102), "", ", ") + String.Format(info, "{0:n0}", q10.Q106) + " người đã hết hạn giấy phép nhưng chưa gia hạn")
                    kl.NDKetLuan += ";"
                    'kl.NDKetLuan = "Doanh nghiệp sử dụng " + q10.Q102 + " người nước ngoài chưa có giấy phép lao động, " + q10.Q106 + " người đã hết hạn giấy phép nhưng chưa gia hạn;"
                    kl.IsViPham = TypeViPham.ViPham
                    kl.TenBangCauHoi = "CauHoi10"
                    kl.TenCotCauHoi = "Q102"
                    kl.PhieuId = hidPhieuID.Value
                    data.KetLuans.AddObject(kl)
                    'Xuất kiến nghị
                    kn = New ThanhTraLaoDongModel.KienNghiDN
                    kn.NDKienNghi = "Làm thủ tục "
                    kn.NDKienNghi += IIf(IsNothing(q10.Q102), "", "cấp giấy phép lao động " + String.Format(info, "{0:n0}", q10.Q102) + " lao động nước ngoài chưa có giấy phép")
                    kn.NDKienNghi += IIf(IsNothing(q10.Q106), "", IIf(IsNothing(q10.Q102), "gia hạn cho ", ", gia hạn cho ") + String.Format(info, "{0:n0}", q10.Q106) + " lao động nước ngoài đã hết hạn giấy phép lao động")
                    kn.NDKienNghi += " theo"

                    kn.TrichDanId = 80
                    kn.TenBangCauHoi = "Cauhoi10"
                    kn.PhieuId = hidPhieuID.Value
                    data.KienNghiDNs.AddObject(kn)
                End If
                ' Xét phần  phương án đào tạo người thay thế
                'If Not IsNothing(q10.Q103) Then
                '    'Xuất kết luận
                '    kl = New ThanhTraLaoDongModel.KetLuan
                '    kl.NDKetLuan = IIf(q10.Q103, "Đã", "Chưa") & " có phương án sử dụng lao động người Việt Nam thay thế;"
                '    kl.IsViPham = IIf(q10.Q103, TypeViPham.KhongViPham, TypeViPham.ViPham)
                '    kl.TenBangCauHoi = "CauHoi10"
                '    kl.TenCotCauHoi = "Q103"
                '    kl.PhieuId = hidPhieuID.Value
                '    data.KetLuans.AddObject(kl)
                '    'Xuất kiến nghị
                '    If kl.IsViPham = 0 Then
                '        kn = New ThanhTraLaoDongModel.KienNghiDN
                '        kn.NDKienNghi = "Lập phương án sử dụng người lao động Việt Nam và người nước ngoài trình cơ quan quản lý nhà nước về lao động địa phương theo"
                '        kn.TrichDanId = 50
                '        kn.TenBangCauHoi = "Cauhoi10"
                '        kn.PhieuId = hidPhieuID.Value
                '        data.KienNghiDNs.AddObject(kn)
                '    End If
                'End If

                ' Xét phần báo cáo định kỳ 6 tháng
                If Not IsNothing(q10.Q104) Then
                    'Xuất kết luận
                    kl = New ThanhTraLaoDongModel.KetLuan
                    kl.NDKetLuan = IIf(q10.Q104, "Đã", "Chưa") & " thực hiện chế độ báo cáo định kỳ sáu tháng và hàng năm về tình hình sử dụng người lao động nước ngoài với cơ quan quản lý về lao động địa phương;"
                    kl.IsViPham = IIf(q10.Q104, TypeViPham.KhongViPham, TypeViPham.ViPham)
                    kl.TenBangCauHoi = "CauHoi10"
                    kl.TenCotCauHoi = "Q104"
                    kl.PhieuId = hidPhieuID.Value
                    data.KetLuans.AddObject(kl)
                    'Xuất kiến nghị
                    If kl.IsViPham = 0 Then
                        kn = New ThanhTraLaoDongModel.KienNghiDN
                        kn.NDKienNghi = "Thực hiện chế độ báo cáo định kỳ sáu tháng và hàng năm về tình hình sử dụng lao động là người nước ngoài với cơ quan quản lý về lao động tại địa phương theo"
                        kn.TrichDanId = 51
                        kn.TenBangCauHoi = "Cauhoi10"
                        kn.PhieuId = hidPhieuID.Value
                        data.KienNghiDNs.AddObject(kn)
                    End If
                End If
                data.SaveChanges()
            End If
        End Using
    End Sub
    Protected Sub XulyKetLuanCauHoi11()
        Using data As New ThanhTraLaoDongEntities
            Dim kl As New KetLuan
            Dim kn As New KienNghiDN
            'Tao moi ket luan dua vao cauhoi4

            Dim q11 = (From a In data.CauHoi11
                        Where a.PhieuId = hidPhieuID.Value).FirstOrDefault

            If Not q11 Is Nothing Then
                If Not IsNothing(q11.Q11) Then
                    If q11.Q11 Then
                        kl = New ThanhTraLaoDongModel.KetLuan
                        Dim ld1, ld2 As Integer
                        If Not IsNothing(q11.Q1111) Then
                            ld1 = q11.Q1111
                        End If
                        If Not IsNothing(q11.Q1112) Then
                            ld2 = q11.Q1112
                        End If
                        If (ld1 + ld2) > 0 Then
                            Dim strCombine As String = ""
                            Dim strTenCotCauHoi As String = ""
                            Dim arrCombine As New ArrayList
                            Dim arrKN As New ArrayList
                            strTenCotCauHoi = IIf(ld1 > 0, "Q1111;", "")
                            strTenCotCauHoi += IIf(ld2 > 0, "Q1112;", "")
                            strCombine += "Chưa thực hiện "
                            If Not IsNothing(q11.Q112) AndAlso Not q11.Q112 Then
                                arrCombine.Add("lập sổ theo dõi riêng")
                                strTenCotCauHoi += "Q112;"
                                arrKN.Add("lập sổ theo dõi riêng")
                            End If
                            If Not IsNothing(q11.Q113) AndAlso Not q11.Q113 Then
                                arrCombine.Add("ký hợp lao động với người đại diện theo pháp luật của trẻ dưới 15 tuổi")
                                strTenCotCauHoi += "Q113;"
                                arrKN.Add("ký hợp lao động với người đại diện theo pháp luật của trẻ dưới 15 tuổi")
                            End If
                            If Not IsNothing(q11.Q114) AndAlso Not q11.Q114 Then
                                arrCombine.Add("tránh bố trí làm các công việc nặng nhọc, nguy hiểm")
                                strTenCotCauHoi += "Q114;"
                                arrKN.Add("tránh bố trí làm các công việc nặng nhọc, nguy hiểm")
                            End If
                            If Not IsNothing(q11.Q115) AndAlso Not q11.Q115 Then
                                arrCombine.Add("lập hồ sơ sức khỏe")
                                strTenCotCauHoi += "Q115;"
                                arrKN.Add("lập hồ sơ sức khỏe")
                            End If
                            If Not IsNothing(q11.Q116) AndAlso Not q11.Q116 Then
                                arrCombine.Add("giảm giờ làm")
                                strTenCotCauHoi += "Q116;"
                                arrKN.Add("giảm giờ làm việc")
                            End If
                            If Not IsNothing(q11.Q117) AndAlso Not q11.Q117 Then
                                arrCombine.Add("nghỉ ngày lễ, nghỉ hàng năm")
                                strTenCotCauHoi += "Q117;"
                                arrKN.Add("nghỉ ngày lễ, nghỉ hàng năm")
                            End If
                            If Not IsNothing(q11.Q118) AndAlso q11.Q118 Then
                                arrCombine.Add("tránh bố trí công việc cấm lao động chưa thành niên")
                                strTenCotCauHoi += "Q118;"
                                arrKN.Add("tránh bố trí công việc cấm lao động chưa thành niên")
                            End If
                            If Not IsNothing(q11.Q119) AndAlso Not q11.Q119 Then
                                arrCombine.Add("tham gia bảo hiểm xã hội")
                                strTenCotCauHoi += "Q119;"
                                arrKN.Add("tham gia bảo hiểm xã hội đối với lao động chưa thành niên")
                            End If
                            If arrCombine.Count > 0 Then
                                'Xuất kết luận
                                kl.NDKetLuan = strCombine + String.Join(", ", TryCast(arrCombine.ToArray(GetType(String)), String())) + " đối với " & String.Format(info, "{0:n0}", (ld1 + ld2)) & " lao động chưa thành niên;"
                                kl.IsViPham = TypeViPham.ViPham
                                kl.TenBangCauHoi = "CauHoi11"
                                kl.TenCotCauHoi = strTenCotCauHoi
                                kl.PhieuId = hidPhieuID.Value
                                data.KetLuans.AddObject(kl)
                                'Xuất kiến nghị
                                kn = New ThanhTraLaoDongModel.KienNghiDN
                                kn.NDKienNghi = "Thực hiện " & String.Join(", ", TryCast(arrKN.ToArray(GetType(String)), String())) + " và các quy định khác về sử dụng lao động chưa thành niên theo"
                                kn.TrichDanId = 52
                                kn.TenBangCauHoi = "Cauhoi11"
                                kn.PhieuId = hidPhieuID.Value
                                data.KienNghiDNs.AddObject(kn)
                            End If
                        Else
                            kl = New ThanhTraLaoDongModel.KetLuan
                            kl.NDKetLuan = "Doanh nghiệp không sử dụng lao động chưa thành niên, lao động người cao tuổi, lao động người tàn tật;"
                            kl.IsViPham = TypeViPham.KhongViPham
                            kl.TenBangCauHoi = "CauHoi11"
                            kl.TenCotCauHoi = "Q11"
                            kl.PhieuId = hidPhieuID.Value
                            data.KetLuans.AddObject(kl)
                        End If
                    End If
                    data.SaveChanges()
                End If
            End If
        End Using
    End Sub
    Protected Sub XulyKetLuanCauHoi12()
        Using data As New ThanhTraLaoDongEntities
            Dim kl As New ThanhTraLaoDongModel.KetLuan
            Dim kn As New ThanhTraLaoDongModel.KienNghiDN
            'Tao moi ket luan dua vao cauhoi12
            Dim q12 = (From a In data.CauHoi12
                                   Where a.PhieuId = hidPhieuID.Value).FirstOrDefault

            If Not IsNothing(q12) Then
                '12. Lao động nữ 
                If Not IsNothing(q12.Q121) Then
                    kl = New ThanhTraLaoDongModel.KetLuan
                    If Not IsNothing(q12.Q1211) Then
                        'Xuất kết luận
                        kl.NDKetLuan = IIf(Not q12.Q1211, "Chưa thực hiện tốt việc tránh", "Đã thực hiện tốt việc tránh") + " bố trí nữ làm công việc đặc biệt nặng nhọc, độc hại, nguy hiểm;"
                        kl.IsViPham = IIf(Not q12.Q1211, TypeViPham.ViPham, TypeViPham.KhongViPham)
                        kl.TenBangCauHoi = "CauHoi12"
                        kl.TenCotCauHoi = "Q1211"
                        kl.PhieuId = hidPhieuID.Value
                        data.KetLuans.AddObject(kl)
                        'Xuất kiến nghị
                        If kl.IsViPham = 0 Then
                            kn = New ThanhTraLaoDongModel.KienNghiDN
                            kn.NDKienNghi = "Tránh bố trí lao động nữ làm công việc đặc biệt nặng nhọc, độc hại, nguy hiểm và thực hiện giảm giờ làm việc đối với nữ làm công việc nặng nhọc, độc hại, nguy hiểm theo"
                            kn.TrichDanId = 53
                            kn.TenBangCauHoi = "Cauhoi12"
                            kn.PhieuId = hidPhieuID.Value
                            data.KienNghiDNs.AddObject(kn)
                        End If
                    End If
                    If Not IsNothing(q12.Q1212) Then
                        'Xuất kết luận
                        kl = New ThanhTraLaoDongModel.KetLuan
                        kl.NDKetLuan = IIf(q12.Q1212, "Đã thực hiện tốt", "Chưa thực hiện") + " giảm giờ làm đối với lao động nữ có thai, nuôi con nhỏ dưới 12 tháng;"
                        kl.IsViPham = IIf(q12.Q1212, TypeViPham.KhongViPham, TypeViPham.ViPham)
                        kl.TenBangCauHoi = "CauHoi12"
                        kl.TenCotCauHoi = "Q1212"
                        kl.PhieuId = hidPhieuID.Value
                        data.KetLuans.AddObject(kl)
                        'Xuất kiến nghị
                        If kl.IsViPham = 0 Then
                            kn = New ThanhTraLaoDongModel.KienNghiDN
                            kn.NDKienNghi = "Thực hiện giảm giờ làm đối với lao động nữ có thai, nuôi con nhỏ dưới 12 tháng theo"
                            kn.TrichDanId = 54
                            kn.TenBangCauHoi = "Cauhoi12"
                            kn.PhieuId = hidPhieuID.Value
                            data.KienNghiDNs.AddObject(kn)
                        End If
                    End If
                    If Not IsNothing(q12.Q1213) Then
                        'Xuất kết luận
                        kl = New ThanhTraLaoDongModel.KetLuan
                        kl.NDKetLuan = IIf(q12.Q1213, "Đã thực hiện tốt bố trí chỗ vệ sinh phụ nữ và trả lương 30 phút vệ sinh mỗi ngày trong thời gian hành kinh theo quy định;", "Bố trí chưa đủ chỗ vệ sinh phụ nữ và chưa trả lương 30 phút vệ sinh mỗi ngày trong thời gian hành kinh theo quy định;") '+ " bố trí chỗ vệ sinh phụ nữ và trả lương 30 phút vệ sinh mỗi ngày trong thời gian hành kinh theo quy định;"
                        kl.IsViPham = IIf(q12.Q1213, TypeViPham.KhongViPham, TypeViPham.ViPham)
                        kl.TenBangCauHoi = "CauHoi12"
                        kl.TenCotCauHoi = "Q1213"
                        kl.PhieuId = hidPhieuID.Value
                        data.KetLuans.AddObject(kl)
                        'Xuất kiến nghị
                        If kl.IsViPham = 0 Then
                            kn = New ThanhTraLaoDongModel.KienNghiDN
                            kn.NDKienNghi = "Bố trí đầy đủ chỗ vệ sinh phụ nữ và trả lương 30 phút vệ sinh mỗi ngày trong thời gian hành kinh theo"
                            kn.TrichDanId = 55
                            kn.TenBangCauHoi = "Cauhoi12"
                            kn.PhieuId = hidPhieuID.Value
                            data.KienNghiDNs.AddObject(kn)
                        End If
                    End If
                End If

                '12. bình đẳng giới
                If Not q12.Q122 Is Nothing And Not q12.Q123 Is Nothing Then
                    ' tuyển dụng trả công phân biệt Nam, Nữ ?
                    If Not IsNothing(q12.Q122) Then
                        kl = New ThanhTraLaoDongModel.KetLuan
                        If q12.Q122 Then
                            kl.NDKetLuan = "Đã áp dụng chính sách tuyển dụng, trả công giữa nam và nữ như nhau;"
                            kl.IsViPham = TypeViPham.KhongViPham
                        Else
                            'Xử lý kết luận
                            kl.NDKetLuan = "Áp dụng chính sách tuyển dụng, trả công giữa nam và nữ chưa bình đẳng;"
                            kl.IsViPham = TypeViPham.ViPham
                            'Xuất kiến nghị
                            kn = New ThanhTraLaoDongModel.KienNghiDN
                            kn.NDKienNghi = "Áp dụng chính sách tuyển dụng lao động và trả công bình đẳng giữa nam và nữ theo"
                            kn.TrichDanId = 56
                            kn.TenBangCauHoi = "Cauhoi12"
                            kn.PhieuId = hidPhieuID.Value
                            data.KienNghiDNs.AddObject(kn)
                        End If
                        kl.TenBangCauHoi = "CauHoi12"
                        kl.TenCotCauHoi = "Q122"
                        kl.PhieuId = hidPhieuID.Value
                        data.KetLuans.AddObject(kl)
                    End If

                    'đào tạo, bổ nhiệm phân biệt nam nữ ?
                    If Not IsNothing(q12.Q123) Then
                        kl = New ThanhTraLaoDongModel.KetLuan
                        If q12.Q123 Then
                            kl.NDKetLuan = "Đã áp dụng chính sách đào tạo và bổ nhiệm cán bộ quản lý giữa nam và nữ như nhau;"
                            kl.IsViPham = TypeViPham.KhongViPham
                        Else
                            'Xuất kết luận
                            kl.NDKetLuan = "Áp dụng chính sách đào tạo và bổ nhiệm cán bộ quản lý giữa nam và nữ chưa bình đẳng;"
                            kl.IsViPham = TypeViPham.ViPham
                            'Xuất kiến nghị
                            kn = New ThanhTraLaoDongModel.KienNghiDN
                            kn.NDKienNghi = "Áp dụng chính sách đào tạo và bổ nhiệm cán bộ quản lý bình đẳng giữa nam và nữ theo"
                            kn.TrichDanId = 56
                            kn.TenBangCauHoi = "Cauhoi12"
                            kn.PhieuId = hidPhieuID.Value
                            data.KienNghiDNs.AddObject(kn)
                        End If
                        kl.TenBangCauHoi = "CauHoi12"
                        kl.TenCotCauHoi = "Q123"
                        kl.PhieuId = hidPhieuID.Value
                        data.KetLuans.AddObject(kl)
                    End If
                    ''ti le can bo
                    If Not IsNothing(q12.Q1241) AndAlso Not IsNothing(q12.Q1242) Then
                        kl = New ThanhTraLaoDongModel.KetLuan
                        kl.NDKetLuan = "Tỷ lệ lao động nữ được cử đi học, tập huấn chiếm " & q12.Q1241 & " % tỷ lệ cán bộ quản lý là nữ chiếm " & q12.Q1242 & " %;"
                        kl.IsViPham = TypeViPham.KhongXet
                        kl.TenBangCauHoi = "CauHoi12"
                        kl.TenCotCauHoi = "Q1241;Q1242"
                        kl.PhieuId = hidPhieuID.Value
                        data.KetLuans.AddObject(kl)
                    End If
                End If
                'Luu bang ketluan           
                data.SaveChanges()
            End If
        End Using
    End Sub
    Protected Sub btnInBienBan_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnInBienBan.Click
        Response.Redirect("../../Page/BienBanThanhTra/BienBanThanhTra.aspx?phieuId=" & hidPhieuID.Value)
    End Sub

    Protected Sub btnInKetLuan_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnInKetLuan.Click
        Response.Redirect("../../Page/BienBanThanhTra/PhieuKetLuan.aspx?phieuId=" & hidPhieuID.Value)
    End Sub

    Protected Sub btnINBBVP_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnINBBVP.Click
        Response.Redirect("../../Page/BienBanThanhTra/BienBanViPham.aspx?phieuId=" & hidPhieuID.Value)
    End Sub
    Protected Sub btnXemKetLuan_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnXemKetLuan.Click
        If Request.Path.Contains("BienBanThanhTra") Then
            Response.Redirect("../../Page/BienBanThanhTra/XemKetLuan.aspx?phieuid=" & hidPhieuID.Value)
        ElseIf Request.Path.Contains("PhieuKiemTra") Then
            Response.Redirect("../../Page/PhieuKiemTra/XemKetLuan.aspx?phieuid=" & hidPhieuID.Value)
        ElseIf Request.Path.Contains("DoanhNghiep") Then
            Response.Redirect("../../Page/XemKetLuan.aspx?phieuid=" & hidPhieuID.Value)
        End If

    End Sub
    Protected Sub btnInPhieuKienNghi_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnInPhieuKienNghi.Click
        If hidIsUser.Value = 2 Then
            Response.Redirect("../../Page/PhieuKienNghi.aspx?phieuId=" & hidPhieuID.Value)
        Else
            Response.Redirect("../../Page/PhieuKiemTra/PhieuKienNghi.aspx?phieuId=" & hidPhieuID.Value)
        End If

    End Sub
#End Region



End Class
