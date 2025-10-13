<%@ Page Language="VB" AutoEventWireup="false" CodeFile="RptAPRekap.aspx.vb" Inherits="Rpt_RptAPRekap_RptAPRekap" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register TagPrefix="GridRpt" TagName="RptGridCtrl" Src="~/UserControl/ReportGrid.ascx" %>
<%@ Register Assembly="BasicFrame.WebControls.BasicDatePicker" Namespace="BasicFrame.WebControls"
    TagPrefix="BDP" %>
<%@ Register TagPrefix="GridRpt2" TagName="RptGridCtrl2" Src="~/UserControl/RangeControl5.ascx" %>    

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Untitled Page</title>
    <script src="../../Function/OpenDlg.js" type="text/javascript"></script>
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
 <link href="https://fonts.googleapis.com/css2?family=Muli:wght@300;400;600&display=swap" rel="stylesheet">    
    <style type="text/css">
        .style1
        {
            width: 32px;
        }
        .style2
        {
            width: 268px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div class="Content">
    <div class="H1">AP Rekap</div>
     <hr style="color:Blue" />     
        <br />
      <table>
        <tr>
            <td>Date</td>
            <td>:</td>
            <td class="style2">
                <BDP:BasicDatePicker ID="tbStartDate" runat="server" DateFormat="dd MMM yyyy" 
                ShowNoneButton = "false" ReadOnly = "true" ValidationGroup="Input"
                ButtonImageHeight="19px" ButtonImageWidth="20px" 
                DisplayType="TextBoxAndImage" 
                TextBoxStyle-CssClass="TextDate" AutoPostBack="True"><TextBoxStyle CssClass="TextDate" /></BDP:BasicDatePicker>
                As
                <BDP:BasicDatePicker ID="tbEndDate" runat="server" DateFormat="dd MMM yyyy" 
                ShowNoneButton = "false" ReadOnly = "true" ValidationGroup="Input"
                ButtonImageHeight="19px" ButtonImageWidth="20px" 
                DisplayType="TextBoxAndImage" 
                TextBoxStyle-CssClass="TextDate" AutoPostBack="True"><TextBoxStyle CssClass="TextDate" /></BDP:BasicDatePicker>
            </td>
            <td class="style1">&nbsp;</td>
            <td> <asp:CheckBox ID="cbAR" runat="server" Checked="true" Text="Invoice" /></td>                  
            <td> <asp:CheckBox ID="cbBPB" runat="server" Checked="true" 
                    Text="RR - Un Invoice" /></td>                  
            <td> &nbsp;</td>                  
            <%--<td>
                <asp:CheckBox ID="cbValue" runat="server" Text="Print Have Value" />                
            </td>
            <td>
                <asp:CheckBox ID="cbForce" runat="server" Text="Force New Page" />                
            </td>  --%>                
        </tr>          
      </table>       
      <table>
        <tr>
            <td>
            <fieldset style="width:260px">
            <legend>Report Type</legend>
            <asp:RadioButtonList ID="RBType" Width="250px" runat="server" RepeatColumns="3">
                <asp:ListItem Selected="True">Summary</asp:ListItem>
                <asp:ListItem>Detail</asp:ListItem>
            </asp:RadioButtonList>          
            </fieldset>            
            <fieldset style="width:260px">
            <legend>Currency Type</legend>
            <asp:RadioButtonList ID="RBCurr" Width="250px" runat="server" RepeatColumns="3">
                <asp:ListItem Selected="True">Foreign</asp:ListItem>
                <asp:ListItem>Home</asp:ListItem>
            </asp:RadioButtonList>          
            </fieldset>            
            <fieldset style="width:260px">
            <legend>Type</legend>
            <asp:RadioButtonList ID="RBFwd" Width="250px" runat="server" RepeatColumns="3">
                <asp:ListItem Selected="True">Backward</asp:ListItem>
                <asp:ListItem>Forward</asp:ListItem>
            </asp:RadioButtonList>          
            </fieldset>
            </td>        
            <td rowspan="2"><GridRpt2:RptGridCtrl2 ID="RangeControl1" runat = "server" /></td>
        </tr>
        </table>        
      <br />
        <asp:Button class="bitbtn btnpreview" runat="server" ID="btnPreview" Visible="false" Text="Preview"/>
        &nbsp;<asp:Button class="bitbtn btnprint" runat="server" ID="btnPrint" Text="Print"/>             
        &nbsp;<asp:Button class="bitbtn btncopyto" runat="server" ID="btnExport" Text="Copy To"/>             
      <br />         
      <br />
      <GridRpt:RptGridCtrl ID="ReportGrid" runat = "server" />
    </div>
    <br />
      <asp:GridView ID="GridExport" runat="server" AutoGenerateColumns="True" 
                Visible="true" ShowFooter="False" GridLines="None" Height="79px" PageSize="20" Width="724px">
               <HeaderStyle CssClass="GridHeader" />
               <RowStyle CssClass="GridItem" Wrap="false" />
               <AlternatingRowStyle CssClass="GridAltItem" />
               <PagerStyle CssClass="GridPager" />
        </asp:GridView>         
    
    <asp:Label ID="lbStatus" runat="server" ForeColor="Red" />
    </form>
    </body>
</html>
