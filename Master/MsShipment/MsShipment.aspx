<%@ Page Language="VB" AutoEventWireup="false" CodeFile="MsShipment.aspx.vb" Inherits="MsShipment_MsShipment" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Shipment File</title>
    <script src="../../Function/OpenDlg.js" type="text/javascript"></script>
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
 <link href="https://fonts.googleapis.com/css2?family=Muli:wght@300;400;600&display=swap" rel="stylesheet">
</head>
<body>
  <form id="form1" runat="server">
      <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
     <div class="Content">
     <div class="H1">Shipment File</div>
     <hr style="color:Blue" />
      <table>
        <tr>
            <td style="width:100px;text-align:right" >Quick Search :</td>
            <td><asp:TextBox runat="server" ID ="tbFilter" CssClass="TextBox"/> 
                <asp:DropDownList runat="server" ID="ddlField" CssClass="DropDownList" >
                  <asp:ListItem Selected="true" Text="Shipment Code" Value="ShipmentCode"></asp:ListItem>
                  <asp:ListItem Text="Shipment Name" Value="ShipmentName"></asp:ListItem>        
                </asp:DropDownList>     
                <asp:Button class="bitbtn btnsearch" runat="server" ID="btnSearch" Text="Search" />                
                <asp:Button class="btngo" runat="server" ID="btnExpand" Text="..."/>                
                <asp:Button class="bitbtn btnprint" runat="server" ID="btnPrint" Text="Print"/>               
                <asp:Button ID="btnShowPopup" runat="server" Style="display: none" />
                <cc1:ModalPopupExtender ID="ModalPopupExtender1" runat="server" 
                    PopupControlID="pnlFind" DropShadow="True" TargetControlID="btnShowPopup"                     
                    BackgroundCssClass="BackgroundStyle" />                           
               
            </td>
        </tr>
     </table>
     <asp:Panel runat="server" ID="pnlSearch" Visible="false">
     <table>   
        <tr>
            <td style="width:100px;text-align:right"><asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlNotasi" >
                    <asp:ListItem Selected="true" Text="OR" Value="OR"></asp:ListItem>
                    <asp:ListItem Text="AND" Value="AND"></asp:ListItem>         
                </asp:DropDownList>
            </td>
            <td><asp:TextBox runat="server" ID ="tbfilter2" CssClass="TextBox"/> 
                  <asp:DropDownList runat="server" ID="ddlField2" CssClass="DropDownList" >
                    <asp:ListItem Selected="true" Text="Shipment Code" Value="ShipmentCode"></asp:ListItem>
                  <asp:ListItem Text="Shipment Name" Value="ShipmentName"></asp:ListItem>        
                  </asp:DropDownList>
                
            </td>
        </tr>
     </table>
     </asp:Panel>
      <br />
      <asp:GridView id="DataGrid" runat="server"  
            ShowFooter="True" AllowSorting="True" 
            AutoGenerateColumns="False" AllowPaging="True" CssClass="Grid">
						<HeaderStyle CssClass="GridHeader"></HeaderStyle>
						<RowStyle CssClass="GridItem" Wrap="false"/>
						<AlternatingRowStyle CssClass="GridAltItem"/>
						<FooterStyle CssClass="GridFooter" />
						<PagerStyle CssClass="GridPager" HorizontalAlign="Center" />					  
				<EmptyDataTemplate>
				    
				</EmptyDataTemplate>	  
				      <Columns>				      
							<asp:TemplateField HeaderText="Shipment Code" HeaderStyle-Width="100" SortExpression="ShipmentCode">
								<Itemtemplate>
									<asp:Label Runat="server" ID="ShipmentCode" text='<%# DataBinder.Eval(Container.DataItem, "ShipmentCode") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:Label Runat="server" ID="ShipmentCodeEdit" Text='<%# DataBinder.Eval(Container.DataItem, "ShipmentCode") %>'>
									</asp:Label>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="ShipmentCodeAdd" CssClass="TextBox" MaxLength="5" Runat="Server" Width="100%"/>
									<cc1:TextBoxWatermarkExtender ID="ShipmentCodeAdd_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="ShipmentCodeAdd" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>
								</FooterTemplate>								
							</asp:TemplateField>							
							
							<asp:TemplateField HeaderText="Shipment Name" HeaderStyle-Width="368" SortExpression="ShipmentName">
								<Itemtemplate>
									<asp:Label Runat="server" ID="ShipmentName" text='<%# DataBinder.Eval(Container.DataItem, "ShipmentName") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:TextBox Runat="server" ID="ShipmentNameEdit" MaxLength="60" Width="100%" CssClass="TextBox" Text='<%# DataBinder.Eval(Container.DataItem, "ShipmentName") %>'>
									</asp:TextBox>
									<cc1:TextBoxWatermarkExtender ID="ShipmentNameEdit_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="ShipmentNameEdit" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>									
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="ShipmentNameAdd" CssClass="TextBox" MaxLength="60" Runat="Server" Width="100%"/>
									<cc1:TextBoxWatermarkExtender ID="ShipmentNameAdd_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="ShipmentNameAdd" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>
								</FooterTemplate>
							</asp:TemplateField>		
							
							<asp:TemplateField HeaderText="Action" headerstyle-width="126" >
								<ItemTemplate>
								    <asp:Button class="bitbtn btnedit" runat="server" ID="btnEdit" Text="Edit" CommandName="Edit" />																						 											
									
									<asp:Button class="bitbtn btndelete" runat="server" ID="btnDelete" Text="Delete" CommandName="Delete"  OnClientClick="return confirm('Sure to delete this data?');" />																						 																		
								
									
								</ItemTemplate>
								<EditItemTemplate>
									<asp:Button class="bitbtn btnsave" runat="server" ID="btnUpdate" Text="Save" CommandName="Update" />																						 																		
									
									
									<asp:Button class="bitbtn btncancel" runat="server" ID="btnCancel" Text="Cancel" CommandName="Cancel" />																						 																		
									
								</EditItemTemplate>
								<FooterTemplate>
								   <asp:Button class="bitbtn btnadd" runat="server" ID="btnAdd" Text="Add" CommandName="Insert" />																						 																		
									
								</FooterTemplate>
							</asp:TemplateField>
							
    					</Columns>
        </asp:GridView>
        
     <asp:Label ID="lstatus" ForeColor="red" runat="server"></asp:Label>    
    </div>
    </form>
</body>

</html>
