Imports System.Data
Imports ThanhTraLaoDongModel
Imports Cls_Common
Imports SecurityService
Partial Class Control_DanhMucCauHoi_List
    Inherits System.Web.UI.UserControl
    Private Shared ReadOnly log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
    Dim level As String = -1
#Region "Sub and Function"
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
            'getMenu()
        End If
    End Sub

    'Protected Sub getMenu()
    '    Using data As New ThanhTraLaoDongEntities
    '        Dim p As List(Of CauHoiHierarchy) = (From q In data.CauHoiHierarchies Where q.ParentId Is Nothing Order By q.CauHoiId Select q).ToList
    '        With ddlCauHoiParent
    '            .Items.Clear()
    '            .DataValueField = "CauHoiId"
    '            .DataTextField = "Title"
    '            .DataSource = p
    '            .DataBind()
    '        End With

    '        ddlCauHoiParent.Items.Insert(0, New ListItem("---Tất cả---", "0"))
    '    End Using
    'End Sub
     
    'Private Sub RecursiveFillTree(ByVal dtParent As List(Of CauHoiHierarchy), ByVal parentID As String)
    '    level += 1
    '    'on the each call level increment 1
    '    Dim appender As New StringBuilder()

    '    For j As Integer = 0 To level - 1

    '        appender.Append("&nbsp;&nbsp;&nbsp;&nbsp;")
    '    Next
    '    If level > 0 Then
    '        appender.Append("|__")
    '    End If

    '    Using data As New ThanhTraLaoDongEntities
    '        Dim dv As List(Of CauHoiHierarchy) = (From q In data.CauHoiHierarchies Where q.ParentId.Equals(parentID) Select q).ToList
    '        Dim i As Integer

    '        If dv.Count > 0 Then
    '            For i = 0 To dv.Count - 1
    '                Dim itm As New ListItem(Server.HtmlDecode(appender.ToString() + dv.Item(i).Title.ToString()), dv.Item(i).CauHoiId.ToString())
    '                'If Check_Selected(dv.Item(i).MenuId) = False Then
    '                '    itm.Attributes.Add("class", "ItemDisabled")
    '                'Else
    '                '    itm.Attributes.Add("class", "ItemActived")
    '                'End If

    '                ddlCauHoiParent.Items.Add(itm)
    '                'ddlMenuChange.Items.Add(itm)
    '                RecursiveFillTree(dtParent, Integer.Parse(dv.Item(i).CauHoiId.ToString()))
    '            Next
    '        End If
    '    End Using
    '    level -= 1
    '    'on the each function end level will decrement by 1
    'End Sub
    'Private Sub RecursiveFillTree(ByVal parentID As String)
    '    level += 1
    '    'on the each call level increment 1
    '    Dim appender As New StringBuilder()
    '    For j As Integer = 0 To level - 1
    '        appender.Append("&nbsp;&nbsp;&nbsp;&nbsp;")
    '    Next
    '    If level > 0 Then
    '        appender.Append("|__")
    '    End If
    '    Using data As New ThanhTraLaoDongEntities
    '        Dim dv As List(Of CauHoiHierarchy) = (From q In data.CauHoiHierarchies Where q.ParentId = parentID Select q).ToList
    '        Dim i As Integer

    '        If dv.Count > 0 Then
    '            For i = 0 To dv.Count - 1
    '                Dim itm As New ListItem(Server.HtmlDecode(appender.ToString() + dv.Item(i).Title.ToString()), dv.Item(i).CauHoiId.ToString())
    '                ddlCauHoiSub.Items.Add(itm)
    '                RecursiveFillTree(dv.Item(i).CauHoiId.ToString())
    '            Next
    '        End If
    '    End Using
    '    level -= 1
    'End Sub

    Protected Sub FillParentMenu()
        Using data As New ThanhTraLaoDongEntities
            Dim query = (From q In data.CauHoiHierarchies Where String.IsNullOrEmpty(q.ParentId) Order By q.Sort Ascending Select q)
            Dim dtList = query.ToList()
            trvMenu.Nodes.Clear()
            For i As Integer = 0 To dtList.Count - 1
                Dim tnParent As New TreeNode()
                tnParent.Text = dtList(i).Title
                tnParent.Value = dtList(i).CauHoiId.ToString()
                trvMenu.Nodes.Add(tnParent)
                tnParent.Collapse()
                FillChild(tnParent, tnParent.Value)
                Dim cauhoiid As String = Session("CauhoiId")
                If dtList(i).CauHoiId.Equals(cauhoiid) Then
                    tnParent.ExpandAll()
                Else
                    tnParent.Collapse()
                End If
            Next
        End Using
    End Sub
    
    'Load du lieu cho menu con
    Protected Sub FillChild(ByVal Parent As TreeNode, ByVal Value As String)
        Using data As New ThanhTraLaoDongEntities
            'Dim query = From q In data.CauHoiHierarchies Where q.ParentId = Value And (q.IsTieuChiMoiBC = False Or q.IsTieuChiMoiBC Is Nothing) Order By q.CauHoiId Ascending Select q
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
            Next
        End Using
    End Sub

    Protected Sub trvMenu_SelectedNodeChanged(sender As Object, e As System.EventArgs) Handles trvMenu.SelectedNodeChanged
        Using data As New ThanhTraLaoDongEntities
            hidCauHoiID.Value = trvMenu.SelectedValue

            Dim chh = (From q In data.CauHoiHierarchies Where q.CauHoiId.Equals(hidCauHoiID.Value)).FirstOrDefault
            txtTieuDeDayDu.Text = chh.Title
            txtTieuDeVietTat.Text = chh.CauHoiVietTat
            'txtParentId.Text = chh.ParentId
            'chkLaViPham.Checked = IIf(Not IsNothing(chh.IsBaoCao), chh.IsBaoCao, False)
            txtTieuChi.Text = IIf(IsNothing(chh.IsTieuChi) OrElse chh.IsTieuChi.Equals(""), "", chh.IsTieuChi)
            txtDieuKien.Text = IIf(IsNothing(chh.ChuoiDieuKien) OrElse chh.ChuoiDieuKien.Equals(""), "", chh.ChuoiDieuKien)
            txtSapXep.Text = IIf(IsNothing(chh.Sort) OrElse chh.Sort.Equals(""), "", chh.Sort)
            txtCauHoiId.Text = IIf(IsNothing(chh.CauHoiId), "", chh.CauHoiId)
            txtParentId.Text = IIf(IsNothing(chh.ParentId), "", chh.ParentId)
            txtParentBCId.Text = IIf(IsNothing(chh.ParentBCId), "", chh.ParentBCId)
            chkIsTieuChiMoiBC.Checked = IIf(IsNothing(chh.IsTieuChiMoiBC), False, chh.IsTieuChiMoiBC)
            Session("CauhoiId") = chh.ParentId
            'If IsNothing(chh.ParentId) Then
            '    ddlCauHoiParent.SelectedValue = chh.CauHoiId
            '    ddlCauHoiSub.Items.Clear()
            '    ddlCauHoiParent.Items.Insert(0, New ListItem("---Tất cả---", "0"))
            '    ddlCauHoiSub.SelectedValue = "0"
            'Else
            '    Dim chhParent = (From q In data.CauHoiHierarchies Where q.CauHoiId.Equals(chh.ParentId)).FirstOrDefault
            '    ddlCauHoiParent.SelectedValue = chhParent.ParentId
            '    ddlCauHoiSub.Items.Clear()
            '    RecursiveFillTree(chh.ParentId)
            '    ddlCauHoiSub.SelectedValue = chh.CauHoiId
            'End If
        End Using
    End Sub
 
    Protected Sub btnSave_Click(sender As Object, e As System.EventArgs) Handles btnSave.Click
        Try
            Using data As New ThanhTraLaoDongEntities
                Dim chh = (From q In data.CauHoiHierarchies Where q.CauHoiId.Equals(hidCauHoiID.Value)).FirstOrDefault
                chh.Title = txtTieuDeDayDu.Text.Trim
                chh.CauHoiVietTat = txtTieuDeVietTat.Text.Trim
                'chh.ParentId = txtParentId.Text.Trim
                'chh.IsBaoCao = chkLaViPham.Checked
                If Not txtTieuChi.Text.Trim.Equals("") And Not txtTieuChi.Text.Trim.Equals("0") Then
                    chh.IsTieuChi = CInt(txtTieuChi.Text.Trim)
                Else
                    chh.IsTieuChi = Nothing
                End If

                chh.ChuoiDieuKien = IIf(txtDieuKien.Text.Trim.Equals(""), "", txtDieuKien.Text.Trim)
                If (txtSapXep.Text.Trim.Equals("")) Then
                    chh.Sort = 0
                Else
                    chh.Sort = CInt(txtSapXep.Text.Trim)
                End If
                'chh.Sort = IIf(txtSapXep.Text.Trim.Equals(""), 0, CInt(txtSapXep.Text.Trim))
                chh.CauHoiId = txtCauHoiId.Text.Trim()
                chh.ParentId = txtParentId.Text.Trim
                chh.ParentBCId = txtParentBCId.Text.Trim
                chh.IsTieuChiMoiBC = chkIsTieuChiMoiBC.Checked
                data.SaveChanges()
                txtTieuDeDayDu.Text = ""
                txtTieuDeVietTat.Text = ""
                'txtTieuChi.Text = ""
                'txtDieuKien.Text = ""
                'txtSapXep.Text = ""
                hidCauHoiID.Value = ""
                'chkIsTieuChiMoiBC.Checked = False
                'txtParentId.Text = ""
                'chkLaViPham.Checked = False
                FillParentMenu()
                'Excute_Javascript("Alertbox('Cập nhật thành công');", Me.Page, True)

            End Using
        Catch ex As Exception
            Excute_Javascript("Alertbox('Cập nhật thất bại.');", Me.Page, True)
        End Try
    End Sub
#End Region


   
End Class
