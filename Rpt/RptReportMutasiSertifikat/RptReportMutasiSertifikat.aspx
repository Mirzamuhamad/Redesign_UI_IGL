<%@ Page Language="VB" AutoEventWireup="false" CodeFile="RptReportMutasiSertifikat.aspx.vb" Inherits="Rpt_RptReportMutasiSertifikat" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register TagPrefix="GridRpt" TagName="RptGridCtrl" Src="~/UserControl/ReportGrid.ascx" %>
<%@ Register Assembly="BasicFrame.WebControls.BasicDatePicker" Namespace="BasicFrame.WebControls"
    TagPrefix="BDP" %>
<%@ Register TagPrefix="GridRpt2" TagName="RptGridCtrl2" Src="~/UserControl/RangeControl5.ascx" %>    

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Report Aloksi</title>
    <script src="../../Function/OpenDlg.js" type="text/javascript"></script>
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
 <link href="https://fonts.googleapis.com/css2?family=Muli:wght@300;400;600&display=swap" rel="stylesheet">    
    
</head>
<body>
    <form id="form1" runat="server">
    <div class="Content">
    <div class="H1">Mutasi Dokumen Sertifkikat</div>
     <hr style="color:#BEBEBE" />     
        <br />
      <table>
        <tr>
            <td>Nomor Alokasi</td>
            <td>:</td>
            <td>
                    <asp:TextBox ID="tbProcess" runat="server" CssClass="TextBoxR" Width="225px" AutoPostBack =true  Enabled="True"/>
                    <asp:Button ID="btnProcess" runat="server" class="btngo" Text="..." Width="25px" />
                    </td>
            <%--<td>
                <asp:CheckBox ID="cbValue" runat="server" Text="Print Have Value" />                
            </td>
            <td>
                <asp:CheckBox ID="cbForce" runat="server" Text="Force New Page" />                
            </td>  --%>                
            <%--<td class="style4">
                    <asp:CheckBox ID="cbEmp" runat="server" Checked="false" Text="Employee No" AutoPostBack="true"/>&nbsp;:
                    <asp:TextBox ID="tbEmpNo1" runat="server" CssClass="TextBox" Width="82px" />
                    &nbsp;s/d
                    <asp:TextBox ID="tbEmpNo2" runat="server" CssClass="TextBox" Width="82px" />
                    </td>--%>
        </tr>          
        </table>      
            
      <br />
        <asp:Button class="bitbtn btnpreview" runat="server" ID="btnPreview" Visible="false" Text="Preview"/>
        &nbsp;<asp:Button class="bitbtn btnprint" runat="server" ID="btnPrint" Text="Print"/>             
        &nbsp;<asp:Button class="bitbtn btncopyto" runat="server" ID="btnExport" Text="Export"/>             
      <br />         
      <br />
      <%--<GridRpt:RptGridCtrl ID="ReportGrid" runat = "server" />--%>
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
