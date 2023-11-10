<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TrCIPLicenAdmInv.aspx.vb" Inherits="Transaction_TrCIPLicenAdmInv_TrCIPLicenAdmInv" 
EnableEventValidation="false" %>

<%@ Register Assembly="BasicFrame.WebControls.BasicDatePicker" Namespace="BasicFrame.WebControls" TagPrefix="BDP" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title> <%--MaintainScrollPositionOnPostback="true" --%>
    <script type="text/javascript" src="../../Function/OpenDlg.js"></script>
    <script type="text/javascript" src="../../Function/Function.JS"></script>
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
        function OpenPopup() {
            var left = (screen.width - 600) / 2; //370
            var top = (screen.height - 600) / 2;
            window.open("../../earchDlgV.Aspx", "", 'toolbar=no, location=no, directories=no, status=no, menubar=no, scrollbars=yes, resizable=yes, copyhistory=no, width=' + 600 + ', height=' + 600 + ', top=' + top + ', left=' + left);
            return false;
        }
        function SetTextTerm() {
            var dropdownTerm = document.getElementById("ddlTerm");
            tbTerm.value = dropdownTerm.options[dropdownTerm.selectedIndex].value;
            //tbForexRate.value = dropdownTerm.options[dropdownTerm.selectedIndex].value;
            //var toDate = document.getElementById('<%=tbDate.ClientID %>_TextBox').value;
        }

        getResult = function(val) {
            db.transaction(function(transaction) {
            transaction.executeSql('SELECT xrange FROM MsTerm WHERE TermCode = ?;', [parseInt(val)], selectedRowValues, errorHandler);
            });
        };
        selectedRowValues = function(transaction, results) {
            for (var i = 0; i < results.rows.length; i++) {
                var row = results.rows.item(i);
                alert(row['xrange']);
            }
        };

        var datValue = $('select').val();
        $('#datepicker').datepicker();
        $('select').change(function() {
            $('#datepicker').datepicker('setDate', '+' + datValue + 'd').datepicker('show');
        });
        
        /*
        function setformat()
        {
        try
        {        
        var VPaymentF = document.getElementById("tbTotalPaymentForex").value.replace(/\$|\,/g,"");
        var VPayment = document.getElementById("tbTotalPayment").value.replace(/\$|\,/g,"");
        var VOther = document.getElementById("tbTotalOthers").value.replace(/\$|\,/g,"");
        var VExpense = document.getElementById("tbTotalExpense").value.replace(/\$|\,/g,"");
        var VCharge = document.getElementById("tbTotalCharge").value.replace(/\$|\,/g, "");
        var Selisih = document.getElementById("tbTotalSelisih").value.replace(/\$|\,/g, "");
        if (isNaN(Selisih) == true) {
        Selisih = 0;
        }

          //Selisih = VPayment + VOther - VExpense - VCharge
          
        document.getElementById("tbTotalPaymentForex").value = setdigit(VPaymentF,'<%=ViewState("DigitCurr")%>');
        document.getElementById("tbTotalPayment").value = setdigit(VPayment,'<%=ViewState("DigitHome")%>');
        document.getElementById("tbTotalOthers").value = setdigit(VOther,'<%=ViewState("DigitHome")%>');
        document.getElementById("tbTotalExpense").value = setdigit(VExpense,'<%=ViewState("DigitHome")%>');
        document.getElementById("tbTotalCharge").value = setdigit(VCharge, '<%=ViewState("DigitHome")%>');
        document.getElementById("tbTotalSelisih").value = setdigit(Selisih, '<%=ViewState("DigitHome")%>');
        }catch (err){
        alert(err.description);
        }      
        }   
        
        function setformatdt()
        {
        try
        {         
        var AmountForex = document.getElementById("tbAmountForexDt").value.replace(/\$|\,/g,""); 
        var AmountHome = document.getElementById("tbAmountHomeDt").value.replace(/\$|\,/g,""); 
        var Rate = document.getElementById("tbRateDt").value.replace(/\$|\,/g,""); 
         
        if(isNaN(Rate) == true)
        {
        Rate = 0;
        }
        if(isNaN(AmountForex) == true)
        {
        AmountForex = 0;
        }                         
        if(isNaN(AmountHome) == true)
        {
        AmountHome = 0;
        }  
        document.getElementById("tbRateDt").value = setdigit(Rate,'<%=ViewState("DigitRate")%>');
        document.getElementById("tbAmountForexDt").value = setdigit(AmountForex,'<%=VIEWSTATE("DigitCurrAcc")%>');
        document.getElementById("tbAmountHomeDt").value = setdigit(AmountHome,'<%=ViewState("DigitHome")%>');
         
        }catch (err){
        alert(err.description);
        }      
        }   

        function setformatdt2()
        {
        try
        {
        var Rate = document.getElementById("tbRateDt2").value.replace(/\$|\,/g,""); 
        //         var ChargeRate = document.getElementById("tbChargeRateDt2").value.replace(/\$|\,/g,""); 
        var PaymentForex = document.getElementById("tbPaymentForexDt2").value.replace(/\$|\,/g,""); 
        var PaymentHome = document.getElementById("tbPaymentHomeDt2").value.replace(/\$|\,/g,""); 
        //         var ChargeForex = document.getElementById("tbChargeForexDt2").value.replace(/\$|\,/g,""); 
        //         var ChargeHome = document.getElementById("tbChargeHomeDt2").value.replace(/\$|\,/g,""); 
        //                          
        document.getElementById("tbRateDt2").value = setdigit(Rate,'<%=ViewState("DigitRate")%>');
        //         document.getElementById("tbChargeRateDt2").value = setdigit(ChargeRate,'<%=ViewState("DigitRate")%>');
        document.getElementById("tbPaymentForexDt2").value = setdigit(PaymentForex,'<%=VIEWSTATE("DigitCurr")%>');
        document.getElementById("tbPaymentHomeDt2").value = setdigit(PaymentHome,'<%=ViewState("DigitHome")%>');
        //         document.getElementById("tbChargeForexDt2").value = setdigit(ChargeForex,'<%=VIEWSTATE("DigitExpenseCurr")%>');
        //         document.getElementById("tbChargeHomeDt2").value = setdigit(ChargeHome,'<%=ViewState("DigitHome")%>');
        }catch (err){
        alert(err.description);
        }
        } */

        function setformatforhd(prmchange) {
            try {
                document.getElementById("tbDPP").value = (parseFloat(document.getElementById("tbBaseForex").value.replace(/\$|\,/g, "")) - parseFloat(document.getElementById("tbDisc").value.replace(/\$|\,/g, "")));
                document.getElementById("tbPPnValue").value = (parseFloat(document.getElementById("tbDPP").value.replace(/\$|\,/g, "")) * parseFloat(document.getElementById("tbPPn").value.replace(/\$|\,/g, ""))) / 100;
                document.getElementById("tbPPhValue").value = (parseFloat(document.getElementById("tbDPP").value.replace(/\$|\,/g, "")) * parseFloat(document.getElementById("tbPPh").value.replace(/\$|\,/g, "")))/ 100;
                document.getElementById("tbTotalAmount").value = parseFloat(document.getElementById("tbDPP").value.replace(/\$|\,/g, "")) + parseFloat(document.getElementById("tbPPnValue").value.replace(/\$|\,/g, "")) - parseFloat(document.getElementById("tbPPhValue").value.replace(/\$|\,/g, ""));

                document.getElementById("tbBaseForex").value = setdigit(document.getElementById("tbBaseForex").value.replace(/\$|\,/g, ""), '<%=ViewState("DigitCurr")%>');
                document.getElementById("tbDisc").value = setdigit(document.getElementById("tbDisc").value.replace(/\$|\,/g, ""), '<%=ViewState("DigitCurr")%>');
                document.getElementById("tbDPP").value = setdigit(document.getElementById("tbDPP").value.replace(/\$|\,/g, ""), '<%=ViewState("DigitCurr")%>');
                document.getElementById("tbPPnValue").value = setdigit(document.getElementById("tbPPnValue").value.replace(/\$|\,/g, ""), '<%=ViewState("DigitCurr")%>');
                document.getElementById("tbPPhValue").value = setdigit(document.getElementById("tbPPhValue").value.replace(/\$|\,/g, ""), '<%=ViewState("DigitCurr")%>');
                document.getElementById("tbPPn").value = setdigit(document.getElementById("tbPPn").value.replace(/\$|\,/g, ""), '<%=ViewState("DigitCurr")%>');
                document.getElementById("tbPPh").value = setdigit(document.getElementById("tbPPh").value.replace(/\$|\,/g, ""), '<%=ViewState("DigitCurr")%>');
                document.getElementById("tbTotalAmount").value = setdigit(document.getElementById("tbTotalAmount").value.replace(/\$|\,/g, ""), '<%=ViewState("DigitCurr")%>');

            } catch (err) {
                alert(err.description);
            }
        }              
    </script> 
    
    <link type="text/css" rel="stylesheet" href="../../Styles/Style.css"/>    
    <style type="text/css">
        .style1 {width: 346px;}
        .style2 {width: 63px; }
        .Gridview-row 
        { font-size: 11.5px;
          background-color:#F7F6F3;color:#333333; cursor:auto;
        }
        .Gridview-row:hover {background-color:#e17009;cursor:pointer;}
       
    </style>
    <link type="text/css" rel="stylesheet" href="../../Styles/circularprogress.css" />           
    <link type="text/css" rel="stylesheet" href="../../Styles/jquery-ui.css" />  
    <script type="text/javascript" src="../../JQuery/jquery.min.js"></script>
    <script type="text/javascript" src="../../JQuery/jquery-ui.js"></script> 
    <%--<script type="text/javascript" src="../../JQuery/quicksearch.js"></script> 
    <script type="text/javascript" src="../../JQuery/ASPSnippets_Pager.min.js" ></script>--%>   
    <script type="text/javascript" src="../../JQuery/jquery-1.3.2.min.js"></script>
    <script type="text/javascript">
 /*-------------------------------------------------------------------------------------------*/    
     /* $(function() {
      $("[id*=btnRecvDoc]").click(function() {
              ShowPopupNoRecvDoc();
              return false;
          });
      });*/
      function ShowPopupNoRecvDoc() {
          $("#dlgNoRecvDoc").dialog({
          title: "List Terima Dokumen",
              width: 600, height: 500, modal: true
          });
      }
      $(document).ready(function() {
          $("#dlgNoRecvDoc tr").click(function(event) {
              var ID = $(this).find("td:nth-child(1)").html();
              var ApplfileNo = $(this).find("td:nth-child(3)").html();
              //var ApplfileDate = new Date($(this).find("td:nth-child(4)").text());
              //var ApplfileDate = Date.parse($(this).find("td:nth-child(4)").text());
              //var ApplfileDate = $(this).find("td:nth-child(4)").last().html()
              var HGBNo = $(this).find("td:nth-child(5)").html();
              var Remark = $(this).find("td:nth-child(9)").html();
              //var targetDate = new Date(targetVal)
              tbRecvDoc.value = ID
              tbApplfileNo.value = ApplfileNo
              tbApplfileDate.value = $(this).find("td:nth-child(4)").text();    //$(this).find("td:nth-child(4)").append(dialogtxt); //ApplfileDate
              //tbApplfileDate.SelectedValue = $(this).find("td:nth-child(4)").html();
              tbHGBNo.value = HGBNo
              tbRemark.value = Remark
          });
          ClosePopupRecvDoc()
      });
      
      function ClosePopupRecvDoc() {
          $('#dlgNoRecvDoc').dialog('close')
          $(this).dialog('close');
      }
/*-------------------------------------------------------------------------------------------*/
/*      $(function() {
        $("[id*=btnSupplier]").click(function() {
              ShowPopupSupplier();
              return false;
          });
      });*/
      function ShowPopupSupplier() {
          $("#dlgSupp").dialog({
              title: "List Supplier",
              width: 500, height: 450, modal: true
          });
      }
      $(document).ready(function() {
         $("#dlgSupp tr").click(function(event) {
              var ID = $(this).find("td:nth-child(1)").html();
              var strName = $(this).find("td:nth-child(2)").html();
              tbSuppCode.value = ID
              tbSuppName.value = strName
          });
          ClosePopupSupplier() 
      });
      function ClosePopupSupplier() {
          $('#dlgSupp').dialog('close')
          $(this).dialog('close');
      }

      function SearchTerm() {
          //return jQuery.trim($("[id*=tbTerm]").val());
          tbTerm.value = jQuery.trim($("[id*=ddlTerm]").val());
      };

      function GetDataXRange() {
          //var sdllTerm = document.getElementById("ddlTerm");
          var sdllTerm = document.getElementById('<%= ddlTerm.ClientID %>');
          //var sValue = document.getElementById('<%= tbTerm.ClientID %>');
          $.ajax({
              type: "GET",
              url: "Transaction_TrCIPLicenAdmInv_TrCIPLicenAdmInv.aspx/GetXRange",
            /*  data: '{Nilai: "' + $("#<%=tbTerm.ClientID%>")[0].value + '" }',      
              data: '{Nilai: "' + SearchTerm() + '", sTermCode: ' + sdllTerm + '}', */
              data: '{sTermCode: ' + Nilai + '}',
              contentType: "application/json; charset=utf-8",
              dataType: "json",
            /*  success: OnSuccess,
              failure: function(response) {
                  alert(response.d); */
               success: function(html) {
                 $('#tbTerm').val(html.trim());

              }
          });
      }
/*
      function getResult() {
          var x = document.getElementById('ddlTerm').value;
          dat = 'param=' + x;
          $.ajax({
              type: "POST",
              url: "Transaction_TrCIPLicenAdmInv_TrCIPLicenAdmInv.aspx/GetXRange",
              data: dat,
              cache: false,
              success: function(html) {
                $('#tbTerm').val(html.trim());
              }
          });
      }   
   */    
      function getdata(){
         var name = document.getElementById("ddlTerm");    
         if(name)
          {
           $.ajax({
           type: "GET",
           url: "Transaction_TrCIPLicenAdmInv_TrCIPLicenAdmInv.aspx/GetXRange",
           data: '{Nilai: "' + $("#<%=tbTerm.ClientID%>")[0].value + '" }',
           contentType: "application/json; charset=utf-8",
           dataType: "json",
           success: function (response) {
              $('#dlgTerm').html(response);
              }
           });
           }
         else
         {
          $('#dlgTerm').html("Pilih Term !!");
         }
      }
      /*-------------------------------------------------------------------------------------------*/
            
      function CheckUploadFile() {
          var sFileLengthRecv = $("#fupRecvFile")[0].files.length;
          var sFileLengthAppl = $("#fupApplFile")[0].files.length;          
          if (sFileLengthRecv === 0) {
              //alert("Belum ada file dokumen 1 (*.Pdf) yang di upload");
              if (confirm("Document file 1 (.Pdf) not yet uploaded. Are you sure upload file ?")) {
                  return false;
              } else {
                  return;
                  ProgressCircle();
              }
          }

          if (sFileLengthAppl === 0) {
              //alert("No file Pdf selected.");
              if (confirm("Document file 2 (.Pdf) not yet uploaded. Are you sure upload file ?")) {
                  return false;
              } else {
                  return;
                  ProgressCircle();      
              }
          }
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
      
      function CheckSizeDoc1(input) {
          const fileSize = input.files[0].size / 1024 / 1024; 
          if (fileSize > 3.5) {
            alert('Files document cannot be larger than 3.5 MB');
            document.getElementById("fupRecvFile").value = null;
            return false;
          } else { 
             return;
          }
        }
        
       function CheckSizeDoc2(input) {
          const fileSize = input.files[0].size / 1024 / 1024; 
          if (fileSize > 3.5) {
            alert('Files document cannot be larger than 3.5 MB');
            document.getElementById("fupApplFile").value = null;
            return false;
          } else { 
             return;
          }
        }

      /*       
      function RowDoubleClick() {
      var GRV = document.getElementById('<%= GVNoRecvDoc.ClientID %>');
      for (var rowId = 0; rowId < GRV.rows.length; rowId++) 
      {
      var cell = GRV.rows[rowId].cells[0];
      tbNoLandPurchase.value = cell.innerHTML;
      }
      }
      $(function() {
      $('.search_textbox').each(function(i) {
      $(this).quicksearch("[id*=GVNoRecvDoc] tr:not(:has(th))", {
      'testQuery': function(query, txt, row) {
      return $(row).children(":eq(" + i + ")").text().toLowerCase().indexOf(query[0].toLowerCase()) != -1;
      }
      });
      });
      });
      */
      /*-------------------------------------------------------------------------------------------*/
//      $(function() {
//          GetSupplier(1);
//      });
//      $("[id*=txtSearchSupplier]").live("keyup", function() {
//         GetSupplier(parseInt(1));
//      });
//      $(".Pager .page").live("click", function() {
//         GetSupplier(parseInt($(this).attr('page')));
//      });
//      function SearchTerm() {
//          return jQuery.trim($("[id*=txtSearchSupplier]").val());
//      };
//      function GetSupplier(pageIndex) {
//          $.ajax({
//              type: "POST",
//              url: "Transaction_TrCIPLicenAdmInv_TrCIPLicenAdmInv.aspx/GetSupplier",
//              data: '{searchTerm: "' + SearchTerm() + '", pageIndex: ' + pageIndex + '}',
//              contentType: "application/json; charset=utf-8",
//              dataType: "json",
//              success: OnSuccess,
//              failure: function(response) {
//                  alert(response.d);
//              },
//              error: function(response) {
//                  alert(response.d);
//              }
//          });
//      }
//      var row;
//      function OnSuccess(response) {
//          var xmlDoc = $.parseXML(response.d);
//          var xml = $(xmlDoc);
//          var customers = xml.find("MsSupplier");
//          if (row == null) {
//              row = $("[id*=GVSupplier] tr:last-child").clone(true);
//          }
//          $("[id*=GVSupplier] tr").not($("[id*=GVSupplier] tr:first-child")).remove();
//          if (customers.length > 0) {
//              $.each(customers, function() {
//                  var customer = $(this);
//                  $("td", row).eq(0).html($(this).find("SuppName").text());
//                  $("td", row).eq(1).html($(this).find("SuppCode").text());
//                  $("[id*=GVSupplier]").append(row);
//                  row = $("[id*=GVSupplier] tr:last-child").clone(true);
//              });
//              var pager = xml.find("Pager");
//              $(".Pager").ASPSnippets_Pager({
//                  ActiveCssClass: "current",
//                  PagerCssClass: "pager",
//                  PageIndex: parseInt(pager.find("PageIndex").text()),
//                  PageSize: parseInt(pager.find("PageSize").text()),
//                  RecordCount: parseInt(pager.find("RecordCount").text())
//              });

////              $(".cssSupplier").each(function() {
////                  var searchPattern = new RegExp('(' + SearchTerm() + ')', 'ig');
////                  $(this).html($(this).text().replace(searchPattern, "<span class = 'highlight'>" + SearchTerm() + "</span>"));
////              });
//              
//          } else {
//              var empty_row = row.clone(true);
//              $("td:first-child", empty_row).attr("colspan", $("td", row).length);
//              $("td:first-child", empty_row).attr("align", "center");
//              $("td:first-child", empty_row).html("No records found for the search criteria.");
//              $("td", empty_row).not($("td:first-child", empty_row)).remove();
//              $("[id*=GVSupplier]").append(empty_row);
//          }
//      };      
      /*-------------------------------------------------------------------------------------------*/
      /*
      function GetSelectedRow() {
      $("#dialog tr").click(function(event) {
      var ID = $(this).find("td:nth-child(1)").html();
      var HrgTanah = $(this).find("td:nth-child(2)").html();
      tbNoLandPurchase.value = ID
      tbBeaTanah.value = HrgTanah
      });
      }*/
      
    </script>    
    <%--<script type="text/javascript" src="../../JQuery/jquery-1.4.1.min.js"></script>
    <script type="text/javascript" src="../../JQuery/ScrollableGridPlugin.js"></script>
    <script type="text/javascript">
        $(document).ready(function() {
        $('#<%=GVNoRecvDoc.ClientID %>').Scrollable(); 
        });
    </script>--%> <%--GridView1/GVNoRecvDoc--%>
        
</head>
<body>
    <form id="form1" runat="server">
    <div class="Content">
    <div class="H1"><asp:Label runat="server" ID="lbTitle"></asp:Label></div>
     <hr style="color:Blue" />        
    <asp:Panel runat="server" id="PnlHd"> 
      <table>
        <tr>
            <td style="width:100px;text-align:right">Quick Search :</td>         
            <%--TransNmbr, TransDate, STATUS, FgReport, UserType, UserCode, UserName, Attn, Remark--%>   
            <td><asp:TextBox CssClass="TextBox" runat="server" ID ="tbFilter"/> 
                  <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlField" >                      
                    <asp:ListItem Selected="True" Value="TransNmbr">Invoice No</asp:ListItem>
                    <asp:ListItem Value="dbo.FormatDate(TransDate)">Invoice Date</asp:ListItem>
                    <asp:ListItem Value="Status">Status</asp:ListItem>
                    <asp:ListItem Value="SuppName">Nama Supplier</asp:ListItem>
                    <asp:ListItem Value="RecvDocNo">No.Terima Dokumen</asp:ListItem>
                    <asp:ListItem Value="dbo.FormatDate(SPSDate)">Tgl. SPS</asp:ListItem>
                    <asp:ListItem Value="dbo.FormatDate(DueDate)">Due Date</asp:ListItem>                                        
                    <%--<asp:ListItem>Attn</asp:ListItem>                                       
                    <asp:ListItem Value="DPNo">DP No</asp:ListItem>  
                    <asp:ListItem Value="DocumentNo">Document No</asp:ListItem>                  
                    <asp:ListItem Value="Voucher_No">Voucher No</asp:ListItem>
                    <asp:ListItem Value="PaymentType">Payment Type</asp:ListItem>
                    <asp:ListItem Value="Payment_Name">Payment Type Name</asp:ListItem>
                    <asp:ListItem Value="BankPayment">Bank Payment</asp:ListItem>
                    <asp:ListItem Value="BankPaymentName">Bank Payment Name</asp:ListItem>
                    <asp:ListItem Value="Account">Account</asp:ListItem>
                    <asp:ListItem Value="Accountname">Account Name</asp:ListItem>--%>
                  </asp:DropDownList>
                  <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlRange"></asp:DropDownList>
                  
                  <asp:Button class="bitbtn btnsearch" runat="server" ID="btnSearch" Text="Search" />																						 											  
                  <asp:Button class="bitbtn btngo" runat="server" ID="btnExpand" Text="..." />																						 											  
            </td>
            <td>
                <%--<asp:LinkButton ID="LbAdvSearch" runat="server" Text="Advanced Search" />--%>
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
                    <asp:ListItem Selected="True" Value="TransNmbr">Invoice No</asp:ListItem>
                    <asp:ListItem Value="dbo.FormatDate(TransDate)">Invoice Date</asp:ListItem>
                    <asp:ListItem Value="Status">Status</asp:ListItem>
                    <asp:ListItem Value="SuppName">Nama Supplier</asp:ListItem>
                    <asp:ListItem Value="RecvDocNo">No.Terima Dokumen</asp:ListItem>
                    <asp:ListItem Value="dbo.FormatDate(SPSDate)">Tgl. SPS</asp:ListItem>
                    <asp:ListItem Value="dbo.FormatDate(DueDate)">Due Date</asp:ListItem>                                        
                    <%--<asp:ListItem Value="DocumentNo">Document No</asp:ListItem>
                    <asp:ListItem Value="Voucher_No">Voucher No</asp:ListItem> 
                    <asp:ListItem Value="PaymentType">Payment Type</asp:ListItem>
                    <asp:ListItem Value="Payment_Name">Payment Type Name</asp:ListItem>
                    <asp:ListItem Value="BankPayment">Bank Payment</asp:ListItem>
                    <asp:ListItem Value="BankPaymentName">Bank Payment Name</asp:ListItem>
                    <asp:ListItem Value="Account">Account</asp:ListItem>
                    <asp:ListItem Value="Accountname">Account Name</asp:ListItem>--%>
              </asp:DropDownList>
          </td>              
        </tr>        
      </table>      
      </asp:Panel>
          <br />
            <asp:Button class="bitbtn btnadd" runat="server" ID="BtnAdd" Text="Add" />	
            &nbsp;                         
            <asp:DropDownList CssClass="DropDownList" ID="ddlCommand" runat="server" Visible="false"/>
            <asp:Button class="btngo" runat="server" ID="BtnGo" Text="G" Visible="false" />                         
          <br/>&nbsp;
          <div style="border:0px  solid; width:100%; height:100%; overflow:auto;">
          <asp:GridView ID="GridView1" runat="server" AllowPaging="True" AllowSorting="true" CssClass="Grid" AutoGenerateColumns="false"> 
              <HeaderStyle CssClass="GridHeader" Wrap="false" ></HeaderStyle>
			  <RowStyle CssClass="GridItem" Wrap="false" />
			  <AlternatingRowStyle CssClass="GridAltItem"/>
			  <PagerStyle CssClass="GridPager" />
              <Columns>              
                  <asp:TemplateField>
                      <HeaderTemplate>
                        <asp:CheckBox ID="cbSelectHd" runat="server" AutoPostBack="true" oncheckedchanged="cbSelectHd_CheckedChanged" />
                      </HeaderTemplate>
                      <ItemTemplate>
                        <asp:CheckBox ID="cbSelect" runat="server" />
                      </ItemTemplate>
                  </asp:TemplateField>
                  <asp:TemplateField HeaderStyle-Width="110" HeaderText="Action">
                      <ItemTemplate>
                          <asp:DropDownList CssClass="DropDownList" ID="ddl" runat="server">
                              <asp:ListItem Selected="True" Text="View" />
                              <asp:ListItem Text="Edit" />
                              <asp:ListItem Text="Print" />
                              <%--<asp:ListItem Text="Print Full" />--%>
                          </asp:DropDownList>
                          <asp:Button class="btngo" runat="server" ID="BtnGo" Text="G" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" CommandName="Go" />                
                       </ItemTemplate>
                      <HeaderStyle Width="110px" />
                  </asp:TemplateField>
                  <asp:BoundField DataField="TransNmbr" HeaderStyle-Width="120px" SortExpression="Nmbr" HeaderText="Invoice No"></asp:BoundField>                  
                  <asp:BoundField DataField="Status" ItemStyle-HorizontalAlign="Center" SortExpression="Status" HeaderText="Status"></asp:BoundField>                  
                  <asp:BoundField DataField="TransDate" dataformatstring="{0:dd MMM yyyy}" htmlencode="true" HeaderStyle-Width="80px" HeaderText="Invoice Date"></asp:BoundField>
                  <asp:BoundField DataField="SuppName" HeaderStyle-Width="120px" HeaderText="Supplier Name"></asp:BoundField>                  
                  <asp:BoundField DataField="RecvDocNo" HeaderStyle-Width="120px" HeaderText="No. Terima Dokumen"></asp:BoundField>                  
                  <asp:BoundField DataField="DueDate" dataformatstring="{0:dd MMM yyyy}" htmlencode="true" HeaderStyle-Width="80px" HeaderText="Due Date"></asp:BoundField>
                  <asp:BoundField DataField="SPSDate" dataformatstring="{0:dd MMM yyyy}" htmlencode="true" HeaderStyle-Width="80px" HeaderText="SPS Date"></asp:BoundField>
                  <%--<asp:BoundField DataField="FgReport" HeaderStyle-Width="80px" HeaderText="Report"></asp:BoundField>
                  <asp:BoundField DataField="UserType" HeaderStyle-Width="80px" SortExpression="UserType" HeaderText="User Type"></asp:BoundField>
                  <asp:BoundField DataField="UserPayment" HeaderStyle-Width="80px" SortExpression="UserPayment" HeaderText="User Payment"></asp:BoundField>
                  <asp:BoundField DataField="Attn" HeaderStyle-Width="200px" SortExpression="Attn" HeaderText="Attn"></asp:BoundField>
                  <asp:BoundField DataField="Currency" HeaderStyle-Width="80px" SortExpression="Currency" HeaderText="Currency"></asp:BoundField>--%>
                  <asp:BoundField DataField="BaseForex" DataFormatString="{0:#,##0.00}" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="80px" HeaderText="Biaya Total"></asp:BoundField>
                  <asp:BoundField DataField="DiscForex" DataFormatString="{0:#,##0.00}" ItemStyle-HorizontalAlign="Right"  HeaderStyle-Width="80px" HeaderText="Potongan"></asp:BoundField>
                  <asp:BoundField DataField="DPPForex" DataFormatString="{0:#,##0.00}" ItemStyle-HorizontalAlign="Right"  HeaderStyle-Width="80px" HeaderText="DPP Value"></asp:BoundField>
                  <asp:BoundField DataField="PPn" DataFormatString="{0:#,##0.00}" ItemStyle-HorizontalAlign="Right"  HeaderStyle-Width="80px" HeaderText="PPn %"></asp:BoundField>
                  <asp:BoundField DataField="PPnForex" DataFormatString="{0:#,##0.00}" ItemStyle-HorizontalAlign="Right"  HeaderStyle-Width="80px" HeaderText="PPn Value"></asp:BoundField>
                  <asp:BoundField DataField="PPh" DataFormatString="{0:#,##0.00}" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="80px" HeaderText="PPh %"></asp:BoundField>
                  <asp:BoundField DataField="PPhForex" DataFormatString="{0:#,##0.00}" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="80px" HeaderText="PPh Value"></asp:BoundField>
                  <asp:BoundField DataField="TotalForex" DataFormatString="{0:#,##0.00}" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="80px" HeaderText="Total Amount"></asp:BoundField>
                  <asp:BoundField DataField="PPHName" HeaderStyle-Width="100px" SortExpression="Remark" HeaderText="PPH Name"></asp:BoundField>                  
                  <asp:BoundField DataField="Remark" HeaderStyle-Width="250px" SortExpression="Remark" HeaderText="Remark"></asp:BoundField>
              </Columns>
          </asp:GridView>
          </div> <br/>
            <asp:Panel runat="server" ID ="pnlNav" Visible="false">
            <asp:Button class="bitbtn btnadd" runat="server" ID="btnAdd2" Text="Add" />	  
            &nbsp;
            <asp:DropDownList ID="ddlCommand2" CssClass="DropDownList" runat="server"/>
            <asp:Button class="btngo" runat="server" ID="btnGo2" Text="G" />                 
            </asp:Panel>
    </asp:Panel>    
    <asp:Panel runat="server" ID="pnlInput" Visible="false">
      <table>
              <tr>
                  <td>Invoice No</td>
                  <td>:</td>
                  <%--<td class="style1"> --%>
                  <td><asp:TextBox ID="tbCode" runat="server" CssClass="TextBoxR" Width="200px" Enabled="False" />
                  </td>
                  <td>CIP</td>
                  <td>:</td>
                  <td>
                      <asp:DropDownList ID="ddlCIP" runat="server" CssClass="DropDownList" ValidationGroup="Input"> <%--AutoPostBack="true"--%>
                          <asp:ListItem Selected="True">Izin</asp:ListItem>
                          <asp:ListItem>Administrasi</asp:ListItem>
                      </asp:DropDownList>
                  </td>
              </tr>                  
          <tr>
              <td>Invoice Date</td>                  
              <%--<td class="style2"> --%>
              <td>:</td>                  
              <td>
                  <BDP:BasicDatePicker ID="tbDate" runat="server" ButtonImageHeight="19px" 
                      ButtonImageWidth="20px" DateFormat="dd MMM yyyy" DisplayType="TextBoxAndImage" 
                      ReadOnly="true" ShowNoneButton="False" TextBoxStyle-CssClass="TextDate" 
                      ValidationGroup="Input">
                      <TextBoxStyle CssClass="TextDate" />
                  </BDP:BasicDatePicker>
              </td>
              <td>SPS Date</td>                  
              <%--<td class="style2"> --%>
              <td>:</td>                  
              <td>
                  <BDP:BasicDatePicker ID="tbSPSDate" runat="server" ButtonImageHeight="19px" 
                      ButtonImageWidth="20px" DateFormat="dd MMM yyyy" DisplayType="TextBoxAndImage" 
                      ReadOnly="true" ShowNoneButton="False" TextBoxStyle-CssClass="TextDate" 
                      ValidationGroup="Input">
                      <TextBoxStyle CssClass="TextDate" />
                  </BDP:BasicDatePicker>
              </td>
          </tr>
        <tr>
            <td><asp:LinkButton ID="lbSupplier" runat="server" ValidationGroup="Input" Text="Supplier/Instansi"/></td>
            <td>:</td>  
            <td>
              <asp:TextBox ID="tbSuppCode" runat="server" CssClass="TextBox" Width="80px" ValidationGroup="Input" /> <%--Enabled="false"--%>
              <asp:TextBox ID="tbSuppName" runat="server" CssClass="TextBox" Width="200px" ValidationGroup="Input" />
              <asp:Button ID="btnSupplier" runat="server" Class="btngo" Text="v" ValidationGroup="Input" />
            </td>                                                    
        </tr>         
        <tr>
            <td>No Terima Dokumen</td>
            <td>:</td>
            <td>
               <asp:TextBox ID="tbRecvDoc" runat="server" CssClass="TextBox" ValidationGroup="Input" Width="200px" AutoPostBack="false" />
               <asp:Button ID="btnRecvDoc" runat="server" Class="btngo" Text="v" ValidationGroup="Input" />                                  
            </td>
            <td>Dokumen 1</td>
            <td>:</td>
            <td>
               <asp:TextBox ID="tbDocNo1" runat="server" CssClass="TextBox" ValidationGroup="Input" Width="130px" />
               <asp:Button ID="btnDeleteDoc1" runat="server" class="bitbtn btndelete" Text="Delete" OnClientClick="return confirm('Sure to delete only document file 1 ?');" />	               
               <asp:FileUpload ID="fupRecvFile" style="color: White;" runat="server" accept=".pdf" onchange="CheckSizeDoc1(this)" />
               <asp:LinkButton ID="lbRecvFile" ValidationGroup="Input" runat="server" Width="200px" Text=""/>
            </td>
        </tr>
        <tr>
            <td>No Berkas Permohonan</td>
            <td>:</td>
            <td>
               <asp:TextBox ID="tbApplfileNo" runat="server" CssClass="TextBox" ValidationGroup="Input" Width="200px" AutoPostBack="false" />                                                
            </td>
            <td>Dokumen 2</td>
            <td>:</td>
            <td>
               <asp:TextBox ID="tbDocNo2" runat="server" CssClass="TextBox" ValidationGroup="Input" Width="130px" />
               <asp:Button ID="btnDeleteDoc2" runat="server" class="bitbtn btndelete" Text="Delete" OnClientClick="return confirm('Sure to delete only document file 2 ?');" />	            
               <asp:FileUpload ID="fupApplFile" style="color: White;" runat="server" accept=".pdf" onchange="CheckSizeDoc2(this)" />            
               <asp:LinkButton ID="lbApplFile" ValidationGroup="Input" runat="server" Width="200px" Text=""/>
            </td>            
        </tr>
        <tr>
            <td>Tgl Berkas Permohonan</td>
            <td>:</td>
            <td>
              <BDP:BasicDatePicker ID="tbApplfileDate" runat="server" DateFormat="dd MMM yyyy" 
                 ReadOnly = "true" ValidationGroup="Input"
                 ButtonImageHeight="19px" ButtonImageWidth="20px" 
                 DisplayType="TextBoxAndImage" 
                 TextBoxStyle-CssClass="TextDate" 
                 ShowNoneButton="False"><TextBoxStyle CssClass="TextDate" />
              </BDP:BasicDatePicker>                
            </td> 
        </tr>
        <tr>
            <td>Alas Hak</td>
            <td>:</td>
            <td>
               <asp:TextBox ID="tbHGBNo" runat="server" CssClass="TextBox" ValidationGroup="Input" Width="200px" AutoPostBack="false" />                                                
            </td>                                 
        </tr>  
        <tr>
            <td>Term</td>
            <td>:</td>
            <td>
              <asp:DropDownList ID="ddlTerm" runat="server" CssClass="DropDownList" ValidationGroup="Input" Width="130px" 
              Height="25px" AutoPostBack="true"  />             

              <BDP:BasicDatePicker ID="tbDueDate" runat="server" DateFormat="dd MMM yyyy" 
                 ReadOnly = "true" ValidationGroup="Input"
                 ButtonImageHeight="19px" ButtonImageWidth="20px" 
                 DisplayType="TextBoxAndImage" 
                 TextBoxStyle-CssClass="TextDate" 
                 ShowNoneButton="False"><TextBoxStyle CssClass="TextDate" />
              </BDP:BasicDatePicker>                                                          
            </td>  <%--onchange="SetTextTerm();" onchange="getResult('1');" OnSelectedIndexChanged="getdata" GetXRange(Nilai) GetDataXRange();--%>                      
        </tr>   
        <tr>
            <td>Currency</td>
            <td>:</td>
            <td>
               <asp:DropDownList ID="ddlCurr" runat="server" CssClass="DropDownList" ValidationGroup="Input" AutoPostBack="true" />
               <asp:TextBox ID="tbForexRate" runat="server" CssClass="TextBox" style="text-align:right;" ValidationGroup="Input" Width="75px" />  
               <asp:TextBox ID="tbTerm" runat="server" CssClass="TextBox" ValidationGroup="Input" Width="50px" Visible="false"  />          
            </td>  
            <td>PPh</td>
            <td>:</td>
            <td>
               <asp:DropDownList ID="ddlPPh" runat="server" CssClass="DropDownList" ValidationGroup="Input" AutoPostBack="true" />
               <asp:TextBox ID="tbType" runat="server" CssClass="TextBox" ValidationGroup="Input" Width="5px" Visible="true"  />          
            </td>  
                              
        </tr>   
        <tr>
            <td>Nominal</td>
            <td>:</td>
            <td colspan="4">
                <table>
                     <tr style="background-color:Silver;text-align:center">
                       <td><asp:Label ID = "lbTotBiaya" runat="server" CssClass="TextBox" Text = "Total Biaya"/></td>
                       <td><asp:Label ID = "lbDisc" runat="server" CssClass="TextBox" Text = "Potongan"/></td>
                       <td><asp:Label ID = "lbDPP" runat="server" CssClass="TextBox" Text = "DPP"/></td>
                       <td><asp:Label ID = "lbPPn" runat="server" CssClass="TextBox" Text = "PPn %"/></td>
                       <td><asp:Label ID = "lbPPnForex" runat="server" CssClass="TextBox" Text = "PPn Value"/></td>
                       <td><asp:Label ID = "lbPPh" runat="server" CssClass="TextBox" Text = "PPh %"/></td>
                       <td><asp:Label ID = "lbPPhForex" runat="server" CssClass="TextBox" Text = "PPh Value"/></td>
                       <td><asp:Label ID = "Label1" runat="server" CssClass="TextBox" Text = "Total Amount"/></td>
                     </tr>
                     <tr>
                        <td><asp:TextBox ID="tbBaseForex" runat="server" CssClass="TextBox" Width="90px" Enabled="false" ValidationGroup="Input" /></td>
                        <td><asp:TextBox ID="tbDisc" runat="server" CssClass="TextBox" Width="60px" Enabled="true" ValidationGroup="Input" /></td> <%--AutoPostBack="true"--%>
                        <td><asp:TextBox ID="tbDPP" runat="server" CssClass="TextBox" Width="80px" Enabled="false" ValidationGroup="Input" /></td> <%--AutoPostBack="true"--%>
                        <td><asp:TextBox ID="tbPPn" runat="server" CssClass="TextBox" Width="40px" Enabled="true" ValidationGroup="Input" /></td> <%--AutoPostBack="true"--%>
                        <td><asp:TextBox ID="tbPPnValue" runat="server" CssClass="TextBox" Width="80px" Enabled="false" ValidationGroup="Input" /></td>
                        <td><asp:TextBox ID="tbPPh" runat="server" CssClass="TextBox" Width="40px" Enabled="true" /></td> <%--AutoPostBack="true"--%>
                        <td><asp:TextBox ID="tbPPhValue" runat="server" CssClass="TextBox" Width="80px" Enabled="false" ValidationGroup="Input" /></td>
                        <td><asp:TextBox ID="tbTotalAmount" runat="server" CssClass="TextBox" Width="100px" Enabled="false" ValidationGroup="Input" /></td>
                     </tr>
                </table>
            </td>                         
        </tr> 
        <tr>
            <td>Remark</td>
            <td>:</td>
            <td><asp:TextBox ID="tbRemark" runat="server" ValidationGroup="Input" CssClass="TextBoxMulti" Width="300px" 
               MaxLength="255" TextMode="MultiLine" />
            </td>
        </tr>
      </table>  
      
      <br />      
      <hr style="color:Blue" />  
      <%--   <asp:Menu
            ID="Menu1"
            runat="server"
            CssClass = "Menu"        
            StaticMenuItemStyle-CssClass = "MenuItem"
            StaticSelectedStyle-CssClass = "MenuSelect"        
            Orientation="Horizontal"
            ItemWrap = "False"
            StaticEnableDefaultPopOutImage="False">            
            <Items>
                <asp:MenuItem Text="Detail Structure" Value="0"></asp:MenuItem>
                <asp:MenuItem Text="Detail Payment" Value="1"></asp:MenuItem>
            </Items>            
        </asp:Menu> --%>
        <br />
        <asp:MultiView 
        ID="MultiView1"
        runat="server"
        ActiveViewIndex="0">
           <asp:View ID="Tab1" runat="server">
              <asp:Panel runat="server" ID="PnlDt">
                <asp:Button ID="btnAdddt" runat="server" class="bitbtn btnadd" Text="Add" ValidationGroup="Input" />
                <%--<asp:FileUpload ID="fuLocation" runat="server" accept=".xls, .xlsx" style="color: White;" />
                <asp:Button ID="btnUpload" runat="server" class="bitbtn btnadd" Width="150px" Text="Upload File Excel" OnClick="btnUpload_Click" />--%> 
                <br/>&nbsp;	
                <div style="border:0px  solid; width:100%; height:100%; overflow:auto;">
                    <asp:GridView ID="GridDt" runat="server" AutoGenerateColumns="false" ShowFooter="False">
                        <HeaderStyle CssClass="GridHeader" />
                        <RowStyle CssClass="GridItem" Wrap="false" />
                        <AlternatingRowStyle CssClass="GridAltItem" />
                        <PagerStyle CssClass="GridPager" />
                        <Columns>
                            <asp:TemplateField HeaderText="Action"><ItemTemplate><asp:Button ID="btnEdit" runat="server" class="bitbtndt btnedit" Text="Edit" CommandName="Edit"/><asp:Button ID="btnDelete" runat="server" class="bitbtndt btndelete" Text="Delete" OnClientClick="return confirm('Sure to delete this data?');" CommandName="Delete"/></ItemTemplate></asp:TemplateField>
                            <asp:BoundField DataField="ItemNo" ItemStyle-HorizontalAlign="Right" HeaderText="Item No" /> <%--DataFormatString="{0:#}" --%>
                            <asp:BoundField DataField="JobName" HeaderStyle-Width="100px" HeaderText="Uraian Pekerjaan" ItemStyle-HorizontalAlign="Left" />
                            <asp:BoundField DataField="Qty" DataFormatString="{0:#,##0}" HeaderStyle-Width="40px" HeaderText="Jumlah" ItemStyle-HorizontalAlign="Right" />
                            <asp:BoundField DataField="UnitCode" HeaderStyle-Width="50px" HeaderText="Satuan" ItemStyle-HorizontalAlign="Left" />
                            <asp:BoundField DataField="PriceForex" DataFormatString="{0:#,##0}" HeaderStyle-Width="60px" HeaderText="Harga/Sat" ItemStyle-HorizontalAlign="Right" />
                            <asp:BoundField DataField="LocationName" HeaderStyle-Width="100px" HeaderText="Tempat Pengajuan" ItemStyle-HorizontalAlign="Left" />                            
                            <asp:BoundField DataField="BeaForex" DataFormatString="{0:#,##0}" HeaderStyle-Width="90px" ItemStyle-HorizontalAlign="Right" HeaderText="Biaya" />
                            <%--<asp:BoundField DataField="Amount" DataFormatString="{0:#,##0}" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Right" HeaderText="Subtotal" />--%>
                            <asp:BoundField DataField="Remark" HeaderStyle-Width="300px" HeaderText="Remark" ItemStyle-HorizontalAlign="Left" />
                            <%--<asp:BoundField DataField="AmountHome" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Right" HeaderText="Amount Home" />--%>
                        </Columns>
                    </asp:GridView>
              </div> <br/>&nbsp;  
              <asp:Button class="bitbtn btnadd" runat="server" ID="btnAddDtke2" Text="Add" ValidationGroup="Input" />	
              </asp:Panel> 
              <asp:Panel runat="server" ID="pnlEditDt" Visible="false">
                <table>              
                    <tr>
                        <td>Item No</td>
                        <td>:</td>
                        <td><asp:Label ID="lbItemNo" runat="server" Text="item" /> <%--<td colspan="4" --%>
                        </td>           
                    </tr>   
                    <tr> 
                        <td>Uraian Pekerjaan</td>
                        <td>:</td>
                        <td>
                          <asp:TextBox runat="server" ID="tbJobName" CssClass="TextBox" Width="365px" MaxLength="255" ValidationGroup="Input" AutoPostBack="False" />                         
                           <%--<asp:DropDownList ID="ddlUserType" runat="server" CssClass="DropDownList" ValidationGroup="Input" Width="100px" AutoPostBack ="False">
                             <asp:ListItem Selected="True">Izin</asp:ListItem>
                             <asp:ListItem>Admin</asp:ListItem>
                           </asp:DropDownList> --%>                       
                        </td>                       
                    </tr>                       
                    <tr> 
                        <td>Jumlah</td>
                        <td>:</td>
                        <td>                            
                            <asp:TextBox ID="tbQty" runat="server" CssClass="TextBox" Width="50px" ValidationGroup="Input" AutoPostBack="False"/>                                    
                            <%--<asp:TextBox CssClass="TextBoxR" runat="server" ID="tbAmountHomeDt" Width="150px"/>--%>                                    
                        </td>                        
                    </tr>
                    <tr>
                        <td>Satuan</td>
                        <td>:</td>
                        <td><asp:DropDownList ID="ddlUnit" runat="server" CssClass="DropDownList" Width="100px"/></td>
                    </tr>
                    <tr> 
                        <td>Harga per Unit</td>
                        <td>:</td>
                        <td>                            
                            <asp:TextBox ID="tbPriceForex" runat="server" CssClass="TextBox" Width="100px" ValidationGroup="Input" AutoPostBack="False"/>                                    
                        </td>                        
                    </tr>                                        
                    <tr> 
                        <td>Tempat Pengajuan</td>
                        <td>:</td>
                        <td>                            
                            <%--<asp:TextBox ID="tbLocationCode" runat="server" CssClass="TextBox" Width="80px" ValidationGroup="Input" AutoPostBack="true"/>--%>                                    
                            <asp:TextBox ID="tbLocationName" runat="server" CssClass="TextBox" Width="250px" ValidationGroup="Input"/>   
                            <%--<asp:Button ID="btnLocation" runat="server" Class="btngo" Text="..." ValidationGroup="Input" />--%>                                                             
                        </td>                        
                    </tr>                    
                    <tr>
                        <td>Biaya</td>
                        <td>:</td>
                        <td>                                
                            <asp:TextBox ID="tbBiaya" runat="server" CssClass="TextBox" Width="100px" ValidationGroup="Input" AutoPostBack="False" />
                            <%--<asp:TextBox runat="server" ID="tbFgSubledDt" visible="false" />
                            <asp:TextBox runat="server" ID="tbFgCostCtr" Visible="false" />                              
                            <asp:Button Class="btngo" ID="btnSubled" Text="..." runat="server" ValidationGroup="Input" />--%>                                                             
                        </td>
                    </tr>                         
                    <%--<tr>                    
                        <td>Structure Code</td>
                        <td>:</td>
                        <td>                                
                           <asp:DropDownList ID="ddlStructureCode" runat="server" CssClass="DropDownList" ValidationGroup="Input" Width="100px" AutoPostBack ="False">
                           </asp:DropDownList>                                                    
                            
                            <asp:TextBox CssClass="TextBox" ValidationGroup="Input" runat="server" ID="tbAccountDt" Width="150px" AutoPostBack="true" />                            
                            <asp:TextBox CssClass="TextBox" runat="server" ID="tbAccountNameDt" Enabled="false" Width="225px"/>
                            <asp:TextBox CssClass="TextBox" runat="server" ID="tbFgType" Visible="False" />
                            <asp:Button Class="btngo" ID="btnAccount" Text="..." runat="server" ValidationGroup="Input" />                                                              
                        </td>
                    </tr>--%>
                    <tr>
                        <td>Remark</td>
                        <td>:</td>
                        <td><asp:TextBox runat="server" ID="tbRemarkDt" CssClass="TextBox" Width="365px" 
                                MaxLength="255" TextMode="MultiLine" />                        
                        </td>
                    </tr>                    
                    <%--<tr>
                       <td>PPn</td>
                       <td>:</td>
                       <td colspan="2">
                       <table>
                        <tr style="background-color:Silver;text-align:center">
                            <td>PPn No</td>
                            <td>PPn Date</td>                            
                        </tr>
                        <tr>
                            <td><asp:TextBox ID="tbPPnNo" runat="server" CssClass="TextBox" ValidationGroup="Input" Enabled="false" Width="150px" /></td>
                            <td><BDP:BasicDatePicker ID="tbPPndate" runat="server" Enabled="false"
                                    ButtonImageHeight="19px" ButtonImageWidth="20px" DateFormat="dd MMM yyyy" 
                                    DisplayType="TextBoxAndImage" ReadOnly="true" ShowNoneButton="False" 
                                    TextBoxStyle-CssClass="TextDate" ValidationGroup="Input">
                                    <TextBoxStyle CssClass="TextDate" />
                                </BDP:BasicDatePicker>
                            </td>                            
                        </tr>
                       </table>
                     </td>                
                    </tr> --%>  

                    <%--<tr>
                        <td>Subtotal</td>
                        <td>:</td>
                        <td>  
                            <asp:TextBox ID="tbSubledNameDt" runat="server" CssClass="TextBox" Width="150px"/>                        
                        </td>                              
                    </tr>--%>
                    
                </table>
                <br />                     
                <asp:Button ID="btnSaveDt" runat="server" class="bitbtndt btnsave" Text="Save" />									
                <asp:Button ID="btnCancelDt" runat="server" class="bitbtndt btncancel" Text="Cancel" />									
           </asp:Panel> 
              
           </asp:View>           
            <asp:View ID="Tab2" runat="server">
                <asp:Panel ID="pnlDt2" runat="server">  
                <asp:Button class="bitbtn btnadd" runat="server" ID="btnAddDt2" Text="Add" ValidationGroup="Input" />	              
                <br/>&nbsp;
                <div style="border:0px  solid; width:100%; height:100%; overflow:auto;">
                    <asp:GridView ID="GridDt2" runat="server" AutoGenerateColumns="False" ShowFooter="False">
                        <HeaderStyle CssClass="GridHeader" />
                        <RowStyle CssClass="GridItem" Wrap="false" />
                        <AlternatingRowStyle CssClass="GridAltItem" />
                        <PagerStyle CssClass="GridPager" />
                        <Columns>
                            <asp:TemplateField HeaderText="Action"><ItemTemplate><asp:Button ID="btnEdit" runat="server" class="bitbtndt btnedit" Text="Edit" CommandName="Edit"/><asp:Button ID="btnDelete" runat="server" class="bitbtndt btndelete" Text="Delete" OnClientClick="return confirm('Sure to delete this data?');" CommandName="Delete"/></ItemTemplate><EditItemTemplate><asp:Button ID="btnUpdate" runat="server" class="bitbtndt btnsave" Text="Save" CommandName="Update"/><asp:Button ID="btnCancel" runat="server" class="bitbtndt btncancel" Text="Cancel" CommandName="Cancel"/></EditItemTemplate></asp:TemplateField>                                        
                            
                            <asp:BoundField DataField="ItemNo" HeaderText="No" />
                            <asp:BoundField DataField="PaymentType" HeaderStyle-Width="150px" HeaderText="Payment Type" ><HeaderStyle Width="150px" /></asp:BoundField>
                            <asp:BoundField DataField="DocumentNo" HeaderStyle-Width="150px" HeaderText="Document No" ><HeaderStyle Width="150px" /></asp:BoundField>
                            <%--<asp:BoundField DataField="Reference" HeaderStyle-Width="120px" HeaderText="Voucher No" >                            
                                <HeaderStyle Width="120px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="GiroDate" HeaderText="Giro Date" />
                            <asp:BoundField DataField="DueDate" HeaderStyle-Width="80px" HeaderText="Due Date" />--%>
                            <asp:BoundField DataField="BankPaymentName" HeaderStyle-Width="150px" HeaderText="Bank Payment" />                                
                            <asp:BoundField DataField="PaymentDate" HeaderStyle-Width="80px" DataFormatString="{0:dd/MMM/yyyy}" HeaderText="Payment Date" />
                            <asp:BoundField DataField="Nominal" DataFormatString="{0:#,##0}" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Right" HeaderText="Nominal" />                                
                            <%--<asp:BoundField DataField="PaymentHome" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Right" HeaderText="Payment Home" />
                            <asp:BoundField DataField="ChargeHome" HeaderStyle-Width="80px" HeaderText="Charge Home" />--%>                                
                            <asp:BoundField DataField="Remark" HeaderStyle-Width="300px" ItemStyle-HorizontalAlign="Left" HeaderText="Remark" />                                                        
                        </Columns>
                    </asp:GridView>
              </div> <br/>&nbsp;    
              <asp:Button class="bitbtn btnadd" runat="server" ID="btnAddDt2Ke2" Text="Add" ValidationGroup="Input" />	              
   
              </asp:Panel>
              <asp:Panel runat="server" ID="pnlEditDt2" Visible="false">
                <table>              
                    <tr>
                        <td>Item No</td>
                        <td>:</td>
                        <td><asp:Label ID="lbItemNoDt2" runat="server" Text="item" /><%--<td colspan="4" --%>
                        </td>           
                    </tr>        
                    <tr>
                        <td>Payment Type</td>
                        <td>:</td>
                        <td><asp:DropDownList CssClass="DropDownList" ID="ddlPayTypeDt2" runat="server" Width="230px" AutoPostBack ="False"/>
                            <%--<asp:TextBox CssClass ="TextBox" ID = "tbFgModeDt2" runat ="server" Visible="false" />--%>
                        </td>
                            
                        <%--<td>Payment Date</td>
                        <td>:</td>
                        <td>
                             <BDP:BasicDatePicker ID="tbGiroDateDt2" runat="server" DateFormat="dd MMM yyyy" 
                             ReadOnly = "true" ValidationGroup="Input" ButtonImageHeight="19px" ButtonImageWidth="20px" 
                             DisplayType="TextBoxAndImage" TextBoxStyle-CssClass="TextDate"
                             ShowNoneButton="False"> 
                             <TextBoxStyle CssClass="TextDate" />
                             </BDP:BasicDatePicker>
                        </td>--%>
                    </tr>
                    <tr>
                        <td>Document No</td>
                        <td>:</td>
                        <td><asp:TextBox ID="tbDocumentNoDt2" runat="server" CssClass="TextBox" Width="150px"/> 
                            <%--<asp:Button Class="btngo" ID="btnDocNo" Text="..." runat="server" /> --%>                                                         
                        </td>
                        <%--<td>Voucher No</td>
                        <td>:</td>
                        <td><asp:TextBox CssClass="TextBox" runat="server" ID="tbVoucherNo" Width="157px"/>
                            <BDP:BasicDatePicker ID="tbPaymentDateDt2" runat="server" DateFormat="dd MMM yyyy" 
                             ReadOnly = "true" ValidationGroup="Input" ButtonImageHeight="19px" ButtonImageWidth="20px" 
                             DisplayType="TextBoxAndImage" TextBoxStyle-CssClass="TextDate" Visible="false"
                             ShowNoneButton="False"> 
                             <TextBoxStyle CssClass="TextDate" />
                             </BDP:BasicDatePicker>
                        </td>--%>                         
                    </tr>
                    <tr>
                        <td>Bank Payment</td>
                        <td>:</td>
                        <td><asp:DropDownList CssClass="DropDownList" ID="ddlBankPaymentDt2" runat="server" Width="200px"/></td>                    
                    </tr> 
                    <tr>
                        <td>Payment Date</td>
                        <td>:</td>
                        <td>
                             <BDP:BasicDatePicker ID="tbDueDateDt2" runat="server" DateFormat="dd MMM yyyy" 
                             ReadOnly = "true" ButtonImageHeight="19px" ButtonImageWidth="20px" 
                             DisplayType="TextBoxAndImage" TextBoxStyle-CssClass="TextDate"
                             ShowNoneButton="False"><TextBoxStyle CssClass="TextDate" /></BDP:BasicDatePicker>
                        </td>                                               
                    </tr>  
                    <%--<tr> 
                        <td>Currency</td>
                        <td>:</td>
                        <td colspan="4">
                            <table>
                                <tr style="background-color:Silver;text-align:center">
                                    <td>Payment</td>
                                    <td>Rate</td>
                                    <td>Charge</td>
                                    <td>Rate</td>                                
                                </tr>
                                <tr>
                                    <td><asp:DropDownList CssClass="DropDownList" ID="ddlCurrDt2" runat="server" Enabled="false" Width="60px"/></td>
                                    <td><asp:TextBox CssClass="TextBox" runat="server" ID="tbRateDt2" Width="65px"/></td>
                                    <td><asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlChargeCurrDt2" Width="60px" AutoPostBack="True"/></td>
                                    <td><asp:TextBox CssClass="TextBox" runat="server" ID="tbChargeRateDt2" Width="65px"/></td>                                    
                                </tr>
                            </table>
                        </td>                                                
                    </tr>--%>  
                     
                    <tr>
                        <td>Nominal</td>
                        <td>:</td>
                        <td> <%--<td colspan="3"--%>
                           <asp:TextBox CssClass="TextBox" runat="server" ID="tbNominal" Width="150px"/>
                            <table>
                                <%--<tr style="background-color:Silver;text-align:center">
                                    <td><asp:Label ID="lbPayForex" CssClass="TextBox" runat="server" Text = "Payment Forex" /></td>
                                    <td><asp:Label ID="lbPayHome" CssClass="TextBox" runat="server" Text = "Payment Home" /></td>                                    
                                    <td><asp:Label ID="lbChargeForex" CssClass="TextBox" runat="server" Text = "Charge Forex" /></td>
                                    <td><asp:Label ID="lbChargeHome" CssClass="TextBox" runat="server" Text = "Charge Home" /></td>                               
                                </tr>--%> 
                                <tr>
                                    <%--<td><asp:TextBox CssClass="TextBox" runat="server" ID="tbPaymentForexDt2" Width="150px"/></td>
                                    <td><asp:TextBox CssClass="TextBoxR" runat="server" ID="tbPaymentHomeDt2" Width="80px"/></td>                                                                        
                                    <td><asp:TextBox CssClass="TextBox" runat="server" ID="tbChargeForexDt2" Width="80px"/></td>
                                    <td><asp:TextBox CssClass="TextBoxR" runat="server" ID="tbChargeHomeDt2" Width="80px"/></td>--%>
                                </tr>
                            </table>
                        </td>
                    </tr>                                   
                    <tr>
                        <td>Remark </td>
                        <td>:</td>
                        <td><asp:TextBox runat="server" ID="tbRemarkDt2" CssClass="TextBox" Width="365px" MaxLength="255" TextMode="MultiLine" />                        
                        </td>
                    </tr>
                </table>
                <br />                     
                <asp:Button ID="btnSaveDt2" runat="server" class="bitbtndt btnsave" Text="Save" />									
                <asp:Button ID="btnCancelDt2" runat="server" class="bitbtndt btncancel" Text="Cancel" />									

           </asp:Panel> 
               
            </asp:View>            
        </asp:MultiView>
    
       <br />          
		<asp:Button ID="btnSaveAll" runat="server" class="bitbtndt btnsavenew" Text="Save & New" validationgroup="Input" OnClientClick="return CheckUploadFile(this);" Width = "90px"/>									
        <asp:Button ID="btnSaveTrans" runat="server" class="bitbtndt btnsave" Text="Save" validationgroup="Input" OnClientClick="return CheckUploadFile(this);" />									
        <asp:Button ID="btnBack" runat="server" class="bitbtndt btncancel" Text="Cancel" validationgroup="Input"/>									
        <asp:Button ID="btnHome" runat="server" class="bitbtndt btnback" Text="Home" />									                                                           
    </asp:Panel>        
    </div>

      <div id="dlgNoRecvDoc" style="display:none;" > 
        <%--<asp:TextBox ID="TextBox1" runat="server" CssClass="TextBox" Width="80px"/> <br/>&nbsp; ; style="display:none; overflow:scroll"--%>  
        <asp:GridView ID="GVNoRecvDoc" runat="server" CssClass="Grid" AutoGenerateColumns="false" PageSize="10" AllowPaging="true" 
        OnPageIndexChanging="OnPageIndexChanging" OnDataBound="OnDataBound" >         
         <%--OnPageIndexChanging="OnPageIndexChanging" OnRowDataBound="GVNoRecvDoc_RowDataBound" OnSelectedIndexChanged="GVNoRecvDoc_SelectedIndexChanged" ondblclick="GetSelectedRow();" --%>
          <HeaderStyle CssClass="GridHeader" Wrap="false" ></HeaderStyle>
          <RowStyle CssClass="GridItem" Wrap = "false" /> <%--Gridview-row--%>
          <AlternatingRowStyle CssClass="GridAltItem"/>
          <PagerStyle CssClass="GridPager" HorizontalAlign="Left" />
          <Columns>                        
            <%--<asp:BoundField DataField="RowNumber" HeaderText="No" HeaderStyle-Width="40" ItemStyle-horizontalAlign ="Center" /> --%>
            <asp:BoundField DataField="TransNmbr" HeaderText="No. Trans" HeaderStyle-Width="90" ItemStyle-horizontalAlign ="Left" />
            <asp:BoundField DataField="TransDate" HeaderText="Trans Date" DataFormatString="{0:dd MMM yyyy}" ItemStyle-horizontalAlign ="Left" HeaderStyle-Width="80" /><%--ItemStyle-Width="150"--%>
            <asp:BoundField DataField="ApplfileNo" HeaderText="No. Berkas" ItemStyle-horizontalAlign ="Left" HeaderStyle-Width="100" />
            <%--<asp:BoundField DataField="ApplfileDate" DataFormatString="{0:dd MMM yyyy}" HeaderText="Tgl. Terima Doc" ItemStyle-horizontalAlign ="Left" HeaderStyle-Width="80" />--%><%--ItemStyle-Width="150"--%>
            <asp:BoundField DataField="ApplfileDate" HeaderText="Tgl. Berkas" DataFormatString="{0:dd MMM yyyy}" ItemStyle-horizontalAlign ="Left" HeaderStyle-Width="80" /><%--ItemStyle-Width="150"--%>
            <asp:BoundField DataField="HGBNo" HeaderText="Alas Hak" ItemStyle-horizontalAlign ="Left" HeaderStyle-Width="100" />
            <asp:BoundField DataField="PICName" HeaderText="Nama PIC" ItemStyle-horizontalAlign ="Left" HeaderStyle-Width="100" />
            <asp:BoundField DataField="BrokerName" HeaderText="Perantara" ItemStyle-horizontalAlign ="Left" HeaderStyle-Width="100" />
            <asp:BoundField DataField="RelatedOffcName" HeaderText="Pejabat Terkait" ItemStyle-horizontalAlign ="Left" HeaderStyle-Width="100" />
            <%--<asp:BoundField DataField="BiayaLainLain" DataFormatString="{0:#,##0}" HeaderText="Biaya Lain-lain" ItemStyle-horizontalAlign ="Right" HeaderStyle-Width="80" />--%>
            <asp:BoundField DataField="Remark" HeaderText="Remark" HeaderStyle-Width="150" ItemStyle-horizontalAlign ="Left" />            
          </Columns>
        </asp:GridView>
      </div> 
       
      <div id="dlgSupp" style="display:none;">
       Search : <asp:TextBox ID="txtSearchSupplier" runat="server" /> 
       <asp:Button ID="btnSearchSupplier" class="bitbtn btnsearch" runat="server" Text="Search" />
        <%--<hr />--%>
        <asp:GridView ID="GVSupplier" runat="server" CssClass="Grid" AutoGenerateColumns="false">
        <HeaderStyle CssClass="GridHeader" Wrap="false" ></HeaderStyle>
          <RowStyle CssClass="GridItem" Wrap = "false" /> <%--Gridview-row--%>
          <AlternatingRowStyle CssClass="GridAltItem"/>
          <PagerStyle CssClass="GridPager" HorizontalAlign="Left" />
          <Columns>
            <asp:BoundField HeaderStyle-Width="80px" DataField="SuppCode" HeaderText="Supplier Code" /><%--ItemStyle-CssClass="cssSupplier" --%>
            <asp:BoundField HeaderStyle-Width="150px" DataField="SuppName" HeaderText="Supplier Name" />
          </Columns>
        </asp:GridView>      
      </div>      
   
    <asp:Label runat ="server" ID="lbStatus" ForeColor="Red"/>
    
    <div class="loading" align="center">
      <br />
       <img src="../../Image/loader.gif" alt="" />
    </div>
        
    </form>
</body>
</html>
