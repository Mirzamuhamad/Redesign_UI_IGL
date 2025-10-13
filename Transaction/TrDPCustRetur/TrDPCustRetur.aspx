<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TrDPCustRetur.aspx.vb" Inherits="TrDPCustRetur" %>

<%@ Register Assembly="BasicFrame.WebControls.BasicDatePicker" Namespace="BasicFrame.WebControls"
    TagPrefix="BDP" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>

    <script src="../../Function/OpenDlg.js" type="text/javascript"></script>
    <script src="../../Function/Function.JS" type="text/javascript"></script>
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
 <link href="https://fonts.googleapis.com/css2?family=Muli:wght@300;400;600&display=swap" rel="stylesheet">

    <script type="text/javascript">         
        function setdigit(nStr, digit)
        {
        try
        {
        var TNstr = parseFloat(nStr);        
        TNstr = TNstr.toFixed(digit);                
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
          var VPayment = document.getElementById("tbTotalPayment").value.replace(/\$|\,/g,"");
          var VDP = document.getElementById("tbTotalDP").value.replace(/\$|\,/g,"");
          var VCharge = document.getElementById("tbTotalCharge").value.replace(/\$|\,/g,"");
          var VSelisih = document.getElementById("tbSelisihKurs").value.replace(/\$|\,/g,"");
          var VDiffRate = document.getElementById("tbDiffRate").value.replace(/\$|\,/g,"");
          
          document.getElementById("tbTotalPayment").value = setdigit(VPayment,'<%=ViewState("DigitHome")%>');
          document.getElementById("tbTotalDP").value = setdigit(VDP,'<%=ViewState("DigitHome")%>');
          document.getElementById("tbTotalCharge").value = setdigit(VCharge,'<%=ViewState("DigitHome")%>');
          document.getElementById("tbSelisihKurs").value = setdigit(VSelisih,'<%=ViewState("DigitHome")%>');
          document.getElementById("tbDiffRate").value = setdigit(VDiffRate,'<%=ViewState("DigitHome")%>');
        }catch (err){
            alert(err.description);
          }      
        }   
        
       function setformatdt()
        {
        try
         {         
         var BaseForex = document.getElementById("tbBaseForexDt").value.replace(/\$|\,/g,""); 
         var PPnForex = document.getElementById("tbPPnForexDt").value.replace(/\$|\,/g,""); 
         var TotalForex = document.getElementById("tbTotalForexDt").value.replace(/\$|\,/g,""); 
         var TotalHome = document.getElementById("tbTotalHomeDt").value.replace(/\$|\,/g,""); 
         var Rate = document.getElementById("tbRateDt").value.replace(/\$|\,/g,""); 
         var DPRate = document.getElementById("tbDPRate").value.replace(/\$|\,/g,""); 
         var PPnRate = document.getElementById("tbPPnRateDt").value.replace(/\$|\,/g,""); 
                          
         document.getElementById("tbRateDt").value = setdigit(Rate,'<%=ViewState("DigitRate")%>');
         document.getElementById("tbDPRate").value = setdigit(DPRate,'<%=ViewState("DigitRate")%>');
         document.getElementById("tbPPnRateDt").value = setdigit(PPnRate,'<%=ViewState("DigitRate")%>');
         document.getElementById("tbBaseForexDt").value = setdigit(BaseForex,'<%=VIEWSTATE("DigitCurr")%>');
         document.getElementById("tbPPnForexDt").value = setdigit(PPnForex,'<%=VIEWSTATE("DigitCurr")%>');
         document.getElementById("tbTotalForexDt").value = setdigit(TotalForex,'<%=VIEWSTATE("DigitCurr")%>');
         document.getElementById("tbTotalHomeDt").value = setdigit(TotalHome,'<%=ViewState("DigitHome")%>');
         
        }catch (err){
            //alert(err.description);
          }      
        }   

        function setformatdt2()
        {
        try
         {
         var Rate = document.getElementById("tbRateDt2").value.replace(/\$|\,/g,""); 
         var ChargeRate = document.getElementById("tbChargeRateDt2").value.replace(/\$|\,/g,""); 
         var PaymentForex = document.getElementById("tbPaymentForexDt2").value.replace(/\$|\,/g,""); 
         var PaymentHome = document.getElementById("tbPaymentHomeDt2").value.replace(/\$|\,/g,""); 
         var ChargeForex = document.getElementById("tbChargeForexDt2").value.replace(/\$|\,/g,""); 
         var ChargeHome = document.getElementById("tbChargeHomeDt2").value.replace(/\$|\,/g,""); 
                          
         document.getElementById("tbRateDt2").value = setdigit(Rate,'<%=ViewState("DigitRate")%>');
         document.getElementById("tbChargeRateDt2").value = setdigit(ChargeRate,'<%=ViewState("DigitRate")%>');
         document.getElementById("tbPaymentForexDt2").value = setdigit(PaymentForex,'<%=VIEWSTATE("DigitExpenseCurr")%>');
         document.getElementById("tbPaymentHomeDt2").value = setdigit(PaymentHome,'<%=ViewState("DigitHome")%>');
         document.getElementById("tbChargeForexDt2").value = setdigit(ChargeForex,'<%=VIEWSTATE("DigitExpenseCurr")%>');
         document.getElementById("tbChargeHomeDt2").value = setdigit(ChargeHome,'<%=ViewState("DigitHome")%>');
         
         //document.getElementById("tbPaymentForexDt2").value = setdigit(PaymentForex,'<%=VIEWSTATE("DigitCurrDt")%>');
        }catch (err){
            alert(err.description);
          }      
        }           
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="Content">
        <div class="H1">
            DP Customer Retur</div>
        <hr style="color: Blue" />
        <asp:Panel runat="server" ID="PnlHd">
            <table>
                <tr>
                    <td style="width: 100px; text-align: right">
                        Quick Search :
                    </td>
                    <%--TransNmbr, TransDate, STATUS, FgReport, UserType, UserCode, UserName, Attn, Remark--%>
                    <td>
                        <asp:TextBox CssClass="TextBox" runat="server" ID="tbFilter" />
                        <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlField">
                            <asp:ListItem Selected="True" Value="TransNmbr">DP Retur No</asp:ListItem>
                            <asp:ListItem Value="Status">Status</asp:ListItem>
                            <asp:ListItem Value="dbo.FormatDate(TransDate)">Date</asp:ListItem>
                            <asp:ListItem Value="FgReport">Report</asp:ListItem>
                            <asp:ListItem Value="Customer_Name">Customer</asp:ListItem>
                            <asp:ListItem Value="Attn">Attn</asp:ListItem>
                            <asp:ListItem Value="DPNo">DP No</asp:ListItem>
                            <asp:ListItem Value="TotalPayment">Payment</asp:ListItem>
                            <asp:ListItem Value="TotalDP">DP</asp:ListItem>
                            <asp:ListItem Value="TotalCharge">Charge</asp:ListItem>
                            <asp:ListItem Value="Remark">Remark</asp:ListItem>
                        </asp:DropDownList>
                        <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlRange">
                        </asp:DropDownList>
                        <asp:Button class="bitbtn btnsearch" runat="server" ID="btnSearch" Text="Search" />
                        <asp:Button class="bitbtn btngo" runat="server" ID="btnExpand" Text="..." />
                    </td>
                    <td>
                        <asp:LinkButton ID="LbAdvSearch" runat="server" Text="Advanced Search" />
                    </td>
                </tr>
            </table>
            <asp:Panel runat="server" ID="pnlSearch" Visible="false">
                <table>
                    <tr>
                        <td style="width: 100px; text-align: right">
                            <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlNotasi">
                                <asp:ListItem Selected="true" Text="OR" Value="OR"></asp:ListItem>
                                <asp:ListItem Text="AND" Value="AND"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:TextBox runat="server" CssClass="TextBox" ID="tbfilter2" />
                            <asp:DropDownList runat="server" CssClass="DropDownList" ID="ddlField2">
                                <asp:ListItem Selected="True" Value="TransNmbr">DP Retur No</asp:ListItem>
                                <asp:ListItem Value="Status">Status</asp:ListItem>
                                <asp:ListItem Value="dbo.FormatDate(TransDate)">Date</asp:ListItem>
                                <asp:ListItem Value="FgReport">Report</asp:ListItem>
                                <asp:ListItem Value="Customer_Name">Customer</asp:ListItem>
                                <asp:ListItem Value="Attn">Attn</asp:ListItem>
                                <asp:ListItem Value="DPNo">DP No</asp:ListItem>
                                <asp:ListItem Value="TotalPayment">Payment</asp:ListItem>
                                <asp:ListItem Value="TotalDP">DP</asp:ListItem>
                                <asp:ListItem Value="TotalCharge">Charge</asp:ListItem>
                                <asp:ListItem Value="Remark">Remark</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <br />
            <asp:Button class="bitbtn btnadd" runat="server" ID="BtnAdd" Text="Add" />
            &nbsp &nbsp &nbsp
            <asp:DropDownList CssClass="DropDownList" ID="ddlCommand" runat="server" Visible="false" />
            <asp:Button class="btngo" runat="server" ID="BtnGo" Text="G" Visible="false" />
            <br />
            <div style="border: 0px  solid; width: 100%; height: 100%; overflow: auto;">
                <asp:GridView ID="GridView1" runat="server" AllowPaging="True" AllowSorting="true"
                    CssClass="Grid" AutoGenerateColumns="false">
                    <HeaderStyle CssClass="GridHeader"></HeaderStyle>
                    <RowStyle CssClass="GridItem" Wrap="false" />
                    <AlternatingRowStyle CssClass="GridAltItem" />
                    <PagerStyle CssClass="GridPager" />
                    <Columns>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:CheckBox ID="cbSelectHd" runat="server" AutoPostBack="true" OnCheckedChanged="cbSelectHd_CheckedChanged" />
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
                                </asp:DropDownList>
                                <asp:Button class="btngo" runat="server" ID="BtnGo" Text="G" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>"
                                    CommandName="Go" />
                            </ItemTemplate>
                            <HeaderStyle Width="110px" />
                        </asp:TemplateField>
                        <asp:BoundField DataField="TransNmbr" HeaderStyle-Width="120px" SortExpression="Nmbr" HeaderText="Payment No"></asp:BoundField>
                        <asp:BoundField DataField="Status" HeaderText="Status"></asp:BoundField>
                        <asp:BoundField DataField="TransDate" HtmlEncode="true" DataFormatString="{0:dd MMM yyyy}" HeaderStyle-Width="80px" SortExpression="TransDate" HeaderText="Payment Date">
                        </asp:BoundField>
                        <asp:BoundField DataField="FgReport" HeaderStyle-Width="80px" HeaderText="Report">
                        </asp:BoundField>
                        <asp:BoundField DataField="Customer" HeaderStyle-Width="200px" SortExpression="Customer" HeaderText="Customer"></asp:BoundField>
                        <asp:BoundField DataField="Attn" HeaderStyle-Width="200px" SortExpression="Attn" HeaderText="Attn"></asp:BoundField>
                        <asp:BoundField DataField="TotalPayment" DataFormatString="{0:#,##0.00}" HeaderStyle-Width="80px" SortExpression="TotalPayment" HeaderText="Payment"></asp:BoundField>
                        <asp:BoundField DataField="TotalDP" DataFormatString="{0:#,##0.00}" HeaderStyle-Width="80px" SortExpression="TotalDP" HeaderText="DP"></asp:BoundField>
                        <asp:BoundField DataField="TotalCharge" DataFormatString="{0:#,##0.00}" HeaderStyle-Width="80px" SortExpression="TotalCharge" HeaderText="Charge"></asp:BoundField>
                        <asp:BoundField DataField="TotalDiffRate" DataFormatString="{0:#,##0.00}" HeaderStyle-Width="80px" SortExpression="TotalDiffRate" HeaderText="Selisih Kurs"/>
                        <asp:BoundField DataField="Remark" HeaderStyle-Width="250px" HeaderText="Remark">
                        </asp:BoundField>
                    </Columns>
                </asp:GridView>
            </div>
            <asp:Panel runat="server" ID="pnlNav" Visible="false">
                <asp:Button class="bitbtn btnadd" runat="server" ID="btnAdd2" Text="Add" />
                &nbsp &nbsp &nbsp
                <asp:DropDownList ID="ddlCommand2" CssClass="DropDownList" runat="server" />
                <asp:Button class="btngo" runat="server" ID="btnGo2" Text="G" />
            </asp:Panel>
        </asp:Panel>
        <asp:Panel runat="server" ID="pnlInput" Visible="false">
            <table>
                <tr>
                    <td>
                        Payment No
                    </td>
                    <td>
                        :
                    </td>
                    <td>
                        <asp:TextBox CssClass="TextBoxR" runat="server" ID="tbCode" Width="150px" Enabled="False" />
                    </td>
                    <td>
                        Payment Date
                    </td>
                    <td>
                        :
                    </td>
                    <td>
                        <BDP:BasicDatePicker ID="tbDate" runat="server" DateFormat="dd MMM yyyy" ReadOnly="true"
                            ValidationGroup="Input" ButtonImageHeight="19px" ButtonImageWidth="20px" DisplayType="TextBoxAndImage"
                            TextBoxStyle-CssClass="TextDate" ShowNoneButton="False">
                            <TextBoxStyle CssClass="TextDate" />
                        </BDP:BasicDatePicker>
                    </td>
                </tr>
                <%--<tr>
              <td>
                  Report
              </td>
              <td>
                  :
              </td>
              <td colspan="4">
                  <asp:DropDownList AutoPostBack="true" ValidationGroup="Input" CssClass="DropDownList"
                      ID="ddlReport" runat="server">
                      <asp:ListItem Selected="True">Y</asp:ListItem>
                      <asp:ListItem>N</asp:ListItem>
                      <asp:ListItem Value="*">All</asp:ListItem>
                  </asp:DropDownList>
              </td>
          </tr>--%>
                <tr>
                    <td>
                        Customer
                    </td>
                    <td>
                        :
                    </td>
                    <td colspan="3">
                        <asp:TextBox CssClass="TextBox" ValidationGroup="Input" runat="server" ID="tbCustCode"
                            AutoPostBack="true" />
                        <asp:TextBox CssClass="TextBox" runat="server" ID="tbCustName" Enabled="false" Width="225px" />
                        <asp:Button Class="btngo" ID="btnCust" Text="..." runat="server" ValidationGroup="Input" />
                    </td>
                </tr>
                <tr>
                    <td>
                        Attn
                    </td>
                    <td>
                        :
                    </td>
                    <td colspan="4">
                        <asp:TextBox runat="server" ValidationGroup="Input" ID="tbAttn" MaxLength="255" CssClass="TextBox"
                            Width="225px" />
                    </td>
                </tr>
                <tr>
                    <td>
                        Total
                    </td>
                    <td>
                        :
                    </td>
                    <td colspan="4">
                        <table>
                            <tr style="background-color: Silver; text-align: center">
                                <td>
                                    <asp:Label ID="lbTotPayment" runat="server" CssClass="TextBox" Text="Payment" />
                                </td>
                                <td>
                                    <asp:Label ID="lbTotDP" runat="server" CssClass="TextBox" Text="DP" />
                                </td>
                                <td>
                                    <asp:Label ID="lbTotCharge" runat="server" CssClass="TextBox" Text="Charge" />
                                </td>
                                <td>
                                    <asp:Label ID="lbSelisihKurs" runat="server" CssClass="TextBox" Text="Selisih Kurs (IDR)" />
                                </td>
                                <td>
                                    <asp:Label ID="lbDiffRate" runat="server" CssClass="TextBox" Text="Difference Rate" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:TextBox runat="server" ID="tbTotalPayment" CssClass="TextBoxR" />
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="tbTotalDP" CssClass="TextBoxR" />
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="tbTotalCharge" CssClass="TextBoxR" />
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="tbSelisihKurs" CssClass="TextBoxR" />
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="tbDiffRate" CssClass="TextBoxR" />
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
                        :
                    </td>
                    <td colspan="4">
                        <asp:TextBox runat="server" ValidationGroup="Input" ID="tbRemark" CssClass="TextBox"
                            Width="225px" />
                    </td>
                </tr>
            </table>
            <br />
            <hr style="color: Blue" />
            <asp:Menu ID="Menu1" runat="server" CssClass="Menu" StaticMenuItemStyle-CssClass="MenuItem"
                StaticSelectedStyle-CssClass="MenuSelect" Orientation="Horizontal" ItemWrap="False"
                StaticEnableDefaultPopOutImage="False">
                <Items>
                    <asp:MenuItem Text="Detail DP" Value="0"></asp:MenuItem>
                    <asp:MenuItem Text="Detail Payment" Value="1"></asp:MenuItem>
                </Items>
            </asp:Menu>
            <br />
            <asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex="0">
                <asp:View ID="Tab1" runat="server">
                    <asp:Panel runat="server" ID="PnlDt">
                        <asp:Button class="bitbtn btnadd" runat="server" ID="btnAdddt" Text="Add" ValidationGroup="Input" />
                        <div style="border: 0px  solid; width: 100%; height: 100%; overflow: auto;">
                            <asp:GridView ID="GridDt" runat="server" AutoGenerateColumns="false" ShowFooter="True">
                                <HeaderStyle CssClass="GridHeader" />
                                <RowStyle CssClass="GridItem" Wrap="false" />
                                <AlternatingRowStyle CssClass="GridAltItem" />
                                <PagerStyle CssClass="GridPager" />
                                <Columns>
                                    <asp:TemplateField HeaderText="Action">
                                        <ItemTemplate>
                                            <asp:Button ID="btnEdit" runat="server" class="bitbtndt btnedit" Text="Edit" CommandName="Edit" />
                                            <asp:Button ID="btnDelete" runat="server" class="bitbtndt btndelete" Text="Delete"
                                                OnClientClick="return confirm('Sure to delete this data?');" CommandName="Delete" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="DPNo" HeaderStyle-Width="110px" HeaderText="DP No" />
                                    <asp:BoundField DataField="DPDate" HeaderStyle-Width="80px" HeaderText="DP Date" />
                                    <asp:BoundField DataField="DPRate" HeaderStyle-Width="80px" HeaderText="DP Rate" />
                                    <asp:BoundField DataField="Currency" HeaderStyle-Width="80px" HeaderText="Currency" />
                                    <asp:BoundField DataField="DPTotal" HeaderStyle-Width="80px" HeaderText="DP Forex" />
                                    <asp:BoundField DataField="DPPaidTotal" HeaderStyle-Width="80px" HeaderText="Paid Forex" />
                                    <asp:BoundField DataField="BaseForex" HeaderStyle-Width="80px" HeaderText="Base Forex" />
                                    <asp:BoundField DataField="PPnForex" HeaderStyle-Width="80px" HeaderText="PPn Forex" />
                                    <asp:BoundField DataField="TotalForex" HeaderStyle-Width="80px" HeaderText="Total Forex" />
                                    <asp:BoundField DataField="TotalHome" HeaderStyle-Width="80px" HeaderText="Total Home" />
                                    <asp:BoundField DataField="Remark" HeaderStyle-Width="200px" HeaderText="Remark" />
                                </Columns>
                            </asp:GridView>
                        </div>
                        <asp:Button class="bitbtn btnadd" runat="server" ID="btnAddDtke2" Text="Add" ValidationGroup="Input" />
                    </asp:Panel>
                    <asp:Panel runat="server" ID="pnlEditDt" Visible="false">
                        <table>
                            <tr>
                                <td width="50px">
                                    DP
                                </td>
                                <td>
                                    :
                                </td>
                                <td colspan="4">
                                    <asp:TextBox CssClass="TextBox" runat="server" ID="tbDPNoDt" Enabled="false" Width="155px" />
                                    <BDP:BasicDatePicker ID="tbDPDateDt" runat="server" DateFormat="dd MMM yyyy" ReadOnly="true"
                                        ValidationGroup="Input" ButtonImageHeight="19px" ButtonImageWidth="20px" DisplayType="TextBox"
                                        TextBoxStyle-CssClass="TextDate" ShowNoneButton="False">
                                        <TextBoxStyle CssClass="TextDate" />
                                    </BDP:BasicDatePicker>
                                    <asp:Button Class="btngo" ID="btnDPNo" Text="..." runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td width="50px">
                                    Currency
                                </td>
                                <td>
                                    :
                                </td>
                                <td>
                                    <asp:DropDownList CssClass="DropDownList" ID="ddlCurrDt" runat="server" Enabled="false"
                                        Width="60px" />
                                </td>
                            </tr>
                            <tr>
                                <td width="50px">
                                    Pay Rate
                                </td>
                                <td>
                                    :
                                </td>
                                <td>
                                    <asp:TextBox CssClass="TextBox" runat="server" ID="tbRateDt" Enabled="false" Width="65px" />
                                </td>
                                
                                <td>
                                    DP Rate
                                </td>
                                <td>
                                    :
                                </td>
                                <td>
                                    <asp:TextBox CssClass="TextBox" runat="server" ID="tbDPRate" Enabled="false" Width="65px" />
                                </td>
                                
                                <td>
                                    PPn Rate
                                </td>
                                <td>
                                    :
                                </td>
                                <td>
                                    <asp:TextBox CssClass="TextBox" runat="server" ID="tbPPnRateDt" Enabled="false" Width="65px" />
                                </td>
                            </tr>
                        </table>
                        <table>
                            <tr>
                                <td width="50px">
                                </td>
                                <td>
                                </td>
                                <td style="background-color: Silver; text-align: center">
                                    D P
                                </td>
                                <td style="background-color: Silver; text-align: center">
                                    Paid
                                </td>
                                <td style="background-color: Silver; text-align: center">
                                    To Retur
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Total
                                </td>
                                <td>
                                    :
                                </td>
                                <td>
                                    <asp:TextBox CssClass="TextBoxR" runat="server" ID="tbTotalDPDt" />
                                </td>
                                <td>
                                    <asp:TextBox CssClass="TextBoxR" runat="server" ID="TbTotalPaidDt" />
                                </td>
                                <td>
                                    <asp:TextBox CssClass="TextBoxR" runat="server" ID="tbTotalForexDt" />
                                    <asp:TextBox CssClass="TextBoxR" runat="server" ID="tbTotalHomeDt" Visible="True" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Base
                                </td>
                                <td>
                                    :
                                </td>
                                <td>
                                    <asp:TextBox CssClass="TextBoxR" runat="server" ID="tbBaseDPDt" />
                                </td>
                                <td>
                                    <asp:TextBox CssClass="TextBoxR" runat="server" ID="tbBasePaidDt" />
                                </td>
                                <td>
                                    <asp:TextBox CssClass="TextBox" runat="server" ID="tbBaseForexDt" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    PPN
                                </td>
                                <td>
                                    :
                                </td>
                                <td>
                                    <asp:TextBox CssClass="TextBoxR" runat="server" ID="tbPPNDPDt" />
                                </td>
                                <td>
                                    <asp:TextBox CssClass="TextBoxR" runat="server" ID="tbPPNPaidDt" />
                                </td>
                                <td>
                                    <asp:TextBox CssClass="TextBox" runat="server" ID="tbPPnForexDt" />
                                </td>
                            </tr>
                        </table>
                        <table>
                            <tr>
                                <td width="50px">
                                    Remark
                                </td>
                                <td>
                                    :
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="tbRemarkDt" CssClass="TextBox" Width="365px" MaxLength="255"
                                        TextMode="MultiLine" />
                                </td>
                            </tr>
                        </table>
                        <br />
                        <asp:Button ID="btnSaveDt" runat="server" class="bitbtndt btnsave" Text="Save" />
                        <asp:Button ID="btnCancelDt" runat="server" class="bitbtndt btncancel" Text="Cancel" />
                    </asp:Panel>
                </asp:View>
                <asp:View ID="Tab2" runat="server">
                    <asp:Panel ID="pnlDt2" runat="server">
                        <asp:Button class="bitbtn btnadd" runat="server" ID="btnAddDt2" Text="Add" ValidationGroup="Input" />
                        <div style="border: 0px  solid; width: 100%; height: 100%; overflow: auto;">
                            <asp:GridView ID="GridDt2" runat="server" AutoGenerateColumns="False" ShowFooter="True">
                                <HeaderStyle CssClass="GridHeader" />
                                <RowStyle CssClass="GridItem" Wrap="false" />
                                <AlternatingRowStyle CssClass="GridAltItem" />
                                <PagerStyle CssClass="GridPager" />
                                <Columns>
                                    <asp:TemplateField HeaderText="Action">
                                        <ItemTemplate>
                                            <asp:Button ID="btnEdit" runat="server" class="bitbtndt btnedit" Text="Edit" CommandName="Edit" />
                                            <asp:Button ID="btnDelete" runat="server" class="bitbtndt btndelete" Text="Delete"
                                                OnClientClick="return confirm('Sure to delete this data?');" CommandName="Delete" />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:Button ID="btnUpdate" runat="server" class="bitbtndt btnsave" Text="Save" CommandName="Update" />
                                            <asp:Button ID="btnCancel" runat="server" class="bitbtndt btncancel" Text="Cancel"
                                                CommandName="Cancel" />
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="ItemNo" HeaderText="No" />
                                    <asp:BoundField DataField="PaymentName" HeaderStyle-Width="150px" HeaderText="Payment">
                                        <HeaderStyle Width="150px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="DocumentNo" HeaderStyle-Width="150px" HeaderText="Document No">
                                        <HeaderStyle Width="150px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Reference" HeaderText="Voucher No" HeaderStyle-Width="120px" />
                                    <asp:BoundField DataField="GiroDate" HeaderText="Giro Date" />
                                    <asp:BoundField DataField="DueDate" HeaderStyle-Width="80px" HeaderText="Due Date" />
                                    <asp:BoundField DataField="BankPaymentName" HeaderStyle-Width="150px" HeaderText="Bank Payment">
                                        <HeaderStyle Width="150px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Currency" HeaderStyle-Width="80px" HeaderText="Currency">
                                        <HeaderStyle Width="80px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="PaymentForex" HeaderStyle-Width="80px" HeaderText="Payment Forex">
                                        <HeaderStyle Width="80px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="PaymentHome" HeaderStyle-Width="80px" HeaderText="Payment Home">
                                        <HeaderStyle Width="80px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="ChargeHome" HeaderStyle-Width="80px" HeaderText="Charge Home">
                                        <HeaderStyle Width="80px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Remark" HeaderText="Remark" />
                                </Columns>
                            </asp:GridView>
                        </div>
                        <asp:Button class="bitbtn btnadd" runat="server" ID="btnAddDt2Ke2" Text="Add" ValidationGroup="Input" />
                    </asp:Panel>
                    <asp:Panel runat="server" ID="pnlEditDt2" Visible="false">
                        <table>
                            <tr>
                                <td>
                                    Item No
                                </td>
                                <td>
                                    :
                                </td>
                                <td colspan="4">
                                    <asp:Label ID="lbItemNoDt2" runat="server" Text="itemmm" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Payment Type
                                </td>
                                <td>
                                    :
                                </td>
                                <td>
                                    <asp:DropDownList CssClass="DropDownList" ID="ddlPayTypeDt2" runat="server" Width="190px"
                                        AutoPostBack="true" />
                                    <asp:TextBox CssClass="TextBox" ID="tbFgModeDt2" runat="server" Visible="false" />
                                </td>
                                <td>
                                    Giro Date
                                </td>
                                <td>
                                    :
                                </td>
                                <td>
                                    <BDP:BasicDatePicker ID="tbGiroDateDt2" runat="server" DateFormat="dd MMM yyyy" ReadOnly="true"
                                        ValidationGroup="Input" ButtonImageHeight="19px" ButtonImageWidth="20px" DisplayType="TextBoxAndImage"
                                        TextBoxStyle-CssClass="TextDate" ShowNoneButton="False">
                                        <TextBoxStyle CssClass="TextDate" />
                                    </BDP:BasicDatePicker>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Document No
                                </td>
                                <td>
                                    :
                                </td>
                                <td>
                                    <asp:TextBox CssClass="TextBox" runat="server" ID="tbDocumentNoDt2" Width="157px" />
                                </td>
                                <td>
                                    Payment Date
                                </td>
                                <td>
                                    :
                                </td>
                                <td>
                                    <BDP:BasicDatePicker ID="tbPaymentDateDt2" runat="server" DateFormat="dd MMM yyyy"
                                        ReadOnly="true" ValidationGroup="Input" ButtonImageHeight="19px" ButtonImageWidth="20px"
                                        DisplayType="TextBoxAndImage" TextBoxStyle-CssClass="TextDate" ShowNoneButton="False">
                                        <TextBoxStyle CssClass="TextDate" />
                                    </BDP:BasicDatePicker>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Voucher No
                                </td>
                                <td>
                                    :
                                </td>
                                <td>
                                    <asp:TextBox CssClass="TextBox" runat="server" ID="tbVoucherNo" Width="157px" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Bank Payment
                                </td>
                                <td>
                                    :
                                </td>
                                <td>
                                    <asp:DropDownList CssClass="DropDownList" ID="ddlBankPaymentDt2" runat="server" />
                                </td>
                                <td>
                                    Due Date
                                </td>
                                <td>
                                    :
                                </td>
                                <td>
                                    <BDP:BasicDatePicker ID="tbDueDateDt2" runat="server" DateFormat="dd MMM yyyy" ReadOnly="true"
                                        ButtonImageHeight="19px" ButtonImageWidth="20px" DisplayType="TextBoxAndImage"
                                        TextBoxStyle-CssClass="TextDate" ShowNoneButton="False">
                                        <TextBoxStyle CssClass="TextDate" />
                                    </BDP:BasicDatePicker>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Currency
                                </td>
                                <td>
                                    :
                                </td>
                                <td colspan="4">
                                    <table>
                                        <tr style="background-color: Silver; text-align: center">
                                            <td>
                                                Payment
                                            </td>
                                            <td>
                                                Rate
                                            </td>
                                            <td>
                                                Charge
                                            </td>
                                            <td>
                                                Rate
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:DropDownList CssClass="DropDownList" ID="ddlCurrDt2" runat="server" Enabled="false"
                                                    Width="60px" />
                                            </td>
                                            <td>
                                                <asp:TextBox CssClass="TextBox" runat="server" ID="tbRateDt2" Width="65px" />
                                            </td>
                                            <td>
                                                <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlChargeCurrDt2" Width="60px"
                                                    AutoPostBack="True" />
                                            </td>
                                            <td>
                                                <asp:TextBox CssClass="TextBox" runat="server" ID="tbChargeRateDt2" Width="65px" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Nominal
                                </td>
                                <td>
                                    :
                                </td>
                                <td colspan="3">
                                    <table>
                                        <tr style="background-color: Silver; text-align: center">
                                            <td>
                                                <asp:Label ID="lbPayForex" CssClass="TextBox" runat="server" Text="Payment Forex" /></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lbPayHome" CssClass="TextBox" runat="server" Text="Payment Home" />
                                            </td>
                                            <td>
                                                <asp:Label ID="lbChargeForex" CssClass="TextBox" runat="server" Text="Charge Forex" />
                                            </td>
                                            <td>
                                                <asp:Label ID="lbChargeHome" CssClass="TextBox" runat="server" Text="Charge Home" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:TextBox CssClass="TextBox" runat="server" ID="tbPaymentForexDt2" Width="80px" />
                                            </td>
                                            <td>
                                                <asp:TextBox CssClass="TextBoxR" runat="server" ID="tbPaymentHomeDt2" Width="80px" />
                                            </td>
                                            <td>
                                                <asp:TextBox CssClass="TextBox" runat="server" ID="tbChargeForexDt2" Width="80px" />
                                            </td>
                                            <td>
                                                <asp:TextBox CssClass="TextBoxR" runat="server" ID="tbChargeHomeDt2" Width="80px" />
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
                                    :
                                </td>
                                <td colspan="4">
                                    <asp:TextBox runat="server" ID="tbRemarkDt2" CssClass="TextBox" Width="365px" MaxLength="255"
                                        TextMode="MultiLine" />
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
            <asp:Button ID="btnSaveAll" runat="server" class="bitbtndt btnsavenew" Text="Save & New"
                ValidationGroup="Input" Width="90" />
            <asp:Button ID="btnSaveTrans" runat="server" class="bitbtndt btnsave" Text="Save"
                ValidationGroup="Input" />
            <asp:Button ID="btnBack" runat="server" class="bitbtndt btncancel" Text="Cancel"
                ValidationGroup="Input" />
            <asp:Button ID="btnHome" runat="server" class="bitbtndt btnback" Text="Home" />
        </asp:Panel>
    </div>
    <asp:Label runat="server" ID="lbStatus" ForeColor="Red" />
    </form>
</body>
</html>
