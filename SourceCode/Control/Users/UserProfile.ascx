<%@ Control Language="VB" AutoEventWireup="false" CodeFile="UserProfile.ascx.vb"
    Inherits="Control_Users_UserProfile" %>
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <div id="Div1" class="BoxField" runat="server">
            <div class="HeadTitle">
                <h3>
                    THÔNG TIN CÁ NHÂN
                </h3>
            </div>
            <div style="width: 200px; height: 200px; float: left">
                <asp:Label ID="lblImage" CssClass="TextContent" runat="server" Text="lblImage" />
            </div>
            <div style="width: 600px; float: left">
                <div class="DivRow" style="width: 600px;padding-left:10px;float:left">
                    <div class="DivLabel" style="float: left; width: 150px">
                        <asp:Label ID="lblUsername_detail" CssClass="DivLabel" runat="server" Text="Tên đăng nhập" />:</div>
                    <div class="TextContent" style="float: left; width: 450px">
                        &nbsp;
                        <asp:Label ID="lblUsername" CssClass="TextContent" runat="server" Text="lblUsername" /></div>
                </div>
                <div class="DivRow" style="width: 600px;padding-left:10px;float:left">
                    <div class="DivLabel" style="float: left; width: 150px">
                        <asp:Label ID="lblFullname_detail" CssClass="DivLabel" runat="server" Text="Họ tên" />:</div>
                    <div class="TextContent" style="float: left; width: 450px">
                        &nbsp;
                        <asp:Label ID="lblFullname" CssClass="TextContent" runat="server" Text="lblFullname" /></div>
                </div>
                <div class="DivRow" style="width: 600px;padding-left:10px;float:left">
                    <div class="DivLabel" style="float: left; width: 150px">
                        <asp:Label ID="lblEmail_detail" CssClass="DivLabel" runat="server" Text="Email" />:</div>
                    <div class="TextContent" style="float: left; width: 450px">
                        &nbsp;
                        <asp:Label ID="lblEmail" CssClass="TextContent" runat="server" Text="lblEmail" /></div>
                </div>
                <div class="DivRow" style="width: 600px;padding-left:10px;float:left">
                    <div class="DivLabel" style="float: left; width: 150px">
                        <asp:Label ID="lblDateofbirth_detail" CssClass="DivLabel" runat="server" Text="Ngày sinh" />:</div>
                    <div class="TextContent" style="float: left; width: 450px">
                        &nbsp;
                        <asp:Label ID="lblDateofbirth" CssClass="TextContent" runat="server" Text="lblDateofbirth" /></div>
                </div>
                <div class="DivRow" style="width: 600px;padding-left:10px;float:left">
                    <div class="DivLabel" style="float: left; width: 150px">
                        <asp:Label ID="lblSex_detail" CssClass="DivLabel" runat="server" Text="Giới tính" />:</div>
                    <div class="TextContent" style="float: left; width: 450px">
                        &nbsp;
                        <asp:Label ID="lblSex" CssClass="TextContent" runat="server" Text="lblSex" /></div>
                </div>
                <div class="DivRow" style="width: 600px;padding-left:10px;float:left">
                    <div class="DivLabel" style="float: left; width: 150px">
                        <asp:Label ID="lblAddress_detail" CssClass="DivLabel" runat="server" Text="Địa chỉ" />:</div>
                    <div class="TextContent" style="float: left; width: 450px">
                        &nbsp;
                        <asp:Label ID="lblAddress" CssClass="TextContent" runat="server" Text="lblAddress" /></div>
                </div>
            </div>
            <div style="width: 800px; float: left">
            <div class="DivRow" style="width: 800px; float: left">
                <div class="DivLabel" style="float: left; width: 200px">
                    <asp:Label ID="lblPhone_detail" CssClass="DivLabel" runat="server" Text="Điện thoại bàn" />:</div>
                <div class="TextContent" style="float: left; width: 600px">
                    &nbsp;
                    <asp:Label ID="lblPhone" CssClass="TextContent" runat="server" Text="lblPhone" /></div>
            </div>
            <div class="DivRow" style="width: 800px; float: left">
                <div class="DivLabel" style="float: left; width: 200px">
                    <asp:Label ID="lblMobile_detail" CssClass="DivLabel" runat="server" Text="Di động" />:</div>
                <div class="TextContent" style="float: left; width: 600px">
                    &nbsp;
                    <asp:Label ID="lblMobile" CssClass="TextContent" runat="server" Text="lblMobile" /></div>
            </div>
            <div class="DivRow" style="width: 800px; float: left">
                <div class="DivLabel" style="float: left; width: 200px">
                    <asp:Label ID="lblWeight_detail" CssClass="DivLabel" runat="server" Text="Cân nặng" />:</div>
                <div class="TextContent" style="float: left; width: 600px">
                    &nbsp;
                    <asp:Label ID="lblWeight" CssClass="TextContent" runat="server" Text="lblWeight" /></div>
            </div>
            <div class="DivRow" style="width: 800px; float: left">
                <div class="DivLabel" style="float: left; width: 200px">
                    <asp:Label ID="lblHeight_detail" CssClass="DivLabel" runat="server" Text="Chiều cao" />:</div>
                <div class="TextContent" style="float: left; width: 600px">
                    &nbsp;
                    <asp:Label ID="lblHeight" CssClass="TextContent" runat="server" Text="lblHeight" /></div>
            </div>
            <div class="DivRow" style="width: 800px; float: left">
                <div class="DivLabel" style="float: left; width: 200px">
                    <asp:Label ID="lblBriefintroduction_detail" CssClass="DivLabel" runat="server" Text="Giới thiệu bản thân" />:</div>
                <div class="TextContent" style="float: left; width: 600px">
                    &nbsp;
                    <asp:Label ID="lblBriefintroduction" CssClass="TextContent" runat="server" Text="lblBriefintroduction" /></div>
            </div>
            <div class="DivRow" style="width: 800px; float: left">
                <div class="DivLabel" style="float: left; width: 200px">
                    <asp:Label ID="lblAcceptednewsletteremail_detail" CssClass="DivLabel" runat="server"
                        Text="Nhận tin mới qua mail" />:</div>
                <div class="TextContent" style="float: left; width: 600px">
                    &nbsp;
                    <asp:CheckBox ID="chkAcceptednewsletteremail" runat="server" Enabled="false" /></div>
            </div>
            <div class="DivRow" style="width: 800px; float: left">
                <div class="DivLabel" style="float: left; width: 200px">
                    <asp:Label ID="lblAcceptedweeklyhotdealemail_detail" CssClass="DivLabel" runat="server"
                        Text="Nhận Hot Deal qua mail" />:</div>
                <div class="TextContent" style="float: left; width: 600px">
                    &nbsp;
                    <asp:CheckBox ID="chkAcceptedweeklyhotdealemail" runat="server" Enabled="false" /></div>
            </div>
            <div class="DivRow" style="width: 800px; float: left">
                <div class="DivLabel" style="float: left; width: 200px">
                    <asp:Label ID="lblLikeshopping_detail" CssClass="DivLabel" runat="server" Text="Mua sắm" />:</div>
                <div class="TextContent" style="float: left; width: 600px">
                    &nbsp;
                    <asp:CheckBox ID="chkLikeShopping" runat="server" Enabled="false" /></div>
            </div>
            <div class="DivRow" style="width: 800px; float: left">
                <div class="DivLabel" style="float: left; width: 200px">
                    <asp:Label ID="lblLiketravel_detail" CssClass="DivLabel" runat="server" Text="Du lịch" />:</div>
                <div class="TextContent" style="float: left; width: 600px">
                    &nbsp;
                    <asp:CheckBox ID="chkLikeTravel" runat="server" Enabled="false" /></div>
            </div>
            <div class="DivRow" style="width: 800px; float: left">
                <div class="DivLabel" style="float: left; width: 200px">
                    <asp:Label ID="lblLikegossip_detail" CssClass="DivLabel" runat="server" Text="Tán gẫu" />:</div>
                <div class="TextContent" style="float: left; width: 600px">
                    &nbsp;
                    <asp:CheckBox ID="chkLikeGossip" runat="server" Enabled="false" /></div>
            </div>
            <div class="DivRow" style="width: 800px; float: left">
                <div class="DivLabel" style="float: left; width: 200px">
                    <asp:Label ID="lblLikecooking_detail" CssClass="DivLabel" runat="server" Text="Nấu ăn" />:</div>
                <div class="TextContent" style="float: left; width: 600px">
                    &nbsp;
                    <asp:CheckBox ID="chkLikeCooking" runat="server" Enabled="false" /></div>
            </div>
            <div class="DivRow" style="width: 800px; float: left">
                <div class="DivLabel" style="float: left; width: 200px">
                    <asp:Label ID="lblLikeflowerdecorate_detail" CssClass="DivLabel" runat="server" Text="Cắm hoa" />:</div>
                <div class="TextContent" style="float: left; width: 600px">
                    &nbsp;
                    <asp:CheckBox ID="chkLikeflowerdecorate" runat="server" Enabled="false" /></div>
            </div>
            <div class="DivRow" style="width: 800px; float: left">
                <div class="DivLabel" style="float: left; width: 200px">
                    <asp:Label ID="lblLikemovie_detail" CssClass="DivLabel" runat="server" Text="Xem phim" />:</div>
                <div class="TextContent" style="float: left; width: 600px">
                    &nbsp;
                    <asp:CheckBox ID="chkLikeMovie" runat="server" Enabled="false" /></div>
            </div>
            <div class="DivRow" style="width: 800px; float: left">
                <div class="DivLabel" style="float: left; width: 200px">
                    <asp:Label ID="lblLikessm_detail" CssClass="DivLabel" runat="server" Text="Likessm" />:</div>
                <div class="TextContent" style="float: left; width: 600px">
                    &nbsp;
                    <asp:CheckBox ID="chkLikessm" runat="server" Enabled="false" /></div>
            </div>
            <div class="DivRow" style="width: 800px; float: left">
                <div class="DivLabel" style="float: left; width: 200px">
                    <asp:Label ID="lblLikechildren_detail" CssClass="DivLabel" runat="server" Text="Thích trẻ em" />:</div>
                <div class="TextContent" style="float: left; width: 600px">
                    &nbsp;
                    <asp:CheckBox ID="chkLikeChildren" runat="server" Enabled="false" /></div>
            </div>
            <div class="DivRow" style="width: 800px; float: left">
                <div class="DivLabel" style="float: left; width: 200px">
                    <asp:Label ID="lblYm_detail" CssClass="DivLabel" runat="server" Text="Yahoo" />:</div>
                <div class="TextContent" style="float: left; width: 600px">
                    &nbsp;
                    <asp:Label ID="lblYm" CssClass="TextContent" runat="server" Text="lblYm" /></div>
            </div>
            <div class="DivRow" style="width: 800px; float: left">
                <div class="DivLabel" style="float: left; width: 200px">
                    <asp:Label ID="lblSkype_detail" CssClass="DivLabel" runat="server" Text="Skype" />:</div>
                <div class="TextContent" style="float: left; width: 600px">
                    &nbsp;
                    <asp:Label ID="lblSkype" CssClass="TextContent" runat="server" Text="lblSkype" /></div>
            </div>
            <div class="DivRow" style="width: 800px; float: left">
                <div class="DivLabel" style="float: left; width: 200px">
                    <asp:Label ID="lblFacebook_detail" CssClass="DivLabel" runat="server" Text="Facebook" />:</div>
                <div class="TextContent" style="float: left; width: 600px">
                    &nbsp;
                    <asp:Label ID="lblFacebook" CssClass="TextContent" runat="server" Text="lblFacebook" /></div>
            </div>
            <div class="DivRow" style="width: 800px; float: left">
                <div class="DivLabel" style="float: left; width: 200px">
                    <asp:Label ID="lblMark_detail" CssClass="DivLabel" runat="server" Text="Điểm cá nhân" />:</div>
                <div class="TextContent" style="float: left; width: 600px">
                    &nbsp;
                    <asp:Label ID="lblMark" CssClass="TextContent" runat="server" Text="lblMark" /></div>
            </div>
            <div class="DivRow" style="width: 800px; float: left">
                <div class="DivLabel" style="float: left; width: 200px">
                    <asp:Label ID="lblMarriedstatus_detail" CssClass="DivLabel" runat="server" Text="Tình trạng hôn nhân" />:</div>
                <div class="TextContent" style="float: left; width: 600px">
                    &nbsp;
                    <asp:Label ID="lblMarriedstatus" CssClass="TextContent" runat="server" Text="lblMarriedstatus" /></div>
            </div>
            </div>
        </div>
        <asp:HiddenField ID="hidID" Value="0" runat="server" />
    </ContentTemplate>
</asp:UpdatePanel>
