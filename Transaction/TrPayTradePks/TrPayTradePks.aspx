<%@ Page MaintainScrollPositionOnPostback ="true" Language="VB" AutoEventWireup="false" CodeFile="TrPayTradePks.aspx.vb" Inherits="Transaction_TrPayTrade_TrPayTrade" %>

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
    
        function setformat()
        {
        try
         {        
          var Peimen = document.getElementById("tbTotalPayment").value.replace(/\$|\,/g,"");
          if(isNaN(Peimen) == true)
            {
                Peimen = 0;
            }  
          var PeimenF = document.getElementById("tbTotalPaymentForex").value.replace(/\$|\,/g,"");
          if(isNaN(PeimenF) == true)
            {
                PeimenF = 0;
            }    
            var Other = document.getElementById("tbTotalOther").value.replace(/\$|\,/g,"");
          if(isNaN(Other) == true)
            {
                Other = 0;
            }  
          var pph = document.getElementById("tbTotalPPh").value.replace(/\$|\,/g,"");
          if(isNaN(pph) == true)
            {
                pph = 0;
            }              
          var Infois = document.getElementById("tbTotalInvoice").value.replace(/\$|\,/g,"");
          if(isNaN(Infois) == true)
            {
                Infois = 0;
            }
          var Cas = document.getElementById("tbTotalCharge").value.replace(/\$|\,/g,"");
          if(isNaN(Cas) == true)
            {
                Cas = 0;
            }
          
          var Kurs = document.getElementById("tbTotalKurs").value.replace(/\$|\,/g,"");
          if(isNaN(Kurs) == true)
            {
                Kurs = 0;
            }    
            
          var Selisih = document.getElementById("tbTotalSelisih").value.replace(/\$|\,/g,"");
          if(isNaN(Selisih) == true)
            {
                Selisih = 0;
            }    
          document.getElementById("tbTotalPaymentForex").value = setdigit(PeimenF,'<%=ViewState("DigitCurr")%>');
          document.getElementById("tbTotalPayment").value = setdigit(Peimen,'<%=ViewState("DigitHome")%>');
          document.getElementById("tbTotalOther").value = setdigit(Other,'<%=ViewState("DigitHome")%>');
          document.getElementById("tbTotalPPh").value = setdigit(pph,'<%=ViewState("DigitHome")%>');
          document.getElementById("tbTotalInvoice").value = setdigit(Infois,'<%=ViewState("DigitHome")%>');
          document.getElementById("tbTotalCharge").value = setdigit(Cas,'<%=ViewState("DigitHome")%>');
          document.getElementById("tbTotalKurs").value = setdigit(Kurs,'<%=ViewState("DigitHome")%>');
          document.getElementById("tbTotalSelisih").value = setdigit(Selisih,'<%=ViewState("DigitHome")%>');
        }catch (err){
            alert(err.description);
          }      
        }   
                
       
       function setformatdt()
        {
        try
         {   
//         var Rate = document.getElementById("tbRate").value.replace(/\$|\,/g,""); 
//         if(isNaN(Rate) == true)
//            {
//                Rate = 0;
//            }
          
//         var ChargeRate = document.getElementById("tbChargeRate").value.replace(/\$|\,/g,""); 
//         if(isNaN(ChargeRate) == true)
//            {
//                ChargeRate = 0;
//            }   
//         var ChargeForex = document.getElementById("tbChargeForex").value.replace(/\$|\,/g,""); 
//         if(isNaN(ChargeForex) == true)
//            {
//                ChargeForex = 0;
//            } 
         var PaymentForex = document.getElementById("tbPaymentForex").value.replace(/\$|\,/g,""); 
         if(isNaN(PaymentForex) == true)
            {
                PaymentForex = 0;
            }  
         document.getElementById("tbPaymentHome").value = parseFloat(document.getElementById("tbPaymentForex").value.replace(/\$|\,/g,"")) * parseFloat(document.getElementById("tbRate").value.replace(/\$|\,/g,""));
//         document.getElementById("tbChargeHome").value = parseFloat(document.getElementById("tbChargeForex").value.replace(/\$|\,/g,"")) * parseFloat(document.getElementById("tbChargeRate").value.replace(/\$|\,/g,""));
         
//         var InvoiceHome = document.getElementById("tbInvoiceHome").value.replace(/\$|\,/g,""); 
//         if(isNaN(InvoiceHome) == true)
//            {
//                InvoiceHome = 0;
//            }
//         var SelisihHome = document.getElementById("tbSelisihHome").value.replace(/\$|\,/g,""); 
//         if(isNaN(SelisihHome) == true)
//            {
//                SelisihHome = 0;
//            }
//         
//         var ChargeHome = document.getElementById("tbChargeHome").value.replace(/\$|\,/g,""); 
//         if(isNaN(ChargeHome) == true)
//            {
//                ChargeHome = 0;
//            }
         var PaymentHome = document.getElementById("tbPaymentHome").value.replace(/\$|\,/g,""); 
         if(isNaN(PaymentHome) == true)
            {
                PaymentHome = 0;
            }
//         var ChargeHome = document.getElementById("tbChargeHome").value.replace(/\$|\,/g,""); 
//         if(isNaN(ChargeHome) == true)
//            {
//                ChargeHome = 0;
//            }                           
//         document.getElementById("tbChargeRate").value = setdigit(ChargeRate,'<%=ViewState("DigitRate")%>');
         //document.getElementById("tbRate").value = setdigit(Rate,'<%=ViewState("DigitRate")%>');
         document.getElementById("tbPaymentForex").value = setdigit(PaymentForex,'<%=VIEWSTATE("DigitCurr")%>');
         document.getElementById("tbPaymentHome").value = setdigit(PaymentHome,'<%=ViewState("DigitHome")%>');
//         document.getElementById("tbChargeForex").value = setdigit(ChargeForex,'<%=VIEWSTATE("DigitChargeCurr")%>');
//         document.getElementById("tbChargeHome").value = setdigit(ChargeHome,'<%=ViewState("DigitHome")%>');
//         document.getElementById("tbSelisihHome").value = setdigit(SelisihHome,'<%=ViewState("DigitHome")%>');
        }catch (err){
            alert(err.description);
          }      
        }   

        function setformatdt2(_prmChange)
        {
        try
         {         
          var TotalToPaid = document.getElementById("tbTotalToPaidDt2").value.replace(/\$|\,/g,""); 
          var TotalInv = document.getElementById("tbTotalInvoiceDt2").value.replace(/\$|\,/g,""); 
          var TotalPaid = document.getElementById("TbTotalPaidDt2").value.replace(/\$|\,/g,""); 
          var PPNRate = document.getElementById("tbPPNRateDt2").value.replace(/\$|\,/g,""); 
          var Rate = document.getElementById("tbRateDt2").value.replace(/\$|\,/g,""); 
//          var PayRate = document.getElementById("tbPayRate").value.replace(/\$|\,/g,""); 
//          var PayRatePPn = document.getElementById("tbPayRatePPn").value.replace(/\$|\,/g,""); 
          var BaseInvoice = document.getElementById("tbBaseInvoiceDt2").value.replace(/\$|\,/g,""); 
          var BasePaid = document.getElementById("tbBasePaidDt2").value.replace(/\$|\,/g,""); 
          var BaseToPaid = document.getElementById("tbBaseToPaidDt2").value.replace(/\$|\,/g,"");           
          var PPNInv = document.getElementById("tbPPNInvoiceDt2").value.replace(/\$|\,/g,""); 
          var PPNPaid = document.getElementById("tbPPNPaidDt2").value.replace(/\$|\,/g,""); 
          var PPNToPaid = document.getElementById("tbPPNToPaidDt2").value.replace(/\$|\,/g,""); 
          var PPhInv = document.getElementById("tbPPhInvoiceDt2").value.replace(/\$|\,/g,""); 
          var PPhPaid = document.getElementById("tbPPhPaidDt2").value.replace(/\$|\,/g,""); 
          var PPhToPaid = document.getElementById("tbPPhToPaidDt2").value.replace(/\$|\,/g,""); 
          
          if (isNaN(PPNToPaid) == true) 
              { PPNToPaid = 0;
              }
              
          if (isNaN(PPhToPaid) == true) 
              { PPhToPaid = 0;
              }
               
          if (isNaN(TotalToPaid) == true) 
              { TotalToPaid = 0;
              }
              
          if (_prmChange == 'total')
          {
            if (parseFloat(TotalToPaid) < 0)
            {
                TotalToPaid= -1*TotalToPaid; 
            }
            var SisaToPaid = parseFloat(TotalInv - TotalPaid);
            if (parseFloat(TotalToPaid) >= parseFloat(SisaToPaid))
            {   TotalToPaid = SisaToPaid; 
            }            
            var SisaToPaid = parseFloat(TotalToPaid);
            if (parseFloat(SisaToPaid) >= parseFloat(PPhInv) - parseFloat(PPhPaid))
            {
                PPhToPaid = parseFloat(PPhInv) - parseFloat(PPhPaid);
                SisaToPaid = SisaToPaid - PPhToPaid;                  
            }
            else 
            {
                PPhToPaid = parseFloat(SisaToPaid);
                SisaToPaid = 0;
            }
            if (parseFloat(SisaToPaid) >= parseFloat(PPNInv) - parseFloat(PPNPaid))
            {
                PPNToPaid = parseFloat(PPNInv) - parseFloat(PPNPaid);
                SisaToPaid = SisaToPaid - PPNToPaid;               
            }
            else 
            {
                PPNToPaid = parseFloat(SisaToPaid);
                SisaToPaid = 0;
            }            
            if (parseFloat(SisaToPaid) >= parseFloat(BaseInvoice) - parseFloat(BasePaid))
            {
                BaseToPaid = parseFloat(BaseInvoice) - parseFloat(BasePaid);
            }
            else 
            {
                BaseToPaid = parseFloat(SisaToPaid);
            }
            TotalToPaid = parseFloat(BaseToPaid) + parseFloat(PPNToPaid)+ parseFloat(PPhToPaid); 
          }
          if (_prmChange == 'base')
          {
            if (parseFloat(BaseToPaid) < 0)
            {
                BaseToPaid= -1*BaseToPaid; 
            }
            if (BaseToPaid > parseFloat(BaseInvoice) - parseFloat(BasePaid))
            {
              BaseToPaid = parseFloat(BaseInvoice) - parseFloat(BasePaid);
            }
            if (parseFloat(BaseToPaid) < 0)
            {  BaseToPaid = 0;
            }
            TotalToPaid = parseFloat(BaseToPaid) + parseFloat(PPNToPaid)+ parseFloat(PPhToPaid);            
          }
          if (_prmChange == 'ppn')
          {
            if (parseFloat(PPNToPaid) < 0)
            {
                PPNToPaid= -1*PPNToPaid; 
            }
            if (PPNToPaid > parseFloat(PPNInv) - parseFloat(PPNPaid))
            {
              PPNToPaid = parseFloat(PPNInv) - parseFloat(PPNPaid);              
            }
            if (parseFloat(PPNToPaid) < 0)
            {  PPNToPaid = 0;
            }
            TotalToPaid = parseFloat(BaseToPaid) + parseFloat(PPNToPaid)+ parseFloat(PPhToPaid);            
          }   
          if (_prmChange == 'pph')
          {
            if (parseFloat(PPhToPaid) < 0)
            {
                PPhToPaid= -1*PPhToPaid; 
            }
            if (PPhToPaid > parseFloat(PPhInv) - parseFloat(PPhPaid))
            {
              PPhToPaid = parseFloat(PPhInv) - parseFloat(PPhPaid);              
            }
            if (parseFloat(PPhToPaid) < 0)
            {  PPhToPaid = 0;
            }
            TotalToPaid = parseFloat(BaseToPaid) + parseFloat(PPNToPaid)+ parseFloat(PPhToPaid);
          }     
            
          document.getElementById("tbPPNInvoiceDt2").value = setdigit(PPNInv,'<%=VIEWSTATE("DigitCurrInv")%>'); 
          document.getElementById("tbPPNPaidDt2").value = setdigit(PPNPaid,'<%=VIEWSTATE("DigitCurrInv")%>'); 
          document.getElementById("tbPPNToPaidDt2").value = setdigit(PPNToPaid,'<%=VIEWSTATE("DigitCurrInv")%>'); 
          document.getElementById("tbPPhInvoiceDt2").value = setdigit(PPhInv,'<%=VIEWSTATE("DigitCurrInv")%>'); 
          document.getElementById("tbPPhPaidDt2").value = setdigit(PPhPaid,'<%=VIEWSTATE("DigitCurrInv")%>'); 
          document.getElementById("tbPPhToPaidDt2").value = setdigit(PPhToPaid,'<%=VIEWSTATE("DigitCurrInv")%>'); 
          document.getElementById("tbTotalToPaidDt2").value = setdigit(TotalToPaid,'<%=VIEWSTATE("DigitCurrInv")%>');
          document.getElementById("tbTotalInvoiceDt2").value = setdigit(TotalInv,'<%=VIEWSTATE("DigitCurrInv")%>');
          document.getElementById("TbTotalPaidDt2").value = setdigit(TotalPaid,'<%=VIEWSTATE("DigitCurrInv")%>'); 
         //document.getElementById("tbPPNRateDt2").value = setdigit(PPNRate,'<%=ViewState("DigitRate")%>');
          //document.getElementById("tbRateDt2").value = setdigit(PayRate,'<%=ViewState("DigitRate")%>');
          
          document.getElementById("tbBaseInvoiceDt2").value = setdigit(BaseInvoice,'<%=VIEWSTATE("DigitCurrInv")%>');
          document.getElementById("tbBasePaidDt2").value = setdigit(BasePaid,'<%=VIEWSTATE("DigitCurrInv")%>');
          document.getElementById("tbBaseToPaidDt2").value = setdigit(BaseToPaid,'<%=VIEWSTATE("DigitCurrInv")%>'); 
        }catch (err){
            alert(err.description);
          }      
        }                      
        
        
        function PressNumeric2()
    {
        var _result = false;

        if(event.keyCode == 110 || event.keyCode==8 || event.keyCode == 9 || event.keyCode==37 || event.keyCode==39 || event.keyCode == 190 || ( event.keyCode >= 48 && event.keyCode <= 57 ) || ( event.keyCode >= 96 && event.keyCode <= 105 ) || (event.keyCode == 45))
        {
            _result = true;
        }
        else
        {
            _result = false;
        }    
    return _result;
    }   
    </script>
    
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
 <link href="https://fonts.googleapis.com/css2?family=Muli:wght@300;400;600&display=swap" rel="stylesheet">
</head>
<body>
    <form id="form1" runat="server">    
    <div class="Content">
    <div class="H1"><asp:Label runat="server" ID="lblTitle"></asp:Label></div>
     <hr style="color:Blue" />        
    <asp:Panel runat="server" id="PnlHd"> 
      <table>
        <tr>
            <td style="width:100px;text-align:right">Quick Search :</td>            
            <td><asp:TextBox CssClass="TextBox" runat="server" ID ="tbFilter"/> 
                  <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlField" >                      
                    <asp:ListItem Selected="True" Value="TransNmbr">Payment No</asp:ListItem>
                    <asp:ListItem Value="dbo.FormatDate(TransDate)">Payment Date</asp:ListItem>
                    <asp:ListItem>Status</asp:ListItem>
                    <asp:ListItem Value="FgReport">Report</asp:ListItem>
                    <asp:ListItem Value="Supplier">Supplier</asp:ListItem>
                    <asp:ListItem>Attn</asp:ListItem>                                       
                    <asp:ListItem Value="Payment_Type">Payment Type</asp:ListItem>
                    <asp:ListItem Value="Payment_Type_Name">Payment Type Name</asp:ListItem>
                    <asp:ListItem Value="BankPayment">Bank Payment</asp:ListItem>
                    <asp:ListItem Value="BankPaymentName">Bank Payment Name</asp:ListItem>
                    <asp:ListItem Value="DocumentNo">Document No</asp:ListItem>
                    <asp:ListItem Value="Voucher_No">Voucher No</asp:ListItem>
                    <asp:ListItem Value="Invoice_No">Invoice No</asp:ListItem>                   
                    <asp:ListItem Value="Supp_Invoice_No">Supp Invoice No</asp:ListItem>
                  </asp:DropDownList>
                  <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlRange"></asp:DropDownList>
                 
                  <asp:Button class="bitbtn btnsearch" runat="server" ID="btnSearch" Text="Search" />																						 											  
                  <asp:Button class="bitbtn btngo" runat="server" ID="btnExpand" Text="..." />																						 											   
            </td>
            <td>
                <asp:LinkButton ID="LbAdvSearch" runat="server" Text="Advanced Search" />
                &nbsp &nbsp &nbsp &nbsp
                <asp:Label runat="server" ID="Label1" Text="Outstanding AP : "/>
                <asp:LinkButton runat="server" ID="lbCount" Text="X" ForeColor="#FF6600" Font-Size="Small" />
                <asp:Label runat="server" ID="Label" Text=" record(s)"/>
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
                    <asp:ListItem Selected="True" Value="TransNmbr">Payment No</asp:ListItem>
                    <asp:ListItem Value="dbo.FormatDate(TransDate)">Payment Date</asp:ListItem>
                    <asp:ListItem>Status</asp:ListItem>
                    <asp:ListItem Value="FgReport">Report</asp:ListItem>
                    <asp:ListItem Value="Supplier">Supplier</asp:ListItem>
                    <asp:ListItem>Attn</asp:ListItem>                                   
                    <asp:ListItem Value="Payment_Type">Payment Type</asp:ListItem>
                    <asp:ListItem Value="Payment_Type_Name">Payment Type Name</asp:ListItem>
                    <asp:ListItem Value="BankPayment">Bank Payment</asp:ListItem>
                    <asp:ListItem Value="BankPaymentName">Bank Payment Name</asp:ListItem>
                    <asp:ListItem Value="DocumentNo">Document No</asp:ListItem>
                    <asp:ListItem Value="Voucher_No">Voucher No</asp:ListItem>
                    <asp:ListItem Value="Invoice_No">Invoice No</asp:ListItem>                    
                    <asp:ListItem Value="Supp_Invoice_No">Supp Invoice No</asp:ListItem>    
              </asp:DropDownList>
          </td>              
        </tr>        
      </table>      
      </asp:Panel>
          <br />
            <asp:Button class="bitbtn btnadd" runat="server" ID="BtnAdd" Text="Add" />	                                  &nbsp &nbsp &nbsp   
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
                              <%--<asp:ListItem Text="Print Full" />--%>
                          </asp:DropDownList>
                          <asp:Button class="btngo" runat="server" ID="BtnGo" Text="G" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" CommandName="Go" />                      </ItemTemplate>
                      <HeaderStyle Width="110px" />
                  </asp:TemplateField>
                  <asp:BoundField DataField="TransNmbr" HeaderStyle-Width="120px" SortExpression="Nmbr" HeaderText="Payment No"></asp:BoundField>                  
                  <asp:BoundField DataField="Status" SortExpression="Status" HeaderText="Status"></asp:BoundField>                  
                  <asp:BoundField DataField="TransDate" dataformatstring="{0:dd MMM yyyy}" htmlencode="true" HeaderStyle-Width="80px" SortExpression="TransDate" ItemStyle-wrap="true" HeaderText="Payment Date" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" > </asp:BoundField>
                  <%--<asp:BoundField DataField="FgReport" HeaderStyle-Width="80px" HeaderText="Report"></asp:BoundField>--%>
                  <asp:BoundField DataField="Supplier" HeaderStyle-Width="200px" SortExpression="Supplier" HeaderText="Supplier"></asp:BoundField>
                  <asp:BoundField DataField="Attn" HeaderStyle-Width="150px" SortExpression="Attn" HeaderText="Attn"></asp:BoundField>
                  <asp:BoundField DataField="Currency" HeaderStyle-Width="80px" SortExpression="Currency" HeaderText="Currency" ></asp:BoundField>
                  <asp:BoundField DataField="TotalPayForexStr" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="80px" SortExpression="TotalPayForex" HeaderText="Payment Forex"></asp:BoundField>
                  <asp:BoundField DataField="TotalPayment" DataFormatString="{0:#,##0.00}" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="80px" SortExpression="TotalPayment" HeaderText="Payment (IDR)"></asp:BoundField>
                  <asp:BoundField DataField="TotalOthers" DataFormatString="{0:#,##0.00}" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="80px" SortExpression="TotalOthers" HeaderText="Others (IDR)"></asp:BoundField>
                  <asp:BoundField DataField="TotalInvoice" DataFormatString="{0:#,##0.00}" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="80px" SortExpression="TotalInvoice" HeaderText="Invoice (IDR)"></asp:BoundField>
                  <%--<asp:BoundField DataField="TotalPPh" DataFormatString="{0:#,##0.00}" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="80px" SortExpression="TotalInvoice" HeaderText="Total PPh"></asp:BoundField>
                  <asp:BoundField DataField="TotalCharge" DataFormatString="{0:#,##0.00}" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="80px" SortExpression="TotalCharge" HeaderText="Total Charge"></asp:BoundField>--%>
                  <%--<asp:BoundField DataField="TotalSelisih" DataFormatString="{0:#,##0.00}" HeaderStyle-Width="80px" SortExpression="TotalSelisih" HeaderText="Total Selisih"></asp:BoundField>--%>
                  <asp:BoundField DataField="Remark" HeaderStyle-Width="250px" SortExpression="Remark" HeaderText="Remark"></asp:BoundField>
              </Columns>
          </asp:GridView>
          </div>
            <asp:Panel runat="server" ID ="pnlNav" Visible="false">           
            <asp:Button class="bitbtn btnadd" runat="server" ID="btnAdd2" Text="Add" />	            &nbsp &nbsp &nbsp
            <asp:DropDownList ID="ddlCommand2" CssClass="DropDownList" runat="server"/>
            <asp:Button class="btngo" runat="server" ID="btnGo2" Text="G" />                      
            </asp:Panel>
    </asp:Panel>    
    <asp:Panel runat="server" ID="pnlInput" Visible="false">
      <table>
        <tr>
            <td>Payment No</td>
            <td>:</td>
            <td><asp:TextBox  CssClass="TextBoxR" runat="server" ID="tbCode" Width="150px" Enabled="False"/>
            </td>
        
            <td>Payment Date</td>
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
        <%--<tr>
                <td>Report</td>
                <td>:</td>
                <td colspan="4"><asp:DropDownList AutoPostBack="true" ValidationGroup="Input" CssClass="DropDownList" ID="ddlReport" runat="server" >
                        <asp:ListItem Selected="True">Y</asp:ListItem>
                        <asp:ListItem>N</asp:ListItem>
                    </asp:DropDownList> 
                </td>    
        </tr>--%>          
        <tr>
            <td><asp:LinkButton ID="lbSupp" ValidationGroup="Input"  runat="server" Text="Supplier"/></td>
            <td>:</td>
            <td colspan="4"><asp:TextBox CssClass="TextBox" ValidationGroup="Input" runat="server" ID="tbSuppCode" AutoPostBack="true" />
                <asp:TextBox CssClass="TextBox" runat="server" ID="tbSuppName" Enabled="false" Width="225px"/>                
                <asp:Button Class="btngo" ID="btnSupp" Text="..." runat="server" ValidationGroup="Input" />                                                 
                <asp:Label ID="lbItemNo7" runat="server" ForeColor="#FF3300" Text="*" />
            </td>
        </tr>
        <tr>
            <td>Attn</td>
            <td>:</td>
            <td colspan="4"><asp:TextBox runat="server" MaxLength = "60" ValidationGroup="Input" ID="tbAttn"  CssClass="TextBox" Width="225px"/></td>
        </tr>
        
        <tr>
            <td>Total</td>
            <td>:</td>
            <td colspan="4">
                <table>
                    <tr style="background-color:Silver;text-align:center">
                       <td><asp:Label ID="lbTotPayForex" runat="server" CssClass="TextBox" Text = "Payment Forex"/></td>                                              
                     </tr>
                     <tr>
                        <td><asp:TextBox runat="server" ValidationGroup="Input" ID="tbTotalPaymentForex" CssClass="TextBoxR" Width="108px"/></td>                        
                     </tr>
                 </table>
            </td>                         
        </tr> 
        <tr>
            <td>Total</td>
            <td>:</td>
            <td colspan="4">
                <table>
                    <tr style="background-color:Silver;text-align:center">
                       <td><asp:Label ID="lbTotPay" runat="server" CssClass="TextBox" Text = "Payment (Home)"/></td>
                       <td><asp:Label ID="lbTotDPForex" runat="server" CssClass="TextBox" Text = "DP"/></td>
                       <td><asp:Label ID="lbTotOther" runat="server" CssClass="TextBox" Text = "Other"/></td>
                       <td><asp:Label ID="lbTotInvoice" runat="server" CssClass="TextBox" Text = "Invoice"/></td>
                       <td><asp:Label ID="lbTotPPh" runat="server" CssClass="TextBox" Text = "PPh"/></td>
                       <td><asp:Label ID="lbTotCharge" runat="server" CssClass="TextBox" Text = "Charge"/></td>
                       <td><asp:Label ID="lbTotKurs" runat="server" CssClass="TextBox" Text = "Kurs"/></td>
                       <td><asp:Label ID="lbTotSelisih" runat="server" CssClass="TextBox" Text = "Difference"/></td>
                     </tr>
                     <tr>
                        <td><asp:TextBox runat="server" ValidationGroup="Input" ID="tbTotalPayment" CssClass="TextBoxR" Width="108px"/></td>
                        <td><asp:TextBox runat="server" ValidationGroup="Input" ID="tbTotalDPForex" CssClass="TextBoxR" Width="108px"/></td>
                        <td><asp:TextBox runat="server" ValidationGroup="Input" ID="tbTotalOther" CssClass="TextBoxR" Width="108px"/></td>
                        <td><asp:TextBox runat="server" ValidationGroup="Input" ID="tbTotalInvoice" CssClass="TextBoxR" Width="108px"/></td>
                        <td><asp:TextBox runat="server" ValidationGroup="Input" ID="tbTotalPPh" CssClass="TextBoxR" Width="108px"/></td>
                        <td><asp:TextBox runat="server" ValidationGroup="Input" ID="tbTotalCharge" CssClass="TextBoxR" Width="108px"/></td>
                        <td><asp:TextBox runat="server" ValidationGroup="Input" ID="tbTotalKurs" CssClass="TextBoxR" Width="108px"/></td>
                        <td><asp:TextBox runat="server" ValidationGroup="Input" ID="tbTotalSelisih" CssClass="TextBoxR" Width="108px"/></td>
                     </tr>
                 </table>
            </td>                         
        </tr> 
        <tr>
            <td>Remark</td>
            <td>:</td>
            <td colspan="4">
                <asp:TextBox ID="tbRemark" runat="server"  ValidationGroup="Input" CssClass="TextBox" MaxLength="255" 
                    TextMode="MultiLine" Width="365px"  />
<%--                <asp:RegularExpressionValidator runat="server" ID="valInput"
                    ControlToValidate="tbRemark"
                    ValidationExpression="^[\s\S]{0,60}$"
                    ErrorMessage="Please enter a maximum of 60 characters"
                    Display="Dynamic">*--%>
                <%--</asp:RegularExpressionValidator>    --%>
            </td>
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
                <asp:MenuItem Text="Detail Invoice" Value="0"></asp:MenuItem>
                <asp:MenuItem Text="Detail Payment" Value="1"></asp:MenuItem>                   
            </Items>            
        </asp:Menu>
        <br />
        <asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex="0">
    
            <asp:View ID="Tab2" runat="server">   
                <%--<hr style="color:Blue" /> --%>                                    
                <asp:Panel ID="pnlDt2" runat="server">
                
                 <asp:Button class="bitbtn btnadd" runat="server" ID="btnAddDt2" Text="Add" ValidationGroup="Input" />	          
                    &nbsp;&nbsp;&nbsp;	          
                 <asp:Button ID="btnGetInv" runat="server" class="bitbtndt btngetitem" Text="Get Data"  width = "65" validationgroup="Input"/>									              
                      
                    <div style="border:0px  solid; width:100%; height:100%; overflow:auto;">
                        <asp:GridView ID="GridDt2" runat="server" AutoGenerateColumns="false" 
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
                                     
                                     <EditItemTemplate>
                                        <asp:Button ID="btnUpdate" runat="server" class="bitbtndt btnsave" Text="Save" CommandName="Update"/>									
                                        <asp:Button ID="btnCancel" runat="server" class="bitbtndt btncancel" Text="Cancel" CommandName="Cancel"/>									                                    
                                      </EditItemTemplate>
                                </asp:TemplateField>
                                
                                <%--<asp:TemplateField>
                                <ItemTemplate>
                                       <asp:Button class="bitbtndt btngetitem" runat="server" ID="btnDetail" Text="Detail" width = "65" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" CommandName="Detail" />
                                       <asp:Label ID="lbFgMode" runat="server" Visible="false" text='<%# DataBinder.Eval(Container.DataItem, "FgMode") %>'/>
                                        <asp:Label ID="lbProductType" runat="server" Visible="false" text='<%# DataBinder.Eval(Container.DataItem, "ProductType") %>' />
                                </asp:TemplateField>--%> 
                                <asp:BoundField DataField="ItemNo" HeaderText="No" />
                                <asp:BoundField DataField="InvoiceNo" HeaderText="Invoice No" />
                                <asp:BoundField DataField="SupplierInvNo" HeaderText="Supp Invoice No" />
                                <asp:BoundField DataField="DueDate" HeaderStyle-Width="80px" HeaderText="Due Date" />
                                <asp:BoundField DataField="Currency" HeaderStyle-Width="80px" HeaderText="Currency" />
                                <%--<asp:BoundField DataField="PayRate" HeaderStyle-Width="80px" HeaderText="Pay Rate" />--%>
                                <asp:BoundField DataField="ForexRate" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Right" HeaderText="Inv Rate" />
                                <asp:BoundField DataField="PPnRate" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Right" HeaderText="PPn Rate" />
                                <asp:BoundField DataField="BaseInvoice" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Right" HeaderText="Base Invoice" />
                                <asp:BoundField DataField="PPNInvoice" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Right"  HeaderText="PPN Invoice" />
                                <asp:BoundField DataField="TotalInvoice" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Right" HeaderText="Total Invoice" />
                                <asp:BoundField DataField="BasePaid" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Right" HeaderText="Base Paid" />
                                <asp:BoundField DataField="PPnPaid" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Right" HeaderText="PPn Paid" />
                                <asp:BoundField DataField="TotalPaid" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Right" HeaderText="Total Paid" />
                                <asp:BoundField DataField="BaseToPaid" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Right" HeaderText="Base To Paid" />
                                <asp:BoundField DataField="PPnToPaid" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Right" HeaderText="PPn To Paid" />
                                <asp:BoundField DataField="TotalToPaid" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Right" HeaderText="Total To Paid" />
                                <asp:BoundField DataField="PPnToPaidHome" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Right" HeaderText="PPn To Paid (Home)" />
                                <asp:BoundField DataField="PPhInvoice" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Right" HeaderText="PPh Invoice" />
                                <asp:BoundField DataField="PPhPaid" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Right" HeaderText="PPh Paid" />
                                <asp:BoundField DataField="PPhToPaid" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Right" HeaderText="PPh To Paid" />
                                <asp:BoundField DataField="Type" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Right" HeaderText="Type" />
                            </Columns>
                        </asp:GridView>
                    </div>    
                    <asp:Button class="bitbtn btnadd" runat="server" ID="btnAddDt2Ke2" Text="Add" ValidationGroup="Input" />	          

                </asp:Panel>
                <asp:Panel runat="server" ID="pnlEditDt2" Visible="false">
                    <table>  
                     <tr>
                        <td>Item No</td>
                        <td>:</td>
                        <td colspan="4"><asp:Label ID="lbItemNo" runat="server" Text="itemmm" />
                        </td>           
                    </tr>                     
                        <tr>
                            <td width="50px">Invoice No</td>
                            <td>:</td>
                            <td><asp:TextBox CssClass="TextBoxR" runat="server" ID="tbInvNoDt2" Width="150px" ReadOnly="true" />
                                <asp:Button Class="btngo" ID="btnInvNo" Text="..." runat="server" />                                                                           <asp:TextBox runat="server" ID="tbFgValueDt2" Visible="false" />
                                <asp:Label ID="lbItemNo5" runat="server" ForeColor="#FF3300" Text="*" />
                            </td>
                    
                            <td>Supplier Invoice No</td>
                            <td>:</td>
                            <td><asp:TextBox CssClass="TextBoxR" runat="server" ID="tbSuppInvNoDt2" Maxlength = "30" ReadOnly="true"
                                    Width="157px" /> </td>
                            <%--<td><asp:TextBox CssClass="TextBoxR" runat="server" ID="tbProductType" Visible="false"
                                    Width="157px" /> </td>--%>
                        </tr>         
                        <tr>                                           
                            <td>Currency</td>
                            <td>:</td>
                            <td><asp:DropDownList CssClass="DropDownList" ID="ddlCurrDt2" runat="server" Enabled="false"
                                    AutoPostBack="True"/>                                
                            </td>
                            <td>Due Date</td>
                            <td>:</td>
                            <td><BDP:BasicDatePicker ID="tbDueDateDt2" runat="server" DateFormat="dd MMM yyyy" 
                                ReadOnly = "true" ShowNoneButton="false"
                                ButtonImageHeight="19px" ButtonImageWidth="20px" 
                                DisplayType="TextBox" 
                                TextBoxStyle-CssClass="TextDate"><TextBoxStyle CssClass="TextDate" /></BDP:BasicDatePicker>
                            </td>
                        </tr>   
                        <tr>
                        <td>Inv & Pay Rate</td>
                        <td>:</td>
                        <td><asp:TextBox CssClass="TextBoxR" ReadOnly="true" runat="server" ID="tbRateDt2"/>
                        <asp:TextBox CssClass="TextBox" runat="server" ID="tbPayRateDt2" Visible="true"/>                         
                        </td>
                    </tr>  
                    <tr>
                        <td>PPN & Pay Rate</td>
                        <td>:</td>
                        <td><asp:TextBox CssClass="TextBoxR" runat="server" ID="tbPPNRateDt2" ReadOnly="true"/>
                            <asp:TextBox CssClass="TextBoxR" runat="server" ID="tbPayRatePPn" ReadOnly="true"/>                                                       
                        </td>
                    </tr>
                    </table>                    
                    <table>                    
                        <tr>
                            <td width="50px"></td>
                            <td></td>
                            <td style="background-color:Silver;text-align:center">Invoice</td>                                    
                            <td style="background-color:Silver;text-align:center">Paid</td>
                            <td style="background-color:Silver;text-align:center">To Paid<asp:Label 
                                    ID="lbItemNo6" runat="server" ForeColor="#FF3300" Text="*" />
                            </td>                                         
                        </tr>
                        <tr>
                            <td>Total</td>
                            <td>:</td>
                            <td><asp:TextBox CssClass="TextBoxR" runat="server" ID="tbTotalInvoiceDt2" /></td>
                            <td><asp:TextBox CssClass="TextBoxR" runat="server" ID="TbTotalPaidDt2" /></td>
                            <td><asp:TextBox CssClass="TextBox" runat="server" ID="tbTotalToPaidDt2"/></td>
                        </tr>
                        <tr>
                           <td>Base</td>
                           <td>:</td>
                           <td><asp:TextBox CssClass="TextBoxR" runat="server" ID="tbBaseInvoiceDt2"/></td>
                           <td><asp:TextBox CssClass="TextBoxR" runat="server" ID="tbBasePaidDt2"/></td>
                           <td><asp:TextBox CssClass="TextBox" runat="server" ID="tbBaseToPaidDt2"/></td>
                        </tr>
                        <tr>
                            <td>PPN</td>
                            <td>:</td>
                            <td><asp:TextBox CssClass="TextBoxR" runat="server" ID="tbPPNInvoiceDt2"/></td>
                            <td><asp:TextBox CssClass="TextBoxR" runat="server" ID="tbPPNPaidDt2"/></td>
                            <td><asp:TextBox CssClass="TextBox" runat="server" ID="tbPPNToPaidDt2"/></td>
                        </tr>
                        <tr>
                            <td>PPh</td>
                            <td>:</td>
                            <td><asp:TextBox CssClass="TextBoxR" runat="server" ID="tbPPhInvoiceDt2"/></td>
                            <td><asp:TextBox CssClass="TextBoxR" runat="server" ID="tbPPhPaidDt2"/></td>
                            <td><asp:TextBox CssClass="TextBox" runat="server" ID="tbPPhToPaidDt2"/></td>
                        </tr>  
                    </table>   
                                    
                    <table>                       
                        <tr>
                            <td width="50px">Remark </td>
                            <td>:</td>
                            <td><asp:TextBox runat="server" ID="tbRemarkDt2" CssClass="TextBox" Width="365px" 
                                    MaxLength="255" TextMode="MultiLine" />                        
                            </td>
                        </tr> 
                        <tr>
                            <td width="50px">Type </td>
                            <td>:</td>
                           <td><asp:TextBox CssClass="TextBox" runat="server" ID="tbType"/></td> 
                        </tr>                                                 
                    </table>
                    <br />                     
                   
                <asp:Button ID="btnSaveDt2" runat="server" class="bitbtndt btnsave" Text="Save" />									
                <asp:Button ID="btnCancelDt2" runat="server" class="bitbtndt btncancel" Text="Cancel" />					
                
                </asp:Panel>                
            </asp:View>  
            
            
            <asp:View ID="Tab1" runat="server">
            <%--<table visible="false">
                <tr>
                <td>Item No</td>
                <td>:</td>
                <td><asp:Label ID="lbItemNodt2" runat="server" Text="" /></td>                                    
                
                <td align="right">Payment Type</td>
                <td>:</td>
                <td><asp:Label ID="lblPaymentTypeDt2" runat="server" Text="" /></td>                                                    
                </tr>
                <tr>
                    <td>Currency & Rate</td>
                    <td>:</td>
                    <td><asp:Label ID="lblCurrPayDt" runat="server" Text="" />
                    <asp:Label ID="Label2" runat="server" Text="/" />
                    <asp:Label ID="lblRatePayDt" runat="server" Text="" />
                    <asp:Label ID="lbFgModeDt2" runat="server" Visible="false" />
                    </td>                                    
                </tr>
                <tr>
                   <td>Payment</td>
                   <td>:</td>
                   <td colspan="4">
                       <table>
                          <tr style="background-color:Silver;text-align:center">
                             <td><asp:Label ID="lblVPayHome" runat="server" CssClass="TextBox" Text = "Payment Home"/></td>                                    
                             <td><asp:Label ID="lblVInvHome" runat="server" CssClass="TextBox" Text = "Invoice Home"/></td> 
                             <td><asp:Label ID="lblVPPhHome" runat="server" CssClass="TextBox" Text = "PPh Home"/></td>   
                             
                          </tr>
                          <tr>
                             <td><asp:TextBox CssClass="TextBox" runat="server" ID="tbPayHomeDt" Width="80px" Enabled ="false"/></td>
                             <td><asp:TextBox CssClass="TextBox" runat="server" ID="tbInvHomeDt" Width="80px" Enabled ="false"/></td>   
                             <td><asp:TextBox CssClass="TextBox" runat="server" ID="tbPPhHomeDt" Width="80px" Enabled ="false"/></td>                                   
                             
                          </tr>
                       </table>
                   </td>
                </tr>                                   
            </table>    --%>
              <asp:Panel runat="server" ID="PnlDt">
                <asp:Button class="bitbtn btnadd" runat="server" ID="btnAdddt" Text="Add" ValidationGroup="Input" />	
                  &nbsp;&nbsp;&nbsp;	
                  <asp:Button ID="btnBackDt2" runat="server" class="bitbtndt btncancel" 
                      Text="Back" validationgroup="Input" />
                <div style="border:0px  solid; width:100%; height:100%; overflow:auto;">
                    <asp:GridView ID="GridDt" runat="server" AutoGenerateColumns="False" 
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
                               
                            <asp:BoundField DataField="ItemNo" HeaderText="No" />
                            <asp:BoundField DataField="PaymentName" HeaderStyle-Width="160px" HeaderText="Payment Type" />                                
                            <asp:BoundField DataField="PaymentDate" Visible="false" HeaderStyle-Width="80px" HeaderText="Payment Date" />
                            <asp:BoundField DataField="DocumentNo" HeaderStyle-Width="150px" HeaderText="Document No" >
                                <HeaderStyle Width="150px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Reference" HeaderStyle-Width="120px" HeaderText="Voucher No" >                            
                                <HeaderStyle Width="120px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="GiroDate" HeaderText="Giro Date" />
                            <asp:BoundField DataField="DueDate" HeaderStyle-Width="80px" HeaderText="Due Date" />
                            <asp:BoundField DataField="BankPaymentName" HeaderStyle-Width="150px" HeaderText="Bank Payment" />
                            <asp:BoundField DataField="Currency" HeaderStyle-Width="80px" HeaderText="Currency" />
                            <asp:BoundField DataField="ForexRate" HeaderStyle-Width="80px" DataFormatString="{0:#,##0.00}" ItemStyle-HorizontalAlign="Right" HeaderText="Rate" />
                            <asp:BoundField DataField="DPRate" HeaderStyle-Width="80px" DataFormatString="{0:#,##0.00}" ItemStyle-HorizontalAlign="Right" HeaderText="DP Rate" />
                            <asp:BoundField DataField="PaymentForex" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Right" HeaderText="Payment Forex" />
                            <asp:BoundField DataField="PaymentHome" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Right" HeaderText="Payment Home" />
                            <asp:BoundField DataField="InvoiceHome" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Right" HeaderText="Invoice Home" Visible=false />
                            <asp:BoundField DataField="PPhHome" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Right" HeaderText="PPh Home" Visible=false />
                            <asp:BoundField DataField="Remark" HeaderStyle-Width="200px" HeaderText="Remark" />
                        </Columns>
                        
                    </asp:GridView>
              </div>   
              <asp:Button class="bitbtn btnadd" runat="server" ID="btnAddDtke2" Text="Add" ValidationGroup="Input" />	
                  &nbsp;&nbsp;&nbsp;	
                  <asp:Button ID="btnBackDt2Ke2" runat="server" class="bitbtndt btncancel" 
                      Text="Back" validationgroup="Input" />
              </asp:Panel> 
              <asp:Panel runat="server" ID="pnlEditDt" Visible="false">
                <table>              
                   <tr>
                        <td>Item No</td>
                        <td>:</td>
                        <td colspan="4"><asp:Label ID="lbItemNoDt" runat="server" Text="itemmm" />
                        </td>           
                    </tr>       
                    <tr>
                        <td>Payment Type</td>
                        <td>:</td>
                        <td><asp:DropDownList CssClass="DropDownList" ID="ddlPaymentType" runat="server" Width="190px" AutoPostBack ="true"/>
                            <asp:TextBox CssClass ="TextBox" ID = "tbFgMode" runat ="server" Visible="False" />
                            <asp:Label ID="lbItemNo0" runat="server" Text="*" ForeColor="#FF3300" />
                        </td>
                            
                        <td>Giro Date</td>
                        <td>:</td>
                        <td>
                             <BDP:BasicDatePicker ID="tbGiroDate" runat="server" DateFormat="dd MMM yyyy" 
                             ReadOnly = "true" ValidationGroup="Input" ButtonImageHeight="19px" ButtonImageWidth="20px" 
                             DisplayType="TextBoxAndImage" TextBoxStyle-CssClass="TextDate"
                             ShowNoneButton="False"><TextBoxStyle CssClass="TextDate" /></BDP:BasicDatePicker>
                        </td>
                    </tr>
                    <tr>
                        <td>Document No</td>
                        <td>:</td>
                        <td><asp:TextBox CssClass="TextBox" runat="server" ID="tbDocumentNo" Width="157px"/> 
                            <asp:Button Class="btngo" ID="btnDocNo" Text="..." runat="server" MaxLength = "60"  />                                          
                        </td>
                        
                        <td>Voucher No</td>
                        <td>:</td>
                        <td><asp:TextBox CssClass="TextBox" runat="server" ID="tbVoucherNo" Width="157px"/>                            
                        <BDP:BasicDatePicker ID="tbPaymentDate" runat="server" DateFormat="dd MMM yyyy" 
                             ReadOnly = "true" ButtonImageHeight="19px" ButtonImageWidth="20px" 
                             DisplayType="TextBoxAndImage" TextBoxStyle-CssClass="TextDate" Visible="False"
                             ShowNoneButton="False"><TextBoxStyle CssClass="TextDate" /></BDP:BasicDatePicker>
                             
                        </td>
                    </tr>                    
                    <tr>
                        <td>Bank Payment</td>
                        <td>:</td>
                        <td><asp:DropDownList CssClass="DropDownList" ID="ddlBankPayment" runat="server"/></td>
                    
                        <td>Due Date</td>
                        <td>:</td>
                        <td>
                             <BDP:BasicDatePicker ID="tbDueDate" runat="server" DateFormat="dd MMM yyyy" 
                             ReadOnly = "true" ButtonImageHeight="19px" ButtonImageWidth="20px" 
                             DisplayType="TextBoxAndImage" TextBoxStyle-CssClass="TextDate"
                             ShowNoneButton="False"><TextBoxStyle CssClass="TextDate" /></BDP:BasicDatePicker>
                        </td>    
                        
                    </tr>   
                    <tr> 
                        <td>Currency</td>
                        <td>:</td>
                        <td colspan="4">
                            <table>
                                <tr style="background-color:Silver;text-align:center">
                                    <td>Payment</td>
                                    <td>Rate</td>
                                    <%--<td>Charge</td>--%>
                                    <%--<td>Rate</td> --%>
                                    <td><asp:Label ID="lbDPRate" runat="server" CssClass="TextBox" Text="DP Rate" /></td>
                                </tr>
                                <tr>
                                    <td><asp:DropDownList CssClass="DropDownList" ID="ddlCurr" runat="server" Enabled="false" Width="60px"/></td>
                                    <td><asp:TextBox CssClass="TextBox" runat="server" ID="tbRate" Width="65px"/></td>
                                    <%--<td><asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlChargeCurr" 
                                            Width="60px" AutoPostBack="True"/></td>
                                    <td><asp:TextBox CssClass="TextBox" runat="server" ID="tbChargeRate" Width="65px"/></td>   --%>
                                    <td>
                                        <asp:TextBox ID="tbDPRate" runat="server" CssClass="TextBoxR" Width="65px" 
                                            Enabled="False" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                        
                        
                    </tr>   
                    <tr>
                        <td>Nominal</td>
                        <td>:</td>
                        <td colspan="4">
                            <table>
                                <tr style="background-color:Silver;text-align:center">
                                    <td><asp:Label ID="lbPayForex" runat="server" CssClass="TextBox" Text = "Payment Forex"/></td>
                                    <td><asp:Label ID="lbPayHome" runat="server" CssClass="TextBox" Text = "Paymen Home"/></td>                                    
                                    <%--<td><asp:Label ID="lbInvoiceHome" runat="server" CssClass="TextBox" Text = "Invoice Home"/></td>                                
                                    <td><asp:Label ID="lbPPhHome" runat="server" CssClass="TextBox" Text = "PPh Home"/></td>--%>                                
                                    <%--<td><asp:Label ID="lbSelisihHome" runat="server" CssClass="TextBox" Text = "Selisih Home"/></td>--%>
                                    <%--<td><asp:Label ID="lbChargeForex" runat="server" CssClass="TextBox" Text = "Charge Forex"/></td>
                                    <td><asp:Label ID="lbChargeHome" runat="server" CssClass="TextBox" Text = "Charge Home"/></td>--%>
                                </tr>
                                <tr>
                                    <td><asp:TextBox CssClass="TextBox" runat="server" ID="tbPaymentForex"  Maxlength = "13" Width="80px"/></td>
                                    <td><asp:TextBox CssClass="TextBoxR" runat="server" ID="tbPaymentHome" Width="80px"/></td>                                    
                                    <%--<td><asp:TextBox CssClass="TextBoxR" runat="server" ID="tbInvoiceHome" Width="80px"/></td>
                                    <td><asp:TextBox CssClass="TextBoxR" runat="server" ID="tbPPhHome" Width="80px"/></td>--%>
                                    <%--<td><asp:TextBox CssClass="TextBoxR" runat="server" ID="tbSelisihHome" Width="80px"/></td>--%>
                                    <%--<td><asp:TextBox CssClass="TextBox" runat="server" ID="tbChargeForex"  Maxlength = "13" Width="80px"/></td>
                                    <td><asp:TextBox CssClass="TextBoxR" runat="server" ID="tbChargeHome" Width="80px"/></td>--%>
                                </tr>
                            </table>
                        </td>
                    </tr>    
                    <%--<tr>
                        <td>Product Type</td>
                        <td>:</td>
                        <td>
                            <asp:DropDownList ID="ddlProductType" runat="server" CssClass="DropDownList" />
                        </td>
                    </tr>                               --%>
                    <tr>
                        <td>Remark </td>
                        <td>:</td>
                        <td colspan="4"><asp:TextBox runat="server" ID="tbRemarkDt" CssClass="TextBox" Width="365px" 
                                MaxLength="255" TextMode="MultiLine"  />                        
                        </td>
                    </tr>
                </table>
                <br />                     
               
               <asp:Button ID="btnSaveDt" runat="server" class="bitbtndt btnsave" Text="Save" />									
               <asp:Button ID="btnCancelDt" runat="server" class="bitbtndt btncancel" Text="Cancel" />									
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
