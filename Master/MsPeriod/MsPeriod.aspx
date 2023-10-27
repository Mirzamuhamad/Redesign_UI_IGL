<%@ Page Language="VB" AutoEventWireup="false" CodeFile="MsPeriod.aspx.vb" Inherits="MsPeriod_MsPeriod" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<%@ Register Assembly="BasicFrame.WebControls.BasicDatePicker" Namespace="BasicFrame.WebControls"
    TagPrefix="BDP" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Period File</title>
    <script src="../../Function/OpenDlg.js" type="text/javascript"></script>
    <script src="../../Function/Function.JS" type="text/javascript"></script>
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
</head>
<body>
  <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>    
     <div class="Content">
     <div class="H1">Period File</div>
     <hr style="color:Blue" />   
     
             
      <table>
        <tr>
            <td style="width:100px;text-align:right" >Quick Search :</td>      
            
            <td><asp:DropDownList ID="ddlYear" Runat="Server"  CssClass="DropDownList"									    
                    OnSelectedIndexChanged =  "ddlYear_SelectedIndexChanged" AutoPostBack ="true"  >
                  </asp:DropDownList>  
                 <asp:TextBox runat="server" ID ="tbFilter" CssClass="TextBox"/> 
                <asp:DropDownList runat="server" ID="ddlField" CssClass="DropDownList" >
                  <asp:ListItem Selected="true" Text="Start Date" Value="Start_Date"></asp:ListItem>
                  <asp:ListItem Text="End Date" Value="End_Name"></asp:ListItem>        
                  <asp:ListItem Text="Closing" Value="FgClosing"></asp:ListItem>        
                </asp:DropDownList>                   
            
                    
                <asp:Button class="bitbtn btnsearch" runat="server" ID="btnSearch" Text="Search" />                
                <asp:Button class="btngo" runat="server" ID="btnExpand" Text="..."/>                
                <asp:Button class="bitbtn btnprint" runat="server" ID="btnPrint" Text="Print"/>               
                <asp:Button ID="btnShowPopup" runat="server" Style="display: none" />
                &nbsp &nbsp &nbsp                   
			    
                <asp:Button class="bitbtndt btnGo" runat="server" ID="BtnGenerateTop" Text="Generate" width = "80" />									                                                                                                                                                                                
                            
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
                  <asp:ListItem Selected="true" Text="Start Date" Value="Start_Date"></asp:ListItem>
                  <asp:ListItem Text="End Date" Value="End_Name"></asp:ListItem>        
                  <asp:ListItem Text="Closing" Value="FgClosing"></asp:ListItem>        
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
				            <%--<asp:BoundField DataField="PeriodCode" HeaderStyle-Width="120px" SortExpression="PeriodCode" Visible="false"></asp:BoundField>                  		      --%>
				            <asp:TemplateField HeaderText="Period" HeaderStyle-Width="68" SortExpression="PeriodCode">
								<Itemtemplate>
									<asp:Label Runat="server" ID="PeriodCode" text='<%# DataBinder.Eval(Container.DataItem, "PeriodCode") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:TextBox Runat="server" Enabled="false" ID="PeriodCodeEdit" MaxLength="20" Width="100%" CssClass="TextBox" Text='<%# DataBinder.Eval(Container.DataItem, "PeriodCode") %>'>
									</asp:TextBox>
									<cc1:TextBoxWatermarkExtender ID="PeriodNameEdit_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="PeriodCodeEdit" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>									
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="PeriodCodeAdd" CssClass="TextBox" Enabled="false" MaxLength="20" Runat="Server" Width="100%"/>
									<cc1:TextBoxWatermarkExtender ID="PeriodNameAdd_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="PeriodCodeAdd" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>
								</FooterTemplate>
							</asp:TemplateField>	
							
							<asp:TemplateField HeaderText="Start Date" HeaderStyle-Width="100" SortExpression="StartDate">
								<Itemtemplate>
									<asp:Label Runat="server" ID="StartDate" text='<%# DataBinder.Eval(Container.DataItem, "Start_Date") %>'>
									</asp:Label>									
								</Itemtemplate>
								<EditItemTemplate>
									<BDP:BasicDatePicker ID="StartDateEdit" runat="server" DateFormat="dd/MMM/yyyy" 
                                      ReadOnly = "true" ValidationGroup="Input"
                                      ButtonImageHeight="19px" ButtonImageWidth="20px" 
                                      DisplayType="TextBoxAndImage" 
                                      TextBoxStyle-CssClass="TextDate" AutoPostBack="True" 
                                      ShowNoneButton="False" SelectedValue = '<%# DataBinder.Eval(Container.DataItem, "StartDate") %>' >
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
							<asp:TemplateField HeaderText="End Date" HeaderStyle-Width="100" SortExpression="EndDate">
								<Itemtemplate>
									<asp:Label Runat="server" ID="EndDate" text='<%# DataBinder.Eval(Container.DataItem, "End_Date") %>'>
									</asp:Label>									
								</Itemtemplate>
								<EditItemTemplate>
									<BDP:BasicDatePicker ID="EndDateEdit" runat="server" DateFormat="dd/MMM/yyyy" 
                                      ReadOnly = "true" ValidationGroup="Input"
                                      ButtonImageHeight="19px" ButtonImageWidth="20px" 
                                      DisplayType="TextBoxAndImage" 
                                      TextBoxStyle-CssClass="TextDate" AutoPostBack="True" 
                                      ShowNoneButton="False" SelectedValue = '<%# DataBinder.Eval(Container.DataItem, "EndDate") %>' >
                                     <TextBoxStyle CssClass="TextDate" />
                                    </BDP:BasicDatePicker>                                                
								</EditItemTemplate>
								<FooterTemplate>
									<BDP:BasicDatePicker ID="EndDateAdd" runat="server" DateFormat="dd/MMM/yyyy" 
                                      ReadOnly = "true" ValidationGroup="Input"
                                      ButtonImageHeight="19px" ButtonImageWidth="20px" 
                                      DisplayType="TextBoxAndImage" 
                                      TextBoxStyle-CssClass="TextDate" AutoPostBack="True" 
                                      ShowNoneButton="False" >
                                     <TextBoxStyle CssClass="TextDate" />
                                    </BDP:BasicDatePicker>   
								</FooterTemplate>								
							</asp:TemplateField>	
							<asp:TemplateField HeaderText="Closing" HeaderStyle-Width="80" SortExpression="FgClosing">
								<Itemtemplate>
									<asp:Label Runat="server" Width="30" ID="FgClosing" text='<%# DataBinder.Eval(Container.DataItem, "FgClosing") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
								    <asp:Label Runat="server" Width="100%" ID="FgClosingEdit" text='<%# DataBinder.Eval(Container.DataItem, "FgClosing") %>'>
									</asp:Label>									
								</EditItemTemplate>
								<FooterTemplate>
									<asp:DropDownList ID="FgClosingAdd" Enabled="false" CssClass="DropDownList" Width="100%" runat="server">
									  <asp:ListItem Selected="True">N</asp:ListItem>
									  <asp:ListItem>Y</asp:ListItem>                                        
									</asp:DropDownList>								    
								</FooterTemplate>
							    <HeaderStyle Width="80px" />
                                <ItemStyle HorizontalAlign="Left" />
							</asp:TemplateField>		
							
							<asp:TemplateField HeaderText="Action" headerstyle-width="126" >
								<ItemTemplate>
								
								  <asp:Button class="bitbtn btnedit" runat="server" ID="btnEdit" Text="Edit" CommandName="Edit" />																						 											
								  <asp:Button class="bitbtn btndelete" runat="server" ID="btnDelete" Text="Delete" CommandName="Delete"  OnClientClick="return confirm('Sure to delete this data?');" />																						 																		
								  <asp:Button class="bitbtn btnclose" runat="server" ID="btnClose" Text="Closing" CommandName="Closing" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" OnClientClick="return confirm('Sure to closing this data?');" />																						 																		
								
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
         
         <asp:Panel runat="server" ID="PnlGenerate" Visible="false">         
         <table>
         
                <tr>
                    <td>Year</td>
                    <td>:</td>
                    <td>                                  
                        <asp:DropDownList ID="ddlYearGenerate" runat="server" CssClass="DropDownList" AutoPostBack="true" /> 
                    </td>
                </tr>         
         
                <tr>
                    <td>Range Period</td>
                    <td>:</td>
                    <td>
                        <asp:TextBox ID="tbRange" runat="server" Text="1" MaxLength="3" Width="30px" AutoPostBack="true"></asp:TextBox>						                                  
                       	<asp:DropDownList ID="ddlRangeType" CssClass="DropDownList" Width="70px" runat="server" AutoPostBack="true">
						  <asp:ListItem Selected="True">Daily</asp:ListItem>
						  <asp:ListItem >Weekly</asp:ListItem>
						  <asp:ListItem >Monthly</asp:ListItem>						  
						</asp:DropDownList>							
                    </td>
                </tr>                
                <tr>
                    <td>Qty Period</td>
                    <td>:</td>
                    <td>
                        <asp:TextBox ID="tbQtyPeriod" runat="server" Text="1" MaxLength="3" Width="30px" AutoPostBack="true"></asp:TextBox>						                                                         	
                    </td>
                </tr> 
                <tr>
                    <td>Period</td>
                    <td>:</td>
                    <td>
                        <BDP:BasicDatePicker ID="tbStartGenerate" runat="server" DateFormat="dd/MMM/yyyy" 
                             ReadOnly = "false" ButtonImageHeight="19px" ButtonImageWidth="20px" 
                             DisplayType="TextBoxAndImage" 
                             TextBoxStyle-CssClass="TextDate" AutoPostBack="True" 
                             ShowNoneButton="False">
                             <TextBoxStyle CssClass="TextDate" />
                        </BDP:BasicDatePicker> 
                        <BDP:BasicDatePicker ID="tbEndGenerate" runat="server" DateFormat="dd/MMM/yyyy" 
                             ReadOnly = "false" ButtonImageHeight="19px" ButtonImageWidth="20px" 
                             DisplayType="TextBoxAndImage" 
                             TextBoxStyle-CssClass="TextDate" Enabled= "false"
                             ShowNoneButton="False">
                             <TextBoxStyle CssClass="TextDate" />
                        </BDP:BasicDatePicker> 
                    </td>    
                </tr>    
                <tr>
                    <td colspan="3" style="text-align: center">
                       <asp:Button class="btngo" runat="server" ID="btnGenerate" Text="Generate" width = "80" />									                                                                                                                                                                                
                       <asp:Button class="bitbtndt btncancel" runat="server" ID="btnCancel" Text="Cancel" width = "90" />									                                                                                                                                                                                
                        
                    </td>
                </tr>                 
         
         </table>
         </asp:Panel>      
     <asp:Label ID="lstatus" ForeColor="red" runat="server"></asp:Label>      
     
    </div>
    </form>
</body>

</html>
