Imports System.Data
Imports ThanhTraLaoDongModel
Imports Cls_Common
Imports SecurityService
Partial Class Control_KienNghi_List
    Inherits System.Web.UI.UserControl
    Private Shared ReadOnly log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
    'Luu nhung MenuId duoc check
    Private IDChecked As String = ""
    Private dtListDMKNSelect As List(Of CauHoiKienNghi)
#Region "Sub and Function"
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
            BindToGrid()
            btnDelete.Attributes.Add("onclick", "return confirmMultiDelete('" & btnDelete.ClientID & "');")
            FillParentMenu()
        End If
    End Sub
    Private Sub BindToGrid(Optional ByVal iPage As Integer = 1, Optional ByVal strSearch As String = "", Optional ByVal strCauHoiId As String = "")
        Using data As New ThanhTraLaoDongEntities
            Dim arrSearch() As String = {iPage.ToString, strSearch, strCauHoiId}
            ViewState("search") = arrSearch
            'So ban ghi muon the hien tren trang
            Dim intPag_Size As Int32 = drpPage_Size.SelectedValue
            Dim p As List(Of uspDMKNSelectAll_Result) = data.uspDMKNSelectAll(strSearch, strCauHoiId, iPage, intPag_Size).ToList()

            Dim strKey_Name() As String = {"KienNghiId", "NoiDungKN", "CauHoiVietTat"}
            'Tong so ban ghi
            If p.Count > 0 Then
                hidCount.Value = p(0).Total
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
    Protected Sub drpPage_Size_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles drpPage_Size.SelectedIndexChanged
        Dim arrSearch() As String
        arrSearch = ViewState("search")
        BindToGrid(1, arrSearch(1), arrSearch(2))
    End Sub
    Protected Sub lnkTitle_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim lnkTile As LinkButton = CType(sender, LinkButton)
        Dim arrSearch() As String
        arrSearch = ViewState("search")
        BindToGrid(lnkTile.ToolTip, arrSearch(1), arrSearch(2))

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
        BindToGrid(hidCur_Page.Value, arrSearch(1), arrSearch(2))
    End Sub
    Protected Sub lnkLast_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkLast.Click
        hidCur_Page.Value = hidCur_Page.Value + 1
        Dim arrSearch() As String
        arrSearch = ViewState("search")
        BindToGrid(hidCur_Page.Value, arrSearch(1), arrSearch(2))
    End Sub
#End Region
#Region "Event for control"

    Protected Sub lnkbtnDelete_Click(ByVal sender As Object, ByVal e As EventArgs)
        Dim intId As Integer
        Dim strLogName As String = ""
        Using data As New ThanhTraLaoDongEntities
            intId = grdShow.DataKeys(hidID.Value)("KienNghiId").ToString

            Dim q = (From p In data.DanhMucKienNghis Where p.KienNghiID = intId Select p).FirstOrDefault
            Try
                Dim chkn = "delete from kiennghiDN where KienNghiID={0};delete from CauHoiKienNghi where DanhMucKienNghiId={1}"
                data.ExecuteStoreCommand(chkn, New Object() {intId, intId})
                data.DanhMucKienNghis.DeleteObject(q)
                data.SaveChanges()
                'Insert_App_Log("Insert  Danhmuchanhvi:" & txtTitle.Text.Trim & "", Function_Name.Danhmuchanhvi, Audit_Type.Create, Request.ServerVariables("REMOTE_ADDR"), Session("UserName"))
                Excute_Javascript("Alertbox('Xóa dữ liệu thành công.');", Me.Page, True)
            Catch ex As Exception
                log4net.Config.XmlConfigurator.Configure()
                log.Error("Error error " & AddTabSpace(1) & Session("Username") & AddTabSpace(1) & "IP:" & GetIPAddress(), ex)
                Excute_Javascript("Alertbox('Xóa thất bại.');", Me.Page, True)
            End Try
        End Using
        BindToGrid()
    End Sub
    Protected Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        Dim intId As Integer = 0
        Dim intCount As Integer
        Dim intTotal As Integer
        Using data As New ThanhTraLaoDongEntities
            Try
                For Each item As GridViewRow In grdShow.Rows
                    Dim chkItem As New CheckBox
                    chkItem = CType(item.FindControl("chkItem"), CheckBox)
                    If chkItem.Checked Then
                        intTotal += 1
                        intId = grdShow.DataKeys(item.RowIndex)("KienNghiId").ToString
                        Dim q = (From p In data.DanhMucKienNghis Where p.KienNghiID = intId Select p).FirstOrDefault
                        Try
                            'Xóa cauhoikiennghi theo kiennghiid
                            Dim chkn = "delete from kiennghiDN where KienNghiID={0};delete from CauHoiKienNghi where DanhMucKienNghiId={1}"
                            data.ExecuteStoreCommand(chkn, New Object() {intId, intId})
                            'Xóa danhmuckiennghi theo kiennghiid
                            data.DanhMucKienNghis.DeleteObject(q)
                            data.SaveChanges()
                            'Insert_App_Log("Insert Danhmuchanhvi:" & txtTitle.Text.Trim & "", Function_Name.Danhmuchanhvi, Audit_Type.Create, Request.ServerVariables("REMOTE_ADDR"), Session("UserName"))
                            intCount += 1
                        Catch ex As Exception
                        End Try
                    End If
                Next
                If intCount > 0 Then
                    Excute_Javascript("Alertbox('Xóa dữ liệu thành công. " & intCount.ToString & " /" & intTotal.ToString & " record.');", Me.Page, True)
                Else
                    Excute_Javascript("Alertbox('Xóa thất bại.');", Me.Page, True)
                End If
            Catch ex As Exception
                log4net.Config.XmlConfigurator.Configure()
                log.Error("Error error " & AddTabSpace(1) & Session("Username") & AddTabSpace(1) & "IP:" & GetIPAddress(), ex)
                Excute_Javascript("Alertbox('Xóa thất bại.');", Me.Page, True)
            End Try
        End Using
        BindToGrid()
    End Sub
    Protected Sub grdShow_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdShow.RowDataBound
        If e.Row.RowIndex >= 0 Then
            'Dim lblSTT As Label = CType(e.Row.FindControl("lblSTT"), Label)
            'lblSTT.Text = CInt(drpPage_Size.SelectedValue) * (CInt(hidCur_Page.Value) - 1).ToString + e.Row.RowIndex + 1
            Dim lnkbtnDelete As LinkButton = CType(e.Row.FindControl("lnkbtnDelete"), LinkButton)
            'lnkbtnDelete.Attributes.Add("onclick", "return ComfirmDialog('" + drpMessage.Items(0).Text + "', 0,'" + lnkbtnDelete.ClientID + "','" + e.Row.RowIndex.ToString + "',1);")
            Dim hplEdit As HyperLink = CType(e.Row.FindControl("hplEdit"), HyperLink)
            hplEdit.NavigateUrl = "../../Page/KienNghi/Edit.aspx?KienNghiId=" & grdShow.DataKeys(e.Row.RowIndex)("KienNghiId").ToString
            Dim hplMota As HyperLink = CType(e.Row.FindControl("hplMota"), HyperLink)
            hplMota.NavigateUrl = "../../Page/KienNghi/Detail.aspx?KienNghiId=" & grdShow.DataKeys(e.Row.RowIndex)("KienNghiId").ToString
            hplMota.Text = grdShow.DataKeys(e.Row.RowIndex)("NoiDungKN").ToString
            If Not IsNothing(grdShow.DataKeys(e.Row.RowIndex)("CauHoiVietTat")) Then
                Dim lblMucvipham As Label = CType(e.Row.FindControl("lblMucvipham"), Label)
                Dim strMucViPham = "<ul>"
                Dim lstMucViPham() As String = grdShow.DataKeys(e.Row.RowIndex)("CauHoiVietTat").ToString.Split("#")
                For i As Integer = 0 To lstMucViPham.Count - 1
                    strMucViPham += "<li>" & (i + 1) & ". " & lstMucViPham(i) & "</li>"
                Next
                strMucViPham += "</ul>"
                lblMucvipham.Text = strMucViPham
            End If

            Dim chkItem As CheckBox = CType(e.Row.FindControl("chkItem"), CheckBox)
            'Permission
            hplEdit.Enabled = HasPermission(Function_Name.KienNghi, Session("RoleID"), 0, Audit_Type.Edit)
            If HasPermission(Function_Name.KienNghi, Session("RoleID"), 0, Audit_Type.Delete) = True Then
                lnkbtnDelete.Attributes.Add("onclick", "return ComfirmDialog('" + drpMessage.Items(0).Text + "', 0,'" + lnkbtnDelete.ClientID + "','" + e.Row.RowIndex.ToString + "',1);")
            Else
                lnkbtnDelete.Enabled = False
                chkItem.Enabled = False
            End If
        End If
    End Sub
#End Region
#Region "Search"
    Protected Sub btnFilter_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFilter.Click
        GetParentNode()
        Dim strCauHoiID As String = ""
        If IDChecked.Length > 1 Then
            strCauHoiID = IDChecked.Substring(1)
        End If
        BindToGrid(1, txtTitleFilter.Text.Trim(), strCauHoiID)
    End Sub
    Protected Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClear.Click
        txtTitleFilter.Text = ""
        BindToGrid(1, txtTitleFilter.Text.Trim())
    End Sub

#End Region
#Region "Tree menu "
    'Load du lieu cho menu cha
    Protected Sub FillParentMenu()
        Using data As New ThanhTraLaoDongEntities
            Dim query = (From q In data.CauHoiHierarchies Where String.IsNullOrEmpty(q.ParentId) Order By q.ParentId Ascending Select q)
            Dim dtList = query.ToList()
            trvMenu.Nodes.Clear()
            Dim tnParentRoot As New TreeNode()
            tnParentRoot.Text = "Tất cả"
            tnParentRoot.Value = ""
            trvMenu.Nodes.Add(tnParentRoot)
            tnParentRoot.ExpandAll()
            For i As Integer = 0 To dtList.Count - 1
                Dim tnParent As New TreeNode()
                tnParent.Text = dtList(i).Title
                tnParent.Value = dtList(i).CauHoiId.ToString()
                tnParentRoot.ChildNodes.Add(tnParent)
                tnParent.Collapse()
                FillChild(tnParent, tnParent.Value)

                If CheckSelectedMenu(dtListDMKNSelect, dtList(i).CauHoiId) = 1 Then
                    tnParent.Checked = True
                    tnParent.ExpandAll()
                Else
                    tnParent.Checked = False

                End If
            Next
        End Using
    End Sub
    'Load du lieu cho menu con
    Protected Sub FillChild(ByVal Parent As TreeNode, ByVal Value As String)
        Using data As New ThanhTraLaoDongEntities
            Dim query = From q In data.CauHoiHierarchies Where q.ParentId = Value Order By q.CauHoiId Ascending Select q
            Dim dtList = query.ToList()
            Parent.ChildNodes.Clear()
            For i As Integer = 0 To dtList.Count - 1
                Dim child As New TreeNode()
                child.Text = dtList(i).Title
                child.Value = dtList(i).CauHoiId.ToString()
                Parent.ChildNodes.Add(child)
                child.Collapse()
                FillChild(child, child.Value)
                If CheckSelectedMenu(dtListDMKNSelect, dtList(i).CauHoiId) = 1 Then
                    child.Checked = True
                    child.ExpandAll()
                    Parent.ExpandAll()
                End If
            Next
        End Using
    End Sub
    'Lay gia tri menu con
    Protected Sub GetParentNode()
        IDChecked = ""

        'For i As Integer = 0 To trvMenu.Nodes.Count - 1
        '    If trvMenu.Nodes(i).Checked = True Then
        '        IDChecked = (IDChecked & ",") + trvMenu.Nodes(i).Value
        '    End If

        'Next
        GetChildNode(trvMenu.Nodes(0))
    End Sub
    'Lay gia tri menu con
    Protected Sub GetChildNode(ByVal tn As TreeNode)
        For i As Integer = 0 To tn.ChildNodes.Count - 1
            If tn.ChildNodes(i).Checked = True Then
                IDChecked = (IDChecked & ",") + tn.ChildNodes(i).Value
            End If
            GetChildNode(tn.ChildNodes(i))
        Next
    End Sub
    Protected Function CheckSelectedMenu(ByVal dtList As List(Of CauHoiKienNghi), ByVal Value As String) As Integer
        Dim Result As Integer = 0
        If Not dtList Is Nothing Then
            For i As Integer = 0 To dtList.Count - 1
                If dtList(i).CauHoiId.Equals(Value) Then
                    Result = 1
                    Exit For
                End If
            Next
        End If

        Return Result
    End Function
    Protected Sub CustomValidator1_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs)
        If trvMenu.CheckedNodes.Count > 0 Then
            args.IsValid = True
        Else
            args.IsValid = False
        End If
    End Sub
    Function GetRootCauHoi(ByVal _CauHoiId As String) As String
        Using _data As New ThanhTraLaoDongEntities
            Dim p = (From q In _data.CauHoiHierarchies Where q.CauHoiId = _CauHoiId Select q.CauHoiId, q.ParentId).FirstOrDefault
            If Not p Is Nothing AndAlso Not p.ParentId Is Nothing Then
                Return GetRootCauHoi(p.ParentId)
            Else
                Return p.CauHoiId
            End If
        End Using
    End Function

#End Region
    

End Class
