<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TrPOImportStatusV2.aspx.vb"
    Inherits="Transaction_TrPOImportStatusV2_TrPOImportStatusV2" %>

<%@ Register Assembly="BasicFrame.WebControls.BasicDatePicker" Namespace="BasicFrame.WebControls"
    TagPrefix="BDP" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Supplier Begin</title>

    <script src="../../Function/OpenDlg.js" type="text/javascript"></script>
    <script src="../../Function/Function.JS" type="text/javascript"></script>
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
 <link href="https://fonts.googleapis.com/css2?family=Muli:wght@300;400;600&display=swap" rel="stylesheet">

    <script type="text/javascript">
        function setdigit(nStr, digit)
        {
        try
        {
        var TNstr = parseFloat(nStr);        
        TNstr = TNstr.toFixed(digit);                
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
        
        function setformat(){   
          try
          {
              var _tempbaseforex = parseFloat(document.getElementById("tbTotalContainer").value.replace(/\$|\,/g, ""));
            
            if(isNaN(_tempbaseforex) == true)
            {
               _tempbaseforex = 0;
            }            
                        
            document.getElementById("tbTotalContainer").value = setdigit(_tempbaseforex, 0);            
            
          }catch (err){
            alert(err.description);
          }
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="Content">
        <div class="H1">
            PO Import Status</div>
        <hr style="color: Blue" />
        <asp:Panel runat="server" ID="PnlHd">
            <table>
                <tr>
                    <td style="width: 100px; text-align: right">
                        Quick Search :
                    </td>
                    <td>
                        <asp:TextBox CssClass="TextBox" runat="server" ID="tbFilter" />
                        <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlField">
                            <asp:ListItem Value="TransNmbr" Selected="True">Trans. No</asp:ListItem>
                            <asp:ListItem Value="Revisi">Revisi</asp:ListItem>
                            <asp:ListItem Value="TransDate">Trans. Date</asp:ListItem>
                            <asp:ListItem Value="Supplier">Supplier Code</asp:ListItem>
                            <asp:ListItem Value="Supplier_Name">Supplier Name</asp:ListItem>
                            <%--<asp:ListItem Value="DRDateWrhs">Delivery Request Wrhs Date</asp:ListItem>
                            <asp:ListItem Value="DRDateETA">Delivery Request ETA</asp:ListItem>
                            <asp:ListItem Value="DRDateETD">Delivery Request ETD</asp:ListItem>
                            <asp:ListItem Value="ShipDateETD">Actual Shipment ETD</asp:ListItem>
                            <asp:ListItem Value="ShipDateETA">Actual Shipment ETA</asp:ListItem>
                            <asp:ListItem Value="ShipDateCustom">Actual Shipment F Customs</asp:ListItem>
                            <asp:ListItem Value="DeliveryInLand">InLand Delivery</asp:ListItem>
                            <asp:ListItem Value="ContainerNo">Container No</asp:ListItem>
                            <asp:ListItem Value="TotalContainer">Total Container</asp:ListItem>
                            <asp:ListItem Value="InvoiceNo">Invoice No</asp:ListItem>
                            <asp:ListItem Value="BLNo">Bill of Lading No</asp:ListItem>
                            <asp:ListItem Value="AJUNo">AJU No</asp:ListItem>
                            <asp:ListItem Value="Respon">Customs Response</asp:ListItem>--%>
                        </asp:DropDownList>
                        <asp:Button class="bitbtn btnsearch" runat="server" ID="btnSearch" Text="Search" />
                        <asp:Button class="bitbtn btngo" runat="server" ID="btnExpand" Text="..." />
                    </td>
                    <td>
                        <asp:LinkButton ID="lkbAdvanceSearch" runat="server" Text="Advanced Search" />
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
                                <asp:ListItem Value="TransNmbr" Selected="True">Trans. No</asp:ListItem>
                                <asp:ListItem Value="Revisi">Revisi</asp:ListItem>
                                <asp:ListItem Value="TransDate">Trans. Date</asp:ListItem>
                                <asp:ListItem Value="Supplier">Supplier Code</asp:ListItem>
                                <asp:ListItem Value="Supplier_Name">Supplier Name</asp:ListItem>
                                <%--<asp:ListItem Value="DRDateWrhs">Delivery Request Wrhs Date</asp:ListItem>
                                <asp:ListItem Value="DRDateETA">Delivery Request ETA</asp:ListItem>
                                <asp:ListItem Value="DRDateETD">Delivery Request ETD</asp:ListItem>
                                <asp:ListItem Value="ShipDateETD">Actual Shipment ETD</asp:ListItem>
                                <asp:ListItem Value="ShipDateETA">Actual Shipment ETA</asp:ListItem>
                                <asp:ListItem Value="ShipDateCustom">Actual Shipment F Customs</asp:ListItem>
                                <asp:ListItem Value="DeliveryInLand">InLand Delivery</asp:ListItem>
                                <asp:ListItem Value="ContainerNo">Container No</asp:ListItem>
                                <asp:ListItem Value="TotalContainer">Total Container</asp:ListItem>
                                <asp:ListItem Value="InvoiceNo">Invoice No</asp:ListItem>
                                <asp:ListItem Value="BLNo">Bill of Lading No</asp:ListItem>
                                <asp:ListItem Value="AJUNo">AJU No</asp:ListItem>
                                <asp:ListItem Value="Respon">Customs Response</asp:ListItem>--%>
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <br />
            <%--<asp:Button class="bitbtn btnadd" runat="server" ID="BtnAdd" Text="Add" />	--%>
            &nbsp &nbsp &nbsp
            <%--<asp:DropDownList CssClass="DropDownList" ID="ddlCommand" runat="server" Visible="false"/>
            <asp:Button class="btngo" runat="server" ID="BtnGo" Text="G" Visible="false" />           --%>
            <br />
            <div style="border: 0px  solid; height: 100%; overflow: auto;">
                <asp:GridView ID="GridView1" runat="server" AllowPaging="True" AllowSorting="true"
                    AutoGenerateColumns="false" CssClass="Grid">
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
                        <asp:TemplateField ItemStyle-Width="80px">
                            <ItemTemplate>
                                <asp:DropDownList CssClass="DropDownList" ID="ddl" runat="server">
                                    <asp:ListItem Selected="True" Text="View" />
                                    <asp:ListItem Text="Edit" />
                                    <%--<asp:ListItem Text="Delete" />--%>
                                    <%--<asp:ListItem Text="Print" />--%>
                                </asp:DropDownList>
                                <asp:Button class="btngo" runat="server" ID="BtnGoDt" Text="G" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>"
                                    CommandName="Go" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="Trans. No." DataField="TransNmbr" HeaderStyle-Width="100px"
                            SortExpression="Nmbr" />
                        <asp:BoundField HeaderText="Revisi" DataField="Revisi" HeaderStyle-Width="10px" SortExpression="Status" />
                        <asp:BoundField HeaderText="Trans. Date" DataField="TransDate" DataFormatString="{0:dd MMM yyyy}"
                            HeaderStyle-Width="80px" SortExpression="TransDate" />
                        <asp:BoundField HeaderText="Supplier" DataField="Supplier" HeaderStyle-Width="100px"
                            SortExpression="Supplier" />
                        <asp:BoundField HeaderText="Supplier Name" DataField="Supplier_Name" HeaderStyle-Width="400px"
                            SortExpression="Supplier_Name" />
                        <%--<asp:BoundField HeaderText="Delivery Request Warehouse Date" dataformatstring="{0:dd MMM yyyy}" DataField="DRDateWrhs" HeaderStyle-Width="80px" SortExpression="DRDateWrhs" />   
				  <asp:BoundField HeaderText="Delivery Request ETA" dataformatstring="{0:dd MMM yyyy}" DataField="DRDateETA" HeaderStyle-Width="80px" SortExpression="DRDateETA" />   				  
				  <asp:BoundField HeaderText="Delivery Request ETD" dataformatstring="{0:dd MMM yyyy}" DataField="DRDateETD" HeaderStyle-Width="80px" SortExpression="DRDateETD" />   
				  <asp:BoundField HeaderText="Actual Shipment ETD" dataformatstring="{0:dd MMM yyyy}" DataField="ShipDateETD" HeaderStyle-Width="80px" SortExpression="ShipDateETD" />   
				  <asp:BoundField HeaderText="Actual Shipment ETA" dataformatstring="{0:dd MMM yyyy}" DataField="ShipDateETA" HeaderStyle-Width="80px" SortExpression="ShipDateETA" />   				  
				  <asp:BoundField HeaderText="Actual Shipment F.Customs" dataformatstring="{0:dd MMM yyyy}" DataField="ShipDateCustom" HeaderStyle-Width="80px" SortExpression="ShipDateCustom" />   
				  <asp:BoundField HeaderText="InLand Delivery" DataField="DeliveryInLand" HeaderStyle-Width="80px" SortExpression="DeliveryInLand" />				  
				  <asp:BoundField HeaderText="Invoice No" DataField="InvoiceNo" HeaderStyle-Width="80px" SortExpression="InvoiceNo" />
				  <asp:BoundField HeaderText="Bill of Lading No" DataField="BLNo" HeaderStyle-Width="80px" SortExpression="BLNo" />
				  <asp:BoundField HeaderText="Container No" DataField="ContainerNo" HeaderStyle-Width="80px" SortExpression="ContainerNo" />
				  <asp:BoundField DataField="TotalContainer" DataFormatString="{0:#,##0}" ItemStyle-HorizontalAlign="Right"  HeaderStyle-Width="80px" SortExpression="TotalContainer" HeaderText="Total Container"/>
				  <asp:BoundField HeaderText="AJU No" DataField="AJUNo" HeaderStyle-Width="80px" SortExpression="AJUNo" />
				  <asp:BoundField HeaderText="Customs Response" DataField="Respon" HeaderStyle-Width="80px" SortExpression="Respon" />--%>
                    </Columns>
                </asp:GridView>
            </div>
            <asp:Panel runat="server" ID="pnlNav" Visible="false">
                <%--<asp:Button class="bitbtn btnadd" runat="server" ID="btnAdd2" Text="Add" />	--%>
                &nbsp &nbsp &nbsp
                <%--<asp:DropDownList ID="ddlCommand2" CssClass="DropDownList" runat="server"/>
            <asp:Button class="btngo" runat="server" ID="btnGo2" Text="G"  />           --%>
            </asp:Panel>
        </asp:Panel>
        <asp:Panel ID="pnlInput" DefaultButton="btnSave" runat="server" Visible="false">
            <table>
                <tr>
                    <td>
                        Trans No / Revisi
                    </td>
                    <td>
                        :
                    </td>
                    <td>
                        <asp:TextBox ID="tbInvoiceNo" MaxLength="20" Enabled="false" runat="server" CssClass="TextBox" />
                        <asp:TextBox ID="tbRevisi" MaxLength="20" Width="20" Enabled="false" runat="server"
                            CssClass="TextBox" />
                    </td>
                    &nbsp &nbsp &nbsp
                    <td>
                        Trans. Date
                    </td>
                    <td>
                        :
                    </td>
                    <td>
                        <BDP:BasicDatePicker ID="tbTransDate" runat="server" DateFormat="dd MMM yyyy" ReadOnly="true"
                            ShowNoneButton="false" Enabled="false" ButtonImageHeight="19px" ButtonImageWidth="20px"
                            DisplayType="TextBox" TextBoxStyle-CssClass="TextDate" AutoPostBack="True">
                            <TextBoxStyle CssClass="TextDate" />
                        </BDP:BasicDatePicker>
                    </td>
                </tr>
                <tr>
                    <td>
                        Supplier
                    </td>
                    <td>
                        :
                    </td>
                    <td colspan="4">
                        <asp:TextBox ID="tbSuppCode" Width="100" runat="server" Enabled="false" CssClass="TextBox" />
                        <asp:TextBox ID="tbSuppName" Width="245" runat="server" Enabled="false" CssClass="TextBox" />
                    </td>
                </tr>
                
                <tr>
                    <td colspan="6">
                        <asp:Button ID="btnSave" runat="server" class="bitbtndt btnsave" Text="Save" ValidationGroup="Input" />
                        <%--<asp:Button ID="btnCancel" runat="server" class="bitbtndt btncancel" Text="Cancel"
                            ValidationGroup="Input" />--%>
                        <%--<asp:Button ID="btnHome" runat="server" class="bitbtndt btnback" Text="Home" />--%>
                    </td>
                </tr>
            </table>
            <br />
            <div style="font-size: medium; color: Blue;">
                Detail</div>
            <hr style="color: Blue" />
            <asp:Panel runat="server" ID="pnlDt">
                <asp:Button class="bitbtndt btnadd" runat="server" ID="btnAddDt" Text="Add" ValidationGroup="Input" />
                <div style="border: 0px  solid; width: 100%; height: 100%; overflow: auto;">
                    <asp:GridView ID="GridDt" runat="server" AutoGenerateColumns="False" ShowFooter="True" >
                        <HeaderStyle CssClass="GridHeader" />
                        <RowStyle CssClass="GridItem" Wrap="true" />
                        <AlternatingRowStyle CssClass="GridAltItem" />
                        <PagerStyle CssClass="GridPager" />
                        <Columns>
                            <%--<asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:CheckBox ID="cbSelectHd" runat="server" AutoPostBack="true" OnCheckedChanged="cbSelectHd_CheckedChanged1" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox ID="cbSelect" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>--%>
                            <asp:TemplateField HeaderText="Action">
                                <ItemTemplate>
                                    <asp:Button class="bitbtndt btnedit" runat="server" ID="btnEdit" Text="Edit" CommandName="Edit" />
                                    <asp:Button class="bitbtndt btndelete" runat="server" ID="btnDelete" Text="Delete"
                                        CommandName="Delete" />
                                </ItemTemplate>
                                <%--<EditItemTemplate>
                                    <asp:Button class="bitbtndt btnsave" runat="server" ID="btnUpdate" Text="Save" CommandName="Update" />
                                    <asp:Button class="bitbtndt  btncancel" runat="server" ID="btnCancel" Text="Cancel"
                                        CommandName="Cancel" />
                                </EditItemTemplate>--%>
                            </asp:TemplateField>
                            <asp:BoundField DataField="ItemNo" HeaderStyle-Width="50px" HeaderText="Item No" />
                            <asp:BoundField HeaderText="Delivery Request Warehouse Date" DataFormatString="{0:dd MMM yyyy}" DataField="DRDateWrhs" HeaderStyle-Width="80px" SortExpression="DRDateWrhs" />
                            <asp:BoundField HeaderText="Delivery Request ETA" DataFormatString="{0:dd MMM yyyy}" DataField="DRDateETA" HeaderStyle-Width="80px" SortExpression="DRDateETA" />
                            <asp:BoundField HeaderText="Delivery Request ETD" DataFormatString="{0:dd MMM yyyy}" DataField="DRDateETD" HeaderStyle-Width="80px" SortExpression="DRDateETD" />
                            <asp:BoundField HeaderText="Actual Shipment ETA" DataFormatString="{0:dd MMM yyyy}" DataField="ShipDateETA" HeaderStyle-Width="80px" SortExpression="ShipDateETA" />
                            <asp:BoundField HeaderText="Actual Shipment ETD" DataFormatString="{0:dd MMM yyyy}" DataField="ShipDateETD" HeaderStyle-Width="80px" SortExpression="ShipDateETD" />
                            <asp:BoundField HeaderText="Actual Shipment F.Customs" DataFormatString="{0:dd MMM yyyy}" DataField="ShipDateCustom" HeaderStyle-Width="80px" SortExpression="ShipDateCustom" />
                            <asp:BoundField HeaderText="InLand Delivery" DataField="DeliveryInLand" HeaderStyle-Width="80px" SortExpression="DeliveryInLand" />
                            <asp:BoundField HeaderText="Invoice No" DataField="InvoiceNo" HeaderStyle-Width="80px" SortExpression="InvoiceNo" />
                            <asp:BoundField HeaderText="Bill of Lading No" DataField="BLNo" HeaderStyle-Width="80px" SortExpression="BLNo" />
                            <asp:BoundField HeaderText="Container No" DataField="ContainerNo" HeaderStyle-Width="80px" SortExpression="ContainerNo" />
                            <asp:BoundField DataField="TotalContainer" DataFormatString="{0:#,##0}" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="80px" SortExpression="TotalContainer" HeaderText="Total Container" />
                            <asp:BoundField HeaderText="AJU No" DataField="AJUNo" HeaderStyle-Width="80px" SortExpression="AJUNo" />
                            <asp:BoundField HeaderText="Customs Response" DataField="Respon" HeaderStyle-Width="80px" SortExpression="Respon" />
                        </Columns>
                    </asp:GridView>
                </div>
                <asp:Button class="bitbtndt btnadd" runat="server" ID="btnAddDt2" Text="Add" ValidationGroup="Input" />
            </asp:Panel>
            <asp:Panel runat="server" ID="pnlEditDt" Visible="false">
                <table>
                    <tr>
                        <td>
                            Item No
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                            <asp:Label ID="lbItemNo" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Delivery Request Date
                        </td>
                        <td>
                            :
                        </td>
                        <td colspan="4">
                            <table>
                                <tr style="background-color: Silver; text-align: center">
                                    <td>
                                        Warehouse
                                    </td>
                                    <td>
                                        ETA
                                    </td>
                                    <td>
                                        ETD
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <BDP:BasicDatePicker ID="tbDRDateWrhs" ValidationGroup="Input" runat="server" DateFormat="dd MMM yyyy"
                                            ShowNoneButton="false" ButtonImageHeight="19px" ButtonImageWidth="20px" DisplayType="TextBoxAndImage"
                                            TextBoxStyle-CssClass="TextDate">
                                        </BDP:BasicDatePicker>
                                    </td>
                                    <td>
                                        <BDP:BasicDatePicker ID="tbDRDateETA" ValidationGroup="Input" runat="server" DateFormat="dd MMM yyyy"
                                            ShowNoneButton="false" ButtonImageHeight="19px" ButtonImageWidth="20px" DisplayType="TextBoxAndImage"
                                            TextBoxStyle-CssClass="TextDate">
                                        </BDP:BasicDatePicker>
                                    </td>
                                    <td>
                                        <BDP:BasicDatePicker ID="tbDRDateETD" ValidationGroup="Input" runat="server" DateFormat="dd MMM yyyy"
                                            ShowNoneButton="false" ButtonImageHeight="19px" ButtonImageWidth="20px" DisplayType="TextBoxAndImage"
                                            TextBoxStyle-CssClass="TextDate">
                                        </BDP:BasicDatePicker>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Actual Shipment Date
                        </td>
                        <td>
                            :
                        </td>
                        <td colspan="4">
                            <table>
                                <tr style="background-color: Silver; text-align: center">
                                    <td>
                                        ETD
                                    </td>
                                    <td>
                                        ETA
                                    </td>
                                    <td>
                                        F. Customs
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <BDP:BasicDatePicker ID="tbShipDateETD" ValidationGroup="Input" runat="server" DateFormat="dd MMM yyyy"
                                            ShowNoneButton="false" ButtonImageHeight="19px" ButtonImageWidth="20px" DisplayType="TextBoxAndImage"
                                            TextBoxStyle-CssClass="TextDate">
                                        </BDP:BasicDatePicker>
                                    </td>
                                    <td>
                                        <BDP:BasicDatePicker ID="tbShipDateETA" ValidationGroup="Input" runat="server" DateFormat="dd MMM yyyy"
                                            ShowNoneButton="false" ButtonImageHeight="19px" ButtonImageWidth="20px" DisplayType="TextBoxAndImage"
                                            TextBoxStyle-CssClass="TextDate">
                                        </BDP:BasicDatePicker>
                                    </td>
                                    <td>
                                        <BDP:BasicDatePicker ID="tbShipDateCustom" ValidationGroup="Input" runat="server"
                                            DateFormat="dd MMM yyyy" ShowNoneButton="false" ButtonImageHeight="19px" ButtonImageWidth="20px"
                                            DisplayType="TextBoxAndImage" TextBoxStyle-CssClass="TextDate">
                                        </BDP:BasicDatePicker>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            InLand Delivery
                        </td>
                        <td>
                            :
                        </td>
                        <td colspan="4">
                            <asp:TextBox ID="tbDeliveryInLand" Width="345" runat="server" CssClass="TextBox" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Invoice No
                        </td>
                        <td>
                            :
                        </td>
                        <td colspan="4">
                            <asp:TextBox ID="tbInvoiceNomor" Width="345" runat="server" CssClass="TextBox" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Bill of Lading No
                        </td>
                        <td>
                            :
                        </td>
                        <td colspan="4">
                            <asp:TextBox ID="tbBLNo" Width="345" runat="server" CssClass="TextBox" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Container No
                        </td>
                        <td>
                            :
                        </td>
                        <td colspan="4">
                            <asp:TextBox ID="tbContainerNo" Width="345" runat="server" CssClass="TextBox" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Total Container
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                            <asp:TextBox ID="tbTotalContainer" Width="100" runat="server" CssClass="TextBox" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            AJU No
                        </td>
                        <td>
                            :
                        </td>
                        <td colspan="4">
                            <asp:TextBox ID="tbAJUNo" Width="345" runat="server" CssClass="TextBox" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Customs Response
                        </td>
                        <td>
                            :
                        </td>
                        <td colspan="4">
                            <asp:TextBox ID="tbRespon" Width="345" runat="server" CssClass="TextBox" />
                        </td>
                    </tr>
                </table>
                <br />
                <asp:Button class="bitbtndt btnsave" runat="server" ID="btnSaveDt" Text="Save" />
                <asp:Button class="bitbtndt btncancel" runat="server" ID="btnCancelDt" Text="Cancel" />
                <br />
            </asp:Panel> 
            <br />
            <%--<asp:Button class="bitbtn btnsavenew" runat="server" ID="btnSaveAll" Text="Save & New"
                ValidationGroup="Input" Width="96px" />--%>
            <asp:Button class="bitbtn btnsave" runat="server" ID="btnSaveTrans" Text="Save" ValidationGroup="Input" />
            <asp:Button class="bitbtn btnback" runat="server" ID="btnBack" Text="Cancel" ValidationGroup="Input" />
            <asp:Button class="btngo" runat="server" ID="btnHome" Text="Home" Width="45px" />
        </asp:Panel>
    </div>
    <asp:Label ID="lbStatus" runat="server" ForeColor="red" />
    </form>
</body>
</html>
