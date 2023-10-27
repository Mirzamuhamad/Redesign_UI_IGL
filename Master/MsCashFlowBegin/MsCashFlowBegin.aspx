<%@ Page Language="VB" AutoEventWireup="false" CodeFile="MsCashFlowBegin.aspx.vb" Inherits="Execute_Master_MsCashFlowBegin_MsCashFlowBegin" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Cash Flow</title>
    <script src="../../Function/OpenDlg.js" type="text/javascript"></script>
    <script src="../../Function/Function.JS" type="text/javascript"></script>
    
    <script type="text/javascript">    
    function openprintdlg() {
            var wOpens;
            wOpens = window.open("../../Rpt/PrintForm.Aspx", "List", "scrollbars=yes,resizable=yes,width=500,height=400");
            wOpens.moveTo(0, 0);
            wOpens.resizeTo(screen.width, screen.height);
     )      
    </script>
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div class="Content">
     <div class="H1">Cash Flow</div>
     <hr style="color:Blue" />
      <table>
        <tr>
            <td style="width:100px;text-align:right" >Quick Search :</td>
            <td><asp:TextBox runat="server" ID ="tbFilter" CssClass="TextBox"/> 
                <asp:DropDownList runat="server" ID="ddlField" CssClass="DropDownList" >
                    <asp:ListItem Selected="true" Text="Year" Value="Year"></asp:ListItem>
                    <asp:ListItem Text="Month" Value="Month"></asp:ListItem>        
                    <asp:ListItem Value="Total">Total</asp:ListItem>
                </asp:DropDownList>     
                <asp:Button class="bitbtn btnsearch" runat="server" ID="btnSearch" Text="Search" />
                <asp:Button class="btngo" runat="server" ID="btnExpand" Text="..."/>
                <asp:Button class="bitbtn btnprint" runat="server" ID="btnPrint" Text="Print"/>
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
                    <asp:ListItem Selected="true" Text="Year" Value="Year"></asp:ListItem>
                    <asp:ListItem Text="Month" Value="Month"></asp:ListItem>        
                    <asp:ListItem Value="Total">Total</asp:ListItem>
                  </asp:DropDownList>
                
            </td>
        </tr>
     </table>
     </asp:Panel>
      <br />
      <asp:GridView id="DataGrid" runat="server" ShowFooter="True" AllowSorting="True" 
            AutoGenerateColumns="False" AllowPaging="True" CssClass="Grid">
						<HeaderStyle CssClass="GridHeader"></HeaderStyle>
						<RowStyle CssClass="GridItem" Wrap="false"/>
						<AlternatingRowStyle CssClass="GridAltItem"/>
						<FooterStyle CssClass="GridFooter" />
						<PagerStyle CssClass="GridPager" />
				      <Columns>
							<asp:TemplateField HeaderText="Year" HeaderStyle-Width="80" SortExpression="Year">
								<Itemtemplate>
									<asp:Label Runat="server" ID="Year" text='<%# DataBinder.Eval(Container.DataItem, "Year") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
								    <asp:DropDownList CssClass="DropDownList" Width="100%" ID="ddlYearEditTemp" runat="server"
								        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Year") %>'
								        DataSourceID="dsYear" DataTextField="Year" DataValueField="Year" Visible="false">
								    </asp:DropDownList>
								    <asp:DropDownList CssClass="DropDownList" Width="100%" ID="ddlYearEdit" runat="server"
								        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Year") %>'
								        DataSourceID="dsYear" DataTextField="Year" DataValueField="Year">
								    </asp:DropDownList>
								    <%--SelectedValue='<%# DataBinder.Eval(Container.DataItem, "setvalue") %>'--%>								
								</EditItemTemplate>
								<FooterTemplate>
									<asp:DropDownList CssClass="DropDownList" Width="100%" ID="ddlYearAdd" runat="server"
								        DataSourceID="dsYear" DataTextField="Year" DataValueField="Year">
								    </asp:DropDownList>
								</FooterTemplate>
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Month" HeaderStyle-Width="80" SortExpression="Month">
								<Itemtemplate>
								    <asp:Label Runat="server" ID="PeriodTemp" text='<%# DataBinder.Eval(Container.DataItem, "Month") %>' Visible="false">
									</asp:Label>
									<asp:Label Runat="server" ID="Period" text='<%# DataBinder.Eval(Container.DataItem, "MonthName") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
								    <asp:DropDownList CssClass="DropDownList" Width="100%" ID="ddlPeriodEditTemp" runat="server"
								        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Month") %>'
								        DataSourceID="dsPeriod" DataTextField="Description" DataValueField="Period" Visible="false">
								    </asp:DropDownList>
								    <asp:DropDownList CssClass="DropDownList" Width="100%" ID="ddlPeriodEdit" runat="server"
								        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Month") %>'
								        DataSourceID="dsPeriod" DataTextField="Description" DataValueField="Period">
								    </asp:DropDownList>
								    <%--SelectedValue='<%# DataBinder.Eval(Container.DataItem, "setvalue") %>'--%>								
								</EditItemTemplate>
								<FooterTemplate>
									<asp:DropDownList CssClass="DropDownList" Width="100%" ID="ddlPeriodAdd" runat="server"
								        DataSourceID="dsPeriod" DataTextField="Description" DataValueField="Period">
								    </asp:DropDownList>
								</FooterTemplate>
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Total" HeaderStyle-Width="100" SortExpression="Total">
								<Itemtemplate>
									<asp:Label Runat="server" ID="Total" text='<%# DataBinder.Eval(Container.DataItem, "Total") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:TextBox ID="TotalEdit" Runat="server" CssClass="TextBox" MaxLength="13" Width="100%" Text='<%# DataBinder.Eval(Container.DataItem, "Total") %>'>
									</asp:TextBox>
									<cc1:TextBoxWatermarkExtender ID="TotalEdit_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="TotalEdit" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="TotalAdd" CssClass="TextBox" MaxLength="13" Runat="Server" Width="100%"/>
									<cc1:TextBoxWatermarkExtender ID="TotalAdd_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="TotalAdd" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>
								</FooterTemplate>
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Action" headerstyle-width="126" >
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
							</asp:TemplateField>
    					</Columns>
        </asp:gridView>       
     <asp:Label ID="lstatus" ForeColor="red" runat="server"></asp:Label>        
    </div>
    
    <asp:SqlDataSource ID="dsYear" runat="server" SelectCommand="select * from GlYear">
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="dsPeriod" runat="server" SelectCommand="select * from GlPeriod where Period Between 1 And 12">
    </asp:SqlDataSource>
    </form>
</body>
</html>
