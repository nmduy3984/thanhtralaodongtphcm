Imports System.Data
Imports ThanhTraLaoDongModel
Imports Cls_Common
Partial Class Control_KienNghi_Detail
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
            If Not Request.QueryString("KienNghiId").ToString.Equals("0") Then
                hidID.Value = Request.QueryString("KienNghiId")
                ShowData()
            End If
        End If
    End Sub
    Protected Sub ShowData()
        Using data As New ThanhTraLaoDongEntities
            Dim p = (From q In data.DanhMucKienNghis Where q.KienNghiID = hidID.Value Select New With {q.NoiDungKN}).ToList
            Dim lstMucViPham = (From a In data.CauHoiHierarchies Join b In data.CauHoiKienNghis On a.CauHoiId Equals b.CauHoiId
                                Where b.DanhMucKienNghiId = hidID.Value Select a).ToList()
            If Not p Is Nothing Then
                lblMota.Text = IIf(IsNothing(p(0).NoiDungKN) = True, "", p(0).NoiDungKN)
                If Not IsNothing(lstMucViPham) Then
                    Dim strTemp As String = "<ul>"
                    For i As Integer = 0 To lstMucViPham.Count - 1
                        strTemp += "<li>" & (i + 1) & ". " & lstMucViPham(i).CauHoiVietTat & "</li>"
                    Next
                    strTemp += "</ul>"
                    lblMucViPham.Text = strTemp
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
