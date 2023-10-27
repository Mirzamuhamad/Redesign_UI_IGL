<%@ Control Language="VB" AutoEventWireup="false" CodeFile="Waiting.ascx.vb" Inherits="UserControl_WebUserControl" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<style>
.modalBackground
{
	background-color: Gray;
	filter: alpha(opacity=80);
	opacity: 0.8;
}

.modalPopup
{
	/*background-color: #FFFFFF;*/
	background-color: #FFFFFF;	
	border-width: 1px;
	-moz-border-radius: 5px;
	border-style: solid;
	border-color: Gray;	
	min-width: 300px;
	max-width: 400px;
	min-height:150px;
	max-height:250px;	
	top:10px;
	left:50px;
}

.topHandle
{
	/*background-color: #97bae6;	*/
	background-color: #0066FF;	
	
}

.table
{
	padding:0;
	margin:0;
}
</style>

<cc1:ModalPopupExtender ID="mpextwait" runat="server" BackgroundCssClass="modalBackground"
    TargetControlID="pnlPopupwait" PopupControlID="pnlPopupwait">
</cc1:ModalPopupExtender> 

<asp:Panel ID="pnlPopupwait" runat="server" CssClass="modalPopup" Style="display: none;">
<table width="100%" height="100%">
        <tr class="topHandle">
            <td colspan="2" align="left" runat="server">
                <asp:Label ID="lblMessage" runat="server" Text = "Please waiting..." Font-Bold="true" ForeColor="White"></asp:Label>
            </td>
        </tr>
        <tr>            
            <td valign="middle">                
                
                <asp:Image runat="server" ID="imageload" Height="50px" Width="50px" ImageAlign="Middle" ImageUrl="../Image/waitLoading.gif" >
                </asp:Image>
                &nbsp                
                <asp:Label runat="server" ID="lblText" Font-Bold="true" style="vertical-align: middle;color: red;" Font-Size="XX-Large" Text = "Loading...">
                </asp:Label>
            </td>
        </tr>        
    </table>
</asp:Panel>
