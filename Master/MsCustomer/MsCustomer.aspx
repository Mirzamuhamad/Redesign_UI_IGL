<%@ Page Language="VB" AutoEventWireup="false" CodeFile="MsCustomer.aspx.vb" Inherits="Master_MsCustomer_MsCustomer" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="GMDatePicker" Namespace="GrayMatterSoft" TagPrefix="cc1" %>
<%@ Register Assembly="BasicFrame.WebControls.BasicDatePicker" Namespace="BasicFrame.WebControls"
    TagPrefix="BDP" %>
<%@ Register Assembly="CrystalDecisions.Web, Version=10.5.3700.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
<html xmlns="http://www.w3.org/2259/xhtml">
<head id="Head1" runat="server">
    <title>Customer File</title>

    <script src="../../Function/OpenDlg.JS" type="text/javascript"></script>

    <script type="text/javascript">
    function OpenPopup() {         
            window.open("../../SearchDlg.Aspx","List","scrollbars=yes,resizable=no,width=500,height=400");        
            return false;
        }   
    function OpenPopup2() {        
        window.open("../../SearchMultiDlg.Aspx","List","scrollbars=yes,resizable=no,width=500,height=400");        
        return false;
    } 
    function OpenPopupSearch() {         
            window.open("../../UserControl/AdvanceSearch.Aspx","List","scrollbars=yes,resizable=no,width=500,height=400");        
            return false;
        }       
       
    function addCommas(nStr)
    {
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
    
    
    </script>

    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .style1
        {
            width: 150px;
        }
        .style2
        {
            width: 8px;
        }
        .style3
        {
            width: 347px;
        }
        .style4
        {
            width: 13px;
        }
        .style5
        {
            width: 9px;
        }
        .style6
        {
        }
        .style7
        {
            width: 94px;
            height: 17px;
        }
        .style8
        {
            width: 8px;
            height: 17px;
        }
        .style9
        {
            height: 17px;
        }
        .style10
        {
            width: 13px;
            height: 17px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div class="Content">
        <div class="H1">
            Customer File</div>
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
                            <asp:ListItem Selected="True" Value="CustCode">Customer Code</asp:ListItem>
                            <asp:ListItem Value="CustName">Customer Name</asp:ListItem>
                            <asp:ListItem Value="MemeberOfGroup">Member Of Group</asp:ListItem>
                            <asp:ListItem Value="SisterCompany">Sister Company</asp:ListItem>
                            <asp:ListItem Value="CustGroupName">Customer Group</asp:ListItem>
                            <asp:ListItem Value="CustTypeName">Customer Type</asp:ListItem>
                            <asp:ListItem Value="FgPPN">PPN</asp:ListItem>
                            <asp:ListItem Value="KodePPn">PPn Code</asp:ListItem>
                            <asp:ListItem Value="PPnName">PPn Name</asp:ListItem>
                            <asp:ListItem Value="Address1">Address 1</asp:ListItem>
                            <asp:ListItem Value="Address2">Address 2</asp:ListItem>
                            <asp:ListItem Value="CityName">City</asp:ListItem>
                            <asp:ListItem Value="ZipCode">Post Code</asp:ListItem>
                            <asp:ListItem Value="Phone">Telephone</asp:ListItem>
                            <asp:ListItem Value="Fax">Fax</asp:ListItem>
                            <asp:ListItem Value="Email">Email</asp:ListItem>
                            <asp:ListItem Value="A.CurrCode">Currency</asp:ListItem>
                            <asp:ListItem Value="PayName">Payment To</asp:ListItem>
                            <asp:ListItem Value="TermName">Payment Term</asp:ListItem>
                            <asp:ListItem Value="FgLimit">Limit</asp:ListItem>
                            <asp:ListItem Value="NPWP">NPWP</asp:ListItem>
                            <asp:ListItem Value="FPCustKode">FP Cust Kode</asp:ListItem>
                            <asp:ListItem Value="FgActive">Active</asp:ListItem>
                            <asp:ListItem Value="PriceBySO">Price By SO</asp:ListItem>
                            <asp:ListItem Value="ContactName">Contact Name</asp:ListItem>
                            <asp:ListItem Value="ContactTitle">Contact Title</asp:ListItem>
                            <asp:ListItem Value="ContactHP">Contact HP</asp:ListItem>
                            <asp:ListItem Value="ContactEmail">Contact Email</asp:ListItem>
                        </asp:DropDownList>
                        <asp:Button class="bitbtn btnsearch" runat="server" ID="btnsearch" Text="Search" />
                        <asp:Button class="bitbtn btngo" runat="server" ID="btnexpand" Text="..." />
                        <asp:Button class="bitbtn btnprint" runat="server" ID="btnprint" Text="Print" />
                    </td>
                    <td>
                        &nbsp;
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
                                <asp:ListItem Selected="True" Value="CustCode">Customer Code</asp:ListItem>
                                <asp:ListItem Value="CustName">Customer Name</asp:ListItem>
                                <asp:ListItem Value="MemberOfGroup">Member Of Group</asp:ListItem>
                                <asp:ListItem Value="SisterCompany">Sister Company</asp:ListItem>
                                <asp:ListItem Value="CustGroupName">Customer Group</asp:ListItem>
                                <asp:ListItem Value="CustTypeName">Customer Type</asp:ListItem>
                                <asp:ListItem Value="FgPPN">PPN</asp:ListItem>
                                <asp:ListItem Value="KodePPn">PPn Code</asp:ListItem>
                                <asp:ListItem Value="PPnName">PPn Name</asp:ListItem>
                                <asp:ListItem Value="Address1">Address 1</asp:ListItem>
                                <asp:ListItem Value="Address2">Address 2</asp:ListItem>
                                <asp:ListItem Value="CityName">City</asp:ListItem>
                                <asp:ListItem Value="ZipCode">Post Code</asp:ListItem>
                                <asp:ListItem Value="Phone">Telephone</asp:ListItem>
                                <asp:ListItem Value="Fax">Fax</asp:ListItem>
                                <asp:ListItem Value="Email">Email</asp:ListItem>
                                <asp:ListItem Value="A.CurrCode">Currency</asp:ListItem>
                                <asp:ListItem Value="PayName">Payment To</asp:ListItem>
                                <asp:ListItem Value="TermName">Payment Term</asp:ListItem>
                                <asp:ListItem Value="FgLimit">Limit</asp:ListItem>
                                <asp:ListItem Value="NPWP">NPWP</asp:ListItem>
                                <asp:ListItem Value="FPCustKode">FP Cust Kode</asp:ListItem>
                                <asp:ListItem Value="FgActive">Active</asp:ListItem>
                                <asp:ListItem Value="PriceBySO">Price By SO</asp:ListItem>
                                <asp:ListItem Value="ContactName">Contact Name</asp:ListItem>
                                <asp:ListItem Value="ContactTitle">Contact Title</asp:ListItem>
                                <asp:ListItem Value="ContactHP">Contact HP</asp:ListItem>
                                <asp:ListItem Value="ContactEmail">Contact Email</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <br />
            <%--<asp:Button ID="BtnAdd" CssClass="Button" runat="server" Text="Add" /> --%>
            <asp:Button class="bitbtndt btnadd" runat="server" ID="btnAdd" Text="Add" />
            <br />
            <div style="border: 0px  solid; width: 100%; height: 350px; overflow: auto;">
                <asp:GridView ID="GridView1" runat="server" AllowPaging="True" AllowSorting="True"
                    CssClass="Grid" AutoGenerateColumns="False">
                    <HeaderStyle CssClass="GridHeader"></HeaderStyle>
                    <RowStyle CssClass="GridItem" Wrap="False" />
                    <AlternatingRowStyle CssClass="GridAltItem" />
                    <PagerStyle CssClass="GridPager" />
                    <Columns>
                        <%--<asp:TemplateField>
                      <ItemTemplate>
                          <asp:CheckBox ID="cbSelect" runat="server" />
                      </ItemTemplate>
                      <HeaderTemplate>
                          <asp:CheckBox ID="cbSelectHd" runat="server" AutoPostBack="true" 
                              oncheckedchanged="cbSelectHd_CheckedChanged" />
                      </HeaderTemplate>
                  </asp:TemplateField>--%>
                        <asp:TemplateField Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="cbSelectHd" runat="server" Text='' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-Width="110px" HeaderText="Action">
                            <ItemTemplate>
                                <asp:DropDownList CssClass="DropDownList" ID="ddl" runat="server">
                                    <asp:ListItem Selected="True" Text="View" />
                                    <asp:ListItem Text="Edit" />
                                    <asp:ListItem>Delete</asp:ListItem>
                                    <asp:ListItem>Copy New</asp:ListItem>
                                </asp:DropDownList>
                                <%--<asp:Button ID="btnGo" CssClass="Button" runat="server" CommandName="Go" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" Text="Go" />--%>
                                <asp:Button class="btngo" runat="server" ID="btngo" Text="G" CommandName="Go" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" />
                            </ItemTemplate>
                            <HeaderStyle Width="110px" />
                        </asp:TemplateField>
                        <asp:BoundField DataField="CustCode" HeaderText="Customer Code" SortExpression="CustCode" />
                        <asp:BoundField DataField="CustName" HeaderText="Customer Name" SortExpression="CustName" />
                        <asp:BoundField DataField="MemberOfGroup" HeaderText="Member Of Group" SortExpression="MemberOfGroup" />
                        <asp:BoundField DataField="SisterCompany" HeaderText="Sister Company" SortExpression="SisterCompany" />
                        <asp:BoundField DataField="CustGroup" HeaderText="Customer Group Code" SortExpression="CustGroup"
                            HeaderStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="CustGroupName" HeaderText="Customer Group" SortExpression="CustGroupName" />
                        <asp:BoundField DataField="CustType" HeaderText="Customer Type Code" SortExpression="CustType">
                        </asp:BoundField>
                        <asp:BoundField DataField="CustTypeName" HeaderText="Customer Type" SortExpression="CustTypeName">
                        </asp:BoundField>
                        <asp:BoundField DataField="FgPPN" HeaderText="PPN" SortExpression="FgPPN" />
                        <asp:BoundField DataField="KodePPn" HeaderText="PPn Code" SortExpression="KodePPn">
                        </asp:BoundField>
                        <asp:BoundField DataField="PPnName" HeaderText="PPn Name" SortExpression="PPnName">
                        </asp:BoundField>
                        <asp:BoundField DataField="Address1" HeaderText="Address1" SortExpression="Address1" />
                        <asp:BoundField DataField="Address2" HeaderText="Address2" SortExpression="Address2" />
                        <asp:BoundField DataField="City" HeaderText="City Code" SortExpression="City"></asp:BoundField>
                        <asp:BoundField DataField="CityName" HeaderText="City" SortExpression="CityName">
                        </asp:BoundField>
                        <asp:BoundField DataField="ZipCode" HeaderText="Post Code" SortExpression="ZipCode" />
                        <asp:BoundField DataField="Phone" HeaderText="Telephone" SortExpression="Phone" />
                        <asp:BoundField DataField="Fax" HeaderText="Fax" SortExpression="Fax" />
                        <asp:BoundField DataField="Email" HeaderText="Email" SortExpression="Email" />
                        <asp:BoundField DataField="CurrCode" HeaderText="Currency" SortExpression="CurrCode" />
                        <asp:BoundField DataField="PayCode" HeaderText="Payment To" SortExpression="PayCode" />
                        <asp:BoundField DataField="PayName" HeaderText="Payment To Name" SortExpression="PayName" />
                        <asp:BoundField DataField="Term" HeaderText="Payment Term Code" SortExpression="Term">
                        </asp:BoundField>
                        <asp:BoundField DataField="TermName" HeaderText="Payment Term Name" SortExpression="TermName">
                        </asp:BoundField>
                        <asp:BoundField DataField="FgLimit" HeaderText="Limit" SortExpression="FgLimit" />
                        <asp:BoundField DataField="NPWP" HeaderText="NPWP" SortExpression="NPWP"></asp:BoundField>
                        <asp:BoundField DataField="FPCustKode" HeaderText="FPCustKode" SortExpression="FPCustKode">
                        </asp:BoundField>
                        <asp:BoundField DataField="FgActive" HeaderText="Active" SortExpression="FgActive" />
                        <asp:BoundField DataField="PriceBySO" HeaderText="Price By SO" SortExpression="PriceBySO" />
                        <asp:BoundField DataField="ContactName" HeaderText="Contact Name" SortExpression="ContactName">
                        </asp:BoundField>
                        <asp:BoundField DataField="ContactTitle" HeaderText="Contact Title" SortExpression="ContactTitle">
                        </asp:BoundField>
                        <asp:BoundField DataField="ContactHp" HeaderText="Contact Hp" SortExpression="ContactHp">
                        </asp:BoundField>
                        <asp:BoundField DataField="ContactEmail" HeaderText="Contact Email" SortExpression="ContactEmail" />
                        <%--<asp:BoundField DataField="CurrCode" HeaderText="Curr Limit" SortExpression="CurrCode" />
                  <asp:BoundField DataField="CustLimit" HeaderText="Max Limit" SortExpression="CustLimit" />    
                  <asp:BoundField DataField="UseLimit" HeaderText="Used Limit" SortExpression="UseLimit" />
                  <asp:BoundField DataField="GracePeriod" HeaderText="GracePeriod" SortExpression="GracePeriod" />
                  <asp:BoundField DataField="Balance" HeaderText="Balance" SortExpression="Balance" />--%>
                    </Columns>
                </asp:GridView>
            </div>
            <asp:Panel runat="server" ID="pnlNav" Visible="false">
                <%--<asp:Button ID="btnAdd2" CssClass="Button" runat="server" Text="Add" /> --%>
                <asp:Button class="bitbtndt btnadd" runat="server" ID="btnadd2" Text="Add" />
            </asp:Panel>
        </asp:Panel>
        <asp:Panel runat="server" ID="pnlView" Visible="false">
            <table>
                <tr>
                    <td class="style1">
                        Customer Code
                    </td>
                    <td class="style2">
                        :
                    </td>
                    <td>
                        <asp:TextBox ID="tbCode" runat="server" CssClass="TextBox" MaxLength="12" Width="149px" />
                        <asp:Label ID="Label1" runat="server" ForeColor="Red" Text="*"></asp:Label>
                    </td>
                    <td class="style4">
                        &nbsp;
                    </td>
                    <td class="style6">
                        &nbsp;
                    </td>
                    <td class="style5">
                        &nbsp;
                    </td>
                    <td class="style3">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td class="style1">
                        Customer Name
                    </td>
                    <td class="style2">
                        :
                    </td>
                    <td>
                        <asp:TextBox ID="tbName" runat="server" CssClass="TextBox" MaxLength="60" Width="225px" />
                        <asp:Label ID="Label5" runat="server" ForeColor="Red" Text="*"></asp:Label>
                    </td>
                    <td class="style4">
                        &nbsp;
                    </td>
                    <td class="style6">
                        &nbsp;
                    </td>
                    <td class="style5">
                        &nbsp;
                    </td>
                    <td class="style3">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td class="style1">
                        Member Of Group
                    </td>
                    <td class="style2">
                        :
                    </td>
                    <td>
                        <asp:TextBox ID="tbMember" runat="server" CssClass="TextBox" MaxLength="100" Width="225px" />
                        <asp:Label ID="Label6" runat="server" ForeColor="Red" Text="*"></asp:Label>
                    </td>
                    <td class="style4">
                        &nbsp;
                    </td>
                    <td class="style6">
                        &nbsp;
                    </td>
                    <td class="style5">
                        &nbsp;
                    </td>
                    <td class="style3">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td class="style1">
                        Sister Company
                    </td>
                    <td class="style2">
                        :
                    </td>
                    <td>
                        <asp:TextBox ID="tbSister" runat="server" CssClass="TextBox" MaxLength="100" Width="225px" />
                        <asp:Label ID="Label7" runat="server" ForeColor="Red" Text="*"></asp:Label>
                    </td>
                    <td class="style4">
                        &nbsp;
                    </td>
                    <td class="style6">
                        &nbsp;
                    </td>
                    <td class="style5">
                        &nbsp;
                    </td>
                    <td class="style3">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td class="style1">
                        Customer Group
                    </td>
                    <td class="style2">
                        :
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlCustGroup" runat="server" CssClass="DropDownList" MaxLength="5"
                            Width="230px">
                        </asp:DropDownList>
                        <asp:Label ID="Label2" runat="server" ForeColor="Red" Text="*"></asp:Label>
                    </td>
                    <td class="style4">
                        &nbsp;
                    </td>
                    <td class="style6">
                        Customer Type
                    </td>
                    <td class="style5">
                        :
                    </td>
                    <td class="style3">
                        <asp:DropDownList ID="ddlCustType" runat="server" Width = "225px" CssClass="DropDownList" MaxLength="5">
                        </asp:DropDownList>
                        <asp:Label ID="Label3" runat="server" ForeColor="Red" Text="*"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="style1">
                        PPN
                    </td>
                    <td class="style2">
                        :
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlPPN" runat="server" CssClass="DropDownList" Width="44px">
                            <asp:ListItem>Y</asp:ListItem>
                            <asp:ListItem>N</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td class="style4">
                        &nbsp;
                    </td>
                    <td class="style6">
                        Kode PPn
                    </td>
                    <td class="style5">
                        :
                    </td>
                    <td class="style3">
                        <asp:DropDownList ID="ddlKodePPn" runat="server" Width ="225px" CssClass="DropDownList" MaxLength="5">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="style1">
                        Address 1
                    </td>
                    <td class="style2">
                        :
                    </td>
                    <td>
                        <asp:TextBox ID="tbAddress1" runat="server" CssClass="TextBox" MaxLength="100" Width="225px" />
                    </td>
                    <td class="style4">
                        &nbsp;
                    </td>
                    <td class="style6">
                        &nbsp;
                    </td>
                    <td class="style5">
                        &nbsp;
                    </td>
                    <td class="style3">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td class="style1">
                        Address 2
                    </td>
                    <td class="style2">
                        :
                    </td>
                    <td>
                        <asp:TextBox ID="tbAddress2" runat="server" CssClass="TextBox" MaxLength="100" Width="225px" />
                    </td>
                    <td class="style4">
                        &nbsp;
                    </td>
                    <td class="style6">
                        &nbsp;
                    </td>
                    <td class="style5">
                        &nbsp;
                    </td>
                    <td class="style3">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td class="style1">
                        City
                    </td>
                    <td class="style2">
                        :
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlCity" runat="server" CssClass="DropDownList" MaxLength="5"
                            Width="230px">
                        </asp:DropDownList>
                    </td>
                    <td class="style4">
                        &nbsp;&nbsp;
                    </td>
                    <td class="style6">
                        Postal Code
                    </td>
                    <td class="style5">
                        :
                    </td>
                    <td class="style3">
                        <asp:TextBox ID="tbPostCode" runat="server" CssClass="TextBox" MaxLength="10" Width="225px" />
                    </td>
                </tr>
                <tr>
                    <td class="style1">
                        Telephone
                    </td>
                    <td class="style2">
                        :
                    </td>
                    <td>
                        <asp:TextBox ID="tbPhone" runat="server" CssClass="TextBox" MaxLength="40" Width="225px" />
                    </td>
                    <td class="style4">
                        &nbsp;
                    </td>
                    <td class="style6">
                        Fax
                    </td>
                    <td class="style5">
                        :
                    </td>
                    <td class="style3">
                        <asp:TextBox ID="tbFax" runat="server" CssClass="TextBox" MaxLength="40" Width="225px" />
                    </td>
                </tr>
                <tr>
                    <td class="style1">
                        Email
                    </td>
                    <td class="style2">
                        :
                    </td>
                    <td>
                        <asp:TextBox ID="tbEmail" runat="server" CssClass="TextBox" MaxLength="50" Width="225px" />
                    </td>
                    <td class="style4">
                        &nbsp;
                    </td>
                    <td class="style6">
                        &nbsp;
                    </td>
                    <td class="style5">
                        &nbsp;
                    </td>
                    <td class="style3">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td class="style1">
                        Currency
                    </td>
                    <td class="style2">
                        :
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlCurr" runat="server" CssClass="DropDownList" Width="100px">
                        </asp:DropDownList>
                        <asp:Label ID="Label8" runat="server" ForeColor="Red" Text="*"></asp:Label>
                    </td>
                    <td class="style4">
                        &nbsp;
                    </td>
                    <td class="style1">
                        Protect Limit
                    </td>
                    <td class="style2">
                        :
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlFgLimit" runat="server" CssClass="DropDownList" Width="72px">
                            <asp:ListItem>Y</asp:ListItem>
                            <asp:ListItem>N</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="style1">
                        Payment To
                    </td>
                    <td class="style2">
                        :
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlPaymentTo" runat="server" CssClass="DropDownList" Width="230px">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="style1">
                        Payment Term
                    </td>
                    <td class="style2">
                        :
                    </td>
                    <td class="style3">
                        <asp:TextBox ID="tbTerm" runat="server" AutoPostBack="True" CssClass="TextBox" 
                            MaxLength="10" Width="41px" />
                        &nbsp;<asp:DropDownList ID="ddlTerm" runat="server" AutoPostBack="True" CssClass="DropDownList"
                            Width="154px">
                        </asp:DropDownList>
                        <asp:Label ID="Label4" runat="server"  ForeColor="Red" Text="*"></asp:Label>
                    </td>
                    <td class="style4">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td class="style1">
                        NPWP
                    </td>
                    <td class="style2">
                        :
                    </td>
                    <td>
                        <asp:TextBox ID="tbNPWP" runat="server" CssClass="TextBox" MaxLength="30" Width="225px" />
                    </td>
                    <td class="style4">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td class="style1">
                        FPCustKode
                    </td>
                    <td class="style2">
                        :
                    </td>
                    <td>
                        <asp:TextBox ID="tbFPCustKode" runat="server" CssClass="TextBox" MaxLength="4" Width="225px" />
                    </td>
                    <td class="style4">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td class="style1">
                        Active
                    </td>
                    <td class="style2">
                        :
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlAktive" runat="server" AutoPostBack="True" CssClass="DropDownList"
                            Width="44px">
                            <asp:ListItem>Y</asp:ListItem>
                            <asp:ListItem>N</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td class="style4">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td class="style1">
                        Price By SO
                    </td>
                    <td class="style2">
                        :
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlPriceBySO" runat="server" AutoPostBack="True" CssClass="DropDownList"
                            Width="44px">
                            <asp:ListItem>Y</asp:ListItem>
                            <asp:ListItem>N</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td class="style4">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td class="style7">
                        Contact Name
                    </td>
                    <td class="style8">
                        :
                    </td>
                    <td class="style9">
                        <asp:TextBox ID="tbCName" runat="server" CssClass="TextBox" MaxLength="50" Width="225px" />
                        <asp:Label ID="Label10" runat="server" ForeColor="Red" Text="*"></asp:Label>
                    </td>
                    <td class="style10">
                    </td>
                    
                    <td class="style7">
                        NIK
                    </td>
                    <td class="style8">
                        :
                    </td>
                    <td class="style9">
                        <asp:TextBox ID="tbNik" runat="server" CssClass="TextBox" MaxLength="50" Width="225px" />
                        <asp:Label ID="Label20" runat="server" ForeColor="Red" Text="*"></asp:Label>
                    </td>
                    <td class="style10">
                    </td>
                </tr>
                <tr>
                    <td class="style1">
                        Contact Title
                    </td>
                    <td class="style2">
                        :
                    </td>
                    <td>
                        <asp:TextBox ID="tbContactTitle" runat="server" CssClass="TextBox" MaxLength="50"
                            Width="225px" />
                        <asp:Label ID="Label9" runat="server" ForeColor="Red" Text="*"></asp:Label>
                    </td>
                    <td class="style4">
                        &nbsp;
                    </td>
                    
                     <td class="style1">
                        Tempat Tgl Lahir
                    </td>
                    <td class="style2">
                        :
                    </td>
                    <td>
                        <asp:TextBox ID="tbTempat" runat="server" CssClass="TextBox" MaxLength="50"
                            Width="100px" />
                        <BDP:BasicDatePicker ID="tbDateLahir" runat="server"  DateFormat="dd MMM yyyy" 
                                    ReadOnly = "true" ValidationGroup="Input"
                                    ButtonImageHeight="19px" ButtonImageWidth="20px" 
                                    DisplayType="TextBox" 
                                    TextBoxStyle-CssClass="TextDate" AutoPostBack="False" 
                                    ShowNoneButton="False"><TextBoxStyle CssClass="TextDate" Width="115px" /></BDP:BasicDatePicker>
                        <asp:Label ID="Label21" runat="server" ForeColor="Red" Text="*"></asp:Label>
                    </td>
                    <td class="style4">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td class="style1">
                        Contact Phone
                    </td>
                    <td class="style2">
                        :
                    </td>
                    <td>
                        <asp:TextBox ID="tbContactHp" runat="server" CssClass="TextBox" MaxLength="40" Width="225px" />
                    </td>
                    <td class="style4">
                        &nbsp;
                    </td>
                    
                    <td class="style1">
                        Warga Negara
                    </td>
                    <td class="style2">
                        :
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlWargaNegara" runat="server" CssClass="DropDownList" Width="230px">
                            <asp:ListItem>WNI</asp:ListItem>
                            <asp:ListItem>WNA</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="style1">
                        Contact Email
                    </td>
                    <td class="style2">
                        :
                    </td>
                    <td>
                        <asp:TextBox ID="tbCEmail" runat="server" CssClass="TextBox" MaxLength="50" Width="225px" />
                    </td>
                    <td class="style4">
                    </td>
                </tr>
                <tr>
                    <td class="style1">
                        &nbsp;
                    </td>
                    <td class="style2">
                        &nbsp;
                    </td>
                    <td>
                        <%--<asp:Button ID="btnSaveHd" runat="server" CssClass="Button" Text="Save" />--%>
                        <asp:Button ID="btnSaveHd" runat="server" class="bitbtndt btnsave" Text="Save" />
                        <%--<asp:Button ID="btnCancelHd" runat="server" CssClass="Button" Text="Cancel" />--%>
                        <asp:Button ID="btnCancelHd" runat="server" class="bitbtndt btncancel" Text="Cancel" />
                        <%--<asp:Button ID="btnReset" runat="server" CssClass="Button" Text="Reset" />--%>
                        <asp:Button ID="btnReset" runat="server" class="bitbtndt btndelete" Text="Reset" Height ="28px" />
                    </td>
                    <td class="style4">
                        &nbsp;
                    </td>
                    <td class="style6">
                        &nbsp;
                    </td>
                    <td class="style5">
                        &nbsp;
                    </td>
                    <td class="style3">
                        &nbsp;
                    </td>
                </tr>
            </table>
            <%--<asp:Button ID="btnBack" runat="server" CssClass="Button" Text="Back" />--%>
            <asp:Button class="bitbtndt btnback" runat="server" ID="btnBack" Text="Back" />
            <br />
            <br />
            <asp:Menu ID="Menu1" runat="server" CssClass="Menu" ItemWrap="False" Orientation="Horizontal"
                StaticEnableDefaultPopOutImage="False" StaticMenuItemStyle-CssClass="MenuItem"
                StaticSelectedStyle-CssClass="MenuSelect">
                <StaticSelectedStyle CssClass="MenuSelect" />
                <StaticMenuItemStyle CssClass="MenuItem" />
                <Items>
                    <%--<asp:MenuItem Text="Detail Limit" Value="5"></asp:MenuItem>--%><asp:MenuItem
                        Text="Detail Contact" Value="0"></asp:MenuItem>
                    <asp:MenuItem Text="Detail Bill To" Value="1"></asp:MenuItem>
                    <asp:MenuItem Text="Detail Address" Value="2"></asp:MenuItem>
                    <asp:MenuItem Text="Detail Field Info" Value="3"></asp:MenuItem>
                    <asp:MenuItem Text="Detail Tax Address" Value="4"></asp:MenuItem>
                    <asp:MenuItem Text="Detail Product" Value="5"></asp:MenuItem>
                </Items>
            </asp:Menu>
            <asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex="0">
                <asp:View ID="Tab1" runat="server">
                    <br>
                    <asp:Panel ID="pnlDt" runat="server">
                        <%--<div style="font-size:medium; color:Blue;">
                        Detail Contact</div>
                    <hr style="color:Blue" />--%>
                        <%--<asp:Button ID="btnAddDt" runat="server" CssClass="Button" Text="Add Item" />--%>
                        <asp:Button class="bitbtndt btnadd" runat="server" ID="btnadddt" Text="Add" CommandName='Insert' />
                        <div style="border: 0px  solid; width: 100%; height: 220px; overflow: auto;">
                            <asp:GridView ID="GridDt" runat="server" AutoGenerateColumns="False" ShowFooter="True">
                                <HeaderStyle CssClass="GridHeader" />
                                <RowStyle CssClass="GridItem" Wrap="false" />
                                <AlternatingRowStyle CssClass="GridAltItem" />
                                <PagerStyle CssClass="GridPager" />
                                <Columns>
                                    <asp:TemplateField HeaderText="Action">
                                        <ItemTemplate>
                                            <%--<asp:Button ID="btnEdit" Runat="server" CommandName="Edit" CssClass="Button" 
                                    Text="Edit" />--%>
                                            <asp:Button class="bitbtndt btnedit" runat="server" ID="btnEdit0" Text="Edit" CommandName="Edit" />
                                            <%--<asp:Button ID="btnDelete" Runat="server" CommandName="Delete" 
                                    CssClass="Button" Text="Delete" />--%>
                                            <asp:Button class="bitbtndt btndelete" runat="server" ID="btnDelete0" Text="Delete"
                                                CommandName="Delete" OnClientClick="return confirm('Sure to delete this data?');" />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <%--<asp:Button ID="btnUpdate" Runat="server" CommandName="Update" 
                                    CssClass="Button" Text="Update" />--%>
                                            <asp:Button class="bitbtndt btnupdate" runat="server" ID="btnUpdate0" Text="Save"
                                                CommandName="Update" />
                                            <%--<asp:Button ID="btnCancel" Runat="server" CommandName="Cancel" 
                                    CssClass="Button" Text="Cancel" />--%>
                                            <asp:Button class="bitbtndt btncancel" runat="server" ID="btnCancel0" Text="Cancel"
                                                CommandName="Cancel" />
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <%--<asp:Button ID="btnAdd" Runat="server" CommandName="Insert" CssClass="Button" --%>
                                            <asp:Button class="bitbtndt btnadd" runat="server" ID="btnadddt" OnClick="btnAddDt_Click"
                                                Text="Add" CommandName="Insert" />
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="ItemNo" HeaderText="No" />
                                    <asp:BoundField DataField="ContactType" HeaderText="Contact Type" SortExpression="ContactType" />
                                    <asp:BoundField DataField="ContactName" HeaderText="Contact Name" />
                                    <asp:BoundField DataField="ContactTitle" HeaderText="Contact Title" />
                                    <asp:BoundField DataField="Address1" HeaderText="Address 1" />
                                    <asp:BoundField DataField="Address2" HeaderText="Address 2" />
                                    <asp:BoundField DataField="ZipCode" HeaderText="Zip Code" />
                                    <asp:BoundField DataField="Phone" HeaderText="Phone" />
                                    <asp:BoundField DataField="Fax" HeaderText="Fax" />
                                    <asp:BoundField DataField="Email" HeaderText="Email" />
                                </Columns>
                            </asp:GridView>
                        </div>
                        <br />
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
                                    <asp:Label ID="lbItemNo" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Contact Type
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlContactType" runat="server" CssClass="DropDownList" Height="16px"
                                        Width="44px">
                                        <asp:ListItem>Mr</asp:ListItem>
                                        <asp:ListItem>Mrs</asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:Label ID="Label19" runat="server" ForeColor="Red" Text="*"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Contact Name
                                </td>
                                <td>
                                    :
                                </td>
                                <td>
                                    <asp:TextBox ID="tbContactName1" runat="server" CssClass="TextBox" MaxLength="50"
                                        Width="269px" />
                                    <asp:Label ID="Label17" runat="server" ForeColor="Red" Text="*"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Contact Title
                                </td>
                                <td>
                                    :
                                </td>
                                <td>
                                    <asp:TextBox ID="tbTitle" runat="server" CssClass="TextBox" Width="269px" MaxLength="50" />
                                    <asp:Label ID="Label18" runat="server" ForeColor="Red" Text="*"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Address 1
                                </td>
                                <td>
                                    :
                                </td>
                                <td>
                                    <asp:TextBox ID="tbContactAddr1" runat="server" CssClass="TextBox" MaxLength="100"
                                        Width="225px" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Address 2
                                </td>
                                <td>
                                    :
                                </td>
                                <td>
                                    <asp:TextBox ID="tbContactAddr2" runat="server" CssClass="TextBox" MaxLength="100"
                                        Width="225px" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Postal Code
                                </td>
                                <td>
                                    :
                                </td>
                                <td>
                                    <asp:TextBox ID="tbPostalCode" runat="server" CssClass="TextBox" MaxLength="10" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Telephone
                                </td>
                                <td>
                                    :
                                </td>
                                <td>
                                    <asp:TextBox ID="tbTelephone" runat="server" CssClass="TextBox" Width="136px" MaxLength="40" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Fax
                                </td>
                                <td>
                                    :
                                </td>
                                <td>
                                    <asp:TextBox ID="tbContactFax" runat="server" CssClass="TextBox" MaxLength="40" Width="138px" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Email
                                </td>
                                <td>
                                    :
                                </td>
                                <td>
                                    <asp:TextBox ID="tbContactEmail" runat="server" CssClass="TextBox" MaxLength="50"
                                        Width="267px" />
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
                        <%--<asp:Button runat="server" CssClass="Button" ID="btnSave" Text="Save" />--%>
                        <%--<asp:Button runat="server" CssClass="Button" ID="btnCancelDt" Text="Cancel" />--%></asp:Panel>
                </asp:View>
                <asp:View ID="Tab2" runat="server">
                    <br />
                    <asp:Panel ID="PnlBillTo" runat="server">
                        <%--<div style="font-size:medium; color:Blue;">
                                    Detail Bill To</div>
                                <hr style="color:Blue" />--%>
                        <%--<asp:Button ID="btnAddDt2" runat="server" CssClass="Button" Text="Add Item" />--%>
                        <asp:Button class="bitbtndt btnadd" runat="server" ID="btnAddBillTo" Text="Add" />
                        <div style="border: 0px  solid; width: 100%; height: 220px; overflow: auto;">
                            <asp:GridView ID="GridViewBillTo" runat="server" AutoGenerateColumns="False" ShowFooter="True">
                                <HeaderStyle CssClass="GridHeader" />
                                <RowStyle CssClass="GridItem" Wrap="false" />
                                <AlternatingRowStyle CssClass="GridAltItem" />
                                <PagerStyle CssClass="GridPager" />
                                <Columns>
                                    <asp:TemplateField HeaderText="Action">
                                        <ItemTemplate>
                                            <%--<asp:Button ID="btnEdit" Runat="server" CommandName="Edit" CssClass="Button" 
                                    Text="Edit" />--%>
                                            <asp:Button class="bitbtndt btnedit" runat="server" ID="btnEdit1" Text="Edit" CommandName="Edit" />
                                            <%--<asp:Button ID="btnDelete" Runat="server" CommandName="Delete" 
                                    CssClass="Button" Text="Delete" />--%>
                                            <asp:Button class="bitbtndt btndelete" runat="server" ID="btnDelete1" Text="Delete"
                                                CommandName="Delete" OnClientClick="return confirm('Sure to delete this data?');" />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <%--<asp:Button ID="btnUpdate" Runat="server" CommandName="Update" 
                                    CssClass="Button" Text="Update" />--%>
                                            <asp:Button class="bitbtndt btnupdate" runat="server" ID="btnUpdate1" Text="cancel"
                                                CommandName="Update" />
                                            <%--<asp:Button ID="btnCancel" Runat="server" CommandName="Cancel" 
                                    CssClass="Button" Text="Cancel" />--%>
                                            <asp:Button class="bitbtndt btncancel" runat="server" ID="btnCancel1" Text="Cancel"
                                                CommandName="Cancel" />
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <%--<asp:Button ID="btnAdd" Runat="server" CommandName="Insert" CssClass="Button" --%>
                                            <asp:Button class="bitbtndt btnadd" runat="server" ID="btnAdd2" Text="Add" OnClick="btnAddBillTo_Click"
                                                CommandName="Insert" />
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="CustCollect" HeaderText="Customer Collect" />
                                    <asp:BoundField DataField="CustName" HeaderText="Customer Collect Name" />
                                </Columns>
                            </asp:GridView>
                        </div>
                        <br />
                    </asp:Panel>
                    <asp:Panel ID="PnlEditBillTo" runat="server" Visible="false">
                        <table>
                            <tr>
                                <td>
                                    Customer
                                </td>
                                <td>
                                    :
                                </td>
                                <td>
                                    <asp:TextBox ID="tbBillTo" runat="server" AutoPostBack="True" MaxLength="12" CssClass="TextBox"
                                        Width="112px" />
                                    <asp:TextBox ID="tbBillToName" runat="server" CssClass="TextBox" Enabled="False"
                                        Width="225px" />
                                    <asp:Button class="bitbtndt btngo" runat="server" ID="btnBillTo" Text="..." />
                                    <asp:Label ID="Label16" runat="server" ForeColor="Red" Text="*"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3" style="text-align: center">
                                    <asp:Button class="bitbtndt btnsave" runat="server" ID="btnSaveBillTo" Text="Save" />
                                    <asp:Button class="bitbtndt btncancel" runat="server" ID="btnCancelBillTo" Text="Cancel" />
                                </td>
                            </tr>
                        </table>
                        <br />
                        <%--<asp:Button runat="server" CssClass="Button" ID="btnSaveAddr" Text="Save" />--%>
                        <%--<asp:Button runat="server" CssClass="Button" ID="btnCancelAddr" Text="Cancel" />            --%></asp:Panel>
                </asp:View>
                <asp:View ID="Tab3" runat="server">
                    <br />
                    <asp:Panel ID="PnlAddress" runat="server">
                        <%--<div style="font-size:medium; color:Blue;">
                                    Detail Address</div>
                                <hr style="color:Blue" />--%>
                        <%--<asp:Button ID="btnAddDt2" runat="server" CssClass="Button" Text="Add Item" />--%>
                        <asp:Button class="bitbtndt btnadd" runat="server" ID="btnAddAddress" Text="Add" />
                        <div style="border: 0px  solid; width: 100%; height: 220px; overflow: auto;">
                            <asp:GridView ID="GridViewAddr" runat="server" AutoGenerateColumns="False" ShowFooter="True">
                                <HeaderStyle CssClass="GridHeader" />
                                <RowStyle CssClass="GridItem" Wrap="false" />
                                <AlternatingRowStyle CssClass="GridAltItem" />
                                <PagerStyle CssClass="GridPager" />
                                <Columns>
                                    <asp:TemplateField HeaderText="Action">
                                        <ItemTemplate>
                                            <asp:Button class="bitbtndt btnedit" runat="server" ID="btnEdit2" Text="Edit" CommandName="Edit" />
                                            <asp:Button class="bitbtndt btndelete" runat="server" ID="btnDelete2" Text="Delete"
                                                CommandName="Delete" OnClientClick="return confirm('Sure to delete this data?');" />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:Button class="bitbtndt btnupdate" runat="server" ID="btnUpdate2" Text="cancel"
                                                CommandName="Update" />
                                            <asp:Button class="bitbtndt btncancel" runat="server" ID="btnCancel2" Text="cancel"
                                                CommandName="Cancel" />
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:Button class="bitbtndt btnadd" runat="server" ID="btnAdd5" Text="Add" OnClick="btnAddAddress_Click"
                                                CommandName="insert" />
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="DeliveryCode" HeaderText="Delivery Code" />
                                    <asp:BoundField DataField="DeliveryName" HeaderText="Delivery Name" />
                                    <asp:BoundField DataField="DeliveryType" HeaderText="Delivery Type" />
                                    <asp:BoundField DataField="DeliveryAddr1" HeaderText="Delivery Address" />
                                    <asp:BoundField DataField="DeliveryAddr2" HeaderText="Delivery Address 2" />
                                    <asp:BoundField DataField="City" HeaderText="City" />
                                    <asp:BoundField DataField="ZipCode" HeaderText="Zip Code" />
                                    <asp:BoundField DataField="PhoneNo" HeaderText="Phone" />
                                    <asp:BoundField DataField="Fax" HeaderText="Fax" />
                                    <asp:BoundField DataField="ContactPerson" HeaderText="Contact Person" />
                                </Columns>
                            </asp:GridView>
                            <br />
                        </div>
                        <br />
                    </asp:Panel>
                    <asp:Panel ID="PnlEditAddress" runat="server" Visible="false">
                        <table>
                            <tr>
                                <td>
                                    Delivery Code
                                </td>
                                <td>
                                    :
                                </td>
                                <td>
                                    <asp:TextBox ID="tbDeliveryCode" runat="server" CssClass="TextBox" MaxLength="12"
                                        Width="225px" />
                                    <asp:Label ID="Label11" runat="server" ForeColor="Red" Text="*"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Delivery Name
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    <asp:TextBox ID="tbDeliveryName" runat="server" CssClass="TextBox" MaxLength="60"
                                        Width="225px" />
                                    <asp:Label ID="Label12" runat="server" ForeColor="Red" Text="*"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Delivery Type
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlDeliveryType" runat="server" CssClass="DropDownList" Height="16px"
                                        Width="114px">
                                        <asp:ListItem>Pabrik</asp:ListItem>
                                        <asp:ListItem>Puri</asp:ListItem>
                                        <asp:ListItem>Office</asp:ListItem>
                                        <asp:ListItem>Toko</asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:Label ID="Label13" runat="server" ForeColor="Red" Text="*"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Delivery Addr 1
                                </td>
                                <td>
                                    :
                                </td>
                                <td>
                                    <asp:TextBox ID="tbAddress1Addr" runat="server" CssClass="TextBox" MaxLength="100"
                                        Width="225px" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Delivery Addr 2
                                </td>
                                <td>
                                    :
                                </td>
                                <td>
                                    <asp:TextBox ID="tbAddress2Addr" runat="server" CssClass="TextBox" MaxLength="100"
                                        Width="225px" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    City
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlCityAddr" runat="server" CssClass="DropDownList" Width="230px">
                                    </asp:DropDownList>
                                    <asp:Label ID="Label14" runat="server" ForeColor="Red" Text="*"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Zip Code
                                </td>
                                <td>
                                    :
                                </td>
                                <td>
                                    <asp:TextBox ID="tbZipCodeAddr" runat="server" CssClass="TextBox" MaxLength="10"
                                        onchange="calculate();" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Phone No
                                </td>
                                <td>
                                    :
                                </td>
                                <td>
                                    <asp:TextBox ID="TbPhoneAddr" runat="server" CssClass="TextBox" Width="136px" MaxLength="20" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Fax
                                </td>
                                <td>
                                    :
                                </td>
                                <td>
                                    <asp:TextBox ID="tbFaxAddr" runat="server" CssClass="TextBox" Width="138px" MaxLength="20" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Contact Person
                                </td>
                                <td>
                                    :
                                </td>
                                <td>
                                    <asp:TextBox ID="tbContactPersonAddr" runat="server" CssClass="TextBox" MaxLength="50"
                                        Width="138px" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3" style="text-align: center">
                                    <asp:Button class="bitbtndt btnsave" runat="server" ID="btnSaveAddr" Text="Save" />
                                    <asp:Button class="bitbtndt btncancel" runat="server" ID="btnCancelAddr" Text="Cancel" />
                                </td>
                            </tr>
                        </table>
                        <br />
                        <%--<asp:Button runat="server" CssClass="Button" ID="btnSaveAddr" Text="Save" />--%>
                        <%--<asp:Button runat="server" CssClass="Button" ID="btnCancelAddr" Text="Cancel" />            --%></asp:Panel>
                </asp:View>
                <asp:View ID="Tab4" runat="server">
                    <br>
                    <asp:Panel runat="server" ID="PnlField">
                        <%--<div style="font-size:medium; color:Blue;">
                Detail Field Info</div>
            <hr style="color:Blue" />--%>
                        <%--<asp:Button ID="btnAddBank" runat="server" CssClass="Button" Text="Add Item" />--%>
                        <asp:Button class="bitbtndt btnadd" runat="server" ID="btnAddField" Text="Add" />
                        <div style="border: 0px  solid; width: 100%; height: 220px; overflow: auto;">
                            <asp:GridView ID="GridViewField" runat="server" AutoGenerateColumns="False" ShowFooter="True">
                                <HeaderStyle CssClass="GridHeader" />
                                <RowStyle CssClass="GridItem" Wrap="false" />
                                <AlternatingRowStyle CssClass="GridAltItem" />
                                <PagerStyle CssClass="GridPager" />
                                <Columns>
                                    <asp:TemplateField HeaderText="Action">
                                        <ItemTemplate>
                                            <%--<asp:Button ID="btnEdit" Runat="server" CommandName="Edit" CssClass="Button" 
                                    Text="Edit" />--%>
                                            <asp:Button class="bitbtndt btnedit" runat="server" ID="btnedit" Text="Edit" CommandName="edit" />
                                            <%--<asp:Button ID="btnDelete" Runat="server" CommandName="Delete" 
                                    CssClass="Button" Text="Delete" />--%>
                                            <asp:Button class="bitbtndt btndelete" runat="server" ID="btnDelete" Text="Delete"
                                                CommandName="delete" OnClientClick="return confirm('Sure to delete this data?');" />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <%--<asp:Button ID="btnUpdate" Runat="server" CommandName="Update" 
                                    CssClass="Button" Text="Update" />--%>
                                            <asp:Button class="bitbtndt btnupdate" runat="server" ID="btnUpdate" Text="Save"
                                                CommandName="Update" />
                                            <%--<asp:Button ID="btnCancel" Runat="server" CommandName="Cancel" 
                                    CssClass="Button" Text="Cancel" />--%>
                                            <asp:Button class="bitbtndt btncancel" runat="server" ID="btnCancel" Text="Cancel"
                                                CommandName="Cancel" />
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <%--<asp:Button ID="btnAdd" Runat="server" CommandName="Insert" CssClass="Button" 
                                    Text="Add Item" />--%>
                                            <asp:Button class="bitbtndt btnadd" runat="server" ID="btnAdd6" Text="Add" OnClick="btnAddField_Click"
                                                CommandName="Insert" />
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="CustField" HeaderText="Customer Field Code" SortExpression="CustField" />
                                    <asp:BoundField DataField="CustFieldName" HeaderText="Customer Field" SortExpression="CustFieldName" />
                                </Columns>
                            </asp:GridView>
                        </div>
                        <br />
                    </asp:Panel>
                    <asp:Panel runat="server" ID="PnlEditField" Visible="false">
                        <table>
                            <tr>
                                <td>
                                    Customer Field
                                </td>
                                <td>
                                    :
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlCustField" runat="server" CssClass="DropDownList" Height="16px"
                                        Width="225px">
                                    </asp:DropDownList>
                                    <asp:Label ID="Label15" runat="server" ForeColor="Red" Text="*"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3" style="text-align: center">
                                    <asp:Button class="bitbtndt btnsave" runat="server" ID="btnSaveField" Text="Save" />
                                    <asp:Button class="bitbtndt btncancel" runat="server" ID="btnCancelField" Text="Cancel" />
                                </td>
                            </tr>
                        </table>
                        <br />
                        <%--<asp:Button runat="server" CssClass="Button" ID="btnSaveDt3" Text="Save" />--%>
                        <%--<asp:Button runat="server" CssClass="Button" ID="btnCancelDt3" Text="Cancel" />            --%>
                        <br />
                        <br />
                    </asp:Panel>
                </asp:View>
                <asp:View ID="Tab5" runat="server">
                    <br />
                    <asp:Panel ID="pnlTax" runat="server">
                        <%--<div style="font-size:medium; color:Blue;">
                                    Detail Bill To</div>
                                <hr style="color:Blue" />--%>
                        <%--<asp:Button ID="btnAddDt2" runat="server" CssClass="Button" Text="Add Item" />--%>
                        <asp:Button class="bitbtndt btnadd" runat="server" ID="btnAddTax" Text="Add" />
                        <div style="border: 0px  solid; width: 100%; height: 220px; overflow: auto;">
                            <asp:GridView ID="GridViewTax" runat="server" AutoGenerateColumns="False" ShowFooter="True">
                                <HeaderStyle CssClass="GridHeader" />
                                <RowStyle CssClass="GridItem" Wrap="false" />
                                <AlternatingRowStyle CssClass="GridAltItem" />
                                <PagerStyle CssClass="GridPager" />
                                <Columns>
                                    <asp:TemplateField HeaderText="Action">
                                        <ItemTemplate>
                                            <asp:Button class="bitbtndt btnedit" runat="server" ID="btnEdit1" Text="Edit" CommandName="Edit" />
                                            <asp:Button class="bitbtndt btndelete" runat="server" ID="btnDelete1" Text="Delete"
                                                CommandName="Delete" OnClientClick="return confirm('Sure to delete this data?');" />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:Button class="bitbtndt btnupdate" runat="server" ID="btnUpdate1" Text="cancel"
                                                CommandName="Update" />
                                            <asp:Button class="bitbtndt btncancel" runat="server" ID="btnCancel1" Text="Cancel"
                                                CommandName="Cancel" />
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:Button class="bitbtndt btnadd" runat="server" ID="btnAdd2" Text="Add" OnClick="btnAddTax_Click"
                                                CommandName="Insert" />
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="CustTaxAddress" HeaderText="Tax Address" />
                                    <asp:BoundField DataField="CustTaxNpwp" HeaderText="Tax NPWP" />
                                </Columns>
                            </asp:GridView>
                        </div>
                        <br />
                    </asp:Panel>
                    <asp:Panel ID="PnlEditTax" runat="server" Visible="false">
                        <table>
                            <tr>
                                <td>
                                    Tax Address
                                </td>
                                <td>
                                    :
                                </td>
                                <td>
                                    <asp:TextBox ID="tbTaxAddress" runat="server" CssClass="TextBoxMulti" MaxLength="255"
                                        TextMode="MultiLine" ValidationGroup="Input" Width="350px" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Tax NPWP
                                </td>
                                <td>
                                    :
                                </td>
                                <td>
                                    <asp:TextBox ID="tbTaxNpwp" runat="server" CssClass="TextBox" Width="350px" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3" style="text-align: center">
                                    <asp:Button ID="btnSaveTax" runat="server" class="bitbtndt btnsave" Text="Save" />
                                    <asp:Button ID="btnCancelTax" runat="server" class="bitbtndt btncancel" Text="Cancel" />
                                </td>
                            </tr>
                        </table>
                        <br />
                        <%--<asp:Button runat="server" CssClass="Button" ID="btnSaveAddr" Text="Save" />--%>
                        <%--<asp:Button runat="server" CssClass="Button" ID="btnCancelAddr" Text="Cancel" />            --%></asp:Panel>
                </asp:View>
                <asp:View ID="View1" runat="server">
                </asp:View>
            </asp:MultiView>
            <br />
        </asp:Panel>
        <asp:Panel runat="server" ID="pnlPrint" Visible="false">
            <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" AutoDataBind="True"
                Height="1036px" Width="928px" />
        </asp:Panel>
        <br />
    </div>
    <asp:Label runat="server" ID="lbStatus" ForeColor="Red" />
    <asp:Label runat="server" ID="lbCode" ForeColor="Red" Visible="False" />
    </form>
</body>
</html>
