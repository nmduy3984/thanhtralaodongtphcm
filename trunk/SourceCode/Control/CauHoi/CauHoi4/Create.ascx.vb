Imports System.Data
Imports ThanhTraLaoDongModel
Imports Cls_Common
Imports SecurityService

Partial Class Control_CauHoi4_Create
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

                hidPhieuID.Value = IIf(IsNothing(Session("phieuid")), 0, Session("phieuid"))
                hidIsUser.Value = IIf(IsNothing(Session("IsUser")), 0, Session("IsUser"))
                hidModePhieu.Value = IIf(IsNothing(Session("ModePhieu")), 0, Session("ModePhieu"))
                'Neu ModePhieu la xem chi tiet
                If hidModePhieu.Value = ModePhieu.Detail Then
                    btnSave.Visible = False
                    btnReset.Visible = False
                End If
                'Load nội dung cauhoi4

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
                    'Kiem tra xem da co phieuid trong bang cauhoi4 chua?
                    'Neu chua thi tao moi
                    'Neu da co thi cap nhat lai
                    Dim q4 = (From a In data.CauHoi4 Where a.PhieuId = hidPhieuID.Value Select a).FirstOrDefault
                    Dim iQ41, iQ4112, iQ41131, iQ4114, iQ4116, iQ461, iQ462, iQ463, iQ464, iQ47, iQ481, iQ482, iQ4101, iQ41011, iQ41021, iQ41022, iQ410311, iQ410312 As Integer
                    iQ41 = GetNumberByFormat(txtQ4111.Text.Trim())
                    iQ4112 = GetNumberByFormat(txtQ4112.Text.Trim())
                    iQ41131 = GetNumberByFormat(txtQ41131.Text.Trim())
                    iQ4114 = GetNumberByFormat(txtQ4114.Text.Trim())
                    iQ4116 = GetNumberByFormat(txtQ4116.Text.Trim())
                    iQ461 = GetNumberByFormat(txtQ461.Text.Trim())
                    iQ462 = GetNumberByFormat(txtQ462.Text.Trim())
                    iQ463 = GetNumberByFormat(txtQ463.Text.Trim())
                    iQ464 = GetNumberByFormat(txtQ464.Text.Trim())
                    iQ47 = GetNumberByFormat(txtQ47.Text.Trim())
                    iQ481 = GetNumberByFormat(txtQ481.Text.Trim())
                    iQ482 = GetNumberByFormat(txtQ482.Text.Trim())
                    iQ4101 = GetNumberByFormat(txtQ4101.Text.Trim())
                    iQ41011 = GetNumberByFormat(txtQ41011.Text.Trim())
                    iQ41021 = GetNumberByFormat(txtQ41021.Text.Trim())
                    iQ41022 = GetNumberByFormat(txtQ41022.Text.Trim())
                    iQ410311 = GetNumberByFormat(txtQ410311.Text.Trim())
                    iQ410312 = GetNumberByFormat(txtQ410312.Text.Trim())

                    'TH1: Insert cauhoi4
                    If q4 Is Nothing Then
                        q4 = New CauHoi4
                        q4.PhieuId = hidPhieuID.Value
                        ''4.1 Tiền lương
                        q4.Q4111 = IIf(iQ41 > 0, iQ41, Nothing) '4.1. Mức lương tối thiểu đang áp dụng
                        q4.Q4114 = IIf(iQ4114 > 0, iQ4114, Nothing) ' Mức lương tối thiểu với lao động đã qua đào tạo
                        q4.Q4116 = IIf(iQ4116 > 0, iQ4116, Nothing) ' Mức tiền lương trung bình/ngày
                        q4.Q4112 = IIf(iQ4112 > 0, iQ4112, Nothing) 'Thu nhập trung bình/người/tháng
                        q4.Q4113 = Nothing
                        q4.Q41131 = Nothing
                        If chkQ4113.SelectedValue <> "" Then 'Trả lương đúng thời hạn
                            q4.Q4113 = chkQ4113.SelectedValue = "1"
                            If q4.Q4113 Then
                                q4.Q41131 = IIf(iQ41131 > 0, iQ41131, Nothing) 'Nợ lương của người lao động
                            End If
                        End If

                        ''4.2. Hình thức trả lương
                        If chkQ421.Checked Or chkQ422.Checked Or chkQ423.Checked Or rdlQ44.SelectedValue <> "" Then
                            q4.Q421 = chkQ421.Checked 'Lương thời gian 
                            q4.Q422 = chkQ422.Checked 'Lương sản phẩm
                            q4.Q423 = chkQ423.Checked 'Lương khoán
                            q4.Q424 = rdlQ424.SelectedValue 'Thỏa thuận trách nhiệm người lao động phải trả phí mở, duy trì tài khoản nhận lương qua ATM
                        ElseIf Not (chkQ421.Checked Or chkQ422.Checked Or chkQ423.Checked) Then
                            q4.Q421 = Nothing
                            q4.Q422 = Nothing
                            q4.Q423 = Nothing
                            q4.Q424 = Nothing
                        End If

                        '4.3. Xây dựng định mức lao động
                        q4.Q43 = Nothing
                        If chkQ43.SelectedValue <> "" Then
                            q4.Q43 = chkQ43.SelectedValue = "1"
                        End If

                        'Xu ly loại hình doanh nghiệp là DNNN?
                        'If hidIsDNNN.Value = 2 Then
                        '    q4.Q44 = 0 '4.4. Áp dụng thang lương lương
                        '    q4.Q45 = Nothing '4.5. Đăng ký thang bảng lương với cơ quan quản lí nhà nước về lao động địa phương
                        'Else
                        q4.Q44 = rdlQ44.SelectedValue '4.4. Áp dụng thang lương lương                            
                        If Not rdlQ44.SelectedValue = "3" Then
                            q4.Q45 = Nothing
                            If chkQ45.SelectedValue <> "" Then
                                q4.Q45 = chkQ45.SelectedValue = "1" '4.5. Đăng ký thang bảng lương với cơ quan quản lí nhà nước về lao động địa phương
                            End If
                        Else
                            q4.Q45 = Nothing
                        End If
                        'End If
                        ''4.6. Mức trả lương làm thêm giờ theo
                        If chkQ46a.Checked Or chkQ46b.Checked Then
                            q4.Q46a = chkQ46a.Checked 'Lương thời gian 
                            q4.Q46b = chkQ46b.Checked 'Lương sản phẩm
                        ElseIf Not (chkQ46a.Checked Or chkQ46b.Checked) Then
                            q4.Q46a = Nothing
                            q4.Q46b = Nothing
                        End If
                        q4.Q461 = Nothing
                        q4.Q462 = Nothing
                        q4.Q463 = Nothing
                        q4.Q464 = Nothing
                        If chkQ46a.Checked Or chkQ46b.Checked Then
                            q4.Q461 = IIf(iQ461 > 0, iQ461, Nothing) 'Làm thêm giờ vào ngày thường
                            q4.Q462 = IIf(iQ462 > 0, iQ462, Nothing) 'Làm thêm giờ vào ngày nghỉ hàng tuần
                            q4.Q463 = IIf(iQ463 > 0, iQ463, Nothing) 'Làm thêm giờ vào ngày lễ, tết
                            q4.Q464 = IIf(iQ464 > 0, iQ464, Nothing) 'Làm thêm giờ vào ban đêm                          
                        End If
                        ''4.7. Mức trả lương làm ca đêm
                        q4.Q47 = IIf(iQ47 > 0, iQ47, Nothing)
                        ''4.8. Phạt tiền, phạt trừ lương
                        q4.Q48 = Nothing
                        If chkQ48.SelectedValue <> "" Then
                            q4.Q48 = Not chkQ48.SelectedValue = "1" 'có?
                        End If
                        q4.Q481 = IIf(iQ481 > 0 And chkQ48.SelectedValue <> "" And chkQ48.SelectedValue = "1", iQ481, Nothing) 'đã phạt
                        q4.Q482 = IIf(iQ482 > 0 And chkQ48.SelectedValue <> "" And chkQ48.SelectedValue = "1", iQ482, Nothing) 'mức phạt trung bình                

                        '4.9. Công khai thang lương, bảng lương và  quy chế thưởng
                        q4.Q49 = Nothing
                        If chkQ49.SelectedValue <> "" Then
                            q4.Q49 = chkQ49.SelectedValue = "1"
                        End If
                        ''4.10. Trả lương chế độ theo
                        q4.Q410 = Nothing
                        If ddlQ410.SelectedValue <> "" Then
                            q4.Q410 = ddlQ410.SelectedValue
                        End If
                        If Not IsNothing(q4.Q410) Then
                            q4.Q4101 = IIf(q4.Q410 > 0 And iQ4101 > 0, iQ4101, Nothing) '4.10.1 Trả lương ngừng việc
                            q4.Q41011 = IIf(q4.Q410 > 0 And iQ41011 > 0, iQ41011, Nothing) 'Mức lương thử việc
                            ''4.10.2 Trợ cấp mất việc
                            q4.Q4102 = Nothing
                            If q4.Q410 > 0 And chkQ4102.SelectedValue <> "" And hidTroCapMatViec.Value > 0 Then
                                q4.Q4102 = chkQ4102.SelectedValue = "1" 'Có?
                            End If
                            q4.Q41021 = IIf(q4.Q410 > 0 And iQ41021 > 0 And hidTroCapMatViec.Value > 0, iQ41021, Nothing) 'Số lao động mất việc, đủ điều kiện hưởng
                            q4.Q41022 = IIf(q4.Q410 > 0 And iQ41022 > 0 And hidTroCapMatViec.Value > 0 And chkQ4102.SelectedValue <> "" And chkQ4102.SelectedValue = "1", iQ41022, Nothing) 'đã trả
                            '4.10.3 Trợ cấp thôi việc
                            q4.Q4103 = Nothing
                            If q4.Q410 > 0 And chkQ4103.SelectedValue <> "" Then
                                q4.Q4103 = chkQ4103.SelectedValue = "1" 'Có?
                            End If
                            q4.Q410311 = IIf(q4.Q410 > 0 And iQ410311 > 0, iQ410311, Nothing) 'Số lao động mất thôi việc đủ điều kiện hưởng
                            q4.Q410312 = IIf(q4.Q410 > 0 And iQ410312 > 0 And chkQ4103.SelectedValue <> "" And chkQ4103.SelectedValue = "1", iQ410312, Nothing) 'đã trả
                            q4.Q41032 = Nothing
                            If q4.Q410 > 0 And chkQ41032.SelectedValue <> "" And chkQ4103.SelectedValue <> "" And chkQ4103.SelectedValue = "1" Then
                                q4.Q41032 = chkQ41032.SelectedValue = "1" '' Có làm tròn số tháng lẻ.
                            End If
                            q4.Q41033 = IIf(q4.Q410 > 0 And chkQ4103.SelectedValue <> "" And chkQ4103.SelectedValue = "1", txtQ41033.Text.Trim(), Nothing) 'Lí do chưa trả
                            '4.10.4 Phụ cấp độc hại hoặc tính tiền độc hại vào lương:
                            q4.Q4104 = Nothing
                            If q4.Q410 > 0 And chkQ4104.SelectedValue <> "" Then
                                q4.Q4104 = chkQ4104.SelectedValue
                                If chkQ4104.SelectedValue = 0 Or chkQ4104.SelectedValue = 1 Then
                                    q4.Q41041 = Nothing
                                Else
                                    q4.Q41041 = txtQ41041.Text.Trim()
                                End If
                            End If
                             
                            '4.10.5 Trả lương ngày lễ, ngày nghỉ hưởng nguyên lương
                            q4.Q4105 = Nothing
                            If q4.Q410 > 0 And chkQ4105.SelectedValue <> "" Then
                                q4.Q4105 = chkQ4105.SelectedValue
                            End If
                        Else
                            '4.10.1 Trả lương ngừng việc
                            q4.Q4101 = Nothing
                            ''4.10.2 Trợ cấp mất việc
                            q4.Q4102 = Nothing
                            q4.Q41021 = Nothing
                            q4.Q41022 = Nothing
                            '4.10.3 Trợ cấp thôi việc
                            q4.Q4103 = Nothing
                            q4.Q410311 = Nothing
                            q4.Q410312 = Nothing
                            q4.Q41032 = Nothing
                            q4.Q41033 = Nothing
                            '4.10.4 Phụ cấp độc hại hoặc tính tiền độc hại vào lương:
                            q4.Q4104 = Nothing
                            '4.10.5 Trả lương ngày lễ, ngày nghỉ hưởng nguyên lương
                            q4.Q4105 = Nothing
                        End If

                        q4.NguoiTao = Session("Username")
                        q4.NgayTao = Date.Now
                        data.CauHoi4.AddObject(q4)


                        'Luu cau hoi da tra loi
                        Dim pn = (From a In data.PhieuNhapHeaders Where a.PhieuID = hidPhieuID.Value Select a).FirstOrDefault
                        pn.CauHoiDaTraLoi = pn.CauHoiDaTraLoi & "4;"
                        data.SaveChanges()
                        If hidIsUser.Value = 1 Then
                            Insert_App_Log("Insert  Cauhoi4: PhieuId - " & hidPhieuID.Value, Function_Name.BienBanThanhTra, Audit_Type.Create, Request.ServerVariables("REMOTE_ADDR"), Session("UserName"))
                        ElseIf hidIsUser.Value = 2 Then
                            Insert_App_Log("Insert  Cauhoi4: PhieuId - " & hidPhieuID.Value, Function_Name.PhieuKiemTra, Audit_Type.Create, Request.ServerVariables("REMOTE_ADDR"), Session("UserName"))
                        End If

                    Else 'TH2: Update cauhoi4       
                        ''4. Tiền lương
                        q4.Q4111 = IIf(iQ41 > 0, iQ41, Nothing) '4.1. Mức lương tối thiểu đang áp dụng
                        q4.Q4114 = IIf(iQ4114 > 0, iQ4114, Nothing) ' Mức lương tối thiểu với lao động đã qua đào tạo
                        q4.Q4116 = IIf(iQ4116 > 0, iQ4116, Nothing) ' Mức tiền lương trung bình/ngày
                        q4.Q4112 = IIf(iQ4112 > 0, iQ4112, Nothing) 'Mức lương trung bình/người/tháng
                        q4.Q4113 = Nothing
                        q4.Q41131 = Nothing
                        If chkQ4113.SelectedValue <> "" Then 'Trả lương đúng thời hạn
                            q4.Q4113 = chkQ4113.SelectedValue = "1"
                            If q4.Q4113 Then
                                q4.Q41131 = IIf(iQ41131 > 0, iQ41131, Nothing) 'Nợ lương của người lao động
                            End If
                        End If
                        ''4.2. Hình thức trả lương
                        If chkQ421.Checked Or chkQ422.Checked Or chkQ423.Checked Or rdlQ44.SelectedValue <> "" Then
                            q4.Q421 = chkQ421.Checked 'Lương thời gian 
                            q4.Q422 = chkQ422.Checked 'Lương sản phẩm
                            q4.Q423 = chkQ423.Checked 'Lương khoán
                            q4.Q424 = rdlQ424.SelectedValue 'Thỏa thuận trách nhiệm người lao động phải trả phí mở, duy trì tài khoản nhận lương qua ATM
                        ElseIf Not (chkQ421.Checked Or chkQ422.Checked Or chkQ423.Checked) Then
                            q4.Q421 = Nothing
                            q4.Q422 = Nothing
                            q4.Q423 = Nothing
                            q4.Q424 = Nothing
                        End If

                        '4.3. Xây dựng định mức lao động
                        q4.Q43 = Nothing
                        If chkQ43.SelectedValue <> "" Then
                            q4.Q43 = chkQ43.SelectedValue = "1"
                        End If

                        'Xu ly loại hình doanh nghiệp là DNNN?
                        'If hidIsDNNN.Value = 2 Then
                        '    q4.Q44 = 0 '4.4. Áp dụng thang lương lương
                        '    q4.Q45 = Nothing '4.5. Đăng ký thang bảng lương với cơ quan quản lí nhà nước về lao động địa phương
                        'Else
                        q4.Q44 = rdlQ44.SelectedValue '4.4. Áp dụng thang lương lương
                        If Not rdlQ44.SelectedValue = "3" Then
                            q4.Q45 = Nothing
                            If chkQ45.SelectedValue <> "" Then
                                q4.Q45 = chkQ45.SelectedValue = "1" '4.5. Đăng ký thang bảng lương với cơ quan quản lí nhà nước về lao động địa phương
                            End If
                        Else
                            q4.Q45 = Nothing
                        End If
                        'End If
                        ''4.6. Mức trả lương làm thêm giờ theo
                        If chkQ46a.Checked Or chkQ46b.Checked Then
                            q4.Q46a = chkQ46a.Checked 'Lương thời gian 
                            q4.Q46b = chkQ46b.Checked 'Lương sản phẩm
                        ElseIf Not (chkQ46a.Checked Or chkQ46b.Checked) Then
                            q4.Q46a = Nothing
                            q4.Q46b = Nothing
                        End If
                        q4.Q461 = Nothing
                        q4.Q462 = Nothing
                        q4.Q463 = Nothing
                        q4.Q464 = Nothing
                        If chkQ46a.Checked Or chkQ46b.Checked Then
                            q4.Q461 = IIf(iQ461 > 0, iQ461, Nothing) 'Làm thêm giờ vào ngày thường
                            q4.Q462 = IIf(iQ462 > 0, iQ462, Nothing) 'Làm thêm giờ vào ngày nghỉ hàng tuần
                            q4.Q463 = IIf(iQ463 > 0, iQ463, Nothing) 'Làm thêm giờ vào ngày lễ, tết
                            q4.Q464 = IIf(iQ464 > 0, iQ464, Nothing) 'Làm thêm giờ vào ban đêm                          
                        End If

                        ''4.7. Mức trả lương làm ca đêm
                        q4.Q47 = IIf(iQ47 > 0, iQ47, Nothing)
                        ''4.8. Phạt tiền, phạt trừ lương
                        q4.Q48 = Nothing
                        If chkQ48.SelectedValue <> "" Then
                            q4.Q48 = Not chkQ48.SelectedValue = "1" 'có?
                        End If
                        q4.Q481 = IIf(iQ481 > 0 And chkQ48.SelectedValue <> "" And chkQ48.SelectedValue = "1", iQ481, Nothing) 'đã phạt
                        q4.Q482 = IIf(iQ482 > 0 And chkQ48.SelectedValue <> "" And chkQ48.SelectedValue = "1", iQ482, Nothing) 'mức phạt trung bình                     

                        '4.9. Công khai thang lương, bảng lương và  quy chế thưởng
                        q4.Q49 = Nothing
                        If chkQ49.SelectedValue <> "" Then
                            q4.Q49 = chkQ49.SelectedValue = "1"
                        End If

                        ''4.10. Trả lương chế độ theo
                        q4.Q410 = Nothing
                        If ddlQ410.SelectedValue <> "" Then
                            q4.Q410 = ddlQ410.SelectedValue
                        End If
                        If Not IsNothing(q4.Q410) Then
                            q4.Q4101 = IIf(q4.Q410 > 0 And iQ4101 > 0, iQ4101, Nothing) '4.10.1 Trả lương ngừng việc
                            q4.Q41011 = IIf(q4.Q410 > 0 And iQ41011 > 0, iQ41011, Nothing) 'Mức lương thử việc
                            ''4.10.2 Trợ cấp mất việc
                            q4.Q4102 = Nothing
                            If q4.Q410 > 0 And chkQ4102.SelectedValue <> "" And hidTroCapMatViec.Value > 0 Then
                                q4.Q4102 = chkQ4102.SelectedValue = "1" 'Có?
                            End If
                            q4.Q41021 = IIf(q4.Q410 > 0 And iQ41021 > 0 And hidTroCapMatViec.Value > 0, iQ41021, Nothing) 'Số lao động mất việc, đủ điều kiện hưởng
                            q4.Q41022 = IIf(q4.Q410 > 0 And iQ41022 > 0 And hidTroCapMatViec.Value > 0 And chkQ4102.SelectedValue <> "" And chkQ4102.SelectedValue = "1", iQ41022, Nothing) 'đã trả
                            '4.10.3 Trợ cấp thôi việc
                            q4.Q4103 = Nothing
                            If q4.Q410 > 0 And chkQ4103.SelectedValue <> "" Then
                                q4.Q4103 = chkQ4103.SelectedValue = "1" 'Có?
                            End If
                            q4.Q410311 = IIf(q4.Q410 > 0 And iQ410311 > 0, iQ410311, Nothing) 'Số lao động mất thôi việc đủ điều kiện hưởng
                            q4.Q410312 = IIf(q4.Q410 > 0 And iQ410312 > 0 And chkQ4103.SelectedValue <> "" And chkQ4103.SelectedValue = "1", iQ410312, Nothing) 'đã trả
                            q4.Q41032 = Nothing
                            If q4.Q410 > 0 And chkQ41032.SelectedValue <> "" And chkQ4103.SelectedValue <> "" And chkQ4103.SelectedValue = "1" Then
                                q4.Q41032 = chkQ41032.SelectedValue = "1" '' Có làm tròn số tháng lẻ.
                            End If
                            q4.Q41033 = IIf(q4.Q410 > 0 And chkQ4103.SelectedValue <> "" And chkQ4103.SelectedValue = "1", txtQ41033.Text.Trim(), Nothing) 'Lí do chưa trả
                            '4.10.4 Phụ cấp độc hại hoặc tính tiền độc hại vào lương:
                            q4.Q4104 = Nothing
                            If q4.Q410 > 0 And chkQ4104.SelectedValue <> "" Then
                                q4.Q4104 = chkQ4104.SelectedValue
                                If chkQ4104.SelectedValue = 0 Or chkQ4104.SelectedValue = 1 Then
                                    q4.Q41041 = Nothing
                                Else
                                    q4.Q41041 = txtQ41041.Text.Trim()
                                End If
                            End If
                            '4.10.5 Trả lương ngày lễ, ngày nghỉ hưởng nguyên lương
                            q4.Q4105 = Nothing
                            If q4.Q410 > 0 And chkQ4105.SelectedValue <> "" Then
                                q4.Q4105 = chkQ4105.SelectedValue
                            End If
                        Else
                            '4.10.1 Trả lương ngừng việc
                            q4.Q4101 = Nothing
                            ''4.10.2 Trợ cấp mất việc
                            q4.Q4102 = Nothing
                            q4.Q41021 = Nothing
                            q4.Q41022 = Nothing
                            '4.10.3 Trợ cấp thôi việc
                            q4.Q4103 = Nothing
                            q4.Q410311 = Nothing
                            q4.Q410312 = Nothing
                            q4.Q41032 = Nothing
                            q4.Q41033 = Nothing
                            '4.10.4 Phụ cấp độc hại hoặc tính tiền độc hại vào lương:
                            q4.Q4104 = Nothing
                            '4.10.5 Trả lương ngày lễ, ngày nghỉ hưởng nguyên lương
                            q4.Q4105 = Nothing
                        End If
                        q4.NguoiSua = Session("Username")
                        q4.NgaySua = Date.Now
                        'Luu ngay sua, nguoi sua phieu
                        Dim pn = (From a In data.PhieuNhapHeaders Where a.PhieuID = hidPhieuID.Value Select a).FirstOrDefault
                        pn.NgaySua = Date.Now
                        pn.NguoiSua = Session("Username")
                        data.SaveChanges()
                        If hidIsUser.Value = 1 Then
                            Insert_App_Log("Update  Cauhoi4: PhieuId - " & hidPhieuID.Value, Function_Name.BienBanThanhTra, Audit_Type.Edit, Request.ServerVariables("REMOTE_ADDR"), Session("UserName"))
                        ElseIf hidIsUser.Value = 2 Then
                            Insert_App_Log("Update  Cauhoi4: PhieuId - " & hidPhieuID.Value, Function_Name.PhieuKiemTra, Audit_Type.Edit, Request.ServerVariables("REMOTE_ADDR"), Session("UserName"))
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
            Dim dnTitle = (From a In data.DoanhNghieps Where a.DoanhNghiepId = DNId).SingleOrDefault
            If Not IsNothing(dnTitle) Then
                lblTitleCompany.Text = "TÌNH HÌNH THỰC HIỆN PHÁP LUẬT LAO ĐỘNG CỦA " & dnTitle.TenDoanhNghiep.Trim()
            End If
            Dim p As CauHoi4 = (From q In data.CauHoi4 Where q.PhieuId = hidPhieuID.Value Select q).FirstOrDefault
            'Gán gia tri loại hình doanh nghiệp
            'Dim dn = (From b In data.DoanhNghieps Join c In data.PhieuNhapHeaders On c.DoanhNghiepId Equals b.DoanhNghiepId Where c.PhieuID = hidPhieuID.Value Select New With {b.LoaiHinhDNId}).FirstOrDefault
            'If Not dn Is Nothing Then
            '    hidIsDNNN.Value = dn.LoaiHinhDNId
            'End If
            'Gán gia tri tro cap mat viec
            Dim q2 = (From b In data.CauHoi2 Where b.PhieuId = hidPhieuID.Value Select b).FirstOrDefault
            If Not q2 Is Nothing Then
                If Not IsNothing(q2.Q241) Then
                    hidTroCapMatViec.Value = q2.Q241
                End If

            End If

            If Not p Is Nothing Then
                txtQ4111.Text = IIf(IsNothing(p.Q4111) = True, "", String.Format("{0:n0}", p.Q4111))
                txtQ4112.Text = IIf(IsNothing(p.Q4112) = True, "", String.Format("{0:n0}", p.Q4112))
                txtQ4114.Text = IIf(IsNothing(p.Q4114) = True, "", String.Format("{0:n0}", p.Q4114))
                txtQ4116.Text = IIf(IsNothing(p.Q4116) = True, "", String.Format("{0:n0}", p.Q4116))
                chkQ4113.ClearSelection()
                If Not IsNothing(p.Q4113) Then
                    chkQ4113.SelectedValue = Math.Abs(CInt(p.Q4113))
                End If
                txtQ41131.Text = IIf(IsNothing(p.Q41131) = True, "", String.Format("{0:n0}", p.Q41131))

                If p.Q421 Is Nothing And p.Q421 Is Nothing And p.Q421 Is Nothing Then
                    chkQ421.Checked = False
                    chkQ422.Checked = False
                    chkQ423.Checked = False
                Else
                    chkQ421.Checked = IIf(IsNothing(p.Q421), False, p.Q421)
                    chkQ422.Checked = IIf(IsNothing(p.Q422), False, p.Q422)
                    chkQ423.Checked = IIf(IsNothing(p.Q423), False, p.Q423)
                End If
                If p.Q424.HasValue Then
                    rdlQ424.SelectedValue = p.Q424
                Else
                    rdlQ424.ClearSelection()
                End If

                chkQ43.ClearSelection()
                If Not p.Q43 Is Nothing Then
                    chkQ43.SelectedValue = Math.Abs(CInt(p.Q43))
                End If
                If p.Q44.HasValue Then
                    rdlQ44.SelectedValue = p.Q44
                Else
                    rdlQ44.ClearSelection()
                End If


                chkQ45.ClearSelection()
                If Not p.Q45 Is Nothing Then
                    chkQ45.SelectedValue = Math.Abs(CInt(p.Q45))
                End If

                If p.Q46a Is Nothing And p.Q46b Is Nothing Then
                    chkQ46a.Checked = False
                    chkQ46b.Checked = False
                Else
                    chkQ46a.Checked = IIf(IsNothing(p.Q46a), False, p.Q46a)
                    chkQ46b.Checked = IIf(IsNothing(p.Q46b), False, p.Q46b)
                End If

                txtQ461.Text = IIf(IsNothing(p.Q461), "", p.Q461)
                txtQ462.Text = IIf(IsNothing(p.Q462), "", p.Q462)
                txtQ463.Text = IIf(IsNothing(p.Q463), "", p.Q463)
                txtQ464.Text = IIf(IsNothing(p.Q464), "", p.Q464)
                txtQ47.Text = IIf(IsNothing(p.Q47), "", p.Q47)
                chkQ48.ClearSelection()
                If Not p.Q48 Is Nothing Then
                    chkQ48.SelectedValue = Math.Abs(CInt(Not p.Q48))
                End If
                txtQ481.Text = IIf(IsNothing(p.Q481) = True, "", String.Format("{0:n0}", p.Q481))
                txtQ482.Text = IIf(IsNothing(p.Q482) = True, "", String.Format("{0:n0}", p.Q482))
                chkQ49.ClearSelection()
                If Not p.Q49 Is Nothing Then
                    chkQ49.SelectedValue = Math.Abs(CInt(p.Q49))
                End If

                ddlQ410.ClearSelection()
                If Not IsNothing(p.Q410) Then
                    ddlQ410.SelectedValue = p.Q410
                End If
                txtQ4101.Text = IIf(IsNothing(p.Q4101) = True, "", p.Q4101)
                txtQ41011.Text = IIf(IsNothing(p.Q41011) = True, "", p.Q41011)
                chkQ4102.ClearSelection()
                If Not p.Q4102 Is Nothing Then
                    chkQ4102.SelectedValue = Math.Abs(CInt(p.Q4102))
                End If

                txtQ41021.Text = IIf(IsNothing(p.Q41021) = True, "", String.Format("{0:n0}", p.Q41021))
                txtQ41022.Text = IIf(IsNothing(p.Q41022) = True, "", String.Format("{0:n0}", p.Q41022))
                chkQ4103.ClearSelection()
                If Not p.Q4103 Is Nothing Then
                    chkQ4103.SelectedValue = Math.Abs(CInt(p.Q4103))
                End If
                txtQ410311.Text = IIf(IsNothing(p.Q410311) = True, "", String.Format("{0:n0}", p.Q410311))
                txtQ410312.Text = IIf(IsNothing(p.Q410312) = True, "", String.Format("{0:n0}", p.Q410312))
                chkQ41032.ClearSelection()
                If Not p.Q41032 Is Nothing Then
                    chkQ41032.SelectedValue = Math.Abs(CInt(p.Q41032))
                End If
                txtQ41033.Text = IIf(IsNothing(p.Q41033) = True, "", p.Q41033)
                '4.10.4 Phụ cấp độc hại hoặc tính tiền độc hại vào lương:
                chkQ4104.ClearSelection()
                If Not IsNothing(p.Q4104) Then
                    chkQ4104.SelectedValue = p.Q4104
                End If
                txtQ41041.Text = IIf(IsNothing(p.Q41041) = True, "", p.Q41041)
                 
                '4.10.5 Trả lương ngày lễ, ngày nghỉ hưởng nguyên lương
                chkQ4105.ClearSelection()
                If Not IsNothing(p.Q4105) Then
                    chkQ4105.SelectedValue = p.Q4105
                End If
            End If
        End Using
    End Sub
    Protected Sub ResetControl()
        txtQ4111.Text = ""
        txtQ4112.Text = ""
        txtQ4114.Text = ""
        txtQ4116.Text = ""
        chkQ4113.ClearSelection()
        txtQ41131.Text = ""
        chkQ421.Checked = False
        chkQ422.Checked = False
        chkQ423.Checked = False
        rdlQ424.ClearSelection()
        chkQ43.ClearSelection()
        rdlQ44.SelectedValue = 3
        chkQ45.ClearSelection()
        chkQ46a.Checked = False
        chkQ46b.Checked = False
        txtQ461.Text = ""
        txtQ462.Text = ""
        txtQ463.Text = ""
        txtQ464.Text = ""
        txtQ47.Text = ""
        chkQ48.ClearSelection()
        txtQ481.Text = ""
        txtQ482.Text = ""
        chkQ49.ClearSelection()

        ddlQ410.ClearSelection()
        txtQ4101.Text = ""
        txtQ41011.Text = ""
        chkQ4102.ClearSelection()
        txtQ41021.Text = ""
        txtQ41022.Text = ""
        chkQ4103.ClearSelection()
        txtQ410311.Text = ""
        txtQ410312.Text = ""
        chkQ41032.ClearSelection()
        txtQ41033.Text = ""
        '4.10.4 Phụ cấp độc hại hoặc tính tiền độc hại vào lương:
        chkQ4104.ClearSelection()
        txtQ41041.Text = ""
        '4.10.5 Trả lương ngày lễ, ngày nghỉ hưởng nguyên lương
        chkQ4105.ClearSelection()
    End Sub
#End Region
#Region "Event for control "
    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Session("phieuid") = hidPhieuID.Value
        Session("IsUser") = hidIsUser.Value
        Session("ModePhieu") = hidModePhieu.Value

        If Save() Then
            Dim iDN = Request.QueryString("DNId")
            Excute_Javascript("AlertboxRedirect('Mời bạn nhập tiếp mục 5.','CauHoi5.aspx?DNId=" & iDN & "');", Me.Page, True)
        Else
            Excute_Javascript("Alertbox('Cập nhật thất bại.');", Me.Page, True)
        End If
    End Sub

    Protected Sub btnReset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReset.Click
        Using data As New ThanhTraLaoDongEntities
            Dim q4 = (From p In data.CauHoi4 Where p.PhieuId = hidPhieuID.Value).FirstOrDefault
            If Not IsNothing(q4) Then
                ShowData()
            Else
                ResetControl()
            End If
        End Using
    End Sub

#End Region
End Class
