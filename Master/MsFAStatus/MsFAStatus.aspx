<%@ Page Language="VB" AutoEventWireup="false" CodeFile="MsFAStatus.aspx.vb" Inherits="Master_MsFAStatus_MsFAStatus" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>FA Status File</title>
    <script src="../../Function/OpenDlg.JS" type="text/javascript"></script>    
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
 <link href="https://fonts.googleapis.com/css2?family=Muli:wght@300;400;600&display=swap" rel="stylesheet">
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div class="Content">
     <div class="H1"><asp:Label runat="server" ID="lblTitle"></asp:Label></div>
     <hr style="color:Blue" />
      <table>
        <tr>
            <td style="width:100px;text-align:right" >Quick Search :</td>
            <td><asp:TextBox runat="server" ID ="tbFilter" CssClass="TextBox" /> 
                <asp:DropDownList runat="server" ID="ddlField" CssClass="DropDownList" >
                  <asp:ListItem Selected="true" Text="FA Status Code" Value="FAStatusCode"></asp:ListItem>
                  <asp:ListItem Text="FA Status Name" Value="FAStatusName"></asp:ListItem>        
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
            <td><asp:TextBox runat="server" ID ="tbfilter2" CssClass="TextBox" /> 
                  <asp:DropDownList runat="server" ID="ddlField2" CssClass="DropDownList" >
                    <asp:ListItem Selected="true" Text="FA Status Code" Value="FAStatusCode"></asp:ListItem>
                  <asp:ListItem Text="FA Status Name" Value="FAStatusName"></asp:ListItem>        
                  </asp:DropDownList>
                
            </td>
        </tr>
     </table>
     </asp:Panel>
      <br />
      
      <br />
      <asp:GridView id="DataGrid" runat="server" ShowFooter="True" AllowSorting="True" 
            AutoGenerateColumns="False" AllowPaging="True" CssClass="Grid">
						<HeaderStyle CssClass="GridHeader"></HeaderStyle>
						<RowStyle CssClass="GridItem" Wrap="false"/>
						<AlternatingRowStyle CssClass="GridAltItem"/>
						<FooterStyle CssClass="GridFooter" />
						<PagerStyle CssClass="GridPager" />
				      <Columns>
							<asp:TemplateField HeaderText="FA Status Code" HeaderStyle-Width="150" SortExpression="FAStatusCode">
								<Itemtemplate>
									<asp:Label Runat="server" ID="FAStatusCode" text='<%# DataBinder.Eval(Container.DataItem, "FAStatusCode") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:Label Runat="server" ID="FAStatusCodeEdit" Text='<%# DataBinder.Eval(Container.DataItem, "FAStatusCode") %>'>
									</asp:Label>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="FAStatusCodeAdd" Placeholder = "can't blank" CssClass="TextBox" MaxLength="1" Runat="Server" Width="100%"/>
									<%--<cc1:TextBoxWatermarkExtender ID="FAStatusCodeAdd_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="FAStatusCodeAdd" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>--%>
								</FooterTemplate>
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="FA Status Name" HeaderStyle-Width="350" SortExpression="FAStatusName">
								<Itemtemplate>
									<asp:Label Runat="server" ID="FAStatusName" text='<%# DataBinder.Eval(Container.DataItem, "FAStatusName") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:TextBox Runat="server" ID="FAStatusNameEdit" CssClass="TextBox" MaxLength="60" Width="100%" Text='<%# DataBinder.Eval(Container.DataItem, "FAStatusName") %>'>
									</asp:TextBox>
									<cc1:TextBoxWatermarkExtender ID="FAStatusNameEdit_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="FAStatusNameEdit" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="FAStatusNameAdd" Placeholder = "can't blank" MaxLength="60" CssClass="TextBox" Runat="Server" Width="100%"/>
									<%--<cc1:TextBoxWatermarkExtender ID="FAStatusNameAdd_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="FAStatusNameAdd" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>--%>
								</FooterTemplate>
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="For Blok" SortExpression="FgForblock">
								<Itemtemplate>
									<asp:Label Runat="server" ID="FgForblock" text='<%# DataBinder.Eval(Container.DataItem, "FgForblock") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:DropDownList CssClass="DropDownList" Runat="server" ID="FgForblockEdit" Width="100%" Text='<%# DataBinder.Eval(Container.DataItem, "FgForblock") %>'>
									    <asp:ListItem>N</asp:ListItem>
									    <asp:ListItem>Y</asp:ListItem>
									</asp:DropDownList>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:DropDownList CssClass="DropDownList" ID="FgForblockAdd" Runat="Server" Width="100%">
									    <asp:ListItem Selected="True">N</asp:ListItem>
									    <asp:ListItem >Y</asp:ListItem>
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
