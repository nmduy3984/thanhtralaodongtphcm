Imports Microsoft.VisualBasic
Imports Cls_Common
Imports ThanhTraLaoDongModel


Public Class SecurityService

    Public Shared Function HasPermission(ByVal FuntionID As Integer, ByVal arrRoleID() As Integer, ByVal StatusID As Integer, ByVal audit As Audit_Type) As Boolean
        
        Using data As New ThanhTraLaoDongEntities
            Dim p As List(Of ThanhTraLaoDongModel.SysFuncRolesStatusPermission) '= (From q In data.SysFuncRolesStatusPermissions).Take(0)
            If StatusID <> 0 Then
                p = (From q In data.SysFuncRolesStatusPermissions Where arrRoleID.Contains(q.RoleId) And (q.FunctionId = FuntionID) And (q.StatusId = StatusID) Select q).ToList
            Else
                p = (From q In data.SysFuncRolesStatusPermissions Where arrRoleID.Contains(q.RoleId) And (q.FunctionId = FuntionID) Select q).ToList
            End If
            If Not p Is Nothing Then
                Dim t As Integer = 0
                For Each i In p
                    t = CInt(t) Or CInt(i.AuditNumber)
                Next
                Return GetPermission(audit, t)
            Else
                Return False
            End If
        End Using

    End Function
    Public Shared Function GetPermission(ByVal audit As Audit_Type, ByVal permission As Integer) As Boolean
        If (CInt(audit And permission) = CInt(audit)) Then
            Return True
        Else
            Return False
        End If
    End Function
    'Dim auditnumber as Integer = CInt(Audit_Type.Create Or Audit_Type.Delete)
    ' SecurityService.SetPermission(Function_Name.Tin_Tuc, 1,1, auditnumber)
    Public Shared Function SetPermission(ByVal functionID As Integer, ByVal RoleID As Integer, ByVal statusID As Integer, ByVal audit_number As Integer) As Boolean
        Using data As New ThanhTraLaoDongEntities
            Try
                Dim h = (From t In data.SysFuncRolesStatusPermissions Where t.FunctionId = functionID And t.RoleId = RoleID And t.StatusId = statusID Select t).FirstOrDefault
                If h Is Nothing Then
                    Dim FuncRolesStatusPermissions As New SysFuncRolesStatusPermission
                    FuncRolesStatusPermissions.FunctionId = functionID
                    FuncRolesStatusPermissions.RoleId = RoleID
                    FuncRolesStatusPermissions.StatusId = statusID
                    FuncRolesStatusPermissions.AuditNumber = audit_number
                    data.SysFuncRolesStatusPermissions.AddObject(FuncRolesStatusPermissions)
                Else
                    h.FunctionId = functionID
                    h.RoleId = RoleID
                    h.StatusId = statusID
                    h.AuditNumber = audit_number
                End If
                data.SaveChanges()
                Return True
            Catch ex As Exception
                Return False
            End Try
        End Using
    End Function

    'Dim auditnumber as Integer = CInt(Audit_Type.Create Or Audit_Type.Delete)
    ' SecurityService.SetPermission(Function_Name.Tin_Tuc, 1,1, auditnumber)
    Public Shared Function UpdatePermission(ByVal OldfunctionID As Integer, ByVal OldRoleID As Integer, ByVal OldstatusID As Integer, ByVal functionID As Integer, ByVal RoleID As Integer, ByVal statusID As Integer, ByVal audit_number As Integer) As Boolean
        Using data As New ThanhTraLaoDongEntities
            Try
                Dim h = (From t In data.SysFuncRolesStatusPermissions Where t.FunctionId = OldfunctionID And t.RoleId = OldRoleID And t.StatusId = OldstatusID Select t).FirstOrDefault
                If Not h Is Nothing Then
                    'Delete object old
                    data.SysFuncRolesStatusPermissions.DeleteObject(h)
                    data.SaveChanges()
                    'Add lai object
                    Dim FuncRolesStatusPermissions As New SysFuncRolesStatusPermission
                    FuncRolesStatusPermissions.FunctionId = functionID
                    FuncRolesStatusPermissions.RoleId = RoleID
                    FuncRolesStatusPermissions.StatusId = statusID
                    FuncRolesStatusPermissions.AuditNumber = audit_number
                    data.SysFuncRolesStatusPermissions.AddObject(FuncRolesStatusPermissions)
                    data.SaveChanges()
                End If
                Return True
            Catch ex As Exception
                Return False
            End Try
        End Using
    End Function

    'Insert_App_Log("Update News Title:" & txtTitle.Text.Trim & "", Function_Name.Tin_Tuc, Audit_Type.Edit, Request.ServerVariables("REMOTE_ADDR"), "Admin")
    Public Shared Sub Insert_App_Log(ByVal Content As String, ByVal FunctionID As Integer, ByVal ActionID As Integer, ByVal client As String, ByVal User_Name As String)
        Using data As New ThanhTraLaoDongEntities
            Try
                Dim p As New Log
                p.Content = Content
                p.FunctionID = FunctionID
                p.EventID = ActionID
                p.ClientIP = client
                p.ActTime = Now
                p.UserName = User_Name
                data.Logs.AddObject(p)
                data.SaveChanges()
            Catch ex As Exception

            End Try
        End Using
    End Sub
    Public Shared Function Check_Selected(ByVal MenuId As Integer) As Boolean
        Dim arr() As Integer = HttpContext.Current.Session("MenuId")
        Dim index As Integer = Array.IndexOf(arr, MenuId)
        If index <> -1 Then
            Return True
        Else
            Return False
        End If
    End Function
End Class
