<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TrSuppRetur.aspx.vb" Inherits="SuppRetur" %>
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
        
        document.getElementById("tbRate").value = setdigit(Rate,'<%=ViewState("DigitRate")%>');
        document.getElementById("tbPPN").value = setdigit(PPn,'<%=ViewState("DigitPercent")%>');
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
        var PriceForex = document.getElementById("tbPrice").value.replace(/\$|\,/g,"");
        var AmountForex = document.getElementById("tbAmountForex").value.replace(/\$|\,/g,"");        
        document.getElementById("tbQty").value = setdigit(Qty,'<%=ViewState("DigitQty")%>');
        document.getElementById("tbQtyWrhs").value = setdigit(QtyWrhs,'<%=ViewState("DigitQty")%>');
        document.getElementById("tbPrice").value = setdigit(PriceForex,'<%=VIEWSTATE("DigitCurr")%>');
        document.getElementById("tbAmountForex").value = setdigit(AmountForex,'<%=VIEWSTATE("DigitCurr")%>');
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
    <div class="H1">Supplier Retur</div>
     <hr style="color:Blue" />        
    <asp:Panel runat="server" id="PnlHd"> 
      <table>
        <tr>
            <td style="width:100px;text-align:right">Quick Search :</td>            
            <td><asp:TextBox CssClass="TextBox" runat="server" ID ="tbFilter"/> 
                  <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlField" >
                      <asp:ListItem Value="TransNmbr" Selected="True">Reference</asp:ListItem>
                      <asp:ListItem>Status</asp:ListItem>
                      <asp:ListItem Value="SJNo">SJ No</asp:ListItem>
                      <asp:ListItem Value="Trans_Date">Date</asp:ListItem>
                      <asp:ListItem Value="Supplier">Supplier</asp:ListItem>
                      <asp:ListItem Value="PurchaseReject">Purchase Reject</asp:ListItem>
                      <asp:ListItem Value="Term_Name">Term</asp:ListItem>
                      <asp:ListItem Value="Currency">Currency</asp:ListItem>
                      <asp:ListItem Value="BaseForex">Base Forex</asp:ListItem>
                      <asp:ListItem Value="PPnForex">PPn Forex</asp:ListItem>
                      <asp:ListItem Value="TotalForex">Total Forex</asp:ListItem>
                      <asp:ListItem Value="Remark">Remark</asp:ListItem>
                  </asp:DropDownList>
                  <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlRange"></asp:DropDownList>
                  <asp:Button class="bitbtn btnsearch" runat="server" ID="btnSearch" Text="Search" />
                  <asp:Button class="btngo" runat="server" ID="btnExpand" Text="..."/>                  
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
                      <asp:ListItem>Status</asp:ListItem>
                      <asp:ListItem Value="SJNo">SJ No</asp:ListItem>
                      <asp:ListItem Value="Trans_Date">Nota Date</asp:ListItem>
                      <asp:ListItem Value="Supplier">Supplier</asp:ListItem>
                      <asp:ListItem Value="PurchaseReject">Purchase Reject</asp:ListItem>
                      <asp:ListItem Value="Term_Name">Term</asp:ListItem>
                      <asp:ListItem Value="Currency">Currency</asp:ListItem>
                      <asp:ListItem Value="BaseForex">Base Forex</asp:ListItem>
                      <asp:ListItem Value="PPnForex">PPn Forex</asp:ListItem>
                      <asp:ListItem Value="TotalForex">Total Forex</asp:ListItem>
                      <asp:ListItem Value="Remark">Remark</asp:ListItem>
              </asp:DropDownList>
          </td>              
        </tr>        
      </table>      
      </asp:Panel>
          <br />
            <asp:Button class="bitbtn btnadd" runat="server" ID="BtnAdd" Text="Add"/>     
            &nbsp &nbsp &nbsp   
            <asp:DropDownList CssClass="DropDownList" ID="ddlCommand" runat="server" Visible="false"/>
            <asp:Button class="btngo" runat="server" ID="BtnGo" Text="G" Visible="false" />                 
          <br />
          <div style="border:0px  solid; width:100%; height:100%; overflow:auto;">
          <asp:GridView ID="GridView1" runat="server" AllowPaging="True" AllowSorting="true"
            CssClass="Grid" AutoGenerateColumns="false"> 
              <HeaderStyle CssClass="GridHeader" Wrap="false"></HeaderStyle>
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
                          </asp:DropDownList>
                          <asp:Button class="btngo" runat="server" ID="BtnGo" Text="G" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" CommandName="Go" />     
                      </ItemTemplate>
                      <HeaderStyle Width="110px" />
                  </asp:TemplateField>
                  <asp:BoundField DataField="TransNmbr" HeaderStyle-Width="120px" SortExpression="Nmbr" HeaderText="Reference"></asp:BoundField>                  
                  <asp:BoundField DataField="Status" HeaderText="Status" SortExpression="Status"></asp:BoundField>                  
                  <asp:BoundField DataField="Trans_Date" HeaderStyle-Width="80px" SortExpression="TransDate" HeaderText="Date"></asp:BoundField>
                  <asp:BoundField DataField="Supplier" HeaderStyle-Width="200px" SortExpression="Supplier" HeaderText="Supplier"></asp:BoundField>
                  <asp:BoundField DataField="PurchaseReject" HeaderStyle-Width="120px" SortExpression="PurchaseReject" HeaderText="Purchase Reject"></asp:BoundField>
                  <asp:BoundField DataField="Term_Name" HeaderStyle-Width="150px" SortExpression="Term_Name" HeaderText="Term"></asp:BoundField>
                  <asp:BoundField DataField="Currency" HeaderStyle-Width="50px" SortExpression="Currency" HeaderText="Currency"></asp:BoundField>
                  <asp:BoundField DataField="BaseForex" HeaderStyle-Width="80px" SortExpression="BaseForex" HeaderText="Base Forex" ItemStyle-HorizontalAlign="Right"></asp:BoundField>
                  <asp:BoundField DataField="PPnForex" HeaderStyle-Width="80px" SortExpression="PPnForex" HeaderText="PPn Forex" ItemStyle-HorizontalAlign="Right"></asp:BoundField>
                  <asp:BoundField DataField="TotalForex" HeaderStyle-Width="80px" SortExpression="TotalForex" HeaderText="Total Forex" ItemStyle-HorizontalAlign="Right"></asp:BoundField>
                  <asp:BoundField DataField="Remark" HeaderStyle-Width="250px" SortExpression="TotalForex" HeaderText="Remark"></asp:BoundField>
              </Columns>
          </asp:GridView>
          </div>
            <asp:Panel runat="server" ID ="pnlNav" Visible="false">
            <asp:Button class="bitbtn btnadd" runat="server" ID="btnAdd2" Text="Add"/>     
            &nbsp &nbsp &nbsp
            <asp:DropDownList ID="ddlCommand2" CssClass="DropDownList" runat="server"/>
            <asp:Button class="btngo" runat="server" ID="btnGo2" Text="G"/>     
            </asp:Panel>
    </asp:Panel>    
    <asp:Panel runat="server" ID="pnlInput" Visible="false">
      <table>
        <tr>
            <td>Nota No</td>
            <td>:</td>
            <td><asp:TextBox CssClass="TextBoxR" runat="server" ID="tbRef" Width="150px" Enabled="False"/>             
                <%--<asp:Label runat="server" Text="   Report : "></asp:Label>
                <asp:DropDownList AutoPostBack="true" Enabled="false" CssClass="DropDownList" ID="ddlReport" runat="server" >
                <asp:ListItem Selected="True">Y</asp:ListItem>
                <asp:ListItem>N</asp:ListItem>
                </asp:DropDownList>--%> 
            </td>            
        </tr>
        <tr>
            <td>Nota Date</td>
            <td>:</td>
            <td><BDP:BasicDatePicker ID="tbDate" runat="server" DateFormat="dd MMM yyyy" 
                        ReadOnly = "true" ValidationGroup="Input"
                        ButtonImageHeight="19px" ButtonImageWidth="20px" 
                        DisplayType="TextBoxAndImage" 
                        TextBoxStyle-CssClass="TextDate" AutoPostBack="True" 
                        ShowNoneButton="False">
                        <TextBoxStyle CssClass="TextDate" />
                </BDP:BasicDatePicker>                
            </td>            
        </tr>         
        <tr>
            <td><asp:LinkButton ID="lbSupp" ValidationGroup="Input"  runat="server" Text="Supplier"/></td>
            <td>:</td>
            <td><asp:TextBox CssClass="TextBox" ValidationGroup="Input" runat="server" ID="tbSuppCode" AutoPostBack="true" />
                <asp:TextBox CssClass="TextBox" runat="server" ID="tbSuppName" Enabled="false" Width="225px"/>
                <asp:Button class="btngo" runat="server" ID="btnSupp" Text="..." ValidationGroup="Input"/>     
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
                        Enabled ="false" ReadOnly = "true" 
                        ButtonImageHeight="19px" ButtonImageWidth="20px" 
                        DisplayType="TextBox" 
                        TextBoxStyle-CssClass="TextDate" AutoPostBack="True" 
                        ShowNoneButton="False">
                        <TextBoxStyle CssClass="TextDate" />
                </BDP:BasicDatePicker>
            </td>                    
        </tr>  
        <tr>
            <td>Purchase Reject</td>
            <td>:</td>
            <td><asp:TextBox runat="server" ID="tbPONo" CssClass="TextBox" Width="150px" Enabled ="false"/> &nbsp &nbsp &nbsp &nbsp &nbsp &nbsp &nbsp
                <asp:Button class="btngo" runat="server" ID="btnGetData" Text="Get Data" 
                    ValidationGroup="Input" Width="57px"/>     
            </td>
        </tr>
        <tr>
            <td>PPn</td>
            <td>:</td>
            <td>
                <table>
                <tr style="background-color:Silver;text-align:center">
                    <td>PPn No</td>
                    <td>PPn Date</td>
                    <td>PPn Rate</td>
                 </tr>
                 <tr>
                    <td><asp:TextBox runat="server" ValidationGroup="Input" ID="tbPPnNo" CssClass="TextBox" Width="150px"/>
                    </td>
                    <td><BDP:BasicDatePicker ID="tbPPndate" runat="server" DateFormat="dd MMM yyyy" 
                            ReadOnly = "true" ValidationGroup="Input"
                            ButtonImageHeight="19px" ButtonImageWidth="20px" 
                            DisplayType="TextBoxAndImage" 
                            TextBoxStyle-CssClass="TextDate" AutoPostBack="True" ShowNoneButton="False">
                            <TextBoxStyle CssClass="TextDate" />
                        </BDP:BasicDatePicker>                
                    </td>
                    <td><asp:TextBox runat="server" ValidationGroup="Input" ID="tbPpnRate" CssClass="TextBox" Width="68px"/>
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
                </td>
        </tr>  
        <tr>
                <td>Total</td>
                <td>:</td>
                <td>
                    <table>
                        <tr style="background-color:Silver;text-align:center">
                            <td>Base Forex</td>
                            <td>PPN %</td>
                            <td>PPN Forex</td>
                            <td>Total Forex</td>
                        </tr>
                        <tr>
                            <td><asp:TextBox ID="tbBaseForex" runat="server" CssClass="TextBoxR" Width="90px"/></td>
                            <td><asp:TextBox ID="tbPPN" ValidationGroup="Input" runat="server" CssClass="TextBox" width="40px"/></td>
                            <td><asp:TextBox ID="tbPPNForex" runat="server" CssClass="TextBoxR" Width="90px"/></td>
                            <td><asp:TextBox ID="tbTotalForex" runat="server" CssClass="TextBoxR" Width="90px"/></td>
                        </tr>
                    </table>
                </td>                
             </tr>   
        <tr>
            <td>Remark</td>
            <td>:</td>
            <td><asp:TextBox runat="server" ValidationGroup="Input" ID="tbRemark" CssClass="TextBox" Width="325px"/></td>
        </tr>
      </table>  
      
      <br />      
      <div style="font-size:medium; color:Blue;">Detail</div>
      <hr style="color:Blue" />  
        <asp:Panel runat="server" ID="pnlDt">
            <asp:Button class="bitbtndt btnadd" runat="server" ID="btnAddDt" Text="Add" Visible ="false"/>     
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
                                <asp:Button class="bitbtndt btnedit" runat="server" ID="btnEdit" Text="Edit" CommandName="Edit"/>     
                                <asp:Button class="bitbtndt btndelete" runat="server" ID="btnDelete" Text="Delete" CommandName="Delete"/>                                     
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:Button class="bitbtndt btnsave" runat="server" ID="btnUpdate" Text="Save" CommandName="Update"/>     
                                <asp:Button class="bitbtndt btncancel" runat="server" ID="btnCancel" Text="Cancel" CommandName="Cancel"/>     
                            </EditItemTemplate>
                            <%--<FooterTemplate>
                            <asp:ImageButton ID="btnAddDt2" ValidationGroup="Input" runat="server"  
                                    ImageUrl="../../Image/btnAddDtOn.png"
                                    onmouseover="this.src='../../Image/btnAddDtOff.png';"
                                    onmouseout="this.src='../../Image/btnAddDtOn.png';"
                                    ImageAlign="AbsBottom" />        
                            </FooterTemplate>--%>                            
                        </asp:TemplateField>
                        <asp:BoundField DataField="SJNo" HeaderStyle-Width="120px" HeaderText="SJ No" />
                        <asp:BoundField DataField="Product" HeaderStyle-Width="80px" HeaderText="Product Code" />
                        <asp:BoundField DataField="Product_Name" HeaderStyle-Width="250px"  HeaderText="Product Name" />
                        <asp:BoundField DataField="QtyOrder" HeaderStyle-Width="100px" HeaderText="Qty" ItemStyle-HorizontalAlign="Right"/>
                        <asp:BoundField DataField="UnitOrder" HeaderStyle-Width="80px" HeaderText="Unit" />                        
                        <asp:BoundField DataField="PriceForex" HeaderStyle-Width="80px" HeaderText="Price" ItemStyle-HorizontalAlign="Right"/>
                        <asp:BoundField DataField="AmountForex" HeaderStyle-Width="80px" HeaderText="Amount Forex" ItemStyle-HorizontalAlign="Right"/>                        
                    </Columns>
                </asp:GridView>
          </div>   
          <asp:Button class="bitbtndt btnadd" runat="server" ID="btnAddDt2" Text="Add" Visible ="false" />                                          
       </asp:Panel>       
       <asp:Panel runat="server" ID="pnlEditDt" Visible="false">
            <table>                
                <tr>
                    <td>SJ No</td>
                    <td>:</td>
                    <td><asp:TextBox CssClass="TextBox" runat="server" ID="tbRRNo" Enabled="false" />
                    </td>
                </tr>                
                <tr>
                    <td>Product</td>
                    <td>:</td>
                    <td><asp:TextBox runat="server" ID="tbProductCode" CssClass="TextBox" Enabled="false" />
                        <asp:TextBox runat="server"  CssClass="TextBox" ID="tbProductName" Enabled="false" Width="200px"/>                         
                    </td>                               
                </tr>                                                    
                <tr>
                    <td>Qty Order</td>
                    <td>:</td>
                    <td><asp:TextBox CssClass="TextBox" runat="server" ID="tbQty" Enabled="false" />
                        <asp:TeXtBox ID="tbUnit" runat="server" CssClass="TextBox" Enabled="false" />
                    </td>
                </tr>                
                <tr>
                    <td>Qty Warehouse</td>
                    <td>:</td>
                    <td><asp:TextBox CssClass="TextBox" runat="server" ID="tbQtyWrhs" Enabled="false" />
                        <asp:TextBox CssClass="TextBox" runat="server" ID="tbUnitWrhs" Enabled="false" />
                    </td>
                </tr>
                <tr>
                    <td>Price </td>
                    <td>:</td>
                    <td><asp:TextBox runat="server" CssClass="TextBox" ID="tbPrice" /></td>
                </tr>              
                <tr>
                    <td>Amount</td>
                    <td>:</td>
                    <td><asp:TextBox runat="server" ID="tbAmountForex" CssClass="TextBoxR" Width="80px"/>
                    </td>
                </tr>  
                <tr>
                    <td>Remark </td>
                    <td>:</td>
                    <td><asp:TextBox runat="server" CssClass="TextBox" ID="tbRemarkDt" Width="320px" /></td>
                </tr>                              
            </table>
            <br />      
            <asp:Button class="bitbtndt btnsave" runat="server" ID="btnSaveDt" Text="Save"/>                                          
            <asp:Button class="bitbtndt btncancel" runat="server" ID="btnCancelDt" Text="Cancel"/>                                                      
       </asp:Panel> 
       <br />
       <asp:Button class="bitbtn btnsavenew" runat="server" ID="btnSaveAll" 
            Text="Save & New" ValidationGroup="Input" Width="95px"/>                                          
       <asp:Button class="bitbtn btnsave" runat="server" ID="btnSaveTrans" Text="Save" ValidationGroup="Input"/>                                          
       <asp:Button class="bitbtn btnback" runat="server" ID="btnBack" Text="Cancel" ValidationGroup="Input"/>                                          
       <asp:Button class="btngo" runat="server" ID="btnHome" Text="Home" Width="41px"/>                                          
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
