<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TrDNCust.aspx.vb" Inherits="TrDNCust" %>
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
        var PpnRate = document.getElementById("tbPpnRate").value.replace(/\$|\,/g,"");
        var Rate = document.getElementById("tbRate").value.replace(/\$|\,/g,"");
        var PPn = document.getElementById("tbPPN").value.replace(/\$|\,/g,"");        
        var BaseForex = document.getElementById("tbBaseForex").value.replace(/\$|\,/g,"");
        var Disc = document.getElementById("tbDisc").value.replace(/\$|\,/g,"");
        var DiscForex = document.getElementById("tbDiscForex").value.replace(/\$|\,/g,"");
        var PPnForex = document.getElementById("tbPPNForex").value.replace(/\$|\,/g,"");
        var TotalForex = document.getElementById("tbTotalForex").value.replace(/\$|\,/g,"");
        
        document.getElementById("tbPpnRate").value = setdigit(PpnRate,'<%=ViewState("DigitRate")%>');
        document.getElementById("tbRate").value = setdigit(Rate,'<%=ViewState("DigitRate")%>');
        document.getElementById("tbPPN").value = setdigit(PPn,'<%=ViewState("DigitPercent")%>');
        document.getElementById("tbBaseForex").value = setdigit(BaseForex,'<%=VIEWSTATE("DigitCurr")%>');
        document.getElementById("tbDisc").value = setdigit(Disc,'<%=ViewState("DigitPercent")%>');
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
         var Qty = document.getElementById("tbQty").value.replace(/\$|\,/g,""); 
         var PriceForex = document.getElementById("tbPrice").value.replace(/\$|\,/g,"");
         var AmountForex = document.getElementById("tbAmountForex").value.replace(/\$|\,/g,"");        
        
         document.getElementById("tbQty").value = setdigit(Qty,'<%=ViewState("DigitQty")%>');
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
    <div class="H1">Debit Note Customer</div>
     <hr style="color:Blue" />        
     <asp:Panel runat="server" id="PnlHd"> 
      <table>
        <tr>
            <td style="width:100px;text-align:right">Quick Search :</td>            
            <td><asp:TextBox CssClass="TextBox" runat="server" ID ="tbFilter"/> 
                  <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlField" >
                      <asp:ListItem Value="TransNmbr" Selected="True">Reference</asp:ListItem>
                      <asp:ListItem>Status</asp:ListItem>
                      <asp:ListItem Value="dbo.FormatDate(TransDate)">Date</asp:ListItem>
                      <asp:ListItem Value="Customer_Name">Customer</asp:ListItem>
                      <asp:ListItem Value="Term_Name">Term</asp:ListItem>
                      <asp:ListItem Value="dbo.FormatDate(DueDate)">Due Date</asp:ListItem>
                      <asp:ListItem Value="Currency">Currency</asp:ListItem>
                      <asp:ListItem>Remark</asp:ListItem>
                      <asp:ListItem Value="PPnNo">PPN NO</asp:ListItem>
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
              <asp:DropDownList ID="ddlField2" runat="server" CssClass="DropDownList">
                  <asp:ListItem Selected="True" Value="TransNmbr">Reference</asp:ListItem>
                  <asp:ListItem>Status</asp:ListItem>
                  <asp:ListItem Value="dbo.FormatDate(TransDate)">Date</asp:ListItem>
                      <asp:ListItem Value="Customer_Name">Customer</asp:ListItem>
                      <asp:ListItem Value="Term_Name">Term</asp:ListItem>
                      <asp:ListItem Value="dbo.FormatDate(DueDate)">Due Date</asp:ListItem>
                      <asp:ListItem Value="Currency">Currency</asp:ListItem>
                      <asp:ListItem>Remark</asp:ListItem>
                      <asp:ListItem Value="PPnNo">PPN NO</asp:ListItem>
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
                              <asp:ListItem Text="Print" />
                          </asp:DropDownList>
                          
                          <asp:Button class="btngo" runat="server" ID="BtnGo" Text="G" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" CommandName="Go" />
                          
                      </ItemTemplate>
                      <HeaderStyle Width="110px" />
                  </asp:TemplateField>
                  <asp:BoundField DataField="TransNmbr" HeaderStyle-Width="120px" SortExpression="Nmbr" HeaderText="Reference"></asp:BoundField>                  
                  <asp:BoundField DataField="Status" HeaderText="Status"></asp:BoundField>                  
                  <asp:BoundField DataField="TransDate" dataformatstring="{0:dd MMM yyyy}" htmlencode="true" HeaderStyle-Width="80px" SortExpression="TransDate" HeaderText="Date"></asp:BoundField>
                  <asp:BoundField DataField="Customer_Name" HeaderStyle-Width="150px" SortExpression="Customer_Name" HeaderText="Customer"></asp:BoundField>
                  <asp:BoundField DataField="Term_Name" HeaderStyle-Width="100px" SortExpression="Term_Name" HeaderText="Term"></asp:BoundField>
                  <asp:BoundField DataField="DueDate" dataformatstring="{0:dd MMM yyyy}" htmlencode="true" HeaderStyle-Width="80px" SortExpression="DueDate" HeaderText="Due Date"></asp:BoundField>
                  <asp:BoundField DataField="PPNNo" HeaderStyle-Width="120px" SortExpression="PPNNo" HeaderText="PPN NO"></asp:BoundField>
                  <asp:BoundField DataField="PPNDate" dataformatstring="{0:dd MMM yyyy}" htmlencode="true" HeaderStyle-Width="80px" SortExpression="PPNDate" HeaderText="PPN Date"></asp:BoundField>                  
                  <asp:BoundField DataField="PPNRate" DataFormatString="{0:#,##0.00}" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="80px" SortExpression="PPNRate" HeaderText="PPN Rate"></asp:BoundField>
                  <asp:BoundField DataField="Currency" HeaderStyle-Width="10px" SortExpression="Currency" HeaderText="Curr"></asp:BoundField>
                  <asp:BoundField DataField="BaseForex" DataFormatString="{0:#,##0.00}" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="80px" SortExpression="BaseForex" HeaderText="Base Forex"></asp:BoundField>
                  <asp:BoundField DataField="DiscForex" DataFormatString="{0:#,##0.00}" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="80px" SortExpression="DiscForex" HeaderText="Disc Forex"></asp:BoundField>
                  <asp:BoundField DataField="PPnForex" DataFormatString="{0:#,##0.00}" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="80px" SortExpression="PPnForex" HeaderText="PPn Forex"></asp:BoundField>
                  <asp:BoundField DataField="TotalForex" DataFormatString="{0:#,##0.00}" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="80px" SortExpression="TotalForex" HeaderText="Total Forex"></asp:BoundField>                  
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
            <td>Reference No</td>
            <td>:</td>
            <td><asp:TextBox CssClass="TextBoxR" runat="server" ID="tbRef" Width="150px" Enabled="False"/>             
                <%--&nbsp &nbsp Report : &nbsp
                <asp:DropDownList AutoPostBack="true" ValidationGroup="Input" CssClass="DropDownList" ID="ddlReport" runat="server" >
                   <asp:ListItem Selected="True">Y</asp:ListItem>
                   <asp:ListItem>N</asp:ListItem>
                </asp:DropDownList>--%> 
                &nbsp &nbsp Date : &nbsp
                <BDP:BasicDatePicker ID="tbDate" runat="server" DateFormat="dd MMM yyyy" 
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
            <td>Cost Center</td>
            <td>:</td>
            <td>
                    <asp:DropDownList ID="ddlCostCtr" runat="server" CssClass="DropDownList" >                        
                    </asp:DropDownList>
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
                        ShowNoneButton="False" ValidationGroup="Input">
                        <TextBoxStyle CssClass="TextDate" />
                </BDP:BasicDatePicker>
            </td>                    
        </tr>          
        <tr>
            <td>PPn</td>
            <td>:</td>
            <td>
                <table>
                <tr style="background-color:Silver;text-align:center">
                   <td>PPn No</td>
                   <td>PPn %</td>
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
                </td>
        </tr>  
        <tr>
                <td>Total</td>
                <td>:</td>
                <td>
                    <table>
                        <tr style="background-color:Silver;text-align:center">
                            <td>Base Forex</td>
                            <td>Disc %</td>
                            <td>Disc Forex</td>
                            <td>PPN %</td>
                            <td>PPN Forex</td>
                            <td>Total Forex</td>
                        </tr>
                        <tr>
                            <td><asp:TextBox ID="tbBaseForex" runat="server" CssClass="TextBoxR" Width="90px"/></td>
                            <td><asp:TextBox ID="tbDisc" runat="server" CssClass="TextBox" width="40px" 
                                    ValidationGroup="Input"/></td>
                            <td><asp:TextBox ID="tbDiscForex" ValidationGroup="Input" runat="server" CssClass="TextBox" Width="90px"/></td>
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
            <td><asp:TextBox runat="server" ValidationGroup="Input" ID="tbRemark" CssClass="TextBoxMulti" MaxLength="255" Width="400px" TextMode="MultiLine"/></td>
        </tr>
      </table>  
      
      <br />      
      <div style="font-size:medium; color:Blue;">Detail</div>
      <hr style="color:Blue" />  
        <asp:Panel runat="server" ID="pnlDt">
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
                            <%--<FooterTemplate>
                            <asp:ImageButton ID="btnAddDt2" ValidationGroup="Input" runat="server"  
                                    ImageUrl="../../Image/btnAddDtOn.png"
                                    onmouseover="this.src='../../Image/btnAddDtOff.png';"
                                    onmouseout="this.src='../../Image/btnAddDtOn.png';"
                                    ImageAlign="AbsBottom" />        
                            </FooterTemplate>--%>                            
                        </asp:TemplateField>
                        <asp:BoundField DataField="ItemNo" HeaderText="No" />
                            <asp:BoundField DataField="Account" HeaderText="Account" />
                            <asp:BoundField DataField="AccountName" HeaderStyle-Width="180px" HeaderText="Account Name" />
                            <asp:BoundField DataField="Subled" HeaderStyle-Width="100px" HeaderText="Subled" />
                            <asp:BoundField DataField="SubledName" HeaderStyle-Width="180px" HeaderText="Subled Name" />
                            <asp:BoundField DataField="Cost_Ctr_Name" HeaderStyle-Width="120px" HeaderText="Cost Center" />                            
                            <asp:BoundField DataField="Qty"  DataFormatString="{0:#,##0.00}" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="80px" HeaderText="Qty" />
                            <asp:BoundField DataField="Unit" HeaderStyle-Width="80px" HeaderText="Unit" />
                            <asp:BoundField DataField="PriceForex"  DataFormatString="{0:#,##0.00}" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="80px" HeaderText="Price Forex" />
                            <asp:BoundField DataField="AmountForex"  DataFormatString="{0:#,##0.00}" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="80px" HeaderText="Amount Forex" />
                            <asp:BoundField DataField="Remark" HeaderStyle-Width="200px" HeaderText="Remark" />
                    </Columns>
                </asp:GridView>
          </div>   
          <asp:Button class="bitbtn btnadd" runat="server" ID="btnAddDt2" Text="Add" ValidationGroup="Input" />	
          
                
       </asp:Panel>       
       <asp:Panel runat="server" ID="pnlEditDt" Visible="false">
            <table>                                
                
                <tr>
                   <td>Item No</td>
                   <td>:</td>
                   <td><asp:Label ID="lbItemNo" runat="server" Text="Item" /></td>           
                </tr>        
                <tr>                    
                   <td>Account</td>
                   <td>:</td>
                   <td><asp:TextBox CssClass="TextBox" ValidationGroup="Input" runat="server" ID="tbAccount" AutoPostBack="true" />
                       <asp:TextBox CssClass="TextBox" runat="server" ID="tbAccountName" Enabled="false" Width="225px"/>
                       <asp:Button Class="btngo" ID="btnAccount" Text="..." runat="server" ValidationGroup="Input" />                                          
                   </td>
                </tr>
                <tr>
                   <td>Subled</td>
                   <td>:</td>
                   <td><asp:TextBox runat="server" ID="tbFgSubled" visible="false" />
                       <asp:TextBox runat="server" ID="tbFgCostCtr" Visible="false" />                              
                       <asp:TextBox CssClass="TextBox" ValidationGroup="Input" runat="server" ID="tbSubled" AutoPostBack="true" />
                       <asp:TextBox CssClass="TextBox" runat="server" ID="tbSubledName" Enabled="false" Width="225px"/>
                       <asp:Button Class="btngo" ID="btnSubled" Text="..." runat="server" ValidationGroup="Input" />                                          
                   </td>
                 </tr>
                 <tr>
                    <td>Cost Center</td>
                    <td>:</td>
                    <td><asp:DropDownList CssClass="DropDownList" ID="ddlCostCenter" runat="server" Width="200px"/></td>
                 </tr>  
                 <tr>
                     <td>Qty</td>
                     <td>:</td>
                     <td><asp:TextBox CssClass="TextBox" runat="server" ID="tbQty" /> 
                         <asp:DropDownList CssClass="DropDownList" ID="ddlUnit" runat="server"/>
                     </td>                    
                 </tr>                
                 <tr>
                     <td>Price</td>
                     <td>:</td>
                     <td><asp:TextBox CssClass="TextBox" runat="server" ID="tbPrice" /> </td>
                 </tr>                     
                 <tr> 
                     <td>Amount</td>
                     <td>:</td>
                     <td><asp:TextBox CssClass="TextBoxR" runat="server" ID="tbAmountForex"/></td>                        
                 </tr>
                 <tr>
                     <td>Remark </td>
                     <td>:</td>
                     <td><asp:TextBox runat="server" ID="tbRemarkDt" CssClass="TextBoxMulti" Width="365px" MaxLength="255" TextMode="MultiLine" /></td>
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
