<%@ Page Language="VB" AutoEventWireup="false" CodeFile="RptEmpUnderContract.aspx.vb" Inherits="Rpt_RptEmpUnderContract_RptEmpUnderContract" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register TagPrefix="GridRpt" TagName="RptGridCtrl" Src="~/UserControl/ReportGrid.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Untitled Page</title>
    <script src="../../Function/OpenDlg.js" type="text/javascript"></script>
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .style1
        {
            width: 155px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div class="Content">
    <div class="H1">Employee Under Contract Report</div>
    <hr style="color:Blue" />  
    <table style="width: 587px">
     <tr>
        <td class="style1">
        <fieldset style="width:217px; height: 59px;">
            <legend>&nbsp;Employee Status : </legend>
            <asp:RadioButtonList ID="rbStatus" Width="209px" runat="server" 
                RepeatColumns="3" Height="41px">
                <asp:ListItem Selected="True">All</asp:ListItem>
                <asp:ListItem>Active</asp:ListItem>
                <asp:ListItem>Non Active</asp:ListItem>
            </asp:RadioButtonList>
        </fieldset></td>
        <td>
        <fieldset style="width:357px">
            <legend>&nbsp;Report Type : </legend>
            <asp:RadioButtonList ID="rbType" Width="349px" runat="server" 
                RepeatColumns="3">
                <asp:ListItem Selected="True">Employee</asp:ListItem>
                <asp:ListItem>Department</asp:ListItem>
                <asp:ListItem>Job Title</asp:ListItem>
                <asp:ListItem>Job Level</asp:ListItem>
                <asp:ListItem>Employee Type</asp:ListItem>
            </asp:RadioButtonList>
        </fieldset></td>
     </tr>
    </table>
        &nbsp;
     <br />
        <br />
        <asp:Button class="bitbtn btnpreview" runat="server" ID="btnPreview" Visible="false" Text="Preview"/>
        <asp:Button class="bitbtn btnprint" runat="server" ID="btnPrint" Text="Print"/>
        <br />
        <br />
        <GridRpt:RptGridCtrl ID="ReportGrid" runat = "server" />         
        <asp:Label ID="lbStatus" runat="server" ForeColor="Red" />
    </div>
    </form>
</body>
</html>
