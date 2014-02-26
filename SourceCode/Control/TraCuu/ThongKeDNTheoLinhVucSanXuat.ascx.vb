
Imports ThanhTraLaoDongModel
Imports Cls_Common
Imports SecurityService
Partial Class Control_TraCuu_ThongKeDNTheoLinhVucSanXuat
    Inherits System.Web.UI.UserControl

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
            LoadType()
        End If
    End Sub



    Protected Sub LoadType()

        With ddlNam
            .AppendDataBoundItems = True
            .Items.Add(New ListItem("---Tất cả---", "1900"))
            .DataSource = Enumerable.Range(2005, 11)
            .DataBind()
            .SelectedValue = Now.Year
        End With


        Using Data As New ThanhTraLaoDongEntities
            Dim lst = Nothing
            If hidTinhThanhTraSo.Value > 0 Then
                lst = (From q In Data.Tinhs Where q.TinhId = hidTinhThanhTraSo.Value Order By q.TenTinh Ascending
                      Select New With {.Value = q.TinhId, .Text = q.TenTinh})
                With ddlDiaPhuong
                    .Items.Clear()
                    .AppendDataBoundItems = True
                    .DataTextField = "Text"
                    .DataValueField = "Value"
                    .DataSource = lst
                    .DataBind()
                End With
            Else
                lst = (From q In Data.Tinhs Order By q.TenTinh Ascending
                          Select New With {.Value = q.TinhId, .Text = q.TenTinh})
                With ddlDiaPhuong
                    .Items.Clear()
                    .AppendDataBoundItems = True
                    .DataTextField = "Text"
                    .DataValueField = "Value"
                    .DataSource = lst
                    .DataBind()
                    .Items.Insert(0, New ListItem(Cls_Common.Str_Opt_ToanQuoc, "-1"))
                End With
            End If

            Dim lstLinhVuc = (From q In Data.LoaiHinhSanXuats Where q.ParentID = 0
               Order By q.Title
               Select New With {.Value = q.LoaiHinhSXId, .Text = q.Title})

            For Each a In lstLinhVuc
                Dim itm As New ListItem(a.Text, a.Value)
                'insert cap 2
                ddlLinhVuc.Items.Add(itm)
                Dim lstLinhVucSecond = (From q In Data.LoaiHinhSanXuats Where q.ParentID = a.Value
                      Order By q.Title
                      Select New With {.Value = q.LoaiHinhSXId, .Text = q.Title})

                For Each b In lstLinhVucSecond
                    Dim itmSecond As New ListItem(" --- " & b.Text, b.Value)
                    ddlLinhVuc.Items.Add(itmSecond)
                Next

            Next

            ddlLinhVuc.Items.Insert(0, New ListItem(Cls_Common.Str_Opt_TatCa, -1))


        End Using
    End Sub

#Region "PRIVATE EVENT FOR CONTROL"

    Private Sub BindToGrid(Optional ByVal iPage As Integer = 1 _
                            , Optional ByVal strTinh As Integer = 0 _
                            , Optional ByVal strLinhVucSXId As Integer = 0 _
                            , Optional ByVal strYearFrom As Integer = 0)
        Using data As New ThanhTraLaoDongEntities
            Dim arrSearch() As String = {iPage.ToString, strTinh.ToString, strLinhVucSXId.ToString, strYearFrom.ToString}

            ViewState("search") = arrSearch
            'So ban ghi muon the hien tren trang
            Dim intPag_Size As Int32 = 100
            Dim p = data.uspTraCuuThongKeDNTheoLinhVucSanXuat(strTinh, strLinhVucSXId, strYearFrom).ToList()
            'Tong so ban ghi
            Dim strKey_Name() As String = {"TinhId"}
            'Tong so ban ghi
            If p.Count > 1 Then
                hidCount.Value = p.Count
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

    Private Sub Button_Click(ByVal sender As Object, ByVal em As System.EventArgs) Handles btnExport.Click
        Dim intDiaPhuong As Integer = ddlDiaPhuong.SelectedValue
        Dim intLinhVucSXId As Integer = ddlLinhVuc.SelectedValue

        BindToGrid(1, intDiaPhuong, intLinhVucSXId, 0)
    End Sub

    Protected Sub btnHuy_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnHuy.Click
        ddlDiaPhuong.SelectedValue = 0
        ddlNam.SelectedIndex = -1
        ddlLinhVuc.SelectedIndex = -1

    End Sub
    Protected Sub grdShow_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdShow.RowDataBound
        If e.Row.RowIndex >= 0 Then

        End If
    End Sub
    Protected Sub drpPage_Size_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles drpPage_Size.SelectedIndexChanged
        Dim arrSearch() As String
        arrSearch = ViewState("search")
        BindToGrid(1, arrSearch(1), arrSearch(2), arrSearch(3))
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
        BindToGrid(CInt(lnkTile.ToolTip.ToString), arrSearch(1), arrSearch(2), arrSearch(3))
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
        BindToGrid(1, arrSearch(1), arrSearch(2), arrSearch(3))

    End Sub
    Protected Sub lnkLast_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkLast.Click
        hidCur_Page.Value = hidCur_Page.Value + 1

        Dim arrSearch() As String
        arrSearch = ViewState("search")
        BindToGrid(1, arrSearch(1), arrSearch(2), arrSearch(3))

    End Sub
#End Region
End Class
