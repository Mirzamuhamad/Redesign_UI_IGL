<%@ Page Language="VB" AutoEventWireup="false" CodeFile="MsCashFlowType.aspx.vb"
    Inherits="Execute_Master_MsCashFlowType_MsCashFlowType" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Cash Flow Type File</title>

    <script src="../../Function/OpenDlg.js" type="text/javascript"></script>

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
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div class="Content">
        <div class="H1">
            Cash Flow Type File</div>
        <hr style="color: Blue" />
        <table>
            <tr>
                <td style="width: 100px; text-align: right">
                    Quick Search :
                </td>
                <td>
                    <asp:TextBox runat="server" ID="tbFilter" CssClass="TextBox" />
                    <asp:DropDownList runat="server" ID="ddlField" CssClass="DropDownList">
                        <asp:ListItem Selected="true" Text="Cash Flow Type Code" Value="CashFlowType_Code"></asp:ListItem>
                        <asp:ListItem Text="Cash Flow Type Name" Value="CashFlowType_Name"></asp:ListItem>
                        <asp:ListItem Value="Flow">Flow</asp:ListItem>
                        <asp:ListItem Text="Cash Flow Category" Value="CashFlowCategoryName"></asp:ListItem>
                        <asp:ListItem Text="Detail Cost Center" Value="FgDetailCtr"></asp:ListItem>
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
                            <asp:ListItem Selected="true" Text="Cash Flow Type Code" Value="CashFlowType_Code"></asp:ListItem>
                            <asp:ListItem Text="Cash Flow Type Name" Value="CashFlowType_Name"></asp:ListItem>
                            <asp:ListItem Value="Flow">Flow</asp:ListItem>
                            <asp:ListItem Text="Cash Flow Category" Value="CashFlowCategoryName"></asp:ListItem>
                            <asp:ListItem Text="Detail Cost Center" Value="FgDetailCtr"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <br />
        <asp:GridView ID="DataGrid" runat="server" ShowFooter="True" AllowSorting="True"
            AutoGenerateColumns="False" AllowPaging="True" CssClass="Grid">
            <HeaderStyle CssClass="GridHeader"></HeaderStyle>
            <RowStyle CssClass="GridItem" Wrap="false" />
            <AlternatingRowStyle CssClass="GridAltItem" />
            <FooterStyle CssClass="GridFooter" />
            <PagerStyle CssClass="GridPager" />
            <Columns>
                <asp:TemplateField HeaderText="Cash Flow Type Code" SortExpression="CashFlowType_Code">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="CashFlowTypeCode" Text='<%# DataBinder.Eval(Container.DataItem, "CashFlowType_Code") %>'>
                        </asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:Label runat="server" ID="CashFlowTypeCodeEdit" Text='<%# DataBinder.Eval(Container.DataItem, "CashFlowType_Code") %>'>
                        </asp:Label>
                    </EditItemTemplate>
                    <FooterTemplate>
                        <asp:TextBox ID="CashFlowTypeCodeAdd" CssClass="TextBox" MaxLength="10" runat="Server"
                            Width="100%" />
                        <cc1:TextBoxWatermarkExtender ID="CashFlowTypeCodeAdd_WtExt" runat="server" Enabled="True"
                            TargetControlID="CashFlowTypeCodeAdd" WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                        </cc1:TextBoxWatermarkExtender>
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Cash Flow Type Name" HeaderStyle-Width="350" SortExpression="CashFlowType_Name">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="CashFlowTypeName" Text='<%# DataBinder.Eval(Container.DataItem, "CashFlowType_Name") %>'>
                        </asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox runat="server" CssClass="TextBox" ID="CashFlowTypeNameEdit" MaxLength="60"
                            Width="100%" Text='<%# DataBinder.Eval(Container.DataItem, "CashFlowType_Name") %>'>
                        </asp:TextBox>
                        <cc1:TextBoxWatermarkExtender ID="CashFlowTypeNameEdit_WtExt" runat="server" Enabled="True"
                            TargetControlID="CashFlowTypeNameEdit" WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                        </cc1:TextBoxWatermarkExtender>
                    </EditItemTemplate>
                    <FooterTemplate>
                        <asp:TextBox ID="CashFlowTypeNameAdd" CssClass="TextBox" MaxLength="60" runat="Server"
                            Width="100%" />
                        <cc1:TextBoxWatermarkExtender ID="CashFlowTypeNameAdd_WtExt" runat="server" Enabled="True"
                            TargetControlID="CashFlowTypeNameAdd" WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                        </cc1:TextBoxWatermarkExtender>
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Flow" SortExpression="Flow">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="Flow" Text='<%# DataBinder.Eval(Container.DataItem, "Flow") %>'>
                        </asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:DropDownList CssClass="DropDownList" runat="server" Width="100%" ID="FlowEdit"
                            Text='<%# DataBinder.Eval(Container.DataItem, "Flow") %>'>
                            <asp:ListItem>IN</asp:ListItem>
                            <asp:ListItem>OUT</asp:ListItem>
                        </asp:DropDownList>
                    </EditItemTemplate>
                    <FooterTemplate>
                        <asp:DropDownList CssClass="DropDownList" ID="FlowAdd" runat="Server" Width="100%">
                            <asp:ListItem Selected="True">IN</asp:ListItem>
                            <asp:ListItem>OUT</asp:ListItem>
                        </asp:DropDownList>
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Cash Flow Category" HeaderStyle-Width="150px" SortExpression="CashFlowCategoryName">
					<Itemtemplate>
						<asp:Label Runat="server" ID="CashFlowCategory" Width="100%" text='<%# DataBinder.Eval(Container.DataItem, "CashFlowCategoryName") %>'>
						</asp:Label>
					</Itemtemplate>
					<EditItemTemplate>
						<asp:DropDownList Runat="server" ID="CashFlowCategoryEdit" Width="100%" CssClass="DropDownList"
                         SelectedValue='<%# DataBinder.Eval(Container.DataItem, "CashFlowCategory") %>'
                         DataSourceID="dsCashFlowCategory" DataTextField="CashFlowCategoryName" 
                         DataValueField="CashFlowCategoryCode">
						</asp:DropDownList>													
					</EditItemTemplate>
					<FooterTemplate>
						<asp:DropDownList CssClass="DropDownList" ID="CashFlowCategoryAdd" Runat="Server" Width="100%" 
                         DataSourceID="dsCashFlowCategory" DataTextField="CashFlowCategoryName" 
                         DataValueField="CashFlowCategoryCode">
						</asp:DropDownList>									
					</FooterTemplate>
				    <HeaderStyle Width="330px"></HeaderStyle>
				</asp:TemplateField>
                <asp:TemplateField HeaderText="Detail Cost Center" SortExpression="FgDetailCtr">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="FgDetailCtr" Text='<%# DataBinder.Eval(Container.DataItem, "FgDetailCtr") %>'>
                        </asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:DropDownList CssClass="DropDownList" runat="server" Width="100%" ID="FgDetailCtrEdit"
                            Text='<%# DataBinder.Eval(Container.DataItem, "FgDetailCtr") %>'>
                            <asp:ListItem>Y</asp:ListItem>
                            <asp:ListItem>N</asp:ListItem>
                        </asp:DropDownList>
                    </EditItemTemplate>
                    <FooterTemplate>
                        <asp:DropDownList CssClass="DropDownList" ID="FgDetailCtrAdd" runat="Server" Width="100%">
                            <asp:ListItem>Y</asp:ListItem>
                            <asp:ListItem Selected="True">N</asp:ListItem>
                        </asp:DropDownList>
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Action" HeaderStyle-Width="126">
                    <ItemTemplate>
                        <asp:Button ID="btnEdit" runat="server" class="bitbtndt btnedit" Text="Edit" CommandName="Edit" />
                        <asp:Button ID="btndelete" runat="server" class="bitbtndt btndelete" Text="Delete"
                            OnClientClick="return confirm('Sure to delete this data?');" CommandName="Delete" />
                        <asp:Button ID="btnAssign" runat="server" Width="70px" class="bitbtndt btndetail"
                            Text="Account" CommandName="Assign" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" />
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
        <asp:SqlDataSource ID="dsCashFlowCategory" runat="server" SelectCommand="EXEC S_GetCashFlowCategory">                                        
        </asp:SqlDataSource>
        <asp:Label ID="lstatus" ForeColor="red" runat="server"></asp:Label>
    </div>
    </form>
</body>
</html>
