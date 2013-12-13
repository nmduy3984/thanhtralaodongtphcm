Imports System.Data
Imports ThanhTraLaoDongModel
Imports Cls_Common
Partial Class Control_Function_Detail
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
            If Session("userid") = "2" Then
                Response.Redirect("../../Login.aspx")
            Else
                If Not Request.QueryString("FuncId").ToString.Equals("0") Then
                    hidID.Value = Request.QueryString("FuncId")
                    ShowData()
                End If
            End If

        End If
    End Sub
    Protected Sub ShowData()
        Using data As New ThanhTraLaoDongEntities
            Dim p = (From q In data.Functions, a In data.Functions
                     Where q.FunctionId = hidID.Value And a.FunctionId = q.ParentId
                                   Select New With {q.FunctionName, q.URL, q.HrefName, q.Sort, q.IsMenu, q.ParentId, .HrefNameParent = a.HrefName}).ToList()
            If p.Count > 0 Then
                lblFunctionName.Text = IIf(IsNothing(p(0).FunctionName) = True, "", p(0).FunctionName)
                lblHrefName.Text = IIf(IsNothing(p(0).HrefName) = True, "", p(0).HrefName)
                lblUrl.Text = IIf(IsNothing(p(0).URL) = True, "", p(0).URL)
                lblSort.Text = IIf(IsNothing(p(0).Sort) = True, "", p(0).Sort)
                lblIsMenu.Text = IIf(IsNothing(p(0).IsMenu) = True, "Chưa kích hoạt", "Đã kích hoạt")
                lblParent.Text = IIf(IsNothing(p(0).HrefNameParent) = True, "", p(0).HrefNameParent)

            End If
        End Using
    End Sub
#End Region
End Class
