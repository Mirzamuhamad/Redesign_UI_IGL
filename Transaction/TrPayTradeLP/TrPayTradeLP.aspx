<%@ Page MaintainScrollPositionOnPostback ="true" Language="VB" AutoEventWireup="false" CodeFile="TrPayTradeLP.aspx.vb" Inherits="Transaction_TrPayTradeLP_TrPayTradeLP" %>

<%@ Register Assembly="BasicFrame.WebControls.BasicDatePicker" Namespace="BasicFrame.WebControls"
    TagPrefix="BDP" %>
    
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Land Purchase Payment</title>
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
          document.getElementById("tbTotalAmount").value = setdigit(PeimenF,'<%=ViewState("DigitCurr")%>');
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


        var PaymentForex = document.getElementById("tbTotalAmount").value.replace(/\$|\,/g,""); 
         if(isNaN(PaymentForex) == true)
            {
                PaymentForex = 0;
            }  
         document.getElementById("tbPaymentHome").value = parseFloat(document.getElementById("tbTotalAmount").value.replace(/\$|\,/g,"")) * parseFloat(document.getElementById("tbRate").value.replace(/\$|\,/g,""));
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
         document.getElementById("tbTotalAmount").value = setdigit(PaymentForex,'<%=VIEWSTATE("DigitCurr")%>');
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



 function setformathd(prmchange)
        {
         try {
          

            document.getElementById("tbTotalPaymentForex").value = parseFloat(document.getElementById("tbPaymentTanah").value.replace(/\$|\,/g,"")) + parseFloat(document.getElementById("tbPaymentNotaris").value.replace(/\$|\,/g,"")) + parseFloat(document.getElementById("tbPaymentModerator").value.replace(/\$|\,/g,"")) + parseFloat(document.getElementById("tbPaymentBPHTB").value.replace(/\$|\,/g,""))+ parseFloat(document.getElementById("tbPaymentSurvey").value.replace(/\$|\,/g,"")) + parseFloat(document.getElementById("tbPaymentLainLain").value.replace(/\$|\,/g,"")); 


           document.getElementById("tbTotalSelisih").value = parseFloat(document.getElementById("tbBankPayment").value.replace(/\$|\,/g,"")) - parseFloat(document.getElementById("tbTotalPaymentForex").value.replace(/\$|\,/g,"")); 






            document.getElementById("tbPaymentTanah").value = setdigit(document.getElementById("tbPaymentTanah").value.replace(/\$|\,/g,""),'<%=ViewState("DigitCurr")%>'); 
            document.getElementById("tbPaymentNotaris").value = setdigit(document.getElementById("tbPaymentNotaris").value.replace(/\$|\,/g,""),'<%=ViewState("DigitCurr")%>');
            document.getElementById("tbPaymentBPHTB").value = setdigit(document.getElementById("tbPaymentBPHTB").value.replace(/\$|\,/g,""),'<%=ViewState("DigitCurr")%>');
            document.getElementById("tbPaymentModerator").value = setdigit(document.getElementById("tbPaymentModerator").value.replace(/\$|\,/g,""),'<%=ViewState("DigitCurr")%>');
            document.getElementById("tbPaymentSurvey").value = setdigit(document.getElementById("tbPaymentSurvey").value.replace(/\$|\,/g,""),'<%=ViewState("DigitCurr")%>');
            document.getElementById("tbPaymentLainLain").value = setdigit(document.getElementById("tbPaymentLainLain").value.replace(/\$|\,/g,""),'<%=ViewState("DigitCurr")%>');
            document.getElementById("tbTotalPaymentForex").value = setdigit(document.getElementById("tbTotalPaymentForex").value.replace(/\$|\,/g,""),'<%=ViewState("DigitCurr")%>');  

             document.getElementById("tbBankPayment").value = setdigit(document.getElementById("tbBankPayment").value.replace(/\$|\,/g,""),'<%=ViewState("DigitCurr")%>');  

              document.getElementById("tbTotalSelisih").value = setdigit(document.getElementById("tbTotalSelisih").value.replace(/\$|\,/g,""),'<%=ViewState("DigitCurr")%>');            
        }catch (err){
            alert(err.description);
          }      
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
                    <asp:ListItem Value="LpNo">Land Purchase No</asp:ListItem>
                    <asp:ListItem Value="LpInvoice">Invoice No</asp:ListItem>
                    <asp:ListItem Value="JenisPayment">Jenis Payment</asp:ListItem>
                    <asp:ListItem Value="AttnName">Attn</asp:ListItem>
                    
                
                  </asp:DropDownList>
                  <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlRange"></asp:DropDownList>
                 
                  <asp:Button class="bitbtn btnsearch" runat="server" ID="btnSearch" Text="Search" />																						 											  
                  <asp:Button class="bitbtn btngo" runat="server" ID="btnExpand" Text="..." />																						 											   
            </td>
            <td>
                <asp:LinkButton ID="LbAdvSearch" runat="server" Text="Advanced Search" />
                &nbsp &nbsp &nbsp &nbsp
                <asp:Label runat="server" ID="Label1" Text="Outstanding LP : "/>
                <asp:LinkButton runat="server" ID="lbCount" Text="X"  Font-Size="Small" />
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
                    <asp:ListItem Value="LpNo">Land Purchase No</asp:ListItem>
                    <asp:ListItem Value="LpInvoice">Invoice No</asp:ListItem>
                    <asp:ListItem Value="JenisPayment">Jenis Payment</asp:ListItem>
                    <asp:ListItem Value="AttnName">Attn</asp:ListItem>
                      
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
              <HeaderStyle Wrap="false" CssClass="GridHeader"></HeaderStyle>
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
                  <asp:BoundField DataField="TransNmbr" HeaderStyle-Width="100px" SortExpression="Nmbr" HeaderText="Payment No"></asp:BoundField>                  
                  <asp:BoundField DataField="Status" HeaderStyle-Width="30px" SortExpression="Status" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderText="Status"></asp:BoundField>                  
                  <asp:BoundField DataField="TransDate" dataformatstring="{0:dd MMM yyyy}" htmlencode="true" HeaderStyle-Width="80px" SortExpression="TransDate" ItemStyle-wrap="true" HeaderText="Payment Date" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" > </asp:BoundField>
                 
                  <asp:BoundField DataField="LPNo" HeaderStyle-Width="100px" SortExpression="LPNo" HeaderText="LP No"  ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ></asp:BoundField>

                  <asp:BoundField DataField="AreaName" HeaderStyle-Width="100px" SortExpression="AreaName" HeaderText="Area" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"></asp:BoundField>
                  
                  <asp:BoundField DataField="LpInvoice" HeaderStyle-Width="100px" SortExpression="LpInvoice" HeaderText="Invoice No" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"></asp:BoundField> 
                  

                  <asp:BoundField DataField="JenisPayment" HeaderStyle-Width="100px" SortExpression="JenisPayment" HeaderText="Jenis Payment" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"></asp:BoundField> 
                  
                   <asp:BoundField DataField="AttnName" HeaderStyle-Width="100px" SortExpression="AttnName" HeaderText="Attn" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"></asp:BoundField> 


                   <asp:BoundField DataField="TotalPayment" DataFormatString="{0:#,##0.00}" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="80px" SortExpression="TotalPayment" HeaderText="Total Amount (Rp)"></asp:BoundField>
                  

                   <asp:BoundField DataField="BankPayment" DataFormatString="{0:#,##0.00}" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="80px" SortExpression="BankPayment" HeaderText="Bank Payment (Rp)"></asp:BoundField>
                  
                  
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
       
        <tr>
            <td>Land Purchase No</td>
            <td>:</td>
            <td colspan="4"><asp:TextBox CssClass="TextBox" ValidationGroup="Input" runat="server" ID="tbLpCode" Width="225px" AutoPostBack="true" />               
                <asp:Button Class="btngo" ID="btnLPNo" Text="..." runat="server" ValidationGroup="Input" />                                                 
                <asp:Label ID="lbItemNo7" runat="server" ForeColor="#FF3300" Text="*" />
            </td>
        </tr>

        <tr>
            <td>Area</td>
            <td>:</td>
            <td ><asp:TextBox runat="server" ID="tbAreaName"  CssClass="TextBox"  Enabled="False" Width="225px"/></td>
            <td ><asp:TextBox runat="server" ValidationGroup="Input" ID="tbArea" Visible="False"  CssClass="TextBox" Width="225px"/></td>
            
        </tr>
        
        <tr>
            <td>Payment</td>
            <td>:</td>
            <td colspan="4">
                <table>

                    <tr style="background-color:Silver;text-align:center">
                       <td><asp:Label ID="lbPaymentTanah" runat="server" CssClass="TextBox" Text = "Tanah"/></td>
                       <td><asp:Label ID="lbPaymentNotaris" runat="server" CssClass="TextBox" Text = "Notaris"/></td>
                       <td><asp:Label ID="lbPaymentBPHTB" runat="server" CssClass="TextBox" Text = "BPHTB"/></td>
                       <td><asp:Label ID="lbPaymentModerator" runat="server" CssClass="TextBox" Text = "Moderator"/></td>
                       <td><asp:Label ID="lbPaymentSurvey" runat="server" CssClass="TextBox" Text = "Survey"/></td>
                       <td><asp:Label ID="lbPaymentLainLain" runat="server" CssClass="TextBox" Text = "Lain - Lain"/></td>
                     </tr>

                     <tr>
                        <td><asp:TextBox style="text-align:right" runat="server" ValidationGroup="Input" ID="tbPaymentTanah"   CssClass="TextBox" Width="108px"/></td>
                        <td><asp:TextBox style="text-align:right" runat="server" ValidationGroup="Input" ID="tbPaymentNotaris"   CssClass="TextBox" Width="108px"/></td>
                        <td><asp:TextBox style="text-align:right" runat="server" ValidationGroup="Input" ID="tbPaymentBPHTB"   CssClass="TextBox" Width="108px"/></td>
                        <td><asp:TextBox style="text-align:right" runat="server" ValidationGroup="Input" ID="tbPaymentModerator"   CssClass="TextBox" Width="108px"/></td>
                        <td><asp:TextBox style="text-align:right" runat="server" ValidationGroup="Input" ID="tbPaymentSurvey"   CssClass="TextBox" Width="108px"/></td>
                        <td><asp:TextBox style="text-align:right" runat="server" ValidationGroup="Input" ID="tbPaymentLainLain"  CssClass="TextBox" Width="108px"/></td>

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
                       <td><asp:Label ID="lbTotPayForex" runat="server" CssClass="TextBox" Text = "Payment Forex"/></td>
                       <td><asp:Label ID="lbBankPayment" runat="server" CssClass="TextBox" Text = "Bank Payment"/></td>
                       <td>Selisih</td>                                              
                     </tr>
                     <tr>
                        <td><asp:TextBox style="text-align:right" runat="server" ValidationGroup="Input" ID="tbTotalPaymentForex" Enabled="false" CssClass="TextBoxR" Width="108px"/></td>
                        <td><asp:TextBox style="text-align:right" runat="server" ValidationGroup="Input" ID="tbBankPayment" Enabled="false" CssClass="TextBoxR" Width="108px"/></td>
                        <td><asp:TextBox style="text-align:right" runat="server" ValidationGroup="Input" ID="tbTotalSelisih" Enabled="false" CssClass="TextBoxR" Width="108px"/></td>                        
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
                <%-- <asp:MenuItem Text="Detail Invoice" Value="0"></asp:MenuItem> --%>
                <asp:MenuItem Text="Detail Payment" Value="1"></asp:MenuItem>                   
            </Items>            
        </asp:Menu>
        <br />
        <asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex="1">
    
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
    
              <asp:Panel runat="server" ID="PnlDt">
                <asp:Button class="bitbtn btnadd" runat="server" ID="btnAdddt" Text="Add" ValidationGroup="Input" />	
                  &nbsp;&nbsp;&nbsp;	
                  <asp:Button ID="btnBackDt2" visible = "false" runat="server" class="bitbtndt btncancel" 
                      Text="Back" validationgroup="Input" />
                <div style="border:0px  solid; width:100%; height:100%; overflow:auto;">
                    <asp:GridView ID="GridDt" runat="server" AutoGenerateColumns="False" 
                        ShowFooter="False">
                        <HeaderStyle CssClass="GridHeader" Wrap="false" />
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
                            <asp:BoundField DataField="Jenis" HeaderStyle-Width="80px" HeaderText="Pay To" />
                            <asp:BoundField DataField="LPInvoiceNo" HeaderStyle-Width="150px" HeaderText="INV No" />                          
                            <asp:BoundField DataField="TotalAmount" HeaderStyle-Width="80px" DataFormatString="{0:#,##0.00}" ItemStyle-HorizontalAlign="Right" HeaderText="Total Amount" />
                            <asp:BoundField DataField="Remark" HeaderStyle-Width="200px" HeaderText="Remark" />
                        </Columns>
                        
                    </asp:GridView>
              </div>   
              <asp:Button class="bitbtn btnadd" runat="server" ID="btnAddDtke2" Text="Add" Visible ="False" />	
                  &nbsp;&nbsp;&nbsp;	
                  <asp:Button ID="btnBackDt2Ke2" runat="server" class="bitbtndt btncancel" Visible ="False" 
                      Text="Back"  />
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
                        <td><asp:DropDownList CssClass="DropDownList" ID="ddlPaymentType" runat="server" Width="232px" AutoPostBack ="true"/>
                            <asp:TextBox CssClass ="TextBox" ID = "tbFgMode" runat ="server" Visible="True" enabled="false" Width="20px" />
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
                        <td><asp:TextBox CssClass="TextBox" runat="server" ID="tbDocumentNo" Width="225px"/> 
                            <asp:Button Class="btngo" ID="btnDocNo" Text="..." runat="server" MaxLength = "60"  />                                          
                        </td>
                        
                        <td>Voucher No</td>
                        <td>:</td>
                        <td><asp:TextBox CssClass="TextBox" runat="server" ID="tbVoucherNo" Width="225px"/>                            
                        <BDP:BasicDatePicker ID="tbPaymentDate" runat="server" DateFormat="dd MMM yyyy" 
                             ReadOnly = "true" ButtonImageHeight="19px" ButtonImageWidth="20px" 
                             DisplayType="TextBoxAndImage" TextBoxStyle-CssClass="TextDate" Visible="False"
                             ShowNoneButton="False"><TextBoxStyle CssClass="TextDate" /></BDP:BasicDatePicker>
                             
                        </td>
                    </tr>                    
                    <tr>
                        <td>Bank Payment</td>
                        <td>:</td>
                        <td><asp:DropDownList CssClass="DropDownList" ID="ddlBankPayment" Width="232px"  runat="server"/></td>
                    
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
                                 
                                    <td><asp:Label ID="lbDPRate" runat="server" CssClass="TextBox" Text="DP Rate" /></td>
                                </tr>
                                <tr>
                                    <td><asp:DropDownList CssClass="DropDownList" ID="ddlCurr" runat="server" Enabled="false" Width="60px"/></td>
                                    <td><asp:TextBox CssClass="TextBox" runat="server" ID="tbRate" Width="65px"/></td>
                   
                                    <td>
                                        <asp:TextBox ID="tbDPRate" runat="server" CssClass="TextBoxR" Width="65px" 
                                            Enabled="False" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                        
                        
                    </tr>   

                     <tr>
                        <td>Payment To</td>
                        <td>:</td>
                        <td colspan="4">
                            <table>
                                <tr style="background-color:Silver;text-align:center">
                                    <td>Payment To</td>   
                                    <td><asp:LinkButton ID="lbInvoice" ValidationGroup="Input" runat="server" Text="Invoice No"/></td>
                                    <td>Payment Nominal</td>                                  
                                </tr>
                                <tr>

                                    <td><asp:DropDownList CssClass="DropDownList" Width="183px" ValidationGroup="Input" Visible="True" runat="server" ID="ddlPayTo" AutoPostBack="True" />
                                        <td>
                                    
                                    <asp:TextBox runat="server" ValidationGroup="Input" ID="tbLpInv" CssClass="TextBox" Enabled="false"  Width="192px"/>
                                     <asp:Button ID="btnLpInv" runat="server" Class="btngo" visible = True Text="v" />
                                        </td>

                                     <td><asp:TextBox style="text-align:right" CssClass="TextBox" runat="server" ID="tbTotalAmount"  Maxlength = "13" Width="177px"/></td>
                                    
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
                                    
                                    <td><asp:Label ID="lbPayForex" runat="server" CssClass="TextBox" Text = "Bank Payment"/></td>
                                    <td><asp:Label ID="lbPayHome" runat="server" CssClass="TextBox" Text = "Payment Home"/></td>                                    
                                </tr>
                                <tr>
                                    
                                    <td><asp:TextBox style="text-align:right" CssClass="TextBox" runat="server" ID="tbPaymentForex"  Maxlength = "13" Width="177px"/></td>
                                    <td><asp:TextBox style="text-align:right" CssClass="TextBoxR" runat="server" ID="tbPaymentHome" Width="176px"/></td>
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
