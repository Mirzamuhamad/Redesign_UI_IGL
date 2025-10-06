<%@ Page Language="VB" AutoEventWireup="false" CodeFile="RptPROutAgingProd.aspx.vb" Inherits="Rpt_RptPROutAgingProd_RptPROutAgingProd" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register TagPrefix="GridRpt" TagName="RptGridCtrl" Src="~/UserControl/ReportGrid.ascx" %>
<%@ Register Assembly="BasicFrame.WebControls.BasicDatePicker" Namespace="BasicFrame.WebControls"
    TagPrefix="BDP" %>
<%@ Register TagPrefix="GridRpt2" TagName="RptGridCtrl2" Src="~/UserControl/RangeControl.ascx" %>    

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Untitled Page</title>
    <script src="../../Function/OpenDlg.js" type="text/javascript"></script>
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
 <link href="https://fonts.googleapis.com/css2?family=Muli:wght@300;400;600&display=swap" rel="stylesheet">    
</head>
<body>
    <form id="form1" runat="server">
    <div class="Content">
    <div class="H1">PR Out Aging Per Product Report</div>
     <hr style="color:Blue" />
      <table>
        <tr>
            <td>Date</td>
            <td>:</td>
            <td>
                <BDP:BasicDatePicker ID="tbTransDate" runat="server" DateFormat="dd MMM yyyy" 
                ShowNoneButton = "false" ReadOnly = "true" ValidationGroup="Input"
                ButtonImageHeight="19px" ButtonImageWidth="20px" 
                DisplayType="TextBoxAndImage" 
                TextBoxStyle-CssClass="TextDate" AutoPostBack="True"><TextBoxStyle CssClass="TextDate" /></BDP:BasicDatePicker>
            </td>
            <td>&nbsp;</td>
            <td>
                <asp:CheckBox ID="cbForce" runat="server" Text="Force New Page" />
            </td>                  
        </tr>  
              
      </table>  
      <table><tr><td>
       <fieldset style="width:260px; " >
            <legend >Report Type</legend>
            <asp:RadioButtonList ID="RBType" Width="250px" runat="server" RepeatColumns="3" 
                Visible="True" >   
                <asp:ListItem Selected="True">Summary</asp:ListItem>
                <asp:ListItem>Detail</asp:ListItem>
            </asp:RadioButtonList>          
        </fieldset>
        <br />
        <fieldset style="width:260px; " >
            <legend ></legend>
            <asp:RadioButtonList ID="RBFoward" Width="250px" runat="server" RepeatColumns="3" 
                Visible="True" Height="24px" >   
                <asp:ListItem Selected="True">Backward</asp:ListItem>
                <asp:ListItem>Foward</asp:ListItem>
            </asp:RadioButtonList>          
        </fieldset>
      </td>
      <td>
       
      <GridRpt2:RptGridCtrl2 ID="RangeControl1" runat = "server" />
       
      </td></tr>
      </table>   
      <br />
      <br />
        <asp:Button class="bitbtn btnpreview" runat="server" ID="btnPreview" Visible="false" Text="Preview"/>
        <asp:Button class="bitbtn btnprint" runat="server" ID="btnPrint" Text="Print"/>
      <br />         
      <br />
      <GridRpt:RptGridCtrl ID="ReportGrid" runat = "server" />
    </div>
    <br />
    
    <asp:Label ID="lbStatus" runat="server" ForeColor="Red" />
    </form>
    </body>
</html>
