<%@ Page Language="VB" AutoEventWireup="false" CodeFile="MsBobotPA.aspx.vb" Inherits="Master_MsBobotPA_MsBobotPA" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitle File</title>
    <script src="../../Function/Function.JS" type="text/javascript"></script>
    <script src="../../Function/OpenDlg.js" type="text/javascript"></script>
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div class="Content">
     <div class="H1">Bobot PA File</div>
     <hr style="color:Blue" />
      <table>
        <tr>
            <td style="width:100px;text-align:right" >Quick Search :</td>
            <td><asp:TextBox runat="server" ID ="tbFilter" CssClass="TextBox"/> 
                <asp:DropDownList runat="server" ID="ddlField" CssClass="DropDownList" >
                  <asp:ListItem Selected="true" Text="Competence Type" Value="CompetenceType"></asp:ListItem>
                  <asp:ListItem Text="KPI Quality" Value="Bobot1"></asp:ListItem>        
                  <asp:ListItem Text="Quality Of General Competence" Value="Bobot2"></asp:ListItem>        
                  <asp:ListItem Text="Quality Of Functional Competencies" Value="Bobot3"></asp:ListItem>        
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
                    <asp:ListItem Selected="true" Text="Competence Type" Value="CompetenceType"></asp:ListItem>
                  <asp:ListItem Text="KPI Quality" Value="Bobot1"></asp:ListItem>        
                  <asp:ListItem Text="Quality Of General Competence" Value="Bobot2"></asp:ListItem>        
                  <asp:ListItem Text="Quality Of Functional Competemcies" Value="Bobot3"></asp:ListItem>        
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
				            <asp:TemplateField HeaderText="Competence Type" HeaderStyle-Width="100" SortExpression="CompetenceType">
								<Itemtemplate>
									<asp:Label Runat="server" ID="CompetenceType" text='<%# DataBinder.Eval(Container.DataItem, "CompetenceType") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:Label Runat="server" ID="CompetenceTypeEdit" Text='<%# DataBinder.Eval(Container.DataItem, "CompetenceType") %>'>
									</asp:Label>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="CompetenceTypeAdd" CssClass="TextBox" MaxLength="30" Runat="Server" Width="100%"/>
									<cc1:TextBoxWatermarkExtender ID="CompetenceTypeAdd_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="CompetenceTypeAdd" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>
								</FooterTemplate>
                                <HeaderStyle Width="100px"></HeaderStyle>
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="KPI Quality(%)" HeaderStyle-Width="70px" SortExpression="Bobot1">
								<Itemtemplate>
									<asp:Label Runat="server" ID="Bobot1" text='<%# DataBinder.Eval(Container.DataItem, "Bobot1") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:TextBox Runat="server" CssClass="TextBox" ID="Bobot1Edit" MaxLength="10" Width="100%" Text='<%# DataBinder.Eval(Container.DataItem, "Bobot1") %>'>
									</asp:TextBox>
									<cc1:TextBoxWatermarkExtender ID="Bobot1Edit_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="Bobot1Edit" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="Bobot1Add" CssClass="TextBox" MaxLength="10" Runat="Server" Width="100%"/>
									<cc1:TextBoxWatermarkExtender ID="Bobot1Add_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="Bobot1Add" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>
								</FooterTemplate>
                            <HeaderStyle Width="70px"></HeaderStyle>
							</asp:TemplateField>	
							
							<asp:TemplateField HeaderText="Quality Of General Competence(%)" HeaderStyle-Width="100px" SortExpression="Bobot2">
								<Itemtemplate>
									<asp:Label Runat="server" ID="Bobot2" text='<%# DataBinder.Eval(Container.DataItem, "Bobot2") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:TextBox Runat="server" CssClass="TextBox" ID="Bobot2Edit" MaxLength="10" Width="100%" Text='<%# DataBinder.Eval(Container.DataItem, "Bobot2") %>'>
									</asp:TextBox>
									<cc1:TextBoxWatermarkExtender ID="Bobot2Edit_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="Bobot2Edit" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="Bobot2Add" CssClass="TextBox" MaxLength="10" Runat="Server" Width="100%"/>
									<cc1:TextBoxWatermarkExtender ID="Bobot2Add_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="Bobot2Add" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>
								</FooterTemplate>
                            <HeaderStyle Width="100px"></HeaderStyle>
							</asp:TemplateField>		
														
							<asp:TemplateField HeaderText="Quality Of Functional Competencies(%)" HeaderStyle-Width="100px" SortExpression="Bobot3">
								<Itemtemplate>
									<asp:Label Runat="server" ID="Bobot3" text='<%# DataBinder.Eval(Container.DataItem, "Bobot3") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:TextBox Runat="server" CssClass="TextBox" ID="Bobot3Edit" MaxLength="10" Width="100%" Text='<%# DataBinder.Eval(Container.DataItem, "Bobot3") %>'>
									</asp:TextBox>
									<cc1:TextBoxWatermarkExtender ID="Bobot3Edit_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="Bobot3Edit" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="Bobot3Add" CssClass="TextBox" MaxLength="10" Runat="Server" Width="100%"/>
									<cc1:TextBoxWatermarkExtender ID="Bobot3Add_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="Bobot3Add" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>
								</FooterTemplate>
                            <HeaderStyle Width="100px"></HeaderStyle>
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
