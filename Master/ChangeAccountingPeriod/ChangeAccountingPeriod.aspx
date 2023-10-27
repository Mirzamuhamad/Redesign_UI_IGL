<%@ Page Language="VB" AutoEventWireup="false" CodeFile="ChangeAccountingPeriod.aspx.vb" Inherits="Master_ChangeAccountingPeriod_ChangeAccountingPeriod" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div class="Content">
    <div class="H1">Change Accounting Period</div>
    <hr style="color:Blue" />
        <table>
            <tr>
                <td>Year</td>
                <td>:</td>
                <td><asp:DropDownList ID="ddlYear" runat="server" CssClass="DropDownList" />                     
                </td>
            </tr>
            <tr>
                <td>Month</td>
                <td>:</td>
                <td><asp:DropDownList ID="ddlMonth" runat="server" CssClass="DropDownList" />                     
                </td>
            </tr>       
            <tr>
                <td colspan="3">&nbsp;</td>
            </tr>
            <tr>
                <td colspan="3"><asp:Button Class ="bitbtn btnsave" id="btnSubmit" runat="server" Text="OK" /></td>
            </tr>     
        </table>        
        <asp:Label ID="lbStatus" runat="server" ForeColor="Red"/>    
    
    </div>
    </form>
</body>
</html>
