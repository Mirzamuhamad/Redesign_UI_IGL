<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TrVoucherApproval.aspx.vb" Inherits="ApprovalVoucher" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<%@ Register Assembly="BasicFrame.WebControls.BasicDatePicker" Namespace="BasicFrame.WebControls"
    TagPrefix="BDP" %>

<%@ Register assembly="CrystalDecisions.Web, Version=10.5.3700.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
     <link type="text/css" rel="stylesheet" href="../../Styles/circularprogress.css" />
    <script src="../../Function/OpenDlg.js" type="text/javascript"></script>
    <script type="text/javascript" src="../../Function/Function.js" ></script> 
    <script type="text/javascript" src="../../JQuery/jquery.min.js"></script>

      
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

        function OpenPopupSupplier() {
            var left = (screen.width - 600) / 2; //370
            var top = (screen.height - 600) / 2;
            window.open("../../earchDlgV.Aspx", "", 'toolbar=no, location=no, directories=no, status=no, menubar=no, scrollbars=yes, resizable=yes, copyhistory=no, width=' + 600 + ', height=' + 600 + ', top=' + top + ', left=' + left);
            return false;
        }

        function OpenPopup() {
            var left = (screen.width - 600) / 2; //370
            var top = (screen.height - 600) / 2;
            //window.open("../../earchDlgV.Aspx", "", 'toolbar=no, location=no, directories=no, status=no, menubar=no, scrollbars=yes, resizable=yes, copyhistory=no, width=' + 600 + ', height=' + 600 + ', top=' + top + ', left=' + left);
            window.open("../../FindDlg.Aspx", "", 'toolbar=no, location=no, directories=no, status=no, menubar=no, scrollbars=yes, resizable=yes, copyhistory=no, width=' + 600 + ', height=' + 600 + ', top=' + top + ', left=' + left);
            return false;
        }
    
        function setformat()
        {
        try
         {          
        var PpnRate = document.getElementById("tbPpnRate").value.replace(/\$|\,/g,"");
        var Rate = document.getElementById("tbRate").value.replace(/\$|\,/g,"");
        var PPn = document.getElementById("tbPPN").value.replace(/\$|\,/g,"");        
        var BaseForex = document.getElementById("tbBaseForex").value.replace(/\$|\,/g,"");
        var Disc = document.getElementById("tbDisc").value.replace(/\$|\,/g,"");
        var DiscForex = document.getElementById("tbDiscForex").value.replace(/\$|\,/g,"");
        var PPnForex = document.getElementById("tbPPNForex").value.replace(/\$|\,/g,"");
        var TotalForex = document.getElementById("tbTotalForex").value.replace(/\$|\,/g,"");
        
        document.getElementById("tbPpnRate").value = setdigit(PpnRate,'<%=ViewState("DigitRate")%>');
        document.getElementById("tbRate").value = setdigit(Rate,'<%=ViewState("DigitRate")%>');
        document.getElementById("tbPPN").value = setdigit(PPn,'<%=ViewState("DigitPercent")%>');
        document.getElementById("tbBaseForex").value = setdigit(BaseForex,'<%=VIEWSTATE("DigitCurr")%>');
        document.getElementById("tbDisc").value = setdigit(Disc,'<%=ViewState("DigitPercent")%>');
        document.getElementById("tbDiscForex").value = setdigit(DiscForex,'<%=VIEWSTATE("DigitCurr")%>');
        document.getElementById("tbPPNForex").value = setdigit(PPnForex,'<%=VIEWSTATE("DigitCurr")%>');
        document.getElementById("tbTotalForex").value = setdigit(TotalForex,'<%=VIEWSTATE("DigitCurr")%>');
        }catch (err){
            alert(err.description);
          }
      }
      
      
      function financial(x) {
          return Number.parseFloat(x).toFixed(2);
      }
      


      function setformatfordt(prmchange) {
          try {

              var ppn = document.getElementById("tbppn").value
              if (ppn == '') {
                  document.getElementById("tbppn").value = 0
              }

              document.getElementById("tbdpp").value = (parseFloat(document.getElementById("tbInvoice").value.replace(/\$|\,/g, "")) - parseFloat(document.getElementById("tbPotongan").value.replace(/\$|\,/g, "")));
              document.getElementById("tbppnValue").value = (parseFloat(document.getElementById("tbdpp").value.replace(/\$|\,/g, "")) * parseFloat(document.getElementById("tbppn").value.replace(/\$|\,/g, ""))) / 100;
              document.getElementById("tbPphValue").value = (parseFloat(document.getElementById("tbdpp").value.replace(/\$|\,/g, "")) * parseFloat(document.getElementById("tbpph").value.replace(/\$|\,/g, ""))) / 100 + 0.001;
              document.getElementById("tbTotalAmount").value = (parseFloat(document.getElementById("tbInvoice").value.replace(/\$|\,/g, "")) - parseFloat(document.getElementById("tbPotongan").value.replace(/\$|\,/g, ""))) + parseFloat(document.getElementById("tbppnValue").value.replace(/\$|\,/g, "")) - parseFloat(document.getElementById("tbPphValue").value.replace(/\$|\,/g, ""));

//              document.getElementById("tbppnValue").value = (parseFloat(document.getElementById("tbppnValue").value.replace(/\$|\,/g, "")).toFixed(2);
//              document.getElementById("tbPphValue").value = financial(document.getElementById("tbPphValue").value.replace(/\$|\,/g, ""));
//              document.getElementById("tbTotalAmount").value =  (parseFloat(document.getElementById("tbTotalAmount").value.replace(/\$|\,/g, "")).toFixed(2);
//       

              document.getElementById("tbTotalAmount").value = setdigit(document.getElementById("tbTotalAmount").value.replace(/\$|\,/g, ""), '<%=ViewState("DigitHome")%>');
              document.getElementById("tbppnValue").value = setdigit(document.getElementById("tbppnValue").value.replace(/\$|\,/g, ""), '<%=ViewState("DigitCurr")%>');
              document.getElementById("tbPphValue").value = setdigit(document.getElementById("tbPphValue").value.replace(/\$|\,/g, ""), '<%=ViewState("DigitCurr")%>');
              document.getElementById("tbInvoice").value = setdigit(document.getElementById("tbInvoice").value.replace(/\$|\,/g, ""), '<%=ViewState("DigitCurr")%>');
              document.getElementById("tbPotongan").value = setdigit(document.getElementById("tbPotongan").value.replace(/\$|\,/g, ""), '<%=ViewState("DigitCurr")%>');      
              document.getElementById("tbdpp").value = setdigit(document.getElementById("tbdpp").value.replace(/\$|\,/g, ""), '<%=ViewState("DigitCurr")%>');
              document.getElementById("tbppn").value = setdigit(document.getElementById("tbppn").value.replace(/\$|\,/g, ""), '<%=ViewState("DigitCurr")%>');
              document.getElementById("tbpph").value = setdigit(document.getElementById("tbpph").value.replace(/\$|\,/g, ""), '<%=ViewState("DigitCurr")%>');

          } catch (err) {
              alert(err.description);
          }
      } 

                       
       function setformatdt()
        {
        try
         {         
         var Qty = document.getElementById("tbQty").value.replace(/\$|\,/g,""); 
         var PriceForex = document.getElementById("tbPrice").value.replace(/\$|\,/g,"");
         var AmountForex = document.getElementById("tbAmountForex").value.replace(/\$|\,/g,"");        
        
         document.getElementById("tbQty").value = setdigit(Qty,'<%=ViewState("DigitQty")%>');
         document.getElementById("tbPrice").value = setdigit(PriceForex,'<%=VIEWSTATE("DigitCurr")%>');
         document.getElementById("tbAmountForex").value = setdigit(AmountForex,'<%=VIEWSTATE("DigitCurr")%>');
        }catch (err){
            alert(err.description);
          }
      }


      function myPopup() {
          var left = (screen.width - 370) / 2;
          var top = (screen.height - 800) / 2;
          window.open("../../earchDlgV.Aspx", "", 'toolbar=no, location=no, directories=no, status=no, menubar=no, scrollbars=yes, resizable=yes, copyhistory=no, width=' + 370 + ', height=' + 800 + ', top=' + top + ', left=' + left);
          return false;
      }


      function ProgressCircle() {
          setTimeout(function() {
              var modal = $('<div />');
              modal.addClass("modal");
              $('body').append(modal);
              var loading = $(".loading");
              loading.show();
              var top = Math.max($(window).height() / 2 - loading[0].offsetHeight / 2, 0);
              var left = Math.max($(window).width() / 2 - loading[0].offsetWidth / 2, 0);
              loading.css({ top: top, left: left });
          }, 200);
      }
      $('form').live("submit", function() {
          ProgressCircle();
      });
      
      
    </script>
    
       
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
 <link href="https://fonts.googleapis.com/css2?family=Muli:wght@300;400;600&display=swap" rel="stylesheet">
    
    
</head>
<body>     
    <form id="form1" runat="server">    
    <div class="Content">
    <div class="H1">Voucher Approval</div>
     <hr />        
     <asp:Panel runat="server" id="PnlHd"> 
      <table>
        <tr>
            <td style="width:100px;text-align:right">Quick Search :</td>            
            <td><asp:TextBox CssClass="TextBox" runat="server" ID ="tbFilter"/> 
                  <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlField" >
                      <asp:ListItem Value="TransNmbr" Selected="True">Reference</asp:ListItem>
                      <asp:ListItem>Status</asp:ListItem>
                      <asp:ListItem Value="dbo.FormatDate(TransDate)">Date</asp:ListItem>
                      <asp:ListItem Value="SuppCode">Supplier</asp:ListItem>
                      <asp:ListItem Value="SuppName">Supplier Name</asp:ListItem>
                      <asp:ListItem Value="TypeInvoice">Invoice Type</asp:ListItem>
                      <asp:ListItem Value="ExpenseType">Expense Type</asp:ListItem> 
                      <asp:ListItem>Remark</asp:ListItem>
                  </asp:DropDownList>
                  <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlRange"></asp:DropDownList>

                  <asp:Button class="bitbtn btnsearch" runat="server" ID="btnSearch" Text="Search" />																						 											  
                  <asp:Button class="bitbtn btngo" runat="server" ID="btnExpand" Text="..." />																						 											  
                 
                 
                
            </td>
            <td>
                <asp:LinkButton ID="LbAdvSearch" runat="server" Text="Advanced Search" />
                 &nbsp &nbsp &nbsp &nbsp
                <asp:Label runat="server" ID="Label1" Text="Outstanding Invoice : "/>
                <asp:LinkButton runat="server" ID="lbCount" Text="X"  Font-Size="Small" />
                <asp:Label runat="server" ID="Label2" Text=" record(s)"/>
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
              <asp:DropDownList ID="ddlField2" runat="server" CssClass="DropDownList">
                  
                  <asp:ListItem Value="dbo.FormatDate(TransDate)">Date</asp:ListItem>
                      <asp:ListItem Value="TransNmbr" Selected="True">Reference</asp:ListItem>
                      <asp:ListItem>Status</asp:ListItem>
                      <asp:ListItem Value="dbo.FormatDate(TransDate)">Date</asp:ListItem>
                      <asp:ListItem Value="SuppCode">Supplier</asp:ListItem>
                      <asp:ListItem Value="SupplierName">Supplier Name</asp:ListItem>
                      <asp:ListItem Value="TypeInvoice">Invoice Type</asp:ListItem>
                      <asp:ListItem Value="ExpenseType">Expense Type</asp:ListItem> 
                      <asp:ListItem>Remark</asp:ListItem>
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
                              <asp:ListItem Text="Print" />
                          </asp:DropDownList>
                          
                          <asp:Button class="btngo" runat="server" ID="BtnGo" Text="G" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" CommandName="Go" />
                          
                      </ItemTemplate>
                      <HeaderStyle Width="110px" />
                  </asp:TemplateField>
                  <asp:BoundField DataField="TransNmbr" HeaderStyle-Width="120px" SortExpression="Nmbr" HeaderText="Reference"></asp:BoundField>                  
                  <asp:BoundField DataField="Status" HeaderText="Status"></asp:BoundField>                  
                  <asp:BoundField DataField="TransDate" dataformatstring="{0:dd MMM yyyy}" htmlencode="true" HeaderStyle-Width="80px" SortExpression="TransDate" HeaderText="Date"></asp:BoundField>
                  <asp:BoundField DataField="SuppName" HeaderStyle-Width="150px" SortExpression="SuppName" HeaderText="Supplier"></asp:BoundField>
                  <asp:BoundField DataField="Attn" HeaderStyle-Width="100px" SortExpression="Attn" HeaderText="Attn"></asp:BoundField>
                  <asp:BoundField DataField="InvoiceType" HeaderStyle-Width="120px" SortExpression="InvoiceType" HeaderText="Invoice Type"></asp:BoundField>
                  
                  <asp:BoundField DataField="BankPenerima" HeaderStyle-Width="150px" SortExpression="BankPenerima" HeaderText="Bank Penerima"></asp:BoundField>
                  <asp:BoundField DataField="NamaPenerima" HeaderStyle-Width="100px" SortExpression="NamaPenerima" HeaderText="Nama Penerima"></asp:BoundField>
                  <asp:BoundField DataField="AccNmbr" HeaderStyle-Width="120px" SortExpression="AccNmbr" HeaderText="Account"></asp:BoundField>
                  
                  <asp:BoundField DataField="TotalInvoice" DataFormatString="{0:#,##0.00}" ItemStyle-HorizontalAlign="Right"  HeaderStyle-Width="80px" SortExpression="TotalInvoice" HeaderText="Total Invoice"></asp:BoundField>
                  
                  <asp:BoundField DataField="Remark" HeaderStyle-Width="120px" SortExpression="Remark" HeaderText="Remark"></asp:BoundField>
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
            <td>Voucher No</td>
            <td>:</td>
            <td><asp:TextBox CssClass="TextBoxR" runat="server" ID="tbRef" Width="225px" Enabled="False"/>        
            </td> 
            
            
            <td>Perkiraan Bayar</td>
            <td>:</td>
            <td>    
                <BDP:BasicDatePicker ID="tbPerkiraanBayar" runat="server" DateFormat="dd MMM yyyy" 
                        ReadOnly = "true" ValidationGroup="Input"
                        ButtonImageHeight="19px" ButtonImageWidth="20px" 
                        DisplayType="TextBoxAndImage" 
                        TextBoxStyle-CssClass="TextDate" AutoPostBack="True" 
                        ShowNoneButton="False"><TextBoxStyle CssClass="TextDate" /></BDP:BasicDatePicker>            
            </td>            
            
                       
        </tr>
        
        
        <tr>
        <td>Voucher Date</td>
            <td>:</td>
            <td>    
                <BDP:BasicDatePicker ID="tbDate" runat="server" DateFormat="dd MMM yyyy" 
                        ReadOnly = "true" ValidationGroup="Input"
                        ButtonImageHeight="19px" ButtonImageWidth="20px" 
                        DisplayType="TextBoxAndImage" 
                        TextBoxStyle-CssClass="TextDate" AutoPostBack="True" 
                        ShowNoneButton="False"><TextBoxStyle CssClass="TextDate" /></BDP:BasicDatePicker>            
            </td> 
            
            <td><asp:LinkButton ID="lblExpense" Visible="True" ValidationGroup="Input"  runat="server" Text="Expense Type"/></td>
            <td>:</td>
            <td>
            <asp:TextBox runat="server" ValidationGroup="Input" ID="tbExpense" CssClass="TextBox" Visible="false" Width="225px"/>
                        <asp:TextBox runat="server" ValidationGroup="Input" ID="tbExpenseName" CssClass="TextBox" Enabled="false"  Width="192px"/>
                         <asp:Button ID="btnExpense" runat="server" Class="btngo" visible = True Text="v" />
                         
                    <asp:DropDownList ID="ddlExpenseType" ValidationGroup="Input" Width="230px" Visible="false" runat="server" CssClass="DropDownList" >       
                    </asp:DropDownList>
            </td> 
            
        </tr>
        
        <tr>
        
        <td>Invoice Type</td>
            <td>:</td>
            <td>
                    <asp:DropDownList ID="ddlInvoiceType" ValidationGroup="Input" Width="230px" AutoPostBack="true"  runat="server" CssClass="DropDownList" > 
                        <asp:ListItem Selected="True" >Invoice</asp:ListItem>
                        <asp:ListItem>Expense</asp:ListItem>
                        <asp:ListItem>PB</asp:ListItem>                        
                    </asp:DropDownList>
            </td> 
            
              <td>Bank Penerima</td>
            <td>:</td>
            <td><asp:TextBox runat="server" ValidationGroup="Input" ID="tbBankPenerima" CssClass="TextBox" Width="225px"/></td>
            
                      
        </tr>

 
        <tr>
            <td><asp:LinkButton ID="lbSupp" Visible="True" ValidationGroup="Input"  runat="server" Text="Supplier"/></td>
            <td>:</td>
            <td>
                <asp:TextBox CssClass="TextBox" ValidationGroup="Input" runat="server" ID="tbSuppCode" Width="100px" AutoPostBack="true" />
                <asp:TextBox CssClass="TextBox" runat="server" ID="tbSuppName" Enabled="false" Width="190px"/>
                <asp:TextBox CssClass="TextBox" runat="server" ID="tbSuppType" Visible="false" Enabled="false" Width="190px"/>
                <asp:Button Class="btngo" ID="btnSupp" Text="..." runat="server" ValidationGroup="Input" />                                  
            </td>
             <td>Nama Penerima</td>
            <td>:</td>
            <td><asp:TextBox runat="server" ValidationGroup="Input" ID="tbNamaPenerima" CssClass="TextBox" Width="225px"/></td>
            
        </tr>
            
            
        </tr>
        
        
        <tr>
        
            <td>Attn</td>
            <td>:</td>
            <td><asp:TextBox runat="server" ValidationGroup="Input" ID="tbAttn" CssClass="TextBox" Width="225px"/>
            <asp:Button class="bitbtn btnsearch" runat="server" ID="btnGetInv" Text="Get Invoice" /></td>
           
            
            <td>Account</td>
            <td>:</td>
            <td><asp:TextBox runat="server" ValidationGroup="Input" ID="tbAccount"  CssClass="TextBox" Width="225px"/></td>
            
            
        </tr>
        <asp:Panel runat="server" ID="pnlPph" Visible ="false">
                            <tr>
                                <td>Pph</td>
                                <td>:</td>
                                <td>
                                <asp:DropDownList CssClass="DropDownList" Width="212px" ValidationGroup="Input" Visible="True" runat="server" ID="ddlpph" AutoPostBack="True" />
                                <asp:TextBox runat="server" ValidationGroup="Input" ID="tbType" CssClass="TextBox" Width="10px" Enabled = "False"/>

                            </td>
        
         </asp:Panel>      
                             
        
        
        
        
                    <tr>
                            <td>Total Invoice</td>
                            <td>:</td>
                            <td colspan="7">
                                <table>
                                    <tr style="background-color:Silver;text-align:center; border-radius:30px;">
                                        <td>Total Invoice</td>
                                        <td>Total Potongan</td>
                                        <td>Total DPP   </td>                                        
                                        <td>Total PPN</td>
                                        <td>Total PPH</td>
                                        <td>Total Amount</td>
                                    </tr>
                                    <tr>
                                        <td><asp:TextBox ID="TbTotalInvoiceHd" ValidationGroup="Input" runat="server" Enabled = "False" CssClass="TextBox" width="120px"/></td>
                                        <td><asp:TextBox ID="tbTotalPotongan" ValidationGroup="Input" runat="server" Enabled = "false" CssClass="TextBox" width="120px"/></td>
                                       <td><asp:TextBox ID="tbTotalDpp" ValidationGroup="Input" runat="server" Enabled = "false" CssClass="TextBox" width="120px"/></td>
                                        <td><asp:TextBox ID="TbtotalPPN"  ValidationGroup="Input" runat="server" Enabled = "false" CssClass="TextBox" Width="120px"/></td> 
                                        <td><asp:TextBox ID="TbTotalPPH"  ValidationGroup="Input" runat="server" Enabled = "false" CssClass="TextBox" Width="120px"/></td>
                                         <td><asp:TextBox ID="tbTotalInvoice"  ValidationGroup="Input" runat="server" Enabled = "false" CssClass="TextBox" Width="120px"/></td>                          
                                    </tr>
                                </table>
                            </td>                
                    </tr>
        
        
        <%--<tr>
            <td><asp:LinkButton ID="lbTerm" ValidationGroup="Input" runat="server" Text="Term"/></td>
            <td>:</td>
            <td><asp:DropDownList CssClass="DropDownList" ValidationGroup="Input" runat="server" ID="ddlTerm" AutoPostBack="true" /> 
                <BDP:BasicDatePicker ID="tbDueDate" runat="server" DateFormat="dd MMM yyyy" 
                        ReadOnly = "true" Enabled="false"
                        ButtonImageHeight="19px" ButtonImageWidth="20px" 
                        DisplayType="TextBox" 
                        TextBoxStyle-CssClass="TextDate" AutoPostBack="True" 
                        ShowNoneButton="False"><TextBoxStyle CssClass="TextDate" /></BDP:BasicDatePicker>
            </td>                    
        </tr>     --%>     
        
      <%--  <tr>
            <td>Supp Inv No & Date</td>
            <td>:</td>
            <td><asp:TextBox runat="server" ValidationGroup="Input" ID="tbSuppInvNo" CssClass="TextBox" Width="150px"/>
            <BDP:BasicDatePicker ID="tbSuppInvDate" runat="server" DateFormat="dd MMM yyyy" 
                        ReadOnly = "true" ValidationGroup="Input"
                        ButtonImageHeight="19px" ButtonImageWidth="20px" 
                        DisplayType="TextBoxAndImage" 
                        TextBoxStyle-CssClass="TextDate" AutoPostBack="True" ShowNoneButton="False"><TextBoxStyle CssClass="TextDate" /></BDP:BasicDatePicker>                                 
            </td>
        </tr>--%>
        <%--<tr>
            <td>PPn</td>
            <td>:</td>
            <td colspan="3">
                <table>
                <tr style="background-color:Silver;text-align:center">
                   <td>PPn No</td>
                   <td>PPn Date</td>
                   <td>PPn Rate</td>
                 </tr>
                 <tr>
                 <td>
                 <asp:TextBox runat="server" ValidationGroup="Input" ID="tbPPnNo" CssClass="TextBox" Width="150px"/>
                 </td>
                 <td>
                 <BDP:BasicDatePicker ID="tbPPndate" runat="server" DateFormat="dd MMM yyyy" 
                            ReadOnly = "true" ValidationGroup="Input"
                            ButtonImageHeight="19px" ButtonImageWidth="20px" 
                            DisplayType="TextBoxAndImage" 
                            TextBoxStyle-CssClass="TextDate" AutoPostBack="True" ShowNoneButton="False"><TextBoxStyle CssClass="TextDate" /></BDP:BasicDatePicker>                
                 </td>
                 <td>
                 <asp:TextBox runat="server" ValidationGroup="Input" ID="tbPpnRate" CssClass="TextBox" Width="68px"/>
                 </td>
                 </tr>
                 </table>
            </td>                         
        </tr> --%>
        <%--<tr>
                <td><asp:LinkButton ID="lbCurr" ValidationGroup="Input" runat="server" Text="Currency"/></td>
                <td>:</td>
                <td><asp:DropDownList ID="ddlCurr" ValidationGroup="Input" AutoPostBack="true" runat="server" CssClass="DropDownList" />                                                                   
                &nbsp Rate : &nbsp <asp:TextBox ID="tbRate" ValidationGroup="Input" runat="server" CssClass="TextBox" Width="68px" />
                </td>
        </tr> --%> 
       <%-- <tr>
                <td>Total</td>
                <td>:</td>
                <td colspan="6">
                    <table>
                        <tr style="background-color:Silver;text-align:center">
                            <td>Base Forex</td>
                            <td>Disc %</td>
                            <td>Disc Forex</td>
                            <td>PPN %</td>
                            <td>PPN Forex</td>
                            <td>Total Forex</td>
                        </tr>
                        <tr>
                            <td><asp:TextBox ID="tbBaseForex" runat="server" CssClass="TextBoxR" Width="90px"/></td>
                            <td><asp:TextBox ID="tbDisc" runat="server" CssClass="TextBox" width="40px" 
                                    ValidationGroup="Input"/></td>
                            <td><asp:TextBox ID="tbDiscForex" ValidationGroup="Input" runat="server" CssClass="TextBox" Width="90px"/></td>
                            <td><asp:TextBox ID="tbPPN" ValidationGroup="Input" runat="server" CssClass="TextBox" width="40px"/></td>
                            <td><asp:TextBox ID="tbPPNForex" runat="server" CssClass="TextBoxR" Width="90px"/></td>
                            <td><asp:TextBox ID="tbTotalForex" runat="server" CssClass="TextBoxR" Width="90px"/></td>
                        </tr>
                    </table>
                </td>                
        </tr> --%>          
        
        
        <tr>
            <td>Remark</td>
            <td>:</td>
            <td><asp:TextBox runat="server" ValidationGroup="Input" ID="tbRemark" CssClass="TextBoxMulti" Width="400px" TextMode="MultiLine" MaxLength="255"/></td>
            
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
                <%-- <asp:MenuItem Text="Detail Invoice" Value="0"></asp:MenuItem> --%>
                <asp:MenuItem Text="Detail Invoice" Value="1"></asp:MenuItem>                   
            </Items>            
        </asp:Menu>
        <hr /> 
      
        <asp:Panel runat="server" ID="pnlDt">
            <asp:Button class="bitbtn btnadd" runat="server" ID="btnAddDt" Text="Add" ValidationGroup="Input" />	
            
            <div style="border:0px  solid; width:100%; height:100%; overflow:auto;">
                <asp:GridView ID="GridDt" runat="server" AutoGenerateColumns="false" 
                    ShowFooter="False"  Wrap="true">
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
                            <EditItemTemplate>
                            	<asp:Button ID="btnUpdate" runat="server" class="bitbtndt btnsave" Text="Save" CommandName="Update"/>									
                                <asp:Button ID="btnCancel" runat="server" class="bitbtndt btncancel" Text="Cancel" CommandName="Cancel"/>									
                                
                            </EditItemTemplate>
                            
                            <%--<FooterTemplate>
                            <asp:ImageButton ID="btnAddDt2" ValidationGroup="Input" runat="server"  
                                    ImageUrl="../../Image/btnAddDtOn.png"
                                    onmouseover="this.src='../../Image/btnAddDtOff.png';"
                                    onmouseout="this.src='../../Image/btnAddDtOn.png';"
                                    ImageAlign="AbsBottom" />        
                            </FooterTemplate>--%>                            
                        </asp:TemplateField>
                            <asp:BoundField DataField="ItemNo" HeaderText="No Item" />
                            <asp:BoundField DataField="InvoiceNo" HeaderText="Invoice No" />
                            <asp:BoundField DataField="InvoiceType" HeaderText="From Invoice" />
                            <asp:BoundField DataField="InvoiceDate" dataformatstring="{0:dd MMM yyyy}" htmlencode="true" HeaderStyle-Width="80px" SortExpression="InvoiceDate" HeaderText="Invoice Date"></asp:BoundField>
                  
                            <asp:BoundField DataField="PONo" HeaderStyle-Width="180px" HeaderText="Reference No" />                           
                            <asp:BoundField DataField="Invoice"  ItemStyle-HorizontalAlign="Right"  HeaderStyle-Width="80px" DataFormatString="{0:#,##0.00}" HeaderText="Invoice" />
                            <asp:BoundField DataField="Potongan"  ItemStyle-HorizontalAlign="Right"  HeaderStyle-Width="80px" DataFormatString="{0:#,##0.00}" HeaderText="Potongan" />
                            <asp:BoundField DataField="Dpp"  ItemStyle-HorizontalAlign="Right"  HeaderStyle-Width="80px" DataFormatString="{0:#,##0.00}" HeaderText="DPP" />
                            <asp:BoundField DataField="PPn"  ItemStyle-HorizontalAlign="Right"  HeaderStyle-Width="80px" DataFormatString="{0:#,##0.00}" HeaderText="PPn (%)" />
                            <asp:BoundField DataField="PPNInvoice"  ItemStyle-HorizontalAlign="Right"  HeaderStyle-Width="80px" DataFormatString="{0:#,##0.00}" HeaderText="PPN Value" />
                            <asp:BoundField DataField="PPh"  ItemStyle-HorizontalAlign="Right"  HeaderStyle-Width="80px" DataFormatString="{0:#,##0.00}" HeaderText="PPh (%)" />
                            <asp:BoundField DataField="PPHInvoice"  ItemStyle-HorizontalAlign="Right"  HeaderStyle-Width="80px" DataFormatString="{0:#,##0.00}" HeaderText="PPH Value" />
                            <asp:BoundField DataField="TotalAmount"  ItemStyle-HorizontalAlign="Right"  HeaderStyle-Width="80px" DataFormatString="{0:#,##0.00}" HeaderText="Total Amount" />
                            <asp:BoundField DataField="Remark" HeaderStyle-Width="200px" HeaderText="Remark" />
                    </Columns>
                </asp:GridView>
          </div>   
          <asp:Button class="bitbtn btnadd" runat="server" ID="btnAddDt2" Text="Add" ValidationGroup="Input" />	
          
                
       </asp:Panel>       
       <asp:Panel runat="server" ID="pnlEditDt" Visible="false">
            <table>                                
                
                <tr>
                        <td>Item No</td>
                        <td>:</td>
                        <td><asp:Label ID="lbItemNo" runat="server" Text="Item" />
                        </td>           
                    </tr>       
                     
                    <tr>                    
                        <td>Invoice No</td>
                        <td>:</td>
                        <td>                             
                            <asp:TextBox CssClass="TextBox" runat="server" ID="tbInvoiceNo" Enabled="true" Width="225px"/>                                          
                        </td>
                    </tr>
                    
                    <tr>                    
                        <td>Invoice Date</td>
                        <td>:</td>
                        <td>                             
                            <BDP:BasicDatePicker ID="tbInvoiceDate" runat="server" DateFormat="dd MMM yyyy" 
                        ReadOnly = "true" ValidationGroup="Input"
                        ButtonImageHeight="19px" ButtonImageWidth="20px" 
                        DisplayType="TextBoxAndImage" 
                        TextBoxStyle-CssClass="TextDate" AutoPostBack="True" 
                        ShowNoneButton="False"><TextBoxStyle CssClass="TextDate" /></BDP:BasicDatePicker>                                         
                        </td>
                    </tr>
                    
                    <tr>
                        <td>Reference No</td>
                        <td>:</td>
                        <td>                                
                             <asp:TextBox CssClass="TextBox" runat="server" ID="tbReference" Enabled="true" Width="225px"/>                                         
                            
                        </td>
                    </tr>
                    
                    <tr>
                            <td>Nominal</td>
                            <td>:</td>
                            <td colspan="7">
                                <table>
                                    <tr style="background-color:Silver;text-align:center; border-radius:30px;">
                                        <td>Invoice</td>
                                        <td>Potongan</td>
                                        <td>DPP   </td>
                                        <td>PPN %</td>
                                        <td>PPN Value</td>
                                        <td>PPH %</td>
                                        <td>PPH Value</td>
                                        <td>Total Amount </td>
                                    </tr>
                                    <tr>
                                        <td><asp:TextBox ID="tbInvoice" ValidationGroup="Input" runat="server" CssClass="TextBox" width="120px"/></td>
                                        <td><asp:TextBox ID="tbPotongan" ValidationGroup="Input" runat="server" CssClass="TextBox" width="120px"/></td>
                                        <td><asp:TextBox ID="tbdpp" ValidationGroup="Input" runat="server" CssClass="TextBox" width="120px"/></td>
                                        <td><asp:TextBox ID="tbppn" ValidationGroup="Input" runat="server" CssClass="TextBox" width="50px"/></td>
                                        <td><asp:TextBox ID="tbppnValue"  ValidationGroup="Input" runat="server" CssClass="TextBox" Width="120px"  /></td>
                                        <td><asp:TextBox ID="tbpph"  ValidationGroup="Input" runat="server" Enabled = "True" CssClass="TextBox" Width="50px"/></td> 
                                        <td><asp:TextBox ID="tbPphValue"  ValidationGroup="Input" runat="server" Enabled = "True" CssClass="TextBox" Width="120px"/></td>  
                                        <td><asp:TextBox ID="tbTotalAmount"  ValidationGroup="Input" runat="server"  CssClass="TextBoxR" Width="120px"/></td>                          
                                    </tr>
                                </table>
                            </td>                
                    </tr> 
                    
                    
                    <tr>
                        <td>Remark </td>
                        <td>:</td>
                        <td><asp:TextBox runat="server" ID="tbRemarkDt" CssClass="TextBox" Width="365px" 
                                MaxLength="255" TextMode="MultiLine" />                        
                        </td>
                    </tr>
                                      
            </table>
            <br />    
            
    		<asp:Button ID="btnSaveDt" runat="server" class="bitbtndt btnsave" Text="Save" />									
            <asp:Button ID="btnCancelDt" runat="server" class="bitbtndt btncancel" Text="Cancel" />									
                 
            
       </asp:Panel> 
       <br />   
       
		<asp:Button ID="btnSaveAll" runat="server" class="bitbtndt btnsavenew" Text="Save & New" validationgroup="Input" Width = "90"/>									
        <asp:Button ID="btnSaveTrans" runat="server" class="bitbtndt btnsave" Text="Save" validationgroup="Input"/>									
        <asp:Button ID="btnBack" runat="server" class="bitbtndt btncancel" Text="Cancel" validationgroup="Input"/>									
        <asp:Button ID="btnHome" runat="server" class="bitbtndt btnback" Text="Home" />									                                                           
       
      
    </asp:Panel>
    <asp:Panel runat="server" ID="pnlPrint" Visible="false">
    <cr:crystalreportviewer ID="CrystalReportViewer1" runat="server" AutoDataBind="True"
               Height="1036px" Width="928px" />
    </asp:Panel>
      <br />            
    </div>
   
    <asp:Label runat ="server" ID="lbStatus" ForeColor="Red"/>
    
    <div class="loading" align="center">
      <%--Loading. Please wait.<br />--%>
      <br />
       <img src="../../Image/loader.gif" alt="" />
    </div>
   
    </form>
    </body>
</html>
