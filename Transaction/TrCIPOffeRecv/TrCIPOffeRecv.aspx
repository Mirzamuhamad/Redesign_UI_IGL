<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TrCIPOffeRecv.aspx.vb" Inherits="Transaction_TrCIPOffeRecv_TrCIPOffeRecv" 
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
                //document.getElementById("tbDPP").value = (parseFloat(document.getElementById("tbBaseForex").value.replace(/\$|\,/g, "")) - parseFloat(document.getElementById("tbDisc").value.replace(/\$|\,/g, "")));
                //document.getElementById("tbPPnValue").value = (parseFloat(document.getElementById("tbDPP").value.replace(/\$|\,/g, "")) * parseFloat(document.getElementById("tbPPn").value.replace(/\$|\,/g, ""))) / 100;
                //document.getElementById("tbPPhValue").value = (parseFloat(document.getElementById("tbDPP").value.replace(/\$|\,/g, "")) * parseFloat(document.getElementById("tbPPh").value.replace(/\$|\,/g, "")))/ 100;
                //document.getElementById("tbTotalAmount").value = parseFloat(document.getElementById("tbDPP").value.replace(/\$|\,/g, "")) + parseFloat(document.getElementById("tbPPnValue").value.replace(/\$|\,/g, "")) - parseFloat(document.getElementById("tbPPhValue").value.replace(/\$|\,/g, ""));

                document.getElementById("tbBaseForex").value = setdigit(document.getElementById("tbBaseForex").value.replace(/\$|\,/g, ""), '<%=ViewState("DigitCurr")%>');
                //document.getElementById("tbDisc").value = setdigit(document.getElementById("tbDisc").value.replace(/\$|\,/g, ""), '<%=ViewState("DigitCurr")%>');
                //document.getElementById("tbDPP").value = setdigit(document.getElementById("tbDPP").value.replace(/\$|\,/g, ""), '<%=ViewState("DigitCurr")%>');
                //document.getElementById("tbPPnValue").value = setdigit(document.getElementById("tbPPnValue").value.replace(/\$|\,/g, ""), '<%=ViewState("DigitCurr")%>');
                //document.getElementById("tbPPhValue").value = setdigit(document.getElementById("tbPPhValue").value.replace(/\$|\,/g, ""), '<%=ViewState("DigitCurr")%>');
                //document.getElementById("tbPPn").value = setdigit(document.getElementById("tbPPn").value.replace(/\$|\,/g, ""), '<%=ViewState("DigitCurr")%>');
                //document.getElementById("tbPPh").value = setdigit(document.getElementById("tbPPh").value.replace(/\$|\,/g, ""), '<%=ViewState("DigitCurr")%>');
                //document.getElementById("tbTotalAmount").value = setdigit(document.getElementById("tbTotalAmount").value.replace(/\$|\,/g, ""), '<%=ViewState("DigitCurr")%>');

                document.getElementById("tbPriceForex1").value = setdigit(document.getElementById("tbPriceForex1").value.replace(/\$|\,/g, ""), '<%=ViewState("DigitCurr")%>');
                document.getElementById("tbPriceForex2").value = setdigit(document.getElementById("tbPriceForex2").value.replace(/\$|\,/g, ""), '<%=ViewState("DigitCurr")%>');
                document.getElementById("tbPriceForex3").value = setdigit(document.getElementById("tbPriceForex3").value.replace(/\$|\,/g, ""), '<%=ViewState("DigitCurr")%>');
                document.getElementById("tbPriceForex4").value = setdigit(document.getElementById("tbPriceForex4").value.replace(/\$|\,/g, ""), '<%=ViewState("DigitCurr")%>');
                document.getElementById("tbPriceForex5").value = setdigit(document.getElementById("tbPriceForex5").value.replace(/\$|\,/g, ""), '<%=ViewState("DigitCurr")%>');

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
        /*
        table { border: 1px solid #ccc; }
        table th
        {
          background-color : #F7F7F7;
          color : #333;
          font-weight : bold;
        }
        table th, table td
        {
          padding : 5px;
          border-color  : #ccc;
        }
        .Pager span
        {
        color: #333;
        background-color: #F7F7F7;
        font-weight: bold;
        text-align: center;
        /*display: inline-block;*/
       /* width: 20px;
        margin-right: 3px;
        line-height: 150%;
        border: 1px solid #ccc;
        }
        .Pager a
        {
        text-align: center; */
        /*display: inline-block;*/
        /*width: 20px;
        border: 1px solid #ccc;
        color: #fff;
        color: #333;
        margin-right: 3px;
        line-height: 150%;
        text-decoration: none;
        }
        .highlight {background-color: #FFFFAF; }
        */
    </style>
    <link type="text/css" rel="stylesheet" href="../../Styles/circularprogress.css" />     
    <link type="text/css" rel="stylesheet" href="../../Styles/jquery-ui.css" />  
    <script type="text/javascript" src="../../JQuery/jquery.min.js"></script>
    <script type="text/javascript" src="../../JQuery/jquery-ui.js"></script> 
    <%--<script type="text/javascript" src="../../JQuery/quicksearch.js"></script> 
    <script type="text/javascript" src="../../JQuery/ASPSnippets_Pager.min.js" ></script>--%>   
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
      $(function() {
        $("[id*=btnArea]").click(function() {
               ShowPopupArea();
              return false;
          });
      });
      function ShowPopupArea() {
          $("#dlgArea").dialog({
              title: "List Area",
              width: 400, height: 450, modal: true
          });
      }
      $(document).ready(function() {
         $("#dlgArea tr").click(function(event) {
              var ID = $(this).find("td:nth-child(1)").html();
              var strName = $(this).find("td:nth-child(2)").html();
              tbAreaCode.value = ID
              tbAreaName.value = strName
          });
          ClosePopupArea() 
      });
      function ClosePopupArea() {
          $('#dlgArea').dialog('close')
          $(this).dialog('close');
      }
/*
      function chbx(obj) {          
          var that = obj;
          if (document.getElementById(that.id).checked == true) {
              document.getElementById('chkAcc1').checked = false;
              document.getElementById('chkAcc2').checked = false;
              document.getElementById('chkAcc3').checked = false;
              document.getElementById(that.id).checked = true;
          }
      }

      $(function() {
          $("input:checkbox").click(function() {
             var checkboxgroup = "input:checkbox[ID='" + $(this).attr("ID") + "']";
              $(checkboxgroup).attr("checked", false);
              $(this).attr("checked", true);
          });
      });
*/
      var selectedBox = null;
      $(document).ready(function() {
          $(".CekSupplier:not([checked])").on('change' , function(){
          $(".CekSupplier").not(this).prop("checked" , false);
           });      
      /*  $(".CekSupplier").click(function() {
              selectedBox = this.id;
              $(".CekSupplier").each(function() {
                  if (this.id == selectedBox) {
                      this.checked = true;
                  }
                  else {
                      this.checked = false;
                  };
              });
          }); */                     
      });

      function CheckUploadFile() {
          var sFileLengthFile1 = $("#fupOfferFile1")[0].files.length;
          var sFileLengthFile2 = $("#fupOfferFile2")[0].files.length;
          var sFileLengthFile3 = $("#fupOfferFile3")[0].files.length;
          var sFileLengthFile4 = $("#fupOfferFile4")[0].files.length;
          var sFileLengthFile5 = $("#fupOfferFile5")[0].files.length;
          
          if (sFileLengthFile1 === 0) {
              //alert("Document file 1 (.Pdf) not yet upload. Are you sure upload file ?"");
              if (confirm("Document file 1 (.Pdf) has not been uploaded. Do you want to upload the file ?")) {
                  return false;
              } else {
                  return;
              }
          }
          if (sFileLengthFile2 === 0) {
              if (confirm("Document file 2 (.Pdf) has not been uploaded. Do you want to upload the file ?")) {
                  return false;
              } else {
                  return;
              }
          }
          if (sFileLengthFile3 === 0) {
              if (confirm("Document file 3 (.Pdf) has not been uploaded. Do you want to upload the file ?")) {
                  return false;
              } else {
                  return;
              }
          }
          if (sFileLengthFile4 === 0) {
              if (confirm("Document file 4 (.Pdf) has not been uploaded. Do you want to upload the file ?")) {
                  return false;
              } else {
                  return;
              }
          }
          if (sFileLengthFile5 === 0) {
              if (confirm("Document file 5 (.Pdf) has not been uploaded. Do you want to upload the file ?")) {
                  return false;
              } else {
                  return;
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
            alert('Files document 1 cannot be larger than 3.5 MB');
            document.getElementById("fupOfferFile1").value = null;
            return false;
          } else { 
             return;
          }
        }
        
       function CheckSizeDoc2(input) {
          const fileSize = input.files[0].size / 1024 / 1024; 
          if (fileSize > 3.5) {
            alert('Files document 2 cannot be larger than 3.5 MB');
            document.getElementById("fupOfferFile2").value = null;
            return false;
          } else { 
             return;
          }
        }
        
       function CheckSizeDoc3(input) {
       const fileSize = input.files[0].size / 1024 / 1024; 
          if (fileSize > 3.5) {
            alert('Files document 3 cannot be larger than 3.5 MB');
            document.getElementById("fupOfferFile3").value = null;
            return false;
          } else { 
             return;
          }
        }
        
       function CheckSizeDoc4(input) {
       const fileSize = input.files[0].size / 1024 / 1024; 
          if (fileSize > 3.5) {
            alert('Files document 4 cannot be larger than 3.5 MB');
            document.getElementById("fupOfferFile4").value = null;
            return false;
          } else { 
             return;
          }
        }
        
       function CheckSizeDoc5(input) {
       const fileSize = input.files[0].size / 1024 / 1024; 
          if (fileSize > 3.5) {
            alert('Files document 5 cannot be larger than 3.5 MB');
            document.getElementById("fupOfferFile5").value = null;
            return false;
          } else { 
             return;
          }
        }

/*-------------------------------------------------------------------------------------------*/

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
      }
      */      
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
            <td><asp:TextBox CssClass="TextBox" runat="server" ID ="tbFilter"/> 
                  <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlField" >                      
                    <asp:ListItem Selected="True" Value="TransNmbr">No. Penerimaan</asp:ListItem>
                    <asp:ListItem Value="dbo.FormatDate(TransDate)">Penerimaan Date</asp:ListItem>
                    <asp:ListItem Value="Status">Status</asp:ListItem>
                    <asp:ListItem Value="InstruksiNo">No. Instruksi</asp:ListItem>
                    <asp:ListItem Value="SuppName">Supplier Name</asp:ListItem>                                      
                    <%--<asp:ListItem Value="dbo.FormatDate(TimeJobDate)">Waktu Pelaksanaan</asp:ListItem>
                    <asp:ListItem Value="CIPAdm">CIP</asp:ListItem>                                        
                    <asp:ListItem>Attn</asp:ListItem>                                       
                    <asp:ListItem Value="DPNo">DP No</asp:ListItem>  
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
                    <asp:ListItem Selected="True" Value="TransNmbr">No. Penerimaan</asp:ListItem>
                    <asp:ListItem Value="dbo.FormatDate(TransDate)">Penerimaan Date</asp:ListItem>
                    <asp:ListItem Value="Status">Status</asp:ListItem>
                    <asp:ListItem Value="InstruksiNo">No. Instruksi</asp:ListItem>
                    <asp:ListItem Value="SuppName">Supplier Name</asp:ListItem>                                      
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
                  <asp:BoundField DataField="TransNmbr" HeaderStyle-Width="120px" SortExpression="Nmbr" HeaderText="Instruksi No"></asp:BoundField>                  
                  <asp:BoundField DataField="Status" ItemStyle-HorizontalAlign="Center" SortExpression="Status" HeaderText="Status"></asp:BoundField>                  
                  <asp:BoundField DataField="TransDate" dataformatstring="{0:dd MMM yyyy}" htmlencode="true" HeaderStyle-Width="80px" HeaderText="Date"></asp:BoundField>
                  <asp:BoundField DataField="InstruksiNo" HeaderStyle-Width="100px" HeaderText="Instruksi No"></asp:BoundField>                  
                  <asp:BoundField DataField="SuppName" HeaderStyle-Width="120px" HeaderText="Supplier Name"></asp:BoundField>                  
                  <asp:BoundField DataField="BaseForex" DataFormatString="{0:#,##0.00}" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="80px" HeaderText="Total HPS"></asp:BoundField>
                  <%--<asp:BoundField DataField="TimeJobDate" dataformatstring="{0:dd MMM yyyy}" htmlencode="true" HeaderStyle-Width="80px" HeaderText="Waktu Pelaksanaan"></asp:BoundField>
                  <asp:BoundField DataField="StartJobDate" dataformatstring="{0:dd MMM yyyy}" htmlencode="true" HeaderStyle-Width="80px" HeaderText="Mulai Pelaksanaan"></asp:BoundField>
                  <asp:BoundField DataField="FinishJobDate" dataformatstring="{0:dd MMM yyyy}" htmlencode="true" HeaderStyle-Width="80px" HeaderText="Selesai Pelaksanaan"></asp:BoundField>
                  <asp:BoundField DataField="Durasi" DataFormatString="{0:#,##0}" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="80px" HeaderText="Durasi (Hari)"></asp:BoundField>--%>
                  <asp:BoundField DataField="Remark" HeaderStyle-Width="250px" SortExpression="Remark" HeaderText="Remark"></asp:BoundField>
              </Columns>
          </asp:GridView>
          </div> <br/>
            <asp:Panel runat="server" ID ="pnlNav" Visible="false">
            <asp:Button class="bitbtn btnadd" runat="server" ID="btnAdd2" Text="Add" />	  
            &nbsp
            <asp:DropDownList ID="ddlCommand2" CssClass="DropDownList" runat="server"/>
            <asp:Button class="btngo" runat="server" ID="btnGo2" Text="G" />                 
            </asp:Panel>
    </asp:Panel>    
    <asp:Panel runat="server" ID="pnlInput" Visible="false">
      <table>
              <tr>
                  <td>No Penerimaan</td>
                  <td>:</td>
                  <td><asp:TextBox ID="tbCode" runat="server" CssClass="TextBoxR" Width="200px" Enabled="False" /></td> 
                  <%--</td>
                  <td style="width: 30px"></td>  
                  <td>Lokasi Pekerjaan</td>
                  <td>:</td>  
                  <td>
                    <asp:TextBox ID="tbAreaCode" runat="server" CssClass="TextBox" Width="80px" ValidationGroup="Input" /> Enabled="false"
                    <asp:TextBox ID="tbAreaName" runat="server" CssClass="TextBox" Width="200px" ValidationGroup="Input" />                    
                  </td>--%>                                                                      
              </tr>                  
          <tr>
              <td>Instruksi Date</td>                  
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
              <%--<td style="width: 30px"></td>
              <td>Waktu Pelaksanaan</td>                  
              <td class="style2"> 
              <td>:</td>                  
              <td>
                  <BDP:BasicDatePicker ID="tbTimeJobDate" runat="server" ButtonImageHeight="19px" 
                      ButtonImageWidth="20px" DateFormat="dd MMM yyyy" DisplayType="TextBoxAndImage" 
                      ReadOnly="true" ShowNoneButton="False" TextBoxStyle-CssClass="TextDate" 
                      ValidationGroup="Input">
                      <TextBoxStyle CssClass="TextDate" />
                  </BDP:BasicDatePicker>
              </td>--%>
          </tr>
        <tr>
            <td>Instruksi No</td>
            <td>:</td>
            <td>
               <asp:TextBox ID="tbInstruksiNo" runat="server" CssClass="TextBox" ValidationGroup="Input" Width="200px" AutoPostBack="false" />
               <asp:Button ID="btnInstruksiNo" runat="server" Class="btngo" Text="v" ValidationGroup="Input" />
            </td>
            <%--<td style="width: 30px"></td>
            <td>Mulai Pelaksanaan</td>
            <td>:</td>
            <td>
              <BDP:BasicDatePicker ID="tbStartJobDate" runat="server" DateFormat="dd MMM yyyy" 
                 ReadOnly = "true" ValidationGroup="Input"
                 ButtonImageHeight="19px" ButtonImageWidth="20px" 
                 DisplayType="TextBoxAndImage" 
                 TextBoxStyle-CssClass="TextDate" 
                 ShowNoneButton="False"><TextBoxStyle CssClass="TextDate" />
              </BDP:BasicDatePicker>                
            </td>--%>            
        </tr>
        <tr>
            <td>Total HPS</td>
            <td>:</td>
            <td>
               <asp:TextBox ID="tbBaseForex" runat="server" CssClass="TextBox" ValidationGroup="Input" Width="100px" AutoPostBack="false" />                                                
            </td>
            <%--<td style="width: 30px"></td>
            <td>Selesai Pelaksanaan</td>
            <td>:</td>
            <td>
              <BDP:BasicDatePicker ID="tbFinishJobDate" runat="server" DateFormat="dd MMM yyyy" 
                 ReadOnly = "true" ValidationGroup="Input"
                 ButtonImageHeight="19px" ButtonImageWidth="20px" 
                 DisplayType="TextBoxAndImage" 
                 TextBoxStyle-CssClass="TextDate" 
                 ShowNoneButton="False"><TextBoxStyle CssClass="TextDate" />
              </BDP:BasicDatePicker>                
            </td>--%>                         
        </tr>

            <%--<td><asp:TextBox ID="TextBox1" runat="server" ValidationGroup="Input" CssClass="TextBox" Width="50px" AutoPostBack="false" />
                <asp:TextBox ID="TextBox2" runat="server" AutoPostBack="false"   CssClass="TextBox" ValidationGroup="Input" Width="200px" />
                <asp:Button ID="Button1" runat="server" Class="btngo" Text="v" ValidationGroup="Input" />
            </td>--%>
        
        <tr>
            <td>Penawaran 1</td>            
            <td>:</td>            
            <td>
                <table>                                
                     <tr style="background-color:Silver;text-align:center">
                       <td><asp:Label ID = "lbSupplier" runat="server" CssClass="TextBox" Text = "Peserta"/></td>
                       <td><asp:Label ID = "lbOfferNo" runat="server" CssClass="TextBox" Text = "No Penawaran"/></td>
                       <td><asp:Label ID = "lbPPn" runat="server" CssClass="TextBox" Text = "File Penawaran"/></td>
                       <td><asp:Label ID = "lbPPnForex" runat="server" CssClass="TextBox" Text = "Mulai Pelaksanaan"/></td>
                       <td><asp:Label ID = "lbPPh" runat="server" CssClass="TextBox" Text = "Selesai Pelaksanaan"/></td>
                       <td><asp:Label ID = "lbPPhForex" runat="server" CssClass="TextBox" Text = "Durasi"/></td>
                       <td><asp:Label ID = "lbDPP" runat="server" CssClass="TextBox" Text = "Harga"/></td>                       
                       <td><asp:Label ID = "Label1" runat="server" CssClass="TextBox" Text = "Cek"/></td>
                     </tr>
                     <tr>
                        <td><asp:TextBox ID="tbSuppCode1" runat="server" Visible = "false" CssClass="TextBox" Width="50px" Enabled="false" ValidationGroup="Input" />
                        <asp:TextBox ID="tbSuppName1" runat="server" CssClass="TextBox" Width="120px" Enabled="false" ValidationGroup="Input" />
                        <asp:Button ID="btnSupplier1" runat="server" Class="btngo" Text="v" ValidationGroup="Input" /></td>
                        
                        <td><asp:TextBox ID="tbOfferNo1" runat="server" CssClass="TextBox" Width="80px" Enabled="true" ValidationGroup="Input" /></td> 
                        <td>
                           <asp:Button ID="btnDeleteDoc1" runat="server" class="bitbtn btndelete" Width="15px" Text="" OnClientClick="return confirm('Sure to delete only document file 1 ?');" />	               
                           <asp:FileUpload ID="fupOfferFile1" style="color: White;" runat="server" accept=".pdf" onchange="CheckSizeDoc1(this)" />
                           <asp:LinkButton ID="lbOfferFile1" ValidationGroup="Input" runat="server" Text=""/> <%--accept="application/pdf"--%>
                        </td> 
                        <td>
                           <BDP:BasicDatePicker ID="tbStartJobDate1" runat="server" ReadOnly="true" ShowNoneButton="False" AutoPostBack="true"  
                             ButtonImageHeight="19px" ButtonImageWidth="20px" DateFormat="dd MMM yyyy" DisplayType="TextBoxAndImage" 
                             TextBoxStyle-CssClass="TextDate" ValidationGroup="Input">
                             <TextBoxStyle CssClass="TextDate" />
                           </BDP:BasicDatePicker>                        
                        </td>
                        <td>
                           <BDP:BasicDatePicker ID="tbFinishJobDate1" runat="server" ReadOnly = "true" ShowNoneButton="False" AutoPostBack="true"                               
                             ButtonImageHeight="19px" ButtonImageWidth="20px" DateFormat="dd MMM yyyy" DisplayType="TextBoxAndImage" 
                             TextBoxStyle-CssClass="TextDate" ValidationGroup="Input" >
                             <TextBoxStyle CssClass="TextDate" />
                           </BDP:BasicDatePicker> 
                        </td>
                        <td>                        
                        <asp:TextBox ID="tbDurasi1" runat="server" CssClass="TextBox" Width="40px" Enabled="False" />
                        </td> 
                        <td><asp:TextBox ID="tbPriceForex1" runat="server" CssClass="TextBox" Width="60px" Enabled="false" ValidationGroup="Input" /></td> 
                        
                          <td>
                              <%--<asp:CheckBox ID="chkAcc1" runat="server" AutoPostBack="true" />--%> 
                              <input id="chkAcc1" runat="server" type="checkbox" class="CekSupplier" />   
                          </td> 
                     </tr>                                                  
                </table>                                
            </td>                                                                                                                 
        </tr>
        <tr>
            <td>Penawaran 2</td>            
            <td>:</td>
            <td>
                <table>                     
                     <tr>
                         <td>
                             <asp:TextBox ID="tbSuppCode2" Visible = "false" runat="server" CssClass="TextBox" Enabled="false" ValidationGroup="Input" Width="50px" />
                             <asp:TextBox ID="tbSuppName2" runat="server" CssClass="TextBox" Enabled="false" ValidationGroup="Input" Width="120px" />
                             <asp:Button ID="btnSupplier2" runat="server" Class="btngo" Text="v" ValidationGroup="Input" />
                         </td>
                         
                        <td><asp:TextBox ID="tbOfferNo2" runat="server" CssClass="TextBox" Width="80px" Enabled="true" ValidationGroup="Input" /></td> 
                        <td>
                           <asp:Button ID="btnDeleteDoc2" runat="server" class="bitbtn btndelete" Width="15px" Text="" OnClientClick="return confirm('Sure to delete only document file 2 ?');" />	               
                           <asp:FileUpload ID="fupOfferFile2" style="color: White;" runat="server" accept=".pdf" onchange="CheckSizeDoc2(this)" />
                           <asp:LinkButton ID="lbOfferFile2" ValidationGroup="Input" runat="server"  Text=""/>
                        </td> 
                        <td>
                           <BDP:BasicDatePicker ID="tbStartJobDate2" runat="server" ReadOnly="true" ShowNoneButton="False" AutoPostBack="true" 
                             ButtonImageHeight="19px" ButtonImageWidth="20px" DateFormat="dd MMM yyyy" DisplayType="TextBoxAndImage" 
                             TextBoxStyle-CssClass="TextDate" ValidationGroup="Input">
                             <TextBoxStyle CssClass="TextDate" />
                           </BDP:BasicDatePicker>                        
                        </td>
                        <td>
                           <BDP:BasicDatePicker ID="tbFinishJobDate2" runat="server" ReadOnly = "true" ShowNoneButton="False" AutoPostBack="true"                             
                             ButtonImageHeight="19px" ButtonImageWidth="20px" DateFormat="dd MMM yyyy" DisplayType="TextBoxAndImage" 
                             TextBoxStyle-CssClass="TextDate" ValidationGroup="Input" >
                             <TextBoxStyle CssClass="TextDate" />
                           </BDP:BasicDatePicker> 
                        </td>
                        <td><asp:TextBox ID="tbDurasi2" runat="server" CssClass="TextBox" Width="40px" Enabled="False" /></td> 
                        <td><asp:TextBox ID="tbPriceForex2" runat="server" CssClass="TextBox" Width="60px" Enabled="false" ValidationGroup="Input" /></td>                         
                        <td>
                            <%--<asp:CheckBox ID="chkAcc2" runat="server" AutoPostBack="true" />--%>
                            <input id="chkAcc2" runat="server" type="checkbox" class="CekSupplier" />
                        </td>                           
                     </tr>
                </table>
            </td>
        </tr>       
        <tr>
            <td>Penawaran 3</td>            
            <td>:</td>
            <td>
                <table>                     
                     <tr>
                         <td>
                             <asp:TextBox ID="tbSuppCode3" Visible = "false" runat="server" CssClass="TextBox" Enabled="false" ValidationGroup="Input" Width="50px" />
                             <asp:TextBox ID="tbSuppName3" runat="server" CssClass="TextBox" Enabled="false" ValidationGroup="Input" Width="120px" />
                             <asp:Button ID="btnSupplier3" runat="server" Class="btngo" Text="v" ValidationGroup="Input" />
                         </td>
                         
                        <td><asp:TextBox ID="tbOfferNo3" runat="server" CssClass="TextBox" Width="80px" Enabled="true" ValidationGroup="Input" /></td> 
                        <td>
                           <asp:Button ID="btnDeleteDoc3" runat="server" class="bitbtn btndelete" Width="15px" Text="" OnClientClick="return confirm('Sure to delete only document file 3 ?');" />	               
                           <asp:FileUpload ID="fupOfferFile3" style="color: White;" runat="server" accept=".pdf" onchange="CheckSizeDoc3(this)" />
                           <asp:LinkButton ID="lbOfferFile3" ValidationGroup="Input" runat="server"  Text=""/>
                        </td> 
                        <td>
                           <BDP:BasicDatePicker ID="tbStartJobDate3" runat="server" ReadOnly="true" ShowNoneButton="False" AutoPostBack="true"  
                             ButtonImageHeight="19px" ButtonImageWidth="20px" DateFormat="dd MMM yyyy" DisplayType="TextBoxAndImage" 
                             TextBoxStyle-CssClass="TextDate" ValidationGroup="Input">
                             <TextBoxStyle CssClass="TextDate" />
                           </BDP:BasicDatePicker>                        
                        </td>
                        <td>
                           <BDP:BasicDatePicker ID="tbFinishJobDate3" runat="server" ReadOnly = "true" ShowNoneButton="False" AutoPostBack="true"                             
                             ButtonImageHeight="19px" ButtonImageWidth="20px" DateFormat="dd MMM yyyy" DisplayType="TextBoxAndImage" 
                             TextBoxStyle-CssClass="TextDate" ValidationGroup="Input" >
                             <TextBoxStyle CssClass="TextDate" />
                           </BDP:BasicDatePicker> 
                        </td>
                        <td><asp:TextBox ID="tbDurasi3" runat="server" CssClass="TextBox" Width="40px" Enabled="False" /></td> 
                        <td><asp:TextBox ID="tbPriceForex3" runat="server" CssClass="TextBox" Width="60px" Enabled="false" ValidationGroup="Input" /></td>                         
                        <td>
                          <%--<asp:CheckBox ID="chkAcc3" runat="server" AutoPostBack="true" />--%>
                          <input id="chkAcc3" runat="server" type="checkbox" class="CekSupplier" />
                        </td>                           
                     </tr>
                </table>
            </td>                
        </tr>    
        <tr>
            <td>Penawaran 4</td>            
            <td>:</td>
            <td>
                <table>                     
                     <tr>
                         <td>
                             <asp:TextBox ID="tbSuppCode4" Visible = "false" runat="server" CssClass="TextBox" Enabled="false" ValidationGroup="Input" Width="50px" />
                             <asp:TextBox ID="tbSuppName4" runat="server" CssClass="TextBox" Enabled="false" ValidationGroup="Input" Width="120px" />
                             <asp:Button ID="btnSupplier4" runat="server" Class="btngo" Text="v" ValidationGroup="Input" />
                         </td>
                         
                        <td><asp:TextBox ID="tbOfferNo4" runat="server" CssClass="TextBox" Width="80px" Enabled="true" ValidationGroup="Input" /></td> 
                        <td>
                           <asp:Button ID="btnDeleteDoc4" runat="server" class="bitbtn btndelete" Width="15px" Text="" OnClientClick="return confirm('Sure to delete only document file 4 ?');" />	               
                           <asp:FileUpload ID="fupOfferFile4" style="color: White;" runat="server" accept=".pdf" onchange="CheckSizeDoc4(this)" />
                           <asp:LinkButton ID="lbOfferFile4" ValidationGroup="Input" runat="server"  Text=""/>
                        </td> 
                        <td>
                           <BDP:BasicDatePicker ID="tbStartJobDate4" runat="server" ReadOnly="true" ShowNoneButton="False" AutoPostBack="true" 
                             ButtonImageHeight="19px" ButtonImageWidth="20px" DateFormat="dd MMM yyyy" DisplayType="TextBoxAndImage" 
                             TextBoxStyle-CssClass="TextDate" ValidationGroup="Input">
                             <TextBoxStyle CssClass="TextDate" />
                           </BDP:BasicDatePicker>                        
                        </td>
                        <td>
                           <BDP:BasicDatePicker ID="tbFinishJobDate4" runat="server" ReadOnly = "true" ShowNoneButton="False" AutoPostBack="true"                             
                             ButtonImageHeight="19px" ButtonImageWidth="20px" DateFormat="dd MMM yyyy" DisplayType="TextBoxAndImage" 
                             TextBoxStyle-CssClass="TextDate" ValidationGroup="Input" >
                             <TextBoxStyle CssClass="TextDate" />
                           </BDP:BasicDatePicker> 
                        </td>
                        <td><asp:TextBox ID="tbDurasi4" runat="server" CssClass="TextBox" Width="40px" Enabled="False" /></td> 
                        <td><asp:TextBox ID="tbPriceForex4" runat="server" CssClass="TextBox" Width="60px" Enabled="false" ValidationGroup="Input" /></td>                         
                        <td>
                          <%--<asp:CheckBox ID="chkAcc4" runat="server" AutoPostBack="true" />--%>
                          <input id="chkAcc4" runat="server" type="checkbox" class="CekSupplier" />
                        </td>                           
                     </tr>
                </table>
            </td>                
        </tr>         
        <tr>
            <td>Penawaran 5</td>            
            <td>:</td>
            <td>
                <table>                     
                     <tr>
                         <td>
                             <asp:TextBox ID="tbSuppCode5" Visible = "false" runat="server" CssClass="TextBox" Enabled="false" ValidationGroup="Input" Width="50px" />
                             <asp:TextBox ID="tbSuppName5" runat="server" CssClass="TextBox" Enabled="false" ValidationGroup="Input" Width="120px" />
                             <asp:Button ID="btnSupplier5" runat="server" Class="btngo" Text="v" ValidationGroup="Input" />
                         </td>
                         
                        <td><asp:TextBox ID="tbOfferNo5" runat="server" CssClass="TextBox" Width="80px" Enabled="true" ValidationGroup="Input" /></td> 
                        <td>
                           <asp:Button ID="btnDeleteDoc5" runat="server" class="bitbtn btndelete" Width="15px" Text="" OnClientClick="return confirm('Sure to delete only document file 5 ?');" />	               
                           <asp:FileUpload ID="fupOfferFile5" style="color: White;" runat="server" accept=".pdf" onchange="CheckSizeDoc5(this)" />
                           <asp:LinkButton ID="lbOfferFile5" ValidationGroup="Input" runat="server" Text=""/>
                        </td> 
                        <td>
                           <BDP:BasicDatePicker ID="tbStartJobDate5" runat="server" ReadOnly="true" ShowNoneButton="False" AutoPostBack="true" 
                             ButtonImageHeight="19px" ButtonImageWidth="20px" DateFormat="dd MMM yyyy" DisplayType="TextBoxAndImage" 
                             TextBoxStyle-CssClass="TextDate" ValidationGroup="Input">
                             <TextBoxStyle CssClass="TextDate" />
                           </BDP:BasicDatePicker>                        
                        </td>
                        <td>
                           <BDP:BasicDatePicker ID="tbFinishJobDate5" runat="server" ReadOnly = "true" ShowNoneButton="False" AutoPostBack="true"                             
                             ButtonImageHeight="19px" ButtonImageWidth="20px" DateFormat="dd MMM yyyy" DisplayType="TextBoxAndImage" 
                             TextBoxStyle-CssClass="TextDate" ValidationGroup="Input" >
                             <TextBoxStyle CssClass="TextDate" />
                           </BDP:BasicDatePicker> 
                        </td>
                        <td><asp:TextBox ID="tbDurasi5" runat="server" CssClass="TextBox" Width="40px" Enabled="False" /></td> 
                        <td><asp:TextBox ID="tbPriceForex5" runat="server" CssClass="TextBox" Width="60px" Enabled="false" ValidationGroup="Input" /></td>                         
                        <td>
                          <%--<asp:CheckBox ID="chkAcc5" runat="server" AutoPostBack="true" />--%>
                          <input id="chkAcc5" runat="server" type="checkbox" class="CekSupplier" />
                        </td>                           
                     </tr>
                </table>
            </td>                
        </tr>         
                    
        <tr>    
                <td>
                    Remark</td>
                <td>
                    :</td>
                <td>
                    <asp:TextBox ID="tbRemark" runat="server" CssClass="TextBoxMulti" 
                        MaxLength="255" TextMode="MultiLine" ValidationGroup="Input" Width="300px" />
                </td>
                <%--<td style="width: 30px"></td>
            <td>Durasi</td>
            <td>:</td>
            <td>
               <asp:TextBox ID="tbDurasi" runat="server" CssClass="TextBox" ValidationGroup="Input" Width="50px" AutoPostBack="false" /> 
               <asp:Label ID = "lbHari" runat="server" Font-Size="Medium" Text = "Hari"/>                                               
            </td>--%>
            
            <%--<td style="width: 30px"></td>
            <td>Durasi</td>
            <td>:</td>
            <td>
               <asp:TextBox ID="tbDurasi" runat="server" CssClass="TextBox" ValidationGroup="Input" Width="50px" AutoPostBack="false" /> 
               <asp:Label ID = "lbHari" runat="server" Font-Size="Medium" Text = "Hari"/>                                               
            </td>--%>
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
                <asp:Button class="bitbtn btnadd" runat="server" ID="btnAdddt" Text="Add" ValidationGroup="Input" /><br/>&nbsp;	
                <div style="border:0px  solid; width:100%; height:100%; overflow:auto;">
                    <asp:GridView ID="GridDt" runat="server" AutoGenerateColumns="false" ShowFooter="False">
                        <HeaderStyle CssClass="GridHeader" />
                        <RowStyle CssClass="GridItem" Wrap="false" />
                        <AlternatingRowStyle CssClass="GridAltItem" />
                        <PagerStyle CssClass="GridPager" />
                        <Columns>
                            <asp:TemplateField HeaderText="Action"><ItemTemplate><asp:Button ID="btnEdit" runat="server" class="bitbtndt btnedit" Text="Edit" CommandName="Edit"/><asp:Button ID="btnDelete" runat="server" class="bitbtndt btndelete" Text="Delete" OnClientClick="return confirm('Sure to delete this data?');" CommandName="Delete"/></ItemTemplate></asp:TemplateField>
                            <asp:BoundField DataField="ItemNo" HeaderText="Item No" />
                            <asp:BoundField DataField="SuppName" HeaderStyle-Width="100px" HeaderText="Supplier Name" ItemStyle-HorizontalAlign="Left" />
                            <asp:BoundField DataField="OfferNo" HeaderStyle-Width="100px" HeaderText="No. Penawaran" ItemStyle-HorizontalAlign="Left" />
                            <%--<asp:BoundField DataField="Area" DataFormatString="{0:#,##0}" HeaderStyle-Width="40px" HeaderText="Harga" ItemStyle-HorizontalAlign="Right" />
                            <asp:BoundField DataField="UnitCode" HeaderStyle-Width="50px" HeaderText="Satuan" ItemStyle-HorizontalAlign="Left" />--%>
                            <asp:BoundField DataField="PriceForex" DataFormatString="{0:#,##0.00}" HeaderStyle-Width="80px" HeaderText="Harga" ItemStyle-HorizontalAlign="Right" />
                            <asp:BoundField DataField="OfferFile" HeaderStyle-Width="100px" HeaderText="File Penawaran" ItemStyle-HorizontalAlign="Left" />                            
                            <asp:BoundField DataField="StartJobDate" dataformatstring="{0:dd MMM yyyy}" htmlencode="true" HeaderStyle-Width="80px" HeaderText="Mulai Pelaksanaan"></asp:BoundField>
                            <asp:BoundField DataField="FinishJobDate" dataformatstring="{0:dd MMM yyyy}" htmlencode="true" HeaderStyle-Width="80px" HeaderText="Selesai Pelaksanaan"></asp:BoundField>
                            <asp:BoundField DataField="Durasi" DataFormatString="{0:#,##0}" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="80px" HeaderText="Durasi (Hari)"></asp:BoundField>
                            <%--<asp:BoundField DataField="Remark" HeaderStyle-Width="300px" HeaderText="Remark" ItemStyle-HorizontalAlign="Left" />
                            <asp:BoundField DataField="AmountHome" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Right" HeaderText="Amount Home" />--%>
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
                        <td>Luas</td>
                        <td>:</td>
                        <td>                            
                            <asp:TextBox ID="tbQty" runat="server" CssClass="TextBox" Width="50px" ValidationGroup="Input" AutoPostBack="False"/>                                    
                            <%--<asp:TextBox CssClass="TextBoxR" runat="server" ID="tbAmountHomeDt" Width="150px"/>--%>                                    
                        </td>                        
                    </tr>
                    <%--<tr>
                        <td>Satuan</td>
                        <td>:</td>
                        <td><asp:DropDownList ID="ddlUnit" runat="server" CssClass="DropDownList" Width="100px"/></td>
                    </tr>--%>
                    <tr> 
                        <td>Estimasi/HPS</td>
                        <td>:</td>
                        <td>                            
                            <asp:TextBox ID="tbPriceForex" runat="server" CssClass="TextBox" Width="100px" ValidationGroup="Input" AutoPostBack="False"/>                                    
                        </td>                        
                    </tr>                                        
                    <%--<tr> 
                        <td>Tempat Pengajuan</td>
                        <td>:</td>
                        <td>                            
                            <asp:TextBox ID="tbLocationCode" runat="server" CssClass="TextBox" Width="80px" ValidationGroup="Input" AutoPostBack="true"/>                                   
                            <asp:TextBox ID="tbLocationName" runat="server" CssClass="TextBox" Width="250px" ValidationGroup="Input"/>   
                            <asp:Button ID="btnLocation" runat="server" Class="btngo" Text="..." ValidationGroup="Input" />                                                             
                        </td>                        
                    </tr>                    
                    <tr>
                        <td>Biaya</td>
                        <td>:</td>
                        <td>                                
                            <asp:TextBox ID="tbBiaya" runat="server" CssClass="TextBox" Width="100px" ValidationGroup="Input" AutoPostBack="False" />
                            <asp:Button Class="btngo" ID="btnSubled" Text="..." runat="server" ValidationGroup="Input" />                                                            
                        </td>
                    </tr>                         
                    <tr>                    
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
                            <td>
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
                <asp:Button class="bitbtn btnadd" runat="server" ID="btnAddDt2" Text="Add" ValidationGroup="Input" Visible="false" />	              
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
              <asp:Button class="bitbtn btnadd" runat="server" ID="btnAddDt2Ke2" Text="Add" ValidationGroup="Input" Visible="false" />	              
   
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
		<asp:Button ID="btnSaveAll" runat="server" class="bitbtndt btnsavenew" Text="Save & New" validationgroup="Input" OnClientClick="return CheckUploadFile(this);" Width = "90"/>									
        <asp:Button ID="btnSaveTrans" runat="server" class="bitbtndt btnsave" Text="Save" validationgroup="Input" OnClientClick="return CheckUploadFile(this);"/>									
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
       
      <div id="dlgArea" style="display:none;">
       <%--Search : <asp:TextBox ID="txtSearchArea" runat="server" /> 
       <asp:Button ID="btnSearchArea" class="bitbtn btnsearch" runat="server" Text="Search" />--%>
        <%--<hr />--%>
        <asp:GridView ID="GVArea" runat="server" CssClass="Grid" AutoGenerateColumns="false">
        <HeaderStyle CssClass="GridHeader" Wrap="false" ></HeaderStyle>
          <RowStyle CssClass="GridItem" Wrap = "false" /> <%--Gridview-row--%>
          <AlternatingRowStyle CssClass="GridAltItem"/>
          <PagerStyle CssClass="GridPager" HorizontalAlign="Left" />
          <Columns>
            <asp:BoundField HeaderStyle-Width="80px" DataField="AreaCode" HeaderText="Area Code" /><%--ItemStyle-CssClass="cssSupplier" --%>
            <asp:BoundField HeaderStyle-Width="150px" DataField="AreaName" HeaderText="Area Name" />
          </Columns>
        </asp:GridView>      
      </div> 
        
    <asp:Label runat ="server" ID="lbStatus" ForeColor="Red"/>
    <div class="loading" align="center">
      Loading. Please wait.<br />
      <br />
       <img src="../../Image/loader.gif" alt="" />
    </div>   
    
    </form>
</body>
</html>
