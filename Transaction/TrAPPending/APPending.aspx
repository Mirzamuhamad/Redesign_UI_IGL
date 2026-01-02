<%@ Page Language="VB" AutoEventWireup="false" CodeFile="APPending.Aspx.vb" Inherits="APPending" %>
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
        // var PriceForex = document.getElementById("tbPrice").value.replace(/\$|\,/g,"");
        // var AmountForex = document.getElementById("tbAmountForex").value.replace(/\$|\,/g,"");        
        
        document.getElementById("tbRate").value = setdigit(Rate,'<%=ViewState("DigitRate")%>');
        document.getElementById("tbPPN").value = setdigit(PPn,'<%=ViewState("DigitPercent")%>');
        document.getElementById("tbBaseForex").value = setdigit(BaseForex,'<%=VIEWSTATE("DigitCurr")%>');
        document.getElementById("tbPPNForex").value = setdigit(PPnForex,'<%=VIEWSTATE("DigitCurr")%>');
        document.getElementById("tbTotalForex").value = setdigit(TotalForex,'<%=VIEWSTATE("DigitCurr")%>');
        //document.getElementById("tbPrice").value = setdigit(Price,'<%=VIEWSTATE("DigitCurr")%>');
        //document.getElementById("tbAmountForex").value = setdigit(AmountForex,'<%=VIEWSTATE("DigitCurr")%>');
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
 <link href="https://fonts.googleapis.com/css2?family=Muli:wght@300;400;600&display=swap" rel="stylesheet">
</head>
<body>     
    <form id="form1" runat="server">    
    <div class="Content">
    <div class="H1"><asp:Label runat="server" ID="lblTitle"></asp:Label></div>
     <hr style="color:Blue" />        
    <asp:Panel runat="server" id="PnlHd"> 
      <table>
        <tr>
            <td style="width:100px;text-align:right">Quick Search :</td>            
            <td><asp:TextBox CssClass="TextBox" runat="server" ID ="tbFilter"/> 
                  <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlField" >
                      <asp:ListItem Value="TransNmbr" Selected="True">Reference</asp:ListItem>
                      <asp:ListItem Value="dbo.FormatDate(TransDate)">Date</asp:ListItem>
                      <asp:ListItem Value="Supplier">Supplier</asp:ListItem>
                      <asp:ListItem>Status</asp:ListItem>
                      <asp:ListItem Value="Wrhs_Name">Warehouse</asp:ListItem>
                      <asp:ListItem>Remark</asp:ListItem>
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
                  <asp:ListItem Value="TransNmbr" Selected="True">Reference</asp:ListItem>
                  <asp:ListItem Value="dbo.FormatDate(TransDate)">Date</asp:ListItem>
                  <asp:ListItem Value="Supplier">Supplier</asp:ListItem>
                  <asp:ListItem>Status</asp:ListItem>
                  <asp:ListItem Value="Wrhs_Name">Warehouse</asp:ListItem>
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
                  <asp:TemplateField>
                      <ItemTemplate>
                          <asp:DropDownList CssClass="DropDownList" ID="ddl" runat="server">
                              <asp:ListItem Selected="True" Text="View" />
                              <asp:ListItem Text="Edit" />                              
                          </asp:DropDownList>
                          <asp:Button class="btngo" runat="server" ID="BtnGo" Text="G" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" CommandName="Go" />                         
                      </ItemTemplate>                      
                  </asp:TemplateField>
                  <asp:BoundField DataField="TransNmbr" HeaderStyle-Width="120px" SortExpression="Nmbr" HeaderText="Reference"></asp:BoundField>                  
                  <asp:BoundField DataField="Status" HeaderText="Status"></asp:BoundField>                  
                  <asp:BoundField DataField="Trans_Date" HeaderStyle-Width="80px" SortExpression="TransDate" HeaderText="Date"></asp:BoundField>
                  <asp:BoundField DataField="Supplier" HeaderStyle-Width="150px" SortExpression="Supplier" HeaderText="Supplier"></asp:BoundField>                  
                  <asp:BoundField DataField="Wrhs_Name" HeaderStyle-Width="150px" SortExpression="Wrhs_Name" HeaderText="Warehouse"></asp:BoundField>                  
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
            <td>RR No</td>
            <td>:</td>
            <td><asp:TextBox CssClass="TextBoxR" runat="server" ID="tbRef" Width="150px" Enabled="False"/>             
                &nbsp Report : &nbsp
                <asp:DropDownList AutoPostBack="true" ValidationGroup="Input" CssClass="DropDownList" ID="ddlReport" runat="server" >
                        <asp:ListItem Selected="True">Y</asp:ListItem>
                        <asp:ListItem>N</asp:ListItem>
                </asp:DropDownList> 
            </td>            
            <td>&nbsp &nbsp &nbsp </td>                
            <td>RR Date</td>
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
            <td><asp:LinkButton ID="lbSupp" ValidationGroup="Input"  runat="server" Text="Supplier"/></td>
            <td>:</td>
            <td colspan="5">
                <asp:TextBox CssClass="TextBox" ValidationGroup="Input" runat="server" ID="tbSuppCode" AutoPostBack="true" />
                <asp:TextBox CssClass="TextBox" runat="server" ID="tbSuppName" Enabled="false" Width="225px"/>
                <asp:Button Class="btngo" ID="btnSupp" Text="..." runat="server" ValidationGroup="Input" />                                                  
                <asp:Label ID="Label6" runat="server" ForeColor="Red">*</asp:Label>
            </td>
        </tr>
        <tr>
            <td>Attn</td>
            <td>:</td>
            <td><asp:TextBox runat="server" ValidationGroup="Input" ID="tbAttn" CssClass="TextBox" Width="225px"/></td>
            <td></td>
            <td>PO No</td>
            <td>:</td>
            <td><asp:TextBox runat="server" ValidationGroup="Input" ID="tbPONo" CssClass="TextBox" Width="150px"/></td>
        </tr>
        <tr>
            <td><asp:LinkButton ID="lbWarehouse" ValidationGroup="Input" runat="server" Text="Warehouse"/></td>
            <td>:</td>
            <td colspan="5"><asp:TextBox CssClass="TextBox" ValidationGroup="Input" runat="server" ID="tbWrhsCode" AutoPostBack="true" /> 
                <asp:TextBox CssClass="TextBox" runat="server" ID="tbWrhsName" enabled="false" Width="225px"/>
                <asp:Button Class="btngo" ID="btnWrhs" Text="..." runat="server" ValidationGroup="Input" />                                                  
                <asp:Label ID="Label1" runat="server" ForeColor="Red">*</asp:Label>
            </td>                    
        </tr>          
        <tr>
            <td>SJ Supplier No</td>
            <td>:</td>
            <td><asp:TextBox runat="server" ValidationGroup="Input" ID="tbSJSuppNo" CssClass="TextBox" Width="150px"/></td>
            <td></td>
            <td>SJ Supplier Date</td>
            <td>:</td>
            <td>
            <BDP:BasicDatePicker ID="tbSJSuppDate" runat="server" DateFormat="dd MMM yyyy" 
                        ReadOnly = "true" ValidationGroup="Input"
                        ButtonImageHeight="19px" ButtonImageWidth="20px" 
                        DisplayType="TextBoxAndImage" 
                        TextBoxStyle-CssClass="TextDate" AutoPostBack="True" ShowNoneButton="False"><TextBoxStyle CssClass="TextDate" /></BDP:BasicDatePicker>                
            </td>            
        </tr> 
        
        
        <tr>
                <td><asp:LinkButton ID="lbCurr" ValidationGroup="Input" runat="server" Text="Currency"/></td>
                <td>:</td>
                <td><asp:DropDownList ID="ddlCurr" ValidationGroup="Input" AutoPostBack="true" runat="server" CssClass="DropDownList" />
                <asp:TextBox ID="tbRate" ValidationGroup="Input" runat="server" CssClass="TextBox" Width="48px" />
                </td>
                <td></td>
                <td>Car No</td>
                <td>:</td>
                <td><asp:TextBox runat="server" ValidationGroup="Input" ID="tbCarNo" CssClass="TextBox" Width="150px"/></td>        
        </tr>  
        <tr>
                <td>Total</td>
                <td>:</td>
                <td colspan="5">
                    <table>
                        <tr style="background-color:Silver;text-align:center">
                            <td>Base Forex</td>
                            <td>PPN %</td>
                            <td>PPN Forex</td>
                            <td>Total Forex</td>
                        </tr>
                        <tr>
                            <td><asp:TextBox ID="tbBaseForex" runat="server" CssClass="TextBoxR" /></td>
                            <td><asp:TextBox ID="tbPPN" ValidationGroup="Input" runat="server" CssClass="TextBox" Width="50px"/></td>
                            <td><asp:TextBox ID="tbPPNForex" runat="server" CssClass="TextBoxR"/></td>
                            <td><asp:TextBox ID="tbTotalForex" runat="server" CssClass="TextBoxR" /></td>
                        </tr>
                    </table>
                </td>                
             </tr>   
        <tr>
            <td>Remark</td>
            <td>:</td>
            <td colspan="5"><asp:TextBox runat="server" ValidationGroup="Input" ID="tbRemark" CssClass="TextBoxMulti" MaxLength="255" TextMode="MultiLine" Width="350px"/></td>
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
                        <asp:BoundField DataField="Product" HeaderStyle-Width="80px" HeaderText="Product Code" />
                        <asp:BoundField DataField="Product_Name" HeaderStyle-Width="250px"  HeaderText="Product Name" />
                        <asp:BoundField DataField="QtyOrder" HeaderStyle-Width="100px" HeaderText="Qty" />
                        <asp:BoundField DataField="UnitOrder" HeaderStyle-Width="80px" HeaderText="Unit" />                        
                        <%--<asp:BoundField DataField="Qty" HeaderStyle-Width="100px" HeaderText="Qty Wrhs" />--%>
                        <%--<asp:BoundField DataField="Unit" HeaderStyle-Width="80px" HeaderText="Unit Wrhs" />--%>
                        <asp:BoundField DataField="PriceForex" HeaderStyle-Width="80px" HeaderText="Price" />
                        <asp:BoundField DataField="AmountForex" HeaderStyle-Width="80px" HeaderText="Amount Forex" />
                        <asp:BoundField DataField="Remark" HeaderStyle-Width="250px" HeaderText="Remark" />
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
                        <asp:TextBox runat="server"  CssClass="TextBox"
                            ID="tbProductName" EnableTheming="True" Enabled="false" Width="300px"/> 
                        <asp:Button Class="btngo" ID="btnProduct" Text= "..." runat="server" 
                            Width="19px" />                                                                   
                        <asp:Label ID="Label2" runat="server" ForeColor="Red">*</asp:Label>
                    </td>                               
                </tr>                                                    
                <tr>
                    <td>Qty Order</td>
                    <td>:</td>
                    <td><asp:TextBox CssClass="TextBox" runat="server" ID="tbQty" AutoPostBack="True" />
                    <asp:DropDownList ID="ddlUnit" ValidationGroup="Input" AutoPostBack="true" runat="server" CssClass="DropDownList" Width="80px" />
                    <asp:Label ID="Label3" runat="server" ForeColor="Red">*</asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>Qty Warehouse</td>
                    <td>:</td>
                    <td><asp:TextBox CssClass="TextBoxR" runat="server" ID="tbQtyWrhs" />
                    <asp:TextBox CssClass="TextBox" runat="server" ID="tbUnitWrhs" Enabled="false" Width="80px"/>
                    <asp:Label ID="Label4" runat="server" ForeColor="Red">*</asp:Label>
                    </td>
                </tr>  
                      
                <tr>
                    <td>Price </td>
                    <td>:</td>
                    <td><asp:TextBox runat="server" CssClass="TextBox" ID="tbPrice"/></td>
                </tr>  
                <tr>
                    <td>Amount Forex </td>
                    <td>:</td>
                    <td><asp:TextBox runat="server" CssClass="TextBoxR" ID="tbAmountForex" /></td>
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
    </asp:Panel>
      <br />            
    </div>
   
    <asp:Label runat ="server" ID="lbStatus" ForeColor="Red"/>
   
    </form>
    </body>
</html>
