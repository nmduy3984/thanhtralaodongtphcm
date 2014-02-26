Imports System.Data
Imports ThanhTraLaoDongModel
Imports Cls_Common
Imports SecurityService
Partial Class Control_CauHoi_ListPKT
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
                Insert_App_Log("Delete  Phieunhapheader:" & strTenPhieu & "", Function_Name.PhieuKiemTra, Audit_Type.Delete, Request.ServerVariables("REMOTE_ADDR"), Session("UserName"))
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
        Using data As New thanhtralaodongEntities
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
            Dim permissiondel As Boolean = HasPermission(Function_Name.PhieuKiemTra, Session("RoleID"), 0, Audit_Type.Delete)
            If (Not policy Is Nothing AndAlso policy = "1") Or
                (Not policy Is Nothing AndAlso Not policy = "1" And permissiondel) Then
                lnkbtnDelete.Enabled = True
                chkItem.Enabled = True
                lnkbtnDelete.Attributes.Add("onclick", "return ComfirmDialog('" + drpMessage.Items(0).Text + "', 0,'" + lnkbtnDelete.ClientID + "','" + e.Row.RowIndex.ToString + "',1);")
            ElseIf Not policy Is Nothing AndAlso Not policy = "1" And Not permissiondel Then
                lnkbtnDelete.Enabled = False
                chkItem.Enabled = False
            End If
            Dim permissionedit As Boolean = HasPermission(Function_Name.PhieuKiemTra, Session("RoleID"), 0, Audit_Type.Edit)
            If (Not policy Is Nothing AndAlso policy = "1") Or
                (Not policy Is Nothing AndAlso Not policy = "1" And permissionedit) Then
                lnkEdit.Enabled = True
                'lnkEdit.Attributes.Add("onclick", "setIndex(" + e.Row.RowIndex.ToString + ");")
            ElseIf Not policy Is Nothing AndAlso Not policy = "1" And Not permissionedit Then
                lnkEdit.Enabled = False
            End If

            'Permission
            lnkTenphieu.Enabled = HasPermission(Function_Name.PhieuKiemTra, Session("RoleID"), 0, Audit_Type.ViewContent)
            'Link tenphieu
            'lnkTenphieu.Attributes.Add("onclick", "setIndex(" + e.Row.RowIndex.ToString + ");")
            lnkTenphieu.Text = grdShow.DataKeys(e.Row.RowIndex)("TenDoanhNghiep").ToString()

            'Tạo phiếu mới kế thừa từ phiếu cũ
            Dim lnkTaoPhieu As LinkButton = CType(e.Row.FindControl("lnkTaoPhieu"), LinkButton)
            'lnkTaoPhieu.Attributes.Add("onclick", "setIndex(" + e.Row.RowIndex.ToString + ");")
            lnkTaoPhieu.Attributes.Add("onclick", "return ComfirmDialog('" + drpMessage.Items(3).Text + "', 0,'" + lnkTaoPhieu.ClientID + "','" + e.Row.RowIndex.ToString + "',1);")
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
        Try
            Using data As New ThanhTraLaoDongEntities
                Dim iPhieuIdOld As Integer = CInt(grdShow.DataKeys(hidIndex.Value)("PhieuID").ToString)
                'B1: Tạo phiếu nhập header
                TaoPhieuHeader(iPhieuIdOld)
                'B2: Tạo các mục câu hỏi
                TaoCauHoi1(iPhieuIdOld)
                TaoCauHoi2(iPhieuIdOld)
                TaoCauHoi3(iPhieuIdOld)
                TaoCauHoi4(iPhieuIdOld)
                TaoCauHoi5(iPhieuIdOld)
                TaoCauHoi6(iPhieuIdOld)
                TaoCauHoi7(iPhieuIdOld)
                TaoCauHoi8(iPhieuIdOld)
                TaoCauHoi9(iPhieuIdOld)
                TaoCauHoi10(iPhieuIdOld)
                TaoCauHoi11(iPhieuIdOld)
                TaoCauHoi12(iPhieuIdOld)
                Excute_Javascript("AlertboxRedirect('Kế thừa phiếu thành công.','../BienBanThanhTra/ThongTinChung.aspx?DNId=" + hidDNId.Value + "');", Me.Page, True)
            End Using
        Catch ex As Exception
            log4net.Config.XmlConfigurator.Configure()
            log.Error("Error error " & AddTabSpace(1) & Session("Username") & AddTabSpace(1) & "IP:" & GetIPAddress(), ex)
        End Try
    End Sub
    Protected Sub TaoPhieuHeader(ByVal iPhieuIdOld As Integer)
        '' Tạo phiếu tại đây
        Using data As New ThanhTraLaoDongEntities
            '' Lấy thông tin Doanh nghiệp ra
            Dim p = (From q In data.PhieuNhapHeaders Where q.PhieuID = iPhieuIdOld).FirstOrDefault()
            If Not IsNothing(p) Then
                ''Luu phieu moi
                hidDNId.Value = p.DoanhNghiepId ' Lưu doanhnghiepid
                Dim pn As New ThanhTraLaoDongModel.PhieuNhapHeader
                pn.TenPhieu = "[BBKT]" + "-" + p.DoanhNghiep.TenDoanhNghiep + "-" + Date.Now.ToString("dd/MM/yyyy HH:mm:ss")
                pn.LoaiPhieu = True
                pn.DoanhNghiepId = p.DoanhNghiepId
                pn.CauHoiDaTraLoi = p.CauHoiDaTraLoi
                pn.IsNopPhat = False
                pn.IsThucHieKienNghi = False
                pn.DinhChiHDThietBi = ""
                pn.TienPhatDuKien = 0
                pn.SoKienNghiChuaThucHien = 0
                pn.SoKienNghiDaThucHien = 0
                pn.SoChiNhanh = p.SoChiNhanh
                pn.TongSoNhanVien = p.TongSoNhanVien
                pn.SoLaoDongNu = p.SoLaoDongNu
                pn.SoNguoiLamNgheNguyHiem = p.SoNguoiLamNgheNguyHiem
                pn.SoNguoiLamCongViecYeuCauNghiemNgat = p.SoNguoiLamCongViecYeuCauNghiemNgat
                pn.TongGiaTriSP = p.TongGiaTriSP
                pn.TongLoiNhuanSauThue = p.TongLoiNhuanSauThue
                pn.IsCongDoan = p.IsCongDoan
                pn.NguoiLienHe = p.NguoiLienHe
                pn.DienThoaiLH = p.DienThoaiLH
                pn.EmailLH = p.EmailLH
                pn.SoQuyenDinh = p.SoQuyenDinh
                pn.NgayKetThucPhieu = Nothing
                pn.NgayTao = Date.Now
                pn.NguoiTao = Session("Username")
                data.PhieuNhapHeaders.AddObject(pn)
                data.SaveChanges()
                Session("phieuid") = pn.PhieuID 'Luu phieuid moi
                hidPhieuIdNew.value = pn.PhieuID
                Session("ModePhieu") = ModePhieu.Edit
                Insert_App_Log("Insert  Phieunhapheader:" & pn.TenPhieu & "", Function_Name.PhieuKiemTra, Audit_Type.Create, Request.ServerVariables("REMOTE_ADDR"), Session("UserName"))

            End If
        End Using

    End Sub
    Protected Sub TaoCauHoi1(ByVal iPhieuIdOld As Integer)
        Try
            Using data As New ThanhTraLaoDongEntities
                Dim q = (From a In data.CauHoi1 Where a.PhieuId = iPhieuIdOld Select a).SingleOrDefault
                If Not IsNothing(q) Then
                    Dim p As New CauHoi1
                    p.PhieuId = hidPhieuIdNew.Value
                    p.Q11 = q.Q11
                    p.Q12 = q.Q12
                    p.Q13 = q.Q13
                    p.NguoiTao = Session("Username")
                    p.NgayTao = Date.Now
                    data.CauHoi1.AddObject(p)
                    data.SaveChanges()
                End If
            End Using
        Catch ex As Exception
            log4net.Config.XmlConfigurator.Configure()
            log.Error("Error error " & AddTabSpace(1) & Session("Username") & AddTabSpace(1) & "IP:" & GetIPAddress(), ex)
        End Try
    End Sub
    Protected Sub TaoCauHoi2(ByVal iPhieuIdOld As Integer)
        Try
            Using data As New ThanhTraLaoDongEntities
                Dim q = (From a In data.CauHoi2 Where a.PhieuId = iPhieuIdOld Select a).SingleOrDefault
                If Not IsNothing(q) Then
                    Dim p As New CauHoi2
                    p.PhieuId = hidPhieuIdNew.Value
                    p.Q21 = q.Q21
                    p.Q211 = q.Q211
                    p.Q212 = q.Q212
                    p.Q213 = q.Q213
                    p.Q214 = q.Q214
                    p.Q215 = q.Q215
                    p.Q216 = q.Q216
                    p.Q217 = q.Q217
                    p.Q218 = q.Q218
                    p.Q221 = q.Q221
                    p.Q222 = q.Q222
                    p.Q223 = q.Q223
                    p.Q224 = q.Q224
                    p.Q225 = q.Q225
                    p.Q231 = q.Q231
                    p.Q2321 = q.Q2321
                    p.Q2322 = q.Q2322
                    p.Q2323 = q.Q2323
                    p.Q233 = q.Q233
                    p.Q241 = q.Q241
                    p.Q242 = q.Q242
                    p.Q219 = q.Q219
                    p.Q226 = q.Q226
                    p.Q22111 = q.Q22111
                    p.Q2331 = q.Q2331
                    p.Q251 = q.Q251
                    p.Q2110 = q.Q2110
                    p.Q2210 = q.Q2210
                    p.Q2211 = q.Q2211
                    p.Q227 = q.Q227
                    p.Q228 = q.Q228
                    p.Q229 = q.Q229
                    p.Q234 = q.Q234
                    p.Q235 = q.Q235
                    p.Q236 = q.Q236
                    p.Q237 = q.Q237
                    p.Q238 = q.Q238

                    p.NguoiTao = Session("Username")
                    p.NgayTao = Date.Now
                    data.CauHoi2.AddObject(p)
                    data.SaveChanges()
                End If
            End Using
        Catch ex As Exception
            log4net.Config.XmlConfigurator.Configure()
            log.Error("Error error " & AddTabSpace(1) & Session("Username") & AddTabSpace(1) & "IP:" & GetIPAddress(), ex)
        End Try
    End Sub
    Protected Sub TaoCauHoi3(ByVal iPhieuIdOld As Integer)
        Try
            Using data As New ThanhTraLaoDongEntities
                Dim q = (From a In data.CauHoi3 Where a.PhieuId = iPhieuIdOld Select a).SingleOrDefault
                If Not IsNothing(q) Then
                    Dim p As New CauHoi3
                    p.PhieuId = hidPhieuIdNew.Value
                    p.Q31 = q.Q31
                    p.Q32 = q.Q32
                    p.Q33 = q.Q33
                    p.Q34 = q.Q34
                    p.Q341 = q.Q341
                    p.Q35 = q.Q35
                    p.Q351 = q.Q351
                    p.NguoiTao = Session("Username")
                    p.NgayTao = Date.Now
                    data.CauHoi3.AddObject(p)
                    data.SaveChanges()
                End If
            End Using
        Catch ex As Exception
            log4net.Config.XmlConfigurator.Configure()
            log.Error("Error error " & AddTabSpace(1) & Session("Username") & AddTabSpace(1) & "IP:" & GetIPAddress(), ex)
        End Try
    End Sub
    Protected Sub TaoCauHoi4(ByVal iPhieuIdOld As Integer)
        Try
            Using data As New ThanhTraLaoDongEntities
                Dim q = (From a In data.CauHoi4 Where a.PhieuId = iPhieuIdOld Select a).SingleOrDefault
                If Not IsNothing(q) Then
                    Dim p As New CauHoi4
                    p.PhieuId = hidPhieuIdNew.Value
                    p.Q4111 = q.Q4111
                    p.Q4112 = q.Q4112
                    p.Q4113 = q.Q4113
                    p.Q4104 = q.Q4104
                    p.Q41131 = q.Q41131
                    p.Q421 = q.Q421
                    p.Q422 = q.Q422
                    p.Q423 = q.Q423
                    p.Q43 = q.Q43
                    p.Q44 = q.Q44
                    p.Q45 = q.Q45
                    p.Q461 = q.Q461
                    p.Q462 = q.Q462
                    p.Q463 = q.Q463
                    p.Q464 = q.Q464
                    p.Q47 = q.Q47
                    p.Q48 = q.Q48
                    p.Q481 = q.Q481
                    p.Q482 = q.Q482
                    p.Q49 = q.Q49
                    p.Q410 = q.Q410
                    p.Q4101 = q.Q4101
                    p.Q4102 = q.Q4102
                    p.Q41021 = q.Q41021
                    p.Q41022 = q.Q41022
                    p.Q4103 = q.Q4103
                    p.Q410311 = q.Q410311
                    p.Q410312 = q.Q410312
                    p.Q41032 = q.Q41032
                    p.Q41033 = q.Q41033
                    p.Q46a = q.Q46a
                    p.Q46b = q.Q46b
                    p.Q4114 = q.Q4114
                    p.Q41011 = q.Q41011
                    p.Q424 = q.Q424
                    p.Q4105 = q.Q4105

                    p.NguoiTao = Session("Username")
                    p.NgayTao = Date.Now
                    data.CauHoi4.AddObject(p)
                    data.SaveChanges()
                End If
            End Using
        Catch ex As Exception
            log4net.Config.XmlConfigurator.Configure()
            log.Error("Error error " & AddTabSpace(1) & Session("Username") & AddTabSpace(1) & "IP:" & GetIPAddress(), ex)
        End Try
    End Sub
    Protected Sub TaoCauHoi5(ByVal iPhieuIdOld As Integer)
        Try
            Using data As New ThanhTraLaoDongEntities
                Dim q = (From a In data.CauHoi5 Where a.PhieuId = iPhieuIdOld Select a).SingleOrDefault
                If Not IsNothing(q) Then
                    Dim p As New CauHoi5
                    p.PhieuId = hidPhieuIdNew.Value
                    p.Q511 = q.Q511
                    p.Q512 = q.Q512
                    p.Q52 = q.Q52
                    p.Q521 = q.Q521
                    p.Q522 = q.Q522
                    p.Q53 = q.Q53
                    p.Q531 = q.Q531
                    p.Q532 = q.Q532
                    p.Q533 = q.Q533
                    p.Q54 = q.Q54
                    p.Q513 = q.Q513
                    p.Q541 = q.Q541
                    p.Q55 = q.Q55

                    p.NguoiTao = Session("Username")
                    p.NgayTao = Date.Now
                    data.CauHoi5.AddObject(p)
                    data.SaveChanges()
                End If
            End Using
        Catch ex As Exception
            log4net.Config.XmlConfigurator.Configure()
            log.Error("Error error " & AddTabSpace(1) & Session("Username") & AddTabSpace(1) & "IP:" & GetIPAddress(), ex)
        End Try
    End Sub
    Protected Sub TaoCauHoi6(ByVal iPhieuIdOld As Integer)
        Try
            Using data As New ThanhTraLaoDongEntities
                Dim q = (From a In data.CauHoi6 Where a.PhieuId = iPhieuIdOld Select a).SingleOrDefault
                If Not IsNothing(q) Then
                    Dim p As New CauHoi6
                    p.PhieuId = hidPhieuIdNew.Value
                    p.Q611 = q.Q611
                    p.Q612 = q.Q612
                    p.Q621 = q.Q621
                    p.Q622 = q.Q622
                    p.Q63 = q.Q63
                    p.Q641 = q.Q641
                    p.Q642 = q.Q642
                    p.Q65 = q.Q65
                    p.Q651 = q.Q651
                    p.Q66 = q.Q66
                    p.Q67 = q.Q67
                    p.Q671 = q.Q671
                    p.Q68 = q.Q68
                    p.Q681 = q.Q681
                    p.Q631 = q.Q631

                    p.NguoiTao = Session("Username")
                    p.NgayTao = Date.Now
                    data.CauHoi6.AddObject(p)
                    data.SaveChanges()
                End If
            End Using
        Catch ex As Exception
            log4net.Config.XmlConfigurator.Configure()
            log.Error("Error error " & AddTabSpace(1) & Session("Username") & AddTabSpace(1) & "IP:" & GetIPAddress(), ex)
        End Try
    End Sub
    Protected Sub TaoCauHoi7(ByVal iPhieuIdOld As Integer)
        Try
            Using data As New ThanhTraLaoDongEntities
                Dim q = (From a In data.CauHoi7 Where a.PhieuId = iPhieuIdOld Select a).SingleOrDefault
                If Not IsNothing(q) Then
                    Dim p As New CauHoi7
                    p.PhieuId = hidPhieuIdNew.Value
                    p.Q701 = q.Q701
                    p.Q7011 = q.Q7011
                    p.Q7012 = q.Q7012
                    p.Q7013 = q.Q7013
                    p.Q7014 = q.Q7014
                    p.Q702 = q.Q702
                    p.Q703 = q.Q703
                    p.Q7031 = q.Q7031
                    p.Q704 = q.Q704
                    p.Q7041 = q.Q7041
                    p.Q7051 = q.Q7051
                    p.Q7052 = q.Q7052
                    p.Q7053 = q.Q7053
                    p.Q70611 = q.Q70611
                    p.Q70612 = q.Q70612
                    p.Q706211 = q.Q706211
                    p.Q706212 = q.Q706212
                    p.Q706221 = q.Q706221
                    p.Q706222 = q.Q706222
                    p.Q706232 = q.Q706232
                    p.Q707 = q.Q707
                    p.Q7071 = q.Q7071
                    p.Q7072 = q.Q7072
                    p.Q7073 = q.Q7073
                    p.Q708 = q.Q708
                    p.Q7081 = q.Q7081
                    p.Q7082 = q.Q7082
                    p.Q7083 = q.Q7083
                    p.Q709 = q.Q709
                    p.Q7091 = q.Q7091
                    p.Q70911 = q.Q70911
                    p.Q71012 = q.Q71012
                    p.Q71011 = q.Q71011
                    p.Q710111 = q.Q710111
                    p.Q711 = q.Q711
                    p.Q7111 = q.Q7111
                    p.Q7112 = q.Q7112
                    p.Q7113 = q.Q7113
                    p.Q7114 = q.Q7114
                    p.Q7115 = q.Q7115
                    p.Q7116 = q.Q7116
                    p.Q7117 = q.Q7117
                    p.Q7118 = q.Q7118
                    p.Q7121 = q.Q7121
                    p.Q7122 = q.Q7122
                    p.Q71221 = q.Q71221
                    p.Q71222 = q.Q71222
                    p.Q713 = q.Q713
                    p.Q7141 = q.Q7141
                    p.Q71421 = q.Q71421
                    p.Q71422 = q.Q71422
                    p.Q7143 = q.Q7143
                    p.Q7144 = q.Q7144
                    p.Q7151 = q.Q7151
                    p.Q71511 = q.Q71511
                    p.Q7152 = q.Q7152
                    p.Q71521 = q.Q71521
                    p.Q715211 = q.Q715211
                    p.Q715212 = q.Q715212
                    p.Q715213 = q.Q715213
                    p.Q716 = q.Q716
                    p.Q7161 = q.Q7161
                    p.Q717 = q.Q717
                    p.Q7171 = q.Q7171
                    p.Q7172 = q.Q7172
                    p.Q7173 = q.Q7173
                    p.Q7174 = q.Q7174
                    p.Q7175 = q.Q7175
                    p.Q7176 = q.Q7176
                    p.Q7181 = q.Q7181
                    p.Q7182 = q.Q7182
                    p.Q7183 = q.Q7183
                    p.Q7184 = q.Q7184
                    p.Q710112 = q.Q710112
                    p.Q710113 = q.Q710113
                    p.Q710114 = q.Q710114
                    p.Q710115 = q.Q710115
                    p.Q718 = q.Q718
                    p.Q7185 = q.Q7185
                    p.Q7015 = q.Q7015
                    p.Q7016 = q.Q7016
                    p.Q71110 = q.Q71110
                    p.Q71111 = q.Q71111
                    p.Q71112 = q.Q71112
                    p.Q7119 = q.Q7119
                    p.Q7131 = q.Q7131
                    p.Q7132 = q.Q7132
                    p.Q7177 = q.Q7177
                    p.Q719 = q.Q719
                    p.Q7178 = q.Q7178
                    p.Q7021 = q.Q7021

                    p.NguoiTao = Session("Username")
                    p.NgayTao = Date.Now
                    data.CauHoi7.AddObject(p)
                    data.SaveChanges()
                End If
            End Using
        Catch ex As Exception
            log4net.Config.XmlConfigurator.Configure()
            log.Error("Error error " & AddTabSpace(1) & Session("Username") & AddTabSpace(1) & "IP:" & GetIPAddress(), ex)
        End Try
    End Sub
    Protected Sub TaoCauHoi8(ByVal iPhieuIdOld As Integer)
        Try
            Using data As New ThanhTraLaoDongEntities
                Dim q = (From a In data.CauHoi8 Where a.PhieuId = iPhieuIdOld Select a).SingleOrDefault
                If Not IsNothing(q) Then
                    Dim p As New CauHoi8
                    p.PhieuId = hidPhieuIdNew.Value
                    p.Q8111 = q.Q8111
                    p.Q8112 = q.Q8112
                    p.Q812 = q.Q812
                    p.Q82 = q.Q82
                    p.Q821 = q.Q821
                    p.Q822 = q.Q822
                    p.Q823 = q.Q823
                    p.Q824 = q.Q824
                    p.Q83 = q.Q83
                    p.Q84 = q.Q84
                    p.Q851 = q.Q851
                    p.Q8511 = q.Q8511
                    p.Q8512 = q.Q8512
                    'p.Q8513 = q.Q8513
                    p.Q8514 = q.Q8514
                    p.Q8515 = q.Q8515
                    p.Q8516 = q.Q8516
                    p.Q81 = q.Q81
                    p.Q8241 = q.Q8241

                    p.NguoiTao = Session("Username")
                    p.NgayTao = Date.Now
                    data.CauHoi8.AddObject(p)
                    data.SaveChanges()
                End If
            End Using
        Catch ex As Exception
            log4net.Config.XmlConfigurator.Configure()
            log.Error("Error error " & AddTabSpace(1) & Session("Username") & AddTabSpace(1) & "IP:" & GetIPAddress(), ex)
        End Try
    End Sub
    Protected Sub TaoCauHoi9(ByVal iPhieuIdOld As Integer)
        Try
            Using data As New ThanhTraLaoDongEntities
                Dim q = (From a In data.CauHoi9 Where a.PhieuId = iPhieuIdOld Select a).SingleOrDefault
                If Not IsNothing(q) Then
                    Dim p As New CauHoi9
                    p.PhieuId = hidPhieuIdNew.Value
                    p.Q9111 = q.Q9111
                    p.Q9112 = q.Q9112
                    p.Q92 = q.Q92
                    p.Q921 = q.Q921
                    p.Q923 = q.Q923
                    p.Q924 = q.Q924
                    p.Q922 = q.Q922
                    p.Q93 = q.Q93
                    p.Q931 = q.Q931
                    p.Q932 = q.Q932
                    p.Q933 = q.Q933
                    p.Q925 = q.Q925

                    p.NguoiTao = Session("Username")
                    p.NgayTao = Date.Now
                    data.CauHoi9.AddObject(p)
                    data.SaveChanges()
                End If
            End Using
        Catch ex As Exception
            log4net.Config.XmlConfigurator.Configure()
            log.Error("Error error " & AddTabSpace(1) & Session("Username") & AddTabSpace(1) & "IP:" & GetIPAddress(), ex)
        End Try
    End Sub
    Protected Sub TaoCauHoi10(ByVal iPhieuIdOld As Integer)
        Try
            Using data As New ThanhTraLaoDongEntities
                Dim q = (From a In data.CauHoi10 Where a.PhieuId = iPhieuIdOld Select a).SingleOrDefault
                If Not IsNothing(q) Then
                    Dim p As New CauHoi10
                    p.PhieuId = hidPhieuIdNew.Value
                    p.Q101 = q.Q101
                    p.Q102 = q.Q102
                    p.Q103 = q.Q103
                    p.Q104 = q.Q104
                    p.Q105 = q.Q105

                    p.NguoiTao = Session("Username")
                    p.NgayTao = Date.Now
                    data.CauHoi10.AddObject(p)
                    data.SaveChanges()
                End If
            End Using
        Catch ex As Exception
            log4net.Config.XmlConfigurator.Configure()
            log.Error("Error error " & AddTabSpace(1) & Session("Username") & AddTabSpace(1) & "IP:" & GetIPAddress(), ex)
        End Try
    End Sub
    Protected Sub TaoCauHoi11(ByVal iPhieuIdOld As Integer)
        Try
            Using data As New ThanhTraLaoDongEntities
                Dim q = (From a In data.CauHoi11 Where a.PhieuId = iPhieuIdOld Select a).SingleOrDefault
                If Not IsNothing(q) Then
                    Dim p As New CauHoi11
                    p.PhieuId = hidPhieuIdNew.Value
                    p.Q1111 = q.Q1111
                    p.Q1112 = q.Q1112
                    p.Q112 = q.Q112
                    p.Q113 = q.Q113
                    p.Q114 = q.Q114
                    p.Q115 = q.Q115
                    p.Q116 = q.Q116
                    p.Q117 = q.Q117
                    p.Q11 = q.Q11
                    p.Q1121 = q.Q1121
                    p.Q1131 = q.Q1131
                    p.Q118 = q.Q118

                    p.NguoiTao = Session("Username")
                    p.NgayTao = Date.Now
                    data.CauHoi11.AddObject(p)
                    data.SaveChanges()
                End If
            End Using
        Catch ex As Exception
            log4net.Config.XmlConfigurator.Configure()
            log.Error("Error error " & AddTabSpace(1) & Session("Username") & AddTabSpace(1) & "IP:" & GetIPAddress(), ex)
        End Try
    End Sub
    Protected Sub TaoCauHoi12(ByVal iPhieuIdOld As Integer)
        Try
            Using data As New ThanhTraLaoDongEntities
                Dim q = (From a In data.CauHoi12 Where a.PhieuId = iPhieuIdOld Select a).SingleOrDefault
                If Not IsNothing(q) Then
                    Dim p As New CauHoi12
                    p.PhieuId = hidPhieuIdNew.Value
                    p.Q121 = q.Q121
                    p.Q1211 = q.Q1211
                    p.Q1212 = q.Q1212
                    p.Q1213 = q.Q1213
                    p.Q122 = q.Q122
                    p.Q123 = q.Q123
                    p.Q1241 = q.Q1241
                    p.Q1242 = q.Q1242

                    p.NguoiTao = Session("Username")
                    p.NgayTao = Date.Now
                    data.CauHoi12.AddObject(p)
                    data.SaveChanges()
                End If
            End Using
        Catch ex As Exception
            log4net.Config.XmlConfigurator.Configure()
            log.Error("Error error " & AddTabSpace(1) & Session("Username") & AddTabSpace(1) & "IP:" & GetIPAddress(), ex)
        End Try
    End Sub
#End Region
End Class
