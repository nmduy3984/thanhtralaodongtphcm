Imports System.Data
Imports ThanhTraLaoDongModel
Imports Cls_Common
Partial Class Control_HanhVi_Editor
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
            If Not Request.QueryString("Hanhviid").ToString.Equals("0") Then
                hidID.Value = Request.QueryString("Hanhviid")
                ShowData()
                BindAccountType()
            End If
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
    Protected Sub ShowData()
        Using data As New ThanhTraLaoDongEntities
            Dim p As DanhMucHanhVi = (From q In data.DanhMucHanhVis Where q.HanhViId = hidID.Value Select q).SingleOrDefault
            If Not p Is Nothing Then
                txtMota.Text = IIf(IsNothing(p.Title) = True, "", p.Title)
                txtMucphatmin.Text = IIf(IsNothing(p.MucPhatMin) = True, "", String.Format("{0:n0}", p.MucPhatMin))
                txtMucphatmax.Text = IIf(IsNothing(p.MucPhatMax) = True, "", String.Format("{0:n0}", p.MucPhatMax))
                ddlLoaihanhvi.SelectedValue = IIf(IsNothing(p.LoaiHanhVi) = True, "", p.LoaiHanhVi)
            End If
        End Using
    End Sub
#End Region
#Region "Event for control"
    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Using data As New ThanhTraLaoDongEntities
            Dim p As DanhMucHanhVi = (From q In data.DanhMucHanhVis Where q.HanhViId = hidID.Value).SingleOrDefault
            Try
                p.HanhViId = hidID.Value
                p.Title = txtMota.Text.Trim()
                p.MucPhatMin = txtMucphatmin.Text.Trim()
                p.MucPhatMax = txtMucphatmax.Text.Trim()
                p.LoaiHanhVi = ddlLoaihanhvi.SelectedValue
                data.SaveChanges()
                'Insert_App_Log("Insert Danhmuchanhvi:" & txtTitle.Text.Trim & "", Function_Name.Danhmuchanhvi, Audit_Type.Create, Request.ServerVariables("REMOTE_ADDR"), Session("UserName"))
                Excute_Javascript("AlertboxRedirect('Cập nhật dữ liệu thành công.','../../Page/HanhVi/List.aspx');", Me.Page, True)
            Catch ex As Exception
                log4net.Config.XmlConfigurator.Configure()
                log.Error("Error error " & AddTabSpace(1) & Session("Username") & AddTabSpace(1) & "IP:" & GetIPAddress(), ex)
                Excute_Javascript("Alertbox('Cập nhật thất bại.');", Me.Page, True)
            End Try
        End Using
    End Sub
    Protected Sub btnReset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReset.Click
        ShowData()
    End Sub
    Protected Sub btnBack_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnBack.Click
        Response.Redirect("List.aspx")
    End Sub
#End Region
End Class
