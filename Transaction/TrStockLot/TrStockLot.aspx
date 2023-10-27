<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TrStockLot.aspx.vb" Inherits="Transaction_TrStockLot_TrStockLot" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<%@ Register assembly="BasicFrame.WebControls.BasicDatePicker" namespace="BasicFrame.WebControls" tagprefix="BDP" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Untitle</title>
    <script src="../../Function/Function.JS" type="text/javascript"></script>
    <script src="../../Function/OpenDlg.JS" type="text/javascript"></script>
    <script type="text/javascript">
    function OpenPopup() {         
        window.open("../../SearchDlg.Aspx","List","scrollbars=yes,resizable=no,width=500,height=400");        
        return false;
    }    
    
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
	    
	    z = (x1 + x2).replace(/\$|\,/g,"");
	    	    
	    if (isNaN(z) == true)
        {
           return 0;
        }    
	              
	    return x1 + x2;
            
	    
	    }catch (err){
            alert(err.description);
          }  
        }
        
        function cekNan(nstr)
        {
            if(isNaN(nstr) == true)
            {
                return 0;
            }
            return nstr;
        }
        
    function setformatHd(prmchange)
        {
         try
         {           
            if(prmchange == "LotNo")
            {
                document.getElementById("tbGenerateQtyPkg").value = setdigit(document.getElementById("tbGenerateQtyPkg").value.replace(/\$|\,/g,""),'<%=ViewState("DigitQty")%>');                
                document.getElementById("tbGenerateNoStart").value = setdigit(document.getElementById("tbGenerateNoStart").value.replace(/\$|\,/g,""),'<%=ViewState("DigitQty")%>');                
                document.getElementById("tbGenerateNoEnd").value = setdigit((parseFloat(document.getElementById("tbGenerateQtyPkg").value.replace(/\$|\,/g,"")) + parseFloat(document.getElementById("tbGenerateNoStart").value.replace(/\$|\,/g,""))) - 1, '<%=ViewState("DigitQty")%>');                                                
            }
         }catch (err){
            alert(err.description);
          }      
        }   
        
    </script>
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .style1
        {
            width: 89px;
        }
        .style2
        {
            width: 358px;
        }
        .style5
        {
            width: 124px;
        }
        .style6
        {
            width: 128px;
        }
        .style7
        {
            width: 82px;
        }
        .style8
        {
            width: 3px;
        }
        .style10
        {
        }
        .style17
        {
        }
        .style18
        {
            width: 1px;
        }
        .style19
        {
            width: 149px;
        }
        .style21
        {
            width: 46px;
        }
        .style22
        {
            width: 170px;
        }
        .style23
        {
            width: 41px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div class="Content">
     <div class="H1">Input Lot No</div>
     <hr style="color:Blue" />           
     <table>
           <tr>
              <td class="style1">Transaction</td>
              <td>:</td>
              <td><asp:TextBox ID="tbTransType" runat="server" CssClass="TextBoxR" Width="137px" />
                  <asp:TextBox ID="tbTransNo" runat="server" CssClass="TextBoxR" Width="180px" />
                  <asp:Button class="btngo" runat="server" ID="btnTrans" Text="..."/>     
              </td>
           </tr>
           <tr>
               <td class="style1">Date</td>
               <td>:</td>
               <td>
            <BDP:BasicDatePicker ID="tbDate" runat="server" DateFormat="dd MMM yyyy" 
                        ButtonImageHeight="19px" ButtonImageWidth="20px" Enabled = "false"
                        DisplayType="TextBoxAndImage" TextBoxStyle-CssClass="TextDate" 
                    ShowNoneButton="False"><TextBoxStyle CssClass="TextDate" /></BDP:BasicDatePicker>                
                   <asp:TextBox ID="tbDoneLot" runat="server" CssClass="TextBox" Width="44px" Visible = "false"/>                
               </td>
           </tr>
           <tr>
               <td class="style1">Warehouse</td>
               <td>:</td>
               <td><asp:DropDownList ID="ddlWarehouse" runat="server" CssClass="DropDownList" 
                       Width="185px" Enabled = "false" Height="21px" />
               </td>
           </tr>
           <tr>
               <td class="style1">Reference</td>
               <td>:</td>
               <td><asp:TextBox ID="tbReff" runat="server" CssClass="TextBoxR" Width="141px" />
               </td>
           </tr>
           </table>
     <br />
     <table>
     <tr>
     <td>
     <asp:Button ID="btnHome" runat="server" class="bitbtndt btnback" Text="Home" />
     </td>
     <td>
     <asp:Menu
            ID="Menu1"
            runat="server"
            CssClass = "Menu"        
            StaticMenuItemStyle-CssClass = "MenuItem"
            StaticSelectedStyle-CssClass = "MenuSelect"        
            Orientation="Horizontal"
            ItemWrap = "False"
            StaticEnableDefaultPopOutImage="False">            
            <StaticSelectedStyle CssClass="MenuSelect" />
            <StaticMenuItemStyle CssClass="MenuItem" />
            <Items>
                <asp:MenuItem Text="Detail" Value="0"></asp:MenuItem>
            </Items>                                    
     </asp:Menu>     
     </td>
     </tr>
     </table>
     
      									                                                           
     <br /> 
        <asp:MultiView 
        ID="MultiView1"
        runat="server"
        ActiveViewIndex="0">                 
           <asp:View ID="tab1" runat="server">
           <asp:Panel runat="server" ID="Panel1">
               <table style="width: 779px">
                   <tr>
                       <td class="style7">Product</td>
                       <td class="style8">:</td>
                       <td class="style2" colspan="3">
                           <asp:DropDownList ID="ddlProduct" runat="server" 
                               CssClass="DropDownList" Width="287px" Height="21px" AutoPostBack="True" />
                       </td>
                   </tr>
                   <tr>
                       <td class="style7">Product Name</td>
                       <td class="style8">:</td>
                       <td class="style2" colspan="3"><asp:TextBox ID="tbProductName" runat="server" CssClass="TextBoxR" Width="350px" />
                           <asp:TextBox ID="tbProduct" runat="server" CssClass="TextBox" Visible="false" 
                               Width="44px" />
                           <asp:TextBox ID="tbFgMove" runat="server" CssClass="TextBox" Visible="false" 
                               Width="44px" />
                           <asp:TextBox ID="tbLifeTime" runat="server" CssClass="TextBox" Visible="false" 
                               Width="44px" />
                           <asp:TextBox ID="tbFgLifeTime" runat="server" CssClass="TextBox" 
                               Visible="false" Width="44px" />
                       </td>
                   </tr>
                   <tr>
                       <td class="style7">Qty</td>
                       <td class="style8">:</td>
                       <td class="style6"><asp:TextBox ID="tbQty1" runat="server" CssClass="TextBoxR" Width="53px" />
                           <asp:Label ID="lblUnit1" runat="server"></asp:Label>
                       </td>
                       <td class="style5"><asp:TextBox ID="tbQty2" runat="server" CssClass="TextBoxR" Width="53px" />
                           <asp:Label ID="lblUnit2" runat="server"></asp:Label>
                       </td>
                       <td class="style2">
                           <asp:TextBox ID="tbQty3" runat="server" CssClass="TextBoxR" Width="53px" />
                           <asp:Label ID="lblUnit3" runat="server"></asp:Label>
                           &nbsp;
                       </td>
                   </tr>
                   <tr>
                       <td class="style7">
                           Total Qty Lot</td>
                       <td class="style8">
                           :</td>
                       <td class="style6">
                           <asp:TextBox ID="tbTQtyLot" runat="server" CssClass="TextBoxR" Width="53px" />
                           <asp:Label ID="lblUnit9" runat="server">Qty Roll</asp:Label>
                       </td>
                       <td class="style5">
                           <asp:TextBox ID="tbTQtyLot2" runat="server" CssClass="TextBoxR" 
                               Width="53px" />
                           <asp:Label ID="lblUnit10" runat="server">Qty Mtr/Roll</asp:Label>
                       </td>
                       <td class="style2">
                           &nbsp;</td>
                   </tr>
               </table>
           </asp:Panel>
           <br />
           
           <asp:Panel runat="server" ID="Panel2">
                <div style="border:0px  solid; width:100%; height:100%; overflow:auto;">
                  <fieldset style="width: 470px">
                    <legend>&nbsp;Set Lot No&nbsp;</legend>
                    <table style="width: 760px">
                        <tr>
                            <td class="style1">Qty Wrhs</td>
                            <td class="style8">:</td>
                            <td class="style22"><asp:TextBox ID="tbGenerateQty" runat="server" CssClass="TextBox" 
                                    Width="44px" />&nbsp;<asp:Label ID="lblUnit4" runat="server"></asp:Label>
                                /<asp:Label ID="lblUnit5" runat="server"></asp:Label>
                                &nbsp;
                            </td>
                            <td class="style23">Qty Packing</td>
                            <td class="style18">
                                :</td>
                            <td class="style19">
                                <asp:TextBox ID="tbGenerateQtyLot" runat="server" CssClass="TextBox" 
                                    Width="44px" />
                                <asp:Label ID="lblUnit6" runat="server"></asp:Label>
                            </td>
                            <td class="style21" align = "right">Digit Lot</td>
                            <td>:</td>
                            <td class="style10">
                                <asp:TextBox ID="tbGeneretDigit" runat="server" CssClass="TextBox" Width="58px" 
                                    MaxLength="2" />
                            </td>
                            <td class="style10">
                                Total Lot</td>
                            <td class="style10">
                                :</td>
                            <td class="style10">
                                <asp:TextBox ID="tbGenerateQtyPkg" runat="server" CssClass="TextBox" 
                                    MaxLength="5" Width="58px" />
                            </td>
                        </tr>
                        <tr>
                            <td class="style1">Prefix</td>
                            <td class="style8">:</td>
                            <td class="style22">
                                <asp:TextBox ID="tbGeneratePerfix" runat="server" CssClass="TextBox" Width="98px" />
                            </td>
                            <td class="style23">Sufix</td>
                            <td class="style18">:</td>
                            <td class="style19">
                                <asp:TextBox ID="tbGenerateSufix" runat="server" CssClass="TextBox" Width="98px" />
                            </td>
                            <td class="style21" align = "right">Lot No</td>
                            <td>:</td>
                            <td class="style10" colspan="4">
                                <asp:TextBox ID="tbGenerateNoStart" runat="server" CssClass="TextBox" Width="58px" />
                                &nbsp;s/d
                                <asp:TextBox ID="tbGenerateNoEnd" runat="server" CssClass="TextBoxR" 
                                    Width="58px" />
                            </td>
                        </tr>
                        <tr>
                            <td class="style1">
                                <asp:Label ID="lblUnit7" runat="server"></asp:Label>
                            </td>
                            <td class="style8">
                                <asp:Label ID="lblUnit8" runat="server" Text = ":"></asp:Label>
                            </td>
                            <td class="style22">
                                <BDP:BasicDatePicker ID="tbGenerateExpire" runat="server" 
                                    ButtonImageHeight="19px" ButtonImageWidth="20px" DateFormat="dd MMM yyyy" 
                                    DisplayType="TextBoxAndImage" ShowNoneButton="False" 
                                    TextBoxStyle-CssClass="TextDate"><TextBoxStyle CssClass="TextDate" /></BDP:BasicDatePicker>
                            </td>
                            <td class="style17" colspan="4">
                                <asp:Panel ID="pnlOut" runat="server" Width="176px">
                                    Pallet No :
                                    <asp:TextBox ID="tbPalletNo" runat="server" CssClass="TextBox" Width="99px" />
                                </asp:Panel>
                                &nbsp; </td>
                            <td class="style10" colspan="5">
                                <asp:Button ID="btnGenerate" runat="server" class="bitbtn btngetitem" 
                                    Text="Generate" />
                                &nbsp;<asp:Button ID="btnOut" runat="server" class="bitbtn btngetitem" 
                                    Text="Outstanding Lot" Width="105px" />
                            </td>
                        </tr>
                    </table>
                    </fieldset>                     
                     <br />
                    <asp:Panel ID="PnlInfo" runat="server" backcolor="silver" BorderColor="silver" BorderStyle="Solid" 
                        Height="100%" Width="222px">
                        &nbsp; <%--</asp:Panel></td>--%>
                        <%-- <td class="style1">
                  <asp:Panel ID="Panel1" runat="server" BorderColor="Black" BorderStyle="Solid" 
            Height="100%" Width="252px">--%> &nbsp;
                        <td>
                            &nbsp;&nbsp;</td>
                        <%-- </asp:Panel></td>--%>
                        <%--<td class="style1">
                  <asp:Panel ID="Panel2" runat="server" BorderColor="Black" BorderStyle="Solid" 
                      Height="100%" Width="83px">--%>
                        <asp:Label ID="Label2" runat="server" Font-Bold="True" ForeColor="Blue" 
                            Text="Delete Selected Item"></asp:Label>
                        &nbsp;&nbsp;<asp:Button ID="btnProcessDel" runat="server" Class="bitbtndt btngo" 
                            Text="Process" ValidationGroup="Input" Width="70" OnClientClick="return confirm('Sure to delete this data?');" />
                        <br />
                    </asp:Panel>
                    <br />
                     <br />
                    <asp:Panel ID="Panel4" runat="server">
                        <asp:GridView ID="DataGrid" runat="server" AllowPaging="False" 
                            AllowSorting="True" AutoGenerateColumns="False" CssClass="Grid" 
                            ShowFooter="True" Width="823px">
                            <HeaderStyle CssClass="GridHeader" />
                            <RowStyle CssClass="GridItem" Wrap="false" />
                            <AlternatingRowStyle CssClass="GridAltItem" />
                            <FooterStyle CssClass="GridFooter" />
                            <PagerStyle CssClass="GridPager" />
                            <Columns>
                                 <asp:TemplateField HeaderStyle-Width="30px" >
                                    <HeaderTemplate>
                                        <asp:CheckBox ID="cbSelectHd" runat="server" AutoPostBack="true" 
                                            oncheckedchanged="cbSelectHd_CheckedChanged1" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="cbSelect" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-Width="50px" HeaderText="Type" 
                                    SortExpression="FlowType">
                                    <Itemtemplate>
                                        <asp:Label ID="FlowType" Runat="server" 
                                            text='<%# DataBinder.Eval(Container.DataItem, "FlowType") %>'> </asp:Label>
                                    </Itemtemplate>
                                    <EditItemTemplate>
                                    <asp:TextBox runat="server" ID="FlowTypeEdit" MaxLength="6" Width="100%" CssClass="TextBox"
                                         AutoPostBack="false" Enabled="false" OnTextChanged="FlowType_TextChanged" Text='<%# DataBinder.Eval(Container.DataItem, "FlowType") %>' />
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:DropDownList CssClass="DropDownList" runat="server" ID="FlowTypeAdd" OnTextChanged="FlowType_TextChanged" AutoPostBack="false" Width="100%">
                                            <asp:ListItem Value="IN" Selected="True">IN</asp:ListItem>
                                            <asp:ListItem Value="OUT">OUT</asp:ListItem>
                                         </asp:DropDownList>
                                    </FooterTemplate>
                                    <HeaderStyle Width="130px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-Width="130px" HeaderText="Lot No" 
                                    SortExpression="LotNo">
                                    <Itemtemplate>
                                        <asp:Label ID="LotNo" Runat="server" 
                                            text='<%# DataBinder.Eval(Container.DataItem, "LotNo") %>'> </asp:Label>
                                    </Itemtemplate>
                                    <EditItemTemplate>
                                    <asp:TextBox runat="server" ID="LotNoEdit" MaxLength="6" Width="100%" CssClass="TextBox"
                                         AutoPostBack="false" OnTextChanged="LotNo_TextChanged" Text='<%# DataBinder.Eval(Container.DataItem, "LotNo") %>' />
                                     
                                        
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="LotNoAdd" runat="server"  OnTextChanged="LotNo_TextChanged" AutoPostBack="false"
                                        
                                            CssClass="TextBox" Width="100%" />
                                        <cc1:TextBoxWatermarkExtender ID="LotNoAdd_TextBoxWatermarkExtender" 
                                            runat="server" Enabled="True" TargetControlID="LotNoAdd" 
                                            WatermarkCssClass="Watermarked" WatermarkText="can't blank">
                                        </cc1:TextBoxWatermarkExtender>
                                    </FooterTemplate>
                                    <HeaderStyle Width="130px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-Width="80" HeaderText="Qty " 
                                    SortExpression="Qty">
                                    <Itemtemplate>
                                        <asp:Label ID="Qty" Runat="server" 
                                            text='<%# DataBinder.Eval(Container.DataItem, "Qty") %>'> </asp:Label>
                                    </Itemtemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="QtyEdit" Runat="server" CssClass="TextBox" OnTextChanged="Qty_TextChanged" AutoPostBack="false"
                                            Text='<%# DataBinder.Eval(Container.DataItem, "Qty") %>' Width="100%">
                                        </asp:TextBox>
                                        <cc1:TextBoxWatermarkExtender ID="QtyEdit_WtExt" runat="server" Enabled="True" 
                                            TargetControlID="QtyEdit" WatermarkCssClass="Watermarked" 
                                            WatermarkText="can't blank">
                                        </cc1:TextBoxWatermarkExtender>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="QtyAdd" Runat="Server" CssClass="TextBox" Width="100%" 
                                        OnTextChanged="Qty_TextChanged" AutoPostBack="false"/>
                                        <cc1:TextBoxWatermarkExtender ID="QtyAdd_WtExt" runat="server" Enabled="True" 
                                            TargetControlID="QtyAdd" WatermarkCssClass="Watermarked" 
                                            WatermarkText="can't blank">
                                        </cc1:TextBoxWatermarkExtender>
                                    </FooterTemplate>
                                    <HeaderStyle Width="80px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-Width="150" HeaderText="Pallet No" 
                                    SortExpression="PalletNo">
                                    <Itemtemplate>
                                        <asp:Label ID="PalletNo" Runat="server" 
                                            text='<%# DataBinder.Eval(Container.DataItem, "PalletNo") %>'> </asp:Label>
                                    </Itemtemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="PalletNoEdit" Runat="server" CssClass="TextBox" MaxLength="20" 
                                            Text='<%# DataBinder.Eval(Container.DataItem, "PalletNo") %>' Width="100%">
                                        </asp:TextBox>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="PalletNoAdd" Runat="Server" CssClass="TextBox" MaxLength="20" 
                                            Width="100%" />
                                    </FooterTemplate>
                                    <HeaderStyle Width="150px" />
                                </asp:TemplateField>
                                
                                <asp:TemplateField HeaderStyle-Width="80" HeaderText="Qty Package" 
                                    SortExpression="QtyPackage">
                                    <Itemtemplate>
                                        <asp:Label ID="QtyPackage" Runat="server" 
                                            text='<%# DataBinder.Eval(Container.DataItem, "QtyPackage") %>'> </asp:Label>
                                    </Itemtemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="QtyPackageEdit" Runat="server" CssClass="TextBox" 
                                            Text='<%# DataBinder.Eval(Container.DataItem, "QtyPackage") %>' Width="100%">
                                        </asp:TextBox>
                                        <cc1:TextBoxWatermarkExtender ID="QtyPackageEdit_WtExt" runat="server" 
                                            Enabled="True" TargetControlID="QtyPackageEdit" WatermarkCssClass="Watermarked" 
                                            WatermarkText="can't blank">
                                        </cc1:TextBoxWatermarkExtender>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="QtyPackageAdd" Runat="Server" CssClass="TextBox" 
                                            Width="100%" />
                                        <cc1:TextBoxWatermarkExtender ID="QtyPackageAdd_WtExt" runat="server" 
                                            Enabled="True" TargetControlID="QtyPackageAdd" WatermarkCssClass="Watermarked" 
                                            WatermarkText="can't blank">
                                        </cc1:TextBoxWatermarkExtender>
                                    </FooterTemplate>
                                    <HeaderStyle Width="80px" />
                                </asp:TemplateField>
                                
                                <asp:TemplateField HeaderStyle-Width="100" HeaderText="Expire Date" 
                                    SortExpression="ExpireDateV">
                                    <Itemtemplate>
                                        <asp:Label ID="ExpireDate" Runat="server" 
                                            text='<%# DataBinder.Eval(Container.DataItem, "ExpireDateV") %>'>
                                        </asp:Label>
                                    </Itemtemplate>
                                    <EditItemTemplate>
                                        <BDP:BasicDatePicker ID="ExpireDateEdit" runat="server" AutoPostBack="True" 
                                            ButtonImageHeight="19px" ButtonImageWidth="20px" DateFormat="dd/MMM/yyyy" 
                                            DisplayType="TextBoxAndImage" ReadOnly="true" 
                                            SelectedValue='<%# DataBinder.Eval(Container.DataItem, "ExpireDate") %>' 
                                            ShowNoneButton="False" TextBoxStyle-CssClass="TextDate" ValidationGroup="Input">
                                            <TextBoxStyle CssClass="TextDate" />
                                        </BDP:BasicDatePicker>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <BDP:BasicDatePicker ID="ExpireDateAdd" runat="server" AutoPostBack="False" 
                                            ButtonImageHeight="19px" ButtonImageWidth="20px" DateFormat="dd/MMM/yyyy" 
                                            DisplayType="TextBoxAndImage" ReadOnly="true" ShowNoneButton="False" 
                                            TextBoxStyle-CssClass="TextDate" ValidationGroup="Input">
                                            <TextBoxStyle CssClass="TextDate" />
                                        </BDP:BasicDatePicker>
                                    </FooterTemplate>
                                    <HeaderStyle Width="100px" />
                                </asp:TemplateField>                                
                                <asp:TemplateField HeaderStyle-Width="126" HeaderText="Action">
                                    <Itemtemplate>
                                        <asp:Button ID="btnEdit" runat="server" class="bitbtndt btnedit" 
                                            CommandName="Edit" Text="Edit" />
                                        <asp:Button ID="btndelete" runat="server" class="bitbtndt btndelete" 
                                            CommandName="Delete" 
                                            OnClientClick="return confirm('Sure to delete this data?');" Text="Delete" />
                                    </Itemtemplate>
                                    <EditItemTemplate>
                                        <asp:Button ID="btnUpdate" runat="server" class="bitbtndt btnsave" 
                                            CommandName="Update" Text="Save" />
                                        <asp:Button ID="btnCancel" runat="server" class="bitbtndt btncancel" 
                                            CommandName="Cancel" Text="Cancel" />
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:Button ID="btnAdd" runat="server" class="bitbtndt btnadd" 
                                            CommandName="Insert" Text="Add" />
                                    </FooterTemplate>
                                    <HeaderStyle Width="126px" />
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </asp:Panel>
                     <br />
                     <br />
              </div>   
           </asp:Panel> 
           
           <asp:Panel runat="server" ID="Panel3">
               <asp:GridView ID="DataGridView" runat="server" AutoGenerateColumns="False" ShowFooter="False">
                   <HeaderStyle CssClass="GridHeader" />
                   <RowStyle CssClass="GridItem" Wrap="false" />
                   <AlternatingRowStyle CssClass="GridAltItem" />
                   <PagerStyle CssClass="GridPager" />
                   <Columns>
                       <asp:BoundField DataField="LotNo" HeaderStyle-Width="150px" HeaderText="Lot No"></asp:BoundField>
                       <asp:BoundField DataField="Qty" HeaderStyle-Width="80px" HeaderText="Qty /Roll"></asp:BoundField>
                       <asp:BoundField DataField="PalletNo" HeaderStyle-Width="150px" HeaderText="Pallet No"></asp:BoundField>
                       <asp:BoundField DataField="QtyPackage" HeaderStyle-Width="80px" HeaderText="Qty Package"></asp:BoundField>
                       <asp:BoundField DataField="ExpireDate" HeaderStyle-Width="80px" HeaderText="Expire Date"></asp:BoundField>                       
                   </Columns>
               </asp:GridView>
           </asp:Panel>
           </asp:View>   
                   
        </asp:MultiView>
        <asp:Button ID="btnHome2" runat="server" class="bitbtndt btnback" Text="Home" />
      <br />
       
       <%--<asp:SqlDataSource ID="dsAccClass" runat="server" SelectCommand="SELECT DISTINCT Class_Code, Class_Account FROM VMsaccount WHERE FgType = 'PL'">
       </asp:SqlDataSource> --%>
       
     <asp:Label ID="lstatus" ForeColor="red" runat="server"></asp:Label>
    </div>
    </form>
</body>

</html>
