
Imports Cls_Common
Partial Class DoanhNghiep_Control_ContentEditorPhieuTKK
    Inherits System.Web.UI.UserControl


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not Page.IsPostBack Then
            hidPhieuIdGen.Value = IIf(IsNothing(Session("phieuid")), 0, CInt(Session("phieuid")))
            If Not Request("DNId") Is Nothing Then
                hidID.Value = Request("DNId")
            Else
                Excute_Javascript("Alertbox('Hãy nhập thông tin doanh nghiệp trước.');window.location='../../DoanhNghiep/Page/ThongTinChung.aspx';", Me.Page, True)

            End If
            If Request.PhysicalPath.Contains("CauHoi10") Then
                lbtCau10.CssClass = "current_manage"
                lbtCau10.Text = "<span class=""current_manage"">Mục 10</span>"

            ElseIf Request.PhysicalPath.Contains("CauHoi11") Then
                lbtCau11.CssClass = "current_manage"
                lbtCau11.Text = "<span class=""current_manage"">Mục 11</span>"

            ElseIf Request.PhysicalPath.Contains("CauHoi12") Then
                lbtCau12.CssClass = "current_manage"
                lbtCau12.Text = "<span class=""current_manage"">Mục 12</span>"
            ElseIf Request.PhysicalPath.Contains("CauHoi1") Then
                lbtCau1.CssClass = "current_manage"
                lbtCau1.Text = "<span class=""current_manage"">Mục 1</span>"
            ElseIf Request.PhysicalPath.Contains("CauHoi2") Then
                lbtCau2.CssClass = "current_manage"
                lbtCau2.Text = "<span class=""current_manage"">Mục 2</span>"
            ElseIf Request.PhysicalPath.Contains("CauHoi3") Then
                lbtCau3.CssClass = "current_manage"
                lbtCau3.Text = "<span class=""current_manage"">Mục 3</span>"
            ElseIf Request.PhysicalPath.Contains("CauHoi4") Then
                lbtCau4.CssClass = "current_manage"
                lbtCau4.Text = "<span class=""current_manage"">Mục 4</span>"

            ElseIf Request.PhysicalPath.Contains("CauHoi5") Then
                lbtCau5.CssClass = "current_manage"
                lbtCau5.Text = "<span class=""current_manage"">Mục 5</span>"

            ElseIf Request.PhysicalPath.Contains("CauHoi6") Then
                lbtCau6.CssClass = "current_manage"
                lbtCau6.Text = "<span class=""current_manage"">Mục 6</span>"

            ElseIf Request.PhysicalPath.Contains("CauHoi7") Then
                lbtCau7.CssClass = "current_manage"
                lbtCau7.Text = "<span class=""current_manage"">Mục 7</span>"

            ElseIf Request.PhysicalPath.Contains("CauHoi8") Then
                lbtCau8.CssClass = "current_manage"
                lbtCau8.Text = "<span class=""current_manage"">Mục 8</span>"

            ElseIf Request.PhysicalPath.Contains("CauHoi9") Then
                lbtCau9.CssClass = "current_manage"
                lbtCau9.Text = "<span class=""current_manage"">Mục 9</span>"

                lbtPhieuNhap.CssClass = "current_manage"
                lbtPhieuNhap.Text = "<span class=""current_manage"">Thông tin chung</span>"
            End If
        End If

    End Sub
    Protected Sub LinkButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbtPhieuNhap.Click, lbtCau1.Click, lbtCau2.Click, lbtCau3.Click, lbtCau4.Click,
            lbtCau5.Click, lbtCau6.Click, lbtCau7.Click, lbtCau8.Click, lbtCau9.Click, lbtCau10.Click, lbtCau11.Click, lbtCau12.Click
        Try
            If Not Session("Username") = "" Then
                Select Case CType(sender, Control).ID
                    Case "lbtPhieuNhap"
                        Response.Redirect("ThongTinChung.aspx?DNId=" & hidID.Value)
                    Case "lbtCau1"
                        li1.Attributes.Add("class", "current_manage")
                        Response.Redirect("CauHoi1.aspx?DNId=" & hidID.Value)
                    Case "lbtCau2"
                        Response.Redirect("CauHoi2.aspx?DNId=" & hidID.Value)
                    Case "lbtCau3"
                        Response.Redirect("CauHoi3.aspx?DNId=" & hidID.Value)
                    Case "lbtCau4"
                        Response.Redirect("CauHoi4.aspx?DNId=" & hidID.Value)
                    Case "lbtCau5"
                        Response.Redirect("CauHoi5.aspx?DNId=" & hidID.Value)
                    Case "lbtCau6"
                        Response.Redirect("CauHoi6.aspx?DNId=" & hidID.Value)
                    Case "lbtCau7"
                        Response.Redirect("CauHoi7.aspx?DNId=" & hidID.Value)
                    Case "lbtCau8"
                        Response.Redirect("CauHoi8.aspx?DNId=" & hidID.Value)
                    Case "lbtCau9"
                        Response.Redirect("CauHoi9.aspx?DNId=" & hidID.Value)
                    Case "lbtCau10"
                        Response.Redirect("CauHoi10.aspx?DNId=" & hidID.Value)
                    Case "lbtCau11"
                        Response.Redirect("CauHoi11.aspx?DNId=" & hidID.Value)
                    Case "lbtCau12"
                        Response.Redirect("CauHoi12.aspx?DNId=" & hidID.Value)
                    Case "lbtYKienDN"
                        Response.Redirect("YKienDoanhNghiep.aspx?DNId=" & hidID.Value)
                End Select
            Else
                Response.Redirect("../../../Login.aspx")
            End If
        Catch ex As Exception
            Excute_Javascript("AlertboxRedirect('Có lỗi tham chiếu số quyết định.','ThongTinChung.aspx?DNId=" & hidID.Value & "');", Me.Page, True)
        End Try
    End Sub

End Class
