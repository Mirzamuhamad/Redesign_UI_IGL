<%@ Page Language="VB" AutoEventWireup="false" CodeFile="MsSeriPajak.aspx.vb" Inherits="Master_MsSeriPajak_MsSeriPajak" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="BasicFrame.WebControls.BasicDatePicker" Namespace="BasicFrame.WebControls"
    TagPrefix="BDP" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Currency File</title>
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
     <div class="H1">Seri Pajak File</div>
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
      <asp:GridView id="DataGrid" runat="server" AllowSorting="True" AutoGenerateColumns="False" AllowPaging="True" CssClass="Grid">
			<HeaderStyle CssClass="GridHeader"></HeaderStyle>
			<RowStyle CssClass="GridItem" Wrap="false"/><AlternatingRowStyle CssClass="GridAltItem"/>
			<FooterStyle CssClass="GridFooter" /><PagerStyle CssClass="GridPager" />
			<Columns>
					<asp:TemplateField HeaderText="Start Date" HeaderStyle-Width="50" SortExpression="StartDate">
						<Itemtemplate>
						  <asp:Label Runat="server" ID="StartDate" text='<%# DataBinder.Eval(Container.DataItem, "StartDate") %>'></asp:Label>
						</Itemtemplate>	
						<EditItemTemplate>
							<asp:Label Runat="server" ID="StartDateEdit" text='<%# DataBinder.Eval(Container.DataItem, "StartDate") %>'></asp:Label> 
						</EditItemTemplate>
						<FooterTemplate>
							<BDP:BasicDatePicker ID="StartDateAdd" runat="server" DateFormat="dd MMM yyyy" 
                            ReadOnly = "true" ValidationGroup="Input" ButtonImageHeight="19px" ButtonImageWidth="20px" 
                            DisplayType="TextBoxAndImage" TextBoxStyle-CssClass="TextDate"                             
                            ShowNoneButton="False"><TextBoxStyle CssClass="TextDate" /></BDP:BasicDatePicker>  
						</FooterTemplate>														
					</asp:TemplateField>
					
					<asp:TemplateField HeaderText="End Date" HeaderStyle-Width="50" SortExpression="EndDate">
						<Itemtemplate>
						  <asp:Label Runat="server" ID="EndDate" text='<%# DataBinder.Eval(Container.DataItem, "EndDate") %>'></asp:Label>
						</Itemtemplate>	
						<EditItemTemplate>
							<BDP:BasicDatePicker ID="EndDateEdit" runat="server" DateFormat="dd MMM yyyy" 
                            ReadOnly = "true" ValidationGroup="Input" ButtonImageHeight="19px" ButtonImageWidth="20px" 
                            DisplayType="TextBoxAndImage" TextBoxStyle-CssClass="TextDate" 
                            SelectedValue='<%# DataBinder.Eval(Container.DataItem, "EndDate") %>'
                            ShowNoneButton="False"><TextBoxStyle CssClass="TextDate" /></BDP:BasicDatePicker>  
						</EditItemTemplate>
						<FooterTemplate>
							<BDP:BasicDatePicker ID="EndDateAdd" runat="server" DateFormat="dd MMM yyyy" 
                            ReadOnly = "true" ValidationGroup="Input" ButtonImageHeight="19px" ButtonImageWidth="20px" 
                            DisplayType="TextBoxAndImage" TextBoxStyle-CssClass="TextDate"                             
                            ShowNoneButton="False"><TextBoxStyle CssClass="TextDate" /></BDP:BasicDatePicker>  
						</FooterTemplate>														
					</asp:TemplateField>
							
					<asp:TemplateField HeaderText="Prefix" HeaderStyle-Width="100" SortExpression="Prefix">
						 <Itemtemplate>
									<asp:Label Runat="server" ID="Prefix" text='<%# DataBinder.Eval(Container.DataItem, "Prefix") %>'>
									</asp:Label>									
						 </Itemtemplate>		
						 <EditItemTemplate>
									<asp:TextBox Runat="server" ID="PrefixEdit" MaxLength="20" Width="100%" CssClass="TextBox" Text='<%# DataBinder.Eval(Container.DataItem, "Prefix") %>'>
									</asp:TextBox>									
						 </EditItemTemplate>
						 <FooterTemplate>
									<asp:TextBox ID="PrefixAdd" CssClass="TextBox" MaxLength="20" Runat="Server" Width="100%"/>									
						 </FooterTemplate>												
					</asp:TemplateField>								
						
					<asp:TemplateField HeaderText="Start No" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="100" SortExpression="StartNo">
						 <Itemtemplate>
									<asp:Label  Runat="server" ID="StartNo" text='<%# DataBinder.Eval(Container.DataItem, "StartNo") %>'>
									</asp:Label>
						 </Itemtemplate>	
						 <EditItemTemplate>
									<asp:TextBox Runat="server" ID="StartNoEdit" MaxLength = "10" Width="100%" CssClass="TextBox" Text='<%# DataBinder.Eval(Container.DataItem, "StartNo") %>'>
									</asp:TextBox>
						 </EditItemTemplate>
						 <FooterTemplate>
									<asp:TextBox ID="StartNoAdd" CssClass="TextBox" MaxLength = "10" Runat="Server" Width="100%"/>
						 </FooterTemplate>									
					</asp:TemplateField>
					
					<asp:TemplateField HeaderText="End No" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="100" SortExpression="EndNo">
						 <Itemtemplate>
									<asp:Label  Runat="server" ID="EndNo" text='<%# DataBinder.Eval(Container.DataItem, "EndNo") %>'>
									</asp:Label>
						 </Itemtemplate>	
						 <EditItemTemplate>
									<asp:TextBox Runat="server" ID="EndNoEdit" MaxLength = "10" Width="100%" CssClass="TextBox" Text='<%# DataBinder.Eval(Container.DataItem, "EndNo") %>'>
									</asp:TextBox>
						 </EditItemTemplate>
						 <FooterTemplate>
									<asp:TextBox ID="EndNoAdd" CssClass="TextBox" MaxLength = "10" Runat="Server" Width="100%"/>
						 </FooterTemplate>									
					</asp:TemplateField>
							
					<asp:TemplateField HeaderText="Length Digit" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="60" SortExpression="LengthDigit">
						 <Itemtemplate>
									<asp:Label  Runat="server" ID="LengthDigit" text='<%# DataBinder.Eval(Container.DataItem, "LengthDigit") %>'>
									</asp:Label>
						 </Itemtemplate>	
						 <EditItemTemplate>
									<asp:TextBox Runat="server" ID="LengthDigitEdit" MaxLength = "10" Width="100%" CssClass="TextBox" Text='<%# DataBinder.Eval(Container.DataItem, "LengthDigit") %>'>
									</asp:TextBox>
						 </EditItemTemplate>
						 <FooterTemplate>
									<asp:TextBox ID="LengthDigitAdd" CssClass="TextBox" MaxLength = "10" Runat="Server" Width="100%"/>
						 </FooterTemplate>									
					</asp:TemplateField>

					 <asp:TemplateField HeaderText="Active" HeaderStyle-Width="20" SortExpression="FgActive">
						 <Itemtemplate>
									<asp:Label Runat="server" ID="FgActive" text='<%# DataBinder.Eval(Container.DataItem, "FgActive") %>'>
									</asp:Label>
						 </Itemtemplate>		
						 <EditItemTemplate>
								    <asp:DropDownList CssClass="DropDownList" Width="100%" ID="FgActiveEdit" text='<%# DataBinder.Eval(Container.DataItem, "FgActive") %>' runat="server">
                                        <asp:ListItem>Y</asp:ListItem>
                                        <asp:ListItem>N</asp:ListItem>
                                    </asp:DropDownList>
						 </EditItemTemplate>	
						 <FooterTemplate>
								    <asp:DropDownList CssClass="DropDownList" Width="100%"  ID="FgActiveAdd" runat="server">
                                        <asp:ListItem>Y</asp:ListItem>
                                        <asp:ListItem Selected="True">N</asp:ListItem>
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
