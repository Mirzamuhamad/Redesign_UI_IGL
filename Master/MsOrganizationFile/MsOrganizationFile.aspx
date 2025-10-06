<%@ Page Language="VB" AutoEventWireup="false" CodeFile="MsOrganizationFile.aspx.vb" Inherits="MsOrganizationFile_MsOrganizationFile" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Organization File</title>
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
     <div class="H1">Organization File</div>
     <hr style="color:Blue" />
      <table>
        <tr>
            <td style="width:100px;text-align:right" >Quick Search :</td>
            <td><asp:TextBox runat="server" ID ="tbFilter" CssClass="TextBox"/> 
                <asp:DropDownList runat="server" ID="ddlField" CssClass="DropDownList" >
                  <asp:ListItem Selected="true" Text="Department Code" Value="DeptCode"></asp:ListItem>
                  <asp:ListItem Text="Department Name" Value="DeptName"></asp:ListItem>
                  <asp:ListItem Text="Department Level" Value="DeptLevel"></asp:ListItem>
                  <asp:ListItem Text="Department Group" Value="DeptGroup"></asp:ListItem>       
                </asp:DropDownList>     
                <asp:Button class="bitbtn btnsearch" runat="server" ID="btnSearch" Text="Search" />                
                <asp:Button class="btngo" runat="server" ID="btnExpand" Text="..."/>                
                <asp:Button class="bitbtn btnprint" runat="server" ID="btnPrint" Text="Print"/>               
                <asp:Button ID="btnShowPopup" runat="server" Style="display: none" />
                <cc1:ModalPopupExtender ID="ModalPopupExtender1" runat="server" 
                    PopupControlID="pnlFind" DropShadow="True" TargetControlID="btnShowPopup"                     
                    BackgroundCssClass="BackgroundStyle" />                           
               
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
                      <asp:ListItem Selected="true" Text="Department Code" Value="DeptCode"></asp:ListItem>
                      <asp:ListItem Text="Department Name" Value="DeptName"></asp:ListItem>
                      <asp:ListItem Text="Department Level" Value="DeptLevel"></asp:ListItem>
                      <asp:ListItem Text="Department Group" Value="DeptGroup"></asp:ListItem>           
                  </asp:DropDownList>
                
            </td>
        </tr>
     </table>
     </asp:Panel>
      <br />
        
     <asp:Label ID="lstatus" ForeColor="red" runat="server"></asp:Label>    
    
      
      <asp:GridView id="DataGrid" runat="server" 
            ShowFooter="True" AllowSorting="True" 
            AutoGenerateColumns="False" AllowPaging="True" CssClass="Grid">
						<HeaderStyle CssClass="GridHeader"></HeaderStyle>
						<RowStyle CssClass="GridItem" Wrap="false"/>
						<AlternatingRowStyle CssClass="GridAltItem"/>
						<FooterStyle CssClass="GridFooter" />
						<PagerStyle CssClass="GridPager" HorizontalAlign="Center" />					  
				<EmptyDataTemplate>
				    
				</EmptyDataTemplate>	  
				      <Columns>				      
							<asp:TemplateField HeaderText="Department Code" HeaderStyle-Width="100" SortExpression="DeptCode">
								<Itemtemplate>
									<asp:Label Runat="server" ID="DeptCode" text='<%# DataBinder.Eval(Container.DataItem, "DeptCode") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:TextBox Runat="server" ID="DeptCodeEdit" MaxLength="10" Width="100%" CssClass="TextBox" Text='<%# DataBinder.Eval(Container.DataItem, "DeptCode") %>'>
									</asp:TextBox>
									<cc1:TextBoxWatermarkExtender ID="DeptCodeEdit_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="DeptCodeEdit" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                    </cc1:TextBoxWatermarkExtender>	
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="DeptCodeAdd" CssClass="TextBox" MaxLength="50" Runat="Server" Width="100%"/>
									<cc1:TextBoxWatermarkExtender ID="DeptCodeAdd_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="DeptCodeAdd" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>
								</FooterTemplate>								
							</asp:TemplateField>							
							
							<asp:TemplateField HeaderText="Department Name" HeaderStyle-Width="300" SortExpression="DeptName">
								<Itemtemplate>
									<asp:Label Runat="server" ID="DeptName" text='<%# DataBinder.Eval(Container.DataItem, "DeptName") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:TextBox Runat="server" ID="DeptNameEdit" MaxLength="60" Width="100%" CssClass="TextBox" Text='<%# DataBinder.Eval(Container.DataItem, "DeptName") %>'>
									</asp:TextBox>
									<cc1:TextBoxWatermarkExtender ID="DeptNameEdit_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="DeptNameEdit" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                    </cc1:TextBoxWatermarkExtender>									
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="DeptNameAdd" CssClass="TextBox" MaxLength="255" Runat="Server" Width="100%"/>
									<cc1:TextBoxWatermarkExtender ID="DeptNameAdd_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="DeptNameAdd" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>
								</FooterTemplate>
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Level" HeaderStyle-Width="50" SortExpression="DeptLevel">
								<Itemtemplate>
									<asp:Label Runat="server" ID="DeptLevel" text='<%# DataBinder.Eval(Container.DataItem, "DeptLevel") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:Label Runat="server" ID="DeptLevelEdit" MaxLength="4" Width="100%" CssClass="TextBox" Text='<%# DataBinder.Eval(Container.DataItem, "DeptLevel") %>'>
									</asp:Label>
									<%--<cc1:TextBoxWatermarkExtender ID="DeptLevelEdit_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="DeptLevelEdit" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                    </cc1:TextBoxWatermarkExtender>--%>									
								</EditItemTemplate>
								<FooterTemplate>
									<%--<asp:TextBox ID="LevelAdd" CssClass="TextBox" MaxLength="10" Runat="Server" Width="100%"/>
									<cc1:TextBoxWatermarkExtender ID="LevelAdd_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="LevelAdd" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>--%>
								</FooterTemplate>
							</asp:TemplateField>	
							
							<asp:TemplateField HeaderText="Department Group" HeaderStyle-Width="300" SortExpression="DeptGroup">
								<Itemtemplate>
									<asp:Label Runat="server" ID="DeptGroup" text='<%# DataBinder.Eval(Container.DataItem, "DeptGroup") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:TextBox Runat="server" ID="DeptGroupEdit" MaxLength="10" Width="30%" CssClass="TextBox" Text='<%# DataBinder.Eval(Container.DataItem, "DeptGroup") %>'
                                                 AutoPostBack="true" OnTextChanged="tbAccount_TextChanged"/>
									<asp:TextBox Runat="server" ID="DeptGroupEditDesc" MaxLength="255" Width="50%" CssClass="TextBox" Enabled="false"/>
									<asp:Button class="btngo" runat="server" ID="btnDeptGroupEdit" Text="..." CommandName="SearchParentEdit"/>
									<cc1:TextBoxWatermarkExtender ID="DeptGroupEdit_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="DeptGroupEdit" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                    </cc1:TextBoxWatermarkExtender>									
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="DeptGroupAdd" OnTextChanged="tbAccount_TextChanged" CssClass="TextBox" MaxLength="10" Runat="Server" Width="30%" AutoPostBack="true"/>
									<asp:TextBox Runat="server" ID="DeptGroupAddDesc" MaxLength="255" Width="50%" CssClass="TextBox" Enabled="false"/>
									<asp:Button class="btngo" runat="server" ID="btnDeptGroupAdd" Text="..." CommandName="SearchParentAdd"/>
									<cc1:TextBoxWatermarkExtender ID="DeptGroupAdd_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="DeptGroupAdd" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>
								</FooterTemplate>
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Action" headerstyle-width="126" >
								<ItemTemplate>
								    <asp:Button class="bitbtn btnedit" runat="server" ID="btnEdit" Text="Edit" CommandName="Edit" />																						 											
									<asp:Button class="bitbtn btndelete" runat="server" ID="btnDelete" Text="Delete" CommandName="Delete"  OnClientClick="return confirm('Sure to delete this data?');" />
								</ItemTemplate>
								<EditItemTemplate>
									<asp:Button class="bitbtn btnsave" runat="server" ID="btnUpdate" Text="Save" CommandName="Update" />																						 																		
									<asp:Button class="bitbtn btncancel" runat="server" ID="btnCancel" Text="Cancel" CommandName="Cancel" />																						 																		
								</EditItemTemplate>
								<FooterTemplate>
								   <asp:Button class="bitbtn btnadd" runat="server" ID="btnAdd" Text="Add" CommandName="Insert" />
								</FooterTemplate>
							</asp:TemplateField>
							
    					</Columns>
        </asp:GridView>
        
        </div>
    </form>
</body>

</html>
