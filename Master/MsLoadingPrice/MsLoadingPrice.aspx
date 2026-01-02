<%@ Page Language="VB" AutoEventWireup="false" CodeFile="MsLoadingPrice.aspx.vb" Inherits="MsLoadingPrice_MsLoadingPrice" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<%@ Register Assembly="BasicFrame.WebControls.BasicDatePicker" Namespace="BasicFrame.WebControls"
    TagPrefix="BDP" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Loading Price File</title>
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
     <div class="H1">Loading Price File</div>
     <hr style="color:Blue" />   
      <table>
        <tr>
            <td style="width:100px;text-align:right" >Quick Search :</td>      
            
            <td>
                 <asp:TextBox runat="server" ID ="tbFilter" CssClass="TextBox"/> 
                <asp:DropDownList runat="server" ID="ddlField" CssClass="DropDownList" >
                  <asp:ListItem Selected="true" Text="Effective Date" Value="Effective_Date"></asp:ListItem>
                  <asp:ListItem Text="Price Bongkar (Rp)" Value="PriceBongkar"></asp:ListItem>        
                  <asp:ListItem Text="Price Muat" Value="PriceMuat"></asp:ListItem>        
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
                  <asp:ListItem Text="Price Bongkar (Rp)" Value="PriceBongkar"></asp:ListItem>        
                  <asp:ListItem Text="Price Muat" Value="PriceMuat"></asp:ListItem>        
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
							
							<asp:TemplateField HeaderText="Price Bongkar" HeaderStyle-Width="100" SortExpression="PriceBongkar">
								<Itemtemplate>
									<asp:Label Runat="server" ID="PriceBongkar" text='<%# Format(DataBinder.Eval(Container.DataItem, "PriceBongkar"),"#,##0.00") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:TextBox Runat="server" ID="PriceBongkarEdit" MaxLength="30" Width="100%" CssClass="TextBox" Text='<%# DataBinder.Eval(Container.DataItem, "PriceBongkar") %>'>
									</asp:TextBox>
									<cc1:TextBoxWatermarkExtender ID="PriceBongkarEdit_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="PriceBongkarEdit" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>									
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="PriceBongkarAdd" CssClass="TextBox" MaxLength="30" Runat="Server" Width="100%"/>
									<cc1:TextBoxWatermarkExtender ID="PriceBongkarAdd_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="PriceBongkarAdd" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>
								</FooterTemplate>
							</asp:TemplateField>	
							
							<asp:TemplateField HeaderText="Price Muat" HeaderStyle-Width="100" SortExpression="PriceMuat">
								<Itemtemplate>
									<asp:Label Runat="server" ID="PriceMuat" text='<%# Format(DataBinder.Eval(Container.DataItem, "PriceMuat"),"#,##0.00") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:TextBox Runat="server" ID="PriceMuatEdit" MaxLength="30" Width="100%" CssClass="TextBox" Text='<%# DataBinder.Eval(Container.DataItem, "PriceMuat") %>'>
									</asp:TextBox>
									<cc1:TextBoxWatermarkExtender ID="PriceMuatrEdit_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="PriceMuatEdit" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>									
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="PriceMuatAdd" CssClass="TextBox" MaxLength="30" Runat="Server" Width="100%"/>
									<cc1:TextBoxWatermarkExtender ID="PriceMuatAdd_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="PriceMuatAdd" 
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
