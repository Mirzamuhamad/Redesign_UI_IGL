<%@ Page Language="VB" AutoEventWireup="false" CodeFile="MsMachineMaintenance.aspx.vb" Inherits="Master_MsMachineMaintenance_MsMachineMaintenance" %>
<%@ Register Assembly="DevExpress.Web.ASPxGridView.v10.2, Version=10.2.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxGridView" TagPrefix="dx" %>
<%@ Register assembly="DevExpress.Web.ASPxEditors.v10.2" namespace="DevExpress.Web.ASPxEditors" tagprefix="dx" %>
<%@ Register Assembly="BasicFrame.WebControls.BasicDatePicker" Namespace="BasicFrame.WebControls"
    TagPrefix="BDP" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Untitled Page</title>
    <script src="../../Function/Function.JS" type="text/javascript"></script>
    <script src="../../Function/OpenDlg.js" type="text/javascript"></script>
    <script type="text/javascript">
    function OpenPopup() {         
        window.open("../../SearchDlg.Aspx","List","scrollbars=yes,resizable=no,width=500,height=400");        
        return false;
    } 

</script>
<link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
</head>
<body>    
    <form id="form1" runat="server">
    <div class="Content">   
    <div class="H1">Machine - Maintenance Item File</div>
    <hr style="color:Blue" /> 
        <br />                     
        
              <table width="100%">
                <tr>
                    <td style="width:100px">Machine</td>
                    <td>:</td>
                    <td>
                        <asp:TextBox CssClass="TextBox" Id="tbCode" runat="server" Width="120px" AutoPostBack="True"/>
                        <asp:TextBox CssClass="TextBox" ID="tbName" runat="server" Width="200px" Enabled="False" /> 
                        <asp:Button ID="btnSearch" runat="server" class="btngo" Text="..."/> 
                        <asp:Button class="bitbtn btnprint" runat="server" ID="btnPrint" Text="Print" />            
                    </td>
                    
                </tr>                 
              </table>  
               <asp:Panel runat="server" ID="PnlAssign">          
               <div style="border:0px  solid; width:100%; height:100%; overflow:auto;">
                    <asp:GridView id="DataGrid" runat="server" ShowFooter="True" AllowSorting="True" 
                            AutoGenerateColumns="False" AllowPaging="True" CssClass="Grid">
						<HeaderStyle CssClass="GridHeader" wrap="false"></HeaderStyle>
						<RowStyle CssClass="GridItem" Wrap="false"/>
						<AlternatingRowStyle CssClass="GridAltItem"/>
						<FooterStyle CssClass="GridFooter" />
						<PagerStyle CssClass="GridPager" />
				      <Columns>
							<asp:TemplateField HeaderText="Maintenance Item Code" HeaderStyle-Width="100" SortExpression="MaintenanceItem" Visible="false">
								<Itemtemplate>
									<asp:Label Runat="server" ID="MaintenanceItemCode" Width="100%"  text='<%# DataBinder.Eval(Container.DataItem, "MaintenanceItem") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:Label Runat="server" ID="MaintenanceItemCodeEdit" Width="100%"  text='<%# DataBinder.Eval(Container.DataItem, "MaintenanceItem") %>'>
									</asp:Label>																    
								</EditItemTemplate>
								<FooterTemplate>
									<asp:Label Runat="server" ID="MaintenanceItemCodeAdd" Width="100%"  text='<%# DataBinder.Eval(Container.DataItem, "MaintenanceItem") %>'>
									</asp:Label>
								</FooterTemplate>							    
							</asp:TemplateField>		
									
													
							<asp:TemplateField HeaderText="Maintenance Item" HeaderStyle-Width="100" SortExpression="MaintenanceItem_Name">
								<Itemtemplate>
									<asp:Label Runat="server" ID="MaintenanceItem" Width="100%"  text='<%# DataBinder.Eval(Container.DataItem, "MaintenanceItem_Name") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:Label Runat="server" ID="MaintenanceItemEdit" Width="100%"  text='<%# DataBinder.Eval(Container.DataItem, "MaintenanceItem_Name") %>'>
									</asp:Label>																    
								</EditItemTemplate>
								<FooterTemplate>
									<asp:DropDownList ID="MaintenanceItemAdd" Runat="Server" Width="100%" 
                                        CssClass="DropDownList" DataSourceID="dsMaintenanceItem" DataTextField="Item_Name" 
                                        DataValueField="Item_No">                                         
									</asp:DropDownList>
								</FooterTemplate>							    
							</asp:TemplateField>																						
										
							<asp:TemplateField HeaderText="Specification" HeaderStyle-Width="150" SortExpression="ItemSpecs">
								<Itemtemplate>
									<asp:Label Runat="server" Width="100%" ID="ItemSpecs" text='<%# DataBinder.Eval(Container.DataItem, "ItemSpecs") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
								     <asp:TextBox ID="ItemSpecsEdit" CssClass="TextBox" Width="100%" MaxLength="255"  Text='<%# DataBinder.Eval(Container.DataItem, "ItemSpecs") %>' Runat="Server"/>								   									
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="ItemSpecsAdd" CssClass="TextBox" Width="100%" MaxLength="255" Runat="Server"/>								   
								</FooterTemplate>
							    <HeaderStyle Width="200px" />
							</asp:TemplateField>											                            								
							
							<asp:TemplateField HeaderText="Item Number" HeaderStyle-Width="60" SortExpression="ItemNumber">
								<Itemtemplate>
									<asp:Label Runat="server" Width="100%" ID="ItemNumber" text='<%# DataBinder.Eval(Container.DataItem, "ItemNumber") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:TextBox ID="ItemNumberEdit" CssClass="TextBox" Width="100%" MaxLength="60" Text='<%# DataBinder.Eval(Container.DataItem, "ItemNumber") %>' Runat="Server"/>								   																		
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="ItemNumberAdd" CssClass="TextBox" Width="100%" MaxLength="60" Runat="Server"/>								   
								</FooterTemplate>
							    <HeaderStyle Width="60px" />
							</asp:TemplateField>		
							
							<asp:TemplateField HeaderText="Item Location" HeaderStyle-Width="60" SortExpression="ItemLocation">
								<Itemtemplate>
									<asp:Label Runat="server" Width="100%" ID="ItemLocation" text='<%# DataBinder.Eval(Container.DataItem, "ItemLocation") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:TextBox ID="ItemLocationEdit" CssClass="TextBox" Width="100%" MaxLength="60" Text='<%# DataBinder.Eval(Container.DataItem, "ItemLocation") %>' Runat="Server"/>								   																		
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="ItemLocationAdd" CssClass="TextBox" Width="100%" MaxLength="60" Runat="Server"/>								   
								</FooterTemplate>
							    <HeaderStyle Width="60px" />
							</asp:TemplateField>	
							
							<asp:TemplateField HeaderText="Item Qty" HeaderStyle-Width="60" SortExpression="ItemQty">
								<Itemtemplate>
									<asp:Label Runat="server" Width="100%" ID="ItemQty" text='<%# DataBinder.Eval(Container.DataItem, "ItemQty") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:TextBox ID="ItemQtyEdit" CssClass="TextBox" Width="100%" MaxLength="60" Text='<%# DataBinder.Eval(Container.DataItem, "ItemQty") %>' Runat="Server"/>								   																		
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="ItemQtyAdd" CssClass="TextBox" Width="100%" MaxLength="60" Runat="Server"/>								   
								</FooterTemplate>
							    <HeaderStyle Width="60px" />
							</asp:TemplateField>										
										
									
							<asp:TemplateField HeaderText="Action" headerstyle-width="180" >
							    <ItemTemplate>
								    <asp:Button ID="btnEdit" runat="server" class="bitbtndt btnedit" Text="Edit" CommandName="Edit"/>
									<asp:Button ID="btndelete" runat="server" class="bitbtndt btndelete" Text="Delete" OnClientClick="return confirm('Sure to delete this data?');" CommandName="Delete"/>																		
									<asp:Button ID="btnView" runat="server" class="bitbtndt btndetail" Text="Detail" CommandName="View" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>"/>
								</ItemTemplate>
								<EditItemTemplate>
									<asp:Button ID="btnUpdate" runat="server" class="bitbtndt btnsave" Text="Save" CommandName="Update"/>									
                                    <asp:Button ID="btnCancel" runat="server" class="bitbtndt btncancel" Text="Cancel" CommandName="Cancel"/>									
								</EditItemTemplate>
								<FooterTemplate>
									<asp:Button class="bitbtndt btnadd" runat="server" ID="btnAdd" Text="Add" CommandName="Insert"/>									
								</FooterTemplate>
							    <HeaderStyle Width="180px" />
							</asp:TemplateField>							
    					</Columns>
                        </asp:GridView> 
                        </div>
        </asp:Panel>
        <asp:Panel ID="pnlNewSlip" runat="server" Visible ="false">
            <table width="100%">
                <tr>
                    <td style="width:100px">Maintenance Item</td>
                    <td>:</td>
                    <td>
                        <asp:TextBox CssClass="TextBox" Id="tbMaintenanceCode" runat="server" Width="120px" Enabled="False"/>
                        <asp:TextBox CssClass="TextBox" ID="tbMaintenanceName" runat="server" Width="200px" Enabled="False" />                         
                    </td>
                    
                </tr>  
                 
            </table> 
            <table>
                   <div style="border:0px  solid; width:100%; height:100%; overflow:auto;">
                    <asp:GridView id="DataGridDt" runat="server" ShowFooter="True" AllowSorting="True" 
                            AutoGenerateColumns="False" AllowPaging="True" CssClass="Grid">
						<HeaderStyle CssClass="GridHeader" wrap="false"></HeaderStyle>
						<RowStyle CssClass="GridItem" Wrap="false"/>
						<AlternatingRowStyle CssClass="GridAltItem"/>
						<FooterStyle CssClass="GridFooter" />
						<PagerStyle CssClass="GridPager" />
				      <Columns>
							<asp:TemplateField HeaderText="Job Code" HeaderStyle-Width="10" SortExpression="Job" Visible="false">
								<Itemtemplate>
									<asp:Label Runat="server" ID="JobCode" Width="100%"  text='<%# DataBinder.Eval(Container.DataItem, "Job") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:Label Runat="server" ID="JobCodeEdit" Width="100%"  text='<%# DataBinder.Eval(Container.DataItem, "Job") %>'>
									</asp:Label>																    
								</EditItemTemplate>
								<FooterTemplate>
									<asp:Label Runat="server" ID="JobCodeAdd" Width="100%"  text='<%# DataBinder.Eval(Container.DataItem, "Job") %>'>
									</asp:Label>
								</FooterTemplate>							    
							</asp:TemplateField>		
																						
							<asp:TemplateField HeaderText="Job" HeaderStyle-Width="200" SortExpression="JobName">
								<Itemtemplate>
									<asp:Label Runat="server" ID="JobName" Width="100%" text='<%# DataBinder.Eval(Container.DataItem, "JobName") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>								    
									<asp:Label Runat="server" ID="JobNameEdit" Width="100%" text='<%# DataBinder.Eval(Container.DataItem, "JobName") %>'>
									</asp:Label>																    
								</EditItemTemplate>
								<FooterTemplate>
									<asp:DropDownList ID="JobNameAdd" Runat="Server" Width="100%" 
                                        CssClass="DropDownList" DataSourceID="dsJob" DataTextField="JobName" 
                                        DataValueField="JobCode">                                         
									</asp:DropDownList>
								</FooterTemplate>							    
							</asp:TemplateField>																						
							
							<asp:TemplateField HeaderText="Maintenance By" HeaderStyle-Width="100" SortExpression="MaintenanceBy">
								<Itemtemplate>
									<asp:Label Runat="server" ID="MaintenanceBy" Width="100%"  text='<%# DataBinder.Eval(Container.DataItem, "MaintenanceBy") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
								    <asp:DropDownList CssClass="DropDownList" ID="MaintenanceByEdit" Runat="Server" Width="100%" OnSelectedIndexChanged="MaintenanceByEdit_SelectedIndexChanged" AutoPostBack="true" text='<%# DataBinder.Eval(Container.DataItem, "MaintenanceBy") %>'> 
									   <asp:ListItem Selected="True">Running Hours</asp:ListItem>
									  <asp:ListItem>Interval</asp:ListItem>									  
									</asp:DropDownList>												    
								</EditItemTemplate>
								<FooterTemplate>								
									<asp:DropDownList ID="MaintenanceByAdd" Runat="Server" Width="100%" CssClass="DropDownList" OnSelectedIndexChanged="MaintenanceByAdd_SelectedIndexChanged" AutoPostBack="true" >                                         
									   <asp:ListItem Selected="True">Running Hours</asp:ListItem>
									  <asp:ListItem>Interval</asp:ListItem>									  
									</asp:DropDownList>
								</FooterTemplate>							    
							</asp:TemplateField>
										
							<asp:TemplateField HeaderText="Running Hour" HeaderStyle-Width="40" SortExpression="RunningHour">
								<Itemtemplate>
									<asp:Label Runat="server" Width="100%" ID="RunningHour" text='<%# DataBinder.Eval(Container.DataItem, "RunningHour") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
								     <asp:TextBox ID="RunningHourEdit" CssClass="TextBox" Width="100%" MaxLength="255"  Text='<%# DataBinder.Eval(Container.DataItem, "RunningHour") %>' Runat="Server"/>								   									
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="RunningHourAdd" CssClass="TextBox" Width="100%" MaxLength="255" Runat="Server"/>								   
								</FooterTemplate>
							    <HeaderStyle Width="40px" />
							</asp:TemplateField>											                            								
							
							<asp:TemplateField HeaderText="Maintenance Duration" HeaderStyle-Width="40" SortExpression="MaintenanceDuration">
								<Itemtemplate>
									<asp:Label Runat="server" Width="100%" ID="MaintenanceDuration" text='<%# DataBinder.Eval(Container.DataItem, "MaintenanceDuration") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<%--<asp:TextBox ID="MaintenanceDurationEdit" CssClass="TextBox" Width="100%" MaxLength="60" Text='<%# DataBinder.Eval(Container.DataItem, "MaintenanceDuration") %>' Runat="Server"/>--%>
									<asp:DropDownList ID="MaintenanceDurationEdit" Runat="Server" Width="100%" 
                                        CssClass="DropDownList" DataSourceID="dsMaintenanceDuration" DataTextField="DurationName" Text='<%# DataBinder.Eval(Container.DataItem, "MaintenanceDuration") %>' 
                                        DataValueField="DurationCode">                                         
									</asp:DropDownList> 
								</EditItemTemplate>
								<FooterTemplate>
									<%--<asp:TextBox ID="MaintenanceDurationAdd" CssClass="TextBox" Width="100%" MaxLength="60" Runat="Server"/>--%>
									<asp:DropDownList ID="MaintenanceDurationAdd" Runat="Server" Width="100%" 
                                        CssClass="DropDownList" DataSourceID="dsMaintenanceDuration" DataTextField="DurationName" 
                                        DataValueField="DurationCode">                                         
									</asp:DropDownList> 
								</FooterTemplate>
							    <HeaderStyle Width="40px" />
							</asp:TemplateField>		
							
							<asp:TemplateField HeaderText="Last Maintenance" HeaderStyle-Width="50" SortExpression="LastMaintenance">
								<Itemtemplate>
									<asp:Label Runat="server" Width="100%" ID="LastMaintenance" text='<%# DataBinder.Eval(Container.DataItem, "LastMaintenanceStr") %>'></asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:Label Runat="server" Width="100%" ID="LastMaintenanceEdit" text='<%# DataBinder.Eval(Container.DataItem, "LastMaintenanceStr") %>'></asp:Label>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:Label Runat="server" Width="100%" ID="LastMaintenanceAdd" text='<%# DataBinder.Eval(Container.DataItem, "LastMaintenance") %>'></asp:Label>
								</FooterTemplate>
							    <HeaderStyle Width="50px" />
							</asp:TemplateField>	
							
							<asp:TemplateField HeaderText="Next Maintenance" HeaderStyle-Width="50" SortExpression="NextMaintenance">
								<Itemtemplate>
									<asp:Label Runat="server" Width="100%" ID="NextMaintenance" text='<%# DataBinder.Eval(Container.DataItem, "NextMaintenanceStr") %>'></asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:Label Runat="server" Width="100%" ID="NextMaintenanceEdit" text='<%# DataBinder.Eval(Container.DataItem, "NextMaintenanceStr") %>'></asp:Label>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:Label Runat="server" Width="100%" ID="NextMaintenanceAdd" text='<%# DataBinder.Eval(Container.DataItem, "NextMaintenance") %>'></asp:Label>
								</FooterTemplate>
							    <HeaderStyle Width="50px" />
							</asp:TemplateField>										
							
							<asp:TemplateField HeaderText="Need Material" HeaderStyle-Width="40" SortExpression="FgNeedMaterial">
								<Itemtemplate>
									<asp:Label Runat="server" ID="FgNeedMaterial" Width="100%"  text='<%# DataBinder.Eval(Container.DataItem, "FgNeedMaterial") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
								    <asp:DropDownList ID="FgNeedMaterialEdit" Runat="Server" Width="100%" CssClass="DropDownList" text='<%# DataBinder.Eval(Container.DataItem, "FgNeedMaterial") %>' >                                         
									   <asp:ListItem Selected="True">Y</asp:ListItem>
									  <asp:ListItem>N</asp:ListItem>									  
									</asp:DropDownList>												    
								</EditItemTemplate>
								<FooterTemplate>
									<asp:DropDownList ID="FgNeedMaterialAdd" Runat="Server" Width="100%" CssClass="DropDownList" >                                         
									   <asp:ListItem Selected="True">Y</asp:ListItem>
									  <asp:ListItem>N</asp:ListItem>									  
									</asp:DropDownList>
								</FooterTemplate>							    
							</asp:TemplateField>			
									
							<asp:TemplateField HeaderText="Action" headerstyle-width="180" >
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
							    <HeaderStyle Width="180px" />
							</asp:TemplateField>							
    					</Columns>
                        </asp:GridView>
                        <table>
                        <tr>
                            <td colspan = "3">
                            <asp:Button class="bitbtn btncancel" runat="server" ID="btnCancelSlip" Text="Cancel" />                                                     
                            </td>                                   
                        </tr>
                        </table>
                 </div>            
            </table>
        </asp:Panel>        
    </div>
    <asp:SqlDataSource ID="dsJob" runat="server" 
          SelectCommand="EXEC S_GetMsJob">
    </asp:SqlDataSource>
    
    <asp:SqlDataSource ID="dsMaintenanceItem" runat="server" 
          SelectCommand="EXEC S_GetMaintenanceItem">
    </asp:SqlDataSource>
    
    <asp:SqlDataSource ID="dsMaintenanceDuration" runat="server" 
          SelectCommand="EXEC S_GetMsMaintenanceDurationChoose">
    </asp:SqlDataSource>
    <asp:Label ID="lbstatus" runat="server" ForeColor="Red"/>
    </form>
</body>
</html>
