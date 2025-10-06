<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TrWOPriority.Aspx.vb" Inherits="Transaction_TrWOPriority_TrWOPriority" %>

<%@ Register Assembly="BasicFrame.WebControls.BasicDatePicker" Namespace="BasicFrame.WebControls"
    TagPrefix="BDP" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">


<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>WO Priority</title>
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
 <link href="https://fonts.googleapis.com/css2?family=Muli:wght@300;400;600&display=swap" rel="stylesheet">   
      <script src="../../Function/OpenDlg.js" type="text/javascript"></script>

    <script src="../../Function/Function.JS" type="text/javascript"></script>
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
        
        function openreport2()
            {
                var wOpen;                
                wOpen = window.open("../../Rpt/PrintForm2.Aspx", "List","scrollbars=yes,resizable=yes,width=500,height=400");            
                wOpen.moveTo(0, 0);
                wOpen.resizeTo(screen.width, screen.height);                        
                return false; 
            }   

        function OpenPopup() {
            window.open("../../SearchDlg.Aspx", "List", "scrollbars=yes,resizable=no,width=500,height=400");
            return false;
        }    
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="Content">
    <div class="H1"><asp:Label ID="lbltitle" runat="server" Text="Label"></asp:Label> </div>
    <hr style="color:Blue" />    
    
    <table>
        <tr>
            <td style="width:100px;text-align:right" >Quick Search :</td>
            <td><asp:TextBox runat="server" ID ="tbFilter" CssClass="TextBox"/> 
                <asp:DropDownList runat="server" ID="ddlField" CssClass="DropDownList" >
                  <asp:ListItem Selected="true" Text="WoNo" Value="WO_No"></asp:ListItem>
                  <asp:ListItem Text="Type" Value="Type"></asp:ListItem>
                  <asp:ListItem Text="Job" Value="Job_Plant"></asp:ListItem>                            
                  <asp:ListItem Text="JobName" Value="JobPlanName"></asp:ListItem>                                           
                  <asp:ListItem Text="StartWeek" Value="Start_Week"></asp:ListItem>                            
                  <asp:ListItem Text="EndWeek" Value="End_Week"></asp:ListItem>                                           
                  <asp:ListItem Text="Team" Value="Team_Code"></asp:ListItem>
                  <asp:ListItem Text="Person" Value="Person"></asp:ListItem>                            
                  <asp:ListItem Text="Capacity" Value="Capacity"></asp:ListItem>                                             
                  <asp:ListItem Text="Qty" Value="Qty"></asp:ListItem>                                             
                  <asp:ListItem Text="Unit" Value="Unit"></asp:ListItem>                                                                         
                </asp:DropDownList>     
                <asp:Button class="bitbtn btnsearch" runat="server" ID="btnSearch" Text="Search" />
                <asp:Button ID="btnExpand" runat="server" class="btngo" Text="..."/>
               <%-- <asp:Button class="bitbtn btnexcel" runat="server" ID="btnExport" Text="Export"/>        --%>
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
                 <asp:ListItem Selected="true" Text="WoNo" Value="WO_No"></asp:ListItem>
                  <asp:ListItem Text="Type" Value="Type"></asp:ListItem>
                  <asp:ListItem Text="Job" Value="Job_Plant"></asp:ListItem>                            
                  <asp:ListItem Text="JobName" Value="JobPlanName"></asp:ListItem>                                           
                  <asp:ListItem Text="StartWeek" Value="Start_Week"></asp:ListItem>                            
                  <asp:ListItem Text="EndWeek" Value="End_Week"></asp:ListItem>                                           
                  <asp:ListItem Text="Team" Value="Team_Code"></asp:ListItem>
                  <asp:ListItem Text="Person" Value="Person"></asp:ListItem>                            
                  <asp:ListItem Text="Capacity" Value="Capacity"></asp:ListItem>                                             
                  <asp:ListItem Text="Qty" Value="Qty"></asp:ListItem>                                             
                  <asp:ListItem Text="Unit" Value="Unit"></asp:ListItem>                                             
                  
                </asp:DropDownList>
            </td>
        </tr>
     </table>
     </asp:Panel>
      <br />
      
    <table>
        <tr>
            <td>Division</td>
            <td>:</td>
            <td><asp:DropDownList runat="server" ID="ddlDivisi" CssClass="DropDownList" 
                    AutoPostBack="True"> </asp:DropDownList> </td>
             <td> Week Priority</td>
             <td> :</td>
             <td> 
                 <asp:TextBox runat="server" ID ="tbWeek" 
                    CssClass="TextBox"/> 
                <asp:Button ID="btnweek" runat="server" class="btngo" Text="..."/>
               </td>
             <td>
            
            
            </td>
        </tr>
       
    </table> 
    <br />
     <asp:Menu ID="Menu1" runat="server" CssClass="Menu" ItemWrap="True" Orientation="Horizontal"
            StaticEnableDefaultPopOutImage="False" StaticMenuItemStyle-CssClass="MenuItem"
            StaticSelectedStyle-CssClass="MenuSelect">
            <StaticSelectedStyle CssClass="MenuSelect" />
            <StaticMenuItemStyle CssClass="MenuItem" />
            <Items>
                <asp:MenuItem Text="WO Priority" Value="0"></asp:MenuItem>
                <asp:MenuItem Text="WO Bottom" Value="1"></asp:MenuItem>
            </Items>
        </asp:Menu>
        <br />
        <asp:MultiView ID="MV1" runat="server" ActiveViewIndex="0">
        
         <asp:View ID="Tab1" runat="server">
            <asp:Panel ID="PnlTop" runat="server" Visible="false">
     <%--<asp:Button class="bitbtn btnUnapply" runat="server" ID="BtnRemove" Text="Remove" width="85"/>--%>
    
                <asp:Button ID="btnPrint" runat="server" class="bitbtn btnsearch" 
                    Text="Print" />
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Button ID="btnPrintTop" runat="server" class="bitbtn btnsearch" 
                    Text="Print WO" />
                <br />
    
    <asp:GridView id="DataGrid" runat="server" 
    ShowFooter="True" AllowSorting="True" 
            AutoGenerateColumns="False" AllowPaging="True"  CssClass="Grid">
						<HeaderStyle CssClass="GridHeader"></HeaderStyle>
						<RowStyle CssClass="GridItem" />
						<AlternatingRowStyle CssClass="GridAltItem"/>
						<PagerStyle CssClass="GridPager" />
				      <Columns>
				            <asp:TemplateField HeaderText="WONO" >
								<Itemtemplate>
									<asp:Label Runat="server" HeaderStyle-Width="110px" ID="WONO" text='<%# DataBinder.Eval(Container.DataItem, "WONO") %>'/>
								</Itemtemplate>														
							</asp:TemplateField>
							
				            <asp:TemplateField HeaderText="WODate" >
								<Itemtemplate>
									<asp:Label Runat="server" ID="WODate"  HeaderStyle-Width="110px" text='<%# Format(DataBinder.Eval(Container.DataItem, "WODate"),"dd MMM yyyy") %>'/>
								</Itemtemplate>														
							</asp:TemplateField>
							
							<%--<asp:BoundField DataField="WODate" DataFormatString="{0:dd MMM yyyy}" HeaderStyle-Width="80px" SortExpression="WODate" HeaderText="WO Date">
                            <HeaderStyle Width="80px" /> </asp:BoundField>--%>
							
							<asp:TemplateField HeaderText="StartWeek" >
								<Itemtemplate>
									<asp:Label Runat="server" ID="StartWeek" text='<%# DataBinder.Eval(Container.DataItem, "StartWeek") %>'/>
								</Itemtemplate>														
							</asp:TemplateField>
				            <asp:TemplateField HeaderText="Team" >
								<Itemtemplate>
									<asp:Label Runat="server" ID="Team" text='<%# DataBinder.Eval(Container.DataItem, "Team") %>'/>
									<asp:Label Runat="server" ID="TeamName" text='<%# DataBinder.Eval(Container.DataItem, "TeamName") %>'/>
								</Itemtemplate>														
							</asp:TemplateField>
				            <asp:TemplateField HeaderText="Job " >
								<Itemtemplate>
									<asp:Label Runat="server" ID="JobPlant" text='<%# DataBinder.Eval(Container.DataItem, "JobPlant") %>'/>
                                    <asp:Label Runat="server" ID="JobPlantName" text='<%# DataBinder.Eval(Container.DataItem, "JobPlantName") %>'/>									
								</Itemtemplate>														
							</asp:TemplateField>
							
				           	 <asp:TemplateField HeaderText="Qty" >
								<Itemtemplate>
									<asp:Label Runat="server" ID="Qty"  text='<%# Format (DataBinder.Eval(Container.DataItem, "Qty"),"#,##0.00") %>'/>
								</Itemtemplate>														
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Unit" >
								<Itemtemplate>
									<asp:Label runat="server" ID="Unit" text='<%# DataBinder.Eval(Container.DataItem, "Unit")%>' />
								</Itemtemplate>														
							</asp:TemplateField>
														
						 <asp:TemplateField HeaderText="WorkDay" >
								<Itemtemplate>
									<asp:Label Runat="server" ID="WorkDay" text='<%# Format(DataBinder.Eval(Container.DataItem, "WorkDay"),"#,##0.00")  %>'/>
								</Itemtemplate>														
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="NormaHK" >
								<Itemtemplate>
									<asp:Label runat="server" ID="NormaHK" text='<%# Format (DataBinder.Eval(Container.DataItem, "NormaHK"),"#,##0.00") %>' />
								</Itemtemplate>														
							</asp:TemplateField>
							
							
							<asp:TemplateField HeaderText="Person" >
								<Itemtemplate>
									<asp:Label runat="server" ID="Person" text='<%# DataBinder.Eval(Container.DataItem, "Person")%>' />
								</Itemtemplate>														
							</asp:TemplateField>
							
						    <asp:TemplateField HeaderText="Priority" >
								<Itemtemplate>
									<asp:Label Runat="server" ID="FgNewPriority"  text='<%# DataBinder.Eval(Container.DataItem, "FgNewPriority") %>'/>
								</Itemtemplate>														
							</asp:TemplateField>	
												
    					</Columns>
        </asp:GridView>                
     
       </asp:Panel>  
        </asp:View>
        <asp:View ID="Tab2" runat="server">
       <asp:Panel ID="PnlBottom" runat="server" Visible="false">
       <asp:Button class="bitbtn btnapply" runat="server" ID="btnAdd" Text="Top Priority" 
               width="95px"/>
    
           &nbsp;<asp:GridView ID="DataGrid1" runat="server" AllowPaging="True" 
               AllowSorting="True" AutoGenerateColumns="False" CssClass="Grid" 
               ShowFooter="True">
               <HeaderStyle CssClass="GridHeader" />
               <RowStyle CssClass="GridItem" />
               <AlternatingRowStyle CssClass="GridAltItem" />
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
                   <asp:TemplateField HeaderText="WONO">
                       <Itemtemplate>
                           <asp:Label ID="WONO" HeaderStyle-Width="100px" Runat="server" 
                               text='<%# DataBinder.Eval(Container.DataItem, "WONO") %>' />
                       </Itemtemplate>
                   </asp:TemplateField>
                   <asp:TemplateField HeaderText="WODate">
                       <Itemtemplate>
                           <asp:Label ID="WODate" Runat="server" 
                              HeaderStyle-Width="110px" 
                               text='<%# Format(DataBinder.Eval(Container.DataItem, "WODate"),"dd MMM yyyy")  %>' />
                       </Itemtemplate>
                   </asp:TemplateField>
                   <asp:TemplateField HeaderText="Team">
                       <Itemtemplate>
                           <asp:Label ID="Team" Runat="server" 
                               text='<%# DataBinder.Eval(Container.DataItem, "Team") %>' />
                           <asp:Label ID="TeamName" Runat="server" 
                               text='<%# DataBinder.Eval(Container.DataItem, "TeamName") %>' />
                       </Itemtemplate>
                   </asp:TemplateField>
                   <asp:TemplateField HeaderText="StartWeek">
                       <Itemtemplate>
                           <asp:Label ID="StartWeek" Runat="server" 
                               text='<%# DataBinder.Eval(Container.DataItem, "StartWeek") %>' />
                       </Itemtemplate>
                   </asp:TemplateField>
                   <asp:TemplateField HeaderText="EndWeek">
                       <Itemtemplate>
                           <asp:Label ID="EndWeek" Runat="server" 
                               text='<%# DataBinder.Eval(Container.DataItem, "EndWeek") %>' />
                       </Itemtemplate>
                   </asp:TemplateField>
                   <asp:TemplateField HeaderText="Job ">
                       <Itemtemplate>
                           <asp:Label ID="JobPlant" Runat="server" 
                               text='<%# DataBinder.Eval(Container.DataItem, "JobPlant") %>' />
                           <asp:Label ID="JobPlantName" Runat="server" 
                               text='<%# DataBinder.Eval(Container.DataItem, "JobPlantName") %>' />
                       </Itemtemplate>
                   </asp:TemplateField>
                   <asp:TemplateField HeaderText="Qty">
                       <Itemtemplate>
                           <asp:Label ID="Qty" Runat="server" 
                               text='<%# Format (DataBinder.Eval(Container.DataItem, "Qty"),"#,##0.00") %>' />
                       </Itemtemplate>
                   </asp:TemplateField>
                   <asp:TemplateField HeaderText="Unit">
                       <Itemtemplate>
                           <asp:Label ID="Unit" runat="server" 
                               text='<%# DataBinder.Eval(Container.DataItem, "Unit")%>' />
                       </Itemtemplate>
                   </asp:TemplateField>
                   <asp:TemplateField HeaderText="WorkDay">
                       <Itemtemplate>
                           <asp:Label ID="WorkDay" Runat="server" 
                               text='<%# Format(DataBinder.Eval(Container.DataItem, "WorkDay"),"#,##0.00") %>' />
                       </Itemtemplate>
                   </asp:TemplateField>
                   <asp:TemplateField HeaderText="NormaHK">
                       <Itemtemplate>
                           <asp:Label ID="NormaHK" runat="server" 
                               text='<%# Format(DataBinder.Eval(Container.DataItem, "NormaHK"),"#,##0.00") %>' />
                       </Itemtemplate>
                   </asp:TemplateField>
                   <asp:TemplateField HeaderText="Person">
                       <Itemtemplate>
                           <asp:Label ID="Person" runat="server" 
                               text='<%# DataBinder.Eval(Container.DataItem, "Person")%>' />
                       </Itemtemplate>
                   </asp:TemplateField>
                   <asp:TemplateField HeaderText="TopPriority">
                       <Itemtemplate>
                           <asp:Label ID="TopPriority" Runat="server" 
                               text='<%# DataBinder.Eval(Container.DataItem, "TopPriority") %>' />
                       </Itemtemplate>
                   </asp:TemplateField>
                   <asp:TemplateField HeaderText="StatusWrhs">
                       <Itemtemplate>
                           <asp:Label ID="StatusWrhs" Runat="server" 
                               text='<%# DataBinder.Eval(Container.DataItem, "StatusWrhs") %>' />
                       </Itemtemplate>
                   </asp:TemplateField>
                   <asp:TemplateField HeaderText="StatusManager">
                       <Itemtemplate>
                           <asp:Label ID="StatusManager" Runat="server" 
                               text='<%# DataBinder.Eval(Container.DataItem, "StatusManager") %>' />
                       </Itemtemplate>
                   </asp:TemplateField>
                   <asp:TemplateField HeaderText="StatusAsisten">
                       <Itemtemplate>
                           <asp:Label ID="StatusAsisten" Runat="server" 
                               text='<%# DataBinder.Eval(Container.DataItem, "StatusAsisten") %>' />
                       </Itemtemplate>
                   </asp:TemplateField>
                   <asp:TemplateField HeaderText="Remark">
                       <Itemtemplate>
                           <asp:Label ID="Remark" Runat="server" 
                               text='<%# DataBinder.Eval(Container.DataItem, "Remark") %>' />
                       </Itemtemplate>
                   </asp:TemplateField>
               </Columns>
           </asp:GridView>
     
       </asp:Panel>
       </asp:View>
        </asp:MultiView>
    </div>
    <asp:GridView ID="GridExport" runat="server" AutoGenerateColumns="True" 
                Visible="true" ShowFooter="False" GridLines="None" Height="79px" PageSize="20" Width="724px">
               <HeaderStyle CssClass="GridHeader" />
               <RowStyle CssClass="GridItem" Wrap="false" />
               <AlternatingRowStyle CssClass="GridAltItem" />
               <PagerStyle CssClass="GridPager" />
           </asp:GridView>     
    <br />
    <asp:Label ID="lbstatus" ForeColor="red" runat="server"></asp:Label>
    </form>
</body>
</html>
