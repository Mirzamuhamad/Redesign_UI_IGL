<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TrSuppInv.aspx.vb" Inherits="SuppInv" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<%@ Register Assembly="BasicFrame.WebControls.BasicDatePicker" Namespace="BasicFrame.WebControls"
    TagPrefix="BDP" %>

<%@ Register assembly="CrystalDecisions.Web, Version=10.5.3700.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>

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
    
    function BaseDiscPPnPPhOtherTotalHd(_prmBaseForex, _prmDiscForex, _prmPPn, _prmPPnForex, _prmPPhForex, _prmOtherForex, _prmTotalForex, _prmChange)
    {
        try
        {        
        var _tempBaseForex = parseFloat(_prmBaseForex.value.replace(/\$|\,/g,""));
        if(isNaN(_tempBaseForex) == true)
        {
           _tempBaseForex = 0;
           _prmBaseForex.value = setdigit(_tempBaseForex,2);// addCommas(_tempBaseForex);
        }        
        var _tempDiscForex = parseFloat(_prmDiscForex.value.replace(/\$|\,/g,""));
        if(isNaN(_tempDiscForex) == true)
        {
           _tempDiscForex = 0;
           _prmDiscForex.value = setdigit(_tempDiscForex,2);//addCommas(_tempDiscForex); 
        }
//        var _tempDisc = parseFloat(_prmDisc.value.replace(/\$|\,/g,""));
//        if(isNaN(_tempDisc) == true)
//        {
//           _tempDisc = 0;
//           _prmDisc.value = addCommas(_tempDisc);
//        }        
        var _tempPPn = parseFloat(_prmPPn.value.replace(/\$|\,/g,""));
        if(isNaN(_tempPPn) == true)
        {
           _tempPPn = 0;
           _prmPPn.value = setdigit(_tempPPn,'<%=VIEWSTATE("DigitPercent")%>');//addCommas(_tempPPn);
        }
        var _tempPPhForex = parseFloat(_prmPPhForex.value.replace(/\$|\,/g,""));
        if(isNaN(_tempPPhForex) == true)
        {
           _tempPPhForex = 0;
           _prmPPhForex.value =  setdigit(_tempPPhForex,'<%=VIEWSTATE("DigitCurr")%>');//addCommas(_tempPPhForex);
        }        
        var _tempOtherForex = parseFloat(_prmOtherForex.value.replace(/\$|\,/g,""));
        if(isNaN(_tempOtherForex) == true)
        {
           _tempOtherForex = 0;
           _prmOtherForex.value = setdigit(_tempOtherForex,'<%=VIEWSTATE("DigitCurr")%>');//addCommas(_tempOtherForex);
        }        
        //alert(_prmBaseForex.value.replace(/\$|\,/g,""));        
        //if (_prmChange == '%')
        //{
        //    _tempDiscForex= (_tempBaseForex * _tempDisc) / 100.00;
        //}
        //_tempDisc = (_tempDiscForex * 100.00) / _tempBaseForex; 
        //if(isNaN(_tempDisc) == true)
        //{
        //   _tempDisc = 0;         
        //}
        var _tempPPnForex = ((_tempBaseForex - _tempDiscForex) * _tempPPn) / 100.00;
        var _tempTotalForex2 = _tempBaseForex - _tempDiscForex + _tempPPnForex + _tempOtherForex - _tempPPhForex;
        _prmBaseForex.value = setdigit(_tempBaseForex,2);//addCommas(_tempTotalForex2);             
        _prmTotalForex.value = setdigit(_tempTotalForex2,2);//addCommas(_tempTotalForex2);             
        _prmDiscForex.value = setdigit(_tempDiscForex,2); //addCommas(_tempDiscForex); 
        _prmPPnForex.value = setdigit(_tempPPnForex,2);//addCommas(_tempPPnForex);
        _prmPPhForex.value = setdigit(_tempPPhForex,2);//addCommas(_tempPPhForex);
        _prmOtherForex.value = setdigit(_tempOtherForex,2);//addCommas(_tempOtherForex);
        
       }catch (err){
            alert(err.description);
       }     
    }
        
        function setformat()
        {
        try
         {          

           
        var Rate = document.getElementById("tbRate").value.replace(/\$|\,/g,"");
        var PPn = document.getElementById("tbPPN").value.replace(/\$|\,/g,"");        
       var BaseForex = document.getElementById("tbBaseForex").value.replace(/\$|\,/g, "");        
        var DiscForex = document.getElementById("tbDiscForex").value.replace(/\$|\,/g,"");
        var PPnForex = document.getElementById("tbPPNForex").value.replace(/\$|\,/g,"");
        var PPhForex = document.getElementById("tbPPhForex").value.replace(/\$|\,/g,"");
        var OtherForex = document.getElementById("tbOtherForex").value.replace(/\$|\,/g,"");
        var TotalForex = document.getElementById("tbTotalForex").value.replace(/\$|\,/g,"");
        var DPForex = document.getElementById("tbDPForex").value.replace(/\$|\,/g,"");
        var fgInclude = document.getElementById("ddlFgPriceInclude").value.replace(/\$|\,/g,"");


        PPnForex = Math.floor((parseFloat(BaseForex) - parseFloat(DiscForex) - parseFloat(DPForex)) * parseFloat(PPn) / 100);
            TotalForex = (parseFloat(BaseForex) - parseFloat(DiscForex) - parseFloat(DPForex)) + parseFloat(PPnForex) - parseFloat(PPhForex) + parseFloat(OtherForex);

            if (PPnForex == TotalForex) {
                TotalForex = 0;
            } 
            
            if (PPnForex < 0) {
                PPnForex = 0;
            }

            
                
       document.getElementById("tbRate").value = setdigit(Rate,'<%=VIEWSTATE("DigitHome")%>');
        document.getElementById("tbRate").value = setdigit(Rate,'<%=VIEWSTATE("DigitHome")%>');
        document.getElementById("tbPPN").value = setdigit(PPn, '<%=VIEWSTATE("DigitHome")%>');
        document.getElementById("tbBaseForex").value = setdigit(BaseForex,'<%=VIEWSTATE("DigitHome")%>');        
        document.getElementById("tbDiscForex").value = setdigit(DiscForex,'<%=VIEWSTATE("DigitHome")%>');
        document.getElementById("tbPPNForex").value = setdigit(PPnForex,'<%=VIEWSTATE("DigitHome")%>');
        document.getElementById("tbPPhForex").value = setdigit(PPhForex,'<%=VIEWSTATE("DigitHome")%>');
        document.getElementById("tbOtherForex").value = setdigit(OtherForex,'<%=VIEWSTATE("DigitHome")%>');
        document.getElementById("tbTotalForex").value = setdigit(TotalForex,'<%=VIEWSTATE("DigitHome")%>');
        document.getElementById("tbDPForex").value = setdigit(DPForex,'<%=VIEWSTATE("DigitHome")%>');
        }catch (err){
            alert(err.description);
          }      
        }   
       
       function QtyPriceBrutoDiscNettoPPhGrossUp(_prmQty, _prmPrice, _prmAmountForex, _prmDisc, _prmDiscForex, _prmNettoForex, _prmPPh, _prmPPhForex, _prmChange)
        {
        try    
        {
        var _tempQty = parseFloat(_prmQty.value.replace(/\$|\,/g,""));
        if(isNaN(_tempQty) == true)
        {
           _tempQty = 0;
           _prmQty.value = addCommas(_tempQty);
        }
        var _tempPrice = parseFloat(_prmPrice.value.replace(/\$|\,/g,""));
        if(isNaN(_tempPrice) == true)
        {
           _tempPrice = 0;
           _prmPrice.value = addCommas(_tempPrice);
        }
        var _tempBrutoForex = _tempQty * _tempPrice;
        if(isNaN(_tempBrutoForex) == true)
        {
           _tempBrutoForex = 0;
           //_prmAmountForex.value = addCommas(_tempBrutoForex);
        }        
        
        var _tempDiscForex = parseFloat(_prmDiscForex.value.replace(/\$|\,/g,""));
        if(isNaN(_tempDiscForex) == true)
        {
           _tempDiscForex = 0;
           _prmDiscForex.value = addCommas(_tempDiscForex);
        }
        var _tempDisc = parseFloat(_prmDisc.value.replace(/\$|\,/g,""));
        if(isNaN(_tempDisc) == true)
        {
           _tempDisc = 0;
           _prmDisc.value = addCommas(_tempDisc);
        }
        if (_prmChange == '%')
        {
            _tempDiscForex= (_tempBrutoForex * _tempDisc) / 100.00;
        }
        _tempDisc = _tempDiscForex * 100.00 / _tempBrutoForex; 
        var _tempPPh = parseFloat(_prmPPh.value.replace(/\$|\,/g,""));
        if(isNaN(_tempPPh) == true)
        {
           _tempPPh = 0;
           _prmPPh.value = addCommas(_tempPPh);
        }        
        var _tempNettoForex = _tempBrutoForex - _tempDiscForex;         
        var _tempPPhForex = (_tempNettoForex * _tempPPh) / 100.00;
//        var _tempGrossUp = document.getElementById("ddlGrossUpPPh").value;        
//        if (_tempGrossUp == 'Y')        
//        {   _tempPPhForex = (_tempNettoForex/((100.00-_tempPPh)/100.00)) -_tempNettoForex;
//        }
        _prmAmountForex.value = setdigit(_tempBrutoForex,'<%=VIEWSTATE("DigitCurr")%>');//addCommas(_tempBrutoForex);
        _prmNettoForex.value = setdigit(_tempNettoForex,'<%=VIEWSTATE("DigitCurr")%>');//addCommas(_tempNettoForex);
        _prmDisc.value = setdigit(_tempDisc,'<%=VIEWSTATE("DigitPercent")%>');//addCommas(_tempDisc);
        _prmDiscForex.value = setdigit(_tempDiscForex,'<%=VIEWSTATE("DigitCurr")%>');//addCommas(_tempDiscForex); 
        _prmPPh.value = setdigit(_tempPPh,'<%=VIEWSTATE("DigitPercent")%>');//addCommas(_tempPPh);
        _prmPPhForex.value = setdigit(_tempPPhForex,'<%=VIEWSTATE("DigitCurr")%>');//addCommas(_tempPPhForex);
        
        }catch (err){
            alert(err.description);
          }      
       } 
                       
       function setformatdt()
        {
        try
         {                 
        var Qty = document.getElementById("tbQty").value.replace(/\$|\,/g,""); 
        var QtyWrhs = document.getElementById("tbQtyWrhs").value.replace(/\$|\,/g,""); 
        var PriceForex = document.getElementById("tbPrice").value.replace(/\$|\,/g,"");
        var BrutoForex = document.getElementById("tbBrutoForex").value.replace(/\$|\,/g,"");        
        var DiscDt = document.getElementById("tbDiscDt").value.replace(/\$|\,/g,"");        
        var DiscDtForex = document.getElementById("tbDiscDtForex").value.replace(/\$|\,/g,"");        
        var NettoForex = document.getElementById("tbNettoForex").value.replace(/\$|\,/g,"");        
        var PPh = document.getElementById("tbPPhDt").value.replace(/\$|\,/g,"");        
        var PPhForex = document.getElementById("tbPPhDtForex").value.replace(/\$|\,/g,"");        
        document.getElementById("tbQty").value = setdigit(Qty,'<%=VIEWSTATE("DigitQty")%>');
        document.getElementById("tbQtyWrhs").value = setdigit(QtyWrhs,'<%=VIEWSTATE("DigitQty")%>');
        document.getElementById("tbPrice").value = setdigit(PriceForex,'<%=VIEWSTATE("DigitCurr")%>');
        document.getElementById("tbBrutoForex").value = setdigit(BrutoForex,'<%=VIEWSTATE("DigitCurr")%>');
        document.getElementById("tbDiscDt").value = setdigit(DiscDt,'<%=VIEWSTATE("DigitPercent")%>');
        document.getElementById("tbDiscDtForex").value = setdigit(DiscDtForex,'<%=VIEWSTATE("DigitCurr")%>');
        document.getElementById("tbNettoForex").value = setdigit(NettoForex,'<%=VIEWSTATE("DigitCurr")%>');
        document.getElementById("tbPPhDt").value = setdigit(PPh,'<%=VIEWSTATE("DigitPercent")%>');
        document.getElementById("tbPPhDtForex").value = setdigit(PPhForex,'<%=VIEWSTATE("DigitCurr")%>');
        }catch (err){
            alert(err.description);
          }      
        }   
        
        function setformatdt2()
        {
        try
        {       
        var DPBaseForex = document.getElementById("tbDPBaseForex").value.replace(/\$|\,/g,""); 
        var DPPPnForex = document.getElementById("tbDPPPnForex").value.replace(/\$|\,/g,""); 
        //var DPTotalForex = document.getElementById("tbDPTotalForex").value.replace(/\$|\,/g,"");
        var DPTotalForex = parseFloat(DPBaseForex) + parseFloat(DPPPnForex);         
        document.getElementById("tbDPBaseForex").value = setdigit(DPBaseForex,'<%=VIEWSTATE("DigitCurrDt2")%>');
        document.getElementById("tbDPPPnForex").value = setdigit(DPPPnForex,'<%=VIEWSTATE("DigitCurrDt2")%>');
        document.getElementById("tbDPTotalForex").value = setdigit(DPTotalForex,'<%=VIEWSTATE("DigitCurrDt2")%>');                
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
                      <asp:ListItem Value="TransNmbr" Selected="True">Reference</asp:ListItem>
                      <asp:ListItem>Status</asp:ListItem>
                      <asp:ListItem Value="dbo.FormatDate(TransDate)">TT Date</asp:ListItem>
                      <asp:ListItem Value="dbo.FormatDate(ContraBonDate)">Contra Bon Date</asp:ListItem>
                      <asp:ListItem Value="ContraBonNo">Contra Bon No</asp:ListItem>
                      <asp:ListItem Value="Supplier">Supplier</asp:ListItem>
                      <asp:ListItem Value="Attn">Attn</asp:ListItem>
                      <asp:ListItem Value="PONo">PO No</asp:ListItem>                      
                      <asp:ListItem Value="SuppInvNo">Supplier Invoice</asp:ListItem>                      
                      <asp:ListItem Value="PPnNo">PPn No</asp:ListItem>                      
                      <asp:ListItem Value="RR_No">RR No</asp:ListItem>
                      <asp:ListItem Value="SJ_Supp_No">SJ Supplier No</asp:ListItem>                      
                      <asp:ListItem Value="dbo.FormatDate(DueDate)">Due Date</asp:ListItem>
                      <asp:ListItem Value="Term_Name">Term</asp:ListItem>
                      <asp:ListItem Value="Currency">Currency</asp:ListItem>
                      <asp:ListItem Value="PriceIncludePPN">Price Include PPN</asp:ListItem>                      
                      <asp:ListItem>Remark</asp:ListItem>
                  </asp:DropDownList>
                  <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlRange"></asp:DropDownList>
                
                  <asp:Button class="bitbtn btnsearch" runat="server" ID="btnSearch" Text="Search" />																						 											  
                  <asp:Button class="bitbtn btngo" runat="server" ID="btnExpand" Text="..." />																						 											  
            </td>
            <td>
                <asp:LinkButton ID="LbAdvSearch" runat="server" Text="Advanced Search" />
                &nbsp &nbsp &nbsp &nbsp
                <asp:Label runat="server" ID="Label1" Text="Outstanding PO : "/>
                <asp:LinkButton runat="server" ID="lbCount" Text="X" ForeColor="#FF6600" Font-Size="Small" />
                <asp:Label runat="server" ID="Label2" Text=" record(s)"/>
            </td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                Show Records</td>
            <td>
                :</td>
            <td>
                <asp:DropDownList ID="ddlRow" runat="server" AutoPostBack="True" 
                    CssClass="DropDownList">
                    <asp:ListItem>20</asp:ListItem>
                    <asp:ListItem>30</asp:ListItem>
                    <asp:ListItem>40</asp:ListItem>
                    <asp:ListItem>50</asp:ListItem>
                    <asp:ListItem>100</asp:ListItem>
                    <asp:ListItem>200</asp:ListItem>
                    <asp:ListItem>300</asp:ListItem>
                </asp:DropDownList></td>
            <td>
                Rows</td>
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
                  <asp:ListItem Value="TransNmbr" Selected="True">Reference</asp:ListItem>
                      <asp:ListItem>Status</asp:ListItem>
                      <asp:ListItem Value="dbo.FormatDate(TransDate)">TT Date</asp:ListItem>
                      <asp:ListItem Value="Supplier">Supplier</asp:ListItem>
                      <asp:ListItem Value="Attn">Attn</asp:ListItem>
                      <asp:ListItem Value="PONo">PO No</asp:ListItem>
                      <asp:ListItem Value="SuppInvNo">Supplier Invoice</asp:ListItem>   
                      <asp:ListItem Value="PPnNo">PPn No</asp:ListItem>                                         
                      <asp:ListItem Value="RR_No">RR No</asp:ListItem>
                      <asp:ListItem Value="SJ_Supp_No">SJ Supplier No</asp:ListItem>                      
                      <asp:ListItem Value="dbo.FormatDate(DueDate)">Due Date</asp:ListItem>
                      <asp:ListItem Value="Term_Name">Term</asp:ListItem>
                      <asp:ListItem Value="Currency">Currency</asp:ListItem>
                      <asp:ListItem Value="PriceIncludePPN">Price Include PPN</asp:ListItem>                      
                      <asp:ListItem>Remark</asp:ListItem>
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
          <br />&nbsp; 
          <div style="border:0px  solid; width:100%; height:100%; overflow:auto;">
          <asp:GridView ID="GridView1" runat="server" AllowPaging="True" AllowSorting="true"
            CssClass="Grid" AutoGenerateColumns="false"> 
              <HeaderStyle CssClass="GridHeader"></HeaderStyle>
						<RowStyle CssClass="GridItem" Wrap="false" />
						<AlternatingRowStyle CssClass="GridAltItem"/>
						<PagerStyle CssClass="GridPager" />
              <Columns>              
                  <asp:TemplateField HeaderStyle-Width="110px" HeaderText="Action">
                      <HeaderTemplate>
                          <asp:CheckBox ID="cbSelectHd" runat="server" AutoPostBack="true"
                          oncheckedchanged="cbSelectHd_CheckedChanged" />
                      </HeaderTemplate>
                      <ItemTemplate>
                          <asp:CheckBox ID="cbSelect" runat="server" />
                      </ItemTemplate>
                  </asp:TemplateField>
                  <asp:TemplateField HeaderStyle-Width="110px" HeaderText="Action">
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
                  <asp:BoundField DataField="Status" HeaderText="Status" ItemStyle-HorizontalAlign="Center"></asp:BoundField>                  
                  <asp:BoundField DataField="TransDate" dataformatstring="{0:dd MMM yyyy}" htmlencode="true" HeaderStyle-Width="80px" SortExpression="TransDate" HeaderText="Date" ItemStyle-HorizontalAlign="Right"></asp:BoundField>
                  <asp:BoundField DataField="Supplier" HeaderStyle-Width="200px" SortExpression="Supplier" HeaderText="Supplier"></asp:BoundField>                  
                  <asp:BoundField DataField="Supplier" HeaderStyle-Width="200px" SortExpression="Supplier" HeaderText="Supplier"></asp:BoundField>
                  <asp:BoundField DataField="PONo" HeaderStyle-Width="120px" SortExpression="PONo" HeaderText="PO No"></asp:BoundField>
                  <asp:BoundField DataField="Term_Name" HeaderStyle-Width="150px" SortExpression="Term_Name" HeaderText="Term"></asp:BoundField>
                  <asp:BoundField DataField="SuppInvNo" HeaderStyle-Width="120px" SortExpression="SuppInvNo" HeaderText="Supp Invoice"></asp:BoundField>
                  <asp:BoundField DataField="Currency" HeaderStyle-Width="50px" SortExpression="Currency" HeaderText="Currency" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                  <asp:BoundField DataField="PriceIncludePPN" HeaderStyle-Width="50px" SortExpression="PriceIncludePPN" HeaderText="Price Include" ItemStyle-HorizontalAlign="Center"></asp:BoundField>                  
                  <asp:BoundField DataField="BaseForex" DataFormatString="{0:#,##0.00}" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Right" SortExpression="BaseForex" HeaderText="Base Forex"></asp:BoundField>
                  <asp:BoundField DataField="DiscForex" DataFormatString="{0:#,##0.00}" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Right" SortExpression="DiscForex" HeaderText="Disc Forex"></asp:BoundField>
                  <asp:BoundField DataField="DPForex" DataFormatString="{0:#,##0.00}" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Right" SortExpression="DPForex" HeaderText="DP Forex"></asp:BoundField>                  
                  <asp:BoundField DataField="PPnForex" DataFormatString="{0:#,##0.00}" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Right" SortExpression="PPnForex" HeaderText="PPn Forex"></asp:BoundField>
                  <asp:BoundField DataField="PPhForex" DataFormatString="{0:#,##0.00}" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Right" SortExpression="PPhForex" HeaderText="PPh Forex"></asp:BoundField>
                  <asp:BoundField DataField="PPnHome" DataFormatString="{0:#,##0.00}" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Right" SortExpression="PPnHome" HeaderText="PPn Home"></asp:BoundField>
                  <asp:BoundField DataField="OtherForex" DataFormatString="{0:#,##0.00}" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Right" SortExpression="OtherForex" HeaderText="Other Forex"></asp:BoundField>
                  <asp:BoundField DataField="TotalForex" DataFormatString="{0:#,##0.00}" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Right" SortExpression="TotalForex" HeaderText="Total Forex"></asp:BoundField>                  
                  <asp:BoundField DataField="Remark" HeaderStyle-Width="250px" HeaderText="Remark"></asp:BoundField>
              </Columns>
          </asp:GridView>
          </div>
          <br />
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
            <td>Inv. No</td>
            <td>:</td>
            <td><asp:TextBox CssClass="TextBoxR" runat="server" ID="tbRef" Width="150px" Enabled="False"/>             
                &nbsp &nbsp Report : &nbsp
                <asp:DropDownList Enabled="false"  CssClass="DropDownList" ID="ddlReport" runat="server" >
                        <asp:ListItem Selected="True">Y</asp:ListItem>
                        <asp:ListItem>N</asp:ListItem>
                    </asp:DropDownList> 
            </td>                    
            <td>Inv. Date</td>
            <td>:</td>
            <td>
            <BDP:BasicDatePicker ID="tbDate" runat="server" DateFormat="dd MMM yyyy" 
                        ReadOnly = "true" ValidationGroup="Input"
                        ButtonImageHeight="19px" ButtonImageWidth="20px" 
                        DisplayType="TextBoxAndImage" 
                        TextBoxStyle-CssClass="TextDate" AutoPostBack="True" 
                        ShowNoneButton="False"><TextBoxStyle CssClass="TextDate" /></BDP:BasicDatePicker>                
            </td>  
                 
        </tr> 
          
        <tr>
            <td><asp:LinkButton ID="lbSupp" ValidationGroup="Input"  runat="server" Text="Supplier"/></td>
            <td>:</td>
            <td colspan="4"><asp:TextBox CssClass="TextBox" ValidationGroup="Input" runat="server" 
                    ID="tbSuppCode" AutoPostBack="true" Width="116px" />
                <asp:TextBox CssClass="TextBox" runat="server" ID="tbSuppName" Enabled="false" Width="225px"/>
                <asp:Button Class="btngo" ID="btnSupp" Text="..." runat="server" ValidationGroup="Input" />
            </td>
        </tr>
        <tr>
            <td>Attn</td>
            <td>:</td>
            <td><asp:TextBox runat="server" ValidationGroup="Input" ID="tbAttn" Maxlength = "60" CssClass="TextBox" Width="225px"/>
            </td>
            <td>Supp Invoice No </td>
            <td>:</td>
            <td><asp:TextBox runat="server" ValidationGroup="Input" ID="tbSuppInvNo" MaxLength = "30" CssClass="TextBox" Width="150px"/>            
            </td>                  
        </tr>
        <tr>
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
            <td>Price Include PPN</td>
            <td>:</td>
            <td><asp:DropDownList Enabled = "False" ValidationGroup="Input" CssClass="DropDownList" ID="ddlFgPriceInclude" runat="server" >
                        <asp:ListItem>Y</asp:ListItem>
                        <asp:ListItem>N</asp:ListItem>
                </asp:DropDownList>                                                                                
            </td>            
        </tr>  
        <tr>
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
                 <asp:TextBox runat="server" ValidationGroup="Input" ID="tbPPnNo" AutoPostBack="true" CssClass="TextBox" Width="150px"/>
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
            <td>
                
            </td>        
        </tr>              

        <tr>
                <td><asp:LinkButton ID="lbCurr" ValidationGroup="Input" runat="server" Text="Currency"/></td>
                <td>:</td>
                <td><asp:DropDownList ID="ddlCurr" ValidationGroup="Input" AutoPostBack="true" runat="server" CssClass="DropDownList" />                                                                   
                &nbsp Rate : &nbsp <asp:TextBox ID="tbRate" ValidationGroup="Input" runat="server" CssClass="TextBox" Width="68px" />
                </td>
                <td>PO No</td>
                <td>:</td>
                <td><asp:TextBox runat="server" ID="tbPONo" CssClass="TextBox" Width="150px" Enabled ="false"/> 
                <asp:Button Class="bitbtndt btnsearch" ID="btnGetData" Text="Get Data" runat="server" ValidationGroup="Input" />
                </td>
        </tr>
        
         
        <tr>
                <td>Total</td>
                <td>:</td>
                <td colspan="4">
                    <table>
                        <tr style="background-color:Silver;text-align:center">
                            <td>Base Forex</td>                            
                            <td>Disc Forex</td>
                            <td>DP Base</td>
                            <td>PPN %</td>
                            <td>PPN Forex</td>
                            <td>PPh Forex</td>
                            <td>Other Forex</td>                            
                            <td>Total Forex</td>
                            
                        </tr>
                        <tr>
                            <td><asp:TextBox ID="tbBaseForex" runat="server" CssClass="TextBoxR" Width="90px"/></td>                            
                            <td><asp:TextBox ID="tbDiscForex" runat="server" CssClass="TextBoxR" Width="90px"/></td>
                            <td><asp:TextBox ID="tbDPForex" runat="server" CssClass="TextBoxR" Width="90px"/></td>
                            <%--<td><asp:TextBox ID="tbPPN" runat="server" AutoPostBack="true" CssClass="TextBox" width="40px"/></td>--%>
                            <td><asp:TextBox ID="tbPPN" runat="server" CssClass="TextBoxR" width="40px"/></td>
                            <td><asp:TextBox ID="tbPPNForex" runat="server" CssClass="TextBoxR" Width="90px"/></td>
                            <td><asp:TextBox ID="tbPPhForex" runat="server" CssClass="TextBoxR" Width="90px"/></td>
                            <td><asp:TextBox ID="tbOtherForex" ValidationGroup="Input" runat="server" CssClass="TextBox" Width="90px"/></td>                            
                            <td><asp:TextBox ID="tbTotalForex" runat="server" CssClass="TextBoxR" Width="90px"/></td>
                            
                        </tr>
                    </table>
                </td>                 
             </tr>   
        <tr>
            <td>Remark</td>
            <td>:</td>
            <td colspan="4"><asp:TextBox runat="server" ValidationGroup="Input" ID="tbRemark" MaxLength = "255" CssClass="TextBoxMulti" Width="350px" TextMode="MultiLine" /></td>
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
                <asp:MenuItem Text="Detail" Value="0"></asp:MenuItem>                   
                <asp:MenuItem Text="Down Payment" Value="1"></asp:MenuItem>                   
            </Items>            
        </asp:Menu>
      <asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex="0">
           <asp:View ID="Tab1" runat="server">  
      <%--<div style="font-size:medium; color:Blue;">Detail</div>
      <hr style="color:Blue" />  --%>
        <asp:Panel runat="server" ID="pnlDt">
            <asp:Button class="bitbtn btnadd" runat="server" ID="btnAddDt" Text="Add" 
                Visible="False" />	
            <div style="border:0px  solid; width:100%; height:100%; overflow:auto;">
                <asp:GridView ID="GridDt" runat="server" AutoGenerateColumns="false" 
                    ShowFooter="False">
                    <HeaderStyle CssClass="GridHeader" />
                    <RowStyle CssClass="GridItem" Wrap="false" />
                    <AlternatingRowStyle CssClass="GridAltItem" />
                    <PagerStyle CssClass="GridPager" />
                    <Columns>
                        <asp:TemplateField HeaderText="Action">
                       <ItemTemplate>
                       <asp:Button ID="btnEdit" runat="server" class="bitbtndt btnedit" 
                                    CommandName="Edit" Text="Edit" />
                                 
								<asp:Button ID="btnDelete" runat="server" class="bitbtndt btndelete" 
                                    CommandName="Delete" 
                                    OnClientClick="return confirm('Sure to delete this data?');" Text="Delete" />
                                </ItemTemplate>
                            <EditItemTemplate>
                              	<asp:Button ID="btnUpdate" runat="server" class="bitbtndt btnsave" 
                                    CommandName="Update" Text="Save" />
                                <asp:Button ID="btnCancel" runat="server" class="bitbtndt btncancel" 
                                    CommandName="Cancel" Text="Cancel" />
                                 </EditItemTemplate>
                            </asp:TemplateField>
                        <asp:BoundField DataField="ReffType" HeaderStyle-Width="50px" HeaderText="RR Type" />
                        <asp:BoundField DataField="ReffNmbr" HeaderStyle-Width="120px" HeaderText="RR No" />
                        <asp:BoundField DataField="Product" HeaderStyle-Width="80px" HeaderText="Product Code" />
                        <asp:BoundField DataField="Product_Name" HeaderStyle-Width="250px"  HeaderText="Product Name" />
                        <asp:BoundField DataField="CostCtr" HeaderStyle-Width="50px"  HeaderText="Cost Ctr" />
                        <asp:BoundField DataField="QtyOrder" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Right" HeaderText="Qty" />
                        <asp:BoundField DataField="UnitOrder" HeaderStyle-Width="80px" HeaderText="Unit" />                        
                        <asp:BoundField DataField="PriceForex" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Right" HeaderText="Price" />
                        <asp:BoundField DataField="BrutoForex" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Right" HeaderText="Bruto Forex" />
                        <asp:BoundField DataField="DiscForex" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Right" HeaderText="Disc Forex" />
                        <asp:BoundField DataField="NettoForex" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Right" HeaderText="Netto Forex" />
                        <asp:BoundField DataField="PPhForex" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Right" HeaderText="PPh Forex" />
                    </Columns>
                </asp:GridView>
          </div>   
          <asp:Button class="bitbtn btnadd" runat="server" ID="btnAddDt2" Text="Add" 
                Visible="False" />	       </asp:Panel>       
       <asp:Panel runat="server" ID="pnlEditDt" Visible="false">
            <table>                
                <tr>
                    <td>RR No & Cost Center</td>
                    <td>:</td>
                    <td><asp:TextBox CssClass="TextBox" runat="server" ID="tbRRType" Enabled="false" 
                            Width="61px" />
                        <asp:TextBox CssClass="TextBox" runat="server" ID="tbRRNo" Enabled="false" />
                        <asp:TextBox CssClass="TextBox" runat="server" ID="tbCostCtr" Enabled="false" />
                    </td>
                </tr>                
                <tr>
                    <td>Product</td>
                    <td>:</td>
                    <td><asp:TextBox runat="server" ID="tbProductCode" CssClass="TextBox" Enabled="false" />
                        <asp:TextBox runat="server"  CssClass="TextBox"
                            ID="tbProductName" Enabled="false" Width="300px"/>                         
                    </td>                               
                </tr>                                                    
                <tr>
                    <td>Qty Order</td>
                    <td>:</td>
                    <td><asp:TextBox CssClass="TextBox" runat="server" ID="tbQty" Enabled="false" Width="50px"/>
                        <asp:TeXtBox ID="tbUnit" runat="server" CssClass="TextBox" Enabled="false" Width="50px"/>
                    </td>
                </tr>                
                <tr>
                    <td>Qty Warehouse</td>
                    <td>:</td>
                    <td><asp:TextBox CssClass="TextBox" runat="server" ID="tbQtyWrhs" Enabled="false" Width="50px" />
                        <asp:TextBox CssClass="TextBox" runat="server" ID="tbUnitWrhs" Enabled="false" Width="50px" />
                    </td>
                </tr>
                <tr>
                    <td>Price </td>
                    <td>:</td>
                    <td><asp:TextBox runat="server" CssClass="TextBox" ID="tbPrice" />
                    <%--//Wayan minta bukain biar bisa input data January (20150422)--%>
                    <%--<asp:TextBox runat="server" CssClass="TextBoxR" ID="tbPrice" />--%>
                    </td>
                </tr>              
                <tr>
                    <td>Amount</td>
                    <td>:</td>
                    <td>
                    <table>
                    <tr style="background-color:Silver;text-align:center">
                        <td>Bruto Forex</td>
                        <td>Disc %</td>
                        <td>Disc Forex</td>
                        <td>Netto Forex</td>
                        <td>PPh %</td>
                        <td>PPh Forex</td>
                    </tr>
                    <tr>
                    <td>
                    <asp:TextBox runat="server" ID="tbBrutoForex" CssClass="TextBoxR" Width="80px"/>
                    </td>
                    <td>
                    <asp:TextBox runat="server" ID="tbDiscDt" CssClass="TextBoxR" Width="40px"/>                            
                    </td>
                    <td>
                    <asp:TextBox runat="server" ID="tbDiscDtForex" CssClass="TextBoxR" Width="80px"/>                            
                    </td>
                    <td>
                    <asp:TextBox runat="server" ID="tbNettoForex" CssClass="TextBoxR" Width="80px"/>                            
                    </td>
                    <td>
                    <asp:TextBox runat="server" ID="tbPPhDt" CssClass="TextBox" Width="40px"/>                            
                    </td>
                    <td>
                    <asp:TextBox runat="server" ID="tbPPhDtForex" CssClass="TextBoxR" Width="80px"/>                            
                    </td>
                 </tr>
                 </table>
            </td>                         
                </tr>  
                <tr>
                    <td>Remark </td>
                    <td>:</td>
                    <td><asp:TextBox runat="server" CssClass="TextBox" ID="tbRemarkDt" MaxLength="255" Width="350px" TextMode="MultiLine" /></td>
                </tr>                              
            </table>
            <br />                     
    		<asp:Button ID="btnSaveDt" runat="server" class="bitbtndt btnsave" Text="Save" />									
            <asp:Button ID="btnCancelDt" runat="server" class="bitbtndt btncancel" Text="Cancel" />									
       </asp:Panel> 
       </asp:View>           
       <asp:View ID="Tab2" runat="server">  
            <asp:Panel ID="pnlDt2" runat="server">      
                <asp:Button class="bitbtn btnadd" runat="server" ID="btnAddDP2" Text="Add" ValidationGroup="Input" />	          
                
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
                                    
                                       			<asp:Button ID="btnEdit" runat="server" class="bitbtndt btnedit" 
                                        CommandName="Edit" Text="Edit" />
                                    <asp:Button ID="btnDelete" runat="server" class="bitbtndt btndelete" 
                                        CommandName="Delete" 
                                        OnClientClick="return confirm('Sure to delete this data?');" Text="Delete" />
                                  </ItemTemplate>
                                <EditItemTemplate>
                                 <asp:Button ID="btnUpdate" runat="server" class="bitbtndt btnsave" 
                                        CommandName="Update" Text="Save" />
                                    <asp:Button ID="btnCancel" runat="server" class="bitbtndt btncancel" 
                                        CommandName="Cancel" Text="Cancel" />
                                  </EditItemTemplate>
                                </asp:TemplateField>                                        
                            <asp:BoundField DataField="DPNo" HeaderStyle-Width="120px" HeaderText="DP No" />
                            <asp:BoundField DataField="Currency" HeaderStyle-Width="50px" HeaderText="Currency" />
                            <asp:BoundField DataField="ForexRate" HeaderStyle-Width="50px" HeaderText="Rate" />                            
                            <asp:BoundField DataField="BaseForex" HeaderStyle-Width="90px" HeaderText="Base Forex" />
                            <asp:BoundField DataField="PPNForex" HeaderStyle-Width="90px" HeaderText="PPN Forex" />
                            <asp:BoundField DataField="TotalForex" HeaderStyle-Width="90px" HeaderText="Total Forex" />                            
                            <asp:BoundField DataField="Remark" HeaderStyle-Width="300px" HeaderText="Remark" />                                                        
                        </Columns>
                    </asp:GridView>
              </div>    
              
              <asp:Button class="bitbtn btnadd" runat="server" ID="btnAddDPKe2" Text="Add" ValidationGroup="Input" />	          
              
              </asp:Panel>
            <asp:Panel runat="server" ID="pnlEditDt2" Visible="false">
                <table>              
                    <tr>
                        <td>DP No</td>
                        <td>:</td>
                        <td><asp:TextBox CssClass="TextBox" runat="server" ID="tbDPNo" Width="157px" Enabled="false"/> 
                        <asp:Button Class="btngo" ID="btnDPNo" Text="..." runat="server" />                                  
                        </td>                        
                    </tr>                    
                    <tr> 
                        <td>Currency</td>
                        <td>:</td>
                        <td>
                            <table>
                                <tr style="background-color:Silver;text-align:center">
                                    <td>Currency</td>
                                    <td>Rate</td>
                                    <td>PPn Rate</td>    
                                    <td>PPn %</td>                                                                    
                                </tr>
                                <tr>
                                    <td><asp:TextBox CssClass="TextBox" ID="tbDPCurrency" runat="server" Enabled="false" Width="60px"/></td>
                                    <td><asp:TextBox CssClass="TextBox" runat="server" ID="tbDPRate" Enabled="false" Width="65px"/></td>
                                    <td><asp:TextBox CssClass="TextBox" runat="server" ID="tbDPPPnRate" Enabled="false" Width="65px"/></td>                                    
                                    <td><asp:TextBox CssClass="TextBox" runat="server" ID="tbDPPPnPercent" Enabled="false" Width="50px"/></td>                                                                        
                                </tr>
                            </table>
                        </td>                                                
                    </tr>   
                    <tr>
                        <td>Nominal</td>
                        <td>:</td>
                        <td>
                            <table>
                                <tr style="background-color:Silver;text-align:center">
                                    <td style="width:95px;">Base Forex</td>                                    
                                    <td style="width:95px;">PPn Forex</td>                                                                  
                                    <td style="width:95px;">Total Forex</td>                                    
                                </tr>
                             </table>
                        </td>
                     </tr>        
                     <tr>
                        <td>DP</td>
                        <td>:</td>
                        <td>
                            <table>
                                <tr>
                                    <td style="width:90px;"><asp:TextBox CssClass="TextBox" runat="server" ID="tbDPBase" Width="90px" Enabled="false" /></td>
                                    <td style="width:90px;"><asp:TextBox CssClass="TextBox" runat="server" ID="tbDPPPn" Width="90px" Enabled="false"/></td>                                                                                                        
                                    <td style="width:90px;"><asp:TextBox CssClass="TextBox" runat="server" ID="tbDPTotal" Width="90px" Enabled="false"/></td>
                                </tr>
                             </table>
                        </td>
                     </tr>        
                     <tr>
                        <td>Paid</td>
                        <td>:</td>
                        <td>
                            <table>
                                <tr>
                                    <td style="width:90px;"><asp:TextBox CssClass="TextBox" runat="server" ID="tbPaidBase" Width="90px" Enabled="false"/></td>
                                    <td style="width:90px;"><asp:TextBox CssClass="TextBox" runat="server" ID="tbPaidPPN" Width="90px" Enabled="false"/></td>                                                                                                        
                                    <td style="width:90px;"><asp:TextBox CssClass="TextBox" runat="server" ID="tbPaidTotal" Width="90px" Enabled="false"/></td>
                                </tr>
                             </table>
                        </td>
                     </tr>        
                     <tr>
                        <td>To Be Paid</td>
                        <td>:</td>
                        <td>
                            <table>
                                <tr>
                                    <td style="width:90px;"><asp:TextBox CssClass="TextBox" runat="server" ID="tbDPBaseForex" Width="90px"/></td>
                                    <td style="width:90px;"><asp:TextBox CssClass="TextBox" runat="server" ID="tbDPPPnForex" Width="90px"/></td>                                                                                                        
                                    <td style="width:90px;"><asp:TextBox CssClass="TextBoxR" runat="server" ID="tbDPTotalForex" Width="90px"/></td>
                                </tr>
                             </table>
                        </td>
                    </tr>        
                                
                    <tr>
                        <td>Remark </td>
                        <td>:</td>
                        <td><asp:TextBox runat="server" ID="tbDPRemarkDt" CssClass="TextBoxMulti" Width="365px" 
                                MaxLength="255" TextMode="MultiLine" />                        
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
		<asp:Button ID="btnSaveAll" runat="server" class="bitbtndt btnsavenew" Text="Save & New" validationgroup="Input" Width = "90"/>									
        <asp:Button ID="btnSaveTrans" runat="server" class="bitbtndt btnsave" Text="Save" validationgroup="Input"/>									
        <asp:Button ID="btnBack" runat="server" class="bitbtndt btncancel" Text="Cancel" validationgroup="Input"/>									
        <asp:Button ID="btnHome" runat="server" class="bitbtndt btnback" Text="Home" />									                                                           
    </asp:Panel>
        <asp:Panel runat="server" ID="pnlPrint" Visible="false">
    
        </asp:Panel>
      <br />            
    </div>
   
    <asp:Label runat ="server" ID="lbStatus" ForeColor="Red"/>
    </form>
    </body>
</html>
