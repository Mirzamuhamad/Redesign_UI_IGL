<%@ Page Language="VB" AutoEventWireup="false" CodeFile="MsWrhsGroup.aspx.vb" Inherits="Master_MsWrhsGroup_MsWrhsGroup" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Warehouse Group File</title>
    <script src="../../Function/OpenDlg.JS" type="text/javascript"></script>    
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div class="Content">
     <div class="H1">Warehouse Group File</div>
     <hr style="color:Blue" />
      <table>
        <tr>
            <td style="width:100px;text-align:right" >Quick Search :</td>
            <td><asp:TextBox runat="server" ID ="tbFilter" CssClass="TextBox"/> 
                <asp:DropDownList runat="server" ID="ddlField" CssClass="DropDownList" >
                  <asp:ListItem Selected="true" Text="Warehouse Group Code" Value="WrhsGroupCode"></asp:ListItem>
                  <asp:ListItem Text="Warehouse Group Name" Value="WrhsGroupName"></asp:ListItem>        
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
            <td style="width:100px;text-align:right"><asp:DropDownList runat="server" CssClass="DropDownList" ID="ddlNotasi" >
                    <asp:ListItem Selected="true" Text="OR" Value="OR"></asp:ListItem>
                    <asp:ListItem Text="AND" Value="AND"></asp:ListItem>         
                </asp:DropDownList>
            </td>
            <td><asp:TextBox runat="server" ID ="tbfilter2" CssClass="TextBox"/> 
                  <asp:DropDownList runat="server" ID="ddlField2" CssClass="DropDownList" >
                    <asp:ListItem Selected="true" Text="Warehouse Group Code" Value="WrhsGroupCode"></asp:ListItem>
                    <asp:ListItem Text="Warehouse Group Name" Value="WrhsGroupName"></asp:ListItem> 
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
							<asp:TemplateField HeaderText="Warehouse Group Code" HeaderStyle-Width="150" SortExpression="WrhsGroupCode">
								<Itemtemplate>
									<asp:Label Runat="server" ID="WrhsGroupCode" text='<%# DataBinder.Eval(Container.DataItem, "WrhsGroupCode") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:Label Runat="server" ID="WrhsGroupCodeEdit" Text='<%# DataBinder.Eval(Container.DataItem, "WrhsGroupCode") %>'>
									</asp:Label>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="WrhsGroupCodeAdd" CssClass="TextBox" MaxLength="3" Runat="Server" Width="100%"/>
									<cc1:TextBoxWatermarkExtender ID="WrhsGroupCodeAdd_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="WrhsGroupCodeAdd" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>
								</FooterTemplate>
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Warehouse Group Name" HeaderStyle-Width="350" SortExpression="WrhsGroupName">
								<Itemtemplate>
									<asp:Label Runat="server" ID="WrhsGroupName" text='<%# DataBinder.Eval(Container.DataItem, "WrhsGroupName") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:TextBox Runat="server" ID="WrhsGroupNameEdit" CssClass="TextBox" MaxLength="60" Width="100%" Text='<%# DataBinder.Eval(Container.DataItem, "WrhsGroupName") %>'>
									</asp:TextBox>
									<cc1:TextBoxWatermarkExtender ID="WrhsGroupNameEdit_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="WrhsGroupNameEdit" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="WrhsGroupNameAdd" CssClass="TextBox" MaxLength="60" Runat="Server" Width="100%"/>
									<cc1:TextBoxWatermarkExtender ID="WrhsGroupNameAdd_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="WrhsGroupNameAdd" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>
								</FooterTemplate>
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Rental" HeaderStyle-Width="30" SortExpression="FgRental">
								<Itemtemplate>
									<asp:Label Runat="server" ID="ddlRental" text='<%# DataBinder.Eval(Container.DataItem, "FgRental") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
								        <asp:DropDownList Enabled="False" runat="server" Width="100%" CssClass="DropDownList" ID="ddlRentalEdit" Text='<%# DataBinder.Eval(Container.DataItem, "FgRental") %>' >
                                            <asp:ListItem Selected="True">Y</asp:ListItem>
                                            <asp:ListItem>N</asp:ListItem>
                                        </asp:DropDownList>
									<%--<asp:TextBox Runat="server" ID="WrhsGroupNameEdit" CssClass="TextBox" MaxLength="60" Width="100%" Text='<%# DataBinder.Eval(Container.DataItem, "FgRental") %>'>
									</asp:TextBox>
									<cc1:TextBoxWatermarkExtender ID="WrhsGroupNameEdit_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="WrhsGroupNameEdit" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>--%>
								</EditItemTemplate>
								<FooterTemplate>
								
								<asp:DropDownList runat="server" Width="100%" CssClass="DropDownList" ID="ddlRentalAdd">
                                    <asp:ListItem Selected="True">Y</asp:ListItem>
                                    <asp:ListItem>N</asp:ListItem>
                                </asp:DropDownList>
									<%--<asp:TextBox ID="WrhsGroupNameAdd" CssClass="TextBox" MaxLength="60" Runat="Server" Width="100%"/>
									<cc1:TextBoxWatermarkExtender ID="WrhsGroupNameAdd_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="WrhsGroupNameAdd" 
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
