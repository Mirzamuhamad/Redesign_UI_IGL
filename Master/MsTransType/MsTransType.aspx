<%@ Page Language="VB" AutoEventWireup="false" CodeFile="MsTransType.aspx.vb" Inherits="Master_MsTransType_MsTransType" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Transaction Type File</title>
    <script src="../../Function/OpenDlg.JS" type="text/javascript"></script>
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div class="Content">
     <div class="H1">Transaction Type File</div>
     <hr style="color:Blue" />
      <table>
        <tr>
            <td style="width:100px;text-align:right" >Quick Search :</td>
            <td><asp:TextBox runat="server" ID ="tbFilter" CssClass="TextBox"/> 
                <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlField" >
                  <asp:ListItem Selected="true" Text="Trans Type Code" Value="TransTypeCode"></asp:ListItem>
                  <asp:ListItem Text="Trans Type Name" Value="TransTypeName"></asp:ListItem>        
                    <asp:ListItem Value="FgAccount">Account</asp:ListItem>
                    <asp:ListItem Value="FgProduct">Product</asp:ListItem>
                    <%--<asp:ListItem Value="FgStockType">Stock Type</asp:ListItem>--%>
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
            <td><asp:TextBox runat="server" CssClass="TextBox" ID ="tbfilter2"/> 
                  <asp:DropDownList runat="server" ID="ddlField2" CssClass="DropDownList" >
                    <asp:ListItem Selected="true" Text="Trans Type Code" Value="TransTypeCode"></asp:ListItem>
                    <asp:ListItem Text="Trans Type Name" Value="TransTypeName"></asp:ListItem> 
                      <asp:ListItem Value="FgAccount">Account</asp:ListItem>
                      <asp:ListItem Value="FgProduct">Product</asp:ListItem>
                      <%--<asp:ListItem Value="FgStockType">Stock Type</asp:ListItem>--%>
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
						<PagerStyle CssClass="GridPager" HorizontalAlign="Left" />
				      <Columns>
							<asp:TemplateField HeaderText="Trans Type Code" HeaderStyle-Width="110" SortExpression="TransTypeCode">
								<Itemtemplate>
									<asp:Label Runat="server" ID="TransTypeCode" MaxLength="5" text='<%# DataBinder.Eval(Container.DataItem, "TransTypeCode") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:Label Runat="server" ID="TransTypeCodeEdit" Width="100%" Text='<%# DataBinder.Eval(Container.DataItem, "TransTypeCode") %>'>
									</asp:Label>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="TransTypeCodeAdd" CssClass="TextBox" MaxLength="5" Runat="Server" Width="100%"/>
									<cc1:TextBoxWatermarkExtender ID="TransTypeCodeAdd_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="TransTypeCodeAdd" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>
								</FooterTemplate>

<HeaderStyle Width="110px"></HeaderStyle>
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Trans Type Name" HeaderStyle-Width="330" SortExpression="TransTypeName">
								<Itemtemplate>
									<asp:Label Runat="server" ID="TransTypeName" MaxLength="60" text='<%# DataBinder.Eval(Container.DataItem, "TransTypeName") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:TextBox Runat="server" ID="TransTypeNameEdit" Width="100%" CssClass="TextBox" MaxLength="60" Text='<%# DataBinder.Eval(Container.DataItem, "TransTypeName") %>'>
									</asp:TextBox>
									<cc1:TextBoxWatermarkExtender ID="TransTypeNameEdit_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="TransTypeNameEdit" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="TransTypeNameAdd" CssClass="TextBox" MaxLength="60" Runat="Server" Width="100%"/>
									<cc1:TextBoxWatermarkExtender ID="TransTypeNameAdd_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="TransTypeNameAdd" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>
								</FooterTemplate>

<HeaderStyle Width="330px"></HeaderStyle>
							</asp:TemplateField>		
							
							<asp:TemplateField HeaderText="Account" HeaderStyle-Width="30" SortExpression="FgAccount">
								<Itemtemplate>
									<asp:Label Runat="server" ID="FgAccount" text='<%# DataBinder.Eval(Container.DataItem, "FgAccount") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:DropDownList Runat="server" CssClass="DropDownList" ID="FgAccountEdit" Width="100%" Text='<%# DataBinder.Eval(Container.DataItem, "FgAccount") %>'>
									    <asp:ListItem>Y</asp:ListItem>
									    <asp:ListItem>N</asp:ListItem>
									</asp:DropDownList>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:DropDownList ID="FgAccountAdd" CssClass="DropDownList" Runat="Server" Width="100%">
									    <asp:ListItem>Y</asp:ListItem>
									    <asp:ListItem Selected="True">N</asp:ListItem>
									</asp:DropDownList>
								</FooterTemplate>

<HeaderStyle Width="30px"></HeaderStyle>
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Product" HeaderStyle-Width="30" SortExpression="FgProduct">
								<Itemtemplate>
									<asp:Label Runat="server" ID="FgProduct" text='<%# DataBinder.Eval(Container.DataItem, "FgProduct") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:DropDownList Runat="server" ID="FgProductEdit" CssClass="DropDownList" Width="100%" Text='<%# DataBinder.Eval(Container.DataItem, "FgProduct") %>'>
									    <asp:ListItem>Y</asp:ListItem>
									    <asp:ListItem>N</asp:ListItem>
									</asp:DropDownList>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:DropDownList ID="FgProductAdd" CssClass="DropDownList" Runat="Server" Width="100%">
									    <asp:ListItem>Y</asp:ListItem>
									    <asp:ListItem Selected="True">N</asp:ListItem>
									</asp:DropDownList>
								</FooterTemplate>
								<HeaderStyle Width="30px"></HeaderStyle>
							</asp:TemplateField>
							<%--
							<asp:TemplateField HeaderText="Stock Type" HeaderStyle-Width="50" SortExpression="FgStockType">
								<Itemtemplate>
									<asp:Label Runat="server" ID="FgStockType" text='<%# DataBinder.Eval(Container.DataItem, "FgStockType") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:DropDownList Runat="server" ID="FgStockTypeEdit" CssClass="DropDownList" Width="100%" Text='<%# DataBinder.Eval(Container.DataItem, "FgStockType") %>'>
									    <asp:ListItem>Y</asp:ListItem>
									    <asp:ListItem>N</asp:ListItem>
									</asp:DropDownList>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:DropDownList ID="FgStockTypeAdd" CssClass="DropDownList" Runat="Server" Width="100%">
									    <asp:ListItem>Y</asp:ListItem>
									    <asp:ListItem Selected="True">N</asp:ListItem>
									</asp:DropDownList>
								</FooterTemplate><HeaderStyle Width="30px"></HeaderStyle>
							</asp:TemplateField>--%>
							
							<asp:TemplateField HeaderText="Action" headerstyle-width="126" >
							    <ItemTemplate>
								    <asp:Button ID="btnEdit" runat="server" class="bitbtndt btnedit" Text="Edit" CommandName="Edit"/>									
                                    <asp:Button ID="btnAssign" runat="server" Width="70px" class="bitbtndt btndetail" Text="Account" CommandName="Assign" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>"/>									                                                      
                                    <%--<asp:Button ID="btnStockType" runat="server" Width="80px" class="bitbtndt btndetail" Text="Stock Type" CommandName="StockType" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>"/>									                                                                                                                          --%>
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
