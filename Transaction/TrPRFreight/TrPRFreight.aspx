<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TrPRFreight.aspx.vb" Inherits="Transaction_TrPRFreight_TrPRFreight" %>

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
       
       function setformat()
        {
        try
         {        
          var VPayment = document.getElementById("tbTotalPayment").value.replace(/\$|\,/g,"");
          var VExpense = document.getElementById("tbTotalExpense").value.replace(/\$|\,/g,"");
          var VCharge = document.getElementById("tbTotalCharge").value.replace(/\$|\,/g,"");
          
          document.getElementById("tbTotalPayment").value = setdigit(VPayment,'<%=ViewState("DigitHome")%>');
          document.getElementById("tbTotalExpense").value = setdigit(VExpense,'<%=ViewState("DigitHome")%>');
          document.getElementById("tbTotalCharge").value = setdigit(VCharge,'<%=ViewState("DigitHome")%>');
        }catch (err){
            alert(err.description);
          }      
        }   
        
       function setformatdt()
        {
        try
         {         
         var AmountForex = document.getElementById("tbAmountForexDt").value.replace(/\$|\,/g,""); 
         var AmountHome = document.getElementById("tbAmountHomeDt").value.replace(/\$|\,/g,""); 
         var Rate = document.getElementById("tbRateDt").value.replace(/\$|\,/g,""); 
                          
         document.getElementById("tbRateDt").value = setdigit(Rate,'<%=ViewState("DigitRate")%>');
         document.getElementById("tbAmountForexDt").value = setdigit(AmountForex,'<%=VIEWSTATE("DigitCurrAcc")%>');
         document.getElementById("tbAmountHomeDt").value = setdigit(AmountHome,'<%=ViewState("DigitHome")%>');
         
        }catch (err){
            alert(err.description);
          }      
        }   

        function setformatdt2(Change)
        {
        try
         {
         var Rate = document.getElementById("tbRateDt2").value.replace(/\$|\,/g,""); 
         var AmountForex = document.getElementById("tbAmountForexDt2").value.replace(/\$|\,/g,""); 
         var PPn = document.getElementById("tbPPnDt2").value.replace(/\$|\,/g,""); 
         var PPnForex = document.getElementById("tbPPnForexDt2").value.replace(/\$|\,/g,""); 
         var TotalForex = document.getElementById("tbTotalForexDt2").value.replace(/\$|\,/g,""); 
         
//         PPnForex = parseFloat(AmountForex) * (parseFloat(PPn) / 100)
//         TotalForex = parseFloat(AmountForex) + parseFloat(PPnForex) 
         
         if (Change == 'AF' ){
           PPnForex = parseFloat(AmountForex) * (parseFloat(PPn) / 100)
           TotalForex = parseFloat(AmountForex) + parseFloat(PPnForex) 
         }
         if (Change == 'PP' ){
           if (AmountForex > 0){
             PPnForex = parseFloat(AmountForex) * (parseFloat(PPn) / 100)
           } else PPnForex = 0
           TotalForex = parseFloat(AmountForex) + parseFloat(PPnForex) 
         }
         if (Change == 'PF' ){
           PPn = (parseFloat(PPnForex) / parseFloat(AmountForex)) * 100           
           TotalForex = parseFloat(AmountForex) + parseFloat(PPnForex) 
         }               
         if (Change == 'TF' ){
           AmountForex = (parseFloat(TotalForex) * 100) / (100 + parseFloat(PPn))
           PPnForex = (parseFloat(TotalForex) * parseFloat(PPn)) / (100 + parseFloat(PPn))
           PPn = (parseFloat(PPnForex) / parseFloat(AmountForex)) * 100
         }                                   
                                                                      
         document.getElementById("tbRateDt2").value = setdigit(Rate,'<%=ViewState("DigitRate")%>');
         document.getElementById("tbAmountForexDt2").value = setdigit(AmountForex,'<%=VIEWSTATE("DigitCurr")%>');
         //document.getElementById("tbPPnDt2").value = setdigit(PPn,'<%=VIEWSTATE("DigitPercent")%>');
         document.getElementById("tbPPnDt2").value = setdigit(PPn,'<%=VIEWSTATE("DigitCurr")%>');
         document.getElementById("tbPPnForexDt2").value = setdigit(PPnForex,'<%=VIEWSTATE("DigitCurr")%>');
         document.getElementById("tbTotalForexDt2").value = setdigit(TotalForex,'<%=VIEWSTATE("DigitCurr")%>');
         
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
    <div class="H1">Purchase Cost</div>
     <hr style="color:Blue" />        
    <asp:Panel runat="server" id="PnlHd"> 
      <table>
        <tr>
            <td style="width:100px;text-align:right">Quick Search :</td>         
            <%--TransNmbr, TransDate, STATUS, FgReport, UserType, UserCode, UserName, Attn, Remark--%>   
            <td><asp:TextBox CssClass="TextBox" runat="server" ID ="tbFilter"/> 
                  <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlField" >                      
                    <asp:ListItem Selected="True" Value="TransNmbr">Purchase Cost No</asp:ListItem>
                    <asp:ListItem Value="dbo.FormatDate(TransDate)">Purchase Cost Date</asp:ListItem>
                    <asp:ListItem>Status</asp:ListItem>
                    <asp:ListItem Value="PONo">PO No</asp:ListItem>
                    <asp:ListItem Value="Supplier">Supplier Code</asp:ListItem>
                    <asp:ListItem Value="Supplier_Name">Supplier Name</asp:ListItem>                                        
                    <asp:ListItem Value="BLNo">BL No</asp:ListItem>                    
                    <asp:ListItem Value="AJUNo">AJU No</asp:ListItem>
                    <asp:ListItem Value="ContainerNo">Container No</asp:ListItem>
                    <asp:ListItem Value="RRNo">RR No</asp:ListItem>
                    <asp:ListItem Value="Remark">Remark</asp:ListItem>
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
          <td style="width:100px;text-align:right">
              <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlNotasi" >
                <asp:ListItem Selected="true" Text="OR" Value="OR"></asp:ListItem>
                <asp:ListItem Text="AND" Value="AND"></asp:ListItem>            
              </asp:DropDownList>
          </td>
          <td>
              <asp:TextBox runat="server" CssClass="TextBox" ID ="tbfilter2"/> 
              <asp:DropDownList runat="server" CssClass="DropDownList" ID="ddlField2" >                     
                    <asp:ListItem Selected="True" Value="TransNmbr">Purchase Cost No</asp:ListItem>
                    <asp:ListItem Value="dbo.FormatDate(TransDate)">Purchase Cost Date</asp:ListItem>
                    <asp:ListItem>Status</asp:ListItem>
                    <asp:ListItem Value="PONo">PO No</asp:ListItem>
                    <asp:ListItem Value="Supplier">Supplier Code</asp:ListItem>
                    <asp:ListItem Value="Supplier_Name">Supplier Name</asp:ListItem>                                        
                    <asp:ListItem Value="BLNo">BL No</asp:ListItem>                    
                    <asp:ListItem Value="AJUNo">AJU No</asp:ListItem>
                    <asp:ListItem Value="ContainerNo">Container No</asp:ListItem>
                    <asp:ListItem Value="RRNo">RR No</asp:ListItem>
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
            <asp:Button class="btngo" runat="server" ID="BtnGo" Text="G" Visible="false" />                         
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
                              <%--<asp:ListItem Text="Print" />--%>
                              
                          </asp:DropDownList>
                          <asp:Button class="btngo" runat="server" ID="BtnGo" Text="G" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" CommandName="Go" />                
                       </ItemTemplate>
                      <HeaderStyle Width="110px" />
                  </asp:TemplateField>
                  <asp:BoundField DataField="TransNmbr" HeaderStyle-Width="120px" SortExpression="Nmbr" HeaderText="Purchase Cost No"></asp:BoundField>                  
                  <asp:BoundField DataField="Status" HeaderText="Status"></asp:BoundField>                  
                  <asp:BoundField DataField="TransDate" dataformatstring="{0:dd MMM yyyy}" htmlencode="true" HeaderStyle-Width="80px" SortExpression="TransDate" HeaderText="Purchase Cost Date"></asp:BoundField>
                  <asp:BoundField DataField="PONo" HeaderStyle-Width="80px" SortExpression="PONo" HeaderText="PO No"></asp:BoundField>
                  <asp:BoundField DataField="Supplier" HeaderStyle-Width="80px" SortExpression="Supplier" HeaderText="Supplier Code"></asp:BoundField>
                  <asp:BoundField DataField="Supplier_Name" HeaderStyle-Width="200px" SortExpression="Supplier_Name" HeaderText="Supplier Name"></asp:BoundField>
                  <asp:BoundField DataField="BLNo" HeaderStyle-Width="80px" SortExpression="BLNo" HeaderText="BL No"></asp:BoundField>
                  <asp:BoundField DataField="AJUNo" HeaderStyle-Width="80px" SortExpression="AJUNo" HeaderText="AJU No"></asp:BoundField>
                  <asp:BoundField DataField="ContainerNo" HeaderStyle-Width="200px" SortExpression="ContainerNo" HeaderText="Container No"></asp:BoundField>                  
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
            <td>Purchase Cost No</td>
            <td>:</td>
            <td><asp:TextBox CssClass="TextBoxR" runat="server" ID="tbCode" Width="150px" Enabled="False"/></td>
        
            <td>Purchase Cost Date</td>
            <td>:</td>
            <td>
            <BDP:BasicDatePicker ID="tbDate" runat="server" DateFormat="dd MMM yyyy" 
                        ReadOnly = "true" ValidationGroup="Input"
                        ButtonImageHeight="19px" ButtonImageWidth="20px" 
                        DisplayType="TextBoxAndImage" 
                        TextBoxStyle-CssClass="TextDate" 
                        ShowNoneButton="False"><TextBoxStyle CssClass="TextDate" /></BDP:BasicDatePicker>                
            </td>            
        </tr> 
        <tr>
            <td>Supplier</td>
            <td>:</td>
            <td colspan="4">
                <asp:TextBox CssClass="TextBox" ValidationGroup="Input" runat="server" ID="tbSuppCode" MaxLength="12" AutoPostBack="true" />
                <asp:TextBox CssClass="TextBox" runat="server" ID="tbSuppName" Enabled="false" MaxLength="60" Width="225px"/>
                <asp:Button Class="btngo" ID="btnSupp" Text="..." runat="server" ValidationGroup="Input" />                                  
            </td>
        </tr>
        <tr>
            <td>PO No</td>
            <td>:</td>
            <td colspan="4"><asp:TextBox CssClass="TextBox" Enabled ="false" runat="server" ID="tbPONo" MaxLength="20" Width="150px" />
                <asp:Button Class="btngo" ID="btnPONo" Text="..." runat="server" ValidationGroup="Input" />                                  
                <asp:Button class="bitbtn btngetitem" runat="server" ID="btnGetDt" Text="Get Item" />     
            </td> 
        </tr>
        
        <tr>
            <td>BL No</td>
            <td>:</td>
            <td colspan="4"><asp:TextBox runat="server" ValidationGroup="Input" ID="tbBLNo" MaxLength="100" CssClass="TextBox" Width="280px"/></td>
        </tr>
        <tr>
            <td>AJU No</td>
            <td>:</td>
            <td colspan="4"><asp:TextBox runat="server" ValidationGroup="Input" ID="tbAJUNo" MaxLength="100" CssClass="TextBox" Width="280px"/></td>
        </tr>
        <tr>
            <td>Container No</td>
            <td>:</td>
            <td colspan="4"><asp:TextBox runat="server" ValidationGroup="Input" ID="tbContainerNo" MaxLength="100" CssClass="TextBox" Width="280px"/></td>
        </tr> 
        <tr>
            <td>Remark</td>
            <td>:</td>
            <td colspan="4"><asp:TextBox runat="server" ValidationGroup="Input" ID="tbRemark" MaxLength="255" CssClass="TextBoxMulti" Width="380px" TextMode="MultiLine"/></td>
        </tr>
      </table>  
      
      <br />      
      <hr style="color:Blue" />  
       <asp:Menu
            ID="Menu1"
            runat="server"
            CssClass = "Menu"        
            StaticMenuItemStyle-CssClass = "MenuItem"
            StaticSelectedStyle-CssClass = "MenuSelect"        
            Orientation="Horizontal"
            ItemWrap = "False"
            StaticEnableDefaultPopOutImage="False">            
            <Items>
                <asp:MenuItem Text="Detail RR" Value="0"></asp:MenuItem>
                <asp:MenuItem Text="Detail Cost" Value="1"></asp:MenuItem>
            </Items>            
        </asp:Menu>
        <br />
        <asp:MultiView 
        ID="MultiView1"
        runat="server"
        ActiveViewIndex="0">
           <asp:View ID="Tab1" runat="server">
              <asp:Panel runat="server" ID="PnlDt">
                <%--<asp:Button class="bitbtn btnadd" runat="server" ID="btnAdddt" Text="Add" Visible="false" ValidationGroup="Input" />	--%>
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
   							          <%--<asp:Button ID="btnEdit" runat="server" class="bitbtndt btnedit" Text="Edit" CommandName="Edit"/>--%>
								      <asp:Button ID="btnDelete" runat="server" class="bitbtndt btndelete" Text="Delete" OnClientClick="return confirm('Sure to delete this data?');" CommandName="Delete"/>									
                                      </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="RRNo" HeaderStyle-Width="150px" HeaderText="RR No" />
                            <asp:BoundField DataField="Product" HeaderStyle-Width="150px" HeaderText="Product" />
                            <asp:BoundField DataField="Product_Name" HeaderStyle-Width="250px" HeaderText="Product Name" />
                            <asp:BoundField DataField="Qty" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="80px" HeaderText="Qty" />
                            <asp:BoundField DataField="Unit" HeaderStyle-Width="80px" HeaderText="Unit" />                            
                        </Columns>
                    </asp:GridView>
              </div>   
              <%--<asp:Button class="bitbtn btnadd" runat="server" ID="btnAddDtke2" Text="Add" Visible="false" ValidationGroup="Input" />	--%>
              </asp:Panel> 
              <asp:Panel runat="server" ID="pnlEditDt" Visible="false">
                <table>              
                    <tr>
                        <td>RR No</td>
                        <td>:</td>
                        <td colspan="4">                                
                            <asp:TextBox CssClass="TextBox" runat="server" ID="tbRRNo" MaxLength="20" Enabled="false" Width="150px"/>
                            <asp:Button Class="btngo" ID="btnRRNo" Text="..." runat="server" ValidationGroup="Input" />                                  
                           
                        </td>
                    </tr>  
                    <tr>                    
                        <td>Product</td>
                        <td>:</td>
                        <td colspan="4">                                
                            <asp:TextBox CssClass="TextBox" ValidationGroup="Input" runat="server" MaxLength="20" ID="tbProductCode" AutoPostBack="true" />
                            <asp:TextBox CssClass="TextBox" runat="server" ID="tbProductName" Enabled="false" MaxLength="60" Width="225px"/>
                            <asp:Button Class="btngo" Visible="false" ID="btnProduct" Text="..." runat="server" ValidationGroup="Input" />                                  
                            
                        </td>
                    </tr>
                                       
                    <tr> 
                        <td>Qty</td>
                        <td>:</td>
                        <td>                            
                            <asp:TextBox CssClass="TextBox" runat="server" ID="tbQty"/>                                    
                            <asp:DropDownList CssClass="DropDownList" ID="ddlUnit" runat="server" Enabled ="false"/>
                        </td>                        
                    </tr>
                    
                </table>
                <br />                     
                <asp:Button ID="btnSaveDt" runat="server" class="bitbtndt btnsave" Text="Save" />									
                <asp:Button ID="btnCancelDt" runat="server" class="bitbtndt btncancel" Text="Cancel" />									
           </asp:Panel> 
              
           </asp:View>           
            <asp:View ID="Tab2" runat="server">
                <asp:Panel ID="pnlDt2" runat="server">  
                <asp:Button class="bitbtn btnadd" runat="server" ID="btnAddDt2" Text="Add" ValidationGroup="Input" />	              
                
                <div style="border:0px  solid; width:100%; height:100%; overflow:auto;">
                    <asp:GridView ID="GridDt2" runat="server" AutoGenerateColumns="False" 
                        ShowFooter="True">
                        <HeaderStyle CssClass="GridHeader" />
                        <RowStyle CssClass="GridItem" Wrap="false" />
                        <AlternatingRowStyle CssClass="GridAltItem" />
                        <PagerStyle CssClass="GridPager" />
                        <Columns>
                            <asp:TemplateField HeaderText="Action" Visible="false" >
                                <ItemTemplate>
   							       <asp:Button ID="btnEdit" runat="server" class="bitbtndt btnedit" Text="Edit" CommandName="Edit"/>
								   <asp:Button ID="btnDelete" runat="server" class="bitbtndt btndelete" Text="Delete" OnClientClick="return confirm('Sure to delete this data?');" CommandName="Delete"/>									
                                </ItemTemplate>
                                <EditItemTemplate>
                               	    <asp:Button ID="btnUpdate" runat="server" class="bitbtndt btnsave" Text="Save" CommandName="Update"/>									
                                    <asp:Button ID="btnCancel" runat="server" class="bitbtndt btncancel" Text="Cancel" CommandName="Cancel"/>									
                                </EditItemTemplate>
                            </asp:TemplateField>                                      
                            
                            <asp:BoundField DataField="CostFreight" HeaderStyle-Width="150px" HeaderText="Cost Freight"  />
                            <asp:BoundField DataField="CostFreightName" HeaderStyle-Width="150px" HeaderText="Cost Freight Name" />
                            <asp:BoundField DataField="Currency" HeaderStyle-Width="80px" HeaderText="Currency" />
                            <asp:BoundField DataField="ForexRate" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="80px" HeaderText="Rate" />
                            <asp:BoundField DataField="AmountForex" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="80px" HeaderText="Amount Forex" />
                            <asp:BoundField DataField="PPn" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="80px" HeaderText="PPn" />
                            <asp:BoundField DataField="PPnForex" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="80px" HeaderText="PPn Forex" />
                            <asp:BoundField DataField="TotalForex" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="80px" HeaderText="Total Forex" />
                            <asp:BoundField DataField="Consignee" HeaderStyle-Width="120px" HeaderText="Consignee" />
                            <asp:BoundField DataField="Consignee_Name" HeaderStyle-Width="180px" HeaderText="Consignee Name" />
                            <asp:BoundField DataField="CostDistribution" HeaderStyle-Width="120px" HeaderText="Cost Distribution" />
                            <asp:BoundField DataField="FgPrepaid" HeaderStyle-Width="80px" HeaderText="Prepaid" />
                            <asp:BoundField DataField="Reference" HeaderStyle-Width="120px" HeaderText="Reference" />
                            
                        </Columns>
                    </asp:GridView>
              </div>    
              <asp:Button class="bitbtn btnadd" runat="server" ID="btnAddDt2Ke2" Text="Add" ValidationGroup="Input" />	              
   
              </asp:Panel>
              <asp:Panel runat="server" ID="pnlEditDt2" Visible="false">
                <table>              
                    <tr>                    
                        <td>Cost Freight</td>
                        <td>:</td>
                        <td>                                
                            <asp:TextBox CssClass="TextBox" runat="server" ID="tbCostFreight" MaxLength="5" AutoPostBack="true" />
                            <asp:TextBox CssClass="TextBox" runat="server" ID="tbCostFreightName" Enabled="false" MaxLength="60" Width="225px"/>
                            <asp:Button Class="btngo" ID="btnCostFreight" Text="..." runat="server" />  
                        </td>
                    </tr>   
                    <tr>                    
                        <td>Consignee</td>
                        <td>:</td>
                        <td>                                
                            <asp:TextBox CssClass="TextBox" ValidationGroup="Input" runat="server" ID="tbConsignee" MaxLength="12" AutoPostBack="true" />
                            <asp:TextBox CssClass="TextBox" runat="server" ID="tbConsigneeName" Enabled="false" MaxLength="60" Width="225px"/>
                            <asp:Button Class="btngo" ID="btnConsignee" Text="..." runat="server" />                                  
                            
                        </td>
                    </tr> 
                    <tr>
                        <td>Prepaid</td>
                        <td>:</td>
                        <td>
                            <asp:DropDownList AutoPostBack="true" CssClass="DropDownList" ID="ddlFgPrepaid" runat="server" >
                            <asp:ListItem Selected="True">Y</asp:ListItem>
                            <asp:ListItem>N</asp:ListItem>
                            </asp:DropDownList>                         
                            &nbsp &nbsp &nbsp &nbsp &nbsp &nbsp Reference : &nbsp 
                            <asp:TextBox CssClass="TextBox" runat="server" ID="tbReference" Enabled="false" MaxLength="20" Width="157px"/>
                            <asp:Button Class="btngo" ID="btnPayNonTrade" Text="..." runat="server"/>                                  
                        </td>                         
                    </tr>
                    <tr> 
                        <td>Nominal</td>
                        <td>:</td>
                        <td>
                            <table>
                                <tr style="background-color:Silver;text-align:center">
                                    <td>Currency</td>
                                    <td>Rate</td>
                                    <td>Amount Forex</td>
                                    <td>PPn</td>
                                    <td>PPn Forex</td>
                                    <td>Total Forex</td>
                                    <td>Cost Distribution</td>
                                </tr>                             
                                <tr>
                                    <td><asp:DropDownList CssClass="DropDownList" AutoPostBack="true" ID="ddlCurrDt2" runat="server" Width="60px"/></td>
                                    <td><asp:TextBox CssClass="TextBox" runat="server" ID="tbRateDt2" Width="65px"/></td>                                    
                                    <td><asp:TextBox CssClass="TextBox" runat="server" ID="tbAmountForexDt2" Width="80px"/></td>
                                    <td><asp:TextBox CssClass="TextBox" runat="server" ID="tbPPnDt2" Width="80px"/></td>
                                    <td><asp:TextBox CssClass="TextBox" runat="server" ID="tbPPnForexDt2" Width="80px"/></td>
                                    <td><asp:TextBox CssClass="TextBox" runat="server" ID="tbTotalForexDt2" Width="80px"/></td>
                                    <td>
                                        <asp:DropDownList AutoPostBack="true" CssClass="DropDownList" ID="ddlCostDistribution" runat="server" Width="100%" >
                                        <asp:ListItem Selected="True">Weight</asp:ListItem>
                                        <asp:ListItem>Volume</asp:ListItem>
                                        <asp:ListItem>Cost</asp:ListItem>
                                        </asp:DropDownList> 
                                    </td>
                                </tr>
                            </table>
                        </td>                    
                    </tr>                  
                </table>
                <br />                     
                <asp:Button ID="btnSaveDt2" runat="server" class="bitbtndt btnsave" Text="Save" />									
                <asp:Button ID="btnCancelDt2" runat="server" class="bitbtndt btncancel" Text="Cancel" />									


           </asp:Panel> 
               
            </asp:View>            
        </asp:MultiView>
    
       <br />          
		<asp:Button ID="btnSaveAll" runat="server" class="bitbtndt btnsavenew" Text="Save & New" validationgroup="Input" Width = "90"/>									
        <asp:Button ID="btnSaveTrans" runat="server" class="bitbtndt btnsave" Text="Save" validationgroup="Input"/>									
        <asp:Button ID="btnBack" runat="server" class="bitbtndt btncancel" Text="Cancel" validationgroup="Input"/>									
        <asp:Button ID="btnHome" runat="server" class="bitbtndt btnback" Text="Home" />									                                                           
    </asp:Panel>        
    </div>
   
    <asp:Label runat ="server" ID="lbStatus" ForeColor="Red"/>
    </form>
</body>
</html>
