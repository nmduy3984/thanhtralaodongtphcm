Imports System.Data
Imports System.Transactions
Imports Cls_Common
Imports SecurityService
Imports ThanhTraLaoDongModel
Imports Novacode
Imports System.IO
Partial Class Control_CauHoi_PhieuKetLuan
    Inherits System.Web.UI.UserControl
    Private Shared ReadOnly log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
    Private lstKienNghiID() As String
    Dim strBHXH_BinhDangGioi As String = ""
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
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
        Try
            Using data As New ThanhTraLaoDongEntities
                Dim ttdn = (From a In data.DoanhNghieps,
                            b In data.Tinhs,
                            c In data.LoaiHinhDoanhNghieps,
                            d In data.PhieuNhapHeaders
                            Where
                                    a.TinhId = b.TinhId And
                                    a.LoaiHinhDNId = c.LoaiHinhDNId And
                                    a.DoanhNghiepId = d.DoanhNghiepId And
                                    d.PhieuID = hidID.Value
                            Select New With {a.TenDoanhNghiep,
                                            a.NamTLDN,
                                            a.DienThoai,
                                            a.Fax,
                                            a.LoaiHinhSanXuat.Title,
                                            a.SoChungNhanDKKD,
                                            a.NgayChungNhanDKKD,
                                            a.LanThayDoi,
                                            a.NgayThayDoi,
                                            a.TruSoChinh,
                                            a.SoTKNganHang,
                                            a.TenNganHang,
                                            a.IsCongDoan,
                                            b.TenTinh,
                                            b.IsTinh,
                                            d.SoQuyenDinh,
                                            c.TenLoaiHinhDN}).FirstOrDefault

                If Not ttdn Is Nothing Then
                    'Xuất chuỗi bảo hiểm xã hội và bình đẳng giới trên phiếu kết luận
                    Dim q6 = (From a In data.KetLuans Where a.TenBangCauHoi.Equals("CauHoi6") And a.PhieuId = hidID.Value).ToList
                    Dim q12 = (From b In data.KetLuans Where b.TenBangCauHoi.Equals("CauHoi12") And b.PhieuId = hidID.Value).ToList
                    If q6.Count > 0 Then
                        strBHXH_BinhDangGioi += ", bảo hiểm xã hội"
                    End If
                    If q12.Count > 0 AndAlso q6.Count > 0 Then
                        strBHXH_BinhDangGioi += " và bình đẳng giới"
                    End If
                    'Xác định thanh tra bộ hay thanh tra sở
                    Dim iUserId As Integer = Session("userid")
                    Dim chkTT = data.uspCheckLoaiThanhTraByUserId(iUserId).FirstOrDefault
                     Dim TinhUserLogin As Tinh = (From t In data.Tinhs Join u In data.Users On t.TinhId Equals u.TinhTP Where u.UserId = iUserId Select t).SingleOrDefault
                    'Tỉnh nơi nhận theo doanh nghiệp
                    hidTinh.Value = IIf(IsNothing(ttdn.IsTinh) OrElse Not ttdn.IsTinh, ("tỉnh " + ttdn.TenTinh), ttdn.TenTinh)
                    'Tỉnh theo user
                    Dim tinhuser As String = IIf(IsNothing(TinhUserLogin.IsTinh) OrElse Not TinhUserLogin.IsTinh, ("tỉnh " + TinhUserLogin.TenTinh), TinhUserLogin.TenTinh)
                    Dim congty = ttdn.TenDoanhNghiep
                    If Not IsNothing(chkTT) Then 'Là thanh tra bộ
                        txtCQBH.Text = "BỘ LAO ĐỘNG - THƯƠNG BINH VÀ XÃ HỘI" + Environment.NewLine + "THANH TRA BỘ"
                        hidcqbh1.Value = "BỘ LAO ĐỘNG - THƯƠNG BINH VÀ XÃ HỘI"
                        hidcqbh2.Value = "THANH TRA BỘ"
                    Else 'Là thanh tra sở
                        txtCQBH.Text = "SỞ LAO ĐỘNG - THƯƠNG BINH VÀ XÃ HỘI " & tinhuser.ToUpper & Environment.NewLine + "THANH TRA SỞ"
                        hidcqbh1.Value = "SỞ LAO ĐỘNG - THƯƠNG BINH VÀ XÃ HỘI " & tinhuser.ToUpper
                        hidcqbh2.Value = "THANH TRA SỞ"
                    End If
                    txtPV.Text = "Việc chấp hành các quy định của pháp luật về lao động" & strBHXH_BinhDangGioi & " tại " + congty

                    Dim strSoQD As String = ""
                    If ttdn.SoQuyenDinh.Split("/").Length > 1 Then
                        strSoQD = ttdn.SoQuyenDinh.Split("/")(0)
                    End If

                    txtCC.Text = "Căn cứ Điều 55 Luật Thanh tra ngày 25 tháng 11 năm 2010;" & Environment.NewLine +
                                "Xét báo cáo kết quả thanh tra của Trưởng đoàn thanh tra theo Quyết định số " & strSoQD & "/QĐ-TTr ngày   tháng   năm  của Chánh Thanh tra " & IIf(IsNothing(chkTT), "Sở", "Bộ") & " Lao động - Thương binh và Xã hội về việc thanh tra việc chấp hành các quy định của pháp luật về lao động" & strBHXH_BinhDangGioi & " tại doanh nghiệp trên địa bàn " + hidTinh.Value + ","
                    hidCanCu.Value = "Căn cứ Điều 55 Luật Thanh tra ngày 25 tháng 11 năm 2010;"
                    hidNDCanCu.Value = "Xét báo cáo kết quả thanh tra của Trưởng đoàn thanh tra theo Quyết định số " & strSoQD & "/QĐ-TTr ngày   tháng   năm  của Chánh Thanh tra " & IIf(IsNothing(chkTT), "Sở", "Bộ") & " Lao động - Thương binh và Xã hội về việc thanh tra việc chấp hành các quy định của pháp luật về lao động" & strBHXH_BinhDangGioi & " tại doanh nghiệp trên địa bàn " + hidTinh.Value + ","
                    Session("ttdn") = ttdn

                End If

                '' Load danh sách các kiến nghị của doanh nghiệp nếu có 
                Dim lsKN As List(Of uspListKienNghiDNByPhieuId_Result) = data.uspListKienNghiDNByPhieuId(hidID.Value).ToList()
                If lsKN.Count > 0 Then
                    Dim strKey_Name() As String = {"KienNghiId", "NDKienNghi"}
                    With grdShow
                        .DataKeyNames = strKey_Name
                        .DataSource = lsKN
                        .DataBind()
                    End With
                End If
            End Using
        Catch ex As Exception
            Excute_Javascript("Alertbox('Đã xảy ra lỗi trong lúc load dữ liệu!');", Me.Page, True)
        End Try
    End Sub
    'Protected Sub hpKienNghi_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles hpKienNghi.Init
    '    hpKienNghi.CssClass = "thickbox"
    '    hpKienNghi.NavigateUrl = "../../Page/KienNghi/PopupSelectKienNghi.aspx?keepThis=true&TB_iframe=true&height=420&width=600&modal=true"
    'End Sub
    Protected Sub grdShow_RowDataBound(ByVal sender As Object, ByVal e As WebControls.GridViewRowEventArgs) Handles grdShow.RowDataBound
        If e.Row.RowIndex >= 0 Then
            Dim lblSTT As Label = CType(e.Row.FindControl("lblSTT"), Label)
            lblSTT.Text = e.Row.RowIndex + 1
        End If
    End Sub
    Protected Sub Button_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSubmit.Click, btnExport.Click
        Select Case CType(sender, Control).ID
            Case "btnSubmit"
                Dim lstKienNghiID() As String
                lstKienNghiID = Strings.Split(hidKienNghiID.Value, Str_Symbol_Group)
                Using data As New ThanhTraLaoDongEntities
                    Dim lsKN = (From q In data.DanhMucKienNghis
                                Where lstKienNghiID.Contains(q.KienNghiID)).ToList()
                    Dim strKey_Name() As String = {"KienNghiID", "NoiDungKN"}
                    With grdShow
                        .DataKeyNames = strKey_Name
                        .DataSource = lsKN
                        .DataBind()
                    End With
                End Using
            Case "btnExport"
                Using sc As New TransactionScope
                    Try
                        Using data As New ThanhTraLaoDongEntities
                            Dim FolderPath = Server.MapPath("~/Template").ToString
                            Dim attributes As FileAttributes = File.GetAttributes(FolderPath & "\KLTT_template.docx")
                            If (attributes And FileAttributes.ReadOnly) = FileAttributes.ReadOnly Then
                                attributes = attributes And Not FileAttributes.ReadOnly
                                File.SetAttributes(FolderPath & "\KLTT_template.docx", attributes)
                            End If
                            Dim pParagraph As Paragraph
                            Using document As DocX = DocX.Load(FolderPath & "\KLTT_template.docx")

                                '' Phần Cơ quan ban hành
                                document.ReplaceText("<<cqbh1>>", hidcqbh1.Value)
                                document.ReplaceText("<<cqbh2>>", hidcqbh2.Value)
                                '' Phạm vi 
                                document.ReplaceText("<<phamvi>>", txtPV.Text)

                                '' căn cứ
                                document.ReplaceText("<<cancu>>", hidCanCu.Value)
                                document.ReplaceText("<<noidungcancu>>", hidNDCanCu.Value)
                                '' Phân thông tin doanh nghiệp ở đây                  
                                pParagraph = (From q In document.Paragraphs
                                            Where q.Text.Contains("<<tendn>>")).FirstOrDefault()

                                '' Lấy thông tin của doanh nghiệp gồm 7 phần 
                                'Dim ttdn = Session("ttdn")
                                Dim iPhieuId = Request.QueryString("PhieuId")
                                Dim ttdn = (From a In data.DoanhNghieps Join b In data.PhieuNhapHeaders On a.DoanhNghiepId Equals b.DoanhNghiepId
                                          Where b.PhieuID = iPhieuId
                                          Select New With {a.TenDoanhNghiep, a.LoaiHinhDoanhNghiep.TenLoaiHinhDN, a.TruSoChinh, a.Tinh.TenTinh, a.Tinh.IsTinh, a.Huyen.TenHuyen, a.Tinh.MoTa, a.DienThoai, a.Fax, a.SoChungNhanDKKD,
                                                          a.NgayChungNhanDKKD, a.LanThayDoi, a.NgayThayDoi, b.IsCongDoan, b.SoQuyenDinh, a.SoTKNganHang, a.TenNganHang, a.LoaiHinhSanXuat.Title, b.KienNghiDNPhaiTH}).FirstOrDefault

                                ''tinh
                                Dim userId As Integer = Session("UserId")
                                Dim TinhUser As Tinh = (From t In data.Tinhs Join u In data.Users On t.TinhId Equals u.TinhTP Where u.UserId = userId Select t).SingleOrDefault
                                Dim chkTT = data.uspCheckLoaiThanhTraByUserId(userId).FirstOrDefault
                                hidIsThanhTra.Value = IIf(Not IsNothing(chkTT) AndAlso chkTT.UserId > 0, 1, 0) '1: thanh tra bộ; 0: thanh tra sở
                                Dim isUser As Integer = Session("IsUser")
                                document.ReplaceText("<<tinh>>", IIf(hidIsThanhTra.Value = 1 AndAlso isUser = 1, "Hà Nội", TinhUser.TenTinh) + "," + " ngày   tháng   năm " & Now.Year.ToString())
                                'Kiểm tra là thanh tra bộ/sở
                                Dim iChkLoaiTT = data.uspCheckLoaiThanhTraByUserId(userId).FirstOrDefault
                                If Not IsNothing(iChkLoaiTT) Then
                                    document.ReplaceText("<<thanhtraketluan>>", "Chánh Thanh tra Bộ Lao động - Thương binh và Xã hội kết luận như sau:")
                                Else
                                    document.ReplaceText("<<thanhtraketluan>>", "Chánh Thanh tra Sở Lao động - Thương binh và Xã hội " & IIf(IsNothing(TinhUser.IsTinh) OrElse Not TinhUser.IsTinh, ("tỉnh " + TinhUser.TenTinh), TinhUser.TenTinh) & " kết luận như sau:")
                                End If

                                For i As Integer = 1 To 7
                                    Dim pNew As Paragraph = document.InsertParagraph '' Mặc định sẻ insert vào cuối cùng của văn bản
                                    pNew.StyleName = pParagraph.StyleName
                                    Select Case i
                                        Case 1
                                            pNew.Append("Tên doanh nghiệp: " + ttdn.TenDoanhNghiep + ".")
                                        Case 2
                                            pNew.Append("Loại hình doanh nghiệp: " + ttdn.TenLoaiHinhDN)
                                        Case 3
                                            pNew.Append("Trụ sở chính: " + ttdn.TruSoChinh + ", " & ttdn.TenHuyen & ", " + IIf(IsNothing(ttdn.IsTinh) OrElse Not ttdn.IsTinh, "tỉnh ", "") + ttdn.TenTinh + Environment.NewLine _
                                                    + "  Điện thoại: " + ttdn.DienThoai + "             Fax:" + ttdn.Fax)
                                        Case 4
                                            Dim strtemp = "Giấy chứng nhận đăng ký kinh doanh số " & ttdn.SoChungNhanDKKD & " do Sở Kế hoạch và Đầu tư " + IIf(IsNothing(ttdn.IsTinh) OrElse Not ttdn.IsTinh, "tỉnh ", "") + ttdn.TenTinh & " cấp ngày " & CType(ttdn.NgayChungNhanDKKD, Date).ToString("dd/MM/yyyy")
                                            If Not IsNothing(ttdn.LanThayDoi) AndAlso ttdn.LanThayDoi > 0 Then
                                                strtemp = strtemp & ", thay đổi lần thứ " & ttdn.LanThayDoi & " ngày " & CType(ttdn.NgayThayDoi, Date).ToString("dd/MM/yyyy") + "."
                                            Else
                                                strtemp = strtemp & "."
                                            End If
                                            pNew.Append(strtemp)
                                        Case 5
                                            pNew.Append("Ngành nghề sản xuất kinh doanh chính: " + ttdn.Title + ".")
                                        Case 6
                                            pNew.Append("Số tài khoản: " + ttdn.SoTKNganHang + "                 tại: " + ttdn.TenNganHang)
                                        Case 7
                                            If Not IsNothing(ttdn.IsCongDoan) Then
                                                If ttdn.IsCongDoan Then
                                                    pNew.Append("Doanh nghiệp đã có tổ chức công đoàn cơ sở")
                                                Else
                                                    pNew.Append("Doanh nghiệp chưa có tổ chức công đoàn cơ sở")
                                                End If
                                            Else
                                                pNew.Append("Doanh nghiệp chưa có tổ chức công đoàn cơ sở")
                                            End If

                                    End Select

                                    pParagraph.InsertParagraphBeforeSelf(pNew)
                                    '' tìm ra paragraph cuối cùng vào  xóa đi 
                                    Dim pNewEnd = document.Paragraphs.LastOrDefault()
                                    pNewEnd.Remove(False)
                                Next
                                pParagraph.Remove(False)

                                ''*********************** Những điều đã làm được **********************************
                                NhungDieuLamDuoc(document)

                                ''*********************** Những điều chưa làm được **********************************
                                NhungDieuChuaLamDuoc(document)

                                '' Phần  danh sách kiến nghị của doanh nghiệp
                                pParagraph = (From q In document.Paragraphs
                                            Where q.Text.Contains("<<kiennghi>>")).FirstOrDefault()
                                Dim lstKNID As List(Of uspListKienNghiDNByPhieuId_Result) = data.uspListKienNghiDNByPhieuId(hidID.Value).ToList()
                                If lstKNID.Count > 0 Then
                                    For index As Integer = 0 To lstKNID.Count - 1
                                        '' Thêm dữ liệu vào document
                                        Dim pNew As Paragraph = document.InsertParagraph '' Mặc định sẻ insert vào cuối cùng của văn bản
                                        pNew.StyleName = pParagraph.StyleName
                                        pNew.Append(lstKNID(index).NDKienNghi)
                                        pParagraph.InsertParagraphBeforeSelf(pNew)
                                        '' tìm ra paragraph cuối cùng vào  xóa đi 
                                        Dim pNewEnd = document.Paragraphs.LastOrDefault()
                                        pNewEnd.Remove(False)
                                    Next
                                    pParagraph.Remove(False)
                                Else
                                    document.ReplaceText("<<kiennghi>>", "")
                                End If
                                ''Thời gian thực hiện kiến nghị
                                'Kiến nghị doanh nghiệp phải thực 
                                Dim lstKNDNPhaiTH = data.uspListKienNghiDNPhaiTHByPhieuId(iPhieuId).ToList
                                If lstKNDNPhaiTH(0).DSKNDNPhaiTH.Length > 0 Then
                                    document.ReplaceText("<<KNDNPhaiTH>>", "Doanh nghiệp phải thực hiện ngay kiến nghị số " & Left(lstKNDNPhaiTH(0).DSKNDNPhaiTH, Len(lstKNDNPhaiTH(0).DSKNDNPhaiTH) - 2) & " nêu trên.")
                                Else
                                    document.ReplaceText("<<KNDNPhaiTH>>", "")
                                End If

                                document.ReplaceText("<<kqththanhtrabo>>", IIf(Not IsNothing(iChkLoaiTT), "Thanh tra Bộ Lao động – Thương binh và Xã hội (số 2 Đinh Lễ, quận Hoàn Kiếm, thành phố  Hà Nội) và ", ""))
                                If Not IsNothing(iChkLoaiTT) Then ' Là thanh tra bộ
                                    ''tinhTTSo
                                    document.ReplaceText("<<tinhTTSo>>", IIf(IsNothing(ttdn.IsTinh) OrElse Not ttdn.IsTinh, "tỉnh " & ttdn.TenTinh, ttdn.TenTinh))
                                    'Địa chỉ thanh tra sở
                                    document.ReplaceText("<<diachiTTSo>>", IIf(IsNothing(ttdn.MoTa), "", ttdn.MoTa))
                                Else ' là thanh tra sở
                                    ''tinhTTSo
                                    document.ReplaceText("<<tinhTTSo>>", IIf(IsNothing(TinhUser.IsTinh) OrElse Not TinhUser.IsTinh, "tỉnh " & TinhUser.TenTinh, TinhUser.TenTinh))
                                    'Địa chỉ thanh tra sở
                                    document.ReplaceText("<<diachiTTSo>>", IIf(IsNothing(TinhUser.MoTa), "", TinhUser.MoTa))
                                End If
                                '****************************** PHẦN CUỐI CHO PHIẾU KẾT LUẬN ********************************
                                ''Noi nhân
                                pParagraph = (From q In document.Paragraphs
                                                               Where q.Text.Contains("<<noinhan>>")).FirstOrDefault()
                                For Each item As String In ReadAllLines(txtNoiNhan.Text)
                                    If Not String.IsNullOrEmpty(item) Then
                                        Dim pNew As Paragraph = document.InsertParagraph '' Mặc định sẽ insert vào cuối cùng của văn bản
                                        pNew.StyleName = pParagraph.StyleName
                                        pNew.Append(item)
                                        pParagraph.InsertParagraphBeforeSelf(pNew)
                                        '' tìm ra paragraph cuối cùng vào  xóa đi 
                                        Dim pNewEnd = document.Paragraphs.LastOrDefault()
                                        pNewEnd.Remove(False)
                                    End If
                                Next
                                pParagraph.Remove(False)

                                ''Neu pho chanh thanh tra
                                If ddlChucDanh.SelectedValue = 2 Then
                                    document.ReplaceText("<<PhochanhTT>>", "Chánh Thanh tra Sở Lao động – Thương binh Xã hội tỉnh " & ttdn.TenTinh & " kiểm tra việc thực hiện kiến nghị thanh tra và xử lý nếu doanh nghiệp không thực hiện kết luận thanh tra./.")
                                Else
                                    document.ReplaceText("<<PhochanhTT>>", "")
                                End If

                                '' Chức danh
                                document.ReplaceText("<<chucdanh>>", IIf(ddlChucDanh.SelectedValue = "2", "KT.CHÁNH THANH TRA" + Environment.NewLine + "P.CHÁNH THANH TRA", "CHÁNH THANH TRA"))

                                ''Người ký
                                document.ReplaceText("<<nguoiky>>", txtNguoiKy.Text)

                                Dim FileName = "KLTT" & Now.ToString("dd_MM_yyyy_hh_mm") & ".docx"
                                document.SaveAs(Server.MapPath("~/Output").ToString & "\" & FileName)
                                ' Request.Url
                                Dim URL As String = "Output/" & FileName
                                Excute_Javascript("AlertboxRedirect('Phiếu kết luận được xuất thành công.','List.aspx');window.location='../../" + URL + "'; ", Me.Page, True)
                            End Using
                            '' thực hiện lưu lại thông tin về kiến nghị doanh nghiệp
                            data.SaveChanges()
                        End Using
                        sc.Complete()
                    Catch ex As Exception
                        Excute_Javascript("Alertbox('Việc xuất dữ liệu bị lỗi! " & ex.Message & "');", Me.Page, True)
                        sc.Dispose()
                    End Try
                End Using
        End Select
    End Sub
    Protected Sub NhungDieuLamDuoc(ByRef docx As Novacode.DocX)
        Dim pParagraph As Paragraph
        pParagraph = (From q In docx.Paragraphs
                        Where q.Text.Contains("<<dalamdc>>")).FirstOrDefault()
        ' lấy dữ liệu ở đây
        Using data As New ThanhTraLaoDongEntities
            '' Lấy ra dữ liêu là những điều làm được 
            Dim kldathuchien = (From a In data.KetLuans
                                Where a.IsViPham = TypeViPham.KhongViPham And
                                        a.TenCotCauHoi.Equals("Q21") = False And
                                        a.PhieuId = hidID.Value And
                                        String.IsNullOrEmpty(a.NDKetLuan) = False
                                Select a.NDKetLuan).ToList()
            If Not IsNothing(kldathuchien) Then
                For Each item In kldathuchien
                    Dim pNew As Paragraph = docx.InsertParagraph
                    pNew.StyleName = pParagraph.StyleName
                    pNew.Append(item)
                    pParagraph.InsertParagraphBeforeSelf(pNew)
                    ' tìm ra paragraph cuối cùng vào  xóa đi 
                    Dim pNewEnd = docx.Paragraphs.LastOrDefault()
                    pNewEnd.Remove(False)
                Next
            End If
            pParagraph.Remove(False)

            '*************** Xuất mục Q21**************
            kldathuchien = (From a In data.KetLuans Where a.IsViPham = TypeViPham.KhongViPham And a.TenCotCauHoi.Equals("Q21") And a.PhieuId = hidID.Value And String.IsNullOrEmpty(a.NDKetLuan) = False Select a.NDKetLuan).ToList()
            If kldathuchien.Count > 0 Then
                Dim strQ21() As String = {}
                strQ21 = kldathuchien(0).Split("#")
                docx.ReplaceText("<<headdalamdcQ21>>", strQ21(0))
                'TH: Có kết luận dalamdcQ21Sub
                'Khai báo pParagraph với template <<dalamdcQ21Sub>>
                pParagraph = (From q In docx.Paragraphs
                        Where q.Text = "<<dalamdcQ21Sub>>").FirstOrDefault()
                For j As Integer = 1 To strQ21.Count - 1
                    Dim pNewQ21 As Paragraph = docx.InsertParagraph
                    pNewQ21.StyleName = pParagraph.StyleName
                    pNewQ21.Append(strQ21(j))
                    pParagraph.InsertParagraphBeforeSelf(pNewQ21)
                    ' tìm ra paragraph cuối cùng vào  xóa đi 
                    Dim pNewEndQ21 = docx.Paragraphs.LastOrDefault()
                    pNewEndQ21.Remove(False)
                Next
                'Remove template <<dalamdcQ21Sub>>
                pParagraph.Remove(False)
            Else
                docx.ReplaceText("<<headdalamdcQ21>>", "")
                docx.ReplaceText("<<dalamdcQ21Sub>>", "")
            End If
        End Using
    End Sub
    Protected Sub NhungDieuChuaLamDuoc(ByRef docx As Novacode.DocX)
        Dim pParagraph As Paragraph
        pParagraph = (From q In docx.Paragraphs
                        Where q.Text.Contains("<<chualamduoc>>")).FirstOrDefault()
        ' lấy dữ liệu ở đây
        Using data As New ThanhTraLaoDongEntities

            Dim klchuald = (From a In data.KetLuans
                                Where a.IsViPham = TypeViPham.ViPham And a.PhieuId = hidID.Value And String.IsNullOrEmpty(a.NDKetLuan) = False
                                Select a.NDKetLuan).ToList()
            If Not IsNothing(klchuald) Then
                For Each item In klchuald
                    Dim pNew As Paragraph = docx.InsertParagraph
                    pNew.StyleName = pParagraph.StyleName
                    pNew.Append(item)
                    pParagraph.InsertParagraphBeforeSelf(pNew)
                    ' tìm ra paragraph cuối cùng vào  xóa đi 
                    Dim pNewEnd = docx.Paragraphs.LastOrDefault()
                    pNewEnd.Remove(False)
                Next
            End If

            pParagraph.Remove(False)
        End Using
    End Sub
End Class
