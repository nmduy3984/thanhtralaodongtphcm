
Imports System.Data
Imports ThanhTraLaoDongModel
Imports Cls_Common
Imports SecurityService
Partial Class Control_KhuCongNghiep_Edit
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
            hidTinhThanhTraSo.Value = Session("TinhThanhTraSo")
            If Not Request.QueryString("KhuCongNghiepId").ToString.Equals("0") Then
                hidID.Value = Request.QueryString("KhuCongNghiepId")
                ShowData()
            End If
        End If
    End Sub

    Protected Sub ShowData()
        Using data As New ThanhTraLaoDongEntities
            Dim p As KhuCongNghiep = (From q In data.KhuCongNghieps Where q.KhuCongNghiepId = hidID.Value Select q).FirstOrDefault
            If Not p Is Nothing Then
                txtTitle.Text = IIf(IsNothing(p.TenKhuCongNghiep) = True, "", p.TenKhuCongNghiep)
                txtDescription.Text = IIf(IsNothing(p.Mota) = True, "", p.Mota)
            End If
        End Using
    End Sub

#End Region
#Region "Event for control "

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click

        Using data As New ThanhTraLaoDongEntities
            Dim p As ThanhTraLaoDongModel.KhuCongNghiep = (From q In data.KhuCongNghieps Where q.KhuCongNghiepId = hidID.Value Select q).FirstOrDefault
            Try
                p.TenKhuCongNghiep = txtTitle.Text.Trim()
                p.TinhId = CInt(hidTinhThanhTraSo.Value)
                p.Mota = txtDescription.Text.Trim()
                p.NgaySua = Date.Now
                p.NguoiSua = Session("Username")
                data.SaveChanges()
                Insert_App_Log("Update KhuCongNghiep:" & p.TenKhuCongNghiep & "", Function_Name.KhuCongNghiep, Audit_Type.Edit, Request.ServerVariables("REMOTE_ADDR"), Session("UserName"))
                Excute_Javascript("Alertbox('Cập nhật dữ liệu thành công.');window.location ='../../Page/KhuCongNghiep/List.aspx';", Me.Page, True)
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
