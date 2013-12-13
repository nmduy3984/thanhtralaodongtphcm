﻿
Imports System.Data
Imports ThanhTraLaoDongModel
Imports Cls_Common
Imports SecurityService
Partial Class Control_QuanHuyen_Edit
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
            LoadType()
            If Not Request.QueryString("HuyenId").ToString.Equals("0") Then
                hidID.Value = Request.QueryString("HuyenId")
                ShowData()
            End If
        End If
    End Sub

    Protected Sub LoadType()
        'Tinh-Thanh pho
        Using Data As New ThanhTraLaoDongEntities
            Dim tp = (From q In Data.Tinhs
                       Select New With {.Value = q.TinhId, .Text = q.TenTinh})
            With ddlTinhTP
                .Items.Clear()
                .AppendDataBoundItems() = True
                .DataTextField = "Text"
                .DataValueField = "Value"
                .DataSource = tp
                .DataBind()
                .Items.Insert(0, New ListItem(Cls_Common.Str_Opt_Chon, 0))
            End With
            'Luong toi thieu
            Dim ltt = (From p In Data.LuongToiThieux
                       Select New With {.Value = p.LuongToiThieuID, .Text = p.TieuDe})
            With ddlLuongToiThieu
                .Items.Clear()
                .AppendDataBoundItems() = True
                .DataTextField = "Text"
                .DataValueField = "Value"
                .DataSource = ltt
                .DataBind()
                .Items.Insert(0, New ListItem(Cls_Common.Str_Opt_Chon, 0))
            End With

        End Using
    End Sub

    Protected Sub ShowData()
        Using data As New ThanhTraLaoDongEntities
            Dim p As Huyen = (From q In data.Huyens Where q.HuyenId = hidID.Value Select q).FirstOrDefault
            If Not p Is Nothing Then
                txtTitle.Text = IIf(IsNothing(p.TenHuyen) = True, "", p.TenHuyen)
                ddlTinhTP.SelectedValue = IIf(IsNothing(p.TinhId) = True, 0, p.TinhId)
                ddlLuongToiThieu.SelectedValue = IIf(IsNothing(p.LuongToiThieuID) = True, 0, p.LuongToiThieuID)
                txtDescription.Text = IIf(IsNothing(p.MoTa) = True, "", p.MoTa)
            End If
        End Using
    End Sub

#End Region
#Region "Event for control "

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click

        Using data As New ThanhTraLaoDongEntities
            Dim p As ThanhTraLaoDongModel.Huyen = (From q In data.Huyens Where q.HuyenId = hidID.Value Select q).FirstOrDefault
            Try
                p.TenHuyen = txtTitle.Text.Trim()
                p.TinhId = ddlTinhTP.SelectedValue
                p.LuongToiThieuID = ddlLuongToiThieu.SelectedValue
                p.Mota = txtDescription.Text.Trim()

                data.SaveChanges()
                Insert_App_Log("Update  Tỉnh/TP:" & p.TenHuyen & "", Function_Name.TinhTP, Audit_Type.Create, Request.ServerVariables("REMOTE_ADDR"), Session("UserName"))
                Excute_Javascript("Alertbox('Cập nhật dữ liệu thành công.');window.location ='../../Page/QuanHuyen/List.aspx';", Me.Page, True)
            Catch ex As Exception
                log4net.Config.XmlConfigurator.Configure()
                log.Error("Error error " & AddTabSpace(1) & Session("Username") & AddTabSpace(1) & "IP:" & GetIPAddress(), ex)
                Excute_Javascript("Alertbox('Cập nhật thất bại.');", Me.Page, True)
            End Try
        End Using

    End Sub

    Protected Sub btnHuy_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnHuy.Click
        Response.Redirect("List.aspx")
    End Sub

#End Region
End Class
