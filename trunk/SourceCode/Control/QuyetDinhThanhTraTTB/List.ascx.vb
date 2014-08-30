Imports System.Data
Imports ThanhTraLaoDongModel
Imports Cls_Common

Partial Class Control_Quyetdinhthanhtra_List
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
            LoadData()
            hidUserName.Value = Session("UserName")
            BindToGrid(, , , , , hidUserName.Value)
            btnDelete.Attributes.Add("onclick", "return confirmMultiDelete('" & btnDelete.ClientID & "');")
        End If
    End Sub
    Private Sub LoadData()
        For i As Integer = 2013 To (Now.Year)
            Dim iTem As New ListItem(i.ToString(), i.ToString())
            ddlFromDate.Items.Add(iTem)
        Next
        ddlFromDate.Items.Insert(0, New ListItem(Cls_Common.Str_Opt_Chon, 2013))

        For i As Integer = 2013 To (Now.Year)
            Dim iTem As New ListItem(i.ToString(), i.ToString())
            ddlToDate.Items.Add(iTem)
        Next
        ddlToDate.Items.Insert(0, New ListItem(Cls_Common.Str_Opt_Chon, 9999))
    End Sub
    Private Sub BindToGrid(Optional ByVal iPage As Integer = 1,
                           Optional ByVal strSearch As String = "",
                           Optional ByVal iFromYear As Integer = 2013,
                           Optional ByVal iToYear As Integer = 9999,
                           Optional ByVal bStatus As Boolean = Nothing,
                           Optional ByVal strUsername As String = "")
        Using data As New ThanhTraLaoDongEntities
            Dim arrSearch() As String = {iPage.ToString, strSearch.ToString, iFromYear.ToString, iToYear.ToString, bStatus.ToString, strUsername}

            ViewState("search") = arrSearch
            'So ban ghi muon the hien tren trang
            Dim intPag_Size As Int32 = drpPage_Size.SelectedValue
            Dim p = data.uspQuyetDinhThanhTraSelectAll(strSearch, strUsername, iFromYear, iToYear, bStatus, iPage, intPag_Size).ToList
            Dim strKey_Name() As String = {"SoQuyetDinh", "SoDN", "IsEdited"}
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
        BindToGrid(lnkTile.ToolTip, arrSearch(1), arrSearch(2), arrSearch(3), arrSearch(4), arrSearch(5))
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
        BindToGrid(hidCur_Page.Value, arrSearch(1), arrSearch(2), arrSearch(3), arrSearch(4), arrSearch(5))

    End Sub
    Protected Sub lnkLast_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkLast.Click
        hidCur_Page.Value = hidCur_Page.Value + 1

        Dim arrSearch() As String
        arrSearch = ViewState("search")
        BindToGrid(hidCur_Page.Value, arrSearch(1), arrSearch(2), arrSearch(3), arrSearch(4), arrSearch(5))

    End Sub
    Protected Sub drpPage_Size_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles drpPage_Size.SelectedIndexChanged
        Dim arrSearch() As String
        arrSearch = ViewState("search")
        BindToGrid(1, arrSearch(1), arrSearch(2), arrSearch(3), arrSearch(4), arrSearch(5))
    End Sub
#End Region
#Region "Event for control"

    Protected Sub lnkbtnDelete_Click(ByVal sender As Object, ByVal e As EventArgs)
        Dim soQD As String = ""
        Dim strLogName As String = ""
        Using data As New ThanhTraLaoDongEntities
            soQD = grdShow.DataKeys(hidID.Value)("SoQuyetDinh").ToString
            Dim q = (From p In data.QuyetDinhThanhTras Where p.SoQuyetDinh.Equals(soQD) Select p).FirstOrDefault
            Try
                'Xóa bảng QuyetDinhTTDoanhNghiep và Doanh nghiệp
                Dim qdttdn = (From a In data.QuyetDinhTTDoanhNghieps Where a.QuyetDinhTT.Equals(q.SoQuyetDinh)).ToList
                If qdttdn.Count = 0 Then
                    data.QuyetDinhThanhTras.DeleteObject(q)
                    data.SaveChanges()
                    Excute_Javascript("Alertbox('Xóa dữ liệu thành công.');", Me.Page, True)
                Else
                    Excute_Javascript("Alertbox('Xóa thất bại. Quyết định này hiện tại có doanh nghiệp tham chiếu đến.');", Me.Page, True)
                End If
            Catch ex As Exception
                log4net.Config.XmlConfigurator.Configure()
                log.Error("Error error " & AddTabSpace(1) & Session("Username") & AddTabSpace(1) & "IP:" & GetIPAddress(), ex)
                Excute_Javascript("Alertbox('Xóa thất bại (" & ex.Message & ").');", Me.Page, True)
            End Try
        End Using
        Dim arrSearch() As String
        arrSearch = ViewState("search")
        BindToGrid(1, arrSearch(1), arrSearch(2), arrSearch(3), arrSearch(4), arrSearch(5))

    End Sub
    Protected Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        Dim soQD As String = ""
        Dim intCount As Integer
        Dim intTotal As Integer
        Dim strSQD = ""
        Using data As New ThanhTraLaoDongEntities
            Try
                For Each item As GridViewRow In grdShow.Rows
                    Dim chkItem As New CheckBox
                    chkItem = CType(item.FindControl("chkItem"), CheckBox)
                    If chkItem.Checked Then
                        intTotal += 1
                        soQD = grdShow.DataKeys(item.RowIndex)("SoQuyetDinh").ToString
                        Dim q = (From p In data.QuyetDinhThanhTras Where p.SoQuyetDinh.Equals(soQD) Select p).FirstOrDefault
                        Try
                            'Xóa bảng QuyetDinhTTDoanhNghiep và Doanh nghiệp
                            Dim qdttdn = (From a In data.QuyetDinhTTDoanhNghieps Where a.QuyetDinhTT.Equals(q.SoQuyetDinh)).ToList
                            If qdttdn.Count = 0 Then
                                data.QuyetDinhThanhTras.DeleteObject(q)
                                data.SaveChanges()
                                intCount += 1
                            Else
                                strSQD += q.SoQuyetDinh & ";"
                            End If
                        Catch ex As Exception
                        End Try
                    End If
                Next
                If intCount > 0 Then
                    Excute_Javascript("Alertbox('Xóa dữ liệu thành công. " & intCount.ToString & " /" & intTotal.ToString & " record." & If(Not strSQD.Equals(""), " Các số quyết định (" & strSQD & ") hiện tại có doanh nghiệp tham chiếu đến", "") & "');", Me.Page, True)
                Else
                    Excute_Javascript("Alertbox('Xóa thất bại." & If(Not strSQD.Equals(""), " Các số quyết định (" & strSQD & ") hiện tại có doanh nghiệp tham chiếu đến", "") & "');", Me.Page, True)
                End If
            Catch ex As Exception
                log4net.Config.XmlConfigurator.Configure()
                log.Error("Error error " & AddTabSpace(1) & Session("Username") & AddTabSpace(1) & "IP:" & GetIPAddress(), ex)
                Excute_Javascript("Alertbox('Xóa thất bại (" & ex.Message & ").');", Me.Page, True)
            End Try
        End Using
        Dim arrSearch() As String
        arrSearch = ViewState("search")
        BindToGrid(1, arrSearch(1), arrSearch(2), arrSearch(3), arrSearch(4), arrSearch(5))

    End Sub
    Protected Sub grdShow_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdShow.RowDataBound
        If e.Row.RowIndex >= 0 Then
            Dim lblSTT As Label = CType(e.Row.FindControl("lblSTT"), Label)
            lblSTT.Text = CInt(drpPage_Size.SelectedValue) * (CInt(hidCur_Page.Value) - 1).ToString + e.Row.RowIndex + 1
            Dim lnkbtnDelete As LinkButton = CType(e.Row.FindControl("lnkbtnDelete"), LinkButton)
            If IsNothing(grdShow.DataKeys(e.Row.RowIndex)("IsEdited")) OrElse grdShow.DataKeys(e.Row.RowIndex)("IsEdited") = False Then
                lnkbtnDelete.Attributes.Add("onclick", "return ComfirmDialog('" + drpMessage.Items(0).Text + "', 0,'" + lnkbtnDelete.ClientID + "','" + e.Row.RowIndex.ToString + "',1);")
            Else
                lnkbtnDelete.Enabled = False
            End If
            Dim hplEdit As HyperLink = CType(e.Row.FindControl("hplEdit"), HyperLink)
            hplEdit.NavigateUrl = "../../Page/QuyetdinhthanhtraTTB/Edit.aspx?SoQuyetDinh=" & grdShow.DataKeys(e.Row.RowIndex)("SoQuyetDinh").ToString
            Dim hplSoQuyetDinh As HyperLink = CType(e.Row.FindControl("hplSoQuyetDinh"), HyperLink)
            hplSoQuyetDinh.NavigateUrl = "../../Page/QuyetdinhthanhtraTTB/Detail.aspx?SoQuyetDinh=" & grdShow.DataKeys(e.Row.RowIndex)("SoQuyetDinh").ToString
            hplSoQuyetDinh.Text = grdShow.DataKeys(e.Row.RowIndex)("SoQuyetDinh").ToString


            Dim lnkbtnSoQD As LinkButton = CType(e.Row.FindControl("lnkbtnSoQD"), LinkButton)
            lnkbtnSoQD.Text = grdShow.DataKeys(e.Row.RowIndex)("SoDN").ToString
            lnkbtnSoQD.Attributes.Add("onclick", "getDSDN('" + grdShow.DataKeys(e.Row.RowIndex)("SoQuyetDinh").ToString + "');")
        End If
    End Sub
#End Region
#Region "Search"
    Protected Sub btnFilter_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFilter.Click
        Dim bStatus As Boolean = IIf(ddlTrangThai.SelectedValue.Equals(""), Nothing, ddlTrangThai.SelectedValue)
         BindToGrid(1, txtTitleFilter.Text.Trim(), CInt(ddlFromDate.SelectedValue), CInt(ddlToDate.SelectedValue), bStatus)
    End Sub
#End Region

    Protected Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClear.Click
        txtTitleFilter.Text = ""
        ddlFromDate.SelectedIndex = 0
        ddlToDate.SelectedIndex = 0
        ddlTrangThai.SelectedValue = ""
        BindToGrid(, , , , , hidUserName.Value)
    End Sub
End Class
