Imports System.Data
Imports ThanhTraLaoDongModel
Imports Cls_Common
Imports SecurityService
Partial Class Control_CauHoi2_Create
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
                'Lưu các session vào các hidden field cho dễ xử lý
                hidPhieuID.Value = IIf(IsNothing(Session("phieuid")), 0, Session("phieuid"))
                hidIsUser.Value = IIf(IsNothing(Session("IsUser")), 0, Session("IsUser"))
                hidModePhieu.Value = IIf(IsNothing(Session("ModePhieu")), 0, Session("ModePhieu"))
                'Neu ModePhieu la xem chi tiet
                If hidModePhieu.Value = ModePhieu.Detail Then
                    btnSave.Visible = False
                    btnReset.Visible = False
                End If
                'Load nội dung cauhoi2
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
                If Not IsNothing(Session("Username")) Then
                    'Kiem tra xem da co phieuid trong bang cauhoi2 chua?
                    'Neu chua thi tao moi
                    'Neu da co thi cap nhat lai
                    Dim q2 = (From a In data.CauHoi2 Where a.PhieuId = hidPhieuID.Value Select a).FirstOrDefault
                    Dim iQ211, iQ212, iQ213, iQ214, iQ215, iQ216, iQ217, iQ218, iQ219, iQ2110, iQ2331, iQ234, iQ235, iQ238, iQ241, iQ251 As Integer
                    iQ211 = GetNumberByFormat(txtQ211.Text.Trim()) 'HĐLĐ không xác định thời hạn
                    iQ212 = GetNumberByFormat(txtQ212.Text.Trim()) 'HĐLĐ xác định thời hạn từ 12 tháng đến 36 tháng
                    iQ213 = GetNumberByFormat(txtQ213.Text.Trim()) 'HĐLĐ xác định thời hạn từ 3 tháng đến dưới 12 tháng
                    iQ214 = GetNumberByFormat(txtQ214.Text.Trim()) 'HĐLĐ mùa vụ dưới 3 tháng
                    iQ215 = GetNumberByFormat(txtQ215.Text.Trim()) 'Hợp đồng khoán gọn theo vụ việc
                    iQ216 = GetNumberByFormat(txtQ216.Text.Trim()) 'Hợp đồng học nghề, thử việc
                    iQ217 = GetNumberByFormat(txtQ217.Text.Trim()) 'Chưa kí hợp đồng lao động
                    iQ219 = GetNumberByFormat(txtQ219.Text.Trim()) 'Số lao động thuê của doanh nghiệp khác
                    iQ2110 = GetNumberByFormat(txtQ2110.Text.Trim()) 'Không phải kí hợp đồng lao động
                    iQ218 = GetNumberByFormat(txtQ218.Text.Trim()) 'Tổng số lao động
                    iQ2331 = GetNumberByFormat(txtQ2331.Text.Trim()) 'Số trường hợp, Giữ bản gốc văn bằng hoặc tiền đặt cọc của người lao động trái luật
                    iQ234 = GetNumberByFormat(txtQ234.Text.Trim()) 'Nhu cầu tuyển dụng năm hiện tại
                    iQ235 = GetNumberByFormat(txtQ235.Text.Trim()) 'Số lao động mới được tuyển
                    iQ238 = GetNumberByFormat(txtQ238.Text.Trim()) 'Số lao động mới được tuyển
                    iQ241 = GetNumberByFormat(txtQ241.Text.Trim()) 'Số lao động bị mất việc làm
                    iQ251 = GetNumberByFormat(txtQ251.Text.Trim()) 'Số lao động thôi việc:

                    'TH1: New Insert cauhoi2
                    If q2 Is Nothing Then
                        q2 = New CauHoi2
                        q2.PhieuId = hidPhieuID.Value
                        ''2. Hợp đồng lao động
                        ''''2.1. Kí hợp đồng đúng loại
                        q2.Q21 = Nothing
                        If chkQ21.SelectedValue <> "" Then
                            q2.Q21 = chkQ21.SelectedValue = "1"
                        End If
                        q2.Q211 = IIf(iQ211 > 0 And chkQ21.SelectedValue <> "", iQ211, Nothing) 'HĐLĐ không xác định thời hạn
                        q2.Q212 = IIf(iQ212 > 0 And chkQ21.SelectedValue <> "", iQ212, Nothing) 'HĐLĐ xác định thời hạn từ 12 tháng đến 36 tháng
                        q2.Q213 = IIf(iQ213 > 0 And chkQ21.SelectedValue <> "", iQ213, Nothing) 'HĐLĐ xác định thời hạn từ 3 tháng đến dưới 12 tháng
                        q2.Q214 = IIf(iQ214 > 0 And chkQ21.SelectedValue <> "", iQ214, Nothing) 'HĐLĐ mùa vụ dưới 3 tháng
                        q2.Q215 = IIf(iQ215 > 0 And chkQ21.SelectedValue <> "", iQ215, Nothing) 'Hợp đồng khoán gọn theo vụ việc
                        q2.Q216 = IIf(iQ216 > 0 And chkQ21.SelectedValue <> "", iQ216, Nothing) 'Hợp đồng học nghề, thử việc
                        q2.Q217 = IIf(iQ217 > 0 And chkQ21.SelectedValue <> "", iQ217, Nothing) 'Chưa kí hợp đồng lao động
                        q2.Q219 = IIf(iQ219 > 0 And chkQ21.SelectedValue <> "", iQ219, Nothing) 'Số lao động thuê của doanh nghiệp khác
                        q2.Q2110 = IIf(iQ2110 > 0 And chkQ21.SelectedValue <> "", iQ2110, Nothing) 'Không phải kí hợp đồng lao động
                        q2.Q218 = IIf(iQ218 > 0 And chkQ21.SelectedValue <> "", iQ218, Nothing) 'Tổng số lao động
                        ''2.2. Ghi hợp đồng lao động cụ thể về:
                        q2.Q221 = Nothing
                        If chkQ221.SelectedValue <> "" Then
                            q2.Q221 = chkQ221.SelectedValue = "1" ''+ Chức danh nghề, công việc độc hại, nguy hiểm
                        End If
                        q2.Q222 = Nothing
                        If chkQ222.SelectedValue <> "" Then
                            q2.Q222 = chkQ222.SelectedValue = "1" ''+ Công việc và địa điểm làm việc
                        End If
                        q2.Q223 = Nothing
                        If chkQ223.SelectedValue <> "" Then
                            q2.Q223 = chkQ223.SelectedValue = "1" '+ Thời hạn của hợp đồng
                        End If
                        q2.Q224 = Nothing
                        If chkQ224.SelectedValue <> "" Then
                            q2.Q224 = chkQ224.SelectedValue = "1" '+ Mức lương
                        End If
                        q2.Q225 = Nothing
                        If chkQ225.SelectedValue <> "" Then
                            q2.Q225 = chkQ225.SelectedValue = "1" '+ Chế độ nâng lương
                        End If
                        q2.Q226 = Nothing
                        If chkQ226.SelectedValue <> "" Then
                            q2.Q226 = chkQ226.SelectedValue = "1" '+ Thời giờ làm việc, nghỉ ngơi
                        End If
                        q2.Q227 = Nothing
                        If chkQ227.SelectedValue <> "" Then
                            q2.Q227 = chkQ227.SelectedValue = "1" '+ Phương tiện bảo vệ cá nhân
                        End If
                        q2.Q228 = Nothing
                        If chkQ228.SelectedValue <> "" Then
                            q2.Q228 = chkQ228.SelectedValue = "1" '+ Chế độ bảo hiểm xã hội, bảo hiểm y tế
                        End If
                        q2.Q229 = Nothing
                        If chkQ229.SelectedValue <> "" Then
                            q2.Q229 = chkQ229.SelectedValue = "1" '+ Đào tạo nâng cao trình độ, tay nghề
                        End If
                        q2.Q2210 = Nothing
                        If chkQ2210.SelectedValue <> "" Then
                            q2.Q2210 = chkQ2210.SelectedValue = "1" '+ Bí mật công nghệ, bí mật kinh doanh
                        End If
                        q2.Q2211 = Nothing
                        q2.Q22111 = Nothing
                        If chkQ2211.SelectedValue <> "" Then
                            q2.Q2211 = Not chkQ2211.SelectedValue = "1" '+ Thỏa thuận trái luật
                            If chkQ2211.SelectedValue = "1" Then
                                q2.Q22111 = txtQ22111.Text
                            Else
                                q2.Q22111 = Nothing
                            End If
                        End If
                        ''2.3. Học nghề và thử việc
                        q2.Q234 = IIf(iQ234 > 0, iQ234, Nothing) 'Nhu cầu tuyển dụng năm hiện tại
                        q2.Q235 = IIf(iQ235 > 0, iQ235, Nothing) 'Số lao động mới được tuyển
                        q2.Q236 = Nothing
                        If chkQ236.SelectedValue <> "" Then
                            q2.Q236 = chkQ236.SelectedValue = "1" 'Kế hoạch đào tạo, nâng cao trình độ, kỹ năng nghề
                        End If
                        q2.Q237 = Nothing
                        If chkQ237.SelectedValue <> "" Then
                            q2.Q237 = chkQ237.SelectedValue = "1" 'Thực hiện ký kết hợp đồng đào tạo nghề  (nếu có)
                        End If
                        q2.Q231 = Nothing
                        If chkQ231.SelectedValue <> "" Then
                            q2.Q231 = Not chkQ231.SelectedValue = "1" '- Thu phí tuyển dụng, học nghề để làm việc cho doanh nghiệp
                        End If
                        ''Áp dụng thời gian thử việc
                        q2.Q2321 = Nothing
                        If chkQ2321.SelectedValue <> "" Then
                            q2.Q2321 = Not chkQ2321.SelectedValue = "1" '+ Quá 60 ngày đối với lao động có chức danh nghề cần trình độ chuyên môn kỹ thuật từ cao đẳng trở lên
                        End If
                        q2.Q2322 = Nothing
                        If chkQ2322.SelectedValue <> "" Then
                            q2.Q2322 = Not chkQ2322.SelectedValue = "1" '+ Quá 30 ngày đối với lao động có chức danh nghề cần trình độ trung cấp
                        End If
                        q2.Q2323 = Nothing
                        If chkQ2323.SelectedValue <> "" Then
                            q2.Q2323 = Not chkQ2323.SelectedValue = "1" '+ Quá 6 ngày đối với lao động khác
                        End If
                        q2.Q233 = Nothing
                        If chkQ233.SelectedValue <> "" Then
                            q2.Q233 = Not chkQ233.SelectedValue = "1" '- Giữ bản gốc văn bằng hoặc tiền đặt cọc của người lao động
                            If chkQ233.SelectedValue = "1" Then
                                q2.Q2331 = IIf(iQ2331 > 0, iQ2331, Nothing) ' số trường hợp
                            Else
                                q2.Q2331 = Nothing
                            End If
                        End If
                        q2.Q238 = IIf(iQ238 > 0, iQ238, Nothing) 'Số người tuyển qua các Trung tâm giới thiệu việc làm
                        ''2.4. Mất việc làm
                        q2.Q241 = IIf(iQ241 > 0, iQ241, Nothing) 'Số lao động bị mất việc làm
                        q2.Q242 = Nothing
                        If chkQ242.SelectedValue <> "" Then
                            q2.Q242 = chkQ242.SelectedValue = "1" '- Báo cáo với Sở LĐTBXH trước khi cho nhiều lao động thôi việc do mất việc làm:
                        End If
                        ''2.5. Số lao động thôi việc:
                        q2.Q251 = IIf(iQ251 > 0, iQ251, Nothing)

                        q2.NguoiTao = Session("Username")
                        q2.NgayTao = Date.Now
                        data.CauHoi2.AddObject(q2)
                        'Luu cau hoi da tra loi
                        Dim pn = (From a In data.PhieuNhapHeaders Where a.PhieuID = hidPhieuID.Value Select a).FirstOrDefault
                        pn.CauHoiDaTraLoi = pn.CauHoiDaTraLoi & "2;"
                        data.SaveChanges()
                        If hidIsUser.Value = 1 Then
                            Insert_App_Log("Insert  Cauhoi2: PhieuId - " & hidPhieuID.Value, Function_Name.BienBanThanhTra, Audit_Type.Create, Request.ServerVariables("REMOTE_ADDR"), Session("UserName"))
                        ElseIf hidIsUser.Value = 2 Then
                            Insert_App_Log("Insert  Cauhoi2: PhieuId - " & hidPhieuID.Value, Function_Name.PhieuKiemTra, Audit_Type.Create, Request.ServerVariables("REMOTE_ADDR"), Session("UserName"))
                        End If
                    Else 'TH2: Update cauhoi2                    
                        ''2. Hợp đồng lao động
                        ''''2.1. Kí hợp đồng đúng loại
                        q2.Q21 = Nothing
                        If chkQ21.SelectedValue <> "" Then
                            q2.Q21 = chkQ21.SelectedValue = "1"
                        End If
                        q2.Q211 = IIf(iQ211 > 0 And chkQ21.SelectedValue <> "", iQ211, Nothing) 'HĐLĐ không xác định thời hạn
                        q2.Q212 = IIf(iQ212 > 0 And chkQ21.SelectedValue <> "", iQ212, Nothing) 'HĐLĐ xác định thời hạn từ 12 tháng đến 36 tháng
                        q2.Q213 = IIf(iQ213 > 0 And chkQ21.SelectedValue <> "", iQ213, Nothing) 'HĐLĐ xác định thời hạn từ 3 tháng đến dưới 12 tháng
                        q2.Q214 = IIf(iQ214 > 0 And chkQ21.SelectedValue <> "", iQ214, Nothing) 'HĐLĐ mùa vụ dưới 3 tháng
                        q2.Q215 = IIf(iQ215 > 0 And chkQ21.SelectedValue <> "", iQ215, Nothing) 'Hợp đồng khoán gọn theo vụ việc
                        q2.Q216 = IIf(iQ216 > 0 And chkQ21.SelectedValue <> "", iQ216, Nothing) 'Hợp đồng học nghề, thử việc
                        q2.Q217 = IIf(iQ217 > 0 And chkQ21.SelectedValue <> "", iQ217, Nothing) 'Chưa kí hợp đồng lao động
                        q2.Q219 = IIf(iQ219 > 0 And chkQ21.SelectedValue <> "", iQ219, Nothing) 'Số lao động thuê của doanh nghiệp khác
                        q2.Q2110 = IIf(iQ2110 > 0 And chkQ21.SelectedValue <> "", iQ2110, Nothing) 'Không phải kí hợp đồng lao động
                        q2.Q218 = IIf(iQ218 > 0 And chkQ21.SelectedValue <> "", iQ218, Nothing) 'Tổng số lao động
                        ''2.2. Ghi hợp đồng lao động cụ thể về:
                        q2.Q221 = Nothing
                        If chkQ221.SelectedValue <> "" Then
                            q2.Q221 = chkQ221.SelectedValue = "1" ''+ Chức danh nghề, công việc độc hại, nguy hiểm
                        End If
                        q2.Q222 = Nothing
                        If chkQ222.SelectedValue <> "" Then
                            q2.Q222 = chkQ222.SelectedValue = "1" ''+ Công việc và địa điểm làm việc
                        End If
                        q2.Q223 = Nothing
                        If chkQ223.SelectedValue <> "" Then
                            q2.Q223 = chkQ223.SelectedValue = "1" '+ Thời hạn của hợp đồng
                        End If
                        q2.Q224 = Nothing
                        If chkQ224.SelectedValue <> "" Then
                            q2.Q224 = chkQ224.SelectedValue = "1" '+ Mức lương
                        End If
                        q2.Q225 = Nothing
                        If chkQ225.SelectedValue <> "" Then
                            q2.Q225 = chkQ225.SelectedValue = "1" '+ Chế độ nâng lương
                        End If
                        q2.Q226 = Nothing
                        If chkQ226.SelectedValue <> "" Then
                            q2.Q226 = chkQ226.SelectedValue = "1" '+ Thời giờ làm việc, nghỉ ngơi
                        End If
                        q2.Q227 = Nothing
                        If chkQ227.SelectedValue <> "" Then
                            q2.Q227 = chkQ227.SelectedValue = "1" '+ Phương tiện bảo vệ cá nhân
                        End If
                        q2.Q228 = Nothing
                        If chkQ228.SelectedValue <> "" Then
                            q2.Q228 = chkQ228.SelectedValue = "1" '+ Chế độ bảo hiểm xã hội, bảo hiểm y tế
                        End If
                        q2.Q229 = Nothing
                        If chkQ229.SelectedValue <> "" Then
                            q2.Q229 = chkQ229.SelectedValue = "1" '+ Đào tạo nâng cao trình độ, tay nghề
                        End If
                        q2.Q2210 = Nothing
                        If chkQ2210.SelectedValue <> "" Then
                            q2.Q2210 = chkQ2210.SelectedValue = "1" '+ Bí mật công nghệ, bí mật kinh doanh
                        End If
                        q2.Q2211 = Nothing
                        q2.Q22111 = Nothing
                        If chkQ2211.SelectedValue <> "" Then
                            q2.Q2211 = Not chkQ2211.SelectedValue = "1" '+ Thỏa thuận trái luật
                            If chkQ2211.SelectedValue = "1" Then
                                q2.Q22111 = txtQ22111.Text
                            Else
                                q2.Q22111 = Nothing
                            End If
                        End If
                        ''2.3. Học nghề và thử việc
                        q2.Q234 = IIf(iQ234 > 0, iQ234, Nothing) 'Nhu cầu tuyển dụng năm hiện tại
                        q2.Q235 = IIf(iQ235 > 0, iQ235, Nothing) 'Số lao động mới được tuyển
                        q2.Q236 = Nothing
                        If chkQ236.SelectedValue <> "" Then
                            q2.Q236 = chkQ236.SelectedValue = "1" 'Kế hoạch đào tạo, nâng cao trình độ, kỹ năng nghề
                        End If
                        q2.Q237 = Nothing
                        If chkQ237.SelectedValue <> "" Then
                            q2.Q237 = chkQ237.SelectedValue = "1" 'Thực hiện ký kết hợp đồng đào tạo nghề  (nếu có)
                        End If
                        q2.Q231 = Nothing
                        If chkQ231.SelectedValue <> "" Then
                            q2.Q231 = Not chkQ231.SelectedValue = "1" '- Thu phí tuyển dụng, học nghề để làm việc cho doanh nghiệp
                        End If
                        ''Áp dụng thời gian thử việc
                        q2.Q2321 = Nothing
                        If chkQ2321.SelectedValue <> "" Then
                            q2.Q2321 = Not chkQ2321.SelectedValue = "1" '+ Quá 60 ngày đối với lao động có chức danh nghề cần trình độ chuyên môn kỹ thuật từ cao đẳng trở lên
                        End If
                        q2.Q2322 = Nothing
                        If chkQ2322.SelectedValue <> "" Then
                            q2.Q2322 = Not chkQ2322.SelectedValue = "1" '+ Quá 30 ngày đối với lao động có chức danh nghề cần trình độ trung cấp
                        End If
                        q2.Q2323 = Nothing
                        If chkQ2323.SelectedValue <> "" Then
                            q2.Q2323 = Not chkQ2323.SelectedValue = "1" '+ Quá 6 ngày đối với lao động khác
                        End If
                        q2.Q233 = Nothing
                        If chkQ233.SelectedValue <> "" Then
                            q2.Q233 = Not chkQ233.SelectedValue = "1" '- Giữ bản gốc văn bằng hoặc tiền đặt cọc của người lao động
                            If chkQ233.SelectedValue = "1" Then
                                q2.Q2331 = IIf(iQ2331 > 0, iQ2331, Nothing) ' số trường hợp
                            Else
                                q2.Q2331 = Nothing
                            End If
                        End If
                        q2.Q238 = IIf(iQ238 > 0, iQ238, Nothing) 'Số người tuyển qua các Trung tâm giới thiệu việc làm
                        ''2.4. Mất việc làm
                        q2.Q241 = IIf(iQ241 > 0, iQ241, Nothing) 'Số lao động bị mất việc làm

                        q2.Q242 = Nothing
                        If chkQ242.SelectedValue <> "" Then
                            q2.Q242 = chkQ242.SelectedValue = "1" '- Báo cáo với Sở LĐTBXH trước khi cho nhiều lao động thôi việc do mất việc làm:
                        End If
                        ''2.5. Số lao động thôi việc:
                        q2.Q251 = IIf(iQ251 > 0, iQ251, Nothing)

                        q2.NguoiSua = Session("Username")
                        q2.NgaySua = Date.Now
                        'Luu ngay sua, nguoi sua phieu
                        Dim pn = (From a In data.PhieuNhapHeaders Where a.PhieuID = hidPhieuID.Value Select a).FirstOrDefault
                        pn.NgaySua = Date.Now
                        pn.NguoiSua = Session("Username")
                        data.SaveChanges()
                        If hidIsUser.Value = 1 Then
                            Insert_App_Log("Update  Cauhoi2: PhieuId - " & hidPhieuID.Value, Function_Name.BienBanThanhTra, Audit_Type.Edit, Request.ServerVariables("REMOTE_ADDR"), Session("UserName"))
                        ElseIf hidIsUser.Value = 1 Then
                            Insert_App_Log("Update  Cauhoi2: PhieuId - " & hidPhieuID.Value, Function_Name.PhieuKiemTra, Audit_Type.Edit, Request.ServerVariables("REMOTE_ADDR"), Session("UserName"))
                        Else
                            'Insert_App_Log("Update  Cauhoi2: PhieuId - " & hidPhieuID.Value, Function_Name.BienBanThanhTra, Audit_Type.Edit, Request.ServerVariables("REMOTE_ADDR"), Session("UserName"))
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

            Dim p As CauHoi2 = (From q In data.CauHoi2 Where q.PhieuId = hidPhieuID.Value Select q).FirstOrDefault
            Dim pn = (From a In data.PhieuNhapHeaders Where a.PhieuID = hidPhieuID.Value Select a).FirstOrDefault

            If Not p Is Nothing Then
                chkQ21.ClearSelection()
                If Not p.Q21 Is Nothing Then
                    chkQ21.SelectedValue = Math.Abs(CInt(p.Q21))
                End If
                txtQ211.Text = IIf(IsNothing(p.Q211), "", String.Format("{0:n0}", p.Q211))
                txtQ212.Text = IIf(IsNothing(p.Q212), "", String.Format("{0:n0}", p.Q212))
                txtQ213.Text = IIf(IsNothing(p.Q213), "", String.Format("{0:n0}", p.Q213))
                txtQ214.Text = IIf(IsNothing(p.Q214), "", String.Format("{0:n0}", p.Q214))
                txtQ215.Text = IIf(IsNothing(p.Q215), "", String.Format("{0:n0}", p.Q215))
                txtQ216.Text = IIf(IsNothing(p.Q216), "", String.Format("{0:n0}", p.Q216))
                txtQ217.Text = IIf(IsNothing(p.Q217), "", String.Format("{0:n0}", p.Q217))
                txtQ219.Text = IIf(IsNothing(p.Q219), "", String.Format("{0:n0}", p.Q219))
                txtQ2110.Text = IIf(IsNothing(p.Q2110), "", String.Format("{0:n0}", p.Q2110))
                txtQ218.Text = IIf(IsNothing(p.Q218), "", String.Format("{0:n0}", p.Q218))
                chkQ221.ClearSelection()
                If Not p.Q221 Is Nothing Then
                    chkQ221.SelectedValue = Math.Abs(CInt(p.Q221)) '+ Chức danh nghề, công việc độc hại, nguy hiểm
                End If
                chkQ222.ClearSelection()
                If Not p.Q222 Is Nothing Then
                    chkQ222.SelectedValue = Math.Abs(CInt(p.Q222)) '+ Công việc và địa điểm làm việc
                End If
                chkQ223.ClearSelection()
                If Not p.Q223 Is Nothing Then
                    chkQ223.SelectedValue = Math.Abs(CInt(p.Q223)) '+ Thời hạn của hợp đồng
                End If
                chkQ224.ClearSelection()
                If Not p.Q224 Is Nothing Then
                    chkQ224.SelectedValue = Math.Abs(CInt(p.Q224)) '+ Mức lương
                End If
                chkQ225.ClearSelection()
                If Not p.Q225 Is Nothing Then
                    chkQ225.SelectedValue = Math.Abs(CInt(p.Q225)) '+ Chế độ nâng lương
                End If
                chkQ226.ClearSelection()
                If Not p.Q226 Is Nothing Then
                    chkQ226.SelectedValue = Math.Abs(CInt(p.Q226)) '+ Thời giờ làm việc, nghỉ ngơ
                End If
                chkQ227.ClearSelection()
                If Not p.Q227 Is Nothing Then
                    chkQ227.SelectedValue = Math.Abs(CInt(p.Q227)) '+ Phương tiện bảo vệ cá nhân
                End If
                chkQ228.ClearSelection()
                If Not p.Q228 Is Nothing Then
                    chkQ228.SelectedValue = Math.Abs(CInt(p.Q228)) '+ Chế độ bảo hiểm xã hội, bảo hiểm y tế
                End If
                chkQ229.ClearSelection()
                If Not p.Q229 Is Nothing Then
                    chkQ229.SelectedValue = Math.Abs(CInt(p.Q229)) '+ Đào tạo nâng cao trình độ, tay nghề
                End If
                chkQ2210.ClearSelection()
                If Not p.Q2210 Is Nothing Then
                    chkQ2210.SelectedValue = Math.Abs(CInt(p.Q2210)) '+ Bí mật công nghệ, bí mật kinh doanh
                End If
                chkQ2211.ClearSelection()
                If Not p.Q2211 Is Nothing Then
                    chkQ2211.SelectedValue = Math.Abs(CInt(Not p.Q2211)) '+ Thỏa thuận trái luật
                End If
                txtQ22111.Text = IIf(IsNothing(p.Q22111) = True, "", p.Q22111) 'Nội dung thỏa thuận trái luật
                txtQ234.Text = ""
                If Not IsNothing(p.Q234) AndAlso p.Q234 > 0 Then
                    txtQ234.Text = String.Format("{0:n0}", p.Q234) '- Nhu cầu tuyển dụng năm hiện tại
                End If
                txtQ235.Text = ""
                If Not IsNothing(p.Q235) AndAlso p.Q235 > 0 Then
                    txtQ235.Text = String.Format("{0:n0}", p.Q235) '- Số lao động mới được tuyển
                End If

                chkQ236.ClearSelection()
                If Not p.Q236 Is Nothing Then
                    chkQ236.SelectedValue = Math.Abs(CInt(p.Q236)) '- Kế hoạch đào tạo, nâng cao trình độ, kỹ năng nghề
                End If
                chkQ237.ClearSelection()
                If Not p.Q237 Is Nothing Then
                    chkQ237.SelectedValue = Math.Abs(CInt(p.Q237)) '- Thực hiện ký kết hợp đồng đào tạo nghề  (nếu có)
                End If
                chkQ231.ClearSelection()
                If Not p.Q231 Is Nothing Then
                    chkQ231.SelectedValue = Math.Abs(CInt(Not p.Q231)) '- Thu phí tuyển dụng, học nghề để làm việc cho doanh nghiệp
                End If
                chkQ2321.ClearSelection()
                If Not p.Q2321 Is Nothing Then
                    chkQ2321.SelectedValue = Math.Abs(CInt(Not p.Q2321)) '+ Quá 60 ngày đối với lao động có chức danh nghề cần trình độ chuyên môn kỹ thuật từ cao đẳng trở lên
                End If
                chkQ2322.ClearSelection()
                If Not p.Q2322 Is Nothing Then
                    chkQ2322.SelectedValue = Math.Abs(CInt(Not p.Q2322)) '+ Quá 30 ngày đối với lao động có chức danh nghề cần trình độ trung cấp
                End If
                chkQ2323.ClearSelection()
                If Not p.Q2323 Is Nothing Then
                    chkQ2323.SelectedValue = Math.Abs(CInt(Not p.Q2323)) '+ Quá 6 ngày đối với lao động khác
                End If
                chkQ233.ClearSelection()
                If Not p.Q233 Is Nothing Then
                    chkQ233.SelectedValue = Math.Abs(CInt(Not p.Q233)) '- Giữ bản gốc văn bằng hoặc tiền đặt cọc của người lao động
                End If
                txtQ2331.Text = ""
                If Not IsNothing(p.Q2331) Then
                    txtQ2331.Text = p.Q2331
                End If
                txtQ238.Text = ""
                If Not IsNothing(p.Q238) AndAlso p.Q238 > 0 Then
                    txtQ238.Text = String.Format("{0:n0}", p.Q238) '- Số người tuyển qua cácTrung tâm giới thiệu việc làm
                End If
                txtQ241.Text = ""
                If Not IsNothing(p.Q241) AndAlso p.Q241 > 0 Then
                    txtQ241.Text = String.Format("{0:n0}", p.Q241) '- Số lao động bị mất việc làm
                End If

                chkQ242.ClearSelection()
                If Not p.Q242 Is Nothing Then
                    chkQ242.SelectedValue = Math.Abs(CInt(p.Q242)) 'Báo cáo với Sở LĐTBXH trước khi cho nhiều lao động thôi việc do mất việc làm
                End If
                txtQ251.Text = ""
                If Not IsNothing(p.Q251) AndAlso p.Q251 > 0 Then
                    txtQ251.Text = String.Format("{0:n0}", p.Q251) '- Số lao động thôi việc:
                End If
            End If
        End Using
    End Sub
    Protected Sub ResetControl()
        chkQ21.ClearSelection()
        txtQ211.Text = ""
        txtQ212.Text = ""
        txtQ213.Text = ""
        txtQ214.Text = ""
        txtQ215.Text = ""
        txtQ216.Text = ""
        txtQ217.Text = ""
        txtQ219.Text = ""
        txtQ2110.Text = ""
        txtQ218.Text = ""
        chkQ221.ClearSelection()
        chkQ222.ClearSelection()
        chkQ223.ClearSelection()
        chkQ224.ClearSelection()
        chkQ225.ClearSelection()
        chkQ226.ClearSelection()
        chkQ227.ClearSelection()
        chkQ228.ClearSelection()
        chkQ229.ClearSelection()
        chkQ2210.ClearSelection()
        chkQ2211.ClearSelection()
        txtQ22111.Text = ""
        txtQ234.Text = ""
        txtQ235.Text = ""
        chkQ236.ClearSelection()
        chkQ237.ClearSelection()
        chkQ231.ClearSelection()
        chkQ2321.ClearSelection()
        chkQ2322.ClearSelection()
        chkQ2323.ClearSelection()
        chkQ233.ClearSelection()
        txtQ238.Text = ""
        txtQ241.Text = ""
        chkQ242.ClearSelection()
        txtQ251.Text = ""
    End Sub
#End Region
#Region "Event for control "
    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Session("phieuid") = hidPhieuID.Value
        Session("IsUser") = hidIsUser.Value
        Session("ModePhieu") = hidModePhieu.Value
        If Save() Then
            Dim iDN = Request.QueryString("DNId")
            Using _data As New ThanhTraLaoDongEntities
                Dim pn = (From q In _data.PhieuNhapHeaders Where q.PhieuID = hidPhieuID.Value).FirstOrDefault
                If Not IsNothing(pn) Then
                    If IsNothing(pn.IsCongDoan) OrElse Not pn.IsCongDoan Then
                        pn.CauHoiDaTraLoi = pn.CauHoiDaTraLoi.Replace("3;", "") & "3;"
                        _data.SaveChanges()
                        Excute_Javascript("AlertboxRedirect('Mời bạn nhập tiếp mục 4.','CauHoi4.aspx?DNId=" & iDN & "');", Me.Page, True)
                    Else
                        Excute_Javascript("AlertboxRedirect('Mời bạn nhập tiếp mục 3.','CauHoi3.aspx?DNId=" & iDN & "');", Me.Page, True)
                    End If
                End If
            End Using
        Else
            Excute_Javascript("Alertbox('Cập nhật thất bại.');", Me.Page, True)
        End If
    End Sub

    Protected Sub btnReset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReset.Click
        Using data As New ThanhTraLaoDongEntities
            Dim q2 = (From p In data.CauHoi2 Where p.PhieuId = hidPhieuID.Value).FirstOrDefault
            If Not IsNothing(q2) Then
                ShowData()
            Else
                ResetControl()
            End If
        End Using
    End Sub
#End Region

End Class
