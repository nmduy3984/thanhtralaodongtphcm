Imports System.Data
Imports ThanhTraLaoDongModel
Imports Cls_Common

Partial Class Control_Users_Detail
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
            If Not Request.QueryString("Userid") Is Nothing Then
                hidID.Value = Request.QueryString("Userid")
                Using data As New ThanhTraLaoDongEntities
                    Dim p = (From q In data.Roles Order By q.Title Ascending, q.RoleId Descending Select q).ToList
                    chklstRole.DataSource = p
                    chklstRole.DataTextField = "Title"
                    chklstRole.DataValueField = "RoleID"
                    chklstRole.DataBind()
                End Using
                ShowData()
            End If
        End If
    End Sub
    Protected Sub ShowData()
        Using data As New ThanhTraLaoDongEntities
            Dim p = (From q In data.Users, t In data.Tinhs Where t.TinhId = q.TinhTP And q.UserId = hidID.Value Select New With {q.UserName, q.Email, q.UserId, q.IsUser, q.LastLogin, q.FullName, q.IsActivated, q.Created, .TenTinh = t.TenTinh}).SingleOrDefault
            If Not p Is Nothing Then
                lblUsername.Text = IIf(IsNothing(p.UserName) = True, "", p.UserName)
                lblEmail.Text = IIf(IsNothing(p.Email) = True, "", p.Email)
                lblTinh.Text = IIf(IsNothing(p.TenTinh), "", p.TenTinh)
                lblLastlogin.Text = String.Format("{0:dd/MM/yyyy hh:mm}", IIf(IsNothing(p.LastLogin) = True, "", p.LastLogin))
                lblFullname.Text = IIf(IsNothing(p.FullName) = True, "", p.FullName)
                lblIsuser.Text = getUserType(IIf(IsNothing(p.IsUser) = True, 1, p.IsUser))
                lblCreated.Text = String.Format("{0:dd/MM/yyyy hh:mm}", IIf(IsNothing(p.Created) = True, "", p.Created))
                CheckIsactivated.Checked = IIf(IsNothing(p.IsActivated) = True, False, p.IsActivated)
            End If
            Dim ur = (From q In data.UserRoles Where q.UserId = hidID.Value Select q.RoleId).ToArray
            If Not ur Is Nothing Then
                For Each role In chklstRole.Items
                    Dim value = role.Value
                    Dim checked = (From q In ur Where q = value Select q).ToList
                    If checked.Count >= 1 Then
                        role.Selected = True
                    End If
                Next
            End If
            chklstRole.Attributes.Add("disabled", "true")
        End Using
    End Sub
    Private Function getUserType(ByVal intUserType As Integer) As String
        For index = 0 To Cls_Common.ArrUser_Type.Length - 1
            If index = intUserType Then
                Return Cls_Common.ArrUser_Type(index)
            End If
        Next
        Return ""
    End Function
#End Region
End Class
