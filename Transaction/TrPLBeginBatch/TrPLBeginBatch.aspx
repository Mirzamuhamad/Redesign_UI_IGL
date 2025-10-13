<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TrPLBeginBatch.aspx.vb" Inherits="Transaction_TrPLBeginBatch_TrPLBeginBatch" %>

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
            var _QtyOutput = parseFloat(document.getElementById("tbQtyM").value.replace(/\$|\,/g,""));
            var _QtyWO = parseFloat(document.getElementById("tbQtyT").value.replace(/\$|\,/g,""));
            var _QtyGood = parseFloat(document.getElementById("tbQtyB").value.replace(/\$|\,/g,""));
//            var _QtyRepair = parseFloat(document.getElementById("tbQtyRepair").value.replace(/\$|\,/g,""));
            var _QtyReject = parseFloat(document.getElementById("tbQtyS").value.replace(/\$|\,/g,""));
            
            
         
            document.getElementById("tbQtyM").value = setdigit(_QtyM,'<%=VIEWSTATE("DigitQty")%>');
            document.getElementById("tbQtyT").value = setdigit(_QtyT,'<%=VIEWSTATE("DigitQty")%>');
            document.getElementById("tbQtyB").value = setdigit(_QtyB,'<%=VIEWSTATE("DigitQty")%>');
            document.getElementById("tbQtyS").value = setdigit(_QtyS,'<%=VIEWSTATE("DigitQty")%>');            
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
            Beginning Batch</div>
        <hr style="color: Blue" />
        <asp:Panel runat="server" ID="PnlHd">
            <table>
                <tr>
                    <td style="width: 100px; text-align: right">
                        Quick Search :
                    </td>
                    <%--TransNmbr, TransDate, STATUS, BatchNo, Division, DivisionName, VarietasName, Varietas, TotalNilai, TotalMN, TotalPN, TotalBook, Remark--%>
                    <td>
                        <asp:TextBox CssClass="TextBox" runat="server" ID="tbFilter" />
                        <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlField">
                            <asp:ListItem Selected="True" Value="TransNmbr">Trans No</asp:ListItem>
                            <asp:ListItem Value="dbo.FormatDate(TransDate)">Trans Date</asp:ListItem>
                            <asp:ListItem>Status</asp:ListItem>
                            <asp:ListItem Value="BatchNo">Batch No</asp:ListItem>
                            <asp:ListItem Value="Division">Division</asp:ListItem>
                            <asp:ListItem Value="DivisionName">Division Name</asp:ListItem>
                            <asp:ListItem Value="Varietas">Varietas</asp:ListItem>
                            <asp:ListItem Value="VarietasName">Varietas Name</asp:ListItem>
                            <asp:ListItem Value="TotalMN">Total Module</asp:ListItem>
                            <asp:ListItem Value="TotalPN">Total Bedeng</asp:ListItem>
                            <asp:ListItem Value="TotalBook">Total Booking</asp:ListItem>
                            <asp:ListItem Value="TotalNilai">Amount</asp:ListItem>
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
                        Rows
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
                                 <asp:ListItem Value="BatchNo">Batch No</asp:ListItem>
                            <asp:ListItem Value="Division">Division</asp:ListItem>
                            <asp:ListItem Value="DivisionName">Division Name</asp:ListItem>
                            <asp:ListItem Value="Varietas">Varietas</asp:ListItem>
                            <asp:ListItem Value="VarietasName">Varietas Name</asp:ListItem>
                            <asp:ListItem Value="TotalMN">Total Module</asp:ListItem>
                            <asp:ListItem Value="TotalPN">Total Bedeng</asp:ListItem>
                            <asp:ListItem Value="TotalBook">Total Booking</asp:ListItem>
                            <asp:ListItem Value="TotalNilai">Amount</asp:ListItem>
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
                                    
                                    <%--<asp:ListItem Text="Print" />--%>
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
                        <asp:BoundField DataField="BatchNo" HeaderStyle-Width="80px" SortExpression="BatchNo"
                            HeaderText="Batch No">
                            <HeaderStyle Width="80px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Division" HeaderStyle-Width="200px" SortExpression="Division"
                            HeaderText="Divisi">
                            <HeaderStyle Width="200px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="DivisionName" HeaderStyle-Width="80px" SortExpression="DivisionName"
                            HeaderText="Division Name">
                            <HeaderStyle Width="80px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Varietas" HeaderStyle-Width="200px" SortExpression="Divisi"
                            HeaderText="Varietas">
                            <HeaderStyle Width="200px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="VarietasName" HeaderStyle-Width="80px" SortExpression="VarietasName"
                            HeaderText="Varietas Name">
                            <HeaderStyle Width="80px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="TotalMN" HeaderText="Module" DataFormatString="{0:#,##0.##}" SortExpression="TotalMN" />
                        <asp:BoundField DataField="TotalPN" HeaderText="Batch" DataFormatString="{0:#,##0.##}" SortExpression="TotalPN" />
                        <asp:BoundField DataField="TotalBook" HeaderText="Booking" DataFormatString="{0:#,##0.##}" SortExpression="TotalBook" />
                        <asp:BoundField DataField="TotalNilai" HeaderText="Amount" DataFormatString="{0:#,##0.##}" SortExpression="TotalNilai" />
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
                    <td >
                        Date</td>
                    <td>
                        :</td>
                    <td colspan="4">
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
                        Batch No</td>
                    <td>
                        :</td>
                    <td>
                         <asp:TextBox ID="tbBatchNo" runat="server" CssClass="TextBox" MaxLength="30" 
                            ValidationGroup="Input" Width="126px" />
                            <asp:Label ID="Label1" runat="server" ForeColor="#FF3300" Text="*"></asp:Label>
                    </td>
                    <td>
                        </td>
                    <td>
                        </td>
                    <td>
                       
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
                        Division</td>
                    <td>
                        :
                    </td>
                    <td >
                        <asp:DropDownList ValidationGroup="Input" CssClass="DropDownList" ID="ddlDivisi"
                            runat="server" AutoPostBack="true" Height="16px" Width="222px" />
                        <asp:Label ID="Label11" runat="server" ForeColor="#FF3300" Text="*"></asp:Label>
                    </td>
                     <td>
                        Varietas</td>
                    <td>
                        :
                    </td>
                    <td colspan="7">
                        <asp:DropDownList ValidationGroup="Input" CssClass="DropDownList" ID="ddlVarietas"
                            runat="server" Height="16px" Width="222px" />
                        <asp:Label ID="Label4" runat="server" ForeColor="#FF3300" Text="*"></asp:Label>
                    </td>
                </tr>
                        
                               
                <tr>
                 <td> Total</td>
                 <td> :</td>
                    <td colspan="6">
                        <table>
                            <tr style="background-color: Silver; text-align: center">
                                <td>
                                    Module</td>
                                <td>
                                    Bedeng</td>
                                <td>
                                    Booking</td>
                                <td>
                                    Amount</td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:TextBox ID="tbQtyModule" runat="server" CssClass="TextBoxR" Enabled="false" 
                                        Width="65px" />
                                </td>
                                <td>
                                    <asp:TextBox ID="tbQtyBedeng" runat="server" CssClass="TextBoxR" Enabled="false" 
                                        Width="65px" />
                                </td>
                                <td>
                                    <asp:TextBox ID="tbBooking" runat="server" CssClass="TextBoxR" Enabled="false" 
                                        Width="65px" />
                                </td>
                                <td>
                                    <asp:TextBox ID="tbAmount" runat="server" CssClass="TextBoxR" Enabled="false" 
                                        Width="65px" />
                                </td>
                            </tr>
                        </table>
                    </td>
                    <tr>
                        <td>
                            Remark
                        </td>
                        <td>
                            :
                        </td>
                        <td colspan="7">
                            <asp:TextBox ID="tbRemarkHD" runat="server" CssClass="TextBoxMulti" 
                                MaxLength="255" TextMode="MultiLine" ValidationGroup="Input" Width="350px" />
                            &nbsp; &nbsp; &nbsp;
                            
                        </td>
                    </tr>
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
                    <asp:MenuItem Text="Detail Job" Value="0" ></asp:MenuItem>
                    <asp:MenuItem Text="Detail Module" Value="1" ></asp:MenuItem>
                    <asp:MenuItem Text="Detail Bedeng" Value="2" ></asp:MenuItem>
                    <asp:MenuItem Text="Detail Booking" Value="3" ></asp:MenuItem>
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
                                    <asp:BoundField DataField="Job" HeaderStyle-Width="150px" 
                                        HeaderText="Job" SortExpression="DivisiBlok"><HeaderStyle Width="150px" /></asp:BoundField>
                                    <asp:BoundField DataField="Job" HeaderStyle-Width="150px" 
                                        HeaderText="Job" SortExpression="DivisiBlok"><HeaderStyle Width="150px" /></asp:BoundField>
                                    <asp:BoundField DataField="JobName" HeaderStyle-Width="200px" 
                                        HeaderText="Job Name" SortExpression="Job Name"><HeaderStyle Width="200px" /></asp:BoundField>
                                    <asp:BoundField DataField="Amount" HeaderStyle-Width="60px" 
                                        HeaderText="Amount" DataFormatString="{0:#,##0.##}" 
                                        ItemStyle-HorizontalAlign="Right" SortExpression="Amount" ><HeaderStyle Width="60px" /><ItemStyle HorizontalAlign="Right" /></asp:BoundField>
                                    <asp:BoundField DataField="Remark" HeaderText="Remark" SortExpression="Remark" />
                                    
                                </Columns>
                            </asp:GridView>
                        </div>
                        <asp:Button class="bitbtn btnadd" runat="server" ID="btnAddDtKe2" Text="Add" ValidationGroup="Input" />
                      
                    </asp:Panel>
                    <asp:Panel runat="server" ID="pnlEditDt" Visible="false">
                        <table>
                            <tr>
                                <td>
                                    Job</td>
                                <td>
                                    :
                                </td>
                                <td colspan="4">
                                      
                                    <asp:TextBox CssClass="TextBox" runat="server" ID="tbJob"
                                        MaxLength="20" AutoPostBack="true" />
                                    <asp:TextBox CssClass="TextBoxR" runat="server" ID="tbJobName" Enabled="false"
                                        MaxLength="60" Width="225px" />
                                   <asp:Button ID="btnJob" runat="server" Class="btngo" Text="..." 
                                        ValidationGroup="Input" /> 
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Amount</td>
                                <td>
                                    :</td>
                                <td colspan="4">
                                   <asp:TextBox CssClass="TextBox" runat="server" ID="tbTAmount" 
                                        MaxLength="60" Width="80px" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Remark</td>
                                <td>
                                    :</td>
                                <td colspan="4">
                                   <asp:TextBox CssClass="TextBox" runat="server" ID="tbRemark" 
                                        MaxLength="60" Width="250px" />
                                </td>
                            </tr>
                            
                            
                           
                        </table>
                        <br />
                        <asp:Button ID="btnSaveDt" runat="server" class="bitbtndt btnsave" Text="Save" />
                        <asp:Button ID="btnCancelDt" runat="server" class="bitbtndt btncancel" Text="Cancel" />
                    </asp:Panel>
                </asp:View>
                <asp:View ID="Tab3" runat="server">
                        
                    <asp:Panel ID="pnlDt2" runat="server">
                        <asp:Button class="bitbtn btnadd" runat="server" ID="btnAddDt2" Text="Add" ValidationGroup="Input" />
                         
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
                                    <asp:BoundField DataField="Module" HeaderText="Module" 
                                        SortExpression="Module" ><HeaderStyle Width="150px" /></asp:BoundField>
                                    <asp:BoundField DataField="ModuleName" HeaderStyle-Width="150px" 
                                        HeaderText="Module Name" SortExpression="ModuleName" ><HeaderStyle Width="200px" /></asp:BoundField>
                                    <asp:BoundField DataField="Qty" HeaderStyle-Width="60px" 
                                        HeaderText="Qty OK" DataFormatString="{0:#,##0.##}" 
                                        ItemStyle-HorizontalAlign="Right" SortExpression="Qty" ><HeaderStyle Width="60px" /><ItemStyle HorizontalAlign="Right" /></asp:BoundField>
                                    <asp:BoundField DataField="QtyRepair" HeaderStyle-Width="60px" 
                                        HeaderText="Qty Repair" DataFormatString="{0:#,##0.##}" 
                                        ItemStyle-HorizontalAlign="Right" SortExpression="QtyRepair" ><HeaderStyle Width="60px" /><ItemStyle HorizontalAlign="Right" /></asp:BoundField>
                                    <asp:BoundField DataField="QtyModule" HeaderStyle-Width="60px" 
                                        HeaderText="Total Module" DataFormatString="{0:#,##0.##}" 
                                        ItemStyle-HorizontalAlign="Right" SortExpression="QtyModule" ><HeaderStyle Width="60px" /><ItemStyle HorizontalAlign="Right" /></asp:BoundField>
                                    <asp:BoundField DataField="Remark" HeaderStyle-Width="150px" 
                                        HeaderText="Remark" SortExpression="Remark" ><HeaderStyle Width="200px" /></asp:BoundField>
                                    
                                </Columns>
                            </asp:GridView>
                        </div>
                        <asp:Button ID="btnAddDt2ke2" runat="server" class="bitbtn btnadd" Text="Add" 
                            ValidationGroup="Input" />
                        
                    </asp:Panel>
                    <asp:Panel runat="server" ID="pnlEditDt2" Visible="false">
                        <table>
                            <tr>
                                <td>
                                    Module</td>
                                <td>
                                    :
                                </td>
                                <td>
                                    <asp:TextBox ID="tbModule" runat="server" AutoPostBack="true" CssClass="TextBox" 
                                        MaxLength="20" ValidationGroup="Input" />
                                    <asp:TextBox ID="tbModuleName" runat="server" CssClass="TextBox" 
                                        Enabled="false" MaxLength="60" Width="225px" />
                                    <asp:Button ID="btnModule" runat="server" Class="btngo" Text="..." 
                                        ValidationGroup="Input" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Qty</td>
                                <td>
                                    :</td>
                                <td>
                                    <table>
                                        <tr style="background-color: Silver; text-align: center">
                                            <td>
                                                OK</td>
                                            <td>
                                                Repair</td>
                                            <td>
                                                Module</td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="tbQtyOK" runat="server" AutoPostBack="true" CssClass="TextBox" 
                                                    Width="65px" />
                                            </td>
                                            <td>
                                                <asp:TextBox ID="tbQtyRepair" runat="server" AutoPostBack="true" 
                                                    CssClass="TextBox" Width="65px" />
                                            </td>
                                            <td>
                                                <asp:TextBox ID="tbQtyModuleM" runat="server" CssClass="TextBoxR" 
                                                    Enabled="false" Width="65px" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <tr>
                                    <td>
                                        Remark
                                    </td>
                                    <td>
                                        :
                                    </td>
                                    <td colspan="7">
                                        <asp:TextBox ID="tbremarkjob" runat="server" CssClass="TextBoxMulti" 
                                            MaxLength="255" TextMode="MultiLine" ValidationGroup="Input" Width="350px" />
                                        &nbsp; &nbsp; &nbsp;
                                    </td>
                                </tr>
                            </tr>
                            </tr>
                        </table>
                        <br />
                        <asp:Button ID="btnSaveDt2" runat="server" class="bitbtndt btnsave" Text="Save" />
                        <asp:Button ID="btnCancelDt2" runat="server" class="bitbtndt btncancel" Text="Cancel" />
                    </asp:Panel>
                </asp:View>
                <asp:View ID="Tab4" runat="server">
               
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
                                    <asp:BoundField DataField="Bedeng" HeaderStyle-Width="150px" 
                                        HeaderText="Bedeng" SortExpression="Bedeng" ><HeaderStyle Width="150px" /></asp:BoundField>
                                    <asp:BoundField DataField="BedengName" HeaderStyle-Width="200px" 
                                        HeaderText="Bedeng Name" SortExpression="BedengName" ><HeaderStyle Width="200px" /></asp:BoundField>
                                    <asp:BoundField DataField="Qty" HeaderStyle-Width="80px" HeaderText="Qty" 
                                        DataFormatString="{0:#,##0.##}" SortExpression="Qty" ><HeaderStyle Width="100px" /></asp:BoundField>
                                    <asp:BoundField DataField="Remark" HeaderStyle-Width="200px" HeaderText="Remark" 
                                        SortExpression="Remark" />
                                 
                                </Columns>
                            </asp:GridView>
                        </div>
                        <asp:Button class="bitbtn btnadd" runat="server" ID="btnAddDt3ke2" Text="Add" ValidationGroup="Input" />
                        
                    </asp:Panel>
                    <asp:Panel runat="server" ID="pnlEditDt3" Visible="false">
                        <table>
                            <tr>
                                <td>
                                    Bedeng</td>
                                <td>
                                    :
                                </td>
                                <td>
                                    <asp:TextBox ID="tbBedeng" runat="server" AutoPostBack="true" 
                                        CssClass="TextBox" MaxLength="20" ValidationGroup="Input" />
                                    <asp:TextBox ID="tbBedengName" runat="server" CssClass="TextBoxR" 
                                        Enabled="false" MaxLength="60" Width="225px" />
                                    <asp:Button ID="btnBedeng" runat="server" Class="btngo" Text="..." 
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
                            <asp:TextBox ID="tbremarkB" runat="server" CssClass="TextBoxMulti" 
                                MaxLength="255" TextMode="MultiLine" ValidationGroup="Input" Width="350px" />
                            &nbsp; &nbsp; &nbsp;
                            
                        </td>
                    </tr>
                        </table>
                        <br />
                        <asp:Button ID="btnSaveDt3" runat="server" class="bitbtndt btnsave" Text="Save" />
                        <asp:Button ID="btnCancelDt3" runat="server" class="bitbtndt btncancel" Text="Cancel" />
                    </asp:Panel>
                </asp:View>
                
                <asp:View ID="Tab5" runat="server">
                 
                            
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
                                    
                                    <asp:BoundField DataField="Block" HeaderStyle-Width="100px" 
                                        HeaderText="Block" SortExpression="Block"><HeaderStyle Width="100px" /></asp:BoundField>
                                    <asp:BoundField DataField="BlockName" HeaderStyle-Width="50px" 
                                        HeaderText="Block Name" SortExpression="BlockName"><HeaderStyle Width="50px" /></asp:BoundField>
                                    <asp:BoundField DataField="QtyMax" DataFormatString="{0:#,##0.##}" 
                                        HeaderStyle-Width="100px" HeaderText="Max Cap" SortExpression="QtyMax" />
                                    <asp:BoundField DataField="QtyUse" DataFormatString="{0:#,##0.##}" 
                                        HeaderStyle-Width="80px" HeaderText="Qty Tertanam" SortExpression="QtyTanam"><HeaderStyle Width="100px" /></asp:BoundField>
                                    <asp:BoundField DataField="QtyTanam" DataFormatString="{0:#,##0.##}" 
                                        HeaderStyle-Width="60px" HeaderText="Qty Booking" 
                                        ItemStyle-HorizontalAlign="Right" SortExpression="Qty"><ItemStyle HorizontalAlign="Right" /></asp:BoundField>
                                    <asp:BoundField DataField="QtySaldo" DataFormatString="{0:#,##0.##}" 
                                        HeaderStyle-Width="80px" HeaderText="Saldo Cap" SortExpression="QtySaldo"><HeaderStyle Width="60px" /></asp:BoundField>
                                    <asp:BoundField DataField="Remark" HeaderStyle-Width="100px" 
                                        HeaderText="Remark" ><HeaderStyle Width="100px" /></asp:BoundField>
                                    
                                </Columns>
                            </asp:GridView>
                        </div>
                        <asp:Button class="bitbtn btnadd" runat="server" ID="btnAddDt4ke2" Text="Add" Visible="false"
                            ValidationGroup="Input" />
                        
                    </asp:Panel>
                    <asp:Panel runat="server" ID="pnlEditDt4" Visible="false">
                        <table>
                            <tr>
                                <td>
                                    Block</td>
                                <td>
                                    :
                                </td>
                                <td>
                                    <asp:TextBox CssClass="TextBox" ID="tbBlock" runat="server" MaxLength="30" Width="150px"
                                        AutoPostBack="true" />
                                    <asp:TextBox ID="tbBlockName" runat="server" CssClass="TextBox" 
                                        Enabled="false" MaxLength="60" Width="225px" />
                                    <asp:Button ID="btnBlock" runat="server" class="btngo" Text="..." />
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
                                            <td>Max Cap</td>
                                            <td>
                                                Qty Tertanam</td>
                                            <td>Qty Booking</td>
                                            <td>
                                                Saldo Cap
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:TextBox CssClass="TextBox" Enabled=false runat="server" ID="tbQtyMax" Width="65px" />
                                            </td>
                                            <td>
                                                <asp:TextBox CssClass="TextBox" Enabled=false runat="server" ID="tbQtyTertanam" 
                                                    Width="65px" />
                                            </td>
                                            <td><asp:TextBox CssClass="TextBox" runat="server" ID="tbQtyBooking" AutoPostBack ="true" Width="65px"/></td>
                                            <td>
                                                <asp:TextBox ID="tbSaldoCap" Enabled=false runat="server" CssClass="TextBox" Width="65px" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            
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
   
    <asp:Label runat="server" ID="lbStatus" ForeColor="Red" />
    </form>
</body>
</html>
