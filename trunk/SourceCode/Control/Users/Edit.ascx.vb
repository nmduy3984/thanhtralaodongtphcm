Imports System.Data
Imports ThanhTraLaoDongModel
Imports Cls_Common
Imports SecurityService

Partial Class Control_Users_Edit
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
            hidTinhThanhTraSo.Value = Session("TinhThanhTraSo")
            BindListRole()
            BindAccountType()
             ShowData()
        End If
    End Sub

    Private Sub BindListRole()
        If Not Request("Userid") Is Nothing Then
            hidID.Value = Request.QueryString("Userid")
            'bind role list
            Using data As New ThanhTraLaoDongEntities
                Dim p = (From q In data.Roles Order By q.Title Ascending, q.RoleId Descending Select q).ToList
                chklstRole.DataSource = p
                chklstRole.DataTextField = "Title"
                chklstRole.DataValueField = "RoleID"
                chklstRole.DataBind()
            End Using
        End If
    End Sub

    Private Sub BindAccountType()
        'Bind AccountType
        Using Data As New ThanhTraLaoDongEntities
            Dim p = (From q In Data.vAccountTypes Select q).ToList
            With ddlTypeUser
                .DataValueField = "ID"
                .DataTextField = "Name"
                .DataSource = p
                .DataBind()
            End With
        End Using
        Dim lstItem As New ListItem("--- Chọn ---", "")
        ddlTypeUser.Items.Insert(0, lstItem)
    End Sub
    Protected Sub ShowData()
        Using data As New ThanhTraLaoDongEntities
            Dim p As User = (From q In data.Users Where q.UserId = hidID.Value).SingleOrDefault
            If Not p Is Nothing Then
                txtUsername.Text = p.UserName
                txtEmail.Text = p.Email
                curUserName.Value = p.UserName
                txtPassword.Attributes.Add("value", p.Password)
                txtPassword.Enabled = False
                txtFullname.Text = IIf(IsNothing(p.FullName) = True, "", p.FullName)
                ddlTypeUser.SelectedValue = p.IsUser
                chkIsActivated.Checked = p.IsActivated
            End If
            'check all of user's role
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
        End Using
    End Sub
#End Region
#Region "Event for control"
    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If checkExistUsername() = False Then
            Using data As New ThanhTraLaoDongEntities
                Dim p As User = (From q In data.Users Where q.UserId = hidID.Value).SingleOrDefault
                Try
                    'update useraccount
                    p.UserId = hidID.Value
                    p.UserName = txtUsername.Text.Trim()
                    p.Email = txtEmail.Text.Trim()
                    p.TinhTP = CInt(hidTinhThanhTraSo.Value)
                    p.FullName = txtFullname.Text.Trim()
                    p.IsUser = ddlTypeUser.SelectedValue
                    p.IsActivated = chkIsActivated.Checked
                    data.SaveChanges()
                    'get list role of user in userrole table
                    Dim ur = (From q In data.UserRoles Where q.UserId = hidID.Value Select q.RoleId).ToArray
                    'update role of user (insert or delete role of user)
                    Dim userRole As New UserRole
                    For Each role In chklstRole.Items
                        Dim value = CType(role.Value, Integer)
                        Dim Existed = (From q In ur Where q = value Select q).ToList
                        If role.Selected = True Then
                            'if role doesnot exist in db but it was checked => insert role
                            If Existed.Count <= 0 Then
                                userRole = New UserRole
                                userRole.UserId = hidID.Value
                                userRole.RoleId = value
                                data.UserRoles.AddObject(userRole)
                                data.SaveChanges()
                            End If
                        Else
                            'if role exist in db but it was unchecked => delete role
                            If Existed.Count >= 1 Then
                                Dim usr = (From q In data.UserRoles Where q.UserId = hidID.Value AndAlso q.RoleId = value Select q).SingleOrDefault
                                data.UserRoles.DeleteObject(usr)
                                data.SaveChanges()
                            End If
                        End If
                    Next
                    Insert_App_Log("Edit User:" & txtUsername.Text.Trim() & "", Function_Name.User, Audit_Type.Edit, Request.ServerVariables("REMOTE_ADDR"), Session("UserName"))
                    Excute_Javascript("Alertbox('Cập nhật dữ liệu thành công.');window.location ='../../Page/Users/List.aspx';", Me.Page, True)
                Catch ex As Exception
                    log4net.Config.XmlConfigurator.Configure()
                    log.Error("Error error " & AddTabSpace(1) & Session("Username") & AddTabSpace(1) & "IP:" & GetIPAddress(), ex)
                    Excute_Javascript("Alertbox('Cập nhật thất bại.')';", Me.Page, True)
                End Try
            End Using

        Else

            Excute_Javascript("Alertbox('Tên đăng nhập đã tồn tại trong hệ thống.');", Me.Page, True)
        End If


    End Sub

    Protected Function checkExistUsername() As Boolean
        If txtUsername.Text.Trim() = curUserName.Value Then
            Return False
        End If

        Using data As New ThanhTraLaoDongEntities
            Dim k As User = (From q In data.Users Where q.UserName = txtUsername.Text.Trim()).SingleOrDefault
            If Not k Is Nothing Then
                Return True
            End If
        End Using

        Return False
    End Function

#End Region

End Class
