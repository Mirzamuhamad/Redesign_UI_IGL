<%@ Page Language="VB" AutoEventWireup="false" CodeFile="MsProductQC.aspx.vb" Inherits="Master_MsProductQC_MsProductQC" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitle</title>
    <script src="../../Function/Function.JS" type="text/javascript"></script>
    <script src="../../Function/OpenDlg.js" type="text/javascript"></script>
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
            <td><asp:TextBox runat="server" ID ="tbFilter" CssClass="TextBox"/> 
                <asp:DropDownList runat="server" ID="ddlField" CssClass="DropDownList" >
                  <asp:ListItem Selected="true" Value="ProductCode" Text="Product Code"></asp:ListItem>
                    <asp:ListItem Value="ProductName" Text="Product Name"></asp:ListItem>                    
                    <asp:ListItem Value="Specification" Text="Specification"></asp:ListItem>                    
                    <asp:ListItem Value="ProductExt" Text="Product Ext"></asp:ListItem>
                    <asp:ListItem Value="Product_Group_Code" Text="Product Group"></asp:ListItem>
                    <asp:ListItem Value="Product_Group_Name" Text="Product Group Name"></asp:ListItem>
                    <asp:ListItem Value="Product_SubGroup_Code" Text="Sub Group"></asp:ListItem>
                    <asp:ListItem Value="Product_SubGroup_Name" Text="Sub Group Name"></asp:ListItem>                    
                    <asp:ListItem Value="FgQC" Text="QC"></asp:ListItem>                    
                </asp:DropDownList>     
                <asp:Button class="bitbtn btnsearch" runat="server" ID="btnSearch" Text="Search" />
                <asp:Button class="btngo" runat="server" ID="btnExpand" Text="..."/>
                <%--<asp:Button class="bitbtn btnprint" runat="server" ID="btnPrint" Text="Print"/>--%>
                &nbsp &nbsp &nbsp &nbsp
                <asp:Label ID="lb1" runat="server" Text = "Setting QC in selected row : "> </asp:Label>
                <asp:DropDownList ID="ddlFgQC" runat="server" CssClass="DropDownList">
                    <asp:ListItem Selected="True" Value="Y" Text="Y"></asp:ListItem>                    
                    <asp:ListItem Value="N" Text="N"></asp:ListItem>                    
                </asp:DropDownList>     
                <asp:Button class="bitbtn btngo" runat="server" ID="btnApply" Text="G"/>
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
                    <asp:ListItem Selected="true" Value="ProductCode" Text="Product Code"></asp:ListItem>
                    <asp:ListItem Value="ProductName" Text="Product Name"></asp:ListItem>                    
                    <asp:ListItem Value="Specification" Text="Specification"></asp:ListItem>                    
                    <asp:ListItem Value="ProductExt" Text="Product Ext"></asp:ListItem>
                    <asp:ListItem Value="Product_Group_Code" Text="Product Group"></asp:ListItem>
                    <asp:ListItem Value="Product_Group_Name" Text="Product Group Name"></asp:ListItem>
                    <asp:ListItem Value="Product_SubGroup_Code" Text="Sub Group"></asp:ListItem>
                    <asp:ListItem Value="Product_SubGroup_Name" Text="Sub Group Name"></asp:ListItem>                    
                    <asp:ListItem Value="FgQC" Text="QC"></asp:ListItem>                    
                  </asp:DropDownList>                
            </td>
        </tr>
     </table>
     </asp:Panel>
      <br />
      <asp:GridView id="DataGrid" runat="server"
            ShowFooter="True" AllowSorting="True"
            AutoGenerateColumns="false" AllowPaging="True" CssClass="Grid">
						<HeaderStyle CssClass="GridHeader"></HeaderStyle>
						<RowStyle CssClass="GridItem" Wrap="true"/>
						<AlternatingRowStyle CssClass="GridAltItem"/>
						<FooterStyle CssClass="GridFooter" />
						<PagerStyle CssClass="GridPager" />
				      <Columns>
				          <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:CheckBox ID="cbSelectHd" runat="server" AutoPostBack="true" 
                                        oncheckedchanged="cbSelectHd_CheckedChanged1" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox ID="cbSelect" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>  
                                                      
							<asp:TemplateField HeaderText="Product Code" ItemStyle-Width="100" HeaderStyle-Width="100" SortExpression="ProductCode">
								<Itemtemplate>
									<asp:Label Runat="server" ID="ProductCode" text='<%# DataBinder.Eval(Container.DataItem, "ProductCode") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:Label Runat="server" ID="ProductCodeEdit" Text='<%# DataBinder.Eval(Container.DataItem, "ProductCode") %>'>
									</asp:Label>
								</EditItemTemplate>								
                                <%--<HeaderStyle Width="100px"></HeaderStyle>--%>
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Product Name" ItemStyle-Width="320" HeaderStyle-Width="320" SortExpression="ProductName">
								<Itemtemplate>
									<asp:Label Runat="server" ID="ProductName" text='<%# DataBinder.Eval(Container.DataItem, "ProductName") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:Label Runat="server" CssClass="TextBox" ID="ProductNameEdit" MaxLength="60" Width="100%" Text='<%# DataBinder.Eval(Container.DataItem, "ProductName") %>'>
									</asp:Label>									
								</EditItemTemplate>
                                <%--<HeaderStyle Width="320px"></HeaderStyle>--%>
							</asp:TemplateField>	
							
							<asp:TemplateField HeaderText="Specification" ItemStyle-Wrap="true" ItemStyle-Width="320" HeaderStyle-Width="320" SortExpression="Specification">
								<Itemtemplate>
									<asp:Label Runat="server" ID="Specification" text='<%# DataBinder.Eval(Container.DataItem, "Specification") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:Label Runat="server" CssClass="TextBox" ID="SpecificationEdit" MaxLength="255" Width="50%" Text='<%# DataBinder.Eval(Container.DataItem, "Specification") %>'>
									</asp:Label>									
								</EditItemTemplate>
                                <%--<HeaderStyle Width="320px"></HeaderStyle>--%>
                                <ItemStyle Width="320" Wrap="true" />
							</asp:TemplateField>	
														
							<asp:TemplateField HeaderText="Product Ext" ItemStyle-Width="100" HeaderStyle-Width="100" SortExpression="ProductExt">
								<Itemtemplate>
									<asp:Label Runat="server" ID="ProductExt" text='<%# DataBinder.Eval(Container.DataItem, "ProductExt") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:Label Runat="server" ID="ProductExtEdit" Text='<%# DataBinder.Eval(Container.DataItem, "ProductExt") %>'>
									</asp:Label>
								</EditItemTemplate>								
                                <%--<HeaderStyle Width="100px"></HeaderStyle>--%>
							</asp:TemplateField>	
													
							<asp:TemplateField HeaderText="Product Sub Group" ItemStyle-Width="150" HeaderStyle-Width="150" SortExpression="Product_SubGroup_Name">
								<Itemtemplate>
									<asp:Label Runat="server" ID="Product_SubGroup_Name" text='<%# DataBinder.Eval(Container.DataItem, "Product_SubGroup_Name") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:Label Runat="server" ID="Product_SubGroup_NameEdit" Text='<%# DataBinder.Eval(Container.DataItem, "Product_SubGroup_Name") %>'>
									</asp:Label>
								</EditItemTemplate>								
                                <%--<HeaderStyle Width="150px"></HeaderStyle>--%>
							</asp:TemplateField>	
													
							<asp:TemplateField HeaderText="QC" SortExpression="FgQC">
								<Itemtemplate>
									<asp:Label Runat="server" ID="FgQC" text='<%# DataBinder.Eval(Container.DataItem, "FgQC") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:DropDownList CssClass="DropDownList" Runat="server" Width="100%" ID="FgQCEdit" Text='<%# DataBinder.Eval(Container.DataItem, "FgQC") %>'>
									    <asp:ListItem>Y</asp:ListItem>
									    <asp:ListItem>N</asp:ListItem>
									</asp:DropDownList>
								</EditItemTemplate>								
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Action" ItemStyle-Width="126" headerstyle-width="126" >
								<ItemTemplate>
								    <asp:Button ID="btnEdit" runat="server" class="bitbtndt btnedit" Text="Edit" CommandName="Edit"/>									
								</ItemTemplate>
								<EditItemTemplate>
									<asp:Button ID="btnUpdate" runat="server" class="bitbtndt btnsave" Text="Save" CommandName="Update"/>									
                                    <asp:Button ID="btnCancel" runat="server" class="bitbtndt btncancel" Text="Cancel" CommandName="Cancel"/>									
								</EditItemTemplate>															
                                <%--<HeaderStyle Width="126px"></HeaderStyle>--%>
							</asp:TemplateField>
							
    					</Columns>
        </asp:GridView>
        <asp:SqlDataSource ID="dsProductType" runat="server" SelectCommand="EXEC S_GetProductType">
        </asp:SqlDataSource>

     <asp:Label ID="lstatus" ForeColor="red" runat="server"></asp:Label>        
    </div>
    </form>
</body>
</html>
