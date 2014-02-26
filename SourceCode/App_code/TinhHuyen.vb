Imports System.Data
Imports ThanhTraLaoDongModel
Imports Cls_Common
Namespace App_Code
    Public Class TinhHuyen

        Private _tinhid As String
        Public Property TinhId() As String
            Get
                Return _tinhid
            End Get
            Set(ByVal value As String)
                _tinhid = value
            End Set
        End Property

        Private _tentinh As String
        Public Property TenTinh() As String
            Get
                Return _tentinh
            End Get
            Set(ByVal value As String)
                _tentinh = value
            End Set
        End Property
        Private _huyenid As String
        Public Property HuyenId() As String
            Get
                Return _huyenid
            End Get
            Set(ByVal value As String)
                _huyenid = value
            End Set
        End Property

        Private _tenhuyen As String
        Public Property TenHuyen() As String
            Get
                Return _tenhuyen
            End Get
            Set(ByVal value As String)
                _tenhuyen = value
            End Set
        End Property
    End Class
End Namespace