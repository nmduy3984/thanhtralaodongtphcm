Imports ThanhTraLaoDongModel
Imports System.Object
Imports Cls_Common
Imports System.IO
Partial Class Page_ChangePassword
    Inherits System.Web.UI.Page
    Private Shared ReadOnly log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Using Data As New ThanhTraLaoDongEntities

            Dim strEncryptPass As String = Encrypt(txtOldPassword.Text)
            Dim userName As String = Session("Username")
            Dim p = (From q In Data.Users Select q Where q.UserName = userName And q.Password = strEncryptPass).FirstOrDefault
            If p Is Nothing Then
                Excute_Javascript("alert(""Mật khẩu cũ không chính xác"")", Me.Page)
                Exit Sub
            Else
                Try
                    p.Password = Encrypt(txtNewPassword.Text)
                    Data.SaveChanges()
                    Excute_Javascript("alert(""Thay đổi mật khẩu thành công"");self.parent.tb_remove();", Me.Page)

                Catch ex As Exception
                    log4net.Config.XmlConfigurator.Configure()
                    log.Error("Error error " & AddTabSpace(1) & Session("Username") & AddTabSpace(1) & "IP:" & GetIPAddress(), ex)
                    Excute_Javascript("alert(""Thay đổi mật khẩu thất bại, hãy kiểm tra lại"")", Me.Page)
                End Try

            End If

        End Using
    End Sub
End Class
