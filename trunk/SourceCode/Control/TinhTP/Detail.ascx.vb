Imports System.Data
Imports ThanhTraLaoDongModel
Imports Cls_Common

Partial Class Control_TinhTP_Detail
    Inherits System.Web.UI.UserControl
#Region "Sub and Function "
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Dim script As ScriptManager = ScriptManager.GetCurrent(Me.Page)
            If Not script Is Nothing AndAlso script.IsInAsyncPostBack Then
                 ScriptManager.RegisterStartupScript(Me.Page, Me.Page.GetType, "duyjs1", "ajaxJqueryToolTip()", True)
            Else
                 Me.Page.ClientScript.RegisterStartupScript(Me.Page.GetType(), "duyjs1", String.Concat("Sys.Application.add_load(function(){", "ajaxJqueryToolTip()", "});"), True)
            End If
            If Not Request.QueryString("TinhId").ToString.Equals("0") Then
                hidID.Value = Request.QueryString("TinhId")
                ShowData()
            End If
        End If
    End Sub
    Protected Sub ShowData()
        Using data As New ThanhTraLaoDongEntities
            Dim p As Tinh = (From q In data.Tinhs Where q.TinhId = hidID.Value Select q).SingleOrDefault
            If Not p Is Nothing Then
                lblCode.Text = IIf(IsNothing(p.MaTinh) = True, "", p.MaTinh)
                lblTitle.Text = IIf(IsNothing(p.TenTinh) = True, "", p.TenTinh)
                lblKyhieu.Text = IIf(IsNothing(p.KiHieu) = True, "", p.KiHieu)
                lblDescription_detail.Text = IIf(IsNothing(p.MoTa) = True, "", p.MoTa)
                lblDienThoai.Text = IIf(IsNothing(p.DienThoai) = True, "", p.DienThoai)
                lblTenSo.Text = IIf(IsNothing(p.TenSo) = True, "", p.TenSo)

            End If
        End Using
    End Sub
#End Region
End Class
