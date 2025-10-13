<%@ Page Language="VB" AutoEventWireup="false" CodeFile="MsReligion.aspx.vb" Inherits="MsReligion_MsReligion" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Religion File</title>
    <script src="../../Function/OpenDlg.js" type="text/javascript"></script>
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
 <link href="https://fonts.googleapis.com/css2?family=Muli:wght@300;400;600&display=swap" rel="stylesheet">
</head>
<body>
  <form id="form1" runat="server">
      <asp:ScriptManager ID="ScriptManager1" runat="server">
      </asp:ScriptManager>
      
      <div class="Content">
     <div class="H1">Religion File</div>
     <hr style="color:Blue" />
      <table>
        <tr>
            <td style="width:100px;text-align:right" >Quick Search :</td>
            <td><asp:TextBox runat="server" ID ="tbFilter" CssClass="TextBox"/> 
                <asp:DropDownList runat="server" ID="ddlField" CssClass="DropDownList" >
                  <asp:ListItem Selected="true" Text="Religion Code" Value="ReligionCode"></asp:ListItem>
                  <asp:ListItem Text="Religion Name" Value="ReligionName"></asp:ListItem>        
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
                    <asp:ListItem Selected="true" Text="Religion Code" Value="ReligionCode"></asp:ListItem>
                  <asp:ListItem Text="Religion Name" Value="ReligionName"></asp:ListItem>        
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
							<asp:TemplateField HeaderText="Religion Code" HeaderStyle-Width="100" SortExpression="ReligionCode">
								<Itemtemplate>
									<asp:Label Runat="server" ID="ReligionCode" text='<%# DataBinder.Eval(Container.DataItem, "ReligionCode") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:Label Runat="server" ID="ReligionCodeEdit" Text='<%# DataBinder.Eval(Container.DataItem, "ReligionCode") %>'>
									</asp:Label>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="ReligionCodeAdd" CssClass="TextBox" MaxLength="5" Runat="Server" Width="100%"/>
									<cc1:TextBoxWatermarkExtender ID="ReligionCodeAdd_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="ReligionCodeAdd" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>
								</FooterTemplate>								
							</asp:TemplateField>							
							
							<asp:TemplateField HeaderText="Religion Name" HeaderStyle-Width="300" SortExpression="ReligionName">
								<Itemtemplate>
									<asp:Label Runat="server" ID="ReligionName" text='<%# DataBinder.Eval(Container.DataItem, "ReligionName") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:TextBox Runat="server" ID="ReligionNameEdit" MaxLength="60" Width="100%" CssClass="TextBox" Text='<%# DataBinder.Eval(Container.DataItem, "ReligionName") %>'>
									</asp:TextBox>
									<cc1:TextBoxWatermarkExtender ID="ReligionNameEdit_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="ReligionNameEdit" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>									
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="ReligionNameAdd" CssClass="TextBox" MaxLength="60" Runat="Server" Width="100%"/>
									<cc1:TextBoxWatermarkExtender ID="ReligionNameAdd_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="ReligionNameAdd" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>
								</FooterTemplate>
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Innatura" HeaderStyle-Width="150" SortExpression="FgInnatura">
								<Itemtemplate>
									<asp:Label Runat="server" ID="Innatura" text='<%# DataBinder.Eval(Container.DataItem, "FgInnatura") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<%--<asp:TextBox Runat="server" ID="InnaturaEdit" MaxLength="60" Width="100%" CssClass="TextBox" Text='<%# DataBinder.Eval(Container.DataItem, "FgInnatura") %>'>
									</asp:TextBox>--%>
									<asp:DropDownList ID="InnaturaEdit" CssClass="DropDownList" Width="100%" runat="server" SelectedValue='<%# DataBinder.Eval(Container.DataItem, "FgInnatura") %>'>
									  <asp:ListItem>N</asp:ListItem>
									  <asp:ListItem>Y</asp:ListItem>                                        
									</asp:DropDownList>
									<%--<cc1:TextBoxWatermarkExtender ID="InnaturaEdit_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="InnaturaEdit" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>--%>									
								</EditItemTemplate>
								<FooterTemplate>
									<%--<asp:TextBox ID="InnaturaAdd" CssClass="TextBox" MaxLength="60" Runat="Server" Width="100%"/>--%>
									<%--<cc1:TextBoxWatermarkExtender ID="InnaturaAdd_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="InnaturaAdd" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>--%>
                                     <asp:DropDownList ID="InnaturaEditAdd" CssClass="DropDownList" Width="100%" runat="server">
									  <asp:ListItem Selected="True">N</asp:ListItem>
									  <asp:ListItem>Y</asp:ListItem>                                        
									</asp:DropDownList>
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
