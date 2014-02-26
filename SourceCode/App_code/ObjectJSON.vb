Namespace App_Code
    Public Class ObjectJSON

        Private _fulltext As String
        Public Property FullText() As String
            Get
                Return _fulltext
            End Get
            Set(ByVal value As String)
                _fulltext = value
            End Set
        End Property

        Private _id As String
        Public Property ID() As String
            Get
                Return _id
            End Get
            Set(ByVal value As String)
                _id = value
            End Set
        End Property

    End Class
End Namespace