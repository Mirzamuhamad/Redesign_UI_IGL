<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TrMTNProgress.aspx.vb" Inherits="Transaction_TrMTNProgress_TrMTNProgress" %>

<%@ Register Assembly="BasicFrame.WebControls.BasicDatePicker" Namespace="BasicFrame.WebControls"
    TagPrefix="BDP" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
    <script src="../../Function/OpenDlg.js" type="text/javascript"></script>
    <script src="../../Function/Function.JS" type="text/javascript"></script>
    <script type="text/javascript">         
        function setdigit(nStr, digit)
        {
        try
        {
        var TNstr = parseFloat(nStr);        
        if ( parseFloat(digit) >= 0) 
        {     
           TNstr = TNstr.toFixed(digit);                
        } 
        nStr = TNstr;        
	    nStr += '';
	    x = nStr.split('.');
	    x1 = x[0];
	    x2 = x.length > 1 ? '.' + x[1] : '';
	    var rgx = /(\d+)(\d{3})/;
	    while (rgx.test(x1)) {
		    x1 = x1.replace(rgx, '$1' + ',' + '$2');
	    }
	    return x1 + x2;
	    }catch (err){
            alert(err.description);
          }  
        }
       
        function setformatdt()
        {
         try
         {  
            document.getElementById("tbQtyDt3").value = setdigit(document.getElementById("tbQtyDt3").value.replace(/\$|\,/g,""),'<%=VIEWSTATE("DigitQty")%>');
                                                            
            
        }catch (err){
            alert(err.description);
          }      
        }   
        
   
    </script>
    
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div class="Content">
    <div class="H1">Maintenance Progress Report</div>
     <hr style="color:Blue" />        
    <asp:Panel runat="server" id="PnlHd"> 
      <table>
        <tr>
            <td style="width:100px;text-align:right">Quick Search :</td>         
            <%--TransNmbr, TransDate, STATUS, FgReport, UserType, UserCode, UserName, Attn, Remark--%>   
            <td><asp:TextBox CssClass="TextBox" runat="server" ID ="tbFilter"/> 
                  <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlField" >                      
                    <asp:ListItem Selected="True" Value="TransNmbr">Report No</asp:ListItem>
                    <asp:ListItem Value="dbo.FormatDate(TransDate)">Report Date</asp:ListItem>
                    <asp:ListItem>Status</asp:ListItem>
                    <asp:ListItem Value="MONo">MO No</asp:ListItem>
                    <asp:ListItem Value="Machine">Machine Code</asp:ListItem>
                    <asp:ListItem Value="Machine_Name">Machine Name</asp:ListItem>                                        
                    <asp:ListItem Value="Practitioner">Practitioner</asp:ListItem>                    
                    <asp:ListItem Value="ProjectManager">ProjectManager Code</asp:ListItem>
                    <asp:ListItem Value="ProjectManager_Name">ProjectManager Name</asp:ListItem>
                    <asp:ListItem Value="ReviewedBy">ReviewedBy Code</asp:ListItem>
                    <asp:ListItem Value="ReviewedBy_Name">ReviewedBy Name</asp:ListItem>
                    <asp:ListItem Value="AcknowledgeBy1">AcknowledgeBy 1 Code</asp:ListItem>
                    <asp:ListItem Value="AcknowledgeBy1_Name">AcknowledgeBy 1 Name</asp:ListItem>
                    <asp:ListItem Value="AcknowledgeBy2">AcknowledgeBy 2 Code</asp:ListItem>
                    <asp:ListItem Value="AcknowledgeBy2_Name">AcknowledgeBy 2 Name</asp:ListItem>
                    <asp:ListItem Value="Remark">Remark</asp:ListItem>
                    <asp:ListItem Value="UserPrep">User Prep</asp:ListItem>
                    <asp:ListItem Value="DatePrep">Date Prep</asp:ListItem>
                    <asp:ListItem Value="UserAppr">User Appr</asp:ListItem>
                    <asp:ListItem Value="DateAppr">Date Appr</asp:ListItem>
                  </asp:DropDownList>
                  <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlRange"></asp:DropDownList>                  
                  <asp:Button class="bitbtn btnsearch" runat="server" ID="btnSearch" Text="Search" />																						 											  
                  <asp:Button class="bitbtn btngo" runat="server" ID="btnExpand" Text="..." />																						 											  
            </td>
            <td>
                <asp:LinkButton ID="LbAdvSearch" runat="server" Text="Advanced Search" />
            </td>
        </tr>
      </table>
      <asp:Panel runat="server" ID="pnlSearch" Visible="false">
      <table>
        <tr>
          <td style="width:100px;text-align:right">
              <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlNotasi" >
                <asp:ListItem Selected="true" Text="OR" Value="OR"></asp:ListItem>
                <asp:ListItem Text="AND" Value="AND"></asp:ListItem>            
              </asp:DropDownList>
          </td>
          <td>
              <asp:TextBox runat="server" CssClass="TextBox" ID ="tbfilter2"/> 
              <asp:DropDownList runat="server" CssClass="DropDownList" ID="ddlField2" >                     
                    <asp:ListItem Selected="True" Value="TransNmbr">Report No</asp:ListItem>
                    <asp:ListItem Value="dbo.FormatDate(TransDate)">Report Date</asp:ListItem>
                    <asp:ListItem>Status</asp:ListItem>
                    <asp:ListItem Value="MONo">MO No</asp:ListItem>
                    <asp:ListItem Value="Machine">Machine Code</asp:ListItem>
                    <asp:ListItem Value="Machine_Name">Machine Name</asp:ListItem>                                        
                    <asp:ListItem Value="Practitioner">Practitioner</asp:ListItem>                    
                    <asp:ListItem Value="ProjectManager">ProjectManager Code</asp:ListItem>
                    <asp:ListItem Value="ProjectManager_Name">ProjectManager Name</asp:ListItem>
                    <asp:ListItem Value="ReviewedBy">ReviewedBy Code</asp:ListItem>
                    <asp:ListItem Value="ReviewedBy_Name">ReviewedBy Name</asp:ListItem>
                    <asp:ListItem Value="AcknowledgeBy1">AcknowledgeBy 1 Code</asp:ListItem>
                    <asp:ListItem Value="AcknowledgeBy1_Name">AcknowledgeBy 1 Name</asp:ListItem>
                    <asp:ListItem Value="AcknowledgeBy2">AcknowledgeBy 2 Code</asp:ListItem>
                    <asp:ListItem Value="AcknowledgeBy2_Name">AcknowledgeBy 2 Name</asp:ListItem>
                    <asp:ListItem Value="Remark">Remark</asp:ListItem>
                    <asp:ListItem Value="UserPrep">User Prep</asp:ListItem>
                    <asp:ListItem Value="DatePrep">Date Prep</asp:ListItem>
                    <asp:ListItem Value="UserAppr">User Appr</asp:ListItem>
                    <asp:ListItem Value="DateAppr">Date Appr</asp:ListItem>                              
              </asp:DropDownList>
          </td>              
        </tr>        
      </table>      
      </asp:Panel>
          <br />
            <asp:Button class="bitbtn btnadd" runat="server" ID="BtnAdd" Text="Add" />	           
            &nbsp &nbsp &nbsp   
            <asp:DropDownList CssClass="DropDownList" ID="ddlCommand" runat="server" Visible="false"/>
            <asp:Button class="btngo" runat="server" ID="BtnGo" Text="G" Visible="false" />                         
          <br />
          <div style="border:0px  solid; width:100%; height:100%; overflow:auto;">
          <asp:GridView ID="GridView1" runat="server" AllowPaging="True" AllowSorting="true"
            CssClass="Grid" AutoGenerateColumns="false"> 
              <HeaderStyle CssClass="GridHeader"></HeaderStyle>
						<RowStyle CssClass="GridItem" Wrap="false" />
						<AlternatingRowStyle CssClass="GridAltItem"/>
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
                  <asp:TemplateField HeaderStyle-Width="110">
                      <ItemTemplate>
                          <asp:DropDownList CssClass="DropDownList" ID="ddl" runat="server">
                              <asp:ListItem Selected="True" Text="View" />
                              <asp:ListItem Text="Edit" />
                              <%--<asp:ListItem Text="Print" />--%>
                              
                          </asp:DropDownList>
                          <asp:Button class="btngo" runat="server" ID="BtnGo" Text="G" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" CommandName="Go" />                
                       </ItemTemplate>
                      <HeaderStyle Width="110px" />
                  </asp:TemplateField>
                  <asp:BoundField DataField="TransNmbr" HeaderStyle-Width="120px" SortExpression="Nmbr" HeaderText="Report No"></asp:BoundField>                  
                  <asp:BoundField DataField="Status" HeaderText="Status"></asp:BoundField>                  
                  <asp:BoundField DataField="TransDate" dataformatstring="{0:dd MMM yyyy}" htmlencode="true" HeaderStyle-Width="80px" SortExpression="TransDate" HeaderText="Report Date"></asp:BoundField>
                  <asp:BoundField DataField="MONo" HeaderStyle-Width="80px" SortExpression="MONo" HeaderText="MO No"></asp:BoundField>
                  <asp:BoundField DataField="Machine" HeaderStyle-Width="80px" SortExpression="Machine" HeaderText="Machine Code"></asp:BoundField>
                  <asp:BoundField DataField="Machine_Name" HeaderStyle-Width="200px" SortExpression="Machine_Name" HeaderText="Machine Name"></asp:BoundField>
                  <asp:BoundField DataField="Practitioner" HeaderStyle-Width="80px" SortExpression="Practitioner" HeaderText="Practitioner"></asp:BoundField>
                  <asp:BoundField DataField="ProjectManager" HeaderStyle-Width="200px" SortExpression="ProjectManager" HeaderText="Project Manager"></asp:BoundField>
                  <asp:BoundField DataField="ProjectManager_Name" HeaderStyle-Width="80px" SortExpression="ProjectManager_Name" HeaderText="Project Manager Name"></asp:BoundField>
                  <asp:BoundField DataField="ReviewedBy" HeaderStyle-Width="250px" SortExpression="ReviewedBy" HeaderText="ReviewedBy"></asp:BoundField>
                  <asp:BoundField DataField="ReviewedBy_Name" HeaderStyle-Width="80px" SortExpression="ReviewedBy_Name" HeaderText="ReviewedBy Name"></asp:BoundField>
                  <asp:BoundField DataField="AcknowledgeBy1" HeaderStyle-Width="250px" SortExpression="AcknowledgeBy1" HeaderText="AcknowledgeBy 1"></asp:BoundField>
                  <asp:BoundField DataField="AcknowledgeBy1_Name" HeaderStyle-Width="80px" SortExpression="AcknowledgeBy1_Name" HeaderText="AcknowledgeBy 1 Name"></asp:BoundField>
                  <asp:BoundField DataField="AcknowledgeBy2" HeaderStyle-Width="250px" SortExpression="AcknowledgeBy2" HeaderText="AcknowledgeBy 2"></asp:BoundField>
                  <asp:BoundField DataField="AcknowledgeBy2_Name" HeaderStyle-Width="80px" SortExpression="AcknowledgeBy2_Name" HeaderText="AcknowledgeBy 2 Name"></asp:BoundField>
                  <asp:BoundField DataField="Remark" HeaderStyle-Width="250px" SortExpression="Remark" HeaderText="Remark"></asp:BoundField>                  
              </Columns>
          </asp:GridView>
          </div>
            <asp:Panel runat="server" ID ="pnlNav" Visible="false">
            <asp:Button class="bitbtn btnadd" runat="server" ID="btnAdd2" Text="Add" />	  
            &nbsp &nbsp &nbsp
            <asp:DropDownList ID="ddlCommand2" CssClass="DropDownList" runat="server"/>
            <asp:Button class="btngo" runat="server" ID="btnGo2" Text="G" />                 
            </asp:Panel>
    </asp:Panel>    
    <asp:Panel runat="server" ID="pnlInput" Visible="false">
      <table>
        <tr>
            <td>Report No</td>
            <td>:</td>
            <td><asp:TextBox CssClass="TextBoxR" runat="server" ID="tbCode" Width="150px" Enabled="False"/></td>
        
            <td>Report Date</td>
            <td>:</td>
            <td>
            <BDP:BasicDatePicker ID="tbDate" runat="server" DateFormat="dd MMM yyyy" 
                        ReadOnly = "true" ValidationGroup="Input"
                        ButtonImageHeight="19px" ButtonImageWidth="20px" 
                        DisplayType="TextBoxAndImage" TextBoxStyle-CssClass="TextDate" 
                        ShowNoneButton="False"><TextBoxStyle CssClass="TextDate" /></BDP:BasicDatePicker>                
            </td>            
        </tr> 
        <tr>
            <td>MO No</td>
            <td>:</td>
            <td colspan="4"><asp:TextBox CssClass="TextBox" Enabled ="false" runat="server" ID="tbMONo" MaxLength="20" Width="150px" />
                <asp:Button Class="btngo" ID="btnMONo" Text="..." runat="server" ValidationGroup="Input" />
                <asp:Label ID="Label12" runat="server" ForeColor="#FF3300" Text="*"></asp:Label>
            </td>               
        </tr>
        <tr>
            <td>Machine</td>
            <td>:</td>
            <td colspan="4">
                <asp:TextBox CssClass="TextBox" Id="tbMachineCode" runat="server" Enabled="false" />
                <asp:TextBox CssClass="TextBox" ID="tbMachineName" runat="server" MaxLength="60" Width="225px" Enabled="False" /> 
                <%--<asp:Button ID="btnMachine" runat="server" class="btngo" Text="..."/> --%>
            </td>   
        </tr>    
        <tr>
            <td>Practitioner</td>
            <td>:</td>
            <td colspan="4"><asp:DropDownList Enabled="false"  CssClass="DropDownList" ID="ddlPractitioner" runat="server" >
                        <asp:ListItem Selected="True">Vendor</asp:ListItem>
                        <asp:ListItem>Internal</asp:ListItem>
                    </asp:DropDownList> 
            </td>    
        </tr>
        
        <tr>
            <td>Project Manager</td>
            <td>:</td>
            <td colspan="4">
                <asp:TextBox CssClass="TextBox" runat="server" ID="tbProjectManager" Enabled="false"  MaxLength="12" />
                <asp:TextBox CssClass="TextBox" runat="server" ID="tbProjectManagerName" Enabled="false" MaxLength="60" Width="225px"/>
                <%--<asp:Button Class="btngo" ID="btnProjectManager" Text="..." runat="server" ValidationGroup="Input" />--%>
            </td>
        </tr>
        
        <tr>
            <td>Reviewed By</td>
            <td>:</td>
            <td colspan="4">
                <asp:TextBox CssClass="TextBox" ValidationGroup="Input" runat="server" ID="tbReviewedBy" MaxLength="12" AutoPostBack="true" />
                <asp:TextBox CssClass="TextBox" runat="server" ID="tbReviewedByName" Enabled="false" MaxLength="60" Width="225px"/>
                <asp:Button Class="btngo" ID="btnReviewedBy" Text="..." runat="server" ValidationGroup="Input" />
                <asp:Label ID="Label13" runat="server" ForeColor="#FF3300" Text="*"></asp:Label>
            </td>
        </tr>  
        
        <tr>
            <td>Acknowledge By 1</td>
            <td>:</td>
            <td colspan="4">
                <asp:TextBox CssClass="TextBox" ValidationGroup="Input" runat="server" ID="tbAcknowledgeBy1" MaxLength="12" AutoPostBack="true" />
                <asp:TextBox CssClass="TextBox" runat="server" ID="tbAcknowledgeBy1Name" Enabled="false" MaxLength="60" Width="225px"/>
                <asp:Button Class="btngo" ID="btnAcknowledgeBy1" Text="..." runat="server" ValidationGroup="Input" />
                <asp:Label ID="Label11" runat="server" ForeColor="#FF3300" Text="*"></asp:Label>
            </td>
        </tr>   
        
        <tr>
            <td>Acknowledge By 2</td>
            <td>:</td>
            <td colspan="4">
                <asp:TextBox CssClass="TextBox" ValidationGroup="Input" runat="server" ID="tbAcknowledgeBy2" MaxLength="12" AutoPostBack="true" />
                <asp:TextBox CssClass="TextBox" runat="server" ID="tbAcknowledgeBy2Name" Enabled="false" MaxLength="60" Width="225px"/>
                <asp:Button Class="btngo" ID="btnAcknowledgeBy2" Text="..." runat="server" ValidationGroup="Input" />
                <asp:Label ID="Label10" runat="server" ForeColor="#FF3300" Text="*"></asp:Label>
            </td>
        </tr>
        
        <tr>
              <td>Remark</td>
              <td>:</td>
              <td colspan="4"><asp:TextBox ID="tbRemark" runat="server" CssClass="TextBox" ValidationGroup="Input" Width="269px" /></td>
          </tr>
        
      </table>  
      
      <br />      
      <hr style="color:Blue" />  
       <asp:Menu
            ID="Menu1"
            runat="server"
            CssClass = "Menu"        
            StaticMenuItemStyle-CssClass = "MenuItem"
            StaticSelectedStyle-CssClass = "MenuSelect"        
            Orientation="Horizontal"
            ItemWrap = "False"
            StaticEnableDefaultPopOutImage="False">            
            <Items>
                <asp:MenuItem Text="Detail Maintenance Item & Job" Value="0"></asp:MenuItem>
                <asp:MenuItem Text="Detail PIC" Value="1"></asp:MenuItem>
                <asp:MenuItem Text="Detail Use Up Material / Job" Value="2"></asp:MenuItem>
                <%--<asp:MenuItem Text="Detail Image Progress Report" Value="2"></asp:MenuItem>--%>
            </Items>            
        </asp:Menu>
        <br />
        <asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex="0">
           <asp:View ID="Tab1" runat="server">
              <asp:Panel runat="server" ID="PnlDt">
                <asp:Button class="bitbtn btnadd" runat="server" ID="btnAdddt" Text="Add" Visible="false" ValidationGroup="Input" />	
                <div style="border:0px  solid; width:100%; height:100%; overflow:auto;">
                    <asp:GridView ID="GridDt" runat="server" AutoGenerateColumns="false" 
                        ShowFooter="True">
                        <HeaderStyle CssClass="GridHeader" />
                        <RowStyle CssClass="GridItem" Wrap="false" />
                        <AlternatingRowStyle CssClass="GridAltItem" />
                        <PagerStyle CssClass="GridPager" />
                        <Columns>
                            <asp:TemplateField HeaderText="Action">
                                      <ItemTemplate>
   							          <asp:Button ID="btnEdit" runat="server" class="bitbtndt btnedit" Text="Edit" CommandName="Edit"/>
								      <asp:Button ID="btnDelete" runat="server" class="bitbtndt btndelete" Text="Delete" OnClientClick="return confirm('Sure to delete this data?');" CommandName="Delete"/>									
                                      </ItemTemplate>
                            </asp:TemplateField>
                            
                            <asp:BoundField DataField="MaintenanceItem" HeaderStyle-Width="100px" HeaderText="Maintenance Item" />
                            <asp:BoundField DataField="MaintenanceItem_Name" HeaderStyle-Width="150px" HeaderText="Maintenance Item Name" />
                            <asp:BoundField DataField="ItemSpec" HeaderStyle-Width="100px" HeaderText="Merk / Spec" />
                            <asp:BoundField DataField="ItemQty" HeaderStyle-Width="100px" HeaderText="Qty Item" />
                            <%--<asp:BoundField DataField="Explanation" HeaderStyle-Width="100px" HeaderText="Explanation" />--%>
                            <asp:BoundField DataField="Job" HeaderStyle-Width="100px" HeaderText="Job" />
                            <asp:BoundField DataField="Job_Name" HeaderStyle-Width="100px" HeaderText="Job Name" />                               
                            <asp:BoundField DataField="JobDescription" HeaderStyle-Width="100px" HeaderText="Work Description" />   
                            <asp:BoundField DataField="Priority" HeaderStyle-Width="100px" HeaderText="Priority" />   
                            <asp:BoundField DataField="StartDate" dataformatstring="{0:dd MMM yyyy}" HeaderStyle-Width="80px" HeaderText="Start Date" />
                            <asp:BoundField DataField="StartTime" HeaderStyle-Width="70px" HeaderText="Start Time" />   
                            <asp:BoundField DataField="EndDate" dataformatstring="{0:dd MMM yyyy}" HeaderStyle-Width="80px" HeaderText="End Date" />
                            <asp:BoundField DataField="EndTime" HeaderStyle-Width="70px" HeaderText="End Time" />   
                            <asp:BoundField DataField="Progress" HeaderStyle-Width="100px" HeaderText="Progress (%)" />   
                            <asp:BoundField DataField="Remark" HeaderStyle-Width="100px" HeaderText="Remark" />   
                        </Columns>
                    </asp:GridView>
              </div>   
              <asp:Button class="bitbtn btnadd" runat="server" ID="btnAddDtke2" Text="Add" Visible="false" ValidationGroup="Input" />	
              </asp:Panel> 
              <asp:Panel runat="server" ID="pnlEditDt" Visible="false">
                <table> 
                    
                    <tr>
                        <td>Maintenance Item</td>
                        <td>:</td>
                        <td colspan="4">
                            <asp:TextBox CssClass="TextBox" Id="tbMaintenanceItem" runat="server" AutoPostBack="True"/>
                            <asp:TextBox CssClass="TextBox" ID="tbMaintenanceItemName" runat="server" MaxLength="60" Width="225px" Enabled="False" /> 
                            <asp:Button ID="btnMaintenanceItem" runat="server" class="btngo" Text="..."/> 
                            <asp:Label ID="Label9" runat="server" ForeColor="#FF3300" Text="*"></asp:Label>
                         </td>   
                    </tr>   
                      
                    <tr>
                        <td>Merk Spec</td>
                        <td>:</td>
                        <td colspan="4"><asp:TextBox ID="tbItemSpec" runat="server" CssClass="TextBox" Enabled="False" Width="269px" /></td>
                     </tr> 
                     <tr>
                        <td>Qty Item</td>
                        <td>:</td>
                        <td colspan="4"><asp:TextBox ID="tbItemQty" runat="server" CssClass="TextBox" Enabled="False" Width="269px" /></td>
                     </tr>     
                     <%--<tr>
                        <td>Explanation</td>
                        <td>:</td>
                        <td colspan="4"><asp:TextBox ID="tbExplanation" runat="server" CssClass="TextBox" Enabled="False" Width="269px" /></td>
                     </tr>--%>
                     <tr>
                        <td>Job</td>
                        <td>:</td>
                        <td colspan="4">
                            <asp:TextBox CssClass="TextBox" Id="tbJob" runat="server" Enabled="False" />
                            <asp:TextBox CssClass="TextBox" ID="tbJobName" runat="server" MaxLength="60" Width="225px" Enabled="False" />                             
                         </td>   
                     </tr>  
                     <tr>
                        <td>Work Description</td>
                        <td>:</td>
                        <td colspan="4"><asp:TextBox ID="tbJobDescription" runat="server" CssClass="TextBox" Enabled="False" Width="269px" /></td>
                     </tr>   
                     <tr>
                        <td>Priority</td>
                        <td>:</td>
                        <td><asp:DropDownList ValidationGroup="Input" CssClass="DropDownList" ID="ddlPriority" runat="server" >
                            <asp:ListItem Selected="True" Value="">Choose One</asp:ListItem>
                            <asp:ListItem>High</asp:ListItem>
                            <asp:ListItem>Medium</asp:ListItem>
                            <asp:ListItem>Low</asp:ListItem>
                            </asp:DropDownList> 
                        </td>  
                        <td>Progress (%)</td>
                        <td>:</td>
                        <td><asp:TextBox ID="tbProgress" runat="server" ValidationGroup="Input" CssClass="TextBox" Width="40" MaxLength="5" /></td>    
                    </tr>
                    <tr>
                        <td>Start Date</td>
                        <td>:</td>
                        <td>
                            <BDP:BasicDatePicker ID="tbStartDate" runat="server" DateFormat="dd MMM yyyy" 
                        ReadOnly = "true" ValidationGroup="Input"
                        ButtonImageHeight="19px" ButtonImageWidth="20px" 
                        DisplayType="TextBoxAndImage" TextBoxStyle-CssClass="TextDate" 
                        ShowNoneButton="False"><TextBoxStyle CssClass="TextDate" /></BDP:BasicDatePicker>  
                        <asp:Label ID="Label7" runat="server" ForeColor="#FF3300" Text="*"></asp:Label>              
                        </td>   
                        <td>Start Time</td>
                        <td>:</td>
                        <td><asp:TextBox ID="tbStartTime" runat="server" ValidationGroup="Input" CssClass="TextBox" Width="40" MaxLength="5" />
                        <asp:Label ID="Label8" runat="server" ForeColor="#FF3300" Text="*"></asp:Label>
                        </td>
                    </tr>
                    
                    <tr>
                        <td>End Date</td>
                        <td>:</td>
                        <td>
                            <BDP:BasicDatePicker ID="tbEndDate" runat="server" DateFormat="dd MMM yyyy" 
                        ReadOnly = "true" ValidationGroup="Input"
                        ButtonImageHeight="19px" ButtonImageWidth="20px" 
                        DisplayType="TextBoxAndImage" TextBoxStyle-CssClass="TextDate" 
                        ShowNoneButton="False"><TextBoxStyle CssClass="TextDate" /></BDP:BasicDatePicker>   
                        <asp:Label ID="Label5" runat="server" ForeColor="#FF3300" Text="*"></asp:Label>             
                        </td> 
                        <td>Start Time</td>
                        <td>:</td>
                        <td><asp:TextBox ID="tbEndTime" runat="server" ValidationGroup="Input" CssClass="TextBox" Width="40" MaxLength="5" />
                        <asp:Label ID="Label6" runat="server" ForeColor="#FF3300" Text="*"></asp:Label>
                        </td>  
                    </tr>
                   
                    <tr>
                        <td>Remark</td>
                        <td>:</td>
                        <td colspan="4"><asp:TextBox ID="tbRemarkDt" runat="server" CssClass="TextBox" ValidationGroup="Input" Width="269px" /></td>
                    </tr>
                </table>
                <br />                     
                <asp:Button ID="btnSaveDt" runat="server" class="bitbtndt btnsave" Text="Save" />									
                <asp:Button ID="btnCancelDt" runat="server" class="bitbtndt btncancel" Text="Cancel" />									
           </asp:Panel> 
              
           </asp:View>           
            <asp:View ID="Tab2" runat="server">
                <asp:Panel ID="pnlDt2" runat="server">  
                <asp:Button class="bitbtn btnadd" runat="server" ID="btnAddDt2" Text="Add" ValidationGroup="Input" />	              
                
                <div style="border:0px  solid; width:100%; height:100%; overflow:auto;">
                    <asp:GridView ID="GridDt2" runat="server" AutoGenerateColumns="False" 
                        ShowFooter="True">
                        <HeaderStyle CssClass="GridHeader" />
                        <RowStyle CssClass="GridItem" Wrap="false" />
                        <AlternatingRowStyle CssClass="GridAltItem" />
                        <PagerStyle CssClass="GridPager" />
                        <Columns>
                            <asp:TemplateField HeaderText="Action" >
                                <ItemTemplate>
   							       <asp:Button ID="btnEdit" runat="server" class="bitbtndt btnedit" Text="Edit" CommandName="Edit"/>
								   <asp:Button ID="btnDelete" runat="server" class="bitbtndt btndelete" Text="Delete" OnClientClick="return confirm('Sure to delete this data?');" CommandName="Delete"/>									
                                </ItemTemplate>
                                <EditItemTemplate>
                               	    <asp:Button ID="btnUpdate" runat="server" class="bitbtndt btnsave" Text="Save" CommandName="Update"/>									
                                    <asp:Button ID="btnCancel" runat="server" class="bitbtndt btncancel" Text="Cancel" CommandName="Cancel"/>									
                                </EditItemTemplate>
                            </asp:TemplateField>                                      
                            
                            <asp:BoundField DataField="PIC" HeaderStyle-Width="100px" HeaderText="PIC Code"  />
                            <asp:BoundField DataField="PICName" HeaderStyle-Width="150px" HeaderText="PIC Name" />
                            <asp:BoundField DataField="Remark" HeaderStyle-Width="200px" HeaderText="Remark" />                            
                                                       
                        </Columns>
                    </asp:GridView>
              </div>    
              <asp:Button class="bitbtn btnadd" runat="server" ID="btnAddDt2Ke2" Text="Add" ValidationGroup="Input" />	              
   
              </asp:Panel>
              <asp:Panel runat="server" ID="pnlEditDt2" Visible="false">
                <table>              
                     <tr>
                        <td>PIC</td>
                        <td>:</td>
                        <td colspan="4">
                            <asp:TextBox CssClass="TextBox" ValidationGroup="Input" runat="server" ID="tbPICCode" MaxLength="12" AutoPostBack="true" />
                            <asp:TextBox CssClass="TextBox" runat="server" ID="tbPICName" Enabled="false" MaxLength="60" Width="225px"/>
                            <asp:Button Class="btngo" ID="btnPIC" Text="..." runat="server" ValidationGroup="Input" />
                            <asp:Label ID="Label4" runat="server" ForeColor="#FF3300" Text="*"></asp:Label>
                        </td>
                    </tr>                        
                   <tr>
                        <td>Remark</td>
                        <td>:</td>
                        <td colspan="4"><asp:TextBox ID="tbRemarkDt2" runat="server" CssClass="TextBox" ValidationGroup="Input" Width="269px" /></td>
                    </tr>
                    
                                           
                </table>
                <br />                     
                <asp:Button ID="btnSaveDt2" runat="server" class="bitbtndt btnsave" Text="Save" />									
                <asp:Button ID="btnCancelDt2" runat="server" class="bitbtndt btncancel" Text="Cancel" />									


           </asp:Panel> 
               
            </asp:View>    
            <asp:View ID="Tab3" runat="server">
                <asp:Panel ID="pnlDt3" runat="server">  
                <asp:Button class="bitbtn btnadd" runat="server" ID="btnAddDt3" Text="Add" ValidationGroup="Input" />	              
                
                <div style="border:0px  solid; width:100%; height:100%; overflow:auto;">
                    <asp:GridView ID="GridDt3" runat="server" AutoGenerateColumns="False" 
                        ShowFooter="True">
                        <HeaderStyle CssClass="GridHeader" />
                        <RowStyle CssClass="GridItem" Wrap="false" />
                        <AlternatingRowStyle CssClass="GridAltItem" />
                        <PagerStyle CssClass="GridPager" />
                        <Columns>
                            <asp:TemplateField HeaderText="Action" >
                                <ItemTemplate>
   							       <asp:Button ID="btnEdit" runat="server" class="bitbtndt btnedit" Text="Edit" CommandName="Edit"/>
								   <asp:Button ID="btnDelete" runat="server" class="bitbtndt btndelete" Text="Delete" OnClientClick="return confirm('Sure to delete this data?');" CommandName="Delete"/>									
                                </ItemTemplate>
                                <EditItemTemplate>
                               	    <asp:Button ID="btnUpdate" runat="server" class="bitbtndt btnsave" Text="Save" CommandName="Update"/>									
                                    <asp:Button ID="btnCancel" runat="server" class="bitbtndt btncancel" Text="Cancel" CommandName="Cancel"/>									
                                </EditItemTemplate>
                            </asp:TemplateField>                                      
                            
                            <asp:BoundField DataField="MaintenanceItem" HeaderStyle-Width="150px" HeaderText="MaintenanceItem" />
                            <asp:BoundField DataField="MaintenanceItem_Name" HeaderStyle-Width="80px" HeaderText="MaintenanceItem Name" />
                            <asp:BoundField DataField="Material" HeaderStyle-Width="80px" HeaderText="Material" />
                            <asp:BoundField DataField="Material_Name" HeaderStyle-Width="80px" HeaderText="Material Name" />
                            <asp:BoundField DataField="Qty" HeaderStyle-Width="120px" HeaderText="Qty" />
                            <asp:BoundField DataField="Unit" HeaderText="Unit" />
                            <asp:BoundField DataField="Remark" HeaderStyle-Width="80px" HeaderText="Remark" />                            
                                                       
                        </Columns>
                    </asp:GridView>
              </div>    
              <asp:Button class="bitbtn btnadd" runat="server" ID="btnAddDt3ke2" Text="Add" ValidationGroup="Input" />	              
   
              </asp:Panel>
              <asp:Panel runat="server" ID="pnlEditDt3" Visible="false">
                <table> 
                    <tr>
                        <td>Maintenance Item</td>
                        <td>:</td>
                        <td colspan="4">
                            <asp:DropDownList ID="ddlMaintenanceItem" runat="server" CssClass="DropDownList" Width="310px" AutoPostBack="true" />
                            <asp:Label ID="Label3" runat="server" ForeColor="#FF3300" Text="*"></asp:Label>
                        </td>
                    </tr>        
                    <tr>
                        <td>Material</td>
                        <td>:</td>
                        <td colspan="4">
                            <asp:TextBox CssClass="TextBox" ValidationGroup="Input" runat="server" ID="tbMaterial" MaxLength="12" AutoPostBack="true" />
                            <asp:TextBox CssClass="TextBox" runat="server" ID="tbMaterialName" Enabled="false" MaxLength="60" Width="225px"/>
                            <asp:Button Class="btngo" ID="btnMaterial" Text="..." runat="server" ValidationGroup="Input" />
                            <asp:Label ID="Label1" runat="server" ForeColor="#FF3300" Text="*"></asp:Label>
                        </td>
                    </tr>        
                     <tr>
                         <td>Qty</td>
                        <td>:</td>
                        <td><asp:TextBox CssClass="TextBox" runat="server" ID="tbQtyDt3" ValidationGroup="Input" MaxLength="20" Width="50px"/>
                            <asp:Label ID="Label2" runat="server" ForeColor="#FF3300" Text="*"></asp:Label>  
                        </td>   
                        <td>Unit</td>
                        <td>:</td>
                        <td><asp:DropDownList CssClass="DropDownList" ID="ddlUnit" Enabled="false" runat="server"/></td>   
                        
                    </tr>     
                     <tr>
                        <td>Remark</td>
                        <td>:</td>
                        <td colspan="4"><asp:TextBox ID="tbRemarkDt3" runat="server" CssClass="TextBox" ValidationGroup="Input" Width="269px" /></td>
                    </tr>    
                             
                </table>
                <br />                     
                <asp:Button ID="btnSaveDt3" runat="server" class="bitbtndt btnsave" Text="Save" />									
                <asp:Button ID="btnCancelDt3" runat="server" class="bitbtndt btncancel" Text="Cancel" />									

           </asp:Panel> 
               
            </asp:View>        
        </asp:MultiView>
    
       <br />          
		<asp:Button ID="btnSaveAll" runat="server" class="bitbtndt btnsavenew" Text="Save & New" validationgroup="Input" Width = "90"/>									
        <asp:Button ID="btnSaveTrans" runat="server" class="bitbtndt btnsave" Text="Save" validationgroup="Input"/>									
        <asp:Button ID="btnBack" runat="server" class="bitbtndt btncancel" Text="Cancel" validationgroup="Input"/>									
        <asp:Button ID="btnHome" runat="server" class="bitbtndt btnback" Text="Home" />									                                                           
    </asp:Panel>        
    </div>
   
    <asp:Label runat ="server" ID="lbStatus" ForeColor="Red"/>
    </form>
</body>
</html>
