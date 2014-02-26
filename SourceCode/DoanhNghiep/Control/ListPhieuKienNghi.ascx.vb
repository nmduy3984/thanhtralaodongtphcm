
Imports System.Data
Imports ThanhTraLaoDongModel
Imports Cls_Common
Imports SecurityService
Partial Class DoanhNghiep_Control_ListPhieuKienNghi
    Inherits System.Web.UI.UserControl
    Private Shared ReadOnly log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Dim script As ScriptManager = ScriptManager.GetCurrent(Me.Page)
            If Not script Is Nothing AndAlso script.IsInAsyncPostBack Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.Page.GetType, "jqueryEnable", "ajaxJquery()", True)
            Else
                Me.Page.ClientScript.RegisterStartupScript(Me.Page.GetType(), "jqueryEnable", String.Concat("Sys.Application.add_load(function(){", "ajaxJquery()", "});"), True)
            End If
            BindToGrid()
            btnDelete.Attributes.Add("onclick", "return confirmMultiDelete('" & btnDelete.ClientID & "');")
        End If
    End Sub
    Private Sub BindToGrid(Optional ByVal iPage As Integer = 1, Optional ByVal strSearch As String = "")
        Using data As New ThanhTraLaoDongEntities
            Dim arrSearch() As String = {iPage.ToString, strSearch.ToString}
            ViewState("search") = arrSearch
            Dim isUser As Integer = CInt(Session("IsUser"))
            Dim UserId As Integer = CInt(Session("UserId"))
            Dim strUserName As String = Session("UserName")

            'So ban ghi muon the hien tren trang
            Dim intPag_Size As Int32 = drpPage_Size.SelectedValue

            'Dim p As New List(Of uspPhieuNhapHeaderSelectAll_Result)
            Dim _NguoiTao As String = Session("UserName")
            Dim p = (From q In data.PhieuNhapHeaders Where q.NguoiTao = _NguoiTao And (q.TenPhieu.Contains(strSearch)) Order By q.NgayTao Descending Select q).Skip((iPage - 1) * intPag_Size).Take(intPag_Size).ToList

            Dim strKey_Name() As String = {"PhieuID", "TenPhieu", "DoanhNghiepId", "NguoiTao", "NgayTao", "CauHoiDaTraLoi"}
            'Tong so ban ghi
            If p.Count > 0 Then
                Dim pn = (From q In data.PhieuNhapHeaders Where q.NguoiTao = _NguoiTao Select q).ToList()
                hidCount.Value = pn.Count
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
    Protected Sub btnSubmit_Click(ByVal sender As Object, ByVal e As EventArgs)
        ' Dim abc = sender
        Try
            Session("phieuid") = grdShow.DataKeys(hidIndex.Value)("PhieuID").ToString
            Session("ModePhieu") = ModePhieu.Edit
            Response.Redirect("PhieuTuKiemTra/ThongTinChung.aspx?DNId=" + grdShow.DataKeys(hidIndex.Value)("DoanhNghiepId").ToString)
        Catch ex As Exception
            log4net.Config.XmlConfigurator.Configure()
            log.Error("Error error " & AddTabSpace(1) & Session("Username") & AddTabSpace(1) & "IP:" & GetIPAddress(), ex)
        End Try
    End Sub
    Protected Sub btnTenPhieu_Click(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Session("phieuid") = grdShow.DataKeys(hidIndex.Value)("PhieuID").ToString
            Session("ModePhieu") = ModePhieu.Detail
            Response.Redirect("PhieuTuKiemTra/ThongTinChung.aspx?DNId=" + grdShow.DataKeys(hidIndex.Value)("DoanhNghiepId").ToString)
        Catch ex As Exception
            log4net.Config.XmlConfigurator.Configure()
            log.Error("Error error " & AddTabSpace(1) & Session("Username") & AddTabSpace(1) & "IP:" & GetIPAddress(), ex)
        End Try
    End Sub
    Protected Sub grdShow_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdShow.RowDataBound
        If e.Row.RowIndex >= 0 Then
            Dim obj = e.Row.DataItem()
            Dim lblSTT As Label = CType(e.Row.FindControl("lblSTT"), Label)
            lblSTT.Text = CInt(drpPage_Size.SelectedValue) * (CInt(hidCur_Page.Value) - 1).ToString + e.Row.RowIndex + 1
            'Tên doanh nghiệp
            Dim lblDoanhNghiep As Label = CType(e.Row.FindControl("lblDoanhNghiep"), Label)
            lblDoanhNghiep.Text = grdShow.DataKeys(e.Row.RowIndex)("TenPhieu").ToString().Split("-")(1)
            'Ngay Tao
            Dim lblNgayTao As Label = CType(e.Row.FindControl("lblNgayTao"), Label)
            lblNgayTao.Text = CType(grdShow.DataKeys(e.Row.RowIndex)("NgayTao"), DateTime).ToString("dd/MM/yyyy HH:mm:ss")
            'Link BBTT
            Dim hplBBTT As HyperLink = CType(e.Row.FindControl("hplBBTT"), HyperLink)
            'Cau hoi da tra loi
            Dim lblCauHoiDaTraLoi As Label = CType(e.Row.FindControl("lblCauHoiDaTraLoi"), Label)
            If grdShow.DataKeys(e.Row.RowIndex)("CauHoiDaTraLoi").ToString().Split(";").Length >= 12 Then
                lblCauHoiDaTraLoi.Text = "Hoàn thành"
                hplBBTT.NavigateUrl = "../../DoanhNghiep/Page/PhieuKienNghi.aspx?phieuId=" & grdShow.DataKeys(e.Row.RowIndex)("PhieuID")
            Else
                lblCauHoiDaTraLoi.Text = "Chưa hoàn thành"
                hplBBTT.Enabled = False
            End If

            Dim strNguoiTao As String = grdShow.DataKeys(e.Row.RowIndex)("NguoiTao").ToString()
            Dim iVaiTro As Integer = grdShow.DataKeys(e.Row.RowIndex)("VaiTro")

             
            'Link tenphieu
            Dim lnkTenphieu As LinkButton = CType(e.Row.FindControl("lnkTenphieu"), LinkButton)
            lnkTenphieu.Attributes.Add("onclick", "setIndex(" + e.Row.RowIndex.ToString + ");")
            lnkTenphieu.Text = grdShow.DataKeys(e.Row.RowIndex)("TenPhieu").ToString()
             
        End If
    End Sub
#Region "Search"
    Protected Sub btnFilter_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFilter.Click
        BindToGrid(1, txtTitleFilter.Text.Trim())
    End Sub
#End Region
End Class
