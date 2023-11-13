<%@ Page Language="VB" AutoEventWireup="false" CodeFile="MsPetty.Aspx.vb" Inherits="Master_MsPetty_MsPetty" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Petty Type File</title>
    <script src="../../Function/OpenDlg.JS" type="text/javascript"></script>
    <script type="text/javascript">
    function OpenPopup() {         
        window.open("../../SearchDlg.Aspx","List","scrollbars=yes,resizable=no,width=500,height=400");        
        return false;
    } 
   
</script>    
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div class="Content">
     <div class="H1">Petty File</div>
     <hr style="color:Blue" />
     <table>
        <tr>
            <td style="text-align:right; width:100px">Quick Search :</td>
            <td><asp:TextBox CssClass="TextBox" runat="server" ID ="tbFilter"/> 
                <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlField" >
                  <asp:ListItem Selected="true" Text="Petty Code" Value="PettyCode"></asp:ListItem>
                  <asp:ListItem Text="Petty Name" Value="PettyName"></asp:ListItem>        
                  <asp:ListItem Value="A.Account">Account</asp:ListItem>        
                    <asp:ListItem Text="Account Name" Value="AccountName"></asp:ListItem>
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
            <td style="text-align:right; width:100px">
                    <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlNotasi" >
                    <asp:ListItem Selected="true" Text="OR" Value="OR"></asp:ListItem>
                    <asp:ListItem Text="AND" Value="AND"></asp:ListItem>         
                </asp:DropDownList>
            </td>
            <td><asp:TextBox CssClass="TextBox" runat="server" ID ="tbfilter2"/> 
                  <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlField2" >
                    <asp:ListItem Selected="true" Text="Petty Code" Value="PettyCode"></asp:ListItem>
                    <asp:ListItem Text="Petty Name" Value="PettyName"></asp:ListItem>        
                    <asp:ListItem Value="A.Account">Account</asp:ListItem>        
                      <asp:ListItem Text="Account Name" Value="AccountName"></asp:ListItem>
                  </asp:DropDownList> 
                                                                     
            </td>
        </tr>
     </table>
     </asp:Panel>
     <br />           
     <%--<div style="border:0px  solid; width:100%; height:100%; overflow:auto;">--%>
      <asp:GridView id="DataGrid" ShowFooter="True" runat="server" AllowSorting="True" 
            AutoGenerateColumns="False" AllowPaging="True" CssClass="Grid">
						<HeaderStyle CssClass="GridHeader" Wrap = "false" ></HeaderStyle>
						<RowStyle CssClass="GridItem" wrap="false"/>
						<AlternatingRowStyle CssClass="GridAltItem"/>
						<FooterStyle CssClass="GridFooter" />
						<PagerStyle CssClass="GridPager" />
				      <Columns>
							<asp:TemplateField HeaderText="Petty Code" HeaderStyle-Width="90" SortExpression="PettyCode">
								<Itemtemplate>
									<asp:Label Runat="server" ID="PettyCode" text='<%# DataBinder.Eval(Container.DataItem, "PettyCode") %>'>
									</asp:Label>
								</Itemtemplate>		
								<EditItemTemplate>
								    <asp:Label runat="server" ID="PettyCodeEdit" text='<%# DataBinder.Eval(Container.DataItem, "PettyCode") %>'/>
								</EditItemTemplate>
								<FooterTemplate>
								    <asp:TextBox Width="100%" placeholder="can't blank" CssClass="TextBox" runat="server" MaxLength="5" ID="PettyCodeAdd" />                                    
								    <%--<cc1:TextBoxWatermarkExtender ID="PettyCodeAdd_TextBoxWatermarkExtender" 
                                        runat="server" Enabled="True" TargetControlID="PettyCodeAdd" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                    </cc1:TextBoxWatermarkExtender>--%>
								</FooterTemplate>												

                            <HeaderStyle Width="90px"></HeaderStyle>
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Petty Name" HeaderStyle-Width="330" SortExpression="PettyName">
								<Itemtemplate>
									<asp:Label Runat="server" ID="PettyName" text='<%# DataBinder.Eval(Container.DataItem, "PettyName") %>'>
									</asp:Label>
								</Itemtemplate>	
								<EditItemTemplate>
								    <asp:TextBox CssClass="TextBox" Width="100%" runat="server" MaxLength="60" ID="PettyNameEdit" text='<%# DataBinder.Eval(Container.DataItem, "PettyName") %>'/>
								    <cc1:TextBoxWatermarkExtender ID="PettyNameEdit_TextBoxWatermarkExtender" 
                                        runat="server" Enabled="True" TargetControlID="PettyNameEdit" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                    </cc1:TextBoxWatermarkExtender>
								</EditItemTemplate>													
								<FooterTemplate>
								    <asp:TextBox CssClass="TextBox" Placeholder ="can't blank" runat="server" MaxLength="60" ID="PettyNameAdd" Width="100%" />
								    <%--<cc1:TextBoxWatermarkExtender ID="PettyNameAdd_TextBoxWatermarkExtender" 
                                        runat="server" Enabled="True" TargetControlID="PettyNameAdd" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                    </cc1:TextBoxWatermarkExtender>--%>
								</FooterTemplate>

<HeaderStyle Width="330px"></HeaderStyle>
							</asp:TemplateField>	
							
							<asp:TemplateField HeaderText="Account" HeaderStyle-Width="120px" SortExpression="Account">
								<Itemtemplate>
									<asp:Label Runat="server" ID="Account" text='<%# DataBinder.Eval(Container.DataItem, "Account") %>'>
									</asp:Label>
								</Itemtemplate>		
								<EditItemTemplate>
								    <asp:TextBox CssClass="TextBox" runat="server" ID="AccountEdit" Width="65%" 
                                        text='<%# DataBinder.Eval(Container.DataItem, "Account") %>' 
                                        ontextchanged="AccountEdit_TextChanged" AutoPostBack="true"/>
                                    <asp:Button class="btngo" runat="server" ID="btnAccEdit" Text="..." CommandName="SearchEdit" />                              
								</EditItemTemplate>						
								<FooterTemplate>
								    <asp:TextBox CssClass="TextBox" runat="server" id="AccountAdd" Width="65%" 
                                        ontextchanged="AccountAdd_TextChanged" AutoPostBack="true" />
                                    <asp:Button class="btngo" placeholder="can't blank" runat="server" ID="btnAccAdd" Text="..." CommandName="SearchAdd" />                
                                    <%--<cc1:TextBoxWatermarkExtender ID="AccountAdd_TextBoxWatermarkExtender" 
                                        runat="server" Enabled="True" TargetControlID="AccountAdd" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                    </cc1:TextBoxWatermarkExtender>--%>
								</FooterTemplate>

<HeaderStyle Width="120px"></HeaderStyle>
							</asp:TemplateField>	
							
							<asp:TemplateField HeaderText="Account Name" HeaderStyle-Width="250" SortExpression="AccountName">
								<Itemtemplate>
									<asp:Label Runat="server" ID="AccountName" text='<%# DataBinder.Eval(Container.DataItem, "AccountName") %>'>
									</asp:Label>
								</Itemtemplate>	
								<EditItemTemplate>
								    <asp:Label Runat="server" ID="AccountNameEdit" text='<%# DataBinder.Eval(Container.DataItem, "AccountName") %>'>
									</asp:Label>
								</EditItemTemplate>	
								<FooterTemplate>
								    <asp:Label Runat="server" ID="AccountNameAdd">
									</asp:Label>
								</FooterTemplate>														

<HeaderStyle Width="250px"></HeaderStyle>
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Action" headerstyle-width="180" >
							    <ItemTemplate>
								    <asp:Button ID="btnEdit" runat="server" class="bitbtndt btnedit" Text="Edit" CommandName="Edit"/>
									<asp:Button ID="btndelete" runat="server" class="bitbtndt btndelete" Text="Delete" OnClientClick="return confirm('Sure to delete this data?');" CommandName="Delete"/>									
									<asp:Button ID="btnAssign" runat="server" class="bitbtndt btnedit" Text="User" CommandName="Detail" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" /> 
								</ItemTemplate>
								<EditItemTemplate>
									<asp:Button ID="btnUpdate" runat="server" class="bitbtndt btnsave" Text="Save" CommandName="Update"/>									
                                    <asp:Button ID="btnCancel" runat="server" class="bitbtndt btncancel" Text="Cancel" CommandName="Cancel"/>									
								</EditItemTemplate>									
								<FooterTemplate>
								   <asp:Button class="bitbtndt btnadd" runat="server" ID="btnAdd" Text="Add" CommandName="Insert"/>									
								</FooterTemplate>												

<HeaderStyle Width="180px"></HeaderStyle>
							</asp:TemplateField>							
    					</Columns>
        </asp:GridView>   
      <asp:Label ID="lstatus" ForeColor="red" runat="server"></asp:Label>        
    </div>
    </form>
</body>
</html>
