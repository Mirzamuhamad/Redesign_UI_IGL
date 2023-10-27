<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TrPOService.aspx.vb" Inherits="Transaction_TrPOService_TrPOService" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="BasicFrame.WebControls.BasicDatePicker" Namespace="BasicFrame.WebControls"
    TagPrefix="BDP" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
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
            try {
         
             document.getElementById("tbBaseForex").value = setdigit(document.getElementById("tbBaseForex").value.replace(/\$|\,/g, ""), '<%=ViewState("DigitCurr")%>');
             document.getElementById("tbDiscForex").value = setdigit(document.getElementById("tbDiscForex").value.replace(/\$|\,/g, "") , '<%=ViewState("DigitCurr")%>');
             if (prmchange == "DP") {
                 document.getElementById("tbDP").value = setdigit(document.getElementById("tbDP").value.replace(/\$|\,/g, ""), '<%=ViewState("DigitPercent")%>');
                 document.getElementById("tbDPForex").value = setdigit((document.getElementById("tbBaseForex").value.replace(/\$|\,/g, "") - document.getElementById("tbDiscForex").value.replace(/\$|\,/g, "")) * document.getElementById("tbDP").value.replace(/\$|\,/g, "") / 100, '<%=ViewState("DigitQty")%>');
             }
             if (prmchange == "DPForex") {
                 document.getElementById("tbDPForex").value = setdigit(document.getElementById("tbDPForex").value.replace(/\$|\,/g, ""), '<%=ViewState("DigitCurr")%>');
                 document.getElementById("tbDP").value = setdigit(document.getElementById("tbDPForex").value.replace(/\$|\,/g, "") / (document.getElementById("tbBaseForex").value.replace(/\$|\,/g, "") - document.getElementById("tbDiscForex").value.replace(/\$|\,/g, "")) * 100, '<%=ViewState("DigitPercent")%>');
             }

             document.getElementById("tbPPN").value = setdigit(document.getElementById("tbPPN").value.replace(/\$|\,/g, ""));
             document.getElementById("tbPPNForex").value = setdigit((document.getElementById("tbBaseForex").value.replace(/\$|\,/g, "") - document.getElementById("tbDiscForex").value.replace(/\$|\,/g, "")) * document.getElementById("tbPPN").value.replace(/\$|\,/g, "") / 100, '<%=ViewState("DigitPercent")%>');
             document.getElementById("tbOtherForex").value = setdigit(document.getElementById("tbOtherForex").value.replace(/\$|\,/g, ""), '<%=ViewState("DigitQty")%>');
             document.getElementById("tbPPHForex").value = setdigit(document.getElementById("tbPPHForex").value.replace(/\$|\,/g, "")  , '<%=ViewState("DigitQty")%>');
             document.getElementById("tbTotalForex").value = parseFloat(document.getElementById("tbBaseForex").value.replace(/\$|\,/g, "")) - parseFloat(document.getElementById("tbDiscForex").value.replace(/\$|\,/g, "")) + parseFloat(document.getElementById("tbPPNForex").value.replace(/\$|\,/g, "")) - parseFloat(document.getElementById("tbPPHForex").value.replace(/\$|\,/g, "")) + parseFloat(document.getElementById("tbOtherForex").value.replace(/\$|\,/g, ""));
             document.getElementById("tbTotalForex").value = setdigit(document.getElementById("tbTotalForex").value.replace(/\$|\,/g, ""), '<%=ViewState("DigitQty")%>');
       
            
        }catch (err){
            alert(err.description);
          }      
        } 
        
        function setformatdt()
        {
         try
         {           
            document.getElementById("tbAmountForexDt").value = parseFloat(document.getElementById("tbQtyOrderDt").value.replace(/\$|\,/g,"")) * parseFloat(document.getElementById("tbPriceForexDt").value.replace(/\$|\,/g,""));
            
            document.getElementById("tbDiscForexDt").value = parseFloat(document.getElementById("tbAmountForexDt").value.replace(/\$|\,/g,"")) * parseFloat(document.getElementById("tbDiscDt").value.replace(/\$|\,/g,"") / 100);
            document.getElementById("tbPPHForexDt").value = (parseFloat(document.getElementById("tbAmountForexDt").value.replace(/\$|\,/g,"")) - parseFloat(document.getElementById("tbDiscForexDt").value.replace(/\$|\,/g,""))) * parseFloat(document.getElementById("tbPPHDt").value.replace(/\$|\,/g,"")) / 100;
            
            document.getElementById("tbTotalForexDt").value = parseFloat(document.getElementById("tbAmountForexDt").value.replace(",","")) - parseFloat(document.getElementById("tbDiscForexDt").value.replace(/\$|\,/g,""));

            document.getElementById("tbQtyOrderDt").value = setdigit(document.getElementById("tbQtyOrderDt").value.replace(/\$|\,/g, ""), '<%=ViewState("DigitQty")%>');
            document.getElementById("tbQtyWrhsDt").value = setdigit(document.getElementById("tbQtyWrhsDt").value.replace(/\$|\,/g, ""), '<%=ViewState("DigitQty")%>');
            document.getElementById("tbPriceForexDt").value = setdigit(document.getElementById("tbPriceForexDt").value.replace(/\$|\,/g,""),'<%=ViewState("DigitCurr")%>');            
            document.getElementById("tbAmountForexDt").value = setdigit(document.getElementById("tbAmountForexDt").value.replace(/\$|\,/g,""),'<%=ViewState("DigitCurr")%>');
            document.getElementById("tbDiscDt").value = setdigit(document.getElementById("tbDiscDt").value.replace(/\$|\,/g, ""), '<%=ViewState("DigitQty")%>');
            document.getElementById("tbDiscForexDt").value = setdigit(document.getElementById("tbDiscForexDt").value.replace(/\$|\,/g,""),'<%=ViewState("DigitCurr")%>');
            document.getElementById("tbPPHDt").value = setdigit(document.getElementById("tbPPHDt").value.replace(/\$|\,/g, ""), '<%=ViewState("DigitQty")%>');
            document.getElementById("tbPPHForexDt").value = setdigit(document.getElementById("tbPPHForexDt").value.replace(/\$|\,/g, "") * '<%=ViewState("FactorRate")%>', '<%=ViewState("DigitCurr")%>');
            document.getElementById("tbTotalForexDt").value = setdigit(document.getElementById("tbTotalForexDt").value.replace(/\$|\,/g,""),'<%=ViewState("DigitCurr")%>');            
            
        }catch (err){
            alert(err.description);
          }      
        }    
        
        function setformatdt2()
        {
         try
         {           
            document.getElementById("tbQtyOrderDt2").value = setdigit(document.getElementById("tbQtyOrderDt2").value.replace(/\$|\,/g,""));
        }catch (err){
            alert(err.description);
          }      
        }   
        
        function setformatdtpr()
        {
         try
         {           
            document.getElementById("tbQtyPR").value = setdigit(document.getElementById("tbQtyPR").value.replace(/\$|\,/g,""));
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
    <div class="H1"><asp:Label runat="server" ID="lblJudul" Text="PO Service"/></div>
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
                      <asp:ListItem Value="ShipmentType">Shipment Type</asp:ListItem>
                      <asp:ListItem Value="SuppContractNo">Supp. Contract No</asp:ListItem>
                      <asp:ListItem Value="CustContractNo">Cust. Contract No</asp:ListItem>
                      <asp:ListItem>Supplier</asp:ListItem>
                      <asp:ListItem Value="SupplierName">Supplier Name</asp:ListItem>
                      <asp:ListItem>Attn</asp:ListItem>
                      <asp:ListItem>Term</asp:ListItem>
                      <asp:ListItem>Delivery</asp:ListItem>
                      <%--<asp:ListItem Value="DeliveryName">Delivery Name</asp:ListItem>--%>
                      <asp:ListItem Value="FgPriceIncludeTax">Price Include PPN</asp:ListItem>
                      <asp:ListItem>Currency</asp:ListItem>
                      <asp:ListItem>Remark</asp:ListItem>
                    </asp:DropDownList>
                    <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlRange"></asp:DropDownList>
                  <asp:Button class="bitbtn btnsearch" runat="server" ID="btnSearch" Text="Search" />
                  <asp:Button class="bitbtn btngo" runat="server" ID="btnExpand" Text="..."/>
            </td>
            <td>
                <asp:LinkButton ID="LbAdvSearch" runat="server" Text="Advanced Search" />
                &nbsp &nbsp &nbsp &nbsp
                <asp:Label runat="server" ID="Label1" Text="Outstanding PR  : "/>
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
                      <asp:ListItem Value="ShipmentType">Shipment Type</asp:ListItem>
                      <asp:ListItem Value="SuppContractNo">Supp. Contract No</asp:ListItem>
                      <asp:ListItem Value="CustContractNo">Cust. ContractNo</asp:ListItem>
                      <asp:ListItem>Supplier</asp:ListItem>
                      <asp:ListItem Value="SupplierName">Supplier Name</asp:ListItem>
                      <asp:ListItem>Attn</asp:ListItem>
                      <asp:ListItem>Term</asp:ListItem>
                      <asp:ListItem>Delivery</asp:ListItem>
                      <%--<asp:ListItem Value="DeliveryName">Delivery Name</asp:ListItem>--%>
                      <asp:ListItem Value="FgPriceIncludeTax">Price Include PPN</asp:ListItem>
                      <asp:ListItem>Currency</asp:ListItem>
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
                  <asp:BoundField DataField="DepartmentName" SortExpression="DepartmentName" HeaderText="Department" />
                  <asp:BoundField DataField="PRNo" SortExpression="PRNO" HeaderText="PR No" />
                  <asp:BoundField DataField="Supplier" SortExpression="Supplier" HeaderText="Supplier" />
                  <asp:BoundField DataField="SupplierName" SortExpression="SupplierName" HeaderText="Supplier Name" />
                  <asp:BoundField DataField="Attn" SortExpression="Attn" HeaderText="Attn" />
                  <asp:BoundField DataField="Term" SortExpression="Term" HeaderText="Term" />
                  <asp:BoundField DataField="ShipmentType" SortExpression="ShipmentType" HeaderText="Shipment Type" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width = "60px" />
                  <asp:BoundField DataField="CustContractNo" SortExpression="CustContractNo" HeaderText="Cust Contract No" />
                  <asp:BoundField DataField="Delivery" SortExpression="Delivery" HeaderText="Delivery" />
                  <%--<asp:BoundField DataField="DeliveryName" SortExpression="DeliveryName" HeaderText="Delivery Name" />--%>
                  <asp:BoundField DataField="Currency" SortExpression="Currency" HeaderText="Currency" />
                  <%--<asp:BoundField DataField="ForexRate" ItemStyle-HorizontalAlign="Right" SortExpression="ForexRate" HeaderText="Forex Rate" />--%>
                  <asp:BoundField DataField="BaseForex" DataFormatString="{0:#,##0.00}" ItemStyle-HorizontalAlign="Right" SortExpression="BaseForex" HeaderText="Base Forex" />
                  <%--<asp:BoundField DataField="Disc" ItemStyle-HorizontalAlign="Right" SortExpression="Disc" HeaderText="Disc" />--%>
                  <asp:BoundField DataField="DiscForex" DataFormatString="{0:#,##0.00}" ItemStyle-HorizontalAlign="Right" SortExpression="DiscForex" HeaderText="Disc Forex" />
                  <asp:BoundField DataField="DP" DataFormatString="{0:#,##0.####}" ItemStyle-HorizontalAlign="Right" SortExpression="DP" HeaderText="DP" />
                  <asp:BoundField DataField="DPForex" DataFormatString="{0:#,##0.00}" ItemStyle-HorizontalAlign="Right" SortExpression="DPForex" HeaderText="DP Forex" />
                  <asp:BoundField DataField="PPn" DataFormatString="{0:#,##0.####}" ItemStyle-HorizontalAlign="Right" SortExpression="PPn" HeaderText="PPN" />
                  <asp:BoundField DataField="PPNForex" DataFormatString="{0:#,##0.00}" ItemStyle-HorizontalAlign="Right" SortExpression="PPNForex" HeaderText="PPN Forex" />
                  <asp:BoundField DataField="OtherForex" DataFormatString="{0:#,##0.00}" ItemStyle-HorizontalAlign="Right" SortExpression="OtherForex" HeaderText="Other Forex" />
                  <asp:BoundField DataField="TotalForex" DataFormatString="{0:#,##0.00}" ItemStyle-HorizontalAlign="Right" SortExpression="TotalForex" HeaderText="Total Forex" />
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
            <td colspan="3"><asp:TextBox CssClass="TextBoxR" ValidationGroup="Input" runat="server" ReadOnly="true" ID="tbCode" Width="150px"/>
                <asp:Label runat="server" ID="Label6" Text=" / "></asp:Label>
                <asp:Label runat="server" ID="lbRevisi"></asp:Label>
                &nbsp &nbsp Report : &nbsp
                <asp:DropDownList ID="ddlReport" runat="server" AutoPostBack="true" 
                    CssClass="DropDownList" Enabled="true" Width="65px">
                    <asp:ListItem Selected="True">Choose One</asp:ListItem>
                    <asp:ListItem>Y</asp:ListItem>
                    <asp:ListItem>N</asp:ListItem>
                </asp:DropDownList>
                <asp:Label ID="Label3" runat="server" ForeColor="Red">*</asp:Label>
            </td>           
            
            <td>PO Date</td>
            <td>:</td>
            <td colspan="3">
            <BDP:BasicDatePicker ID="tbDate" runat="server" DateFormat="dd MMM yyyy" 
                        ReadOnly = "true" ShowNoneButton="false"
                        ButtonImageHeight="19px" ButtonImageWidth="20px" 
                        DisplayType="TextBoxAndImage" 
                        TextBoxStyle-CssClass="TextDate"><TextBoxStyle CssClass="TextDate" /></BDP:BasicDatePicker>                
                
            </td>            
        </tr>      
          <tr>
              <td>
                  PR No</td>
              <td>
                  :</td>
              <td colspan="3">
                  <asp:TextBox ID="tbPRNo" runat="server" CssClass="TextBox" Enabled="False" 
                      Width="207px" />
                  <asp:Button ID="btnPRNo" runat="server" ValidationGroup="Input" class="bitbtn btngo" Text="..." />
                  <asp:Label ID="lbred1" runat="server" ForeColor="Red">*</asp:Label>
              </td>
              <td>
                  MO No</td>
              <td>
                  :</td>
              <td>
                  <asp:TextBox ID="tbMONo" runat="server" CssClass="TextBox" Enabled="False" 
                      Width="207px" />
              </td>
          </tr>
          <tr>
              <td>
                  Equipment</td>
              <td>
                  :</td>
              <td colspan="3">
                  <asp:TextBox ID="tbEquip" runat="server" CssClass="TextBox" 
                      Enabled="false" Width="80px" />
                  <asp:TextBox ID="tbEquipName" runat="server" CssClass="TextBox" Enabled="False" 
                      Width="207px" />
                  <asp:Button ID="btnEquip" Visible="false" runat="server" class="bitbtn btngo" Text="..." />
              </td>
              <td>
                  Req. Service Date</td>
              <td>
                  :</td>
              <td>
                  <BDP:BasicDatePicker ID="tbServiceDate" runat="server" ButtonImageHeight="19px" 
                      ButtonImageWidth="20px" DateFormat="dd MMM yyyy" DisplayType="TextBoxAndImage" 
                      ReadOnly="true" ShowNoneButton="false" TextBoxStyle-CssClass="TextDate">
                      <TextBoxStyle CssClass="TextDate" />
                  </BDP:BasicDatePicker>
              </td>
          </tr>
        <tr>
            <td><asp:LinkButton ID="lbSupplier" runat="server" Text="Supplier"/></td>
            <td>:</td>
            <td colspan="3" >
                <asp:TextBox ID="tbSuppCode" ValidationGroup="Input" AutoPostBack="true" Width="80px" runat="server" CssClass="TextBox" />
                <asp:TextBox ID="tbSuppName" Width="207px" runat="server" Enabled="False" 
                    CssClass="TextBox" />
                <asp:Button class="bitbtn btngo" runat="server" ID="btnSupp" Text="..."/>
                <asp:Label ID="Label4" runat="server" ForeColor="Red">*</asp:Label>
            </td>
            <td>Attn</td>
            <td>:</td>
            <td>
                <asp:TextBox ID="tbAttn" Width="200px" runat="server" ValidationGroup="Input"
                    CssClass="TextBox" MaxLength="255" />
            </td>            
        </tr> 
        <tr>            
            <td>Department</td>
            <td>:</td>
            <td colspan="3">
                <asp:DropDownList runat="server" ID="ddlDept" CssClass="DropDownList" 
                    ValidationGroup="input">
                </asp:DropDownList>
                <asp:Label ID="Label7" runat="server" ForeColor="Red">*</asp:Label>
            </td>
            <td>Supp & Cust. Contract No</td>
            <td><asp:Label ID="Label8" runat="server" Text=":" /></td>            
            <td>
                <asp:TextBox ID="tbSuppPONo" Width="135px" runat="server" ValidationGroup="Input" CssClass="TextBox" MaxLength="255" />
                <asp:TextBox ID="tbCustContractNo" Width="135px" runat="server" 
                    ValidationGroup="Input" CssClass="TextBox" MaxLength="255" />
            </td>
        </tr>   
               
        <tr>
            <td>Term</td>
            <td>:</td>
            <td colspan="3"><asp:DropDownList ID="ddlTerm" runat="server" 
                    CssClass="DropDownList" ValidationGroup="Input" Height="16px" 
                    Width="147px" AutoPostBack="True" />
                    &nbsp
                    <asp:Button class="bitbtn btngo" ValidationGroup="Input" runat="server" 
                    ID="btnGetData" Text="Get Data" Visible="false" Width="74px"/>
            </td>            
            
                    
            <td>Term Payment</td>
            <td>:</td>
            <td>
                <asp:TextBox ID="tbTOPRemark" runat="server" CssClass="TextBox" MaxLength="255" 
                    ValidationGroup="Input" Width="196px" />
            </td>            
        
        </tr>        
       <tr>
            <td>
                Delivery Place</td>
            <td>:</td>
            <td colspan="3"><asp:DropDownList ID="ddlDelivery" runat="server" 
                    CssClass="DropDownList" ValidationGroup="Input" Height="16px" 
                    Width="144px" AutoPostBack="True"></asp:DropDownList>
                    &nbsp &nbsp Add Cost : &nbsp            
                <asp:DropDownList ID="ddlfgAdditional" ValidationGroup="Input" 
                                AutoPostBack="true" runat="server" CssClass="DropDownList" >
                            <asp:ListItem>Y</asp:ListItem>
                            <asp:ListItem>N</asp:ListItem>
                            </asp:DropDownList>
            </td>
                        
            <td>Shipment Type</td>
            <td>:</td>
            <td><asp:DropDownList ID="ddlShipmentType" runat="server" CssClass="DropDownList" ValidationGroup="Input" Width="180px" />                    
                <asp:Label ID="Label5" runat="server" ForeColor="Red">*</asp:Label>            
            </td>
        </tr>      
          <tr>
              <td>
                  Delivery Addr</td>
              <td>
                  :</td>
              <td>
                  <asp:TextBox ID="tbDeliveryAddr" runat="server" CssClass="TextBoxMulti" 
                      MaxLength="255" ValidationGroup="Input" Width="314px"  
                      TextMode="MultiLine" />
                  &nbsp;</td>
              <td>
                  &nbsp;
              </td>
              <td>
                  &nbsp;</td>
              <td>Add Cost Remark</td>
            <td>:</td>
            <td><asp:TextBox ID="tbAddCostRemark" runat="server" CssClass="TextBoxMulti" MaxLength="255" TextMode="MultiLine"
                                ValidationGroup="Input" Width="300px" />
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
                      MaxLength="255" ValidationGroup="Input" Width="200px" />
              </td>
              <td>
                  &nbsp;</td>
              <td>
                  &nbsp;</td>
                  
              <td>
                  Price Include PPN</td>
              <td>
                  :</td>
              <td>
                  <asp:DropDownList ID="ddlfgInclude" runat="server" AutoPostBack="true" 
                      CssClass="DropDownList" Enabled="true" Width="46px">
                      <asp:ListItem>Y</asp:ListItem>
                      <asp:ListItem>N</asp:ListItem>
                  </asp:DropDownList>
              </td>    
              
          </tr>
       <tr>
            <td><asp:LinkButton ID="lbCurrency"  runat="server" Text="Currency"/></td>
            <td>:</td>
            <td colspan="3"><asp:DropDownList ID="ddlCurrHd" ValidationGroup="Input" AutoPostBack="true" runat="server" CssClass="DropDownList"/>                
            </td>
            <td>DP</td>
            <td>:</td>
            <td><asp:TextBox ID="tbDP" runat="server" ValidationGroup="Input" Width="45px" CssClass="TextBox" /> % = &nbsp
               <asp:TextBox ID="tbDPForex" runat="server" ValidationGroup="Input" Width="81px" CssClass="TextBox" />
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
                        <td><asp:TextBox ID="tbBaseForex" ValidationGroup="Input" runat="server" Width="80px" CssClass="TextBoxR"/></td>
                        <td><asp:TextBox ID="tbDiscForex" ValidationGroup="Input" runat="server" Width="80px" CssClass="TextBoxR"/></td>
                        <td><asp:TextBox ID="tbPPN" ValidationGroup="Input" runat="server" Width="40px" CssClass="TextBox"/></td>
                        <td><asp:TextBox ID="tbPPNForex" ValidationGroup="Input" runat="server" Width="80px" CssClass="TextBoxR"/></td>
                        <td><asp:TextBox ID="tbPPHForex" ValidationGroup="Input" runat="server" Width="80px" CssClass="TextBoxR"/></td>
                        <td><asp:TextBox ID="tbOtherForex" ValidationGroup="Input" runat="server" Width="80px" CssClass="TextBox"/></td>
                        <td><asp:TextBox ID="tbTotalForex" ValidationGroup="Input" runat="server" Width="80px" CssClass="TextBoxR" /></td>
                    </tr>
                </table>
            </td>                
        </tr>
        
            
            <tr>
            <td>Remark</td>
            <td>:</td>
            <td colspan="6">
                <asp:TextBox Width = "281px" runat="server" ValidationGroup="Input" 
                    ID="tbRemark" CssClass="TextBoxMulti" TextMode="MultiLine" MaxLength="255" /></td>
        </tr>
      </table>  
      <div style="font-size:medium; color:Blue;">Detail</div>
      <hr style="color:Blue" />
        <asp:MultiView 
        
        ID="MultiView1"
        runat="server"
        ActiveViewIndex="1">
           <asp:View ID="Tab1" runat="server">
              <asp:Panel runat="server" ID="PnlDt">
                <div style="border:0px  solid; width:100%; height:100%; overflow:auto;">
                    <asp:Button ID="btnAddDt" runat="server" class="bitbtndt btnadd" Text="Add" />
                    <br />
                    <asp:GridView ID="GridDt" runat="server" AutoGenerateColumns="False" 
                        ShowFooter="True">
                        <HeaderStyle CssClass="GridHeader" />
                        <RowStyle CssClass="GridItem" Wrap="false" />
                        <AlternatingRowStyle CssClass="GridAltItem" />
                        <PagerStyle CssClass="GridPager" />
                        <Columns>
                            <asp:TemplateField HeaderText="Action">
                                  <ItemTemplate>
                                    <asp:Button ID="btnEdit" runat="server" class="bitbtn btnedit" Text="Edit" CommandName="Edit"/>
                                    <asp:Button ID="btnDelete" runat="server" class="bitbtn btndelete" Text="Delete" CommandName="Delete"/>
                                  </ItemTemplate>                                  
                            </asp:TemplateField>                            
                            
                            <asp:TemplateField>
                                <ItemTemplate>
                                  <asp:Button ID="btnClosing" runat="server" class="bitbtn btnclosing" Text="Closing" CommandName="Closing" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" />     
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
                            <asp:BoundField DataField="Specification2" HeaderStyle-Width="250px" 
                                HeaderText="Specification2" >
                                <HeaderStyle Width="250px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="QtyOrder" ItemStyle-HorizontalAlign="Right" 
                                HeaderStyle-Width="80px" HeaderText="Qty Order" >
                                <HeaderStyle Width="80px" />
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="UnitOrder" HeaderStyle-Width="80px" 
                                HeaderText="Unit Order" >
                                <HeaderStyle Width="80px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Qty" ItemStyle-HorizontalAlign="Right" 
                                HeaderStyle-Width="80px" HeaderText="Qty Wrhs" >
                                <HeaderStyle Width="80px" />
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Unit" HeaderStyle-Width="80px" 
                                HeaderText="Unit Wrhs" >
                                <HeaderStyle Width="80px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="PriceForex" ItemStyle-HorizontalAlign="Right" 
                                HeaderStyle-Width="80px" HeaderText="Price" >
                                <HeaderStyle Width="80px" />
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="BrutoForex" ItemStyle-HorizontalAlign="Right" 
                                HeaderStyle-Width="80px" HeaderText="Amount Forex" >
                                <HeaderStyle Width="80px" />
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Disc" ItemStyle-HorizontalAlign="Right" 
                                HeaderStyle-Width="80px" HeaderText="Disc %" >
                                <HeaderStyle Width="80px" />
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="DiscForex" ItemStyle-HorizontalAlign="Right" 
                                HeaderStyle-Width="80px" HeaderText="Disc Forex" >
                                <HeaderStyle Width="80px" />
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="PPh" ItemStyle-HorizontalAlign="Right" 
                                HeaderStyle-Width="80px" HeaderText="PPh %" >
                                <HeaderStyle Width="80px" />
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="PPhForex" ItemStyle-HorizontalAlign="Right" 
                                HeaderStyle-Width="80px" HeaderText="PPh Forex" >                        
                                <HeaderStyle Width="80px" />
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="NettoForex" ItemStyle-HorizontalAlign="Right" 
                                HeaderStyle-Width="80px" HeaderText="Total Forex" >
                                <HeaderStyle Width="80px" />
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Remark" HeaderStyle-Width="250px" 
                                HeaderText="Remark" >                            
                                <HeaderStyle Width="250px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="QtyRR" HeaderStyle-Width="80px" HeaderText="Qty RR" 
                                ItemStyle-HorizontalAlign="Right" >
                            
                                <HeaderStyle Width="80px" />
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            
                            <asp:BoundField DataField="UserClose" HeaderStyle-Width="80px" 
                                HeaderText="User Close" >
                                <HeaderStyle Width="80px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="DateClose" HeaderStyle-Width="80px" 
                                HeaderText="Date Close" >
                                <HeaderStyle Width="80px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="RemarkClose" HeaderStyle-Width="250px" 
                                HeaderText="Closing Remark" >
                                <HeaderStyle Width="250px" />
                            </asp:BoundField>
                        </Columns>
                    </asp:GridView>
                    <br />
                    <asp:Button ID="btnAddDt1" runat="server" class="bitbtndt btnadd" Text="Add" />
              </div>   
              </asp:Panel> 
              <asp:Panel runat="server" ID="pnlEditDt" Visible="false">
                <table>              
                    <tr>
                        <td>Product</td>
                        <td>:</td>
                        <td><asp:TextBox runat="server" ID="tbProductDt" CssClass="TextBox" AutoPostBack="true" />
                            <asp:TextBox runat="server"  CssClass="TextBoxR"
                                ID="tbProductNameDt" EnableTheming="True" ReadOnly="True" Enabled="False" 
                                Width="200px"/>                             
                            <asp:Button ID="btnProduct" runat="server" class="bitbtn btngo" Text="..." />
                        </td>           
                    </tr>        
                    <tr>
                        <td>Specification</td>
                        <td>:</td>
                        <td>
                            <asp:TextBox ID="tbSpecificationDt" runat="server" CssClass="TextBoxMulti" 
                                MaxLength="255" TextMode="MultiLine" Width="365px" />
                        </td>
                    </tr>                            
                    <tr>
                        <td>Specification 2</td>
                        <td>:</td>
                        <td>
                            <asp:TextBox ID="tbSpecificationDt2" runat="server" CssClass="TextBoxMulti" 
                                MaxLength="255" TextMode="MultiLine" Width="365px" />
                        </td>
                    </tr>                
                    <tr>
                        <td>Qty</td>
                        <td>:</td>
                        <td>
                            <table cellspacing="0" cellpadding="0">
                                <tr style="background-color:Silver;text-align:center">
                                    <td>Order</td>
                                    <td>Unit Order</td>
                                    <td>Wrhs</td>
                                    <td>Unit Wrhs</td>
                                              
                                </tr>
                                <tr>
                                    <td><asp:TextBox ID="tbQtyOrderDt" runat="server" CssClass="TextBox" 
                                            AutoPostBack="true" /></td>
                                    <td>
                                        <asp:DropDownList ID="ddlUnitOrderDt" runat="server" AutoPostBack="true" 
                                            CssClass="DropDownList" />
                                    </td>
                                    <td><asp:TextBox ID="tbQtyWrhsDt"  runat="server" CssClass="TextBoxR"/></td>
                                    <td>
                                        <asp:DropDownList ID="ddlUnitWrhsDt" runat="server" CssClass="DropDownList" 
                                            Enabled="False" />
                                    </td>
                                    
                                </tr>
                            </table>
                        </td>                
                    </tr>                    
                    <tr>
                        <td>Amount</td>
                        <td>:</td>
                        <td>
                            <table cellspacing="0" cellpadding="0">
                                <tr style="background-color:Silver;text-align:center">
                                    <td>
                                        Price</td>
                                    <td>
                                        Amount</td>
                                    <td>
                                        Disc %</td>
                                    <td>
                                        Disc Forex</td>
                                    <td>
                                        Total Forex</td>
                                    <td>
                                        PPH %</td>
                                    <td>
                                        PPH Forex</td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="tbPriceForexDt" runat="server" AutoPostBack="True" 
                                            CssClass="TextBox" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="tbAmountForexDt" runat="server" CssClass="TextBoxR" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="tbDiscDt" runat="server" AutoPostBack="True" 
                                            CssClass="TextBox" Width="45px" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="tbDiscForexDt" runat="server" CssClass="TextBoxR" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="tbTotalForexDt" runat="server" CssClass="TextBoxR" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="tbPPHDt" runat="server" AutoPostBack="True" CssClass="TextBox" 
                                            Width="45px" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="tbPPHForexDt" runat="server" CssClass="TextBoxR" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Remark
                        </td>
                        <td>
                            :</td>
                        <td>
                            <asp:TextBox ID="tbRemarkDt" runat="server" CssClass="TextBox" MaxLength="255" 
                                TextMode="MultiLine" Width="365px" />
                        </td>
                    </tr>
                </table>
                <br />                     
                  <asp:Button ID="btnSaveDt" runat="server" class="bitbtndt btnsave" Text="Save" 
                      validationgroup="Input" />
                  <asp:Button ID="btnCancelDt" runat="server" class="bitbtndt btncancel" 
                      Text="Cancel" />
                
           </asp:Panel> 
              
           </asp:View>
            <asp:View ID="Tab3" runat="server">
                <asp:Panel ID="pnlDt2" runat="server">
                Product : <asp:Label ID="lbProductDt2" runat="server" Text="Product oi" />
                <br />
                <asp:Button ID="btnAddDt2" runat="server" class="bitbtndt btnadd" Text="Add" />									
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
                            <asp:Button ID="btnEdit" runat="server" class="bitbtn btnedit" Text="Edit" CommandName="Edit"/>
                            <asp:Button ID="btnDelete" runat="server" class="bitbtn btndelete" Text="Delete" CommandName="Delete"/>
                            
                             </ItemTemplate> 
                             <EditItemTemplate>
                             <asp:Button  class="bitbtn btnupdatedt" runat="server" ValidationGroup="Input" ID="btnUpdate" Text="Update" /> 
                             <asp:Button  class="bitbtn btncanceldt" runat="server" ValidationGroup="Input" ID="btnCancel" Text="Cancel" />                              
                             </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Delivery" HeaderText="Delivery Date" />
                            
                            <asp:BoundField DataField="QtyOrder" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="80px" HeaderText="Qty Order" />
                            <asp:BoundField DataField="UnitOrder" HeaderStyle-Width="80px" HeaderText="Unit Order" />
                            <asp:BoundField DataField="Remark" HeaderText="Remark" />
                            <asp:BoundField DataField="Product" HeaderText="prodak" />
                        </Columns>
                    </asp:GridView>
              </div>   
              <asp:Button ID="btnAddDt2Ke2" runat="server" class="bitbtndt btnadd" Text="Add" />									
                
              </asp:Panel>
              <asp:Panel runat="server" ID="pnlEditDt2" Visible="false">
                <table>      
                    <tr>
                        <td>Delivery Date</td>
                        <td>:</td>
                        <td><BDP:BasicDatePicker ID="tbDeliveryDateDt2" runat="server" DateFormat="dd MMM yyyy" 
                            ReadOnly = "true" ShowNoneButton="false"
                            ButtonImageHeight="19px" ButtonImageWidth="20px" 
                            DisplayType="TextBoxAndImage" 
                            TextBoxStyle-CssClass="TextDate"><TextBoxStyle CssClass="TextDate" /></BDP:BasicDatePicker>
                        </td>
                    </tr>        
                    <tr>
                        <td>Qty</td>
                        <td>:</td>
                        <td>
                            <table cellspacing="0" cellpadding="0">
                                <tr style="background-color:Silver;text-align:center">
                                    <td>Order</td>
                                    <td>Unit</td>                                    
                                </tr>
                                <tr>
                                    <td><asp:TextBox CssClass="TextBox" runat="server" ID="tbQtyOrderDt2"/></td>
                                    <td><asp:DropDownList CssClass="DropDownList" ID="ddlUnitOrderDt2" runat="server"/></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>Remark </td>
                        <td>:</td>
                        <td><asp:TextBox runat="server" ID="tbRemarkDt2" CssClass="TextBox" Width="365px" 
                                MaxLength="255" TextMode="MultiLine" />                        
                        </td>
                    </tr>                                                  
                </table>
                <br />        
                  <asp:Button ID="btnSaveDt2" runat="server" class="bitbtndt btnsave" Text="Save" 
                      validationgroup="Input" />
                  <asp:Button ID="btnCancelDt2" runat="server" class="bitbtndt btncancel" 
                      Text="Cancel" />
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
    </form>
</body>
</html>
