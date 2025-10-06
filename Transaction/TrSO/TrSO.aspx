<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TrSO.aspx.vb" Inherits="Transaction_TrSO_TrSO" %>

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
             document.getElementById("tbDiscForex").value = setdigit(document.getElementById("tbDiscForex").value.replace(/\$|\,/g, ""), '<%=ViewState("DigitCurr")%>');
             if (prmchange == "DP") {
                 document.getElementById("tbDP").value = setdigit(document.getElementById("tbDP").value.replace(/\$|\,/g, ""), '<%=ViewState("DigitPercent")%>');
                 document.getElementById("tbDPForex").value = setdigit((document.getElementById("tbBaseForex").value.replace(/\$|\,/g, "") - document.getElementById("tbDiscForex").value.replace(/\$|\,/g, "")) * document.getElementById("tbDP").value.replace(/\$|\,/g, "") / 100, '<%=ViewState("DigitQty")%>');
             }
             if (prmchange == "DPForex") {
                 document.getElementById("tbDPForex").value = setdigit(document.getElementById("tbDPForex").value.replace(/\$|\,/g, ""), '<%=ViewState("DigitCurr")%>');
                 if (document.getElementById("tbBaseForex").value.replace(/\$|\,/g, "") > 0) { 
                 document.getElementById("tbDP").value = setdigit(document.getElementById("tbDPForex").value.replace(/\$|\,/g, "") / (document.getElementById("tbBaseForex").value.replace(/\$|\,/g, "") - document.getElementById("tbDiscForex").value.replace(/\$|\,/g, "")) * 100, '<%=ViewState("DigitPercent")%>'); }
             }        
            //document.getElementById("tbDiscForex").value = document.getElementById("tbBaseForex").value.replace(/\$|\,/g,"") * document.getElementById("tbDisc").value.replace(/\$|\,/g,"") / 100;
            document.getElementById("tbPPNForex").value = (document.getElementById("tbBaseForex").value.replace(/\$|\,/g,"") - document.getElementById("tbDiscForex").value.replace(/\$|\,/g,"")) * document.getElementById("tbPPN").value.replace(/\$|\,/g,"") / 100;
            //document.getElementById("tbDPForex").value = document.getElementById("tbBaseForex").value.replace(/\$|\,/g,"") * document.getElementById("tbDP").value.replace(/\$|\,/g,"") / 100;
            
            document.getElementById("tbTotalForex").value = parseFloat(document.getElementById("tbBaseForex").value.replace(/\$|\,/g,"")) - parseFloat(document.getElementById("tbDiscForex").value.replace(/\$|\,/g,"")) + parseFloat(document.getElementById("tbPPNForex").value.replace(/\$|\,/g,""));
            //document.getElementById("tbRateHd").value = setdigit(document.getElementById("tbRateHd").value.replace(/\$|\,/g, ""));
            //document.getElementById("tbDP").value = setdigit(document.getElementById("tbDP").value.replace(/\$|\,/g, ""));
            //document.getElementById("tbDPForex").value = setdigit(document.getElementById("tbDPForex").value.replace(/\$|\,/g,""),'<%=ViewState("DigitCurr")%>');
            //document.getElementById("tbBaseForex").value = setdigit(document.getElementById("tbBaseForex").value.replace(/\$|\,/g,""),'<%=ViewState("DigitCurr")%>');
            //document.getElementById("tbDisc").value = setdigit(document.getElementById("tbDisc").value.replace(/\$|\,/g, "");
            //document.getElementById("tbDiscForex").value = setdigit(document.getElementById("tbDiscForex").value.replace(/\$|\,/g,""),'<%=ViewState("DigitCurr")%>');
            document.getElementById("tbTotalForex").value = setdigit(document.getElementById("tbTotalForex").value.replace(/\$|\,/g,""),'<%=ViewState("DigitCurr")%>');
            document.getElementById("tbPPN").value = setdigit(document.getElementById("tbPPN").value.replace(/\$|\,/g, ""));
            document.getElementById("tbPPNForex").value = setdigit(document.getElementById("tbPPNForex").value.replace(/\$|\,/g,""),'<%=ViewState("DigitCurr")%>');            
        }catch (err){
            alert(err.description);
          }      
        } 
        
        function setformatdt()
        {
         try
         {
//           document.getElementById("tbDiscForexDt").value = parseFloat(document.getElementById("tbAmountForexDt").value.replace(/\$|\,/g,"")) * parseFloat(document.getElementById("tbDiscDt").value.replace(/\$|\,/g,"") / 100);
//            
//            document.getElementById("tbTotalForexDt").value = parseFloat(document.getElementById("tbAmountForexDt").value.replace(",","")) - parseFloat(document.getElementById("tbDiscForexDt").value.replace(/\$|\,/g,""));
//                     
//            document.getElementById("tbQtyM2Dt").value = setdigit(document.getElementById("tbQtyM2Dt").value.replace(/\$|\,/g,""));
//            document.getElementById("tbQtyRollDt").value = setdigit(document.getElementById("tbQtyRollDt").value.replace(/\$|\,/g,""));
//            document.getElementById("tbQtyDt").value = setdigit(document.getElementById("tbQtyDt").value.replace(/\$|\,/g,""));
//            document.getElementById("tbPriceForexDt").value = setdigit(document.getElementById("tbPriceForexDt").value.replace(/\$|\,/g,""),'<%=ViewState("DigitCurr")%>');            
//            document.getElementById("tbAmountForexDt").value = setdigit(document.getElementById("tbAmountForexDt").value.replace(/\$|\,/g,""),'<%=ViewState("DigitCurr")%>');
//            document.getElementById("tbDiscDt").value = setdigit(document.getElementById("tbDiscDt").value.replace(/\$|\,/g,""));
//            document.getElementById("tbDiscForexDt").value = setdigit(document.getElementById("tbDiscForexDt").value.replace(/\$|\,/g,""),'<%=ViewState("DigitCurr")%>');
//            document.getElementById("tbTotalForexDt").value = setdigit(document.getElementById("tbTotalForexDt").value.replace(/\$|\,/g,""),'<%=ViewState("DigitCurr")%>');            
            
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
        
        function setformatDtSub()
        {
         try
         {           
            document.getElementById("tbQtySub").value = setdigit(document.getElementById("tbQtySub").value.replace(/\$|\,/g,""));
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
        function revisi()
        {
            try
            {
                var result = prompt("Remark Revisi", "");
                if (result){
                    document.getElementById("HiddenRemarkRevisi").value = result;
                    
                } else {
                    document.getElementById("HiddenRemarkRevisi").value = "False Value";
                   
                }
               /* var LConfirm; 
                var r = confirm("Sure you want Revisi ? ");
                
                    if (r == true) {
                       LConfirm = "OK";
                    } else {
                        LConfirm= "Cancel";
                    }
                    document.getElementById("HiddenRemarkRevisi").value = LConfirm;
                    */
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
 <link href="https://fonts.googleapis.com/css2?family=Muli:wght@300;400;600&display=swap" rel="stylesheet">    
</head>
<body>
    <form id="form1" runat="server">
    <div class="Content">
    <div class="H1"><asp:Label runat="server" ID="lblJudul" Text="Sales Order"/></div>
        <hr style="color:Blue" />
        <asp:Panel runat="server" id="PnlHd"> 
      <table>
        <tr>
            <td style="width:100px;text-align:right">Quick Search :</td>            
            <td><asp:TextBox CssClass="TextBox" runat="server" ID ="tbFilter"/> 
                  <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlField" >
                      <asp:ListItem Value ="TransNmbr" Selected="True">SO No</asp:ListItem>
                      <asp:ListItem>Revisi</asp:ListItem>
                      <asp:ListItem>Status</asp:ListItem>
                      <asp:ListItem Value="dbo.FormatDate(TransDate)">Date</asp:ListItem>
                      <asp:ListItem>Status</asp:ListItem>
                      <asp:ListItem Value="SOKind">SO Kind</asp:ListItem>
                      <asp:ListItem Value="SOType">SO Type</asp:ListItem>
                      <asp:ListItem Value="ProductGroup">Product Group</asp:ListItem>
                      <asp:ListItem Value="ProductGroupName">Product Group Name</asp:ListItem>
                      <asp:ListItem>Customer</asp:ListItem>
                      <asp:ListItem Value="CustomerName">Customer Name</asp:ListItem>
                      <asp:ListItem>Attn</asp:ListItem>
                      <asp:ListItem Value="Phone">Telephone</asp:ListItem>
                      <asp:ListItem Value="CustPONo">PO Cust No</asp:ListItem>
                      <asp:ListItem Value="ContractNo">Contract No</asp:ListItem>
                      <%--<asp:ListItem Value="IRRNo">IRR No</asp:ListItem>--%>
                      <asp:ListItem Value="FgNeedDelivery">Need Delivery</asp:ListItem>
                      <asp:ListItem>Delivery</asp:ListItem>
                      <asp:ListItem Value="DeliveryAddr">Address</asp:ListItem>
                      <asp:ListItem Value="DeliveryCity">City</asp:ListItem>
                      <asp:ListItem Value="DeliveryCostBy">Delivery Cost By</asp:ListItem>
                      <asp:ListItem Value="FgPriceIncludeTax">Price Include PPN</asp:ListItem>
                      <asp:ListItem>Term</asp:ListItem>
                      <asp:ListItem Value="TermRemark">Term Remark</asp:ListItem>
                      <asp:ListItem>Currency</asp:ListItem>
                      <asp:ListItem>Remark</asp:ListItem>
                      <asp:ListItem Value="RemarkRevisi">Remark Revisi</asp:ListItem>
                    </asp:DropDownList>
                    <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlRange"></asp:DropDownList>
                  <asp:Button class="bitbtn btnsearch" runat="server" ID="btnSearch" Text="Search" />
                  <asp:Button class="bitbtn btngo" runat="server" ID="btnExpand" Text="..."/>
            </td>
            <td>
                <asp:LinkButton ID="LbAdvSearch" runat="server" Text="Advanced Search" />
                &nbsp &nbsp &nbsp &nbsp
                <%--<asp:Label runat="server" ID="Label1" Text="IRR Gantung : "/>
                <asp:LinkButton runat="server" ID="lbCount" Text="X" ForeColor="#FF6600" Font-Size="Small" />
                <asp:Label runat="server" ID="Label2" Text=" record(s)"/>--%>
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
                  <asp:ListItem Value ="TransNmbr" Selected="True">SO No</asp:ListItem>
                      <asp:ListItem>Revisi</asp:ListItem>
                      <asp:ListItem>Status</asp:ListItem>
                      <asp:ListItem Value="dbo.FormatDate(TransDate)">Date</asp:ListItem>
                      <asp:ListItem Value="SOKind">SO Kind</asp:ListItem>
                      <asp:ListItem Value="SOType">SO Type</asp:ListItem>
                      <asp:ListItem Value="ProductGroup">Product Group</asp:ListItem>
                      <asp:ListItem Value="ProductGroupName">Product Group Name</asp:ListItem>
                      <asp:ListItem>Customer</asp:ListItem>
                      <asp:ListItem Value="CustomerName">Customer Name</asp:ListItem>
                      <asp:ListItem>Attn</asp:ListItem>
                      <asp:ListItem Value="Phone">Telephone</asp:ListItem>
                      <asp:ListItem Value="CustPONo">PO Cust No</asp:ListItem>
                      <asp:ListItem Value="ContractNo">Contract No</asp:ListItem>
                      <%--<asp:ListItem Value="IRRNo">IRR No</asp:ListItem>--%>
                      <asp:ListItem Value="FgNeedDelivery">Need Delivery</asp:ListItem>
                      <asp:ListItem>Delivery</asp:ListItem>
                      <asp:ListItem Value="DeliveryAddr">Address</asp:ListItem>
                      <asp:ListItem Value="DeliveryCity">City</asp:ListItem>
                      <asp:ListItem Value="DeliveryCostBy">Delivery Cost By</asp:ListItem>
                      <asp:ListItem Value="FgPriceIncludeTax">Price Include PPN</asp:ListItem>
                      <asp:ListItem>Term</asp:ListItem>
                      <asp:ListItem Value="TermRemark">Term Remark</asp:ListItem>
                      <asp:ListItem>Currency</asp:ListItem>
                      <asp:ListItem>Remark</asp:ListItem>
                      <asp:ListItem Value="RemarkRevisi">Remark Revisi</asp:ListItem>
                      <asp:ListItem Value="Product">Product</asp:ListItem>
              </asp:DropDownList>
          </td>              
        </tr>        
      </table>      
      </asp:Panel>
          <br />
            <asp:Button class="bitbtn btnadd" runat="server" ID="BtnAdd" Text="Add" ValidationGroup="Input"/>	 
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
                              <asp:ListItem Text="Revisi" />
                              <asp:ListItem Text="Print" />
                              <asp:ListItem Text="Print 2" />
                              <asp:ListItem Text="Generate DO" />
                          </asp:DropDownList>
                          <asp:Button class="bitbtn btngo" runat="server" ID="btnGo" Text="G"	 
                          CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" CommandName="Go" />
                      </ItemTemplate>
                      <HeaderStyle Width="110px" />
                  </asp:TemplateField>
                  <asp:BoundField DataField="TransNmbr" SortExpression="Nmbr" HeaderText="SO No"></asp:BoundField>                  
                  <asp:BoundField DataField="Revisi" HeaderText="Revisi"></asp:BoundField>
                  <asp:BoundField DataField="Status" HeaderText="Status"></asp:BoundField>                  
                  <asp:BoundField DataField="TransDate" HtmlEncode="true" DataFormatString="{0:dd MMM yyyy}" SortExpression="TransDate" HeaderText="Date"></asp:BoundField>
                  <asp:BoundField DataField="CustPONo" SortExpression="CustPONo" HeaderText="PO Cust No" />
                  <asp:BoundField DataField="Customer" SortExpression="Customer" HeaderText="Customer" />
                  <asp:BoundField DataField="CustomerName" SortExpression="CustomerName" HeaderText="Customer Name" />
                  <asp:BoundField DataField="Attn" SortExpression="Attn" HeaderText="Attn" />
                  <asp:BoundField DataField="Phone" SortExpression="Phone" HeaderText="Telephone" />
                  <asp:BoundField DataField="SOKind" SortExpression="SOKind" HeaderText="SO Kind" />
                  <asp:BoundField DataField="SOType" SortExpression="SOtype" HeaderText="SO Type" />
                  <asp:BoundField DataField="ProductGroup" SortExpression="ProductGroup" HeaderText="Product Group" />
                  <asp:BoundField DataField="ProductGroupName" SortExpression="ProductGroupName" HeaderText="Product Group Name" />
                  <asp:BoundField DataField="ContractNo" SortExpression="ContractNo" HeaderText="Contract No" />
                  <%--<asp:BoundField DataField="IRRNo" SortExpression="IRRNo" HeaderText="IRR No" />--%>
                  <asp:BoundField DataField="FgNeedDelivery" SortExpression="FgNeedDelivery" HeaderText="Need Delivery" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width = "60px" />
                  <asp:BoundField DataField="Term" SortExpression="Term" HeaderText="Term" />
                  <asp:BoundField DataField="TermRemark" SortExpression="TermRemark" HeaderText="TermRemark" />
                  <asp:BoundField DataField="Delivery" SortExpression="Delivery" HeaderText="Delivery" />
                  <asp:BoundField DataField="DeliveryAddr" SortExpression="DeliveryAddr" HeaderText="Address" />
                  <asp:BoundField DataField="DeliveryCity" SortExpression="DeliveryCity" HeaderText="City" />
                  <asp:BoundField DataField="Currency" SortExpression="Currency" HeaderText="Currency" />
                  <asp:BoundField DataField="BaseForex" DataFormatString="{0:#,##0.00}" ItemStyle-HorizontalAlign="Right" SortExpression="BaseForex" HeaderText="Base Forex" />
                  <asp:BoundField DataField="DiscForex" DataFormatString="{0:#,##0.00}" ItemStyle-HorizontalAlign="Right" SortExpression="DiscForex" HeaderText="Disc Forex" />
                  <asp:BoundField DataField="DP" DataFormatString="{0:#,##0.00}" ItemStyle-HorizontalAlign="Right" SortExpression="DP" HeaderText="DP" />
                  <asp:BoundField DataField="DPForex" DataFormatString="{0:#,##0.00}" ItemStyle-HorizontalAlign="Right" SortExpression="DPForex" HeaderText="DP Forex" />
                  <asp:BoundField DataField="PPn" DataFormatString="{0:#,##0.####}" ItemStyle-HorizontalAlign="Right" SortExpression="PPn" HeaderText="PPN" />
                  <asp:BoundField DataField="PPNForex" DataFormatString="{0:#,##0.00}" ItemStyle-HorizontalAlign="Right" SortExpression="PPNForex" HeaderText="PPN Forex" />
                  <asp:BoundField DataField="TotalForex" DataFormatString="{0:#,##0.00}" ItemStyle-HorizontalAlign="Right" SortExpression="TotalForex" HeaderText="Total Forex" />
                  <asp:BoundField DataField="Remark" sortExpression="Remark" HeaderText="Remark"></asp:BoundField>
                  <asp:BoundField DataField="RemarkRevisi" sortExpression="RemarkRevisi" HeaderText="Remark Revisi"></asp:BoundField>
              </Columns>
          </asp:GridView>
          </div>
            <asp:Panel runat="server" ID ="pnlNav" Visible="false">
            <asp:Button class="bitbtn btnadd" runat="server" ID="btnAdd2" Text="Add" ValidationGroup="Input"/>	 
            &nbsp &nbsp &nbsp
            <asp:DropDownList ID="ddlCommand2" CssClass="DropDownList" runat="server"/>
            <asp:Button class="bitbtn btngo" runat="server" ID="btnGo2" Text="G" />	 
            </asp:Panel>
    </asp:Panel>
    <asp:Panel runat="server" ID="pnlInput" Visible="false">
      <table>
        <tr>
            <td>SO No</td>
            <td>:</td>
            <td colspan="3">
            <asp:TextBox CssClass="TextBoxR" ValidationGroup="Input" runat="server" ReadOnly="true" ID="tbCode" Width="150px"/>
            <asp:Label runat="server" ID="Label3" Text =" Rev : "></asp:Label>
            <asp:Label runat="server" ID="lbRevisi"></asp:Label>
            </td>           
            
            <td>SO Date</td>
            <td>:</td>
            <td><BDP:BasicDatePicker ID="tbDate" runat="server" DateFormat="dd MMM yyyy" 
                        ReadOnly = "true" ShowNoneButton="false"
                        ButtonImageHeight="19px" ButtonImageWidth="20px" 
                        DisplayType="TextBoxAndImage" 
                        TextBoxStyle-CssClass="TextDate" ValidationGroup="Input"><TextBoxStyle CssClass="TextDate" /></BDP:BasicDatePicker></td>
        </tr>      
        <tr>
            <td>SO Type</td>
            <td>:</td>
            <td colspan="3">
                  <asp:DropDownList ID="ddlSOType" runat="server" 
                      CssClass="DropDownList" Enabled="true" Width="74px" 
                      ValidationGroup="Input">
                      <asp:ListItem>Local</asp:ListItem>
                      <asp:ListItem>Export</asp:ListItem>
                  </asp:DropDownList>
              </td>          
        
            <td>SO Kind</td>
            <td>:</td>
            <td>
                <asp:DropDownList Id ="ddlSOKind" Enabled="true" runat="server" 
                    CssClass="DropDownList" AutoPostBack="True" ValidationGroup="Input" >
                    <asp:ListItem Selected="True" >Regular</asp:ListItem>
                    <asp:ListItem>Replacement</asp:ListItem>
                    <asp:ListItem>Sample</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>                 
        <tr>
            <td><asp:LinkButton ID="lbProductGroup" runat="server" Text="Product Group"/></td>
            <td>:</td>
            <td colspan="3" >
                <asp:TextBox ID="tbProductGroup" ValidationGroup="Input" AutoPostBack="true" Width="80px" runat="server" CssClass="TextBox" MaxLength="5"/>
                <asp:TextBox ID="tbProductGroupName" Width="207px" runat="server" Enabled="False" MaxLength="60"
                    CssClass="TextBoxR" />
                <asp:Button class="bitbtn btngo" runat="server" ID="btnProductGroup" ValidationGroup="Input"
                    Text="..."/>
                <asp:Label ID="lbred" runat="server" ForeColor="Red">*</asp:Label>
            </td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td>
                <asp:TextBox ID="tbIRRNo" runat="server" CssClass="TextBoxR" Enabled="False" 
                    Visible="False" Width="130px" />
                <asp:Button ID="btnIRRNo" runat="server" class="bitbtn btngo" Text="..." 
                    Visible="False" />
            </td>            
        </tr>
        <tr>
            <td><asp:LinkButton ID="lbCustomer" runat="server" Text="Customer"/></td>
            <td>:</td>
            <td colspan="3" >
                <asp:TextBox ID="tbCustCode" ValidationGroup="Input" AutoPostBack="true" Width="80px" runat="server" CssClass="TextBox" MaxLength="12"/>
                <asp:TextBox ID="tbCustName" Width="207px" runat="server" Enabled="False" 
                    CssClass="TextBoxR" />
                <asp:Button class="bitbtn btngo" runat="server" ID="btnCust" Text="..." ValidationGroup="Input"/>
                <asp:Label ID="lbred0" runat="server" ForeColor="Red">*</asp:Label>
            </td>
            <td>Attn</td>
            <td>:</td>
            <td>
                
                <asp:TextBox ID="tbAttn" runat="server" CssClass="TextBox" MaxLength="60" 
                    ValidationGroup="Input" Width="155px" />
                
            </td>            
        </tr> 
        <tr>            
            <td>Department</td>
            <td>:</td>
            <td colspan="3">
                <asp:DropDownList ID="ddlDept" runat="server" CssClass="DropDownList" 
                    ValidationGroup="Input">
                </asp:DropDownList>
            </td>
            <td>Telephone</td>
            <td>:</td>            
            <td><asp:TextBox ID="tbPhone" Width="150px" runat="server" ValidationGroup="Input" CssClass="TextBox" MaxLength="60" />
            </td>
        </tr>   
        <tr>            
            <td>PO Cust No&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </td>
            <td>:</td>
            <td colspan="3">
                <asp:TextBox ID="tbCustPONo" Width="150px" runat="server" ValidationGroup="Input" CssClass="TextBox" MaxLength="255" />
                <asp:Label ID="lbred1" runat="server" ForeColor="Red">*</asp:Label>
            </td>
            <td>Contract No&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </td>
            <td><asp:Label ID="lbTitikDelivPriority" runat="server" Text=":" /></td>            
            <td>
                <asp:TextBox ID="tbContractNo" Width="150px" runat="server" 
                    ValidationGroup="Input" CssClass="TextBox" MaxLength="255" />
                <asp:Label ID="lbred5" runat="server" ForeColor="Red">*</asp:Label>
            </td>
        </tr>    
        <tr>            
            <td>Need Delivery</td>
            <td>:</td>
            <td colspan="3">
                <asp:DropDownList ID="ddlNeedDelivery" runat="server" AutoPostBack="true" 
                    CssClass="DropDownList" Enabled="true" ValidationGroup="Input" Width="74px">
                    <asp:ListItem>Y</asp:ListItem>
                    <asp:ListItem>N</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>            
            <td> &nbsp;</td>
        </tr>   
        <tr>
            <td>Delivery Place</td>
            <td>:</td>
            <td colspan="3" >
                <asp:TextBox ID="tbdelivery" ValidationGroup="Input" AutoPostBack="true" Width="80px" runat="server" CssClass="TextBox" MaxLength="12"/>
                <asp:TextBox ID="tbdeliveryName" Width="207px" runat="server" Enabled="False" MaxLength="60"
                    CssClass="TextBoxR" />
                <asp:Button class="bitbtn btngo" runat="server" ID="btnDelivery" Text="..." ValidationGroup="Input"/>
                <asp:Label ID="lbred2" runat="server" ForeColor="Red">*</asp:Label>
            </td>
            <td>City</td>
            <td>:</td>
            <td>
                <asp:DropDownList ID="ddlCity" runat="server" AutoPostBack="True" 
                    CssClass="DropDownList" Height="16px" ValidationGroup="Input" Width="147px" />
            </td>            
        </tr>
        <tr>            
            <td>Address</td>
            <td>:</td>
            <td colspan="3">
                <asp:TextBox ID="tbAddress" runat="server" CssClass="TextBox" Height="43px" 
                    MaxLength="255" TextMode="MultiLine" ValidationGroup="Input" Width="314px" />
            </td>
            <td>
                  &nbsp;</td>
              <td>
                  &nbsp;</td>
              <td>
              &nbsp;</td>
        </tr>
        <tr>
            <td>Term</td>
            <td>:</td>
            <td colspan="3"><asp:DropDownList ID="ddlTerm" runat="server" 
                    CssClass="DropDownList" ValidationGroup="Input" Height="16px" 
                    Width="147px" AutoPostBack="True" />
            </td>            
            
            <td>Term Remark</td>
            <td>:</td>
            <td>
                <asp:TextBox ID="tbTermRemark" runat="server" CssClass="TextBox" MaxLength="255" 
                    ValidationGroup="Input" Width="196px" />
            </td>            
        
        </tr>          
        <tr>
              <td>
                  Delivery Cost By</td>
              <td>
                  :</td>
              <td>
                 <asp:DropDownList ID="ddlCostBy" runat="server" AutoPostBack="False" 
                      CssClass="DropDownList" Enabled="False" Width="108px" Height="16px" 
                      ValidationGroup="Input">
                      <asp:ListItem Value="">Choose One</asp:ListItem>
                      <asp:ListItem>Company</asp:ListItem>
                      <asp:ListItem>Customer</asp:ListItem>
                  </asp:DropDownList>
                  <asp:Label ID="lbred3" runat="server" ForeColor="Red">*</asp:Label>
              </td>
              <td>
                  &nbsp;
              </td>
              <td>
                  &nbsp;</td>
              <td>
                  Price Include PPN</td>
              <td>
                  :</td>
              <td>
                  <asp:DropDownList ID="ddlfgInclude" runat="server" AutoPostBack="true" 
                      CssClass="DropDownList" Enabled="true" Width="46px" 
                      ValidationGroup="Input" Visible="True">
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
                <table>
                    <tr style="background-color:Silver;text-align:center">
                        <td>Base Forex</td>                        
                        <td>Disc Forex</td>
                        <td>PPN %</td>
                        <td>PPN Forex</td>                                                
                        <td>Total Forex</td>
                    </tr>
                    <tr>
                        <td><asp:TextBox ID="tbBaseForex" ValidationGroup="Input" runat="server" Width="120px" CssClass="TextBoxR"/></td>                        
                        <td><asp:TextBox ID="tbDiscForex" ValidationGroup="Input" runat="server" Width="120px" CssClass="TextBoxR"/></td>
                        <td><asp:TextBox ID="tbPPN" ValidationGroup="Input" runat="server" Width="40px" CssClass="TextBox"/></td>
                        <td><asp:TextBox ID="tbPPNForex" ValidationGroup="Input" runat="server" Width="120px" CssClass="TextBoxR"/></td>                                                
                        <td><asp:TextBox ID="tbTotalForex" ValidationGroup="Input" runat="server" Width="120px" CssClass="TextBoxR" /></td>
                    </tr>
                </table>
            </td>                
        </tr>
        <tr>
            <td>Unit Price Dec Place</td>
            <td>:</td>
            <td colspan="2">
                <asp:TextBox ID="tbPriceDec" runat="server" CssClass="TextBox" 
                    ValidationGroup="Input" Width="54px" />
            </td>
            <td colspan="2">
                Sub Total Dec Place</td>
            <td>
                :</td>
            <td>
                <asp:TextBox ID="tbTotalDec" runat="server" CssClass="TextBox" 
                    ValidationGroup="Input" Width="40px" />
            </td>
        </tr>
        <tr>
            <td>Remark</td>
            <td>:</td>
            <td colspan="6">
                <asp:TextBox ID="tbRemark" runat="server" CssClass="TextBoxMulti" 
                    MaxLength="255" TextMode="MultiLine" ValidationGroup="Input" Width="350px" />
                <asp:Button ID="btnGetData" runat="server" class="bitbtn btngo" Text="Get Data" 
                    ValidationGroup="Input" Visible="false" Width="74px" />
            </td>
        </tr>
          <tr>
              <td>
                  Remark Revisi</td>
              <td>
                  :</td>
              <td colspan="6">
                  <asp:Label ID="lbRemarkRevisi" runat="server"></asp:Label>
              </td>
          </tr>
      </table>  
      
      <br />      
      <div style="font-size:medium; color:Blue;">Detail</div>
      <hr style="color:Blue" />
        <asp:Menu
            ID="Menu1"
            Width="100%"
            runat="server"
            StaticMenuItemStyle-CssClass = "MenuItem"
            StaticSelectedStyle-CssClass = "MenuSelect"    
            Orientation="Horizontal"            
            StaticEnableDefaultPopOutImage="False" Visible="False">            
            <StaticSelectedStyle CssClass="MenuSelect" />
            <StaticMenuItemStyle CssClass="MenuItem" />
            <Items>
                <asp:MenuItem Selected="true" Text="Detail SO" Value="0"></asp:MenuItem>                
                <asp:MenuItem Text="Detail Price" Value="1"></asp:MenuItem>
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
                    <asp:Button ID="btnAddDtSO" runat="server" class="bitbtndt btnadd" Text="Add" 
                        validationgroup="Input" Width="60" />
                    <br />
                    <br />
                    <asp:GridView ID="GridDt" runat="server" AutoGenerateColumns="false" 
                        ShowFooter="True">
                        <HeaderStyle CssClass="GridHeader" />
                        <RowStyle CssClass="GridItem" Wrap="false" />
                        <AlternatingRowStyle CssClass="GridAltItem" />
                        <PagerStyle CssClass="GridPager" />
                        <Columns>
                            <asp:TemplateField HeaderText="Action">
                                  <ItemTemplate>
                                    <asp:Button ID="btnEdit" runat="server" class="bitbtn btnedit" Text="Edit Price" CommandName="Edit"/>
                                    <asp:Button ID="btnDelete" runat="server" class="bitbtn btndelete" Text="Delete" CommandName="Delete"/>
                                  </ItemTemplate>                                  
                            </asp:TemplateField>                            
                            <asp:TemplateField>
                                <ItemTemplate>
                                  <asp:Button ID="btnDetail" runat="server" class="bitbtn btndetail" Text="Schedule" CommandName="Detail" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" />
                                  <asp:Button ID="btnDetailSub" runat="server" class="bitbtn btndetail" Text="Detail Price" CommandName="DetailSub" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" Width="100"/>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                  <asp:Button ID="btnClosing" runat="server" class="bitbtn btnclosing" Text="Closing" CommandName="Closing" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" />     
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Product" HeaderText="Product" />
                            <asp:BoundField DataField="ProductName" HeaderStyle-Width="250px" HeaderText="Product Name" />
                            <asp:BoundField DataField="Specification" HeaderStyle-Width="250px" HeaderText="Specification" />
                            <asp:BoundField DataField="Specification2" HeaderStyle-Width="250px" HeaderText="Specification2" />
                            <asp:BoundField DataField="QtyOrder" DataFormatString="{0:#,##0.##}" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="80px" HeaderText="Qty Order" />
                            <asp:BoundField DataField="UnitOrder" HeaderStyle-Width="80px" HeaderText="Unit Order" />
                            <asp:BoundField DataField="Qty" DataFormatString="{0:#,##0.##}" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="80px" HeaderText="Qty 1" />
                            <asp:BoundField DataField="Unit" HeaderStyle-Width="80px" HeaderText="Unit 1" />
                            <asp:BoundField DataField="QtyRoll" DataFormatString="{0:#,##0.##}" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="80px" HeaderText="Qty 2" />
                            <asp:BoundField DataField="UnitPack" HeaderStyle-Width="80px" HeaderText="Unit 2" />
                            <asp:BoundField DataField="QtyM2"  DataFormatString="{0:#,##0.##}" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="80px" HeaderText="Qty 3" />
                            <asp:BoundField DataField="UnitM2" HeaderStyle-Width="80px" HeaderText="Unit 3" />
                            <asp:BoundField DataField="PriceForex" DataFormatString="{0:#,##0.#######}" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="80px" HeaderText="Price" />
                            <asp:BoundField DataField="AmountForex" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:#,##0.#######}" HeaderStyle-Width="80px" HeaderText="Amount Forex" />
                            <asp:BoundField DataField="Disc" DataFormatString="{0:#,##0.##}" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="80px" HeaderText="Disc %" />
                            <asp:BoundField DataField="DiscForex" DataFormatString="{0:#,##0.#######}" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="80px" HeaderText="Disc Forex" />
                            <asp:BoundField DataField="NettoForex" DataFormatString="{0:#,##0.#######}" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="80px" HeaderText="Total Forex" />
                            <asp:BoundField DataField="CommisionForex" DataFormatString="{0:#,##0.#######}" ItemStyle-HorizontalAlign="Right"  HeaderStyle-Width="80px" HeaderText="Commision/Product" />
                            <asp:BoundField DataField="CommisionUnit" HeaderStyle-Width="250px" HeaderText="Unit Commision" />
                            <asp:BoundField DataField="Remark" HeaderStyle-Width="250px" HeaderText="Remark" />
                            <asp:BoundField DataField="QtyDO" DataFormatString="{0:#,##0.##}" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="80px" HeaderText="Qty DO" />
                            <asp:BoundField DataField="DoneClosing" HeaderStyle-Width="250px" HeaderText="Done Closing" />
                            <asp:BoundField DataField="QtyClose" DataFormatString="{0:#,##0.##}" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="80px" HeaderText="Qty Close" />
                            <asp:BoundField DataField="RemarkClose" HeaderStyle-Width="250px" HeaderText="Remark Close" />
                            </Columns>
                    </asp:GridView>
                    <br />
                    <asp:Button ID="btnAddDtSO2" runat="server" class="bitbtndt btnadd" Text="Add" 
                        validationgroup="Input" Width="60" />
                    <br />
              </div>   
              </asp:Panel> 
              <asp:Panel runat="server" ID="pnlEditDt" Visible="false">
                <table>              
                    <tr>
                        <td>Product</td>
                        <td>:</td>
                        <td colspan="6"><asp:TextBox runat="server" ID="tbProductDt" CssClass="TextBox" AutoPostBack="true" />
                            <asp:TextBox runat="server"  CssClass="TextBoxR"
                                ID="tbProductNameDt" EnableTheming="True" ReadOnly="True" Enabled="False" 
                                Width="200px"/>                             
                            <asp:Button ID="btnProductDt" runat="server" class="bitbtn btngo" Text="..." />
                            <asp:Label ID="lbred4" runat="server" ForeColor="Red">*</asp:Label>
                        </td>           
                    </tr>        
                    <tr>
                        <td>Specification</td>
                        <td>:</td>
                        <td colspan="6">
                            <asp:TextBox ID="tbSpecificationDt" runat="server" CssClass="TextBox" 
                                ReadOnly="true" TextMode="MultiLine" Width="360px" MaxLength="255" />
                        </td>
                    </tr>                            
                    <tr>
                        <td>
                            Specification 2</td>
                        <td>
                            :</td>
                        <td colspan="6">
                            <asp:TextBox ID="tbSpecificationDt2" runat="server" CssClass="TextBox" 
                                ReadOnly="true" TextMode="MultiLine" Width="360px" MaxLength="255" />
                        </td>
                    </tr>
                    <tr>
                        <td>Qty</td>
                        <td>:</td>
                        <td colspan="6">
                            <table>
                                <tr style="background-color:Silver;text-align:center">
                                    <td>Order Qty</td>
                                    <td>Order&nbsp; Unit </td>                                
                                    <td>
                                        Wrhs Qty</td>
                                    <td>
                                        Wrhs Unit</td>
                                    <td>
                                        <asp:Label ID="lbUnitPack" runat="server"></asp:Label>
                                    </td>
                                    
                                    <td>
                                        <asp:Label ID="lbUnitM2" runat="server"></asp:Label>
                                    </td>
                                    
                                    <td>
                                        Qty DO</td>
                                    
                                </tr>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="tbQtyOrderDt" runat="server" CssClass="TextBoxR" 
                                            Enabled="False" />
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlUnitOrderDt" runat="server" AutoPostBack="True" 
                                            CssClass="DropDownList" Enabled="False" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="tbQtyWrhsDt" runat="server" CssClass="TextBoxR" 
                                            Enabled="False" />
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlUnitWrhsDt" runat="server" AutoPostBack="True" 
                                            CssClass="DropDownList" Enabled="False" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="tbQtyRollDt" runat="server" AutoPostBack="true" 
                                            CssClass="TextBoxR" />
                                        <asp:TextBox ID="tbQtyPerRoll" runat="server" CssClass="TextBoxR" 
                                            Visible="False" />
                                    </td>
                                    
                                    <td>
                                        <asp:TextBox ID="tbQtyM2Dt" runat="server" AutoPostBack="true" 
                                            CssClass="TextBoxR" />
                                        <asp:TextBox ID="tbQtyPerM2" runat="server" CssClass="TextBoxR" 
                                            Visible="False" />
                                    </td>
                                    
                                    <td>
                                        <asp:TextBox ID="tbQtyDO" runat="server" AutoPostBack="true" 
                                            CssClass="TextBoxR" />
                                    </td>
                                    
                                </tr>
                            </table>
                        </td>
                    </tr>                
                    <tr>
                        <td>Amount</td>
                        <td>:</td>
                        <td colspan="6">
                            <table>
                                <tr style="background-color:Silver;text-align:center">
                                    <td>Price*</td>
                                    <td>Amount</td>
                                    <td>Disc %</td>
                                    <td>Disc Forex</td>
                                    <td>Total Forex</td>
                                                             
                                              
                                </tr>
                                <tr>
                                    <td><asp:TextBox ID="tbPriceForexDt" runat="server" CssClass="TextBox" 
                                            AutoPostBack="True" /></td>
                                    <td><asp:TextBox ID="tbAmountForexDt"  runat="server" CssClass="TextBoxR" /></td>
                                    <td><asp:TextBox ID="tbDiscDt" AutoPostBack="true" runat="server" CssClass="TextBox" Width="45px"/></td>
                                    <td><asp:TextBox ID="tbDiscForexDt" AutoPostBack="true" runat="server" CssClass="TextBox" /></td>
                                    <td><asp:TextBox ID="tbTotalForexDt" runat="server" CssClass="TextBoxR" /></td>
                                    <td>&nbsp;</td>
                                    <td>&nbsp;</td>
                                    
                                </tr>
                            </table>
                        </td>                
                    </tr>                    
                    <tr>
                        <td>
                            Commision/Product</td>
                        <td>
                            :</td>
                        <td>
                            <asp:TextBox ID="tbcommision" runat="server" CssClass="TextBox" 
                                AutoPostBack="True" />
                        </td>
                        <td>
                            Unit Commision</td>
                        <td>
                            :</td>
                        <td>
                            <asp:DropDownList ID="ddlUnitCommision" runat="server" CssClass="DropDownList" 
                                ValidationGroup="Input">
                                <asp:ListItem>Y</asp:ListItem>
                                <asp:ListItem>N</asp:ListItem>
                            </asp:DropDownList>
                            <asp:Label ID="lbred6" runat="server" ForeColor="Red">*</asp:Label>
                        </td>
                        <td>
                            &nbsp;</td>
                        <td>
                            <asp:DropDownList ID="ddlPartial" runat="server" CssClass="DropDownList" 
                                Visible="False">
                                <asp:ListItem>Y</asp:ListItem>
                                <asp:ListItem Selected="True">N</asp:ListItem>
                            </asp:DropDownList>
                            <BDP:BasicDatePicker ID="tbDeliveryDate" runat="server" 
                                ButtonImageHeight="19px" ButtonImageWidth="20px" DateFormat="dd MMM yyyy" 
                                DisplayType="TextBoxAndImage" ReadOnly="true" ShowNoneButton="false" 
                                TextBoxStyle-CssClass="TextDate" Visible="False"><TextBoxStyle CssClass="TextDate" /></BDP:BasicDatePicker>
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
                        <td>
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
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
                <asp:Label ID="Label5" runat="server" Font-Bold="true" ForeColor="Blue" Text="Product : " />
                    <asp:Label ID="lbProductSub" runat="server" Font-Bold="true" ForeColor="Blue" Text="Product oi" />
                    <asp:Label ID="lbProductNameSub" runat="server" Font-Bold="true" ForeColor="Blue" Text="" />
                    <br />
                <asp:Panel ID="pnlDtSub" runat="server">
                    
                    <br />
                    
                <asp:Button ID="btnAddDtSub" runat="server" class="bitbtndt btnadd" Text="Add" validationgroup="Input" Width = "60"/>									

                    &nbsp;<asp:Button ID="btnbackPR0" runat="server" class="bitbtndt btnback" 
                        Text="Back" Width="60" />
                    <br />
                    <br />
                    <div style="border:0px  solid; width:100%; height:100%; overflow:auto;">
                        <asp:GridView ID="GridSub" runat="server" AutoGenerateColumns="false" 
                            ShowFooter="True">
                            <HeaderStyle CssClass="GridHeader" />
                            <RowStyle CssClass="GridItem" Wrap="false" />
                            <AlternatingRowStyle CssClass="GridAltItem" />
                            <PagerStyle CssClass="GridPager" />
                            <Columns>
                                <asp:TemplateField HeaderText="Action">
                                    <ItemTemplate>
                                        <asp:Button ID="btnEdit" runat="server" class="bitbtn btnedit" 
                                            CommandName="Edit" Text="Edit" />
                                        <asp:Button ID="btnDelete" runat="server" class="bitbtn btndelete" 
                                            CommandName="Delete" Text="Delete" />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Button ID="btnUpdate" runat="server" class="bitbtn btnupdate" 
                                            Text="Update" />
                                        <asp:Button ID="btnCancel" runat="server" class="bitbtn btncancel" 
                                            Text="Cancel" />
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="ItemNo" HeaderText="No" />
                                <asp:BoundField DataField="Remark" HeaderText="Remark" />
                                <asp:BoundField DataField="Qty" HeaderStyle-Width="80px" HeaderText="Qty" 
                                    ItemStyle-HorizontalAlign="Right" />
                                <asp:BoundField DataField="Unit" HeaderStyle-Width="80px" HeaderText="Unit" />
                                <asp:BoundField DataField="Price" HeaderStyle-Width="80px" HeaderText="Price" 
                                    ItemStyle-HorizontalAlign="Right" />
                            </Columns>
                        </asp:GridView>
                    </div>
                    <br />
                    <asp:Button ID="btnAddDtSubke2" runat="server" class="bitbtndt btnadd" 
                        Text="Add" validationgroup="Input" Width="60" />
                    &nbsp;<asp:Button ID="btnbackPR" runat="server" class="bitbtndt btnback" Text="Back" 
                        Width="60" />

              </asp:Panel>
              <asp:Panel runat="server" ID="pnlEditDtSub" Visible="false">
                <table>      
                     
                    <tr>
                        <td>
                            No</td>
                        <td>
                            :</td>
                        <td>
                            <asp:Label ID="Lbitem" runat="server" Text="Label"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>Detail Remark</td>
                        <td>:</td>
                        <td><asp:TextBox Width="332px" CssClass="TextBox" runat="server" 
                                ID="tbDetailRemark" TextMode="MultiLine"/></td>
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
                                    <td><asp:TextBox CssClass="TextBox" runat="server" ID="tbQtySub"/></td>
                                    <td><asp:DropDownList CssClass="DropDownList" ID="ddlUnitSub" Enabled="false" runat="server"/></td>
                                    <td>
                                        <asp:TextBox ID="txUnitOrder" runat="server" CssClass="TextBox" 
                                            Visible="False" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>                              
                    <tr>
                        <td>
                            Price</td>
                        <td>
                            :</td>
                        <td>
                            <asp:TextBox ID="tbPriceSub" runat="server" CssClass="TextBox" />
                        </td>
                    </tr>
                </table>
                <br />  
                
                  <asp:Button ID="btnSavePR" runat="server" class="bitbtndt btnsave" Text="Save" 
                      validationgroup="Input" />
                <asp:Button ID="btnCancelPR" runat="server" class="bitbtndt btncancel" Text="Cancel" />									
           </asp:Panel> 
                           
            </asp:View>
            <asp:View ID="Tab3" runat="server">
                <asp:Label ID="Label4" runat="server" Text="Product :" />
                <asp:Label ID="lbProductDt2" runat="server" Font-Bold="true" ForeColor="Blue" Text="Product oi" />
                <asp:Label ID="lbProductDtName2" runat="server" Font-Bold="true" ForeColor="Blue" Text="" />
                &nbsp;<asp:Label ID="lbQtyOrderDt2" runat="server" Font-Bold="true" ForeColor="Blue" 
                    Text="" />
                <br />
                <br />
                <asp:Panel ID="pnlDt2" runat="server">
                
                    <br />
                
                <asp:Button ID="btnAddDt2" runat="server" class="bitbtndt btnadd" Text="Add" ValidationGroup="Input"/>									
                
                    &nbsp;<asp:Button ID="btnbackSO2" runat="server" class="bitbtndt btnback" 
                        Text="Back" Width="60" />
                    <br />
                    <br />
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
                                        <asp:Button ID="btnEdit" runat="server" class="bitbtn btnedit" 
                                            CommandName="Edit" Text="Edit" />
                                        <asp:Button ID="btnDelete" runat="server" class="bitbtn btndelete" 
                                            CommandName="Delete" Text="Delete" />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Button ID="btnUpdate" runat="server" class="bitbtn btnupdatedt" 
                                            Text="Update" ValidationGroup="Input" />
                                        <asp:Button ID="btnCancel" runat="server" class="bitbtn btncanceldt" 
                                            Text="Cancel" ValidationGroup="Input" />
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="Delivery" HeaderText="Delivery Date" />
                                <asp:BoundField DataField="QtyOrder" HeaderStyle-Width="80px" 
                                    HeaderText="Qty Order" ItemStyle-HorizontalAlign="Right" />
                                <asp:BoundField DataField="UnitOrder" HeaderStyle-Width="80px" 
                                    HeaderText="Unit Order" />
                                <asp:BoundField DataField="QtyPack" HeaderStyle-Width="80px" 
                                    HeaderText="Qty Pack" ItemStyle-HorizontalAlign="Right" />
                                <asp:BoundField DataField="UnitPack" HeaderStyle-Width="80px" 
                                    HeaderText="Unit Pack" />
                                <asp:BoundField DataField="Remark" HeaderText="Remark" />
                            </Columns>
                        </asp:GridView>
                        <br />
                    </div>
                    <asp:Button ID="btnAddDt2Ke2" runat="server" class="bitbtndt btnadd" Text="Add" 
                        ValidationGroup="Input" />
                    &nbsp;<asp:Button ID="btnbackSO" runat="server" class="bitbtndt btnback" 
                        Text="Back"  Width="60" />
                    &nbsp;</asp:Panel>
              <asp:Panel runat="server" ID="pnlEditDt2" Visible="false">
                <table >      
                    <tr>
                        <td>Delivery Date</td>
                        <td>:</td>
                        <td>
                            <BDP:BasicDatePicker ID="tbDeliveryDateDt2" runat="server" AutoPostBack="True" 
                                ButtonImageHeight="19px" ButtonImageWidth="20px" DateFormat="dd MMM yyyy" 
                                DisplayType="TextBoxAndImage" ShowNoneButton="False" 
                                TextBoxStyle-CssClass="TextDate" ValidationGroup="Input"><TextBoxStyle CssClass="TextDate" /></BDP:BasicDatePicker>
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
                                    <td>
                                        Pack</td>
                                    <td>
                                        Pack</td>
                                </tr>
                                <tr cellspacing="0" cellpadding="0">
                                    <td ><asp:TextBox CssClass="TextBox" runat="server" ID="tbQtyOrderDt2" 
                                            AutoPostBack="True" Width="96px"/></td>
                                    <td><asp:DropDownList CssClass="DropDownList" ID="ddlUnitOrderDt2" Enabled="false" runat="server"/></td>
                                    <td>
                                        <asp:TextBox ID="tbQtyPackDt2" runat="server" AutoPostBack="True" 
                                            CssClass="TextBox" Width="99px" />
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlUnitPackDt2" runat="server" AutoPostBack="True" 
                                            CssClass="DropDownList" />
                                    </td>
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
    <asp:HiddenField ID="HiddenRemarkRevisi" runat="server" />
    
    </form>
</body>
</html>
