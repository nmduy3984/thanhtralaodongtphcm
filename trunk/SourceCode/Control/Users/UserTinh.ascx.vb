
Imports System.Data
Imports ThanhTraLaoDongModel
Imports Cls_Common
Imports SecurityService
Partial Class Control_Users_UserTinh
    Inherits System.Web.UI.UserControl
    Private Shared ReadOnly log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
    'Luu nhung MenuId duoc check
    Private IDChecked As String = ""
    Private dtListMenuSelect As List(Of UsersTinh)

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
            FillUsers()
            FillParentMenu()
            If Not String.IsNullOrEmpty(Request.QueryString("Userid")) Then
                ddlUserID.SelectedValue = Request.QueryString("Userid")
                ShowData()
            End If
        End If
    End Sub
    'Load du lieu cho nguoi dung
    Protected Sub FillUsers()
        Using data As New ThanhTraLaoDongEntities
            Dim query = (From q In data.Users Where q.IsActivated = True Select q.UserId, q.UserName).ToList
            ddlUserID.DataValueField = "UserId"
            ddlUserID.DataTextField = "UserName"
            ddlUserID.DataSource = query
            ddlUserID.DataBind()
            ddlUserID.Items.Insert(0, New ListItem("-----Chọn-----", 0))
            ddlUserID.SelectedValue = 0
        End Using
    End Sub
    'Load du lieu cho menu cha
    Protected Sub FillParentMenu()
        Using data As New ThanhTraLaoDongEntities
            Dim query = From q In data.Tinhs Order By q.TenTinh Ascending Select q
            Dim dtList = query.ToList()
            trvMenu.Nodes.Clear()
            Dim tnParentRoot As New TreeNode()
            tnParentRoot.Text = "Tất cả"
            tnParentRoot.Value = ""
            trvMenu.Nodes.Add(tnParentRoot)

            tnParentRoot.ExpandAll()


            For i As Integer = 0 To dtList.Count - 1
                Dim tnParent As New TreeNode()
                tnParent.Text = dtList(i).TenTinh
                tnParent.Value = dtList(i).TinhId.ToString()
                tnParentRoot.ChildNodes.Add(tnParent)
                ' tnParent.Collapse()
                'FillChild(tnParent, Int32.Parse(tnParent.Value))
               
                If CheckSelectedMenu(dtListMenuSelect, dtList(i).TinhId) = 1 Then
                    tnParent.Checked = True
                    'tnParent.ExpandAll()
                Else
                    tnParent.Checked = False

                End If
            Next
        End Using
    End Sub
    'Load du lieu cho menu con
    
    'Lay gia tri menu con
    
    'Lay gia tri menu con
    Protected Sub GetParentNode()
        IDChecked = ""

        'For i As Integer = 0 To trvMenu.Nodes.Count - 1
        '    If trvMenu.Nodes(i).Checked = True Then
        '        IDChecked = (IDChecked & ",") + trvMenu.Nodes(i).Value
        '    End If

        'Next
        GetChildNode(trvMenu.Nodes(0))
    End Sub
    'Lay gia tri menu con
    Protected Sub GetChildNode(ByVal tn As TreeNode)
        For i As Integer = 0 To tn.ChildNodes.Count - 1
            If tn.ChildNodes(i).Checked = True Then
                IDChecked = (IDChecked & ",") + tn.ChildNodes(i).Value
            End If
            GetChildNode(tn.ChildNodes(i))
        Next
    End Sub
    Protected Function CheckSelectedMenu(ByVal dtList As List(Of UsersTinh), ByVal Value As Integer) As Integer
        Dim Result As Integer = 0
        If Not dtList Is Nothing Then
            For i As Integer = 0 To dtList.Count - 1
                If Int32.Parse(dtList(i).TinhId.ToString()) = Value Then
                    Result = 1
                    Exit For
                End If
            Next
        End If

        Return Result
    End Function
    Protected Sub CustomValidator1_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs)
        If trvMenu.CheckedNodes.Count > 0 Then
            args.IsValid = True
        Else
            args.IsValid = False
        End If
    End Sub

#End Region
#Region "Event for control "
    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Using data As New ThanhTraLaoDongEntities
            Try
                'Xoa du lieu cu
                Dim userid As Integer = ddlUserID.SelectedValue
                Dim h As List(Of ThanhTraLaoDongModel.UsersTinh) = (From t In data.UsersTinhs Where t.UserId = userid Select t).ToList
                For Each k As ThanhTraLaoDongModel.UsersTinh In h
                    data.UsersTinhs.DeleteObject(k)
                    data.SaveChanges()
                Next
                'Them du lieu moi
                GetParentNode()
                Dim arrID() As String = IDChecked.Split(",")
                If arrID.Length > 0 Then
                    For i As Integer = 1 To arrID.Length - 1
                        Dim p As New ThanhTraLaoDongModel.UsersTinh
                        p.UserId = ddlUserID.SelectedValue
                        p.TinhId = arrID(i)
                        data.UsersTinhs.AddObject(p)
                        data.SaveChanges()
                        p = Nothing
                    Next
                End If

                Excute_Javascript("AlertboxRedirect('Cập nhật dữ liệu thành công.','" & Request.Url().ToString & "');", Me.Page, True)
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
    Protected Sub ddlUserID_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlUserID.SelectedIndexChanged
        ShowData()
    End Sub
    Protected Sub ShowData()
        Using data As New ThanhTraLaoDongEntities
            Dim userid As Integer = ddlUserID.SelectedValue
            dtListMenuSelect = (From h In data.UsersTinhs Where h.UserId = userid Select h).ToList
        End Using
        FillParentMenu()
    End Sub
#End Region
End Class
