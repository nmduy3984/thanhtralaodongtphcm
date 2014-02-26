<%@ Page Title="" Language="VB" MasterPageFile="~/Site.master" AutoEventWireup="false"
    CodeFile="Help.aspx.vb" Inherits="Page_Help_Help" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="uplYtgchanthuong" runat="server">
        <ContentTemplate>
            <div class="BoxField">
                <script type="text/javascript" language="JavaScript">
                    function autoResize(id) {
                        var newheight;
                        var newwidth;

                        if (document.getElementById) {
                            newheight = document.getElementById(id).contentWindow.document.body.scrollHeight;
                            newwidth = document.getElementById(id).contentWindow.document.body.scrollWidth;
                        }

                        document.getElementById(id).height = (newheight) + "px";
                        document.getElementById(id).width = (newwidth) + "px";
                    }
                </script>
                <iframe src="Help.htm" id="iframe1"  width="100%" height="100%" style="border: 0px" onload="autoResize('iframe1');">
                </iframe>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
