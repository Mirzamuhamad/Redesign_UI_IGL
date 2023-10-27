<%@ Page Language="VB" AutoEventWireup="false" CodeFile="MsWrhsLocation.aspx.vb" Inherits="Master_MsWrhsLocation_MsWrhsLocation" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Warehouse Location File</title>
    <script src="../../Function/OpenDlg.JS" type="text/javascript"></script>
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div class="Content">
     <div class="H1">Warehouse Location File</div>
     <hr style="color:Blue" />
      <table>
        <tr>
            <td style="width:100px;text-align:right" >Quick Search :</td>
            <td><asp:TextBox runat="server" ID ="tbFilter" CssClass="TextBox" /> 
                <asp:DropDownList runat="server" ID="ddlField" CssClass="DropDownList" >
                  <asp:ListItem Selected="true" Text="Wrhs Location Code" Value="WLocationCode"></asp:ListItem>
                  <asp:ListItem Text="Wrhs Location Name" Value="WLocationName"></asp:ListItem>        
                    <asp:ListItem Value="FgAll">All</asp:ListItem>
                    <asp:ListItem Value="FgReject">Reject</asp:ListItem>
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
                    <asp:ListItem Selected="true" Text="Wrhs Location Code" Value="WLocationCode"></asp:ListItem>
                    <asp:ListItem Text="Wrhs Location Name" Value="WLocationName"></asp:ListItem> 
                      <asp:ListItem Value="FgAll">All</asp:ListItem>
                      <asp:ListItem Value="FgReject">Reject</asp:ListItem>
                  </asp:DropDownList>
                
            </td>
        </tr>
     </table>
     </asp:Panel>
      <br />
      <asp:GridView id="DataGrid" runat="server" PageSize = "10"
            ShowFooter="True" AllowSorting="True" 
            AutoGenerateColumns="False" AllowPaging="True" CssClass="Grid">
						<HeaderStyle CssClass="GridHeader"></HeaderStyle>
						<RowStyle CssClass="GridItem" Wrap="false"/>
						<AlternatingRowStyle CssClass="GridAltItem"/>						
						<FooterStyle CssClass="GridFooter" /> 
						<PagerStyle CssClass="GridPager" />						
				      <Columns>
							<asp:TemplateField HeaderText="Wrhs Location Code" SortExpression="WLocationCode">
								<Itemtemplate>
									<asp:Label Runat="server" ID="WLocationCode" text='<%# DataBinder.Eval(Container.DataItem, "WLocationCode") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:Label Runat="server" ID="WLocationCodeEdit" Text='<%# DataBinder.Eval(Container.DataItem, "WLocationCode") %>'>
									</asp:Label>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="WLocationCodeAdd" Placeholder = "can't blank" CssClass="TextBox" MaxLength="10" Runat="Server"/>
									<%--<cc1:TextBoxWatermarkExtender ID="WLocationCodeAdd_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="WLocationCodeAdd" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>--%>
								</FooterTemplate>
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Wrhs Location Name" HeaderStyle-Width="370" SortExpression="WLocationName">
								<Itemtemplate>
									<asp:Label Runat="server" ID="WLocationName" text='<%# DataBinder.Eval(Container.DataItem, "WLocationName") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:TextBox Runat="server" ID="WLocationNameEdit" Width="100%" CssClass="TextBox" Text='<%# DataBinder.Eval(Container.DataItem, "WLocationName") %>'>
									</asp:TextBox>
									<cc1:TextBoxWatermarkExtender ID="WLocationNameEdit_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="WLocationNameEdit" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="WLocationNameAdd" Placeholder = "can't blank" Width="100%" MaxLength="60" CssClass="TextBox" Runat="Server"/>
									<%--<cc1:TextBoxWatermarkExtender ID="WLocationNameAdd_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="WLocationNameAdd"
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>--%>
								</FooterTemplate>
							</asp:TemplateField>		
							
							<asp:TemplateField HeaderText="All" SortExpression="FgAll">
								<Itemtemplate>
									<asp:Label Runat="server" ID="FgAll" text='<%# DataBinder.Eval(Container.DataItem, "FgAll") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:DropDownList Runat="server" ID="FgAllEdit" CssClass="DropDownList" Width="100%" Text='<%# DataBinder.Eval(Container.DataItem, "FgAll") %>'>
									    <asp:ListItem>Y</asp:ListItem>
									    <asp:ListItem>N</asp:ListItem>
									</asp:DropDownList>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:DropDownList ID="FgAllAdd" Runat="Server" CssClass="DropDownList" >
									    <asp:ListItem>Y</asp:ListItem>
									    <asp:ListItem Selected="True">N</asp:ListItem>
									</asp:DropDownList>
								</FooterTemplate>
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Reject" SortExpression="FgReject">
								<Itemtemplate>
									<asp:Label Runat="server" ID="FgReject" text='<%# DataBinder.Eval(Container.DataItem, "FgReject") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:DropDownList Runat="server" ID="FgRejectEdit" CssClass="DropDownList" Width="100%" Text='<%# DataBinder.Eval(Container.DataItem, "FgReject") %>'>
									    <asp:ListItem>Y</asp:ListItem>
									    <asp:ListItem>N</asp:ListItem>
									</asp:DropDownList>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:DropDownList ID="FgRejectAdd" Runat="Server" CssClass="DropDownList" >
									    <asp:ListItem>Y</asp:ListItem>
									    <asp:ListItem Selected="True">N</asp:ListItem>
									</asp:DropDownList>
								</FooterTemplate>
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Action" headerstyle-width="126" >
								<ItemTemplate>
								    <asp:Button ID="btnEdit" runat="server" class="bitbtndt btnedit" Text="Edit" CommandName="Edit"/>
									<asp:Button ID="btndelete" runat="server" class="bitbtndt btndelete" Text="Delete" OnClientClick="return confirm('Sure to delete this data?');" CommandName="Delete"/>									
									<asp:Button ID="btnAssign" runat="server" class="bitbtndt btnedit" Text="Warehouse" CommandName="Assign" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" Width = "80" /> 									           						
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
