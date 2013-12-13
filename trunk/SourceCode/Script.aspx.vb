Imports System.Data
Imports ThanhTraLaoDongModel
Imports Cls_Common
Imports SecurityService
Partial Class Script
    Inherits System.Web.UI.Page

    Protected Sub submit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles submit.Click
        Using data As New ThanhTraLaoDongEntities
            If (txtScript.Text.Trim.Length > 0) Then
                data.ExecuteStoreCommand(txtScript.Text)
            End If
        End Using
    End Sub
End Class
