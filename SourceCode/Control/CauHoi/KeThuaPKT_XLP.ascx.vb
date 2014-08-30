Imports System.Data
Imports ThanhTraLaoDongModel
Imports Cls_Common
Imports SecurityService
Partial Class Control_CauHoi_KeThuaPKT_XLP
    Inherits System.Web.UI.UserControl
    Private Shared ReadOnly log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            If Not Request.QueryString("PhieuID") Is Nothing Then
                hidID.Value = Request.QueryString("PhieuID")
                BindDataQuestion(hidID.Value)
            End If
        End If
        Dim script As ScriptManager = ScriptManager.GetCurrent(Me.Page)
        If Not script Is Nothing AndAlso script.IsInAsyncPostBack Then
            ScriptManager.RegisterStartupScript(Me.Page, Me.Page.GetType, "duyjs", "ajaxJquery()", True)
        Else
            Me.Page.ClientScript.RegisterStartupScript(Me.Page.GetType(), "duyjs", String.Concat("Sys.Application.add_load(function(){", "ajaxJquery()", "});"), True)
        End If
    End Sub
    'Show du lieu cua contest
    Sub BindDataQuestion(ByVal iQaId As String)
        Using data As New ThanhTraLaoDongEntities
            Dim ArrId As String() = iQaId.Split(",")
            Dim p = (From q In data.PhieuNhapHeaders Join a In data.DoanhNghieps On q.DoanhNghiepId Equals a.DoanhNghiepId
                     Where ArrId.Contains(q.PhieuID)
                     Select New With {q.PhieuID,
                                        a.TenDoanhNghiep,
                                      a.DoanhNghiepId}).ToList
            Dim strKey_Name() As String = {"PhieuID", "TenDoanhNghiep", "DoanhNghiepId"}
            With grdShow
                .DataKeyNames = strKey_Name
                .DataSource = p
                .DataBind()
            End With

            Dim lst = (From q In data.QuyetDinhThanhTras
                           Order By q.SoQuyetDinh
                           Select New With {.Value = q.SoQuyetDinh, .Text = q.SoQuyetDinh}).ToList
            With ddlSoQuyetDinh
                .Items.Clear()
                .AppendDataBoundItems() = True
                .DataTextField = "Text"
                .DataValueField = "Value"
                .DataSource = lst
                .DataBind()
                .Items.Insert(0, New ListItem(Cls_Common.Str_Opt_Chon, 0))
            End With
        End Using
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Dim phieuId As Integer = 0
        Dim DNId As Integer = 0
        Dim SQD As String = ddlSoQuyetDinh.SelectedValue
        Using data As New ThanhTraLaoDongEntities
            For Each item As GridViewRow In grdShow.Rows
                Dim chkItem As New CheckBox
                chkItem = CType(item.FindControl("chkItem"), CheckBox)
                If chkItem.Checked Then
                    phieuId = grdShow.DataKeys(item.RowIndex)("PhieuID").ToString
                    DNId = grdShow.DataKeys(item.RowIndex)("DoanhNghiepId").ToString


                    Try
                        'Thay doi trang thai IsEdited bang QuyetDinhThanhTra
                        Dim qdtt = (From a In data.QuyetDinhThanhTras
                                   Where a.SoQuyetDinh.Contains(SQD) Select a).FirstOrDefault
                        If Not qdtt Is Nothing Then
                            qdtt.IsEdited = True
                            data.SaveChanges()
                        End If
                        'Thêm doanh nghiệp vào bảng QuyetDinhTTDoanhNghiep  nếu chưa có
                        Dim qddn As QuyetDinhTTDoanhNghiep = (From a In data.QuyetDinhTTDoanhNghieps Where a.DoanhNghiepId = DNId And a.QuyetDinhTT.Equals(SQD)).SingleOrDefault
                        If qddn Is Nothing Then
                            qddn = New QuyetDinhTTDoanhNghiep
                            qddn.QuyetDinhTT = SQD
                            qddn.DoanhNghiepId = DNId
                            qddn.IsMoi = True
                            data.QuyetDinhTTDoanhNghieps.AddObject(qddn)
                            data.SaveChanges()
                        End If

                        Dim iPhieuIdOld As Integer = phieuId
                        'B1: Tạo phiếu nhập header
                        TaoPhieuHeader(iPhieuIdOld, SQD)
                        'B2: Tạo các mục câu hỏi
                        TaoCauHoi1(iPhieuIdOld)
                        TaoCauHoi2(iPhieuIdOld)
                        TaoCauHoi3(iPhieuIdOld)
                        TaoCauHoi4(iPhieuIdOld)
                        TaoCauHoi5(iPhieuIdOld)
                        TaoCauHoi6(iPhieuIdOld)
                        TaoCauHoi7(iPhieuIdOld)
                        TaoCauHoi8(iPhieuIdOld)
                        TaoCauHoi9(iPhieuIdOld)
                        TaoCauHoi10(iPhieuIdOld)
                        TaoCauHoi11(iPhieuIdOld)
                        TaoCauHoi12(iPhieuIdOld)
                        KetLuan(iPhieuIdOld)
                        HanhViDN(iPhieuIdOld)
                        Excute_Javascript("AlertboxRedirect('Kế thừa phiếu thành công.','../BienBanThanhTra/List.aspx');", Me.Page, True)
                    Catch ex As Exception
                        log4net.Config.XmlConfigurator.Configure()
                        log.Error("Error error " & AddTabSpace(1) & Session("Username") & AddTabSpace(1) & "IP:" & GetIPAddress(), ex)
                        Excute_Javascript("Alertbox('Kế thừa thất bại.');", Me.Page, True)
                    End Try
                End If
            Next

        End Using
    End Sub
#Region "Tạo BBTT kế thừa từ PTKT hoặc kế thừa PTKT --> PTKT"
    Protected Sub TaoPhieuHeader(ByVal iPhieuIdOld As Integer, ByVal SQD As String)
        '' Tạo phiếu tại đây
        Using data As New ThanhTraLaoDongEntities
            '' Lấy thông tin Doanh nghiệp ra
            Dim p = (From q In data.PhieuNhapHeaders Where q.PhieuID = iPhieuIdOld).FirstOrDefault()
            If Not IsNothing(p) Then
                ''Luu phieu moi
                'hidDNId.Value = p.DoanhNghiepId ' Lưu doanhnghiepid
                Dim pn As New ThanhTraLaoDongModel.PhieuNhapHeader
                pn.TenPhieu = "[BBKT]" + "-" + p.DoanhNghiep.TenDoanhNghiep + "-" + Date.Now.ToString("dd/MM/yyyy HH:mm:ss")
                pn.LoaiPhieu = True
                pn.DoanhNghiepId = p.DoanhNghiepId
                pn.CauHoiDaTraLoi = p.CauHoiDaTraLoi
                pn.IsNopPhat = False
                pn.IsThucHieKienNghi = False
                pn.DinhChiHDThietBi = ""
                pn.TienPhatDuKien = 0
                pn.SoKienNghiChuaThucHien = 0
                pn.SoKienNghiDaThucHien = 0
                pn.SoChiNhanh = p.SoChiNhanh
                pn.TongSoNhanVien = p.TongSoNhanVien
                pn.SoLaoDongNu = p.SoLaoDongNu
                pn.SoNguoiLamNgheNguyHiem = p.SoNguoiLamNgheNguyHiem
                pn.SoNguoiLamCongViecYeuCauNghiemNgat = p.SoNguoiLamCongViecYeuCauNghiemNgat
                pn.TongGiaTriSP = p.TongGiaTriSP
                pn.TongLoiNhuanSauThue = p.TongLoiNhuanSauThue
                pn.IsCongDoan = p.IsCongDoan
                pn.NguoiLienHe = p.NguoiLienHe
                pn.DienThoaiLH = p.DienThoaiLH
                pn.EmailLH = p.EmailLH
                pn.SoQuyenDinh = SQD
                pn.NgayKetThucPhieu = Nothing
                pn.NgayTao = Date.Now
                pn.NguoiTao = Session("Username")
                data.PhieuNhapHeaders.AddObject(pn)
                data.SaveChanges()
                Session("phieuid") = pn.PhieuID 'Luu phieuid moi
                hidPhieuIdNew.Value = pn.PhieuID
                Session("ModePhieu") = ModePhieu.Edit
                Insert_App_Log("Insert  Phieunhapheader:" & pn.TenPhieu & "", Function_Name.PhieuKiemTra, Audit_Type.Create, Request.ServerVariables("REMOTE_ADDR"), Session("UserName"))

            End If
        End Using

    End Sub
    Protected Sub TaoCauHoi1(ByVal iPhieuIdOld As Integer)
        Try
            Using data As New ThanhTraLaoDongEntities
                Dim q = (From a In data.CauHoi1 Where a.PhieuId = iPhieuIdOld Select a).SingleOrDefault
                If Not IsNothing(q) Then
                    Dim p As New CauHoi1
                    p.PhieuId = hidPhieuIdNew.Value
                    p.Q11 = q.Q11
                    p.Q12 = q.Q12
                    p.Q13 = q.Q13
                    p.NguoiTao = Session("Username")
                    p.NgayTao = Date.Now
                    data.CauHoi1.AddObject(p)
                    data.SaveChanges()
                End If
            End Using
        Catch ex As Exception
            log4net.Config.XmlConfigurator.Configure()
            log.Error("Error error " & AddTabSpace(1) & Session("Username") & AddTabSpace(1) & "IP:" & GetIPAddress(), ex)
        End Try
    End Sub
    Protected Sub TaoCauHoi2(ByVal iPhieuIdOld As Integer)
        Try
            Using data As New ThanhTraLaoDongEntities
                Dim q = (From a In data.CauHoi2 Where a.PhieuId = iPhieuIdOld Select a).SingleOrDefault
                If Not IsNothing(q) Then
                    Dim p As New CauHoi2
                    p.PhieuId = hidPhieuIdNew.Value
                    p.Q21 = q.Q21
                    p.Q211 = q.Q211
                    p.Q212 = q.Q212
                    p.Q213 = q.Q213
                    p.Q214 = q.Q214
                    p.Q215 = q.Q215
                    p.Q216 = q.Q216
                    p.Q217 = q.Q217
                    p.Q218 = q.Q218
                    p.Q221 = q.Q221
                    p.Q222 = q.Q222
                    p.Q223 = q.Q223
                    p.Q224 = q.Q224
                    p.Q225 = q.Q225
                    p.Q231 = q.Q231
                    p.Q2321 = q.Q2321
                    p.Q2322 = q.Q2322
                    p.Q2323 = q.Q2323
                    p.Q233 = q.Q233
                    p.Q241 = q.Q241
                    p.Q242 = q.Q242
                    p.Q219 = q.Q219
                    p.Q226 = q.Q226
                    p.Q22111 = q.Q22111
                    p.Q2331 = q.Q2331
                    p.Q251 = q.Q251
                    p.Q2110 = q.Q2110
                    p.Q2210 = q.Q2210
                    p.Q2211 = q.Q2211
                    p.Q227 = q.Q227
                    p.Q228 = q.Q228
                    p.Q229 = q.Q229
                    p.Q234 = q.Q234
                    p.Q235 = q.Q235
                    p.Q236 = q.Q236
                    p.Q237 = q.Q237
                    p.Q238 = q.Q238

                    p.NguoiTao = Session("Username")
                    p.NgayTao = Date.Now
                    data.CauHoi2.AddObject(p)
                    data.SaveChanges()
                End If
            End Using
        Catch ex As Exception
            log4net.Config.XmlConfigurator.Configure()
            log.Error("Error error " & AddTabSpace(1) & Session("Username") & AddTabSpace(1) & "IP:" & GetIPAddress(), ex)
        End Try
    End Sub
    Protected Sub TaoCauHoi3(ByVal iPhieuIdOld As Integer)
        Try
            Using data As New ThanhTraLaoDongEntities
                Dim q = (From a In data.CauHoi3 Where a.PhieuId = iPhieuIdOld Select a).SingleOrDefault
                If Not IsNothing(q) Then
                    Dim p As New CauHoi3
                    p.PhieuId = hidPhieuIdNew.Value
                    p.Q31 = q.Q31
                    p.Q32 = q.Q32
                    p.Q33 = q.Q33
                    p.Q34 = q.Q34
                    p.Q341 = q.Q341
                    p.Q35 = q.Q35
                    p.Q351 = q.Q351
                    p.NguoiTao = Session("Username")
                    p.NgayTao = Date.Now
                    data.CauHoi3.AddObject(p)
                    data.SaveChanges()
                End If
            End Using
        Catch ex As Exception
            log4net.Config.XmlConfigurator.Configure()
            log.Error("Error error " & AddTabSpace(1) & Session("Username") & AddTabSpace(1) & "IP:" & GetIPAddress(), ex)
        End Try
    End Sub
    Protected Sub TaoCauHoi4(ByVal iPhieuIdOld As Integer)
        Try
            Using data As New ThanhTraLaoDongEntities
                Dim q = (From a In data.CauHoi4 Where a.PhieuId = iPhieuIdOld Select a).SingleOrDefault
                If Not IsNothing(q) Then
                    Dim p As New CauHoi4
                    p.PhieuId = hidPhieuIdNew.Value
                    p.Q4111 = q.Q4111
                    p.Q4112 = q.Q4112
                    p.Q4113 = q.Q4113
                    p.Q4104 = q.Q4104
                    p.Q41131 = q.Q41131
                    p.Q421 = q.Q421
                    p.Q422 = q.Q422
                    p.Q423 = q.Q423
                    p.Q43 = q.Q43
                    p.Q44 = q.Q44
                    p.Q45 = q.Q45
                    p.Q461 = q.Q461
                    p.Q462 = q.Q462
                    p.Q463 = q.Q463
                    p.Q464 = q.Q464
                    p.Q47 = q.Q47
                    p.Q48 = q.Q48
                    p.Q481 = q.Q481
                    p.Q482 = q.Q482
                    p.Q49 = q.Q49
                    p.Q410 = q.Q410
                    p.Q4101 = q.Q4101
                    p.Q4102 = q.Q4102
                    p.Q41021 = q.Q41021
                    p.Q41022 = q.Q41022
                    p.Q4103 = q.Q4103
                    p.Q410311 = q.Q410311
                    p.Q410312 = q.Q410312
                    p.Q41032 = q.Q41032
                    p.Q41033 = q.Q41033
                    p.Q46a = q.Q46a
                    p.Q46b = q.Q46b
                    p.Q4114 = q.Q4114
                    p.Q41011 = q.Q41011
                    p.Q424 = q.Q424
                    p.Q4105 = q.Q4105

                    p.NguoiTao = Session("Username")
                    p.NgayTao = Date.Now
                    data.CauHoi4.AddObject(p)
                    data.SaveChanges()
                End If
            End Using
        Catch ex As Exception
            log4net.Config.XmlConfigurator.Configure()
            log.Error("Error error " & AddTabSpace(1) & Session("Username") & AddTabSpace(1) & "IP:" & GetIPAddress(), ex)
        End Try
    End Sub
    Protected Sub TaoCauHoi5(ByVal iPhieuIdOld As Integer)
        Try
            Using data As New ThanhTraLaoDongEntities
                Dim q = (From a In data.CauHoi5 Where a.PhieuId = iPhieuIdOld Select a).SingleOrDefault
                If Not IsNothing(q) Then
                    Dim p As New CauHoi5
                    p.PhieuId = hidPhieuIdNew.Value
                    p.Q511 = q.Q511
                    p.Q512 = q.Q512
                    p.Q52 = q.Q52
                    p.Q521 = q.Q521
                    p.Q522 = q.Q522
                    p.Q53 = q.Q53
                    p.Q531 = q.Q531
                    p.Q532 = q.Q532
                    p.Q533 = q.Q533
                    p.Q54 = q.Q54
                    p.Q513 = q.Q513
                    p.Q541 = q.Q541
                    p.Q55 = q.Q55

                    p.NguoiTao = Session("Username")
                    p.NgayTao = Date.Now
                    data.CauHoi5.AddObject(p)
                    data.SaveChanges()
                End If
            End Using
        Catch ex As Exception
            log4net.Config.XmlConfigurator.Configure()
            log.Error("Error error " & AddTabSpace(1) & Session("Username") & AddTabSpace(1) & "IP:" & GetIPAddress(), ex)
        End Try
    End Sub
    Protected Sub TaoCauHoi6(ByVal iPhieuIdOld As Integer)
        Try
            Using data As New ThanhTraLaoDongEntities
                Dim q = (From a In data.CauHoi6 Where a.PhieuId = iPhieuIdOld Select a).SingleOrDefault
                If Not IsNothing(q) Then
                    Dim p As New CauHoi6
                    p.PhieuId = hidPhieuIdNew.Value
                    p.Q611 = q.Q611
                    p.Q612 = q.Q612
                    p.Q621 = q.Q621
                    p.Q622 = q.Q622
                    p.Q63 = q.Q63
                    p.Q641 = q.Q641
                    p.Q642 = q.Q642
                    p.Q65 = q.Q65
                    p.Q651 = q.Q651
                    p.Q66 = q.Q66
                    p.Q67 = q.Q67
                    p.Q671 = q.Q671
                    p.Q68 = q.Q68
                    p.Q681 = q.Q681
                    p.Q631 = q.Q631

                    p.NguoiTao = Session("Username")
                    p.NgayTao = Date.Now
                    data.CauHoi6.AddObject(p)
                    data.SaveChanges()
                End If
            End Using
        Catch ex As Exception
            log4net.Config.XmlConfigurator.Configure()
            log.Error("Error error " & AddTabSpace(1) & Session("Username") & AddTabSpace(1) & "IP:" & GetIPAddress(), ex)
        End Try
    End Sub
    Protected Sub TaoCauHoi7(ByVal iPhieuIdOld As Integer)
        Try
            Using data As New ThanhTraLaoDongEntities
                Dim q = (From a In data.CauHoi7 Where a.PhieuId = iPhieuIdOld Select a).SingleOrDefault
                If Not IsNothing(q) Then
                    Dim p As New CauHoi7
                    p.PhieuId = hidPhieuIdNew.Value
                    p.Q701 = q.Q701
                    p.Q7011 = q.Q7011
                    p.Q7012 = q.Q7012
                    p.Q7013 = q.Q7013
                    p.Q7014 = q.Q7014
                    p.Q702 = q.Q702
                    p.Q703 = q.Q703
                    p.Q7031 = q.Q7031
                    p.Q704 = q.Q704
                    p.Q7041 = q.Q7041
                    p.Q7051 = q.Q7051
                    p.Q7052 = q.Q7052
                    p.Q7053 = q.Q7053
                    p.Q70611 = q.Q70611
                    p.Q70612 = q.Q70612
                    p.Q706211 = q.Q706211
                    p.Q706212 = q.Q706212
                    p.Q706221 = q.Q706221
                    p.Q706222 = q.Q706222
                    p.Q706232 = q.Q706232
                    p.Q707 = q.Q707
                    p.Q7071 = q.Q7071
                    p.Q7072 = q.Q7072
                    p.Q7073 = q.Q7073
                    p.Q708 = q.Q708
                    p.Q7081 = q.Q7081
                    p.Q7082 = q.Q7082
                    p.Q7083 = q.Q7083
                    p.Q709 = q.Q709
                    p.Q7091 = q.Q7091
                    p.Q70911 = q.Q70911
                    p.Q71012 = q.Q71012
                    p.Q71011 = q.Q71011
                    p.Q710111 = q.Q710111
                    p.Q711 = q.Q711
                    p.Q7111 = q.Q7111
                    p.Q7112 = q.Q7112
                    p.Q7113 = q.Q7113
                    p.Q7114 = q.Q7114
                    p.Q7115 = q.Q7115
                    p.Q7116 = q.Q7116
                    p.Q7117 = q.Q7117
                    p.Q7118 = q.Q7118
                    p.Q7121 = q.Q7121
                    p.Q7122 = q.Q7122
                    p.Q71221 = q.Q71221
                    p.Q71222 = q.Q71222
                    p.Q713 = q.Q713
                    p.Q7141 = q.Q7141
                    p.Q71421 = q.Q71421
                    p.Q71422 = q.Q71422
                    p.Q7143 = q.Q7143
                    p.Q7144 = q.Q7144
                    p.Q7151 = q.Q7151
                    p.Q71511 = q.Q71511
                    p.Q7152 = q.Q7152
                    p.Q71521 = q.Q71521
                    p.Q715211 = q.Q715211
                    p.Q715212 = q.Q715212
                    p.Q715213 = q.Q715213
                    p.Q716 = q.Q716
                    p.Q7161 = q.Q7161
                    p.Q717 = q.Q717
                    p.Q7171 = q.Q7171
                    p.Q7172 = q.Q7172
                    p.Q7173 = q.Q7173
                    p.Q7174 = q.Q7174
                    p.Q7175 = q.Q7175
                    p.Q7176 = q.Q7176
                    p.Q7181 = q.Q7181
                    p.Q7182 = q.Q7182
                    p.Q7183 = q.Q7183
                    p.Q7184 = q.Q7184
                    p.Q710112 = q.Q710112
                    p.Q710113 = q.Q710113
                    p.Q710114 = q.Q710114
                    p.Q710115 = q.Q710115
                    p.Q718 = q.Q718
                    p.Q7185 = q.Q7185
                    p.Q7015 = q.Q7015
                    p.Q7016 = q.Q7016
                    p.Q71110 = q.Q71110
                    p.Q71111 = q.Q71111
                    p.Q71112 = q.Q71112
                    p.Q7119 = q.Q7119
                    p.Q7131 = q.Q7131
                    p.Q7132 = q.Q7132
                    p.Q7177 = q.Q7177
                    p.Q719 = q.Q719
                    p.Q7178 = q.Q7178
                    p.Q7021 = q.Q7021

                    p.NguoiTao = Session("Username")
                    p.NgayTao = Date.Now
                    data.CauHoi7.AddObject(p)
                    data.SaveChanges()
                End If
            End Using
        Catch ex As Exception
            log4net.Config.XmlConfigurator.Configure()
            log.Error("Error error " & AddTabSpace(1) & Session("Username") & AddTabSpace(1) & "IP:" & GetIPAddress(), ex)
        End Try
    End Sub
    Protected Sub TaoCauHoi8(ByVal iPhieuIdOld As Integer)
        Try
            Using data As New ThanhTraLaoDongEntities
                Dim q = (From a In data.CauHoi8 Where a.PhieuId = iPhieuIdOld Select a).SingleOrDefault
                If Not IsNothing(q) Then
                    Dim p As New CauHoi8
                    p.PhieuId = hidPhieuIdNew.Value
                    p.Q8111 = q.Q8111
                    p.Q8112 = q.Q8112
                    p.Q812 = q.Q812
                    p.Q82 = q.Q82
                    p.Q821 = q.Q821
                    p.Q822 = q.Q822
                    p.Q823 = q.Q823
                    p.Q824 = q.Q824
                    p.Q83 = q.Q83
                    p.Q84 = q.Q84
                    p.Q851 = q.Q851
                    p.Q8511 = q.Q8511
                    p.Q8512 = q.Q8512
                    'p.Q8513 = q.Q8513
                    p.Q8514 = q.Q8514
                    p.Q8515 = q.Q8515
                    p.Q8516 = q.Q8516
                    p.Q81 = q.Q81
                    p.Q8241 = q.Q8241

                    p.NguoiTao = Session("Username")
                    p.NgayTao = Date.Now
                    data.CauHoi8.AddObject(p)
                    data.SaveChanges()
                End If
            End Using
        Catch ex As Exception
            log4net.Config.XmlConfigurator.Configure()
            log.Error("Error error " & AddTabSpace(1) & Session("Username") & AddTabSpace(1) & "IP:" & GetIPAddress(), ex)
        End Try
    End Sub
    Protected Sub TaoCauHoi9(ByVal iPhieuIdOld As Integer)
        Try
            Using data As New ThanhTraLaoDongEntities
                Dim q = (From a In data.CauHoi9 Where a.PhieuId = iPhieuIdOld Select a).SingleOrDefault
                If Not IsNothing(q) Then
                    Dim p As New CauHoi9
                    p.PhieuId = hidPhieuIdNew.Value
                    p.Q9111 = q.Q9111
                    p.Q9112 = q.Q9112
                    p.Q92 = q.Q92
                    p.Q921 = q.Q921
                    p.Q923 = q.Q923
                    p.Q924 = q.Q924
                    p.Q922 = q.Q922
                    p.Q93 = q.Q93
                    p.Q931 = q.Q931
                    p.Q932 = q.Q932
                    p.Q933 = q.Q933
                    p.Q925 = q.Q925

                    p.NguoiTao = Session("Username")
                    p.NgayTao = Date.Now
                    data.CauHoi9.AddObject(p)
                    data.SaveChanges()
                End If
            End Using
        Catch ex As Exception
            log4net.Config.XmlConfigurator.Configure()
            log.Error("Error error " & AddTabSpace(1) & Session("Username") & AddTabSpace(1) & "IP:" & GetIPAddress(), ex)
        End Try
    End Sub
    Protected Sub TaoCauHoi10(ByVal iPhieuIdOld As Integer)
        Try
            Using data As New ThanhTraLaoDongEntities
                Dim q = (From a In data.CauHoi10 Where a.PhieuId = iPhieuIdOld Select a).SingleOrDefault
                If Not IsNothing(q) Then
                    Dim p As New CauHoi10
                    p.PhieuId = hidPhieuIdNew.Value
                    p.Q101 = q.Q101
                    p.Q102 = q.Q102
                    p.Q103 = q.Q103
                    p.Q104 = q.Q104
                    p.Q105 = q.Q105

                    p.NguoiTao = Session("Username")
                    p.NgayTao = Date.Now
                    data.CauHoi10.AddObject(p)
                    data.SaveChanges()
                End If
            End Using
        Catch ex As Exception
            log4net.Config.XmlConfigurator.Configure()
            log.Error("Error error " & AddTabSpace(1) & Session("Username") & AddTabSpace(1) & "IP:" & GetIPAddress(), ex)
        End Try
    End Sub
    Protected Sub TaoCauHoi11(ByVal iPhieuIdOld As Integer)
        Try
            Using data As New ThanhTraLaoDongEntities
                Dim q = (From a In data.CauHoi11 Where a.PhieuId = iPhieuIdOld Select a).SingleOrDefault
                If Not IsNothing(q) Then
                    Dim p As New CauHoi11
                    p.PhieuId = hidPhieuIdNew.Value
                    p.Q1111 = q.Q1111
                    p.Q1112 = q.Q1112
                    p.Q112 = q.Q112
                    p.Q113 = q.Q113
                    p.Q114 = q.Q114
                    p.Q115 = q.Q115
                    p.Q116 = q.Q116
                    p.Q117 = q.Q117
                    p.Q11 = q.Q11
                    p.Q1121 = q.Q1121
                    p.Q1131 = q.Q1131
                    p.Q118 = q.Q118

                    p.NguoiTao = Session("Username")
                    p.NgayTao = Date.Now
                    data.CauHoi11.AddObject(p)
                    data.SaveChanges()
                End If
            End Using
        Catch ex As Exception
            log4net.Config.XmlConfigurator.Configure()
            log.Error("Error error " & AddTabSpace(1) & Session("Username") & AddTabSpace(1) & "IP:" & GetIPAddress(), ex)
        End Try
    End Sub
    Protected Sub TaoCauHoi12(ByVal iPhieuIdOld As Integer)
        Try
            Using data As New ThanhTraLaoDongEntities
                Dim q = (From a In data.CauHoi12 Where a.PhieuId = iPhieuIdOld Select a).SingleOrDefault
                If Not IsNothing(q) Then
                    Dim p As New CauHoi12
                    p.PhieuId = hidPhieuIdNew.Value
                    p.Q121 = q.Q121
                    p.Q1211 = q.Q1211
                    p.Q1212 = q.Q1212
                    p.Q1213 = q.Q1213
                    p.Q122 = q.Q122
                    p.Q123 = q.Q123
                    p.Q1241 = q.Q1241
                    p.Q1242 = q.Q1242

                    p.NguoiTao = Session("Username")
                    p.NgayTao = Date.Now
                    data.CauHoi12.AddObject(p)
                    data.SaveChanges()
                End If
            End Using
        Catch ex As Exception
            log4net.Config.XmlConfigurator.Configure()
            log.Error("Error error " & AddTabSpace(1) & Session("Username") & AddTabSpace(1) & "IP:" & GetIPAddress(), ex)
        End Try
    End Sub
    Protected Sub KetLuan(ByVal iPhieuIdOld As Integer)
        Try
            Using data As New ThanhTraLaoDongEntities
                Dim q = (From a In data.KetLuans Where a.PhieuId = iPhieuIdOld Select a).ToList
                If q.Count > 0 Then
                    Dim lstKL As New List(Of KetLuan)
                    For i As Integer = 0 To q.Count - 1
                        Dim p As New KetLuan
                        p.PhieuId = hidPhieuIdNew.Value
                        p.TenCotCauHoi = q(i).TenCotCauHoi
                        p.NDKetLuan = q(i).NDKetLuan
                        p.IsViPham = q(i).IsViPham
                        p.TenBangCauHoi = q(i).TenBangCauHoi
                        lstKL.Add(p)
                    Next
                    For i As Integer = 0 To lstKL.Count - 1
                        data.KetLuans.AddObject(lstKL(i))
                        data.SaveChanges()
                    Next
                End If
            End Using
        Catch ex As Exception
            log4net.Config.XmlConfigurator.Configure()
            log.Error("Error error " & AddTabSpace(1) & Session("Username") & AddTabSpace(1) & "IP:" & GetIPAddress(), ex)
        End Try
    End Sub
    Protected Sub HanhViDN(ByVal iPhieuIdOld As Integer)
        Try
            Using data As New ThanhTraLaoDongEntities
                Dim q = (From a In data.HanhViDNs Where a.PhieuId = iPhieuIdOld Select a).ToList
                If q.Count > 0 Then
                    Dim lstHVDN As New List(Of HanhViDN)
                    For i As Integer = 0 To q.Count - 1
                        Dim p As New HanhViDN
                        p.PhieuId = hidPhieuIdNew.Value
                        p.HanhViId = q(i).HanhViId
                        p.MucPhat = q(i).MucPhat
                        p.DoanhNghiepID = q(i).DoanhNghiepID
                        lstHVDN.Add(p)
                    Next
                    For i As Integer = 0 To lstHVDN.Count - 1
                        data.HanhViDNs.AddObject(lstHVDN(i))
                        data.SaveChanges()
                    Next
                End If
            End Using
        Catch ex As Exception
            log4net.Config.XmlConfigurator.Configure()
            log.Error("Error error " & AddTabSpace(1) & Session("Username") & AddTabSpace(1) & "IP:" & GetIPAddress(), ex)
        End Try
    End Sub

#End Region

    Protected Sub btnBack_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBack.Click
        Response.Redirect("../../Page/PhieuKiemTra/List.aspx")
    End Sub

End Class
