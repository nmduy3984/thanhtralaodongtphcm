Imports System.Data
Imports ThanhTraLaoDongModel
Imports Cls_Common

Partial Class Control_Sysfuncrolesstatuspermission_Detail
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
        End If
    End Sub
    Private Sub load_dataSource()
        Using Data As New ThanhTraLaoDongEntities
            Try
                'getStatus()
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
    'Protected Sub getStatus()
    '    Using data As New ThanhTraLaoDongEntities
    '        Dim p As List(Of Status) = (From q In data.Status Select q).ToList
    '        drlStatus.DataValueField = "StatusId"
    '        drlStatus.DataTextField = "Title"
    '        drlStatus.DataSource = p
    '        drlStatus.DataBind()
    '    End Using
    'End Sub
    Protected Sub ShowData()
        Using data As New ThanhTraLaoDongEntities
            Dim _roleID = CInt(Request.QueryString("RoleId").ToString)
            Dim _statusID = CInt(Request.QueryString("StatusId").ToString)
            Dim _functionID = CInt(Request.QueryString("FunctionID").ToString)
            Dim p = (From q In data.SysFuncRolesStatusPermissions Where q.FunctionId = _functionID And q.RoleId = _roleID And q.StatusId = _statusID Select RoleId = q.RoleId, FunctionId = q.FunctionId, StatusId = q.StatusId, AuditNumber = q.AuditNumber, RoleName = q.Role.Title, SysFunctionName = q.Function.FunctionName).SingleOrDefault
            If Not p Is Nothing Then
                lblFunctionId.Text = IIf(IsNothing(p.FunctionId) = True, "", p.SysFunctionName)
                lblRoleid.Text = IIf(IsNothing(p.RoleId) = True, "", p.RoleName)
                'drlStatus.Items.FindByValue(IIf(IsNothing(p.StatusId) = True, "", p.StatusId)).Selected = True
                Dim _auditNumber As Integer = p.AuditNumber
                For Each item In chklstAuditnumber.Items
                    item.Selected = CInt(_auditNumber) And CInt(item.Value)
                Next
            End If
        End Using
    End Sub
#End Region
End Class
