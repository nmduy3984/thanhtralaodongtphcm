Imports System.Data
Imports ThanhTraLaoDongModel
Imports Cls_Common
Imports SecurityService

Partial Class Control_DoanhNghiep_ContentEditor
    Inherits System.Web.UI.UserControl
    Private Shared ReadOnly log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
#Region "Sub and Function "
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Using data As New ThanhTraLaoDongEntities
                'Session("ModePhieu") = Nothing
                If Not Request.QueryString("DNId") Is Nothing Then
                    hidID.Value = Request.QueryString("DNId")
                    btnDelete.Attributes.Add("onclick", "return ComfirmDialog('Bạn có chắc chắn muốn xóa không?',0,'" + btnDelete.ClientID + "','" + hidID.Value.ToString + "',1);")
                End If
                If Not IsNothing(Session("DoanhNghiepIdCreate")) Then
                    hidID.Value = CInt(Session("DoanhNghiepIdCreate"))
                End If
                'active tab tuong ung voi Page
                'InVisble button ko can thiet
                If Request.PhysicalPath.Contains("List.aspx") Then
                    lbtList.CssClass = "current_manage"
                    lbtList.Text = "<span class=""current_manage"">Quản lý</span>"
                    lbtEdit.Visible = False
                    lbtView.Visible = False
                    btnDelete.Visible = False
                     lbtPTKT.Visible = False
                    Session("DoanhNghiepIdCreate") = Nothing
                 ElseIf Request.PhysicalPath.Contains("Create.aspx") Then
                    lbtAdd.CssClass = "current_manage"
                    lbtAdd.Text = "<span class=""current_manage"">Thêm mới</span>"
                    lbtEdit.Visible = False
                    lbtView.Visible = False
                    btnDelete.Visible = False
                    lbtPTKT.Visible = False
                    If Not IsNothing(Session("DoanhNghiepIdCreate")) Then
                        lbtPTKT.Visible = True
                    End If
                ElseIf Request.PhysicalPath.Contains("Detail.aspx") Then
                    lbtView.CssClass = "current_manage"
                    lbtView.Text = "<span class=""current_manage"">Xem</span>"
                    setPermission()
                    'B2: Xử lý menu Thanh tra/Xử lý phiếu
                    'Xem doanh nghiệp đã được điền thông tin đủ chưa(IsHoanThanh) để enable nút tạo mới BBTT/tạo mới PTKT cho hợp lí
                    Dim dn As DoanhNghiep = (From a In data.DoanhNghieps
                                             Where a.DoanhNghiepId = hidID.Value Select a).SingleOrDefault
                    lbtAdd.Visible = True
                    lbtEdit.Visible = True
                    lbtView.Visible = True
                    btnDelete.Visible = True
                    lbtPTKT.Visible = True
                    lbtPTKT.Enabled = True
                    If Not IsNothing(dn) AndAlso (Not dn.IsHoanThanh OrElse IsNothing(dn.IsHoanThanh)) Then
                        lbtPTKT.Enabled = False
                    End If

                ElseIf Request.PhysicalPath.Contains("Edit.aspx") Then
                    lbtEdit.CssClass = "current_manage"
                    lbtEdit.Text = "<span class=""current_manage"">Sửa</span>"
                    setPermission()
                    'B2: Xử lý menu Thanh tra/Xử lý phiếu
                    'Xem doanh nghiệp đã được điền thông tin đủ chưa(IsHoanThanh) để enable nút tạo mới BBTT/tạo mới PTKT cho hợp lí
                    Dim dn As DoanhNghiep = (From a In data.DoanhNghieps
                                             Where a.DoanhNghiepId = hidID.Value Select a).SingleOrDefault
                    lbtAdd.Visible = True
                    lbtEdit.Visible = True
                    lbtView.Visible = True
                    btnDelete.Visible = True
                    lbtPTKT.Visible = True
                    lbtPTKT.Enabled = True
                    If Not IsNothing(dn) AndAlso (Not dn.IsHoanThanh OrElse IsNothing(dn.IsHoanThanh)) Then
                        lbtPTKT.Enabled = False
                    End If

                End If
                'Không hiện 2 tab tạo BB và Phiếu nếu sửa doanh nghiệp được click link từ thông tin chung
                If Not IsNothing(Session("EditInfoPhieu")) Then
                    lbtPTKT.Visible = False
                End If
            End Using
        End If
    End Sub
    Protected Sub setPermission()
        lbtAdd.Visible = HasPermission(Function_Name.DoanhNghiepXLP, Session("RoleID"), 0, Audit_Type.Create)
        lbtView.Visible = HasPermission(Function_Name.DoanhNghiepXLP, Session("RoleID"), 0, Audit_Type.ViewContent)
        lbtEdit.Visible = HasPermission(Function_Name.DoanhNghiepXLP, Session("RoleID"), 0, Audit_Type.Edit)
        btnDelete.Visible = HasPermission(Function_Name.DoanhNghiepXLP, Session("RoleID"), 0, Audit_Type.Delete)
    End Sub
    Protected Sub lbtList_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbtList.Click
        Response.Redirect("List.aspx")
    End Sub
    Protected Sub lbtAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbtAdd.Click
        Response.Redirect("Create.aspx")
    End Sub
    Protected Sub lbtEdit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbtEdit.Click
        Response.Redirect("Edit.aspx?DNId=" & hidID.Value & "")
    End Sub
    Protected Sub lbtView_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbtView.Click
        Response.Redirect("Detail.aspx?DNId=" & hidID.Value & "")
    End Sub

     Protected Sub lbtPTKT_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbtPTKT.Click
        TaoPhieuHeader(2)
        Session("DoanhNghiepIdCreate") = Nothing
        Response.Redirect("../PhieuKiemTra/ThongTinChung.aspx?DNId=" & hidID.Value)
    End Sub

    Protected Sub lnkbtnDelete_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnDelete.Click
        Using data As New ThanhTraLaoDongEntities
            Dim isUser As String
            Dim UserName As String
            isUser = Session("IsUser")
            UserName = Session("UserName")
            Dim dn = (From q In data.DoanhNghieps
                      Where q.DoanhNghiepId = hidID.Value
                      Select q).FirstOrDefault()
            If Not IsNothing(dn) Then
                If isUser = UserType.Admin Or UserName = dn.NguoiTao Then '' Có quyền Xóa
                    Dim intId As Integer
                    Dim strLogName As String = ""
                    intId = hidID.Value
                    Try
                        Dim pn = (From a In data.PhieuNhapHeaders Where a.DoanhNghiepId = intId).ToList()
                        If pn.Count = 0 Then
                            'Dim qddn = (From a In data.QuyetDinhTTDoanhNghieps Where a.DoanhNghiepId = intId).ToList
                            'If qddn.Count > 0 Then
                            '    For i As Integer = 0 To qddn.Count - 1
                            '        Dim SQD As String = qddn(i).QuyetDinhTT
                            '        Dim qdtt = (From a In data.QuyetDinhThanhTras
                            '           Where a.SoQuyetDinh.Equals(SQD) Select a).FirstOrDefault
                            '        If qdtt Is Nothing Then
                            '            qdtt.IsEdited = False
                            '            data.QuyetDinhThanhTras.AddObject(qdtt)
                            '            data.SaveChanges()
                            '        End If
                            '        data.QuyetDinhTTDoanhNghieps.DeleteObject(qddn(i))
                            '        data.SaveChanges()
                            '    Next
                            'End If
                            data.DoanhNghieps.DeleteObject(dn)
                            data.SaveChanges()
                            Insert_App_Log("Delete Doanh Nghiep:" & intId & "", Function_Name.DoanhNghiepXLP, Audit_Type.Delete, Request.ServerVariables("REMOTE_ADDR"), Session("UserName"))
                            Excute_Javascript("Alertbox('Xóa dữ liệu thành công.');window.location ='../../Page/DoanhNghiepXLP/List.aspx';", Me.Page, True)
                        Else
                            Excute_Javascript("Alertbox('Xóa doanh nghiệp thất bại. Doanh nghiệp đang được tham chiếu từ biên bản thanh tra hoặc phiếu kiểm tra.');", Me.Page, True)
                        End If
                    Catch ex As Exception
                        log4net.Config.XmlConfigurator.Configure()
                        log.Error("Error error " & AddTabSpace(1) & Session("Username") & AddTabSpace(1) & "IP:" & GetIPAddress(), ex)
                        Excute_Javascript("Alertbox('Xóa doanh nghiệp thất bại (" & ex.Message & ").');", Me.Page, True)
                    End Try
                Else
                    Excute_Javascript("AlertboxRedirect('Bạn không có quyền xóa với doanh nghiệp này.','Detail.aspx?DNId=" + hidID.Value + "');", Me.Page, True)
                End If
            Else
                Excute_Javascript("AlertboxRedirect('Không tồn tại doanh nghiệp này.','List.aspx.aspx');", Me.Page, True)
            End If
        End Using
    End Sub

    Protected Sub TaoPhieuHeader(ByVal _Type As Integer)
        '' Tạo phiếu tại đây
        Using data As New ThanhTraLaoDongEntities
            '' Lấy thông tin Doanh nghiệp ra
            Dim p = (From q In data.DoanhNghieps Where q.DoanhNghiepId = hidID.Value).FirstOrDefault()
            If Not IsNothing(p) Then

                ''Luu phieu moi
                Dim pn As New ThanhTraLaoDongModel.PhieuNhapHeader
                If _Type = 1 Then
                    pn.TenPhieu = "[BBKT]" + "-" + p.TenDoanhNghiep + "-" + Date.Now.ToString("dd/MM/yyyy HH:mm:ss")
                    pn.LoaiPhieu = True
                    'cập nhật trạng thái IsEdited Quyết định thanh tra cho biết có được sửa Số QĐ nữa hay không?
                Else
                    pn.TenPhieu = "[PTKT]" + "-" + p.TenDoanhNghiep + "-" + Date.Now.ToString("dd/MM/yyyy HH:mm:ss")
                    pn.LoaiPhieu = False
                    pn.SoQuyenDinh = "PTKT/xxx/" & Now.Year.ToString & "/" & p.Tinh.KiHieu & "/No"
                End If
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
                pn.SoNguoiLamCongViecYeuCauNghiemNgat = p.SoNguoiLamCongViecYeuCauNghiemNgat
                pn.TongGiaTriSP = p.TongGiaTriSP
                pn.TongLoiNhuanSauThue = p.TongLoiNhuanSauThue
                pn.IsCongDoan = p.IsCongDoan
                pn.NguoiLienHe = p.NguoiLienHe
                pn.DienThoaiLH = p.DienThoaiLH
                pn.EmailLH = p.EmailLH
                pn.NgayTao = Date.Now
                pn.NguoiTao = Session("Username")
                data.PhieuNhapHeaders.AddObject(pn)
                data.SaveChanges()

                Session("phieuid") = pn.PhieuID 'Luu phieuid moi
                Session("ModePhieu") = ModePhieu.Create
                Insert_App_Log("Insert  Phieunhapheader:" & pn.TenPhieu & "", Function_Name.BienBanThanhTra, Audit_Type.Create, Request.ServerVariables("REMOTE_ADDR"), Session("UserName"))
            End If
        End Using

    End Sub
#End Region
End Class
