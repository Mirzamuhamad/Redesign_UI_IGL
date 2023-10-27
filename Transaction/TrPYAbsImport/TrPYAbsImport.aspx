<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TrPYAbsImport.aspx.vb" Inherits="Transaction_TrPYAbsImport_TrPYAbsImport" %>
<%@ Register Assembly="BasicFrame.WebControls.BasicDatePicker" Namespace="BasicFrame.WebControls"
    TagPrefix="BDP" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Untitled Page</title>
    <script src="../../Function/OpenDlg.js" type="text/javascript"></script>
    <script src="../../Function/Function.js" type="text/javascript"></script>
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
         var AmountForex = document.getElementById("tbAmountForex").value.replace(/\$|\,/g,""); 
         var Rate = document.getElementById("tbRate").value.replace(/\$|\,/g,""); 
                          
         document.getElementById("tbRate").value = setdigit(Rate,'<%=ViewState("DigitRate")%>');
         document.getElementById("tbAmountForex").value = setdigit(AmountForex,'<%=VIEWSTATE("DigitCurr")%>');                 
        }catch (err){
            alert(err.description);
          }      
        }     
    </script>
    
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .style1
        {
        }
        .style6
        {
            width: 90px;
            text-align:right;
        }
        .style7
        {
            width: 94px;
        }
        </style>
</head>
<body>
    <form id="form1" runat="server">
    <div class="Content">
    <div class="H1">Absence Import</div>
     <hr style="color:Blue" />        
      
      <table>
        <tr>            
            <td style="width:150px;text-align:right"><b>Please Select Excel File : </b></td> 
            <td>                
                <asp:FileUpload ID="fileuploadExcel" runat="server" />&nbsp;  
                <asp:Button class="bitbtn btnpreview" Width="98px" runat="server" ID="btnUpload" Text="Upload" />
            </td>
        </tr>
        <tr>
            <td style="width:150px;text-align:right"><b>Display Data : </b></td> 
            <td>
                <asp:DropDownList ID="ddlSheets" runat="server" Width="200px" AutoPostBack="true" CssClass="DropDownList"
                    AppendDataBoundItems = "true">
                </asp:DropDownList>
            </td>             
        </tr>
        <tr>
        <td style="width:150px;text-align:right">Period Date : </td>
        <td><BDP:BasicDatePicker ID="tbStartDate" runat="server" DateFormat="dd MMM yyyy" 
                        ReadOnly = "true" ValidationGroup="Input"
                        ButtonImageHeight="19px" ButtonImageWidth="20px" 
                        DisplayType="TextBoxAndImage" 
                        TextBoxStyle-CssClass="TextDate" AutoPostBack="true"
                        ShowNoneButton="False"><TextBoxStyle CssClass="TextDate" /></BDP:BasicDatePicker>
                <b>s/d </b>
                <BDP:BasicDatePicker ID="tbEndDate" runat="server" DateFormat="dd MMM yyyy" 
                        ReadOnly = "true" ValidationGroup="Input"
                        ButtonImageHeight="19px" ButtonImageWidth="20px" 
                        DisplayType="TextBoxAndImage" 
                        TextBoxStyle-CssClass="TextDate" AutoPostBack="true"
                        ShowNoneButton="False"><TextBoxStyle CssClass="TextDate" /></BDP:BasicDatePicker>                            
        </td>
        </tr>
        <tr>
            <td></td>  
            <td>
            <asp:Button class="bitbtn btnpreview" Width="98px" runat="server" ID="btnImport" Text="Import Excel" />
            <asp:Button class="bitbtn btnpreview" Width="98px" runat="server" ID="btnImportDb" Text="Import DB" />
            <asp:Button class="bitbtn btnpreview" Width="98px" runat="server" ID="btnSorted" Text="Sorted" visible="false"/>
            <asp:Button class="bitbtn btnpreview" Width="120px" runat="server" ID="btnArrange" Text="Generate Absence" />
            <asp:Button class="bitbtn btnpreview" Width="155px" runat="server" ID="btnAbsence" Text="Absence Not In Schedule" />
            <asp:Button class="bitbtn btnpreview" Width="155px" runat="server" ID="btnSchedule" Text="Schedule Not In Absence" />
            </td>
        </tr>
        <%--<tr>
            <td style="width:150px;text-align:right">Period Date :</td>         
            <td>
            </td>
            </tr>--%>
      </table>      
                <br />  
          <asp:Panel runat="server" id="PnlHd">          
          <div style="border:0px  solid; width:100%; height:100%; overflow:auto;">
          <asp:GridView ID="GridView1" runat="server" AllowPaging="True" AllowSorting="true" PageSize="50" 
            CssClass="Grid" AutoGenerateColumns="true"> 
              <HeaderStyle CssClass="GridHeader" Wrap = "false"></HeaderStyle>
						<RowStyle CssClass="GridItem" Wrap="false" />
						<AlternatingRowStyle CssClass="GridAltItem"/>
						<PagerStyle CssClass="GridPager" />
              <Columns>              
                  				  	
					
              </Columns>
          </asp:GridView>
          </div>   
          </asp:Panel>
          <asp:Panel runat="server" ID="pnlImport" Visible="false">
            <fieldset style="width:800px">
                    <legend>Import Data Absence</legend>
                    <b>Absence ID : </b>&nbsp
                <asp:DropDownList ID="ddlFieldNIK" runat="server" Width="140px" CssClass="DropDownList" 
                    AppendDataBoundItems = "true">
                </asp:DropDownList>
                &nbsp &nbsp &nbsp &nbsp <b>Date Log : </b>&nbsp
                <asp:DropDownList ID="ddlFieldDate" runat="server" Width="140px" CssClass="DropDownList"
                    AppendDataBoundItems = "true">
                </asp:DropDownList>
                &nbsp &nbsp &nbsp &nbsp <b>Time Log : </b>&nbsp
                <asp:DropDownList ID="ddlFieldTime" runat="server" Width="140px" CssClass="DropDownList"
                    AppendDataBoundItems = "true">
                </asp:DropDownList>
                &nbsp
                <asp:Button ID="btnGenerate" runat="server" class="bitbtndt btnok" Width="110px" Text="Generate Data" /> 
              
            </fieldset>  
                   
      
      <br />
      <div style="border:0px  solid; width:100%; height:100%; overflow:auto;">
      <asp:GridView ID="GridImport" runat="server" AllowPaging="True" AllowSorting="true" PageSize="30" 
            CssClass="Grid" AutoGenerateColumns="false"> 
              <HeaderStyle CssClass="GridHeader" Wrap = "false"></HeaderStyle>
						<RowStyle CssClass="GridItem" Wrap="false" />
						<AlternatingRowStyle CssClass="GridAltItem"/>
						<PagerStyle CssClass="GridPager" />
              <Columns>              
                  <asp:BoundField HeaderText="Emp ID" DataField="EmpNumb" HeaderStyle-Width="110" SortExpression="EmpNumb" />
				  <asp:BoundField HeaderText="Emp Name" DataField="EmpName" HeaderStyle-Width="210" SortExpression="EmpName" />
				  <asp:BoundField HeaderText="Absence ID" DataField="AbsenceCard" HeaderStyle-Width="80" SortExpression="AbsenceCard" />
				  <asp:BoundField HeaderText="No" DataField="ItemNo" HeaderStyle-Width="20" SortExpression="ItemNo" />
				  <asp:BoundField HeaderText="Date" DataField="AbsDate" dataformatstring="{0:dd MMM yyyy}" htmlencode="true" HeaderStyle-Width="60" SortExpression="AbsDate" />
				  <asp:BoundField HeaderText="Time" DataField="AbsTime" HeaderStyle-Width="50" SortExpression="AbsTime" />
              </Columns>
          </asp:GridView>
          </div>
      </asp:Panel>     
      
      <asp:Panel runat="server" ID="pnlSorted" Visible="false">
      <div style="border:0px  solid; width:100%; height:100%; overflow:auto;">
      <asp:GridView ID="GridSorted" runat="server" AllowPaging="True" AllowSorting="true" PageSize="20" 
            CssClass="Grid" AutoGenerateColumns="False"> 
              <HeaderStyle CssClass="GridHeader" Wrap = "false"></HeaderStyle>
						<RowStyle CssClass="GridItem" Wrap="false" />
						<AlternatingRowStyle CssClass="GridAltItem"/>
						<PagerStyle CssClass="GridPager" />
              <Columns>              
                  <asp:BoundField HeaderText="Emp No" DataField="EmpNumb" HeaderStyle-Width="100" SortExpression="EmpNumb" />
				  <asp:BoundField HeaderText="Emp Name" DataField="Emp_Name" HeaderStyle-Width="180"  SortExpression="Emp_Name"/>
				  <asp:BoundField HeaderText="Organization" DataField="Department_Name" HeaderStyle-Width="180"  SortExpression="Department_Name"/>
				  <asp:BoundField HeaderText="Date" HtmlEncode="true" DataFormatString="{0:dd MMM yyyy}" DataField="AbsDate" HeaderStyle-Width="50" SortExpression="AbsDate"  />
				  <asp:BoundField HeaderText="Time" DataField="AbsTime" HeaderStyle-Width="20"  SortExpression="AbsTime"/>
				  <asp:BoundField HeaderText="Shift" DataField="Shift" HeaderStyle-Width="40"  SortExpression="Shift"/>
				  <asp:BoundField HeaderText="Shift In" DataField="ShiftIn" HeaderStyle-Width="40"  SortExpression="ShiftIn"/>
				  <asp:BoundField HeaderText="Shift Out" DataField="ShiftOut" HeaderStyle-Width="40"  SortExpression="ShiftOut"/>
				  <asp:BoundField HeaderText="Status" DataField="Status" HeaderStyle-Width="30"  SortExpression="Status"/>
				  <asp:BoundField HeaderText="Signed" DataField="Signed" HeaderStyle-Width="30"  SortExpression="Signed"/>				  
              </Columns>
          </asp:GridView>
          </div>
      <br />        
      </asp:Panel> 
      <asp:Panel runat="server" ID="pnlArrange" Visible="false">
      <div style="border:0px  solid; width:100%; height:100%; overflow:auto;">
      <asp:GridView ID="GridArrange" runat="server" AllowPaging="True" AllowSorting="true" PageSize="20" 
            CssClass="Grid" AutoGenerateColumns="False"> 
              <HeaderStyle CssClass="GridHeader" Wrap = "false"></HeaderStyle>
						<RowStyle CssClass="GridItem" Wrap="false" />
						<AlternatingRowStyle CssClass="GridAltItem"/>
						<PagerStyle CssClass="GridPager" />
              <Columns>              
                  <asp:BoundField HeaderText="Emp No" DataField="EmpNumb" HeaderStyle-Width="100" SortExpression="EmpNumb" />
				  <asp:BoundField HeaderText="Emp Name" DataField="EmpName" HeaderStyle-Width="180"  SortExpression="EmpName"/>
				  <asp:BoundField HeaderText="Organization" DataField="SectionName" HeaderStyle-Width="180"  SortExpression="SectionName"/>
				  <asp:BoundField HeaderText="Date In" HtmlEncode="true" DataFormatString="{0:dd MMM yyyy}" DataField="DateIn" HeaderStyle-Width="50" SortExpression="DateIn"  />
				  <asp:BoundField HeaderText="Time In" DataField="TimeIn" HeaderStyle-Width="20"  SortExpression="TimeIn"/>
				  <asp:BoundField HeaderText="Date Out" HtmlEncode="true" DataFormatString="{0:dd MMM yyyy}" DataField="DateOut" HeaderStyle-Width="50" SortExpression="DateOut"  />
				  <asp:BoundField HeaderText="Time Out" DataField="TimeOut" HeaderStyle-Width="20"  SortExpression="TimeOut"/>
				  <asp:BoundField HeaderText="Shift" DataField="ShiftName" HeaderStyle-Width="40"  SortExpression="ShiftName"/>
				  <asp:BoundField HeaderText="Shift In" DataField="ShiftIn" HeaderStyle-Width="40"  SortExpression="ShiftIn"/>
				  <asp:BoundField HeaderText="Shift Out" DataField="ShiftOut" HeaderStyle-Width="40"  SortExpression="ShiftOut"/>
				  <asp:BoundField HeaderText="Late In" DataField="FgLateIn" HeaderStyle-Width="30"  SortExpression="FgLateIn"/>
				  <asp:BoundField HeaderText="Late Minute" DataField="LateMinute" HeaderStyle-Width="30"  SortExpression="LateMinute"/>
				  <asp:BoundField HeaderText="Early Out" DataField="FgEO" HeaderStyle-Width="30"  SortExpression="FgEO"/>
				  <asp:BoundField HeaderText="EO Minute" DataField="EOMinute" HeaderStyle-Width="30"  SortExpression="EOMinute"/>
				  <asp:BoundField HeaderText="Absence" DataField="AbsStatusName" HeaderStyle-Width="100"  SortExpression="AbsStatusName"/>
				  <%--<asp:BoundField HeaderText="Scan In" DataField="FgScanIn" HeaderStyle-Width="30"  SortExpression="FgScanIn"/>
				  <asp:BoundField HeaderText="Scan Out" DataField="FgScanOut" HeaderStyle-Width="30"  SortExpression="FgScanOut"/>				  --%>
              </Columns>
          </asp:GridView>
          </div>
      </asp:Panel>
      
      <asp:Panel runat="server" ID="pnlAbsenceNIS" Visible="false">
      <div style="border:0px  solid; width:100%; height:100%; overflow:auto;">
      <asp:GridView ID="GridAbsenceNIS" runat="server" AllowPaging="True" AllowSorting="true" PageSize="20" 
            CssClass="Grid" AutoGenerateColumns="False"> 
              <HeaderStyle CssClass="GridHeader" Wrap = "false"></HeaderStyle>
						<RowStyle CssClass="GridItem" Wrap="false" />
						<AlternatingRowStyle CssClass="GridAltItem"/>
						<PagerStyle CssClass="GridPager" />
              <Columns>              
                  <asp:BoundField HeaderText="EmpNumb" DataField="EmpNumb" HeaderStyle-Width="100" SortExpression="EmpNumb" />
				  <asp:BoundField HeaderText="EmpName" DataField="EmpName" HeaderStyle-Width="180"  SortExpression="EmpName"/>
				  <asp:BoundField HeaderText="Absence Date" HtmlEncode="true" DataFormatString="{0:dd MMM yyyy}" DataField="AbsDate" HeaderStyle-Width="50" SortExpression="AbsDate"  />
				  <asp:BoundField HeaderText="Hari" DataField="Hari" HeaderStyle-Width="30"  SortExpression="Hari"/>
				  
				  <%--<asp:BoundField HeaderText="Organization" DataField="SectionName" HeaderStyle-Width="180"  SortExpression="SectionName"/>
				  <asp:BoundField HeaderText="Date In" HtmlEncode="true" DataFormatString="{0:dd MMM yyyy}" DataField="DateIn" HeaderStyle-Width="50" SortExpression="DateIn"  />
				  <asp:BoundField HeaderText="Time In" DataField="TimeIn" HeaderStyle-Width="20"  SortExpression="TimeIn"/>
				  <asp:BoundField HeaderText="Date Out" HtmlEncode="true" DataFormatString="{0:dd MMM yyyy}" DataField="DateOut" HeaderStyle-Width="50" SortExpression="DateOut"  />
				  <asp:BoundField HeaderText="Time Out" DataField="TimeOut" HeaderStyle-Width="20"  SortExpression="TimeOut"/>
				  <asp:BoundField HeaderText="Shift" DataField="ShiftName" HeaderStyle-Width="40"  SortExpression="ShiftName"/>
				  <asp:BoundField HeaderText="Shift In" DataField="ShiftIn" HeaderStyle-Width="40"  SortExpression="ShiftIn"/>
				  <asp:BoundField HeaderText="Shift Out" DataField="ShiftOut" HeaderStyle-Width="40"  SortExpression="ShiftOut"/>
				  <asp:BoundField HeaderText="Late In" DataField="FgLateIn" HeaderStyle-Width="30"  SortExpression="FgLateIn"/>
				  <asp:BoundField HeaderText="Late Minute" DataField="LateMinute" HeaderStyle-Width="30"  SortExpression="LateMinute"/>
				  <asp:BoundField HeaderText="Early Out" DataField="FgEO" HeaderStyle-Width="30"  SortExpression="FgEO"/>
				  <asp:BoundField HeaderText="EO Minute" DataField="EOMinute" HeaderStyle-Width="30"  SortExpression="EOMinute"/>
				  <asp:BoundField HeaderText="Absence" DataField="AbsStatusName" HeaderStyle-Width="100"  SortExpression="AbsStatusName"/>--%>
				  <%--<asp:BoundField HeaderText="Scan In" DataField="FgScanIn" HeaderStyle-Width="30"  SortExpression="FgScanIn"/>
				  <asp:BoundField HeaderText="Scan Out" DataField="FgScanOut" HeaderStyle-Width="30"  SortExpression="FgScanOut"/>				  --%>
              </Columns>
          </asp:GridView>
          </div>
      </asp:Panel>
      
      <asp:Panel runat="server" ID="pnlScheduleNIS" Visible="false">
      <div style="border:0px  solid; width:100%; height:100%; overflow:auto;">
      <asp:GridView ID="GridScheduleNIS" runat="server" AllowPaging="True" AllowSorting="true" PageSize="20" 
            CssClass="Grid" AutoGenerateColumns="False"> 
              <HeaderStyle CssClass="GridHeader" Wrap = "false"></HeaderStyle>
						<RowStyle CssClass="GridItem" Wrap="false" />
						<AlternatingRowStyle CssClass="GridAltItem"/>
						<PagerStyle CssClass="GridPager" />
              <Columns>              
                  <asp:BoundField HeaderText="EmpNumb" DataField="EmpNumb" HeaderStyle-Width="100" SortExpression="EmpNumb" />
				  <asp:BoundField HeaderText="EmpName" DataField="EmpName" HeaderStyle-Width="180"  SortExpression="EmpName"/>
				  <asp:BoundField HeaderText="Schedule Date" HtmlEncode="true" DataFormatString="{0:dd MMM yyyy}" DataField="ScheduleDate" HeaderStyle-Width="50" SortExpression="AbsDate"  />
				  <asp:BoundField HeaderText="Hari" DataField="Hari" HeaderStyle-Width="30"  SortExpression="Hari"/>
				  <asp:BoundField HeaderText="Shift" DataField="Shift" HeaderStyle-Width="30"  SortExpression="Shift"/>
              </Columns>
          </asp:GridView>
          </div>
      </asp:Panel>   
      
      <asp:Panel runat="server" ID="pnlImportDb" Visible="false">
      <div style="border:0px  solid; width:100%; height:100%; overflow:auto;">
      <asp:GridView ID="GridImportDb" runat="server" AllowPaging="True" AllowSorting="true" PageSize="20" 
            CssClass="Grid" AutoGenerateColumns="False"> 
              <HeaderStyle CssClass="GridHeader" Wrap = "false"></HeaderStyle>
						<RowStyle CssClass="GridItem" Wrap="false" />
						<AlternatingRowStyle CssClass="GridAltItem"/>
						<PagerStyle CssClass="GridPager" />
              <Columns>              
                  <asp:BoundField HeaderText="Emp ID" DataField="EmpNumb" HeaderStyle-Width="110" SortExpression="EmpNumb" />
				  <asp:BoundField HeaderText="Emp Name" DataField="EmpName" HeaderStyle-Width="210" SortExpression="EmpName" />
				  <asp:BoundField HeaderText="Absence ID" DataField="AbsenceCard" HeaderStyle-Width="80" SortExpression="AbsenceCard" />
				  <asp:BoundField HeaderText="No" DataField="ItemNo" HeaderStyle-Width="20" SortExpression="ItemNo" />
				  <asp:BoundField HeaderText="Date" DataField="AbsDate" dataformatstring="{0:dd MMM yyyy}" htmlencode="true" HeaderStyle-Width="60" SortExpression="AbsDate" />
				  <asp:BoundField HeaderText="Time" DataField="AbsTime" HeaderStyle-Width="50" SortExpression="AbsTime" />
              </Columns>
          </asp:GridView>
          </div>
      </asp:Panel>    
    </div>
   
    <asp:Label runat ="server" ID="lbStatus" ForeColor="Red"/>    
    </form>
</body>
</html>
