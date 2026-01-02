<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TrPlanBlock.Aspx.vb" Inherits="Transcation_TrPlanBlock_TrPlanBlock" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<%@ Register Assembly="BasicFrame.WebControls.BasicDatePicker" Namespace="BasicFrame.WebControls"
    TagPrefix="BDP" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">


<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>TBM / TM Perawatan Planning</title>    
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
 <link href="https://fonts.googleapis.com/css2?family=Muli:wght@300;400;600&display=swap" rel="stylesheet">
    <style type="text/css">
        .style1
        {
            height: 16px;
        }
    </style>
    </head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div class="Content">
    <div class="H1">TBM / TM Perawatan Planning</div>
    <hr style="color:Blue" />    
    <asp:Panel id="pnlHd" runat="server">
    <table>
        <tr>
            <td style="width:100px;text-align:right" >Quick Search :</td>
            <td><asp:TextBox runat="server" ID ="tbFilter" CssClass="TextBox"/> 
                <asp:DropDownList runat="server" ID="ddlField" CssClass="DropDownList" >
                  <asp:ListItem Selected="true" Text="Block Code" Value="A.Block_Code"></asp:ListItem>
                  <asp:ListItem Text="Block Name" Value="A.Block_Name"></asp:ListItem>
                  <asp:ListItem Text="Status Tanam" Value="A.StatusTanam"></asp:ListItem>                          
                  <asp:ListItem Text="Master Planning" Value="B.MasterPlan"></asp:ListItem>                          
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
                  <asp:ListItem Selected="true" Text="Block Code" Value="A.Block_Code"></asp:ListItem>
                  <asp:ListItem Text="Block Name" Value="A.Block_Name"></asp:ListItem>
                  <asp:ListItem Text="Status Tanam" Value="A.StatusTanamCode"></asp:ListItem>                          
                  <asp:ListItem Text="Master Planning" Value="B.MasterPlan"></asp:ListItem>                                             
                </asp:DropDownList>
            </td>
        </tr>
     </table>
     </asp:Panel>
      <br />
      
      
      <asp:Panel runat="server" ID="pnlMaster" Visible="True"> 
      <table>
      <tr>
       
         <td>Reference</td>
         <td>:</td>

            <td><asp:TextBox ID="tbCode" runat="server" CssClass="TextBoxR" Enabled="False" 
                    Visible="True" Width="150px" />
          </td>
            <td>&nbsp;</td>
       </tr>
      <tr>
       
       <td>Date</td>
       <td>:</td>
            <td><BDP:BasicDatePicker ID="tbDate" runat="server" AutoPostBack="True" 
                    ButtonImageHeight="19px" ButtonImageWidth="20px" DateFormat="dd MMM yyyy" 
                    DisplayType="TextBoxAndImage" ReadOnly="true" ShowNoneButton="False" 
                    TextBoxStyle-CssClass="TextDate" ValidationGroup="Input" Visible="True">
                <TextBoxStyle CssClass="TextDate" />
                </BDP:BasicDatePicker>
          </td>
            <td>&nbsp;</td>
       </tr>
       <tr>
       
       <td>Year</td>
       <td>:</td>
            <td><asp:DropDownList ID="ddlYear" runat="server" AutoPostBack="true" 
                    CssClass="DropDownList" Height="16px" Width="137px">
                <asp:ListItem Selected="True">Choose One</asp:ListItem>
                </asp:DropDownList>
           </td>
            <td>&nbsp;</td>
       </tr>
       <tr>
                <td> Divisi&nbsp; </td>	
                <td>:</td>		                
           <td>
               <asp:DropDownList ID="ddlDivisi" runat="server" AutoPostBack="true" 
                   CssClass="DropDownList">
               </asp:DropDownList>
               <asp:Button ID="BtnApply1" runat="server" class="bitbtn btnapply" 
                   Text="Apply" />
           </td>
           <td>
               &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
               <asp:DropDownList ID="ddlmasterplan" runat="server" 
                   CssClass="DropDownList">
               </asp:DropDownList>
               &nbsp;
               <asp:Button ID="BtnApply2" runat="server" class="bitbtn btnapply" 
                   Text="Get Master Plan" Height="21px" Width="109px" />
               
               </td>
       </tr>
     
      </table>
      </asp:Panel>
    <asp:GridView id="DataGrid" runat="server" 
            AutoGenerateColumns="False" AllowPaging="True" CssClass="Grid" 
            ShowFooter = "True" >
						<HeaderStyle CssClass="GridHeader"></HeaderStyle>
						<RowStyle CssClass="GridItem" />
						<AlternatingRowStyle CssClass="GridAltItem"/>
						<FooterStyle CssClass="GridFooter" />
						<PagerStyle CssClass="GridPager" />
				      <Columns>
				      <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:CheckBox ID="cbSelectHd" runat="server" AutoPostBack="true" 
                                    oncheckedchanged="cbSelectHd_CheckedChanged" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="cbSelect" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        
				            <asp:TemplateField HeaderText="Block Code" HeaderStyle-Width="130px"  >
								<Itemtemplate>
									<asp:Label Runat="server" ID="BlockCode" text='<%# DataBinder.Eval(Container.DataItem, "BlockCode") %>'/>
								</Itemtemplate>
								<FooterTemplate>
								<asp:TextBox Runat="server" ID="BlockCodeAdd" MaxLength="12" Width="65%" CssClass="TextBox" 
									Text='<%# DataBinder.Eval(Container.DataItem, "BlockCode") %>'
									ontextchanged="tbBlockCode_TextChanged" AutoPostBack="true"/>
									<cc1:TextBoxWatermarkExtender ID="BlockCodeAdd_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="BlockCodeAdd" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                    </cc1:TextBoxWatermarkExtender>		
                                    <asp:Button class="btngo" runat="server" ID="btnBlockCode"  CommandName ="SearchBlockCodeAdd" Text="..." />
                                 
								</FooterTemplate>													
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Block Name" HeaderStyle-Width="80px" >
								<Itemtemplate>
									<asp:Label Runat="server" ID="BlockName" text='<%# DataBinder.Eval(Container.DataItem, "BlockName") %>'/>
								</Itemtemplate>	
								<FooterTemplate>
									<asp:Label Runat="server" ID="BlockNameAdd" text='<%# DataBinder.Eval(Container.DataItem, "BlockName") %>'/>
								</FooterTemplate>															
							</asp:TemplateField>
								
							<asp:TemplateField HeaderText="Status Tanam" HeaderStyle-Width="100px" >
								<Itemtemplate>
									<asp:Label Runat="server" ID="StatusTanam" text='<%# DataBinder.Eval(Container.DataItem, "StatusTanam") %>' Width="100" Visible="False" />
									<asp:Label Runat="server" ID="StatusTanamName" text='<%# DataBinder.Eval(Container.DataItem, "StatusTanamName") %>' Width="100" />
								</Itemtemplate>	
								<FooterTemplate>
									<asp:Label Runat="server" ID="StatusTanamAdd" text='<%# DataBinder.Eval(Container.DataItem, "StatusTanam") %>' Width="100" Visible="False" />
									<asp:Label Runat="server" ID="StatusTanamNameAdd" text='<%# DataBinder.Eval(Container.DataItem, "StatusTanamName") %>' Width="100" />
								</FooterTemplate>		
							</asp:TemplateField>								
							
							<asp:TemplateField HeaderText="Master Plan" HeaderStyle-Width="150px" >
								<Itemtemplate>
								<div style = "text-align: center;">
								<asp:Label Runat="server" ID="MasterPlan" text='<%# DataBinder.Eval(Container.DataItem, "MasterPlan") %>' Width="100" />									
									<%--<asp:DropDownList ID="MasterPlan" CssClass="DropDownList" Width="100%" runat="server" 
									SelectedValue='<%# DataBinder.Eval(Container.DataItem, "MasterPlan") %>' 
								            DataSourceID="dsGetMasterPlan" DataTextField="Master_Plan_No" 
                                            DataValueField="Master_Plan_No"> 
                                        <asp:ListItem Selected="true">Choose One</asp:ListItem>                                                                   
                                     </asp:DropDownList>	--%>
                                     </Div>
								</Itemtemplate>	
								<FooterTemplate>
									   	<asp:DropDownList ID="MasterPlanAdd" CssClass="DropDownList" Width="100%" runat="server"
								            DataSourceID="dsGetMasterPlan" DataTextField="Master_Plan_No" 
                                            DataValueField="Master_Plan_No">  
                                         <asp:ListItem Selected="True">Choose One</asp:ListItem>                                                                                                   
									</asp:DropDownList>	
									 
								</FooterTemplate>		
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Status" HeaderStyle-Width="10px" >
								<Itemtemplate>
								<div style ="text-align:center;">
									<asp:Label Runat="server" ID="Status" text='<%# DataBinder.Eval(Container.DataItem, "Status") %>' Width="10" />
									</div>
								</Itemtemplate>	
								<FooterTemplate>
									<asp:Label Runat="server" ID="StatusAdd" text='<%# DataBinder.Eval(Container.DataItem, "Status") %>' Width="10" />
									 
								</FooterTemplate>		
							</asp:TemplateField>								
							
							<asp:TemplateField HeaderText="Action" HeaderStyle-Width="580px" >
								<Itemtemplate>
								    <asp:Button class="bitbtn btndetail" runat="server"  ID="btnGetAppr" Text="Get Approval" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" CommandName="GetAppr" Width="90" />							    
								    <asp:Button class="bitbtn btndetail" runat="server"  ID="btnPost" Text="Post" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" CommandName="Post" Width="80" />							    
								    <asp:Button class="bitbtn btndetail" runat="server"  ID="btnUnPost" Text="Un Post" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" CommandName="UnPost" Width="80" />							    
								    <asp:Button class="bitbtn btndetail" runat="server"  ID="btnGenerate" Text="Generate" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" CommandName="Generate" Width="80" />							    
								    <asp:Button class="bitbtn btndelete" runat="server" ID="btnDeleteHd" Text="Delete" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" CommandName="DeleteHd"  OnClientClick="return confirm('Sure to delete this data?');" />		
								    <asp:Button class="bitbtn btndetail" runat="server"  ID="btnSchedule" Text="Schedule" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" CommandName="Schedule" Width="80" />							    
								</Itemtemplate>
								<FooterTemplate>
								<asp:Button ID="btnAddHd" runat="server" class="bitbtn btnadd" Text="Add" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" CommandName="InsertHd" />
								</FooterTemplate>
										
                            <%--    <ItemStyle HorizontalAlign="Right"></ItemStyle>--%>
							</asp:TemplateField>       
					   									
    					</Columns>
        </asp:GridView>              
    </asp:Panel>
    
    
      <asp:Panel ID="pnlHKFactor" runat="server" Visible = "false" Width="1084px">  
     <table>
       <tr>
       <td class="style1">
            <asp:Label ID="lblBlockHK" CssClass="H1" runat="server" Text="" />             
       </td>
       <td class="style1">
           <asp:Label ID="lblStatusTanamHK" runat="server" CssClass="H1" Text="" />
       </td>
            
                <caption>
                    &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                    </caption>
          
       </tr>
       <tr>
            <td>
            <asp:Button class="bitbtn btnback" runat="server" ID="btnBackHK" Text="Back" />
            </td>            
       </tr>
     </table>
	    <div style="border:0px  solid; width:100%; height:100%; overflow:auto;">
            <asp:Panel ID="pnlViewDt" runat="server" Visible="False">
                <table>
                    <tr>
                        <td Width="70%">
                            <asp:Label ID="lblBlockMaterial" runat="server" CssClass="H1" Text=" " />
                            <asp:GridView ID="DataGridHK" runat="server" AllowPaging="True" 
                                AutoGenerateColumns="False" CssClass="Grid" Visible="True">
                                <HeaderStyle CssClass="GridHeader" />
                                <RowStyle CssClass="GridItem" />
                                <AlternatingRowStyle CssClass="GridAltItem" />
                                <PagerStyle CssClass="GridPager" />
                                <Columns>
                                    <asp:TemplateField HeaderText="Job Code" ItemStyle-HorizontalAlign="Left">
                                        <Itemtemplate>
                                            <asp:Label ID="JobPlant" Runat="server" 
                                                text='<%# DataBinder.Eval(Container.DataItem, "JobPlant") %>' />
                                        </Itemtemplate>
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Job Name" SortExpression="JobName">
                                        <ItemTemplate>
                                            <asp:Label ID="JobPlantName" Runat="server" 
                                                text='<%# DataBinder.Eval(Container.DataItem, "JobPlantName") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Job No" SortExpression="JobName">
                                        <ItemTemplate>
                                            <asp:Label ID="JobNo" Runat="server" 
                                                text='<%# DataBinder.Eval(Container.DataItem, "JobNo") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Team" SortExpression="TeamName">
                                        <ItemTemplate>
                                            <asp:Label ID="TeamName" Runat="server" 
                                                text='<%# DataBinder.Eval(Container.DataItem, "TeamName") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Month" SortExpression="Month">
                                        <ItemTemplate>
                                            <asp:Label ID="Month" Runat="server" 
                                                text='<%# DataBinder.Eval(Container.DataItem, "Month") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Qty" SortExpression="Qty">
                                        <ItemTemplate>
                                            <asp:Label ID="Qty" Runat="server" 
                                                text='<%# DataBinder.Eval(Container.DataItem, "Qty") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Unit" SortExpression="Unit">
                                        <ItemTemplate>
                                            <asp:Label ID="Unit" Runat="server" 
                                                text='<%# DataBinder.Eval(Container.DataItem, "Unit") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="WorkDay" SortExpression="WorkDay">
                                        <ItemTemplate>
                                            <asp:Label ID="WorkDay" Runat="server" 
                                                text='<%# DataBinder.Eval(Container.DataItem, "WorkDay") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Capacity" SortExpression="Capacity">
                                        <ItemTemplate>
                                            <asp:Label ID="Capacity" Runat="server" 
                                                text='<%# DataBinder.Eval(Container.DataItem, "Capacity") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Person" SortExpression="Person">
                                        <ItemTemplate>
                                            <asp:Label ID="Person" Runat="server" 
                                                text='<%# DataBinder.Eval(Container.DataItem, "Person") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderStyle-Width="80px" HeaderText="Action">
                                        <Itemtemplate>
                                            <asp:Button ID="btnMaterial" runat="server" class="bitbtn btndetail" 
                                                CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" 
                                                CommandName="Material" Text="Material" Width="80" />
                                        </Itemtemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </td>
                        <td>
                            <br />
                            <asp:Panel ID="pnlView" runat="server">
                                <asp:Label ID="lblJob" runat="server" CssClass="H1" />
                                <br />
                                <asp:GridView ID="DataGridMaterial" runat="server" AllowPaging="false" 
                                    AllowSorting="false" AutoGenerateColumns="False" ShowFooter="True">
                                    <HeaderStyle CssClass="GridHeader" wrap="false" />
                                    <RowStyle CssClass="GridItem" wrap="false" />
                                    <AlternatingRowStyle CssClass="GridAltItem" />
                                    <FooterStyle CssClass="GridFooter" Wrap="false" />
                                    <PagerStyle CssClass="GridPager" />
                                    <Columns>
                                        <asp:TemplateField HeaderStyle-Width="128" HeaderText="Material Code" 
                                            SortExpression="MaterialCode">
                                            <Itemtemplate>
                                                <asp:Label ID="Material" Runat="server" 
                                                    text='<%# DataBinder.Eval(Container.DataItem, "Material") %>'>
                                                </asp:Label>
                                            </Itemtemplate>
                                            <HeaderStyle Width="128px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-Width="250" HeaderText="Material Name" 
                                            SortExpression="MaterialName">
                                            <Itemtemplate>
                                                <asp:Label ID="MaterialName" Runat="server" 
                                                    text='<%# DataBinder.Eval(Container.DataItem, "MaterialName") %>'>
                                            </asp:Label>
                                            </Itemtemplate>
                                            <HeaderStyle Width="250px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-Width="100px" HeaderText="Qty" 
                                            ItemStyle-HorizontalAlign="Right" SortExpression="Qty">
                                            <Itemtemplate>
                                                <asp:Label ID="Qty1" Runat="server" 
                                                    text='<%# Format(DataBinder.Eval(Container.DataItem, "Qty"),"#,##0.00") %>'>
									</asp:Label>
                                            </Itemtemplate>
                                            <HeaderStyle Width="100px" />
                                            <ItemStyle HorizontalAlign="Right" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-Width="100px" HeaderText="Unit" 
                                            ItemStyle-HorizontalAlign="Right" SortExpression="Unit">
                                            <Itemtemplate>
                                                <asp:Label ID="Unit0" Runat="server" 
                                                    text='<%# DataBinder.Eval(Container.DataItem, "Unit") %>'>
									</asp:Label>
                                            </Itemtemplate>
                                            <HeaderStyle Width="100px" />
                                            <ItemStyle HorizontalAlign="Right" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-Width="100" HeaderText="Dosis" 
                                            ItemStyle-HorizontalAlign="Right" SortExpression="Dosis">
                                            <Itemtemplate>
                                                <asp:Label ID="Dosis" Runat="server" 
                                                    text='<%# Format(DataBinder.Eval(Container.DataItem, "Dosis"),"#,##0.00") %>'>
									</asp:Label>
                                            </Itemtemplate>
                                            <HeaderStyle Width="100px" />
                                            <ItemStyle HorizontalAlign="Right" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                <br />
                                <br />
                            </asp:Panel>
                        </td>
                        <caption>
                            &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                        </caption>
                    </tr>
                </table>
                <div style="border:0px  solid; width:100%; height:100%; overflow:auto;">
                </div>
                <asp:Button ID="btnBack2" runat="server" class="bitbtn btnback" Text="Back" />
            </asp:Panel>
            <br />
        </div>        
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
           <asp:SqlDataSource ID="dsGetMasterPlan" runat="server" SelectCommand="SELECT Master_Plan_No, Effective_Date FROm V_PLPlanBlockGetMasterPlan" ></asp:SqlDataSource>  
           <asp:SqlDataSource ID="dsGetStatusTanam" runat="server" SelectCommand="select PlantPeriodCode, PlantPeriodName from MsPlantPeriod" ></asp:SqlDataSource>   
    <br />
    
    
    
    </form>
</body>
</html>
