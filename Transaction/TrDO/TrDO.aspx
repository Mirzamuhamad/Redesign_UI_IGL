<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TrDO.aspx.vb" Inherits="Transaction_TrDO_TrDO" %>
<%@ Register Assembly="BasicFrame.WebControls.BasicDatePicker" Namespace="BasicFrame.WebControls"
    TagPrefix="BDP" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Untitled Page</title>
    <script src="../../Function/OpenDlg.js" type="text/javascript"></script>
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
	    x2 = x.length > 1 ? '.' + x[1 : '';
	    var rgx = /(\d+)(\d{3})/;
	    while (rgx.test(x1)) {
		    x1 = x1.replace(rgx, '$1' + ',' + '$2');
	    }
	    return x1 + x2;
	    }catch (err){
            alert(err.description);
          }  
        }
    
    function closing()
        {
            try
            {
                var result = prompt("Remark Close", "");
                if (result){
                    document.getElementById("HiddenRemarkClose").value = result;
                } else {
                    document.getElementById("HiddenRemarkClose").value = "False Value";
                }
                postback();
                //document.form1.submit();                
            }catch(err){
                  alert(err.description);
            }        
        }
        
        function setformatdt()
        {
         try
         {  
            document.getElementById("tbQtyM2").value = setdigit(document.getElementById("tbQtyM2").value.replace(/\$|\,/g,""),'<%=VIEWSTATE("DigitQty")%>');                    
//            document.getElementById("tbQtyRoll").value = setdigit(document.getElementById("tbQtyRoll").value.replace(/\$|\,/g,""),'<%=VIEWSTATE("DigitQty")%>');
            document.getElementById("tbQtyRoll").value = setdigit(document.getElementById("tbQtyRoll").value.replace(/\$|\,/g,""),'#,##0');
            document.getElementById("tbQtyWrhs").value = setdigit(document.getElementById("tbQtyWrhs").value.replace(/\$|\,/g,""),'<%=VIEWSTATE("DigitQty")%>');
            document.getElementById("tbQtyOrder").value = setdigit(document.getElementById("tbQtyOrder").value.replace(/\$|\,/g,""),'<%=VIEWSTATE("DigitQty")%>');
            document.getElementById("tbPrice").value = setdigit(document.getElementById("tbPrice").value.replace(/\$|\,/g,""),'<%=VIEWSTATE("DigitQty")%>');            
            
        }catch (err){
            alert(err.description);
          }      
        }
    
    function postback()
        {
            __doPostBack('','');
        }
    
    </script>    
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
 <link href="https://fonts.googleapis.com/css2?family=Muli:wght@300;400;600&display=swap" rel="stylesheet">
</head>
<body>
    <form id="form1" runat="server">
    <div class="Content">
        <div class="H1">Delivery Order</div>
        <hr style="color:Blue" />
        <asp:Panel runat="server" id="PnlHd"> 
      <table>
        <tr>
            <td style="width:100px;text-align:right">Quick Search :</td>            
            <td><asp:TextBox CssClass="TextBox" runat="server" ID ="tbFilter"/> 
                  <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlField" >
                    <asp:ListItem Selected="True" Value="TransNmbr">DO No</asp:ListItem>
                    <asp:ListItem Value="dbo.FormatDate(TransDate)">DO Date</asp:ListItem>
                    <asp:ListItem>Revisi</asp:ListItem>
                    <asp:ListItem>Status</asp:ListItem>
                    <asp:ListItem Value="Customer">Customer Code</asp:ListItem>
                    <asp:ListItem Value="Customer_Name">Customer Name</asp:ListItem>                                        
                    <asp:ListItem Value="SONo">SO No</asp:ListItem>
                    <asp:ListItem Value="CustPONo">PO Cust No</asp:ListItem>                    
                    <asp:ListItem Value="FgNeedDelivery">Need Delivery</asp:ListItem>
                    <asp:ListItem Value="DeliveryCode">Delivery Code</asp:ListItem>
                    <asp:ListItem Value="DeliveryName">Delivery Name</asp:ListItem>
                    <asp:ListItem Value="DeliveryAddr">Delivery Addr</asp:ListItem>
                    <asp:ListItem Value="DeliveryCity">Delivery City</asp:ListItem>
                    <asp:ListItem Value="dbo.FormatDate(DeliveryDate)">Delivery Date</asp:ListItem>
                    <asp:ListItem Value="DeliveryHour">Delivery Hour</asp:ListItem>
                    <asp:ListItem Value="Remark">Remark</asp:ListItem>                     
                    </asp:DropDownList>
                  <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlRange"></asp:DropDownList> 
                  <asp:Button class="bitbtn btnsearch" runat="server" ID="btnSearch" Text="Search" />                  
                  <asp:Button class="btngo" runat="server" ID="btnExpand" Text="..."/>  
            </td>
            <td>
                <asp:LinkButton ID="LbAdvSearch" runat="server" Text="Advanced Search" />
                &nbsp &nbsp &nbsp &nbsp
                <asp:Label runat="server" ID="Label1" Text="Outstanding SO : "/>
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
                  <asp:ListItem Selected="True" Value="TransNmbr">DO No</asp:ListItem>
                    <asp:ListItem Value="dbo.FormatDate(TransDate)">DO Date</asp:ListItem>
                    <asp:ListItem>Revisi</asp:ListItem>
                    <asp:ListItem>Status</asp:ListItem>
                    <asp:ListItem Value="Customer">Customer Code</asp:ListItem>
                    <asp:ListItem Value="Customer_Name">Customer Name</asp:ListItem>                                        
                    <asp:ListItem Value="SONo">SO No</asp:ListItem>
                    <asp:ListItem Value="CustPONo">PO Cust No</asp:ListItem>                    
                    <asp:ListItem Value="FgNeedDelivery">Need Delivery</asp:ListItem>
                    <asp:ListItem Value="DeliveryCode">Delivery Code</asp:ListItem>
                    <asp:ListItem Value="DeliveryName">Delivery Name</asp:ListItem>
                    <asp:ListItem Value="DeliveryAddr">Delivery Addr</asp:ListItem>
                    <asp:ListItem Value="DeliveryCity">Delivery City</asp:ListItem>
                    <asp:ListItem Value="dbo.FormatDate(DeliveryDate)">Delivery Date</asp:ListItem>
                    <asp:ListItem Value="DeliveryHour">Delivery Hour</asp:ListItem>
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
            <asp:Button class="btngo" Visible="false" runat="server" ID="BtnGo" Text="G"/>
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
                          <asp:CheckBox ID="cbSelectHd" runat="server" AutoPostBack="true" oncheckedchanged="cbSelectHd_CheckedChanged" />
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
                              <asp:ListItem Text="Delete" />
                              <asp:ListItem Text="Revisi" />
                              <asp:ListItem Text="Print" />
                          </asp:DropDownList>
                          <asp:Button class="btngo" runat="server" ID="BtnGo" Text="G" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" CommandName="Go" />
                      </ItemTemplate>
                      <HeaderStyle Width="110px" />
                  </asp:TemplateField>
                  <asp:BoundField DataField="TransNmbr" HeaderStyle-Width="120px" SortExpression="Nmbr" HeaderText="DO No"></asp:BoundField>                  
                  <asp:BoundField DataField="Status" HeaderText="Status"></asp:BoundField>                  
                  <asp:BoundField DataField="Revisi" HeaderText="Revisi"></asp:BoundField>
                  <asp:BoundField DataField="TransDate" dataformatstring="{0:dd MMM yyyy}" htmlencode="true" HeaderStyle-Width="80px" SortExpression="TransDate" HeaderText="DO Date"></asp:BoundField>
                  <asp:BoundField DataField="Customer" HeaderStyle-Width="80px" SortExpression="Customer" HeaderText="Customer Code"></asp:BoundField>
                  <asp:BoundField DataField="Customer_Name" HeaderStyle-Width="200px" SortExpression="Customer_Name" HeaderText="Customer Name"></asp:BoundField>
                  <asp:BoundField DataField="SONo" HeaderStyle-Width="80px" SortExpression="SONo" HeaderText="SO No"></asp:BoundField>
                  <asp:BoundField DataField="SORev" HeaderStyle-Width="30px" SortExpression="SORev" HeaderText="SO Revisi"></asp:BoundField>
                  <asp:BoundField DataField="CustPONo" HeaderStyle-Width="80px" SortExpression="CustPONo" HeaderText="PO Cust No"></asp:BoundField>
                  <asp:BoundField DataField="FgNeedDelivery" HeaderStyle-Width="80px" SortExpression="FgNeedDelivery" HeaderText="Need Delivery"></asp:BoundField>
                  <asp:BoundField DataField="DeliveryCode" HeaderStyle-Width="200px" SortExpression="DeliveryCode" HeaderText="Delivery Code"></asp:BoundField>
                  <asp:BoundField DataField="DeliveryName" HeaderStyle-Width="200px" SortExpression="DeliveryName" HeaderText="Delivery Name"></asp:BoundField>
                  <%--<asp:BoundField DataField="DeliveryAddr" HeaderStyle-Width="200px" SortExpression="DeliveryAddr" HeaderText="Delivery Addr"></asp:BoundField>--%>
                  <asp:BoundField DataField="DeliveryCity" HeaderStyle-Width="200px" SortExpression="DeliveryCity" HeaderText="Delivery City"></asp:BoundField>
                  <asp:BoundField DataField="DeliveryDate" dataformatstring="{0:dd MMM yyyy}" htmlencode="true" HeaderStyle-Width="80px" SortExpression="DeliveryDate" HeaderText="Delivery Date"></asp:BoundField>
                  <asp:BoundField DataField="DeliveryHour" HeaderStyle-Width="200px" SortExpression="DeliveryHour" HeaderText="Delivery Hour"></asp:BoundField>
                  <asp:BoundField DataField="Remark" HeaderStyle-Width="250px" HeaderText="Remark"></asp:BoundField>
              </Columns>
          </asp:GridView>
          </div>
            <asp:Panel runat="server" ID ="pnlNav" Visible="false">
            <asp:Button class="bitbtn btnadd" runat="server" ID="btnAdd2" Text="Add" />
            &nbsp &nbsp &nbsp
            <asp:DropDownList ID="ddlCommand2" CssClass="DropDownList" runat="server"/>
            <asp:Button class="btngo" runat="server" ID="btnGo2" Text="G"/>  
            </asp:Panel>
    </asp:Panel>    
    <asp:Panel runat="server" ID="pnlInput" Visible="false">
      <table>
        <tr>
            <td>DO No</td>
            <td>:</td>
            <td><asp:TextBox CssClass="TextBoxR" runat="server" ID="tbCode" Width="150px" Enabled="False"/></td>        
            <td>DO Date</td>
            <td>:</td>
            <td>
            <BDP:BasicDatePicker ID="tbDate" runat="server" DateFormat="dd MMM yyyy" 
                        ReadOnly = "true" ValidationGroup="Input"
                        ButtonImageHeight="19px" ButtonImageWidth="20px" 
                        DisplayType="TextBoxAndImage" 
                        TextBoxStyle-CssClass="TextDate" 
                        ShowNoneButton="False"><TextBoxStyle CssClass="TextDate" /></BDP:BasicDatePicker>                
            </td>   
            <td>Revisi</td>
            <td>:</td>
            <td><asp:Label runat="server" ID="lbRevisi"></asp:Label></td>         
        </tr>
         
        <tr>
            <td>Customer</td>
            <td>:</td>
            <td colspan="4">
                <asp:TextBox CssClass="TextBox" ValidationGroup="Input" runat="server" ID="tbCustCode" MaxLength="12" AutoPostBack="true" />
                <asp:TextBox CssClass="TextBox" runat="server" ID="tbCustName" Enabled="false" MaxLength="60" Width="225px"/>
                <asp:Button Class="btngo" ID="btnCust" Text="..." runat="server" ValidationGroup="Input" />                                  
            </td>
        </tr>
        <tr>
            <td>SO No</td>
            <td>:</td>
            <td colspan="4"><asp:TextBox CssClass="TextBox" Enabled ="false" runat="server" ID="tbSONo" MaxLength="20" Width="150px" />
            <asp:TextBox CssClass="TextBox" Enabled ="false" runat="server" ID="tbSONoRev" MaxLength="20" Width="20px" />
                <asp:Button Class="btngo" ID="btnSONo" Text="..." runat="server" ValidationGroup="Input" />                                  
                
            </td>
        </tr> 
        <tr>
            <td>SO Contract 
            </td>
            <td>:</td>
            <td><asp:TextBox CssClass="TextBox" runat="server" MaxLength="30" ID="tbNoKontrakSO" Width="150px" Enabled="False"/></td>        
                   
        </tr> 
        <tr>
           <td> DO Contract
            </td>
            <td>:</td>
            <td><asp:TextBox CssClass="TextBox" runat="server" MaxLength="30" ID="tbNoKontrak" Width="150px" Visible= "True" Enabled="True"/></td>        
                   
        </tr> 
        <tr> 
            <td>PO Cust No</td>
            <td>:</td>
            <td><asp:TextBox CssClass="TextBox" Enabled ="false" runat="server" ID="tbCustPONo" MaxLength="20" Width="150px" />
            </td> 
            
            <td>Need Delivery</td>
            <td>:</td>
            <td><asp:DropDownList AutoPostBack="true" ValidationGroup="Input" CssClass="DropDownList" ID="ddlNeedDelivery" runat="server" >
                        <asp:ListItem Selected="True">Y</asp:ListItem>
                        <asp:ListItem>N</asp:ListItem>
                </asp:DropDownList>                    
            </td>            
        </tr>        
        <tr>
            <td>Delivery Place</td>
            <td>:</td>
            <td colspan="4">
                <asp:TextBox CssClass="TextBox" ValidationGroup="Input" runat="server" ID="tbDeliveryCode" MaxLength="12" AutoPostBack="true" />
                <asp:TextBox CssClass="TextBox" runat="server" ID="tbDeliveryName" Enabled="false" MaxLength="60" Width="225px"/>
                <asp:Button Class="btngo" ID="btnDelivery" Text="..." runat="server" ValidationGroup="Input" />                                  
            </td>
        </tr>
        <tr>
            <td>Address</td>
            <td>:</td>
            <td colspan="4"><asp:TextBox Width = "365px" runat="server" Enabled="false" ValidationGroup="Input" ID="tbDeliveryAddress" CssClass="TextBox" MaxLength="255" TextMode="MultiLine" />
            </td>
        </tr>
		
        <tr>
            <td>City</td>
            <td>:</td>
            <td Width="250px"><asp:DropDownList CssClass="DropDownList" runat="server" ValidationGroup="Input" Width = "250px" ID="ddlDeliveryCity" /></td>                       
            
            <td>SO Delivery Date</td>
            <td>:</td>
            <td><asp:DropDownList CssClass="DropDownList" runat="server" ValidationGroup="Input" AutoPostBack="true" Width = "150px" ID="ddlDeliveryDate" /></td>           
        </tr>
		
        <tr>                        
            <td> Est Delivery Date</td>
            <td>:</td>
            <td><BDP:BasicDatePicker ID="tbDeliveryDate" runat="server" DateFormat="dd MMM yyyy" 
                        ReadOnly = "true" ValidationGroup="Input"
                        ButtonImageHeight="19px" ButtonImageWidth="20px" 
                        DisplayType="TextBoxAndImage" 
                        TextBoxStyle-CssClass="TextDate" 
                        ShowNoneButton="False"><TextBoxStyle CssClass="TextDate" /></BDP:BasicDatePicker>
						s/d
						<BDP:BasicDatePicker ID="tbDeliveryDate2" runat="server" DateFormat="dd MMM yyyy" 
                        ReadOnly = "true" ValidationGroup="Input"
                        ButtonImageHeight="19px" ButtonImageWidth="20px" 
                        DisplayType="TextBoxAndImage" 
                        TextBoxStyle-CssClass="TextDate" 
                        ShowNoneButton="False"><TextBoxStyle CssClass="TextDate" /></BDP:BasicDatePicker>
						</td>                                  
            
            <td>Delivery Hour</td>
            <td>:</td>
            <td><asp:TextBox runat="server" ID="tbDeliveryHour" Width = "65px" ValidationGroup="Input"  MaxLength="5"  CssClass="TextBox" /></td>                       
        </tr>              
        
        <tr>
            <td>Remark</td>
            <td>:</td>
            <td colspan="4"><asp:TextBox Width = "365px" runat="server" ValidationGroup="Input" ID="tbRemark" CssClass="TextBox" MaxLength="255" TextMode="MultiLine" />                                                           
            <asp:Button class="bitbtn btngetitem" runat="server" ID="btnGetData" Text="Get Item" />     
            </td>
            
        </tr>
      </table>  
      
      <br />      
      <div style="font-size:medium; color:Blue;">Detail</div>
      <hr style="color:Blue" />  
        <asp:Panel runat="server" ID="pnlDt">
            <asp:Button class="bitbtndt btnadd" runat="server" ID="btnAdddt" Text="Add" ValidationGroup="Input" />                 
            <div style="border:0px  solid; overflow:auto;">
                <asp:GridView ID="GridDt" runat="server" AutoGenerateColumns="false" 
                    ShowFooter="True">
                    <HeaderStyle CssClass="GridHeader" />
                    <RowStyle CssClass="GridItem" Wrap="false" />
                    <AlternatingRowStyle CssClass="GridAltItem" />
                    <PagerStyle CssClass="GridPager" />
                    <Columns>
                        <asp:TemplateField HeaderText="Action">
                            <ItemTemplate>
                                <asp:Button ID="btnEdit" runat="server" class="bitbtndt btnedit" Text="Edit" CommandName="Edit" />
                                <asp:Button ID="btnDelete" runat="server" class="bitbtndt btndelete" Text="Delete"
                                        OnClientClick="return confirm('Sure to delete this data?');" CommandName="Delete" />       
                            </ItemTemplate>                                                                     
                        </asp:TemplateField>
                        <asp:TemplateField>
                                <ItemTemplate>
                                  <asp:Button ID="btnClosing" runat="server" class="bitbtn btnclosing" Text="Closing" CommandName="Closing" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" />     
                                </ItemTemplate>
                         </asp:TemplateField>                       
                        <asp:BoundField DataField="Product" HeaderStyle-Width="120px" HeaderText="Product Code" />
                        <asp:BoundField DataField="ProductName" HeaderText="Product Name" HeaderStyle-Width="200px" />
                        <asp:BoundField DataField="Specification" HeaderStyle-Width="200px" HeaderText="Specification" />                        
                        <asp:BoundField DataField="QtyOrder" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="80px" HeaderText="Qty Order" />
                        <asp:BoundField DataField="UnitOrder" HeaderStyle-Width="80px" HeaderText="Unit Order" />
                        <asp:BoundField DataField="QtyWrhs" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="80px" HeaderText="Qty Wrhs" />
                        <asp:BoundField DataField="UnitWrhs" HeaderStyle-Width="80px" HeaderText="Unit Wrhs" />
                        <asp:BoundField DataField="PriceForex" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="80px" HeaderText="Price" />
                        <asp:BoundField DataField="QtyM2" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="80px" HeaderText="Qty (M2)" />
                        <asp:BoundField DataField="QtyRoll" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="80px" HeaderText="Qty (Roll)" />
                        <asp:BoundField DataField="QtySJ" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="80px" HeaderText="Qty SJ" />
                        <asp:BoundField DataField="Remark" HeaderStyle-Width="200px" HeaderText="Remark" />
                        <asp:BoundField DataField="DoneClosing" HeaderStyle-Width="250px" HeaderText="Done Closing" />
                        <asp:BoundField DataField="QtyClose" DataFormatString="{0:#,##0.##}" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="80px" HeaderText="Qty Close" />
                        <asp:BoundField DataField="RemarkClose" HeaderStyle-Width="250px" HeaderText="Remark Close" />                        
                    </Columns>
                </asp:GridView>
          </div>   
          <asp:Button class="bitbtndt btnadd" runat="server" ID="btnAddDt2" Text="Add" ValidationGroup="Input" />                  
       </asp:Panel>             
       <asp:Panel runat="server" DefaultButton="btnSaveDt" ID="pnlEditDt" Visible="false">
            <table>              
                <tr>
                    <td>Product</td>
                    <td>:</td>
                    <td><asp:TextBox runat="server" ID="tbProduct" CssClass="TextBox" AutoPostBack="true" />
                        <asp:TextBox runat="server"  CssClass="TextBox" ID="tbProductName" EnableTheming="True" ReadOnly="True" Enabled="False" Width="200px"/>
                        <asp:Button ID="btnProduct" runat="server" class="btngo" Text="..."/>                        
                    </td>
                </tr>   
                <tr>
                    <td>Specification</td>
                    <td>:</td>
                    <td><asp:TextBox runat="server"  CssClass="TextBox" ID="tbSpec" EnableTheming="True" ReadOnly="True" Enabled="False" Width="200px"/>
                    </td>
                </tr>   
                <tr>
                    <td>Qty Order</td>
                    <td>:</td>
                    <td><asp:TextBox runat="server" ID="tbQtyOrder" CssClass="TextBox" AutoPostBack="true" />
                    <asp:DropDownList CssClass="DropDownList" runat="server" Enabled="false" ID="ddlUnitOrder"/></td>                    
                </tr>                                                    
                <tr>
                    <td>Qty Wrhs</td>
                    <td>:</td>
                    <td><asp:TextBox runat="server" ID="tbQtyWrhs" CssClass="TextBox" Enabled="false" />
                    <asp:DropDownList CssClass="DropDownList" runat="server" Enabled="false" ID="ddlUnitWrhs"/></td>                    
                </tr> 
                <tr>
                    <td>Price Forex</td>
                    <td>:</td>
                    <td><asp:TextBox runat="server" ID="tbPrice" ValidationGroup="Input" CssClass="TextBox" />                 
                </tr>   
                <tr>
                    <td></td>
                    <td></td>
                    <td><asp:TextBox runat="server" ID="tbQtyM2" CssClass="TextBox" Enabled="false" Visible="False"/> 
                    </td>                    
                </tr>  
                <tr>
                    <td></td>
                    <td></td>
                    <td><asp:TextBox runat="server" ID="tbQtyRoll" CssClass="TextBox" Enabled="false" Visible="False"/> 
                    </td>                    
                </tr>                
                <tr>
                    <td>Remark</td>
                    <td>:</td>
                    <td><asp:TextBox Width = "365px" runat="server" ValidationGroup="Input" ID="tbRemarkDt" CssClass="TextBox" MaxLength="255" TextMode="MultiLine" />                                        
                   
            </td>
        </tr>
                                    
            </table>
            <br />                     
            <asp:Button ID="btnSaveDt" runat="server" class="bitbtndt btnsave" Text="Save"/> &nbsp;         
            <asp:Button ID="btnCancelDt" runat="server" class="bitbtndt btncancel" Text="Cancel"/> 
       </asp:Panel> 
       
       <br />          
       <asp:Button ID="btnSaveAll" runat="server" class="bitbtndt btnsave" 
            Text="Save & New" ValidationGroup="Input" Width="97px"/> &nbsp;    
        <asp:Button ID="btnSaveTrans" runat="server" class="bitbtndt btnsave" Text="Save" ValidationGroup="Input"/> &nbsp;    
        <asp:Button ID="btnBack" runat="server" class="bitbtndt btncancel" Text="Cancel" ValidationGroup="Input"/>  &nbsp;
        <asp:Button ID="btnHome" runat="server" class="bitbtndt btncancel" Text="Home"/> 
    </asp:Panel>
    
    </div>     
    <asp:Label runat ="server" ID="lbStatus" ForeColor="Red"/>
    <asp:HiddenField ID="HiddenRemarkClose" runat="server" />
    </form>
</body>
</html>
