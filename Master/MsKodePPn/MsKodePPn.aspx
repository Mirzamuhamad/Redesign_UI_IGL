<%@ Page Language="VB" AutoEventWireup="false" CodeFile="MsKodePPn.aspx.vb" Inherits="MsKodePPn_MsKodePPn" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Kode PPn</title>

    <script src="../../Function/OpenDlg.js" type="text/javascript"></script>
    <script src="../../Function/Function.JS" type="text/javascript"></script>
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div class="Content">
        <div class="H1">
            Kode PPn</div>
        <hr style="color: Blue" />
        <table>
            <tr>
                <td style="width: 100px; text-align: right">
                    Quick Search :
                </td>
                <td>
                    <asp:TextBox runat="server" ID="tbFilter" CssClass="TextBox"/>
                    <asp:DropDownList runat="server" ID="ddlField" CssClass="DropDownList">
                        <asp:ListItem Selected="true" Text="PPn Code" Value="PPnCode"></asp:ListItem>
                        <asp:ListItem Text="PPn Name" Value="PPnName"></asp:ListItem>
                    </asp:DropDownList>
                    <asp:Button class="bitbtn btnsearch" runat="server" ID="btnSearch" Text="Search" />
                    <asp:Button class="btngo" runat="server" ID="btnExpand" Text="..." />
                    <asp:Button class="bitbtn btnprint" runat="server" ID="btnPrint" Text="Print" />
                    <%--<button id="btnSearch" runat="server" class="bitbtn btnsearch" accesskey="s">
                        <span style="text-decoration: underline;">S</span>earch</button>
                    <button id="btnExpand" runat="server" class="btngo" accesskey=".">
                        <span style="text-decoration: underline;">...</span></button>
                    <button id="btnPrint" runat="server" class="bitbtn btnprint" accesskey="p">
                        <span style="text-decoration: underline;">P</span>rint</button>--%>
                    <asp:Button ID="btnShowPopup" runat="server" Style="display: none" />
                    <cc1:ModalPopupExtender ID="ModalPopupExtender1" runat="server" PopupControlID="pnlFind"
                        DropShadow="True" TargetControlID="btnShowPopup" BackgroundCssClass="BackgroundStyle" />
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
                        <asp:TextBox runat="server" ID="tbfilter2" CssClass="TextBox" />
                        <asp:DropDownList runat="server" ID="ddlField2" CssClass="DropDownList">
                            <asp:ListItem Selected="true" Text="PPn Code" Value="PPnCode"></asp:ListItem>
                            <asp:ListItem Text="PPn Name" Value="PPnName"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <br />
        <asp:Label ID="lstatus" ForeColor="red" runat="server"></asp:Label>
        <asp:Panel runat="server" ID="pnl1" Visible="true">
            <div style="border-style: solid; border-color: inherit; border-width: 0px; width: 100%;
                height: 100%; overflow: auto;">
                <asp:GridView ID="DataGrid" runat="server" ShowFooter="True" AllowSorting="True"
                    AutoGenerateColumns="False" AllowPaging="True" CssClass="Grid">
                    <HeaderStyle CssClass="GridHeader"></HeaderStyle>
                    <RowStyle CssClass="GridItem" Wrap="false" />
                    <AlternatingRowStyle CssClass="GridAltItem" />
                    <FooterStyle CssClass="GridFooter" />
                    <PagerStyle CssClass="GridPager" HorizontalAlign="Center" />
                    <EmptyDataTemplate>
                    </EmptyDataTemplate>
                    <Columns>
                        <asp:TemplateField HeaderText="PPn Code" HeaderStyle-Width="80" SortExpression="PPnCode">
                            <ItemTemplate>
                                <asp:Label runat="server" ID="PPnCode" Text='<%# DataBinder.Eval(Container.DataItem, "PPnCode") %>'>
                                </asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:Label runat="server" ID="PPnCodeEdit" Text='<%# DataBinder.Eval(Container.DataItem, "PPnCode") %>'>
                                </asp:Label>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox ID="PPnCodeAdd" CssClass="TextBox" MaxLength="3" runat="Server" Width="100%" />
                                <cc1:TextBoxWatermarkExtender ID="PPnCodeAdd_WtExt" runat="server" Enabled="True"
                                    TargetControlID="PPnCodeAdd" WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                </cc1:TextBoxWatermarkExtender>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="PPn Name" HeaderStyle-Width="250px" SortExpression="PPnName">
                            <ItemTemplate>
                                <asp:Label runat="server" ID="PPnName" Text='<%# DataBinder.Eval(Container.DataItem, "PPnName") %>'>
                                </asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox runat="server" ID="PPnNameEdit" MaxLength="50" Width="100%" CssClass="TextBox"
                                    Text='<%# DataBinder.Eval(Container.DataItem, "PPnName") %>'>
                                </asp:TextBox>
                                <cc1:TextBoxWatermarkExtender ID="PPnNameEdit_WtExt" runat="server" Enabled="True"
                                    TargetControlID="PPnNameEdit" WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                </cc1:TextBoxWatermarkExtender>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox ID="PPnNameAdd" CssClass="TextBox" MaxLength="50" runat="Server" Width="100%" />
                                <cc1:TextBoxWatermarkExtender ID="PPnNameAdd_WtExt" runat="server" Enabled="True"
                                    TargetControlID="PPnNameAdd" WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                </cc1:TextBoxWatermarkExtender>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="PPn Paid" SortExpression="PPnPaid">
                            <ItemTemplate>
                                <asp:Label runat="server" ID="PPnPaid" Text='<%# DataBinder.Eval(Container.DataItem, "PPnPaid") %>'>
                                </asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:DropDownList ID="PPnPaidEdit" CssClass="DropDownList" Width="100%" runat="server"
                                    SelectedValue='<%# DataBinder.Eval(Container.DataItem, "PPnPaid") %>'>
                                    <asp:ListItem>Y</asp:ListItem>
                                    <asp:ListItem>N</asp:ListItem>
                                </asp:DropDownList>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:DropDownList ID="PPnPaidAdd" CssClass="DropDownList" Width="100%" runat="server">
                                    <asp:ListItem Selected="True">Y</asp:ListItem>
                                    <asp:ListItem>N</asp:ListItem>
                                </asp:DropDownList>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="PPn Code 1" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="90px"
                            SortExpression="PPnCode1">
                            <ItemTemplate>
                                <asp:Label runat="server" ID="PPnCode1" Text='<%# DataBinder.Eval(Container.DataItem, "PPnCode1") %>'>
                                </asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox runat="server" ID="PPnCode1Edit" MaxLength="5" CssClass="TextBox" Width="100%"
                                    Text='<%# DataBinder.Eval(Container.DataItem, "PPnCode1") %>'>
                                </asp:TextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox ID="PPnCode1Add" CssClass="TextBox" Width="100%" MaxLength="5" runat="Server"></asp:TextBox>
                                <cc1:TextBoxWatermarkExtender ID="PPnCode1Add_TextBoxWatermarkExtender" runat="server"
                                    Enabled="True" TargetControlID="PPnCode1Add" WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                </cc1:TextBoxWatermarkExtender>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Start Nominal 1" ItemStyle-HorizontalAlign="Right"
                            HeaderStyle-Width="90px" SortExpression="StartNominal1">
                            <ItemTemplate>
                                <asp:Label runat="server" ID="StartNominal1" Text='<%# DataBinder.Eval(Container.DataItem, "StartNominal1", "{0:#,##0.00}") %>'>
                                </asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox runat="server" ID="StartNominal1Edit" MaxLength="9" CssClass="TextBox"
                                    Width="100%" Text='<%# DataBinder.Eval(Container.DataItem, "StartNominal1", "{0:#,##0.00}") %>'>
                                </asp:TextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox ID="StartNominal1Add" CssClass="TextBox" Width="100%" MaxLength="9"
                                    runat="Server"></asp:TextBox>
                                <cc1:TextBoxWatermarkExtender ID="StartNominal1Add_TextBoxWatermarkExtender" runat="server"
                                    Enabled="True" TargetControlID="StartNominal1Add" WatermarkText="can't blank"
                                    WatermarkCssClass="Watermarked">
                                </cc1:TextBoxWatermarkExtender>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="End Nominal 1" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="90px"
                            SortExpression="EndNominal1">
                            <ItemTemplate>
                                <asp:Label runat="server" ID="EndNominal1" Text='<%# DataBinder.Eval(Container.DataItem, "EndNominal1", "{0:#,##0.00}") %>'>
                                </asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox runat="server" ID="EndNominal1Edit" MaxLength="9" CssClass="TextBox"
                                    Width="100%" Text='<%# DataBinder.Eval(Container.DataItem, "EndNominal1", "{0:#,##0.00}") %>'>
                                </asp:TextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox ID="EndNominal1Add" CssClass="TextBox" Width="100%" MaxLength="9" runat="Server"></asp:TextBox>
                                <cc1:TextBoxWatermarkExtender ID="EndNominal1Add_TextBoxWatermarkExtender" runat="server"
                                    Enabled="True" TargetControlID="EndNominal1Add" WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                </cc1:TextBoxWatermarkExtender>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="PPn Code 2" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="90px"
                            SortExpression="PPnCode2">
                            <ItemTemplate>
                                <asp:Label runat="server" ID="PPnCode2" Text='<%# DataBinder.Eval(Container.DataItem, "PPnCode2") %>'>
                                </asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox runat="server" ID="PPnCode2Edit" MaxLength="5" CssClass="TextBox" Width="100%"
                                    Text='<%# DataBinder.Eval(Container.DataItem, "PPnCode2") %>'>
                                </asp:TextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox ID="PPnCode2Add" CssClass="TextBox" Width="100%" MaxLength="5" runat="Server"></asp:TextBox>
                                <cc1:TextBoxWatermarkExtender ID="PPnCode2Add_TextBoxWatermarkExtender" runat="server"
                                    Enabled="True" TargetControlID="PPnCode2Add" WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                </cc1:TextBoxWatermarkExtender>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Start Nominal 2" ItemStyle-HorizontalAlign="Right"
                            HeaderStyle-Width="90px" SortExpression="StartNominal2">
                            <ItemTemplate>
                                <asp:Label runat="server" ID="StartNominal2" Text='<%# DataBinder.Eval(Container.DataItem, "StartNominal2", "{0:#,##0.00}") %>'>
                                </asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox runat="server" ID="StartNominal2Edit" MaxLength="9" CssClass="TextBox"
                                    Width="100%" Text='<%# DataBinder.Eval(Container.DataItem, "StartNominal2", "{0:#,##0.00}") %>'>
                                </asp:TextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox ID="StartNominal2Add" CssClass="TextBox" Width="100%" MaxLength="9"
                                    runat="Server"></asp:TextBox>
                                <cc1:TextBoxWatermarkExtender ID="StartNominal2Add_TextBoxWatermarkExtender" runat="server"
                                    Enabled="True" TargetControlID="StartNominal2Add" WatermarkText="can't blank"
                                    WatermarkCssClass="Watermarked">
                                </cc1:TextBoxWatermarkExtender>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="End Nominal 2" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="90px"
                            SortExpression="EndNominal2">
                            <ItemTemplate>
                                <asp:Label runat="server" ID="EndNominal2" Text='<%# DataBinder.Eval(Container.DataItem, "EndNominal2", "{0:#,##0.00}") %>'>
                                </asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox runat="server" ID="EndNominal2Edit" MaxLength="9" CssClass="TextBox"
                                    Width="100%" Text='<%# DataBinder.Eval(Container.DataItem, "EndNominal2", "{0:#,##0.00}") %>'>
                                </asp:TextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox ID="EndNominal2Add" CssClass="TextBox" Width="100%" MaxLength="9" runat="Server"></asp:TextBox>
                                <cc1:TextBoxWatermarkExtender ID="EndNominal2Add_TextBoxWatermarkExtender" runat="server"
                                    Enabled="True" TargetControlID="EndNominal2Add" WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                </cc1:TextBoxWatermarkExtender>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Action">
                            <ItemTemplate>
                                <asp:Button class="bitbtn btnedit" runat="server" ID="btnEdit" Text="Edit" CommandName="Edit" />
                                <asp:Button class="bitbtn btndelete" runat="server" ID="btnDelete" Text="Delete"
                                    CommandName="Delete" OnClientClick="return confirm('Sure to delete this data?');" />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:Button class="bitbtn btnsave" runat="server" ID="btnUpdate" Text="Save" CommandName="Update" />
                                <asp:Button class="bitbtn btncancel" runat="server" ID="btnCancel" Text="Cancel"
                                    CommandName="Cancel" />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:Button class="bitbtn btnadd" runat="server" ID="btnAdd" Text="Add" CommandName="Insert" />
                            </FooterTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </asp:Panel>
    </div>
    </form>
</body>
</html>
