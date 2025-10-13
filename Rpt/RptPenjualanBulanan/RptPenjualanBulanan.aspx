<%@ Page Language="VB" AutoEventWireup="false" CodeFile="RptPenjualanBulanan.aspx.vb" Inherits="Rpt_RptPenjualanBulanan_RptPenjualanBulanan" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register TagPrefix="GridRpt" TagName="RptGridCtrl" Src="~/UserControl/ReportGrid.ascx" %>

<%@ Register Assembly="BasicFrame.WebControls.BasicDatePicker" Namespace="BasicFrame.WebControls"
    TagPrefix="BDP" %>

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
    <div class="H1">Monthly Sales Report (CI) </div>
    <hr style="color:Blue" />                
        <br />
        <table>
            <tr>
                <td>Period</td>
                <td>:</td>
                <td>
                    <asp:DropDownList CssClass="DropDownList" ID="ddlMonthStart" runat="server">                              
                          </asp:DropDownList>
                    <asp:DropDownList CssClass="DropDownList" ID="ddlYearStart" runat="server">                              
                          </asp:DropDownList>
                    &nbsp;</td>                               
                <td>
                    s/d</td>                               
                <td>
                    <asp:DropDownList CssClass="DropDownList" ID="ddlMonthEnd" runat="server">                              
                          </asp:DropDownList>
                    <asp:DropDownList CssClass="DropDownList" ID="ddlYearEnd" runat="server">                              
                          </asp:DropDownList>                          
                </td>                               
            </tr>
        </table>  
        <table>            
            <tr>
                <td>Print Data</td>
                <td>:</td>
                <td>
                    <asp:CheckBoxList  ID="cbPrintData" runat="server" RepeatColumns="4" >
                        <asp:ListItem Selected="True">Qty</asp:ListItem>
                        <asp:ListItem>M2</asp:ListItem>
                        <asp:ListItem>Roll</asp:ListItem>
                        <asp:ListItem>Nominal</asp:ListItem>
                    </asp:CheckBoxList>
                </td>
            </tr>
        </table>          
        <table>            
            <tr>
                <td>Report Type</td>
                <td>:</td>
                <td>
                    <asp:RadioButtonList  ID="rbGroup" runat="server" RepeatColumns="2" >
                        <asp:ListItem Selected="True">Summary</asp:ListItem>
                        <asp:ListItem>Detail</asp:ListItem>
                    </asp:RadioButtonList>
                </td>
            </tr>
        </table>          
        <asp:Button class="bitbtn btnpreview" runat="server" ID="btnPreview" Visible="false" Text="Preview"/>
        <asp:Button class="bitbtn btnprint" runat="server" ID="btnPrint" Text="Print"/>
        <br />
        <GridRpt:RptGridCtrl ID="ReportGrid" runat = "server" />         
        <asp:Label ID="lbStatus" runat="server" ForeColor="Red" />
    </div>
    </form>
</body>
</html>
