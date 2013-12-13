Imports ThanhTraLaoDongModel
Imports Cls_Common
Imports SecurityService

Partial Class Control_Report_PhieuKetQuaThanhTra
    Inherits System.Web.UI.UserControl

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        ''check function event
        'If Not HasPermission(Function_Name.User, Session("RoleID"), 0, Audit_Type.Create) Then
        '    Excute_Javascript("AlertboxRedirect('" + Str_not_right_to_access + "','" + ResolveUrl("~/Page/Homepage.aspx") + "');", Me.Page, True)
        'End If
    End Sub
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
            hidTinhThanhTraSo.Value = Session("TinhThanhTraSo")
            LoadData()
        End If

    End Sub

    Private Sub LoadData()
        Using data As New ThanhTraLaoDongEntities
            If hidTinhThanhTraSo.Value > 0 Then
                Dim lst = (From q In data.Tinhs Where q.TinhId = hidTinhThanhTraSo.Value
                           Order By q.TenTinh Ascending
                            Select New With {.Value = q.TinhId, .Text = q.TenTinh}).ToList
                With ddlProvince
                    .Items.Clear()
                    .AppendDataBoundItems = True
                    .DataTextField = "Text"
                    .DataValueField = "Value"
                    .DataSource = lst
                    .DataBind()
                End With
                '' Load thông tin mặc định cho Huyện
                Dim tinhId = lst(0).Value
                Dim lsthuyen = (From a In data.Huyens Where a.TinhId = tinhId
                                Order By a.TenHuyen
                                Select New With {.Value = a.HuyenId, .Text = a.TenHuyen}).ToList
                With ddlHuyen
                    .Items.Clear()
                    .AppendDataBoundItems() = True
                    .DataTextField = "Text"
                    .DataValueField = "Value"
                    .DataSource = lsthuyen
                    .DataBind()
                    .Items.Insert(0, New ListItem(Cls_Common.Str_Opt_Chon, 0))
                End With
            Else
                Dim lst = (From q In data.Tinhs
                            Order By q.TenTinh Ascending
                            Select New With {.Value = q.TinhId, .Text = q.TenTinh}).ToList
                With ddlProvince
                    .Items.Clear()
                    .AppendDataBoundItems = True
                    .DataTextField = "Text"
                    .DataValueField = "Value"
                    .DataSource = lst
                    .DataBind()
                    .Items.Insert(0, New ListItem(Cls_Common.Str_Opt_ToanQuoc, "-1"))
                End With
                '' Load thông tin mặc định cho Huyện
                With ddlHuyen
                    .Items.Clear()
                    .AppendDataBoundItems() = True
                    .Items.Insert(0, New ListItem(Cls_Common.Str_Opt_Chon, 0))
                End With
            End If
        End Using
        txtYearFrom.Text = Now.Year
        txtYearTo.Text = Now.Year
    End Sub

#Region "PRIVATE EVENT FOR CONTROL"
    Private Sub BindToGrid(Optional ByVal strTinh As Integer = -1, Optional ByVal strHuyen As Integer = 0, Optional ByVal strYear_From As Integer = 0, Optional ByVal strYear_To As Integer = 0)
        Using data As New ThanhTraLaoDongEntities
            Dim p = data.usp_Rpt_Phieu_KetQuaThanhTra(strTinh, strHuyen, strYear_From, strYear_To).ToList

            If Not p Is Nothing Then
                With grdShow
                    .DataSource = p
                    .DataBind()
                End With
            Else
                With grdShow
                    .DataSource = Nothing
                    .DataBind()
                End With
            End If
        End Using
    End Sub

    Private Sub Button_Click(ByVal sender As Object, ByVal em As System.EventArgs) Handles btnExport.Click
        Dim strTinh As Integer = ddlProvince.SelectedValue
        Dim strHuyen As Integer = ddlHuyen.SelectedValue
        Dim strYear_From As Integer = IIf(String.IsNullOrEmpty(txtYearFrom.Text.Trim()), Now.Year, txtYearFrom.Text.Trim())
        Dim strYear_To As Integer = IIf(String.IsNullOrEmpty(txtYearTo.Text.Trim()), Now.Year, txtYearTo.Text.Trim())
        BindToGrid(strTinh, strHuyen, strYear_From, strYear_To)
    End Sub

    Protected Sub grdShow_RowCreated(ByVal sender As Object, ByVal e As GridViewRowEventArgs) Handles grdShow.RowCreated
        If e.Row.RowType = DataControlRowType.Header Then
            Dim HeaderGridRow1 As New GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert)
            Dim HeaderGridRow2 As New GridViewRow(1, 0, DataControlRowType.Header, DataControlRowState.Insert)

            Dim HeaderCell As New TableCell()
            ''For Header 1
            HeaderCell.Text = "Tiêu Chí"
            HeaderCell.RowSpan = 3
            HeaderCell.CssClass = "GridHeader"
            HeaderCell.HorizontalAlign = HorizontalAlign.Center
            HeaderGridRow1.Cells.Add(HeaderCell)

            HeaderCell = New TableCell()
            HeaderCell.Text = "Loại hình doanh nghiệp".ToUpper()
            HeaderCell.ColumnSpan = 10
            HeaderCell.HorizontalAlign = HorizontalAlign.Center
            HeaderGridRow1.Cells.Add(HeaderCell)
            grdShow.Controls(0).Controls.AddAt(0, HeaderGridRow1)


            '' For Header 2           

           For index As Integer = 1 To 5
                Dim header As String = ""
                Select Case index
                    Case 1
                        header = "FDI"
                    Case 2
                        header = "Nhà Nước"
                    Case 3
                        header = "Ngoài Nhà Nước"
                    Case 4
                        header = "Khác"
                    Case 5
                        header = "Tổng"
                End Select
                HeaderCell = New TableCell()
                HeaderCell.Text = header
                HeaderCell.ColumnSpan = 2
                HeaderCell.HorizontalAlign = HorizontalAlign.Center
                HeaderGridRow2.Cells.Add(HeaderCell)
            Next

            grdShow.Controls(0).Controls.AddAt(1, HeaderGridRow2)
        End If
    End Sub


#End Region
End Class
