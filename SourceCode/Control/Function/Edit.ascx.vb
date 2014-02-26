Imports System.Data
Imports ThanhTraLaoDongModel
Imports Cls_Common
Imports SecurityService
Partial Class Control_Function_Edit
    Inherits System.Web.UI.UserControl
    Private Shared ReadOnly log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
#Region "Sub and Function "
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Dim script As ScriptManager = ScriptManager.GetCurrent(Me.Page)
            If Not script Is Nothing AndAlso script.IsInAsyncPostBack Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.Page.GetType, "duyjs", "ajaxJquery()", True)
                ScriptManager.RegisterStartupScript(Me.Page, Me.Page.GetType, "duyjs1", "ajaxJqueryToolTip()", True)
            Else
                Me.Page.ClientScript.RegisterStartupScript(Me.Page.GetType(), "duyjs", String.Concat("Sys.Application.add_load(function(){", "ajaxJquery()", "});"), True)
                Me.Page.ClientScript.RegisterStartupScript(Me.Page.GetType(), "duyjs1", String.Concat("Sys.Application.add_load(function(){", "ajaxJqueryToolTip()", "});"), True)
            End If
            If Session("userid") = "2" Then
                Response.Redirect("../../Login.aspx")
            Else
                If Not Request.QueryString("FuncId").ToString.Equals("0") Then
                    hidID.Value = Request.QueryString("FuncId")
                    LoadFunction()
                    ShowData()

                End If
            End If
        End If
    End Sub
    Private Sub LoadFunction()
        Using data As New ThanhTraLaoDongEntities
            Dim func = (From a In data.Functions Where a.IsMenu = True Select a.FunctionId, a.FunctionName).ToList()
            With ddlParentId
                .SelectedIndex = -1
                .SelectedValue = Nothing
                .ClearSelection()
                .DataTextField = "FunctionName"
                .DataValueField = "FunctionId"
                .DataSource = func
                .DataBind()
            End With
            Dim item As New ListItem("--- Chọn ---", 0)
            ddlParentId.Items.Insert(0, item)
        End Using
    End Sub
    Protected Sub ShowData()
        Using data As New ThanhTraLaoDongEntities
            Dim p As [Function] = (From q In data.Functions Where q.FunctionId = hidID.Value Select q).SingleOrDefault
            If Not p Is Nothing Then
                txtFunctionName.Text = IIf(IsNothing(p.FunctionName) = True, "", p.FunctionName)
                txtUrl.Text = IIf(IsNothing(p.URL) = True, "", p.URL)
                txtHrefName.Text = IIf(IsNothing(p.HrefName) = True, "", p.HrefName)
                txtSort.Text = IIf(IsNothing(p.Sort) = True, "", p.Sort)
                chkIsMenu.Checked = IIf(IsNothing(p.IsMenu) = True, "", p.IsMenu)
                ddlParentId.SelectedValue = p.ParentId
            End If
        End Using
    End Sub
     

#End Region

#Region "Event for control"

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
         
            Using data As New ThanhTraLaoDongEntities
            Dim p As [Function] = (From q In data.Functions Where q.FunctionId = hidID.Value).SingleOrDefault
                Try

                p.FunctionName = txtFunctionName.Text.Trim()
                p.URL = txtUrl.Text.Trim()
                p.HrefName = txtHrefName.Text.Trim()
                p.IsMenu = chkIsMenu.Checked
                p.Sort = txtSort.Text.Trim()
                p.ParentId = ddlParentId.SelectedValue
                p.NgaySua = Now()
                p.NguoiSua = Session("UserName")
                    data.SaveChanges()
                Insert_App_Log("Update Function:" & p.HrefName & "", Function_Name.FunctionSys, Audit_Type.Edit, Request.ServerVariables("REMOTE_ADDR"), Session("UserName"))
                Excute_Javascript("Alertbox('Cập nhật dữ liệu thành công.');window.location ='../../Page/Function/List.aspx';", Me.Page, True)
                Catch ex As Exception
                    log4net.Config.XmlConfigurator.Configure()
                    log.Error("Error error " & AddTabSpace(1) & Session("Username") & AddTabSpace(1) & "IP:" & GetIPAddress(), ex)
                    Excute_Javascript("Alertbox('Cập nhật thất bại.');", Me.Page, True)
                End Try
            End Using
       
    End Sub
    Protected Sub btnHuy_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnHuy.Click
        Response.Redirect("List.aspx")
    End Sub
#End Region
End Class
