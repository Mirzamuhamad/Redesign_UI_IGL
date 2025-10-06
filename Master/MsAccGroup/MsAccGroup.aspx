<%@ Page Language="VB" AutoEventWireup="false" CodeFile="MsAccGroup.aspx.vb" Inherits="Execute_Master_MsAccGroup_MsAccGroup" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Account Group File</title>
    <script src="../../Function/OpenDlg.JS" type="text/javascript"></script>         
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
 <link href="https://fonts.googleapis.com/css2?family=Muli:wght@300;400;600&display=swap" rel="stylesheet">
</head>
<body>
    <form id="form1" runat="server">    
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div class="Content">
     <div class="H1">Account Group File</div>
     <hr style="color:Blue" />
      <table>
        <tr>
            <td style="width:100px;text-align:right" >Quick Search :</td>
            <td><asp:TextBox runat="server" CssClass="TextBox" ID ="tbFilter"/> 
                <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlField" >
                  <asp:ListItem Selected="true" Text="Account Group Code" Value="AccGroupCode"></asp:ListItem>
                  <asp:ListItem Text="Account Group Name" Value="AccGroupName"></asp:ListItem>        
                    <asp:ListItem Value="AccTypeName">Account Type</asp:ListItem>
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
                  <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlField2" >
                    <asp:ListItem Selected="true" Text="Account Group Code" Value="AccGroupCode"></asp:ListItem>
                    <asp:ListItem Text="Account Group Name" Value="AccGroupName"></asp:ListItem> 
                      <asp:ListItem Value="AccTypeName">Account Type</asp:ListItem>
                  </asp:DropDownList>
                
            </td>
        </tr>
     </table>
     </asp:Panel>
      <br />
      <asp:GridView id="DataGrid" runat="server" 
            ShowFooter="True" AllowSorting="True" 
            AutoGenerateColumns="False" AllowPaging="True"  CssClass="Grid">
						<HeaderStyle CssClass="GridHeader"></HeaderStyle>
						<RowStyle CssClass="GridItem" Wrap="false"/>
						<AlternatingRowStyle CssClass="GridAltItem"/>
						<FooterStyle CssClass="GridFooter" />
						<PagerStyle CssClass="GridPager" Wrap="false" HorizontalAlign="Center"/>
				      <Columns>
							<asp:TemplateField HeaderText="Account Group Code" HeaderStyle-Width="110" SortExpression="AccGroupCode">
								<Itemtemplate>
									<asp:Label Runat="server" ID="AccGroupCode" text='<%# DataBinder.Eval(Container.DataItem, "AccGroupCode") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:Label Runat="server" ID="AccGroupCodeEdit" Text='<%# DataBinder.Eval(Container.DataItem, "AccGroupCode") %>'>
									</asp:Label>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="AccGroupCodeAdd" placeholder="can't blank" CssClass="TextBox" MaxLength="4" Runat="Server" Width="100%"/>
									<%--<cc1:TextBoxWatermarkExtender ID="AccGroupCodeAdd_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="AccGroupCodeAdd" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender> --%>
								</FooterTemplate>
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Account Group Name" HeaderStyle-Width="330" SortExpression="AccGroupName">
								<Itemtemplate>
									<asp:Label Runat="server" ID="AccGroupName" text='<%# DataBinder.Eval(Container.DataItem, "AccGroupName") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:TextBox Runat="server" CssClass="TextBox" MaxLength="60" ID="AccGroupNameEdit" Width="100%" Text='<%# DataBinder.Eval(Container.DataItem, "AccGroupName") %>'>
									</asp:TextBox>
									<cc1:TextBoxWatermarkExtender ID="AccGroupNameEdit_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="AccGroupNameEdit" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>									
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="AccGroupNameAdd" placeholder="can't blank" CssClass="TextBox" MaxLength="60" Runat="Server" Width="100%"/>
									<%--<cc1:TextBoxWatermarkExtender ID="AccGroupNameAdd_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="AccGroupNameAdd" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>--%>
								</FooterTemplate>
							</asp:TemplateField>		
							
							<asp:TemplateField HeaderText="Account Type" HeaderStyle-Width="160" SortExpression="AccTypeName">
								<Itemtemplate>
									<asp:Label Runat="server" ID="AccType" text='<%# DataBinder.Eval(Container.DataItem, "AccTypeName") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:DropDownList Runat="server" ID="AccTypeEdit" Width="100%" CssClass="DropDownList"
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "AccType") %>'
                                        DataSourceID="dsAccType" DataTextField="AccTypeName" 
                                        DataValueField="AccTypeCode">
									</asp:DropDownList>													
								</EditItemTemplate>
								<FooterTemplate>
									<asp:DropDownList ID="AccTypeAdd" Runat="Server" Width="100%" CssClass="DropDownList"
                                        DataSourceID="dsAccType" DataTextField="AccTypeName" 
                                        DataValueField="AccTypeCode">
									</asp:DropDownList>									
								</FooterTemplate>
							</asp:TemplateField>
							
							<%--<asp:TemplateField HeaderText="Cost Center" HeaderStyle-Width="30" SortExpression="FgCostCtr">
								<Itemtemplate>
									<asp:Label Runat="server" ID="FgCostCtr" text='<%# DataBinder.Eval(Container.DataItem, "FgCostCtr") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:DropDownList Runat="server" ID="FgCostCtrEdit" Width="100%" CssClass="DropDownList"
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "FgCostCtr") %>' >
									    <asp:ListItem>N</asp:ListItem>
									    <asp:ListItem>Y</asp:ListItem>
									</asp:DropDownList>													
								</EditItemTemplate>
								<FooterTemplate>
									<asp:DropDownList ID="FgCostCtrAdd" Runat="Server" Width="100%" CssClass="DropDownList" >
									    <asp:ListItem Selected="True">N</asp:ListItem>
									    <asp:ListItem >Y</asp:ListItem> 
									</asp:DropDownList>									
								</FooterTemplate>
							</asp:TemplateField>--%>

							
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
        
        <asp:SqlDataSource ID="dsAccType" runat="server"                                                                                 
                                        SelectCommand="EXEC S_GetAccType">                                        
        </asp:SqlDataSource>
     <asp:Label ID="lstatus" ForeColor="red" runat="server"></asp:Label>        
    </div>
    </form>
</body>
</html>
