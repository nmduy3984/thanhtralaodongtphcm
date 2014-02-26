Imports System.Data
Imports ThanhTraLaoDongModel
Imports System.IO
Imports Cls_Common
Partial Class Control_CauHoi_XemKetLuan
    Inherits System.Web.UI.UserControl

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            If Not Session("Username") = "" Then
                If Not Request.QueryString("PhieuID").ToString.Equals("0") Then
                    hidID.Value = Request.QueryString("PhieuID")
                End If
                LoadData()
                'btnInPhieu.Visible = False
                'btnInPhieuKienNghi.Visible = False
                'If Request.Path.Contains("PhieuKiemTra") Then
                '    btnInPhieuKienNghi.Visible = True
                'ElseIf Request.Path.Contains("BienBanThanhTra") Then
                '    btnInPhieu.Visible = True
                'End If
            Else
                Response.Redirect("../../Login.aspx")
            End If
        End If
    End Sub
    Private Sub LoadData()
        Using data As New ThanhTraLaoDongEntities
            Dim kvp = (From a In data.KetLuans Where a.IsViPham = 1 And a.PhieuId = hidID.Value Select New With {a.NDKetLuan}).ToList
            Dim str_keyname() As String = {"NDKetLuan"}
            With grdShowVP
                .DataKeyNames = str_keyname
                .DataSource = kvp
                .DataBind()
            End With
            Dim vp = (From a In data.KetLuans Where a.IsViPham = 0 And a.PhieuId = hidID.Value Select New With {a.NDKetLuan}).ToList
            With grdshowKVP
                .DataKeyNames = str_keyname
                .DataSource = vp
                .DataBind()
            End With
        End Using
    End Sub

    Protected Sub grdshowKVP_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdshowKVP.RowDataBound
        If e.Row.RowIndex >= 0 Then
            Dim lblSTT As Label = CType(e.Row.FindControl("lblSTT"), Label)
            lblSTT.Text = e.Row.RowIndex + 1
        End If
    End Sub

    Protected Sub grdShowVP_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdShowVP.RowDataBound
        If e.Row.RowIndex >= 0 Then
            Dim lblSTT As Label = CType(e.Row.FindControl("lblSTT"), Label)
            lblSTT.Text = e.Row.RowIndex + 1
        End If
    End Sub

    'Protected Sub btnInPhieu_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnInPhieu.Click
    '    Response.Redirect("BienBanThanhTra.aspx?phieuid=" & hidID.Value)
    'End Sub

    'Protected Sub btnInPhieuKienNghi_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnInPhieuKienNghi.Click
    '    Response.Redirect("PhieuKienNghi.aspx?phieuid=" & hidID.Value)
    'End Sub
End Class
