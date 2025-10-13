<%@ Page Language="VB" AutoEventWireup="false" CodeFile="MsAccountSetup.aspx.vb" Inherits="Master_MsAccountSetup_MsAccountSetup" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitle</title>
    <script src="../../Function/OpenDlg.js" type="text/javascript"></script>
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
 <link href="https://fonts.googleapis.com/css2?family=Muli:wght@300;400;600&display=swap" rel="stylesheet">
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div class="Content">
     <div class="H1">Account Classification</div>
     <hr style="color:Blue" />
      <table>
        <tr>
            <td style="width:100px;text-align:right" >Quick Search :</td>
            <td><asp:TextBox runat="server" ID ="tbFilter" CssClass="TextBox"/> 
                <asp:DropDownList runat="server" ID="ddlField" CssClass="DropDownList" >
                  <asp:ListItem Selected="true" Text="Group Rpt" Value="GroupAlias"></asp:ListItem>
                  <asp:ListItem Text="Type" Value="Type"></asp:ListItem>        
                  <asp:ListItem Text="Group Code" Value="GroupCode"></asp:ListItem>  
                  <asp:ListItem Text="Group Name" Value="GroupName"></asp:ListItem>  
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
            <td><asp:TextBox runat="server" CssClass="TextBox" ID ="tbfilter2"/> 
                  <asp:DropDownList runat="server" ID="ddlField2" CssClass="DropDownList" >
                    <asp:ListItem Selected="true" Text="Group Rpt" Value="GroupAlias"></asp:ListItem>
                    <asp:ListItem Text="Type" Value="Type"></asp:ListItem>        
                    <asp:ListItem Text="Group Code" Value="GroupCode"></asp:ListItem>  
                    <asp:ListItem Text="Group Name" Value="GroupName"></asp:ListItem>  
                  </asp:DropDownList>
                
            </td>
        </tr>
     </table>
     </asp:Panel>
      <br />
      <asp:GridView id="DataGrid" runat="server" Width="950"
            ShowFooter="True" AllowSorting="True" 
            AutoGenerateColumns="False" AllowPaging="True" CssClass="Grid">
						<HeaderStyle CssClass="GridHeader"></HeaderStyle>
						<RowStyle CssClass="GridItem" Wrap="false"/>
						<AlternatingRowStyle CssClass="GridAltItem"/>
						<FooterStyle CssClass="GridFooter" />
						<PagerStyle CssClass="GridPager" />
				      <Columns>
							<asp:TemplateField HeaderText="Setup Code" Visible="false" HeaderStyle-Width="120" SortExpression="GroupRpt">
								<Itemtemplate>
									<asp:Label Runat="server" ID="GrpCode" text='<%# DataBinder.Eval(Container.DataItem, "GroupRpt") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:Label Runat="server" ID="ddlGrpCodeEdit" Text='<%# DataBinder.Eval(Container.DataItem, "GroupRpt") %>'>
									</asp:Label>
								</EditItemTemplate>								
                                <HeaderStyle Width="120px"></HeaderStyle>
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Group Report" HeaderStyle-Width="250px" SortExpression="GroupRpt">
								<Itemtemplate>
									<asp:Label Runat="server" ID="GroupRpt" text='<%# DataBinder.Eval(Container.DataItem, "GroupAlias") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:Label Runat="server" ID="ddlGroupRptEdit" Text='<%# DataBinder.Eval(Container.DataItem, "GroupAlias") %>'>
									</asp:Label>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:DropDownList ID="ddlGroupRptAdd" Runat="Server" Width="100%" CssClass="DropDownList"									    
                                        DataSourceID="dsGroupRpt" DataTextField="GroupAlias" 
                                        DataValueField="GroupRpt">
									</asp:DropDownList>
								</FooterTemplate>
                                <HeaderStyle Width="250px"></HeaderStyle>
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Type" HeaderStyle-Width="80" SortExpression="Type">
								<Itemtemplate>
									<asp:Label Runat="server" ID="Type" text='<%# DataBinder.Eval(Container.DataItem, "Type") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:DropDownList CssClass="DropDownList" Runat="server" ID="ddlTypeEdit" Width="100%" OnSelectedIndexChanged = "ddlType_SelectedIndexChange" AutoPostBack = "true" Text='<%# DataBinder.Eval(Container.DataItem, "Type") %>'>
									    <asp:ListItem>Class</asp:ListItem>
									    <asp:ListItem>Sub Group</asp:ListItem>
									    <asp:ListItem>Group</asp:ListItem>
									    <asp:ListItem>Type</asp:ListItem>
									</asp:DropDownList>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:DropDownList CssClass="DropDownList" ID="ddlTypeAdd" Runat="Server" Width="100%" OnSelectedIndexChanged = "ddlType_SelectedIndexChange" AutoPostBack = "true">
									    <asp:ListItem Selected ="True" >Class</asp:ListItem>
									    <asp:ListItem>Sub Group</asp:ListItem>
									    <asp:ListItem>Group</asp:ListItem>
									    <asp:ListItem>Type</asp:ListItem>
									</asp:DropDownList>
								</FooterTemplate>
                                <HeaderStyle Width="80px"></HeaderStyle>
							</asp:TemplateField>		
							
							<asp:TemplateField HeaderText="Group Code" HeaderStyle-Width="90px" SortExpression="GroupCode">
								<Itemtemplate>
									<asp:Label Runat="server" ID="GroupCode" text='<%# DataBinder.Eval(Container.DataItem, "GroupCode") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:TextBox Runat="server" ID="GroupCodeEdit" MaxLength="12" Width="65px" CssClass="TextBox" AutoPostBack = "true" ontextchanged="tbGroupCode_TextChanged" Text='<%# DataBinder.Eval(Container.DataItem, "GroupCode") %>'/>
									<cc1:TextBoxWatermarkExtender ID="GroupCodeEdit_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="GroupCodeEdit" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                    </cc1:TextBoxWatermarkExtender>
                                    <asp:Button class="btngo" runat="server" ID="btnGroupEdit" Text="..." CommandName="SearchEdit"/>                                         						    
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="GroupCodeAdd" CssClass="TextBox" MaxLength="12" Width="65px" Runat="Server" AutoPostBack = "true" ontextchanged="tbGroupCode_TextChanged"  />
									<cc1:TextBoxWatermarkExtender ID="GroupCodeAdd_WtExt"
                                        runat="server" Enabled="True" TargetControlID="GroupCodeAdd" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                    </cc1:TextBoxWatermarkExtender>
                                    <asp:Button class="btngo" runat="server" ID="btnGroupAdd" Text="..." CommandName="SearchAdd"/>
								</FooterTemplate>                                
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Group Name" HeaderStyle-Width="250" SortExpression="GroupName">
								<Itemtemplate>
									<asp:Label Runat="server" ID="GroupName" text='<%# DataBinder.Eval(Container.DataItem, "GroupName") %>'>
									</asp:Label>
								</Itemtemplate>	
								<EditItemTemplate>
									<asp:Label Runat="server" ID="GroupNameEdit" Text='<%# DataBinder.Eval(Container.DataItem, "GroupName") %>'>
									</asp:Label>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:Label Runat="server" ID="GroupNameAdd">
									</asp:Label>
								</FooterTemplate>							
							</asp:TemplateField>	
							
							<asp:TemplateField HeaderText="Action" headerstyle-width="110" >
								<ItemTemplate>
								    <asp:Button class="bitbtn btnedit" runat="server" ID="btnEdit" Text="Edit" CommandName="Edit"/>
									<asp:Button class="bitbtn btndelete" runat="server" ID="btnDelete" Text="Delete" OnClientClick="return confirm('Sure to delete this data?');" CommandName="Delete"/>									
								</ItemTemplate>
								<EditItemTemplate>
									<asp:Button class="bitbtn btnsave" runat="server" ID="btnUpdate" Text="Save" CommandName="Update"/>
									<asp:Button class="bitbtn btncancel" runat="server" ID="btnCancel" Text="Cancel" CommandName="Cancel"/>
								</EditItemTemplate>
								<FooterTemplate>
								    <asp:Button class="bitbtn btnadd" runat="server" ID="btnAdd" Text="Add" CommandName="Insert"/>									
								</FooterTemplate>
                                <HeaderStyle Width="110px"></HeaderStyle>
							</asp:TemplateField>
							
    					</Columns>
        </asp:GridView>
        <asp:SqlDataSource ID="dsGroupRpt" runat="server" SelectCommand="SELECT * FROM VMsGroupRpt">
        </asp:SqlDataSource>

     <asp:Label ID="lstatus" ForeColor="red" runat="server"></asp:Label>        
    </div>
    </form>
</body>
</html>
