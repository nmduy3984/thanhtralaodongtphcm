Imports System.Linq
Imports ThanhTraLaoDongModel
Imports Cls_Common
Imports SecurityService

Partial Class Control_Report_PhanTichTinhHinhViPhamLD2
    Inherits System.Web.UI.UserControl

    'Luu nhung MenuId duoc check
    Private IDChecked As String = ""
    Private dtListDMKNSelect As List(Of CauHoiKienNghi)
    Private HeaderReport2 As String = "" ' mỗi giá trị cách nhau bởi dấu #
    Private CountHeader As Integer = 0 ' Chứa giá trị số cột của báo cáo 
    Private rowCount As Integer = 0 ' chứa số dòng của báo cáo 
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
            LoadViPham()
            LoadLHSX()
            'trvViPham.Attributes.Add("onclick", "OnTreeClick(event)")
            'trvLHSX.Attributes.Add("onclick", "OnTreeClick(event)")
            txtYearFrom.Text = Now.Year
            txtYearTo.Text = Now.Year
        End If
        trvViPham.CssClass = "tree"
    End Sub

    Private Sub LoadLHSX()
        Using data As New ThanhTraLaoDongEntities

            '' Load danh sách loại hình sản xuất 
            Dim dtListReport = (From q In data.LoaiHinhSanXuats
                                Where q.ParentID = 0
                                Order By q.ParentID Ascending
                                Select q).ToList()
            trvLHSX.Nodes.Clear()
            Dim tnParentRoot As New TreeNode()
            tnParentRoot = New TreeNode()
            tnParentRoot.Text = "Tất cả"
            tnParentRoot.Value = ""
            tnParentRoot.NavigateUrl = "javascript:void(0)"
            trvLHSX.Nodes.Add(tnParentRoot)
            tnParentRoot.ExpandAll()
            For i As Integer = 0 To dtListReport.Count - 1
                Dim tnParent As New TreeNode()
                tnParent.Text = dtListReport(i).Title
                tnParent.Value = dtListReport(i).LoaiHinhSXId.ToString()
                tnParentRoot.ChildNodes.Add(tnParent)
                tnParentRoot.NavigateUrl = "javascript:void(0)"
                tnParent.Collapse()
                FillChild(tnParent, tnParent.Value, 0, 2)
            Next
        End Using
    End Sub

    Private Sub LoadViPham()
        Using data As New ThanhTraLaoDongEntities
            Dim dtList = (From q In data.CauHoiHierarchies
                          Where String.IsNullOrEmpty(q.ParentId) And q.IsBaoCao
                          Order By q.ParentId Ascending
                          Select q).ToList()
            trvViPham.Nodes.Clear()
            Dim tnParentRoot As New TreeNode()
            tnParentRoot.Text = "Tất cả"
            tnParentRoot.Value = ""
            tnParentRoot.NavigateUrl = "javascript:void(0)"
            trvViPham.Nodes.Add(tnParentRoot)
            tnParentRoot.ExpandAll()
            For i As Integer = 0 To dtList.Count - 1
                Dim tnParent As New TreeNode()
                tnParent.Text = dtList(i).CauHoiVietTat
                tnParent.Value = dtList(i).CauHoiId.ToString()
                tnParentRoot.ChildNodes.Add(tnParent)
                tnParentRoot.NavigateUrl = "javascript:void(0)"
                tnParent.Collapse()
                FillChild(tnParent, tnParent.Value, 0, 1)
            Next
        End Using

    End Sub

    'Load du lieu cho menu con
    Protected Sub FillChild(ByVal Parent As TreeNode, ByVal Value As String, ByVal isLevel As Integer, ByVal type As Integer)
        Using data As New ThanhTraLaoDongEntities
            Select Case type
                Case 1 '' Lấy dữ liệu theo CauHoiHierarchies
                    Dim query = From q In data.CauHoiHierarchies Where q.ParentId = Value Order By q.CauHoiId Ascending Select q
                    Dim dtList = query.ToList()
                    isLevel = isLevel + IIf(dtList.Count >= 0, 1, 0)
                    If isLevel <= 3 Then
                        Parent.ChildNodes.Clear()
                        For i As Integer = 0 To dtList.Count - 1
                            Dim child As New TreeNode()
                            child.Text = dtList(i).CauHoiVietTat
                            child.Value = dtList(i).CauHoiId.ToString()
                            Parent.ChildNodes.Add(child)
                            child.Collapse()
                            FillChild(child, child.Value, isLevel, 1)
                        Next
                    End If
                Case 2 '' Lấy dữ liệu theo Loại hình sản xuất
                    Dim query = From q In data.LoaiHinhSanXuats Where q.ParentID = Value Order By q.LoaiHinhSXId Ascending Select q
                    Dim dtList = query.ToList()
                    isLevel = isLevel + IIf(dtList.Count >= 0, 1, 0)
                    If isLevel <= 4 Then
                        Parent.ChildNodes.Clear()
                        For i As Integer = 0 To dtList.Count - 1
                            Dim child As New TreeNode()
                            child.Text = dtList(i).Title
                            child.Value = dtList(i).LoaiHinhSXId.ToString()
                            Parent.ChildNodes.Add(child)
                            child.Collapse()
                            FillChild(child, child.Value, isLevel, 2)
                        Next
                    End If
            End Select
        End Using
    End Sub

    'Lay gia tri menu con
    Protected Sub GetChildNode(ByVal tn As TreeNode, _
                               ByRef strID As String, _
                               Optional ByRef StrHeader As String = "", _
                               Optional ByRef CountChecked As Integer = 0)
        For i As Integer = 0 To tn.ChildNodes.Count - 1
            If tn.ChildNodes(i).Checked Then
                strID = strID & tn.ChildNodes(i).Value & Str_Symbol_Group
                StrHeader = StrHeader & tn.ChildNodes(i).Text & Str_Symbol_Group
                CountChecked = CountChecked + 1
            End If
            GetChildNode(tn.ChildNodes(i), strID, StrHeader, CountChecked)
        Next
    End Sub
#End Region
#Region "PRIVATE EVENT FOR CONTROL"
    Private Sub BindToGrid(Optional ByVal strReportType As String = "", Optional ByVal strReportParams1 As String = "", Optional ByVal strReportParams2 As String = "")
        Using data As New ThanhTraLaoDongEntities

            Dim p = data.usp_Rpt_PhanTichTinhHinhViPhamLD(strReportType, strReportParams1, strReportParams2, TypeViPham.ViPham, True).ToList
            'Bật lại các column của GridView thành True
            For index As Integer = 0 To grdShow.Columns.Count - 1
                grdShow.Columns(index).Visible = True
            Next

            'Tong so ban ghi
            If Not p Is Nothing Then
                rowCount = p.Count
                With grdShow
                    .DataSource = p
                    .DataBind()
                End With
                ' Xét để ẩn đi các column mong muốn
                Dim type As Integer = ddlType.SelectedValue
                'If type = 2 Or type = 3 Or type = 7 Or type = 8 Then '' Nếu loại báo cáo khác lọai báo cáo về Loại hình doanh nghiệp thì xét số cột để ẩn hiện
                For index As Integer = 0 To grdShow.Columns.Count - 1
                    grdShow.Columns(index).Visible = (index <= CountHeader * 2)
                Next
                'End If
            Else
            With grdShow
                .DataSource = Nothing
                .DataBind()
            End With
            End If
        End Using
    End Sub

    Private Sub Button_Click(ByVal sender As Object, ByVal em As System.EventArgs) Handles btnExport.Click
        Dim StrTypeReportParam As String = ""
        Dim StrViPhamParam As String = ""
        HeaderReport2 = ""
        CountHeader = 0
        Select Case ddlType.SelectedValue
            Case 1 ' nhận input là danh sách vi phạm
                CountHeader = 5
                GetChildNode(trvViPham.Nodes(0), StrViPhamParam)
            Case 2 ' nhận input là danh sách vi phạm và danh sách Lĩnh vực sản xuất
                GetChildNode(trvLHSX.Nodes(0), StrTypeReportParam, HeaderReport2, CountHeader)
                GetChildNode(trvViPham.Nodes(0), StrViPhamParam)
            Case 3 '' Nhận input là danh sách năm và danh sách vi phạm
                For StartDate As Integer = CInt(txtYearFrom.Text.Trim) To CInt(txtYearTo.Text.Trim)
                    StrTypeReportParam = StrTypeReportParam & StartDate & Str_Symbol_Group
                    HeaderReport2 = HeaderReport2 & StartDate & Str_Symbol_Group
                    CountHeader = CountHeader + 1
                Next
                GetChildNode(trvViPham.Nodes(0), StrViPhamParam)
            Case 4 '' nhận input là danh sách các vi phạm
                CountHeader = 5
                GetChildNode(trvViPham.Nodes(0), StrViPhamParam)
            Case 5, 6
                CountHeader = 5
            Case 7, 8 ' nhận input là năm từ và năm đến
                CountHeader = 3
                StrTypeReportParam = txtYearFrom.Text.Trim
                StrViPhamParam = txtYearTo.Text.Trim
        End Select

        If StrTypeReportParam.Length > 1 And StrTypeReportParam.Contains(Str_Symbol_Group) Then
            StrTypeReportParam = StrTypeReportParam.Substring(0, StrTypeReportParam.Length - 1).ToString()
        End If
        If StrViPhamParam.Length > 1 And StrViPhamParam.Contains(Str_Symbol_Group) Then
            StrViPhamParam = StrViPhamParam.Substring(0, StrViPhamParam.Length - 1).ToString()
        End If
        BindToGrid(ddlType.SelectedValue, StrTypeReportParam, StrViPhamParam)
    End Sub

    Protected Sub grdShow_RowCreated(ByVal sender As Object, ByVal e As GridViewRowEventArgs) Handles grdShow.RowCreated
        If e.Row.RowType = DataControlRowType.Header Then
            Dim HeaderGridRow1 As New GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert)
            Dim HeaderGridRow2 As New GridViewRow(1, 0, DataControlRowType.Header, DataControlRowState.Insert)

            Dim headeReport1 As String = ""
            Dim headeReport2 As String = ""
            Select Case ddlType.SelectedValue
                Case 1 ' Mẫu 4
                    grdShow.ShowHeader = True
                    headeReport1 = "Tình hình vi phạm theo loại hình doanh nghiệp".ToUpper()
                    headeReport2 = "Các lỗi vi phạm".ToUpper()
                    CreateGridViewRow(HeaderGridRow2, 1)
                Case 2 ' Mẫu 5
                    grdShow.ShowHeader = True
                    headeReport1 = "Tình hình vi phạm theo loại hình sản xuất".ToUpper()
                    headeReport2 = "Các lỗi vi phạm".ToUpper()
                    CreateGridViewRow(HeaderGridRow2, 2)
                Case 3 ' Mẫu 6
                    grdShow.ShowHeader = True
                    headeReport1 = "Tình hình vi phạm theo năm ".ToUpper()
                    headeReport2 = "Các lỗi vi phạm".ToUpper()
                    CreateGridViewRow(HeaderGridRow2, 2)
                Case 4 ' Mẫu 7
                    grdShow.ShowHeader = True
                    headeReport1 = "Tình hình vi phạm theo các yếu tố liên quan ".ToUpper()
                    headeReport2 = "Các lỗi vi phạm".ToUpper()
                    CreateGridViewRow(HeaderGridRow2, 3)
                Case 5, 6 ' Mẫu 8 9.Đổi cột % thành số tiền
                    grdShow.ShowHeader = True
                    headeReport1 = "Hành vi xử lí theo loại hình doanh nghiệp".ToUpper()
                    headeReport2 = IIf(ddlType.SelectedValue = 5, "20 hành vi xử phạt nhiều nhất".ToUpper(), "20 Địa phương xử phạt nhiều nhất".ToUpper())
                    CreateGridViewRow(HeaderGridRow2, 1)

                Case 7 ' Mẫu 10
                    headeReport1 = "Tình tai nạn lao động các doanh nghiệp đã thanh tra ".ToUpper()
                    headeReport2 = "10 Lĩnh vực sản xuất có nhiều tai nạn lao động".ToUpper()
                    CreateGridViewRow(HeaderGridRow2, 4)
                    grdShow.ShowHeader = False
                Case 8 ' Mẫu 11
                    headeReport1 = "Tình tai nạn lao động các doanh nghiệp đã thanh tra ".ToUpper()
                    headeReport2 = "10 địa phương có nhiều tai nạn lao động".ToUpper()
                    CreateGridViewRow(HeaderGridRow2, 4)
                    grdShow.ShowHeader = False
            End Select

            ''For Header 1
            Dim HeaderCell As New TableCell()
            HeaderCell.Text = headeReport2
            HeaderCell.RowSpan = IIf(ddlType.SelectedValue < 7, 3, 2)
            HeaderCell.CssClass = "GridHeader"
            HeaderCell.HorizontalAlign = HorizontalAlign.Center
            HeaderGridRow1.Cells.Add(HeaderCell)

            'For Header 2
            HeaderCell = New TableCell()
            HeaderCell.Text = headeReport1
            HeaderCell.CssClass = "GridHeader"
            HeaderCell.ColumnSpan = IIf(ddlType.SelectedValue <> 1, CountHeader * 2, 10)
            HeaderCell.HorizontalAlign = HorizontalAlign.Center
            HeaderGridRow1.Cells.Add(HeaderCell)
            grdShow.Controls(0).Controls.AddAt(0, HeaderGridRow1)

            'For Header 3
            grdShow.Controls(0).Controls.AddAt(1, HeaderGridRow2)
        End If
    End Sub

    Protected Sub CreateGridViewRow(ByRef grdViewRow As GridViewRow, ByVal typeGridView As Integer)
        Select Case typeGridView
            Case 1 '' Tạo GridView có các mục bên dưới
                Dim HeaderCell As New TableCell()
                For index As Integer = 1 To 5
                    Dim header As String = ""
                    Select Case index
                        Case 1
                            header = "FDI"
                        Case 2
                            header = "Nhà Nước"
                        Case 3
                            header = "Ngoài Nhà Nước"
                        Case 4
                            header = "Khác"
                        Case 5
                            header = "Tổng"
                    End Select
                    HeaderCell = New TableCell()
                    HeaderCell.Text = header
                    HeaderCell.ColumnSpan = 2
                    HeaderCell.HorizontalAlign = HorizontalAlign.Center
                    grdViewRow.Cells.Add(HeaderCell)
                Next
            Case 2 '' Tạo GridView có số cột tự động
                Dim HeaderCell As New TableCell()
                Dim listHeader() As String = Strings.Split(HeaderReport2, Str_Symbol_Group)
                For Each item In listHeader
                    If Not String.IsNullOrEmpty(item) Then
                        HeaderCell = New TableCell()
                        HeaderCell.Text = item
                        HeaderCell.ColumnSpan = 2
                        HeaderCell.HorizontalAlign = HorizontalAlign.Center
                        grdViewRow.Cells.Add(HeaderCell)
                    End If
                Next
            Case 3 '' Tạo GriwView cho mẫu báo cáo số 7
                Dim HeaderCell As New TableCell()
                For index As Integer = 1 To 5
                    Dim header As String = ""
                    Select Case index
                        Case 1
                            header = "Có CĐCS"
                        Case 2
                            header = "Chưa có CĐCS"
                        Case 3
                            header = "Có bộ phận AT"
                        Case 4
                            header = "Chưa có bộ phận AT"
                        Case 5
                            header = "Tổng"
                    End Select
                    HeaderCell = New TableCell()
                    HeaderCell.Text = header
                    HeaderCell.ColumnSpan = 2
                    HeaderCell.HorizontalAlign = HorizontalAlign.Center
                    grdViewRow.Cells.Add(HeaderCell)
                Next
            Case 4 ' Tạo header GridView cho các mẫu số 10, 11
                Dim HeaderCell As New TableCell()
                For index As Integer = 1 To 6
                    Dim header As String = ""
                    Select Case index
                        Case 1
                            header = "Số vụ"
                        Case 2
                            header = "Số người chết"
                        Case 3
                            header = "Số bị thương nặng"
                        Case 4
                            header = "Số bị thương nhẹ"
                        Case 5
                            header = "Tổng số nạn nhân"
                        Case 6
                            header = "Tổng thiệt hại"
                    End Select
                    HeaderCell = New TableCell()
                    HeaderCell.Text = header
                    HeaderCell.HorizontalAlign = HorizontalAlign.Center
                    grdViewRow.Cells.Add(HeaderCell)
                Next
                grdViewRow.CssClass = "GridHeader"
        End Select
    End Sub
    Protected Sub grdShow_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdShow.RowDataBound
        If e.Row.RowType = DataControlRowType.Header And (ddlType.SelectedValue = 5 Or ddlType.SelectedValue = 6) Then
            For index = 0 To e.Row.Cells.Count - 1
                If index > 1 And (index Mod 2 = 0) Then
                    e.Row.Cells(index).Text = "Số tiền"
                End If
            Next
        End If
        If rowCount > 2 Then
            If ddlType.SelectedValue <= 4 Then
                Select Case e.Row.RowIndex
                    Case rowCount - 2 '' Dòng tổng
                        e.Row.CssClass = "ClassRowRed"
                    Case rowCount - 1 '' Dòng Trung bình
                        e.Row.CssClass = "ClassRowYellow"
                End Select
            Else
                Select Case e.Row.RowIndex
                    Case rowCount - 1 '' Dòng Tổng
                        e.Row.CssClass = "ClassRowRed"
                End Select
            End If
        End If
    End Sub

    Protected Sub DropDownList_SelectedIndedChange(ByVal sender As Object, ByVal e As EventArgs) Handles ddlType.SelectedIndexChanged
        Select Case ddlType.SelectedValue
            Case 2
                LoadLHSX()
                LoadViPham()
            Case Is <= 4
                LoadViPham()
        End Select
        grdShow.DataSource = Nothing
        grdShow.DataBind()
    End Sub
#End Region
End Class
