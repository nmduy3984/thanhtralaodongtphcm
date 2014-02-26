Imports System.Data
Imports ThanhTraLaoDongModel
Imports Cls_Common
Imports SecurityService

Partial Class Control_Otherlist_List
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
            LoadType()
            If Not Request.QueryString("Type") Is Nothing Then
                HidType.Value = Request.QueryString("Type")
                hidIdEdit.Value = Request.QueryString("Id")
                isEdit.Value = Request.QueryString("Edit")
                ShowData()
            End If
            BindToGrid()
        End If
    End Sub

    Protected Sub LoadType()
        Using Context As New ThanhTraLaoDongEntities
            Dim p = (From temp In Context.OtherListTables Where temp.ValueType = 0 Select temp).ToList
            Dim lstItem As New ListItem("--- Chọn ---", "0")
            ddlType.Items.Clear()
            ddlType.DataTextField = "OtherListName"
            ddlType.DataValueField = "OLCode"
            ddlType.DataSource = p
            ddlType.DataBind()
            ddlType.Items.Insert(0, lstItem)


            Dim lstItemSearch As New ListItem("--- Chọn ---", "")
            ddlTypeSearch.Items.Clear()
            ddlTypeSearch.DataTextField = "OtherListName"
            ddlTypeSearch.DataValueField = "OLCode"
            ddlTypeSearch.DataSource = p
            ddlTypeSearch.DataBind()
            ddlTypeSearch.Items.Insert(0, lstItemSearch)
        End Using
    End Sub

    Protected Sub ShowData()
        Using data As New ThanhTraLaoDongEntities
            Dim p As OtherList = (From q In data.OtherLists Where q.Type = HidType.Value And q.Id = hidIdEdit.Value Select q).SingleOrDefault
            If Not p Is Nothing Then
                ddlType.SelectedValue = p.Type
                txtId.Text = IIf(IsNothing(p.Id) = True, "", p.Id)
                txtName.Text = IIf(IsNothing(p.Name) = True, "", p.Name)
                ddlType.Enabled = False
            End If
        End Using
    End Sub

    Private Sub BindToGrid(Optional ByVal iPage As Integer = 1, Optional ByVal strSearch As String = "", Optional ByVal strSearchType As String = "")
        Using data As New ThanhTraLaoDongEntities
            'So ban ghi muon the hien tren trang
            Dim arrSearch() As String = {iPage.ToString, strSearch, strSearchType}
            ViewState("search") = arrSearch
            Dim intPag_Size As Int32 = drpPage_Size.SelectedValue
            Dim p = (From q In data.OtherLists Where q.Type.Contains(strSearchType) And q.Name.Contains(strSearch) Order By q.Type, q.Id Descending Select q).Skip((iPage - 1) * intPag_Size).Take(intPag_Size).ToList
            Dim strKey_Name() As String = {"Type", "Id"}
            'Tong so ban ghi
            If p.Count > 0 Then
                hidCount.Value = (From q In data.OtherLists Where q.Type.Contains(strSearchType) And q.Name.Contains(strSearch) Order By q.Type, q.Id Descending Select q).ToList.Count()
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


            'hidCount.Value = data.OtherLists.Count
            'Create_Pager(hidCount.Value, iPage, intPag_Size, 10)
            'With grdShow
            '    .DataKeyNames = strKey_Name
            '    .DataSource = p
            '    .DataBind()
            'End With
            'lblTotal.Text = "Hiển thị " + (((iPage - 1) * intPag_Size) + 1).ToString + " đến " + (((iPage - 1) * intPag_Size) + grdShow.Rows.Count).ToString + " trong tổng số " + hidCount.Value.ToString + " bản ghi."
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
        Dim strType As String
        Dim strLogName As String = ""
        Using data As New ThanhTraLaoDongEntities
            intId = grdShow.DataKeys(hidID.Value)("Id").ToString
            strType = grdShow.DataKeys(hidID.Value)("Type").ToString
            Dim q = (From p In data.OtherLists Where p.Id = intId And p.Type = strType Select p).FirstOrDefault

            Try
                data.OtherLists.DeleteObject(q)
                data.SaveChanges()
                Insert_App_Log("Delete LoaiHanhVi:" & q.Name & "", Function_Name.LoaiHanhVi, Audit_Type.Delete, Request.ServerVariables("REMOTE_ADDR"), Session("UserName"))
                Excute_Javascript("Alertbox('Xóa dữ liệu thành công.');", Me.Page, True)
            Catch ex As Exception
                log4net.Config.XmlConfigurator.Configure()
                log.Error("Error error " & AddTabSpace(1) & Session("Username") & AddTabSpace(1) & "IP:" & GetIPAddress(), ex)
                Excute_Javascript("Alertbox('Xóa thất bại.');", Me.Page, True)
            End Try
        End Using
        BindToGrid()
    End Sub
    'Protected Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelete.Click
    '    Dim intId As Integer = 0
    '    Dim intCount As Integer
    '    Dim intTotal As Integer
    '    Using data As New ThanhTraLaoDongEntities
    '        Try
    '            For Each item As GridViewRow In grdShow.Rows
    '                Dim chkItem As New CheckBox
    '                chkItem = CType(item.FindControl("chkItem"), CheckBox)
    '                If chkItem.Checked Then
    '                    intTotal += 1
    '                    intId = grdShow.DataKeys(item.RowIndex)("Type").ToString
    '                    Dim q = (From p In Data.Otherlists Where p.Type = intId Select p).FirstOrDefault
    '                    intId = grdShow.DataKeys(item.RowIndex)("Id").ToString
    '                    q = (From p In data.OtherLists Where p.Id = intId Select p).FirstOrDefault
    '                    Try
    '                        data.Otherlists.DeleteObject(q)
    '                        data.SaveChanges()
    '                        Insert_App_Log("Delete OtherList:" & q.Name & "", Function_Name.Danhmuc, Audit_Type.Delete, Request.ServerVariables("REMOTE_ADDR"), Session("UserName"))
    '                        intCount += 1
    '                    Catch ex As Exception
    '                    End Try
    '                End If
    '            Next
    '            If intCount > 0 Then
    '                Excute_Javascript("Alertbox('Xóa dữ liệu thành công. " & intCount.ToString & " /" & intTotal.ToString & " record.');", Me.Page, True)
    '            Else
    '                Excute_Javascript("Alertbox('Xóa thất bại.');", Me.Page, True)
    '            End If
    '        Catch ex As Exception
    '            log4net.Config.XmlConfigurator.Configure()
    '            log.Error("Error error " & AddTabSpace(1) & Session("Username") & AddTabSpace(1) & "IP:" & GetIPAddress(), ex)
    '            Excute_Javascript("Alertbox('Xóa thất bại.');", Me.Page, True)
    '        End Try
    '    End Using
    '    BindToGrid()
    'End Sub

    Protected Sub grdShow_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdShow.RowDataBound
        If e.Row.RowIndex >= 0 Then
            Dim lblSTT As Label = CType(e.Row.FindControl("lblSTT"), Label)
            lblSTT.Text = CInt(drpPage_Size.SelectedValue) * (CInt(hidCur_Page.Value) - 1).ToString + e.Row.RowIndex + 1
            Dim lnkbtnDelete As LinkButton = CType(e.Row.FindControl("lnkbtnDelete"), LinkButton)
            lnkbtnDelete.Attributes.Add("onclick", "return ComfirmDialog('" + drpMessage.Items(0).Text + "', 0,'" + lnkbtnDelete.ClientID + "','" + e.Row.RowIndex.ToString + "',1);")

            Dim hplEdit As HyperLink = CType(e.Row.FindControl("hplEdit"), HyperLink)
            hplEdit.NavigateUrl = "../../Page/Otherlist/List.aspx?Edit=1&Type=" & grdShow.DataKeys(e.Row.RowIndex)("Type").ToString & "&Id=" & grdShow.DataKeys(e.Row.RowIndex)("Id").ToString
            Dim hplId As HyperLink = CType(e.Row.FindControl("hplId"), HyperLink)
            hplId.NavigateUrl = "../../Page/Otherlist/List.aspx?Edit=1&Type=" & grdShow.DataKeys(e.Row.RowIndex)("Type").ToString & "&Id=" & grdShow.DataKeys(e.Row.RowIndex)("Id").ToString

            Dim hplType As HyperLink = CType(e.Row.FindControl("hplType"), HyperLink)
            hplType.NavigateUrl = "../../Page/Otherlist/List.aspx?Edit=1&Type=" & grdShow.DataKeys(e.Row.RowIndex)("Type").ToString & "&Id=" & grdShow.DataKeys(e.Row.RowIndex)("Id").ToString
            hplId.Text = grdShow.DataKeys(e.Row.RowIndex)("Id").ToString
            hplType.Text = grdShow.DataKeys(e.Row.RowIndex)("Type").ToString

            Dim chkItem As CheckBox = CType(e.Row.FindControl("chkItem"), CheckBox)
            'Permission
            'hplEdit.Enabled = HasPermission(Function_Name.Danhmuc, Session("RoleID"), 0, Audit_Type.Edit)
            'If HasPermission(Function_Name.Danhmuc, Session("RoleID"), 0, Audit_Type.Delete) = True Then
              'Else
            '    lnkbtnDelete.Enabled = False
            '    chkItem.Enabled = False
            'End If

        End If
    End Sub

#End Region

#Region "Search"

    Protected Sub btnFilter_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFilter.Click
        BindToGrid(1, txtTitleFilter.Text.Trim(), ddlTypeSearch.SelectedValue)
    End Sub

#End Region

    Protected Sub drpPage_Size_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles drpPage_Size.SelectedIndexChanged
        Dim arrSearch() As String
        arrSearch = ViewState("search")
        BindToGrid(1, arrSearch(1), arrSearch(2))

    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Using data As New ThanhTraLaoDongEntities
            If isEdit.Value = 1 Then
                Dim p As OtherList = (From q In data.OtherLists Where q.Type = HidType.Value And q.Id = hidIdEdit.Value).SingleOrDefault
                Try
                    p.Name = txtName.Text.Trim()
                    data.SaveChanges()
                    Excute_Javascript("Alertbox('Cập nhật dữ liệu thành công.');window.location ='../../Page/Otherlist/List.aspx';", Me.Page, True)
                Catch ex As Exception
                    log4net.Config.XmlConfigurator.Configure()
                    log.Error("Error error " & AddTabSpace(1) & Session("Username") & AddTabSpace(1) & "IP:" & GetIPAddress(), ex)
                    Excute_Javascript("Alertbox('Cập nhật thất bại.');", Me.Page, True)
                End Try
                isEdit.Value = 0
            Else
                Dim p As New OtherList
                Try
                    p.Type = ddlType.SelectedValue
                    p.Id = txtId.Text
                    p.Name = txtName.Text.Trim()
                    p.Status = True
                    data.OtherLists.AddObject(p)
                    data.SaveChanges()
                    Excute_Javascript("Alertbox('Cập nhật dữ liệu thành công.');window.location ='../../Page/Otherlist/List.aspx';", Me.Page, True)
                    txtName.Text = ""
                Catch ex As Exception
                    log4net.Config.XmlConfigurator.Configure()
                    log.Error("Error error " & AddTabSpace(1) & Session("Username") & AddTabSpace(1) & "IP:" & GetIPAddress(), ex)
                    Excute_Javascript("Alertbox('Cập nhật thất bại.');", Me.Page, True)
                End Try
            End If
        End Using
    End Sub

    Protected Sub btnHuy_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnHuy.Click
        Response.Redirect("List.aspx")
    End Sub

    Protected Sub ddlType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlType.SelectedIndexChanged
        Using data As New ThanhTraLaoDongEntities
            Dim ddlSelect As String = ddlType.SelectedValue
            If Not ddlType.SelectedValue = "" Then
                Dim count = (From temp In data.OtherLists Where temp.Type = ddlSelect).Count
                txtId.Text = count + 1
            Else
                txtId.Text = ""
            End If
        End Using
    End Sub

    Protected Sub ddlTypeSearch_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlTypeSearch.SelectedIndexChanged
        BindToGrid(1, txtTitleFilter.Text.Trim(), ddlTypeSearch.SelectedValue)
    End Sub

End Class
