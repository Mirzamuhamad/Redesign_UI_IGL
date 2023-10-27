<%@ Page Language="VB" AutoEventWireup="false" CodeFile="GraphPerbandinganBPBInvoice.aspx.vb" Inherits="Rpt_GraphPerbandinganBPBInvoice_GraphPerbandinganBPBInvoice" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register TagPrefix="GridRpt" TagName="RptGridCtrl" Src="~/UserControl/ReportGrid.ascx" %>

<%@ Register Assembly="BasicFrame.WebControls.BasicDatePicker" Namespace="BasicFrame.WebControls"
    TagPrefix="BDP" %>
    
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Untitled Page</title>
    <script src="../../Function/OpenDlg.js" type="text/javascript"></script>
    <script src="../../Function/Function.JS" type="text/javascript"></script>
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div class="Content">
    <div class="H1">Graph Perbandingan BPB Invoice</div>
    <hr style="color:Blue" />                
        <br />
        <table>
            <tr>
            <td>Range Period</td>
            <td>:</td>
            <td><asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlPeriod" AutoPostBack="true" >
                      <asp:ListItem Value="1" Selected="True">Monthly</asp:ListItem>
                      <asp:ListItem Value="3">3 Month</asp:ListItem>
                      <asp:ListItem Value="6">Semester</asp:ListItem>
                      <asp:ListItem Value="12">Yearly</asp:ListItem>                      
                      </asp:DropDownList>
                      <asp:TextBox ID="tbPeriod" runat="server" Text = "1" AutoPostBack="true" Width="30px"></asp:TextBox> x Period        
                  </td>
            </tr>
           <tr>
            <td>Period</td>
            <td>:</td>
            <td><asp:DropDownList ID="ddlStartYear" runat="server" CssClass="DropDownList" AutoPostBack="true"/> 
                <asp:DropDownList ID="ddlStartMonth" runat="server" CssClass="DropDownList" AutoPostBack="true"/>
                s/d         
                <asp:DropDownList ID="ddlEndYear" runat="server" CssClass="DropDownList" Enabled="false" /> 
                <asp:DropDownList ID="ddlEndMonth" runat="server" CssClass="DropDownList" Enabled="false"/>                 
            </td>                  
        </tr>
        </table>
        <table>
        <tr>            
            <td>
            <fieldset style="width:260px">
            <legend>Report Type : </legend>
                <asp:RadioButtonList ID="RBType" Width="250px" runat="server" RepeatColumns="2">
                <asp:ListItem Selected="True">Qty</asp:ListItem>
                <asp:ListItem>Total (Rp)</asp:ListItem>
            </asp:RadioButtonList>          
            </fieldset>
            </td>
        </tr> 
        </table>
        <br>
        <asp:ImageButton ID="btnPreview" runat="server"  
                    ImageUrl="../../Image/btnPreviewOn.png"
                    onmouseover="this.src='../../Image/btnPreviewOff.png';"
                    onmouseout="this.src='../../Image/btnPreviewOn.png';"
                    ImageAlign="AbsBottom" />        
        <asp:ImageButton ID="btnPrint" runat="server"  
                    ImageUrl="../../Image/btnPrintOn.png"
                    onmouseover="this.src='../../Image/btnPrintOff.png';"
                    onmouseout="this.src='../../Image/btnPrintOn.png';"
                    ImageAlign="AbsBottom" />                
        <br />
        <GridRpt:RptGridCtrl ID="ReportGrid" runat = "server" />         
        <asp:Label ID="lbStatus" runat="server" ForeColor="Red" />
    </div>
    </form>
</body>
</html>
