<%@ Page Language="VB" AutoEventWireup="false" CodeFile="MsPurposeUse.aspx.vb" Inherits="Master_MsPurposeUse_MsPurposeUse" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Purpose File</title>
    <script src="../../Function/OpenDlg.JS" type="text/javascript"></script> 
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
 <link href="https://fonts.googleapis.com/css2?family=Muli:wght@300;400;600&display=swap" rel="stylesheet">
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div class="Content">
     <div class="H1">Purpose File</div>
     <hr style="color:Blue" />
      <table>
        <tr>
            <td style="width:100px;text-align:right" >Quick Search :</td>
            <td><asp:TextBox CssClass="TextBox" runat="server" ID ="tbFilter"/> 
                <asp:DropDownList runat="server" ID="ddlField" CssClass="DropDownList" >
                  <asp:ListItem Selected="true" Text="Purpose Code" Value="PurposeUseCode"></asp:ListItem>
                  <asp:ListItem Text="Purpose Name" Value="PurposeUseName"></asp:ListItem>        
                  <asp:ListItem Value="Account">Account</asp:ListItem>
                  <asp:ListItem Value="AccountName">Account Name</asp:ListItem>
                </asp:DropDownList>     
                <%--<asp:Button runat="server" CssClass="Button" ID="btnSearch" Text="Search" />--%>
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
            <td><asp:TextBox CssClass="TextBox" runat="server" ID ="tbfilter2"/> 
                  <asp:DropDownList runat="server" ID="ddlField2" CssClass="DropDownList" >
                    <asp:ListItem Selected="true" Text="Purpose Code" Value="PurposeUseCode"></asp:ListItem>
                    <asp:ListItem Text="Purpose Name" Value="PurposeUseName"></asp:ListItem> 
                    <asp:ListItem Value="Account">Account</asp:ListItem>
                    <asp:ListItem Value="AccountName">Account Name</asp:ListItem>
                  </asp:DropDownList>
                
            </td>
        </tr>
     </table>
     </asp:Panel>
      <br />
      <div style="border:0px  solid; width:100%; height:100%; overflow:auto;">
      <asp:GridView id="DataGrid" runat="server" width = "880"
            ShowFooter="True" AllowSorting="True" 
            AutoGenerateColumns="False" AllowPaging="True" CssClass="Grid">
						<HeaderStyle CssClass="GridHeader"  wrap="false" > </HeaderStyle>
						<RowStyle CssClass="GridItem" Wrap="False"/>
						<AlternatingRowStyle CssClass="GridAltItem"/>
						<FooterStyle CssClass="GridFooter" />
						<PagerStyle CssClass="GridPager"  />
				      <Columns>
							<asp:TemplateField HeaderText="Purpose Code" HeaderStyle-Width="110" SortExpression="PurposeUseCode">
								<Itemtemplate>
									<asp:Label Runat="server" ID="PurposeUseCode" text='<%# DataBinder.Eval(Container.DataItem, "PurposeUseCode") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:Label Runat="server" ID="PurposeUseCodeEdit" Text='<%# DataBinder.Eval(Container.DataItem, "PurposeUseCode") %>'>
									</asp:Label>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="PurposeUseCodeAdd" MaxLength="5" CssClass="TextBox" Runat="Server" Width="100%"/>
									<cc1:TextBoxWatermarkExtender ID="PurposeUseCodeAdd_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="PurposeUseCodeAdd" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>
								</FooterTemplate>

                            <HeaderStyle Width="110px"></HeaderStyle>
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Purpose Name" HeaderStyle-Width="300" SortExpression="PurposeUseName">
								<Itemtemplate>
									<asp:Label Runat="server" ID="PurposeUseName" Width="100%" text='<%# DataBinder.Eval(Container.DataItem, "PurposeUseName") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:TextBox Runat="server" CssClass="TextBox" ID="PurposeUseNameEdit" MaxLength="60" Width="100%" Text='<%# DataBinder.Eval(Container.DataItem, "PurposeUseName") %>'>
									</asp:TextBox>
							
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="PurposeUseNameAdd" CssClass="TextBox" MaxLength="60" Runat="Server" Width="100%"/>
									<cc1:TextBoxWatermarkExtender ID="PurposeUseNameAdd_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="PurposeUseNameAdd" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>
								</FooterTemplate>

                           <HeaderStyle Width="200px"></HeaderStyle>
							</asp:TemplateField>		
							
							<asp:TemplateField HeaderText="Account" HeaderStyle-Width="128" SortExpression="Account">
								<Itemtemplate>
									<asp:Label Runat="server" Width="128" ID="Account" TEXT='<%# DataBinder.Eval(Container.DataItem, "Account") %>'>
									</asp:Label>
								</Itemtemplate>	
								<EditItemTemplate>
								    <asp:TextBox CssClass="TextBox" runat="server" Width="90px" ID="AccountEdit" 
                                        TEXT='<%# DataBinder.Eval(Container.DataItem, "Account") %>' 
                                        AutoPostBack="true" MaxLength="12"  ontextchanged="tbAccount_TextChanged"/>
                                    <%--<asp:Button ID="btnAccountEdit" runat="server" CommandName="SearchAccountEdit" 
                                        CssClass="Button" TEXT="..." />--%>
                                    <asp:Button class="btngo" runat="server" ID="btnAccountEdit" Text="..." CommandName="SearchAccountEdit"/>
								</EditItemTemplate>							
								<FooterTemplate>
								    <asp:TextBox CssClass="TextBox" OnTextChanged="tbAccount_TextChanged" runat="server" id="AccountAdd" Width="90" 
                                        AutoPostBack="true" />
                                     <%--<asp:Button CssClass="Button" ID="btnAccountAdd" TEXT="..." runat="server" CommandName="SearchAccountAdd" />--%>
                                     <asp:Button class="btngo" runat="server" ID="btnAccountAdd" Text="..." CommandName="SearchAccountAdd"/>                                    
                                     <cc1:TextBoxWatermarkExtender ID="AccountAdd_TextBoxWatermarkExtender" 
                                        runat="server" Enabled="True" TargetControlID="AccountAdd" 
                                        WatermarkText="[Account]" WatermarkCssClass="Watermarked">
                                    </cc1:TextBoxWatermarkExtender>
								</FooterTemplate>
							    <HeaderStyle Width="128px" />
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Account Name" HeaderStyle-Width="280" SortExpression="AccountName">
								<Itemtemplate>
									<asp:Label Runat="server" Width="280" ID="AccountName" TEXT='<%# DataBinder.Eval(Container.DataItem, "AccountName") %>'>
									</asp:Label>
								</Itemtemplate>	
								<EditItemTemplate>
								    <asp:Label Runat="server" ID="AccountNameEdit" Width="280" TEXT='<%# DataBinder.Eval(Container.DataItem, "AccountName") %>'>
									</asp:Label>
								</EditItemTemplate>	
								<FooterTemplate>
                                    <asp:Label ID="AccountNameAdd" Runat="server" 
                                        TEXT='<%# DataBinder.Eval(Container.DataItem, "AccountName") %>' Width="280px"></asp:Label>
                                </FooterTemplate>
							    <HeaderStyle Width="280px" />
							</asp:TemplateField>	
							
							
							<asp:TemplateField HeaderText="Action" headerstyle-width="600px" >
								<ItemTemplate>
								
								    <asp:Button ID="btnEdit" runat="server" class="bitbtndt btnedit" Text="Edit" CommandName="Edit"/>
									<asp:Button ID="btnDelete" runat="server" class="bitbtndt btndelete" Text="Delete" OnClientClick="return confirm('Sure to delete this data?');" CommandName="Delete"/>									

								</ItemTemplate>
								<EditItemTemplate>
									<%--<asp:Button CssClass="Button" CommandName="Update" Text="Update" ID="btnUpdate" Runat="server" Width="60" />--%>
									<asp:Button ID="btnUpdate" runat="server" class="bitbtndt btnsave" Text="Save" CommandName="Update"/>									
                                    <asp:Button ID="btnCancel" runat="server" class="bitbtndt btncancel" Text="Cancel" CommandName="Cancel"/>									
									
									                
								</EditItemTemplate>
								<FooterTemplate>
								    <asp:Button class="bitbtndt btnadd" runat="server" ID="btnAdd" Text="Add" CommandName="Insert"/>									
									<%--<asp:Button CssClass="Button" CommandName="Insert" Text="Add" ID="btnAdd" Runat="server" Width="95" />--%>
									                
								</FooterTemplate>

<HeaderStyle Width="600px"></HeaderStyle>
							</asp:TemplateField>
							
    					</Columns>
        </asp:GridView>
        </div>
        <asp:SqlDataSource ID="dsAccount" runat="server"                                                                                 
           SelectCommand="SELECT AccountCode, AccountName FROM VMsAccount">                                        
        </asp:SqlDataSource>        
     <asp:Label ID="lstatus" ForeColor="red" runat="server"></asp:Label>        
    </div>
    </form>
</body>
</html>
