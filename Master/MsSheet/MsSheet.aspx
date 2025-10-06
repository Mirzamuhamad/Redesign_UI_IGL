<%@ Page Language="VB" AutoEventWireup="false" CodeFile="MsSheet.aspx.vb" Inherits="Execute_Master_MsSheet_MsSheet" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Sheet</title>
    <script src="../../Function/OpenDlg.js" type="text/javascript"></script>
    <script src="../../Function/Function.JS" type="text/javascript"></script>
    
    <script type="text/javascript">    
    function openprintdlg() {
            var wOpens;
            wOpens = window.open("../../Rpt/PrintForm.Aspx", "List", "scrollbars=yes,resizable=yes,width=500,height=400");
            wOpens.moveTo(0, 0);
            wOpens.resizeTo(screen.width, screen.height);
     )      
    </script>
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
 <link href="https://fonts.googleapis.com/css2?family=Muli:wght@300;400;600&display=swap" rel="stylesheet">
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div class="Content">
     <div class="H1">Sheet</div>
     <hr style="color:Blue" />
      <table>
        <tr>
            <td style="width:100px;text-align:right" >Quick Search :</td>
            <td><asp:TextBox runat="server" ID ="tbFilter" CssClass="TextBox"/> 
                <asp:DropDownList runat="server" ID="ddlField" CssClass="DropDownList" >
                    <asp:ListItem Selected="true" Text="Sheet Code" Value="Sheet"></asp:ListItem>
                    <asp:ListItem Text="Sheet Name" Value="SheetName"></asp:ListItem>        
                    <asp:ListItem Value="StartSheet">Start Sheet</asp:ListItem>
                    <asp:ListItem Value="EndSheet">End Sheet</asp:ListItem>
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
                    <asp:ListItem Selected="true" Text="Sheet Code" Value="Sheet"></asp:ListItem>
                    <asp:ListItem Text="Sheet Name" Value="SheetName"></asp:ListItem>        
                    <asp:ListItem Value="StartSheet">Start Sheet</asp:ListItem>
                    <asp:ListItem Value="EndSheet">End Sheet</asp:ListItem>
                  </asp:DropDownList>
            </td>
        </tr>
     </table>
     </asp:Panel>
      <br />
      <asp:GridView id="DataGrid" runat="server" ShowFooter="True" AllowSorting="True" 
            AutoGenerateColumns="False" AllowPaging="True" CssClass="Grid">
						<HeaderStyle CssClass="GridHeader"></HeaderStyle>
						<RowStyle CssClass="GridItem" Wrap="false"/>
						<AlternatingRowStyle CssClass="GridAltItem"/>
						<FooterStyle CssClass="GridFooter" />
						<PagerStyle CssClass="GridPager" />
				      <Columns>
							<asp:TemplateField HeaderText="Sheet Code" SortExpression="Sheet">
								<Itemtemplate>
									<asp:Label Runat="server" ID="Sheet" text='<%# DataBinder.Eval(Container.DataItem, "Sheet") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:Label Runat="server" ID="SheetEditTemp" Text='<%# DataBinder.Eval(Container.DataItem, "Sheet") %>' Visible="false">
									</asp:Label>
									<asp:TextBox Runat="server" ID="SheetEdit" CssClass="TextBox" MaxLength="5" Width="100%" Text='<%# DataBinder.Eval(Container.DataItem, "Sheet") %>'>
									</asp:TextBox>
									<cc1:TextBoxWatermarkExtender ID="SheetEdit_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="SheetEdit" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="SheetAdd" CssClass="TextBox" MaxLength="5" Runat="Server" Width="100%"/>
									<cc1:TextBoxWatermarkExtender ID="SheetAdd_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="SheetAdd" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>
								</FooterTemplate>
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Sheet Name" HeaderStyle-Width="350" SortExpression="SheetName">
								<Itemtemplate>
									<asp:Label Runat="server" ID="SheetName" text='<%# DataBinder.Eval(Container.DataItem, "SheetName") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:TextBox ID="SheetNameEdit" Runat="server" CssClass="TextBox" MaxLength="60" Width="100%" Text='<%# DataBinder.Eval(Container.DataItem, "SheetName") %>'>
									</asp:TextBox>
									<cc1:TextBoxWatermarkExtender ID="SheetNameEdit_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="SheetNameEdit" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="SheetNameAdd" CssClass="TextBox" MaxLength="60" Runat="Server" Width="100%"/>
									<cc1:TextBoxWatermarkExtender ID="SheetNameAdd_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="SheetNameAdd" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>
								</FooterTemplate>
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Start Sheet" HeaderStyle-Width="100" SortExpression="StartSheet">
								<Itemtemplate>
									<asp:Label Runat="server" ID="StartSheet" text='<%# DataBinder.Eval(Container.DataItem, "StartSheet") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:TextBox ID="StartSheetEdit" Runat="server" CssClass="TextBox" MaxLength="4" Width="100%" Text='<%# DataBinder.Eval(Container.DataItem, "StartSheet") %>'>
									</asp:TextBox>
									<cc1:TextBoxWatermarkExtender ID="StartSheetEdit_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="StartSheetEdit" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="StartSheetAdd" CssClass="TextBox" MaxLength="4" Runat="Server" Width="100%"/>
									<cc1:TextBoxWatermarkExtender ID="StartSheetAdd_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="StartSheetAdd" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>
								</FooterTemplate>
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="End Sheet" HeaderStyle-Width="100" SortExpression="EndSheet">
								<Itemtemplate>
									<asp:Label Runat="server" ID="EndSheet" text='<%# DataBinder.Eval(Container.DataItem, "EndSheet") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:TextBox ID="EndSheetEdit" Runat="server" CssClass="TextBox" MaxLength="4" Width="100%" Text='<%# DataBinder.Eval(Container.DataItem, "EndSheet") %>'>
									</asp:TextBox>
									<cc1:TextBoxWatermarkExtender ID="EndSheetEdit_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="EndSheetEdit" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="EndSheetAdd" CssClass="TextBox" MaxLength="4" Runat="Server" Width="100%"/>
									<cc1:TextBoxWatermarkExtender ID="EndSheetAdd_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="EndSheetAdd" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>
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
							</asp:TemplateField>
    					</Columns>
        </asp:gridView>       
     <asp:Label ID="lstatus" ForeColor="red" runat="server"></asp:Label>        
    </div>
    
    <asp:SqlDataSource ID="dsUnit" runat="server" SelectCommand="EXEC S_GetUnit">
        </asp:SqlDataSource>
    </form>
</body>
</html>
