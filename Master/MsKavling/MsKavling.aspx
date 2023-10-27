<%@ Page Language="VB" AutoEventWireup="false" CodeFile="MsKavling.aspx.vb" Inherits="Master_MsKavling_MsKavling" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Kavling File</title>
    <script src="../../Function/OpenDlg.js" type="text/javascript"></script>
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div class="Content">
     <div class="H1">Kavling File</div>
     <hr style="color:Blue" />
      <table>
        <tr>
            <td style="width:100px;text-align:right" >Quick Search :</td>
            <td><asp:TextBox runat="server" ID ="tbFilter" CssClass="TextBox"/> 
                <asp:DropDownList runat="server" ID="ddlField" CssClass="DropDownList" >
                  <asp:ListItem Selected="true" Text="Code" Value="KavlingCode"></asp:ListItem>
                  <asp:ListItem Text="Name" Value="KavlingName"></asp:ListItem>        
                  <asp:ListItem Text="Block" Value="Block"></asp:ListItem>
                  <asp:ListItem Text="Area" Value="Area"></asp:ListItem>         
                  <asp:ListItem Text="Max Capacity" Value="MaxCap"></asp:ListItem>         
                  <asp:ListItem Text="Owner" Value="OwnerName"></asp:ListItem>         
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
                    <asp:ListItem Selected="true" Text="Code" Value="KavlingCode"></asp:ListItem>
                    <asp:ListItem Text="Description" Value="KavlingName"></asp:ListItem> 
                    <asp:ListItem Text="Block" Value="Block"></asp:ListItem>
                    <asp:ListItem Text="Area" Value="Area"></asp:ListItem> 
                    <asp:ListItem Text="Max Capacity" Value="MaxCap"></asp:ListItem>         
                  <asp:ListItem Text="Owner" Value="OwnerName"></asp:ListItem>                
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
							<asp:TemplateField HeaderText="Kavling Code" HeaderStyle-Width="100" SortExpression="KavlingCode">
								<Itemtemplate>
									<asp:Label Runat="server" ID="KavlingCode" text='<%# DataBinder.Eval(Container.DataItem, "KavlingCode") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:Label Runat="server" ID="KavlingCodeEdit" Text='<%# DataBinder.Eval(Container.DataItem, "KavlingCode") %>'>
									</asp:Label>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="KavlingCodeAdd" CssClass="TextBox" MaxLength="5" Runat="Server" Width="100%"/>
									<cc1:TextBoxWatermarkExtender ID="KavlingCodeAdd_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="KavlingCodeAdd" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>
								</FooterTemplate>

<HeaderStyle Width="100px"></HeaderStyle>
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Kavling Name" HeaderStyle-Width="320" SortExpression="KavlingName">
								<Itemtemplate>
									<asp:Label Runat="server" ID="KavlingName" text='<%# DataBinder.Eval(Container.DataItem, "KavlingName") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:TextBox Runat="server" CssClass="TextBox" ID="KavlingNameEdit" MaxLength="60" Width="100%" Text='<%# DataBinder.Eval(Container.DataItem, "KavlingName") %>' Enabled="True">
									</asp:TextBox>
									<cc1:TextBoxWatermarkExtender ID="KavlingNameEdit_WtExt" 
                                        runat="server" Enabled="False" TargetControlID="KavlingNameEdit" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="KavlingNameAdd" CssClass="TextBox" MaxLength="60" Runat="Server" Width="100%"/>
									<cc1:TextBoxWatermarkExtender ID="KavlingNameAdd_WtExt" 
                                        runat="server" Enabled="False" TargetControlID="KavlingNameAdd" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>
								</FooterTemplate>

<HeaderStyle Width="320px"></HeaderStyle>
							</asp:TemplateField>		
							
															
								<asp:TemplateField HeaderText="Block" HeaderStyle-Width="320" ItemStyle-HorizontalAlign="Center" SortExpression="Block">
								<Itemtemplate>
									<asp:Label Runat="server"  Width="80" ID="Block" TEXT='<%# DataBinder.Eval(Container.DataItem, "BlockName") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:DropDownList ID="BlockEdit" CssClass="DropDownList" Width="100%" runat="server"
									OnSelectedIndexChanged="ddlBlockEdit_SelectedIndexChanged" SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Block") %>'
									  DataSourceID="dsGetBlock" DataTextField="BlockName" 
                                        DataValueField="BlockCode">                                                                             
								</asp:DropDownList>								    
								</EditItemTemplate>
								<FooterTemplate>
									<asp:DropDownList ID="BlockAdd" CssClass="DropDownList" Width="100%" runat="server"
									OnSelectedIndexChanged="ddlBlockAdd_SelectedIndexChanged" 
									   DataSourceID="dsGetBlock" DataTextField="BlockName" 
                                        DataValueField="BlockCode">                               
									</asp:DropDownList>								    
								</FooterTemplate>
<HeaderStyle Width="320px" />
                                <ItemStyle HorizontalAlign="Center" />
							</asp:TemplateField>
							
							
							<asp:TemplateField HeaderText="Luas" HeaderStyle-Width="80" SortExpression="Area">
								<Itemtemplate>
									<asp:Label Runat="server" ID="Area" text='<%# DataBinder.Eval(Container.DataItem, "Area") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:TextBox Runat="server" CssClass="TextBox" ID="AreaEdit" MaxLength="20" Width="100%" Text='<%# DataBinder.Eval(Container.DataItem, "Area") %>' Enabled="True">
									</asp:TextBox>
									<cc1:TextBoxWatermarkExtender ID="AreaEdit_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="AreaEdit" 
                                        WatermarkText="0" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="AreaAdd" Text ="0" CssClass="TextBox" MaxLength="5" Runat="Server" Width="100%" Enabled="False" />
								</FooterTemplate>
<HeaderStyle Width="80"></HeaderStyle>
							</asp:TemplateField>

<asp:TemplateField HeaderText="Max Capacity" HeaderStyle-Width="80" SortExpression="MaxCap">
								<Itemtemplate>
									<asp:Label Runat="server" ID="MaxCap" text='<%# DataBinder.Eval(Container.DataItem, "MaxCap") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:TextBox Runat="server" CssClass="TextBox" ID="MaxCapEdit" MaxLength="20" Width="100%" Text='<%# DataBinder.Eval(Container.DataItem, "MaxCap") %>' Enabled="True">
									</asp:TextBox>
									<cc1:TextBoxWatermarkExtender ID="MaxCapEdit_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="MaxCapEdit" 
                                        WatermarkText="0" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="MaxCapAdd" Text ="0" CssClass="TextBox" MaxLength="5" Runat="Server" Width="100%" Enabled="true" />
								</FooterTemplate>
<HeaderStyle Width="80"></HeaderStyle>
							</asp:TemplateField>

							<asp:TemplateField HeaderText="Owner" HeaderStyle-Width="320" SortExpression="OwnerName">
								<Itemtemplate>
									<asp:Label Runat="server" ID="OwnerName" text='<%# DataBinder.Eval(Container.DataItem, "OwnerName") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:TextBox Runat="server" CssClass="TextBox" ID="OwnerNameEdit" MaxLength="60" Width="100%" Text='<%# DataBinder.Eval(Container.DataItem, "OwnerName") %>' Enabled="True">
									</asp:TextBox>
									<cc1:TextBoxWatermarkExtender ID="OwnerNameEdit_WtExt" 
                                        runat="server" Enabled="False" TargetControlID="OwnerNameEdit" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="OwnerNameAdd" CssClass="TextBox" MaxLength="60" Runat="Server" Width="100%"/>
									<cc1:TextBoxWatermarkExtender ID="OwnerNameAdd_WtExt" 
                                        runat="server" Enabled="False" TargetControlID="OwnerNameAdd" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>
								</FooterTemplate>

<HeaderStyle Width="320px"></HeaderStyle>
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
                       
     <asp:SqlDataSource ID="dsGetBlock" runat="server" SelectCommand="SELECT BlockCode, BlockName, Area FROM MsBlock" ></asp:SqlDataSource>
     <asp:Label ID="lstatus" ForeColor="red" runat="server"></asp:Label>        
    </div>
    </form>
</body>
</html>
