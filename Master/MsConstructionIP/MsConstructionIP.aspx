<%@ Page Language="VB" AutoEventWireup="false" CodeFile="MsConstructionIP.aspx.vb" Inherits="MsConstructionIP_MsConstructionIP" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<%@ Register Assembly="BasicFrame.WebControls.BasicDatePicker" Namespace="BasicFrame.WebControls"
    TagPrefix="BDP" %>
    
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Country File</title>
    <script src="../../Function/OpenDlg.js" type="text/javascript"></script>
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
 <link href="https://fonts.googleapis.com/css2?family=Muli:wght@300;400;600&display=swap" rel="stylesheet">
</head>
<body>
  <form id="form1" runat="server">
      <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
     <div class="Content">
     <div class="H1">Construction In Progress</div>
     <hr style="ConstructionIP:Blue" />
      <table>
        <tr>
            <td style="width:100px;text-align:right" >Quick Search :</td>
            <td><asp:TextBox runat="server" ID ="tbFilter" CssClass="TextBox"/> 
                <asp:DropDownList runat="server" ID="ddlField" CssClass="DropDownList" >
                  <asp:ListItem Selected="true" Text="CIP Code" Value="CIPCode"></asp:ListItem>
                  <asp:ListItem Text="CIP Name" Value="CIPName"></asp:ListItem> 
                  <asp:ListItem Text="Startdate" Value="Startdate"></asp:ListItem>
                  <asp:ListItem Text="Enddate" Value="Enddate"></asp:ListItem>
                  <asp:ListItem Text="FgActive" Value="FgActive"></asp:ListItem>
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
                    <asp:ListItem Selected="true" Text="ConstructionIP Code" Value="CIPCode"></asp:ListItem>
                  <asp:ListItem Text="description" Value="CIPName"></asp:ListItem>        
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
							<asp:TemplateField HeaderText="CIP Code" HeaderStyle-Width="100px" SortExpression="CIPCode">
								<Itemtemplate>
									<asp:Label Runat="server" ID="CIPCode" text='<%# DataBinder.Eval(Container.DataItem, "CIPCode") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:Label Runat="server" ID="CIPCodeEdit" Text='<%# DataBinder.Eval(Container.DataItem, "CIPCode") %>'>
									</asp:Label>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="CIPCodeAdd" CssClass="TextBox" MaxLength="3" Runat="Server" Width="100%"/>
									<cc1:TextBoxWatermarkExtender ID="CIPCodeAdd_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="CIPCodeAdd" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>
								</FooterTemplate>								
							</asp:TemplateField>							
							
							<asp:TemplateField HeaderText="CIP Name" HeaderStyle-Width="250px" SortExpression="CIPName">
								<Itemtemplate>
									<asp:Label Runat="server" ID="CIPName" text='<%# DataBinder.Eval(Container.DataItem, "CIPName") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:TextBox Runat="server" ID="CIPNameEdit" MaxLength="60" Width="100%" CssClass="TextBox" Text='<%# DataBinder.Eval(Container.DataItem, "CIPName") %>'>
									</asp:TextBox>
									<cc1:TextBoxWatermarkExtender ID="CIPNameEdit_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="CIPNameEdit" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>									
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="CIPNameAdd" CssClass="TextBox" MaxLength="60" Runat="Server" Width="100%"/>
									<cc1:TextBoxWatermarkExtender ID="CIPNameAdd_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="CIPNameAdd" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>
								</FooterTemplate>
							</asp:TemplateField>
							<%------------%>
							<asp:TemplateField HeaderText="Start Date" HeaderStyle-Width="90px" SortExpression="Startdate">
							    <Itemtemplate>
									<asp:Label Runat="server" ID="StartDate" text='<%# DataBinder.Eval(Container.DataItem, "Start_Date") %>' Visible = "false">
									</asp:Label>
									<asp:Label Runat="server" ID="Start" text='<%# DataBinder.Eval(Container.DataItem, "Start_Date") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<BDP:BasicDatePicker ID="StartDateEdit" runat="server" DateFormat="dd/MMM/yyyy" 
                                      ReadOnly = "true" ValidationGroup="Input"
                                      ButtonImageHeight="19px" ButtonImageWidth="20px" 
                                      DisplayType="TextBoxAndImage" 
                                      TextBoxStyle-CssClass="TextDate" AutoPostBack = "True" 
                                      ShowNoneButton="False" SelectedValue = '<%# DataBinder.Eval(Container.DataItem, "Startdate") %>' >
                                    <TextBoxStyle CssClass="TextDate" />
                                    </BDP:BasicDatePicker>                
                               	</EditItemTemplate>
								<FooterTemplate>
									<BDP:BasicDatePicker ID="StartDateAdd" runat="server" DateFormat="dd/MMM/yyyy" 
                                      ReadOnly = "true" ValidationGroup="Input"
                                      ButtonImageHeight="19px" ButtonImageWidth="20px" 
                                      DisplayType="TextBoxAndImage" 
                                      TextBoxStyle-CssClass="TextDate" AutoPostBack="True" 
                                      ShowNoneButton="False" >
                                     <TextBoxStyle CssClass="TextDate" />
                                    </BDP:BasicDatePicker>   
								</FooterTemplate>
								</asp:TemplateField>		
							<%----------------------%>
							
							<asp:TemplateField HeaderText="End Date" HeaderStyle-Width="90px" SortExpression="Enddate" >
								<Itemtemplate>
									<asp:Label Runat="server" ID="Enddate" text='<%# DataBinder.Eval(Container.DataItem, "End_date") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
								<asp:Label Runat="server" ID="EnddateEdit" text='<%# DataBinder.Eval(Container.DataItem, "Enddate") %>'>
									</asp:Label>
								
									<%--<BDP:BasicDatePicker ID="EnddateEdit" runat="server" DateFormat="dd/MM/yyyy" 
                                      ReadOnly = "true" ValidationGroup="Input"
                                      ButtonImageHeight="19px" ButtonImageWidth="20px" 
                                      DisplayType="TextBoxAndImage" 
                                      TextBoxStyle-CssClass="TextDate" AutoPostBack = "True" 
                                      ShowNoneButton="False" Enable="False" SelectedValue = '<%# DataBinder.Eval(Container.DataItem, "Enddate") %>' >
                                    <TextBoxStyle CssClass="TextDate" />
                                    </BDP:BasicDatePicker> --%>
																		
								</EditItemTemplate>
								
								<FooterTemplate>
								<asp:Label Runat="server" ID="EnddateAdd" text='<%# DataBinder.Eval(Container.DataItem, "Enddate") %>'>
									</asp:Label>
								
									<%--<BDP:BasicDatePicker ID="EnddateAdd" runat="server" DateFormat="dd/MM/yyyy" 
                                      ReadOnly = "true" ValidationGroup="Input"
                                      ButtonImageHeight="19px" ButtonImageWidth="20px" 
                                      DisplayType="TextBoxAndImage" 
                                      TextBoxStyle-CssClass="TextDate" AutoPostBack="True" 
                                      ShowNoneButton="False" >
                                     <TextBoxStyle CssClass="TextDate" />
                                    </BDP:BasicDatePicker>--%>   
								</FooterTemplate>
								
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Active" HeaderStyle-Width="20px" SortExpression="FgActive">
								<Itemtemplate>
									<asp:Label Runat="server" ID="FgActive" text='<%# DataBinder.Eval(Container.DataItem, "FgActive") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
								<asp:Label Runat="server" ID="FgActiveEdit" text='<%# DataBinder.Eval(Container.DataItem, "FgActive") %>'>
									</asp:Label>
									<%--<asp:TextBox Runat="server" ID="FgActiveEdit" MaxLength="60" Width="100%" CssClass="TextBox" Text='<%# DataBinder.Eval(Container.DataItem, "FgActive") %>'>
									</asp:TextBox>
									<cc1:TextBoxWatermarkExtender ID="FgActiveEdit_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="FgActiveEdit" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>--%>									
								</EditItemTemplate>
								<FooterTemplate>
								<asp:Label Runat="server" ID="FgActiveAdd" text='<%# DataBinder.Eval(Container.DataItem, "FgActive") %>'>
									</asp:Label>
								
									<%--<asp:TextBox ID="FgActiveAdd" CssClass="TextBox" MaxLength="60" Runat="Server" Width="100%"/>
									<cc1:TextBoxWatermarkExtender ID="FgActiveAdd_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="FgActiveAdd" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>--%>
								</FooterTemplate>
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
								    <asp:Label Runat="server" ID="AccountNameAdd">
									</asp:Label>
								</FooterTemplate>														                                
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Action" headerstyle-width="126px" >
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
        
     <asp:Label ID="lstatus" ForeConstructionIP="red" runat="server"></asp:Label>    
    </div>
    </form>
</body>

</html>
