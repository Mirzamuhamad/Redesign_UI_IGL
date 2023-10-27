<%@ Page Language="VB" AutoEventWireup="false" CodeFile="MsLevelPlant.aspx.vb" Inherits="Master_MsLevelPlant_MsLevelPlant" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Level Plantation File</title>
    <script src="../../Function/OpenDlg.js" type="text/javascript"></script>
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div class="Content">
     <div class="H1">Level Plantation File</div>
     <hr style="color:Blue" />
      <table>
        <tr>
            <td style="width:100px;text-align:right" >Quick Search :</td>
            <td><asp:TextBox runat="server" ID ="tbFilter" CssClass="TextBox"/> 
                <asp:DropDownList runat="server" ID="ddlField" CssClass="DropDownList" >
                  <asp:ListItem Selected="true" Text="Code" Value="LevelPlantCode"></asp:ListItem>
                  <asp:ListItem Text="Name" Value="LevelPlantName"></asp:ListItem>        
                  <asp:ListItem Text="Alias" Value="LevelPlantAlias"></asp:ListItem>
                  <asp:ListItem Text="MPS" Value="FgMPS"></asp:ListItem>         
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
                    <asp:ListItem Selected="true" Text="Code" Value="LevelPlantCode"></asp:ListItem>
                    <asp:ListItem Text="Description" Value="LevelPlantName"></asp:ListItem> 
                    <asp:ListItem Text="Alias" Value="LevelPlantAlias"></asp:ListItem>
                    <asp:ListItem Text="MPS" Value="FgMPS"></asp:ListItem>        
                  </asp:DropDownList>
                
            </td>
        </tr>
     </table>
     </asp:Panel>
      <br />
      <asp:GridView id="DataGrid" runat="server" 
            ShowFooter="True" AllowSorting="True" 
            AutoGenerateColumns="False" AllowPaging="True" CssClass="Grid">
						<HeaderStyle CssClass="GridHeader"></HeaderStyle>
						<RowStyle CssClass="GridItem" Wrap="false"/>
						<AlternatingRowStyle CssClass="GridAltItem"/>
						<FooterStyle CssClass="GridFooter" />
						<PagerStyle CssClass="GridPager" />
				      <Columns>
							<asp:TemplateField HeaderText="Level Plantation Code" HeaderStyle-Width="100" SortExpression="LevelPlantCode">
								<Itemtemplate>
									<asp:Label Runat="server" ID="LevelPlantCode" text='<%# DataBinder.Eval(Container.DataItem, "LevelPlantCode") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:Label Runat="server" ID="LevelPlantCodeEdit" Text='<%# DataBinder.Eval(Container.DataItem, "LevelPlantCode") %>'>
									</asp:Label>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="LevelPlantCodeAdd" CssClass="TextBox" MaxLength="3" Runat="Server" Width="100%"/>
									<cc1:TextBoxWatermarkExtender ID="LevelPlantCodeAdd_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="LevelPlantCodeAdd" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>
								</FooterTemplate>

<HeaderStyle Width="100px"></HeaderStyle>
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Level Plantation Name" HeaderStyle-Width="320" SortExpression="LevelPlantName">
								<Itemtemplate>
									<asp:Label Runat="server" ID="LevelPlantName" text='<%# DataBinder.Eval(Container.DataItem, "LevelPlantName") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:TextBox Runat="server" CssClass="TextBox" ID="LevelPlantNameEdit" MaxLength="60" Width="100%" Text='<%# DataBinder.Eval(Container.DataItem, "LevelPlantName") %>'>
									</asp:TextBox>
									<cc1:TextBoxWatermarkExtender ID="LevelPlantNameEdit_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="LevelPlantNameEdit" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="LevelPlantNameAdd" CssClass="TextBox" MaxLength="60" Runat="Server" Width="100%"/>
									<cc1:TextBoxWatermarkExtender ID="LevelPlantNameAdd_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="LevelPlantNameAdd" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>
								</FooterTemplate>

<HeaderStyle Width="320px"></HeaderStyle>
							</asp:TemplateField>		
							
							<asp:TemplateField HeaderText="Alias" HeaderStyle-Width="120" SortExpression="LevelPlantAlias">
								<Itemtemplate>
									<asp:Label Runat="server" ID="LevelPlantAlias" text='<%# DataBinder.Eval(Container.DataItem, "LevelPlantAlias") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:TextBox Runat="server" CssClass="TextBox" ID="LevelPlantAliasEdit" MaxLength="5" Width="100%" Text='<%# DataBinder.Eval(Container.DataItem, "LevelPlantAlias") %>'>
									</asp:TextBox>
									<cc1:TextBoxWatermarkExtender ID="LevelPlantAliasEdit_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="LevelPlantAliasEdit" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="LevelPlantAliasAdd" CssClass="TextBox" MaxLength="5" Runat="Server" Width="100%"/>
									<cc1:TextBoxWatermarkExtender ID="LevelPlantAliasAdd_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="LevelPlantAliasAdd" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>
								</FooterTemplate>
<HeaderStyle Width="320"></HeaderStyle>
							</asp:TemplateField>
								
								<asp:TemplateField HeaderText="MPS" HeaderStyle-Width="80" ItemStyle-HorizontalAlign="Center" SortExpression="FgMPS">
								<Itemtemplate>
									<asp:Label Runat="server" Width="80" ID="FgMPS" TEXT='<%# DataBinder.Eval(Container.DataItem, "FgMPS") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:DropDownList ID="FgMPSEdit" CssClass="DropDownList" Width="100%" runat="server" SelectedValue='<%# DataBinder.Eval(Container.DataItem, "FgMPS") %>'>
									  <asp:ListItem>Y</asp:ListItem>
									  <asp:ListItem>N</asp:ListItem>                                                                            
								</asp:DropDownList>								    
								</EditItemTemplate>
								<FooterTemplate>
									<asp:DropDownList ID="FgMPSAdd" CssClass="DropDownList" Width="100%" runat="server">
									  <asp:ListItem Selected="True">Y</asp:ListItem>                                     
									  <asp:ListItem>N</asp:ListItem>                               
									</asp:DropDownList>								    
								</FooterTemplate>
<HeaderStyle Width="80px" />
                                <ItemStyle HorizontalAlign="Center" />
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
