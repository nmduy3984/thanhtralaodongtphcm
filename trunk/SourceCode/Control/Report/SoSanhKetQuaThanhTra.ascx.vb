﻿Imports ThanhTraLaoDongModel
Imports Cls_Common
Imports SecurityService

Partial Class Control_Report_SoSanhKetQuaThanhTra
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
            'trvTinh.Attributes.Add("onclick", "OnTreeClick(event)")
            'trvChuyenMuc.Attributes.Add("onclick", "OnTreeClick(event)")
            txtYearFrom.Text = Now.Year
            txtYearTo.Text = Now.Year
        End If
    End Sub

    Private Sub LoadData()
        Using data As New ThanhTraLaoDongEntities
            '' Load danh sách loại báo cáo 
            Dim lstReportType = (From q In data.OtherLists
                                 Where q.Type.Contains("ReportType")
                                 Select New With {.Text = q.Name, .Value = q.Id}).ToList
            With ddlType
                .Items.Clear()
                .DataTextField = "Text"
                .DataValueField = "Value"
                .DataSource = lstReportType
                .DataBind()
            End With

            '' Load danh sách tỉnh thành 
            Dim query = Nothing
            Dim tnParentRoot As New TreeNode()
            If hidTinhThanhTraSo.Value = 0 Then
                query = (From q In data.Tinhs Order By q.TenTinh Ascending Select q).ToList
                trvTinh.Nodes.Clear()
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
            Else
                query = (From q In data.Tinhs Where q.TinhId = hidTinhThanhTraSo.Value Select q.TenTinh).SingleOrDefault
                hidTenTinh.Value = query
            End If

            '' Load danh sách chuyện mục báo cáo 
            Dim dtListReport = (From q In data.OtherLists Where q.Type.Contains("ReportCategoryType")).ToList()
            trvChuyenMuc.Nodes.Clear()
            tnParentRoot = New TreeNode()
            tnParentRoot.Text = "Tất cả"
            tnParentRoot.Value = ""
            tnParentRoot.NavigateUrl = "javascript:void(0)"
            trvChuyenMuc.Nodes.Add(tnParentRoot)

            tnParentRoot.ExpandAll()
            For i As Integer = 0 To dtListReport.Count - 1
                Dim tnParent As New TreeNode()
                tnParent.Text = dtListReport(i).Name
                tnParent.Value = dtListReport(i).Id.ToString()
                tnParent.NavigateUrl = "javascript:void(0)"
                tnParentRoot.ChildNodes.Add(tnParent)
            Next
        End Using
    End Sub
#End Region
#Region "PRIVATE EVENT FOR CONTROL"
    Private Sub BindToGrid(Optional ByVal strReportType As String = "1", Optional ByVal strReportParams As String = "")
        Using data As New ThanhTraLaoDongEntities
            Dim arrSearch() As String = {strReportType, strReportParams}
            ViewState("search") = arrSearch

            Dim p As List(Of usp_RptSumary_Result) = data.usp_Rpt_KetQuaThanhTra2(strReportType, strReportParams).ToList
            'Bật lại các column của GridView thành True
            For index As Integer = 0 To grdShow.Columns.Count - 1
                grdShow.Columns(index).Visible = True
            Next

            'Tong so ban ghi
            If Not p Is Nothing Then
                With grdShow
                    .DataSource = p
                    .DataBind()
                End With

                ' Xét để ẩn đi các column mong muốn
                For index As Integer = 0 To grdShow.Columns.Count - 1
                    grdShow.Columns(index).Visible = (index <= CountHeader * 2)
                Next
            Else
                With grdShow
                    .DataSource = Nothing
                    .DataBind()
                End With
            End If
        End Using
    End Sub

    Private Sub Button_Click(ByVal sender As Object, ByVal em As System.EventArgs) Handles btnExport.Click
        Dim StrParam As String = ""
        HeaderReport2 = ""
        CountHeader = 0
        Select Case ddlType.SelectedValue
            Case 1
                If hidTinhThanhTraSo.Value = 0 Then
                    For i As Integer = 0 To trvTinh.Nodes(0).ChildNodes.Count - 1
                        If trvTinh.Nodes(0).ChildNodes(i).Checked = True Then
                            StrParam = StrParam + trvTinh.Nodes(0).ChildNodes(i).Value + Str_Symbol_Group
                            HeaderReport2 = HeaderReport2 + trvTinh.Nodes(0).ChildNodes(i).Text + Str_Symbol_Group
                            CountHeader = CountHeader + 1
                        End If
                    Next
                Else
                    StrParam = hidTinhThanhTraSo.Value.ToString
                    HeaderReport2 = hidTenTinh.Value
                    CountHeader = 1
                End If
            Case 2
                For StartDate As Integer = CInt(txtYearFrom.Text.Trim) To CInt(txtYearTo.Text.Trim)
                    StrParam = StrParam & StartDate & Str_Symbol_Group
                    HeaderReport2 = HeaderReport2 & StartDate & Str_Symbol_Group
                    CountHeader = CountHeader + 1
                Next
            Case 3
                For i As Integer = 0 To trvChuyenMuc.Nodes(0).ChildNodes.Count - 1
                    If trvChuyenMuc.Nodes(0).ChildNodes(i).Checked = True Then
                        StrParam = StrParam + trvChuyenMuc.Nodes(0).ChildNodes(i).Value + Str_Symbol_Group
                        HeaderReport2 = HeaderReport2 + trvChuyenMuc.Nodes(0).ChildNodes(i).Text + Str_Symbol_Group
                        CountHeader = CountHeader + 1
                    End If
                Next
        End Select
        If StrParam.Length > 1 Then
            StrParam = StrParam.Substring(0, StrParam.Length - 1).ToString()
        End If
        BindToGrid(ddlType.SelectedValue, StrParam)
    End Sub

    Protected Sub grdShow_RowCreated(ByVal sender As Object, ByVal e As GridViewRowEventArgs) Handles grdShow.RowCreated

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

            Dim headeReport As String = ""
            Select Case ddlType.SelectedValue
                Case 1
                    headeReport = "Địa phương"
                Case 2
                    headeReport = "Năm"
                Case 3
                    headeReport = "Chuyên Mục"
            End Select

            HeaderCell = New TableCell()
            HeaderCell.Text = headeReport
            HeaderCell.ColumnSpan = CountHeader * 2
            HeaderCell.HorizontalAlign = HorizontalAlign.Center
            HeaderGridRow1.Cells.Add(HeaderCell)


            Dim listHeader() As String = Strings.Split(HeaderReport2, Str_Symbol_Group)

            For Each item In listHeader
                If Not String.IsNullOrEmpty(item) Then
                    HeaderCell = New TableCell()
                    HeaderCell.Text = item
                    HeaderCell.ColumnSpan = 2
                    HeaderCell.HorizontalAlign = HorizontalAlign.Center
                    HeaderGridRow2.Cells.Add(HeaderCell)
                End If
            Next

            grdShow.Controls(0).Controls.AddAt(0, HeaderGridRow1)
            grdShow.Controls(0).Controls.AddAt(1, HeaderGridRow2)
        End If
    End Sub

    Protected Sub DropDownList_SelectedIndedChange(ByVal sender As Object, ByVal e As EventArgs) Handles ddlType.SelectedIndexChanged
        grdShow.DataSource = Nothing
        grdShow.DataBind()
    End Sub
#End Region
End Class
