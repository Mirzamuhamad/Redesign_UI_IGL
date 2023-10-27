<%@ Page Language="VB" AutoEventWireup="false" CodeFile="MsSetupGiro.aspx.vb" Inherits="Master_MsSetupGiro_MsSetupGiro" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Setup Giro</title>
    <script src="../../Function/OpenDlg.js" type="text/javascript"></script>
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">   
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager> 
    <div class="Content">
     <div class="H1">Setup Giro</div>
     <hr style="color:Blue" />
      <table>
        <tr>
            <td style="width:100px;text-align:right" >Giro Type :</td>
            <td>
                <asp:DropDownList ID="ddlType" CssClass="DropDownList" runat="server" 
                    AutoPostBack="True">                    
                </asp:DropDownList>
                
            </td>
        </tr>
     </table>
     
      <br />
      <asp:GridView id="DataGrid" runat="server" ShowFooter="True" AllowSorting="True" 
            AutoGenerateColumns="False" AllowPaging="True" CssClass="Grid">
						<HeaderStyle CssClass="GridHeader"></HeaderStyle>
						<RowStyle CssClass="GridItem" Wrap="false"/>
						<AlternatingRowStyle CssClass="GridAltItem"/>
						<FooterStyle CssClass="GridFooter" />
						<PagerStyle CssClass="GridPager" />
				      <Columns>
							<asp:TemplateField HeaderText="Currency" SortExpression="Currency" HeaderStyle-Width = "50px">
								<Itemtemplate>
									<asp:Label Runat="server" ID="Currency" text='<%# DataBinder.Eval(Container.DataItem, "Currency") %>'>
									</asp:Label>					
								</Itemtemplate>
								<EditItemTemplate>
									<asp:DropDownList Runat="server" ID="CurrencyEdit" CssClass="DropDownList" Width="100%"  Enabled = "false" 
									SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Currency") %>' 
                                        DataSourceID="dsCurrency" DataTextField="Currency" 
                                        DataValueField="Currency">									    
									</asp:DropDownList>	
								</EditItemTemplate>
								<FooterTemplate>
									<asp:DropDownList ID="CurrencyAdd" Runat="Server" Width="100%" CssClass="DropDownList"									    
                                        DataSourceID="dsCurrency" DataTextField="Currency" 
                                        DataValueField="Currency">
									</asp:DropDownList>
								</FooterTemplate>
								<HeaderStyle Width="50px"></HeaderStyle>
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Account" HeaderStyle-Width="150px" SortExpression="Account">
								<Itemtemplate>
								    <asp:Label ID="Account" Runat="server" text='<%# DataBinder.Eval(Container.DataItem, "Account") %>'>
                                    </asp:Label>									
								</Itemtemplate>
								<EditItemTemplate>
								    <asp:TextBox ID="AccountEdit" runat="server" AutoPostBack="true" CssClass="TextBox" ontextchanged="tbAccount_TextChanged" text='<%# DataBinder.Eval(Container.DataItem, "Account") %>' Width="90" MaxLength="12" />
                            		<asp:Button ID="btnAccEdit" runat="server" class="btngo" Text=" ... " CommandName="SearchEdit"/>	
								</EditItemTemplate>
								<FooterTemplate>
								   <asp:TextBox ID="AccountAdd" runat="server" AutoPostBack="true" CssClass="TextBox" ontextchanged="tbAccount_TextChanged" text='<%# DataBinder.Eval(Container.DataItem, "Account") %>' Width="90" MaxLength="12" />
                            		<asp:Button ID="btnAccAdd" runat="server" class="btngo" Text=" ... " CommandName="SearchAdd"/>
								</FooterTemplate>
								<HeaderStyle Width="150px"></HeaderStyle>
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Description" HeaderStyle-Width="300" SortExpression="Description" >
								<Itemtemplate>
									<asp:Label Runat="server" ID="Description" text='<%# DataBinder.Eval(Container.DataItem, "Description") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:Label Runat="server" ID="DescriptionEdit" MaxLength="60" CssClass="TextBox" Width="100%" Text='<%# DataBinder.Eval(Container.DataItem, "Description") %>'>
									</asp:Label>																		
								</EditItemTemplate>
								<FooterTemplate>
									<asp:Label ID="DescriptionAdd" Runat="Server" MaxLength="60" CssClass="TextBox" Width="100%"/>
								</FooterTemplate>
								<HeaderStyle Width="300px"></HeaderStyle>
							</asp:TemplateField>							
							
							
							<asp:TemplateField HeaderText="Action" headerstyle-width="126" >
								<ItemTemplate>
								   <asp:Button ID="btnEdit" runat="server" class="bitbtndt btnedit" Text="Edit" CommandName="Edit"/>
								   &nbsp;
								   <asp:Button ID="btnDelete" runat="server" class="bitbtndt btndelete" Text="Delete" OnClientClick="return confirm('Sure to delete this data?');" CommandName="Delete"/>										
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
        
        <asp:SqlDataSource ID="dsCurrency" runat="server" SelectCommand="SELECT Currency FROM VMsCurrency"></asp:SqlDataSource>
     <asp:Label ID="lstatus" ForeColor="red" runat="server"></asp:Label>        
    </div>
    </form>
</body>
</html>
