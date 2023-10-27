<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TrPlanLand.aspx.vb" Inherits="Transaction_TrPlanland_TrPlanland" %>

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
</head>
<body>
    <form id="form1" runat="server">
    <div class="Content">
        <div class="H1">
             <asp:Label ID="lbltitle" runat="server" ForeColor="000000" Text="*"></asp:Label></div>
        <hr style="color: Blue" />
        <asp:Panel runat="server" ID="PnlHd">
            <table>
                <tr>
                    <td style="width: 100px; text-align: right">
                        Quick Search :
                    </td>
                    <%--TransNmbr, TransDate, STATUS, TransName, Division, DivisionName, StartDate, MasterPlanNo, QtyTarget, TargetType, Areal, QtyTarget, Remark--%>
                    <td>
                        <asp:TextBox CssClass="TextBox" runat="server" ID="tbFilter" />
                        <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlField">
                            <asp:ListItem Selected="True" Value="TransNmbr">Trans No</asp:ListItem>
                            <asp:ListItem Value="dbo.FormatDate(TransDate)">Trans Date</asp:ListItem>
                            <asp:ListItem>Status</asp:ListItem>
                            <asp:ListItem Value="TransName">Planning Name</asp:ListItem>
                            <asp:ListItem Value="Divisi">Divisi</asp:ListItem>
                            <asp:ListItem Value="DivisiName">Divisi Name</asp:ListItem>
                            <asp:ListItem Value="MasterPlanNo">MasterPlan No</asp:ListItem>
                            <asp:ListItem Value="StartDate">Start Date</asp:ListItem>
                            <asp:ListItem Value="TargetType">Target By</asp:ListItem>
                            <asp:ListItem Value="Areal">Luas</asp:ListItem>
                            <asp:ListItem Value="TargetRange">Target Range</asp:ListItem>
                            <asp:ListItem Value="QtyTarget">Target Tanam (bibit) </asp:ListItem>
                            <asp:ListItem Value="QtyBibitPN">Jumlah Bibit di PN</asp:ListItem>
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
                                 <asp:ListItem Value="TransName">Planning Name</asp:ListItem>
                            <asp:ListItem Value="Divisi">Divisi</asp:ListItem>
                            <asp:ListItem Value="DivisiName">Divisi Name</asp:ListItem>
                            <asp:ListItem Value="MasterPlanNo">MasterPlan No</asp:ListItem>
                            <asp:ListItem Value="StartDate">Start Date</asp:ListItem>
                            <asp:ListItem Value="TargetType">Target By</asp:ListItem>
                            <asp:ListItem Value="Areal">Luas</asp:ListItem>
                            <asp:ListItem Value="TargetRange">Target Range</asp:ListItem>
                            <asp:ListItem Value="QtyTarget">Target Tanam (bibit) </asp:ListItem>
                            <asp:ListItem Value="QtyBibitPN">Jumlah Bibit di PN</asp:ListItem>
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
                        <asp:BoundField DataField="TransName" HeaderStyle-Width="80px" SortExpression="TransName"
                            HeaderText="Planning Name">
                            <HeaderStyle Width="80px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Divisi" HeaderStyle-Width="200px" SortExpression="Divisi"
                            HeaderText="Divisi">
                            <HeaderStyle Width="200px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="DivisiName" HeaderStyle-Width="80px" SortExpression="DivisiName"
                            HeaderText="Divisi Name">
                            <HeaderStyle Width="80px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="MasterPlanNo" HeaderStyle-Width="200px" SortExpression="Divisi"
                            HeaderText="MasterPlan No">
                            <HeaderStyle Width="200px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="StartDate" HeaderStyle-Width="80px" SortExpression="StartDate" HtmlEncode="true" DataFormatString="{0:dd MMM yyyy}" 
                            HeaderText="Start Date">
                            <HeaderStyle Width="80px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="TargetType" HeaderText="Target By" SortExpression="TargetType" />
                        <asp:BoundField DataField="Areal" HeaderText="Luas" DataFormatString="{0:#,##0.##}" SortExpression="Areal" />
                        <asp:BoundField DataField="TargetRange" HeaderText="Range" DataFormatString="{0:#,##0}" SortExpression="QtyTarget" />
                        <asp:BoundField DataField="QtyTarget" HeaderText="Target Tanam (bibit) " DataFormatString="{0:#,##0.##}" SortExpression="QtyTarget" />
                        <asp:BoundField DataField="QtyBibitPN" HeaderText="Jumlah Bibit di PN " DataFormatString="{0:#,##0.##}" SortExpression="QtyBibitPN" />
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
                        Planning Name</td>
                    <td>
                        :</td>
                    <td>
                         <asp:TextBox ID="tbTransName" runat="server" CssClass="TextBox" MaxLength="30" 
                            ValidationGroup="Input" Width="150px" />
                            <asp:Label ID="Label1" runat="server" ForeColor="#FF3300" Text="*"></asp:Label>
                    </td>
                   
                    <td >
                        Start Date</td>
                    <td>
                        :</td>
                    <td>
                        <BDP:BasicDatePicker ID="tdpStart" runat="server" DateFormat="dd MMM yyyy" 
                        ReadOnly = "true" ValidationGroup="Input"
                        ButtonImageHeight="19px" ButtonImageWidth="20px" 
                        DisplayType="TextBoxAndImage" 
                        TextBoxStyle-CssClass="TextDate" AutoPostBack="True" 
                    ShowNoneButton="False"><TextBoxStyle CssClass="TextDate" /></BDP:BasicDatePicker>   </td>
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
                    <td></td>
                    <td></td>
                    <td><asp:Label ID="lblRegenerate" runat="server" ForeColor="#FF3300" fontsize="140" 
                            Font-Size="X-Large"></asp:Label></td>
                    </tr>
                    <tr>
                     <td>
                        MasterPlan No</td>
                    <td>
                        :
                    </td>
                    <td >
                        <asp:TextBox ID="tbMasterPlan" runat="server" CssClass="TextBoxR" Enabled="false" 
                                        Width="150px" />
                                        <asp:Button ID="btnMasterPlan" runat="server" class="btngo" Text="..." />
                        <asp:Label ID="Label4" runat="server" ForeColor="#FF3300" Text="*"></asp:Label>
                    </td>
                      <td>
                        Target By</td>
                    <td>
                        :
                    </td>
                    <td >
                            <asp:DropDownList  ValidationGroup="Input" CssClass="DropDownList" ID="ddlTargetBy" runat="server">
                                    <asp:ListItem Selected="True" Text="Optimal" />
                                    <asp:ListItem Text="Time" />
                                    <asp:ListItem Text="People" />
                                </asp:DropDownList>
                        
                        <asp:Label ID="Label2" runat="server" ForeColor="#FF3300" Text="*"></asp:Label>
                    </td>
                </tr>
                        
                               
                <tr>
                 <td> Qty</td>
                 <td> :</td>
                    <td colspan="4">
                        <table>
                            <tr style="background-color: Silver; text-align: center">
                                <td>
                                    Luas (ha)</td>
                                <td>
                                    Range</td>
                                <td>
                                    Target Tanam (bibit)</td>
                                <td>
                                    <asp:Label ID="lblJml" runat="server" Text="*"></asp:Label></td>
                            </tr>
                            <tr>
                                
                                <td>
                                    <asp:TextBox ID="tbAreal" runat="server" CssClass="TextBoxR" Enabled="false" 
                                        Width="65px" />
                                </td>
                                <td>
                                    <asp:TextBox ID="tbTargetRange" runat="server" CssClass="TextBoxR" Enabled="false" 
                                        Width="65px" />
                                </td>
                                <td>
                                    <asp:TextBox ID="tbQtyTarget" runat="server" CssClass="TextBox" 
                                        Width="90px" />
                                </td>
                                 <td>
                                    <asp:TextBox ID="tbQtyBibitPN" runat="server" CssClass="TextBox" 
                                        Width="110px" />
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
                        <td colspan="4">
                            <asp:TextBox ID="tbRemarkHD" runat="server" CssClass="TextBoxMulti" 
                                MaxLength="255" TextMode="MultiLine" ValidationGroup="Input" Width="350px" />
                            &nbsp; &nbsp; &nbsp;
                           <asp:Button ID="btnSchedule" runat="server" Class="btngo" Text="Generate Schedule" Width="120px"
                                        ValidationGroup="Input" />  
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
                    <asp:MenuItem Text="Block" Value="0" ></asp:MenuItem>
                    <asp:MenuItem Text="Equipment" Value="1" ></asp:MenuItem>
                    <asp:MenuItem Text="Schedule Job" Value="2" ></asp:MenuItem>
                    <%--<asp:MenuItem Text="Schedule Job Detail" Value="3" ></asp:MenuItem>--%>
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
                                    
                                    <asp:BoundField DataField="Block" HeaderStyle-Width="150px" 
                                        HeaderText="Block" SortExpression="DivisiBlok"><HeaderStyle Width="150px" /></asp:BoundField>
                                    <asp:BoundField DataField="BlockName" HeaderStyle-Width="200px" 
                                        HeaderText="Block Name" SortExpression="Block Name"><HeaderStyle Width="200px" /></asp:BoundField>
                                    <asp:BoundField DataField="Area" HeaderStyle-Width="60px" 
                                        HeaderText="Luas" DataFormatString="{0:#,##0.##}" 
                                        ItemStyle-HorizontalAlign="Right" SortExpression="Area" ><HeaderStyle Width="60px" /><ItemStyle HorizontalAlign="Right" /></asp:BoundField>
                                    <asp:BoundField DataField="QtyTarget" HeaderStyle-Width="60px" 
                                        HeaderText="Qty Target Tanam" DataFormatString="{0:#,##0.##}" 
                                        ItemStyle-HorizontalAlign="Right" SortExpression="Target Tanam (bibit) " ><HeaderStyle Width="60px" /><ItemStyle HorizontalAlign="Right" /></asp:BoundField>
                                    
                                    
                                </Columns>
                            </asp:GridView>
                        </div>
                        <asp:Button class="bitbtn btnadd" runat="server" ID="btnAddDtKe2" Text="Add" ValidationGroup="Input" />
                      
                    </asp:Panel>
                    <asp:Panel runat="server" ID="pnlEditDt" Visible="false">
                        <table>
                            <tr>
                                <td>
                                    Block</td>
                                <td>
                                    :
                                </td>
                                <td>
                                      
                                    <asp:TextBox CssClass="TextBox" runat="server" ID="tbBlock"
                                        MaxLength="20" AutoPostBack="true" />
                                    <asp:TextBox CssClass="TextBoxR" runat="server" ID="tbBlockName" Enabled="false"
                                        MaxLength="60" Width="225px" />
                                   <asp:Button ID="btnBlock" runat="server" Class="btngo" Text="..." 
                                        ValidationGroup="Input" /> 
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Luas </td>
                                <td>
                                    :</td>
                                <td>
                                       <asp:TextBox CssClass="TextBox" runat="server" ID="tbArealB" 
                                        MaxLength="60" Width="80px" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Qty Target Tanam (bibit)</td>
                                <td>
                                    :</td>
                                <td>
                                  <asp:TextBox CssClass="TextBox" runat="server" ID="tbqtyTargetB" 
                                        MaxLength="60" Width="80px" />
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
                                    <asp:BoundField DataField="Equipment" HeaderText="Equipment" 
                                        SortExpression="Equipment" ><HeaderStyle Width="150px" /></asp:BoundField>
                                    <asp:BoundField DataField="EquipmentName" HeaderStyle-Width="150px" 
                                        HeaderText="Equipment Name" SortExpression="EquipmentName" ><HeaderStyle Width="200px" /></asp:BoundField>
                                    <asp:BoundField DataField="Qty" HeaderStyle-Width="60px" 
                                        HeaderText="Qty" DataFormatString="{0:#,##0.##}" 
                                        ItemStyle-HorizontalAlign="Right" SortExpression="Qty" ><HeaderStyle Width="60px" /><ItemStyle HorizontalAlign="Right" /></asp:BoundField>
                                    <asp:BoundField DataField="Unit" HeaderStyle-Width="60px" 
                                        HeaderText="Unit"
                                        ItemStyle-HorizontalAlign="Left" SortExpression="Unit" ><HeaderStyle Width="60px" /><ItemStyle HorizontalAlign="Right" /></asp:BoundField>
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
                                    Equipment</td>
                                <td>
                                    :
                                </td>
                                <td>
                                    <asp:TextBox ID="tbEquip" runat="server" AutoPostBack="true" CssClass="TextBox" 
                                        MaxLength="20" ValidationGroup="Input" />
                                    <asp:TextBox ID="tbEquipName" runat="server" CssClass="TextBox" 
                                        Enabled="false" MaxLength="60" Width="225px" />
                                    <asp:Button ID="btnEquip" runat="server" Class="btngo" Text="..." 
                                        ValidationGroup="Input" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Qty</td>
                                <td>
                                    :</td>
                                <td>
                                    <asp:TextBox ID="tbQty" runat="server" CssClass="TextBox" Width="65px" />
                                    <asp:Label ID="lblUnit" runat="server" ForeColor="#FF3300" Text="*"></asp:Label>
                                </td>
                                <tr>
                                    <td>
                                        Remark
                                    </td>
                                    <td>
                                        :
                                    </td>
                                    <td>
                                        <asp:TextBox ID="tbremarkEquip" runat="server" CssClass="TextBoxMulti" 
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
                                                
                                    <asp:TemplateField HeaderText="Detail">
                                        <ItemTemplate>
                                            <asp:Button ID="btnDetailMaterial" runat="server" Class="bitbtndt btngetitem" CommandArgument="<%# Container.DataItemIndex %>"
                                                CommandName="DetailMaterial" Text="Detail" Width="70" />
                                        </ItemTemplate>
                                    </asp:TemplateField>   
                                    
                                    <asp:BoundField DataField="JobPlant" HeaderStyle-Width="150px" 
                                        HeaderText="JobPlant" SortExpression="JobPlant" ><HeaderStyle Width="150px" /></asp:BoundField>
                                    <asp:BoundField DataField="JobPlantName" HeaderStyle-Width="200px" 
                                        HeaderText="JobPlant Name" SortExpression="JobPlantName" ><HeaderStyle Width="200px" /></asp:BoundField>
                                    <asp:BoundField DataField="Team" HeaderStyle-Width="150px" 
                                        HeaderText="Team" SortExpression="Team" ><HeaderStyle Width="150px" /></asp:BoundField>
                                    <asp:BoundField DataField="TeamName" HeaderStyle-Width="200px" 
                                        HeaderText="Team Name" SortExpression="TeamName" ><HeaderStyle Width="200px" /></asp:BoundField>
                                    <asp:BoundField DataField="Qty" HeaderStyle-Width="80px" HeaderText="Qty" 
                                        DataFormatString="{0:#,##0.##}" SortExpression="Qty" ><HeaderStyle Width="100px" /></asp:BoundField>
                                    <asp:BoundField DataField="Unit" HeaderStyle-Width="150px" 
                                        HeaderText="Unit" SortExpression="Unit" ><HeaderStyle Width="50px" /></asp:BoundField>
                                    <asp:BoundField DataField="WorkDay" HeaderStyle-Width="80px" HeaderText="WorkDay" 
                                        DataFormatString="{0:#,##0.##}" SortExpression="WorkDay" ><HeaderStyle Width="100px" /></asp:BoundField>
                                    <asp:BoundField DataField="Capacity" HeaderStyle-Width="80px" HeaderText="Capacity" 
                                        DataFormatString="{0:#,##0.##}" SortExpression="Capacity" ><HeaderStyle Width="100px" /></asp:BoundField>
                                    <asp:BoundField DataField="Person" HeaderStyle-Width="80px" HeaderText="Person" 
                                        DataFormatString="{0:#,##0.##}" SortExpression="Person" ><HeaderStyle Width="100px" /></asp:BoundField>
                                   
                                 
                                </Columns>
                            </asp:GridView>
                        </div>
                        <asp:Button class="bitbtn btnadd" runat="server" ID="btnAddDt3ke2" Text="Add" ValidationGroup="Input" />
                        
                    </asp:Panel>
                    <asp:Panel runat="server" ID="pnlEditDt3" Visible="false">
                        <table>
                            <tr>
                                <td>
                                    Job Plant</td>
                                <td>
                                    :
                                </td>
                                <td>
                                    <asp:TextBox ID="tbJobPlant" runat="server" Enabled ="false"
                                        CssClass="TextBoxR" MaxLength="20" />
                                    <asp:TextBox ID="tbJobPlantName" runat="server" CssClass="TextBoxR" 
                                        Enabled="false" MaxLength="60" Width="225px" />
                                    
                                    <asp:Button ID="btnJobPlant" runat="server" Class="btngo" Text="..." 
                                        ValidationGroup="Input" />
                                    
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Team</td>
                                <td>
                                    :
                                </td>
                                <td>
                                    <asp:TextBox ID="tbTeam" runat="server" AutoPostBack="true" 
                                        CssClass="TextBox" MaxLength="20" ValidationGroup="Input" />
                                    <asp:TextBox ID="tbTeamName" runat="server" CssClass="TextBoxR" 
                                        Enabled="false" MaxLength="60" Width="225px" />
                                    <asp:Button ID="btnTeam" runat="server" Class="btngo" Text="..." 
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
                                    
                                    <asp:TextBox ID="tbQtyT" runat="server" 
                                        CssClass="TextBox" ValidationGroup="Input" Width="65px" DataFormatString="{0:#,##0.##}"/>
                                    <asp:Label ID="lblUnitT" runat="server" ForeColor="#FF3300" Text="*"></asp:Label>    
                                    
                                </td>
                            </tr>
                            <tr>
                            <td colspan="3">
                        <table>
                            <tr style="background-color: Silver; text-align: center">
                                <td>
                                    Work Day</td>
                                <td>
                                    Capacity</td>
                                <td>
                                    Start Date</td>
                                <td>
                                    End Date</td>
                                    <td>
                                    Person</td>
                            </tr>
                            <tr>
                                
                                <td>
                                    <asp:TextBox ID="tbWorkDay" runat="server" CssClass="TextBoxR" Enabled="false" 
                                        Width="65px" DataFormatString="{0:#,#0.##}" />
                                </td>
                                <td>
                                    <asp:TextBox ID="tbcapacity" runat="server" CssClass="TextBoxR" Enabled="false" 
                                        Width="65px" DataFormatString="{0:#,#0.##}" />
                                </td>
                                <td>
                                    <BDP:BasicDatePicker ID="tdpStartDate" runat="server" DateFormat="dd MMM yyyy" 
                                        ReadOnly = "true" ValidationGroup="Input" ButtonImageHeight="19px" ButtonImageWidth="20px" 
                                        DisplayType="TextBoxAndImage" TextBoxStyle-CssClass="TextDate" AutoPostBack="True" 
                                        ShowNoneButton="False"><TextBoxStyle CssClass="TextDate" /></BDP:BasicDatePicker>  
                                </td>
                                 <td>
                                   <BDP:BasicDatePicker ID="tdpEndDate" runat="server" DateFormat="dd MMM yyyy" 
                                        ReadOnly = "true" ValidationGroup="Input" ButtonImageHeight="19px" ButtonImageWidth="20px" 
                                        DisplayType="TextBoxAndImage" TextBoxStyle-CssClass="TextDate" AutoPostBack="True" 
                                        ShowNoneButton="False"><TextBoxStyle CssClass="TextDate" /></BDP:BasicDatePicker> 
                                </td>
                                 <td>
                                    <asp:TextBox ID="tbperson" runat="server" CssClass="TextBox" ValidationGroup="Input" 
                                        Width="65px" DataFormatString="{0:#,#0.##}" />
                                </td>
                            </tr>
                        </table>
                    </td>
                    </tr>
                              </table>
                        <br />
                        <asp:Button ID="btnSaveDt3" runat="server" class="bitbtndt btnsave" Text="Save" />
                        <asp:Button ID="btnCancelDt3" runat="server" class="bitbtndt btncancel" Text="Cancel" />
                    </asp:Panel>
                </asp:View>
                
                <asp:View ID="Tab5" runat="server">
                
                <asp:Panel ID="pnlInfoDt" runat="server">
                        <asp:Label ID="lblJobplan" runat="server" Text="JobPlan :" />
                        <asp:Label ID="lblJobplanCode" runat="server" Font-Bold="true" ForeColor="Blue"
                            Text="JobPlanCode" />
                        <asp:Label ID="lblJobPlanNameDt" runat="server" Font-Bold="true" ForeColor="Blue"
                            Text="JobPlanName" />
                        <%--&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:Label ID="Label3" runat="server" Font-Bold="true" Text="Total Qty : " />
                        <asp:Label ID="lblTotalQty" runat="server" Font-Bold="true" ForeColor="Red" Text="0" />
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:Label ID="Label8" runat="server" Font-Bold="true" Text="Total Qty Compare :" />
                        <asp:Label ID="lblTotalQtyCompare" runat="server" Font-Bold="true" ForeColor="Red"
                            Text="0" />--%>
                            
                    </asp:Panel>
                       <br /> 
                    <asp:Panel runat="server" ID="PnlDt4">
                    
                        <asp:Button class="bitbtn btnadd" runat="server" ID="btnAdddt4" Text="Add" Visible="false"
                            ValidationGroup="Input" />
                        
                                
                        &nbsp;
                        <asp:Button ID="btnBackDt" runat="server" class="bitbtndt btnback" Text="Back" 
                            Width="60" />
                            <br />
                            <br/>
                        <div style="border: 0px  solid; width: 100%; height: 100%; overflow: auto;">
                            <asp:GridView ID="GridDt4" runat="server" AutoGenerateColumns="False" 
                                ShowFooter="True">
                                <HeaderStyle CssClass="GridHeader" />
                                <RowStyle CssClass="GridItem" Wrap="false" />
                                <AlternatingRowStyle CssClass="GridAltItem" />
                                <PagerStyle CssClass="GridPager" />
                                <Columns>
                                    <asp:TemplateField HeaderText="Action">
                                        <ItemTemplate>
                                            <asp:Button ID="btnEdit" runat="server" class="bitbtndt btnedit" 
                                                CommandName="Edit" Text="Edit" />
                                            <asp:Button ID="btnDelete" runat="server" class="bitbtndt btndelete" 
                                                CommandName="Delete" 
                                                OnClientClick="return confirm('Sure to delete this data?');" Text="Delete" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="Material" HeaderStyle-Width="100px" 
                                        HeaderText="Material" SortExpression="Block">
                                        <HeaderStyle Width="100px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="MaterialName" HeaderStyle-Width="50px" 
                                        HeaderText="Material Name" SortExpression="BlockName">
                                        <HeaderStyle Width="50px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Qty" DataFormatString="{0:#,##0.##}" 
                                        HeaderStyle-Width="100px" HeaderText="Qty" ItemStyle-HorizontalAlign="Right" 
                                        SortExpression="Qty" />
                                    <asp:BoundField DataField="Unit" HeaderStyle-Width="80px" HeaderText="Unit" 
                                        SortExpression="Unit">
                                        <HeaderStyle Width="40px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Dosis" DataFormatString="{0:#,##0.##}" 
                                        HeaderStyle-Width="60px" HeaderText="Dosis" ItemStyle-HorizontalAlign="Right" 
                                        SortExpression="Dosis">
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:BoundField>
                                </Columns>
                            </asp:GridView>
                            <br />
                        </div>
                        <asp:Button class="bitbtn btnadd" runat="server" ID="btnAddDt4ke2" Text="Add" Visible="false"
                            ValidationGroup="Input" />
                        
                        &nbsp;
                        <asp:Button ID="btnBackDt2" runat="server" class="bitbtndt btnback" Text="Back" 
                            Width="60" />
                        
                    </asp:Panel>
                    <br />
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
                                        AutoPostBack="true" />
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
                                            <td>Qty</td>
                                            <td>
                                                Unit</td>
                                            <td>Dosis</td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:TextBox CssClass="TextBoxR" runat="server" ID="tbQtyM" Enabled="false" Width="65px" />
                                            </td>
                                            <td>
                                                <asp:TextBox CssClass="TextBoxR" runat="server" ID="tbUnit" Enabled="false" Width="65px" />
                                            </td>
                                            <td><asp:TextBox CssClass="TextBox" runat="server" ID="tbDosis" Enabled="false" Width="65px"/></td>
                                            
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
