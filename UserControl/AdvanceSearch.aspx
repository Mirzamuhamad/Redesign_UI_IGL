<%@ Page Language="VB" AutoEventWireup="false" CodeFile="AdvanceSearch.aspx.vb" Inherits="UserControl_AdvanceSearch" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register TagPrefix="UC1" TagName="UserControl" Src="~/UserControl/AdvanceFind.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <UC1:UserControl ID="UserControl1" runat="server" />
    </div>
    </form>
</body>
</html>
