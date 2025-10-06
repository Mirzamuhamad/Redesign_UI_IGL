<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TrSTRRPO.aspx.vb" Inherits="Transaction_TrSTRRPO_TrSTRRPO" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="BasicFrame.WebControls.BasicDatePicker" Namespace="BasicFrame.WebControls"
    TagPrefix="BDP" %>
<%@ Register assembly="CrystalDecisions.Web, Version=10.5.3700.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>
<%@ Register Assembly="BasicFrame.WebControls.BasicDatePicker" Namespace="BasicFrame.WebControls"
    TagPrefix="BInv" %>
<%@ Register assembly="FastReport" namespace="FastReport.Web" tagprefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Untitled Page</title>
    <script src="../../Function/OpenDlg.js" type="text/javascript"></script>
    <script src="../../Function/Function.JS" type="text/javascript"></script>
          <link type="text/css" rel="stylesheet" href="../../Styles/circularprogress.css" />
    <script type="text/javascript" src="../../JQuery/jquery.min.js"></script>
    <script type="text/javascript">

        function ProgressCircle() {
            setTimeout(function() {
                var modal = $('<div />');
                modal.addClass("modal");
                $('body').append(modal);
                var loading = $(".loading");
                loading.show();
                var top = Math.max($(window).height() / 2 - loading[0].offsetHeight / 2, 0);
                var left = Math.max($(window).width() / 2 - loading[0].offsetWidth / 2, 0);
                loading.css({ top: top, left: left });
            }, 200);
        }
        $('form').live("submit", function() {
            ProgressCircle();
        });
        
               
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
    
        function setformat(_prmchange)
        {
        try
         {         
        
        var Qty = parseFloat(document.getElementById("tbQty").value.replace(/\$|\,/g,"")); 
        var QtyOrder = parseFloat(document.getElementById("tbQtyOrder").value.replace(/\$|\,/g,"")); 
       
        
        if(isNaN(QtyOrder) == true)
        {
           QtyOrder = 0;
        }
         
        document.getElementById("tbQty").value = setdigit(Qty,'<%=ViewState("DigitQty")%>');
        document.getElementById("tbQtyOrder").value = setdigit(QtyOrder,'<%=ViewState("DigitQty")%>');
         }catch (err){
            alert(err.description);
          }      
        } 
        
            function deletetrans()
        {
            try
            {
                
                 var result = confirm("Sure Delete Transaction ?");
                if (result){
                    document.getElementById("HiddenRemarkDelete").value = "true";
                } else {
                    document.getElementById("HiddenRemarkDelete").value = "false";
                }
                postback();
                //document.form1.submit();                
            }catch(err){
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
</head>
<body>     
    <form id="form1" runat="server">    
    <div class="Content">
    <div class="H1"><asp:Label runat="server" ID="lblTitle" Text="Receiving Report"></asp:Label></div>
     <hr style="color:Blue" />        
    <asp:Panel runat="server" id="PnlHd"> 
      <table>
        <tr>
            <td style="width:100px;text-align:right">Quick Search :</td>            
            <td><asp:TextBox CssClass="TextBox" runat="server" ID ="tbFilter"/> 
                  <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlField" >
                      <asp:ListItem Value="TransNmbr" Selected="True">Reference</asp:ListItem>
                      <asp:ListItem Value="Status">Status</asp:ListItem>
                     <asp:ListItem Value="PONo">PO NO</asp:ListItem>
                      <asp:ListItem Value="FgReport">Report</asp:ListItem>
                      <asp:ListItem Value="FgHome">Home</asp:ListItem>
                      <asp:ListItem Value="ProductCode">Product Code</asp:ListItem>
                      <asp:ListItem Value="ProductName">Product Name</asp:ListItem>
                      <asp:ListItem Value="Supplier">Supplier</asp:ListItem>
                      <asp:ListItem Value="Supplier_Name">Supplier Name</asp:ListItem>                      
                      <asp:ListItem Value="ShipTo">Ship To</asp:ListItem>                      
                      <asp:ListItem Value="ShipToName">Ship To Name</asp:ListItem>                      
                      <asp:ListItem Value="Wrhs_Name">Warehouse</asp:ListItem>
                      <asp:ListItem Value="SJSuppNo">SJ Supp. No</asp:ListItem>
                      <asp:ListItem Value="SJSuppDate">SJ Supp. Date</asp:ListItem>
                      <asp:ListItem Value="CarNo">Car No</asp:ListItem>
                      <asp:ListItem Value="Driver">Driver</asp:ListItem>
                      <asp:ListItem>Remark</asp:ListItem>
                  </asp:DropDownList>
                  <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlRange"></asp:DropDownList>
                 
                  <asp:Button class="bitbtn btnsearch" runat="server" ID="btnSearch" Text="Search" />																						 											  
                  <asp:Button class="bitbtn btngo" runat="server" ID="btnExpand" Text="..." />																					 											  
            </td>
            <td>
                <asp:LinkButton ID="LbAdvSearch" runat="server" Text="Advanced Search" />
                &nbsp &nbsp &nbsp &nbsp
                <asp:Label runat="server" ID="Label1" Text="Outstanding PO : "/>
                <asp:LinkButton runat="server" ID="lbCount" Text="X" ForeColor="#FF6600" Font-Size="Small" />
                <asp:Label runat="server" ID="Label2" Text=" record(s)"/>
            </td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                Show Records</td>
            <td>
                :</td>
            <td>
                <asp:DropDownList ID="ddlRow" runat="server" AutoPostBack="True" 
                    CssClass="DropDownList">
                    <asp:ListItem Selected="true" >10</asp:ListItem>
                    <asp:ListItem>20</asp:ListItem>
                    <asp:ListItem>30</asp:ListItem>
                    <asp:ListItem>40</asp:ListItem>
                    <asp:ListItem>50</asp:ListItem>
                    <asp:ListItem>100</asp:ListItem>
                    <asp:ListItem>200</asp:ListItem>
                    <asp:ListItem>300</asp:ListItem>
                </asp:DropDownList></td>
            <td>
                Rows</td>
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
                  <asp:ListItem Value="TransNmbr" Selected="True">Reference</asp:ListItem>
                      <asp:ListItem Value="dbo.FormatDate(TransDate)">Date</asp:ListItem>
                      <asp:ListItem>Status</asp:ListItem>
                      <asp:ListItem Value="PONo">PO NO</asp:ListItem>
                      <asp:ListItem Value="FgReport">Report</asp:ListItem>
                      <asp:ListItem Value="FgHome">Home</asp:ListItem>
                      <asp:ListItem Value="ProductCode">Product Code</asp:ListItem>
                      <asp:ListItem Value="ProductName">Product Name</asp:ListItem>
                      <asp:ListItem Value="Supplier">Supplier</asp:ListItem>
                      <asp:ListItem Value="Supplier_Name">Supplier Name</asp:ListItem>                      
                      <asp:ListItem Value="ShipTo">Ship To</asp:ListItem>                      
                      <asp:ListItem Value="ShipToName">Ship To Name</asp:ListItem>                      
                      <asp:ListItem Value="Wrhs_Name">Warehouse</asp:ListItem>
                      <asp:ListItem Value="SJSuppNo">SJ Supp. No</asp:ListItem>
                      <asp:ListItem Value="SJSuppDate">SJ Supp. Date</asp:ListItem>
                      <asp:ListItem Value="CarNo">Car No</asp:ListItem>
                      <asp:ListItem Value="Driver">Driver</asp:ListItem>
                      <asp:ListItem>Remark</asp:ListItem>
              </asp:DropDownList>
          </td>              
        </tr>        
      </table>      
      </asp:Panel>
          <br />          
            <asp:Button class="bitbtn btnadd" runat="server" ID="BtnAdd" Text="Add" />	           
            &nbsp;   
            <asp:DropDownList CssClass="DropDownList" ID="ddlCommand" runat="server" Visible="false"/>
            <asp:Button class="btngo" runat="server" ID="BtnGo" Text="G" Visible="false" />                      
          <br />&nbsp; 
          <div style="border:0px  solid; width:100%; height:100%; overflow:auto;">
              <asp:GridView ID="GridView1" runat="server" AllowPaging="True" 
                  AllowSorting="True" AutoGenerateColumns="False" CssClass="Grid">
                  <HeaderStyle CssClass="GridHeader" Wrap="false"/>
                  <RowStyle CssClass="GridItem" Wrap="false" />
                  <AlternatingRowStyle CssClass="GridAltItem" />
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
                              <asp:DropDownList ID="ddl" runat="server" CssClass="DropDownList">
                                  <asp:ListItem Selected="True" Text="View" />
                                  <asp:ListItem Text="Edit" />
                                  <asp:ListItem Text="Print" />
                              </asp:DropDownList>                              
                              <asp:Button class="btngo" runat="server" ID="BtnGo" Text="G" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" CommandName="Go" />
                          </ItemTemplate>
                          <HeaderStyle Width="110px" />
                      </asp:TemplateField>
                      <asp:BoundField DataField="TransNmbr" HeaderStyle-Width="120px" 
                          HeaderText="Reference" SortExpression="Nmbr">
                          <HeaderStyle Width="120px" />
                      </asp:BoundField>
                      <asp:BoundField DataField="Status" HeaderText="Status" />
                      <asp:BoundField DataField="TransDate" HtmlEncode="true" DataFormatString="{0:dd MMM yyyy}" HeaderStyle-Width="80px" 
                          HeaderText="Date" SortExpression="TransDate">
                          <HeaderStyle Width="80px" />
                      </asp:BoundField>
                      <asp:BoundField DataField="PONo" HeaderText="PO NO" SortExpression="PONo" />
                      <asp:BoundField DataField="Supplier" HeaderText="Supplier" 
                          SortExpression="Supplier" />
                      <asp:BoundField DataField="Supplier_Name" HeaderText="Supplier Name" 
                          SortExpression="Supplier_Name" />                      
                      <asp:BoundField DataField="ShipTo" HeaderText="Ship To" 
                          SortExpression="ShipTo" />
                      <asp:BoundField DataField="ShipToName" HeaderText="ShipTo Name" 
                          SortExpression="ShipToName" />                      
                      <asp:BoundField DataField="Wrhs_Name" HeaderStyle-Width="102px" 
                          HeaderText="Warehouse" SortExpression="Wrhs_Name">
                          <HeaderStyle Width="200px" />
                      </asp:BoundField>
                      <asp:BoundField DataField="Driver" HeaderText="Driver" SortExpression="Driver" />
                      <asp:BoundField DataField="CarNo" HeaderText="Car No" SortExpression="CarNo"  />
                      <%--<asp:BoundField DataField="PPnNo" HeaderText="PPn No" />
                      <asp:BoundField DataField="PPNDate"  HtmlEncode="true" DataFormatString="{0:dd MMM yyyy}" HeaderText="PPN Date" />
                      <asp:BoundField DataField="PPNRate" HeaderText="PPN Rate" DataFormatString="{0:#,##0.00}"
                      ItemStyle-HorizontalAlign="Right" />--%>
                                              
                      <asp:BoundField DataField="Remark" HeaderStyle-Width="200px" 
                          HeaderText="Remark">
                          <HeaderStyle Width="250px" />
                      </asp:BoundField>
                  </Columns>
              </asp:GridView>
          </div>
            <br
            <asp:Panel runat="server" ID ="pnlNav" Visible="false">
            <asp:Button class="bitbtn btnadd" runat="server" ID="btnAdd2" Text="Add" />	 
            &nbsp; 
            <asp:DropDownList ID="ddlCommand2" CssClass="DropDownList" runat="server"/>
            <asp:Button class="btngo" runat="server" ID="btnGo2" Text="G" />  
            </asp:Panel>
    </asp:Panel>    
    <asp:Panel runat="server" ID="pnlInput" Visible="false">
      <table>
        <tr>
            <td>Reference</td>
            <td>:</td>
            <td><asp:TextBox CssClass="TextBoxR" runat="server" ID="tbReference" Width="150px" Enabled="False"/> </td>                    
            <td>Date</td>
            <td>:</td>
            <td>
            <BDP:BasicDatePicker ID="tbDate" runat="server" DateFormat="dd MMM yyyy" 
                        ReadOnly = "true" ValidationGroup="Input"
                        ButtonImageHeight="19px" ButtonImageWidth="20px" 
                        DisplayType="TextBoxAndImage" 
                        TextBoxStyle-CssClass="TextDate" AutoPostBack="True" 
                        ShowNoneButton="False"><TextBoxStyle CssClass="TextDate" /></BDP:BasicDatePicker>                
            </td>            
        </tr> 
        <tr>
         <td>
                  PO No</td>
              <td>
                  :</td>
              <td>
                  <asp:TextBox ID="tbPONo" runat="server" CssClass="TextBox" 
                      Width="150px" Enabled = "false" AutoPostBack="False" />
                  <asp:Button Class="btngo" ID="btnPONo" Text="..." runat="server" ValidationGroup="Input" />                                                                               
                 
                
                  <asp:Label ID="Label3" runat="server" ForeColor="Red">*</asp:Label>
              </td>
              
              <td>
                  Report / Home</td>
              <td>
                  :</td>
              <td>
                   <asp:TextBox ID="tbFgReport" runat="server" CssClass="TextBoxR" enabled="false" />
                     <asp:TextBox ID="tbfgHome" runat="server" CssClass="TextBoxR" enabled="false" />
                  <asp:Label ID="Label9" runat="server" ForeColor="Red">*</asp:Label>
              </td>
        </tr>
        <tr>
            <td>
                <asp:LinkButton ID="lbSupplier" runat="server" Text="Supplier" 
                    ValidationGroup="Input" />
            </td>
            <td>:</td>
            <td colspan="4">
                <asp:TextBox ID="tbSuppCode" runat="server" AutoPostBack="true" MaxLength = "12" 
                    CssClass="TextBox" />
                <asp:TextBox ID="tbSuppName" runat="server" CssClass="TextBoxR" Enabled="false" 
                    EnableTheming="True" Width="223px" />
                <asp:Button Class="btngo" ID="btnSupp" Text="..." runat="server" ValidationGroup="Input" />                                                                       
                <asp:Label ID="Label4" runat="server" ForeColor="Red">*</asp:Label>
            </td> 
          </tr>          
              
         <tr>
            <td>
                <asp:LinkButton ID="lbShip" runat="server" Text="Ship To" 
                    ValidationGroup="Input" />
            </td>
            <td>:</td>
            <td colspan="4">
                <asp:TextBox ID="tbShip" runat="server" AutoPostBack="true" MaxLength = "12" 
                    CssClass="TextBox" />
                <asp:TextBox ID="tbShipName" runat="server" CssClass="TextBoxR" Enabled="false" 
                    EnableTheming="True" Width="223px" />
                <asp:Button Class="btngo" ID="btnShip" Text="..." runat="server" ValidationGroup="Input" />                                                                       
                <asp:Label ID="Label6" runat="server" ForeColor="Red">*</asp:Label>
            </td> 
          </tr> 
        <tr>
            <td><asp:LinkButton ID="lbWarehouse" ValidationGroup="Input" runat="server" Text="Warehouse Receive"/></td>
            <td>:</td>
            <td><asp:DropDownList CssClass="DropDownList" ValidationGroup="Input" runat="server" ID="ddlwrhs" AutoPostBack="true" Width="200px" />
                <asp:TextBox runat="server" ID="tbFgSubLed" Visible="false"/>                
                <asp:Label ID="Label5" runat="server" ForeColor="Red">*</asp:Label>
            </td>         
           
        </tr>  
        <tr>
            <td>SJ Supplier No</td>
            <td>:</td>
            <td>
                <asp:TextBox ID="tbSJSuppNo" runat="server" CssClass="TextBox" MaxLength = "30" 
                    ValidationGroup="Input" Width="200px" />
                    <asp:Label ID="lbred2" runat="server" ForeColor="Red">*</asp:Label>
            </td> 
              <td>
                  SJ Supplier Date</td>
              <td>
                  :</td>
              <td>
                  <BDP:BasicDatePicker ID="tbSJSuppDate" runat="server" AutoPostBack="True" 
                      ButtonImageHeight="19px" ButtonImageWidth="20px" DateFormat="dd MMM yyyy" 
                      DisplayType="TextBoxAndImage" ReadOnly="true" ShowNoneButton="False" 
                      TextBoxStyle-CssClass="TextDate" ValidationGroup="Input"><TextBoxStyle CssClass="TextDate" /></BDP:BasicDatePicker>
              </td>
          </tr>
          <tr>
              <td>
                  Driver</td>
              <td>
                  :</td>
              <td>
                  <asp:TextBox ID="tbDriver" runat="server" CssClass="TextBox" MaxLength = "60" 
                      ValidationGroup="Input" Width="200px" />
              </td>
              <td>
                  Car No</td>
              <td>
                  :</td>
              <td>
                  <asp:TextBox ID="tbCarNo" runat="server" CssClass="TextBox" MaxLength = "60" 
                      ValidationGroup="Input" Width="200px" />
                      
                      <asp:Button ID="btnGetDt" runat="server" Class="bitbtndt btnsearch" 
                      Text="Get Data PO" ValidationGroup="Input"  />
              </td>
              
          </tr>
          
          <tr>
                <td>
                         <%--   PPn
                        </td>
                        <td>
                            :--%>
                        </td>
                        <td colspan="2">
                            <table>
                                <tr style="background-color: Silver; text-align: center">
                                    <%--<td>
                                        PPn No
                                    </td>
                                    <td>
                                        PPn Date
                                    </td>
                                    <td>
                                        PPn Rate
                                    </td>--%>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="tbPPnNo" Visible ="false"  runat="server" Enabled="False" CssClass="TextBox" ValidationGroup="Input"
                                            Width="150px" />
                                    </td>
                                    <td>
                                        <BInv:BasicDatePicker ID="tbPPnDate" Visible ="false"  runat="server" DateFormat="dd MMM yyyy" ShowNoneButton="false"
                                            ValidationGroup="Input" Enabled="False" ButtonImageHeight="19px" ButtonImageWidth="20px"
                                            DisplayType="TextBoxAndImage" TextBoxStyle-CssClass="TextDate" AutoPostBack="True">
                                        </BInv:BasicDatePicker>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="tbPpnRate" runat="server"  Visible ="false"  Enabled="False" CssClass="TextBox" ValidationGroup="Input"
                                            Width="68px" />
                                             
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
          

        <tr>
            <td>Remark</td>
            <td>:</td>
            <td colspan="4"><asp:TextBox runat="server" ValidationGroup="Input" ID="tbRemark" CssClass="TextBoxMulti" MaxLength="255" TextMode="MultiLine" Width="350px"/></td>
        </tr>
      </table>  
      
      <br />      
      <div style="font-size:medium; color:Blue;">Detail</div>
      <hr style="color:Blue" />  
        <asp:Panel runat="server" ID="pnlDt">
            <asp:Button class="bitbtn btnadd" runat="server" ID="btnAddDt" Text="Add" ValidationGroup="Input" />	                 
            <div style="border:0px  solid; width:100%; height:100%; overflow:auto;">
                <asp:GridView ID="GridDt" runat="server" AutoGenerateColumns="False" 
                    ShowFooter="False">
                    <HeaderStyle CssClass="GridHeader" />
                    <RowStyle CssClass="GridItem" Wrap="false" />
                    <AlternatingRowStyle CssClass="GridAltItem" />
                    <PagerStyle CssClass="GridPager" />
                    <Columns>
                        <asp:TemplateField HeaderText="Action">
                            <ItemTemplate>
   							    <asp:Button ID="btnEdit" runat="server" class="bitbtndt btnedit" Text="Edit" CommandName="Edit"/>
								<asp:Button ID="btnDelete" runat="server" class="bitbtndt btndelete" Text="Delete" OnClientClick="return confirm('Sure to delete this data?');" CommandName="Delete"/>									                            
                            </ItemTemplate>
                            <EditItemTemplate>
                               	<asp:Button ID="btnUpdate" runat="server" class="bitbtndt btnsave" Text="Save" CommandName="Update"/>									
                                <asp:Button ID="btnCancel" runat="server" class="bitbtndt btncancel" Text="Cancel" CommandName="Cancel"/>									
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="ProductCode" HeaderStyle-Width="120px" 
                            HeaderText="Product Code" >
                            <HeaderStyle Width="120px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Product_Name" HeaderStyle-Width="250px"  
                            HeaderText="Product Name" >
                            <HeaderStyle Width="250px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Specification" HeaderText="Specification" HeaderStyle-Width="150px"  
                            SortExpression="Specification" >
                            <HeaderStyle Width="150px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="ProductPart" HeaderStyle-Width="120px" 
                            HeaderText="Product Part" >
                            <HeaderStyle Width="120px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="ProductPart_Name" HeaderStyle-Width="250px"  
                            HeaderText="Product Part Name" >
                            <HeaderStyle Width="250px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="QtyPO" DataFormatString="{0:#,##0.##}" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="80px" 
                            HeaderText="Qty PO" 
                            SortExpression="QtyPO">
                            <HeaderStyle Width="80px" />
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:BoundField>
                        
                        <asp:BoundField DataField="Qty" DataFormatString="{0:#,##0.##}" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="80px" 
                            HeaderText="Qty RR" 
                            SortExpression="Qty">
                            <HeaderStyle Width="80px" />
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Unit" HeaderStyle-Width="80px" HeaderText="Unit Order" 
                            SortExpression="Unit" >
                            <HeaderStyle Width="80px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="HaveQC" HeaderStyle-Width="60px" HeaderText="HaveQC" >
                            <HeaderStyle Width="60px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="JmlPacking" DataFormatString="{0:#,##0.##}" 
                            ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="80px"  HeaderText="@" >
                            <HeaderStyle Width="80px" />
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:BoundField>
                        <asp:BoundField DataField="QtyPacking" DataFormatString="{0:#,##0.##}" 
                            ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="80px"  HeaderText="Qty Packing" >
                            <HeaderStyle Width="80px" />
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:BoundField>
                        <asp:BoundField DataField="UnitPacking" HeaderStyle-Width="60px" HeaderText="Unit Packing" >
                            <HeaderStyle Width="60px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="QtySisa" DataFormatString="{0:#,##0.##}" 
                            ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="80px"  HeaderText="Qty Sisa" >
                            <HeaderStyle Width="80px" />
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:BoundField>
                        <asp:BoundField DataField="UnitPacking" HeaderStyle-Width="60px" HeaderText="Unit Sisa" >
                            <HeaderStyle Width="60px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Remark" HeaderStyle-Width="120px" 
                            HeaderText="Remark" >
                            <HeaderStyle Width="120px" />
                        </asp:BoundField>
                    </Columns>
                </asp:GridView>
          </div>   
          <asp:Button class="bitbtn btnadd" runat="server" ID="btnAddDt2" Text="Add" ValidationGroup="Input" />	                 
       </asp:Panel>             
       <asp:Panel runat="server" ID="pnlEditDt" Visible="false">
            <table>                
                <tr>
                    <td><asp:LinkButton ID="lbProduct"  runat="server" Text="Product"/> </td>
                    <td>:</td>
                    <td><asp:TextBox runat="server" ID="tbProductCode" CssClass="TextBox" AutoPostBack="true" />
                        <asp:TextBox runat="server"  CssClass="TextBoxR"
                            ID="tbProductName" EnableTheming="True" Enabled="false" Width="200px"/> 
                        <asp:Button Class="btngo" ID="btnProduct" Text="..." runat="server" ValidationGroup="Input" />                                                       
                        <asp:Label ID="Label7" runat="server" ForeColor="Red">*</asp:Label>
                    </td>                               
                </tr>
                <tr>
                    <td>Specification</td>
                    <td>:</td>
                    <td>
                        <asp:TextBox ID="tbSpecification" runat="server" CssClass="TextBoxMultiR" 
                            EnableTheming="True" Width="350px" TextMode="MultiLine" />
                    </td>
                </tr>   
                <tr>
                    <td><asp:LinkButton ID="lbProductPart"  runat="server" Text="Product Part"/> </td>
                    <td>:</td>
                    <td><asp:TextBox runat="server" ID="tbProductPart" CssClass="TextBox" AutoPostBack="true" />
                        <asp:TextBox runat="server"  CssClass="TextBoxR"
                            ID="tbProductPartName" EnableTheming="True" Enabled="false" Width="200px"/> 
                        <asp:Button Class="btngo" ID="btnProductPart" Text="..." runat="server" ValidationGroup="Input" />                                                       
                        <asp:Label ID="Label10" runat="server" ForeColor="Red">*</asp:Label>
                    </td>                               
                </tr>   
                <tr>
                    <td>
                        Qty </td>
                    <td>
                        :</td>
                    <td>
                        <table cellpadding="0" cellspacing="1">
                            <tr style="background-color:Silver;text-align:center">
                                <td>
                                    PO
                                    <asp:Label ID="Label8" runat="server" ForeColor="Red">*</asp:Label>
                                </td>
                                <td>
                                    RR</td>
                                    <td>
                                    Have QC</td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:TextBox ID="tbQtyPO" runat="server" 
                                        CssClass="TextBox" Width="80px" />
                                </td>
                                <td>
                                    <asp:TextBox ID="tbQty" runat="server" CssClass="TextBox" 
                                        Width="80px" AutoPostBack="true"  />
                                        <asp:TextBox ID="tbUnit" runat="server" CssClass="TextBoxR" 
                                        Enabled="false" Width="50px" />
                                    
                               
                                </td>
                                <td>
                                   
                                   
                                    <asp:DropDownList ID="ddlHaveQC" runat="server" CssClass="DropDownList" 
                                        Enabled="False">
                                        <asp:ListItem Value="Y"></asp:ListItem>
                                        <asp:ListItem Value="N"></asp:ListItem>
                                    </asp:DropDownList>
                                   
                                   
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                        <td>Packing</td>
                        <td>:</td>
                        <td>
                            <table cellspacing="0" cellpadding="0">
                                <tr style="background-color:Silver;text-align:center">
                                    <td>@</td>
                                    <td>
                                        Qty</td>
                                    <td>
                                        Unit</td>
                                    <td>Sisa </td>
                                    <td>
                                        Unit Sisa
                                    </td>
                                    
                                </tr>
                                <tr>
                                    <td><asp:TextBox CssClass="TextBoxR" runat="server" ID="tbJmlPacking" Width="80px" 
                                            Enabled="False" /></td>
                                    <td>
                                   
                                        <asp:TextBox ID="tbQtyPacking" runat="server" CssClass="TextBox" Width="80px" 
                                            AutoPostBack="true" />
                                    </td>
                                    <td>
                                       <asp:DropDownList CssClass="DropDownList" ID="ddlUnitPacking" runat="server" 
                                          Enabled="False"/> 
                                    </td>
                                    <td><asp:TextBox CssClass="TextBox" runat="server" ID="tbQtySisa" Width="80px" 
                                            Enabled="False"  AutoPostBack="True"/></td>
                                    <td>
                                        <asp:DropDownList CssClass="DropDownList" ID="ddlUnitSisa" runat="server" 
                                          Enabled="False"/> 
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
                        :</td>
                    <td>
                        <asp:TextBox ID="tbRemarkDt" runat="server" CssClass="TextBoxMulti" 
                            MaxLength="255" TextMode="MultiLine" Width="350px" />
                    </td>
                </tr>
            </table>
            <br />                     
            <asp:Button ID="btnSaveDt" runat="server" class="bitbtndt btnsave" Text="Save" />									
            <asp:Button ID="btnCancelDt" runat="server" class="bitbtndt btncancel" Text="Cancel" />	
       </asp:Panel> 
       <br />          
    	<asp:Button ID="btnSaveAll" runat="server" class="bitbtndt btnsavenew" Text="Save & New" validationgroup="Input" Width = "90"/>									
        <asp:Button ID="btnSaveTrans" runat="server" class="bitbtndt btnsave" Text="Save" validationgroup="Input"/>									
        <asp:Button ID="btnBack" runat="server" class="bitbtndt btncancel" Text="Cancel" validationgroup="Input"/>									
        <asp:Button ID="btnHome" runat="server" class="bitbtndt btnback" Text="Home" />	  
    </asp:Panel>
    <asp:Panel runat="server" ID="pnlPrint" Visible="false">
    <cr:crystalreportviewer ID="CrystalReportViewer1" runat="server" AutoDataBind="True"
               Height="1036px" Width="928px" />
<br />
        <div>
            <cc1:WebReport ID="WebReport1" runat="server" AutoHeight="True" 
                AutoWidth="True" Height="100%" Width="100%" />
        </div>
    </asp:Panel>
      <br />            
    </div>
   
    <asp:Label runat ="server" ID="lbStatus" ForeColor="Red"/>
    <asp:HiddenField ID="HiddenRemarkDelete" runat="server" />
            <div class="loading" align="center">
      <br />
       <img src="../../Image/loader.gif" alt="" />
    </div>
    </form>
    </body>
</html>
