<%@ Page Language="VB" AutoEventWireup="false" CodeFile="MsRegional.aspx.vb" Inherits="MsRegional_MsRegional" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Regional File</title>
    <script src="../../Function/OpenDlg.JS" type="text/javascript"></script>            
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
 <link href="https://fonts.googleapis.com/css2?family=Muli:wght@300;400;600&display=swap" rel="stylesheet">
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div class="Content">
     <div class="H1">Regional File</div>
     <hr style="color:Blue" />
           <table>
        <tr>
            <td style="width:100px;text-align:right" >Quick Search :</td>
            <td><asp:TextBox runat="server" ID ="tbFilter" CssClass="TextBox" /> 
                <asp:DropDownList runat="server" ID="ddlField" CssClass="DropDownList" >
                  <asp:ListItem Selected="true" Text="Regional Code" Value="RegionalCode"></asp:ListItem>
                  <asp:ListItem Text="Regional Name" Value="RegionalName"></asp:ListItem>        
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
                    <asp:ListItem Selected="true" Text="Regional Code" Value="RegionalCode"></asp:ListItem>
                    <asp:ListItem Text="Regional Name" Value="RegionalName"></asp:ListItem> 
                  </asp:DropDownList>                
            </td>
        </tr>
     </table>
     </asp:Panel>
      <br />
      <asp:GridView id="DataGrid" runat="server" ShowFooter="True" AllowSorting="True" 
            AutoGenerateColumns="False" AllowPaging="True" CssClass="Grid">
						<HeaderStyle CssClass="GridHeader"></HeaderStyle>
						<RowStyle CssClass="GridItem" Wrap="false"/>
						<AlternatingRowStyle CssClass="GridAltItem"/>
						<FooterStyle CssClass="GridFooter" />
						<PagerStyle CssClass="GridPager" />
				      <Columns>
							<asp:TemplateField HeaderText="Regional Code" HeaderStyle-Width="120" SortExpression="RegionalCode">
								<Itemtemplate>
									<asp:Label Runat="server" ID="RegionalCode" text='<%# DataBinder.Eval(Container.DataItem, "RegionalCode") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:Label Runat="server" ID="RegionalCodeEdit" Text='<%# DataBinder.Eval(Container.DataItem, "RegionalCode") %>'>
									</asp:Label>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="RegionalCodeAdd" Placeholder = "can't blank" CssClass="TextBox" MaxLength="5" Runat="Server" Width="100%"/>
									<%--<cc1:TextBoxWatermarkExtender ID="RegionalCodeAdd_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="RegionalCodeAdd" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>--%>
								</FooterTemplate>
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Regional Name" HeaderStyle-Width="350" SortExpression="RegionalName">
								<Itemtemplate>
									<asp:Label Runat="server" ID="RegionalName" text='<%# DataBinder.Eval(Container.DataItem, "RegionalName") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:TextBox Runat="server" CssClass="TextBox" ID="RegionalNameEdit" MaxLength="60" Width="100%" Text='<%# DataBinder.Eval(Container.DataItem, "RegionalName") %>'>
									</asp:TextBox>
									<cc1:TextBoxWatermarkExtender ID="RegionalNameEdit_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="RegionalNameEdit" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="RegionalNameAdd" Placeholder = "can't blank" CssClass="TextBox" Runat="Server" MaxLength="60" Width="100%"/>
									<%--<cc1:TextBoxWatermarkExtender ID="RegionalNameAdd_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="RegionalNameAdd" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>--%>
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
