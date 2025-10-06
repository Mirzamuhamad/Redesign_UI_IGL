<%@ Page Language="VB" AutoEventWireup="false" CodeFile="MsPPHRate.aspx.vb" Inherits="Master_MsPPHRate_MsPPHRate" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Rate PPH Gaji</title>
    <script src="../../Function/Function.JS" type="text/javascript"></script>
    <script src="../../Function/OpenDlg.JS" type="text/javascript"></script> 
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
 <link href="https://fonts.googleapis.com/css2?family=Muli:wght@300;400;600&display=swap" rel="stylesheet">
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div class="Content">
     <div class="H1">Rate PPH Gaji</div>
     <hr style="color:Blue" />
           <table>
        <tr>
            <td style="width:100px;text-align:right" >Quick Search :</td>
            <td><asp:TextBox runat="server" ID ="tbFilter" CssClass="TextBox" /> 
                <asp:DropDownList runat="server" ID="ddlField" CssClass="DropDownList" >
                  <asp:ListItem Selected="true" Text="Code" Value="Code"></asp:ListItem>
                  <asp:ListItem Text="Start Value" Value="StartValue"></asp:ListItem>        
                  <asp:ListItem Text="End Value" Value="EndValue"></asp:ListItem>        
                  <asp:ListItem Text="Rate NPWP" Value="RateNPWP"></asp:ListItem>        
                  <asp:ListItem Text="Rate Non NPWP" Value="RateNonNPWP"></asp:ListItem>        
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
                    <asp:ListItem Selected="true" Text="Code" Value="Code"></asp:ListItem>
                    <asp:ListItem Text="Start Value" Value="StartValue"></asp:ListItem>        
                  <asp:ListItem Text="End Value" Value="EndValue"></asp:ListItem>        
                  <asp:ListItem Text="Rate NPWP" Value="RateNPWP"></asp:ListItem>        
                  <asp:ListItem Text="Rate Non NPWP" Value="RateNonNPWP"></asp:ListItem>   
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
							<asp:TemplateField HeaderText="Code" HeaderStyle-Width="80" SortExpression="Code" ItemStyle-HorizontalAlign="Right" >
								<Itemtemplate>
									<asp:Label Runat="server" ID="Code" text='<%# DataBinder.Eval(Container.DataItem, "Code") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:Label Runat="server" ID="CodeEdit" Text='<%# DataBinder.Eval(Container.DataItem, "Code") %>'>
									</asp:Label>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="CodeAdd" CssClass="TextBox" MaxLength="4" Runat="Server" Width="100%"/>
									<cc1:TextBoxWatermarkExtender ID="CodeAdd_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="CodeAdd" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>
								</FooterTemplate>
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Start Value" HeaderStyle-Width="100" SortExpression="StartValue" ItemStyle-HorizontalAlign="Right" >
								<Itemtemplate>
									<asp:Label Runat="server" ID="StartValue" text='<%# DataBinder.Eval(Container.DataItem, "StartValue") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:TextBox Runat="server" CssClass="TextBox" ID="StartValueEdit" MaxLength="10" Width="100%" Text='<%# DataBinder.Eval(Container.DataItem, "StartValue") %>'>
									</asp:TextBox>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="StartValueAdd" CssClass="TextBox" Runat="Server" MaxLength="10" Width="100%"/>
									<cc1:TextBoxWatermarkExtender ID="StartValueAdd_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="StartValueAdd" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>
								</FooterTemplate>
							</asp:TemplateField>		
							
							<asp:TemplateField HeaderText="End Value" HeaderStyle-Width="100" SortExpression="EndValue" ItemStyle-HorizontalAlign="Right" >
								<Itemtemplate>
									<asp:Label Runat="server" ID="EndValue" text='<%# DataBinder.Eval(Container.DataItem, "EndValue") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:TextBox Runat="server" CssClass="TextBox" ID="EndValueEdit" MaxLength="10" Width="100%" Text='<%# DataBinder.Eval(Container.DataItem, "EndValue") %>'>
									</asp:TextBox>
									
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="EndValueAdd" CssClass="TextBox" Runat="Server" MaxLength="10" Width="100%"/>
									<cc1:TextBoxWatermarkExtender ID="EndValueAdd_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="EndValueAdd" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>
								</FooterTemplate>
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Rate NPWP (%)" HeaderStyle-Width="100" SortExpression="RateNPWP" ItemStyle-HorizontalAlign="Right" >
								<Itemtemplate>
									<asp:Label Runat="server" ID="RateNPWP" text='<%# DataBinder.Eval(Container.DataItem, "RateNPWP") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:TextBox Runat="server" CssClass="TextBox" ID="RateNPWPEdit" MaxLength="10" Width="100%" Text='<%# DataBinder.Eval(Container.DataItem, "RateNPWP") %>'>
									</asp:TextBox>
									
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="RateNPWPAdd" CssClass="TextBox" Runat="Server" MaxLength="10" Width="100%"/>
									<cc1:TextBoxWatermarkExtender ID="RateNPWPAdd_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="RateNPWPAdd" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>
								</FooterTemplate>
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Rate Non NPWP (%)" HeaderStyle-Width="120" SortExpression="RateNonNPWP" ItemStyle-HorizontalAlign="Right" >
								<Itemtemplate>
									<asp:Label Runat="server" ID="RateNonNPWP" text='<%# DataBinder.Eval(Container.DataItem, "RateNonNPWP") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:TextBox Runat="server" CssClass="TextBox" ID="RateNonNPWPEdit" MaxLength="10" Width="100%" Text='<%# DataBinder.Eval(Container.DataItem, "RateNonNPWP") %>'>
									</asp:TextBox>
									
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="RateNonNPWPAdd" CssClass="TextBox" Runat="Server" MaxLength="10" Width="100%"/>
									<cc1:TextBoxWatermarkExtender ID="RateNonNPWPAdd_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="RateNonNPWPAdd" 
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
        </asp:GridView>
        
     <asp:Label ID="lstatus" ForeColor="red" runat="server"></asp:Label>
    </div>
    </form>
</body>
</html>
