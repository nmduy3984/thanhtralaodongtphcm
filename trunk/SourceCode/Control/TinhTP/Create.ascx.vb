Imports System.Data
Imports ThanhTraLaoDongModel
Imports Cls_Common
Imports SecurityService
Partial Class Control_TinhTP_Create
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

   
    Protected Function CheckExist(ByVal strCode As String) As Boolean
        Using data As New ThanhTraLaoDongEntities
            Dim p As Tinh = (From q In data.Tinhs Where q.MaTinh.Equals(strCode) Select q).SingleOrDefault
            If Not p Is Nothing Then
                Return True
            Else
                Return False
            End If
        End Using

    End Function
#End Region
#Region "Event for control "
    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If CheckExist(txtCode.Text.Trim) Then
            Excute_Javascript("Alertbox('Mã tỉnh/TP đã tồn tại.');", Me.Page, True)
        Else
            Using data As New ThanhTraLaoDongEntities
                Dim p As New ThanhTraLaoDongModel.Tinh
                Try
                    p.MaTinh = txtCode.Text.Trim()
                    p.TenTinh = txtTitle.Text.Trim()
                    p.KiHieu = makeURLFriendly(txtTitle.Text.Trim)
                    p.MoTa = txtDescription.Text.Trim()
                    p.TenSo = txtTenSo.Text.Trim()
                    p.DienThoai = txtDienThoai.Text.Trim()
                    p.IsTinh = chkIsTinh.Checked
                    data.Tinhs.AddObject(p)
                    data.SaveChanges()
                    Insert_App_Log("Insert  Tỉnh/TP:" & p.TenTinh & "", Function_Name.TinhTP, Audit_Type.Create, Request.ServerVariables("REMOTE_ADDR"), Session("UserName"))
                    Excute_Javascript("Alertbox('Cập nhật dữ liệu thành công.');window.location ='../../Page/TinhTP/List.aspx';", Me.Page, True)
                Catch ex As Exception
                    log4net.Config.XmlConfigurator.Configure()
                    log.Error("Error error " & AddTabSpace(1) & Session("Username") & AddTabSpace(1) & "IP:" & GetIPAddress(), ex)
                    Excute_Javascript("Alertbox('Cập nhật thất bại.');", Me.Page, True)
                End Try
            End Using
        End If


    End Sub
    Protected Sub btnHuy_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnHuy.Click
        Response.Redirect("List.aspx")
    End Sub
#End Region
End Class
