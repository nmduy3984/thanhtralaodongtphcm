Imports System.Data
Imports ThanhTraLaoDongModel
Imports Cls_Common
Imports SecurityService
Partial Class Control_CauHoi_ListPKT_XLP
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
                hidIsUser.Value = Session("IsUser")
                hidUserId.Value = Session("UserId")
                hidUserName.Value = Session("UserName")

                BindToGrid(, , hidUserName.Value, , , Date.Now)
                btnDelete.Attributes.Add("onclick", "return confirmMultiDelete('" & btnDelete.ClientID & "');")
                btnKeThua.Attributes.Add("onclick", "return confirmMultiKeThua('" & btnKeThua.ClientID & "');")

            Else
                Response.Redirect("../../Login.aspx")
            End If
        End If
    End Sub
    Private Sub BindToGrid(Optional ByVal iPage As Integer = 1,
                           Optional ByVal strSearch As String = "",
                           Optional ByVal strNguoiTao As String = "",
                           Optional ByVal strTrangThai As Integer = 0,
                           Optional ByVal strNgayTaoTu As Date = #1/1/1900#,
                           Optional ByVal strNgayTaoDen As Date = #1/1/9999#)
        Using data As New ThanhTraLaoDongEntities
            Dim arrSearch() As String = {iPage.ToString, strSearch.ToString, strNguoiTao.ToString, strTrangThai.ToString, strNgayTaoTu.ToString, strNgayTaoDen.ToString}
            ViewState("search") = arrSearch

            'So ban ghi muon the hien tren trang
            Dim intPag_Size As Int32 = drpPage_Size.SelectedValue

            Dim p = data.uspPhieuNhapHeaderSelectAll(strSearch, 0, hidUserName.Value, hidIsUser.Value, hidUserId.Value, strNguoiTao, strTrangThai, strNgayTaoTu, strNgayTaoDen, iPage, intPag_Size).ToList

            Dim strKey_Name() As String = {"PhieuID", "TenDoanhNghiep", "DoanhNghiepId", "NguoiTao", "VaiTro", "IsHoanThanh"}
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
            'Thống kê nhanh
            Dim thongke = data.uspThongKePTKT(Session("UserName"), CInt(Session("UserId"))).SingleOrDefault
            If Not IsNothing(thongke) Then
                lblThongKe.Text = "<ul><b>Thống kê nhanh</b>"
                lblThongKe.Text += "<li>1. Số DN đang tự kiểm tra: <span style='color:blue;'><b>" & String.Format(info, "{0:n0}", thongke.CountPTKTKHT) & "</b></span><li>"
                lblThongKe.Text += "<li>2. Số DN đã tự kiểm tra: <span style='color:blue;'><b>" & String.Format(info, "{0:n0}", thongke.CountPTKTHT) & "</b></span><li>"
                lblThongKe.Text += "<li>3. Tổng số kiến nghị: <span style='color:blue;'><b>" & String.Format(info, "{0:n0}", thongke.CountKNDNPTKT) & "</b></span><li>"
                lblThongKe.Text += "</ul>"
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
    Protected Function DeletePhieu(ByVal iPhieuId As Integer) As Boolean

        Dim strLogName As String = ""
        Dim strTenPhieu = ""
        Using data As New ThanhTraLaoDongEntities
            Try

                strTenPhieu = (From p In data.PhieuNhapHeaders Where p.PhieuID = iPhieuId Select p.TenPhieu).FirstOrDefault
                Dim strSQLExcute As String = ""
                strSQLExcute = "DELETE FROM HanhViDN WHERE PhieuID=" & iPhieuId & " ; "
                strSQLExcute = strSQLExcute & "DELETE FROM KetLuan WHERE PhieuID=" & iPhieuId & " ; "
                strSQLExcute = strSQLExcute & "DELETE FROM KienNghiDN WHERE PhieuID=" & iPhieuId & " ; "
                strSQLExcute = strSQLExcute & "DELETE FROM CauHoi1 WHERE PhieuID=" & iPhieuId & " ; "
                strSQLExcute = strSQLExcute & "DELETE FROM CauHoi2 WHERE PhieuID=" & iPhieuId & " ; "
                strSQLExcute = strSQLExcute & "DELETE FROM CauHoi3 WHERE PhieuID=" & iPhieuId & " ; "
                strSQLExcute = strSQLExcute & "DELETE FROM CauHoi4 WHERE PhieuID=" & iPhieuId & " ; "
                strSQLExcute = strSQLExcute & "DELETE FROM CauHoi5 WHERE PhieuID=" & iPhieuId & " ; "
                strSQLExcute = strSQLExcute & "DELETE FROM CauHoi6 WHERE PhieuID=" & iPhieuId & " ; "
                strSQLExcute = strSQLExcute & "DELETE FROM CauHoi7 WHERE PhieuID=" & iPhieuId & " ; "
                strSQLExcute = strSQLExcute & "DELETE FROM CauHoi8 WHERE PhieuID=" & iPhieuId & " ; "
                strSQLExcute = strSQLExcute & "DELETE FROM CauHoi9 WHERE PhieuID=" & iPhieuId & " ; "
                strSQLExcute = strSQLExcute & "DELETE FROM CauHoi10 WHERE PhieuID=" & iPhieuId & " ; "
                strSQLExcute = strSQLExcute & "DELETE FROM CauHoi11 WHERE PhieuID=" & iPhieuId & " ; "
                strSQLExcute = strSQLExcute & "DELETE FROM CauHoi12 WHERE PhieuID=" & iPhieuId & " ; "
                strSQLExcute = strSQLExcute & "DELETE FROM PhieuNhapHeader WHERE PhieuID=" & iPhieuId & " ; "

                data.ExecuteStoreCommand(strSQLExcute)
                data.SaveChanges()
                Insert_App_Log("Delete  Phieunhapheader:" & strTenPhieu & "", Function_Name.PhieuKiemTra_XLP, Audit_Type.Delete, Request.ServerVariables("REMOTE_ADDR"), Session("UserName"))
                Return True
            Catch ex As Exception
                log4net.Config.XmlConfigurator.Configure()
                log.Error("Error error " & AddTabSpace(1) & Session("Username") & AddTabSpace(1) & "IP:" & GetIPAddress(), ex)
                Return False
            End Try
        End Using
    End Function

    Protected Sub lnkbtnDelete_Click(ByVal sender As Object, ByVal e As EventArgs)
        Dim bResult As Boolean = DeletePhieu(grdShow.DataKeys(hidID.Value)("PhieuID").ToString)
        If bResult Then
            Excute_Javascript("Alertbox('Xóa dữ liệu thành công.');", Me.Page, True)
        Else
            Excute_Javascript("Alertbox('Xóa thất bại.');", Me.Page, True)
        End If
        BindToGrid()
    End Sub
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
    Protected Sub btnTenPhieu_Click(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Session("phieuid") = grdShow.DataKeys(hidIndex.Value)("PhieuID").ToString
            Session("ModePhieu") = ModePhieu.Detail
            Response.Redirect("ThongTinChung.aspx?DNId=" + grdShow.DataKeys(hidIndex.Value)("DoanhNghiepId").ToString)
        Catch ex As Exception
            log4net.Config.XmlConfigurator.Configure()
            log.Error("Error error " & AddTabSpace(1) & Session("Username") & AddTabSpace(1) & "IP:" & GetIPAddress(), ex)
        End Try
    End Sub
    'Protected Sub btnBBTT_Click(ByVal sender As Object, ByVal e As System.EventArgs)
    '    Try
    '        Dim strPhieuId As String = IIf(IsNothing(grdShow.DataKeys(hidIndex.Value)("PhieuID")), "", grdShow.DataKeys(hidIndex.Value)("PhieuID").ToString())
    '        Response.Redirect("BienBanThanhTra.aspx?phieuId=" + strPhieuId)
    '    Catch ex As Exception
    '        log4net.Config.XmlConfigurator.Configure()
    '        log.Error("Error error " & AddTabSpace(1) & Session("Username") & AddTabSpace(1) & "IP:" & GetIPAddress(), ex)
    '    End Try
    'End Sub
    'Protected Sub btnKLTT_Click(ByVal sender As Object, ByVal e As System.EventArgs)
    '    Try
    '        Dim strPhieuId As String = IIf(IsNothing(grdShow.DataKeys(hidIndex.Value)("PhieuID")), "", grdShow.DataKeys(hidIndex.Value)("PhieuID").ToString())
    '        Response.Redirect("KetLuanThanhTra.aspx?phieuId=" + strPhieuId)
    '    Catch ex As Exception
    '        log4net.Config.XmlConfigurator.Configure()
    '        log.Error("Error error " & AddTabSpace(1) & Session("Username") & AddTabSpace(1) & "IP:" & GetIPAddress(), ex)
    '    End Try
    'End Sub
    Protected Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        Dim iPhieuId As Integer = 0
        Dim intCount As Integer
        Dim intTotal As Integer
        Using data As New ThanhTraLaoDongEntities
            Try
                For Each item As GridViewRow In grdShow.Rows
                    Dim chkItem As New CheckBox
                    chkItem = CType(item.FindControl("chkItem"), CheckBox)
                    If chkItem.Checked Then
                        intTotal += 1
                        iPhieuId = grdShow.DataKeys(item.RowIndex)("PhieuID").ToString
                        Try
                            DeletePhieu(iPhieuId)
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
            Dim obj = e.Row.DataItem()
            Dim hplPKN As HyperLink = CType(e.Row.FindControl("hplPKN"), HyperLink)
            If grdShow.DataKeys(e.Row.RowIndex)("IsHoanThanh").ToString().Contains("Hoàn thành") Then
                hplPKN.NavigateUrl = "../../Page/PhieuKiemTra/PhieuKienNghi.aspx?phieuId=" & grdShow.DataKeys(e.Row.RowIndex)("PhieuID")
            Else
                hplPKN.Enabled = False
            End If

            Dim lnkEdit As LinkButton = CType(e.Row.FindControl("lnkEdit"), LinkButton)
            Dim lnkTenphieu As LinkButton = CType(e.Row.FindControl("lnkTenphieu"), LinkButton)
            Dim lnkbtnDelete As LinkButton = CType(e.Row.FindControl("lnkbtnDelete"), LinkButton)
            Dim chkItem As CheckBox = CType(e.Row.FindControl("chkItem"), CheckBox)
            Dim policy As String = grdShow.DataKeys(e.Row.RowIndex)("VaiTro")

            'Xử lý phân quyền chọn từ hệ thống và điều kiện riêng từ store
            Dim ScriptManager As System.Web.UI.ScriptManager = System.Web.UI.ScriptManager.GetCurrent(Me.Page)
            ScriptManager.RegisterAsyncPostBackControl(lnkbtnDelete)

            'Permission
            Dim permissiondel As Boolean = HasPermission(Function_Name.PhieuKiemTra_XLP, Session("RoleID"), 0, Audit_Type.Delete)
            If (Not policy Is Nothing AndAlso policy = "1") Or
                (Not policy Is Nothing AndAlso Not policy = "1" And permissiondel) Then
                lnkbtnDelete.Enabled = True
                chkItem.Enabled = True
                lnkbtnDelete.Attributes.Add("onclick", "return ComfirmDialog('" + drpMessage.Items(0).Text + "', 0,'" + lnkbtnDelete.ClientID + "','" + e.Row.RowIndex.ToString + "',1);")
            ElseIf Not policy Is Nothing AndAlso Not policy = "1" And Not permissiondel Then
                lnkbtnDelete.Enabled = False
                chkItem.Enabled = False
            End If
            Dim permissionedit As Boolean = HasPermission(Function_Name.PhieuKiemTra_XLP, Session("RoleID"), 0, Audit_Type.Edit)
            If (Not policy Is Nothing AndAlso policy = "1") Or
                (Not policy Is Nothing AndAlso Not policy = "1" And permissionedit) Then
                lnkEdit.Enabled = True
                'lnkEdit.Attributes.Add("onclick", "setIndex(" + e.Row.RowIndex.ToString + ");")
            ElseIf Not policy Is Nothing AndAlso Not policy = "1" And Not permissionedit Then
                lnkEdit.Enabled = False
            End If

            'Permission
            lnkTenphieu.Enabled = HasPermission(Function_Name.PhieuKiemTra_XLP, Session("RoleID"), 0, Audit_Type.ViewContent)
            'Link tenphieu
            'lnkTenphieu.Attributes.Add("onclick", "setIndex(" + e.Row.RowIndex.ToString + ");")
            lnkTenphieu.Text = grdShow.DataKeys(e.Row.RowIndex)("TenDoanhNghiep").ToString()

            'Tạo phiếu mới kế thừa từ phiếu cũ
            Dim lnkTaoPhieu As LinkButton = CType(e.Row.FindControl("lnkTaoPhieu"), LinkButton)
            lnkTaoPhieu.Attributes.Add("onclick", "return ComfirmDialog('" + drpMessage.Items(3).Text + "', 0,'" + lnkTaoPhieu.ClientID + "','" + e.Row.RowIndex.ToString + "',1);")

            'Xử lý ẩn link khi user hiện tại đăng nhập không phải là user tạo phiếu kiểm tra

            If Not (grdShow.DataKeys(e.Row.RowIndex)("NguoiTao").ToString().Contains(Session("Username")) _
                         Or Session("IsUser").Equals(UserType.Admin)) Then
                lnkbtnDelete.Enabled = False
                chkItem.Enabled = False

                lnkEdit.Enabled = False

                lnkTaoPhieu.Enabled = False
                lnkTenphieu.Enabled = False
                lnkTenphieu.Attributes.Add("style", "color:gray !important")
            End If
        End If
    End Sub

    Protected Sub drpPage_Size_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles drpPage_Size.SelectedIndexChanged
        Dim arrSearch() As String
        arrSearch = ViewState("search")
        BindToGrid(1, arrSearch(1), arrSearch(2), arrSearch(3), arrSearch(4), arrSearch(5))
    End Sub

    Protected Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClear.Click
        BindToGrid(1, "")
        txtTitleFilter.Text = ""
        txtNguoiTao.Text = ""
        txtFromDate.Text = ""
        txtToDate.Text = ""
        ddlTrangThai.ClearSelection()
    End Sub
#End Region
#Region "Search"
    Protected Sub btnFilter_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFilter.Click
        Dim fDate As Date = IIf(txtFromDate.Text = "", #1/1/1900#, StringToDate(txtFromDate.Text))
        Dim TDate As Date = IIf(txtToDate.Text = "", #1/1/9999#, StringToDate(txtToDate.Text))
        BindToGrid(1, txtTitleFilter.Text.Trim(), txtNguoiTao.Text.Trim(), ddlTrangThai.SelectedValue, fDate, TDate)
    End Sub
#End Region
#Region "Tạo BBTT kế thừa từ PTKT hoặc kế thừa PTKT --> PTKT"
    Protected Sub lnkTaoPhieu_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Using data As New ThanhTraLaoDongEntities
            Dim iPhieuIdOld As Integer = CInt(grdShow.DataKeys(hidIndex.Value)("PhieuID").ToString)
            Response.Redirect("../../Page/PhieuKiemTra/KeThuaXLP.aspx?PhieuID=" & iPhieuIdOld)
        End Using
    End Sub
#End Region

    Protected Sub btnKeThua_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnKeThua.Click
        Dim StringId As String = ""
        Dim intTotal As Integer
        Using data As New ThanhTraLaoDongEntities
            For Each item As GridViewRow In grdShow.Rows
                Dim chkItem As New CheckBox
                chkItem = CType(item.FindControl("chkItem"), CheckBox)
                If chkItem.Checked Then
                    intTotal += 1
                    StringId += grdShow.DataKeys(item.RowIndex)("PhieuID").ToString & ","
                    'Write Code here
                End If
            Next
            Response.Redirect("../../Page/PhieuKiemTra/KeThuaXLP.aspx?PhieuID=" & StringId)
        End Using
    End Sub
End Class
