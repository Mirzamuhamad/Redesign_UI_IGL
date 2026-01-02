<%@ Page Language="VB" AutoEventWireup="false" CodeFile="MsCFCategory.aspx.vb" Inherits="Execute_Master_MsCashFlowType_MsCashFlowType" %>
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
 <link href="https://fonts.googleapis.com/css2?family=Muli:wght@300;400;600&display=swap" rel="stylesheet">
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div class="Content">
     <div class="H1">Cash Flow Category File</div>
     <hr style="color:Blue" />
      <table>
        <tr>
            <td style="width:100px;text-align:right" >Quick Search :</td>
            <td><asp:TextBox runat="server" ID ="tbFilter" CssClass="TextBox"/> 
                <asp:DropDownList runat="server" ID="ddlField" CssClass="DropDownList" >
                  <asp:ListItem Selected="true" Text="Cash Flow Category Code" Value="CFCategoryCode"></asp:ListItem>
                  <asp:ListItem Text="Cash Flow Category Name" Value="CFCategoryName"></asp:ListItem>  
                  <asp:ListItem Value="CFType">CashFlowType</asp:ListItem>                        
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
                    <asp:ListItem Selected="true" Text="Cash Flow Category Code" Value="CFCategoryCode"></asp:ListItem>
                    <asp:ListItem Text="Cash Flow Category Name" Value="CFCategoryName"></asp:ListItem> 
                    <asp:ListItem Value="CFType">CashFlowType</asp:ListItem>                                                
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
							<asp:TemplateField HeaderText="Cash Flow Category Code" SortExpression="CFCategoryCode">
								<Itemtemplate>
									<asp:Label Runat="server" ID="CFCategoryCode" text='<%# DataBinder.Eval(Container.DataItem, "CFCategoryCode") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:Label Runat="server" ID="CashFlowCategoryCodeEdit" Text='<%# DataBinder.Eval(Container.DataItem, "CFCategoryCode") %>'>
									</asp:Label>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="CashFlowCategoryCodeAdd" CssClass="TextBox" MaxLength="10" Runat="Server" Width="100%"/>
									<cc1:TextBoxWatermarkExtender ID="CashFlowCategoryCodeAdd_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="CashFlowCategoryCodeAdd" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>
								</FooterTemplate>
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Cash Flow Category Name" HeaderStyle-Width="350" SortExpression="CFCategoryName">
								<Itemtemplate>
									<asp:Label Runat="server" ID="CFCategoryName" text='<%# DataBinder.Eval(Container.DataItem, "CFCategoryName") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:TextBox Runat="server" CssClass="TextBox" ID="CashFlowCategoryNameEdit" MaxLength="60" Width="100%" Text='<%# DataBinder.Eval(Container.DataItem, "CFCategoryName") %>'>
									</asp:TextBox>
									<cc1:TextBoxWatermarkExtender ID="CashFlowCategoryNameEdit_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="CashFlowCategoryNameEdit" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="CashFlowCategoryNameAdd" CssClass="TextBox" MaxLength="60" Runat="Server" Width="100%"/>
									<cc1:TextBoxWatermarkExtender ID="CashFlowCategoryNameAdd_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="CashFlowCategoryNameAdd" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>
								</FooterTemplate>
							</asp:TemplateField>		
							<asp:TemplateField HeaderText="Cash Flow Type" HeaderStyle-Width="250" SortExpression="CFType">
								<Itemtemplate>
									<asp:Label Runat="server" ID="CFType" text='<%# DataBinder.Eval(Container.DataItem, "CFType") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:DropDownList Runat="server" ID="CashFlowTypeEdit" Width="100%" CssClass="DropDownList" 
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "CFType") %>' 
                                        DataSourceID="dsCFType" DataTextField="CFTypeName" 
                                        DataValueField="CFTypeCode">
									    
									</asp:DropDownList>								    
								</EditItemTemplate>
								<FooterTemplate>
									<asp:DropDownList ID="CashFlowTypeAdd" Runat="Server" Width="100%" CssClass="DropDownList"									    
                                        DataSourceID="dsCFType" DataTextField="CFTypeName" 
                                        DataValueField="CFTypeCode">
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
        </asp:GridView>       
        
        <asp:SqlDataSource ID="dsCFType" runat="server"                 
                SelectCommand="EXEC S_MsCFCategoryGetCFType">
        </asp:SqlDataSource>
        
     <asp:Label ID="lstatus" ForeColor="red" runat="server"></asp:Label>        
    </div>
    </form>
</body>
</html>
