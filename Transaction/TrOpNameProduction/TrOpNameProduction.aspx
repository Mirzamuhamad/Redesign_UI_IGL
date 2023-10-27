<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TrOpNameProduction.aspx.vb"
    Inherits="Transaction_TrOpNameProduction_TrOpNameProduction" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="BasicFrame.WebControls.BasicDatePicker" Namespace="BasicFrame.WebControls"
    TagPrefix="BDP" %>
<%@ Register Assembly="CrystalDecisions.Web, Version=10.5.3700.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Opname Production</title>

    <script src="../../Function/OpenDlg.js" type="text/javascript"></script>
    <script src="../../Function/Function.JS" type="text/javascript"></script>
    <script type="text/javascript" src="../../JS/jquerymodal/jquery.min.js"></script>

    <script src="../../JS/jquerymodal/jquery-ui.js" type="text/javascript"></script>
    <link href="../../JS/jquerymodal/jquery-ui.css" rel="stylesheet" type="text/css" />
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />

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
    
        function setformat()
        {
        
         try
         {           
        document.getElementById("tbOldPrice").value = setdigit(document.getElementById("tbOldPrice").value.replace(/\$|\,/g,""),'<%=VIEWSTATE("DigitCurr")%>');
        document.getElementById("tbNewPrice").value = setdigit(document.getElementById("tbNewPrice").value.replace(/\$|\,/g,""),'<%=VIEWSTATE("DigitCurr")%>');
//        document.getElementById("tbAdjustPercent").value = setdigit(document.getElementById("tbAdjustPercent").value.replace(/\$|\,/g,""),'<%=VIEWSTATE("DigitCurr")%>');
        }catch (err){
            alert(err.description);
          }      
        }
        
        $(document).ready(function(){
            $("#tbTime").mask("'00:00");
        });
        
        function hitung(prmchange)
        {
            var tbQty = document.getElementById("tbQty");
            //var tbQtyActual = document.getElementById("tbQtyActual");
            //var tbQtyAdjust = document.getElementById("tbQtyAdjust");
            
            if (tbQty.value == "")
            {
                tbQty.value == "0";
            }
            //if (tbPrice.value == "")
            //{
            //    tbPrice.value == "0";
            //}
            
            //tbQtyAdjust.value = parseFloat(tbQtyActual.value.replace(/,/g,"")) - parseFloat(tbQtySystem.value.replace(/,/g,""))
        }  
    </script>

    <style type="text/css">
        .style1
        {
            width: 67px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div class="Content">
        <div class="H1">
            <asp:Label ID="Labelmenu" runat="server" Text=""></asp:Label></div>
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
                            <asp:ListItem Value="TransNmbr" Selected="True">Reference</asp:ListItem>
                            <asp:ListItem Value="dbo.FormatDate(TransDate)">Date</asp:ListItem>
                            <asp:ListItem Value="Status">Status</asp:ListItem>
                            <asp:ListItem Value="Shift">Shift</asp:ListItem>
                            <asp:ListItem Value="ShiftName">Shift Name</asp:ListItem>
                            <asp:ListItem Value="OpnameType">Opname Type</asp:ListItem>
                            <%--<asp:ListItem Value="Warehouse">Warehouse Code</asp:ListItem>
                            <asp:ListItem Value="Wrhs_Name">Warehouse Name</asp:ListItem>--%>
                            <asp:ListItem Value="Remark">Remark</asp:ListItem>
                            <asp:ListItem Value="Type">Type</asp:ListItem>
                            <asp:ListItem Value="Product">Product</asp:ListItem>
                            <asp:ListItem Value="Product_Name">Product Name</asp:ListItem>
                        </asp:DropDownList>
                        <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlRange">
                        </asp:DropDownList>
                        <asp:Button class="bitbtn btnsearch" runat="server" ID="btnSearch" Text="Search" />
                        <asp:Button class="btngo" runat="server" ID="btnExpand" Text="..." />
                    </td>
                    <td>
                        <asp:LinkButton ID="LbAdvSearch" runat="server" Text="Advanced Search" />
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
                                <asp:ListItem Value="TransNmbr" Selected="True">Reference</asp:ListItem>
                                <asp:ListItem Value="dbo.FormatDate(TransDate)">Date</asp:ListItem>
                                <asp:ListItem Value="Status">Status</asp:ListItem>
                                <asp:ListItem Value="Shift">Shift</asp:ListItem>
                                <asp:ListItem Value="ShiftName">Shift Name</asp:ListItem>
                                <%--<asp:ListItem Value="OpnameType">Opname Type</asp:ListItem>--%>
                                <%--<asp:ListItem Value="Warehouse">Warehouse Code</asp:ListItem>
                                <asp:ListItem Value="Wrhs_Name">Warehouse Name</asp:ListItem>--%>
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
                    <HeaderStyle CssClass="GridHeader" Wrap="false"></HeaderStyle>
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
                                    <asp:ListItem Text="Copy New" />
                                    <%--<asp:ListItem Text="Print" />--%>
                                </asp:DropDownList>
                                <asp:Button class="btngo" runat="server" ID="BtnGo" Text="G" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>"
                                    CommandName="Go" />
                            </ItemTemplate>
                            <HeaderStyle Width="110px" />
                        </asp:TemplateField>
                        <asp:BoundField DataField="TransNmbr" HeaderStyle-Width="120px" SortExpression="Nmbr"
                            HeaderText="Reference">
                            <HeaderStyle Width="120px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Status" HeaderText="Status" SortExpression="Status"></asp:BoundField>
                        <asp:BoundField DataField="TransDate" DataFormatString="{0:dd MMM yyyy}" HtmlEncode="true"
                            HeaderStyle-Width="80px" HeaderText="Date" SortExpression="TransDate">
                            <HeaderStyle Width="80px" />
                        </asp:BoundField>
                        <%--<asp:BoundField DataField="Status" HeaderText="Status" SortExpression="Status"></asp:BoundField>--%>
                        <asp:BoundField DataField="Shift" HeaderText="Shift Code" SortExpression="Shift" />
                        <asp:BoundField DataField="ShiftName" HeaderText="Shift Name" SortExpression="ShiftName" />
                        <%--<asp:BoundField DataField="OpnameType" HeaderText="Opname Type" SortExpression="OpnameType" />--%>
                        <%--<asp:BoundField DataField="Warehouse" HeaderText="Warehouse Code" SortExpression="Warehouse" />
                        <asp:BoundField DataField="Wrhs_Name" HeaderText="Warehouse Name" SortExpression="Wrhs_Name" />--%>
                        <asp:BoundField DataField="Remark" HeaderText="Remark" SortExpression="Remark" />
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
            <asp:Panel runat="server" ID="PnlWOgetdata" Visible="true">
                <table>
                    <tr>
                        <td>
                            Reference
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                            <asp:TextBox CssClass="TextBox" Enabled="false" runat="server" ID="tbRef" Width="149px" />
                        </td>
                        <td>
                            Date / Shift
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                            <BDP:BasicDatePicker ID="tbDate" runat="server" DateFormat="dd MMM yyyy" ReadOnly="true"
                                ValidationGroup="Input" ButtonImageHeight="19px" ButtonImageWidth="20px" DisplayType="TextBoxAndImage"
                                TextBoxStyle-CssClass="TextDate" AutoPostBack="True" ShowNoneButton="False">
                                <TextBoxStyle CssClass="TextDate" />
                            </BDP:BasicDatePicker>
                            <asp:DropDownList CssClass="DropDownList" ValidationGroup="Input" runat="server"
                                ID="ddlShift" />
                            <asp:Label ID="Label16" runat="server" Text="*" ForeColor="#FF3300"></asp:Label>
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
                            <asp:TextBox ID="tbRemark" runat="server" CssClass="TextBox" ValidationGroup="Input"
                                Width="352px" MaxLength="255" Height="50px" TextMode="MultiLine" />
                            &nbsp;
                            <asp:Button class="btngo" runat="server" ID="btnGetDt" Text="Get Data" ValidationGroup="Input"
                                Width="56px" />
                        </td>
                    </tr>
                </table>
                <br />
                <div style="font-size: medium; color: Blue;">
                    Detail</div>
                <hr style="color: Blue" />
            </asp:Panel>
            <asp:Menu ID="Menu1" runat="server" CssClass="Menu" ItemWrap="False" Orientation="Horizontal"
                StaticEnableDefaultPopOutImage="False" StaticMenuItemStyle-CssClass="MenuItem"
                StaticSelectedStyle-CssClass="MenuSelect" Visible="False">
                <StaticSelectedStyle CssClass="MenuSelect" />
                <StaticMenuItemStyle CssClass="MenuItem" />
                <Items>
                    <asp:MenuItem Text="Detail " Value="0"></asp:MenuItem>
                    <asp:MenuItem Text="Detail WO" Value="1"></asp:MenuItem>
                    <asp:MenuItem Text="Import Excel" Value="2"></asp:MenuItem>
                </Items>
            </asp:Menu>
            <asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex="0">
                <asp:View ID="Tab1" runat="server">
                    <br />
                    <asp:Panel ID="pnlDt" runat="server">
                        <table>
                            <tr>
                                <td>
                                    <%--<asp:Panel ID="Panel1" runat="server" BorderColor="Black" BorderStyle="Solid" 
                    Height="100%" Width="224px">
            &nbsp;<asp:Label ID="Label1" runat="server" Font-Bold="True" ForeColor="Blue" Text="Set Adjust"></asp:Label>
            <asp:DropDownList ID="ddlAdjustType" runat="server" AutoPostBack="False" 
                    CssClass="DropDownList" Height="17px" Width="36px" >
                <asp:ListItem>+</asp:ListItem>
                <asp:ListItem>-</asp:ListItem>
                </asp:DropDownList>
            <asp:TextBox ID="tbAdjPercent" runat="server" AutoPostBack="False" 
                    CssClass="TextBox" Width="54px" /> %
            <asp:Button class="btngo" runat="server" ID="btnProcess" Text="Process" ValidationGroup="Input" Width="49px"/>       
            </asp:Panel>--%>
                                </td>
                            </tr>
                        </table>
                        
                        <asp:Button ID="btnAddDt" runat="server" class="bitbtndt btnadd" Text="Add" ValidationGroup="Input" />
                        &nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:Button ID="btnImport" runat="server" class="bitbtndt btnadd" Text="Import Excel" Width="100px"/>
                        <div style="border: 0px  solid; width: 100%; height: 100%; overflow: auto;">
                            <asp:GridView ID="GridDt" runat="server" AutoGenerateColumns="False" ShowFooter="True">
                                <HeaderStyle CssClass="GridHeader" />
                                <RowStyle CssClass="GridItem" Wrap="false" />
                                <AlternatingRowStyle CssClass="GridAltItem" />
                                <PagerStyle CssClass="GridPager" />
                                <Columns>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:CheckBox ID="cbSelectHd" runat="server" AutoPostBack="true" OnCheckedChanged="cbSelectHd_CheckedChanged1" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="cbSelect" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Action">
                                        <ItemTemplate>
                                            <asp:Button ID="btnEdit" runat="server" class="bitbtndt btnedit" CommandName="Edit"
                                                Text="Edit" />
                                            <asp:Button ID="btnDelete" runat="server" class="bitbtndt btndelete" CommandName="Delete"
                                                Text="Delete" />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:Button ID="btnUpdate" runat="server" class="bitbtndt btnsave" CommandName="Update"
                                                Text="Save" />
                                            <asp:Button ID="btnCancel" runat="server" class="bitbtndt  btncancel" CommandName="Cancel"
                                                Text="Cancel" />
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <%--<asp:BoundField DataField="TransNmbr" HeaderStyle-Width="50px" HeaderText="Reference">
                        <HeaderStyle Width="50px" />
                        </asp:BoundField>--%>
                                    <asp:BoundField DataField="Type" HeaderStyle-Width="100px" HeaderText="Type">
                                        <HeaderStyle Width="80px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Product" HeaderStyle-Width="100px" HeaderText="Product">
                                        <HeaderStyle Width="100px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Product_Name" HeaderStyle-Width="200px" HeaderText="Product Name">
                                        <HeaderStyle Width="200px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="ProductFG" HeaderStyle-Width="100px" HeaderText="ProductFG">
                                        <HeaderStyle Width="100px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="ProductName" HeaderStyle-Width="200px" HeaderText="ProductFG Name">
                                        <HeaderStyle Width="200px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="WONo" HeaderStyle-Width="200px" HeaderText="WONo">
                                        <HeaderStyle Width="100px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Qty" HeaderText="Qty" ItemStyle-HorizontalAlign="Right">
                                        <HeaderStyle Width="100px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Unit" HeaderStyle-Width="100px" HeaderText="Unit">
                                        <HeaderStyle Width="100px" />
                                    </asp:BoundField>
                                    <%-- <asp:BoundField DataField="UnitName" HeaderStyle-Width="200px" HeaderText="Unit Name">
                        <HeaderStyle Width="200px" />
                        </asp:BoundField>--%>
                                    <asp:BoundField DataField="Remark" HeaderStyle-Width="200px" HeaderText="Remark">
                                        <HeaderStyle Width="200px" />
                                    </asp:BoundField>
                                </Columns>
                            </asp:GridView>
                        </div>
                        <asp:Button ID="btnAddDt2" runat="server" class="bitbtndt btnadd" Text="Add" ValidationGroup="Input" />
                    </asp:Panel>
                    <asp:Panel runat="server" ID="pnlEditDt" Visible="false">
                        <table>
                            <tr>
                                <td>
                                    <asp:LinkButton ID="lbProduct" runat="server" Text="Product" />
                                </td>
                                <td>
                                    :
                                </td>
                                <td>
                                    <asp:DropDownList CssClass="DropDownList" AutoPostBack="true" ValidationGroup="Input"
                                        runat="server" ID="ddlType">
                                        <asp:ListItem>Material</asp:ListItem>
                                        <asp:ListItem>WIP</asp:ListItem>
                                        <asp:ListItem>FG</asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:TextBox ID="tbProductCode" runat="server" CssClass="TextBox" Width="126px" AutoPostBack="True"
                                        MaxLength="20" />
                                    <asp:TextBox ID="TbProductName" runat="server" CssClass="TextBoxR" Width="250px"
                                        Enabled="false" />
                                    <asp:Button class="btngo" runat="server" ID="btnProduct" Text="..." />
                                    <asp:Label ID="Label12" runat="server" ForeColor="#FF3300" Text="*"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblProductFG" runat="server" Text="Product FG"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lbltitikFG" runat="server" Text=":"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="tbProductCodeFG" runat="server" CssClass="TextBox" MaxLength="20"
                                        ValidationGroup="Input" Width="126px" AutoPostBack="true" />
                                    <asp:TextBox ID="TbProductNameFG" runat="server" CssClass="TextBoxR" ValidationGroup="Input"
                                        Width="250px" Enabled="false" />
                                    <asp:Button ID="btnProductFG" runat="server" class="bitbtn btngo" Text="..." ValidationGroup="Input" />
                                    <asp:Label ID="Label17" runat="server" ForeColor="#FF3300" Text="*"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblWOno" runat="server" Text="WO No"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lbltitik" runat="server" Text=":"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="tbWONo" runat="server" CssClass="TextBox" Enabled="False" />
                                    <asp:Button ID="btnWO" runat="server" class="bitbtn btngo" Text="..." ValidationGroup="Input" />
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
                                    <asp:TextBox ID="tbQty" runat="server" CssClass="TextBox" />
                                    <asp:DropDownList ID="ddlUnit" runat="server" CssClass="DropDownList" Height="17px"
                                        Enabled="false">
                                    </asp:DropDownList>
                                    <asp:Label ID="Label3" runat="server" ForeColor="#FF3300" Text="*"></asp:Label>
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
                                    <asp:TextBox ID="tbRemarkDt" runat="server" CssClass="TextBox" ValidationGroup="Input"
                                        Width="352px" MaxLength="255" Height="50px" TextMode="MultiLine" />
                                </td>
                            </tr>
                        </table>
                        <br />
                        <asp:Button class="bitbtndt btnsave" runat="server" ID="btnSaveDt" Text="Save" />
                        <asp:Button class="bitbtndt btncancel" runat="server" ID="btnCancelDt" Text="Cancel" />
                        <br />
                    </asp:Panel>
                </asp:View>
                <asp:View ID="Tab2" runat="server">
                    <table>
                        <tr>
                            <td style="text-align: right" class="style1">
                                <b>WO No : </b>
                            </td>
                            <td>
                                <asp:TextBox ID="tbWONoAll" runat="server" AutoPostBack="True" CssClass="TextBoxR"
                                    Enabled="False" />
                                <asp:Button ID="btnWOAll" runat="server" class="bitbtn btngo" Text="..." ValidationGroup="Input" />
                                &nbsp;
                            </td>
                            <td>
                                <b>WO Date : </b>&nbsp;<asp:Label ID="lbProductFG" runat="server"></asp:Label>
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                <b>Product FG : </b>
                            </td>
                            <td>
                                <asp:Label ID="lbCode" runat="server"></asp:Label>
                                <asp:Label ID="LbName" runat="server"></asp:Label>
                                <asp:Label ID="LbSpec" runat="server"></asp:Label>
                            </td>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                    </table>
                    <br />
                    <asp:Panel runat="server" ID="pnlWO">
                    </asp:Panel>
                    <br />
                    <asp:Panel runat="server" ID="Panel1">
                        <div style="border: 0px  solid; width: 100%; height: 100%; overflow: auto;">
                            <asp:GridView ID="GridExcel" runat="server" AutoGenerateColumns="False" CssClass="Grid">
                                <HeaderStyle CssClass="GridHeader" />
                                <RowStyle CssClass="GridItem" />
                                <AlternatingRowStyle CssClass="GridAltItem" />
                                <PagerStyle CssClass="GridPager" />
                                <Columns>
                                    <asp:TemplateField HeaderText="Type" ItemStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                            <asp:Label ID="Type" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Type") %>' />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Product" SortExpression="Product">
                                        <ItemTemplate>
                                            <asp:Label ID="Product" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Product") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Product Name" ItemStyle-HorizontalAlign="Left" SortExpression="Product_Name">
                                        <ItemTemplate>
                                            <asp:Label ID="Product_Name" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Product_Name") %>' />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="ProductFG" ItemStyle-HorizontalAlign="Left" SortExpression="ProductFG">
                                        <ItemTemplate>
                                            <asp:Label ID="ProductFG" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "ProductFG") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="ProductFG Name" ItemStyle-HorizontalAlign="Left" SortExpression="ProductFG_Name">
                                        <ItemTemplate>
                                            <asp:Label ID="ProductFG_Name" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "ProductFG_Name") %>' />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Qty" ItemStyle-HorizontalAlign="Right" SortExpression="Qty">
                                        <ItemTemplate>
                                            <asp:TextBox ID="Qty" runat="server" CssClass="TextBox" Style="text-align: right;"
                                                Text='<%# DataBinder.Eval(Container.DataItem, "Qty") %>' />
                                        </ItemTemplate>
                                        <ItemTemplate>
                                            <asp:TextBox ID="Qty" runat="server" CssClass="TextBox" Style="text-align: right;"
                                                Text='<%# DataBinder.Eval(Container.DataItem, "Qty") %>' />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Unit" SortExpression="Unit">
                                        <ItemTemplate>
                                            <asp:Label ID="Unit" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Unit") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                            <br />
                            <asp:Button ID="btnOK" runat="server" class="bitbtndt btnsave" Text="OK" ValidationGroup="Input" />
                            &nbsp;<asp:Button ID="btnCancelWO" runat="server" class="bitbtndt btncancel" Text="Cancel"
                                ValidationGroup="Input" />
                    </asp:Panel>
                </asp:View>
                
                <asp:View ID="Tab3" runat="server">                
                    <table>
                        <tr>            
                            <td style="width:150px;text-align:right"><b>Please Select Excel File : </b></td> 
                            <td>                
                                <asp:FileUpload ID="fileuploadExcel" runat="server" />&nbsp;  
                                <asp:Button class="bitbtn btnpreview" Width="98px" runat="server" ID="btnUploadExcel" Text="Upload" />
                            </td>
                        </tr>
                        <tr>
                            <td style="width:150px;text-align:right"><b>Display Data : </b></td> 
                            <td>
                                <asp:DropDownList ID="ddlSheets" runat="server" Width="200px" AutoPostBack="true" CssClass="DropDownList"
                                    AppendDataBoundItems = "true">
                                </asp:DropDownList>
                            </td>             
                        </tr>        
                    </table>      
                    <br />  
                  <asp:Panel runat="server" ID="pnlImport">
                        <%--<fieldset style="width:800px">--%>
                         
                        <u><b>Import Data Opname</b></u><table>
                        <tr>
                            <td>Type</td>
                            <td>:</td>
                            <td><asp:DropDownList ID="ddlFindType" runat="server" Width="140px" CssClass="DropDownList" AppendDataBoundItems = "true"/></td>
                            <td>Product</td>
                            <td>:</td>
                            <td><asp:DropDownList ID="ddlFindProductCode" runat="server" Width="140px" CssClass="DropDownList" AppendDataBoundItems = "true"/></td>
                            <td><asp:Button ID="btnGenerate" runat="server" class="bitbtndt btnok" Width="110px" Text="Import To System" />                         
                            </td>
                        </tr> 
                        <tr>
                            <td>Qty</td>
                            <td>:</td>
                            <td><asp:DropDownList ID="ddlFindQty" runat="server" Width="140px" CssClass="DropDownList" AppendDataBoundItems = "true"/></td>
                            <td>Unit</td>
                            <td>:</td>
                            <td><asp:DropDownList ID="ddlFindUnit" runat="server" AppendDataBoundItems="true" CssClass="DropDownList" Width="140px" /></td>
                            <td></td>
                            <td></td>
                            <td></td>
                            
                        </tr>
                        <tr>                            
                            <td>Remark</td>
                            <td>:</td>
                            <td>
                                <asp:DropDownList ID="ddlFindRemark" runat="server" AppendDataBoundItems="true" 
                                    CssClass="DropDownList" Width="140px" />
                            </td>
                            <td></td>
                            <td></td>
                            <td></td>
                            
                        </tr>              
                        </table>
                    </asp:Panel> 
                       <br />       
                      <asp:Panel runat="server" id="Panel2">          
                      <div style="border:0px  solid; width:100%; height:100%; overflow:auto;">
                      <asp:GridView ID="GridView2" Width="1000" runat="server" AllowPaging="False" 
                        CssClass="Grid" AutoGenerateColumns="true"> 
                          <HeaderStyle CssClass="GridHeader" Wrap = "false"></HeaderStyle>
						            <RowStyle CssClass="GridItem" Wrap="false" />
						            <AlternatingRowStyle CssClass="GridAltItem"/>
						            <PagerStyle CssClass="GridPager" />
                          <Columns>              
                              				  	
            					
                          </Columns>
                      </asp:GridView>
                      </div>   
                      </asp:Panel>         
               
            </asp:View> 
            </asp:MultiView>
            <br />
            <asp:Button class="bitbtn btnsavenew" runat="server" ID="btnSaveAll" Text="Save & New"
                ValidationGroup="Input" Width="96px" />
            <asp:Button class="bitbtn btnsave" runat="server" ID="btnSaveTrans" Text="Save" ValidationGroup="Input" />
            <asp:Button class="bitbtn btnback" runat="server" ID="btnBack" Text="Cancel" ValidationGroup="Input" />
            <asp:Button class="btngo" runat="server" ID="btnHome" Text="Home" Width="45px" />
        </asp:Panel>
    </div>
    <asp:Panel runat="server" ID="pnlPrint" Visible="false">
        <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" AutoDataBind="True"
            Height="1036px" Width="928px" />
    </asp:Panel>
    <br />
    </div>
    <asp:Label runat="server" ID="lbStatus" ForeColor="Red" />
    </form>
</body>
</html>
