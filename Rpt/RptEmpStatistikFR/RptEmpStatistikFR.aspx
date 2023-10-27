<%@ Page Language="VB" AutoEventWireup="false" CodeFile="RptEmpStatistikFR.aspx.vb" Inherits="Rpt_RptEmpStatistikFR_ReportTemplate" %>

<%@ Register Assembly="GMDatePicker" Namespace="GrayMatterSoft" TagPrefix="cc1" %>
<%@ Register assembly="CrystalDecisions.Web, Version=10.5.3700.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register TagPrefix="GridRpt" TagName="RptGridCtrl" Src="~/UserControl/ReportGrid.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
    <script src="../../Function/OpenDlg.js" type="text/javascript"></script>
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />    
</head>
<body>
    <form id="form1" runat="server">
    <div class="Content">
    <div class="H1">Employee Statistic Report</div>
     <hr style="color:Blue" />
      <table>
        <tr>
            <td>Year</td>
            <td>:</td>
            <td><asp:DropDownList ID="ddlStartYear" runat="server" CssClass="DropDownList" /> &nbsp- <asp:DropDownList ID="ddlEndYear" runat="server" CssClass="DropDownList" />                  
            </td>                  
        </tr>
        <tr>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td>&nbsp
                </td>                  
        </tr>          
      </table>    
      <br />
      <asp:Button class="bitbtn btnpreview" runat="server" ID="btnPreview" Visible="false" Text="Preview" />   
      <asp:Button class="bitbtn btnprint" runat="server" ID="btnPrint" Text="Print" />   
      <br />  
      <GridRpt:RptGridCtrl ID="ReportGrid" runat = "server" Visible = "false"/>
    </div>
    <br />    
    <asp:Label ID="lbStatus" runat="server" ForeColor="Red" />
    </form>
    </body>
</html>
