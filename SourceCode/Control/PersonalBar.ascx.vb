Imports System.Data
Imports ThanhTraLaoDongModel
Imports Cls_Common
Partial Class Control_PersonalBar
    Inherits System.Web.UI.UserControl

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            
            If Session("UserName") Is Nothing Then
                Dim retURL = Request.RawUrl
                Response.Redirect("~/Login.aspx?retURL=" & retURL)
            Else
                'If checkAccessRight() <> 1 Then
                '    '   Excute_Javascript("Alertbox('Bạn không đủ quyền truy cập mục này.');window.location.href='" & Request.Url.ToString.Split("Page")(0) & "Page/Invalid.aspx" & "'", Me.Page, True)
                '    Excute_Javascript("Alertbox('Bạn không đủ quyền truy cập mục này.');window.location ='" & Request.Url.ToString.Split("Page")(0) & "Page/Invalid.aspx" & "';", Me.Page, True)

                'End If
            End If
        End If
    End Sub
          
    Private Function checkAccessRight() As Integer

        Dim arrFuntionId As Integer() = Session("Function")
        Dim sufixURL = Request.Url.ToString.Split("Page")(0)
        Dim len = Request.Url.ToString.Length()
        Dim found = Request.Url.ToString.IndexOf("Page")

        Dim PureURL = Request.Url.ToString.Substring(found, len - found)
        'bo qua check quyen trang thong bao loi
        If PureURL = "Page/Invalid.aspx" Then
            Return 1
        End If

        PureURL = PureURL.Substring(0, PureURL.LastIndexOf("/") + 1)
  

        Using data As New ThanhTraLaoDongEntities
            'get FunctionId
            Dim _FunctionId As Integer = (From q In data.Functions Where q.URL.Contains(PureURL) Select q.FunctionId).FirstOrDefault
            Dim _UserId As Integer = Session("UserId")
            Dim k = (From q In data.UserRoles Where q.RoleId = _FunctionId And q.UserId = _UserId Select q).FirstOrDefault
            If Not k Is Nothing Then
                Return 1
            End If
        End Using
        Return 0

    End Function

    Protected Sub lbtnExit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbtnExit.Click
        Session.RemoveAll()
        Response.Redirect("~/Login.aspx")
    End Sub
    Protected Sub hplChangePassword_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles hplChangePassword.Init

        hplChangePassword.NavigateUrl = "~/Page/ChangePassword.aspx?keepThis=true&TB_iframe=true&height=215&width=210&modal=true"
    End Sub


End Class
