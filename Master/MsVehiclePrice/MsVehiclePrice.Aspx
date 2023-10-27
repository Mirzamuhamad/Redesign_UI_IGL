<%@ Page Language="VB" AutoEventWireup="false" CodeFile="MsVehiclePrice.Aspx.vb" Inherits="Master_MsVehiclePrice_MsVehiclePrice" %>

<%@ Register Assembly="BasicFrame.WebControls.BasicDatePicker" Namespace="BasicFrame.WebControls"
    TagPrefix="BDP" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">


<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Job Price File</title>
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />   
    <script src="../../Function/Function.JS" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="Content">
    <div class="H1">Vehicle Price</div>
    <hr style="color:Blue" />    
    
    <table>
        <tr>
            <td style="width:100px;text-align:right" >Quick Search :</td>
            <td><asp:TextBox runat="server" ID ="tbFilter" CssClass="TextBox"/> 
                <asp:DropDownList runat="server" ID="ddlField" CssClass="DropDownList" >
                  <asp:ListItem Selected="true" Text="Car Code" Value="A.CarNo"></asp:ListItem>
                  <asp:ListItem Text="Car Name" Value="A.CarName"></asp:ListItem>
                  <%--<asp:ListItem Text="Unit" Value="A.Unit"></asp:ListItem>  --%>                           
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
                      <asp:ListItem Selected="true" Text="Car Code" Value="CarNo"></asp:ListItem>
                  <asp:ListItem Text="Car Name" Value="CarName"></asp:ListItem>
                  <%--<asp:ListItem Text="Unit" Value="Unit"></asp:ListItem>--%>                      
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
            <td style="width:100px;text-align:right" >Merk :</td>
            <td><asp:DropDownList runat="server" ID="ddlCategory" CssClass="DropDownList" AutoPostBack= "true">                            
             </asp:DropDownList> </td>
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
				            <asp:TemplateField HeaderText="Car Code">
								<Itemtemplate>
									<asp:Label Runat="server" ID="CarCode" text='<%# DataBinder.Eval(Container.DataItem, "CarNo") %>'/>
								</Itemtemplate>														
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Car Name">
								<Itemtemplate>
									<asp:Label Runat="server" ID="CarName" text='<%# DataBinder.Eval(Container.DataItem, "CarName") %>'/>
								</Itemtemplate>																
							</asp:TemplateField>
							
							<%--<asp:TemplateField HeaderText="Merk">
								<Itemtemplate>
									<asp:Label Runat="server" ID="Merk" text='<%# DataBinder.Eval(Container.DataItem, "Merk") %>'/>
								</Itemtemplate>																
							</asp:TemplateField>								
						--%>
							<%--<asp:TemplateField HeaderText="Start Date" >
								<Itemtemplate>
								    <asp:Label Runat="server" ID="StartDate" text='<%# DataBinder.Eval(Container.DataItem, "StartDate") %>'/>
								</Itemtemplate>																								
							</asp:TemplateField>--%>
							
							<asp:BoundField 
                            DataField="LastDate" HtmlEncode="true" DataFormatString="{0:dd-MMM-yy}" 
                            HeaderText="Start Date" SortExpression="StartDate" HeaderStyle-Width="120px" >
                            <FooterStyle HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Center" /> 
                          </asp:BoundField>
							
							<%--<asp:TemplateField HeaderText="Last Price" ItemStyle-HorizontalAlign = "Right" >
								<Itemtemplate>
								<asp:Label Runat="server" ID="LastPrice" style="text-align: right" text='<%# String.Format("{0:###,###.####}",Convert.ToDecimal(DataBinder.Eval(Container.DataItem, "Price"))) %>'/>
                                </Itemtemplate>									
							</asp:TemplateField>--%>
							
							<asp:TemplateField HeaderText="Unit" ItemStyle-HorizontalAlign = "Right" SortExpression="Unit">
								<Itemtemplate> 
									<asp:DropDownlist  CssClass="DropDownList" DataSourceID="dsUnit" 
                                    DataTextField="Unit_Code" DataValueField="Unit_Code" Runat="server" ID="Unit" SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Unit") %>'/>
								</Itemtemplate>	
							</asp:TemplateField>

							<asp:TemplateField HeaderText="Price Employe" ItemStyle-HorizontalAlign = "Right" SortExpression="Price">
								<Itemtemplate>
								<asp:TextBox runat="server" CssClass="TextBox" style="text-align: right" ID="Price" Maxlength = "9" text='<%# String.Format("{0:###,###.####}",Convert.ToDecimal(DataBinder.Eval(Container.DataItem, "Price"))) %>'/>
								</Itemtemplate>		
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Price " ItemStyle-HorizontalAlign = "Right" SortExpression="Price2">
								<Itemtemplate>
								<asp:TextBox runat="server" CssClass="TextBox" style="text-align: right" ID="Price2" Maxlength = "9" text='<%# String.Format("{0:###,###.####}",Convert.ToDecimal(DataBinder.Eval(Container.DataItem, "Price2"))) %>'/>
								</Itemtemplate>		
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Price 1 Trip" ItemStyle-HorizontalAlign = "Right" SortExpression="Price1trip">
								<Itemtemplate>
								<asp:TextBox runat="server" CssClass="TextBox" style="text-align: right" ID="Price1Trip" Maxlength = "9" text='<%# String.Format("{0:###,###.####}",Convert.ToDecimal(DataBinder.Eval(Container.DataItem, "Price1Trip"))) %>'/>
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
    <asp:SqlDataSource ID="dsUnit" runat="server"            
           SelectCommand="EXEC S_GetUnit">
        </asp:SqlDataSource>
    <asp:Label ID="lbstatus" ForeColor="red" runat="server"></asp:Label>
    </form>
</body>
</html>
