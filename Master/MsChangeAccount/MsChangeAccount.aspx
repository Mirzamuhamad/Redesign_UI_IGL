<%@ Page Language="VB" AutoEventWireup="false" CodeFile="MsChangeAccount.aspx.vb" Inherits="Master_MsChangeAccount_MsChangeAccount" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Untitled Page</title>
     <script type="text/javascript">
    function OpenPopup() {         
        window.open("../../SearchDlg.Aspx","List","scrollbars=yes,resizable=no,width=500,height=400");        
        return false;
    }    
   </script>
   <script src="../../Function/OpenDlg.JS" type="text/javascript"></script>
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div class="Content">
    <div class="H1">Replace Account</div>
    <hr style="color:Blue" />
        <table>
            
            <tr>
                <td>Current Account</td>
                <td>:</td>
                <td>                                
                    <asp:TextBox CssClass="TextBox" ValidationGroup="Input" runat="server" 
                        ID="tbCode" AutoPostBack="true" Width="145px" />
                    <asp:TextBox CssClass="TextBoxR" runat="server" ID="tbName" Enabled="false" 
                        Width="225px"/>
                    <asp:Button ID="btnCurrent" runat="server" class="btngo" Text="..."/>                                
                </td>
            </tr>
            <tr>
                <td>New Account</td>
                <td>:</td>
                <td>                                
                    <asp:TextBox CssClass="TextBox" ValidationGroup="Input" runat="server" 
                        ID="tbCodeNew" AutoPostBack="true" Width="145px" />
                    <asp:TextBox CssClass="TextBoxR" runat="server" ID="tbNewName" Enabled="false" 
                        Width="225px"/>
                    <asp:Button ID="btnNewProduct" runat="server" class="btngo" Text="..."/>                  
                </td>
            </tr>
            <tr>
                <td colspan="3" align="center">
                   <asp:Button class="bitbtn btnsave" runat="server" ID="btnSave" Text="Save" />
                </td>
            </tr>
        </table>   
    <asp:Label ID="lbStatus" runat="server" ForeColor="Red" />     
    </div>
    </form>
</body>
</html>
