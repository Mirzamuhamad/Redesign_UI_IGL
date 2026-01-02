<%@ Page Language="VB" AutoEventWireup="false" CodeFile="MsTerminateReason.aspx.vb" Inherits="MsTerminateReason_MsTerminateReason" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>>Terminate Reason File</title>
    <script src="../../Function/Function.JS" type="text/javascript"></script>
    <script src="../../Function/OpenDlg.js" type="text/javascript"></script>
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
 <link href="https://fonts.googleapis.com/css2?family=Muli:wght@300;400;600&display=swap" rel="stylesheet">
</head>
<body>
  <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
     <div class="Content">
     <div class="H1">Terminate Reason File</div>
     <hr style="color:Blue" />
      <table>
        <tr>
            <td style="width:100px;text-align:right" >Quick Search :</td>
            <td><asp:TextBox runat="server" ID ="tbFilter" CssClass="TextBox"/> 
                <asp:DropDownList runat="server" ID="ddlField" CssClass="DropDownList" >
                  <asp:ListItem Selected="true" Text="Reason Code" Value="PHKCode"></asp:ListItem>
                  <asp:ListItem Text="Reason Name" Value="PHKName"></asp:ListItem>        
                  <asp:ListItem Text="BlackList" Value="FgBlacklist"></asp:ListItem>        
                  <asp:ListItem Text="Severance Variable" Value="Variable1"></asp:ListItem>                          
                  <asp:ListItem Text="Service Variable" Value="Variable2"></asp:ListItem>                          
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
                  <asp:ListItem Selected="true" Text="Reason Code" Value="PHKCode"></asp:ListItem>
                  <asp:ListItem Text="Reason Name" Value="PHKName"></asp:ListItem>        
                  <asp:ListItem Text="BlackList" Value="FgBlacklist"></asp:ListItem>        
                  <asp:ListItem Text="Severance Variable" Value="Variable1"></asp:ListItem>                          
                  <asp:ListItem Text="Service Variable" Value="Variable2"></asp:ListItem>                                            </asp:DropDownList>
                
            </td>
        </tr>
     </table>
     </asp:Panel>
      <br />
      <div style="border:0px  solid; width:100%; height:100%; overflow:auto;">
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
							<asp:TemplateField HeaderText="Reason Code" HeaderStyle-Width="100" SortExpression="PHKCode">
								<Itemtemplate>
									<asp:Label Runat="server" ID="ReasonCode" text='<%# DataBinder.Eval(Container.DataItem, "PHKCode") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:Label Runat="server" ID="ReasonCodeEdit" Text='<%# DataBinder.Eval(Container.DataItem, "PHKCode") %>'>
									</asp:Label>								
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="ReasonCodeAdd" CssClass="TextBox" MaxLength="5" Runat="Server" Width="100%"/>
									<cc1:TextBoxWatermarkExtender ID="ReasonCodeAdd_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="ReasonCodeAdd" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked"   >
                                     </cc1:TextBoxWatermarkExtender>
								</FooterTemplate>								
							</asp:TemplateField>							
							
							<asp:TemplateField HeaderText="Reason Name" HeaderStyle-Width="368" SortExpression="PHKName">
								<Itemtemplate>
									<asp:Label Runat="server" ID="ReasonName" text='<%# DataBinder.Eval(Container.DataItem, "PHKName") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:TextBox Runat="server" ID="ReasonNameEdit" MaxLength="60" Width="100%" CssClass="TextBox" Text='<%# DataBinder.Eval(Container.DataItem, "PHKName") %>'>
									</asp:TextBox>
									<cc1:TextBoxWatermarkExtender ID="ReasonNameEdit_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="ReasonNameEdit" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>									
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="ReasonNameAdd" CssClass="TextBox" MaxLength="60" Runat="Server" Width="100%"/>
									<cc1:TextBoxWatermarkExtender ID="ReasonNameAdd_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="ReasonNameAdd" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>
								</FooterTemplate>
							</asp:TemplateField>	
							
							 <asp:TemplateField HeaderText="Blacklist" HeaderStyle-Width="30" ItemStyle-HorizontalAlign="Center" SortExpression="fgBlacklist">
								<Itemtemplate>
									<asp:Label Runat="server" Width="30" ID="Blacklist" TEXT='<%# DataBinder.Eval(Container.DataItem, "FgBlacklist") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:DropDownList ID="BlackListEdit" CssClass="DropDownList" Width="100%" runat="server" SelectedValue='<%# DataBinder.Eval(Container.DataItem, "FgBlacklist") %>'>
									  <asp:ListItem>Y</asp:ListItem>
									  <asp:ListItem>N</asp:ListItem>                                        									  
								</asp:DropDownList>								    
								</EditItemTemplate>
								<FooterTemplate>
									<asp:DropDownList ID="BlackListAdd" CssClass="DropDownList" Width="100%" runat="server">
									  <asp:ListItem Selected="True">Y</asp:ListItem>
									  <asp:ListItem>N</asp:ListItem>                                        									  
									</asp:DropDownList>								    
								</FooterTemplate>
							    <HeaderStyle Width="30px" />
                                <ItemStyle HorizontalAlign="Center" />
							</asp:TemplateField>				
							
							
							
							<asp:TemplateField HeaderText="Severance Variable" HeaderStyle-Width="50" SortExpression="Variable1">
								<Itemtemplate>
									<asp:Label Runat="server" ID="SeveranceVariable" text='<%# DataBinder.Eval(Container.DataItem, "variable1") %>'>									
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:TextBox Runat="server" ID="SeveranceVariableEdit" MaxLength="9" Width="100%" CssClass="TextBox" Text='<%# DataBinder.Eval(Container.DataItem, "Variable1") %>'>									
									</asp:TextBox>
									 <cc1:TextBoxWatermarkExtender ID="SeveranceVariableEdit_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="SeveranceVariableEdit" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>	
                                     								                                    
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="SeveranceVariableAdd" CssClass="TextBox" MaxLength="9" Runat="Server" Width="100%"/>
									
									 <cc1:TextBoxWatermarkExtender  ID="SeveranceVariableAdd_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="SeveranceVariableAdd" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender> 
                                                                        
								</FooterTemplate>
							</asp:TemplateField>	
							
							<asp:TemplateField HeaderText="Service Variable" HeaderStyle-Width="50" SortExpression="Variable2">
								<Itemtemplate>
									<asp:Label Runat="server" ID="ServiceVariable" text='<%# DataBinder.Eval(Container.DataItem, "variable2") %>'>									
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:TextBox Runat="server" ID="ServiceVariableEdit" MaxLength="9" Width="100%" CssClass="TextBox" Text='<%# DataBinder.Eval(Container.DataItem, "Variable2") %>'>									
									</asp:TextBox>
									 <cc1:TextBoxWatermarkExtender ID="ServiceVariableEdit_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="ServiceVariableEdit" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>	
                                     								                                    
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="ServiceVariableAdd" CssClass="TextBox" MaxLength="9" Runat="Server" Width="100%"/>
									
									 <cc1:TextBoxWatermarkExtender  ID="ServiceVariableAdd_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="ServiceVariableAdd" 
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
								   <%--<asp:Button CommandName="Insert" Text="Add" CssClass="Button" ID="btnAdd" Runat="server" Width="95" />--%>
								   <asp:Button class="bitbtn btnadd" runat="server" ID="btnAdd" Text="Add" CommandName="Insert" />																						 																		
									
								</FooterTemplate>
							</asp:TemplateField>
							
    					</Columns>
        </asp:GridView>
        </div >
     <asp:Label ID="lstatus" ForeColor="red" runat="server"></asp:Label>     
    
    </div>
    </form>
</body>

</html>
