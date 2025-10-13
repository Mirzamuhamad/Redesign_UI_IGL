<%@ Page Language="VB" AutoEventWireup="false" CodeFile="RptLeaveCard.aspx.vb" Inherits="Rpt_RptLeaveCard_RptLeaveCard" %>

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
    <div class="H1">Leave Card </div>
    <hr style="color:Blue" />        
        <table>
            <tr>
                <td>Period</td>
                <td>:</td>
                <td>
                    <asp:DropDownList CssClass="DropDownList" ID="ddlStartYear" runat="server">                              
                          </asp:DropDownList>
                </td>
                &nbsp
                <td>&nbsp;</td>
                &nbsp
                <td> 
                    &nbsp;</td>
                 <td>
                     &nbsp;</td>                     
            </tr>
        </table>   
        <br />   
        <table>
            <tr>
                <td>
                <fieldset style="width:310px">
                <legend>Sort By</legend>
                <asp:RadioButtonList ID="RBType" Width="300px" runat="server" RepeatColumns="2">
                <asp:ListItem Selected="True">Employee</asp:ListItem>
                <asp:ListItem>Department</asp:ListItem>                
                </asp:RadioButtonList>          
                </fieldset>
                </td>
            </tr>                
        </table>
        <br />   
        <asp:Button class="bitbtn btnpreview" runat="server" ID="btnPreview" Visible="false" Text="Preview" />   
        <asp:Button class="bitbtn btnprint" runat="server" ID="btnPrint" Text="Print" /> 
        <asp:Button class="bitbtn btnexcel" runat="server" ID="btnExport" Text="Export" Visible = "false" />                      
        <br />
        <GridRpt:RptGridCtrl ID="ReportGrid" runat = "server" />         
        <asp:Label ID="lbStatus" runat="server" ForeColor="Red" />
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
