<%@ Page Language="VB" AutoEventWireup="false" CodeFile="MsMerk.aspx.vb" Inherits="MsMerk_MsMerk" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Grade File</title>
    <script src="../../Function/OpenDlg.js" type="text/javascript"></script>
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
 <link href="https://fonts.googleapis.com/css2?family=Muli:wght@300;400;600&display=swap" rel="stylesheet">
  

   

</head>
<body>
  <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
     <div class="Content">
     <div class="H1">Merk File</div>
     <hr style="color:Blue" />     
    
      <table>
        <tr>
            <td style="width:100px;text-align:right" >Quick Search :</td>
            <td><asp:TextBox runat="server" ID ="tbFilter" CssClass="TextBox"/> 
                <asp:DropDownList runat="server" ID="ddlField" CssClass="DropDownList" >
                  <asp:ListItem Selected="true" Text="Merk Code" Value="MerkCode"></asp:ListItem>
                  <asp:ListItem Text="Merk Name" Value="MerkName"></asp:ListItem>        
                </asp:DropDownList>     
                <%--<asp:Button runat="server" ID="btnSearch" Text="Search" CssClass="Button" />--%>
            
                    
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
                    <asp:ListItem Selected="true" Text="Merk Code" Value="MerkCode"></asp:ListItem>
                  <asp:ListItem Text="Merk Name" Value="MerkName"></asp:ListItem>        
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
							<asp:TemplateField HeaderText="Merk Code" HeaderStyle-Width="100" SortExpression="MerkCode">
								<Itemtemplate>
									<asp:Label Runat="server" ID="MerkCode" text='<%# DataBinder.Eval(Container.DataItem, "MerkCode") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:Label Runat="server" ID="MerkCodeEdit" Text='<%# DataBinder.Eval(Container.DataItem, "MerkCode") %>'>
									</asp:Label>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="MerkCodeAdd" CssClass="TextBox" MaxLength="3" Runat="Server" Width="100%"/>
<%--									<asp:RegularExpressionValidator ID="rgv" runat="server" ControlToValidate="MerkCodeAdd" ErrorMessage="Please enter valid data."
                                      ValidationExpression="([a-z]|[A-Z]|[0-9]|[ ]|[-]|[_])*">
                                     </asp:RegularExpressionValidator>--%>
									 <cc1:TextBoxWatermarkExtender ID="MerkCodeAdd_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="MerkCodeAdd" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>
								</FooterTemplate>								
							</asp:TemplateField>							
							
							<asp:TemplateField HeaderText="Merk Name" HeaderStyle-Width="368" SortExpression="MerkName">
								<Itemtemplate>
									<asp:Label Runat="server" ID="MerkName" text='<%# DataBinder.Eval(Container.DataItem, "MerkName") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:TextBox Runat="server" ID="MerkNameEdit" MaxLength="60" Width="100%" CssClass="TextBox" Text='<%# DataBinder.Eval(Container.DataItem, "MerkName") %>'>
									</asp:TextBox>									
									 <%--<asp:RegularExpressionValidator ID="rgv1" runat="server" ControlToValidate="MerkNameEdit" ErrorMessage="Please enter valid data."
                                      ValidationExpression="([a-z]|[A-Z]|[0-9]|[ ]|[-]|[_])*"  Display="Static"> 
                                     </asp:RegularExpressionValidator>		--%>							
									<cc1:TextBoxWatermarkExtender ID="MerkNameEdit_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="MerkNameEdit" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>	                                     							
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="MerkNameAdd" CssClass="TextBox" MaxLength="60" Runat="Server" Width="100%"/>
<%--									<asp:RegularExpressionValidator ID="rgv2" runat="server" ControlToValidate="MerkNameAdd" ErrorMessage="Please enter valid data."
                                      ValidationExpression="([a-z]|[A-Z]|[0-9]|[ ]|[-]|[_])*">
                                     </asp:RegularExpressionValidator>--%>
									<cc1:TextBoxWatermarkExtender ID="MerkNameAdd_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="MerkNameAdd" 
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

    </div>
    </form>
</body>

</html>
