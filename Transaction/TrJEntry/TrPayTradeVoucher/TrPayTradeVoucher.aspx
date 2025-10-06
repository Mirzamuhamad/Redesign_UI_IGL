<%@ Page MaintainScrollPositionOnPostback ="true" Language="VB" AutoEventWireup="false" CodeFile="TrPayTradeVoucher.aspx.vb" Inherits="Transaction_TrPayTradeVoucher" %>

<%@ Register Assembly="BasicFrame.WebControls.BasicDatePicker" Namespace="BasicFrame.WebControls"
    TagPrefix="BDP" %>
    
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Payment Voucher</title>
    <script src="../../Function/OpenDlg.js" type="text/javascript"></script>
    <script src="../../Function/Function.JS" type="text/javascript"></script>
    <link type="text/css" rel="stylesheet" href="../../Styles/circularprogress.css" /> 
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

    function OpenFind() {
        var left = (screen.width - 600) / 2; //370
        var top = (screen.height - 600) / 2;
        //window.open("../../earchDlgV.Aspx", "", 'toolbar=no, location=no, directories=no, status=no, menubar=no, scrollbars=yes, resizable=yes, copyhistory=no, width=' + 600 + ', height=' + 600 + ', top=' + top + ', left=' + left);
        window.open("../../FindDlg.Aspx", "", 'toolbar=no, location=no, directories=no, status=no, menubar=no, scrollbars=yes, resizable=yes, copyhistory=no, width=' + 600 + ', height=' + 600 + ', top=' + top + ', left=' + left);
        return false;
    }

    function setformathd(prmchange) {
        try {


            document.getElementById("tbTotalSelisih").value = parseFloat(document.getElementById("tbTotPayment").value.replace(/\$|\,/g, "")) - parseFloat(document.getElementById("tbTotBankPayment").value.replace(/\$|\,/g, "")) - parseFloat(document.getElementById("tbPotongan").value.replace(/\$|\,/g, ""));



            document.getElementById("tbTotalSelisih").value = setdigit(document.getElementById("tbTotalSelisih").value.replace(/\$|\,/g, ""), '<%=ViewState("DigitCurr")%>');
            document.getElementById("tbTotPayment").value = setdigit(document.getElementById("tbTotPayment").value.replace(/\$|\,/g, ""), '<%=ViewState("DigitCurr")%>');
            document.getElementById("tbTotBankPayment").value = setdigit(document.getElementById("tbTotBankPayment").value.replace(/\$|\,/g, ""), '<%=ViewState("DigitCurr")%>');


        } catch (err) {
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
                    <asp:ListItem Value="SuppCode">Supplier</asp:ListItem>
                    <asp:ListItem Value="Supplier_Name">Supplier Name</asp:ListItem>
                    <asp:ListItem Value="Attn">Attn</asp:ListItem>    
                    <asp:ListItem Value="Remark">Remark</asp:ListItem>                                    
                  </asp:DropDownList>
                  <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlRange"></asp:DropDownList>
                 
                  <asp:Button class="bitbtn btnsearch" runat="server" ID="btnSearch" Text="Search" />																						 											  
                  <asp:Button class="bitbtn btngo" runat="server" ID="btnExpand" Text="..." />																						 											   
            </td>
            <td>
                <asp:LinkButton ID="LbAdvSearch" runat="server" Text="Advanced Search" />
                &nbsp &nbsp &nbsp &nbsp
                <asp:Label runat="server" ID="Label1" Text="Outstanding Voucher : "/>
                <asp:LinkButton runat="server" ID="lbCount" Text="X" Font-Size="Small" />
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
                    <asp:ListItem Value="SuppCode">Supplier</asp:ListItem>
                    <asp:ListItem Value="Supplier_Name">Supplier Name</asp:ListItem>
                    <asp:ListItem Value="Attn">Attn</asp:ListItem>    
                    <asp:ListItem Value="Remark">Remark</asp:ListItem>   
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
              <HeaderStyle CssClass="GridHeader" Wrap="false"></HeaderStyle>
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
                  <asp:BoundField DataField="SuppCode" HeaderStyle-Width="200px" SortExpression="SuppCode" HeaderText="Supplier"></asp:BoundField>
                  <asp:BoundField DataField="Supplier_Name" HeaderStyle-Width="200px" SortExpression="Supplier_Name" HeaderText="Supplier Name"></asp:BoundField>
                  <asp:BoundField DataField="Attn" HeaderStyle-Width="150px" SortExpression="Attn" HeaderText="Attn"></asp:BoundField>
                
                  <asp:BoundField DataField="TotalPayment" DataFormatString="{0:#,##0.00}" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="80px" SortExpression="TotalPayment" HeaderText="Total Payment (Rp)"></asp:BoundField>
                  <asp:BoundField DataField="BankPayment" DataFormatString="{0:#,##0.00}" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="80px" SortExpression="BankPayment" HeaderText="Bank Payment (Rp)"></asp:BoundField>
                  <asp:BoundField DataField="Potongan" DataFormatString="{0:#,##0.00}" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="80px" SortExpression="Potongan" HeaderText="Potongan (Rp)"></asp:BoundField>
                 
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
            <td><asp:LinkButton ID="lbSupp" ValidationGroup="Input"  runat="server" Text="Supplier"/></td>
            <td>:</td>
            <td><asp:TextBox CssClass="TextBox" ValidationGroup="Input" runat="server" ID="tbSuppCode" AutoPostBack="true" />
                <asp:TextBox CssClass="TextBox" runat="server" ID="tbSuppName" Enabled="false" Width="225px"/>                
                <asp:Button Class="btngo" ID="btnSupp" Text="..." runat="server" ValidationGroup="Input" />  
                <asp:TextBox CssClass="TextBox" runat="server" ID="tbSuppType" Visible="false" Width="225px"/>                                                
                <asp:Label ID="lbItemNo7" runat="server" ForeColor="#FF3300" Text="*" />
            </td>
        </tr>
        
        <tr>
            <td>Attn</td>
            <td>:</td>
            <td><asp:TextBox runat="server" MaxLength = "60" ValidationGroup="Input" ID="tbAttn"  CssClass="TextBox" Width="225px"/></td>
        </tr>
        

        <tr>
            <td>Total</td>
            <td>:</td>
            <td colspan="4">
                <table>
                    <tr style="background-color:Silver;text-align:center">
                       <td><asp:Label ID="lbPaymentVoucher" runat="server" CssClass="TextBox" Text = "Payment Voucher"/></td>                     
                       <td><asp:Label ID="lbBankPayment" runat="server" CssClass="TextBox" Text = "Bank Payment"/></td>
                       <td><asp:Label ID="lbPotongan" runat="server" CssClass="TextBox" Text = "potongan"/></td>
                       <td><asp:Label ID="lbSelisih" runat="server" CssClass="TextBox" Text = "Selisih"/></td>
                     </tr>
                     <tr>
                        <td><asp:TextBox runat="server" ValidationGroup="Input" Enabled="false"  ID="tbTotPayment" CssClass="TextBox" Width="108px"/></td>
                        <td><asp:TextBox runat="server" ValidationGroup="Input" Enabled="false"  ID="tbTotBankPayment" CssClass="TextBox" Width="108px"/></td>
                        <td><asp:TextBox runat="server" ValidationGroup="Input" Enabled="false"  ID="tbPotongan" CssClass="TextBox" Width="108px"/></td>
                        <td><asp:TextBox runat="server" ValidationGroup="Input" Enabled="false"  ID="tbTotalSelisih" CssClass="TextBox" Width="108px"/></td>
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
            </td>
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
                <asp:MenuItem Text="Detail Voucher" Value="0"></asp:MenuItem>
                <asp:MenuItem Text="Detail Payment" Value="1"></asp:MenuItem>                   
            </Items>            
        </asp:Menu>
        <hr />
        <asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex="0">
    
            <asp:View ID="Tab2" runat="server">   
                                   
                <asp:Panel ID="pnlDt2" runat="server">
                
                 <asp:Button class="bitbtn btnadd" runat="server" ID="btnAddDt2" Text="Add" ValidationGroup="Input" />	          
                    &nbsp;&nbsp;&nbsp;	          
                 <asp:Button ID="btnGetInv" runat="server" class="bitbtn btnsearch" Text="Get Voucher" validationgroup="Input"/>									              
                      
                    <div style="border:0px  solid; width:100%; height:100%; overflow:auto;">
                        <asp:GridView ID="GridDt2" runat="server" AutoGenerateColumns="false" 
                            ShowFooter="False">
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

                                <asp:BoundField DataField="VoucherNo" HeaderStyle-Width="150px" HeaderText="Voucher No" />
                                <asp:BoundField DataField="Currency" HeaderStyle-Width="50px" HeaderText="Currency"></asp:BoundField>
                                 <asp:BoundField DataField="DueDate" dataformatstring="{0:dd MMM yyyy}" htmlencode="true" HeaderStyle-Width="100px" ItemStyle-wrap="true" HeaderText="Due Date" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" > </asp:BoundField>
                                <asp:BoundField DataField="AmountVoucher" DataFormatString="{0:#,##0.00}" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="100px"  HeaderText="Amount Voucher"></asp:BoundField>
                                <asp:BoundField DataField="Remark" HeaderStyle-Width="250px" SortExpression="Remark" HeaderText="Remark"></asp:BoundField>
                            </Columns>
                        </asp:GridView>
                    </div>    
                    <asp:Button class="bitbtn btnadd" runat="server" ID="btnAddDt2Ke2" Text="Add" ValidationGroup="Input" />	          

                </asp:Panel>
                <asp:Panel runat="server" ID="pnlEditDt2" Visible="false">
                    <table>  
                                        
                        <tr>
                            <td>Voucher No</td>
                            <td>:</td>
                            <td><asp:TextBox CssClass="TextBox" Enabled = "false" runat="server" ID="tbVoucher" Width="150px" ReadOnly="False" />
                                <asp:Button Class="btngo" ID="btnInvNo" Text="..." runat="server" />                                                                           <asp:TextBox runat="server" ID="tbFgValueDt2" Visible="false" />
                                <asp:Label ID="lbItemNo5" runat="server" ForeColor="#FF3300" Text="*" />
                            </td>
                   

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
                        <td> Amount Voucher </td>
                        <td>:</td>
                        <td><asp:TextBox CssClass="TextBox" ReadOnly="False" Enabled = "false" runat="server" ID="tbAmountVoucher"/>                         
                        </td>
                    </tr>  
                    
                    
                                
                    <tr>
                        <td>Remark</td>
                        <td>:</td>
                        <td colspan="4">
                            <asp:TextBox ID="tbRemarkdt" runat="server"  ValidationGroup="Input" CssClass="TextBox" MaxLength="255" 
                                TextMode="MultiLine" Width="365px"  />

                        </td>
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
                  <asp:Button ID="btnBackDt2" runat="server" class="bitbtndt btncancel" 
                      Text="Back" validationgroup="Input" />
                <div style="border:0px  solid; width:100%; height:100%; overflow:auto;">
                    <asp:GridView ID="GridDt" runat="server" AutoGenerateColumns="False" 
                        ShowFooter="False">
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
                        <td><asp:DropDownList CssClass="DropDownList" ID="ddlPaymentType" runat="server" AutoPostBack ="true"/>
                            <asp:TextBox CssClass ="TextBox" ID = "tbFgMode" runat ="server" Width="30px" Enabled="false" Visible="True" />
                            <asp:Label ID="lbItemNo0" runat="server" Text="*" ForeColor="#FF3300" />
                        </td>
                            
                        <td>Giro Date</td>
                        <td>:</td>
                        <td>
                             <BDP:BasicDatePicker ID="tbGiroDate" runat="server" DateFormat="dd MMM yyyy" 
                             ReadOnly = "true" ValidationGroup="Input" ButtonImageHeight="19px" ButtonImageWidth="20px" 
                             DisplayType="TextBoxAndImage" TextBoxStyle-CssClass="TextDate"
                             ShowNoneButton="False"><TextBoxStyle CssClass="TextDate" Width="157px"/></BDP:BasicDatePicker>
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
                        <td><asp:TextBox CssClass="TextBox" runat="server" ID="tbVoucherNo" Width="157px"/>                            
                        <BDP:BasicDatePicker ID="tbPaymentDate" runat="server" DateFormat="dd MMM yyyy" 
                             ReadOnly = "true" ButtonImageHeight="19px" ButtonImageWidth="20px" 
                             DisplayType="TextBoxAndImage" TextBoxStyle-CssClass="TextDate" Visible="False"
                             ShowNoneButton="False"><TextBoxStyle CssClass="TextDate" Width="157px"/></BDP:BasicDatePicker>
                             
                        </td>
                    </tr>                    
                    <tr>
                        <td>Bank Payment</td>
                        <td>:</td>
                        <td><asp:DropDownList CssClass="DropDownList" ID="ddlBankPayment" Width="230px" runat="server"/></td>
                    
                        <td>Due Date</td>
                        <td>:</td>
                        <td>
                             <BDP:BasicDatePicker ID="tbDueDate" runat="server" DateFormat="dd MMM yyyy" 
                             ReadOnly = "true" ButtonImageHeight="19px" ButtonImageWidth="20px" 
                             DisplayType="TextBoxAndImage" TextBoxStyle-CssClass="TextDate"
                             ShowNoneButton="False"><TextBoxStyle CssClass="TextDate" Width="157px"/></BDP:BasicDatePicker>
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
                                    <td style="background-color:white;text-align:center"><asp:Label ID="lbDPRate" runat="server" CssClass="TextBox" Text="DP Rate" /></td>
                                </tr>
                                <tr>
                                    <td><asp:DropDownList CssClass="DropDownList" ID="ddlCurr" runat="server" Enabled="false" Width="105px"/></td>
                                    <td><asp:TextBox CssClass="TextBox" runat="server" ID="tbRate" Width="110px"/></td>
                                   
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
                                    <td><asp:Label ID="lbPayHome" runat="server" CssClass="TextBox" Text = "Payment Home"/></td> 
                                </tr>
                                <tr>
                                    <td><asp:TextBox CssClass="TextBox" runat="server" ID="tbPaymentForex"  Maxlength = "13" Width="150px"/></td>
                                    <td><asp:TextBox CssClass="TextBoxR" runat="server" ID="tbPaymentHome" Width="150px"/></td> 
                            </table>
                        </td>
                    </tr>    

                    <tr>
                        <td>Remark </td>
                        <td>:</td>
                        <td colspan="4"><asp:TextBox runat="server" ID="tbRemarkDt2" CssClass="TextBox" Width="365px" 
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
    
     
    <div class="loading" align="center">
      <%--Loading. Please wait.<br />--%>
      <br />
       <img src="../../Image/loader.gif" alt="" />
    </div>
   
    </form>
</body>
</html>
