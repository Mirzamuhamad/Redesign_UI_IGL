<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TrPYTerminateProcess.aspx.vb" Inherits="Transaction_TrPYTerminateProcess_TrPYTerminateProcess" %>
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
    <style type="text/css">
    #UpdatePanel1, #UpdatePanel2, #UpdateProgress1 { 
      border-right: gray 1px solid; border-top: gray 1px solid; 
      border-left: gray 1px solid; border-bottom: gray 1px solid;
    }
    #UpdatePanel1, #UpdatePanel2 { 
      width:200px; height:200px; position: relative;
      float: left; margin-left: 10px; margin-top: 10px;
     }
     #UpdateProgress1 {
      width: 400px; background-color: #FFC080; 
      bottom: 0%; left: 0px; position: absolute;
     }
    </style>
    
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
    </head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server" />
            <script type="text/javascript">
                var prm = Sys.WebForms.PageRequestManager.getInstance();
                prm.add_initializeRequest(InitializeRequest);
                prm.add_endRequest(EndRequest);
                var postBackElement;
                function InitializeRequest(sender, args) {
                    if (prm.get_isInAsyncPostBack()) {
                        args.set_cancel(true);
                    }
                    postBackElement = args.get_postBackElement();
                    if (postBackElement.id == 'ButtonTrigger') {
                        $get('UpdateProgress1').style.display = "block";
                    }
                }
                function EndRequest(sender, args) {
                    if (postBackElement.id == 'ButtonTrigger') {
                        $get('UpdateProgress1').style.display = "none";
                    }
                }
                function AbortPostBack() {
                    if (prm.get_isInAsyncPostBack()) {
                        prm.abortPostBack();
                    }
                }
            </script>
           <%-- <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel2" >
            <ProgressTemplate >
              Update in progress...
              <input type="button" value="stop" onclick="AbortPostBack()" />
            </ProgressTemplate>
            </asp:UpdateProgress>
            <asp:Button ID="ButtonTrigger" runat="server" Text="Refresh Panel 1" OnClick="Button_Click" />    
            <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server">--%>
            <%--<ContentTemplate>
            <%=DateTime.Now.ToString() %> <br />
            The trigger for this panel 
            causes the UpdateProgress to be displayed
            even though the UpdateProgress is associated
            with panel 2.     
            <br />
            </ContentTemplate>--%>
            <%--<Triggers>
              <asp:AsyncPostBackTrigger ControlID="ButtonTrigger" />
            </Triggers>
            </asp:UpdatePanel>
            <asp:UpdatePanel ID="UpdatePanel2" UpdateMode="Conditional" runat="server">
            <ContentTemplate>
            <%=DateTime.Now.ToString() %> <br />
            <asp:Button ID="Button2" runat="server" Text="Refresh Panel" OnClick="Button_Click"/>    
            </ContentTemplate>
            </asp:UpdatePanel>--%>
            
    
    
    
    <div class="Content">
    <div class="H1">Payroll Terminate Process</div>
     <hr style="color:Blue" />        
      <table>
        <tr>
            <td style="width:120px;text-align:right">Period :</td>         
            <td>
                <asp:DropDownList ID="ddlYear" runat="server" CssClass="DropDownList" Width="60px" AutoPostBack ="true" />
                <asp:DropDownList ID="ddlMonth" runat="server" CssClass="DropDownList" Width="120px" AutoPostBack ="true" />               
            </td>
            <td style="width:100px;text-align:right">Terminate No :</td>         
            <td>                
                <asp:TextBox ID="tbTerminateNo" runat="server" CssClass="TextBox" Width="150px" Enabled="false"/>                               
                <asp:Button class="bitbtn btngo" runat="server" ID="btnProcess" Text="..." />
                <asp:TextBox runat="server" ID="tbStatus" Visible = "false"></asp:TextBox>
            </td>
            <td></td>
            </tr>
            <tr> 
            <td style="width:120px;text-align:right">Employee :</td>         
            <td colspan="3">
                <asp:TextBox ID="tbEmpNo" runat="server" CssClass="TextBox" Width="120px" Enabled="false"/>                               
                <asp:TextBox ID="tbEmpName" runat="server" CssClass="TextBox" Width="316px" Enabled="false"/>                               
            </td>
            <td>
                <asp:Button class="bitbtn btnpreview" Width="120px" runat="server" ID="btnCompute" Text="Compute Terminate" />
                <asp:Button class="bitbtn btnpreview" Width="120px" runat="server" ID="btnComplete" Text="Complete Terminate" />                
                <asp:Button class="bitbtn btnpreview" Width="120px" runat="server" ID="btnUnComplete" Text="Un-Posting" />                
            </td>
            </tr>   
                   
            <tr>
            <td style="width:120px;text-align:right">Hire & End Date :</td>         
            <td><asp:TextBox ID="tbHireDate" runat="server" CssClass="TextBox" Width="86px" Enabled="false"/>                                               
                <asp:TextBox ID="tbEndDate" runat="server" CssClass="TextBox" Width="86px" Enabled="false"/>                                               
                
            </td>  
            <td style="width:100px;text-align:right">PHK Reason :</td>         
            <td>                
                <asp:TextBox ID="tbPHKReason" runat="server" CssClass="TextBox" Width="150px" Enabled="false"/>                                               
            </td>
            <td></td>
            </tr>          
            <tr>
            <td style="width:120px;text-align:right">Total Process & Paid :</td>         
            <td><asp:TextBox ID="tbTotalProses" runat="server" CssClass="TextBox" Width="86px" Enabled="false"/>                                                               
                <asp:TextBox ID="tbTotalPaid" runat="server" CssClass="TextBox" Width="86px" Enabled="false"/>                                                               
            </td>  
            <td style="width:100px;text-align:right">Masa Kerja :</td>                         
            <td>                
               <asp:TextBox ID="tbMasaKerja" runat="server" CssClass="TextBox" Width="150px" Enabled="false"/>                                                
            </td>
            <td></td>
            </tr>          
        </table>      
          <br />  
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
                <asp:MenuItem Text="Result Salary" Value="0"></asp:MenuItem>
                <asp:MenuItem Text="Payment Loan" Value="1"></asp:MenuItem>
            </Items>            
        </asp:Menu>
        <hr />
        <asp:Panel ID="pnlData" runat="server" Visible="false">
        <asp:MultiView 
        ID="MultiView1"
        runat="server"
        ActiveViewIndex="0">
        <asp:View ID="Tab1" runat="server">
          <asp:Panel runat="server" id="pnlResult">          
          <div style="border:0px  solid; width:100%; height:100%; overflow:auto;">
          <asp:GridView ID="GridView1" runat="server" AllowPaging="True" AllowSorting="true" PageSize="50" 
            CssClass="Grid" AutoGenerateColumns="false"> 
              <HeaderStyle CssClass="GridHeader" Wrap = "false"></HeaderStyle>
						<RowStyle CssClass="GridItem" Wrap="false" />
						<AlternatingRowStyle CssClass="GridAltItem"/>
						<PagerStyle CssClass="GridPager" />
              <Columns>              
                  
                  <asp:BoundField HeaderText="Code" DataField="Code" HeaderStyle-Width="50" SortExpression="Code"  />
                  <asp:BoundField HeaderText="Description" DataField="Description" HeaderStyle-Width="200" SortExpression="Description"  />
                  <asp:BoundField HeaderText="Currency" DataField="Currency" HeaderStyle-Width="50" SortExpression="Currency"  />
                  <asp:BoundField HeaderText="Total" DataField="Total" HeaderStyle-Width="80" />
                  <asp:BoundField HeaderText="To Be Paid" DataField="AmountPaid" HeaderStyle-Width="80" />
                  <asp:BoundField DataField="StartDate" dataformatstring="{0:dd MMM yyyy}" htmlencode="true" HeaderStyle-Width="80px" HeaderText="Start Date" SortExpression="StartDate"/>
                  <asp:BoundField HeaderText="Pesangon" DataField="FgPesangon" HeaderStyle-Width="30" />                  
              </Columns>
          </asp:GridView>
          </div>   
          </asp:Panel>
          </asp:View>           
        <asp:View ID="Tab2" runat="server">
          <div style="border:0px  solid; width:100%; height:100%; overflow:auto;">
          <asp:GridView ID="GridLoan" runat="server" AllowPaging="True" AllowSorting="true" PageSize="50" 
            CssClass="Grid" AutoGenerateColumns="false"> 
              <HeaderStyle CssClass="GridHeader" Wrap = "false"></HeaderStyle>
						<RowStyle CssClass="GridItem" Wrap="false" />
						<AlternatingRowStyle CssClass="GridAltItem"/>
						<PagerStyle CssClass="GridPager" />
              <Columns>                                
                  <asp:BoundField HeaderText="Loan No" DataField="LoanNo" HeaderStyle-Width="120" SortExpression="LoanNo"  />
                  <asp:BoundField HeaderText="Revisi" DataField="Revisi" HeaderStyle-Width="20" SortExpression="Revisi"  />
                  <asp:BoundField HeaderText="Periode" DataField="Periode" HeaderStyle-Width="20" SortExpression="Periode"  />
                  <asp:BoundField DataField="ClaimDate" dataformatstring="{0:dd MMM yyyy}" htmlencode="true" HeaderStyle-Width="80px" HeaderText="Claim Date" SortExpression="ClaimDate"/>                  
                  <asp:BoundField HeaderText="Emp No" DataField="EmpNumb" HeaderStyle-Width="120" SortExpression="EmpNumb"  />
                  <asp:BoundField HeaderText="Emp Name" DataField="EmpName" HeaderStyle-Width="200" SortExpression="EmpName"  />
                  <asp:BoundField HeaderText="Currency" DataField="Currency" HeaderStyle-Width="80" SortExpression="Currency"  />
                  <asp:BoundField HeaderText="Amount" DataField="AmountAngsuran" HeaderStyle-Width="80" />                  
              </Columns>
          </asp:GridView>
          </div>                   
      </asp:View>                 
      </asp:MultiView> 
      </asp:Panel>
      <asp:Panel ID="pnlView" runat="server" Visible="false">
          <asp:GridView ID="GridForView" runat="server" AllowPaging="true" 
              AutoGenerateColumns="true" ShowFooter="True">
              <HeaderStyle CssClass="GridHeader" />
              <RowStyle CssClass="GridItem" Wrap="false" />
              <AlternatingRowStyle CssClass="GridAltItem" />
              <PagerStyle CssClass="GridPager" />
          </asp:GridView>
          <br />
          <table>
              <tr>
                  <td>
                      <asp:Label ID="lbMessage" runat="server" ForeColor="Blue" />
                  </td>
              </tr>
              <tr>
                  <td>
                      <asp:Button ID="btnYes" runat="server" CssClass="Button" Text="Yes" />
                      <asp:Button ID="btnNo" runat="server" CssClass="Button" Text="No" />
                  </td>
              </tr>
              <tr>
                  <td>
                      <asp:Button ID="btnReturn" runat="server" CssClass="Button" Text="Return" />
                  </td>
              </tr>
          </table>
        
    
    </asp:Panel>                      
    </div>
   
    <asp:Label runat ="server" ID="lbStatus" ForeColor="Red"/>
    </form>
</body>
</html>
