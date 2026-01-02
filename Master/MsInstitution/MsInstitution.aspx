<%@ Page Language="VB" AutoEventWireup="false" CodeFile="MsInstitution.aspx.vb" Inherits="MsInstitution_MsInstitution" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Institution File</title>
    <script src="../../Function/OpenDlg.js" type="text/javascript"></script>
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
 <link href="https://fonts.googleapis.com/css2?family=Muli:wght@300;400;600&display=swap" rel="stylesheet">
</head>
<body>
  <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <%--<asp:UpdatePanel ID="SSSS" runat="server">--%>
    <%--    <ContentTemplate>--%>
    <%--    </ContentTemplate>--%>
    <%--</asp:UpdatePanel>--%>
     <div class="Content">
     <%--<asp:UpdatePanel ID="UpdAJAXMain" runat="server">--%>
     <%--   <ContentTemplate>--%>
     <div class="H1">Institution File</div>
     <hr style="color:Blue" />
      <table>
        <tr>
            <td style="width:100px;text-align:right" >Quick Search :</td>
            <td><asp:TextBox runat="server" ID ="tbFilter" CssClass="TextBox"/> 
                <asp:DropDownList runat="server" ID="ddlField" CssClass="DropDownList" >
                  <asp:ListItem Selected="true" Text="Institution Code" Value="InstitutionCode"></asp:ListItem>
                  <asp:ListItem Text="Institution Name" Value="InstitutionName"></asp:ListItem>        
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
                    <asp:ListItem Selected="true" Text="Institution Code" Value="InstitutionCode"></asp:ListItem>
                  <asp:ListItem Text="Institution Name" Value="InstitutionName"></asp:ListItem>        
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
						<PagerStyle CssClass="GridPager" HorizontalAlign="Center" />					  
				<EmptyDataTemplate>
				    
				</EmptyDataTemplate>	  
				      <Columns>				      
							<asp:TemplateField HeaderText="Institution Code" HeaderStyle-Width="100" SortExpression="InstitutionCode">
								<Itemtemplate>
									<asp:Label Runat="server" ID="InstitutionCode" text='<%# DataBinder.Eval(Container.DataItem, "InstitutionCode") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:Label Runat="server" ID="InstitutionCodeEdit" Text='<%# DataBinder.Eval(Container.DataItem, "InstitutionCode") %>'>
									</asp:Label>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="InstitutionCodeAdd" CssClass="TextBox" MaxLength="5" Runat="Server" Width="100%"/>
									<cc1:TextBoxWatermarkExtender ID="InstitutionCodeAdd_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="InstitutionCodeAdd" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>
								</FooterTemplate>								
							</asp:TemplateField>							
							
							<asp:TemplateField HeaderText="Institution Name" HeaderStyle-Width="368" SortExpression="InstitutionName">
								<Itemtemplate>
									<asp:Label Runat="server" ID="InstitutionName" text='<%# DataBinder.Eval(Container.DataItem, "InstitutionName") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:TextBox Runat="server" ID="InstitutionNameEdit" MaxLength="50" Width="100%" CssClass="TextBox" Text='<%# DataBinder.Eval(Container.DataItem, "InstitutionName") %>'>
									</asp:TextBox>
									<cc1:TextBoxWatermarkExtender ID="InstitutionNameEdit_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="InstitutionNameEdit" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>									
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="InstitutionNameAdd" CssClass="TextBox" MaxLength="50" Runat="Server" Width="100%"/>
									<cc1:TextBoxWatermarkExtender ID="InstitutionNameAdd_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="InstitutionNameAdd" 
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
        
     <asp:Label ID="lstatus" ForeColor="red" runat="server"></asp:Label>
     <asp:Panel ID="pnlFind" runat="server"> </asp:Panel> 
     <%--</ContentTemplate>--%>
     <%--</asp:UpdatePanel>--%>  
    </div>
    </form>
</body>

</html>
