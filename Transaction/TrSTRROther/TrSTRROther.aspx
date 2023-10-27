<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TrSTRROther.aspx.vb" Inherits="Transaction_TrSTRROther_TrSTRROther" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="BasicFrame.WebControls.BasicDatePicker" Namespace="BasicFrame.WebControls"
    TagPrefix="BDP" %>
<%@ Register assembly="CrystalDecisions.Web, Version=10.5.3700.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>

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
        if (Qty >= 0)
        document.getElementById("tbQty").value = setdigit(Qty,'<%=ViewState("DigitQty")%>');
        
        }catch (err){
            alert(err.description);
          }      
        }   
        
//       function OpenTransactionSelf(_form, _keyid, _code) {
//            window.open("../../Transaction/"+_form+"/"+_form+".Aspx?KeyId="+_keyid+"&ContainerId="+_form+"Id&Code="+_code,"_self","scrollbars=yes,resizable=no,width=700,height=500");          
//            return false;
//        }
    </script>
    
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
</head>
<body>     
    <form id="form1" runat="server">    
    <div class="Content">
    <div class="H1">&nbsp;<asp:Label ID="Labelmenu" runat="server" Text="RR Other"></asp:Label>
            </div>
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
                      <asp:ListItem Value="Warehouse">Warehouse</asp:ListItem>
                      <asp:ListItem Value="WarehouseName">Warehouse Name</asp:ListItem>
                      <asp:ListItem>Subled</asp:ListItem>
                      <asp:ListItem Value="SubledName">Subled Name</asp:ListItem>                  
                     
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
                  <asp:ListItem Value="dbo.FormatDate(TransDate)">Date</asp:ListItem>
                  <asp:ListItem>Status</asp:ListItem>
                  <asp:ListItem Value="Warehouse">Warehouse</asp:ListItem>
                  <asp:ListItem Value="WarehouseName">Warehouse Name</asp:ListItem>
                  <asp:ListItem>Subled</asp:ListItem>
                  <asp:ListItem Value="SubledName">Subled Name</asp:ListItem>                
                 
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
                  <%--<asp:BoundField DataField="QCNo" HeaderText="QC No" SortExpression="QCNo" />--%>
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
                          <asp:Button class="btngo" runat="server" ID="BtnGo" Text="G"
                           CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" CommandName="Go" />                          
                              
                      </ItemTemplate>
                      <HeaderStyle Width="110px" />
                  </asp:TemplateField>
                  <asp:BoundField DataField="TransNmbr" HeaderStyle-Width="120px" 
                      HeaderText="Reference" SortExpression="Nmbr">
                      <HeaderStyle Width="120px" />
                  </asp:BoundField>
                  <asp:BoundField DataField="Status" HeaderText="Status" />
                  <asp:BoundField DataField="TransDate" HtmlEncode="true" DataFormatString="{0:dd MMM yyyy}" HeaderStyle-Width="80px" SortExpression="TransDate"
                      HeaderText="Date">
                      <HeaderStyle Width="80px" />
                  </asp:BoundField>
                  <asp:BoundField DataField="WarehouseName" HeaderStyle-Width="102px" 
                      HeaderText="Warehouse" SortExpression="WarehouseName">
                      <HeaderStyle Width="200px" />
                  </asp:BoundField>
                  <asp:BoundField DataField="Subled" 
                      HeaderText="Subled" SortExpression="Subled">
                  </asp:BoundField>
                  <asp:BoundField DataField="SubledName" HeaderText="Subled Name" 
                      SortExpression="SubledName" />
                  <asp:BoundField DataField="RRFromName" HeaderStyle-Width="200px" 
                      HeaderText="RR From" SortExpression="RRFromName" >
                      <HeaderStyle Width="200px" />
                  </asp:BoundField>
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
        </tr>
        <tr>
            <td>Date</td>
            <td>:</td>
            <td>
            <BDP:BasicDatePicker ID="tbDate" runat="server" DateFormat="dd MMM yyyy" 
                        ReadOnly = "true" ValidationGroup="Input"
                        ButtonImageHeight="19px" ButtonImageWidth="20px" 
                        DisplayType="TextBoxAndImage" 
                        TextBoxStyle-CssClass="TextDate" AutoPostBack="True" 
                        ShowNoneButton="False"><TextBoxStyle CssClass="TextDate" /></BDP:BasicDatePicker>                
                &nbsp;<asp:TextBox ID="tbRRType" runat="server" Visible="false" />
            </td>            
        </tr> 
         
        <tr>
            <td><asp:LinkButton ID="lbWarehouse" ValidationGroup="Input" runat="server" Text="Warehouse"/></td>
            <td>:</td>
            <td><asp:DropDownList CssClass="DropDownList" ValidationGroup="Input" runat="server" ID="ddlwrhs" AutoPostBack="true" Width="200px" />
                <asp:Label ID="lbA" runat="server" ForeColor="Red">*</asp:Label>
                <asp:TextBox runat="server" ID="tbFgSubLed" Visible="false"/>                
            </td>                    
        </tr>  
        <tr>
            <td>SubLed</td>
            <td>:</td>
            <td><asp:TextBox CssClass="TextBox" ValidationGroup="Input" runat="server" ID="tbSubLed" AutoPostBack="true" /> 
                <asp:TextBox CssClass="TextBoxR" runat="server" ID="tbSubLedName" enabled="false" Width="225px"/>                
                <asp:Button Class="btngo" ID="btnSubLed" Text="..." runat="server" ValidationGroup="Input" />                                  
            </td>                    
        </tr> 
        <tr>
            <td>
                <asp:LinkButton ID="lbRRFrom" runat="server" Text="Supplier" 
                    ValidationGroup="Input" />
                <asp:Label ID="Label1" runat="server" Text="RR From"></asp:Label>
            </td>
            <td>&nbsp;</td>
            <td>
                <asp:TextBox ID="tbRRFrom" runat="server" AutoPostBack="True" 
                    CssClass="TextBox" ValidationGroup = "Input" />
                <asp:TextBox ID="tbRRFromName" runat="server" CssClass="TextBoxR" Enabled="false" 
                    EnableTheming="True" Width="200px" />
                <asp:Button Class="btngo" ID="btnRRFrom" Text="..." runat="server" ValidationGroup="Input" />                                  
                
                <asp:Label ID="lbA0" runat="server" ForeColor="Red">*</asp:Label>
                
            </td>            
        </tr> 
        <tr>
            <td>SJ No</td>
            <td>:</td>
            <td>
                <asp:TextBox ID="tbSJSuppNo" runat="server" CssClass="TextBox" 
                    ValidationGroup="Input" Width="200px" />
                <asp:Label ID="lbA1" runat="server" ForeColor="Red">*</asp:Label>
            </td>
        </tr>
          <tr>
              <td>
                  SJ Date</td>
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
                  Received By</td>
              <td>
                  :</td>
              <td>
                  <asp:TextBox ID="tbReceivedBy" runat="server" CssClass="TextBox" 
                      ValidationGroup="Input" Width="200px" />
                  &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                  
                  <asp:Button Class="bitbtndt btngetitem" ID="btnGetDt" Text="Get Data" Width = "70" runat="server" ValidationGroup="Input" />                                  
                  
              </td>
          </tr>
          <tr>
              <td>
                  Remark</td>
              <td>
                  :</td>
              <td>
                  <asp:TextBox ID="tbRemark" runat="server" CssClass="TextBox" 
                      ValidationGroup="Input" Width="225px" />
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
                            HeaderText="Product Code" >
                            <HeaderStyle Width="120px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="ProductName" HeaderStyle-Width="250px"  
                            HeaderText="Product Name" >
                            <HeaderStyle Width="250px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Specification" HeaderText="Specification" HeaderStyle-Width="150px"  
                            SortExpression="Specification" >
                            <HeaderStyle Width="150px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Qty" HeaderStyle-Width="80px" HeaderText="Qty" >
                            <HeaderStyle Width="80px" />
                        </asp:BoundField>                        
                        <asp:BoundField DataField="Unit" HeaderStyle-Width="80px" HeaderText="Unit" >
                            <HeaderStyle Width="80px" />
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
                        <asp:Button Class="btngo" ID="btnProduct" Text= "..." runat="server" />                                         
                        
                        <asp:Label ID="lbA2" runat="server" ForeColor="Red">*</asp:Label>
                        
                    </td>                               
                </tr>
                <tr>
                    <td>Specification</td>
                    <td>:</td>
                    <td>
                        <asp:TextBox ID="tbSpecification" runat="server" CssClass="TextBoxR" 
                            Enabled="false" EnableTheming="True" Width="301px" />
                    </td>
                </tr>      
                <tr>
                    <td>Qty </td>
                    <td>:</td>
                    <td><asp:TextBox ID="tbQty" runat="server" CssClass="TextBox" 
                            AutoPostBack="True" />
                        <asp:TextBox ID="tbUnit" runat="server" CssClass="TextBoxR" Enabled="false" 
                            Width="71px" />
                    </td>                    
                </tr>                
                <tr>
                    <td>Remark </td>
                    <td>:</td>
                    <td><asp:TextBox ID="tbRemarkDt" runat="server" CssClass="TextBox" Width="250px" /></td>                    
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
