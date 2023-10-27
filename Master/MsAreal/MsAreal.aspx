<%@ Page Language="VB" AutoEventWireup="false" CodeFile="MsAreal.aspx.vb" Inherits="Master_MsAreal_MsAreal" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Areal File</title>
    <script src="../../Function/OpenDlg.js" type="text/javascript"></script>
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div class="Content">
     <div class="H1">Areal File</div>
     <hr style="color:Blue" />
      <table>
        <tr>
            <td style="width:100px;text-align:right" >Quick Search :</td>
            <td><asp:TextBox runat="server" ID ="tbFilter" CssClass="TextBox"/> 
                <asp:DropDownList runat="server" ID="ddlField" CssClass="DropDownList" >
                  <asp:ListItem Selected="true" Text="Code" Value="ArealCode"></asp:ListItem>
                  <asp:ListItem Text="Name" Value="ArealName"></asp:ListItem>        
                  <asp:ListItem Text="Division" Value="Division"></asp:ListItem>
                  <asp:ListItem Text="Area" Value="Area"></asp:ListItem>         
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
                    <asp:ListItem Selected="true" Text="Code" Value="ArealCode"></asp:ListItem>
                    <asp:ListItem Text="Description" Value="ArealName"></asp:ListItem> 
                    <asp:ListItem Text="Division" Value="Division"></asp:ListItem>
                    <asp:ListItem Text="Area" Value="Area"></asp:ListItem>        
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
							<asp:TemplateField HeaderText="Areal Code" HeaderStyle-Width="100" SortExpression="ArealCode">
								<Itemtemplate>
									<asp:Label Runat="server" ID="ArealCode" text='<%# DataBinder.Eval(Container.DataItem, "ArealCode") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:Label Runat="server" ID="ArealCodeEdit" Text='<%# DataBinder.Eval(Container.DataItem, "ArealCode") %>'>
									</asp:Label>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="ArealCodeAdd" CssClass="TextBox" MaxLength="5" Runat="Server" Width="100%"/>
									<cc1:TextBoxWatermarkExtender ID="ArealCodeAdd_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="ArealCodeAdd" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>
								</FooterTemplate>

<HeaderStyle Width="100px"></HeaderStyle>
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Areal Name" HeaderStyle-Width="320" SortExpression="ArealName">
								<Itemtemplate>
									<asp:Label Runat="server" ID="ArealName" text='<%# DataBinder.Eval(Container.DataItem, "ArealName") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:TextBox Runat="server" CssClass="TextBox" ID="ArealNameEdit" MaxLength="60" Width="100%" Text='<%# DataBinder.Eval(Container.DataItem, "ArealName") %>' Enabled="True">
									</asp:TextBox>
									<cc1:TextBoxWatermarkExtender ID="ArealNameEdit_WtExt" 
                                        runat="server" Enabled="False" TargetControlID="ArealNameEdit" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="ArealNameAdd" CssClass="TextBox" MaxLength="60" Runat="Server" Width="100%"/>
									<cc1:TextBoxWatermarkExtender ID="ArealNameAdd_WtExt" 
                                        runat="server" Enabled="False" TargetControlID="ArealNameAdd" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>
								</FooterTemplate>

<HeaderStyle Width="320px"></HeaderStyle>
							</asp:TemplateField>		
							
															
								<asp:TemplateField HeaderText="Division" HeaderStyle-Width="320" ItemStyle-HorizontalAlign="Center" SortExpression="Division">
								<Itemtemplate>
									<asp:Label Runat="server"  Width="80" ID="Division" TEXT='<%# DataBinder.Eval(Container.DataItem, "DivisionName") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:DropDownList ID="DivisionEdit" CssClass="DropDownList" Width="100%" runat="server"
									OnSelectedIndexChanged="ddlDivisionEdit_SelectedIndexChanged" SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Division") %>'
									  DataSourceID="dsGetSection" DataTextField="DivisionName" 
                                        DataValueField="DivisionCode">                                                                             
								</asp:DropDownList>								    
								</EditItemTemplate>
								<FooterTemplate>
									<asp:DropDownList ID="DivisionAdd" CssClass="DropDownList" Width="100%" runat="server"
									OnSelectedIndexChanged="ddlDivisionAdd_SelectedIndexChanged" 
									   DataSourceID="dsGetSection" DataTextField="DivisionName" 
                                        DataValueField="DivisionCode">                               
									</asp:DropDownList>								    
								</FooterTemplate>
<HeaderStyle Width="320px" />
                                <ItemStyle HorizontalAlign="Center" />
							</asp:TemplateField>
							
							
							<asp:TemplateField HeaderText="Area" HeaderStyle-Width="80" SortExpression="Area">
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
                       
     <asp:SqlDataSource ID="dsGetSection" runat="server" SelectCommand="SELECT DivisionCode, DivisionName, Area FROM MsDivision" ></asp:SqlDataSource>
     <asp:Label ID="lstatus" ForeColor="red" runat="server"></asp:Label>        
    </div>
    </form>
</body>
</html>
