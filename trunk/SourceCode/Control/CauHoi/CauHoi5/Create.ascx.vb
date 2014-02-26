Imports System.Data
Imports ThanhTraLaoDongModel
Imports Cls_Common
Imports SecurityService

Partial Class Control_CauHoi5_Create
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

                If hidModePhieu.Value = ModePhieu.Detail Then
                    btnSave.Visible = False
                    btnReset.Visible = False
                End If

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
                    Dim q5 = (From q In data.CauHoi5 Where q.PhieuId = hidPhieuID.Value Select q).FirstOrDefault
                    Dim iQ521, iQ522, iQ531, iQ532, iQ533 As Integer
                    'iQ511 = GetNumberByFormat(txtQ511.Text.Trim())
                    'iQ512 = GetNumberByFormat(txtQ512.Text.Trim())
                    'iQ513 = GetNumberByFormat(txtQ513.Text.Trim())
                    iQ521 = GetNumberByFormat(txtQ521.Text.Trim())
                    iQ522 = GetNumberByFormat(txtQ522.Text.Trim())
                    iQ531 = GetNumberByFormat(txtQ531.Text.Trim())
                    iQ532 = GetNumberByFormat(txtQ532.Text.Trim())
                    iQ533 = GetNumberByFormat(txtQ533.Text.Trim())


                    If q5 Is Nothing Then
                        q5 = New CauHoi5
                        q5.PhieuId = hidPhieuID.Value
                        '5.1. Số giờ làm việc:
                        q5.Q511 = Nothing '5.1. Số giờ làm việc
                        q5.Q512 = Nothing
                        q5.Q513 = Nothing
                        
                        If txtQ511.Text.Trim().Equals("") OrElse txtQ511.Text.Trim().Equals("0") Then
                            q5.Q511 = 0
                        Else
                            q5.Q511 = txtQ511.Text.Trim()
                        End If
                        If txtQ512.Text.Trim().Equals("") OrElse txtQ512.Text.Trim().Equals("0") Then
                            q5.Q512 = 0
                        Else
                            q5.Q512 = txtQ512.Text.Trim()
                        End If
                        If txtQ513.Text.Trim().Equals("") OrElse txtQ513.Text.Trim().Equals("0") Then
                            q5.Q513 = 0
                        Else
                            q5.Q513 = txtQ513.Text.Trim()
                        End If
                        If txtQ514.Text.Trim().Equals("") OrElse txtQ514.Text.Trim().Equals("0") Then
                            q5.Q514 = 0
                        Else
                            q5.Q514 = txtQ514.Text.Trim()
                        End If
                        If txtQ515.Text.Trim().Equals("") OrElse txtQ515.Text.Trim().Equals("0") Then
                            q5.Q515 = 0
                        Else
                            q5.Q515 = txtQ515.Text.Trim()
                        End If
                        '5.2. Làm thêm giờ:
                        q5.Q52 = Nothing
                        If chkQ52.SelectedValue <> "" Then
                            q5.Q52 = chkQ52.SelectedValue = "1"
                        End If

                        q5.Q521 = IIf(iQ521 > 0 And chkQ52.SelectedValue <> "" And chkQ52.SelectedValue = "1", iQ521, Nothing)
                        q5.Q522 = IIf(iQ522 > 0 And chkQ52.SelectedValue <> "" And chkQ52.SelectedValue = "1", iQ522, Nothing)

                        '5.3. Thực hiện nghỉ phép hàng năm hưởng nguyên lương.
                        q5.Q53 = Nothing
                        If chkQ53.SelectedValue <> "" Then
                            q5.Q53 = chkQ53.SelectedValue = "1"
                        End If
                        q5.Q531 = IIf(iQ531 > 0 And chkQ53.SelectedValue <> "" And chkQ53.SelectedValue = "1", iQ531, Nothing) 'Lao động làm công việc bình thường
                        q5.Q532 = IIf(iQ532 > 0 And chkQ53.SelectedValue <> "" And chkQ53.SelectedValue = "1", iQ532, Nothing) 'Lao động làm công việc nặng nhọc, độc hại, nguy hiểm
                        q5.Q533 = IIf(iQ533 > 0 And chkQ53.SelectedValue <> "" And chkQ53.SelectedValue = "1", iQ533, Nothing) 'Lao động làm công việc đặc biệt nặng nhọc, độc hại, nguy hiểm

                        '5.4. Thực hiện nghỉ việc riêng hưởng nguyên lương lương:
                        q5.Q54 = Nothing
                        If chkQ54.SelectedValue <> "" Then
                            q5.Q54 = chkQ54.SelectedValue = "1"
                            q5.Q541 = Nothing
                        Else
                            q5.Q541 = txtQ541.Text.Trim()
                        End If

                        '5.5 Nghỉ việc riêng không hưởng lương
                        q5.Q55 = Nothing
                        If chkQ55.SelectedValue <> "" Then
                            q5.Q55 = chkQ55.SelectedValue = "1"
                        End If

                        q5.NguoiTao = Session("Username")
                        q5.NgayTao = Date.Now
                        data.CauHoi5.AddObject(q5)

                        'Luu cau hoi da tra loi
                        Dim pn = (From a In data.PhieuNhapHeaders Where a.PhieuID = hidPhieuID.Value Select a).FirstOrDefault
                        pn.CauHoiDaTraLoi = pn.CauHoiDaTraLoi & "5;"

                        data.SaveChanges()
                        If hidIsUser.Value = 1 Then
                            Insert_App_Log("Insert  Cauhoi5: PhieuId - " & hidPhieuID.Value, Function_Name.BienBanThanhTra, Audit_Type.Create, Request.ServerVariables("REMOTE_ADDR"), Session("UserName"))
                        ElseIf hidIsUser.Value = 2 Then
                            Insert_App_Log("Insert  Cauhoi5: PhieuId - " & hidPhieuID.Value, Function_Name.PhieuKiemTra, Audit_Type.Create, Request.ServerVariables("REMOTE_ADDR"), Session("UserName"))

                        End If

                    Else
                        '5.1. Số giờ làm việc:
                        q5.Q511 = Nothing '5.1. Số giờ làm việc
                        q5.Q512 = Nothing
                        q5.Q513 = Nothing
                        
                        If txtQ511.Text.Trim().Equals("") OrElse txtQ511.Text.Trim().Equals("0") Then
                            q5.Q511 = 0
                        Else
                            q5.Q511 = txtQ511.Text.Trim()
                        End If
                        If txtQ512.Text.Trim().Equals("") OrElse txtQ512.Text.Trim().Equals("0") Then
                            q5.Q512 = 0
                        Else
                            q5.Q512 = txtQ512.Text.Trim()
                        End If
                        If txtQ513.Text.Trim().Equals("") OrElse txtQ513.Text.Trim().Equals("0") Then
                            q5.Q513 = 0
                        Else
                            q5.Q513 = txtQ513.Text.Trim()
                        End If
                        If txtQ514.Text.Trim().Equals("") OrElse txtQ514.Text.Trim().Equals("0") Then
                            q5.Q514 = 0
                        Else
                            q5.Q514 = txtQ514.Text.Trim()
                        End If
                        If txtQ515.Text.Trim().Equals("") OrElse txtQ515.Text.Trim().Equals("0") Then
                            q5.Q515 = 0
                        Else
                            q5.Q515 = txtQ515.Text.Trim()
                        End If
                        '5.2. Làm thêm giờ:
                        q5.Q52 = Nothing
                        If chkQ52.SelectedValue <> "" Then
                            q5.Q52 = chkQ52.SelectedValue = "1"
                        End If

                        q5.Q521 = IIf(iQ521 > 0 And chkQ52.SelectedValue <> "" And chkQ52.SelectedValue = "1", iQ521, Nothing)
                        q5.Q522 = IIf(iQ522 > 0 And chkQ52.SelectedValue <> "" And chkQ52.SelectedValue = "1", iQ522, Nothing)

                        '5.3. Thực hiện nghỉ phép hàng năm hưởng nguyên lương.
                        q5.Q53 = Nothing
                        If chkQ53.SelectedValue <> "" Then
                            q5.Q53 = chkQ53.SelectedValue = "1"
                        End If
                        q5.Q531 = IIf(iQ531 > 0 And chkQ53.SelectedValue <> "" And chkQ53.SelectedValue = "1", iQ531, Nothing) 'Lao động làm công việc bình thường
                        q5.Q532 = IIf(iQ532 > 0 And chkQ53.SelectedValue <> "" And chkQ53.SelectedValue = "1", iQ532, Nothing) 'Lao động làm công việc nặng nhọc, độc hại, nguy hiểm
                        q5.Q533 = IIf(iQ533 > 0 And chkQ53.SelectedValue <> "" And chkQ53.SelectedValue = "1", iQ533, Nothing) 'Lao động làm công việc đặc biệt nặng nhọc, độc hại, nguy hiểm

                        '5.4. Thực hiện nghỉ việc riêng hưởng nguyên lương lương:
                        q5.Q54 = Nothing
                        If chkQ54.SelectedValue <> "" Then
                            q5.Q54 = chkQ54.SelectedValue = "1"
                            q5.Q541 = Nothing
                        Else
                            q5.Q541 = txtQ541.Text.Trim()
                        End If

                        '5.5 Nghỉ việc riêng không hưởng lương
                        q5.Q55 = Nothing
                        If chkQ55.SelectedValue <> "" Then
                            q5.Q55 = chkQ55.SelectedValue = "1"
                        End If

                        q5.NguoiSua = Session("Username")
                        q5.NgaySua = Date.Now

                        'Luu ngay sua, nguoi sua phieu
                        Dim pn = (From a In data.PhieuNhapHeaders Where a.PhieuID = hidPhieuID.Value Select a).FirstOrDefault
                        pn.NgaySua = Date.Now
                        pn.NguoiSua = Session("Username")
                        data.SaveChanges()
                        If hidIsUser.Value = 1 Then
                            Insert_App_Log("Update  Cauhoi5: PhieuId - " & hidPhieuID.Value, Function_Name.BienBanThanhTra, Audit_Type.Edit, Request.ServerVariables("REMOTE_ADDR"), Session("UserName"))
                        ElseIf hidIsUser.Value = 2 Then
                            Insert_App_Log("Update  Cauhoi5: PhieuId - " & hidPhieuID.Value, Function_Name.PhieuKiemTra, Audit_Type.Edit, Request.ServerVariables("REMOTE_ADDR"), Session("UserName"))
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
            'Tiêu đề doanh nghiệp
            Dim DNId As Integer = IIf(IsNothing(Request.QueryString("DNId")), 0, CInt(Request.QueryString("DNId")))
            Dim dn = (From a In data.DoanhNghieps Where a.DoanhNghiepId = DNId).SingleOrDefault
            If Not IsNothing(dn) Then
                lblTitleCompany.Text = "TÌNH HÌNH THỰC HIỆN PHÁP LUẬT LAO ĐỘNG CỦA " & dn.TenDoanhNghiep.Trim()
            End If
            Dim p As CauHoi5 = (From q In data.CauHoi5 Where q.PhieuId = hidPhieuID.Value Select q).FirstOrDefault
            If Not p Is Nothing Then
                txtQ511.Text = IIf(IsNothing(p.Q511) = True, "", p.Q511)
                txtQ512.Text = IIf(IsNothing(p.Q512) = True, "", p.Q512)
                txtQ514.Text = IIf(IsNothing(p.Q514) = True, "", p.Q514)
                txtQ515.Text = IIf(IsNothing(p.Q515) = True, "", p.Q515)
                txtQ513.Text = IIf(IsNothing(p.Q513) = True, "", p.Q513)
                chkQ52.ClearSelection()
                If Not p.Q52 Is Nothing Then
                    chkQ52.SelectedValue = Math.Abs(CInt(p.Q52))
                End If

                txtQ521.Text = IIf(IsNothing(p.Q521) = True, "", p.Q521)
                txtQ522.Text = IIf(IsNothing(p.Q522) = True, "", p.Q522)
                chkQ53.ClearSelection()
                If Not p.Q53 Is Nothing Then
                    chkQ53.SelectedValue = Math.Abs(CInt(p.Q53))
                End If
                txtQ531.Text = IIf(IsNothing(p.Q531) = True, "", p.Q531)
                txtQ532.Text = IIf(IsNothing(p.Q532) = True, "", p.Q532)
                txtQ533.Text = IIf(IsNothing(p.Q533) = True, "", p.Q533)
                chkQ54.ClearSelection()
                If Not p.Q54 Is Nothing Then
                    chkQ54.SelectedValue = Math.Abs(CInt(p.Q54))
                End If
                txtQ541.Text = IIf(IsNothing(p.Q541) = True, "", p.Q541)
                chkQ55.ClearSelection()
                If Not p.Q55 Is Nothing Then
                    chkQ55.SelectedValue = Math.Abs(CInt(p.Q55))
                End If

            End If
        End Using
    End Sub
    Protected Sub ResetControl()
        txtQ511.Text = ""
        txtQ512.Text = ""
        txtQ514.Text = ""
        txtQ515.Text = ""
        txtQ513.Text = ""
        chkQ52.ClearSelection()
        txtQ521.Text = ""
        txtQ522.Text = ""
        chkQ53.ClearSelection()
        txtQ531.Text = ""
        txtQ532.Text = ""
        txtQ533.Text = ""
        chkQ54.ClearSelection()
        txtQ541.Text = ""
        chkQ55.ClearSelection()
    End Sub
#End Region
#Region "Event for control "
    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Session("phieuid") = hidPhieuID.Value
        Session("IsUser") = hidIsUser.Value
        Session("ModePhieu") = hidModePhieu.Value

        If Save() Then
            Dim iDN = Request.QueryString("DNId")
            Excute_Javascript("AlertboxRedirect('Mời bạn nhập tiếp mục 6.','CauHoi6.aspx?DNId=" & iDN & "');", Me.Page, True)
        Else
            Excute_Javascript("Alertbox('Cập nhật thất bại.');", Me.Page, True)
        End If
    End Sub

    Protected Sub btnReset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReset.Click
        Using data As New ThanhTraLaoDongEntities

            Dim q5 = (From p In data.CauHoi5 Where p.PhieuId = hidPhieuID.Value).FirstOrDefault
            If Not IsNothing(q5) Then
                ShowData()
            Else
                ResetControl()
            End If
        End Using
    End Sub
    'Protected Sub btnBack_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnBack.Click
    '    Response.Redirect("List.aspx")
    'End Sub
#End Region
End Class
