<%@ Page Language="VB" AutoEventWireup="false" CodeFile="MsTPH.aspx.vb" Inherits="Master_MsTPH_MsTPH" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>TPH File</title>
    <script src="../../Function/OpenDlg.js" type="text/javascript"></script>
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div class="Content">
     <div class="H1">TPH File</div>
     <hr style="color:Blue" />
      <table>
        <tr>
            <td style="width:100px;text-align:right" >Quick Search :</td>
            <td><asp:TextBox runat="server" ID ="tbFilter" CssClass="TextBox"/> 
                <asp:DropDownList runat="server" ID="ddlField" CssClass="DropDownList" >
                  <asp:ListItem Selected="true" Text="TPH" Value="TPH"></asp:ListItem>
                  <asp:ListItem Text="Transit" Value="FgTransit"></asp:ListItem>        
                  <asp:ListItem Text="Factory" Value="FgFactor"></asp:ListItem>         
                </asp:DropDownList>     
                <asp:Button class="bitbtn btnsearch" runat="server" ID="btnSearch" Text="Search" />
                <asp:Button class="btngo" runat="server" ID="btnExpand" Text="..."/>
                <asp:Button class="bitbtn btnprint" runat="server" ID="btnPrint" Text="Print"/>
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
                  <asp:ListItem Selected="true" Text="TPH" Value="TPH"></asp:ListItem>
                  <asp:ListItem Text="Transit" Value="FgTransit"></asp:ListItem>        
                  <asp:ListItem Text="Factory" Value="FgFactor"></asp:ListItem>          
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
						<PagerStyle CssClass="GridPager" />
				      <Columns>
							<asp:TemplateField HeaderText="TPH" HeaderStyle-Width="100" SortExpression="TPH">
								<Itemtemplate>
									<asp:Label Runat="server" ID="TPH" text='<%# DataBinder.Eval(Container.DataItem, "TPH") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:Label Runat="server" ID="TPHEdit" Text='<%# DataBinder.Eval(Container.DataItem, "TPH") %>'>
									</asp:Label>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="TPHAdd" CssClass="TextBox" MaxLength="20" Runat="Server" Width="100%"/>
									<cc1:TextBoxWatermarkExtender ID="TPHAdd_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="TPHAdd" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>
								</FooterTemplate>

<HeaderStyle Width="100px"></HeaderStyle>
							</asp:TemplateField>
							
								<asp:TemplateField HeaderText="Transit" HeaderStyle-Width="80" ItemStyle-HorizontalAlign="Center" SortExpression="FgTransit">
								<Itemtemplate>
									<asp:Label Runat="server" Width="80" ID="FgTransit" TEXT='<%# DataBinder.Eval(Container.DataItem, "FgTransit") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:DropDownList ID="FgTransitEdit" CssClass="DropDownList" Width="100%" runat="server" SelectedValue='<%# DataBinder.Eval(Container.DataItem, "FgTransit") %>'>
									  <asp:ListItem>Y</asp:ListItem>
									  <asp:ListItem>N</asp:ListItem>                                                                            
								</asp:DropDownList>								    
								</EditItemTemplate>
								<FooterTemplate>
									<asp:DropDownList ID="FgTransitAdd" CssClass="DropDownList" Width="100%" runat="server">
									  <asp:ListItem Selected="True">Y</asp:ListItem>                                     
									  <asp:ListItem>N</asp:ListItem>                               
									</asp:DropDownList>								    
								</FooterTemplate>
<HeaderStyle Width="80px" />
                                <ItemStyle HorizontalAlign="Center" />
							</asp:TemplateField>		
							
								
								<asp:TemplateField HeaderText="Factory" HeaderStyle-Width="80" ItemStyle-HorizontalAlign="Center" SortExpression="FgFactory">
								<Itemtemplate>
									<asp:Label Runat="server" Width="80" ID="FgFactory" TEXT='<%# DataBinder.Eval(Container.DataItem, "FgFactory") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:DropDownList ID="FgFactoryEdit" CssClass="DropDownList" Width="100%" runat="server" SelectedValue='<%# DataBinder.Eval(Container.DataItem, "FgFactory") %>'>
									  <asp:ListItem>Y</asp:ListItem>
									  <asp:ListItem>N</asp:ListItem>                                                                            
								</asp:DropDownList>								    
								</EditItemTemplate>
								<FooterTemplate>
									<asp:DropDownList ID="FgFactoryAdd" CssClass="DropDownList" Width="100%" runat="server">
									  <asp:ListItem Selected="True">Y</asp:ListItem>                                     
									  <asp:ListItem>N</asp:ListItem>                               
									</asp:DropDownList>								    
								</FooterTemplate>
<HeaderStyle Width="80px" />
                                <ItemStyle HorizontalAlign="Center" />
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="SPBL" HeaderStyle-Width="80" ItemStyle-HorizontalAlign="Center" SortExpression="FgSPBL">
								<Itemtemplate>
									<asp:Label Runat="server" Width="80" ID="FgSPBL" TEXT='<%# DataBinder.Eval(Container.DataItem, "FgSPBL") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:DropDownList ID="FgSPBLEdit" CssClass="DropDownList" Width="100%" runat="server" SelectedValue='<%# DataBinder.Eval(Container.DataItem, "FgSPBL") %>'>
									  <asp:ListItem>Y</asp:ListItem>
									  <asp:ListItem>N</asp:ListItem>                                                                            
								</asp:DropDownList>								    
								</EditItemTemplate>
								<FooterTemplate>
									<asp:DropDownList ID="FgSPBLAdd" CssClass="DropDownList" Width="100%" runat="server">
									  <asp:ListItem Selected="True">Y</asp:ListItem>                                     
									  <asp:ListItem>N</asp:ListItem>                               
									</asp:DropDownList>								    
								</FooterTemplate>
<HeaderStyle Width="80px" />
                                <ItemStyle HorizontalAlign="Center" />
							</asp:TemplateField>


							
							<asp:TemplateField HeaderText="Action" headerstyle-width="126" >
								<ItemTemplate>
								    <asp:Button ID="btnEdit" runat="server" class="bitbtndt btnedit" Text="Edit" CommandName="Edit"/>
									<asp:Button ID="btndelete" runat="server" class="bitbtndt btndelete" Text="Delete" OnClientClick="return confirm('Sure to delete this data?');" CommandName="Delete"/>									
								</ItemTemplate>
								<EditItemTemplate>
									<asp:Button ID="btnUpdate" runat="server" class="bitbtndt btnsave" Text="Save" CommandName="Update"/>									
                                    <asp:Button ID="btnCancel" runat="server" class="bitbtndt btncancel" Text="Cancel" CommandName="Cancel"/>									
								</EditItemTemplate>								
								<FooterTemplate>
									<asp:Button class="bitbtndt btnadd" runat="server" ID="btnAdd" Text="Add" CommandName="Insert"/>									
								</FooterTemplate>

<HeaderStyle Width="126px"></HeaderStyle>
							</asp:TemplateField>
							
							
							
    					</Columns>
        </asp:GridView>
                       

     <asp:Label ID="lstatus" ForeColor="red" runat="server"></asp:Label>        
    </div>
    </form>
</body>
</html>
