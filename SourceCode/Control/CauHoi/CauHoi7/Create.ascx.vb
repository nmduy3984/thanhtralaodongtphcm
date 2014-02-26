Imports System.Data
Imports ThanhTraLaoDongModel
Imports Cls_Common
Imports SecurityService
Partial Class Control_CauHoi56_Create
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

                'Xử lý mục 7.14(được khám)
                Using data As New ThanhTraLaoDongEntities
                    'Kiểm tra xem mục 2 nhập?
                    Dim q2 As CauHoi2 = (From a In data.CauHoi2 Where a.PhieuId = hidPhieuID.Value Select a).FirstOrDefault
                    If IsNothing(q2) Then
                        'liên quan đến mục 7.14 (số được khám) do đó phải nhập mục 2 trước
                        Dim DNId = IIf(IsNothing(Request.QueryString("DNId")), 0, Request.QueryString("DNId"))
                        Excute_Javascript("AlertboxRedirect('Vui lòng nhập mục 2.','CauHoi2.aspx?DNId=" & DNId & "');", Me.Page, True)
                    Else
                        Dim p As CauHoi7 = (From q In data.CauHoi7 Where q.PhieuId = hidPhieuID.Value Select q).FirstOrDefault
                        If IsNothing(p) Then
                            Dim pn = (From a In data.PhieuNhapHeaders Where a.PhieuID = hidPhieuID.Value).FirstOrDefault()
                            Dim lhdnid = data.uspLoaiHinhDNId(hidPhieuID.Value).FirstOrDefault
                            'tính số được khám sức khỏe định kỳ của người lao động
                            Dim soduockham As Integer
                            Dim KhongPhaiKyHDLD As Integer = 0
                            If pn.DoanhNghiep.LoaiHinhDNId = 1 Then
                                KhongPhaiKyHDLD = IIf(IsNothing(q2.Q2110), 0, q2.Q2110)
                            End If
                            soduockham = IIf(IsNothing(q2.Q218), 0, q2.Q218) - (IIf(IsNothing(q2.Q217), 0, q2.Q217) + IIf(IsNothing(q2.Q219), 0, q2.Q219) + KhongPhaiKyHDLD)
                            txtQ71421.Text = IIf(soduockham > 0, FormatNumber(soduockham), "")
                            'hidSoNguoiDuocKham.Value = IIf(soduockham > 0, FormatNumber(soduockham), "")
                            hidTongLaoDong.Value = IIf(IsNothing(pn.TongSoNhanVien), 0, pn.TongSoNhanVien)
                            txtSoNguoiPhaiCapTheAT.Text = IIf(IsNothing(pn.SoNguoiLamCongViecYeuCauNghiemNgat) = True, "", pn.SoNguoiLamCongViecYeuCauNghiemNgat)
                            hidSoLDLamCVDHNH.Value = IIf(IsNothing(pn.SoNguoiLamNgheNguyHiem) = True, "", pn.SoNguoiLamNgheNguyHiem)
                        Else
                            'Load nội dung cauhoi7
                            ShowData()
                        End If
                    End If

                End Using
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
            'Integer.TryParse(Session("phieuid").ToString(), hidPhieuID.Value)

            ' Check Exists CauHoi7 by PhieID
            Dim cauhoi7 As CauHoi7 = (From q In data.CauHoi7 Where q.PhieuId = hidPhieuID.Value).FirstOrDefault()

            ' Phần chung 
            Dim iQ701, iQ7011, iQ7012, iQ7013, iQ7014, iQ7051, iQ7052, iQ70611, iQ70612, iQ706212,
                   iQ706222, iQ706232, iQ710111, iQ710112, iQ710113, iQ710114, iQ7054, iQ7055, iQ7056, iQ7057, iQ7058, iQ7059,
                   iQ711, iQ7111, iQ7112, iQ7113, iQ7114, iQ7116, iQ7117, iQ7118, iQ71118, iCodeQ7121, iQ7122, iQ71221,
                   iCodeQ7141, iQ71421, iQ71422, iQ7151, iQ7152, iQ715211, iQ715212, iQ715213 As Integer

            iQ701 = GetNumberByFormat(txtQ701.Text.Trim())
            iQ7011 = GetNumberByFormat(txtQ7011.Text.Trim())
            iQ7012 = GetNumberByFormat(txtQ7012.Text.Trim())
            iQ7013 = GetNumberByFormat(txtQ7013.Text.Trim())
            iQ7014 = GetNumberByFormat(txtQ7014.Text.Trim())
            iQ7051 = GetNumberByFormat(txtQ7051.Text.Trim())
            iQ7052 = GetNumberByFormat(txtQ7052.Text.Trim())
            iQ70611 = GetNumberByFormat(txtQ70611.Text.Trim())
            iQ70612 = GetNumberByFormat(txtQ70612.Text.Trim())
            iQ706212 = GetNumberByFormat(txtQ706212.Text.Trim())
            iQ706222 = GetNumberByFormat(txtQ706222.Text.Trim())
            iQ706232 = GetNumberByFormat(txtQ706232.Text.Trim())

            iQ710111 = GetNumberByFormat(txtQ710111.Text.Trim())
            iQ710112 = GetNumberByFormat(txtQ710112.Text.Trim())
            iQ710113 = GetNumberByFormat(txtQ710113.Text.Trim())
            iQ710114 = GetNumberByFormat(txtQ710114.Text.Trim())

            iQ711 = GetNumberByFormat(txtQ711.Text.Trim())
            iQ7111 = GetNumberByFormat(txtQ7111.Text.Trim())
            iQ7112 = GetNumberByFormat(txtQ7112.Text.Trim())
            iQ7113 = GetNumberByFormat(txtQ7113.Text.Trim())
            iQ7114 = GetNumberByFormat(txtQ7114.Text.Trim())
            iQ7116 = GetNumberByFormat(txtQ7116.Text.Trim())
            iQ7117 = GetNumberByFormat(txtQ7117.Text.Trim())
            iQ7118 = GetNumberByFormat(txtQ7118.Text.Trim())
            iQ71118 = GetNumberByFormat(txtQ71118.Text.Trim())
            iCodeQ7121 = GetNumberByFormat(txtCodeQ7121.Text.Trim())
            iQ7122 = GetNumberByFormat(txtQ7122.Text.Trim())
            iQ71221 = GetNumberByFormat(txtQ71221.Text.Trim())
            iCodeQ7141 = GetNumberByFormat(txtCodeQ7141.Text.Trim())
            iQ71421 = GetNumberByFormat(txtQ71421.Text.Trim())
            iQ71422 = GetNumberByFormat(txtQ71422.Text.Trim())
            iQ7151 = GetNumberByFormat(txtQ7151.Text.Trim())
            iQ7152 = GetNumberByFormat(txtQ7152.Text.Trim())
            iQ715211 = GetNumberByFormat(txtQ715211.Text.Trim())
            iQ715212 = GetNumberByFormat(txtQ715212.Text.Trim())
            iQ715213 = GetNumberByFormat(txtQ715213.Text.Trim())

            iQ7054 = GetNumberByFormat(txtQ7054.Text.Trim())
            iQ7055 = GetNumberByFormat(txtQ7055.Text.Trim())
            iQ7056 = GetNumberByFormat(txtQ7056.Text.Trim())
            iQ7057 = GetNumberByFormat(txtQ7057.Text.Trim())
            iQ7058 = GetNumberByFormat(txtQ7058.Text.Trim())
            iQ7059 = GetNumberByFormat(txtQ7059.Text.Trim())
            Try
                If Not Session("Username") = "" Then
                    If cauhoi7 Is Nothing Then
                        cauhoi7 = New CauHoi7
                        cauhoi7.PhieuId = hidPhieuID.Value

                        '' Phần 7.1 Tổ chức bộ máy làm công tác an toàn
                        cauhoi7.Q701 = IIf(iQ701 > 0, iQ701, Nothing)
                        If iQ701 > 0 Then
                            cauhoi7.Q7011 = iQ7011 'IIf(iQ7011 > 0, iQ7011, Nothing) ' Cán bộ chuyên trách an toàn
                            cauhoi7.Q7015 = IIf(chkQ7015.SelectedValue = "", Nothing, chkQ7015.SelectedValue = "1") 'Hợp đồng với Tổ chức dịch vụ an toàn lao động
                            cauhoi7.Q7012 = iQ7012 'IIf(iQ7012 > 0, iQ7012, Nothing) ' Cán bộ y tế
                            cauhoi7.Q7016 = IIf(chkQ7016.SelectedValue = "", Nothing, chkQ7016.SelectedValue = "1") 'Hợp đồng chăm sóc sức khỏe với cơ sở Y tế địa phương
                            cauhoi7.Q7013 = IIf(iQ7013 > 0, iQ7013, Nothing) ' Mạng lưới an toàn viên
                            cauhoi7.Q7014 = iQ7014 'IIf(iQ7014 > 0, iQ7014, Nothing) ' Hội đồng bảo hộ lao động
                        Else
                            cauhoi7.Q7011 = Nothing
                            cauhoi7.Q7015 = Nothing
                            cauhoi7.Q7012 = Nothing
                            cauhoi7.Q7016 = Nothing
                            cauhoi7.Q7013 = Nothing
                            cauhoi7.Q7014 = Nothing
                        End If

                        ' Phần 7.2 Phân định trách nhiệm về an toàn vệ sinh lao độ
                        cauhoi7.Q702 = IIf(chkQ702.SelectedValue = "", Nothing, chkQ702.SelectedValue = "1")
                        'cauhoi7.Q7021 = IIf(chkQ7021.SelectedValue = "", Nothing, chkQ7021.SelectedValue = "1") 'Thống kê số người làm công việc độc hại
                        cauhoi7.Q7021 = Nothing 'Thống kê số người làm công việc độc hại
                        If chkQ7021.SelectedValue <> "" Then
                            cauhoi7.Q7021 = chkQ7021.SelectedValue
                        End If
                        ' Phần 7.3 Xây dựng kế hoạch công tác an toàn vệ sinh lao động hàng năm                    
                        cauhoi7.Q703 = rdlQ703.SelectedValue '' ichkQ703
                        cauhoi7.Q7031 = IIf(rdlQ703.SelectedValue = 3, txtQ7031.Text.Trim(), Nothing) ' Thiếu nội dung

                        '' Phần 7.4 Xây dựng quy trình, biện pháp an toàn
                        cauhoi7.Q704 = rdlQ704.SelectedValue 'ichkQ704
                        cauhoi7.Q7041 = IIf(rdlQ704.SelectedValue = 3, txtQ7041.Text.Trim(), Nothing) ' thiếu nội dung

                        '' Phần 7.5 Kiểm định thiết bị
                        cauhoi7.Q7051 = IIf(iQ7051 > 0, iQ7051, Nothing) ' Số kiểm định
                        cauhoi7.Q7052 = IIf(iQ7052 > 0, iQ7052, Nothing) ' Số chưa kiểm định
                        cauhoi7.Q7053 = IIf(iQ7052 > 0, txtQ7053.Text.Trim(), Nothing) ' Cụ thể từng loại cho số chưa kiểm định
                        cauhoi7.Q7054 = IIf(iQ7054 > 0, iQ7054, Nothing) ' Số thiết bị nâng
                        cauhoi7.Q7055 = IIf(iQ7055 > 0, iQ7055, Nothing) ' Số thiết bị chịu áp lực
                        cauhoi7.Q7056 = IIf(iQ7056 > 0, iQ7056, Nothing) ' Số Hệ thống lạnh
                        cauhoi7.Q7057 = IIf(iQ7057 > 0, iQ7057, Nothing) ' Số đồng hồ đo nhiệt và đo áp lực
                        cauhoi7.Q7059 = IIf(iQ7059 > 0, iQ7059, Nothing) ' Thiết bị khác
                        cauhoi7.Q7058 = IIf(iQ7058 > 0, iQ7058, Nothing) ' Tổng số thiết bị có yêu cầu nghiêm ngặt

                        '' Phần7.6 Huấn luyện an toàn vệ sinh lao động
                        cauhoi7.Q706 = IIf(chkQ706.SelectedValue = "", Nothing, chkQ706.SelectedValue = "1")
                        If chkQ706.SelectedValue = "1" Then
                            cauhoi7.Q70611 = IIf(iQ70611 > 0, iQ70611, Nothing) ' Cho cán bộ quản lý
                            cauhoi7.Q70612 = IIf(iQ70612 > 0, iQ70612, Nothing) 'Tổng số cán bộ 
                            cauhoi7.Q706211 = chkQ706211.Checked ' Huấn luyện cho lao động lần đầu đủ 
                            If Not chkQ706211.Checked And iQ706212 = 0 Then
                                cauhoi7.Q706211 = Nothing
                            End If
                            cauhoi7.Q706212 = IIf(chkQ706211.Checked Or (Not chkQ706211.Checked And iQ706212 = 0), Nothing, iQ706212) ' Số ld thiếu lần đầu 
                            cauhoi7.Q706221 = chkQ706221.Checked ' Huấn luyện cho lao động lần định kỳ 
                            If Not chkQ706221.Checked And iQ706222 = 0 Then
                                cauhoi7.Q706221 = Nothing
                            End If
                            cauhoi7.Q706222 = IIf(chkQ706221.Checked Or (Not chkQ706221.Checked And iQ706222 = 0), Nothing, iQ706222) ' số ld thiết lần định kỳ
                            cauhoi7.Q706232 = IIf(iQ706232 > 0, iQ706232, Nothing)  ' số ld thiếu cấp thẻ
                        Else
                            cauhoi7.Q70611 = Nothing ' Cho cán bộ quản lý
                            cauhoi7.Q70612 = Nothing 'Tổng số cán bộ 
                            cauhoi7.Q706211 = Nothing ' Huấn luyện cho lao động lần đầu đủ 
                            cauhoi7.Q706212 = Nothing  ' Số ld thiếu lần đầu 
                            cauhoi7.Q706221 = Nothing ' Huấn luyện cho lao động lần định kỳ 
                            cauhoi7.Q706222 = Nothing ' số ld thiết lần định kỳ
                            cauhoi7.Q706232 = Nothing  ' số ld thiếu cấp thẻ
                        End If

                        '' Phần 7.7 Hồ sơ huấn luyện
                        cauhoi7.Q707 = chkQ707.SelectedValue = "1"
                        If chkQ707.SelectedValue = "0" Then ' Không đầy đủ
                            cauhoi7.Q7071 = chkQ7071.Checked ' Thiếu tài liệu
                            cauhoi7.Q7072 = chkQ7072.Checked 'thiếu sổ theo dõi
                            cauhoi7.Q7073 = chkQ7073.Checked 'thiếu bài kiểm trả
                        ElseIf chkQ707.SelectedValue = "1" Then
                            cauhoi7.Q7071 = False ' Thiếu tài liệu
                            cauhoi7.Q7072 = False 'thiếu sổ theo dõi
                            cauhoi7.Q7073 = False
                        ElseIf chkQ707.SelectedValue = "" Then
                            cauhoi7.Q707 = Nothing
                            cauhoi7.Q7071 = Nothing ' Thiếu tài liệu
                            cauhoi7.Q7072 = Nothing 'thiếu sổ theo dõi
                            cauhoi7.Q7073 = Nothing
                        End If

                        '' Phần 7.8 Nội dung huấn luyện
                        cauhoi7.Q708 = chkQ708.SelectedValue = "1"
                        If chkQ708.SelectedValue = "0" Then 'Không đầy đủ 
                            cauhoi7.Q7081 = chkQ7081.Checked 'Thiếu vệ sinh lao động
                            cauhoi7.Q7082 = chkQ7082.Checked 'Thiếu cấp cứu tai nạn lao động  
                            cauhoi7.Q7083 = chkQ7083.Checked 'Thiếu loại tài liệu: quy trình, biện pháp an toàn 
                        ElseIf chkQ708.SelectedValue = "1" Then
                            cauhoi7.Q7081 = False 'Thiếu loại tài liệu: quy trình, biện pháp an toàn 
                            cauhoi7.Q7082 = False ' Thiếu vệ sinh lao động  
                            cauhoi7.Q7083 = False 'Thiếu cấp cứu tai nạn lao động
                        ElseIf chkQ708.SelectedValue = "" Then
                            cauhoi7.Q708 = Nothing
                            cauhoi7.Q7081 = Nothing 'Thiếu loại tài liệu: quy trình, biện pháp an toàn 
                            cauhoi7.Q7082 = Nothing ' Thiếu vệ sinh lao động  
                            cauhoi7.Q7083 = Nothing 'Thiếu cấp cứu tai nạn lao động
                        End If

                        '' Phần 7.9 Trang bị phương tiện bảo vệ cá nhân theo danh mục nghề
                        cauhoi7.Q709 = chkQ709.SelectedValue = "1"
                        If chkQ709.SelectedValue = "0" Then
                            cauhoi7.Q7091 = txtQ7091.Text.Trim() ' Thiếu loại gì?
                            cauhoi7.Q70911 = txtQ70911.Text.Trim() ' cho chức danh nghề nghiệp gì 
                        ElseIf chkQ709.SelectedValue = "1" Then
                            cauhoi7.Q7091 = "" ' Thiếu loại gì?
                            cauhoi7.Q70911 = "" ' cho chức danh nghề nghiệp gì 
                        ElseIf chkQ709.SelectedValue = "" Then
                            cauhoi7.Q709 = Nothing
                            cauhoi7.Q7091 = "" ' Thiếu loại gì?
                            cauhoi7.Q70911 = "" ' cho chức danh nghề nghiệp gì 
                        End If

                        '' Phần 7.10 Thực hiện bồi dưỡng cho người lao động làm các công việc độc hại, nguy hiểm và đặc biệt độc hại, nguy hiểm
                        cauhoi7.Q71011 = Nothing
                        If chkQ71011.SelectedValue <> "" Then
                            If chkQ71011.SelectedValue = "0" Then
                                cauhoi7.Q71011 = False
                                cauhoi7.Q710111 = IIf(iQ710111 > 0, iQ710111, Nothing)
                                cauhoi7.Q710112 = IIf(iQ710112 > 0, iQ710112, Nothing)
                                cauhoi7.Q710113 = IIf(iQ710113 > 0, iQ710113, Nothing)
                                cauhoi7.Q710114 = IIf(iQ710114 > 0, iQ710114, Nothing)
                                cauhoi7.Q710115 = IIf(String.IsNullOrEmpty(txtQ710115.Text) = True, Nothing, txtQ710115.Text.Trim())
                            Else
                                cauhoi7.Q71011 = True
                                cauhoi7.Q710111 = Nothing
                                cauhoi7.Q710112 = Nothing
                                cauhoi7.Q710113 = Nothing
                                cauhoi7.Q710114 = Nothing
                                cauhoi7.Q710115 = Nothing
                            End If
                        End If
                        cauhoi7.Q71012 = Nothing
                        If chkQ71012.SelectedValue <> "" Then
                            cauhoi7.Q71012 = chkQ71012.SelectedValue = "1" 'Bồi dưỡng bằng tiền
                        End If

                        '' Phần 7.11 Tổng số vụ tai nạn lao động
                        cauhoi7.Q711 = iQ711
                        If iQ711 > 0 Then
                            cauhoi7.Q7111 = iQ7111 ' Số người chết   
                            cauhoi7.Q7112 = iQ7112 ' Số người bị thương nặng
                            cauhoi7.Q7116 = iQ7116 ' Số người bị thương nhẹ
                            cauhoi7.Q7113 = iQ7113 ' khai báo, điều tra.. vụ
                            cauhoi7.Q7114 = iQ7114 '' Giải quyết chết độ... người
                            cauhoi7.Q7119 = IIf(chkQ7119.SelectedValue = "", Nothing, chkQ7119.SelectedValue = "1") 'Tiền lương trong thời gian điều trị
                            cauhoi7.Q71110 = IIf(chkQ71110.SelectedValue = "", Nothing, chkQ71110.SelectedValue = "1") 'Bồi thường, trợ cấp
                            cauhoi7.Q71111 = IIf(chkQ71111.SelectedValue = "", Nothing, chkQ71111.SelectedValue = "1") ' Bố trí làm việc phù hợp
                            cauhoi7.Q71112 = IIf(chkQ71112.SelectedValue = "", Nothing, chkQ71112.SelectedValue = "1") 'thanh toán chi phí Y tế
                            cauhoi7.Q7115 = IIf(chkQ7115.SelectedValue = "", Nothing, chkQ7115.SelectedValue = "1") ' Xác định rõ nguyên nhân
                            cauhoi7.Q7117 = iQ7117 ' Tổng thiệt hại
                            cauhoi7.Q7118 = iQ7118 ' Tổng số người bị tai nạn
                            cauhoi7.Q71118 = iQ71118 ' chưa khai báo, điều tra.. vụ
                        Else
                            cauhoi7.Q711 = Nothing
                            cauhoi7.Q7111 = Nothing ' Số người chết   
                            cauhoi7.Q7112 = Nothing ' Số người bị thương nặng
                            cauhoi7.Q7116 = Nothing ' Số người bị thương nhẹ
                            cauhoi7.Q7113 = Nothing ' khai báo, điều tra.. vụ
                            cauhoi7.Q7114 = Nothing '' Giải quyết chết độ... người
                            cauhoi7.Q7119 = Nothing 'Tiền lương trong thời gian điều trị
                            cauhoi7.Q71110 = Nothing 'Bồi thường, trợ cấp
                            cauhoi7.Q71111 = Nothing ' Bố trí làm việc phù hợp
                            cauhoi7.Q71112 = Nothing 'thanh toán chi phí Y tế
                            cauhoi7.Q7115 = Nothing ' Xác định rõ nguyên nhân
                            cauhoi7.Q7117 = Nothing ' Tổng thiệt hại
                            cauhoi7.Q7118 = Nothing ' Tổng số người bị tai nạn
                            cauhoi7.Q71118 = Nothing ' chưa khai báo, điều tra.. vụ
                        End If

                        '' Phần 7.12 Đo đạc, kiểm tra môi trường tại nơi làm việc
                        cauhoi7.Q7121 = iCodeQ7121 ' Năm đo gần nhất
                        If iCodeQ7121 > 0 Then
                            cauhoi7.Q7122 = iQ7122 ' Số mẫu đã đo
                            If iQ7122 > 0 Then
                                cauhoi7.Q71221 = iQ71221 ' số mẫu không đạt
                                cauhoi7.Q71222 = txtQ71222.Text.Trim() 'Các yếu tố độc hại vượt tiêu chuẩn cho phép 
                            Else
                                cauhoi7.Q71221 = Nothing ' số mẫu không đạt
                                cauhoi7.Q71222 = Nothing 'Các yếu tố độc hại vượt tiêu chuẩn cho phép 
                            End If
                        Else
                            cauhoi7.Q7121 = Nothing
                            cauhoi7.Q7122 = Nothing ' Số mẫu đã đo
                            cauhoi7.Q71221 = Nothing ' số mẫu không đạt
                            cauhoi7.Q71222 = Nothing 'Các yếu tố độc hại vượt tiêu chuẩn cho phép 
                        End If


                        '' Phần 7.13 Các biện pháp kỹ thuật nhằm cải thiện điều kiện làm việc

                        cauhoi7.Q713 = Nothing
                        cauhoi7.Q7131 = Nothing
                        cauhoi7.Q7132 = Nothing
                        If chkQ713.SelectedValue <> "" Then
                            If (chkQ713.SelectedValue = 0 Or chkQ713.SelectedValue = 1) Then
                                cauhoi7.Q713 = chkQ713.SelectedValue = 1
                            End If
                            cauhoi7.Q7131 = txtQ7131.Text.Trim() 'Đã thực hiện
                            cauhoi7.Q7132 = txtQ7132.Text.Trim() ' Chưa thực hiện
                        End If
                        '' Phần 7.14  Khám sức khỏe định kỳ cho người lao động
                        cauhoi7.Q7141 = IIf(iCodeQ7141 > 0, iCodeQ7141, Nothing) ' năm gần nhất
                        cauhoi7.Q71421 = IIf(iQ71421 > 0, iQ71421, Nothing) 'số người được khám 
                        cauhoi7.Q71422 = IIf(iQ71422 > 0, iQ71422, Nothing) ' Số người chưa được khám 
                        cauhoi7.Q7143 = IIf(chkQ7143.SelectedValue = "", Nothing, chkQ7143.SelectedValue = "1") ' H/S quản lý sức khỏe 
                        cauhoi7.Q7144 = IIf(chkQ7144.SelectedValue = "", Nothing, chkQ7144.SelectedValue = "1") ' H/S khám sức khỏe tuyển dụng 

                        '' Phân 7.15 Khám phát hiện bệnh nghề nghiệp hàng năm cho người lao động
                        cauhoi7.Q7151 = iQ7151 ' số người được khám
                        If iQ7151 > 0 Then
                            cauhoi7.Q71511 = txtQ71511.Text.Trim() ' Các loại bệnh
                        Else
                            cauhoi7.Q7151 = Nothing
                            cauhoi7.Q71511 = Nothing ' Các loại bệnh
                        End If
                        cauhoi7.Q7152 = iQ7152 ' Số người mắc bệnh nghề nghiệp
                        If iQ7152 > 0 Then
                            cauhoi7.Q71521 = txtQ71521.Text.Trim() ' Loại bệnh nghề nghiệp
                            cauhoi7.Q715211 = iQ715211 ' số người được giám định, điều trị
                            cauhoi7.Q715212 = iQ715212 ' số người được cấp sổ
                            cauhoi7.Q715213 = iQ715213 ' số ngươid được chuyển công việc khác
                        Else
                            cauhoi7.Q7152 = Nothing ' Số người mắc bệnh nghề nghiệp
                            cauhoi7.Q71521 = Nothing ' Loại bệnh nghề nghiệp
                            cauhoi7.Q715211 = Nothing ' số người được giám định, điều trị
                            cauhoi7.Q715212 = Nothing ' số người được cấp sổ
                            cauhoi7.Q715213 = Nothing ' số ngươid được chuyển công việc khác
                        End If


                        '' Phần 7.16 Thành lập đội cấp cứu
                        cauhoi7.Q716 = IIf(chkQ716.SelectedValue = "", Nothing, chkQ716.SelectedValue = "1")
                        cauhoi7.Q7161 = IIf(chkQ7161.SelectedValue = "", Nothing, chkQ7161.SelectedValue = "1")  'Trang bị phương tiện, túi thuốc cấp cứu

                        '' Phần 7.17 Kiểm tra thực tế yếu tố nguy hiểm tại nơi làm việc
                        cauhoi7.Q717 = chkQ717.SelectedValue = "1"
                        If chkQ717.SelectedValue = "1" Then
                            cauhoi7.Q7171 = chkQ7171.Checked 'Bộ phận chuyển động không bao che 
                            cauhoi7.Q7172 = chkQ7172.Checked 'Thiếu lan can, rào ngăn  
                            cauhoi7.Q7173 = chkQ7173.Checked 'thiếu biển báo nơi nguy hiểm
                            cauhoi7.Q7174 = chkQ7174.Checked 'Hộp cầu dao điện hở 
                            cauhoi7.Q7175 = chkQ7175.Checked 'Đấu nối điện không đúng quy cách; Thiết bị điện không nối đất; Người lao động không sử dụng phương tiện bảo vệ cá nhân 
                            cauhoi7.Q7176 = txtQ7176.Text.Trim() 'Khác
                            cauhoi7.Q7177 = chkQ7177.Checked 'Thiếu bảng chỉ dẫn an toàn đặt tại nơi làm việc
                            cauhoi7.Q7178 = chkQ7178.Checked 'Người lao động không sử dụng phương tiện bảo vệ cá nhân
                        ElseIf chkQ717.SelectedValue = "" Or chkQ717.SelectedValue = "0" Then
                            cauhoi7.Q717 = Nothing
                            cauhoi7.Q7171 = Nothing 'Bộ phận chuyển động không bao che 
                            cauhoi7.Q7172 = Nothing 'Thiếu lan can, rào ngăn  
                            cauhoi7.Q7173 = Nothing 'thiếu biển báo nơi nguy hiểm
                            cauhoi7.Q7174 = Nothing 'Hộp cầu dao điện hở 
                            cauhoi7.Q7175 = Nothing 'Đấu nối điện không đúng quy cách; Thiết bị điện không nối đất; Người lao động không sử dụng phương tiện bảo vệ cá nhân 
                            cauhoi7.Q7176 = Nothing 'Khác
                            cauhoi7.Q7177 = Nothing 'Thiếu bảng chỉ dẫn an toàn đặt tại nơi làm việc
                            cauhoi7.Q7178 = Nothing 'Người lao động không sử dụng phương tiện bảo vệ cá nhân
                        End If


                        '' Phần 7.18 Tự kiểm tra an toàn vệ sinh lao động
                        cauhoi7.Q718 = IIf(chkQ718.SelectedValue = "", Nothing, chkQ718.SelectedValue = "0")
                        If chkQ718.SelectedValue <> "" Then
                            If Not cauhoi7.Q718 Then
                                cauhoi7.Q7181 = chkQ7181.Checked 'Không thường xuyên 
                                cauhoi7.Q7182 = chkQ7182.Checked 'không ghi sổ kiểm tra
                                cauhoi7.Q7183 = chkQ7183.Checked 'không nghiệm thu an toàn thiết bị, giàn giáo trước khi sử dụng 
                                cauhoi7.Q7185 = chkQ7185.Checked 'Không đo điện trở tiếp đất các thiết bị điện, hệ thống chống sét 
                                cauhoi7.Q7184 = txtQ7184.Text.Trim() ' Khác
                            Else
                                cauhoi7.Q7181 = Nothing 'Không thường xuyên 
                                cauhoi7.Q7182 = Nothing 'không ghi sổ kiểm tra
                                cauhoi7.Q7183 = Nothing 'không nghiệm thu an toàn thiết bị, giàn giáo trước khi sử dụng 
                                cauhoi7.Q7185 = Nothing 'Không đo điện trở tiếp đất các thiết bị điện, hệ thống chống sét 
                                cauhoi7.Q7184 = Nothing ' Khác
                            End If
                        Else
                            cauhoi7.Q7181 = Nothing 'Không thường xuyên 
                            cauhoi7.Q7182 = Nothing 'không ghi sổ kiểm tra
                            cauhoi7.Q7183 = Nothing 'không nghiệm thu an toàn thiết bị, giàn giáo trước khi sử dụng 
                            cauhoi7.Q7185 = Nothing 'Không đo điện trở tiếp đất các thiết bị điện, hệ thống chống sét 
                            cauhoi7.Q7184 = Nothing ' Khác
                        End If
                        ''7.19 Lấy ý kiến đại diện người lao động về công tác an toàn vệ sinh lao động
                        cauhoi7.Q719 = Nothing
                        If chkQ719.SelectedValue <> "" Then
                            cauhoi7.Q719 = chkQ719.SelectedValue = "1"
                        End If
                        cauhoi7.NguoiTao = Session("Username")
                        cauhoi7.NgayTao = Date.Now
                        data.CauHoi7.AddObject(cauhoi7)
                        'Luu cau hoi da tra loi
                        Dim pn = (From a In data.PhieuNhapHeaders Where a.PhieuID = hidPhieuID.Value Select a).FirstOrDefault
                        pn.CauHoiDaTraLoi = pn.CauHoiDaTraLoi & "7;"
                        data.SaveChanges()
                        If hidIsUser.Value = 1 Then
                            Insert_App_Log("Insert  Cauhoi7: PhieuId - " & hidPhieuID.Value, Function_Name.BienBanThanhTra, Audit_Type.Create, Request.ServerVariables("REMOTE_ADDR"), Session("UserName"))
                        ElseIf hidIsUser.Value = 2 Then
                            Insert_App_Log("Insert  Cauhoi7: PhieuId - " & hidPhieuID.Value, Function_Name.PhieuKiemTra, Audit_Type.Create, Request.ServerVariables("REMOTE_ADDR"), Session("UserName"))
                        End If
                        Return True
                    Else

                        '' Phần 7.1 Tổ chức bộ máy làm công tác an toàn
                        cauhoi7.Q701 = IIf(iQ701 > 0, iQ701, Nothing)
                        If iQ701 > 0 Then
                            cauhoi7.Q7011 = iQ7011 'IIf(iQ7011 > 0, iQ7011, Nothing) ' Cán bộ chuyên trách an toàn
                            cauhoi7.Q7015 = IIf(chkQ7015.SelectedValue = "", Nothing, chkQ7015.SelectedValue = "1") 'Hợp đồng với Tổ chức dịch vụ an toàn lao động
                            cauhoi7.Q7012 = iQ7012 'IIf(iQ7012 > 0, iQ7012, Nothing) ' Cán bộ y tế
                            cauhoi7.Q7016 = IIf(chkQ7016.SelectedValue = "", Nothing, chkQ7016.SelectedValue = "1") 'Hợp đồng chăm sóc sức khỏe với cơ sở Y tế địa phương
                            cauhoi7.Q7013 = IIf(iQ7013 > 0, iQ7013, Nothing) ' Mạng lưới an toàn viên
                            cauhoi7.Q7014 = iQ7014 'IIf(iQ7014 > 0, iQ7014, Nothing) ' Hội đồng bảo hộ lao động
                        Else
                            cauhoi7.Q7011 = Nothing
                            cauhoi7.Q7015 = Nothing
                            cauhoi7.Q7012 = Nothing
                            cauhoi7.Q7016 = Nothing
                            cauhoi7.Q7013 = Nothing
                            cauhoi7.Q7014 = Nothing
                        End If

                        ' Phần 7.2 Phân định trách nhiệm về an toàn vệ sinh lao độ
                        cauhoi7.Q702 = IIf(chkQ702.SelectedValue = "", Nothing, chkQ702.SelectedValue = "1")
                        'cauhoi7.Q7021 = IIf(chkQ7021.SelectedValue = "", Nothing, chkQ7021.SelectedValue = "1") 'Thống kê số người làm công việc độc hại
                        cauhoi7.Q7021 = Nothing 'Thống kê số người làm công việc độc hại
                        If chkQ7021.SelectedValue <> "" Then
                            cauhoi7.Q7021 = chkQ7021.SelectedValue
                        End If
                        ' Phần 7.3 Xây dựng kế hoạch công tác an toàn vệ sinh lao động hàng năm                    
                        cauhoi7.Q703 = rdlQ703.SelectedValue '' ichkQ703
                        cauhoi7.Q7031 = IIf(rdlQ703.SelectedValue = 3, txtQ7031.Text.Trim(), Nothing) ' Thiếu nội dung

                        '' Phần 7.4 Xây dựng quy trình, biện pháp an toàn
                        cauhoi7.Q704 = rdlQ704.SelectedValue 'ichkQ704
                        cauhoi7.Q7041 = IIf(rdlQ704.SelectedValue = 3, txtQ7041.Text.Trim(), Nothing) ' thiếu nội dung

                        '' Phần 7.5 Kiểm định thiết bị
                        cauhoi7.Q7051 = IIf(iQ7051 > 0, iQ7051, Nothing) ' Số kiểm định
                        cauhoi7.Q7052 = IIf(iQ7052 > 0, iQ7052, Nothing) ' Số chưa kiểm định
                        cauhoi7.Q7053 = IIf(iQ7052 > 0, txtQ7053.Text.Trim(), Nothing) ' Cụ thể từng loại cho số chưa kiểm định
                        cauhoi7.Q7054 = IIf(iQ7054 > 0, iQ7054, Nothing) ' Số thiết bị nâng
                        cauhoi7.Q7055 = IIf(iQ7055 > 0, iQ7055, Nothing) ' Số thiết bị chịu áp lực
                        cauhoi7.Q7056 = IIf(iQ7056 > 0, iQ7056, Nothing) ' Số Hệ thống lạnh
                        cauhoi7.Q7057 = IIf(iQ7057 > 0, iQ7057, Nothing) ' Số đồng hồ đo nhiệt và đo áp lực
                        cauhoi7.Q7059 = IIf(iQ7059 > 0, iQ7059, Nothing) ' Thiết bị khác
                        cauhoi7.Q7058 = IIf(iQ7058 > 0, iQ7058, Nothing) ' Tổng số thiết bị có yêu cầu nghiêm ngặt

                        '' Phần7.6 Huấn luyện an toàn vệ sinh lao động
                        cauhoi7.Q706 = IIf(chkQ706.SelectedValue = "", Nothing, chkQ706.SelectedValue = "1")
                        If chkQ706.SelectedValue = "1" Then
                            cauhoi7.Q70611 = IIf(iQ70611 > 0, iQ70611, Nothing) ' Cho cán bộ quản lý
                            cauhoi7.Q70612 = IIf(iQ70612 > 0, iQ70612, Nothing) 'Tổng số cán bộ 
                            cauhoi7.Q706211 = chkQ706211.Checked ' Huấn luyện cho lao động lần đầu đủ 
                            If Not chkQ706211.Checked And iQ706212 = 0 Then
                                cauhoi7.Q706211 = Nothing
                            End If
                            cauhoi7.Q706212 = IIf(chkQ706211.Checked Or (Not chkQ706211.Checked And iQ706212 = 0), Nothing, iQ706212) ' Số ld thiếu lần đầu 
                            cauhoi7.Q706221 = chkQ706221.Checked ' Huấn luyện cho lao động lần định kỳ 
                            If Not chkQ706221.Checked And iQ706222 = 0 Then
                                cauhoi7.Q706221 = Nothing
                            End If
                            cauhoi7.Q706222 = IIf(chkQ706221.Checked Or (Not chkQ706221.Checked And iQ706222 = 0), Nothing, iQ706222) ' số ld thiết lần định kỳ
                            cauhoi7.Q706232 = IIf(iQ706232 > 0, iQ706232, Nothing)  ' số ld thiếu cấp thẻ
                        Else
                            cauhoi7.Q70611 = Nothing ' Cho cán bộ quản lý
                            cauhoi7.Q70612 = Nothing 'Tổng số cán bộ 
                            cauhoi7.Q706211 = Nothing ' Huấn luyện cho lao động lần đầu đủ 
                            cauhoi7.Q706212 = Nothing  ' Số ld thiếu lần đầu 
                            cauhoi7.Q706221 = Nothing ' Huấn luyện cho lao động lần định kỳ 
                            cauhoi7.Q706222 = Nothing ' số ld thiết lần định kỳ
                            cauhoi7.Q706232 = Nothing  ' số ld thiếu cấp thẻ
                        End If

                        '' Phần 7.7 Hồ sơ huấn luyện
                        cauhoi7.Q707 = chkQ707.SelectedValue = "1"
                        If chkQ707.SelectedValue = "0" Then ' Không đầy đủ
                            cauhoi7.Q7071 = chkQ7071.Checked ' Thiếu tài liệu
                            cauhoi7.Q7072 = chkQ7072.Checked 'thiếu sổ theo dõi
                            cauhoi7.Q7073 = chkQ7073.Checked 'thiếu bài kiểm trả
                        ElseIf chkQ707.SelectedValue = "1" Then
                            cauhoi7.Q7071 = False ' Thiếu tài liệu
                            cauhoi7.Q7072 = False 'thiếu sổ theo dõi
                            cauhoi7.Q7073 = False
                        ElseIf chkQ707.SelectedValue = "" Then
                            cauhoi7.Q707 = Nothing
                            cauhoi7.Q7071 = Nothing ' Thiếu tài liệu
                            cauhoi7.Q7072 = Nothing 'thiếu sổ theo dõi
                            cauhoi7.Q7073 = Nothing
                        End If

                        '' Phần 7.8 Nội dung huấn luyện
                        cauhoi7.Q708 = chkQ708.SelectedValue = "1"
                        If chkQ708.SelectedValue = "0" Then 'Không đầy đủ
                            cauhoi7.Q7081 = chkQ7081.Checked 'Thiếu loại tài liệu: quy trình, biện pháp an toàn 
                            cauhoi7.Q7082 = chkQ7082.Checked ' Thiếu vệ sinh lao động  
                            cauhoi7.Q7083 = chkQ7083.Checked 'Thiếu cấp cứu tai nạn lao động
                        ElseIf chkQ708.SelectedValue = "1" Then
                            cauhoi7.Q7081 = False 'Thiếu loại tài liệu: quy trình, biện pháp an toàn 
                            cauhoi7.Q7082 = False ' Thiếu vệ sinh lao động  
                            cauhoi7.Q7083 = False 'Thiếu cấp cứu tai nạn lao động
                        ElseIf chkQ708.SelectedValue = "" Then
                            cauhoi7.Q708 = Nothing
                            cauhoi7.Q7081 = Nothing 'Thiếu loại tài liệu: quy trình, biện pháp an toàn 
                            cauhoi7.Q7082 = Nothing ' Thiếu vệ sinh lao động  
                            cauhoi7.Q7083 = Nothing 'Thiếu cấp cứu tai nạn lao động
                        End If

                        '' Phần 7.9 Trang bị phương tiện bảo vệ cá nhân theo danh mục nghề
                        cauhoi7.Q709 = chkQ709.SelectedValue = "1"
                        If chkQ709.SelectedValue = "0" Then
                            cauhoi7.Q7091 = txtQ7091.Text.Trim() ' Thiếu loại gì?
                            cauhoi7.Q70911 = txtQ70911.Text.Trim() ' cho chức danh nghề nghiệp gì 
                        ElseIf chkQ709.SelectedValue = "1" Then
                            cauhoi7.Q7091 = "" ' Thiếu loại gì?
                            cauhoi7.Q70911 = "" ' cho chức danh nghề nghiệp gì 
                        ElseIf chkQ709.SelectedValue = "" Then
                            cauhoi7.Q709 = Nothing
                            cauhoi7.Q7091 = "" ' Thiếu loại gì?
                            cauhoi7.Q70911 = "" ' cho chức danh nghề nghiệp gì 
                        End If

                        '' Phần 7.10 Thực hiện bồi dưỡng cho người lao động làm các công việc độc hại, nguy hiểm và đặc biệt độc hại, nguy hiểm
                        cauhoi7.Q71011 = Nothing
                        If chkQ71011.SelectedValue <> "" Then
                            If chkQ71011.SelectedValue = "0" Then
                                cauhoi7.Q71011 = False
                                cauhoi7.Q710111 = IIf(iQ710111 > 0, iQ710111, Nothing)
                                cauhoi7.Q710112 = IIf(iQ710112 > 0, iQ710112, Nothing)
                                cauhoi7.Q710113 = IIf(iQ710113 > 0, iQ710113, Nothing)
                                cauhoi7.Q710114 = IIf(iQ710114 > 0, iQ710114, Nothing)
                                cauhoi7.Q710115 = IIf(String.IsNullOrEmpty(txtQ710115.Text) = True, Nothing, txtQ710115.Text.Trim())
                            Else
                                cauhoi7.Q71011 = True
                                cauhoi7.Q710111 = Nothing
                                cauhoi7.Q710112 = Nothing
                                cauhoi7.Q710113 = Nothing
                                cauhoi7.Q710114 = Nothing
                                cauhoi7.Q710115 = Nothing
                            End If
                        End If
                        cauhoi7.Q71012 = Nothing
                        If chkQ71012.SelectedValue <> "" Then
                            cauhoi7.Q71012 = chkQ71012.SelectedValue = "1" 'Bồi dưỡng bằng tiền
                        End If

                        '' Phần 7.11 Tổng số vụ tai nạn lao động
                        cauhoi7.Q711 = iQ711
                        If iQ711 > 0 Then
                            cauhoi7.Q7111 = iQ7111 ' Số người chết   
                            cauhoi7.Q7112 = iQ7112 ' Số người bị thương nặng
                            cauhoi7.Q7116 = iQ7116 ' Số người bị thương nhẹ
                            cauhoi7.Q7113 = iQ7113 ' khai báo, điều tra.. vụ
                            cauhoi7.Q7114 = iQ7114 '' Giải quyết chết độ... người
                            cauhoi7.Q7119 = IIf(chkQ7119.SelectedValue = "", Nothing, chkQ7119.SelectedValue = "1") 'Tiền lương trong thời gian điều trị
                            cauhoi7.Q71110 = IIf(chkQ71110.SelectedValue = "", Nothing, chkQ71110.SelectedValue = "1") 'Bồi thường, trợ cấp
                            cauhoi7.Q71111 = IIf(chkQ71111.SelectedValue = "", Nothing, chkQ71111.SelectedValue = "1") ' Bố trí làm việc phù hợp
                            cauhoi7.Q71112 = IIf(chkQ71112.SelectedValue = "", Nothing, chkQ71112.SelectedValue = "1") 'thanh toán chi phí Y tế
                            cauhoi7.Q7115 = IIf(chkQ7115.SelectedValue = "", Nothing, chkQ7115.SelectedValue = "1") ' Xác định rõ nguyên nhân
                            cauhoi7.Q7117 = iQ7117 ' Tổng thiệt hại
                            cauhoi7.Q7118 = iQ7118 ' Tổng số người bị tai nạn
                            cauhoi7.Q71118 = iQ71118 ' chưa khai báo, điều tra.. vụ
                        Else
                            cauhoi7.Q711 = Nothing
                            cauhoi7.Q7111 = Nothing ' Số người chết   
                            cauhoi7.Q7112 = Nothing ' Số người bị thương nặng
                            cauhoi7.Q7116 = Nothing ' Số người bị thương nhẹ
                            cauhoi7.Q7113 = Nothing ' khai báo, điều tra.. vụ
                            cauhoi7.Q7114 = Nothing '' Giải quyết chết độ... người
                            cauhoi7.Q7119 = Nothing 'Tiền lương trong thời gian điều trị
                            cauhoi7.Q71110 = Nothing 'Bồi thường, trợ cấp
                            cauhoi7.Q71111 = Nothing ' Bố trí làm việc phù hợp
                            cauhoi7.Q71112 = Nothing 'thanh toán chi phí Y tế
                            cauhoi7.Q7115 = Nothing ' Xác định rõ nguyên nhân
                            cauhoi7.Q7117 = Nothing ' Tổng thiệt hại
                            cauhoi7.Q7118 = Nothing ' Tổng số người bị tai nạn
                            cauhoi7.Q71118 = Nothing ' chưa khai báo, điều tra.. vụ
                        End If

                        '' Phần 7.12 Đo đạc, kiểm tra môi trường tại nơi làm việc
                        cauhoi7.Q7121 = iCodeQ7121 ' Năm đo gần nhất
                        If iCodeQ7121 > 0 Then
                            cauhoi7.Q7122 = iQ7122 ' Số mẫu đã đo
                            If iQ7122 > 0 Then
                                cauhoi7.Q71221 = iQ71221 ' số mẫu không đạt
                                cauhoi7.Q71222 = txtQ71222.Text.Trim() 'Các yếu tố độc hại vượt tiêu chuẩn cho phép 
                            Else
                                cauhoi7.Q71221 = Nothing ' số mẫu không đạt
                                cauhoi7.Q71222 = Nothing 'Các yếu tố độc hại vượt tiêu chuẩn cho phép 
                            End If
                        Else
                            cauhoi7.Q7121 = Nothing
                            cauhoi7.Q7122 = Nothing ' Số mẫu đã đo
                            cauhoi7.Q71221 = Nothing ' số mẫu không đạt
                            cauhoi7.Q71222 = Nothing 'Các yếu tố độc hại vượt tiêu chuẩn cho phép 
                        End If

                        '' Phần 7.13 Các biện pháp kỹ thuật nhằm cải thiện điều kiện làm việc

                        cauhoi7.Q713 = Nothing
                        cauhoi7.Q7131 = Nothing
                        cauhoi7.Q7132 = Nothing
                        If chkQ713.SelectedValue <> "" Then
                            If (chkQ713.SelectedValue = 0 Or chkQ713.SelectedValue = 1) Then
                                cauhoi7.Q713 = chkQ713.SelectedValue = 1
                            End If
                            cauhoi7.Q7131 = txtQ7131.Text.Trim() 'Đã thực hiện
                            cauhoi7.Q7132 = txtQ7132.Text.Trim() ' Chưa thực hiện
                        End If
                        '' Phần 7.14  Khám sức khỏe định kỳ cho người lao động
                        cauhoi7.Q7141 = IIf(iCodeQ7141 > 0, iCodeQ7141, Nothing) ' năm gần nhất
                        cauhoi7.Q71421 = IIf(iQ71421 > 0, iQ71421, Nothing) 'số người được khám 
                        cauhoi7.Q71422 = IIf(iQ71422 > 0, iQ71422, Nothing) ' Số người chưa được khám 
                        cauhoi7.Q7143 = IIf(chkQ7143.SelectedValue = "", Nothing, chkQ7143.SelectedValue = "1") ' H/S quản lý sức khỏe 
                        cauhoi7.Q7144 = IIf(chkQ7144.SelectedValue = "", Nothing, chkQ7144.SelectedValue = "1") ' H/S khám sức khỏe tuyển dụng 

                        '' Phân 7.15 Khám phát hiện bệnh nghề nghiệp hàng năm cho người lao động
                        cauhoi7.Q7151 = iQ7151 ' số người được khám
                        If iQ7151 > 0 Then
                            cauhoi7.Q71511 = txtQ71511.Text.Trim() ' Các loại bệnh
                        Else
                            cauhoi7.Q7151 = Nothing
                            cauhoi7.Q71511 = Nothing ' Các loại bệnh
                        End If
                        cauhoi7.Q7152 = iQ7152 ' Số người mắc bệnh nghề nghiệp
                        If iQ7152 > 0 Then
                            cauhoi7.Q71521 = txtQ71521.Text.Trim() ' Loại bệnh nghề nghiệp
                            cauhoi7.Q715211 = iQ715211 ' số người được giám định, điều trị
                            cauhoi7.Q715212 = iQ715212 ' số người được cấp sổ
                            cauhoi7.Q715213 = iQ715213 ' số ngươid được chuyển công việc khác
                        Else
                            cauhoi7.Q7152 = Nothing ' Số người mắc bệnh nghề nghiệp
                            cauhoi7.Q71521 = Nothing ' Loại bệnh nghề nghiệp
                            cauhoi7.Q715211 = Nothing ' số người được giám định, điều trị
                            cauhoi7.Q715212 = Nothing ' số người được cấp sổ
                            cauhoi7.Q715213 = Nothing ' số ngươid được chuyển công việc khác
                        End If


                        '' Phần 7.16 Thành lập đội cấp cứu
                        cauhoi7.Q716 = IIf(chkQ716.SelectedValue = "", Nothing, chkQ716.SelectedValue = "1")
                        cauhoi7.Q7161 = IIf(chkQ7161.SelectedValue = "", Nothing, chkQ7161.SelectedValue = "1")  'Trang bị phương tiện, túi thuốc cấp cứu

                        '' Phần 7.17 Kiểm tra thực tế yếu tố nguy hiểm tại nơi làm việc
                        cauhoi7.Q717 = chkQ717.SelectedValue = "1"
                        If chkQ717.SelectedValue = "1" Then
                            cauhoi7.Q7171 = chkQ7171.Checked 'Bộ phận chuyển động không bao che 
                            cauhoi7.Q7172 = chkQ7172.Checked 'Thiếu lan can, rào ngăn  
                            cauhoi7.Q7173 = chkQ7173.Checked 'thiếu biển báo nơi nguy hiểm
                            cauhoi7.Q7174 = chkQ7174.Checked 'Hộp cầu dao điện hở 
                            cauhoi7.Q7175 = chkQ7175.Checked 'Đấu nối điện không đúng quy cách; Thiết bị điện không nối đất; Người lao động không sử dụng phương tiện bảo vệ cá nhân 
                            cauhoi7.Q7176 = txtQ7176.Text.Trim() 'Khác
                            cauhoi7.Q7177 = chkQ7177.Checked 'Thiếu bảng chỉ dẫn an toàn đặt tại nơi làm việc
                            cauhoi7.Q7178 = chkQ7178.Checked 'Người lao động không sử dụng phương tiện bảo vệ cá nhân
                        ElseIf chkQ717.SelectedValue = "" Or chkQ717.SelectedValue = "0" Then
                            cauhoi7.Q717 = Nothing
                            cauhoi7.Q7171 = Nothing 'Bộ phận chuyển động không bao che 
                            cauhoi7.Q7172 = Nothing 'Thiếu lan can, rào ngăn  
                            cauhoi7.Q7173 = Nothing 'thiếu biển báo nơi nguy hiểm
                            cauhoi7.Q7174 = Nothing 'Hộp cầu dao điện hở 
                            cauhoi7.Q7175 = Nothing 'Đấu nối điện không đúng quy cách; Thiết bị điện không nối đất; Người lao động không sử dụng phương tiện bảo vệ cá nhân 
                            cauhoi7.Q7176 = Nothing 'Khác
                            cauhoi7.Q7177 = Nothing 'Thiếu bảng chỉ dẫn an toàn đặt tại nơi làm việc
                            cauhoi7.Q7178 = Nothing 'Người lao động không sử dụng phương tiện bảo vệ cá nhân
                        End If


                        '' Phần 7.18 Tự kiểm tra an toàn vệ sinh lao động
                        cauhoi7.Q718 = IIf(chkQ718.SelectedValue = "", Nothing, chkQ718.SelectedValue = "0")
                        If chkQ718.SelectedValue <> "" Then
                            If Not cauhoi7.Q718 Then
                                cauhoi7.Q7181 = chkQ7181.Checked 'Không thường xuyên 
                                cauhoi7.Q7182 = chkQ7182.Checked 'không ghi sổ kiểm tra
                                cauhoi7.Q7183 = chkQ7183.Checked 'không nghiệm thu an toàn thiết bị, giàn giáo trước khi sử dụng 
                                cauhoi7.Q7185 = chkQ7185.Checked 'Không đo điện trở tiếp đất các thiết bị điện, hệ thống chống sét 
                                cauhoi7.Q7184 = txtQ7184.Text.Trim() ' Khác
                            Else
                                cauhoi7.Q7181 = Nothing 'Không thường xuyên 
                                cauhoi7.Q7182 = Nothing 'không ghi sổ kiểm tra
                                cauhoi7.Q7183 = Nothing 'không nghiệm thu an toàn thiết bị, giàn giáo trước khi sử dụng 
                                cauhoi7.Q7185 = Nothing 'Không đo điện trở tiếp đất các thiết bị điện, hệ thống chống sét 
                                cauhoi7.Q7184 = Nothing ' Khác
                            End If
                        Else
                            cauhoi7.Q7181 = Nothing 'Không thường xuyên 
                            cauhoi7.Q7182 = Nothing 'không ghi sổ kiểm tra
                            cauhoi7.Q7183 = Nothing 'không nghiệm thu an toàn thiết bị, giàn giáo trước khi sử dụng 
                            cauhoi7.Q7185 = Nothing 'Không đo điện trở tiếp đất các thiết bị điện, hệ thống chống sét 
                            cauhoi7.Q7184 = Nothing ' Khác
                        End If
                        ''7.19 Lấy ý kiến đại diện người lao động về công tác an toàn vệ sinh lao động
                        cauhoi7.Q719 = Nothing
                        If chkQ719.SelectedValue <> "" Then
                            cauhoi7.Q719 = chkQ719.SelectedValue = "1"
                        End If

                        cauhoi7.NguoiSua = Session("Username")
                        cauhoi7.NgaySua = Date.Now
                        'Luu ngay sua, nguoi sua phieu
                        Dim pn = (From a In data.PhieuNhapHeaders Where a.PhieuID = hidPhieuID.Value Select a).FirstOrDefault
                        pn.NgaySua = Date.Now
                        pn.NguoiSua = Session("Username")
                        data.SaveChanges()
                        If hidIsUser.Value = 1 Then
                            Insert_App_Log("Update  Cauhoi7: PhieuId - " & hidPhieuID.Value, Function_Name.BienBanThanhTra, Audit_Type.Edit, Request.ServerVariables("REMOTE_ADDR"), Session("UserName"))
                        ElseIf hidIsUser.Value = 2 Then
                            Insert_App_Log("Update  Cauhoi7: PhieuId - " & hidPhieuID.Value, Function_Name.PhieuKiemTra, Audit_Type.Edit, Request.ServerVariables("REMOTE_ADDR"), Session("UserName"))
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
            Dim p As CauHoi7 = (From q In data.CauHoi7 Where q.PhieuId = hidPhieuID.Value Select q).FirstOrDefault
            Dim pn = (From a In data.PhieuNhapHeaders Where a.PhieuID = hidPhieuID.Value).FirstOrDefault()
            If Not IsNothing(pn) Then
                txtSoNguoiPhaiCapTheAT.Text = IIf(IsNothing(pn.SoNguoiLamCongViecYeuCauNghiemNgat) = True, "", pn.SoNguoiLamCongViecYeuCauNghiemNgat)
                hidSoLDLamCVDHNH.Value = IIf(IsNothing(pn.SoNguoiLamNgheNguyHiem) = True, "", pn.SoNguoiLamNgheNguyHiem)
                hidTongLaoDong.Value = IIf(IsNothing(pn.TongSoNhanVien), 0, pn.TongSoNhanVien)
            End If
            If Not p Is Nothing Then
                txtQ701.Text = IIf(IsNothing(p.Q701) = True, "", p.Q701)
                txtQ7011.Text = IIf(IsNothing(p.Q7011) = True, "", p.Q7011)
                If p.Q7015.HasValue Then
                    chkQ7015.SelectedValue = Math.Abs(CInt(p.Q7015))
                Else
                    chkQ7015.ClearSelection()
                End If
                txtQ7012.Text = IIf(IsNothing(p.Q7012) = True, "", p.Q7012)
                If p.Q7016.HasValue Then
                    chkQ7016.SelectedValue = Math.Abs(CInt(p.Q7016))
                Else
                    chkQ7016.ClearSelection()
                End If
                txtQ7013.Text = IIf(IsNothing(p.Q7013) = True, "", p.Q7013)
                txtQ7014.Text = IIf(IsNothing(p.Q7014) = True, "", p.Q7014)
                If p.Q702.HasValue Then
                    chkQ702.SelectedValue = Math.Abs(CInt(p.Q702))
                Else
                    chkQ702.ClearSelection()
                End If
                If p.Q7021.HasValue Then
                    chkQ7021.SelectedValue = p.Q7021
                Else
                    chkQ7021.ClearSelection()
                End If
                If p.Q703.HasValue Then
                    rdlQ703.SelectedValue = p.Q703
                Else
                    rdlQ703.ClearSelection()
                End If

                txtQ7031.Text = IIf(IsNothing(p.Q7031) = True, "", p.Q7031)
                If p.Q704.HasValue Then
                    rdlQ704.SelectedValue = p.Q704
                Else
                    rdlQ704.ClearSelection()
                End If
                txtQ7041.Text = IIf(IsNothing(p.Q7041) = True, "", p.Q7041)
                txtQ7051.Text = IIf(IsNothing(p.Q7051) = True, "", p.Q7051)
                txtQ7052.Text = IIf(IsNothing(p.Q7052) = True, "", p.Q7052)
                txtQ7053.Text = IIf(IsNothing(p.Q7053) = True, "", p.Q7053)

                txtQ7054.Text = IIf(IsNothing(p.Q7054) = True, "", p.Q7054)
                txtQ7055.Text = IIf(IsNothing(p.Q7055) = True, "", p.Q7055)
                txtQ7056.Text = IIf(IsNothing(p.Q7056) = True, "", p.Q7056)
                txtQ7057.Text = IIf(IsNothing(p.Q7057) = True, "", p.Q7057)
                txtQ7059.Text = IIf(IsNothing(p.Q7059) = True, "", p.Q7059)
                txtQ7058.Text = IIf(IsNothing(p.Q7058) = True, "", p.Q7058)

                If p.Q706.HasValue Then
                    chkQ706.SelectedValue = Math.Abs(CInt(p.Q706))
                Else
                    chkQ706.ClearSelection()
                End If
                txtQ70611.Text = IIf(IsNothing(p.Q70611) = True, "", p.Q70611)
                txtQ70612.Text = IIf(IsNothing(p.Q70612) = True, "", p.Q70612)
                chkQ706211.Checked = IIf(IsNothing(p.Q706211) = True, False, p.Q706211)
                txtQ706212.Text = IIf(IsNothing(p.Q706212) = True, "", p.Q706212)
                chkQ706221.Checked = IIf(IsNothing(p.Q706221) = True, False, p.Q706221)
                txtQ706222.Text = IIf(IsNothing(p.Q706222) = True, "", p.Q706222)
                txtQ706232.Text = IIf(IsNothing(p.Q706232) = True, "", p.Q706232)
                If Not IsNothing(pn.SoNguoiLamCongViecYeuCauNghiemNgat) AndAlso pn.SoNguoiLamCongViecYeuCauNghiemNgat = 0 Then
                    txtQ706232.Text = ""
                End If

                If p.Q707.HasValue Then
                    chkQ707.SelectedValue = Math.Abs(CInt(p.Q707))
                Else
                    chkQ707.ClearSelection()
                End If
                chkQ7071.Checked = IIf(IsNothing(p.Q7071) = True, False, p.Q7071)
                chkQ7072.Checked = IIf(IsNothing(p.Q7072) = True, False, p.Q7072)
                chkQ7073.Checked = IIf(IsNothing(p.Q7073) = True, False, p.Q7073)
                If p.Q708.HasValue Then
                    chkQ708.SelectedValue = Math.Abs(CInt(p.Q708))
                Else
                    chkQ708.ClearSelection()
                End If
                chkQ7081.Checked = IIf(IsNothing(p.Q7081) = True, False, p.Q7081)
                chkQ7082.Checked = IIf(IsNothing(p.Q7082) = True, False, p.Q7082)
                chkQ7083.Checked = IIf(IsNothing(p.Q7083) = True, False, p.Q7083)
                If p.Q709.HasValue Then
                    chkQ709.SelectedValue = Math.Abs(CInt(p.Q709))
                Else
                    chkQ709.ClearSelection()
                End If
                txtQ7091.Text = IIf(IsNothing(p.Q7091) = True, "", p.Q7091)
                txtQ70911.Text = IIf(IsNothing(p.Q70911) = True, "", p.Q70911)
                '7.10
                If p.Q71012 Is Nothing Then
                    chkQ71012.ClearSelection()
                Else
                    chkQ71012.SelectedValue = Math.Abs(CInt(p.Q71012))
                End If
                If p.Q71011 Is Nothing Then
                    chkQ71011.ClearSelection()
                Else
                    chkQ71011.SelectedValue = Math.Abs(CInt(p.Q71011))
                End If
                txtQ710111.Text = IIf(IsNothing(p.Q710111) = True, "", p.Q710111)
                txtQ710112.Text = IIf(IsNothing(p.Q710112) = True, "", p.Q710112)
                txtQ710113.Text = IIf(IsNothing(p.Q710113) = True, "", p.Q710113)
                txtQ710114.Text = IIf(IsNothing(p.Q710114) = True, "", p.Q710114)
                txtQ710115.Text = IIf(IsNothing(p.Q710115) = True, "", p.Q710115)
                '7.11
                txtQ711.Text = IIf(IsNothing(p.Q711) = True, "", p.Q711)
                txtQ7111.Text = IIf(IsNothing(p.Q7111) = True, "", p.Q7111)
                txtQ7112.Text = IIf(IsNothing(p.Q7112) = True, "", p.Q7112)
                txtQ7113.Text = IIf(IsNothing(p.Q7113) = True, "", p.Q7113)
                txtQ7114.Text = IIf(IsNothing(p.Q7114) = True, "", p.Q7114)
                If p.Q7119.HasValue Then
                    chkQ7119.SelectedValue = Math.Abs(CInt(p.Q7119))
                Else
                    chkQ7119.ClearSelection()
                End If
                If p.Q71110.HasValue Then
                    chkQ71110.SelectedValue = Math.Abs(CInt(p.Q71110))
                Else
                    chkQ71110.ClearSelection()
                End If
                If p.Q71111.HasValue Then
                    chkQ71111.SelectedValue = Math.Abs(CInt(p.Q71111))
                Else
                    chkQ71111.ClearSelection()
                End If
                If p.Q71112.HasValue Then
                    chkQ71112.SelectedValue = Math.Abs(CInt(p.Q71112))
                Else
                    chkQ71112.ClearSelection()
                End If
                If p.Q7115.HasValue Then
                    chkQ7115.SelectedValue = Math.Abs(CInt(p.Q7115))
                Else
                    chkQ7115.ClearSelection()
                End If

                txtQ7116.Text = IIf(IsNothing(p.Q7116) = True, "", p.Q7116)
                txtQ7117.Text = IIf(IsNothing(p.Q7117) = True, "", p.Q7117)
                txtQ7118.Text = IIf(IsNothing(p.Q7118) = True, "", p.Q7118)
                txtQ71118.Text = IIf(IsNothing(p.Q71118) = True, "", p.Q71118)
                txtCodeQ7121.Text = IIf(IsNothing(p.Q7121) = True, "", p.Q7121)
                txtQ7122.Text = IIf(IsNothing(p.Q7122) = True, "", p.Q7122)
                txtQ71221.Text = IIf(IsNothing(p.Q71221) = True, "", p.Q71221)
                txtQ71222.Text = p.Q71222
                If p.Q713.HasValue Then
                    chkQ713.SelectedValue = Math.Abs(CInt(p.Q713))
                Else
                    chkQ713.ClearSelection()
                End If
                txtQ7131.Text = IIf(IsNothing(p.Q7131) = True, "", p.Q7131)
                txtQ7132.Text = IIf(IsNothing(p.Q7132) = True, "", p.Q7132)
                txtCodeQ7141.Text = IIf(IsNothing(p.Q7141) = True, "", p.Q7141)
                'Số phải khám sức khỏe định kỳ
                Dim q2 As CauHoi2 = (From a In data.CauHoi2 Where a.PhieuId = hidPhieuID.Value Select a).FirstOrDefault
                If Not IsNothing(q2) Then
                    Dim soduockham As Integer
                    Dim KhongPhaiKyHDLD As Integer = 0
                    If pn.DoanhNghiep.LoaiHinhDNId = 1 Then
                        KhongPhaiKyHDLD = IIf(IsNothing(q2.Q2110), 0, q2.Q2110)
                    End If
                    soduockham = IIf(IsNothing(q2.Q218), 0, q2.Q218) - (IIf(IsNothing(q2.Q217), 0, q2.Q217) + IIf(IsNothing(q2.Q219), 0, q2.Q219) + KhongPhaiKyHDLD)
                    txtQ71421.Text = IIf(soduockham > 0, FormatNumber(soduockham), "")
                End If
                'hidSoNguoiDuocKham.Value = IIf(IsNothing(p.Q7141), 0, p.Q7141)
                txtQ71422.Text = IIf(IsNothing(p.Q71422) = True, "", p.Q71422)
                If p.Q7143.HasValue Then
                    chkQ7143.SelectedValue = Math.Abs(CInt(p.Q7143))
                Else
                    chkQ7143.ClearSelection()
                End If
                If p.Q7144.HasValue Then
                    chkQ7144.SelectedValue = Math.Abs(CInt(p.Q7144))
                Else
                    chkQ7144.ClearSelection()
                End If
                txtQ7151.Text = IIf(IsNothing(p.Q7151) = True, "", p.Q7151)
                txtQ71511.Text = IIf(IsNothing(p.Q71511) = True, "", p.Q71511)
                txtQ7152.Text = IIf(IsNothing(p.Q7152) = True, "", p.Q7152)
                txtQ71521.Text = IIf(IsNothing(p.Q71521) = True, "", p.Q71521)
                txtQ715211.Text = IIf(IsNothing(p.Q715211) = True, "", p.Q715211)
                txtQ715212.Text = IIf(IsNothing(p.Q715212) = True, "", p.Q715212)
                txtQ715213.Text = IIf(IsNothing(p.Q715213) = True, "", p.Q715213)
                If p.Q716.HasValue Then
                    chkQ716.SelectedValue = Math.Abs(CInt(p.Q716))
                Else
                    chkQ716.ClearSelection()
                End If
                If p.Q7161.HasValue Then
                    chkQ7161.SelectedValue = Math.Abs(CInt(p.Q7161))
                Else
                    chkQ7161.ClearSelection()
                End If
                If p.Q717.HasValue Then
                    chkQ717.SelectedValue = Math.Abs(CInt(p.Q717))
                Else
                    chkQ717.ClearSelection()
                End If
                chkQ7171.Checked = IIf(IsNothing(p.Q7171) = True, False, p.Q7171)
                chkQ7172.Checked = IIf(IsNothing(p.Q7172) = True, False, p.Q7172)
                chkQ7173.Checked = IIf(IsNothing(p.Q7173) = True, False, p.Q7173)
                chkQ7174.Checked = IIf(IsNothing(p.Q7174) = True, False, p.Q7174)
                chkQ7175.Checked = IIf(IsNothing(p.Q7175) = True, False, p.Q7175)
                txtQ7176.Text = IIf(IsNothing(p.Q7176) = True, "", p.Q7176)
                chkQ7178.Checked = IIf(IsNothing(p.Q7178) = True, False, p.Q7178)
                '7.18
                If p.Q718.HasValue Then
                    chkQ718.SelectedValue = Math.Abs(CInt(Not p.Q718))
                Else
                    chkQ718.ClearSelection()
                End If
                chkQ7181.Checked = IIf(IsNothing(p.Q7181) = True, False, p.Q7181)
                chkQ7182.Checked = IIf(IsNothing(p.Q7182) = True, False, p.Q7182)
                chkQ7183.Checked = IIf(IsNothing(p.Q7183) = True, False, p.Q7183)
                chkQ7185.Checked = IIf(IsNothing(p.Q7185) = True, False, p.Q7185)
                txtQ7184.Text = IIf(IsNothing(p.Q7184) = True, "", p.Q7184)
                '7.19
                If p.Q719.HasValue Then
                    chkQ719.SelectedValue = Math.Abs(CInt(p.Q719))
                Else
                    'chkQ719.ClearSelection()
                End If
            End If
        End Using
    End Sub
    Protected Sub ResetControl()
        txtQ701.Text = ""
        txtQ7011.Text = ""
        chkQ7015.ClearSelection()
        txtQ7012.Text = ""
        chkQ7016.ClearSelection()
        txtQ7013.Text = ""
        txtQ7014.Text = ""
        chkQ702.ClearSelection()
        rdlQ703.ClearSelection()
        txtQ7031.Text = ""
        rdlQ704.ClearSelection()
        txtQ7041.Text = ""
        '7.5
        txtQ7051.Text = ""
        txtQ7052.Text = ""
        txtQ7053.Text = ""
        txtQ7054.Text = ""
        txtQ7055.Text = ""
        txtQ7056.Text = ""
        txtQ7057.Text = ""
        txtQ7059.Text = ""
        txtQ7058.Text = ""
        '7.6
        chkQ706.ClearSelection()
        txtQ70611.Text = ""
        txtQ70612.Text = ""
        chkQ706211.Checked = False
        txtQ706212.Text = ""
        chkQ706221.Checked = False
        txtQ706222.Text = ""
        'chkQ706231.Checked = False
        txtQ706232.Text = ""
        chkQ707.ClearSelection()
        chkQ7071.Checked = False
        chkQ7072.Checked = False
        chkQ7073.Checked = False
        chkQ708.ClearSelection()
        chkQ7081.Checked = False
        chkQ7082.Checked = False
        chkQ7083.Checked = False
        chkQ709.ClearSelection()
        txtQ7091.Text = ""
        txtQ70911.Text = ""
        '7.10
        chkQ71012.ClearSelection()
        chkQ71011.ClearSelection()
        txtQ710111.Text = ""
        txtQ710112.Text = ""
        txtQ710113.Text = ""
        txtQ710114.Text = ""
        txtQ710115.Text = ""
        '7.11
        txtQ711.Text = ""
        txtQ7111.Text = ""
        txtQ7112.Text = ""
        txtQ7113.Text = ""
        txtQ7114.Text = ""
        txtQ7118.Text = ""
        txtQ71118.Text = ""
        chkQ7119.ClearSelection()
        chkQ71110.ClearSelection()
        chkQ71111.ClearSelection()
        chkQ71112.ClearSelection()
        chkQ7115.ClearSelection()
        txtCodeQ7121.Text = ""
        txtQ7122.Text = ""
        txtQ71221.Text = ""
        txtQ71222.Text = ""
        chkQ713.ClearSelection()
        txtQ7131.Text = ""
        txtQ7132.Text = ""
        txtCodeQ7141.Text = ""
        txtQ71421.Text = ""
        txtQ71422.Text = ""
        chkQ7143.ClearSelection()
        chkQ7144.ClearSelection()
        txtQ7151.Text = ""
        txtQ71511.Text = ""
        txtQ7152.Text = ""
        txtQ71521.Text = ""
        txtQ715211.Text = ""
        txtQ715212.Text = ""
        txtQ715213.Text = ""
        chkQ716.ClearSelection()
        chkQ7161.ClearSelection()
        chkQ717.ClearSelection()
        chkQ7171.Checked = False
        chkQ7172.Checked = False
        chkQ7173.Checked = False
        chkQ7174.Checked = False
        chkQ7175.Checked = False
        txtQ7176.Text = ""
        chkQ7178.Checked = False
        chkQ718.ClearSelection()
        chkQ7181.Checked = False
        chkQ7182.Checked = False
        chkQ7183.Checked = False
        chkQ7185.Checked = False
        txtQ7184.Text = ""
        chkQ719.ClearSelection()
        chkQ7021.ClearSelection()
    End Sub
#End Region
#Region "Event for control "
    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Session("phieuid") = hidPhieuID.Value
        Session("IsUser") = hidIsUser.Value
        Session("ModePhieu") = hidModePhieu.Value

        If Save() Then
            Dim iDN = Request.QueryString("DNId")
            Excute_Javascript("AlertboxRedirect('Mời bạn nhập tiếp mục 8.','CauHoi8.aspx?DNId=" & iDN & "');", Me.Page, True)
        Else
            Excute_Javascript("Alertbox('Cập nhật thất bại.');", Me.Page, True)
        End If
    End Sub

    Protected Sub btnReset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReset.Click
        Using data As New ThanhTraLaoDongEntities
            Dim q7 = (From p In data.CauHoi7 Where p.PhieuId = hidPhieuID.Value).FirstOrDefault
            If Not IsNothing(q7) Then
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
