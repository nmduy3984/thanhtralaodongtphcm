Namespace App_Code
    Public Class DoanhNghiep

        Private _doanhNghiepId As String
        Public Property DoanhNghiepId() As Integer
            Get
                Return _doanhNghiepId
            End Get
            Set(ByVal value As Integer)
                _doanhNghiepId = value
            End Set
        End Property
        Private _tenDoanhNghiep As String
        Public Property TenDoanhNghiep() As String
            Get
                Return _tenDoanhNghiep
            End Get
            Set(ByVal value As String)
                _tenDoanhNghiep = value
            End Set
        End Property

        Private _diaChi As String
        Public Property DiaChi() As String
            Get
                Return _diaChi
            End Get
            Set(ByVal value As String)
                _diaChi = value
            End Set
        End Property
        Private _thoiGianLamViec As String
        Public Property ThoiGianLamViec() As String
            Get
                Return _thoiGianLamViec
            End Get
            Set(ByVal value As String)
                _thoiGianLamViec = value
            End Set
        End Property
    End Class
End Namespace