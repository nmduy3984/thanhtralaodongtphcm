Imports System.Data
Imports ThanhTraLaoDongModel
Imports Cls_Common
Imports SecurityService
Partial Class Control_CauHoi_ListBaoCaoThucHien
    Inherits System.Web.UI.UserControl
    Private Shared ReadOnly log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
#Region "Sub and Function"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            If Not Session("Username") = "" Then
                Dim script As ScriptManager = ScriptManager.GetCurrent(Me.Page)
                If Not script Is Nothing AndAlso script.IsInAsyncPostBack Then
                    ScriptManager.RegisterStartupScript(Me.Page, Me.Page.GetType, "duyjs", "ajaxJquery()", True)
                    ScriptManager.RegisterStartupScript(Me.Page, Me.Page.GetType, "duyjs1", "ajaxJqueryToolTip()", True)
                Else
                    Me.Page.ClientScript.RegisterStartupScript(Me.Page.GetType(), "duyjs", String.Concat("Sys.Application.add_load(function(){", "ajaxJquery()", "});"), True)
                    Me.Page.ClientScript.RegisterStartupScript(Me.Page.GetType(), "duyjs1", String.Concat("Sys.Application.add_load(function(){", "ajaxJqueryToolTip()", "});"), True)
                End If
                hidTinhThanhTraSo.Value = Session("TinhThanhTraSo")
                hidIsUser.Value = Session("IsUser")
                hidUserId.Value = Session("UserId")
                hidUserName.Value = Session("UserName")
                BindToGrid(, , , , , 1)
            Else
                Response.Redirect("../../Login.aspx")
            End If
        End If
    End Sub
    Private Sub BindToGrid(Optional ByVal iPage As Integer = 1,
                            Optional ByVal strSearch As String = "",
                            Optional ByVal strTinh As String = "",
                            Optional ByVal strNam As String = "",
                            Optional ByVal strSoQD As String = "",
                            Optional ByVal iFilterUser As Integer = 0)
        Using data As New ThanhTraLaoDongEntities
            Dim arrSearch() As String = {iPage.ToString, strSearch.ToString, strTinh.ToString, strNam.ToString, strSoQD.ToString, iFilterUser.ToString}
            ViewState("search") = arrSearch

            'So ban ghi muon the hien tren trang
            Dim intPag_Size As Int32 = drpPage_Size.SelectedValue

            Dim p = data.uspBaoCaoThucHien(strSearch, strTinh, strNam, strSoQD, iFilterUser, hidUserName.Value, hidIsUser.Value, hidUserId.Value, iPage, intPag_Size).ToList

            Dim strKey_Name() As String = {"PhieuID", "TenPhieu", "DoanhNghiepId", "NguoiTao", "VaiTro", "IsHoanThanh"}
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
#End Region
#Region "Event for control"
    Protected Sub btnSubmit_Click(ByVal sender As Object, ByVal e As EventArgs)
        ' Dim abc = sender
        Try
            Session("phieuid") = grdShow.DataKeys(hidIndex.Value)("PhieuID").ToString
            Session("ModePhieu") = ModePhieu.Edit
            Response.Redirect("ThongTinChung.aspx?DNId=" + grdShow.DataKeys(hidIndex.Value)("DoanhNghiepId").ToString)
        Catch ex As Exception
            log4net.Config.XmlConfigurator.Configure()
            log.Error("Error error " & AddTabSpace(1) & Session("Username") & AddTabSpace(1) & "IP:" & GetIPAddress(), ex)
        End Try
    End Sub

    Protected Sub grdShow_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdShow.RowDataBound
        If e.Row.RowIndex >= 0 Then
            'Tên doanh nghiệp
            Dim lblDoanhNghiep As Label = CType(e.Row.FindControl("lblDoanhNghiep"), Label)
            lblDoanhNghiep.Text = grdShow.DataKeys(e.Row.RowIndex)("TenPhieu").ToString().Split("-")(1)

            'Link BBTT
            Dim hplBCTH As HyperLink = CType(e.Row.FindControl("hplBCTH"), HyperLink)

            'Cau hoi da tra loi
            If grdShow.DataKeys(e.Row.RowIndex)("IsHoanThanh").ToString().Contains("Hoàn thành") Then
                hplBCTH.NavigateUrl = "../../Page/BienBanThanhTra/BaoCaoThucHien.aspx?phieuId=" & grdShow.DataKeys(e.Row.RowIndex)("PhieuID")
            Else
                hplBCTH.Enabled = False
            End If
            Dim lblTenphieu As Label = CType(e.Row.FindControl("lblTenphieu"), Label)
            lblTenphieu.Text = grdShow.DataKeys(e.Row.RowIndex)("TenPhieu").ToString()
        End If
    End Sub
#End Region
#Region "Search"
    Protected Sub btnFilter_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFilter.Click
        If hidTinhThanhTraSo.Value > 0 Then
            BindToGrid(1, txtTitleFilter.Text.Trim(), , txtNam.Text.Trim(), txtSoQD.Text.Trim())
        Else
            BindToGrid(1, txtTitleFilter.Text.Trim(), txtTinh.Text.Trim(), txtNam.Text.Trim(), txtSoQD.Text.Trim())
        End If
    End Sub
#End Region

    Protected Sub drpPage_Size_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles drpPage_Size.SelectedIndexChanged
        Dim arrSearch() As String
        arrSearch = ViewState("search")
        BindToGrid(1, arrSearch(1), arrSearch(2), arrSearch(3), arrSearch(4), arrSearch(5))
    End Sub

    Protected Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClear.Click
        BindToGrid(1, "", "", "", "")
        txtTitleFilter.Text = ""
    End Sub
    Protected Sub hplChonTinh_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles hplChonTinh.Init
        hplChonTinh.CssClass = "thickbox"
        hplChonTinh.NavigateUrl = "~/Page/TinhTP/PopupTinh.aspx?keepThis=true&TB_iframe=true&height=420&width=500&modal=true"
    End Sub
End Class
