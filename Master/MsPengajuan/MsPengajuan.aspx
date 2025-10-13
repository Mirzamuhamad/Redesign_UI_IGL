<%@ Page Language="VB" AutoEventWireup="false" CodeFile="MsPengajuan.aspx.vb" Inherits="Master_Pengajuan" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Kegiatam File</title>
    <script src="../../Function/OpenDlg.js" type="text/javascript"></script>
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
 <link href="https://fonts.googleapis.com/css2?family=Muli:wght@300;400;600&display=swap" rel="stylesheet">
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div class="Content">
     <div class="H1">Tempat Pengajuan</div>
     <hr CssClass="Hr" />
      <table>
        <tr>
            <td style="width:100px;text-align:left" >Quick Search :</td>
            <td><asp:TextBox runat="server" ID ="tbFilter" CssClass="TextBox"/> 
                <asp:DropDownList runat="server" ID="ddlField" CssClass="DropDownList" >
                  <asp:ListItem Selected="true" Text="Code" Value="PlaceCode"></asp:ListItem>
                  <asp:ListItem Text="Name" Value="PlaceName"></asp:ListItem>                
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
                    <asp:ListItem Selected="true" Text="Code" Value="PlaceCode"></asp:ListItem>
                    <asp:ListItem Text="Description" Value="PlaceName"></asp:ListItem> 
                            
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
							<asp:TemplateField HeaderText="PlaceCode" HeaderStyle-Width="100" SortExpression="PlaceCode">
								<Itemtemplate>
									<asp:Label Runat="server" ID="PlaceCode" text='<%# DataBinder.Eval(Container.DataItem, "PlaceCode") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:Label Runat="server" ID="PlaceCode" text='<%# DataBinder.Eval(Container.DataItem, "PlaceCode") %>'>
									</asp:Label>
								</EditItemTemplate>
								<FooterTemplate >
									<asp:TextBox placeholder="can't blank" ID="PlaceCodeAdd" CssClass="TextBox" MaxLength="5" Runat="Server" Width="100%"/>
									
								</FooterTemplate>

<HeaderStyle Width="100px"></HeaderStyle>
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="PlaceName " HeaderStyle-Width="320" SortExpression="PlaceName">
								<Itemtemplate>
									<asp:Label Runat="server" ID="PlaceName" text='<%# DataBinder.Eval(Container.DataItem, "PlaceName") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:TextBox Runat="server" CssClass="TextBox" ID="PlaceNameEdit" MaxLength="40" Width="100%" Text='<%# DataBinder.Eval(Container.DataItem, "PlaceName") %>'>
									</asp:TextBox>
									<cc1:TextBoxWatermarkExtender ID="PlaceNameEdit_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="PlaceNameEdit" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>
								</EditItemTemplate>
								
								<FooterTemplate>
									<asp:TextBox placeholder="can't blank" ID="PlaceNameAdd" CssClass="TextBox" MaxLength="100" Runat="Server" Width="100%">
									</asp:TextBox>
									
								</FooterTemplate>


<HeaderStyle Width="320"></HeaderStyle>
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
                       

     <asp:Label ID="lstatus" ForeColor="red" runat="server"></asp:Label>        
    </div>
    </form>
</body>
</html>
