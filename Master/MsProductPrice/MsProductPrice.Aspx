<%@ Page Language="VB" AutoEventWireup="false" CodeFile="MsProductPrice.Aspx.vb" Inherits="Master_MsProductPrice_MsProductPrice" %>

<%@ Register Assembly="BasicFrame.WebControls.BasicDatePicker" Namespace="BasicFrame.WebControls"
    TagPrefix="BDP" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">


<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Product Price File</title>
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />   
    <script src="../../Function/Function.JS" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="Content">
    <div class="H1"><asp:Label runat="server" ID="lblTitle"></asp:Label></div>
    <hr style="color:Blue" />    
    
    <table>
        <tr>
            <td style="width:100px;text-align:right" >Quick Search :</td>
            <td><asp:TextBox runat="server" ID ="tbFilter" CssClass="TextBox"/> 
                <asp:DropDownList runat="server" ID="ddlField" CssClass="DropDownList" >
                  <asp:ListItem Selected="true" Text="Product Code" Value="A.ProductCode"></asp:ListItem>
                  <asp:ListItem Text="Product Name" Value="A.ProductName"></asp:ListItem>                            
                </asp:DropDownList>     
                <asp:Button class="bitbtn btnsearch" runat="server" ID="btnSearch" Text="Search" />
                <asp:Button ID="btnExpand" runat="server" class="btngo" Text="..."/>
                <asp:Button class="bitbtn btnexcel" runat="server" ID="btnExport" Text="Export"/>        
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
                  <asp:DropDownList ID="ddlField2" runat="server" CssClass="DropDownList">
                      <asp:ListItem Selected="true" Text="Procduct Code" Value="A.PrductCode"></asp:ListItem>
                      <asp:ListItem Text="Product Name" Value="A.ProductName"></asp:ListItem>                      
                </asp:DropDownList>
            </td>
        </tr>
     </table>
     </asp:Panel>
      <br />
      
    <table>
        <tr>
            <td>Date</td>
            <td>:</td>
            <td><BDP:BasicDatePicker ID="tbDate" runat="server" 
                       DateFormat="dd MMM yyyy" ButtonImageHeight="19px" ButtonImageWidth="20px" 
                       DisplayType="TextBoxAndImage" ReadOnly="true" 
                        TextBoxStyle-CssClass="TextDate" AutoPostBack="True">
                    <TextBoxStyle CssClass="TextDate" />
                    </BDP:BasicDatePicker>
            </td>
            <td>
            <asp:Button class="bitbtn btnapply" runat="server" ID="BtnApply" Text="Apply" />
            <%--<asp:ImageButton ID="BtnApply" runat="server"  
                    ImageUrl="../../Image/btnapplyon.png"
                    onmouseover="this.src='../../Image/btnapplyoff.png';"
                    onmouseout="this.src='../../Image/btnapplyon.png';"
                    ImageAlign="AbsBottom" />                --%>
            </td>
             <td style="width:100px;text-align:right" >Product Type :</td>
            <td><asp:DropDownList runat="server" ID="ddlCategory" CssClass="DropDownList" AutoPostBack= "true" >                            
             </asp:DropDownList> </td>
        </tr>
    </table> 
    <br />
    <asp:GridView id="DataGrid" runat="server" 
    ShowFooter="True" AllowSorting="True" 
            AutoGenerateColumns="False" AllowPaging="True"  CssClass="Grid">
						<HeaderStyle CssClass="GridHeader"></HeaderStyle>
						<RowStyle CssClass="GridItem" />
						<AlternatingRowStyle CssClass="GridAltItem"/>
						<PagerStyle CssClass="GridPager" />
				      <Columns>
				            <asp:TemplateField HeaderText="Product Code" >
								<Itemtemplate>
									<asp:Label Runat="server" ID="ProductCode" text='<%# DataBinder.Eval(Container.DataItem, "ProductCode") %>'/>
								</Itemtemplate>														
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Product Name" >
								<Itemtemplate>
									<asp:Label Runat="server" ID="ProductName" text='<%# DataBinder.Eval(Container.DataItem, "ProductName") %>'/>
								</Itemtemplate>																
							</asp:TemplateField>								
						
							<asp:TemplateField HeaderText="Last Date" >
								<Itemtemplate>
								    <asp:Label Runat="server" ID="tbLastDate" text='<%# DataBinder.Eval(Container.DataItem, "LastDate") %>'/>
								</Itemtemplate>																								
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Last Price" ItemStyle-HorizontalAlign = "Right">
								<Itemtemplate>
								<asp:Label Runat="server" ID="LastPrice" style="text-align: right"  text='<%# String.Format("{0:###,###.####}",Convert.ToDecimal(DataBinder.Eval(Container.DataItem, "Price"))) %>'/>
                                </Itemtemplate>									
							</asp:TemplateField>

							<asp:TemplateField HeaderText="Price" ItemStyle-HorizontalAlign = "Left" SortExpression="Price">
								<Itemtemplate>
								<asp:TextBox runat="server" CssClass="TextBox" style="text-align: right" ID="Price" Maxlength = "9" text='<%# String.Format("{0:###,###.####}",Convert.ToDecimal(DataBinder.Eval(Container.DataItem, "Price"))) %>'/>
								</Itemtemplate>		
							</asp:TemplateField>

							<asp:TemplateField HeaderText="Unit" ItemStyle-HorizontalAlign = "Right" SortExpression="Unit">
								<Itemtemplate>
									<asp:Label Runat="server" ID="Unit" text='<%# DataBinder.Eval(Container.DataItem, "Unit") %>'/>
								</Itemtemplate>	
							</asp:TemplateField>												
    					</Columns>
        </asp:GridView>               
    </div>
    <asp:GridView ID="GridExport" runat="server" AutoGenerateColumns="True" 
                Visible="true" ShowFooter="False" GridLines="None" Height="79px" PageSize="20" Width="724px">
               <HeaderStyle CssClass="GridHeader" />
               <RowStyle CssClass="GridItem" Wrap="false" />
               <AlternatingRowStyle CssClass="GridAltItem" />
               <PagerStyle CssClass="GridPager" />
           </asp:GridView>     
    <br />
    <asp:Label ID="lbstatus" ForeColor="red" runat="server"></asp:Label>
    </form>
</body>
</html>
