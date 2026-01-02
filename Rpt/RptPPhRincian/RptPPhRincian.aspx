<%@ Page Language="VB" AutoEventWireup="false" CodeFile="RptPPhRincian.aspx.vb" Inherits="Rpt_RptPPhRincian_RptPPhRincian" %>

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
    <div class="H1">Rincian PPH</div>
    <hr style="color:Blue" />        
        Prosess Salary&nbsp;
                <td>:</td>
                </td>
                        <asp:TextBox CssClass="TextBox" ID="tbCode" runat="server" 
            Width="150px" Enabled="False" /> 
                        &nbsp;<asp:Button ID="btnSearch" runat="server" class="btngo" 
            Text="..."/>                        
                    &nbsp;<asp:CheckBox ID="cbForceNewPage" runat="server" Text="Force New Page" />
                <td> 
                </td>  
                <td>
                </td>              
            </tr>
            <br />
        <br />
        <asp:Button class="bitbtn btnpreview" runat="server" ID="btnPreview" Visible="false" Text="Preview"/>
        &nbsp;<asp:Button class="bitbtn btnprint" runat="server" ID="btnPrint" Text="Print"/>             
        &nbsp;<asp:Button class="bitbtn btnexcel" runat="server" ID="btnExport" Text="Export"/>        
        <br />
      <GridRpt:RptGridCtrl ID="ReportGrid" runat = "server" />                
      <asp:GridView ID="GridExport" runat="server" AutoGenerateColumns="True" 
                Visible="true" ShowFooter="False" GridLines="None" Height="79px" PageSize="20" Width="724px">
               <HeaderStyle CssClass="GridHeader" />
               <RowStyle CssClass="GridItem" Wrap="false" />
               <AlternatingRowStyle CssClass="GridAltItem" />
               <PagerStyle CssClass="GridPager" />
        </asp:GridView>         
      </div>
    <asp:Label ID="lbStatus" runat="server" ForeColor="Red" />    
    </form>
</body>
</html>
