Imports System.Data
Imports System.Text.RegularExpressions
Imports ThanhTraLaoDongModel
Imports Cls_Common
Imports SecurityService
Imports System.Transactions

Partial Class Control_CauHoi_BaoCaoThucHien
    Inherits System.Web.UI.UserControl
    Private Shared ReadOnly log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            If Not Session("Username") = "" Then
                Dim script As ScriptManager = ScriptManager.GetCurrent(Me.Page)
                If Not script Is Nothing AndAlso script.IsInAsyncPostBack Then
                    ScriptManager.RegisterStartupScript(Me.Page, Me.Page.GetType, "duyjs", "ajaxJquery()", True)
                    ScriptManager.RegisterStartupScript(Me.Page, Me.Page.GetType, "duyjs1", "ajaxJqueryToolTip()", True)
                Else
                    Me.Page.ClientScript.RegisterStartupScript(Me.Page.GetType(), "duyjs", String.Concat("Sys.Application.add_load(function(){", "ajaxJquery()", "});"), True)
                    Me.Page.ClientScript.RegisterStartupScript(Me.Page.GetType(), "duyjs1", String.Concat("Sys.Application.add_load(function(){", "ajaxJqueryToolTip()", "});"), True)
                End If
                If Not IsNothing(Request.QueryString("phieuid")) AndAlso Not Request.QueryString("phieuid").Equals("") Then
                    hidPhieuId.Value = Request.QueryString("phieuid")
                Else
                    Response.Redirect("List.aspx")
                End If
                LoadData()
            Else
                Response.Redirect("../../Login.aspx")
            End If
        End If
    End Sub
    Protected Sub LoadData()
        Try
            Using _data As New ThanhTraLaoDongEntities

                Dim pn = (From q In _data.PhieuNhapHeaders Where q.PhieuID = hidPhieuId.Value).FirstOrDefault
                If Not IsNothing(pn) Then
                    If pn.LoaiPhieu Then
                        txtTienPhatDuKien.Text = IIf(IsNothing(pn.TienPhatDuKien), "", String.Format("{0:n0}", pn.TienPhatDuKien))
                        txtTienDaPhat.Text = IIf(IsNothing(pn.TienNopPhat), "", String.Format("{0:n0}", pn.TienNopPhat))
                        If Not IsNothing(pn.NgayNopPhat) Then
                            txtNgayNopPhat.Text = IIf(CType(pn.NgayNopPhat, Date).ToString("dd/MM/yyyy").Equals("01/01/1900"), "", CType(pn.NgayNopPhat, Date).ToString("dd/MM/yyyy"))
                        End If
                    Else
                        lblTienPhatDuKien.Visible = False
                        txtTienPhatDuKien.Visible = False
                        lblTienDaPhat.Visible = False
                        txtTienDaPhat.Visible = False
                        lblNgayNopPhat.Visible = False
                        txtNgayNopPhat.Visible = False
                    End If

                End If
                Dim lsKN As List(Of uspListKienNghiDNByPhieuId_Result) = _data.uspListKienNghiDNByPhieuId(hidPhieuId.Value).ToList()
                If Not IsNothing(lsKN) Then
                    hidCountKN.Value = lsKN.Count
                    Dim strKey_Name() As String = {"Isthuchien"}
                    With grdShow
                        .DataKeyNames = strKey_Name
                        .DataSource = lsKN
                        .DataBind()
                    End With
                End If
            End Using
        Catch ex As Exception
            log4net.Config.XmlConfigurator.Configure()
            log.Error("Error error " & AddTabSpace(1) & Session("Username") & AddTabSpace(1) & "IP:" & GetIPAddress(), ex)
            Excute_Javascript("Alertbox('Load dữ liệu bị lỗi." & ex.Message & "');", Me.Page, True)
        End Try

    End Sub
    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Using sc As New TransactionScope
            Try
                Using data As New ThanhTraLaoDongEntities
                    Dim pn As PhieuNhapHeader = (From a In data.PhieuNhapHeaders Where a.PhieuID = hidPhieuId.Value).FirstOrDefault
                    If Not IsNothing(pn) Then
                        If pn.LoaiPhieu Then
                            Dim tienNopPhat As Integer = GetNumberByFormat(txtTienDaPhat.Text)
                            pn.TienNopPhat = tienNopPhat
                            pn.NgayNopPhat = StringToDate(txtNgayNopPhat.Text.Trim())
                            pn.IsNopPhat = pn.TienNopPhat > 0
                        End If
                        ''''Lưu IsThucHien kiến nghị
                        Dim sqlExec1 As String
                        If hidIsThucHien.Value.Length > 1 Then
                            hidIsThucHien.Value = IIf(hidIsThucHien.Value.EndsWith(","), hidIsThucHien.Value.Substring(0, hidIsThucHien.Value.Length - 1), hidIsThucHien.Value)
                            sqlExec1 = "UPDATE KienNghiDN Set IsThucHien = 1 WHERE PhieuID = " + hidPhieuId.Value.Trim + " AND KienNghiID in (" + hidIsThucHien.Value.ToString + ")"
                            sqlExec1 = sqlExec1 + "; UPDATE KienNghiDN Set IsThucHien = 0 WHERE PhieuID = " + hidPhieuId.Value.Trim + " AND KienNghiID not in (" + hidIsThucHien.Value.ToString + ")"
                            data.ExecuteStoreCommand(sqlExec1)
                        Else
                            sqlExec1 = " UPDATE KienNghiDN Set IsThucHien = 0 WHERE PhieuID = " + hidPhieuId.Value.Trim
                            data.ExecuteStoreCommand(sqlExec1)
                        End If
                        ''''Lưu số kiến nghị đã thực hiện
                        pn.SoKienNghiDaThucHien = hidCountDaThucHienKN.Value
                        pn.SoKienNghiChuaThucHien = hidCountKN.Value - hidCountDaThucHienKN.Value
                        pn.IsThucHieKienNghi = pn.SoKienNghiDaThucHien > 0

                        data.SaveChanges()
                        sc.Complete()
                        Excute_Javascript("AlertboxRedirect('Cập nhật thành công.','../BaoCaoThucHien/List.aspx');", Me.Page, True)

                    End If
                End Using
            Catch ex As Exception
                sc.Dispose()
                log4net.Config.XmlConfigurator.Configure()
                log.Error("Error error " & AddTabSpace(1) & Session("Username") & AddTabSpace(1) & "IP:" & GetIPAddress(), ex)
                Excute_Javascript("Alertbox('Cập nhật thất bại." & ex.Message & "');", Me.Page, True)
            End Try
        End Using
    End Sub

    Protected Sub grdShow_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdShow.RowDataBound
        If e.Row.RowIndex >= 0 Then
            Dim chkItem As CheckBox = CType(e.Row.FindControl("chkItem"), CheckBox)
            chkItem.Checked = grdShow.DataKeys(e.Row.RowIndex)("Isthuchien")
        End If
    End Sub

    Protected Sub btnReset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReset.Click
        LoadData()
    End Sub
End Class
