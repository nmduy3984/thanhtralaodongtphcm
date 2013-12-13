Imports System.Data
Imports ThanhTraLaoDongModel
Imports Cls_Common
Partial Class Control_QuyetDinhThanhTra_Detail
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
                If Not Request.QueryString("SoQuyetDinh").ToString.Equals("") Then
                    hidID.Value = Request.QueryString("SoQuyetDinh")
                    ShowData()
                End If
            Else
                Response.Redirect("../../Login.aspx")
            End If
        End If
    End Sub
    Protected Sub ShowData()
        Using data As New ThanhTraLaoDongEntities
            Dim p = (From c In data.QuyetDinhThanhTras Join a In data.vChucDanhs On c.ChucDanhNguoiKy Equals a.Id
                    Where c.SoQuyetDinh.Equals(hidID.Value)
                    Select New With {c, a.Name}).ToList

            If p.Count > 0 Then
                lblSoQD.Text = IIf(IsNothing(p(0).c.SoQuyetDinh) = True, "", p(0).c.SoQuyetDinh)
                If p(0).c.LoaiQuyetDinh = 1 Then
                    lblLoaiQuyetDinh.Text = "Thanh tra"
                Else
                    lblLoaiQuyetDinh.Text = "Kiểm tra"
                End If

                lblPhamVi.Text = IIf(IsNothing(p(0).c.PhamVi) = True, "", p(0).c.PhamVi)
                If Not IsNothing(p(0).c.CanCuLuat) Then
                    Dim arrCanCuLuat() As String = p(0).c.CanCuLuat.Split(Str_Symbol_Group)
                    Dim str = "<ol>"
                    For index As Integer = 0 To arrCanCuLuat.Count - 2
                        '' Thêm dữ liệu vào document
                        str += "<li>" + arrCanCuLuat(index) + "</li>"
                    Next
                    str += "<li>" + arrCanCuLuat(arrCanCuLuat.Count - 1) + "</li></ol>"
                    lblCanCuLuat.Text = str
                End If

                If Not IsNothing(p(0).c.CanCuQuyetDinh) Then
                    Dim arrCanCuQuyetDinh() As String = p(0).c.CanCuQuyetDinh.Split(Str_Symbol_Group)
                    Dim str = "<ol>"
                    For index As Integer = 0 To arrCanCuQuyetDinh.Count - 2
                        '' Thêm dữ liệu vào document
                        str += "<li>" + arrCanCuQuyetDinh(index) + "</li>"
                    Next
                    str += "<li>" + arrCanCuQuyetDinh(arrCanCuQuyetDinh.Count - 1) + "</li></ol>"
                    lblCanCuQD.Text = str
                End If
                lblDiaBan.Text = IIf(IsNothing(p(0).c.DiaBanTTKT) = True, "", p(0).c.Tinh.TenTinh)

                If Not IsNothing(p(0).c.ThanhVienDoan) Then
                    Dim arrThanhVienDoan() As String = p(0).c.ThanhVienDoan.Split(Str_Symbol_Group)
                    Dim str = "<ul style='list-style-type: circle'>"
                    For index As Integer = 0 To arrThanhVienDoan.Count - 2
                        '' Thêm dữ liệu vào document
                        str += "<li>" + arrThanhVienDoan(index) + "</li>"
                    Next
                    str += "<li>" + arrThanhVienDoan(arrThanhVienDoan.Count - 1) + "</li></ul>"
                    lblThanhTraDoan.Text = str
                End If
                lblTrachNhiemThiHanh.Text = IIf(IsNothing(p(0).c.TrachNhiemThiHanh) = True, "", p(0).c.TrachNhiemThiHanh)
                lblChucDanh.Text = p(0).Name
                If Not IsNothing(p(0).c.NoiNhan) Then
                    Dim arrNoiNhan() As String = p(0).c.NoiNhan.Split(Str_Symbol_Group)
                    Dim str = "<ul style='list-style-type: circle'>"
                    For index As Integer = 0 To arrNoiNhan.Count - 2
                        '' Thêm dữ liệu vào document
                        str += "<li>" + arrNoiNhan(index) + "</li>"
                    Next
                    str += "<li>" + arrNoiNhan(arrNoiNhan.Count - 1) + "</li></ul>"
                    lblNoiNhan.Text = str
                End If
                 
                lblNguoiKyQuyetDinh.Text = IIf(IsNothing(p(0).c.NguoiKyQuyetDinh) = True, "", p(0).c.NguoiKyQuyetDinh)
                If IsNothing(p(0).c.NgayTao) Then
                    lblNgayTao.Text = ""
                Else
                    lblNgayTao.Text = CType(p(0).c.NgayTao, Date).ToString("dd/MM/yyyy")
                End If
                lblNguoiTao.Text = IIf(IsNothing(p(0).c.NguoiTao) = True, "", p(0).c.NguoiTao)
            Else
                Excute_Javascript("AlertboxRedirect('Không tồn tại Số Quyết định này.','List.aspx');", Me.Page, True)
            End If
        End Using
    End Sub
#End Region
#Region "Event for control"
    Protected Sub btnBack_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs) Handles btnBack.Click
        Response.Redirect("List.aspx")
    End Sub

#End Region
End Class
