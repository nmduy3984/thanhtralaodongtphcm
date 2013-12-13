Imports System.Data
Imports ThanhTraLaoDongModel
Imports Cls_Common
Imports SecurityService

Partial Class Control_TinhTP_List
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
            btnDelete.Attributes.Add("onclick", "return confirmMultiDelete('" & btnDelete.ClientID & "');")
        End If
    End Sub
    Private Sub BindToGrid(Optional ByVal iPage As Integer = 1, Optional ByVal strSearch As String = "")
        Using data As New ThanhTraLaoDongEntities
            'So ban ghi muon the hien tren trang
            Dim arrSearch() As String = {iPage.ToString, strSearch.ToString}
            ViewState("search") = arrSearch
            Dim intPag_Size As Int32 = drpPage_Size.SelectedValue
            Dim p = (From q In data.Tinhs Where q.TenTinh.Contains(strSearch) Or q.MaTinh.Contains(strSearch) Order By q.MaTinh Ascending Select q).Skip((iPage - 1) * intPag_Size).Take(intPag_Size).ToList
            Dim strKey_Name() As String = {"TinhId", "TenTinh", "MaTinh"}
            'Tong so ban ghi
            If p.Count > 0 Then
                Dim tinh = (From q In data.Tinhs Where q.TenTinh.Contains(strSearch) Or q.MaTinh.Contains(strSearch) Order By q.MaTinh Ascending Select q).ToList
                hidCount.Value = tinh.Count()
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
    Protected Sub lnkTitle_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim lnkTile As LinkButton = CType(sender, LinkButton)
        Dim arrSearch() As String
        arrSearch = ViewState("search")
        BindToGrid(lnkTile.ToolTip, arrSearch(1))
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
        BindToGrid(hidCur_Page.Value, arrSearch(1))
    End Sub
    Protected Sub lnkLast_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkLast.Click
        hidCur_Page.Value = hidCur_Page.Value + 1
        Dim arrSearch() As String
        arrSearch = ViewState("search")
        BindToGrid(hidCur_Page.Value, arrSearch(1))
    End Sub
#End Region

#Region "Event for control"
    Protected Sub drpPage_Size_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles drpPage_Size.SelectedIndexChanged
        BindToGrid(1)
    End Sub
    Protected Sub lnkbtnDelete_Click(ByVal sender As Object, ByVal e As EventArgs)
        Dim intId As Integer
        Dim strLogName As String = ""
        Using data As New ThanhTraLaoDongEntities
            intId = grdShow.DataKeys(hidID.Value)("TinhId").ToString
           
            Dim q = (From p In data.Tinhs Where p.TinhId = intId Select p).FirstOrDefault
            Try
                data.Tinhs.DeleteObject(q)
                data.SaveChanges()
                Insert_App_Log("Delete Tinh/TP:" & q.TenTinh & "", Function_Name.TinhTP, Audit_Type.Delete, Request.ServerVariables("REMOTE_ADDR"), Session("UserName"))
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
                        intId = grdShow.DataKeys(item.RowIndex)("TinhId").ToString

                        Dim q = (From p In data.Tinhs Where p.TinhId = intId Select p).FirstOrDefault
                        Try
                            data.Tinhs.DeleteObject(q)
                            data.SaveChanges()
                            Insert_App_Log("Delete Tinh/TP:" & q.TenTinh & "", Function_Name.TinhTP, Audit_Type.Delete, Request.ServerVariables("REMOTE_ADDR"), Session("UserName"))
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

            Dim hplEdit As HyperLink = CType(e.Row.FindControl("hplEdit"), HyperLink)
            hplEdit.NavigateUrl = "../../Page/TinhTP/Edit.aspx?TinhId=" & grdShow.DataKeys(e.Row.RowIndex)("TinhId").ToString
            Dim hplCode As HyperLink = CType(e.Row.FindControl("hplCode"), HyperLink)
            hplCode.NavigateUrl = "../../Page/TinhTP/Detail.aspx?TinhId=" & grdShow.DataKeys(e.Row.RowIndex)("TinhId").ToString
            hplCode.Text = grdShow.DataKeys(e.Row.RowIndex)("MaTinh").ToString

            Dim chkItem As CheckBox = CType(e.Row.FindControl("chkItem"), CheckBox)
            'Permission
            hplEdit.Enabled = HasPermission(Function_Name.TinhTP, Session("RoleID"), 0, Audit_Type.Edit)
            If HasPermission(Function_Name.TinhTP, Session("RoleID"), 0, Audit_Type.Delete) = True Then
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
        BindToGrid(1, txtTitleFilter.Text.Trim())
    End Sub
#End Region

End Class
