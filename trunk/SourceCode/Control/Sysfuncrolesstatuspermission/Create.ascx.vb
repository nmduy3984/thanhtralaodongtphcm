Imports System.Data
Imports ThanhTraLaoDongModel
Imports Cls_Common
Imports SecurityService

Partial Class Control_Sysfuncrolesstatuspermission_Create
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
            getStatus()
            load_dataSource()
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
    Private Sub load_dataSource()
        Using Data As New ThanhTraLaoDongEntities
            Try
                'Load dropdowlist Role
                Dim _role = (From q In Data.Roles Order By q.Title Ascending Select q.RoleId, q.Title).ToList
                ddlRoleid.DataSource = _role
                ddlRoleid.DataTextField = "Title"
                ddlRoleid.DataValueField = "RoleID"
                ddlRoleid.DataBind()
                Dim lstItem As New ListItem("--- Chọn ---", "")
                ddlRoleid.Items.Insert(0, lstItem)
                'Load Dropdownlist sysfunction
                Dim _sysFunction = (From q In Data.Functions Order By q.HrefName Ascending Select q.FunctionId, q.FunctionName).ToList
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
    Function CheckExistsRolePermission(ByVal intFunction As Integer, ByVal intRole As Integer) As Boolean
        Using Data As New ThanhTraLaoDongEntities
            Dim p As List(Of SysFuncRolesStatusPermission)
            If hidStatus.Value = 1 Then
                p = (From q In Data.SysFuncRolesStatusPermissions Where q.FunctionId = intFunction And q.RoleId = intRole And q.StatusId = drlStatus.SelectedValue Select q).ToList()
            Else
                p = (From q In Data.SysFuncRolesStatusPermissions Where q.FunctionId = intFunction And q.RoleId = intRole Select q).ToList()
            End If
            If p.Count > 0 Then
                Return True
            End If
            Return False
        End Using
    End Function

#End Region
#Region "Event for control "
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
            If CheckExistsRolePermission(ddlFunctionID.SelectedValue, ddlRoleid.SelectedValue) = True Then
                Excute_Javascript("Alertbox('Vai trò chức năng này đã tồn tại, vui lòng chọn vai trò chức năng khác.');", Me.Page, True)
            Else
                'save data to sysfuncrolesstatuspermission table
                'neu function ko co status 
                If hidStatus.Value = 1 Then
                    SetPermission(CInt(ddlFunctionID.SelectedValue), CInt(ddlRoleid.SelectedValue), CInt(drlStatus.SelectedValue), _auditNumber)
                    Insert_App_Log("Create Sysfuncrolesstatuspermission:" & ddlFunctionID.Text & " - " & ddlRoleid.Text & " - " & drlStatus.Text & _auditNumber & "", Function_Name.SysFuncRolesStatusPermission, Audit_Type.Create, Request.ServerVariables("REMOTE_ADDR"), Session("UserName"))
                Else
                    SetPermission(CInt(ddlFunctionID.SelectedValue), CInt(ddlRoleid.SelectedValue), 0, _auditNumber)
                    Insert_App_Log("Create Sysfuncrolesstatuspermission:" & ddlFunctionID.Text & " - " & ddlRoleid.Text & _auditNumber & "", Function_Name.SysFuncRolesStatusPermission, Audit_Type.Create, Request.ServerVariables("REMOTE_ADDR"), Session("UserName"))
                End If

                Excute_Javascript("Alertbox('Cập nhật dữ liệu thành công.');window.location ='../../Page/Sysfuncrolesstatuspermission/List.aspx';", Me.Page, True)
            End If
        Catch ex As Exception
            log4net.Config.XmlConfigurator.Configure()
            log.Error("Error error " & AddTabSpace(1) & Session("Username") & AddTabSpace(1) & "IP:" & GetIPAddress(), ex)
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
