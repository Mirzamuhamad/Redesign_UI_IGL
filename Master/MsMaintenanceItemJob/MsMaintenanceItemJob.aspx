<%@ Page Language="VB" AutoEventWireup="false" CodeFile="MsMaintenanceItemJob.aspx.vb" Inherits="Master_MsMaintenanceItemJob_MsMaintenanceItemJob" %>
<%@ Register Assembly="DevExpress.Web.ASPxGridView.v10.2, Version=10.2.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxGridView" TagPrefix="dx" %>
<%@ Register assembly="DevExpress.Web.ASPxEditors.v10.2" namespace="DevExpress.Web.ASPxEditors" tagprefix="dx" %>
<%@ Register Assembly="BasicFrame.WebControls.BasicDatePicker" Namespace="BasicFrame.WebControls"
    TagPrefix="BDP" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Untitled Page</title>
    <script src="../../Function/Function.JS" type="text/javascript"></script>
    <script src="../../Function/OpenDlg.js" type="text/javascript"></script>
    <script type="text/javascript">
    function OpenPopup() {         
        window.open("../../SearchDlg.Aspx","List","scrollbars=yes,resizable=no,width=500,height=400");        
        return false;
    } 

</script>
<link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .style1
        {
            width: 126px;
        }
        .style2
        {
            width: 127px;
        }
        .style3
        {
            width: 6px;
        }
        .style4
        {
            width: 128px;
        }
        .style5
        {
            width: 138px;
        }
        .style6
        {
            width: 34px;
        }
        .style7
        {
            width: 416px;
        }
        .style8
        {
            width: 4px;
        }
    </style>
</head>
<body>    
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div class="Content">   
    <div class="H1">Maintenance Item - Job File</div>
    <hr style="color:Blue" /> 
        <br />                     
        <asp:Panel runat="server" ID="pnlHd">
                <table>
                    <tr>
                    <td class="style4" >Quick Search </td>
                    <td class="style8">:</td>
                        <td>
                            <asp:TextBox ID="tbFilter" runat="server" CssClass="TextBox" Width="120px" />
                            <asp:DropDownList ID="ddlField" runat="server" CssClass="DropDownList">
                                <asp:ListItem Selected="true" Text="Maintenance Item" Value="MaintenanceItem"></asp:ListItem>
                                <asp:ListItem Text="Maintenance Item Name" Value="ItemName"></asp:ListItem>                   
                                <asp:ListItem Text="Job Code" Value="Job"></asp:ListItem>
                                <asp:ListItem Text="Job Name" Value="JobName"></asp:ListItem>
                                <asp:ListItem Text="Job Description" Value="JobDescription"></asp:ListItem>
                                <asp:ListItem Text="First Schedule" Value="dbo.FormatDate(FirstSchedule)"></asp:ListItem>
                                <asp:ListItem Text="MTN Pattern" Value="PatternName"></asp:ListItem>                            
                                <asp:ListItem Text="Active" Value="FgActive"></asp:ListItem>
                            </asp:DropDownList>
                            <asp:Button ID="btnSearch2" runat="server" class="bitbtn btnsearch" 
                                Text="Search" />
                            <asp:Button ID="btnExpand" runat="server" class="btngo" Text="..." />
                        </td>
                        <td class="style6">
                            &nbsp;</td>
                        <td>
                            Show Records :</td>
                        <td>
                            <asp:DropDownList ID="ddlRow" runat="server" AutoPostBack="True" 
                                CssClass="DropDownList">
                                <asp:ListItem Selected="True" Value="15">Choose One</asp:ListItem>
                                <asp:ListItem>20</asp:ListItem>
                                <asp:ListItem>30</asp:ListItem>
                                <asp:ListItem>40</asp:ListItem>
                                <asp:ListItem>50</asp:ListItem>
                                <asp:ListItem>100</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td>
                            Rows</td>
                    </tr>
            </table>
         <asp:Panel runat="server" ID="pnlSearch" Visible="false">
            <table>   
                <tr>
                <td style="text-align:right" class="style5">&nbsp;</td>
                <td>
                    <asp:DropDownList ID="ddlNotasi" runat="server" CssClass="DropDownList">
                        <asp:ListItem Selected="true" Text="OR" Value="OR"></asp:ListItem>
                        <asp:ListItem Text="AND" Value="AND"></asp:ListItem>
                    </asp:DropDownList>
                    <asp:TextBox runat="server" ID ="tbfilter2" CssClass="TextBox" Width="120px"/> 
                      <asp:DropDownList runat="server" ID="ddlField2" CssClass="DropDownList" >
                        <asp:ListItem Selected="true" Text="Maintenance Item" Value="MaintenanceItem"></asp:ListItem>
                        <asp:ListItem Text="Maintenance Item Name" Value="ItemName"></asp:ListItem>                   
                        <asp:ListItem Text="Job Code" Value="Job"></asp:ListItem>
                        <asp:ListItem Text="Job Name" Value="JobName"></asp:ListItem>
                        <asp:ListItem Text="Job Description" Value="JobDescription"></asp:ListItem>
                        <asp:ListItem Text="First Schedule" Value="dbo.FormatDate(FirstSchedule)"></asp:ListItem>
                        <asp:ListItem Text="MTN Pattern" Value="PatternName"></asp:ListItem>                            
                        <asp:ListItem Text="Active" Value="FgActive"></asp:ListItem>                                                    
                      </asp:DropDownList>
                </td>
                </tr>
            </table>
        </asp:Panel>
        </asp:Panel>
        
              <table width="100%">
                <tr>
                    <td class="style1">Maintenance Section</td>
                    <td class="style3">:</td>
                    <td class="style7">
                        <asp:TextBox CssClass="TextBox" Id="tbCode" runat="server" Width="120px" AutoPostBack="True"/>
                        <asp:TextBox CssClass="TextBox" ID="tbName" runat="server" Width="230px" 
                            Enabled="False" /> 
                        <asp:Button ID="btnSearch" runat="server" class="btngo" Text="..."/> 
                    </td>
                    
                    <td><asp:Button class="bitbtn btncopyto" runat="server" ID="btnCopy" Text="Copy Job" />            
                        <asp:Button class="bitbtn btnprint" runat="server" ID="btnPrint" Text="Print" />            
                    </td>
                    
                </tr>                 
              </table>
            
            <asp:Panel runat="server" ID="PanelCopy" Visible="false">
            <table style="width: 1130px">   
                <tr>
                <td style="text-align:right" class="style2">&nbsp;</td>
                <td>
                    <fieldset style="width:450px">
                        <legend>&nbsp;[Copy Job]&nbsp;</legend>
                        <table style="width: 557px">
                        <tr>
                            <td>From Maintenance Item</td>
                            <td>:</td>
                            <td><asp:TextBox ID="tbItemFrom" runat="server" AutoPostBack="True" CssClass="TextBox" Width="120px" />
                                <asp:TextBox ID="tbItemNameFrom" runat="server" CssClass="TextBox" Enabled="False" Width="230px" />
                                <asp:Button ID="btnItemFrom" runat="server" class="btngo" Text="..." />
                            </td>
                        </tr>
                            <tr>
                                <td>To Maintenance Item</td>
                                <td>:</td>
                                <td><asp:TextBox ID="tbItemTo" runat="server" AutoPostBack="True" CssClass="TextBox" Width="120px" />
                                    <asp:TextBox ID="tbItemNameTo" runat="server" CssClass="TextBox" Enabled="False" Width="230px" />
                                    <asp:Button ID="btnItemTo" runat="server" class="btngo" Text="..." />
                                    &nbsp;<asp:Button ID="btnCopyOK" runat="server" class="btngo" Text="G O" 
                                        Width="24px" />
                                </td>
                            </tr>
                        </table>
                    </fieldset></td>
                </tr>
            </table>
        </asp:Panel>  
       
        <asp:Panel runat="server" ID="PnlAssign">     
             <div style="border:0px  solid; width:100%; height:100%; overflow:auto;">
                    <asp:GridView id="DataGrid" runat="server" ShowFooter="True" AllowSorting="True" 
                            AutoGenerateColumns="False" AllowPaging="True" CssClass="Grid">
						<HeaderStyle CssClass="GridHeader" wrap="false"></HeaderStyle>
						<RowStyle CssClass="GridItem" Wrap="false"/>
						<AlternatingRowStyle CssClass="GridAltItem"/>
						<FooterStyle CssClass="GridFooter" />
						<PagerStyle CssClass="GridPager" />
				      <Columns>
							<asp:TemplateField HeaderText="Maintenance Item" HeaderStyle-Width="130" SortExpression="MaintenanceItem">
								<Itemtemplate>
									<asp:Label Runat="server" ID="ItemNo" text='<%# DataBinder.Eval(Container.DataItem, "MaintenanceItem") %>'>
									</asp:Label>
								</Itemtemplate>	
								<EditItemTemplate>
									<asp:TextBox Runat="server" ID="ItemNoEdit" MaxLength="10" Width="65%" CssClass="TextBox" Text='<%# DataBinder.Eval(Container.DataItem, "MaintenanceItem") %>' ontextchanged="ItemNo_TextChanged" AutoPostBack="true"/>
									<cc1:TextBoxWatermarkExtender ID="ItemNoEdit_WtExt" runat="server" Enabled="True" TargetControlID="ItemNoEdit" WatermarkText="can't blank" WatermarkCssClass="Watermarked"></cc1:TextBoxWatermarkExtender>									
									<asp:Button runat="server" ID="btnItemEdit" CommandName="btnItemEdit" CssClass="bitbtn btngo" Text="..." />
                				</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="ItemNoAdd" CssClass="TextBox" MaxLength="10" Width="65%" ontextchanged="ItemNo_TextChanged" AutoPostBack="true" Runat="Server" />
									<cc1:TextBoxWatermarkExtender ID="ItemNoAdd_WtExt" runat="server" Enabled="True" TargetControlID="ItemNoAdd" WatermarkText="can't blank" WatermarkCssClass="Watermarked"></cc1:TextBoxWatermarkExtender>									
								    <asp:Button runat="server" ID="btnItemAdd" CommandName="btnItemAdd" CssClass="bitbtn btngo" Text="..." />                					
								</FooterTemplate>															
							</asp:TemplateField>	
							
							<asp:TemplateField HeaderText="Maintenance Item Name" HeaderStyle-Width="180" SortExpression="MaintenanceItemName">
								<Itemtemplate>
									<asp:Label Runat="server" ID="ItemName" text='<%# DataBinder.Eval(Container.DataItem, "MaintenanceItemName") %>'>
									</asp:Label>
								</Itemtemplate>	
								<EditItemTemplate>
									<asp:Label Runat="server" ID="ItemNameEdit" Text='<%# DataBinder.Eval(Container.DataItem, "MaintenanceItemName") %>'>
									</asp:Label>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:Label Runat="server" ID="ItemNameAdd">
									</asp:Label>
								</FooterTemplate>							
							</asp:TemplateField>													
										
							<asp:TemplateField HeaderText="Job Code" HeaderStyle-Width="60" SortExpression="Job">
								<Itemtemplate>
									<asp:Label Runat="server" Width="100%" ID="Job" text='<%# DataBinder.Eval(Container.DataItem, "Job") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
								     <asp:TextBox ID="JobEdit" CssClass="TextBox" Width="100%" MaxLength="5"  Text='<%# DataBinder.Eval(Container.DataItem, "Job") %>' Runat="Server"/>								   									
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="JobAdd" CssClass="TextBox" Width="100%" MaxLength="5" Runat="Server"/>								   
								</FooterTemplate>
							    <HeaderStyle Width="60px" />
							</asp:TemplateField>											                            								
							
							<asp:TemplateField HeaderText="Job Name" HeaderStyle-Width="180" SortExpression="JobName">
								<Itemtemplate>
									<asp:Label Runat="server" Width="100%" ID="JobName" text='<%# DataBinder.Eval(Container.DataItem, "JobName") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:TextBox ID="JobNameEdit" CssClass="TextBox" Width="100%" MaxLength="60" Text='<%# DataBinder.Eval(Container.DataItem, "JobName") %>' Runat="Server"/>								   																		
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="JobNameAdd" CssClass="TextBox" Width="100%" MaxLength="60" Runat="Server"/>								   
								</FooterTemplate>
							    <HeaderStyle Width="180px" />
							</asp:TemplateField>		
							
							<asp:TemplateField HeaderText="Job Description" HeaderStyle-Width="250" SortExpression="JobDescription" ItemStyle-Wrap = "true">
								<Itemtemplate>
									<asp:Label Runat="server" Width="100%" ID="JobDescription" text='<%# DataBinder.Eval(Container.DataItem, "JobDescription") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:TextBox ID="JobDescriptionEdit" CssClass="TextBox" Width="100%" MaxLength="255" Text='<%# DataBinder.Eval(Container.DataItem, "JobDescription") %>' Runat="Server"/>								   																		
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="JobDescriptionAdd" CssClass="TextBox" Width="100%" MaxLength="255" Runat="Server"/>								   
								</FooterTemplate>
							    <HeaderStyle Width="250px" />
							</asp:TemplateField>	
							
							<asp:TemplateField HeaderText="First Schedule" HeaderStyle-Width="100" SortExpression="FirstSchedule">
								<Itemtemplate>
									<asp:Label Runat="server" ID="FirstSchedule" text='<%# DataBinder.Eval(Container.DataItem, "FirstScheduleV") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<BDP:BasicDatePicker ID="FirstScheduleEdit" runat="server" DateFormat="dd/MMM/yyyy" 
                                      ReadOnly = "true" ValidationGroup="Input"
                                      ButtonImageHeight="19px" ButtonImageWidth="20px" 
                                      DisplayType="TextBoxAndImage" 
                                      TextBoxStyle-CssClass="TextDate" AutoPostBack = "True" 
                                      ShowNoneButton="False" SelectedValue = '<%# DataBinder.Eval(Container.DataItem, "FirstSchedule") %>' >
                                    <TextBoxStyle CssClass="TextDate" />
                                    </BDP:BasicDatePicker>                
                               	</EditItemTemplate>
								<FooterTemplate>
									<BDP:BasicDatePicker ID="FirstScheduleAdd" runat="server" DateFormat="dd/MMM/yyyy" 
                                      ReadOnly = "true" ValidationGroup="Input"
                                      ButtonImageHeight="19px" ButtonImageWidth="20px" 
                                      DisplayType="TextBoxAndImage" 
                                      TextBoxStyle-CssClass="TextDate" AutoPostBack="True" 
                                      ShowNoneButton="False" >
                                     <TextBoxStyle CssClass="TextDate" />
                                    </BDP:BasicDatePicker>   
								</FooterTemplate>								
							</asp:TemplateField>																						
							
							<%--<asp:TemplateField HeaderText="Maintenance Duration" HeaderStyle-Width="40" SortExpression="MaintenanceDuration">
								<Itemtemplate>
									<asp:Label Runat="server" Width="100%" ID="MaintenanceDuration" text='<%# DataBinder.Eval(Container.DataItem, "MaintenanceDurationName") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:DropDownList ID="MaintenanceDurationEdit" Runat="Server" Width="100%" 
                                        CssClass="DropDownList" DataSourceID="dsMaintenanceDuration" DataTextField="DurationName" Text='<%# DataBinder.Eval(Container.DataItem, "MaintenanceDuration") %>' 
                                        DataValueField="DurationCode">                                         
									</asp:DropDownList> 
								</EditItemTemplate>
								<FooterTemplate>
									<asp:DropDownList ID="MaintenanceDurationAdd" Runat="Server" Width="100%" 
                                        CssClass="DropDownList" DataSourceID="dsMaintenanceDuration" DataTextField="DurationName" 
                                        DataValueField="DurationCode">                                         
									</asp:DropDownList> 
								</FooterTemplate>
							    <HeaderStyle Width="40px" />
							</asp:TemplateField>--%>
							
							<asp:TemplateField HeaderText="MTN Pattern" HeaderStyle-Width="40" SortExpression="MTNPattern">
								<Itemtemplate>
									<asp:Label Runat="server" Width="100%" ID="MTNPattern" text='<%# DataBinder.Eval(Container.DataItem, "MTNPatternName") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:DropDownList ID="MTNPatternEdit" Runat="Server" Width="100%" 
                                        CssClass="DropDownList" DataSourceID="dsMTNPattern" DataTextField="PatternName" Text='<%# DataBinder.Eval(Container.DataItem, "MTNPattern") %>' 
                                        DataValueField="PatternCode">                                         
									</asp:DropDownList> 
								</EditItemTemplate>
								<FooterTemplate>
									<asp:DropDownList ID="MTNPatternAdd" Runat="Server" Width="100%" 
                                        CssClass="DropDownList" DataSourceID="dsMTNPattern" DataTextField="PatternName" 
                                        DataValueField="PatternCode">                                         
									</asp:DropDownList> 
								</FooterTemplate>
							    <HeaderStyle Width="40px" />
							</asp:TemplateField>										
							
							<asp:TemplateField HeaderText="Active" HeaderStyle-Width="100" SortExpression="FgActive">
								<Itemtemplate>
									<asp:Label Runat="server" ID="FgActive" Width="100%"  text='<%# DataBinder.Eval(Container.DataItem, "FgActive") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
								    <asp:DropDownList CssClass="DropDownList" ID="FgActiveEdit" Runat="Server" Width="100%" text='<%# DataBinder.Eval(Container.DataItem, "FgActive") %>'> 
									   <asp:ListItem Selected="True">Y</asp:ListItem>
									  <asp:ListItem>N</asp:ListItem>									  
									</asp:DropDownList>												    
								</EditItemTemplate>
								<FooterTemplate>								
									<asp:DropDownList ID="FgActiveAdd" Runat="Server" Width="100%" CssClass="DropDownList" >                                         
									   <asp:ListItem Selected="True">Y</asp:ListItem>
									  <asp:ListItem>N</asp:ListItem>									  
									</asp:DropDownList>
								</FooterTemplate>							    
							</asp:TemplateField>										
									
							<asp:TemplateField HeaderText="Action" headerstyle-width="250" >
							    <ItemTemplate>
							        <asp:Button ID="btnCopy" runat="server" class="bitbtndt btnadd"  Text="Copy" CommandName="Copy" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>"/>
								    <asp:Button ID="btnEdit" runat="server" class="bitbtndt btnedit" Text="Edit" CommandName="Edit"/>
									<asp:Button ID="btndelete" runat="server" class="bitbtndt btndelete" Text="Delete" OnClientClick="return confirm('Sure to delete this data?');" CommandName="Delete"/>																		
									<asp:Button ID="btnView" runat="server" class="bitbtndt btndetail" Text="Spare Part" CommandName="View" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" Width = "80px"/>
								</ItemTemplate>
								<EditItemTemplate>
									<asp:Button ID="btnUpdate" runat="server" class="bitbtndt btnsave" Text="Save" CommandName="Update"/>									
                                    <asp:Button ID="btnCancel" runat="server" class="bitbtndt btncancel" Text="Cancel" CommandName="Cancel"/>									
								</EditItemTemplate>
								<FooterTemplate>
									<asp:Button class="bitbtndt btnadd" runat="server" ID="btnAdd" Text="Add" CommandName="Insert"/>									
								</FooterTemplate>
							    <HeaderStyle Width="250px" />
							</asp:TemplateField>							
    					</Columns>
                        </asp:GridView> 
                        </div>
        </asp:Panel>
        <asp:Panel ID="pnlNewSlip" runat="server" Visible ="false">
            <table width="100%">
                <tr>
                    <td class="style2">Maintenance Item</td>
                    <td>:</td>
                    <td>
                        <asp:TextBox CssClass="TextBox" Id="tbMaintenanceCode" runat="server" Width="120px" Enabled="False"/>
                        <asp:TextBox CssClass="TextBox" ID="tbMaintenanceName" runat="server" 
                            Width="230px" Enabled="False" />                         
                    </td>
                    
                </tr>  
                 
                <tr>
                    <td class="style2">Job</td>
                    <td>:</td>
                    <td><asp:TextBox ID="tbJobCode" runat="server" CssClass="TextBox" Enabled="False" 
                            Width="120px" />
                        <asp:TextBox ID="tbJobNameV" runat="server" CssClass="TextBox" Enabled="False" 
                            Width="405px" />
                    </td>
                </tr>
                 
            </table> 
            <table>
                   <div style="border:0px  solid; width:100%; height:100%; overflow:auto;">
                    <asp:GridView id="DataGridDt" runat="server" ShowFooter="True" AllowSorting="True" 
                            AutoGenerateColumns="False" AllowPaging="True" CssClass="Grid">
						<HeaderStyle CssClass="GridHeader" wrap="false"></HeaderStyle>
						<RowStyle CssClass="GridItem" Wrap="false"/>
						<AlternatingRowStyle CssClass="GridAltItem"/>
						<FooterStyle CssClass="GridFooter" />
						<PagerStyle CssClass="GridPager" />
				      <Columns>
							<asp:TemplateField HeaderText="Spare Part Code" HeaderStyle-Width="180" SortExpression="Material">
								<Itemtemplate>
									<asp:Label Runat="server" Width="100%"  ID="Material" text='<%# DataBinder.Eval(Container.DataItem, "Material") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
								     <%--<asp:Label Runat="server" Width="100%"  ID="MaterialEdit" text='<%# DataBinder.Eval(Container.DataItem, "Material") %>'>
									</asp:Label>--%>
								     <asp:TextBox ID="MaterialEdit" ontextchanged="Material_TextChanged" AutoPostBack="true" CssClass="TextBox" Width="80%" Text='<%# DataBinder.Eval(Container.DataItem, "Material") %>' Runat="Server"/>								   									
								     <asp:Button ID="btnMaterialEdit" CommandName="btnMaterialEdit" runat="server" class="btngo" Text="..."/> 
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="MaterialAdd" ontextchanged="Material_TextChanged" AutoPostBack="true" CssClass="TextBox" Width="80%" MaxLength="5" Runat="Server"/>								   
									<asp:Button ID="btnMaterialAdd" CommandName="btnMaterialAdd" runat="server" class="btngo" Text="..."/> 									
								</FooterTemplate>
							    <HeaderStyle Width="180px" />
							</asp:TemplateField>			
																						
							<asp:TemplateField HeaderText="Spare Part Name" HeaderStyle-Width="255" SortExpression="MaterialName">
								<Itemtemplate>
									<asp:Label Runat="server" ID="MaterialName" Width="100%"  text='<%# DataBinder.Eval(Container.DataItem, "MaterialName") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:Label Runat="server" ID="MaterialNameEdit" Width="100%"  text='<%# DataBinder.Eval(Container.DataItem, "MaterialName") %>'>
									</asp:Label>													    
								</EditItemTemplate>
								<FooterTemplate>
									<asp:Label Runat="server" ID="MaterialNameAdd" Width="100%"  text='<%# DataBinder.Eval(Container.DataItem, "MaterialName") %>'>
									</asp:Label>
								</FooterTemplate>							    
							</asp:TemplateField>																						
							
							<asp:TemplateField HeaderText="Specification" HeaderStyle-Width="255" SortExpression="Specification">
								<Itemtemplate>
									<asp:Label Runat="server" ID="Specification" Width="100%"  text='<%# DataBinder.Eval(Container.DataItem, "Specification") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:Label Runat="server" ID="SpecificationEdit" Width="100%"  text='<%# DataBinder.Eval(Container.DataItem, "Specification") %>'>
									</asp:Label>													    
								</EditItemTemplate>
								<FooterTemplate>
									<asp:Label Runat="server" ID="SpecificationAdd" Width="100%"  text='<%# DataBinder.Eval(Container.DataItem, "Specification") %>'>
									</asp:Label>
								</FooterTemplate>							    
							</asp:TemplateField>	
										
							<asp:TemplateField HeaderText="Qty" HeaderStyle-Width="70" SortExpression="Qty">
								<Itemtemplate>
									<asp:Label Runat="server" Width="100%" ID="Qty" text='<%# DataBinder.Eval(Container.DataItem, "Qty") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
								     <asp:TextBox ID="QtyEdit" CssClass="TextBox" Width="100%" Text='<%# DataBinder.Eval(Container.DataItem, "Qty") %>' Runat="Server"/>								   									
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="QtyAdd" CssClass="TextBox" Width="100%" MaxLength="5" Runat="Server"/>								   
								</FooterTemplate>
							    <HeaderStyle Width="70px" />
							</asp:TemplateField>										                            								
							
							<asp:TemplateField HeaderText="Unit" HeaderStyle-Width="80" SortExpression="Unit">
								<Itemtemplate>
									<asp:Label Runat="server" ID="Unit" Width="100%" text='<%# DataBinder.Eval(Container.DataItem, "Unit") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:DropDownList Runat="server" ID="UnitEdit" Width="100%" 
                                        CssClass="DropDownList" SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Unit") %>'
                                        DataSourceID="dsUnit" DataTextField="Unit_Code" 
                                        DataValueField="Unit_Code" Enabled ="false">                                        
									</asp:DropDownList>								    
								</EditItemTemplate>
								<FooterTemplate>
									<asp:DropDownList ID="UnitAdd" Runat="Server" Width="100%" 
                                        CssClass="DropDownList" DataSourceID="dsUnit" DataTextField="Unit_Code" 
                                        DataValueField="Unit_Code" Enabled = "false">                                         
									</asp:DropDownList>
								</FooterTemplate>
							    <HeaderStyle Width="80px" />
							</asp:TemplateField>																								
																
							<asp:TemplateField HeaderText="Action" headerstyle-width="180" >
							    <ItemTemplate>
								    <asp:Button ID="btnEdit" runat="server" class="bitbtndt btnedit" Text="Edit" CommandName="Edit"/>
									<asp:Button ID="btndelete" runat="server" class="bitbtndt btndelete" Text="Delete" OnClientClick="return confirm('Sure to delete this data?');" CommandName="Delete"/>																		
								</ItemTemplate>
								<EditItemTemplate>
									<asp:Button ID="btnUpdate" runat="server" class="bitbtndt btnsave" Text="Save" CommandName="Update"/>									
                                    <asp:Button ID="btnCancel" runat="server" class="bitbtndt btncancel" Text="Cancel" CommandName="Cancel"/>									
								</EditItemTemplate>
								<FooterTemplate>
									<asp:Button class="bitbtndt btnadd" runat="server" ID="btnAdd" Text="Add" CommandName="Insert"/>									
								</FooterTemplate>
							    <HeaderStyle Width="180px" />
							</asp:TemplateField>							
    					</Columns>
                        </asp:GridView>
                        <table>
                        <tr>
                            <td>
                            <asp:Button class="bitbtn btncancel" runat="server" ID="btnCancelSlip" Text="Back" />                                                     
                            </td>                                   
                        </tr>
                        </table>
                 </div>            
            </table>
        </asp:Panel>        
    </div>    
    
    <%--<asp:SqlDataSource ID="dsMaintenanceDuration" runat="server" SelectCommand="EXEC S_GetMsMaintenanceDurationChoose"></asp:SqlDataSource>--%>
    <asp:SqlDataSource ID="dsUnit" runat="server" SelectCommand="EXEC S_GetUnit"></asp:SqlDataSource>
    <asp:SqlDataSource ID="dsMTNPattern" runat="server" SelectCommand="EXEC S_GetMTNPatternChoose"></asp:SqlDataSource>
    <asp:Label ID="lbstatus" runat="server" ForeColor="Red"/>
    </form>
</body>
</html>
