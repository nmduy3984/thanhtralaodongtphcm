Imports System.IO
Imports System.Data
Imports SecurityService
 
Imports Cls_Common

Partial Class Control_BackupDatabase_List
    Inherits System.Web.UI.UserControl

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            If Not Session("Username") = "" Then
                Dim script As ScriptManager = ScriptManager.GetCurrent(Me.Page)
                If Not script Is Nothing AndAlso script.IsInAsyncPostBack Then
                    ScriptManager.RegisterStartupScript(Me.Page, Me.Page.GetType, "duyjs", "ajaxJquery()", True)
                 Else
                    Me.Page.ClientScript.RegisterStartupScript(Me.Page.GetType(), "duyjs", String.Concat("Sys.Application.add_load(function(){", "ajaxJquery()", "});"), True)
                 End If
                 
                'btnDelete.Attributes.Add("onclick", "return confirmMultiDelete('" & btnDelete.ClientID & "');")
                BindToGrid()
            Else
                Response.Redirect("../../Login.aspx")
            End If
        End If
        

    End Sub
    Private Sub BindToGrid()
        Dim appPath As String = HttpContext.Current.Request.ApplicationPath
        Dim physicalPath As String = HttpContext.Current.Request.MapPath(appPath) + "/BackupDatabase/"
        Dim folder As New DirectoryInfo(physicalPath)
        If folder.Exists Then
            Dim t As New DataTable
            t.Columns.Add("Id", GetType(Integer))
            t.Columns.Add("Name", GetType(String))
            t.Columns.Add("Size", GetType(Integer))
            t.Columns.Add("Create", GetType(DateTime))
            Dim i As Integer = 0
            For Each f As FileInfo In folder.GetFiles
                t.Rows.Add(i + 1, f.Name, f.Length, f.CreationTime)
                i = i + 1
            Next
            Dim strKey_Name() As String = {"Id", "Name"}
            With grdShow
                .DataKeyNames = strKey_Name
                .DataSource = t
                .DataBind()
            End With
        End If
    End Sub

    Protected Sub grdShow_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdShow.RowDataBound
        If e.Row.RowIndex >= 0 Then
            Dim hplFileName As HyperLink = CType(e.Row.FindControl("hplFileName"), HyperLink)
            Dim lnkbtnDelete As LinkButton = CType(e.Row.FindControl("lnkbtnDelete"), LinkButton)
            Dim chkItem As CheckBox = CType(e.Row.FindControl("chkItem"), CheckBox)
            lnkbtnDelete.Attributes.Add("onclick", "return ComfirmDialog('" + drpMessage.Items(0).Text + "', 0,'" + lnkbtnDelete.ClientID + "','" + e.Row.RowIndex.ToString + "',1);")

            'Xử lý phân quyền chọn từ hệ thống và điều kiện riêng từ store
            Dim ScriptManager As System.Web.UI.ScriptManager = System.Web.UI.ScriptManager.GetCurrent(Me.Page)
            ScriptManager.RegisterAsyncPostBackControl(lnkbtnDelete)
            Dim appPath As String = HttpContext.Current.Request.ApplicationPath
            hplFileName.Text = grdShow.DataKeys(e.Row.RowIndex)("Name").ToString()
            hplFileName.NavigateUrl = appPath + "/BackupDatabase/" + grdShow.DataKeys(e.Row.RowIndex)("Name").ToString()
        End If
    End Sub

    Protected Sub lnkbtnDelete_Click(ByVal sender As Object, ByVal e As EventArgs)
        Dim appPath As String = HttpContext.Current.Request.ApplicationPath
        Dim physicalPath As String = HttpContext.Current.Request.MapPath(appPath) + "/BackupDatabase/"
        Dim folder As New DirectoryInfo(physicalPath)
        For Each f As FileInfo In folder.GetFiles
            If f.Name.Equals(grdShow.DataKeys(hidID.Value)("Name")) Then
                f.Delete()
                Excute_Javascript("AlertboxRedirect('Xóa thành công.','List.aspx');", Me.Page, True)
                BindToGrid()
                Exit For
            End If
        Next
    End Sub

    Protected Sub bntBackup_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles bntBackup.Click
        Using data As New ThanhTraLaoDongModel.ThanhTraLaoDongEntities
            Dim appPath As String = HttpContext.Current.Request.ApplicationPath
            Dim physicalPath As String = HttpContext.Current.Request.MapPath(appPath) + "/BackupDatabase/"
            Dim folder As New DirectoryInfo(physicalPath)
            If Not folder.Exists Then
                folder.Create()
                data.uspBackupDatabase("ThanhTraLaoDong", physicalPath)
                BindToGrid()
            Else
                data.uspBackupDatabase("ThanhTraLaoDong", physicalPath)
                BindToGrid()
            End If
        End Using
    End Sub

   
End Class
