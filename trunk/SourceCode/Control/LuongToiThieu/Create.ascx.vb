Imports System.Data
Imports ThanhTraLaoDongModel
Imports Cls_Common
Partial Class Control_LuongToiThieu_Create
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
            Using data As New ThanhTraLaoDongEntities
                Dim lstTinh = (From q In data.Tinhs
                            Order By q.TenTinh
                            Select New With {.Value = q.TinhId, .Text = q.TenTinh})
                With ddlTinh
                    .Items.Clear()
                    .AppendDataBoundItems() = True
                    .DataTextField = "Text"
                    .DataValueField = "Value"
                    .DataSource = lstTinh
                    .DataBind()
                    .Items.Insert(0, New ListItem(Cls_Common.Str_Opt_Chon, 0))
                End With
                '' Load thông tin mặc định cho Huyện
                With ddlHuyen
                    .Items.Clear()
                    .AppendDataBoundItems() = True
                    .Items.Insert(0, New ListItem(Cls_Common.Str_Opt_Chon, 0))
                End With

            End Using
        End If
    End Sub
    
    Protected Function Save() As Boolean
        Using data As New ThanhTraLaoDongEntities
            Dim p As New ThanhTraLaoDongModel.LuongToiThieu
            Try
                p.TieuDe = txtMota.Text.Trim()
                p.MucLuongToiThieu = txtMucLuongToiThieu.Text.Trim()
                p.IsNhaNuoc = ddlLoainhanuoc.SelectedValue
                p.QuyetDinh = IIf(String.IsNullOrEmpty(txtQuyetDinh.Text), Nothing, txtQuyetDinh.Text)
                p.NgayQuyetDinh = IIf(IsNothing(txtNgayQuyetDinh.Text), Nothing, StringToDate(txtNgayQuyetDinh.Text))
                data.LuongToiThieux.AddObject(p)
                data.SaveChanges()
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
        txtMucLuongToiThieu.Text = ""
        ddlLoainhanuoc.SelectedValue = ""
        txtQuyetDinh.Text = ""
        txtNgayQuyetDinh.Text = ""
    End Sub
#End Region
#Region "Event for control "
    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If Save() Then
            Excute_Javascript("AlertboxRedirect('Cập nhật dữ liệu thành công.','../../Page/LuongToiThieu/List.aspx');", Me.Page, True)
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

    Protected Sub ddlTinh_SelectedIndexChanged1(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlTinh.SelectedIndexChanged
        Dim tinhID As Integer = ddlTinh.SelectedValue
        ddlHuyen.Items.Clear()
        Using data As New ThanhTraLaoDongEntities
            Dim lstHuyen = (From q In data.Huyens
                        Where q.TinhId = tinhID
                        Order By q.TenHuyen
                        Select New With {.Value = q.HuyenId, .Text = q.TenHuyen}).ToList()

            With ddlHuyen
                .AppendDataBoundItems() = True
                .DataTextField = "Text"
                .DataValueField = "Value"
                .DataSource = lstHuyen
                .DataBind()
                .Items.Insert(0, New ListItem(Cls_Common.Str_Opt_Chon, 0))
            End With
            lstHuyen = Nothing
        End Using
    End Sub
End Class
