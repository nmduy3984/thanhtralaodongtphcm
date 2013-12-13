
Imports Excel = Microsoft.Office.Interop.Excel
Imports Microsoft.Office.Interop
Imports ThanhTraLaoDongModel
Imports Cls_Common
Imports SecurityService
Imports System.IO
Imports System.Drawing
Partial Class Control_TraCuu_1_DSDNChuaDoMoiTruongLadoDong
    Inherits System.Web.UI.UserControl

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
            txtToDate.Text = Now.Date.ToString("dd/MM/yyyy")
            txtFromDate.Text = Now.Date.AddDays(-31).ToString("dd/MM/yyyy")
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
                            , Optional ByVal strFromDate As Date = #1/1/1900# _
                            , Optional ByVal strToDate As Date = #1/1/3999#)
        Using data As New ThanhTraLaoDongEntities
            Dim arrSearch() As String = {iPage.ToString, strTinh.ToString, strFromDate, strToDate}

            ViewState("search") = arrSearch
            'So ban ghi muon the hien tren trang
            Dim intPag_Size As Int32 = drpPage_Size.SelectedValue
            Dim p = data.uspTraCuuDSDoanhNghiepChuaDoMoiTruong(strTinh, strFromDate, strToDate, iPage, intPag_Size).ToList()
            'Tong so ban ghi
            Dim strKey_Name() As String = {"DoanhNghiepId"}
            'Tong so ban ghi
            If p.Count > 1 Then
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

    Private Sub Button_Click(ByVal sender As Object, ByVal em As System.EventArgs) Handles btnExport.Click, btnExportExcel.Click
        Select Case CType(sender, Control).ID
            Case "btnExport"
                Dim intDiaPhuong As Integer = ddlDiaPhuong.SelectedValue
                Dim intYearFrom As Integer = ddlYearFrom.SelectedValue
                Dim fDate As Date = StringToDate(txtFromDate.Text)
                Dim tDate As Date = StringToDate(IIf(String.IsNullOrEmpty(txtToDate.Text.Trim) = True, "31/12/3999", txtToDate.Text.Trim), "dd/mm/yyyy")

                BindToGrid(1, intDiaPhuong, fDate, tDate)
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
        If e.Row.RowIndex >= 0 Then
      
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


            Dim fDate As Date = StringToDate(txtFromDate.Text)
            Dim tDate As Date = StringToDate(IIf(String.IsNullOrEmpty(txtToDate.Text.Trim) = True, "31/12/3999", txtToDate.Text.Trim), "dd/mm/yyyy")

            Dim p = data.uspTraCuuDSDoanhNghiepChuaDoMoiTruong(intDiaPhuong, fDate, tDate, 1, 999999).ToList()

            Try

                identity = Now.ToString("ddMMyyyyHHmmss")
                pathFile = FolderPath & "\DSDNChuaDoMoiTruongLadoDong.xlsx"
                fileNameTemp = "DSDNChuaDoMoiTruongLadoDong" & "_" & identity & ".xlsx"
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
                    Dim arrRange(p.Count, 8) As Object
                    For pos As Integer = 0 To p.Count - 1
                        Dim col As Integer = 0
                        arrRange(pos, 0) = IIf(IsNothing(p(pos).RowNum), "", p(pos).RowNum)
                        col += 1
                        arrRange(pos, col) = IIf(IsNothing(p(pos).TenDoanhNghiep), "", p(pos).TenDoanhNghiep)
                        col += 1
                        arrRange(pos, col) = IIf(IsNothing(p(pos).TruSoChinh), "", p(pos).TruSoChinh)
                        col += 1
                        arrRange(pos, col) = IIf(IsNothing(p(pos).Email), "", p(pos).Email)
                        col += 1
                        arrRange(pos, col) = IIf(IsNothing(p(pos).DienThoai), "", p(pos).DienThoai)
                        col += 1
                        arrRange(pos, col) = ""
                        col += 1
                        arrRange(pos, col) = ""
                        col += 1
                        arrRange(pos, col) = ""
                     Next
                    'B2: Gán mảng vào sheet

                    If arrRange.Length > 0 Then
                        _sheet_Detail.Range("A1", "A1").Value = "Danh sách doanh nghiệp chưa đo môi trường lao động năm " + ddlYearFrom.SelectedValue.ToString
                        Dim arrSheet As Excel.Range
                        arrSheet = _sheet_Detail.Range("A4", "H" & +(p.Count + 3).ToString())
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
