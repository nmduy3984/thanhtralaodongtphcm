Imports System.Data
Imports ThanhTraLaoDongModel
Imports Cls_Common
Partial Class Control_HanhVi_Detail
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
            If Not Request.QueryString("Hanhviid").ToString.Equals("0") Then
                hidID.Value = Request.QueryString("Hanhviid")
                ShowData()
            End If
        End If
    End Sub
    Protected Sub ShowData()
        Using data As New thanhtralaodongEntities
            Dim p = (From q In data.DanhMucHanhVis, a In data.vLoaiHanhVis Where q.HanhViId = hidID.Value And a.Id = q.LoaiHanhVi Select New With {q.Title, q.MucPhatMax, q.MucPhatMin, a.Name}).ToList
            If Not p Is Nothing Then
                lblMota.Text = IIf(IsNothing(p(0).Title) = True, "", p(0).Title)
                lblMucphatmin.Text = IIf(IsNothing(p(0).MucPhatMin) = True, "", String.Format("{0:n0}", p(0).MucPhatMin))
                lblMucphatmax.Text = IIf(IsNothing(p(0).MucPhatMax) = True, "", String.Format("{0:n0}", p(0).MucPhatMax))
                lblLoaihanhvi.Text = IIf(IsNothing(p(0).Name) = True, "", p(0).Name)
            End If
        End Using
    End Sub
#End Region
#Region "Event for control"
    Protected Sub btnBack_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnBack.Click
        Response.Redirect("List.aspx")
    End Sub
#End Region
End Class
