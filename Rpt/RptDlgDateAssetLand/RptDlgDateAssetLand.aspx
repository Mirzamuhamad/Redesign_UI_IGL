<%@ Page Language="VB" AutoEventWireup="false" CodeFile="RptDlgDateAssetLand.aspx.vb" Inherits="RptDlgDateAssetLand_RptDlgDateAssetLand" %>

<%@ Register Assembly="GMDatePicker" Namespace="GrayMatterSoft" TagPrefix="cc1" %>
<%@ Register assembly="CrystalDecisions.Web, Version=10.5.3700.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>
<%@ Register Assembly="BasicFrame.WebControls.BasicDatePicker" Namespace="BasicFrame.WebControls"
    TagPrefix="BDP" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register TagPrefix="GridRpt" TagName="RptGridCtrl" Src="~/UserControl/ReportDoubleGrid.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />    
    <style type="text/css">
        .style1
        {
            width: 52px;
        }
    </style>
    
    <script type="text/javascript">         
        function openreport()
            {
                var wOpen;
                wOpen = window.open("../../Rpt/PrintForm.Aspx", '_blank');
                wOpen.moveTo(0, 0);
                wOpen.resizeTo(screen.width, screen.height);
                return false;  
             }             
        function openreport2()
            {
                var wOpen;
                wOpen = window.open("../../Rpt/PrintForm2.Aspx", '_blank');
                wOpen.moveTo(0, 0);
                wOpen.resizeTo(screen.width, screen.height);
                return false; 
            }    
       function openreport3()
            {
                var wOpen;
                wOpen = window.open("../../Rpt/PrintForm3.Aspx", '_blank');
                wOpen.moveTo(0, 0);
                wOpen.resizeTo(screen.width, screen.height);
                return false; 
            }         
      function openreport4()
            {
                var wOpen;
                wOpen = window.open("../../Rpt/PrintForm4.Aspx", '_blank');
                wOpen.moveTo(0, 0);
                wOpen.resizeTo(screen.width, screen.height);
                return false; 
            }          
      function openreport5()
            {
                var wOpen;
                wOpen = window.open("../../Rpt/PrintForm5.Aspx", '_blank');
                wOpen.moveTo(0, 0);
                wOpen.resizeTo(screen.width, screen.height);
                return false;
            }          
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePartialRendering="true" >
    </asp:ScriptManager>
    <div class="Content">
    <div class="H1"><asp:Label runat="server" ID="lblTitle"></asp:Label></div>
     <hr style="color:Blue" />
      <table>
        <tr>
            <td>Date</td>
                <td>:</td>
                <td>
                    <BDP:BasicDatePicker ID="tbStartDate" runat="server" DateFormat="dd MMM yyyy" Visible = "False" 
                        ShowNoneButton = "false" ReadOnly = "true" ButtonImageHeight="19px" ButtonImageWidth="20px" 
                        DisplayType="TextBoxAndImage" 
                        TextBoxStyle-CssClass="TextDate" ><TextBoxStyle CssClass="TextDate" /></BDP:BasicDatePicker>
                </td>
                <!-- <td>As</td> -->
                <td> 
                    <BDP:BasicDatePicker ID="tbEndDate" runat="server" DateFormat="dd MMM yyyy" 
                        ShowNoneButton = "false" ReadOnly = "true" ButtonImageHeight="19px" ButtonImageWidth="20px" 
                        DisplayType="TextBoxAndImage" 
                        TextBoxStyle-CssClass="TextDate"><TextBoxStyle CssClass="TextDate" /></BDP:BasicDatePicker>
                </td> 
                <td>&nbsp &nbsp &nbsp <asp:CheckBox ID="cbPrintValue" runat="server" Text="Print Have Value" />
                </td>                 
                <td>&nbsp &nbsp &nbsp <asp:CheckBox ID="cbForceNewPage" runat="server" Text="Force New Page" />
                </td>                                   
       </tr>        
       </table>    
       <table>
            <tr>
                <td>
                <fieldset runat="server" id="fsRpt" style="width:400px">
                <legend runat="server" id="lgRpt"><asp:Label runat="server" ID="lbReportType"></asp:Label></legend>
                <asp:RadioButtonList ID="RBType" Width="400px" runat="server" RepeatColumns="2">
                </asp:RadioButtonList>          
                </fieldset>
                </td>
                <td>
                <fieldset runat="server" id="fsRpt2" style="width:400px">
                <legend runat="server" id="lgRpt2"><asp:Label runat="server" ID="lbReportType2"></asp:Label></legend>
                <asp:RadioButtonList ID="RBType2" Width="400px" runat="server" RepeatColumns="2">
                </asp:RadioButtonList>          
                </fieldset>
                </td>
            </tr>                
        </table>
                        
        <%--<asp:UpdatePanel ID="UpdatePanel2" runat="server">        
          <ContentTemplate>                                      --%>
        <asp:Button class="bitbtn btnpreview" runat="server" ID="btnPreview" Visible="false" Text="Preview"/>
        <asp:Button class="bitbtn btnprint" runat="server" ID="btnPrint" Visible ="false"  Text="Print"/>
        <asp:Button class="bitbtn btnexcel" runat="server" ID="btnExport" Text="Export"/>        
        <br />
        <br />  
        <%--</ContentTemplate>                                      
        </asp:UpdatePanel>--%>
                        
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">        
          <ContentTemplate>                                      
                <GridRpt:RptGridCtrl ID="ReportGrid" runat = "server" />
                
           <asp:GridView ID="GridExport" runat="server" AutoGenerateColumns="True" 
                Visible="true" ShowFooter="False" GridLines="None" Height="79px" PageSize="20" Width="724px">
               <HeaderStyle CssClass="GridHeader" />
               <RowStyle CssClass="GridItem" Wrap="false" />
               <AlternatingRowStyle CssClass="GridAltItem" />
               <PagerStyle CssClass="GridPager" />
           </asp:GridView>     
          </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <br />    
    <asp:Label ID="lbStatus" runat="server" ForeColor="Red" />    
    
    </form>
    </body>
</html>
