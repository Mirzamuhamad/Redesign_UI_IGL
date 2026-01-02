<%@ Page Language="VB" AutoEventWireup="false" CodeFile="MsCashFlow.aspx.vb" Inherits="MsCashFlow_MsCashFlow" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<%@ Register Assembly="BasicFrame.WebControls.BasicDatePicker" Namespace="BasicFrame.WebControls"
    TagPrefix="BDP" %>
    
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Cash Flow</title>
    <script src="../../Function/OpenDlg.js" type="text/javascript"></script>
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
 <link href="https://fonts.googleapis.com/css2?family=Muli:wght@300;400;600&display=swap" rel="stylesheet">
</head>
<body>
  <form id="form1" runat="server">
      <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
     <div class="Content">
     <div class="H1">Cash Flow</div>
     <hr style="color:Blue" />
     <asp:Panel id="pnlHd" runat="server">
          <table>
            <tr>
                <td style="width:100px;text-align:right" >Quick Search :</td>
                <td><asp:TextBox runat="server" ID ="tbFilter" CssClass="TextBox"/> 
                    <asp:DropDownList runat="server" ID="ddlField" CssClass="DropDownList" >
                      <asp:ListItem Selected="true" Text="Code" Value="CashFlowCode"></asp:ListItem>
                      <asp:ListItem Text="Name" Value="CashFlowName"></asp:ListItem> 
                      <asp:ListItem Text="Flow Type" Value="CashFlowType"></asp:ListItem> 
                      <asp:ListItem Text="Account" Value="Account"></asp:ListItem>
                      <asp:ListItem Text="AccountName" Value="AccountName"></asp:ListItem>       
                      <asp:ListItem Text="Days" Value="Days"></asp:ListItem>
                      <asp:ListItem Text="Currency" Value="Currency"></asp:ListItem>
                      <asp:ListItem Text="Nominal" Value="Nominal"></asp:ListItem>
                      <asp:ListItem Text="StartYear" Value="StartYear"></asp:ListItem>
                      <asp:ListItem Text="StartMonth" Value="StartMonth"></asp:ListItem>
                      <asp:ListItem Text="EndYear" Value="EndYear"></asp:ListItem>
                      <asp:ListItem Text="EndMonth" Value="EndMonth"></asp:ListItem>
                      
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
                      <asp:ListItem Selected="true" Text="Code" Value="CashFlowCode"></asp:ListItem>
                      <asp:ListItem Text="Name" Value="CashFlowName"></asp:ListItem>        
                      <asp:ListItem Text="Flow Type" Value="CashFlowType"></asp:ListItem>        
                      <asp:ListItem Text="Account" Value="Account"></asp:ListItem>        
                      <asp:ListItem Text="Days" Value="Days"></asp:ListItem>        
                      <asp:ListItem Text="Currency" Value="Currency"></asp:ListItem>        
                      <asp:ListItem Text="Nominal" Value="Nominal"></asp:ListItem>        
                      <asp:ListItem Text="Start Year" Value="StartYear"></asp:ListItem>        
                      <asp:ListItem Text="Start Month" Value="StartMonth"></asp:ListItem>        
                      <asp:ListItem Text="End Year" Value="EndYear"></asp:ListItem>        
                      <asp:ListItem Text="End Month" Value="EndMonth"></asp:ListItem>        
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
							    <asp:TemplateField HeaderText="Code" HeaderStyle-Width="100px" SortExpression="CashFlowCode">
								    <Itemtemplate>
									    <asp:Label Runat="server" ID="CashFlowCode" text='<%# DataBinder.Eval(Container.DataItem, "CashFlowCode") %>'>
									    </asp:Label>
								    </Itemtemplate>
								    <EditItemTemplate>
									    <asp:Label Runat="server" ID="CashFlowCodeEdit" Text='<%# DataBinder.Eval(Container.DataItem, "CashFlowCode") %>'>
									    </asp:Label>
								    </EditItemTemplate>
								    <FooterTemplate>
									    <asp:TextBox ID="CashFlowCodeAdd" CssClass="TextBox" MaxLength="10" Runat="Server" Width="100%"/>
									    <cc1:TextBoxWatermarkExtender ID="CashFlowCodeAdd_WtExt" 
                                            runat="server" Enabled="True" TargetControlID="CashFlowCodeAdd" 
                                            WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                         </cc1:TextBoxWatermarkExtender>
								    </FooterTemplate>								
							    </asp:TemplateField>							
    							
							    <asp:TemplateField HeaderText="Name" HeaderStyle-Width="100px" SortExpression="CashFlowName">
								    <Itemtemplate>
									    <asp:Label Runat="server" ID="CashFlowName" text='<%# DataBinder.Eval(Container.DataItem, "CashFlowName") %>'>
									    </asp:Label>
								    </Itemtemplate>
								    <EditItemTemplate>
									    <asp:TextBox Runat="server" ID="CashFlowNameEdit" MaxLength="60" Width="100%" CssClass="TextBox" Text='<%# DataBinder.Eval(Container.DataItem, "CashFlowName") %>'>
									    </asp:TextBox>
									    <cc1:TextBoxWatermarkExtender ID="CashFlowNameEdit_WtExt" 
                                            runat="server" Enabled="True" TargetControlID="CashFlowNameEdit" 
                                            WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                         </cc1:TextBoxWatermarkExtender>									
								    </EditItemTemplate>
								    <FooterTemplate>
									    <asp:TextBox ID="CashFlowNameAdd" CssClass="TextBox" MaxLength="60" Runat="Server" Width="100%"/>
									    <cc1:TextBoxWatermarkExtender ID="CashFlowNameAdd_WtExt" 
                                            runat="server" Enabled="True" TargetControlID="CashFlowNameAdd" 
                                            WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                         </cc1:TextBoxWatermarkExtender>
								    </FooterTemplate>
							    </asp:TemplateField>
    							
							    <asp:TemplateField HeaderText="Flow Type" HeaderStyle-Width="100px" SortExpression="CashFlowTypeName">
								    <Itemtemplate>
									    <asp:Label Runat="server" ID="CashFlowType" text='<%# DataBinder.Eval(Container.DataItem, "CashFlowTypeName") %>'>
									    </asp:Label>
								    </Itemtemplate>
								    <EditItemTemplate>
									    <asp:DropDownList Runat="server" ID="CashFlowTypeEdit" Width="100%" CssClass="DropDownList"
                                            SelectedValue='<%# DataBinder.Eval(Container.DataItem, "CashFlowType") %>'
                                            DataSourceID="dsCashFlowType" DataTextField="CashFlowTypeName" 
                                            DataValueField="CashFlowTypeCode">
									    </asp:DropDownList>													
								    </EditItemTemplate>
								    <FooterTemplate>
									    <asp:DropDownList ID="CashFlowTypeAdd" Runat="Server" Width="100%" CssClass="DropDownList"
                                            DataSourceID="dsCashFlowType" DataTextField="CashFlowTypeName" 
                                            DataValueField="CashFlowTypeCode">
									    </asp:DropDownList>									
								    </FooterTemplate>
							    </asp:TemplateField>
    							
    							
							    <asp:TemplateField HeaderText="Account" HeaderStyle-Width="100" SortExpression="Account">
								    <Itemtemplate>
									    <asp:Label Runat="server" ID="Account" text='<%# DataBinder.Eval(Container.DataItem, "Account") %>'>
									    </asp:Label>
								    </Itemtemplate>																	
								    <EditItemTemplate>
								        <asp:TextBox CssClass="TextBox" runat="server" ID="AccountEdit" Width="50" 
                                            text='<%# DataBinder.Eval(Container.DataItem, "Account") %>' 
                                             ontextchanged="AccountEdit_TextChanged" AutoPostBack="true"/>
                                        <asp:Button class="btngo" runat="server" ID="btnAccEdit" Text="..." CommandName="SearchEdit" />                              
								    </EditItemTemplate>						
								    <FooterTemplate>
								        <asp:TextBox CssClass="TextBox" runat="server" id="AccountAdd" Width="50" 
                                            ontextchanged="AccountAdd_TextChanged" AutoPostBack="true" />
                                        <asp:Button class="btngo" runat="server" ID="btnAccAdd" Text="..." CommandName="SearchAdd" />                                                        
								    </FooterTemplate>                                
							    </asp:TemplateField>
    							
							    <asp:TemplateField HeaderText="Account Name" HeaderStyle-Width="100px" SortExpression="AccountName">
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
    							
							    <asp:TemplateField HeaderText="Days" HeaderStyle-Width="20px" SortExpression="Days">
								    <Itemtemplate>
									    <asp:Label Runat="server" ID="Days" Width="50px" text='<%# DataBinder.Eval(Container.DataItem, "Days") %>'>
									    </asp:Label>
								    </Itemtemplate>
								    <EditItemTemplate>
									    <asp:TextBox Runat="server" ID="DaysEdit" Width="100%" ontextchanged="DaysEdit_TextChanged" AutoPostBack="true" Text='<%# DataBinder.Eval(Container.DataItem, "Days") %>'>
									    </asp:TextBox>
								    </EditItemTemplate>
								    <FooterTemplate>
									    <asp:TextBox ID="DaysAdd" CssClass="TextBox" MaxLength="2" Runat="Server" Width="100%"  ontextchanged="DaysAdd_TextChanged" AutoPostBack="true"/>
									    <cc1:TextBoxWatermarkExtender ID="DaysAdd_WtExt" 
                                            runat="server" Enabled="True" TargetControlID="DaysAdd" 
                                            WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                         </cc1:TextBoxWatermarkExtender>
								    </FooterTemplate>								
							    </asp:TemplateField>
    							
							    <asp:TemplateField HeaderText="Currency" HeaderStyle-Width="20px" SortExpression="Currency">
								    <Itemtemplate>
									    <asp:Label Runat="server" ID="Currency" text='<%# DataBinder.Eval(Container.DataItem, "Currency") %>'>
									    </asp:Label>
								    </Itemtemplate>
								    <EditItemTemplate>
									    <asp:DropDownList Runat="server" ID="CurrencyEdit" Width="100%" CssClass="DropDownList"
                                            SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Currency") %>'
                                            DataSourceID="dsCurrency" DataTextField="Currency_Name" 
                                            DataValueField="Currency">
									    </asp:DropDownList>													
								    </EditItemTemplate>
								    <FooterTemplate>
									    <asp:DropDownList ID="CurrencyAdd" Runat="Server" Width="100%" CssClass="DropDownList"
                                            DataSourceID="dsCurrency" DataTextField="Currency_Name" 
                                            DataValueField="Currency">
									    </asp:DropDownList>									
								    </FooterTemplate>
							    </asp:TemplateField>
    							
							    <asp:TemplateField HeaderText="Nominal" HeaderStyle-Width="50px" SortExpression="Nominal" ItemStyle-HorizontalAlign = "Right">
								    <Itemtemplate>
									    <asp:Label Runat="server" ID="Nominal" DataFormatString="{0:#,##0.00}" text='<%# DataBinder.Eval(Container.DataItem, "Nominal") %>'>
									    </asp:Label>
								    </Itemtemplate>
								    <EditItemTemplate>
									    <asp:TextBox Runat="server" ID="NominalEdit" Width="100%" Text='<%# DataBinder.Eval(Container.DataItem, "Nominal") %>'>
									    </asp:TextBox>
								    </EditItemTemplate>
								    <FooterTemplate>
									    <asp:TextBox ID="NominalAdd" CssClass="TextBox" MaxLength="8" Runat="Server" Width="100%"/>
									    <cc1:TextBoxWatermarkExtender ID="NominalAdd_WtExt" 
                                            runat="server" Enabled="True" TargetControlID="NominalAdd" 
                                            WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                         </cc1:TextBoxWatermarkExtender>
								    </FooterTemplate>								
							    </asp:TemplateField>
    							
							    <asp:TemplateField HeaderText="StartYear" HeaderStyle-Width="50px" SortExpression="StartYear">
								    <Itemtemplate>
									    <asp:Label Runat="server" ID="StartYear" Width="50px" text='<%# DataBinder.Eval(Container.DataItem, "StartYear") %>'>
									    </asp:Label>
								    </Itemtemplate>
								    <EditItemTemplate>
									    <asp:TextBox Runat="server" ID="StartYearEdit" MaxLength="4" Width="100%" ontextchanged="StartYearAdd_TextChanged" AutoPostBack="true" Text='<%# DataBinder.Eval(Container.DataItem, "StartYear") %>'>
									    </asp:TextBox>
								    </EditItemTemplate>
								    <FooterTemplate>
									    <asp:TextBox ID="StartYearAdd" CssClass="TextBox" MaxLength="4" Runat="Server" Width="100%" ontextchanged="StartYearAdd_TextChanged" AutoPostBack="true"/>
									    <cc1:TextBoxWatermarkExtender ID="StartYearAdd_WtExt" 
                                            runat="server" Enabled="True" TargetControlID="StartYearAdd" 
                                            WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                         </cc1:TextBoxWatermarkExtender>
								    </FooterTemplate>								
							    </asp:TemplateField>
    							
							    <asp:TemplateField HeaderText="StartMonth" HeaderStyle-Width="80" SortExpression="StartMonth">
								    <Itemtemplate>
									    <asp:Label Runat="server" ID="StartMonth" text='<%# DataBinder.Eval(Container.DataItem, "StartMonth") %>'>
									    </asp:Label>
								    </Itemtemplate>
								    <EditItemTemplate>
									    <asp:DropDownList Runat="server" ID="StartMonthEdit" Width="100%" CssClass="DropDownList"
                                            SelectedValue='<%# DataBinder.Eval(Container.DataItem, "StartMonth") %>'
                                            DataSourceID="dsStartMonth" DataTextField="Description" 
                                            DataValueField="Period">
									    </asp:DropDownList>													
								    </EditItemTemplate>
								    <FooterTemplate>
									    <asp:DropDownList ID="StartMonthAdd" Runat="Server" Width="100%" CssClass="DropDownList"
                                            DataSourceID="dsStartMonth" DataTextField="Description" 
                                            DataValueField="Period">
									    </asp:DropDownList>									
								    </FooterTemplate>
							    </asp:TemplateField>
    							
							    <asp:TemplateField HeaderText="EndYear" HeaderStyle-Width="80px" SortExpression="EndYear">
								    <Itemtemplate>
									    <asp:Label Runat="server" ID="EndYear" text='<%# DataBinder.Eval(Container.DataItem, "EndYear") %>'>
									    </asp:Label>
								    </Itemtemplate>
								    <EditItemTemplate>
									    <asp:TextBox Runat="server" ID="EndYearEdit" MaxLength="4" Width="100%" Text='<%# DataBinder.Eval(Container.DataItem, "EndYear") %>'>
									    </asp:TextBox>
								    </EditItemTemplate>
								    <FooterTemplate>
									    <asp:TextBox ID="EndYearAdd" CssClass="TextBox" MaxLength="4" Runat="Server" Width="100%"/>
									    <cc1:TextBoxWatermarkExtender ID="EndYearAdd_WtExt" 
                                            runat="server" Enabled="True" TargetControlID="EndYearAdd" 
                                            WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                         </cc1:TextBoxWatermarkExtender>
								    </FooterTemplate>								
							    </asp:TemplateField>
    							
							    <asp:TemplateField HeaderText="EndMonth" HeaderStyle-Width="80" SortExpression="EndMonth">
								    <Itemtemplate>
									    <asp:Label Runat="server" ID="EndMonth" text='<%# DataBinder.Eval(Container.DataItem, "EndMonth") %>'>
									    </asp:Label>
								    </Itemtemplate>
								    <EditItemTemplate>
									    <asp:DropDownList Runat="server" ID="EndMonthEdit" Width="100%" CssClass="DropDownList"
                                            SelectedValue='<%# DataBinder.Eval(Container.DataItem, "EndMonth") %>'
                                            DataSourceID="dsstartMonth" DataTextField="Description" 
                                            DataValueField="Period">
									    </asp:DropDownList>													
								    </EditItemTemplate>
								    <FooterTemplate>
									    <asp:DropDownList ID="EndMonthAdd" Runat="Server" Width="100%" CssClass="DropDownList"
                                            DataSourceID="dsstartMonth" DataTextField="Description" 
                                            DataValueField="Period">
									    </asp:DropDownList>									
								    </FooterTemplate>
							    </asp:TemplateField>
    							
							    <asp:TemplateField HeaderText="Action" headerstyle-width="126px" >
								    <ItemTemplate>
								        <asp:Button class="bitbtn btnedit" width="40px" runat="server" ID="btnEdit" Text="Edit" CommandName="Edit" />																					 											
									    <asp:Button class="bitbtn btndelete" width="50px" runat="server" ID="btnDelete" Text="Delete" CommandName="Delete"  OnClientClick="return confirm('Sure to delete this data?');" />																						 																		
								        <asp:Button class="bitbtn btndetail" width="50px" runat="server" ID="btnDetail" Text="Detail" CommandName="Detail" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" /> 
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
        </asp:Panel>
        <asp:Panel ID="pnlDt" runat="server" Visible = "false">  
         <asp:Label ID="label1" CssClass="H1" runat="server" Text="Cash Flow : " />   
         <asp:Label ID="lbCashFlow" ForeColor="Blue" CssClass="H1" Font-Bold="true" runat="server" Alighment="right"/>
         <asp:Label ID="Label2" ForeColor="Blue" CssClass="H1" Font-Bold="true" runat="server" Text="-"/>
         <asp:Label ID="lbCashFlowName" ForeColor="Blue" CssClass="H1" Font-Bold="true" runat="server" />
         <asp:Label ID="lbStartYear" ForeColor="Blue" CssClass="H1" Font-Bold="true" runat="server" visible=false/>
         <asp:Label ID="lbEndYear" ForeColor="Blue" CssClass="H1" Font-Bold="true" runat="server" visible=false/>
         <asp:Label ID="lbStartMonth" ForeColor="Blue" CssClass="H1" Font-Bold="true" runat="server" visible=false/>
         <asp:Label ID="lbEndMonth" ForeColor="Blue" CssClass="H1" Font-Bold="true" runat="server" visible=false/>
         <asp:Label ID="lbAmount" ForeColor="Blue" CssClass="H1" Font-Bold="true" runat="server" visible=false/>
	     <br />
	    <asp:Button class="bitbtn btnback" runat="server" ID="btnBackDtTop" Text="Back" />
	    <asp:Button class="bitbtn btngenerate" runat="server" ID="btnGenerate" 
                Text="Generate Data" Width="94px" />
	    <br />    	 
	        <div style="border:0px  solid; width:100%; height:100%; overflow:auto;">
            <asp:GridView id="DataGridDt" runat="server" ShowFooter="True" AllowSorting="True" 
                AutoGenerateColumns="False" AllowPaging="True" Visible="true">                
						    <HeaderStyle CssClass="GridHeader" wrap="false"></HeaderStyle>
						    <RowStyle CssClass="GridItem" wrap="false"/>
						    <AlternatingRowStyle CssClass="GridAltItem"/>
						    <FooterStyle CssClass="GridFooter" Wrap="false" />
						    <PagerStyle CssClass="GridPager" />
				          <Columns>			            
    						
							    <asp:TemplateField HeaderText="Year" HeaderStyle-Width="80" SortExpression="Year">
								    <Itemtemplate>
									    <asp:Label Runat="server" Width="80" ID="Year" text='<%# DataBinder.Eval(Container.DataItem, "Year") %>'>
									    </asp:Label>
								    </Itemtemplate>			
								    <EditItemTemplate>
								        <asp:DropDownList ID="YearDtEdit" CssClass="DropDownList" runat="server" DataSourceID="dsYear" Width="100%" 
                                            DataTextField="Year" DataValueField="Year" AutoPostBack="true" SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Year") %>' />                                        
                                            
                                        <asp:Label Runat="server" Width="80" ID="YearTemp" text='<%# DataBinder.Eval(Container.DataItem, "Year") %>' Visible="false">
									    </asp:Label>    
								    </EditItemTemplate>	
								    <FooterTemplate>								    
								        <asp:DropDownList ID="YearDtAdd" CssClass="DropDownList" runat="server" DataSourceID="dsYear" Width="100%" 
                                            DataTextField="Year" DataValueField="Year" AutoPostBack="true" >                                        
                                        </asp:DropDownList>								    
								    </FooterTemplate>											
							    </asp:TemplateField>
    							
							    <asp:TemplateField HeaderText="Month" HeaderStyle-Width="128" SortExpression="Month">
								    <Itemtemplate>
									    <asp:Label Runat="server" Width="128" ID="Month" TEXT='<%# DataBinder.Eval(Container.DataItem, "Month") %>'>
									    </asp:Label>
								    </Itemtemplate>	
								    <EditItemTemplate>
									    <asp:DropDownList Runat="server" ID="MonthEdit" Width="100%" CssClass="DropDownList"
                                            SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Month") %>'
                                            DataSourceID="dsStartMonth" DataTextField="Description" 
                                            DataValueField="Period">
									    </asp:DropDownList>
									    
									    <asp:Label Runat="server" Width="128" ID="MonthTemp" TEXT='<%# DataBinder.Eval(Container.DataItem, "Month") %>' Visible="false">
									    </asp:Label>													
								    </EditItemTemplate>
								    <FooterTemplate>
									    <asp:DropDownList ID="MonthAdd" Runat="Server" Width="100%" CssClass="DropDownList"
                                            DataSourceID="dsStartMonth" DataTextField="Description" 
                                            DataValueField="Period">
									    </asp:DropDownList>									
								    </FooterTemplate>
							    </asp:TemplateField>
    							    							
							    <asp:TemplateField HeaderText="Amount" HeaderStyle-Width="70" SortExpression="Amount" ItemStyle-HorizontalAlign = "Right">
								    <Itemtemplate>
									    <asp:Label Runat="server" Width="70" ID="Amount" DataFormatString="{0:#,##0.00}" TEXT='<%# DataBinder.Eval(Container.DataItem, "Amount") %>'>
									    </asp:Label>
								    </Itemtemplate>
								    <EditItemTemplate>
									    <asp:TextBox Runat="server" ID="AmountEdit" CssClass="TextBox" Width="90%" Text='<%# DataBinder.Eval(Container.DataItem, "Amount") %>'>
									    </asp:TextBox>
								    </EditItemTemplate>
								    <FooterTemplate>
									    <asp:TextBox ID="AmountAdd" CssClass="TextBox" Width="90%" Runat="Server"/>
								        <cc1:TextBoxWatermarkExtender ID="AmountAdd_TextBoxWatermarkExtender" 
                                            runat="server" Enabled="True" TargetControlID="AmountAdd" WatermarkText="can't blank" WatermarkCssClass="Watermarked" >
                                        </cc1:TextBoxWatermarkExtender>
								    </FooterTemplate>
							    </asp:TemplateField>    														       						       				  																	
    																																						
							    <asp:TemplateField HeaderText="Action" HeaderStyle-Width="126px" ItemStyle-Wrap="false">
								    <ItemTemplate>
								        <asp:Button ID="btnEdit" runat="server" class="bitbtndt btnedit" Text="Edit" CommandName="Edit"/>
							            <asp:Button ID="btnDelete" runat="server" class="bitbtndt btndelete" Text="Delete" OnClientClick="return confirm('Sure to delete this data?');" CommandName="Delete"/>									
                        	        </ItemTemplate>																
								    <EditItemTemplate>
								        <asp:Button ID="btnUpdate" runat="server" class="bitbtndt btnsave" Text="Save" CommandName="Update"/>									
                                        <asp:Button ID="btnCancel" runat="server" class="bitbtndt btncancel" Text="Cancel" CommandName="Cancel"/>								
								    </EditItemTemplate>
								    <FooterTemplate>
									    <asp:Button class="bitbtndt btnadd" runat="server" ID="btnAdd" Text="Add" CommandName="Insert"/>
								    </FooterTemplate>
							    </asp:TemplateField>							
    					    </Columns>
            </asp:GridView>  
            </div>        
	        <asp:Button class="bitbtn btnback" runat="server" ID="Button2" Text="Back" />
        </asp:Panel>
        <asp:SqlDataSource ID="dsCashFlowType" runat="server"                                                                                 
                                        SelectCommand="EXEC S_GetCashFlowType">                                        
        </asp:SqlDataSource>
        <asp:SqlDataSource ID="dsCurrency" runat="server"                                                                                 
                                        SelectCommand="EXEC S_GetCurrency">                                        
                                        </asp:SqlDataSource>
        <asp:SqlDataSource ID="dsstartMonth" runat="server"                                                                                 
                                        SelectCommand="EXEC S_GetPeriod">                                        
        </asp:SqlDataSource> 
        <asp:SqlDataSource ID="dsYear" runat="server"                                                                                 
                                        SelectCommand="EXEC S_GetYear">                                        
        </asp:SqlDataSource>                               
     <asp:Label ID="lstatus" ForeConstructionIP="red" runat="server"></asp:Label>    
    </div>
    </form>
</body>

</html>
