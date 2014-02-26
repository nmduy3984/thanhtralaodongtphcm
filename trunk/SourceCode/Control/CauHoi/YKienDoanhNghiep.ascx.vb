Imports System.Data
Imports ThanhTraLaoDongModel
Imports Cls_Common
Imports SecurityService
Imports System.IO

Partial Class Control_CauHoi_YKienDoanhNghiep
    Inherits System.Web.UI.UserControl

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            If Not Session("Username") = "" Then
                Dim script As ScriptManager = ScriptManager.GetCurrent(Me.Page)
                If Not script Is Nothing AndAlso script.IsInAsyncPostBack Then
                    ScriptManager.RegisterStartupScript(Me.Page, Me.Page.GetType, "TTLDjs", "ajaxJquery()", True)
                Else
                    Me.Page.ClientScript.RegisterStartupScript(Me.Page.GetType(), "TTLDjs", String.Concat("Sys.Application.add_load(function(){", "ajaxJquery()", "});"), True)
                End If
                hidPhieuID.Value = Session("phieuid")
                LoadData()

            Else
                Response.Redirect("../../Login.aspx")
            End If
        End If
    End Sub
    Protected Sub LoadData()
        Using data As New ThanhTraLaoDongEntities
            Dim phieuNhap = (From q In data.PhieuNhapHeaders Where q.PhieuID = hidPhieuID.Value).FirstOrDefault()
            If Not IsNothing(phieuNhap) Then
                If Not IsNothing(phieuNhap.YKienCuaDN) Then
                    Dim Strykien As String = ""
                    For Each item As String In Strings.Split(phieuNhap.YKienCuaDN, Str_Symbol_Group)
                        Strykien = Strykien + item + Environment.NewLine
                    Next
                    txtYKienDN.Text = Strykien
                    chkQ11.SelectedValue = 1
                Else
                    chkQ11.SelectedValue = 0
                End If
            Else
                chkQ11.SelectedValue = 0
            End If
        End Using
    End Sub
    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click

        Dim iPhieuId As Integer = CInt(Session("phieuid"))
        Using data As New ThanhTraLaoDongEntities
            Dim phieuNhap = (From q In data.PhieuNhapHeaders Where q.PhieuID = iPhieuId).FirstOrDefault()

            If Not IsNothing(phieuNhap) Then
                If chkQ11.SelectedValue = 1 Then
                    Dim strResult As String = ""
                    For Each item In ReadAllLines(txtYKienDN.Text)
                        If Not String.Equals(item, "") Then
                            strResult = strResult & item & Str_Symbol_Group
                        End If
                    Next
                    'Nếu có ý kiến thì thực hiện cắt đi ký tự "Str_Symbol_Group" cuối cùng.
                    If strResult.Length > 0 Then
                        phieuNhap.YKienCuaDN = strResult.Substring(0, strResult.Length - Str_Symbol_Group.Length)
                    End If
                Else
                    phieuNhap.YKienCuaDN = Nothing
                End If
                data.SaveChanges()
                Excute_Javascript("window.location ='List.aspx';", Me.Page, True)
            End If
        End Using
    End Sub

End Class
