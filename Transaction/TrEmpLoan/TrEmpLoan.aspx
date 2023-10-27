<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TrEmpLoan.aspx.vb" Inherits="Transaction_TrEmpLoan_TrEmpLoan" %>
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
    
        function setformat()
        {
         try
         {  document.getElementById("tbAmountLoan").value = setdigit(document.getElementById("tbAmountLoan").value.replace(/\$|\,/g,""),'<%=ViewState("DigitCurr")%>');
            document.getElementById("tbInterest").value = setdigit(document.getElementById("tbInterest").value.replace(/\$|\,/g,""),'<%=ViewState("DigitCurr")%>');
            document.getElementById("tbAmountInterest").value = setdigit(document.getElementById("tbAmountInterest").value.replace(/\$|\,/g,""),'<%=ViewState("DigitCurr")%>');
            document.getElementById("tbTotalPembayaran").value = setdigit(document.getElementById("tbTotalPembayaran").value.replace(/\$|\,/g,""),'<%=ViewState("DigitCurr")%>');
            document.getElementById("tbangsuran").value = setdigit(document.getElementById("tbangsuran").value.replace(/\$|\,/g,""),'<%=ViewState("DigitCurr")%>'); 
        }catch (err){
            alert(err.description);
          }    
         } 
         
        function setformatdt()
        {
         try
         {  
            document.getElementById("tbAmountbeginDt").value = setdigit(document.getElementById("tbAmountbeginDt").value.replace(/\$|\,/g,""),'<%=ViewState("DigitCurr")%>'); 
            document.getElementById("tbAmountAngsurDt").value = setdigit(document.getElementById("tbAmountAngsurDt").value.replace(/\$|\,/g,""),'<%=ViewState("DigitCurr")%>'); 
            document.getElementById("tbAmountPokokDt").value = setdigit(document.getElementById("tbAmountPokokDt").value.replace(/\$|\,/g,""),'<%=ViewState("DigitCurr")%>'); 
            document.getElementById("tbAmountBungaDt").value = setdigit(document.getElementById("tbAmountBungaDt").value.replace(/\$|\,/g,""),'<%=ViewState("DigitCurr")%>'); 
            document.getElementById("tbAmountEndDt").value = setdigit(document.getElementById("tbAmountEndDt").value.replace(/\$|\,/g,""),'<%=ViewState("DigitCurr")%>'); 
        }catch (err){
            alert(err.description);
          }      
        }  
        
        function Confirm() {
            var confirm_value = document.createElement("INPUT");
            confirm_value.type = "hidden";
            confirm_value.name = "confirm_value";
            if (confirm("Do you want to applied to all data?")) {
                confirm_value.value = "Yes";
            } else {
                confirm_value.value = "No";
            }
            document.forms[0].appendChild(confirm_value);
        }

    
    </script>    
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div class="Content">
        <div class="H1">Employee Loan</div>
        <hr style="color:Blue" />
        <asp:Panel runat="server" id="PnlHd"> 
      <table>
        <tr>
            <td style="width:100px;text-align:right">Quick Search :</td>            
            <td><asp:TextBox CssClass="TextBox" runat="server" ID ="tbFilter"/> 
                  <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlField" >
                    <asp:ListItem Selected="True" Value="TransNmbr">Employee Loan No</asp:ListItem>
                    <asp:ListItem Value="dbo.FormatDate(TransDate)">Employee Loan Date</asp:ListItem>
                    <asp:ListItem>Revisi</asp:ListItem>
                    <asp:ListItem>Status</asp:ListItem>
                    <asp:ListItem Value="LoanType">Loan Type</asp:ListItem>
                    <asp:ListItem Value="JobLvlName">Job Level</asp:ListItem>                                        
                    <asp:ListItem Value="EmpNumb">Employee No</asp:ListItem>
                    <asp:ListItem Value="EmpName">Employee Name</asp:ListItem>                    
                    <asp:ListItem Value="LoanName">Loan</asp:ListItem>
                    <asp:ListItem Value="dbo.FormatDate(StartClaim)">Start Claim</asp:ListItem>
                    <asp:ListItem Value="AmountLoan">Amount Loan</asp:ListItem>
                    <asp:ListItem Value="Interest">Interest(%)/Year</asp:ListItem>
                    <asp:ListItem Value="TotalBunga">Amount Interest</asp:ListItem>
                    <asp:ListItem Value="TotalPay">Payment</asp:ListItem>
                    <asp:ListItem Value="QrtPeriod">Qty Term Period</asp:ListItem>
                    <asp:ListItem Value="TermPeriod">Term Period</asp:ListItem>
                    <asp:ListItem Value="Angsuran">Installment</asp:ListItem>
                    <asp:ListItem Value="Remark">Remark</asp:ListItem>                     
                    </asp:DropDownList>
                  <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlRange"></asp:DropDownList> 
                  <asp:Button class="bitbtn btnsearch" runat="server" ID="btnSearch" Text="Search" />                  
                  <asp:Button class="btngo" runat="server" ID="btnExpand" Text="..."/>  
            </td>
            <td>
                &nbsp;
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
                   <asp:ListItem Selected="True" Value="TransNmbr">Employee Loan No</asp:ListItem>
                    <asp:ListItem Value="dbo.FormatDate(TransDate)">Employee Loan Date</asp:ListItem>
                    <asp:ListItem>Revisi</asp:ListItem>
                    <asp:ListItem>Status</asp:ListItem>
                    <asp:ListItem Value="LoanType">Loan Type</asp:ListItem>
                    <asp:ListItem Value="JobLvlName">Job Level</asp:ListItem>                                        
                    <asp:ListItem Value="EmpNumb">Employee No</asp:ListItem>
                    <asp:ListItem Value="EmpName">Employee Name</asp:ListItem>                    
                    <asp:ListItem Value="LoanName">Loan</asp:ListItem>
                    <asp:ListItem Value="dbo.FormatDate(StartClaim)">Start Claim</asp:ListItem>
                    <asp:ListItem Value="AmountLoan">Amount Loan</asp:ListItem>
                    <asp:ListItem Value="Interest">Interest(%)/Year</asp:ListItem>
                    <asp:ListItem Value="TotalBunga">Amount Interest</asp:ListItem>
                    <asp:ListItem Value="TotalPay">Payment</asp:ListItem>
                    <asp:ListItem Value="QrtPeriod">Qty Term Period</asp:ListItem>
                    <asp:ListItem Value="TermPeriod">Term Period</asp:ListItem>
                    <asp:ListItem Value="Angsuran">Installment</asp:ListItem>
                    <asp:ListItem Value="Remark">Remark</asp:ListItem>                     
              </asp:DropDownList>
          </td>              
        </tr>        
      </table>      
      </asp:Panel>
          <br />
            <asp:Button class="bitbtn btnadd" runat="server" ID="BtnAdd" Text="Add" />
            
            &nbsp &nbsp &nbsp   
            <asp:DropDownList CssClass="DropDownList" ID="ddlCommand" runat="server" Visible="false"/>
            <asp:Button class="btngo" Visible="false" runat="server" ID="BtnGo" Text="G"/>
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
                              <asp:ListItem Text="Revisi" />
                              <asp:ListItem Text="Print" />
                          </asp:DropDownList>
                          <asp:Button class="btngo" runat="server" ID="BtnGo" Text="G" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" CommandName="Go" />
                      </ItemTemplate>
                      <HeaderStyle Width="110px" />
                  </asp:TemplateField>
                  <asp:BoundField DataField="TransNmbr" HeaderStyle-Width="120px" SortExpression="Nmbr" HeaderText="Employee Loan No"></asp:BoundField>                  
                  <asp:BoundField DataField="Status" HeaderText="Status"></asp:BoundField>                  
                  <asp:BoundField DataField="Revisi" HeaderText="Revisi"></asp:BoundField>
                  <asp:BoundField DataField="TransDate" dataformatstring="{0:dd MMM yyyy}" htmlencode="true" HeaderStyle-Width="80px" SortExpression="TransDate" HeaderText="Employee Loan Date"></asp:BoundField>
                  <asp:BoundField DataField="LoanType" HeaderStyle-Width="80px" SortExpression="LoanType" HeaderText="LoanType"></asp:BoundField>
                  <asp:BoundField DataField="JobLvlName" HeaderStyle-Width="100px" SortExpression="JobLvlName" HeaderText="Job Level"></asp:BoundField>
                  <asp:BoundField DataField="EmpNumb" HeaderStyle-Width="80px" SortExpression="EmpNumb" HeaderText="Employee No"></asp:BoundField>
                  <asp:BoundField DataField="EmpName" HeaderStyle-Width="200px" SortExpression="EmpName" HeaderText="Employee Name"></asp:BoundField>
                  <asp:BoundField DataField="LoanName" HeaderStyle-Width="100px" SortExpression="LoanName" HeaderText="Loan"></asp:BoundField>
                  <asp:BoundField DataField="StartClaim" dataformatstring="{0:dd MMM yyyy}" htmlencode="true" HeaderStyle-Width="80px" SortExpression="StartClaim" HeaderText="Start Claim"></asp:BoundField>
                  <asp:BoundField DataField="CurrCode" HeaderStyle-Width="80px" SortExpression="CurrCode" HeaderText="Currency"></asp:BoundField>
                  <asp:BoundField DataField="AmountLoan" HeaderStyle-Width="80px" SortExpression="AmountLoan" HeaderText="Amount Loan"></asp:BoundField>
                  <asp:BoundField DataField="Interest" HeaderStyle-Width="80px" SortExpression="Interest" HeaderText="Interest(%)/Year"></asp:BoundField>
                  <asp:BoundField DataField="TotalBunga" HeaderStyle-Width="80px" SortExpression="TotalBunga" HeaderText="Amount Interest"></asp:BoundField>
                  <asp:BoundField DataField="TotalPay" HeaderStyle-Width="80px" SortExpression="TotalPay" HeaderText="Payment"></asp:BoundField>
                  <asp:BoundField DataField="QtyPeriod" HeaderStyle-Width="80px" SortExpression="QtyPeriod" HeaderText="Qty Term Period"></asp:BoundField>
                  <asp:BoundField DataField="TermPeriod" HeaderStyle-Width="80px" SortExpression="TermPeriod" HeaderText="Term Period"></asp:BoundField>
                  <asp:BoundField DataField="Angsuran" HeaderStyle-Width="80px" SortExpression="Angsuran" HeaderText="Installment"></asp:BoundField>
                  <asp:BoundField DataField="Remark" HeaderStyle-Width="250px" HeaderText="Remark"></asp:BoundField>
              </Columns>
          </asp:GridView>
          </div>
            <asp:Panel runat="server" ID ="pnlNav" Visible="false">
            <asp:Button class="bitbtn btnadd" runat="server" ID="btnAdd2" Text="Add" />
            &nbsp &nbsp &nbsp
            <asp:DropDownList ID="ddlCommand2" CssClass="DropDownList" runat="server"/>
            <asp:Button class="btngo" runat="server" ID="btnGo2" Text="G"/>  
            </asp:Panel>
    </asp:Panel>    
    <asp:Panel runat="server" ID="pnlInput" Visible="false">
      <table>
        <tr>
            <td>Employee Loan No</td>
            <td>:</td>
            <td><asp:TextBox CssClass="TextBoxR" runat="server" ID="tbCode" Width="150px" Enabled="False"/>
                &nbsp;Rev :
                <asp:Label ID="lbRevisi" runat="server"></asp:Label>
            </td>        
            <td>Eployee Loan Date</td>
            <td>:</td>
            <td>
            <BDP:BasicDatePicker ID="tbDate" runat="server" DateFormat="dd MMM yyyy" 
                        ReadOnly = "true" ValidationGroup="Input"
                        ButtonImageHeight="19px" ButtonImageWidth="20px" 
                        DisplayType="TextBoxAndImage" 
                        TextBoxStyle-CssClass="TextDate" 
                        ShowNoneButton="False"><TextBoxStyle CssClass="TextDate" /></BDP:BasicDatePicker>                
            </td>           
        </tr> 
        <tr>
            <td>Payment Type</td>
            <td>:</td>
            <td colspan="4">
                <asp:DropDownList ID="ddlLoanType" runat="server" AutoPostBack="true" 
                    CssClass="DropDownList" Height="16px" ValidationGroup="Input" Width="90px">
                    <asp:ListItem>Angsuran</asp:ListItem>
                    <asp:ListItem>Berjangka</asp:ListItem>
                </asp:DropDownList>
                <asp:Label ID="label2" runat="server" ForeColor="#FF3300" Text="*"></asp:Label>
            </td>
        </tr>
          <tr>
              <td>Job Level</td>
              <td>:</td>
              <td colspan="4">
                  <asp:DropDownList ID="ddlJobLevel" runat="server" AutoPostBack="true" 
                      CssClass="DropDownList" Height="17px" ValidationGroup="Input" Width="241px">
                  </asp:DropDownList>
                  <asp:Label ID="label1" runat="server" Text = "*" ForeColor="#FF3300"></asp:Label>
              </td>
          </tr>
          <tr>
              <td>Employee</td>
              <td>:</td>
              <td colspan="4">
                  <asp:TextBox ID="tbEmpNo" runat="server" AutoPostBack="true" CssClass="TextBox" MaxLength="15" ValidationGroup="Input" />
                  <asp:TextBox ID="tbEmpName" runat="server" CssClass="TextBoxR" Enabled="False" 
                      MaxLength="100" Width="349px" />
                  <asp:Button ID="btnEmp" runat="server" Class="btngo" Text="..." ValidationGroup="Input" />
                  <asp:Label ID="label3" runat="server" ForeColor="#FF3300" Text="*"></asp:Label>
              </td>
          </tr>
        <tr>
            <td>Loan Type</td>
            <td>:</td>
            <td>
                <asp:DropDownList ID="ddlLoan" runat="server" CssClass="DropDownList" 
                    Height="16px" ValidationGroup="Input" Width="196px">
                </asp:DropDownList>
                <asp:Label ID="label4" runat="server" ForeColor="#FF3300" Text="*"></asp:Label>
            </td>
            <td>
                Start Claim</td>
            <td>
                :</td>
            <td>
                <BDP:BasicDatePicker ID="tbStartClaim" runat="server" ButtonImageHeight="19px" 
                    ButtonImageWidth="20px" DateFormat="dd MMM yyyy" DisplayType="TextBoxAndImage" 
                    ReadOnly="true" ShowNoneButton="False" TextBoxStyle-CssClass="TextDate" 
                    ValidationGroup="Input">
                    <TextBoxStyle CssClass="TextDate" />
                </BDP:BasicDatePicker>
                <asp:Label ID="label5" runat="server" ForeColor="#FF3300" Text="*"></asp:Label>
            </td>
        </tr> 
        <tr> 
            <td>Amount</td>
            <td>:</td>
            <td colspan="4">
                <table>
                    <tr align = "center">
                        <td bgcolor="#CCCCCC">Currency</td>
                        <td bgcolor="#CCCCCC">Loan<asp:Label ID="label6" runat="server" ForeColor="#FF3300" 
                                Text="*"></asp:Label>
                        </td>
                        <td bgcolor="#CCCCCC">Interest(%)/Year<asp:Label ID="label7" runat="server" 
                                ForeColor="#FF3300" Text="*"></asp:Label>
                        </td>
                        <td bgcolor="#CCCCCC">Interest</td>
                        <td bgcolor="#CCCCCC">Payment</td>
                    </tr>
                    <tr>
                        <td><asp:DropDownList ID="ddlCurrency" runat="server" CssClass="DropDownList" Height="16px" Enabled = "False" Width="55px">
                            </asp:DropDownList>
                        </td>
                        <td><asp:TextBox ID="tbAmountLoan" runat="server" CssClass="TextBox" Height="20px" ValidationGroup="Input" Width="104px" />
                        </td>
                        <td><asp:TextBox ID="tbInterest" runat="server" CssClass="TextBox" Height="20px" ValidationGroup="Input" Width="104px" />
                        </td>
                        <td><asp:TextBox ID="tbAmountInterest" runat="server" CssClass="TextBoxR" Height="20px" Width="104px" Enabled="False"/>
                        </td>
                        <td><asp:TextBox ID="tbTotalPembayaran" runat="server" CssClass="TextBoxR" Height="20px" Width="104px" Enabled="False"/>
                        </td>
                    </tr>
                </table>
            </td> 
        </tr>        
        <tr>
            <td><asp:Label ID="lbJangka" runat="server"></asp:Label>
            </td>
            <td>:</td>
            <td colspan="4">
                <asp:TextBox ID="tbQtyPeriod" runat="server" CssClass="TextBox" Enabled="False" Height="18px" Width="35px" />
                <asp:DropDownList ID="ddlTerm" runat="server" 
                    CssClass="DropDownList" ValidationGroup="Input" Width="100px" 
                    Height="19px" >
                    <asp:ListItem>Month</asp:ListItem>
                    <asp:ListItem>Half Month</asp:ListItem>
                    <asp:ListItem>Week</asp:ListItem>
                </asp:DropDownList>
                <asp:Label ID="label8" runat="server" ForeColor="#FF3300" Text="*"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>Installment</td>
            <td>:</td>
            <td colspan="4">
                <asp:TextBox ID="tbangsuran" runat="server" CssClass="TextBox" Height="20px" ValidationGroup="Input" Width="104px" />/Periode
                <asp:Label ID="label9" runat="server" ForeColor="#FF3300" Text="*"></asp:Label>
            </td>
        </tr>
        
        <tr>
            <td>Remark</td>
            <td>:</td>
            <td colspan="4"><asp:TextBox Width = "365px" runat="server" ValidationGroup="Input" ID="tbRemark" CssClass="TextBox" MaxLength="60"/>                                        
            &nbsp;&nbsp;&nbsp;       
            <asp:Button ID="btnGetData" runat="server" class="bitbtn btngetitem" 
                    Text="Get Data" />
            </td>
        </tr>
      </table>  
      
      <br />      
      <div style="font-size:medium; color:Blue;">Detail</div>
      <hr style="color:Blue" />  
        <asp:Panel runat="server" ID="pnlDt">
        
        <table>
            <tr>
            <td>            
            <asp:Panel ID="Panel1" runat="server" BorderColor="Black" BorderStyle="Solid" Height="100%" Width="160px">&nbsp;          
            &nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="Label11" runat="server" ForeColor="Blue" Text="Change Start Claim"></asp:Label>&nbsp;&nbsp;:&nbsp;&nbsp;<br />
            
            &nbsp;&nbsp;&nbsp;&nbsp;<asp:DropDownList ID="ddlAdjustType" runat="server" AutoPostBack="False" CssClass="DropDownList" Height="20px" Width="130px" >
                <asp:ListItem>Selected Only</asp:ListItem>
                <asp:ListItem>All Forward</asp:ListItem>
            </asp:DropDownList><br />
            &nbsp;&nbsp;&nbsp;&nbsp;Extend &nbsp;&nbsp;:&nbsp;
            <asp:TextBox ID="tbExtend" runat="server" AutoPostBack="False" 
                    CssClass="TextBox" Width="45px" /> Days<br />
            
            &nbsp;&nbsp;&nbsp;&nbsp;<asp:Button class="btngo" runat="server" ID="btnProcess" Text="OK" ValidationGroup="Input" Width="36px"/>       
            </asp:Panel>
            </td>
            </tr>
            </table><br />
            
            <asp:Button class="bitbtndt btnadd" runat="server" ID="btnAdddt" Text="Add" Visible = "false"/>                 
            <div style="border:0px  solid; overflow:auto;">
                <asp:GridView ID="GridDt" runat="server" AutoGenerateColumns="false" 
                    ShowFooter="True">
                    <HeaderStyle CssClass="GridHeader" />
                    <RowStyle CssClass="GridItem" Wrap="false" />
                    <AlternatingRowStyle CssClass="GridAltItem" />
                    <PagerStyle CssClass="GridPager" />
                    <Columns>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:CheckBox ID="cbSelectHd" runat="server" AutoPostBack="true" oncheckedchanged="cbSelectHd_CheckedChanged1" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="cbSelect" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Action">
                            <ItemTemplate>
                                <asp:Button ID="btnEdit" runat="server" class="bitbtndt btnedit" Text="Edit" CommandName="Edit" />
                                <%--<asp:Button ID="btnDelete" runat="server" class="bitbtndt btndelete" Text="Delete"
                                        OnClientClick="return confirm('Sure to delete this data?');" CommandName="Delete" />       --%>
                            </ItemTemplate>                                                                     
                        </asp:TemplateField>
                                               
                        <asp:BoundField DataField="Periode" HeaderStyle-Width="80px" HeaderText="Periode" />
                        <asp:BoundField DataField="StartClaim" HeaderText="Start Claim" dataformatstring="{0:dd MMM yyyy}" htmlencode="true" HeaderStyle-Width="80px" />
                        <asp:BoundField DataField="AmountBegin" HeaderStyle-Width="80px" HeaderText="Amount Beginning" />                        
                        <asp:BoundField DataField="AmountAngsur" HeaderStyle-Width="80px" HeaderText="Amount Installment" />
                        <asp:BoundField DataField="AmountPokok" HeaderStyle-Width="80px" HeaderText="Amount Base Loan" />
                        <asp:BoundField DataField="AmountBunga" HeaderStyle-Width="80px" HeaderText="Amount Interest" />
                        <asp:BoundField DataField="AmountEnd" HeaderStyle-Width="80px" HeaderText="Amount Ending" />
                        <asp:BoundField DataField="Remark" HeaderStyle-Width="200px" HeaderText="Remark" />
                         <asp:BoundField DataField="DonePaid" HeaderStyle-Width="200px" HeaderText="DonePaid" Visible = "false"/>                        
                    </Columns>
                </asp:GridView>
          </div>   
          <asp:Button class="bitbtndt btnadd" runat="server" ID="btnAddDt2" Text="Add" Visible = "false" />                  
       </asp:Panel>             
       <asp:Panel runat="server" DefaultButton="btnSaveDt" ID="pnlEditDt" Visible="false">
            <table>              
                <tr>
                    <td>Periode</td>
                    <td>:</td>
                    <td><asp:TextBox runat="server" ID="tbPeriode" CssClass="TextBoxR" Height="18px" Enabled="False" Width="69px"/></td>
                </tr>   
                <tr>
                    <td>Start Claim</td>
                    <td>:</td>
                    <td>
                        <BDP:BasicDatePicker ID="tbStartClaimDt" runat="server" ButtonImageHeight="19px" 
                            ButtonImageWidth="20px" DateFormat="dd MMM yyyy" DisplayType="TextBoxAndImage" 
                            ReadOnly="true" ShowNoneButton="False" TextBoxStyle-CssClass="TextDate" 
                            ValidationGroup="Input" Enabled = "false">
                            <TextBoxStyle CssClass="TextDate" />
                        </BDP:BasicDatePicker>
                    </td>
                </tr>                                                    
                <tr>
                    <td>Amount</td>
                    <td>:</td>
                    <td>
                       <table>
                            <tr align="center">
                                <td bgcolor="#CCCCCC">Beginning</td>
                                <td bgcolor="#CCCCCC">Installment<asp:Label ID="label10" runat="server" 
                                        ForeColor="#FF3300" Text="*"></asp:Label>
                                </td>
                                <td bgcolor="#CCCCCC">Base Loan</td>
                                <td bgcolor="#CCCCCC">Interest</td>
                                <td bgcolor="#CCCCCC">Ending</td>
                            </tr>
                            <tr>
                                <td><asp:TextBox ID="tbAmountbeginDt" runat="server" CssClass="TextBoxR" Height="20px" ValidationGroup="Input" Width="104px" Enabled="False"/></td>
                                <td><asp:TextBox ID="tbAmountAngsurDt" runat="server" CssClass="TextBox" Height="20px" ValidationGroup="Input" Width="104px" /></td>
                                <td><asp:TextBox ID="tbAmountPokokDt" runat="server" CssClass="TextBoxR" Height="20px" ValidationGroup="Input" Width="104px" Enabled="False"/></td>
                                <td><asp:TextBox ID="tbAmountBungaDt" runat="server" CssClass="TextBoxR" Height="20px" ValidationGroup="Input" Width="104px" Enabled="False"/></td>
                                <td><asp:TextBox ID="tbAmountEndDt" runat="server" CssClass="TextBoxR" Height="20px" ValidationGroup="Input" Width="104px" Enabled="False"/></td>
                            </tr>
                       </table>
                    </td>                    
                </tr>   
                <tr>
                    <td>Remark</td>
                    <td>:</td>
                    <td><asp:TextBox runat="server" ID="tbRemarkDt" CssClass="TextBox" ValidationGroup="Input" Width="365px" /> 
                        <asp:TextBox ID="tbDonePaid" runat="server" CssClass="TextBox" Visible = "false" Height="18px" Width="69px" />
                    </td>                    
                </tr>                
                                    
            </table>
            <br />                     
            <asp:Button ID="btnSaveDt" runat="server" class="bitbtndt btnsave" Text="Save" OnClientClick = "Confirm()"/> &nbsp;         
            <asp:Button ID="btnCancelDt" runat="server" class="bitbtndt btncancel" Text="Cancel"/> 
       </asp:Panel> 
       
       <br />          
       <asp:Button ID="btnSaveAll" runat="server" class="bitbtndt btnsave" 
            Text="Save & New" ValidationGroup="Input" Width="97px"/> &nbsp;    
        <asp:Button ID="btnSaveTrans" runat="server" class="bitbtndt btnsave" Text="Save" ValidationGroup="Input"/> &nbsp;    
        <asp:Button ID="btnBack" runat="server" class="bitbtndt btncancel" Text="Cancel" ValidationGroup="Input"/>  &nbsp;
        <asp:Button ID="btnHome" runat="server" class="bitbtndt btncancel" Text="Home"/> 
    </asp:Panel>
    
    </div>     
    <asp:Label runat ="server" ID="lbStatus" ForeColor="Red"/>
    </form>
</body>
</html>
