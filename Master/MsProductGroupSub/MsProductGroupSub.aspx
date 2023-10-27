<%@ Page Language="VB" AutoEventWireup="false" CodeFile="MsProductGroupSub.aspx.vb" Inherits="MsProductGroupSub_MsProductGroupSub" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Product Sub Group File</title>
    <script src="../../Function/OpenDlg.JS" type="text/javascript"></script> 
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
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
                    <asp:ListItem Selected="true" Text="Sub Group Code" Value="ProductSubGrpCode"></asp:ListItem>
                    <asp:ListItem Text="Sub Group Name" Value="ProductSubGrpName"></asp:ListItem> 
                    <asp:ListItem Text="Group Name" Value="ProductGrpName"></asp:ListItem>                   
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
                    <asp:ListItem Selected="true" Text="Sub Group Code" Value="ProductSubGrpCode"></asp:ListItem>
                    <asp:ListItem Text="Sub Group Name" Value="ProductSubGrpName"></asp:ListItem> 
                    <asp:ListItem Text="Group Name" Value="ProductGrpName"></asp:ListItem>                              
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
							<asp:TemplateField HeaderText="Product Sub Group Code" HeaderStyle-Width="160" SortExpression="ProductSubGrpCode">
								<Itemtemplate>
									<asp:Label Runat="server" ID="ProductSubGrpCode" text='<%# DataBinder.Eval(Container.DataItem, "ProductSubGrpCode") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:Label Runat="server" ID="ProductSubGrpCodeEdit" Text='<%# DataBinder.Eval(Container.DataItem, "ProductSubGrpCode") %>'>
									</asp:Label>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="ProductSubGrpCodeAdd" Placeholder="can't blank" CssClass="TextBox" MaxLength="6" Runat="Server" Width="100%"/>
									<%--<cc1:TextBoxWatermarkExtender ID="ProductSubGrpCodeAdd_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="ProductSubGrpCodeAdd" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>--%>
								</FooterTemplate>
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Product Sub Group Name" HeaderStyle-Width="300" SortExpression="ProductSubGrpName">
								<Itemtemplate>
									<asp:Label Runat="server" ID="ProductSubGrpName" text='<%# DataBinder.Eval(Container.DataItem, "ProductSubGrpName") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:TextBox Runat="server" ID="ProductSubGrpNameEdit" MaxLength="60" CssClass="TextBox" Width="100%" Text='<%# DataBinder.Eval(Container.DataItem, "ProductSubGrpName") %>'>
									</asp:TextBox>
									<cc1:TextBoxWatermarkExtender ID="ProductSubGrpNameEdit_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="ProductSubGrpNameEdit" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="ProductSubGrpNameAdd" Placeholder="can't blank" Runat="Server" MaxLength="60" CssClass="TextBox" Width="100%"/>
									<%--<cc1:TextBoxWatermarkExtender ID="ProductSubGrpNameAdd_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="ProductSubGrpNameAdd" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>--%>
								</FooterTemplate>
							</asp:TemplateField>		
							
							<asp:TemplateField HeaderText="Product Group" HeaderStyle-Width="220" SortExpression="ProductGrpName">
								<Itemtemplate>
									<asp:Label Runat="server" ID="ProductGroup" text='<%# DataBinder.Eval(Container.DataItem, "ProductGrpName") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:DropDownList Runat="server" ID="ProductGroupEdit" CssClass="DropDownList" Width="100%" 
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "ProductGroup") %>' 
                                        DataSourceID="dsProductGroup" DataTextField="Product_Group_Name" 
                                        DataValueField="Product_Group_Code">									    
									</asp:DropDownList>								    
								</EditItemTemplate>
								<FooterTemplate>
									<asp:DropDownList ID="ProductGroupAdd" Runat="Server" Width="100%" CssClass="DropDownList"									    
                                        DataSourceID="dsProductGroup" DataTextField="Product_Group_Name" 
                                        DataValueField="Product_Group_Code">
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
        
        <asp:SqlDataSource ID="dsProductGroup" runat="server"            
           SelectCommand="EXEC S_GetProductGroup">
        </asp:SqlDataSource>
     <asp:Label ID="lstatus" ForeColor="red" runat="server"></asp:Label>        
    </div>
    </form>
</body>
</html>
