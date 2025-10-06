<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TrSTSJ.aspx.vb" Inherits="Transaction_TrSTSJ_TrSTSJ" %>
<%@ Register Assembly="BasicFrame.WebControls.BasicDatePicker" Namespace="BasicFrame.WebControls"
    TagPrefix="BDP" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Untitled Page</title>
    <script src="../../Function/OpenDlg.js" type="text/javascript"></script>
    <script src="../../Function/Function.js" type="text/javascript"></script>
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
            document.getElementById("tbQtyM2").value = setdigit(document.getElementById("tbQtyM2").value.replace(/\$|\,/g,""),'<%=VIEWSTATE("DigitQty")%>');                    
            document.getElementById("tbQtyRoll").value = setdigit(document.getElementById("tbQtyRoll").value.replace(/\$|\,/g,""),'<%=VIEWSTATE("DigitQty")%>');
            document.getElementById("tbQtyWrhs").value = setdigit(document.getElementById("tbQtyWrhs").value.replace(/\$|\,/g,""),'<%=VIEWSTATE("DigitQty")%>');
            document.getElementById("tbQtyOrder").value = setdigit(document.getElementById("tbQtyOrder").value.replace(/\$|\,/g,""),'<%=VIEWSTATE("DigitQty")%>');
            document.getElementById("tbQtyPacking").value = setdigit(document.getElementById("tbQtyPacking").value.replace(/\$|\,/g,""),'<%=VIEWSTATE("DigitQty")%>');                    
            document.getElementById("tbQtyPerPack").value = setdigit(document.getElementById("tbQtyPerPack").value.replace(/\$|\,/g,""),'<%=VIEWSTATE("DigitQty")%>');
            document.getElementById("tbNetWeight").value = setdigit(document.getElementById("tbNetWeight").value.replace(/\$|\,/g,""),'<%=VIEWSTATE("DigitQty")%>');
            document.getElementById("tbGrossWeight").value = setdigit(document.getElementById("tbGrossWeight").value.replace(/\$|\,/g,""),'<%=VIEWSTATE("DigitQty")%>');
                        
            
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
        <div class="H1">Delivery Note</div>
        <hr style="color:Blue" />
        <asp:Panel runat="server" id="PnlHd"> 
      <table>
        <tr>
            <td style="width:80px;text-align:right">Quick Search :</td>            
            <td><asp:TextBox CssClass="TextBox" runat="server" ID ="tbFilter"/> 
                  <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlField" >
                    <asp:ListItem Selected="True" Value="TransNmbr">DN No</asp:ListItem>
                    <asp:ListItem Value="dbo.FormatDate(TransDate)">DN Date</asp:ListItem>
                    <asp:ListItem>Status</asp:ListItem>
                    <asp:ListItem Value="Customer">Customer Code</asp:ListItem>
                    <asp:ListItem Value="Customer_Name">Customer Name</asp:ListItem>
                    <asp:ListItem Value="SONo">SO No</asp:ListItem>
                    <asp:ListItem Value="DONo">DO No</asp:ListItem>
                    <asp:ListItem Value="CustPONo">PO Cust No</asp:ListItem>        
                    <asp:ListItem Value="Warehouse_Code">Warehouse Code</asp:ListItem>
                    <asp:ListItem Value="Warehouse_Name">Warehouse Name</asp:ListItem>            
                    <asp:ListItem Value="SubLed_Code">SubLed Code</asp:ListItem>
                    <asp:ListItem Value="SubLed_Name">SubLed Name</asp:ListItem>
                    <asp:ListItem Value="Issued_By">Issued By</asp:ListItem>
                    <asp:ListItem Value="CarNo">CarNo</asp:ListItem>
                    <asp:ListItem Value="Driver">Driver</asp:ListItem>
                    <asp:ListItem Value="DeliveryTime">Delivery Time</asp:ListItem>
                    <asp:ListItem Value="DeliveryBy">Delivery By</asp:ListItem>
                    <asp:ListItem Value="DeliveryCostBy">Delivery Cost By</asp:ListItem>
                    <asp:ListItem Value="Remark">Remark</asp:ListItem>                     
                    </asp:DropDownList>
                  <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlRange"></asp:DropDownList>  
                  <asp:Button class="bitbtn btnsearch" runat="server" ID="btnSearch" Text="Search" />                  
                  <asp:Button class="btngo" runat="server" ID="btnExpand" Text="..."/>  
            </td>
            <td>
                <asp:LinkButton ID="LbAdvSearch" runat="server" Text="Advanced Search" />
                &nbsp &nbsp &nbsp &nbsp
                <asp:Label runat="server" ID="Label13" Text="Outstanding DO : "/>
                <asp:LinkButton runat="server" ID="lbCount" Text="X" ForeColor="#FF6600" Font-Size="Small" />
                <asp:Label runat="server" ID="Label14" Text=" record(s)"/>
            </td>
            <td class="style1">&nbsp;</td>
            <td>Show Records :</td>
            <td>
                <asp:DropDownList ID="ddlRow" runat="server" AutoPostBack="True" 
                    CssClass="DropDownList">
                    <asp:ListItem Selected="True" Value="15">Choose One</asp:ListItem>
                    <asp:ListItem>20</asp:ListItem>
                    <asp:ListItem>30</asp:ListItem>
                    <asp:ListItem>40</asp:ListItem>
                    <asp:ListItem>50</asp:ListItem>
                    <asp:ListItem>100</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td>Rows</td> 
        </tr>
      </table>
      <asp:Panel runat="server" ID="pnlSearch" Visible="false">
      <table>
        <tr>
          <td style="width:100px;text-align:right">
              <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlNotasi" >
                <asp:ListItem Selected="true" Text="OR" Value="OR"></asp:ListItem>
                <asp:ListItem Text="AND" Value="AND"></asp:ListItem>            
              </asp:DropDownList>
          </td>
          <td>
           
              <asp:TextBox runat="server" CssClass="TextBox" ID ="tbfilter2"/> 
              <asp:DropDownList runat="server" CssClass="DropDownList" ID="ddlField2" >
                    <asp:ListItem Selected="True" Value="TransNmbr">DN No</asp:ListItem>
                    <asp:ListItem Value="dbo.FormatDate(TransDate)">DN Date</asp:ListItem>
                    <asp:ListItem>Status</asp:ListItem>
                    <asp:ListItem Value="Customer">Customer Code</asp:ListItem>
                    <asp:ListItem Value="Customer_Name">Customer Name</asp:ListItem>
                    <asp:ListItem Value="SONo">SO No</asp:ListItem>
                    <asp:ListItem Value="DONo">DO No</asp:ListItem>
                    <asp:ListItem Value="CustPONo">PO Cust No</asp:ListItem>        
                    <asp:ListItem Value="Warehouse_Code">Warehouse Code</asp:ListItem>
                    <asp:ListItem Value="Warehouse_Name">Warehouse Name</asp:ListItem>            
                    <asp:ListItem Value="SubLed_Code">SubLed Code</asp:ListItem>
                    <asp:ListItem Value="SubLed_Name">SubLed Name</asp:ListItem>
                    <asp:ListItem Value="Issued_By">Issued By</asp:ListItem>
                    <asp:ListItem Value="CarNo">CarNo</asp:ListItem>
                    <asp:ListItem Value="Driver">Driver</asp:ListItem>
                    <asp:ListItem Value="DeliveryTime">Delivery Time</asp:ListItem>
                    <asp:ListItem Value="DeliveryBy">Delivery By</asp:ListItem>
                    <asp:ListItem Value="DeliveryCostBy">Delivery Cost By</asp:ListItem>
                    <asp:ListItem Value="Remark">Remark</asp:ListItem>                     
              </asp:DropDownList>
          </td>              
        </tr>        
      </table>      
      </asp:Panel>
          <br />
            <asp:Button class="bitbtn btnadd" runat="server" ID="BtnAdd" Text="Add" />
            
            &nbsp &nbsp &nbsp   
            <asp:DropDownList CssClass="DropDownList" ID="ddlCommand" runat="server" Visible="false"/>
            <asp:Button class="btngo" Visible="false" runat="server" OnClick = "OnConfirm" OnClientClick = "Confirm()" ID="BtnGo" Text="G"/>
          <br />
          <div style="border:0px  solid; width:100%; height:100%; overflow:auto;">
          <asp:GridView ID="GridView1" runat="server" AllowPaging="True" AllowSorting="true"
            CssClass="Grid" AutoGenerateColumns="false"> 
              <HeaderStyle CssClass="GridHeader"></HeaderStyle>
						<RowStyle CssClass="GridItem" Wrap="false" />
						<AlternatingRowStyle CssClass="GridAltItem"/>
						<PagerStyle CssClass="GridPager" />
              <Columns>              
                  <asp:TemplateField>
                      <HeaderTemplate>
                          <asp:CheckBox ID="cbSelectHd" runat="server" AutoPostBack="true"
                          oncheckedchanged="cbSelectHd_CheckedChanged" />
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
                              <asp:ListItem Text="Input Lot No" />                              
                              <%--<asp:ListItem Text="Delete" />--%>
                              <asp:ListItem Text="Print" />
                              <asp:ListItem Text="Complete" />
                              <asp:ListItem Text="Un-Complete" />
                              <asp:ListItem Text="Cancel" />
                          </asp:DropDownList>
                          <asp:Button class="btngo" runat="server" ID="BtnGo" Text="G" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" CommandName="Go" />
                      </ItemTemplate>
                      <HeaderStyle Width="110px" />
                  </asp:TemplateField>
                  <asp:BoundField DataField="TransNmbr" HeaderStyle-Width="120px" SortExpression="Nmbr" HeaderText="DN No"></asp:BoundField>                  
                  <asp:BoundField DataField="Status" HeaderText="Status"></asp:BoundField>                  
                  <asp:BoundField DataField="TransDate" dataformatstring="{0:dd MMM yyyy}" htmlencode="true" HeaderStyle-Width="80px" SortExpression="TransDate" HeaderText="DN Date"></asp:BoundField>
                  <asp:BoundField DataField="Customer" HeaderStyle-Width="80px" SortExpression="Customer" HeaderText="Customer Code"></asp:BoundField>
                  <asp:BoundField DataField="Customer_Name" HeaderStyle-Width="200px" SortExpression="Customer_Name" HeaderText="Customer Name"></asp:BoundField>
                  <asp:BoundField DataField="SONo" HeaderStyle-Width="80px" SortExpression="SONo" HeaderText="SO No"></asp:BoundField>
                  <asp:BoundField DataField="DONo" HeaderStyle-Width="80px" SortExpression="DONo" HeaderText="DO No"></asp:BoundField>
                  <asp:BoundField DataField="CustPONo" HeaderStyle-Width="80px" SortExpression="CustPONo" HeaderText="PO Cust No"></asp:BoundField>
                  <asp:BoundField DataField="Warehouse_Code" HeaderStyle-Width="80px" SortExpression="Warehouse_Code" HeaderText="Warehouse Code"></asp:BoundField>
                  <asp:BoundField DataField="Warehouse_Name" HeaderStyle-Width="200px" SortExpression="Warehouse_Name" HeaderText="Warehouse Name"></asp:BoundField>
                  <asp:BoundField DataField="SubLed_Code" HeaderStyle-Width="80px" SortExpression="SubLed_Code" HeaderText="SubLed Code"></asp:BoundField>
                  <asp:BoundField DataField="SubLed_Name" HeaderStyle-Width="200px" SortExpression="SubLed_Name" HeaderText="SubLed Name"></asp:BoundField>
                  <asp:BoundField DataField="Issued_By" HeaderStyle-Width="80px" SortExpression="Issued_By" HeaderText="Issued By"></asp:BoundField>
                  <asp:BoundField DataField="CarNo" HeaderStyle-Width="200px" SortExpression="CarNo" HeaderText="Car No"></asp:BoundField>
                  <asp:BoundField DataField="Driver" HeaderStyle-Width="200px" SortExpression="Driver" HeaderText="Driver"></asp:BoundField>
                  <asp:BoundField DataField="DeliveryTime" HeaderStyle-Width="200px" SortExpression="DeliveryTime" HeaderText="Delivery Time"></asp:BoundField>
                  <asp:BoundField DataField="DeliveryBy" HeaderStyle-Width="200px" SortExpression="DeliveryBy" HeaderText="Delivery By"></asp:BoundField>
                  <asp:BoundField DataField="DeliveryCostBy" HeaderStyle-Width="200px" SortExpression="DeliveryCostBy" HeaderText="Delivery Cost By"></asp:BoundField>
                  <asp:BoundField DataField="Remark" HeaderStyle-Width="250px" HeaderText="Remark"></asp:BoundField>
              </Columns>
          </asp:GridView>
          </div>
            <asp:Panel runat="server" ID ="pnlNav" Visible="false">
            <asp:Button class="bitbtn btnadd" runat="server" ID="btnAdd2" Text="Add" />
            &nbsp &nbsp &nbsp
            <asp:DropDownList ID="ddlCommand2" CssClass="DropDownList" runat="server"/>
            <asp:Button class="btngo" runat="server" OnClick = "OnConfirm2" OnClientClick = "Confirm2()" ID="btnGo2" Text="G"/>  
            </asp:Panel>
    </asp:Panel>    
    <asp:Panel runat="server" ID="pnlInput" Visible="false">
      <table>
        <tr>
            <td>DN No</td>
            <td>:</td>
            <td><asp:TextBox CssClass="TextBoxR" runat="server" ID="tbCode" Width="150px" Enabled="False"/></td>        
            <td>DN Date</td>
            <td>:</td>
            <td>
            <BDP:BasicDatePicker ID="tbDate" runat="server" DateFormat="dd MMM yyyy" 
                        ReadOnly = "true" ValidationGroup="Input"
                        ButtonImageHeight="19px" ButtonImageWidth="20px" 
                        DisplayType="TextBoxAndImage" 
                        TextBoxStyle-CssClass="TextDate" 
                        ShowNoneButton="False"><TextBoxStyle CssClass="TextDate" /></BDP:BasicDatePicker>                
            </td>   
            <td>Report</td>
            <td>:</td>
            <td colspan="4"><asp:DropDownList AutoPostBack="true" ValidationGroup="Input" CssClass="DropDownList" ID="ddlReport" runat="server" >
                        <asp:ListItem Selected="True">Y</asp:ListItem>
                        <asp:ListItem>N</asp:ListItem>
                    </asp:DropDownList> 
            </td>    
        </tr> 
         
        <tr>
            <td>DO No</td>
            <td>:</td>
            <td colspan="4"><asp:TextBox CssClass="TextBox" Enabled ="false" runat="server" ID="tbDONo" MaxLength="20" Width="150px" />
                <asp:Button Class="btngo" ID="btnDONo" Text="..." runat="server" ValidationGroup="Input" />                                                  
                <asp:Label ID="Label1" runat="server" ForeColor="Red">*</asp:Label>    
            </td>
        </tr> 
        <tr>
            <td>Customer</td>
            <td>:</td>
            <td colspan="4">
                <asp:TextBox CssClass="TextBox" ValidationGroup="Input" runat="server" ID="tbCustCode" MaxLength="12" AutoPostBack="true" />
                <asp:TextBox CssClass="TextBox" runat="server" ID="tbCustName" Enabled="false" MaxLength="60" Width="225px"/>
                <asp:Button Class="btngo" ID="btnCust" Text="..." runat="server" ValidationGroup="Input" />                                  
                <asp:Label ID="lbred1" runat="server" ForeColor="Red">*</asp:Label>
            </td>
        </tr>
        <tr>
            <td>SO No</td>
            <td>:</td>
            <td><asp:TextBox CssClass="TextBox" Enabled ="false" runat="server" ID="tbSONo" MaxLength="20" Width="150px" />               
            </td>
            <td>PO Cust No</td>
            <td>:</td>
            <td><asp:TextBox CssClass="TextBox" Enabled ="false" runat="server" ID="tbCustPONo" MaxLength="20" Width="150px" />
            </td>   
        </tr>         
        <tr>
            <td><asp:LinkButton ID="lbWarehouse" ValidationGroup="Input" runat="server" Text="Warehouse"/></td>
            <td>:</td>
            <td colspan="4"><asp:DropDownList CssClass="DropDownList" ValidationGroup="Input" runat="server" ID="ddlwrhs" AutoPostBack="true" Width="200px" />
                <asp:Label ID="Label2" runat="server" ForeColor="Red">*</asp:Label>               
                <asp:TextBox runat="server" ID="tbFgSubLed" Visible="false"/> 
                
                
            </td>                    
        </tr>  
        <tr>
            <td>Subled</td>
            <td>:</td>
            <td colspan="4">
                <asp:TextBox CssClass="TextBox" ValidationGroup="Input" runat="server" ID="tbSubledCode" MaxLength="10" AutoPostBack="true" />
                <asp:TextBox CssClass="TextBox" runat="server" ID="tbSubledName" Enabled="false" MaxLength="60" Width="225px"/>
                <asp:Button Class="btngo" ID="btnSubled" Text="..." runat="server" ValidationGroup="Input" />                                  
            </td>
        </tr> 
        <tr>
            <td>Vehicle No</td>
            <td>:</td>
            <td colspan="4">
                <asp:TextBox CssClass="TextBox" ValidationGroup="Input" runat="server" ID="tbVehicleCode" MaxLength="12" AutoPostBack="true" />
                <asp:TextBox CssClass="TextBox" runat="server" ID="tbVehicleName" Enabled="false" MaxLength="60" Width="225px"/>
                <asp:Button Class="btngo" ID="btnVehicle" Text="..." runat="server" ValidationGroup="Input" />                                  
                <asp:Label ID="Label3" runat="server" ForeColor="Red">*</asp:Label>
            </td>
        </tr>  
        <tr>
            <td>Issued By</td>
            <td>:</td>
            <td width="200px">
                <asp:TextBox CssClass="TextBox" runat="server" ID="tbIssuedBy" MaxLength="60" Width="200px"/>
            </td>
        </tr>
        <tr>
            <td>Driver</td>
            <td>:</td>
            <td width="200px">
                <asp:TextBox CssClass="TextBox" runat="server" ID="tbDriver" MaxLength="60" Width="200px"/>
            </td>
            <td>Delivery Time</td>
            <td>:</td>
            <td><asp:TextBox runat="server" ID="tbDeliveryTime" Width = "35px" MaxLength="5"  CssClass="TextBox" /></td>                       
        </tr>
                
        <tr>
            <td>Delivery By</td>
            <td>:</td>
            <td><asp:DropDownList CssClass="DropDownList" runat="server" ValidationGroup="Input" Width = "100px" ID="ddlDeliveryBy" AutoPostBack="True" >
                    <asp:ListItem Selected="True">Company</asp:ListItem>
                    <asp:ListItem>Expedition</asp:ListItem>
                </asp:DropDownList> 
            </td> 
            <td>Delivery Cost By</td>
            <td>:</td>
            <td><asp:DropDownList CssClass="DropDownList" runat="server" Width = "100px" ID="ddlDeliveryCostBy" Enabled="False">
                    <asp:ListItem Selected="True" Value="">Choose One</asp:ListItem>
                    <asp:ListItem>Company</asp:ListItem>
                    <asp:ListItem>Customer</asp:ListItem>
                </asp:DropDownList> 
            </td>                         
        </tr>
                     
        
        <tr>
            <td>Remark</td>
            <td>:</td>
            <td colspan="4"><asp:TextBox Width = "365px" runat="server" ValidationGroup="Input" ID="tbRemark" CssClass="TextBox" MaxLength="255" TextMode="MultiLine" />                                        
            <asp:Button class="bitbtn btngetitem" runat="server" ID="btnGetData" Text="Get Item" />        
            </td>
        </tr>
      </table>  
      
      <br />      
      <div style="font-size:medium; color:Blue;">Detail</div>
      <hr style="color:Blue" />  
        <asp:Panel runat="server" ID="pnlDt">
            <asp:Button class="bitbtndt btnadd" runat="server" ID="btnAdddt" Text="Add" ValidationGroup="Input" />                 
            <div style="border:0px  solid; overflow:auto;">
                <asp:GridView ID="GridDt" runat="server" AutoGenerateColumns="false" 
                    ShowFooter="True">
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
                                               
                        <asp:BoundField DataField="Product" HeaderStyle-Width="120px" HeaderText="Product Code" />
                        <asp:BoundField DataField="Product_Name" HeaderText="Product Name" HeaderStyle-Width="200px" />
                        <asp:BoundField DataField="Specification" HeaderStyle-Width="200px" HeaderText="Specification" />                        
                        <%--<asp:BoundField DataField="Location" HeaderStyle-Width="120px" HeaderText="Location Code" />--%>
                        <asp:TemplateField Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lbLocation" runat="server" text='<%# DataBinder.Eval(Container.DataItem, "Location") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>   
                        <asp:BoundField DataField="Location_Name" HeaderText="Location Name" HeaderStyle-Width="200px" />
                        <asp:BoundField DataField="QtyOrder" HeaderStyle-Width="80px" HeaderText="Qty Order" />
                        <asp:BoundField DataField="UnitOrder" HeaderStyle-Width="80px" HeaderText="Unit Order" />
                        <asp:BoundField DataField="Qty" HeaderStyle-Width="80px" HeaderText="Qty" />
                        <asp:BoundField DataField="Unit" HeaderStyle-Width="80px" HeaderText="Unit" />
                        <asp:BoundField DataField="QtyM2" HeaderStyle-Width="80px" HeaderText="Qty (M2)" />
                        <asp:BoundField DataField="QtyRoll" HeaderStyle-Width="80px" HeaderText="Qty (Roll)" />
                        <asp:BoundField DataField="QtyPacking" HeaderStyle-Width="80px" HeaderText="Qty Packing" />
                        <asp:BoundField DataField="UnitPacking" HeaderStyle-Width="80px" HeaderText="Unit Packing" />
                        <asp:BoundField DataField="QtyPerPack" HeaderStyle-Width="80px" HeaderText="Qty /Pack" />
                        <asp:BoundField DataField="NetWeight" HeaderStyle-Width="80px" HeaderText="Net Weight" />
                        <asp:BoundField DataField="GrossWeight" HeaderStyle-Width="80px" HeaderText="Gross Weight" />
                        <asp:BoundField DataField="Remark" HeaderStyle-Width="200px" HeaderText="Remark" />
                                                
                    </Columns>
                </asp:GridView>
          </div>   
          <asp:Button class="bitbtndt btnadd" runat="server" ID="btnAddDt2" Text="Add" ValidationGroup="Input" />                  
       </asp:Panel>             
       <asp:Panel runat="server" DefaultButton="btnSaveDt" ID="pnlEditDt" Visible="false">
       <table width="100%">
            <tr>
                <td style="width:60%">
                <table>              
                <tr>
                    <td>Product</td>
                    <td>:</td>
                    <td><asp:TextBox runat="server" ID="tbProduct" CssClass="TextBox" AutoPostBack="true" />
                        <asp:TextBox runat="server"  CssClass="TextBox" ID="tbProductName" EnableTheming="True" ReadOnly="True" Enabled="False" Width="200px"/>
                        <asp:Button ID="btnProduct" runat="server" class="btngo" Text="..."/>
                        <asp:Label ID="Label4" runat="server" ForeColor="Red">*</asp:Label>                        
                        <asp:Label ID="lbProdGrp" runat="server" >*</asp:Label> <%--Visible=False--%>
                    </td>
                </tr>   
                <tr>
                    <td>Specification</td>
                    <td>:</td>
                    <td><asp:TextBox runat="server"  CssClass="TextBox" ID="tbSpec" EnableTheming="True" ReadOnly="True" Enabled="False" Width="200px"/>
                    </td>
                </tr>
                <tr>
                    <td>Location</td>
                    <td>:</td>
                    <td>                     
                        <asp:DropDownList ID="ddlLocation" runat="server" AutoPostBack="True" CssClass="DropDownList" />
                        <asp:Label ID="Label5" runat="server" ForeColor="Red">*</asp:Label>
                    </td>
                </tr>   
                <tr>
                    <td>Qty Order</td>
                    <td>:</td>
                    <td><asp:TextBox runat="server" ID="tbQtyOrder" CssClass="TextBox" /> <%--AutoPostBack="true" --%>
                    <asp:DropDownList CssClass="DropDownList" runat="server" Enabled="false" ID="ddlUnitOrder"/>
                    <asp:Button ID="btnQtyOrder" runat="server" class="btngo" Text="..."/>
                    <asp:Label ID="Label12" runat="server" ForeColor="Red">*</asp:Label>
                    </td>                                        
                </tr>                                                   
                <tr>
                    <td>Qty Wrhs</td>
                    <td>:</td>
                    <td><asp:TextBox runat="server" ID="tbQtyWrhs" CssClass="TextBox" /> <%--AutoPostBack="true" --%>
                    <asp:DropDownList CssClass="DropDownList" runat="server" Enabled="false" ID="ddlUnitWrhs"/>
                    <asp:Button ID="btnQtyWrhs" runat="server" class="btngo" Text="..."/>
                    <asp:Label ID="Label6" runat="server" ForeColor="Red">*</asp:Label>
                    </td>                                        
                </tr>   
                <tr>
                    <td>Qty</td>
                    <td>:</td>
                    <td><asp:TextBox runat="server" ID="tbQtyM2" CssClass="TextBox" /> M2
                    <asp:Label ID="Label11" runat="server" ForeColor="Red">*</asp:Label>
                    </td>                    
                </tr>  
                <tr>
                    <td>Qty</td>
                    <td>:</td>
                    <td><asp:TextBox runat="server" ID="tbQtyRoll" CssClass="TextBox" /> Roll <%--AutoPostBack="true" --%>
                    &nbsp <asp:Button ID="btnQtyRoll" runat="server" class="btngo" Text="..."/>
                    <asp:Label ID="Label10" runat="server" ForeColor="Red">*</asp:Label>
                    </td>                    
                </tr> 
                <tr>
                    <td>Qty Packing</td>
                    <td>:</td>
                    <td><asp:TextBox runat="server" ID="tbQtyPacking" Enabled="false" CssClass="TextBox" />
                    <asp:DropDownList CssClass="DropDownList" runat="server" Enabled="false" ID="ddlUnitPacking"/></td>                    
                </tr> 
                <tr>
                    <td>Qty</td>
                    <td>:</td>
                    <td><asp:TextBox runat="server" ID="tbQtyPerPack" AutoPostBack="true" CssClass="TextBox" /> /Pack
                    <asp:Label ID="Label7" runat="server" ForeColor="Red">*</asp:Label>
                    </td>                    
                </tr>   
                <tr>
                    <td>Net Weight</td>
                    <td>:</td>
                    <td><asp:TextBox runat="server" ID="tbNetWeight" CssClass="TextBox" /> gr
                    <asp:Label ID="Label8" runat="server" ForeColor="Red">*</asp:Label>
                    </td>                    
                </tr> 
                <tr>
                    <td>Gross Weight</td>
                    <td>:</td>
                    <td><asp:TextBox runat="server" ID="tbGrossWeight" CssClass="TextBox" /> gr
                    <asp:Label ID="Label9" runat="server" ForeColor="Red">*</asp:Label>
                    </td>                    
                </tr>               
                <tr>
                    <td>Remark</td>
                    <td>:</td>
                    <td><asp:TextBox Width = "365px" runat="server" ValidationGroup="Input" ID="tbRemarkDt" CssClass="TextBox" MaxLength="255" TextMode="MultiLine" />                                        
                    </td>
                </tr>                                    
                </table>
                </td>
                <td style="vertical-align:top;width:40%">
               		<asp:Panel runat="server" ID="PnlInfo" Visible="false" Height="100%" Width="100%">
                        <asp:Label ID="lbInfo" runat="server" ForeColor="Blue" Font-Bold="true" Text="Info Stock :"></asp:Label>
                        <br />
                        <div style="border:0px  solid; width:100%; height:100%; overflow:auto;">
                            <asp:GridView ID="GridInfo" runat="server" AutoGenerateColumns="false" ShowFooter="true">
                                <HeaderStyle CssClass="GridHeader" />
                                <RowStyle CssClass="GridItem" Wrap="false" />
                                <AlternatingRowStyle CssClass="GridAltItem" />
                                <PagerStyle CssClass="GridPager" />
                                <Columns>
                                    <asp:BoundField DataField="Code" HeaderStyle-Width="120px" HeaderText="Location" />
                                    <asp:BoundField DataField="Qty" HeaderStyle-Width="70px" HeaderText="Qty" />
                                    <asp:BoundField DataField="QtyMin" HeaderStyle-Width="70px" HeaderText="Qty Min" />
                                </Columns>
                            </asp:GridView>
                        </div>
                    </asp:Panel>
                </td>
            </tr>
        </table>   
            <br />                     
            <asp:Button ID="btnSaveDt" runat="server" class="bitbtndt btnsave" Text="Save"/> &nbsp;         
            <asp:Button ID="btnCancelDt" runat="server" class="bitbtndt btncancel" Text="Cancel"/> 
       </asp:Panel> 
       
       <br />          
       <asp:Button ID="btnSaveAll" runat="server" class="bitbtndt btnsave" 
            Text="Save & New" ValidationGroup="Input" Width="97px"/> &nbsp;    
        <asp:Button ID="btnSaveTrans" runat="server" class="bitbtndt btnsave" Text="Save" ValidationGroup="Input"/> &nbsp;    
        <asp:Button ID="btnBack" runat="server" class="bitbtndt btncancel" Text="Cancel" ValidationGroup="Input"/>  &nbsp;
        <asp:Button ID="btnHome" runat="server" class="bitbtndt btncancel" Text="Home"/> 
    </asp:Panel>
    <asp:Panel runat="server" ID="pnlComplete" Visible="false">
      <table>
        <tr>
            
            <td>Received Date</td>
            <td>:</td>
            <td>
                <BDP:BasicDatePicker ID="tbRRDate" runat="server" DateFormat="dd MMM yyyy" 
                        ReadOnly = "true" ValidationGroup="Input"
                        ButtonImageHeight="19px" ButtonImageWidth="20px" 
                        DisplayType="TextBoxAndImage" 
                        TextBoxStyle-CssClass="TextDate" 
                        ShowNoneButton="False"><TextBoxStyle CssClass="TextDate" />
                </BDP:BasicDatePicker>                
            </td>   
            
        </tr> 
         
        <tr>
            <td>Vehicle No</td>
            <td>:</td>
            <td>
                <asp:TextBox CssClass="TextBox" ValidationGroup="Input" runat="server" ID="tbRRVehicleCode" MaxLength="12" AutoPostBack="true" />
                <asp:TextBox CssClass="TextBox" runat="server" ID="tbRRVehicleName" Enabled="false" MaxLength="60" Width="225px"/>
                <asp:Button Class="btngo" ID="btnRRVehicle" Text="..." runat="server" ValidationGroup="Input" />                                  
            </td>
        </tr>  
        <tr>
            <td>Driver</td>
            <td>:</td>
            <td>
                <asp:TextBox CssClass="TextBox" runat="server" ID="tbRRDriver" MaxLength="60" Width="200px"/>
            </td>
            
        </tr>
        <tr>
            <td>Expedition Name</td>
            <td>:</td>
            <td>
                <asp:TextBox CssClass="TextBox" runat="server" ID="tbRRExpeditionName" Enabled="False" MaxLength="60" Width="200px"/>
            </td>
        </tr>
        <tr>
            <td>Expedition No</td>
            <td>:</td>
            <td>
                <asp:TextBox CssClass="TextBox" runat="server" ID="tbRRExpeditionNo" Enabled="False" MaxLength="60" Width="200px"/>
            </td>
            
        </tr> 
        <tr>
            <td>Time</td>
            <td>:</td>
            <td colspan="3">
                <table>
                <tr style="background-color:Silver;text-align:center">
                   <td>Arrival</td>
                   <td>Unloading</td>
                   <td>Finish</td>
                 </tr>
                 <tr>
                     <td>
                        <asp:TextBox runat="server" ID="tbArrivalTime" Width = "55px" MaxLength="5"  CssClass="TextBox" />
                     </td>
                     <td>
                        <asp:TextBox runat="server" ID="tbUnloadingTime" Width = "55px" MaxLength="5"  CssClass="TextBox" />                
                     </td>
                     <td>
                        <asp:TextBox runat="server" ID="tbFinishTime" Width = "55px" MaxLength="5"  CssClass="TextBox" />                
                     </td>
                 </tr>
                 </table>
            </td>                         
        </tr>      
                           
        
        <tr>
            <td>Remark</td>
            <td>:</td>
            <td colspan="4"><asp:TextBox Width = "365px" runat="server" ValidationGroup="Input" ID="tbRRRemark" CssClass="TextBox" MaxLength="255" TextMode="MultiLine" />                                        
                   
            </td>
        </tr>
      </table>  
      
      <br />      
      <div style="font-size:medium; color:Blue;">Detail</div>
      <hr style="color:Blue" />  
        <asp:Panel runat="server" ID="Panel5">
            
            <div style="border:0px  solid; overflow:auto;">
                <asp:GridView ID="GridCompleteDt" runat="server" AutoGenerateColumns="false" ShowFooter="False">
                    <HeaderStyle CssClass="GridHeader" />
                    <RowStyle CssClass="GridItem" Wrap="false" />
                    <AlternatingRowStyle CssClass="GridAltItem" />
                    <PagerStyle CssClass="GridPager" />
                    <Columns>
                            <asp:TemplateField HeaderText="Product" HeaderStyle-Width="100" SortExpression="Product">
								<Itemtemplate>
									<asp:Label Runat="server" ID="ProductCode" text='<%# DataBinder.Eval(Container.DataItem, "Product") %>'>
									</asp:Label>
								</Itemtemplate>			
								<EditItemTemplate>
								    <asp:Label runat="server" ID="ProductCodeEdit" text='<%# DataBinder.Eval(Container.DataItem, "Product") %>' />
								</EditItemTemplate>	
								<FooterTemplate>								    
								    <asp:Label runat="server" ID="ProductCodeAdd" text='<%# DataBinder.Eval(Container.DataItem, "Product") %>' />							    
								</FooterTemplate>											
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Product Name" HeaderStyle-Width="100" SortExpression="ProductName">
								<Itemtemplate>
									<asp:Label Runat="server" ID="ProductName" text='<%# DataBinder.Eval(Container.DataItem, "ProductName") %>'>
									</asp:Label>
								</Itemtemplate>			
								<EditItemTemplate>
								    <asp:Label runat="server" ID="ProductNameEdit" text='<%# DataBinder.Eval(Container.DataItem, "ProductName") %>' />
								</EditItemTemplate>	
								<FooterTemplate>								    
								    <asp:Label runat="server" ID="ProductNameAdd" text='<%# DataBinder.Eval(Container.DataItem, "ProductName") %>' />							    
								</FooterTemplate>											
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Location" HeaderStyle-Width="100" SortExpression="Location">
								<Itemtemplate>
									<asp:Label Runat="server" ID="LocationCode" text='<%# DataBinder.Eval(Container.DataItem, "Location") %>'>
									</asp:Label>
								</Itemtemplate>			
								<EditItemTemplate>
								    <asp:Label runat="server" ID="LocationCodeEdit" text='<%# DataBinder.Eval(Container.DataItem, "Location") %>' />
								</EditItemTemplate>	
								<FooterTemplate>								    
								    <asp:Label runat="server" ID="LocationCodeAdd" text='<%# DataBinder.Eval(Container.DataItem, "Location") %>' />							    
								</FooterTemplate>											
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Location Name" HeaderStyle-Width="100" SortExpression="Location_Name">
								<Itemtemplate>
									<asp:Label Runat="server" ID="LocationName" text='<%# DataBinder.Eval(Container.DataItem, "Location_Name") %>'>
									</asp:Label>
								</Itemtemplate>			
								<EditItemTemplate>
								    <asp:Label runat="server" ID="LocationNameEdit" text='<%# DataBinder.Eval(Container.DataItem, "Location_Name") %>' />
								</EditItemTemplate>	
								<FooterTemplate>								    
								    <asp:Label runat="server" ID="LocationNameAdd" text='<%# DataBinder.Eval(Container.DataItem, "Location_Name") %>' />							    
								</FooterTemplate>											
							</asp:TemplateField>
										            							
							<asp:TemplateField HeaderText="Qty Wrhs" HeaderStyle-Width="80" SortExpression="Qty">
								<Itemtemplate>
									<asp:Label Runat="server" Width="80" ID="QtyWrhs" text='<%# DataBinder.Eval(Container.DataItem, "Qty") %>'>
									</asp:Label>
								</Itemtemplate>			
								<EditItemTemplate>
								    <asp:Label runat="server" ID="QtyWrhsEdit" text='<%# DataBinder.Eval(Container.DataItem, "Qty") %>' />
								</EditItemTemplate>	
								<FooterTemplate>								    
								    <asp:Label runat="server" ID="QtyWrhsAdd" text='<%# DataBinder.Eval(Container.DataItem, "Qty") %>' />							    
								</FooterTemplate>											
							</asp:TemplateField>	
																											
							<asp:TemplateField HeaderText="Qty Received" HeaderStyle-Width="70" SortExpression="QtyRR">
								<Itemtemplate>
									<asp:Label Runat="server" Width="100%" ID="QtyRR" text='<%# DataBinder.Eval(Container.DataItem, "QtyRR") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>									
									<asp:TextBox Runat="server" CssClass="TextBox" ID="QtyRREdit" Width="90%" 
									OnTextChanged ="QtyRREdit_TextChanged" AutoPostBack="True" Text='<%# DataBinder.Eval(Container.DataItem, "QtyRR") %>'>									
									</asp:TextBox>																		
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="QtyRRAdd" CssClass="TextBox" Width="90%" Runat="Server"/>								    
								</FooterTemplate>
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Qty Loss" HeaderStyle-Width="70" SortExpression="QtyLoss">
								<Itemtemplate>
									<asp:Label Runat="server" Width="100%" ID="QtyLoss" text='<%# DataBinder.Eval(Container.DataItem, "QtyLoss") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>									
									<asp:TextBox Runat="server" CssClass="TextBox" ID="QtyLossEdit" Width="90%" 
									OnTextChanged ="QtyLossEdit_TextChanged" AutoPostBack="True" Text='<%# DataBinder.Eval(Container.DataItem, "QtyLoss") %>'>
									</asp:TextBox>																		
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="QtyLossAdd" CssClass="TextBox" Width="90%" Runat="Server"/>								    
								</FooterTemplate>
							</asp:TemplateField>
																																																															
							<asp:TemplateField HeaderText="Unit Wrhs" SortExpression="Unit">
								<Itemtemplate>
									<asp:Label Runat="server" ID="UnitWrhs" text='<%# DataBinder.Eval(Container.DataItem, "Unit") %>'>
									</asp:Label>
								</Itemtemplate>			
								<EditItemTemplate>
								    <asp:Label runat="server" ID="UnitWrhsEdit" text='<%# DataBinder.Eval(Container.DataItem, "Unit") %>' />
								</EditItemTemplate>	
								<FooterTemplate>								    
								    <asp:Label runat="server" ID="UnitWrhsAdd" text='<%# DataBinder.Eval(Container.DataItem, "Unit") %>' />							    
								</FooterTemplate>											
							</asp:TemplateField>							
							
							<asp:TemplateField HeaderText="Action" HeaderStyle-Width="126px" ItemStyle-Wrap="false">
								<ItemTemplate>
                                    <asp:Button ID="btnEdit" runat="server" class="bitbtndt btnedit" Text="Edit" CommandName="Edit" />
                                    <asp:Button ID="btnDelete" runat="server" class="bitbtndt btndelete" Text="Delete"
                                        OnClientClick="return confirm('Sure to delete this data?');" Visible="False"  CommandName="Delete" />                                    
								</ItemTemplate>																
								<EditItemTemplate>
									<asp:Button ID="btnUpdate" runat="server" class="bitbtndt btnsave" Text="Save" CommandName="Update"/>
                                    <asp:Button ID="btnCancel" runat="server" class="bitbtndt btncancel" Text="Cancel"  CommandName="Cancel"/>   
								</EditItemTemplate>
								<%--<FooterTemplate>
								    <asp:Button class="bitbtn btnadd" CommandName="Insert" runat="server" ID="Button1" Text="Add" />									
								</FooterTemplate>--%>
							</asp:TemplateField>	                       
                                                
                    </Columns>
                </asp:GridView>
          </div>   
          
       </asp:Panel>             
       <asp:Button ID="btnComplete" runat="server" class="bitbtndt btnsave" Width="75px" Text="Complete" ValidationGroup="Input"/> &nbsp;    
        <asp:Button ID="btnCancelComplete" runat="server" class="bitbtndt btncancel" Text="Cancel" ValidationGroup="Input"/>  &nbsp; 
       
       <br />          
       
    </asp:Panel>
    <asp:Panel runat="server" ID="pnlCancel" Visible="false">
      <table>
        <tr>
            
            <td>Received Date</td>
            <td>:</td>
            <td>
                <BDP:BasicDatePicker ID="tbCancelDate" runat="server" DateFormat="dd MMM yyyy" 
                        ReadOnly = "true" ValidationGroup="Input"
                        ButtonImageHeight="19px" ButtonImageWidth="20px" 
                        DisplayType="TextBoxAndImage" 
                        TextBoxStyle-CssClass="TextDate" 
                        ShowNoneButton="False"><TextBoxStyle CssClass="TextDate" />
                </BDP:BasicDatePicker>                
            </td>   
            
        </tr> 
                 
        <tr>
            <td>Reason Cancel</td>
            <td>:</td>
            <td colspan="4"><asp:TextBox Width = "365px" runat="server" ValidationGroup="Input" ID="tbReasonCancel" CssClass="TextBox" MaxLength="255" TextMode="MultiLine" />                                        
                   
            </td>
        </tr>
      </table>  
      
                    
       <asp:Button ID="btnCancelTrans" runat="server" class="bitbtndt btnsave" Width="75px" Text="Cancel" ValidationGroup="Input"/> &nbsp;    
        <asp:Button ID="btnCancelCancel" runat="server" class="bitbtndt btncancel" Text="Home" ValidationGroup="Input"/>  &nbsp; 
       
       <br />          
       
    </asp:Panel>
    </div>     
    <asp:Label runat ="server" ID="lbStatus" ForeColor="Red"/>
    </form>
    
</body>
</html>
