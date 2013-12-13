Imports Excel = Microsoft.Office.Interop.Excel
Imports Microsoft.Office.Interop
Imports System.Data.EntityClient
Imports System.Data
Imports ThanhTraLaoDongModel
Imports Cls_Common
Imports SecurityService
Imports System.Data.SqlClient
Imports System.IO
Imports System.Drawing
Partial Class Control_Report_BangTongHopViPham
    Inherits System.Web.UI.UserControl

    'Luu nhung MenuId duoc check
    Private IDChecked As String = ""
    Private HeaderReport2 As String = "" ' mỗi giá trị cách nhau bởi dấu #
    Private CountHeader As Integer = 0 ' Chứa giá trị số cột của báo cáo 
    'Lưu dữ liệu từ store để xuất báo cáo web và xuất báo cáo excel

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
            LoadUser()
            LoadData()
        End If
    End Sub
    Private Sub LoadUser()
        Using data As New ThanhTraLaoDongEntities
            Dim u = (From a In data.Users Select New With {a.UserId, a.UserName}).ToList
            With ddlNguoiDung
                .Items.Clear()
                .AppendDataBoundItems = True
                .DataTextField = "UserName"
                .DataValueField = "UserName"
                .DataSource = u
                .DataBind()
                .Items.Insert(0, New ListItem(Str_Opt_TatCa, ""))
            End With
            ddlNguoiDung.SelectedValue = Session("UserName")
        End Using
    End Sub
    Private Sub LoadData()
        Using data As New ThanhTraLaoDongEntities

            ''Load số quyết định
            Dim lstSQD = data.uspSoQuyetDinhSelectAll(ddlNguoiDung.SelectedValue)

            With ddlSoQuyetDinh
                .Items.Clear()
                .AppendDataBoundItems = True
                .DataTextField = "SoQuyetDinh"
                .DataValueField = "SoQuyetDinh"
                .DataSource = lstSQD
                .DataBind()
                .Items.Insert(0, New ListItem(Str_Opt_TatCa, ""))
            End With
            '' Load danh sách tỉnh thành 
            If hidTinhThanhTraSo.Value > 0 Then
                Dim lst = (From q In data.Tinhs Where q.TinhId = hidTinhThanhTraSo.Value
                           Order By q.TenTinh Ascending
                            Select New With {.Value = q.TinhId, .Text = q.TenTinh}).ToList
                With ddlTinh
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
                With ddlTinh
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
            
            'Xử lý cây chuyên mục/ vi phạm
            Dim dtList = (From q In data.CauHoiHierarchies
                            Where String.IsNullOrEmpty(q.ParentId)
                            Order By q.Sort Ascending Select q).ToList()
            trvChuyenMuc.Nodes.Clear()
            Dim tnParentRoot As New TreeNode()
            tnParentRoot.Text = "Tất cả"
            tnParentRoot.Value = ""
            tnParentRoot.NavigateUrl = "javascript:void(0)"
            trvChuyenMuc.Nodes.Add(tnParentRoot)
            tnParentRoot.ExpandAll()
            For i As Integer = 0 To dtList.Count - 1
                Dim tnParent As New TreeNode()
                tnParent.Text = dtList(i).CauHoiVietTat
                tnParent.Value = dtList(i).CauHoiId.ToString()
                tnParentRoot.ChildNodes.Add(tnParent)
                tnParentRoot.NavigateUrl = "javascript:void(0)"
                tnParent.Collapse()
                FillChild(tnParent, tnParent.Value, 0)
            Next
            'Xử lý cây thống kê số liệu
            dtList = (From q In data.CauHoiHierarchies
                            Where (q.IsTieuChi = 2)
                            Order By q.Sort Ascending Select q).ToList()
            trvThongKe.Nodes.Clear()
            tnParentRoot = New TreeNode()
            tnParentRoot.Text = "Tất cả"
            tnParentRoot.Value = ""
            tnParentRoot.NavigateUrl = "javascript:void(0)"
            trvThongKe.Nodes.Add(tnParentRoot)
            tnParentRoot.ExpandAll()
            For i As Integer = 0 To dtList.Count - 1
                Dim tnParent As New TreeNode()
                tnParent.Text = dtList(i).CauHoiVietTat
                tnParent.Value = dtList(i).CauHoiId.ToString()
                tnParentRoot.ChildNodes.Add(tnParent)
                tnParentRoot.NavigateUrl = "javascript:void(0)"
                tnParent.Collapse()
                'FillChild(tnParent, tnParent.Value, 0)
            Next
        End Using
    End Sub
    'Load du lieu cho menu con
    Protected Sub FillChild(ByVal Parent As TreeNode, ByVal Value As String, ByVal isLevel As Integer)
        Using data As New ThanhTraLaoDongEntities '' Lấy dữ liệu theo CauHoiHierarchies
            Dim query = From q In data.CauHoiHierarchies
                        Where q.ParentBCId = Value And
                        q.IsTieuChi = 1
                        Order By q.Sort Ascending
                        Select q
            Dim dtList = query.ToList()
            isLevel = isLevel + IIf(dtList.Count >= 0, 1, 0)
            If isLevel <= 4 Then
                Parent.ChildNodes.Clear()
                For i As Integer = 0 To dtList.Count - 1
                    Dim child As New TreeNode()
                    child.Text = dtList(i).CauHoiVietTat
                    child.NavigateUrl = "javascript:void(0)"
                    child.Value = dtList(i).CauHoiId.ToString()
                    Parent.ChildNodes.Add(child)
                    child.Collapse()
                    FillChild(child, child.Value, isLevel)
                Next
            End If
        End Using
    End Sub

    'Lay gia tri menu con
    Protected Sub GetChildNode(ByVal tn As TreeNode, _
                               ByRef strID As String, _
                               Optional ByRef StrHeader As String = "", _
                               Optional ByRef CountChecked As Integer = 0, _
                               Optional ByVal IsBCTong As Boolean = False)
        If Not IsBCTong Then
            For i As Integer = 0 To tn.ChildNodes.Count - 1
                If tn.ChildNodes(i).Checked Then
                    strID = strID & tn.ChildNodes(i).Value & Str_Symbol_Group
                    StrHeader = StrHeader & tn.ChildNodes(i).Text & Str_Symbol_Group
                    CountChecked = CountChecked + 1
                End If
                GetChildNode(tn.ChildNodes(i), strID, StrHeader, CountChecked, IsBCTong)
            Next
        Else
            For i As Integer = 0 To tn.ChildNodes.Count - 1
                strID = strID & tn.ChildNodes(i).Value & Str_Symbol_Group
                StrHeader = StrHeader & tn.ChildNodes(i).Text & Str_Symbol_Group
                CountChecked = CountChecked + 1
                GetChildNode(tn.ChildNodes(i), strID, StrHeader, CountChecked, IsBCTong)
            Next
        End If

    End Sub

    Private Sub CreateHeader(ByVal _dt As DataTable, Optional ByVal IsBCTong As Boolean = False)
        Dim bfield As New BoundField()
        bfield.HeaderText = "Tiêu Chí"
        bfield.DataField = "Name"
        grdShow.Columns.Add(bfield)
        If Not IsBCTong Then
            For index As Integer = 0 To _dt.Rows.Count - 1
                'If index = 1 Then
                bfield = New BoundField()
                bfield.HeaderText = _dt.Rows(index)("TenVietTat").ToString
                bfield.DataField = "Col" & index
                grdShow.Columns.Add(bfield)
                'End If
            Next
        End If
        
        'Thêm cột tổng
        bfield = New BoundField()
        bfield.HeaderText = "Tổng"
        bfield.DataField = "SumVp"
        grdShow.Columns.Add(bfield)
        '{
        '    //Declare the bound field and allocate memory for the bound field.
        '    BoundField bfield = new BoundField();

        '    //Initalize the DataField value.
        '    bfield.DataField = col.ColumnName;

        '    //Initialize the HeaderText field value.
        '    bfield.HeaderText = col.ColumnName;

        '    //Add the newly created bound field to the GridView.
        '    GrdDynamic.Columns.Add(bfield);
        '}

        '//Initialize the DataSource
        'GrdDynamic.DataSource = dt;

        '//Bind the datatable with the GridView.
        'GrdDynamic.DataBind();
    End Sub
#End Region
#Region "PRIVATE EVENT FOR CONTROL"
    Private Sub BindToGrid(Optional ByVal strSoQD As String = "#",
                           Optional ByVal strTinhID As String = "-1",
                           Optional ByVal strHuyenID As String = "0",
                           Optional ByVal strLoaiNguoiDung As Integer = 0,
                           Optional ByVal strViPham As String = "",
                           Optional ByVal IsBCTong As Boolean = False)
        Try
            Using data As New ThanhTraLaoDongEntities
                data.CommandTimeout = 360000
                grdShow.Columns.Clear()
                Dim arrSearch() As String = {strSoQD, strTinhID, strViPham}
                ViewState("search") = arrSearch

                Dim ds As New DataSet()
                Dim entityConn As EntityConnection = data.Connection

                Dim sqlConn As SqlConnection = entityConn.StoreConnection '(SqlConnection)
                Dim cmdReport As SqlCommand = New SqlCommand("usp_Rpt_TongHopViPhamLaoDong", sqlConn)
                Dim daReport As SqlDataAdapter = New SqlDataAdapter(cmdReport)
                Using cmdReport
                    cmdReport.CommandType = CommandType.StoredProcedure
                    Dim parameter1 As SqlParameter = New SqlParameter("@SoQuyetDinh", strSoQD)
                    cmdReport.Parameters.Add(parameter1)
                    Dim parameter2 As SqlParameter = New SqlParameter("@MaTinh", strTinhID)
                    cmdReport.Parameters.Add(parameter2)
                    Dim parameter5 As SqlParameter = New SqlParameter("@MaHuyen", strHuyenID)
                    cmdReport.Parameters.Add(parameter5)
                    Dim parameter3 As SqlParameter = New SqlParameter("@LoaiNguoiDung", strLoaiNguoiDung)
                    cmdReport.Parameters.Add(parameter3)
                    Dim parameter4 As SqlParameter = New SqlParameter("@pr_ReportParams", strViPham)
                    cmdReport.Parameters.Add(parameter4)
                    daReport.Fill(ds)
                End Using
                Dim dt1 As DataTable = ds.Tables(0)
                Dim dt2 As DataTable = ds.Tables(1)

                'Tạo header cho table 
                CreateHeader(dt1, IsBCTong)
                'Tong so ban ghi
                If Not IsNothing(dt2) Then
                    With grdShow
                        .DataSource = dt2
                        .DataBind()
                    End With

                    '' Xét để ẩn đi các column mong muốn
                    'For index As Integer = 0 To grdShow.Columns.Count - 1
                    '    grdShow.Columns(index).Visible = (index <= CountHeader * 2)
                    'Next
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

    Private Sub Button_Click(ByVal sender As Object, ByVal em As System.EventArgs) Handles btnExport.Click, btnExportExcel.Click, btnBCTong.Click
        Select Case CType(sender, Control).ID
            Case "btnExport"
                Dim strSoQD = ddlSoQuyetDinh.SelectedValue.ToString()
                Dim tinh = ddlTinh.SelectedValue
                Dim huyen = ddlHuyen.SelectedValue
                Dim iLoaiNguoiDung = CInt(ddlLoaiThanhTra.SelectedValue)
                Dim StrViPhamParam As String = ""
                GetChildNode(trvChuyenMuc.Nodes(0), StrViPhamParam)
                GetChildNode(trvThongKe.Nodes(0), StrViPhamParam)
                BindToGrid(strSoQD, tinh, huyen, iLoaiNguoiDung, StrViPhamParam)
            Case "btnExportExcel"
                ExportExcel()
            Case "btnBCTong"
                Dim strSoQD = ddlSoQuyetDinh.SelectedValue.ToString()
                Dim tinh = ddlTinh.SelectedValue
                Dim huyen = ddlHuyen.SelectedValue
                Dim iLoaiNguoiDung = CInt(ddlLoaiThanhTra.SelectedValue)
                Dim StrViPhamParam As String = ""
                GetChildNode(trvChuyenMuc.Nodes(0), StrViPhamParam, , , True)
                GetChildNode(trvThongKe.Nodes(0), StrViPhamParam, , , True)
                BindToGrid(strSoQD, tinh, huyen, iLoaiNguoiDung, StrViPhamParam, True)
        End Select

    End Sub
    Protected Sub ddlNguoiDung_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlNguoiDung.SelectedIndexChanged
        Using data As New ThanhTraLaoDongEntities
            ''Load số quyết định
            Dim lstSQD = data.uspSoQuyetDinhSelectAll(ddlNguoiDung.SelectedValue)

            With ddlSoQuyetDinh
                .Items.Clear()
                .AppendDataBoundItems = True
                .DataTextField = "SoQuyetDinh"
                .DataValueField = "SoQuyetDinh"
                .DataSource = lstSQD
                .DataBind()
                .Items.Insert(0, New ListItem(Str_Opt_TatCa, ""))
            End With
        End Using
    End Sub
#End Region
#Region "Export Excel"
    Private Sub ExportExcel(Optional ByVal IsBCTong As Boolean = False)
        Using data As New ThanhTraLaoDongEntities

            Dim _app As Excel.Application
            Dim _workBook As Excel.Workbook

            Dim identity As String = String.Empty
            Dim pathFile As String = String.Empty
            Dim pathFileTemp As String = String.Empty
            Dim fileNameTemp As String = String.Empty
            Dim FolderPath = Server.MapPath("~/Template").ToString

            Dim ds As New DataSet()
            Dim entityConn As EntityConnection = data.Connection

            Dim strSoQD = ddlSoQuyetDinh.SelectedValue.ToString()
            Dim tinh = ddlTinh.SelectedValue
            Dim huyen = ddlHuyen.SelectedValue
            Dim iLoaiNguoiDung = CInt(ddlLoaiThanhTra.SelectedValue)
            Dim StrViPhamParam As String = ""
            GetChildNode(trvChuyenMuc.Nodes(0), StrViPhamParam, , , IsBCTong)
            GetChildNode(trvThongKe.Nodes(0), StrViPhamParam, , , IsBCTong)

            Dim sqlConn As SqlConnection = entityConn.StoreConnection '(SqlConnection)
            Dim cmdReport As SqlCommand = New SqlCommand("usp_Rpt_TongHopViPhamLaoDong", sqlConn)
            Dim daReport As SqlDataAdapter = New SqlDataAdapter(cmdReport)
            Using cmdReport
                cmdReport.CommandType = CommandType.StoredProcedure
                Dim parameter1 As SqlParameter = New SqlParameter("@SoQuyetDinh", strSoQD)
                cmdReport.Parameters.Add(parameter1)
                Dim parameter2 As SqlParameter = New SqlParameter("@MaTinh", tinh)
                cmdReport.Parameters.Add(parameter2)
                Dim parameter5 As SqlParameter = New SqlParameter("@MaHuyen", huyen)
                cmdReport.Parameters.Add(parameter5)
                Dim parameter3 As SqlParameter = New SqlParameter("@LoaiNguoiDung", iLoaiNguoiDung)
                cmdReport.Parameters.Add(parameter3)
                Dim parameter4 As SqlParameter = New SqlParameter("@pr_ReportParams", StrViPhamParam)
                cmdReport.Parameters.Add(parameter4)
                daReport.Fill(ds)
            End Using
            Dim dt_Hearder As DataTable = ds.Tables(0)
            Dim dt_Content As DataTable = ds.Tables(1)


            Try

                identity = Now.ToString("ddMMyyyyHHmmss")
                pathFile = FolderPath & "\BaoCaoTongHopViPham_template.xlsx"
                'Xet IsBCTong có xuất tất cả hoặc chỉ cột tổng?
                If Not IsBCTong Then
                    fileNameTemp = "BaoCaoTongHopViPham" & "_" & identity & ".xlsx"
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

                        'Tạo dòng Header
                        Dim rowcol As String = ""
                        _sheet_Detail.Range("A2", "A2").Value = "Stt"
                        _sheet_Detail.Range("B2", "B2").Value = "Tiêu chí"
                        If dt_Hearder.Rows.Count <= strLetterTotal.Length Then
                            Dim pos As Integer = 0
                            For pos = 2 To dt_Hearder.Rows.Count + 1
                                _sheet_Detail.Range(strLetterTotal(pos) + "2", strLetterTotal(pos) + "2").Value = dt_Hearder(pos - 2)("TenVietTat")
                            Next
                            'Thêm cột tổng
                            _sheet_Detail.Range(strLetterTotal(pos) + "2", strLetterTotal(pos) + "2").Value = "Tổng"
                        Else
                            Dim posAlphabet As Integer = 0
                            Dim pos As Integer = 0
                            For pos = 2 To dt_Hearder.Rows.Count + 1
                                If pos <= strLetterTotal.Length - 1 Then
                                    _sheet_Detail.Range(strLetterTotal(pos) + "2", strLetterTotal(pos) + "2").Value = dt_Hearder(pos - 2)("TenVietTat")
                                Else
                                    If pos \ strLetterTotal.Length = 0 Then
                                        posAlphabet = 0
                                    End If
                                    rowcol = strLetterTotal(posAlphabet) + strLetterTotal((pos \ strLetterTotal.Length) - 1) + "2"
                                    _sheet_Detail.Range(rowcol, rowcol).Value = dt_Hearder(pos - 2)("TenVietTat")
                                End If
                                posAlphabet += 1
                            Next
                            'Thêm cột tổng
                            If pos <= strLetterTotal.Length - 1 Then
                                _sheet_Detail.Range(strLetterTotal(pos) + "2", strLetterTotal(pos) + "2").Value = "Tổng"
                            Else
                                If pos \ strLetterTotal.Length = 0 Then
                                    posAlphabet = 0
                                End If
                                rowcol = strLetterTotal(posAlphabet) + strLetterTotal((pos \ strLetterTotal.Length) - 1) + "2"
                                _sheet_Detail.Range(rowcol, rowcol).Value = "Tổng"
                            End If

                        End If

                        'Xuất nội dung
                        'B1: Đưa nội dung vào một mảng
                        Dim arrRange(dt_Content.Rows.Count, dt_Hearder.Rows.Count + 3) As Object
                        For pos As Integer = 0 To dt_Content.Rows.Count - 1
                            'arrRange(pos, 0) = pos + 1
                            'For col As Integer = 1 To (dt_Hearder.Rows.Count + 1)
                            '    arrRange(pos, col) = dt_Content.Rows(pos)(col)
                            'Next
                            arrRange(pos, 0) = pos + 1
                            arrRange(pos, 1) = dt_Content.Rows(pos)("Name")
                            arrRange(pos, 2) = dt_Content.Rows(pos)("Col0")
                            Dim col As Integer = 0
                            For col = 4 To (dt_Hearder.Rows.Count + 2)
                                arrRange(pos, col - 1) = dt_Content.Rows(pos)(col)
                            Next
                            arrRange(pos, dt_Hearder.Rows.Count + 2) = dt_Content.Rows(pos)("SumVp")
                        Next
                        'B2: Gán mảng vào sheet
                        Dim NameColumn As String = ""
                        Dim titleName As String = ""
                        If (dt_Hearder.Rows.Count + 3) <= strLetterTotal.Length Then
                            NameColumn = strLetterTotal(dt_Hearder.Rows.Count + 2) + (dt_Content.Rows.Count + 2).ToString
                            titleName = strLetterTotal(dt_Hearder.Rows.Count + 2) + "1"
                        Else
                            NameColumn = strLetterTotal(((dt_Hearder.Rows.Count + 2) / strLetterTotal.Length) - 1) + strLetterTotal(((dt_Hearder.Rows.Count + 2) \ strLetterTotal.Length) - 1) + (dt_Content.Rows.Count + 2).ToString
                            titleName = strLetterTotal(((dt_Hearder.Rows.Count + 2) / strLetterTotal.Length) - 1) + strLetterTotal(((dt_Hearder.Rows.Count + 2) \ strLetterTotal.Length) - 1) + "1"
                        End If

                        If arrRange.Length > 0 Then
                            Dim arrSheet As Excel.Range
                            _sheet_Detail.Range("A1", titleName).MergeCells = True
                            _sheet_Detail.Range("A1", titleName).Value = "BÁO CÁO TỔNG HỢP VI PHẠM"
                            arrSheet = _sheet_Detail.Range("A3", NameColumn)
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
                Else 'chỉ xuất cột tổng
                    fileNameTemp = "BaoCao_Tong_TongHopViPham" & "_" & identity & ".xlsx"
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

                        'Tạo dòng Header
                        Dim rowcol As String = ""
                        _sheet_Detail.Range("A2", "A2").Value = "Stt"
                        _sheet_Detail.Range("B2", "B2").Value = "Tiêu chí"
                        'Thêm cột tổng
                        _sheet_Detail.Range("C2", "C2").Value = "Tổng"


                        'Xuất nội dung
                        'B1: Đưa nội dung vào một mảng
                        Dim arrRange(dt_Content.Rows.Count, 3) As Object
                        For pos As Integer = 0 To 2
                            'arrRange(pos, 0) = pos + 1
                            'For col As Integer = 1 To (dt_Hearder.Rows.Count + 1)
                            '    arrRange(pos, col) = dt_Content.Rows(pos)(col)
                            'Next
                            arrRange(pos, 0) = pos + 1
                            arrRange(pos, 1) = dt_Content.Rows(pos)("Name")
                            'arrRange(pos, 2) = dt_Content.Rows(pos)("Col0")
                            'Dim col As Integer = 0
                            'For col = 4 To (dt_Hearder.Rows.Count + 2)
                            '    arrRange(pos, col - 1) = dt_Content.Rows(pos)(col)
                            'Next
                            arrRange(pos, 2) = dt_Content.Rows(pos)("SumVp")
                        Next
                        'B2: Gán mảng vào sheet
                        Dim NameColumn As String = ""
                        Dim titleName As String = ""
                        'If (dt_Hearder.Rows.Count + 3) <= strLetterTotal.Length Then
                        NameColumn = strLetterTotal(2) + (3).ToString
                        titleName = strLetterTotal(2) + "1"
                        'Else
                        '    NameColumn = strLetterTotal(((dt_Hearder.Rows.Count + 2) / strLetterTotal.Length) - 1) + strLetterTotal(((dt_Hearder.Rows.Count + 2) \ strLetterTotal.Length) - 1) + (dt_Content.Rows.Count + 2).ToString
                        '    titleName = strLetterTotal(((dt_Hearder.Rows.Count + 2) / strLetterTotal.Length) - 1) + strLetterTotal(((dt_Hearder.Rows.Count + 2) \ strLetterTotal.Length) - 1) + "1"
                        'End If

                        If arrRange.Length > 0 Then
                            Dim arrSheet As Excel.Range
                            _sheet_Detail.Range("A1", titleName).MergeCells = True
                            _sheet_Detail.Range("A1", titleName).Value = "BÁO CÁO TỔNG HỢP VI PHẠM"
                            arrSheet = _sheet_Detail.Range("A3", NameColumn)
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
