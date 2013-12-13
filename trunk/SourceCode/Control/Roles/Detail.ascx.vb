Imports System.Data
Imports ThanhTraLaoDongModel
Imports Cls_Common

Partial Class Control_Roles_Detail
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
            If Not Request.QueryString("Roleid") Is Nothing Then
                hidID.Value = Request.QueryString("Roleid")
                ShowData()
            End If
        End If
    End Sub
    Protected Sub ShowData()
        Using data As New ThanhTraLaoDongEntities
            Dim p As Role = (From q In data.Roles Where q.RoleId = hidID.Value Select q).SingleOrDefault
            If Not p Is Nothing Then
                lblTitle.Text = IIf(IsNothing(p.Title) = True, "", p.Title)
                lblDescription.Text = IIf(IsNothing(p.Description) = True, "", p.Description)
            End If
        End Using
    End Sub
#End Region
End Class
