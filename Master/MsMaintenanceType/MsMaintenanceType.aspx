<%@ Page Language="VB" AutoEventWireup="false" CodeFile="MsMaintenanceType.aspx.vb" Inherits="Master_MsMaintenanceType_MsMaintenanceType" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitle</title>
    <script src="../../Function/OpenDlg.js" type="text/javascript"></script>
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
 <link href="https://fonts.googleapis.com/css2?family=Muli:wght@300;400;600&display=swap" rel="stylesheet">
    <style type="text/css">
        .style1
        {
            width: 22px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div class="Content">
     <div class="H1">Maintenance Type File</div>
     <hr style="color:Blue" />
      <table>
        <tr>
            <td style="width:100px;text-align:right" >Quick Search :</td>
            <td><asp:TextBox runat="server" ID ="tbFilter" CssClass="TextBox"/> 
                <asp:DropDownList runat="server" ID="ddlField" CssClass="DropDownList" >
                  <asp:ListItem Selected="true" Text="Maintenance Type Code" Value="MaintenanceTypeCode"></asp:ListItem>
                  <asp:ListItem Text="Maintenance Type Name" Value="MaintenancetypeName"></asp:ListItem> 
                  <asp:ListItem Text="Maintenance Section" Value="MTNSectionName"></asp:ListItem>        
                </asp:DropDownList>     
                <asp:Button class="bitbtn btnsearch" runat="server" ID="btnSearch" Text="Search" />
                <asp:Button class="btngo" runat="server" ID="btnExpand" Text="..."/>
                <asp:Button class="bitbtn btnprint" runat="server" ID="btnPrint" Text="Print"/>
            </td>
            <td class="style1">&nbsp;</td>
            <td>Show Records :</td>
            <td><asp:DropDownList ID="ddlRow" runat="server" CssClass="DropDownList" 
                    AutoPostBack="True">
                <asp:ListItem Selected="True" Value = "15">Choose One</asp:ListItem>
                <asp:ListItem>20</asp:ListItem>
                <asp:ListItem>30</asp:ListItem>
                <asp:ListItem>40</asp:ListItem>
                <asp:ListItem>50</asp:ListItem>
                <asp:ListItem>100</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td>Rows</td>
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
                    <asp:ListItem Selected="true" Text="Maintenance Type Code" Value="MaintenanceTypeCode"></asp:ListItem>
                    <asp:ListItem Text="Maintenance Type Name" Value="MaintenanceTypeName"></asp:ListItem> 
                    <asp:ListItem Text="Maintenance Section" Value="MTNSectionName"></asp:ListItem> 
                  </asp:DropDownList>
                
            </td>
        </tr>
     </table>
     </asp:Panel>
      <br />
      <asp:GridView id="DataGrid" runat="server" 
            ShowFooter="True" AllowSorting="True" 
            AutoGenerateColumns="False" AllowPaging="True" CssClass="Grid">
						<HeaderStyle CssClass="GridHeader" Wrap="false"></HeaderStyle>
						<RowStyle CssClass="GridItem" Wrap="false"/>
						<AlternatingRowStyle CssClass="GridAltItem"/>
						<FooterStyle CssClass="GridFooter" />
						<PagerStyle CssClass="GridPager" />
				      <Columns>
				            <asp:TemplateField HeaderText="Maintenance Type Code" HeaderStyle-Width="100" SortExpression="MaintenanceTypeCode">
								<Itemtemplate>
									<asp:Label Runat="server" ID="MaintenanceTypeCode" text='<%# DataBinder.Eval(Container.DataItem, "MaintenanceTypeCode") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:Label Runat="server" ID="MaintenanceTypeCodeEdit" Text='<%# DataBinder.Eval(Container.DataItem, "MaintenanceTypeCode") %>'>
									</asp:Label>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="MaintenanceTypeCodeAdd" CssClass="TextBox" MaxLength="5" Runat="Server" Width="100%"/>
									<cc1:TextBoxWatermarkExtender ID="MaintenanceTypeCodeAdd_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="MaintenanceTypeCodeAdd" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>
								</FooterTemplate>
                                <HeaderStyle Width="100px"></HeaderStyle>
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Maintenance Type Name" HeaderStyle-Width="320" SortExpression="MaintenanceTypeName">
								<Itemtemplate>
									<asp:Label Runat="server" ID="MaintenanceTypeName" text='<%# DataBinder.Eval(Container.DataItem, "MaintenanceTypeName") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:TextBox Runat="server" CssClass="TextBox" ID="MaintenanceTypeNameEdit" MaxLength="60" Width="100%" Text='<%# DataBinder.Eval(Container.DataItem, "MaintenanceTypeName") %>'>
									</asp:TextBox>
									<cc1:TextBoxWatermarkExtender ID="MaintenanceTypeNameEdit_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="MaintenanceTypeNameEdit" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="MaintenanceTypeNameAdd" CssClass="TextBox" MaxLength="60" Runat="Server" Width="100%"/>
									<cc1:TextBoxWatermarkExtender ID="MaintenanceTypeNameAdd_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="MaintenanceTypeNameAdd" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>
								</FooterTemplate>
                            <HeaderStyle Width="320px"></HeaderStyle>
							</asp:TemplateField>	
							
							<asp:TemplateField HeaderText="Maintenance Section" HeaderStyle-Width="220" SortExpression="MaintenanceSection">
								<Itemtemplate>
								    <asp:Label Runat="server" ID="MTNSectionName" text='<%# DataBinder.Eval(Container.DataItem, "MTNSectionName") %>'>
									</asp:Label>
									<asp:Label Runat="server" ID="MaintenanceSection" text='<%# DataBinder.Eval(Container.DataItem, "MaintenanceSection") %>' Visible = "false">
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:DropDownList Runat="server" ID="MaintenanceSectionEdit" CssClass="DropDownList" Width="100%" 
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "MaintenanceSection") %>' 
                                        DataSourceID="dsGetSection" DataTextField="MTNSectionName" 
                                        DataValueField="MTNSectionCode">									    
									</asp:DropDownList>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:DropDownList ID="MaintenanceSectionAdd" Runat="Server" Width="100%" CssClass="DropDownList"									    
                                        DataSourceID="dsGetSection" DataTextField="MTNSectionName" 
                                        DataValueField="MTNSectionCode">
									</asp:DropDownList>
								</FooterTemplate>
							</asp:TemplateField>		
														
							<asp:TemplateField HeaderText="Action" headerstyle-width="126" >
								<ItemTemplate>
								    <asp:Button ID="btnEdit" runat="server" class="bitbtndt btnedit" Text="Edit" CommandName="Edit"/>
									<asp:Button ID="btndelete" runat="server" class="bitbtndt btndelete" Text="Delete" OnClientClick="return confirm('Sure to delete this data?');" CommandName="Delete"/>									
									
									<asp:Button ID="btnDetail" runat="server" class="bitbtn btndetail" Text="User" CommandName="Detail" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>"/>	
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
        <asp:SqlDataSource ID="dsGetSection" runat="server" SelectCommand="SELECT MTNSectionCode, MTNSectionName FROM VMsMaintenanceSection" ></asp:SqlDataSource>       
     <asp:Label ID="lstatus" ForeColor="red" runat="server"></asp:Label>        
    </div>
    </form>
</body>
</html>
