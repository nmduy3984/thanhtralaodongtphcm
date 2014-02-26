<%@ Control Language="VB" AutoEventWireup="false" CodeFile="sysFunctionMenu.ascx.vb" Inherits="Control_sysFunction_sysFunctionMenu" %>

<div class="menu_pages">
    <div class="tabs_menu"> 
        <ul>
            <asp:Repeater ID="rptMenu" runat="server"> 
                <ItemTemplate> 
                    <li>
                        <asp:HyperLink ID="hplTitle" runat="server">Title</asp:HyperLink>
                   </li>
                </ItemTemplate>
            </asp:Repeater>


            <li class="MenuMore">
                <div class="viewMore">
    	                &nbsp;
                        <div class="moreLink">
        	                <div class="moreLink_top">
                            </div>
                                
                                <asp:Repeater ID="rptMenuMore" runat="server"> 
                                    <ItemTemplate> 
                                        
                                            <asp:HyperLink ID="hplTitle" runat="server">Title</asp:HyperLink>
                                       
                                    </ItemTemplate>
                                </asp:Repeater>

                            <div class="moreLink_bottom">
                            </div>
                        </div>
                </div>
            </li>


        </ul>
    </div>
</div>

