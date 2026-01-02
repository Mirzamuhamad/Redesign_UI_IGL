<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TrSettingAccount.aspx.vb"
    Inherits="Transaction_TrSettingAccount_TrSettingAccount" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="DevExpress.Web.ASPxGridView.v10.2, Version=10.2.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxGridView" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v10.2" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Setting Account</title>

    <script src="../../Function/Function.JS" type="text/javascript"></script>
    <script src="../../Function/OpenDlg.js" type="text/javascript"></script>
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
 <link href="https://fonts.googleapis.com/css2?family=Muli:wght@300;400;600&display=swap" rel="stylesheet">
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div class="Content">
        <div class="H1">
            Setting Account</div>
        <hr style="color: Blue" />
        <asp:Menu ID="Menu1" runat="server" CssClass="Menu" ItemWrap="True" Orientation="Horizontal"
            StaticEnableDefaultPopOutImage="False" StaticMenuItemStyle-CssClass="MenuItem"
            StaticSelectedStyle-CssClass="MenuSelect">
            <StaticSelectedStyle CssClass="MenuSelect" />
            <StaticMenuItemStyle CssClass="MenuItem" />
            <Items>
                <asp:MenuItem Text="Product Type" Value="0"></asp:MenuItem>
                <asp:MenuItem Text="Customer Group" Value="1"></asp:MenuItem>
                <asp:MenuItem Text="Supplier Type" Value="2"></asp:MenuItem>
                <asp:MenuItem Text="FA Sub Group" Value="3"></asp:MenuItem>
                <asp:MenuItem Text="Setup Account" Value="4"></asp:MenuItem>
            </Items>
        </asp:Menu>
        <br />
        <asp:MultiView ID="MV1" runat="server" ActiveViewIndex="0">
            <asp:View ID="TabProductBentuk" runat="server">
                <asp:Panel ID="pnlHd" runat="server">
                    <asp:Panel runat="server" ID="pnlCari" Visible="true">
                        <table>
                            <tr>
                                <td style="text-align: left;">
                                    Quick Search :
                                </td>
                                <td>
                                    <asp:TextBox CssClass="TextBox" runat="server" ID="tbFilter" />
                                    <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlField">
                                        <asp:ListItem Selected="true" Text="Product Type Code" Value="ProductMateri"></asp:ListItem>
                                        <asp:ListItem Text="Product Type Name" Value="MateriName"></asp:ListItem>
                                        <asp:ListItem Text="Wrhs Type" Value="WrhsType"></asp:ListItem>
                                        <asp:ListItem Text="Acc Invent" Value="AccInvent"></asp:ListItem>
                                        <asp:ListItem Text="Acc Invent Name" Value="C.AccountName"></asp:ListItem>
                                        <asp:ListItem Text="Acc Cogs" Value="AccCogs"></asp:ListItem>
                                        <asp:ListItem Text="Acc Cogs Name" Value="E.AccountName"></asp:ListItem>
                                        <asp:ListItem Text="Acc Adjust Cogs" Value="AccAdjustCOGS"></asp:ListItem>
                                        <asp:ListItem Text="Acc Adjust Cogs Name" Value="J.AccountName"></asp:ListItem>
                                        <asp:ListItem Text="Acc Transit SJ" Value="AccTransitSJ"></asp:ListItem>
                                        <asp:ListItem Text="Acc Transit SJ Name" Value="F.AccountName"></asp:ListItem>
                                        <asp:ListItem Text="Acc Sales" Value="AccSales"></asp:ListItem>
                                        <asp:ListItem Text="Acc Sales Name" Value="K.AccountName"></asp:ListItem>
                                        <asp:ListItem Text="Acc Disc" Value="AccDisc"></asp:ListItem>
                                        <asp:ListItem Text="Acc Disc Name" Value="P.AccountName"></asp:ListItem>
                                        <asp:ListItem Text="Acc Transit Wrhs" Value="AccTransitWrhs"></asp:ListItem>
                                        <asp:ListItem Text="Acc Transit Wrhs Name" Value="G.AccountName"></asp:ListItem>
                                        <asp:ListItem Text="Acc Transit PRetur" Value="AccTransitPRetur"></asp:ListItem>
                                        <asp:ListItem Text="Acc Transit PRetur Name" Value="H.AccountName"></asp:ListItem>
                                        <asp:ListItem Text="Acc Transit SRetur" Value="AccTransitSRetur"></asp:ListItem>
                                        <asp:ListItem Text="Acc Transit SRetur Name" Value="I.AccountName"></asp:ListItem>
                                        <%--<asp:ListItem Text="Acc PReturn" Value="AccPReturn"></asp:ListItem>
                                        <asp:ListItem Text="Acc PReturn Name" Value="J.AccountName"></asp:ListItem>--%>
                                        <asp:ListItem Text="Acc SReturn" Value="AccSReturn"></asp:ListItem>
                                        <asp:ListItem Text="Acc SReturn Name" Value="L.AccountName"></asp:ListItem>
                                        <asp:ListItem Text="Acc STCAdjust" Value="AccSTCAdjust"></asp:ListItem>
                                        <asp:ListItem Text="Acc STCAdjust Name" Value="M.AccountName"></asp:ListItem>
                                        <asp:ListItem Text="Acc STCLost" Value="AccSTCLost"></asp:ListItem>
                                        <asp:ListItem Text="Acc STCLost Name" Value="N.AccountName"></asp:ListItem>
                                        <asp:ListItem Text="Acc SampleExps" Value="AccSampleExps"></asp:ListItem>
                                        <asp:ListItem Text="Acc SampleExps Name" Value="O.AccountName"></asp:ListItem>
                                        <asp:ListItem Text="Acc WIP Labor" Value="AccWIPLabor"></asp:ListItem>
                                        <asp:ListItem Text="Acc WIP Labor Name" Value="P.AccountName"></asp:ListItem>
                                        <asp:ListItem Text="Acc WIP Labor2" Value="AccWIPLabor2"></asp:ListItem>
                                        <asp:ListItem Text="Acc WIP Labor2 Name" Value="Q.AccountName"></asp:ListItem>
                                        <asp:ListItem Text="Acc WIP FOH" Value="AccWIPFOH"></asp:ListItem>
                                        <asp:ListItem Text="Acc WIP FOH Name" Value="R.AccountName"></asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:Button class="bitbtn btnsearch" runat="server" ID="btnSearch" Text="Search" />
                                    <asp:Button class="btngo" runat="server" ID="btnExpand" Text="..." />
                                    <asp:Button class="bitbtn btnprint" runat="server" id="btnPrintPT" Text="Print" />
                                </td>
                            </tr>
                        </table>
                        <asp:Panel runat="server" ID="pnlSearch" Visible="false">
                            <table>
                                <tr>
                                    <td style="width: 100px; text-align: right">
                                        <asp:DropDownList runat="server" ID="ddlNotasi">
                                            <asp:ListItem Selected="true" Text="OR" Value="OR"></asp:ListItem>
                                            <asp:ListItem Text="AND" Value="AND"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:TextBox CssClass="Button" runat="server" ID="tbfilter2" />
                                        <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlField2">
                                            <asp:ListItem Selected="true" Text="Product Type Code" Value="ProductMateri"></asp:ListItem>
                                            <asp:ListItem Text="Product Type Name" Value="MateriName"></asp:ListItem>
                                            <asp:ListItem Text="Wrhs Type" Value="WrhsType"></asp:ListItem>
                                            <asp:ListItem Text="Acc Invent" Value="AccInvent"></asp:ListItem>
                                            <asp:ListItem Text="Acc Invent Name" Value="C.AccountName"></asp:ListItem>
                                            <asp:ListItem Text="Acc Cogs" Value="AccCogs"></asp:ListItem>
                                            <asp:ListItem Text="Acc Cogs Name" Value="E.AccountName"></asp:ListItem>
                                            <asp:ListItem Text="Acc Adjust Cogs" Value="AccAdjustCOGS"></asp:ListItem>
                                            <asp:ListItem Text="Acc Adjust Cogs Name" Value="J.AccountName"></asp:ListItem>
                                            <asp:ListItem Text="Acc Transit SJ" Value="AccTransitSJ"></asp:ListItem>
                                            <asp:ListItem Text="Acc Transit SJ Name" Value="F.AccountName"></asp:ListItem>
                                            <asp:ListItem Text="Acc Sales" Value="AccSales"></asp:ListItem>
                                            <asp:ListItem Text="Acc Sales Name" Value="K.AccountName"></asp:ListItem>
                                            <asp:ListItem Text="Acc Disc" Value="AccDisc"></asp:ListItem>
                                            <asp:ListItem Text="Acc Disc Name" Value="P.AccountName"></asp:ListItem>
                                            <asp:ListItem Text="Acc Transit Wrhs" Value="AccTransitWrhs"></asp:ListItem>
                                            <asp:ListItem Text="Acc Transit Wrhs Name" Value="G.AccountName"></asp:ListItem>
                                            <asp:ListItem Text="Acc Transit PRetur" Value="AccTransitPRetur"></asp:ListItem>
                                            <asp:ListItem Text="Acc Transit PRetur Name" Value="H.AccountName"></asp:ListItem>
                                            <asp:ListItem Text="Acc Transit SRetur" Value="AccTransitSRetur"></asp:ListItem>
                                            <asp:ListItem Text="Acc Transit SRetur Name" Value="I.AccountName"></asp:ListItem>
                                            <asp:ListItem Text="Acc SReturn" Value="AccSReturn"></asp:ListItem>
                                            <asp:ListItem Text="Acc SReturn Name" Value="L.AccountName"></asp:ListItem>
                                            <asp:ListItem Text="Acc STCAdjust" Value="AccSTCAdjust"></asp:ListItem>
                                            <asp:ListItem Text="Acc STCAdjust Name" Value="M.AccountName"></asp:ListItem>
                                            <asp:ListItem Text="Acc STCLost" Value="AccSTCLost"></asp:ListItem>
                                            <asp:ListItem Text="Acc STCLost Name" Value="N.AccountName"></asp:ListItem>
                                            <asp:ListItem Text="Acc SampleExps" Value="AccSampleExps"></asp:ListItem>
                                            <asp:ListItem Text="Acc SampleExps Name" Value="O.AccountName"></asp:ListItem>
                                            <asp:ListItem Text="Acc WIP Labor" Value="AccWIPLabor"></asp:ListItem>
                                            <asp:ListItem Text="Acc WIP Labor Name" Value="P.AccountName"></asp:ListItem>
                                            <asp:ListItem Text="Acc WIP Labor2" Value="AccWIPLabor2"></asp:ListItem>
                                            <asp:ListItem Text="Acc WIP Labor2 Name" Value="Q.AccountName"></asp:ListItem>
                                            <asp:ListItem Text="Acc WIP FOH" Value="AccWIPFOH"></asp:ListItem>
                                            <asp:ListItem Text="Acc WIP FOH Name" Value="R.AccountName"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </asp:Panel>
                    <br />
                    <asp:Panel ID="pnlDt" runat="server" Visible="true">
                        <asp:Button class="bitbtndt btnadd" Width="60px" runat="server" ID="btnAddDt" Text="Add"
                            CommandName="Insert" />
                        <div style="border: 0px  solid; width: 100%; height: 100%; overflow: auto;">
                            <asp:GridView ID="DataGridDt" runat="server" AllowPaging="True" AllowSorting="True"
                                AutoGenerateColumns="False" ShowFooter="False">
                                <HeaderStyle CssClass="GridHeader" Wrap="false" />
                                <RowStyle CssClass="GridItem" Wrap="false" />
                                <AlternatingRowStyle CssClass="GridAltItem" />
                                <FooterStyle CssClass="GridFooter" Wrap="false" />
                                <PagerStyle CssClass="GridPager" />
                                <Columns>
                                    <asp:TemplateField HeaderStyle-Width="110">
                                        <ItemTemplate>
                                            <asp:DropDownList CssClass="DropDownList" ID="ddl" runat="server">
                                                <asp:ListItem Selected="True" Text="Edit" />
                                                <asp:ListItem>Delete</asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:Button class="btngo" runat="server" ID="btnExpand" Text="G" CommandName="Go"
                                                CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" />
                                        </ItemTemplate>
                                        <HeaderStyle Width="110px" />
                                    </asp:TemplateField>

                                    <asp:BoundField DataField="MateriCode" SortExpression="MateriCode" HeaderText="Product Type" />
                                    <asp:BoundField DataField="MateriName" SortExpression="MateriName" HeaderText="Product TYpe Name" />
                                    <asp:BoundField DataField="WrhsType" SortExpression="WrhsType" HeaderText="Wrhs Type" />    
                                    <asp:BoundField DataField="AccInvent" SortExpression="AccInvent" HeaderText="Acc. Invent" />
                                    <asp:BoundField DataField="AccInventName" SortExpression="AccInventName" HeaderText="Acc. Invent Name" />
                                    <asp:BoundField DataField="AccCogs" SortExpression="AccCogs" HeaderText="Acc. Cogs" />
                                    <asp:BoundField DataField="AccCogsName" SortExpression="AccCogsName" HeaderText="Acc. Cogs Name" />
                                    <asp:BoundField DataField="AccTransitSJ" SortExpression="AccTransitSJ" HeaderText="Acc. Transit SJ" />                    
                                    <asp:BoundField DataField="AccTransitSJName" SortExpression="AccTransitSJName" HeaderText="Acc. Transit SJ Name" />
                                    <asp:BoundField DataField="AccSales" SortExpression="AccSales" HeaderText="Acc. Sales" />
                                    <asp:BoundField DataField="AccSalesName" SortExpression="AccSalesName" HeaderText="Acc. Sales Name" />
                                    <asp:BoundField DataField="AccTransitWrhs" SortExpression="AccTransitWrhs" HeaderText="Acc. Transit Wrhs" />
                                    <asp:BoundField DataField="AccTransitWrhsName" SortExpression="AccTransitWrhsName" HeaderText="Acc. Transit Wrhs Name" />
                                    <asp:BoundField DataField="AccTransitReject" SortExpression="AccTransitReject" HeaderText="Acc. Transit Reject" />
                                    <asp:BoundField DataField="AccTransitRejectName" SortExpression="AccTransitTRejectName" HeaderText="Acc. Transit Reject Name" />
                                    <asp:BoundField DataField="AccSRetur" SortExpression="AccTransitSRetur" HeaderText="Acc. Transit SRetur" />
                                    <asp:BoundField DataField="AccSReturName" SortExpression="AccTransitSReturName" HeaderText="Acc. Transit SRetur Name" />                    
                                    <asp:BoundField DataField="AccTransitRetur" SortExpression="AccTransitretur" HeaderText="Acc. TRetur" />
                                    <asp:BoundField DataField="AccTransitReturName" SortExpression="AccTransitreturName" HeaderText="Acc. TR Name" />
                                    <asp:BoundField DataField="AccExpLoss" SortExpression="AccSReturn" HeaderText="Acc. ExpLoss" />
                                    <asp:BoundField DataField="AccExpLossName" SortExpression="AccSReturnName" HeaderText="Acc. ExpLoss Name" />
                                        <asp:TemplateField HeaderStyle-Width="126px" HeaderText="Action" Visible="false" ItemStyle-Wrap="false">
                                        <EditItemTemplate>
                                            <asp:Button ID="btnUpdate0" runat="server" class="bitbtndt btnsave" Text="Save" CommandName="Update" />
                                            <asp:Button ID="btnCancel0" runat="server" class="bitbtndt btncancel" Text="Cancel" CommandName="Cancel" />
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Button ID="btnEdit0" runat="server" class="bitbtndt btnedit" Text="Edit" CommandName="Edit" />
                                            <asp:Button ID="btnDelete0" runat="server" class="bitbtndt btndelete" Text="Delete" OnClientClick="return confirm('Sure to delete this data?');" CommandName="Delete" />
                                        </ItemTemplate>
                                        <HeaderStyle Width="126px" />
                                        <ItemStyle Wrap="False" />
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                        <asp:Button class="bitbtndt btnadd" Width="60px" runat="server" ID="btnAddDt2" Text="Add" CommandName="Insert" />
                    </asp:Panel>
                    <asp:Panel ID="pnlInputDt" runat="server" Visible="false">
                        <table>
                            <tr>
                                <td>
                                    Product Type
                                </td>
                                <td>
                                    :
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlProductBentuk" runat="server" CssClass="DropDownList" />
                                    &nbsp;<asp:TextBox ID="tbProductJenis" runat="server" Visible = "False" 
                                        CssClass="TextBox" MaxLength="15" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Warehouse Type
                                </td>
                                <td>
                                    :
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlWrhsType" runat="server" CssClass="DropDownList" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Acc. Invent
                                </td>
                                <td>
                                    :
                                </td>
                                <td>
                                    <asp:TextBox ID="tbAccInvent" CssClass="TextBox" MaxLength="15" runat="server" AutoPostBack="True" />
                                    <asp:TextBox ID="tbAccInventName" CssClass="TextBox" MaxLength="60" Width="280" Enabled="False"
                                        runat="server" />
                                    <asp:Button ID="btnAccInvent" runat="server" class="btngo" Text="..." />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Acc. COGS
                                </td>
                                <td>
                                    :
                                </td>
                                <td>
                                    <asp:TextBox ID="tbAccCOGS" CssClass="TextBox" MaxLength="15" runat="server" AutoPostBack="True" />
                                    <asp:TextBox ID="tbAccCOGSName" CssClass="TextBox" MaxLength="60" Width="280" Enabled="False"
                                        runat="server" />
                                    <asp:Button ID="btnAccCOGS" runat="server" class="btngo" Text="..." />
                                </td>
                            </tr>
                             <tr>
                <td>Acc. S Retur</td>
                <td>:</td>
                <td>
                    <asp:TextBox ID="tbAccSRetur" CssClass="TextBox" MaxLength="15" runat="server" AutoPostBack="True" />
                    <asp:TextBox ID="tbAccSReturName" CssClass="TextBox" MaxLength="60" Width=280 Enabled="False" runat="server" />
                    <asp:Button ID="btnAccSRetur" runat="server" class="btngo" Text="..."/>
                </td>
            </tr>
            <tr>
                <td>Acc. Transit SJ</td>
                <td>:</td>
                <td>
                    <asp:TextBox ID="tbAccTransitSJ" CssClass="TextBox" MaxLength="15" runat="server" AutoPostBack="True" />
                    <asp:TextBox ID="tbAccTransitSJName" CssClass="TextBox" MaxLength="60" Width=280 Enabled="False" runat="server" />
                    <asp:Button ID="btnAccTransitSJ" runat="server" class="btngo" Text="..."/>
                </td>
            </tr>    
            <tr>
                <td>Acc. Sales</td>
                <td>:</td>
                <td>
                    <asp:TextBox ID="tbAccSales" CssClass="TextBox" MaxLength="15" runat="server" AutoPostBack="True" />
                    <asp:TextBox ID="tbAccSalesName" CssClass="TextBox" MaxLength="60" Width=280 Enabled="False" runat="server" />
                    <asp:Button ID="btnAccSales" runat="server" class="btngo" Text="..."/>
                </td>
            </tr>   
           <tr>
                <td>Acc. </td>
                <td>:</td>
                <td>
                    <asp:TextBox ID="tbAccTransitWrhs" CssClass="TextBox" MaxLength="15" runat="server" AutoPostBack="True" />
                    <asp:TextBox ID="tbAccTransitWrhsName" CssClass="TextBox" MaxLength="60" Width=280 Enabled="False" runat="server" />
                    <asp:Button ID="btnAccTransitWrhs" runat="server" class="btngo" Text="..."/>
                </td>
            </tr>                                           
            <tr>
                <td>Acc. Transit Reject</td>
                <td>:</td>
                <td>
                    <asp:TextBox ID="tbAccTransitReject" CssClass="TextBox" MaxLength="15" runat="server" AutoPostBack="True" />
                    <asp:TextBox ID="tbAccTransitRejectName" CssClass="TextBox" MaxLength="60" Width=280 Enabled="False" runat="server" />
                    <asp:Button ID="btnAccTransitReject" runat="server" class="btngo" Text="..."/>
                </td>
            </tr> 
            <tr>
                <td>Acc. Transit Retur</td>
                <td>:</td>
                <td>
                    <asp:TextBox ID="tbAccTransitRetur" CssClass="TextBox" MaxLength="15" runat="server" AutoPostBack="True" />
                    <asp:TextBox ID="tbAccTransitReturName" CssClass="TextBox" MaxLength="60" Width=280 Enabled="False" runat="server" />
                    <asp:Button ID="btnAccTransitRetur" runat="server" class="btngo" Text="..."/>
                </td>
            </tr> 
            
            <tr>
                <td>Acc. Exp Loss</td>
                <td>:</td>
                <td>
                    <asp:TextBox ID="tbAccExpLoss" CssClass="TextBox" MaxLength="15" runat="server" AutoPostBack="True" />
                    <asp:TextBox ID="tbAccExpLossName" CssClass="TextBox" MaxLength="60" Width=280 Enabled="False" runat="server" />
                    <asp:Button ID="btnAccExpLoss" runat="server" class="btngo" Text="..."/>
                </td>
            </tr> 
                            <tr>
                                <td>
                                    <asp:Button ID="btnCancel" runat="server" class="bitbtndt btncancel" Text="Back"
                                        CommandName="Cancel" />
                                </td>
                                <td align="center" colspan="2">
                                    <asp:Button ID="btnSave" runat="server" class="bitbtndt btnsave" Text="Save" />
                                    <asp:Button ID="btnReset" runat="server" class="bitbtndt btncancel" Text="Reset"
                                        CommandName="Reset" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </asp:Panel>
            </asp:View>
            <asp:View ID="TabCustomer" runat="server">
                <asp:Panel ID="pnlHdCustGroup" runat="server">
                    <asp:Panel runat="server" ID="pnlCariCustGroup" Visible="true">
                        <table>
                            <tr>
                                <td style="text-align: left;">
                                    Quick Search :
                                </td>
                                <td>
                                    <asp:TextBox CssClass="TextBox" runat="server" ID="tbFilterCustGroup" />
                                    <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlFieldCustGroup">
                                        <asp:ListItem Selected="true" Text="Customer Group Code" Value="CustGroup"></asp:ListItem>
                                        <asp:ListItem Text="Customer Group Name" Value="CustGroupName"></asp:ListItem>
                                        <asp:ListItem Text="Currency" Value="A.CurrCode"></asp:ListItem>
                                        <asp:ListItem Text="Acc AR" Value="AccAR"></asp:ListItem>
                                        <asp:ListItem Text="Acc AR Name" Value="C.AccountName"></asp:ListItem>
                                        <asp:ListItem Text="Acc SJ Uninvoice" Value="AccSJUninvoice"></asp:ListItem>
                                        <asp:ListItem Text="Acc SJ Uninvoice Name" Value="P.AccountName"></asp:ListItem>
                                        <asp:ListItem Text="Acc Disc" Value="AccDisc"></asp:ListItem>
                                        <asp:ListItem Text="Acc Disc Name" Value="L.AccountName"></asp:ListItem>
                                        <asp:ListItem Text="Acc Other" Value="AccOther"></asp:ListItem>
                                        <asp:ListItem Text="Acc Other Name" Value="M.AccountName"></asp:ListItem>
                                        <asp:ListItem Text="Acc Credit AR" Value="AccCreditAR"></asp:ListItem>
                                        <asp:ListItem Text="Acc Credit AR Name" Value="E.AccountName"></asp:ListItem>
                                        <asp:ListItem Text="Acc DP" Value="AccDP"></asp:ListItem>
                                        <asp:ListItem Text="Acc DP Name" Value="F.AccountName"></asp:ListItem>
                                        <asp:ListItem Text="Acc Deposit" Value="AccDeposit"></asp:ListItem> 
                                        <asp:ListItem Text="Acc Deposit Name" Value="G.AccountName"></asp:ListItem>
                                        <asp:ListItem Text="Acc PPN" Value="AccPPN"></asp:ListItem>
                                        <asp:ListItem Text="Acc PPN Name" Value="I.AccountName"></asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:Button class="bitbtn btnsearch" runat="server" ID="btnSearchCustGroup" Text="Search" />
                                    <asp:Button class="btngo" runat="server" ID="btnExpandCustGroup" Text="..." />
                                    <asp:Button class="bitbtn btnprint" runat="server" id="btnPrintCG" Text="Print" />
                                </td>
                            </tr>
                        </table>
                        <asp:Panel runat="server" ID="pnlSearchCustGroup" Visible="false">
                            <table>
                                <tr>
                                    <td style="width: 100px; text-align: right">
                                        <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlNotasiCustGroup">
                                            <asp:ListItem Selected="true" Text="OR" Value="OR"></asp:ListItem>
                                            <asp:ListItem Text="AND" Value="AND"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:TextBox CssClass="Button" runat="server" ID="tbfilter2CustGroup" />
                                        <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlField2CustGroup">
                                            <asp:ListItem Selected="true" Text="Customer Group Code" Value="CustGroup"></asp:ListItem>
                                            <asp:ListItem Text="Customer Group Name" Value="CustGroupName"></asp:ListItem>
                                            <asp:ListItem Text="Currency" Value="A.CurrCode"></asp:ListItem>
                                            <asp:ListItem Text="Acc AR" Value="AccAR"></asp:ListItem>
                                            <asp:ListItem Text="Acc AR Name" Value="C.AccountName"></asp:ListItem>
                                            <asp:ListItem Text="Acc SJ Uninvoice" Value="AccSJUninvoice"></asp:ListItem>
                                            <asp:ListItem Text="Acc SJ Uninvoice Name" Value="P.AccountName"></asp:ListItem>
                                            <asp:ListItem Text="Acc Disc" Value="AccDisc"></asp:ListItem>
                                            <asp:ListItem Text="Acc Disc Name" Value="L.AccountName"></asp:ListItem>
                                            <asp:ListItem Text="Acc Other" Value="AccOther"></asp:ListItem>
                                            <asp:ListItem Text="Acc Other Name" Value="M.AccountName"></asp:ListItem>
                                            <asp:ListItem Text="Acc Credit AR" Value="AccCreditAR"></asp:ListItem>
                                            <asp:ListItem Text="Acc Credit AR Name" Value="E.AccountName"></asp:ListItem>
                                            <asp:ListItem Text="Acc DP" Value="AccDP"></asp:ListItem>
                                            <asp:ListItem Text="Acc DP Name" Value="F.AccountName"></asp:ListItem>
                                            <asp:ListItem Text="Acc Deposit" Value="AccDeposit"></asp:ListItem>                
                                            <asp:ListItem Text="Acc Deposit Name" Value="G.AccountName"></asp:ListItem>
                                            <asp:ListItem Text="Acc PPN" Value="AccPPN"></asp:ListItem>
                                            <asp:ListItem Text="Acc PPN Name" Value="I.AccountName"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </asp:Panel>
                    <br />
                    <asp:Panel ID="pnlDtCustGroup" runat="server" Visible="true">
                        <asp:Button class="bitbtn btnadd" Width="60px" runat="server" ID="BtnAddCustGroup"
                            Text="Add" />
                        <br />
                        <div style="border: 0px  solid; width: 100%; height: 100%; overflow: auto;">
                            <asp:GridView ID="DataGridDtCustGroup" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                                AllowPaging="True" ShowFooter="False">
                                <HeaderStyle CssClass="GridHeader" Wrap="false" />
                                <RowStyle CssClass="GridItem" Wrap="false" />
                                <AlternatingRowStyle CssClass="GridAltItem" />
                                <FooterStyle CssClass="GridFooter" Wrap="false" />
                                <PagerStyle CssClass="GridPager" />
                                <Columns>
                                    <asp:TemplateField HeaderText="Action">
                                        <ItemTemplate>
                                            <asp:Button class="bitbtndt btnedit" runat="server" ID="btnEdit0" Text="Edit" CommandName="Edit" />
                                            <asp:Button class="bitbtndt btndelete" runat="server" ID="btnDelete0" Text="Delete"
                                                CommandName="Delete" OnClientClick="return confirm('Sure to delete this data?');" />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:Button class="bitbtndt btnupdate" runat="server" ID="btnUpdate0" Text="Save"
                                                CommandName="Update" />
                                            <asp:Button class="bitbtndt btncancel" runat="server" ID="btnCancel0" Text="Cancel"
                                                CommandName="Cancel" />
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:Button class="bitbtndt btnadd" runat="server" ID="btnadddt" Text="Add" CommandName="Insert" />
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="CustGroup" HeaderText="Cust Group" />
                                    <asp:BoundField DataField="CustGroupName" HeaderText="Cust Group Name" />
                                    <asp:BoundField DataField="CurrCode" HeaderText="Currency" />
                                    <asp:BoundField DataField="AccAR" HeaderText="Acc. AR" />
                                    <asp:BoundField DataField="AccARName" HeaderText="Acc. AR Name" />
                                    <asp:BoundField DataField="AccSJUninvoice" HeaderText="Acc. SJ Uninvoice" />
                                    <asp:BoundField DataField="AccSJUninvoiceName" HeaderText="Acc. SJ Uninvoice Name" />
                                    <asp:BoundField DataField="AccDisc" HeaderText="Acc. Disc" />
                                    <asp:BoundField DataField="AccDiscName" HeaderText="Acc. Disc Name" SortExpression="AccDiscName"
                                        HeaderStyle-HorizontalAlign="Center">
                                        <HeaderStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="AccOther" HeaderText="Acc. Other" SortExpression="AccOther" />
                                    <asp:BoundField DataField="AccOtherName" HeaderText="Acc. Other Name" SortExpression="AccOtherName" />
                                    <asp:BoundField DataField="AccCreditAR" HeaderText="Acc. Credit AR" SortExpression="AccCreditAR">
                                    </asp:BoundField>
                                    <asp:BoundField DataField="AccCreditARName" HeaderText="Acc. Credit AR Name" SortExpression="AccCreditARName">
                                    </asp:BoundField>
                                    <asp:BoundField DataField="AccDP" HeaderText="Acc. DP" SortExpression="AccDP" />
                                    <asp:BoundField DataField="AccDPName" HeaderText="Acc. DP Name" SortExpression="AccDPName" />
                                    <asp:BoundField DataField="AccDeposit" HeaderText="Acc. Deposit" SortExpression="AccDeposit" />
                                    <asp:BoundField DataField="AccDepositName" HeaderText="Acc. Deposit Name" SortExpression="AccDepositName" />
                                    <asp:BoundField DataField="AccPPN" HeaderText="Acc. PPN" SortExpression="AccPPN" />
                                    <asp:BoundField DataField="AccPPNName" HeaderText="Acc. PPN Name" SortExpression="AccPPNName" />
                                    <asp:BoundField DataField="AccPotongan" HeaderText="Acc. Potongan" SortExpression="AccPotongan" />
                                     <asp:BoundField DataField="AccPotonganName" HeaderText="Acc. Potongan Name" SortExpression="AccPotonganName" />
                                </Columns>
                            </asp:GridView>
                        </div>
                        <asp:Button class="bitbtn btnadd" Width="60px" runat="server" ID="BtnAdd2CustGroup"
                            Text="Add" />
                    </asp:Panel>
                    <asp:Panel runat="server" ID="PnlEditDetailCustGroup" Visible="false">
                        <table>
                            <tr>
                                <td>
                                    Customer Group
                                </td>
                                <td>
                                    :
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlCustGroup" runat="server" CssClass="DropDownList" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Currency
                                </td>
                                <td>
                                    :
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlCurrency" CssClass="DropDownList" runat="server" DataSourceID="dsCurrency"
                                        Width="50%" DataTextField="Currency" DataValueField="Currency" AutoPostBack="true"
                                        OnSelectedIndexChanged="ddlCurrency_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Acc. AR
                                </td>
                                <td>
                                    :
                                </td>
                                <td>
                                    <asp:TextBox ID="tbAccAR" runat="server" AutoPostBack="True" MaxLength="12" CssClass="TextBox"
                                        Width="112px" />
                                    <asp:TextBox ID="tbAccARName" runat="server" CssClass="TextBox" Enabled="False" Width="250px" />
                                    <asp:Button class="bitbtndt btngo" runat="server" ID="btnAccAR" Text="..." />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Acc. SJ Uninvoice
                                </td>
                                <td>
                                    :
                                </td>
                                <td>
                                    <asp:TextBox ID="tbAccSJUninvoice" runat="server" AutoPostBack="True" MaxLength="12"
                                        CssClass="TextBox" Width="112px" />
                                    <asp:TextBox ID="tbAccSJUninvoiceName" runat="server" CssClass="TextBox" Enabled="False"
                                        Width="250px" />
                                    <asp:Button class="bitbtndt btngo" runat="server" ID="btnAccSJUninvoice" Text="..." />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Acc. Disc
                                </td>
                                <td>
                                    :
                                </td>
                                <td>
                                    <asp:TextBox ID="tbAccDiscCustGroup" runat="server" AutoPostBack="True" MaxLength="12"
                                        CssClass="TextBox" Width="112px" />
                                    <asp:TextBox ID="tbAccDiscNameCustGroup" runat="server" CssClass="TextBox" Enabled="False"
                                        Width="250px" />
                                    <asp:Button class="bitbtndt btngo" runat="server" ID="btnAccDiscCustGroup" Text="..." />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Acc. Other
                                </td>
                                <td>
                                    :
                                </td>
                                <td>
                                    <asp:TextBox ID="tbAccOther" runat="server" AutoPostBack="True" MaxLength="12" CssClass="TextBox"
                                        Width="112px" />
                                    <asp:TextBox ID="tbAccOtherName" runat="server" CssClass="TextBox" Enabled="False"
                                        Width="250px" />
                                    <asp:Button class="bitbtndt btngo" runat="server" ID="btnAccOther" Text="..." />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Acc. Credit AR
                                </td>
                                <td>
                                    :
                                </td>
                                <td>
                                    <asp:TextBox ID="tbAccCreditAR" runat="server" AutoPostBack="True" MaxLength="12"
                                        CssClass="TextBox" Width="112px" />
                                    <asp:TextBox ID="tbAccCreditARName" runat="server" CssClass="TextBox" Enabled="False"
                                        Width="250px" />
                                    <asp:Button class="bitbtndt btngo" runat="server" ID="btnAccCreditAR" Text="..." />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Acc. DP
                                </td>
                                <td>
                                    :
                                </td>
                                <td>
                                    <asp:TextBox ID="tbAccDP" runat="server" AutoPostBack="True" MaxLength="12" CssClass="TextBox"
                                        Width="112px" />
                                    <asp:TextBox ID="tbAccDPName" runat="server" CssClass="TextBox" Enabled="False" Width="250px" />
                                    <asp:Button class="bitbtndt btngo" runat="server" ID="btnAccDP" Text="..." />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Acc. Deposit
                                </td>
                                <td>
                                    :
                                </td>
                                <td>
                                    <asp:TextBox ID="tbAccDeposit" runat="server" AutoPostBack="True" MaxLength="12"
                                        CssClass="TextBox" Width="112px" />
                                    <asp:TextBox ID="tbAccDepositName" runat="server" CssClass="TextBox" Enabled="False"
                                        Width="250px" />
                                    <asp:Button class="bitbtndt btngo" runat="server" ID="btnAccDeposit" Text="..." />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Acc. PPN
                                </td>
                                <td>
                                    :
                                </td>
                                <td>
                                    <asp:TextBox ID="tbAccPPN" runat="server" AutoPostBack="True" MaxLength="12" CssClass="TextBox"
                                        Width="112px" />
                                    <asp:TextBox ID="tbAccPPNName" runat="server" CssClass="TextBox" Enabled="False"
                                        Width="250px" />
                                    <asp:Button class="bitbtndt btngo" runat="server" ID="btnAccPPN" Text="..." />
                                </td>
                            </tr>
                            
                             <tr>
                               <td>Acc. Potongan</td>
                                <td>:</td>
                                <td>
                                <asp:TextBox ID="tbAccPotongan" runat="server" AutoPostBack="True" MaxLength="12"
                                  CssClass="TextBox" Width="112px" />
                                <asp:TextBox ID="tbAccPotonganName" runat="server" CssClass="TextBox" 
                                   Enabled="False" Width="250px" />
                                <asp:Button class="bitbtndt btngo" runat="server" ID="btnAccPotongan" Text="..." />									                                                                                                                                                                                                                                                        
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Button class="bitbtndt btncancel" runat="server" ID="btnCanceldtCustGroup" Text="Back" />
                                </td>
                                <td align="center" colspan="2">
                                    <asp:Button class="bitbtndt btnsave" runat="server" ID="btnSaveDtCustGroup" Text="Save" />
                                    <asp:Button ID="btnResetCustGroup" runat="server" class="bitbtndt btncancel" Text="Reset"
                                        CommandName="Reset" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </asp:Panel>
            </asp:View>
            <asp:View ID="TabSupplierType" runat="server">
                <asp:Panel runat="server" ID="pnlCariSuppType" Visible="true">
                    <table>
                        <tr>
                            <td style="text-align: left;">
                                Quick Search :
                            </td>
                            <td>
                                <asp:TextBox CssClass="TextBox" runat="server" ID="tbFilterSuppType" />
                                <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlFieldSuppType">
                                    <asp:ListItem Selected="true" Text="Supplier Type Code" Value="SuppType"></asp:ListItem>
                                    <asp:ListItem Text="Supplier Type Name" Value="SuppTypeName"></asp:ListItem>
                                    <asp:ListItem Text="Currency" Value="A.CurrCode"></asp:ListItem>
                                    <asp:ListItem Text="Acc AP" Value="Accap"></asp:ListItem>
                                    <asp:ListItem Text="Acc AP Name" Value="C.AccountName"></asp:ListItem>
                                    <asp:ListItem Text="Acc AP Pending" Value="Accappending"></asp:ListItem>
                                    <asp:ListItem Text="Acc AP Pending Name" Value="D.AccountName"></asp:ListItem>
                                    <asp:ListItem Text="Acc Debit AP" Value="AccDebitAP"></asp:ListItem>
                                    <asp:ListItem Text="Acc Debit AP Name" Value="E.AccountName"></asp:ListItem>
                                    <asp:ListItem Text="Acc DP" Value="AccDP"></asp:ListItem>
                                    <asp:ListItem Text="Acc DP Name" Value="F.AccountName"></asp:ListItem>
                                    <asp:ListItem Text="Acc Deposit" Value="AccDeposit"></asp:ListItem>
                                    <asp:ListItem Text="Acc Deposit Name" Value="G.AccountName"></asp:ListItem>
                                    <asp:ListItem Text="Acc VariantPO" Value="AccVariantPO"></asp:ListItem>
                                    <asp:ListItem Text="Acc VariantPO Name" Value="H.AccountName"></asp:ListItem>
                                    <asp:ListItem Text="Acc PPN" Value="Accppn"></asp:ListItem>
                                    <asp:ListItem Text="Acc PPN Name" Value="I.AccountName"></asp:ListItem>
                                    <asp:ListItem Text="Acc Freight" Value="AccFreight"></asp:ListItem>
                                    <asp:ListItem Text="Acc Freight Name" Value="J.AccountName"></asp:ListItem>
                                    <asp:ListItem Text="Acc Other" Value="AccOther"></asp:ListItem>
                                    <asp:ListItem Text="Acc Other Name" Value="M.AccountName"></asp:ListItem>
                                    <asp:ListItem Text="Acc PPH" Value="AccPPH"></asp:ListItem>
                                    <asp:ListItem Text="Acc PPH Name" Value="K.AccountName"></asp:ListItem>
                                    <asp:ListItem Text="Acc Disc" Value="AccDisc"></asp:ListItem>
                                    <asp:ListItem Text="Acc Disc Name" Value="L.AccountName"></asp:ListItem>
                                </asp:DropDownList>
                                <asp:Button class="bitbtn btnsearch" runat="server" ID="btnSearchSuppType" Text="Search" />
                                <asp:Button class="btngo" runat="server" ID="btnExpandSuppType" Text="..." />
                                <asp:Button class="bitbtn btnprint" runat="server" id="btnPrintST" Text="Print" />
                            </td>
                        </tr>
                    </table>
                    <asp:Panel runat="server" ID="pnlSearchSuppType" Visible="false">
                        <table>
                            <tr>
                                <td style="width: 100px; text-align: right">
                                    <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlNotasiSuppType">
                                        <asp:ListItem Selected="true" Text="OR" Value="OR"></asp:ListItem>
                                        <asp:ListItem Text="AND" Value="AND"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:TextBox CssClass="Button" runat="server" ID="tbfilter2SuppType" />
                                    <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlField2SuppType">
                                        <asp:ListItem Selected="true" Text="Supplier Type Code" Value="SuppType"></asp:ListItem>
                                        <asp:ListItem Text="Supplier Type Name" Value="SuppTypeName"></asp:ListItem>
                                        <asp:ListItem Text="Currency" Value="A.CurrCode"></asp:ListItem>
                                        <asp:ListItem Text="Acc AP" Value="Accap"></asp:ListItem>
                                        <asp:ListItem Text="Acc AP Name" Value="C.AccountName"></asp:ListItem>
                                        <asp:ListItem Text="Acc AP Pending" Value="Accappending"></asp:ListItem>
                                        <asp:ListItem Text="Acc AP Pending Name" Value="D.AccountName"></asp:ListItem>
                                        <asp:ListItem Text="Acc Debit AP" Value="AccDebitAP"></asp:ListItem>
                                        <asp:ListItem Text="Acc Debit AP Name" Value="E.AccountName"></asp:ListItem>
                                        <asp:ListItem Text="Acc DP" Value="AccDP"></asp:ListItem>
                                        <asp:ListItem Text="Acc DP Name" Value="F.AccountName"></asp:ListItem>
                                        <asp:ListItem Text="Acc Deposit" Value="AccDeposit"></asp:ListItem>
                                        <asp:ListItem Text="Acc Deposit Name" Value="G.AccountName"></asp:ListItem>
                                        <asp:ListItem Text="Acc VariantPO" Value="AccVariantPO"></asp:ListItem>
                                        <asp:ListItem Text="Acc VariantPO Name" Value="H.AccountName"></asp:ListItem>
                                        <asp:ListItem Text="Acc PPN" Value="Accppn"></asp:ListItem>
                                        <asp:ListItem Text="Acc PPN Name" Value="I.AccountName"></asp:ListItem>
                                        <asp:ListItem Text="Acc Freight" Value="AccFreight"></asp:ListItem>
                                        <asp:ListItem Text="Acc Freight Name" Value="J.AccountName"></asp:ListItem>
                                        <asp:ListItem Text="Acc Other" Value="AccOther"></asp:ListItem>
                                        <asp:ListItem Text="Acc Other Name" Value="M.AccountName"></asp:ListItem>
                                        <asp:ListItem Text="Acc PPH" Value="AccPPH"></asp:ListItem>
                                        <asp:ListItem Text="Acc PPH Name" Value="K.AccountName"></asp:ListItem>
                                        <asp:ListItem Text="Acc Disc" Value="AccDisc"></asp:ListItem>
                                        <asp:ListItem Text="Acc Disc Name" Value="L.AccountName"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </asp:Panel>
                <br />
                <asp:Panel ID="pnlDtSuppType" runat="server" Visible="true">
                    <asp:Button class="bitbtn btnadd" runat="server" ID="btnAddSuppType" Text="Add" />
                    <br />
                    <div style="border: 0px  solid; width: 100%; height: 100%; overflow: auto;">
                        <asp:GridView ID="DataGridDtSuppType" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                            AllowPaging="True" ShowFooter="false">
                            <HeaderStyle CssClass="GridHeader" Wrap="false"></HeaderStyle>
                            <RowStyle CssClass="GridItem" Wrap="false" />
                            <AlternatingRowStyle CssClass="GridAltItem" />
                            <FooterStyle CssClass="GridFooter" />
                            <PagerStyle CssClass="GridPager" />
                            <Columns>
                                <asp:TemplateField HeaderText="Action" HeaderStyle-Width="126px" ItemStyle-Wrap="false">
                                    <ItemTemplate>
                                        <asp:Button ID="btnEdit" runat="server" class="bitbtndt btnedit" Text="Edit" CommandName="Edit" />
                                        <asp:Button ID="btndelete" runat="server" class="bitbtndt btndelete" Text="Delete"
                                            OnClientClick="return confirm('Sure to delete this data?');" CommandName="Delete" />
                                    </ItemTemplate>
                                    <HeaderStyle Width="126px" />
                                    <ItemStyle Wrap="False" />
                                </asp:TemplateField>
                                <asp:BoundField DataField="SuppType" HeaderText="Supplier Type" HeaderStyle-Width="80"
                                    SortExpression="SuppType">
                                    <HeaderStyle Width="80px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="SuppTypeName" HeaderText="Supplier Type Name" HeaderStyle-Width="80"
                                    SortExpression="SuppTypeName">
                                    <HeaderStyle Width="80px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="CurrCode" HeaderText="Currency" HeaderStyle-Width="80"
                                    SortExpression="CurrCode">
                                    <HeaderStyle Width="80px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Accap" HeaderText="Acc. AP" HeaderStyle-Width="128" SortExpression="Accap">
                                    <HeaderStyle Width="128px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="AccapName" HeaderText="Acc. AP Name" HeaderStyle-Width="280"
                                    SortExpression="AccapName">
                                    <HeaderStyle Width="280px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Accappending" HeaderText="Acc. AP Pending" HeaderStyle-Width="128"
                                    SortExpression="Accappending">
                                    <HeaderStyle Width="128px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="AccappendingName" HeaderText="Acc. AP Pending Name" HeaderStyle-Width="280"
                                    SortExpression="AccappendingName">
                                    <HeaderStyle Width="280px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="AccDebitAP" HeaderText="Acc. Debit AP" HeaderStyle-Width="128"
                                    SortExpression="AccDebitAP">
                                    <HeaderStyle Width="128px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="AccDebitAPName" HeaderText="Acc. Debit AP Name" HeaderStyle-Width="280"
                                    SortExpression="AccDebitAPName">
                                    <HeaderStyle Width="280px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="AccDP" HeaderText="Acc. DP" HeaderStyle-Width="128" SortExpression="AccDP">
                                    <HeaderStyle Width="128px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="AccDPName" HeaderText="Acc. DP Name" HeaderStyle-Width="280"
                                    SortExpression="AccDPName">
                                    <HeaderStyle Width="280px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="AccDeposit" HeaderText="Acc. Deposit" HeaderStyle-Width="128"
                                    SortExpression="AccDeposit">
                                    <HeaderStyle Width="128px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="AccDepositName" HeaderText="Acc. Deposit Name" HeaderStyle-Width="280"
                                    SortExpression="AccDepositName">
                                    <HeaderStyle Width="280px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="AccVariantPO" HeaderText="Acc. Variant PO" HeaderStyle-Width="128"
                                    SortExpression="AccVariantPO">
                                    <HeaderStyle Width="128px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="AccVariantPOName" HeaderText="Acc. Variant PO Name" HeaderStyle-Width="280"
                                    SortExpression="AccVariantPOName">
                                    <HeaderStyle Width="280px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Accppn" HeaderText="Acc. PPN" HeaderStyle-Width="128"
                                    SortExpression="Accppn">
                                    <HeaderStyle Width="128px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="AccPPNName" HeaderText="Acc. PPN Name" HeaderStyle-Width="280"
                                    SortExpression="AccPPNName">
                                    <HeaderStyle Width="280px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="AccFreight" HeaderText="Acc. Freight" HeaderStyle-Width="128"
                                    SortExpression="AccFreight">
                                    <HeaderStyle Width="128px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="AccFreightName" HeaderText="Acc. Freight Name" HeaderStyle-Width="280"
                                    SortExpression="AccFreightName">
                                    <HeaderStyle Width="280px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="AccOther" HeaderText="Acc. Other" HeaderStyle-Width="128"
                                    SortExpression="AccOther">
                                    <HeaderStyle Width="128px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="AccOtherName" HeaderText="Acc. Other Name" HeaderStyle-Width="280"
                                    SortExpression="AccOtherName">
                                    <HeaderStyle Width="280px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="AccPPH" HeaderText="Acc. PPH" HeaderStyle-Width="128"
                                    SortExpression="AccPPH">
                                    <HeaderStyle Width="128px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="AccPPHName" HeaderText="Acc. PPH Name" HeaderStyle-Width="280"
                                    SortExpression="AccPPHName">
                                    <HeaderStyle Width="280px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="AccDisc" HeaderText="Acc. Disc" HeaderStyle-Width="128"
                                    SortExpression="AccDisc">
                                    <HeaderStyle Width="128px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="AccDiscName" HeaderText="Acc. Disc Name" HeaderStyle-Width="280"
                                    SortExpression="AccDiscName">
                                    <HeaderStyle Width="280px" />
                                </asp:BoundField>
                            </Columns>
                        </asp:GridView>
                    </div>
                    <asp:Button class="bitbtn btnadd" runat="server" ID="btnAdd2SuppType" Text="Add" />
                </asp:Panel>
                <asp:Panel runat="server" ID="pnlInputDtSuppType" Visible="false">
                    <table>
                        <tr>
                            <td>
                                Supplier Type
                            </td>
                            <td>
                                :
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlSuppType" runat="server" CssClass="DropDownList" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Currency
                            </td>
                            <td>
                                :
                            </td>
                            <td>
                                <asp:DropDownList runat="server" CssClass="DropDownList" ID="ddlCurrCodeDt" Width="160px"
                                    AutoPostBack="True">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Acc. AP
                            </td>
                            <td>
                                :
                            </td>
                            <td>
                                <asp:TextBox runat="server" MaxLength="12" CssClass="TextBox" ID="tbAccAP" Width="127px"
                                    AutoPostBack="True" />
                                <asp:TextBox runat="server" MaxLength="60" CssClass="TextBoxR" ID="tbAccAPName" Width="250px"
                                    ReadOnly="True" />
                                <asp:Button class="btngo" runat="server" ID="btnAccAP" Text="..." />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Acc. AP Pending
                            </td>
                            <td>
                                :
                            </td>
                            <td>
                                <asp:TextBox runat="server" MaxLength="12" CssClass="TextBox" ID="tbAccAPPending"
                                    Width="127px" AutoPostBack="True" />
                                <asp:TextBox runat="server" MaxLength="60" CssClass="TextBoxR" ID="tbAccAPPendingName"
                                    Width="250px" ReadOnly="True" />
                                <asp:Button class="btngo" runat="server" ID="btnAccAPPending" Text="..." />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Acc. Debit AP
                            </td>
                            <td>
                                :
                            </td>
                            <td>
                                <asp:TextBox runat="server" MaxLength="12" CssClass="TextBox" ID="tbAccDebitAP" Width="127px"
                                    AutoPostBack="True" />
                                <asp:TextBox runat="server" MaxLength="60" CssClass="TextBoxR" ID="tbAccDebitAPName"
                                    Width="250px" ReadOnly="True" />
                                <asp:Button class="btngo" runat="server" ID="btnAccDebitAP" Text="..." />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Acc. DP
                            </td>
                            <td>
                                :
                            </td>
                            <td>
                                <asp:TextBox runat="server" MaxLength="12" CssClass="TextBox" ID="tbAccDPSuppType"
                                    Width="127px" AutoPostBack="True" />
                                <asp:TextBox runat="server" MaxLength="60" CssClass="TextBoxR" ID="tbAccDPNameSuppType"
                                    Width="250px" ReadOnly="True" />
                                <asp:Button class="btngo" runat="server" ID="btnAccDPSuppType" Text="..." />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Acc. Deposit
                            </td>
                            <td>
                                :
                            </td>
                            <td>
                                <asp:TextBox runat="server" MaxLength="12" CssClass="TextBox" ID="tbAccDepositSuppType"
                                    Width="127px" AutoPostBack="True" />
                                <asp:TextBox runat="server" MaxLength="60" CssClass="TextBoxR" ID="tbAccDepositNameSuppType"
                                    Width="250px" ReadOnly="True" />
                                <asp:Button class="btngo" runat="server" ID="btnAccDepositSuppType" Text="..." />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Acc. Variant PO
                            </td>
                            <td>
                                :
                            </td>
                            <td>
                                <asp:TextBox runat="server" MaxLength="12" CssClass="TextBox" ID="tbAccVariantPO"
                                    Width="127px" AutoPostBack="True" />
                                <asp:TextBox runat="server" MaxLength="60" CssClass="TextBoxR" ID="tbAccVariantPOName"
                                    Width="250px" ReadOnly="True" />
                                <asp:Button class="btngo" runat="server" ID="btnAccVariantPO" Text="..." />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Acc. PPN
                            </td>
                            <td>
                                :
                            </td>
                            <td>
                                <asp:TextBox runat="server" MaxLength="12" CssClass="TextBox" ID="tbAccPPNSuppType"
                                    Width="127px" AutoPostBack="True" />
                                <asp:TextBox runat="server" MaxLength="60" CssClass="TextBoxR" ID="tbAccPPNNameSuppType"
                                    Width="250px" ReadOnly="True" />
                                <asp:Button class="btngo" runat="server" ID="btnAccPPNSuppType" Text="..." />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Acc. Freight
                            </td>
                            <td>
                                :
                            </td>
                            <td>
                                <asp:TextBox runat="server" MaxLength="12" CssClass="TextBox" ID="tbAccFreight" Width="127px"
                                    AutoPostBack="True" />
                                <asp:TextBox runat="server" MaxLength="60" CssClass="TextBoxR" ID="tbAccFreightName"
                                    Width="250px" ReadOnly="True" />
                                <asp:Button class="btngo" runat="server" ID="btnAccFreight" Text="..." />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Acc. Other
                            </td>
                            <td>
                                :
                            </td>
                            <td>
                                <asp:TextBox runat="server" MaxLength="12" CssClass="TextBox" ID="tbAccOtherSuppType"
                                    Width="127px" AutoPostBack="True" />
                                <asp:TextBox runat="server" MaxLength="60" CssClass="TextBoxR" ID="tbAccOtherNameSuppType"
                                    Width="250px" ReadOnly="True" />
                                <asp:Button class="btngo" runat="server" ID="btnAccOtherSuppType" Text="..." />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Acc. PPH
                            </td>
                            <td>
                                :
                            </td>
                            <td>
                                <asp:TextBox runat="server" MaxLength="12" CssClass="TextBox" ID="tbAccPPH" Width="127px"
                                    AutoPostBack="True" />
                                <asp:TextBox runat="server" MaxLength="60" CssClass="TextBoxR" ID="tbAccPPHName"
                                    Width="250px" ReadOnly="True" />
                                <asp:Button class="btngo" runat="server" ID="btnAccPPH" Text="..." />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Acc. Disc
                            </td>
                            <td>
                                :
                            </td>
                            <td>
                                <asp:TextBox runat="server" MaxLength="12" CssClass="TextBox" ID="tbAccDiscSuppType"
                                    Width="127px" AutoPostBack="True" />
                                <asp:TextBox runat="server" MaxLength="60" CssClass="TextBoxR" ID="tbAccDiscNameSuppType"
                                    Width="250px" ReadOnly="True" />
                                <asp:Button class="btngo" runat="server" ID="btnAccDiscSuppType" Text="..." />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Button ID="btnCancelSuppType" runat="server" class="bitbtndt btncancel" Text="Back"
                                    CommandName="Cancel" />
                            </td>
                            <td colspan="2" align="center">
                                <asp:Button ID="BtnSaveSuppType" runat="server" class="bitbtndt btnsave" Text="Save"
                                    CommandName="Update" />
                                <asp:Button ID="btnResetSuppType" runat="server" class="bitbtndt btncancel" Text="Reset"
                                    CommandName="Cancel" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </asp:View>
            <asp:View ID="TabFASubGroup" runat="server">
                <asp:Panel runat="server" ID="pnlCariFASubGroup" Visible="true">
                    <table>
                        <tr>
                            <td style="text-align: left;">
                                Quick Search :
                            </td>
                            <td>
                                <asp:TextBox CssClass="TextBox" runat="server" ID="tbFilterFASubGroup" />
                                <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlFieldFASubGroup">
                                    <asp:ListItem Selected="true" Text="FA Sub Group Code" Value="FASubGroup"></asp:ListItem>
                                    <asp:ListItem Text="FA Sub Group Name" Value="FASubGrpName"></asp:ListItem>
                                    <asp:ListItem Text="Currency" Value="A.CurrCode"></asp:ListItem>
                                    <asp:ListItem Text="Acc FA" Value="AccFA"></asp:ListItem>
                                    <asp:ListItem Text="Acc FA Name" Value="FA.AccountName"></asp:ListItem>
                                    <asp:ListItem Text="Acc Depr" Value="AccDepr"></asp:ListItem>
                                    <asp:ListItem Text="Acc Depr Name" Value="De.AccountName"></asp:ListItem>
                                    <asp:ListItem Text="Acc Akum Depr" Value="AccAkumDepr"></asp:ListItem>
                                    <asp:ListItem Text="Acc Akum Depr Name" Value="AK.AccountName"></asp:ListItem>
                                    <asp:ListItem Text="Acc Sales" Value="AccSales"></asp:ListItem>
                                    <asp:ListItem Text="Acc Sales Name" Value="SA.AccountName"></asp:ListItem>
                                </asp:DropDownList>
                                <asp:Button class="bitbtn btnsearch" runat="server" ID="btnSearchFASubGroup" Text="Search" />
                                <asp:Button class="btngo" runat="server" ID="btnExpandFASubGroup" Text="..." />
                                <asp:Button class="bitbtn btnprint" runat="server" id="btnPrintFSG" Text="Print" />
                            </td>
                        </tr>
                    </table>
                    <asp:Panel runat="server" ID="pnlSearchFASubGroup" Visible="false">
                        <table>
                            <tr>
                                <td style="width: 100px; text-align: right">
                                    <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlNotasiFASubGroup">
                                        <asp:ListItem Selected="true" Text="OR" Value="OR"></asp:ListItem>
                                        <asp:ListItem Text="AND" Value="AND"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:TextBox CssClass="Button" runat="server" ID="tbfilter2FASubGroup" />
                                    <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlField2FASubGroup">
                                        <asp:ListItem Selected="true" Text="FA Sub Group Code" Value="FASubGroup"></asp:ListItem>
                                        <asp:ListItem Text="FA Sub Group Name" Value="FASubGrpName"></asp:ListItem>
                                        <asp:ListItem Text="Currency" Value="A.CurrCode"></asp:ListItem>
                                        <asp:ListItem Text="Acc FA" Value="AccFA"></asp:ListItem>
                                        <asp:ListItem Text="Acc FA Name" Value="FA.AccountName"></asp:ListItem>
                                        <asp:ListItem Text="Acc Depr" Value="AccDepr"></asp:ListItem>
                                        <asp:ListItem Text="Acc Depr Name" Value="De.AccountName"></asp:ListItem>
                                        <asp:ListItem Text="Acc Akum Depr" Value="AccAkumDepr"></asp:ListItem>
                                        <asp:ListItem Text="Acc Akum Depr Name" Value="AK.AccountName"></asp:ListItem>
                                        <asp:ListItem Text="Acc Sales" Value="AccSales"></asp:ListItem>
                                        <asp:ListItem Text="Acc Sales Name" Value="SA.AccountName"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </asp:Panel>
                <%--<asp:Panel ID="PanelInfoFASubGroup" runat="server" Visible="false">
                    <asp:Label ID="label1" CssClass="H1" runat="server" Text="Supplier Type : " />
                    <asp:Label ID="lbGroupTypeCode" ForeColor="Blue" CssClass="H1" Font-Bold="true" runat="server" />
                </asp:Panel>--%>
                <br />
                <asp:Panel ID="pnlDtFASubGroup" runat="server" Visible="true">
                    <asp:Button class="bitbtn btnadd" runat="server" ID="btnAddFASubGroup" Text="Add" />
                    <br />
                    <div style="border: 0px  solid; width: 100%; height: 100%; overflow: auto;">
                        <asp:GridView ID="DataGridDtFASubGroup" runat="server" ShowFooter="True" AllowSorting="True"
                            AutoGenerateColumns="False" AllowPaging="True">
                            <HeaderStyle CssClass="GridHeader" Wrap="false"></HeaderStyle>
                            <RowStyle CssClass="GridItem" Wrap="false" />
                            <AlternatingRowStyle CssClass="GridAltItem" />
                            <FooterStyle CssClass="GridFooter" />
                            <PagerStyle CssClass="GridPager" />
                            <Columns>
                                <asp:TemplateField HeaderText="Action" HeaderStyle-Width="126px" ItemStyle-Wrap="false">
                                    <ItemTemplate>
                                        <asp:Button ID="btnEdit" runat="server" class="bitbtndt btnedit" Text="Edit" CommandName="Edit" />
                                        <asp:Button ID="btnDelete" runat="server" class="bitbtndt btndelete" Text="Delete"
                                            OnClientClick="return confirm('Sure to delete this data?');" CommandName="Delete" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="FASubGroup" HeaderText="FA Sub Group" HeaderStyle-Width="80"
                                    SortExpression="FASubGroup">
                                    <HeaderStyle Width="80px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="FASubGrpName" HeaderText="FA Sub Group Name" HeaderStyle-Width="80"
                                    SortExpression="FASubGrpName">
                                    <HeaderStyle Width="80px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="CurrCode" HeaderText="Currency" HeaderStyle-Width="80"
                                    SortExpression="CurrCode">
                                    <HeaderStyle Width="80px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="AccFA" HeaderText="Acc. FA" HeaderStyle-Width="128" SortExpression="AccFA">
                                    <HeaderStyle Width="128px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="AccFAName" HeaderText="Acc. FA Name" HeaderStyle-Width="280"
                                    SortExpression="AccFAName">
                                    <HeaderStyle Width="280px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="AccDepr" HeaderText="Acc. Depreciation" HeaderStyle-Width="128"
                                    SortExpression="AccDepr">
                                    <HeaderStyle Width="128px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="AccDeprName" HeaderText="Acc. Depreciation Name" HeaderStyle-Width="280"
                                    SortExpression="AccDeprName">
                                    <HeaderStyle Width="280px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="AccAkumDepr" HeaderText="Acc. Akum. Depr." HeaderStyle-Width="128"
                                    SortExpression="AccAkumDepr">
                                    <HeaderStyle Width="128px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="AccAkumDeprName" HeaderText="Acc. Akum. Depr. Name" HeaderStyle-Width="280"
                                    SortExpression="AccAkumDeprName">
                                    <HeaderStyle Width="280px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="AccSales" HeaderText="Acc. Sales" HeaderStyle-Width="128"
                                    SortExpression="AccSales">
                                    <HeaderStyle Width="128px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="AccSalesName" HeaderText="Acc. Sales Name" HeaderStyle-Width="280"
                                    SortExpression="AccSalesName">
                                    <HeaderStyle Width="280px" />
                                </asp:BoundField>
                            </Columns>
                        </asp:GridView>
                    </div>
                    <asp:Button class="bitbtn btnadd" runat="server" ID="btnAdd2FASubGroup" Text="Add" />
                </asp:Panel>
                <asp:Panel runat="server" ID="pnlInputDtFASubGroup" Visible="false">
                    <table>
                        <tr>
                            <td>
                                FA Sub Group
                            </td>
                            <td>
                                :
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlFASubGroup" runat="server" CssClass="DropDownList" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Currency
                            </td>
                            <td>
                                :
                            </td>
                            <td>
                                <asp:DropDownList runat="server" CssClass="DropDownList" ID="ddlCurrDtFASubGroup"
                                    Width="160px" AutoPostBack="True">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Acc. FA
                            </td>
                            <td>
                                :
                            </td>
                            <td>
                                <asp:TextBox runat="server" MaxLength="12" CssClass="TextBox" ID="tbAccFASubGroup"
                                    Width="127px" AutoPostBack="True" />
                                <asp:TextBox runat="server" MaxLength="60" CssClass="TextBoxR" ID="tbAccFANameFASubGroup"
                                    Width="250px" ReadOnly="True" />
                                <asp:Button class="btngo" runat="server" ID="btnAccFASubGroup" Text="..." />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Acc. Depreciation
                            </td>
                            <td>
                                :
                            </td>
                            <td>
                                <asp:TextBox runat="server" MaxLength="12" CssClass="TextBox" ID="tbAccDeprFASubGroup"
                                    Width="127px" AutoPostBack="True" />
                                <asp:TextBox runat="server" MaxLength="60" CssClass="TextBoxR" ID="tbAccDeprNameFASubGroup"
                                    Width="250px" ReadOnly="True" />
                                <asp:Button class="btngo" runat="server" ID="btnAccDeprFASubGroup" Text="..." />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Acc. Akum. Depreciation
                            </td>
                            <td>
                                :
                            </td>
                            <td>
                                <asp:TextBox runat="server" MaxLength="12" CssClass="TextBox" ID="tbAccAkumDeprFASubGroup"
                                    Width="127px" AutoPostBack="True" />
                                <asp:TextBox runat="server" MaxLength="60" CssClass="TextBoxR" ID="tbAccAkumDeprNameFASubGroup"
                                    Width="250px" ReadOnly="True" />
                                <asp:Button class="btngo" runat="server" ID="btnAccAkumDeprFASubGroup" Text="..." />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Acc. Sales
                            </td>
                            <td>
                                :
                            </td>
                            <td>
                                <asp:TextBox runat="server" MaxLength="12" CssClass="TextBox" ID="tbAccSalesFASubGroup"
                                    Width="127px" AutoPostBack="True" />
                                <asp:TextBox runat="server" MaxLength="60" CssClass="TextBoxR" ID="tbAccSalesNameFASubGroup"
                                    Width="250px" ReadOnly="True" />
                                <asp:Button class="btngo" runat="server" ID="btnAccSalesFASubGroup" Text="..." />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Button ID="btnCancelFASubGroup" runat="server" class="bitbtndt btncancel" Text="Back"
                                    CommandName="Cancel" />
                            </td>
                            <td colspan="2" align="center">
                                <asp:Button ID="BtnSaveFASubGroup" runat="server" class="bitbtndt btnsave" Text="Save"
                                    CommandName="Update" />
                                <asp:Button ID="btnResetFASubGroup" runat="server" class="bitbtndt btncancel" Text="Reset"
                                    CommandName="Cancel" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </asp:View>
            <asp:View ID="TabSetupAccount" runat="server">
                <asp:Panel ID="PnlMainSetupAcc" runat="server">
                    <dx:ASPxGridView ID="DGSetupAcc" runat="server" EnableCallBacks="false" KeyFieldName="SetCode"
                        emptydatatext="There are no data record(s) to display." allowpaging="True">
                        <Settings ShowFilterRow="True" ShowFilterBar="Auto" ShowFilterRowMenu="True" />
                        <SettingsBehavior AllowFocusedRow="True" />
                        <ClientSideEvents RowDblClick="function(s, e) {DGSetupAcc.PerformCallback(s.GetFocusedRowIndex());}" />
                        <Columns>
                            <dx:GridViewDataColumn FieldName="SetDescription" Caption="Setup Accout" VisibleIndex="1"
                                Width="230px">
                                <Settings AutoFilterCondition="Contains" />
                            </dx:GridViewDataColumn>
                            <dx:GridViewDataColumn FieldName="SetValue" Caption="Account" VisibleIndex="2" Width="60px">
                                <Settings AutoFilterCondition="Contains" />
                            </dx:GridViewDataColumn>
                            <dx:GridViewDataColumn FieldName="AccountName" Caption="Account Name" VisibleIndex="3"
                                Width="200px">
                                <Settings AutoFilterCondition="Contains" />
                            </dx:GridViewDataColumn>
                            <dx:GridViewDataColumn FieldName="SetRemark" Caption="Set Remark" VisibleIndex="4"
                                Visible="false" />
                            <dx:GridViewDataColumn FieldName="SetCode" Caption="Set Code" VisibleIndex="5" Visible="false" />
                            <dx:GridViewCommandColumn ButtonType="Image" Caption="#" VisibleIndex="6" Width="50px">
                                <CustomButtons>
                                    <dx:GridViewCommandColumnCustomButton ID="btnDelete" Text="Clear" Image-SpriteProperties-CssClass="btngo"
                                        Image-Url="../../Image/idelete.BMP" Image-ToolTip="Clear" />
                                </CustomButtons>
                            </dx:GridViewCommandColumn>
                        </Columns>
                    </dx:ASPxGridView>
                </asp:Panel>
                <asp:Panel ID="pnlInputSetupAcc" runat="server" Visible="false">
                    <table>
                        <tr>
                            <td>
                                Setup Account
                            </td>
                            <td>
                                :
                            </td>
                            <td>
                                <asp:Label ID="lbSetDescription" ForeColor="Blue" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Account
                            </td>
                            <td>
                                :
                            </td>
                            <td>
                                <asp:TextBox ID="tbSetupAcc" CssClass="TextBox" runat="server" Width="100px" AutoPostBack="True" />
                                <asp:TextBox ID="tbSetupAccName" CssClass="TextBox" Width="200px" runat="server"
                                    Enabled="false" />
                                <asp:Button ID="btnSearchSetupAcc" class="btngo" runat="server" Text="Search" Width="40px" />
                            </td>
                        </tr>
                        <tr>
                            <td align="center" colspan="3">
                                <asp:Button class="btngo" ID="btnSaveSetupAcc" runat="server" Text="Save" Width="39px" />
                                <asp:Button ID="btnCancelSetupAcc" class="btngo" runat="server" Text="Cancel" Width="39px" />
                                <asp:Button ID="btnResetSetupAcc" CssClass="btngo" runat="server" Text="Reset" Width="39px" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </asp:View>
        </asp:MultiView>
        
        <asp:SqlDataSource ID="dsProductCategory" runat="server" SelectCommand="EXEC S_GetProductCategory">
        </asp:SqlDataSource>
        <asp:SqlDataSource ID="dsPClass" runat="server" SelectCommand="EXEC S_GetProductClass">
        </asp:SqlDataSource>
        <asp:SqlDataSource ID="dsCurrency" runat="server" SelectCommand="EXEC S_GetCurrency">
        </asp:SqlDataSource>
        <asp:Label ID="lbstatus" ForeColor="red" runat="server"></asp:Label>
    </div>
    </form>
</body>
</html>
