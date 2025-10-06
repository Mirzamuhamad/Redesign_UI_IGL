<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TrCustRetur.aspx.vb" Inherits="CustRetur" %>
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
                       
        function setformat(_prmchange)
        {
        try
         {          
        var Rate = document.getElementById("tbRate").value.replace(/\$|\,/g,"");
        var PPn = document.getElementById("tbPPN").value.replace(/\$|\,/g,"");        
        var BaseForex = document.getElementById("tbBaseForex").value.replace(/\$|\,/g,"");
        var DiscForex = document.getElementById("tbDiscForex").value.replace(/\$|\,/g,"");                
        var PPnForex = document.getElementById("tbPPNForex").value.replace(/\$|\,/g,"");        
        var TotalForex = document.getElementById("tbTotalForex").value.replace(/\$|\,/g,"");
        var tbPriceTax = document.getElementById("tbPriceTax").value.replace(/\$|\,/g,"");
        if(isNaN(PPn) == true)        
        {   PPn = 0;
        }
        if (tbPriceTax == 'Y')
        {   PPnForex = (parseFloat(TotalForex)*PPn)/(100+parseFloat(PPn));
            BaseForex = ((parseFloat(TotalForex)*100)/(100+parseFloat(PPn))) + parseFloat(DiscForex);
            
        } else    
        {   
            PPnForex = (parseFloat(BaseForex) - parseFloat(DiscForex)) * parseFloat(PPn)/100;
            TotalForex = parseFloat(BaseForex) - parseFloat(DiscForex) + parseFloat(PPnForex);
        }
        document.getElementById("tbRate").value = setdigit(Rate,'<%=VIEWSTATE("DigitRate")%>');
        document.getElementById("tbPPN").value = setdigit(PPn,'<%=VIEWSTATE("DigitPercent")%>');
        document.getElementById("tbBaseForex").value = setdigit(BaseForex,'<%=VIEWSTATE("DigitCurr")%>');
        document.getElementById("tbDiscForex").value = setdigit(DiscForex,'<%=VIEWSTATE("DigitCurr")%>');
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
         var qty = document.getElementById("tbQty").value.replace(/\$|\,/g,"");
         //var qtyM2 = document.getElementById("tbQtyM2").value.replace(/\$|\,/g,"");
         //var qtyRoll = document.getElementById("tbQtyRoll").value.replace(/\$|\,/g,"");         
         var price = document.getElementById("tbPriceForex").value.replace(/\$|\,/g,"");
         var discforex = document.getElementById("tbDiscDtForex").value.replace(/\$|\,/g,"");
         //var priceunit = document.getElementById("ddlPriceUnit").value.replace(/\$|\,/g,"");
         var subtotal = document.getElementById("tbAmountForex").value.replace(/\$|\,/g,"");
         
         if(isNaN(qty) == true)        
         {   qty = 0;
         }
         //if(isNaN(qtyM2) == true)        
         //{   qtyM2 = 0;
         //}
         //if(isNaN(qtyRoll) == true)        
         //{   qtyRoll = 0;
         //}
         if(isNaN(price) == true)        
         {   price = 0;
         }
         if(isNaN(discforex) == true)        
         {   discforex = 0;
         }
         //if(isNaN(priceunit) == true)        
         //{   priceunit = 'Warehouse';
         //}
         
//         if (priceunit == 'M2')
//         {  
//            subtotal = parseFloat(qtyM2)*parseFloat(price);
//         } else if (priceunit == 'Roll')
//         {  
//            subtotal = parseFloat(qtyRoll)*parseFloat(price);
//         } else 
//         {
//            subtotal = parseFloat(qty)*parseFloat(price);
//         }         
         var netto = parseFloat(subtotal) - parseFloat(discforex); 
                           
         document.getElementById("tbQty").value = setdigit(qty,'<%=ViewState("DigitQty")%>');                  
         //document.getElementById("tbQtyM2").value = setdigit(qtyM2,'<%=ViewState("DigitQty")%>');                  
         //document.getElementById("tbQtyRoll").value = setdigit(qtyRoll,'<%=ViewState("DigitQty")%>');                  
         document.getElementById("tbPriceForex").value = setdigit(price,'<%=ViewState("DigitCurr")%>');
         document.getElementById("tbAmountForex").value = setdigit(subtotal,'<%=ViewState("DigitCurr")%>');
         document.getElementById("tbDiscDtForex").value = setdigit(discforex,'<%=ViewState("DigitCurr")%>');
         document.getElementById("tbNettoForex").value = setdigit(netto,'<%=ViewState("DigitCurr")%>');

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
    <div class="H1">Customer Retur </div>
     <hr style="color:Blue" />        
    <asp:Panel runat="server" id="PnlHd"> 
      <table>
        <tr>
            <td style="width:100px;text-align:right">Quick Search :</td>            
            <td><asp:TextBox CssClass="TextBox" runat="server" ID ="tbFilter"/> 
                  <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlField" >
                      <asp:ListItem Value="TransNmbr" Selected="True">Reference</asp:ListItem>
                      <asp:ListItem>Status</asp:ListItem>
                      <%--<asp:ListItem Value="Printed">Printed</asp:ListItem>--%>
                      <asp:ListItem Value="Trans_Date">Date</asp:ListItem>                                                                  
                      <asp:ListItem Value="RR_No">RR No</asp:ListItem>                      
                      <asp:ListItem Value="Customer_Name">Customer Name</asp:ListItem>
                      <asp:ListItem Value="Bill_To_Name">Bill To</asp:ListItem>                      
                      <asp:ListItem Value="Term_Name">Term</asp:ListItem>
                      <asp:ListItem Value="Due_Date">Due Date</asp:ListItem>
                      <asp:ListItem Value="Currency">Currency</asp:ListItem>
                      <asp:ListItem Value="PPnNo">PPn No</asp:ListItem>
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
                      <asp:ListItem Value="TransNmbr" Selected="True">TransNmbr</asp:ListItem>
                      <asp:ListItem>Status</asp:ListItem>
                      <%--<asp:ListItem Value="Printed">Printed</asp:ListItem>--%>
                      <asp:ListItem Value="Trans_Date">Date</asp:ListItem>
                      <asp:ListItem Value="RR_No">RR No</asp:ListItem>                      
                      <asp:ListItem Value="Customer_Name">Customer Name</asp:ListItem>
                      <asp:ListItem Value="Bill_To_Name">Bill To</asp:ListItem>
                      <asp:ListItem Value="Term_Name">Term</asp:ListItem>
                      <asp:ListItem Value="Due_Date">Due Date</asp:ListItem>
                      <asp:ListItem Value="Currency">Currency</asp:ListItem>
                      <asp:ListItem Value="PPnNo">PPn No</asp:ListItem>
                      <asp:ListItem>Remark</asp:ListItem>                      
              </asp:DropDownList>
          </td>              
        </tr>        
      </table>      
      </asp:Panel>
          <br />
            <asp:Button class="bitbtn btnadd" runat="server" ID="BtnAdd" Text="Add" />	            
            &nbsp &nbsp &nbsp   
            <asp:DropDownList CssClass="DropDownList" ID="ddlCommand" runat="server" Visible="false"/>
            <asp:Button class="btngo" runat="server" ID="BtnGo" Text="G" Visible="false"/>                                       <br />
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
                              <asp:ListItem Text="Print" />
                              <%--<asp:ListItem Text="Print Tax" />--%>
                          </asp:DropDownList>
                          <asp:Button class="btngo" runat="server" ID="BtnGo" Text="G"
                           CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" CommandName="Go" />
                      </ItemTemplate>
                      <HeaderStyle Width="110px" />
                  </asp:TemplateField>
                  <asp:BoundField DataField="TransNmbr" HeaderStyle-Width="120px" SortExpression="Nmbr" HeaderText="Nota No"></asp:BoundField>                  
                  <asp:BoundField DataField="Status" HeaderText="Status"></asp:BoundField>                  
                  <asp:BoundField DataField="FgReport" HeaderText="Fg Report"></asp:BoundField>                  
                  <asp:BoundField DataField="Trans_Date" HeaderStyle-Width="80px" SortExpression="TransDate" HeaderText="Date"></asp:BoundField>
                  <asp:BoundField DataField="Customer" HeaderStyle-Width="80px" SortExpression="Customer" HeaderText="Customer"></asp:BoundField>
                  <asp:BoundField DataField="Customer_Name" HeaderStyle-Width="200px" SortExpression="Customer_Name" HeaderText="Customer Name"></asp:BoundField>
                  <asp:BoundField DataField="Bill_To_Name" HeaderStyle-Width="200px" SortExpression="Bill_To_Name" HeaderText="Bill To"></asp:BoundField>
                  <asp:BoundField DataField="Term_Name" HeaderStyle-Width="150px" SortExpression="Term_Name" HeaderText="Term"></asp:BoundField>
                  <asp:BoundField DataField="DueDate" HeaderStyle-Width="80px" SortExpression="DueDate" HeaderText="Due Date"></asp:BoundField>
                  <asp:BoundField DataField="Currency" HeaderStyle-Width="30px" SortExpression="Currency" HeaderText="Currency"></asp:BoundField>
                  <asp:BoundField DataField="BaseForex" HeaderStyle-Width="80px" DataFormatString="{0:#,##0.00}" SortExpression="BaseForex" ItemStyle-HorizontalAlign="Right" HeaderText="Base Forex"></asp:BoundField>
                  <asp:BoundField DataField="PPnForex" HeaderStyle-Width="80px" DataFormatString="{0:#,##0.00}" SortExpression="PPnForex" ItemStyle-HorizontalAlign="Right" HeaderText="PPn Forex"></asp:BoundField>
                  <asp:BoundField DataField="TotalForex" HeaderStyle-Width="80px" DataFormatString="{0:#,##0.00}" SortExpression="TotalForex" ItemStyle-HorizontalAlign="Right" HeaderText="Total Forex"></asp:BoundField>
                  <asp:BoundField DataField="FgPriceTax" HeaderStyle-Width="80px" SortExpression="FgPriceTax" ItemStyle-HorizontalAlign="Left" HeaderText="Price Include Tax"></asp:BoundField>
                  <asp:BoundField DataField="Remark" HeaderStyle-Width="250px" HeaderText="Remark"></asp:BoundField>                  
              </Columns>
          </asp:GridView>
          </div>
            <asp:Panel runat="server" ID ="pnlNav" Visible="false">
            <asp:Button class="bitbtn btnadd" runat="server" ID="btnAdd2" Text="Add" />	            
            &nbsp &nbsp &nbsp
            <asp:DropDownList ID="ddlCommand2" CssClass="DropDownList" runat="server"/>
            <asp:Button class="btngo" runat="server" ID="btnGo2" Text="G" />                             
            </asp:Panel>
    </asp:Panel>    
    <asp:Panel runat="server" ID="pnlInput" Visible="false">
      <table>
        <tr>
            <td>Nota No</td>
            <td>:</td>
            <td><asp:TextBox CssClass="TextBoxR" runat="server" ID="tbCode" Width="150px" Enabled="False"/>                          
            <%--<asp:DropDownList AutoPostBack="true" Visible="True" Enabled="False" CssClass="DropDownList" ID="ddlReport" runat="server" >
                        <asp:ListItem>N</asp:ListItem>
                        <asp:ListItem>Y</asp:ListItem>
                    </asp:DropDownList>--%> 
                &nbsp &nbsp &nbsp &nbsp Nota Date : &nbsp &nbsp    
                <BDP:BasicDatePicker ID="tbDate" runat="server" DateFormat="dd MMM yyyy" 
                        ReadOnly = "true" ValidationGroup="Input"
                        ButtonImageHeight="19px" ButtonImageWidth="20px" 
                        DisplayType="TextBoxAndImage" 
                        TextBoxStyle-CssClass="TextDate" AutoPostBack="True" 
                        ShowNoneButton="False">
                        <TextBoxStyle CssClass="TextDate" />
             </BDP:BasicDatePicker>  
             
             <asp:Button class="bitbtn btngo" ValidationGroup="Input" runat="server" 
                    ID="btnGetData" Text="Get Data" Visible="false" Width="74px" ReadOnly = "true" />    
                    
            </td>   
            
       </tr>               
            
        <tr>
            <td><asp:LinkButton ID="lbCust" ValidationGroup="Input"  runat="server" Text="Customer"/></td>
            <td>:</td>
            <td><asp:TextBox CssClass="TextBox" ValidationGroup="Input" runat="server" ID="tbCustCode" AutoPostBack="true" />
                <asp:TextBox CssClass="TextBox" runat="server" ID="tbCustName" Enabled="false" Width="225px"/>
                <asp:Button Class="btngo" ID="btnCust" Text="..." runat="server" ValidationGroup="Input" />                                                                                       
                
            </td>
        </tr>
        <tr>
            <td><asp:Label ID="lbBillTo" ValidationGroup="Input"  runat="server" Text="Bill To"/></td>
            <td>:</td>
            <td><asp:TextBox CssClass="TextBox" ValidationGroup="Input" runat="server" ID="tbBillToCode" AutoPostBack="true" />
                <asp:TextBox CssClass="TextBox" runat="server" ID="tbBillToName" Enabled="false" Width="225px"/>
                <asp:Button Class="btngo" ID="btnBillTo" Text="..." runat="server" ValidationGroup="Input" />                                                                                       
                
            </td>
        </tr>
        <tr>
            <td>Attn</td>
            <td>:</td>
            <td><asp:TextBox runat="server" ValidationGroup="Input" ID="tbAttn" CssClass="TextBox" Width="225px"/></td>
        </tr>
        <tr>
            <td><asp:LinkButton ID="lbTerm" ValidationGroup="Input" runat="server" Text="Term"/></td>
            <td>:</td>
            <td><asp:DropDownList CssClass="DropDownList" ValidationGroup="Input" runat="server" ID="ddlTerm" AutoPostBack="true" /> 
                <BDP:BasicDatePicker ID="tbDueDate" runat="server" DateFormat="dd MMM yyyy" 
                        ReadOnly = "true" Enabled="false"
                        ButtonImageHeight="19px" ButtonImageWidth="20px" 
                        DisplayType="TextBox" 
                        TextBoxStyle-CssClass="TextDate" AutoPostBack="True" 
                        ShowNoneButton="False">
                        <TextBoxStyle CssClass="TextDate" />
                </BDP:BasicDatePicker>
            </td>                    
        </tr>  
                
        <tr>
            <td>PPn</td>
            <td>:</td>
            <td colspan="3">
                <table>
                <tr style="background-color:Silver;text-align:center">
                   <td>PPn No</td>
                   <td>PPn Date</td>
                   <td>PPn Rate</td>
                 </tr>
                 <tr>
                 <td>
                 <asp:TextBox runat="server" ValidationGroup="Input" ID="tbPPnNo" CssClass="TextBox" Width="150px"/>
                 </td>
                 <td>
                 <BDP:BasicDatePicker ID="tbPPndate" runat="server" DateFormat="dd MMM yyyy" 
                            ReadOnly = "true" ValidationGroup="Input"
                            ButtonImageHeight="19px" ButtonImageWidth="20px" 
                            DisplayType="TextBoxAndImage" 
                            TextBoxStyle-CssClass="TextDate" AutoPostBack="True" ShowNoneButton="False">
                            <TextBoxStyle CssClass="TextDate" />
                 </BDP:BasicDatePicker>                
                 </td>
                 <td>
                 <asp:TextBox runat="server" ValidationGroup="Input" ID="tbPpnRate" CssClass="TextBox" Width="68px"/>
                 </td>
                 </tr>
                 </table>
            </td>                         
        </tr> 
        <tr>
                <td><asp:LinkButton ID="lbCurr" ValidationGroup="Input" runat="server" Text="Currency"/></td>
                <td>:</td>
                <td><asp:DropDownList ID="ddlCurr" ValidationGroup="Input" AutoPostBack="true" runat="server" CssClass="DropDownList" />                                                                   
                &nbsp Rate : &nbsp <asp:TextBox ID="tbRate" ValidationGroup="Input" runat="server" CssClass="TextBox" Width="68px" />
                &nbsp Price Include Tax : &nbsp <asp:TextBox runat="server"  CssClass="TextBoxR"
                                                 ID="tbPriceTax" Width="50px"/>
                </td>
        </tr>  
        <tr>
                <td>Total</td>
                <td>:</td>
                <td colspan="5">
                    <table>
                        <tr style="background-color:Silver;text-align:center">
                            <td>Base Forex</td>
                            <td>Disc Forex</td>
                            <td>PPN %</td>
                            <td>PPN Forex</td>
                            <td>Total Forex</td>
                        </tr>
                        <tr>
                            <td><asp:TextBox ID="tbBaseForex" runat="server" CssClass="TextBoxR" Width="90px"/></td>
                            <%--<td><asp:TextBox ID="tbDisc" runat="server" CssClass="TextBox" width="40px"/></td>--%>
                            <td><asp:TextBox ID="tbDiscForex" runat="server" CssClass="TextBoxR" Width="90px"/></td>
                            <td><asp:TextBox ID="tbPPN" ValidationGroup="Input" runat="server" CssClass="TextBox" width="40px"  AutoPostBack ="true"/></td>
                            <td><asp:TextBox ID="tbPPNForex" runat="server" CssClass="TextBoxR" Width="90px"/></td>                            
                            <td><asp:TextBox ID="tbTotalForex" runat="server" CssClass="TextBoxR" Width="90px"/></td>
                        </tr>
                    </table>
                </td>                
        </tr>   
        <%--<tr>
            <td><asp:Label ID="lbBankReceipt" ValidationGroup="Input" runat="server" Text="Bank Receipt"/></td>
            <td>:</td>
            <td><asp:DropDownList CssClass="DropDownList" ValidationGroup="Input" runat="server" ID="ddlBankReceipt" />                 
            </td>
                                
        </tr>  --%>
        <tr>
            <td>Remark</td>
            <td>:</td>
            <td><asp:TextBox runat="server" ValidationGroup="Input" ID="tbRemark" CssClass="TextBoxMulti" 
                         MaxLength="255" TextMode="MultiLine"  Width="435px"/></td>
            <td>
            </td>
            <td>
            </td>
            <td>&nbsp;</td>
        </tr>        
      </table>  
      
      <br />      
      <div style="font-size:medium; color:Blue;">Detail</div>
      <hr style="color:Blue" />  
        <asp:Panel runat="server" ID="pnlDt">          
            <asp:Button class="bitbtn btnadd" runat="server" ID="btnAddDt" Text="Add" 
                Visible="False" />	
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
                            <%--<FooterTemplate>
                            <asp:ImageButton ID="btnAddDt2" ValidationGroup="Input" runat="server"  
                                    ImageUrl="../../Image/btnAddDtOn.png"
                                    onmouseover="this.src='../../Image/btnAddDtOff.png';"
                                    onmouseout="this.src='../../Image/btnAddDtOn.png';"
                                    ImageAlign="AbsBottom" />        
                            </FooterTemplate>--%>                            
                        </asp:TemplateField>                        
                        <asp:BoundField DataField="RRNo" HeaderStyle-Width="50px" HeaderText="RR No" />                                                
                        <asp:BoundField DataField="SJNo" HeaderStyle-Width="50px" HeaderText="SJ No" />                                                
                        <asp:BoundField DataField="Product" HeaderStyle-Width="80px" HeaderText="Product" />
                        <asp:BoundField DataField="Product_Name" HeaderStyle-Width="150px" HeaderText="Product Name" />                        
                        <asp:BoundField DataField="Qty" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Right" HeaderText="Qty" />
                        <asp:BoundField DataField="Unit" HeaderStyle-Width="80px" HeaderText="Unit" />
                        <asp:BoundField DataField="QtyM2" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Right" HeaderText="Qty M2" Visible="False"/>
                        <asp:BoundField DataField="QtyRoll" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Right" HeaderText="Qty Roll" Visible="False"/>                            
                        <asp:BoundField DataField="PriceForex" DataFormatString="{0:#,##0.####}" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Right" HeaderText="Price Forex" />                           
                        <asp:BoundField DataField="AmountForex" DataFormatString="{0:#,##0.####}" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Right" HeaderText="Amount Forex" />
                        <asp:BoundField DataField="Disc" DataFormatString="{0:#,##0.####}" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Right" HeaderText="Discount (%)" />
                        <asp:BoundField DataField="DiscForex" DataFormatString="{0:#,##0.####}" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Right" HeaderText="Discount Forex" />
                        <asp:BoundField DataField="NettoForex" DataFormatString="{0:#,##0.####}" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Right" HeaderText="Netto Forex" />
                        <asp:BoundField DataField="Remark" HeaderStyle-Width="200px" HeaderText="Remark" />
                    </Columns>
                </asp:GridView>
          </div>   
         <asp:Button class="bitbtn btnadd" runat="server" ID="btnAddDt2" Text="Add" 
                Visible="False" />	       
       </asp:Panel>       
       <asp:Panel runat="server" ID="pnlEditDt" Visible="false">
            <table>    
                <tr>
                    <td>RR - SJ No</td>
                    <td>:</td>
                    <td><asp:TextBox CssClass="TextBox" runat="server" ID="tbBPBNo" Enabled="false" />
                    <asp:TextBox CssClass="TextBox" runat="server" ID="tbSJNo" Enabled="false" />
                    <asp:Button Class="btngo" ID="btnBPBNo" Text="..." runat="server" ValidationGroup="Input" visible = "false" />                                                                                                  
                    </td>
                </tr>                                
                <tr>
                    <td>Product</td>
                    <td>:</td>
                    <td><asp:TextBox runat="server" ID="tbProductCode" CssClass="TextBox" Enabled="false" Width="130px" />
                        <asp:TextBox runat="server"  CssClass="TextBox"
                            ID="tbProductName" Enabled="false" Width="250px"/>
                     </td>                               
                </tr>                                                    
                <tr>
                    <td>Qty</td>
                    <td>:</td>
                    <td><asp:TextBox CssClass="TextBox" runat="server" ID="tbQty" Enabled="false" />
                        <asp:TeXtBox ID="tbUnit" runat="server" CssClass="TextBox" Enabled="false" />
                    </td>
                </tr>      
                <tr>
                    <td><asp:Label ID="LbQtyM2" runat="server" Text="Qty M2" Visible="False"/> </td>
                    <td><asp:Label ID="lbDot1" runat="server" Text=":" Visible="False"/></td>
                    <td><asp:TextBox CssClass="TextBox" runat="server" ID="tbQtyM2" AutoPostBack="True" style="text-align:right;" Visible="False"/></td>
                </tr>
                 <tr>
                    <td><asp:Label ID="LbQtyRoll" runat="server" Text="Qty Roll" Visible="False"/> </td> 
                    <td><asp:Label ID="lbDot2" runat="server" Text=":" Visible="False"/></td>
                    <td><asp:TextBox CssClass="TextBox" runat="server" ID="tbQtyRoll" AutoPostBack="True" style="text-align:right;" Visible="False"/></td>
                </tr>
                
                <%--<tr>
                    <td>Price In Unit</td>
                    <td>:</td>
                    <td><asp:DropDownList Id ="ddlPriceUnit" Enabled="true" runat="server" 
                        CssClass="DropDownList" AutoPostBack="True" ValidationGroup="Input" >
                        <asp:ListItem Selected="True" >Warehouse</asp:ListItem>
                        <asp:ListItem>M2</asp:ListItem>
                        <asp:ListItem>Roll</asp:ListItem>
                    </asp:DropDownList></td>
                    <td><asp:DropDownList ID="ddlUnit" ValidationGroup="Input" AutoPostBack="true" runat="server" CssClass="DropDownList" /></td>                    
                </tr>   --%>             

                          
                <tr>
                    <td>Amount</td>
                    <td>:</td>
                    <td>
                      <table>
                                <tr style="background-color:Silver;text-align:center">
                                    <td>Price Forex</td>
                                    <td>Amount Forex</td>
                                    <td>Disc (%)</td>
                                    <td>Disc Forex</td>
                                    <td>Netto Forex</td>
                                </tr>
                                <tr>
                                    <td><asp:TextBox runat="server" ID="tbPriceForex" AutopostBack = "True" CssClass="TextBox"/></td>
                                    <td><asp:TextBox runat="server" ID="tbAmountForex" CssClass="TextBoxR"/></td>
                                    <td><asp:TextBox runat="server" ID="tbDiscDt" ValidationGroup="Input" Width="40px" CssClass="TextBox" AutoPostBack="True"/></td>
                                    <td><asp:TextBox runat="server" ID="tbDiscDtForex" ValidationGroup="Input" CssClass="TextBox" AutoPostBack="True"/></td>
                                    <td><asp:TextBox runat="server" ID="tbNettoForex" CssClass="TextBoxR"/></td>
                                </tr>
                       </table>                        
                    </td>
                </tr>                                 
                <tr>
                    <td>Remark </td>
                    <td>:</td>
                    <td><asp:TextBox runat="server" ID="tbRemarkDt" CssClass="TextBoxMulti" 
                         MaxLength="255" TextMode="MultiLine"  Width="435px"/></td>
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
    <asp:Label runat ="server" ID="lbCustCode" ForeColor="Red" Visible = False/>
    </form>
    </body>
</html>
