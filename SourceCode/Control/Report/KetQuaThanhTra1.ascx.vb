Imports Excel = Microsoft.Office.Interop.Excel
Imports Microsoft.Office.Interop
Imports ThanhTraLaoDongModel
Imports Cls_Common
Imports SecurityService
Imports System.IO
Imports System.Drawing
Partial Class Control_Report_KetQuaThanhTra1
    Inherits System.Web.UI.UserControl

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        ''check function event
        'If Not HasPermission(Function_Name.User, Session("RoleID"), 0, Audit_Type.Create) Then
        '    Excute_Javascript("AlertboxRedirect('" + Str_not_right_to_access + "','" + ResolveUrl("~/Page/Homepage.aspx") + "');", Me.Page, True)
        'End If
    End Sub
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
            txtYearFrom.Text = Now.Year
            txtYearTo.Text = Now.Year
        End If

    End Sub

    Private Sub LoadData()
        Using data As New ThanhTraLaoDongEntities

            If hidTinhThanhTraSo.Value > 0 Then
                Dim lst = (From q In data.Tinhs Where q.TinhId = hidTinhThanhTraSo.Value
                           Order By q.TenTinh Ascending
                            Select New With {.Value = q.TinhId, .Text = q.TenTinh}).ToList
                With ddlProvince
                    .Items.Clear()
                    .AppendDataBoundItems = True
                    .DataTextField = "Text"
                    .DataValueField = "Value"
                    .DataSource = lst
                    .DataBind()
                 End With
                '' Load thông tin mặc định cho Huyện
                Dim tinhId = lst(0).Value
                Dim lsthuyen = (From a In data.Huyens Where a.TinhId = tinhId
                                Order By a.TenHuyen
                                Select New With {.Value = a.HuyenId, .Text = a.TenHuyen}).ToList
                With ddlHuyen
                    .Items.Clear()
                    .AppendDataBoundItems() = True
                    .DataTextField = "Text"
                    .DataValueField = "Value"
                    .DataSource = lsthuyen
                    .DataBind()
                    .Items.Insert(0, New ListItem(Cls_Common.Str_Opt_Chon, 0))
                End With
            Else
                Dim lst = (From q In data.Tinhs
                            Order By q.TenTinh Ascending
                            Select New With {.Value = q.TinhId, .Text = q.TenTinh}).ToList
                With ddlProvince
                    .Items.Clear()
                    .AppendDataBoundItems = True
                    .DataTextField = "Text"
                    .DataValueField = "Value"
                    .DataSource = lst
                    .DataBind()
                    .Items.Insert(0, New ListItem(Cls_Common.Str_Opt_ToanQuoc, "-1"))
                End With
                '' Load thông tin mặc định cho Huyện
                With ddlHuyen
                    .Items.Clear()
                    .AppendDataBoundItems() = True
                    .Items.Insert(0, New ListItem(Cls_Common.Str_Opt_Chon, 0))
                End With
            End If
        End Using
    End Sub

#Region "PRIVATE EVENT FOR CONTROL"
    Private Sub BindToGrid(Optional ByVal strTinh As Integer = -1, Optional ByVal strHuyen As Integer = 0, Optional ByVal strYear_From As Integer = 0, Optional ByVal strYear_To As Integer = 0)
        Using data As New ThanhTraLaoDongEntities
            Dim p = data.usp_Rpt_KetQuaThanhTra1(strTinh, strHuyen, strYear_From, strYear_To).ToList

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
                Dim strTinh As Integer = ddlProvince.SelectedValue
                Dim strHuyen As Integer = ddlHuyen.SelectedValue
                Dim strYear_From As Integer = IIf(String.IsNullOrEmpty(txtYearFrom.Text.Trim()), Now.Year, txtYearFrom.Text.Trim())
                Dim strYear_To As Integer = IIf(String.IsNullOrEmpty(txtYearTo.Text.Trim()), Now.Year, txtYearTo.Text.Trim())
                BindToGrid(strTinh, strHuyen, strYear_From, strYear_To)
            Case "btnExportExcel"
                ExportExcel()
        End Select

    End Sub

    Protected Sub grdShow_RowCreated(ByVal sender As Object, ByVal e As GridViewRowEventArgs) Handles grdShow.RowCreated
        Using data As New ThanhTraLaoDongEntities


            If e.Row.RowType = DataControlRowType.Header Then
                Dim HeaderGridRow1 As New GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert)
                Dim HeaderGridRow2 As New GridViewRow(1, 0, DataControlRowType.Header, DataControlRowState.Insert)

                Dim HeaderCell As New TableCell()
                ''For Header 1
                HeaderCell.Text = "Tiêu Chí"
                HeaderCell.RowSpan = 3
                HeaderCell.CssClass = "GridHeader"
                HeaderCell.HorizontalAlign = HorizontalAlign.Center
                HeaderGridRow1.Cells.Add(HeaderCell)

                HeaderCell = New TableCell()
                HeaderCell.Text = "Loại hình kinh tế"
                HeaderCell.ColumnSpan = 10
                HeaderCell.HorizontalAlign = HorizontalAlign.Center
                HeaderGridRow1.Cells.Add(HeaderCell)
                grdShow.Controls(0).Controls.AddAt(0, HeaderGridRow1)

                '' For Header 2           
                Dim lstLHDN = (From q In data.LoaiHinhDoanhNghieps).ToList
                For index As Integer = 1 To 5
                    Dim header As String = ""
                    Select Case index
                        Case 1
                            header = lstLHDN(index - 1).TenLoaiHinhDN
                        Case 2
                            header = lstLHDN(index - 1).TenLoaiHinhDN
                        Case 3
                            header = lstLHDN(index - 1).TenLoaiHinhDN
                        Case 4
                            header = lstLHDN(index - 1).TenLoaiHinhDN
                        Case 5
                            header = "Tổng"
                    End Select
                    HeaderCell = New TableCell()
                    HeaderCell.Text = header
                    HeaderCell.ColumnSpan = 2
                    HeaderCell.HorizontalAlign = HorizontalAlign.Center
                    HeaderGridRow2.Cells.Add(HeaderCell)
                Next
                grdShow.Controls(0).Controls.AddAt(1, HeaderGridRow2)
            End If
        End Using
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
            Dim strTinh As Integer = ddlProvince.SelectedValue
            Dim strHuyen As Integer = ddlHuyen.SelectedValue
            Dim strYear_From As Integer = IIf(String.IsNullOrEmpty(txtYearFrom.Text.Trim()), Now.Year, txtYearFrom.Text.Trim())
            Dim strYear_To As Integer = IIf(String.IsNullOrEmpty(txtYearTo.Text.Trim()), Now.Year, txtYearTo.Text.Trim())
            Dim p = data.usp_Rpt_KetQuaThanhTra1(strTinh, strHuyen, strYear_From, strYear_To).ToList
            Try

                identity = Now.ToString("ddMMyyyyHHmmss")
                pathFile = FolderPath & "\BangTongHopKetQua_Template.xlsx"
                fileNameTemp = "BangTongHopKetQua" & "_" & identity & ".xlsx"
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
                    Dim arrRange(p.Count, 12) As Object
                    For pos As Integer = 0 To p.Count - 1
                        arrRange(pos, 0) = pos + 1
                        Dim col As Integer = 1
                        arrRange(pos, col) = IIf(IsNothing(p(pos).Name), "", p(pos).Name)
                        col += 1
                        arrRange(pos, col) = IIf(IsNothing(p(pos).Col1), "0", p(pos).Col1)
                        col += 1
                        arrRange(pos, col) = IIf(IsNothing(p(pos).P_Col1), "0", CType(p(pos).P_Col1, Double))
                        col += 1
                        arrRange(pos, col) = IIf(IsNothing(p(pos).Col2), "0", p(pos).Col2)
                        col += 1
                        arrRange(pos, col) = IIf(IsNothing(p(pos).P_Col2), "0", CType(p(pos).P_Col2, Double))
                        col += 1
                        arrRange(pos, col) = IIf(IsNothing(p(pos).Col3), "0", p(pos).Col3)
                        col += 1
                        arrRange(pos, col) = IIf(IsNothing(p(pos).P_Col3), "0", CType(p(pos).P_Col3, Double))
                        col += 1
                        arrRange(pos, col) = IIf(IsNothing(p(pos).Col4), "0", p(pos).Col4)
                        col += 1
                        arrRange(pos, col) = IIf(IsNothing(p(pos).P_Col4), "0", CType(p(pos).P_Col4, Double))
                        col += 1
                        arrRange(pos, col) = IIf(IsNothing(p(pos).Col5), "0", p(pos).Col5)
                        col += 1
                        arrRange(pos, col) = IIf(IsNothing(p(pos).P_Col5), "0", p(pos).P_Col5)
                    Next
                    'B2: Gán mảng vào sheet

                    If arrRange.Length > 0 Then
                        Dim arrSheet As Excel.Range
                        arrSheet = _sheet_Detail.Range("A6", "L" & +(p.Count + 5).ToString())
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
