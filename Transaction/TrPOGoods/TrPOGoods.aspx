<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TrPOGoods.aspx.vb" Inherits="Transaction_TrPOGoods_TrPOGoods" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="BasicFrame.WebControls.BasicDatePicker" Namespace="BasicFrame.WebControls"
    TagPrefix="BDP" %>
    
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>PO - In House</title>
    <script src="../../Function/OpenDlg.js" type="text/javascript"></script>
    <script src="../../Function/Function.JS" type="text/javascript"></script>
      <link type="text/css" rel="stylesheet" href="../../Styles/circularprogress.css" />
    <script type="text/javascript" src="../../JQuery/jquery.min.js"></script>
    <script type="text/javascript">

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
	    
	    z = (x1 + x2).replace(/\$|\,/g,"");
	    	    
	    if (isNaN(z) == true)
        {
           return 0;
        }    
	              
	    return x1 + x2;
            
	    
	    }catch (err){
            alert(err.description);
          }  
        }
        
        function cekNan(nstr)
        {
            if(isNaN(nstr) == true)
            {
                return 0;
            }
            return nstr;
        }
    
        function setformathd(prmchange)
        {
         try
         {           
            document.getElementById("tbBaseForex").value = setdigit(document.getElementById("tbBaseForex").value.replace(/\$|\,/g,""),'<%=ViewState("DigitCurr")%>');
            document.getElementById("tbDiscForex").value = setdigit(document.getElementById("tbDiscForex").value.replace(/\$|\,/g,""),'<%=ViewState("DigitCurr")%>');
            if(prmchange == "DP")
            {
                if (document.getElementById("tbDPForex").value < 0) or(document.getElementById("tbDP").value < 0) 
                {
                document.getElementById("tbDP").value = setdigit(document.getElementById("tbDP").value.replace(/\$|\,/g,""),'<%=ViewState("DigitPercent")%>');                
                document.getElementById("tbDPForex").value = setdigit((document.getElementById("tbBaseForex").value.replace(/\$|\,/g,"") - document.getElementById("tbDiscForex").value.replace(/\$|\,/g,"")) * document.getElementById("tbDP").value.replace(/\$|\,/g,"") / 100, '<%=ViewState("DigitQty")%>');                                
                }
            }
            if(prmchange == "DPForex")
            {
                if (document.getElementById("tbDPForex").value < 0) or(document.getElementById("tbDP").value < 0)
                {
                document.getElementById("tbDPForex").value = setdigit(document.getElementById("tbDPForex").value.replace(/\$|\,/g,""),'<%=ViewState("DigitCurr")%>');
                document.getElementById("tbDP").value = setdigit(document.getElementById("tbDPForex").value.replace(/\$|\,/g, "")/ (document.getElementById("tbBaseForex").value.replace(/\$|\,/g,"") - document.getElementById("tbDiscForex").value.replace(/\$|\,/g,"")) * 100,'<%=ViewState("DigitPercent")%>');
                }
            }
            document.getElementById("tbPPN").value = setdigit(document.getElementById("tbPPN").value.replace(/\$|\,/g, ""));
            document.getElementById("tbPPNForex").value = setdigit((document.getElementById("tbBaseForex").value.replace(/\$|\,/g,"") - document.getElementById("tbDiscForex").value.replace(/\$|\,/g,"")) * document.getElementById("tbPPN").value.replace(/\$|\,/g,"") / 100,'<%=ViewState("DigitPercent")%>');
            document.getElementById("tbOtherForex").value = setdigit(document.getElementById("tbOtherForex").value.replace(/\$|\,/g,""),'<%=ViewState("DigitQty")%>');
            document.getElementById("tbPPHForex").value = setdigit(document.getElementById("tbPPHForex").value.replace(/\$|\,/g,""),'<%=ViewState("DigitQty")%>');                        
            
            document.getElementById("tbTotalForex").value = parseFloat(document.getElementById("tbBaseForex").value.replace(/\$|\,/g, "")) - parseFloat(document.getElementById("tbDiscForex").value.replace(/\$|\,/g, "")) + parseFloat(document.getElementById("tbPPNForex").value.replace(/\$|\,/g, "")) - parseFloat(document.getElementById("tbPPHForex").value.replace(/\$|\,/g, "")) + parseFloat(document.getElementById("tbOtherForex").value.replace(/\$|\,/g, ""));
            document.getElementById("tbTotalForex").value = setdigit(document.getElementById("tbTotalForex").value.replace(/\$|\,/g, ""), '<%=ViewState("DigitQty")%>');
            
            
        }catch (err){
            alert(err.description);
          }      
        } 
        
        function setformatdt(prmchange1)
        {
         try
         {  
//            document.getElementById("tbAmountForexDt").value = setdigit(document.getElementById("tbAmountForexDt").value.replace(/\$|\,/g,""),'<%=ViewState("DigitCurr")%>');
//           
//            if(prmchange1 == "DP")
//            {
//                if (document.getElementById("tbDiscForexDt").value < 0) or(document.getElementById("tbDiscDt").value < 0) 
//                {
//                document.getElementById("tbDiscDt").value = setdigit(document.getElementById("tbDiscDt").value.replace(/\$|\,/g,""),'<%=ViewState("DigitPercent")%>');                
//                document.getElementById("tbDiscForexDt").value = setdigit((document.getElementById("tbAmountForexDt").value.replace(/\$|\,/g,"") * document.getElementById("tbDiscDt").value.replace(/\$|\,/g,""))  / 100, '<%=ViewState("DigitQty")%>');                                
//                }
//            }
//            if(prmchange1 == "DPForex")
//            {
//                if (document.getElementById("tbDiscForexDt").value < 0) or(document.getElementById("tbDiscDt").value < 0)
//                {
//                document.getElementById("tbDiscForexDt").value = setdigit(document.getElementById("tbDiscForexDt").value.replace(/\$|\,/g,""),'<%=ViewState("DigitCurr")%>');
//                document.getElementById("tbDiscDt").value = setdigit((document.getElementById("tbDiscForexDt").value.replace(/\$|\,/g,"")) * 100) / (document.getElementById("tbAmountForexDt").value.replace(/\$|\,/g,""),'<%=ViewState("DigitPercent")%>');
//                }
//            }
            //var priceforex = parseFloat(document.getElementById("tbPriceForexDt").value.replace(/\$|\,/g,""));
            //document.getElementById("tbPriceForexDt").value = setdigit(priceforex,'<%=ViewState("DigitQty")%>');         
            
           // document.getElementById("tbAmountForexDt").value = parseFloat(document.getElementById("tbQtyOrderDt").value.replace(/\$|\,/g,"")) * parseFloat(document.getElementById("tbPriceForexDt").value.replace(/\$|\,/g,""));
            
            //document.getElementById("tbDiscForexDt").value = parseFloat(document.getElementById("tbAmountForexDt").value.replace(/\$|\,/g,"")) * parseFloat(document.getElementById("tbDiscDt").value.replace(/\$|\,/g,"")) / 100;
           // document.getElementById("tbPPHForexDt").value = (parseFloat(document.getElementById("tbAmountForexDt").value.replace(/\$|\,/g,"")) - parseFloat(document.getElementById("tbDiscForexDt").value.replace(/\$|\,/g,""))) * parseFloat(document.getElementById("tbPPHDt").value.replace(/\$|\,/g,"")) / 100;
            //document.getElementById("tbTotalForexDt").value = parseFloat(document.getElementById("tbAmountForexDt").value.replace(",","")) - parseFloat(document.getElementById("tbDiscForexDt").value.replace(/\$|\,/g,""));
            
            //document.getElementById("tbQtyOrderDt").value = setdigit(document.getElementById("tbQtyOrderDt").value.replace(/\$|\,/g,""),'<%=ViewState("DigitQty")%>');
            //document.getElementById("tbQtyWrhsDt").value = setdigit(document.getElementById("tbQtyWrhsDt").value.replace(/\$|\,/g,""),'<%=ViewState("DigitQty")%>');                        
            //document.getElementById("tbAmountForexDt").value = setdigit(document.getElementById("tbAmountForexDt").value.replace(/\$|\,/g,""),'<%=ViewState("DigitQty")%>');
            //document.getElementById("tbDiscDt").value = setdigit(document.getElementById("tbDiscDt").value.replace(/\$|\,/g,""),'<%=ViewState("DigitQty")%>');            
            //document.getElementById("tbDiscForexDt").value = setdigit(document.getElementById("tbDiscForexDt").value.replace(/\$|\,/g,""),'<%=ViewState("DigitQty")%>');
            //document.getElementById("tbPPHDt").value = setdigit(document.getElementById("tbPPHDt").value.replace(/\$|\,/g,""),'<%=ViewState("DigitQty")%>');
            //document.getElementById("tbPPHForexDt").value = setdigit(document.getElementById("tbPPHForexDt").value.replace(/\$|\,/g,""),'<%=ViewState("DigitQty")%>');
            //document.getElementById("tbTotalForexDt").value = setdigit(document.getElementById("tbTotalForexDt").value.replace(/\$|\,/g,""),'<%=ViewState("DigitQty")%>');                        
        }catch (err){
            alert(err.description);
          }      
        }    
        
        function setformatdt2()
        {
         try
         {           
            document.getElementById("tbQtyOrderDt2").value = setdigit(document.getElementById("tbQtyOrderDt2").value.replace(/\$|\,/g,""),'<%=ViewState("DigitQty")%>');
        }catch (err){
            alert(err.description);
          }      
        }   
        
        function setformatdtpr()
        {
         try
         {           
            document.getElementById("tbQtyPR").value = setdigit(document.getElementById("tbQtyPR").value.replace(/\$|\,/g,""),'<%=ViewState("DigitQty")%>');
        }catch (err){
            alert(err.description);
          }      
        }     
       
       
        function closing()
        {
            try
            {
                var result = prompt("Remark Close", "");
                if (result){
                    document.getElementById("HiddenRemarkClose").value = result;
                } else {
                    document.getElementById("HiddenRemarkClose").value = "False Value";
                }
                postback();
                //document.form1.submit();                
            }catch(err){
                alert(err.description);
            }        
        }
        
           function deletetrans()
        {
            try
            {
                
                 var result = confirm("Sure Delete Transaction ?");
                if (result){
                    document.getElementById("HiddenRemarkDelete").value = "true";
                } else {
                    document.getElementById("HiddenRemarkDelete").value = "false";
                }
                postback();
                //document.form1.submit();                
            }catch(err){
                alert(err.description);
            }        
        }
        
        function postback()
        {
            __doPostBack('','');
        }
    </script>    
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />    
</head>
<body>
    <form id="form1" runat="server">
    <div class="Content">
    <div class="H1"><asp:Label runat="server" ID="lblJudul" Text="PO - In House"/></div>
        <hr style="color:Blue" />
        <asp:Panel runat="server" id="PnlHd"> 
      <table>
        <tr>
            <td style="width:100px;text-align:right">Quick Search :</td>            
            <td><asp:TextBox CssClass="TextBox" runat="server" ID ="tbFilter"/> 
                  <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlField" >
                      <asp:ListItem Value ="TransNmbr" Selected="True">PO No</asp:ListItem>
                      <asp:ListItem>Revisi</asp:ListItem>
                      <asp:ListItem Value="dbo.FormatDate(TransDate)">Date</asp:ListItem>
                      <asp:ListItem Value="FgReport">Report</asp:ListItem>
                      <asp:ListItem>Status</asp:ListItem>
                      <asp:ListItem Value="RequestNo">PR No</asp:ListItem>
                      
                      <asp:ListItem Value="POType">PO Type</asp:ListItem>
                      <asp:ListItem>Supplier</asp:ListItem>
                      <asp:ListItem Value="SupplierName">Supplier Name</asp:ListItem>
                      <asp:ListItem Value="Attn">Attn</asp:ListItem>
                      <asp:ListItem Value="SuppContractNo">Supp. Contract No</asp:ListItem>
                      <asp:ListItem Value="CustContractNo">Cust. Contract No</asp:ListItem>
                      
                      <asp:ListItem>Term</asp:ListItem>
                      <asp:ListItem Value="TermPayment">Term Payment</asp:ListItem>
                      <asp:ListItem>Delivery</asp:ListItem>
                      <%--<asp:ListItem Value="DeliveryName">Delivery Name</asp:ListItem>--%>
                      <asp:ListItem Value="ShipmentType">Shipment Type</asp:ListItem>
                      <asp:ListItem Value="FgPriceIncludeTax">Price Include PPN</asp:ListItem>
                      <asp:ListItem>Currency</asp:ListItem>
                      <asp:ListItem>Remark</asp:ListItem>
                      <asp:ListItem Value="ProductCode">Product Code</asp:ListItem>
                      <asp:ListItem Value="ProductName">Product Name</asp:ListItem>
                    </asp:DropDownList>
                    <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlRange"></asp:DropDownList>
                  <asp:Button class="bitbtn btnsearch" runat="server" ID="btnSearch" Text="Search" />
                  <asp:Button class="bitbtn btngo" runat="server" ID="btnExpand" Text="..."/>
            </td>
            <td>
                <asp:LinkButton ID="LbAdvSearch" runat="server" Text="Advanced Search" />
                &nbsp &nbsp &nbsp &nbsp
                <asp:Label runat="server" ID="Label1" Text="Outstanding PR : "/>
                <asp:LinkButton runat="server" ID="lbCount" Text="X" ForeColor="#FF6600" Font-Size="Small" />
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
              <asp:DropDownList runat="server" CssClass="DropDownList" ID="ddlField2" >
                  <asp:ListItem Value ="TransNmbr" Selected="True">PO No</asp:ListItem>
                      <asp:ListItem>Revisi</asp:ListItem>
                      <asp:ListItem Value="dbo.FormatDate(TransDate)">Date</asp:ListItem>
                      <asp:ListItem Value="FgReport">Report</asp:ListItem>
                      <asp:ListItem>Status</asp:ListItem>
                      <asp:ListItem Value="RequestNo">PR No</asp:ListItem>
                      <asp:ListItem Value="POType">PO Type</asp:ListItem>
                      <asp:ListItem>Supplier</asp:ListItem>
                      <asp:ListItem Value="SupplierName">Supplier Name</asp:ListItem>
                      <asp:ListItem Value="Attn">Attn</asp:ListItem>
                      <asp:ListItem Value="SuppContractNo">Supp. Contract No</asp:ListItem>
                      <asp:ListItem Value="CustContractNo">Cust. Contract No</asp:ListItem>
                      <asp:ListItem>Term</asp:ListItem>
                      <asp:ListItem Value="TermPayment">Term Payment</asp:ListItem>
                      <asp:ListItem>Delivery</asp:ListItem>
                      <%--<asp:ListItem Value="DeliveryName">Delivery Name</asp:ListItem>--%>
                      <asp:ListItem Value="ShipmentType">Shipment Type</asp:ListItem>
                      <asp:ListItem Value="FgPriceIncludeTax">Price Include PPN</asp:ListItem>
                      <asp:ListItem>Currency</asp:ListItem>
                      <asp:ListItem>Remark</asp:ListItem>
                      <asp:ListItem Value="ProductCode">Product Code</asp:ListItem>
                      <asp:ListItem Value="ProductName">Product Name</asp:ListItem>
              </asp:DropDownList>
          </td>              
        </tr>        
      </table>      
      </asp:Panel>
          <br />
            <asp:Button class="bitbtn btnadd" runat="server" ID="BtnAdd" Text="Add"/>	 
            &nbsp &nbsp &nbsp   
            <asp:DropDownList CssClass="DropDownList" ID="ddlCommand" runat="server" Visible="false"/>
            <asp:Button class="bitbtn btngo" runat="server" ID="BtnGo" Text="G" Visible="false" />
          <br />
          <div style="border:0px  solid; width:100%; height:100%; overflow:auto;">
          <asp:GridView ID="GridView1" runat="server" AllowPaging="True" AllowSorting="True"
            CssClass="Grid" AutoGenerateColumns="False"> 
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
                              <asp:ListItem Text="Print 2" />
                              <asp:ListItem Text="Print Delivery" />
                              <asp:ListItem Text="Revisi" />
                              
                          </asp:DropDownList>
                          <asp:Button class="bitbtn btngo" runat="server" ID="btnGo" Text="G"	 
                          CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" CommandName="Go" />
                      </ItemTemplate>
                      <HeaderStyle Width="110px" />
                  </asp:TemplateField>
                  <asp:BoundField DataField="TransNmbr" SortExpression="Nmbr" HeaderText="PO No"></asp:BoundField>                  
                  <asp:BoundField DataField="Revisi" HeaderText="Revisi"></asp:BoundField>
                  <asp:BoundField DataField="Status" HeaderText="Status"></asp:BoundField>                  
                  <asp:BoundField DataField="TransDate" HtmlEncode="true" DataFormatString="{0:dd MMM yyyy}" SortExpression="TransDate" HeaderText="Date"></asp:BoundField>
                  <%--<asp:BoundField DataField="FgReport" sortExpression="FgReport" HeaderText="Report"></asp:BoundField>                  --%>
                  <asp:BoundField DataField="POType" SortExpression="POType" HeaderText="PO Type" 
                      HeaderStyle-HorizontalAlign="Left" >
                      <HeaderStyle HorizontalAlign="Left" />
                  </asp:BoundField>
                  <asp:BoundField DataField="DepartmentName" SortExpression="DepartmentName" HeaderText="Department" />
                  <asp:BoundField DataField="Supplier" SortExpression="Supplier" HeaderText="Supplier" />
                  <asp:BoundField DataField="SupplierName" SortExpression="SupplierName" HeaderText="Supplier Name" />
                  <asp:BoundField DataField="Attn" SortExpression="Attn" HeaderText="Attn" />
                  <asp:BoundField DataField="SuppContractNo" SortExpression="SuppContractNo" 
                      HeaderText="No Penawaran" />
                  <asp:BoundField DataField="Term" SortExpression="Term" HeaderText="Term" />
                  <asp:BoundField DataField="TermPayment" SortExpression="TermPayment" HeaderText="Term Payment" />
                  <asp:BoundField DataField="Delivery" SortExpression="Delivery" HeaderText="Delivery" />
                  <asp:BoundField DataField="ShipmentType" SortExpression="ShipmentType" 
                      HeaderText="Shipment Type" HeaderStyle-HorizontalAlign="Left" 
                      HeaderStyle-Width = "60px" >
                      <HeaderStyle HorizontalAlign="Left" Width="60px" />
                  </asp:BoundField>
                  <asp:BoundField DataField="FgPriceIncludeTax" 
                      SortExpression="FgPriceIncludeTax" HeaderText="Price Include PPN" 
                      HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width = "60px" >
                      <HeaderStyle HorizontalAlign="Left" Width="60px" />
                  </asp:BoundField>
                  <%--<asp:BoundField DataField="DeliveryName" SortExpression="DeliveryName" HeaderText="Delivery Name" />--%>
                  <asp:BoundField DataField="Currency" SortExpression="Currency" HeaderText="Currency" />
                  <%--<asp:BoundField DataField="ForexRate" ItemStyle-HorizontalAlign="Right" SortExpression="ForexRate" HeaderText="Forex Rate" />--%>
                  <asp:BoundField DataField="BaseForex" DataFormatString="{0:#,##0.00}" 
                      ItemStyle-HorizontalAlign="Right" SortExpression="BaseForex" 
                      HeaderText="Base Forex" >
                      <ItemStyle HorizontalAlign="Right" />
                  </asp:BoundField>
                  <%--<asp:BoundField DataField="Disc" ItemStyle-HorizontalAlign="Right" SortExpression="Disc" HeaderText="Disc" />--%>
                  <asp:BoundField DataField="DiscForex" DataFormatString="{0:#,##0.00}" 
                      ItemStyle-HorizontalAlign="Right" SortExpression="DiscForex" 
                      HeaderText="Disc Forex" >
                      <ItemStyle HorizontalAlign="Right" />
                  </asp:BoundField>
                  <asp:BoundField DataField="DP" DataFormatString="{0:#,##0.####}" 
                      ItemStyle-HorizontalAlign="Right" SortExpression="DP" HeaderText="DP" >
                      <ItemStyle HorizontalAlign="Right" />
                  </asp:BoundField>
                  <asp:BoundField DataField="DPForex" DataFormatString="{0:#,##0.00}" 
                      ItemStyle-HorizontalAlign="Right" SortExpression="DPForex" 
                      HeaderText="DP Forex" >
                      <ItemStyle HorizontalAlign="Right" />
                  </asp:BoundField>
                  <asp:BoundField DataField="PPn" DataFormatString="{0:#,##0.####}" 
                      ItemStyle-HorizontalAlign="Right" SortExpression="PPn" HeaderText="PPN" >
                      <ItemStyle HorizontalAlign="Right" />
                  </asp:BoundField>
                  <asp:BoundField DataField="PPNForex" DataFormatString="{0:#,##0.00}" 
                      ItemStyle-HorizontalAlign="Right" SortExpression="PPNForex" 
                      HeaderText="PPN Forex" >
                      <ItemStyle HorizontalAlign="Right" />
                  </asp:BoundField>
                  <asp:BoundField DataField="PPHForex" DataFormatString="{0:#,##0.00}" 
                      ItemStyle-HorizontalAlign="Right" SortExpression="PPHForex" 
                      HeaderText="PPH Forex" >
                      <ItemStyle HorizontalAlign="Right" />
                  </asp:BoundField>
                  <asp:BoundField DataField="OtherForex" DataFormatString="{0:#,##0.00}" 
                      ItemStyle-HorizontalAlign="Right" SortExpression="OtherForex" 
                      HeaderText="Other Forex" >
                      <ItemStyle HorizontalAlign="Right" />
                  </asp:BoundField>
                  <asp:BoundField DataField="TotalForex" DataFormatString="{0:#,##0.00}" 
                      ItemStyle-HorizontalAlign="Right" SortExpression="TotalForex" 
                      HeaderText="Total Forex" >
                      <ItemStyle HorizontalAlign="Right" />
                  </asp:BoundField>
                  <asp:BoundField DataField="Remark" sortExpression="Remark" HeaderText="Remark"></asp:BoundField>
                  
              </Columns>
          </asp:GridView>
          </div>
            <asp:Panel runat="server" ID ="pnlNav" Visible="false">
            <asp:Button class="bitbtn btnadd" runat="server" ID="btnAdd2" Text="Add"/>	 
            &nbsp &nbsp &nbsp
            <asp:DropDownList ID="ddlCommand2" CssClass="DropDownList" runat="server"/>
            <asp:Button class="bitbtn btngo" runat="server" ID="btnGo2" Text="G"/>	 
            </asp:Panel>
    </asp:Panel>
    <asp:Panel runat="server" ID="pnlInput" Visible="false">
      <table>
        <tr>
            <td>PO No</td>
            <td>:</td>
            <td colspan="3"><asp:TextBox CssClass="TextBoxR" ValidationGroup="Input" runat="server" ReadOnly="true" ID="tbCode" Width="200px"/>
            <asp:Label runat="server" ID="Label3" Text=" Rev : "></asp:Label>
            <asp:Label runat="server" ID="lbRevisi"></asp:Label>
            </td>           
            
            <td>PO Date</td>
            <td>:</td>
            <td>
            <BDP:BasicDatePicker ID="tbDate" runat="server" DateFormat="dd MMM yyyy" ValidationGroup="Input" 
                        ReadOnly = "true" ShowNoneButton="false"
                        ButtonImageHeight="19px" ButtonImageWidth="20px" 
                        DisplayType="TextBoxAndImage" 
                        TextBoxStyle-CssClass="TextDate"><TextBoxStyle Width="225px" CssClass="TextDate" /></BDP:BasicDatePicker>                
            </td>
        </tr>      
                   
          <tr>
              <td>
                  PO Type</td>
              <td>
                  :</td>
              <td colspan="3">
                  <asp:DropDownList ID="ddlType" runat="server" ValidationGroup="Input" 
                      CssClass="DropDownList" Enabled="true" Width="74px">
                      <asp:ListItem>Lokal</asp:ListItem>
                      <asp:ListItem>Import</asp:ListItem>
                  </asp:DropDownList>                  
                  
              </td>
              <td>
                  <asp:Label runat="server" ID="Label4" Text="Report"></asp:Label>
              </td>
              <td>
                  :</td>
              <td>
                  <asp:DropDownList Id ="ddlReport" Enabled="true" runat="server" Width="74px" 
                    ValidationGroup="Input" CssClass="DropDownList">
                    <asp:ListItem Selected="True">Choose One</asp:ListItem>
                    <asp:ListItem>Y</asp:ListItem>
                    <asp:ListItem>N</asp:ListItem>
                </asp:DropDownList>
                  <asp:Label ID="lbred" runat="server" ForeColor="Red">*</asp:Label>
              </td>
          </tr>
        <tr>
            <td><asp:LinkButton ID="lbSupplier" runat="server" Text="Supplier"/></td>
            <td>:</td>
            <td colspan="3" >
                <asp:TextBox ID="tbSuppCode" ValidationGroup="Input" AutoPostBack="true" Width="80px" runat="server" CssClass="TextBox" />
                <asp:TextBox ID="tbSuppName" Width="207px" runat="server" Enabled="False" 
                    CssClass="TextBox" />
                <asp:Button class="bitbtn btngo" ValidationGroup="Input"  runat="server" ID="btnSupp" Text="..."/>
                <asp:Label ID="lbred1" runat="server" ForeColor="Red">*</asp:Label>
            </td>
            <td>Attn</td>
            <td>:</td>
            <td>
                <asp:TextBox ID="tbAttn" Width="255px" runat="server" ValidationGroup="Input"
                    CssClass="TextBox" MaxLength="255" />
            </td>            
        </tr>    
        <tr>            
            <td>Department</td>
            <td>:</td>
            <td colspan="3">
                <asp:DropDownList runat="server" ID="ddlDept" Width="230px"  CssClass="DropDownList" ValidationGroup="Input">
                </asp:DropDownList>
                <asp:Label ID="Label6" runat="server" ForeColor="Red">*</asp:Label>
            </td>
            <td>No Penawaran</td>
            <td><asp:Label ID="lbTitikDelivPriority" runat="server" Text=":" /></td>            
            <td>
                <asp:TextBox ID="tbSuppPONo" Width="122px" runat="server" ValidationGroup="Input" CssClass="TextBox" MaxLength="255" />
                <asp:TextBox ID="tbCustContractNo" Width="123px" runat="server" 
                    ValidationGroup="Input" CssClass="TextBox" MaxLength="255" />
            </td>
        </tr>       
        <tr>
            <td>Term</td>
            <td>:</td>
            <td colspan="3"><asp:DropDownList ID="ddlTerm" runat="server" 
                    CssClass="DropDownList" ValidationGroup="Input" 
                    Width="230px" AutoPostBack="True" />
                   
            </td>            
            
            <td>Term Payment</td>
            <td>:</td>
            <td>
                <asp:TextBox ID="tbTOPRemark" runat="server" CssClass="TextBox" MaxLength="255" 
                    ValidationGroup="Input" Width="255px" />
            </td>            
        
        </tr>        
       <tr>
            <td>Delivery Place</td>
            <td>:</td>
            <td colspan="3"><asp:DropDownList ID="ddlDelivery" runat="server" 
                    CssClass="DropDownList" ValidationGroup="Input" 
                    Width="144px" AutoPostBack="True"></asp:DropDownList>
                    &nbsp &nbsp Add Cost :&nbsp
                    <asp:DropDownList ID="ddlfgAdditional" ValidationGroup="Input" Width="50px"
                                AutoPostBack="true" runat="server" CssClass="DropDownList" >
                            <asp:ListItem>Y</asp:ListItem>
                            <asp:ListItem>N</asp:ListItem>
                            </asp:DropDownList>
            </td>
                        
            <td>Shipment Type</td>
            <td>:</td>
            <td><asp:DropDownList ID="ddlShipmentType" runat="server" CssClass="DropDownList" ValidationGroup="Input" Width="230px" />                    
            
                <asp:Label ID="lbred2" runat="server" ForeColor="Red">*</asp:Label>
            
            </td>
        </tr>      
          <tr>
              <td>
                  Delivery Addr</td>
              <td>
                  :</td>
              <td>
                  <asp:TextBox ID="tbDeliveryAddr" runat="server" CssClass="TextBoxMulti" ValidationGroup="Input" 
                      MaxLength="255" Width="255px" TextMode="MultiLine" />
                  &nbsp;</td>
              <td>
                  &nbsp;
              </td>
              <td>
                  &nbsp;</td>
              <td>Add Cost Remark</td>
              <td>:</td>
              <td><asp:TextBox ID="tbAddCostRemark" runat="server" CssClass="TextBoxMulti" MaxLength="255" TextMode="MultiLine"
                                ValidationGroup="Input" Width="255px" />
            </td>
          </tr>
          <tr>
              <td>
                  Delivery City
              </td>
              <td>
                  :</td>
              <td>
                  <asp:TextBox ID="tbDeliveryCity" runat="server" CssClass="TextBox" 
                      MaxLength="255" ValidationGroup="Input" Width="255px" />
                      &nbsp
                <asp:Button class="bitbtn btnsearch" ValidationGroup="Input" runat="server" 
                    ID="btnGetData" Text="Get Data" Visible="false"/> 
              </td>
              <td>
                  &nbsp;</td>
              <td>
                  &nbsp;</td>
              <td>
                  &nbsp;</td>
              <td>
                  &nbsp;</td>
              <td>
                  <asp:DropDownList ID="ddlfgInclude" runat="server" ValidationGroup="Input" 
                      CssClass="DropDownList" Enabled="true" Width="50px" AutoPostBack="True" 
                      Visible="False">
                      <asp:ListItem>Y</asp:ListItem>
                      <asp:ListItem>N</asp:ListItem>
                  </asp:DropDownList>
              </td>    
              
          </tr>
       <tr>
            <td><asp:LinkButton ID="lbCurrency"  runat="server" Text="Currency"/></td>
            <td>:</td>
            <td colspan="3"><asp:DropDownList ID="ddlCurrHd" ValidationGroup="Input" AutoPostBack="true" runat="server" CssClass="DropDownList"/>                
            <asp:TextBox ID="tbRateHd" runat="server" ValidationGroup="Input" Width="75px" CssClass="TextBox" />
            </td>
            <td>DP/Payment Progress</td>
            <td>:</td>
            <td><asp:TextBox ID="tbDP" runat="server" ValidationGroup="Input" Width="45px" CssClass="TextBox" /> % = &nbsp
               <asp:TextBox ID="tbDPForex" runat="server" ValidationGroup="Input" Width="110px" CssClass="TextBox" />
            </td>
        </tr>        
        <tr>
            <td>Amount</td>
            <td>:</td>
            <td colspan="6">
                <table cellspacing="0" cellpadding="0">
                    <tr style="background-color:Silver;text-align:center">
                        <td>Base Forex</td>
                        <td>Disc Forex</td>
                        <td>PPN %</td>
                        <td>PPN Forex</td>
                        <td>PPH Forex</td>
                        <td>Other Forex</td>
                        <td>Total Forex</td>
                    </tr>
                    <tr>
                        <td><asp:TextBox ID="tbBaseForex" ValidationGroup="Input" runat="server" 
                                Width="110px" CssClass="TextBoxR" Enabled="False"/></td>
                        <td><asp:TextBox ID="tbDiscForex" ValidationGroup="Input" runat="server" 
                                Width="110px" CssClass="TextBoxR" Enabled="False"/></td>
                        <td><asp:TextBox ID="tbPPN" ValidationGroup="Input" runat="server" Width="40px" CssClass="TextBox"/></td>
                        <td><asp:TextBox ID="tbPPNForex" ValidationGroup="Input" runat="server" 
                                Width="110px" CssClass="TextBoxR" Enabled="False"/></td>
                        <td><asp:TextBox ID="tbPPHForex" ValidationGroup="Input" runat="server" Width="110px" CssClass="TextBoxR"/></td>
                        <td><asp:TextBox ID="tbOtherForex" ValidationGroup="Input" runat="server" Width="110px" CssClass="TextBox"/></td>
                        <td><asp:TextBox ID="tbTotalForex" ValidationGroup="Input" runat="server" Width="110px" CssClass="TextBoxR" /></td>
                    </tr>
                </table>
            </td>                
        </tr>
        <tr>
            <%--<td>Unit Price Dec Place</td>
            <td>:</td>--%>
            <td colspan="2">
                <asp:TextBox ID="tbDPForex0" Visible = "false" runat="server" CssClass="TextBox" 
                    ValidationGroup="Input" Width="54px" />
            </td>
            <%--<td colspan="2">
                Sub Total Dec Place</td>
            <td>--%>
                :</td>
            <td>
                <asp:TextBox ID="tbPPN0" runat="server" Visible ="false"  CssClass="TextBox" 
                    ValidationGroup="Input" Width="40px" />
            </td>
        </tr>
          <tr>
              <td>
                  Remark</td>
              <td>
                  :</td>
              <td colspan="6">
                  <asp:TextBox ID="tbRemark" runat="server" CssClass="TextBoxMulti" 
                      MaxLength="255" TextMode="MultiLine" ValidationGroup="Input" Width="350px" />
              </td>
          </tr>
      </table>  
      
      <br />      
      <div style="font-size:medium; color:Blue;">Detail</div>
      <hr style="color:Blue" />
        <asp:Menu
            ID="Menu1"
            Width="25%"
            runat="server"
            StaticMenuItemStyle-CssClass = "MenuItem"
            StaticSelectedStyle-CssClass = "MenuSelect"    
            Orientation="Horizontal"            
            StaticEnableDefaultPopOutImage="False">            
            <StaticSelectedStyle CssClass="MenuSelect" />
            <StaticMenuItemStyle CssClass="MenuItem" />
            <Items>
                <asp:MenuItem Text="Detail PO" Value="0"></asp:MenuItem>
                <asp:MenuItem Selected="true" Text="Detail PR" Value="1"></asp:MenuItem>
            </Items>
        </asp:Menu>
        <br />
        <asp:MultiView 
        
        ID="MultiView1"
        runat="server"
        ActiveViewIndex="1">
           <asp:View ID="Tab1" runat="server">
              <asp:Panel runat="server" ID="PnlDt">
                <div style="border:0px  solid; width:100%; height:100%; overflow:auto;">
                    <asp:GridView ID="GridDt" runat="server" AutoGenerateColumns="False" 
                        ShowFooter="False">
                        <HeaderStyle CssClass="GridHeader" />
                        <RowStyle CssClass="GridItem" Wrap="false" />
                        <AlternatingRowStyle CssClass="GridAltItem" />
                        <PagerStyle CssClass="GridPager" />
                        <Columns>
                            <%--<asp:BoundField DataField="FgPartialDelivery" HeaderStyle-Width="80px" HeaderText="Partial Delivery" />                            
                            <asp:BoundField DataField="DeliveryDate" DataFormatString="{0:dd MMM yyyy}" HeaderStyle-Width="80px" HeaderText="Delivery Date" HtmlEncode="true" />
                            --%>
                            <asp:TemplateField HeaderText="Action">
                                <ItemTemplate>
                                    <asp:Button ID="btnEdit" runat="server" class="bitbtn btnedit" 
                                        CommandName="Edit" Text="Edit Price" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:Button ID="btnClosing" runat="server" class="bitbtn btnedit" 
                                        CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" 
                                        CommandName="Closing" Text="Closing" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Product" HeaderText="Product" />
                            <asp:BoundField DataField="ProductName" HeaderStyle-Width="250px" 
                                HeaderText="Product Name" >
                                <HeaderStyle Width="250px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Specification" HeaderStyle-Width="250px" 
                                HeaderText="Specification" >
                                <HeaderStyle Width="250px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="QtyWrhsPO" DataFormatString="{0:#,##0.##}" 
                                HeaderStyle-Width="250px" HeaderText="Qty PR" >
                                <HeaderStyle Width="80px" />
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="QtyWrhsFree" HeaderStyle-Width="80px" 
                                HeaderText="Qty Non PR" DataFormatString="{0:#,##0.##}" 
                                ItemStyle-HorizontalAlign="Right" >
                                <HeaderStyle Width="80px" />
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Qty" DataFormatString="{0:#,##0.##}" 
                                HeaderStyle-Width="80px" HeaderText="Total">
                                <HeaderStyle Width="80px" />
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Unit" HeaderStyle-Width="80px" HeaderText="Unit" 
                                ItemStyle-HorizontalAlign="Right" SortExpression="Unit">
                                <HeaderStyle Width="80px" />
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="PriceForex" DataFormatString="{0:#,##0.##}" 
                                HeaderStyle-Width="80px" HeaderText="Price">
                                <HeaderStyle Width="80px" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="QtyOrder" DataFormatString="{0:#,##0.##}" 
                                HeaderStyle-Width="80px" HeaderText="Qty Order" 
                                ItemStyle-HorizontalAlign="Right">
                                <HeaderStyle Width="80px" />
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="UnitOrder" HeaderStyle-Width="80px" 
                                HeaderText="Unit Order" ItemStyle-HorizontalAlign="Left">
                                <HeaderStyle Width="80px" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="QtyPack" DataFormatString="{0:#,##0.##}" 
                                HeaderStyle-Width="80px" HeaderText="@" ItemStyle-HorizontalAlign="Right">
                                <HeaderStyle Width="80px" />
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="BrutoForex" DataFormatString="{0:#,##0.##}" 
                                HeaderStyle-Width="80px" HeaderText="Amount" ItemStyle-HorizontalAlign="Right">
                                <HeaderStyle Width="80px" />
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Disc" DataFormatString="{0:#,##0.##}" 
                                HeaderStyle-Width="80px" HeaderText="Disc %" ItemStyle-HorizontalAlign="Right">
                                <HeaderStyle Width="80px" />
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="DiscForex" DataFormatString="{0:#,##0.##}" 
                                HeaderStyle-Width="80px" HeaderText="Disc Forex" 
                                ItemStyle-HorizontalAlign="Right">
                                <HeaderStyle Width="80px" />
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="PPh" DataFormatString="{0:#,##0.##}" 
                                HeaderStyle-Width="80px" HeaderText="PPh %" ItemStyle-HorizontalAlign="Right">
                                <HeaderStyle Width="80px" />
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="PPhForex" DataFormatString="{0:#,##0.##}" 
                                HeaderStyle-Width="80px" HeaderText="PPh Forex" 
                                ItemStyle-HorizontalAlign="Right">
                                <HeaderStyle Width="80px" />
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="NettoForex" DataFormatString="{0:#,##0.##}" 
                                HeaderStyle-Width="80px" HeaderText="Total Forex" 
                                ItemStyle-HorizontalAlign="Right">
                                <HeaderStyle Width="80px" />
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Remark" HeaderStyle-Width="250px" 
                                HeaderText="Remark">
                                <HeaderStyle Width="250px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="QtyRR" DataFormatString="{0:#,##0.##}" 
                                HeaderStyle-Width="80px" HeaderText="Qty RR" ItemStyle-HorizontalAlign="Right">
                                <HeaderStyle Width="80px" />
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="UserClose" HeaderStyle-Width="80px" 
                                HeaderText="User Close">
                                <HeaderStyle Width="80px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="DateClose" DataFormatString="{0:dd MMM yyyy}" 
                                HeaderStyle-Width="80px" HeaderText="Date Close">
                                <HeaderStyle Width="80px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="RemarkClose" HeaderStyle-Width="250px" 
                                HeaderText="Closing Remark">
                                <HeaderStyle Width="250px" />
                            </asp:BoundField>
                        </Columns>
                    </asp:GridView>
              </div>   
              </asp:Panel> 
              <asp:Panel runat="server" ID="pnlEditDt" Visible="false">
                <table>
                <tr>
                <td style="width:80%">
                    <table style="width:100%; height: 210px;">              
                    <tr>
                        <td>Product</td>
                        <td>:</td>
                        <td colspan="19"><asp:TextBox runat="server" ID="tbProductDt" CssClass="TextBoxR" 
                                Enabled="False" />
                            <asp:TextBox runat="server"  CssClass="TextBox"
                                ID="tbProductNameDt" EnableTheming="True" ReadOnly="True" Enabled="False" 
                                Width="232px"/>                             
                        </td>           
                    </tr>        
                    <tr>
                        <td>Specification</td>
                        <td>:</td>
                        <td colspan="19">
                            <asp:TextBox ID="tbSpecification" runat="server" CssClass="TextBoxMulti" 
                                MaxLength="255" TextMode="MultiLine" Width="365px" />
                        </td>
                    </tr>                            
                    <tr>
                        <td>Qty</td>
                        <td>:</td>
                        <td colspan="19">
                            <table cellspacing="0" cellpadding="0">
                                <tr style="background-color:Silver;text-align:center">
                                    <td>PR</td>
                                    <td>
                                        Non PR</td>
                                    <td>
                                        Total</td>
                                    <td>Unit </td>
                                    <td>
                                        Price<asp:Label ID="lbred3" runat="server" ForeColor="Red">*</asp:Label>
                                    </td>
                                    <td>Qty Order</td>
                                    <td>Unit Order</td>                                
                                    <td>
                                        @</td>
                                </tr>
                                <tr>
                                    <td><asp:TextBox CssClass="TextBoxR" runat="server" ID="tbQtyWrhsPODt" Width="80px" 
                                            Enabled="False" /></td>
                                    <td>
                                        <asp:TextBox ID="tbQtyWrhsFreeDt" runat="server" CssClass="TextBox" 
                                            Enabled="False" Width="80px" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="tbQtyDt" runat="server" CssClass="TextBoxR" Enabled="False" 
                                            Width="80px" />
                                    </td>
                                    <td><asp:DropDownList CssClass="DropDownList" ID="ddlUnitWrhsDt" runat="server" 
                                          Enabled="False"/> </td>
                                    <td>
                                        <asp:TextBox ID="tbPriceForexDt" runat="server" AutoPostBack="true" 
                                            CssClass="TextBox" Width="80px" />
                                    </td>
                                    <td><asp:TextBox CssClass="TextBox" runat="server" ID="tbQtyOrderDt" Width="80px" 
                                            Enabled="True"  AutoPostBack="True"/></td>
                                    <td><asp:DropDownList CssClass="DropDownList" ID="ddlUnitOrderDt" runat="server" 
                                              AutoPostBack="True"/></td>
                                    <td>
                                        <asp:TextBox ID="tbQtyPack" runat="server" CssClass="TextBox" 
                                            Width="80px" AutoPostBack="True" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>                
                    <tr>
                        <td>Amount</td>
                        <td>:</td>
                        <td colspan="19">
                            <table cellspacing="0" cellpadding="0">
                                <tr style="background-color:Silver;text-align:center">
                                    <td>Amount</td>
                                    <td>Disc %</td>
                                    <td>Disc Forex</td>
                                    <td>Total Forex</td>
                                    <td>PPH %</td>
                                    <td>PPH Forex</td>                          
                                              
                                </tr>
                                <tr>
                                    <td><asp:TextBox ID="tbAmountForexDt"  runat="server" CssClass="TextBoxR" 
                                            Width="80px" /></td>
                                    <td><asp:TextBox ID="tbDiscDt"  runat="server" AutoPostBack="true" CssClass="TextBox" Width="45px" /></td>
                                    <td><asp:TextBox ID="tbDiscForexDt" runat="server" CssClass="TextBox" 
                                            Width="80px" AutoPostBack="True" /></td>
                                    <td><asp:TextBox ID="tbTotalForexDt" runat="server" CssClass="TextBoxR" 
                                            Width="80px" /></td>
                                    <td><asp:TextBox ID="tbPPHDt" runat="server" AutoPostBack="true" CssClass="TextBox" Width="45px" /></td>
                                    <td><asp:TextBox ID="tbPPHForexDt" runat="server" CssClass="TextBoxR" 
                                            Width="80px" /></td>
                                    
                                </tr>
                            </table>
                        </td>                
                    </tr>                    
                    <%--<tr>
                                               
                      <td></td>
                        <td>&nbsp;</td>
                        <td> &nbsp;</td>                        
                    </tr>--%>
                    
                    <tr>
                        <td>
                            Remark
                        </td>
                        <td>
                            :</td>
                        <td colspan="16">
                            <asp:TextBox ID="tbRemarkDt" runat="server" CssClass="TextBoxMulti" MaxLength="255" 
                                TextMode="MultiLine" Width="365px" />
                        </td>
                    </tr>
                    <tr>
                        <td>&nbsp;</td>
                        <td>&nbsp;</td>
                        <td>
                            <BDP:BasicDatePicker ID="tbDeliveryDateDt" runat="server" 
                                ButtonImageHeight="19px" ButtonImageWidth="20px" DateFormat="dd MMM yyyy" 
                                DisplayType="TextBoxAndImage" ReadOnly="true" ShowNoneButton="false" 
                                TextBoxStyle-CssClass="TextDate" Enabled="False" Visible="False"><TextBoxStyle CssClass="TextDate" /></BDP:BasicDatePicker>
                        </td>
                        <td>
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                        <td>
                            <asp:DropDownList ID="ddlPartial" runat="server" CssClass="DropDownList" 
                                Enabled="False" ValidationGroup="Input" Visible="False">
                                <asp:ListItem>Y</asp:ListItem>
                                <asp:ListItem Selected="True">N</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td>
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                        <td>&nbsp;</td>
                        <td>&nbsp;</td>
                        <td> 
                            &nbsp;</td> 
                    </tr>
                    </table>
                </td>
                 <td style="vertical-align:top;width:40%">
               		<asp:Panel runat="server" ID="PanelInfo" Visible="false" Height="100%" Width="100%">
                        <asp:Label ID="lbInfo" runat="server" ForeColor="Blue" Font-Bold="true" Text="Info Convert :"></asp:Label>
                        <br />
                        <div style="border:0px  solid; width:100%; height:100%; overflow:auto;">
                            <asp:GridView ID="GridInfo" runat="server" AutoGenerateColumns="false" ShowFooter="False">
                                <HeaderStyle CssClass="GridHeader" />
                                <RowStyle CssClass="GridItem" Wrap="false" />
                                <AlternatingRowStyle CssClass="GridAltItem" />
                                <PagerStyle CssClass="GridPager" />
                                <Columns>
                                    <%--<asp:BoundField DataField="Code" HeaderStyle-Width="120px" HeaderText="Location" />--%>
                                    <asp:BoundField DataField="UnitCode" HeaderStyle-Width="70px" HeaderText="Unit Code" />
                                    <asp:BoundField DataField="UnitConvert" HeaderStyle-Width="70px" HeaderText="Unit Convert" />
                                    <asp:BoundField DataField="Rate" DataFormatString="{0:#,##0.##}" HeaderStyle-Width="70px" HeaderText="Rate" />
                                    
                                </Columns>
                            </asp:GridView>
                        </div>
                    </asp:Panel>
                </td>
                
                <%--<td style="vertical-align:top;">
                    <asp:Panel runat="server" ID="PnlInfo" Visible="true" Height="100%" Width="100%">
                         <asp:Label ID="lbInfo" runat="server" ForeColor="Blue" Font-Bold="true" Text="Info Price :"></asp:Label>
                         <br />
                         <asp:TextBox ID="tbRemarkPrice" runat="server" CssClass="TextBoxMultiR" MaxLength="255" 
                            TextMode="MultiLine" Width="365px" Height="100px">
                         </asp:TextBox>
                    </asp:Panel>
                </td>--%>
                </tr>
                </table>
                
                <br />                     
                  <asp:Button ID="btnSaveDt" runat="server" class="bitbtndt btnsave" Text="Save" 
                      validationgroup="Input" />
                  <asp:Button ID="btnCancelDt" runat="server" class="bitbtndt btncancel" 
                      Text="Cancel" />
                
           </asp:Panel> 
              
           </asp:View>
           <asp:View ID="Tab2" runat="server">                
                <asp:Panel ID="pnlDtPR" runat="server">
                <asp:Button ID="btnAddDtPR" runat="server" class="bitbtndt btnadd" Text="Add" validationgroup="Input" Width = "60"/>									
                <br /><br />
                <div style="border:0px  solid; width:100%; height:100%; overflow:auto;">
                    <asp:GridView ID="GridPR" runat="server" AutoGenerateColumns="False" 
                        ShowFooter="False">
                        <HeaderStyle CssClass="GridHeader" />
                        <RowStyle CssClass="GridItem" Wrap="false" />
                        <AlternatingRowStyle CssClass="GridAltItem" />
                        <PagerStyle CssClass="GridPager" />
                        <Columns>
                            <asp:TemplateField HeaderText="Action">
                            <ItemTemplate>
                            <asp:Button  class="bitbtn btnedit" runat="server" ID="btnEdit" Text="Edit Qty" CommandName="Edit" />
                            <asp:Button  class="bitbtn btndelete" runat="server" ID="btnDelete" Text="Delete" CommandName="Delete"/>
                            </ItemTemplate>
                            <EditItemTemplate>
                            <asp:Button  class="bitbtn btnupdate" runat="server" ID="btnUpdate" Text="Update" CommandName="Update"/>
                            <asp:Button  class="bitbtn btncancel" runat="server" ID="btnCancel" Text="Cancel" CommandName="Cancel"/>
                            </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="RequestNo" HeaderText="Request No" />
                            <asp:BoundField DataField="PRDelivery" DataFormatString="{0:dd MMM yyyy}" 
                                    HeaderText="PR Delivery" />
                                <asp:BoundField DataField="Delivery" DataFormatString="{0:dd MMM yyyy}" 
                                    HeaderText="Delivery Date" FooterStyle-HorizontalAlign="Right" >                                
                                    <FooterStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Product" HeaderText="Product" />
                            <asp:BoundField DataField="ProductName" HeaderStyle-Width="250px" 
                                HeaderText="Product Name" >
                                <HeaderStyle Width="250px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Specification" HeaderText="Specification" />                            
                            <asp:BoundField DataField="Qty" HeaderText="Qty" HeaderStyle-Width="80px" 
                                ItemStyle-HorizontalAlign="Right" >                            
                                <HeaderStyle Width="80px" />
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="QtyPR" ItemStyle-HorizontalAlign="Right" 
                                HeaderStyle-Width="80px" HeaderText="Qty PR Ori" >
                                <HeaderStyle Width="80px" />
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Unit" HeaderStyle-Width="80px" HeaderText="Unit" >
                                <HeaderStyle Width="80px" />
                            </asp:BoundField>
                            <%--<asp:BoundField DataField="UnitOrder" HeaderStyle-Width="80px" HeaderText="Unit Order" />--%>
                        </Columns>
                    </asp:GridView>
              </div>    
              <br />
              <asp:Button ID="btnAddDtPRke2" runat="server" class="bitbtndt btnadd" Text="Add" validationgroup="Input" Width = "60"/>									

              </asp:Panel>
              <asp:Panel runat="server" ID="pnlEditDtPR" Visible="false">
                <table>      
                    <tr>
                        <td>Request No</td>
                        <td>:</td>
                        <td><asp:TextBox Width="200px" CssClass="TextBoxR" runat="server" ID="tbRequestPR" 
                                ReadOnly="True"/>
                                <asp:Button class="bitbtn btngo" runat="server" ID="btnRequestPR" Text="..."/>
                            <asp:Label ID="lbred4" runat="server" ForeColor="Red">*</asp:Label>
                            <asp:TextBox Width="20px" runat="server" ID="tbStatus" Visible="False"/>
                        </td>
                    </tr>        
                    <tr>
                            <td>
                                Request Delivery Date</td>
                            <td>
                                :</td>
                            <td>
                                <BDP:BasicDatePicker ID="tbPRDelivery" runat="server" ButtonImageHeight="19px" 
                                    ButtonImageWidth="20px" DateFormat="dd MMM yyyy" DisplayType="TextBoxAndImage" 
                                    Enabled="False" ReadOnly="true" ShowNoneButton="false" 
                                    TextBoxStyle-CssClass="TextDate"><TextBoxStyle CssClass="TextDate" /></BDP:BasicDatePicker>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Delivery Date</td>
                            <td>
                                :</td>
                            <td>
                                <BDP:BasicDatePicker ID="tbDeliveryDateDt2" runat="server" 
                                    ButtonImageHeight="19px" ButtonImageWidth="20px" DateFormat="dd MMM yyyy" 
                                    DisplayType="TextBoxAndImage" ReadOnly="true" ShowNoneButton="false" 
                                    TextBoxStyle-CssClass="TextDate"><TextBoxStyle CssClass="TextDate" /></BDP:BasicDatePicker>
                            </td>
                        </tr>
                    <tr>
                        <td>Product</td>
                        <td>:</td>
                        <td><asp:TextBox runat="server" ID="tbProductPR" ReadOnly="true" CssClass="TextBoxR" AutoPostBack="true" />
                            <asp:TextBox runat="server" 
                                ID="tbProductNamePR" EnableTheming="True" ReadOnly="true" 
                                CssClass="TextBoxR" Enabled="False" 
                                Width="232px"/>                             
                        </td>           
                    </tr>  
                    <tr>
                        <td>Specification</td>
                        <td>:</td>
                        <td><asp:TextBox Width="360px" ReadOnly="true" CssClass="TextBox" runat="server" 
                                ID="tbSpecificationPR" TextMode="MultiLine"/></td>
                    </tr>                                  
                    
                    <tr>
                        <td>
                            Qty</td>
                        <td>
                            :</td>
                        <td>
                            <table cellpadding="0" cellspacing="0">
                                <tr style="background-color:Silver;text-align:center">
                                    <td>
                                        Qty PO</td>
                                    <td>
                                        Qty PR Ori</td>
                                    <td>
                                        Unit</td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="tbQtyPR" runat="server" CssClass="TextBox" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="tbQtyPROri" runat="server" CssClass="TextBoxR" 
                                            Enabled="False" />
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlUnitPR" runat="server" CssClass="DropDownList" 
                                            Enabled="False" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    
                </table>
                <br />  
                
                  <asp:Button ID="btnSavePR" runat="server" class="bitbtndt btnsave" Text="Save" 
                      validationgroup="Input" />
                <asp:Button ID="btnCancelPR" runat="server" class="bitbtndt btncancel" Text="Cancel" />									
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
    <asp:HiddenField ID="HiddenRemarkClose" runat="server" />
    <asp:HiddenField ID="HiddenRemarkDelete" runat="server" />
    
        <div class="loading" align="center">
      <br />
       <img src="../../Image/loader.gif" alt="" />
    </div>
    
    </form>
</body>
</html>
