<%@ Page Language="VB" AutoEventWireup="false" CodeFile="MsCashFlowMonth.aspx.vb"
    Inherits="Execute_Master_MsCashFlowMonth_MsCashFlowMonth" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Cash Flow Monthly</title>

    <script src="../../Function/OpenDlg.js" type="text/javascript"></script>
    <script src="../../Function/Function.JS" type="text/javascript"></script>

    <script type="text/javascript">    
        function openprintdlg() {
                var wOpens;
                wOpens = window.open("../../Rpt/PrintForm.Aspx", "List", "scrollbars=yes,resizable=yes,width=500,height=400");
                wOpens.moveTo(0, 0);
                wOpens.resizeTo(screen.width, screen.height);
         )      
    </script>

    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
 <link href="https://fonts.googleapis.com/css2?family=Muli:wght@300;400;600&display=swap" rel="stylesheet">
    <style type="text/css">
        .style2
        {
            width: 115px;
            text-align: right;
            height: 26px;
        }
        .style4
        {
            height: 26px;
        }
        .style6
        {
            width: 75px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div class="Content">
        <div class="H1">
            Cash Flow Monthly</div>
        <hr style="color: Blue" />
        <table>
            <tr>
                <td>
                    <asp:Panel runat="server" ID="pnlSaldo" BackColor="LightGray">
                        <table>
                            <tr>
                                <td style="text-align: left">
                                    <%--class="style3"--%>
                                    Year
                                </td>
                                <td>
                                    <%--class="style4"--%>
                                    :
                                </td>
                                <td>
                                    <%--class="style4"--%>
                                    <asp:DropDownList ID="ddlYear" runat="server" CssClass="DropDownList" AutoPostBack="true" />
                                </td>
                                <td style="text-align: right">
                                    <%--class="style6"--%>
                                    Month
                                </td>
                                <td>
                                    :
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlPeriod" runat="server" CssClass="DropDownList" AutoPostBack="true" />
                                </td>
                                <%--<td class="style2">
                                    Saldo Beginning :
                                </td>
                                <td>
                                    <asp:TextBox CssClass="TextBox" runat="server" ID="TbSaldoBegin" Width="100px" />
                                    <asp:Label ID="lblHome" runat="server" Text=""></asp:Label>
                                </td>
                                <td class="style6">
                                    <asp:Button ID="btnApply" runat="server" class="bitbtndt btnedit" Text="Apply" AutoPostBack="true" />
                                </td>--%>
                            </tr>
                        </table>
                    </asp:Panel>
                </td>
                <td>
                    
                </td>
            </tr>
        </table>
        <br />
        <asp:Menu ID="Menu1" runat="server" CssClass="Menu" ItemWrap="True" Orientation="Horizontal"
            StaticEnableDefaultPopOutImage="False" StaticMenuItemStyle-CssClass="MenuItem"
            StaticSelectedStyle-CssClass="MenuSelect">
            <StaticSelectedStyle CssClass="MenuSelect" />
            <StaticMenuItemStyle CssClass="MenuItem" />
            <Items>
                <asp:MenuItem Text="Beginning" Value="0"></asp:MenuItem>
                <asp:MenuItem Text="Forecast" Value="1"></asp:MenuItem>
            </Items>
        </asp:Menu>
        <br />
        <asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex="0">
            <asp:View ID="TabBeginning" runat="server">
                <div style="border: 0px  solid; width: 100%; height: 100%; overflow: auto;">
                    <asp:GridView ID="DataGridBegin" runat="server" ShowFooter="True" AllowSorting="True"
                        AutoGenerateColumns="False" AllowPaging="True" CssClass="Grid">
                        <HeaderStyle CssClass="GridHeader"></HeaderStyle>
                        <RowStyle CssClass="GridItem" Wrap="false" />
                        <AlternatingRowStyle CssClass="GridAltItem" />
                        <FooterStyle CssClass="GridFooter" />
                        <PagerStyle CssClass="GridPager" />
                        <Columns>
                            <%--<asp:TemplateField HeaderText="Currency" HeaderStyle-Width="80" SortExpression="Currency">
                                <ItemTemplate>
                                    <asp:DropDownList runat="server" ID="CurrencyEdit" Width="100%" CssClass="DropDownList"
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Currency") %>' DataSourceID="dsCurrency"
                                        DataTextField="Currency" DataValueField="Currency">
                                    </asp:DropDownList>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Total" HeaderStyle-Width="80" SortExpression="Total">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="Total" Text='<%# DataBinder.Eval(Container.DataItem, "Total", "{0:#,##0.00}") %>'>
                                    </asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Total" HeaderStyle-Width="80" SortExpression="Total">
                                <ItemTemplate>
                                    <asp:Button class="bitbtndt btnadd" runat="server" ID="btnAdd" Text="Add"
                                        CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" CommandName="Add"/>
                                </ItemTemplate>
                            </asp:TemplateField>--%>
                            
                            <asp:TemplateField HeaderText="Currency" HeaderStyle-Width="80" SortExpression="Currency">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="Currency" Text='<%# DataBinder.Eval(Container.DataItem, "Currency") %>'>
                                    </asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:DropDownList runat="server" ID="CurrencyEdit" Width="100%" CssClass="DropDownList"
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Currency") %>' DataSourceID="dsCurrency"
                                        DataTextField="Currency" DataValueField="Currency">
                                    </asp:DropDownList>
                                    
                                    <asp:DropDownList runat="server" ID="CurrencyEditTemp" Width="100%" CssClass="DropDownList"
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Currency") %>' DataSourceID="dsCurrency"
                                        DataTextField="Currency" DataValueField="Currency" Visible="false">
                                    </asp:DropDownList>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:DropDownList ID="CurrencyAdd" runat="Server" Width="100%" CssClass="DropDownList"
                                        DataSourceID="dsCurrency" DataTextField="Currency" DataValueField="Currency">
                                    </asp:DropDownList>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Total" HeaderStyle-Width="100" SortExpression="Total">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="Total" Text='<%# DataBinder.Eval(Container.DataItem, "Total", "{0:#,##0.00}") %>'>
                                    </asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="TotalEdit" runat="server" CssClass="TextBox" MaxLength="13" Width="100%"
                                        Text='<%# DataBinder.Eval(Container.DataItem, "Total", "{0:#,##0.00}")%>'>
                                    </asp:TextBox>
                                    <cc1:TextBoxWatermarkExtender ID="TotalEdit_WtExt" runat="server" Enabled="True"
                                        TargetControlID="TotalEdit" WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                    </cc1:TextBoxWatermarkExtender>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="TotalAdd" CssClass="TextBox" MaxLength="13" runat="Server" Width="100%" />
                                    <cc1:TextBoxWatermarkExtender ID="TotalAdd_WtExt" runat="server" Enabled="True" TargetControlID="TotalAdd"
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                    </cc1:TextBoxWatermarkExtender>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Action" HeaderStyle-Width="126">
                                <ItemTemplate>
                                    <asp:Button ID="btnEdit" runat="server" class="bitbtndt btnedit" Text="Edit" CommandName="Edit" />
                                    <asp:Button ID="btndelete" runat="server" class="bitbtndt btndelete" Text="Delete"
                                        OnClientClick="return confirm('Sure to delete this data?');" CommandName="Delete" />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Button ID="btnUpdate" runat="server" class="bitbtndt btnsave" Text="Save" CommandName="Update" />
                                    <asp:Button ID="btnCancel" runat="server" class="bitbtndt btncancel" Text="Cancel"
                                        CommandName="Cancel" />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:Button class="bitbtndt btnadd" runat="server" ID="btnAdd" Text="Add" CommandName="Insert" />
                                </FooterTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </asp:View>
            <asp:View ID="TabForecast" runat="server">
                <table>
                    <tr>
                        <td>
                            <asp:Panel runat="server" ID="pnlGetData" BackColor="LightGray">
                                <table>
                                    <tr>
                                        <td class="style4">
                                            Get Data From :
                                        </td>
                                        <td>
                                            <asp:DropDownList runat="server" CssClass="DropDownList" ID="ddlDataFrom">
                                                <asp:ListItem Selected="True">Previous Period</asp:ListItem>
                                                <asp:ListItem>Master Data</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            Existing Data Overwrite :
                                        </td>
                                        <td>
                                            <asp:DropDownList runat="server" CssClass="DropDownList" ID="ddlOverwrite">
                                                <asp:ListItem Selected="True">Y</asp:ListItem>
                                                <asp:ListItem>N</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:Button class="btngo" runat="server" ID="BtnGo" Text="G" AutoPostBack="true" />
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </td>
                    </tr>
                </table>
                <br>
                <div style="border: 0px  solid; width: 100%; height: 100%; overflow: auto;">
                    <asp:GridView ID="DataGrid" runat="server" ShowFooter="True" AllowSorting="True"
                        AutoGenerateColumns="False" AllowPaging="True" CssClass="Grid">
                        <HeaderStyle CssClass="GridHeader"></HeaderStyle>
                        <RowStyle CssClass="GridItem" Wrap="false" />
                        <AlternatingRowStyle CssClass="GridAltItem" />
                        <FooterStyle CssClass="GridFooter" />
                        <PagerStyle CssClass="GridPager" />
                        <Columns>
                            <asp:TemplateField HeaderText="Cash Flow" HeaderStyle-Width="135px" SortExpression="CashFlow">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="CashFlow" Text='<%# DataBinder.Eval(Container.DataItem, "CashFlow") %>'>
                                    </asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox runat="server" CssClass="TextBox" ID="CashFlowEdit" Width="100%" Text='<%# DataBinder.Eval(Container.DataItem, "CashFlow") %>' />
                                    <%--<asp:Button class="btngo" runat="server" ID="btnCashFlowEdit" Width="20px" Text="..." CommandName="SearchEdit" />--%>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox CssClass="TextBox" runat="server" ID="CashFlowAdd" Width="100px" AutoPostBack="true"
                                        OnTextChanged="tbCashFlow_TextChanged" />
                                    <asp:Button class="btngo" runat="server" ID="btnCashFlowAdd" Width="20px" Text="..."
                                        CommandName="SearchAdd" />
                                    <cc1:TextBoxWatermarkExtender ID="AccountAdd_TextBoxWatermarkExtender" runat="server"
                                        Enabled="True" TargetControlID="CashFlowAdd" WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                    </cc1:TextBoxWatermarkExtender>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Cash Flow Name" HeaderStyle-Width="250px" SortExpression="CashFlowName">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="CashFlowName" Width="100%" Text='<%# DataBinder.Eval(Container.DataItem, "CashFlowName") %>'>
                                    </asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Label runat="server" ID="CashFlowNameEdit" Width="100%" Text='<%# DataBinder.Eval(Container.DataItem, "CashFlowName") %>'>
                                    </asp:Label>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:Label runat="server" ID="CashFlowNameAdd">
                                    </asp:Label>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Days" HeaderStyle-Width="5px" SortExpression="Days">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="Days" Width="100%" Text='<%# DataBinder.Eval(Container.DataItem, "Days") %>'>
                                    </asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox runat="server" ID="DaysEdit" CssClass="TextBox" Width="100%" Text='<%# DataBinder.Eval(Container.DataItem, "Days") %>'>
                                    </asp:TextBox>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="DaysAdd" CssClass="TextBox" MaxLength="2" runat="Server" Width="100%" />
                                    <cc1:TextBoxWatermarkExtender ID="DaysAdd_WtExt" runat="server" Enabled="True" TargetControlID="DaysAdd"
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                    </cc1:TextBoxWatermarkExtender>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Currency" HeaderStyle-Width="80" SortExpression="Currency">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="Currency" Text='<%# DataBinder.Eval(Container.DataItem, "Currency") %>'>
                                    </asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:DropDownList runat="server" ID="CurrencyEdit" Width="100%" CssClass="DropDownList"
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Currency") %>' DataSourceID="dsCurrency"
                                        DataTextField="Currency" DataValueField="Currency">
                                    </asp:DropDownList>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:DropDownList ID="CurrencyAdd" runat="Server" Width="100%" CssClass="DropDownList"
                                        DataSourceID="dsCurrency" DataTextField="Currency" DataValueField="Currency">
                                    </asp:DropDownList>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Total" HeaderStyle-Width="100" SortExpression="Total">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="Total" Text='<%# DataBinder.Eval(Container.DataItem, "Total") %>'>
                                    </asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="TotalEdit" runat="server" CssClass="TextBox" MaxLength="13" Width="100%"
                                        Text='<%# DataBinder.Eval(Container.DataItem, "Total") %>'>
                                    </asp:TextBox>
                                    <cc1:TextBoxWatermarkExtender ID="TotalEdit_WtExt" runat="server" Enabled="True"
                                        TargetControlID="TotalEdit" WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                    </cc1:TextBoxWatermarkExtender>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="TotalAdd" CssClass="TextBox" MaxLength="13" runat="Server" Width="100%" />
                                    <cc1:TextBoxWatermarkExtender ID="TotalAdd_WtExt" runat="server" Enabled="True" TargetControlID="TotalAdd"
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                    </cc1:TextBoxWatermarkExtender>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Action" HeaderStyle-Width="126">
                                <ItemTemplate>
                                    <asp:Button ID="btnEdit" runat="server" class="bitbtndt btnedit" Text="Edit" CommandName="Edit" />
                                    <asp:Button ID="btndelete" runat="server" class="bitbtndt btndelete" Text="Delete"
                                        OnClientClick="return confirm('Sure to delete this data?');" CommandName="Delete" />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Button ID="btnUpdate" runat="server" class="bitbtndt btnsave" Text="Save" CommandName="Update" />
                                    <asp:Button ID="btnCancel" runat="server" class="bitbtndt btncancel" Text="Cancel"
                                        CommandName="Cancel" />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:Button class="bitbtndt btnadd" runat="server" ID="btnAdd" Text="Add" CommandName="Insert" />
                                </FooterTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </asp:View>
        </asp:MultiView>
        <br />
        <asp:Label ID="lblStatus" ForeColor="red" runat="server"></asp:Label>
    </div>
    <asp:SqlDataSource ID="dsCurrency" runat="server" SelectCommand="EXEC S_GetCurrency">
    </asp:SqlDataSource>
    </form>
</body>
</html>
