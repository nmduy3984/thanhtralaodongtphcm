Imports System.Data
Imports ThanhTraLaoDongModel
Imports Cls_Common
Imports SecurityService

Partial Class Control_CauHoi6_Create
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
                    Dim q6 = (From q In data.CauHoi6 Where q.PhieuId = hidPhieuID.Value Select q).FirstOrDefault
                    Dim iQ611, iQ612, iQ621, iQ622, iQ63, iQ631, iQ641, iQ642, iQ65, iQ66 As Integer
                    iQ611 = GetNumberByFormat(txtQ611.Text.Trim())
                    iQ612 = GetNumberByFormat(txtQ612.Text.Trim())
                    iQ621 = GetNumberByFormat(txtQ621.Text.Trim())
                    iQ622 = GetNumberByFormat(txtQ622.Text.Trim())
                    iQ63 = GetNumberByFormat(txtQ63.Text.Trim())
                    iQ631 = GetNumberByFormat(txtQ631.Text.Trim())
                    iQ641 = GetNumberByFormat(txtQ641.Text.Trim())
                    iQ642 = GetNumberByFormat(txtQ642.Text.Trim())
                    iQ65 = GetNumberByFormat(txtQ65.Text.Trim())
                    iQ66 = GetNumberByFormat(txtQ66.Text.Trim())

                    If q6 Is Nothing Then
                        q6 = New CauHoi6
                        q6.PhieuId = hidPhieuID.Value
                        ''6. Bảo hiểm xã hội, bảo hiểm thất nghiệp 
                        ''6.1. Tổng số người thuộc đối tượng tham gia BHXH bắt buộc
                        q6.Q611 = IIf(iQ611 > 0, iQ611, Nothing) '6.1. Tổng số người thuộc đối tượng tham gia BHXH bắt buộc
                        q6.Q612 = IIf(iQ611 > 0 And iQ612 >= 0, iQ612, Nothing) 'số người đã tham gia BHXH
                        ''6.2. Tổng số người thuộc đối tượng tham gia Bảo hiểm thất nghiệp
                        q6.Q621 = IIf(iQ621 > 0, iQ621, Nothing) 'Tổng số người thuộc đối tượng tham gia Bảo hiểm thất nghiệp
                        q6.Q622 = IIf(iQ621 > 0 And iQ622 >= 0, iQ622, Nothing) 'số người đã tham gia BHXH
                        ''6.3. Số người đã được cấp sổ BHXH
                        q6.Q63 = IIf(iQ63 > 0, iQ63, Nothing)
                        q6.Q631 = IIf(iQ631 > 0, iQ631, Nothing)
                        '6.4. Tổng số tiền chậm đóng/ số phải đóng hàng tháng
                        q6.Q641 = IIf(iQ641 > 0, iQ641, Nothing) 'Tổng số tiền chậm đóng
                        q6.Q642 = IIf(iQ642 > 0, iQ642, Nothing) 'số phải đóng hàng tháng
                        ''6.5. Số sổ BHXH chưa trả cho người thôi việc
                        q6.Q65 = IIf(iQ65 > 0, iQ65, Nothing) 'Số sổ BHXH chưa trả cho người thôi việc
                        q6.Q651 = IIf(iQ65 > 0, txtQ651.Text.Trim(), Nothing) 'Lí do chưa trả sổ
                        ''6.6. Số tiền khấu trừ lương của người lao động chưa nộp cho bảo hiểm xã hội
                        q6.Q66 = Nothing
                        If iQ641 > 0 Then
                            q6.Q66 = IIf(iQ66 > 0, iQ66, Nothing)
                        End If
                        '6.7 Trả tiềnbảo hiểm xã hội vào lương và ngày nghỉ phép
                        q6.Q67 = Nothing
                        If chkQ67.SelectedValue <> "" Then
                            q6.Q67 = chkQ67.SelectedValue
                            If chkQ67.SelectedValue = 0 Or chkQ67.SelectedValue = 1 Then
                                q6.Q671 = Nothing
                            Else
                                q6.Q671 = txtQ671.Text.Trim()
                            End If
                        End If
                        '6.8 Làm thủ tục thanh toán các chế độ bảo hiểm xã hội đầy đủ, kịp thời
                        q6.Q68 = Nothing
                        q6.Q681 = Nothing
                        If chkQ68.SelectedValue <> "" Then
                            q6.Q68 = chkQ68.SelectedValue = "0"
                            If Not q6.Q68 Then
                                q6.Q681 = txtQ681.Text.Trim()
                            End If
                        End If

                        q6.NguoiTao = Session("Username")
                        q6.NgayTao = Date.Now
                        data.CauHoi6.AddObject(q6)

                        'Luu cau hoi da tra loi
                        Dim pn = (From a In data.PhieuNhapHeaders Where a.PhieuID = hidPhieuID.Value Select a).FirstOrDefault
                        pn.CauHoiDaTraLoi = pn.CauHoiDaTraLoi & "6;"

                        data.SaveChanges()
                        If hidIsUser.Value = 1 Then
                            Insert_App_Log("Insert  Cauhoi6: PhieuId - " & hidPhieuID.Value, Function_Name.BienBanThanhTra, Audit_Type.Create, Request.ServerVariables("REMOTE_ADDR"), Session("UserName"))
                        ElseIf hidIsUser.Value = 2 Then
                            Insert_App_Log("Insert  Cauhoi6: PhieuId - " & hidPhieuID.Value, Function_Name.PhieuKiemTra, Audit_Type.Create, Request.ServerVariables("REMOTE_ADDR"), Session("UserName"))
                        End If

                    Else
                        ''6. Bảo hiểm xã hội, bảo hiểm thất nghiệp 
                        ''6.1. Tổng số người thuộc đối tượng tham gia BHXH bắt buộc
                        q6.Q611 = IIf(iQ611 > 0, iQ611, Nothing) '6.1. Tổng số người thuộc đối tượng tham gia BHXH bắt buộc
                        q6.Q612 = IIf(iQ611 > 0 And iQ612 >= 0, iQ612, Nothing) 'số người đã tham gia BHXH
                        ''6.2. Tổng số người thuộc đối tượng tham gia Bảo hiểm thất nghiệp
                        q6.Q621 = IIf(iQ621 > 0, iQ621, Nothing) 'Tổng số người thuộc đối tượng tham gia Bảo hiểm thất nghiệp
                        q6.Q622 = IIf(iQ621 > 0 And iQ622 >= 0, iQ622, Nothing) 'số người đã tham gia BHXH
                        ''6.3. Số người đã được cấp sổ BHXH
                        q6.Q63 = IIf(iQ63 > 0, iQ63, Nothing)
                        q6.Q631 = IIf(iQ631 > 0, iQ631, Nothing)
                        '6.4. Tổng số tiền chậm đóng/ số phải đóng hàng tháng
                        q6.Q641 = IIf(iQ641 > 0, iQ641, Nothing) 'Tổng số tiền chậm đóng
                        q6.Q642 = IIf(iQ642 > 0, iQ642, Nothing) 'số phải đóng hàng tháng
                        ''6.5. Số sổ BHXH chưa trả cho người thôi việc
                        q6.Q65 = IIf(iQ65 > 0, iQ65, Nothing) 'Số sổ BHXH chưa trả cho người thôi việc
                        q6.Q651 = IIf(iQ65 > 0, txtQ651.Text.Trim(), Nothing) 'Lí do chưa trả sổ
                        ''6.6. Số tiền khấu trừ lương của người lao động chưa nộp cho bảo hiểm xã 
                        q6.Q66 = Nothing
                        If iQ641 > 0 Then
                            q6.Q66 = IIf(iQ66 > 0, iQ66, Nothing)
                        End If
                        '6.7 Trả tiềnbảo hiểm xã hội vào lương và ngày nghỉ phép
                        q6.Q67 = Nothing
                        If chkQ67.SelectedValue <> "" Then
                            q6.Q67 = chkQ67.SelectedValue
                            If chkQ67.SelectedValue = 0 Or chkQ67.SelectedValue = 1 Then
                                q6.Q671 = Nothing
                            Else
                                q6.Q671 = txtQ671.Text.Trim()
                            End If
                        End If
                        '6.8 Làm thủ tục thanh toán các chế độ bảo hiểm xã hội đầy đủ, kịp thời
                        q6.Q68 = Nothing
                        q6.Q681 = Nothing
                        If chkQ68.SelectedValue <> "" Then
                            q6.Q68 = chkQ68.SelectedValue = "0"
                            If Not q6.Q68 Then
                                q6.Q681 = txtQ681.Text.Trim()
                            End If
                        End If
                        q6.NguoiSua = Session("Username")
                        q6.NgaySua = Date.Now
                        'Luu ngay sua, nguoi sua phieu
                        Dim pn = (From a In data.PhieuNhapHeaders Where a.PhieuID = hidPhieuID.Value Select a).FirstOrDefault
                        pn.NgaySua = Date.Now
                        pn.NguoiSua = Session("Username")
                        data.SaveChanges()
                        If hidIsUser.Value = 1 Then
                            Insert_App_Log("Update  Cauhoi6: PhieuId - " & hidPhieuID.Value, Function_Name.BienBanThanhTra, Audit_Type.Edit, Request.ServerVariables("REMOTE_ADDR"), Session("UserName"))
                        ElseIf hidIsUser.Value = 2 Then
                            Insert_App_Log("Update  Cauhoi6: PhieuId - " & hidPhieuID.Value, Function_Name.PhieuKiemTra, Audit_Type.Edit, Request.ServerVariables("REMOTE_ADDR"), Session("UserName"))
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
            Dim p As CauHoi6 = (From q In data.CauHoi6 Where q.PhieuId = hidPhieuID.Value Select q).FirstOrDefault
            Dim q2 As CauHoi2 = (From q In data.CauHoi2 Where q.PhieuId = hidPhieuID.Value Select q).FirstOrDefault
            Dim lhdnid = data.uspLoaiHinhDNId(hidPhieuID.Value).FirstOrDefault
            Dim iSum3HDLDFist As Integer = 0
            Dim iSum2HDLDFist As Integer = 0
            If Not q2 Is Nothing Then
                iSum3HDLDFist = IIf(IsNothing(q2.Q211), 0, q2.Q211) + IIf(IsNothing(q2.Q212), 0, q2.Q212) + IIf(IsNothing(q2.Q213), 0, q2.Q213) + IIf(Not IsNothing(lhdnid.LoaiHinhDNId) And lhdnid.LoaiHinhDNId = 1, 0, q2.Q2110)
                iSum2HDLDFist = IIf(IsNothing(q2.Q211), 0, q2.Q211) + IIf(IsNothing(q2.Q212), 0, q2.Q212) + IIf(Not IsNothing(lhdnid.LoaiHinhDNId) And lhdnid.LoaiHinhDNId = 1, 0, q2.Q2110)
                txtQ611.Text = iSum3HDLDFist
                txtQ621.Text = iSum2HDLDFist
                hidSum3HDLDFirst.Value = iSum3HDLDFist
                hidSum2HDLDFirst.Value = iSum2HDLDFist
            End If

            If Not p Is Nothing Then
                txtQ612.Text = IIf(iSum3HDLDFist = 0 Or IsNothing(p.Q612) Or (iSum3HDLDFist = 0 And Not IsNothing(p.Q612)), "", p.Q612)
                txtQ622.Text = IIf(iSum2HDLDFist = 0 Or IsNothing(p.Q622) Or (Not IsNothing(p.Q622) And iSum2HDLDFist = 0), "", p.Q622)
                txtQ63.Text = IIf(iSum3HDLDFist = 0 Or IsNothing(p.Q63) Or (iSum3HDLDFist = 0 And Not IsNothing(p.Q63)), "", p.Q63)
                txtQ631.Text = IIf(IsNothing(p.Q631) = True, "", p.Q631)
                txtQ641.Text = IIf(IsNothing(p.Q641) = True, "", p.Q641)
                txtQ642.Text = IIf(IsNothing(p.Q642) = True, "", p.Q642)
                txtQ65.Text = IIf(iSum3HDLDFist = 0 Or IsNothing(p.Q65) Or (iSum3HDLDFist = 0 And Not IsNothing(p.Q65)), "", p.Q65)
                txtQ651.Text = IIf(IsNothing(p.Q651) = True, "", p.Q651)
                txtQ66.Text = IIf(IsNothing(p.Q66) = True, "", String.Format("{0:n0}", p.Q66))
                chkQ67.ClearSelection()
                If Not p.Q67 Is Nothing Then
                    chkQ67.SelectedValue = p.Q67
                End If
                txtQ671.Text = IIf(IsNothing(p.Q671) = True, "", p.Q671)
                If Not IsNothing(p.Q68) Then
                    chkQ68.SelectedValue = Math.Abs(CInt(Not p.Q68))
                End If
                txtQ681.Text = IIf(IsNothing(p.Q681), "", p.Q681)
            End If
        End Using
    End Sub
    Protected Sub ResetControl()
        txtQ612.Text = ""
        txtQ622.Text = ""
        txtQ63.Text = ""
        txtQ631.Text = ""
        txtQ641.Text = ""
        txtQ642.Text = ""
        txtQ65.Text = ""
        txtQ651.Text = ""
        txtQ66.Text = ""
        chkQ67.ClearSelection()
        txtQ671.Text = ""
        chkQ68.ClearSelection()
        txtQ681.Text = ""
    End Sub
#End Region
#Region "Event for control "
    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Session("phieuid") = hidPhieuID.Value
        Session("IsUser") = hidIsUser.Value
        Session("ModePhieu") = hidModePhieu.Value

        If Save() Then
            Dim iDN = Request.QueryString("DNId")
            Excute_Javascript("AlertboxRedirect('Mời bạn nhập tiếp mục 7.','CauHoi7.aspx?DNId=" & iDN & "');", Me.Page, True)
        Else
            Excute_Javascript("Alertbox('Cập nhật thất bại.');", Me.Page, True)
        End If
    End Sub

    Protected Sub btnReset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReset.Click
        Using data As New ThanhTraLaoDongEntities

            Dim q6 = (From p In data.CauHoi6 Where p.PhieuId = hidPhieuID.Value).FirstOrDefault
            If Not IsNothing(q6) Then
                ShowData()
            Else
                ResetControl()
            End If
        End Using
    End Sub

#End Region
End Class
