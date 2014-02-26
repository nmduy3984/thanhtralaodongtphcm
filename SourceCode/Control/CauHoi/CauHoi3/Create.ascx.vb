Imports System.Data
Imports ThanhTraLaoDongModel
Imports Cls_Common
Imports SecurityService

Partial Class Control_CauHoi3_Create
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
                    'Kiem tra xem da co phieuid trong bang cauhoi3 chua?
                    'Neu chua thi tao moi
                    'Neu da co thi cap nhat lai
                    Dim q3 = (From q In data.CauHoi3 Where q.PhieuId = hidPhieuID.Value Select q).FirstOrDefault
                    Dim iQ31 As Integer
                    iQ31 = GetNumberByFormat(txtCodeQ31.Text.Trim())

                    'TH1: Insert cauhoi3
                    If IsCD.Value Then
                        If q3 Is Nothing Then
                            q3 = New CauHoi3
                            q3.PhieuId = hidPhieuID.Value
                            ''3. Thỏa ước lao động tập thể
                            q3.Q31 = IIf(iQ31 > 0, iQ31, Nothing) 'Năm ký kết
                            q3.Q32 = Nothing
                            If iQ31 > 0 And chkQ32.SelectedValue <> "" Then
                                q3.Q32 = chkQ32.SelectedValue = "1" 'Đã đăng ký?
                            End If
                            q3.Q33 = IIf(iQ31 > 0, txtQ33.Text.Trim(), Nothing) 'Nội dung không phù hợp pháp luật
                            '3.1. Quy trình thương lượng tập thể
                            q3.Q34 = Nothing
                            If iQ31 > 0 And chkQ34.SelectedValue <> "" Then
                                q3.Q34 = chkQ34.SelectedValue = "0"
                            End If
                            q3.Q341 = IIf(iQ31 > 0 And chkQ34.SelectedValue <> "" And chkQ34.SelectedValue = "1", txtQ341.Text.Trim(), Nothing) 'Nội dung không đúng quy định
                            'Thực hiện các nội dung thỏa ước lao động tập thể đã ký
                            q3.Q35 = Nothing
                            q3.Q351 = Nothing
                            If chkQ35.SelectedValue <> "" Then
                                q3.Q35 = chkQ35.SelectedValue = "0"
                                If Not q3.Q35 Then
                                    q3.Q351 = txtQ351.Text.Trim()
                                End If
                            End If

                            q3.NguoiTao = Session("Username")
                            q3.NgayTao = Date.Now
                            data.CauHoi3.AddObject(q3)
                            'Luu cau hoi da tra loi
                            Dim pn = (From a In data.PhieuNhapHeaders Where a.PhieuID = hidPhieuID.Value Select a).FirstOrDefault
                            pn.CauHoiDaTraLoi = pn.CauHoiDaTraLoi & "3;"
                            data.SaveChanges()
                            If hidIsUser.Value = 1 Then
                                Insert_App_Log("Insert  Cauhoi3: PhieuId - " & hidPhieuID.Value, Function_Name.BienBanThanhTra, Audit_Type.Create, Request.ServerVariables("REMOTE_ADDR"), Session("UserName"))
                            ElseIf hidIsUser.Value = 2 Then
                                Insert_App_Log("Insert  Cauhoi3: PhieuId - " & hidPhieuID.Value, Function_Name.PhieuKiemTra, Audit_Type.Create, Request.ServerVariables("REMOTE_ADDR"), Session("UserName"))
                            End If
                        Else 'TH2: Update cauhoi3                
                            ''3. Thỏa ước lao động tập thể
                            q3.Q31 = IIf(iQ31 > 0, iQ31, Nothing) 'Năm ký kết
                            q3.Q32 = Nothing
                            If iQ31 > 0 And chkQ32.SelectedValue <> "" Then
                                q3.Q32 = chkQ32.SelectedValue = "1" 'Đã đăng ký?
                            End If
                            q3.Q33 = IIf(iQ31 > 0 AndAlso Not String.IsNullOrEmpty(txtQ33.Text.Trim()), txtQ33.Text.Trim(), Nothing) 'Nội dung không phù hợp pháp luật
                            '3.1. Quy trình thương lượng tập thể
                            q3.Q34 = Nothing
                            If iQ31 > 0 And chkQ34.SelectedValue <> "" Then
                                q3.Q34 = chkQ34.SelectedValue = "0"
                            End If
                            q3.Q341 = IIf(iQ31 > 0 And chkQ34.SelectedValue <> "" And chkQ34.SelectedValue = "1", txtQ341.Text.Trim(), Nothing) 'Nội dung không đúng quy định
                            'Thực hiện các nội dung thỏa ước lao động tập thể đã ký
                            q3.Q35 = Nothing
                            q3.Q351 = Nothing
                            If chkQ35.SelectedValue <> "" Then
                                q3.Q35 = chkQ35.SelectedValue = "0"
                                If Not q3.Q35 Then
                                    q3.Q351 = txtQ351.Text.Trim()
                                End If
                            End If
                            q3.NguoiSua = Session("Username")
                            q3.NgaySua = Date.Now
                            'Luu ngay sua, nguoi sua phieu
                            Dim pn = (From a In data.PhieuNhapHeaders Where a.PhieuID = hidPhieuID.Value Select a).FirstOrDefault
                            pn.NgaySua = Date.Now
                            pn.NguoiSua = Session("Username")
                            data.SaveChanges()
                            If hidIsUser.Value = 1 Then
                                Insert_App_Log("Update  Cauhoi3: PhieuId - " & hidPhieuID.Value, Function_Name.BienBanThanhTra, Audit_Type.Edit, Request.ServerVariables("REMOTE_ADDR"), Session("UserName"))
                            Else
                                Insert_App_Log("Update  Cauhoi3: PhieuId - " & hidPhieuID.Value, Function_Name.PhieuKiemTra, Audit_Type.Edit, Request.ServerVariables("REMOTE_ADDR"), Session("UserName"))
                            End If
                        End If
                        Return True
                    Else
                        'Luu cau hoi da tra loi
                        Dim pn = (From a In data.PhieuNhapHeaders Where a.PhieuID = hidPhieuID.Value Select a).FirstOrDefault
                        If Not pn Is Nothing Then
                            If pn.CauHoiDaTraLoi.ToString().Contains("3") = False Then
                                pn.CauHoiDaTraLoi = "3;"
                                data.SaveChanges()
                            End If
                        End If
                        Return False
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
            Dim dnTitle = (From a In data.DoanhNghieps Where a.DoanhNghiepId = DNId).SingleOrDefault
            If Not IsNothing(dnTitle) Then
                lblTitleCompany.Text = "TÌNH HÌNH THỰC HIỆN PHÁP LUẬT LAO ĐỘNG CỦA " & dnTitle.TenDoanhNghiep.Trim()
            End If

            Dim q3 As CauHoi3 = (From a In data.CauHoi3 Where a.PhieuId = hidPhieuID.Value Select a).FirstOrDefault
            Dim dn = (From b In data.PhieuNhapHeaders Where b.PhieuID = hidPhieuID.Value Select New With {b.IsCongDoan}).FirstOrDefault
            If Not dn Is Nothing AndAlso dn.IsCongDoan Then
                'Gan isCongDoan cho hiddenfield
                IsCD.Value = dn.IsCongDoan
                If Not q3 Is Nothing Then
                    txtCodeQ31.Text = IIf(IsNothing(q3.Q31), "", q3.Q31)
                    chkQ32.ClearSelection()
                    If Not q3.Q32 Is Nothing Then
                        chkQ32.SelectedValue = Math.Abs(CInt(q3.Q32))
                    End If
                    txtQ33.Text = q3.Q33
                    chkQ34.ClearSelection()
                    If Not q3.Q34 Is Nothing Then
                        chkQ34.SelectedValue = Math.Abs(CInt(Not q3.Q34))
                    End If
                    txtQ341.Text = q3.Q341
                    If Not IsNothing(q3.Q35) Then
                        chkQ35.SelectedValue = Math.Abs(CInt(Not q3.Q35))
                    End If
                    txtQ351.Text = IIf(IsNothing(q3.Q351), "", q3.Q351)
                End If
            End If
        End Using
    End Sub
    Protected Sub ResetControl()
        txtCodeQ31.Text = ""
        chkQ32.ClearSelection()
        txtQ33.Text = ""
        chkQ34.ClearSelection()
        txtQ341.Text = ""
        chkQ35.ClearSelection()
        txtQ351.Text = ""
    End Sub
#End Region
#Region "Event for control "
    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Session("phieuid") = hidPhieuID.Value
        Session("IsUser") = hidIsUser.Value
        Session("ModePhieu") = hidModePhieu.Value

        If Save() Then
            Dim iDN = Request.QueryString("DNId")
            Excute_Javascript("AlertboxRedirect('Mời bạn nhập tiếp mục 4.','CauHoi4.aspx?DNId=" & iDN & "');", Me.Page, True)
        Else
            Excute_Javascript("Alertbox('Cập nhật thất bại.');", Me.Page, True)
        End If
    End Sub

    Protected Sub btnReset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReset.Click
        Using data As New ThanhTraLaoDongEntities
            Dim q3 = (From p In data.CauHoi3 Where p.PhieuId = hidPhieuID.Value).FirstOrDefault
            If Not IsNothing(q3) Then
                ShowData()
            Else
                ResetControl()
            End If
        End Using
    End Sub
#End Region
End Class
