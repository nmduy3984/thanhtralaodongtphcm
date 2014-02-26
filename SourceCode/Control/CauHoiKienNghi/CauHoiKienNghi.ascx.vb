Imports System.Data
Imports ThanhTraLaoDongModel
Imports Cls_Common
Imports SecurityService
Partial Class Control_CauHoiKienNghi_CauHoiKienNghi
    Inherits System.Web.UI.UserControl
    Private Shared ReadOnly log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    'Luu nhung MenuId duoc check
    Private IDChecked As String = ""
    Private dtListDMKNSelect As List(Of CauHoiKienNghi)

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Dim script As ScriptManager = ScriptManager.GetCurrent(Me.Page)
            If Not script Is Nothing AndAlso script.IsInAsyncPostBack Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.Page.GetType, "duyjs", "ajaxJquery()", True)
            Else
                Me.Page.ClientScript.RegisterStartupScript(Me.Page.GetType(), "duyjs", String.Concat("Sys.Application.add_load(function(){", "ajaxJquery()", "});"), True)
            End If
            FillParentMenu()
            'Load danh mục kiến nghị
            LoadDMKN()
        End If
    End Sub
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
    Protected Sub LoadDMKN()
        Using data As New ThanhTraLaoDongEntities
            Dim kn = (From q In data.DanhMucKienNghis Select q.KienNghiID, q.NoiDungKN).ToList
            With ddlDMKNID
                .DataTextField = "NoiDungKN"
                .DataValueField = "KienNghiID"
                .DataSource = kn
                .DataBind()
                .Items.Insert(0, New ListItem("----- Chọn -----", 0))
                .SelectedValue = 0
            End With

        End Using
    End Sub
    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Using data As New ThanhTraLaoDongEntities
            Try
                'Xoa du lieu cu
                Dim DMKNId As Integer = ddlDMKNID.SelectedValue
                Dim h As List(Of ThanhTraLaoDongModel.CauHoiKienNghi) = (From t In data.CauHoiKienNghis Where t.DanhMucKienNghiId = DMKNId Select t).ToList
                For Each k As ThanhTraLaoDongModel.CauHoiKienNghi In h
                    data.CauHoiKienNghis.DeleteObject(k)
                    data.SaveChanges()
                Next
                'Them du lieu moi
                GetParentNode()
                Dim arrID() As String = IDChecked.Split(",")
                Dim flagUpdateRootCauHoi As Integer = 0
                If arrID.Length > 0 Then
                    For i As Integer = 1 To arrID.Length - 1
                        Dim p As New ThanhTraLaoDongModel.CauHoiKienNghi
                        p.DanhMucKienNghiId = ddlDMKNID.SelectedValue
                        p.CauHoiId = arrID(i)
                        'update lai root Cau hoi
                        If flagUpdateRootCauHoi = 0 Then
                            Dim strResult = GetRootCauHoi(p.CauHoiId)
                            If Not strResult.Equals("") Then
                                flagUpdateRootCauHoi = 1
                                Dim dmkn = (From q In data.DanhMucKienNghis Where q.KienNghiID = ddlDMKNID.SelectedValue).FirstOrDefault
                                dmkn.RootCauHoi = strResult
                            End If
                        End If
                        data.CauHoiKienNghis.AddObject(p)
                        data.SaveChanges()
                        p = Nothing
                    Next
                End If

                Excute_Javascript("Alertbox('Cập nhật dữ liệu thành công.');", Me.Page, True) 'window.location ='../../Page/Users/List.aspx';
            Catch ex As Exception
                log4net.Config.XmlConfigurator.Configure()
                log.Error("Error error " & AddTabSpace(1) & Session("Username") & AddTabSpace(1) & "IP:" & GetIPAddress(), ex)
                Excute_Javascript("Alertbox('Cập nhật thất bại." & ex.Message & "');", Me.Page, True)
            End Try
        End Using
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

    Protected Sub btnHuy_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnHuy.Click
        Response.Redirect(Request.Url.ToString())
    End Sub

    Protected Sub ddlDMKNID_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlDMKNID.DataBound
        Dim ddl As ListBox = CType(sender, ListBox)
        If Not IsNothing(ddl) Then
            For Each li As ListItem In ddl.Items
                li.Attributes("title") = li.Text
            Next
        End If
    
    End Sub
    Protected Sub ddlDMKNID_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlDMKNID.SelectedIndexChanged
        ShowData()
    End Sub
    Protected Sub ShowData()
        Using data As New ThanhTraLaoDongEntities
            Dim DMKNId As Integer = ddlDMKNID.SelectedValue
            dtListDMKNSelect = (From h In data.CauHoiKienNghis Where h.DanhMucKienNghiId = DMKNId Select h).ToList
        End Using
        FillParentMenu()
    End Sub
End Class
