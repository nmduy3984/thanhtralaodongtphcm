Imports System.Data
Imports ThanhTraLaoDongModel
Imports Cls_Common
Imports SecurityService

Partial Class Control_Roles_ContentEditor
    Inherits System.Web.UI.UserControl
    Private Shared ReadOnly log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
#Region "Sub and Function "
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            'active tab tuong ung voi Page
            'InVisble button ko can thiet
            If Request.PhysicalPath.Contains("List.aspx") Then
                lbtList.CssClass = "current_manage"
                lbtList.Text = "<span class=""current_manage"">Quản lý</span>"
                lbtEdit.Visible = False
                lbtView.Visible = False
                btnDelete.Visible = False
                lbtAdd.Visible = HasPermission(Function_Name.Role, Session("RoleID"), 0, Audit_Type.Create)
            ElseIf Request.PhysicalPath.Contains("Create.aspx") Then
                lbtAdd.CssClass = "current_manage"
                lbtAdd.Text = "<span class=""current_manage"">Thêm mới</span>"
                lbtEdit.Visible = False
                lbtView.Visible = False
                btnDelete.Visible = False
                lbtAdd.Visible = HasPermission(Function_Name.Role, Session("RoleID"), 0, Audit_Type.Create)
            ElseIf Request.PhysicalPath.Contains("Detail.aspx") Then
                lbtView.CssClass = "current_manage"
                lbtView.Text = "<span class=""current_manage"">Xem</span>"
                setPermission()
            ElseIf Request.PhysicalPath.Contains("Edit.aspx") Then
                lbtEdit.CssClass = "current_manage"
                lbtEdit.Text = "<span class=""current_manage"">Sửa</span>"
                setPermission()
            End If

            If Not Request.QueryString("Roleid") Is Nothing Then
                hidID.Value = Request.QueryString("Roleid")
                btnDelete.Attributes.Add("onclick", "return ComfirmDialog('Bạn có chắc chắn muốn xóa không?',0,'" + btnDelete.ClientID + "','" + hidID.Value.ToString + "',1);")
            End If
        End If
    End Sub
    Protected Sub setPermission()
        lbtAdd.Visible = HasPermission(Function_Name.Role, Session("RoleID"), 0, Audit_Type.Create)
        lbtView.Visible = HasPermission(Function_Name.Role, Session("RoleID"), 0, Audit_Type.ViewContent)
        lbtEdit.Visible = HasPermission(Function_Name.Role, Session("RoleID"), 0, Audit_Type.Edit)
        btnDelete.Visible = HasPermission(Function_Name.Role, Session("RoleID"), 0, Audit_Type.Delete)
    End Sub
    Protected Sub lbtList_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbtList.Click
        Response.Redirect("List.aspx")
    End Sub
    Protected Sub lbtAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbtAdd.Click
        Response.Redirect("Create.aspx")
    End Sub
    Protected Sub lbtEdit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbtEdit.Click
        Response.Redirect("Edit.aspx?RoleId=" & hidID.Value & "")
    End Sub
    Protected Sub lbtView_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbtView.Click
        Response.Redirect("Detail.aspx?RoleId=" & hidID.Value & "")
    End Sub
    Protected Sub lnkbtnDelete_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnDelete.Click
        Using data As New ThanhTraLaoDongEntities
            Dim intId As Integer
            Dim strLogName As String = ""
            intId = hidID.Value
            Dim q = (From p In data.Roles Where p.RoleId = intId Select p).FirstOrDefault
            Try
                data.Roles.DeleteObject(q)
                data.SaveChanges()
                Insert_App_Log("Delete Roles:" & q.Title & "", Function_Name.Role, Audit_Type.Delete, Request.ServerVariables("REMOTE_ADDR"), Session("UserName"))
                Excute_Javascript("Alertbox('Xóa dữ liệu thành công.');window.location ='../../Page/Roles/List.aspx';", Me.Page, True)
            Catch ex As Exception
                log4net.Config.XmlConfigurator.Configure()
                log.Error("Error error " & AddTabSpace(1) & Session("Username") & AddTabSpace(1) & "IP:" & GetIPAddress(), ex)
                Excute_Javascript("Alertbox('Xóa thất bại.');", Me.Page, True)
            End Try
        End Using
    End Sub
#End Region
End Class
