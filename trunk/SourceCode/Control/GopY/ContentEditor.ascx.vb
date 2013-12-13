Imports System.Data
Imports ThanhTraLaoDongModel
Imports Cls_Common
Imports SecurityService

Partial Class Control_GopY_ContentEditor
    Inherits System.Web.UI.UserControl
    Private Shared ReadOnly log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

#Region "Sub and Function "
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            'active tab tuong ung voi Page
            'InVisble button ko can thiet
            If Request.PhysicalPath.Contains("List.aspx") Then
                lbtList.CssClass = "current_manage"
                lbtList.Text = "<span class='current_manage'>Quản lý</span>"
            End If
            If Not Request.QueryString("GopYId") Is Nothing Then
                hidID.Value = Request.QueryString("GopYId")
            End If
        End If
    End Sub
    Protected Sub setPermission()
        'Se bat lai khi dua chuc nang phan quyen
        'lbtAdd.Visible = HasPermission(Function_Name.GopY, Session("RoleID"), 0, Audit_Type.Create)
    End Sub
    Protected Sub lbtList_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbtList.Click
        Response.Redirect("List.aspx")
    End Sub

#End Region

End Class
