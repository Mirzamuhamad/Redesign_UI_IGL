<%@ Page Language="VB" AutoEventWireup="false" CodeFile="RptExpSummaryAdv.aspx.vb" Inherits="Rpt_RptExpSummaryAdv_RptExpSummaryAdv" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Untitled Page</title>
    <script src="../../Function/OpenDlg.js" type="text/javascript"></script>
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div class="Content">
    <div class="H1">Expense Summary Analysis</div>
    <hr style="color:Blue" />                
        <table>
            <tr>
                <td>Current Period</td>
                <td>:</td>
                <td>
                    <asp:DropDownList ID="ddlYear" runat="server" CssClass="DropDownList" />
                    <asp:DropDownList ID="ddlPeriod" runat="server" CssClass="DropDownList" />
                </td>   
                <td></td>
                <td></td>
                <td></td>
            </tr>
            <tr>
                <td>Compare Period</td>
                <td>:</td>
                <td>
                    <asp:DropDownList ID="ddlYear2" runat="server" CssClass="DropDownList" />
                    <asp:DropDownList ID="ddlPeriod2" runat="server" CssClass="DropDownList" />
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
                       <asp:ListItem Value="0" Selected="True">Summary</asp:ListItem>
                       <asp:ListItem Value="1">Detail</asp:ListItem>                                              
                    </asp:RadioButtonList> 
                </td>
            </tr>      
            <tr>
                <td class="style6">Expense Type</td>
                <td class="style13">:</td>
                <td colspan="4" class="style2">
   
                    <asp:DropDownList ID="ddlExpense" runat="server" CssClass="DropDownList" />
   
                </td>                   
            </tr>      
        </table>  
        <br /> 
        <asp:Button class="bitbtn btnpreview" runat="server" ID="btnPreview" Visible="false" Text="Preview" />   
        <asp:Button class="bitbtn btnprint" runat="server" ID="btnPrint" Text="Print" /> 
        <asp:Button class="bitbtn btnexcel" runat="server" ID="btnExport" Text="Export" />         
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