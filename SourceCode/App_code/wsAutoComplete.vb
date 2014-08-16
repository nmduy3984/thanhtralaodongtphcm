Imports System.Web
Imports System.Data
Imports System.Data.SqlClient
Imports System.Web.Services
Imports System.Web.Script.Services
Imports System.Web.Services.Protocols
Imports ThanhTraLaoDongModel
Imports System.Data.Objects

' To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
<System.Web.Script.Services.ScriptService()> _
<WebService(Namespace:="http://tempuri.org/")> _
<WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Public Class wsAutoComplete
    Inherits System.Web.Services.WebService

    <WebMethod()> _
<ScriptMethod(ResponseFormat:=ResponseFormat.Json)> _
    Public Function CheckName(ByVal strName As String) As String
        Using _data As New ThanhTraLaoDongEntities
            If strName.Trim <> "" Then
                Dim p = (From q In _data.Users Where q.UserName = strName Select q).FirstOrDefault
                If Not p Is Nothing Then
                    Return "0"
                Else
                    Return "1"
                End If
            Else
                Return "1"
            End If
        End Using
    End Function
    'Tim kiem muc tieu
    <WebMethod()> _
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)> _
    Public Function SearchAutoComplete(ByVal strSrc As String) As List(Of String)
        Dim json As New List(Of String)
        Using Data As New ThanhTraLaoDongEntities
            If strSrc <> "" Then
                Dim p = (From q In Data.LoaiHinhSanXuats Where q.Title.Contains(strSrc) Order By q.LoaiHinhSXId Ascending Select q).Take(20).ToList
                If p.Count > 0 Then
                    For Each i In p
                        json.Add(i.Title)
                    Next
                End If
            End If
        End Using
        'Export du lieu sang kieu du lieu json
        Return json.ToList()
    End Function
    'Kiểm tra trùng số đăng ký kinh doanh
    <WebMethod()> _
<ScriptMethod(ResponseFormat:=ResponseFormat.Json)> _
    Public Function CheckDuplicateSoDKKD(ByVal strName As String) As String
        Using _data As New ThanhTraLaoDongEntities
            If strName.Trim <> "" Then
                Dim p = (From q In _data.DoanhNghieps Where q.SoChungNhanDKKD = strName Select q).FirstOrDefault
                If Not p Is Nothing Then
                    Return "0"
                Else
                    Return "1"
                End If
            Else
                Return "1"
            End If
        End Using
    End Function

    <WebMethod()> _
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)> _
    Public Function CheckNameLoaiHinhSX(ByVal strName As String) As String
        Using _data As New ThanhTraLaoDongEntities
            If strName.Trim <> "" Then
                Dim p = (From q In _data.LoaiHinhSanXuats Where q.Title = strName Select q).FirstOrDefault
                If Not p Is Nothing Then
                    Return p.LoaiHinhSXId.ToString
                Else
                    Return "0"
                End If
            End If
            Return "0"
        End Using
    End Function

    'Tìm kiếm tỉnh/Tp
    <WebMethod()> _
        <ScriptMethod(ResponseFormat:=ResponseFormat.Json)> _
    Public Function SearchAutoProvince(ByVal strSrc As String) As List(Of String)
        Dim json As New List(Of String)
        Using Data As New ThanhTraLaoDongEntities
            If strSrc <> "" Then
                Dim p = (From q In Data.Tinhs Where q.TenTinh.Contains(strSrc) Order By q.TinhId Ascending Select q).Take(20).ToList
                If p.Count > 0 Then
                    For Each i In p
                        json.Add(i.TenTinh)
                    Next
                End If
            End If
        End Using
        'Export du lieu sang kieu du lieu json
        Return json.ToList()
    End Function

    <WebMethod()> _
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)> _
    Public Function KiemTraTrungDoanhNghiep(ByVal strSoDKKD As String, ByVal DoanhNghiepId As Integer) As Integer
        Using _data As New ThanhTraLaoDongEntities
            Dim dn As New DoanhNghiep
            If strSoDKKD <> "" Then
                If DoanhNghiepId > 0 Then
                    dn = (From q In _data.DoanhNghieps Join a In _data.Tinhs On q.TinhId Equals a.TinhId
                                          Where
                                          q.DoanhNghiepId <> DoanhNghiepId And
                                           q.SoChungNhanDKKD.Equals(strSoDKKD)
                                          Select q).FirstOrDefault
                Else
                    dn = (From q In _data.DoanhNghieps Join a In _data.Tinhs On q.TinhId Equals a.TinhId
                              Where q.SoChungNhanDKKD.Equals(strSoDKKD)
                              Select q).FirstOrDefault
                End If
            Else
                Return -1
            End If
            If Not dn Is Nothing Then
                Return dn.DoanhNghiepId
            End If
            Return 0
        End Using
    End Function
    <WebMethod()> _
     <ScriptMethod(ResponseFormat:=ResponseFormat.Json)> _
    Public Function KiemTraTrungSoQD(ByVal strSoDKKD As String) As Integer
        Using _data As New ThanhTraLaoDongEntities
            Dim qdtt As List(Of QuyetDinhThanhTra) = Nothing
            If strSoDKKD <> "" Then
                qdtt = (From a In _data.QuyetDinhThanhTras Where a.SoQuyetDinh.Equals(strSoDKKD) Select a).ToList()
            End If
            If Not IsNothing(qdtt) AndAlso qdtt.Count > 0 Then
                Return 1
            End If
            Return 0
        End Using
    End Function
    <WebMethod()> _
       <ScriptMethod(ResponseFormat:=ResponseFormat.Json)> _
    Public Function LayDSTinh(ByVal IsThanhTra As Integer, ByVal TinhId As Integer) As List(Of App_Code.ObjectJSON)
        Using data As New ThanhTraLaoDongEntities
            Dim json As New List(Of App_Code.ObjectJSON)
            If IsThanhTra = 1 Then
                Dim lstTinh = (From a In data.Tinhs Order By a.TenTinh).ToList
                For i As Integer = 0 To lstTinh.Count - 1
                    Dim obj As New App_Code.ObjectJSON
                    obj.ID = lstTinh(i).TinhId
                    obj.FullText = lstTinh(i).TenTinh
                    json.Add(obj)
                Next
            Else
                Dim lstTinh = (From a In data.Tinhs Where a.TinhId = TinhId Order By a.TenTinh).ToList
                For i As Integer = 0 To lstTinh.Count - 1
                    Dim obj As New App_Code.ObjectJSON
                    obj.ID = lstTinh(i).TinhId
                    obj.FullText = lstTinh(i).TenTinh
                    json.Add(obj)
                Next
            End If
            Return json
        End Using
    End Function
    <WebMethod()> _
       <ScriptMethod(ResponseFormat:=ResponseFormat.Json)> _
    Public Function LayDSHuyen(ByVal TinhId As Integer) As List(Of App_Code.ObjectJSON)
        Using data As New ThanhTraLaoDongEntities
            Dim json As New List(Of App_Code.ObjectJSON)
            Dim lstHuyen = (From a In data.Huyens Where a.TinhId = TinhId Select a).ToList
            For i As Integer = 0 To lstHuyen.Count - 1
                Dim obj As New App_Code.ObjectJSON
                obj.ID = lstHuyen(i).HuyenId
                obj.FullText = lstHuyen(i).TenHuyen
                json.Add(obj)
            Next
            Return json
        End Using
    End Function
    <WebMethod()> _
       <ScriptMethod(ResponseFormat:=ResponseFormat.Json)> _
    Public Function LayDSDNTheoSoQD(ByVal SoQD As String) As List(Of App_Code.DoanhNghiep)
        Using data As New ThanhTraLaoDongEntities
            Dim json As New List(Of App_Code.DoanhNghiep)
            Dim lstDN = (From a In data.DoanhNghieps
                    Join b In data.QuyetDinhTTDoanhNghieps On a.DoanhNghiepId Equals b.DoanhNghiepId
                    Join c In data.QuyetDinhThanhTras On b.QuyetDinhTT Equals c.SoQuyetDinh
                    Where c.SoQuyetDinh.Equals(SoQD) Order By a.ThoiGianLamViec
                    Select a).ToList
            For i As Integer = 0 To lstDN.Count - 1
                Dim obj As New App_Code.DoanhNghiep
                obj.DoanhNghiepId = CInt(lstDN(i).DoanhNghiepId)
                obj.TenDoanhNghiep = lstDN(i).TenDoanhNghiep
                obj.DiaChi = lstDN(i).TruSoChinh + ", " + lstDN(i).Huyen.TenHuyen + ", " + lstDN(i).Tinh.TenTinh
                obj.ThoiGianLamViec = If(lstDN(i).ThoiGianLamViec Is Nothing, "", CType(lstDN(i).ThoiGianLamViec, Date).ToString("dd/MM/yyyy"))
                json.Add(obj)
            Next
            Return json
        End Using
    End Function
End Class