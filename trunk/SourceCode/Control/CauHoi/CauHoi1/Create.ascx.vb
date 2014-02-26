Imports System.Data
Imports ThanhTraLaoDongModel
Imports Cls_Common
Imports SecurityService

Partial Class Control_CauHoi1_Create
    Inherits System.Web.UI.UserControl
    Private Shared ReadOnly log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

#Region "Sub and Function"
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
                'Load cauhoi1
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
                    'Kiem tra xem da co phieuid trong bang cauhoi1  chua?
                    'Neu chua thi tao moi
                    'Neu da co thi cap nhat lai
                    Dim q1 = (From a In data.CauHoi1 Where a.PhieuId = hidPhieuID.Value Select a).FirstOrDefault
                    If q1 Is Nothing Then
                        'Tao moi cauhoi1
                        q1 = New CauHoi1
                        q1.PhieuId = hidPhieuID.Value
                        '1.1. Khai trình, báo cáo định kỳ về tuyển dụng, sử dụng lao động với cơ quan quản lí nhà nước về lao động địa phương
                        q1.Q11 = Nothing
                        If chkQ11.SelectedValue <> "" Then
                            q1.Q11 = chkQ11.SelectedValue = "1"
                        End If

                        '1.2. Báo cáo định kỳ về công tác an toàn vệ sinh lao động với cơ quan quản lí nhà nước về lao động địa phương
                        q1.Q12 = Nothing
                        If chkQ12.SelectedValue <> "" Then
                            q1.Q12 = chkQ12.SelectedValue = "1"
                        End If

                        '1.3. Báo cáo định kỳ về tai nạn lao động với Sở Lao động - Thương binh và Xã hội
                        q1.Q13 = Nothing
                        If chkQ13.SelectedValue <> "" Then
                            q1.Q13 = chkQ13.SelectedValue = "1"
                        End If

                        q1.NguoiTao = Session("Username")
                        q1.NgayTao = Date.Now
                        data.CauHoi1.AddObject(q1)

                        'Luu cau hoi da tra loi
                        Dim pn = (From a In data.PhieuNhapHeaders Where a.PhieuID = hidPhieuID.Value Select a).FirstOrDefault
                        pn.CauHoiDaTraLoi = pn.CauHoiDaTraLoi & "1;"

                        data.SaveChanges()

                        If hidIsUser.Value = 1 Then
                            Insert_App_Log("Insert  Cauhoi1: PhieuId - " & hidPhieuID.Value, Function_Name.BienBanThanhTra, Audit_Type.Create, Request.ServerVariables("REMOTE_ADDR"), Session("UserName"))
                        ElseIf hidIsUser.Value = 2 Then
                            Insert_App_Log("Insert  Cauhoi1: PhieuId - " & hidPhieuID.Value, Function_Name.PhieuKiemTra, Audit_Type.Create, Request.ServerVariables("REMOTE_ADDR"), Session("UserName"))
                        End If
                    Else 'Cap nhat cauhoi1

                        '1.1. Khai trình, báo cáo định kỳ về tuyển dụng, sử dụng lao động với cơ quan quản lí nhà nước về lao động địa phương
                        q1.Q11 = Nothing
                        If chkQ11.SelectedValue <> "" Then
                            q1.Q11 = chkQ11.SelectedValue = "1"
                        End If

                        '1.2. Báo cáo định kỳ về công tác an toàn vệ sinh lao động với cơ quan quản lí nhà nước về lao động địa phương
                        q1.Q12 = Nothing
                        If chkQ12.SelectedValue <> "" Then
                            q1.Q12 = chkQ12.SelectedValue = "1"
                        End If

                        '1.3. Báo cáo định kỳ về tai nạn lao động với Sở Lao động - Thương binh và Xã hội
                        q1.Q13 = Nothing
                        If chkQ13.SelectedValue <> "" Then
                            q1.Q13 = chkQ13.SelectedValue = "1"
                        End If

                        q1.NguoiSua = Session("Username")
                        q1.NgaySua = Date.Now
                        'Luu ngay sua, nguoi sua phieu
                        Dim pn = (From a In data.PhieuNhapHeaders Where a.PhieuID = hidPhieuID.Value Select a).FirstOrDefault
                        pn.NgaySua = Date.Now
                        pn.NguoiSua = Session("Username")

                        data.SaveChanges()
                        If hidIsUser.Value = 1 Then
                            Insert_App_Log("Update  Cauhoi1: PhieuId - " & hidPhieuID.Value, Function_Name.BienBanThanhTra, Audit_Type.Edit, Request.ServerVariables("REMOTE_ADDR"), Session("UserName"))
                        ElseIf hidIsUser.Value = 2 Then
                            Insert_App_Log("Update  Cauhoi1: PhieuId - " & hidPhieuID.Value, Function_Name.PhieuKiemTra, Audit_Type.Edit, Request.ServerVariables("REMOTE_ADDR"), Session("UserName"))
                        End If

                    End If
                    Return (True)
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

            Dim p As CauHoi1 = (From q In data.CauHoi1 Where q.PhieuId = hidPhieuID.Value Select q).FirstOrDefault
            If Not p Is Nothing Then
                '1.1. Khai trình, báo cáo định kỳ về tuyển dụng, sử dụng lao động với cơ quan quản lí nhà nước về lao động địa phương
                If p.Q11 Is Nothing Then
                    chkQ11.ClearSelection()
                Else
                    chkQ11.SelectedValue = Math.Abs(CInt(p.Q11))
                End If

                '1.2. Báo cáo định kỳ về công tác an toàn vệ sinh lao động với cơ quan quản lí nhà nước về lao động địa phương
                If p.Q12 Is Nothing Then
                    chkQ12.ClearSelection()
                Else
                    chkQ12.SelectedValue = Math.Abs(CInt(p.Q12))
                End If

                '1.3. Báo cáo định kỳ về tai nạn lao động với Sở Lao động - Thương binh và Xã hội
                If p.Q13 Is Nothing Then
                    chkQ13.ClearSelection()
                Else
                    chkQ13.SelectedValue = Math.Abs(CInt(p.Q13))
                End If

            End If
        End Using
    End Sub

    Protected Sub ResetControl()
        chkQ11.ClearSelection()
        chkQ12.ClearSelection()
        chkQ13.ClearSelection()
    End Sub
#End Region
#Region "Event for control "
    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Session("phieuid") = hidPhieuID.Value
        Session("IsUser") = hidIsUser.Value
        Session("ModePhieu") = hidModePhieu.Value

        If Save() Then
            Dim iDN = Request.QueryString("DNId")
            Excute_Javascript("AlertboxRedirect('Mời bạn nhập tiếp mục 2.','CauHoi2.aspx?DNId=" & iDN & "');", Me.Page, True)
        Else
            Excute_Javascript("Alertbox('Cập nhật thất bại.');", Me.Page, True)
        End If
    End Sub

    Protected Sub btnReset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReset.Click
        Using data As New ThanhTraLaoDongEntities
            Dim q1 = (From p In data.CauHoi1 Where p.PhieuId = hidPhieuID.Value).FirstOrDefault
            If Not IsNothing(q1) Then
                ShowData()
            Else
                ResetControl()
            End If
        End Using
    End Sub

#End Region
End Class
