<%@ Page Language="VB" AutoEventWireup="false" CodeFile="MsMachineLoadFactorOH.aspx.vb" Inherits="MsMachineLoadFactorOH_MsMachineLoadFactorOH" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Capacity Load Factor File</title>
    <script src="../../Function/Function.JS" type="text/javascript"></script>
    <script src="../../Function/OpenDlg.JS" type="text/javascript"></script>            
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div class="Content">
     <div class="H1">Capacity Load Factor File</div>
     <hr style="color:Blue" />
        <table>
        <tr>
            <td style="width:100px;text-align:right" >Quick Search :</td>
            <td><asp:TextBox runat="server" ID ="tbFilter" CssClass="TextBox" /> 
                <asp:DropDownList runat="server" ID="ddlField" CssClass="DropDownList" >
                  <asp:ListItem Selected="true" Text="Product Code" Value="Product"></asp:ListItem>
                  <asp:ListItem Text="Product Name" Value = "ProductName"></asp:ListItem>        
                  <asp:ListItem Text="Process Code" Value = "ProcessCode"></asp:ListItem>        
                  <asp:ListItem Text="Process Name" Value = "ProcessName"></asp:ListItem>        
                  <asp:ListItem Text="ProductOutput" Value = "ProductOutput"></asp:ListItem>        
                  <asp:ListItem Text="ProductOutputName" Value = "ProductOutputName"></asp:ListItem>        
                  <asp:ListItem Text="Qty / Man Hour" Value = "LaborRate"></asp:ListItem>   
                  <asp:ListItem Text="Qty / Machine Hour" Value = "OHRate"></asp:ListItem>                          
                </asp:DropDownList>     
                <asp:Button class="bitbtn btnsearch" runat="server" ID="btnSearch" Text="Search" />
                <asp:Button class="btngo" runat="server" ID="btnExpand" Text="..."/>
                <asp:Button class="bitbtn btnprint" runat="server" ID="btnPrint" Text="Print"/>
                &nbsp &nbsp &nbsp &nbsp
                Qty/Man Hour = Capacity / (Man Power * Hours)
                &nbsp &nbsp
                Qty/Machine Hour = Capacity / (Load Factor OH * Hours)
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
                  <asp:ListItem Selected="true" Text="Product Code" Value="Product"></asp:ListItem>
                  <asp:ListItem Text="Product Name" Value = "ProductName"></asp:ListItem>        
                  <asp:ListItem Text="Process Code" Value = "ProcessCode"></asp:ListItem>        
                  <asp:ListItem Text="Process Name" Value = "ProcessName"></asp:ListItem>        
                  <asp:ListItem Text="Product Output" Value = "ProductOutput"></asp:ListItem>        
                  <asp:ListItem Text="Product Output Name" Value = "ProductOutputName"></asp:ListItem>        
                  <asp:ListItem Text="Qty / Man Hour" Value = "LaborRate"></asp:ListItem>
                  <asp:ListItem Text="Qty / Machine Hour" Value = "OHRate"></asp:ListItem>
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
                <td><asp:Label ID="Label1" runat="server" Text = "Capacity : " />                 
                <asp:TextBox ID="tbCapacity" runat="server" CssClass="TextBox" MaxLength="15" Width="50px" />
                &nbsp &nbsp &nbsp &nbsp
                <asp:Label ID="Label2" runat="server" Text = "Hours : " />                             
                <asp:TextBox ID="tbHour" runat="server" CssClass="TextBox" MaxLength="15" Width="50px" />
                &nbsp &nbsp &nbsp &nbsp
                <asp:Label ID="Label3" runat="server" Text = "Man Power : " />                 
                <asp:TextBox ID="tbManPower" runat="server" CssClass="TextBox" MaxLength="15" Width="50px" />
                &nbsp &nbsp &nbsp &nbsp
                <asp:Label ID="Label4" runat="server" Text = "Factor OH : " />                
                <asp:TextBox ID="tbFactorOH" runat="server" CssClass="TextBox" MaxLength="15" Width="50px" />
                &nbsp &nbsp
                <asp:Button class="bitbtn btngo" runat="server" ID="btnApply" Text="G"/></td>
           </tr>           
           <%--<tr>
                <td><asp:Label ID="Label2" runat="server" Text = "Over Head Rate : " /> </td>                
                <td><asp:TextBox ID="tbOHRate" runat="server" CssClass="TextBox" MaxLength="15" 
                        Width="50px" /> 
                <asp:Button class="bitbtn btngo" runat="server" ID="btnApply2" Text="G"/></td>
           </tr>--%>
          
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
				      
							<asp:TemplateField HeaderText="Product Code" HeaderStyle-Width="100" SortExpression="Product">
								<Itemtemplate>
									<asp:Label Runat="server" ID="Product" text='<%# DataBinder.Eval(Container.DataItem, "Product") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:Label Runat="server" ID="ProductEdit"  Text='<%# DataBinder.Eval(Container.DataItem, "Product") %>'>
									</asp:Label>
								</EditItemTemplate>
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Product Name" HeaderStyle-Width="200" SortExpression="ProductName">
								<Itemtemplate>
									<asp:Label Runat="server" ID="ProductName" text='<%# DataBinder.Eval(Container.DataItem, "ProductName") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:Label Runat="server" ID="ProductNameEdit" text='<%# DataBinder.Eval(Container.DataItem, "ProductName") %>'>
									</asp:Label>
								</EditItemTemplate>
							</asp:TemplateField>		
							
							<asp:TemplateField HeaderText="Process" HeaderStyle-Width="200" SortExpression="Process">
								<Itemtemplate>
									<asp:Label Runat="server" ID="Process" text='<%# DataBinder.Eval(Container.DataItem, "Process") %>'>
									</asp:Label>
								</Itemtemplate>	
								<EditItemTemplate>
									<asp:Label Runat="server" ID="ProcessEdit" text='<%# DataBinder.Eval(Container.DataItem, "Process") %>'>
									</asp:Label>								
								</EditItemTemplate>								
							</asp:TemplateField>							  			
							
							<asp:TemplateField HeaderText="Process Name" HeaderStyle-Width="200" SortExpression="ProcessName">
								<Itemtemplate>
									<asp:Label Runat="server" ID="ProcessName" text='<%# DataBinder.Eval(Container.DataItem, "ProcessName") %>'>
									</asp:Label>
								</Itemtemplate>	
								<EditItemTemplate>
									<asp:Label Runat="server" ID="ProcessNameEdit" Text='<%# DataBinder.Eval(Container.DataItem, "ProcessName") %>'>
									</asp:Label>
								</EditItemTemplate>
							</asp:TemplateField>
							
        					<asp:TemplateField HeaderText="Product Output" HeaderStyle-Width="80" SortExpression="ProductOutput">
								<Itemtemplate>
									<asp:Label Runat="server" ID="ProductOutput" text='<%# DataBinder.Eval(Container.DataItem, "ProductOutput") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
    								<asp:Label Runat="server" ID="ProductOutputEdit" text='<%# DataBinder.Eval(Container.DataItem, "ProductOutput") %>'>
									</asp:Label>
								</EditItemTemplate>
							</asp:TemplateField>	
							
						     <asp:TemplateField HeaderText="Product Output Name" HeaderStyle-Width="200" SortExpression="ProductOutputName">
								<Itemtemplate>
									<asp:Label Runat="server" ID="ProductOutputName" text='<%# DataBinder.Eval(Container.DataItem, "ProductOutputName") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>														
									<asp:Label Runat="server" ID="ProductOutputNameEdit" text='<%# DataBinder.Eval(Container.DataItem, "ProductOutputName") %>'>
									</asp:Label>								
								</EditItemTemplate>
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Capacity" HeaderStyle-Width="100" SortExpression="Capacity" ItemStyle-HorizontalAlign = "Right">
								<Itemtemplate>
									<asp:Label Runat="server" ID="Capacity" text='<%# String.Format("{0:###,##0.00}",Convert.ToDecimal(DataBinder.Eval(Container.DataItem, "Capacity"))) %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:TextBox Runat="server" CssClass="TextBox" ID="CapacityEdit" MaxLength="9" Width="80" text='<%# String.Format("{0:###,##0.00}",Convert.ToDecimal(DataBinder.Eval(Container.DataItem, "Capacity"))) %>'>
									</asp:TextBox>									
								</EditItemTemplate>
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Hours" HeaderStyle-Width="100" SortExpression="Hours" ItemStyle-HorizontalAlign = "Right">
								<Itemtemplate>
									<asp:Label Runat="server" ID="Hours" text='<%# String.Format("{0:###,##0.00}",Convert.ToDecimal(DataBinder.Eval(Container.DataItem, "Hours"))) %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:TextBox Runat="server" CssClass="TextBox" ID="HoursEdit" MaxLength="9" Width="80" text='<%# String.Format("{0:###,##0.00}",Convert.ToDecimal(DataBinder.Eval(Container.DataItem, "Hours"))) %>'>
									</asp:TextBox>									
								</EditItemTemplate>
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Man Power" HeaderStyle-Width="100" SortExpression="ManPower" ItemStyle-HorizontalAlign = "Right">
								<Itemtemplate>
									<asp:Label Runat="server" ID="ManPower" text='<%# String.Format("{0:###,##0.00}",Convert.ToDecimal(DataBinder.Eval(Container.DataItem, "ManPower"))) %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:TextBox Runat="server" CssClass="TextBox" ID="ManPowerEdit" MaxLength="9" Width="80" text='<%# String.Format("{0:###,##0.00}",Convert.ToDecimal(DataBinder.Eval(Container.DataItem, "ManPower"))) %>'>
									</asp:TextBox>									
								</EditItemTemplate>
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Factor OH" HeaderStyle-Width="100" SortExpression="FactorOH" ItemStyle-HorizontalAlign = "Right">
								<Itemtemplate>
									<asp:Label Runat="server" ID="FactorOH" text='<%# String.Format("{0:###,##0.00}",Convert.ToDecimal(DataBinder.Eval(Container.DataItem, "FactorOH"))) %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:TextBox Runat="server" CssClass="TextBox" ID="FactorOHEdit" MaxLength="9" Width="80" text='<%# String.Format("{0:###,##0.00}",Convert.ToDecimal(DataBinder.Eval(Container.DataItem, "FactorOH"))) %>'>
									</asp:TextBox>									
								</EditItemTemplate>
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Qty / Man Hour" HeaderStyle-Width="100" SortExpression="LaborRate" ItemStyle-HorizontalAlign = "Right">
								<Itemtemplate>
									<asp:Label Runat="server" ID="LaborRate"  text='<%# String.Format("{0:###,##0.00}",Convert.ToDecimal(DataBinder.Eval(Container.DataItem, "LaborRate"))) %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:TextBox Runat="server" CssClass="TextBox" ID="LaborRateEdit" MaxLength="9" Width="80" text='<%# String.Format("{0:###,##0.00}",Convert.ToDecimal(DataBinder.Eval(Container.DataItem, "LaborRate"))) %>'>
									</asp:TextBox>									
								</EditItemTemplate>
							</asp:TemplateField>
																				
							<asp:TemplateField HeaderText="Qty / Machine Hour" HeaderStyle-Width="100" SortExpression="OHRate" ItemStyle-HorizontalAlign = "Right">
								<Itemtemplate>
									<asp:Label Runat="server" ID="OHRate" text='<%# String.Format("{0:###,##0.00}",Convert.ToDecimal(DataBinder.Eval(Container.DataItem, "OHRate"))) %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:TextBox Runat="server" CssClass="TextBox" ID="OHRateEdit" MaxLength="9" Width="80" text='<%# String.Format("{0:###,##0.00}",Convert.ToDecimal(DataBinder.Eval(Container.DataItem, "OHRate"))) %>'>
									</asp:TextBox>									
								</EditItemTemplate>
							</asp:TemplateField>			

							
							<%--<asp:TemplateField HeaderText="Action" headerstyle-width="126" >
								<ItemTemplate>
								    <asp:Button ID="btnEdit" runat="server" class="bitbtndt btnedit" Text="Edit" CommandName="Edit"/>									
								</ItemTemplate>
								<EditItemTemplate>
									<asp:Button ID="btnUpdate" runat="server" class="bitbtndt btnsave" Text="Save" CommandName="Update"/>									
                                    <asp:Button ID="btnCancel" runat="server" class="bitbtndt btncancel" Text="Cancel" CommandName="Cancel"/>									
								</EditItemTemplate>
							</asp:TemplateField>--%>
							
    					</Columns>
        </asp:GridView>
        
       <%-- <asp:SqlDataSource ID="dsUnit" runat="server"                                                                                 
               SelectCommand="EXEC S_GetProcess">                                        
        </asp:SqlDataSource>--%>

        
     <asp:Label ID="lstatus" ForeColor="red" runat="server"></asp:Label>
    </div>
    </form>
</body>
</html>
