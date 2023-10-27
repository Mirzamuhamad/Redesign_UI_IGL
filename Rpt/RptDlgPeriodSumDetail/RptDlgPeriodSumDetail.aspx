<%@ Page Language="VB" AutoEventWireup="false" CodeFile="RptDlgPeriodSumDetail.aspx.vb" Inherits="Rpt_RptDlgPeriodSumDetail_ReportTemplate" %>

<%@ Register Assembly="GMDatePicker" Namespace="GrayMatterSoft" TagPrefix="cc1" %>
<%@ Register assembly="CrystalDecisions.Web, Version=10.5.3700.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>
<%@ Register Assembly="BasicFrame.WebControls.BasicDatePicker" Namespace="BasicFrame.WebControls"
    TagPrefix="BDP" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register TagPrefix="GridRpt" TagName="RptGridCtrl" Src="~/UserControl/ReportDoubleGrid.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
    <script src="../../Function/OpenDlg.js" type="text/javascript"></script>
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />    
    <style type="text/css">
        .style1
        {
            width: 52px;
        }
    </style>
    
    <script type="text/javascript">
        function period(str,txtbox)
        {
            var tahun2;
            var x;
            var y;
            var nilai = document.getElementById("tbPeriode").value;
            
            
            if (str=="0"){
                str="1";
                nilai.value=str;
            }
            
            //alert(str);
            
            var range = document.getElementById("ddlPeriod").value;
            var nilairange=(range * nilai) -1;
            //alert(range);
            //alert(nilairange);
            var tahun =document.getElementById('ddlStartYear').value;
            var bulan =document.getElementById('ddlStartMonth').value;
            //x=bulan;
            //y=nilairange;
            //alert(x);
            //alert(y);
            var bulan2=parseInt(bulan)+parseInt(nilairange);
            //alert(tahun);
            //alert(bulan);
            //alert(bulan2);
            
            if (bulan2 > 24){
                bulan2=parseInt(bulan2)-24;
                tahun2=parseInt(tahun)+2;
            }else if (bulan2 > 12){
                bulan2=parseInt(bulan2)-12;
                tahun2=parseInt(tahun)+1;
            }else{
                tahun2=tahun;
            }
            //alert(tahun2);
            //alert(bulan2);
            document.getElementById("ddlEndYear").value=tahun2;
            document.getElementById("ddlEndMonth").value=bulan2;
        } 
        
        function period2()
        {
            var tahun2;
            var val;
            var str = document.getElementById("tbPeriode").value;
            var nilai = document.getElementById("tbPeriode").value;
                        
            if (str=="0"){
                val="1";
                nilai.value=val;
            }
            
            var range = document.getElementById("ddlPeriod").value;
            var nilairange=(range * nilai) -1;
            var tahun =document.getElementById('ddlStartYear').value;
            var bulan =document.getElementById('ddlStartMonth').value;
            var bulan2=parseInt(bulan)+parseInt(nilairange);
                        
            if (bulan2 > 24){
                bulan2=parseInt(bulan2)-24;
                tahun2=parseInt(tahun)+2;
            }else if (bulan2 > 12){
                bulan2=parseInt(bulan2)-12;
                tahun2=parseInt(tahun)+1;
            }else{
                tahun2=tahun;
            }
            
            document.getElementById("ddlEndYear").value=tahun2;
            document.getElementById("ddlEndMonth").value=bulan2;
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
            <td>Range Period</td>
            <td>:</td>
            <td><asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlPeriod" AutoPostBack="true" >
                      <asp:ListItem Value="1" Selected="True">Monthly</asp:ListItem>
                      <asp:ListItem Value="3">3 Month</asp:ListItem>
                      <asp:ListItem Value="6">Semester</asp:ListItem>
                      <asp:ListItem Value="12">Yearly</asp:ListItem>                      
                      </asp:DropDownList>
                      <asp:TextBox ID="tbPeriod" runat="server" CssClass="TextBox" Text = "1" AutoPostBack="true" Width="30px" Visible="True"></asp:TextBox> 
                      <%--<input id="tbPeriode" type="text" class="TextBox" style="width:30px" runat="server" onkeyup="period(this.value,'tbPeriode')" value="1" /> x Period        --%>
                  </td>
            </tr>
           <tr>
            <td>Period</td>
            <td>:</td>
            <td><asp:DropDownList ID="ddlStartYear" runat="server" CssClass="DropDownList" AutoPostBack="true"/> 
                <asp:DropDownList ID="ddlStartMonth" runat="server" CssClass="DropDownList" AutoPostBack="true"/>
                s/d         
                <asp:DropDownList ID="ddlEndYear" runat="server" CssClass="DropDownList" Enabled="false"/> 
                <asp:DropDownList ID="ddlEndMonth" runat="server" CssClass="DropDownList" Enabled="false"/>                 
            </td>   
            <td>&nbsp &nbsp &nbsp<asp:CheckBox ID="cbPrintValue" runat="server" Text="Print Have Value" />  
                </td>                 
                <td>&nbsp &nbsp &nbsp<asp:CheckBox ID="cbForceNewPage" runat="server" Text="Force New Page" /> 
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
        <asp:Button class="bitbtn btnprint" runat="server" ID="btnPrint" Text="Print"/>
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
