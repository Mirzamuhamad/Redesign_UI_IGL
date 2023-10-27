<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TrStAdjust.Aspx.vb" Inherits="TrStAdjust" %>
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
        var Qty = document.getElementById("tbQty").value.replace(/\$|\,/g,""); 
//        var PriceForex = document.getElementById("tbPrice").value.replace(/\$|\,/g,"");
//        var AmountForex = document.getElementById("tbAmountForex").value.replace(/\$|\,/g,"");        
        document.getElementById("tbQty").value = setdigit(Qty,'<%=ViewState("DigitQty")%>');
//        document.getElementById("tbPrice").value = setdigit(PriceForex,'<%=ViewState("DigitHome")%>');
//        document.getElementById("tbAmountForex").value = setdigit(AmountForex,'<%=ViewState("DigitHome")%>');
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
    <div class="H1">Stock Adjustment</div>
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
                      <asp:ListItem Value="Wrhs_Name">Warehouse</asp:ListItem>
                      <asp:ListItem Value="Location_Name">Location</asp:ListItem>
                      <asp:ListItem Value="Operator">Operator</asp:ListItem>
                      <asp:ListItem Value="OpnameNo">Opname No</asp:ListItem>
                      <asp:ListItem>Remark</asp:ListItem>
                  </asp:DropDownList>
                  <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlRange"></asp:DropDownList>
                  <asp:Button class="bitbtn btnsearch" runat="server" ID="btnSearch" Text="Search" />																						 											  
                  <asp:Button class="bitbtn btngo" runat="server" ID="btnExpand" Text="..." />																						 											                    
                  
            </td>
            <td>
                <asp:LinkButton ID="LbAdvSearch" runat="server" Text="Advanced Search" />
            </td>
             <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                Show Records</td>
            <td>
                :</td>
            <td>
                <asp:DropDownList ID="ddlRow" runat="server" AutoPostBack="True" 
                    CssClass="DropDownList">
                    <asp:ListItem>20</asp:ListItem>
                    <asp:ListItem>30</asp:ListItem>
                    <asp:ListItem>40</asp:ListItem>
                    <asp:ListItem>50</asp:ListItem>
                    <asp:ListItem>100</asp:ListItem>
                    <asp:ListItem>200</asp:ListItem>
                    <asp:ListItem>300</asp:ListItem>
                </asp:DropDownList></td>
            <td>
                Rows</td>          
        </tr>  
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
                      <asp:ListItem Value="Wrhs_Name">Warehouse</asp:ListItem>
                      <asp:ListItem Value="Location_Name">Location</asp:ListItem>
                      <asp:ListItem Value="Operator">Operator</asp:ListItem>
                      <asp:ListItem Value="OpnameNo">Opname No</asp:ListItem>
                      <asp:ListItem>Remark</asp:ListItem>
              </asp:DropDownList>
          </td>    
          
               
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
                  <asp:BoundField DataField="TransNmbr" HeaderStyle-Width="120px" SortExpression="Nmbr" HeaderText="Reference">
                      <HeaderStyle Width="120px" />
                  </asp:BoundField>                  
                  <asp:BoundField DataField="Status" HeaderText="Status"></asp:BoundField>                  
                                    <asp:BoundField DataField="TransDate" HtmlEncode="true" DataFormatString="{0:dd MMM yyyy}" HeaderStyle-Width="80px" SortExpression="TransDate" HeaderText="Date">
                      <HeaderStyle Width="80px" />
                  </asp:BoundField>
                  <asp:BoundField DataField="Wrhs_Name" HeaderStyle-Width="200px" 
                      HeaderText="Warehouse" SortExpression="Wrhs_Name">
                      <HeaderStyle Width="200px" />
                  </asp:BoundField>
                  <asp:BoundField DataField="FromOpname" HeaderStyle-Width="50px" 
                      SortExpression="FromOpname" HeaderText="From Opname">
                      <HeaderStyle Width="50px" />
                  </asp:BoundField>
                  <asp:BoundField DataField="Operator" HeaderStyle-Width="200px" 
                      SortExpression="Operator" HeaderText="Operator">
                      <HeaderStyle Width="200px" />
                  </asp:BoundField>
                  <asp:BoundField DataField="Remark" HeaderStyle-Width="250px" 
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
                </td>         
            </tr> 
            <tr>
                <td><asp:LinkButton ID="lbWarehouse" ValidationGroup="Input" runat="server" Text="Warehouse"/></td>
                <td>:</td>
                <td><asp:DropDownList CssClass="DropDownList" ValidationGroup="Input" runat="server" ID="ddlwrhs" AutoPostBack="true" Width="200px" />
                    <asp:TextBox runat="server" ID="tbFgSubLed" Visible="false"/>                
                </td>                  
            </tr>  
            <tr>
                <td>SubLed</td>
                <td>:</td>
                <td><asp:TextBox CssClass="TextBox" ValidationGroup="Input" runat="server" ID="tbSubLed" AutoPostBack="true" /> 
                    <asp:TextBox CssClass="TextBox" runat="server" ID="tbSubLedName" enabled="false" Width="225px"/>
                    <asp:Button Class="btngo" ID="btnSubLed" Text="..." runat="server" ValidationGroup="Input" />                                  
                    
                </td>                   
            </tr>  
            <tr>
                <td>Operator</td>
                <td>:</td>
                <td><asp:TextBox runat="server" ValidationGroup="Input" ID="tbOperator" CssClass="TextBox" Width="200px"/> &nbsp &nbsp &nbsp 
                    </td>
            </tr>                        
            <tr>
                <td>
                    From Opname</td>
                <td>
                    :</td>
                <td>
                    <asp:DropDownList ID="ddlFromOpname" runat="server" CssClass="DropDownList" 
                        Height="16px" ValidationGroup="Input" Width="44px">
                        <asp:ListItem>Y</asp:ListItem>
                        <asp:ListItem>N</asp:ListItem>
                    </asp:DropDownList>
                    &nbsp &nbsp &nbsp &nbsp &nbsp
                    <asp:label runat="server" Id ="lbOpnameMinus" Text = "Get Data "></asp:label>
                    <asp:DropDownList ID="ddlReffType" runat="server" CssClass="DropDownList" 
                        Height="16px" ValidationGroup="Input" Width="44px">
                        <asp:ListItem Selected="True">+</asp:ListItem>
                        <asp:ListItem>-</asp:ListItem>
                    </asp:DropDownList>
                    <asp:label runat="server" Id ="Label1" Text = " from Opname : "></asp:label>
                      <asp:TextBox CssClass="TextBox" runat="server" ID="tbOpnameMinus" Enabled="false" Width="125px"/>
                      <asp:Button Class="btngo" ID="btnOpnameMinus" Text="..." runat="server" ValidationGroup="Input" />                                                                 
                      
                </td>                
            </tr>
            <tr>
                <td>Remark</td>
                <td>:</td>
                <td><asp:TextBox runat="server" ValidationGroup="Input" ID="tbRemark" CssClass="TextBox" Width="225px"/>
                    &nbsp;<asp:Button ID="btnGetDt" runat="server" Class="btngo" Text="From Opname" 
                        ValidationGroup="Input" Width="88px" />
                    <asp:Button ID="btnGetDtNon" runat="server" Class="btngo" Text="From Product" 
                        ValidationGroup="Input" Width="100px" />
                </td>               
            </tr>
          </table>  
      <br />      
      <div style="font-size:medium; color:Blue;">Detail</div>
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
                            <%--<FooterTemplate>
                            <asp:ImageButton ID="btnAddDt2" ValidationGroup="Input" runat="server"  
                                    ImageUrl="../../Image/btnAddDtOn.png"
                                    onmouseover="this.src='../../Image/btnAddDtOff.png';"
                                    onmouseout="this.src='../../Image/btnAddDtOn.png';"
                                    ImageAlign="AbsBottom" />        
                            </FooterTemplate>--%>                            
                        </asp:TemplateField>
                        <asp:BoundField DataField="OpnameNo" HeaderText="Opname No" />
                        <asp:BoundField DataField="Product" HeaderStyle-Width="120px" 
                            HeaderText="Product Code" >
                            <HeaderStyle Width="120px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Product_Name" HeaderStyle-Width="250px"  
                            HeaderText="Product Name" >
                            <HeaderStyle Width="250px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Specification" HeaderText="Specification" />
                       <asp:BoundField DataField="FgOperator" HeaderStyle-Width="10px"  
                            HeaderText="Adjust" ItemStyle-HorizontalAlign="Center" >
                            <HeaderStyle Width="10px" />
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Qty" HeaderStyle-Width="80px" 
                            HeaderText="Qty " ItemStyle-HorizontalAlign ="Right" 
                            DataFormatString="{0:#,##0.##}" FooterStyle-HorizontalAlign="Right" >
                            <FooterStyle HorizontalAlign="Right" />
                            <HeaderStyle Width="80px" />
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Unit" HeaderStyle-Width="80px" HeaderText="Unit " >
                            <HeaderStyle Width="80px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="PriceCost" HeaderText="Price Cost" 
                            DataFormatString="{0:#,##0.##}" FooterStyle-HorizontalAlign="Right" 
                            ItemStyle-HorizontalAlign="Right" >                                                                        
                            <FooterStyle HorizontalAlign="Right" />
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:BoundField>
                        <asp:BoundField DataField="TotalCost" 
                            HeaderText="Total Cost" ItemStyle-HorizontalAlign ="Right" 
                            DataFormatString="{0:#,##0.##}" FooterStyle-HorizontalAlign="Right" >
                            <FooterStyle HorizontalAlign="Right" />
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Remark" HeaderStyle-Width="250px" 
                            ItemStyle-HorizontalAlign="Left" FooterStyle-HorizontalAlign="Left" 
                            HeaderText="Remark" >
                            <FooterStyle HorizontalAlign="Left" />
                            <HeaderStyle Width="250px" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                    </Columns>
                </asp:GridView>
          </div>   
          <asp:Button class="bitbtn btnadd" runat="server" ID="btnAddDt2" Text="Add" ValidationGroup="Input" />	
                 
       </asp:Panel>             
       <asp:Panel runat="server" ID="pnlEditDt" Visible="false">
        <table width="100%">
            <tr>
                <td style="width:60%">
                    <table>                
                        <tr>
                            <td>Opname No</td>
                            <td>:</td>
                            <td>
                                <asp:TextBox ID="tbOpnameNo" runat="server" CssClass="TextBox" Enabled="false" 
                                    Width="150px" />
                                <asp:Button Class="btngo" ID="btnOpnameNo" Text= "..." runat="server" Visible ="false"  />                                              
                                
                            </td>                               
                        </tr>
                         <tr>
                            <td>
                                <asp:LinkButton ID="lbProduct" runat="server" Text="Product" />
                             </td>
                            <td>:</td>
                            <td><asp:TextBox runat="server" ID="tbProductCode" 
                                    CssClass="TextBox"  Width="151px" AutoPostBack="true"/>
                                <asp:TextBox ID="tbProductName" runat="server" CssClass="TextBox" 
                                    Enabled="false" EnableTheming="True" Width="200px" />
                                <asp:Button Class="btngo" ID="btnProduct" Text= "..." runat="server" />                                                  
                                
                                <asp:Label ID="Label4" runat="server" ForeColor="Red">*</asp:Label>
                                
                             </td>
                        </tr>

                        <tr>
                            <td>Specification</td>
                            <td>:</td>
                            <td>
                                <asp:TextBox ID="tbSpecification" runat="server" CssClass="TextBoxR" 
                                    Enabled="false" ValidationGroup="Input" Width="351px" />
                            </td>
                        </tr>                                                    
                        
                        <tr>
                            <td>Qty</td>
                            <td>:</td>
                            <td>
                                <table cellspacing="0" cellpadding="0">
                                    <tr style="background-color:Silver;text-align:center">
                                        <td>Qty
                                            <asp:Label ID="Label5" runat="server" ForeColor="Red">*</asp:Label>
                                        </td>
                                        <td>Unit 
                                            <asp:Label ID="Label6" runat="server" ForeColor="Red">*</asp:Label>
                                        </td>
                                        
                                    </tr>
                                    <tr>
                                        <td><asp:DropDownList ID="ddlFgOperator" runat="server" AutoPostBack="True" 
                                            CssClass="DropDownList">
                                            <asp:ListItem Selected="True" Value="+">+</asp:ListItem>
                                            <asp:ListItem Value="-">-</asp:ListItem>
                                            </asp:DropDownList>
                                        <asp:TextBox CssClass="TextBox" runat="server" ID="tbQty" 
                                                AutoPostBack="true" Width="80px" /></td>
                                        <td>
                                            <asp:TextBox ID="tbUnit" runat="server" Width="75px" CssClass="TextBox" Enabled="false" />
                                        </td>
                                        
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                        <td>Cost Center</td>
                        <td>:</td>
                        <td>
                            <asp:DropDownList ID="ddlCostCtr" runat="server" CssClass="DropDownList" Width="200px">
                            </asp:DropDownList> 
                            <asp:Label ID="Label2" runat="server" ForeColor="Red">*</asp:Label>
                        </td>
                        </tr>
                        <%--<tr>
                            <td>Qty</td>
                            <td>:</td>
                            <td>
                                <asp:DropDownList ID="ddlFgOperator" runat="server" AutoPostBack="True" 
                                    CssClass="DropDownList">
                                    <asp:ListItem Selected="True" Value="+">+</asp:ListItem>
                                    <asp:ListItem Value="-">-</asp:ListItem>
                                </asp:DropDownList>
                                <asp:TextBox ID="tbQty" runat="server" 
                                    CssClass="TextBox" AutoPostBack="True" />
                            </td>
                        </tr>--%>                              
                        <tr>
                            <td>
                                Price Cost</td>
                            <td>
                                &nbsp;</td>
                            <td>
                                <asp:TextBox ID="tbPrice" runat="server" CssClass="TextBox" Enabled="false" 
                                    AutoPostBack="True" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Total Cost</td>
                            <td>
                                &nbsp;</td>
                            <td>
                                <asp:TextBox ID="tbTotal" runat="server" CssClass="TextBoxR" Enabled="False" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Remark
                            </td>
                            <td>
                                :</td>
                            <td>
                                <asp:TextBox ID="tbRemarkDt" runat="server" CssClass="TextBox" Width="250px" />
                            </td>
                        </tr>
                    </table>
                </td>
                <td style="vertical-align:top;width:40%">
				    &nbsp;</td>
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
                 
                <u><b>Import Data Adjust</b></u><table>
                <tr>
                    <td>Product Code</td>
                    <td>:</td>
                    <td><asp:DropDownList ID="ddlFindProductCode" runat="server" Width="140px" CssClass="DropDownList" AppendDataBoundItems = "true"/></td>
                    <td colspan="3"><asp:Button ID="btnGenerate" runat="server" class="bitbtndt btnok" Width="110px" Text="Import To System" />                         
                    </td>
                </tr> 
                <tr>
                    <td>Qty Wrhs</td>
                    <td>:</td>
                    <td><asp:DropDownList ID="ddlFindQty" runat="server" Width="139px" 
                            CssClass="DropDownList" AppendDataBoundItems = "true"/></td>
                    <td>&nbsp;</td>
                    <td>
                        &nbsp;</td>
                    <td>
                        &nbsp;</td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                    
                </tr>
                <tr>
                    <td>Adjust</td>
                    <td>:</td>
                    <td>
                        <asp:DropDownList ID="ddlFindAdjust" runat="server" AppendDataBoundItems="true" 
                            CssClass="DropDownList" Width="140px" />
                    </td>
                    <td>&nbsp;</td>
                    <td>
                        &nbsp;</td>
                    <td>
                        &nbsp;</td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                    
                </tr>              
                    <tr>
                        <td>
                            Cost Center</td>
                        <td>
                            :</td>
                        <td>
                            <asp:DropDownList ID="ddlFindCostCtr" runat="server" 
                                AppendDataBoundItems="true" CssClass="DropDownList" Width="140px" />
                        </td>
                        <td>
                            Remark</td>
                        <td>
                            :</td>
                        <td>
                            <asp:DropDownList ID="ddlFindRemark" runat="server" AppendDataBoundItems="true" 
                                CssClass="DropDownList" Width="140px" />
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
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
