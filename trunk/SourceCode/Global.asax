<%@ Application Language="VB" %>
<script RunAt="server">

    Sub Application_Start(ByVal sender As Object, ByVal e As EventArgs)
        ' Code that runs on application startup
        Application("TatCa") = 0
        Application("visitors_online") = 0
    End Sub
    
    Sub Application_End(ByVal sender As Object, ByVal e As EventArgs)
        ' Code that runs on application shutdown
        
    End Sub
        
    Sub Application_Error(ByVal sender As Object, ByVal e As EventArgs)
        ' Code that runs when an unhandled error occurs
    End Sub

    Sub Session_Start(ByVal sender As Object, ByVal e As EventArgs)
        ' Code that runs when a new session is started
        Try
            Using data As New ThanhTraLaoDongModel.ThanhTraLaoDongEntities
                Application.Lock()
                Application("visitors_online") = CType(Application("visitors_online"), Long) + 1
                Application.UnLock()
                Dim dtb = data.spThongKe_Edit().ToList()
                If dtb.Count > 0 Then
                    Application("TatCa") = Long.Parse("0" + dtb(0).TatCa).ToString("#,###")
                End If
            End Using
        Catch
        End Try
    End Sub

    Sub Session_End(ByVal sender As Object, ByVal e As EventArgs)
        ' Code that runs when a session ends. 
        ' Note: The Session_End event is raised only when the sessionstate mode
        ' is set to InProc in the Web.config file. If session mode is set to StateServer 
        ' or SQLServer, the event is not raised.
        
        Application.Lock()
        Application("visitors_online") = CType(Application("visitors_online"), Long) - 1
        Application.UnLock()
    End Sub
       
</script>
