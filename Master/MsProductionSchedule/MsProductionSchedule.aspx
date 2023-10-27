<%@ Page Language="VB" AutoEventWireup="false" CodeFile="MsProductionSchedule.aspx.vb" Inherits="MsMachineLoadFactorLB_MsMachineLoadFactorLB" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="BasicFrame.WebControls.BasicDatePicker" Namespace="BasicFrame.WebControls"  TagPrefix="BDP" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Production Schedule File</title>
    <script src="../../Function/Function.JS" type="text/javascript"></script>
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
            <td style="width:100px;text-align:right" >Search : </td>
            <td><asp:TextBox runat="server" ID ="tbFilter" CssClass="TextBox" /> 
                <asp:DropDownList runat="server" ID="ddlField" CssClass="DropDownList" >
                  <asp:ListItem Selected="true" Text="Type" Value="ReffType"></asp:ListItem>
                  <asp:ListItem Text="Reference" Value = "Reference"></asp:ListItem>        
                  <asp:ListItem Text="Product" Value = "Product"></asp:ListItem>        
                  <asp:ListItem Text="Product Name" Value = "ProductName"></asp:ListItem>        
                  <asp:ListItem Text="Work Ctr" Value = "WorkCtrName"></asp:ListItem>        
                  <asp:ListItem Text="Delivery Date" Value = "Delivery_Date"></asp:ListItem>     
                  <asp:ListItem Text="Delivery Date Rev" Value = "DeliveryDate_Rev"></asp:ListItem>   
                  <asp:ListItem Text="Unit" Value = "Unit"></asp:ListItem>
                  <asp:ListItem Text="Machine" Value = "Machine"></asp:ListItem>
                  <asp:ListItem Text="Production Date" Value = "Production_Date"></asp:ListItem>
                </asp:DropDownList>     
                <asp:Button class="bitbtn btnsearch" runat="server" ID="btnSearch" Text="Search" />
                <asp:Button class="btngo" runat="server" ID="btnExpand" Text="..."/>
                <asp:Button class="bitbtn btnprint" runat="server" ID="btnPrint" Text="Print"/>
                <asp:Button class="bitbtn btnexcel" runat="server" ID="btnExport" Text="Export" />                  
                Show Records:                
                <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlShowRecord" AutoPostBack="true">
                    <asp:ListItem Selected="True" Value="15">Choose One</asp:ListItem>
                    <asp:ListItem Value="20">20</asp:ListItem>
                    <asp:ListItem Value="30">30</asp:ListItem>
                    <asp:ListItem Value="40">40</asp:ListItem>
                    <asp:ListItem Value="50">50</asp:ListItem>
                    <asp:ListItem Value="100">100</asp:ListItem>  
                </asp:DropDownList>                  
                Rows
                &nbsp &nbsp &nbsp &nbsp
                <asp:GridView ID="GridExport" runat="server" AutoGenerateColumns="True" 
                Visible="true" ShowFooter="False" GridLines="None" Height="79px" PageSize="20" Width="724px">
               <HeaderStyle CssClass="GridHeader" />
               <RowStyle CssClass="GridItem" Wrap="false" />
               <AlternatingRowStyle CssClass="GridAltItem" />
               <PagerStyle CssClass="GridPager" />
           </asp:GridView>
                
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
                  <asp:ListItem Selected="true" Text="Type" Value="ReffType"></asp:ListItem>
                  <asp:ListItem Text="Reference" Value = "Reference"></asp:ListItem>        
                  <asp:ListItem Text="Product" Value = "Product"></asp:ListItem>        
                  <asp:ListItem Text="Product Name" Value = "ProductName"></asp:ListItem>        
                  <asp:ListItem Text="Work Ctr" Value = "WorkCtrName"></asp:ListItem>        
                  <asp:ListItem Text="Delivery Date" Value = "Delivery_Date"></asp:ListItem>     
                  <asp:ListItem Text="Delivery Date Rev" Value = "DeliveryDate_Rev"></asp:ListItem>   
                  <asp:ListItem Text="Unit" Value = "Unit"></asp:ListItem>
                  <asp:ListItem Text="Machine" Value = "Machine"></asp:ListItem>
                  <asp:ListItem Text="Production Date" Value = "Production_Date"></asp:ListItem>
                  </asp:DropDownList>                
            </td>
        </tr>
     </table>
     </asp:Panel>
        </br>
        <br />
        <table>
            <tr>
                <td>
                    <table bgcolor=silver >
                    <tr>
                        <td colspan = "2"><b><asp:Label ID="lb1" runat="server" Text = "Delivery Revision : " /></b> </td>                
                    </tr>
                    <tr>
                        <td><asp:CheckBox ID="cbDeliveryRevDate" runat="server" />
                        <asp:Label ID="Label2" runat="server" Text = "Date : " /> </td>                
                        <td><BDP:BasicDatePicker ID="tbDeliveryRevDate" runat="server" DateFormat="dd MMM yyyy" ButtonImageHeight="19px" ButtonImageWidth="20px" ReadOnly = "true" 
                        DisplayType="TextBoxAndImage" TextBoxStyle-CssClass="TextDate" ShowNoneButton="False" />
                        </td>
                
                    </tr>
                    <tr>
                        <td><asp:CheckBox ID="cbDeliveryRevDays" runat="server" />
                        <asp:Label ID="Label1" runat="server" Text = "Days : " /> </td>                
                        <td><asp:TextBox ID="tbDeliveryRevDays" runat="server" CssClass="TextBox" MaxLength="15" Width="50px" /> </td>
                    </tr>                   
                    <tr>
                        <td>&nbsp</td> 
                        <td><asp:Button class="bitbtn btngo" runat="server" ID="btnApplyDelRevDate" Width=100px Text="Go"/></td>     
                    </tr>
                    </table>
                </td>
                
                <td>
                    &nbsp
                </td>
                <td>
                    &nbsp
                </td>
                
                <td>
                    <table bgcolor=silver >
                    <tr>
                        <td colspan = "2"><b><asp:Label ID="Label3" runat="server" Text = "Production Date : " /></b> </td> 
                    </tr>
                    <tr>
                        <td><asp:CheckBox ID="cbProdDate" runat="server" />
                        <asp:Label ID="Label4" runat="server" Text = "Date : " /> </td>                
                        <td><BDP:BasicDatePicker ID="tbProdDate" runat="server" DateFormat="dd MMM yyyy" ButtonImageHeight="19px" ButtonImageWidth="20px" ReadOnly = "true" 
                            DisplayType="TextBoxAndImage" TextBoxStyle-CssClass="TextDate" ShowNoneButton="False" />
                        </td>
                
                    </tr>
                    <tr>
                        <td><asp:CheckBox ID="cbProdDays" runat="server" />
                        <asp:Label ID="Label5" runat="server" Text = "Days : " /> </td>                
                        <td><asp:TextBox ID="tbProdDays" runat="server" CssClass="TextBox" MaxLength="15" Width="50px" /> </td>
                    </tr>                   
                    <tr>
                        <td>&nbsp</td> 
                        <td><asp:Button class="bitbtn btngo" runat="server" ID="btnApplyProdDate" Width=100px Text="Go"/></td>     
                    </tr>
                    </table>     
                </td>
                
                <td>
                    &nbsp
                </td>
                <td>
                    &nbsp
                </td>
               
                <td>
                    <table bgcolor=silver >
                    <tr>
                        <td colspan = "2"><b><asp:Label ID="Label6" runat="server" Text = "Machine : " /></b> </td>
                    </tr>
                    <tr>
                        <td><asp:Label ID="Label7" runat="server" Text = "Machine : " /> </td>                
                        <td><asp:DropDownList ID="ddlMachine" runat="server" CssClass="DropDownList" ValidationGroup="Input" /></td>                
                    </tr>                           
                    <tr>
                        <td>&nbsp</td> 
                        <td><asp:Button class="bitbtn btngo" runat="server" ID="btnApplyMachine" Width=100px Text="Go"/></td>     
                    </tr>
                    </table>
                </td>
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
				      
							<asp:TemplateField HeaderText="Type" HeaderStyle-Width="100" SortExpression="ReffType">
								<Itemtemplate>
									<asp:Label Runat="server" ID="ReffType" text='<%# DataBinder.Eval(Container.DataItem, "ReffType") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:Label Runat="server" ID="ReffTypeEdit"  Text='<%# DataBinder.Eval(Container.DataItem, "ReffType") %>'>
									</asp:Label>
								</EditItemTemplate>
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Reference" HeaderStyle-Width="200" SortExpression="Reference">
								<Itemtemplate>
									<asp:Label Runat="server" ID="Reference" text='<%# DataBinder.Eval(Container.DataItem, "Reference") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:Label Runat="server" ID="ReferenceEdit" text='<%# DataBinder.Eval(Container.DataItem, "Reference") %>'>
									</asp:Label>
								</EditItemTemplate>
							</asp:TemplateField>		
							
							<asp:TemplateField HeaderText="Product" HeaderStyle-Width="200" SortExpression="Product">
								<Itemtemplate>
									<asp:Label Runat="server" ID="Product" text='<%# DataBinder.Eval(Container.DataItem, "Product") %>'>
									</asp:Label>
								</Itemtemplate>	
								<EditItemTemplate>
									<asp:Label Runat="server" ID="ProductEdit" text='<%# DataBinder.Eval(Container.DataItem, "Product") %>'>
									</asp:Label>								
								</EditItemTemplate>								
							</asp:TemplateField>							  			
							
							<asp:TemplateField HeaderText="Product Name" HeaderStyle-Width="200" SortExpression="ProductName">
								<Itemtemplate>
									<asp:Label Runat="server" ID="ProductName" text='<%# DataBinder.Eval(Container.DataItem, "ProductName") %>'>
									</asp:Label>
								</Itemtemplate>	
								<EditItemTemplate>
									<asp:Label Runat="server" ID="ProductNameEdit" Text='<%# DataBinder.Eval(Container.DataItem, "ProductName") %>'>
									</asp:Label>
								</EditItemTemplate>
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Work Ctr" HeaderStyle-Width="200" SortExpression="WorkCtrName">
								<Itemtemplate>
									<asp:Label Runat="server" ID="WorkCtrName" text='<%# DataBinder.Eval(Container.DataItem, "WorkCtrName") %>'>
									</asp:Label>
								</Itemtemplate>	
								<EditItemTemplate>
									<asp:Label Runat="server" ID="WorkCtrNameEdit" Text='<%# DataBinder.Eval(Container.DataItem, "WorkCtrName") %>'>
									</asp:Label>
								</EditItemTemplate>
							</asp:TemplateField>
							
        					<asp:TemplateField HeaderText="Delivery Date" HeaderStyle-Width="200" SortExpression="DeliveryDate">
								<Itemtemplate>
									<asp:Label Runat="server" ID="DeliveryDate" text='<%# DataBinder.Eval(Container.DataItem, "Delivery_Date") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
    								<%--<BDP:BasicDatePicker ID="DeliveryDateEdit" runat="server" DateFormat="dd MMM yyyy" ButtonImageHeight="19px" ButtonImageWidth="20px" ReadOnly = "true" 
                                        DisplayType="TextBoxAndImage" TextBoxStyle-CssClass="TextDate" ShowNoneButton="False" text='<%# DataBinder.Eval(Container.DataItem, "ProductionDate") %>'>
                                    </BDP:BasicDatePicker>--%>
    								<asp:Label Runat="server" ID="DeliveryDateEdit" text='<%# DataBinder.Eval(Container.DataItem, "DeliveryDate") %>'>
									</asp:Label>
								</EditItemTemplate>
							</asp:TemplateField>	
							
							<asp:TemplateField HeaderText="Delivery Date Revisi" HeaderStyle-Width="200" SortExpression="DeliveryDateRev">
								<Itemtemplate>
									<asp:Label Runat="server" ID="DeliveryDateRev" text='<%# DataBinder.Eval(Container.DataItem, "DeliveryDate_Rev") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
								    <BDP:BasicDatePicker ID="DeliveryDateRevEdit" runat="server" DateFormat="dd MMM yyyy" ButtonImageHeight="19px" ButtonImageWidth="20px" ReadOnly = "true" 
                                        DisplayType="TextBoxAndImage" TextBoxStyle-CssClass="TextDate" ShowNoneButton="False" SelectedValue='<%# DataBinder.Eval(Container.DataItem, "DeliveryDateRev") %>'>
                                    </BDP:BasicDatePicker>    								
								</EditItemTemplate>
							</asp:TemplateField>
							
						     <asp:TemplateField HeaderText="Qty Wrhs" HeaderStyle-Width="50" SortExpression="QtyWrhs" ItemStyle-HorizontalAlign = "Right">
								<Itemtemplate>
									<asp:Label Runat="server" ID="QtyWrhs" DataFormatString="{0:#,##0.00}" text='<%# DataBinder.Eval(Container.DataItem, "QtyWrhs") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>														
									<asp:Label Runat="server" ID="QtyWrhsEdit" text='<%# DataBinder.Eval(Container.DataItem, "QtyWrhs") %>'>
									</asp:Label>								
								</EditItemTemplate>
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Qty (M2)" HeaderStyle-Width="50" SortExpression="Qty_M2" ItemStyle-HorizontalAlign = "Right">
								<Itemtemplate>
									<asp:Label Runat="server" ID="QtyM2" DataFormatString="{0:#,##0.00}" text='<%# DataBinder.Eval(Container.DataItem, "Qty_M2") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>														
									<asp:Label Runat="server" ID="QtyM2Edit" text='<%# DataBinder.Eval(Container.DataItem, "Qty_M2") %>'>
									</asp:Label>								
								</EditItemTemplate>
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Qty (ROLL)" HeaderStyle-Width="50" SortExpression="Qty_ROLL" ItemStyle-HorizontalAlign = "Right">
								<Itemtemplate>
									<asp:Label Runat="server" ID="QtyROLL" DataFormatString="{0:#,##0.00}" text='<%# DataBinder.Eval(Container.DataItem, "Qty_ROLL") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>														
									<asp:Label Runat="server" ID="QtyROLLEdit" text='<%# DataBinder.Eval(Container.DataItem, "Qty_ROLL") %>'>
									</asp:Label>								
								</EditItemTemplate>
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Unit" HeaderStyle-Width="50" SortExpression="Unit">
								<Itemtemplate>
									<asp:Label Runat="server" ID="Unit" text='<%# DataBinder.Eval(Container.DataItem, "Unit") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>														
									<asp:Label Runat="server" ID="UnitEdit" text='<%# DataBinder.Eval(Container.DataItem, "Unit") %>'>
									</asp:Label>								
								</EditItemTemplate>
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Machine" HeaderStyle-Width="100" SortExpression="Machine">
								<Itemtemplate>
									<asp:Label Runat="server" ID="Machine" text='<%# DataBinder.Eval(Container.DataItem, "Machine") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
								    <asp:DropDownList Runat="server" ID="MachineEdit" Width="100%" CssClass="DropDownList"
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Machine") %>'
                                        DataSourceID="dsMachine" DataTextField="Machine_Name" DataValueField="Machine_Code">
									</asp:DropDownList>	
									<%--<asp:TextBox Runat="server" CssClass="TextBox" ID="MachineEdit" MaxLength="9" Width="80" Text='<%# DataBinder.Eval(Container.DataItem, "Machine") %>'>
									</asp:TextBox>--%>									
								</EditItemTemplate>
							</asp:TemplateField>	
							
							<asp:TemplateField HeaderText="Production Date" HeaderStyle-Width="100" SortExpression="ProductionDate">
								<Itemtemplate>
									<asp:Label Runat="server" ID="ProductionDate" text='<%# DataBinder.Eval(Container.DataItem, "Production_Date") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
								    <BDP:BasicDatePicker ID="ProductionDateEdit" runat="server" DateFormat="dd MMM yyyy" ButtonImageHeight="19px" ButtonImageWidth="20px" ReadOnly = "true" 
                                        DisplayType="TextBoxAndImage" TextBoxStyle-CssClass="TextDate" ShowNoneButton="False" SelectedValue='<%# DataBinder.Eval(Container.DataItem, "ProductionDate") %>'>
                                    </BDP:BasicDatePicker>																
								</EditItemTemplate>
							</asp:TemplateField>	
							
							<asp:TemplateField HeaderText="Action" headerstyle-width="126" >
								<ItemTemplate>
								    <asp:Button ID="btnEdit" runat="server" class="bitbtndt btnedit" Text="Edit" CommandName="Edit"/>									
								</ItemTemplate>
								<EditItemTemplate>
									<asp:Button ID="btnUpdate" runat="server" class="bitbtndt btnsave" Text="Save" CommandName="Update"/>									
                                    <asp:Button ID="btnCancel" runat="server" class="bitbtndt btncancel" Text="Cancel" CommandName="Cancel"/>									
								</EditItemTemplate>
							</asp:TemplateField>
							
    					</Columns>
        </asp:GridView>
        
        <asp:SqlDataSource ID="dsMachine" runat="server" SelectCommand="EXEC S_GetMachineForProdPlan" />                                                
        
     <asp:Label ID="lstatus" ForeColor="red" runat="server"></asp:Label>
    </div>
    </form>
</body>
</html>
