<%@ Page Language="VB" AutoEventWireup="false" CodeFile="MsProductChangeLevel.aspx.vb" Inherits="Master_MsProductChangeLevel_MsProductChangeLevel" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Level Product</title>
    <script src="../../Function/Function.JS" type="text/javascript"></script>
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
            <td><asp:TextBox runat="server" ID ="tbFilter" CssClass="TextBox"/> 
                <asp:DropDownList runat="server" ID="ddlField" CssClass="DropDownList" >
                  <asp:ListItem Selected="true" Text="Product Code" Value="Product_Code"></asp:ListItem>
                  <asp:ListItem Text="Product Name" Value="Product_Name"></asp:ListItem>        
                  <asp:ListItem Value="Specification">Specification</asp:ListItem>
                  <asp:ListItem Value="ProductGrp_Name">Group Name</asp:ListItem>
                  <asp:ListItem Value="LevelProduct">Level Product</asp:ListItem>
                </asp:DropDownList>     
                <asp:Button class="bitbtn btnsearch" runat="server" ID="btnSearch" Text="Search" />
                <asp:Button class="btngo" runat="server" ID="btnExpand" Text="..."/>
                <%--<asp:Button class="bitbtn btnprint" runat="server" ID="btnPrint" Text="Print"/>--%>
            </td>
        </tr>
     </table>
     <asp:Panel runat="server" ID="pnlSearch" Visible="false">
     <table>   
        <tr>
            <td style="width:100px;text-align:right"><asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlNotasi" >
                    <asp:ListItem Selected="true" Text="OR" Value="OR"></asp:ListItem>
                </asp:DropDownList>
                    <asp:ListItem Text="AND" Value="AND"></asp:ListItem>         
            </td>
            <td><asp:TextBox runat="server" ID ="tbfilter2" CssClass="TextBox"/> 
                  <asp:DropDownList runat="server" ID="ddlField2" CssClass="DropDownList" >
                    <asp:ListItem Selected="true" Text="Product Code" Value="Product_Code"></asp:ListItem>
                    <asp:ListItem Text="Product Name" Value="Product_Name"></asp:ListItem>        
                    <asp:ListItem Value="Specification">Specification</asp:ListItem>
                    <asp:ListItem Value="ProductGrp_Name">Group Name</asp:ListItem>
                    <asp:ListItem Value="LevelProduct">Level Product</asp:ListItem>
                  </asp:DropDownList>
                
            </td>
        </tr>
     </table>
     </asp:Panel>
      <br />
      <%--<div style="border:0px  solid; width:100%; height:100%; overflow:auto;">--%>
      <asp:GridView id="DataGrid" runat="server" ShowFooter="True" AllowSorting="True" 
            AutoGenerateColumns="False" AllowPaging="True" CssClass="Grid">
						<HeaderStyle CssClass="GridHeader"></HeaderStyle>
						<RowStyle CssClass="GridItem" wrap="false" />
						<AlternatingRowStyle CssClass="GridAltItem"/>
						<FooterStyle CssClass="GridFooter" />
						<PagerStyle CssClass="GridPager" HorizontalAlign="Center" />
				      <Columns>
							<asp:TemplateField HeaderText="Product Code" HeaderStyle-Width="110" SortExpression="Product_Code">
								<Itemtemplate>
									<asp:Label Runat="server" ID="Product_Code" text='<%# DataBinder.Eval(Container.DataItem, "Product_Code") %>'>
									</asp:Label>
								</Itemtemplate>								
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Product Name" HeaderStyle-Width="320" SortExpression="Product_Name">
								<Itemtemplate>
									<asp:Label Runat="server" ID="Product_Name" text='<%# DataBinder.Eval(Container.DataItem, "Product_Name") %>'>
									</asp:Label>
								</Itemtemplate>								
							</asp:TemplateField>		
							
							<asp:TemplateField HeaderText="Specification" HeaderStyle-Width="250" SortExpression="Specification">
								<Itemtemplate>
									<asp:Label Runat="server" ID="Specification" text='<%# DataBinder.Eval(Container.DataItem, "Specification") %>'>
									</asp:Label>
								</Itemtemplate>								
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Group Name" HeaderStyle-Width="250" SortExpression="ProductGrp_Name">
								<Itemtemplate>
									<asp:Label Runat="server" ID="ProductGrp_Name" text='<%# DataBinder.Eval(Container.DataItem, "ProductGrp_Name") %>'>
									</asp:Label>
								</Itemtemplate>								
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Level Product" HeaderStyle-Width="50" SortExpression="ProductGrp_Name">
								<Itemtemplate>
									<asp:Label Runat="server" ID="LevelProduct" text='<%# DataBinder.Eval(Container.DataItem, "LevelProduct") %>'>
									</asp:Label>
								</Itemtemplate>								
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Change Level" headerstyle-width="80" >
							    <ItemTemplate>
							        <asp:TextBox Runat="server" ID="tbChangeLevel" CssClass="TextBox" MaxLength="2" Width="40%"/>
							        &nbsp;
								    <asp:Button class="btngo" runat="server" ID="BtnGo" Text="G" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" CommandName="Go"/>
								</ItemTemplate>								
							</asp:TemplateField>
							
    					</Columns>
        </asp:gridview>   
     <asp:Label ID="lstatus" ForeColor="red" runat="server"></asp:Label>        
    </div>
    </form>
</body>
</html>
