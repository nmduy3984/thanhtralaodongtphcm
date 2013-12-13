Imports System.Data
Imports ThanhTraLaoDongModel
Imports Cls_Common
Imports SecurityService
Partial Class Control_DanhMucCauHoi_Create
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

        End If
    End Sub

#End Region
#Region "Event for control "
    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Using data As New ThanhTraLaoDongEntities
            Dim p As New ThanhTraLaoDongModel.CauHoiHierarchy
            Try
                p.CauHoiId = txtCauHoiId.Text.Trim()
                p.Title = txtNDMucVP.Text.Trim
                p.ParentId = txtParentId.Text.Trim
                p.CauHoiVietTat = txtCauHoiVietTat.Text.Trim
                If Not txtTieuChi.Text.Trim.Equals("") And Not txtTieuChi.Text.Trim.Equals("0") Then
                    p.IsTieuChi = CInt(txtTieuChi.Text.Trim)
                End If

                p.ChuoiDieuKien = IIf(txtDieuKien.Text.Trim.Equals(""), "", txtDieuKien.Text.Trim)
                If txtSapXep.Text.Trim.Equals("") Then
                    p.Sort = 0
                Else
                    p.Sort = CInt(txtSapXep.Text.Trim)
                End If

                p.IsTieuChiMoiBC = chkIsTieuChiMoiBC.Checked
                data.CauHoiHierarchies.AddObject(p)
                data.SaveChanges()
                'Insert_App_Log("Insert  Danhmuctrichdan:" & p.TrichDanId & "", Function_Name.DanhMucTrichDan, Audit_Type.Create, Request.ServerVariables("REMOTE_ADDR"), Session("UserName"))
                Excute_Javascript("Alertbox('Cập nhật dữ liệu thành công.');window.location ='../../Page/DanhMucCauHoi/List.aspx';", Me.Page, True)
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
