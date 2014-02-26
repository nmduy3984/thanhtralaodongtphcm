Imports System.Data
Imports ThanhTraLaoDongModel
Imports Cls_Common
Partial Class Control_Users_UserProfile
    Inherits System.Web.UI.UserControl
#Region "Sub and Function "
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            If Not Request.QueryString("Username").ToString.Equals("0") Then
                hidID.Value = Request.QueryString("Username")
                ShowData()
            End If
        End If
    End Sub
    Protected Sub ShowData()
        Using data As New ThanhTraLaoDongEntities
            Dim p = (From q In data.UsersProfiles Where q.User.UserName = hidID.Value Select New With {q.AcceptedNewsLetterEmail, q.AcceptedWeeklyHotDealEmail, q.DateOfBirth, q.Sex, q.Address, q.Phone _
                                                                                                , q.Mobile, q.Avarta, q.Weight, q.Height, q.BriefIntroduction, q.LikeShopping, q.LikeTravel, q.LikeGossip _
                                                                                               , q.LikeCooking, q.LikeFlowerDecorate, q.LikeMovie, q.LikeSSM, q.LikeChildren, q.YM, q.Skype, q.Facebook, q.Mark, q.MarriedStatus, q.User.Email, q.User.UserName, q.User.FullName}).SingleOrDefault
            If Not p Is Nothing Then
                'lblUserid.Text = IIf(IsNothing(p.UserId) = True, "", p.UserId)
                lblUsername.Text = IIf(IsNothing(p.UserName) = True, "", p.UserName)
                lblFullname.Text = IIf(IsNothing(p.FullName) = True, "", p.FullName)
                lblEmail.Text = IIf(IsNothing(p.Email) = True, "", p.Email)
                chkAcceptednewsletteremail.Checked = IIf(IsNothing(p.AcceptedNewsLetterEmail) = True, "", p.AcceptedNewsLetterEmail)
                chkAcceptedweeklyhotdealemail.Checked = IIf(IsNothing(p.AcceptedWeeklyHotDealEmail) = True, "", p.AcceptedWeeklyHotDealEmail)
                If Not p.DateOfBirth Is Nothing Then
                    lblDateofbirth.Text = Convert.ToDateTime(IIf(IsNothing(p.DateOfBirth) = True, "", p.DateOfBirth)).ToString("dd/MM/yyyy")
                Else
                    lblDateofbirth.Text = ""
                End If
                Dim Sex As Integer = IIf(IsNothing(p.Sex) = True, 0, p.Sex)
                If Sex = 1 Then
                    lblSex.Text = "Nam"
                ElseIf Sex = 2 Then
                    lblSex.Text = "Nữ"
                Else
                    lblSex.Text = ""
                End If
                lblAddress.Text = IIf(IsNothing(p.Address) = True, "", p.Address)
                lblPhone.Text = IIf(IsNothing(p.Phone) = True, "", p.Phone)
                lblMobile.Text = IIf(IsNothing(p.Mobile) = True, "", p.Mobile)
                'lblAvarta.Text = IIf(IsNothing(p.Avarta) = True, "", p.Avarta)
                lblWeight.Text = IIf(IsNothing(p.Weight) = True, "", p.Weight)
                lblHeight.Text = IIf(IsNothing(p.Height) = True, "", p.Height)
                lblBriefintroduction.Text = IIf(IsNothing(p.BriefIntroduction) = True, "", p.BriefIntroduction)
                chkLikeShopping.Checked = IIf(IsNothing(p.LikeShopping) = True, "", p.LikeShopping)
                chkLikeTravel.Checked = IIf(IsNothing(p.LikeTravel) = True, "", p.LikeTravel)
                chkLikeGossip.Checked = IIf(IsNothing(p.LikeGossip) = True, "", p.LikeGossip)
                chkLikeCooking.Checked = IIf(IsNothing(p.LikeCooking) = True, "", p.LikeCooking)
                chkLikeflowerdecorate.Checked = IIf(IsNothing(p.LikeFlowerDecorate) = True, "", p.LikeFlowerDecorate)
                chkLikeMovie.Checked = IIf(IsNothing(p.LikeMovie) = True, "", p.LikeMovie)
                chkLikessm.Checked = IIf(IsNothing(p.LikeSSM) = True, "", p.LikeSSM)
                chkLikeChildren.Checked = IIf(IsNothing(p.LikeChildren) = True, "", p.LikeChildren)
                lblYm.Text = IIf(IsNothing(p.YM) = True, "", p.YM)
                lblSkype.Text = IIf(IsNothing(p.Skype) = True, "", p.Skype)
                lblFacebook.Text = IIf(IsNothing(p.Facebook) = True, "", p.Facebook)
                lblMark.Text = IIf(IsNothing(p.Mark) = True, "", p.Mark)
                Dim Married As Integer = IIf(IsNothing(p.MarriedStatus) = True, -1, p.MarriedStatus)
                If Married = 1 Then
                    lblMarriedstatus.Text = "Đã kết hôn"
                ElseIf Married = 0 Then
                    lblMarriedstatus.Text = "Độc thân"
                Else
                    lblMarriedstatus.Text = ""
                End If
                Dim Avatar As String = IIf(IsNothing(p.Avarta) = True, "", p.Avarta)
                If Avatar <> "" Then
                    Dim strChuoiHTML As String = "<img  Width='200px' Height='200px' src='" & Avatar.Trim() & " '/> "
                    lblImage.Text = strChuoiHTML.Trim()
                Else
                    Dim strChuoiHTML As String = "<img  Width='200px' Height='200px' src='../../Uploads/UserProfile/no_avatar.jpg'/> "
                    lblImage.Text = strChuoiHTML.Trim()
                End If
            End If
        End Using
    End Sub
#End Region
End Class
