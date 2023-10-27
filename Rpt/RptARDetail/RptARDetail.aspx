<%@ Page Language="VB" AutoEventWireup="false" CodeFile="RptARDetail.aspx.vb" Inherits="Rpt_RptARDetail_RptARDetail" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="BasicFrame.WebControls.BasicDatePicker" Namespace="BasicFrame.WebControls"
    TagPrefix="BDP" %>
    
<%@ Register TagPrefix="GridRpt" TagName="RptGridCtrl" Src="~/UserControl/ReportGrid.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Untitled Page</title>
    <script src="../../Function/OpenDlg.js" type="text/javascript"></script>
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .style1
        {
            width: 83px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div class="Content">
    <div class="H1">AR Detail</div>
    <hr style="color:Blue" />                
        <table>
            <tr>
                <td>Period</td>
                <td>:</td>
                <td>
                    <BDP:BasicDatePicker ID="tbStartDate" runat="server" DateFormat="dd MMM yyyy" 
                        ShowNoneButton = "false" ReadOnly = "true" ValidationGroup="Input"
                        ButtonImageHeight="19px" ButtonImageWidth="20px" 
                        DisplayType="TextBoxAndImage" 
                        TextBoxStyle-CssClass="TextDate"><TextBoxStyle CssClass="TextDate" /></BDP:BasicDatePicker>
                </td>   
                <td>as at</td>
                <td>
                    <BDP:BasicDatePicker ID="tbEndDate" runat="server" DateFormat="dd MMM yyyy" 
                        ShowNoneButton = "false" ReadOnly = "true" ValidationGroup="Input"
                        ButtonImageHeight="19px" ButtonImageWidth="20px" 
                        DisplayType="TextBoxAndImage" 
                        TextBoxStyle-CssClass="TextDate"><TextBoxStyle CssClass="TextDate" /></BDP:BasicDatePicker>
                </td>
            </tr>
           </table>           
            
            Report Type :
            <table>
                <tr>
                    <td class="style1">
                        <asp:CheckBox ID="cbAll" runat="server" Text = "All" Checked="true">                       
                        </asp:CheckBox> 
                    </td>
                    <td class="style1">
                        <asp:CheckBox ID="cbCN" runat="server" Text = "CN" Checked="false">                       
                        </asp:CheckBox> 
                    </td>
                    <td class="style1">
                        <asp:CheckBox ID="cbSales" runat="server" Text = "Sales" Checked="false">                       
                        </asp:CheckBox> 
                    </td>
                    <td class="style1">
                        <asp:CheckBox ID="cbReceiptAR" runat="server" Text = "Receipt A/R" Checked="false">                       
                        </asp:CheckBox> 
                    </td>
                </tr>
                <tr>
                    <td class="style1">
                        <asp:CheckBox ID="cbDN" runat="server" Text = "DN" Checked="false">                       
                        </asp:CheckBox> 
                    </td>
                    <td class="style1">
                        <asp:CheckBox ID="cbGiro" runat="server" Text = "Giro Drawn" Checked="false">                       
                        </asp:CheckBox> 
                    </td>                
                    <td class="style1">
                        <asp:CheckBox ID="cbDP" runat="server" Text = "DP Terima" Checked="false">                       
                        </asp:CheckBox> 
                    </td>                
                </tr>
                </table>                
            
        
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