<%@ Page Language="VB" AutoEventWireup="false" CodeFile="RptUserRight.aspx.vb" Inherits="Rpt_RptUserRight_RptUserRight" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register TagPrefix="GridRpt" TagName="RptGridCtrl" Src="~/UserControl/ReportGrid.ascx" %>

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
    <div class="H1">User Right Report</div>
    <hr style="color:Blue" />                
        <br />
        <fieldset style="width:120px">
            <legend>Order By</legend>
                    <asp:RadioButtonList  ID="rbGroup" runat="server" RepeatColumns="2" >
                        <asp:ListItem Selected="True" Value="UserId">User Id</asp:ListItem>
                        <asp:ListItem>Menu</asp:ListItem>
                    </asp:RadioButtonList>
        </fieldset>  
        <fieldset style="width:150px">
            <legend>Status User</legend>
            <asp:RadioButtonList ID="rbStatus" Width="250px" runat="server" RepeatColumns="3">
                <asp:ListItem Selected="True">All</asp:ListItem>
                <asp:ListItem>Active</asp:ListItem>
                <asp:ListItem>Non Active</asp:ListItem>
            </asp:RadioButtonList>
         </fieldset>    
        <asp:Button class="bitbtn btnpreview" runat="server" ID="btnPreview" Visible="false" Text="Preview"/>
        <asp:Button class="bitbtn btnprint" runat="server" ID="btnPrint" Text="Print"/>               
        <asp:Button class="bitbtn btnexcel" runat="server" ID="btnExport" Text="Export"/>        
        <br />
        <GridRpt:RptGridCtrl ID="ReportGrid" runat = "server" />         
        <asp:Label ID="lbStatus" runat="server" ForeColor="Red" />
        <br />
           <asp:GridView ID="GridExport" runat="server" AutoGenerateColumns="True" 
                Visible="true" ShowFooter="False" GridLines="None" Height="79px" PageSize="20" Width="724px">
               <HeaderStyle CssClass="GridHeader" />
               <RowStyle CssClass="GridItem" Wrap="false" />
               <AlternatingRowStyle CssClass="GridAltItem" />
               <PagerStyle CssClass="GridPager" />
           </asp:GridView>
    </div>
    </form>
</body>
</html>
