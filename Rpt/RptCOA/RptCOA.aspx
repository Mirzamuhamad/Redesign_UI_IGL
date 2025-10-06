<%@ Page Language="VB" AutoEventWireup="false" CodeFile="RptCOA.aspx.vb" Inherits="Rpt_RptCOA_RptCOA" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
    <script src="../../Function/OpenDlg.js" type="text/javascript"></script>
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
 <link href="https://fonts.googleapis.com/css2?family=Muli:wght@300;400;600&display=swap" rel="stylesheet">
</head>
<body>
    <form id="form1" runat="server">
    <div class="Content">
    <div class="H1">Chart of Accounts Report</div>
    <hr style="color:Blue" />        
        <table>
        <%--<tr>
        <td>
        <asp:Label runat="server" ID="lbComp" Text="Company : ">
        </asp:Label>
        <asp:DropDownList ID="ddlCompany" runat="server">
        </asp:DropDownList>
        </td>
        </tr>--%>
        <tr>
        <td>
        <fieldset style="width:260px">
            <legend>Status Account</legend>
            <asp:RadioButtonList ID="RBList1" Width="250px" runat="server" RepeatColumns="3">
                <asp:ListItem Selected="True">All</asp:ListItem>
                <asp:ListItem>Active</asp:ListItem>
                <asp:ListItem>Non Active</asp:ListItem>
            </asp:RadioButtonList>
        </fieldset>
        </td>
        </tr>
        </table>
        
        <br />
        <asp:Button class="bitbtn btnpreview" runat="server" ID="btnPreview" Visible="false" Text="Preview"/>
        &nbsp;<asp:Button class="bitbtn btnprint" runat="server" ID="btnPrint" Text="Print"/>
        <br />
        <asp:Label ID="lbStatus" runat="server" ForeColor="Red" />
    </div>
    </form>
</body>
</html>
