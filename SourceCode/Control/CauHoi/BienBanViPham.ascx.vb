Imports System.Data
Imports System.Transactions
Imports Cls_Common
Imports SecurityService
Imports ThanhTraLaoDongModel
Imports Novacode
Imports System.IO
Partial Class Control_CauHoi_BienBanViPham
    Inherits System.Web.UI.UserControl
    Private Shared ReadOnly log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
    Private lstHanhViID() As String
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
                LoadData()
            Else
                Response.Redirect("../../Login.aspx")
            End If
        End If
    End Sub
    Protected Sub LoadData()

        Using data As New ThanhTraLaoDongEntities
            Dim dn = (From a In data.PhieuNhapHeaders.Include("DoanhNghiep") Where a.PhieuID = hidID.Value Select a.DoanhNghiep, a.SoQuyenDinh).FirstOrDefault
            Dim strSoQD As String = ""
            If dn.SoQuyenDinh.Split("/").Length > 1 Then
                strSoQD = dn.SoQuyenDinh.Split("/")(0)
            End If
            txtCQBH.Text = "THANH TRA BỘ LĐTBXH" & vbNewLine & "ĐOÀN THANH TRA " & vbNewLine & strSoQD & "/QĐ-TTr"
            txtPV.Text = "Hôm nay, hồi " & Now.Hour & " giờ " & Now.Minute & " ngày " & Now.Day & " tháng " & Now.Month & " năm " & Now.Year & " tại " & dn.DoanhNghiep.TenDoanhNghiep
            txtDiaChiTT.Text = dn.DoanhNghiep.Tinh.MoTa
            ''Load thông tin Người đại diện hợp pháp, Người lập biên bản, Người chứng kiến
            Dim pnh As PhieuNhapHeader = (From a In data.PhieuNhapHeaders Where a.PhieuID = hidID.Value).FirstOrDefault
            If Not IsNothing(pnh.ThanhPhanThamGia) Then
                If Not IsNothing(pnh.DaiDienDoanhNghiep) Then
                    Dim strNguoiDaiDienHopPhap() As String = pnh.DaiDienDoanhNghiep.Split("#")(0).Split("-")(0).Split(";")(0).Split(" ")
                    For i As Integer = 1 To strNguoiDaiDienHopPhap.Count - 1
                        txtNguoiDaiDienHopPhap.Text += strNguoiDaiDienHopPhap(i) + " "
                    Next
                End If
                If Not IsNothing(pnh.ThanhPhanThamGia) Then
                    Dim strNguoiLapBB() As String = pnh.ThanhPhanThamGia.Split("#")(0).Split("-")(0).Split(";")(0).Split(" ")
                    For i As Integer = 1 To strNguoiLapBB.Count - 1
                        txtNguoiLapBB.Text += strNguoiLapBB(i) + " "
                    Next
                    'Dim strNguoiChungKien() As String = pnh.ThanhPhanThamGia.Split("#")(2).Split("-")(0).Split(" ")
                    'For i As Integer = 1 To strNguoiChungKien.Count - 1
                    '    txtNguoiChungKien.Text += strNguoiChungKien(i) + " "
                    'Next
                End If
            End If
            
            ''Load danh sach hanh vi vi pham
            Dim lsHV = (From q In data.DanhMucHanhVis, a In data.vLoaiHanhVis, b In data.HanhViDNs
                                    Where a.Id = q.LoaiHanhVi And q.HanhViId = b.HanhViId And b.PhieuId = hidID.Value
                                    Order By q.HanhViId Descending
                                    Select New With {q.HanhViId, q.Title, q.MucPhatMax, q.MucPhatMin, .LoaiHanhVi = a.Name}).ToList()
            If Not IsNothing(lsHV) Then
                Dim strKey_Name() As String = {"HanhViId", "MucPhatMin", "MucPhatMax"}

                With grdShow
                    .DataKeyNames = strKey_Name
                    .DataSource = lsHV
                    .DataBind()
                End With
            End If
        End Using
    
    End Sub
    Protected Sub hpKienNghi_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles hpKienNghi.Init
        hpKienNghi.CssClass = "thickbox"
        hpKienNghi.NavigateUrl = "../../Page/HanhVi/PopupHanhVi.aspx?keepThis=true&TB_iframe=true&height=520&width=1000&modal=true"
    End Sub
    Protected Sub grdShow_RowCreated(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdShow.RowCreated
        If e.Row.RowType = DataControlRowType.Footer Then
            Dim HeaderGridRow1 As New GridViewRow(0, 0, DataControlRowType.Footer, DataControlRowState.Insert)
            Dim HeaderCell As New TableCell()
            'Footer column 1
            HeaderCell.Text = "Tổng cộng"
            HeaderCell.ColumnSpan = 3
            HeaderCell.CssClass = "GridHeader"
            HeaderCell.HorizontalAlign = HorizontalAlign.Center
            HeaderGridRow1.Cells.Add(HeaderCell)

            'Footer column 2
            'Tính tổng
            
            HeaderCell = New TableCell()
            HeaderCell.Text = ""
            HeaderCell.ColumnSpan = 1
            HeaderCell.CssClass = "GridHeader SumClass"
            HeaderCell.HorizontalAlign = HorizontalAlign.Center
            HeaderGridRow1.Cells.Add(HeaderCell)
            grdShow.Controls(0).Controls.AddAt(grdShow.Controls(0).Controls.Count, HeaderGridRow1)

        End If
    End Sub
    Protected Sub grdShow_RowDataBound(ByVal sender As Object, ByVal e As WebControls.GridViewRowEventArgs) Handles grdShow.RowDataBound
        If e.Row.RowIndex >= 0 Then
            Dim lblSTT As Label = CType(e.Row.FindControl("lblSTT"), Label)
            lblSTT.Text = e.Row.RowIndex + 1
            Dim txtMucPhatDeNghi As TextBox = CType(e.Row.FindControl("txtMucPhatDeNghi"), TextBox)
            txtMucPhatDeNghi.Text = String.Format("{0:n0}", (grdShow.DataKeys(e.Row.RowIndex)("MucPhatMin") + grdShow.DataKeys(e.Row.RowIndex)("MucPhatMax")) / 2)

        End If
    End Sub
    Private Sub BindToGrid()
        lstHanhViID = Strings.Split(hidHanhViID.Value, Str_Symbol_Group)
        Using data As New ThanhTraLaoDongEntities
            Dim lsHV = (From q In data.DanhMucHanhVis, a In data.vLoaiHanhVis
                        Where a.Id = q.LoaiHanhVi And lstHanhViID.Contains(q.HanhViId) Order By q.HanhViId Descending Select New With {q.HanhViId, q.Title, q.MucPhatMax, q.MucPhatMin, .LoaiHanhVi = a.Name}).ToList()

            Dim strKey_Name() As String = {"HanhViId", "Title", "MucPhatMin", "MucPhatMax"}
            With grdShow
                .DataKeyNames = strKey_Name
                .DataSource = lsHV
                .DataBind()
            End With
        End Using

    End Sub
    Protected Sub Button_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSubmit.Click, btnExport.Click
        Select Case CType(sender, Control).ID
            Case "btnSubmit"
                BindToGrid()
            Case "btnExport"
                InBaoCaoHanhVi()
        End Select
    End Sub
    Private Sub InBaoCaoHanhVi()
        Try
            Dim FolderPath = Server.MapPath("~/Template").ToString
            Dim attributes As FileAttributes = File.GetAttributes(FolderPath & "\BBVP_Template.docx")
            If (attributes And FileAttributes.ReadOnly) = FileAttributes.ReadOnly Then
                attributes = attributes And Not FileAttributes.ReadOnly
                File.SetAttributes(FolderPath & "\BBVP_Template.docx", attributes)
            End If
            Using document As DocX = DocX.Load(FolderPath & "\BBVP_Template.docx")

                Using data As New ThanhTraLaoDongEntities
                    Dim iPhieuId = CInt(Request.QueryString("phieuId"))
                    Dim dn = (From a In data.DoanhNghieps,
                                   b In data.Tinhs,
                                   c In data.LoaiHinhDoanhNghieps,
                                   d In data.PhieuNhapHeaders
                              Where
                                    a.TinhId = b.TinhId And
                                    a.LoaiHinhDNId = c.LoaiHinhDNId And
                                    a.DoanhNghiepId = d.DoanhNghiepId And
                                    d.PhieuID = iPhieuId
                              Select New With {a.DoanhNghiepId,
                                                a.TenDoanhNghiep,
                                               a.KhuCongNghiep,
                                               a.NamTLDN,
                                               a.DienThoai,
                                               a.Fax,
                                               a.LoaiHinhSanXuat.Title,
                                               a.SoChungNhanDKKD,
                                               a.NgayChungNhanDKKD,
                                               a.LanThayDoi,
                                               a.NgayThayDoi,
                                               a.TruSoChinh,
                                               a.IsCongDoan,
                                               a.SoTKNganHang,
                                               a.TenNganHang,
                                               d.YKienCuaDN,
                                               b.TenTinh,
                                               b.MoTa,
                                               a.Huyen.TenHuyen,
                                               d.SoQuyenDinh,
                                               c.TenLoaiHinhDN}).FirstOrDefault
                    ''''Cơ quan ban hành
                    document.ReplaceText("<<cqhc>>", "THANH TRA BỘ LĐTBXH" & vbNewLine & "ĐOÀN THANH TRA ")
                    ''So quyet dinh
                    Dim strSoQD() As String = dn.SoQuyenDinh.Split("/")
                    document.ReplaceText("<<soQDTT>>", strSoQD(0) & "/QĐ-TTr")
                    ''''Tỉnh
                    document.ReplaceText("<<tinh>>", dn.TenTinh & "," + " ngày " + Now.Day.ToString() + " tháng " + Now.Month.ToString() + " năm " + Now.Year.ToString())
                    ''''Phạm vi
                    document.ReplaceText("<<phamvi>>", txtPV.Text)

                    ''Chúng tôi gồm:
                    document.ReplaceText("<<tenthanhtra>>", txtNguoiLapBB.Text)
                    document.ReplaceText("<<chucvuthanhtra>>", txtChucVuNguoiLapBB.Text)
                    ''Với sự chứng kiến của 
                    document.ReplaceText("<<tennguoichungkien>>", txtNguoiChungKien.Text)
                    document.ReplaceText("<<chucvunguoichungkien>>", txtChucVuNguoiChungKien.Text)

                    'Thành phần Đoàn thanh tra
                    Dim pParagraph As Paragraph

                    ''''THÔNG TIN CHUNG VỀ DOANH NGHIỆP
                    pParagraph = (From q In document.Paragraphs
                                    Where q.Text.Contains("<<doanhnghiep>>")).FirstOrDefault()
                    For i As Integer = 1 To 5
                        Dim pNewDN As Paragraph = document.InsertParagraph
                        pNewDN.StyleName = pParagraph.StyleName
                        Select Case i
                            Case 1
                                pNewDN.Append(dn.TenDoanhNghiep & " do " & txtNguoiDaiDienHopPhap.Text & " là người đại diện theo pháp luật")
                            Case 2
                                pNewDN.Append("Trụ sở chính: " & dn.TruSoChinh & " " & dn.KhuCongNghiep & ", " & dn.TenHuyen & ", " & dn.TenTinh)
                            Case 3
                                Dim strtemp = "Giấy chứng nhận đầu tư số " & dn.SoChungNhanDKKD & " do ban quản lí các khu chế xuất và công nghiệp " & dn.TenTinh & " cấp ngày " & CType(dn.NgayChungNhanDKKD, Date).ToString("dd/MM/yyyy")
                                If Not IsNothing(dn.LanThayDoi) AndAlso dn.LanThayDoi > 0 Then
                                    strtemp = strtemp & ", thay đổi lần thứ " & dn.LanThayDoi & " ngày " & CType(dn.NgayThayDoi, Date).ToString("dd/MM/yyyy") + "."
                                Else
                                    strtemp = strtemp & "."
                                End If
                                pNewDN.Append(strtemp)
                            Case 4
                                pNewDN.Append("Ngành nghề sản xuất kinh doanh chính: " & dn.Title)
                            Case 5
                                pNewDN.Append("Số tài khoản: " + dn.SoTKNganHang + "                 tại: " + dn.TenNganHang)

                        End Select
                        pParagraph.InsertParagraphBeforeSelf(pNewDN)
                        ' tìm ra paragraph cuối cùng vào  xóa đi 
                        Dim pNewEnd = document.Paragraphs.LastOrDefault()
                        pNewEnd.Remove(False)
                    Next
                    pParagraph.Remove(False)

                    '*************************Nội dung hành vi ***************************
                    data.ExecuteStoreCommand("Delete From HanhViDN where phieuID= " & hidID.Value)
                    pParagraph = (From q In document.Paragraphs
                                   Where q.Text.Contains("<<noidung>>")).FirstOrDefault()
                    lstHanhViID = hidHanhViID.Value.Split("#")
                    Dim lstHVID = (From a In data.DanhMucHanhVis Where lstHanhViID.Contains(a.HanhViId) Order By a.HanhViId Descending).ToList()
                    Dim sumMucPhat As Integer = 0
                    For index As Integer = 0 To lstHVID.Count - 1
                        Dim hvDN As New HanhViDN
                        hvDN.PhieuId = hidID.Value
                        hvDN.HanhViId = lstHVID(index).HanhViId
                        hvDN.MucPhat = CType(grdShow.Rows(index).FindControl("txtMucPhatDeNghi"), TextBox).Text
                        sumMucPhat = sumMucPhat + hvDN.MucPhat
                        hvDN.DoanhNghiepID = dn.DoanhNghiepId
                        data.HanhViDNs.AddObject(hvDN)

                        '' Thêm dữ liệu vào document
                        Dim pNew As Paragraph = document.InsertParagraph '' Mặc định sẻ insert vào cuối cùng của văn bản
                        pNew.StyleName = pParagraph.StyleName
                        pNew.Append(lstHVID(index).Title)
                        pParagraph.InsertParagraphBeforeSelf(pNew)
                        '' tìm ra paragraph cuối cùng vào  xóa đi 
                        Dim pNewEnd = document.Paragraphs.LastOrDefault()
                        pNewEnd.Remove(False)
                    Next
                    Dim pn As PhieuNhapHeader = (From a In data.PhieuNhapHeaders Where a.PhieuID = iPhieuId).FirstOrDefault
                    If Not IsNothing(pn) Then
                        pn.TienPhatDuKien = sumMucPhat
                    End If

                    data.SaveChanges()
                    pParagraph.Remove(False)

                    ''''Ý kiến trình bày của người vi phạm /đại diện tổ chức vi phạm hành chính
                    document.ReplaceText("<<ykiennguoivipham>>", txtYKienNguoiViPham.Text)

                    ''''ykiennguoilamchung
                    If String.Equals(txtBienPhap.Text, "") Then
                        document.ReplaceText("<<nhattri>>", "Nhất trí")
                        document.ReplaceText("<<ykiennguoilamchung>>", "")
                    Else
                        document.ReplaceText("<<ykiennguoilamchung>>", txtYKienNguoiLamChung.Text)
                        document.ReplaceText("<<nhattri>>", "")
                    End If

                    ''''ykiennguoibihai
                    document.ReplaceText("<<ykiennguoibihai>>", txtYKienNguoiBiHai.Text)

                    ''''nguoilapbienban
                    document.ReplaceText("<<nguoilapbienban>>", txtNguoiLapBB.Text)

                    ''''nguoichungkien
                    document.ReplaceText("<<nguoichungkien>>", txtNguoiChungKien.Text)

                    ''''Các biện pháp ngăn chặn vi phạm hành chính được áp dụng gồm
                    If String.Equals(txtBienPhap.Text, "") Then
                        document.ReplaceText("<<khongco>>", "không có")
                        document.ReplaceText("<<bienphap>>", "")
                    Else
                        document.ReplaceText("<<bienphap>>", txtBienPhap.Text)
                        document.ReplaceText("<<khongco>>", "")
                    End If


                    ''Thông tin địa chỉ thanh tra sở
                    document.ReplaceText("<<thongtinTT>>", dn.TenTinh + " (" + txtDiaChiTT.Text + ") " + "lúc  giờ  ngày  tháng  năm " + Now.Year.ToString() + " để giải quyết vụ vi phạm")
                    document.ReplaceText("<<tinhTT>>", dn.TenTinh)
                End Using
                'Xuat BBTT ra file word
                Dim FileName = "BBVP_" & Now.ToString("dd_MM_yyyy_hh_mm") & ".docx"
                ' Request.Url
                Dim URL As String = "Output/" & FileName
                document.SaveAs(Server.MapPath("~/Output").ToString & "\" & FileName)
                Excute_Javascript("AlertboxRedirect('In biên bản vi phạm hành chính thành công.','List.aspx');window.location='../../" + URL + "'; ", Me.Page, True)

            End Using

        Catch ex As Exception
            Excute_Javascript("Alertbox('Việc xuất dữ liệu bị lỗi! " & ex.Message & "');", Me.Page, True)
            log4net.Config.XmlConfigurator.Configure()
            log.Error("Error error " & AddTabSpace(1) & Session("Username") & AddTabSpace(1) & "IP:" & GetIPAddress(), ex)
        End Try
    End Sub
End Class
