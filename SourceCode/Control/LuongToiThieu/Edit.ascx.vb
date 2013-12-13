Imports System.Data
Imports ThanhTraLaoDongModel
Imports Cls_Common
Partial Class Control_LuongToiThieu_Editor
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
            If Not Request.QueryString("LTTID").ToString.Equals("0") Then
                hidID.Value = Request.QueryString("LTTID")
                ShowData()
            End If
        End If
    End Sub
    
    Protected Sub ShowData()
        Using data As New ThanhTraLaoDongEntities
            Dim p As LuongToiThieu = (From q In data.LuongToiThieux Where q.LuongToiThieuID = hidID.Value Select q).SingleOrDefault
            If Not p Is Nothing Then
                txtMota.Text = IIf(IsNothing(p.TieuDe) = True, "", p.TieuDe)
                txtMucluongtoithieu.Text = IIf(IsNothing(p.MucLuongToiThieu) = True, "", String.Format("{0:n0}", p.MucLuongToiThieu))
                ddlLoainhanuoc.SelectedValue = IIf(IsNothing(p.IsNhaNuoc) = True, "", p.IsNhaNuoc)
                txtQuyetDinh.Text = IIf(IsNothing(p.QuyetDinh), "", p.QuyetDinh)
                If Not IsNothing(p.NgayQuyetDinh) Then
                    txtNgayQuyetDinh.Text = CType(p.NgayQuyetDinh, Date).ToString("dd/MM/yyyy")
                Else
                    txtNgayQuyetDinh.Text = ""
                End If


            End If
        End Using
    End Sub
#End Region
#Region "Event for control"
    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Using data As New ThanhTraLaoDongEntities
            Dim p As LuongToiThieu = (From q In data.LuongToiThieux Where q.LuongToiThieuID = hidID.Value).SingleOrDefault
            Try
                p.TieuDe = txtMota.Text.Trim()
                p.MucLuongToiThieu = txtMucluongtoithieu.Text.Trim()
                p.IsNhaNuoc = ddlLoainhanuoc.SelectedValue
                p.QuyetDinh = IIf(String.IsNullOrEmpty(txtQuyetDinh.Text), Nothing, txtQuyetDinh.Text)
                p.NgayQuyetDinh = IIf(IsNothing(txtNgayQuyetDinh.Text), Nothing, StringToDate(txtNgayQuyetDinh.Text))
                data.SaveChanges()
                'Insert_App_Log("Insert Danhmuchanhvi:" & txtTitle.Text.Trim & "", Function_Name.Danhmuchanhvi, Audit_Type.Create, Request.ServerVariables("REMOTE_ADDR"), Session("UserName"))
                Excute_Javascript("AlertboxRedirect('Cập nhật dữ liệu thành công.','../../Page/LuongToiThieu/List.aspx');", Me.Page, True)
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
