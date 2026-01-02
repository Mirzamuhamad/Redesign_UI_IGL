<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TrWOResult.aspx.vb" Inherits="Transaction_TrWOResult_TrWOResult" %>

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
    // function openprintdlg() {
     //    var wOpens;
     //    wOpens = window.open("../../Rpt/PrintForm.Aspx", "List", "scrollbars=yes,resizable=yes,width=500,height=400");
    //     wOpens.moveTo(0, 0);
    //     wOpens.resizeTo(screen.width, screen.height);
    // }
	
	function openprintdlg3ds() {
            var wOpen;
            wOpen = window.open("../../Rpt/PrintForm3.Aspx", "List", "scrollbars=yes,resizable=yes,width=500,height=400");
            wOpen.moveTo(0, 0);
            wOpen.resizeTo(screen.width, screen.height);
            return false;
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
    <style type="text/css">
        .style1
        {
            width: 70px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div class="Content">
        <div class="H1">
            Laporan Kerja Mandor ( LKM )</div>
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
                            <asp:ListItem Value="WrhsName">Warehouse</asp:ListItem>
                            <asp:ListItem Value="Division">Division</asp:ListItem>
                            <asp:ListItem Value="Supplier">Kontraktor</asp:ListItem>
                            <asp:ListItem Value="SupplierName">Kontraktor Name</asp:ListItem>
                            <asp:ListItem Value="WorkBy">Work By</asp:ListItem>
                            <asp:ListItem Value="WorkByName">Work By Name</asp:ListItem>
                            <asp:ListItem Value="FgBorongan">Borongan</asp:ListItem>
                            <asp:ListItem Value="CheckBy">Mandor</asp:ListItem>
                            <asp:ListItem Value="CheckByName">Mandor Name</asp:ListItem>
                            <asp:ListItem Value="AcknowledBy">Asisten</asp:ListItem>
                            <asp:ListItem Value="AcknowledByName">Asisten Name</asp:ListItem>
                            <asp:ListItem Value="Remark">Remark</asp:ListItem>
                            
                        </asp:DropDownList>
                        <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlRange">
                        </asp:DropDownList>
                        <asp:Button class="bitbtn btnsearch" runat="server" ID="btnSearch" Text="Search" />
                        <asp:Button class="bitbtn btngo" runat="server" ID="btnExpand" Text="..." />
                    </td>
                    <td>
                        <asp:LinkButton ID="LbAdvSearch" runat="server" Text="Advanced Search" />
                        Show Records :
                        <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlShowRecord" AutoPostBack="true">
                            <asp:ListItem Selected="True" Value="15">Choose One</asp:ListItem>
                            <asp:ListItem Value="20">20</asp:ListItem>
                            <asp:ListItem Value="30">30</asp:ListItem>
                            <asp:ListItem Value="40">40</asp:ListItem>
                            <asp:ListItem Value="50">50</asp:ListItem>
                            <asp:ListItem Value="100">100</asp:ListItem>
                        </asp:DropDownList>
                        Rows  &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:Label ID="Label17" runat="server" Text="WO Outstanding : " />
                        <asp:LinkButton ID="lbCount" runat="server" Font-Size="Small" 
                            ForeColor="#FF6600" Text="X" />
                        <asp:Label ID="Label" runat="server" Text=" record(s)" />
                    </td>
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
                                <asp:ListItem Value="WrhsName">Warehouse</asp:ListItem>
                                <asp:ListItem Value="Division">Division</asp:ListItem>
                                <asp:ListItem Value="Supplier">Kontraktor</asp:ListItem>
                                <asp:ListItem Value="SupplierName">Kontraktor Name</asp:ListItem>
                                <asp:ListItem Value="WorkBy">Work By</asp:ListItem>
                                <asp:ListItem Value="WorkByName">Work By Name</asp:ListItem>
                                <asp:ListItem Value="FgBorongan">Borongan</asp:ListItem>
                                <asp:ListItem Value="CheckBy">Mandor</asp:ListItem>
                                <asp:ListItem Value="CheckByName">Mandor Name</asp:ListItem>
                                <asp:ListItem Value="AcknowledBy">Asisten</asp:ListItem>
                                <asp:ListItem Value="AcknowledByName">Asisten Name</asp:ListItem>
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
									<asp:ListItem Text="Print CPWO" />
									<asp:ListItem Text="Print CPWO Blank" />
									
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
                        <asp:BoundField DataField="WrhsName" HeaderStyle-Width="80px" SortExpression="WrhsName"
                            HeaderText="warehouse Name">
                            <HeaderStyle Width="80px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Division" HeaderStyle-Width="200px" SortExpression="Division"
                            HeaderText="Division">
                            <HeaderStyle Width="200px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="DivisionName" HeaderStyle-Width="80px" SortExpression="DivisionName"
                            HeaderText="Division Name">
                            <HeaderStyle Width="80px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Supplier" HeaderStyle-Width="80px" SortExpression="Supplier"
                            HeaderText="Kontraktor">
                            <HeaderStyle Width="80px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="SupplierName" HeaderStyle-Width="200px" 
                            SortExpression="SupplierName" HeaderText="Kontraktor Name">
                            <HeaderStyle Width="200px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="WorkBy" HeaderStyle-Width="80px" SortExpression="WorkBy"
                            HeaderText="Work By">
                            <HeaderStyle Width="80px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="WorkByName" HeaderStyle-Width="250px" SortExpression="WorkByName"
                            HeaderText="Work By Name">
                            <HeaderStyle Width="250px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="FgBorongan" HeaderStyle-Width="80px" SortExpression="FgBorongan"
                            HeaderText="Job Borongan">
                            <HeaderStyle Width="80px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="CheckBy" SortExpression="CheckBy"
                            HeaderText="Mandor"></asp:BoundField>
                        <asp:BoundField DataField="CheckByName" HeaderText="Mandor Name" 
                            SortExpression="CheckByName" />
                        <asp:BoundField DataField="AcknowledBy" HeaderText="Asisten Code" 
                            SortExpression="AcknowledBy" />
                        <asp:BoundField DataField="AcknowledByName" HeaderText="Asisten Name" 
                            SortExpression="AcknowledByName" />
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
                        Warehouse</td>
                    <td>
                        :
                    </td>
                    <td>
                        <asp:DropDownList ValidationGroup="Input" CssClass="DropDownList" AutoPostBack = "true"
                            ID="ddlWrhs" runat="server" Height="16px" Width="149px" >
                        </asp:DropDownList>
                        <asp:Label ID="lblFgWrhsFgSubled" runat="server" ForeColor="#FF3300" Text=""></asp:Label>
                        <asp:Label ID="Label12" runat="server" ForeColor="#FF3300" Text="*"></asp:Label>
                    </td>
                    <td>
                        Kontraktor</td>
                    <td>
                        :</td>
                    <td colspan="4">
                        <asp:TextBox ID="tbKontraktor" runat="server" CssClass="TextBox" MaxLength="30" AutoPostBack ="true"
                            ValidationGroup="Input" Width="150px" />
                        <asp:TextBox ID="tbKontraktorName" runat="server" CssClass="TextBoxR" 
                            Enabled="false" MaxLength="60" Width="225px" />
                        <asp:Button ID="btnKontraktor" runat="server" class="btngo" Text="..." />
                    </td>
                    <td>
                      </td>
                    <td>
                       </td>
                    <td>
                       
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
                        Job Borongan</td>
                    <td>
                        :</td>
                    <td>
                       <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlFgBorongan" Width="50px" ValidationGroup="Input"> 
                    <asp:ListItem Selected="True">Y</asp:ListItem>
                    <asp:ListItem>N</asp:ListItem>                    
                </asp:DropDownList> 
                              
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
                        Mandor</td>
                    <td>
                        :</td>
                    <td colspan="7">
                        <asp:TextBox ID="tbMandor" runat="server" CssClass="TextBox" MaxLength="30" AutoPostBack ="true"
                            ValidationGroup="Input" Width="150px" />
                        <asp:TextBox ID="tbMandorName" runat="server" CssClass="TextBoxR" 
                            Enabled="false" MaxLength="60" Width="225px" />
                        <asp:Button ID="btnMandor" runat="server" class="btngo" Text="..." 
                            ValidationGroup="Input" />
                        <asp:Label ID="Label13" runat="server" ForeColor="#FF3300" Text="*"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        Asisten</td>
                    <td>
                        :</td>
                    <td colspan="7">
                        <asp:TextBox ID="tbAsisten" runat="server" CssClass="TextBox" MaxLength="30" AutoPostBack ="true"
                            ValidationGroup="Input" Width="150px" />
                        <asp:TextBox ID="tbAsistenName" runat="server" CssClass="TextBoxR" 
                            Enabled="false" MaxLength="60" Width="225px" />
                        <asp:Button ID="btnAsisten" runat="server" class="btngo" Text="..." />
                        <asp:Label ID="Label14" runat="server" ForeColor="#FF3300" Text="*"></asp:Label>
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
                StaticEnableDefaultPopOutImage="False" Visible ="false">
                <StaticSelectedStyle CssClass="MenuSelect" />
                <StaticMenuItemStyle CssClass="MenuItem" />
                <Items>
                    <asp:MenuItem Text="Divisi / Batch" Value="0" ></asp:MenuItem>
                    <asp:MenuItem Text="Material" Value="3" ></asp:MenuItem>
                    <asp:MenuItem Text="Machine" Value="1" ></asp:MenuItem>
                    <asp:MenuItem Text="Operator" Value="2" ></asp:MenuItem>
                    
                </Items>
            </asp:Menu>
            <br />
             
            <asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex="0">
                
                <asp:View ID="Tab2" runat="server">
                    
                    <asp:Panel ID="pnlDt" runat="server">
                    
                        <asp:Button class="bitbtn btnadd" runat="server" ID="btnAddDt" Text="Add" ValidationGroup="Input" />
						&nbsp;<asp:TextBox ID="LblCode" runat="server" Visible ="False" 
                            CssClass="TextBox" MaxLength="30" ValidationGroup="Input" Width="150px" />
                       
                        <div style="border: 0px  solid; width: 100%; height: 100%; overflow: auto;">
                            <asp:GridView ID="GridDt" runat="server" AutoGenerateColumns="False" 
                                ShowFooter="True" style="margin-right: 449px" Width="1965px">
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
                                     <asp:Button ID="btnViewM" runat="server" class="bitbtndt btndetail" 
                                                CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" 
                                                CommandName="ViewM" Text="Material" />
                                    <asp:Button ID="btnView" runat="server" class="bitbtndt btndetail" 
                                                CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" 
                                                CommandName="ViewA" Text="Machine" />
                                    <asp:Button ID="btnViewE" runat="server" class="bitbtndt btndetail" 
                                                CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" 
                                                CommandName="ViewE" Text="Operator" />             
                                              </ItemTemplate>
                                    
                                    <EditItemTemplate></EditItemTemplate></asp:TemplateField>
                                    <asp:BoundField DataField="WONo" HeaderStyle-Width="100px" 
                                        HeaderText="WO No" SortExpression="WONo"><HeaderStyle Width="100px" /></asp:BoundField>
                                        
                                    <asp:BoundField DataField="Type" HeaderStyle-Width="50px" 
                                        HeaderText="Type" SortExpression="Type"><HeaderStyle Width="50px" /></asp:BoundField>
                                        
                                    <asp:BoundField DataField="DivisiBlok" HeaderStyle-Width="60px" 
                                        HeaderText="Divisi Blok" SortExpression="DivisiBlok"><HeaderStyle Width="60px" /></asp:BoundField>
                                    
                                    <asp:BoundField DataField="DivisiBlokName" HeaderStyle-Width="60px" 
                                        HeaderText="Blok Name" SortExpression="Batch Name"><HeaderStyle Width="60px" /></asp:BoundField>
                                    
                                    <asp:BoundField DataField="JobPlant" HeaderText="Job Plant" HeaderStyle-Width="60px" 
                                        SortExpression="JobPlant" />
                                    
                                    <asp:BoundField DataField="Job_Name" HeaderText="Job Plant Name" HeaderStyle-Width="200px"
                                        SortExpression="Job_Name" />
                                    
                                    <asp:BoundField DataField="QtyWo" DataFormatString="{0:#,##0.##}" 
                                        HeaderStyle-Width="70px" HeaderText="Qty WO" SortExpression="QtyWo"><HeaderStyle Width="70px" /></asp:BoundField>
                                    
                                    <asp:BoundField DataField="QtyDone" DataFormatString="{0:#,##0.##}" 
                                        HeaderStyle-Width="70px" HeaderText="Qty Done" SortExpression="QtyDone"><HeaderStyle Width="70px" /></asp:BoundField>
                                    
                                    <asp:BoundField DataField="Qty" DataFormatString="{0:#,##0.##}" 
                                        HeaderStyle-Width="70px" HeaderText="Qty LKM" SortExpression="Qty"><HeaderStyle Width="70px" /></asp:BoundField>
                                    
                                    <asp:BoundField DataField="QtySDHI" DataFormatString="{0:#,##0.##}" 
                                        HeaderStyle-Width="70px" HeaderText="Qty SDHI" SortExpression="QtySDHI"><HeaderStyle Width="70px" /></asp:BoundField>
                                    
                                    <asp:BoundField DataField="Capacity"  DataFormatString="{0:#,##0.##}" HeaderStyle-Width="50px" 
                                        HeaderText="Capacity" SortExpression="Capacity" ><HeaderStyle Width="50px" /></asp:BoundField>
                                    
                                    <asp:BoundField DataField="TKTotal" DataFormatString="{0:#,##0.##}" 
                                        HeaderStyle-Width="50px" HeaderText="TK WO" SortExpression="TKTotal"><HeaderStyle Width="50px" /></asp:BoundField>
                                    
                                    <asp:BoundField DataField="TKDone" DataFormatString="{0:#,##0.##}" 
                                        HeaderStyle-Width="60px" HeaderText="TK Done" SortExpression="QtyDone"><HeaderStyle Width="60px" /></asp:BoundField>
                                    
                                    <asp:BoundField DataField="Person" DataFormatString="{0:#,##0.##}" 
                                        HeaderStyle-Width="50px" HeaderText="TK LKM" SortExpression="Person"><HeaderStyle Width="50px" /></asp:BoundField>
                                    
                                    <asp:BoundField DataField="TKSDHI" DataFormatString="{0:#,##0.##}" 
                                        HeaderStyle-Width="50px" HeaderText="TK SDHI " SortExpression="TKSDHI"><HeaderStyle Width="50px" /></asp:BoundField>
                                    
                                    <asp:BoundField DataField="HariTotal" DataFormatString="{0:#,##0.##}" 
                                        HeaderStyle-Width="50px" HeaderText="Hari Wo" SortExpression="HariTotal"><HeaderStyle Width="70px" /></asp:BoundField>
                                    
                                    <asp:BoundField DataField="HariDone" DataFormatString="{0:#,##0.##}" 
                                        HeaderStyle-Width="50px" HeaderText="Hari Done" SortExpression="HariDone"><HeaderStyle Width="70px" /></asp:BoundField>
                                    
                                    <asp:BoundField DataField="HKNorma" DataFormatString="{0:#,##0.##}" 
                                        HeaderStyle-Width="50px" HeaderText="HK Norma" SortExpression="HKNorma"><HeaderStyle Width="70px" /></asp:BoundField>
                                    
                                    <asp:BoundField DataField="HK" HeaderStyle-Width="50px" 
                                        HeaderText="HK (Days)" DataFormatString="{0:#,##0.##}" 
                                        ItemStyle-HorizontalAlign="Right" SortExpression="HK" ><HeaderStyle Width="50px" /><ItemStyle HorizontalAlign="Right" /></asp:BoundField>
                                    
                                    <asp:BoundField DataField="Unit" HeaderText="Unit" HeaderStyle-Width="50px"  SortExpression="Unit" />
                                    
                                    <asp:BoundField DataField="FgFinish" HeaderText="Finish" SortExpression="FgFinish" HeaderStyle-Width="50px"  />
                                    
                                    <%--<asp:BoundField DataField="JobPlant" HeaderText="Job Plant" SortExpression="JobPlant" />                                    
                                    <asp:BoundField DataField="Job_Name" HeaderText="Job Plant Name" SortExpression="Job_Name" />--%>
                                    
                                    <asp:BoundField DataField="StartGawang" HeaderText="Start Gawang" HeaderStyle-Width="50px"  SortExpression="StartGawang" />
                                    
                                    <asp:BoundField DataField="EndGawang" HeaderText="End Gawang" HeaderStyle-Width="50px"  SortExpression="EndGawang" />
                                    
                                    
                                    
                                    <asp:BoundField DataField="CompleteAsisten" HeaderText="Complete Asisten" SortExpression="CompleteAsisten" HeaderStyle-Width="50px"  />
                                    
                                    <asp:BoundField DataField="CompleteAudit" HeaderText="Complete Audit" SortExpression="CompleteAudit" HeaderStyle-Width="50px"  />
                                    
                                    <asp:BoundField DataField="CompleteDenda" HeaderText="Complete Denda" SortExpression="CompleteDenda" HeaderStyle-Width="50px"   />
                                    
                                    <asp:BoundField DataField="Remark" HeaderText="Remark" SortExpression="Remark" HeaderStyle-Width="250px"  />
                                </Columns>
                            </asp:GridView>
                        </div>
                        <asp:Button class="bitbtn btnadd" runat="server" ID="btnAddDtKe2" Text="Add" ValidationGroup="Input" />
                      
                    </asp:Panel>
                    <asp:Panel runat="server" ID="pnlEditDt" Visible="false">
                        <table>
                              <tr>
                                <td>
                                    WO No</td>
                                <td>
                                    :
                                </td>
                                <td colspan="4">
                                    <asp:TextBox CssClass="TextBoxR" Enabled ="false"  runat="server" ID="tbWONO"
                                        MaxLength="20"/>
                                    <asp:Button ID="btnWONo" runat="server" class="btngo" Text="..." />
                                    
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Divisi / Block</td>
                                <td>
                                    :</td>
                                <td colspan="4">
                                <asp:DropDownList ValidationGroup="Input" CssClass="DropDownList" AutoPostBack = "true"
                                        ID="ddlType" runat="server" Height="19px" Width="139px" >
                                        <asp:ListItem>Divisi</asp:ListItem>
                                        <asp:ListItem>Block</asp:ListItem>
                                        </asp:DropDownList>
                                <asp:TextBox ID="tbBlock" runat="server" CssClass="TextBoxR" MaxLength="30" 
                            ValidationGroup="Input" Width="68px" Enabled="false" />
                        <asp:TextBox ID="tbBlockName" runat="server" CssClass="TextBoxR" 
                            Enabled="false" MaxLength="60" Width="77px" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    
                                    Qty</td>
                                <td>
                                    :
                                </td>
                                <td colspan="4">
                                    <table>
                                        <tr style="background-color: Silver; text-align: center">
                                            <td>
                                                WO</td>
                                            <td>
                                                Done</td>
                                            <td>
                                                LKM</td>
                                            
                                            <td>
                                                SDHI</td>
                                           
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="tbQtyWO" runat="server"  CssClass="TextBox" Width="65px" Enabled="False" />
                                            </td>
                                            
                                            <td>
                                                <asp:TextBox ID="tbQtyDone" runat="server" CssClass="TextBox" Enabled="False" Width="65px"  />
                                            </td>
                                            <td>
                                                <asp:TextBox ID="tbQty" runat="server" CssClass="TextBox" Width="65px" Enabled="True" AutoPostBack = "true" />
                                            </td>
                                            <td>
                                                <asp:TextBox ID="tbQtySDHI" runat="server" CssClass="TextBox" Enabled="False" 
                                                    Width="70px" />
                                            </td>
                                           
                                            
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Capacity</td>
                                <td>
                                    :</td>
                                <td colspan="4">
                                <asp:TextBox ID="tbQtyCapacity" runat="server" CssClass="TextBox" MaxLength="30" 
                            ValidationGroup="Input" Width="150px" />
                                </td>
                            </tr>
                            
                            <tr>
                                <td>
                                    TK
                                </td>
                                <td>
                                    :
                                </td>
                                <td colspan="4">
                                    <table>
                                        <tr style="background-color: Silver; text-align: center">
                                            <td>
                                                WO</td>
                                            <td>
                                                Done</td>
                                            <td>
                                                LKM</td>
                                            
                                            <td>
                                                SDHI</td>
                                           
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="tbTKWO" runat="server" AutoPostBack="true" Enabled="False" CssClass="TextBox" Width="65px" />
                                            </td>
                                            <td>
                                                <asp:TextBox ID="tbTKDone" runat="server" CssClass="TextBox" Enabled="False" Width="65px" />
                                            </td>
                                            <td>
                                                <asp:TextBox ID="tbTKLKM" runat="server" CssClass="TextBox" Enabled="False" Width="65px" />
                                            </td>
                                            <td>
                                                <asp:TextBox ID="tbTKSDHI" runat="server" CssClass="TextBox" Enabled="False" AutoPostBack = "true"
                                                    Width="65px" />
                                            </td>
                                           
                                            
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            
                        
                            <tr>
                                <td>
                                    Hari
                                </td>
                                <td>
                                    :
                                </td>
                                <td colspan="4">
                                    <table>
                                        <tr style="background-color: Silver; text-align: center">
                                            <td>
                                                WO</td>
                                            <td>
                                                Done</td>
                                            <td>
                                                HK Norma</td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="tbHariWO" runat="server" AutoPostBack="true" CssClass="TextBox" Width="65px" />
                                            </td>
                                            <td>
                                                <asp:TextBox ID="tbHariDone" runat="server" CssClass="TextBox" Width="65px" />
                                            </td>
                                            <td>
                                                <asp:TextBox ID="tbHKNorma" runat="server" CssClass="TextBoxR" Enabled="false" Width="65px" />
                                            </td>
                                            
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                   HK (days)
                                </td>
                                <td>
                                    :
                                </td>
                                <td class="style1">
                                      <asp:TextBox ID="tbHK" runat="server" CssClass="TextBox" Enabled="True" Width="65px" />
                                </td>
                                <td>
                                    Unit</td>
                                <td>
                                    :</td>
                                <td>
                                      <asp:TextBox ID="tbUnit" runat="server" CssClass="TextBoxR" Enabled="false" Width="65px" />
                                </td>
                            </tr>
                           <tr>
                                <td>
                                   Finish
                                </td>
                                <td>
                                    :
                                </td>
                                <td class="style1">
                                      <asp:TextBox ID="tbfgFinish" runat="server" CssClass="TextBox" Enabled="false" Width="65px" />
                                </td>
                                <td>
                                    Job</td>
                                <td>
                                    :</td>
                                <td>
                                      <asp:TextBox ID="tbJob" runat="server" CssClass="TextBoxR" Enabled="false" Width="65px" />
                                      <asp:TextBox ID="tbJobName" runat="server" CssClass="TextBoxR" Enabled="false" 
                                          Width="137px" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                   Start Gawang
                                </td>
                                <td>
                                    :
                                </td>
                                <td class="style1">
                                      <asp:TextBox ID="tbStartGawang" runat="server" CssClass="TextBox" Enabled="True" Width="65px" />
                                </td>
                                <td>
                                    End Gawang</td>
                                <td>
                                    :</td>
                                <td>
                                      <asp:TextBox ID="tbEndGawang" runat="server" CssClass="TextBox" Enabled="True" Width="65px" />
                                      
                                </td>
                            </tr>
                            <tr>
                                <td>
                                   Remark
                                </td>
                                <td>
                                    :
                                </td>
                                <td class="style1">
                                      <asp:TextBox ID="tbRemarkDt" runat="server" CssClass="TextBox" Enabled="True" Width="65px" />
                                </td>
                                <td>
                                    Complete</td>
                                <td>
                                    :</td>
                                <td>
                                      <asp:TextBox ID="tbCompleteAsisten" runat="server" CssClass="TextBoxR" Enabled="false" Width="65px" />
                                      <asp:TextBox ID="tbCompleteAudit" runat="server" CssClass="TextBoxR" Enabled="false" Width="65px" />
                                      <asp:TextBox ID="tbCompleteDenda" runat="server" CssClass="TextBoxR" Enabled="false" Width="65px" />
                                      
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
                                    <asp:BoundField DataField="Machine_Name" HeaderStyle-Width="150px" 
                                        HeaderText="Machine Name" SortExpression="Machine_Name" ><HeaderStyle Width="200px" /></asp:BoundField>
                                    <asp:BoundField DataField="StartHour" HeaderStyle-Width="50px" 
                                        HeaderText="Start Hour" SortExpression="StartHour" ><HeaderStyle Width="60px" /><ItemStyle HorizontalAlign="Right" /></asp:BoundField>
                                    <asp:BoundField DataField="EndHour" HeaderStyle-Width="50px" 
                                        HeaderText="End Hour" SortExpression="EndHour" ><HeaderStyle Width="60px" /><ItemStyle HorizontalAlign="Right" /></asp:BoundField>    
                                    <asp:BoundField DataField="Duration" HeaderStyle-Width="50px" 
                                        HeaderText="Duration" SortExpression="Duration" ><HeaderStyle Width="60px" /><ItemStyle HorizontalAlign="Right" /></asp:BoundField>        
                                    <asp:BoundField DataField="OperatorMachine" HeaderStyle-Width="150px" 
                                        HeaderText="Operator Machine" SortExpression="OperatorMachine" ><HeaderStyle Width="120px" /></asp:BoundField>
                                    <asp:BoundField DataField="OperatorMachineName" HeaderStyle-Width="150px" 
                                        HeaderText="Operator Machine Name" SortExpression="OperatorMachineName" ><HeaderStyle Width="200px" /></asp:BoundField>                                             
                                        
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
                                    Hour</td>
                                <td>
                                    :
                                </td>
                                <td>
                                    <asp:TextBox ID="tbStartHour" runat="server" AutoPostBack="true" CssClass="TextBox" 
                                        MaxLength="20" ValidationGroup="Input" />
                                    <asp:TextBox ID="tbEndHour" runat="server" CssClass="TextBox" 
                                        Enabled="false" MaxLength="20" />
                                    
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Duration Hour</td>
                                <td>
                                    :
                                </td>
                                <td>
                                    
                                    <asp:TextBox ID="tbQtyDuration" runat="server" CssClass="TextBox" 
                                        Width="65px" />
                                    
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Operation Machine</td>
                                <td>
                                    :
                                </td>
                                <td>
                                    <asp:TextBox ID="tbOp" runat="server" AutoPostBack="true" CssClass="TextBox" 
                                        MaxLength="20" ValidationGroup="Input" />
                                    <asp:TextBox ID="tbOpName" runat="server" CssClass="TextBox" 
                                        Enabled="false" MaxLength="60" Width="225px" />
                                    <asp:Button ID="btnOp" runat="server" Class="btngo" Text="..." 
                                        ValidationGroup="Input" />
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
                    <asp:Panel ID="pnlDt3" runat="server">
                        <asp:Button class="bitbtn btnadd" runat="server" ID="btnAddDt3" Text="Add" ValidationGroup="Input" />
                         
                        <asp:Button ID="BtnBackDt3" runat="server" class="bitbtndt btnback" Text="Back" 
                            ValidationGroup="Input" Width="71px" />
                         
                         
                        &nbsp;<asp:Button ID="btnGetOperator" runat="server" Width="110px" class="bitbtn btnadd" Text="Get Operator" 
                            ValidationGroup="Input" />
                        &nbsp;&nbsp;
                         
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
                                    <asp:BoundField DataField="Operator" HeaderStyle-Width="150px" 
                                        HeaderText="Operator Code" SortExpression="Operator" ><HeaderStyle Width="150px" /></asp:BoundField>
                                    <asp:BoundField DataField="OperatorBorongan" HeaderStyle-Width="200px" 
                                        HeaderText="Operator Name" SortExpression="OperatorBorongan" ><HeaderStyle Width="200px" /></asp:BoundField>
                                    <%--<asp:BoundField DataField="OperatorBorongan" HeaderStyle-Width="200px" 
                                        HeaderText="Operator Borongan" SortExpression="OperatorBoriongan" ><HeaderStyle Width="200px" /></asp:BoundField>--%>
                                    <asp:BoundField DataField="WorkDay" HeaderStyle-Width="80px" HeaderText="Work (Day)" 
                                        DataFormatString="{0:#,##0.##}" SortExpression="WorkDay" ><HeaderStyle Width="100px" /></asp:BoundField>
                                    <asp:BoundField DataField="WorkAdd" HeaderStyle-Width="80px" HeaderText="Add (Day)" 
                                        DataFormatString="{0:#,##0.##}" SortExpression="WorkAdd" ><HeaderStyle Width="100px" /></asp:BoundField>
                                    <asp:BoundField DataField="RemarkGawangan" HeaderStyle-Width="30px" HeaderText="RemarkGawangan" 
                                        SortExpression="RemarkGawangan" />
                                 
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
                                    Operator</td>
                                <td>
                                    :
                                </td>
                                <td>
                                    <asp:TextBox ID="tbOperator" runat="server" AutoPostBack="true" 
                                        CssClass="TextBox" MaxLength="20" ValidationGroup="Input" />
                                    <asp:TextBox ID="tbOperatorName" runat="server" CssClass="TextBox" 
                                         MaxLength="60" Width="225px" />
                                    <asp:Button ID="btnOperator" runat="server" Class="btngo" Text="..." 
                                        ValidationGroup="Input" />
                                    <asp:TextBox ID="tbOpBorongan" runat="server" CssClass="TextBoxR" 
                                        Enabled="false" Visible="False" MaxLength="60" Width="225px" />
                                                                            
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Work Day</td>
                                <td>
                                    :
                                </td>
                                <td>
                                    
                                    <asp:TextBox ID="tbWorkDay" runat="server" 
                                        CssClass="TextBox" ValidationGroup="Input" Width="65px" />
                                    <asp:TextBox ID="tbWorkAdd" runat="server"  
                                        CssClass="TextBoxR" Enabled ="false" Width="65px" />
                                    
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Remark (Gawangan)</td>
                                <td>
                                    :
                                </td>
                                <td>
                                    
                                    <asp:TextBox ID="tbRemarkGawangan" runat="server" 
                                        CssClass="TextBox" ValidationGroup="Input" Width="65px" />
                                    
                                </td>
                            </tr>
                        </table>
                        <br />
                        <asp:Button ID="btnSaveDt3" runat="server" class="bitbtndt btnsave" Text="Save" />
                        <asp:Button ID="btnCancelDt3" runat="server" class="bitbtndt btncancel" Text="Cancel" />
                    </asp:Panel>
                </asp:View>
                
                <asp:View ID="Tab5" runat="server">
                <asp:Label ID="Label4" runat="server" ForeColor="Blue" Text="WONo :"></asp:Label>
                <asp:Label ID="lblWONoMT" runat="server" ForeColor="Blue" Text=""></asp:Label>
                  <asp:Label ID="Label2" runat="server" ForeColor="Blue" Text="Type :"></asp:Label>
                     <asp:Label ID="LblTypeMT" runat="server" ForeColor="Blue" Text=""></asp:Label><br />
                                <asp:Label ID="Label6" runat="server" ForeColor="Blue" Text="Batch :"></asp:Label>
                                 <asp:Label ID="LblBatchMT" runat="server" ForeColor="Blue" Text=""></asp:Label>
                                <asp:Label ID="Label8" runat="server" ForeColor="Blue" Text="Batch Name :"></asp:Label>
                                 <asp:Label ID="LblBatchNameMT" runat="server" ForeColor="Blue" Text=""></asp:Label><br />
                    <asp:Button ID="BtnBackDt4" runat="server" class="bitbtndt btnback" Text="Back"
                            ValidationGroup="Input" />  
                            
                    <asp:Panel runat="server" ID="PnlDt4">
                    
                        <asp:Button class="bitbtn btnadd" runat="server" ID="btnAdddt4" Text="Add" Visible="false"
                            ValidationGroup="Input" />
                        
                                
                        <div style="border: 0px  solid; width: 100%; height: 100%; overflow: auto;">
                            <asp:GridView ID="GridDt4" runat="server" AutoGenerateColumns="False" 
                                ShowFooter="True">
                                <HeaderStyle CssClass="GridHeader" />
                                <RowStyle CssClass="GridItem" Wrap="false" />
                                <AlternatingRowStyle CssClass="GridAltItem" />
                                <PagerStyle CssClass="GridPager" />
                                <Columns>
                                    <asp:TemplateField HeaderText="Action"><ItemTemplate><asp:Button ID="btnEdit" runat="server" class="bitbtndt btnedit" 
                                                CommandName="Edit" Text="Edit" /><asp:Button ID="btnDelete" runat="server" class="bitbtndt btndelete" 
                                                CommandName="Delete" 
                                                OnClientClick="return confirm('Sure to delete this data?');" Text="Delete" /></ItemTemplate></asp:TemplateField>
                                    
                                    <asp:BoundField DataField="Material" HeaderStyle-Width="100px" 
                                        HeaderText="Material" SortExpression="Material"><HeaderStyle Width="100px" /></asp:BoundField>
                                    <asp:BoundField DataField="Material_Name" HeaderStyle-Width="50px" 
                                        HeaderText="Material Name" SortExpression="Material_Name"><HeaderStyle Width="50px" /></asp:BoundField>
                                    <asp:BoundField DataField="Unit" HeaderStyle-Width="70px" HeaderText="Unit"><HeaderStyle Width="70px" /></asp:BoundField>
                                    <asp:BoundField DataField="QtyTarget" DataFormatString="{0:#,##0.##}" 
                                        HeaderStyle-Width="100px" HeaderText="Qty Target" SortExpression="QtyTarget" />
                                    <asp:BoundField DataField="QtyDone" DataFormatString="{0:#,##0.##}" 
                                        HeaderStyle-Width="60px" HeaderText="Qty Done" 
                                        ItemStyle-HorizontalAlign="Right" SortExpression="QtyDone"><ItemStyle HorizontalAlign="Right" /></asp:BoundField>
                                    <asp:BoundField DataField="Qty" DataFormatString="{0:#,##0.##}" 
                                        HeaderStyle-Width="150px" HeaderText="Qty" SortExpression="Qty"><HeaderStyle Width="100px" /></asp:BoundField>
                                    <asp:BoundField DataField="QtySDHI" DataFormatString="{0:#,##0.##}" 
                                        HeaderStyle-Width="200px" HeaderText="Qty SDHI" SortExpression="QtySDHI"><HeaderStyle Width="60px" /></asp:BoundField>
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
                                                Target
                                            </td>
                                            <td>
                                                Done</td>
                                            <%--<td>Repair</td>--%>
                                            <td>
                                                Qty
                                            </td>
                                            <td>
                                                SDHI
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:TextBox CssClass="TextBox" runat="server" Enabled ="False" ID="tbQtyTarget" Width="65px" />
                                            </td>
                                            <td>
                                                <asp:TextBox CssClass="TextBox" runat="server" Enabled ="False" ID="tbQtyDoneMaterial" 
                                                    Width="65px" />
                                            </td>
                                            <td>
                                                <asp:TextBox ID="tbQtyMaterial" runat="server" CssClass="TextBox" Width="65px" />
                                            </td>
                                            <td>
                                                <asp:TextBox ID="tbQtySDHIMaterial" Enabled ="False" runat="server" CssClass="TextBox" Width="65px" />
                                            </td>
                                        </tr>
                                    </table>
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
    <asp:Label runat="server" ID="lbStatus" ForeColor="Red" />
    </form>
</body>
</html>
