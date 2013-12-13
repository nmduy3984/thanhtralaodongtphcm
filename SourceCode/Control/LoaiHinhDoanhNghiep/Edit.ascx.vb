
Imports System.Data
Imports ThanhTraLaoDongModel
Imports Cls_Common
Imports SecurityService

Partial Class Control_LoaiHinhDoanhNghiep_Edit
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
            If Not Request.QueryString("LoaiHinhDNId").ToString.Equals("0") Then
                hidID.Value = Request.QueryString("LoaiHinhDNId")

                ShowData()
            End If
        End If
    End Sub

    
    Protected Sub ShowData()
        Using data As New ThanhTraLaoDongEntities
            Dim p As LoaiHinhDoanhNghiep = (From q In data.LoaiHinhDoanhNghieps Where q.LoaiHinhDNId = hidID.Value Select q).SingleOrDefault
            If Not p Is Nothing Then
                '  txtCode.Text = IIf(IsNothing(p.Code) = True, "", p.Code)
                txtTitle.Text = IIf(IsNothing(p.TenLoaiHinhDN) = True, "", p.TenLoaiHinhDN)
                '  ddlType.SelectedValue = IIf(IsNothing(p.ParentID) = True, 0, p.ParentID)
                '   txtDescription.Text = IIf(IsNothing(p.Description) = True, "", p.Description)
            End If
        End Using
    End Sub

    Protected Function CheckExist(ByVal strTitle As String, ByVal intID As Integer) As Boolean
        Using data As New ThanhTraLaoDongEntities
            Dim p As LoaiHinhDoanhNghiep = (From q In data.LoaiHinhDoanhNghieps Where Not (q.LoaiHinhDNId = intID) And q.TenLoaiHinhDN.Equals(strTitle) Select q).SingleOrDefault
            If Not p Is Nothing Then
                Return True
            Else
                Return False
            End If
        End Using

    End Function
#End Region
#Region "Event for control"
    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If CheckExist(txtTitle.Text.Trim, hidID.Value) Then
            Excute_Javascript("Alertbox('Loại hình doanh nghiệp đã tồn tại');", Me.Page, True)
        Else
            Using data As New ThanhTraLaoDongEntities
                Dim p As LoaiHinhDoanhNghiep = (From q In data.LoaiHinhDoanhNghieps Where q.LoaiHinhDNId = hidID.Value).SingleOrDefault
                Try
                    p.LoaiHinhDNId = hidID.Value
                    '  p.Code = txtCode.Text.Trim()
                    p.TenLoaiHinhDN = txtTitle.Text.Trim()
                    ' p.ParentID = ddlType.SelectedValue
                    ' p.Description = txtDescription.Text.Trim()
                    data.SaveChanges()
                    Insert_App_Log("Insert Loai hinh doanh nghiep:" & txtTitle.Text.Trim & "", Function_Name.LoaiHinhDN, Audit_Type.Create, Request.ServerVariables("REMOTE_ADDR"), Session("UserName"))
                    Excute_Javascript("Alertbox('Cập nhật dữ liệu thành công.');window.location ='../../Page/LoaiHinhDoanhNghiep/List.aspx';", Me.Page, True)
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
