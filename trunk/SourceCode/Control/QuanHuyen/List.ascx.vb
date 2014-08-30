Imports System.Data
Imports ThanhTraLaoDongModel
Imports Cls_Common
Imports SecurityService

Partial Class Control_QuanHuyen_List
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
            hidTinhThanhTraSo.Value = Session("TinhThanhTraSo")
            'bind muc luong
            LoadType(Request("LuongToiThieuID"))


            'SEARCH theo muc luong toi thieu

            If Not Request("LuongToiThieuID") Is Nothing Then
                BindToGrid(1, "", Request("LuongToiThieuID"), CInt(hidTinhThanhTraSo.Value))
            Else
                BindToGrid(, , , CInt(hidTinhThanhTraSo.Value))
            End If


            btnDelete.Attributes.Add("onclick", "return confirmMultiDelete('" & btnDelete.ClientID & "');")
        End If
    End Sub

    Protected Sub LoadType(ByVal _LuongToiThieuId As Integer)
        'Tinh-Thanh pho
        Using Data As New ThanhTraLaoDongEntities
            'Luong toi thieu
            Dim ltt = (From p In Data.LuongToiThieux
                       Select New With {.Value = p.LuongToiThieuID, .Text = p.TieuDe & " - " & p.MucLuongToiThieu})
            With ddlMucluong
                .Items.Clear()
                .AppendDataBoundItems() = True
                .DataTextField = "Text"
                .DataValueField = "Value"
                .DataSource = ltt
                .DataBind()
                .Items.Insert(0, New ListItem(Cls_Common.Str_Opt_TatCa, 0))
            End With
            ddlMucluong.SelectedValue = _LuongToiThieuId
        End Using
    End Sub

    Private Sub BindToGrid(Optional ByVal iPage As Integer = 1,
                           Optional ByVal strSearch As String = "",
                           Optional ByVal _LuongToiThieuID As Integer = 0,
                           Optional ByVal _TinhId As Integer = 0)
        Using data As New ThanhTraLaoDongEntities
            Dim arrSearch() As String = {iPage, strSearch, _LuongToiThieuID.ToString, _TinhId.ToString}
            ViewState("search") = arrSearch
            'So ban ghi muon the hien tren trang
            Dim intPag_Size As Int32 = drpPage_Size.SelectedValue
            Dim p = (From q In data.Huyens Where q.TenHuyen.Contains(strSearch) _
                                                And (_LuongToiThieuID = 0 Or q.LuongToiThieuID = _LuongToiThieuID) _
                                                And (_TinhId = 0 Or q.TinhId = _TinhId)
                                                    Order By q.TenHuyen Ascending _
                                                    Select q.HuyenId, q.TenHuyen, q.Tinh.TenTinh, q.LuongToiThieu.TieuDe, q.Mota).Skip((iPage - 1) * intPag_Size).Take(intPag_Size).ToList
            Dim strKey_Name() As String = {"HuyenId", "TenHuyen"}
            'Tong so ban ghi
            If p.Count > 0 Then
                Dim count = (From q In data.Huyens Where q.TenHuyen.Contains(strSearch) _
                                                And (_LuongToiThieuID = 0 Or q.LuongToiThieuID = _LuongToiThieuID) And (_TinhId = 0 Or q.TinhId = _TinhId) Select q.HuyenId).ToList().Count
                hidCount.Value = count
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
        BindToGrid(lnkTile.ToolTip, arrSearch(1), arrSearch(2), arrSearch(3))
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
        BindToGrid(hidCur_Page.Value, arrSearch(1), arrSearch(2), arrSearch(3))
    End Sub

    Protected Sub lnkLast_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkLast.Click
        hidCur_Page.Value = hidCur_Page.Value + 1
        Dim arrSearch = ViewState("search")
        BindToGrid(hidCur_Page.Value, arrSearch(1), arrSearch(2), arrSearch(3))
    End Sub

#End Region

#Region "Event for control"

    Protected Sub drpPage_Size_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles drpPage_Size.SelectedIndexChanged
        Dim arrSearch() As String
        arrSearch = ViewState("search")
        BindToGrid(1, arrSearch(1), arrSearch(2), arrSearch(3))

    End Sub

    Protected Sub lnkbtnDelete_Click(ByVal sender As Object, ByVal e As EventArgs)
        Dim intId As Integer
        Dim strLogName As String = ""
        Using data As New ThanhTraLaoDongEntities
            intId = grdShow.DataKeys(hidID.Value)("HuyenId").ToString

            Dim q = (From p In data.Huyens Where p.HuyenId = intId Select p).FirstOrDefault
            Try
                Dim dn = (From a In data.DoanhNghieps Where a.HuyenId = intId).ToList
                If dn.Count = 0 Then
                    data.Huyens.DeleteObject(q)
                    data.SaveChanges()
                    Insert_App_Log("Delete Quan/Huyen:" & q.TenHuyen & "", Function_Name.QuanHuyen, Audit_Type.Delete, Request.ServerVariables("REMOTE_ADDR"), Session("UserName"))
                    Excute_Javascript("Alertbox('Xóa dữ liệu thành công.');", Me.Page, True)
                Else
                    Excute_Javascript("Alertbox('Xóa thất bại. Huyện này hiện tại có doanh nghiệp tham chiếu đến.');", Me.Page, True)
                End If
            Catch ex As Exception
                log4net.Config.XmlConfigurator.Configure()
                log.Error("Error error " & AddTabSpace(1) & Session("Username") & AddTabSpace(1) & "IP:" & GetIPAddress(), ex)
                Excute_Javascript("Alertbox('Xóa thất bại (" & ex.Message & ").');", Me.Page, True)
            End Try
        End Using
        BindToGrid(, , , CInt(hidTinhThanhTraSo.Value))
    End Sub

    Protected Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        Dim intId As Integer = 0
        Dim intCount As Integer
        Dim intTotal As Integer
        Dim strHuyen = ""
        Using data As New ThanhTraLaoDongEntities
            Try
                For Each item As GridViewRow In grdShow.Rows
                    Dim chkItem As New CheckBox
                    chkItem = CType(item.FindControl("chkItem"), CheckBox)
                    If chkItem.Checked Then
                        intTotal += 1
                        intId = grdShow.DataKeys(item.RowIndex)("HuyenId").ToString
                        Dim q = (From p In data.Huyens Where p.HuyenId = intId Select p).FirstOrDefault
                        Dim dn = (From a In data.DoanhNghieps Where a.HuyenId = intId).ToList
                        If dn.Count = 0 Then
                            data.Huyens.DeleteObject(q)
                            data.SaveChanges()
                            Insert_App_Log("Delete Quan/Huyen:" & q.TenHuyen & "", Function_Name.QuanHuyen, Audit_Type.Delete, Request.ServerVariables("REMOTE_ADDR"), Session("UserName"))
                            intCount += 1
                        Else
                            strHuyen += q.TenHuyen & ";"
                        End If
                    End If
                Next
                If intCount > 0 Then
                    Excute_Javascript("Alertbox('Xóa dữ liệu thành công." & intCount.ToString & " /" & intTotal.ToString & " record. " & If(Not strHuyen.Equals(""), " Các huyện (" & strHuyen & ") hiện tại có doanh nghiệp tham chiếu đến", "") & "');", Me.Page, True)
                Else
                    Excute_Javascript("Alertbox('Xóa thất bại." & If(Not strHuyen.Equals(""), " Các huyện (" & strHuyen & ") hiện tại có doanh nghiệp tham chiếu đến", "") & "');", Me.Page, True)
                End If
            Catch ex As Exception
                log4net.Config.XmlConfigurator.Configure()
                log.Error("Error error " & AddTabSpace(1) & Session("Username") & AddTabSpace(1) & "IP:" & GetIPAddress(), ex)
                Excute_Javascript("Alertbox('Xóa thất bại (" & ex.Message & ").');", Me.Page, True)
            End Try
        End Using
        BindToGrid(, , , CInt(hidTinhThanhTraSo.Value))
    End Sub

    Protected Sub grdShow_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdShow.RowDataBound
        If e.Row.RowIndex >= 0 Then
            Dim lblSTT As Label = CType(e.Row.FindControl("lblSTT"), Label)
            lblSTT.Text = CInt(drpPage_Size.SelectedValue) * (CInt(hidCur_Page.Value) - 1).ToString + e.Row.RowIndex + 1
            Dim lnkbtnDelete As LinkButton = CType(e.Row.FindControl("lnkbtnDelete"), LinkButton)

            Dim hplEdit As HyperLink = CType(e.Row.FindControl("hplEdit"), HyperLink)
            hplEdit.NavigateUrl = "../../Page/QuanHuyen/Edit.aspx?HuyenId=" & grdShow.DataKeys(e.Row.RowIndex)("HuyenId").ToString

            Dim hplTitle As HyperLink = CType(e.Row.FindControl("hplTitle"), HyperLink)
            hplTitle.NavigateUrl = "../../Page/QuanHuyen/Detail.aspx?HuyenId=" & grdShow.DataKeys(e.Row.RowIndex)("HuyenId").ToString
            hplTitle.Text = grdShow.DataKeys(e.Row.RowIndex)("TenHuyen").ToString

            Dim chkItem As CheckBox = CType(e.Row.FindControl("chkItem"), CheckBox)
            'Permission
            hplEdit.Enabled = HasPermission(Function_Name.QuanHuyen, Session("RoleID"), 0, Audit_Type.Edit)
            If HasPermission(Function_Name.QuanHuyen, Session("RoleID"), 0, Audit_Type.Delete) = True Then
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
        BindToGrid(1, txtTitleFilter.Text.Trim(), ddlMucluong.SelectedValue, CInt(hidTinhThanhTraSo.Value))
    End Sub

#End Region

    Protected Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClear.Click
        txtTitleFilter.Text = ""
        ddlMucluong.SelectedIndex = -1
        BindToGrid(, , , CInt(hidTinhThanhTraSo.Value))
    End Sub
End Class
