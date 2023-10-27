<%@ Page Language="VB" AutoEventWireup="false" CodeFile="MsAccSubGroup.aspx.vb" Inherits="Execute_Master_MsAccSubGroup_MsAccSubGroup" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Account Sub Group File</title>
    <script src="../../Function/OpenDlg.JS" type="text/javascript"></script>         
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div class="Content">
     <div class="H1">Account Sub Group File</div>
     <hr style="color:Blue" />
      <table>
        <tr>
            <td style="width:100px;text-align:right" >Quick Search :</td>
            <td><asp:TextBox CssClass="TextBox" runat="server" ID ="tbFilter"/> 
                <asp:DropDownList runat="server" ID="ddlField" CssClass="DropDownList" >
                  <asp:ListItem Selected="true" Text="Account Sub Group Code" Value="AccSubGroupCode"></asp:ListItem>
                  <asp:ListItem Text="Account Sub Group Name" Value="AccSubGroupName"></asp:ListItem>        
                    <asp:ListItem Value="AccGroupName">Account Group</asp:ListItem>
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
            <td><asp:TextBox CssClass="TextBox" runat="server" ID ="tbfilter2"/> 
                  <asp:DropDownList runat="server" ID="ddlField2" CssClass="DropDownList" >
                    <asp:ListItem Selected="true" Text="Account Sub Group Code" Value="AccSubGroupCode"></asp:ListItem>
                  <asp:ListItem Text="Account Sub Group Name" Value="AccSubGroupName"></asp:ListItem>        
                    <asp:ListItem Value="AccGroupName">Account Group</asp:ListItem>
                  </asp:DropDownList>
                
            </td>
        </tr>
     </table>
     </asp:Panel>
      <br />
      <asp:GridView id="DataGrid" runat="server" 
            ShowFooter="True" AllowSorting="True" 
            AutoGenerateColumns="False" AllowPaging="True" CssClass="Grid">
						<HeaderStyle CssClass="GridHeader" Wrap = "false"> </HeaderStyle>
						<RowStyle CssClass="GridItem" Wrap="false"/>
						<AlternatingRowStyle CssClass="GridAltItem"/>
						<FooterStyle CssClass="GridFooter" />
						<PagerStyle CssClass="GridPager"  />
				      <Columns>
							<asp:TemplateField HeaderText="Account Sub Group Code" HeaderStyle-Width="110" SortExpression="AccSubGroupCode">
								<Itemtemplate>
									<asp:Label Runat="server" ID="AccSubGroupCode" text='<%# DataBinder.Eval(Container.DataItem, "AccSubGroupCode") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:Label Runat="server" ID="AccSubGroupCodeEdit" Text='<%# DataBinder.Eval(Container.DataItem, "AccSubGroupCode") %>'>
									</asp:Label>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="AccSubGroupCodeAdd" placeholder="can't blank" MaxLength="6" CssClass="TextBox" Runat="Server" Width="100%"/>
									<%--<cc1:TextBoxWatermarkExtender ID="AccSubGroupCodeAdd_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="AccSubGroupCodeAdd" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender> --%>
								</FooterTemplate>
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Account Sub Group Name" HeaderStyle-Width="275" SortExpression="AccSubGroupName">
								<Itemtemplate>
									<asp:Label Runat="server" ID="AccSubGroupName" text='<%# DataBinder.Eval(Container.DataItem, "AccSubGroupName") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:TextBox Runat="server" CssClass="TextBox" ID="AccSubGroupNameEdit" MaxLength="60" Width="100%" Text='<%# DataBinder.Eval(Container.DataItem, "AccSubGroupName") %>'>
									</asp:TextBox>
									<cc1:TextBoxWatermarkExtender ID="AccSubGroupNameEdit_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="AccSubGroupNameEdit" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="AccSubGroupNameAdd" placeholder="can't blank" CssClass="TextBox" MaxLength="60" Runat="Server" Width="90%"/>
									<%--<cc1:TextBoxWatermarkExtender ID="AccSubGroupNameAdd_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="AccSubGroupNameAdd" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender> --%>
								</FooterTemplate>
							</asp:TemplateField>		
							
							<asp:TemplateField HeaderText="Account Group" HeaderStyle-Width="210" SortExpression="AccGroupName">
								<Itemtemplate>
									<asp:Label Runat="server" ID="AccGroup" text='<%# DataBinder.Eval(Container.DataItem, "AccGroupName") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:DropDownList Runat="server" ID="AccSubGroupEdit" Width="100%" CssClass="DropDownList" 
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "AccGroup") %>' 
                                        DataSourceID="dsAccGroup" DataTextField="AccGroupName" 
                                        DataValueField="AccGroupCode">
									</asp:DropDownList>								    
								</EditItemTemplate>
								<FooterTemplate>
									<asp:DropDownList ID="AccGroupAdd" Runat="Server" Width="100%" CssClass="DropDownList"									    
                                        DataSourceID="dsAccGroup" DataTextField="AccGroupName" 
                                        DataValueField="AccGroupCode">
									</asp:DropDownList>
								</FooterTemplate>
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Action" headerstyle-width="126" >
								<ItemTemplate>
								    <asp:Button style = "height: 20px;" ID="btnEdit" runat="server" class="bitbtndt btnedit" Text="Edit" CommandName="Edit"/>
									<asp:Button style = "height: 20px;"  ID="btndelete" runat="server" class="bitbtndt btndelete" Text="Delete" OnClientClick="return confirm('Sure to delete this data?');" CommandName="Delete"/>									
								</ItemTemplate>
								<EditItemTemplate>
									<asp:Button style = "height: 20px;" ID="btnUpdate" runat="server" class="bitbtndt btnsave" Text="Save" CommandName="Update"/>									
                                    <asp:Button style = "height: 20px;" ID="btnCancel" runat="server" class="bitbtndt btncancel" Text="Cancel" CommandName="Cancel"/>									
								</EditItemTemplate>								
								<FooterTemplate>
									<asp:Button class="bitbtndt btnadd" runat="server" ID="btnAdd" Text="Add" CommandName="Insert"/>									
								</FooterTemplate>

<HeaderStyle Width="126px"></HeaderStyle>
							</asp:TemplateField>
							
    					</Columns>
        </asp:GridView>
        <asp:SqlDataSource ID="dsAccGroup" runat="server"                                                                                 
           SelectCommand="SELECT AccGroupCode, AccGroupName FROM MsAccGroup">                                        
        </asp:SqlDataSource>        
     <asp:Label ID="lstatus" ForeColor="red" runat="server"></asp:Label>        
    </div>
    </form>
</body>
</html>
