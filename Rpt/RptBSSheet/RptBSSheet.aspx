<%@ Page Language="VB" AutoEventWireup="false" CodeFile="RptBSSheet.aspx.vb" Inherits="Rpt_RptBSSheet_RptBSSheet" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<%@ Register Assembly="BasicFrame.WebControls.BasicDatePicker" Namespace="BasicFrame.WebControls"
    TagPrefix="BDP" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Untitled Page</title>
    <script src="../../Function/OpenDlg.js" type="text/javascript"></script>
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div class="Content">
    <div class="H1">Balance Sheet Report</div>
    <hr style="color:Blue" />                
        <table>
            <tr>
                <td>Current Period</td>
                <td>:</td>
                <td>
                    <asp:DropDownList ID="ddlYear" runat="server" CssClass="DropDownList" />
                    <asp:DropDownList ID="ddlPeriod" runat="server" CssClass="DropDownList" />
                &nbsp;&nbsp; -&nbsp;&nbsp;
                    <asp:DropDownList ID="ddlBudgetCurrent" runat="server" CssClass="DropDownList" >
                        <asp:ListItem Selected="True" Value="ACTUAL">ACTUAL</asp:ListItem>
                        <asp:ListItem Value="BUDGET">BUDGET</asp:ListItem>
                    </asp:DropDownList>
                </td>   
                <td></td>
                <td>&nbsp;</td>
                <td>
                &nbsp;</td>
            </tr>
            <tr>
                <td>Compare Period</td>
                <td>:</td>
                <td>
                    <asp:DropDownList ID="ddlYearEnd" runat="server" CssClass="DropDownList" />
                    <asp:DropDownList ID="ddlPeriodEnd" runat="server" CssClass="DropDownList" />
                &nbsp;&nbsp; -&nbsp;&nbsp;
                    <asp:DropDownList ID="ddlBudgetCompare" runat="server" CssClass="DropDownList" >
                        <asp:ListItem Selected="True" Value="ACTUAL">ACTUAL</asp:ListItem>
                        <asp:ListItem Value="BUDGET">BUDGET</asp:ListItem>
                    </asp:DropDownList>
                </td>   
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td>Report Type</td>
                <td>:</td>
                <td colspan="4">
                   <asp:RadioButtonList ID="rbType" runat="server" RepeatColumns="3" 
                        Width="100%">
                       <asp:ListItem Value="0" Selected="True">Summary By Sub Group</asp:ListItem>
                       <asp:ListItem Value="1">Summary By Class</asp:ListItem>
                       <asp:ListItem Value="2">Detail By Account</asp:ListItem>
                    </asp:RadioButtonList> 
                </td>
            </tr>
            <tr>
                <td>&nbsp</td>
                <td>&nbsp</td>
                <td colspan="4" style="font-weight: 700">
                    <asp:CheckBox ID="cbValue" runat="server" Text="Print Value" />
                &nbsp;<asp:CheckBox ID="cbValue2" runat="server" Text="With Analisa" />
                </td>
            </tr>
        </table>          
        <asp:Button class="bitbtn btnpreview" runat="server" ID="btnPreview" Visible="false" Text="Preview"/>
        <asp:Button class="bitbtn btnprint" runat="server" ID="btnPrint" Text="Print"/>
        <asp:Button class="bitbtn btnexcel" runat="server" ID="btnExport" Text="Export"/>        
        <br />
        <asp:Label ID="lbStatus" runat="server" ForeColor="Red" />
        <asp:GridView ID="GridExport" runat="server" AutoGenerateColumns="True" 
                Visible="true" ShowFooter="False" GridLines="None" Height="79px" PageSize="20" Width="724px">
               <HeaderStyle CssClass="GridHeader" />
               <RowStyle CssClass="GridItem" Wrap="false" />
               <AlternatingRowStyle CssClass="GridAltItem" />
               <PagerStyle CssClass="GridPager" />
           </asp:GridView>
    </div>
    </form>
</body>
</html>