Imports Excel = Microsoft.Office.Interop.Excel
Imports Microsoft.Office.Interop
Imports ThanhTraLaoDongModel
Imports Cls_Common
Imports SecurityService
Imports System.IO
Imports System.Drawing
Partial Class Control_Report_TongHopKetQuaThanhTraLD
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
            lblReportName.Text = "Tổng hợp kết quả thanh tra theo Quyết định thanh tra năm "
            LoadData()
        End If
    End Sub
    Private Sub BindToGrid(Optional ByVal strSoQD As String = "#")
        Try
            Using data As New ThanhTraLaoDongEntities
                Dim q = data.uspBCTongHopKQTTTheoQD(strSoQD).ToList

                'Tạo header cho table 
                'CreateHeader(dt1)
                'Tong so ban ghi
                If Not IsNothing(q) Then
                    With grdShow
                        .DataSource = q
                        .DataBind()
                    End With
                Else
                    With grdShow
                        .DataSource = Nothing
                        .DataBind()
                    End With
                End If
            End Using
        Catch ex As Exception
            Dim str = "Xãy ra lỗi trong lúc thực thi store. " & ex.Message.ToString
            Excute_Javascript("Alertbox('" & str.Replace("'", " ") & "')", Me.Page, True)
        End Try

    End Sub

    Protected Sub grdShow_RowCreated(ByVal sender As Object, ByVal e As GridViewRowEventArgs) Handles grdShow.RowCreated
        'If e.Row.RowType = DataControlRowType.Header Then
        '    Dim HeaderGridRow1 As New GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert)
        '    Dim HeaderGridRow2 As New GridViewRow(1, 0, DataControlRowType.Header, DataControlRowState.Insert)

        '    Dim HeaderCell As New TableCell()

        '    For index As Integer = 1 To 7
        '        Dim header As String = ""
        '        HeaderCell = New TableCell()
        '        Select Case index
        '            Case 1
        '                header = "Stt"
        '            Case 2
        '                header = "Số QĐ"
        '            Case 3
        '                header = "Tỉnh/TP"
        '            Case 4
        '                header = "Số doanh nghiệp Thanh tra"
        '            Case 5
        '                header = "Số kiến nghị"
        '            Case 6
        '                header = "Số DN báo cáo thực hiện"
        '            Case 7
        '                header = "Số biên bản VP"
        '                'Case 8
        '                '    header = "Số tiền (triệu đồng)"
        '                'Case 9
        '                '    header = "Đã nộp"
        '        End Select
        '        HeaderCell = New TableCell()
        '        HeaderCell.RowSpan = 2
        '        HeaderCell.Text = header
        '        HeaderCell.HorizontalAlign = HorizontalAlign.Center
        '        HeaderGridRow1.Cells.Add(HeaderCell)
        '    Next
        '    HeaderCell = New TableCell()
        '    HeaderCell.Text = "Số tiền (triệu đồng)"
        '    HeaderCell.ColumnSpan = 2
        '    'HeaderCell.RowSpan = 1
        '    HeaderCell.HorizontalAlign = HorizontalAlign.Center
        '    HeaderGridRow1.Cells.Add(HeaderCell)
        '    grdShow.Controls(0).Controls.AddAt(0, HeaderGridRow1)


        'HeaderCell = New TableCell()
        'HeaderCell.Text = ""
        'HeaderCell.ColumnSpan = 7
        ''HeaderCell.RowSpan = 2
        'HeaderCell.HorizontalAlign = HorizontalAlign.Center
        'HeaderGridRow2.Cells.Add(HeaderCell)

        'HeaderCell.Text = "Đã phạt"
        'HeaderCell.RowSpan = 1
        'HeaderCell.ColumnSpan = 2
        'HeaderCell.HorizontalAlign = HorizontalAlign.Center
        'HeaderGridRow2.Cells.Add(HeaderCell)

        'HeaderCell.Text = "Đã nộp"
        'HeaderCell.RowSpan = 1
        'HeaderCell.ColumnSpan = 2
        'HeaderCell.HorizontalAlign = HorizontalAlign.Center
        'HeaderGridRow2.Cells.Add(HeaderCell)
        'grdShow.Controls(0).Controls.AddAt(1, HeaderGridRow2)
        'End If
    End Sub
    Private Sub LoadData()
        For i As Integer = 2013 To (Now.Year + 5)
            Dim iTem As New ListItem(i.ToString(), i.ToString())
            ddlNam.Items.Add(iTem)
        Next
        ddlNam.Items.Insert(0, New ListItem(Cls_Common.Str_Opt_Chon, "0"))
    End Sub
    Private Sub load_TreeviewRole()
        Using data As New ThanhTraLaoDongEntities
            Dim nam As String = ddlNam.SelectedValue.ToString
            Dim p = data.uspDanhSachSoQuyetDinhTheoNamThuocTTB(nam).ToList
            chkSoQD.DataSource = p
            chkSoQD.DataTextField = "SoQuyenDinh"
            chkSoQD.DataValueField = "SoQuyenDinh"
            chkSoQD.DataBind()
        End Using
    End Sub

    Protected Sub ddlNam_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlNam.SelectedIndexChanged
        lblReportName.Text = "Tổng hợp kết quả thanh tra theo Quyết định thanh tra năm " & ddlNam.SelectedValue
        load_TreeviewRole()
    End Sub
    Private Sub Button_Click(ByVal sender As Object, ByVal em As System.EventArgs) Handles btnExport.Click, btnExportExcel.Click
        Select Case CType(sender, Control).ID
            Case "btnExport"
                BindToGrid(hidSoQD.Value)
            Case "btnExportExcel"
                ExportExcel()
        End Select

    End Sub
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
            Dim p = data.uspBCTongHopKQTTTheoQD(hidSoQD.Value).ToList
            Try

                identity = Now.ToString("ddMMyyyyHHmmss")
                pathFile = FolderPath & "\TongHopKetQuaThanhTraTheoQuyetDinhThahTra.xlsx"
                fileNameTemp = "TongHopKetQuaThanhTraTheoQuyetDinhThahTra" & "_" & identity & ".xlsx"
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
                    Dim arrRange(p.Count, 9) As Object
                    For pos As Integer = 0 To p.Count - 1
                        arrRange(pos, 0) = pos + 1
                        Dim col As Integer = 1
                        arrRange(pos, col) = IIf(IsNothing(p(pos).SQD), "", p(pos).SQD)
                        col += 1
                        arrRange(pos, col) = IIf(IsNothing(p(pos).TenTinh), "0", p(pos).TenTinh)
                        col += 1
                        arrRange(pos, col) = IIf(IsNothing(p(pos).SoDNTT), "0", CType(p(pos).SoDNTT, Double))
                        col += 1
                        arrRange(pos, col) = IIf(IsNothing(p(pos).SoKN), "0", p(pos).SoKN)
                        col += 1
                        arrRange(pos, col) = IIf(IsNothing(p(pos).SoDNBCTH), "0", CType(p(pos).SoDNBCTH, Double))
                        col += 1
                        arrRange(pos, col) = IIf(IsNothing(p(pos).SoBBVP), "0", p(pos).SoBBVP)
                        col += 1
                        arrRange(pos, col) = IIf(IsNothing(p(pos).DaPhat), "0", CType(p(pos).DaPhat, Double))
                        col += 1
                        arrRange(pos, col) = IIf(IsNothing(p(pos).DaNop), "0", p(pos).DaNop)
                    Next
                    'B2: Gán mảng vào sheet

                    If arrRange.Length > 0 Then
                        _sheet_Detail.Range("A1", "A1").Value = "Tổng hợp kết quả thanh tra theo Quyết định thanh tra năm " + ddlNam.SelectedValue.ToString
                        Dim arrSheet As Excel.Range
                        arrSheet = _sheet_Detail.Range("A5", "I" & +(p.Count + 4).ToString())
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
