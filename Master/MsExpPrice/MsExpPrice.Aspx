<%@ Page Language="VB" AutoEventWireup="false" CodeFile="MsExpPrice.Aspx.vb" Inherits="Master_MsExpPrice_MsExpPrice" %>

<%@ Register Assembly="BasicFrame.WebControls.BasicDatePicker" Namespace="BasicFrame.WebControls"
    TagPrefix="BDP" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">


<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Expedition Price File</title>
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
 <link href="https://fonts.googleapis.com/css2?family=Muli:wght@300;400;600&display=swap" rel="stylesheet">   
    <script src="../../Function/Function.JS" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="Content">
    <div class="H1">Expedition Price File</div>
    <hr style="color:Blue" />    
    
    <table>
        <tr>
            <td style="width:100px;text-align:right" >Quick Search :</td>
            <td><asp:TextBox runat="server" ID ="tbFilter" CssClass="TextBox"/> 
                <asp:DropDownList runat="server" ID="ddlField" CssClass="DropDownList" >
                  <asp:ListItem Selected="true" Text="Customer Code" Value="A.Customer"></asp:ListItem>
                  <asp:ListItem Text="Customer Name" Value="A.CustomerName"></asp:ListItem>                            
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
                      <asp:ListItem Selected="true" Text="Customer Code" Value="A.Customer"></asp:ListItem>
                      <asp:ListItem Text="Customer Name" Value="A.CustomerName"></asp:ListItem>                      
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
        </tr>
    </table> 
    <br />
    <asp:GridView id="DataGrid" runat="server" 
            AutoGenerateColumns="False" CssClass="Grid">
						<HeaderStyle CssClass="GridHeader"></HeaderStyle>
						<RowStyle CssClass="GridItem" />
						<AlternatingRowStyle CssClass="GridAltItem"/>
						<PagerStyle CssClass="GridPager" />
				      <Columns>
				            <asp:TemplateField HeaderText="Customer Code">
								<Itemtemplate>
									<asp:Label Runat="server" ID="Customer" text='<%# DataBinder.Eval(Container.DataItem, "Customer") %>'/>
								</Itemtemplate>														
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Customer Name">
								<Itemtemplate>
									<asp:Label Runat="server" ID="CustomerName" text='<%# DataBinder.Eval(Container.DataItem, "CustomerName") %>'/>
								</Itemtemplate>																
							</asp:TemplateField>								
						
							<asp:TemplateField HeaderText="Last Date" >
								<Itemtemplate>
								    <asp:Label Runat="server" ID="tbLastDate" text='<%# DataBinder.Eval(Container.DataItem, "LastDate") %>'/>
								</Itemtemplate>																								
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Last Price Employee" ItemStyle-HorizontalAlign = "Right" >
								<Itemtemplate>
								<asp:Label Runat="server" ID="LastPriceEmp" style="text-align: right" text='<%# String.Format("{0:###,###.####}",Convert.ToDecimal(DataBinder.Eval(Container.DataItem, "PriceEmp"))) %>'/>
                                </Itemtemplate>									
							</asp:TemplateField>

							<asp:TemplateField HeaderText="Price Employee" ItemStyle-HorizontalAlign = "Right" SortExpression="PriceEmp">
								<Itemtemplate>
								<asp:TextBox runat="server" CssClass="TextBox" style="text-align: right" ID="PriceEmp" Maxlength = "9" text='<%# String.Format("{0:###,###.####}",Convert.ToDecimal(DataBinder.Eval(Container.DataItem, "PriceEmp"))) %>'/>
								</Itemtemplate>		
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Last Price Contractor" ItemStyle-HorizontalAlign = "Right" >
								<Itemtemplate>
								<asp:Label Runat="server" ID="LastPriceContractor" style="text-align: right" text='<%# String.Format("{0:###,###.####}",Convert.ToDecimal(DataBinder.Eval(Container.DataItem, "PriceContractor"))) %>'/>
                                </Itemtemplate>									
							</asp:TemplateField>

							<asp:TemplateField HeaderText="Price Contractor" ItemStyle-HorizontalAlign = "Right" SortExpression="PriceContractor">
								<Itemtemplate>
								<asp:TextBox runat="server" CssClass="TextBox" style="text-align: right" ID="PriceContractor" Maxlength = "9" text='<%# String.Format("{0:###,###.####}",Convert.ToDecimal(DataBinder.Eval(Container.DataItem, "PriceContractor"))) %>'/>
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
