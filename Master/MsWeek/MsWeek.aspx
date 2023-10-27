<%@ Page Language="VB" AutoEventWireup="false" CodeFile="MsWeek.aspx.vb" Inherits="Transaction_MsWeek_MsWeek" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<%@ Register Assembly="BasicFrame.WebControls.BasicDatePicker" Namespace="BasicFrame.WebControls" TagPrefix="BDP" %>
    
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Week File</title>
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />   
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
        </style>
</head>
<body>
    <form id="form1" runat="server">
    <div class="Content">
        <div class="H1">&nbsp;<asp:Label ID="Labelmenu" runat="server" Text="Week File"></asp:Label></div>
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
                            <asp:ListItem Value="Year" Selected="True">Year</asp:ListItem>
                            <asp:ListItem Value="WeekNo">Week No</asp:ListItem>
                            <asp:ListItem Value="Startdate">Start Date</asp:ListItem>
                            <asp:ListItem Value="EndDate">End Date</asp:ListItem>
                        </asp:DropDownList>
                       
                        
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
                            <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlField2">
                            <asp:ListItem Value="Year" Selected="True">Year</asp:ListItem>
                            <asp:ListItem Value="WeekNo">Week No</asp:ListItem>
                            <asp:ListItem Value="Startdate">Start Date</asp:ListItem>
                            <asp:ListItem Value="EndDate">End Date</asp:ListItem>
                        </asp:DropDownList>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <br />
            <table>
                <tr>
                    <td class="style2">
                        Year</td>
                    <td class="style3">
                        :
                    </td>
                    <td class="style3">
                        <asp:TextBox ID="TbYear" runat="server" CssClass="TextBox" />
                    </td>
                    <td class="style3">
                        <asp:Button class="bitbtn btnapply" runat="server" ID="BtnApply" Text="Generate" Visible="true"/>
                        <%--&nbsp--%>
                        <%--<asp:Button class="bitbtn btnprint" runat="server" ID="BtnPrint" Text="Print" />--%>
                        <%--<asp:ImageButton ID="BtnApply" runat="server"  
                    ImageUrl="../../Image/btnapplyon.png"
                    onmouseover="this.src='../../Image/btnapplyoff.png';"
                    onmouseout="this.src='../../Image/btnapplyon.png';"
                    ImageAlign="AbsBottom" />                --%>
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
                    <%--<asp:TemplateField>
                        <HeaderTemplate>
                            <asp:CheckBox ID="cbSelectHd" runat="server" AutoPostBack="true" OnCheckedChanged="cbSelectHd_CheckedChanged" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="cbSelect" runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>--%>
                  
                    <asp:TemplateField HeaderText="Year">
                        <ItemTemplate>
                            <asp:Label runat="server" ID="Year" Text='<%# DataBinder.Eval(Container.DataItem, "Year") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="WeekNo">
                        <ItemTemplate>
                            <asp:Label runat="server" ID="WeekNo" Text='<%# DataBinder.Eval(Container.DataItem, "WeekNo") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    
                    
                   <asp:TemplateField HeaderText="Start Date" Visible="false">
                        <ItemTemplate>
                            <asp:Label runat="server" ID="tbStartDate" Text='<%# DataBinder.Eval(Container.DataItem, "StartDate") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="StartDate" DataFormatString="{0:dd MMM yyyy}"
                        HeaderStyle-Width="80px" SortExpression="StartDate" HeaderText="Start Date">
                        <HeaderStyle Width="80px" />
                    </asp:BoundField>
                    
                    <asp:TemplateField HeaderText="End Date" Visible="false">
                        <ItemTemplate>
                            <asp:Label runat="server" ID="tbEndDate" Text='<%# DataBinder.Eval(Container.DataItem, "EndDate") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="EndDate" DataFormatString="{0:dd MMM yyyy}"
                        HeaderStyle-Width="80px" SortExpression="EndDate" HeaderText="End Date">
                        <HeaderStyle Width="80px" />
                    </asp:BoundField>
                    
                     
                    <%--<asp:TemplateField HeaderText="Last Price" ItemStyle-HorizontalAlign = "Right">
                        <ItemTemplate>
                            <asp:Label runat="server" ID="LastPrice" Text='<%# DataBinder.Eval(Container.DataItem, "LastPrice") %>' />
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Right" />
                    </asp:TemplateField>--%>
                    
                    <%--<asp:TemplateField HeaderText="Action" >
						<ItemTemplate>		
						    <asp:Button ID="btnApply" runat="server" class="bitbtndt btnedit" Text="Apply" CommandName="Edit"/>										             
						</ItemTemplate>
					</asp:TemplateField>--%>
                </Columns>
            </asp:GridView>
        </asp:Panel>
        <%--<asp:TemplateField>
                        <HeaderTemplate>
                            <asp:CheckBox ID="cbSelectHd" runat="server" AutoPostBack="true" OnCheckedChanged="cbSelectHd_CheckedChanged" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="cbSelect" runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>--%>
        <br />
        
    </div>
    <br />
    <asp:Label ID="lbstatus" ForeColor="red" runat="server"></asp:Label>
    <%--<asp:TemplateField HeaderText="Last Price" ItemStyle-HorizontalAlign = "Right">
                        <ItemTemplate>
                            <asp:Label runat="server" ID="LastPrice" Text='<%# DataBinder.Eval(Container.DataItem, "LastPrice") %>' />
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Right" />
                    </asp:TemplateField>--%>
    </form>
</body>
</html>
