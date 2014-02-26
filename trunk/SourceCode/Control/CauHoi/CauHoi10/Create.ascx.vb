Imports System.Data
Imports ThanhTraLaoDongModel
Imports Cls_Common
Imports SecurityService
Partial Class Control_CauHoi10_Create
    Inherits System.Web.UI.UserControl
    Private Shared ReadOnly log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

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
                'Neu ModePhieu la xem chi tiet
                hidPhieuID.Value = IIf(IsNothing(Session("phieuid")), 0, Session("phieuid"))
                hidIsUser.Value = IIf(IsNothing(Session("IsUser")), 0, Session("IsUser"))
                hidModePhieu.Value = IIf(IsNothing(Session("ModePhieu")), 0, Session("ModePhieu"))
                'Neu ModePhieu la xem chi tiet
                If hidModePhieu.Value = ModePhieu.Detail Then
                    btnSave.Visible = False
                    btnReset.Visible = False
                End If
                'Load nội dung cauhoi10
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
    Protected Function Save() As Boolean
        Using data As New ThanhTraLaoDongEntities

            Try
                If Not Session("Username") = "" Then
                    Dim iQ101, iQ102, iQ105, iQ106 As Integer
                    iQ101 = GetNumberByFormat(txtQ101.Text.Trim()) 'Tổng số lao động là người nước ngoài
                    iQ102 = GetNumberByFormat(txtQ102.Text.Trim()) 'Số người phải có giấy phép lao động
                    iQ105 = GetNumberByFormat(txtQ105.Text.Trim()) 'Số chưa được cấp giấy phép
                    iQ106 = GetNumberByFormat(txtQ106.Text.Trim()) 'Số người chưa được gia hạn giấy phép lao động
                    Dim cauhoi10 As CauHoi10 = (From q In data.CauHoi10 Where q.PhieuId = hidPhieuID.Value).FirstOrDefault()
                    ' Check Exists CauHoi10 by PhieID
                    If cauhoi10 Is Nothing Then
                        cauhoi10 = New CauHoi10
                        cauhoi10.PhieuId = hidPhieuID.Value
                        cauhoi10.Q105 = IIf(iQ105 > 0, iQ105, Nothing) 'Tổng số lao động là người nước ngoài
                        If iQ105 > 0 Then
                            cauhoi10.Q101 = IIf(iQ101 > 0, iQ101, Nothing) 'Số người phải có giấy phép lao động
                            cauhoi10.Q102 = IIf(iQ102 > 0, iQ102, Nothing) 'Số chưa được cấp giấy phép
                            'cauhoi10.Q103 = chkQ103.SelectedValue = "1"
                            'If chkQ103.SelectedValue = "" Then
                            '    cauhoi10.Q103 = Nothing 'Phương án đào tạo người Việt Nam thay thế người nước ngoài
                            'End If
                            cauhoi10.Q104 = chkQ104.SelectedValue = "1"
                            If chkQ104.SelectedValue = "" Then
                                cauhoi10.Q104 = Nothing 'Thực hiện chế độ báo cáo định kỳ sáu tháng và hằng năm về tình hình sử dụng người lao động nước ngoài với cơ quan quản lý về lao động tại địa phương
                            End If
                        Else
                            cauhoi10.Q101 = Nothing
                            cauhoi10.Q102 = Nothing
                            cauhoi10.Q103 = Nothing
                            cauhoi10.Q104 = Nothing
                        End If
                        cauhoi10.Q106 = IIf(iQ106 > 0, iQ106, Nothing) 'Số người chưa được gia hạn giấy phép lao động
                        cauhoi10.NguoiTao = Session("Username")
                        cauhoi10.NgayTao = Date.Now
                        data.CauHoi10.AddObject(cauhoi10)
                        'Luu cau hoi da tra loi
                        Dim pn = (From a In data.PhieuNhapHeaders Where a.PhieuID = hidPhieuID.Value Select a).FirstOrDefault
                        pn.CauHoiDaTraLoi = pn.CauHoiDaTraLoi & "10;"
                        data.SaveChanges()
                        If hidIsUser.Value = 1 Then
                            Insert_App_Log("Insert  Cauhoi10: PhieuId - " & hidPhieuID.Value, Function_Name.BienBanThanhTra, Audit_Type.Create, Request.ServerVariables("REMOTE_ADDR"), Session("UserName"))
                        ElseIf hidIsUser.Value = 2 Then
                            Insert_App_Log("Insert  Cauhoi10: PhieuId - " & hidPhieuID.Value, Function_Name.PhieuKiemTra, Audit_Type.Create, Request.ServerVariables("REMOTE_ADDR"), Session("UserName"))
                        End If
                        Return True
                    Else
                        cauhoi10.Q105 = IIf(iQ105 > 0, iQ105, Nothing) 'Tổng số lao động là người nước ngoài
                        If iQ105 > 0 Then
                            cauhoi10.Q101 = IIf(iQ101 > 0, iQ101, Nothing) 'Số người phải có giấy phép lao động
                            cauhoi10.Q102 = IIf(iQ102 > 0, iQ102, Nothing) 'Số chưa được cấp giấy phép
                            'cauhoi10.Q103 = chkQ103.SelectedValue = "1"
                            'If chkQ103.SelectedValue = "" Then
                            '    cauhoi10.Q103 = Nothing 'Phương án đào tạo người Việt Nam thay thế người nước ngoài
                            'End If
                            cauhoi10.Q104 = chkQ104.SelectedValue = "1"
                            If chkQ104.SelectedValue = "" Then
                                cauhoi10.Q104 = Nothing 'Thực hiện chế độ báo cáo định kỳ sáu tháng và hằng năm về tình hình sử dụng người lao động nước ngoài với cơ quan quản lý về lao động tại địa phương
                            End If
                        Else
                            cauhoi10.Q101 = Nothing
                            cauhoi10.Q102 = Nothing
                            cauhoi10.Q103 = Nothing
                            cauhoi10.Q104 = Nothing
                        End If
                        cauhoi10.Q106 = IIf(iQ106 > 0, iQ106, Nothing) 'Số người chưa được gia hạn giấy phép lao động
                        cauhoi10.NguoiSua = Session("Username")
                        cauhoi10.NgaySua = Date.Now
                        'Luu ngay sua, nguoi sua phieu
                        Dim pn = (From a In data.PhieuNhapHeaders Where a.PhieuID = hidPhieuID.Value Select a).FirstOrDefault
                        pn.NgaySua = Date.Now
                        pn.NguoiSua = Session("Username")
                        data.SaveChanges()
                        If hidIsUser.Value = 1 Then
                            Insert_App_Log("Update  Cauhoi10: PhieuId - " & hidPhieuID.Value, Function_Name.BienBanThanhTra, Audit_Type.Edit, Request.ServerVariables("REMOTE_ADDR"), Session("UserName"))
                        ElseIf hidIsUser.Value = 2 Then
                            Insert_App_Log("Update  Cauhoi10: PhieuId - " & hidPhieuID.Value, Function_Name.PhieuKiemTra, Audit_Type.Edit, Request.ServerVariables("REMOTE_ADDR"), Session("UserName"))
                        End If
                        Return True
                    End If
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
            'Tiêu đề doanh nghiệp
            Dim DNId As Integer = IIf(IsNothing(Request.QueryString("DNId")), 0, CInt(Request.QueryString("DNId")))
            Dim dn = (From a In data.DoanhNghieps Where a.DoanhNghiepId = DNId).SingleOrDefault
            If Not IsNothing(dn) Then
                lblTitleCompany.Text = "TÌNH HÌNH THỰC HIỆN PHÁP LUẬT LAO ĐỘNG CỦA " & dn.TenDoanhNghiep.Trim()
            End If

            Dim p As CauHoi10 = (From q In data.CauHoi10 Where q.PhieuId = hidPhieuID.Value Select q).FirstOrDefault
            If Not p Is Nothing Then
                txtQ105.Text = IIf(IsNothing(p.Q105) = True, "", p.Q105) 'Tổng số lao động là người nước ngoài
                txtQ101.Text = IIf(IsNothing(p.Q101) = True, "", p.Q101) 'Số người phải có giấy phép lao động
                txtQ102.Text = IIf(IsNothing(p.Q102) = True, "", p.Q102) 'Số chưa được cấp giấy phép

                'If p.Q103.HasValue Then
                '    chkQ103.SelectedValue = Math.Abs(CInt(p.Q103)) 'Phương án đào tạo người Việt Nam thay thế người nước ngoài
                'Else
                '    chkQ103.ClearSelection()
                'End If
                If p.Q104.HasValue Then
                    chkQ104.SelectedValue = Math.Abs(CInt(p.Q104)) 'Thực hiện chế độ báo cáo định kỳ sáu tháng và hằng năm về tình hình sử dụng người lao động nước ngoài với cơ quan quản lý về lao động tại địa phương
                Else
                    chkQ104.ClearSelection()
                End If
                txtQ106.Text = IIf(IsNothing(p.Q106) = True, "", p.Q106) 'Số người chưa được gia hạn giấy phép lao động
            End If
        End Using
    End Sub
    Protected Sub ResetControl()
        txtQ105.Text = ""
        txtQ101.Text = ""
        txtQ102.Text = ""
        'chkQ103.ClearSelection()
        chkQ104.ClearSelection()
        txtQ106.Text = ""
    End Sub
#End Region
#Region "Event for control "
    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Session("phieuid") = hidPhieuID.Value
        Session("IsUser") = hidIsUser.Value
        Session("ModePhieu") = hidModePhieu.Value

        If Save() Then
            Dim iDN = Request.QueryString("DNId")
            Excute_Javascript("AlertboxRedirect('Mời bạn nhập tiếp mục 11.','CauHoi11.aspx?DNId=" & iDN & "');", Me.Page, True)
        Else
            Excute_Javascript("Alertbox('Cập nhật thất bại.');", Me.Page, True)
        End If
    End Sub

    Protected Sub btnReset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReset.Click
        Using data As New ThanhTraLaoDongEntities
            Dim q10 = (From p In data.CauHoi10 Where p.PhieuId = hidPhieuID.Value).FirstOrDefault
            If Not IsNothing(q10) Then
                ShowData()
            Else
                ResetControl()
            End If
        End Using
    End Sub
#End Region
End Class
