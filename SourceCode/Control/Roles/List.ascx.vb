Imports System.Data
Imports ThanhTraLaoDongModel
Imports Cls_Common
Imports SecurityService

Partial Class Control_Roles_List
    Inherits System.Web.UI.UserControl
    Private Shared ReadOnly log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
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
            If HasPermission(Function_Name.Role, Session("RoleID"), 0, Audit_Type.Delete) = True Then
                btnDelete.Attributes.Add("onclick", "return confirmMultiDelete('" & btnDelete.ClientID & "');")
            Else
                btnDelete.Enabled = False
            End If
        End If
    End Sub
    Private Sub BindToGrid(Optional ByVal iPage As Integer = 1, Optional ByVal strSearch As String = "")
        Using data As New ThanhTraLaoDongEntities
            Dim arrSearch() As String = {iPage.ToString, strSearch}
            ViewState("search") = arrSearch
            'So ban ghi muon the hien tren trang
            Dim intPag_Size As Int32 = drpPage_Size.SelectedValue
            Dim p = (From q In data.Roles Order By q.Title Ascending, q.RoleId Descending Select q)
            Dim strKey_Name() As String = {"RoleId", "Title"}
            If strSearch.Trim.Length >= 1 Then
                p = p.Where(Function(q) q.Title.Contains(strSearch.Trim) Or q.Description.Contains(strSearch.Trim))
            End If
            Dim x = p.Skip((iPage - 1) * intPag_Size).Take(intPag_Size).ToList
            'Tong so ban ghi
            hidCount.Value = p.Count
            Create_Pager(hidCount.Value, iPage, intPag_Size, CInt(drpPage_Size.SelectedValue))
            With grdShow
                .DataKeyNames = strKey_Name
                .DataSource = x
                .DataBind()
            End With
            lblTotal.Text = "Hiển thị " + (((iPage - 1) * intPag_Size) + 1).ToString + " đến " + (((iPage - 1) * intPag_Size) + grdShow.Rows.Count).ToString + " trong tổng số " + hidCount.Value.ToString + " bản ghi."
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
        Dim arrSearch() As String
        arrSearch = ViewState("search")
        BindToGrid(CInt(lnkTile.ToolTip.ToString), arrSearch(1).ToString)
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
        BindToGrid(CInt(hidCur_Page.Value.ToString), arrSearch(1).ToString)
    End Sub
    Protected Sub lnkLast_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkLast.Click
        hidCur_Page.Value = hidCur_Page.Value + 1
        Dim arrSearch() As String
        arrSearch = ViewState("search")
        BindToGrid(CInt(hidCur_Page.Value.ToString), arrSearch(1).ToString)
    End Sub
#End Region
#Region "Event for control"
    Protected Sub chkAll_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs)
        For Each itm As GridViewRow In grdShow.Rows
            Dim chkItem As CheckBox = CType(itm.Cells(0).FindControl("chkItem"), CheckBox)
            If chkItem.Enabled = True Then
                chkItem.Checked = CType(sender, CheckBox).Checked
            End If
        Next
    End Sub
    Protected Sub lnkbtnDelete_Click(ByVal sender As Object, ByVal e As EventArgs)
        Using data As New ThanhTraLaoDongEntities
            Dim intId As Integer
            Dim strLogName As String = ""
            intId = grdShow.DataKeys(hidID.Value)("RoleId").ToString
            Dim q = (From p In data.Roles Where p.RoleId = intId Select p).FirstOrDefault
            Try
                data.Roles.DeleteObject(q)
                data.SaveChanges()
                Insert_App_Log("Delete Roles:" & q.Title & "", Function_Name.Role, Audit_Type.Delete, Request.ServerVariables("REMOTE_ADDR"), Session("UserName"))
                Excute_Javascript("Alertbox('Xóa dữ liệu thành công.');", Me.Page, True)
            Catch ex As Exception
                log4net.Config.XmlConfigurator.Configure()
                log.Error("Error error " & AddTabSpace(1) & Session("Username") & AddTabSpace(1) & "IP:" & GetIPAddress(), ex)
                Excute_Javascript("Alertbox('Xóa thất bại.');", Me.Page, True)
            End Try
            Dim arrSearch() As String
            arrSearch = ViewState("search")
            BindToGrid(CInt(arrSearch(0).ToString), arrSearch(1).ToString)
        End Using
    End Sub
    Protected Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        Using data As New ThanhTraLaoDongEntities
            Dim intId As Integer = 0
            Dim intCount As Integer
            Dim intTotal As Integer
            Dim strLogName As String = ""
            Try
                For Each item As GridViewRow In grdShow.Rows
                    Dim chkItem As New CheckBox
                    chkItem = CType(item.FindControl("chkItem"), CheckBox)
                    If chkItem.Checked Then
                        intTotal += 1
                        intId = grdShow.DataKeys(item.RowIndex)("RoleId").ToString
                        Dim q = (From p In data.Roles Where p.RoleId = intId Select p).FirstOrDefault
                        strLogName = strLogName & "," & q.Title
                        Try
                            data.Roles.DeleteObject(q)
                            data.SaveChanges()
                            intCount += 1
                        Catch ex As Exception
                        End Try
                    End If
                Next
                If intCount > 0 Then
                    Insert_App_Log("Delete Roles:" & strLogName & "", Function_Name.Role, Audit_Type.Delete, Request.ServerVariables("REMOTE_ADDR"), Session("UserName"))
                    Excute_Javascript("Alertbox('Xóa dữ liệu thành công. " & intCount.ToString & " /" & intTotal.ToString & " record.');", Me.Page, True)
                Else
                    Excute_Javascript("Alertbox('Xóa thất bại.');", Me.Page, True)
                End If
            Catch ex As Exception
                log4net.Config.XmlConfigurator.Configure()
                log.Error("Error error " & AddTabSpace(1) & Session("Username") & AddTabSpace(1) & "IP:" & GetIPAddress(), ex)
                Excute_Javascript("Alertbox('Xóa thất bại.');", Me.Page, True)
            End Try
            Dim arrSearch() As String
            arrSearch = ViewState("search")
            BindToGrid(CInt(arrSearch(0).ToString), arrSearch(1).ToString)
        End Using
    End Sub
    Protected Sub grdShow_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles grdShow.PageIndexChanging
        grdShow.PageIndex = e.NewPageIndex
        BindToGrid()
    End Sub
    Protected Sub grdShow_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdShow.RowDataBound
        If e.Row.RowIndex >= 0 Then
            Dim lnkbtnDelete As LinkButton = CType(e.Row.FindControl("lnkbtnDelete"), LinkButton)

            Dim ScriptManager As System.Web.UI.ScriptManager = System.Web.UI.ScriptManager.GetCurrent(Me.Page)
            ScriptManager.RegisterAsyncPostBackControl(lnkbtnDelete)

            lnkbtnDelete.Attributes.Add("onclick", "return ComfirmDialog('" + drpMessage.Items(0).Text + "', 0,'" + lnkbtnDelete.ClientID + "','" + e.Row.RowIndex.ToString + "',1);")
            Dim hplEdit As HyperLink = CType(e.Row.FindControl("hplEdit"), HyperLink)
            hplEdit.NavigateUrl = "../../Page/Roles/Edit.aspx?Roleid=" & grdShow.DataKeys(e.Row.RowIndex)("RoleId").ToString
            Dim hplTitle As HyperLink = CType(e.Row.FindControl("hplTitle"), HyperLink)
            hplTitle.NavigateUrl = "../../Page/Roles/Detail.aspx?Roleid=" & grdShow.DataKeys(e.Row.RowIndex)("RoleId").ToString
            hplTitle.Text = grdShow.DataKeys(e.Row.RowIndex)("Title").ToString

            Dim chkItem As CheckBox = CType(e.Row.FindControl("chkItem"), CheckBox)
            'Permission
            If HasPermission(Function_Name.Role, Session("RoleID"), 0, Audit_Type.Delete) = True Then
                lnkbtnDelete.Attributes.Add("onclick", "return ComfirmDialog('" + drpMessage.Items(0).Text + "', 0,'" + lnkbtnDelete.ClientID + "','" + e.Row.RowIndex.ToString + "',1);")
            Else
                lnkbtnDelete.Enabled = False
                chkItem.Enabled = False
            End If

            'Permission
            hplEdit.Enabled = HasPermission(Function_Name.Role, Session("RoleID"), 0, Audit_Type.Edit)

            'Permission
            hplTitle.Enabled = HasPermission(Function_Name.Role, Session("RoleID"), 0, Audit_Type.ViewContent)
        End If
    End Sub
#End Region
#Region "Search"
    Protected Sub btnFilter_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFilter.Click
        BindToGrid(1, txtTitleFilter.Text.Trim())
    End Sub
#End Region

    Protected Sub drpPage_Size_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles drpPage_Size.SelectedIndexChanged
        Dim arrSearch() As String
        arrSearch = ViewState("search")
        BindToGrid(1, arrSearch(1).ToString)
    End Sub
End Class
