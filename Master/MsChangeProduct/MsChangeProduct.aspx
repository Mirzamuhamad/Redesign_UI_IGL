<%@ Page Language="VB" AutoEventWireup="false" CodeFile="MsChangeProduct.aspx.vb" Inherits="Master_MsChangeProduct_MsChangeProduct" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Untitled Page</title>
    <script src="../../Function/OpenDlg.js" type="text/javascript"></script>
    <script src="../../Function/Function.JS" type="text/javascript"></script>
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
 <link href="https://fonts.googleapis.com/css2?family=Muli:wght@300;400;600&display=swap" rel="stylesheet">
</head>
<body>
    <form id="form1" runat="server">
    <div class="Content">
    <div class="H1">Change Product</div>
    <hr style="color:Blue" />
        <table>
            <tr>
                <td>Type</td>
                <td>:</td>
                <td>
                  <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlType">
                      <asp:ListItem>Finish Good</asp:ListItem>
                      <asp:ListItem>Material</asp:ListItem>
                    </asp:DropDownList>
                  </td>
            </tr>
            <tr>
                <td>Current Product</td>
                <td>:</td>
                <td>                                
                    <asp:TextBox CssClass="TextBox" ValidationGroup="Input" runat="server" 
                        ID="tbCode" AutoPostBack="true" Width="145px" />
                    <asp:TextBox CssClass="TextBoxR" runat="server" ID="tbName" Enabled="false" 
                        Width="225px"/>
                    <asp:ImageButton ID="btnCurrent" runat="server" ValidationGroup="Input" 
                      ImageUrl="../../Image/btnDot2on.png"
                      onmouseover="this.src='../../Image/btnDot2off.png';"
                      onmouseout="this.src='../../Image/btnDot2on.png';"
                      ImageAlign="AbsBottom" />               
                </td>
            </tr>
            <tr>
                <td>New Product</td>
                <td>:</td>
                <td>                                
                    <asp:TextBox CssClass="TextBox" ValidationGroup="Input" runat="server" 
                        ID="tbCodeNew" AutoPostBack="true" Width="145px" />
                    <asp:TextBox CssClass="TextBoxR" runat="server" ID="tbNewName" Enabled="false" 
                        Width="225px"/>
                    <asp:ImageButton ID="btnNewProduct" runat="server" ValidationGroup="Input" 
                      ImageUrl="../../Image/btnDot2on.png"
                      onmouseover="this.src='../../Image/btnDot2off.png';"
                      onmouseout="this.src='../../Image/btnDot2on.png';"
                      ImageAlign="AbsBottom" />               
                </td>
            </tr>
            <tr>
                <td colspan="3" align="center">
                    <asp:Button runat="server" ID="btnSave" Text="Save" CssClass="Button" />
                </td>
            </tr>
        </table>   
    <asp:Label ID="lbStatus" runat="server" ForeColor="Red" />     
    </div>
    </form>
</body>
</html>
