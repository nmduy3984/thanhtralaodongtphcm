Imports System.Transactions
Imports Novacode
Imports ThanhTraLaoDongModel
Imports System.IO
Imports Cls_Common
Imports SecurityService
Partial Class Control_CauHoi_BienBanThanhTra_BienBanThanhTra
    Inherits System.Web.UI.UserControl
    Private lstKienNghiID() As String
    Dim strBHXH_BinhDangGioi As String = ""
    Private Shared ReadOnly log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            If Not Session("Username") = "" Then
                Dim script As ScriptManager = ScriptManager.GetCurrent(Me.Page)
                If Not script Is Nothing AndAlso script.IsInAsyncPostBack Then
                    ScriptManager.RegisterStartupScript(Me.Page, Me.Page.GetType, "duyjs", "ajaxJquery()", True)
                    ScriptManager.RegisterStartupScript(Me.Page, Me.Page.GetType, "duyjs1", "ajaxJqueryToolTip()", True)
                Else
                    Me.Page.ClientScript.RegisterStartupScript(Me.Page.GetType(), "duyjs", String.Concat("Sys.Application.add_load(function(){", "ajaxJquery()", "});"), True)
                    Me.Page.ClientScript.RegisterStartupScript(Me.Page.GetType(), "duyjs1", String.Concat("Sys.Application.add_load(function(){", "ajaxJqueryToolTip()", "});"), True)
                End If
                If Not Request.QueryString("PhieuID").ToString.Equals("0") Then
                    hidID.Value = Request.QueryString("PhieuID")
                End If
                hidUsername.Value = IIf(IsNothing(Session("Username")), "", Session("Username"))
                LoadData()
            Else
                Response.Redirect("../../Login.aspx")
            End If
        End If
    End Sub
    Protected Sub LoadData()
        Using data As New ThanhTraLaoDongEntities
            'Dim dn = (From a In data.PhieuNhapHeaders.Include("DoanhNghiep") Where a.PhieuID = hidID.Value Select a.DoanhNghiep.TenDoanhNghiep, a.SoQuyenDinh, a.DoanhNghiepId, a.NgayKetThucPhieu).FirstOrDefault
            Dim dn = (From a In data.PhieuNhapHeaders Join b In data.DoanhNghieps On a.DoanhNghiepId Equals b.DoanhNghiepId Where a.PhieuID = hidID.Value Select a.DoanhNghiep.TenDoanhNghiep, a.SoQuyenDinh, a.DoanhNghiepId, a.NgayKetThucPhieu, a.ThanhPhanThamGia).FirstOrDefault
            If Not dn Is Nothing Then
                'recheck validate tu phieu nhap
                Dim strSoQD As String = ""
                If dn.SoQuyenDinh.Split("/").Length > 1 Then
                    strSoQD = dn.SoQuyenDinh.Split("/")(0)
                End If
                'Xuất chuỗi bảo hiểm xã hội và bình đẳng giới trên phiếu kết luận
                Dim q6 = (From a In data.KetLuans Where a.TenBangCauHoi.Equals("CauHoi6") And a.PhieuId = hidID.Value).ToList
                Dim q12 = (From b In data.KetLuans Where b.TenBangCauHoi.Equals("CauHoi12") And b.PhieuId = hidID.Value).ToList
                If q6.Count > 0 Then
                    strBHXH_BinhDangGioi += ", bảo hiểm xã hội"
                End If
                If q12.Count > 0 AndAlso q6.Count > 0 Then
                    strBHXH_BinhDangGioi += " và bình đẳng giới"
                End If
                'Kiểm tra là thanh tra bộ/sở
                Dim iUserId As Integer = Session("UserId")
                Dim iChkLoaiTT = data.uspCheckLoaiThanhTraByUserId(iUserId).FirstOrDefault
                Dim TinhUserLogin As Tinh = (From t In data.Tinhs Join u In data.Users On t.TinhId Equals u.TinhTP Where u.UserId = iUserId Select t).SingleOrDefault
                'Nếu thuộc tính IsTinh là true thì không thêm 'tỉnh', ngược lại thêm tỉnh sau thuộc tính TenTinh
                Dim tinh As String = ""
                If Not IsNothing(TinhUserLogin) Then
                    tinh = IIf(IsNothing(TinhUserLogin.IsTinh) OrElse Not TinhUserLogin.IsTinh, "tỉnh " & TinhUserLogin.TenTinh, TinhUserLogin.TenTinh)
                End If
                'Load thành phần tham gia
                Dim strThanhPhanThamGia = ""
                If Not IsNothing(dn.ThanhPhanThamGia) Then
                    Dim arrThanhVienDoan() As String = dn.ThanhPhanThamGia.Split(Str_Symbol_Group)
                    For index As Integer = 0 To arrThanhVienDoan.Count - 2
                        strThanhPhanThamGia = strThanhPhanThamGia + arrThanhVienDoan(index) & Environment.NewLine
                    Next
                    strThanhPhanThamGia = strThanhPhanThamGia + arrThanhVienDoan(arrThanhVienDoan.Count - 1)
                End If
                txtDoanThanhTra.Text = strThanhPhanThamGia

                If Not IsNothing(iChkLoaiTT) Then 'Là thanh tra bộ
                    Dim TinhDN = (From a In data.DoanhNghieps Where a.DoanhNghiepId = dn.DoanhNghiepId Select New With {a.Tinh.TenTinh, a.Tinh.IsTinh}).SingleOrDefault
                    txtCoQuanBanHanh.Text = "THANH TRA BỘ LĐTBXH" & vbNewLine & "ĐOÀN THANH TRA " & strSoQD & "/QĐ-TTr"
                    hidTTBo.Value = "THANH TRA BỘ LĐTBXH"
                    txtQDThanhTra.Text = "Thực hiện Quyết định số " & strSoQD & "/QĐ-TTr ngày ... tháng ... năm ... của Chánh thanh tra Bộ Lao động - Thương binh và Xã hội, "
                    txtQDThanhTra.Text = txtQDThanhTra.Text & "ngày " & Day(dn.NgayKetThucPhieu).ToString & " tháng " & Month(dn.NgayKetThucPhieu).ToString & " năm " & Year(dn.NgayKetThucPhieu).ToString & " Đoàn thanh tra của Bộ Lao động - Thương binh và Xã hội đã tiến hành thanh tra việc chấp hành các quy định của pháp luật lao động" & strBHXH_BinhDangGioi & " tại "
                    txtQDThanhTra.Text = txtQDThanhTra.Text & dn.TenDoanhNghiep & "."
                    'txtDoanThanhTra.Text = "Ông ... - Thanh tra viên ... Bộ Lao động – Thương binh và Xã hội - Trưởng đoàn" & Environment.NewLine & "Ông ... - Thanh tra viên ... Bộ Lao động - Thương binh và Xã hội - Thành viên" & Environment.NewLine & "Ông ... - Thanh tra viên Sở Lao động - Thương binh và Xã hội ... - Thành viên."
                Else 'Là thanh sở
                    txtCoQuanBanHanh.Text = "SỞ LĐTBXH " & tinh.ToUpper & vbNewLine & "ĐOÀN THANH TRA " & strSoQD & "/QĐ-TTr"
                    hidTTBo.Value = "SỞ LĐTBXH " & tinh.ToUpper
                    txtQDThanhTra.Text = "Thực hiện Quyết định số " & strSoQD & "/QĐ-TTr ngày ... tháng ... năm ... của Chánh thanh tra Sở Lao động - Thương binh và Xã hội " & tinh & ", "
                    txtQDThanhTra.Text = txtQDThanhTra.Text & "ngày " & Day(dn.NgayKetThucPhieu).ToString & " tháng " & Month(dn.NgayKetThucPhieu).ToString & " năm " & Year(dn.NgayKetThucPhieu).ToString & " Đoàn thanh tra của Sở đã tiến hành thanh tra việc chấp hành các quy định của pháp luật lao động" & strBHXH_BinhDangGioi & " tại "
                    txtQDThanhTra.Text = txtQDThanhTra.Text & dn.TenDoanhNghiep & "."
                    'txtDoanThanhTra.Text = "Ông ... - Thanh tra viên ... Sở Lao động – Thương binh và Xã hội - Trưởng đoàn" & Environment.NewLine & "Ông ... - Thanh tra viên ... Sở Lao động - Thương binh và Xã hội - Thành viên" & Environment.NewLine & "Ông ... - Thanh tra viên Sở Lao động - Thương binh và Xã hội ... - Thành viên."
                End If

                hidDoanTT.Value = "ĐOÀN THANH TRA " & strSoQD & "/QĐ-TTr"
                txtPhamVi.Text = "Việc thực hiện pháp luật về lao động" & strBHXH_BinhDangGioi & " tại " & dn.TenDoanhNghiep
                txtNoiDungLamViec.Text = "Trưởng đoàn công bố quyết định thanh tra, thống nhất nội dung thanh tra và chương trình làm việc." & Environment.NewLine & "Đại diện doanh nghiệp báo cáo với Đoàn về tình hình chấp hành các quy định của pháp luật lao động" & strBHXH_BinhDangGioi & " tại doanh nghiệp." & Environment.NewLine & "Đoàn tiến hành kiểm tra hồ sơ, sổ sách chứng từ có liên quan, kiểm tra thực tế điều kiện làm việc của người lao động tại doanh nghiệp, kết quả như sau:"
                'Kiến nghị đoàn
                txtKNDoan.Text = ""
                Dim stt As Integer = 0
                Dim issign As Boolean = False
                Dim q7 = (From p In data.CauHoi7 Where p.PhieuId = hidID.Value).SingleOrDefault
                If Not IsNothing(q7) Then
                    If Not IsNothing(q7.Q7052) AndAlso q7.Q7052 > 0 Then
                        stt += 1
                        txtKNDoan.Text += stt.ToString + ". Đình chỉ hoạt động đối với " + String.Format(info, "{0:n0}", q7.Q7052) + " thiết bị có yêu cầu nghiêm ngặt về an toàn lao động chưa kiểm định " + IIf(IsNothing(q7.Q7053) OrElse q7.Q7053.Trim().Equals(""), "", "(" + q7.Q7053 + ")") + "để kiểm định kĩ thuật an toàn, nếu đảm bảo tiêu chuẩn an toàn mới tiếp tục sử dụng." & Environment.NewLine
                        issign = True

                    End If
                    If Not IsNothing(q7.Q717) AndAlso q7.Q717 Then
                        Dim strCombine = ""
                        Dim arrKL As New ArrayList
                        If Not IsNothing(q7.Q7171) And q7.Q7171 Then
                            arrKL.Add("bộ phận chuyển động không bao che")
                        End If
                        If Not IsNothing(q7.Q7172) And q7.Q7172 Then
                            arrKL.Add("thiếu lan can, rào ngăn")
                        End If
                        If Not IsNothing(q7.Q7173) And q7.Q7173 Then
                            arrKL.Add("thiếu biển báo nơi nguy hiểm")
                        End If
                        If Not IsNothing(q7.Q7177) And q7.Q7177 Then
                            arrKL.Add("thiếu bảng chỉ dẫn an toàn đối với máy, thiết bị đặt tại vị trí dễ đọc, dễ thấy tại nơi làm việc")
                        End If
                        If Not IsNothing(q7.Q7174) And q7.Q7174 Then
                            arrKL.Add("hộp cầu dao điện hở")
                        End If
                        If Not IsNothing(q7.Q7175) And q7.Q7175 Then
                            arrKL.Add("đấu nối điện không đúng quy cách")
                        End If
                        If Not IsNothing(q7.Q7178) And q7.Q7178 Then
                            arrKL.Add("người lao động không sử dụng phương tiện bảo vệ cá nhân")
                        End If
                        If Not String.IsNullOrEmpty(q7.Q7176) Then
                            arrKL.Add(q7.Q7176)
                        End If
                        If arrKL.Count > 0 Then
                            'Xuất kết luận
                            strCombine = String.Join(", ", TryCast(arrKL.ToArray(GetType(String)), String()))
                            arrKL.Clear()
                        End If
                        stt += 1
                        txtKNDoan.Text += stt.ToString + ". Xử lý ngay các yếu tố nguy hiểm tại nơi làm việc: " + strCombine + "." & Environment.NewLine
                        issign = True

                    End If
                    If issign Then
                        stt += 1
                        txtKNDoan.Text += stt.ToString + ". Các thiếu sót khác, doanh nghiệp thực hiện theo kết luận thanh tra."
                    End If
                End If
            End If
        End Using
    End Sub
    Protected Sub btnInPhieu_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnInPhieu.Click
        InBienBan()
    End Sub
    Protected Sub InBienBan()
        'Using sc As New TransactionScope
        Try
            Dim FolderPath = Server.MapPath("~/Template").ToString
            Dim attributes As FileAttributes = File.GetAttributes(FolderPath & "\BBTT_Template.docx")
            If (attributes And FileAttributes.ReadOnly) = FileAttributes.ReadOnly Then
                attributes = attributes And Not FileAttributes.ReadOnly
                File.SetAttributes(FolderPath & "\BBTT_Template.docx", attributes)
            End If
            Using document As DocX = DocX.Load(FolderPath & "\BBTT_Template.docx")
                Using data As New ThanhTraLaoDongEntities
                    Dim iPhieuId = CInt(Request.QueryString("phieuId"))
                    Dim dn = (From a In data.DoanhNghieps Join b In data.PhieuNhapHeaders On a.DoanhNghiepId Equals b.DoanhNghiepId
                                Where b.PhieuID = iPhieuId
                                Select New With {b.YKienCuaDN, a.NamTLDN, a.TenDoanhNghiep, a.LoaiHinhDoanhNghiep.TenLoaiHinhDN, a.TruSoChinh, a.Tinh.TenTinh, a.Tinh.IsTinh, a.Huyen.TenHuyen, a.DienThoai, a.Fax, a.SoChungNhanDKKD,
                                                a.NgayChungNhanDKKD, a.LanThayDoi, a.NgayThayDoi, b.IsCongDoan, a.SoTKNganHang, a.TenNganHang, a.LoaiHinhSanXuat.Title, b.NgayKetThucPhieu}).FirstOrDefault

                    'Phân biệt tỉnh/tp
                    Dim tinhtp = IIf(IsNothing(dn.IsTinh) OrElse Not dn.IsTinh, "tỉnh ", "") + dn.TenTinh
                    ''''Cơ quan ban hành              
                    document.ReplaceText("<<cqbanhanh1>>", hidTTBo.Value)
                    document.ReplaceText("<<cqbanhanh2>>", hidDoanTT.Value)
                    ''''Tỉnh
                    document.ReplaceText("<<tinh>>", dn.TenTinh + "," + " ngày " + Day(dn.NgayKetThucPhieu).ToString + " tháng " + Month(dn.NgayKetThucPhieu).ToString + " năm " + Year(dn.NgayKetThucPhieu).ToString)
                    ''''Phạm vi
                    document.ReplaceText("<<phamvi>>", txtPhamVi.Text)
                    ''''Quyết định thanh tra
                    document.ReplaceText("<<qdthanhtra>>", txtQDThanhTra.Text)

                    'Thành phần Đoàn thanh tra
                    Dim pParagraph As Paragraph
                    pParagraph = (From q In document.Paragraphs
                                    Where q.Text.Contains("<<doanthanhtra>>")).FirstOrDefault()

                    Dim lstDoanTT = ReadAllLines(txtDoanThanhTra.Text)
                    For Each item In lstDoanTT
                        If Not String.IsNullOrEmpty(item) Then
                            Dim pNew As Paragraph = document.InsertParagraph
                            pNew.StyleName = pParagraph.StyleName
                            pNew.Append(item)
                            pParagraph.InsertParagraphBeforeSelf(pNew)
                            ' tìm ra paragraph cuối cùng vào  xóa đi 
                            Dim pNewEnd = document.Paragraphs.LastOrDefault()
                            pNewEnd.Remove(False)
                        End If
                    Next
                    pParagraph.Remove(False)

                    ' ''''Đại diện doanh nghiệp làm việc với Đoàn gồm
                    pParagraph = (From q In document.Paragraphs
                                    Where q.Text.Contains("<<daidiendoanhnghiep>>")).FirstOrDefault()
                    Dim lstDaiDienDN = ReadAllLines(txtDaiDienDN.Text)
                    For Each item In lstDaiDienDN
                        Dim pNew As Paragraph = document.InsertParagraph
                        pNew.StyleName = pParagraph.StyleName
                        pNew.Append(item)
                        pParagraph.InsertParagraphBeforeSelf(pNew)
                        ' tìm ra paragraph cuối cùng vào  xóa đi 
                        Dim pNewEnd = document.Paragraphs.LastOrDefault()
                        pNewEnd.Remove(False)
                    Next
                    pParagraph.Remove(False)

                    ''''NỘI DUNG LÀM VIỆC
                    pParagraph = (From q In document.Paragraphs
                                    Where q.Text.Contains("<<noidunglamviec>>")).FirstOrDefault()
                    'Dim strContent = "Ông Lê Mạnh Kiểm - Trưởng đoàn thanh tra công bố quyết định thanh tra, thống nhất nội dung thanh tra và chương trình làm việc." & Environment.NewLine & "Đại diện doanh nghệpbáo cáo với Đoàn thanh tra về tình hình chấp hành các quy định của pháp luật lao động, bảo hiểm xã hội và bình đẳng giới tại doanh nghiệp." & Environment.NewLine & "Đoàn thanh tra tiến hành kiểm tra hồ sơ, sổ sách chứng từ có liên quan, kiểm tra thực tế điều kiện làm việc của người lao động tạidoanh nghiệp. Kết quả như sau:"
                    Dim lstContent = ReadAllLines(txtNoiDungLamViec.Text)
                    For Each item In lstContent
                        Dim pNew As Paragraph = document.InsertParagraph
                        pNew.StyleName = pParagraph.StyleName
                        pNew.Append(item)
                        pParagraph.InsertParagraphBeforeSelf(pNew)
                        ' tìm ra paragraph cuối cùng vào  xóa đi 
                        Dim pNewEnd = document.Paragraphs.LastOrDefault()
                        pNewEnd.Remove(False)
                    Next
                    pParagraph.Remove(False)

                    ''''THÔNG TIN CHUNG VỀ DOANH NGHIỆP
                    pParagraph = (From q In document.Paragraphs
                                    Where q.Text.Contains("<<thongtindoanhnghiep>>")).FirstOrDefault()
                    For i As Integer = 1 To 8
                        Dim pNewDN As Paragraph = document.InsertParagraph
                        pNewDN.StyleName = pParagraph.StyleName
                        Select Case i
                            Case 1
                                pNewDN.Append("Tên doanh nghiệp: " & dn.TenDoanhNghiep)
                            Case 2
                                pNewDN.Append("Loại hình doanh nghiệp: " & dn.TenLoaiHinhDN)
                            Case 3
                                pNewDN.Append("Doanh nghiệp thành lập năm: " & dn.NamTLDN)
                            Case 4
                                pNewDN.Append("Điện thoại: " & dn.DienThoai & vbTab & vbTab & "Fax: " & dn.Fax)
                            Case 5
                                pNewDN.Append("Ngành nghề sản xuất kinh doanh chính: " & dn.Title)
                            Case 6
                                Dim strtemp = "Giấy chứng nhận đăng ký kinh doanh số " & dn.SoChungNhanDKKD & " do Sở Kế hoạch và Đầu tư " & tinhtp & " cấp ngày " & CType(dn.NgayChungNhanDKKD, Date).ToString("dd/MM/yyyy")
                                If Not IsNothing(dn.LanThayDoi) AndAlso dn.LanThayDoi > 0 Then
                                    strtemp = strtemp & ", thay đổi lần thứ " & dn.LanThayDoi & " ngày " & CType(dn.NgayThayDoi, Date).ToString("dd/MM/yyyy") + "."
                                Else
                                    strtemp = strtemp & "."
                                End If
                                pNewDN.Append(strtemp)
                            Case 7
                                pNewDN.Append("Trụ sở chính của doanh nghiệp: " & dn.TruSoChinh + ", " & dn.TenHuyen & ", " + tinhtp)
                            Case 8
                                If Not IsNothing(dn.IsCongDoan) Then
                                    If dn.IsCongDoan Then
                                        pNewDN.Append("Doanh nghiệp đã có tổ chức công đoàn cơ sở")
                                    Else
                                        pNewDN.Append("Doanh nghiệp chưa có tổ chức công đoàn cơ sở")
                                    End If
                                Else
                                    pNewDN.Append("Doanh nghiệp chưa có tổ chức công đoàn cơ sở")
                                End If
                        End Select
                        pParagraph.InsertParagraphBeforeSelf(pNewDN)
                        ' tìm ra paragraph cuối cùng vào  xóa đi 
                        Dim pNewEnd = document.Paragraphs.LastOrDefault()
                        pNewEnd.Remove(False)
                    Next
                    pParagraph.Remove(False)

                    '********************* TÌNH HÌNH THỰC HIỆN PHÁP LUẬT LAO ĐỘNG, BẢO HIỂM XÃ HỘI VÀ BÌNH ĐẲNG GIỚI ******************************
                    '*********************** Cau hoi 1-12 ****************
                    Dim arrStr() As String = {"",
                                                "Các loại báo cáo định kỳ",
                                                "Tuyển dụng và đào tạo lao động",
                                                "Thỏa ước lao động tập thể",
                                                "Tiền lương",
                                                "Thời giờ làm việc, thời giờ nghỉ ngơi",
                                                "Bảo hiểm xã hội, bảo hiểm thất nghiệp",
                                                "An toàn lao động, vệ sinh lao động",
                                                "Kỷ luật lao động, trách nhiệm vật chất",
                                                "Tranh chấp lao động",
                                                "Lao động là người nước ngoài",
                                                "Lao động đặc thù",
                                                "Lao động nữ và bình đẳng giới"}
                    For k As Integer = 1 To 12
                        Dim strSTT = "<<cauhoi" & k & ">>"
                        Dim strCauhoi = "CauHoi" & k & ""
                        ' lấy dữ liệu ở đây
                        Dim kl = (From a In data.KetLuans Where a.PhieuId = iPhieuId And a.TenBangCauHoi = strCauhoi And String.IsNullOrEmpty(a.NDKetLuan) = False Select a.NDKetLuan, a.TenCotCauHoi).ToList()
                        If kl.Count = 0 Then
                            document.ReplaceText("<<Muc" & k & ">>", "")
                            document.ReplaceText(strSTT, "")
                        Else
                            document.ReplaceText("<<Muc" & k & ">>", arrStr(k))
                            pParagraph = (From q In document.Paragraphs
                                            Where q.Text = strSTT).FirstOrDefault()
                            For i As Integer = 0 To kl.Count - 1
                                If Not kl(i).TenCotCauHoi.Equals("Q21") Then
                                    Dim pNew As Paragraph = document.InsertParagraph
                                    pNew.StyleName = pParagraph.StyleName
                                    pNew.Append(kl(i).NDKetLuan)
                                    pParagraph.InsertParagraphBeforeSelf(pNew)
                                    ' tìm ra paragraph cuối cùng vào  xóa đi 
                                    Dim pNewEnd = document.Paragraphs.LastOrDefault()
                                    pNewEnd.Remove(False)
                                End If
                            Next
                            pParagraph.Remove(False)
                        End If
                    Next
                    '*************** Xuất mục Q21**************
                    Dim klQ21 = (From a In data.KetLuans Where a.IsViPham = TypeViPham.KhongViPham And a.TenCotCauHoi.Equals("Q21") And a.PhieuId = hidID.Value And String.IsNullOrEmpty(a.NDKetLuan) = False Select a.NDKetLuan).ToList()
                    If klQ21.Count > 0 Then
                        Dim strQ21() As String = {}
                        strQ21 = klQ21(0).Split("#")
                        document.ReplaceText("<<HeadQ21>>", strQ21(0))
                        'TH: Có kết luận dalamdcQ21Sub
                        'Khai báo pParagraph với template <<Q21>>
                        pParagraph = (From q In document.Paragraphs
                                Where q.Text = "<<Q21>>").FirstOrDefault()
                        For j As Integer = 1 To strQ21.Count - 1
                            Dim pNewQ21 As Paragraph = document.InsertParagraph
                            pNewQ21.StyleName = pParagraph.StyleName
                            pNewQ21.Append(strQ21(j))
                            pParagraph.InsertParagraphBeforeSelf(pNewQ21)
                            ' tìm ra paragraph cuối cùng vào  xóa đi 
                            Dim pNewEndQ21 = document.Paragraphs.LastOrDefault()
                            pNewEndQ21.Remove(False)
                        Next
                        'Remove template <<dalamdcQ21Sub>>
                        pParagraph.Remove(False)
                    Else
                        document.ReplaceText("<<HeadQ21>>", "")
                        document.ReplaceText("<<Q21>>", "")
                    End If
                    '***************** 1.	Những quy định của pháp luật đã được doanh nghiệp thực hiện ***************************
                    pParagraph = (From q In document.Paragraphs
                            Where q.Text.Contains("<<qdDNdathuchien>>")).FirstOrDefault()
                    ' lấy dữ liệu ở đây
                    Dim kldathuchien = (From a In data.KetLuans Where a.IsViPham = TypeViPham.KhongViPham And a.TenCotCauHoi.Equals("Q21") = False And a.PhieuId = iPhieuId And String.IsNullOrEmpty(a.NDKetLuan) = False Select a.NDKetLuan, a.TenCotCauHoi).ToList()
                    For i As Integer = 0 To kldathuchien.Count - 1

                        Dim pNew As Paragraph = document.InsertParagraph
                        pNew.StyleName = pParagraph.StyleName
                        pNew.Append(kldathuchien(i).NDKetLuan)
                        pParagraph.InsertParagraphBeforeSelf(pNew)
                        ' tìm ra paragraph cuối cùng vào  xóa đi 
                        Dim pNewEnd = document.Paragraphs.LastOrDefault()
                        pNewEnd.Remove(False)
                    Next
                    pParagraph.Remove(False)

                    '*************** Xuất mục Q21**************
                    kldathuchien = (From a In data.KetLuans Where a.IsViPham = TypeViPham.KhongViPham And a.TenCotCauHoi.Equals("Q21") And a.PhieuId = iPhieuId And String.IsNullOrEmpty(a.NDKetLuan) = False Select a.NDKetLuan, a.TenCotCauHoi).ToList()
                    If kldathuchien.Count > 0 Then
                        Dim strQ21() As String = {}
                        strQ21 = kldathuchien(0).NDKetLuan.Split("#")
                        document.ReplaceText("<<HeadqdDNdathuchienQ21>>", strQ21(0))
                        'TH: Có kết luận qdDNdathuchienQ21Sub
                        'Khai báo pParagraph với template <<qdDNdathuchienQ21Sub>>
                        pParagraph = (From q In document.Paragraphs
                                Where q.Text = "<<qdDNdathuchienQ21Sub>>").FirstOrDefault()
                        For j As Integer = 1 To strQ21.Count - 1
                            Dim pNewQ21 As Paragraph = document.InsertParagraph
                            pNewQ21.StyleName = pParagraph.StyleName
                            pNewQ21.Append(strQ21(j))
                            pParagraph.InsertParagraphBeforeSelf(pNewQ21)
                            ' tìm ra paragraph cuối cùng vào  xóa đi 
                            Dim pNewEndQ21 = document.Paragraphs.LastOrDefault()
                            pNewEndQ21.Remove(False)
                        Next
                        'Remove template <<qdDNdathuchienQ21Sub>>
                        pParagraph.Remove(False)

                    Else
                        document.ReplaceText("<<HeadqdDNdathuchienQ21>>", "")
                        document.ReplaceText("<<qdDNdathuchienQ21Sub>>", "")
                    End If


                    '****************** 2.	Những quy định của pháp luật chưa được doanh nghiệp thực hiện hoặc thực hiện chưa đầy đủ ****************
                    pParagraph = (From q In document.Paragraphs
                                            Where q.Text.Contains("<<qdNchuathuchien>>")).FirstOrDefault()
                    ' lấy dữ liệu ở đây
                    Dim klchuathuchien = (From a In data.KetLuans Where a.IsViPham = TypeViPham.ViPham And a.PhieuId = iPhieuId And String.IsNullOrEmpty(a.NDKetLuan) = False Select a.NDKetLuan).ToList()
                    For i As Integer = 0 To klchuathuchien.Count - 1
                        Dim pNew As Paragraph = document.InsertParagraph
                        pNew.StyleName = pParagraph.StyleName
                        pNew.Append(klchuathuchien(i))
                        pParagraph.InsertParagraphBeforeSelf(pNew)
                        ' tìm ra paragraph cuối cùng vào  xóa đi 
                        Dim pNewEnd = document.Paragraphs.LastOrDefault()
                        pNewEnd.Remove(False)
                    Next
                    pParagraph.Remove(False)

                    '*********************** Kiến nghị của đoàn ***********************
                    If txtKNDoan.Text.Trim().Equals("") Then
                        document.ReplaceText("<<kiennghidoantitle>>", "")
                        document.ReplaceText("<<kiennghidoan>>", "")
                    Else
                        document.ReplaceText("<<kiennghidoantitle>>", "KIẾN NGHỊ CỦA ĐOÀN")
                        pParagraph = (From q In document.Paragraphs
                                                            Where q.Text.Contains("<<kiennghidoan>>")).FirstOrDefault()
                        Dim lsttxtKNDoan = ReadAllLines(txtKNDoan.Text)
                        For Each item In lsttxtKNDoan
                            Dim pNew As Paragraph = document.InsertParagraph
                            pNew.StyleName = pParagraph.StyleName
                            pNew.Append(item.Substring(3))
                            pParagraph.InsertParagraphBeforeSelf(pNew)
                            ' tìm ra paragraph cuối cùng vào  xóa đi 
                            Dim pNewEnd = document.Paragraphs.LastOrDefault()
                            pNewEnd.Remove(False)
                        Next
                        pParagraph.Remove(False)
                    End If

                    '*********************** Ý kiến doanh nghiệp **********************************
                    ' lấy dữ liệu ở đây
                    If Not IsNothing(dn.YKienCuaDN) Then
                        pParagraph = (From q In document.Paragraphs
                                                                   Where q.Text.Contains("<<ykiendoanhnghiep>>")).FirstOrDefault()
                        Dim lstYKienDN = ReadAllLines(dn.YKienCuaDN.Replace(Str_Symbol_Group, vbNewLine))
                        For i As Integer = 0 To lstYKienDN.Count - 1
                            Dim pNew As Paragraph = document.InsertParagraph
                            pNew.StyleName = pParagraph.StyleName

                            pNew.Append(lstYKienDN(i))
                            pParagraph.InsertParagraphBeforeSelf(pNew)
                            ' tìm ra paragraph cuối cùng vào  xóa đi 
                            Dim pNewEnd = document.Paragraphs.LastOrDefault()
                            pNewEnd.Remove(False)
                        Next
                        pParagraph.Remove(False)
                    Else
                        document.ReplaceText("<<ykiendoanhnghiep>>", "Nhất trí với nội dung biên bản làm việc")
                    End If
                    document.ReplaceText("<<giophut>>", Now.Hour.ToString + " giờ " + Now.Minute.ToString + " phút ")

                    '******************* Đại diện doanh nghiệp ký *********************
                    document.ReplaceText("<<daidienDN>>", txtNguoiKy.Text)
                    '******************* Đại diện thanh tra ký **********************
                    document.ReplaceText("<<daidienthanhtra>>", txtTruongDoanTT.Text)

                    ''Lưu thông tin thành phần tham gia, đại diện doanh nghiệp
                    Dim pnh As PhieuNhapHeader = (From a In data.PhieuNhapHeaders Where a.PhieuID = iPhieuId).SingleOrDefault
                    If Not IsNothing(pnh) Then
                        pnh.ThanhPhanThamGia = String.Join("#", ReadAllLines(txtDoanThanhTra.Text))
                        pnh.DaiDienDoanhNghiep = String.Join("#", ReadAllLines(txtDaiDienDN.Text))
                    End If
                    data.SaveChanges()
                End Using
                'Xuat BBTT ra file word
                Dim FileName = "BBTT_" & Now.ToString("dd_MM_yyyy_hh_mm") & ".docx"
                ' Request.Url
                Dim URL As String = "Output/" & FileName
                document.SaveAs(Server.MapPath("~/Output").ToString & "\" & FileName)
                Excute_Javascript("AlertboxRedirect('Biên bản được in thành công.','List.aspx');window.location='../../" + URL + "'; ", Me.Page, True)

            End Using
            'sc.Complete()
        Catch ex As Exception
            Excute_Javascript("Alertbox('Việc xuất dữ liệu bị lỗi! " & ex.Message & "');", Me.Page, True)
            'sc.Dispose()
        End Try

        'End Using
    End Sub
    Protected Sub btnReset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReset.Click
        LoadData()
        txtNguoiKy.Text = ""
        txtTruongDoanTT.Text = ""
    End Sub

End Class
