Imports System.Data
Imports ThanhTraLaoDongModel
Imports Cls_Common
Imports SecurityService
Partial Class Control_LoaiHinhSanXuat_Edit
    Inherits System.Web.UI.UserControl
    Private Shared ReadOnly log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
    Dim level As String = -1
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
            If Not Request.QueryString("LoaiHinhSXId").ToString.Equals("0") Then
                hidID.Value = Request.QueryString("LoaiHinhSXId")
                getLoaiHinhSanXuat()
                ShowData()
            End If
        End If
    End Sub
    Protected Sub ShowData()
        Using data As New ThanhTraLaoDongEntities
            Dim p As LoaiHinhSanXuat = (From q In data.LoaiHinhSanXuats Where q.LoaiHinhSXId = hidID.Value Select q).SingleOrDefault
            If Not p Is Nothing Then
                txtCode.Text = IIf(IsNothing(p.Code) = True, "", p.Code)
                txtTitle.Text = IIf(IsNothing(p.Title) = True, "", p.Title)
                txtIDLoaihinhSX.Text = IIf(IsNothing(p.ParentID) = True, "", p.ParentID)
                Dim h = (From k In data.LoaiHinhSanXuats Where k.LoaiHinhSXId = p.ParentID Select k).FirstOrDefault
                If Not h Is Nothing Then
                    txtLoaihinhSX.Text = IIf(IsNothing(p.ParentID) = True, "", p.Title)
                End If

                txtSoGioLamThemToiDaTheoNam.Text = IIf(IsNothing(p.SoGioLamThemToiDaTheoNam) = True, "", p.SoGioLamThemToiDaTheoNam)
            End If
        End Using
    End Sub

    Protected Sub getLoaiHinhSanXuat()
        'ddlParent.Items.Clear()
        'ddlParent.AppendDataBoundItems = True
        'ddlParent.Items.Insert(0, New ListItem("---Tất cả---", "0"))
        'Using data As New ThanhTraLaoDongEntities
        '    Dim p As List(Of LoaiHinhSanXuat) = (From q In data.LoaiHinhSanXuats Where q.ParentID = 0 Select q).ToList
        '    ddlParent.DataValueField = "LoaiHinhSXId"
        '    ddlParent.DataTextField = "Title"
        '    RecursiveFillTree(p, 0)
        'End Using
    End Sub
    Private Sub RecursiveFillTree(ByVal dtParent As List(Of LoaiHinhSanXuat), ByVal parentID As Integer)
        'level += 1
        ''on the each call level increment 1
        'Dim appender As New StringBuilder()
        'For j As Integer = 0 To level - 1
        '    appender.Append("&nbsp;&nbsp;&nbsp;&nbsp;")
        'Next
        'If level > 0 Then
        '    appender.Append("|__")
        'End If
        'Using data As New ThanhTraLaoDongEntities
        '    Dim dv As List(Of LoaiHinhSanXuat) = (From q In data.LoaiHinhSanXuats Where q.ParentID = parentID Select q).ToList
        '    Dim i As Integer

        '    If dv.Count > 0 Then
        '        For i = 0 To dv.Count - 1
        '            Dim itm As New ListItem(Server.HtmlDecode(appender.ToString() + dv.Item(i).Code.ToString() + " - " + dv.Item(i).Title.ToString()), dv.Item(i).LoaiHinhSXId.ToString())
        '            itm.Attributes.Add("class", "ItemActived")
        '            ddlParent.Items.Add(itm)
        '            RecursiveFillTree(dtParent, Integer.Parse(dv.Item(i).LoaiHinhSXId.ToString()))
        '        Next
        '    End If
        'End Using
        'level -= 1
        'on the each function end level will decrement by 1
    End Sub

    Protected Function CheckExist(ByVal strCode As String, ByVal intID As Integer) As Boolean
        Using data As New ThanhTraLaoDongEntities
            Dim p As LoaiHinhSanXuat = (From q In data.LoaiHinhSanXuats Where Not (q.LoaiHinhSXId = intID) And q.Code.Equals(strCode) Select q).SingleOrDefault
            If Not p Is Nothing Then
                Return True
            Else
                Return False
            End If
        End Using

    End Function
#End Region
#Region "Event for control"
    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If CheckExist(txtCode.Text.Trim, hidID.Value) Then
            Excute_Javascript("Alertbox('Mã loại hình sản xuất đã tồn tại');", Me.Page, True)
        Else
            Using data As New ThanhTraLaoDongEntities
                Dim p As LoaiHinhSanXuat = (From q In data.LoaiHinhSanXuats Where q.LoaiHinhSXId = hidID.Value).SingleOrDefault
                Try
                    p.LoaiHinhSXId = hidID.Value
                    p.Code = txtCode.Text.Trim()
                    p.Title = txtTitle.Text.Trim()
                    If String.IsNullOrEmpty(txtIDLoaihinhSX.Text.Trim) = False Then
                        p.ParentID = CInt(txtIDLoaihinhSX.Text.Trim)
                    Else
                        p.ParentID = 0
                    End If
                    p.SoGioLamThemToiDaTheoNam = txtSoGioLamThemToiDaTheoNam.Text.Trim()
                    data.SaveChanges()
                    Insert_App_Log("Update Loai hinh san xuat:" & txtTitle.Text.Trim & "", Function_Name.LoaiHinhSanXuat, Audit_Type.Edit, Request.ServerVariables("REMOTE_ADDR"), Session("UserName"))
                    Excute_Javascript("Alertbox('Cập nhật dữ liệu thành công.');window.location ='../../Page/LoaiHinhSanXuat/List.aspx';", Me.Page, True)
                Catch ex As Exception
                    log4net.Config.XmlConfigurator.Configure()
                    log.Error("Error error " & AddTabSpace(1) & Session("Username") & AddTabSpace(1) & "IP:" & GetIPAddress(), ex)
                    Excute_Javascript("Alertbox('Cập nhật thất bại.');", Me.Page, True)
                End Try
            End Using
        End If
    End Sub
    Protected Sub btnHuy_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnHuy.Click
        Response.Redirect("List.aspx")
    End Sub
#End Region
End Class
