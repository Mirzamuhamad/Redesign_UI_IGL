<%@ Page Language="VB" AutoEventWireup="false" CodeFile="MsPPH.aspx.vb" Inherits="MsPPH" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<%@ Register Assembly="BasicFrame.WebControls.BasicDatePicker" Namespace="BasicFrame.WebControls"
    TagPrefix="BDP" %>
    
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>PPH File</title>
    <script src="../../Function/OpenDlg.js" type="text/javascript"></script>
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
 <link href="https://fonts.googleapis.com/css2?family=Muli:wght@300;400;600&display=swap" rel="stylesheet">
</head>
<body>
  <form id="form1" runat="server">
      <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
     <div class="Content">
     <div class="H1">PPH File</div>
     <hr style="ConstructionIP:Blue" />
      <table>
        <tr>
            <td style="width:100px;text-align:right" >Quick Search :</td>
            <td><asp:TextBox runat="server" ID ="tbFilter" CssClass="TextBox"/> 
                <asp:DropDownList runat="server" ID="ddlField" CssClass="DropDownList" >
                  <asp:ListItem Selected="true" Text="PPH Code" Value="PPHCode"></asp:ListItem>
                  <asp:ListItem Text="PPH Name" Value="PPHName"></asp:ListItem> 
                  <asp:ListItem Text="Account" Value="Account"></asp:ListItem>
                  <asp:ListItem Text="AccountName" Value="AccountName"></asp:ListItem>       
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
                     <asp:ListItem Selected="true" Text="PPH Code" Value="PPHCode"></asp:ListItem>
                  <asp:ListItem Text="PPH Name" Value="PPHName"></asp:ListItem> 
                  <asp:ListItem Text="Account" Value="Account"></asp:ListItem>
                  <asp:ListItem Text="AccountName" Value="AccountName"></asp:ListItem>       
                  </asp:DropDownList>
                
            </td>
        </tr>
     </table>
     </asp:Panel>
      <br />
      <asp:GridView id="DataGrid" runat="server" width = "100%"
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
							<asp:TemplateField HeaderText="PPH Code" HeaderStyle-Width="100px" SortExpression="PPHCode">
								<Itemtemplate>
									<asp:Label Runat="server" ID="PPHCode" text='<%# DataBinder.Eval(Container.DataItem, "PPHCode") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:Label Runat="server" ID="PPHCodeEdit" Text='<%# DataBinder.Eval(Container.DataItem, "PPHCode") %>'>
									</asp:Label>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="PPHCodeAdd" CssClass="TextBox" MaxLength="3" Runat="Server" Width="100%"/>
									<cc1:TextBoxWatermarkExtender ID="PPHCodeAdd_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="PPHCodeAdd" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>
								</FooterTemplate>								
							</asp:TemplateField>							
							
							<asp:TemplateField HeaderText="PPH Name" HeaderStyle-Width="250px" SortExpression="PPHName">
								<Itemtemplate>
									<asp:Label Runat="server" ID="PPHName" text='<%# DataBinder.Eval(Container.DataItem, "PPHName") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:TextBox Runat="server" ID="PPHNameEdit" MaxLength="60" Width="100%" CssClass="TextBox" Text='<%# DataBinder.Eval(Container.DataItem, "PPHName") %>'>
									</asp:TextBox>
									<cc1:TextBoxWatermarkExtender ID="PPHNameEdit_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="PPHNameEdit" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>									
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="PPHNameAdd" CssClass="TextBox" MaxLength="60" Runat="Server" Width="100%"/>
									<cc1:TextBoxWatermarkExtender ID="PPHNameAdd_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="PPHNameAdd" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>
								</FooterTemplate>
							</asp:TemplateField>

							<%------------%>							

							<asp:TemplateField HeaderText="Type" HeaderStyle-Width="80" ItemStyle-HorizontalAlign="Center" SortExpression="Type">
								<Itemtemplate>
									<asp:Label Runat="server" Width="80" ID="Type" TEXT='<%# DataBinder.Eval(Container.DataItem, "Type") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:DropDownList ID="TypeEdit" CssClass="DropDownList" Width="100%" runat="server" SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Type") %>'>
									  <asp:ListItem>+</asp:ListItem>
									  <asp:ListItem>-</asp:ListItem>                                                                            
								</asp:DropDownList>								    
								</EditItemTemplate>
								<FooterTemplate>
									<asp:DropDownList ID="TypeAdd" CssClass="DropDownList" Width="100%" runat="server">
									  <asp:ListItem Selected="True">+</asp:ListItem>                                     
									  <asp:ListItem>-</asp:ListItem>                               
									</asp:DropDownList>								    
								</FooterTemplate>
								  <HeaderStyle Width="80px" />
                                <ItemStyle HorizontalAlign="Center" />
							</asp:TemplateField>
							
							
							<asp:TemplateField HeaderText="Account" HeaderStyle-Width="130px" SortExpression="Account">
								<Itemtemplate>
									<asp:Label Runat="server" ID="Account" text='<%# DataBinder.Eval(Container.DataItem, "Account") %>'>
									</asp:Label>
								</Itemtemplate>		
								<EditItemTemplate>
								    <asp:TextBox CssClass="TextBox" runat="server" ID="AccountEdit" Width="70%" 
                                        text='<%# DataBinder.Eval(Container.DataItem, "Account") %>' 
                                         AutoPostBack="true"/>
                                    <asp:Button class="btngo" runat="server" ID="btnAccEdit" Text="..." CommandName="SearchEdit" />                            
								</EditItemTemplate>	

								<FooterTemplate>
								    <asp:TextBox CssClass="TextBox" runat="server" id="AccountAdd" Width="70%" 
                                        AutoPostBack="true" />
                                    <asp:Button class="btngo" runat="server" ID="btnAccAdd" Text="..." CommandName="SearchAdd" />                
                                    <cc1:TextBoxWatermarkExtender ID="AccountAdd_TextBoxWatermarkExtender" 
                                        runat="server" Enabled="True" TargetControlID="AccountAdd" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                    </cc1:TextBoxWatermarkExtender>
								</FooterTemplate>                                
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Account Name" HeaderStyle-Width="200px" SortExpression="AccountName">
								<Itemtemplate>
									<asp:Label Runat="server" ID="AccountName" Width="100%" text='<%# DataBinder.Eval(Container.DataItem, "AccountName") %>'>
									</asp:Label>
								</Itemtemplate>	
								<EditItemTemplate>
								    <asp:Label Runat="server" ID="AccountNameEdit" Width="100%" text='<%# DataBinder.Eval(Container.DataItem, "AccountName") %>'>
									</asp:Label>
								</EditItemTemplate>	
								<FooterTemplate>
								    <asp:Label style = "color: black;" Runat="server" ID="AccountNameAdd">
									</asp:Label>
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
        
     <asp:Label ID="lstatus" ForeConstructionIP="red" runat="server"></asp:Label>    
    </div>
    </form>
</body>

</html>
