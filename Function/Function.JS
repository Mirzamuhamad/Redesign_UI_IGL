function addCommas(nStr) {
   //var TNstr = parseFloat(nStr);        
   //TNstr = TNstr.toFixed(digit);                
   //nStr = TNstr;        
   nStr += '';
   x = nStr.split('.');
   x1 = x[0];
   x2 = x.length > 1 ? '.' + x[1] : '';
   var rgx = /(\d+)(\d{3})/;
   while (rgx.test(x1)) {
      x1 = x1.replace(rgx, '$1' + ',' + '$2');
   }
   return x1 + x2;
}

function QtyxPrice(_prmQty, _prmPrice, _prmTotal) {
   var _tempQty = parseFloat(_prmQty.value.replace(/\$|\,/g, ""));
   if (isNaN(_tempQty) == true) {
      _tempQty = 0;
   }
   var _tempPrice = parseFloat(_prmPrice.value.replace(/\$|\,/g, ""));
   if (isNaN(_tempPrice) == true) {
      _tempPrice = 0;
   }
   var _tempTotal = _tempQty * _tempPrice;

   _prmQty.value = addCommas(_tempQty);
   _prmPrice.value = addCommas(_tempPrice);
   _prmTotal.value = addCommas(_tempTotal);
}

function kurang(_prmA, _prmB, _prmResult) {
   var _tempA = parseFloat(_prmA.value.replace(/\$|\,/g, ""));
   if (isNaN(_tempA) == true) {
      _tempA = 0;
   }
   var _tempB = parseFloat(_prmB.value.replace(/\$|\,/g, ""));
   if (isNaN(_tempB) == true) {
      _tempB = 0;
   }
   var _tempResult = _tempA - _tempB;

   _prmA.value = addCommas(_tempA);
   _prmB.value = addCommas(_tempB);
   _prmResult.value = addCommas(_tempResult);
}

function kali(_prmA, _prmB, _prmResult) {
   try {
      var _tempA = parseFloat(_prmA.value.replace(/\$|\,/g, ""));
      if (isNaN(_tempA) == true) {
         _tempA = 0;
      }
      var _tempB = parseFloat(_prmB.value.replace(/\$|\,/g, ""));
      if (isNaN(_tempB) == true) {
         _tempB = 0;
      }
      var _tempResult = _tempA * _tempB;

      _prmA.value = addCommas(_tempA);
      _prmB.value = addCommas(_tempB);
      _prmResult.value = addCommas(_tempResult);
   } catch (err) {
      alert(err.description);
   }
}

function tambah(_prmA, _prmB, _prmResult) {
   try {
      var _tempA = parseFloat(_prmA.value.replace(/\$|\,/g, ""));
      if (isNaN(_tempA) == true) {
         _tempA = 0;
      }
      var _tempB = parseFloat(_prmB.value.replace(/\$|\,/g, ""));
      if (isNaN(_tempB) == true) {
         _tempB = 0;
      }
      var _tempResult = _tempA + _tempB;

      _prmA.value = addCommas(_tempA);
      _prmB.value = addCommas(_tempB);
      _prmResult.value = addCommas(_tempResult);
   } catch (err) {
      alert(err.description);
   }
}

function kalippnrate(_prmAForex, _prmARate, _prmBForex, _prmBRate, _prmResult) {
   try {
      var _tempAForex = parseFloat(_prmAForex.value.replace(/\$|\,/g, ""));
      if (isNaN(_tempAForex) == true) {
         _tempAForex = 0;
      }
      var _tempARate = parseFloat(_prmARate.value.replace(/\$|\,/g, ""));
      if (isNaN(_tempARate) == true) {
         _tempARate = 0;
      }
      var _tempBForex = parseFloat(_prmBForex.value.replace(/\$|\,/g, ""));
      if (isNaN(_tempBForex) == true) {
         _tempBForex = 0;
      }
      var _tempBRate = parseFloat(_prmBRate.value.replace(/\$|\,/g, ""));
      if (isNaN(_tempBRate) == true) {
         _tempBRate = 0;
      }
      var _tempResult = (_tempAForex * _tempARate) + (_tempBForex * _tempBRate);

      _prmAForex.value = addCommas(_tempAForex);
      _prmARate.value = addCommas(_tempARate);
      _prmBForex.value = addCommas(_tempBForex);
      _prmBRate.value = addCommas(_tempBRate);
      _prmResult.value = addCommas(_tempResult);
   } catch (err) {
      alert(err.description);
   }
}

function RatexForex(_prmRate, _prmForex, _prmHome) {
   var _tempRate = parseFloat(_prmRate.value.replace(/\$|\,/g, ""));
   if (isNaN(_tempRate) == true) {
      _tempRate = 0;
   }
   var _tempForex = parseFloat(_prmForex.value.replace(/\$|\,/g, ""));
   if (isNaN(_tempForex) == true) {
      _tempForex = 0;
   }
   var _tempHome = _tempRate * _tempForex;
   _prmHome.value = addCommas(_tempHome);
}

function RatexDCForex(_prmRate, _prmDrForex, _prmCrForex, _prmDrHome, _prmCrHome) {
   var _tempRate = parseFloat(_prmRate.value.replace(/\$|\,/g, ""));
   if (isNaN(_tempRate) == true) {
      _tempRate = 0;
   }
   var _tempDrForex = parseFloat(_prmDrForex.value.replace(/\$|\,/g, ""));
   if (isNaN(_tempDrForex) == true) {
      _tempDrForex = 0;
   }
   var _tempCrForex = parseFloat(_prmCrForex.value.replace(/\$|\,/g, ""));
   if (isNaN(_tempCrForex) == true) {
      _tempCrForex = 0;
   }
   var _tempDrHome = _tempRate * _tempDrForex;
   var _tempCrHome = _tempRate * _tempCrForex;

   _prmRate.value = addCommas(_tempRate);
   _prmDrForex.value = addCommas(_tempDrForex);
   _prmCrForex.value = addCommas(_tempCrForex);
   _prmDrHome.value = addCommas(_tempDrHome);
   _prmCrHome.value = addCommas(_tempCrHome);
}

function BasePPnTotal(_prmBaseForex, _prmPPn, _prmPPnForex, _prmTotalForex) {
   try {
      var _tempBaseForex = parseFloat(_prmBaseForex.value.replace(/\$|\,/g, ""));
      if (isNaN(_tempBaseForex) == true) {
         _tempBaseForex = 0;
         _prmBaseForex.value = addCommas(_tempBaseForex);
      }
      var _tempPPn = parseFloat(_prmPPn.value.replace(/\$|\,/g, ""));
      if (isNaN(_tempPPn) == true) {
         _tempPPn = 0;
         _prmPPn.value = addCommas(_tempPPn);
      }
      //alert(_prmBaseForex.value.replace(/\$|\,/g,""));
      var _tempPPnForex = (_tempBaseForex * _tempPPn) / 100.00;
      var _tempTotalForex = _tempBaseForex + _tempPPnForex;
      _prmPPnForex.value = addCommas(_tempPPnForex);
      _prmTotalForex.value = addCommas(_tempTotalForex);
   } catch (err) {
      alert(err.description);
   }
}

function BaseDiscPPnPPhOtherTotal(_prmBaseForex, _prmDisc, _prmDiscForex, _prmPPn, _prmPPnForex, _prmPPhForex, _prmOtherForex, _prmTotalForex, _prmChange) {
   try {
      var _tempBaseForex = parseFloat(_prmBaseForex.value.replace(/\$|\,/g, ""));
      if (isNaN(_tempBaseForex) == true) {
         _tempBaseForex = 0;
         _prmBaseForex.value = addCommas(_tempBaseForex);
      }
      var _tempDiscForex = parseFloat(_prmDiscForex.value.replace(/\$|\,/g, ""));
      if (isNaN(_tempDiscForex) == true) {
         _tempDiscForex = 0;
         _prmDiscForex.value = addCommas(_tempDiscForex);
      }
      var _tempDisc = parseFloat(_prmDisc.value.replace(/\$|\,/g, ""));
      if (isNaN(_tempDisc) == true) {
         _tempDisc = 0;
         _prmDisc.value = addCommas(_tempDisc);
      }
      var _tempPPn = parseFloat(_prmPPn.value.replace(/\$|\,/g, ""));
      if (isNaN(_tempPPn) == true) {
         _tempPPn = 0;
         _prmPPn.value = addCommas(_tempPPn);
      }
      var _tempPPhForex = parseFloat(_prmPPhForex.value.replace(/\$|\,/g, ""));
      if (isNaN(_tempPPhForex) == true) {
         _tempPPhForex = 0;
         _prmPPhForex.value = addCommas(_tempPPhForex);
      }
      var _tempOtherForex = parseFloat(_prmOtherForex.value.replace(/\$|\,/g, ""));
      if (isNaN(_tempOtherForex) == true) {
         _tempOtherForex = 0;
         _prmOtherForex.value = addCommas(_tempOtherForex);
      }
      //alert(_prmBaseForex.value.replace(/\$|\,/g,""));        
      if (_prmChange == '%') {
         _tempDiscForex = (_tempBaseForex * _tempDisc) / 100.00;
      }
      _tempDisc = (_tempDiscForex * 100.00) / _tempBaseForex;
      if (isNaN(_tempDisc) == true) {
         _tempDisc = 0;
      }
      var _tempPPnForex = ((_tempBaseForex - _tempDiscForex) * _tempPPn) / 100.00;
      var _tempTotalForex = _tempBaseForex - _tempDiscForex + _tempPPnForex - _tempPPhForex + _tempOtherForex;

      _prmDisc.value = addCommas(_tempDisc);
      _prmDiscForex.value = addCommas(_tempDiscForex);
      _prmPPnForex.value = addCommas(_tempPPnForex);
      _prmPPhForex.value = addCommas(_tempPPhForex);
      _prmOtherForex.value = addCommas(_tempOtherForex);
      _prmTotalForex.value = addCommas(_tempTotalForex);
   } catch (err) {
      alert(err.description);
   }
}

function BaseDiscPPnTotal(_prmBaseForex, _prmDisc, _prmDiscForex, _prmPPn, _prmPPnForex, _prmTotalForex, _prmChange) {
   try {
      var _tempBaseForex = parseFloat(_prmBaseForex.value.replace(/\$|\,/g, ""));
      if (isNaN(_tempBaseForex) == true) {
         _tempBaseForex = 0;
         _prmBaseForex.value = addCommas(_tempBaseForex);
      }
      var _tempDiscForex = parseFloat(_prmDiscForex.value.replace(/\$|\,/g, ""));
      if (isNaN(_tempDiscForex) == true) {
         _tempDiscForex = 0;
         _prmDiscForex.value = addCommas(_tempDiscForex);
      }
      var _tempDisc = parseFloat(_prmDisc.value.replace(/\$|\,/g, ""));
      if (isNaN(_tempDisc) == true) {
         _tempDisc = 0;
         _prmDisc.value = addCommas(_tempDisc);
      }
      var _tempPPn = parseFloat(_prmPPn.value.replace(/\$|\,/g, ""));
      if (isNaN(_tempPPn) == true) {
         _tempPPn = 0;
         _prmPPn.value = addCommas(_tempPPn);
      }

      if (_prmChange == '%') {
         _tempDiscForex = (_tempBaseForex * _tempDisc) / 100.00;
      }
      _tempDisc = (_tempDiscForex * 100.00) / _tempBaseForex;
      if (isNaN(_tempDisc) == true) {
         _tempDisc = 0;
      }
      var _tempPPnForex = ((_tempBaseForex - _tempDiscForex) * _tempPPn) / 100.00;
      var _tempTotalForex = _tempBaseForex - _tempDiscForex + _tempPPnForex;


      _prmDisc.value = addCommas(_tempDisc);
      _prmDiscForex.value = addCommas(_tempDiscForex);
      _prmPPnForex.value = addCommas(_tempPPnForex);
      _prmTotalForex.value = addCommas(_tempTotalForex);
   } catch (err) {
      alert(err.description);
   }
}

function QtyPriceBrutoDiscNettoPPh(_prmQty, _prmPrice, _prmAmountForex, _prmDisc, _prmDiscForex, _prmNettoForex, _prmPPh, _prmPPhForex, _prmChange) {
   try {
      var _tempQty = parseFloat(_prmQty.value.replace(/\$|\,/g, ""));
      if (isNaN(_tempQty) == true) {
         _tempQty = 0;
         _prmQty.value = addCommas(_tempQty);
      }
      var _tempPrice = parseFloat(_prmPrice.value.replace(/\$|\,/g, ""));
      if (isNaN(_tempPrice) == true) {
         _tempPrice = 0;
         _prmPrice.value = addCommas(_tempPrice);
      }
      var _tempBrutoForex = _tempQty * _tempPrice;
      if (isNaN(_tempBrutoForex) == true) {
         _tempBrutoForex = 0;
         //_prmAmountForex.value = addCommas(_tempBrutoForex);
      }

      var _tempDiscForex = parseFloat(_prmDiscForex.value.replace(/\$|\,/g, ""));
      if (isNaN(_tempDiscForex) == true) {
         _tempDiscForex = 0;
         _prmDiscForex.value = addCommas(_tempDiscForex);
      }
      var _tempDisc = parseFloat(_prmDisc.value.replace(/\$|\,/g, ""));
      if (isNaN(_tempDisc) == true) {
         _tempDisc = 0;
         _prmDisc.value = addCommas(_tempDisc);
      }
      if (_prmChange == '%') {
         _tempDiscForex = (_tempBrutoForex * _tempDisc) / 100.00;
      }
      _tempDisc = _tempDiscForex * 100.00 / _tempBrutoForex;
      var _tempPPh = parseFloat(_prmPPh.value.replace(/\$|\,/g, ""));
      if (isNaN(_tempPPh) == true) {
         _tempPPh = 0;
         _prmPPh.value = addCommas(_tempPPh);
      }
      var _tempNettoForex = _tempBrutoForex - _tempDiscForex;
      var _tempPPhForex = (_tempNettoForex * _tempPPh) / 100.00;

      _prmAmountForex.value = addCommas(_tempBrutoForex);
      _prmNettoForex.value = addCommas(_tempNettoForex);
      _prmDisc.value = addCommas(_tempDisc);
      _prmDiscForex.value = addCommas(_tempDiscForex);
      _prmPPh.value = addCommas(_tempPPh);
      _prmPPhForex.value = addCommas(_tempPPhForex);

   } catch (err) {
      alert(err.description);
   }
}


function QtyPriceBrutoDiscNettoPPhGross(_prmQty, _prmPrice, _prmAmountForex, _prmDisc, _prmDiscForex, _prmNettoForex, _prmPPh, _prmPPhForex, _prmChange, _prmGrossUp) {
   try {
      var _tempQty = parseFloat(_prmQty.value.replace(/\$|\,/g, ""));
      if (isNaN(_tempQty) == true) {
         _tempQty = 0;
         _prmQty.value = addCommas(_tempQty);
      }
      var _tempPrice = parseFloat(_prmPrice.value.replace(/\$|\,/g, ""));
      if (isNaN(_tempPrice) == true) {
         _tempPrice = 0;
         _prmPrice.value = addCommas(_tempPrice);
      }
      var _tempBrutoForex = _tempQty * _tempPrice;
      if (isNaN(_tempBrutoForex) == true) {
         _tempBrutoForex = 0;
         //_prmAmountForex.value = addCommas(_tempBrutoForex);
      }

      var _tempDiscForex = parseFloat(_prmDiscForex.value.replace(/\$|\,/g, ""));
      if (isNaN(_tempDiscForex) == true) {
         _tempDiscForex = 0;
         _prmDiscForex.value = addCommas(_tempDiscForex);
      }
      var _tempDisc = parseFloat(_prmDisc.value.replace(/\$|\,/g, ""));
      if (isNaN(_tempDisc) == true) {
         _tempDisc = 0;
         _prmDisc.value = addCommas(_tempDisc);
      }
      if (_prmChange == '%') {
         _tempDiscForex = (_tempBrutoForex * _tempDisc) / 100.00;
      }
      _tempDisc = _tempDiscForex * 100.00 / _tempBrutoForex;
      var _tempPPh = parseFloat(_prmPPh.value.replace(/\$|\,/g, ""));
      if (isNaN(_tempPPh) == true) {
         _tempPPh = 0;
         _prmPPh.value = addCommas(_tempPPh);
      }
      var _tempNettoForex = _tempBrutoForex - _tempDiscForex;
      var _tempPPhForex = (_tempNettoForex * _tempPPh) / 100.00;
      var _tempGrossUp = _prmGrossUp.value;
      if (_tempGrossUp == 'Y') {
         _tempPPhForex = (_tempNettoForex / ((100.00 - _tempPPh) / 100.00)) - _tempNettoForex;
      }
      _prmAmountForex.value = addCommas(_tempBrutoForex);
      _prmNettoForex.value = addCommas(_tempNettoForex);
      _prmDisc.value = addCommas(_tempDisc);
      _prmDiscForex.value = addCommas(_tempDiscForex);
      _prmPPh.value = addCommas(_tempPPh);
      _prmPPhForex.value = addCommas(_tempPPhForex);

   } catch (err) {
      alert(err.description);
   }
}

//NB: Numeric type
function PressNumeric() {
   var _result = false;

   if (event.keyCode == 110 || event.keyCode == 8 || event.keyCode == 9 || event.keyCode == 37 || event.keyCode == 39 || event.keyCode == 190 || (event.keyCode >= 48 && event.keyCode <= 57) || (event.keyCode >= 96 && event.keyCode <= 105)) {
      _result = true;
   }
   else {
      _result = false;
   }

   return _result;
}

function PressFlag() {
   var _result = false;

   if (event.keyCode == 89 || event.keyCode == 78 || event.keyCode == 110 || event.keyCode == 9 || event.keyCode == 37 || event.keyCode == 39) {
      _result = true;
   }
   else {
      _result = false;
   }
   return _result;
}

function PressNumericMinus() {
   var _result = false;

   if (event.keyCode == 110 || event.keyCode == 8 || event.keyCode == 9 || event.keyCode == 37 || event.keyCode == 39 || event.keyCode == 190 || event.keyCode == 45 || event.keyCode == 109 || event.keyCode == 189 || (event.keyCode >= 48 && event.keyCode <= 57) || (event.keyCode >= 96 && event.keyCode <= 105)) {
      _result = true;
   }
   else {
      _result = false;
   }

   return _result;
}

function PressAlphaNumeric() {
   var _result = false;

   if (event.keyCode == 110 || event.keyCode == 8 || event.keyCode == 9 || event.keyCode == 37 || event.keyCode == 39 || event.keyCode == 190 || (event.keyCode >= 48 && event.keyCode <= 57) || (event.keyCode >= 65 && event.keyCode <= 90) || (event.keyCode >= 97 && event.keyCode <= 122)) {
      _result = true;
   }
   else {
      _result = false;
   }

   return _result;
}

function calculate() {
   var Rate = document.getElementById("tbRate");
   var Debit = document.getElementById("tbDebitForex");
   var Credit = document.getElementById("tbCreditForex");
   document.getElementById("tbDebitHome").value = Rate.value.replace(",", "") * Debit.value.replace(",", "");
   document.getElementById("tbCreditHome").value = Rate.value.replace(",", "") * Credit.value.replace(",", "");
   document.getElementById("tbDebitHome").value = addCommas(document.getElementById("tbDebitHome").value);
   document.getElementById("tbCreditHome").value = addCommas(document.getElementById("tbCreditHome").value);
   document.getElementById("tbRate").value = addCommas(document.getElementById("tbRate").value);
   document.getElementById("tbDebitForex").value = addCommas(document.getElementById("tbDebitForex").value);
   document.getElementById("tbCreditForex").value = addCommas(document.getElementById("tbCreditForex").value);
}

function currencystate(prmcurrency, prmrate) {
   var tempcurrency = prmcurrency.value;
   var temprate = parseFloat(prmrate.value.replace(",", ""));
   prmrate.disable = true;
   if (tempcurrency.value.toLowerCase() == "rp") {
      _tempRate = 1;
      prmRate.value = addCommas(tempRate);
      prmrate.disable = false;
   }
}

function GetCurrency(_prmCurrency, _prmDecimalPlace) {
   var _result;
   if (isNaN(_prmDecimalPlace) || _prmDecimalPlace == "") {
      _prmDecimalPlace = "8";
   }
   var _decimal = Math.pow(10, parseInt(_prmDecimalPlace));
   _prmCurrency = _prmCurrency.toString().replace(/\$|\,/g, '');
   _result = Math.round(_prmCurrency * _decimal) / _decimal
   return _result;
}

function Confirm() {
   var confirm_value = document.createElement("INPUT");
   confirm_value.type = "hidden";
   confirm_value.name = "confirm_value";

   var ddlCommand = document.getElementById("ddlCommand")

   if (ddlCommand.value == "Delete") {
      if (confirm("Are you sure you want to Delete?")) {
         confirm_value.value = "Yes";
      } else {
         confirm_value.value = "Tidak";
      }
      document.forms[0].appendChild(confirm_value);
   } else {
      confirm_value.value = "No";
      document.forms[0].appendChild(confirm_value);
   }
}

function Confirm2() {
   var confirm_value = document.createElement("INPUT");
   confirm_value.type = "hidden";
   confirm_value.name = "confirm_value";

   var ddlCommand2 = document.getElementById("ddlCommand2")

   if (ddlCommand2.value == "Delete") {
      if (confirm("Are you sure you want to Delete?")) {
         confirm_value.value = "Yes";
      } else {
         confirm_value.value = "Tidak";
      }
      document.forms[0].appendChild(confirm_value);
   } else {
      confirm_value.value = "No";
      document.forms[0].appendChild(confirm_value);
   }
}


// Script untuk loading form

$(document).ready(function () {

   // Jika form submit biasa
   $("form").on("submit", function () {
      console.log("Button");
      $(".loading-form").fadeIn(10);
   });

   // // Tangkap LinkButton manual
   // $(document).on("click", "a[href^='javascript:__doPostBack']", function () {
   //     console.log("LinkButton");
   //     $(".loading-form").fadeIn(10);
   // });
   // // Tangkap semua LinkButton manual
   $(document).on("click", "a[href^='javascript:__doPostBack'], a[href^='javascript:WebForm_DoPostBackWithOptions'], input[type='submit'], button[type='submit']", function () {
      console.log("ASP.NET Button Click Detected");
      $(".loading-form").fadeIn(10);
   });

   // Tangkap perubahan pada dropdown ASP.NET (misalnya autopostback)
$(document).on("change", "select", function () {
    // Deteksi hanya dropdown yang punya autopostback (optional)
    if (this.onchange && this.onchange.toString().includes("__doPostBack")) {
        console.log("ASP.NET Dropdown Change Detected");
        $(".loading-form").fadeIn(10);
    }
});

// Deteksi checkbox ASP.NET yang AutoPostBack
$(document).on("change", "input[type='checkbox']", function () {
    if (this.onchange && this.onchange.toString().includes("__doPostBack")) {
        console.log("ASP.NET Checkbox Change Detected");
        $(".loading-form").fadeIn(10);
    }
});

// ✅ Checkbox AutoPostBack (termasuk yang pakai setTimeout)
$(document).on("click", "input[type='checkbox']", function () {
    const hasPostback =
        (this.onclick && this.onclick.toString().includes("__doPostBack")) ||
        (this.onclick && this.onclick.toString().includes("setTimeout('__doPostBack"));
    if (hasPostback) {
        console.log("ASP.NET Checkbox AutoPostBack Detected");
        $(".loading-form").fadeIn(10);
    }
});

   // Tangkap semua postback ASP.NET (termasuk UpdatePanel)
   if (typeof (Sys) !== "undefined" && Sys.WebForms) {
      var prm = Sys.WebForms.PageRequestManager.getInstance();

      prm.add_beginRequest(function () {
         console.log("Button link");
         $(".loading-form").fadeIn(10);
      });

      prm.add_endRequest(function () {

         $(".loading-form").fadeOut(10);
      });
   }

   console.log("✅ Loading handler aktif dan LinkButton terdeteksi");
});
// ---------------------------------------------------------------