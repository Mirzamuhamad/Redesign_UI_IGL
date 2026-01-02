<%@ Page Language="VB" AutoEventWireup="false" CodeFile="MsProductCost.aspx.vb" Inherits="MsMachineLoadFactorOH_MsMachineLoadFactorOH" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Product Cost Price File</title>
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
            <td><asp:TextBox runat="server" ID ="tbFilter" CssClass="TextBox" /> 
                <asp:DropDownList runat="server" ID="ddlField" CssClass="DropDownList" >
                  <asp:ListItem Selected="true" Text="Product Code" Value="Product_Code"></asp:ListItem>
                  <asp:ListItem Text="Product Name" Value = "Product_Name"></asp:ListItem>        
               <%--   <asp:ListItem Text="Price Cost" Value = "PriceCost"></asp:ListItem>   --%>                       
                </asp:DropDownList>     
                <asp:Button class="bitbtn btnsearch" runat="server" ID="btnSearch" Text="Search" />
                <asp:Button class="btngo" runat="server" ID="btnExpand" Text="..."/>
                <asp:Button class="bitbtn btnprint" runat="server" ID="btnPrint" Text="Print"/>
                &nbsp &nbsp &nbsp &nbsp&nbsp;
            </td>
        </tr>
        </table>
        </br>
        
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
                   <asp:ListItem Selected="true" Text="Product Code" Value="Product_Code"></asp:ListItem>
                  <asp:ListItem Text="Product Name" Value = "Product_Name"></asp:ListItem>        
                <%--  <asp:ListItem Text="Price Cost" Value = "PriceCost"></asp:ListItem>  --%>   
                  </asp:DropDownList>                
            </td>
        </tr>
     </table>
     </asp:Panel>
      
      <table bgcolor=silver >
          <tr>
                <td><b><asp:Label ID="lb1" runat="server" Text = "Setting in selected row " /></b> </td>
          </tr>
          <tr>
                <td><asp:Label ID="Label1" runat="server" Text = "Price Cost :" />                 
                <asp:TextBox ID="tbPrice" runat="server" CssClass="TextBox" MaxLength="15" 
                        Width="50px" />
                &nbsp 
                <asp:Button class="bitbtn btngo" runat="server" ID="btnApply" Text="G"/></td>
           </tr>           
           
          
        </table>
        <br />
      <asp:GridView id="DataGrid" runat="server" ShowFooter="True" AllowSorting="True" 
            AutoGenerateColumns="False" AllowPaging="True" CssClass="Grid">
						<HeaderStyle CssClass="GridHeader"></HeaderStyle>
						<RowStyle CssClass="GridItem" Wrap="false"/>
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
				      
							<asp:TemplateField HeaderText="Product Code" HeaderStyle-Width="100" SortExpression="Product_Code">
								<Itemtemplate>
									<asp:Label Runat="server" ID="Product" text='<%# DataBinder.Eval(Container.DataItem, "Product_Code") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:Label Runat="server" ID="ProductEdit"  Text='<%# DataBinder.Eval(Container.DataItem, "Product_Code") %>'>
									</asp:Label>
								</EditItemTemplate>

<HeaderStyle Width="100px"></HeaderStyle>
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Product Name" HeaderStyle-Width="200" SortExpression="Product_Name">
								<Itemtemplate>
									<asp:Label Runat="server" ID="ProductName" text='<%# DataBinder.Eval(Container.DataItem, "Product_Name") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:Label Runat="server" ID="ProductNameEdit" text='<%# DataBinder.Eval(Container.DataItem, "Product_Name") %>'>
									</asp:Label>
								</EditItemTemplate>

<HeaderStyle Width="200px"></HeaderStyle>
							</asp:TemplateField>		
							
							<asp:TemplateField HeaderText="Price Cost" HeaderStyle-Width="100" 
                                SortExpression="PriceCost" ItemStyle-HorizontalAlign = "Right">
								<Itemtemplate>
									<asp:Label Runat="server" ID="PriceCost" text='<%# String.Format("{0:###,##0.00}",Convert.ToDecimal(DataBinder.Eval(Container.DataItem, "PriceCost"))) %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:TextBox Runat="server" CssClass="TextBox" ID="PriceCostEdit" MaxLength="9" Width="80" text='<%# String.Format("{0:###,##0.00}",Convert.ToDecimal(DataBinder.Eval(Container.DataItem, "PriceCost"))) %>'>
									</asp:TextBox>									
								</EditItemTemplate>

                    <HeaderStyle Width="100px"></HeaderStyle>

            <ItemStyle HorizontalAlign="Right"></ItemStyle>
							</asp:TemplateField>
													
							
    					</Columns>
        </asp:GridView>
        
             
     <asp:Label ID="lstatus" ForeColor="red" runat="server"></asp:Label>
    </div>
    </form>
</body>
</html>
