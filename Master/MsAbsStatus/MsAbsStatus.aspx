<%@ Page Language="VB" AutoEventWireup="false" CodeFile="MsAbsStatus.aspx.vb" Inherits="MsAbsStatus_MsAbsStatus" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Absen Status File</title>
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
     <div class="H1">Absen Status File</div>
     <hr style="color:Blue" />
      <table>
        <tr>
            <td style="width:100px;text-align:right" >Quick Search :</td>
            <td><asp:TextBox runat="server" ID ="tbFilter" CssClass="TextBox"/> 
                <asp:DropDownList runat="server" ID="ddlField" CssClass="DropDownList" >
                  <asp:ListItem Selected="true" Text="Absen Status Code" Value="AbsStatusCode"></asp:ListItem>
                  <asp:ListItem Text="Absen Status Name" Value="AbsStatusName"></asp:ListItem>        
                  <asp:ListItem Text="Absence" Value="FgAbsence"></asp:ListItem>        
                  <asp:ListItem Text="MinusAbsensi" Value="MinusAbsensi"></asp:ListItem>                          
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
                    <asp:ListItem Selected="true" Text="Absen Status Code" Value="AbsStatusCode"></asp:ListItem>
                   <asp:ListItem Text="Absen Status Name" Value="AbsStatusName"></asp:ListItem>        
                   <asp:ListItem Text="Absence" Value="FgAbsence"></asp:ListItem>        
                   <asp:ListItem Text="MinusAbsensi" Value="MinusAbsensi"></asp:ListItem>                          
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
							<asp:TemplateField HeaderText="Absen Status Code" HeaderStyle-Width="100" SortExpression="AbsStatusCode">
								<Itemtemplate>
									<asp:Label Runat="server" ID="AbsStatusCode" text='<%# DataBinder.Eval(Container.DataItem, "AbsStatusCode") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:Label Runat="server" ID="AbsStatusCodeEdit" Text='<%# DataBinder.Eval(Container.DataItem, "AbsStatusCode") %>'>
									</asp:Label>								
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="AbsStatusCodeAdd" CssClass="TextBox" MaxLength="3" Runat="Server" Width="100%"/>
									<cc1:TextBoxWatermarkExtender ID="AbsStatusCodeAdd_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="AbsStatusCodeAdd" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked"   >
                                     </cc1:TextBoxWatermarkExtender>
								</FooterTemplate>								
							</asp:TemplateField>							
							
							<asp:TemplateField HeaderText="Absen Status Name" HeaderStyle-Width="368" SortExpression="AbsStatusName">
								<Itemtemplate>
									<asp:Label Runat="server" ID="AbsStatusName" text='<%# DataBinder.Eval(Container.DataItem, "AbsStatusName") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:TextBox Runat="server" ID="AbsStatusNameEdit" MaxLength="50" Width="100%" CssClass="TextBox" Text='<%# DataBinder.Eval(Container.DataItem, "AbsStatusName") %>'>
									</asp:TextBox>
									<cc1:TextBoxWatermarkExtender ID="AbsStatusNameEdit_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="AbsStatusNameEdit" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>									
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="AbsStatusNameAdd" CssClass="TextBox" MaxLength="50" Runat="Server" Width="100%"/>
									<cc1:TextBoxWatermarkExtender ID="AbsStatusNameAdd_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="AbsStatusNameAdd" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>
								</FooterTemplate>
							</asp:TemplateField>	
							
							 <asp:TemplateField HeaderText="Absence" HeaderStyle-Width="30" ItemStyle-HorizontalAlign="Center" SortExpression="FgAbsence">
								<Itemtemplate>
									<asp:Label Runat="server" Width="30" ID="Absence" TEXT='<%# DataBinder.Eval(Container.DataItem, "FgAbsence") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:DropDownList ID="AbsenceEdit" CssClass="DropDownList" Width="100%" runat="server" SelectedValue='<%# DataBinder.Eval(Container.DataItem, "FgAbsence") %>'>
									  <asp:ListItem>Y</asp:ListItem>
									  <asp:ListItem>N</asp:ListItem>                                        									  
								</asp:DropDownList>								    
								</EditItemTemplate>
								<FooterTemplate>
									<asp:DropDownList ID="AbsenceAdd" CssClass="DropDownList" Width="100%" runat="server">
									  <asp:ListItem Selected="True">Y</asp:ListItem>
									  <asp:ListItem>N</asp:ListItem>                                        									  
									</asp:DropDownList>								    
								</FooterTemplate>
							    <HeaderStyle Width="30px" />
                                <ItemStyle HorizontalAlign="Center" />
							</asp:TemplateField>				
							
							
							
							<asp:TemplateField HeaderText="Minus Absensi" HeaderStyle-Width="50" ItemStyle-HorizontalAlign = "Right" SortExpression="MinusAbsensi">
								<Itemtemplate>
									<asp:Label Runat="server" ID="MinusAbsensi" text='<%# DataBinder.Eval(Container.DataItem, "MinusAbsensi") %>'>									
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:TextBox Runat="server" ID="MinusAbsensiEdit" MaxLength="9" Width="100%" CssClass="TextBox" Text='<%# DataBinder.Eval(Container.DataItem, "MinusAbsensi") %>'>									
									</asp:TextBox>
									 <cc1:TextBoxWatermarkExtender ID="MinusAbsensiEdit_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="MinusAbsensiEdit" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>	
                                     								                                    
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="MinusAbsensiAdd" CssClass="TextBox" MaxLength="9" Runat="Server" Width="100%"/>
									
									 <cc1:TextBoxWatermarkExtender  ID="MinusAbsensiAdd_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="MinusAbsensiAdd" 
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
								   <%--<asp:Button CommandName="Insert" Text="Add" CssClass="Button" ID="btnAdd" Runat="server" Width="95" />--%>
								   <asp:Button class="bitbtn btnadd" runat="server" ID="btnAdd" Text="Add" CommandName="Insert" />																						 																		
									
								</FooterTemplate>
							</asp:TemplateField>
							
    					</Columns>
        </asp:GridView>
        
     <asp:Label ID="lstatus" ForeColor="red" runat="server"></asp:Label>     
     </ContentTemplate>
     </asp:UpdatePanel>  
    </div>
    </form>
</body>

</html>
