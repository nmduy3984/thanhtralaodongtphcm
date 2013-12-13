Imports Excel = Microsoft.Office.Interop.Excel
Imports Microsoft.Office.Interop
Imports ThanhTraLaoDongModel
Imports Cls_Common
Imports SecurityService
Imports System.IO
Imports System.Drawing
Partial Class Control_TraCuu_DS100DoanhNghiep
    Inherits System.Web.UI.UserControl
    Dim arrHeader As String() = {"", "Số người chết ", "người làm việc có yêu cầu nghiêm ngặt", _
                             "Tổng giá trị sản phẩm", "nhuận sau thuế ", "Mức lương trung bình", _
                             "Số lao động chưa thành niên", "Chậm đóng tiền BHXH", "Chưa tham BHXH", "Số vụ đình công"}
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Dim script As ScriptManager = ScriptManager.GetCurrent(Me.Page)
            If Not script Is Nothing AndAlso script.IsInAsyncPostBack Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.Page.GetType, "duyjs", "ajaxJquery()", True)
                ScriptManager.RegisterStartupScript(Me.Page, Me.Page.GetType, "duyjs1", "ajaxJqueryToolTip()", True)
            Else
                Me.Page.ClientScript.RegisterStartupScript(Me.Page.GetType(), "duyjs", String.Concat("Sys.Application.add_load(function(){", "ajaxJquery()", "});"), True)
                Me.Page.ClientScript.RegisterStartupScript(Me.Page.GetType(), "duyjs1", String.Concat("Sys.Application.add_load(function(){", "ajaxJqueryToolTip()", "});"), True)
            End If
            hidTinhThanhTraSo.Value = Session("TinhThanhTraSo")
            getYear()
            LoadType()
        End If
    End Sub

    Protected Sub getYear()

        ddlYearFrom.AppendDataBoundItems = True
        ddlYearFrom.Items.Add(New ListItem("---Tất cả---", "1900"))
        ddlYearFrom.DataSource = Enumerable.Range(2005, 11)
        ddlYearFrom.DataBind()
        ddlYearFrom.SelectedValue = Now.Year


    End Sub

    Protected Sub LoadType()
        Using Data As New ThanhTraLaoDongEntities
            Dim lst = Nothing
            If hidTinhThanhTraSo.Value > 0 Then
                lst = (From q In Data.Tinhs Where q.TinhId = hidTinhThanhTraSo.Value Order By q.TenTinh Ascending
                      Select New With {.Value = q.TinhId, .Text = q.TenTinh})
                With ddlDiaPhuong
                    .Items.Clear()
                    .AppendDataBoundItems = True
                    .DataTextField = "Text"
                    .DataValueField = "Value"
                    .DataSource = lst
                    .DataBind()
                End With
            Else
                lst = (From q In Data.Tinhs Order By q.TenTinh Ascending
                          Select New With {.Value = q.TinhId, .Text = q.TenTinh})
                With ddlDiaPhuong
                    .Items.Clear()
                    .AppendDataBoundItems = True
                    .DataTextField = "Text"
                    .DataValueField = "Value"
                    .DataSource = lst
                    .DataBind()
                    .Items.Insert(0, New ListItem(Cls_Common.Str_Opt_ToanQuoc, "-1"))
                End With
            End If
        End Using
    End Sub

#Region "PRIVATE EVENT FOR CONTROL"

    Private Sub BindToGrid(Optional ByVal iPage As Integer = 1 _
                            , Optional ByVal strTinh As Integer = 0 _
                            , Optional ByVal strYearFrom As Integer = 0 _
                            , Optional ByVal strYearTo As Integer = 0)
        Using data As New ThanhTraLaoDongEntities
            Dim arrSearch() As String = {iPage.ToString, strTinh.ToString, strYearFrom.ToString, strYearTo.ToString}

            ViewState("search") = arrSearch
            'So ban ghi muon the hien tren trang
            Dim intPag_Size As Int32 = drpPage_Size.SelectedValue
            Dim p = data.uspTraCuuTop100(strTinh, strYearFrom, ddlDieuKien.SelectedValue).ToList()
            'Tong so ban ghi
            Dim strKey_Name() As String = {"DoanhNghiepId"}
            'Tong so ban ghi
            If p.Count > 1 Then
                hidCount.Value = p.Count
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

    Private Sub Button_Click(ByVal sender As Object, ByVal em As System.EventArgs) Handles btnExport.Click, btnExportExcel.Click
        Select Case CType(sender, Control).ID
            Case "btnExport"
                Dim intDiaPhuong As Integer = ddlDiaPhuong.SelectedValue
                Dim intYearFrom As Integer = ddlYearFrom.SelectedValue
                BindToGrid(1, intDiaPhuong, intYearFrom, 0)
            Case "btnExportExcel"
                ExportExcel()
        End Select
    End Sub

    Protected Sub btnHuy_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnHuy.Click
        ddlDiaPhuong.SelectedValue = 0
        txtLoaihinhSX.Text = ""
        ddlYearFrom.SelectedIndex = -1

    End Sub
    Protected Sub grdShow_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdShow.RowDataBound

        If (e.Row.RowType = DataControlRowType.Header) Then
            e.Row.Cells(5).Text = arrHeader(ddlDieuKien.SelectedValue)
        End If


    End Sub
    Protected Sub drpPage_Size_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles drpPage_Size.SelectedIndexChanged
        Dim arrSearch() As String
        arrSearch = ViewState("search")
        BindToGrid(1, arrSearch(1), arrSearch(2), arrSearch(3))
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

        Dim arrSearch() As String
        arrSearch = ViewState("search")
        BindToGrid(CInt(lnkTile.ToolTip.ToString), arrSearch(1), arrSearch(2), arrSearch(3))
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

        Dim arrSearch() As String
        arrSearch = ViewState("search")
        BindToGrid(1, arrSearch(1), arrSearch(2), arrSearch(3))

    End Sub
    Protected Sub lnkLast_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkLast.Click
        hidCur_Page.Value = hidCur_Page.Value + 1

        Dim arrSearch() As String
        arrSearch = ViewState("search")
        BindToGrid(1, arrSearch(1), arrSearch(2), arrSearch(3))

    End Sub
#End Region
#Region "Export Excel"
    Private Sub ExportExcel()
        Using data As New ThanhTraLaoDongEntities

            Dim _app As Excel.Application
            Dim _workBook As Excel.Workbook
            Dim identity As String = String.Empty
            Dim pathFile As String = String.Empty
            Dim pathFileTemp As String = String.Empty
            Dim fileNameTemp As String = String.Empty
            Dim FolderPath = Server.MapPath("~/Template").ToString

            'Load dữ liệu        
            Dim intDiaPhuong As Integer = ddlDiaPhuong.SelectedValue
            Dim intYearFrom As Integer = ddlYearFrom.SelectedValue
            Dim p = data.uspTraCuuTop100(intDiaPhuong, intYearFrom, ddlDieuKien.SelectedValue).ToList()
            Try

                identity = Now.ToString("ddMMyyyyHHmmss")
                pathFile = FolderPath & "\DS100DoanhNghiep.xlsx"
                fileNameTemp = "DS100DoanhNghiep" & "_" & identity & ".xlsx"
                pathFileTemp = Server.MapPath("~/Output").ToString & "\" & fileNameTemp

                'Kiểm tra có tồn tại file Template mẫu?
                If File.Exists(pathFile) Then
                    File.Copy(pathFile, pathFileTemp, True)

                    ' Check File Attribute Read-Only and remove if it has
                    Dim tempFileInfo As FileInfo = New FileInfo(pathFileTemp)
                    If (tempFileInfo.Attributes And FileAttributes.ReadOnly) = FileAttributes.ReadOnly Then
                        ' Remove Attribute Read-Only
                        tempFileInfo.Attributes = tempFileInfo.Attributes Xor FileAttribute.ReadOnly
                    End If

                    _app = New Excel.Application
                    ' Open Excel spreadsheet
                    _workBook = _app.Workbooks.Open(pathFileTemp)
                    ' Create a Sheet reflect 
                    Dim _sheet_Detail As Excel.Worksheet = _workBook.Sheets(1)

                    'Xuất nội dung
                    'B1: Đưa nội dung vào một mảng
                    Dim arrRange(p.Count, 6) As Object
                    For pos As Integer = 0 To p.Count - 1
                        Dim col As Integer = 0
                        arrRange(pos, 0) = IIf(IsNothing(p(pos).RowNum), "", p(pos).RowNum)
                        col += 1
                        arrRange(pos, col) = IIf(IsNothing(p(pos).TenDoanhNghiep), "", p(pos).TenDoanhNghiep)
                        col += 1
                        arrRange(pos, col) = IIf(IsNothing(p(pos).TruSoChinh), "", p(pos).TruSoChinh)
                        col += 1
                        arrRange(pos, col) = IIf(IsNothing(p(pos).NgayKetThucPhieu), "", CType(p(pos).NgayKetThucPhieu, Date).Year.ToString)
                        col += 1
                        arrRange(pos, col) = IIf(IsNothing(p(pos).TongSoNhanVien), "0", p(pos).TongSoNhanVien)
                        col += 1
                        arrRange(pos, col) = IIf(IsNothing(p(pos).soluong), "0", p(pos).soluong)
                    Next
                    'B2: Gán mảng vào sheet
                    Dim arrTitle() As String = {"Số người chết", "Số người làm việc có yêu cầu nghiêm ngặt", "Tổng giá trị sản phẩm", "Lợi nhuận sau thuế", "Mức lương trung bình", "Số lao động chưa thành niên", "Số vụ đình công"}
                    If arrRange.Length > 0 Then
                        _sheet_Detail.Range("A1", "A1").Value = "Danh sách 100 doanh nghiệp " + ddlDieuKien.SelectedItem.Text.ToString + " " + ddlDiaPhuong.SelectedItem.Text.ToString
                        _sheet_Detail.Range("F3", "F3").Value = arrTitle(ddlDieuKien.SelectedValue - 1)
                        Dim arrSheet As Excel.Range
                        arrSheet = _sheet_Detail.Range("A4", "F" & +(p.Count + 3).ToString())
                        With arrSheet
                            .Value2 = arrRange
                            .Borders.LineStyle = Excel.XlLineStyle.xlContinuous
                            .Borders(Excel.XlBordersIndex.xlInsideVertical).LineStyle = Excel.XlLineStyle.xlContinuous
                            .Borders(Excel.XlBordersIndex.xlInsideHorizontal).LineStyle = Excel.XlLineStyle.xlContinuous
                            .Borders(Excel.XlBordersIndex.xlEdgeLeft).LineStyle = Excel.XlLineStyle.xlContinuous
                            .Borders(Excel.XlBordersIndex.xlEdgeRight).LineStyle = Excel.XlLineStyle.xlContinuous
                            .Interior.Color = Color.White
                        End With
                    End If
                    _workBook.Save()
                    _workBook.Close()
                    _app.Quit()
                    GC.Collect()
                    KillSpecificExcelFileProcess(_app)
                    Excute_Javascript("Alertbox('Xuất báo cáo thành công.');window.location='../../Output/" + fileNameTemp + "';", Me.Page, True)
                Else
                    Throw New FileNotFoundException("File does not exist ! ", IO.Path.GetFileName(pathFile))
                End If


            Catch ex As Exception
                If Not _app Is Nothing Then
                    _workBook.Close()
                    _app.Quit()
                End If
                ' Xóa file ở thư mục Virtual Data đã coppy từ thư mục template
                If File.Exists(pathFileTemp) Then
                    Kill(pathFileTemp)
                End If
                Throw ex
            End Try
        End Using
    End Sub
#End Region
End Class
