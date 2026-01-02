<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TrPriceListSupplier.aspx.vb" Inherits="TrPriceListSupplier" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<%@ Register Assembly="BasicFrame.WebControls.BasicDatePicker" Namespace="BasicFrame.WebControls"
    TagPrefix="BDP" %>

<%@ Register assembly="CrystalDecisions.Web, Version=10.5.3700.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Price List Supplier</title>
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
        
        document.getElementById("tbRate").value = setdigit(Rate,'<%=VIEWSTATE("DigitRate")%>');
        document.getElementById("tbPPN").value = setdigit(PPn,'<%=VIEWSTATE("DigitPercent")%>');
        document.getElementById("tbBaseForex").value = setdigit(BaseForex,'<%=VIEWSTATE("DigitCurr")%>');
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
        var QtyWrhs = document.getElementById("tbQtyWrhs").value.replace(/\$|\,/g,"");         
        document.getElementById("tbQty").value = setdigit(Qty,'<%=VIEWSTATE("DigitQty")%>');
        document.getElementById("tbQtyWrhs").value = setdigit(QtyWrhs,'<%=VIEWSTATE("DigitQty")%>');        
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
    <div class="H1">Price List Supplier</div>
     <hr style="color:Blue" />        
    <asp:Panel runat="server" id="PnlHd"> 
      <table>
        <tr>
            <td style="width:100px;text-align:right">Quick Search :</td>            
            <td><asp:TextBox CssClass="TextBox" runat="server" ID ="tbFilter"/> 
                  <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlField" >
                      <asp:ListItem Value="TransNmbr" Selected="True">Reference</asp:ListItem>
                      <asp:ListItem Value="Status">Status</asp:ListItem> 
                      <asp:ListItem Value="dbo.FormatDate(TransDate)">Date</asp:ListItem>
                      <asp:ListItem Value="dbo.FormatDate(EffectiveDate)">Effective Date</asp:ListItem>
                      <asp:ListItem Value="Product">Product</asp:ListItem>                                          
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
                  <asp:ListItem Value="TransNmbr" Selected="True">Reference</asp:ListItem>
                      <asp:ListItem Value="Status">Status</asp:ListItem> 
                      <asp:ListItem Value="dbo.FormatDate(TransDate)">Date</asp:ListItem>
                      <asp:ListItem Value="dbo.FormatDate(EffectiveDate)">Effective Date</asp:ListItem>
                      <asp:ListItem Value="Product">Product</asp:ListItem>                                          
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
            <asp:Button class="btngo" runat="server" ID="BtnGo" Text="G" Visible="false" />                                <br />
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
                  <asp:TemplateField HeaderStyle-Width="120">
                      <ItemTemplate>
                          <asp:DropDownList CssClass="DropDownList" ID="ddl" runat="server">
                              <asp:ListItem Selected="True" Text="View" />
                              <asp:ListItem Text="Edit" />
                              <asp:ListItem Text="Print" />
                          </asp:DropDownList>
                          <asp:Button class="btngo" runat="server" ID="BtnGo" Text="G" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" CommandName="Go" />                                            
                      </ItemTemplate>
                      <HeaderStyle Width="80px" />
                  </asp:TemplateField>
                  <asp:BoundField DataField="TransNmbr" HeaderStyle-Width="120px" SortExpression="Nmbr" HeaderText="Reference"></asp:BoundField>                  
                  <asp:BoundField DataField="Status" HeaderText="Status" SortExpression = "Status"></asp:BoundField>                  
                  <asp:BoundField DataField="TransDate" dataformatstring="{0:dd MMM yyyy}" htmlencode="true" HeaderStyle-Width="80px" SortExpression="TransDate" HeaderText="Date"></asp:BoundField>
                  <asp:BoundField DataField="EffectiveDate" dataformatstring="{0:dd MMM yyyy}" htmlencode="true" HeaderStyle-Width="80px" SortExpression="EffectiveDate" HeaderText="Effective Date "></asp:BoundField>
                  <asp:BoundField DataField="Product" HeaderStyle-Width="80px" SortExpression="Product" HeaderText="Product"></asp:BoundField>
                  <asp:BoundField DataField="Product_Name" HeaderStyle-Width="80px" SortExpression="Product" HeaderText="Product Name"></asp:BoundField>
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
            <td>Supplier Price No</td>
            <td>:</td>
            <td><asp:TextBox CssClass="TextBoxR" runat="server" ID="tbRef" Width="150px" Enabled="False"/>             
            </td>                    
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td>
                &nbsp;</td>            
        </tr> 
        <tr>
            <td>Date</td>
            <td>:</td>
            <td colspan="4">
                <BDP:BasicDatePicker ID="tbDate" runat="server" AutoPostBack="True" 
                    ButtonImageHeight="19px" ButtonImageWidth="20px" DateFormat="dd MMM yyyy" 
                    DisplayType="TextBoxAndImage" ReadOnly="true" ShowNoneButton="False" 
                    TextBoxStyle-CssClass="TextDate" ValidationGroup="Input">
                    <TextBoxStyle CssClass="TextDate" />
                </BDP:BasicDatePicker>
              
            </td>            
        </tr>
        <tr>
            <td>Effective Date</td>
            <td>:</td>
            <td colspan="4">
                <BDP:BasicDatePicker ID="tbEffectiveDate" runat="server" AutoPostBack="True" 
                    ButtonImageHeight="19px" ButtonImageWidth="20px" DateFormat="dd MMM yyyy" 
                    DisplayType="TextBoxAndImage" ReadOnly="true" ShowNoneButton="False" 
                    TextBoxStyle-CssClass="TextDate" ValidationGroup="Input">
                    <TextBoxStyle CssClass="TextDate" />
                </BDP:BasicDatePicker>
            </td>            
        </tr>        
        <tr>
             <td><asp:LinkButton ID="lbProduct"  runat="server" Text="Product"/> </td>
                   
            <td>:</td>
            <td colspan="4"> 
                <asp:TextBox CssClass="TextBox" runat="server" 
                    ID="tbProductCode" AutoPostBack="true" />                
                <asp:TextBox ID="tbProductName" runat="server" CssClass="TextBox" Enabled="false" 
                    EnableTheming="True" Width="200px" />
                <asp:Button ID="btnProduct" runat="server" Class="btngo" Text="..." 
                    ValidationGroup="Input" />
            </td>        
        </tr>   
            
        
        <tr> 
            <td>Remark</td>
            <td>:</td>
            <td colspan="4"><asp:TextBox runat="server" ValidationGroup="Input" ID="tbRemark" MaxLength="255" CssClass="TextBoxMulti" TextMode="MultiLine" Width="350px"/></td>
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
                        </asp:TemplateField>
                        <asp:BoundField DataField="Supplier" HeaderStyle-Width="80px" HeaderText="Supplier Code" />
                        <asp:BoundField DataField="SupplierName" HeaderStyle-Width="250px"  HeaderText="Supplier Name" />
                        <asp:BoundField DataField="No_SPK" HeaderStyle-Width="250px"  HeaderText="No SPK" />
                        <asp:BoundField DataField="PreviousPrice" DataFormatString="{0:#,##0.00}" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="100px" HeaderText="Previous Price" />                                             
                        <asp:BoundField DataField="TodayPrice" DataFormatString="{0:#,##0.00}" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="100px" HeaderText="Today Price" />
                        <asp:BoundField DataField="Remark" HeaderStyle-Width="200px" HeaderText="Remark" />
                    </Columns>
                </asp:GridView>
          </div>   
          <asp:Button class="bitbtn btnadd" runat="server" ID="btnAddDt2" Text="Add" ValidationGroup="Input" />	  
       </asp:Panel>             
       <asp:Panel runat="server" ID="pnlEditDt" Visible="false">
            <table>                
                <tr>
                    <td><asp:LinkButton ID="lbSupp"  runat="server" Text="Supplier"/> </td>
                    <td>:</td>
                    <td><asp:TextBox runat="server" ID="tbSuppCode" CssClass="TextBox" AutoPostBack="true" />
                        <asp:TextBox runat="server"  CssClass="TextBox"
                            ID="tbSuppName" EnableTheming="True" Enabled="false" Width="200px"/> 
                        <asp:Button Class="btngo" ID="btnSupp" Text="..." runat="server" ValidationGroup="Input" />                                               
                        
                    </td>                               
                </tr>                   
                <tr>
                    <td>No SPK</td>
                    <td>:</td>
                    <td>
                        <asp:TextBox ID="tbNoSpk" runat="server" CssClass="TextBox" 
                            Enabled="True" />
                    </td>
                </tr>
                <tr>
                    <td>PreviousPrice</td>
                    <td>:</td>
                    <td>
                        <asp:TextBox ID="tbPreviousPrice" runat="server" CssClass="TextBox" 
                            MaxLength="13" />
                    </td>
							<td>
                        <asp:TextBox ID="tbPrevious" runat="server" Visible = "False"  CssClass="TextBox" 
                            MaxLength="13" />
                    </td>                    
                </tr>
                <tr>
                    <td>Today Price</td>
                    <td>:</td>
                    <td><asp:TextBox CssClass="TextBox" runat="server" ID="tbTodayPrice" AutoPostBack="True" 
                            MaxLength="13" /></td>
                </tr>
  
                <tr>
                    <td>Remark </td>
                    <td>:</td>
                    <td><asp:TextBox runat="server" MaxLength="255" CssClass="TextBoxMulti" TextMode="MultiLine" ID="tbRemarkDt" Width = "355" /></td>
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
