Imports System.Object
Imports ThanhTraLaoDongModel
Imports Cls_Common
Imports System.IO
Partial Class Login
    Inherits System.Web.UI.Page
    Private Shared ReadOnly log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            'Check if the browser support cookies     
            If Request.Browser.Cookies Then
                If Request.Cookies("SITELOGIN") IsNot Nothing Then
                    txtUserName.Text = Request.Cookies("SITELOGIN")("USERNAME")
                    'chkRememberMe.Checked = True
                End If
            End If
        End If
    End Sub
    Protected Sub btnLogin_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnLogin.Click
        If chkRememberMe.Checked Then
            If (Request.Browser.Cookies) Then
                'Check if the cookie with name SITELOGIN exist on user's machine
                If (Request.Cookies("SITELOGIN") Is Nothing) Then
                    'Create a cookie with expiry of 30 days                    
                    Response.Cookies("SITELOGIN").Expires = DateTime.Now.AddDays(30)
                    Response.Cookies("SITELOGIN").Item("USERNAME") = txtUserName.Text
                Else
                    Response.Cookies("SITELOGIN").Item("USERNAME") = txtUserName.Text
                End If
            End If
        End If

        'Login here
        'set session

        Using data As New ThanhTraLaoDongEntities
            Try
                Dim strEncryptPass As String = Encrypt(txtPassword.Text)
                'Dim k = From q In data.Users Select q
                'Dim sql = CType(k, System.Data.Objects.ObjectQuery).ToTraceString

                Dim p As User = (From q In data.Users Where q.UserName = txtUserName.Text And q.Password = strEncryptPass).SingleOrDefault
                If (Not p Is Nothing) AndAlso (p.IsActivated = True) Then
                    Session("UserName") = txtUserName.Text
                    Session("UserId") = p.UserId

                    'Hiển thị banner

                    Dim chkTT = data.uspCheckLoaiThanhTraByUserId(p.UserId).FirstOrDefault
                    Dim userTinh = (From a In data.Tinhs Where a.TinhId = p.TinhTP Select a).FirstOrDefault
                    Session("chkTT") = "1"
                    If IsNothing(chkTT) Then
                        Session("chkTT") = "0"
                        If Not IsNothing(userTinh) Then
                            Session("TinhDayDu") = IIf(IsNothing(userTinh.IsTinh) OrElse Not userTinh.IsTinh, "tỉnh ", "") + userTinh.TenTinh
                            Session("TinhVietTat") = userTinh.KiHieu '+ IIf(IsNothing(userTinh.IsTinh) OrElse Not userTinh.IsTinh, " Province", "")
                        End If
                    End If
                    If Not IsNothing(userTinh) Then
                        Session("InfoContactUser") = "Địa chỉ: " + userTinh.MoTa + " - Điện thoại: " + userTinh.DienThoai
                    Else
                        Session("InfoContactUser") = ""
                    End If
                    'Cập nhật lần đăng nhập cuối của user
                    p.LastLogin = Now
                    'Gán Session cho IsUser
                    Session("IsUser") = p.IsUser
                    Dim arrID() As Integer = (From t In data.UserRoles Where t.UserId = p.UserId Select t.RoleId).ToArray
                    If arrID.Length > 0 Then
                        Session("RoleID") = arrID
                        Dim k = (From q In data.SysFuncRolesStatusPermissions Where arrID.Contains(q.RoleId) Select q.FunctionId).Distinct().ToArray
                        Session("Function") = k
                        If k.Length <= 0 Then
                            Excute_Javascript("Alertbox('Bạn chưa được cấp quyền quản trị.');", Me.Page, True)
                            Exit Sub
                        End If
                    End If
                    'Tham số xác định tỉnh thành
                    Dim tinhThamSo = (From a In data.SYS_PARAMETERS Where a.Name.Equals("TinhThanhTraSo") Select a.Val).SingleOrDefault
                    Session("TinhThanhTraSo") = 0
                    If Not IsNothing(tinhThamSo) Then
                        Session("TinhThanhTraSo") = tinhThamSo
                    End If
                    data.SaveChanges()
                    Dim retURL As String
                    If p.IsUser = UserType.DoanhNghiep Then
                        retURL = "DoanhNghiep/Page/Logged.aspx"
                    Else
                        retURL = IIf(Request("retURL") Is Nothing, "Page/Logged.aspx", Request("retURL"))
                    End If

                    Response.Redirect(retURL, False)
                Else
                    Excute_Javascript("Alertbox('Tên đăng nhập hoặc mật khẩu chưa chính xác, hoặc tài khoản chưa được kích hoạt.');", Me.Page, True)
                End If

            Catch ex As Exception
                log4net.Config.XmlConfigurator.Configure()
                log.Error("Error error " & AddTabSpace(1) & Session("Username") & AddTabSpace(1) & "IP:" & GetIPAddress(), ex)
            End Try
        End Using
    End Sub


End Class
