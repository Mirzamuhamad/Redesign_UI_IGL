<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TrSetupAutoNmbr.aspx.vb" Inherits="Transaction_TrSetupAutoNmbr_TrSetupAutoNmbr" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<%@ Register Assembly="DevExpress.Web.ASPxGridView.v10.2, Version=10.2.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxGridView" TagPrefix="dx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">    
    <div class="Content">
     <div class="H1">Setup Auto Number</div>
     <hr style="color:Blue" />
     <asp:Panel ID="PnlMain" runat="server">
     <dx:ASPxGridView ID="DataGrid" runat="server" EnableCallBacks="false" KeyFieldName="SetCode" EmptyDataText="There are no data record(s) to display." AllowPaging="True" >
     <Settings ShowFilterRow="True" ShowFilterBar="Auto" ShowFilterRowMenu="True"  />
     <SettingsBehavior AllowFocusedRow="True" />
     <ClientSideEvents RowDblClick="function(s, e) {DataGrid.PerformCallback(s.GetFocusedRowIndex());}" />
     <Columns>
        <dx:GridViewDataColumn FieldName="SetDescription" Caption="Setup Description" VisibleIndex="1" Width = "250px"><Settings AutoFilterCondition="Contains" /></dx:GridViewDataColumn>
        <dx:GridViewDataColumn FieldName="SetValue" Caption="Set Value" VisibleIndex="2"  Width = "180px"><Settings AutoFilterCondition="Contains" /></dx:GridViewDataColumn>
        <dx:GridViewDataColumn FieldName="SetCode" Caption="Set Code" VisibleIndex="3" Visible="false" />
         </Columns>
                                                    
      </dx:ASPxGridView>
      <%--<asp:datagrid id="DataGrid" runat="server" OnPageIndexChanged="Grid_Change" 
            OnItemCommand="ItemCommand" AllowSorting="True" 
            AutoGenerateColumns="False" AllowPaging="True">
						<HeaderStyle CssClass="GridHeader"></HeaderStyle>
						<ItemStyle CssClass="GridItem" />
						<AlternatingItemStyle CssClass="GridAltItem"/>
						<PagerStyle CssClass="GridPager" Mode="NumericPages" />
				      <Columns>
							<asp:TemplateColumn HeaderText="Setup Description" SortExpression="SetDescription">
								<Itemtemplate>
									<asp:Label Runat="server" ID="SetDescription" text='<%# DataBinder.Eval(Container.DataItem, "SetDescription") %>'>
									</asp:Label>
								</Itemtemplate>														
							</asp:TemplateColumn>
							
							<asp:TemplateColumn HeaderText="Set Value" SortExpression="SetValue">
								<Itemtemplate>
									<asp:Label Runat="server" ID="SetValue" text='<%# DataBinder.Eval(Container.DataItem, "SetValue") %>'>
									</asp:Label>
								</Itemtemplate>														
							</asp:TemplateColumn>		
							
							<asp:TemplateColumn HeaderText="SetCode" Visible="false">
								<Itemtemplate>
									<asp:Label Runat="server" ID="SetCode" text='<%# DataBinder.Eval(Container.DataItem, "SetCode") %>'>
									</asp:Label>
								</Itemtemplate>								
							</asp:TemplateColumn>							
														
							<asp:TemplateColumn HeaderText="Action">
								<ItemTemplate>
									<asp:Button CommandName="Edit" CssClass="Button" Text="Edit" ID="btnEdit" Runat="server" Width="50" />									
								</ItemTemplate>								
							</asp:TemplateColumn>							
    					</Columns>
        </asp:datagrid>--%>
      </asp:Panel>
     <asp:Panel ID="pnlInput" runat="server" Visible="false">         
        <table>
            <tr>
                <td colspan="9"><asp:CheckBox AutoPostBack = "true" ID="cxCompany" runat="server" Text="Company" /></td>
            </tr>
            <tr> 
                <td>Company Alias</td>
                <td>:</td>
                <td><asp:TextBox CssClass="TextBox" ID="tbAlias" Width="33px" runat="server" Enabled="false" Text="++" /></td>
                
                <td>Separator</td>
                <td>:</td>
                <td><asp:DropDownList ID="ddlSeparator1" CssClass="DropDownList" runat="server" AutoPostBack="true">
                        <asp:ListItem> </asp:ListItem>
                        <asp:ListItem>/</asp:ListItem>
                        <asp:ListItem>-</asp:ListItem>
                        <asp:ListItem>.</asp:ListItem>
                    </asp:DropDownList> 
                </td>
                
                <td>Priority</td>
                <td>:</td>
                <td><asp:DropDownList ID="ddlPriority1" CssClass="DropDownList" runat="server" AutoPostBack="true">
                        <asp:ListItem>1</asp:ListItem>
                        <asp:ListItem>2</asp:ListItem>
                        <asp:ListItem>3</asp:ListItem>
                        <asp:ListItem>4</asp:ListItem>
                        <asp:ListItem>5</asp:ListItem>
                        <asp:ListItem>6</asp:ListItem>                        
                    </asp:DropDownList>                
                </td>                
            </tr>    
            
            <tr>
                <td colspan="9"><asp:CheckBox AutoPostBack = "true" ID="cxTransaction" runat="server" Text="Transaction" /></td>
            </tr>
            <tr>
                <td>Transaction Alias</td>
                <td>:</td>
                <td><asp:TextBox ID="tbTransType" CssClass="TextBox" Width="33px" runat="server" /></td>
                
                <td>Separator</td>
                <td>:</td>
                <td><asp:DropDownList ID="ddlSeparator2" CssClass="DropDownList" runat="server" AutoPostBack="true">
                        <asp:ListItem> </asp:ListItem>
                        <asp:ListItem>/</asp:ListItem>
                        <asp:ListItem>-</asp:ListItem>
                        <asp:ListItem>.</asp:ListItem>
                    </asp:DropDownList>                
                </td>
                
                <td>Priority</td>
                <td>:</td>
                <td><asp:DropDownList ID="ddlPriority2" CssClass="DropDownList" runat="server" AutoPostBack="true">
                        <asp:ListItem>1</asp:ListItem>
                        <asp:ListItem>2</asp:ListItem>
                        <asp:ListItem>3</asp:ListItem>
                        <asp:ListItem>4</asp:ListItem>
                        <asp:ListItem>5</asp:ListItem>  
                        <asp:ListItem>6</asp:ListItem>                      
                    </asp:DropDownList>                
                </td>                
            </tr>                
            
            <tr>
                <td colspan="9"><asp:CheckBox AutoPostBack = "true" ID="cxAddParam" runat="server" Text="Additional Parameter" /></td>
            </tr>
            <tr>
                <td>Digit Parameter</td>
                <td>:</td>
                <td><asp:DropDownList ID="ddlDigitParam" CssClass="DropDownList" runat="server" AutoPostBack="true">
                        <asp:ListItem>1</asp:ListItem>
                        <asp:ListItem>2</asp:ListItem>
                        <asp:ListItem>3</asp:ListItem>
                        <asp:ListItem>4</asp:ListItem>
                        <asp:ListItem>5</asp:ListItem>
                        <asp:ListItem>6</asp:ListItem>
                    </asp:DropDownList>
                </td>
                
                <td>Separator</td>
                <td>:</td>
                <td><asp:DropDownList ID="ddlSeparator3" CssClass="DropDownList" runat="server" AutoPostBack="true">
                        <asp:ListItem> </asp:ListItem>
                        <asp:ListItem>/</asp:ListItem>
                        <asp:ListItem>-</asp:ListItem>
                        <asp:ListItem>.</asp:ListItem>
                    </asp:DropDownList>                
                </td>
                
                <td>Priority</td>
                <td>:</td>
                <td><asp:DropDownList ID="ddlPriority3" CssClass="DropDownList" runat="server" AutoPostBack="true">
                        <asp:ListItem>1</asp:ListItem>
                        <asp:ListItem>2</asp:ListItem>
                        <asp:ListItem>3</asp:ListItem>
                        <asp:ListItem>4</asp:ListItem>
                        <asp:ListItem>5</asp:ListItem> 
                        <asp:ListItem>6</asp:ListItem>                       
                    </asp:DropDownList>                
                </td>                
            </tr>  
            
            <tr>
                <td colspan="9"><asp:CheckBox AutoPostBack = "true" ID="cxYear" runat="server" Text="Year" /></td>
            </tr>
            <tr>
                <td>Digit Year</td>
                <td>:</td>
                <td><asp:DropDownList ID="ddlDigitYear" CssClass="DropDownList" runat="server" AutoPostBack="true">                        
                        <asp:ListItem>2</asp:ListItem>
                        <asp:ListItem>4</asp:ListItem>                        
                    </asp:DropDownList>
                </td>
                
                <td>Separator</td>
                <td>:</td>
                <td><asp:DropDownList ID="ddlSeparator4" CssClass="DropDownList" runat="server" AutoPostBack="true">
                        <asp:ListItem> </asp:ListItem>
                        <asp:ListItem>/</asp:ListItem>
                        <asp:ListItem>-</asp:ListItem>
                        <asp:ListItem>.</asp:ListItem>
                    </asp:DropDownList>                
                </td>
                
                <td>Priority</td>
                <td>:</td>
                <td><asp:DropDownList ID="ddlPriority4" CssClass="DropDownList" runat="server" AutoPostBack="true">
                        <asp:ListItem>1</asp:ListItem>
                        <asp:ListItem>2</asp:ListItem>
                        <asp:ListItem>3</asp:ListItem>
                        <asp:ListItem>4</asp:ListItem>
                        <asp:ListItem>5</asp:ListItem> 
                        <asp:ListItem>6</asp:ListItem>                       
                    </asp:DropDownList>                
                </td>                
            </tr>  
                         
            <tr>
                <td colspan="2"><asp:CheckBox AutoPostBack = "true" ID="cxMonth" runat="server" Text="Month" /></td>
                <td colspan="7"><asp:CheckBox AutoPostBack = "true" ID="cxByYear" runat="server" Text="Generate Number by year" /></td>
            </tr>
            <tr>
                <td>Digit Month</td>
                <td>:</td>
                <td><asp:DropDownList ID="ddlDigitMonth" CssClass="DropDownList" runat="server" AutoPostBack="true">                        
                        <asp:ListItem>Huruf</asp:ListItem>
                        <asp:ListItem>Angka</asp:ListItem>
                        <asp:ListItem>Singkatan</asp:ListItem>
                    </asp:DropDownList>
                </td>
                
                <td>Separator</td>
                <td>:</td>
                <td><asp:DropDownList ID="ddlSeparator5" CssClass="DropDownList" runat="server" AutoPostBack="true">
                        <asp:ListItem> </asp:ListItem>
                        <asp:ListItem>/</asp:ListItem>
                        <asp:ListItem>-</asp:ListItem>
                        <asp:ListItem>.</asp:ListItem>
                    </asp:DropDownList>                
                </td>
                
                <td>Priority</td>
                <td>:</td>
                <td><asp:DropDownList ID="ddlPriority5" CssClass="DropDownList" runat="server" AutoPostBack="true">
                        <asp:ListItem>1</asp:ListItem>
                        <asp:ListItem>2</asp:ListItem>
                        <asp:ListItem>3</asp:ListItem>
                        <asp:ListItem>4</asp:ListItem>
                        <asp:ListItem>5</asp:ListItem>
                        <asp:ListItem>6</asp:ListItem>
                    </asp:DropDownList>                
                </td>                
            </tr>   
            
            
            <tr>
                <td>Digit Number</td>
                <td>:</td>
                <td><asp:DropDownList ID="ddlNumber" CssClass="DropDownList" runat="server" AutoPostBack="true">                        
                        <asp:ListItem>1</asp:ListItem>
                        <asp:ListItem>2</asp:ListItem>
                        <asp:ListItem>3</asp:ListItem>
                        <asp:ListItem>4</asp:ListItem>
                        <asp:ListItem>5</asp:ListItem>
                        <asp:ListItem>6</asp:ListItem>
                        <asp:ListItem>7</asp:ListItem>
                        <asp:ListItem>8</asp:ListItem>
                    </asp:DropDownList>
                </td>
                
                <td>Separator</td>
                <td>:</td>
                <td><asp:DropDownList ID="ddlSeparator6" CssClass="DropDownList" runat="server" AutoPostBack="true">
                        <asp:ListItem> </asp:ListItem>
                        <asp:ListItem>/</asp:ListItem>
                        <asp:ListItem>-</asp:ListItem>
                        <asp:ListItem>.</asp:ListItem>
                    </asp:DropDownList>                
                </td>
                
                <td>Priority</td>
                <td>:</td>
                <td><asp:DropDownList ID="ddlPriority6" CssClass="DropDownList" runat="server" AutoPostBack="true" 
                        style="height: 22px">
                        <asp:ListItem>1</asp:ListItem>
                        <asp:ListItem>2</asp:ListItem>
                        <asp:ListItem>3</asp:ListItem>
                        <asp:ListItem>4</asp:ListItem>
                        <asp:ListItem>5</asp:ListItem>
                        <asp:ListItem>6</asp:ListItem>
                    </asp:DropDownList>                
                </td>                
            </tr>   
            <tr>
                <td>Description</td>
                <td>:</td>
                <td colspan="7"><asp:TextBox ID="tbDescription" CssClass="TextBox" runat="server" Width="100%" /></td>
            </tr>    
            <tr>
                <td>Current Format</td>
                <td>:</td>
                <td colspan="7"><asp:Label ID="lbCurrentFormat" runat="server"></asp:Label></td>
            </tr>
            <tr>
                <td>New Format</td>
                <td>:</td>
                <td colspan="7"><asp:Label ID="lbNewFormat" runat="server"></asp:Label></td>
            </tr>
            <tr>
                <td>Example</td>
                <td>:</td>
                <td colspan="7"><asp:Label ID="lbExample" runat="server" Font-Bold="True" 
                        Font-Size="Medium" ForeColor="#FF3300"></asp:Label></td>
            </tr>
            <tr>
                <td align="center" colspan="9">
                    <asp:Button ID="btnSave" runat="server" CssClass="btngo" Text="Save" Width="38px" /> 
                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="btngo" 
                        Width="38px" />                     
                </td>
            </tr>
        </table>        
     </asp:Panel>   
     <asp:Label ID="lstatus" ForeColor="red" runat="server"></asp:Label>        
    </div>
    </form>
</body>
</html>
