<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TrBOM.Aspx.vb" Inherits="TrBOM" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="BasicFrame.WebControls.BasicDatePicker" Namespace="BasicFrame.WebControls"
    TagPrefix="BDP" %>
<%--<%@ Register assembly="CrystalDecisions.Web, Version=12.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>--%>
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
        
       function setformatHd(prmchange)
        {
         try
         {       
             if(prmchange == "Hd")
             {                                
                document.getElementById("tbTotalgr").value = (parseFloat(document.getElementById("tbLength").value.replace(/\$|\,/g, "")) * parseFloat(document.getElementById("tbWidth").value.replace(/\$|\,/g, "")) * parseFloat(document.getElementById("tbSheet").value.replace(/\$|\,/g, "")) * parseFloat(document.getElementById("tbGSM").value.replace(/\$|\,/g, ""))) / 10000;
                document.getElementById("tbTotalPremix").value = (parseFloat(document.getElementById("tbLength").value.replace(/\$|\,/g, "")) * parseFloat(document.getElementById("tbWidth").value.replace(/\$|\,/g, "")) * parseFloat(document.getElementById("tbSheet").value.replace(/\$|\,/g, "")) * parseFloat(document.getElementById("tbGSM").value.replace(/\$|\,/g, "")) * parseFloat(document.getElementById("tbRatio").value.replace(/\$|\,/g, ""))) / 10000
                 
//                document.getElementById("tbTotalgr").value = setdigit(document.getElementById("tbTotalgr").value.replace(/\$|\,/g,""),'<%=ViewState("DigitCurr")%>');
//                document.getElementById("tbTotalPremix").value = setdigit(document.getElementById("tbTotalPremix").value.replace(/\$|\,/g,""),'<%=ViewState("DigitCurr")%>');
//                document.getElementById("tbLength").value = setdigit(document.getElementById("tbLength").value.replace(/\$|\,/g,""),'<%=ViewState("DigitCurr")%>');
//                document.getElementById("tbWidth").value = setdigit(document.getElementById("tbWidth").value.replace(/\$|\,/g,""),'<%=ViewState("DigitCurr")%>');
//                document.getElementById("tbSheet").value = setdigit(document.getElementById("tbSheet").value.replace(/\$|\,/g,""),'<%=ViewState("DigitCurr")%>');
//                document.getElementById("tbGSM").value = setdigit(document.getElementById("tbGSM").value.replace(/\$|\,/g,""),'<%=ViewState("DigitCurr")%>');
//                document.getElementById("tbRatio").value = setdigit(document.getElementById("tbRatio").value.replace(/\$|\,/g,""),'<%=ViewState("DigitCurr")%>');                
             }              
        }catch (err){
            alert(err.description);
          }      
        } 
        
        function postback()
        {
            __doPostBack('','');
        }   
    </script>

    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
 <link href="https://fonts.googleapis.com/css2?family=Muli:wght@300;400;600&display=swap" rel="stylesheet">
    <style type="text/css">
        .style2
        {
            width: 10px;
        }
        .style9
        {
            width: 95px;
        }
        .style10
        {
            width: 67px;
        }
        .style12
        {
            width: 5px;
        }
        .style13
        {
            width: 72px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div class="Content">
        <div class="H1">
            Bill Of Material</div>
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
                            <asp:ListItem Selected="True" Value="TransNmbr">BOM No</asp:ListItem>
                            <asp:ListItem Value="dbo.FormatDate(TransDate)">BOM Date</asp:ListItem>
                            <asp:ListItem>Status</asp:ListItem>
                            <asp:ListItem Value="dbo.FormatDate(EffectiveDate)">Effective Date</asp:ListItem>
                            <%--<asp:ListItem Value="Customer">Customer Code</asp:ListItem>
                      <asp:ListItem Value="Customer_Name">Customer Name</asp:ListItem>--%>
                            <asp:ListItem Value="Product">Product Code</asp:ListItem>
                            <asp:ListItem Value="Product_Name">Product Name</asp:ListItem>
                            <asp:ListItem Value="BOMType">BOM Type</asp:ListItem>
                            <asp:ListItem>Remark</asp:ListItem>
                            <asp:ListItem Value="FormulaName">Formula Name</asp:ListItem>
                        </asp:DropDownList>
                        <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlRange">
                        </asp:DropDownList>
                        <asp:Button class="bitbtn btnsearch" runat="server" ID="btnSearch" Text="Search" />
                        <asp:Button class="bitbtn btngo" runat="server" ID="btnExpand" Text="..." />
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
                                <asp:ListItem Selected="True" Value="TransNmbr">BOM No</asp:ListItem>
                                <asp:ListItem Value="dbo.FormatDate(TransDate)">BOM Date</asp:ListItem>
                                <asp:ListItem>Status</asp:ListItem>
                                <asp:ListItem Value="dbo.FormatDate(EffectiveDate)">Effective Date</asp:ListItem>
                                <%-- <asp:ListItem Value="Customer">Customer Code</asp:ListItem>
                      <asp:ListItem Value="Customer_Name">Customer Name</asp:ListItem>--%>
                                <asp:ListItem Value="Product">Product Code</asp:ListItem>
                                <asp:ListItem Value="Product_Name">Product Name</asp:ListItem>
                                <asp:ListItem Value="BOMType">BOM Type</asp:ListItem>
                                <asp:ListItem>Remark</asp:ListItem>
                                <asp:ListItem Value="FormulaName">Formula Name</asp:ListItem>
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
                                    <asp:ListItem Text="Print" />
                                    <asp:ListItem Text="Copy New" />
                                </asp:DropDownList>
                                <asp:Button class="btngo" runat="server" ID="BtnGo" Text="G" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>"
                                    CommandName="Go" />
                            </ItemTemplate>
                            <HeaderStyle Width="110px" />
                        </asp:TemplateField>
                        <asp:BoundField DataField="TransNmbr" HeaderStyle-Width="120px" SortExpression="Nmbr"
                            HeaderText="BOM No"></asp:BoundField>
                        <asp:BoundField DataField="Status" HeaderText="Status"></asp:BoundField>
                        <asp:BoundField DataField="TransDate" DataFormatString="{0:dd MMM yyyy}" HtmlEncode="true"
                            HeaderStyle-Width="80px" SortExpression="TransDate" HeaderText="BOM Date"></asp:BoundField>
                        <asp:BoundField DataField="EffectiveDate" DataFormatString="{0:dd MMM yyyy}" HtmlEncode="true"
                            HeaderStyle-Width="80px" SortExpression="EffectiveDate" HeaderText="Effective Date">
                        </asp:BoundField>
                        <asp:BoundField DataField="BOMType" HeaderStyle-Width="50px" SortExpression="BOMType"
                            HeaderText="BOM Type"></asp:BoundField>
                        <asp:BoundField DataField="Product" HeaderStyle-Width="150px" SortExpression="Product"
                            HeaderText="Product Code"></asp:BoundField>
                        <asp:BoundField DataField="Product_Name" HeaderStyle-Width="250px" SortExpression="Product_Name"
                            HeaderText="Product Name"></asp:BoundField>
                        <asp:BoundField DataField="Specification" HeaderStyle-Width="250px" SortExpression="Specification"
                            HeaderText="Specification"></asp:BoundField>
                        <%--<asp:BoundField DataField="Customer" HeaderStyle-Width="150px" SortExpression="Customer" HeaderText="Customer Code"></asp:BoundField>
                  <asp:BoundField DataField="Customer_Name" HeaderStyle-Width="200px" SortExpression="Customer_Name" HeaderText="Customer Name"></asp:BoundField>--%>
                        <asp:BoundField DataField="Remark" HeaderStyle-Width="250px" HeaderText="Remark">
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
            <table style="width: 676px">
                <tr style="width: 80px">
                    <td class="style9">
                        BOM No
                    </td>
                    <td style="width: 10px">
                        :
                    </td>
                    <td colspan="3">
                        <asp:TextBox CssClass="TextBox" Enabled="false" runat="server" ID="tbTransNo" Width="149px" />
                    </td>
                </tr>
                <tr>
                    <td class="style9">
                        BOM Date
                    </td>
                    <td>
                        :
                    </td>
                    <td colspan="3">
                        <BDP:BasicDatePicker ID="tbDate" runat="server" DateFormat="dd MMM yyyy" ShowNoneButton="false"
                            ReadOnly="true" ValidationGroup="Input" ButtonImageHeight="19px" ButtonImageWidth="20px"
                            DisplayType="TextBoxAndImage" TextBoxStyle-CssClass="TextDate" AutoPostBack="True"><TextBoxStyle CssClass="TextDate" /></BDP:BasicDatePicker>
                    </td>
                </tr>
                <tr>
                    <td class="style9">
                        Effective Date
                    </td>
                    <td>
                        :
                    </td>
                    <td>
                        <BDP:BasicDatePicker ID="tbEffectiveDate" runat="server" DateFormat="dd MMM yyyy"
                            ShowNoneButton="false" ReadOnly="true" ValidationGroup="Input" ButtonImageHeight="19px"
                            ButtonImageWidth="20px" DisplayType="TextBoxAndImage" TextBoxStyle-CssClass="TextDate"
                            AutoPostBack="True"><TextBoxStyle CssClass="TextDate" /></BDP:BasicDatePicker>
                    </td>
                    <td>Product Level :
                        <asp:Label ID="lbProductLevel" runat="server" Font-Bold="true"/>
                    </td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td class="style9">
                        Product
                    </td>
                    <td>
                        :
                    </td>
                    <td colspan="3">
                        <asp:TextBox ID="tbProductCode" runat="server" CssClass="TextBox" MaxLength="20"
                            ValidationGroup="Input" Width="124px" AutoPostBack="true" />
                        <asp:TextBox ID="tbProductName" runat="server" CssClass="TextBoxR" ValidationGroup="Input"
                            Width="358px" />
                        <asp:Button ID="btnProduct" runat="server" class="bitbtn btngo" Text="..." ValidationGroup="Input" />
                        <asp:Label ID="label1" runat="server" Text="*" Font-Underline="False" ForeColor="#FF3300" />
                    </td>
                </tr>
                <tr>
                    <td class="style9">
                        Specification
                    </td>
                    <td>
                        :
                    </td>
                    <td colspan="3">
                        <asp:TextBox ID="tbSpecificationHd" runat="server" CssClass="TextBox" Enabled="False"
                            MaxLength="255" TextMode="MultiLine" Width="430px" />
                    </td>
                </tr>
                <%--<tr>
              <td class="style9">Customer</td>
              <td>:</td>
              <td>
                  <asp:TextBox ID="tbCustCode" runat="server" CssClass="TextBoxR" Enabled="false" 
                      MaxLength="12" Width="124px" />
                  <asp:TextBox ID="tbCustName" runat="server" CssClass="TextBoxR" Enabled="false" 
                      Width="361px" />
              </td>
          </tr>--%>
                <tr>
                    <td class="style9">
                        BOM Type
                    </td>
                    <td>
                        :
                    </td>
                    <td colspan="3">
                        <asp:DropDownList ID="ddlBomType" runat="server" CssClass="DropDownList" Height="16px"
                            ValidationGroup="Input" Width="96px" AutoPostBack="True">
                        </asp:DropDownList>
                        <asp:Label ID="label20" runat="server" Font-Underline="False" ForeColor="#FF3300"
                            Text="*" />
                    </td>
                </tr>
            </table>
            <asp:Panel runat="server" ID="pnlTissue">
                <table style="width: 676px">
                    <tr style="width: 80px">
                        <td class="style9">
                            Length
                        </td>
                        <td style="width: 10px">
                            :
                        </td>
                        <td>
                            <asp:TextBox ID="tbLength" runat="server" CssClass="TextBox" ValidationGroup="Input"
                                Width="72px" />
                            cm
                            <asp:Label ID="label3" runat="server" Font-Underline="False" ForeColor="#FF3300"
                                Text="*" />
                        </td>
                        <td align="right">
                            Width
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                            <asp:TextBox ID="tbWidth" runat="server" CssClass="TextBox" ValidationGroup="Input"
                                Width="72px" />
                            cm
                            <asp:Label ID="label4" runat="server" Font-Underline="False" ForeColor="#FF3300"
                                Text="*" />
                        </td>
                    </tr>
                    <tr>
                        <td class="style10">
                            Sheet
                        </td>
                        <td class="style12">
                            :
                        </td>
                        <td>
                            <asp:TextBox ID="tbSheet" runat="server" CssClass="TextBox" ValidationGroup="Input"
                                Width="72px" />
                            /pack
                            <asp:Label ID="label6" runat="server" Font-Underline="False" ForeColor="#FF3300"
                                Text="*" />
                        </td>
                        <td align="right">
                            GSM
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                            <asp:TextBox ID="tbGSM" runat="server" CssClass="TextBox" ValidationGroup="Input"
                                Width="72px" />
                            <asp:Label ID="label5" runat="server" Font-Underline="False" ForeColor="#FF3300"
                                Text="*" />
                        </td>
                    </tr>
                    <tr>
                        <td class="style10">
                            Ratio Premix
                        </td>
                        <td class="style12">
                            :
                        </td>
                        <td>
                            <asp:TextBox ID="tbRatio" runat="server" CssClass="TextBox" ValidationGroup="Input"
                                Width="72px" />
                            <asp:Label ID="label7" runat="server" Font-Underline="False" ForeColor="#FF3300"
                                Text="*" />
                        </td>
                        <td align="right">
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td class="style10">
                            Total
                        </td>
                        <td class="style12">
                            :
                        </td>
                        <td>
                            <asp:TextBox ID="tbTotalgr" runat="server" CssClass="TextBoxR" Width="72px" />
                            gr
                        </td>
                        <td align="right">
                            Premix
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                            <asp:TextBox ID="tbTotalPremix" runat="server" CssClass="TextBoxR" Width="72px" />
                            gr
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <table style="width: 676px">
                <tr>
                    <td class="style9">
                        Remark
                    </td>
                    <td class="style2">
                        :
                    </td>
                    <td>
                        <asp:TextBox ID="tbRemark" runat="server" CssClass="TextBox" MaxLength="255" TextMode="MultiLine"
                            ValidationGroup="Input" Width="430px" />
                    </td>
                </tr>
            </table>
            <br />
            <div style="font-size: medium; color: Blue;">
                Detail</div>
            <hr style="color: Blue" />
            <%--<asp:Menu
            ID="Menu1"            
            runat="server"
            CssClass = "Menu"        
            StaticMenuItemStyle-CssClass = "MenuItem"
            StaticSelectedStyle-CssClass = "MenuSelect"        
            Orientation="Horizontal"
            ItemWrap = "False"
            StaticEnableDefaultPopOutImage="False">            
            <Items>
                <asp:MenuItem Text="Detail" Value="0"></asp:MenuItem>                   
            </Items>            
        </asp:Menu>
        <br />--%>
            <asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex="0">
                <asp:View ID="tabProcess" runat="server">
                    <asp:Panel runat="server" ID="pnlDtProcess">
                        <asp:Button class="bitbtn btnadd" runat="server" ID="btnAddDtProcess" Text="Add"
                            ValidationGroup="Input" />
                        <br />
                        <br />
                        <div style="border: 0px  solid; width: 100%; height: 100%; overflow: auto;">
                            <asp:GridView ID="GridProcess" runat="server" AutoGenerateColumns="False" ShowFooter="True">
                                <HeaderStyle CssClass="GridHeader" />
                                <RowStyle CssClass="GridItem" Wrap="false" />
                                <AlternatingRowStyle CssClass="GridAltItem" />
                                <PagerStyle CssClass="GridPager" />
                                <Columns>
                                    <asp:TemplateField HeaderText="Action">
                                        <ItemTemplate>
                                            <asp:Button ID="btnEdit" runat="server" class="bitbtndt btnedit" CommandName="Edit"
                                                Text="Edit" />
                                            <asp:Button ID="btnDelete" runat="server" class="bitbtndt btndelete" CommandName="Delete"
                                                OnClientClick="return confirm('Sure to delete this data?');" Text="Delete" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Detail">
                                        <ItemTemplate>
                                            <asp:Button ID="btnDetailMaterial" runat="server" Class="bitbtndt btngetitem" CommandArgument="<%# Container.DataItemIndex %>"
                                                CommandName="DetailMaterial" Text="Detail" Width="70" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="Process" HeaderStyle-Width="80px" HeaderText="Process Code">
                                        <HeaderStyle Width="80px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="ProcessName" HeaderStyle-Width="250px" HeaderText="Process Name">
                                        <HeaderStyle Width="250px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Successor" HeaderStyle-Width="80px" HeaderText="Successor Code">
                                        <HeaderStyle Width="80px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="SuccessorName" HeaderStyle-Width="250px" HeaderText="Successor Name">
                                        <HeaderStyle Width="250px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="FormulaName" HeaderStyle-Width="80px" HeaderText="Formula Name">
                                        <HeaderStyle Width="80px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="OutputType" HeaderStyle-Width="80px" HeaderText="Output Type">
                                        <HeaderStyle Width="80px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="ProductOutput" HeaderStyle-Width="80px" HeaderText="Product Output">
                                        <HeaderStyle Width="80px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="ProductOutputName" HeaderStyle-Width="250px" HeaderText="Product Output Name">
                                        <HeaderStyle Width="250px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="QtyOutput" HeaderStyle-Width="80px" HeaderText="Qty Output">
                                        <HeaderStyle Width="80px" />
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="UnitOutput" HeaderStyle-Width="80px" HeaderText="Unit Output">
                                        <HeaderStyle Width="80px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="FgSubkon" HeaderStyle-Width="80px" HeaderText="Subkon">
                                        <HeaderStyle Width="80px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Remark" HeaderStyle-Width="255px" HeaderText="Remark">
                                        <HeaderStyle Width="255px" />
                                    </asp:BoundField>
                                </Columns>
                            </asp:GridView>
                        </div>
                        <br />
                        <asp:Button class="bitbtn btnadd" runat="server" ID="btnAddDt2Process" Text="Add"
                            ValidationGroup="Input" />
                    </asp:Panel>
                    <asp:Panel runat="server" ID="pnlEditDtProcess" Visible="false">
                        <table style="width: 673px">
                            <tr>
                                <td>
                                    <asp:LinkButton ID="lbProcess" runat="server" Text="Process" />
                                </td>
                                <td>
                                    :
                                </td>
                                <td colspan="4">
                                    <asp:TextBox runat="server" ID="tbProcessCode" CssClass="TextBox" AutoPostBack="true"
                                        MaxLength="5" Width="124px" />
                                    <asp:TextBox runat="server" CssClass="TextBoxR" ID="tbProcessName" Width="387px"
                                        Enabled="False" />
                                    <asp:Button Class="btngo" ID="btnProcess" Text="..." runat="server" ValidationGroup="Input" />
                                    <asp:Label ID="label2" runat="server" Font-Underline="False" ForeColor="#FF3300"
                                        Text="*" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Successor
                                </td>
                                <td>
                                    :
                                </td>
                                <td colspan="4">
                                    <asp:TextBox ID="tbSuccessorCode" runat="server" AutoPostBack="true" CssClass="TextBox"
                                        MaxLength="5" Width="124px" />
                                    <asp:TextBox ID="tbSuccessorName" runat="server" CssClass="TextBoxR" Width="386px"
                                        Enabled="False" />
                                    <asp:Button ID="btnSuccessor" runat="server" Class="btngo" Text="..." ValidationGroup="Input" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Formula No
                                </td>
                                <td>
                                    :
                                </td>
                                <td colspan="4">
                                    <asp:TextBox ID="tbFormulaProcess" runat="server" AutoPostBack="true" CssClass="TextBox"
                                        MaxLength="20" Width="124px" />
                                    <asp:TextBox ID="tbFormulaName" runat="server" CssClass="TextBoxR" Enabled="False"
                                        Width="386px" />
                                    <asp:Button ID="btnFormulaProcess" runat="server" Class="btngo" Text="..." ValidationGroup="Input" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Output Type
                                </td>
                                <td>
                                    :
                                </td>
                                <td colspan="4">
                                    <asp:DropDownList ID="ddlOutputType" runat="server" AutoPostBack="true" CssClass="DropDownList"
                                        Height="16px" Width="49px" Enabled="False">
                                        <asp:ListItem>FG</asp:ListItem>
                                        <asp:ListItem Selected="True">WIP</asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:Label ID="label16" runat="server" Font-Underline="False" ForeColor="#FF3300"
                                        Text="*" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Subkon
                                </td>
                                <td>
                                    :
                                </td>
                                <td colspan="4">
                                    <asp:DropDownList ID="ddlSubkon" runat="server" CssClass="DropDownList" Height="16px"
                                        Enabled="true">
                                        <asp:ListItem>Y</asp:ListItem>
                                        <asp:ListItem Selected="True">N</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Product Output
                                </td>
                                <td>
                                    :
                                </td>
                                <td colspan="4">
                                    <asp:TextBox ID="tbProductOutputCode" runat="server" AutoPostBack="true" CssClass="TextBox"
                                        MaxLength="20" Width="124px" />
                                    <asp:TextBox ID="tbProductOutputName" runat="server" CssClass="TextBoxR" Width="384px"
                                        Enabled="False" />
                                    <asp:Button ID="btnProductOutput" runat="server" Class="btngo" Text="..." ValidationGroup="Input" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Qty Output
                                </td>
                                <td>
                                    :
                                </td>
                                <td>
                                    <asp:TextBox ID="tbQtyOutput" runat="server" CssClass="TextBox" MaxLength="20" Width="45px" />
                                </td>
                                <td align="right">
                                    Unit&nbsp; Output
                                </td>
                                <td>
                                    :
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlUnitOutput" runat="server" CssClass="DropDownList" Enabled="false"
                                        Width="62px" />
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
                                    <asp:TextBox ID="tbRemarkProcess" runat="server" CssClass="TextBox" MaxLength="255"
                                        TextMode="MultiLine" Width="526px" />
                                </td>
                            </tr>
                        </table>
                        <br />
                        <asp:Button ID="btnSaveProcess" runat="server" class="bitbtndt btnsave" Text="Save" />
                        <asp:Button ID="btnCancelProcess" runat="server" class="bitbtndt btncancel" Text="Cancel" />
                    </asp:Panel>
                    <br />
                </asp:View>
                <asp:View ID="Tab1" runat="server">
                    <asp:Panel ID="pnlInfoDt" runat="server">
                        <asp:Label ID="lblProcessDt" runat="server" Text="Process :" />
                        <asp:Label ID="lblProcessCodeDt" runat="server" Font-Bold="true" ForeColor="Blue"
                            Text="ProcessCode" />
                        <asp:Label ID="lblProcessNameDt" runat="server" Font-Bold="true" ForeColor="Blue"
                            Text="ProcessName" />
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:Label ID="Label9" runat="server" Font-Bold="true" Text="Total Qty : " />
                        <asp:Label ID="lblTotalQty" runat="server" Font-Bold="true" ForeColor="Red" Text="0" />
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:Label ID="Label8" runat="server" Font-Bold="true" Text="Total Qty Compare :" />
                        <asp:Label ID="lblTotalQtyCompare" runat="server" Font-Bold="true" ForeColor="Red"
                            Text="0" />
                    </asp:Panel>
                    <br />
                    <asp:Panel runat="server" ID="pnlDt">
                        <asp:Button class="bitbtn btnadd" runat="server" ID="btnAddDt" Text="Add" ValidationGroup="Input" />
                        <asp:Button ID="btnBackDt" runat="server" class="bitbtndt btnback" Text="Back" Width="60" />
                        <asp:Button ID="btnGetDt" runat="server" class="bitbtn btngetitem" Text="Get Material"
                            ValidationGroup="Input" Width="95px" />
                        <br />
                        <br />
                        <div style="border: 0px  solid; width: 100%; height: 100%; overflow: auto;">
                            <asp:GridView ID="GridDt" runat="server" AutoGenerateColumns="False" ShowFooter="True">
                                <HeaderStyle CssClass="GridHeader" />
                                <RowStyle CssClass="GridItem" Wrap="false" />
                                <AlternatingRowStyle CssClass="GridAltItem" />
                                <PagerStyle CssClass="GridPager" />
                                <Columns>
                                    <asp:TemplateField HeaderText="Action">
                                        <ItemTemplate>
                                            <asp:Button ID="btnEdit" runat="server" class="bitbtndt btnedit" CommandName="Edit"
                                                Text="Edit" />
                                            <asp:Button ID="btnDelete" runat="server" class="bitbtndt btndelete" CommandName="Delete"
                                                OnClientClick="return confirm('Sure to delete this data?');" Text="Delete" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="Material" HeaderStyle-Width="80px" HeaderText="Material Code">
                                        <HeaderStyle Width="80px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="MaterialName" HeaderStyle-Width="250px" HeaderText="Material Name">
                                        <HeaderStyle Width="250px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Specification" HeaderStyle-Width="250px" HeaderText="Specification">
                                        <HeaderStyle Width="250px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="MaterialAlternate" HeaderStyle-Width="80px" HeaderText="Material Alternate Code">
                                        <HeaderStyle Width="80px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="MaterialAlternateName" HeaderStyle-Width="250px" HeaderText="Material Alternate Name">
                                        <HeaderStyle Width="250px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Qty" HeaderStyle-Width="80px" HeaderText="Qty">
                                        <HeaderStyle Width="80px" />
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Unit" HeaderStyle-Width="80px" HeaderText="Unit">
                                        <HeaderStyle Width="80px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="QtyCompare" HeaderStyle-Width="80px" HeaderText="Qty Compare">
                                        <HeaderStyle Width="80px" />
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="UnitCompare" HeaderStyle-Width="80px" HeaderText="Unit Compare">
                                        <HeaderStyle Width="80px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Remark" HeaderStyle-Width="250px" HeaderText="Remark">
                                        <HeaderStyle Width="250px" />
                                    </asp:BoundField>
                                </Columns>
                            </asp:GridView>
                        </div>
                        <br />
                        <asp:Button class="bitbtn btnadd" runat="server" ID="btnAddDt2" Text="Add" ValidationGroup="Input" />
                        <asp:Button ID="btnBackDt2" runat="server" class="bitbtndt btnback" Text="Back" Width="60" />
                    </asp:Panel>
                    <asp:Panel runat="server" ID="pnlEditDt" Visible="false">
                        <table style="width: 673px">
                            <tr>
                                <td>
                                    <asp:LinkButton ID="lbMaterial" runat="server" Text="Material" />
                                </td>
                                <td>
                                    :
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="tbMaterialCode" CssClass="TextBox" AutoPostBack="true"
                                        MaxLength="20" Width="124px" />
                                    <asp:TextBox runat="server" CssClass="TextBoxR" ID="tbMaterialName" Width="358px" />
                                    <asp:Button Class="btngo" ID="btnMaterial" Text="..." runat="server" ValidationGroup="Input" />
                                    <asp:Label ID="label10" runat="server" Font-Underline="False" ForeColor="#FF3300"
                                        Text="*" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Specification
                                </td>
                                <td>
                                    :
                                </td>
                                <td>
                                    <asp:TextBox ID="tbSpecificationDt" runat="server" CssClass="TextBox" Enabled="False"
                                        MaxLength="255" TextMode="MultiLine" Width="430px" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Material Alternate
                                </td>
                                <td>
                                    :
                                </td>
                                <td>
                                    <asp:TextBox ID="tbMaterialAlternate" runat="server" AutoPostBack="true" CssClass="TextBox"
                                        MaxLength="20" Width="124px" />
                                    <asp:TextBox ID="tbMaterialAlternateName" runat="server" CssClass="TextBoxR" Width="358px"
                                        Enabled="false" />
                                    <asp:Button ID="btnMaterialAlternate" runat="server" Class="btngo" Text="..." ValidationGroup="Input" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    <table frame="border">
                                        <tr align="center" style="background-color: Gray">
                                            <td>
                                                Qty
                                            </td>
                                            <td>
                                                Unit
                                            </td>
                                            <td class="style13">
                                                Qty Compare
                                            </td>
                                            <td>
                                                Unit Compare
                                            </td>
                                        </tr>
                                        <tr align="center">
                                            <td>
                                                <asp:TextBox ID="tbQtyDt" runat="server" CssClass="TextBox" Width="43px" />
                                                <asp:Label ID="label11" runat="server" Font-Underline="False" ForeColor="#FF3300"
                                                    Text="*" />
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlUnitDt" runat="server" CssClass="DropDownList" Enabled="False"
                                                    Width="62px" />
                                            </td>
                                            <td class="style13">
                                                <asp:TextBox ID="tbQtyCompare" runat="server" CssClass="TextBox" Width="43px" />
                                                <asp:Label ID="label21" runat="server" Font-Underline="False" ForeColor="#FF3300"
                                                    Text="*" />
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlUnitCompare" runat="server" CssClass="DropDownList" Height="17px"
                                                    Width="97px" />
                                                <asp:Label ID="label22" runat="server" Font-Underline="False" ForeColor="#FF3300"
                                                    Text="*" />
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
                                    <asp:TextBox ID="tbRemarkDt" runat="server" CssClass="TextBox" MaxLength="255" TextMode="MultiLine"
                                        Width="430px" />
                                </td>
                            </tr>
                        </table>
                        <br />
                        <asp:Button ID="btnSaveDt" runat="server" class="bitbtndt btnsave" Text="Save" />
                        <asp:Button ID="btnCancelDt" runat="server" class="bitbtndt btncancel" Text="Cancel" />
                    </asp:Panel>
                    <br />
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
        <asp:Panel runat="server" ID="pnlPrint" Visible="false">
        </asp:Panel>
        <br />
    </div>
    <asp:Label runat="server" ID="lbStatus" ForeColor="Red" />
    </form>
</body>
</html>
