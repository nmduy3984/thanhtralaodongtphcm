Imports Excel = Microsoft.Office.Interop.Excel
Imports Microsoft.Office.Interop
Imports System.IO
Imports System.Drawing
Imports ThanhTraLaoDongModel
Imports Cls_Common
Imports SecurityService
Partial Class Control_Report_ChiSoThongKeLaoDongToanQuoc
    Inherits System.Web.UI.UserControl
    'Luu nhung MenuId duoc check
    Private IDChecked As String = ""
    Private HeaderReport2 As String = "" ' mỗi giá trị cách nhau bởi dấu #
    Private CountHeader As Integer = 0 ' Chứa giá trị số cột của báo cáo 
#Region "SUB AND FUNCTION"
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
            LoadData()
        End If
    End Sub

    Private Sub LoadData()
        Using data As New ThanhTraLaoDongEntities
            '' Load năm báo cáo
            For i As Integer = 2013 To (Now.Year + 5)
                Dim iTem As New ListItem(i.ToString(), i.ToString())
                ddlNam.Items.Add(iTem)
            Next
            ddlNam.Items.Insert(0, New ListItem(Cls_Common.Str_Opt_Chon, "0"))

            '' Load danh sách tỉnh thành 
            Dim query = Nothing
            If hidTinhThanhTraSo.Value = 0 Then
                query = (From q In data.Tinhs Order By q.TenTinh Ascending Select q).ToList
                trvTinh.Nodes.Clear()
                Dim tnParentRoot As New TreeNode()
                tnParentRoot.Text = "Tất cả"
                tnParentRoot.Value = ""
                tnParentRoot.NavigateUrl = "javascript:void(0)"
                trvTinh.Nodes.Add(tnParentRoot)

                tnParentRoot.ExpandAll()
                For i As Integer = 0 To query.Count - 1
                    Dim tnParent As New TreeNode()
                    tnParent.Text = query(i).TenTinh
                    tnParent.Value = query(i).TinhId.ToString()
                    tnParent.NavigateUrl = "javascript:void(0)"
                    tnParentRoot.ChildNodes.Add(tnParent)
                Next
            End If
        End Using
    End Sub
#End Region
#Region "PRIVATE EVENT FOR CONTROL"
    Private Sub BindToGrid(Optional ByVal strReportType As String = "#", Optional ByVal strReportParams As String = "")
        Using data As New ThanhTraLaoDongEntities
            Dim arrSearch() As String = {strReportType, strReportParams}
            ViewState("search") = arrSearch

            Dim p = data.uspChiSoThongKeLaoDongTheoTinhNam(strReportType, strReportParams).ToList
          
            'Tong so ban ghi
            If Not p Is Nothing Then
                With grdShow
                    .DataSource = p
                    .DataBind()
                End With
            Else
                With grdShow
                    .DataSource = Nothing
                    .DataBind()
                End With
            End If
        End Using
    End Sub

    Private Sub Button_Click(ByVal sender As Object, ByVal em As System.EventArgs) Handles btnExport.Click, btnExportExcel.Click
        Select Case CType(sender, Control).ID
            Case "btnExport"
                Dim StrParam As String = ""
                If hidTinhThanhTraSo.Value = 0 Then
                    For i As Integer = 0 To trvTinh.Nodes(0).ChildNodes.Count - 1
                        If trvTinh.Nodes(0).ChildNodes(i).Checked = True Then
                            StrParam = StrParam + trvTinh.Nodes(0).ChildNodes(i).Value + Str_Symbol_Group
                            HeaderReport2 = HeaderReport2 + trvTinh.Nodes(0).ChildNodes(i).Text + Str_Symbol_Group
                            CountHeader = CountHeader + 1
                        End If
                    Next
                    If StrParam.Length > 1 Then
                        StrParam = StrParam.Substring(0, StrParam.Length - 1).ToString()
                    End If
                Else
                    StrParam = hidTinhThanhTraSo.Value.ToString
                End If
                BindToGrid(ddlNam.SelectedValue, StrParam)
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
            Dim StrParam As String = ""
            If hidTinhThanhTraSo.Value = 0 Then
                For i As Integer = 0 To trvTinh.Nodes(0).ChildNodes.Count - 1
                    If trvTinh.Nodes(0).ChildNodes(i).Checked = True Then
                        StrParam = StrParam + trvTinh.Nodes(0).ChildNodes(i).Value + Str_Symbol_Group
                        HeaderReport2 = HeaderReport2 + trvTinh.Nodes(0).ChildNodes(i).Text + Str_Symbol_Group
                        CountHeader = CountHeader + 1
                    End If
                Next
                If StrParam.Length > 1 Then
                    StrParam = StrParam.Substring(0, StrParam.Length - 1).ToString()
                End If
            Else
                StrParam = hidTinhThanhTraSo.Value.ToString
            End If
            Dim p = data.uspChiSoThongKeLaoDongTheoTinhNam(ddlNam.SelectedValue, StrParam).ToList
            Try

                identity = Now.ToString("ddMMyyyyHHmmss")
                pathFile = FolderPath & "\ChiSoThongKeToanQuoc.xlsx"
                fileNameTemp = "ChiSoThongKeToanQuoc" & "_" & identity & ".xlsx"
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
                    Dim arrRange(p.Count, 5) As Object
                    For pos As Integer = 0 To p.Count - 1
                        Dim col As Integer = 0
                        arrRange(pos, col) = IIf(IsNothing(p(pos).Stt), "", p(pos).Stt)
                        col += 1
                        arrRange(pos, col) = IIf(IsNothing(p(pos).TieuChi), "", p(pos).TieuChi)
                        col += 1
                        arrRange(pos, col) = IIf(IsNothing(p(pos).BienBan), "", String.Format("{0:n2}", CType(p(pos).BienBan, Decimal)))
                        col += 1
                        arrRange(pos, col) = IIf(IsNothing(p(pos).Phieu), "", String.Format("{0:n2}", p(pos).Phieu))
                        col += 1
                        arrRange(pos, col) = IIf(IsNothing(p(pos).Chung), "", String.Format("{0:n2}", p(pos).Chung))
                    Next
                    'B2: Gán mảng vào sheet

                    If arrRange.Length > 0 Then
                        _sheet_Detail.Range("A1", "A1").Value = "Chỉ số thống kê lao động toàn quốc/tỉnh năm " + ddlNam.SelectedValue.ToString
                        Dim arrSheet As Excel.Range
                        arrSheet = _sheet_Detail.Range("A4", "E" & +(p.Count + 3).ToString())
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

#End Region
End Class
