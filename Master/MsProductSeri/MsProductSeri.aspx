<%@ Page Language="VB" AutoEventWireup="false" CodeFile="MsProductSeri.aspx.vb" Inherits="MsProductSeri_MsProductSeri" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Product Size File</title>
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
                    <asp:ListItem Selected="true" Text="Product Jenis" Value="ProductJenis"></asp:ListItem>
                    <asp:ListItem Text="Product Type" Value="ProductType"></asp:ListItem> 
                    <asp:ListItem Text="Jenis Name" Value="JenisName"></asp:ListItem>
                    <asp:ListItem Text="Seri Name" Value="SeriName"></asp:ListItem>                    
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
            <td><asp:TextBox runat="server" ID = "tbfilter2" CssClass="TextBox"/> 
                  <asp:DropDownList runat="server" ID="ddlField2" CssClass="DropDownList" >
                    <asp:ListItem Selected="true" Text="Product Jenis" Value="ProductJenis"></asp:ListItem>
                    <asp:ListItem Text="Product Type" Value="ProductType"></asp:ListItem> 
                    <asp:ListItem Text="Jenis Name" Value="JenisName"></asp:ListItem>
                    <asp:ListItem Text="Seri Name" Value="SeriName"></asp:ListItem>                              
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
				      
				            <asp:TemplateField HeaderText="Product Jenis" HeaderStyle-Width="200" SortExpression="ProductJenis">
								<Itemtemplate>
								    <asp:Label Runat="server" ID="ProductJenis" text='<%# DataBinder.Eval(Container.DataItem, "ProductJenis") %>'>
									</asp:Label>    
									<asp:Label Runat="server" ID="JenisName" text='<%# DataBinder.Eval(Container.DataItem, "JenisName") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:Label Runat="server" ID="ProductJenisEdit" Text='<%# DataBinder.Eval(Container.DataItem, "ProductJenis") %>'>
									</asp:Label>
									<asp:Label Runat="server" ID="JenisName" Text='<%# DataBinder.Eval(Container.DataItem, "JenisName") %>'>
									</asp:Label>
									<%--<asp:DropDownList ID="ProductJenisEdit" Runat="Server" Width="100%" CssClass="DropDownList"
									    Text='<%# DataBinder.Eval(Container.DataItem, "ProductJenis") %>'									    
                                        DataSourceID="dsProductJenis" DataTextField="Product_Jenis_Name" 
                                        DataValueField="Product_Jenis_Code">
									</asp:DropDownList>--%>								    
								</EditItemTemplate>
								<FooterTemplate>
									<asp:DropDownList ID="ProductJenisAdd" Runat="Server" Width="100%" CssClass="DropDownList"									    
                                        DataSourceID="dsProductJenis" DataTextField="Product_Jenis_Name" 
                                        DataValueField="Product_Jenis_Code">
									</asp:DropDownList>
								</FooterTemplate>
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Product Type" HeaderStyle-Width="200" SortExpression="ProductType">
								<Itemtemplate>
									<asp:Label Runat="server" ID="ProductType" text='<%# DataBinder.Eval(Container.DataItem, "TypeName") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<%--<asp:Label Runat="server" ID="ProductTypeEdit" Text='<%# DataBinder.Eval(Container.DataItem, "ProductType") %>'>
									</asp:Label>--%>	
									<asp:DropDownList ID="ProductTypeEdit" Runat="Server" Width="100%" CssClass="DropDownList"
									    Text='<%# DataBinder.Eval(Container.DataItem, "ProductType") %>'									    
                                        DataSourceID="dsProductJenisNo" DataTextField="Product_Type_Name" 
                                        DataValueField="Product_Type_No">
									</asp:DropDownList>							    
								</EditItemTemplate>
								<FooterTemplate>
									<asp:DropDownList ID="ProductTypeAdd" Runat="Server" Width="100%" CssClass="DropDownList"									    
                                        DataSourceID="dsProductJenisNo" DataTextField="Product_Type_Name" 
                                        DataValueField="Product_Type_No">
									</asp:DropDownList>
								</FooterTemplate>
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Product Seri" HeaderStyle-Width="160" SortExpression="ProductSeri">
								<Itemtemplate>
									<asp:Label Runat="server" ID="ProductSeri" text='<%# DataBinder.Eval(Container.DataItem, "ProductSeri") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:TextBox Runat="server" ID="ProductSeriEdit" Text='<%# DataBinder.Eval(Container.DataItem, "ProductSeri") %>' />
									<cc1:TextBoxWatermarkExtender ID="ProductSeriEdit_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="ProductSeriEdit" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>									
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="ProductSeriAdd" CssClass="TextBox" MaxLength="2" Runat="Server" Width="100%"/>
									<cc1:TextBoxWatermarkExtender ID="ProductSeriAdd_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="ProductSeriAdd" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>
								</FooterTemplate>
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Product Seri Name" HeaderStyle-Width="160" SortExpression="SeriName">
								<Itemtemplate>
									<asp:Label Runat="server" ID="SeriName" text='<%# DataBinder.Eval(Container.DataItem, "SeriName") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:TextBox Runat="server" ID="SeriNameEdit"  MaxLength="60" CssClass="TextBox" Width="100%" Text='<%# DataBinder.Eval(Container.DataItem, "SeriName") %>'>
									</asp:TextBox>
									<cc1:TextBoxWatermarkExtender ID="SeriNameEdit_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="SeriNameEdit" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="SeriNameAdd" Runat="Server" MaxLength="60" CssClass="TextBox" Width="100%"/>
									<cc1:TextBoxWatermarkExtender ID="SeriNameAdd_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="SeriNameAdd" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>
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
        
        <asp:SqlDataSource ID="dsProductJenis" runat="server"            
           SelectCommand="EXEC S_GetProductJenis">
        </asp:SqlDataSource>
        
        <asp:SqlDataSource ID="dsProductJenisNo" runat="server"            
           SelectCommand="EXEC S_GetProductTypeSeri">
        </asp:SqlDataSource>
     <asp:Label ID="lstatus" ForeColor="red" runat="server"></asp:Label>        
    </div>
    </form>
</body>
</html>
