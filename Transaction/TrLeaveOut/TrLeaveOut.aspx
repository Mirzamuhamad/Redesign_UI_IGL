<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TrLeaveOut.aspx.vb" Inherits="Transaction_TrLeaveOut_TrLeaveOut" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="BasicFrame.WebControls.BasicDatePicker" Namespace="BasicFrame.WebControls"
    TagPrefix="BDP" %>
    
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Untitled Page</title>
    <script src="../../Function/OpenDlg.js" type="text/javascript"></script>
    <script src="../../Function/Function.js" type="text/javascript"></script>
    <script type="text/javascript">  
    function openClosingdlg(_keyid, _prm) {  
                window.open("../../Transaction/TrLeaveOut/FormComplete.Aspx?KeyId="+_keyid+"&ContainerId="+_prm+"Id","List","scrollbars=yes,resizable=no,width=700,height=500");
                return false;
                          
        }        
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
       function setformat()
        {
        try
         {
             var QtyLeave = document.getElementById("tbLeave").value.replace(/\$|\,/g, "");
             document.getElementById("tbLeave").value = setdigit(QtyLeave, '<%=Viewstate("DigitQty")%>');
        }catch (err){
            alert(err.description);
          }      
        }   
    </script>
    
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
 <link href="https://fonts.googleapis.com/css2?family=Muli:wght@300;400;600&display=swap" rel="stylesheet">
    <style type="text/css">
        .style1
        {
        }
        .style6
        {
            width: 90px;
        }
        .style8
        {
            width: 618px;
        }
        .style9
        {
            width: 58px;
        }
        .style10
        {
            width: 3px;
        }
        .style11
        {
            width: 250px;
        }
        .style12
        {
            width: 28px;
        }
        </style>
</head>
<body>
    <form id="form1" runat="server">
    <div class="Content">
    <div class="H1"><asp:Label runat ="server" ID="lbTitle">Employee Leave</asp:Label></div>
     <hr style="color:Blue" />        
    <asp:Panel runat="server" id="PnlHd"> 
      <table>
        <tr>
            <td style="width:100px;text-align:right">Quick Search :</td>         
            <td><asp:TextBox CssClass="TextBox" runat="server" ID ="tbFilter"/> 
                  <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlField" >     
                    <asp:ListItem Selected="True" Value="TransNmbr">Reference</asp:ListItem>
                    <asp:ListItem Value="dbo.FormatDate(TransDate)">Date</asp:ListItem>
                    <asp:ListItem>Status</asp:ListItem>
                    <asp:ListItem Value="Remark">Remark</asp:ListItem>
                    <asp:ListItem Value="Employee_No">Emp No</asp:ListItem>
                    <asp:ListItem Value="Employee_Name">Emp Name</asp:ListItem>
                    <asp:ListItem Value="JobTitleName">Job Title </asp:ListItem>
                    
                    <asp:ListItem Value="Dept_Name">Organization</asp:ListItem>
                   </asp:DropDownList>
                  <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlRange"></asp:DropDownList>
                  <asp:Button class="bitbtn btnsearch" runat="server" ID="btnSearch" Text="Search" />
                  <asp:Button class="btngo" runat="server" ID="btnExpand" Text="..."/>                  
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
                    <asp:ListItem Selected="True" Value="TransNmbr">Reference</asp:ListItem>
                    <asp:ListItem Value="dbo.FormatDate(TransDate)">Date</asp:ListItem>
                    <asp:ListItem>Status</asp:ListItem>
                    <asp:ListItem Value="Remark">Remark</asp:ListItem>
                    <asp:ListItem Value="Employee_No">Emp No</asp:ListItem>
                    <asp:ListItem Value="Employee_Name">Emp Name</asp:ListItem>
                    <asp:ListItem Value="JobTitleName">Job Title </asp:ListItem>
                    
                    <asp:ListItem Value="Dept_Name">Organization</asp:ListItem>
              </asp:DropDownList>
          </td>              
        </tr>        
      </table>      
      </asp:Panel>
          <br />
           <asp:Button class="bitbtn btnadd" runat="server" ID="BtnAdd" Text="Add"/>	 
            &nbsp &nbsp &nbsp   
            <asp:DropDownList CssClass="DropDownList" ID="ddlCommand" runat="server" Visible="false"/>
            <asp:Button class="btngo" runat="server" ID="BtnGo" Text="G" 
            Visible="False"/>
          <br />
          <div style="border:0px  solid; width:100%; height:100%; overflow:auto;">
          <asp:GridView ID="GridView1" runat="server" AllowPaging="True" AllowSorting="true"
            CssClass="Grid" AutoGenerateColumns="false"> 
              <HeaderStyle CssClass="GridHeader" Wrap = "false"></HeaderStyle>
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
                              <asp:ListItem Text="Print" />
                          </asp:DropDownList>
                          <asp:Button class="btngo" runat="server" ID="BtnGo" Text="G" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" CommandName="Go" />
                      </ItemTemplate>
                      <HeaderStyle Width="110px" />
                  </asp:TemplateField>            
                  <asp:BoundField DataField="TransNmbr" HeaderStyle-Width="120px" SortExpression="Nmbr" HeaderText="Reference"></asp:BoundField>                  
                  <asp:BoundField DataField="Status" SortExpression="Status" HeaderText="Status"></asp:BoundField>                  
                  <asp:BoundField DataField="TransDate" dataformatstring="{0:dd MMM yyyy}" htmlencode="true" HeaderStyle-Width="80px" SortExpression="TransDate" HeaderText="Date"></asp:BoundField>
                  <asp:BoundField DataField="Employee_No" SortExpression="Employee_No" HeaderText="Employee No"></asp:BoundField>                  
                  <asp:BoundField DataField="Employee_Name" SortExpression="Employee_Name" HeaderText="Employee Name"></asp:BoundField>                  
                  <asp:BoundField DataField="Remark" HeaderStyle-Width="350px" SortExpression="Remark" HeaderText="Remark"></asp:BoundField>
                  <asp:BoundField DataField="Done_Complete" HeaderStyle-Width="50px" SortExpression="Done_Complete" HeaderText="Complete"></asp:BoundField>
                  <%--<asp:BoundField DataField="Employee_No" HeaderStyle-Width="80px" SortExpression="Employee_No" HeaderText="Employee No"></asp:BoundField>                  
                  <asp:BoundField DataField="Employee_Name" HeaderStyle-Width="80px" SortExpression="Employee_Name" HeaderText="Employee"></asp:BoundField>                  
                  <asp:BoundField DataField="JobTitleName" HeaderStyle-Width="80px" SortExpression="JobTitleName" HeaderText="Job Title"></asp:BoundField>                  
                  <asp:BoundField DataField="SubSectionName" HeaderStyle-Width="80px" SortExpression="SubSectionName" HeaderText="Sub Section"></asp:BoundField>                  
                  <asp:BoundField DataField="Dept_Name" HeaderStyle-Width="80px" SortExpression="Dept_Name" HeaderText="Department"></asp:BoundField>                  --%>
              </Columns>
          </asp:GridView>
          </div>
            <asp:Panel runat="server" ID ="pnlNav" Visible="false">
            <asp:Button class="bitbtn btnadd" runat="server" ID="btnAdd2" Text="Add"/>	
            &nbsp &nbsp &nbsp
            <asp:DropDownList ID="ddlCommand2" CssClass="DropDownList" runat="server"/>
            <asp:Button class="btngo" runat="server" ID="btnGo2" Text="G"/>                
            </asp:Panel>
    </asp:Panel>    
    <asp:Panel runat="server" ID="pnlInput" Visible="false">
      <table>
        <tr>
            <td class="style6">No</td>
            <td>:</td>
            <td><asp:TextBox CssClass="TextBoxR" runat="server" ID="tbCode" Width="150px" Enabled="False"/></td>
            
            <td>Date</td>
            <td>:</td>
            <td><BDP:BasicDatePicker ID="tbDate" runat="server" DateFormat="dd MMM yyyy" 
                        ReadOnly = "true" ValidationGroup="Input"
                        ButtonImageHeight="19px" ButtonImageWidth="20px" 
                        DisplayType="TextBoxAndImage" 
                        TextBoxStyle-CssClass="TextDate" 
                        ShowNoneButton="False"><TextBoxStyle CssClass="TextDate" /></BDP:BasicDatePicker>                
                <asp:Button ID="btnGetData" runat="server" class="bitbtn btngo" 
                    Text="Get Employee" ValidationGroup="Input" Visible="false" Width="74px" />
            </td>            
        </tr>
        <tr>
            <td class="style6">Remark</td>
            <td>:</td>
            <td colspan="4" style="margin-left: 80px">
            <asp:TextBox ID="tbRemark" runat="server" CssClass="TextBoxMulti" ValidationGroup="Input" Width="380px" MaxLength="255" TextMode="MultiLine" />
                <asp:DropDownList ID="ddlLeaveCategory" runat="server" CssClass="DropDownList" 
                    ValidationGroup="Input" Visible="False" Width="160px">
                    <asp:ListItem>Permission</asp:ListItem>
                    <asp:ListItem>Leave</asp:ListItem>
                </asp:DropDownList>
            </td>            
        </tr>
      </table>  
      
      <br />      
      <div style="font-size:medium; color:Blue;">Detail</div>
      <hr style="color:Blue" />  
              <asp:Panel runat="server" ID="PnlDt">
              <asp:Button class="bitbtndt btnadd" runat="server" ID="btnAdddt" Text="Add" ValidationGroup="Input" />
                  <br />
                  <br />
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
                                        <asp:Button ID="btnEdit" runat="server" class="bitbtn btnedit" Text="Edit" CommandName="Edit"/>
                                        <asp:Button ID="btnDelete" runat="server" class="bitbtndt btndelete" Text="Delete" OnClientClick="return confirm('Sure to delete this data?');" CommandName="Delete"/>						                                      
                                      </ItemTemplate>
                            </asp:TemplateField>
                             <asp:TemplateField>
                              <ItemTemplate> 
                               <asp:Button Class="bitbtndt btngetitem"  ID="btnClosing" runat="server" Text="Complete" CommandArgument='<%# Container.DataItemIndex %>' CommandName="Closing" Width = "70px" />
                              </ItemTemplate>
                            </asp:TemplateField> 
                            
                            <asp:TemplateField>
                              <ItemTemplate> 
                               <asp:Button Class="bitbtndt btngetitem"  ID="btnUnClosing" runat="server" Text="Un Complete" CommandArgument='<%# Container.DataItemIndex %>' CommandName="UnClosing" Width = "90px" />
                              </ItemTemplate>
                            </asp:TemplateField> 
                            
                            
                            <asp:BoundField DataField="EmpNumb" HeaderText="Employee No" HeaderStyle-Width="100px" />   
                            <asp:BoundField DataField="EmpName" HeaderText="Employee Name" HeaderStyle-Width="200px" />
                            <asp:BoundField DataField="HireDate" dataformatstring="{0:dd MMM yyyy}" htmlencode="true" HeaderText="Hire Date" HeaderStyle-Width="70px" />
<%--                            <asp:BoundField DataField="SubSectionName" HeaderText="Sub Section" HeaderStyle-Width="150px" />--%>
                            <asp:BoundField DataField="JobTitleName" HeaderText="Job Title" HeaderStyle-Width="150px"/>
                            <asp:BoundField DataField="DepartmentName" HeaderText="Organization" HeaderStyle-Width="150px"/>                            
                             
                        
                            <asp:BoundField DataField="LeaveTypeName" HeaderText="Leave Type" HeaderStyle-Width="150px"/>                            
                            <asp:BoundField DataField="FgLess1Day" HeaderText="Less 1 Day" HeaderStyle-Width="30px"/>                            
                            <asp:BoundField DataField="StartDate" dataformatstring="{0:dd MMM yyyy}" htmlencode="true" HeaderText="Start Date" HeaderStyle-Width="70px"/>
                            <asp:BoundField DataField="StartTime" HeaderText="Start Time" />                            
                            <asp:BoundField DataField="EndDate" dataformatstring="{0:dd MMM yyyy}" htmlencode="true" HeaderText="End Date" HeaderStyle-Width="70px"/>
                            <asp:BoundField DataField="EndTime" HeaderText="End Time" />                            
                            <asp:BoundField DataField="QtyTotal" HeaderStyle-Width="50px" HeaderText="Total (days)" />
                            <asp:BoundField DataField="QtyHoliday" HeaderStyle-Width="50px" HeaderText="Holiday (days)" />
                            <asp:BoundField DataField="QtyDispensasi" HeaderStyle-Width="50px" HeaderText="Dispensasi (days)" />
                            <asp:BoundField DataField="QtyTaken" HeaderStyle-Width="50px" HeaderText="Taken (days)" />
                            <asp:BoundField DataField="ContactAddr" HeaderStyle-Width="250px" HeaderText="Address Contact" />
                            <asp:BoundField DataField="ContactPhone" HeaderStyle-Width="120px" HeaderText="Phone Contact" />
                            <asp:BoundField DataField="DoneComplete" HeaderStyle-Width="50px" HeaderText="Complete" />
                            <asp:BoundField DataField="Actual_Start_Date" dataformatstring="{0:dd MMM yyyy}" htmlencode="true" HeaderText="Complete Start Date" HeaderStyle-Width="70px"/>
                            <asp:BoundField DataField="Actual_Start_Time" HeaderText="Complete Start Time" />                            
                            <asp:BoundField DataField="Actual_End_Date" dataformatstring="{0:dd MMM yyyy}" htmlencode="true" HeaderText="Complete End Date" HeaderStyle-Width="70px"/>
                            <asp:BoundField DataField="Actual_End_Time" HeaderText="Complete End Time" />                            
                            <asp:BoundField DataField="Actual_Qty_Total" HeaderStyle-Width="50px" HeaderText="Complete Total (days)" />
                            <asp:BoundField DataField="Actual_Qty_Holiday" HeaderStyle-Width="50px" HeaderText="Complete Holiday (days)" />
                            <asp:BoundField DataField="Actual_Qty_Dispensasi" HeaderStyle-Width="50px" HeaderText="Complete Dispensasi (days)" />
                            <asp:BoundField DataField="Actual_Qty_Taken" HeaderStyle-Width="50px" HeaderText="Complete Taken (days)" />
                            
                        </Columns>
                    </asp:GridView>
              </div>   
                  <br />
              <asp:Button class="bitbtndt btnadd" runat="server" ID="btnAddDtke2" Text="Add" ValidationGroup="Input" />
              </asp:Panel> 
              <asp:Panel runat="server" ID="pnlEditDt" Visible="false">
                <table class="style8">    
                    <tr>
                        <td class="style9">Employee</td>
                        <td class="style10">:</td>
                        <td colspan="4"><asp:TextBox CssClass="TextBox" ValidationGroup="Input" runat="server" ID="tbEmpNumb" MaxLength="12" Width="100px" 
                                AutoPostBack="True" />
                                <asp:TextBox CssClass="TextBoxR" runat="server" ID="tbEmpName" 
                                MaxLength="60" Width="337px" Enabled="False" />
                        <asp:Button class="btngo" runat="server" ID="btnEmp" Text="..."/>
                        </td>
                    </tr>   
                    <tr>
                        <td class="style9">
                            Hire Date</td>
                        <td class="style10">
                            :</td>
                        <td colspan="4">
                            <BDP:BasicDatePicker ID="tbHireDate" runat="server" ButtonImageHeight="19px" 
                                ButtonImageWidth="20px" DateFormat="dd MMM yyyy" DisplayType="TextBoxAndImage" 
                                ReadOnly="true" ShowNoneButton="False" TextBoxStyle-CssClass="TextDate" 
                                ValidationGroup="Input" Enabled="False">
                                <TextBoxStyle CssClass="TextDate" />
                            </BDP:BasicDatePicker>
                        </td>
                    </tr>
                    <tr>
                        <%--<td class="style9">Sub Section</td>
                        <td class="style10">:</td>
                        <td class="style11">
                            <asp:DropDownList ID="ddlSubSection" runat="server" CssClass="DropDownList" 
                                Enabled="false" Height="16px" ValidationGroup="Input" Width="250px" />
                        </td>--%>
                        <td class="style12">
                            Job Title</td>
                        <td class="style10">
                            :</td>
                        <td class="style11">
                            <asp:DropDownList ID="ddlJobTitle" runat="server" CssClass="DropDownList" 
                                Enabled="false" Height="16px" ValidationGroup="Input" Width="250px" />
                        </td>
                    </tr>       
                    <tr>
                        <td class="style9">
                            Leave Type</td>
                        <td class="style10">
                            :</td>
                        <td class="style11">
                            <asp:DropDownList ID="ddlLeaveType" runat="server" CssClass="DropDownList" 
                                Height="16px" ValidationGroup="Input" Width="250px" AutoPostBack="True" />
                        </td>
                        <td class="style12">
                            Organization </td>
                        <td class="style10">
                            :</td>
                        <td class="style11">
                            <asp:DropDownList ID="ddlDepartment" runat="server" CssClass="DropDownList" 
                                Height="16px" ValidationGroup="Input" Width="250px" Enabled="False" />
                        </td>
                    </tr>
                    <tr>
                        <td class="style1">
                            Date</td>
                        <td class="style10">
                            :</td>
                        <td colspan="4">
                            <table cellpadding="0" cellspacing="0">
                                <tr style="background-color:Silver;text-align:center">
                                    <td>
                                        Less 1 Day</td>
                                    <td>
                                        Start Date</td>
                                    <td>
                                        Start Time</td>
                                    <td>
                                        End Date</td>
                                    <td>
                                        End Time</td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:DropDownList ID="ddlFgLess1Day" runat="server" AutoPostBack="True" 
                                            CssClass="DropDownList" Height="16px" ValidationGroup="Input" Width="50px">
                                            <asp:ListItem>Y</asp:ListItem>
                                            <asp:ListItem Selected="True">N</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <BDP:BasicDatePicker ID="tbStartDate" runat="server" ButtonImageHeight="19px" 
                                            ButtonImageWidth="20px" DateFormat="dd MMM yyyy" DisplayType="TextBoxAndImage" 
                                            ReadOnly="true" ShowNoneButton="False" TextBoxStyle-CssClass="TextDate" 
                                            ValidationGroup="Input">
                                            <TextBoxStyle CssClass="TextDate" />
                                        </BDP:BasicDatePicker>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="tbStartTime" runat="server" AutoPostBack="True" 
                                            CssClass="TextBox" ValidationGroup="Input" Width="83px" />
                                    </td>
                                    <td>
                                        <BDP:BasicDatePicker ID="tbEndDate" runat="server" ButtonImageHeight="19px" 
                                            ButtonImageWidth="20px" DateFormat="dd MMM yyyy" DisplayType="TextBoxAndImage" 
                                            ReadOnly="true" ShowNoneButton="False" TextBoxStyle-CssClass="TextDate" 
                                            ValidationGroup="Input" AutoPostBack="True">
                                            <TextBoxStyle CssClass="TextDate" />
                                        </BDP:BasicDatePicker>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="tbEndTime" runat="server" AutoPostBack="True" 
                                            CssClass="TextBox" ValidationGroup="Input" Width="83px" />
                                        <asp:Label ID="lbFgHoliday" runat="server" Text="Label" Visible="False"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>Qty (days)</td>
                        <td class="style10">:</td>
                        <td colspan="4">
                            <table cellspacing="0" cellpadding="0">
                                <tr style="background-color:Silver;text-align:center">
                                    <td>Total</td>
                                    <td>Holiday</td>
                                    <td>
                                        Dispensasi</td>
                                    <td>Taken</td>
                                </tr>
                            <tr>
                                <td><asp:TextBox ID="tbTotal" ValidationGroup="Input" runat="server" Width="80px" 
                                        Enabled="false"  CssClass="TextBoxR"/></td>
                                <td><asp:TextBox ID="tbHoliday" ValidationGroup="Input" runat="server" Width="83px" 
                                        CssClass="TextBoxR" Enabled="False"/></td>
                                <td style="font-weight: 700">
                                    <asp:TextBox ID="tbDispensasi" runat="server" CssClass="TextBoxR" 
                                        Enabled="false" ValidationGroup="Input" Width="80px" />
                                </td>
                                <td><asp:TextBox ID="tbTaken" ValidationGroup="Input" runat="server" 
                                        Enabled="false" Width="80px" CssClass="TextBoxR"/></td>        
                            </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="style9">Contact Address</td>
                        <td class="style10">:</td>
                        <td colspan="4" class="style1" style="margin-left: 40px">
                            <asp:TextBox ID="tbaddr" runat="server" CssClass="TextBoxMulti" MaxLength="255" 
                                ValidationGroup="Input" Width="360px"/>
                        </td>                        
                    </tr>                    
                    <tr>
                        <td class="style9">
                            Contact Phone</td>
                        <td class="style10">
                            :</td>
                        <td class="style1" colspan="4" style="margin-left: 40px">
                            <asp:TextBox ID="tbPhone" runat="server" CssClass="TextBox" MaxLength="50" 
                                ValidationGroup="Input" Width="200px" />
                        </td>
                    </tr>
                    <tr>
                        <td class="style9">
                            Reason</td>
                        <td class="style10">
                            :</td>
                        <td class="style1" colspan="4" style="margin-left: 40px">
                            <asp:TextBox ID="tbReason" runat="server" CssClass="TextBoxMulti" MaxLength="255" 
                                ValidationGroup="Input" Width="360px" />
                                <asp:TextBox ID="tbDoneComplete" runat="server" CssClass="TextBox" MaxLength="5" 
                                ValidationGroup="Input" Visible="False" Width="53px" />
                        </td>
                    </tr>
                    
                </table>
                <br />           
                <asp:Button ID="btnSaveDt" runat="server" class="bitbtndt btnsave" Text="Save"/> &nbsp;         
                <asp:Button ID="btnCancelDt" runat="server" class="bitbtndt btncancel" Text="Cancel"/> 
           </asp:Panel> 
       <br />      
        <asp:Button ID="btnSaveAll" runat="server" class="bitbtndt btnsave" Text="Save & New" ValidationGroup="Input" Width="97px"/> &nbsp;    
        <asp:Button ID="btnSaveTrans" runat="server" class="bitbtndt btnsave" Text="Save" ValidationGroup="Input"/> &nbsp;    
        <asp:Button ID="btnBack" runat="server" class="bitbtndt btncancel" Text="Cancel" ValidationGroup="Input"/>  &nbsp;
        <asp:Button ID="btnHome" runat="server" class="btngo" Text="Home" Width="48px"/>    
        <br />
    </asp:Panel>        
    </div>
   
    <asp:Label runat ="server" ID="lbStatus" ForeColor="Red"/>
    </form>
</body>
</html>
