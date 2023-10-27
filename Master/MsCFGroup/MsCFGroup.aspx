<%@ Page Language="VB" AutoEventWireup="false" CodeFile="MsCFGroup.aspx.vb" Inherits="Execute_Master_MsFlowType_MsFlowType" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Cash Flow Category File</title>
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
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div class="Content">
     <div class="H1">
            <table>
                <tr>
                    <td>
                        Cash Flow Group File
                    </td>   
                </tr>
            </table>    
       </div>
     <hr style="color:Blue" />
      <table>
        <tr>
            <td style="width:100px;text-align:right" >Quick Search :</td>
            <td><asp:TextBox runat="server" ID ="tbFilter" CssClass="TextBox"/> 
                <asp:DropDownList runat="server" ID="ddlField" CssClass="DropDownList" >
                  <asp:ListItem Selected="true" Text="Cash Flow Group Code" Value="CFGroupCode"></asp:ListItem>
                  <asp:ListItem Text="Cash Flow Group Name" Value="CFGroupName"></asp:ListItem>  
                  <asp:ListItem Value="CFCategory">CashFlowCategory</asp:ListItem>  
                  <asp:ListItem Value="FlowType">FlowType</asp:ListItem>   
                  <asp:ListItem Value="FgPreviousPeriod">FgPreviousPeriod</asp:ListItem>                     
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
                    <asp:ListItem Selected="true" Text="Cash Flow Group Code" Value="CFGroupCode"></asp:ListItem>
                    <asp:ListItem Text="Cash Flow Group Name" Value="CFGroupName"></asp:ListItem>  
                    <asp:ListItem Value="CFCategory">CashFlowCategory</asp:ListItem>  
                    <asp:ListItem Value="FlowType">FlowType</asp:ListItem>    
                    <asp:ListItem Value="FgPreviousPeriod">FgPreviousPeriod</asp:ListItem>                                            
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
							<asp:TemplateField HeaderText="Cash Flow Group Code" SortExpression="CFGroupCode">
								<Itemtemplate>
									<asp:Label Runat="server" ID="CFGroupCode" text='<%# DataBinder.Eval(Container.DataItem, "CFGroupCode") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:Label Runat="server" ID="CashFlowGroupCodeEdit" Text='<%# DataBinder.Eval(Container.DataItem, "CFGroupCode") %>'>
									</asp:Label>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="CashFlowGroupCodeAdd" CssClass="TextBox" MaxLength="10" Runat="Server" Width="100%"/>
									<cc1:TextBoxWatermarkExtender ID="CashFlowGroupCodeAdd_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="CashFlowGroupCodeAdd" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>
								</FooterTemplate>
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Cash Flow Group Name" HeaderStyle-Width="350" SortExpression="CFGroupName">
								<Itemtemplate>
									<asp:Label Runat="server" ID="CFGroupName" text='<%# DataBinder.Eval(Container.DataItem, "CFGroupName") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:TextBox Runat="server" CssClass="TextBox" ID="CashFlowGroupNameEdit" MaxLength="60" Width="100%" Text='<%# DataBinder.Eval(Container.DataItem, "CFGroupName") %>'>
									</asp:TextBox>
									<cc1:TextBoxWatermarkExtender ID="CashFlowGroupNameEdit_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="CashFlowGroupNameEdit" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="CashFlowGroupNameAdd" CssClass="TextBox" MaxLength="60" Runat="Server" Width="100%"/>
									<cc1:TextBoxWatermarkExtender ID="CashFlowGroupNameAdd_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="CashFlowGroupNameAdd" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>
								</FooterTemplate>
							</asp:TemplateField>	
							<asp:TemplateField HeaderText="Cash Flow Category" HeaderStyle-Width="250" SortExpression="CFCategory">
								<Itemtemplate>
									<asp:Label Runat="server" ID="CFCategory" text='<%# DataBinder.Eval(Container.DataItem, "CFCategory") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:DropDownList Runat="server" ID="CashFlowCategoryEdit" Width="100%" CssClass="DropDownList" 
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "CFCategory") %>' 
                                        DataSourceID="dsCFCategory" DataTextField="CFCategoryName" 
                                        DataValueField="CFCategoryCode">
									    
									</asp:DropDownList>								    
								</EditItemTemplate>
								<FooterTemplate>
									<asp:DropDownList ID="CashFlowCategoryAdd" Runat="Server" Width="100%" CssClass="DropDownList"									    
                                        DataSourceID="dsCFCategory" DataTextField="CFCategoryName" 
                                        DataValueField="CFCategoryCode">
									</asp:DropDownList>
								</FooterTemplate>
							</asp:TemplateField>	
							<asp:TemplateField HeaderText="Flow Type" HeaderStyle-Width="250" SortExpression="FlowType">
								<Itemtemplate>
									<asp:Label Runat="server" ID="FlowType" text='<%# DataBinder.Eval(Container.DataItem, "FlowType") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:DropDownList CssClass="DropDownList" Runat="server" Width="100%" ID="FlowTypeEdit" Text='<%# DataBinder.Eval(Container.DataItem, "FlowType") %>'>
									    <asp:ListItem>IN</asp:ListItem>
									    <asp:ListItem>OUT</asp:ListItem>
									</asp:DropDownList>							    
								</EditItemTemplate>
								<FooterTemplate>
									<asp:DropDownList CssClass="DropDownList" ID="FlowTypeAdd" Runat="Server" Width="100%">
									    <asp:ListItem Selected="True">IN</asp:ListItem>
									    <asp:ListItem>OUT</asp:ListItem>
									</asp:DropDownList>
								</FooterTemplate>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="FgPreviousPeriod" SortExpression="FgPreviousPeriod">
								<Itemtemplate>
									<asp:Label Runat="server" ID="FgPreviousPeriod" text='<%# DataBinder.Eval(Container.DataItem, "FgPreviousPeriod") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:DropDownList CssClass="DropDownList" Runat="server" Width="100%" ID="FgPreviousPeriodEdit" Text='<%# DataBinder.Eval(Container.DataItem, "FgPreviousPeriod") %>'>
									    <asp:ListItem>Y</asp:ListItem>
									    <asp:ListItem>N</asp:ListItem>
									</asp:DropDownList>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:DropDownList CssClass="DropDownList" ID="FgPreviousPeriodAdd" Runat="Server" Width="100%">
									    <asp:ListItem>Y</asp:ListItem>
									    <asp:ListItem Selected="True">N</asp:ListItem>
									</asp:DropDownList>
								</FooterTemplate>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Action" headerstyle-width="126" >
							    <ItemTemplate>
								    <asp:Button ID="btnEdit" runat="server" class="bitbtndt btnedit" Text="Edit" CommandName="Edit"/>
									<asp:Button ID="btndelete" runat="server" class="bitbtndt btndelete" Text="Delete" OnClientClick="return confirm('Sure to delete this data?');" CommandName="Delete"/>									
									<asp:Button runat="server" ID="btnDetail" CommandName="Detail" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" CssClass="bitbtn btndetail" Text="Detail" />
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
        </asp:GridView>       
        
        <asp:SqlDataSource ID="dsCFCategory" runat="server"                 
                SelectCommand="EXEC S_MsCFGroupGetCategory">
        </asp:SqlDataSource>
        
     <asp:Label ID="lstatus" ForeColor="red" runat="server"></asp:Label>        
    </div>
    </form>
</body>
</html>
