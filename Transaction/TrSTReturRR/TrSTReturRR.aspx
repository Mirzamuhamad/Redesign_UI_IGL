<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TrSTReturRR.aspx.vb" Inherits="TrSTReturRR" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="BasicFrame.WebControls.BasicDatePicker" Namespace="BasicFrame.WebControls"
    TagPrefix="BDP" %>
<%@ Register assembly="CrystalDecisions.Web, Version=10.5.3700.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Receiving Return Customer</title>
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
        var Qty = document.getElementById("tbQty").value.replace(/\$|\,/g,"");
        var QtyM2 = document.getElementById("tbQtyM2").value.replace(/\$|\,/g,"");
        var QtyRoll = document.getElementById("tbQtyRoll").value.replace(/\$|\,/g,"");
        
        if (Qty >= 0)
        document.getElementById("tbQty").value = setdigit(Qty,'<%=ViewState("DigitQty")%>');
        if (QtyM2 >= 0)
        document.getElementById("tbQtyM2").value = setdigit(QtyM2,'<%=ViewState("DigitQty")%>');
        if (QtyRoll >= 0)
        document.getElementById("tbQtyRoll").value = setdigit(QtyRoll,'<%=ViewState("DigitQty")%>');
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
    <div class="H1">Receiving Return Customer</div>
     <hr style="color:Blue" />        
    <asp:Panel runat="server" id="PnlHd"> 
      <table>
        <tr>
            <td style="width:100px;text-align:right">Quick Search :</td>            
            <td><asp:TextBox CssClass="TextBox" runat="server" ID ="tbFilter"/> 
                  <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlField" >
                      <asp:ListItem Value="TransNmbr" Selected="True">Reference</asp:ListItem>
                      <asp:ListItem Value="dbo.FormatDate(TransDate)">Date</asp:ListItem>
                      <asp:ListItem>Status</asp:ListItem>
                      <asp:ListItem Value="Customer_Name">Customer</asp:ListItem>
                      <asp:ListItem Value="Attn">Attention</asp:ListItem>
                      <asp:ListItem Value="RequestNo">Request No</asp:ListItem>
                      <asp:ListItem Value="Wrhs_Name">Warehouse</asp:ListItem>
                      <asp:ListItem Value="Subled_Name">Subled</asp:ListItem>
                      <asp:ListItem Value="ReceivedBy">Received By</asp:ListItem>
                      <asp:ListItem Value="SJCustNo">DN Customer No</asp:ListItem>
                      <asp:ListItem Value="dbo.FormatDate(SJCustDate)">DN Customer Date</asp:ListItem>
                      <asp:ListItem Value="FgReport">Report</asp:ListItem>                      
                      <asp:ListItem Value="Remark">Remark</asp:ListItem>
                    </asp:DropDownList>
                  <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlRange"></asp:DropDownList>
                  <asp:Button class="bitbtn btnsearch" runat="server" ID="btnSearch" Text="Search" />																						 											  
                  <asp:Button class="bitbtn btngo" runat="server" ID="btnExpand" Text="..." />																					 											  
            </td>
            <td>
                <asp:LinkButton ID="LbAdvSearch" runat="server" Text="Advanced Search" />
                &nbsp &nbsp &nbsp &nbsp
                <asp:Label runat="server" ID="Label1" Text="Request Gantung : "/>
                <asp:LinkButton runat="server" ID="lbCount" Text="X" ForeColor="#FF6600" Font-Size="Small" />
                <asp:Label runat="server" ID="Label2" Text=" record(s)"/>
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
                      <asp:ListItem>Status</asp:ListItem>
                      <asp:ListItem Value="Customer_Name">Customer</asp:ListItem>
                      <asp:ListItem Value="Attn">Attention</asp:ListItem>
                      <asp:ListItem Value="RequestNo">Request No</asp:ListItem>
                      <asp:ListItem Value="Wrhs_Name">Warehouse</asp:ListItem>
                      <asp:ListItem Value="Subled_Name">Subled</asp:ListItem>
                      <asp:ListItem Value="ReceivedBy">Received By</asp:ListItem>
                      <asp:ListItem Value="SJCustNo">DN Customer No</asp:ListItem>
                      <asp:ListItem Value="dbo.FormatDate(SJCustDate)">DN Customer Date</asp:ListItem>
                      <asp:ListItem Value="FgReport">Report</asp:ListItem>                      
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
            <asp:Button class="btngo" runat="server" ID="BtnGo" Text="G" Visible="false"/>                             
          <br />
          <div style="border:0px  solid; width:100%; height:100%; overflow:auto;">
          <asp:GridView ID="GridView1" runat="server" AllowPaging="True" AllowSorting="True"
            CssClass="Grid" AutoGenerateColumns="False"> 
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
                              <asp:ListItem Text="Print" />
                          </asp:DropDownList>
                          <asp:Button class="btngo" runat="server" ID="BtnGo" Text="G"
                           CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" CommandName="Go" />                          
                      </ItemTemplate>
                      <HeaderStyle Width="110px" />
                  </asp:TemplateField>                     
                  <asp:BoundField DataField="TransNmbr" HeaderStyle-Width="120px" SortExpression="Nmbr" HeaderText="Reference" />
                  <asp:BoundField DataField="Status" HeaderText="Status" SortExpression="Status"></asp:BoundField>                  
                  <asp:BoundField DataField="TransDate" HtmlEncode="true" DataFormatString="{0:dd MMM yyyy}" HeaderStyle-Width="80px" HeaderText="Date" SortExpression="TransDate" />
                  <asp:BoundField DataField="Customer_Name" HeaderText="Customer" SortExpression="Customer_Name" />
                  <asp:BoundField DataField="Attn" HeaderText="Attn" SortExpression="Attn"></asp:BoundField>
                  <asp:BoundField DataField="RequestNo" HeaderText="Request No." SortExpression="RequestNo" />
                  <asp:BoundField DataField="Wrhs_Name" HeaderText="Warehouse" SortExpression="Wrhs_Name" />
                  <asp:BoundField DataField="Subled_Name" HeaderText="Subled" SortExpression="Subled_Name" />                      
                  <asp:BoundField DataField="ReceivedBy" HeaderText="Received By" SortExpression="ReceivedBy" />                      
                  <asp:BoundField DataField="SJCustNo" HeaderStyle-Width="80px" HeaderText="DN Cust. No" SortExpression="SJCustNo" />                      
                  <asp:BoundField DataField="SJCustDate" HtmlEncode="true" DataFormatString="{0:dd MMM yyyy}" HeaderText="DN Cust. Date" SortExpression="SJCustDate" />                       
                  <asp:BoundField DataField="FgReport" HeaderText="Report" SortExpression="FgReport" />                                                                  
                  <asp:BoundField DataField="Remark" HeaderStyle-Width="80px" HeaderText="Remark">
                   </asp:BoundField>
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
            <td>Receiving Retur No.</td>
            <td>:</td>
            <td><asp:TextBox CssClass="TextBox" Enabled = "false" runat="server" ID="tbTransNo" Width="149px"/> 
                <%--Report : 
                <asp:DropDownList ID ="ddlReport" Enabled="false" runat ="server" CssClass="DropDownList">
                                    <asp:ListItem>Y</asp:ListItem>
                                    <asp:ListItem>N</asp:ListItem>
                                </asp:DropDownList>--%> 
            </td>            
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
                <td>Customer</td>
                <td>:</td>
                <td>
                <asp:TextBox ID="tbCustCode" ValidationGroup="Input" AutoPostBack="true" Width="100" runat="server" CssClass="TextBox" />
                <asp:TextBox ID="tbCustName" Width="175" runat="server" 
                        ReadOnly="true" CssClass="TextBoxR" />
                <asp:Button Class="btngo" ID="btnCust" Text="..." runat="server" ValidationGroup="Input" />                                                         
                </td>                
                
                <td>Attention</td>
                <td>:</td>
                <td>
                <asp:TextBox ID="tbAttn" ValidationGroup="Input" AutoPostBack="False" Width="250px" 
                        runat="server" CssClass="TextBox" />
                </td>                   
             </tr>               
        <tr>
            <td>Request No.</td>
            <td>:</td>
            <td><asp:TextBox runat="server" ValidationGroup="Input" ID="tbReturNo" 
                    CssClass="TextBoxR" Width="150px" ReadOnly="True" />
                <asp:Button Class="btngo" ID="btnRequestNo" Text="..." runat="server" ValidationGroup="Input" />                                                                         
                                                                                
            </td>             
              <td>
                  Warehouse</td>
              <td>:</td>
              <td colspan="4">
                  <asp:DropDownList ID="ddlWrhs" runat="server" AutoPostBack="true" 
                      CssClass="DropDownList" Height="16px" ValidationGroup="Input" Width="200px">
                  </asp:DropDownList>
                  <asp:TextBox ID="tbFgSubled" runat="server" AutoPostBack="true" 
                      CssClass="TextBox" Visible="False" />
              </td>
              <td></td>
                <td></td>
                <td></td>
          </tr>
          <tr>
              <td>
                  Subled</td>
              <td>:</td>
              <td colspan="4">
                  <asp:TextBox ID="tbSubled" runat="server" CssClass="TextBox" AutoPostBack = "True" />
                  <asp:TextBox ID="tbSubledName" runat="server" CssClass="TextBoxR" 
                      Enabled="False" Width="200px" />
                  <asp:Button Class="btngo" ID="btnSubled" Text="..." runat="server" ValidationGroup="Input" />                                                                                           
              </td>
              <td></td>
                <td></td>
                <td></td>
          </tr>          
            <tr>
              <td>
                  DN Customer No</td>
              <td>
                  &nbsp;</td>
              <td>
                  <asp:TextBox ID="tbDNCustNo" runat="server" CssClass="TextBox" 
                      ValidationGroup="Input" Width="236px" />
              </td>
            <td>DN Customer Date</td>
            <td>:</td>
            <td>
            <BDP:BasicDatePicker ID="tbDNCustDate" runat="server" DateFormat="dd MMM yyyy" 
                        ReadOnly = "true" ValidationGroup="Input"
                        ButtonImageHeight="19px" ButtonImageWidth="20px" 
                        DisplayType="TextBoxAndImage" 
                        TextBoxStyle-CssClass="TextDate" AutoPostBack="True" 
                    ShowNoneButton="False"><TextBoxStyle CssClass="TextDate" /></BDP:BasicDatePicker>                
            </td>                        
        </tr>    
        <tr>
              <td>
                  Received By</td>
              <td>:</td>
              <td>
                  <asp:TextBox ID="tbReceivedBy" runat="server" CssClass="TextBox" 
                      ValidationGroup="Input" Width="236px" />
              </td>
              <td>
                  Remark</td>
              <td>
                  :</td>
              <td>
                  <asp:TextBox ID="tbRemark" runat="server" CssClass="TextBox" 
                      ValidationGroup="Input" Width="269px" />
                
              </td>             
          </tr>
      </table>  
      
      <br />      
      <div style="font-size:medium; color:Blue;">Detail</div>
      <hr style="color:Blue" />  
        <asp:Panel runat="server" ID="pnlDt">
            <asp:Button class="bitbtn btnadd" runat="server" ID="btnAddDt" Text="Add" ValidationGroup="Input" />	        
            <div style="border:0px  solid; width:100%; height:100%; overflow:auto;">
                <asp:GridView ID="GridDt" runat="server" AutoGenerateColumns="False" 
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
                        <asp:BoundField DataField="SJNo" HeaderStyle-Width="120px" 
                            HeaderText="SJ No." SortExpression="SJNo" >                            
                            <HeaderStyle Width="120px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Product" HeaderStyle-Width="120px" 
                            HeaderText="Product" SortExpression="Product" >                            
                            <HeaderStyle Width="120px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Product_Name" HeaderText="Product Name" HeaderStyle-Width="200px" 
                            SortExpression="Product_Name" >
                            <HeaderStyle Width="200px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Specification" HeaderText="Specification" HeaderStyle-Width="200px" 
                            SortExpression="Specification" >
                            <HeaderStyle Width="200px" />
                        </asp:BoundField>
                        <asp:TemplateField Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lbLocation" runat="server" text='<%# DataBinder.Eval(Container.DataItem, "Location") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>   
                        <asp:BoundField DataField="Location_Name" HeaderStyle-Width="150px" 
                            HeaderText="Location" SortExpression="Location_Name" >                            
                            <HeaderStyle Width="150px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Qty" HeaderText="Qty Wrhs" DataFormatString="{0:#,##0.00}"  ItemStyle-HorizontalAlign="Right" 
                            SortExpression="Qty" >
                            <HeaderStyle Width="80px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Unit" HeaderText="Unit Wrhs" SortExpression="Unit" />                                                
                        <asp:BoundField DataField="QtyM2" HeaderStyle-Width="100px" HeaderText="Qty M2" DataFormatString="{0:#,##0.00}" ItemStyle-HorizontalAlign = "Right"  />
                        <asp:BoundField DataField="QtyRoll" HeaderStyle-Width="100px" HeaderText="Qty Roll" DataFormatString="{0:#,##0.00}" ItemStyle-HorizontalAlign = "Right"  />                            
                        <asp:BoundField DataField="Remark" HeaderStyle-Width="80px" 
                            HeaderText="Remark" >
                            <HeaderStyle Width="250px" />
                        </asp:BoundField>
                    </Columns>
                </asp:GridView>
          </div>   
          <asp:Button class="bitbtn btnadd" runat="server" ID="btnAddDt2" Text="Add" ValidationGroup="Input" />	
       </asp:Panel>             
       <asp:Panel runat="server" ID="pnlEditDt" Visible="false">
            <table>            
                <tr>
                    <td>
                        <asp:LinkButton ID="lbProduct" runat="server" Text="Product" />
                    </td>
                    <td>:</td>
                    <td>
                        <asp:TextBox ID="tbProdCode" runat="server" AutoPostBack="true" 
                            CssClass="TextBox" />
                        <asp:TextBox runat="server"  CssClass="TextBoxR"
                            ID="tbProdName" EnableTheming="True" Enabled="false" Width="200px"/>     
                        <%--<asp:TextBox ID="tbProdName" runat="server" CssClass="TextBox" Enabled="False" 
                            EnableTheming="True" ReadOnly="True" Width="200px" />--%>
                        <asp:Button Class="btngo" ID="btnProd" Text="..." runat="server" ValidationGroup="Input" />                                                                         
                        
                    </td>
                </tr>
                <tr>
                    <td>SJ No </td>                    
                    <td>:</td>
                    <td>
                        <asp:TextBox runat="server"  CssClass="TextBoxR"
                            ID="tbSJNo" EnableTheming="True" Enabled="false" Width="200px"/>     
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:label ID="lbSpec" runat="server" Text="Specification" />
                    </td>
                    <td>:</td>
                    <td>
                        <asp:TextBox ID="tbSpecification" runat="server" CssClass="TextBox" Enabled="False" 
                            EnableTheming="True" ReadOnly="True" Width="200px" />                        
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:LinkButton ID="lbLocation" runat="server" Text="Location" />
                    </td>
                    <td>
                        :</td>
                    <td>
                        <asp:DropDownList ID="ddlLocation" runat="server" AutoPostBack="True" 
                            CssClass="DropDownList" Width="150px" />
                    </td>
                </tr>
                <tr>
                    <td>
                        Qty</td>
                    <td>
                        :</td>
                    <td>
                        <asp:TextBox ID="tbQty" runat="server" CssClass="TextBox" 
                            AutoPostBack="True" />
                    </td>
                </tr>
                <tr>
                    <td><asp:Label ID="LbQtyM2" runat="server" Text="Qty M2"/> </td>
                    <td><asp:Label ID="lbDot1" runat="server" Text=":"/></td>
                    <td><asp:TextBox CssClass="TextBox" runat="server" ID="tbQtyM2" AutoPostBack="True" style="text-align:right;" /></td>
                </tr>
                 <tr>
                    <td><asp:Label ID="LbQtyRoll" runat="server" Text="Qty Roll"/> </td> 
                    <td><asp:Label ID="lbDot2" runat="server" Text=":"/></td>
                    <td><asp:TextBox CssClass="TextBox" runat="server" ID="tbQtyRoll" AutoPostBack="True" style="text-align:right;" /></td>
                </tr>
                <tr>
                    <td>
                        Unit</td>
                    <td>
                        &nbsp;</td>
                    <td>
                        <asp:TextBox ID="tbUnit" runat="server" CssClass="TextBoxR" Enabled="False" 
                            Width="50px" />
                    </td>
                </tr>                                
                <tr>
                    <td>
                        Remark
                    </td>
                    <td>
                        :</td>
                    <td>
                        <asp:TextBox ID="tbRemarkDt" runat="server" CssClass="TextBox" Width="304px" />
                    </td>
                </tr>
            </table>
            <br />
            <asp:Button ID="btnSaveDt" runat="server" class="bitbtndt btnsave" Text="Save" />									
            <asp:Button ID="btnCancelDt" runat="server" class="bitbtndt btncancel" Text="Cancel" />									
            <br />
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
