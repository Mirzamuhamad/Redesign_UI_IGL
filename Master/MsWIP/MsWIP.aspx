<%@ Page Language="VB" AutoEventWireup="false" CodeFile="MsWIP.aspx.vb" Inherits="Execute_Master_MsWIP_MsWIP" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>WIP</title>
    <script src="../../Function/OpenDlg.js" type="text/javascript"></script>
    <script type="text/javascript">    
    function openprintdlg() {
            var wOpens;
            wOpens = window.open("../../Rpt/PrintForm.Aspx", "List", "scrollbars=yes,resizable=yes,width=500,height=400");
            wOpens.moveTo(0, 0);
            wOpens.resizeTo(screen.width, screen.height);
     )      
    </script>
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
 <link href="https://fonts.googleapis.com/css2?family=Muli:wght@300;400;600&display=swap" rel="stylesheet">
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div class="Content">
     <div class="H1">WIP</div>
     <hr style="color:Blue" />
      <table>
        <tr>
            <td style="width:100px;text-align:right" >Quick Search :</td>
            <td><asp:TextBox runat="server" ID ="tbFilter" CssClass="TextBox"/> 
                <asp:DropDownList runat="server" ID="ddlField" CssClass="DropDownList" >
                    <asp:ListItem Selected="true" Text="WIP Code" Value="WIPCode"></asp:ListItem>
                    <asp:ListItem Text="WIP Name" Value="WIPName"></asp:ListItem>        
                    <asp:ListItem Value="WIPType">WIP Type</asp:ListItem>
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
                    <asp:ListItem Selected="true" Text="WIP Code" Value="WIPCode"></asp:ListItem>
                    <asp:ListItem Text="WIP Name" Value="WIPName"></asp:ListItem>        
                    <asp:ListItem Value="WIPType">WIP Type</asp:ListItem>
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
							<asp:TemplateField HeaderText="WIP Code" SortExpression="WIPCode">
								<Itemtemplate>
									<asp:Label Runat="server" ID="WIPCode" text='<%# DataBinder.Eval(Container.DataItem, "WIPCode") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:Label Runat="server" ID="WIPCodeEdit" Text='<%# DataBinder.Eval(Container.DataItem, "WIPCode") %>'>
									</asp:Label>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="WIPCodeAdd" CssClass="TextBox" MaxLength="20" Runat="Server" Width="100%"/>
									<cc1:TextBoxWatermarkExtender ID="WIPCodeAdd_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="WIPCodeAdd" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>
								</FooterTemplate>
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="WIP Name" HeaderStyle-Width="350" SortExpression="WIPName">
								<Itemtemplate>
									<asp:Label Runat="server" ID="WIPName" text='<%# DataBinder.Eval(Container.DataItem, "WIPName") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:TextBox ID="WIPNameEdit" Runat="server" CssClass="TextBox" MaxLength="60" Width="100%" Text='<%# DataBinder.Eval(Container.DataItem, "WIPName") %>'>
									</asp:TextBox>
									<cc1:TextBoxWatermarkExtender ID="WIPNameEdit_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="WIPNameEdit" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="WIPNameAdd" CssClass="TextBox" MaxLength="60" Runat="Server" Width="100%"/>
									<cc1:TextBoxWatermarkExtender ID="WIPNameAdd_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="WIPNameAdd" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>
								</FooterTemplate>
							</asp:TemplateField>
							
							
							
							<asp:TemplateField HeaderText="WIP Type" SortExpression="WIPType">
								<Itemtemplate>
									<asp:Label Runat="server" ID="WIPType" text='<%# DataBinder.Eval(Container.DataItem, "WIPType") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:DropDownList ID="WIPTypeEdit" CssClass="DropDownList" Runat="server" Width="100%" Text='<%# DataBinder.Eval(Container.DataItem, "WIPType") %>'>
									    <asp:ListItem>FG</asp:ListItem>
									    <asp:ListItem>None</asp:ListItem>
									</asp:DropDownList>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:DropDownList ID="WIPTypeAdd" CssClass="DropDownList" Runat="Server" Width="100%">
									    <asp:ListItem Selected="True">FG</asp:ListItem>
									    <asp:ListItem>None</asp:ListItem>
									</asp:DropDownList>
								</FooterTemplate>
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Unit" HeaderStyle-Width="150" SortExpression="UnitName">
								<Itemtemplate>
									<asp:Label Runat="server" ID="Unit" text='<%# DataBinder.Eval(Container.DataItem, "UnitName") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
								    <asp:DropDownList CssClass="DropDownList" Width="100%" ID="ddlUnitEdit" runat="server"
								        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Unit") %>'
								        DataSourceID="dsUnit" DataTextField="Unit_Name" 
                                        DataValueField="Unit_Code">
								    </asp:DropDownList>
								    <%--SelectedValue='<%# DataBinder.Eval(Container.DataItem, "setvalue") %>'--%>								
								</EditItemTemplate>
								<FooterTemplate>
									<asp:DropDownList CssClass="DropDownList" Width="100%" ID="ddlUnitAdd" runat="server"
								        DataSourceID="dsUnit" DataTextField="Unit_Name" 
                                        DataValueField="Unit_Code">
								    </asp:DropDownList>
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
    
    <asp:SqlDataSource ID="dsUnit" runat="server" SelectCommand="EXEC S_GetUnit">
        </asp:SqlDataSource>
    </form>
</body>
</html>
