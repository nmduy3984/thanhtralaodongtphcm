
Imports System.Data
Imports ThanhTraLaoDongModel
Imports Cls_Common
Partial Class Control_sysFunction_sysFunctionMenu
    Inherits System.Web.UI.UserControl
    Public strMenu As String = ""

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not Session("Function") Is Nothing Then
            If UserType.DoanhNghiep = Session("isUser") Then
                Response.Redirect("Login.aspx")
            End If
            strMenu = CreateMenu()
        End If

    End Sub

    Private Function CreateMenu() As String
        Dim i As Integer = 1
        Dim sMenu As String = ""
        Dim sufixURL = Request.Url.ToString.Split("Page")(0)
        Dim arrFunctionId As Integer() = Session("Function")
        Using data As New ThanhTraLaoDongEntities
            'get first menu
            'Dim p = From q In data.Functions Where q.ParentId = 0 And q.IsMenu = True Order By q.Sort Ascending Select q
            Dim p = From q In data.Functions Where q.ParentId = 0 And q.IsMenu = True And arrFunctionId.Contains(q.FunctionId) Order By q.Sort Ascending Select q

            If Not p Is Nothing Then
                For Each a As ThanhTraLaoDongModel.Function In p
                    'neu menu dang duoc open
                    If CheckOpenItem(a, arrFunctionId) Then
                        sMenu = sMenu & "<ul class=""current"" id=""MenuCurrent"">" & vbCrLf
                        sMenu = sMenu & "<div>" & vbCrLf
                        sMenu = sMenu + "<li>" & "<a href=""javascript:return false;""><span>" & a.HrefName & "</span></a>" & vbCrLf
                        ' Draw subMenu Here
                        Dim sSubMenu As String = ""
                        sSubMenu = SubMenu(a.FunctionId, i, "show")
                        sMenu = sMenu & sSubMenu
                        sMenu = sMenu & "</li>" & vbCrLf
                        sMenu = sMenu & "</div>" & vbCrLf
                        sMenu = sMenu & "</ul>" & vbCrLf
                        'nguoc lai 
                    Else
                        sMenu = sMenu & "<ul class=""select Nolink"" id=""MenuSelect"">" & vbCrLf
                        sMenu = sMenu & "<div>" & vbCrLf

                        sMenu = sMenu + "<li>" & "<a href=""javascript:return false;""><span>" & a.HrefName & "</span></a>" & vbCrLf
                        ' Draw subMenu Here
                        Dim sSubMenu As String = ""
                        sSubMenu = SubMenu(a.FunctionId, i, "select_sub")
                        sMenu = sMenu & sSubMenu
                        sMenu = sMenu & "</li>" & vbCrLf
                        sMenu = sMenu & "</div>" & vbCrLf
                        sMenu = sMenu & "</ul>" & vbCrLf
                    End If
                    i = i + 1
                Next
            End If
        End Using

        Return sMenu

    End Function

    Private Function SubMenu(ByVal FunctionId As Integer, ByVal padding As Integer, ByVal classOpen As String) As String
        Dim strDes As String = ""
        Dim sufixURL = Request.Url.ToString.Split("Page")(0)
        Dim arrFunctionId As Integer() = Session("Function")
        Using data As New ThanhTraLaoDongEntities
            'get first menu
            Dim p As List(Of ThanhTraLaoDongModel.Function) = (From q In data.Functions Where q.ParentId = FunctionId _
                                                               And q.IsMenu = True _
                                                               And arrFunctionId.Contains(q.FunctionId) Order By q.Sort Ascending Select q).ToList

            'Get ChildNode value for each node
            strDes = strDes & "<div class=""" & classOpen & """ id=""sub_id"">" & vbCrLf
            strDes = strDes & "<ul class=""sub_" & padding & """>" & vbCrLf

            If Not p Is Nothing Then
                For Each a As ThanhTraLaoDongModel.Function In p
                    'Take ID querystring in other to set active
                    If CheckOpenItem(a, arrFunctionId) Then
                        strDes = strDes + "<li class='ActiveSubMenu'>" & "<a   href=" & sufixURL & a.URL & ">" & a.HrefName & "</a></li>" & vbCrLf
                    Else
                        strDes = strDes + "<li class='NormalSubMenu'>" & "<a  href=" & sufixURL & a.URL & ">" & a.HrefName & "</a></li>" & vbCrLf
                    End If
                Next
            End If

            strDes = strDes & "</ul>" & vbCrLf
            strDes = strDes & "</div>" & vbCrLf

        End Using
        Return strDes

    End Function

    Public Function CheckOpenItem(ByVal a As ThanhTraLaoDongModel.Function, ByVal arrFunctionId As Integer()) As String

        Dim sufixURL = Request.Url.ToString.Split("Page")(0)
        Dim PureURL = Request.Url.ToString.Replace(sufixURL, "")
        'neu la report thi active khac
        If Not PureURL.Contains("Page/Report/") Then
            '' Nếu là Tra cứu thì active khác
            If Not PureURL.Contains("Page/TraCuu/") Then
                PureURL = PureURL.Substring(0, PureURL.LastIndexOf("/") + 1)
            Else
                PureURL = PureURL.Replace("Page/TraCuu/", "")
            End If
        Else
            PureURL = PureURL.Replace("Page/Report/", "")
        End If


        If Not a.URL Is Nothing Then
            ' PureURL = a.URL.Replace("List.aspx", "")
            If a.URL.Contains(PureURL) Then
                Return 1
            End If

        End If

        Using data As New ThanhTraLaoDongEntities
            'lay ra FunctionId cua cur Menu
            Dim p = (From q In data.Functions Where q.URL.Contains(PureURL) And arrFunctionId.Contains(q.FunctionId) Select q).FirstOrDefault

            If Not p Is Nothing Then
                Dim ParentId As Integer = p.ParentId
                'lay menu cha
                Dim k = (From q In data.Functions Where q.FunctionId = ParentId Select q).FirstOrDefault
                If Not k Is Nothing Then
                    'kiem tra neu = FuncionId truyen vao
                    If k.FunctionId = a.FunctionId Then
                        Return 1
                    End If
                End If
            End If
        End Using
        Return 0
    End Function
End Class
