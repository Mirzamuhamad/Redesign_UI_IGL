<%@ Page Language="VB" AutoEventWireup="false" CodeFile="SAUsers.aspx.vb" Inherits="Master_SAUsers_SAUsers" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>User File</title>

    <script src="../../Function/OpenDlg.JS" type="text/javascript"></script>
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true">
    </asp:ScriptManager>
    <div class="Content">
        <div class="H1">
            User File</div>
        <hr style="color: Blue" />
        <table>
            <tr>
                <td style="width: 100px; text-align: right">
                    Quick Search :
                </td>
                <td>
                    <asp:TextBox runat="server" ID="tbFilter" CssClass="TextBox" />
                    <asp:DropDownList runat="server" ID="ddlField" CssClass="DropDownList">
                        <asp:ListItem Selected="true" Text="User Code" Value="UserID"></asp:ListItem>
                        <asp:ListItem Text="User Name" Value="UserName"></asp:ListItem>
                        <asp:ListItem Text="User Group" Value="UserGrp_Name"></asp:ListItem>
                        <asp:ListItem Value="FgActive">Active</asp:ListItem>
                    </asp:DropDownList>
                    <asp:Button class="bitbtn btnsearch" runat="server" ID="btnSearch" Text="Search" />
                    <asp:Button class="btngo" runat="server" ID="btnExpand" Text="..." />
                    <asp:Button class="bitbtn btnprint" runat="server" ID="btnPrint" Text="Print" />
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
                            <asp:ListItem Selected="true" Text="User Code" Value="UserID"></asp:ListItem>
                            <asp:ListItem Text="User Name" Value="UserName"></asp:ListItem>
                            <asp:ListItem Text="User Group" Value="UserGrp_Name"></asp:ListItem>
                            <asp:ListItem Value="FgActive">Active</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <br />
        <div style="border: 0px  solid; width: 100%; height: 100%; overflow: auto;">
            <asp:GridView ID="DataGrid" runat="server" ShowFooter="True" ActiveowSorting="True"
                AutoGenerateColumns="False" ActiveowPaging="True" CssClass="Grid" AllowSorting="true"
                AllowPaging="True">
                <HeaderStyle CssClass="GridHeader"></HeaderStyle>
                <RowStyle CssClass="GridItem" Wrap="False" />
                <AlternatingRowStyle CssClass="GridAltItem" />
                <FooterStyle CssClass="GridFooter" />
                <PagerStyle CssClass="GridPager" />
                <Columns>
                    <asp:TemplateField HeaderText="User Code" SortExpression="UserID">
                        <ItemTemplate>
                            <asp:Label runat="server" ID="UserID" Text='<%# DataBinder.Eval(Container.DataItem, "UserID") %>'>
                            </asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label runat="server" ID="UserIDEdit" Text='<%# DataBinder.Eval(Container.DataItem, "UserID") %>'>
                            </asp:Label>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="UserIDAdd" CssClass="TextBox" placeholder="can't blank" MaxLength="30" runat="Server" Width="100%" />
                            <%--<cc1:TextBoxWatermarkExtender ID="UserIDAdd_WtExt" runat="server" Enabled="True"
                                TargetControlID="UserIDAdd" WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                            </cc1:TextBoxWatermarkExtender>--%>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="User Name" HeaderStyle-Width="350" SortExpression="UserName">
                        <ItemTemplate>
                            <asp:Label runat="server" ID="UserName" Text='<%# DataBinder.Eval(Container.DataItem, "UserName") %>'>
                            </asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox runat="server" CssClass="TextBox" ID="UserNameEdit" placeholder="can't blank" MaxLength="60" Width="100%"
                                Text='<%# DataBinder.Eval(Container.DataItem, "UserName") %>'>
                            </asp:TextBox>
                            <%--<cc1:TextBoxWatermarkExtender ID="UserNameEdit_WtExt" runat="server" Enabled="True"
                                TargetControlID="UserNameEdit" WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                            </cc1:TextBoxWatermarkExtender>--%>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="UserNameAdd" CssClass="TextBox" MaxLength="60" placeholder="can't blank" runat="Server" Width="100%" />
                           <%-- <cc1:TextBoxWatermarkExtender ID="UserNameAdd_WtExt" runat="server" Enabled="True"
                                TargetControlID="UserNameAdd" WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                            </cc1:TextBoxWatermarkExtender>--%>
                        </FooterTemplate>
                        <HeaderStyle Width="350px"></HeaderStyle>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Password" HeaderStyle-Width="350" SortExpression="UserPassword1">
                        <ItemTemplate>
                            <asp:Label runat="server" ID="UserPassword1" Width="100%" Text="********">
                            </asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox runat="server" CssClass="TextBox" MaxLength="100" TextMode="Password"
                                ReadOnly="true" ID="UserPassword1Edit" Width="100%" Text='<%# DataBinder.Eval(Container.DataItem, "UserPassword1") %>'>
                            </asp:TextBox>
                            <%--<cc1:TextBoxWatermarkExtender ID="UserPassword1Edit_WtExt" runat="server" Enabled="True"
                                TargetControlID="UserPassword1Edit" WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                            </cc1:TextBoxWatermarkExtender>--%>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="UserPassword1Add" TextMode="Password" placeholder="can't blank" CssClass="TextBox" MaxLength="100"
                                runat="Server" Width="100%" />
                                
                            <%--<cc1:TextBoxWatermarkExtender ID="UserPassword1Add_WtExt" runat="server" Enabled="True"
                                TargetControlID="UserPassword1Add" WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                            </cc1:TextBoxWatermarkExtender>--%>
                        </FooterTemplate>
                        <HeaderStyle Width="350px"></HeaderStyle>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="User Group" HeaderStyle-Width="160" SortExpression="UserGroup">
                        <ItemTemplate>
                            <asp:Label runat="server" ID="UserGroup" Text='<%# DataBinder.Eval(Container.DataItem, "UserGroupName") %>'>
                            </asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:DropDownList runat="server" ID="UserGroupEdit" Width="100%" CssClass="DropDownList"
                                SelectedValue='<%# DataBinder.Eval(Container.DataItem, "UserGroup") %>' DataSourceID="dsUserGroup"
                                DataTextField="UserGrp_Name" DataValueField="UserGrp_Code">
                            </asp:DropDownList>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:DropDownList ID="UserGroupAdd" runat="Server" Width="100%" CssClass="DropDownList"
                                DataSourceID="dsUserGroup" DataTextField="UserGrp_Name" DataValueField="UserGrp_Code">
                            </asp:DropDownList>
                        </FooterTemplate>
                        <HeaderStyle Width="160px"></HeaderStyle>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Change Period" HeaderStyle-Width="60px" SortExpression="FgChangePeriod">
                        <ItemTemplate>
                            <asp:Label runat="server" ID="FgChangePeriod" Text='<%# DataBinder.Eval(Container.DataItem, "FgChangePeriod") %>'>
                            </asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:DropDownList CssClass="DropDownList" runat="server" Width="100%" ID="FgChangePeriodEdit"
                                Text='<%# DataBinder.Eval(Container.DataItem, "FgChangePeriod") %>'>
                                <asp:ListItem>Y</asp:ListItem>
                                <asp:ListItem>N</asp:ListItem>
                            </asp:DropDownList>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:DropDownList CssClass="DropDownList" ID="FgChangePeriodAdd" runat="Server" Width="100%">
                                <asp:ListItem>Y</asp:ListItem>
                                <asp:ListItem>N</asp:ListItem>
                            </asp:DropDownList>
                        </FooterTemplate>
                        <HeaderStyle Width="60px"></HeaderStyle>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Active" HeaderStyle-Width="80px" SortExpression="FgActive">
                        <ItemTemplate>
                            <asp:Label runat="server" ID="FgActive" Text='<%# DataBinder.Eval(Container.DataItem, "FgActive") %>'>
                            </asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:DropDownList CssClass="DropDownList" runat="server" Width="100%" ID="FgActiveEdit"
                                Text='<%# DataBinder.Eval(Container.DataItem, "FgActive") %>'>
                                <asp:ListItem>Y</asp:ListItem>
                                <asp:ListItem>N</asp:ListItem>
                            </asp:DropDownList>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:DropDownList CssClass="DropDownList" ID="FgActiveAdd" runat="Server" Width="100%">
                                <asp:ListItem>Y</asp:ListItem>
                                <asp:ListItem>N</asp:ListItem>
                            </asp:DropDownList>
                        </FooterTemplate>
                        <HeaderStyle Width="80px"></HeaderStyle>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Action" HeaderStyle-Width="200">
                        <ItemTemplate>
                            <asp:Button ID="btnEdit" runat="server" class="bitbtndt btnedit" Text="Edit" CommandName="Edit" />
                            <asp:Button ID="btndelete" runat="server" class="bitbtndt btndelete" Text="Delete"
                                OnClientClick="return confirm('Sure to delete this data?');" CommandName="Delete" />
                            <asp:Button ID="btnReset" runat="server" class="bitbtndt btndelete" Width="80px"
                                Text="Reset Pwd" OnClientClick="return confirm('Sure to reset password ?');"
                                CommandName="Reset" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" />
                            <asp:Label runat="server" ID="FgAdmin" Text='<%# DataBinder.Eval(Container.DataItem, "FgAdmin") %>' Visible="false">
                            </asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Button ID="btnUpdate" runat="server" class="bitbtndt btnsave" Text="Save" CommandName="Update" />
                            <asp:Button ID="btnCancel" runat="server" class="bitbtndt btncancel" Text="Cancel"
                                CommandName="Cancel" />
                            <asp:Label runat="server" ID="FgAdminEdit" Text='<%# DataBinder.Eval(Container.DataItem, "FgAdmin") %>' Visible="false">
                            </asp:Label>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:Button class="bitbtndt btnadd" runat="server" ID="btnAdd" Text="Add" CommandName="Insert" />
                        </FooterTemplate>
                        <HeaderStyle Width="200px"></HeaderStyle>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
        <asp:SqlDataSource ID="dsUserGroup" runat="server" SelectCommand="EXEC S_GetUserGroup">
        </asp:SqlDataSource>
        <asp:Label ID="lstatus" ForeColor="red" runat="server"></asp:Label>
    </div>
    </form>
</body>
</html>
