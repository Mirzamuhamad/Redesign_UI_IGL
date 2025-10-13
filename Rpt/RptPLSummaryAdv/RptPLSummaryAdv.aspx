<%@ Page Language="VB" AutoEventWireup="false" CodeFile="RptPLSummaryAdv.aspx.vb" Inherits="Rpt_RptPLSummaryAdv_RptPLSummaryAdv" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register TagPrefix="GridRpt" TagName="RptGridCtrl" Src="~/UserControl/ReportGrid.ascx" %>


<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Untitled Page</title>
    <script src="../../Function/OpenDlg.js" type="text/javascript"></script>
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
 <link href="https://fonts.googleapis.com/css2?family=Muli:wght@300;400;600&display=swap" rel="stylesheet">
</head>
<body>
    <form id="form1" runat="server">
    <div class="Content">
    <div class="H1">Profit Loss Analysis</div>
    <hr style="color:Blue" />                
        <table>
            <tr>
                <td>Current Period</td>
                <td>:</td>
                <td>
                    <asp:DropDownList ID="ddlYear" runat="server" CssClass="DropDownList" />
                    <asp:DropDownList ID="ddlPeriod" runat="server" CssClass="DropDownList" />
                &nbsp; -&nbsp;
                    <asp:DropDownList ID="ddlBudgetCurrent" runat="server" CssClass="DropDownList" >
                        <asp:ListItem Selected="True">ACTUAL</asp:ListItem>
                        <asp:ListItem>BUDGET</asp:ListItem>
                    </asp:DropDownList>
&nbsp;</td>                  
            </tr>
            <tr>
                <td>Compare Period</td>
                <td>:</td>
                <td>
                    <asp:DropDownList ID="ddlYear2" runat="server" CssClass="DropDownList" />
                    <asp:DropDownList ID="ddlPeriod2" runat="server" CssClass="DropDownList" />
                &nbsp; -&nbsp;
                    <asp:DropDownList ID="ddlBudgetCompare" runat="server" CssClass="DropDownList" >
                        <asp:ListItem Selected="True">ACTUAL</asp:ListItem>
                        <asp:ListItem>BUDGET</asp:ListItem>
                    </asp:DropDownList>
&nbsp;</td>                   
            </tr>            
            <tr>
                <td>Print</td>
                <td>:</td>
                <td>
                <asp:checkbox ID="cbYTD" runat="server" Text="Year To Date" />
                </td>
            </tr>            
        </table> 
        <fieldset style="width:480px" visible="False">
            <legend>Report Type :</legend>
            <asp:RadioButtonList ID="rbType" runat="server" RepeatColumns="4">
                       <asp:ListItem Value="0" Selected="True">Summary</asp:ListItem>
                       <asp:ListItem Value="1">Detail Group Account</asp:ListItem>                       
                       <asp:ListItem Value="2">Detail Class Account</asp:ListItem>                       
                       <asp:ListItem Value="3">Detail Account</asp:ListItem>                       
                    </asp:RadioButtonList>       
        </fieldset>  
        <br/>
        <asp:Button class="bitbtn btnpreview" runat="server" ID="btnPreview" Visible="false" Text="Preview" />   
        <asp:Button class="bitbtn btnprint" runat="server" ID="btnPrint" Text="Print" />   
        <asp:Button class="bitbtn btnexcel" runat="server" ID="btnExport" Text="Export" /> 
        <br />
        <GridRpt:RptGridCtrl ID="ReportGrid" runat = "server" />         
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