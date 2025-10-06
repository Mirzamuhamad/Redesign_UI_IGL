<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TrLHP.aspx.vb" Inherits="Transaction_TrLHP_TrLHP" %>

<%@ Register Assembly="BasicFrame.WebControls.BasicDatePicker" Namespace="BasicFrame.WebControls"
    TagPrefix="BDP" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>

    <script src="../../Function/OpenDlg.js" type="text/javascript"></script>

    <script src="../../Function/Function.JS" type="text/javascript"></script>

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
            var _QtyOutput = parseFloat(document.getElementById("tbQtyOutput").value.replace(/\$|\,/g,""));
            var _QtyWO = parseFloat(document.getElementById("tbQtyWO").value.replace(/\$|\,/g,""));
            var _QtyGood = parseFloat(document.getElementById("tbQtyGood").value.replace(/\$|\,/g,""));
//            var _QtyRepair = parseFloat(document.getElementById("tbQtyRepair").value.replace(/\$|\,/g,""));
            var _QtyReject = parseFloat(document.getElementById("tbQtyReject").value.replace(/\$|\,/g,""));
            
            _QtyOutput = _QtyGood + _QtyReject
         
            document.getElementById("tbQtyWO").value = setdigit(_QtyWO,'<%=VIEWSTATE("DigitQty")%>');
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
            Laporan Hasil Produksi</div>
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
                            <asp:ListItem Selected="True" Value="TransNmbr">LHP No</asp:ListItem>
                            <asp:ListItem Value="dbo.FormatDate(TransDate)">Input Date</asp:ListItem>
                            <asp:ListItem>Status</asp:ListItem>
                            <%--<asp:ListItem Value="Type">Type</asp:ListItem>--%>
                            <asp:ListItem Value="LHPNo">LHP No</asp:ListItem>
                            <asp:ListItem Value="WorkCtr">WorkCtr</asp:ListItem>
                            <asp:ListItem Value="WorkCtr_Name">WorkCtr Name</asp:ListItem>
                            <asp:ListItem Value="ProductionDate">Production Date</asp:ListItem>
                            <asp:ListItem Value="Shift">Shift</asp:ListItem>
                            <asp:ListItem Value="Shift_Name">Shift Name</asp:ListItem>
                            <asp:ListItem Value="WorkHour">Work Hour</asp:ListItem>
                            <asp:ListItem Value="WONo">WO No</asp:ListItem>
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
                                <%--<asp:ListItem Value="Type">Type</asp:ListItem>--%>
                                <asp:ListItem Value="LHPNo">LHP No</asp:ListItem>
                                <asp:ListItem Value="WorkCtr">WorkCtr</asp:ListItem>
                                <asp:ListItem Value="WorkCtr_Name">WorkCtr Name</asp:ListItem>
                                <asp:ListItem Value="ProductionDate">Production Date</asp:ListItem>
                                <asp:ListItem Value="Shift">Shift</asp:ListItem>
                                <asp:ListItem Value="Shift_Name">Shift Name</asp:ListItem>
                                <asp:ListItem Value="WorkHour">Work Hour</asp:ListItem>
                                <asp:ListItem Value="WONo">WO No</asp:ListItem>
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
                <asp:GridView ID="GridView1" runat="server" AllowPaging="True" AllowSorting="true"
                    CssClass="Grid" AutoGenerateColumns="false">
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
                                    <asp:ListItem Text="Lot No" />
                                    <%--<asp:ListItem Text="Print" />--%>
                                </asp:DropDownList>
                                <asp:Button class="btngo" runat="server" ID="BtnGo" Text="G" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>"
                                    CommandName="Go" />
                            </ItemTemplate>
                            <HeaderStyle Width="110px" />
                        </asp:TemplateField>
                        <asp:BoundField DataField="TransNmbr" HeaderStyle-Width="120px" SortExpression="Nmbr"
                            HeaderText="Result No"></asp:BoundField>
                        <asp:BoundField DataField="Status" HeaderText="Status"></asp:BoundField>
                        <asp:BoundField DataField="TransDate" DataFormatString="{0:dd MMM yyyy}" HtmlEncode="true"
                            HeaderStyle-Width="80px" SortExpression="TransDate" HeaderText="Result Date">
                        </asp:BoundField>
                        <%--<asp:BoundField DataField="Type" HeaderStyle-Width="80px" SortExpression="Type" HeaderText="Type"></asp:BoundField>--%>
                        <asp:BoundField DataField="LHPNo" HeaderStyle-Width="80px" SortExpression="LHPNo"
                            HeaderText="LHP No"></asp:BoundField>
                        <asp:BoundField DataField="WorkCtr" HeaderStyle-Width="200px" SortExpression="WorkCtr"
                            HeaderText="WorkCtr"></asp:BoundField>
                        <asp:BoundField DataField="WorkCtr_Name" HeaderStyle-Width="80px" SortExpression="WorkCtr_Name"
                            HeaderText="WorkCtr Name"></asp:BoundField>
                        <asp:BoundField DataField="ProductionDate" HeaderStyle-Width="200px" SortExpression="ProductionDate"
                            DataFormatString="{0:dd MMM yyyy}" HeaderText="Production Date"></asp:BoundField>
                        <asp:BoundField DataField="Shift" HeaderStyle-Width="80px" SortExpression="Shift"
                            HeaderText="Shift"></asp:BoundField>
                        <asp:BoundField DataField="Shift_Name" HeaderStyle-Width="250px" SortExpression="Shift_Name"
                            HeaderText="Shift Name"></asp:BoundField>
                        <asp:BoundField DataField="WorkHour" HeaderStyle-Width="80px" SortExpression="WorkHour"
                            HeaderText="Work Hour"></asp:BoundField>
                        <asp:BoundField DataField="Remark" HeaderStyle-Width="250px" SortExpression="Remark"
                            HeaderText="Remark"></asp:BoundField>
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
                        LHP No
                    </td>
                    <td>
                        :
                    </td>
                    <td>
                        <asp:TextBox CssClass="TextBoxR" runat="server" ID="tbCode" Width="150px" Enabled="False" />
                    </td>
                    <td>
                        Input Date
                    </td>
                    <td>
                        :
                    </td>
                    <td>
                        <BDP:BasicDatePicker ID="tbDate" runat="server" DateFormat="dd MMM yyyy" ReadOnly="true"
                            ValidationGroup="Input" ButtonImageHeight="19px" ButtonImageWidth="20px" DisplayType="TextBoxAndImage"
                            TextBoxStyle-CssClass="TextDate" ShowNoneButton="False">
                            <TextBoxStyle CssClass="TextDate" />
                        </BDP:BasicDatePicker>
                    </td>
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
                        Work Center
                    </td>
                    <td>
                        :
                    </td>
                    <td>
                        <asp:DropDownList ValidationGroup="Input" CssClass="DropDownList" ID="ddlWorkCtr"
                            runat="server" AutoPostBack="true" />
                        <asp:Label ID="Label11" runat="server" ForeColor="#FF3300" Text="*"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        Production Date & Shift
                    </td>
                    <td>
                        :
                    </td>
                    <td>
                        <BDP:BasicDatePicker ID="tbProductionDate" runat="server" DateFormat="dd MMM yyyy"
                            ReadOnly="true" ValidationGroup="Input" ButtonImageHeight="19px" ButtonImageWidth="20px"
                            DisplayType="TextBoxAndImage" TextBoxStyle-CssClass="TextDate" ShowNoneButton="False">
                            <TextBoxStyle CssClass="TextDate" />
                        </BDP:BasicDatePicker>
                        <asp:DropDownList ValidationGroup="Input" CssClass="DropDownList" ID="ddlShift" runat="server" />
                        <asp:Label ID="Label12" runat="server" ForeColor="#FF3300" Text="*"></asp:Label>
                    </td>
                    <td>
                        Work Hour
                    </td>
                    <td>
                        :
                    </td>
                    <td>
                        <asp:TextBox CssClass="TextBox" runat="server" ID="tbWorkHour" Width="100px" />
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
                        <asp:TextBox ID="tbRemark" runat="server" CssClass="TextBoxMulti" TextMode="MultiLine"
                            ValidationGroup="Input" Width="350px" MaxLength="255" />
                        &nbsp &nbsp &nbsp
                        <asp:Button ID="btnGetDt" runat="server" class="btngo" Text="Get WO" Width="80px"
                            Visible="True" />
                    </td>
                </tr>
            </table>
            <br />
            <hr style="color: Blue" />
            <asp:Menu ID="Menu1" runat="server" CssClass="Menu" StaticMenuItemStyle-CssClass="MenuItem"
                StaticSelectedStyle-CssClass="MenuSelect" Orientation="Horizontal" ItemWrap="False"
                StaticEnableDefaultPopOutImage="False">
                <Items>
                    <asp:MenuItem Text="Detail WO" Value="0"></asp:MenuItem>
                    <%--<asp:MenuItem Text="Detail Material WO" Value="1"></asp:MenuItem>--%>
                    <asp:MenuItem Text="Detail Material Waste" Value="2"></asp:MenuItem>
                    <asp:MenuItem Text="Detail Machine Down Time" Value="3"></asp:MenuItem>
                    <asp:MenuItem Text="Detail Operator" Value="4"></asp:MenuItem>
                </Items>
            </asp:Menu>
            <br />
            <asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex="0">
                <asp:View ID="Tab1" runat="server">
                    <asp:Panel runat="server" ID="PnlDt">
                        <asp:Button class="bitbtn btnadd" runat="server" ID="btnAdddt" Text="Add" Visible="false"
                            ValidationGroup="Input" />
                        <div style="border: 0px  solid; width: 100%; height: 100%; overflow: auto;">
                            <asp:GridView ID="GridDt" runat="server" AutoGenerateColumns="false" ShowFooter="True">
                                <HeaderStyle CssClass="GridHeader" />
                                <RowStyle CssClass="GridItem" Wrap="false" />
                                <AlternatingRowStyle CssClass="GridAltItem" />
                                <PagerStyle CssClass="GridPager" />
                                <Columns>
                                    <asp:TemplateField HeaderText="Action">
                                        <ItemTemplate>
                                            <asp:Button ID="btnEdit" runat="server" class="bitbtndt btnedit" Text="Edit" CommandName="Edit" />
                                            <asp:Button ID="btnDelete" runat="server" class="bitbtndt btndelete" Text="Delete"
                                                OnClientClick="return confirm('Sure to delete this data?');" CommandName="Delete" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField Visible="false">
                                        <ItemTemplate>
                                            <asp:Button ID="btnView" runat="server" class="bitbtndt btndetail" Text="Detail"
                                                CommandName="View" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="WONo" HeaderStyle-Width="100px" HeaderText="WO No" />
                                    <asp:BoundField DataField="ItemNo" HeaderStyle-Width="50px" HeaderText="Item No" />
                                    <asp:BoundField DataField="Product" HeaderStyle-Width="150px" HeaderText="Product" />
                                    <asp:BoundField DataField="Product_Name" HeaderStyle-Width="200px" HeaderText="Product Name" />
                                    <asp:BoundField DataField="LotNo" HeaderStyle-Width="100px" HeaderText="Lot No" />
                                    <asp:BoundField DataField="QtyWO" HeaderStyle-Width="60px" ItemStyle-HorizontalAlign="Right"
                                        HeaderText="Qty WO" />
                                    <asp:BoundField DataField="QtyOutput" HeaderStyle-Width="60px" ItemStyle-HorizontalAlign="Right"
                                        HeaderText="Qty Output" />
                                    <asp:BoundField DataField="QtyOK" HeaderStyle-Width="60px" ItemStyle-HorizontalAlign="Right"
                                        HeaderText="Qty OK" />
                                    <%--<asp:BoundField DataField="QtyRepair" HeaderStyle-Width="60px" HeaderText="Qty Repair" />   --%>
                                    <asp:BoundField DataField="QtyReject" HeaderStyle-Width="60px" ItemStyle-HorizontalAlign="Right"
                                        HeaderText="Qty Reject" />
                                    <asp:BoundField DataField="Unit" HeaderStyle-Width="70px" HeaderText="Unit" />
                                    <asp:BoundField DataField="CauseReject" HeaderStyle-Width="100px" HeaderText="Cause Reject" />
                                    <asp:BoundField DataField="Warehouse" HeaderStyle-Width="100px" HeaderText="Warehouse" />
                                    <asp:BoundField DataField="Warehouse_Name" HeaderStyle-Width="100px" HeaderText="Warehouse Name" />
                                    <asp:BoundField DataField="Location" HeaderStyle-Width="100px" HeaderText="Location" />
                                    <asp:BoundField DataField="Location_Name" HeaderStyle-Width="100px" HeaderText="Location Name" />
                                    <asp:BoundField DataField="Machine" HeaderStyle-Width="100px" HeaderText="Machine" />
                                    <asp:BoundField DataField="Machine_Name" HeaderStyle-Width="100px" HeaderText="Machine Name" />
                                    <%--<asp:BoundField DataField="MachineHour" HeaderStyle-Width="100px" HeaderText="Machine Hour" />   
                            <asp:BoundField DataField="ManPower" HeaderStyle-Width="100px" HeaderText="Man Power" />   --%>
                                    <asp:BoundField DataField="Remark" HeaderStyle-Width="100px" HeaderText="Remark" />
                                </Columns>
                            </asp:GridView>
                        </div>
                        <asp:Button class="bitbtn btnadd" runat="server" ID="btnAddDtke2" Text="Add" Visible="false"
                            ValidationGroup="Input" />
                    </asp:Panel>
                    <asp:Panel runat="server" ID="pnlEditDt" Visible="false">
                        <table>
                            <tr>
                                <td>
                                    WO No
                                </td>
                                <td>
                                    :
                                </td>
                                <td>
                                    <asp:TextBox CssClass="TextBox" ID="tbWONo" runat="server" MaxLength="30" Width="150px"
                                        Enabled="False" />
                                    <asp:Button ID="btnWONo" runat="server" class="btngo" Text="..." />
                                    <asp:Label ID="Label9" runat="server" ForeColor="#FF3300" Text="*"></asp:Label>
                                </td>
                                <td>
                                    Item No
                                </td>
                                <td>
                                    :
                                </td>
                                <td>
                                    <asp:Label ID="lbItemNo" runat="server" Text="" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Product
                                </td>
                                <td>
                                    :
                                </td>
                                <td colspan="4">
                                    <asp:TextBox CssClass="TextBox" Enabled="false" runat="server" ID="tbProductCode"
                                        MaxLength="20" />
                                    <asp:TextBox CssClass="TextBox" runat="server" ID="tbProductName" Enabled="false"
                                        MaxLength="60" Width="225px" />
                                    <%--<asp:Button Class="btngo" ID="btnProduct" Text="..." runat="server" ValidationGroup="Input" />  --%>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Lot No
                                </td>
                                <td>
                                    :
                                </td>
                                <td>
                                    <asp:TextBox CssClass="TextBox" ValidationGroup="Input" runat="server" MaxLength="50"
                                        ID="tbLotNo" Width="200px" />
                                    <asp:Label ID="lbOutputType" runat="server" Text="" Visible="false" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Qty
                                </td>
                                <td>
                                    :
                                </td>
                                <td colspan="4">
                                    <table>
                                        <tr style="background-color: Silver; text-align: center">
                                            <td>
                                                WO
                                            </td>
                                            <td>
                                                Output
                                            </td>
                                            <td>
                                                Good
                                            </td>
                                            <%--<td>Repair</td>--%>
                                            <td>
                                                Reject
                                            </td>
                                            <td>
                                                Unit
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:TextBox CssClass="TextBoxR" runat="server" ID="tbQtyWO" Width="65px" />
                                            </td>
                                            <td>
                                                <asp:TextBox CssClass="TextBoxR" runat="server" ID="tbQtyOutput" Width="65px" />
                                            </td>
                                            <td>
                                                <asp:TextBox CssClass="TextBox" runat="server" ID="tbQtyGood" Width="65px" />
                                            </td>
                                            <%--<td><asp:TextBox CssClass="TextBox" runat="server" ID="tbQtyRepair" Width="65px"/></td>--%>
                                            <td>
                                                <asp:TextBox CssClass="TextBox" runat="server" ID="tbQtyReject" Width="65px" />
                                            </td>
                                            <td>
                                                <asp:DropDownList Enabled="false" CssClass="DropDownList" ID="ddlUnit" runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Penyebab Reject
                                </td>
                                <td>
                                    :
                                </td>
                                <td colspan="4">
                                    <asp:TextBox ID="tbCauseReject" runat="server" CssClass="TextBoxMulti" TextMode="MultiLine"
                                        ValidationGroup="Input" Width="269px" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Warehouse
                                </td>
                                <td>
                                    :
                                </td>
                                <td>
                                    <asp:DropDownList ValidationGroup="Input" CssClass="DropDownList" ID="ddlWarehouse"
                                        runat="server" Width="250px" AutoPostBack="true" />
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
                            <tr>
                                <td>
                                    Location
                                </td>
                                <td>
                                    :
                                </td>
                                <td>
                                    <asp:DropDownList ValidationGroup="Input" CssClass="DropDownList" ID="ddlLocation"
                                        Width="250px" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Machine
                                </td>
                                <td>
                                    :
                                </td>
                                <td>
                                    <asp:DropDownList ValidationGroup="Input" CssClass="DropDownList" ID="ddlMachine"
                                        Width="250px" runat="server" />
                                    <asp:Label ID="Label1" runat="server" ForeColor="#FF3300" Text="*"></asp:Label>
                                </td>
                            </tr>
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
                                <td colspan="4">
                                    <asp:TextBox ID="tbRemarkDt" runat="server" CssClass="TextBoxMulti" 
                                        TextMode="MultiLine" ValidationGroup="Input" Width="269px" />
                                </td>
                            </tr>
                        </table>
                        <br />
                        <asp:Button ID="btnSaveDt" runat="server" class="bitbtndt btnsave" Text="Save" />
                        <asp:Button ID="btnCancelDt" runat="server" class="bitbtndt btncancel" Text="Cancel" />
                    </asp:Panel>
                </asp:View>
                <asp:View ID="Tab2" runat="server">
                    <table>
                        <tr>
                            <td>
                                WO No
                            </td>
                            <td>
                                :
                            </td>
                            <td width="100px">
                                <asp:Label ID="lbWODt2" runat="server" Font-Bold="true" ForeColor="Blue" Text="WONo" />
                            </td>
                        </tr>
                    </table>
                    <asp:Panel ID="pnlDt2" runat="server">
                        <asp:Button class="bitbtn btnadd" runat="server" ID="btnAddDt2" Text="Add" ValidationGroup="Input" />
                        <asp:Button ID="btnBackDt2" runat="server" class="bitbtndt btncancel" Text="Back"
                            ValidationGroup="Input" />
                        <div style="border: 0px  solid; width: 100%; height: 100%; overflow: auto;">
                            <asp:GridView ID="GridDt2" runat="server" AutoGenerateColumns="False" ShowFooter="True">
                                <HeaderStyle CssClass="GridHeader" />
                                <RowStyle CssClass="GridItem" Wrap="false" />
                                <AlternatingRowStyle CssClass="GridAltItem" />
                                <PagerStyle CssClass="GridPager" />
                                <Columns>
                                    <asp:TemplateField HeaderText="Action">
                                        <ItemTemplate>
                                            <asp:Button ID="btnEdit" runat="server" class="bitbtndt btnedit" Text="Edit" CommandName="Edit" />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <%--<asp:BoundField DataField="WONo" HeaderStyle-Width="100px" HeaderText="WO No"  />--%>
                                    <asp:BoundField DataField="Material" HeaderStyle-Width="150px" HeaderText="Material" />
                                    <asp:BoundField DataField="Material_Name" HeaderStyle-Width="200px" HeaderText="Material Name" />
                                    <asp:BoundField DataField="Qty" HeaderStyle-Width="60px" ItemStyle-HorizontalAlign="Right"
                                        HeaderText="Qty" />
                                    <asp:BoundField DataField="Unit" HeaderStyle-Width="150px" HeaderText="Unit" />
                                    <asp:BoundField DataField="Warehouse" HeaderStyle-Width="200px" HeaderText="Warehouse" />
                                    <asp:BoundField DataField="Warehouse_Name" HeaderStyle-Width="100px" HeaderText="Warehouse Name" />
                                    <%--<asp:BoundField DataField="SubLed" HeaderStyle-Width="100px" HeaderText="SubLed" />
                            <asp:BoundField DataField="SubLed_Name" HeaderStyle-Width="150px" HeaderText="SubLed Name" />--%>
                                    <asp:BoundField DataField="Location" HeaderStyle-Width="100px" HeaderText="Location" />
                                    <asp:BoundField DataField="Location_Name" HeaderStyle-Width="150px" HeaderText="Location Name" />
                                    <asp:BoundField DataField="Remark" HeaderStyle-Width="200px" HeaderText="Remark" />
                                </Columns>
                            </asp:GridView>
                        </div>
                        <asp:Button class="bitbtn btnadd" runat="server" ID="btnAddDt2Ke2" Text="Add" ValidationGroup="Input" />
                        <asp:Button ID="btnBackDtke2" runat="server" class="bitbtndt btncancel" Text="Back"
                            ValidationGroup="Input" />
                    </asp:Panel>
                    <asp:Panel runat="server" ID="pnlEditDt2" Visible="false">
                        <table>
                            <%--<tr>
                        <td>WO No</td>
                        <td>:</td>
                        <td colspan="4"><asp:Label ID="lbWONo" runat="server" Text="WO No" />
                        </td>           
                    </tr>      --%>
                            <tr>
                                <td>
                                    Material
                                </td>
                                <td>
                                    :
                                </td>
                                <td colspan="4">
                                    <asp:TextBox CssClass="TextBox" ValidationGroup="Input" runat="server" ID="tbMaterialDt2"
                                        MaxLength="20" AutoPostBack="true" />
                                    <asp:TextBox CssClass="TextBox" runat="server" ID="tbMaterialNameDt2" Enabled="false"
                                        MaxLength="60" Width="225px" />
                                    <asp:Button Class="btngo" ID="btnMaterialDt2" Text="..." runat="server" ValidationGroup="Input" />
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
                                                Qty
                                            </td>
                                            <td>
                                                Unit
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:TextBox CssClass="TextBox" runat="server" ID="tbQtyDt2" Width="65px" />
                                            </td>
                                            <td>
                                                <asp:DropDownList Enabled="false" CssClass="DropDownList" ID="ddlUnitDt2" runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Warehouse
                                </td>
                                <td>
                                    :
                                </td>
                                <td colspan="4">
                                    <asp:DropDownList ID="ddlwrhsDt2" runat="server" AutoPostBack="true" CssClass="DropDownList"
                                        ValidationGroup="Input" Width="200px" />
                                    <asp:TextBox ID="tbFgSubLed" runat="server" Visible="false" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    SubLed
                                </td>
                                <td>
                                    :
                                </td>
                                <td>
                                    <asp:TextBox ID="tbSubLed" runat="server" AutoPostBack="true" CssClass="TextBox"
                                        ValidationGroup="Input" />
                                    <asp:TextBox ID="tbSubLedName" runat="server" CssClass="TextBox" Enabled="false"
                                        Width="225px" />
                                    <asp:Button Class="btngo" ID="btnSubLed" Text="..." runat="server" ValidationGroup="Input" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Location
                                </td>
                                <td>
                                    :
                                </td>
                                <td>
                                    <asp:DropDownList ValidationGroup="Input" CssClass="DropDownList" ID="ddlLocationDt2"
                                        runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Remark
                                </td>
                                <td>
                                    :
                                </td>
                                <td colspan="4">
                                    <asp:TextBox ID="tbRemarkDt2" runat="server" CssClass="TextBoxMulti" TextMode="MultiLine"
                                        ValidationGroup="Input" Width="269px" />
                                </td>
                            </tr>
                        </table>
                        <br />
                        <asp:Button ID="btnSaveDt2" runat="server" class="bitbtndt btnsave" Text="Save" />
                        <asp:Button ID="btnCancelDt2" runat="server" class="bitbtndt btncancel" Text="Cancel" />
                    </asp:Panel>
                </asp:View>
                <asp:View ID="Tab3" runat="server">
                    <asp:Panel ID="pnlDt3" runat="server">
                        <asp:Button class="bitbtn btnadd" runat="server" ID="btnAddDt3" Text="Add" ValidationGroup="Input" />
                        <asp:Button class="btngo" runat="server" ID="btnGetWaste" Text="Get Waste" 
                            ValidationGroup="Input" Visible="True" Width="80px" />
                        <div style="border: 0px  solid; width: 100%; height: 100%; overflow: auto;">
                            <asp:GridView ID="GridDt3" runat="server" AutoGenerateColumns="False" 
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
                                        <EditItemTemplate>
                                            <asp:Button ID="btnUpdate" runat="server" class="bitbtndt btnsave" 
                                                CommandName="Update" Text="Save" />
                                            <asp:Button ID="btnCancel" runat="server" class="bitbtndt btncancel" 
                                                CommandName="Cancel" Text="Cancel" />
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="ItemNo" HeaderText="No" />
                                    <asp:BoundField DataField="Material" HeaderStyle-Width="150px" 
                                        HeaderText="Waste" />
                                    <asp:BoundField DataField="Material_Name" HeaderStyle-Width="200px" 
                                        HeaderText="Waste Name" />
                                    <asp:BoundField DataField="Qty" HeaderStyle-Width="60px" HeaderText="Qty" 
                                        ItemStyle-HorizontalAlign="Right" />
                                    <asp:BoundField DataField="Unit" HeaderStyle-Width="60px" HeaderText="Unit" />
                                    <asp:BoundField DataField="CauseWaste" HeaderStyle-Width="200px" 
                                        HeaderText="Cause Waste" />
                                    <asp:BoundField DataField="Remark" HeaderStyle-Width="200px" 
                                        HeaderText="Remark" />
                                </Columns>
                            </asp:GridView>
                        </div>
                        <asp:Button ID="btnAddDt3ke2" runat="server" class="bitbtn btnadd" Text="Add" 
                            ValidationGroup="Input" />
                    </asp:Panel>
                    <asp:Panel runat="server" ID="pnlEditDt3" Visible="false">
                        <table>
                            <tr>
                                <td>
                                    No
                                </td>
                                <td>
                                    :
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="lbItemDt3" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Waste
                                </td>
                                <td>
                                    :
                                </td>
                                <td colspan="4">
                                    <asp:TextBox CssClass="TextBox" ValidationGroup="Input" runat="server" ID="tbMaterialDt3"
                                        MaxLength="20" AutoPostBack="true" />
                                    <asp:TextBox CssClass="TextBox" runat="server" ID="tbMaterialNameDt3" Enabled="false"
                                        MaxLength="60" Width="225px" />
                                    <asp:Button Class="btngo" ID="btnMaterialDt3" Text="..." runat="server" ValidationGroup="Input" />
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
                                                Qty
                                            </td>
                                            <td>
                                                Unit
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:TextBox CssClass="TextBox" runat="server" ID="tbQtyDt3" Width="65px" />
                                            </td>
                                            <td>
                                                <asp:DropDownList CssClass="DropDownList" ID="ddlUnitDt3" runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Cause Waste
                                </td>
                                <td>
                                    :
                                </td>
                                <td colspan="4">
                                    <asp:TextBox ID="tbCauseWaste" runat="server" CssClass="TextBoxMulti" TextMode="MultiLine"
                                        ValidationGroup="Input" Width="269px" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Remark
                                </td>
                                <td>
                                    :
                                </td>
                                <td colspan="4">
                                    <asp:TextBox ID="tbRemarkDt3" runat="server" CssClass="TextBoxMulti" TextMode="MultiLine"
                                        ValidationGroup="Input" Width="269px" />
                                </td>
                            </tr>
                        </table>
                        <br />
                        <asp:Button ID="btnSaveDt3" runat="server" class="bitbtndt btnsave" Text="Save" />
                        <asp:Button ID="btnCancelDt3" runat="server" class="bitbtndt btncancel" Text="Cancel" />
                    </asp:Panel>
                </asp:View>
                <asp:View ID="Tab4" runat="server">
                    <asp:Panel ID="pnlDt4" runat="server">
                        <asp:Button class="bitbtn btnadd" runat="server" ID="btnAddDt4" Text="Add" ValidationGroup="Input" />
                        <div style="border: 0px  solid; width: 100%; height: 100%; overflow: auto;">
                            <asp:GridView ID="GridDt4" runat="server" AutoGenerateColumns="False" ShowFooter="True">
                                <HeaderStyle CssClass="GridHeader" />
                                <RowStyle CssClass="GridItem" Wrap="false" />
                                <AlternatingRowStyle CssClass="GridAltItem" />
                                <PagerStyle CssClass="GridPager" />
                                <Columns>
                                    <asp:TemplateField HeaderText="Action">
                                        <ItemTemplate>
                                            <asp:Button ID="btnEdit" runat="server" class="bitbtndt btnedit" Text="Edit" CommandName="Edit" />
                                            <asp:Button ID="btnDelete" runat="server" class="bitbtndt btndelete" Text="Delete"
                                                OnClientClick="return confirm('Sure to delete this data?');" CommandName="Delete" />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:Button ID="btnUpdate" runat="server" class="bitbtndt btnsave" Text="Save" CommandName="Update" />
                                            <asp:Button ID="btnCancel" runat="server" class="bitbtndt btncancel" Text="Cancel"
                                                CommandName="Cancel" />
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="ItemNo" HeaderStyle-Width="30px" HeaderText="No" />
                                    <asp:BoundField DataField="Machine" HeaderStyle-Width="150px" HeaderText="Machine" />
                                    <asp:BoundField DataField="Machine_Name" HeaderStyle-Width="200px" HeaderText="Machine Name" />
                                    <asp:BoundField DataField="StartTime" HeaderStyle-Width="80px" HeaderText="Start Time" />
                                    <asp:BoundField DataField="EndTime" HeaderStyle-Width="80px" HeaderText="End Time" />
                                    <asp:BoundField DataField="Duration" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="80px"
                                        HeaderText="Duration (minutes)" />
                                    <asp:BoundField DataField="DownTime" Visible="False" HeaderStyle-Width="100px" HeaderText="Klasifikasi DownTime" />
                                    <asp:BoundField DataField="DownTimeName" HeaderStyle-Width="100px" HeaderText="Klasifikasi DownTime" />
                                    <asp:BoundField DataField="CauseDownTime" HeaderStyle-Width="100px" HeaderText="Penyebab DownTime" />
                                    <asp:BoundField DataField="Remark" HeaderStyle-Width="200px" HeaderText="Remark" />
                                </Columns>
                            </asp:GridView>
                        </div>
                        <asp:Button class="bitbtn btnadd" runat="server" ID="btnAddDt4ke2" Text="Add" ValidationGroup="Input" />
                    </asp:Panel>
                    <asp:Panel runat="server" ID="pnlEditDt4" Visible="false">
                        <table>
                            <tr>
                                <td>
                                    Item No
                                </td>
                                <td>
                                    :
                                </td>
                                <td colspan="4">
                                    <asp:Label ID="lbItemNoDt4" runat="server" Text="itemmm" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Machine
                                </td>
                                <td>
                                    :
                                </td>
                                <td colspan="4">
                                    <asp:TextBox CssClass="TextBox" ValidationGroup="Input" runat="server" ID="tbMachineCodeDt4"
                                        MaxLength="20" AutoPostBack="true" />
                                    <asp:TextBox CssClass="TextBox" runat="server" ID="tbMachineNameDt4" Enabled="false"
                                        MaxLength="60" Width="225px" />
                                    <asp:Button Class="btngo" ID="btnMachineDt4" Text="..." runat="server" ValidationGroup="Input" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Time
                                </td>
                                <td>
                                    :
                                </td>
                                <td colspan="4">
                                    <table>
                                        <tr style="background-color: Silver; text-align: center">
                                            <td>
                                                Start
                                            </td>
                                            <td>
                                                End
                                            </td>
                                            <td>
                                                Duration (minutes)
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:TextBox CssClass="TextBox" ValidationGroup="Input" runat="server" AutoPostBack="True"
                                                    ID="tbStartTime" Width="65px" />
                                            </td>
                                            <td>
                                                <asp:TextBox CssClass="TextBox" runat="server" AutoPostBack="True" ID="tbEndTime"
                                                    Width="65px" />
                                            </td>
                                            <td>
                                                <asp:TextBox CssClass="TextBox" runat="server" ID="tbDuration" Width="100px" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Klasifikasi Down Time
                                </td>
                                <td>
                                    :
                                </td>
                                <td colspan="4">
                                    <asp:TextBox CssClass="TextBox" ValidationGroup="Input" runat="server" ID="tbDownTime"
                                        MaxLength="20" AutoPostBack="true" />
                                    <asp:TextBox CssClass="TextBox" runat="server" ID="tbDownTimeName" Enabled="false"
                                        MaxLength="60" Width="225px" />
                                    <asp:Button Class="btngo" ID="btnDownTime" Text="..." runat="server" ValidationGroup="Input" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Cause DownTime
                                </td>
                                <td>
                                    :
                                </td>
                                <td colspan="4">
                                    <asp:TextBox ID="tbCauseDownTime" runat="server" CssClass="TextBoxMulti" TextMode="MultiLine"
                                        ValidationGroup="Input" Width="269px" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Remark
                                </td>
                                <td>
                                    :
                                </td>
                                <td colspan="4">
                                    <asp:TextBox ID="tbRemarkDt4" runat="server" CssClass="TextBoxMulti" TextMode="MultiLine"
                                        ValidationGroup="Input" Width="269px" />
                                </td>
                            </tr>
                        </table>
                        <br />
                        <asp:Button ID="btnSaveDt4" runat="server" class="bitbtndt btnsave" Text="Save" />
                        <asp:Button ID="btnCancelDt4" runat="server" class="bitbtndt btncancel" Text="Cancel" />
                    </asp:Panel>
                </asp:View>
                <asp:View ID="Tab5" runat="server">
                    <asp:Panel ID="pnlDt5" runat="server">
                        <asp:Button class="bitbtn btnadd" runat="server" ID="btnAddDt5" Text="Add" ValidationGroup="Input" />
                        <asp:Button ID="btnGetOperator" runat="server" class="btngo" Text="Get Operator"
                            Width="80px" ValidationGroup="Input" Visible="True" />
                        <div style="border: 0px  solid; width: 100%; height: 100%; overflow: auto;">
                            <asp:GridView ID="GridDt5" runat="server" AutoGenerateColumns="False" ShowFooter="True">
                                <HeaderStyle CssClass="GridHeader" />
                                <RowStyle CssClass="GridItem" Wrap="false" />
                                <AlternatingRowStyle CssClass="GridAltItem" />
                                <PagerStyle CssClass="GridPager" />
                                <Columns>
                                    <asp:TemplateField HeaderText="Action">
                                        <ItemTemplate>
                                            <%--<asp:Button ID="btnEdit" runat="server" class="bitbtndt btnedit" Text="Edit" CommandName="Edit"/>--%>
                                            <asp:Button ID="btnDelete" runat="server" class="bitbtndt btndelete" Text="Delete"
                                                OnClientClick="return confirm('Sure to delete this data?');" CommandName="Delete" />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:Button ID="btnUpdate" runat="server" class="bitbtndt btnsave" Text="Save" CommandName="Update" />
                                            <asp:Button ID="btnCancel" runat="server" class="bitbtndt btncancel" Text="Cancel"
                                                CommandName="Cancel" />
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="EmpNumb" HeaderStyle-Width="150px" HeaderText="Operator" />
                                    <asp:BoundField DataField="Emp_Name" HeaderStyle-Width="200px" HeaderText="Operator Name" />
                                    <asp:BoundField DataField="JobTitle" HeaderStyle-Width="150px" HeaderText="Job Title" />
                                    <asp:BoundField DataField="JobTtlName" HeaderStyle-Width="200px" HeaderText="Job Title Name" />
                                </Columns>
                            </asp:GridView>
                        </div>
                        <asp:Button class="bitbtn btnadd" runat="server" ID="btnAddDt5ke2" Text="Add" ValidationGroup="Input" />
                    </asp:Panel>
                    <asp:Panel runat="server" ID="pnlEditDt5" Visible="false">
                        <table>
                            <tr>
                                <td>
                                    Operator
                                </td>
                                <td>
                                    :
                                </td>
                                <td colspan="4">
                                    <asp:TextBox CssClass="TextBox" ValidationGroup="Input" runat="server" ID="tbEmp"
                                        MaxLength="20" AutoPostBack="true" />
                                    <asp:TextBox CssClass="TextBox" runat="server" ID="tbEmpName" Enabled="false" MaxLength="60"
                                        Width="225px" />
                                    <asp:Button Class="btngo" ID="btnEmp" Text="..." runat="server" ValidationGroup="Input" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Job Title
                                </td>
                                <td>
                                    :
                                </td>
                                <td colspan="4">
                                    <asp:TextBox ID="tbJobTitle" runat="server" CssClass="TextBox" Enabled="false" />
                                    <asp:TextBox ID="tbJobTitleName" runat="server" CssClass="TextBox" Enabled="false"
                                        Width="225" />
                                </td>
                            </tr>
                        </table>
                        <br />
                        <asp:Button ID="btnSaveDt5" runat="server" class="bitbtndt btnsave" Text="Save" />
                        <asp:Button ID="btnCancelDt5" runat="server" class="bitbtndt btncancel" Text="Cancel" />
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
        <asp:Panel runat="server" ID="pnlLotNo" Visible="false">
            <table>
                <tr>
                    <td>
                        LHP No
                    </td>
                    <td>
                        :
                    </td>
                    <td>
                        <asp:TextBox CssClass="TextBox" runat="server" ID="tbLotLHPNo" Width="150px" Enabled="False" />
                    </td>
                    <td>
                        Input Date
                    </td>
                    <td>
                        :
                    </td>
                    <td>
                        <BDP:BasicDatePicker ID="tbLotLHPDate" runat="server" DateFormat="dd MMM yyyy" ReadOnly="true"
                            ValidationGroup="Input" Enabled="false" ButtonImageHeight="19px" ButtonImageWidth="20px"
                            DisplayType="TextBoxAndImage" TextBoxStyle-CssClass="TextDate" ShowNoneButton="False">
                            <TextBoxStyle CssClass="TextDate" />
                        </BDP:BasicDatePicker>
                    </td>
                </tr>
            </table>
            <br />
            <div style="font-size: medium; color: Blue;">
                Detail</div>
            <hr style="color: Blue" />
            <asp:Panel runat="server" ID="pnlLotNoDetail">
                <table>
                    <tr>
                        <td>
                            WO No & Item No
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                            <asp:DropDownList ValidationGroup="Input" CssClass="DropDownList" ID="ddlLotWOItemNo"
                                runat="server" AutoPostBack="True" Width="250px" />
                            <asp:Label ID="lbLotWONo" runat="server" Visible="false" Text="*" />
                            <asp:Label ID="lbLotItemNo" runat="server" Visible="false" Text="*" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Product
                        </td>
                        <td>
                            :
                        </td>
                        <td colspan="4">
                            <asp:TextBox CssClass="TextBox" Enabled="false" runat="server" ID="tbLotProductCode"
                                MaxLength="20" />
                            <asp:TextBox CssClass="TextBox" runat="server" ID="tbLotProductName" Enabled="false"
                                MaxLength="60" Width="225px" />
                            <%--<asp:Button Class="btngo" ID="btnProduct" Text="..." runat="server" ValidationGroup="Input" />  --%>
                        </td>
                        <tr>
                            <td>
                                Warehouse
                            </td>
                            <td>
                                :
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlLotWrhs" runat="server" CssClass="DropDownList" 
                                    Enabled="false" ValidationGroup="Input" Width="250px" />
                            </td>
                            <tr>
                                <td>
                                    Qty
                                </td>
                                <td>
                                    :
                                </td>
                                <td colspan="4">
                                    <table>
                                        <tr style="background-color: Silver; text-align: center">
                                            <td>
                                                Good
                                            </td>
                                            <td>
                                                Reject
                                            </td>
                                            <td>
                                                Unit
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="tbLotQtyOK" runat="server" CssClass="TextBoxR" Width="65px" />
                                            </td>
                                            <td>
                                                <asp:TextBox ID="tbLotQtyReject" runat="server" CssClass="TextBoxR" 
                                                    Width="65px" />
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlLotUnit" runat="server" CssClass="DropDownList" 
                                                    Enabled="false" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                </table>
            </asp:Panel>
            <asp:Menu ID="Menu2" runat="server" CssClass="Menu" StaticMenuItemStyle-CssClass="MenuItem"
                StaticSelectedStyle-CssClass="MenuSelect" Orientation="Horizontal" ItemWrap="False"
                StaticEnableDefaultPopOutImage="False">
                <Items>
                    <asp:MenuItem Text="USE" Value="0"></asp:MenuItem>
                    <asp:MenuItem Text="OUTPUT" Value="1"></asp:MenuItem>
                </Items>
            </asp:Menu>
            <br />
            <asp:MultiView ID="MultiView2" runat="server" ActiveViewIndex="0">
                <br />
                <asp:View ID="View1" runat="server">
                    <asp:Panel ID="pnlUse" runat="server">
                        <asp:GridView ID="GridRollUse" runat="server" ShowFooter="True" AllowSorting="True"
                            AutoGenerateColumns="False" AllowPaging="True" CssClass="Grid">
                            <HeaderStyle CssClass="GridHeader"></HeaderStyle>
                            <RowStyle CssClass="GridItem" Wrap="false" />
                            <AlternatingRowStyle CssClass="GridAltItem" />
                            <FooterStyle CssClass="GridFooter" />
                            <PagerStyle CssClass="GridPager" HorizontalAlign="Center" />
                            <EmptyDataTemplate>
                            </EmptyDataTemplate>
                            <Columns>
                                <asp:TemplateField HeaderText="Roll No" HeaderStyle-Width="100" SortExpression="RollNo">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="RollNo" Text='<%# DataBinder.Eval(Container.DataItem, "RollNo") %>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox runat="server" ID="RollNoEdit" MaxLength="20" Width="100%" CssClass="TextBox"
                                            Text='<%# DataBinder.Eval(Container.DataItem, "RollNo") %>'>
                                        </asp:TextBox>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="RollNoAdd" CssClass="TextBox" MaxLength="20" runat="Server" Width="100%" />
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Action" HeaderStyle-Width="126">
                                    <ItemTemplate>
                                        <asp:Button class="bitbtn btnedit" runat="server" ID="btnEdit" Text="Edit" CommandName="Edit" />
                                        <asp:Button class="bitbtn btndelete" runat="server" ID="btnDelete" Text="Delete"
                                            CommandName="Delete" OnClientClick="return confirm('Sure to delete this data?');" />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Button class="bitbtn btnsave" runat="server" ID="btnUpdate" Text="Save" CommandName="Update" />
                                        <asp:Button class="bitbtn btncancel" runat="server" ID="btnCancel" Text="Cancel"
                                            CommandName="Cancel" />
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:Button class="bitbtn btnadd" runat="server" ID="btnAdd" Text="Add" CommandName="Insert" />
                                    </FooterTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </asp:Panel>
                </asp:View>
                <asp:View ID="View2" runat="server">
                    <asp:Panel ID="Panel1" runat="server">
                        <table>
                            <tr>
                                <td>
                                    <fieldset style="width: 700px">
                                        <legend>&nbsp;Set Lot No&nbsp;</legend>
                                        <table>
                                            <tr>
                                                <td>
                                                    Size
                                                </td>
                                                <td>
                                                    :
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="tbGenerateSize" runat="server" CssClass="TextBox" Width="44px" />&nbsp;mm
                                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;GSM :
                                                    <asp:TextBox ID="tbGenerateGSM" runat="server" CssClass="TextBox" Width="58px" />
                                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Length :
                                                    <asp:TextBox ID="tbGenerateLength" runat="server" CssClass="TextBox" Width="98px" />&nbsp;m
                                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Weight :
                                                    <asp:TextBox ID="tbGenerateWeight" runat="server" CssClass="TextBox" Width="58px" />&nbsp;Kg
                                                    &nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    Prefix
                                                </td>
                                                <td>
                                                    :
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="tbGeneratePrefix" runat="server" CssClass="TextBox" Width="44px" />
                                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Sufix :
                                                    <asp:TextBox ID="tbGenerateSufix" runat="server" CssClass="TextBox" Width="98px" />
                                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Digit Lot :
                                                    <asp:TextBox ID="tbGeneretDigit" runat="server" CssClass="TextBox" Width="58px" />
                                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Lot No :
                                                    <asp:TextBox ID="tbGenerateNoStart" runat="server" CssClass="TextBox" Width="58px" />
                                                    &nbsp;s/d
                                                    <asp:TextBox ID="tbGenerateNoEnd" runat="server" CssClass="TextBox" Width="58px" />
                                                    &nbsp;&nbsp;&nbsp;&nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    Use Roll No
                                                </td>
                                                <td>
                                                    :
                                                </td>
                                                <td>
                                                    <asp:DropDownList CssClass="DropDownList" ID="ddlUseRollNo" runat="server" />
                                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Status :
                                                    <asp:DropDownList ID="ddlStatus" runat="server" CssClass="DropDownList" Width="100px">
                                                        <asp:ListItem>Good</asp:ListItem>
                                                        <asp:ListItem>Reject</asp:ListItem>
                                                        <asp:ListItem>Blok</asp:ListItem>
                                                    </asp:DropDownList>
                                                    <asp:Button ID="btnGenerate" runat="server" class="bitbtn btngetitem" Text="Generate" />
                                                    Show Records :
                                                    <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlShowRecord2" AutoPostBack="true">
                                                        <asp:ListItem Selected="True" Value="15">Choose One</asp:ListItem>
                                                        <asp:ListItem Value="20">20</asp:ListItem>
                                                        <asp:ListItem Value="30">30</asp:ListItem>
                                                        <asp:ListItem Value="40">40</asp:ListItem>
                                                        <asp:ListItem Value="50">50</asp:ListItem>
                                                        <asp:ListItem Value="100">100</asp:ListItem>
                                                        <asp:ListItem Value="200">200</asp:ListItem>
                                                        <asp:ListItem Value="500">500</asp:ListItem>
                                                    </asp:DropDownList>
                                                    Rows
                                                </td>
                                            </tr>
                                        </table>
                                    </fieldset>
                                </td>
                                <td>
                                    <table>
                                        <tr style="background-color: Silver; text-align: center">
                                            <td>
                                                Good
                                            </td>
                                            <td>
                                                Reject
                                            </td>
                                            <td>
                                                Blok
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="tbGood" runat="server" CssClass="TextBoxR" Width="65px" />
                                            </td>
                                            <td>
                                                <asp:TextBox ID="tbReject" runat="server" CssClass="TextBoxR" 
                                                    Width="65px" />
                                            </td>
                                            <td>
                                                <asp:TextBox ID="tbBlok" runat="server" CssClass="TextBoxR" 
                                                    Width="65px" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                        <br />
                        <asp:Label ID="Label2" runat="server" Font-Bold="True" ForeColor="Blue" Text="Delete Selected Item"></asp:Label>
                        &nbsp;&nbsp;<%--<asp:Button ID="btnProcessDel" runat="server" 
                            Class="bitbtndt btngo" Text="Process" ValidationGroup="Input" Width="70" />--%>
                        <asp:Button ID="btnProcessDel" runat="server" class="bitbtndt btndelete" Text="Delete"
                            OnClientClick="return confirm('Sure to delete this data?');" CommandName="Delete" />
                        &nbsp;&nbsp;&nbsp
                        <asp:Button ID="btnApply" runat="server" class="bitbtn btnsave" Text="Apply" />

                        <br>
                        <asp:GridView ID="GridRollOutput" runat="server" ShowFooter="True" AllowSorting="True"
                            AutoGenerateColumns="False" AllowPaging="True" CssClass="Grid">
                            <HeaderStyle CssClass="GridHeader"></HeaderStyle>
                            <RowStyle CssClass="GridItem" Wrap="false" />
                            <AlternatingRowStyle CssClass="GridAltItem" />
                            <FooterStyle CssClass="GridFooter" />
                            <PagerStyle CssClass="GridPager" HorizontalAlign="Center" />
                            <EmptyDataTemplate>
                            </EmptyDataTemplate>
                            <Columns>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:CheckBox ID="cbSelectHd" runat="server" AutoPostBack="true" OnCheckedChanged="cbSelectHd_CheckedChanged1" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="cbSelect" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="No" HeaderStyle-Width="50" SortExpression="RollNo">
                                    <ItemTemplate>
                                        <%--<asp:Label runat="server" ID="NoUrut" Text='<%# DataBinder.Eval(Container.DataItem, "NoUrut") %>'>
                                        </asp:Label>--%>
                                        <asp:Label runat="server" ID="NoUrutEdit" Text='<%# DataBinder.Eval(Container.DataItem, "NoUrut") %>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                    <%--<EditItemTemplate>
                                        <asp:Label runat="server" ID="NoUrutEdit" Text='<%# DataBinder.Eval(Container.DataItem, "NoUrut") %>'>
                                        </asp:Label>
                                    </EditItemTemplate>--%>
                                    <FooterTemplate>
                                        <asp:Label runat="server" ID="NoUrutAdd" />
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Roll No" HeaderStyle-Width="100" SortExpression="RollNo">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="RollNo" Text='<%# DataBinder.Eval(Container.DataItem, "RollNo") %>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                    <%--<EditItemTemplate>
                                        <asp:TextBox runat="server" ID="RollNoEdit" MaxLength="20" Width="100%" CssClass="TextBox"
                                            Text='<%# DataBinder.Eval(Container.DataItem, "RollNo") %>'>
                                        </asp:TextBox>
                                    </EditItemTemplate>--%>
                                    <FooterTemplate>
                                        <asp:Label runat="server" ID="RollNoAdd" />
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Length (m)" HeaderStyle-Width="100" SortExpression="Length">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="Length" Text='<%# DataBinder.Eval(Container.DataItem, "Length") %>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                    <%--<EditItemTemplate>
                                        <asp:TextBox runat="server" ID="LengthEdit" MaxLength="20" Width="100%" CssClass="TextBox"
                                            Text='<%# DataBinder.Eval(Container.DataItem, "Length") %>'>
                                        </asp:TextBox>
                                    </EditItemTemplate>--%>
                                    <FooterTemplate>
                                        <asp:Label runat="server" ID="LengthAdd" />
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Size (mm)" HeaderStyle-Width="100" SortExpression="Width">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="Width" Text='<%# DataBinder.Eval(Container.DataItem, "Width") %>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                    <%--<EditItemTemplate>
                                        <asp:TextBox runat="server" ID="WidthEdit" MaxLength="20" Width="100%" CssClass="TextBox"
                                            Text='<%# DataBinder.Eval(Container.DataItem, "Width") %>'>
                                        </asp:TextBox>
                                    </EditItemTemplate>--%>
                                    <FooterTemplate>
                                        <asp:Label runat="server" ID="WidthAdd" />
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="GSM" HeaderStyle-Width="100" SortExpression="GSM">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="GSM" Text='<%# DataBinder.Eval(Container.DataItem, "GSM") %>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                    <%--<EditItemTemplate>
                                        <asp:TextBox runat="server" ID="GSMEdit" MaxLength="20" Width="100%" CssClass="TextBox"
                                            Text='<%# DataBinder.Eval(Container.DataItem, "GSM") %>'>
                                        </asp:TextBox>
                                    </EditItemTemplate>--%>
                                    <FooterTemplate>
                                        <asp:Label runat="server" ID="GSMAdd" />
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Weight (Kg)" HeaderStyle-Width="100" SortExpression="WeightRoll">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="WeightRoll" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "WeightRoll") %>'>
                                        </asp:Label>
                                        <asp:TextBox runat="server" ID="WeightRollEdit" MaxLength="20" Width="100%" CssClass="TextBox"
                                            Text='<%# DataBinder.Eval(Container.DataItem, "WeightRoll") %>'>
                                        </asp:TextBox>
                                    </ItemTemplate>
                                    <%--<EditItemTemplate>
                                        <asp:TextBox runat="server" ID="WeightRollEdit" MaxLength="20" Width="100%" CssClass="TextBox"
                                            Text='<%# DataBinder.Eval(Container.DataItem, "WeightRoll") %>'>
                                        </asp:TextBox>
                                    </EditItemTemplate>--%>
                                    <FooterTemplate>
                                        <asp:Label runat="server" ID="WeightRollAdd" />
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Use Roll No" HeaderStyle-Width="100" SortExpression="UseRollNo">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="UseRollNo" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "UseRollNo") %>'>
                                        </asp:Label>
                                        <asp:DropDownList runat="server" ID="UseRollNoEdit" Width="100%" CssClass="DropDownList">
                                        </asp:DropDownList>
                                        <%--SelectedValue='<%# DataBinder.Eval(Container.DataItem, "UseRollNo") %>' DataSourceID="dsUseRollNo"
                                            DataTextField="RollNo" DataValueField="RollNoCode">--%>
                                    </ItemTemplate>
                                    <%--<EditItemTemplate>
                                        <asp:DropDownList runat="server" ID="UseRollNoEdit" Width="100%" CssClass="DropDownList"
                                            SelectedValue='<%# DataBinder.Eval(Container.DataItem, "UseRollNo") %>' DataSourceID="dsUseRollNo"
                                            DataTextField="RollNo" DataValueField="RollNoCode">
                                        </asp:DropDownList>
                                    </EditItemTemplate>--%>
                                    <FooterTemplate>
                                        <asp:DropDownList ID="UseRollNoAdd" runat="Server" Width="100%" CssClass="DropDownList">
                                        </asp:DropDownList>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Status" HeaderStyle-Width="100" SortExpression="Status">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="Status" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "Status") %>'>
                                        </asp:Label>
                                        <asp:DropDownList runat="server" ID="StatusEdit" Width="100%" CssClass="DropDownList"> <%--SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Status") %>'--%>
                                            <asp:ListItem Selected="True">Good</asp:ListItem>
                                            <asp:ListItem>Reject</asp:ListItem>
                                            <asp:ListItem>Blok</asp:ListItem>
                                        </asp:DropDownList>
                                    </ItemTemplate>
                                    <%--<EditItemTemplate>
                                        <asp:DropDownList runat="server" ID="StatusEdit" Width="100%" CssClass="DropDownList"
                                            SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Status") %>'>
                                            <asp:ListItem Selected="True">Good</asp:ListItem>
                                            <asp:ListItem>Reject</asp:ListItem>
                                            <asp:ListItem>Blok</asp:ListItem>
                                        </asp:DropDownList>
                                    </EditItemTemplate>--%>
                                    <FooterTemplate>
                                        <asp:DropDownList ID="StatusAdd" runat="Server" Width="100%" CssClass="DropDownList">
                                            <asp:ListItem Selected="True">Good</asp:ListItem>
                                            <asp:ListItem>Reject</asp:ListItem>
                                            <asp:ListItem>Blok</asp:ListItem>
                                        </asp:DropDownList>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Action">
                                    <ItemTemplate>
                                        <%--<asp:Button class="bitbtn btnedit" runat="server" ID="btnEdit" Text="Edit" CommandName="Edit" />--%>
                                        <asp:Button class="bitbtn btndelete" runat="server" ID="btnDelete" Text="Delete"
                                            CommandName="Delete" OnClientClick="return confirm('Sure to delete this data?');" />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <%--<asp:Button class="bitbtn btnsave" runat="server" ID="btnUpdate" Text="Save" CommandName="Update" />--%>
                                        <asp:Button class="bitbtn btncancel" runat="server" ID="btnCancel" Text="Cancel"
                                            CommandName="Cancel" />
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:Button class="bitbtn btnadd" runat="server" ID="btnAdd" Text="Add" CommandName="Insert" />
                                    </FooterTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </asp:Panel>
                </asp:View>
            </asp:MultiView>
            <asp:Button ID="btnBackLot" runat="server" class="bitbtndt btncancel" Text="Cancel"
                ValidationGroup="Input" />
        </asp:Panel>
    </div>
    <%--<td><asp:TextBox CssClass="TextBox" runat="server" ID="tbQtyRepair" Width="65px"/></td>--%>
    <asp:Label runat="server" ID="lbStatus" ForeColor="Red" />
    </form>
</body>
</html>
