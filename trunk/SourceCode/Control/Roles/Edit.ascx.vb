Imports System.Data
Imports ThanhTraLaoDongModel
Imports Cls_Common
Imports SecurityService

Partial Class Control_Roles_Edit
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
            If Not Request.QueryString("Roleid") Is Nothing Then
                hidID.Value = Request.QueryString("Roleid")
                ShowData()
            End If
        End If
    End Sub
    Protected Sub ShowData()
        Using data As New ThanhTraLaoDongEntities
            Dim p As ThanhTraLaoDongModel.Role = (From q In data.Roles Where q.RoleId = hidID.Value).SingleOrDefault
            If Not p Is Nothing Then
                txtTitle.Text = IIf(IsNothing(p.Title) = True, "", p.Title)
                txtDescription.Text = IIf(IsNothing(p.Description) = True, "", p.Description)
                hidTitle.Value = IIf(IsNothing(p.Title) = True, "", p.Title)
            End If
        End Using
    End Sub
#End Region
#Region "Event for control"
    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Using data As New ThanhTraLaoDongEntities
            'Kiem tra trung tieu de
            If hidTitle.Value.Trim <> txtTitle.Text.Trim() Then
                Dim Title As String = txtTitle.Text.Trim()
                Dim pCheck = (From q In data.Roles Where q.Title = txtTitle.Text.Trim() Select q).ToList()
                If pCheck.Count > 0 Then
                    Excute_Javascript("Alertbox('Vai trò đã tồn tại trong hệ thống, vui lòng nhập vài trò khác');", Me.Page)
                    Return
                End If
            End If
            Dim p As Role = (From q In data.Roles Where q.RoleId = hidID.Value).SingleOrDefault
            Try
                p.RoleId = hidID.Value
                p.Title = txtTitle.Text.Trim()
                p.Description = txtDescription.Text.Trim()
                data.SaveChanges()
                Insert_App_Log("Edit Roles:" & txtTitle.Text.Trim & "", Function_Name.Role, Audit_Type.Edit, Request.ServerVariables("REMOTE_ADDR"), Session("UserName"))
                Excute_Javascript("Alertbox('Cập nhật dữ liệu thành công.');window.location ='../../Page/Roles/List.aspx';", Me.Page, True)
            Catch ex As Exception
                log4net.Config.XmlConfigurator.Configure()
                log.Error("Error error " & AddTabSpace(1) & Session("Username") & AddTabSpace(1) & "IP:" & GetIPAddress(), ex)
                Excute_Javascript("Alertbox('Cập nhật thất bại.');window.location ='../../Page/Roles/List.aspx';", Me.Page, True)
            End Try
        End Using
    End Sub
#End Region
End Class
