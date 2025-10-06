<%@ Page Language="VB" AutoEventWireup="false" CodeFile="RptPEEmpList.aspx.vb" Inherits="Rpt_RptPEEmpList_RptPEEmpList" %>

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
    <div class="H1">Employee Listing</div>
    <hr style="color:Blue" />        
        <fieldset style="width:260px">
            <legend>&nbsp;Group By : </legend>
            <asp:RadioButtonList ID="RBList1" Width="516px" runat="server" 
                RepeatColumns="3">
                <asp:ListItem Selected="True">Employee</asp:ListItem>
                <asp:ListItem>Department</asp:ListItem>
                <asp:ListItem>Job Level</asp:ListItem>
                <asp:ListItem>Job Title</asp:ListItem>
                <asp:ListItem>Employee Type</asp:ListItem>
                <asp:ListItem>Work Place</asp:ListItem>
            </asp:RadioButtonList>
        </fieldset>
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
