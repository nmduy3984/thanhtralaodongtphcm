Imports System.Data
Imports ThanhTraLaoDongModel
Imports Cls_Common
Imports SecurityService

Partial Class Control_CauHoi9_Create
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
                LoadData()

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
    Private Sub LoadData()
        Using data As New ThanhTraLaoDongEntities
            Dim isCD As Boolean = (From q In data.PhieuNhapHeaders.Include("DoanhNghiep")
                              Select q.DoanhNghiep.IsCongDoan).FirstOrDefault()
            hidIsCongDoan.Value = isCD
        End Using
    End Sub
    Protected Function Save() As Boolean
        Using data As New ThanhTraLaoDongEntities
            ' Check Exists CauHoi9 by PhieID
            Dim cauhoi9 As CauHoi9 = (From q In data.CauHoi9 Where q.PhieuId = hidPhieuID.Value).FirstOrDefault()
            Try
                If Not Session("Username") = "" Then
                    Dim iQ9111, iQ9112, iQ92, iQ921, iQ922, iQ925, iQ93, iQ931 As Integer
                    iQ9111 = GetNumberByFormat(txtQ9111.Text.Trim())
                    iQ9112 = GetNumberByFormat(txtQ9112.Text.Trim())
                    iQ92 = GetNumberByFormat(txtQ92.Text.Trim())
                    iQ921 = GetNumberByFormat(txtQ921.Text.Trim())
                    iQ922 = GetNumberByFormat(txtQ922.Text.Trim())
                    iQ925 = GetNumberByFormat(txtQ925.Text.Trim())
                    iQ93 = GetNumberByFormat(txtQ93.Text.Trim())
                    iQ931 = GetNumberByFormat(txtQ931.Text.Trim())

                    If cauhoi9 Is Nothing Then
                        cauhoi9 = New CauHoi9
                        cauhoi9.PhieuId = hidPhieuID.Value
                        ''9. Tranh chấp lao động
                        ''9.1. Tranh chấp cá nhân
                        cauhoi9.Q9111 = IIf(iQ9111 > 0, iQ9111, Nothing) 'Tranh chấp cá nhân
                        cauhoi9.Q9112 = IIf(iQ9111 > 0 And iQ9112 > 0, iQ9112, Nothing) 'đã hòa giải thành
                        ''9.2. Tranh chấp tập thể
                        cauhoi9.Q92 = IIf(iQ92 > 0, iQ92, Nothing) 'Tranh chấp tập thể
                        cauhoi9.Q921 = IIf(iQ92 > 0 And iQ921 > 0, iQ921, Nothing) 'trong đó tranh chấp về lợi ích
                        cauhoi9.Q922 = IIf(iQ92 > 0 And iQ922 > 0, iQ922, Nothing) 'đình công tự phát
                        cauhoi9.Q923 = IIf(iQ92 > 0, txtQ923.Text.Trim(), Nothing) 'Người lao động đòi
                        cauhoi9.Q924 = IIf(iQ92 > 0, txtQ924.Text.Trim(), Nothing) 'Kết quả giải quyết
                        cauhoi9.Q925 = IIf(iQ925 > 0, iQ925, Nothing) 'Số người tham gia đình công sau số vụ đình công tự phát
                        ''9.3. Số vụ khiếu nại về lao động
                        cauhoi9.Q93 = IIf(iQ93 > 0, iQ93, Nothing) 'Số vụ khiếu nại về lao động
                        cauhoi9.Q931 = IIf(iQ93 > 0 And iQ931 > 0, iQ931, Nothing) 'Đã  giải quyết xong
                        cauhoi9.Q932 = IIf(iQ93 > 0, txtQ932.Text.Trim(), Nothing) 'Nguyên nhân chính
                        cauhoi9.Q933 = IIf(iQ93 > 0, txtQ933.Text.Trim(), Nothing) 'kết quả giải quyết

                        cauhoi9.NguoiTao = Session("Username")
                        cauhoi9.NgayTao = Date.Now
                        data.CauHoi9.AddObject(cauhoi9)

                        'Luu cau hoi da tra loi
                        Dim pn = (From a In data.PhieuNhapHeaders Where a.PhieuID = hidPhieuID.Value Select a).FirstOrDefault
                        pn.CauHoiDaTraLoi = pn.CauHoiDaTraLoi & "9;"

                        data.SaveChanges()
                        If hidIsUser.Value = 1 Then
                            Insert_App_Log("Update  Cauhoi9: PhieuId - " & hidPhieuID.Value, Function_Name.BienBanThanhTra, Audit_Type.Create, Request.ServerVariables("REMOTE_ADDR"), Session("UserName"))
                        ElseIf hidIsUser.Value = 2 Then
                            Insert_App_Log("Update  Cauhoi9: PhieuId - " & hidPhieuID.Value, Function_Name.PhieuKiemTra, Audit_Type.Create, Request.ServerVariables("REMOTE_ADDR"), Session("UserName"))
                        End If
                        Return True
                    Else
                        ''9. Tranh chấp lao động
                        ''9.1. Tranh chấp cá nhân
                        cauhoi9.Q9111 = IIf(iQ9111 > 0, iQ9111, Nothing) 'Tranh chấp cá nhân
                        cauhoi9.Q9112 = IIf(iQ9111 > 0 And iQ9112 > 0, iQ9112, Nothing) 'đã hòa giải thành
                        ''9.2. Tranh chấp tập thể
                        cauhoi9.Q92 = IIf(iQ92 > 0, iQ92, Nothing) 'Tranh chấp tập thể
                        cauhoi9.Q921 = IIf(iQ92 > 0 And iQ921 > 0, iQ921, Nothing) 'trong đó tranh chấp về lợi ích
                        cauhoi9.Q922 = IIf(iQ92 > 0 And iQ922 > 0, iQ922, Nothing) 'đình công tự phát
                        cauhoi9.Q923 = IIf(iQ92 > 0, txtQ923.Text.Trim(), Nothing) 'Người lao động đòi
                        cauhoi9.Q924 = IIf(iQ92 > 0, txtQ924.Text.Trim(), Nothing) 'Kết quả giải quyết
                        cauhoi9.Q925 = IIf(iQ925 > 0, iQ925, Nothing) 'Số người tham gia đình công sau số vụ đình công tự phát
                        ''9.3. Số vụ khiếu nại về lao động
                        cauhoi9.Q93 = IIf(iQ93 > 0, iQ93, Nothing) 'Số vụ khiếu nại về lao động
                        cauhoi9.Q931 = IIf(iQ93 > 0 And iQ931 > 0, iQ931, Nothing) 'Đã  giải quyết xong
                        cauhoi9.Q932 = IIf(iQ93 > 0, txtQ932.Text.Trim(), Nothing) 'Nguyên nhân chính
                        cauhoi9.Q933 = IIf(iQ93 > 0, txtQ933.Text.Trim(), Nothing) 'kết quả giải quyết

                        cauhoi9.NguoiSua = Session("Username")
                        cauhoi9.NgaySua = Date.Now
                        'Luu ngay sua, nguoi sua phieu
                        Dim pn = (From a In data.PhieuNhapHeaders Where a.PhieuID = hidPhieuID.Value Select a).FirstOrDefault
                        pn.NgaySua = Date.Now
                        pn.NguoiSua = Session("Username")

                        data.SaveChanges()
                        If hidIsUser.Value = 1 Then
                            Insert_App_Log("Update  Cauhoi9: PhieuId - " & hidPhieuID.Value, Function_Name.BienBanThanhTra, Audit_Type.Edit, Request.ServerVariables("REMOTE_ADDR"), Session("UserName"))
                        ElseIf hidIsUser.Value = 2 Then
                            Insert_App_Log("Update  Cauhoi9: PhieuId - " & hidPhieuID.Value, Function_Name.PhieuKiemTra, Audit_Type.Edit, Request.ServerVariables("REMOTE_ADDR"), Session("UserName"))
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
            Dim p As CauHoi9 = (From q In data.CauHoi9 Where q.PhieuId = hidPhieuID.Value Select q).FirstOrDefault
            If Not p Is Nothing Then
                txtQ9111.Text = IIf(IsNothing(p.Q9111) = True, "", p.Q9111)
                txtQ9112.Text = IIf(IsNothing(p.Q9112) = True, "", p.Q9112)
                txtQ92.Text = IIf(IsNothing(p.Q92) = True, "", p.Q92)
                txtQ921.Text = IIf(IsNothing(p.Q921) = True, "", p.Q921)
                txtQ922.Text = IIf(IsNothing(p.Q922) = True, "", p.Q922)
                txtQ923.Text = IIf(IsNothing(p.Q923) = True, "", p.Q923)
                txtQ924.Text = IIf(IsNothing(p.Q924) = True, "", p.Q924)
                txtQ925.Text = IIf(IsNothing(p.Q925) = True, "", p.Q925) 'Số người tham gia đình công sau số vụ đình công tự phát
                txtQ93.Text = IIf(IsNothing(p.Q93) = True, "", p.Q93)
                txtQ931.Text = IIf(IsNothing(p.Q931) = True, "", p.Q931)
                txtQ932.Text = IIf(IsNothing(p.Q932) = True, "", p.Q932)
                txtQ933.Text = IIf(IsNothing(p.Q933) = True, "", p.Q933)

            End If
        End Using
    End Sub
    Protected Sub ResetControl()
        txtQ9111.Text = ""
        txtQ9112.Text = ""
        txtQ92.Text = ""
        txtQ921.Text = ""
        txtQ922.Text = ""
        txtQ923.Text = ""
        txtQ924.Text = ""
        txtQ925.Text = ""
        txtQ93.Text = ""
        txtQ931.Text = ""
        txtQ932.Text = ""
        txtQ933.Text = ""

    End Sub
#End Region
#Region "Event for control "
    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Session("phieuid") = hidPhieuID.Value
        Session("IsUser") = hidIsUser.Value
        Session("ModePhieu") = hidModePhieu.Value

        If Save() Then
            Dim iDN = Request.QueryString("DNId")
            Excute_Javascript("AlertboxRedirect('Mời bạn nhập tiếp mục 10.','CauHoi10.aspx?DNId=" & iDN & "');", Me.Page, True)
        Else
            Excute_Javascript("Alertbox('Cập nhật thất bại.');", Me.Page, True)
        End If
    End Sub

    Protected Sub btnReset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReset.Click
        Using data As New ThanhTraLaoDongEntities
            Dim q9 = (From p In data.CauHoi9 Where p.PhieuId = hidPhieuID.Value).FirstOrDefault
            If Not IsNothing(q9) Then
                ShowData()
            Else
                ResetControl()
            End If
        End Using
    End Sub

#End Region
End Class
