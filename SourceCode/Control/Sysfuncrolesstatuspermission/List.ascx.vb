Imports System.Data
Imports ThanhTraLaoDongModel
Imports Cls_Common
Imports SecurityService

Partial Class Control_Sysfuncrolesstatuspermission_List
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
            LoadRoles()
            LoadFunction()
            LoadStatus()
            BindToGrid()
            If HasPermission(Function_Name.SysFuncRolesStatusPermission, Session("RoleID"), 0, Audit_Type.Delete) = True Then
                btnDelete.Attributes.Add("onclick", "return confirmMultiDelete('" & btnDelete.ClientID & "');")
            Else
                btnDelete.Enabled = False
            End If

        End If
    End Sub
    Sub LoadRoles()
        'Load dropdowlist Role
        Using Data As New ThanhTraLaoDongEntities
            Dim _role = (From q In Data.Roles Order By q.Title Ascending Select q.RoleId, q.Title).ToList
            ddlRoleid.DataSource = _role
            ddlRoleid.DataTextField = "Title"
            ddlRoleid.DataValueField = "RoleID"
            ddlRoleid.DataBind()
            Dim lstItem As New ListItem("--- Tất cả ---", "-1")
            ddlRoleid.Items.Insert(0, lstItem)
        End Using
    End Sub
    Sub LoadFunction()
        'Load Dropdownlist sysfunction
        Using Data As New ThanhTraLaoDongEntities
            Dim _sysFunction = (From q In Data.Functions Order By q.FunctionName Ascending Select q.FunctionId, q.FunctionName).ToList
            ddlFunctionID.DataSource = _sysFunction
            ddlFunctionID.DataTextField = "FunctionName"
            ddlFunctionID.DataValueField = "FunctionId"
            ddlFunctionID.DataBind()
            Dim lstItem As New ListItem("--- Tất cả ---", "-1")
            ddlFunctionID.Items.Insert(0, lstItem)
        End Using
    End Sub
    Protected Sub LoadStatus()
        Using data As New ThanhTraLaoDongEntities
            Dim p As List(Of Status) = (From q In data.Status Select q).ToList
            drlStatus.DataValueField = "StatusId"
            drlStatus.DataTextField = "Title"
            drlStatus.DataSource = p
            drlStatus.DataBind()
            Dim iTem As New ListItem("---Tất cả---", "-1")
            drlStatus.Items.Insert(0, iTem)
        End Using
    End Sub
    Private Sub BindToGrid(Optional ByVal iPage As Integer = 1, Optional ByVal strSearch As String = "", Optional ByVal sRoleId As String = "-1", Optional ByVal sFuncId As String = "-1", Optional ByVal sStatusId As String = "-1")
        Using data As New ThanhTraLaoDongEntities
            Dim arrSearch() As String = {iPage.ToString, strSearch, sRoleId, sFuncId, sStatusId}
            ViewState("search") = arrSearch
            'So ban ghi muon the hien tren trang
            Dim intPag_Size As Int32 = drpPage_Size.SelectedValue

            Dim p = (data.uspSysFuncRolesStatusPermissionSelectAll(sRoleId, sFuncId, sStatusId)).ToList

            'Tong so ban ghi
            hidCount.Value = p.Count

            Dim q = p.Skip((iPage - 1) * intPag_Size).Take(intPag_Size).ToList

            Dim strKey_Name() As String = {"FunctionId", "StatusId", "RoleId", "RoleName", "AuditNumber"}

            Create_Pager(hidCount.Value, iPage, intPag_Size, 10)
            With grdShow
                .DataKeyNames = strKey_Name
                .DataSource = q
                .DataBind()
            End With
            If (hidCount.Value <> "0") Then
                lblTotal.Text = "Hiển thị " + (((iPage - 1) * intPag_Size) + 1).ToString + " đến " + (((iPage - 1) * intPag_Size) + grdShow.Rows.Count).ToString + " trong tổng số " + hidCount.Value.ToString + " bản ghi."
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
        lnkTitle = e.Item.FindControl("lnkTitle")
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
        BindToGrid(CInt(lnkTile.ToolTip.ToString), arrSearch(1).ToString, arrSearch(2).ToString, arrSearch(3).ToString, arrSearch(4).ToString)
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
        BindToGrid(CInt(hidCur_Page.Value.ToString), arrSearch(1).ToString, arrSearch(2).ToString, arrSearch(3).ToString, arrSearch(4).ToString)
    End Sub
    Protected Sub lnkLast_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkLast.Click
        hidCur_Page.Value = hidCur_Page.Value + 1
        Dim arrSearch() As String
        arrSearch = ViewState("search")
        BindToGrid(CInt(hidCur_Page.Value.ToString), arrSearch(1).ToString, arrSearch(2).ToString, arrSearch(3).ToString, arrSearch(4).ToString)
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
        Dim intId As Integer
        Dim intfuncID As Integer = 0
        Dim intstatusID As Integer = 0
        Dim strLogName As String = ""
        Using data As New ThanhTraLaoDongEntities
            intId = grdShow.DataKeys(hidID.Value)("RoleId").ToString
            intfuncID = grdShow.DataKeys(hidID.Value)("FunctionId").ToString
            intstatusID = grdShow.DataKeys(hidID.Value)("StatusId").ToString
            Dim _rolequeryResult = (From p In data.SysFuncRolesStatusPermissions Where p.RoleId = intId And p.StatusId = intstatusID And p.FunctionId = intfuncID Select p).SingleOrDefault
            Try
                data.SysFuncRolesStatusPermissions.DeleteObject(_rolequeryResult)
                data.SaveChanges()
                Insert_App_Log("Delete Sysfuncrolesstatuspermission:" & intfuncID & " - " & intId & " - " & intstatusID & "", Function_Name.SysFuncRolesStatusPermission, Audit_Type.Delete, Request.ServerVariables("REMOTE_ADDR"), Session("UserName"))
                Excute_Javascript("Alertbox('Xóa dữ liệu thành công.');", Me.Page, True)
            Catch ex As Exception
                log4net.Config.XmlConfigurator.Configure()
                log.Error("Error error " & AddTabSpace(1) & Session("Username") & AddTabSpace(1) & "IP:" & GetIPAddress(), ex)
                Excute_Javascript("Alertbox('Xóa thất bại.');", Me.Page, True)
            End Try
        End Using
        Dim arrSearch() As String
        arrSearch = ViewState("search")
        BindToGrid(CInt(arrSearch(0).ToString), arrSearch(1).ToString, arrSearch(2).ToString, arrSearch(3).ToString, arrSearch(4).ToString)
    End Sub
    Protected Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        Dim intId As Integer = 0
        Dim intfuncID As Integer = 0
        Dim intstatusID As Integer = 0
        Dim intCount As Integer
        Dim intTotal As Integer
        Using data As New ThanhTraLaoDongEntities
            Try
                For Each item As GridViewRow In grdShow.Rows
                    Dim chkItem As New CheckBox
                    chkItem = CType(item.FindControl("chkItem"), CheckBox)
                    If chkItem.Checked Then
                        intTotal += 1
                        intId = grdShow.DataKeys(item.RowIndex)("RoleId").ToString
                        intfuncID = grdShow.DataKeys(item.RowIndex)("FunctionId").ToString
                        intstatusID = grdShow.DataKeys(item.RowIndex)("StatusId").ToString
                        Dim _rolequeryResult = (From p In data.SysFuncRolesStatusPermissions Where p.RoleId = intId And p.StatusId = intstatusID And p.FunctionId = intfuncID Select p).SingleOrDefault
                        Try
                            data.SysFuncRolesStatusPermissions.DeleteObject(_rolequeryResult)
                            data.SaveChanges()
                            intCount += 1
                            Insert_App_Log("Delete Sysfuncrolesstatuspermission:" & intfuncID & " - " & intId & " - " & intstatusID & "", Function_Name.SysFuncRolesStatusPermission, Audit_Type.Delete, Request.ServerVariables("REMOTE_ADDR"), Session("UserName"))
                        Catch ex As Exception
                        End Try
                    End If
                    'Dim q = (From p In data.SysFuncRolesStatusPermissions Where p.StatusId = intId Select p).FirstOrDefault
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
        Dim arrSearch() As String
        arrSearch = ViewState("search")
        BindToGrid(CInt(arrSearch(0).ToString), arrSearch(1).ToString, arrSearch(2).ToString, arrSearch(3).ToString, arrSearch(4).ToString)
    End Sub
    Protected Sub grdShow_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles grdShow.PageIndexChanging
        grdShow.PageIndex = e.NewPageIndex
        BindToGrid()
    End Sub
    Protected Sub grdShow_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdShow.RowDataBound
        If e.Row.RowIndex >= 0 Then
            Dim lblSTT As Label = CType(e.Row.FindControl("lblSTT"), Label)
            lblSTT.Text = CInt(drpPage_Size.SelectedValue) * (CInt(hidCur_Page.Value) - 1).ToString + e.Row.RowIndex + 1

            Dim lnkbtnDelete As LinkButton = CType(e.Row.FindControl("lnkbtnDelete"), LinkButton)
            Dim ScriptManager As System.Web.UI.ScriptManager = System.Web.UI.ScriptManager.GetCurrent(Me.Page)
            ScriptManager.RegisterAsyncPostBackControl(lnkbtnDelete)

            Dim chkItem As CheckBox = CType(e.Row.FindControl("chkItem"), CheckBox)
            Dim hplEdit As HyperLink = CType(e.Row.FindControl("hplEdit"), HyperLink)
            Dim hplRoleid As HyperLink = CType(e.Row.FindControl("hplRoleid"), HyperLink)
            Dim chklst As CheckBoxList = CType(e.Row.FindControl("chklst"), CheckBoxList)
            'Permission
            If HasPermission(Function_Name.SysFuncRolesStatusPermission, Session("RoleID"), 0, Audit_Type.Delete) = True Then
                lnkbtnDelete.Attributes.Add("onclick", "return ComfirmDialog('" + drpMessage.Items(0).Text + "', 0,'" + lnkbtnDelete.ClientID + "','" + e.Row.RowIndex.ToString + "',1);")
            Else
                lnkbtnDelete.Enabled = False
                chkItem.Enabled = False
            End If

            Dim _auditDictionary As Dictionary(Of String, Integer)

            If ViewState("data") Is Nothing Then
                ViewState("data") = BindListRole()
                _auditDictionary = BindListRole()
            Else
                _auditDictionary = CType(ViewState("data"), Dictionary(Of String, Integer))
            End If
            Dim x = From q In _auditDictionary Select AuditType = "", AuditValue = q.Value
            chklst.DataSource = x
            chklst.DataTextField = "AuditType"
            chklst.DataValueField = "AuditValue"
            chklst.DataBind()
            'bind Audit Number

            Dim _auditNumber As Integer = CInt(grdShow.DataKeys(e.Row.RowIndex)("AuditNumber").ToString)
            For Each item In chklst.Items
                item.Selected = CInt(_auditNumber) And CInt(item.Value)
            Next
            'Permission
            hplEdit.Enabled = HasPermission(Function_Name.SysFuncRolesStatusPermission, Session("RoleID"), 0, Audit_Type.Edit)

            'hplTitle.Text = grdShow.DataKeys(e.Row.RowIndex)("Title").ToString
            'Permission
            hplRoleid.Enabled = HasPermission(Function_Name.SysFuncRolesStatusPermission, Session("RoleID"), 0, Audit_Type.ViewContent)

            lnkbtnDelete.Attributes.Add("onclick", "return ComfirmDialog('" + drpMessage.Items(0).Text + "', 0,'" + lnkbtnDelete.ClientID + "','" + e.Row.RowIndex.ToString + "',1);")
            hplEdit.NavigateUrl = "../../Page/Sysfuncrolesstatuspermission/Edit.aspx?Functionid=" & grdShow.DataKeys(e.Row.RowIndex)("FunctionId").ToString & "&Roleid=" & grdShow.DataKeys(e.Row.RowIndex)("RoleId").ToString & "&Statusid=" & grdShow.DataKeys(e.Row.RowIndex)("StatusId").ToString
            hplRoleid.NavigateUrl = "../../Page/Sysfuncrolesstatuspermission/Detail.aspx?Functionid=" & grdShow.DataKeys(e.Row.RowIndex)("FunctionId").ToString & "&Roleid=" & grdShow.DataKeys(e.Row.RowIndex)("RoleId").ToString & "&Statusid=" & grdShow.DataKeys(e.Row.RowIndex)("StatusId").ToString
            If IsNothing(grdShow.DataKeys(e.Row.RowIndex)("RoleName")) Then
                hplRoleid.Text = ""
            Else
                hplRoleid.Text = grdShow.DataKeys(e.Row.RowIndex)("RoleName").ToString
            End If
        End If
    End Sub
    Private Function BindListRole() As Dictionary(Of String, Integer)
        Dim _auditDictionary As New Dictionary(Of String, Integer)
        ' Add two keys.
        _auditDictionary.Add(Cls_Common.Audit_Type.Create.ToString, Cls_Common.Audit_Type.Create)
        _auditDictionary.Add(Cls_Common.Audit_Type.Delete.ToString, Cls_Common.Audit_Type.Delete)
        _auditDictionary.Add(Cls_Common.Audit_Type.Edit.ToString, Cls_Common.Audit_Type.Edit)
        _auditDictionary.Add(Cls_Common.Audit_Type.ViewContent.ToString, Cls_Common.Audit_Type.ViewContent)
        _auditDictionary.Add(Cls_Common.Audit_Type.Submit.ToString, Cls_Common.Audit_Type.Submit)
        _auditDictionary.Add(Cls_Common.Audit_Type.Publish.ToString, Cls_Common.Audit_Type.Publish)
        Return _auditDictionary
    End Function
#End Region
#Region "Search"
    Protected Sub btnFilter_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFilter.Click
        BindToGrid(1, "", ddlRoleid.SelectedValue, ddlFunctionID.SelectedValue, drlStatus.SelectedValue)
    End Sub
#End Region

    Protected Sub drpPage_Size_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles drpPage_Size.SelectedIndexChanged
        Dim arrSearch() As String
        arrSearch = ViewState("search")
        BindToGrid(1, arrSearch(1).ToString, arrSearch(2).ToString, arrSearch(3).ToString, arrSearch(4).ToString)
    End Sub


End Class
