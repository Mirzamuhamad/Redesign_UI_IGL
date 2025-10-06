<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TrEmpOverTime.aspx.vb" Inherits="Transaction_TrEmpOverTime_TrEmpOverTime" %>
<%--<%@ Page EnableEventValidation="true" %>--%>
<%@ Register Assembly="BasicFrame.WebControls.BasicDatePicker" Namespace="BasicFrame.WebControls" TagPrefix="BDP" %>
    
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
    <script src="../../Function/OpenDlg.js" type="text/javascript"></script>
    <script src="../../Function/Function.js" type="text/javascript"></script>
    <script type="text/javascript">  
        function openClosingdlg(_keyid, _prm) {  
                window.open("../../Transaction/TrEmpOverTime/FormComplete.Aspx?KeyId="+_keyid+"&ContainerId="+_prm+"Id","List","scrollbars=yes,resizable=no,width=700,height=500");
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
//         var EstCost = document.getElementById("tbEstCost").value.replace(/\$|\,/g,"");                           
//         document.getElementById("tbEstCost").value = setdigit(EstCost,'<%=Viewstate("DigitCurr")%>');
        }catch (err){
            alert(err.description);
          }      
        }
        
        function closing()
        {
            try
            {
                var result = prompt("Catatan Dokter", "", "Start Hour", "" );
                //var result2 = prompt("Start Hour", "");
                if (result){
                    document.getElementById("HiddenRemarkClose").value = result;
                    //document.getElementById("HiddenStartHour").value = result2;
                    
                } else {
                    document.getElementById("HiddenRemarkClose").value = "False Value";
                    //document.getElementById("HiddenStartHour").value = "False Value";
                }
                postback();
                //document.form1.submit();                
            }catch(err){
                alert(err.description);
            }        
        }
        
        function postback()
        {
            __doPostBack('','');
        }      
    </script>
    
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
 <link href="https://fonts.googleapis.com/css2?family=Muli:wght@300;400;600&display=swap" rel="stylesheet">
    <style type="text/css">
        .style1
        {
            height: 22px;
        }
    </style>
    </head>
<body>
    <form id="form1" runat="server">
    <div class="Content">
    <div class="H1">Employee Overtime</div>
     <hr style="color:Blue" />        
    <asp:Panel runat="server" id="PnlHd"> 
      <table>
        <tr>
            <td style="width:100px;text-align:right">Quick Search :</td>         
            <td><asp:TextBox CssClass="TextBox" runat="server" ID ="tbFilter"/> 
                  <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlField" >     
                    <asp:ListItem Selected="True" Value="TransNmbr">Overtime No</asp:ListItem>
                    <asp:ListItem Value="dbo.FormatDate(TransDate)">Overtime Date</asp:ListItem>
                    <asp:ListItem>Status</asp:ListItem>
                    <asp:ListItem Value="DeptName">Organization</asp:ListItem>                    
                    <asp:ListItem Value="DayType">Day Type</asp:ListItem>                    
                    <asp:ListItem Value="FgSusulan">Susulan</asp:ListItem>                    
                    <asp:ListItem Value="dbo.FormatDate(StartDate)">Start Date</asp:ListItem>
                    <asp:ListItem Value="StartTime">Start Time</asp:ListItem>
                    <asp:ListItem Value="dbo.FormatDate(EndDate)">End Date</asp:ListItem>
                    <asp:ListItem Value="EndTime">End Time</asp:ListItem>
                    <asp:ListItem Value="MinuteBruto">Minute Bruto</asp:ListItem>
                    <asp:ListItem Value="MinuteBreak">Minute Break</asp:ListItem>
                    <asp:ListItem Value="MinuteNetto">Minute Netto</asp:ListItem>
                    <asp:ListItem Value="FgMealAllowance">Meal Allowance</asp:ListItem>
                    <asp:ListItem Value="AcknowBy">Acknowledge By</asp:ListItem>
                    <asp:ListItem Value="AcknowByName">Acknowledge By Name</asp:ListItem>
                    <asp:ListItem Value="AcknowJbt">Acknowledge Job Title</asp:ListItem>
                    <asp:ListItem Value="ApprBy">Approval By</asp:ListItem>
                    <asp:ListItem Value="ApprByName">Approval By Name</asp:ListItem>
                    <asp:ListItem Value="ApprJbt">Approval Job Title</asp:ListItem>
                    <asp:ListItem Value="Remark">Remark</asp:ListItem>                    
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
                    <asp:ListItem Selected="True" Value="TransNmbr">Overtime No</asp:ListItem>
                    <asp:ListItem Value="dbo.FormatDate(TransDate)">Overtime Date</asp:ListItem>
                    <asp:ListItem>Status</asp:ListItem>
                    <asp:ListItem Value="DeptName">Organization</asp:ListItem>                    
                    <asp:ListItem Value="DayType">Day Type</asp:ListItem>                    
                    <asp:ListItem Value="FgSusulan">Susulan</asp:ListItem>                    
                    <asp:ListItem Value="dbo.FormatDate(StartDate)">Start Date</asp:ListItem>
                    <asp:ListItem Value="StartTime">Start Time</asp:ListItem>
                    <asp:ListItem Value="dbo.FormatDate(EndDate)">End Date</asp:ListItem>
                    <asp:ListItem Value="EndTime">End Time</asp:ListItem>
                    <asp:ListItem Value="MinuteBruto">Minute Bruto</asp:ListItem>
                    <asp:ListItem Value="MinuteBreak">Minute Break</asp:ListItem>
                    <asp:ListItem Value="MinuteNetto">Minute Netto</asp:ListItem>
                    <asp:ListItem Value="FgMealAllowance">Meal Allowance</asp:ListItem>
                    <asp:ListItem Value="AcknowBy">Acknowledge By</asp:ListItem>
                    <asp:ListItem Value="AcknowByName">Acknowledge By Name</asp:ListItem>
                    <asp:ListItem Value="AcknowJbt">Acknowledge Job Title</asp:ListItem>
                    <asp:ListItem Value="ApprBy">Approval By</asp:ListItem>
                    <asp:ListItem Value="ApprByName">Approval By Name</asp:ListItem>
                    <asp:ListItem Value="ApprJbt">Approval Job Title</asp:ListItem>
                    <asp:ListItem Value="Remark">Remark</asp:ListItem>                                       
              </asp:DropDownList>
          </td>              
        </tr>        
      </table>      
      </asp:Panel>
          <br />
           <asp:Button class="bitbtn btnadd" runat="server" ID="BtnAdd" Text="Add"/>	 
            &nbsp &nbsp &nbsp   
            <asp:DropDownList CssClass="DropDownList" ID="ddlCommand" runat="server" Visible="false"/>
            <asp:Button class="btngo" runat="server" ID="BtnGo" Text="G"/>
          <br />
          <div style="border:0px  solid; width:100%; height:100%; overflow:auto;">
              <asp:GridView ID="GridView1" runat="server" AllowPaging="True" 
                  AllowSorting="true" AutoGenerateColumns="false" CssClass="Grid">
                  <HeaderStyle CssClass="GridHeader" Wrap="false" />
                  <RowStyle CssClass="GridItem" Wrap="false" />
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
                      <asp:TemplateField HeaderStyle-Width="110">
                          <ItemTemplate>
                              <asp:DropDownList ID="ddl" runat="server" CssClass="DropDownList">
                                  <asp:ListItem Selected="True" Text="View" />
                                  <asp:ListItem Text="Edit" />
                                  <asp:ListItem Text="Print" />
                                  <asp:ListItem Text="Complete" />
                              </asp:DropDownList>
                              <asp:Button ID="BtnGo" runat="server" class="btngo" 
                                  CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" CommandName="Go" 
                                  Text="G" />
                          </ItemTemplate>
                          <HeaderStyle Width="110px" />
                      </asp:TemplateField>
                      <asp:BoundField DataField="TransNmbr" HeaderStyle-Width="120px" HeaderText="Overtime No" SortExpression="Nmbr" />
                      <asp:BoundField DataField="Status" HeaderText="Status" SortExpression="Status" />
                      <asp:BoundField DataField="TransDate" dataformatstring="{0:dd MMM yyyy}" HeaderStyle-Width="80px" HeaderText="Overtime Date" htmlencode="true" SortExpression="TransDate" />
                      <asp:BoundField DataField="DeptName" HeaderStyle-Width="200px" HeaderText="Organization" SortExpression="DeptName" />
                      <asp:BoundField DataField="DayType" HeaderStyle-Width="180px" HeaderText="Day Type" SortExpression="DayType" />
                      <asp:BoundField DataField="FgSusulan" HeaderStyle-Width="80px" HeaderText="Susulan" SortExpression="FgSusulan" />
                      <asp:BoundField DataField="StartDate" dataformatstring="{0:dd MMM yyyy}" HeaderStyle-Width="80px" HeaderText="Start Date" htmlencode="true" SortExpression="StartDate" />
                      <asp:BoundField DataField="StartTime" HeaderStyle-Width="80px" HeaderText="Start Time" SortExpression="StartTime" />
                      <asp:BoundField DataField="EndDate" dataformatstring="{0:dd MMM yyyy}" HeaderStyle-Width="80px" HeaderText="End Date" htmlencode="true" SortExpression="EndDate" />
                      <asp:BoundField DataField="EndTime" HeaderStyle-Width="80px" HeaderText="End Time" SortExpression="EndTime" />
                      <asp:BoundField DataField="MinuteBruto" HeaderStyle-Width="80px" HeaderText="Minute Bruto" SortExpression="MinuteBruto" />
                      <asp:BoundField DataField="MinuteBreak" HeaderStyle-Width="80px" HeaderText="Minute Break" SortExpression="MinuteBreak" />
                      <asp:BoundField DataField="MinuteNetto" HeaderStyle-Width="80px" HeaderText="Minute Netto" SortExpression="MinuteNetto" />
                      <asp:BoundField DataField="FgMealAllowance" HeaderStyle-Width="80px" HeaderText="Meal Allowance" SortExpression="FgMealAllowance" />
                      <asp:BoundField DataField="AcknowBy" HeaderStyle-Width="100px" HeaderText="Acknowledge By" SortExpression="AcknowBy" />
                      <asp:BoundField DataField="AcknowByName" HeaderStyle-Width="200px" HeaderText="Acknowledge By Name" SortExpression="AcknowByName" />
                      <asp:BoundField DataField="AcknowJbtName" HeaderStyle-Width="100px" HeaderText="Acknowledge Job Title" SortExpression="AcknowJbtName" />
                      <asp:BoundField DataField="ApprBy" HeaderStyle-Width="100px" HeaderText="Approval By" SortExpression="ApprBy" />
                      <asp:BoundField DataField="ApprByName" HeaderStyle-Width="200px" HeaderText="Approval By Name" SortExpression="ApprByName" />
                      <asp:BoundField DataField="ApprJbtName" HeaderStyle-Width="100px" HeaderText="Approval Job Title" SortExpression="ApprJbtName" />
                      <asp:BoundField DataField="Remark" HeaderStyle-Width="250px" HeaderText="Remark" SortExpression="Remark" />
                     
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
      <table style="width: 816px">
        <tr>
            <td>Overtime No</td>
            <td>:</td>
            <td><asp:TextBox CssClass="TextBoxR" runat="server" ID="tbCode" Width="150px" Enabled="False"/>
            </td>
            
            <td>Overtime Date</td>
            <td>:</td>
            <td><BDP:BasicDatePicker ID="tbDate" runat="server" DateFormat="dd MMM yyyy" 
                        ReadOnly = "true" ValidationGroup="Input"
                        ButtonImageHeight="19px" ButtonImageWidth="20px" 
                        DisplayType="TextBoxAndImage" 
                        TextBoxStyle-CssClass="TextDate" 
                        ShowNoneButton="False"><TextBoxStyle CssClass="TextDate" /></BDP:BasicDatePicker>                
            </td>            
        </tr>         
          <tr>
              <td>Organization</td>
              <td>:</td>
              <td>
                  <asp:DropDownList ID="ddlDepartment" runat="server" CssClass="DropDownList" ValidationGroup="Input" Width="180px">
                  </asp:DropDownList>
                  <asp:Label ID="Label1" runat="server" Text="*" ForeColor = "Red"></asp:Label>
              </td>
              <td>Day Type</td>
              <td>:</td>
              <td>
                  <asp:DropDownList ID="ddlDayType" runat="server" CssClass="DropDownList" 
                      Height="17px" ValidationGroup="Input" Width="91px">
                      <asp:ListItem>Work</asp:ListItem>
                      <asp:ListItem>Holiday</asp:ListItem>
                  </asp:DropDownList>
                  <asp:Label ID="Label2" runat="server" ForeColor="Red" Text="*"></asp:Label>
              </td>
              
          </tr>
          <tr>
              <td>Start Date & Time</td>
              <td>:</td>
              <td>
                  <BDP:BasicDatePicker ID="tbStartDate" runat="server" ButtonImageHeight="19px" 
                      ButtonImageWidth="20px" DateFormat="dd MMM yyyy" DisplayType="TextBoxAndImage" 
                      ReadOnly="true" ShowNoneButton="False" TextBoxStyle-CssClass="TextDate" 
                      ValidationGroup="Input" AutoPostBack="True"><TextBoxStyle CssClass="TextDate" /></BDP:BasicDatePicker>
                  <asp:TextBox ID="tbStartTime" runat="server" CssClass="TextBox" MaxLength="5" 
                      ValidationGroup="Input" Width="53px" AutoPostBack="True" />
                  <asp:Label ID="Label6" runat="server" ForeColor="Red" Text="*"></asp:Label>
              </td>          
              <td>End Date & Time</td>
              <td>:</td>
              <td>
                  <BDP:BasicDatePicker ID="tbEndDate" runat="server" ButtonImageHeight="19px" 
                      ButtonImageWidth="20px" DateFormat="dd MMM yyyy" DisplayType="TextBoxAndImage" 
                      ReadOnly="true" ShowNoneButton="False" TextBoxStyle-CssClass="TextDate" 
                      ValidationGroup="Input" AutoPostBack="True"><TextBoxStyle CssClass="TextDate" /></BDP:BasicDatePicker>
                    <asp:TextBox ID="tbEndTime" runat="server" CssClass="TextBox" MaxLength="5" 
                      ValidationGroup="Input" Width="53px" AutoPostBack="True" />
                  <asp:Label ID="Label7" runat="server" ForeColor="Red" Text="*"></asp:Label>
              </td>
          </tr>
          <tr>
              <td>Minute</td>
              <td>:</td>
              <td>
                <table>
                    <tr align ="center">
                        <td bgcolor = "#cccccc">Minute Bruto</td>
                        <td bgcolor = "#cccccc">Minute Break</td>
                        <td bgcolor = "#cccccc">Minute Netto</td>
                    </tr>
                    <tr>
                        <td><asp:TextBox ID="tbMinuteBruto" runat="server" CssClass="TextBoxR" 
                                Enabled = "false" Width="65px" /></td>
                        <td><asp:TextBox ID="tbMinuteBreak" runat="server" CssClass="TextBox" 
                                ValidationGroup="Input" Width="67px" AutoPostBack="True" /></td>
                        <td><asp:TextBox ID="tbMinuteNetto" runat="server" CssClass="TextBoxR" 
                                Enabled = "false" Width="68px" /></td>
                    </tr>
                </table>
              </td>
              <td colspan="3">
              
              <table style="width: 197px">
              <tr>
              <td>Susulan</td>
              <td>:</td>
              <td>
                  <asp:DropDownList ID="ddlSusulan" runat="server" CssClass="DropDownList" 
                      Height="23px" ValidationGroup="Input" Width="41px">
                      <asp:ListItem>Y</asp:ListItem>
                      <asp:ListItem>N</asp:ListItem>
                  </asp:DropDownList>
                  <asp:Label ID="Label3" runat="server" ForeColor="Red" Text="*"></asp:Label>
              </td>
              </tr>
              <tr>
              <td>Meal Allowance</td>
              <td>:</td>
              <td>
                  <asp:DropDownList ID="ddlFgMealAllowance" runat="server" CssClass="DropDownList" 
                      Height="23px" ValidationGroup="Input" Width="41px">
                      <asp:ListItem>Y</asp:ListItem>
                      <asp:ListItem>N</asp:ListItem>
                  </asp:DropDownList>
                  <asp:Label ID="Label8" runat="server" ForeColor="Red" Text="*"></asp:Label>
              </td>
              </tr>
              </table>
              </td>
          </tr>
          <tr>
              <td>Acknowledge By</td>
              <td>:</td>
              <td colspan="4">
                  <asp:TextBox ID="tbAcknowBy" runat="server" AutoPostBack="True" CssClass="TextBox" MaxLength="15" ValidationGroup="Input" Width="116px" />
                  <asp:TextBox ID="tbAcknowByName" runat="server" CssClass="TextBoxR" 
                      MaxLength="255" Width="325px" Enabled="False" />
                  <asp:Button ID="btnAcknow" runat="server" class="btngo" Text="..." 
                      ValidationGroup="Input" />
                  <asp:Label ID="Label9" runat="server" ForeColor="Red" Text="*"></asp:Label>
              </td>
          </tr>
          <tr>
              <td>Acknowledge Job Title</td>
              <td>:</td>
              <td colspan="4">
                  <asp:DropDownList ID="ddlAcknowledgeJbt" runat="server" CssClass="DropDownList" Enabled = "false" Height="18px" Width="222px">
                  </asp:DropDownList>
              </td>
          </tr>
          <tr>
              <td>Approval By</td>
              <td>:</td>
              <td colspan="4">
                  <asp:TextBox ID="tbApprBy" runat="server" AutoPostBack="True" CssClass="TextBox" MaxLength="15" ValidationGroup="Input" Width="116px" />
                  <asp:TextBox ID="tbApprByName" runat="server" CssClass="TextBoxR" 
                      MaxLength="255" Width="325px" Enabled="False" />
                  <asp:Button ID="btnAppr" runat="server" class="btngo" Text="..." 
                      ValidationGroup="Input" />
                  <asp:Label ID="Label10" runat="server" ForeColor="Red" Text="*"></asp:Label>
              </td>
          </tr>
          <tr>
              <td>Approval Job Title</td>
              <td>:</td>
              <td colspan="4">
                  <asp:DropDownList ID="ddlApprJbt" runat="server" CssClass="DropDownList" Enabled = "false" Height="18px" Width="222px">
                  </asp:DropDownList>
              </td>
          </tr>
          <tr>
              <td>Remark</td>
              <td>:</td>
              <td colspan="4">
                  <asp:TextBox ID="tbRemark" runat="server" CssClass="TextBoxMulti" MaxLength="255" 
                      ValidationGroup="Input" Width="386px" TextMode="MultiLine" />&nbsp;
                  <asp:Button ID="btnGetData" runat="server" class="bitbtn btngetitem" 
                      Text="Get Data" ValidationGroup="Input" />
              </td>
          </tr>
      </table>  
      
      <br />      
      <div style="font-size:medium; color:Blue;">Detail</div>
      <hr style="color:Blue" />  
      
              <asp:Menu ID="Menu1" runat="server" CssClass="Menu" ItemWrap="False" 
            Orientation="Horizontal" StaticEnableDefaultPopOutImage="False" 
            StaticMenuItemStyle-CssClass="MenuItem" 
            StaticSelectedStyle-CssClass="MenuSelect" Visible="False">
                  <StaticSelectedStyle CssClass="MenuSelect" />
                  <StaticMenuItemStyle CssClass="MenuItem" />
                  <Items>
                      <asp:MenuItem Text="Detail " Value="0"></asp:MenuItem>
                      <asp:MenuItem Text="Detail WO" Value="1"></asp:MenuItem>
                  </Items>
        </asp:Menu>
        <asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex="0">
           <asp:View ID="Tab1" runat="server"> 
        <br />
              <asp:Panel runat="server" ID="PnlDt">
              <asp:Button class="bitbtndt btnadd" runat="server" ID="btnAdddt" Text="Add" ValidationGroup="Input" />
                <div style="border:0px  solid; width:100%; height:100%; overflow:auto;">
                    <asp:GridView ID="GridDt" runat="server" AutoGenerateColumns="false" AllowSorting="False" 
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
                              
                               <asp:Button Class="bitbtndt btngetitem"  ID="btnClosing" runat="server" Text="Complete" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" CommandName="Closing" Width = "70px" /> <%--UseSubmitBehavior="false"--%>
                              </ItemTemplate>
                             
                        </asp:TemplateField> 
                         <asp:TemplateField>
                         <ItemTemplate> 
                         <%--CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>"  --%>
                               <asp:Button Class="bitbtndt btngetitem"  ID="btnUnClosing" runat="server" Text="Un Complete" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" CommandName="UnClosing" Width = "90px" />
                              </ItemTemplate>
                             
                        </asp:TemplateField>   
                            <asp:BoundField DataField="EmpNumb" SortExpression="EmpNumb" HeaderText="Employee No" />
                            <asp:BoundField DataField="EmpName"  HeaderText="Employee Name" />
                            <asp:BoundField DataField="DayType" HeaderStyle-Width="80px" HeaderText="Day Type" />
                            <asp:BoundField DataField="StartDate" dataformatstring="{0:dd MMM yyyy}" HeaderStyle-Width="80px" HeaderText="Start Date" htmlencode="true"/>
                            <asp:BoundField DataField="EndDate" dataformatstring="{0:dd MMM yyyy}" HeaderStyle-Width="80px" HeaderText="End Date" htmlencode="true"/>
                            <asp:BoundField DataField="StartTime" HeaderStyle-Width="80px" HeaderText="Start Time" />
                            <asp:BoundField DataField="EndTime" HeaderStyle-Width="80px" HeaderText="End Time" />
                            <asp:BoundField DataField="MinuteBruto" HeaderStyle-Width="80px" HeaderText="Minute Bruto" />
                            <asp:BoundField DataField="MinuteBreak" HeaderStyle-Width="80px" HeaderText="Minute Break" />
                            <asp:BoundField DataField="MinuteNetto" HeaderStyle-Width="80px" HeaderText="Minute Netto" />
                            <asp:BoundField DataField="HourNetto" HeaderStyle-Width="80px" HeaderText="Hour Netto" />
                            <asp:BoundField DataField="FgMealAllowance" HeaderStyle-Width="80px" HeaderText="Meal Allowance" />
                            <asp:BoundField DataField="Remark" HeaderStyle-Width="250px" HeaderText="Remark" />                            
                            <asp:BoundField DataField="DoneComplete" HeaderStyle-Width="80px" HeaderText="Done Complete" /> 
                        </Columns>
                    </asp:GridView>
              </div>   
              <asp:Button class="bitbtndt btnadd" runat="server" ID="btnAddDtke2" Text="Add" ValidationGroup="Input" />
              </asp:Panel> 
              <asp:Panel runat="server" ID="pnlEditDt" Visible="false">
                <table style="width: 659px">             
                    <tr>
                        <td>Employee</td>
                        <td>:</td>
                        <td colspan="4"><asp:TextBox ID="tbEmpNo" runat="server" CssClass="TextBox" MaxLength="15" ValidationGroup="Input" Width="116px" AutoPostBack="True" />
                            <asp:TextBox ID="tbEmpName" runat="server" CssClass="TextBoxR" MaxLength="255" 
                                Width="325px" Enabled="False" />
                            <asp:Button ID="btnEmp" runat="server" class="btngo" Text="..." />
                            <asp:Label ID="Label11" runat="server" ForeColor="Red" Text="*"></asp:Label>
                        </td>
                    </tr>
                    <tr>                    
                        <td>Day Type</td>
                        <td>:</td>
                        <td colspan="4">
                            <asp:DropDownList ID="ddlDayTypeDt" runat="server" CssClass="DropDownList" 
                                Height="18px" ValidationGroup="Input" Width="80px">
                                <asp:ListItem>Work</asp:ListItem>
                                <asp:ListItem>Holiday</asp:ListItem>
                            </asp:DropDownList>
                            <asp:Label ID="Label12" runat="server" ForeColor="Red" Text="*"></asp:Label>
                        </td>
                    </tr>
                    <tr>                    
                        <td>Start Date</td>
                        <td>:</td>
                        <td style="margin-left: 40px">
                            <BDP:BasicDatePicker ID="tbStartDateDt" runat="server" ButtonImageHeight="19px" 
                                ButtonImageWidth="20px" DateFormat="dd MMM yyyy" DisplayType="TextBoxAndImage" 
                                ReadOnly="true" ShowNoneButton="False" TextBoxStyle-CssClass="TextDate" 
                                ValidationGroup="Input" AutoPostBack="True"><TextBoxStyle CssClass="TextDate" /></BDP:BasicDatePicker>
                            <asp:Label ID="Label13" runat="server" ForeColor="Red" Text="*"></asp:Label>
                        </td>
                        <td style="margin-left: 40px">
                            Start Time</td>
                        <td style="margin-left: 40px">
                            :</td>
                        <td style="margin-left: 40px">
                            <asp:TextBox ID="tbStartTimeDt" runat="server" CssClass="TextBox" MaxLength="5" 
                                ValidationGroup="Input" Width="53px" AutoPostBack="True" />
                            <asp:Label ID="Label15" runat="server" ForeColor="Red" Text="*"></asp:Label>
                        </td>
                    </tr>                    
                    <tr>
                        <td>End Date</td>
                        <td>:</td>
                        <td style="margin-left: 40px">
                            <BDP:BasicDatePicker ID="tbEndDateDt" runat="server" ButtonImageHeight="19px" 
                                ButtonImageWidth="20px" DateFormat="dd MMM yyyy" DisplayType="TextBoxAndImage" 
                                ReadOnly="true" ShowNoneButton="False" TextBoxStyle-CssClass="TextDate" 
                                ValidationGroup="Input" AutoPostBack="True"><TextBoxStyle CssClass="TextDate" /></BDP:BasicDatePicker>
                            <asp:Label ID="Label14" runat="server" ForeColor="Red" Text="*"></asp:Label>
                        </td>
                        <td style="margin-left: 40px">
                            End Time</td>
                        <td style="margin-left: 40px">
                            :</td>
                        <td style="margin-left: 40px">
                            <asp:TextBox ID="tbEndTimeDt" runat="server" CssClass="TextBox" MaxLength="5" 
                                ValidationGroup="Input" Width="53px" AutoPostBack="True" />
                            <asp:Label ID="Label16" runat="server" ForeColor="Red" Text="*"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="style1">Minute</td>
                        <td class="style1">:</td>
                        <td style="margin-left: 40px" class="style1">
                        <table>
                            <tr align ="center">
                                <td bgcolor = "#cccccc">Minute Bruto</td>
                                <td bgcolor = "#cccccc">Minute Break</td>
                                <td bgcolor = "#cccccc">Minute Netto</td>
                            </tr>
                            <tr>
                                <td><asp:TextBox ID="tbMinuteBrutoDt" runat="server" CssClass="TextBoxR" Enabled = "false" Width="64px" /></td>
                                <td><asp:TextBox ID="tbMinuteBreakDt" runat="server" CssClass="TextBox" 
                                        ValidationGroup="Input" Width="64px" AutoPostBack="True" /></td>
                                <td><asp:TextBox ID="tbMinuteNettoDt" runat="server" CssClass="TextBoxR" 
                                        Enabled = "false" Width="64px" AutoPostBack="True" OnTextChanged = "tbMinuteNettoDt_TextChanged"/></td>
                            </tr>
                        </table>
                       </td>
                        <td class="style1" style="margin-left: 40px">Hour Netto</td>
                        <td class="style1" style="margin-left: 40px">:</td>
                        <td class="style1" style="margin-left: 40px">
                            <asp:TextBox ID="tbHourNetto" runat="server" CssClass="TextBoxR" Width="53px" 
                                Enabled="False" AutoPostBack="True" />
                        </td>
                    </tr>
                    <tr>
                        <td>Meal Allowance</td>
                        <td>:</td>
                        <td style="margin-left: 40px" colspan="4">
                            <asp:DropDownList ID="ddlFgAllowanceDt" runat="server" CssClass="DropDownList" 
                                Height="20px" ValidationGroup="Input" Width="44px">
                                <asp:ListItem>Y</asp:ListItem>
                                <asp:ListItem>N</asp:ListItem>
                            </asp:DropDownList>
                            <asp:Label ID="Label17" runat="server" ForeColor="Red" Text="*"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>Remark</td>
                        <td>:</td>
                        <td style="margin-left: 40px" colspan="4">
                        <asp:TextBox ID="tbRemarkDt" runat="server" CssClass="TextBox" MaxLength="60" ValidationGroup="Input" Width="308px" />
                            <asp:TextBox ID="tbDoneComplete" runat="server" CssClass="TextBox" MaxLength="5" 
                                ValidationGroup="Input" Visible="False" Width="53px" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                        <td style="margin-left: 40px" colspan="4">
                            &nbsp;</td>
                    </tr>
                </table>
                <br />           
                <asp:Button ID="btnSaveDt" runat="server" class="bitbtndt btnsave" Text="Save"/> &nbsp;         
                <asp:Button ID="btnCancelDt" runat="server" class="bitbtndt btncancel" Text="Cancel"/> 
           </asp:Panel> 
            </asp:View>
             <asp:View ID="Tab2" runat="server">
             <asp:Panel runat="server" ID="Panel1">
        <table style="background-color: #1085C7; color: #FFFFFF;" width = "733px">
            <tr> 
                <td class="style1">
                <div style="border-style: solid; border-color: inherit; border-width: 0px; width:692%; height:13px; overflow:auto;">
                    Complete Overtime</div>   
                </td>         
            </tr>
        </table>
       <asp:Panel runat="server" ID="Panel2">
            <table style="width: 733px">
                <tr>
                    <td>Overtime No</td>
                    <td>:</td>
                    <td colspan="4">
                        <asp:Label ID="lblOvertime" runat="server" Text="OvertimeNo"></asp:Label>
                     </td>
                    <td rowspan="12">
                        <table frame="border" style="border-style: solid; width: 136px;">
                            <tr align="center">
                                <td bgcolor="#cccccc" colspan="3">
                                    Info Absence</td>
                            </tr>
                            <tr>
                                <td>
                                    Time In</td>
                                <td>
                                    :</td>
                                <td>
                                    <asp:TextBox ID="tbTimeIn" runat="server" CssClass="TextBoxR" Enabled="false" 
                                        Width="53px" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Time Out</td>
                                <td>
                                    :</td>
                                <td>
                                    <asp:TextBox ID="TbTimeOut" runat="server" CssClass="TextBoxR" Enabled="false" 
                                        Width="53px" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>Overtime Date</td>
                    <td>:</td>
                    <td colspan="4">
                        <BDP:BasicDatePicker ID="tbDateC" runat="server" ButtonImageHeight="19px" 
                            ButtonImageWidth="20px" DateFormat="dd MMM yyyy" DisplayType="TextBoxAndImage" 
                            ReadOnly="true" ShowNoneButton="False" TextBoxStyle-CssClass="TextDate" 
                            Enabled = "false">
                            <TextBoxStyle CssClass="TextDate"/>
                        </BDP:BasicDatePicker>
                    </td>            
                </tr>                      
                <tr>
                    <td>Start Date</td>
                    <td>:</td>
                    <td>
                        <BDP:BasicDatePicker ID="tbStartDateC" runat="server" ButtonImageHeight="19px" 
                            ButtonImageWidth="20px" DateFormat="dd MMM yyyy" DisplayType="TextBoxAndImage" 
                            ReadOnly="true" ShowNoneButton="False" TextBoxStyle-CssClass="TextDate" 
                            Enabled ="false">
                            <TextBoxStyle CssClass="TextDate" />
                        </BDP:BasicDatePicker>
                    </td>
                    <td align = "right">Start Time</td>
                    <td>:</td>
                    <td><asp:TextBox ID="tbStartTimeC" runat="server" CssClass="TextBoxR" MaxLength="5" 
                            Width="53px" Enabled = "false"/>
                    </td>
                </tr>
                <tr>
                    <td>End Date</td>
                    <td>:</td>
                    <td><BDP:BasicDatePicker ID="tbEndDateC" runat="server" ButtonImageHeight="19px" 
                            ButtonImageWidth="20px" DateFormat="dd MMM yyyy" DisplayType="TextBoxAndImage" 
                            ReadOnly="true" ShowNoneButton="False" TextBoxStyle-CssClass="TextDate" 
                            Enabled = "false">
                            <TextBoxStyle CssClass="TextDate" />
                        </BDP:BasicDatePicker>
                    </td>
                    <td align = "right">End Time</td>
                    <td>:</td>
                    <td><asp:TextBox ID="tbEndTimeC" runat="server" CssClass="TextBox" MaxLength="5" 
                            Enabled = "false" Width= "53px" />
                    </td>
                </tr>
                <tr>
                    <td>Minute</td>
                    <td>:</td>
                    <td colspan="2">
                    <table>
                        <tr align = "center">
                        <td bgcolor = "#cccccc">Minute Bruto</td>
                        <td bgcolor = "#cccccc">Minute Break</td>
                        <td bgcolor = "#cccccc">Minute Netto</td>
                        </tr>
                        <tr>
                        <td><asp:TextBox ID="tbMinuteBrutoC" runat="server" CssClass="TextBoxR" 
                                Enabled="false" Width="65px" /></td>
                        <td><asp:TextBox ID="tbMinuteBreakC" runat="server" CssClass="TextBoxR" 
                                Enabled = "false" Width="67px" /></td>
                        <td><asp:TextBox ID="tbMinuteNettoC" runat="server" CssClass="TextBoxR" 
                                Enabled="false" Width="68px" /></td>
                        </tr>
                    </table>
                        </td>
                    <td colspan="2">
                        &nbsp;</td>
                </tr>
                <tr>
                    <td>Employee</td>
                    <td>:</td>
                    <td colspan="4">
                        <asp:TextBox ID="tbEmpNoC" runat="server" CssClass="TextBoxR" MaxLength="15" 
                            Width="116px" Enabled="False" />
                        <asp:TextBox ID="tbEmpNameC" runat="server" CssClass="TextBoxR" Enabled="False" 
                            MaxLength="255" Width="325px" />
                    </td>
                </tr>
                <tr>
                    <td>Day Type</td>
                    <td>:</td>
                    <td colspan="4">
                        <asp:DropDownList ID="ddlDayTypeC" runat="server" CssClass="DropDownList" 
                            Height="17px"  Width="91px" Enabled = "false">
                            <asp:ListItem>Work</asp:ListItem>
                            <asp:ListItem>Holiday</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>Act Start Date</td>
                    <td>:</td>
                    <td>
                        <BDP:BasicDatePicker ID="tbActStartDateC" runat="server" ButtonImageHeight="19px" 
                            ButtonImageWidth="20px" DateFormat="dd MMM yyyy" DisplayType="TextBoxAndImage" 
                            ReadOnly="true" ShowNoneButton="False" TextBoxStyle-CssClass="TextDate" 
                            ValidationGroup="Input" AutoPostBack="True">
                            <TextBoxStyle CssClass="TextDate" />
                        </BDP:BasicDatePicker>
                    </td>
                    <td align = "right">Act Start Time</td>
                    <td>:</td>
                    <td><asp:TextBox ID="tbActStartTimeC" runat="server" AutoPostBack="True" 
                            CssClass="TextBox" MaxLength="5" ValidationGroup="Input" Width="53px" />
                    </td>
                </tr>
                <tr>
                    <td>Act End Date</td>
                    <td>:</td>
                    <td><BDP:BasicDatePicker ID="tbActEndDateC" runat="server" ButtonImageHeight="19px" 
                            ButtonImageWidth="20px" DateFormat="dd MMM yyyy" DisplayType="TextBoxAndImage" 
                            ReadOnly="true" ShowNoneButton="False" TextBoxStyle-CssClass="TextDate" 
                            ValidationGroup="Input" AutoPostBack="True">
                            <TextBoxStyle CssClass="TextDate" />
                        </BDP:BasicDatePicker>
                    </td>
                    <td align = "right">Act End Time</td>
                    <td>:</td>
                    <td><asp:TextBox ID="tbActEndTimeC" runat="server" AutoPostBack="True" 
                            CssClass="TextBox" MaxLength="5" ValidationGroup="Input" Width="53px" />
                    </td>
                </tr>
                <tr>
                    <td>Minute</td>
                    <td>:</td>
                    <td colspan="2">
                    <table>
                        <tr align ="center">
                            <td bgColor="#cccccc">Minute Bruto</td>
                            <td bgColor="#cccccc">Minute Break</td>
                            <td bgColor="#cccccc">Minute Netto</td>
                        </tr>
                        <tr>
                            <td><asp:TextBox ID="tbActMinuteBrutoC" runat="server" CssClass="TextBoxR" 
                                    Enabled="false" Width="65px" /></td>
                            <td><asp:TextBox ID="tbActMinuteBreakC" runat="server" AutoPostBack="True" 
                                    CssClass="TextBox" ValidationGroup="Input" Width="67px" /></td>
                            <td><asp:TextBox ID="tbActMinuteNettoC" runat="server" CssClass="TextBoxR" 
                                    Enabled="false" Width="68px" />
                            </td>
                        </tr>
                    </table>
                    </td>
                    <td colspan="2">
                        &nbsp;</td>
                </tr>
                <tr>
                    <td>Hour Netto</td>
                    <td>:</td>
                    <td><asp:TextBox ID="tbActHournettoC" runat="server" CssClass="TextBoxR" 
                            Enabled="false" Width="65px" />
                    </td>
                    <td align = "right">OT Hour</td>
                    <td>:</td>
                    <td><asp:TextBox ID="tbOTHour" runat="server" CssClass="TextBoxR" Enabled="false" Width="65px" />
                    </td>
                </tr>
                <tr>
                    <td>Act Meal Allowance</td>
                    <td>:</td>
                    <td colspan="4"><asp:DropDownList ID="ddlActFgMealAllowanceC" runat="server" 
                            CssClass="DropDownList" Height="23px" ValidationGroup="Input" Width="41px">
                            <asp:ListItem>Y</asp:ListItem>
                            <asp:ListItem>N</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
            </table>
            <br />                     
       </asp:Panel>           
            <asp:Button ID="btnOK2" runat="server" class="bitbtn btnok" Text="OK" Width="59px" />
            <asp:Button ID="btnCancel2" runat="server" class="bitbtn btncancel" Text="Cancel" Width="59px" />
            <asp:Button ID="btnBack2" runat="server" class="bitbtn btnback" Text="Back" 
                Width="59px" Visible="False" />           
       </asp:Panel>
             </asp:View>
             </asp:MultiView> 
        <br />
       <br />      
        <asp:Button ID="btnSaveAll" runat="server" class="bitbtndt btnsave" 
            Text="Save & New" ValidationGroup="Input" Width="97px"/> &nbsp;    
        <asp:Button ID="btnSaveTrans" runat="server" class="bitbtndt btnsave" Text="Save" ValidationGroup="Input"/> &nbsp;    
        <asp:Button ID="btnBack" runat="server" class="bitbtndt btncancel" Text="Cancel" ValidationGroup="Input"/>  &nbsp;
        <asp:Button ID="btnHome" runat="server" class="btngo" Text="Home" Width="48px"/>    
    </asp:Panel>        
    </div>
   
    <asp:Label runat ="server" ID="lbStatus" ForeColor="Red"/>
    <asp:HiddenField ID="HiddenRemarkClose" runat="server" />
    <asp:HiddenField ID="HiddenStartHour" runat="server" />
    <asp:HiddenField ID="HiddenEndHour" runat="server" />
    </form>
</body>
</html>
