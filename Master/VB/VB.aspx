<%@ Page Language="VB" AutoEventWireup="false" CodeFile="VB.aspx.vb" Inherits="VB" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Import Excel Data into Database</title>
</head>
<body>
    <form id="form1" runat="server">
<asp:Panel ID="Panel1" runat="server">
    <asp:FileUpload ID="FileUpload1" runat="server" />
    <asp:Button ID="btnUpload" runat="server" Text="Upload" 
             OnClick="btnUpload_Click" />
    <br />
    <asp:Label ID="lblMessage" runat="server" Text="" />
</asp:Panel>
<asp:Panel ID="Panel2" runat="server" Visible = "false" >
    <asp:Label ID="Label5" runat="server" Text="File Name"/>
    <asp:Label ID="lblFileName" runat="server" Text=""/>
    <br />
    <asp:Label ID="Label2" runat="server" Text="Select Sheet" />
    <asp:DropDownList ID="ddlSheets" runat="server" 
                    AppendDataBoundItems = "true">
    </asp:DropDownList>
    <br />
    <asp:Label ID="Label3" runat="server" Text="Enter Source Table Name"/>
    <asp:TextBox ID="txtTable" runat="server"></asp:TextBox>
    <br />
    <asp:Label ID="Label1" runat="server" Text="Has Header Row?" />
    <br />
    <asp:RadioButtonList ID="rbHDR" runat="server">
        <asp:ListItem Text = "Yes" Value = "Yes" Selected = "True" >
        </asp:ListItem>
        <asp:ListItem Text = "No" Value = "No"></asp:ListItem>
    </asp:RadioButtonList>
    <br />
    <asp:Button ID="btnSave" runat="server" Text="Save" 
          OnClick="btnSave_Click" />
    <asp:Button ID="btnCancel" runat="server" Text="Cancel" 
          OnClick="btnCancel_Click" />        
 </asp:Panel>
   <asp:Label runat="server" id="lbStatus"></asp:Label>
    </form>
</body>
</html>
