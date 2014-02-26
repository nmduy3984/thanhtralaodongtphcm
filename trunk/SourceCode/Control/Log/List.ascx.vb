Imports System.Data
Imports ThanhTraLaoDongModel
Imports Cls_Common
Imports SecurityService
Partial Class Control_Log_List
    Inherits System.Web.UI.UserControl
    Private Shared ReadOnly log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
#Region "Sub and Function"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            'xu ly jquery khi post back
            Dim script As ScriptManager = ScriptManager.GetCurrent(Me.Page)
            If Not script Is Nothing AndAlso script.IsInAsyncPostBack Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.Page.GetType, "duyjs", "ajaxJquery()", True)
                ScriptManager.RegisterStartupScript(Me.Page, Me.Page.GetType, "duyjs1", "ajaxJqueryToolTip()", True)
            Else
                Me.Page.ClientScript.RegisterStartupScript(Me.Page.GetType(), "duyjs", String.Concat("Sys.Application.add_load(function(){", "ajaxJquery()", "});"), True)
                Me.Page.ClientScript.RegisterStartupScript(Me.Page.GetType(), "duyjs1", String.Concat("Sys.Application.add_load(function(){", "ajaxJqueryToolTip()", "});"), True)
            End If
            getFunction()
            BindToGrid()
            If HasPermission(Function_Name.Log, Session("RoleID"), 0, Audit_Type.Delete) = True Then
                btnDelete.Attributes.Add("onclick", "return confirmMultiDelete('" & btnDelete.ClientID & "');")
            Else
                btnDelete.Enabled = False
            End If
            btnDelete.Attributes.Add("onclick", "return confirmMultiDelete('" & btnDelete.ClientID & "');")
            
        End If
    End Sub
    Protected Sub ResetControl()
        txtContent.Text = ""
        txtClientip.Text = ""
        txtFrom.Text = ""
        txtTo.Text = ""
        '  txtTitleFilter.Text = ""
        txtUsername.Text = ""
        drlAction.SelectedValue = 0
        drlFunction.SelectedValue = 0
    End Sub
    Protected Sub getFunction()
        Dim data As New ThanhTraLaoDongEntities
        Dim p = (From q In data.Functions Select q).ToList
        drlFunction.DataValueField = "FunctionId"
        drlFunction.DataTextField = "FunctionName"
        drlFunction.DataSource = p
        drlFunction.DataBind()
        drlFunction.Items.Insert(0, New ListItem("------Tất cả -----", 0))
        drlFunction.SelectedValue = 0
    End Sub
    Private Sub BindToGrid(Optional ByVal iPage As Integer = 1, Optional ByVal strContent As String = "", Optional ByVal intFunction As Integer = 0, Optional ByVal intAction As Integer = 0, Optional ByVal dtmDateFrom As String = "", Optional ByVal dtmDateTo As String = "", Optional ByVal strIP As String = "", Optional ByVal strUserName As String = "")
        Using data As New ThanhTraLaoDongEntities
            Dim arrSearch() As String = {iPage.ToString, strContent, intFunction, intAction, dtmDateFrom, dtmDateTo, strIP, strUserName}
            ViewState("search") = arrSearch

            'So ban ghi muon the hien tren trang
            Dim odtmDateFrom As Object = Nothing
            If (dtmDateFrom.Trim <> "") Then
                odtmDateFrom = StringToDate(dtmDateFrom)
            End If

            Dim odtmDateTo As Object = Nothing
            If (dtmDateTo.Trim <> "") Then
                odtmDateTo = StringToDateTime(dtmDateTo, "23:55:55")
            End If
            Dim intPag_Size As Int32 = drpPage_Size.SelectedValue
            Dim p As List(Of uspLogSelectAll_Result) = data.uspLogSelectAll(IIf(String.IsNullOrEmpty(strContent) = True, Nothing, strContent), IIf(intFunction.ToString.Equals("0"), Nothing, intFunction), IIf(intAction.ToString.Equals("0"), Nothing, intAction), odtmDateFrom, odtmDateTo, IIf(String.IsNullOrEmpty(strIP) = True, Nothing, strIP), IIf(String.IsNullOrEmpty(strUserName) = True, Nothing, strUserName), iPage, intPag_Size).ToList()
            'Tong so ban ghi
            If p.Count > 0 Then
                hidCount.Value = p.FirstOrDefault.Total()
                Create_Pager(hidCount.Value, iPage, intPag_Size, 10)
            Else
                hidCount.Value = 0
                With rptPage
                    .DataSource = Nothing
                    .DataBind()
                End With
            End If
            Dim strKey_Name() As String = {"LogID", "FunctionName", "Content"}
            With grdShow
                .DataKeyNames = strKey_Name
                .DataSource = p
                .DataBind()
            End With
            If (hidCount.Value > 0) Then
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
        'dang ky async ko postback
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
        BindToGrid(CInt(lnkTile.ToolTip.ToString), arrSearch(1).ToString, CInt(arrSearch(2).ToString), CInt(arrSearch(3).ToString), arrSearch(4).ToString, arrSearch(5), arrSearch(6).ToString, arrSearch(7).ToString)
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
        BindToGrid(hidCur_Page.Value, arrSearch(1).ToString, CInt(arrSearch(2).ToString), CInt(arrSearch(3).ToString), arrSearch(4), arrSearch(5), arrSearch(6).ToString, arrSearch(7).ToString)

    End Sub
    Protected Sub lnkLast_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkLast.Click
        hidCur_Page.Value = hidCur_Page.Value + 1
        Dim arrSearch() As String
        arrSearch = ViewState("search")
        BindToGrid(hidCur_Page.Value, arrSearch(1).ToString, CInt(arrSearch(2).ToString), CInt(arrSearch(3).ToString), arrSearch(4), arrSearch(5), arrSearch(6).ToString, arrSearch(7).ToString)

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
        Dim strLogName As String = ""
        Using data As New ThanhTraLaoDongEntities
            intId = grdShow.DataKeys(hidID.Value)("LogID").ToString
            Dim q = (From p In Data.Logs Where p.LogID = intId Select p).FirstOrDefault
            Try
                data.Logs.DeleteObject(q)
                Data.SaveChanges()
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
                        intId = grdShow.DataKeys(item.RowIndex)("LogID").ToString
                        Dim q = (From p In Data.Logs Where p.LogID = intId Select p).FirstOrDefault
                        Try
                            data.Logs.DeleteObject(q)
                            data.SaveChanges()
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
            Dim lblSTT As Label = CType(e.Row.FindControl("lblSTT"), Label)
            lblSTT.Text = CInt(drpPage_Size.SelectedValue) * (CInt(hidCur_Page.Value) - 1).ToString + e.Row.RowIndex + 1
            Dim lnkbtnDelete As LinkButton = CType(e.Row.FindControl("lnkbtnDelete"), LinkButton)

            Dim ltrContent As Literal = CType(e.Row.FindControl("ltrContent"), Literal)
            ltrContent.Text = grdShow.DataKeys(e.Row.RowIndex)("Content").ToString
            'dang ky async ko postback
            'Dim ScriptManager As System.Web.UI.ScriptManager = System.Web.UI.ScriptManager.GetCurrent(Me.Page)
            'ScriptManager.RegisterAsyncPostBackControl(lnkbtnDelete)
            'Dim chkItem As CheckBox = CType(e.Row.FindControl("chkItem"), CheckBox)
            ''Permission
            'If HasPermission(Function_Name.Log, Session("RoleID"), 0, Audit_Type.Delete) = True Then
            '    lnkbtnDelete.Attributes.Add("onclick", "return ComfirmDialog('" + drpMessage.Items(0).Text + "', 0,'" + lnkbtnDelete.ClientID + "','" + e.Row.RowIndex.ToString + "',1);")
            'Else
            '    lnkbtnDelete.Enabled = False
            '    chkItem.Enabled = False
            'End If

        End If
    End Sub
    Protected Sub drpPage_Size_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles drpPage_Size.SelectedIndexChanged
        If grdShow.Rows.Count > 0 Then
            Dim arrSearch() As String
            arrSearch = ViewState("search")
            BindToGrid(1, arrSearch(1).ToString, CInt(arrSearch(2).ToString), CInt(arrSearch(3).ToString), arrSearch(4).ToString, arrSearch(5).ToString, arrSearch(6).ToString, arrSearch(7).ToString)
        End If
    End Sub
#End Region
#Region "Search"
    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        BindToGrid(1, txtContent.Text.Trim(), CInt(drlFunction.SelectedValue), CInt(drlAction.SelectedValue), txtFrom.Text.Trim, txtTo.Text.Trim, txtClientip.Text.Trim, txtUsername.Text.Trim)
    End Sub
    Protected Sub btnHuy_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnHuy.Click
        BindToGrid()
        ResetControl()
    End Sub
#End Region

End Class
