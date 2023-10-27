<%@ Page Language="VB" AutoEventWireup="false" CodeFile="MsPanenPremi.aspx.vb" Inherits="MsPanenPremi_MsPanenPremi" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<%@ Register Assembly="BasicFrame.WebControls.BasicDatePicker" Namespace="BasicFrame.WebControls"
    TagPrefix="BDP" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Panen Premi File</title>
    <script src="../../Function/Function.JS" type="text/javascript"></script>
    <script src="../../Function/OpenDlg.js" type="text/javascript"></script>
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
</head>
<body>
  <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>    
     <div class="Content">
     <div class="H1">Panen Premi File</div>
     <hr style="color:Blue" />   
      <table>
        <tr>
            <td style="width:100px;text-align:right" >Quick Search :</td>      
            
            <td>
                 <asp:TextBox runat="server" ID ="tbFilter" CssClass="TextBox"/> 
                <asp:DropDownList runat="server" ID="ddlField" CssClass="DropDownList" >
                  <asp:ListItem Selected="true" Text="Effective Date" Value="Effective_Date"></asp:ListItem>
                  <asp:ListItem Text="Premi Mandor" Value="PremiMandor"></asp:ListItem>        
                  <asp:ListItem Text="Premi Krani" Value="PremiKrani"></asp:ListItem> 
                  <asp:ListItem Text="Min Person In Team" Value="MinHKInTeam"></asp:ListItem>          
                </asp:DropDownList>                   
            
                    
                <asp:Button class="bitbtn btnsearch" runat="server" ID="btnSearch" Text="Search" />                
                <asp:Button class="btngo" runat="server" ID="btnExpand" Text="..."/>                
                <asp:Button class="bitbtn btnprint" runat="server" ID="btnPrint" Text="Print"/>               
                <asp:Button ID="btnShowPopup" runat="server" Style="display: none" />
                &nbsp &nbsp &nbsp                   
			    
                <cc1:ModalPopupExtender ID="ModalPopupExtender1" runat="server" 
                    PopupControlID="pnlFind" DropShadow="True" TargetControlID="btnShowPopup"                     
                    BackgroundCssClass="BackgroundStyle" />                           
                                   
            </td>
        </tr>
     </table>     
     <asp:Panel runat="server" ID="pnlSearch" Visible="false">
     <table>   
        <tr>
            <td style="width:160px;text-align:right">
            <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlNotasi" >
                    <asp:ListItem Selected="true" Text="OR" Value="OR"></asp:ListItem>
                    <asp:ListItem Text="AND" Value="AND"></asp:ListItem>         
                </asp:DropDownList>
            </td>
            <td><asp:TextBox runat="server" ID ="tbfilter2" CssClass="TextBox"/> 
                  <asp:DropDownList runat="server" ID="ddlField2" CssClass="DropDownList" >
                  <asp:ListItem Selected="true" Text="Effective Date" Value="Effective_Date"></asp:ListItem>
                  <asp:ListItem Text="Premi Mandor" Value="PremiMandor"></asp:ListItem>        
                  <asp:ListItem Text="Premi Krani" Value="PremiKrani"></asp:ListItem> 
                  <asp:ListItem Text="Min Person In Team" Value="MinHKInTeam"></asp:ListItem>         
                  </asp:DropDownList>
            </td>
        </tr>
     </table>
     </asp:Panel>        
     
      <br />
      <asp:GridView id="DataGrid" runat="server"  
            ShowFooter="True" AllowSorting="False" 
            AutoGenerateColumns="False" AllowPaging="True" CssClass="Grid">
						<HeaderStyle CssClass="GridHeader"></HeaderStyle>
						<RowStyle CssClass="GridItem" Wrap="false"/>
						<AlternatingRowStyle CssClass="GridAltItem"/>
						<FooterStyle CssClass="GridFooter" />
						<PagerStyle CssClass="GridPager" HorizontalAlign="Center" />					  
				<EmptyDataTemplate>
				    
				</EmptyDataTemplate>	  
				      <Columns>				      
							<asp:TemplateField HeaderText="Effective Date" HeaderStyle-Width="100" SortExpression="EffectiveDate">
								<Itemtemplate>
									<asp:Label Runat="server" ID="EffectiveDate" text='<%# DataBinder.Eval(Container.DataItem, "Effective_Date") %>'>
									</asp:Label>									
								</Itemtemplate>
								<EditItemTemplate>
									<BDP:BasicDatePicker ID="EffectiveDateEdit" runat="server" DateFormat="dd/MMM/yyyy" 
                                      ReadOnly = "true" Enabled="false"
                                      ButtonImageHeight="19px" ButtonImageWidth="20px" 
                                      DisplayType="TextBoxAndImage" 
                                      TextBoxStyle-CssClass="TextDate"
                                      ShowNoneButton="False" SelectedValue = '<%# DataBinder.Eval(Container.DataItem, "EffectiveDate") %>' >
                                     <TextBoxStyle CssClass="TextDate" />
                                    </BDP:BasicDatePicker>
								</EditItemTemplate>
								<FooterTemplate>
									<BDP:BasicDatePicker ID="EffectiveDateAdd" runat="server" DateFormat="dd/MMM/yyyy" 
                                      ReadOnly = "true" ValidationGroup="Input"
                                      ButtonImageHeight="19px" ButtonImageWidth="20px" 
                                      DisplayType="TextBoxAndImage" 
                                      TextBoxStyle-CssClass="TextDate"
                                      ShowNoneButton="False" >
                                     <TextBoxStyle CssClass="TextDate" />
                                    </BDP:BasicDatePicker>   
								</FooterTemplate>								
							</asp:TemplateField>							
							
							<asp:TemplateField HeaderText="Premi Mandor (%)" HeaderStyle-Width="100" SortExpression="PremiMandor">
								<Itemtemplate>
									<asp:Label Runat="server" ID="PremiMandor" text='<%# DataBinder.Eval(Container.DataItem, "PremiMandor") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:TextBox Runat="server" ID="PremiMandorEdit" MaxLength="30" Width="100%" CssClass="TextBox" Text='<%# DataBinder.Eval(Container.DataItem, "PremiMandor") %>'>
									</asp:TextBox>
									<cc1:TextBoxWatermarkExtender ID="PremiMandorEdit_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="PremiMandorEdit" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>									
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="PremiMandorAdd" CssClass="TextBox" MaxLength="30" Runat="Server" Width="100%"/>
									<cc1:TextBoxWatermarkExtender ID="PremiMandorAdd_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="PremiMandorAdd" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>
								</FooterTemplate>
							</asp:TemplateField>	
							
							<asp:TemplateField HeaderText="Premi Krani (%)" HeaderStyle-Width="100" SortExpression="PremiKrani">
								<Itemtemplate>
									<asp:Label Runat="server" ID="PremiKrani" text='<%# DataBinder.Eval(Container.DataItem, "PremiKrani") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:TextBox Runat="server" ID="PremiKraniEdit" MaxLength="30" Width="100%" CssClass="TextBox" Text='<%# DataBinder.Eval(Container.DataItem, "PremiKrani") %>'>
									</asp:TextBox>
									<cc1:TextBoxWatermarkExtender ID="PremiKranirEdit_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="PremiKraniEdit" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>									
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="PremiKraniAdd" CssClass="TextBox" MaxLength="30" Runat="Server" Width="100%"/>
									<cc1:TextBoxWatermarkExtender ID="PremiKraniAdd_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="PremiKraniAdd" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>
								</FooterTemplate>
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Min Person In Team" HeaderStyle-Width="100" SortExpression="MinHKInTeam">
								<Itemtemplate>
									<asp:Label Runat="server" ID="MinHKInTeam" text='<%# DataBinder.Eval(Container.DataItem, "MinHKInTeam") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:TextBox Runat="server" ID="MinHKInTeamEdit" MaxLength="30" Width="100%" CssClass="TextBox" Text='<%# DataBinder.Eval(Container.DataItem, "MinHKInTeam") %>'>
									</asp:TextBox>
									<cc1:TextBoxWatermarkExtender ID="MinHKInTeamEdit_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="MinHKInTeamEdit" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>									
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="MinHKInTeamAdd" CssClass="TextBox" MaxLength="30" Runat="Server" Width="100%"/>
									<cc1:TextBoxWatermarkExtender ID="MinHKInTeamAdd_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="MinHKInTeamAdd" 
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
     
    </div>
    </form>
</body>

</html>
