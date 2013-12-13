Imports System.Data
Imports ThanhTraLaoDongModel
Imports Cls_Common
Partial Class Control_KienNghi_Create
    Inherits System.Web.UI.UserControl
    Private Shared ReadOnly log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
    'Luu nhung MenuId duoc check
    Private IDChecked As String = ""
    Private dtListDMKNSelect As List(Of CauHoiKienNghi)
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
            FillParentMenu()
            BindAccountType()
        End If
    End Sub
    Private Sub BindAccountType()
        'Bind AccountType
        Using Data As New ThanhTraLaoDongEntities
            Dim p = (From q In Data.OtherLists Where q.Type.Equals("LoaiHanhVi") Order By q.Name Select q).ToList
            With ddlLoaihanhvi
                .DataValueField = "ID"
                .DataTextField = "Name"
                .DataSource = p
                .DataBind()
            End With
        End Using
        Dim lstItem As New ListItem("--- Chọn ---", "")
        ddlLoaihanhvi.Items.Insert(0, lstItem)
    End Sub
    Protected Function Save() As Boolean
        Using data As New ThanhTraLaoDongEntities
            Dim p As New ThanhTraLaoDongModel.DanhMucKienNghi
            Try
                p.NoiDungKN = txtMota.Text.Trim()
                p.RootCauHoi = ddlLoaihanhvi.SelectedValue.ToString()
                data.DanhMucKienNghis.AddObject(p)
                data.SaveChanges()
                Dim knID = p.KienNghiID
                'Them du lieu moi
                GetParentNode()
                Dim arrID() As String = IDChecked.Split(",")
                If arrID.Length > 0 Then
                    For i As Integer = 1 To arrID.Length - 1
                        Dim chkn As New ThanhTraLaoDongModel.CauHoiKienNghi
                        chkn.DanhMucKienNghiId = knID
                        chkn.CauHoiId = arrID(i)
                        data.CauHoiKienNghis.AddObject(chkn)
                        data.SaveChanges()
                        p = Nothing
                    Next
                End If

                'Insert_App_Log("Insert  Danhmuchanhvi:" & txtTitle.Text.Trim & "", Function_Name.Danhmuchanhvi, Audit_Type.Create, Request.ServerVariables("REMOTE_ADDR"), Session("UserName"))
                Return True
            Catch ex As Exception
                log4net.Config.XmlConfigurator.Configure()
                log.Error("Error error " & AddTabSpace(1) & Session("Username") & AddTabSpace(1) & "IP:" & GetIPAddress(), ex)
                Return False
            End Try
        End Using
    End Function
    Protected Sub ResetControl()
        txtMota.Text = ""
    End Sub
#End Region
#Region "Event for control "
    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If Save() Then
            Excute_Javascript("AlertboxRedirect('Cập nhật dữ liệu thành công.','../../Page/KienNghi/List.aspx');", Me.Page, True)
        Else
            Excute_Javascript("Alertbox('Cập nhật thất bại.');", Me.Page, True)
        End If
    End Sub
    
    Protected Sub btnReset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReset.Click
        ResetControl()
    End Sub
    Protected Sub btnBack_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnBack.Click
        Response.Redirect("List.aspx")
    End Sub
#End Region
#Region "Tree menu "
    'Load du lieu cho menu cha
    Protected Sub FillParentMenu()
        Using data As New ThanhTraLaoDongEntities
            Dim query = (From q In data.CauHoiHierarchies Where String.IsNullOrEmpty(q.ParentId) Order By q.ParentId Ascending Select q)
            Dim dtList = query.ToList()
            trvMenu.Nodes.Clear()
            Dim tnParentRoot As New TreeNode()
            tnParentRoot.Text = "Tất cả"
            tnParentRoot.Value = ""
            trvMenu.Nodes.Add(tnParentRoot)
            tnParentRoot.ExpandAll()
            For i As Integer = 0 To dtList.Count - 1
                Dim tnParent As New TreeNode()
                tnParent.Text = dtList(i).Title
                tnParent.Value = dtList(i).CauHoiId.ToString()
                tnParentRoot.ChildNodes.Add(tnParent)
                tnParent.Collapse()
                FillChild(tnParent, tnParent.Value)

                If CheckSelectedMenu(dtListDMKNSelect, dtList(i).CauHoiId) = 1 Then
                    tnParent.Checked = True
                    tnParent.ExpandAll()
                Else
                    tnParent.Checked = False

                End If
            Next
        End Using
    End Sub
    'Load du lieu cho menu con
    Protected Sub FillChild(ByVal Parent As TreeNode, ByVal Value As String)
        Using data As New ThanhTraLaoDongEntities
            Dim query = From q In data.CauHoiHierarchies Where q.ParentId = Value Order By q.CauHoiId Ascending Select q
            Dim dtList = query.ToList()
            Parent.ChildNodes.Clear()
            For i As Integer = 0 To dtList.Count - 1
                Dim child As New TreeNode()
                child.Text = dtList(i).Title
                child.Value = dtList(i).CauHoiId.ToString()
                Parent.ChildNodes.Add(child)
                child.Collapse()
                FillChild(child, child.Value)
                If CheckSelectedMenu(dtListDMKNSelect, dtList(i).CauHoiId) = 1 Then
                    child.Checked = True
                    child.ExpandAll()
                    Parent.ExpandAll()
                End If
            Next
        End Using
    End Sub
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
    Protected Function CheckSelectedMenu(ByVal dtList As List(Of CauHoiKienNghi), ByVal Value As String) As Integer
        Dim Result As Integer = 0
        If Not dtList Is Nothing Then
            For i As Integer = 0 To dtList.Count - 1
                If dtList(i).CauHoiId.Equals(Value) Then
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
    Function GetRootCauHoi(ByVal _CauHoiId As String) As String
        Using _data As New ThanhTraLaoDongEntities
            Dim p = (From q In _data.CauHoiHierarchies Where q.CauHoiId = _CauHoiId Select q.CauHoiId, q.ParentId).FirstOrDefault
            If Not p Is Nothing AndAlso Not p.ParentId Is Nothing Then
                Return GetRootCauHoi(p.ParentId)
            Else
                Return p.CauHoiId
            End If
        End Using
    End Function
#End Region
End Class
