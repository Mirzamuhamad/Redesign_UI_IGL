<%@ Page Language="VB" AutoEventWireup="false" CodeFile="GraphPembelianBulanan.aspx.vb" Inherits="Rpt_GraphPembelianBulanan_ReportTemplate" %>

<%@ Register Assembly="GMDatePicker" Namespace="GrayMatterSoft" TagPrefix="cc1" %>
<%@ Register assembly="CrystalDecisions.Web, Version=10.5.3700.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register TagPrefix="GridRpt" TagName="RptGridCtrl" Src="~/UserControl/ReportGrid.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
    <script src="../../Function/OpenDlg.js" type="text/javascript"></script>
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
 <link href="https://fonts.googleapis.com/css2?family=Muli:wght@300;400;600&display=swap" rel="stylesheet">    
</head>
<body>
    <form id="form1" runat="server">
    <div class="Content">
    <div class="H1">Graph Pembelian Bulanan</div>
     <hr style="color:Blue" />
      <table>
        <tr>
            <td>Start Period</td>
            <td>:</td>
            <td><asp:DropDownList ID="ddlStartYear" runat="server" CssClass="DropDownList" /> &nbsp
                <asp:DropDownList ID="ddlStartMonth" runat="server" CssClass="DropDownList" />                 
            </td>                  
        </tr>
        <tr>
            <td>End Period</td>
            <td>:</td>
            <td><asp:DropDownList ID="ddlEndYear" runat="server" CssClass="DropDownList" /> &nbsp
                <asp:DropDownList ID="ddlEndMonth" runat="server" CssClass="DropDownList" />                 
            </td>                  
        </tr>          
      </table>    
      <table>
      <tr>
      <td>      
      <fieldset style="width:260px">
                    <legend>Report Type</legend>
                    <asp:RadioButtonList ID="rgReport" runat="server" RepeatDirection="Horizontal">
                      <asp:ListItem Value="0" Selected="True">PO</asp:ListItem>
                      <asp:ListItem Value="1">BTB</asp:ListItem>
                      <asp:ListItem Value="2">FAKTUR</asp:ListItem>
                  </asp:RadioButtonList>       
        </fieldset>  
        </td>
        <td>        
        <fieldset style="width:260px">
                    <legend>Currency Type</legend>
                    <asp:RadioButtonList ID="rgData" runat="server" RepeatDirection="Horizontal">
                      <asp:ListItem Value="0" Selected="True">Foreign</asp:ListItem>
                      <asp:ListItem Value="1">Home</asp:ListItem>                      
                  </asp:RadioButtonList>       
        </fieldset>  
        </td>
      </tr>
      </table>
        <asp:Button class="bitbtn btnpreview" runat="server" ID="btnView" Text="Preview" />   
        <asp:Button class="bitbtn btnprint" runat="server" ID="btnPrint" Text="Print" /> 
        <asp:Button class="bitbtn btnexcel" runat="server" ID="btnExport" Text="Export" />                    
      <GridRpt:RptGridCtrl ID="ReportGrid" runat = "server" />
      <asp:GridView ID="GridExport" runat="server" AutoGenerateColumns="True" 
                Visible="true" ShowFooter="False" GridLines="None" Height="79px" PageSize="20" Width="724px">
               <HeaderStyle CssClass="GridHeader" />
               <RowStyle CssClass="GridItem" Wrap="false" />
               <AlternatingRowStyle CssClass="GridAltItem" />
               <PagerStyle CssClass="GridPager" />
           </asp:GridView>         
    </div>
    <br />    
    <asp:Label ID="lbStatus" runat="server" ForeColor="Red" />
    </form>
    </body>
</html>
