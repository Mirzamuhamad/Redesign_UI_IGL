<%@ Page Language="VB" AutoEventWireup="false" CodeFile="RptExpSummary.aspx.vb" Inherits="Rpt_RptExpSummary_RptExpSummary" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<%@ Register Assembly="BasicFrame.WebControls.BasicDatePicker" Namespace="BasicFrame.WebControls"
    TagPrefix="BDP" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Untitled Page</title>
    <script src="../../Function/OpenDlg.js" type="text/javascript"></script>    
   
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .style2
        {
            height: 14px;
        }
        .style3
        {
            width: 163px;
        }
        .style5
        {
            width: 82px;
        }
        .style6
        {
            height: 14px;
            width: 82px;
        }
        .style8
        {
            width: 187px;
        }
        .style9
        {
            width: 151px;
        }
        .style10
        {
            width: 8px;
        }
        .style11
        {
            width: 2px;
        }
        .style13
        {
            height: 14px;
            width: 5px;
        }
        .style14
        {
            width: 5px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div class="Content">
    <div class="H1">Expense Summary&nbsp; Report</div>
    <hr style="color:Blue" />                
        <table style="width: 477px">
            <tr>
                <td class="style5">Range Period</td>
                <td class="style14">:</td>
                <td class="style8" colspan="2">
                    <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlPeriodX" 
                        AutoPostBack="true" >
                      <asp:ListItem Value="1" Selected="True">Monthly</asp:ListItem>
                      <asp:ListItem Value="3">3 Month</asp:ListItem>
                      <asp:ListItem Value="6">Semester</asp:ListItem>
                      <asp:ListItem Value="12">Yearly</asp:ListItem>                      
                      </asp:DropDownList>
                      <asp:TextBox ID="tbPeriod" runat="server" Text = "1" AutoPostBack="true" Width="30px"></asp:TextBox> x Period        
                  </td>
                <td class="style11">&nbsp;</td>
                <td class="style3">
                    &nbsp;</td>    
            </tr>
            <tr>
                <td class="style5">Period</td>
                <td class="style14">:</td>
                <td class="style9">
                    <asp:DropDownList ID="ddlYear" runat="server" CssClass="DropDownList" 
                        AutoPostBack="True" />
                    <asp:DropDownList ID="ddlPeriod" runat="server" CssClass="DropDownList" 
                        AutoPostBack="True" />
                </td>
                <td class="style10">
                    s/d</td>
                <td class="style11">&nbsp;&nbsp;</td>
                <td class="style3">
                    <asp:DropDownList ID="ddlYearEnd" runat="server" CssClass="DropDownList" Enabled="False" />
                    <asp:DropDownList ID="ddlPeriodEnd" runat="server" CssClass="DropDownList" Enabled="False"  />
                </td>    
            </tr>
            <tr>
                <td class="style6">Expense Type</td>
                <td class="style13">:</td>
                <td colspan="4" class="style2">
   
                    <asp:DropDownList ID="ddlExpense" runat="server" CssClass="DropDownList" />
   
                </td>                   
            </tr>
            <tr>
                <td class="style5">Report Type</td>
                <td class="style14">:</td>
                <td colspan = "4">
                    <asp:RadioButtonList ID="rbType" runat="server" RepeatColumns="2" Width="100%" 
                        style="margin-right: 0px">
                       <asp:ListItem Value="0" Selected="True">Summary</asp:ListItem>
                       <asp:ListItem Value="1">Detail</asp:ListItem>
                    </asp:RadioButtonList> 
                </td>
            </tr>
            <tr>
                <td class="style5">&nbsp;</td>
                <td class="style14">&nbsp;</td>
                <td colspan="4">
                    &nbsp;</td>
            </tr>
        </table>        
        <br />
        <asp:Button class="bitbtn btnpreview" runat="server" ID="btnPreview" Visible="false" Text="Preview" />   
        <asp:Button class="bitbtn btnprint" runat="server" ID="btnPrint" Text="Print" />           
        <br />
        <asp:Label ID="lbStatus" runat="server" ForeColor="Red" />
    </div>
    </form>
</body>
</html>