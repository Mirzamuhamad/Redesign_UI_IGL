<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TrDPCustList.aspx.vb" Inherits="Transaction_TrDPCustList_TrDPCustList " %>

<%@ Register Assembly="BasicFrame.WebControls.BasicDatePicker" Namespace="BasicFrame.WebControls"
    TagPrefix="BDP" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>DP Customer Invoice</title>

    <script src="../../Function/OpenDlg.js" type="text/javascript"></script>

    <script src="../../Function/Function.JS" type="text/javascript"></script>

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
        
        function setformat(){   
          try
          { 
            var _tempbaseforex = parseFloat(document.getElementById("tbBaseForex").value.replace(/\$|\,/g,""));
            var _tempppn = parseFloat(document.getElementById("tbPPN").value.replace(/\$|\,/g,""));
            var _tempppnforex = parseFloat(document.getElementById("tbPPNForex").value.replace(/\$|\,/g,""));
            var _temptotalforex = parseFloat(document.getElementById("tbTotalForex").value.replace(/\$|\,/g,""));
            var _tempRate = parseFloat(document.getElementById("tbRate").value.replace(/\$|\,/g,""));
            var _tempPPnRate = parseFloat(document.getElementById("tbPPNRate").value.replace(/\$|\,/g,""));
            
            document.getElementById("tbPPN").value = setdigit(_tempppn, '<%=ViewState("DigitPercent")%>');
            document.getElementById("tbBaseForex").value = setdigit(_tempbaseforex, '<%=ViewState("DigitCurr")%>');            
            document.getElementById("tbPPNForex").value = setdigit(_tempppnforex, '<%=ViewState("DigitCurr")%>');
            document.getElementById("tbTotalForex").value = setdigit(_temptotalforex, '<%=ViewState("DigitCurr")%>');
            document.getElementById("tbRate").value = setdigit(_tempRate,'<%=ViewState("DigitRate")%>');
            document.getElementById("tbPPNRate").value = setdigit(_tempPPnRate,'<%=ViewState("DigitRate")%>');                       
          }catch (err){
            alert(err.description);
          }
        }
        function setformatdt()
        {
         try
         {  
//         var Qty = parseFloat(document.getElementById("tbQty").value.replace(/\$|\,/g,""));
         var Price = parseFloat(document.getElementById("tbPriceForex").value.replace(/\$|\,/g,""));
            //document.getElementById("tbQty").value = setdigit(Qty,'<%=VIEWSTATE("DigitQty")%>');                    
            document.getElementById("tbPriceForex").value = setdigit(Price,4);
                        
        }catch (err){
            alert(err.description);
          }      
        }
    </script>

    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .style1
        {
            width: 3px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div class="Content">
        <div class="H1">
            DP Customer Invoice</div>
        <hr style="color: Blue" />
        <asp:Panel runat="server" ID="PnlHd">
            <table>
                <tr>
                    <td style="width: 100px; text-align: right">
                        Quick Search :
                    </td>
                    <td>
                        <asp:TextBox CssClass="TextBox" runat="server" ID="tbFilter" />
                        <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlField">
                            <asp:ListItem Value="TransNmbr" Selected="True">DP No</asp:ListItem>
                            <asp:ListItem Value="Status">Status</asp:ListItem>
                            <asp:ListItem Value="dbo.FormatDate(TransDate)">DP Date</asp:ListItem>
                            <asp:ListItem Value="FgReport">Report</asp:ListItem>
                            <asp:ListItem Value="Customer_Name">Customer</asp:ListItem>
                            <asp:ListItem Value="Attn">Attention</asp:ListItem>
                            <asp:ListItem Value="SONo">SO No</asp:ListItem>
                            <asp:ListItem Value="PPNNo">PPN No</asp:ListItem>
                            <asp:ListItem Value="dbo.FormatDate(PPNDate)">PPN Date</asp:ListItem>
                            <asp:ListItem Value="PPNRate">PPN Rate</asp:ListItem>
                            <asp:ListItem Value="Currency">Currency</asp:ListItem>
                            <asp:ListItem Value="ForexRate">Rate</asp:ListItem>
                            <asp:ListItem Value="BaseForex">Base Forex</asp:ListItem>
                            <asp:ListItem Value="PPN">PPN</asp:ListItem>
                            <asp:ListItem Value="PPNForex">PPN Forex</asp:ListItem>
                            <asp:ListItem Value="TotalForex">Total Forex</asp:ListItem>
                            <asp:ListItem Value="Remark">Remark</asp:ListItem>
                            <asp:ListItem Value="CostCtr">Cost Center</asp:ListItem>
                        </asp:DropDownList>
                        <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlRange">
                        </asp:DropDownList>
                        <asp:Button class="bitbtn btnsearch" runat="server" ID="btnSearch" Text="Search" />
                        <asp:Button class="btngo" runat="server" ID="btnExpand" Text="..." />
                    </td>
                    <td>
                        <asp:LinkButton ID="lkbAdvanceSearch" runat="server" Text="Advanced Search" />
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
                                <asp:ListItem Value="TransNmbr" Selected="True">DP No</asp:ListItem>
                                <asp:ListItem Value="Status">Status</asp:ListItem>
                                <asp:ListItem Value="dbo.FormatDate(TransDate)">DP Date</asp:ListItem>
                                <asp:ListItem Value="FgReport">Report</asp:ListItem>
                                <asp:ListItem Value="Customer_Name">Customer</asp:ListItem>
                                <asp:ListItem Value="Attn">Attention</asp:ListItem>
                                <asp:ListItem Value="SONo">SO No</asp:ListItem>
                                <asp:ListItem Value="PPNNo">PPN No</asp:ListItem>
                                <asp:ListItem Value="dbo.FormatDate(PPNDate)">PPN Date</asp:ListItem>
                                <asp:ListItem Value="PPNRate">PPN Rate</asp:ListItem>
                                <asp:ListItem Value="Currency">Currency</asp:ListItem>
                                <asp:ListItem Value="ForexRate">Rate</asp:ListItem>
                                <asp:ListItem Value="BaseForex">Base Forex</asp:ListItem>
                                <asp:ListItem Value="PPN">PPN</asp:ListItem>
                                <asp:ListItem Value="PPNForex">PPN Forex</asp:ListItem>
                                <asp:ListItem Value="TotalForex">Total Forex</asp:ListItem>
                                <asp:ListItem Value="Remark">Remark</asp:ListItem>
                                <asp:ListItem Value="CostCtr">Cost Center</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <br />
            <asp:Button class="bitbtn btnadd" runat="server" ID="BtnAdd" Text="Add" />
            &nbsp &nbsp &nbsp
            <asp:DropDownList CssClass="DropDownList" ID="ddlCommand" runat="server" Visible="false" />
            <asp:Button class="btngo" runat="server" ID="BtnGo" Text="G" Visible="False" />
            <br />
            <div style="border: 0px  solid; height: 100%; overflow: auto;">
                <asp:GridView ID="GridView1" runat="server" AllowPaging="True" AllowSorting="false"
                    AutoGenerateColumns="false" CssClass="Grid">
                    <HeaderStyle CssClass="GridHeader" />
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
                        <asp:TemplateField ItemStyle-Width="80px">
                            <ItemTemplate>
                                <asp:DropDownList ID="ddl" runat="server" CssClass="DropDownList">
                                    <asp:ListItem Selected="True" Text="View" />
                                    <asp:ListItem Text="Edit" />
                                    <asp:ListItem Text="Delete" />
                                    <asp:ListItem Text="Print" />
                                    <asp:ListItem Text="Print Tax" />
                                </asp:DropDownList>
                                <asp:Button class="btngo" runat="server" ID="BtnGo" Text="G" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>"
                                    CommandName="Go" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="TransNmbr" HeaderStyle-Width="320px" HeaderText="DP No"
                            SortExpression="Nmbr" />
                        <asp:BoundField DataField="Status" HeaderStyle-Width="10px" HeaderText="Status" SortExpression="Status" />
                        <asp:BoundField DataField="TransDate" HtmlEncode="true" DataFormatString="{0:dd MMM yyyy}"
                            HeaderStyle-Width="80px" HeaderText="Date" SortExpression="TransDate" />
                        <asp:BoundField DataField="FgReport" HeaderStyle-Width="10px" HeaderText="Report"
                            SortExpression="FgReport" />
                        <asp:BoundField DataField="Customer_Name" HeaderStyle-Width="250px" HeaderText="Customer"
                            SortExpression="Customer_Name" />
                        <asp:BoundField DataField="Attn" HeaderStyle-Width="250px" HeaderText="Attention"
                            SortExpression="Attn" />
                        <asp:BoundField DataField="SONo" HeaderStyle-Width="150px" HeaderText="SO No" SortExpression="SONo" />
                        <asp:BoundField DataField="CostCtr" HeaderStyle-Width="200px" HeaderText="Cost Center"
                            SortExpression="CostCtr" />
                        <asp:BoundField DataField="PPNNo" HeaderStyle-Width="150px" HeaderText="PPN No" SortExpression="PPNNo" />
                        <asp:BoundField DataField="PPNDate" HtmlEncode="true" DataFormatString="{0:dd MMM yyyy}"
                            HeaderStyle-Width="80px" HeaderText="PPN Date" SortExpression="PPNDate" />
                        <asp:BoundField DataField="PPNRate" HeaderStyle-Width="80px" HeaderText="PPN Rate"
                            DataFormatString="{0:#,##0.####}" SortExpression="PPNRate" />
                        <asp:BoundField DataField="Currency" HeaderStyle-Width="15px" HeaderText="Currency"
                            SortExpression="Currency" />
                        <asp:BoundField DataField="BaseForex" DataFormatString="{0:#,##0.00}" ItemStyle-HorizontalAlign="Right"
                            HeaderStyle-Width="80px" HeaderText="Base Forex" SortExpression="BaseForex" />
                        <asp:BoundField DataField="PPn" DataFormatString="{0:#,##0.####}" ItemStyle-HorizontalAlign="Right"
                            HeaderStyle-Width="50px" HeaderText="PPn" SortExpression="PPn" />
                        <asp:BoundField DataField="PPnForex" DataFormatString="{0:#,##0.00}" ItemStyle-HorizontalAlign="Right"
                            HeaderStyle-Width="80px" HeaderText="PPn Forex" SortExpression="PPnForex" />
                        <asp:BoundField DataField="TotalForex" DataFormatString="{0:#,##0.00}" ItemStyle-HorizontalAlign="Right"
                            HeaderStyle-Width="80px" HeaderText="Total Forex" SortExpression="TotalForex" />
                        <asp:BoundField DataField="Remark" HeaderStyle-Width="200px" HeaderText="Remark"
                            SortExpression="Remark" />
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
        <asp:Panel ID="pnlInput" DefaultButton="btnSaveTrans" runat="server" Visible="false">
            <table>
                <tr>
                    <td>
                        DP No
                    </td>
                    <td class="style1">
                        :
                    </td>
                    <td>
                        <asp:TextBox ID="tbTransNo" ValidationGroup="Input" runat="server" CssClass="TextBoxR"
                            Enabled="False" ReadOnly="True" Width="150px" />
                        <%--&nbsp Report : &nbsp   
                     <asp:DropDownList AutoPostBack="true" CssClass="DropDownList" ID="ddlReport" runat="server" Enabled="false" >
                        <asp:ListItem Selected="True">Y</asp:ListItem>
                        <asp:ListItem>N</asp:ListItem>
                    </asp:DropDownList>--%>
                        &nbsp &nbsp Date : &nbsp
                        <BDP:BasicDatePicker ID="tbTransDate" runat="server" DateFormat="dd MMM yyyy" ShowNoneButton="false"
                            ReadOnly="true" ValidationGroup="Input" ButtonImageHeight="19px" ButtonImageWidth="20px"
                            DisplayType="TextBoxAndImage" TextBoxStyle-CssClass="TextDate" AutoPostBack="True">
                            <TextBoxStyle CssClass="TextDate" />
                        </BDP:BasicDatePicker>
                    </td>
                </tr>
                <tr>
                    <td>
                        Customer
                    </td>
                    <td class="style1">
                        :
                    </td>
                    <td>
                        <asp:TextBox ID="tbCustCode" ValidationGroup="Input" AutoPostBack="true" Width="100"
                            runat="server" CssClass="TextBox" />
                        <asp:TextBox ID="tbCustName" Width="300" runat="server" ReadOnly="true" CssClass="TextBoxR" />
                        <asp:Button Class="btngo" ID="btnCust" Text="..." runat="server" ValidationGroup="Input" />
                        <asp:Label ID="Label11" runat="server" ForeColor="#FF3300" Text="*"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        Cust. Tax Address
                    </td>
                    <td>
                        :
                    </td>
                    <td colspan="4">
                        <asp:TextBox runat="server" ID="tbCustTaxAddress" CssClass="TextBoxMulti" ValidationGroup="Input"
                            Width="365px" MaxLength="255" TextMode="MultiLine" />
                        <asp:Button ID="btnCustTax" runat="server" class="btngo" Text="..." ValidationGroup="Input" />
                    </td>
                </tr>
                <tr>
                    <td>
                        Cust. Tax NPWP
                    </td>
                    <td>
                        :
                    </td>
                    <td colspan="4">
                        <asp:TextBox ID="tbCustTaxNPWP" runat="server" CssClass="TextBox" ValidationGroup="Input"
                            Width="225px" />
                    </td>
                </tr>
                <tr>
                    <td>
                        Attention
                    </td>
                    <td class="style1">
                        :
                    </td>
                    <td>
                        <asp:TextBox ID="tbAttn" ValidationGroup="Input" runat="server" CssClass="TextBox"
                            Width="300" />
                    </td>
                </tr>
                <tr>
                    <td>
                        SO No
                    </td>
                    <td class="style1">
                        :
                    </td>
                    <td>
                        <asp:TextBox ID="tbSONo" ValidationGroup="Input" runat="server" CssClass="TextBoxR"
                            Width="165" Enabled="False" ReadOnly="True" />
                        <asp:Button Class="btngo" ID="btnSO" Text="..." runat="server" ValidationGroup="Input" />
                        <%--<asp:Label ID="Label1" runat="server" ForeColor="#FF3300" Text="*"></asp:Label>--%>
                    </td>
                </tr>
                <tr>
                    <td>
                        Cost Center
                    </td>
                    <td class="style1">
                        :
                    </td>
                    <%--<td><asp:TextBox ID="tbCostCtrCode" ValidationGroup="Input" AutoPostBack="true" Width="100" runat="server" CssClass="TextBox" />
                <asp:TextBox ID="tbCostCtrName" Width="150" runat="server" 
                        ReadOnly="true" CssClass="TextBoxR" />
                <asp:Button Class="btngo" ID="btnCostCtr" Text="..." runat="server" ValidationGroup="Input" />--%>
                    <td>
                        <asp:DropDownList ID="ddlCostCtr" ValidationGroup="Input" runat="server" CssClass="DropDownList" />
                        <asp:Label ID="Label1" runat="server" ForeColor="#FF3300" Text="*"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:LinkButton ID="lbCurr" runat="server" Text="Currency - Rate" />
                    </td>
                    <td class="style1">
                        :
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlCurr" ValidationGroup="Input" AutoPostBack="true" runat="server"
                            CssClass="DropDownList" Width="90px" />
                        <asp:TextBox ID="tbRate" ValidationGroup="Input" runat="server" Width="75px" CssClass="TextBox" />
                        &nbsp &nbsp DP &nbsp : &nbsp
                        <asp:TextBox ID="tbDP" runat="server" AutoPostBack="true" ValidationGroup="Input"
                            Width="45px" CssClass="TextBox" />
                        %
                    </td>
                </tr>
                <tr>
                    <td>
                        PPN
                    </td>
                    <td class="style1">
                        :
                    </td>
                    <td>
                        <table>
                            <tr style="background-color: Silver; text-align: center">
                                <td>
                                    No
                                </td>
                                <td>
                                    Date
                                </td>
                                <td>
                                    Rate
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:TextBox ID="tbPPNNo" ValidationGroup="Input" runat="server" CssClass="TextBox" />
                                </td>
                                <td>
                                    <BDP:BasicDatePicker ID="tbPPNDate" ValidationGroup="Input" runat="server" DateFormat="dd MMM yyyy"
                                        ButtonImageHeight="19px" ButtonImageWidth="20px" DisplayType="TextBoxAndImage"
                                        ReadOnly="true" TextBoxStyle-CssClass="TextDate">
                                    </BDP:BasicDatePicker>
                                </td>
                                <td>
                                    <asp:TextBox ID="tbPPNRate" ValidationGroup="Input" runat="server" CssClass="TextBox" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        Amount
                    </td>
                    <td class="style1">
                        :
                    </td>
                    <td>
                        <table>
                            <tr style="background-color: Silver; text-align: center">
                                <td>
                                    Base Forex
                                </td>
                                <td>
                                    PPN %
                                </td>
                                <td>
                                    PPN Forex
                                </td>
                                <td>
                                    Total Forex
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:TextBox ID="tbBaseForex" ValidationGroup="Input" runat="server" CssClass="TextBox"
                                        Enable="false" />
                                </td>
                                <td>
                                    <asp:TextBox ID="tbPPN" ValidationGroup="Input" runat="server" CssClass="TextBox" />
                                </td>
                                <td>
                                    <asp:TextBox ID="tbPPNForex" runat="server" CssClass="TextBoxR" />
                                </td>
                                <td>
                                    <asp:TextBox ID="tbTotalForex" runat="server" CssClass="TextBoxR" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        Remark
                    </td>
                    <td class="style1">
                        :
                    </td>
                    <td>
                        <asp:TextBox ID="tbRemark" ValidationGroup="Input" Width="358px" runat="server" CssClass="TextBox" />
                        <asp:Button ID="BtnGetData" runat="server" class="bitbtn btnsearch" Text="Get Data"
                            Visible="false" />
                    </td>
                </tr>
            </table>
            <br />
            <br />
            <div style="font-size: medium; color: Blue;">
                Detail</div>
            <hr style="color: Blue" />
            <asp:Panel ID="pnlDt" runat="server">
                <asp:Button ID="btnAddDt" runat="server" class="bitbtn btnadd" Text="Add" ValidationGroup="Input" />
                <div style="border: 0px  solid; width: 100%; height: 100%; overflow: auto;">
                    <asp:GridView ID="GridDt" runat="server" AutoGenerateColumns="false" ShowFooter="True">
                        <HeaderStyle CssClass="GridHeader" />
                        <RowStyle CssClass="GridItem" Wrap="false" />
                        <AlternatingRowStyle CssClass="GridAltItem" />
                        <PagerStyle CssClass="GridPager" />
                        <Columns>
                            <asp:TemplateField HeaderText="Action">
                                <ItemTemplate>
                                    <asp:Button ID="btnEdit" runat="server" class="bitbtn btnedit" CommandName="Edit"
                                        Text="Edit" />
                                    <asp:Button ID="btnDelete" runat="server" class="bitbtndt btndelete" CommandName="Delete"
                                        OnClientClick="return confirm('Sure to delete this data?');" Text="Delete" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="ItemNo" HeaderText="No" />
                            <asp:BoundField DataField="ProductName" HeaderStyle-Width="150px" HeaderText="Product Name" />
                            <asp:BoundField DataField="Qty" DataFormatString="{0:#,##0.00}" HeaderStyle-Width="80px"
                                HeaderText="Qty" ItemStyle-HorizontalAlign="Right" />
                            <asp:BoundField DataField="Unit" HeaderStyle-Width="80px" HeaderText="Unit" />
                            <asp:BoundField DataField="PriceForex" DataFormatString="{0:#,##0.00}" HeaderStyle-Width="80px"
                                HeaderText="Price Forex" ItemStyle-HorizontalAlign="Right" />
                            <asp:BoundField DataField="AmountForex" DataFormatString="{0:#,##0.00}" HeaderStyle-Width="80px"
                                HeaderText="Amount Forex" ItemStyle-HorizontalAlign="Right" />
                        </Columns>
                    </asp:GridView>
                </div>
                <asp:Button ID="btnAddDt2" runat="server" class="bitbtn btnadd" Text="Add" ValidationGroup="Input" />
            </asp:Panel>
            <asp:Panel ID="pnlEditDt" runat="server" Visible="false">
                <table>
                    <tr>
                        <td>
                            Item No
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                            <asp:Label ID="lbItemNo" runat="server" Text="itemmm" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Product
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="tbProductName" CssClass="TextBoxMulti" Width="365px"
                                MaxLength="255" TextMode="MultiLine" />
                            <%--<asp:TextBox ID="tbProductName" runat="server" CssClass="TextBox" Width="250px" AutoPostBack ="True" />--%>
                            <asp:Button ID="btnProduct" runat="server" class="btngo" Text="..." ValidationGroup="Input" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td>
                            <table>
                                <tr style="background-color: Silver; text-align: center">
                                    <td>
                                        Qty
                                    </td>
                                    <td>
                                        Unit
                                    </td>
                                    <td>
                                        Price
                                    </td>
                                    <td>
                                        Amount
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="tbQty" runat="server" CssClass="TextBox" AutoPostBack="true" />
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlUnit" runat="server" CssClass="DropDownList">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="tbPriceForex" ValidationGroup="Input" runat="server" Width="120px"
                                            CssClass="TextBox" AutoPostBack="True" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="tbAmountForex" ValidationGroup="Input" runat="server" Width="120px"
                                            CssClass="TextBoxR" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                <br />
                <asp:Button ID="btnSaveDt" runat="server" class="bitbtndt btnsave" Text="Save" />
                &nbsp;
                <asp:Button ID="btnCancelDt" runat="server" class="bitbtndt btncancel" Text="Cancel" />
            </asp:Panel>
            <br />
            <br />
            <asp:Button ID="btnSaveAll" runat="server" class="bitbtndt btnsave" Text="Save &amp; New"
                ValidationGroup="Input" Width="97px" />
            &nbsp;
            <asp:Button ID="btnSaveTrans" runat="server" class="bitbtndt btnsave" Text="Save"
                ValidationGroup="Input" />
            &nbsp;
            <asp:Button ID="btnBack" runat="server" class="bitbtndt btncancel" Text="Cancel"
                ValidationGroup="Input" />
            &nbsp;
            <asp:Button ID="btnHome" runat="server" class="bitbtndt btncancel" Text="Home" />
        </asp:Panel>
        <br />
        <br />
    </div>
    <asp:Label ID="lbStatus" runat="server" ForeColor="red" />
    </form>
</body>
</html>
