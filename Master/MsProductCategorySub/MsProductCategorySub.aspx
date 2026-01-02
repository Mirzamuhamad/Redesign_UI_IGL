<%@ Page Language="VB" AutoEventWireup="false" CodeFile="MsProductCategorySub.aspx.vb" Inherits="MsProductCategorySub_MsProductCategorySub" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Product Sub Category File</title>
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
                    <asp:ListItem Selected="true" Text="Sub Category Code" Value="ProductSubCatCode"></asp:ListItem>
                    <asp:ListItem Text="Sub Category Name" Value="ProductSubCatName"></asp:ListItem>                   
                    <asp:ListItem Text="Category Name" Value="ProductCatName"></asp:ListItem>
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
                    <asp:ListItem Selected="true" Text="Sub Category Code" Value="ProductSubCatCode"></asp:ListItem>
                    <asp:ListItem Text="Sub Category Name" Value="ProductSubCatName"></asp:ListItem>
                    <asp:ListItem Text="Category Name" Value="ProductCatName"></asp:ListItem> 
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
							<asp:TemplateField HeaderText="Sub Category Code" HeaderStyle-Width="140" SortExpression="ProductSubCatCode">
								<Itemtemplate>
									<asp:Label Runat="server" ID="ProductSubCatCode" text='<%# DataBinder.Eval(Container.DataItem, "ProductSubCatCode") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:Label Runat="server" ID="ProductSubCatCodeEdit" Text='<%# DataBinder.Eval(Container.DataItem, "ProductSubCatCode") %>'>
									</asp:Label>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="ProductSubCatCodeAdd" CssClass="TextBox" MaxLength="10" Runat="Server" Width="100%"/>
									<cc1:TextBoxWatermarkExtender ID="ProductSubCatCodeAdd_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="ProductSubCatCodeAdd" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>
								</FooterTemplate>
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Sub Category Name" HeaderStyle-Width="300" SortExpression="ProductSubCatName">
								<Itemtemplate>
									<asp:Label Runat="server" ID="ProductSubCatName" text='<%# DataBinder.Eval(Container.DataItem, "ProductSubCatName") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:TextBox Runat="server" ID="ProductSubCatNameEdit" MaxLength="60" CssClass="TextBox" Width="100%" Text='<%# DataBinder.Eval(Container.DataItem, "ProductSubCatName") %>'>
									</asp:TextBox>
									<cc1:TextBoxWatermarkExtender ID="ProductSubCatNameEdit_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="ProductSubCatNameEdit" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="ProductSubCatNameAdd" Runat="Server" MaxLength="60" CssClass="TextBox" Width="100%"/>
									<cc1:TextBoxWatermarkExtender ID="ProductSubCatNameAdd_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="ProductSubCatNameAdd" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>
								</FooterTemplate>
							</asp:TemplateField>		
							
							<asp:TemplateField HeaderText="Product Category" HeaderStyle-Width="220" SortExpression="ProductCatName">
								<Itemtemplate>
									<asp:Label Runat="server" ID="ProductCategory" text='<%# DataBinder.Eval(Container.DataItem, "ProductCatName") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:DropDownList Runat="server" ID="ProductCategoryEdit" CssClass="DropDownList" Width="100%" 
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "ProductCategory") %>' 
                                        DataSourceID="dsProductCategory" DataTextField="ProductCatName" 
                                        DataValueField="ProductCatCode">									    
									</asp:DropDownList>								    
								</EditItemTemplate>
								<FooterTemplate>
									<asp:DropDownList ID="ProductCategoryAdd" Runat="Server" Width="100%" CssClass="DropDownList"									    
                                        DataSourceID="dsProductCategory" DataTextField="ProductCatName" 
                                        DataValueField="ProductCatCode">
									</asp:DropDownList>
								</FooterTemplate>
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Action" headerstyle-width="126" >
								<ItemTemplate>
								    <asp:Button ID="btnEdit" runat="server" class="bitbtndt btnedit" Text="Edit" CommandName="Edit"/>
									<asp:Button ID="btndelete" runat="server" class="bitbtndt btndelete" Text="Delete" OnClientClick="return confirm('Sure to delete this data?');" CommandName="Delete"/>									
									<asp:Button ID="btnDetail" runat="server" class="bitbtndt btndetail" Text="Detail" CommandName="Detail" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>"/>									
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
        
        <asp:SqlDataSource ID="dsProductCategory" runat="server"            
           SelectCommand="SELECT ProductCatCode, ProductCatName FROM VMsProductCategory">
        </asp:SqlDataSource>
     <asp:Label ID="lstatus" ForeColor="red" runat="server"></asp:Label>        
    </div>
    </form>
</body>
</html>
