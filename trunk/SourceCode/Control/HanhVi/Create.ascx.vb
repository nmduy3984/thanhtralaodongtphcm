Imports System.Data
Imports ThanhTraLaoDongModel
Imports Cls_Common
Partial Class Control_HanhVi_Create
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
            BindAccountType()

        End If
    End Sub
    Private Sub BindAccountType()
        'Bind AccountType
        Using Data As New ThanhTraLaoDongEntities
            Dim p = (From q In Data.vLoaiHanhVis Select q).ToList
            With ddlLoaihanhvi
                .DataValueField = "ID"
                .DataTextField = "Name"
                .DataSource = p
                .DataBind()
            End With
        End Using
        Dim lstItem As New ListItem("--- Chọn ---", "")
        ddlLoaihanhvi.Items.Insert(0, lstItem)
    End Sub
    Protected Function Save() As Boolean
        Using data As New thanhtralaodongEntities
            Dim p As New thanhtralaodongModel.Danhmuchanhvi
            Try
                p.Title = txtMota.Text.Trim()
                p.MucPhatMin = txtMucphatmin.Text.Trim()
                p.MucPhatMax = txtMucphatmax.Text.Trim()
                p.LoaiHanhVi = ddlLoaihanhvi.SelectedValue
                Data.Danhmuchanhvis.AddObject(p)
                Data.SaveChanges()
                'Insert_App_Log("Insert  Danhmuchanhvi:" & txtTitle.Text.Trim & "", Function_Name.Danhmuchanhvi, Audit_Type.Create, Request.ServerVariables("REMOTE_ADDR"), Session("UserName"))
                Return True
            Catch ex As Exception
                log4net.Config.XmlConfigurator.Configure()
                log.Error("Error error " & AddTabSpace(1) & Session("Username") & AddTabSpace(1) & "IP:" & GetIPAddress(), ex)
                Return False
            End Try
        End Using
    End Function
    Protected Sub ResetControl()
        txtMota.Text = ""
        txtMucphatmin.Text = ""
        txtMucphatmax.Text = ""
        ddlLoaihanhvi.SelectedValue = ""
    End Sub
#End Region
#Region "Event for control "
    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If Save() Then
            Excute_Javascript("AlertboxRedirect('Cập nhật dữ liệu thành công.','../../Page/HanhVi/List.aspx');", Me.Page, True)
        Else
            Excute_Javascript("Alertbox('Cập nhật thất bại.');", Me.Page, True)
        End If
    End Sub
    Protected Sub btnSaveAndNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveAndNew.Click
        If Save() Then
            Excute_Javascript("Alertbox('Cập nhật dữ liệu thành công.');", Me.Page, True)
            ResetControl()
        Else
            Excute_Javascript("Alertbox('Cập nhật thất bại.');", Me.Page, True)
        End If
    End Sub
    Protected Sub btnReset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReset.Click
        ResetControl()
    End Sub
    Protected Sub btnBack_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnBack.Click
        Response.Redirect("List.aspx")
    End Sub
#End Region
End Class
