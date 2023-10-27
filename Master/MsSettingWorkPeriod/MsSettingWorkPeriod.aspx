<%@ Page Language="VB" AutoEventWireup="false" CodeFile="MsSettingWorkPeriod.aspx.vb" Inherits="MsSettingWorkPeriod_MsSettingWorkPeriod" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Country File</title>
    <script src="../../Function/Function.JS" type="text/javascript"></script>
    <script src="../../Function/OpenDlg.js" type="text/javascript"></script>
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
</head>
<body>
  <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
     <div class="Content">
     <div class="H1">Setting Work Period File</div>
     <hr style="color:Blue" />       
        
   
      <table>
        <tr>
            <td style="width:100px;text-align:right" >Quick Search :</td>
            <td>
             <asp:DropDownList runat="server" ID="ddlType" CssClass="DropDownList" OnSelectedIndexChanged =  "ddlType_SelectedIndexChanged" AutoPostBack ="true"  >
                  <asp:ListItem Selected="true" Text="Masa Kerja" ></asp:ListItem>
                  <asp:ListItem Text="Pesangon" ></asp:ListItem>        
                  <asp:ListItem Text="Penghargaan" ></asp:ListItem>        
                  <asp:ListItem Text="Pisah" ></asp:ListItem>                          
                  <asp:ListItem Text="THR" ></asp:ListItem>                                            
                </asp:DropDownList> 
            </td>    
            <td><asp:TextBox runat="server" ID ="tbFilter" CssClass="TextBox"/> 
                <asp:DropDownList runat="server" ID="ddlField" CssClass="DropDownList" >
                  <asp:ListItem Selected="true" Text="Start Year" Value="StartYear"></asp:ListItem>
                  <asp:ListItem Text="End Year" Value="EndYear"></asp:ListItem>        
                  <asp:ListItem Text="Base On" Value="BaseOn"></asp:ListItem>        
                  <asp:ListItem Text="Formula" Value="Formula"></asp:ListItem>                          
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
     <br />
     <asp:Panel runat="server" ID="pnlSearch" Visible="false">
     <table>   
        <tr>
            <td style="width:100px;text-align:right" ></td>
            <td style="width:100px;text-align:right"><asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlNotasi" >
                    <asp:ListItem Selected="true" Text="OR" Value="OR"></asp:ListItem>
                    <asp:ListItem Text="AND" Value="AND"></asp:ListItem>         
                </asp:DropDownList>
            </td>
            <td><asp:TextBox runat="server" ID ="tbfilter2" CssClass="TextBox"/> 
                  <asp:DropDownList runat="server" ID="ddlField2" CssClass="DropDownList" >
                     <asp:ListItem Selected="true" Text="Start Year" Value="StartYear"></asp:ListItem>
                     <asp:ListItem Text="End Year" Value="EndYear"></asp:ListItem>        
                     <asp:ListItem Text="Base On" Value="BaseOn"></asp:ListItem>        
                     <asp:ListItem Text="Formula" Value="Formula"></asp:ListItem>  
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
							<asp:TemplateField HeaderText="Allowance Type" HeaderStyle-Width="110" SortExpression="TunjanganType">
								<Itemtemplate>
									<asp:Label Runat="server" ID="TunjanganType" text='<%# DataBinder.Eval(Container.DataItem, "TunjanganType") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:Label Runat="server" ID="TunjanganTypeEdit" Text='<%# DataBinder.Eval(Container.DataItem, "TunjanganType") %>'>
									</asp:Label>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:Label Runat="server" ID="TunjanganTypeAdd"  >
									</asp:Label>
								</FooterTemplate>								
							</asp:TemplateField>							
							
							<asp:TemplateField HeaderText="Start Year" ItemStyle-HorizontalAlign = "Right" HeaderStyle-Width="70" SortExpression="StartYear">
								<Itemtemplate>
									<asp:Label Runat="server" ID="StartYear" text='<%# DataBinder.Eval(Container.DataItem, "StartYear") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
								    <asp:Label Runat="server" ID="StartYearEdit" text='<%# DataBinder.Eval(Container.DataItem, "StartYear") %>'>
									</asp:Label>
									<%--<asp:TextBox Runat="server" ID="StartYearEdit" MaxLength="4" Width="100%" CssClass="TextBox" Text='<%# DataBinder.Eval(Container.DataItem, "StartYear") %>'>
									</asp:TextBox>
									<cc1:TextBoxWatermarkExtender ID="StartYearEdit_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="StartYearEdit" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>								--%>	
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="StartYearAdd" CssClass="TextBox" MaxLength="4" Runat="Server" Width="100%"/>
									<cc1:TextBoxWatermarkExtender ID="StartYearAdd_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="StartYearAdd" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>
								</FooterTemplate>
							</asp:TemplateField>		
							
							<asp:TemplateField HeaderText="End Year" HeaderStyle-Width="70" itemStyle-HorizontalAlign = "Right" SortExpression="EndYear">
								<Itemtemplate>
									<asp:Label Runat="server" ID="EndYear" text='<%# DataBinder.Eval(Container.DataItem, "EndYear") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:TextBox Runat="server" ID="EndYearEdit" MaxLength="4" Width="100%" CssClass="TextBox" Text='<%# DataBinder.Eval(Container.DataItem, "EndYear") %>'>
									</asp:TextBox>
									<cc1:TextBoxWatermarkExtender ID="EndYearEdit_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="EndYearEdit" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>									
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="EndYearAdd" CssClass="TextBox" MaxLength="4" Runat="Server" Width="100%"/>
									<cc1:TextBoxWatermarkExtender ID="EndYearAdd_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="EndYearAdd" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>
								</FooterTemplate>
							</asp:TemplateField>		
							
							
							<asp:TemplateField HeaderText="BaseOn" HeaderStyle-Width="80" SortExpression="BaseOn">
								<Itemtemplate>
									<asp:Label Runat="server" Width="30" ID="BaseOn" text='<%# DataBinder.Eval(Container.DataItem, "BaseOn") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:DropDownList ID="BaseOnEdit" CssClass="DropDownList" Width="100%" runat="server" SelectedValue='<%# DataBinder.Eval(Container.DataItem, "BaseOn") %>'>
									  <asp:ListItem>Amount</asp:ListItem>
									  <asp:ListItem>Wages</asp:ListItem>                                        
									</asp:DropDownList>								    
								</EditItemTemplate>
								<FooterTemplate>
									<asp:DropDownList ID="BaseOnAdd" CssClass="DropDownList" Width="100%" runat="server">
									  <asp:ListItem Selected="True">Amount</asp:ListItem>
									  <asp:ListItem>Wages</asp:ListItem>                                        
									</asp:DropDownList>								    
								</FooterTemplate>
							    <HeaderStyle Width="80px" />
                                <ItemStyle HorizontalAlign="Left" />
							</asp:TemplateField>	


							<asp:TemplateField HeaderText="Formula" HeaderStyle-Width="150" itemStyle-HorizontalAlign = "Right"  SortExpression="Formula">
								<Itemtemplate>
									<asp:Label Runat="server" ID="Formula" text='<%# DataBinder.Eval(Container.DataItem, "Formula") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:TextBox Runat="server" ID="FormulaEdit" MaxLength="9" Width="100%" CssClass="TextBox" Text='<%# DataBinder.Eval(Container.DataItem, "Formula") %>'>
									</asp:TextBox>
									<cc1:TextBoxWatermarkExtender ID="FormulaEdit_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="FormulaEdit" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>									
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="FormulaAdd" CssClass="TextBox" MaxLength="9" Runat="Server" Width="100%"/>
									<cc1:TextBoxWatermarkExtender ID="FormulaAdd_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="FormulaAdd" 
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
