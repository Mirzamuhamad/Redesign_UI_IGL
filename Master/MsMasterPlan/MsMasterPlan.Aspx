<%@ Page Language="VB" AutoEventWireup="false" CodeFile="MsMasterPlan.Aspx.vb" Inherits="Master_MsMasterPlan_MsMasterPlan" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<%@ Register Assembly="BasicFrame.WebControls.BasicDatePicker" Namespace="BasicFrame.WebControls"
    TagPrefix="BDP" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">


<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Master Plan</title>    
    <script src="../../Function/Function.JS" type="text/javascript"></script>
     <script src="../../Function/OpenDlg.JS" type="text/javascript"></script>     
     <script type="text/javascript">
         function OpenPopup() {
             window.open("../../SeaDlg.Aspx", "List", "scrollbars=yes,resizable=no,width=500,height=400");
             return false;
         }
         function openprintdlg() {
             var wOpens;
             wOpens = window.open("../../Rpt/PrintForm.Aspx", "List", "scrollbars=yes,resizable=yes,width=500,height=400");
             wOpens.moveTo(0, 0);
             wOpens.resizeTo(screen.width, screen.height);
         }

         function OpenPopup() {
             window.open("../../SearchDlg.Aspx", "List", "scrollbars=yes,resizable=no,width=500,height=400");
             return false;
         }    
    </script>
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div class="Content">
    <div class="H1">Master Plan</div>
    <hr style="color:Blue" />    
    <asp:Panel id="pnlHd" runat="server">
    <table>
        <tr>
            <td style="width:100px;text-align:right" > Quick Search : </td>
            
                <td>
                   <asp:TextBox runat="server" ID ="tbFilter" CssClass="TextBox"/> 
                    <asp:DropDownList runat="server" ID="ddlField" CssClass="DropDownList" >
                      <asp:ListItem Selected="true" Text="Job Code" Value="A.JobCode"></asp:ListItem>
                      <%--<asp:ListItem Text="TransNmbr" Value="TransNmbr"></asp:ListItem>--%>
                      <asp:ListItem Text="Job Name" Value="A.JobName"></asp:ListItem>
                      <asp:ListItem Text="Status Tanam" Value="PlantPeriodName"></asp:ListItem>                          
                    </asp:DropDownList>  
                       
                    <asp:Button class="bitbtn btnsearch" runat="server" ID="btnSearch" Text="Search" />
                    <asp:Button ID="btnExpand" runat="server" class="btngo" Text="..."/>
                    <asp:Button class="bitbtn btnexcel" runat="server" ID="btnExport" Text="Export" 
                     Visible="False"/>        
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
                  <asp:DropDownList ID="ddlField2" runat="server" CssClass="DropDownList">
                  <asp:ListItem Selected="true" Text="Job Code" Value="A.JobCode"></asp:ListItem>
                  <%--<asp:ListItem Text="TransNmbr" Value="TransNmbr"></asp:ListItem>--%>
                  <asp:ListItem Text="Job Name" Value="A.JobName"></asp:ListItem>
                  <asp:ListItem Text="Status Tanam" Value="PlantPeriodName"></asp:ListItem>                      
                </asp:DropDownList>
            </td>
        </tr>
     </table>
     </asp:Panel>
      <br />
      
      
      <asp:Panel runat="server" ID="pnlMaster" Visible="True"> 
      <table>
       <tr>
        <td>Master Plan</td>
            <td>:</td>
            <td><asp:DropDownList runat="server" ID="ddlMaster" CssClass="DropDownList" AutoPostBack="true"> 
            <asp:ListItem Selected="True">Choose One</asp:ListItem>                       
                </asp:DropDownList>
                </td>
                <td> <asp:Button class="bitbtn btnadd" runat="server" ID="btnAdd" Text="Add" CommandName="Insert" />
                     <asp:Button ID="btnFind" runat="server" class="bitbtn btnadd" Text="Find" />
                     <asp:Button class="bitbtn btnedit" runat="server" ID="btnEdit" Text="Edit" CommandName="Edit" />																						 															
					 <asp:Button class="bitbtn btndelete" runat="server" ID="btnDelete" Text="Delete" CommandName="Delete"  OnClientClick="return confirm('Sure to delete this data?');" />																						 																		
				</td>
				<td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Copy From </td>
                <td>:</td> 
                <td><asp:DropDownList runat="server" ID="ddlCopyFrom" CssClass="DropDownList" AutoPostBack="true"> 
            <asp:ListItem Selected="True">Choose One</asp:ListItem>                       
                </asp:DropDownList>
                </td> 
                <td> <asp:Button class="btngo" runat="server" ID="btnCopyFrom" Text="G" ValidationGroup="Input"
                            Width="25px" />
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            
                    <asp:Button ID="btnGetJob" runat="server" class="btngo" Text="Get Job" 
                        ValidationGroup="Input" Width="100px" />
                       
                            </td>
                
       </tr>
    
    
      </table>
      </asp:Panel>
      
      
    <table>
   
        <tr>
            <td>Job Group</td>
            <td>:</td>
            <td><asp:DropDownList runat="server" ID="ddlJobGroup" CssClass="DropDownList" AutoPostBack="true">                        
                </asp:DropDownList></td>
            <td> 
            <asp:Button class="bitbtn btnapply" runat="server" ID="BtnApply1" Text="Apply" />           
            </td>
            
                <td> 
                       &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;     
                    <asp:Button ID="BtnGenerate" runat="server" class="btngo" Text="Generate" 
                        ValidationGroup="Input" Width="100px" />
               </td>
        </tr>
    </table> 
    
    <br />
    
    <asp:GridView id="DataGrid" runat="server" 
            ShowFooter="True" AllowSorting="True"
            AutoGenerateColumns="False" AllowPaging = "True" CssClass="Grid" Width="1435px" >
						<HeaderStyle CssClass="GridHeader"></HeaderStyle>
						<RowStyle CssClass="GridItem" />
						<AlternatingRowStyle CssClass="GridAltItem"/>
						<FooterStyle CssClass="GridFooter" />
						<PagerStyle CssClass="GridPager" />
				      <Columns>
				            <asp:TemplateField HeaderText="Job Code">
								<Itemtemplate>
									<asp:Label Runat="server" ID="JobCode" text='<%# DataBinder.Eval(Container.DataItem, "JobCode") %>'/>
								</Itemtemplate>
								<FooterTemplate>
									<asp:TextBox CssClass="TextBox" OnTextChanged="tbJobCode_TextChanged" runat="server" id="JobCodeAdd" Width="90" 
                                        AutoPostBack="true" />
                                     <asp:Button class="btngo" runat="server" ID="btnJobCodeAdd" Text="..." CommandName="SearchJobCodeAdd"/>                                    
                                     <cc1:TextBoxWatermarkExtender ID="JobCodeAdd_TextBoxWatermarkExtender" 
                                        runat="server" Enabled="True" TargetControlID="JobCodeAdd" 
                                        WatermarkText="[JobCode]" WatermarkCssClass="Watermarked">
                                    </cc1:TextBoxWatermarkExtender>
								</FooterTemplate>													
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Job Name">
								<Itemtemplate>
									<asp:Label Runat="server" ID="JobName" text='<%# DataBinder.Eval(Container.DataItem, "JobName") %>'/>
								</Itemtemplate>	
								<FooterTemplate>
									<asp:TextBox ID="JobNameAdd" CssClass="TextBox" Enabled="false" MaxLength="60" Runat="Server" Width="100%"/>
									<cc1:TextBoxWatermarkExtender ID="JobNameAdd_WtExt" 
                                        runat="server" Enabled="False" TargetControlID="JobNameAdd" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>
								</FooterTemplate>															
							</asp:TemplateField>
								
							<asp:TemplateField HeaderText="Status Tanam">
							
							
								<Itemtemplate>
									<asp:Label Runat="server" ID="StatusTanam" text='<%# DataBinder.Eval(Container.DataItem, "StatusTanam") %>' Width="100" Visible="False" />
									<asp:Label Runat="server" ID="StatusTanamName" text='<%# DataBinder.Eval(Container.DataItem, "StatusTanamName") %>' Width="100" />
								</Itemtemplate>
									
								<FooterTemplate>
									   	<asp:DropDownList ID="StatusTanamAdd" CssClass="DropDownList" Width="100%" runat="server"
								            DataSourceID="dsGetStatusTanam" DataTextField="PlantPeriodName" 
                                            DataValueField="PlantPeriodCode">                               
									    </asp:DropDownList>							 
								</FooterTemplate>										
							</asp:TemplateField>							
							
							<asp:TemplateField HeaderText="Capacity" ItemStyle-HorizontalAlign = "Right" >
								<Itemtemplate>
								<asp:TextBox runat="server" CssClass="TextBox" style="text-align: right" ID="Capacity" Maxlength = "9" text='<%# DataBinder.Eval(Container.DataItem, "Capacity")%>' Width="40" />
								<asp:Label Runat="server" ID="lbUnitCap" text='<%# DataBinder.Eval(Container.DataItem, "Unit") %>'/>
								<asp:Label Runat="server" ID="Label2" text="/HK"/>
                                </Itemtemplate>	
                                <FooterTemplate>
									<asp:TextBox runat="server" CssClass="TextBox" style="text-align: right" ID="CapacityAdd" Maxlength = "9" Width="40" />
									<asp:Label Runat="server" ID="lbUnitCapAdd" text='<%# DataBinder.Eval(Container.DataItem, "Unit") %>'/>
									<asp:Label Runat="server" ID="Label2" text="/HK" />
								</FooterTemplate>								
                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
							</asp:TemplateField>
														
							<asp:TemplateField HeaderText="Convertion" ItemStyle-HorizontalAlign = "Right" SortExpression="QtyConvert">
								<Itemtemplate>
								    <asp:TextBox runat="server" CssClass="TextBox" style="text-align: right" ID="QtyConvert" Maxlength = "9" text='<%# DataBinder.Eval(Container.DataItem, "QtyConvert") %>' Width="35" />
								    <asp:Label Runat="server" ID="lbUnitC" text='<%# DataBinder.Eval(Container.DataItem, "UnitConvert") %>'/>
								    <asp:Label Runat="server" ID="Label1" text="/"/>
								    <asp:TextBox runat="server" CssClass="TextBox" style="text-align: right" 
                                        ID="QtyAreal" Maxlength = "9" 
                                        text='<%# DataBinder.Eval(Container.DataItem, "QtyAreal") %>' Width="20px" />
								    <asp:Label Runat="server" ID="lbUnitA" text='<%# DataBinder.Eval(Container.DataItem, "UnitAreal") %>'/>
								</Itemtemplate>	
								<FooterTemplate>
									<asp:TextBox runat="server" CssClass="TextBox" style="text-align: right" ID="QtyConvertAdd" Maxlength = "9" Width="35" />
									<asp:Label Runat="server" ID="lbUnitCAdd" text='<%# DataBinder.Eval(Container.DataItem, "UnitConvert") %>'/>
									<asp:TextBox runat="server" CssClass="TextBox" style="text-align: right" ID="QtyArealAdd" Text="1" Maxlength = "9" Width="35" />
									<asp:Label Runat="server" ID="lbUnitArAdd" text='<%# DataBinder.Eval(Container.DataItem, "UnitAreal") %>'/>
								</FooterTemplate>		
                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
							</asp:TemplateField>						
							
							<asp:TemplateField HeaderText="Volume (%)" ItemStyle-HorizontalAlign = "Right" SortExpression="Volume">
								<Itemtemplate>
								<asp:TextBox runat="server" CssClass="TextBox" style="text-align: right" ID="Volume" Maxlength = "9" text='<%# DataBinder.Eval(Container.DataItem, "Volume") %>' Width="40" />
								</Itemtemplate>
								 <FooterTemplate>
									<asp:TextBox runat="server" CssClass="TextBox" style="text-align: right" ID="VolumeAdd" Maxlength = "9" Text="100" Width="40" />
								</FooterTemplate>
                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Rotasi" ItemStyle-HorizontalAlign = "Right" >
								<Itemtemplate>
								<asp:DropDownList ID="Rotasi" CssClass="DropDownList" Width="100%" runat="server"
								    SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Rotasi") %>' 
									   DataSourceID="dsGetRotasi" DataTextField="RotationName" 
                                        DataValueField="RotationCode">                               
									</asp:DropDownList>
									<asp:Label runat="server" ID="XRotation" text='<%# DataBinder.Eval(Container.DataItem, "XRotation") %>' Width="40" Visible="False" />	
                                </Itemtemplate>
                                 <FooterTemplate>
									<asp:DropDownList ID="RotasiAdd" CssClass="DropDownList" Width="100%" runat="server"								        
									   DataSourceID="dsGetRotasi" DataTextField="RotationName" 
                                        DataValueField="RotationCode">                               
									</asp:DropDownList>
								</FooterTemplate>
                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Used Material" ItemStyle-HorizontalAlign = "Right" >
								<Itemtemplate>
								<asp:DropDownList runat="server" CssClass="DropDownList" ID="UsedMaterial"
                                  Width="80px" SelectedValue='<%# DataBinder.Eval(Container.DataItem, "UsedMaterial") %>'  >
                                        <asp:ListItem Value = "Y">Y</asp:ListItem>
                                        <asp:ListItem Value = "N">N</asp:ListItem>
                                        <asp:ListItem Value = ""> </asp:ListItem>
                                        </asp:DropDownList>		
                                </Itemtemplate>	
                                <FooterTemplate>
									<asp:DropDownList ID="UsedMaterialAdd" CssClass="DropDownList" Width="100%" runat="server">
									  <asp:ListItem >Y</asp:ListItem>                                     
									  <asp:ListItem Selected="True">N</asp:ListItem>                               
									</asp:DropDownList>								    
								</FooterTemplate>								
                                <ControlStyle Width="40px" />
                                <HeaderStyle Width="40px" />
                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Action" ItemStyle-HorizontalAlign = "Right">
								<Itemtemplate>
								    <asp:Button class="bitbtn btndelete" runat="server" ID="btnDeleteHd" Text="Delete" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" CommandName="DeleteHd"  OnClientClick="return confirm('Sure to delete this data?');" />		
								    <asp:Button ID="btnHKFactor" runat="server" class="bitbtndt btndetail" Text="HKFactor" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" CommandName="HKFactor" Width="80" />							    
								    <asp:Button ID="btnPercentage" runat="server" class="bitbtndt btndetail" Text="RMPercentage" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" CommandName="Percentage" Width="80" />								
									<asp:Button ID="btnView" runat="server" class="bitbtndt btndetail" Text="Material" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" CommandName="Detail" />
								</Itemtemplate>
								<FooterTemplate>
								<asp:Button ID="btnAddHd" runat="server" class="bitbtn btnadd" Text="Add" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" CommandName="InsertHd" />
								</FooterTemplate>										
                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
							</asp:TemplateField>    
					   									
    					</Columns>
        </asp:GridView>      

    </asp:Panel>
    
<%--    <asp:panel runat="server" ID="pnlInput" Visible="true">
    
    <asp:GridView id="GridView1" runat="server" 
            ShowFooter="True" AllowSorting="True"
            AutoGenerateColumns="False" CssClass="Grid" Width="1435px" >
						<HeaderStyle CssClass="GridHeader"></HeaderStyle>
						<RowStyle CssClass="GridItem" />
						<AlternatingRowStyle CssClass="GridAltItem"/>
						<FooterStyle CssClass="GridFooter" />
						<PagerStyle CssClass="GridPager" />
				      <Columns>
				            <asp:TemplateField HeaderText="Job Code">				            								
								<FooterTemplate>
									<asp:TextBox CssClass="TextBox" OnTextChanged="tbJobCode_TextChanged" runat="server" id="JobCodeAdd" Width="90" 
                                        AutoPostBack="true" />
                                     <asp:Button class="btngo" runat="server" ID="btnJobCodeAdd" Text="..." CommandName="SearchJobCodeAdd"/>                                    
                                     <cc1:TextBoxWatermarkExtender ID="JobCodeAdd_TextBoxWatermarkExtender" 
                                        runat="server" Enabled="True" TargetControlID="JobCodeAdd" 
                                        WatermarkText="[JobCode]" WatermarkCssClass="Watermarked">
                                    </cc1:TextBoxWatermarkExtender>
								</FooterTemplate>													
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Job Name">
							
								<FooterTemplate>
									<asp:TextBox ID="JobNameAdd" CssClass="TextBox" Enabled="false" MaxLength="60" Runat="Server" Width="100%"/>
									<cc1:TextBoxWatermarkExtender ID="JobNameAdd_WtExt" 
                                        runat="server" Enabled="False" TargetControlID="JobNameAdd" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>
								</FooterTemplate>															
							</asp:TemplateField>
								
							<asp:TemplateField HeaderText="Status Tanam">
							
								<FooterTemplate>
									   	<asp:DropDownList ID="StatusTanamAdd" CssClass="DropDownList" Width="100%" runat="server"
								            DataSourceID="dsGetStatusTanam" DataTextField="PlantPeriodName" 
                                            DataValueField="PlantPeriodCode">                               
									    </asp:DropDownList>							 
								</FooterTemplate>										
							</asp:TemplateField>							
							
							<asp:TemplateField HeaderText="Capacity" ItemStyle-HorizontalAlign = "Right" >
								
                                <FooterTemplate>
									<asp:TextBox runat="server" CssClass="TextBox" style="text-align: right" ID="CapacityAdd" Maxlength = "9" Width="40" />
									<asp:Label Runat="server" ID="lbUnitCapAdd" text='<%# DataBinder.Eval(Container.DataItem, "Unit") %>'/>
									<asp:Label Runat="server" ID="Label2" text="/HK"/>
								</FooterTemplate>								
                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
							</asp:TemplateField>
														
							<asp:TemplateField HeaderText="Convertion" ItemStyle-HorizontalAlign = "Right" SortExpression="QtyConvert">
								
								<FooterTemplate>
									<asp:TextBox runat="server" CssClass="TextBox" style="text-align: right" ID="QtyConvertAdd" Maxlength = "9" Width="35" />
									<asp:Label Runat="server" ID="lbUnitCAdd" text='<%# DataBinder.Eval(Container.DataItem, "UnitConvert") %>'/>
									<asp:TextBox runat="server" CssClass="TextBox" style="text-align: right" ID="QtyArealAdd" Text="1" Maxlength = "9" Width="35" />
									<asp:Label Runat="server" ID="lbUnitArAdd" text='<%# DataBinder.Eval(Container.DataItem, "UnitAreal") %>'/>
								</FooterTemplate>		
                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
							</asp:TemplateField>						
							
							<asp:TemplateField HeaderText="Volume (%)" ItemStyle-HorizontalAlign = "Right" SortExpression="Volume">
								
								 <FooterTemplate>
									<asp:TextBox runat="server" CssClass="TextBox" style="text-align: right" ID="VolumeAdd" Maxlength = "9" Text="100" Width="40" />
								</FooterTemplate>
                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Rotasi" ItemStyle-HorizontalAlign = "Right" >
								
                                 <FooterTemplate>
									<asp:DropDownList ID="RotasiAdd" CssClass="DropDownList" Width="100%" runat="server"								        
									   DataSourceID="dsGetRotasi" DataTextField="RotationName" 
                                        DataValueField="RotationCode">                               
									</asp:DropDownList>
								</FooterTemplate>
                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Used Material" ItemStyle-HorizontalAlign = "Right" >
                                <FooterTemplate>
									<asp:DropDownList ID="UsedMaterialAdd" CssClass="DropDownList" Width="100%" runat="server">
									  <asp:ListItem >Y</asp:ListItem>                                     
									  <asp:ListItem Selected="True">N</asp:ListItem>                               
									</asp:DropDownList>								    
								</FooterTemplate>								
                                <ControlStyle Width="40px" />
                                <HeaderStyle Width="40px" />
                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Action" ItemStyle-HorizontalAlign = "Right">
								<FooterTemplate>
								<asp:Button ID="btnAddHd" runat="server" class="bitbtn btnadd" Text="Add" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" CommandName="InsertHd" />
								</FooterTemplate>
										
                             <ItemStyle HorizontalAlign="Right"></ItemStyle>
							</asp:TemplateField>       
					   									
    					</Columns>
        </asp:GridView>      
    
    </asp:panel>--%>
    
    
    <asp:Panel runat="server" ID="pnlView" Visible="False">
        <table>
            <tr>
                <td>Master Plan Code</td>
                <td>:</td> 
                <td><asp:TextBox runat="server" CssClass="TextBox" ID="TbMasterCode" Enabled="False" MaxLength = "100" Width="300"/>
                </td>
            </tr>  
            <tr>
                <td>Master Plan Name</td>
                <td>:</td> 
                <td><asp:TextBox runat="server" CssClass="TextBox" ID="tbMasterName" MaxLength = "100" Width="300"/>
                <asp:Label ID="Label4" runat="server" ForeColor="Red" Text="*"></asp:Label></td>
            </tr>  
            <tr>
                <td>Effective Date</td>
                <td>:</td>
                <td>
                    <BDP:BasicDatePicker ID="tbDate" runat="server" DateFormat="dd MMM yyyy" 
                        ReadOnly = "true" ValidationGroup="Input"
                        ButtonImageHeight="19px" ButtonImageWidth="20px" 
                        DisplayType="TextBoxAndImage" 
                        TextBoxStyle-CssClass="TextDate" AutoPostBack="True" 
                        ShowNoneButton="False">
                        <TextBoxStyle CssClass="TextDate" />
                    </BDP:BasicDatePicker>
                </td>                 
            </tr>           
            <tr>
                <td>Varietas</td>
                <td>:</td>
                <td><asp:DropDownList runat="server" CssClass="DropDownList" ID="ddlVarietas" ValidationGroup="Input" MaxLength = "20"
                        Width = "200px" Height="18px">
                    </asp:DropDownList>
                </td>
            </tr>           
            <tr>
                <td>Land Type</td>
                <td>:</td>
                <td><asp:DropDownList runat="server" CssClass="DropDownList" ID="ddlLandType" ValidationGroup="Input" MaxLength = "20"
                        Width = "200px" Height="18px">
                    </asp:DropDownList>
                    
                </td>
            </tr>
            
            <tr>
                <td>Land Scape</td>
                <td>:</td>
                <td><asp:DropDownList runat="server" CssClass="DropDownList" ID="ddlLandScape" ValidationGroup="Input" MaxLength = "20"
                        Width = "200px" Height="18px" >
                    </asp:DropDownList>
                </td>
            </tr>
            
            <tr>
                <td>Plant Type</td>
                <td>:</td>
                <td><asp:DropDownList runat="server" CssClass="DropDownList" ID="ddlPlantType" ValidationGroup="Input" MaxLength = "20"
                        Width = "200px" Height="18px" >
                        <asp:ListItem>Konvensional</asp:ListItem>
                        <asp:ListItem Selected="True" >Tablet</asp:ListItem>
                    </asp:DropDownList></td>
            </tr>
            <tr>
                <td>Pokok</td>
                <td>:</td>
                <td><asp:TextBox runat="server" CssClass="TextBox" ID="QtyPokok" MaxLength = "100" Width="70"/>
                <asp:Label ID="Label87" runat="server" Text="/Ha"></asp:Label>
                </td>
            </tr>
            

            <tr>
                <td align="center" colspan="3">
                    <asp:Button ID="BtnSave" runat="server" class="bitbtndt btnsave" 
                        CommandName="Update" Text="Save" />
                    &nbsp;
                    <asp:Button ID="btnCancel" runat="server" class="bitbtndt btncancel" 
                        CommandName="Cancel" Text="Cancel" />
                    &nbsp;							                                                           
                </td>
                
            </tr>
        </table>
      </asp:Panel>
      
      <asp:Panel ID="pnlHKFactor" runat="server" Visible = "false">  
     <table>
       <tr>
       <td>
            <asp:Label ID="lblJobHK" CssClass="H1" runat="server" Text="" />             
       </td>
       <td>
       </td>
            
                &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                <tr>
                    <td>
                        <asp:Label ID="lblStatusTanamHK" runat="server" CssClass="H1" Text="" />
                    </td>
                </tr>
          
       </tr>
       <tr>
            <td>
            <asp:Button class="bitbtn btnback" runat="server" ID="btnBackHK" Text="Back" />
            <asp:Button class="bitbtn btnapply" runat="server" ID="btnApplyHK" Text="Apply" />
            </td>  
            <td>
                   
            </td>
            <td>
            </td>
          
       </tr>
     </table>
     
     
	 <br />
	 
	 
	 <br />
	 
	    <div style="border:0px  solid; width:100%; height:100%; overflow:auto;">
            <asp:GridView ID="DataGridHK" runat="server" AutoGenerateColumns="False" 
                CssClass="Grid" AllowPaging="false" Visible="True">
                <HeaderStyle CssClass="GridHeader" />
                <RowStyle CssClass="GridItem" />
                <AlternatingRowStyle CssClass="GridAltItem" />
                <PagerStyle CssClass="GridPager" />
                <Columns>
                    <asp:TemplateField HeaderText="No" ItemStyle-HorizontalAlign = "Right" >
                        <Itemtemplate>
                            <asp:Label ID="RotasiNoHK" Runat="server" 
                                text='<%# DataBinder.Eval(Container.DataItem, "RotasiNo") %>' />
                        </Itemtemplate>
                        <ItemStyle HorizontalAlign="Right" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="HK Factor" SortExpression="HKFactor">
                        <ItemTemplate>
                            <asp:TextBox ID="HKFactor" Runat="server" CssClass="TextBox" style="text-align:center;"
                                text='<%# DataBinder.Eval(Container.DataItem, "HKRate","{0:#,##,##}") %>' />
                        </ItemTemplate>
                    </asp:TemplateField> 
                </Columns>
            </asp:GridView>
            <br />
        </div>        
	    <asp:Button class="bitbtn btnback" runat="server" ID="btnbackHK2" Text="Back" />
     </asp:Panel>
      
    
    
    <asp:Panel ID="pnlDt" runat="server" Visible = "false" Width="1187px">  
     <table>
       <tr>
       <td>
            <asp:Label ID="lblJobDt" CssClass="H1" runat="server" Text="" />             
       </td>
       <td>
       </td>
            &nbsp &nbsp &nbsp &nbsp &nbsp
       <td>
            <asp:Label ID="lblStatusTanamDt" CssClass="H1" runat="server" Text="" />             
       </td>
       </tr>
       <tr>
            <td>
            <asp:Button class="bitbtn btnback" runat="server" ID="btnBackDtTop" Text="Back" />
            <asp:Button class="bitbtn btnapply" runat="server" ID="BtnApply" Text="Apply" />
            </td>  
            <td>
                   
            </td>
            <td>
            </td>
          
       </tr>
     </table>
     
     
	 <br />
	 
	 
	 <br />
	 
	    <div style="border:0px  solid; width:100%; height:100%; overflow:auto;">
            <asp:GridView ID="DataGridDt" runat="server" AutoGenerateColumns="False" 
                CssClass="Grid" AllowPaging="false">
                <HeaderStyle CssClass="GridHeader" />
                <RowStyle CssClass="GridItem" />
                <AlternatingRowStyle CssClass="GridAltItem" />
                <PagerStyle CssClass="GridPager" />
                <Columns>
                    <asp:TemplateField HeaderText="No" ItemStyle-HorizontalAlign = "Right" >
                        <Itemtemplate>
                            <asp:Label ID="RotasiNo" Runat="server" 
                                text='<%# DataBinder.Eval(Container.DataItem, "RotasiNo") %>' />
                        </Itemtemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Percentage" SortExpression="RMPercentage">
                        <ItemTemplate>
                            <asp:TextBox ID="RMPercentage" Runat="server" CssClass="TextBox" style="text-align:center;"
                                text='<%# DataBinder.Eval(Container.DataItem, "RMPercentage","{0:#,##,##}") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    
                </Columns>
            </asp:GridView>
            <br />
        </div>        
	    <asp:Button class="bitbtn btnback" runat="server" ID="Button2" Text="Back" />
     </asp:Panel>
     
     
     
     <asp:Panel ID="pnlViewDt" runat="server" Visible = "False">  
     <table>
     <tr>
     <td Width="100%">
     
            <asp:Label ID="lblJobMaterial" CssClass="H1" runat="server" Text=" " />             
       </td>
       <td>
       </td>
            <caption>
                &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                <tr>
                    <td>
                        <asp:Label ID="lblStatusTanamMaterial" runat="server" CssClass="H1" Text=" " />
                    </td>
                </tr>
         </caption>
         </tr>
         <tr>
     <td Width="100%">
     
            <asp:Label ID="lblJobMaterial2" CssClass="H1" Visible="False" runat="server" Text=" " />             
       </td>
       <td>
       </td>
            <caption>
                &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                <tr>
                    <td>
                        <asp:Label ID="lblStatusTanamMaterial2" Visible="False" runat="server" CssClass="H1" Text=" " />
                    </td>
                </tr>
         </caption>
         </tr>
         <caption>
             <hr>
             <tr>
                 <td>
                     <asp:Button ID="btnGetMaterial" runat="server" class="btngo" 
                         Text="Get Material" ValidationGroup="Input" Width="105px" />
                 </td>
             </tr>
             </hr>
         </caption>
     
     
     </table>     
     
	    
     
	 <br />
	 <br />
	 
	    <div style="border:0px  solid; width:100%; height:100%; overflow:auto;">
	    
            <asp:GridView ID="DataGridMaterial" runat="server" AllowPaging="false" 
                AllowSorting="false" AutoGenerateColumns="False" ShowFooter="True">
                <HeaderStyle CssClass="GridHeader" wrap="false" />
                <RowStyle CssClass="GridItem" wrap="false" />
                <AlternatingRowStyle CssClass="GridAltItem" />
                <FooterStyle CssClass="GridFooter" Wrap="false" />
                <PagerStyle CssClass="GridPager" />
                <Columns>
					
                    <asp:TemplateField HeaderStyle-Width="128" HeaderText="Product Code" 
                        SortExpression="ProductCode">
                        <Itemtemplate>
                            <asp:Label ID="ProductCode" Runat="server" 
                                text='<%# DataBinder.Eval(Container.DataItem, "ProductCode") %>'>
                            </asp:Label>
                        </Itemtemplate>
                        <EditItemTemplate>
                          
                            <asp:Label ID="ProductCodeEdit" runat="server" AutoPostBack="true" Maxlength = "12" 
                                CssClass="TextBox" ontextchanged="tbProductCode_TextChanged" 
                                text='<%# DataBinder.Eval(Container.DataItem, "ProductCode") %>' Width="90" />
                            <asp:Button ID="btnProductCode" runat="server" class="btngo" Text="..." CommandName="SearchProductCodeEdit"/>                            
                        </EditItemTemplate>
                        <FooterTemplate>
                           
                            <asp:TextBox ID="ProductCodeAdd" runat="server" AutoPostBack="true" Text = "" 
                                CssClass="TextBox" ontextchanged="tbProductCode_TextChanged" Width="90" MaxLength="12" />
                            <asp:Button ID="btnProductCodeAdd" runat="server" class="btngo" Text="..." CommandName="SearchProductCodeAdd"/>                            
                            </FooterTemplate>
                        <HeaderStyle Width="128" />
                    </asp:TemplateField>
                    
                    <asp:TemplateField HeaderStyle-Width="250" HeaderText="Product Name" 
                        SortExpression="ProductName">
                        <Itemtemplate>
                            <asp:Label ID="ProductName" Runat="server" 
                                text='<%# DataBinder.Eval(Container.DataItem, "ProductName") %>'>
                            </asp:Label>
                        </Itemtemplate>
                        <EditItemTemplate>
                            <asp:Label ID="ProductNameEdit" Runat="server" 
                                text='<%# DataBinder.Eval(Container.DataItem, "ProductName") %>' Width="250%">
                            </asp:Label>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:Label ID="ProductNameAdd" Runat="server" Width="250"> </asp:Label>
                        </FooterTemplate>
                        <HeaderStyle Width="250" />                       
                    </asp:TemplateField>   
                    
                    <asp:TemplateField HeaderText="Qty" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Right" SortExpression="Qty">
						       <Itemtemplate >
									<asp:Label Runat="server" DataFormatString="{0:#,##0.##}" ID="Qty" text='<%# DataBinder.Eval(Container.DataItem, "Qty","{0:#,##,##}") %>'>
									</asp:Label>
								</Itemtemplate>	
								<EditItemTemplate>
									<asp:TextBox Runat="server" ID="QtyEdit" Width="95" MaxLength="22" CssClass="TextBox" Text='<%# DataBinder.Eval(Container.DataItem, "Qty","{0:#,##}") %>'>
									</asp:TextBox>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="QtyAdd" CssClass="TextBox" Runat="Server" Width="95" MaxLength="22"  Text= "0"/>
								</FooterTemplate>									
					           <HeaderStyle Width="100" />
                               <ItemStyle HorizontalAlign="Right" />
					</asp:TemplateField>
                    
                    <asp:TemplateField HeaderText="Unit" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Right" SortExpression="Unit">
						       <Itemtemplate >
									<asp:Label Runat="server" ID="Unit" text='<%# DataBinder.Eval(Container.DataItem, "Unit") %>'>
									</asp:Label>
								</Itemtemplate>	
								<EditItemTemplate>
									<asp:Label Runat="server" ID="UnitEdit" Width="95" MaxLength="22" CssClass="TextBox" Text='<%# DataBinder.Eval(Container.DataItem, "Unit","{0:#,##}") %>'>
									</asp:Label>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:Label ID="UnitAdd" CssClass="TextBox" Runat="Server" Width="95" MaxLength="22"  Text= ""/>
								</FooterTemplate>									
					           <HeaderStyle Width="100" />
                               <ItemStyle HorizontalAlign="Right" />
					</asp:TemplateField>
					
					<asp:TemplateField HeaderText="QtyCap" HeaderStyle-Width="100" ItemStyle-HorizontalAlign="Right" SortExpression="QtyCap">
						       <Itemtemplate>
									<asp:Label Runat="server" ID="QtyCap" DataFormatString="{0:#,##0.##}" text='<%# DataBinder.Eval(Container.DataItem, "QtyCap","{0:#,##,##}") %>'>
									</asp:Label>
								</Itemtemplate>	
								<EditItemTemplate>
									<asp:TextBox Runat="server" ID="QtyCapEdit" Width="95" CssClass="TextBox"  MaxLength="22" Text='<%# DataBinder.Eval(Container.DataItem, "QtyCap","{0:#,##}") %>'>
									</asp:TextBox>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="QtyCapAdd" CssClass="TextBox" Runat="Server" Width="95" MaxLength="22"  Text= "0"/>
								</FooterTemplate>									
					           <HeaderStyle Width="100" />
                               <ItemStyle HorizontalAlign="Right" />
					</asp:TemplateField>
					
							                   
                    
                    <asp:TemplateField HeaderStyle-Width="126px" HeaderText="Action" 
                        ItemStyle-Wrap="false">
                        <EditItemTemplate>
                          
                            <asp:Button ID="btnUpdate0" runat="server" class="bitbtndt btnsave" Text="Save"  CommandName="Update"/>                                                        
             
                            <asp:Button ID="btnCancel0" runat="server" class="bitbtndt btnsave" Text="Cancel"  CommandName="Cancel"/>                                                                                    
                        </EditItemTemplate>
                        <FooterTemplate>
                          
                            <asp:Button ID="btnAdd0" runat="server" class="bitbtndt btnadd" Text="Add"  CommandName="Insert"/>                                                                                                                
                        </FooterTemplate>
                        <ItemTemplate>
                          
                            <asp:Button ID="btnEdit0" runat="server" class="bitbtndt btnedit" Text="Edit"  CommandName="Edit"/>                                                                                                                
                           
                            <asp:Button ID="btnDelete0" runat="server" class="bitbtndt btndelete" Text="Delete" OnClientClick="return confirm('Sure to delete this data?');" CommandName="Delete"/>                                                                                                                                           
                        </ItemTemplate>
                        <HeaderStyle Width="126px" />
                        <ItemStyle Wrap="False" />
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
	    
        </div>        
	    <asp:Button class="bitbtn btnback" runat="server" ID="btnBack2" Text="Back" />
	       				                        
     </asp:Panel>
     <asp:Label ID="lbstatus" ForeColor="red" runat="server"></asp:Label>
    </div>
    
    
    
    
    <asp:GridView ID="GridExport" runat="server" AutoGenerateColumns="True" 
                Visible="true" ShowFooter="False" GridLines="None" Height="79px" PageSize="20" Width="724px">
               <HeaderStyle CssClass="GridHeader" />
               <RowStyle CssClass="GridItem" Wrap="false" />
               <AlternatingRowStyle CssClass="GridAltItem" />
               <PagerStyle CssClass="GridPager" />
           </asp:GridView>  
           <asp:SqlDataSource ID="dsGetRotasi" runat="server" SelectCommand="SELECT RotationCode, RotationName FROM V_MsRotasiFill" ></asp:SqlDataSource>  
           <asp:SqlDataSource ID="dsGetStatusTanam" runat="server" SelectCommand="select PlantPeriodCode, PlantPeriodName from MsPlantPeriod" ></asp:SqlDataSource>   
    <br />
    
    
    
    </form>
</body>
</html>
