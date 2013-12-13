Imports System.Data
Imports ThanhTraLaoDongModel
Imports Cls_Common
Imports SecurityService
Partial Class Control_HanhVi_PopupHanhVi
    Inherits System.Web.UI.UserControl
    Private Shared ReadOnly log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
#Region "Sub and Function"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Dim script As ScriptManager = ScriptManager.GetCurrent(Me.Page)
            If Not script Is Nothing AndAlso script.IsInAsyncPostBack Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.Page.GetType, "TTLDjs", "ajaxJquery()", True)
            Else
                Me.Page.ClientScript.RegisterStartupScript(Me.Page.GetType(), "TTLDjs", String.Concat("Sys.Application.add_load(function(){", "ajaxJquery()", "});"), True)
            End If
            BindToGrid()
            btnSelect.Attributes.Add("onclick", "return SelectHanhVi();")
        End If
    End Sub
    
    Private Sub BindToGrid(Optional ByVal iPage As Integer = 1, _
                           Optional ByVal NoiDungHV As String = "")
        Using data As New ThanhTraLaoDongEntities
            Dim arrSearch() As String = {iPage.ToString, NoiDungHV}
            ViewState("search") = arrSearch
            'So ban ghi muon the hien tren trang
            Dim intPag_Size As Int32 = drpPage_Size.SelectedValue
            Dim p = (From q In data.DanhMucHanhVis, a In data.vLoaiHanhVis
                     Where a.Id = q.LoaiHanhVi And (q.Title.Contains(NoiDungHV) Or NoiDungHV = "") Order By q.HanhViId Descending Select New With {q.HanhViId, q.Title, q.MucPhatMax, q.MucPhatMin, .LoaiHanhVi = a.Name}).ToList().Skip((iPage - 1) * intPag_Size).Take(intPag_Size)
            Dim strKey_Name() As String = {"HanhViId", "MucPhatMin", "MucPhatMax"}
            'Tong so ban ghi
            If (Not p Is Nothing) Then
                hidCount.Value = (From q In data.DanhMucHanhVis, a In data.vLoaiHanhVis
                     Where a.Id = q.LoaiHanhVi And (q.Title.Contains(NoiDungHV) Or NoiDungHV = "")).Count
            End If
            Create_Pager(hidCount.Value, iPage, intPag_Size, 6)
            With grdShow
                .DataKeyNames = strKey_Name
                .DataSource = p
                .DataBind()
            End With
            lblTotal.Text = "Hiển thị " + (((iPage - 1) * intPag_Size) + 1).ToString + " đến " + (((iPage - 1) * intPag_Size) + grdShow.Rows.Count).ToString + " trong tổng số " + hidCount.Value.ToString + " bản ghi."
        End Using
    End Sub
    Protected Sub grdShow_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdShow.RowDataBound
        If e.Row.RowIndex >= 0 Then
            Dim lblSTT As Label = CType(e.Row.FindControl("lblSTT"), Label)
            lblSTT.Text = CInt(drpPage_Size.SelectedValue) * (CInt(hidCur_Page.Value) - 1).ToString + e.Row.RowIndex + 1
            Dim lblMucPhatMin As Label = CType(e.Row.FindControl("lblMucPhatMin"), Label)
            lblMucPhatMin.Text = String.Format("{0:n0}", grdShow.DataKeys(e.Row.RowIndex)("MucPhatMin"))
            Dim lblMucPhatMax As Label = CType(e.Row.FindControl("lblMucPhatMax"), Label)
            lblMucPhatMax.Text = String.Format("{0:n0}", grdShow.DataKeys(e.Row.RowIndex)("MucPhatMax"))
        End If
    End Sub
    Protected Sub drpPage_Size_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles drpPage_Size.SelectedIndexChanged
        Dim arrSearch() As String
        arrSearch = ViewState("search")
        BindToGrid(1, arrSearch(1))
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

    Protected Sub btnFilter_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFilter.Click
        BindToGrid(1, txtHanhVi.Text.Trim())
    End Sub

    Protected Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClear.Click
        txtHanhVi.Text = ""
        BindToGrid(1, txtHanhVi.Text.Trim())
    End Sub

  
End Class
