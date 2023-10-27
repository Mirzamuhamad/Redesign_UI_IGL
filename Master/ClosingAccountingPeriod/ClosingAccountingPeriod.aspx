<%@ Page Language="VB" AutoEventWireup="false" CodeFile="ClosingAccountingPeriod.aspx.vb" Inherits="Master_ClosingAccountingPeriod_ClosingAccountingPeriod" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .style1
        {
            width: 163px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div class="Content">
    <div class="H1">Closing Accounting Period</div>
    <hr  />
        <table>
            <tr>
                <td>Closing Accounting Period</td>   
                <td> : </td>   
                <td><asp:Label ID="lblYearMonth1"  style="color:Red;" runat="server" ForeColor="Black" Width="150px"/></td>                  
                <td><asp:Button Class ="bitbtn btnsave" id="btnClose" runat="server"  Text="Closing" Width="100px" Height ="20"/></td> 
                <td><asp:Label ID ="lblYear"  style="color:Black;" runat="server" ForeColor="Black" Width="50px" Visible="False"/></td>                              
                <td><asp:Label ID ="lblPeriod"  style="color:Black;" runat="server" ForeColor="Black" Width="50px" Visible="False"/></td>  
            </tr>
            <tr>
                <td>Unclosing Accounting Period</td> 
                <td> : </td>   
                <td><asp:Label ID="lblYearMonth2"  style="color:Red;" runat="server" ForeColor="Black" Width="150px"/></td>  
                <td><asp:Button  Class ="bitbtn btndelete" id="btnUnClose" runat="server"  Text="UnClosing" Width="100px" /></td>                                             
                <td><asp:Label ID ="lblYear2"  style="color:Black;" runat="server" ForeColor="Black" Width="30px" Visible="False"/></td>                              
                <td><asp:Label ID ="lblPeriod2"  style="color:Black;" runat="server" ForeColor="Black" Width="30px" Visible="False"/></td>                  
            </tr>      
         
        </table>       
           <asp:Label ID="lbStatus" runat="server" ForeColor="Red"/>      
    </div>
    </form>
</body>
</html>
