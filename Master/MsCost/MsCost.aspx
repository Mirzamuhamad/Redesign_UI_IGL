<%@ Page Language="VB" AutoEventWireup="false" CodeFile="MsCost.aspx.vb" Inherits="MsCost_MsCost" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Stock Type File</title>
    <script src="../../Function/OpenDlg.js" type="text/javascript"></script>    
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
     <div class="Content">
     <div class="H1">Cost File</div>
     <hr style="color:Blue" />
     
     <%--<asp:UpdatePanel ID="UpdAJAXHd" runat="server">
        <ContentTemplate>      --%>
      <table>
        <tr>
            <td style="width:100px;text-align:right" >Quick Search :</td>
            <td><asp:TextBox runat="server" ID ="tbFilter" CssClass="TextBox" />             
                <asp:DropDownList runat="server" ID="ddlField" CssClass="DropDownList" > 
                    <asp:ListItem Selected="true" Text="Cost Code" Value="CostFreightCode"></asp:ListItem>
                    <asp:ListItem Text="Cost Name" Value="CostFreightName"></asp:ListItem>                   
                    <asp:ListItem Text="Account Expense Name" Value="AccExpName"></asp:ListItem>                   
                    <asp:ListItem Text="Account Adjust Name" Value="AccAdjName"></asp:ListItem>                   
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
                    <asp:ListItem Selected="true" Text="Cost Code" Value="CostFreightCode"></asp:ListItem>
                    <asp:ListItem Text="Cost Name" Value="CostFreightName"></asp:ListItem> 
                    <asp:ListItem Text="Account Expense Name" Value="AccExpName"></asp:ListItem>                   
                    <asp:ListItem Text="Account Adjust Name" Value="AccAdjName"></asp:ListItem>                   
                  </asp:DropDownList>
                
            </td>
        </tr>
     </table>
     </asp:Panel>
      <%--</ContentTemplate>
        </asp:UpdatePanel>--%>
      <br />
       <%--<asp:UpdatePanel ID="UpdAJAXDt" runat="server">
        <ContentTemplate>   --%>
      <div style="border:0px  solid; width:100%; height:100%; overflow:auto;">
      <asp:GridView id="DataGrid" width = "1000" runat="server" ShowFooter="False" AllowSorting="True" 
            AutoGenerateColumns="False" AllowPaging="True" CssClass="Grid">
						<HeaderStyle CssClass="GridHeader"></HeaderStyle>
						<RowStyle CssClass="GridItem" Wrap="FAlse"/>
						<AlternatingRowStyle CssClass="GridAltItem"/>
						<FooterStyle CssClass="GridFooter" />
						<PagerStyle CssClass="GridPager" />
				      <EmptyDataTemplate>
				    
				      </EmptyDataTemplate>	
				      <Columns>				      
							<asp:TemplateField HeaderText="Code" HeaderStyle-Width="80px" 
                                SortExpression="CostFreightCode">
								<Itemtemplate>
									<asp:Label Runat="server" ID="CostCode" text='<%# DataBinder.Eval(Container.DataItem, "CostFreightCode") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:Label Runat="server" ID="CostCodeEdit" Text='<%# DataBinder.Eval(Container.DataItem, "CostFreightCode") %>'>
									</asp:Label>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="CostCodeAdd" CssClass="TextBox" MaxLength="5" Runat="Server" Width="100%"/>
									<cc1:TextBoxWatermarkExtender ID="CostCodeAdd_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="CostCodeAdd" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>
								</FooterTemplate>


							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Name" HeaderStyle-Width="300px" 
                                SortExpression="CostFreightName">
								<Itemtemplate>
									<asp:Label Runat="server" ID="CostName" text='<%# DataBinder.Eval(Container.DataItem, "CostFreightName") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:TextBox Runat="server" ID="CostNameEdit" MaxLength="60" CssClass="TextBox" Width="100%" Text='<%# DataBinder.Eval(Container.DataItem, "CostFreightName") %>'>
									</asp:TextBox>
									<cc1:TextBoxWatermarkExtender ID="CostNameEdit_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="CostNameEdit" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="CostNameAdd" Runat="Server" MaxLength="60" CssClass="TextBox" Width="100%"/>
									<cc1:TextBoxWatermarkExtender ID="CostNameAdd_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="CostNameAdd" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>
								</FooterTemplate>

							</asp:TemplateField>									
							
							
							<asp:TemplateField HeaderText="Account Expense" HeaderStyle-Width="15%" 
                                SortExpression="AccTransit">
								<Itemtemplate>
									<asp:Label Runat="server" ID="AccountExp" text='<%# DataBinder.Eval(Container.DataItem, "AccTransit") %>'>
									</asp:Label>
								</Itemtemplate>	
								<EditItemTemplate>
									<asp:TextBox Runat="server" ID="AccountExpEdit" MaxLength="12" Width="75%" CssClass="TextBox" 
									Text='<%# DataBinder.Eval(Container.DataItem, "AccTransit") %>'
									ontextchanged="AccountExpEdit_TextChanged" AutoPostBack="true"/>
									<cc1:TextBoxWatermarkExtender ID="AccountExpEdit_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="AccountExpEdit" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                    </cc1:TextBoxWatermarkExtender>		
                                    <asp:Button class="btngo" runat="server" ID="btnAccExpEdit" CommandName ="btnAccExpEdit" Text="..." />
								</EditItemTemplate>
								
								<FooterTemplate>								
									<asp:TextBox ID="AccountExpAdd" CssClass="TextBox" MaxLength="12" Width="75%" 
									ontextchanged="AccountExpAdd_TextChanged" AutoPostBack="true" Runat="Server" />
									<cc1:TextBoxWatermarkExtender ID="AccountExpAdd_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="AccountExpAdd" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                    </cc1:TextBoxWatermarkExtender>			
                                    <asp:Button class="btngo" runat="server" ID="btnAccExpAdd" CommandName ="btnAccExpAdd" Text="..."  />																						
								</FooterTemplate>															                                
							</asp:TemplateField>	
							
							
							<asp:TemplateField HeaderText="Acc. Expense Name" HeaderStyle-Width="200px" SortExpression="AccExpName">
								<Itemtemplate>
									<asp:Label Runat="server" ID="AccountExpName" text='<%# DataBinder.Eval(Container.DataItem, "AccExpName") %>'>
									</asp:Label>
								</Itemtemplate>	
								<EditItemTemplate>
									<asp:Label Runat="server" ID="AccountExpNameEdit" Text='<%# DataBinder.Eval(Container.DataItem, "AccExpName") %>'>
									</asp:Label>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:Label Runat="server" ID="AccountExpNameAdd">
									</asp:Label>
								</FooterTemplate>							
							</asp:TemplateField>
							
                            <asp:TemplateField HeaderText="Acc. Adjustment" HeaderStyle-Width="15%" 
                                SortExpression="AccAdjust">
								<Itemtemplate>
									<asp:Label Runat="server" ID="AccountAdj" text='<%# DataBinder.Eval(Container.DataItem, "AccAdjust") %>'>
									</asp:Label>
								</Itemtemplate>	
								<EditItemTemplate>
									<asp:TextBox Runat="server" ID="AccountAdjEdit" MaxLength="12" Width="75%" CssClass="TextBox" 
									Text='<%# DataBinder.Eval(Container.DataItem, "AccAdjust") %>'
									ontextchanged="AccountAdjEdit_TextChanged" AutoPostBack="true"/>
									<cc1:TextBoxWatermarkExtender ID="AccountAdjEdit_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="AccountAdjEdit" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                    </cc1:TextBoxWatermarkExtender>					
                                    <asp:Button class="btngo" runat="server" ID="btnAccAdjEdit" CommandName ="btnAccAdjEdit"  Text="..." />                        
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="AccountAdjAdd" CssClass="TextBox" MaxLength="12" Width="75%" 
									ontextchanged="AccountAdjAdd_TextChanged" AutoPostBack="true" Runat="Server" />
									<cc1:TextBoxWatermarkExtender ID="AccountAdjAdd_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="AccountAdjAdd" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                    </cc1:TextBoxWatermarkExtender>									
									<asp:Button class="btngo" runat="server" ID="btnAccAdjAdd" CommandName ="btnAccAdjAdd" Text="..." />
								</FooterTemplate>															                                
							</asp:TemplateField>	
							
							<asp:TemplateField HeaderText="Account Adjustment Name" HeaderStyle-Width="200px" SortExpression="AccAdjName">
								<Itemtemplate>
									<asp:Label Runat="server" ID="AccountAdjName" text='<%# DataBinder.Eval(Container.DataItem, "AccAdjName") %>'>
									</asp:Label>
								</Itemtemplate>	
								<EditItemTemplate>
									<asp:Label Runat="server" ID="AccountAdjNameEdit" Text='<%# DataBinder.Eval(Container.DataItem, "AccAdjName") %>'>
									</asp:Label>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:Label Runat="server" ID="AccountAdjNameAdd">
									</asp:Label>
								</FooterTemplate>							
							</asp:TemplateField>																										
							
							<asp:TemplateField HeaderText="Action" headerstyle-width="150px" >
								<ItemTemplate>		
								    <asp:Button class="bitbtn btnedit" runat="server" ID="btnEdit" Text="Edit" CommandName="Edit" />																						 											
									
									<asp:Button class="bitbtn btndelete" runat="server" ID="btnDelete" Text="Delete" CommandName="Delete"  OnClientClick="return confirm('Sure to delete this data?');" />																						 																		
									
								</ItemTemplate>
								<EditItemTemplate>
									<%--<asp:Button CommandName="Update" Text="Update" CssClass="Button" ID="btnUpdate" Runat="server" Width="60" />--%>
									<asp:Button class="bitbtn btnsave" runat="server" ID="btnUpdate" Text="Save" CommandName="Update" />																						 																		
									
									<%--<asp:Button CommandName="Cancel" Text="Cancel" CssClass="Button" ID="btnCancel" Runat="server" Width="50" />--%>
									<asp:Button class="bitbtn btncancel" runat="server" ID="btnCancel" Text="Cancel" CommandName="Cancel" />																						 																		
									
								</EditItemTemplate>
								<FooterTemplate>
									<%--<asp:Button CommandName="Insert" Text="Add" CssClass="Button" ID="btnAdd" Runat="server" Width="95" />--%>
									<asp:Button class="bitbtn btnadd" runat="server" ID="btnAdd" Text="Add" CommandName="Insert" />																						 																		
									
								</FooterTemplate>


							</asp:TemplateField>
							
    					</Columns>
        </asp:GridView>   
        </div >
         <%--</ContentTemplate>
        </asp:UpdatePanel>--%>             
       <asp:Label ID="lstatus" ForeColor="red" runat="server"></asp:Label>     
    </div>
    </form>
</body>
</html>
