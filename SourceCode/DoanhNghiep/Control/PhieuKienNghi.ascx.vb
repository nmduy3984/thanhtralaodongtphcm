Imports Novacode
Imports ThanhTraLaoDongModel
Imports System.IO
Imports Cls_Common
Imports SecurityService
Partial Class Control_CauHoi_PhieuKienNghi
    Inherits System.Web.UI.UserControl
    Private Shared ReadOnly log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
#Region "Sub and Function"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsNothing(Request.QueryString("phieuId")) Then
            hidID.Value = Request.QueryString("phieuId")
        End If
        If Not IsPostBack Then
            Dim script As ScriptManager = ScriptManager.GetCurrent(Me.Page)
            If Not script Is Nothing AndAlso script.IsInAsyncPostBack Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.Page.GetType, "duyjs", "ajaxJquery()", True)
            Else
                Me.Page.ClientScript.RegisterStartupScript(Me.Page.GetType(), "duyjs", String.Concat("Sys.Application.add_load(function(){", "ajaxJquery()", "});"), True)
            End If
            BindToGrid()
        End If
    End Sub
    Private Sub BindToGrid(Optional ByVal iPage As Integer = 1, Optional ByVal strSearch As String = "")
        Using data As New ThanhTraLaoDongEntities
            'So ban ghi muon the hien tren trang
            Dim intPag_Size As Int32 = drpPage_Size.SelectedValue
            Dim p As New List(Of uspDMKienNghi_Result)
            p = data.uspDMKienNghi(hidID.Value, iPage, intPag_Size).ToList()
            Dim strKey_Name() As String = {"NDKienNghi"}
            'Tong so ban ghi
            If p.Count > 0 Then
                hidCount.Value = p.FirstOrDefault.Total()
                Create_Pager(hidCount.Value, iPage, intPag_Size, 10)
            Else
                hidCount.Value = 0
                With rptPage
                    .DataSource = Nothing
                    .DataBind()
                End With
            End If
            With grdShow
                .DataKeyNames = strKey_Name
                .DataSource = p
                .DataBind()
            End With
            If (hidCount.Value > 0) Then
                lblTotal.Text = "Hiển thị " + (((iPage - 1) * intPag_Size) + 1).ToString("#,#") + " đến " + (((iPage - 1) * intPag_Size) + grdShow.Rows.Count).ToString("#,#") + " trong tổng số " + CInt(hidCount.Value).ToString("#,#") + " bản ghi."
            Else
                lblTotal.Text = ""
            End If
        End Using
    End Sub
    Sub Create_Pager(ByVal Total_Record As Integer, ByVal Page_Index As Integer, ByVal Page_Size As Integer, ByVal Page2Show As Integer)
        Dim TotalPage As Integer = IIf((Total_Record Mod Page_Size) = 0, Total_Record / Page_Size, Total_Record \ Page_Size + 1)
        'lu lai tong so ban ghi
        hidIndex_page.Value = TotalPage
        'gan lai curPage de set active
        hidCur_Page.Value = Page_Index
        'generate ra left page
        Dim cPageGenerate_left As IEnumerable(Of Integer)
        If Page_Index <= Page2Show Then
            cPageGenerate_left = Enumerable.Range(1, Page_Index)
        Else
            cPageGenerate_left = Enumerable.Range(Page_Index - Page2Show, Page2Show)
        End If
        'generate ra right page
        Dim cPageGenerate_Right As IEnumerable(Of Integer)
        If Page_Index + Page2Show <= TotalPage Then
            cPageGenerate_Right = Enumerable.Range(Page_Index, Page2Show + 1)
        Else
            cPageGenerate_Right = Enumerable.Range(Page_Index, TotalPage - Page_Index + 1)
        End If
        'union 2 range va bind to Grid
        With rptPage
            .DataSource = cPageGenerate_left.Union(cPageGenerate_Right)
            .DataBind()
        End With
    End Sub
    Protected Sub rptPage_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles rptPage.ItemDataBound
        Dim lnkTitle As LinkButton
        lnkTitle = e.Item.FindControl("lnkTitle")
        Dim ScriptManager As System.Web.UI.ScriptManager = System.Web.UI.ScriptManager.GetCurrent(Me.Page)
        ScriptManager.RegisterAsyncPostBackControl(lnkTitle)
        If e.Item.DataItem = hidCur_Page.Value Then
            lnkTitle.Text = "<span class='current'>" & e.Item.DataItem & "</span>"
        Else
            lnkTitle.Text = "<span>" & e.Item.DataItem & "</span>"
        End If
        lnkTitle.ToolTip = e.Item.DataItem
    End Sub
    Protected Sub lnkTitle_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim lnkTile As LinkButton = CType(sender, LinkButton)
        BindToGrid(lnkTile.ToolTip)
        lnkLast.Enabled = True
        lnkFirst.Enabled = True
        If CInt(lnkTile.ToolTip) = hidIndex_page.Value Then
            lnkLast.Enabled = False
        ElseIf CInt(lnkTile.ToolTip) = 1 Then
            lnkFirst.Enabled = False
        End If
    End Sub
    Protected Sub lnkFirst_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkFirst.Click
        If hidCur_Page.Value > 1 Then
            hidCur_Page.Value = hidCur_Page.Value - 1
        End If
        BindToGrid(hidCur_Page.Value)
    End Sub
    Protected Sub lnkLast_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkLast.Click
        hidCur_Page.Value = hidCur_Page.Value + 1
        BindToGrid(hidCur_Page.Value)
    End Sub
#End Region
#Region "Event for control"

    Protected Sub grdShow_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdShow.RowDataBound
        If e.Row.RowIndex >= 0 Then
            
            Dim lblNoidungkn As Label = CType(e.Row.FindControl("lblNoidungkn"), Label)
            lblNoidungkn.Text = grdShow.DataKeys(e.Row.RowIndex)("NDKienNghi").ToString
        End If
    End Sub
#End Region
#Region "Search"

#End Region

    Protected Sub btnInPhieuKN_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnInPhieuKN.Click
        Try
            Dim FolderPath = Server.MapPath("~/Template").ToString
            If Not IsNothing(Session("IsUser")) AndAlso Session("IsUser") = 2 Then
                Dim attributes As FileAttributes = File.GetAttributes(FolderPath & "\PKN_Template_DN.docx")
                If (attributes And FileAttributes.ReadOnly) = FileAttributes.ReadOnly Then
                    attributes = attributes And Not FileAttributes.ReadOnly
                    File.SetAttributes(FolderPath & "\PKN_Template_DN.docx", attributes)
                End If
                Using document As DocX = DocX.Load(FolderPath & "\PKN_Template_DN.docx")
                    Using data As New ThanhTraLaoDongEntities
                        Dim dn = (From a In data.DoanhNghieps,
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
                                                   a.IsCongDoan,
                                                   d.YKienCuaDN,
                                                   d.SoQuyenDinh,
                                                    a.Huyen.TenHuyen,
                                                   b.TenTinh,
                                                   b.TenSo,
                                                   c.TenLoaiHinhDN}).FirstOrDefault

                        ''''Cơ quan ban hành
                        document.ReplaceText("<<coquan1>>", "SỞ LAO ĐỘNG - THƯƠNG BINH" & Environment.NewLine & " VÀ XÃ HỘI " & dn.TenTinh.ToUpper())
                        document.ReplaceText("<<coquan2>>", "THANH TRA SỞ")
                        ''<<nam>>
                        document.ReplaceText("<<nam>>", Now.Year.ToString)

                        ''''ngày in phiếu
                        document.ReplaceText("<<ngayinphieu>>", dn.TenTinh & ", ngày " & Now.Day.ToString & " tháng " & Now.Month.ToString & " năm " & Now.Year.ToString)

                        ''''tên doanh nghiệp
                        document.ReplaceText("<<tenDN>>", dn.TenDoanhNghiep)

                        ''''Nội dung
                        Dim pParagraph As Paragraph
                        pParagraph = (From q In document.Paragraphs
                                        Where q.Text.Contains("<<noidung>>")).FirstOrDefault()
                        Dim p As List(Of uspListKienNghiDNByPhieuId_Result) = data.uspListKienNghiDNByPhieuId(hidID.Value).ToList()
                        For i As Integer = 0 To p.Count - 1
                            Dim pNew As Paragraph = document.InsertParagraph
                            pNew.StyleName = pParagraph.StyleName
                            pNew.Append(p(i).NDKienNghi)
                            pParagraph.InsertParagraphBeforeSelf(pNew)
                            ' tìm ra paragraph cuối cùng vào  xóa đi 
                            Dim pNewEnd = document.Paragraphs.LastOrDefault()
                            pNewEnd.Remove(False)
                        Next
                        pParagraph.Remove(False)

                        ''thongtinTT
                        document.ReplaceText("<<thongtinTT>>", IIf(IsNothing(dn.TenSo), "", dn.TenSo))

                    End Using
                    'Xuat BBTT ra file word
                    Dim FileName = "PKN_" & Now.ToString("dd_MM_yyyy_hh_mm") & "_DN.docx"
                    ' Request.Url
                    Dim URL As String = "Output/" & FileName
                    document.SaveAs(Server.MapPath("~/Output").ToString & "\" & FileName)
                    Dim pageReturn = ""
                    If Session("IsUser") = 2 Then
                        pageReturn = "ListPhieuKienNghi.aspx"
                    Else
                        pageReturn = "List.aspx"
                    End If
                    Excute_Javascript("AlertboxRedirect('In phiếu kiến nghị thành công.','" & pageReturn & "');window.location='../../" + URL + "'; ", Me.Page, True)
                End Using
            Else
                Dim attributes As FileAttributes = File.GetAttributes(FolderPath & "\PKN_Template_TT.docx")
                If (attributes And FileAttributes.ReadOnly) = FileAttributes.ReadOnly Then
                    attributes = attributes And Not FileAttributes.ReadOnly
                    File.SetAttributes(FolderPath & "\PKN_Template_TT.docx", attributes)
                End If
                Using document As DocX = DocX.Load(FolderPath & "\PKN_Template_TT.docx")
                    Using data As New ThanhTraLaoDongEntities
                        Dim dn = (From a In data.DoanhNghieps,
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
                                                   a.IsCongDoan,
                                                   d.YKienCuaDN,
                                                   d.SoQuyenDinh,
                                                    a.Huyen.TenHuyen,
                                                   b.TenTinh,
                                                   c.TenLoaiHinhDN}).FirstOrDefault

                        ''''Cơ quan ban hành
                        document.ReplaceText("<<coquan1>>", "SỞ LAO ĐỘNG - THƯƠNG BINH" & Environment.NewLine & " VÀ XÃ HỘI " & dn.TenTinh.ToUpper())
                        document.ReplaceText("<<coquan2>>", "THANH TRA SỞ")
                        ''Phạm vi
                        document.ReplaceText("<<tinh>>", dn.TenTinh)

                        ''''ngày in phiếu
                        document.ReplaceText("<<ngayinphieu>>", dn.TenTinh & ", ngày " & Now.Day.ToString & " tháng " & Now.Month.ToString & " năm " & Now.Year.ToString)

                        ''''tên doanh nghiệp
                        document.ReplaceText("<<tenDN>>", dn.TenDoanhNghiep)

                        ''''địa chỉ
                        document.ReplaceText("<<diachi>>", dn.TruSoChinh + ", huyện " & dn.TenHuyen & ", " + dn.TenTinh)

                        ''''Nội dung
                        Dim pParagraph As Paragraph
                        pParagraph = (From q In document.Paragraphs
                                        Where q.Text.Contains("<<noidung>>")).FirstOrDefault()
                        Dim p As List(Of uspListKienNghiDNByPhieuId_Result) = data.uspListKienNghiDNByPhieuId(hidID.Value).ToList()
                        For i As Integer = 0 To p.Count - 1
                            Dim pNew As Paragraph = document.InsertParagraph
                            pNew.StyleName = pParagraph.StyleName
                            pNew.Append(p(i).NDKienNghi)
                            pParagraph.InsertParagraphBeforeSelf(pNew)
                            ' tìm ra paragraph cuối cùng vào  xóa đi 
                            Dim pNewEnd = document.Paragraphs.LastOrDefault()
                            pNewEnd.Remove(False)
                        Next
                        pParagraph.Remove(False)

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

                        ''Người ký
                        document.ReplaceText("<<nguoiky>>", txtNguoiKy.Text.Trim)
                    End Using
                    'Xuat BBTT ra file word
                    Dim FileName = "PKN_" & Now.ToString("dd_MM_yyyy_hh_mm") & "_TT.docx"
                    ' Request.Url
                    Dim URL As String = "Output/" & FileName
                    document.SaveAs(Server.MapPath("~/Output").ToString & "\" & FileName)
                    Dim pageReturn = ""
                    If Session("IsUser") = 2 Then
                        pageReturn = "ListPhieuKienNghi.aspx"
                    Else
                        pageReturn = "List.aspx"
                    End If
                    Excute_Javascript("AlertboxRedirect('In phiếu kiến nghị thành công.','" & pageReturn & "');window.location='../../" + URL + "'; ", Me.Page, True)
                End Using
            End If
        Catch ex As Exception
            Excute_Javascript("Alertbox('Việc xuất dữ liệu bị lỗi! " & ex.Message & "');", Me.Page, True)
            log4net.Config.XmlConfigurator.Configure()
            log.Error("Error error " & AddTabSpace(1) & Session("Username") & AddTabSpace(1) & "IP:" & GetIPAddress(), ex)
        End Try
    End Sub

    Protected Sub drpPage_Size_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles drpPage_Size.SelectedIndexChanged
        BindToGrid()
    End Sub
End Class
