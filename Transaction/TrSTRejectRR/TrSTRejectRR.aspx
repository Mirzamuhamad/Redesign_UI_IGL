<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TrSTRejectRR.aspx.vb" Inherits="TrSTRejectRR" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="BasicFrame.WebControls.BasicDatePicker" Namespace="BasicFrame.WebControls"
    TagPrefix="BDP" %>
<%@ Register assembly="CrystalDecisions.Web, Version=10.5.3700.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Adjust Customer Limit</title>
    <script src="../../Function/OpenDlg.js" type="text/javascript"></script>
    <script src="../../Function/Function.JS" type="text/javascript"></script>
    <script type="text/javascript">         
        
    </script>
    
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
</head>
<body>     
    <form id="form1" runat="server">    
    <div class="Content">
    <div class="H1">Receiving Return Supplier</div>
     <hr style="color:Blue" />        
    <asp:Panel runat="server" id="PnlHd"> 
      <table>
        <tr>
            <td style="width:100px;text-align:right">Quick Search :</td>            
            <td><asp:TextBox CssClass="TextBox" runat="server" ID ="tbFilter"/> 
                  <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlField" >
                      <asp:ListItem Value="TransNmbr" Selected="True">Receiving Retur No</asp:ListItem>
                      <asp:ListItem Value="dbo.FormatDate(TransDate)">Date</asp:ListItem>
                      <asp:ListItem>Status</asp:ListItem>
                      <asp:ListItem Value="Supplier_Name">Supplier</asp:ListItem>
                      <asp:ListItem Value="Attn">Attention</asp:ListItem>
                      <asp:ListItem Value="SJReject">Delivery Retur No</asp:ListItem>
                      <asp:ListItem Value="Wrhs_Name">Warehouse</asp:ListItem>
                      <asp:ListItem Value="Subled_Name">Subled</asp:ListItem>
                      <asp:ListItem Value="ReceivedBy">Received By</asp:ListItem>
                      <asp:ListItem Value="SJSuppNo">DN Supplier No</asp:ListItem>
                      <asp:ListItem Value="SJSuppDate">DN Supplier Date</asp:ListItem>
                      <asp:ListItem Value="FgReport">Fg Report</asp:ListItem>                      
                      <asp:ListItem Value="Remark">Remark</asp:ListItem>
                    </asp:DropDownList>
                    <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlRange"></asp:DropDownList>
                  <asp:Button class="bitbtn btnsearch" runat="server" ID="btnSearch" Text="Search" />																						 											  
                  <asp:Button class="bitbtn btngo" runat="server" ID="btnExpand" Text="..." />																					 											  
            </td>
            <td>
                <asp:LinkButton ID="LbAdvSearch" runat="server" Text="Advanced Search" />
                &nbsp &nbsp &nbsp &nbsp
                <asp:Label runat="server" ID="Label1" Text="Purchase Reject Gantung : "/>
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
                      <asp:ListItem Value="TransNmbr" Selected="True">Receiving Retur No</asp:ListItem>
                      <asp:ListItem Value="dbo.FormatDate(TransDate)">Date</asp:ListItem>
                      <asp:ListItem>Status</asp:ListItem>
                      <asp:ListItem Value="Supplier_Name">Supplier</asp:ListItem>
                      <asp:ListItem Value="Attn">Attention</asp:ListItem>
                      <asp:ListItem Value="SJReject">Delivery Retur No</asp:ListItem>
                      <asp:ListItem Value="Wrhs_Name">Warehouse</asp:ListItem>
                      <asp:ListItem Value="Subled_Name">Subled</asp:ListItem>
                      <asp:ListItem Value="ReceivedBy">Received By</asp:ListItem>
                      <asp:ListItem Value="SJSuppNo">DN Supplier No</asp:ListItem>
                      <asp:ListItem Value="SJSuppDate">DN Supplier Date</asp:ListItem>
                      <asp:ListItem Value="FgReport">Fg Report</asp:ListItem>                      
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
                              <asp:ListItem Text="Print" />
                          </asp:DropDownList>
                          <asp:Button class="btngo" runat="server" ID="BtnGo" Text="G" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" CommandName="Go" />      
                      </ItemTemplate>
                      <HeaderStyle Width="110px" />
                  </asp:TemplateField>                     
                  <asp:BoundField DataField="TransNmbr" HeaderStyle-Width="120px" SortExpression="Nmbr" HeaderText="Delivery Note No.">
                      <HeaderStyle Width="120px" />
                  </asp:BoundField>                  
                  <asp:BoundField DataField="Status" HeaderText="Status" SortExpression="Status"></asp:BoundField>                  
                  <asp:BoundField DataField="TransDate" HtmlEncode="true" DataFormatString="{0:dd MMM yyyy}" HeaderStyle-Width="80px" 
                      HeaderText="Date" SortExpression="TransDate">
                      <HeaderStyle Width="80px" />
                  </asp:BoundField>
                  <asp:BoundField DataField="FgReport" HeaderText="Fg Report" 
                      SortExpression="FgReport" />                                                                  
                  <asp:BoundField DataField="Supplier_Name" HeaderText="Supplier" 
                      SortExpression="Supplier_Name" >
                      <HeaderStyle Width="80px" />
                  </asp:BoundField>
                  <asp:BoundField DataField="Attn" 
                      HeaderText="Attention" SortExpression="Attn">
                  </asp:BoundField>
                  <asp:BoundField DataField="SJReject" HeaderText="Delivery Retur No." 
                      SortExpression="SJReject" />
                  <asp:BoundField DataField="Wrhs_Name" HeaderText="Warehouse" 
                      SortExpression="Wrhs_Name" />
                  <asp:BoundField DataField="Subled_Name" HeaderText="Subled Name" 
                      SortExpression="Subled_Name" />                      
                  <asp:BoundField DataField="ReceivedBy" HeaderText="Received By" 
                      SortExpression="ReceivedBy" />                      
                  <asp:BoundField DataField="SJSuppNo" HeaderText="DN Supplier No" 
                      SortExpression="SJSuppNo" />                      
                  <asp:BoundField DataField="SJSuppDate" HtmlEncode="true" DataFormatString="{0:dd MMM yyyy}" HeaderText="DN Supplier Date" 
                      SortExpression="SJSuppDate" />                                         
                  <asp:BoundField DataField="Remark" HeaderStyle-Width="80px" HeaderText="Remark">
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
            <td>Receiving Retur No.</td>
            <td>:</td>
            <td style="width:270px;"><asp:TextBox CssClass="TextBox" Enabled = "false" runat="server" ID="tbTransNo" Width="149px"/> 
            <%--<asp:Label runat="server" Text="   Report : "></asp:Label>
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
                <td>Supplier</td>
                <td>:</td>
                <td colspan="4">
                <asp:TextBox ID="tbSuppCode" ValidationGroup="Input" AutoPostBack="true" Width="100" runat="server" CssClass="TextBox" />
                <asp:TextBox ID="tbSuppName" Width="275" runat="server" 
                        ReadOnly="true" CssClass="TextBoxR" />
                <asp:Button Class="btngo" ID="btnSupp" Text="..." runat="server" ValidationGroup="Input" />                                                                     
                </td>                
             </tr>                             
        <tr>
            <td>Delivery Retur No.</td>
            <td>:</td>
            <td><asp:TextBox runat="server" ValidationGroup="Input" ID="tbReturNo" 
                    CssClass="TextBoxR" Width="150px" ReadOnly="True" />
                <asp:Button Class="btngo" ID="btnPO" Text="..." runat="server" ValidationGroup="Input" />                                                                                                                                  
            </td> 
            <td>Attention</td>
                <td>:</td>
                <td>
                <asp:TextBox ID="tbAttn" ValidationGroup="Input" AutoPostBack="False" Width="236px" 
                        runat="server" CssClass="TextBox" MaxLength = "60" />
                </td>   
        </tr>                                                          
          <tr>
              <td>
                  Warehouse</td>
              <td>:</td>
              <td>
                  <asp:DropDownList ID="ddlWrhs" runat="server" AutoPostBack="true" 
                      CssClass="DropDownList" Height="16px" ValidationGroup="Input" Width="200px">
                  </asp:DropDownList>
                  <asp:TextBox ID="tbFgSubled" runat="server" AutoPostBack="true" 
                      CssClass="TextBox" Visible="False" />
              </td>
              <td>
                  ReceivedBy</td>
              <td>:</td>
              <td>
                  <asp:TextBox ID="tbReceivedBy" runat="server" CssClass="TextBox" 
                      ValidationGroup="Input" Width="236px" MaxLength = "60"/>
              </td>
          </tr>
          <tr>
              <td>
                  Subled</td>
              <td>:</td>
              <td colspan="4">
                  <asp:TextBox ID="tbSubled" runat="server" CssClass="TextBox" />
                  <asp:TextBox ID="tbSubledName" runat="server" CssClass="TextBoxR" 
                      Enabled="False" Width="200px" />
                  <asp:Button Class="btngo" ID="btnSubled" Text="..." runat="server" ValidationGroup="Input" />                                                                                                                  
             </td>
          </tr>
          <tr>
              <td>
                  DN Supp No</td>
              <td>:</td>
              <td>
                  <asp:TextBox ID="tbDNSuppNo" runat="server" CssClass="TextBox" 
                      ValidationGroup="Input" Width="236px" MaxLength = "30"/>
              </td>
            <td>DN Supp Date</td>
            <td>:</td>
            <td>
            <BDP:BasicDatePicker ID="tbDNSuppDate" runat="server" DateFormat="dd MMM yyyy" 
                        ReadOnly = "true" ValidationGroup="Input"
                        ButtonImageHeight="19px" ButtonImageWidth="20px" 
                        DisplayType="TextBoxAndImage" 
                        TextBoxStyle-CssClass="TextDate" AutoPostBack="True" 
                    ShowNoneButton="False"><TextBoxStyle CssClass="TextDate" /></BDP:BasicDatePicker>                
            </td>                        
        </tr>    
          <tr>
              <td>Remark</td>
              <td>:</td>
              <td colspan="4">
                  <asp:TextBox ID="tbRemark" runat="server" CssClass="TextBox" 
                      ValidationGroup="Input" Width="369px" MaxLength = "255" />
                <asp:Button Class="bitbtn btngetitem" ID="btnGetDt" Text="Get Data" runat="server" ValidationGroup="Input" />                                                                                                                                              
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
                        <asp:BoundField DataField="Product" HeaderStyle-Width="120px" 
                            HeaderText="Product" SortExpression="Product" >                            
                            <HeaderStyle Width="120px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Product_Name" HeaderText="Product Name" HeaderStyle-Width="200px" 
                            SortExpression="Product_Name" >
                            <HeaderStyle Width="200px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Specification" 
                            HeaderText="Specification" SortExpression="Specification" >                            
                            <HeaderStyle Width="200px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Qty" HeaderText="Qty Wrhs" DataFormatString="{0:#,##0.00}"  ItemStyle-HorizontalAlign="Right" 
                            SortExpression="Qty" >
                            <HeaderStyle Width="80px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Unit" HeaderText="Unit Wrhs" SortExpression="Unit" />                                                
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
                        <asp:TextBox ID="tbProdName" runat="server" CssClass="TextBox" Enabled="False" 
                            EnableTheming="True" ReadOnly="True" Width="200px" />
                        <asp:Button Class="btngo" ID="btnProd" Text="..." runat="server"  /> 
                    </td>
                </tr>
                <tr>
                    <td>
                        Spesification</td>
                    <td>&nbsp;</td>
                    <td>
                        <asp:TextBox ID="tbSpecification" runat="server" CssClass="TextBoxR" 
                            Enabled="False" Width="303px" />
                     </td>
                </tr>
                <tr>
                    <td>
                        Qty</td>
                    <td>
                        :</td>
                    <td>
                        <asp:TextBox ID="tbQty" runat="server" CssClass="TextBox" 
                            AutoPostBack="False" />
                    </td>
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
                        <asp:TextBox ID="tbRemarkDt" runat="server" CssClass="TextBox" Width="304px" MaxLength = "255"/>
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
