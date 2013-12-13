Imports System.Data
Imports ThanhTraLaoDongModel
Imports Cls_Common
Imports SecurityService

Partial Class Control_Roles_Create
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

        End If
    End Sub
#End Region
#Region "Event for control "
    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Using data As New ThanhTraLaoDongEntities
            Dim p As New Role
            Dim pCheck = (From q In data.Roles Where q.Title = txtTitle.Text.Trim() Select q).ToList
            If Not pCheck.Count > 0 Then
                Try
                    p.Title = txtTitle.Text.Trim()
                    p.Description = txtDescription.Text.Trim()
                    data.Roles.AddObject(p)
                    data.SaveChanges()
                    Insert_App_Log("Insert Roles:" & txtTitle.Text.Trim & "", Function_Name.Role, Audit_Type.Create, Request.ServerVariables("REMOTE_ADDR"), Session("UserName"))
                    Excute_Javascript("Alertbox('Cập nhật dữ liệu thành công.');window.location ='../../Page/Roles/List.aspx';", Me.Page, True)
                Catch ex As Exception
                    log4net.Config.XmlConfigurator.Configure()
                    log.Error("Error error " & AddTabSpace(1) & Session("Username") & AddTabSpace(1) & "IP:" & GetIPAddress(), ex)
                    Excute_Javascript("Alertbox('Cập nhật thất bại.');window.location ='../../Page/Roles/List.aspx';", Me.Page, True)
                End Try
            Else
                Excute_Javascript("Alertbox('Vai trò đã tồn tại trong hệ thống, vui lòng nhập vài trò khác');", Me.Page)
            End If

        End Using
    End Sub
#End Region
End Class
