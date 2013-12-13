Imports System.Data
Imports ThanhTraLaoDongModel
Imports Cls_Common
Imports System.Globalization

Partial Class Control_SysParameter_Create
    Inherits System.Web.UI.UserControl
    Private Shared ReadOnly log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
#Region "Sub and Function "
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Dim script As ScriptManager = ScriptManager.GetCurrent(Me.Page)
            If Not script Is Nothing AndAlso script.IsInAsyncPostBack Then
                 ScriptManager.RegisterStartupScript(Me.Page, Me.Page.GetType, "duyjs1", "ajaxJqueryToolTip()", True)
            Else
                 Me.Page.ClientScript.RegisterStartupScript(Me.Page.GetType(), "duyjs1", String.Concat("Sys.Application.add_load(function(){", "ajaxJqueryToolTip()", "});"), True)
            End If
            load_SysParameter()
        End If
    End Sub
    Private Sub load_SysParameter()
        Using Data As New ThanhTraLaoDongEntities
            Try
                Dim q = (From p In Data.SYS_PARAMETERS Select Name = p.Name, Val = p.Val, Val2 = p.Val2, Type = If(p.Type = 1, "String", If(p.Type = 2, "Number", If(p.Type = 3, "DateTime", If(p.Type = 4, "Boonlean", "Unknown")))), Description = p.Description, ID = p.ID, Activated = p.Activated, IsSystem = p.IsSystem).ToList
                Dim arr As String() = {"ID", "Val", "Val2", "Type", "IsSystem"}
                With grdShow
                    .DataSource = q
                    .DataKeyNames = arr
                    .DataBind()
                End With
            Catch ex As Exception
                Excute_Javascript("Alertbox('Có lỗi trong quá trình load dữ liệu - " + ex.Message + "');", Me.Page, True)
            End Try
        End Using
    End Sub
#End Region
#Region "Event for control "
    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        'if hidID.value="" => create, # "" => edit

        Using data As New ThanhTraLaoDongEntities
            Dim p As New ThanhTraLaoDongModel.SYS_PARAMETERS

            If hidID.Value.Trim.Length >= 1 Then
                Dim ID As Integer = CInt(hidID.Value)
                p = (From q In data.SYS_PARAMETERS Where q.ID = ID Select q).SingleOrDefault
            End If
            Try
                p.Name = txtName.Text.Trim()
                p.Type = CInt(ddlType.SelectedValue)
                Select Case CInt(ddlType.SelectedValue)
                    Case 4
                        p.Val = CBool(ddlBoolean.SelectedValue)
                    Case 3
                        p.Val = StringToDateTime(txtVal.Text.Trim, "0:0")
                        If txtVal2.Text.Trim.Length >= 1 Then
                            p.Val2 = StringToDateTime(txtVal2.Text.Trim, "0:0")
                        End If
                    Case 2
                        p.Val = Double.Parse(txtVal.Text.Trim)
                        If txtVal2.Text.Trim.Length >= 1 Then
                            p.Val2 = Double.Parse(txtVal2.Text.Trim)
                        End If
                    Case Else
                        p.Val = txtVal.Text.Trim()
                        p.Val2 = txtVal2.Text.Trim()
                End Select
                p.Description = txtDescription.Text.Trim()
                p.Activated = chkIsActivated.Checked
                p.IsSystem = chkisSys.Checked
                If hidID.Value.Trim.Length <= 0 Then
                    data.SYS_PARAMETERS.AddObject(p)
                End If
                data.SaveChanges()
                load_SysParameter()
                Excute_Javascript("resetForm();Alertbox('Cập nhật dữ liệu thành công');checkHideStatusddlTypeBoolean();", Me.Page, True)
            Catch ex As Exception
                log4net.Config.XmlConfigurator.Configure()
                log.Error("Error error " & AddTabSpace(1) & Session("Username") & AddTabSpace(1) & "IP:" & GetIPAddress(), ex)
                Excute_Javascript("checkHideStatusddlTypeBoolean();Alertbox('Cập nhật thất bại - " + ex.Message + "');", Me.Page, True)
            End Try
        End Using
    End Sub

    Protected Sub lnkbtnEdit_Click(ByVal sender As Object, ByVal e As EventArgs)
        Using Data As New ThanhTraLaoDongEntities
            Try
                Dim ID As Integer = hidID.Value
                Dim q = (From p In Data.SYS_PARAMETERS Where p.ID = ID Select p).SingleOrDefault
                If Not q Is Nothing Then
                    Dim type As Integer = 0
                    txtName.Text = q.Name.ToString
                    txtName.Enabled = False
                    type = IIf(IsNothing(q.Type), 0, q.Type)
                    ddlType.SelectedValue = type
                    Select Case type
                        Case 4
                            ddlBoolean.SelectedValue = q.Val.ToString
                            txtVal.Text = ""
                            txtVal2.Text = ""
                        Case 3
                            'cdate("#"+ q.Val+"#")
                            'Dim culture As IFormatProvider = New CultureInfo("vi-VN", True)
                            'txtVal.Text = DateTime.ParseExact(q.Val, "dd/MM/yyyy", culture, DateTimeStyles.NoCurrentDateDefault).ToString
                            txtVal.Text = String.Format("{0:dd/MM/yyyy}", CDate("#" + q.Val + "#"))
                            If q.Val2.Trim.Length >= 1 Then
                                txtVal2.Text = String.Format("{0:dd/MM/yyyy}", CDate("#" + q.Val2 + "#"))
                            End If
                        Case 2
                            txtVal.Text = Double.Parse(q.Val)
                            If txtVal2.Text.Trim.Length >= 1 Then
                                txtVal2.Text = Double.Parse(q.Val2)
                            End If
                        Case Else
                            txtVal.Text = q.Val.ToString
                            txtVal2.Text = q.Val2
                    End Select
                    txtDescription.Text = q.Description
                    chkIsActivated.Checked = q.Activated
                    chkisSys.Checked = q.IsSystem
                    chkisSys.Enabled = Not q.IsSystem
                    Excute_Javascript("setTypeData();", Me.Page, True)

                End If
            Catch ex As Exception
                Excute_Javascript("Alertbox('Có lỗi trong quá trình load dữ liệu - " + ex.Message + "');", Me.Page, True)
            End Try
        End Using
    End Sub

    Protected Sub lnkbtnDelete_Click(ByVal sender As Object, ByVal e As EventArgs)
        Using Data As New ThanhTraLaoDongEntities
            Try
                Dim ID As Integer = hidID.Value
                Dim q = (From p In Data.SYS_PARAMETERS Where p.ID = ID Select p).SingleOrDefault
                If Not q Is Nothing Then
                    Data.SYS_PARAMETERS.DeleteObject(q)
                    Data.SaveChanges()
                    load_SysParameter()
                    Excute_Javascript("Alertbox('Xóa dữ liệu thành công');", Me.Page, True)
                Else
                    Excute_Javascript("Alertbox('Dữ liệu cần xóa không còn tồn tại');", Me.Page, True)
                End If
            Catch ex As Exception
                Excute_Javascript("Alertbox('Có lỗi trong quá trình xóa dữ liệu - " + ex.Message + "');", Me.Page, True)
            End Try
        End Using
    End Sub

    Protected Sub grdShow_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdShow.RowDataBound
        If e.Row.RowIndex >= 0 Then
            Dim lnkbtnDelete As LinkButton = CType(e.Row.FindControl("lnkbtnDelete"), LinkButton)

            If CBool(grdShow.DataKeys(e.Row.RowIndex)("IsSystem")) Then
                lnkbtnDelete.Enabled = False
                lnkbtnDelete.Attributes.Add("class", "aspNetDisabled TextBox")
            Else
                lnkbtnDelete.Attributes.Add("onclick", "return confirmDelete('" + grdShow.DataKeys(e.Row.RowIndex)("ID").ToString + "');")
            End If

            Dim lblVal As Label = CType(e.Row.FindControl("lblVal"), Label)
            Dim lblVal2 As Label = CType(e.Row.FindControl("lblVal2"), Label)

            Select Case grdShow.DataKeys(e.Row.RowIndex)("Type").ToString
                Case "DateTime"
                    lblVal.Text = String.Format("{0:dd/MM/yyyy}", CDate("#" + grdShow.DataKeys(e.Row.RowIndex)("Val").ToString + "#"))
                    If grdShow.DataKeys(e.Row.RowIndex)("Val").ToString.Length >= 1 Then
                        lblVal2.Text = String.Format("{0:dd/MM/yyyy}", CDate("#" + grdShow.DataKeys(e.Row.RowIndex)("Val2").ToString + "#"))
                    End If
                Case 2

                Case Else

            End Select


            Dim lnkbtnEdit As LinkButton = CType(e.Row.FindControl("lnkbtnEdit"), LinkButton)
            lnkbtnEdit.Attributes.Add("onclick", "Edit('" + grdShow.DataKeys(e.Row.RowIndex)("ID").ToString + "');")
        End If
    End Sub
#End Region
End Class

