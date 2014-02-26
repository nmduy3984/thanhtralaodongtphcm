Imports System.Data
Imports ThanhTraLaoDongModel
Imports Cls_Common
Imports SecurityService
Partial Class Control_GopY_Create
    Inherits System.Web.UI.UserControl
    Private Shared ReadOnly log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
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
            LoadDanhGia()
        End If
    End Sub
    Private Sub LoadDanhGia()
        Using data As New ThanhTraLaoDongEntities
            Dim userid As Integer = CInt(Session("UserId"))
            Dim g = (From a In data.DanhGias Where a.UserId = userid).FirstOrDefault
            Dim TongSoUserDanhGia As Integer = data.DanhGias.ToList.Count
            Dim SumLevel1 = (From a In data.DanhGias Where a.MaTieuChi.Equals("1")).ToList
            Dim SumLevel2 = (From a In data.DanhGias Where a.MaTieuChi.Equals("2")).ToList
            Dim SumLevel3 = (From a In data.DanhGias Where a.MaTieuChi.Equals("3")).ToList
            If TongSoUserDanhGia = 0 Then
                TongSoUserDanhGia = 1
            End If
            Dim PerLevel1 = Math.Round((SumLevel1.Count / TongSoUserDanhGia) * 100, 2)
            Dim PerLevel2 = Math.Round((SumLevel2.Count / TongSoUserDanhGia) * 100, 2)
            Dim PerLevel3 = Math.Round((SumLevel3.Count / TongSoUserDanhGia) * 100, 2)
            lblKetQua.Text = "Rất hay: <span style='color:#FF0000;font-weight:bold;'>" & PerLevel1 & "</span>% &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Trung bình: <span style='color:#FF0000;font-weight:bold;'>" & PerLevel2 & "</span>% &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Không cần: <span style='color:#FF0000;font-weight:bold;'>" & PerLevel3 & "</span>%"
            lblSoNguoi.Text = "Rất hay <span style='color:#FF0000;font-weight:bold;'>" & String.Format(info, "{0:n0}", SumLevel1.Count) & "/" & String.Format(info, "{0:n0}", TongSoUserDanhGia) & "</span> &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Trung bình <span style='color:#FF0000;font-weight:bold;'>" & String.Format(info, "{0:n0}", SumLevel2.Count) & "/" & String.Format(info, "{0:n0}", TongSoUserDanhGia) & "</span> &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Không cần <span style='color:#FF0000;font-weight:bold;'>" & String.Format(info, "{0:n0}", SumLevel3.Count) & "/" & String.Format(info, "{0:n0}", TongSoUserDanhGia) & "</span>"
            If Not IsNothing(g) Then
                rdlTieuChi.SelectedValue = g.MaTieuChi
                btnBieuQuyet.Text = "Thay đổi"
            End If
        End Using
    End Sub
#End Region
#Region "Event for control "
    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Using data As New ThanhTraLaoDongEntities
            Dim p As New ThanhTraLaoDongModel.GopY
            Try
                Dim curdate As Date = Now
                Dim userid As Integer = CInt(Session("UserId"))
                Dim g = (From a In data.Gopies Where a.NgayTao.Value.Day = curdate.Day And a.NgayTao.Value.Month = curdate.Month And a.NgayTao.Value.Year = curdate.Year And a.UserId = userid).FirstOrDefault
                If IsNothing(g) Then
                    'If txtNoiDung.Text.Trim().Length >= 200 Then
                    p.NoiDung = txtNoiDung.Text.Trim()
                    p.UserId = Session("UserId")
                    p.NguoiTao = Session("Username")
                    p.NgayTao = Now()
                    data.Gopies.AddObject(p)
                    data.SaveChanges()
                    txtNoiDung.Text = ""
                    Insert_App_Log("Insert  GopY:" & p.GopYId & "", Function_Name.GopY, Audit_Type.Create, Request.ServerVariables("REMOTE_ADDR"), Session("UserName"))
                    Excute_Javascript("Alertbox('Cập nhật dữ liệu thành công.');", Me.Page, True)
                    'Else
                    '    Excute_Javascript("Alertbox('Nội dung góp ý ít nhất 200 kí tự.');", Me.Page, True)
                    'End If
                Else
                    btnSave.Enabled = False
                    Excute_Javascript("Alertbox('Bạn đã tạo góp ý trong ngày. Xin cám ơn!');", Me.Page, True)
                End If

            Catch ex As Exception
                log4net.Config.XmlConfigurator.Configure()
                log.Error("Error error " & AddTabSpace(1) & Session("Username") & AddTabSpace(1) & "IP:" & GetIPAddress(), ex)
                Excute_Javascript("Alertbox('Cập nhật thất bại.');", Me.Page, True)
            End Try
        End Using

    End Sub

#End Region

    Protected Sub btnBieuQuyet_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBieuQuyet.Click
        Using data As New ThanhTraLaoDongEntities
            Dim p As New ThanhTraLaoDongModel.DanhGia
            Try
                Dim userid As Integer = CInt(Session("UserId"))
                Dim g = (From a In data.DanhGias Where a.UserId = userid).FirstOrDefault
                If IsNothing(g) Then
                    p.MaTieuChi = rdlTieuChi.SelectedValue
                    p.UserId = Session("UserId")
                    p.NguoiTao = Session("Username")
                    p.NgayTao = Now()
                    data.DanhGias.AddObject(p)
                    data.SaveChanges()
                    btnBieuQuyet.Text = "Thay đổi"
                    LoadDanhGia()
                    Excute_Javascript("Alertbox('Đánh giá thành công.');", Me.Page, True)
                Else
                    g.MaTieuChi = rdlTieuChi.SelectedValue
                    g.NguoiSua = Session("Username")
                    g.NgaySua = Now()
                    data.SaveChanges()
                    LoadDanhGia()
                    Excute_Javascript("Alertbox('Thay đổi đánh giá thành công.');", Me.Page, True)
                End If

            Catch ex As Exception
                log4net.Config.XmlConfigurator.Configure()
                log.Error("Error error " & AddTabSpace(1) & Session("Username") & AddTabSpace(1) & "IP:" & GetIPAddress(), ex)
                Excute_Javascript("Alertbox('Cập nhật thất bại.');", Me.Page, True)
            End Try
        End Using
    End Sub
End Class
