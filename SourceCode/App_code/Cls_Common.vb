Imports ThanhTraLaoDongModel
Imports Microsoft.VisualBasic
'Imports System.Net.Mail
Imports System.Data
Imports System.IO
Imports System.Collections
Imports System.Collections.Generic
Imports System.Linq
Imports System.Xml.Linq
Imports System.Web.UI
Imports System.Web.UI.HtmlControls
Imports Excel = Microsoft.Office.Interop.Excel
Imports Microsoft.Office.Interop
Imports KillProcessExcel
Imports System.Text
'//them 15/09
Imports System.Object
Imports System.Security.Cryptography
Imports Rebex.Mime.Headers

'Imports App_Code.DAClass
''Mail
'Imports Rebex.Mail
'Imports Rebex.Mime.Headers
'Imports Rebex.Security nghia


Public Class Cls_Common
    Public Shared ReadOnly log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
#Region "Common String"
    Public Shared strLetterTotal() As String = {"A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z"}
    Public Shared strA() As String = {"A", "Á", "Ả", "Ã", "À", "Ạ", "Â", "Ấ", "Ẩ", "Ẫ", "Ầ", "Ậ", "Ă", "Ắ", "Ẳ", "Ẵ", "Ặ", "Ằ"}
    Public Shared strD() As String = {"D", "Đ"}
    Public Shared strE() As String = {"E", "É", "È", "Ẻ", "Ẽ", "Ẹ", "Ê", "Ế", "Ề", "Ể", "Ễ", "Ệ"}
    Public Shared strI() As String = {"I", "Í", "Ị", "Ì", "Ỉ", "Ĩ"}
    Public Shared strO() As String = {"O", "Ó", "Ò", "Ỏ", "Õ", "Ọ", "Ô", "Ố", "Ồ", "Ổ", "Ỗ", "Ộ", "Ơ", "Ớ", "Ờ", "Ợ", "Ở", "Ỡ"}
    Public Shared strU() As String = {"U", "Ú", "Ù", "Ủ", "Ũ", "Ụ", "Ư", "Ứ", "Ừ", "Ử", "Ữ", "Ự"}
    Public Shared strY() As String = {"Y", "Ý", "Ỳ", "Ỷ", "Ỹ", "Ỵ"}
    Public Shared Start_Page As String = "~/UI/Pages/MasterPages/Homepage.aspx"
    Public Shared Login_Page As String = "~/UI/Pages/MasterPages/Login.aspx"
    Public Shared Lead_Source As String = "Lead_Source"
    Public Shared Lead_Status As String = "Lead_Status"
    Public Shared Industry As String = "Industry"
    Public Shared strDigit_group_symbol As String = ","
    Public Shared Str_login_failure As String = "Tên đăng nhập,mật khẩu hoặc quyền truy cập không hợp lệ."
    Public Shared Str_not_right_to_access As String = "Bạn chưa được cấp quyền truy nhập site này.Hãy liên hệ với Administrator."
    Public Shared Str_update_success As String = "Cập nhật thành công."
    Public Shared Str_update_failure As String = "Cập nhật thất bại."
    Public Shared Str_delete_success As String = "Xóa thành công."
    Public Shared Str_delete_failure As String = "Xóa thất bại."
    Public Shared Str_confirm_delete As String = "Bạn có chắc chắn muốn xóa bản ghi này không?"
    Public Shared Str_confirm_multi_delete As String = "Bạn có chắc chắn muốn xóa bản ghi đã chọn?"
    Public Shared Str_confirm_select_delete As String = "Bạn phải chọn ít nhất một bản ghi."
    Public Shared Str_error_select_para As String = "Không tìm thấy tham số hệ thống."
    Public Shared Str_Exist_User As String = "Đã tồn tại người dùng này trong hệ thống."
    Public Shared Str_Opt_Chon As String = "--- Chọn ---"
    Public Shared Str_Opt_TatCa As String = "--- Tất cả ---"
    Public Shared Str_Opt_ToanQuoc As String = "--- Toàn quốc ---"
    Public Shared Str_Symbol_Group As String = "#"
    'Qui định định dạng dấu phân cách thập phân theo khu vực
    Public Shared info As System.Globalization.CultureInfo = System.Globalization.CultureInfo.GetCultureInfo("vi-VN")
    Public Shared MainDomain As String = ConfigurationManager.AppSettings("MainDomain")

#End Region
#Region "Common Property"
    Public Enum ThamSoSys
        LoaiHinhDN = 2 'Cho biết loại hình doanh nghiệp
        PhanTramKhamSucKhoeDinhKy = 20
        SoSanhNamThanhLapDN = 5
    End Enum
    Public Enum Function_Name
        User = 2
        Role = 3
        SysFuncRolesStatusPermission = 4
        Log = 5
        Ytgchanthuong = 6
        PhieuKiemTra = 12
        BienBanThanhTra = 14
		PhieuKiemTra_XLP = 87
        BienBanThanhTra_TT = 96
        DoanhNghiepBBTT = 113
        DoanhNghiepPTKT = 114
        DoanhNghiepTT = 95
        DoanhNghiepXLP = 86
        TinhTP = 22
        LoaiHinhDN = 23
        LoaiHinhSanXuat = 24
        QuanHuyen = 26
        HanhVi = 37
        BaoCaoKetQuaThanhTra1 = 30
        BaoCaoKetQuaThanhTra2 = 31
        BaoCaoPhanTichTinhVPLD1 = 32
        BaoCaoPhanTichTinhVPLD2 = 33
        BaoCao_PhieuKetQuaThanhTra = 38
        Phieu_BaocaoPhanTichTinhVPLD = 39
        KienNghi = 43
        LuongToiThieu = 44
        DanhMucCauHoi = 46
        FunctionSys = 54
        LoaiHanhVi = 55
        DanhMucTrichDan = 56

        GopY = 73
        QuyetDinhThanhTra = 74
        KhuCongNghiep = 1
    End Enum
    ''' <summary>
    ''' Mode của phiếu nhập
    ''' </summary>
    ''' <remarks></remarks>
    Public Enum ModePhieu
        Create = 1
        Edit = 2
        Detail = 3
    End Enum
    ''' <summary>
    ''' Xét trường hợp mục kết luận là vi phạm hay không ?
    ''' </summary>
    ''' <remarks></remarks>
    Public Enum TypeViPham
        ViPham = 0
        KhongViPham = 1
        KhongXet = 2
    End Enum
    ''' <summary>
    ''' Loại biên bản là thanh tra hay phiếu kết luận
    ''' </summary>
    ''' <remarks></remarks>
    Public Enum TypeBienBan
        BienBanThanhTra = True
        PhieuKetLuan = False
    End Enum
    ''' <summary>
    '''  Loại Doanh nghiệp để xét mức lương tối thiểu
    ''' </summary>
    ''' <remarks></remarks>
    Public Enum Luong_Type
        NhaNuoc = 1
        DoanhNghiep = 0
    End Enum
    Public Enum Audit_Type
        Create = 1
        Edit = 2
        Delete = 4
        ViewContent = 8
        Submit = 16
        Publish = 32
    End Enum

    Public Shared ArrStatus_System As String() = {"Success", "Failure"}
    Public Enum Status_System
        Success = 0
        Failure = 1
    End Enum

    Public Shared ArrAction_System As String() = {"OpenConnection", "CloseConnection", "GetUserDomain"}
    Public Enum Action_System
        OpenConnection = 0
        CloseConnection = 1
        GetUserDomain = 2
    End Enum

    'Comment : 15/12/2010
    'Author : TinhNT
    'Output : 
    'Input: du lieu bang APP_EVENT
    Public Enum Action_User
        Insert = 1
        Update = 2
        Delete = 3
        Inactive_User = 4
        Active_User = 5
    End Enum
    Public Shared ArrUser_Type As String() = {"Quản trị", "Thanh tra", "Doanh nghiệp"}

    Public Enum User_Status
        Inactive = 0
        Active = 1
    End Enum

    Public Enum Field_Type
        Strings = 1
        Numbers = 2
        DateTimes = 3
    End Enum

    Public Enum Assign_To
        User = 0
        Group = 1
    End Enum

    '' TuanCd 19/12/2012
    '' Loại User
    Public Enum UserType
        ThanhTra = 1
        DoanhNghiep = 2
        Admin = 3
    End Enum

    Public Shared ReadOnly Property Old_Cus_Days() As Integer
        Get
            If Not System.Configuration.ConfigurationManager.AppSettings("Old_Cus_Days") Is Nothing Then
                Return Integer.Parse(System.Configuration.ConfigurationManager.AppSettings("Old_Cus_Days"))
            Else
                Return 1
            End If
        End Get
    End Property

    Public Shared ReadOnly Property Old_Cus_Interest() As Decimal
        Get
            If Not System.Configuration.ConfigurationManager.AppSettings("Old_Cus_Interest") Is Nothing Then
                Return Decimal.Parse(System.Configuration.ConfigurationManager.AppSettings("Old_Cus_Interest"))
            Else
                Return 1
            End If
        End Get
    End Property

#End Region

#Region "Common Function & Sub"

    'Cách sử dụng Log
    'log.Error("Error error " & AddTabSpace(1) & Session("Username") & AddTabSpace(1) & "IP:" & GetIPAddress(), ex)
    'log.Info("Info error " & AddTabSpace(1) & Session("Username") & AddTabSpace(1) & "IP:" & GetIPAddress(), ex)
    'log.Fatal("Fatal error " & AddTabSpace(1) & Session("Username") & AddTabSpace(1) & "IP:" & GetIPAddress(), ex)
    'log.Warn("Warn error " & AddTabSpace(1) & Session("Username") & AddTabSpace(1) & "IP:" & GetIPAddress(), ex)
    'Lay dia chi IP cua may Client
    Public Shared Function GetIPAddress() As String
        Dim context As System.Web.HttpContext =
            System.Web.HttpContext.Current

        Dim sIPAddress As String =
            context.Request.ServerVariables("HTTP_X_FORWARDED_FOR")

        If String.IsNullOrEmpty(sIPAddress) Then
            Return context.Request.ServerVariables("REMOTE_ADDR")
        Else
            Dim ipArray As String() = sIPAddress.Split(
                New [Char]() {","c})
            Return ipArray(0)
        End If
    End Function

    'Lay thoi gian ghi log
    Public Shared Function CreateLogFiles() As String()
        Dim arr() As String
        'sLogFormat used to create log files format :
        ' dd/mm/yyyy hh:mm:ss AM/PM ==> Log Message
        Dim sErrorTime As String
        sErrorTime = DateTime.Now.ToShortDateString().ToString() + " " + DateTime.Now.ToLongTimeString().ToString() + " ==> "

        'this variable used to create log filename format "
        'for example filename : ErrorLogYYYYMMDD
        Dim sLogFormat As String
        Dim sYear As String = DateTime.Now.Year.ToString()
        Dim sMonth As String = IIf(CType(DateTime.Now.Month, Int32) < 10, "0" + DateTime.Now.Month.ToString, DateTime.Now.Month.ToString)
        Dim sDay As String = IIf(CType(DateTime.Now.Day, Int32) < 10, "0" + DateTime.Now.Day.ToString, DateTime.Now.Day.ToString)
        sLogFormat = sYear & "_" & sMonth & "_" & sDay
        arr = (sErrorTime + "$" + sLogFormat).Split("$")
        Return arr
    End Function
    'ghi log ket noi vao he thong
    'vd: Insert_Sys_Log(CreateLogFiles(), ArrAction_System(Action_System.CloseConnection), ArrModule_System(Module_System.QuanTriNguoiDung), ArrStatus_System(Status_System.Failure), ex.Message.ToString, GetIPAddress())
    'Public Shared Sub Insert_Sys_Log(ByVal arrLogTime() As String, ByVal ActionID As String, ByVal FunctionID As String, ByVal Status As String, ByVal Msg As String, ByVal client As String)
    '    Dim sw As StreamWriter = New StreamWriter(HttpContext.Current.Server.MapPath("~/LogFiles/" + arrLogTime(1).ToString + ".log"), True)
    '    Dim strLog As String
    '    strLog = "#" & arrLogTime(0).ToString & AddTabSpace(1) & AddTabSpace(1) & ActionID & AddTabSpace(1) & FunctionID & AddTabSpace(1) & Status & AddTabSpace(1) & Msg & AddTabSpace(1) & "IP" & AddTabSpace(1) & client
    '    sw.WriteLine(strLog)
    '    sw.Flush()
    '    sw.Close()
    'End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="Obj"></param>
    ''' <returns></returns>
    ''' <remarks>Se viet lai doi so khong nhan chuoi</remarks>
    Public Shared Function Format_CRM_Object(ByVal Obj As Object) As Object
        If Not IsDBNull(Obj) Then
            If TypeOf Obj Is String Then
                Return Obj
            ElseIf IsDate(Obj) Then
                Dim reg As Regex = New Regex("^([0-1][0-9]|2[0-3]):[0-5][0-9]:[0-5][0-9]$")
                If reg.IsMatch(Obj) Then Return Obj
                Return Date.Parse(Obj).ToString("dd/MM/yyyy")
            Else
                If TypeOf Obj Is Integer Or (Obj Mod 1 = 0) Then
                    Return String.Format("{0:N0}", Obj)
                Else
                    Return String.Format("{0:N}", Obj)
                End If
            End If
        Else
            Return ""
        End If
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="Obj"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function Process_CRM_Special_SQL(ByVal Obj As String) As String
        Obj = Obj.Replace("_", "[_]")
        Obj = Obj.Replace("[", "[[]")
        Obj = Obj.Replace("%", "[%]")
        Return Obj
    End Function

    Public Shared Sub RenderControl_Excel(ByVal Response As Object, ByVal mk As HtmlGenericControl, Optional ByVal obj As Page = Nothing, Optional ByVal nameSheet As String = Nothing)
        Response.Clear()
        Dim tw As New System.IO.StringWriter()
        Dim hw As New System.Web.UI.HtmlTextWriter(tw)
        Dim form As New HtmlForm
        Response.ContentType = "application/ms-excel"
        If nameSheet = Nothing Then
            nameSheet = "Nova\ExcelReportCRM.xls"
        End If
        Response.AppendHeader("Content-Disposition", "attachment; filename=" + nameSheet) 'Nova\ExcelReportCRM.xls
        'Dim img_ As New Image
        'img_.ImageUrl = "~/UI/Images/logo_report.png"
        'img_.RenderControl(hw)

        If obj IsNot Nothing Then ' Hientv giai quyet khi reder gridview
            form.Controls.Add(mk)
            obj.Controls.Add(form)
            form.RenderControl(hw)
        Else
            mk.RenderControl(hw)
        End If
        'Append CSS file 21/07 duynp
        Dim fi As FileInfo = New FileInfo(System.Web.HttpContext.Current.Server.MapPath("~/UI/CSS/style.css"))
        Dim sb As New System.Text.StringBuilder
        Dim sr As StreamReader = fi.OpenText()
        Do While sr.Peek() >= 0
            sb.Append(sr.ReadLine())
        Loop
        sr.Close() '-------------------
        Response.Write("<html><head><style type='text/css'>" & sb.ToString() & "</style><head>" & tw.ToString() & "</html>")
        'Response.Write(tw.ToString())
        Response.Flush()
        Response.End()
    End Sub


    'Xoa file trong Path dua vao
    Private Shared Sub DeleteFileInPath(ByVal strPath As String)
        Try
            Dim dir As DirectoryInfo
            dir = New DirectoryInfo(strPath)
            If dir.GetFiles().Length > 0 Then
                For Each f As FileInfo In dir.GetFiles()
                    f.Delete()
                Next
            Else
                'khong co file
            End If
        Catch ex As Exception
        End Try
    End Sub
    Public Shared Sub Message(ByVal strMessage As String, ByVal cls As Object, Optional ByVal addScriptTags As Boolean = True)
        strMessage = strMessage.Replace("'", "\'")
        ScriptManager.RegisterStartupScript(cls, cls.GetType(), "javascriptfunction", "alert('" & strMessage & "');", addScriptTags)
    End Sub

    Public Shared Sub Excute_Javascript(ByVal strFunction As String, ByVal cls As Object, Optional ByVal addScriptTags As Boolean = True)
        'strFunction = strFunction.Replace("'", "\'")
        ScriptManager.RegisterStartupScript(cls, cls.GetType(), "javascriptfunction", strFunction, addScriptTags)
    End Sub

    Private Shared Function ProcessVal(ByVal pvstr As String) As String
        Dim s As String
        s = pvstr
        s = Replace(s, "//", " ")
        s = Replace(s, ".", " ")
        s = Replace(s, ",", " ")
        s = Replace(s, ":", " ")
        s = Replace(s, ";", " ")
        s = Replace(s, "?", " ")
        s = Replace(s, "!", " ")
        s = Replace(s, "[", " ")
        s = Replace(s, "]", " ")
        s = Replace(s, "=", " ")
        s = Replace(s, "(", " ")
        s = Replace(s, ")", " ")
        s = Replace(s, """", " ")
        s = Replace(s, "--", " ")
        Do While InStr(s, "  ") > 0
            s = Replace(s, "  ", " ")
        Loop
        Return s
    End Function

    Public Shared Function killChars(ByVal strWords As String) As String
        Dim badChars() As String = {"select", "drop", ";", "--", "insert", "delete", "xp_", "and"} 'bo tu :"or" vi xung dot voi Administrator - TinhNT
        Dim newChars As String
        newChars = strWords
        'newChars = ProcessVal(strWords)
        newChars = Replace(newChars, "'", "''")
        For i As Integer = 0 To UBound(badChars)
            newChars = Replace(newChars, badChars(i), "")
        Next
        killChars = newChars
    End Function

    Public Shared Function AddSTT(ByVal dt As DataTable, ByVal NameCol As String) As DataTable
        Dim dtTmp As DataTable = dt
        dtTmp.Columns.Add(New DataColumn(NameCol))
        For i As Int32 = 0 To dtTmp.Rows.Count - 1
            dtTmp.Rows(i)(NameCol) = i
        Next
        Return dtTmp
    End Function

    'Encrypt for MD5
    Public Shared Function Encrypt(ByVal mess As String) As String
        Dim x As System.Security.Cryptography.MD5CryptoServiceProvider = New System.Security.Cryptography.MD5CryptoServiceProvider
        Dim bs() As Byte = System.Text.Encoding.UTF8.GetBytes(mess)
        bs = x.ComputeHash(bs)
        Dim s As System.Text.StringBuilder = New System.Text.StringBuilder()
        For Each b As Byte In bs
            s.Append(b.ToString("x2").ToLower())
        Next
        Return s.ToString().ToUpper
    End Function
    'Hàm xử lý Date
    '2008-10-09 Phuongnt
    Public Shared Function StringToDate(ByVal strVal As String, Optional ByVal strFormat As String = "dd/mm/yyyy") As Date
        Dim a() As String
        Dim KQ As Date
        a = Split(strVal, "/")
        Try
            Select Case strFormat.ToUpper
                Case "DD/MM/YYYY"
                    KQ = New Date(CInt(a(2)), CInt(a(1)), CInt(a(0)))
                Case "MM/DD/YYYY"
                    KQ = New Date(CInt(a(2)), CInt(a(0)), CInt(a(1)))
                Case "YYYY/MM/DD"
                    KQ = New Date(CInt(a(0)), CInt(a(1)), CInt(a(2)))
            End Select
        Catch ex As Exception
            KQ = New Date(1900, 1, 1)
        End Try
        Return KQ
    End Function

    Public Shared Function DateToString(ByVal strVal As String, Optional ByVal strFormat As String = "mm/dd/yyyy") As String
        Dim ar() As String = strVal.Split(" ")
        Dim strcheck As String
        Dim a() As String
        Dim KQ As String = ""
        If ar.Length > 1 Then
            strcheck = ar(0)
        Else
            strcheck = strVal.Trim
        End If
        a = Split(strcheck, "/")
        Try
            Select Case strFormat.ToUpper
                Case "MM/DD/YYYY"
                    KQ = CStr(a(1)) & "/" & CStr(a(0)) & "/" & CStr(a(2))
                    'Thaivv thêm trường hợp này vào
                Case "YYYY/MM/DD"
                    KQ = CStr(a(2)) & "/" & CStr(a(1)) & "/" & CStr(a(0))
            End Select
        Catch ex As Exception
        End Try
        Return KQ
    End Function
    ' format Date co kiem tra DBNull
    Public Shared Function formatDate(ByVal objDate As Object) As String
        If Not IsDBNull(objDate) Then
            Return Date.Parse(objDate).ToString("dd/MM/yyyy")
        End If
        Return ""
    End Function
    'Hientv convert strDate(dd/mm/yyyy),strTime(hh:mm) -> DateTime
    Public Shared Function StringToDateTime(ByVal strDate As String, ByVal strTime As String) As DateTime
        If Not String.IsNullOrEmpty(strDate) And Not String.IsNullOrEmpty(strTime) Then
            Dim arrDate = strDate.Split("/")
            Dim arrTime = strTime.Split(":")
            'dd/mm/yyyy
            Dim newDate = New DateTime(arrDate(2), arrDate(1), arrDate(0), arrTime(0), arrTime(1), 0)
            Return newDate
        End If
        Return Nothing
    End Function
    Public Shared Function FindAndReplace(ByVal obj As Object, ByVal FindVal As Object, ByVal ReplaceVal As Object) As Object
        If obj = FindVal Then
            Return ReplaceVal
        End If
        Return obj
    End Function

    Public Shared Sub AddConfirmDelete(ByVal gv As GridView, ByVal e As GridViewRowEventArgs, ByVal Alertmessage As String)
        If e.Row.RowType = DataControlRowType.DataRow Then
            For Each dcf As DataControlField In gv.Columns
                If dcf.ToString() = "CommandField" Then
                    If CType(dcf, CommandField).ShowDeleteButton = True Then
                        e.Row.Cells(gv.Columns.IndexOf(dcf)).Attributes.Add("onclick", "return confirm('" & Alertmessage & "');")
                    End If
                End If
            Next
        End If
    End Sub

    Public Shared Function RandomString() As String
        '========================
        'RETURNS single char
        '========================
        '0  - 25 Uppercase A - Z
        '26 - 51 Lowercase a - z
        '52 - 61 Numbers   0 - 9
        Dim validChar() As String = {"A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z", "0", "1", "2", "3", "4", "5", "6", "7", "8", "9"}
        Randomize()
        Return validChar(Int((61 * Rnd()) + 1))
    End Function

    Public Shared Function Get_Address_Name(ByVal strName As String) As String
        Dim tmp As String
        If strName <> "" Then
            Dim first, last, len As Integer
            last = 0
            len = strName.Length
            first = strName.LastIndexOf("/")
            last = strName.LastIndexOf("?")
            If last <> -1 Then
                Dim lens As Integer = 0
                lens = last - (first + 1)
                tmp = strName.Substring(first + 1, lens)
            Else
                tmp = strName.Substring(first + 1)
            End If
            Get_Address_Name = tmp
            Return Get_Address_Name
        Else
            Return ""
        End If
    End Function
    Public Shared Function SendMailOutlook(ByVal strReceiverEMail As String, ByVal strSubject As String, ByVal strContent As String, ByVal strReceiverCCEMail As String, ByVal strReceiverBCCEMail As String, Optional ByVal pSystemName As String = "System_Name_Here") As Boolean
        'Dim oApp As Microsoft.Office.Interop.Outlook.Application
        'Dim oNS As Microsoft.Office.Interop.Outlook.NameSpace
        'Dim oObFld As Microsoft.Office.Interop.Outlook.MAPIFolder
        'Dim oMail As Microsoft.Office.Interop.Outlook.MailItem
        Try
            '    oApp = New Microsoft.Office.Interop.Outlook.Application
            '    oNS = oApp.GetNamespace("MAPI")
            '    oNS.Session.Logon("", "", False, False)
            '    oObFld = oNS.GetDefaultFolder(Microsoft.Office.Interop.Outlook.OlDefaultFolders.olFolderOutbox)
            '    oMail = oObFld.Items.Add(Microsoft.Office.Interop.Outlook.OlItemType.olMailItem)
            '    oMail.To = strReceiverEMail
            '    oMail.CC = strReceiverCCEMail
            '    oMail.BCC = strReceiverBCCEMail
            '    oMail.Subject = strSubject
            '    oMail.BodyFormat = Microsoft.Office.Interop.Outlook.OlBodyFormat.olFormatHTML
            '    oMail.Body = strContent
            '    oMail.DeleteAfterSubmit = True
            '    oMail.Send()
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function
    Public Shared Function SMTP_SERVER(ByVal dt As Object) As String
        If Not dt Is Nothing Then
            Try
                Dim data As DataTable = CType(dt, DataTable)
                Dim dv As DataView = data.DefaultView
                dv.RowFilter = "Name like 'SMTP_SERVER'"
                If dv.Count = 1 Then
                    Return dv.Item(0).Item("Val")
                End If
            Catch ex As Exception
                Return ""
            End Try
        End If
        Return ""
    End Function
    Public Shared Function ADMIN_EMAIL_ADDRESS(ByVal dt As Object) As String
        If Not dt Is Nothing Then
            Try
                Dim data As DataTable = CType(dt, DataTable)
                Dim dv As DataView = data.DefaultView
                dv.RowFilter = "Name like 'ADMIN_EMAIL_ADDRESS'"
                If dv.Count = 1 Then
                    Return dv.Item(0).Item("Val")
                End If
            Catch ex As Exception
                Return ""
            End Try
        End If
        Return ""
    End Function
    Public Shared Function ADMIN_EMAIL_PASSWORD(ByVal dt As Object) As String
        If Not dt Is Nothing Then
            Try
                Dim data As DataTable = CType(dt, DataTable)
                Dim dv As DataView = data.DefaultView
                dv.RowFilter = "Name like 'ADMIN_EMAIL_PASSWORD'"
                If dv.Count = 1 Then
                    Return dv.Item(0).Item("Val")
                End If
            Catch ex As Exception
                Return ""
            End Try
        End If
        Return ""
    End Function

    Public Shared Function SMTP_PORT(ByVal dt As Object) As String
        If Not dt Is Nothing Then
            Try
                Dim data As DataTable = CType(dt, DataTable)
                Dim dv As DataView = data.DefaultView
                dv.RowFilter = "Name like 'SMTP_PORT'"
                If dv.Count = 1 Then
                    Return dv.Item(0).Item("Val")
                End If
            Catch ex As Exception
                Return ""
            End Try
        End If
        Return ""
    End Function

    Public Shared Function SMTP_SSL(ByVal dt As Object) As Boolean
        If Not dt Is Nothing Then
            Try
                Dim data As DataTable = CType(dt, DataTable)
                Dim dv As DataView = data.DefaultView
                dv.RowFilter = "Name like 'SMTP_SSL'"
                If dv.Count = 1 Then
                    Return CType(dv.Item(0).Item("Val"), Boolean)
                End If
            Catch ex As Exception
                Return ""
            End Try
        End If
        Return ""
    End Function

    'Hientv
    Public Shared Function ConvertChar_VN_To_Eng(ByVal strChar As String) As String
        Dim strUpChar = strChar.ToString.ToUpper
        If Array.IndexOf(strA, strUpChar) > -1 Then Return "A"
        If Array.IndexOf(strD, strUpChar) > -1 Then Return "D"
        If Array.IndexOf(strE, strUpChar) > -1 Then Return "E"
        If Array.IndexOf(strI, strUpChar) > -1 Then Return "I"
        If Array.IndexOf(strO, strUpChar) > -1 Then Return "O"
        If Array.IndexOf(strU, strUpChar) > -1 Then Return "U"
        If Array.IndexOf(strY, strUpChar) > -1 Then Return "Y"
        Return strUpChar
    End Function

    'Hientv
    Public Shared Function Generate_StrSQL_Filter_By_Char(ByVal strFilter As String, ByVal strFieldName As String) As String
        Dim strKeySearch As String = ""
        Dim arrChar As New ArrayList
        arrChar.Add(strA)
        arrChar.Add(strE)
        arrChar.Add(strD)
        arrChar.Add(strI)
        arrChar.Add(strO)
        arrChar.Add(strU)
        arrChar.Add(strY)
        For Each itm As String() In arrChar
            If Array.IndexOf(itm, strFilter) > -1 Then
                For i As Integer = 0 To itm.Length - 1
                    strKeySearch += " Substring(" + strFieldName + ",1,1)" + " Like '" + itm(i) + "%'"
                    If i < itm.Length - 1 Then
                        strKeySearch += " OR "
                    End If
                Next
                Return strKeySearch
            End If
        Next
        strKeySearch = " Substring(" + strFieldName + ",1,1)" + " Like '" + strFilter + "%'"
        Return strKeySearch
    End Function

    'Public Shared Function Search_Linq_By_Char(ByVal dt As DataTable, ByVal obj As Object, ByVal strFilter As String, ByVal strFieldName As String) As IList
    '    Dim arrChar As New ArrayList
    '    Dim query As List(Of Object)
    '    arrChar.Add(strA)
    '    arrChar.Add(strE)
    '    arrChar.Add(strD)
    '    arrChar.Add(strI)
    '    arrChar.Add(strO)
    '    arrChar.Add(strU)
    '    arrChar.Add(strY)
    '    For Each itm As String() In arrChar
    '        If Array.IndexOf(itm, strFilter) > -1 Then
    '            For i As Integer = 0 To itm.Length - 1
    '                strKeySearch += strFieldName + " Like '" + itm(i) + "%'"
    '                If i < itm.Length - 1 Then
    '                    strKeySearch += " OR "
    '                End If
    '            Next
    '            Return strKeySearch
    '        End If
    '    Next

    'End Function

    'Hientv
    Public Shared Sub DeleteFile(ByVal strPath As String)
        If File.Exists(strPath) Then
            Dim fileInfo As FileAttribute = File.GetAttributes(strPath)
            If fileInfo = FileAttribute.ReadOnly Then
                File.SetAttributes(strPath, FileAttributes.Archive)
            End If
            File.Delete(strPath)
        End If
    End Sub

    Public Shared Function ConvertDateBack(ByVal strDateTime As String, Optional ByVal blnShowTime As Int32 = 0, Optional ByVal strTypeCurent As String = "DD/MM/YYYY") As Object
        Dim strDate As String = ""
        Dim strTime As String = ""
        Dim strDeli As String = "/"
        Dim strRet As String = ""
        Dim strPara(1) As String
        Dim strARet(1) As String
        strDateTime = Trim(strDateTime)
        If strDateTime <> "" Then
            If InStr(strDateTime, " ") > 0 Then
                strDate = Left(strDateTime, InStr(strDateTime, " ") - 1)
                strTime = Trim(Right(strDateTime, Len(strDateTime) - InStr(strDateTime, " ")))
            Else
                strDate = strDateTime
            End If
            If InStr(strDate, "/") <> 0 Then
                strDeli = "/"
            End If
            If InStr(strDate, "-") <> 0 Then
                strDeli = "-"
            End If
        Else
            Return DBNull.Value
        End If
        If strDeli <> "" And strDate <> "" Then
            Dim bytPos1 As Byte
            Dim bytPos2 As Byte
            bytPos1 = InStr(strDate, strDeli)
            bytPos2 = InStrRev(strDate, strDeli)
            ' convert to format mm/dd/yyyy
            Select Case UCase(strTypeCurent)
                Case "DD/MM/YYYY"
                    strRet = Mid(strDate, bytPos1 + 1, bytPos2 - bytPos1 - 1) & "/" & Left(strDate, bytPos1 - 1) & "/" & Right(strDate, Len(strDate) - bytPos2)
                Case "MM/DD/YYYY"
                    strRet = strDate
                Case "YYYY/DD/MM"
                    strRet = Right(strDate, Len(strDate) - bytPos2) & "/" & Mid(strDate, bytPos1 + 1, bytPos2 - bytPos1 - 1) & "/" & Left(strDate, bytPos1 - 1)
                Case "YYYY/MM/DD"
                    strRet = Mid(strDate, bytPos1 + 1, bytPos2 - bytPos1 - 1) & "/" & Right(strDate, Len(strDate) - bytPos2) & "/" & Left(strDate, bytPos1 - 1)
            End Select
            If blnShowTime = 1 Then
                If strTime <> "" Then
                    strRet = strRet & " " & strTime
                Else
                    strRet = strRet & " 00:00:00"
                End If
            ElseIf blnShowTime = 2 Then
                If strTime <> "" Then
                    strRet = strRet & " " & strTime
                Else
                    strRet = strRet & " 23:59:59"
                End If
            End If
        Else
            strRet = strDateTime
        End If
        Return strRet
    End Function

    ' Format so theo chuoi dinh dang
    Public Shared Function FormatNumber(ByVal obj As Object) As Object
        If IsNumeric(obj) Then
            Return Decimal.Parse(obj).ToString("#,###")
        End If
        Return obj
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="str">12345661456.23</param>
    ''' <param name="group"></param>
    ''' <param name="dec"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function FormatCurrency(ByVal str As String, Optional ByVal group As String = ",", Optional ByVal dec As String = ".") As String
        Return Format(CType(str, Double), "#,##0.###")
        'Dim str_before As String = ""
        'Dim str_after As String = ""
        'If str.IndexOf(".") <> -1 Then
        '    str_before = str.Substring(0, str.IndexOf("."))
        '    'str_after = str.Replace(str_before, "").Replace(".", "")
        '    str_after = str.Split(".")(1)


        'Else
        '    str_before = str
        'End If
        'Dim strreturn As String = ""
        'str_before = StrReverse(str_before)
        'While str_before.Length > 0
        '    If str_before.Length > 3 Then
        '        strreturn &= str_before.Substring(0, 3) & group : str_before = str_before.Substring(3)
        '    Else
        '        strreturn &= str_before : Exit While
        '    End If
        'End While
        'str_before = StrReverse(strreturn)
        ''If str_after <> "" AndAlso str_after <> "." AndAlso Integer.Parse(str_after.Substring(1)) > 0 Then
        'If str_after <> "" AndAlso str_after <> "." Then ' AndAlso Integer.Parse(str_after.Substring(1)) > 0 Then
        '    Return str_before & dec & str_after
        'Else
        '    Return str_before
        'End If
    End Function

    Public Shared Function Format_Currency(ByVal str As String) As String
        Dim strNumber As String = ""
        strNumber += str.Split(".")(0).Replace(",", ".")
        If str.IndexOf(".") <> -1 Then
            Return strNumber + "," + str.Split(".")(1)
        End If
        Return strNumber

    End Function
#End Region

    ''' <summary>
    ''' Cắt bỏ tất cả mọi số không ở phía sau số và trả về chuỗi
    ''' Nếu đó là số chẵn thì trả về là số nguyên bỏ đi dấu chấm
    ''' Ngược lại thì chỉ bỏ số không
    ''' </summary>
    ''' <param name="str">Số truyền vào được chuyển thành chuỗi</param>
    ''' <returns>Trả về chuỗi đã hoàn thiện</returns>
    ''' <remarks>Của Lê Minh Thành</remarks>
    ''' 
    Public Shared Function Del_Zero_Of_Number(ByVal str As String) As String
        If str.IndexOf(".") <> -1 Then
            For i = str.Length - 1 To 0 Step -1
                If str(i) = "0" Then
                    str = str.Remove(i)
                Else
                    If str.Length = str.IndexOf(".") + 1 Then
                        str = str.Remove(str.IndexOf("."))
                    End If
                    Return str
                End If
            Next
        End If
        Return str
    End Function

    ''' <summary>
    ''' Change_Money_To_String
    ''' Dùng để đọc tiền
    ''' Giá trị thấp nhất là 0 đồng
    ''' Cao nhất là tỷ tỷ
    ''' </summary>
    ''' <param name="str">Số tiền được chuyển vào phải convert sang chuổi</param>
    ''' <returns>trả về chuổi tiền đọc được</returns>
    ''' <remarks>Chuổi tiền cần đọc phải hoàn toàn là số, không có chấm hoặc phẩy</remarks>
    Public Shared Function Change_Money_To_String(ByVal str As String) As String
        Try
            If str = "0" Then
                Return "không đồng"
            End If
            Dim Group_Str(0 To 6) As String
            'Chuổi kết quả
            Dim str_rs As String = ""
            'Chuổi tạm
            Dim str_tmp As String = ""
            'Vị trí đơn vị hiện tại
            Dim intDonVi As Integer = 0
            Dim group_number As Integer = 3
            Dim end_for As Integer = 0
            If str.Length Mod 3 = 0 Then 'Chia hết cho 3
                end_for = str.Length / 3
            Else
                end_for = str.Length \ 3 + 1
            End If
            For j = 1 To end_for

                'Lọc lấy nhóm 3 chữ số
                If str.Length >= 3 * j Then 'Dài 3 chữ số và không phải nhóm cuối cùng
                    group_number = 3
                    Group_Str(intDonVi) = str.Substring(str.Length - 3 * j, group_number)
                Else 'Nhóm cuối cùng
                    group_number = 3 - (3 * j - str.Length)
                    Group_Str(intDonVi) = str.Substring(0, group_number)
                End If
                'xác định loại hàng nhóm 3 chữ số
                Select Case (intDonVi.ToString())
                    Case "0" : str_tmp += " đồng"
                    Case "1" : str_tmp += " ngàn"
                    Case "2" : str_tmp += " triệu"
                    Case "3" : str_tmp += " tỷ"
                    Case "4" : str_tmp += " ngàn tỷ"
                    Case "5" : str_tmp += " triệu tỷ"
                    Case "6" : str_tmp += " tỷ tỷ"
                End Select
                'Chạy từng ký tự trong nhóm 3 chữ số
                For i = (group_number - 1) To 0 Step -1
                    'Xác định đơn vị trong nhóm 3 chữ số
                    If group_number = 3 Then
                        Select Case (i.ToString())
                            Case "0" : str_tmp = " trăm" + str_tmp
                            Case "1"
                                If Group_Str(intDonVi)(i).ToString() = "0" Then
                                    str_tmp = " lẻ" + str_tmp
                                Else
                                    str_tmp = " mươi" + str_tmp
                                End If
                        End Select
                    Else
                        If group_number = 2 Then
                            If i = 0 And Group_Str(intDonVi)(i).ToString() <> "1" Then
                                str_tmp = " mươi" + str_tmp
                            End If
                            'Select Case (i.ToString())
                            '    Case "0"
                            'End Select
                        End If
                    End If
                    Select Case (Group_Str(intDonVi)(i).ToString())
                        Case "1"
                            If group_number = 3 And i = 0 Then
                                str_tmp = " một" + str_tmp
                            ElseIf group_number = 2 And i = 0 Then
                                str_tmp = " mười" + str_tmp
                            Else
                                str_tmp = " mốt" + str_tmp
                            End If

                        Case "2" : str_tmp = " hai" + str_tmp
                        Case "3" : str_tmp = " ba" + str_tmp
                        Case "4" : str_tmp = " bốn" + str_tmp
                        Case "5" : str_tmp = " năm" + str_tmp
                        Case "6" : str_tmp = " sáu" + str_tmp
                        Case "7" : str_tmp = " bảy" + str_tmp
                        Case "8" : str_tmp = " tám" + str_tmp
                        Case "9" : str_tmp = " chín" + str_tmp
                    End Select
                Next

                'Lưu lại
                Group_Str(intDonVi) = str_tmp
                str_tmp = ""
                intDonVi = intDonVi + 1
            Next
            'Đảo ngược vị trí
            For i = (intDonVi - 1) To 0 Step -1
                str_rs += Group_Str(i)
            Next
            Return (str_rs)
        Catch ex As Exception

        End Try
        'Hoàn thành
        Return ""
    End Function

    Public Shared Function AddTabSpace(ByVal Num As Integer) As String
        If Num <= 0 Then Return ChrW(9)
        Dim strTab As String = ""
        For i As Integer = 1 To Num
            strTab &= ChrW(9)
        Next
        Return strTab
    End Function
#Region "Ham load du lieu vao dropdownlist"
    ''' <summary>
    ''' Date: 13/12/2010
    ''' Author: TinhNT
    ''' </summary>
    ''' <param name="drp">drp is dropdownlist/list</param>
    ''' <remarks></remarks>
    ''' 
    Public Shared Sub Load_Data_To_DropDownlist_Listbox(ByVal drp As Object, ByVal dt As DataTable, ByVal strKeyName As String, ByVal KeyField As String, Optional ByVal strSelect As String = "")
        drp.Items.Clear()
        If Not String.IsNullOrEmpty(strSelect) Then
            drp.Items.Add(New ListItem(strSelect, "0"))
            drp.SelectedIndex = 0
        End If
        If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
            For Each itm As DataRow In dt.Rows
                drp.Items.Add(New ListItem(itm(strKeyName), itm(KeyField)))
            Next
        End If
        dt = Nothing
    End Sub
    'ThongDN viet ngay: 08122010
    Public Shared Sub FillCombo(ByVal ComboboxName As DropDownList, ByVal dt As DataTable, ByVal DataValueField As String, ByVal DataTextField As String, ByVal DataTextEmpty As String)
        Dim Item As DataRow
        ComboboxName.Items.Clear()
        ComboboxName.Items.Add(New ListItem(DataTextEmpty, "0"))
        For Each Item In dt.Rows
            ComboboxName.Items.Add(New ListItem(System.Convert.ToString(Item(DataTextField)), System.Convert.ToString(Item(DataValueField))))
        Next
    End Sub

    Public Shared Sub FillCombo(ByVal ComboboxName As DropDownList, ByVal dt As DataTable, ByVal DataValueField As String, ByVal DataTextField As String)
        If (dt Is Nothing) Then
            Return
        End If
        Try
            ComboboxName.DataSource = Nothing
            ComboboxName.Items.Clear()
            If (DataValueField = "") Then
                If (dt.Columns.Count > 1) Then
                    ComboboxName.DataValueField = dt.Columns(0).ColumnName.ToString()
                    ComboboxName.DataTextField = dt.Columns(1).ColumnName.ToString()
                Else
                    ComboboxName.DataValueField = dt.Columns(0).ColumnName.ToString()
                    ComboboxName.DataTextField = dt.Columns(0).ColumnName.ToString()
                End If
            Else
                ComboboxName.DataValueField = DataValueField
                ComboboxName.DataTextField = DataTextField
            End If
            ComboboxName.DataSource = dt
            ComboboxName.SelectedIndex = -1
            ComboboxName.DataBind()
        Catch ex As Exception

        End Try
    End Sub
#End Region
    '05/03/2011
    'duynp
    'Sendmail_SMTP(ADMIN_EMAIL_ADDRESS(Session("PARAMETER")), Email(0), SMTP_SERVER(Session("PARAMETER")), Email(0), "", "title mail", strBody, ADMIN_EMAIL_PASSWORD(Session("PARAMETER")), SMTP_PORT(Session("PARAMETER")), SMTP_SSL(Session("PARAMETER")))
    Public Shared Function Sendmail_SMTP(ByVal MFrom As String, ByVal YourName As String, ByVal SMTP As String, ByVal MailTo As String, ByVal cc As String, ByVal Subject As String, ByVal Body As String, ByVal Password As String, ByVal Port As String, ByVal SSL As Boolean) As Boolean

        ' hien tai dang su dung port default = 25, SSL = ???
        Dim success As Boolean = True
        Try
            Dim smtpsv As New Rebex.Net.Smtp
            smtpsv.Connect(SMTP)
            smtpsv.Login(MFrom, Password)
            Dim message As Rebex.Mail.MailMessage = New Rebex.Mail.MailMessage()
            message.Subject = Subject
            message.BodyHtml = Body
            message.From = New MailAddressCollection(MFrom)
            message.To = New MailAddressCollection(MailTo)
            'send mail 
            smtpsv.Send(message)
            smtpsv.Disconnect()
        Catch ex As Exception
            log.Error("Error error " & AddTabSpace(1) & HttpContext.Current.Session("Username") & AddTabSpace(1) & "IP:" & GetIPAddress(), ex)
            Return False
        End Try
        Return success
    End Function

    Public Shared Function makeURLFriendly(ByVal str As String) As String
        ' cộng hòa --> cong-hoa 
        str = str.ToLower().Trim()
        'remove special char
        Dim FindText As String = "ä|à|á|ạ|ả|ã|â|ầ|ấ|ậ|ẩ|ẫ|ă|ằ|ắ|ặ|ẳ|ẵ|ç|è|é|ẹ|ẻ|ẽ|ê|ề|ế|ệ|ể|ễ|ì|í|î|ị|ỉ|ĩ|ö|ò|ó|ọ|ỏ|õ|ô|ồ|ố|ộ|ổ|ỗ|ơ|ờ|ớ|ợ|ở|ỡ|ü|ù|ú|ụ|ủ|ũ|ư|ừ|ứ|ự|ử|ữ|ỳ|ý|ỵ|ỷ|ỹ|đ"
        Dim Replace1 As String = "a|a|a|a|a|a|a|a|a|a|a|a|a|a|a|a|a|a|c|e|e|e|e|e|e|e|e|e|e|e|i|i|i|i|i|i|o|o|o|o|o|o|o|o|o|o|o|o|o|o|o|o|o|o|u|u|u|u|u|u|u|u|u|u|u|u|y|y|y|y|y|d"
        For i As Integer = 0 To str.Length - 1
            Dim Found As Integer = FindText.IndexOf(str(i))
            If Found <> -1 Then
                str = str.Replace(str(i), Replace1(Found))
            End If
        Next
        'Dim specialChar As String = "[^a-zA-Z0-9]"
        'str = Regex.Replace(str, specialChar, " ").ToString()
        ''finalize
        'str = str.Replace("----", "-")
        'str = str.Replace("---", "-")
        'str = str.Replace("--", "-")
        Return str.ToUpper
    End Function

    Public Shared Function EncodePass(ByVal strPAss As String) As String
        Dim strCode As String
        Dim md5Hasher As New MD5CryptoServiceProvider
        Dim hashedDataBytes As Byte()
        Dim encoder As New UTF8Encoding
        hashedDataBytes = md5Hasher.ComputeHash(encoder.GetBytes(strPAss))
        strCode = Convert.ToBase64String(hashedDataBytes)
        Return strCode
    End Function

    ''' <summary>
    ''' substring long description to short description
    ''' </summary>
    ''' <param name="desc">your description</param>
    ''' <param name="intWords">number word want to keep alive</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function hintDesc(ByVal desc As String, ByVal intWords As Integer) As String
        Dim toLong = desc.Length > intWords
        Dim s_ As String = ""
        Try
            s_ = IIf(toLong, desc.Substring(0, intWords - 1), desc)
            s_ = IIf(toLong, s_.Substring(0, s_.LastIndexOf(" "c)), s_)
        Catch ex As Exception
            If toLong Then
                s_ = desc.Substring(0, intWords - 1)
            Else
                s_ = desc
            End If
            Return IIf(toLong, s_ & " ...", s_)
        End Try
        Return IIf(toLong, s_ & " ...", s_)
    End Function
    'Add keyword
    Public Shared Sub AddKeyWordJavascript(ByVal strKeyWord As String)

    End Sub
    'Remove keyword
    Public Shared Sub RemoveKeyWordJavascript(ByVal strKeyWord As String)

    End Sub

    ''/************************************
    ''* Dec 14 2012
    ''* TuanCd thêm mới để thay thế ký tự ','
    ''*
    ''*/************************************

    ''' <summary>
    ''' Trả về chuỗi đã được thay thế bởi ký tự
    ''' </summary>
    ''' <param name="_strText">Chuỗi gốc </param>
    ''' <param name="spec">ký tự cần thay thế</param>
    ''' <param name="repl">ký tự được thay thế</param>
    ''' <returns>một số Integer</returns>
    ''' <remarks></remarks>
    Public Shared Function GetNumberByFormat(ByVal _strText As Object, Optional ByVal spec As String = ",", Optional ByVal repl As String = "")
        Try
            Return CInt(Strings.Replace(_strText.ToString.Trim(), spec, repl))
        Catch ex As Exception
            Return 0
        End Try
    End Function

    ''' <summary>
    ''' Đọc chuổi văn bản thành một List dựa vào ký tự xuống dòng
    ''' </summary>
    ''' <param name="source">Chuỗi nguồn</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function ReadAllLines(ByVal source As String) As List(Of String)
        If source Is Nothing Then
            Return Nothing
        End If

        Dim lines As New List(Of String)
        Using reader As New StringReader(source)
            While reader.Peek() <> -1
                lines.Add(reader.ReadLine())
            End While
        End Using

        Return lines
    End Function
    Public Shared Sub KillSpecificExcelFileProcess(ByVal _app As Excel._Application)
        Dim killexcel As New LibKillProcessExcel()
        Dim sVer As [String] = _app.Version
        Dim iHandle As IntPtr = IntPtr.Zero
        If Single.Parse(sVer) >= 10 Then
            iHandle = New IntPtr(_app.Parent.Hwnd)
        End If
        Dim caption As String = _app.Caption
        killexcel.EnsureProcessKilled(iHandle, caption)

    End Sub
End Class

