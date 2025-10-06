<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TrSTRRService.aspx.vb" Inherits="Transaction_TrSTRRService_TrSTRRService" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="BasicFrame.WebControls.BasicDatePicker" Namespace="BasicFrame.WebControls"
    TagPrefix="BDP" %>
<%@ Register assembly="CrystalDecisions.Web, Version=10.5.3700.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>

<%@ Register assembly="FastReport" namespace="FastReport.Web" tagprefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
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
        var Qty = document.getElementById("tbQty").value.replace(/\$|\,/g,""); 
//        var QtyRR = document.getElementById("tbQtyRR").value.replace(/\$|\,/g,""); 
        var QtyOrder = document.getElementById("tbQtyOrder").value.replace(/\$|\,/g,""); 
        document.getElementById("tbQty").value = setdigit(Qty,'<%=ViewState("DigitQty")%>');
//        document.getElementById("tbQtyRR").value = setdigit(QtyRR,'<%=ViewState("DigitQty")%>');
        document.getElementById("tbQtyOrder").value = setdigit(QtyOrder,'<%=ViewState("DigitQty")%>');
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
    <div class="H1">Receiving Report Service</div>
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
                      <asp:ListItem Value="Supplier">Supplier</asp:ListItem>
                      <asp:ListItem Value="Supplier_Name">Supplier Name</asp:ListItem>                  
                      <asp:ListItem Value="PONo">PO NO</asp:ListItem>
                      <%--<asp:ListItem Value="Product_Type_Name">Product Type</asp:ListItem>--%>
                      <asp:ListItem Value="Wrhs_Name">Warehouse</asp:ListItem>
                      <asp:ListItem Value="ReceivedBy">Received By</asp:ListItem>
                      <asp:ListItem>Remark</asp:ListItem>
                  </asp:DropDownList>
                  <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlRange"></asp:DropDownList>
                 
                  <asp:Button class="bitbtn btnsearch" runat="server" ID="btnSearch" Text="Search" />																						 											  
                  <asp:Button class="bitbtn btngo" runat="server" ID="btnExpand" Text="..." />																					 											  
            </td>
            <td>
                <asp:LinkButton ID="LbAdvSearch" runat="server" Text="Advanced Search" />
                &nbsp &nbsp &nbsp &nbsp
                <asp:Label runat="server" ID="Label1" Text="PO Gantung : "/>
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
                      <asp:ListItem Value="Supplier">Supplier</asp:ListItem>
                      <asp:ListItem Value="Supplier_Name">Supplier Name</asp:ListItem>                      
                      <asp:ListItem Value="PONo">PO NO</asp:ListItem>
                      <%--<asp:ListItem Value="Product_Type_Name">Product Type</asp:ListItem>--%>
                      <asp:ListItem Value="Wrhs_Name">Warehouse</asp:ListItem>
                      <asp:ListItem Value="ReceivedBy">Received By</asp:ListItem>
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
                                  <asp:ListItem Selected="True" Text="View" />
                                  <asp:ListItem Text="Edit" />
                                  <asp:ListItem Text="Input Lot No" />                              
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
                      <asp:BoundField DataField="Supplier" HeaderText="Supplier" 
                          SortExpression="Supplier" />
                      <asp:BoundField DataField="Supplier_Name" HeaderText="Supplier Name" 
                          SortExpression="Supplier_Name" />                      
                      <asp:BoundField DataField="PONo" HeaderText="PO NO" SortExpression="PONo" />
<%--                      <asp:BoundField DataField="Product_Type_Name" HeaderStyle-Width="150px" 
                          HeaderText="Product Type" SortExpression="Product_Type_Name">
                          <HeaderStyle Width="150px" />
                      </asp:BoundField>--%>
                      <asp:BoundField DataField="Wrhs_Name" HeaderStyle-Width="102px" 
                          HeaderText="Warehouse" SortExpression="Wrhs_Name">
                          <HeaderStyle Width="200px" />
                      </asp:BoundField>
                      <asp:BoundField DataField="SubLed_Name" HeaderStyle-Width="200px" 
                          HeaderText="SubLed" SortExpression="SubLed_Name">
                          <HeaderStyle Width="200px" />
                      </asp:BoundField>
                      <asp:BoundField DataField="Expedition" HeaderText="Expedition" SortExpression="Expedition" />
                      <asp:BoundField DataField="CarNo" HeaderText="Car No" SortExpression="CarNo"  />
                      <asp:BoundField DataField="ContainerNo" HeaderText="Container" SortExpression="ContainerNo"/>
                      <asp:BoundField DataField="ReceivedBy" HeaderStyle-Width="200px" 
                          HeaderText="Received By" SortExpression="ReceivedBy">
                          <HeaderStyle Width="200px" />
                      </asp:BoundField>
                      <asp:BoundField DataField="Remark" HeaderStyle-Width="200px" 
                          HeaderText="Remark">
                          <HeaderStyle Width="250px" />
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
            <td>Reference</td>
            <td>:</td>
            <td><asp:TextBox CssClass="TextBoxR" runat="server" ID="tbReference" Width="150px" Enabled="False"/> </td>            
            <td>&nbsp &nbsp &nbsp</td>
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
                <asp:LinkButton ID="lbSupplier" runat="server" Text="Supplier" 
                    ValidationGroup="Input" />
            </td>
            <td>:</td>
            <td colspan="5">
                <asp:TextBox ID="tbSuppCode" ValidationGroup="Input" runat="server" AutoPostBack="true" MaxLength = "12" 
                    CssClass="TextBox" />
                <asp:TextBox ID="tbSuppName" runat="server" CssClass="TextBoxR" Enabled="false" 
                    EnableTheming="True" Width="200px" />
                <asp:Button Class="btngo" ID="btnSupp" Text="..." runat="server" ValidationGroup="Input" />                                                       
                <asp:Label ID="Label8" runat="server" ForeColor="Red">*</asp:Label>
                <%--<asp:ImageButton ID="btnSupp" runat="server" ImageAlign="AbsBottom" 
                    ImageUrl="../../Image/btnDot2on.png" 
                    onmouseout="this.src='../../Image/btnDot2on.png';" 
                    onmouseover="this.src='../../Image/btnDot2off.png';" ValidationGroup="Input" />--%>
            </td>        
              
          </tr>
        <tr>
            <td><asp:LinkButton ID="lbWarehouse" ValidationGroup="Input" runat="server" Text="Warehouse Receive"/></td>
            <td>:</td>
            <td><asp:DropDownList CssClass="DropDownList" ValidationGroup="Input" runat="server" ID="ddlwrhs" AutoPostBack="true" Width="200px" />
                <asp:TextBox runat="server" ID="tbFgSubLed" Visible="false"/>                
                <asp:Label ID="Label3" runat="server" ForeColor="Red">*</asp:Label>
            </td> 
            <td></td>                   
            <td>
                  PO No</td>
              <td>
                  :</td>
              <td>
                  <asp:TextBox ID="tbPONo" runat="server" CssClass="TextBox" 
                      Width="150px" Enabled = "false" AutoPostBack="False" />
                  <asp:Button Class="btngo" ID="btnPONo" Text="..." runat="server" ValidationGroup="Input" />                                                                               
                  <asp:TextBox ID="tbFgReport" runat="server" Visible="False" />
                  <asp:Label ID="Label4" runat="server" ForeColor="Red">*</asp:Label>
              </td>
        </tr>  
        <tr>
            <td>Receive SubLed</td>
            <td>:</td>
            <td colspan="5"><asp:TextBox CssClass="TextBox" ValidationGroup="Input" runat="server" ID="tbSubLed" AutoPostBack="true" /> 
                <asp:TextBox CssClass="TextBoxR" runat="server" ID="tbSubLedName" enabled="false" Width="225px"/>
                <asp:Button Class="btngo" ID="btnSubLed" Text="..." runat="server" ValidationGroup="Input" />                                                                               
              
            </td>                    
        </tr>  
        <tr>
            <td>SJ Supplier No</td>
            <td>:</td>
            <td>
                <asp:TextBox ID="tbSJSuppNo" runat="server" CssClass="TextBox" MaxLength = "30" 
                    ValidationGroup="Input" Width="200px" />
                    <asp:Label ID="Label5" runat="server" ForeColor="Red">*</asp:Label>
             </td>
             <td></td>
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
                  Expedition</td>
              <td>:</td>
              <td>
                  <asp:TextBox ID="tbExpedition" runat="server" CssClass="TextBox" MaxLength = "60" 
                      ValidationGroup="Input" Width="200px" />
              </td>   
              <td></td>       
              <td>
                  Car No</td>
              <td>
                  &nbsp;</td>
              <td>
                  <asp:TextBox ID="tbCarNo" runat="server" CssClass="TextBox" MaxLength = "60" 
                      ValidationGroup="Input" Width="200px" />
              </td>
          </tr>
          <tr>
              <td>
                  Container No</td>
              <td>:</td>
              <td>
                  <asp:TextBox ID="tbContainer" runat="server" CssClass="TextBox" MaxLength = "100" 
                      ValidationGroup="Input" Width="200px" />
              </td>    
              <td></td>      
              <td>
                  Received By</td>
              <td>
                  :</td>
              <td>
                  <asp:TextBox ID="tbReceivedBy" runat="server" CssClass="TextBox" MaxLength = "60" 
                      ValidationGroup="Input" Width="200px" />
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
                        <asp:BoundField DataField="Product" HeaderStyle-Width="120px" 
                            HeaderText="Product Code" >
                            <HeaderStyle Width="120px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Product_Name" HeaderStyle-Width="250px"  
                            HeaderText="Product Name" >
                            <HeaderStyle Width="250px" />
                        </asp:BoundField>                        
                        <%--<asp:BoundField DataField="Fg_Stock" HeaderStyle-Width="120px" 
                            HeaderText="Fg Stock" >
                            <HeaderStyle Width="120px" />
                        </asp:BoundField>--%>
                        <asp:BoundField DataField="Specification" HeaderText="Specification" HeaderStyle-Width="150px"  
                            SortExpression="Specification" >
                            <HeaderStyle Width="150px" />
                        </asp:BoundField>
                        <asp:TemplateField Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lbLocation" runat="server" text='<%# DataBinder.Eval(Container.DataItem, "Location") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>   
                        <asp:BoundField DataField="Location_Name" HeaderStyle-Width="150px"  
                            HeaderText="Location" >
                            <HeaderStyle Width="150px" />
                        </asp:BoundField>
                        <%--<asp:BoundField DataField="QtyPO" HeaderStyle-Width="80px" 
                            HeaderText="Qty PO" ItemStyle-HorizontalAlign="Right" 
                            SortExpression="QtyPO">
                            <HeaderStyle Width="80px" />
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>--%>
                        <asp:BoundField DataField="QtyOrder" HeaderStyle-Width="80px" 
                            HeaderText="Qty Order" ItemStyle-HorizontalAlign="Right" 
                            SortExpression="QtyOrder">
                            <HeaderStyle Width="80px" />
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:BoundField DataField="UnitOrder" HeaderStyle-Width="80px" HeaderText="Unit Order" 
                            SortExpression="UnitOrder" />
                         <asp:BoundField DataField="Qty" HeaderStyle-Width="80px" HeaderText="Qty Wrhs" >
                        <HeaderStyle Width="80px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Unit" HeaderStyle-Width="60px" HeaderText="Unit Wrhs" >
                            <HeaderStyle Width="60px" />
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
                        <asp:TextBox runat="server" ID="tbFgStock" CssClass="TextBox" AutoPostBack="true" Visible = "False" />
                        <asp:TextBox runat="server"  CssClass="TextBoxR"
                            ID="tbProductName" EnableTheming="True" Enabled="false" Width="250px"/> 
                        <asp:Button Class="btngo" ID="btnProduct" Text="..." runat="server" ValidationGroup="Input" />                                                       
                        <asp:Label ID="Label6" runat="server" ForeColor="Red">*</asp:Label>
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
                    <td>Location</td>
                    <td>:</td>
                    <td>                     
                        <asp:DropDownList ID="ddlLocation" runat="server" AutoPostBack="False" 
                            CssClass="DropDownList" Width="180px" />
                         <asp:Label ID="Label7" runat="server" ForeColor="Red">*</asp:Label>   
                    </td>
                </tr>
                <tr>
                    <td>Qty Order</td>
                    <td>:</td>
                    <td>
                        <asp:TextBox ID="tbQtyOrder" runat="server" CssClass="TextBox" 
                            AutoPostBack="True" />
                        <asp:TextBox CssClass="TextBoxR" runat="server" ID="tbUnitOrder" Enabled="false" Width="50px" />
                    </td>                    
                </tr>    
                <tr>
                    <td>Qty Wrhs</td>
                    <td>:</td>
                    <td>
                        <asp:TextBox ID="tbQty" runat="server" CssClass="TextBoxR" Enabled="False" />
                        <asp:TextBox runat="server" CssClass="TextBoxR" ID="tbUnit" Enabled="false" Width="50px" />
                    </td>                    
                </tr>                
                <tr>
                    <td>
                        Remark
                    </td>
                    <td>
                        :</td>
                    <td>
                        <asp:TextBox ID="tbRemarkDt" runat="server" CssClass="TextBoxMulti" Width="350px" MaxLength = "255" TextMode="MultiLine" />
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
<%-- <cc1:WebReport ID="WebReport1" runat="server" AutoHeight="True" 
            AutoWidth="True" Width="100%" Height = "100%" 
            ShowRefreshButton="False" />--%><br />
        <div>
            <cc1:WebReport ID="WebReport1" runat="server" AutoHeight="True" 
                AutoWidth="True" Height="100%" Width="100%" />
        </div>
    </asp:Panel>
      <br />            
    </div>
   
    <asp:Label runat ="server" ID="lbStatus" ForeColor="Red"/>
   
    </form>
    </body>
</html>
