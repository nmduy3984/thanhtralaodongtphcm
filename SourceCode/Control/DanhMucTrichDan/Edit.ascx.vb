Imports System.Data
Imports ThanhTraLaoDongModel
Imports Cls_Common
Imports SecurityService
Partial Class Control_DanhMucTrichDan_Edit
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

            If Not Request.QueryString("TrichDanId").ToString.Equals("0") Then
                hidID.Value = Request.QueryString("TrichDanId")
                ShowData()
            End If
        End If
    End Sub

    Protected Sub ShowData()
        Using data As New ThanhTraLaoDongEntities
            Dim p As DanhMucTrichDan = (From q In data.DanhMucTrichDans Where q.TrichDanId = hidID.Value Select q).SingleOrDefault
            If Not p Is Nothing Then
                txtNDTrichDan.Text = IIf(IsNothing(p.NDTrichDan) = True, "", p.NDTrichDan)
                If Request.QueryString("Mod").Equals("edit") Then
                    lblTenCotCauHoi.Visible = True
                    txtTenCotCauHoi.Visible = True
                    txtTenCotCauHoi.Text = IIf(IsNothing(p.TenCotCauHoi), "", p.TenCotCauHoi)
                End If

            End If
        End Using
    End Sub
     
#End Region

#Region "Event for control"

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Using data As New ThanhTraLaoDongEntities
            Dim p As DanhMucTrichDan = (From q In data.DanhMucTrichDans Where q.TrichDanId = hidID.Value).SingleOrDefault
                Try
                p.NDTrichDan = txtNDTrichDan.Text.Trim()
                If Request.QueryString("Mod").Equals("edit") Then
                    p.TenCotCauHoi = txtTenCotCauHoi.Text.Trim()
                End If

                p.NguoiSua = Session("username")
                p.NgaySua = Now()
                    data.SaveChanges()
                Insert_App_Log("Update Danhmuctrichdan:" & p.TrichDanId & "", Function_Name.DanhMucTrichDan, Audit_Type.Edit, Request.ServerVariables("REMOTE_ADDR"), Session("UserName"))
                    Excute_Javascript("Alertbox('Cập nhật dữ liệu thành công.');window.location ='../../Page/DanhMucTrichDan/List.aspx';", Me.Page, True)
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
