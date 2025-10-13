<%@ Page Language="VB" AutoEventWireup="false" CodeFile="ChangeUserPassword.aspx.vb" Inherits="Master_ChangeUserPassword_ChangeUserPassword" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
 <link href="https://fonts.googleapis.com/css2?family=Muli:wght@300;400;600&display=swap" rel="stylesheet">
</head>
<body>
    <form id="form1" runat="server">
    <div class="Content">
    <div class="H1">Change Password</div>
    <hr style="color:Blue" />
        <table>
            <tr>
                <td>User</td>
                <td>:</td>
                <td><asp:Label ID="lbUser" runat="server" ForeColor="Blue" /></td>
            </tr>
            <tr>
                <td>Old Password</td>
                <td>:</td>
                <td><asp:TextBox TextMode="Password" ID="tbOldPassword" runat="server" /></td>
            </tr>
            <tr>
                <td>New Password</td>
                <td>:</td>
                <td><asp:TextBox TextMode="Password" ID="tbNewPassword" runat="server" /></td>
            </tr>
            <tr>
                <td>Confirm New Password</td>
                <td>:</td>
                <td><asp:TextBox TextMode="Password" ID="tbRetypePassword" runat="server" /></td>
            </tr>
            <tr>
                <td colspan="3" align="center">
                    <asp:Button runat="server" class="bitbtn btnsave" ID="btnSave" Text="Save"  />
                    <asp:Button runat="server"  class="bitbtn btnclear" ID="btnReset" Text="Reset" />
                </td>
            </tr>
        </table>   
    <asp:Label ID="lbStatus" runat="server" ForeColor="Red" />     
    </div>
    </form>
</body>
</html>
