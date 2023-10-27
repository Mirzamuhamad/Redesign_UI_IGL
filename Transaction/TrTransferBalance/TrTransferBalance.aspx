<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TrTransferBalance.aspx.vb" Inherits="Transaction_TrTransferBalance_TrTransferBalance" %>

<%@ Register Assembly="BasicFrame.WebControls.BasicDatePicker" Namespace="BasicFrame.WebControls"
    TagPrefix="BDP" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Transfer Ending Balance</title>
    <script src="../../Function/OpenDlg.js" type="text/javascript"></script>
    <script src="../../Function/Function.JS" type="text/javascript"></script>
    <script type="text/javascript">
        function setdigit(nStr, digit)
        {
        try
        {
        var TNstr = parseFloat(nStr);        
        if ( parseFloat(digit) >= 0) 
        {     
           TNstr = TNstr.toFixed(digit);                
        } 
        nStr = TNstr;        
	    nStr += '';
	    x = nStr.split('.');
	    x1 = x[0];
	    x2 = x.length > 1 ? '.' + x[1] : '';
	    var rgx = /(\d+)(\d{3})/;
	    while (rgx.test(x1)) {
		    x1 = x1.replace(rgx, '$1' + ',' + '$2');
	    }
	    return x1 + x2;
	    }catch (err){
            alert(err.description);
          }  
        }
        
        function setformat(){   
          try
          { 
                      
          }catch (err){
            alert(err.description);
          }
        }
    </script>
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
    
</head>
<body>
    <form id="form1" runat="server">
    <div class="Content">
    <div class="H1">Transfer Ending Balance</div>
     <hr style="color:Blue" />        
    <asp:Panel runat="server" id="PnlHd"> 
      <table>
        <tr>
            
        </tr>
      </table>
      <asp:Panel runat="server" ID="pnlSearch" Visible="false">
      <table>
        <tr>
                
        </tr>        
      </table>      
      </asp:Panel>
         <table>
          <tr>
                <td><asp:Label ID="label1"  runat="server" Text="Transfer Ending Balance From This Year : "/></td>                
                <td><asp:DropDownList ID="ddlYear" ValidationGroup="Input" AutoPostBack="true" runat="server" CssClass="DropDownList" />  to  </td>
                <td><asp:Label ID="lbYearAfter"  runat="server" Text="2020"/></td>                
          </tr>
          <tr>
              <td>
              <asp:Button class="bitbtn btnapply" runat="server" ID="btnProcess" Text="Process" />           
          </tr>
       
        </table>
            
     </asp:Panel>    
             
    </div>   
    <asp:Label ID="lbStatus" runat="server" ForeColor="red" />     
    </form>
</body>
</html>
