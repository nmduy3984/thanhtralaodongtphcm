Imports System.Data
Imports ThanhTraLaoDongModel
Imports Cls_Common
Imports SecurityService

Partial Class Control_Users_ContentEditor
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
                lbtResetPass.Visible = False
                lbtAdd.Visible = HasPermission(Function_Name.User, Session("RoleID"), 0, Audit_Type.Create)
                lbtUsersTinh.Visible = False
                If Session("TinhThanhTraSo") = 0 Then
                    lbtUsersTinh.Visible = True
                End If
            ElseIf Request.PhysicalPath.Contains("Create.aspx") Then
                lbtAdd.CssClass = "current_manage"
                lbtAdd.Text = "<span class=""current_manage"">Thêm mới</span>"
                lbtEdit.Visible = False
                lbtView.Visible = False
                btnDelete.Visible = False
                lbtResetPass.Visible = False
                lbtAdd.Visible = HasPermission(Function_Name.User, Session("RoleID"), 0, Audit_Type.Create)
                lbtUsersTinh.Visible = False
                If Session("TinhThanhTraSo") = 0 Then
                    lbtUsersTinh.Visible = True
                End If
            ElseIf Request.PhysicalPath.Contains("Detail.aspx") Then
                lbtView.CssClass = "current_manage"
                lbtView.Text = "<span class=""current_manage"">Xem</span>"
                lbtUsersTinh.Visible = False
                If Session("TinhThanhTraSo") = 0 Then
                    lbtUsersTinh.Visible = True
                End If
                setPermission()
            ElseIf Request.PhysicalPath.Contains("Edit.aspx") Then
                lbtEdit.CssClass = "current_manage"
                lbtEdit.Text = "<span class=""current_manage"">Sửa</span>"
                lbtUsersTinh.Visible = False
                If Session("TinhThanhTraSo") = 0 Then
                    lbtUsersTinh.Visible = True
                End If
                setPermission()

            ElseIf Request.PhysicalPath.Contains("UserTinh.aspx") Then

                lbtUsersTinh.CssClass = "current_manage"
                lbtUsersTinh.Text = "<span class=""current_manage"">Người dùng - Tỉnh thành</span>"
                lbtResetPass.Visible = False
                lbtEdit.Visible = False
                lbtView.Visible = False
                btnDelete.Visible = False
                '  setPermission()
            ElseIf Request.PhysicalPath.Contains("ResetPass.aspx") Then
                lbtResetPass.CssClass = "current_manage"
                lbtResetPass.Text = "<span class=""current_manage"">Cấp lại mật khẩu</span>"
                lbtResetPass.Visible = True
                lbtUsersTinh.Visible = False
                If Session("TinhThanhTraSo") = 0 Then
                    lbtUsersTinh.Visible = True
                End If
                setPermission()
            End If
            If Not Request.QueryString("Userid") Is Nothing Then
                hidID.Value = Request.QueryString("Userid")
                btnDelete.Attributes.Add("onclick", "return ComfirmDialog('Bạn có chắc chắn muốn xóa không?',0,'" + btnDelete.ClientID + "','" + hidID.Value.ToString + "',1);")
            End If
        End If
    End Sub
    Protected Sub setPermission()
        lbtAdd.Visible = HasPermission(Function_Name.User, Session("RoleID"), 0, Audit_Type.Create)
        lbtView.Visible = HasPermission(Function_Name.User, Session("RoleID"), 0, Audit_Type.ViewContent)
        lbtEdit.Visible = HasPermission(Function_Name.User, Session("RoleID"), 0, Audit_Type.Edit)
        btnDelete.Visible = HasPermission(Function_Name.User, Session("RoleID"), 0, Audit_Type.Delete)

    End Sub
    Protected Sub lbtList_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbtList.Click
        Response.Redirect("List.aspx")
    End Sub
    Protected Sub lbtAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbtAdd.Click
        Response.Redirect("Create.aspx")
    End Sub
    Protected Sub lbtEdit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbtEdit.Click
        Response.Redirect("Edit.aspx?UserId=" & hidID.Value & "")
    End Sub
    Protected Sub lbtView_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbtView.Click
        Response.Redirect("Detail.aspx?UserId=" & hidID.Value & "")
    End Sub

    Protected Sub lbtUsersTinh_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbtUsersTinh.Click
        Response.Redirect("UserTinh.aspx?UserId=" & hidID.Value & "")
    End Sub

    Protected Sub lnkbtnDelete_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnDelete.Click
        Using data As New ThanhTraLaoDongEntities
            Dim intId As Integer
            Dim strLogName As String = ""
            intId = hidID.Value
            Dim q = (From p In data.Users Where p.UserId = intId Select p).FirstOrDefault
            Try
                data.Users.DeleteObject(q)
                data.SaveChanges()
                Excute_Javascript("Alertbox('Xóa dữ liệu thành công.');window.location ='../../Page/Users/List.aspx';", Me.Page, True)
            Catch ex As Exception
                log4net.Config.XmlConfigurator.Configure()
                log.Error("Error error " & AddTabSpace(1) & Session("Username") & AddTabSpace(1) & "IP:" & GetIPAddress(), ex)
                Excute_Javascript("Alertbox('Xóa thất bại.');", Me.Page, True)
            End Try
        End Using
    End Sub
    Protected Sub lbtResetPass_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbtResetPass.Click
        Response.Redirect("ResetPass.aspx?UserId=" & hidID.Value & "")
    End Sub
#End Region

End Class
