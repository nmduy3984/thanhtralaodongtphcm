Imports System.Data
Imports ThanhTraLaoDongModel
Imports Cls_Common
Partial Class Control_QuanHuyen_Detail
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
            If Not Request.QueryString("HuyenId").ToString.Equals("0") Then
                hidID.Value = Request.QueryString("HuyenId")
                ShowData()
            End If
        End If
    End Sub
    Protected Sub ShowData()
        Using data As New ThanhTraLaoDongEntities
            Dim p As Huyen = (From q In data.Huyens Where q.HuyenId = hidID.Value Select q).FirstOrDefault
            If Not p Is Nothing Then
                lblTitle.Text = IIf(IsNothing(p.TenHuyen) = True, "", p.TenHuyen)
                lblLuongToiThieu.Text = IIf(IsNothing(p.LuongToiThieuID) = True, "", p.LuongToiThieu.TieuDe)
                lblDescription_detail.Text = IIf(IsNothing(p.MoTa) = True, "", p.MoTa)
            End If
        End Using
    End Sub
#End Region
End Class
