Imports System.Data
Imports ThanhTraLaoDongModel
Imports Cls_Common
Imports SecurityService

Partial Class Control_Users_Register
    Inherits System.Web.UI.UserControl
    Private Shared ReadOnly log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

#Region "Sub and Function "

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not IsPostBack Then
            Dim script As ScriptManager = ScriptManager.GetCurrent(Me.Page)
            If Not script Is Nothing AndAlso script.IsInAsyncPostBack Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.Page.GetType, "TTLDjs", "ajaxJquery()", True)
            Else
                Me.Page.ClientScript.RegisterStartupScript(Me.Page.GetType(), "TTLDjs", String.Concat("Sys.Application.add_load(function(){", "ajaxJquery()", "});"), True)
            End If
            Page.Title = "Đăng ký tài khoản"
            LoadType()
            LoadData()
            'getMenu()
        End If

    End Sub
    Protected Sub LoadType()
        Using Data As New ThanhTraLaoDongEntities
            Dim lstLinhVuc = (From q In Data.LoaiHinhSanXuats Where q.ParentID = 0
               Order By q.Title
               Select New With {.Value = q.LoaiHinhSXId, .Text = q.Title})

            For Each a In lstLinhVuc
                Dim itm As New ListItem(a.Text, a.Value)
                'insert cap 2
                ddlLinhVuc.Items.Add(itm)
                Dim lstLinhVucSecond = (From q In Data.LoaiHinhSanXuats Where q.ParentID = a.Value
                      Order By q.Title
                      Select New With {.Value = q.LoaiHinhSXId, .Text = q.Title})

                For Each b In lstLinhVucSecond
                    Dim itmSecond As New ListItem(" --- " & b.Text, b.Value)
                    ddlLinhVuc.Items.Add(itmSecond)
                Next
            Next
            ddlLinhVuc.Items.Insert(0, New ListItem(Cls_Common.Str_Opt_Chon, -1))
        End Using
    End Sub
    Protected Sub LoadData()
        Using data As New ThanhTraLaoDongEntities
            'Tham số xác định tỉnh thành
            Dim tinhThamSo = (From a In data.SYS_PARAMETERS Where a.Name.Equals("TinhThanhTraSo") Select a.Val).SingleOrDefault

            '' Load thông tin Tỉnh theo User login tại đây
            Dim lstTinh = Nothing
            If tinhThamSo = 0 Then
                lstTinh = (From q In data.Tinhs
                       Order By q.TenTinh
                       Select New With {.Value = q.TinhId, .Text = q.TenTinh})
                With ddlTinh
                    .Items.Clear()
                    .AppendDataBoundItems() = True
                    .DataTextField = "Text"
                    .DataValueField = "Value"
                    .DataSource = lstTinh
                    .DataBind()
                    .Items.Insert(0, New ListItem(Cls_Common.Str_Opt_Chon, "0"))
                End With
                '' Load thông tin mặc định cho Huyện
                With ddlHuyen
                    .Items.Clear()
                    .AppendDataBoundItems() = True
                    .Items.Insert(0, New ListItem(Cls_Common.Str_Opt_Chon, "0"))
                End With
            Else
                lstTinh = (From q In data.Tinhs Where q.TinhId = tinhThamSo
                       Order By q.TenTinh
                       Select New With {.Value = q.TinhId, .Text = q.TenTinh})
                With ddlTinh
                    .Items.Clear()
                    .AppendDataBoundItems() = True
                    .DataTextField = "Text"
                    .DataValueField = "Value"
                    .DataSource = lstTinh
                    .DataBind()
                End With
                Dim lstHuyen = (From a In data.Huyens
                                Where a.TinhId = tinhThamSo
                                Order By a.TenHuyen
                                Select New With {.Text = a.TenHuyen, .Value = a.HuyenId})
                '' Load thông tin mặc định cho Huyện
                With ddlHuyen
                    .Items.Clear()
                    .AppendDataBoundItems() = True
                    .DataTextField = "Text"
                    .DataValueField = "Value"
                    .DataSource = lstHuyen
                    .DataBind()
                    .Items.Insert(0, New ListItem(Cls_Common.Str_Opt_Chon, "0"))
                End With
            End If

            ' Load thông tin Loại hình Doanh Nghiệp
            Dim lstLHDN = (From q In data.LoaiHinhDoanhNghieps
                        Select New With {.Value = q.LoaiHinhDNId, .Text = q.TenLoaiHinhDN})

            With ddlLoaiHinhDN
                .Items.Clear()
                .AppendDataBoundItems() = True
                .DataTextField = "Text"
                .DataValueField = "Value"
                .DataSource = lstLHDN
                .DataBind()
                .Items.Insert(0, New ListItem(Cls_Common.Str_Opt_Chon, "0"))
            End With

            '' Clear hết Data những biến đã tạo ra 
            lstTinh = Nothing
            lstLHDN = Nothing

        End Using
    End Sub

    Protected Sub getMenu()
        'ddlLoaiHinhSX.Items.Clear()
        'Using data As New ThanhTraLaoDongEntities
        '    Dim p As List(Of LoaiHinhSanXuat) = (From q In data.LoaiHinhSanXuats Select q).ToList
        '    ddlLoaiHinhSX.DataValueField = "LoaiHinhSXId"
        '    ddlLoaiHinhSX.DataTextField = "Title"

        '    RecursiveFillTree(p, 0)
        '    ddlLoaiHinhSX.Items.Insert(0, New ListItem("---Chọn---", "0"))
        'End Using
    End Sub

    Dim level As Integer = 0
    Private Sub RecursiveFillTree(ByVal dtParent As List(Of LoaiHinhSanXuat), ByVal parentID As Integer)
        'level += 1
        ''on the each call level increment 1
        'Dim appender As New StringBuilder()

        'For j As Integer = 0 To level - 1

        '    appender.Append("&nbsp;&nbsp;&nbsp;&nbsp;")
        'Next
        'If level > 0 Then
        '    appender.Append("|__")
        'End If

        'Using data As New ThanhTraLaoDongEntities
        '    Dim dv As List(Of LoaiHinhSanXuat) = (From q In data.LoaiHinhSanXuats
        '                                            Where q.ParentID = parentID Select q).ToList
        '    Dim i As Integer

        '    If dv.Count > 0 Then
        '        For i = 0 To dv.Count - 1
        '            Dim itm As New ListItem(Server.HtmlDecode(appender.ToString() + dv.Item(i).Title.ToString()), dv.Item(i).LoaiHinhSXId.ToString())


        '            ddlLoaiHinhSX.Items.Add(itm)
        '            RecursiveFillTree(dtParent, Integer.Parse(dv.Item(i).LoaiHinhSXId.ToString()))
        '        Next
        '    End If
        'End Using
        'level -= 1
        'on the each function end level will decrement by 1
    End Sub

    Protected Function Save() As Boolean
        Using data As New ThanhTraLaoDongEntities
            Dim p As New DoanhNghiep
            Try
                Dim iLanThayDoi, iSoCN, iSoNguoiLVDH, iSoLaoDongNu As Integer

                p.TenDoanhNghiep = txtTendoanhnghiep.Text.Trim()
                p.DienThoai = txtCodeDienThoaiDN.Text.Trim()
                p.Fax = txtFax.Text.Trim()
                p.NamTLDN = txtCodeNamtldn.Text.Trim()
                p.LoaiHinhDNId = ddlLoaiHinhDN.SelectedValue

                p.TruSoChinh = txtTrusochinh.Text.Trim()
                p.HuyenId = ddlHuyen.SelectedValue
                p.TinhId = ddlTinh.SelectedValue

                p.Url = txtUrl.Text.Trim()
                p.Email = txtEmailDN.Text.Trim()
                If Not ddlLinhVuc.SelectedValue.Equals("-1") Then
                    p.LoaiHinhSXId = ddlLinhVuc.SelectedValue
                End If

                p.LanThayDoi = iLanThayDoi
                p.SoChiNhanh = iSoCN
                p.SoNguoiLamNgheNguyHiem = iSoNguoiLVDH
                p.SoLaoDongNu = iSoLaoDongNu

                p.NguoiTao = txtUserNameReg.Text.Trim
                p.NgayTao = Date.Now
                data.DoanhNghieps.AddObject(p)
                data.SaveChanges()
                'Insert_App_Log("Insert  Doanhnghiep:" & txtTendoanhnghiep.Text.Trim() & "", Function_Name.DoanhNghiep, Audit_Type.Create, Request.ServerVariables("REMOTE_ADDR"), Session("UserName"))

                Return True
            Catch ex As Exception
                log4net.Config.XmlConfigurator.Configure()
                log.Error("Error error " & AddTabSpace(1) & Session("Username") & AddTabSpace(1) & "IP:" & GetIPAddress(), ex)
                Return False
            End Try
        End Using
    End Function

    Protected Sub ResetControl()

    End Sub

#End Region

#Region "Event for control "

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Response.Redirect("Login.aspx")
    End Sub

    Protected Sub btnRegister_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRegister.Click
        If CaptchaControl1.Page.IsValid Then
            Using data As New ThanhTraLaoDongEntities
                Dim p As New User
                'check user exist
                Dim check = (From qr In data.Users Where qr.UserName = txtUserNameReg.Text Select qr.UserName).FirstOrDefault
                If check Is Nothing Then
                    Try
                        p.UserName = txtUserNameReg.Text.Trim()
                        p.Password = Cls_Common.Encrypt(txtPass.Text.Trim())
                        p.Email = txtEmail.Text.Trim()
                        p.FullName = txtHoTen.Text.Trim()
                        p.IsUser = 2
                        p.Created = Date.Now
                        p.IsActivated = False
                        p.DienThoai = txtCodeDienThoai.Text.Trim
                        p.TinhTP = ddlTinh.SelectedValue
                        data.Users.AddObject(p)
                        data.SaveChanges()
                        Insert_App_Log("Register User:" & txtUserNameReg.Text.Trim() & "", Function_Name.User, Audit_Type.Create, Request.ServerVariables("REMOTE_ADDR"), Session("UserName"))

                        Save()
                        Excute_Javascript("AlertboxRedirect('Đăng ký người dùng thành công. Hãy đợi để được kích hoạt tài khoản.','Login.aspx');", Me.Page, True)


                    Catch ex As Exception
                        log4net.Config.XmlConfigurator.Configure()
                        log.Error("Error error " & AddTabSpace(1) & Session("Username") & AddTabSpace(1) & "IP:" & GetIPAddress(), ex)
                        Excute_Javascript("Alertbox('Thêm người dùng thất bại. Hãy liên lạc với người quản trị để được hỗ trợ');", Me.Page, True)
                    End Try
                Else
                    Excute_Javascript("Alertbox('Người dùng đã tồn tại trong hệ thống');", Me.Page, True)
                End If

            End Using
        Else
            Excute_Javascript("Alertbox('Mã kiểm tra chưa chính xác.');", Me.Page, True)
        End If
    End Sub

    Protected Sub ddlTinh_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlTinh.SelectedIndexChanged
        Dim tinhID As Integer = ddlTinh.SelectedValue
        ddlHuyen.Items.Clear()
        Using data As New ThanhTraLaoDongEntities
            Dim lstHuyen = (From q In data.Huyens
                        Where q.TinhId = tinhID
                        Order By q.TenHuyen
                        Select New With {.Value = q.HuyenId, .Text = q.TenHuyen}).ToList()

            With ddlHuyen
                .AppendDataBoundItems() = True
                .DataTextField = "Text"
                .DataValueField = "Value"
                .DataSource = lstHuyen
                .DataBind()
                .Items.Insert(0, New ListItem(Cls_Common.Str_Opt_Chon, 0))
            End With
            lstHuyen = Nothing
        End Using
        txtRePass.Attributes.Add("value", txtRePass.Text)
        txtPass.Attributes.Add("value", txtPass.Text)

    End Sub

    Protected Sub btnLogin_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnLogin.Click
        If chkRememberMe.Checked Then
            If (Request.Browser.Cookies) Then
                'Check if the cookie with name SITELOGIN exist on user's machine
                If (Request.Cookies("SITELOGIN") Is Nothing) Then
                    'Create a cookie with expiry of 30 days                    
                    Response.Cookies("SITELOGIN").Expires = DateTime.Now.AddDays(30)
                    Response.Cookies("SITELOGIN").Item("USERNAME") = txtUserNameLogin.Text
                Else
                    Response.Cookies("SITELOGIN").Item("USERNAME") = txtUserNameLogin.Text
                End If
            End If
        End If

        'Login here
        'set session

        Using data As New ThanhTraLaoDongEntities
            Try
                Dim strEncryptPass As String = Encrypt(txtPassowrd.Text)
                'Dim k = From q In data.Users Select q
                'Dim sql = CType(k, System.Data.Objects.ObjectQuery).ToTraceString

                Dim p As User = (From q In data.Users Where q.UserName = txtUserNameLogin.Text And q.Password = strEncryptPass).SingleOrDefault
                If (Not p Is Nothing) AndAlso (p.IsActivated = True) Then
                    Session("UserName") = txtUserNameLogin.Text
                    Session("UserId") = p.UserId
                    'Session("IsThanhTraBo") = p.IsThanhTraBo
                    'Dim intUserId As Integer = p.UserId
                    p.LastLogin = Now
                    data.SaveChanges()
                    'Lay menu tu UserMenu
                    'Dim arrMenu() As Integer = (From a In data.UsersMenus Where a.UserId = p.UserId Select a.MenuId).ToArray
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

                        'Else
                        '    Excute_Javascript("Alertbox('Bạn chưa được cấp quyền quản trị.');", Me.Page, True)
                        '    Exit Sub
                    End If
                    Dim retURL As String = IIf(Request("retURL") Is Nothing, "Page/Logged.aspx", Request("retURL"))
                    Response.Redirect(retURL)
                Else
                    Excute_Javascript("Alertbox('Tên đăng nhập hoặc mật khẩu chưa chính xác, hoặc tài khoản chưa được kích hoạt.');", Me.Page, True)
                End If

            Catch ex As Exception
                log4net.Config.XmlConfigurator.Configure()
                log.Error("Error error " & AddTabSpace(1) & Session("Username") & AddTabSpace(1) & "IP:" & GetIPAddress(), ex)
            End Try
        End Using
    End Sub

    'Protected Sub hplForgotpassword_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles hplForgotpassword.Init
    '    hplForgotpassword.NavigateUrl = "~/Forgotpassword.aspx?keepThis=true&TB_iframe=true&height=250&width=250&modal=true"
    'End Sub

#End Region

End Class
