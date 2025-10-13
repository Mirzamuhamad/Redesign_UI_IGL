<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TrWOPlanBatch.aspx.vb" Inherits="Transaction_TrWOPLanBatch_TrWOPLanBatch" %>

<%@ Register Assembly="BasicFrame.WebControls.BasicDatePicker" Namespace="BasicFrame.WebControls"
    TagPrefix="BDP" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>

    <script src="../../Function/OpenDlg.js" type="text/javascript"></script>

    <script src="../../Function/Function.JS" type="text/javascript"></script>
    <script type="text/javascript">
     function OpenPopup() {
         window.open("../../SeaDlg.Aspx", "List", "scrollbars=yes,resizable=no,width=500,height=400");
         return false;
     }
     function openprintdlg() {
         var wOpens;
         wOpens = window.open("../../Rpt/PrintForm.Aspx", "List", "scrollbars=yes,resizable=yes,width=500,height=400");
         wOpens.moveTo(0, 0);
         wOpens.resizeTo(screen.width, screen.height);
     }

     function OpenPopup() {
         window.open("../../SearchDlg.Aspx", "List", "scrollbars=yes,resizable=no,width=500,height=400");
         return false;
     }    
    </script>
    <script type="text/javascript">         
        function setdigit(nStr, digit)
        {
        try
        {
        var TNstr = parseFloat(nStr);        
        if ( parseFloat(digit) >= 0) 
        {     
           TNstr = TNstr.toFixed(digit);                
        } 
        nStr = TNstr;        
	    nStr += '';
	    x = nStr.split('.');
	    x1 = x[0];
	    x2 = x.length > 1 ? '.' + x[1] : '';
	    var rgx = /(\d+)(\d{3})/;
	    while (rgx.test(x1)) {
		    x1 = x1.replace(rgx, '$1' + ',' + '$2');
	    }
	    return x1 + x2;
	    }catch (err){
            alert(err.description);
          }  
        }
       
        function setformatdt()
        {
         try
         {  
            var _QtyOutput = parseFloat(document.getElementById("tbQtyE").value.replace(/\$|\,/g,""));
            var _QtyWO = parseFloat(document.getElementById("tbQtyWO").value.replace(/\$|\,/g,""));
            var _QtyGood = parseFloat(document.getElementById("tbQtyGood").value.replace(/\$|\,/g,""));
//            var _QtyRepair = parseFloat(document.getElementById("tbQtyRepair").value.replace(/\$|\,/g,""));
            var _QtyReject = parseFloat(document.getElementById("tbQtyReject").value.replace(/\$|\,/g,""));
            
            
         
            document.getElementById("tbQtyE").value = setdigit(_QtyWO,'<%=VIEWSTATE("DigitQty")%>');
            document.getElementById("tbQtyOutput").value = setdigit(_QtyOutput,'<%=VIEWSTATE("DigitQty")%>');
            document.getElementById("tbQtyGood").value = setdigit(_QtyGood,'<%=VIEWSTATE("DigitQty")%>');
//            document.getElementById("tbQtyRepair").value = setdigit(_QtyRepair,'<%=VIEWSTATE("DigitQty")%>');
            document.getElementById("tbQtyReject").value = setdigit(_QtyReject,'<%=VIEWSTATE("DigitQty")%>');            
            //alert("test 2");                                                
            
        }catch (err){
            alert(err.description);
          }      
        }   
        
   
    </script>

    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
 <link href="https://fonts.googleapis.com/css2?family=Muli:wght@300;400;600&display=swap" rel="stylesheet">
</head>
<body>
    <form id="form1" runat="server">
    <div class="Content">
        <div class="H1">
            WO Bottom Batch</div>
        <hr style="color: Blue" />
        <asp:Panel runat="server" ID="PnlHd">
            <table>
                <tr>
                    <td style="width: 100px; text-align: right">
                        Quick Search :
                    </td>
                    <%--TransNmbr, TransDate, STATUS, FgReport, UserType, UserCode, UserName, Attn, Remark--%>
                    <td>
                        <asp:TextBox CssClass="TextBox" runat="server" ID="tbFilter" />
                        <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlField">
                            <asp:ListItem Selected="True" Value="TransNmbr">Trans No</asp:ListItem>
                            <asp:ListItem Value="dbo.FormatDate(TransDate)">Trans Date</asp:ListItem>
                            <asp:ListItem>Status</asp:ListItem>
                            <asp:ListItem Value="ReffType">Reff Type</asp:ListItem>
                            <asp:ListItem Value="Reference">Reference</asp:ListItem>
                            <asp:ListItem Value="DivisiName">Divisi</asp:ListItem>
                            <asp:ListItem Value="MonthNo">Rotasi No</asp:ListItem>
                            <asp:ListItem Value="WorkByName">Work By</asp:ListItem>
                            <asp:ListItem Value="Person">Person</asp:ListItem>
                            <asp:ListItem Value="JobPlant">Job Name</asp:ListItem>
                            <asp:ListItem Value="JobName">Job Name</asp:ListItem>
                            <asp:ListItem Value="CIPCode">CIP</asp:ListItem>
                            <asp:ListItem Value="CIPName">CIP Name</asp:ListItem>
                            <asp:ListItem Value="Supplier">Kontraktor</asp:ListItem>
                            <asp:ListItem Value="SuppName">Kontraktor Name</asp:ListItem>
                            <asp:ListItem Value="QtyBlok">Qty Batch</asp:ListItem>
                            <asp:ListItem Value="Qty">Qty </asp:ListItem>
                            <asp:ListItem Value="Unit">Unit</asp:ListItem>
                            <asp:ListItem Value="EstStartWeek">Est Start Week</asp:ListItem>
                            <asp:ListItem Value="EstEndWeek">Est End Week</asp:ListItem>
                            <asp:ListItem Value="QtyWeek">Qty Week</asp:ListItem>
                            <asp:ListItem Value="Remark">Remark</asp:ListItem>
                        </asp:DropDownList>
                        <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlRange">
                        </asp:DropDownList>
                        <asp:Button class="bitbtn btnsearch" runat="server" ID="btnSearch" Text="Search" />
                        <asp:Button class="bitbtn btngo" runat="server" ID="btnExpand" Text="..." />
                    </td>
                    <td>
                        <asp:LinkButton ID="LbAdvSearch" runat="server" Text="Advanced Search" />
                        &nbsp;&nbsp; Show Records :
                        <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlShowRecord" AutoPostBack="true">
                            <asp:ListItem Selected="True" Value="15">Choose One</asp:ListItem>
                            <asp:ListItem Value="20">20</asp:ListItem>
                            <asp:ListItem Value="30">30</asp:ListItem>
                            <asp:ListItem Value="40">40</asp:ListItem>
                            <asp:ListItem Value="50">50</asp:ListItem>
                            <asp:ListItem Value="100">100</asp:ListItem>
                        </asp:DropDownList>
                        Rows&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:Label ID="Label17" runat="server" Text="Reference : " />
                        <asp:LinkButton ID="lbCount" runat="server" Font-Size="Small" 
                            ForeColor="#FF6600" Text="X" />
                        <asp:Label ID="Label" runat="server" Text=" record(s)" />
                        &nbsp;</td>
                </tr>
            </table>
            <asp:Panel runat="server" ID="pnlSearch" Visible="false">
                <table>
                    <tr>
                        <td style="width: 100px; text-align: right">
                            <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlNotasi">
                                <asp:ListItem Selected="true" Text="OR" Value="OR"></asp:ListItem>
                                <asp:ListItem Text="AND" Value="AND"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:TextBox runat="server" CssClass="TextBox" ID="tbfilter2" />
                            <asp:DropDownList runat="server" CssClass="DropDownList" ID="ddlField2">
                                <asp:ListItem Selected="True" Value="TransNmbr">LHP No</asp:ListItem>
                                <asp:ListItem Value="dbo.FormatDate(TransDate)">Input Date</asp:ListItem>
                                <asp:ListItem>Status</asp:ListItem>
                                <asp:ListItem Value="ReffType">Reff Type</asp:ListItem>
                            <asp:ListItem Value="Reference">Reference</asp:ListItem>
                            <asp:ListItem Value="DivisiName">Divisi</asp:ListItem>
                            <asp:ListItem Value="MonthNo">Rotasi No</asp:ListItem>
                            <asp:ListItem Value="WorkByName">Work By</asp:ListItem>
                            <asp:ListItem Value="Person">Person</asp:ListItem>
                            <asp:ListItem Value="JobPlant">Job Name</asp:ListItem>
                            <asp:ListItem Value="JobName">Job Name</asp:ListItem>
                            <asp:ListItem Value="CIPCode">CIP</asp:ListItem>
                            <asp:ListItem Value="CIPName">CIP Name</asp:ListItem>
                            <asp:ListItem Value="Supplier">Kontraktor</asp:ListItem>
                            <asp:ListItem Value="SuppName">Kontraktor Name</asp:ListItem>
                            <asp:ListItem Value="QtyBlok">Qty Batch</asp:ListItem>
                            <asp:ListItem Value="Qty">Qty </asp:ListItem>
                            <asp:ListItem Value="Unit">Unit</asp:ListItem>
                            <asp:ListItem Value="EstStartWeek">Est Start Week</asp:ListItem>
                            <asp:ListItem Value="EstEndWeek">Est End Week</asp:ListItem>
                            <asp:ListItem Value="QtyWeek">Qty Week</asp:ListItem>
                            <asp:ListItem Value="Remark">Remark</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <br />
            <asp:Button class="bitbtn btnadd" runat="server" ID="BtnAdd" Text="Add" />
            &nbsp &nbsp &nbsp
            <asp:DropDownList CssClass="DropDownList" ID="ddlCommand" runat="server" Visible="false" />
            <asp:Button class="btngo" runat="server" ID="BtnGo" Text="G" Visible="false" />
            <br />
            <div style="border: 0px  solid; width: 100%; height: 100%; overflow: auto;">
                <asp:GridView ID="GridView1" runat="server" AllowPaging="True" AllowSorting="True"
                    CssClass="Grid" AutoGenerateColumns="False">
                    <HeaderStyle CssClass="GridHeader"></HeaderStyle>
                    <RowStyle CssClass="GridItem" Wrap="false" />
                    <AlternatingRowStyle CssClass="GridAltItem" />
                    <PagerStyle CssClass="GridPager" />
                    <Columns>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:CheckBox ID="cbSelectHd" runat="server" AutoPostBack="true" OnCheckedChanged="cbSelectHd_CheckedChanged" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="cbSelect" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-Width="110">
                            <ItemTemplate>
                                <asp:DropDownList CssClass="DropDownList" ID="ddl" runat="server">
                                    <asp:ListItem Selected="True" Text="View" />
                                    <asp:ListItem Text="Edit" />
                                    
                                    <asp:ListItem Text="Print" />
                                </asp:DropDownList>
                                <asp:Button class="btngo" runat="server" ID="BtnGo" Text="G" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>"
                                    CommandName="Go" />
                            </ItemTemplate>
                            <HeaderStyle Width="110px" />
                        </asp:TemplateField>
                        <asp:BoundField DataField="TransNmbr" HeaderStyle-Width="120px" SortExpression="Nmbr"
                            HeaderText="Trans No">
                            <HeaderStyle Width="120px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Status" HeaderText="Status"></asp:BoundField>
                        <asp:BoundField DataField="TransDate" DataFormatString="{0:dd MMM yyyy}" HtmlEncode="true"
                            HeaderStyle-Width="80px" SortExpression="TransDate" 
                            HeaderText="Trans Date">
                            <HeaderStyle Width="80px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Divisi" HeaderStyle-Width="200px" SortExpression="Divisi"
                            HeaderText="Divisi">
                            <HeaderStyle Width="200px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="DivisiName" HeaderStyle-Width="80px" SortExpression="DivisiName"
                            HeaderText="Division Name">
                            <HeaderStyle Width="80px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="ReffType" HeaderStyle-Width="80px" SortExpression="ReffType"
                            HeaderText="Reff Type">
                            <HeaderStyle Width="80px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Reference" HeaderStyle-Width="200px" 
                            SortExpression="Reference" HeaderText="Reference">
                            <HeaderStyle Width="200px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="MonthNo" HeaderStyle-Width="80px" SortExpression="MonthNo"
                            HeaderText="Rotasi No">
                            <HeaderStyle Width="80px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="WorkByName" HeaderStyle-Width="250px" SortExpression="WorkByName"
                            HeaderText="Work By">
                            <HeaderStyle Width="250px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Person" HeaderStyle-Width="80px" SortExpression="Person"
                            HeaderText="Person">
                            <HeaderStyle Width="80px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="JobPlant" SortExpression="JobPlant"
                            HeaderText="Job Code"></asp:BoundField>
                        <asp:BoundField DataField="JobPlantName" HeaderText="Job Name" 
                            SortExpression="JobPlantName" />
                        <asp:BoundField DataField="CIP" HeaderText="CIP Code" 
                            SortExpression="CIP" />
                        <asp:BoundField DataField="CIPName" HeaderText="CIP Name" 
                            SortExpression="CIPName" />
                        <asp:BoundField DataField="Supplier" HeaderText="Kontraktor Code" 
                            SortExpression="Supplier" />
                        <asp:BoundField DataField="SupplierName" HeaderText="Kontraktor Name" 
                            SortExpression="SupplierName" />
                        <asp:BoundField DataField="QtyBlok" HeaderText="Qty Batch" 
                            SortExpression="QtyBlok" />
                        <asp:BoundField DataField="Qty" HeaderText="Qty" SortExpression="Qty" />
                        <asp:BoundField DataField="Unit" HeaderText="Unit" SortExpression="Unit" />
                        <asp:BoundField DataField="EstStartWeek" HeaderText="Est. Start Week" 
                            SortExpression="EstStartWeek" />
                        <asp:BoundField DataField="EstEndWeek" HeaderText="Est. End Week" 
                            SortExpression="EstEndWeek" />
                        <asp:BoundField DataField="QtyWeek" HeaderText="Qty Week" 
                            SortExpression="QtyWeek" />
                        <asp:BoundField DataField="Remark" HeaderStyle-Width="250px" 
                            HeaderText="Remark" SortExpression="Remark">
                            <HeaderStyle Width="250px" />
                        </asp:BoundField>
                    </Columns>
                </asp:GridView>
            </div>
            <asp:Panel runat="server" ID="pnlNav" Visible="false">
                <asp:Button class="bitbtn btnadd" runat="server" ID="btnAdd2" Text="Add" />
                &nbsp &nbsp &nbsp
                <asp:DropDownList ID="ddlCommand2" CssClass="DropDownList" runat="server" />
                <asp:Button class="btngo" runat="server" ID="btnGo2" Text="G" />
            </asp:Panel>
        </asp:Panel>
        <asp:Panel runat="server" ID="pnlInput" Visible="false">
            <table>
                <tr>
                    <td>
                        Trans No
                    </td>
                    <td>
                        :
                    </td>
                    <td>
                        <asp:TextBox CssClass="TextBoxR" runat="server" ID="tbCode" Width="150px" Enabled="False" />
                    </td>
                    <td>
                        Date</td>
                    <td>
                        :</td>
                    <td>
                        <BDP:BasicDatePicker ID="tbDate" runat="server" DateFormat="dd MMM yyyy" 
                        ReadOnly = "true" ValidationGroup="Input"
                        ButtonImageHeight="19px" ButtonImageWidth="20px" 
                        DisplayType="TextBoxAndImage" 
                        TextBoxStyle-CssClass="TextDate" AutoPostBack="True" 
                    ShowNoneButton="False"><TextBoxStyle CssClass="TextDate" /></BDP:BasicDatePicker>   </td>
                    <td>
                        &nbsp;</td>
                    <td>
                        &nbsp;</td>
                    <td>
                        &nbsp;</td>
                </tr>
                <tr>
                    <%--<td>LHP Type</td>
            <td>:</td>
            <td><asp:DropDownList ValidationGroup="Input" CssClass="DropDownList" ID="ddlLHPType" runat="server" >
                        <asp:ListItem Selected="True">PE</asp:ListItem>
                        <asp:ListItem>TISSUE</asp:ListItem>
                    </asp:DropDownList> 
            </td>   --%>
                    <td>
                        Divisi</td>
                    <td>
                        :
                    </td>
                    <td colspan="7">
                        <asp:DropDownList ValidationGroup="Input" CssClass="DropDownList" ID="ddlDivisi"
                            runat="server" AutoPostBack="true" Height="16px" Width="222px" />
                        <asp:Label ID="Label11" runat="server" ForeColor="#FF3300" Text="*"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        Reference Type</td>
                    <td>
                        :
                    </td>
                    <td>
                        <asp:DropDownList ValidationGroup="Input" CssClass="DropDownList" AutoPostBack = "true"
                            ID="ddlReffType" runat="server" Height="16px" Width="149px" >
                            <asp:ListItem Selected="True">Schedule</asp:ListItem>
                            <asp:ListItem>Other</asp:ListItem>
                        </asp:DropDownList>
                        <asp:Label ID="Label12" runat="server" ForeColor="#FF3300" Text="*"></asp:Label>
                    </td>
                    <td>
                        Reference</td>
                    <td>
                        :</td>
                    <td>
                        <asp:TextBox ID="tbReffNo" runat="server" CssClass="TextBoxR" MaxLength="30" 
                             Width="150px" Enabled ="False" />
                        <asp:Button ID="btnReff" runat="server" class="btngo" Text="..." 
                            ValidationGroup="Input" />
                    </td>
                    <td>
                        Rotasi No</td>
                    <td>
                        :</td>
                    <td>
                        <asp:TextBox ID="tbRotasi" runat="server" CssClass="TextBox" MaxLength="30" 
                            ValidationGroup="Input" Width="56px" />
                    </td>
                </tr>
                <tr>
                    <td>
                        Work By</td>
                    <td>
                        :</td>
                    <td>
                        <asp:DropDownList ID="ddlWorkBy" runat="server" AutoPostBack="true" 
                            CssClass="DropDownList" ValidationGroup="Input" Height="16px" 
                            Width="150px" />
                    </td>
                    <td>
                        Person</td>
                    <td>
                        :</td>
                    <td>
                        <asp:TextBox ID="tbPerson" runat="server" CssClass="TextBox" MaxLength="30" 
                            ValidationGroup="Input" Width="76px" AutoPostBack="True" />
                    </td>
                    <td>
                        &nbsp;</td>
                    <td>
                        &nbsp;</td>
                    <td>
                        &nbsp;</td>
                </tr>
                <tr>
                    <td>
                        Job</td>
                    <td>
                        :</td>
                    <td colspan="7">
                        <asp:TextBox ID="tbJob" runat="server" CssClass="TextBox" MaxLength="30" AutoPostBack ="true"
                            ValidationGroup="Input" Width="150px" />
                        <asp:TextBox ID="tbJobName" runat="server" CssClass="TextBoxR" 
                            Enabled="false" MaxLength="60" Width="225px" />
                        <asp:Button ID="btnJob" runat="server" class="btngo" Text="..." 
                            ValidationGroup="Input" />
                        <asp:Label ID="Label13" runat="server" ForeColor="#FF3300" Text="*"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        CIP</td>
                    <td>
                        :</td>
                    <td colspan="7">
                        <asp:TextBox ID="tbCIP" runat="server" CssClass="TextBox" MaxLength="30" AutoPostBack ="true"
                            ValidationGroup="Input" Width="150px" />
                        <asp:TextBox ID="tbCIPName" runat="server" CssClass="TextBoxR" 
                            Enabled="false" MaxLength="60" Width="225px" />
                        <asp:Button ID="btnCIP" runat="server" class="btngo" Text="..." />
                        <asp:Label ID="Label14" runat="server" ForeColor="#FF3300" Text="*"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        Kontraktor</td>
                    <td>
                        :</td>
                    <td colspan="7">
                        <asp:TextBox ID="tbKontraktor" runat="server" CssClass="TextBox" MaxLength="30" AutoPostBack ="true"
                            ValidationGroup="Input" Width="150px" />
                        <asp:TextBox ID="tbKontraktorName" runat="server" CssClass="TextBoxR" 
                            Enabled="false" MaxLength="60" Width="225px" />
                        <asp:Button ID="btnKontraktor" runat="server" class="btngo" Text="..." />
                    </td>
                </tr>
                <tr>
                    <td>
                        Qty Batch</td>
                    <td>
                        :</td>
                    <td>
                        <asp:TextBox ID="tbQtyBlok" runat="server" CssClass="TextBoxR" Enabled="False" 
                            Width="65px" />
                    </td>
                    <td>
                        Qty</td>
                    <td>
                        :</td>
                    <td>
                        <asp:TextBox ID="tbQty" runat="server" CssClass="TextBoxR" 
                            Enabled="False" Width="65px" />
                    </td>
                    <td>
                        Unit</td>
                    <td>
                        :</td>
                    <td>
                        <asp:TextBox ID="tbUnit" runat="server" CssClass="TextBoxR" 
                            Enabled="False" Width="65px" />
                    </td>
                </tr>
                <tr>
                    <td>
                        Est Statrt Week</td>
                    <td>
                        :</td>
                    <td>
                        <asp:TextBox ID="tbStart" runat="server" CssClass="TextBoxR" MaxLength="30" 
                            Enabled = "false" Width="150px" />
                        <asp:Button ID="btnStart" runat="server" class="btngo" Text="..." 
                            ValidationGroup="Input" />
                        <asp:Label ID="Label15" runat="server" ForeColor="#FF3300" Text="*"></asp:Label>
                    </td>
                    <td>
                        Est End Week</td>
                    <td>
                        :</td>
                    <td>
                        <asp:TextBox ID="tbEnd" runat="server" CssClass="TextBoxR" MaxLength="30" 
                            Enabled = "false"  Width="150px" />
                        <asp:Button ID="btnEnd" runat="server" class="btngo" Text="..." 
                            ValidationGroup="Input" />
                        <asp:Label ID="Label16" runat="server" ForeColor="#FF3300" Text="*"></asp:Label>
                    </td>
                    <td>
                        Qty Week</td>
                    <td>
                        :</td>
                    <td>
                        <asp:TextBox ID="tbQtyWeek" runat="server" CssClass="TextBoxR" Enabled="False" 
                            MaxLength="30" Width="56px" />
                    </td>
                </tr>
                <tr>
                    <td>
                        Remark
                    </td>
                    <td>
                        :
                    </td>
                    <td colspan="7">
                        <asp:TextBox ID="tbRemark" runat="server" CssClass="TextBoxMulti" 
                            MaxLength="255" TextMode="MultiLine" ValidationGroup="Input" Width="350px" />
                        &nbsp; &nbsp; &nbsp;
                        <asp:Button ID="btnGetDt" runat="server" class="btngo" Text="Get Detail" 
                            Visible="True" Width="80px" />
                    </td>
                </tr>
            </table>
            <br />
            <hr style="color: Blue" />
            <asp:Menu ID="Menu1" runat="server" CssClass="Menu" StaticMenuItemStyle-CssClass="MenuItem"
                StaticSelectedStyle-CssClass="MenuSelect" Orientation="Horizontal" ItemWrap="False"
                StaticEnableDefaultPopOutImage="False">
                <StaticSelectedStyle CssClass="MenuSelect" />
                <StaticMenuItemStyle CssClass="MenuItem" />
                <Items>
                    <asp:MenuItem Text="Detail Divisi / Batch" Value="0" ></asp:MenuItem>
                    <asp:MenuItem Text="Detail Machine" Value="1" ></asp:MenuItem>
                    <asp:MenuItem Text="Detail Equipment" Value="2" ></asp:MenuItem>
                    <asp:MenuItem Text="Detail Material" Value="3" ></asp:MenuItem>
                </Items>
            </asp:Menu>
            <br />
             
            <asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex="0">
                
                <asp:View ID="Tab2" runat="server">
                    
                    <asp:Panel ID="pnlDt" runat="server">
                    
                        <asp:Button class="bitbtn btnadd" runat="server" ID="btnAddDt" Text="Add" ValidationGroup="Input" />
                       
                        <div style="border: 0px  solid; width: 100%; height: 100%; overflow: auto;">
                            <asp:GridView ID="GridDt" runat="server" AutoGenerateColumns="False" ShowFooter="True">
                                <HeaderStyle CssClass="GridHeader" />
                                <RowStyle CssClass="GridItem" Wrap="false" />
                                <AlternatingRowStyle CssClass="GridAltItem" />
                                <PagerStyle CssClass="GridPager" />
                                 
                                <Columns>
                              
                                    <asp:TemplateField HeaderText="Action" ><ItemTemplate><asp:Button ID="btnEdit" runat="server" class="bitbtndt btnedit" 
                                                CommandName="Edit" Text="Edit" />
                                    <asp:Button ID="btnDelete" runat="server" class="bitbtndt btndelete" 
                                                CommandName="Delete" 
                                                OnClientClick="return confirm('Sure to delete this data?');" Text="Delete" />                                                
                                                </ItemTemplate>
                                    <EditItemTemplate></EditItemTemplate></asp:TemplateField>
                                    <asp:TemplateField HeaderText="Action" ><ItemTemplate>
                                    <asp:Button ID="btnView" runat="server" class="bitbtndt btndetail" 
                                                CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" 
                                                CommandName="ViewA" Text="Machine" />
                                    <asp:Button ID="btnViewE" runat="server" class="bitbtndt btndetail" 
                                                CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" 
                                                CommandName="ViewE" Text="Equipment" />             
                                   <asp:Button ID="btnViewM" runat="server" class="bitbtndt btndetail" 
                                                CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" 
                                                CommandName="ViewM" Text="Material" />
                                                </ItemTemplate>
                                    <EditItemTemplate></EditItemTemplate></asp:TemplateField>
                                    
                                    <asp:BoundField DataField="Type" HeaderStyle-Width="150px" 
                                        HeaderText="Type" SortExpression="Type"><HeaderStyle Width="50px" /></asp:BoundField>
                                    <asp:BoundField DataField="DivisiBlok" HeaderStyle-Width="150px" 
                                        HeaderText="Batch" SortExpression="DivisiBlok"><HeaderStyle Width="150px" /></asp:BoundField>
                                    <asp:BoundField DataField="DivisiBlokName" HeaderStyle-Width="200px" 
                                        HeaderText="Batch Name" SortExpression="Batch Name"><HeaderStyle Width="200px" /></asp:BoundField>
                                    <asp:BoundField DataField="StatusTanam" HeaderText="Status Tanam Code" 
                                        SortExpression="StatusTanam" />
                                    <asp:BoundField DataField="StatusTanamName" HeaderText="Status Tanam Name" SortExpression="StatusTanamName" />
                                    <asp:BoundField DataField="Percentage" HeaderStyle-Width="60px" 
                                        HeaderText="Percentage" DataFormatString="{0:#,##0.##}" 
                                        ItemStyle-HorizontalAlign="Right" SortExpression="Percentage" ><HeaderStyle Width="60px" /><ItemStyle HorizontalAlign="Right" /></asp:BoundField>
                                    <asp:BoundField DataField="QtyTotal" HeaderStyle-Width="150px" 
                                        HeaderText="Qty Total" SortExpression="QtyTotal" ><HeaderStyle Width="150px" /></asp:BoundField>
                                    <asp:BoundField DataField="Qty" DataFormatString="{0:#,##0.##}" 
                                        HeaderStyle-Width="100px" HeaderText="Qty WO" SortExpression="Qty"><HeaderStyle Width="100px" /></asp:BoundField>
                                    <asp:BoundField DataField="NormaHK" DataFormatString="{0:#,##0.##}" 
                                        HeaderStyle-Width="150px" HeaderText="Capacity (HK)" SortExpression="NormaHK"><HeaderStyle Width="150px" /></asp:BoundField>
                                    <asp:BoundField DataField="TargetHK" DataFormatString="{0:#,##0.##}" 
                                        HeaderText="Target HK" SortExpression="TargetHK" />
                                    <asp:BoundField DataField="WorkDay" DataFormatString="{0:#,##0.##}" 
                                        HeaderText="Work Day" SortExpression="WorkDay" />
                                    <asp:BoundField DataField="StartDate" DataFormatString="{0:dd MMM yyyy}" 
                                        HeaderText="Start Date" SortExpression="StartDate" />
                                    <asp:BoundField DataField="EndDate" DataFormatString="{0:dd MMM yyyy}" 
                                        HeaderText="End Date" SortExpression="EndDate" />
                                    
                                </Columns>
                            </asp:GridView>
                        </div>
                        <asp:Button class="bitbtn btnadd" runat="server" ID="btnAddDtKe2" Text="Add" ValidationGroup="Input" />
                      
                    </asp:Panel>
                    <asp:Panel runat="server" ID="pnlEditDt" Visible="false">
                        <table>
                            <%--<tr>
                        <td>WO No</td>
                        <td>:</td>
                        <td colspan="4"><asp:Label ID="lbWONo" runat="server" Text="WO No" />
                        </td>           
                    </tr>      --%>
                            <tr>
                                <td>
                                    Type / Batch</td>
                                <td>
                                    :
                                </td>
                                <td colspan="4">
                                      <asp:DropDownList ValidationGroup="Input" CssClass="DropDownList" AutoPostBack = "true"
                                        ID="ddlType" runat="server" Height="16px" Width="149px" >
                                        <asp:ListItem>Divisi</asp:ListItem>
                                        <asp:ListItem>Batch</asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:TextBox CssClass="TextBoxR" Enabled ="false"  runat="server" ID="tbBatch"
                                        MaxLength="20" AutoPostBack="true" />
                                    <asp:TextBox CssClass="TextBoxR" runat="server" ID="tbBatchName" Enabled="false"
                                        MaxLength="60" Width="225px" />
                                    
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Status Tanam</td>
                                <td>
                                    :</td>
                                <td colspan="4">
                                   <asp:TextBox CssClass="TextBoxR" runat="server" ID="tbStatusTanam" Enabled="false"
                                        MaxLength="60" Width="250px" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <td>
                                    :
                                </td>
                                <td colspan="4">
                                    <table>
                                        <tr style="background-color: Silver; text-align: center">
                                            <td>
                                                Percentage</td>
                                            <td>
                                                Qty Total</td>
                                            <td>
                                                Qty WO</td>
                                            <td>
                                                Kapasitas (HK)</td>
                                            <td>
                                                Target HK</td>
                                            <td>
                                                Work Day</td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="tbPercentage" runat="server" AutoPostBack="true" CssClass="TextBox" Width="65px" />
                                            </td>
                                            <td>
                                                <asp:TextBox ID="tbTotal" runat="server" CssClass="TextBox" Width="65px" />
                                            </td>
                                            <td>
                                                <asp:TextBox ID="tbQtyWO" runat="server" CssClass="TextBoxR" Enabled="false" Width="65px" />
                                            </td>
                                            <td>
                                                <asp:TextBox ID="tbQtyCapacity" runat="server" CssClass="TextBox" AutoPostBack = "true"
                                                    Width="70px" />
                                            </td>
                                            <td>
                                                <asp:TextBox ID="tbQtyTarget" runat="server" CssClass="TextBoxR" Enabled="false" Width="65px" />
                                            </td>
                                            <td>
                                                <asp:TextBox ID="tbQtyWorkDay" runat="server" CssClass="TextBoxR" Enabled="false" Width="65px" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Start Date
                                </td>
                                <td>
                                    :
                                </td>
                                <td>
                                    <BDP:BasicDatePicker ID="tbStartDate" runat="server" ButtonImageHeight="19px" 
                                        ButtonImageWidth="20px" DateFormat="dd MMM yyyy" DisplayType="TextBoxAndImage" 
                                        Enabled="false" ReadOnly="true" ShowNoneButton="False" 
                                        TextBoxStyle-CssClass="TextDate" ValidationGroup="Input"><TextBoxStyle CssClass="TextDate" /></BDP:BasicDatePicker>
                                </td>
                                <td>
                                    End Date</td>
                                <td>
                                    :</td>
                                <td>
                                    <BDP:BasicDatePicker ID="tbEndDate" runat="server" ButtonImageHeight="19px" 
                                        ButtonImageWidth="20px" DateFormat="dd MMM yyyy" DisplayType="TextBoxAndImage" 
                                        Enabled="false" ReadOnly="true" ShowNoneButton="False" 
                                        TextBoxStyle-CssClass="TextDate" ValidationGroup="Input"><TextBoxStyle CssClass="TextDate" /></BDP:BasicDatePicker>
                                </td>
                            </tr>
                           
                        </table>
                        <br />
                        <asp:Button ID="btnSaveDt" runat="server" class="bitbtndt btnsave" Text="Save" />
                        <asp:Button ID="btnCancelDt" runat="server" class="bitbtndt btncancel" Text="Cancel" />
                    </asp:Panel>
                </asp:View>
                <asp:View ID="Tab3" runat="server">
                <asp:Label ID="LblTypeMA" runat="server" ForeColor="Blue" Text="Type :" ></asp:Label>
                     <asp:Label ID="LblTypeM" runat="server" ForeColor="Blue" Text=""></asp:Label><br />
                                <asp:Label ID="LblBlocMA" runat="server" ForeColor="Blue" Text="Batch :"></asp:Label>
                                 <asp:Label ID="LblBatchM" runat="server" ForeColor="Blue" Text=""></asp:Label>
                                <asp:Label ID="LblBlocNameMA" runat="server" ForeColor="Blue" Text="Batch Name :"></asp:Label>
                                 <asp:Label ID="LblBatchNameM" runat="server"  ForeColor="Blue" Text=""></asp:Label><br />
                        <asp:Button ID="btnBackDt2" runat="server" class="bitbtn btnback" Text="Back" ValidationGroup="Input" />     
                    <asp:Panel ID="pnlDt2" runat="server">
                        <asp:Button class="bitbtn btnadd" runat="server" ID="btnAddDt2" Text="Add" ValidationGroup="Input" />
                         
                        <asp:Button class="btngo" runat="server" ID="btnGetWaste" Text="Get Machine" 
                            ValidationGroup="Input" Visible="True" Width="80px" />    
                        <div style="border: 0px  solid; width: 100%; height: 100%; overflow: auto;">
                            <asp:GridView ID="GridDt2" runat="server" AutoGenerateColumns="False" 
                                ShowFooter="True">
                                <HeaderStyle CssClass="GridHeader" />
                                <RowStyle CssClass="GridItem" Wrap="false" />
                                <AlternatingRowStyle CssClass="GridAltItem" />
                                <PagerStyle CssClass="GridPager" />
                                <Columns>
                                    <asp:TemplateField HeaderText="Action"><ItemTemplate><asp:Button ID="btnEdit" runat="server" class="bitbtndt btnedit" 
                                                CommandName="Edit" Text="Edit" /><asp:Button ID="btnDelete" runat="server" class="bitbtndt btndelete" 
                                                CommandName="Delete" 
                                                OnClientClick="return confirm('Sure to delete this data?');" Text="Delete" /></ItemTemplate><EditItemTemplate><asp:Button ID="btnUpdate" runat="server" class="bitbtndt btnsave" 
                                                CommandName="Update" Text="Save" /><asp:Button ID="btnCancel" runat="server" class="bitbtndt btncancel" 
                                                CommandName="Cancel" Text="Cancel" /></EditItemTemplate></asp:TemplateField>
                                    <asp:BoundField DataField="Machine" HeaderText="Machine" 
                                        SortExpression="Machine" ><HeaderStyle Width="150px" /></asp:BoundField>
                                    <asp:BoundField DataField="MachineName" HeaderStyle-Width="150px" 
                                        HeaderText="Machine Name" SortExpression="MachineName" ><HeaderStyle Width="200px" /></asp:BoundField>
                                    <asp:BoundField DataField="EstHour" HeaderStyle-Width="200px" 
                                        HeaderText="Est. Duration Hour" SortExpression="EstHour" ><HeaderStyle Width="60px" /><ItemStyle HorizontalAlign="Right" /></asp:BoundField>
                                </Columns>
                            </asp:GridView>
                        </div>
                        <asp:Button ID="btnAddDt2ke2" runat="server" class="bitbtn btnadd" Text="Add" 
                            ValidationGroup="Input" />
                        <asp:Button ID="btnBackDt2ke2" runat="server" class="bitbtndt btnback" Text="Back"
                            ValidationGroup="Input" />             
                        
                    </asp:Panel>
                    <asp:Panel runat="server" ID="pnlEditDt2" Visible="false">
                        <table>
                            <tr>
                                <td>
                                    Machine</td>
                                <td>
                                    :
                                </td>
                                <td>
                                    <asp:TextBox ID="tbMachine" runat="server" AutoPostBack="true" CssClass="TextBox" 
                                        MaxLength="20" ValidationGroup="Input" />
                                    <asp:TextBox ID="tbMachineName" runat="server" CssClass="TextBox" 
                                        Enabled="false" MaxLength="60" Width="225px" />
                                    <asp:Button ID="btnMachine" runat="server" Class="btngo" Text="..." 
                                        ValidationGroup="Input" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Est. Duration Hour</td>
                                <td>
                                    :
                                </td>
                                <td>
                                    
                                    <asp:TextBox ID="tbQtyDuration" runat="server" CssClass="TextBox" 
                                        Width="65px" />
                                    
                                </td>
                            </tr>
                        </table>
                        <br />
                        <asp:Button ID="btnSaveDt2" runat="server" class="bitbtndt btnsave" Text="Save" />
                        <asp:Button ID="btnCancelDt2" runat="server" class="bitbtndt btncancel" Text="Cancel" />
                    </asp:Panel>
                </asp:View>
                <asp:View ID="Tab4" runat="server">
                <asp:Label ID="Label1" runat="server" ForeColor="Blue" Text="Type :"></asp:Label>
                     <asp:Label ID="LblTypeE" runat="server"  ForeColor="Blue" Text=""></asp:Label><br />
                                <asp:Label ID="Label3" runat="server" ForeColor="Blue" Text="Batch :"></asp:Label>
                                 <asp:Label ID="LblBatchE" runat="server" ForeColor="Blue" Text=""></asp:Label>
                                <asp:Label ID="Label5" runat="server" ForeColor="Blue" Text="Batch Name :"></asp:Label>
                                 <asp:Label ID="LblBatchNameE" runat="server" ForeColor="Blue" Text=""></asp:Label><br />
                    <asp:Button ID="BtnBackDt3" runat="server" class="bitbtndt btnback" Text="Back"
                            ValidationGroup="Input" />  
                    <asp:Panel ID="pnlDt3" runat="server">
                        <asp:Button class="bitbtn btnadd" runat="server" ID="btnAddDt3" Text="Add" ValidationGroup="Input" />
                         
                        <div style="border: 0px  solid; width: 100%; height: 100%; overflow: auto;">
                        
                            <asp:GridView ID="GridDt3" runat="server" AutoGenerateColumns="False" ShowFooter="True">
                                <HeaderStyle CssClass="GridHeader" />
                                <RowStyle CssClass="GridItem" Wrap="false" />
                                <AlternatingRowStyle CssClass="GridAltItem" />
                                <PagerStyle CssClass="GridPager" />
                                <Columns>
                                    <asp:TemplateField HeaderText="Action"><ItemTemplate><asp:Button ID="btnEdit" runat="server" class="bitbtndt btnedit" Text="Edit" CommandName="Edit" /><asp:Button ID="btnDelete" runat="server" class="bitbtndt btndelete" Text="Delete"
                                                OnClientClick="return confirm('Sure to delete this data?');" CommandName="Delete" /></ItemTemplate><EditItemTemplate><asp:Button ID="btnUpdate" runat="server" class="bitbtndt btnsave" Text="Save" CommandName="Update" /><asp:Button ID="btnCancel" runat="server" class="bitbtndt btncancel" Text="Cancel"
                                                CommandName="Cancel" /></EditItemTemplate></asp:TemplateField>
                                    <asp:BoundField DataField="Equipment" HeaderStyle-Width="150px" 
                                        HeaderText="Equipment" SortExpression="Equipment" ><HeaderStyle Width="150px" /></asp:BoundField>
                                    <asp:BoundField DataField="EquipmentName" HeaderStyle-Width="200px" 
                                        HeaderText="Equipment Name" SortExpression="EquipmentName" ><HeaderStyle Width="200px" /></asp:BoundField>
                                    <asp:BoundField DataField="Qty" HeaderStyle-Width="80px" HeaderText="Qty" 
                                        DataFormatString="{0:#,##0.##}" SortExpression="Qty" ><HeaderStyle Width="100px" /></asp:BoundField>
                                    <asp:BoundField DataField="Unit" HeaderStyle-Width="30px" HeaderText="Unit" 
                                        SortExpression="Unit" />
                                 
                                </Columns>
                            </asp:GridView>
                        </div>
                        <asp:Button class="bitbtn btnadd" runat="server" ID="btnAddDt3ke2" Text="Add" ValidationGroup="Input" />
                        <asp:Button class="bitbtn btnback" runat="server" ID="BtnBackDt3ke2" Text="Back"  />
                    </asp:Panel>
                    <asp:Panel runat="server" ID="pnlEditDt3" Visible="false">
                        <table>
                            <tr>
                                <td>
                                    Equipment</td>
                                <td>
                                    :
                                </td>
                                <td>
                                    <asp:TextBox ID="tbEquip" runat="server" AutoPostBack="true" 
                                        CssClass="TextBox" MaxLength="20" ValidationGroup="Input" />
                                    <asp:TextBox ID="tbEquipName" runat="server" CssClass="TextBoxR" 
                                        Enabled="false" MaxLength="60" Width="225px" />
                                    <asp:Button ID="btnEquip" runat="server" Class="btngo" Text="..." 
                                        ValidationGroup="Input" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Qty</td>
                                <td>
                                    :
                                </td>
                                <td>
                                    
                                    <asp:TextBox ID="tbQtyE" runat="server" 
                                        CssClass="TextBox" ValidationGroup="Input" Width="65px" />
                                    <asp:TextBox ID="tbUnitE" runat="server"  
                                        CssClass="TextBoxR" Enabled ="false"  Width="65px" />
                                    
                                </td>
                            </tr>
                        </table>
                        <br />
                        <asp:Button ID="btnSaveDt3" runat="server" class="bitbtndt btnsave" Text="Save" />
                        <asp:Button ID="btnCancelDt3" runat="server" class="bitbtndt btncancel" Text="Cancel" />
                    </asp:Panel>
                </asp:View>
                
                <asp:View ID="Tab5" runat="server">
                  <asp:Label ID="Label2" runat="server" ForeColor="Blue" Text="Type :"></asp:Label>
                     <asp:Label ID="LblTypeMT" runat="server" ForeColor="Blue" Text=""></asp:Label><br />
                                <asp:Label ID="Label6" runat="server" ForeColor="Blue" Text="Batch :"></asp:Label>
                                 <asp:Label ID="LblBatchMT" runat="server" ForeColor="Blue" Text=""></asp:Label>
                                <asp:Label ID="Label8" runat="server" ForeColor="Blue" Text="Batch Name :"></asp:Label>
                                 <asp:Label ID="LblBatchNameMT" runat="server" ForeColor="Blue" Text=""></asp:Label><br />
                    <asp:Button ID="BtnBackDt4" runat="server" class="bitbtndt btnback" Text="Back"
                            ValidationGroup="Input" />  
                            
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:Button ID="btnGetData" runat="server" class="bitbtndt btnsave" 
                        Text="Get Material" Visible="True" Width="82px" />
                            
                    <asp:Panel runat="server" ID="PnlDt4">
                    
                        <asp:Button class="bitbtn btnadd" runat="server" ID="btnAdddt4" Text="Add" Visible="false"
                            ValidationGroup="Input" />
                        
                                
                        &nbsp;&nbsp;&nbsp;
                                                
                                
                        <div style="border: 0px  solid; width: 100%; height: 100%; overflow: auto;">
                            <asp:GridView ID="GridDt4" runat="server" AutoGenerateColumns="False" 
                                ShowFooter="True">
                                <HeaderStyle CssClass="GridHeader" />
                                <RowStyle CssClass="GridItem" Wrap="false" />
                                <AlternatingRowStyle CssClass="GridAltItem" />
                                <PagerStyle CssClass="GridPager" />
                                <Columns>
                                    <%--<asp:BoundField DataField="QtyRepair" HeaderStyle-Width="60px" HeaderText="Qty Repair" />   --%>
                                    <%--<asp:BoundField DataField="MachineHour" HeaderStyle-Width="100px" HeaderText="Machine Hour" />   
                            <asp:BoundField DataField="ManPower" HeaderStyle-Width="100px" HeaderText="Man Power" />   --%>
                                    <asp:TemplateField HeaderText="Action"><ItemTemplate><asp:Button ID="btnEdit" runat="server" class="bitbtndt btnedit" 
                                                CommandName="Edit" Text="Edit" /><asp:Button ID="btnDelete" runat="server" class="bitbtndt btndelete" 
                                                CommandName="Delete" 
                                                OnClientClick="return confirm('Sure to delete this data?');" Text="Delete" /></ItemTemplate></asp:TemplateField>
                                    
                                    <asp:BoundField DataField="Material" HeaderStyle-Width="100px" 
                                        HeaderText="Material" SortExpression="Material"><HeaderStyle Width="100px" /></asp:BoundField>
                                        
                                    <asp:BoundField DataField="MaterialName" HeaderStyle-Width="50px" 
                                        HeaderText="Material Name" SortExpression="MaterialName"><HeaderStyle Width="50px" /></asp:BoundField>
                                    <asp:BoundField DataField="QtyTotal" DataFormatString="{0:#,##0.##}" 
                                        HeaderStyle-Width="100px" HeaderText="Qty Total" SortExpression="QtyTotal" />
                                    <asp:BoundField DataField="Qty" DataFormatString="{0:#,##0.##}" 
                                        HeaderStyle-Width="60px" HeaderText="Qty Material" 
                                        ItemStyle-HorizontalAlign="Right" SortExpression="Qty"><ItemStyle HorizontalAlign="Right" /></asp:BoundField>
                                    <asp:BoundField DataField="Unit" HeaderStyle-Width="70px" HeaderText="Unit"><HeaderStyle Width="70px" /></asp:BoundField>
                                    <asp:BoundField DataField="Remark" HeaderStyle-Width="100px" 
                                        HeaderText="Remark" ><HeaderStyle Width="100px" /></asp:BoundField>
                                    <asp:BoundField DataField="QtyDosis" DataFormatString="{0:#,##0.##}" 
                                        HeaderStyle-Width="150px" HeaderText="Dosis/Unit" SortExpression="QtyDosis"><HeaderStyle Width="100px" /></asp:BoundField>
                                    <asp:BoundField DataField="QtyPokok" DataFormatString="{0:#,##0.##}" 
                                        HeaderStyle-Width="200px" HeaderText="Dosis / Pokok" SortExpression="QtyPokok"><HeaderStyle Width="60px" /></asp:BoundField>
                                </Columns>
                            </asp:GridView>
                        </div>
                        <asp:Button class="bitbtn btnadd" runat="server" ID="btnAddDt4ke2" Text="Add" Visible="false"
                            ValidationGroup="Input" />
                        <asp:Button class="bitbtn btnback" runat="server" ID="btnbackDt4ke2" Text="Back" />    
                    </asp:Panel>
                    <asp:Panel runat="server" ID="pnlEditDt4" Visible="false">
                        <table>
                            <tr>
                                <td>
                                    Material</td>
                                <td>
                                    :
                                </td>
                                <td>
                                    <asp:TextBox CssClass="TextBox" ID="tbMaterial" runat="server" MaxLength="30" Width="150px"
                                        Enabled="False" />
                                    <asp:TextBox ID="tbMaterialName" runat="server" CssClass="TextBox" 
                                        Enabled="false" MaxLength="60" Width="225px" />
                                    <asp:Button ID="btnMaterial" runat="server" class="btngo" Text="..." />
                                    <asp:Label ID="Label9" runat="server" ForeColor="#FF3300" Text="*"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Qty
                                </td>
                                <td>
                                    :
                                </td>
                                <td>
                                    <table>
                                        <tr style="background-color: Silver; text-align: center">
                                            <td>
                                                Total
                                            </td>
                                            <td>
                                                Material</td>
                                            <%--<td>Repair</td>--%>
                                            <td>
                                                Unit
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:TextBox CssClass="TextBoxR" runat="server" ID="tbQtyTotal" Width="65px" />
                                            </td>
                                            <td>
                                                <asp:TextBox CssClass="TextBoxR" runat="server" ID="tbQtyMaterial" 
                                                    Width="65px" />
                                            </td>
                                            <%--<td><asp:TextBox CssClass="TextBox" runat="server" ID="tbQtyRepair" Width="65px"/></td>--%>
                                            <td>
                                                <asp:TextBox ID="tbUnitM" runat="server" CssClass="TextBoxR" Width="65px" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Dosis / Unit
                                </td>
                                <td>
                                    :
                                </td>
                                <td>
                                    <asp:TextBox ID="tbQtyDosis" runat="server" CssClass="TextBoxR" Width="65px" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Dosis / Pokok
                                </td>
                                <td>
                                    :
                                </td>
                                <td>
                                    <asp:TextBox ID="tbQtyPokok" runat="server" CssClass="TextBoxR" Width="65px" />
                                </td>
                            </tr>
                            <%--tr>
                        <td>Warehouse</td>
                        <td>:</td>
                        <td colspan="4">
                            <asp:TextBox CssClass="TextBox" ValidationGroup="Input" runat="server" ID="tbWrhsCode" MaxLength="20" AutoPostBack="true" />
                            <asp:TextBox CssClass="TextBox" runat="server" ID="tbWrhsName" Enabled="false" MaxLength="60" Width="225px"/>
                            <asp:Button Class="btngo" ID="btnWrhs" Text="..." runat="server" ValidationGroup="Input" />
                         </td>
                    </tr>  --%>
                            <%--<tr>
                        <td>Machine</td>
                        <td>:</td>
                        <td colspan="4">
                            <asp:TextBox CssClass="TextBox" ValidationGroup="Input" runat="server" ID="tbMachineCode" MaxLength="20" AutoPostBack="true" />
                            <asp:TextBox CssClass="TextBox" runat="server" ID="tbMachineName" Enabled="false" MaxLength="60" Width="225px"/>
                            <asp:Button Class="btngo" ID="btnMachine" Text="..." runat="server" ValidationGroup="Input" />
                         </td>
                    </tr> --%>
                            <%-- <tr>
                        <td>Machine Work Hour</td>
                        <td>:</td>
                        <td><asp:TextBox CssClass="TextBox" runat="server" MaxLength="50" ID="tbMachineHour"  Enabled= False Width="100px" /></td>
                        <td>Used Man Power</td>
                        <td>:</td>
                        <td><asp:TextBox CssClass="TextBox" runat="server" MaxLength="50" ID="tbManPower"  ValidationGroup="Input" Width="100px" /></td>
                    </tr>--%>
                            <tr>
                                <td>
                                    Remark
                                </td>
                                <td>
                                    :
                                </td>
                                <td>
                                    <asp:TextBox ID="tbRemarkDt" runat="server" CssClass="TextBoxMulti" 
                                        TextMode="MultiLine" ValidationGroup="Input" Width="269px" />
                                </td>
                            </tr>
                        </table>
                        <br />
                        <asp:Button ID="btnSaveDt4" runat="server" class="bitbtndt btnsave" Text="Save" />
                        <asp:Button ID="btnCancelDt4" runat="server" class="bitbtndt btncancel" Text="Cancel" />
                    </asp:Panel>
                </asp:View>
            </asp:MultiView>
            <br />
            <asp:Button ID="btnSaveAll" runat="server" class="bitbtndt btnsavenew" Text="Save & New"
                ValidationGroup="Input" Width="90" />
            <asp:Button ID="btnSaveTrans" runat="server" class="bitbtndt btnsave" Text="Save"
                ValidationGroup="Input" />
            <asp:Button ID="btnBack" runat="server" class="bitbtndt btncancel" Text="Cancel"
                ValidationGroup="Input" />
            <asp:Button ID="btnHome" runat="server" class="bitbtndt btnback" Text="Home" />
        </asp:Panel>
        
    </div>
    <%--<asp:BoundField DataField="MachineHour" HeaderStyle-Width="100px" HeaderText="Machine Hour" />   
                            <asp:BoundField DataField="ManPower" HeaderStyle-Width="100px" HeaderText="Man Power" />   --%>
    <asp:Label runat="server" ID="lbStatus" ForeColor="Red" />
    </form>
</body>
</html>
