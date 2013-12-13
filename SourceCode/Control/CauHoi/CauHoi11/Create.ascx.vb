Imports System.Data
Imports ThanhTraLaoDongModel
Imports Cls_Common
Imports SecurityService

Partial Class Control_CauHoi11_Create
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
                'Lưu các session và hidden field để dễ xử lý
                hidPhieuID.Value = IIf(IsNothing(Session("phieuid")), 0, Session("phieuid"))
                hidIsUser.Value = IIf(IsNothing(Session("IsUser")), 0, Session("IsUser"))
                hidModePhieu.Value = IIf(IsNothing(Session("ModePhieu")), 0, Session("ModePhieu"))
                'Neu ModePhieu la xem chi tiet
                If hidModePhieu.Value = ModePhieu.Detail Then
                    btnSave.Visible = False
                    btnReset.Visible = False
                End If
                'Load nội dung cauhoi11
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
                    Dim iQ1111, iQ1112, iQ1121, iQ1131 As Integer
                    iQ1111 = GetNumberByFormat(txtQ1111.Text.Trim()) 'Số lao động dưới 15 tuổi
                    iQ1112 = GetNumberByFormat(txtQ1112.Text.Trim()) 'từ 15 đến 18 tuổi
                    iQ1121 = GetNumberByFormat(txtQ1121.Text.Trim()) 'Lao động người cao tuổi
                    iQ1131 = GetNumberByFormat(txtQ1131.Text.Trim()) 'Lao động tàn tật

                    Dim cauhoi11 = (From q In data.CauHoi11 Where q.PhieuId = hidPhieuID.Value).FirstOrDefault()
                    ' Check Exists CauHoi11 by PhieID
                    If cauhoi11 Is Nothing Then '' Nếu chưa có câu hỏi 11 cho phiếu  này thì tạo mới.
                        cauhoi11 = New CauHoi11
                        cauhoi11.PhieuId = hidPhieuID.Value
                        ''''''''Nếu Q11 không check bỏ qua mục 11
                        ''''''''Nếu Q11 check có tiếp tục lưu các mục nhỏ trong mục 11, ngược lại không lưu
                        cauhoi11.Q11 = Nothing '11. Lao động đặc thù 
                        If chkQ11.SelectedValue <> "" Then
                            If chkQ11.SelectedValue = "1" Then
                                cauhoi11.Q11 = True '11. Lao động đặc thù 
                                '11.1 Lao động chưa thành niên
                                ''''''Nếu một trong hai Số lao động dưới 15 tuổi & từ 15 đến 18 tuổi có giá trị thì lưu các mục nhỏ bên trong, ngược lại không lưu
                                If iQ1111 > 0 Or iQ1112 > 0 Then
                                    cauhoi11.Q1111 = IIf(iQ1111 > 0, iQ1111, Nothing) 'Số lao động dưới 15 tuổi
                                    cauhoi11.Q1112 = IIf(iQ1112 > 0, iQ1112, Nothing) 'từ 15 đến 18 tuổi
                                    cauhoi11.Q112 = Nothing 'Lập sổ theo dõi riêng
                                    If chkQ112.SelectedValue <> "" Then
                                        cauhoi11.Q112 = chkQ112.SelectedValue = "1"
                                    End If
                                    cauhoi11.Q113 = Nothing 'Kí hợp đồng lao động với người đại diện theo pháp luật của trẻ dưới 15 tuổi
                                    If chkQ113.SelectedValue <> "" Then
                                        cauhoi11.Q113 = chkQ113.SelectedValue = "1"
                                    End If
                                    cauhoi11.Q114 = Nothing 'Làm các công việc nặng nhọc, nguy hiểm
                                    If chkQ114.SelectedValue <> "" Then
                                        cauhoi11.Q114 = Not chkQ114.SelectedValue = "1"
                                    End If
                                    cauhoi11.Q115 = Nothing 'Lập hồ sơ sức khoẻ
                                    If chkQ115.SelectedValue <> "" Then
                                        cauhoi11.Q115 = chkQ115.SelectedValue = "1"
                                    End If
                                    cauhoi11.Q116 = Nothing 'Thực hiện giảm giờ làm với lao động chưa thành niên
                                    If chkQ116.SelectedValue <> "" Then
                                        cauhoi11.Q116 = chkQ116.SelectedValue = "1"
                                    End If
                                    cauhoi11.Q117 = Nothing 'Tạo điều kiện cho người chưa thành niên học văn hóa
                                    If chkQ117.SelectedValue <> "" Then
                                        cauhoi11.Q117 = chkQ117.SelectedValue = "1"
                                    End If
                                    cauhoi11.Q118 = Nothing ' Bố trí công việc cấm người chưa thành niên
                                    If chkQ118.SelectedValue <> "" Then
                                        cauhoi11.Q118 = chkQ118.SelectedValue = "1"
                                    End If
                                    cauhoi11.Q119 = Nothing ' Tham gia BHXH
                                    If chkQ119.SelectedValue <> "" Then
                                        cauhoi11.Q119 = chkQ119.SelectedValue = "1"
                                    End If
                                Else
                                    '11.1 Lao động chưa thành niên
                                    cauhoi11.Q1111 = Nothing 'Số lao động dưới 15 tuổi
                                    cauhoi11.Q1112 = Nothing 'từ 15 đến 18 tuổi
                                    cauhoi11.Q112 = Nothing 'Lập sổ theo dõi riêng
                                    cauhoi11.Q113 = Nothing 'Kí hợp đồng lao động với người đại diện theo pháp luật của trẻ dưới 15 tuổi
                                    cauhoi11.Q114 = Nothing 'Làm các công việc nặng nhọc, nguy hiểm
                                    cauhoi11.Q115 = Nothing 'Lập hồ sơ sức khoẻ
                                    cauhoi11.Q116 = Nothing 'Thực hiện giảm giờ làm với lao động chưa thành niên
                                    cauhoi11.Q117 = Nothing 'Tạo điều kiện cho người chưa thành niên học văn hóa
                                    cauhoi11.Q118 = Nothing ' Bố trí công việc cấm người chưa thành niên
                                    cauhoi11.Q118 = Nothing ' Tham gia BHXH
                                End If
                                '11.2 Lao động người cao tuổi
                                cauhoi11.Q1121 = IIf(iQ1121 > 0, iQ1121, Nothing)
                                '11.3 Lao động tàn tật
                                cauhoi11.Q1131 = IIf(iQ1131 > 0, iQ1131, Nothing)
                            Else
                                cauhoi11.Q11 = False '11. Lao động đặc thù 
                                '11.1 Lao động chưa thành niên
                                cauhoi11.Q1111 = Nothing 'Số lao động dưới 15 tuổi
                                cauhoi11.Q1112 = Nothing 'từ 15 đến 18 tuổi
                                cauhoi11.Q112 = Nothing 'Lập sổ theo dõi riêng
                                cauhoi11.Q113 = Nothing 'Kí hợp đồng lao động với người đại diện theo pháp luật của trẻ dưới 15 tuổi
                                cauhoi11.Q114 = Nothing 'Làm các công việc nặng nhọc, nguy hiểm
                                cauhoi11.Q115 = Nothing 'Lập hồ sơ sức khoẻ
                                cauhoi11.Q116 = Nothing 'Thực hiện giảm giờ làm với lao động chưa thành niên
                                cauhoi11.Q117 = Nothing 'Tạo điều kiện cho người chưa thành niên học văn hóa
                                cauhoi11.Q118 = Nothing ' Bố trí công việc cấm người chưa thành niên
                                '11.2 Lao động người cao tuổi
                                cauhoi11.Q1121 = Nothing
                                '11.3 Lao động tàn tật
                                cauhoi11.Q1131 = Nothing
                            End If
                        Else
                            '11.1 Lao động chưa thành niên
                            cauhoi11.Q1111 = Nothing 'Số lao động dưới 15 tuổi
                            cauhoi11.Q1112 = Nothing 'từ 15 đến 18 tuổi
                            cauhoi11.Q112 = Nothing 'Lập sổ theo dõi riêng
                            cauhoi11.Q113 = Nothing 'Kí hợp đồng lao động với người đại diện theo pháp luật của trẻ dưới 15 tuổi
                            cauhoi11.Q114 = Nothing 'Làm các công việc nặng nhọc, nguy hiểm
                            cauhoi11.Q115 = Nothing 'Lập hồ sơ sức khoẻ
                            cauhoi11.Q116 = Nothing 'Thực hiện giảm giờ làm với lao động chưa thành niên
                            cauhoi11.Q117 = Nothing 'Tạo điều kiện cho người chưa thành niên học văn hóa
                            cauhoi11.Q118 = Nothing ' Bố trí công việc cấm người chưa thành niên
                            '11.2 Lao động người cao tuổi
                            cauhoi11.Q1121 = Nothing
                            '11.3 Lao động tàn tật
                            cauhoi11.Q1131 = Nothing
                        End If
                        cauhoi11.NguoiTao = Session("Username")
                        cauhoi11.NgayTao = Date.Now
                        data.CauHoi11.AddObject(cauhoi11)
                        'Luu cau hoi da tra loi
                        Dim pn = (From a In data.PhieuNhapHeaders Where a.PhieuID = hidPhieuID.Value Select a).FirstOrDefault
                        pn.CauHoiDaTraLoi = pn.CauHoiDaTraLoi & "11;"
                        data.SaveChanges()
                        If hidIsUser.Value = 1 Then
                            Insert_App_Log("Insert  Cauhoi11: PhieuId - " & hidPhieuID.Value, Function_Name.BienBanThanhTra, Audit_Type.Create, Request.ServerVariables("REMOTE_ADDR"), Session("UserName"))
                        ElseIf hidIsUser.Value = 2 Then
                            Insert_App_Log("Insert  Cauhoi11: PhieuId - " & hidPhieuID.Value, Function_Name.PhieuKiemTra, Audit_Type.Create, Request.ServerVariables("REMOTE_ADDR"), Session("UserName"))
                        End If
                        Return True
                    Else
                        ''''''''Nếu Q11 không check bỏ qua mục 11
                        ''''''''Nếu Q11 check có tiếp tục lưu các mục nhỏ trong mục 11, ngược lại không lưu
                        cauhoi11.Q11 = Nothing '11. Lao động đặc thù 
                        If chkQ11.SelectedValue <> "" Then
                            If chkQ11.SelectedValue = "1" Then
                                cauhoi11.Q11 = True '11. Lao động đặc thù 
                                '11.1 Lao động chưa thành niên
                                ''''''Nếu một trong hai Số lao động dưới 15 tuổi & từ 15 đến 18 tuổi có giá trị thì lưu các mục nhỏ bên trong, ngược lại không lưu
                                If iQ1111 > 0 Or iQ1112 > 0 Then
                                    cauhoi11.Q1111 = IIf(iQ1111 > 0, iQ1111, Nothing) 'Số lao động dưới 15 tuổi
                                    cauhoi11.Q1112 = IIf(iQ1112 > 0, iQ1112, Nothing) 'từ 15 đến 18 tuổi
                                    cauhoi11.Q112 = Nothing 'Lập sổ theo dõi riêng
                                    If chkQ112.SelectedValue <> "" Then
                                        cauhoi11.Q112 = chkQ112.SelectedValue = "1"
                                    End If
                                    cauhoi11.Q113 = Nothing 'Kí hợp đồng lao động với người đại diện theo pháp luật của trẻ dưới 15 tuổi
                                    If chkQ113.SelectedValue <> "" Then
                                        cauhoi11.Q113 = chkQ113.SelectedValue = "1"
                                    End If
                                    cauhoi11.Q114 = Nothing 'Làm các công việc nặng nhọc, nguy hiểm
                                    If chkQ114.SelectedValue <> "" Then
                                        cauhoi11.Q114 = Not chkQ114.SelectedValue = "1"
                                    End If
                                    cauhoi11.Q115 = Nothing 'Lập hồ sơ sức khoẻ
                                    If chkQ115.SelectedValue <> "" Then
                                        cauhoi11.Q115 = chkQ115.SelectedValue = "1"
                                    End If
                                    cauhoi11.Q116 = Nothing 'Thực hiện giảm giờ làm với lao động chưa thành niên
                                    If chkQ116.SelectedValue <> "" Then
                                        cauhoi11.Q116 = chkQ116.SelectedValue = "1"
                                    End If
                                    cauhoi11.Q117 = Nothing 'Tạo điều kiện cho người chưa thành niên học văn hóa
                                    If chkQ117.SelectedValue <> "" Then
                                        cauhoi11.Q117 = chkQ117.SelectedValue = "1"
                                    End If
                                    cauhoi11.Q118 = Nothing ' Bố trí công việc cấm người chưa thành niên
                                    If chkQ118.SelectedValue <> "" Then
                                        cauhoi11.Q118 = chkQ118.SelectedValue = "1"
                                    End If
                                    cauhoi11.Q119 = Nothing ' Tham gia BHXH
                                    If chkQ119.SelectedValue <> "" Then
                                        cauhoi11.Q119 = chkQ119.SelectedValue = "1"
                                    End If
                                Else
                                    '11.1 Lao động chưa thành niên
                                    cauhoi11.Q1111 = Nothing 'Số lao động dưới 15 tuổi
                                    cauhoi11.Q1112 = Nothing 'từ 15 đến 18 tuổi
                                    cauhoi11.Q112 = Nothing 'Lập sổ theo dõi riêng
                                    cauhoi11.Q113 = Nothing 'Kí hợp đồng lao động với người đại diện theo pháp luật của trẻ dưới 15 tuổi
                                    cauhoi11.Q114 = Nothing 'Làm các công việc nặng nhọc, nguy hiểm
                                    cauhoi11.Q115 = Nothing 'Lập hồ sơ sức khoẻ
                                    cauhoi11.Q116 = Nothing 'Thực hiện giảm giờ làm với lao động chưa thành niên
                                    cauhoi11.Q117 = Nothing 'Tạo điều kiện cho người chưa thành niên học văn hóa
                                    cauhoi11.Q118 = Nothing ' Bố trí công việc cấm người chưa thành niên
                                    cauhoi11.Q118 = Nothing ' Tham gia BHXH
                                End If
                                '11.2 Lao động người cao tuổi
                                cauhoi11.Q1121 = IIf(iQ1121 > 0, iQ1121, Nothing)
                                '11.3 Lao động tàn tật
                                cauhoi11.Q1131 = IIf(iQ1131 > 0, iQ1131, Nothing)
                            Else
                                cauhoi11.Q11 = False '11. Lao động đặc thù 
                                '11.1 Lao động chưa thành niên
                                cauhoi11.Q1111 = Nothing 'Số lao động dưới 15 tuổi
                                cauhoi11.Q1112 = Nothing 'từ 15 đến 18 tuổi
                                cauhoi11.Q112 = Nothing 'Lập sổ theo dõi riêng
                                cauhoi11.Q113 = Nothing 'Kí hợp đồng lao động với người đại diện theo pháp luật của trẻ dưới 15 tuổi
                                cauhoi11.Q114 = Nothing 'Làm các công việc nặng nhọc, nguy hiểm
                                cauhoi11.Q115 = Nothing 'Lập hồ sơ sức khoẻ
                                cauhoi11.Q116 = Nothing 'Thực hiện giảm giờ làm với lao động chưa thành niên
                                cauhoi11.Q117 = Nothing 'Tạo điều kiện cho người chưa thành niên học văn hóa
                                cauhoi11.Q118 = Nothing ' Bố trí công việc cấm người chưa thành niên
                                '11.2 Lao động người cao tuổi
                                cauhoi11.Q1121 = Nothing
                                '11.3 Lao động tàn tật
                                cauhoi11.Q1131 = Nothing
                            End If
                        Else
                            '11.1 Lao động chưa thành niên
                            cauhoi11.Q1111 = Nothing 'Số lao động dưới 15 tuổi
                            cauhoi11.Q1112 = Nothing 'từ 15 đến 18 tuổi
                            cauhoi11.Q112 = Nothing 'Lập sổ theo dõi riêng
                            cauhoi11.Q113 = Nothing 'Kí hợp đồng lao động với người đại diện theo pháp luật của trẻ dưới 15 tuổi
                            cauhoi11.Q114 = Nothing 'Làm các công việc nặng nhọc, nguy hiểm
                            cauhoi11.Q115 = Nothing 'Lập hồ sơ sức khoẻ
                            cauhoi11.Q116 = Nothing 'Thực hiện giảm giờ làm với lao động chưa thành niên
                            cauhoi11.Q117 = Nothing 'Tạo điều kiện cho người chưa thành niên học văn hóa
                            cauhoi11.Q118 = Nothing ' Bố trí công việc cấm người chưa thành niên
                            '11.2 Lao động người cao tuổi
                            cauhoi11.Q1121 = Nothing
                            '11.3 Lao động tàn tật
                            cauhoi11.Q1131 = Nothing
                        End If

                        cauhoi11.NguoiSua = Session("Username")
                        cauhoi11.NgaySua = Date.Now
                        'Luu ngay sua, nguoi sua phieu
                        Dim pn = (From a In data.PhieuNhapHeaders Where a.PhieuID = hidPhieuID.Value Select a).FirstOrDefault
                        pn.NgaySua = Date.Now
                        pn.NguoiSua = Session("Username")
                        data.SaveChanges()
                        If hidIsUser.Value = 1 Then
                            Insert_App_Log("Update  Cauhoi11: PhieuId - " & hidPhieuID.Value, Function_Name.BienBanThanhTra, Audit_Type.Edit, Request.ServerVariables("REMOTE_ADDR"), Session("UserName"))
                        ElseIf hidIsUser.Value = 2 Then
                            Insert_App_Log("Update  Cauhoi11: PhieuId - " & hidPhieuID.Value, Function_Name.PhieuKiemTra, Audit_Type.Edit, Request.ServerVariables("REMOTE_ADDR"), Session("UserName"))
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

            Dim p As CauHoi11 = (From q In data.CauHoi11 Where q.PhieuId = hidPhieuID.Value Select q).FirstOrDefault
            If Not p Is Nothing Then
                If p.Q11.HasValue Then
                    chkQ11.SelectedValue = Math.Abs(CInt(p.Q11))
                Else
                    chkQ11.ClearSelection()
                End If
                txtQ1111.Text = IIf(IsNothing(p.Q1111) = True, "", p.Q1111) 'Số lao động dưới 15 tuổi 
                txtQ1112.Text = IIf(IsNothing(p.Q1112) = True, "", p.Q1112) 'từ 15 đến 18 tuổi

                If p.Q112.HasValue Then
                    chkQ112.SelectedValue = Math.Abs(CInt(p.Q112)) 'Lập sổ theo dõi riêng
                Else
                    chkQ112.ClearSelection()
                End If

                If p.Q113.HasValue Then
                    chkQ113.SelectedValue = Math.Abs(CInt(p.Q113)) 'Kí hợp đồng lao động với người đại diện theo pháp luật của trẻ dưới 15 tuổi
                Else
                    chkQ113.ClearSelection()
                End If

                If p.Q114.HasValue Then
                    chkQ114.SelectedValue = Math.Abs(CInt(Not p.Q114)) 'Làm các công việc nặng nhọc, nguy hiểm
                Else
                    chkQ114.ClearSelection()
                End If

                If p.Q115.HasValue Then
                    chkQ115.SelectedValue = Math.Abs(CInt(p.Q115)) 'Lập hồ sơ sức khoẻ
                Else
                    chkQ115.ClearSelection()
                End If

                If p.Q116.HasValue Then
                    chkQ116.SelectedValue = Math.Abs(CInt(p.Q116)) 'Thực hiện giảm giờ làm với lao động chưa thành niên
                Else
                    chkQ116.ClearSelection()
                End If
                If p.Q117.HasValue Then
                    chkQ117.SelectedValue = Math.Abs(CInt(p.Q117)) 'Tạo điều kiện cho người chưa thành niên học văn hóa
                Else
                    chkQ117.ClearSelection()
                End If
                If p.Q118.HasValue Then
                    chkQ118.SelectedValue = Math.Abs(CInt(p.Q118)) 'Bố trí công việc cấm người chưa thành niên
                Else
                    chkQ118.ClearSelection()
                End If
                If p.Q119.HasValue Then
                    chkQ119.SelectedValue = Math.Abs(CInt(p.Q119)) 'Tham gia BHXH
                Else
                    chkQ119.ClearSelection()
                End If
                txtQ1121.Text = IIf(IsNothing(p.Q1121), "", p.Q1121)
                txtQ1131.Text = IIf(IsNothing(p.Q1131), "", p.Q1131)
            End If
        End Using
    End Sub
    Protected Sub ResetControl()
        chkQ11.ClearSelection()
        txtQ1111.Text = ""
        txtQ1112.Text = ""
        chkQ112.ClearSelection()
        chkQ113.ClearSelection()
        chkQ114.ClearSelection()
        chkQ115.ClearSelection()
        chkQ116.ClearSelection()
        chkQ117.ClearSelection()
        txtQ1121.Text = ""
        txtQ1131.Text = ""
    End Sub
#End Region
#Region "Event for control "
    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Session("phieuid") = hidPhieuID.Value
        Session("IsUser") = hidIsUser.Value
        Session("ModePhieu") = hidModePhieu.Value

        If Save() Then
            Dim iDN = Request.QueryString("DNId")
            Excute_Javascript("AlertboxRedirect('Mời bạn nhập tiếp mục 12.','CauHoi12.aspx?DNId=" & iDN & "');", Me.Page, True)
        Else
            Excute_Javascript("Alertbox('Cập nhật thất bại.');", Me.Page, True)
        End If
    End Sub

    Protected Sub btnReset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReset.Click
        Using data As New ThanhTraLaoDongEntities
            Dim q11 = (From p In data.CauHoi11 Where p.PhieuId = hidPhieuID.Value).FirstOrDefault
            If Not IsNothing(q11) Then
                ShowData()
            Else
                ResetControl()
            End If
        End Using
    End Sub
    
#End Region


   
End Class
