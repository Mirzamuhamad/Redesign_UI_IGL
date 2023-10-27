

<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TrCustInv.aspx.vb" Inherits="CustInv" %>
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
        
        function setformat()
        {
        try
         {          
        var Rate = document.getElementById("tbRate").value.replace(/\$|\,/g,"");
        var PPn = document.getElementById("tbPPN").value.replace(/\$|\,/g,"");        
        var BaseForex = document.getElementById("tbBaseForex").value.replace(/\$|\,/g,"");
        var DiscForex = document.getElementById("tbDiscForex").value.replace(/\$|\,/g,"");        
        var PPnForex = document.getElementById("tbPPNForex").value.replace(/\$|\,/g,"");
        var TotalForex = document.getElementById("tbTotalForex").value.replace(/\$|\,/g,"");
        //var fgInclude = document.getElementById("ddlfgInclude").value.replace(/\$|\,/g,"");
        var DPForex = document.getElementById("tbDPForex").value.replace(/\$|\,/g,"");
        var TotalInvoice = document.getElementById("tbTotalInvoice").value.replace(/\$|\,/g,"");
                
//        if (fgInclude == 'Y')
//        {   PPnForex = (parseFloat(TotalInvoice)*PPn)/(100+parseFloat(PPn));
//            BaseForex = ((parseFloat(TotalInvoice)*100)/(100+parseFloat(PPn))) + parseFloat(DiscForex) + parseFloat(DPForex);
//            
//        } else    
//        {               
//            PPnForex = (parseFloat(BaseForex) - parseFloat(DiscForex) - parseFloat(DPForex)) * parseFloat(PPn)/100;                        
//            TotalInvoice = (parseFloat(BaseForex) - parseFloat(DiscForex) - parseFloat(DPForex)) + parseFloat(PPnForex);
//        }
//        TotalForex = parseFloat(BaseForex) - parseFloat(DiscForex) - parseFloat(DPForex)
        document.getElementById("tbPPN").value = setdigit(PPn, '<%=ViewState("DigitCurr")%>');
        document.getElementById("tbBaseForex").value = setdigit(BaseForex,'<%=VIEWSTATE("DigitCurr")%>');
        document.getElementById("tbDiscForex").value = setdigit(DiscForex,'<%=VIEWSTATE("DigitCurr")%>');
        document.getElementById("tbPPNForex").value = setdigit(PPnForex,'<%=VIEWSTATE("DigitCurr")%>');
        document.getElementById("tbTotalForex").value = setdigit(TotalForex,'<%=VIEWSTATE("DigitCurr")%>');
        document.getElementById("tbDPForex").value = setdigit(DPForex,'<%=VIEWSTATE("DigitCurr")%>');
        document.getElementById("tbTotalInvoice").value = setdigit(TotalInvoice,'<%=VIEWSTATE("DigitCurr")%>');
        }catch (err){
            alert(err.description);
          }      
        }

        function setformatdt(prmchange)
        {
        try {         
//         var qty = document.getElementById("tbQty").value.replace(/\$|\,/g,"");
//         var price = document.getElementById("tbPriceForex").value.replace(/\$|\,/g,"");
//         //var discforex = document.getElementById("tbDiscForexDt").value.replace(/\$|\,/g, "");
//         //var disc = document.getElementById("tbDisc").value.replace(/\$|\,/g, "");

//         var subtotal = qty*price;
//         var total = subtotal;
//         
//            
//         //document.getElementById("tbBaseForex").value = setdigit(document.getElementById("tbBaseForex").value.replace(/\$|\,/g, ""), '<%=ViewState("DigitCurr")%>');
//         //document.getElementById("tbDiscForexDt").value = setdigit(document.getElementById("tbDiscForexDt").value.replace(/\$|\,/g, ""), '<%=ViewState("DigitCurr")%>');
//         if (prmchange == "Disc") {
//             document.getElementById("tbDisc").value = setdigit(document.getElementById("tbDisc").value.replace(/\$|\,/g, ""), '<%=ViewState("DigitPercent")%>');
//             //document.getElementById("tbDiscForexDt").value = setdigit((document.getElementById("tbBaseForex").value.replace(/\$|\,/g, "") - document.getElementById("tbDiscForex").value.replace(/\$|\,/g, "")) * document.getElementById("tbDP").value.replace(/\$|\,/g, "") / 100, '<%=ViewState("DigitQty")%>');
//         }
//         if (prmchange == "DiscForex") {
//             //document.getElementById("tbDiscForexDt").value = setdigit(document.getElementById("tbDiscForexDt").value.replace(/\$|\,/g, ""), '<%=ViewState("DigitCurr")%>');
//             document.getElementById("tbDisc").value = setdigit(document.getElementById("tbDiscForexDt").value.replace(/\$|\,/g, "") / (document.getElementById("tbBaseForex").value.replace(/\$|\,/g, "") - document.getElementById("tbDiscForex").value.replace(/\$|\,/g, "")) * 100, '<%=ViewState("DigitPercent")%>');
//         }
//            
//         document.getElementById("tbQty").value = setdigit(qty, '<%=ViewState("DigitQty")%>');                  
//         //document.getElementById("tbPriceForex").value = setdigit(price,'<%=ViewState("DigitCurr")%>');
//         //document.getElementById("tbAmountForex").value = setdigit(subtotal,'<%=ViewState("DigitCurr")%>');
//         //document.getElementById("tbDisc").value = setdigit(disc, '<%=ViewState("DigitCurr")%>');
//         //document.getElementById("tbDiscForexDt").value = setdigit(discforex, '<%=ViewState("DigitCurr")%>');
//         document.getElementById("tbTotalForexDt").value = setdigit(total,'<%=ViewState("DigitCurr")%>');

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
        var DPRate = document.getElementById("tbDPRate").value.replace(/\$|\,/g,"");         
        var CIRate = document.getElementById("tbRate").value.replace(/\$|\,/g,"");         
        //var DPTotalForex = document.getElementById("tbDPTotalForex").value.replace(/\$|\,/g,"");
        var DPTotalForex = parseFloat(DPBaseForex) + parseFloat(DPPPnForex);         
        var DPDPInvoice = (parseFloat(DPBaseForex) * parseFloat(DPRate)) / parseFloat(CIRate)) 
        document.getElementById("tbDPBaseForex").value = setdigit(DPBaseForex,'<%=VIEWSTATE("DigitCurrDt2")%>');
        document.getElementById("tbDPPPnForex").value = setdigit(DPPPnForex,'<%=VIEWSTATE("DigitCurrDt2")%>');
        document.getElementById("tbDPTotalForex").value = setdigit(DPTotalForex,'<%=VIEWSTATE("DigitCurrDt2")%>'); 
        document.getElementById("tbDPDPInvoice").value = setdigit(DPDPInvoice,'<%=VIEWSTATE("DigitHome")%>');                
        }catch (err){
            alert(err.description);
          }      
        }     
    </script>
    
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
</head>
<body>     
    <form id="form1" runat="server">    
    <div class="Content">
    <div class="H1">Customer Invoice <asp:Label ID="lblJudul" runat="server" Text=""/></div>
     <hr style="color:Blue" />        
    <asp:Panel runat="server" id="PnlHd"> 
      <table>
        <tr>
            <td style="width:100px;text-align:right">Quick Search :</td>            
            <td><asp:TextBox CssClass="TextBox" runat="server" ID ="tbFilter"/> 
                  <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlField" >
                      <asp:ListItem Value="TransNmbr" Selected="True">Reference</asp:ListItem>
                      <asp:ListItem>Status</asp:ListItem>
                      <asp:ListItem Value="Printed">Printed</asp:ListItem>
                      <asp:ListItem Value="dbo.FormatDate(TransDate)">Date</asp:ListItem>
                      <asp:ListItem Value="PPnNo">PPn No</asp:ListItem>
                      <asp:ListItem Value="BPB_No">SJ No</asp:ListItem>
                      <asp:ListItem Value="SO_No">SO No</asp:ListItem>
                      <asp:ListItem Value="CustPONo">Cust. PO No</asp:ListItem>
                      <asp:ListItem Value="Customer_Name">Customer Name</asp:ListItem>
                      <asp:ListItem Value="Bill_To_Name">Bill To</asp:ListItem>                      
                      <asp:ListItem Value="Term_Name">Term</asp:ListItem>
                      <asp:ListItem Value="dbo.FormatDate(DueDate)">Due Date</asp:ListItem>
                      <asp:ListItem Value="Currency">Currency</asp:ListItem> 
                      <asp:ListItem Value="DPNo">DP No</asp:ListItem>                     
                      <asp:ListItem>Remark</asp:ListItem>
                   </asp:DropDownList>
                  <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlRange"></asp:DropDownList>
                  <asp:Button class="bitbtn btnsearch" runat="server" ID="btnSearch" Text="Search" />
                <asp:Button class="btngo" runat="server" ID="btnExpand" Text="..."/>
            </td>
            <td>
                <asp:LinkButton ID="LbAdvSearch" runat="server" Text="Advanced Search" />
				&nbsp &nbsp &nbsp &nbsp
                <asp:Label runat="server" ID="Label1" Text="Outstanding SJ : "/>
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
                      <asp:ListItem Value="TransNmbr" Selected="True">TransNmbr</asp:ListItem>
                      <asp:ListItem>Status</asp:ListItem>
                      <asp:ListItem Value="Printed">Printed</asp:ListItem>
                      <asp:ListItem Value="dbo.FormatDate(TransDate)">Date</asp:ListItem>
                      <asp:ListItem Value="PPnNo">PPn No</asp:ListItem>
                      <asp:ListItem Value="BPB_No">SJ No</asp:ListItem>
                      <asp:ListItem Value="SO_No">SO No</asp:ListItem>
                      <asp:ListItem Value="CustPONo">Cust. PO No</asp:ListItem>
                      <asp:ListItem Value="Customer_Name">Customer Name</asp:ListItem>
                      <asp:ListItem Value="Bill_To_Name">Bill To</asp:ListItem>
                      <asp:ListItem Value="Term_Name">Term</asp:ListItem>
                      <asp:ListItem Value="dbo.FormatDate(DueDate)">Due Date</asp:ListItem>
                      <asp:ListItem Value="Currency">Currency</asp:ListItem>
                      <asp:ListItem Value="DPNo">DP No</asp:ListItem>
                      <asp:ListItem>Remark</asp:ListItem>
                      
              </asp:DropDownList>
          </td>              
        </tr>        
      </table>      
      </asp:Panel>
          <br />
            <asp:Button class="bitbtn btnadd" runat="server" ID="BtnAdd" Text="Add"/>	 
            &nbsp &nbsp &nbsp   
            <asp:DropDownList CssClass="DropDownList" ID="ddlCommand" runat="server" Visible="false"/>
            <asp:Button class="bitbtn btngo" runat="server" ID="BtnGo" Text="G" Visible="false"/>
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
                          <asp:CheckBox ID="cbSelectHd" runat="server" AutoPostBack="true" oncheckedchanged="cbSelectHd_CheckedChanged" />
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
                              <%--<asp:ListItem Text="Print 2" />
                              <asp:ListItem Text="Print Export" />--%>
                              <asp:ListItem Text="Print Tax" />
                          </asp:DropDownList>
                          <asp:Button class="bitbtn btngo" runat="server" ID="btnGo" Text="G"	 
                          CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" CommandName="Go" />
                      </ItemTemplate>
                      <HeaderStyle Width="110px" />
                  </asp:TemplateField>
                  <asp:BoundField DataField="TransNmbr" HeaderStyle-Width="120px" SortExpression="Nmbr" HeaderText="Invoice No"></asp:BoundField>                  
                  <asp:BoundField DataField="Status" HeaderText="Status"></asp:BoundField>                  
                  <asp:BoundField DataField="TransDate" dataformatstring="{0:dd MMM yyyy}" htmlencode="true" HeaderStyle-Width="80px" SortExpression="TransDate" HeaderText="Date"></asp:BoundField>
                  <asp:BoundField DataField="PPnNo" HeaderStyle-Width="120px" SortExpression="PPnNo" HeaderText="PPn No"></asp:BoundField>
                  <asp:BoundField DataField="Customer" HeaderStyle-Width="80px" SortExpression="Customer" HeaderText="Customer"></asp:BoundField>
                  <asp:BoundField DataField="Customer_Name" HeaderStyle-Width="200px" SortExpression="Customer_Name" HeaderText="Customer Name"></asp:BoundField>
                  <asp:BoundField DataField="CustTaxAddress" HeaderStyle-Width="180px" SortExpression="CustTaxAddress" HeaderText="Cust. Tax Address"></asp:BoundField>
                  <asp:BoundField DataField="CustTaxNPWP" HeaderStyle-Width="120px" SortExpression="CustTaxNPWP" HeaderText="Cust. Tax NPWP"></asp:BoundField>
                  <asp:BoundField DataField="Bill_To_Name" HeaderStyle-Width="200px" SortExpression="Bill_To_Name" HeaderText="Bill To"></asp:BoundField>
                  <%--<asp:BoundField DataField="Reference" HeaderStyle-Width="120px" SortExpression="Reference" HeaderText="Reference"></asp:BoundField>--%>
                  <asp:BoundField DataField="FgPriceIncludeTax" HeaderStyle-Width="30px" SortExpression="FgPriceIncludeTax" HeaderText="Price include"></asp:BoundField>
                   <asp:BoundField DataField="Term_Name" HeaderStyle-Width="150px" SortExpression="Term_Name" HeaderText="Term"></asp:BoundField>
                  <asp:BoundField DataField="DueDate" dataformatstring="{0:dd MMM yyyy}" htmlencode="true" HeaderStyle-Width="80px" SortExpression="DueDate" HeaderText="Due Date"></asp:BoundField>
                  <asp:BoundField DataField="Currency" HeaderStyle-Width="30px" SortExpression="Currency" HeaderText="Currency"></asp:BoundField>
                  <asp:BoundField DataField="BaseForex" DataFormatString="{0:#,##0.00}" HeaderStyle-Width="80px" SortExpression="BaseForex" ItemStyle-HorizontalAlign="Right" HeaderText="Base Forex"></asp:BoundField>
                  <asp:BoundField DataField="DiscForex" DataFormatString="{0:#,##0.00}" HeaderStyle-Width="80px" SortExpression="DiscForex" ItemStyle-HorizontalAlign="Right" HeaderText="Disc Forex"></asp:BoundField>
                  <asp:BoundField DataField="DPForex" DataFormatString="{0:#,##0.00}" HeaderStyle-Width="80px" SortExpression="DPForex" ItemStyle-HorizontalAlign="Right" HeaderText="DP Forex"></asp:BoundField>
                  <asp:BoundField DataField="TotalForex" DataFormatString="{0:#,##0.00}" HeaderStyle-Width="80px" SortExpression="TotalForex" ItemStyle-HorizontalAlign="Right" HeaderText="Total Net Amount"></asp:BoundField>
                  <asp:BoundField DataField="PPnForex" DataFormatString="{0:#,##0.00}" HeaderStyle-Width="80px" SortExpression="PPnForex" ItemStyle-HorizontalAlign="Right" HeaderText="PPn Forex"></asp:BoundField>
                  <asp:BoundField DataField="PPnHome" DataFormatString="{0:#,##0.00}" HeaderStyle-Width="80px" SortExpression="PPnHome" ItemStyle-HorizontalAlign="Right" HeaderText="PPn (IDR)"></asp:BoundField>                  
                  <asp:BoundField DataField="TotalInvoice" DataFormatString="{0:#,##0.00}" HeaderStyle-Width="80px" SortExpression="TotalInvoice" ItemStyle-HorizontalAlign="Right" HeaderText="Total Gross Amount"></asp:BoundField>
                  <asp:BoundField DataField="Remark" HeaderStyle-Width="250px" HeaderText="Remark"></asp:BoundField>
                </Columns>
          </asp:GridView>
          </div>
            <asp:Panel runat="server" ID ="pnlNav" Visible="false">
            <%--<asp:Button class="bitbtn btngo" runat="server" ID="btnGo" Text="G"	 
                          CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" CommandName="Go" />--%>
            <asp:Button ID="BtnAdd2" runat="server" class="bitbtn btnadd" Text="Add" />
            &nbsp &nbsp &nbsp
            <asp:DropDownList ID="ddlCommand2" CssClass="DropDownList" runat="server"/>
            <asp:Button class="bitbtn btngo" runat="server" ID="btnGo2" Text="G"/>	                
                
                
            </asp:Panel>
    </asp:Panel>    
    <asp:Panel runat="server" ID="pnlInput" Visible="false">
        
        <table>
            <tr>
                <td>Invoice No</td>
                <td>:</td>
                <td><asp:TextBox ID="tbCode" runat="server" CssClass="TextBoxR" Enabled="False" Width="150px" />
                    <asp:DropDownList ID="ddlReport" runat="server" AutoPostBack="true" CssClass="DropDownList" Visible="False" >
                        <asp:ListItem Selected="True">Y</asp:ListItem>
                        <asp:ListItem>N</asp:ListItem>
                    </asp:DropDownList>                    
                </td>
                <td>Date</td>
                <td>:</td>
                <td><BDP:BasicDatePicker ID="tbDate" runat="server" AutoPostBack="True" 
                        ButtonImageHeight="19px" ButtonImageWidth="20px" DateFormat="dd MMM yyyy" 
                        DisplayType="TextBoxAndImage" ReadOnly="true" ShowNoneButton="False" 
                        TextBoxStyle-CssClass="TextDate" ValidationGroup="Input">
                        <TextBoxStyle CssClass="TextDate" />
                    </BDP:BasicDatePicker>
                </td>
            </tr>
            <tr>
                <td><asp:LinkButton ID="lbCust" runat="server" Text="Customer" ValidationGroup="Input" /></td>
                <td>:</td>
                <td colspan="4">
                    <asp:TextBox ID="tbCustCode" runat="server" AutoPostBack="true" CssClass="TextBox" ValidationGroup="Input" />
                    <asp:TextBox ID="tbCustName" runat="server" CssClass="TextBox" Enabled="false" Width="260px" />
                    <asp:Button ID="btnCust" runat="server" class="btngo" Text="..." ValidationGroup="Input" />
                    <asp:Label ID="Label11" runat="server" ForeColor="#FF3300" Text="*"></asp:Label>
                </td>                
            </tr>
            <tr>
                <td>Cust. Tax Address</td>
                <td>:</td>
                <td colspan="4">
                    <asp:TextBox runat="server" ID="tbCustTaxAddress" CssClass="TextBoxMulti" Width="365px" MaxLength="255" TextMode="MultiLine" /> 
                    <asp:Button ID="btnCustTax" runat="server" class="btngo" Text="..." ValidationGroup="Input" />
                </td>
            </tr>
            <tr>
                <td>Cust. Tax NPWP</td>
                <td>:</td>
                <td colspan="4">
                    <asp:TextBox ID="tbCustTaxNPWP" runat="server" CssClass="TextBox" ValidationGroup="Input" Width="225px" />                              
                </td>
            </tr>
            <tr>
                <td><asp:Label ID="lbBillTo" runat="server" Text="Bill To" ValidationGroup="Input" /></td>
                <td>:</td>
                <td colspan="4">                
                    <asp:TextBox ID="tbBillToCode" runat="server" AutoPostBack="true" CssClass="TextBox" ValidationGroup="Input" />
                    <asp:TextBox ID="tbBillToName" runat="server" CssClass="TextBox" Enabled="false" Width="260px" />
                    <asp:Button ID="btnBillTo" runat="server" class="btngo" Text="..." ValidationGroup="Input" />
                </td>                                                                                
                   
            </tr>
            <tr>
                <td>Attn</td>
                <td>:</td>
                <td colspan="4">
                    <asp:TextBox ID="tbAttn" runat="server" CssClass="TextBox" ValidationGroup="Input" Width="225px" />
                </td>
            </tr>
            <tr>
                <td><asp:LinkButton ID="lbTerm" runat="server" Text="Term" ValidationGroup="Input" /></td>
                <td>:</td>
                <td colspan="5">
                    <asp:DropDownList ID="ddlTerm" runat="server" AutoPostBack="true" CssClass="DropDownList" ValidationGroup="Input" />
                    <BDP:BasicDatePicker ID="tbDueDate" runat="server" AutoPostBack="True" 
                        ButtonImageHeight="19px" ButtonImageWidth="20px" DateFormat="dd MMM yyyy" 
                        DisplayType="TextBox" Enabled="false" ReadOnly="true" ShowNoneButton="False" 
                        TextBoxStyle-CssClass="TextDate">
                        <TextBoxStyle CssClass="TextDate" />
                    </BDP:BasicDatePicker>
                    <asp:Button ID="BtnGetData" runat="server" class="bitbtn btnsearch" 
                        Text="Get Data" ValidationGroup="Input" />
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
                            <td><asp:TextBox ID="tbPPnNo" runat="server" CssClass="TextBox" ValidationGroup="Input" Width="150px" /></td>
                            <td><BDP:BasicDatePicker ID="tbPPndate" runat="server" AutoPostBack="True" 
                                    ButtonImageHeight="19px" ButtonImageWidth="20px" DateFormat="dd MMM yyyy" 
                                    DisplayType="TextBoxAndImage" ReadOnly="true" ShowNoneButton="False" 
                                    TextBoxStyle-CssClass="TextDate" ValidationGroup="Input">
                                    <TextBoxStyle CssClass="TextDate" />
                                </BDP:BasicDatePicker>
                            </td>
                            <td><asp:TextBox ID="tbPpnRate" runat="server" CssClass="TextBox" ValidationGroup="Input" Width="68px" /></td>
                        </tr>
                    </table>
                </td>
				
				
				 <td colspan="3">
                    <table>
                        <tr style="background-color:Silver;text-align:center">
                            <td>Edit Price</td>
                            
                        </tr>
                        <tr>
                             <td><asp:TextBox ID="tbEditPrice" runat="server" CssClass="TextBox" AutoPostBack="True" ValidationGroup="Input" Width="100px" /></td>
                        </tr>
                    </table>
                </td>
				
                
            </tr>
            <tr>
                <td><asp:LinkButton ID="lbCurr" runat="server" Text="Currency" ValidationGroup="Input" /></td>
                <td>:</td>
                <td><asp:DropDownList ID="ddlCurr" runat="server" AutoPostBack="true" CssClass="DropDownList" ValidationGroup="Input" />
                    &nbsp; Rate : &nbsp;
                    <asp:TextBox ID="tbRate" runat="server" CssClass="TextBox" ValidationGroup="Input" Width="68px" />                        
                    <asp:Label ID="lbDigit" runat="server" Text=""/>
                </td>           
                
                <td>Price Include PPN</td>
                <td>:</td>
                <td>
                    <asp:DropDownList ID="ddlfgInclude" runat="server" CssClass="DropDownList" Enabled="false">
                        <asp:ListItem Selected="True" Text="N" />
                        <asp:ListItem Text="Y" />
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>Total</td>
                <td>:</td>
                <td colspan="4">
                    <table>
                        <tr style="background-color:Silver;text-align:center">
                            <td>Base Forex</td>
                            <%--<td>Disc %</td>--%> <%--<td>Disc Forex</td>--%>
                            <td>Disc Forex</td>
                            <td>DP Forex</td>
                            <td>Total Net Amount</td>
                            <td>PPN %</td>
                            <td>PPN Forex</td>
                            <td>Total Gross Amount</td>
                            
                        </tr>
                        <tr>
                            <td><asp:TextBox ID="tbBaseForex" runat="server" CssClass="TextBoxR" Width="90px" /></td>
                            <%--<td><asp:TextBox ID="tbDisc" runat="server" CssClass="TextBox" width="40px"/></td>--%>
                            <%--<td><asp:TextBox ID="tbDiscForex" ValidationGroup="Input" runat="server" CssClass="TextBox" Width="90px"/></td>--%>
                            <td><asp:TextBox ID="tbDiscForex" runat="server" CssClass="TextBoxR" Width="90px" /></td>
                            <td><asp:TextBox ID="tbDPForex" runat="server" CssClass="TextBoxR" Width="90px"/></td>
                            <td><asp:TextBox ID="tbTotalForex" runat="server" CssClass="TextBoxR" Width="120px" /></td>
                            <td><asp:TextBox ID="tbPPN" runat="server" CssClass="TextBox" ValidationGroup="Input" width="40px" /></td>
                            <td><asp:TextBox ID="tbPPNForex" runat="server" CssClass="TextBoxR" Width="90px" /></td>
                            <td><asp:TextBox ID="tbTotalInvoice" runat="server" CssClass="TextBoxR" Width="120px" /></td>
                            
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>Remark</td>
                <td>:</td>
                <td colspan="4">
                    <asp:TextBox ID="tbRemark" runat="server" CssClass="TextBox" ValidationGroup="Input" Width="225px" />                              
                </td>
            </tr>
        </table>
       
        
         
       
        <br />
        <br />      
      <asp:Menu
            ID="Menu1" runat="server" CssClass = "Menu"        
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
        <%--<div style="font-size:medium; color:Blue;">
            Detail</div>
        <hr style="color:Blue" />--%>
        <asp:Panel ID="pnlDt" runat="server">
            <%--<asp:Button ID="btnAddDt" runat="server" class="bitbtn btnadd" Text="Add" 
                ValidationGroup="Input" />--%>
            <div style="border:0px  solid; width:100%; height:100%; overflow:auto;">
                <asp:GridView ID="GridDt" runat="server" AutoGenerateColumns="false" 
                    ShowFooter="True">
                    <HeaderStyle CssClass="GridHeader" />
                    <RowStyle CssClass="GridItem" Wrap="false" />
                    <AlternatingRowStyle CssClass="GridAltItem" />
                    <PagerStyle CssClass="GridPager" />
                    <Columns>
                        <asp:TemplateField HeaderText="Action">
                            <ItemTemplate>
                                <asp:Button ID="btnEdit" runat="server" class="bitbtn btnedit" CommandName="Edit" Text="Edit" />
                                <asp:Button ID="btnDelete" runat="server" class="bitbtndt btndelete" CommandName="Delete" 
                                    OnClientClick="return confirm('Sure to delete this data?');" Text="Delete" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="SJNo" HeaderStyle-Width="50px" HeaderText="DN No" />
                        <asp:BoundField DataField="SONo" HeaderStyle-Width="50px" HeaderText="SO No" />
                        <asp:BoundField DataField="Product" HeaderStyle-Width="80px" HeaderText="Product" />
                        <asp:BoundField DataField="Product_Name" HeaderStyle-Width="150px" HeaderText="Product Name" />
                        <asp:BoundField DataField="Qty" DataFormatString="{0:#,##0.####}" HeaderStyle-Width="80px" HeaderText="Qty" ItemStyle-HorizontalAlign="Right" />
                        <asp:BoundField DataField="Unit" HeaderStyle-Width="80px" HeaderText="Unit" />
                        <asp:BoundField DataField="QtyM2" DataFormatString="{0:#,##0.####}" HeaderStyle-Width="80px" HeaderText="Qty M2" ItemStyle-HorizontalAlign="Right" />
                        <asp:BoundField DataField="QtyRoll" DataFormatString="{0:#,##0.####}" HeaderStyle-Width="80px" HeaderText="Qty Roll" ItemStyle-HorizontalAlign="Right" />
                        <asp:BoundField DataField="PriceForex" DataFormatString="{0:#,##0.########}" HeaderStyle-Width="80px" HeaderText="Price Forex" ItemStyle-HorizontalAlign="Right" />
                        <asp:BoundField DataField="AmountForex" HeaderStyle-Width="80px" HeaderText="Amount Forex" ItemStyle-HorizontalAlign="Right" />
                        <asp:BoundField DataField="Disc" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Right"
                            HeaderText="Discount" />
                        <asp:BoundField DataField="DiscForex" HeaderStyle-Width="80px" 
                            HeaderText="Discount Forex" ItemStyle-HorizontalAlign="Right" />
                        <asp:BoundField DataField="NettoForex" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Right" 
                            HeaderText="Netto Forex" />
                        <asp:BoundField DataField="CommissionForex" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Right"                             HeaderText="Commision" />
                        <asp:BoundField DataField="CommissionUnit" HeaderStyle-Width="80px" HeaderText="Commision Unit" />
                        <asp:BoundField DataField="Remark" HeaderStyle-Width="200px" HeaderText="Remark" />
                    </Columns>
                </asp:GridView>
            </div>
            <%--<asp:Button ID="btnAddDt2" runat="server" class="bitbtn btnadd" Text="Add" 
                ValidationGroup="Input" />--%></asp:Panel>
        <asp:Panel ID="pnlEditDt" runat="server" Visible="false">
            <table>
                <tr>
                    <td>DN No - SO No</td>
                    <td>:</td>
                    <td><asp:TextBox ID="tbSJNo" runat="server" CssClass="TextBox" Enabled="false" />
                        <asp:TextBox ID="tbSONo" runat="server" CssClass="TextBox" Enabled="false" />
                        <asp:Button ID="btnSJNo" runat="server" class="btngo" Text="..." ValidationGroup="Input" />
                    </td>
                </tr>
                <tr>
                    <td>Product</td>
                    <td>:</td>
                    <td><asp:TextBox ID="tbProductCode" runat="server" CssClass="TextBox" Enabled="false" Width="130px" />
                        <asp:TextBox ID="tbProductName" runat="server" CssClass="TextBox" Enabled="false" Width="250px" />
                    </td>
                </tr>
                <tr>
                    <td>Qty</td>
                    <td>:</td>
                    <td>
                        <table>
                            <tr style="background-color:Silver;text-align:center">
                                <td>Qty</td>
                                <td>Unit</td>
                                <td></td>
                                <td> </td>
                                <td>Unit Wrhs</td>
                            </tr>
                            <tr>
                                <td><asp:TextBox ID="tbQty" runat="server" CssClass="TextBoxR" AutoPostBack="true" 
                                        Enabled="False"/></td>
                                <td><asp:TextBox ID="tbUnit" runat="server" CssClass="TextBoxR" Width="30px" 
                                        Enabled="False" /></td>
                                <td><asp:TextBox ID="tbQtyM2" runat="server" CssClass="TextBoxR" visible ="False" Enabled="False" /></td>
                                <td><asp:TextBox ID="tbQtyRoll" runat="server" visible ="False" CssClass="TextBoxR" 
                                        Enabled="False" /></td>
                                <td>
                                    <asp:DropDownList ID="ddlUnitWrhsDt" runat="server" CssClass="DropDownList" 
                                        Enabled="False">
                                        <asp:ListItem>Warehouse</asp:ListItem>
                                        <asp:ListItem>M2</asp:ListItem>
                                        <asp:ListItem>Roll</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
               <tr>
            <td>Amount</td>
            <td>:</td>
            <td>
                <table>
                    <tr style="background-color:Silver;text-align:center">
                        <td>Price</td>                        
                        <td>Amount</td>
                        <td>Disc (%)</td>
                        <td>Disc Forex</td>                                                
                        <td>Total Forex</td>
                    </tr>
                    <tr>
                        <td><asp:TextBox ID="tbPriceForex" ValidationGroup="Input" runat="server" Width="120px" CssClass="TextBox" AutoPostBack="True"/></td>                        
                        <td><asp:TextBox ID="tbAmountForex" ValidationGroup="Input" runat="server" Width="120px" CssClass="TextBoxR"/></td>
                        <td><asp:TextBox ID="tbDisc" ValidationGroup="Input" runat="server" Width="40px" CssClass="TextBox" AutoPostBack="True"/></td>
                        <td><asp:TextBox ID="tbDiscForexDt" ValidationGroup="Input" runat="server" Width="120px" CssClass="TextBox" AutoPostBack="True"/></td>                                                
                        <td><asp:TextBox ID="tbTotalForexDt" ValidationGroup="Input" runat="server" Width="120px" CssClass="TextBoxR" /></td>
                    </tr>
                </table>
            </td>                
        </tr> 
          <tr>
                    <td>Commision</td>
                    <td>:</td>
                    <td><asp:TextBox ID="tbcommision" runat="server" CssClass="TextBox" ValidationGroup="Input" Width="120px" />
                        <asp:DropDownList ID="ddlUnitCommision" runat="server" AutoPostBack="true" CssClass="DropDownList" ValidationGroup="Input" />
                    </td>
                </tr>
                <tr>
                    <td>Remark</td>
                    <td>:</td>
                    <td><asp:TextBox ID="tbRemarkDt" runat="server" CssClass="TextBox" Width="225px" /></td>
                </tr>
            </table>
            <br />
            <asp:Button ID="btnSaveDt" runat="server" class="bitbtndt btnsave" Text="Save" />
            &nbsp;
            <asp:Button ID="btnCancelDt" runat="server" class="bitbtndt btncancel" Text="Cancel" />
        </asp:Panel>
        </asp:View>           
       <asp:View ID="Tab2" runat="server">  
            <asp:Panel ID="pnlDt2" runat="server">      
                <asp:Button class="bitbtn btnadd" runat="server" ID="btnAddDP2" Text="Add" ValidationGroup="Input" />	          
                
                <div style="border:0px  solid; width:100%; height:100%; overflow:auto;">
                    <asp:GridView ID="GridDt2" runat="server" AutoGenerateColumns="false" 
                        ShowFooter="True">
                        <HeaderStyle CssClass="GridHeader" />
                        <RowStyle CssClass="GridItem" Wrap="false" />
                        <AlternatingRowStyle CssClass="GridAltItem" />
                        <PagerStyle CssClass="GridPager" />
                        <Columns>
                            <asp:TemplateField HeaderText="Action"><ItemTemplate>                            
                            			<asp:Button ID="btnEdit" runat="server" class="bitbtndt btnedit" Text="Edit" CommandName="Edit"/>
								        <asp:Button ID="btnDelete" runat="server" class="bitbtndt btndelete" Text="Delete" OnClientClick="return confirm('Sure to delete this data?');" CommandName="Delete"/>									
                                        </ItemTemplate><EditItemTemplate>
                                        <asp:Button ID="btnUpdate" runat="server" class="bitbtndt btnsave" Text="Save" CommandName="Update"/>									
                                        <asp:Button ID="btnCancel" runat="server" class="bitbtndt btncancel" Text="Cancel" CommandName="Cancel"/>									
                                        </EditItemTemplate></asp:TemplateField>                                        
                            <asp:BoundField DataField="DPNo" HeaderStyle-Width="120px" HeaderText="DP No" />
                            <asp:BoundField DataField="Currency" HeaderStyle-Width="50px" HeaderText="Currency" />
                            <%--<asp:BoundField DataField="ForexRate" HeaderStyle-Width="50px" HeaderText="Rate" />                            --%>
                            <asp:BoundField DataField="BaseForex" HeaderStyle-Width="90px" HeaderText="Base Forex" />
                            <%--<asp:BoundField DataField="PPNForex" HeaderStyle-Width="90px" HeaderText="PPN Forex" />
                            <asp:BoundField DataField="TotalForex" HeaderStyle-Width="90px" HeaderText="Total Forex" />--%>
                            <asp:BoundField DataField="DPInvoice" HeaderStyle-Width="90px" HeaderText="DP Invoice"/>
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
                        <td colspan="3">
                            <table>
                                <tr style="background-color:Silver;text-align:center">
                                    <td>Currency</td>                                    
                                    <td>PPn %</td> 
                                    <%--<td>Rate</td>--%>
                                    <%--<td>PPn Rate</td>--%>    
                                                                                                       
                                </tr>
                                <tr>
                                    <td><asp:TextBox CssClass="TextBox" ID="tbDPCurrency" runat="server" Enabled="false" Width="60px"/></td>
                                    <td><asp:TextBox CssClass="TextBox" runat="server" ID="tbDPPPnPercent" Enabled="false" Width="50px"/></td>                                                                        
                                    <td><asp:TextBox CssClass="TextBox" runat="server" ID="tbDPRate" Enabled="false" Width="65px" AutoPostBack="true" Visible="false" /></td>
                                    <td><asp:TextBox CssClass="TextBox" runat="server" ID="tbDPPPnRate" Enabled="false" Width="65px" Visible="false" /></td>                                    
                                    
                                </tr>
                            </table>
                        </td>                                                
                    </tr>   
                    <tr>
                        <td>Nominal</td>
                        <td>:</td>
                        <td colspan="5">
                            <table>
                                <tr style="background-color:Silver;text-align:center">
                                    <td style="width:95px;">Base Forex</td>                                    
                                    <%--<td style="width:95px;">PPn Forex</td>                                                                  
                                    <td style="width:95px;">Total Forex</td>   --%>                                 
                                </tr>
                             </table>
                        </td>
                     </tr>        
                     <tr>
                        <td>DP</td>
                        <td>:</td>
                        <td colspan="5">
                            <table>
                                <tr>
                                    <td style="width:90px;"><asp:TextBox CssClass="TextBox" runat="server" ID="tbDPBase" Width="90px" Enabled="false" /></td>
                                    <td style="width:90px;"><asp:TextBox CssClass="TextBox" runat="server" ID="tbDPPPn" Width="90px" Enabled="false" Visible="false" /></td>
                                    <td style="width:90px;"><asp:TextBox CssClass="TextBox" runat="server" ID="tbDPTotal" Width="90px" Enabled="false" Visible="false" /></td>
                                </tr>
                             </table>
                        </td>
                     </tr>        
                     <tr>
                        <td>Paid</td>
                        <td>:</td>
                        <td colspan="5">
                            <table>
                                <tr>
                                    <td style="width:90px;"><asp:TextBox CssClass="TextBox" runat="server" ID="tbPaidBase" Width="90px" Enabled="false"/></td>
                                    <td style="width:90px;"><asp:TextBox CssClass="TextBox" runat="server" ID="tbPaidPPN" Width="90px" Enabled="false" Visible="false" /></td>                                                                                                      
                                    <td style="width:90px;"><asp:TextBox CssClass="TextBox" runat="server" ID="tbPaidTotal" Width="90px" Enabled="false" Visible="false" /></td>
                                </tr>
                             </table>
                        </td>
                     </tr>        
                     <tr>
                        <td>To Be Paid</td>
                        <td>:</td>
                        <td colspan="5">
                            <table>
                                <tr>
                                    <td style="width:90px;"><asp:TextBox CssClass="TextBox" runat="server" ID="tbDPBaseForex" Width="90px" AutoPostBack="true"/></td>
                                    <td style="width:90px;"><asp:TextBox CssClass="TextBox" runat="server" ID="tbDPPPnForex" Width="90px" Enabled="false" Visible="false" /></td>
                                    <td style="width:90px;"><asp:TextBox CssClass="TextBoxR" runat="server" ID="tbDPTotalForex" Width="90px" Visible="false" /></td>
                                    
                                </tr>
                             </table>
                        </td>
                    </tr> 
                    
                    <tr>
                        <td>DP Invoice</td>
                        <td>:</td>
                        <td colspan="5">
                            <table>
                                <tr>  
                                    <td style="width: 90px;">
                                        <asp:TextBox CssClass="TextBox" runat="server" ID="tbDPDPInvoice" Width="90px" AutoPostBack="true"/>
                                    </td>
                                </tr>
                             </table>
                        </td>
                    </tr>                                      
                    <tr>
                        <td>Remark </td>
                        <td>:</td>
                        <td colspan="5"><asp:TextBox runat="server" ID="tbDPRemarkDt" CssClass="TextBoxMulti" Width="365px" 
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
        <br />
        <asp:Button ID="btnSaveAll" runat="server" class="bitbtndt btnsave" 
            Text="Save &amp; New" ValidationGroup="Input" Width="97px" />
        &nbsp;    
        <asp:Button ID="btnSaveTrans" runat="server" class="bitbtndt btnsave" Text="Save" ValidationGroup="Input"/> &nbsp;    
        <asp:Button ID="btnBack" runat="server" class="bitbtndt btncancel" Text="Cancel" ValidationGroup="Input"/>  &nbsp;
        <asp:Button ID="btnHome" runat="server" class="bitbtndt btncancel" Text="Home"/>   
    </asp:Panel>
    <asp:Panel runat="server" ID="pnlPrint" Visible="false">
    <cr:crystalreportviewer ID="CrystalReportViewer1" runat="server" AutoDataBind="True"
               Height="1036px" Width="928px" />
    </asp:Panel>
      <br />            
    </div>
   
    <asp:Label runat ="server" ID="lbStatus" ForeColor="Red"/>
    </form>
    </body>
</html>
