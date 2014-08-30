﻿Imports System.Data
Imports ThanhTraLaoDongModel
Imports Cls_Common
Imports SecurityService

Partial Class Control_LoaiHinhSanXuat_ContentEditor
    Inherits System.Web.UI.UserControl
    Private Shared ReadOnly log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
#Region "Sub and Function "
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            'active tab tuong ung voi Page
            'InVisble button ko can thiet
            If Request.PhysicalPath.Contains("List.aspx") Then
                lbtList.CssClass = "current_manage"
                lbtList.Text = "<span class='current_manage'>Quản lý</span>"
                lbtEdit.Visible = False
                lbtView.Visible = False
                btnDelete.Visible = False
                'Chi ap dung cho du lieu xuat ban
                'tabs_state.Visible = False
                'cboAction.Visible = False
                'Permission
                lbtAdd.Visible = HasPermission(Function_Name.LoaiHinhSanXuat, Session("RoleID"), 0, Audit_Type.Create)
            ElseIf Request.PhysicalPath.Contains("Create.aspx") Then
                lbtAdd.CssClass = "current_manage"
                lbtAdd.Text = "<span class='current_manage'>Thêm mới</span>"
                lbtEdit.Visible = False
                lbtView.Visible = False
                btnDelete.Visible = False
                'Chi ap dung cho du lieu xuat ban
                ' tabs_state.Visible = False
                ' cboAction.Visible = False
                'Permission
                lbtAdd.Visible = HasPermission(Function_Name.LoaiHinhSanXuat, Session("RoleID"), 0, Audit_Type.Create)
            ElseIf Request.PhysicalPath.Contains("Detail.aspx") Then
                lbtView.CssClass = "current_manage"
                lbtView.Text = "<span class='current_manage'>Xem</span>"
                'Permission
                setPermission()
            ElseIf Request.PhysicalPath.Contains("Edit.aspx") Then
                lbtEdit.CssClass = "current_manage"
                lbtEdit.Text = "<span class='current_manage'>Sửa</span>"
                'Permission
                setPermission()
            End If
            If Not Request.QueryString("LoaiHinhSXId") Is Nothing Then
                hidID.Value = Request.QueryString("LoaiHinhSXId")
                'Load list action status
                'LoadListStatus(hidID.Value)
                'Permission
                btnDelete.Visible = HasPermission(Function_Name.LoaiHinhSanXuat, Session("RoleID"), 0, Audit_Type.Delete)
                btnDelete.Attributes.Add("onclick", "return ComfirmDialog('Bạn có chắc chắn muốn xóa không?',0,'" + btnDelete.ClientID + "','" + hidID.Value.ToString + "',1);")
            End If
        End If
    End Sub
    Protected Sub setPermission()
        'Se bat lai khi dua chuc nang phan quyen
        lbtAdd.Visible = HasPermission(Function_Name.LoaiHinhSanXuat, Session("RoleID"), 0, Audit_Type.Create)
        lbtView.Visible = HasPermission(Function_Name.LoaiHinhSanXuat, Session("RoleID"), 0, Audit_Type.ViewContent)
        lbtEdit.Visible = HasPermission(Function_Name.LoaiHinhSanXuat, Session("RoleID"), 0, Audit_Type.Edit)
        btnDelete.Visible = HasPermission(Function_Name.LoaiHinhSanXuat, Session("RoleID"), 0, Audit_Type.Delete)
    End Sub
    Protected Sub lbtList_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbtList.Click
        Response.Redirect("List.aspx")
    End Sub
    Protected Sub lbtAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbtAdd.Click
        Response.Redirect("Create.aspx")
    End Sub
    Protected Sub lbtEdit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbtEdit.Click
        Response.Redirect("Edit.aspx?LoaiHinhSXId=" & hidID.Value & "")
    End Sub
    Protected Sub lbtView_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbtView.Click
        Response.Redirect("Detail.aspx?LoaiHinhSXId=" & hidID.Value & "")
    End Sub
    Protected Sub lnkbtnDelete_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnDelete.Click
        Dim intId As Integer
        Dim strLogName As String = ""
        Using data As New ThanhTraLaoDongEntities
            intId = hidID.Value
            Dim q = (From p In data.LoaiHinhSanXuats Where p.LoaiHinhSXId = intId Select p).FirstOrDefault
            Try
                Dim dn = (From a In data.DoanhNghieps Where a.LoaiHinhSXId = intId).ToList
                Dim lhsxChild = (From a In data.LoaiHinhSanXuats Where a.ParentID = intId).ToList
                If dn.Count = 0 And lhsxChild.Count = 0 Then
                    data.LoaiHinhSanXuats.DeleteObject(q)
                    data.SaveChanges()
                    Insert_App_Log("Delete  Loai hinh san xuat:" & q.Title & "", Function_Name.LoaiHinhSanXuat, Audit_Type.Delete, Request.ServerVariables("REMOTE_ADDR"), Session("UserName"))
                    Excute_Javascript("Alertbox('Xóa dữ liệu thành công.');window.location ='../../Page/LoaiHinhSanXuat/List.aspx';", Me.Page, True)
                Else
                    Excute_Javascript("Alertbox('Xóa thất bại. Loại hình sản xuất này hiện tại có doanh nghiệp hoặc loại hình sản xuất khác tham chiếu đến.');", Me.Page, True)
                End If
            Catch ex As Exception
                log4net.Config.XmlConfigurator.Configure()
                log.Error("Error error " & AddTabSpace(1) & Session("Username") & AddTabSpace(1) & "IP:" & GetIPAddress(), ex)
                Excute_Javascript("Alertbox('Xóa thất bại (" & ex.Message & ").');", Me.Page, True)
            End Try
        End Using
    End Sub

#End Region
End Class