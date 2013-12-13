Imports System.Data
Imports ThanhTraLaoDongModel
Imports Cls_Common
Partial Class Control_sysFunction_sysFunctionMenu
    Inherits System.Web.UI.UserControl
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            BindMenu()
        End If
    End Sub
    Protected Sub BindMenu()
        Using data As New ThanhTraLaoDongEntities
            If Session("Function") Is Nothing Then
                Response.Redirect("~/Login.aspx")
            End If
            Dim arrFuntionId As Integer() = Session("Function")

            Dim p = From q In data.Functions Where q.IsMenu = True And arrFuntionId.Contains(q.FunctionId) Order By q.FunctionName Ascending Select q
            'check role here

            Dim k = p.Take(10)
            Dim h = p.Skip(10)
            With rptMenu
                .DataSource = k
                .DataBind()
            End With

            With rptMenuMore
                .DataSource = h
                .DataBind()

            End With
        End Using

    End Sub

    Protected Sub rptMenu_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles rptMenu.ItemDataBound
        Dim hplTitle As HyperLink
        hplTitle = e.Item.FindControl("hplTitle")
        Dim sufixURL = Request.Url.ToString.Split("Page")(0)

        Using data As New ThanhTraLaoDongEntities
            Dim p = CType(e.Item.DataItem, ThanhTraLaoDongModel.Function)
            hplTitle.NavigateUrl = sufixURL & p.URL
            If Request.Url.ToString.Contains(p.URL.Replace("List.aspx", "")) Then
                hplTitle.Text = "<span class=""current"">" & p.FunctionName & "</span>"
                hplTitle.CssClass = "current"
            Else
                hplTitle.Text = "<span>" & p.FunctionName & "</span>"
            End If
        End Using


    End Sub



    Protected Sub rptMenuMore_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles rptMenuMore.ItemDataBound
        Dim hplTitle As HyperLink
        hplTitle = e.Item.FindControl("hplTitle")
        Dim sufixURL = Request.Url.ToString.Split("Page")(0)

        Using data As New ThanhTraLaoDongEntities
            Dim p = CType(e.Item.DataItem, ThanhTraLaoDongModel.Function)
            hplTitle.NavigateUrl = sufixURL & p.URL

            hplTitle.Text = p.FunctionName
        End Using




    End Sub
End Class
