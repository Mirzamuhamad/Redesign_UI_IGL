<%@ Page Language="VB" AutoEventWireup="false" CodeFile="RptStockCardSum.aspx.vb" Inherits="Rpt_RptStockCardSum_RptStockCardSum" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register TagPrefix="GridRpt" TagName="RptGridCtrl" Src="~/UserControl/ReportDoubleGrid.ascx" %>

<%@ Register Assembly="BasicFrame.WebControls.BasicDatePicker" Namespace="BasicFrame.WebControls"
    TagPrefix="BDP" %>
    
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
    <div class="H1">&nbsp;<asp:Label ID="Labelmenu" runat="server" Text="Report Stock Card Summary"></asp:Label></div>
    <hr style="color:Blue" />        
        <%--fieldset style="width:260px">
            <legend>Report Type</legend>
            <asp:RadioButtonList ID="RBType" Width="250px" runat="server" RepeatColumns="3">
                <asp:ListItem Selected="True">Class</asp:ListItem>
                <asp:ListItem>Type</asp:ListItem>
            </asp:RadioButtonList>          
        </fieldset>--%>
        <br />
        <table>
            <tr>
                <td>Period</td>
                <td>:</td>
                <td>
                    <BDP:BasicDatePicker ID="tbStartDate" runat="server" DateFormat="dd MMM yyyy" 
                        ShowNoneButton = "false" ReadOnly = "true" ValidationGroup="Input"
                        ButtonImageHeight="19px" ButtonImageWidth="20px" 
                        DisplayType="TextBoxAndImage" 
                        TextBoxStyle-CssClass="TextDate" AutoPostBack="True"><TextBoxStyle CssClass="TextDate" /></BDP:BasicDatePicker>
                </td>
                <td>As</td>
                <td> 
                    <BDP:BasicDatePicker ID="tbEndDate" runat="server" DateFormat="dd MMM yyyy" 
                        ShowNoneButton = "false" ReadOnly = "true" ValidationGroup="Input"
                        ButtonImageHeight="19px" ButtonImageWidth="20px" 
                        DisplayType="TextBoxAndImage" 
                        TextBoxStyle-CssClass="TextDate" AutoPostBack="True"><TextBoxStyle CssClass="TextDate" /></BDP:BasicDatePicker>
                </td>  
                <td>
                <asp:CheckBox ID="cbForce" runat="server" Text="Force New Page" />
                </td>              
            </tr>            
        </table>
        <%--<fieldset style="width:260px">
                    <legend>Currency Type</legend>
                    <asp:RadioButtonList ID="RBCurr" Width="250px" runat="server" RepeatColumns="3">
                    <asp:ListItem Selected="True">Foreign</asp:ListItem>
                    <asp:ListItem>Home</asp:ListItem>
                    </asp:RadioButtonList>          
        </fieldset>  --%>
              
       <asp:Button class="bitbtn btnpreview" runat="server" ID="btnPreview" Visible="false" Text="Preview" />   
        <asp:Button class="bitbtn btnprint" runat="server" ID="btnPrint" Text="Print" /> 
        <asp:Button class="bitbtn btncopyto" runat="server" ID="btnExport" Text="Export" Visible="True"/>                         
        <br />
        <asp:GridView ID="GridExport" runat="server" AutoGenerateColumns="True" 
                Visible="true" ShowFooter="False" GridLines="None" Height="79px" PageSize="20" Width="724px">
               <HeaderStyle CssClass="GridHeader" />
               <RowStyle CssClass="GridItem" Wrap="false" />
               <AlternatingRowStyle CssClass="GridAltItem" />
               <PagerStyle CssClass="GridPager" />
        </asp:GridView> 
        <GridRpt:RptGridCtrl ID="ReportGrid" runat = "server" />         
        <asp:Label ID="lbStatus" runat="server" ForeColor="Red" />
    </div>
    </form>
</body>
</html>
