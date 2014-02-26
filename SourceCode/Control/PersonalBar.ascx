<%@ Control Language="VB" AutoEventWireup="false" CodeFile="PersonalBar.ascx.vb" Inherits="Control_PersonalBar" %>

<div class="userTop">

    <%--<a href="javascript: return false;" class="userID"><%= Session("UserName")%></a>--%>
    <asp:HyperLink id="hplChangePassword" runat="server" Text="Đổi mật khẩu" CssClass="changePass thickbox" ></asp:HyperLink>
     <asp:LinkButton id="lbtnExit" runat="server"  CssClass="logout"
            Text="Thoát" CausesValidation="False"></asp:LinkButton>
    </div>
   

