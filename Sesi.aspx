<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Sesi.aspx.vb" Inherits="Sesi"
    EnableEventValidation="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Sesi</title>

    <script src="../../Function/OpenDlg.js" type="text/javascript"></script>
    <script src="../../Function/Function.JS" type="text/javascript"></script>
    <link href="Styles/Style.css" rel="stylesheet" type="text/css" />
</head>
<body>
    
    <form id="form1" runat="server">
    <div style=" margin:auto;    width: 50%;    padding-top: 70px; padding-left: 130px; " class="Content">
        <div style="padding-left: 70px;">
        <img width="250" src="image/session2.png" alt="">
        </div>
        <div class="H1">
            Upss...... Your Session Has Expired !!</div>
        <br />
        <div style="padding-left: 100px;">
        <asp:Button runat="server" class="bitbtn btnsave" ID="btnSesi" Text="Back to Log In" Font-Bold="True" Height="30px"
            Width="150px" />
        </div>
    </div>
    </form>
</div>
</body>
</html>
