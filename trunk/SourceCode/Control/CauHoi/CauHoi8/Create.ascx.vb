Imports System.Data
Imports ThanhTraLaoDongModel
Imports Cls_Common
Imports SecurityService

Partial Class Control_CauHoi8_Create
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
                ' Gán PhieuID vào hidden field
                hidPhieuID.Value = IIf(IsNothing(Session("phieuid")), 0, Session("phieuid"))
                hidIsUser.Value = IIf(IsNothing(Session("IsUser")), 0, Session("IsUser"))
                hidModePhieu.Value = IIf(IsNothing(Session("ModePhieu")), 0, Session("ModePhieu"))
                'Neu ModePhieu la xem chi tiet
                If hidModePhieu.Value = ModePhieu.Detail Then
                    btnSave.Visible = False
                    btnReset.Visible = False
                End If
                'Load nội dung câu hỏi 8

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

            ' Check Exists CauHoi8 by PhieID
            Dim cauhoi8 As CauHoi8 = (From q In data.CauHoi8 Where q.PhieuId = hidPhieuID.Value).FirstOrDefault()
            Try
                If Not Session("Username") = "" Then
                    Dim iCodeQ8111, iQ82, iQ821, iQ822, iQ823, iQ824, iQ84 As Integer
                    iCodeQ8111 = GetNumberByFormat(txtCodeQ8111.Text.Trim())
                    iQ82 = GetNumberByFormat(txtQ82.Text.Trim())
                    iQ821 = GetNumberByFormat(txtQ821.Text.Trim())
                    iQ822 = GetNumberByFormat(txtQ822.Text.Trim())
                    iQ823 = GetNumberByFormat(txtQ823.Text.Trim())
                    iQ824 = GetNumberByFormat(txtQ824.Text.Trim())
                    iQ84 = GetNumberByFormat(txtQ84.Text.Trim())
                    If cauhoi8 Is Nothing Then
                        cauhoi8 = New CauHoi8
                        cauhoi8.PhieuId = hidPhieuID.Value
                        ''8. Kỷ luật lao động, trách nhiệm vật chất
                        ''8.1. Xây dựng và đăng ký Nội quy lao động
                        ''''''''Nếu Q81 không check bỏ qua mục 8.1
                        ''''''''Nếu Q81 check có tiếp tục lưu các mục nhỏ trong mục 8.1, ngược lại không lưu
                        cauhoi8.Q81 = Nothing
                        If chkQ81.SelectedValue <> "" Then
                            If chkQ81.SelectedValue = "1" Then
                                cauhoi8.Q81 = True
                                cauhoi8.Q8111 = IIf(iCodeQ8111 > 0, iCodeQ8111, Nothing) 'Năm ban hành
                                cauhoi8.Q8112 = Nothing
                                If chkQ8112.SelectedValue <> "" Then
                                    cauhoi8.Q8112 = chkQ8112.SelectedValue = 1 'Đã đăng ký?
                                End If
                                cauhoi8.Q812 = txtQ812.Text.Trim() 'Nội dung không phù hợp pháp luật
                            Else
                                cauhoi8.Q81 = False
                                cauhoi8.Q8111 = Nothing 'Năm ban hành
                                cauhoi8.Q8112 = Nothing 'Đã đăng ký?
                                cauhoi8.Q812 = Nothing 'Nội dung không phù hợp pháp luật
                            End If
                        End If

                        ''8.2. Tổng số vụ kỷ luật lao động 
                        cauhoi8.Q82 = IIf(iQ82 > 0, iQ82, Nothing) 'Tổng số vụ kỷ luật lao động 
                        cauhoi8.Q821 = IIf(iQ821 > 0, iQ821, Nothing) 'Khiển trách
                        cauhoi8.Q822 = IIf(iQ822 > 0, iQ822, Nothing) 'Kéo dài thời hạn nâng lương
                        cauhoi8.Q823 = IIf(iQ823 > 0, iQ823, Nothing) 'Sa thải
                        cauhoi8.Q824 = IIf(iQ824 > 0, iQ824, Nothing) 'Khác
                        cauhoi8.Q8241 = IIf(iQ824 > 0, txtQ8241.Text, Nothing) 'Hình thức xử lý
                        ''8.3. Báo cáo với Sở LĐTBXH sau khi sa thải người lao động
                        cauhoi8.Q83 = Nothing
                        If iQ82 > 0 And chkQ83.SelectedValue <> "" Then
                            cauhoi8.Q83 = chkQ83.SelectedValue = 1
                        End If
                        ''8.4. Số vụ bồi thường trách nhiệm vật chất
                        cauhoi8.Q84 = IIf(iQ84 > 0, iQ84, Nothing)
                        ''8.5. Xử lí kỉ luật lao động
                        cauhoi8.Q851 = Nothing
                        If chkQ851.SelectedValue <> "" Then
                            cauhoi8.Q851 = chkQ851.SelectedValue = 1 'Đúng quy trình
                        End If
                        If chkQ851.SelectedValue <> "" Then
                            cauhoi8.Q8511 = IIf(chkQ851.SelectedValue = 0, chkQ8511.Checked, Nothing) 'Không đúng thẩm quyền
                            cauhoi8.Q8512 = IIf(chkQ851.SelectedValue = 0, chkQ8512.Checked, Nothing) 'Quá thời hiệu
                            'cauhoi8.Q8513 = IIf(chkQ851.SelectedValue = 0, chkQ8513.Checked, Nothing) 'áp dụng sai hình thức kỉ luật theo Bộ Luật lao động 
                            cauhoi8.Q8514 = IIf(chkQ851.SelectedValue = 0, chkQ8514.Checked, Nothing) 'Không chứng minh được lỗi của người lao động
                            cauhoi8.Q8515 = IIf(chkQ851.SelectedValue = 0, chkQ8515.Checked, Nothing) 'Họp xét kỉ luật không ghi biên bản họp
                            cauhoi8.Q8516 = IIf(chkQ851.SelectedValue = 0, txtQ8516.Text.Trim(), Nothing) 'sai khác
                        Else
                            cauhoi8.Q8511 = Nothing 'Không đúng thẩm quyền
                            cauhoi8.Q8512 = Nothing 'Quá thời hiệu
                            'cauhoi8.Q8513 = Nothing 'áp dụng sai hình thức kỉ luật theo Bộ Luật lao động 
                            cauhoi8.Q8514 = Nothing 'Không chứng minh được lỗi của người lao động
                            cauhoi8.Q8515 = Nothing 'Họp xét kỉ luật không ghi biên bản họp
                            cauhoi8.Q8516 = Nothing 'sai khác
                        End If
                        cauhoi8.NguoiTao = Session("Username")
                        cauhoi8.NgayTao = Date.Now

                        data.CauHoi8.AddObject(cauhoi8)
                        data.SaveChanges()
                        'Luu cau hoi da tra loi
                        Dim pn = (From a In data.PhieuNhapHeaders Where a.PhieuID = hidPhieuID.Value Select a).FirstOrDefault
                        pn.CauHoiDaTraLoi = pn.CauHoiDaTraLoi & "8;"
                        data.SaveChanges()
                        If hidIsUser.Value = 1 Then
                            Insert_App_Log("Update  Cauhoi8: PhieuId - " & hidPhieuID.Value, Function_Name.BienBanThanhTra, Audit_Type.Create, Request.ServerVariables("REMOTE_ADDR"), Session("UserName"))
                        ElseIf hidIsUser.Value = 2 Then
                            Insert_App_Log("Update  Cauhoi8: PhieuId - " & hidPhieuID.Value, Function_Name.PhieuKiemTra, Audit_Type.Create, Request.ServerVariables("REMOTE_ADDR"), Session("UserName"))
                        End If
                        Return True

                    Else
                        ''8. Kỷ luật lao động, trách nhiệm vật chất
                        ''8.1. Xây dựng và đăng ký Nội quy lao động
                        cauhoi8.Q81 = Nothing
                        If chkQ81.SelectedValue <> "" Then
                            If chkQ81.SelectedValue = "1" Then
                                cauhoi8.Q81 = True
                                cauhoi8.Q8111 = IIf(iCodeQ8111 > 0, iCodeQ8111, Nothing) 'Năm ban hành
                                cauhoi8.Q8112 = Nothing
                                If chkQ8112.SelectedValue <> "" Then
                                    cauhoi8.Q8112 = chkQ8112.SelectedValue = 1 'Đã đăng ký?
                                End If
                                cauhoi8.Q812 = txtQ812.Text.Trim() 'Nội dung không phù hợp pháp luật
                            Else
                                cauhoi8.Q81 = False
                                cauhoi8.Q8111 = Nothing 'Năm ban hành
                                cauhoi8.Q8112 = Nothing 'Đã đăng ký?
                                cauhoi8.Q812 = Nothing 'Nội dung không phù hợp pháp luật
                            End If
                        End If
                        ''8.2. Tổng số vụ kỷ luật lao động 
                        cauhoi8.Q82 = IIf(iQ82 > 0, iQ82, Nothing) 'Tổng số vụ kỷ luật lao động 
                        cauhoi8.Q821 = IIf(iQ821 > 0, iQ821, Nothing) 'Khiển trách
                        cauhoi8.Q822 = IIf(iQ822 > 0, iQ822, Nothing) 'Kéo dài thời hạn nâng lương
                        cauhoi8.Q823 = IIf(iQ823 > 0, iQ823, Nothing) 'Sa thải
                        cauhoi8.Q824 = IIf(iQ824 > 0, iQ824, Nothing) 'Khác
                        cauhoi8.Q8241 = IIf(iQ824 > 0, txtQ8241.Text, Nothing) 'Hình thức xử lý

                        ''8.3. Báo cáo với Sở LĐTBXH sau khi sa thải người lao động
                        cauhoi8.Q83 = Nothing
                        If iQ82 > 0 And chkQ83.SelectedValue <> "" Then
                            cauhoi8.Q83 = chkQ83.SelectedValue = 1
                        End If
                        ''8.4. Số vụ bồi thường trách nhiệm vật chất
                        cauhoi8.Q84 = IIf(iQ84 > 0, iQ84, Nothing)
                        ''8.5. Xử lí kỉ luật lao động
                        cauhoi8.Q851 = Nothing
                        If chkQ851.SelectedValue <> "" Then
                            cauhoi8.Q851 = chkQ851.SelectedValue = 1 'Đúng quy trình
                        End If
                        If chkQ851.SelectedValue <> "" Then
                            cauhoi8.Q8511 = IIf(chkQ851.SelectedValue = 0, chkQ8511.Checked, Nothing) 'Không đúng thẩm quyền
                            cauhoi8.Q8512 = IIf(chkQ851.SelectedValue = 0, chkQ8512.Checked, Nothing) 'Quá thời hiệu
                            'cauhoi8.Q8513 = IIf(chkQ851.SelectedValue = 0, chkQ8513.Checked, Nothing) 'áp dụng sai hình thức kỉ luật theo Bộ Luật lao động 
                            cauhoi8.Q8514 = IIf(chkQ851.SelectedValue = 0, chkQ8514.Checked, Nothing) 'Không chứng minh được lỗi của người lao động
                            cauhoi8.Q8515 = IIf(chkQ851.SelectedValue = 0, chkQ8515.Checked, Nothing) 'Họp xét kỉ luật không ghi biên bản họp
                            cauhoi8.Q8516 = IIf(chkQ851.SelectedValue = 0, txtQ8516.Text.Trim(), Nothing) 'sai khác
                        Else
                            cauhoi8.Q8511 = Nothing 'Không đúng thẩm quyền
                            cauhoi8.Q8512 = Nothing 'Quá thời hiệu
                            'cauhoi8.Q8513 = Nothing 'áp dụng sai hình thức kỉ luật theo Bộ Luật lao động 
                            cauhoi8.Q8514 = Nothing 'Không chứng minh được lỗi của người lao động
                            cauhoi8.Q8515 = Nothing 'Họp xét kỉ luật không ghi biên bản họp
                            cauhoi8.Q8516 = Nothing 'sai khác
                        End If

                        cauhoi8.NguoiSua = Session("Username")
                        cauhoi8.NgaySua = Date.Now
                        'Luu ngay sua, nguoi sua phieu
                        Dim pn = (From a In data.PhieuNhapHeaders Where a.PhieuID = hidPhieuID.Value Select a).FirstOrDefault
                        pn.NgaySua = Date.Now
                        pn.NguoiSua = Session("Username")
                        data.SaveChanges()
                        If hidIsUser.Value = 1 Then
                            Insert_App_Log("Update  Cauhoi8: PhieuId - " & hidPhieuID.Value, Function_Name.BienBanThanhTra, Audit_Type.Edit, Request.ServerVariables("REMOTE_ADDR"), Session("UserName"))
                        ElseIf hidIsUser.Value = 2 Then
                            Insert_App_Log("Update  Cauhoi8: PhieuId - " & hidPhieuID.Value, Function_Name.PhieuKiemTra, Audit_Type.Edit, Request.ServerVariables("REMOTE_ADDR"), Session("UserName"))
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
            Dim iPhieuId As Integer = CInt(Session("phieuid"))
            Dim p As CauHoi8 = (From q In data.CauHoi8 Where q.PhieuId = iPhieuId Select q).SingleOrDefault
            If Not p Is Nothing Then
                If p.Q81.HasValue Then
                    chkQ81.SelectedValue = Math.Abs(CInt(p.Q81))
                Else
                    chkQ81.ClearSelection()
                End If
                txtCodeQ8111.Text = IIf(IsNothing(p.Q8111) = True, "", p.Q8111)
                If p.Q8112.HasValue Then
                    chkQ8112.SelectedValue = Math.Abs(CInt(p.Q8112))
                Else
                    chkQ8112.ClearSelection()
                End If
                txtQ812.Text = IIf(IsNothing(p.Q812) = True, "", p.Q812)
                txtQ82.Text = IIf(IsNothing(p.Q82) = True, "", p.Q82)
                txtQ821.Text = IIf(IsNothing(p.Q821) = True, "", p.Q821)
                txtQ822.Text = IIf(IsNothing(p.Q822) = True, "", p.Q822)
                txtQ823.Text = IIf(IsNothing(p.Q823) = True, "", p.Q823)
                txtQ824.Text = IIf(IsNothing(p.Q824) = True, "", p.Q824)
                txtQ8241.Text = IIf(IsNothing(p.Q8241) = True, "", p.Q8241)
                If p.Q83.HasValue Then
                    chkQ83.SelectedValue = Math.Abs(CInt(p.Q83))
                Else
                    chkQ83.ClearSelection()
                End If
                txtQ84.Text = IIf(IsNothing(p.Q84) = True, "", p.Q84)
                If p.Q851.HasValue Then
                    chkQ851.SelectedValue = Math.Abs(CInt(p.Q851))
                Else
                    chkQ851.ClearSelection()
                End If
                chkQ8511.Checked = IIf(IsNothing(p.Q8511) = True, False, p.Q8511)
                chkQ8512.Checked = IIf(IsNothing(p.Q8512) = True, False, p.Q8512)
                'chkQ8513.Checked = IIf(IsNothing(p.Q8513) = True, False, p.Q8513)
                chkQ8514.Checked = IIf(IsNothing(p.Q8514) = True, False, p.Q8514)
                chkQ8515.Checked = IIf(IsNothing(p.Q8515) = True, False, p.Q8515)
                txtQ8516.Text = IIf(IsNothing(p.Q8516) = True, "", p.Q8516)
            End If
        End Using
    End Sub
    Protected Sub ResetControl()
        txtCodeQ8111.Text = ""
        chkQ8112.ClearSelection()
        txtQ812.Text = ""
        txtQ82.Text = ""
        txtQ821.Text = ""
        txtQ822.Text = ""
        txtQ823.Text = ""
        txtQ824.Text = ""
        txtQ8241.Text = ""
        chkQ83.ClearSelection()
        txtQ84.Text = ""
        chkQ851.ClearSelection()
        chkQ8511.Checked = False
        chkQ8512.Checked = False
        'chkQ8513.Checked = False
        chkQ8514.Checked = False
        chkQ8515.Checked = False
        txtQ8516.Text = ""
    End Sub
#End Region
#Region "Event for control "
    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Session("phieuid") = hidPhieuID.Value
        Session("IsUser") = hidIsUser.Value
        Session("ModePhieu") = hidModePhieu.Value

        If Save() Then
            Dim iDN = Request.QueryString("DNId")
            Excute_Javascript("AlertboxRedirect('Mời bạn nhập tiếp mục 9.','CauHoi9.aspx?DNId=" & iDN & "');", Me.Page, True)
        Else
            Excute_Javascript("Alertbox('Cập nhật thất bại.');", Me.Page, True)
        End If
    End Sub

    Protected Sub btnReset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReset.Click
        Using data As New ThanhTraLaoDongEntities
            Dim iPhieuId As Integer = CInt(Session("phieuid"))
            Dim q8 = (From p In data.CauHoi8 Where p.PhieuId = iPhieuId).FirstOrDefault
            If Not IsNothing(q8) Then
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
