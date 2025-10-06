<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TrSetupPayroll.aspx.vb" Inherits="Transaction_TrSetupPayroll_TrSetupPayroll" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<%@ Register Assembly="DevExpress.Web.ASPxGridView.v10.2, Version=10.2.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxGridView" TagPrefix="dx" %>

<%@ Register assembly="DevExpress.Web.ASPxEditors.v10.2" namespace="DevExpress.Web.ASPxEditors" tagprefix="dx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
    <script type="text/javascript">
        function OpenPopup() {         
            window.open("../../SearchDlg.Aspx","List","scrollbars=yes,resizable=no,width=500,height=400");        
            return false;
        }    
    </script>   
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
 <link href="https://fonts.googleapis.com/css2?family=Muli:wght@300;400;600&display=swap" rel="stylesheet">
</head>
<body>
    <form id="form1" runat="server">
    <div class="Content">
     <div class="H1">Setup Payroll</div>
     <hr style="color:Blue" />
     <asp:Panel ID="PnlMain" runat="server">
     
     <dx:ASPxGridView ID="DataGrid" runat="server" EnableCallBacks="false" KeyFieldName="SetCode" EmptyDataText="There are no data record(s) to display." AllowPaging="True" >
     <Settings ShowFilterRow="True" ShowFilterBar="Auto" ShowFilterRowMenu="True"  />
     <SettingsBehavior AllowFocusedRow="True" />
     <ClientSideEvents RowDblClick="function(s, e) {DataGrid.PerformCallback(s.GetFocusedRowIndex());}" />
     <Columns>
        <dx:GridViewDataColumn FieldName="SetDescription" Caption="Setup Payroll" VisibleIndex="1" Width = "230px"><Settings AutoFilterCondition="Contains" /></dx:GridViewDataColumn>
        <dx:GridViewDataColumn FieldName="SetValue" Caption="Payroll Code" VisibleIndex="2" Width = "60px"><Settings AutoFilterCondition="Contains" /></dx:GridViewDataColumn>        
        <dx:GridViewDataColumn FieldName="PayrollName" Caption="Payroll Name" VisibleIndex="3" Width = "200px"><Settings AutoFilterCondition="Contains" /></dx:GridViewDataColumn>
        <dx:GridViewDataColumn FieldName="SetRemark" Caption="Set Remark" VisibleIndex="4" Visible="false" />
        <dx:GridViewDataColumn FieldName="SetCode" Caption="Set Code" VisibleIndex="5" Visible="false" />        
        <dx:GridViewCommandColumn ButtonType="Image" Caption="#" VisibleIndex="6" Width = "50px" >            
            <CustomButtons><dx:GridViewCommandColumnCustomButton ID="btnDelete"  Text = "Clear" Image-SpriteProperties-CssClass ="btngo" Image-Url ="../../Image/idelete.BMP" Image-ToolTip = "Clear" /> </CustomButtons>
        </dx:GridViewCommandColumn>
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
							<asp:TemplateColumn HeaderText="Setup Account" SortExpression="SetDescription">
								<Itemtemplate>
									<asp:Label Runat="server" ID="SetDescription" text='<%# DataBinder.Eval(Container.DataItem, "SetDescription") %>'>
									</asp:Label>
								</Itemtemplate>														
							</asp:TemplateColumn>
							
							<asp:TemplateColumn HeaderText="Account" SortExpression="SetValue">
								<Itemtemplate>
									<asp:Label Runat="server" ID="SetValue" text='<%# DataBinder.Eval(Container.DataItem, "SetValue") %>'>
									</asp:Label>
								</Itemtemplate>														
							</asp:TemplateColumn>		
							
							<asp:TemplateColumn HeaderText="Account Name" SortExpression="AccountName">
								<Itemtemplate>
									<asp:Label Runat="server" ID="AccountName" text='<%# DataBinder.Eval(Container.DataItem, "AccountName") %>'>
									</asp:Label>
								</Itemtemplate>								
							</asp:TemplateColumn>
							
							<asp:TemplateColumn HeaderText="SetRemark" Visible="false">
								<Itemtemplate>
									<asp:Label Runat="server" ID="SetRemark" text='<%# DataBinder.Eval(Container.DataItem, "SetRemark") %>'>
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
									&nbsp;
									<asp:Button CommandName="Delete" CssClass="Button" Text="Delete" ID="btnDelete" Runat="server" Width="55" />
								</ItemTemplate>								
							</asp:TemplateColumn>
							
    					</Columns>
        </asp:datagrid>--%>
      </asp:Panel>
     <asp:Panel ID="pnlInput" runat="server" Visible="false">
        <table>
            <tr>
                <td>Setup Payroll</td>
                <td>:</td>
                <td><asp:label ID="lbSetDescription" ForeColor="Blue" runat="server"/></td>                
            </tr>            
            <tr>
                <td>Payroll</td>
                <td>:</td>
                <td><asp:TextBox ID="tbPayroll" CssClass="TextBox" runat="server" Width="100px" AutoPostBack="True" /><asp:TextBox ID="tbPayrollName" CssClass="TextBox" Width="200px" runat="server" Enabled="false" />
                    <asp:Button ID="btnSearchPayroll" class="btngo" runat="server" Text="Search" 
                        Width="40px"/></td>                
            </tr>                        
            <tr>
                <td align="center" colspan="3"><asp:Button class="btngo" ID="btnSave" 
                        runat="server" Text="Save" Width="39px" /> <asp:Button ID="btnCancel" class="btngo" runat="server" Text="Cancel" Width="39px"/> <asp:Button ID="btnReset" CssClass="btngo" runat="server" Text="Reset" Width="39px"/></td>
            </tr>
        </table>
     </asp:Panel>   
     <asp:Label ID="lstatus" ForeColor="red" runat="server"></asp:Label>        
    </div>
    </form>
</body>
</html>
