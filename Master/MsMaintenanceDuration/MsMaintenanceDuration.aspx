<%@ Page Language="VB" AutoEventWireup="false" CodeFile="MsMaintenanceDuration.aspx.vb" Inherits="Master_MsMaintenanceDuration_MsMaintenanceDuration" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitle</title>
    <script src="../../Function/Function.JS" type="text/javascript"></script>
    <script src="../../Function/OpenDlg.js" type="text/javascript"></script>
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
 <link href="https://fonts.googleapis.com/css2?family=Muli:wght@300;400;600&display=swap" rel="stylesheet">
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div class="Content">
     <div class="H1">Maintenance Interval File</div>
     <hr style="color:Blue" />
      <table>
        <tr>
            <td style="width:100px;text-align:right" >Quick Search :</td>
            <td><asp:TextBox runat="server" ID ="tbFilter" CssClass="TextBox"/> 
                <asp:DropDownList runat="server" ID="ddlField" CssClass="DropDownList" >
                  <asp:ListItem Selected="true" Text="Period Code" Value="DurationCode"></asp:ListItem>
                  <asp:ListItem Text="Period Name" Value="DurationName"></asp:ListItem>  
                  <asp:ListItem Text="Days" Value="Days"></asp:ListItem>        
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
                    <asp:ListItem Selected="true" Text="Period Code" Value="DurationCode"></asp:ListItem>
                    <asp:ListItem Text="Period Name" Value="DurationName"></asp:ListItem>
                    <asp:ListItem Text="Days" Value="Days"></asp:ListItem>         
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
							<asp:TemplateField HeaderText="Period Code" HeaderStyle-Width="100" SortExpression="DurationCode">
								<Itemtemplate>
									<asp:Label Runat="server" ID="DurationCode" text='<%# DataBinder.Eval(Container.DataItem, "DurationCode") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:Label Runat="server" ID="DurationCodeEdit" Text='<%# DataBinder.Eval(Container.DataItem, "DurationCode") %>'>
									</asp:Label>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="DurationCodeAdd" CssClass="TextBox" MaxLength="3" Runat="Server" Width="100%"/>
									<cc1:TextBoxWatermarkExtender ID="DurationCodeAdd_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="DurationCodeAdd" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>
								</FooterTemplate>

<HeaderStyle Width="100px"></HeaderStyle>
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Period Name" HeaderStyle-Width="320" SortExpression="DurationName">
								<Itemtemplate>
									<asp:Label Runat="server" ID="DurationName" text='<%# DataBinder.Eval(Container.DataItem, "DurationName") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:TextBox Runat="server" CssClass="TextBox" ID="DurationNameEdit" MaxLength="60" Width="100%" Text='<%# DataBinder.Eval(Container.DataItem, "DurationName") %>'>
									</asp:TextBox>
									<cc1:TextBoxWatermarkExtender ID="DurationNameEdit_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="DurationNameEdit" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="DurationNameAdd" CssClass="TextBox" MaxLength="60" Runat="Server" Width="100%"/>
									<cc1:TextBoxWatermarkExtender ID="DurationNameAdd_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="DurationNameAdd" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>
								</FooterTemplate>

<HeaderStyle Width="320px"></HeaderStyle>
							</asp:TemplateField>		
							
							<asp:TemplateField HeaderText="Days" HeaderStyle-Width="50" SortExpression="Days" ItemStyle-HorizontalAlign = "Right" HeaderStyle-HorizontalAlign = "Right">
								<Itemtemplate>
									<asp:Label Runat="server" ID="Days" text='<%# DataBinder.Eval(Container.DataItem, "Days") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:TextBox Runat="server" CssClass="TextBox" ID="DaysEdit" MaxLength="12" Width="100%" Text='<%# DataBinder.Eval(Container.DataItem, "Days") %>'>
									</asp:TextBox>
									<cc1:TextBoxWatermarkExtender ID="DaysEdit_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="DaysEdit" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="DaysAdd" CssClass="TextBox" MaxLength="12" Runat="Server" Width="100%"/>
									<cc1:TextBoxWatermarkExtender ID="DaysAdd_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="DaysAdd" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>
								</FooterTemplate>

<HeaderStyle Width="50px"></HeaderStyle>
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
        <asp:SqlDataSource ID="dsProductType" runat="server" SelectCommand="EXEC S_GetProductType">
        </asp:SqlDataSource>

     <asp:Label ID="lstatus" ForeColor="red" runat="server"></asp:Label>        
    </div>
    </form>
</body>
</html>
