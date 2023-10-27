<%@ Page Language="VB" AutoEventWireup="false" CodeFile="PrintForm.Aspx.vb" Inherits="Rpt_PrintForm" %>

<%@ Register Assembly="FastReport" Namespace="FastReport.Web" TagPrefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Print Form</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>   
        <cc1:WebReport ID="WebReport1" runat="server" AutoHeight="True"  
            AutoWidth="True" Width="100%" Height = "100%" />
    </div>
    <asp:Label ID="lbstatus" ForeColor="red" runat="server"></asp:Label>
        
    </form>
</body>
</html>
