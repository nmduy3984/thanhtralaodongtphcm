Imports System.Data
Imports ThanhTraLaoDongModel
Imports Cls_Common
Imports SecurityService

Partial Class Control_Sysfuncrolesstatuspermission_Edit
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
            If Not Request.QueryString("Functionid") Is Nothing And Not Request.QueryString("Roleid") Is Nothing And Not Request.QueryString("Statusid") Is Nothing Then
                hidID.Value = Request.QueryString("Functionid")
                load_dataSource()
                ShowData()
            End If
        Else

        End If
    End Sub
    Protected Sub getStatus()
        Using data As New ThanhTraLaoDongEntities
            Dim p As List(Of Status) = (From q In data.Status Select q).ToList
            drlStatus.DataValueField = "StatusId"
            drlStatus.DataTextField = "Title"
            drlStatus.DataSource = p
            drlStatus.DataBind()
        End Using
    End Sub
    Protected Sub ShowData()
        Using data As New ThanhTraLaoDongEntities
            Try
                Dim roleID = CInt(Request.QueryString("RoleId").ToString)
                Dim statusID = CInt(Request.QueryString("StatusId").ToString)
                Dim functionID = CInt(Request.QueryString("FunctionId").ToString)
                Dim p As SysFuncRolesStatusPermission = (From q In data.SysFuncRolesStatusPermissions Where q.FunctionId = functionID And q.RoleId = roleID And q.StatusId = statusID Select q).SingleOrDefault

                If Not p Is Nothing Then
                    ddlFunctionID.Items.FindByValue(functionID).Selected = True
                    Dim h = (From q In data.Functions Select q Where q.FunctionId = ddlFunctionID.SelectedValue).FirstOrDefault
                    If Not p Is Nothing Then
                        If h.IsStatus = True Then
                            divStatus.Visible = True
                            hidStatus.Value = 1
                        Else
                            divStatus.Visible = False
                            hidStatus.Value = 0
                        End If
                    End If
                    ddlRoleid.Items.FindByValue(roleID).Selected = True
                    If (statusID > 0) Then
                        drlStatus.Items.FindByValue(statusID).Selected = True
                    Else
                        drlStatus.SelectedIndex = -1
                    End If
                    'Luu lai gia tri Function,Role,Status
                    hidFunction.Value = p.FunctionId
                    hidRole.Value = p.RoleId
                    hidStatusValue.Value = p.StatusId
                    Dim _auditNumber As Integer = p.AuditNumber
                    For Each item In chklstAuditnumber.Items
                        item.Selected = CInt(_auditNumber) And CInt(item.Value)
                    Next
                Else
                    Excute_Javascript("Alertbox('Không tồn tại dữ liệu này.');window.location ='../../Page/Sysfuncrolesstatuspermission/List.aspx';", Me.Page, True)
                End If
            Catch ex As Exception
                log4net.Config.XmlConfigurator.Configure()
                log.Error("Error error " & AddTabSpace(1) & Session("Username") & AddTabSpace(1) & "IP:" & GetIPAddress(), ex)
                Excute_Javascript("Alertbox('Không tồn tại dữ liệu này.');window.location ='../../Page/Sysfuncrolesstatuspermission/List.aspx';", Me.Page, True)
            End Try

        End Using
    End Sub
    Private Sub load_dataSource()
        Using Data As New ThanhTraLaoDongEntities
            Try
                getStatus()
                'Load dropdowlist Role
                Dim _role = (From q In Data.Roles Order By q.Title Ascending Select q.RoleId, q.Title).ToList
                ddlRoleid.DataSource = _role
                ddlRoleid.DataTextField = "Title"
                ddlRoleid.DataValueField = "RoleID"
                ddlRoleid.DataBind()
                Dim lstItem As New ListItem("--- Chọn ---", "")
                ddlRoleid.Items.Insert(0, lstItem)
                'Load Dropdownlist sysfunction
                Dim _sysFunction = (From q In Data.Functions Order By q.FunctionName Ascending Select q.FunctionId, q.FunctionName).ToList
                ddlFunctionID.DataSource = _sysFunction
                ddlFunctionID.DataTextField = "FunctionName"
                ddlFunctionID.DataValueField = "FunctionId"
                ddlFunctionID.DataBind()
                ddlFunctionID.Items.Insert(0, lstItem)
                'Load Dropdownlist AuditType
                'Get Enum AuditType to dictionary
                Dim _auditDictionary As New Dictionary(Of String, Integer)
                ' Add two keys.
                _auditDictionary.Add(Cls_Common.Audit_Type.Create.ToString, Cls_Common.Audit_Type.Create)
                _auditDictionary.Add(Cls_Common.Audit_Type.Delete.ToString, Cls_Common.Audit_Type.Delete)
                _auditDictionary.Add(Cls_Common.Audit_Type.Edit.ToString, Cls_Common.Audit_Type.Edit)
                _auditDictionary.Add(Cls_Common.Audit_Type.ViewContent.ToString, Cls_Common.Audit_Type.ViewContent)
                _auditDictionary.Add(Cls_Common.Audit_Type.Submit.ToString, Cls_Common.Audit_Type.Submit)
                _auditDictionary.Add(Cls_Common.Audit_Type.Publish.ToString, Cls_Common.Audit_Type.Publish)

                'create generic list with FieldName
                Dim x = From q In _auditDictionary Select AuditType = q.Key, AuditValue = q.Value
                chklstAuditnumber.DataSource = x
                chklstAuditnumber.DataTextField = "AuditType"
                chklstAuditnumber.DataValueField = "AuditValue"
                chklstAuditnumber.DataBind()
            Catch ex As Exception
                log4net.Config.XmlConfigurator.Configure()
                log.Error("Error error " & AddTabSpace(1) & Session("Username") & AddTabSpace(1) & "IP:" & GetIPAddress(), ex)
            End Try
        End Using
    End Sub
    'Kiem tra vai tro chuc nang ton tai
    Function CheckExistsRolePermission(ByVal intFunction As Integer, ByVal intRole As Integer) As Boolean
        Using Data As New ThanhTraLaoDongEntities
            If hidFunction.Value <> ddlFunctionID.SelectedValue Or hidRole.Value <> ddlRoleid.SelectedValue Or hidStatusValue.Value <> drlStatus.SelectedValue Then
                Dim p As List(Of SysFuncRolesStatusPermission)
                If hidStatus.Value = 1 Then
                    p = (From q In Data.SysFuncRolesStatusPermissions Where q.FunctionId = intFunction And q.RoleId = intRole And q.StatusId = drlStatus.SelectedValue Select q).ToList()
                Else
                    p = (From q In Data.SysFuncRolesStatusPermissions Where q.FunctionId = intFunction And q.RoleId = intRole Select q).ToList()
                End If
                If p.Count > 0 Then
                    Return True
                End If
            End If
            Return False
        End Using
    End Function

    'Kiem tra vai tro chuc nang ton tai ma khong co status
    Function CheckExistsRolePermissionNoStatus(ByVal intFunction As Integer, ByVal intRole As Integer) As Boolean
        Using Data As New ThanhTraLaoDongEntities
            If hidFunction.Value <> ddlFunctionID.SelectedValue Or hidRole.Value <> ddlRoleid.SelectedValue Then
                Dim p As List(Of SysFuncRolesStatusPermission)
                p = (From q In Data.SysFuncRolesStatusPermissions Where q.FunctionId = intFunction And q.RoleId = intRole Select q).ToList()
                If p.Count > 0 Then
                    Return True
                End If
            End If
            Return False
        End Using
    End Function
#End Region
#Region "Event for control"
    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Try
            Dim _auditNumber As Integer
            Dim _count As Integer = 0
            For Each item In chklstAuditnumber.Items
                If item.Selected Then
                    If _count = 0 Then
                        _auditNumber = CInt(item.Value)
                    Else
                        _auditNumber = CInt(_auditNumber) Or CInt(item.Value)
                    End If
                    _count += 1
                End If
            Next
            'Check data exists
            If hidStatus.Value = 1 Then
                If CheckExistsRolePermission(ddlFunctionID.SelectedValue, ddlRoleid.SelectedValue) = True Then
                    Excute_Javascript("Alertbox('Vai trò chức năng này đã tồn tại, vui lòng chọn vai trò chức năng khác.');", Me.Page, True)
                    Return
                End If
            Else 'Cac chuc nang khong co trang thai
                If CheckExistsRolePermissionNoStatus(ddlFunctionID.SelectedValue, ddlRoleid.SelectedValue) = True Then
                    Excute_Javascript("Alertbox('Vai trò chức năng này đã tồn tại, vui lòng chọn vai trò chức năng khác.');", Me.Page, True)
                    Return
                End If
            End If

            Dim roleID = Request.QueryString("RoleId").ToString
            Dim statusID = Request.QueryString("StatusId").ToString
            Dim functionID = Request.QueryString("FunctionId").ToString
            'save data to sysfuncrolesstatuspermission table
            'neu function ko co status 
            If hidStatus.Value = 1 Then
                UpdatePermission(functionID, roleID, statusID, CInt(ddlFunctionID.SelectedValue), CInt(ddlRoleid.SelectedValue), CInt(drlStatus.SelectedValue), _auditNumber)
                Insert_App_Log("Edit Sysfuncrolesstatuspermission:" & ddlFunctionID.Text & " - " & ddlRoleid.Text & " - " & drlStatus.Text & " - " & _auditNumber & "", Function_Name.SysFuncRolesStatusPermission, Audit_Type.Edit, Request.ServerVariables("REMOTE_ADDR"), Session("UserName"))
            Else
                UpdatePermission(functionID, roleID, statusID, CInt(ddlFunctionID.SelectedValue), CInt(ddlRoleid.SelectedValue), 0, _auditNumber)
                Insert_App_Log("Edit Sysfuncrolesstatuspermission:" & ddlFunctionID.Text & " - " & ddlRoleid.Text & " - " & _auditNumber & "", Function_Name.SysFuncRolesStatusPermission, Audit_Type.Edit, Request.ServerVariables("REMOTE_ADDR"), Session("UserName"))
            End If

            'save data to sysfuncrolesstatuspermission table
            ' SetPermission(CInt(ddlFunctionID.SelectedValue), CInt(ddlRoleid.SelectedValue), CInt(drlStatus.SelectedValue), _auditNumber)
            Excute_Javascript("Alertbox('Cập nhật dữ liệu thành công.');window.location ='../../Page/Sysfuncrolesstatuspermission/List.aspx';", Me.Page, True)

        Catch ex As Exception
            log4net.Config.XmlConfigurator.Configure()
            log.Error("Error error " & AddTabSpace(1) & Session("Username") & AddTabSpace(1) & "IP:" & GetIPAddress(), ex)
            Excute_Javascript("Alertbox('Cập nhật dữ liệu thất bại.');window.location ='../../Page/Sysfuncrolesstatuspermission/List.aspx';", Me.Page, True)
        End Try
    End Sub
    Protected Sub btnHuy_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnHuy.Click
        Response.Redirect("List.aspx")
    End Sub
#End Region

    Protected Sub ddlFunctionID_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlFunctionID.SelectedIndexChanged
        Using Data As New ThanhTraLaoDongEntities
            Dim p = (From q In Data.Functions Select q Where q.FunctionId = ddlFunctionID.SelectedValue).FirstOrDefault
            If Not p Is Nothing Then
                If p.IsStatus = True Then
                    divStatus.Visible = True
                    hidStatus.Value = 1
                Else
                    divStatus.Visible = False
                    hidStatus.Value = 0
                End If
            End If
        End Using
    End Sub
End Class
