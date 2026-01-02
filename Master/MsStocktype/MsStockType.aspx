<%@ Page Language="VB" AutoEventWireup="false" CodeFile="MsStockType.aspx.vb" Inherits="MsStockType_MsStockType" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Stock Type File</title>
    <script src="../../Function/OpenDlg.js" type="text/javascript"></script>    
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
 <link href="https://fonts.googleapis.com/css2?family=Muli:wght@300;400;600&display=swap" rel="stylesheet">
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div class="Content">
     <div class="H1"><asp:Label runat="server" ID="lblTitle" Text ="Stock Type File"></asp:Label></div>
     <hr style="color:Blue" />
      <table>
        <tr>
            <td style="width:100px;text-align:right" >Quick Search :</td>
            <td><asp:TextBox runat="server" ID ="tbFilter" CssClass="TextBox" /> 
                <asp:DropDownList runat="server" ID="ddlField" CssClass="DropDownList" > 
                    <asp:ListItem Selected="true" Text="Stock Type Code" Value="Stock_Type_Code"></asp:ListItem>
                    <asp:ListItem Text="Stock Type Name" Value="Stock_Type_Name"></asp:ListItem>                   
                </asp:DropDownList>     
                <asp:Button runat="server" ID="btnSearch" CssClass="bitbtn btnsearch" Text="Search" />
                <asp:Button runat="server" ID="btnExpand" CssClass="bitbtn btngo" Text="..." />
                &nbsp
                <asp:Button runat="server" ID="btnPrint" CssClass="bitbtn btnprint" Text="Print" />                
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
                    <asp:ListItem Selected="true" Text="Stock Type Code" Value="Stock_Type_Code"></asp:ListItem>
                    <asp:ListItem Text="Stock Type Name" Value="Stock_Type_Name"></asp:ListItem>                   
                  </asp:DropDownList>
                
            </td>
        </tr>
     </table>
     </asp:Panel>
      <br />
      <asp:GridView id="DataGrid" runat="server" ShowFooter="True" AllowSorting="True" Width="1000"
            AutoGenerateColumns="False" AllowPaging="True" CssClass="Grid">
						<HeaderStyle CssClass="GridHeader"></HeaderStyle>
						<RowStyle CssClass="GridItem" Wrap="false"/>
						<AlternatingRowStyle CssClass="GridAltItem"/>
						<FooterStyle CssClass="GridFooter" />
						<PagerStyle CssClass="GridPager" />
				      <Columns>
							<asp:TemplateField HeaderText="Code" HeaderStyle-Width="80" SortExpression="Stock_Type_Code">
								<Itemtemplate>
									<asp:Label Runat="server" ID="StockTypeCode" text='<%# DataBinder.Eval(Container.DataItem, "Stock_Type_Code") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:Label Runat="server" ID="StockTypeCodeEdit" Text='<%# DataBinder.Eval(Container.DataItem, "Stock_Type_Code") %>'>
									</asp:Label>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="StockTypeCodeAdd" CssClass="TextBox" MaxLength="5" Runat="Server" Width="100%"/>
									<cc1:TextBoxWatermarkExtender ID="StockTypeCodeAdd_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="StockTypeCodeAdd" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>
								</FooterTemplate>
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Name" HeaderStyle-Width="180" SortExpression="Stock_Type_Name">
								<Itemtemplate>
									<asp:Label Runat="server" ID="StockTypeName" text='<%# DataBinder.Eval(Container.DataItem, "Stock_Type_Name") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:TextBox Runat="server" ID="StockTypeNameEdit" MaxLength="60" CssClass="TextBox" Width="100%" Text='<%# DataBinder.Eval(Container.DataItem, "Stock_Type_Name") %>'>
									</asp:TextBox>
									<cc1:TextBoxWatermarkExtender ID="StockTypeNameEdit_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="StockTypeNameEdit" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="StockTypeNameAdd" Runat="Server" MaxLength="60" CssClass="TextBox" Width="100%"/>
									<cc1:TextBoxWatermarkExtender ID="StockTypeNameAdd_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="StockTypeNameAdd" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>
								</FooterTemplate>
							</asp:TemplateField>		
							
							<asp:TemplateField HeaderText="Account Expense" HeaderStyle-Width="150" SortExpression="AccountExpense">
								<Itemtemplate>
									<asp:Label Runat="server" ID="Account" text='<%# DataBinder.Eval(Container.DataItem, "AccountExpense") %>'>
									</asp:Label>
								</Itemtemplate>	
								<EditItemTemplate>
									<asp:TextBox Runat="server" ID="AccountEdit" MaxLength="12" Width="80%" CssClass="TextBox" 
									Text='<%# DataBinder.Eval(Container.DataItem, "AccountExpense") %>'
									ontextchanged="AccountEdit_TextChanged" AutoPostBack="true"/>
									<cc1:TextBoxWatermarkExtender ID="AccountEdit_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="AccountEdit" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                    </cc1:TextBoxWatermarkExtender>									
									<asp:Button runat="server" ID="btnAccEdit" CommandName="btnAccEdit" CssClass="btngo" Text="..." />
                				</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="AccountAdd" CssClass="TextBox" MaxLength="12" Width="80%" 
									ontextchanged="AccountAdd_TextChanged" AutoPostBack="true" Runat="Server" />
									<cc1:TextBoxWatermarkExtender ID="AccountAdd_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="AccountAdd" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                    </cc1:TextBoxWatermarkExtender>									
								    <asp:Button runat="server" ID="btnAccAdd" CommandName="btnAccAdd" CssClass="btngo" Text="..." />                					
								</FooterTemplate>															
							</asp:TemplateField>	
							
							<asp:TemplateField HeaderText="Account Expense Name" HeaderStyle-Width="180" SortExpression="AccountExpenseName">
								<Itemtemplate>
									<asp:Label Runat="server" ID="AccountName" text='<%# DataBinder.Eval(Container.DataItem, "AccountExpenseName") %>'>
									</asp:Label>
								</Itemtemplate>	
								<EditItemTemplate>
									<asp:Label Runat="server" ID="AccountNameEdit" Text='<%# DataBinder.Eval(Container.DataItem, "AccountExpenseName") %>'>
									</asp:Label>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:Label Runat="server" ID="AccountNameAdd">
									</asp:Label>
								</FooterTemplate>							
							</asp:TemplateField>													
							
							<asp:TemplateField HeaderText="Account Income" HeaderStyle-Width="150" SortExpression="AccountIncome">
								<Itemtemplate>
									<asp:Label Runat="server" ID="AccountIncome" text='<%# DataBinder.Eval(Container.DataItem, "AccountIncome") %>'>
									</asp:Label>
								</Itemtemplate>	
								<EditItemTemplate>
									<asp:TextBox Runat="server" ID="AccountIncomeEdit" MaxLength="12" Width="80%" CssClass="TextBox" 
									Text='<%# DataBinder.Eval(Container.DataItem, "AccountIncome") %>'
									ontextchanged="AccountIncomeEdit_TextChanged" AutoPostBack="true"/>
									<cc1:TextBoxWatermarkExtender ID="AccountIncomeEdit_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="AccountIncomeEdit" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                    </cc1:TextBoxWatermarkExtender>									
									<asp:Button runat="server" ID="btnAccIncomeEdit" CommandName="btnAccIncomeEdit" CssClass="btngo" Text="..." />
                				</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="AccountIncomeAdd" CssClass="TextBox" MaxLength="12" Width="80%" 
									ontextchanged="AccountIncomeAdd_TextChanged" AutoPostBack="true" Runat="Server" />
									<cc1:TextBoxWatermarkExtender ID="AccountIncomeAdd_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="AccountIncomeAdd" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                    </cc1:TextBoxWatermarkExtender>									
								    <asp:Button runat="server" ID="btnAccIncomeAdd" CommandName="btnAccIncomeAdd" CssClass="btngo" Text="..." />                					
								</FooterTemplate>															
							</asp:TemplateField>	
							
							<asp:TemplateField HeaderText="Account Income Name" HeaderStyle-Width="180" SortExpression="AccountIncomeName">
								<Itemtemplate>
									<asp:Label Runat="server" ID="AccountNameIncome" text='<%# DataBinder.Eval(Container.DataItem, "AccountIncomeName") %>'>
									</asp:Label>
								</Itemtemplate>	
								<EditItemTemplate>
									<asp:Label Runat="server" ID="AccountNameIncomeEdit" Text='<%# DataBinder.Eval(Container.DataItem, "AccountIncomeName") %>'>
									</asp:Label>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:Label Runat="server" ID="AccountNameIncomeAdd">
									</asp:Label>
								</FooterTemplate>							
							</asp:TemplateField>													
							
							<asp:TemplateField HeaderText="Action" headerstyle-width="126" >
								<ItemTemplate>									
									<asp:Button runat="server" ID="btnEdit" CommandName="Edit" CssClass="bitbtn btnedit" Text="Edit"/>                					
									&nbsp;									
									<asp:Button runat="server" ID="btnDelete" OnClientClick="return confirm('Sure to delete this data?');" CommandName="Delete"  CssClass="bitbtn btndelete" Text="Delete" />                					
									<asp:Button runat="server" ID="btnDetail" CommandName="Detail" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" CssClass="bitbtn btndetail" Text="Detail" />                														
								</ItemTemplate>
								<EditItemTemplate>
									<asp:Button CommandName="Update" Text="Update" CssClass="bitbtn btnsave" ID="btnUpdate" Runat="server"/>
									&nbsp;
									<asp:Button CommandName="Cancel" Text="Cancel" CssClass="bitbtn btncancel" ID="btnCancel" Runat="server"/>									
								</EditItemTemplate>
								<FooterTemplate>
									<asp:Button CommandName="Insert" Text="Add" CssClass="bitbtn btnadd" ID="btnAdd" Runat="server"/>									
								</FooterTemplate>
							</asp:TemplateField>
							
    					</Columns>
        </asp:GridView>                
     <asp:Label ID="lstatus" ForeColor="red" runat="server"></asp:Label>        
    </div>
    </form>
</body>
</html>
