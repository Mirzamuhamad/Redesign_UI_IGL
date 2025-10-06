<%@ Page Language="VB" AutoEventWireup="false" CodeFile="MsCostingPrice.aspx.vb" Inherits="Transaction_MsCostingPrice_MsCostingPrice" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<%@ Register Assembly="BasicFrame.WebControls.BasicDatePicker" Namespace="BasicFrame.WebControls" TagPrefix="BDP" %>
    
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Costing Price List</title>
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
 <link href="https://fonts.googleapis.com/css2?family=Muli:wght@300;400;600&display=swap" rel="stylesheet">   
    <script src="../../Function/Function.JS" type="text/javascript"></script>
    <script src="../../Function/OpenDlg.JS" type="text/javascript"></script>
    
    <script type="text/javascript" src="../../JS/jquerymodal/jquery.min.js"></script>
    <script src="../../JS/jquerymodal/jquery-ui.js" type="text/javascript"></script>
    <link href="../../JS/jquerymodal/jquery-ui.css" rel="stylesheet" type="text/css" />
    
    <%--<script type="text/javascript">
        function OpenPopup() {         
        window.open("../../SearchDlg.Aspx","List","scrollbars=yes,resizable=no,width=500,height=400");        
        return false;
        }    
    </script>--%>
    <style type="text/css">
        .style1
        {
            width: 116px;
            text-align: right;
            height: 24px;
        }
        .style2
        {
            width: 92px;
            text-align: right;
            height: 24px;
        }
        .style3
        {
            height: 24px;
        }
        .style4
        {
            width: 170px;
            text-align: right;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div class="Content">
        <div class="H1">&nbsp;<asp:Label ID="Labelmenu" runat="server" Text="Costing Price List"></asp:Label>
            </div>
        <hr style="color: Blue" />
        
        <asp:Panel ID="PnlMain" runat="server">
        
            <table>
                <tr>
                    <td style="width: 100px; text-align: right">
                        Quick Search :
                    </td>
                    <td>
                        <asp:TextBox CssClass="TextBox" runat="server" ID="tbFilter" />
                        <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlField">
                            <asp:ListItem Value="Costing" Selected="True">Costing</asp:ListItem>
                            <asp:ListItem Value="CostingName">Costing Name</asp:ListItem>
                            <asp:ListItem Value="Sheet">Sheet</asp:ListItem>
                            <asp:ListItem Value="SheetName">Sheet Name</asp:ListItem>
                        </asp:DropDownList>
                        <%--<asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlRange">
                        </asp:DropDownList>--%>
                        <asp:Button class="bitbtn btnsearch" runat="server" ID="btnSearch" Text="Search" />
                        <asp:Button class="bitbtn btngo" runat="server" ID="btnExpand" Text="..." />
                    </td>
                    <td>
                        <%--<asp:LinkButton ID="LbAdvSearch" runat="server" Text="Advanced Search" />--%>
                        <asp:Button class="bitbtn btnprint" runat="server" ID="BtnPrint" Text="Print" Visible="false"/>
                    </td>
                    <%--<td class="style4">
                        Type :
                    </td>
                    <td>
                        <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlType" AutoPostBack="true">
                            <asp:ListItem>Service</asp:ListItem>
                            <asp:ListItem>Trading</asp:ListItem>
                        </asp:DropDownList>
                    </td>--%>
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
                                <asp:ListItem Value="Costing" Selected="True">Costing</asp:ListItem>
                                <asp:ListItem Value="CostingName">Costing Name</asp:ListItem>
                                <asp:ListItem Value="Sheet">Sheet</asp:ListItem>
                                <asp:ListItem Value="SheetName">Sheet Name</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <br />
            <table>
                <tr>
                    <td class="style2">
                        Effective Date
                    </td>
                    <td class="style3">
                        :
                    </td>
                    <td class="style3">
                        <BDP:BasicDatePicker ID="tbDate" runat="server" DateFormat="dd MMM yyyy" ButtonImageHeight="19px"
                            ButtonImageWidth="20px" DisplayType="TextBoxAndImage" ReadOnly="true" TextBoxStyle-CssClass="TextDate"
                            AutoPostBack="True"><TextBoxStyle CssClass="TextDate" /></BDP:BasicDatePicker>
                    </td>
                    <td class="style3">
                        <asp:Button class="bitbtn btnapply" runat="server" ID="BtnApply" Text="Apply" Visible="false"/>
                        <%--&nbsp--%>
                        <%--<asp:Button class="bitbtn btnprint" runat="server" ID="BtnPrint" Text="Print" />--%>
                        <%--<asp:ImageButton ID="BtnApply" runat="server"  
                    ImageUrl="../../Image/btnapplyon.png"
                    onmouseover="this.src='../../Image/btnapplyoff.png';"
                    onmouseout="this.src='../../Image/btnapplyon.png';"
                    ImageAlign="AbsBottom" />                --%>
                    </td>
                    <td class="style1">
                        Additional Price
                    </td>
                    <td class="style3">
                        :
                    </td>
                    <td class="style3">
                        <asp:DropDownList runat="server" CssClass="DropDownList" ID="ddlSatuan">
                                <asp:ListItem Selected="True">%</asp:ListItem>
                                <asp:ListItem>Nominal</asp:ListItem>
                            </asp:DropDownList>
                    </td>
                    <td class="style3">
                        <asp:TextBox CssClass="TextBox" runat="server" ID="TbPrise" />
                    </td>
                    <td class="style3">
                        <asp:Button class="btngo" runat="server" ID="BtnGo" Text="G"/>
                    </td>
                </tr>
                <tr>
                    <td>
                        <br />
                        <asp:Button class="bitbtn btnadd" runat="server" ID="BtnNew" Text="New Item" />
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <br />
        <asp:Panel runat="server" ID="pnlService" Visible="false">
            <asp:GridView ID="DataGrid" runat="server" AutoGenerateColumns="False" 
                CssClass="Grid" AllowPaging="True">
                <HeaderStyle CssClass="GridHeader"></HeaderStyle>
                <RowStyle CssClass="GridItem" />
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
                  
                    <asp:TemplateField HeaderText="Costing">
                        <ItemTemplate>
                            <asp:Label runat="server" ID="Costing" Text='<%# DataBinder.Eval(Container.DataItem, "Costing") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Costing Name">
                        <ItemTemplate>
                            <asp:Label runat="server" ID="CostingName" Text='<%# DataBinder.Eval(Container.DataItem, "CostingName") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Sheet">
                        <ItemTemplate>
                            <asp:Label runat="server" ID="Sheet" Text='<%# DataBinder.Eval(Container.DataItem, "Sheet") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Sheet Name">
                        <ItemTemplate>
                            <asp:Label runat="server" ID="SheetName" Text='<%# DataBinder.Eval(Container.DataItem, "SheetName") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--<asp:TemplateField HeaderText="Area Service Name">
                        <ItemTemplate>
                            <asp:Label runat="server" ID="AreaServiceName" Text='<%# DataBinder.Eval(Container.DataItem, "AreaServiceName") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>--%>
                    
                    <%--<asp:TemplateField HeaderText="Unit" Visible="false">
                        <ItemTemplate>
                            <asp:Label runat="server" ID="Unit" Text='<%# DataBinder.Eval(Container.DataItem, "Unit") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>--%>
                    <%--<asp:TemplateField HeaderText="Unit">
                        <ItemTemplate>
                            <asp:Label runat="server" ID="Unit1" Text='<%# DataBinder.Eval(Container.DataItem, "Unit") %>' Visible="false"/>
                            <asp:DropDownList CssClass="DropDownList" Width="100%" ID="ddlUnitEdit" runat="server"
                                SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Unit") %>' 
                                DataSourceID="dsUnit" DataTextField="Unit_Name" DataValueField="Unit_Code">
                            </asp:DropDownList>
                        </ItemTemplate>
                    </asp:TemplateField>--%>
                    <%--<asp:TemplateField HeaderText="Currency">
                        <ItemTemplate>
                            <asp:Label runat="server" ID="Currency" Text='<%# DataBinder.Eval(Container.DataItem, "Currency") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>--%>
                    
                    <asp:TemplateField HeaderText="Price" ItemStyle-HorizontalAlign="Right" SortExpression="Price">
                        <ItemTemplate>
                            <asp:TextBox runat="server" CssClass="TextBox" ID="Price" MaxLength="20"
                                Text='<%# DataBinder.Eval(Container.DataItem, "Price") %>' />
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Right" />
                    </asp:TemplateField>
                    
                    <asp:TemplateField HeaderText="Last Date" Visible="false">
                        <ItemTemplate>
                            <asp:Label runat="server" ID="tbLastDate" Text='<%# DataBinder.Eval(Container.DataItem, "LastDate") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="LastDate" DataFormatString="{0:dd MMM yyyy}"
                        HeaderStyle-Width="80px" SortExpression="LastDate" HeaderText="Last Date">
                        <HeaderStyle Width="80px" />
                    </asp:BoundField>
                    
                    <asp:TemplateField HeaderText="Last Price" ItemStyle-HorizontalAlign = "Right">
                        <ItemTemplate>
                            <asp:Label runat="server" ID="LastPrice" Text='<%# DataBinder.Eval(Container.DataItem, "LastPrice") %>' />
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Right" />
                    </asp:TemplateField>
                    
                    <asp:TemplateField HeaderText="Action" >
						<ItemTemplate>		
						    <asp:Button ID="btnApply" runat="server" class="bitbtndt btnedit" Text="Apply" CommandName="Edit"/>										             
						</ItemTemplate>
					</asp:TemplateField>
                </Columns>
            </asp:GridView>
        </asp:Panel>
        <%--<asp:Panel runat="server" ID="pnlTrading" Visible="false">
            
        </asp:Panel>--%>
        <br />
        <asp:Panel runat="server" ID="pnlInput" Visible="false">
        <table>
            <tr>
                <td>
                    Effective Date
                </td>
                <td>
                    :
                </td>
                <td>
                    <%--<asp:TextBox runat="server" CssClass="TextBox" ID="tbEffectiveDate" ValidationGroup="Input"
                        Enabled="false" />--%>
                    <BDP:BasicDatePicker ID="tbEffectiveDate" runat="server" DateFormat="dd MMM yyyy"
                        ButtonImageHeight="19px" ButtonImageWidth="20px" DisplayType="TextBoxAndImage"
                        ReadOnly="true" TextBoxStyle-CssClass="TextDate">
                        <TextBoxStyle CssClass="TextDate" />
                    </BDP:BasicDatePicker>
                    &nbsp;<asp:TextBox ID="tbType" runat="server" Visible="false" />
                </td>
            </tr>
            <tr>
                <td>Costing</td>
                <td>:</td>
                <td>
                    <asp:TextBox ID="TbCosting" CssClass="TextBox" MaxLength="12" runat="server" AutoPostBack="True" />
                    <asp:TextBox ID="TbCostingName" CssClass="TextBox" MaxLength="60" Width="200" Enabled="False" runat="server" />
                    <asp:Button ID="btnCosting" runat="server" class="btngo" Text="..."/>
                </td>
            </tr>
            <tr>
                <td>Sheet</td>
                <td>:</td>
                <td>
                    <asp:TextBox ID="TbSheet" CssClass="TextBox" MaxLength="12" runat="server" AutoPostBack="True" />
                    <asp:TextBox ID="TbSheetName" CssClass="TextBox" MaxLength="60" Width="200" Enabled="False" runat="server" />
                    <asp:Button ID="btnSheet" runat="server" class="btngo" Text="..."/>
                </td>
            </tr>
            <%----------------------------------------------------------------------------------------%>
            <tr>
                <td>Last Price</td>
                <td>:</td>
                <td>
                    <asp:TextBox ID="TbLastPrice" CssClass="TextBox" runat="server" Enabled="false"/>
                </td> 
            </tr>
            <tr>
                <td>Last Effective</td>
                <td>:</td>
                <td>
                    <asp:TextBox ID="TbLastEffective" CssClass="TextBox" runat="server" Enabled="false"/>
                </td> 
            </tr>
            <%----------------------------------------------------------------------------------------%>
            <tr>
                <td>Price</td>
                <td>:</td>
                <td>
                    <asp:TextBox runat="server" ID="tbPrice" CssClass="TextBox" ValidationGroup="Input"/>
                    <%--<asp:TextBox ID="tbPrice" runat="server" CssClass="TextBox" MaxLength="30" 
                        ValidationGroup="Input" Width="129px" />--%>
                </td>
            </tr>
            
            <tr>
                <td colspan="3" align="center">
                <asp:Button ID="BtnSave" runat="server" class="bitbtndt btnsave" Text="Save" CommandName="Update" ValidationGroup="Input"/>&nbsp;									
                <asp:Button ID="btnReset" runat="server" class="bitbtndt btncancel" Text="Reset" CommandName="Cancel" ValidationGroup="Input"/>&nbsp;                     
                <asp:Button ID="btnCancel" runat="server" class="bitbtndt btncancel" Text="Back" CommandName="Cancel"/>&nbsp;									                                    
                </td>
            </tr>
        </table>
      </asp:Panel>
    </div>
    <br />
    <asp:Label ID="lbstatus" ForeColor="red" runat="server"></asp:Label>
    <asp:SqlDataSource ID="dsUnit" runat="server" SelectCommand="EXEC S_GetUnit">
    </asp:SqlDataSource>
    </form>
</body>
</html>
