Imports System.Data
Imports ThanhTraLaoDongModel
Imports Cls_Common
Imports SecurityService
Imports System.Globalization

Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Web
Imports System.Web.UI

Partial Class Control_Users_List
    Inherits System.Web.UI.UserControl
    Private Shared ReadOnly log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
    Public TypeUser As New Dictionary(Of String, String)
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

            BindAccountType()
            BindToGrid()
            If HasPermission(Function_Name.User, Session("RoleID"), 0, Audit_Type.Delete) = True Then
                btnDelete.Attributes.Add("onclick", "return confirmMultiDelete('" & btnDelete.ClientID & "');")
            Else
                btnDelete.Enabled = False
            End If
        End If
    End Sub
    Private Sub BindAccountType()
        'Bind AccountType
        Using Data As New ThanhTraLaoDongEntities
            Dim p = (From q In Data.vAccountTypes Select q).ToList
            With ddlTypeUser
                .DataValueField = "ID"
                .DataTextField = "Name"
                .DataSource = p
                .DataBind()
            End With
            'set account type into dictionary
            For Each i As vAccountType In p
                TypeUser(i.Id) = i.Name
            Next
            Session("TypeUser") = TypeUser
        End Using
        Dim lstItem As New ListItem("--- Tất cả ---", 0)
        ddlTypeUser.Items.Insert(0, lstItem)
        'set default la bien tap
        ' ddlTypeUser.SelectedValue = 1
        'set account type into dictionary



    End Sub
    Private Sub BindToGrid(Optional ByVal iPage As Integer = 1, Optional ByVal strSearch As String = "")
        Using data As New ThanhTraLaoDongEntities
            Dim arrSearch() As String = {iPage.ToString, strSearch}
            ViewState("search") = arrSearch
            'So ban ghi muon the hien tren trang
            Dim intPag_Size As Int32 = drpPage_Size.SelectedValue
            Dim qr = From q In data.Users Order By q.IsUser Descending, q.UserName Ascending Select q


            If strSearch.Trim.Length >= 1 Then
                qr = qr.Where(Function(q) q.UserName.Contains(strSearch.Trim) Or q.FullName.Contains(strSearch.Trim))
            End If


            If ddlTypeUser.SelectedValue <> 0 Then
                qr = qr.Where(Function(q) q.IsUser = ddlTypeUser.SelectedValue)
            End If

            If Not String.IsNullOrEmpty(txtLastLogin.Text.Trim) Then
                Dim strbLastlogin As String = txtLastLogin.Text & " 00:00:01"
                Dim streLastlogin As String = txtLastLogin.Text & " 23:59:59"
                Dim dtmbLastlogin As DateTime = DateTime.ParseExact(strbLastlogin, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture)
                Dim dtmeLastlogin As DateTime = DateTime.ParseExact(streLastlogin, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture)
                qr = qr.Where(Function(q) q.LastLogin >= dtmbLastlogin And q.LastLogin <= dtmeLastlogin)
            End If

            If Not String.IsNullOrEmpty(txtCreatedFrom.Text.Trim) Then
                Dim dtmCreatedFrom As Date = StringToDate(txtCreatedFrom.Text.Trim, "dd/mm/yyyy")
                qr = qr.Where(Function(q) q.Created >= dtmCreatedFrom)
            End If

            If Not String.IsNullOrEmpty(txtCreatedTo.Text.Trim) Then
                Dim dtmCreatedTo As Date = StringToDate(txtCreatedTo.Text.Trim, "dd/mm/yyyy").AddDays(1)
                qr = qr.Where(Function(q) q.Created <= dtmCreatedTo)
            End If

            Dim p = qr.Skip((iPage - 1) * intPag_Size).Take(intPag_Size).ToList
            Dim strKey_Name() As String = {"UserName", "UserId", "IsUser", "IsActivated"}
            'Tong so ban ghi
            hidCount.Value = qr.Count
            Create_Pager(hidCount.Value, iPage, intPag_Size, 10)
            With grdShow
                .DataKeyNames = strKey_Name
                .DataSource = p
                .DataBind()
            End With
            lblTotal.Text = "Hiển thị " + (((iPage - 1) * intPag_Size) + 1).ToString + " đến " + (((iPage - 1) * intPag_Size) + grdShow.Rows.Count).ToString + " trong tổng số " + hidCount.Value.ToString + " bản ghi."
        End Using
    End Sub
    Private Function getUserType(ByVal intUserType As Integer) As String
        For index = 0 To Cls_Common.ArrUser_Type.Length - 1
            If index = intUserType Then
                Return Cls_Common.ArrUser_Type(index)
            End If
        Next
        Return ""
    End Function
    Sub Create_Pager(ByVal Total_Record As Integer, ByVal Page_Index As Integer, ByVal Page_Size As Integer, ByVal Page2Show As Integer)
        Dim TotalPage As Integer = IIf((Total_Record Mod Page_Size) = 0, Total_Record / Page_Size, Total_Record \ Page_Size + 1)
        'lu lai tong so ban ghi
        hidIndex_page.Value = TotalPage
        'gan lai curPage de set active
        hidCur_Page.Value = Page_Index
        'generate ra left page
        Dim cPageGenerate_left As IEnumerable(Of Integer)
        If Page_Index <= Page2Show Then
            cPageGenerate_left = Enumerable.Range(1, Page_Index)
        Else
            cPageGenerate_left = Enumerable.Range(Page_Index - Page2Show, Page2Show)
        End If
        'generate ra right page
        Dim cPageGenerate_Right As IEnumerable(Of Integer)
        If Page_Index + Page2Show <= TotalPage Then
            cPageGenerate_Right = Enumerable.Range(Page_Index, Page2Show + 1)
        Else
            cPageGenerate_Right = Enumerable.Range(Page_Index, TotalPage - Page_Index + 1)
        End If
        'union 2 range va bind to Grid
        With rptPage
            .DataSource = cPageGenerate_left.Union(cPageGenerate_Right)
            .DataBind()
        End With
    End Sub
    Protected Sub rptPage_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles rptPage.ItemDataBound
        Dim lnkTitle As LinkButton
        Dim ScriptManager As System.Web.UI.ScriptManager = System.Web.UI.ScriptManager.GetCurrent(Me.Page)
        lnkTitle = e.Item.FindControl("lnkTitle")
        ScriptManager.RegisterAsyncPostBackControl(lnkTitle)
        If e.Item.DataItem = hidCur_Page.Value Then
            lnkTitle.Text = "<span class='current'>" & e.Item.DataItem & "</span>"
        Else
            lnkTitle.Text = "<span>" & e.Item.DataItem & "</span>"
        End If
        lnkTitle.ToolTip = e.Item.DataItem
    End Sub
    Protected Sub lnkTitle_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim lnkTile As LinkButton = CType(sender, LinkButton)
        Dim arrSearch() As String
        arrSearch = ViewState("search")
        BindToGrid(CInt(lnkTile.ToolTip.ToString), arrSearch(1).ToString)
        lnkLast.Enabled = True
        lnkFirst.Enabled = True
        If CInt(lnkTile.ToolTip) = hidIndex_page.Value Then
            lnkLast.Enabled = False
        ElseIf CInt(lnkTile.ToolTip) = 1 Then
            lnkFirst.Enabled = False
        End If
    End Sub
    Protected Sub lnkFirst_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkFirst.Click
        If hidCur_Page.Value > 1 Then
            hidCur_Page.Value = hidCur_Page.Value - 1
        End If
        Dim arrSearch() As String
        arrSearch = ViewState("search")
        BindToGrid(CInt(hidCur_Page.Value.ToString), arrSearch(1).ToString)
    End Sub
    Protected Sub lnkLast_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkLast.Click
        hidCur_Page.Value = hidCur_Page.Value + 1
        Dim arrSearch() As String
        arrSearch = ViewState("search")
        BindToGrid(CInt(hidCur_Page.Value.ToString), arrSearch(1).ToString)
    End Sub
#End Region
#Region "Event for control"

    Protected Sub chkAll_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs)
        For Each itm As GridViewRow In grdShow.Rows
            Dim chkItem As CheckBox = CType(itm.Cells(0).FindControl("chkItem"), CheckBox)
            If chkItem.Enabled = True Then
                chkItem.Checked = CType(sender, CheckBox).Checked
            End If
        Next
    End Sub
    Protected Sub lnkbtnDelete_Click(ByVal sender As Object, ByVal e As EventArgs)
        Using data As New ThanhTraLaoDongEntities
            Dim intId As Integer
            Dim strLogName As String = ""
            intId = grdShow.DataKeys(hidID.Value)("UserId").ToString
            Dim q = (From p In data.Users Where p.UserId = intId Select p).FirstOrDefault
            Try
                data.Users.DeleteObject(q)
                data.SaveChanges()
                Insert_App_Log("Delete User:" & q.UserName & "", Function_Name.User, Audit_Type.Delete, Request.ServerVariables("REMOTE_ADDR"), Session("UserName"))
                Excute_Javascript("Alertbox('Xóa dữ liệu thành công.');", Me.Page, True)
            Catch ex As Exception
                log4net.Config.XmlConfigurator.Configure()
                log.Error("Error error " & AddTabSpace(1) & Session("Username") & AddTabSpace(1) & "IP:" & GetIPAddress(), ex)
                Excute_Javascript("Alertbox('Xóa thất bại. Do người dùng đang được tham chiếu đến từ bảng khác.');", Me.Page, True)
            End Try
            Dim arrSearch() As String
            arrSearch = ViewState("search")
            BindToGrid(CInt(arrSearch(0).ToString), arrSearch(1).ToString)
        End Using
    End Sub
    Protected Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelete.Click

        Using data As New ThanhTraLaoDongEntities
            Dim intId As Integer = 0
            Dim intCount As Integer
            Dim intTotal As Integer
            Try
                For Each item As GridViewRow In grdShow.Rows
                    Dim chkItem As New CheckBox
                    chkItem = CType(item.FindControl("chkItem"), CheckBox)
                    If chkItem.Checked Then
                        intTotal += 1
                        intId = grdShow.DataKeys(item.RowIndex)("UserId").ToString
                        Dim q = (From p In data.Users Where p.UserId = intId Select p).FirstOrDefault
                        Try
                            data.Users.DeleteObject(q)
                            data.SaveChanges()
                            intCount += 1
                            Insert_App_Log("Delete User:" & q.UserName & "", Function_Name.User, Audit_Type.Delete, Request.ServerVariables("REMOTE_ADDR"), Session("UserName"))
                        Catch ex As Exception
                        End Try
                    End If
                Next
                If intCount > 0 Then
                    Excute_Javascript("Alertbox('Xóa dữ liệu thành công. " & intCount.ToString & " /" & intTotal.ToString & " record.');", Me.Page, True)
                Else
                    Excute_Javascript("Alertbox('Xóa thất bại.');", Me.Page, True)
                End If
            Catch ex As Exception
                log4net.Config.XmlConfigurator.Configure()
                log.Error("Error error " & AddTabSpace(1) & Session("Username") & AddTabSpace(1) & "IP:" & GetIPAddress(), ex)
                Excute_Javascript("Alertbox('Xóa thất bại.');", Me.Page, True)
            End Try
            Dim arrSearch() As String
            arrSearch = ViewState("search")
            BindToGrid(CInt(arrSearch(0).ToString), arrSearch(1).ToString)
        End Using
    End Sub
    Protected Sub grdShow_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles grdShow.PageIndexChanging
        grdShow.PageIndex = e.NewPageIndex
        BindToGrid()
    End Sub
    Protected Sub grdShow_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdShow.RowDataBound
        If e.Row.RowIndex >= 0 Then
            'Dim p As ThanhTraLaoDongModel.User = CType(e, ThanhTraLaoDongModel.User)

            Dim lnkbtnDelete As LinkButton = CType(e.Row.FindControl("lnkbtnDelete"), LinkButton)
            Dim ScriptManager As System.Web.UI.ScriptManager = System.Web.UI.ScriptManager.GetCurrent(Me.Page)
            ScriptManager.RegisterAsyncPostBackControl(lnkbtnDelete)

            lnkbtnDelete.Attributes.Add("onclick", "return ComfirmDialog('" + drpMessage.Items(0).Text + "', 0,'" + lnkbtnDelete.ClientID + "','" + e.Row.RowIndex.ToString + "',1);")
            Dim hplEdit As HyperLink = CType(e.Row.FindControl("hplEdit"), HyperLink)
            hplEdit.NavigateUrl = "../../Page/Users/Edit.aspx?Userid=" & grdShow.DataKeys(e.Row.RowIndex)("UserId").ToString
            Dim hplUsername As HyperLink = CType(e.Row.FindControl("hplUsername"), HyperLink)
            hplUsername.NavigateUrl = "../../Page/Users/Detail.aspx?Userid=" & grdShow.DataKeys(e.Row.RowIndex)("UserId").ToString
            hplUsername.Text = grdShow.DataKeys(e.Row.RowIndex)("UserName").ToString
            Dim lblAccountType As Label = CType(e.Row.FindControl("lblAccountType"), Label)
            lblAccountType.Text = Session("TypeUser")((grdShow.DataKeys(e.Row.RowIndex)("IsUser").ToString))
            Dim lblIsActivated As Label = CType(e.Row.FindControl("lblIsActivated"), Label)
            If Not IsNothing(grdShow.DataKeys(e.Row.RowIndex)("IsActivated")) AndAlso grdShow.DataKeys(e.Row.RowIndex)("IsActivated") = True Then
                lblIsActivated.Text = "Đã kích hoạt"
            Else
                lblIsActivated.Text = "Chưa kích hoạt"
            End If


            Dim chkItem As CheckBox = CType(e.Row.FindControl("chkItem"), CheckBox)

            'Permission
            If HasPermission(Function_Name.User, Session("RoleID"), 0, Audit_Type.Delete) = True Then
                ' lnkbtnDelete.Attributes.Add("onclick", "return ComfirmDialog('" + drpMessage.Items(0).Text + "', 0,'" + lnkbtnDelete.ClientID + "','" + e.Row.RowIndex.ToString + "',1);")
                lnkbtnDelete.Attributes.Add("onclick", "return ComfirmDialog('" + drpMessage.Items(0).Text + "', 0,'" + lnkbtnDelete.ClientID + "','" + e.Row.RowIndex.ToString + "',1);")
            Else
                lnkbtnDelete.Enabled = False
                chkItem.Enabled = False
            End If

            'Permission
            hplEdit.Enabled = HasPermission(Function_Name.User, Session("RoleID"), 0, Audit_Type.Edit)

            'Permission
            hplUsername.Enabled = HasPermission(Function_Name.User, Session("RoleID"), 0, Audit_Type.ViewContent)
        End If
    End Sub
#End Region
#Region "Search"
    Protected Sub btnFilter_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFilter.Click
        BindToGrid(1, txtTitleFilter.Text.Trim())
    End Sub


    Protected Sub drpPage_Size_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles drpPage_Size.SelectedIndexChanged
        Dim arrSearch() As String
        arrSearch = ViewState("search")
        BindToGrid(1, arrSearch(1).ToString)
    End Sub
    Protected Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClear.Click
        ddlTypeUser.SelectedIndex = -1
        txtTitleFilter.Text = ""
        txtCreatedFrom.Text = ""
        txtCreatedTo.Text = ""
        txtLastLogin.Text = ""
        BindToGrid()
    End Sub
#End Region

    Protected Sub btnExportToExcel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExportToExcel.Click
        Dim strCol As String() = {"STT", "Họ tên", "Tên truy cập", "Email", "Giới tính", "Ngày sinh"}
        Dim arrSex As String() = {"", "Nam", "Nữ", "Khác"}
        Using data As New ThanhTraLaoDongEntities

            Dim qr = (From q In data.Users Order By q.IsUser Descending, q.UserName Ascending Select q)

            If txtTitleFilter.Text.Trim.Length >= 1 Then
                qr = qr.Where(Function(q) q.UserName.Contains(txtTitleFilter.Text.Trim) Or q.FullName.Contains(txtTitleFilter.Text.Trim))
            End If

            If ddlTypeUser.SelectedValue <> 0 Then
                qr = qr.Where(Function(q) q.IsUser = ddlTypeUser.SelectedValue)
            End If

            If Not String.IsNullOrEmpty(txtLastLogin.Text.Trim) Then
                Dim strbLastlogin As String = txtLastLogin.Text & " 00:00:01"
                Dim streLastlogin As String = txtLastLogin.Text & " 23:59:59"
                Dim dtmbLastlogin As DateTime = DateTime.ParseExact(strbLastlogin, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture)
                Dim dtmeLastlogin As DateTime = DateTime.ParseExact(streLastlogin, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture)
                qr = qr.Where(Function(q) q.LastLogin >= dtmbLastlogin And q.LastLogin <= dtmeLastlogin)
            End If

            If Not String.IsNullOrEmpty(txtCreatedFrom.Text.Trim) Then
                Dim dtmCreatedFrom As Date = StringToDate(txtCreatedFrom.Text.Trim, "dd/mm/yyyy")
                qr = qr.Where(Function(q) q.Created >= dtmCreatedFrom)
            End If

            If Not String.IsNullOrEmpty(txtCreatedTo.Text.Trim) Then
                Dim dtmCreatedTo As Date = StringToDate(txtCreatedTo.Text.Trim, "dd/mm/yyyy")
                qr = qr.Where(Function(q) q.Created <= dtmCreatedTo)
            End If

            Dim x = qr.ToList

            'Export to excel
            Response.ContentType = "application/csv"
            Response.Charset = ""
            Response.AddHeader("Content-Disposition", "attachment;filename=Danhsachnguoidung.xls")
            Response.ContentEncoding = Encoding.Unicode
            Response.BinaryWrite(Encoding.Unicode.GetPreamble())

            Dim sb As StringBuilder = New StringBuilder()

            For i As Integer = 0 To strCol.Length - 1
                sb.Append(strCol(i))
                If (i < strCol.Length - 1) Then
                    sb.Append(vbTab)
                End If
            Next

            Response.Write(sb.ToString() & vbCrLf)
            Response.Flush()

            For i As Integer = 0 To x.Count - 1
                sb = New StringBuilder()
                sb.Append((i + 1).ToString())
                sb.Append(vbTab)

                If (Not x(i).FullName Is Nothing AndAlso x(i).FullName.ToString <> "") Then
                    sb.Append(x(i).FullName.ToString())
                Else
                    sb.Append("")
                End If
                sb.Append(vbTab)

                If (Not x(i).UserName Is Nothing AndAlso x(i).UserName.ToString <> "") Then
                    sb.Append(x(i).UserName.ToString())
                Else
                    sb.Append("")
                End If
                sb.Append(vbTab)

                If (Not x(i).Email Is Nothing AndAlso x(i).Email.ToString <> "") Then
                    sb.Append(x(i).Email.ToString())
                Else
                    sb.Append("")
                End If
                sb.Append(vbTab)

                If (Not x(i).UsersProfile Is Nothing AndAlso Not x(i).UsersProfile.Sex Is Nothing AndAlso x(i).UsersProfile.Sex.ToString <> "") Then
                    sb.Append(arrSex(x(i).UsersProfile.Sex.ToString()))
                Else
                    sb.Append("")
                End If
                sb.Append(vbTab)

                If (Not x(i).UsersProfile Is Nothing AndAlso Not x(i).UsersProfile.DateOfBirth Is Nothing AndAlso x(i).UsersProfile.DateOfBirth.ToString <> "") Then
                    sb.Append(CType(x(i).UsersProfile.DateOfBirth.ToString(), DateTime).ToString("dd/MM/yyyy"))
                Else
                    sb.Append("")
                End If
                Response.Write(sb.ToString() & vbCrLf)
                Response.Flush()
            Next
            Response.End()
        End Using
    End Sub
End Class
