<%@ Control Language="VB" AutoEventWireup="false" CodeFile="MsgBox.ascx.vb" Inherits="UserControl_MsgBox" %>
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
	background-color: #ffffdd;
	border-width: 1px;
	-moz-border-radius: 5px;
	border-style: solid;
	border-color: Gray;	
	min-width: 300px;
	max-width:450px;
	min-height:100px;
	max-height:200px;	
	top:100px;
	left:150px;
}

.topHandle
{
	background-color: #97bae6;	
}

.table
{
	padding:0;
	margin:0;
}
</style>

<cc1:ModalPopupExtender ID="mpext" runat="server" BackgroundCssClass="modalBackground"
    TargetControlID="pnlPopup" PopupControlID="pnlPopup">
</cc1:ModalPopupExtender> 

<asp:Panel ID="pnlPopup" runat="server" CssClass="modalPopup" Style="display: none;"
    DefaultButton="btnYes">
    <table width="100%">
        <tr class="topHandle">
            <td colspan="2" align="left" runat="server" id="tdCaption">
                &nbsp; <asp:Label ID="lblCaption" runat="server" Text="test"></asp:Label>
            </td>
        </tr>
        <tr>
            <td style="width: 60px" valign="middle" align="center">
                <asp:Image ID="imgInfo" runat="server" ImageUrl="../Image/Msginfo.gif"/>
            </td>
            <td valign="middle" align="left">
                <asp:Label ID="lblMessage" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td colspan="2" align="right">
                <asp:Button ID="btnYes" autopostback="true" runat="server" Text="Yes" Width="60px" />
                <asp:Button ID="btnNo" autopostback="true" runat="server" Text="No" Width="60px" Visible="false" />
            </td>
        </tr>
    </table>
</asp:Panel>

<script type="text/javascript">
        function fnClickOK(sender, e)
        {
            __doPostBack(sender,e);
        }
</script>