
Imports System.Data
Imports ThanhTraLaoDongModel
Imports Cls_Common
Imports SecurityService
Partial Class DoanhNghiep_Control_ListPheuTuKiemTra
    Inherits System.Web.UI.UserControl
    Private Shared ReadOnly log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
#Region "Sub and Function"
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
#End Region
#Region "Event for control"
    Protected Function DeletePhieu(ByVal iPhieuId As Integer) As Boolean

        Dim strLogName As String = ""
        Dim strTenPhieu = ""
        Using data As New ThanhTraLaoDongEntities
            Dim q = (From p In data.PhieuNhapHeaders Where p.PhieuID = iPhieuId Select p).FirstOrDefault
            strTenPhieu = q.TenPhieu
            Dim cauhoi1 = (From q1 In data.CauHoi1 Where q1.PhieuId = iPhieuId Select q1).FirstOrDefault
            Dim cauhoi2 = (From q2 In data.CauHoi2 Where q2.PhieuId = iPhieuId Select q2).FirstOrDefault
            Dim cauhoi3 = (From q3 In data.CauHoi3 Where q3.PhieuId = iPhieuId Select q3).FirstOrDefault
            Dim cauhoi4 = (From q4 In data.CauHoi4 Where q4.PhieuId = iPhieuId Select q4).FirstOrDefault
            Dim cauhoi5 = (From q5 In data.CauHoi5 Where q5.PhieuId = iPhieuId Select q5).FirstOrDefault
            Dim cauhoi6 = (From q6 In data.CauHoi6 Where q6.PhieuId = iPhieuId Select q6).FirstOrDefault
            Dim cauhoi7 = (From q7 In data.CauHoi7 Where q7.PhieuId = iPhieuId Select q7).FirstOrDefault
            Dim cauhoi8 = (From q8 In data.CauHoi8 Where q8.PhieuId = iPhieuId Select q8).FirstOrDefault
            Dim cauhoi9 = (From q9 In data.CauHoi9 Where q9.PhieuId = iPhieuId Select q9).FirstOrDefault
            Dim cauhoi10 = (From q10 In data.CauHoi10 Where q10.PhieuId = iPhieuId Select q10).FirstOrDefault
            Dim cauhoi11 = (From q11 In data.CauHoi11 Where q11.PhieuId = iPhieuId Select q11).FirstOrDefault
            Dim cauhoi12 = (From q12 In data.CauHoi12 Where q12.PhieuId = iPhieuId Select q12).FirstOrDefault
            Dim ketluan = (From kl In data.KetLuans Where kl.PhieuId = iPhieuId Select kl)
            Try
                If Not q Is Nothing Then
                    If Not cauhoi1 Is Nothing Then
                        data.CauHoi1.DeleteObject(cauhoi1)
                    End If
                    If Not cauhoi2 Is Nothing Then
                        data.CauHoi2.DeleteObject(cauhoi2)
                    End If
                    If Not cauhoi3 Is Nothing Then
                        data.CauHoi3.DeleteObject(cauhoi3)
                    End If
                    If Not cauhoi4 Is Nothing Then
                        data.CauHoi4.DeleteObject(cauhoi4)
                    End If
                    If Not cauhoi5 Is Nothing Then
                        data.CauHoi5.DeleteObject(cauhoi5)
                    End If
                    If Not cauhoi6 Is Nothing Then
                        data.CauHoi6.DeleteObject(cauhoi6)
                    End If
                    If Not cauhoi7 Is Nothing Then
                        data.CauHoi7.DeleteObject(cauhoi7)
                    End If
                    If Not cauhoi8 Is Nothing Then
                        data.CauHoi8.DeleteObject(cauhoi8)
                    End If
                    If Not cauhoi9 Is Nothing Then
                        data.CauHoi9.DeleteObject(cauhoi9)
                    End If
                    If Not cauhoi10 Is Nothing Then
                        data.CauHoi10.DeleteObject(cauhoi10)
                    End If
                    If Not cauhoi11 Is Nothing Then
                        data.CauHoi11.DeleteObject(cauhoi11)
                    End If
                    If Not cauhoi12 Is Nothing Then
                        data.CauHoi12.DeleteObject(cauhoi12)
                    End If
                    For Each item As KetLuan In ketluan
                        data.KetLuans.DeleteObject(item)
                    Next
                    data.PhieuNhapHeaders.DeleteObject(q)
                End If
                data.SaveChanges()
                Insert_App_Log("Delete  Phieunhapheader:" & strTenPhieu & "", Function_Name.BienBanThanhTra, Audit_Type.Delete, Request.ServerVariables("REMOTE_ADDR"), Session("UserName"))
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
            Dim lblSTT As Label = CType(e.Row.FindControl("lblSTT"), Label)
            lblSTT.Text = CInt(drpPage_Size.SelectedValue) * (CInt(hidCur_Page.Value) - 1).ToString + e.Row.RowIndex + 1
            'Tên doanh nghiệp
            Dim lblDoanhNghiep As Label = CType(e.Row.FindControl("lblDoanhNghiep"), Label)
            lblDoanhNghiep.Text = grdShow.DataKeys(e.Row.RowIndex)("TenPhieu").ToString().Split("-")(1)
            'Ngay Tao
            Dim lblNgayTao As Label = CType(e.Row.FindControl("lblNgayTao"), Label)
            lblNgayTao.Text = CType(grdShow.DataKeys(e.Row.RowIndex)("NgayTao"), DateTime).ToString("dd/MM/yyyy HH:mm:ss")
            'Cau hoi da tra loi
            Dim lblCauHoiDaTraLoi As Label = CType(e.Row.FindControl("lblCauHoiDaTraLoi"), Label)
            If grdShow.DataKeys(e.Row.RowIndex)("CauHoiDaTraLoi").ToString().Split(";").Length >= 12 Then
                lblCauHoiDaTraLoi.Text = "Hoàn thành"
            Else
                lblCauHoiDaTraLoi.Text = "Chưa hoàn thành"
            End If

            Dim strNguoiTao As String = grdShow.DataKeys(e.Row.RowIndex)("NguoiTao").ToString()
            Dim iVaiTro As Integer = grdShow.DataKeys(e.Row.RowIndex)("VaiTro")

            'Link sua
            Dim lnkEdit As LinkButton = CType(e.Row.FindControl("lnkEdit"), LinkButton)
            lnkEdit.Attributes.Add("onclick", "setIndex(" + e.Row.RowIndex.ToString + ");")
            'Link tenphieu
            Dim lnkTenphieu As LinkButton = CType(e.Row.FindControl("lnkTenphieu"), LinkButton)
            lnkTenphieu.Attributes.Add("onclick", "setIndex(" + e.Row.RowIndex.ToString + ");")
            lnkTenphieu.Text = grdShow.DataKeys(e.Row.RowIndex)("TenPhieu").ToString()
            'Link xoa
            Dim lnkbtnDelete As LinkButton = CType(e.Row.FindControl("lnkbtnDelete"), LinkButton)
            lnkbtnDelete.Attributes.Add("onclick", "return ComfirmDialog('" + drpMessage.Items(0).Text + "', 0,'" + lnkbtnDelete.ClientID + "','" + e.Row.RowIndex.ToString + "',1);")


        End If
    End Sub
#End Region
#Region "Search"
    Protected Sub btnFilter_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFilter.Click
        BindToGrid(1, txtTitleFilter.Text.Trim())
    End Sub
#End Region


    Protected Sub btnCreate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCreate.Click
        TaoPhieuHeader(Session("Username"))
    End Sub
    Protected Sub TaoPhieuHeader(ByVal _Creator As String)
        '' Tạo phiếu tại đây
        Using data As New ThanhTraLaoDongEntities
            '' Lấy thông tin Doanh nghiệp ra
            Dim p = (From q In data.DoanhNghieps Where q.NguoiTao = _Creator).FirstOrDefault()
            If Not IsNothing(p) Then

                ''Luu phieu moi
                Dim pn As New ThanhTraLaoDongModel.PhieuNhapHeader

                pn.TenPhieu = "[PTKT]" + "-" + p.TenDoanhNghiep + "-" + Date.Now.ToString("dd/MM/yyyy HH:mm:ss")
                pn.LoaiPhieu = False

                pn.DoanhNghiepId = p.DoanhNghiepId
                pn.CauHoiDaTraLoi = ""
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
                pn.TongGiaTriSP = p.TongGiaTriSP
                pn.IsCongDoan = p.IsCongDoan

                pn.NgayTao = Now.Date
                pn.NguoiTao = Session("Username")
                data.PhieuNhapHeaders.AddObject(pn)
                data.SaveChanges()
                Session("phieuid") = pn.PhieuID 'Luu phieuid moi
                Session("ModePhieu") = ModePhieu.Create
                Insert_App_Log("Insert  Phieunhapheader:" & pn.TenPhieu & "", Function_Name.BienBanThanhTra, Audit_Type.Create, Request.ServerVariables("REMOTE_ADDR"), Session("UserName"))
                Response.Redirect("PhieuTuKiemTra/ThongTinChung.aspx?DNId=" & p.DoanhNghiepId)
            Else
                Excute_Javascript("AlertboxRedirect('Bạn vui lòng nhập thông tin doanh nghiệp.','ThongTin.aspx');", Me.Page, True)
            End If
        End Using



    End Sub

End Class
