Imports System.Data
Imports Cls_Common
Imports SecurityService
Imports ThanhTraLaoDongModel
Imports System.Data.Objects

Partial Class Control_DoanhNghiep_List
    Inherits System.Web.UI.UserControl
    Private Shared ReadOnly log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
    Public Flag As Boolean = True
#Region "Sub and Function"

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        '' Check xem User do da co doanh nghiep chua
        'Dim userName As String = Session("UserName")
        'Using data As New ThanhTraLaoDongEntities
        '    Dim count As Integer = (From q In data.DoanhNghieps
        '                           Where q.NguoiTao.Contains(userName)
        '                           Select q.DoanhNghiepId).Count()
        '    If count = 0 Then
        '        Flag = False
        '        Excute_Javascript("AlertboxRedirect('User chưa tạo doanh nghiệp hãy tạo doanh nghiệp trước.','Create.aspx');", Me.Page, True)
        '    End If
        'End Using
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack And Flag Then
            Dim script As ScriptManager = ScriptManager.GetCurrent(Me.Page)
            If Not script Is Nothing AndAlso script.IsInAsyncPostBack Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.Page.GetType, "duyjs", "ajaxJquery()", True)
                ScriptManager.RegisterStartupScript(Me.Page, Me.Page.GetType, "duyjs1", "ajaxJqueryToolTip()", True)
            Else
                Me.Page.ClientScript.RegisterStartupScript(Me.Page.GetType(), "duyjs", String.Concat("Sys.Application.add_load(function(){", "ajaxJquery()", "});"), True)
                Me.Page.ClientScript.RegisterStartupScript(Me.Page.GetType(), "duyjs1", String.Concat("Sys.Application.add_load(function(){", "ajaxJqueryToolTip()", "});"), True)
            End If
            hidTinhThanhTraSo.Value = Session("TinhThanhTraSo")
            btnDelete.Attributes.Add("onclick", "return confirmMultiDelete('" & btnDelete.ClientID & "');")
            LoadData()
            getMenu()
            BindToGrid()
        End If
    End Sub

    Protected Sub LoadData()
        Using data As New ThanhTraLaoDongEntities

            '' Load thông tin Tỉnh theo User login tại đây
            Dim userID As Integer = CInt(Session("UserId"))
            Dim isUser As Integer = CInt(Session("IsUser"))

            '' Load thông tin mặc định cho Huyện
            Dim tinhID As Integer = CInt(hidTinhThanhTraSo.Value)
            Dim lstHuyen = (From q In data.Huyens
                            Where q.TinhId = tinhID
                            Order By q.TenHuyen
                            Select New With {.Value = q.HuyenId, .Text = q.TenHuyen}).ToList()

            With ddlHuyen
                .AppendDataBoundItems() = True
                .DataTextField = "Text"
                .DataValueField = "Value"
                .DataSource = lstHuyen
                .DataBind()
                .Items.Insert(0, New ListItem(Cls_Common.Str_Opt_TatCa, -1))
            End With

            ' Load thông tin Loại hình Doanh Nghiệp
            Dim lstLHDN = (From q In data.LoaiHinhDoanhNghieps
                        Select New With {.Value = q.LoaiHinhDNId, .Text = q.TenLoaiHinhDN})

            With ddlLoaiHinhDN
                .Items.Clear()
                .AppendDataBoundItems() = True
                .DataTextField = "Text"
                .DataValueField = "Value"
                .DataSource = lstLHDN
                .DataBind()
                .Items.Insert(0, New ListItem(Cls_Common.Str_Opt_TatCa, -1))
            End With

            'Set value mặc định cho ListRadioButton Công đoàn
            rblCDCS.SelectedIndex = 0

            '' Clear hết Data những biến đã tạo ra 
             lstLHDN = Nothing
            'lstLHSX = Nothing
        End Using
    End Sub

    Protected Sub getMenu()
        ddlLoaiHinhSX.Items.Clear()
        Using data As New ThanhTraLaoDongEntities
            Dim p As List(Of LoaiHinhSanXuat) = (From q In data.LoaiHinhSanXuats Select q).ToList
            ddlLoaiHinhSX.DataValueField = "MenuID"
            ddlLoaiHinhSX.DataTextField = "Title"

            RecursiveFillTree(p, 0)
            ddlLoaiHinhSX.Items.Insert(0, New ListItem("---Tất cả---", "-1"))
        End Using
    End Sub
    Dim level As Integer = 0
    Private Sub RecursiveFillTree(ByVal dtParent As List(Of LoaiHinhSanXuat), ByVal parentID As Integer)
        level += 1
        'on the each call level increment 1
        Dim appender As New StringBuilder()

        For j As Integer = 0 To level - 1

            appender.Append("&nbsp;&nbsp;&nbsp;&nbsp;")
        Next
        If level > 0 Then
            appender.Append("|__")
        End If

        Using data As New ThanhTraLaoDongEntities
            Dim dv As List(Of LoaiHinhSanXuat) = (From q In data.LoaiHinhSanXuats
                                                  Where q.ParentID = parentID Select q).ToList
            Dim i As Integer

            If dv.Count > 0 Then
                For i = 0 To dv.Count - 1
                    Dim itm As New ListItem(Server.HtmlDecode(appender.ToString() + dv.Item(i).Title.ToString()), dv.Item(i).LoaiHinhSXId.ToString())
                    ddlLoaiHinhSX.Items.Add(itm)
                    RecursiveFillTree(dtParent, Integer.Parse(dv.Item(i).LoaiHinhSXId.ToString()))
                Next
            End If
        End Using
        level -= 1
        'on the each function end level will decrement by 1
    End Sub

    'Binding data to Girdview 
    Private Sub BindToGrid(Optional ByVal iPage As Integer = 1 _
                          , Optional ByVal strMaDN As String = "" _
                          , Optional ByVal strTenDN As String = "" _
                          , Optional ByVal strTinh As Integer = -1 _
                          , Optional ByVal strHuyen As Integer = -1 _
                          , Optional ByVal strLoaiHinhDN As Integer = -1 _
                          , Optional ByVal strLoaiHinhSX As Integer = -1 _
                          , Optional ByVal strFromDate As Integer = 1900 _
                          , Optional ByVal strToDate As Integer = 9999 _
                          , Optional ByVal strIsCongDoan As Integer = -1 _
                          , Optional ByVal strSoDKKD As String = "" _
                          , Optional ByVal iTongLaoDong As Integer = 0 _
                          , Optional ByVal iTongLoiNhuan As Integer = 0 _
                          , Optional ByVal strUserNameFind As String = "")
        Try
            Using data As New ThanhTraLaoDongEntities
                Dim arrSearch() As String = {iPage.ToString, strMaDN.ToString, strTenDN.ToString, strTinh.ToString,
                                             strHuyen.ToString, strLoaiHinhDN.ToString, strLoaiHinhSX.ToString,
                                             strFromDate.ToString, strToDate.ToString, strIsCongDoan.ToString, strSoDKKD.ToString, iTongLaoDong.ToString, iTongLoiNhuan.ToString, strUserNameFind}

                ViewState("search") = arrSearch
                Dim UserName As String = Session("UserName")
                Dim IsUser As Integer
                Integer.TryParse(Session("IsUser"), IsUser)

                'So ban ghi muon the hien tren trang
                Dim intPag_Size As Int32 = drpPage_Size.SelectedValue
                Dim p As List(Of uspDoanhNghiepSelectAll_Result) = _
                    data.uspDoanhNghiepSelectAll(strMaDN _
                                                , strTenDN _
                                                , strTinh _
                                                , strHuyen _
                                                , strLoaiHinhDN _
                                                , strLoaiHinhSX _
                                                , strFromDate _
                                                , strToDate _
                                                , strSoDKKD _
                                                , iTongLaoDong _
                                                , iTongLoiNhuan _
                                                , strIsCongDoan _
                                                , UserName _
                                                , strUserNameFind _
                                                , IsUser _
                                                , 2 _
                                                , iPage, intPag_Size).ToList

                Dim strKey_Name() As String = {"DoanhNghiepId", "MaDN", "TenDN", "HasPermission", "NguoiTao"}
                'Tong so ban ghi
                If p.Count > 0 Then
                    hidCount.Value = p.FirstOrDefault.Total()
                    Create_Pager(hidCount.Value, iPage, intPag_Size, 10)
                Else
                    hidCount.Value = 0
                    With rptPage
                        .DataSource = Nothing
                        .DataBind()
                    End With
                End If
                With grdShow
                    .DataKeyNames = strKey_Name
                    .DataSource = p
                    .DataBind()
                End With
                If (hidCount.Value > 0) Then
                    lblTotal.Text = "Hiển thị " + (((iPage - 1) * intPag_Size) + 1).ToString("#,#") + " đến " + (((iPage - 1) * intPag_Size) + grdShow.Rows.Count).ToString("#,#") + " trong tổng số " + CInt(hidCount.Value).ToString("#,#") + " bản ghi."
                Else
                    lblTotal.Text = ""
                End If
            End Using
        Catch ex As Exception
            Excute_Javascript("Alertbox('Đã xảy ra lỗi trong lúc Load dữ liệu:" + ex.Message + "');", Me.Page, True)
        End Try

    End Sub

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
        lnkTitle = e.Item.FindControl("lnkTitle")
        Dim ScriptManager As System.Web.UI.ScriptManager = System.Web.UI.ScriptManager.GetCurrent(Me.Page)
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
        BindToGrid(lnkTile.ToolTip, arrSearch(1), arrSearch(2), arrSearch(3), arrSearch(4), arrSearch(5), arrSearch(6), arrSearch(7), arrSearch(8), arrSearch(9), arrSearch(10), arrSearch(11), arrSearch(12))
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
        BindToGrid(hidCur_Page.Value, arrSearch(1), arrSearch(2), arrSearch(3), arrSearch(4), arrSearch(5), arrSearch(6), arrSearch(7), arrSearch(8), arrSearch(9), arrSearch(10), arrSearch(11), arrSearch(12))

    End Sub
    Protected Sub lnkLast_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkLast.Click
        hidCur_Page.Value = hidCur_Page.Value + 1

        Dim arrSearch() As String
        arrSearch = ViewState("search")
        BindToGrid(hidCur_Page.Value, arrSearch(1), arrSearch(2), arrSearch(3), arrSearch(4), arrSearch(5), arrSearch(6), arrSearch(7), arrSearch(8), arrSearch(9), arrSearch(10), arrSearch(11), arrSearch(12))

    End Sub
#End Region
#Region "Event for control"
    Protected Sub lnkbtnDelete_Click(ByVal sender As Object, ByVal e As EventArgs)
        Dim intId As Integer
        Dim strLogName As String = ""
        Using data As New ThanhTraLaoDongEntities
            Dim isUser As String = Session("IsUser")
            Dim UserName As String = Session("UserName")
            intId = grdShow.DataKeys(hidID.Value)("DoanhNghiepId").ToString
            Dim dn = (From q In data.DoanhNghieps
                      Where q.DoanhNghiepId = intId
                      Select q).FirstOrDefault()
            If Not IsNothing(dn) Then
                If isUser = UserType.Admin Or UserName = dn.NguoiTao Then '' Có quyền Xóa                    
                    Try
                        Dim pn = (From a In data.PhieuNhapHeaders Where a.DoanhNghiepId = intId).ToList()
                        If pn.Count = 0 Then
                            'Dim qddn = (From a In data.QuyetDinhTTDoanhNghieps Where a.DoanhNghiepId = intId).ToList
                            'If qddn.Count > 0 Then
                            '    For i As Integer = 0 To qddn.Count - 1
                            '        Dim SQD As String = qddn(i).QuyetDinhTT
                            '        Dim qdtt = (From a In data.QuyetDinhThanhTras
                            '           Where a.SoQuyetDinh.Equals(SQD) Select a).FirstOrDefault
                            '        If qdtt Is Nothing Then
                            '            qdtt.IsEdited = False
                            '            data.QuyetDinhThanhTras.AddObject(qdtt)
                            '            data.SaveChanges()
                            '        End If
                            '        data.QuyetDinhTTDoanhNghieps.DeleteObject(qddn(i))
                            '        data.SaveChanges()
                            '    Next
                            'End If
                            data.DoanhNghieps.DeleteObject(dn)
                            data.SaveChanges()
                            Insert_App_Log("Delete Doanh Nghiep:" & intId & "", Function_Name.DoanhNghiepXLP, Audit_Type.Delete, Request.ServerVariables("REMOTE_ADDR"), Session("UserName"))
                            Excute_Javascript("Alertbox('Xóa dữ liệu thành công.');", Me.Page, True)
                        Else
                            Excute_Javascript("Alertbox('Xóa doanh nghiệp thất bại. Doanh nghiệp đang được tham chiếu từ biên bản thanh tra hoặc phiếu kiểm tra.');", Me.Page, True)
                        End If
                    Catch ex As Exception
                        log4net.Config.XmlConfigurator.Configure()
                        log.Error("Error error " & AddTabSpace(1) & Session("Username") & AddTabSpace(1) & "IP:" & GetIPAddress(), ex)
                        Excute_Javascript("Alertbox('Xóa doanh nghiệp thất bại (" & ex.Message & ").');", Me.Page, True)
                    End Try
                Else
                    Excute_Javascript("Alertbox('Bạn không có quyền xóa với doanh nghiệp này.');", Me.Page, True)
                End If
            Else
                Excute_Javascript("Alertbox('Không tồn tại doanh nghiệp này.');", Me.Page, True)
            End If
        End Using
        Dim arrSearch() As String
        arrSearch = ViewState("search")
        BindToGrid(1, arrSearch(1), arrSearch(2), arrSearch(3), arrSearch(4), arrSearch(5), arrSearch(6), arrSearch(7), arrSearch(8), arrSearch(9), arrSearch(10), arrSearch(11))
    End Sub
    Protected Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        Dim intId As Integer = 0
        Dim intCount As Integer
        Dim intTotal As Integer
        Dim isUser As String = Session("IsUser")
        Dim UserName As String = Session("UserName")
        Dim strDN1 As String = ""
        Dim strDN2 As String = ""
        Dim strDN3 As String = ""
        Dim strCombine As String = ""
        Using data As New ThanhTraLaoDongEntities
            Try
                For Each item As GridViewRow In grdShow.Rows
                    Dim chkItem As New CheckBox
                    chkItem = CType(item.FindControl("chkItem"), CheckBox)
                    If chkItem.Checked Then
                        intTotal += 1
                        intId = grdShow.DataKeys(item.RowIndex)("DoanhNghiepId").ToString
                        Dim dn = (From q In data.DoanhNghieps
                                  Where q.DoanhNghiepId = intId
                                  Select q).FirstOrDefault()
                        If Not IsNothing(dn) Then
                            If isUser = UserType.Admin Or UserName = dn.NguoiTao Then '' Có quyền Xóa 
                                Dim pn = (From a In data.PhieuNhapHeaders Where a.DoanhNghiepId = intId).ToList()
                                If pn.Count = 0 Then
                                    'Dim qddn = (From a In data.QuyetDinhTTDoanhNghieps Where a.DoanhNghiepId = intId).ToList
                                    'If qddn.Count > 0 Then
                                    '    For i As Integer = 0 To qddn.Count - 1
                                    '        Dim SQD As String = qddn(i).QuyetDinhTT
                                    '        Dim qdtt = (From a In data.QuyetDinhThanhTras
                                    '           Where a.SoQuyetDinh.Equals(SQD) Select a).FirstOrDefault
                                    '        If qdtt Is Nothing Then
                                    '            qdtt.IsEdited = False
                                    '            data.QuyetDinhThanhTras.AddObject(qdtt)
                                    '            data.SaveChanges()
                                    '        End If
                                    '        data.QuyetDinhTTDoanhNghieps.DeleteObject(qddn(i))
                                    '        data.SaveChanges()
                                    '    Next
                                    'End If
                                    data.DoanhNghieps.DeleteObject(dn)
                                    data.SaveChanges()
                                    intCount += 1
                                    Insert_App_Log("Delete Doanh Nghiep:" & intId & "", Function_Name.DoanhNghiepXLP, Audit_Type.Delete, Request.ServerVariables("REMOTE_ADDR"), Session("UserName"))
                                Else
                                    strDN1 += dn.TenDoanhNghiep & "; "
                                End If
                            Else
                                strDN2 += dn.TenDoanhNghiep & "; "
                            End If
                        Else
                            strDN3 += dn.TenDoanhNghiep & "; "
                        End If
                    End If
                Next
                strCombine += If(Not strDN1.Equals(""), "Doanh nghiệp (" & strDN1 & ") đang được tham chiếu từ biên bản thanh tra hoặc phiếu kiểm tra.\n", "")
                strCombine += If(Not strDN2.Equals(""), "Doanh nghiệp (" & strDN2 & ") bạn không có quyền xóa.\n", "")
                strCombine += If(Not strDN3.Equals(""), "Doanh nghiệp (" & strDN3 & ") không tồn tại.", "")

                If intCount > 0 Then
                    Excute_Javascript("Alertbox('Xóa dữ liệu thành công. " & intCount.ToString & " /" & intTotal.ToString & " record. " & If(Not strCombine.Equals(""), "Các doanh nghiệp không xóa được do:\n" & strCombine, "") & "');", Me.Page, True)
                Else
                    Excute_Javascript("Alertbox('Xóa doanh nghiệp thất bại do:\n" & If(Not strCombine.Equals(""), strCombine, "") & "');", Me.Page, True)
                End If
            Catch ex As Exception
                log4net.Config.XmlConfigurator.Configure()
                log.Error("Error error " & AddTabSpace(1) & Session("Username") & AddTabSpace(1) & "IP:" & GetIPAddress(), ex)
                Excute_Javascript("Alertbox('Xóa doanh nghiệp thất bại (" & ex.Message & ").');", Me.Page, True)
            End Try
        End Using

        Dim arrSearch() As String
        arrSearch = ViewState("search")
        BindToGrid(1, arrSearch(1), arrSearch(2), arrSearch(3), arrSearch(4), arrSearch(5), arrSearch(6), arrSearch(7), arrSearch(8), arrSearch(9), arrSearch(10), arrSearch(11))

    End Sub
    Protected Sub grdShow_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdShow.RowDataBound
        If e.Row.RowIndex >= 0 Then
            Dim hplHoSoDN As HyperLink = CType(e.Row.FindControl("hplHoSoDN"), HyperLink)
            hplHoSoDN.NavigateUrl = "../../Page/DoanhNghiepXLP/HoSoDoanhNghiep.aspx?DNId=" & grdShow.DataKeys(e.Row.RowIndex)("DoanhNghiepId")
            'Permission
            Dim lnkbtnDelete As LinkButton = CType(e.Row.FindControl("lnkbtnDelete"), LinkButton)
            Dim hplEdit As HyperLink = CType(e.Row.FindControl("hplEdit"), HyperLink)
            Dim chkItem As CheckBox = CType(e.Row.FindControl("chkItem"), CheckBox)
            Dim policy As String = grdShow.DataKeys(e.Row.RowIndex)("HasPermission")

            'Xử lý phân quyền chọn từ hệ thống và điều kiện riêng từ store
            Dim ScriptManager As System.Web.UI.ScriptManager = System.Web.UI.ScriptManager.GetCurrent(Me.Page)
            ScriptManager.RegisterAsyncPostBackControl(lnkbtnDelete)

            'Permission
            Dim permissiondel As Boolean = HasPermission(Function_Name.DoanhNghiepXLP, Session("RoleID"), 0, Audit_Type.Delete)
            If (Not policy Is Nothing AndAlso policy = "1") Or
                (Not policy Is Nothing AndAlso Not policy = "1" And permissiondel) Then
                lnkbtnDelete.Enabled = True
                chkItem.Enabled = True
                lnkbtnDelete.Attributes.Add("onclick", "return ComfirmDialog('" + drpMessage.Items(0).Text + "', 0,'" + lnkbtnDelete.ClientID + "','" + e.Row.RowIndex.ToString + "',1);")
            ElseIf Not policy Is Nothing AndAlso Not policy = "1" And Not permissiondel Then
                lnkbtnDelete.Enabled = False
                chkItem.Enabled = False
            End If
            Dim permissionedit As Boolean = HasPermission(Function_Name.DoanhNghiepXLP, Session("RoleID"), 0, Audit_Type.Edit)
            If (Not policy Is Nothing AndAlso policy = "1") Or
                (Not policy Is Nothing AndAlso Not policy = "1" And permissionedit) Then
                hplEdit.Enabled = True
                hplEdit.NavigateUrl = "../../Page/DoanhNghiepXLP/Edit.aspx?DNId=" & grdShow.DataKeys(e.Row.RowIndex)("DoanhNghiepId")
            ElseIf Not policy Is Nothing AndAlso Not policy = "1" And Not permissionedit Then
                hplEdit.Enabled = False
            End If

            Dim hplTenDN As HyperLink = CType(e.Row.FindControl("hplTenDN"), HyperLink)
            hplTenDN.Text = grdShow.DataKeys(e.Row.RowIndex)("TenDN")
            hplTenDN.NavigateUrl = "../../Page/DoanhNghiepXLP/Detail.aspx?DNId=" & grdShow.DataKeys(e.Row.RowIndex)("DoanhNghiepId")
            'Permission
            hplTenDN.Enabled = HasPermission(Function_Name.DoanhNghiepXLP, Session("RoleID"), 0, Audit_Type.ViewContent)

            'Xử lý ẩn link khi user hiện tại đăng nhập không phải là user tạo doanh nghiệp
            If Not (grdShow.DataKeys(e.Row.RowIndex)("NguoiTao").ToString().Contains(Session("Username")) _
                         Or Session("IsUser").Equals(UserType.Admin)) Then
                lnkbtnDelete.Enabled = False
                chkItem.Enabled = False

                hplEdit.Enabled = False

                hplTenDN.Enabled = False
                hplTenDN.Attributes.Add("style", "color:gray !important")
                hplHoSoDN.Enabled = False
            End If
        End If
    End Sub
    Protected Sub drpPage_Size_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles drpPage_Size.SelectedIndexChanged
        Dim arrSearch() As String
        arrSearch = ViewState("search")
        BindToGrid(1, arrSearch(1), arrSearch(2), arrSearch(3), arrSearch(4), arrSearch(5), arrSearch(6), arrSearch(7), arrSearch(8), arrSearch(9), arrSearch(10), arrSearch(11))
    End Sub

 #End Region
#Region "Search"
    Protected Sub btnFilter_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFilter.Click
 
        Dim intNamTu As Integer
        If txtTuNam.Text.Trim() = "" Then
            intNamTu = 1900
        Else
            intNamTu = CInt(txtTuNam.Text.Trim)
        End If

        Dim intNamDen As Integer
        If txtDenNam.Text.Trim() = "" Then
            intNamDen = 9999
        Else
            intNamDen = CInt(txtDenNam.Text.Trim)
        End If

         BindToGrid(1, "", txtTenDN.Text.Trim, CInt(hidTinhThanhTraSo.Value), ddlHuyen.SelectedValue, _
                      ddlLoaiHinhDN.SelectedValue, ddlLoaiHinhSX.SelectedValue, intNamTu, intNamDen, _
                      rblCDCS.SelectedValue, txtSoDKKD.Text.Trim, ddlTongSoLaoDong.SelectedValue, ddlLoiNhuan.SelectedValue, txtNguoiTao.Text.Trim())
    End Sub
#End Region

    Protected Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClear.Click
        txtDenNam.Text = ""
        txtTuNam.Text = ""
        txtTenDN.Text = ""
        txtSoDKKD.Text = ""
         ddlHuyen.SelectedIndex = -1
        ddlLoaiHinhDN.SelectedIndex = -1
        rblCDCS.SelectedValue = -1
        ddlLoaiHinhSX.SelectedIndex = -1
        ddlTongSoLaoDong.ClearSelection()
        ddlLoiNhuan.ClearSelection()
        txtNguoiTao.Text = ""
        BindToGrid()
    End Sub
End Class
