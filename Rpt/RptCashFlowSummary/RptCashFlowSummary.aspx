<%@ Page Language="VB" AutoEventWireup="false" CodeFile="RptCashFlowSummary.aspx.vb" Inherits="RptCashFlowForcash" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register TagPrefix="GridRpt" TagName="RptGridCtrl" Src="~/UserControl/ReportSingleGrid.ascx" %>
<%@ Register Assembly="BasicFrame.WebControls.BasicDatePicker" Namespace="BasicFrame.WebControls"
    TagPrefix="BDP" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Untitled Page</title>
    <%--<script src="../../Function/OpenDlg.js" type="text/javascript"></script>--%>
    <script src="../../Function/Function.JS" type="text/javascript"></script>
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
 <link href="https://fonts.googleapis.com/css2?family=Muli:wght@300;400;600&display=swap" rel="stylesheet">
    
    <script type="text/javascript">         
        function openreport()
            {
                wOpen = window.open("../../Rpt/PrintForm.Aspx", "List","scrollbars=yes,resizable=yes,width=500,height=400");                            
                wOpen.moveTo(0, 0);
                wOpen.resizeTo(screen.width, screen.height);                        
                return false; 
             }             
        function openreport2()
            {
                var wOpen;                
                wOpen = window.open("../../Rpt/PrintForm2.Aspx", "List","scrollbars=yes,resizable=yes,width=500,height=400");            
                wOpen.moveTo(0, 0);
                wOpen.resizeTo(screen.width, screen.height);                        
                return false; 
            }                     
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="Content">
    <div class="H1">Cash Flow Summary</div>
    <hr style="color:Blue" />                
        <table>
            <tr>
                <td>Period</td>
                <td>:</td>
                <td>
                    <asp:DropDownList ID="ddlYear" runat="server" CssClass="DropDownList" />
                    <asp:DropDownList ID="ddlPeriod" runat="server" CssClass="DropDownList" />
                        &nbsp;&nbsp; &nbsp;&nbsp;
                </td>
                <td>Report in </td>
                <td>:</td>
                <td>     
                    <asp:DropDownList ID="ddlCurrency" runat="server" CssClass="DropDownList" Width="120px">  
                      <asp:ListItem Selected="true" Text="Forecast" Value="Forecast"></asp:ListItem>
                      <asp:ListItem Text="Actual" Value="Actual"></asp:ListItem>                       
                    </asp:DropDownList>
                    &nbsp;&nbsp; &nbsp;&nbsp;
                </td>       
                <%--<td>Rate</td>
                <td>:</td>--%>               
                              
            </tr>
            <%--<tr>
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
            </tr>--%>
            <tr>
                <td>&nbsp</td>
                <td>&nbsp</td>
                <td colspan="4" style="font-weight: 700">
                    <asp:CheckBox ID="cbValue" runat="server" Text="Print Value" Visible="false"/>
                &nbsp;<asp:CheckBox ID="cbValue2" runat="server" Text="With Analisa" Visible="false"/>
                </td>
            </tr>
        </table>          
        <asp:Button class="bitbtn btnpreview" runat="server" ID="btnPreview" Visible="false" Text="Preview"/>
        <asp:Button class="bitbtn btnprint" runat="server" ID="btnPrint" Text="Print"/>
        <asp:Button class="bitbtn btnexcel" runat="server" ID="btnExport" Text="Export"/>        
        <br />
        <asp:Label ID="lbStatus" runat="server" ForeColor="Red" />
        <GridRpt:RptGridCtrl ID="ReportGrid" runat = "server" />
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