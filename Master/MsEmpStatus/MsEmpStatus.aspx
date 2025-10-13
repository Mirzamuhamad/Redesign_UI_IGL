<%@ Page Language="VB" AutoEventWireup="false" CodeFile="MsEmpStatus.aspx.vb" Inherits="Master_MsEmpStatus_MsEmpStatus" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Employee Status File</title>
    <script src="../../Function/OpenDlg.js" type="text/javascript"></script>
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
 <link href="https://fonts.googleapis.com/css2?family=Muli:wght@300;400;600&display=swap" rel="stylesheet">
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div class="Content">
     <div class="H1">Employee Status File</div>
     <hr style="color:Blue" />
      <table>
        <tr>
            <td style="width:100px;text-align:right" >Quick Search :</td>
            <td><asp:TextBox runat="server" ID ="tbFilter" CssClass="TextBox"/> 
                <asp:DropDownList runat="server" ID="ddlField" CssClass="DropDownList" >
                  <asp:ListItem Selected="true" Text="Employee Status Code" Value="EmpStatusCode"></asp:ListItem>
                  <asp:ListItem Text="Epmloyee Status Name" Value="EmpStatusName"></asp:ListItem> 
                  <asp:ListItem Text="Permanent" Value="FgPermanent"></asp:ListItem>                    
                  <asp:ListItem Text="Leave" Value="CanLeave"></asp:ListItem>
                  <asp:ListItem Text="PPh" Value="FgPPh"></asp:ListItem>
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
            <td><asp:TextBox runat="server" CssClass="TextBox ID ="tbfilter2"/> 
                  <asp:DropDownList runat="server" ID="ddlField2" CssClass="DropDownList" >
                    <asp:ListItem Selected="true" Text="Employee Status Code" Value="EmpStatusCode"></asp:ListItem>
                    <asp:ListItem Text="Employee Status Name" Value="EmpStatusName"></asp:ListItem> 
                    <asp:ListItem Text="Permanent" Value="FgPermanent"></asp:ListItem>                    
                    <asp:ListItem Text="Leave" Value="CanLeave"></asp:ListItem>
                    <asp:ListItem Text="PPh" Value="FgPPh"></asp:ListItem>
                  </asp:DropDownList>
                
            </td>
        </tr>
     </table>
     </asp:Panel>
      <br />
      <asp:GridView id="DataGrid" runat="server" 
            ShowFooter="True" AllowSorting="True" 
            AutoGenerateColumns="False" AllowPaging="True" CssClass="Grid">
						<HeaderStyle CssClass="GridHeader" Wrap = "False"></HeaderStyle>
						<RowStyle CssClass="GridItem" Wrap="false"/>
						<AlternatingRowStyle CssClass="GridAltItem"/>
						<FooterStyle CssClass="GridFooter" />
						<PagerStyle CssClass="GridPager" />
				      <Columns>
							<asp:TemplateField HeaderText="Employee Status Code" HeaderStyle-Width="100" SortExpression="EmpStatusCode">
								<Itemtemplate>
									<asp:Label Runat="server" ID="EmpStatusCode" text='<%# DataBinder.Eval(Container.DataItem, "EmpStatusCode") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:Label Runat="server" ID="EmpStatusCodeEdit" Text='<%# DataBinder.Eval(Container.DataItem, "EmpStatusCode") %>'>
								</asp:Label>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="EmpStatusCodeAdd" CssClass="TextBox" MaxLength="5" Runat="Server" Width="100%"/>
									<cc1:TextBoxWatermarkExtender ID="EmpStatusCodeAdd_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="EmpStatusCodeAdd" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>
								</FooterTemplate>
                            <HeaderStyle Width="100px"></HeaderStyle>
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Employee Status Name" HeaderStyle-Width="320" SortExpression="EmpStatusName">
								<Itemtemplate>
									<asp:Label Runat="server" ID="EmpStatusName" text='<%# DataBinder.Eval(Container.DataItem, "EmpStatusName") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:TextBox Runat="server" CssClass="TextBox" ID="EmpStatusNameEdit" MaxLength="60" Width="100%" Text='<%# DataBinder.Eval(Container.DataItem, "EmpStatusName") %>'>
									</asp:TextBox>
									<cc1:TextBoxWatermarkExtender ID="EmpStatusNameEdit_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="EmpStatusNameEdit" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="EmpStatusNameAdd" CssClass="TextBox" MaxLength="60" Runat="Server" Width="100%"/>
									<cc1:TextBoxWatermarkExtender ID="EmpStatusNameAdd_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="EmpStatusNameAdd" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>
								</FooterTemplate>
                            <HeaderStyle Width="320px"></HeaderStyle>
							</asp:TemplateField>	
							
							<asp:TemplateField HeaderText="Permanent" SortExpression="FgPermanent">
								<Itemtemplate>
									<asp:Label Runat="server" ID="FgPermanent" text='<%# DataBinder.Eval(Container.DataItem, "FgPermanent") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:DropDownList CssClass="DropDownList" Runat="server" ID="FgPermanentEdit" Width="100%" Text='<%# DataBinder.Eval(Container.DataItem, "FgPermanent") %>'>
									    <asp:ListItem>Y</asp:ListItem>
									    <asp:ListItem>N</asp:ListItem>
									</asp:DropDownList>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:DropDownList CssClass="DropDownList" ID="FgPermanentAdd" Runat="Server" Width="100%">
									    <asp:ListItem >Y</asp:ListItem>
									    <asp:ListItem Selected="True">N</asp:ListItem>
									</asp:DropDownList>
								</FooterTemplate>
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Leave" SortExpression="CanLeave">
								<Itemtemplate>
									<asp:Label Runat="server" ID="CanLeave" text='<%# DataBinder.Eval(Container.DataItem, "CanLeave") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:DropDownList CssClass="DropDownList" Runat="server" ID="CanLeaveEdit" Width="100%" Text='<%# DataBinder.Eval(Container.DataItem, "CanLeave") %>'>
									    <asp:ListItem>Y</asp:ListItem>
									    <asp:ListItem>N</asp:ListItem>
									</asp:DropDownList>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:DropDownList CssClass="DropDownList" ID="CanLeaveAdd" Runat="Server" Width="100%">
									    <asp:ListItem Selected="True">Y</asp:ListItem>
									    <asp:ListItem >N</asp:ListItem>
									</asp:DropDownList>
								</FooterTemplate>
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="PPh" SortExpression="FgPPh">
								<Itemtemplate>
									<asp:Label Runat="server" ID="FgPPh" text='<%# DataBinder.Eval(Container.DataItem, "FgPPh") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:DropDownList CssClass="DropDownList" Runat="server" ID="FgPPhEdit" Width="100%" Text='<%# DataBinder.Eval(Container.DataItem, "FgPPh") %>'>
									    <asp:ListItem>Y</asp:ListItem>
									    <asp:ListItem>N</asp:ListItem>
									</asp:DropDownList>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:DropDownList CssClass="DropDownList" ID="FgPPhAdd" Runat="Server" Width="100%">
									    <asp:ListItem >Y</asp:ListItem>
									    <asp:ListItem Selected="True">N</asp:ListItem>
									</asp:DropDownList>
								</FooterTemplate>
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="GrossUpPPH" SortExpression="FgGrossUp">
								<Itemtemplate>
									<asp:Label Runat="server" ID="FgGrossUp" text='<%# DataBinder.Eval(Container.DataItem, "FgGrossUp") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:DropDownList CssClass="DropDownList" Runat="server" ID="FgGrossUpEdit" Width="100%" Text='<%# DataBinder.Eval(Container.DataItem, "FgGrossUp") %>'>
									    <asp:ListItem>Y</asp:ListItem>
									    <asp:ListItem>N</asp:ListItem>
									</asp:DropDownList>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:DropDownList CssClass="DropDownList" ID="FgGrossUpAdd" Runat="Server" Width="100%">
									    <asp:ListItem >Y</asp:ListItem>
									    <asp:ListItem Selected="True">N</asp:ListItem>
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

<HeaderStyle Width="126px"></HeaderStyle>
							</asp:TemplateField>						
    					</Columns>
        </asp:GridView>
     <asp:Label ID="lstatus" ForeColor="red" runat="server"></asp:Label>        
    </div>
    </form>
</body>
</html>
