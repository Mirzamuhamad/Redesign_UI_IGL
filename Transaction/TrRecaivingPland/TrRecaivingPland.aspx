<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TrRecaivingPland.aspx.vb" Inherits="TrRecaivingPland" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<%@ Register Assembly="BasicFrame.WebControls.BasicDatePicker" Namespace="BasicFrame.WebControls"
    TagPrefix="BDP" %>

<%@ Register assembly="CrystalDecisions.Web, Version=10.5.3700.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>

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
    
        function setformat()
        {
        try
         {          
        var Rate = document.getElementById("tbRate").value.replace(/\$|\,/g,"");
        var PPn = document.getElementById("tbPPN").value.replace(/\$|\,/g,"");        
        var BaseForex = document.getElementById("tbBaseForex").value.replace(/\$|\,/g,"");
        var PPnForex = document.getElementById("tbPPNForex").value.replace(/\$|\,/g,"");
        var TotalForex = document.getElementById("tbTotalForex").value.replace(/\$|\,/g,"");
        
        document.getElementById("tbRate").value = setdigit(Rate,'<%=VIEWSTATE("DigitRate")%>');
        document.getElementById("tbPPN").value = setdigit(PPn,'<%=VIEWSTATE("DigitPercent")%>');
        document.getElementById("tbBaseForex").value = setdigit(BaseForex,'<%=VIEWSTATE("DigitCurr")%>');
        document.getElementById("tbPPNForex").value = setdigit(PPnForex,'<%=VIEWSTATE("DigitCurr")%>');
        document.getElementById("tbTotalForex").value = setdigit(TotalForex,'<%=VIEWSTATE("DigitCurr")%>');
        }catch (err){
            alert(err.description);
          }      
        }   
        
        function setformatdt()
        {
        try
         {         
        var Qty = document.getElementById("tbQty").value.replace(/\$|\,/g,""); 
        var QtyWrhs = document.getElementById("tbQtyWrhs").value.replace(/\$|\,/g,"");         
        document.getElementById("tbQty").value = setdigit(Qty,'<%=VIEWSTATE("DigitQty")%>');
        document.getElementById("tbQtyWrhs").value = setdigit(QtyWrhs,'<%=VIEWSTATE("DigitQty")%>');        
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
    <div class="H1">Receiving Plan</div>
     <hr style="color:Blue" />        
    <asp:Panel runat="server" id="PnlHd"> 
      <table>
        <tr>
            <td style="width:100px;text-align:right">Quick Search :</td>            
            <td><asp:TextBox CssClass="TextBox" runat="server" ID ="tbFilter"/> 
                      <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlField" >
                      <asp:ListItem Value="TransNmbr" Selected="True">Reference</asp:ListItem>
                      <asp:ListItem Value="Revisi">Revisi</asp:ListItem>
                      <asp:ListItem Value="Attn">Attn</asp:ListItem>
                      <asp:ListItem Value="StartRR">Start RR</asp:ListItem>
                      <asp:ListItem Value="EndRR">End RR</asp:ListItem>

                      <asp:ListItem>Remark</asp:ListItem>
                  </asp:DropDownList>
                  <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlRange"></asp:DropDownList>
                  
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
                  <asp:ListItem Value="Revisi">Revisi</asp:ListItem>
                  <asp:ListItem Value="Attn">Attn</asp:ListItem>
                  <asp:ListItem Value="StartRR">Start RR</asp:ListItem>
                  <asp:ListItem Value="EndRR">End RR</asp:ListItem>

                  <asp:ListItem>Remark</asp:ListItem>                                    
              </asp:DropDownList>
          </td>              
        </tr>        
      </table>      
      </asp:Panel>
          <br />
            &nbsp &nbsp &nbsp   
            <asp:DropDownList CssClass="DropDownList" ID="ddlCommand" runat="server" Visible="false"/>
            <asp:Button class="btngo" runat="server" ID="BtnGo" Text="G" Visible="false" />                                
        <asp:GridView ID="GridView1" runat="server" AllowPaging="True" 
            AllowSorting="True" AutoGenerateColumns="False" CssClass="Grid">
            <HeaderStyle CssClass="GridHeader" />
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
                            <asp:ListItem Selected="True" Text="Receving Plan" />
                        </asp:DropDownList>
                        <asp:Button ID="btnGo" runat="server" class="bitbtn btngo" 
                            CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" CommandName="Go" 
                            Text="G" />
                    </ItemTemplate>
                    <HeaderStyle Width="110px" />
                </asp:TemplateField>
                <asp:BoundField DataField="TransNmbr" HeaderText="PO No" 
                    SortExpression="Nmbr" />
                <asp:BoundField DataField="Revisi" HeaderText="Revisi" />
                <asp:BoundField DataField="Status" HeaderText="Status" />
                <asp:BoundField DataField="TransDate" DataFormatString="{0:dd MMM yyyy}" 
                    HeaderText="Date" HtmlEncode="true" SortExpression="TransDate" />
                <%--<asp:BoundField DataField="FgReport" sortExpression="FgReport" HeaderText="Report"></asp:BoundField>                  --%>
                <asp:BoundField DataField="POType" HeaderStyle-HorizontalAlign="Left" 
                    HeaderText="PO Type" SortExpression="POType">
                <HeaderStyle HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:BoundField DataField="DepartmentName" HeaderText="Department" 
                    SortExpression="DepartmentName" />
                <asp:BoundField DataField="Supplier" HeaderText="Supplier" 
                    SortExpression="Supplier" />
                <asp:BoundField DataField="SupplierName" HeaderText="Supplier Name" 
                    SortExpression="SupplierName" />
                <asp:BoundField DataField="Attn" HeaderText="Attn" SortExpression="Attn" />
                <asp:BoundField DataField="SuppContractNo" HeaderText="No Penawaran" 
                    SortExpression="SuppContractNo" />
                <asp:BoundField DataField="Term" HeaderText="Term" SortExpression="Term" />
                <asp:BoundField DataField="TermPayment" HeaderText="Term Payment" 
                    SortExpression="TermPayment" />
                <asp:BoundField DataField="Delivery" HeaderText="Delivery" 
                    SortExpression="Delivery" />
                <asp:BoundField DataField="ShipmentType" HeaderStyle-HorizontalAlign="Left" 
                    HeaderStyle-Width="60px" HeaderText="Shipment Type" 
                    SortExpression="ShipmentType">
                <HeaderStyle HorizontalAlign="Left" Width="60px" />
                </asp:BoundField>
                <asp:BoundField DataField="FgPriceIncludeTax" 
                    HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="60px" 
                    HeaderText="Price Include PPN" SortExpression="FgPriceIncludeTax">
                <HeaderStyle HorizontalAlign="Left" Width="60px" />
                </asp:BoundField>
                <asp:BoundField DataField="Currency" HeaderText="Currency" 
                    SortExpression="Currency" />
                <asp:BoundField DataField="BaseForex" DataFormatString="{0:#,##0.00}" 
                    HeaderText="Base Forex" ItemStyle-HorizontalAlign="Right" 
                    SortExpression="BaseForex">
                <ItemStyle HorizontalAlign="Right" />
                </asp:BoundField>
                <asp:BoundField DataField="DiscForex" DataFormatString="{0:#,##0.00}" 
                    HeaderText="Disc Forex" ItemStyle-HorizontalAlign="Right" 
                    SortExpression="DiscForex">
                <ItemStyle HorizontalAlign="Right" />
                </asp:BoundField>
                <asp:BoundField DataField="DP" DataFormatString="{0:#,##0.####}" 
                    HeaderText="DP" ItemStyle-HorizontalAlign="Right" SortExpression="DP">
                <ItemStyle HorizontalAlign="Right" />
                </asp:BoundField>
                <asp:BoundField DataField="DPForex" DataFormatString="{0:#,##0.00}" 
                    HeaderText="DP Forex" ItemStyle-HorizontalAlign="Right" 
                    SortExpression="DPForex">
                <ItemStyle HorizontalAlign="Right" />
                </asp:BoundField>
                <asp:BoundField DataField="PPn" DataFormatString="{0:#,##0.####}" 
                    HeaderText="PPN" ItemStyle-HorizontalAlign="Right" SortExpression="PPn">
                <ItemStyle HorizontalAlign="Right" />
                </asp:BoundField>
                <asp:BoundField DataField="PPNForex" DataFormatString="{0:#,##0.00}" 
                    HeaderText="PPN Forex" ItemStyle-HorizontalAlign="Right" 
                    SortExpression="PPNForex">
                <ItemStyle HorizontalAlign="Right" />
                </asp:BoundField>
                <asp:BoundField DataField="PPHForex" DataFormatString="{0:#,##0.00}" 
                    HeaderText="PPH Forex" ItemStyle-HorizontalAlign="Right" 
                    SortExpression="PPHForex">
                <ItemStyle HorizontalAlign="Right" />
                </asp:BoundField>
                <asp:BoundField DataField="OtherForex" DataFormatString="{0:#,##0.00}" 
                    HeaderText="Other Forex" ItemStyle-HorizontalAlign="Right" 
                    SortExpression="OtherForex">
                <ItemStyle HorizontalAlign="Right" />
                </asp:BoundField>
                <asp:BoundField DataField="TotalForex" DataFormatString="{0:#,##0.00}" 
                    HeaderText="Total Forex" ItemStyle-HorizontalAlign="Right" 
                    SortExpression="TotalForex">
                <ItemStyle HorizontalAlign="Right" />
                </asp:BoundField>
                <asp:BoundField DataField="Remark" HeaderText="Remark" 
                    sortExpression="Remark" />
            </Columns>
        </asp:GridView>
        <br />
          <div style="border:0px  solid; width:100%; height:100%; overflow:auto;">
          
          
          </div>
            <asp:Panel runat="server" ID ="pnlNav" Visible="false">
            &nbsp &nbsp &nbsp
            <asp:DropDownList ID="ddlCommand2" CssClass="DropDownList" runat="server"/>
            <asp:Button class="btngo" runat="server" ID="btnGo2" Text="G" />  
            </asp:Panel>
    </asp:Panel>   
    <asp:Panel runat="server" ID="pnlDelivery">
            <asp:Button class="bitbtn btnadd" runat="server" ID="BtnAdd" Text="Add" />	                  
            <div style="border:0px  solid; width:100%; height:100%; overflow:auto;">
                <asp:GridView ID="GridView2" runat="server" AutoGenerateColumns="false" 
                    ShowFooter="True">
                    <HeaderStyle CssClass="GridHeader" />
                    <RowStyle CssClass="GridItem" Wrap="false" />
                    <AlternatingRowStyle CssClass="GridAltItem" />
                    <PagerStyle CssClass="GridPager" />
                    <Columns>
                        <asp:TemplateField HeaderText="Action">
                            <ItemTemplate>
                                <asp:Button ID="btnEdit" runat="server" class="bitbtndt btnedit" Text="Edit" CommandName="Edit"/>
								<asp:Button ID="btnDelete" runat="server" class="bitbtndt btndelete" Text="Delete" OnClientClick="return confirm('Sure to delete this data?');" CommandName="Delete"/>									   
								<asp:Button ID="btnDetail" runat="server" class="bitbtndt btndetail" Text="Detail" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>"  CommandName="Detail"/>									   
								
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:Button ID="btnUpdate" runat="server" class="bitbtndt btnsave" Text="Save" CommandName="Update"/>									
                                <asp:Button ID="btnCancel" runat="server" class="bitbtndt btncancel" Text="Cancel" CommandName="Cancel"/>									
                            </EditItemTemplate>                            
                        </asp:TemplateField>
                  <asp:BoundField DataField="FgHome" HeaderText="Home"></asp:BoundField>                  
                  <asp:BoundField DataField="Shipto" SortExpression="Shipto" HeaderText="Ship to" 
                      HeaderStyle-HorizontalAlign="Left" >
                      <HeaderStyle HorizontalAlign="Left" />
                  </asp:BoundField>
                  <asp:BoundField DataField="ShiptoName" SortExpression="ShiptoName" HeaderText="Ship To" />
                  <asp:BoundField DataField="Attn" SortExpression="Attn" HeaderText="Attn" />
                  <asp:BoundField DataField="Address1" SortExpression="Address1" 
                      HeaderText="Address 1" />
                  <asp:BoundField DataField="Address2" SortExpression="Address2" HeaderText="Address2" />
                  <asp:BoundField DataField="StartRR" HtmlEncode="true" DataFormatString="{0:dd MMM yyyy}" SortExpression="StartRR" HeaderText="Start RR"></asp:BoundField>
                  <asp:BoundField DataField="EndRR" HtmlEncode="true" DataFormatString="{0:dd MMM yyyy}" SortExpression="EndRR" HeaderText="End RR"></asp:BoundField>
                  <asp:BoundField DataField="WrhsCode" SortExpression="WrhsCode" HeaderText="Wrhs Code" />
                  <asp:BoundField DataField="WrhsName" SortExpression="WrhsName" HeaderText="Wrhs Name" />
                  <asp:BoundField DataField="Remark" sortExpression="Remark" HeaderText="Remark"></asp:BoundField>
                    </Columns>
                </asp:GridView>
          </div>   
          <asp:Button class="bitbtn btnadd" runat="server" ID="btnAdd2" Text="Add" />	  
       </asp:Panel> 
    <asp:Panel runat="server" ID="pnlInput" Visible="false">
        &nbsp;<asp:Panel runat="server" ID="pnlinputdel" Visible="false">
      <table>
        <tr>
            <td>Po No</td>
            <td>:</td>
            <td><asp:TextBox CssClass="TextBoxR" runat="server" ID="tbRef" Width="150px" Enabled="False"/>             
            <asp:TextBox CssClass="TextBoxR" runat="server" ID="tbrev" Width="50px" Enabled="False"/>             
            </td>                    
            <td></td>
        </tr> 
        <tr>
            <td>Home</td>
            <td>:</td>
            <td>
                <asp:DropDownList ID="ddlHome" runat="server" AutoPostBack="true" 
                    CssClass="DropDownList" Enabled="true" ValidationGroup="Input" Width="40px">
                    <asp:ListItem>Y</asp:ListItem>
                    <asp:ListItem>N</asp:ListItem>
                </asp:DropDownList>
              
            </td>            
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td>
                <%--'<a href="javascript:__doPostBack('lbSupp','')">Ship TO</a></td>--%>
                <a >Ship TO</a></td>
            <td>:</td>
            <td><asp:TextBox runat="server" ID="tbSuppCode" 
                    CssClass="TextBox" AutoPostBack="true"/>
                <asp:TextBox ID="tbSuppName" runat="server" CssClass="TextBox" Enabled="false" 
                    Width="225px" />
                <asp:Button ID="btnSupp" runat="server" Class="btngo" Text="..." 
                    ValidationGroup="Input" />
            </td>            
            <td>
                &nbsp;</td>
        </tr>        
          <tr>
              <td>
                  Attn</td>
              <td>
                  :</td>
              <td>
                  <asp:TextBox ID="tbAttn" runat="server" CssClass="TextBox" MaxLength="50" 
                      ValidationGroup="Input" Width="250px" />
                      
                      
              </td>
              <td>
                  &nbsp;</td>
          </tr>
          <tr>
              <td>
                  Address 1</td>
              <td>
                  :</td>
              <td>
                  <asp:TextBox ID="tbAddress1" runat="server" CssClass="TextBox" MaxLength="50" 
                      ValidationGroup="Input" Width="250px" />
              </td>
              <td>
                  &nbsp;</td>
          </tr>
        <tr>
            <td>Address 2</td>
            <td>:</td>
            <td>
                <asp:TextBox ID="tbAddress2" runat="server" CssClass="TextBox" MaxLength="50" 
                    ValidationGroup="Input" Width="250px" />
            </td>        
            <td>
                &nbsp;</td>
        </tr>   
        <tr>
                <td>Start RR</td>
                <td>:</td>
                <td>
                    <BDP:BasicDatePicker ID="tbStartDate" runat="server" AutoPostBack="False" 
                        ButtonImageHeight="19px" ButtonImageWidth="20px" DateFormat="dd MMM yyyy" 
                        DisplayType="TextBoxAndImage" ReadOnly="true" ShowNoneButton="False" 
                        TextBoxStyle-CssClass="TextDate" ValidationGroup="Input">
                        <TextBoxStyle CssClass="TextDate" />
                    </BDP:BasicDatePicker>
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; End RR :&nbsp;
                    <BDP:BasicDatePicker ID="tbEndDate" runat="server" AutoPostBack="True" 
                        ButtonImageHeight="19px" ButtonImageWidth="20px" DateFormat="dd MMM yyyy" 
                        DisplayType="TextBoxAndImage" ReadOnly="true" ShowNoneButton="False" 
                        TextBoxStyle-CssClass="TextDate" ValidationGroup="Input">
                        <TextBoxStyle CssClass="TextDate" />
                    </BDP:BasicDatePicker>
                </td>                
                <td>
                    &nbsp;</td>
         </tr>         
        
        <tr> 
            <td>Warehouse</td>
            <td>:</td>
            <td>
                <asp:DropDownList ID="ddlWarehouse" runat="server" CssClass="DropDownList" 
                    ValidationGroup="Input" Width="250px" />
            </td>
            <td>
                &nbsp;</td>
        </tr>
          <tr>
              <td>
                  Remark</td>
              <td>
                  :</td>
              <td>
                  <asp:TextBox ID="tbRemark" runat="server" CssClass="TextBoxMulti" 
                      MaxLength="255" TextMode="MultiLine" ValidationGroup="Input" Width="350px" />
              </td>
              <td>
                  &nbsp;</td>
          </tr>
          <tr>
              <td>
                  &nbsp;</td>
              <td>
                  &nbsp;</td>
              <td>
                  <asp:Button ID="btnSaveDelivery" runat="server" class="bitbtndt btnsave" 
                      Text="Save" />
                  <asp:Button ID="btnCancelDelivery" runat="server" class="bitbtndt btncancel" 
                      Text="Cancel" />
              </td>
              <td>
                  &nbsp;</td>
          </tr>
      </table>  
      </asp:Panel> 
      <br />      
     
        <asp:Panel runat="server" ID="pnlDt" Visible =false >
         <div style="font-size:medium; color:Blue;">Detail<br />
             <table> <tr><td>Home</td><td>:</td><td><asp:Label ID="tbFgHome" runat="server"></asp:Label></td><td><asp:Button ID="btnGetDt" runat="server" Class="btngo" 
                 Text="Get ALL Data From PO" ValidationGroup="Input" Width="185px" /></td></tr>
             <tr><td>Ship To</td><td>:</td><td> <asp:Label ID="lblShipto" runat="server">
             </asp:Label></td><td><asp:Label ID="lblShiptoName" runat="server"></asp:Label></td></tr>
             </table>
               
                          
             
             
            </div>
      <hr style="color:Blue" />  
            <asp:Button class="bitbtn btnadd" runat="server" ID="btnAddDt" Text="Add" ValidationGroup="Input" />	                  
            <div style="border:0px  solid; width:100%; height:100%; overflow:auto;">
                <asp:GridView ID="GridDt" runat="server" AutoGenerateColumns="false" 
                    ShowFooter="True">
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
                        <%--<asp:BoundField DataField="No" HeaderStyle-Width="80px" HeaderText="No" />--%>
                        <asp:BoundField DataField="ProductCode" HeaderStyle-Width="80px" HeaderText="Product Code" />
                        <asp:BoundField DataField="Product_Name" HeaderStyle-Width="250px"  HeaderText="Product Name" />
                        
                        <%--<asp:BoundField DataField="FgHome" HeaderStyle-Width="250px"  HeaderText="FgHome" />--%>
                        <asp:BoundField DataField="JmlPacking" DataFormatString="{0:#,##0}" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="100px" HeaderText="@" />                   
                        <asp:BoundField DataField="Qty" DataFormatString="{0:#,##0.00}" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="100px" HeaderText="Qty" />                      
                        <asp:BoundField DataField="Unit" HeaderStyle-Width="80px" HeaderText="Unit" /> 
                        <asp:BoundField DataField="QtyPacking" DataFormatString="{0:#,##0.00}" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="100px" HeaderText="Qty Packing" />                   
                        <asp:BoundField DataField="UnitPacking" HeaderStyle-Width="80px" HeaderText="Unit Packing" />
                        <asp:BoundField DataField="StartDelivery" HeaderStyle-Width="80px"  dataformatstring="{0:dd MMM yyyy}"  HeaderText="Start RR" />
                        <asp:BoundField DataField="EndDelivery" HeaderStyle-Width="80px"  dataformatstring="{0:dd MMM yyyy}"  HeaderText="End RR" />
                        <asp:BoundField DataField="Remark" HeaderStyle-Width="80px" HeaderText="Remark" />
                    </Columns>
                </asp:GridView>
          </div>   
          <asp:Button class="bitbtn btnadd" runat="server" ID="btnAddDt2" Text="Add" ValidationGroup="Input" />	 
           <asp:Button class="bitbtn btnback" runat="server" ID="btnBackDt2" Text="Back" ValidationGroup="Input" />	 
       </asp:Panel>             
       <asp:Panel runat="server" ID="pnlEditDt" Visible="false">
            <table>                
                <%--<tr>
                    <td>No</td>
                    <td>:</td>
                    <td><asp:TextBox runat="server" ID="tbProductCode" CssClass="TextBox" 
                            AutoPostBack="true" Height="16px" Width="54px" />
                        
                    </td>                               
                </tr>--%>                   
                <tr>
                    <td>
                        Product </td>
                    <td>
                        :</td>
                    <td>
                        <asp:TextBox ID="tbProductCode" runat="server" AutoPostBack="true" 
                            CssClass="TextBox" Width="136px" />
                        <asp:TextBox ID="tbProductName" runat="server" CssClass="TextBoxR" 
                            Enabled="False" Width="250px" />
                        <asp:Button ID="btnProduct" runat="server" Class="btngo" Text="..." 
                            ValidationGroup="Input" />
                    </td>
                </tr>
                <tr>
                    <td>
                                Packing</td>
                            <td>
                                :</td>
                            <td>
                                <table>
                                    <tr style="background-color: Silver; text-align: center">
                                        <td>
                                            @</td>
                                        <td>
                                            Qty</td>
                                        <td>
                                            Unit</td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="tbjmlpack" runat="server" CssClass="TextBox" Enabled="True" 
                                                Width="62px" AutoPostBack="True" />
                                        </td>
                                        <td>
                                            <asp:TextBox ID="tbQtyPacking" runat="server" CssClass="TextBox" Enabled="True" 
                                                Width="57px" AutoPostBack="True" />
                                        </td>
                                        <td>
                                            <asp:TextBox ID="tbUnitPacking" runat="server" CssClass="TextBoxR" Enabled="False" 
                                                Width="57px" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                </tr>
                <tr>
                    <td>Qty</td>
                    <td>:</td>
                    <td><asp:TextBox CssClass="TextBox" runat="server" ID="tbQty" 
                            Width="65px" AutoPostBack="True" />
                        <asp:TextBox ID="tbUnit" runat="server" CssClass="TextBoxR" Width="57px" 
                            Enabled="False" />
                    </td>
                </tr>
                <tr>
                    <td>Start RR</td>
                    <td>:</td>
                    <td>
                        <BDP:BasicDatePicker ID="tbStartDel" runat="server" AutoPostBack="True" 
                            ButtonImageHeight="19px" ButtonImageWidth="20px" DateFormat="dd MMM yyyy" 
                            DisplayType="TextBoxAndImage" ReadOnly="true" ShowNoneButton="False" 
                            TextBoxStyle-CssClass="TextDate" ValidationGroup="Input">
                            <TextBoxStyle CssClass="TextDate" />
                        </BDP:BasicDatePicker>
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; End RR :&nbsp;
                        <BDP:BasicDatePicker ID="tbEndDel" runat="server" AutoPostBack="True" 
                            ButtonImageHeight="19px" ButtonImageWidth="20px" DateFormat="dd MMM yyyy" 
                            DisplayType="TextBoxAndImage" ReadOnly="true" ShowNoneButton="False" 
                            TextBoxStyle-CssClass="TextDate" ValidationGroup="Input">
                            <TextBoxStyle CssClass="TextDate" />
                        </BDP:BasicDatePicker>
                    </td>
                </tr>  
                <tr>
                    <td>Remark </td>
                    <td>:</td>
                    <td><asp:TextBox runat="server" MaxLength="255" CssClass="TextBoxMulti" TextMode="MultiLine" ID="tbRemarkDt" Width = "355" /></td>
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
    </asp:Panel>
      <br />            
    </div>
   
    <asp:Label runat ="server" ID="lbStatus" ForeColor="Red"/>
   
    </form>
    </body>
</html>
