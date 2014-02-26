Imports System.Data
Imports ThanhTraLaoDongModel
Imports Cls_Common
Imports SecurityService

Partial Class Control_Users_Create
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
            load_TreeviewRole()
            BindAccountType()
            txtPassword.Attributes.Add("OnBlur", "CheckValidPass()")
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
    Private Sub load_TreeviewRole()
        Using data As New ThanhTraLaoDongEntities
            Dim p = (From q In data.Roles Order By q.Title Ascending, q.RoleId Descending Select q).ToList
            chklstRole.DataSource = p
            chklstRole.DataTextField = "Title"
            chklstRole.DataValueField = "RoleID"
            chklstRole.DataBind()
        End Using
    End Sub
#End Region
#Region "Event for control "
    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Using data As New ThanhTraLaoDongEntities
            Dim p As New User
            'check user exist
            Dim check = (From qr In data.Users Where qr.UserName = txtUsername.Text Select qr.UserName).FirstOrDefault
            If check Is Nothing Then
                Try
                    p.UserName = txtUsername.Text.Trim()
                    p.Password = Cls_Common.Encrypt(txtPassword.Text.Trim())
                    p.Email = txtEmail.Text.Trim()
                    p.TinhTP = CInt(hidTinhThanhTraSo.Value)
                    p.FullName = txtFullname.Text.Trim()
                    p.IsUser = ddlTypeUser.SelectedValue
                    p.Created = Date.Now
                    p.IsActivated = chkIsActivated.Checked
                    data.Users.AddObject(p)
                    data.SaveChanges()

                    Dim userRole As New UserRole
                    For Each role In chklstRole.Items
                        If role.Selected = True Then
                            userRole = New UserRole
                            userRole.UserId = p.UserId
                            userRole.RoleId = role.Value
                            data.UserRoles.AddObject(userRole)
                            data.SaveChanges()
                        End If
                    Next

                    'Lưu user được quyền thanh tra tỉnh nào
                    If hidTinhThanhTraSo.Value > 0 Then
                        Dim ut As New ThanhTraLaoDongModel.UsersTinh
                        ut.UserId = p.UserId
                        ut.TinhId = hidTinhThanhTraSo.Value
                        data.UsersTinhs.AddObject(ut)
                        data.SaveChanges()
                    End If

                    Insert_App_Log("Create User:" & txtUsername.Text.Trim() & "", Function_Name.User, Audit_Type.Create, Request.ServerVariables("REMOTE_ADDR"), Session("UserName"))
                    Excute_Javascript("Alertbox('Cập nhật dữ liệu thành công.');window.location ='../../Page/Users/List.aspx';", Me.Page, True)
                Catch ex As Exception
                    log4net.Config.XmlConfigurator.Configure()
                    log.Error("Error error " & AddTabSpace(1) & Session("Username") & AddTabSpace(1) & "IP:" & GetIPAddress(), ex)
                    Excute_Javascript("Alertbox('Thêm người dùng thất bại.');window.location ='../../Page/Users/Create.aspx';", Me.Page, True)
                End Try
            Else
                Excute_Javascript("Alertbox('Người dùng đã tồn tại trong hệ thống')", Me.Page, True)
            End If

        End Using
    End Sub
#End Region

    

End Class
