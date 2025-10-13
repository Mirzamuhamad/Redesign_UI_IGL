<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TrCosting.aspx.vb" Inherits="Transaction_TrCosting_TrCosting" %>
<%@ Register Assembly="BasicFrame.WebControls.BasicDatePicker" Namespace="BasicFrame.WebControls"
    TagPrefix="BDP" %>

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
    
        function setformatdt()
        {
         try
         {  
            document.getElementById("tbQty").value = setdigit(document.getElementById("tbQty").value.replace(/\$|\,/g,""),'<%=VIEWSTATE("DigitQty")%>');                    
            document.getElementById("tbPrice").value = setdigit(document.getElementById("tbPrice").value.replace(/\$|\,/g,""),'<%=VIEWSTATE("DigitQty")%>');
            document.getElementById("tbTotal").value = setdigit(document.getElementById("tbTotal").value.replace(/\$|\,/g,""),'<%=VIEWSTATE("DigitQty")%>');
                                    
            
        }catch (err){
            alert(err.description);
          }      
        }
        
        function hitungtotal()
        {
         try
         {  
            var _Qty = parseFloat(document.getElementById("tbQty").value.replace(/\$|\,/g,""));
            var _Price = parseFloat(document.getElementById("tbPrice").value.replace(/\$|\,/g,""));
            var _Waste = parseFloat(document.getElementById("tbWaste").value.replace(/\$|\,/g,""));
            var _Total = parseFloat(document.getElementById("tbTotal").value.replace(/\$|\,/g,""));
            
            _Total = (_Qty * _Price) * ((100 + _Waste) /100);
         
            document.getElementById("tbQty").value = setdigit(_Qty,'<%=VIEWSTATE("DigitQty")%>');
            document.getElementById("tbPrice").value = setdigit(_Price,'<%=VIEWSTATE("DigitQty")%>');
            document.getElementById("tbWaste").value = setdigit(_Waste,'<%=VIEWSTATE("DigitQty")%>');
            document.getElementById("tbTotal").value = setdigit(_Total,'<%=VIEWSTATE("DigitQty")%>');            
            
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
        <div class="H1">Standard Costing</div>
        <hr style="color:Blue" />
        <asp:Panel runat="server" id="PnlHd"> 
      <table>
        <tr>
            <td style="width:100px;text-align:right">Quick Search :</td>            
            <td><asp:TextBox CssClass="TextBox" runat="server" ID ="tbFilter"/> 
                  <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlField" >
                    <asp:ListItem Selected="True" Value="TransNmbr">Costing No</asp:ListItem>
                    <asp:ListItem Value="dbo.FormatDate(TransDate)">Costing Date</asp:ListItem>
                    <asp:ListItem>Status</asp:ListItem>                    
                    <asp:ListItem Value="Product">Product Code</asp:ListItem>
                    <asp:ListItem Value="Product_Name">Product Name</asp:ListItem>                    
                    <asp:ListItem Value="Sheet_Name">Sheet</asp:ListItem>
                    <asp:ListItem Value="Currency">Currency</asp:ListItem>
                    <asp:ListItem Value="dbo.FormatDate(EffectiveDate)">Effective Date</asp:ListItem>                    
                    <asp:ListItem Value="Remark">Remark</asp:ListItem>                     
                    </asp:DropDownList>
                  <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlRange"></asp:DropDownList>   
                  <asp:Button class="bitbtn btnsearch" runat="server" ID="btnSearch" Text="Search" />                  
                  <asp:Button class="btngo" runat="server" ID="btnExpand" Text="..."/>  
            </td>
            <td>
                &nbsp;
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
                    <asp:ListItem Selected="True" Value="TransNmbr">Costing No</asp:ListItem>
                    <asp:ListItem Value="dbo.FormatDate(TransDate)">Costing Date</asp:ListItem>
                    <asp:ListItem>Status</asp:ListItem>                    
                    <asp:ListItem Value="Product">Product Code</asp:ListItem>
                    <asp:ListItem Value="Product_Name">Product Name</asp:ListItem>                    
                    <asp:ListItem Value="Sheet_Name">Sheet</asp:ListItem>
                    <asp:ListItem Value="Currency">Currency</asp:ListItem>
                    <asp:ListItem Value="dbo.FormatDate(EffectiveDate)">Effective Date</asp:ListItem>
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
                              <asp:ListItem Text="Delete" />
                              <asp:ListItem Text="Copy New" />
                              <asp:ListItem Text="Print" />
                          </asp:DropDownList>
                          <asp:Button class="btngo" runat="server" ID="BtnGo" Text="G" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" CommandName="Go" />
                      </ItemTemplate>
                      <HeaderStyle Width="110px" />
                  </asp:TemplateField>
                  <asp:BoundField DataField="TransNmbr" HeaderStyle-Width="120px" SortExpression="Nmbr" HeaderText="Costing No"></asp:BoundField>                  
                  <asp:BoundField DataField="Status" HeaderText="Status"></asp:BoundField>                  
                  <asp:BoundField DataField="TransDate" dataformatstring="{0:dd MMM yyyy}" htmlencode="true" HeaderStyle-Width="80px" SortExpression="TransDate" HeaderText="Costing Date"></asp:BoundField>                  
                  <asp:BoundField DataField="Product" HeaderStyle-Width="80px" SortExpression="Product" HeaderText="Product Code"></asp:BoundField>
                  <asp:BoundField DataField="Product_Name" HeaderStyle-Width="200px" SortExpression="Product_Name" HeaderText="Product Name"></asp:BoundField>
                  <asp:BoundField DataField="Sheet_Name" HeaderStyle-Width="200px" SortExpression="Sheet_Name" HeaderText="Sheet Name"></asp:BoundField>
                  <asp:BoundField DataField="Currency" HeaderStyle-Width="200px" SortExpression="Currency" HeaderText="Currency"></asp:BoundField>
                  <asp:BoundField DataField="ForexRate" DataFormatString="{0:#,##0.00}" HeaderStyle-Width="80px" SortExpression="ForexRate" HeaderText="Rate"></asp:BoundField>
                  <asp:BoundField DataField="EffectiveDate" dataformatstring="{0:dd MMM yyyy}" htmlencode="true" HeaderStyle-Width="80px" SortExpression="EffectiveDate" HeaderText="Effective Date"></asp:BoundField>                  
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
            <td>Costing No</td>
            <td>:</td>
            <td><asp:TextBox CssClass="TextBoxR" runat="server" ID="tbCode" Width="150px" Enabled="False"/></td>        
            <td>Costing Date</td>
            <td>:</td>
            <td>
            <BDP:BasicDatePicker ID="tbDate" runat="server" DateFormat="dd MMM yyyy" 
                        ReadOnly = "true" ValidationGroup="Input"
                        ButtonImageHeight="19px" ButtonImageWidth="20px" 
                        DisplayType="TextBoxAndImage" 
                        TextBoxStyle-CssClass="TextDate" 
                        ShowNoneButton="False"><TextBoxStyle CssClass="TextDate" /></BDP:BasicDatePicker>                
            </td>                 
        </tr> 
         
        
        <tr>
            <td>Product</td>
            <td>:</td>
            <td colspan="4">
                <asp:TextBox CssClass="TextBox" ValidationGroup="Input" runat="server" ID="tbProductCode" MaxLength="12" AutoPostBack="true" />
                <asp:TextBox CssClass="TextBox" runat="server" ID="tbProductName" Enabled="false" MaxLength="60" Width="225px"/>
                <asp:Button Class="btngo" ID="btnProduct" Text="..." runat="server" ValidationGroup="Input" />                                  
            </td>
        </tr>        
        <tr>            
            <td>Sheet</td>
            <td>:</td>
            <td colspan="3">
                <asp:DropDownList ID="ddlSheet" runat="server" CssClass="DropDownList" ValidationGroup="Input" Width="225px"/>
            </td>
        </tr>  
        <tr> 
            <td></td>
            <td></td>
            <td colspan="4">
               <table>
               <tr style="background-color:Silver;text-align:center">
                 <td>Curr</td>
                 <td>Rate (USD)</td>
                 <td>Effective Date</td>                                    
               </tr>
               <tr>
                  <td><asp:DropDownList CssClass="DropDownList" ID="ddlCurr" ValidationGroup="Input"  runat="server" Width="60px"/></td>
                  <td><asp:TextBox CssClass="TextBox" runat="server" ID="tbRate" ValidationGroup="Input"  Width="80px"/></td>
                  <td><BDP:BasicDatePicker ID="tbEffDate" runat="server" DateFormat="dd MMM yyyy" ValidationGroup="Input" 
                       ReadOnly = "true" ButtonImageHeight="19px" ButtonImageWidth="20px" DisplayType="TextBoxAndImage" 
                       TextBoxStyle-CssClass="TextDate" ShowNoneButton="False"> 
                       <TextBoxStyle CssClass="TextDate" /> </BDP:BasicDatePicker>
                   </td>                                    
                </tr>
                </table>
             </td>
        </tr>     
        <tr>
            <td>Remark</td>
            <td>:</td>
            <td><asp:TextBox Width = "365px" runat="server" ValidationGroup="Input" ID="tbRemark" CssClass="TextBox" MaxLength="255" TextMode="MultiLine" />                                                           
            </td>
            <td></td>
            <td></td>
            <td><asp:Button ID="btnGetData" runat="server" class="btngo" ValidationGroup="Input" Text="Get Detail" Width="80px" Visible="True"/> &nbsp &nbsp &nbsp 
            <asp:DropDownList AutoPostBack="true" ValidationGroup="Input" CssClass="DropDownList" ID="ddlTypeGetData" runat="server" >
                        <asp:ListItem Selected="True">BOM</asp:ListItem>
                        <asp:ListItem>Formula PP</asp:ListItem>                        
                    </asp:DropDownList> 
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
                        <asp:BoundField DataField="CostingType" HeaderStyle-Width="100px" HeaderText="Costing Type" />
                        <asp:BoundField DataField="CostingCode" HeaderStyle-Width="120px" HeaderText="Costing Code" />
                        <asp:BoundField DataField="CostingName" HeaderStyle-Width="120px" HeaderText="Costing Name" />
                        <asp:BoundField DataField="Qty" DataFormatString="{0:#,##0.00}" HeaderStyle-Width="80px" HeaderText="Qty" />
                        <asp:BoundField DataField="Unit" HeaderStyle-Width="80px" HeaderText="Unit" />
                        <asp:BoundField DataField="Price" DataFormatString="{0:#,##0.00}" HeaderStyle-Width="80px" HeaderText="Price" />
                        <asp:BoundField DataField="Waste" HeaderStyle-Width="80px" HeaderText="Waste" />
                        <asp:BoundField DataField="Total" DataFormatString="{0:#,##0.00}" HeaderStyle-Width="80px" HeaderText="Total" />
                                                
                    </Columns>
                </asp:GridView>
          </div>   
          <asp:Button class="bitbtndt btnadd" runat="server" ID="btnAddDt2" Text="Add" ValidationGroup="Input" />                  
       </asp:Panel>             
       <asp:Panel runat="server" DefaultButton="btnSaveDt" ID="pnlEditDt" Visible="false">
            <table> 
                <tr>             
                <td>Costing Type</td>
                <td>:</td>
                <td><asp:DropDownList AutoPostBack="true" ValidationGroup="Input" CssClass="DropDownList" ID="ddlCostingType" runat="server" >
                        <asp:ListItem Selected="True">Material</asp:ListItem>
                        <asp:ListItem>Labor Direct</asp:ListItem>
                        <asp:ListItem>Labor In-Direct</asp:ListItem>
                        <asp:ListItem>Overhead</asp:ListItem>
                    </asp:DropDownList> 
                </td>  
                </tr>
                <tr>
                    <td>Costing</td>
                    <td>:</td>
                    <td>
                        <asp:TextBox CssClass="TextBox" ValidationGroup="Input" runat="server" ID="tbCostingCode" MaxLength="12" AutoPostBack="true" />
                        <asp:TextBox CssClass="TextBox" runat="server" ID="tbCostingName" Enabled="false" MaxLength="60" Width="225px"/>
                        <asp:Button Class="btngo" ID="btnCosting" Text="..." runat="server" ValidationGroup="Input" />                                  
                    </td>
                </tr> 
                <tr> 
                    <td></td>
                    <td></td>
                    <td>
                    <table>
                      <tr style="background-color:Silver;text-align:center">
                        <td>Qty</td>
                        <td>Unit</td>
                        <td>Price</td>
                        <td>Waste (%)</td>
                        <td>Total</td>
                      </tr>
                      <tr>
                        <td><asp:TextBox CssClass="TextBox" runat="server" ID="tbQty" Width="70px"/></td>
                        <td><asp:DropDownList CssClass="DropDownList" ID="ddlUnit" runat="server" Enabled="false" Width="80px"/></td>
                        <td><asp:TextBox CssClass="TextBox" runat="server" ID="tbPrice" Width="70px"/></td>
                        <td><asp:TextBox CssClass="TextBox" runat="server" ID="tbWaste" Width="65px"/></td>
                        <td><asp:TextBox CssClass="TextBoxR" runat="server" ID="tbTotal" Width="80px"/></td>                                                        
                      </tr>
                    </table>
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
