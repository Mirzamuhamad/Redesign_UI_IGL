<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TrPPnReporting.aspx.vb"
    Inherits="Master_TrPPnReporting_TrPPnReporting" EnableEventValidation="false" %>

<%--EnableEventValidation="false"--%>
<%@ Register Assembly="BasicFrame.WebControls.BasicDatePicker" Namespace="BasicFrame.WebControls"
    TagPrefix="BDP" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>PPn Reporting</title>

    <script type="text/javascript">
        function OpenPopup() {         
            window.open("../../SearchDlg.Aspx","List","scrollbars=yes,resizable=no,width=500,height=400");        
            return false;
        }    
    </script>

    <script src="../../Function/OpenDlg.JS" type="text/javascript"></script>
    <script src="../../Function/Function.JS" type="text/javascript"></script>
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
 <link href="https://fonts.googleapis.com/css2?family=Muli:wght@300;400;600&display=swap" rel="stylesheet">
    
    <style type="text/css">
        .style1
        {
            width: 400px;
        }
        .style2
        {
            width: 428px;
        }
        .style3
        {
            width: 104px;
        }
        .style4
        {
            width: 20px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div class="Content">
        <div class="H1">
            PPn Reporting</div>
        <hr style="color: Blue" />
        <br />
        <asp:Panel ID="pnlView" runat="server" Visible="true">
            <table>
                <tr>
                    <td style="width: 100px; text-align: right">
                        Quick Search :
                    </td>
                    <td>
                        <asp:TextBox CssClass="TextBox" runat="server" ID="tbFilter" />
                        <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlField">
                            <asp:ListItem Value="A.InvoiceNo" Selected="True">Invoice No</asp:ListItem>
                            <asp:ListItem Value="dbo.FormatDate(A.InvoiceDate)">Invoice Date</asp:ListItem>
                            <asp:ListItem Value="A.Supplier">Buyer</asp:ListItem>
                            <asp:ListItem Value="A.Supplier_Name">Buyer Name</asp:ListItem>
                            <asp:ListItem Value="A.Currency">Currency</asp:ListItem>
                            <asp:ListItem Value="A.PPNNo">PPN No</asp:ListItem>
                            <asp:ListItem Value="dbo.FormatDate(A.PPNDate)">PPN Date</asp:ListItem>
                        </asp:DropDownList>
                        <asp:Button class="bitbtn btnsearch" runat="server" ID="btnSearch" Text="Search" />
                        <asp:Button class="bitbtn btngo" runat="server" ID="btnExpand" Text="..." />
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
                                <asp:ListItem Value="A.InvoiceNo" Selected="True">Invoice No</asp:ListItem>
                                <asp:ListItem Value="dbo.FormatDate(A.InvoiceDate)">Invoice Date</asp:ListItem>
                                <asp:ListItem Value="A.Supplier">Buyer</asp:ListItem>
                                <asp:ListItem Value="A.Supplier_Name">Buyer Name</asp:ListItem>
                                <asp:ListItem Value="A.Currency">Currency</asp:ListItem>
                                <asp:ListItem Value="A.PPNNo">PPN No</asp:ListItem>
                                <asp:ListItem Value="dbo.FormatDate(A.PPNDate)">PPN Date</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel runat="server" ID="Panel1" Visible="true">
                <table>
                    <tr>
                        <td>
                            Period
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlPeriod" runat="server" AutoPostBack="false" CssClass="DropDownList" />
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlYear" runat="server" AutoPostBack="false" CssClass="DropDownList" />
                        </td>
                        <td>
                            <asp:Button class="bitbtn btngo" runat="server" ID="btnPeriod" Text="G" />
                        </td>
                        <td class="style3">
                        </td>
                        <td>
                            PPn Masukan :
                        </td>
                        <td>
                            <asp:Label ID="lblMasukan" runat="server" Text="_"></asp:Label>
                            &nbsp;
                            <asp:Label ID="lblHome1" runat="server" Text="_"></asp:Label>
                        </td>
                        <td class="style4">
                        </td>
                        <td>
                            PPn Keluaran :
                        </td>
                        <td>
                            <asp:Label ID="lblKeluaran" runat="server" Text="_"></asp:Label>
                            &nbsp;
                            <asp:Label ID="lblHome2" runat="server" Text="_"></asp:Label>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <br />
            <asp:Menu ID="Menu1" runat="server" CssClass="Menu" ItemWrap="False" Orientation="Horizontal"
                StaticEnableDefaultPopOutImage="False" StaticMenuItemStyle-CssClass="MenuItem"
                StaticSelectedStyle-CssClass="MenuSelect">
                <StaticSelectedStyle CssClass="MenuSelect" />
                <StaticMenuItemStyle CssClass="MenuItem" />
                <Items>
                    <asp:MenuItem Text="Assign" Value="0"></asp:MenuItem>
                    <asp:MenuItem Text="Cancel" Value="1"></asp:MenuItem>
                    <asp:MenuItem Text="Available" Value="2"></asp:MenuItem>
                </Items>
            </asp:Menu>
            <br />
            <asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex="0" Visible="true">
                <asp:View ID="TabContact" runat="server">
                    <br />
                    <asp:Panel ID="pnlDt" runat="server">
                        <table>
                            <tr>
                                <td>
                                    <asp:Button class="bitbtn btnadd" runat="server" ID="btnRemove" Text="Remove" CommandName="Insert"
                                        Visible="true" />
                                </td>
                                <td>
                                    <asp:Button class="bitbtn btnadd" runat="server" ID="btnRemoveAll" Text="Remove All"
                                        CommandName="Insert" Visible="true" Width="85px" />
                                </td>
                                <td class="style1">
                                    <asp:Button class="bitbtn btnpreview" Width="140px" runat="server" ID="btnImport"
                                        Text="Export PPN Masukan" />
                                    <asp:Button class="bitbtn btnpreview" Width="140px" runat="server" ID="btnImportKeluaran"
                                        Text="Export PPN Keluaran" />    
                                </td>
                                <td>
                                    Show Records:
                                    <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlShowRecord" AutoPostBack="true">
                                        <asp:ListItem Selected="True" Value="10">Choose One</asp:ListItem>
                                        <asp:ListItem Value="20">20</asp:ListItem>
                                        <asp:ListItem Value="30">30</asp:ListItem>
                                        <asp:ListItem Value="40">40</asp:ListItem>
                                        <asp:ListItem Value="50">50</asp:ListItem>
                                        <asp:ListItem Value="100">100</asp:ListItem>
                                    </asp:DropDownList>
                                    Rows
                                </td>
                            </tr>
                        </table>
                        <br />
                        <div style="border: 0px  solid; width: 100%; height: 100%; overflow: auto;">
                            <asp:GridView ID="GridReport" runat="server" AutoGenerateColumns="False" AllowSorting="True"
                                ShowFooter="True" PageSize="10" AllowPaging="True" CssClass="Grid">
                                <HeaderStyle CssClass="GridHeader" />
                                <RowStyle CssClass="GridItem" Wrap="false" />
                                <AlternatingRowStyle CssClass="GridAltItem" />
                                <PagerStyle CssClass="GridPager" />
                                <Columns>
                                    <%--<asp:TemplateField HeaderText="Action">
                                        <ItemTemplate>
                                            <asp:Button class="bitbtndt btnedit" runat="server" ID="btnEditDt" Text="Edit" CommandNAme="Edit"  />									                                
                                            <asp:Button class="bitbtndt btndelete" runat="server" ID="btnDeleteDt" Text="Delete" CommandNAme="Delete" OnClientClick="return confirm('Sure to delete this data?');"  />									                                                                        
                                        </ItemTemplate>
                                    </asp:TemplateField>--%>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:CheckBox ID="cbSelectHd" runat="server" AutoPostBack="true" OnCheckedChanged="cbSelectHd_CheckedChanged" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="cbSelect" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="InvoiceNo" HeaderStyle-Width="135" HeaderText="Invoice No"
                                        SortExpression="InvoiceNo" />
                                    <asp:BoundField DataField="InvoiceDate" HtmlEncode="true" DataFormatString="{0:dd MMM yyyy}"
                                        HeaderText="Invoice Date" SortExpression="InvoiceDate" />
                                    <asp:BoundField DataField="UserCode" HeaderText="Buyer" SortExpression="UserCode" />
                                    <asp:BoundField DataField="UserName" HeaderText="Buyer Name" SortExpression="UserName" />
                                    <asp:BoundField DataField="PPNNo" HeaderText="PPn No" SortExpression="PPNNo" />
                                    <asp:BoundField DataField="PPNDate" HtmlEncode="true" DataFormatString="{0:dd MMM yyyy}"
                                        HeaderText="PPn Date" SortExpression="PPNDate" />
                                    <asp:BoundField DataField="Currency" HeaderText="Currency" SortExpression="Currency" />
                                    <asp:BoundField DataField="PPNRate" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:#,##0.00}"
                                        HeaderText="PPn Rate" SortExpression="PPNRate" />
                                    <asp:BoundField DataField="Amount" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:#,##0.00}"
                                        HeaderText="Base Forex" SortExpression="Amount" />
                                    <asp:BoundField DataField="AmountPPN" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:#,##0.00}"
                                        HeaderText="PPn Forex" SortExpression="AmountPPN" />
                                    <asp:BoundField DataField="PPn" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:#,##0.00}"
                                        HeaderText="PPn" SortExpression="PPn" />
                                </Columns>
                            </asp:GridView>
                            <asp:GridView ID="GridReportTemp" runat="server" AutoGenerateColumns="False" AllowSorting="True"
                                ShowFooter="True" AllowPaging="false" CssClass="Grid">
                                <HeaderStyle CssClass="GridHeader" />
                                <RowStyle CssClass="GridItem" Wrap="false" />
                                <AlternatingRowStyle CssClass="GridAltItem" />
                                <PagerStyle CssClass="GridPager" />
                                <Columns>
                                    <asp:BoundField DataField="InvoiceNo" HeaderStyle-Width="135" HeaderText="Invoice No"
                                        SortExpression="InvoiceNo" />
                                    <asp:BoundField DataField="InvoiceDate" HtmlEncode="true" DataFormatString="{0:dd MMM yyyy}"
                                        HeaderText="Invoice Date" SortExpression="InvoiceDate" />
                                    <asp:BoundField DataField="UserCode" HeaderText="Buyer" SortExpression="UserCode" />
                                    <asp:BoundField DataField="UserName" HeaderText="Buyer Name" SortExpression="UserName" />
                                    <asp:BoundField DataField="PPNNo" HeaderText="PPn No" SortExpression="PPNNo" />
                                    <asp:BoundField DataField="PPNDate" HtmlEncode="true" DataFormatString="{0:dd MMM yyyy}"
                                        HeaderText="PPn Date" SortExpression="PPNDate" />
                                    <asp:BoundField DataField="Currency" HeaderText="Currency" SortExpression="Currency" />
                                    <asp:BoundField DataField="PPNRate" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:#,##0.00}"
                                        HeaderText="PPn Rate" SortExpression="PPNRate" />
                                    <asp:BoundField DataField="Amount" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:#,##0.00}"
                                        HeaderText="Base Forex" SortExpression="Amount" />
                                    <asp:BoundField DataField="AmountPPN" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:#,##0.00}"
                                        HeaderText="PPn Forex" SortExpression="AmountPPN" />
                                    <asp:BoundField DataField="PPn" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:#,##0.00}"
                                        HeaderText="PPn" SortExpression="PPn" />
                                </Columns>
                            </asp:GridView>
                            <asp:GridView ID="GridReportTemp1" runat="server" AutoGenerateColumns="False" AllowSorting="True"
                                ShowFooter="True" AllowPaging="false" CssClass="Grid">
                                <HeaderStyle CssClass="GridHeader" />
                                <RowStyle CssClass="GridItem" Wrap="false" />
                                <AlternatingRowStyle CssClass="GridAltItem" />
                                <PagerStyle CssClass="GridPager" />
                                <Columns>
                                    <asp:BoundField DataField="InvoiceNo" HeaderStyle-Width="135" HeaderText="Invoice No"
                                        SortExpression="InvoiceNo" />
                                    <asp:BoundField DataField="InvoiceDate" HtmlEncode="true" DataFormatString="{0:dd MMM yyyy}"
                                        HeaderText="Invoice Date" SortExpression="InvoiceDate" />
                                    <asp:BoundField DataField="SONo" HeaderText="SO No" SortExpression="SONo" />
                                    <asp:BoundField DataField="CustPONo" HeaderText="Cust PO No" SortExpression="CustPONo" />
                                    <asp:BoundField DataField="Customer" HeaderText="Customer" SortExpression="Customer" />
                                    <asp:BoundField DataField="PPNNo" HeaderText="PPn No" SortExpression="PPNNo" />
                                    <asp:BoundField DataField="PPNDate" HtmlEncode="true" DataFormatString="{0:dd MMM yyyy}"
                                        HeaderText="PPn Date" SortExpression="PPNDate" />                                    
                                    <asp:BoundField DataField="PPNRate" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:#,##0.00}"
                                        HeaderText="PPn Rate" SortExpression="PPNRate" />
                                    <asp:BoundField DataField="Currency" HeaderText="Currency" SortExpression="Currency" />    
                                    <asp:BoundField DataField="ForexRate" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:#,##0.00}"
                                        HeaderText="Forex Rate" SortExpression="ForexRate" />
                                    <asp:BoundField DataField="Base" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:#,##0.00}"
                                        HeaderText="Base Forex" SortExpression="Base" />
                                    <asp:BoundField DataField="PPN" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:#,##0.00}"
                                        HeaderText="PPn" SortExpression="PPN" />
                                    <asp:BoundField DataField="PPnRp" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:#,##0.00}"
                                        HeaderText="PPn Rp" SortExpression="PPnRp" />
                                    <asp:BoundField DataField="Remark" HeaderText="Remark" SortExpression="Remark" />    
                                </Columns>
                            </asp:GridView>
                        </div>
                        <br />
                    </asp:Panel>
                    <%--<asp:Panel ID="pnlEditDt" runat="server" Visible="false">
                        <table>
                            <tr>
                                <td>
                                    Item No</td>
                                <td>
                                    :</td>
                                <td>
                                    <asp:Label ID="lbItemNo" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Contact Name</td>
                                <td>
                                    :</td>
                                <td>
                                    <asp:TextBox runat="server" MaxLength="80" CssClass="TextBox" ID="tbContactName" 
                                    Width="250px" ValidationGroup="Input"/>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Contact Title</td>
                                <td>
                                    :</td>
                                <td>
                                    <asp:TextBox runat="server" MaxLength="80" CssClass="TextBox" ID="tbContactTitle" 
                                    Width="250px" ValidationGroup="Input"/>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Phone</td>
                                <td>
                                    :</td>
                                <td>
                                    <asp:TextBox runat="server" MaxLength="80" CssClass="TextBox" ID="tbPhone2" 
                                    Width="250px" ValidationGroup="Input"/>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Handphone</td>
                                <td>
                                    :</td>
                                <td>
                                    <asp:TextBox runat="server" MaxLength="80" CssClass="TextBox" ID="tbHandphone" 
                                    Width="250px" ValidationGroup="Input"/>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Email</td>
                                <td>
                                    :</td>
                                <td>
                                    <asp:TextBox runat="server" MaxLength="80" CssClass="TextBox" ID="tbEmail" 
                                    Width="250px" ValidationGroup="Input"/>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Religion</td>
                                <td>
                                    :</td>
                                <td>
                                    <asp:DropDownList ID="ddlReligion" runat="server" CssClass="DropDownList" 
                                        Enabled="true" Width="150px">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Birth Date</td>
                                <td>
                                    :</td>
                                <td>
                                    <BDP:BasicDatePicker ID="tbBirthDate" runat="server" DateFormat="dd MMM yyyy" 
                                        ReadOnly = "true" ValidationGroup="Input"
                                        ButtonImageHeight="19px" ButtonImageWidth="20px" 
                                        DisplayType="TextBoxAndImage" 
                                        TextBoxStyle-CssClass="TextDate" 
                                        ShowNoneButton="False"><TextBoxStyle CssClass="TextDate" />
                                    </BDP:BasicDatePicker>
                                </td>
                            </tr>
                             
                            <tr>
                                <td colspan="3" style="text-align: center">
                                    <asp:Button class="bitbtndt btnsave" runat="server" ID="btnSaveDt" Text="Save" />									                                                                                                                                                        
                                    <asp:Button class="bitbtndt btncancel" runat="server" ID="btnCancelDt" Text="Cancel" />									                                                                                                                                                                                                                                
                                </td>
                            </tr>
                        </table>
                        <br />
                    </asp:Panel>--%>
                </asp:View>
                <asp:View ID="TabCancel" runat="server">
                    <br />
                    <table>
                        <tr>
                            <td>
                                <asp:Button class="bitbtn btnadd" runat="server" ID="btnRemoveCancel" Text="Remove" CommandName="Insert"
                                    Visible="true" />
                            </td>
                            <td class="style2">
                                <asp:Button class="bitbtn btnadd" runat="server" ID="btnRemoveCancelAll" Text="Remove All" CommandName="Insert"
                                    Visible="true" Width="85px" />
                            </td>
                            <td>
                                Show Records:
                                <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlShowRecord3" AutoPostBack="true">
                                    <asp:ListItem Selected="True" Value="10">Choose One</asp:ListItem>
                                    <asp:ListItem Value="20">20</asp:ListItem>
                                    <asp:ListItem Value="30">30</asp:ListItem>
                                    <asp:ListItem Value="40">40</asp:ListItem>
                                    <asp:ListItem Value="50">50</asp:ListItem>
                                    <asp:ListItem Value="100">100</asp:ListItem>
                                </asp:DropDownList>
                                Rows
                            </td>
                        </tr>
                    </table>
                    <br />
                    <div style="border: 0px  solid; width: 100%; height: 100%; overflow: auto;">
                        <asp:GridView ID="GridRemove" runat="server" AutoGenerateColumns="False" AllowSorting="True"
                            ShowFooter="True" AllowPaging="True" CssClass="Grid">
                            <HeaderStyle CssClass="GridHeader" />
                            <RowStyle CssClass="GridItem" Wrap="false" />
                            <AlternatingRowStyle CssClass="GridAltItem" />
                            <PagerStyle CssClass="GridPager" />
                            <Columns>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:CheckBox ID="cbSelectHd3" runat="server" AutoPostBack="true" OnCheckedChanged="cbSelectHd3_CheckedChanged" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="cbSelect3" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="UserCode" HeaderText="User Code" SortExpression="UserCode" />
                                <asp:BoundField DataField="UserName" HeaderText="User Name" SortExpression="UserName" />
                                <asp:BoundField DataField="InvoiceNo" HeaderStyle-Width="135" HeaderText="Invoice No"
                                    SortExpression="InvoiceNo" />
                                <asp:BoundField DataField="InvoiceDate" HtmlEncode="true" DataFormatString="{0:dd MMM yyyy}"
                                    HeaderText="Invoice Date" SortExpression="InvoiceDate" />
                                <asp:BoundField DataField="PPNNo" HeaderText="PPn No" SortExpression="PPNNo" />
                                <asp:BoundField DataField="PPNDate" HtmlEncode="true" DataFormatString="{0:dd MMM yyyy}"
                                    HeaderText="PPn Date" SortExpression="PPNDate" />
                                <asp:BoundField DataField="Currency" HeaderText="Currency" SortExpression="Currency" />
                                <asp:BoundField DataField="PPNRate" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:#,##0.00}"
                                    HeaderText="PPn Rate" SortExpression="PPNRate" />
                                <asp:BoundField DataField="Amount" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:#,##0.00}"
                                    HeaderText="Base Forex" SortExpression="Amount" />
                                <asp:BoundField DataField="AmountPPN" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:#,##0.00}"
                                    HeaderText="PPn Forex" SortExpression="AmountPPN" />
                                <asp:BoundField DataField="PPn" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:#,##0.00}"
                                    HeaderText="PPn" SortExpression="PPn" />
                            </Columns>
                        </asp:GridView>
                    </div>
                </asp:View>
                <asp:View ID="TabCustomer" runat="server">
                    <br />
                    <asp:Panel ID="PanelCust" runat="server">
                        <table>
                            <tr>
                                <td>
                                    <asp:Button class="bitbtn btnadd" runat="server" ID="btnAdd" Text="Add" CommandName="Insert"
                                        Visible="true" />
                                </td>
                                <td>
                                    <asp:Button class="bitbtn btnadd" runat="server" ID="btnAddAll" Text="Add All" CommandName="Insert"
                                        Visible="true" />
                                </td>
                                <td>
                                    <asp:Button class="bitbtn btnadd" runat="server" ID="btnRmv" Text="Remove" CommandName="Insert"
                                        Visible="true" />
                                </td>
                                <td class="style2">
                                    <asp:Button class="bitbtn btnadd" runat="server" ID="btnRmvAll" Text="Remove All" CommandName="Insert"
                                        Visible="true" Width="85px"/>
                                </td>
                                <td>
                                    Show Records:
                                    <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlShowRecord2" AutoPostBack="true">
                                        <asp:ListItem Selected="True" Value="10">Choose One</asp:ListItem>
                                        <asp:ListItem Value="20">20</asp:ListItem>
                                        <asp:ListItem Value="30">30</asp:ListItem>
                                        <asp:ListItem Value="40">40</asp:ListItem>
                                        <asp:ListItem Value="50">50</asp:ListItem>
                                        <asp:ListItem Value="100">100</asp:ListItem>
                                    </asp:DropDownList>
                                    Rows
                                </td>
                            </tr>
                        </table>
                        <br />
                        <div style="border: 0px  solid; width: 100%; height: 100%; overflow: auto;">
                            <asp:GridView ID="GridNonReport" runat="server" AutoGenerateColumns="False" AllowSorting="True"
                                ShowFooter="True" AllowPaging="True" CssClass="Grid">
                                <HeaderStyle CssClass="GridHeader" />
                                <RowStyle CssClass="GridItem" Wrap="false" />
                                <AlternatingRowStyle CssClass="GridAltItem" />
                                <PagerStyle CssClass="GridPager" />
                                <Columns>
                                    <%--<asp:TemplateField HeaderText="Action">
                                        <ItemTemplate>
                                            <asp:Button class="bitbtndt btnedit" runat="server" ID="btnEditDt" Text="Edit" CommandNAme="Edit"  />									                                
                                            <asp:Button class="bitbtndt btndelete" runat="server" ID="btnDeleteDt" Text="Delete" CommandNAme="Delete" OnClientClick="return confirm('Sure to delete this data?');"  />									                                                                        
                                        </ItemTemplate>
                                    </asp:TemplateField>--%>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:CheckBox ID="cbSelectHd2" runat="server" AutoPostBack="true" OnCheckedChanged="cbSelectHd2_CheckedChanged" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="cbSelect2" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="InvoiceNo" HeaderStyle-Width="135" HeaderText="Invoice No"
                                        SortExpression="InvoiceNo" />
                                    <asp:BoundField DataField="InvoiceDate" HtmlEncode="true" DataFormatString="{0:dd MMM yyyy}"
                                        HeaderText="Invoice Date" SortExpression="InvoiceDate" />
                                    <asp:BoundField DataField="UserCode" HeaderText="Buyer" SortExpression="UserCode" />
                                    <asp:BoundField DataField="UserName" HeaderText="Buyer Name" SortExpression="UserName" />
                                    <asp:BoundField DataField="PPNNo" HeaderText="PPn No" SortExpression="PPNNo" />
                                    <asp:BoundField DataField="PPNDate" HtmlEncode="true" DataFormatString="{0:dd MMM yyyy}"
                                        HeaderText="PPn Date" SortExpression="PPNDate" />
                                    <asp:BoundField DataField="Currency" HeaderText="Currency" SortExpression="Currency" />
                                    <asp:BoundField DataField="PPNRate" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:#,##0.00}"
                                        HeaderText="PPn Rate" SortExpression="PPNRate" />
                                    <asp:BoundField DataField="Amount" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:#,##0.00}"
                                        HeaderText="Base Forex" SortExpression="Amount" />
                                    <asp:BoundField DataField="AmountPPN" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:#,##0.00}"
                                        HeaderText="PPn Forex" SortExpression="AmountPPN" />
                                    <asp:BoundField DataField="PPn" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:#,##0.00}"
                                        HeaderText="PPn" SortExpression="PPn" />
                                </Columns>
                            </asp:GridView>
                        </div>
                    </asp:Panel>
                </asp:View>
            </asp:MultiView>
        </asp:Panel>
        <%--<asp:SqlDataSource ID="dsCurrency" runat="server"       
        SelectCommand="EXEC S_GetCurrency">
    </asp:SqlDataSource>--%>
        <%--<asp:SqlDataSource ID="dsBank" runat="server"       
        SelectCommand="EXEC S_GetBank">
    </asp:SqlDataSource>--%>
        <asp:Label ID="lbstatus" ForeColor="red" runat="server"></asp:Label>
    </div>
    </form>
</body>
</html>
