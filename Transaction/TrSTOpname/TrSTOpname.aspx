<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TrSTOpname.aspx.vb" Inherits="Transaction_TrSTOpname_TrSTOpname" %>

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
        document.getElementById("tbQtySystem").value = setdigit(document.getElementById("tbQtySystem").value.replace(/\$|\,/g,""),'<%=ViewState("DigitQty")%>');
        document.getElementById("tbQtyActual").value = setdigit(document.getElementById("tbQtyActual").value.replace(/\$|\,/g,""),'<%=ViewState("DigitQty")%>');
        document.getElementById("tbQtyOpname").value = setdigit(document.getElementById("tbQtyOpname").value.replace(/\$|\,/g,""),'<%=ViewState("DigitQty")%>');
        }catch (err){
            alert(err.description);
          }      
        } 
        function setformatdt()
        {
        try
         {         
        var TQtySystem = document.getElementById("tbQtySystem").value.replace(/\$|\,/g,""); 
        var TQtyActual = document.getElementById("tbQtyActual").value.replace(/\$|\,/g,"");
        var TQtyOpname = document.getElementById("tbQtyOpname").value.replace(/\$|\,/g,"");                             
                
        document.getElementById("tbQtySystem").value = setdigit(TQtySystem,'<%=ViewState("DigitQty")%>');
        document.getElementById("tbQtyActual").value = setdigit(TQtyActual,'<%=ViewState("DigitQty")%>');
        document.getElementById("tbQtyOpname").value = setdigit(TQtyOpname,'<%=ViewState("DigitQty")%>');
                
        }catch (err){
            alert(err.description);
          }      
        }     
    </script>
    
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
 <link href="https://fonts.googleapis.com/css2?family=Muli:wght@300;400;600&display=swap" rel="stylesheet">
<%--    <style type="text/css">
        .style1
        {
            width: 1500px;
        }
    </style>--%>
</head>
<body>     
    <form id="form1" runat="server">    
    <div class="Content">
    <div class="H1">Stock Opname</div>
     <hr style="color:Blue" />        
    <asp:Panel runat="server" id="PnlHd"> 
      <table>
        <tr>
            <td style="width:100px;text-align:right">Quick Search :</td>            
            <td><asp:TextBox CssClass="TextBox" runat="server" ID ="tbFilter"/> 
                  <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlField" >
                      <asp:ListItem Value="Reference" Selected="True">Reference</asp:ListItem>
                      <asp:ListItem Value="dbo.FormatDate(TransDate)">Date</asp:ListItem>
                      <asp:ListItem>Status</asp:ListItem>
                      <asp:ListItem Value="dbo.FormatDate(PositionDate)">Position Date</asp:ListItem>
                      <asp:ListItem Value="Warehouse_Name">Warehouse</asp:ListItem>
                      <asp:ListItem Value="Product">Product Code</asp:ListItem>
                      <asp:ListItem Value="Product_Name">Product Name</asp:ListItem>
                      <asp:ListItem Value="Specification">Specification</asp:ListItem>
                      <asp:ListItem Value="Subled_Name">Subled</asp:ListItem>
                      <asp:ListItem Value="Remark">Remark</asp:ListItem>
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
                  <asp:ListItem Value="Reference" Selected="True">Reference</asp:ListItem>
                  <asp:ListItem Value="dbo.FormatDate(TransDate)">Date</asp:ListItem>
                  <asp:ListItem>Status</asp:ListItem>
                  <asp:ListItem Value="dbo.FormatDate(PositionDate)">Position Date</asp:ListItem>
                  <asp:ListItem Value="Warehouse_Name">Warehouse</asp:ListItem>
                  <asp:ListItem Value="Product">Product Code</asp:ListItem>
                  <asp:ListItem Value="Product_Name">Product Name</asp:ListItem>
                  <asp:ListItem Value="Specification">Specification</asp:ListItem>
                  <asp:ListItem Value="Subled_Name">Subled</asp:ListItem>
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
                  <asp:BoundField DataField="Reference" HeaderStyle-Width="120px" 
                      SortExpression="Nmbr" HeaderText="Reference">
                      <HeaderStyle Width="120px" />
                  </asp:BoundField>                  
                  <asp:BoundField DataField="Status" HeaderText="Status" SortExpression="Status"></asp:BoundField>                  
                  <asp:BoundField DataField="TransDate" HtmlEncode="true" DataFormatString="{0:dd MMM yyyy}" HeaderStyle-Width="80px" 
                      HeaderText="Date" SortExpression="TransDate">
                      <HeaderStyle Width="80px" />
                  </asp:BoundField>
                  <asp:BoundField DataField="PositionDate" HtmlEncode="true" DataFormatString="{0:dd MMM yyyy}" HeaderText="Position Date" 
                      SortExpression="PositionDate" >
                      <HeaderStyle Width="80px" />
                  </asp:BoundField>
                  <asp:BoundField DataField="Warehouse_Name" 
                      HeaderText="Warehouse " SortExpression="Warehouse_Name">
                  </asp:BoundField>
                
                  <asp:BoundField DataField="Operator" HeaderText="Operator" 
                      SortExpression="Operator" />
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
            <td>Reference</td>
            <td>:</td>
            <td><asp:TextBox CssClass="TextBox" Enabled = "false" runat="server" ID="tbRef" Width="149px"/> 
            </td>            

            <td>
                &nbsp;</td>

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
            </td>                        

            <td>
                &nbsp;</td>

        </tr>      
        <tr>
            <td>Position Date</td>
            <td>:</td>
            <td>
                <BDP:BasicDatePicker ID="tbPositionDate" runat="server" AutoPostBack="True" 
                    ButtonImageHeight="19px" ButtonImageWidth="20px" DateFormat="dd MMM yyyy" 
                    DisplayType="TextBoxAndImage" TextBoxStyle-CssClass="TextDate" 
                    ValidationGroup="Input" ShowNoneButton="False"><TextBoxStyle CssClass="TextDate" /></BDP:BasicDatePicker>                    
            </td>
            

            
            <td>
                &nbsp;</td>
            

            
        </tr>
          <tr>
              <td>
                  Warehouse</td>
              <td>
                  :</td>
              <td>
                  <asp:DropDownList ID="ddlWrhs" runat="server" AutoPostBack="true" 
                      CssClass="DropDownList" Height="16px" ValidationGroup="Input" Width="200px">
                  </asp:DropDownList>
                  
                  <asp:TextBox ID="tbFgSubled" runat="server" AutoPostBack="true" 
                      CssClass="TextBox" Visible="False" />                                   
              </td>

              <td>
                  &nbsp;</td>

          </tr>

          <tr>
              <td>
                  Subled</td>
              <td>
                  :</td>
              <td>
                  <asp:TextBox ID="tbSubled" runat="server" CssClass="TextBox" Width = "30" />
                  <asp:TextBox ID="tbSubledName" runat="server" CssClass="TextBox" 
                      Enabled="False" Width="200px" />                      
                  <asp:Button Class="btngo" ID="btnSubled" Text="..." runat="server" />                                     
               
              </td>
              
              <td>
                  &nbsp;</td>
              
          </tr>
          <tr>
              <td>
                  Operator</td>
              <td>
                 :</td>
              <td>
                  <asp:TextBox ID="tbOperator" runat="server" CssClass="TextBox" 
                      ValidationGroup="Input" Width="236px" />
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
                  <asp:TextBox ID="tbRemark" runat="server" CssClass="TextBox" 
                      ValidationGroup="Input" Width="236px" />
                                          
                  <asp:Button ID="btnGetDtNon" runat="server" Class="bitbtndt btngetitem" 
                      Text="From Product" ValidationGroup="Input" width="100px" />
                  <asp:Button ID="btnXML" runat="server" Class="bitbtndt btngetitem" 
                      Text="Get Last Data" Visible="False" Width="100px" />
                  <asp:Button ID="btnGetDt" runat="server" Class="bitbtndt btngetitem" 
                      Text="From Opname" Visible="False" width="100px" />
                                          
              </td>

              <td>
                  &nbsp;</td>

          </tr>
          <tr>
            <td> &nbsp;</td>
            <td>&nbsp;</td>
            <td>
              <%--<td colspan="3">--%>
          
              </td>
              <td>
                  &nbsp;</td>
          </tr>
      </table>  
      <asp:Panel ID="PnlInfo" runat="server" BorderColor="Black" BorderStyle="Solid" 
            Height="100%" Width="700px">
            &nbsp;
            <%--</asp:Panel></td>--%>
            <%-- <td class="style1">
                  <asp:Panel ID="Panel1" runat="server" BorderColor="Black" BorderStyle="Solid" 
            Height="100%" Width="252px">--%> &nbsp;
            <td>
                &nbsp;&nbsp;</td>
            <%-- </asp:Panel></td>--%>
            <%--<td class="style1">
                  <asp:Panel ID="Panel2" runat="server" BorderColor="Black" BorderStyle="Solid" 
                      Height="100%" Width="83px">--%>
            <asp:Label ID="Label2" runat="server" Font-Bold="True" ForeColor="Blue" 
                Text="Delete Selected Item"></asp:Label>
            &nbsp;&nbsp;<asp:Button Class="bitbtndt btngo" Width = "70"  ID="btnProcessDel" 
                Text="Process" runat="server" ValidationGroup="Input"  />                                     
            <br />
                      
                      
                  </asp:Panel>
        
      
      <br />      
      <div style="font-size:medium; color:Blue;">Detail<br />
          
        </div>
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
                <asp:MenuItem Text="Detail " Value="0"></asp:MenuItem>
                <asp:MenuItem Text="Import Excel" Value="1"></asp:MenuItem>
            </Items>            
        </asp:Menu>
      <asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex="0">
           <asp:View ID="Tab1" runat="server">
        <asp:Panel runat="server" ID="pnlDt">
            <asp:Button class="bitbtn btnadd" runat="server" ID="btnAddDt" Text="Add" ValidationGroup="Input" />	        
            <div style="border:0px  solid; width:100%; height:100%; overflow:auto;">
                <asp:GridView ID="GridDt" runat="server" AutoGenerateColumns="False" AllowSorting="True"
                    ShowFooter="True">
                    <HeaderStyle CssClass="GridHeader" />
                    <RowStyle CssClass="GridItem" Wrap="false" />
                    <AlternatingRowStyle CssClass="GridAltItem" />
                    <PagerStyle CssClass="GridPager" />
                    <Columns>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:CheckBox ID="cbSelectHd" runat="server" AutoPostBack="true" 
                                    oncheckedchanged="cbSelectHd_CheckedChanged1" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="cbSelect" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
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
                            HeaderText="Specification" SortExpression="Specification" FooterStyle-HorizontalAlign="Right" >                            
                            <HeaderStyle Width="200px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="QtySystem" 
                            HeaderText="Qty System" SortExpression="QtySystem" 
                            DataFormatString="{0:#,##0.00}" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" >                            
                            <HeaderStyle Width="80px" />
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:BoundField>
                        <asp:BoundField DataField="QtyActual" DataFormatString="{0:#,##0.00}" ItemStyle-HorizontalAlign="Right"   
                            HeaderText="Qty Opname" SortExpression="QtyActual" FooterStyle-HorizontalAlign="Right"  >
                            <HeaderStyle Width="80px" />
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:BoundField>
                        <asp:BoundField DataField="QtyOpname" HeaderText="Qty Selisih" 
                            DataFormatString="{0:#,##0.00}" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" 
                            SortExpression="QtyOpname" >
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Unit" HeaderText="Unit"
                            SortExpression="Unit" HeaderStyle-Width="150px" >
                            <HeaderStyle Width="80px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Remark" HeaderText="Remark" 
                            HeaderStyle-Width="80px" >
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
                        <asp:TextBox ID="tbCode" runat="server" AutoPostBack="true" 
                            CssClass="TextBox" Width="136px" />
                        <asp:TextBox ID="tbName" runat="server" CssClass="TextBox" Enabled="False" 
                            EnableTheming="True" ReadOnly="True" Width="200px" />
                        <asp:Button Class="btngo" ID="btn1" Text="..." runat="server" />                                     

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
                            <td>Qty</td>
                            <td>:</td>
                            <td>
                                <table cellspacing="0" cellpadding="0">
                                    <tr style="background-color:Silver;text-align:center">
                                        <td>System
                                        </td>
                                        <td>
                                            Opname</td>
                                        <td>
                                            Selisih</td>
                                    </tr>
                                    <tr>
                                        <td>
                                        <asp:TextBox CssClass="TextBoxR" runat="server" ID="tbQtySystem" Width="80px" 
                                                Enabled="False" /></td>
                                        <td>
                                            <asp:TextBox ID="tbQtyActual" runat="server" AutoPostBack="True" 
                                                CssClass="TextBox" Width="80px" />
                                        </td>
                                        <td>
                                            <asp:TextBox ID="tbQtyOpname" runat="server" CssClass="TextBoxR" 
                                                Enabled="False" Width="80px" />
                                        </td>        
                                    </tr>
                                </table>
                            </td>
                        </tr>
                
                <tr>
                    <td>
                        Unit</td>
                    <td>
                        :</td>
                    <td>
                        <asp:TextBox ID="tbUnit" runat="server" CssClass="TextBoxR" Enabled="False" 
                            Width="75px" />
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
        
        </asp:View>
        
        <asp:View ID="Tab2" runat="server">
                
              <table>
        <tr>            
            <td style="width:150px;text-align:right"><b>Please Select Excel File : </b></td> 
            <td>                
                <asp:FileUpload ID="fileuploadExcel" runat="server" />&nbsp;  
                <asp:Button class="bitbtn btnpreview" Width="98px" runat="server" ID="btnUploadExcel" Text="Upload" />
            </td>
        </tr>
        <tr>
            <td style="width:150px;text-align:right"><b>Display Data : </b></td> 
            <td>
                <asp:DropDownList ID="ddlSheets" runat="server" Width="200px" AutoPostBack="true" CssClass="DropDownList"
                    AppendDataBoundItems = "true">
                </asp:DropDownList>
            </td>             
        </tr>        
      </table>      
                <br />  
          <asp:Panel runat="server" ID="pnlImport">
                <%--<fieldset style="width:800px">--%>
                 
                <u><b>Import Data Opname</b></u><table>
                <tr>
                    <td>Product Code</td>
                    <td>:</td>
                    <td><asp:DropDownList ID="ddlFindProductCode" runat="server" Width="140px" CssClass="DropDownList" AppendDataBoundItems = "true"/></td>
                    <td>Remark</td>
                    <td>:</td>
                    <td>
                        <asp:DropDownList ID="ddlFindRemark" runat="server" AppendDataBoundItems="true" 
                            CssClass="DropDownList" Width="140px" />
                    </td>
                    <td><asp:Button ID="btnGenerate" runat="server" class="bitbtndt btnok" Width="110px" Text="Import To System" />                         
                    </td>
                </tr> 
                <tr>
                    <td>Qty Opname</td>
                    <td>:</td>
                    <td><asp:DropDownList ID="ddlFindQtyOpname" runat="server" Width="140px" CssClass="DropDownList" AppendDataBoundItems = "true"/></td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                    <td>
                        &nbsp;</td>
                    <td></td>
                    <td></td>
                    <td></td>
                    
                </tr>
                </table>
            </asp:Panel> 
           <br />       
          <asp:Panel runat="server" id="Panel1">          
          <div style="border:0px  solid; width:100%; height:100%; overflow:auto;">
          <asp:GridView ID="GridExcel" Width="1000" runat="server" AllowPaging="False" 
            CssClass="Grid" AutoGenerateColumns="true"> 
              <HeaderStyle CssClass="GridHeader" Wrap = "false"></HeaderStyle>
						<RowStyle CssClass="GridItem" Wrap="false" />
						<AlternatingRowStyle CssClass="GridAltItem"/>
						<PagerStyle CssClass="GridPager" />
              <Columns>              
                  				  	
					
              </Columns>
          </asp:GridView>
          </div>   
          </asp:Panel>
           
               
            </asp:View> 
        </asp:MultiView> 
        							                                                           
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
