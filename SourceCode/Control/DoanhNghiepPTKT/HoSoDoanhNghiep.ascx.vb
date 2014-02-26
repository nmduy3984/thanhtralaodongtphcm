Imports ThanhTraLaoDongModel
Imports Cls_Common
Imports SecurityService
Partial Class Control_DoanhNghiep_HoSoDoanhNghiep
    Inherits System.Web.UI.UserControl
    Private Shared ReadOnly log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

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
            If Not IsNothing(Request.QueryString("DNId")) Then
                hidID.Value = Request.QueryString("DNId")
            End If
            BindToGrid()
        End If
    End Sub
    Private Sub BindToGrid(Optional ByVal strTinh As Integer = -1, Optional ByVal strYear_From As Integer = 0, Optional ByVal strYear_To As Integer = 0)
        Using data As New ThanhTraLaoDongEntities
            Dim dn As DoanhNghiep = (From a In data.DoanhNghieps Where a.DoanhNghiepId = hidID.Value Select a).FirstOrDefault
            If Not IsNothing(dn) Then
                lblTitle.Text += dn.TenDoanhNghiep
            End If
            Dim p = data.uspHoSoDoanhNghiepByDNId(hidID.Value).ToList
            Dim strKey_name() As String = {"NamDT", "SoLoiDT", "PhieuIdDT", "ThangNamTKT", "PhieuIdTKT", "NamXP", "PhieuIdXP"}
            'Tong so ban ghi
            If Not p Is Nothing Then
                With grdShow
                    .DataKeyNames = strKey_name
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
    Protected Sub grdShow_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdShow.RowCreated
        If e.Row.RowType = DataControlRowType.Header Then
            Dim HeaderGridRow1 As New GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert)
            Dim HeaderCell As New TableCell()
            ''For Header 1
            HeaderCell.Text = "Stt"
            HeaderCell.RowSpan = 2
            HeaderCell.CssClass = "GridHeader"
            HeaderCell.HorizontalAlign = HorizontalAlign.Center
            HeaderGridRow1.Cells.Add(HeaderCell)

            '' For Header 2           
            Dim arrTitle() As String = {"Số lần thanh tra", "Số lần tự kiểm tra", "Số lần xử phạt"}
            For Each item In arrTitle
                HeaderCell = New TableCell()
                HeaderCell.Text = item
                HeaderCell.ColumnSpan = 2
                HeaderCell.HorizontalAlign = HorizontalAlign.Center
                HeaderGridRow1.Cells.Add(HeaderCell)
            Next
            grdShow.Controls(0).Controls.AddAt(0, HeaderGridRow1)
        End If
    End Sub

   
    Protected Sub grdShow_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdShow.RowDataBound
        If e.Row.RowIndex >= 0 Then
            Dim lnkSoLanThanhTra As LinkButton = CType(e.Row.FindControl("lnkSoLanThanhTra"), LinkButton)
            lnkSoLanThanhTra.Text = IIf(grdShow.DataKeys(e.Row.RowIndex)("NamDT") = 0, "", grdShow.DataKeys(e.Row.RowIndex)("NamDT"))
            lnkSoLanThanhTra.Attributes.Add("onclick", "setIndex(" & e.Row.RowIndex.ToString & ");")

            Dim hplSoLoiThanhTra As HyperLink = CType(e.Row.FindControl("hplSoLoiThanhTra"), HyperLink)
            hplSoLoiThanhTra.Text = grdShow.DataKeys(e.Row.RowIndex)("SoLoiDT")
            hplSoLoiThanhTra.NavigateUrl = "../../Page/BienBanThanhTra/BaoCaoThucHien.aspx?phieuId=" + grdShow.DataKeys(hidIndex.Value)("PhieuIdDT").ToString

            Dim lnkSoLanTuKiemTra As LinkButton = CType(e.Row.FindControl("lnkSoLanTuKiemTra"), LinkButton)
            lnkSoLanTuKiemTra.Text = IIf(IsNothing(grdShow.DataKeys(e.Row.RowIndex)("ThangNamTKT")), "", grdShow.DataKeys(e.Row.RowIndex)("ThangNamTKT"))
            lnkSoLanTuKiemTra.Attributes.Add("onclick", "setIndex(" & e.Row.RowIndex.ToString & ");")

            Dim hplSoLanXuPhat As HyperLink = CType(e.Row.FindControl("hplSoLanXuPhat"), HyperLink)
            hplSoLanXuPhat.Text = IIf(grdShow.DataKeys(e.Row.RowIndex)("NamXP") = 0, "", grdShow.DataKeys(e.Row.RowIndex)("NamXP"))
            hplSoLanXuPhat.NavigateUrl = "../../Page/BienBanThanhTra/BienBanViPham.aspx?phieuId=" + grdShow.DataKeys(e.Row.RowIndex)("PhieuIdXP").ToString
        End If
    End Sub
    Protected Sub btnSoLanThanhTra_Click(ByVal sender As Object, ByVal e As EventArgs)
        ' Dim abc = sender
        Try
            Session("phieuid") = grdShow.DataKeys(hidIndex.Value)("PhieuIdDT").ToString
            Session("ModePhieu") = ModePhieu.Detail
            Response.Redirect("../../Page/BienBanThanhTra/ThongTinChung.aspx?DNId=" + hidID.Value)
        Catch ex As Exception
            log4net.Config.XmlConfigurator.Configure()
            log.Error("Error error " & AddTabSpace(1) & Session("Username") & AddTabSpace(1) & "IP:" & GetIPAddress(), ex)
        End Try
    End Sub
     
    Protected Sub btnSoLanTuKiemTra_Click(ByVal sender As Object, ByVal e As EventArgs)
        ' Dim abc = sender
        Try
            Session("phieuid") = grdShow.DataKeys(hidIndex.Value)("PhieuIdTKT").ToString
            Session("ModePhieu") = ModePhieu.Detail
            Response.Redirect("../../Page/PhieuKiemTra/ThongTinChung.aspx?DNId=" + hidID.Value)
        Catch ex As Exception
            log4net.Config.XmlConfigurator.Configure()
            log.Error("Error error " & AddTabSpace(1) & Session("Username") & AddTabSpace(1) & "IP:" & GetIPAddress(), ex)
        End Try
    End Sub
     

    
End Class
