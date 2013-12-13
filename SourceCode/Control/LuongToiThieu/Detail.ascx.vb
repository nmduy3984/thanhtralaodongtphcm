Imports System.Data
Imports ThanhTraLaoDongModel
Imports Cls_Common
Partial Class Control_LuongToiThieu_Detail
    Inherits System.Web.UI.UserControl
#Region "Sub and Function "
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Dim script As ScriptManager = ScriptManager.GetCurrent(Me.Page)
            If Not script Is Nothing AndAlso script.IsInAsyncPostBack Then
                 ScriptManager.RegisterStartupScript(Me.Page, Me.Page.GetType, "duyjs1", "ajaxJqueryToolTip()", True)
            Else
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
            Dim p = (From q In data.LuongToiThieux Where q.LuongToiThieuID = hidID.Value Select q).ToList
            If Not p Is Nothing Then
                lblMota.Text = IIf(IsNothing(p(0).TieuDe) = True, "", p(0).TieuDe)
                lblMucluongtoithieu.Text = IIf(IsNothing(p(0).MucLuongToiThieu) = True, "", String.Format("{0:n0}", p(0).MucLuongToiThieu))
                lblLoainhanuoc.Text = IIf(p(0).IsNhaNuoc = 1, "Nhà nước", "Ngoài nhà nước")
                lblQuyetDinh.Text = IIf(IsNothing(p(0).QuyetDinh), "", p(0).QuyetDinh)
                If IsNothing(p(0).NgayQuyetDinh) Then
                    lblNgayQuyetDinh.Text = ""
                Else
                    lblNgayQuyetDinh.Text = CType(p(0).NgayQuyetDinh, Date).ToString("dd/MM/yyyy")
                End If

            End If
        End Using
    End Sub
#End Region
#Region "Event for control"
    Protected Sub btnBack_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnBack.Click
        Response.Redirect("List.aspx")
    End Sub
#End Region
End Class
