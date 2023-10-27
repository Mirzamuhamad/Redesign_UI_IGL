<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TrFNSIFreight.aspx.vb" Inherits="Transaction_TrFNSIFreight_TrFNSIFreight" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="BasicFrame.WebControls.BasicDatePicker" Namespace="BasicFrame.WebControls"
    TagPrefix="BInv" %>
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
                    TNstr = TNstr.toFixed(digit);                
                    nStr = TNstr;        
                    nStr += '';
                    x = nStr.split('.');
                    x1 = x[0];
                    x2 = x.length > 1 ? '.' + x[1] : '';
                    var rgx = /(\d+)(\d{3})/;
                    
                    while (rgx.test(x1)) 
                    {
                        x1 = x1.replace(rgx, '$1' + ',' + '$2');
                    }
                    
                    return x1 + x2;
	            }
	            catch (err)
	            {
                    alert(err.description);
                }  
        }
        
        function HitungInv(_prmA, _prmB, _prmY, _prmResult)
        {   
            try
            {
                var _tempA = parseFloat(_prmA.value.replace(/\$|\,/g,""));
                if(isNaN(_tempA) == true)
                {
                    _tempA = 0;
                }            
                
                var _tempB = parseFloat(_prmB.value.replace(/\$|\,/g,""));
                if(isNaN(_tempB) == true)
                {
                    _tempB = 0;
                }            
                
                var _tempY = parseFloat(_prmY.value.replace(/\$|\,/g,""));
                if(isNaN(_tempY) == true)
                {
                   _tempY = 0;
                }            
                var _tempResult = (_tempA - _tempB) / _tempY;
        
                _prmA.value = addCommas(_tempA); 
                _prmB.value = addCommas(_tempB);                             
                _prmY.value = addCommas(_tempY);   
                _prmResult.value = addCommas(_tempResult);               
            }
            catch (err)
            {
                alert(err.description);
            }
        }
    
        function setformat()
        {   
            try
            { 
                var _tempbaseforex = parseFloat(document.getElementById("tbBaseForex").value.replace(/\$|\,/g,""));
                //var _tempppn = parseFloat(document.getElementById("tbPPN").value.replace(/\$|\,/g,""));
                var _tempppnforex = parseFloat(document.getElementById("tbPPNForex").value.replace(/\$|\,/g,""));
                var _temptotalforex = parseFloat(document.getElementById("tbTotalForex").value.replace(/\$|\,/g,""));
                var _tempRate = parseFloat(document.getElementById("tbRate").value.replace(/\$|\,/g,""));
                //var _tempPPnRate = parseFloat(document.getElementById("tbPPNRate").value.replace(/\$|\,/g,""));
            
                _temptotalforex = _tempbaseforex + _tempppnforex;
                        
                // document.getElementById("tbPPN").value = setdigit(_tempppn, '<%=ViewState("DigitPercent")%>');
                document.getElementById("tbBaseForex").value = setdigit(_tempbaseforex, '<%=ViewState("DigitCurr")%>');            
                document.getElementById("tbPPNForex").value = setdigit(_tempppnforex, '<%=ViewState("DigitCurr")%>');
                document.getElementById("tbTotalForex").value = setdigit(_temptotalforex, '<%=ViewState("DigitCurr")%>');
                document.getElementById("tbRate").value = setdigit(_tempRate,'<%=ViewState("DigitRate")%>');
                //document.getElementById("tbPPNRate").value = setdigit(_tempPPnRate,'<%=ViewState("DigitRate")%>');                 
            }
            catch (err)
            {
                alert(err.description);
            }
        }
        
        function setformatDt(Change)
        {   
            try
            {
                var AmountForex = parseFloat(document.getElementById("tbAmountRealisasi").value.replace(/\$|\,/g, ""));
                var PPn = parseFloat(document.getElementById("tbPPnDt").value.replace(/\$|\,/g, ""));
                var PPnForex = parseFloat(document.getElementById("tbPPNForexDt").value.replace(/\$|\,/g, ""));
                var Balance = parseFloat(document.getElementById("tbBalance").value.replace(/\$|\,/g, ""));

                //PPnForex = AmountForex; //* (PPn/100);
                //Balance = AmountForex + PPnForex ;

                if (Change == 'AF' )
                {
                    PPnForex = parseFloat(AmountForex) * (parseFloat(PPn) / 100)
                    Balance = parseFloat(AmountForex) + parseFloat(PPnForex) 
                }
                 
                if (Change == 'PP' )
                {
                    if (AmountForex > 0)
                    {
                        PPnForex = parseFloat(AmountForex) * (parseFloat(PPn) / 100)
                    } 
                    else 
                        PPnForex = 0
                        
                    Balance = parseFloat(AmountForex) + parseFloat(PPnForex) 
                }
                
                if (Change == 'PF' )
                {
                    PPn = (parseFloat(PPnForex) / parseFloat(AmountForex)) * 100           
                    Balance = parseFloat(AmountForex) + parseFloat(PPnForex) 
                }
                               
                if (Change == 'TF' )
                {
                    AmountForex = (parseFloat(Balance) * 100) / (100 + parseFloat(PPn))
                    PPnForex = (parseFloat(Balance) * parseFloat(PPn)) / (100 + parseFloat(PPn))
                    PPn = (  parseFloat(PPnForex) / parseFloat(AmountForex)) * 100
                }                                   

                document.getElementById("tbAmountRealisasi").value = setdigit(AmountForex, '<%=ViewState("DigitCurr")%>');
                document.getElementById("tbPPnDt").value = setdigit(PPn, '<%=ViewState("DigitCurr")%>');
                document.getElementById("tbPPNForexDt").value = setdigit(PPnForex, '<%=ViewState("DigitCurr")%>');
                document.getElementById("tbBalance").value = setdigit(Balance, '<%=ViewState("DigitCurr")%>');
            
            }
            catch (err)
            {
                alert(err.description);
            }
        }
    </script>

    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
    <%--    <style type="text/css">
        .style1
        {
            width: 3px;
        }
    </style>--%>
</head>
<body>
    <form id="form1" runat="server">
    <div class="Content">
        <div class="H1">
            Invoice Expedition</div>
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
                            <asp:ListItem Value="TransNmbr" Selected="True">Inv No</asp:ListItem>
                            <asp:ListItem Value="dbo.FormatDate(TransDate)">Inv Date</asp:ListItem>
                            <asp:ListItem>Status</asp:ListItem>
                            <asp:ListItem Value="FgReport">Report</asp:ListItem>
                            <asp:ListItem Value="PurchaseFreight">Purchase Cost No</asp:ListItem>
                            <asp:ListItem Value="SupplierName">Supplier</asp:ListItem>
                            <asp:ListItem Value="Attn">Attention</asp:ListItem>
                            <asp:ListItem Value="PONo">PO No</asp:ListItem>
                            <asp:ListItem Value="SuppInvoice">SuppInvoice</asp:ListItem>
                            <asp:ListItem Value="Term">Term</asp:ListItem>
                            <asp:ListItem Value="Currency">Currency</asp:ListItem>
                            <asp:ListItem Value="ForexRate">Rate</asp:ListItem>
                            <asp:ListItem Value="BaseForex">Base Forex</asp:ListItem>
                            <asp:ListItem Value="PPN">PPN</asp:ListItem>
                            <asp:ListItem Value="PPNForex">PPN Forex</asp:ListItem>
                            <asp:ListItem Value="TotalForex">Total Forex</asp:ListItem>
                            <asp:ListItem Value="Remark">Remark</asp:ListItem>
                            <asp:ListItem Value="SuppInvNo">SuppInvoice Dt</asp:ListItem>
                        </asp:DropDownList>
                        <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlRange">
                        </asp:DropDownList>
                        <asp:Button class="bitbtn btnsearch" runat="server" ID="btnSearch" Text="Search" />
                        <asp:Button class="btngo" runat="server" ID="btnExpand" Text="..." />
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
                        <td>
                            <asp:TextBox CssClass="TextBox" runat="server" ID="tbFilter2" />
                            <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlField2">
                                <asp:ListItem Value="TransNmbr" Selected="True">Inv No</asp:ListItem>
                                <asp:ListItem Value="dbo.FormatDate(TransDate)">Inv Date</asp:ListItem>
                                <asp:ListItem>Status</asp:ListItem>
                                <asp:ListItem Value="FgReport">Report</asp:ListItem>
                                <asp:ListItem Value="PurchaseFreight">Purchase Cost No</asp:ListItem>
                                <asp:ListItem Value="SupplierName">Supplier</asp:ListItem>
                                <asp:ListItem Value="Attn">Attention</asp:ListItem>
                                <asp:ListItem Value="PONo">PO No</asp:ListItem>
                                <asp:ListItem Value="SuppInvoice">SuppInvoice</asp:ListItem>
                                <asp:ListItem Value="Term">Term</asp:ListItem>
                                <asp:ListItem Value="Currency">Currency</asp:ListItem>
                                <asp:ListItem Value="ForexRate">Rate</asp:ListItem>
                                <asp:ListItem Value="BaseForex">Base Forex</asp:ListItem>
                                <asp:ListItem Value="PPN">PPN</asp:ListItem>
                                <asp:ListItem Value="PPNForex">PPN Forex</asp:ListItem>
                                <asp:ListItem Value="TotalForex">Total Forex</asp:ListItem>
                                <asp:ListItem Value="Remark">Remark</asp:ListItem>
                                <asp:ListItem Value="SuppInvNo">SuppInvoice Dt</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <br />
            <asp:Button class="bitbtn btnadd" runat="server" ID="BtnAdd" Text="Add" />
            &nbsp &nbsp &nbsp
            <asp:DropDownList CssClass="DropDownList" ID="ddlCommand" runat="server" Visible="false" />
            <asp:Button class="btngo" runat="server" ID="BtnGo" Visible="false" Text="G" />
            <br />
            <div style="border: 0px  solid; width: 100%; height: 100%; overflow: auto;">
            </div>
            <asp:GridView ID="GridView1" runat="server" AllowPaging="True" AllowSorting="True"
                CssClass="Grid" AutoGenerateColumns="False">
                <HeaderStyle CssClass="GridHeader"></HeaderStyle>
                <RowStyle CssClass="GridItem" Wrap="false" />
                <AlternatingRowStyle CssClass="GridAltItem" />
                <PagerStyle CssClass="GriInvager" />
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
                                <%--<asp:ListItem Text="Print" />                              --%>
                            </asp:DropDownList>
                            <asp:Button class="btngo" runat="server" ID="BtnGo" Text="G" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>"
                                CommandName="Go" />
                        </ItemTemplate>
                        <HeaderStyle Width="110px" />
                    </asp:TemplateField>
                    <asp:BoundField DataField="TransNmbr" HeaderStyle-Width="320px" HeaderText="Inv No"
                        SortExpression="Nmbr" />
                    <asp:BoundField DataField="Status" HeaderStyle-Width="10px" HeaderText="Status" SortExpression="Status" />
                    <asp:BoundField DataField="TransDate" HtmlEncode="true" DataFormatString="{0:dd MMM yyyy}"
                        HeaderStyle-Width="80px" HeaderText="Date" SortExpression="TransDate" />
                    <asp:BoundField DataField="PurchaseFreight" HeaderStyle-Width="250px" HeaderText="Purchase Cost No"
                        SortExpression="PurchaseFreight" />
                    <asp:BoundField DataField="SupplierName" HeaderStyle-Width="250px" HeaderText="Supplier"
                        SortExpression="SupplierName" />
                    <asp:BoundField DataField="Attn" HeaderStyle-Width="250px" HeaderText="Attention"
                        SortExpression="Attn" />
                    <asp:BoundField DataField="PONo" HeaderStyle-Width="150px" HeaderText="PO No" SortExpression="PONo" />
                    <asp:BoundField DataField="SuppInvoice" HeaderText="SuppInv. No" SortExpression="SuppInvoice" />
                    <asp:BoundField DataField="Term" HeaderText="Term" SortExpression="Term" />
                    <asp:BoundField DataField="DueDate" HtmlEncode="true" DataFormatString="{0:dd MMM yyyy}" HeaderText="Due Date" SortExpression="DueDate" />
                    <asp:BoundField DataField="Currency" HeaderStyle-Width="15px" HeaderText="Currency"
                        SortExpression="Currency" />
                    <asp:BoundField DataField="BaseForex" DataFormatString="{0:#,##0.00}" ItemStyle-HorizontalAlign="Right"
                        HeaderStyle-Width="800px" HeaderText="Base Forex" SortExpression="BaseForex" />
                    <asp:BoundField DataField="PPnForex" DataFormatString="{0:#,##0.00}" ItemStyle-HorizontalAlign="Right"
                        HeaderStyle-Width="80px" HeaderText="PPn Forex" SortExpression="PPnForex" />
                    <asp:BoundField DataField="TotalForex" DataFormatString="{0:#,##0.00}" ItemStyle-HorizontalAlign="Right"
                        HeaderStyle-Width="80px" HeaderText="Total Forex" SortExpression="TotalForex" />
                    <asp:BoundField DataField="Remark" HeaderStyle-Width="200px" HeaderText="Remark"
                        SortExpression="Remark" />
                </Columns>
            </asp:GridView>
            <asp:Panel runat="server" ID="pnlNav" Visible="false">
                <asp:Button class="bitbtn btnadd" runat="server" ID="btnAdd2" Text="Add" />
                &nbsp &nbsp &nbsp
                <asp:DropDownList ID="ddlCommand2" CssClass="DropDownList" runat="server" />
                <asp:Button class="btngo" runat="server" ID="btnGo2" Text="G" />
            </asp:Panel>
        </asp:Panel>
        <asp:Panel runat="server" ID="pnlInput" Visible="false">
            <table style="width: 750px">
                <tr>
                    <td>Inv No</td>
                    <td>:</td>
                    <td>
                        <asp:TextBox ID="tbTransNo" ValidationGroup="Input" runat="server" CssClass="TextBoxR"
                            Enabled="False" ReadOnly="True" Width="150px" />
                        <%--<asp:Label runat="server" Text="   Report : "></asp:Label>    
                    <asp:DropDownList AutoPostBack="true" ValidationGroup="Input" CssClass="DropDownList" ID="ddlReport" runat="server" >
                        <asp:ListItem Selected="True">Y</asp:ListItem>
                        <asp:ListItem>N</asp:ListItem>
                    </asp:DropDownList>--%>
                    </td>
                    <td>Date / Contra Bon Date</td>
                    <td>:</td>
                    <td>
                        <BInv:BasicDatePicker ID="tbTransDate" runat="server" DateFormat="dd MMM yyyy" ShowNoneButton="false"
                            ReadOnly="true" ValidationGroup="Input" ButtonImageHeight="19px" ButtonImageWidth="20px"
                            DisplayType="TextBoxAndImage" TextBoxStyle-CssClass="TextDate" AutoPostBack="True">
                            <TextBoxStyle CssClass="TextDate" /></BInv:BasicDatePicker>   
                        /
                        <BInv:BasicDatePicker ID="tbCBDate" runat="server" DateFormat="dd MMM yyyy" 
                        ReadOnly = "true" ValidationGroup="Input"
                        ButtonImageHeight="19px" ButtonImageWidth="20px" 
                        DisplayType="TextBoxAndImage" 
                        TextBoxStyle-CssClass="TextDate" AutoPostBack="True" 
                        ShowNoneButton="False"><TextBoxStyle CssClass="TextDate" /></BInv:BasicDatePicker>                            
                    </td>            
                </tr>
                <tr>
                    <td>Contra Bon No.</td>
                    <td>:</td>
                    <td><asp:TextBox runat="server" ValidationGroup="Input" ID="tbCBno"  Maxlength = "30" CssClass="TextBoxR" Width="225px"/></td>
                    
                    <td>Aging Start Date</td>
                    <td>:</td>
                    <td> <fieldset style="width:190px;text-align:center">
                            <%--<legend> Apply for : </legend>--%>
                            <asp:RadioButtonList ID="rbAgingDate" runat="server" autopostback=True  RepeatColumns="2" Width="200px" >
                                <asp:ListItem Value= "0" Text="" Selected="True"></asp:ListItem>
                                <asp:ListItem Value= "1" Text="" ></asp:ListItem>                        
                            </asp:RadioButtonList>                
                            </fieldset> </td>   
                    
                </tr>
                <tr>
                    <td>Purchase Cost No.</td>
                    <td class="style1">:</td>
                    <td>
                        <asp:TextBox ID="tbPCNo" runat="server" CssClass="TextBoxR" Enabled="False" ReadOnly="True"
                            ValidationGroup="Input" MaxLength="20" Width="150" />
                        <asp:Button ID="btnPCNo" runat="server" class="btngo" Text="..." />
                    </td>
                    <td>PO No</td>
                    <td class="style1">:</td>
                    <td>
                        <asp:TextBox ID="tbPONo" ValidationGroup="Input" runat="server" CssClass="TextBoxR"
                            Width="150" Enabled="False" ReadOnly="True" />
                    </td>
                </tr>
                <tr>
                    <td>
                        Supplier
                    </td>
                    <td class="style1">
                        :
                    </td>
                    <td colspan="4">
                        <asp:TextBox ID="tbSuppCode" MaxLength="12" ValidationGroup="Input" AutoPostBack="true" Width="100"
                            runat="server" CssClass="TextBox" />
                        <asp:TextBox ID="tbSuppName" Width="248px" runat="server" ReadOnly="true" CssClass="TextBoxR" />
                        <asp:Button class="btngo" ValidationGroup="Input" runat="server" ID="btnSupp" Text="..." />
                    </td>
                </tr>
                <tr>
                    <td>
                        Supplier Inv. No.
                    </td>
                    <td>
                        :
                    </td>
                    <td colspan="4">
                        <asp:TextBox ID="tbSuppInvoice" MaxLength="30" ValidationGroup="Input" runat="server" Width="200px"
                            CssClass="TextBox" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:LinkButton ID="lbTerm" runat="server" Text="Term" ValidationGroup="Input" />
                    </td>
                    <td>
                        :
                    </td>
                    <td colspan="5">
                        <asp:DropDownList ID="ddlTerm" runat="server" AutoPostBack="true" CssClass="DropDownList"
                            ValidationGroup="Input" />
                        <BInv:BasicDatePicker ID="tbDueDate" runat="server" DateFormat="dd MMM yyyy" ShowNoneButton="false"
                            ValidationGroup="Input" ButtonImageHeight="19px" ButtonImageWidth="20px" DisplayType="TextBoxAndImage"
                            TextBoxStyle-CssClass="TextDate" AutoPostBack="True">
                            <TextBoxStyle CssClass="TextDate" />
                        </BInv:BasicDatePicker>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:LinkButton ID="lbCurr" runat="server" Text="Currency" />
                    </td>
                    <td class="style1">
                        :
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlCurr" ValidationGroup="Input" AutoPostBack="true" runat="server"
                            CssClass="DropDownList" Enabled="False" />
                    </td>
                    <td>
                        Forex Rate
                    </td>
                    <td>
                        :
                    </td>
                    <td>
                        <asp:TextBox ID="tbRate" ValidationGroup="Input" runat="server" CssClass="TextBox"
                            Enabled="False" Width="103px" />
                    </td>
                </tr>
                <tr>
                    <td>
                        Amount
                    </td>
                    <td class="style1">
                        :
                    </td>
                    <td colspan="4">
                        <table>
                            <tr style="background-color: Silver; text-align: center">
                                <td>
                                    Base Forex
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
                                    <asp:TextBox ID="tbBaseForex" ValidationGroup="Input" runat="server" CssClass="TextBoxR" />
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
                    <td colspan="4">
                        <asp:TextBox ID="tbRemark" MaxLength="255" ValidationGroup="Input" Width="358px" runat="server" CssClass="TextBox" />
                    </td>
                </tr>
            </table>
            <br />
            <div style="font-size: medium; color: Blue;">
                Detail</div>
            <hr style="color: Blue" />
            <asp:Panel runat="server" ID="pnlDt">
                <asp:Button class="bitbtndt btnadd" runat="server" ID="btnAdddt" Text="Add" ValidationGroup="Input" />
                <br />
                <div style="border: 0px  solid; width: 100%; height: 100%; overflow: auto;">
                    <asp:GridView ID="GridDt" runat="server" AutoGenerateColumns="False" ShowFooter="True">
                        <HeaderStyle CssClass="GridHeader" />
                        <RowStyle CssClass="GridItem" Wrap="false" />
                        <AlternatingRowStyle CssClass="GridAltItem" />
                        <PagerStyle CssClass="GriInvager" />
                        <Columns>
                            <asp:TemplateField HeaderText="Action">
                                <ItemTemplate>
                                    <asp:Button ID="btnEdit" runat="server" class="bitbtndt btnedit" Text="Edit" CommandName="Edit" />
                                    <asp:Button ID="btnDelete" runat="server" class="bitbtndt btndelete" Text="Delete"
                                        OnClientClick="return confirm('Sure to delete this data?');" CommandName="Delete" />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Button ID="btnUpdate" runat="server" class="bitbtndt btnsave" Text="Update"
                                        CommandName="Update" />
                                    <asp:Button ID="btnCancel" runat="server" class="bitbtndt btncancel" Text="Cancel"
                                        CommandName="Cancel" />
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="CostItem" HeaderText="Cost" SortExpression="CostItem"
                                HeaderStyle-Width="70px" />
                            <asp:BoundField DataField="CostItemName" HeaderText="Cost Name" SortExpression="CostItemName"
                                HeaderStyle-Width="150px" />
                            <asp:BoundField DataField="AmountEst" HeaderText="Amount (Estimasi)" DataFormatString="{0:#,##0.00}"
                                ItemStyle-HorizontalAlign="Right" />
                            <asp:BoundField DataField="AmountForex" HeaderText="Base Forex (Realisasi)" DataFormatString="{0:#,##0.00}"
                                ItemStyle-HorizontalAlign="Right" />
                            <asp:BoundField DataField="PPn" HeaderText="PPn % (Realisasi)" DataFormatString="{0:#,##0.00}"
                                ItemStyle-HorizontalAlign="Right" />
                            <asp:BoundField DataField="PPnForex" HeaderText="PPn Forex (Realisasi)" DataFormatString="{0:#,##0.00}"
                                ItemStyle-HorizontalAlign="Right" />
                            <asp:BoundField DataField="TotalForex" HeaderText="Total Forex (Realisasi)" DataFormatString="{0:#,##0.00}"
                                ItemStyle-HorizontalAlign="Right" />
                            <asp:BoundField DataField="Balance" HeaderText="Selisih (+/-)" DataFormatString="{0:#,##0.00}"
                                ItemStyle-HorizontalAlign="Right" />
                            <asp:BoundField DataField="PPnNo" HeaderText="PPn No" />
                            <asp:BoundField DataField="PPNDate"  HtmlEncode="true" DataFormatString="{0:dd MMM yyyy}" HeaderText="PPN Date" />
                            <asp:BoundField DataField="PPNRate" HeaderText="PPN Rate" DataFormatString="{0:#,##0.00}"
                                ItemStyle-HorizontalAlign="Right" />
                            <asp:BoundField DataField="Consignee" HeaderText="Consignee" SortExpression="Consignee"
                                HeaderStyle-Width="100px" />
                            <asp:BoundField DataField="Consignee_Name" HeaderText="Consignee Name" SortExpression="Consignee_Name"
                                HeaderStyle-Width="150px" />
                            <asp:BoundField DataField="SuppInvNo" HeaderText="SuppInv. No" SortExpression="SuppInvNo" />
                            <asp:BoundField DataField="Remark" HeaderText="Remark" HeaderStyle-Width="150px" />
                        </Columns>
                    </asp:GridView>
                </div>
                <asp:Button class="bitbtndt btnadd" runat="server" ID="btnAddDt2" Text="Add" ValidationGroup="Input" />
            </asp:Panel>
            <asp:Panel runat="server" ID="pnlEditDt" Visible="false">
                <table>
                    <tr>
                        <td>
                            Cost
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                            <asp:TextBox ID="tbCost" ValidationGroup="Input" runat="server" CssClass="TextBox"
                                Width="80" AutoPostBack="true" />
                            <asp:TextBox ID="tbCostName" ValidationGroup="Input" runat="server" CssClass="TextBoxR"
                                Width="268px" />
                            <asp:Button class="btngo" runat="server" ID="btnCost" Text="..." />
                        </td>
                    </tr>
                    <%--<tr>
                        <td>Estimasi</td>
                        <td>:</td>
                        <td colspan="4">
                        
                        </td>
                    </tr>--%>
                    <tr>
                        <td>
                            Amount
                        </td>
                        <td>
                            :
                        </td>
                        <td colspan="3">
                            <table>
                                <tr style="background-color: Silver; text-align: center">
                                    <td>
                                        <asp:Label ID="Label3" CssClass="TextBox" runat="server" Text="Amount" />
                                    </td>
                                    <td>
                                        <asp:Label ID="Label4" CssClass="TextBox" runat="server" Text="Base Forex" />
                                    </td>
                                    <td>
                                        <asp:Label ID="Label5" CssClass="TextBox" runat="server" Text="PPn (%)" />
                                    </td>
                                    <td>
                                        <asp:Label ID="Label6" CssClass="TextBox" runat="server" Text="PPn Forex" />
                                    </td>
                                    <td>
                                        <asp:Label ID="Label7" CssClass="TextBox" runat="server" Text="Total Forex" />
                                    </td>
                                </tr>
                                <tr style="background-color: Silver; text-align: center">
                                    <td>
                                        <asp:Label ID="Label2" CssClass="TextBox" runat="server" Text="Estimasi" />
                                    </td>
                                    <td>
                                        <asp:Label ID="lbAmountEst" CssClass="TextBox" runat="server" Text="Realisasi" />
                                    </td>
                                    <td>
                                        <asp:Label ID="lbAmountRealisasi" CssClass="TextBox" runat="server" Text="Realisasi" />
                                    </td>
                                    <td>
                                        <asp:Label ID="Label1" CssClass="TextBox" runat="server" Text="Realisasi" />
                                    </td>
                                    <td>
                                        <asp:Label ID="lbBalance" CssClass="TextBox" runat="server" Text="Realisasi" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:TextBox CssClass="TextBoxR" runat="server" ID="tbAmountEst" Width="80px" Enabled="False" />
                                    </td>
                                    <td>
                                        <asp:TextBox CssClass="TextBox" runat="server" ID="tbAmountRealisasi" Width="80px" AutoPostBack="false"/>
                                    </td>
                                    <td>
                                        <asp:TextBox CssClass="TextBox" runat="server" ID="tbPPnDt" Width="80px" AutoPostBack="true" />
                                    </td>
                                    <td>
                                        <asp:TextBox CssClass="TextBox" runat="server" ID="tbPPNForexDt" Width="80px" />
                                    </td>
                                    <td>
                                        <asp:TextBox CssClass="TextBox" runat="server" ID="tbBalance" Width="80px" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            PPn
                        </td>
                        <td>
                            :
                        </td>
                        <td colspan="3">
                            <table>
                                <tr style="background-color: Silver; text-align: center">
                                    <td>
                                        PPn No
                                    </td>
                                    <td>
                                        PPn Date
                                    </td>
                                    <td>
                                        PPn Rate
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="tbPPnNoDt" runat="server" Enabled="False" CssClass="TextBox" ValidationGroup="Input"
                                            Width="150px" />
                                    </td>
                                    <td>
                                        <BInv:BasicDatePicker ID="tbPPnDateDt" runat="server" DateFormat="dd MMM yyyy" ShowNoneButton="false"
                                            ValidationGroup="Input" Enabled="False" ButtonImageHeight="19px" ButtonImageWidth="20px"
                                            DisplayType="TextBoxAndImage" TextBoxStyle-CssClass="TextDate" AutoPostBack="True">
                                        </BInv:BasicDatePicker>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="tbPpnRateDt" runat="server" Enabled="False" CssClass="TextBox" ValidationGroup="Input"
                                            Width="68px" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Consignee
                        </td>
                        <td class="style1">
                            :
                        </td>
                        <td colspan="4">
                            <asp:TextBox ID="tbConsignee" ValidationGroup="Input" AutoPostBack="true" Width="100"
                                runat="server" CssClass="TextBox" />
                            <asp:TextBox ID="tbConsigneeName" Width="248px" runat="server" ReadOnly="true" CssClass="TextBoxR" />
                            <asp:Button class="btngo" runat="server" ID="btnConsignee" Text="..." />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Consignee Inv. No.
                        </td>
                        <td>
                            :
                        </td>
                        <td colspan="4">
                            <asp:TextBox ID="tbSuppInvNo" MaxLength="30"  ValidationGroup="Input" runat="server" Width="200px"
                                CssClass="TextBox" />
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
                            <asp:TextBox runat="server" ID="tbRemarkDt" CssClass="TextBox" Width="365px" MaxLength="255"
                                TextMode="MultiLine" />
                        </td>
                    </tr>
                </table>
                <br />
                <asp:Button ID="btnSaveDt" runat="server" class="bitbtndt btnsave" Text="Save" />
                &nbsp;
                <asp:Button ID="btnCancelDt" runat="server" class="bitbtndt btncancel" Text="Cancel" />
            </asp:Panel>
            <br />
            <asp:Button ID="btnSaveAll" runat="server" class="bitbtndt btnsave" Text="Save & New"
                ValidationGroup="Input" Width="97px" />
            &nbsp;
            <asp:Button ID="btnSaveTrans" runat="server" class="bitbtndt btnsave" Text="Save"
                ValidationGroup="Input" />
            &nbsp;
            <asp:Button ID="btnBack" runat="server" class="bitbtndt btncancel" Text="Cancel"
                ValidationGroup="Input" />
            &nbsp;
            <asp:Button ID="btnHome" runat="server" class="bitbtndt btncancel" Text="Back" />
        </asp:Panel>
        <br />
    </div>
    <asp:Label runat="server" ID="lbStatus" ForeColor="Red" />
    </form>
</body>
</html>
