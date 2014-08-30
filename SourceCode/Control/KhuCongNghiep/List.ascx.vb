Imports System.Data
Imports ThanhTraLaoDongModel
Imports Cls_Common
Imports SecurityService

Partial Class Control_KhuCongNghiep_List
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
            BindToGrid(, , CInt(hidTinhThanhTraSo.Value))
            btnDelete.Attributes.Add("onclick", "return confirmMultiDelete('" & btnDelete.ClientID & "');")
        End If
    End Sub

    Private Sub BindToGrid(Optional ByVal iPage As Integer = 1,
                           Optional ByVal strSearch As String = "",
                           Optional ByVal _TinhId As Integer = 0)
        Using data As New ThanhTraLaoDongEntities
            Dim arrSearch() As String = {iPage, strSearch, _TinhId.ToString}
            ViewState("search") = arrSearch
            'So ban ghi muon the hien tren trang
            Dim intPag_Size As Int32 = drpPage_Size.SelectedValue
            Dim p = (From q In data.KhuCongNghieps Where q.TenKhuCongNghiep.Contains(strSearch) _
                                                 And (_TinhId = 0 Or q.TinhId = _TinhId)
                                                    Order By q.TenKhuCongNghiep Ascending _
                                                    Select q.KhuCongNghiepId, q.TenKhuCongNghiep, q.Tinh.TenTinh, q.Mota, q.NguoiTao, q.NgayTao).Skip((iPage - 1) * intPag_Size).Take(intPag_Size).ToList
            Dim strKey_Name() As String = {"KhuCongNghiepId", "TenKhuCongNghiep"}
            'Tong so ban ghi
            If p.Count > 0 Then
                Dim count = (From q In data.KhuCongNghieps Where q.TenKhuCongNghiep.Contains(strSearch) _
                                                  And (_TinhId = 0 Or q.TinhId = _TinhId) Select q.KhuCongNghiepId).ToList().Count
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
        Dim arrSearch = ViewState("search")
        BindToGrid(hidCur_Page.Value, arrSearch(1), arrSearch(2))
    End Sub

#End Region

#Region "Event for control"

    Protected Sub drpPage_Size_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles drpPage_Size.SelectedIndexChanged
        Dim arrSearch() As String
        arrSearch = ViewState("search")
        BindToGrid(1, arrSearch(1), arrSearch(2))

    End Sub

    Protected Sub lnkbtnDelete_Click(ByVal sender As Object, ByVal e As EventArgs)
        Dim intId As Integer
        Dim strLogName As String = ""
        Using data As New ThanhTraLaoDongEntities
            intId = grdShow.DataKeys(hidID.Value)("KhuCongNghiepId").ToString

            Dim q = (From p In data.KhuCongNghieps Where p.KhuCongNghiepId = intId Select p).FirstOrDefault
            Try
                Dim dn = (From a In data.DoanhNghieps Where a.KhuCongNghiepId = intId).ToList
                If dn.Count = 0 Then
                    data.KhuCongNghieps.DeleteObject(q)
                    data.SaveChanges()
                    Insert_App_Log("Delete KhuCongNghiep:" & q.TenKhuCongNghiep & "", Function_Name.KhuCongNghiep, Audit_Type.Delete, Request.ServerVariables("REMOTE_ADDR"), Session("UserName"))
                    Excute_Javascript("Alertbox('Xóa dữ liệu thành công.');", Me.Page, True)
                Else
                    Excute_Javascript("Alertbox('Xóa thất bại. Khu công nghiệp này hiện tại có doanh nghiệp tham chiếu đến.');", Me.Page, True)
                End If
            Catch ex As Exception
                log4net.Config.XmlConfigurator.Configure()
                log.Error("Error error " & AddTabSpace(1) & Session("Username") & AddTabSpace(1) & "IP:" & GetIPAddress(), ex)
                Excute_Javascript("Alertbox('Xóa thất bại (" & ex.Message & ").');", Me.Page, True)
            End Try
        End Using
        BindToGrid(, , CInt(hidTinhThanhTraSo.Value))
    End Sub

    Protected Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        Dim intId As Integer = 0
        Dim intCount As Integer
        Dim intTotal As Integer
        Dim strKCN = ""
        Using data As New ThanhTraLaoDongEntities
            Try
                For Each item As GridViewRow In grdShow.Rows
                    Dim chkItem As New CheckBox
                    chkItem = CType(item.FindControl("chkItem"), CheckBox)
                    If chkItem.Checked Then
                        intTotal += 1
                        intId = grdShow.DataKeys(item.RowIndex)("KhuCongNghiepId").ToString
                        Dim q = (From p In data.KhuCongNghieps Where p.KhuCongNghiepId = intId Select p).FirstOrDefault
                        Dim dn = (From a In data.DoanhNghieps Where a.KhuCongNghiepId = intId).ToList
                        If dn.Count = 0 Then
                            data.KhuCongNghieps.DeleteObject(q)
                            data.SaveChanges()
                            intCount += 1
                        Else
                            strKCN += q.TenKhuCongNghiep & ";"
                        End If
                    End If
                Next
                If intCount > 0 Then
                    Excute_Javascript("Alertbox('Xóa dữ liệu thành công." & intCount.ToString & " /" & intTotal.ToString & " record. " & If(Not strKCN.Equals(""), " Các khu công nghiệp (" & strKCN & ") hiện tại có doanh nghiệp tham chiếu đến", "") & "');", Me.Page, True)
                Else
                    Excute_Javascript("Alertbox('Xóa thất bại." & If(Not strKCN.Equals(""), " Các khu công nghiệp (" & strKCN & ") hiện tại có doanh nghiệp tham chiếu đến", "") & "');", Me.Page, True)
                End If
            Catch ex As Exception
                log4net.Config.XmlConfigurator.Configure()
                log.Error("Error error " & AddTabSpace(1) & Session("Username") & AddTabSpace(1) & "IP:" & GetIPAddress(), ex)
                Excute_Javascript("Alertbox('Xóa thất bại (" & ex.Message & ").');", Me.Page, True)
            End Try
        End Using
        BindToGrid(, , CInt(hidTinhThanhTraSo.Value))
    End Sub

    Protected Sub grdShow_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdShow.RowDataBound
        If e.Row.RowIndex >= 0 Then
            Dim lblSTT As Label = CType(e.Row.FindControl("lblSTT"), Label)
            lblSTT.Text = CInt(drpPage_Size.SelectedValue) * (CInt(hidCur_Page.Value) - 1).ToString + e.Row.RowIndex + 1
            Dim lnkbtnDelete As LinkButton = CType(e.Row.FindControl("lnkbtnDelete"), LinkButton)

            Dim hplEdit As HyperLink = CType(e.Row.FindControl("hplEdit"), HyperLink)
            hplEdit.NavigateUrl = "../../Page/KhuCongNghiep/Edit.aspx?KhuCongNghiepId=" & grdShow.DataKeys(e.Row.RowIndex)("KhuCongNghiepId").ToString

            Dim hplTitle As HyperLink = CType(e.Row.FindControl("hplTitle"), HyperLink)
            hplTitle.NavigateUrl = "../../Page/KhuCongNghiep/Detail.aspx?KhuCongNghiepId=" & grdShow.DataKeys(e.Row.RowIndex)("KhuCongNghiepId").ToString
            hplTitle.Text = grdShow.DataKeys(e.Row.RowIndex)("TenKhuCongNghiep").ToString

            Dim chkItem As CheckBox = CType(e.Row.FindControl("chkItem"), CheckBox)
            'Permission
            hplEdit.Enabled = HasPermission(Function_Name.KhuCongNghiep, Session("RoleID"), 0, Audit_Type.Edit)
            If HasPermission(Function_Name.KhuCongNghiep, Session("RoleID"), 0, Audit_Type.Delete) = True Then
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
        BindToGrid(1, txtTitleFilter.Text.Trim(), CInt(hidTinhThanhTraSo.Value))
    End Sub

#End Region

    Protected Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClear.Click
        txtTitleFilter.Text = ""
        BindToGrid(, , CInt(hidTinhThanhTraSo.Value))
    End Sub
End Class
