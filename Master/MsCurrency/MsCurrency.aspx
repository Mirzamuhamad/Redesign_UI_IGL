<%@ Page Language="VB" AutoEventWireup="false" CodeFile="MsCurrency.aspx.vb" Inherits="Master_MsCurrency_MsCurrency" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Currency File</title>
    <script src="../../Function/Function.JS" type="text/javascript"></script>
    <script src="../../Function/OpenDlg.js" type="text/javascript"></script>
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />    
</head>
    <body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div class="Content">
     <div class="H1">Currency File</div>
     <hr style="color:Blue" />     
      <table>
        <tr>
            <td style="width:100px;text-align:right" >Quick Search :</td>
            <td><asp:TextBox runat="server" ID ="tbFilter" CssClass="TextBox"/> 
                <asp:DropDownList runat="server" ID="ddlField" CssClass="DropDownList" >
                  <asp:ListItem Selected="true" Text="Currency Code" Value="CurrCode"></asp:ListItem>
                  <asp:ListItem Text="Currency Name" Value="CurrName"></asp:ListItem>        
                    <asp:ListItem Value="ValueToleransi">Toleransi Value</asp:ListItem>
                    <asp:ListItem Value="DigitDecimal">Digit Desimal</asp:ListItem>
                    <asp:ListItem Value="fgHome">Home</asp:ListItem>
                    <asp:ListItem Value="FgBook">Book</asp:ListItem>
                </asp:DropDownList>     
                <asp:Button class="bitbtn btnsearch" runat="server" ID="btnSearch" Text="Search" />
                <asp:Button ID="btnExpand" runat="server" class="btngo" Text="..."/>
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
                  <asp:DropDownList ID="ddlField2" runat="server" CssClass="DropDownList">
                      <asp:ListItem Selected="true" Text="Currency Code" Value="CurrCode"></asp:ListItem>
                      <asp:ListItem Text="Currency Name" Value="CurrName"></asp:ListItem>
                      <asp:ListItem Value="ValueToleransi">Toleransi Value</asp:ListItem>
                      <asp:ListItem Value="DigitDecimal">Digit Desimal</asp:ListItem>
                      <asp:ListItem Value="fgHome">Home</asp:ListItem>
                      <asp:ListItem Value="FgBook">Book</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
     </table>
     </asp:Panel>
      <br />
      <asp:GridView id="DataGrid" runat="server" AllowSorting="True" 
            AutoGenerateColumns="False" AllowPaging="True" CssClass="Grid">
						<HeaderStyle CssClass="GridHeader"></HeaderStyle>
						<RowStyle CssClass="GridItem" Wrap="false"/>
						<AlternatingRowStyle CssClass="GridAltItem"/>
						<FooterStyle CssClass="GridFooter" />
						<PagerStyle CssClass="GridPager" />
				      <Columns>
							<asp:TemplateField HeaderText="Currency Code" HeaderStyle-Width="60" SortExpression="CurrCode">
								<Itemtemplate>
									<asp:Label Runat="server" ID="CurrCode" text='<%# DataBinder.Eval(Container.DataItem, "CurrCode") %>'>
									</asp:Label>
								</Itemtemplate>	
								<EditItemTemplate>
									<asp:Label Runat="server" ID="CurrCodeEdit" Width="100%" Text='<%# DataBinder.Eval(Container.DataItem, "CurrCode") %>'>
									</asp:Label>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="CurrCodeAdd" CssClass="TextBox" MaxLength="5" Runat="Server" Width="100%"/>
									<cc1:TextBoxWatermarkExtender ID="CurrCodeAdd_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="CurrCodeAdd" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>
								</FooterTemplate>														
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Currency Name" HeaderStyle-Width="320" SortExpression="CurrName">
								<Itemtemplate>
									<asp:Label Runat="server" ID="CurrName" text='<%# DataBinder.Eval(Container.DataItem, "CurrName") %>'>
									</asp:Label>									
								</Itemtemplate>		
								<EditItemTemplate>
									<asp:TextBox Runat="server" ID="CurrNameEdit" MaxLength="60" Width="100%" CssClass="TextBox" Text='<%# DataBinder.Eval(Container.DataItem, "CurrName") %>'>
									</asp:TextBox>
									<cc1:TextBoxWatermarkExtender ID="CurrNameEdit_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="CurrNameEdit" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="CurrNameAdd" CssClass="TextBox" MaxLength="60" Runat="Server" Width="100%"/>
									<cc1:TextBoxWatermarkExtender ID="CurrNameAdd_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="CurrNameAdd" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>
								</FooterTemplate>												
							</asp:TemplateField>								
						
							<asp:TemplateField HeaderText="Toleransi Value" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="60" SortExpression="ValueToleransi">
								<Itemtemplate>
									<asp:Label  Runat="server" ID="ValueToleransi" text='<%# DataBinder.Eval(Container.DataItem, "ValueToleransi") %>'>
									</asp:Label>
								</Itemtemplate>	
								<EditItemTemplate>
									<asp:TextBox Runat="server" ID="ValueToleransiEdit" MaxLength = "9" Width="100%" CssClass="TextBox" Text='<%# DataBinder.Eval(Container.DataItem, "ValueToleransi") %>'>
									</asp:TextBox>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="ValueToleransiAdd" CssClass="TextBox" MaxLength = "9" Runat="Server" Width="100%"/>
								</FooterTemplate>									
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Digit Desimal" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="60" SortExpression="DigitDecimal">
								<Itemtemplate>
									<asp:Label Runat="server" ID="DigitDecimal" text='<%# DataBinder.Eval(Container.DataItem, "DigitDecimal") %>'>
									</asp:Label>
								</Itemtemplate>	
								<EditItemTemplate>
									<asp:DropDownList CssClass="DropDownList" ID="DigitDecimalEdit" Width="100%" runat="server" text='<%# DataBinder.Eval(Container.DataItem, "DigitDecimal") %>'>
                                        <asp:ListItem>0</asp:ListItem>
                                        <asp:ListItem>1</asp:ListItem>
                                        <asp:ListItem>2</asp:ListItem>
                                        <asp:ListItem>3</asp:ListItem>
                                        <asp:ListItem>4</asp:ListItem>
                                    </asp:DropDownList>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:DropDownList CssClass="DropDownList" Width="100%" ID="DigitDecimalAdd" runat="server">
                                        <asp:ListItem Selected="True">0</asp:ListItem>
                                        <asp:ListItem>1</asp:ListItem>
                                        <asp:ListItem>2</asp:ListItem>
                                        <asp:ListItem>3</asp:ListItem>
                                        <asp:ListItem>4</asp:ListItem>
                                    </asp:DropDownList>
								</FooterTemplate>							
							</asp:TemplateField>

							<asp:TemplateField HeaderText="Home" HeaderStyle-Width="40" SortExpression="FgHome">
								<Itemtemplate>
									<asp:Label Runat="server" ID="FgHome" text='<%# DataBinder.Eval(Container.DataItem, "FgHome") %>'>
									</asp:Label>
								</Itemtemplate>		
								<EditItemTemplate>
								    <asp:DropDownList CssClass="DropDownList" Width="100%" ID="FgHomeEdit" text='<%# DataBinder.Eval(Container.DataItem, "FgHome") %>' runat="server">
                                        <asp:ListItem>Y</asp:ListItem>
                                        <asp:ListItem>N</asp:ListItem>
                                    </asp:DropDownList>
								</EditItemTemplate>	
								<FooterTemplate>
								    <asp:DropDownList CssClass="DropDownList" Width="100%"  ID="FgHomeAdd" runat="server">
                                        <asp:ListItem>Y</asp:ListItem>
                                        <asp:ListItem Selected="True">N</asp:ListItem>
                                    </asp:DropDownList>
								</FooterTemplate>
							</asp:TemplateField>


							<asp:TemplateField HeaderText="Book" HeaderStyle-Width="40" SortExpression="FgBook">
								<Itemtemplate>
									<asp:Label Runat="server" ID="FgBook" text='<%# DataBinder.Eval(Container.DataItem, "FgBook") %>'>
									</asp:Label>
								</Itemtemplate>	
								<EditItemTemplate>
								    <asp:DropDownList CssClass="DropDownList" ID="FgBookEdit" Width="100%" runat="server" text='<%# DataBinder.Eval(Container.DataItem, "FgBook") %>'>
                                        <asp:ListItem>Y</asp:ListItem>
                                        <asp:ListItem>N</asp:ListItem>
                                    </asp:DropDownList>
								</EditItemTemplate>							
								<FooterTemplate>
								    <asp:DropDownList CssClass="DropDownList" ID="FgBookAdd" Width="100%" runat="server">
                                        <asp:ListItem Selected="True">Y</asp:ListItem>
                                        <asp:ListItem>N</asp:ListItem>
                                    </asp:DropDownList>
								</FooterTemplate>
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
							</asp:TemplateField>							
    					</Columns>
        </asp:GridView>       
     <asp:Label ID="lstatus" ForeColor="red" runat="server"></asp:Label>        
    </div>
    </form>
</body>
</html>
