Imports System.Data
Imports ThanhTraLaoDongModel
Imports Cls_Common

Partial Class Control_LoaiHinhSanXuat_Detail
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
            If Not Request.QueryString("LoaiHinhSXId").ToString.Equals("0") Then
                hidID.Value = Request.QueryString("LoaiHinhSXId")
                ShowData()
            End If
        End If
    End Sub
    Protected Sub ShowData()
        Using data As New ThanhTraLaoDongEntities
            Dim p As LoaiHinhSanXuat = (From q In data.LoaiHinhSanXuats Where q.LoaiHinhSXId = hidID.Value Select q).SingleOrDefault
            If Not p Is Nothing Then
                lblCode.Text = IIf(IsNothing(p.Code) = True, "", p.Code)
                lblTitle.Text = IIf(IsNothing(p.Title) = True, "", p.Title)
                Dim q = (From h In data.LoaiHinhSanXuats Where h.LoaiHinhSXId = p.ParentID Select h).FirstOrDefault
                If Not q Is Nothing Then
                    lblParentid.Text = q.Title.ToString
                End If
                lblDescription.Text = IIf(IsNothing(p.Description) = True, "", p.Description)
            End If
        End Using
    End Sub
#End Region
End Class
