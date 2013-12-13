Imports System.Data
Imports ThanhTraLaoDongModel
Imports Cls_Common
Imports SecurityService

Partial Class Control_User_Detail
    Inherits System.Web.UI.UserControl
    Private Shared ReadOnly log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
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
                ShowData()
            End If
        End If
    End Sub
   
    Protected Sub ShowData()
        Using data As New ThanhTraLaoDongEntities
            Dim p = (From q In data.Users Where q.UserId = hidID.Value Select New With {q.UserName, q.Email, q.UserId, q.IsUser, q.LastLogin, q.FullName, q.IsActivated, q.Created}).SingleOrDefault
            If Not p Is Nothing Then
                lblUsername.Text = IIf(IsNothing(p.UserName) = True, "", p.UserName) & "&nbsp;"
                lblEmail.Text = IIf(IsNothing(p.Email) = True, "", p.Email) & "&nbsp;"
                lblFullname.Text = IIf(IsNothing(p.FullName) = True, "", p.FullName) & "&nbsp;"
            End If
        End Using
    End Sub
#End Region

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Using data As New ThanhTraLaoDongEntities
            Dim p = (From q In data.Users Where q.UserId = hidID.Value Select q).SingleOrDefault
            If Not p Is Nothing Then
                Try
                    p.Password = Cls_Common.Encrypt(txtPassword.Text.Trim())
                    data.SaveChanges()
                    Insert_App_Log("Reset password user:" & p.UserName & "", Function_Name.User, Audit_Type.Edit, Request.ServerVariables("REMOTE_ADDR"), Session("UserName"))
                    Excute_Javascript("Alertbox('Nhập lại mật khẩu người dùng thành công.');window.location ='../../Page/Users/List.aspx';", Me.Page, True)
                Catch ex As Exception
                    log4net.Config.XmlConfigurator.Configure()
                    log.Error("Reset password user error " & AddTabSpace(1) & Session("Username") & AddTabSpace(1) & "IP:" & GetIPAddress(), ex)
                    Excute_Javascript("Alertbox('Nhập lại mật khẩu người dùng thất bại.');", Me.Page, True)
                End Try
            End If
        End Using
    End Sub
End Class
