<%@ Page Language="VB" AutoEventWireup="false" CodeFile="AccountingPeriod.aspx.vb" Inherits="AccountingPeriod" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Accounting Period</title>
    <link href="Styles/Style.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div class="Content">
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
                <td colspan="3"><asp:Button id="btnSubmit" runat="server" CssClass="Button" Text="OK" /></td>
            </tr>     
        </table>
        <asp:Label ID="lbStatus" runat="server" ForeColor="Red"/>
    </div>
    </form>    
</body>
</html>
