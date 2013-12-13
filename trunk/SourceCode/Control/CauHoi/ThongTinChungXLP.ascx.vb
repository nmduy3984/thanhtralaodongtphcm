
Imports System.Data
Imports ThanhTraLaoDongModel
Imports Cls_Common
Partial Class Control_CauHoi_ThongTinChungXLP
    Inherits System.Web.UI.UserControl
#Region "Sub and Function "
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
                If Not Request.QueryString("DNId").ToString.Equals("0") Then
                    hidID.Value = Request.QueryString("DNId")
                    ShowData()
                End If

            Else
                Response.Redirect("../../Login.aspx")
            End If
        End If
    End Sub
    Protected Sub ShowData()
        Using data As New ThanhTraLaoDongEntities
            Dim iBool As Boolean = Request.PhysicalPath.Contains("DoanhNghiep")
            Dim iPhieuId = CInt(Session("phieuid"))
            Dim p As New List(Of uspDoanhNghiepDetail_Result)
            If iBool Then
                p = data.uspDoanhNghiepDetail(hidID.Value, 0, 1).ToList()
            Else
                p = data.uspDoanhNghiepDetail(hidID.Value, iPhieuId, 2).ToList()
            End If

            If p.Count > 0 Then
                Dim pn = (From a In data.PhieuNhapHeaders Where a.PhieuID = iPhieuId).FirstOrDefault
                lkbTendoanhnghiep.Text = IIf(IsNothing(p(0).TenDoanhNghiep) = True, "", p(0).TenDoanhNghiep)
                lblDienthoai.Text = IIf(IsNothing(p(0).DienThoai) = True, "", p(0).DienThoai)
                lblFax.Text = IIf(IsNothing(p(0).Fax) = True, "", p(0).Fax)
                lblNamtldn.Text = IIf(IsNothing(p(0).NamTLDN) = True, "", p(0).NamTLDN)
                lblLoaihinhdnid.Text = IIf(IsNothing(p(0).LoaiHinhDoanhNghiep) = True, "", p(0).LoaiHinhDoanhNghiep)
                lblTrusochinh.Text = IIf(IsNothing(p(0).TruSoChinh) = True, "", p(0).TruSoChinh)
                lblHuyenid.Text = IIf(IsNothing(p(0).Huyen) = True, "", p(0).Huyen)
                lblTinhid.Text = IIf(IsNothing(p(0).Tinh) = True, "", p(0).Tinh)
                lblKhucongnghiep.Text = IIf(IsNothing(p(0).KhuCongNghiep) = True, "", p(0).KhuCongNghiep)
                lblSotknganhang.Text = IIf(IsNothing(p(0).SoTKNganHang) = True, "", p(0).SoTKNganHang)
                lblTennganhang.Text = IIf(IsNothing(p(0).TenNganHang) = True, "", p(0).TenNganHang)
                lblUrl.Text = IIf(IsNothing(p(0).Url) = True, "", p(0).Url)
                lblUrl.NavigateUrl = IIf(IsNothing(p(0).Url) = True, "", p(0).Url)
                lblEmail.Text = IIf(IsNothing(p(0).Email) = True, "", p(0).Email)
                lblLoaihinhsxid.Text = IIf(IsNothing(p(0).LoaiHinhSanXuat) = True, "", p(0).LoaiHinhSanXuat)
                lblSochungnhandkkd.Text = IIf(IsNothing(p(0).SoChungNhanDKKD) = True, "", p(0).SoChungNhanDKKD)
                If IsNothing(p(0).NgayChungNhanDKKD) Then
                    lblNgaychungnhandkkd.Text = ""
                Else
                    lblNgaychungnhandkkd.Text = CType(p(0).NgayChungNhanDKKD, Date).ToString("dd/MM/yyyy")
                End If
                lblSochinhanh.Text = IIf(IsNothing(p(0).SoChiNhanh) = True, "", String.Format("{0:n0}", p(0).SoChiNhanh))
                lblTongsonhanvien.Text = IIf(IsNothing(p(0).TongSoNhanVien) = True, "", String.Format("{0:n0}", p(0).TongSoNhanVien))
                lblSolaodongnu.Text = IIf(IsNothing(p(0).SoLaoDongNu) = True, "", String.Format("{0:n0}", p(0).SoLaoDongNu))
                lblSonguoilamnghenguyhiem.Text = IIf(IsNothing(p(0).SoNguoiLamNgheNguyHiem) = True, "", String.Format("{0:n0}", p(0).SoNguoiLamNgheNguyHiem))
                lblNguoilamCVCoYCNN.Text = IIf(IsNothing(p(0).SoNguoiLamCongViecYeuCauNghiemNgat) = True, "", String.Format("{0:n0}", p(0).SoNguoiLamCongViecYeuCauNghiemNgat))
                lblTonggiatrisp.Text = IIf(IsNothing(p(0).TongGiaTriSP) = True, "", p(0).TongGiaTriSP)
                lblTongloinhuansauthue.Text = IIf(IsNothing(p(0).TongLoiNhuanSauThue) = True, "", p(0).TongLoiNhuanSauThue)
                If IsNothing(p(0).IsCongDoan) Or Not p(0).IsCongDoan Then
                    lblIscongdoan.Text = "Không có"
                Else
                    lblIscongdoan.Text = "Có"
                End If
                lblNguoiLienHe.Text = IIf(IsNothing(p(0).NguoiLienHe) = True, "", p(0).NguoiLienHe)
                lblDienThoaiLH.Text = IIf(IsNothing(p(0).DienThoaiLH) = True, "", p(0).DienThoaiLH)
                lblEmailLH.Text = IIf(IsNothing(p(0).EmailLH) = True, "", p(0).EmailLH)
                'generate so quyet dinh                
            Else
                Excute_Javascript("AlertboxRedirect('Không tồn tại doanh nghiệp này.','List.aspx');", Me.Page, True)
            End If
        End Using
    End Sub
#End Region
#Region "Event for control"
    Protected Sub btnBack_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs) Handles btnBack.Click
        Response.Redirect("List.aspx")
    End Sub

    Protected Sub btnSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSubmit.Click
        Session("EditInfoPhieu") = 2
        Response.Redirect("../../Page/DoanhNghiepXLP/Edit.aspx?DNId=" & hidID.Value)
    End Sub
#End Region
End Class
