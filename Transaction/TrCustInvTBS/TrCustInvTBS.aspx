

<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TrCustInvTBS.aspx.vb" Inherits="CustInvTBS" %>
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
        var DiscForex = document.getElementById("tbDiscForex").value.replace(/\$|\,/g,"");        
        var PPnForex = document.getElementById("tbPPNForex").value.replace(/\$|\,/g,"");
        var TotalForex = document.getElementById("tbTotalForex").value.replace(/\$|\,/g,"");
        //var fgInclude = document.getElementById("ddlfgInclude").value.replace(/\$|\,/g,"");
        var DPForex = document.getElementById("tbDPForex").value.replace(/\$|\,/g,"");
        var TotalInvoice = document.getElementById("tbTotalInvoice").value.replace(/\$|\,/g,"");
                
        document.getElementById("tbPPN").value = setdigit(PPn, '<%=ViewState("DigitCurr")%>');
        document.getElementById("tbBaseForex").value = setdigit(BaseForex,'<%=VIEWSTATE("DigitCurr")%>');
        document.getElementById("tbDiscForex").value = setdigit(DiscForex,'<%=VIEWSTATE("DigitCurr")%>');
        document.getElementById("tbPPNForex").value = setdigit(PPnForex,'<%=VIEWSTATE("DigitCurr")%>');
        document.getElementById("tbTotalForex").value = setdigit(TotalForex,'<%=VIEWSTATE("DigitCurr")%>');
        document.getElementById("tbDPForex").value = setdigit(DPForex,'<%=VIEWSTATE("DigitCurr")%>');
        document.getElementById("tbTotalInvoice").value = setdigit(TotalInvoice,'<%=VIEWSTATE("DigitCurr")%>');
        }catch (err){
            alert(err.description);
          }      
        }

        function setformatdt(prmchange)
        {
        try {  
        }catch (err){
            alert(err.description);
          }      
        } 
        
        function setformatdt2()
        {
        try
        {       
        var DPBaseForex = document.getElementById("tbDPBaseForex").value.replace(/\$|\,/g,""); 
        var DPPPnForex = document.getElementById("tbDPPPnForex").value.replace(/\$|\,/g,"");         
        var DPRate = document.getElementById("tbDPRate").value.replace(/\$|\,/g,"");         
        var CIRate = document.getElementById("tbRate").value.replace(/\$|\,/g,"");         
        var DPTotalForex = parseFloat(DPBaseForex) + parseFloat(DPPPnForex);         
        var DPDPInvoice = (parseFloat(DPBaseForex) * parseFloat(DPRate)) / parseFloat(CIRate)) 
        document.getElementById("tbDPBaseForex").value = setdigit(DPBaseForex,'<%=VIEWSTATE("DigitCurrDt2")%>');
        document.getElementById("tbDPPPnForex").value = setdigit(DPPPnForex,'<%=VIEWSTATE("DigitCurrDt2")%>');
        document.getElementById("tbDPTotalForex").value = setdigit(DPTotalForex,'<%=VIEWSTATE("DigitCurrDt2")%>'); 
        document.getElementById("tbDPDPInvoice").value = setdigit(DPDPInvoice,'<%=VIEWSTATE("DigitHome")%>');                
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
    <div class="H1">Customer Invoice TBS</div>
     <hr style="color:Blue" />        
    <asp:Panel runat="server" id="PnlHd"> 
      <table>
        <tr>
            <td style="width:100px;text-align:right">Quick Search :</td>            
            <td><asp:TextBox CssClass="TextBox" runat="server" ID ="tbFilter"/> 
                  <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlField" >
                      <asp:ListItem Value="TransNmbr" Selected="True">Reference</asp:ListItem>
                      <asp:ListItem>Status</asp:ListItem>
                      <asp:ListItem Value="Printed">Printed</asp:ListItem>
                      <asp:ListItem Value="dbo.FormatDate(TransDate)">Date</asp:ListItem>
                      <asp:ListItem Value="PPnNo">PPn No</asp:ListItem>
                      <asp:ListItem Value="SPBNo">SPB No</asp:ListItem>
                      <asp:ListItem Value="SPBManual">SPB Manual</asp:ListItem>
                      <asp:ListItem Value="CustName">Customer Name</asp:ListItem>
                      <asp:ListItem Value="TermName">Term</asp:ListItem>
                      <asp:ListItem Value="dbo.FormatDate(DueDate)">Due Date</asp:ListItem>
                      <asp:ListItem Value="Currency">Currency</asp:ListItem> 
                      <asp:ListItem>Remark</asp:ListItem>
                   </asp:DropDownList>
                  <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlRange"></asp:DropDownList>
                  <asp:Button class="bitbtn btnsearch" runat="server" ID="btnSearch" Text="Search" />
                <asp:Button class="btngo" runat="server" ID="btnExpand" Text="..."/>
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
                      <asp:ListItem Value="TransNmbr" Selected="True">TransNmbr</asp:ListItem>
                      <asp:ListItem>Status</asp:ListItem>
                      <asp:ListItem Value="Printed">Printed</asp:ListItem>
                      <asp:ListItem Value="dbo.FormatDate(TransDate)">Date</asp:ListItem>
                      <asp:ListItem Value="dbo.FormatDate(TransDate)">Date</asp:ListItem>
                      <asp:ListItem Value="PPnNo">PPn No</asp:ListItem>
                      <asp:ListItem Value="SPBNo">SPB No</asp:ListItem>
                      <asp:ListItem Value="SPBManual">SPB Manual</asp:ListItem>
                      <asp:ListItem Value="CustName">Customer Name</asp:ListItem>
                      <asp:ListItem Value="TermName">Term</asp:ListItem>
                      <asp:ListItem Value="dbo.FormatDate(DueDate)">Due Date</asp:ListItem>
                      <asp:ListItem Value="Currency">Currency</asp:ListItem> 
                      <asp:ListItem>Remark</asp:ListItem>
                      
              </asp:DropDownList>
          </td>              
        </tr>        
      </table>      
      </asp:Panel>
          <br />
            <asp:Button class="bitbtn btnadd" runat="server" ID="BtnAdd" Text="Add"/>	 
            &nbsp &nbsp &nbsp   
            <asp:DropDownList CssClass="DropDownList" ID="ddlCommand" runat="server" Visible="false"/>
            <asp:Button class="bitbtn btngo" runat="server" ID="BtnGo" Text="G" Visible="false"/>
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
                              <asp:ListItem Text="Print" />
                              
                          </asp:DropDownList>
                          <asp:Button class="bitbtn btngo" runat="server" ID="btnGo" Text="G"	 
                          CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" CommandName="Go" />
                      </ItemTemplate>
                      <HeaderStyle Width="110px" />
                  </asp:TemplateField>
                  <asp:BoundField DataField="TransNmbr" HeaderStyle-Width="120px" SortExpression="Nmbr" HeaderText="Invoice No"></asp:BoundField>                  
                  <asp:BoundField DataField="Status" HeaderText="Status"></asp:BoundField>                  
                  <asp:BoundField DataField="TransDate" dataformatstring="{0:dd MMM yyyy}" htmlencode="true" HeaderStyle-Width="80px" SortExpression="TransDate" HeaderText="Date"></asp:BoundField>
                  <asp:BoundField DataField="SPBNo" HeaderStyle-Width="80px" SortExpression="SPBNo" HeaderText="SPB No"></asp:BoundField>
                  <asp:BoundField DataField="SPBManual" HeaderStyle-Width="80px" SortExpression="SPBManual" HeaderText="SPB Manual"></asp:BoundField>
                  <asp:BoundField DataField="CustCode" HeaderStyle-Width="80px" SortExpression="CustCode" HeaderText="Customer"></asp:BoundField>
                  <asp:BoundField DataField="CustName" HeaderStyle-Width="200px" SortExpression="CustName" HeaderText="Customer Name"></asp:BoundField>
                  <asp:BoundField DataField="TermName" HeaderStyle-Width="150px" SortExpression="TermName" HeaderText="Term"></asp:BoundField>
                  <asp:BoundField DataField="DueDate" dataformatstring="{0:dd MMM yyyy}" htmlencode="true" HeaderStyle-Width="80px" SortExpression="DueDate" HeaderText="Due Date"></asp:BoundField>
                  <asp:BoundField DataField="CurrCode" HeaderStyle-Width="30px" SortExpression="CurrCode" HeaderText="Currency"></asp:BoundField>
                  <asp:BoundField DataField="BankReceiptName" HeaderStyle-Width="120px" SortExpression="BankReceiptName" HeaderText="Bank Receipt"></asp:BoundField>
                  <asp:BoundField DataField="PPnNo" HeaderStyle-Width="120px" SortExpression="PPnNo" HeaderText="PPn No"></asp:BoundField>
                  <asp:BoundField DataField="BaseForex" DataFormatString="{0:#,##0.00}" HeaderStyle-Width="80px" SortExpression="BaseForex" ItemStyle-HorizontalAlign="Right" HeaderText="Base Forex"></asp:BoundField>
                  <asp:BoundField DataField="TotalForex" DataFormatString="{0:#,##0.00}" HeaderStyle-Width="80px" SortExpression="TotalForex" ItemStyle-HorizontalAlign="Right" HeaderText="Total Net Amount"></asp:BoundField>
                  <asp:BoundField DataField="PPnForex" DataFormatString="{0:#,##0.00}" HeaderStyle-Width="80px" SortExpression="PPnForex" ItemStyle-HorizontalAlign="Right" HeaderText="PPn Forex"></asp:BoundField>
                  <asp:BoundField DataField="Remark" HeaderStyle-Width="250px" HeaderText="Remark"></asp:BoundField>
                </Columns>
          </asp:GridView>
          </div>
            <asp:Panel runat="server" ID ="pnlNav" Visible="false">
            <%--<asp:Button class="bitbtn btngo" runat="server" ID="btnGo" Text="G"	 
                          CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" CommandName="Go" />--%>
            <asp:Button ID="BtnAdd2" runat="server" class="bitbtn btnadd" Text="Add" />
            &nbsp &nbsp &nbsp
            <asp:DropDownList ID="ddlCommand2" CssClass="DropDownList" runat="server"/>
            <asp:Button class="bitbtn btngo" runat="server" ID="btnGo2" Text="G"/>	                
                
                
            </asp:Panel>
    </asp:Panel>    
    <asp:Panel runat="server" ID="pnlInput" Visible="false">
        
        <table>
            <tr>
                <td>Invoice No</td>
                <td>:</td>
                <td><asp:TextBox ID="tbCode" runat="server" CssClass="TextBoxR" Enabled="False" Width="150px" />
                    <asp:DropDownList ID="ddlReport" runat="server" AutoPostBack="true" CssClass="DropDownList" Visible="False" >
                        <asp:ListItem Selected="True">Y</asp:ListItem>
                        <asp:ListItem>N</asp:ListItem>
                    </asp:DropDownList>                    
                </td>
                <td>Date</td>
                <td>:</td>
                <td><BDP:BasicDatePicker ID="tbDate" runat="server" AutoPostBack="True" 
                        ButtonImageHeight="19px" ButtonImageWidth="20px" DateFormat="dd MMM yyyy" 
                        DisplayType="TextBoxAndImage" ReadOnly="true" ShowNoneButton="False" 
                        TextBoxStyle-CssClass="TextDate" ValidationGroup="Input">
                        <TextBoxStyle CssClass="TextDate" />
                    </BDP:BasicDatePicker>
                </td>
            </tr>
            <tr>
                <td>SPB No</td>
                <td>:</td>
                <td colspan="4">
                    <asp:TextBox ID="tbSPB" runat="server" AutoPostBack="true" Enabled="false" CssClass="TextBoxR"  />
                    <asp:Button ID="btnSPB" runat="server" class="btngo" Text="..." ValidationGroup="Input" />
                    <asp:Label ID="Label1" runat="server" ForeColor="#FF3300" Text="*"></asp:Label>
                </td>                
            </tr>
            <tr>
                <td><asp:LinkButton ID="lbCust" runat="server" Text="Customer" ValidationGroup="Input" /></td>
                <td>:</td>
                <td colspan="4">
                    <asp:TextBox ID="tbCustCode" runat="server" AutoPostBack="true" CssClass="TextBox" ValidationGroup="Input" />
                    <asp:TextBox ID="tbCustName" runat="server" CssClass="TextBox" Enabled="false" Width="260px" />
                    <asp:Button ID="btnCust" runat="server" class="btngo" Text="..." ValidationGroup="Input" />
                    <asp:Label ID="Label11" runat="server" ForeColor="#FF3300" Text="*"></asp:Label>
                </td>                
            </tr>
            
            <tr>
                <td>Attn</td>
                <td>:</td>
                <td colspan="4">
                    <asp:TextBox ID="tbAttn" runat="server" CssClass="TextBox" ValidationGroup="Input" Width="225px" />
                </td>
            </tr>
            <tr>
                <td><asp:LinkButton ID="lbTerm" runat="server" Text="Term" ValidationGroup="Input" /></td>
                <td>:</td>
                <td colspan="4">
                    <asp:DropDownList ID="ddlTerm" runat="server" AutoPostBack="true" CssClass="DropDownList" ValidationGroup="Input" />
                    <BDP:BasicDatePicker ID="tbDueDate" runat="server" AutoPostBack="True" 
                        ButtonImageHeight="19px" ButtonImageWidth="20px" DateFormat="dd MMM yyyy" 
                        DisplayType="TextBox" Enabled="false" ReadOnly="true" ShowNoneButton="False" 
                        TextBoxStyle-CssClass="TextDate">
                        <TextBoxStyle CssClass="TextDate" />
                    </BDP:BasicDatePicker>
                    <asp:Button ID="BtnGetData" runat="server" class="bitbtn btnsearch" 
                        Text="Get Data" ValidationGroup="Input" />
                </td>                
            </tr>
            <tr>
                <td>PPn</td>
                <td>:</td>
                <td colspan="3">
                    <table>
                        <tr style="background-color:Silver;text-align:center">
                            <td>PPn No</td>
                            <td>PPn Date</td>
                            <td>PPn Rate</td>
                        </tr>
                        <tr>
                            <td><asp:TextBox ID="tbPPnNo" runat="server" CssClass="TextBox" ValidationGroup="Input" Width="150px" /></td>
                            <td><BDP:BasicDatePicker ID="tbPPndate" runat="server" AutoPostBack="True" 
                                    ButtonImageHeight="19px" ButtonImageWidth="20px" DateFormat="dd MMM yyyy" 
                                    DisplayType="TextBoxAndImage" ReadOnly="true" ShowNoneButton="False" 
                                    TextBoxStyle-CssClass="TextDate" ValidationGroup="Input">
                                    <TextBoxStyle CssClass="TextDate" />
                                </BDP:BasicDatePicker>
                            </td>
                            <td><asp:TextBox ID="tbPpnRate" runat="server" CssClass="TextBox" ValidationGroup="Input" Width="68px" /></td>
                        </tr>
                    </table>
                </td>
                
            </tr>
             <tr>
                <td>SPB Manual</td>
                <td>:</td>
                <td colspan="4">
                    <asp:TextBox ID="tbSPBManual" runat="server" CssClass="TextBox" ValidationGroup="Input" Width="150px" />
                </td>
            </tr>
            <tr>
                <td><asp:LinkButton ID="lbReceipt" runat="server" Text="Bank Receipt" ValidationGroup="Input" /></td>
                <td>:</td>
                <td><asp:DropDownList ID="ddlReceipt" runat="server" AutoPostBack="true" CssClass="DropDownList" ValidationGroup="Input" />                    
                </td>           
                
                <td></td>
                <td></td>
                <td>
                   
                </td>
            </tr>
            <tr>
                <td><asp:LinkButton ID="lbCurr" runat="server" Text="Currency" ValidationGroup="Input" /></td>
                <td>:</td>
                <td><asp:DropDownList ID="ddlCurr" runat="server" AutoPostBack="true" CssClass="DropDownList" ValidationGroup="Input" />
                    &nbsp; Rate : &nbsp;
                    <asp:TextBox ID="tbRate" runat="server" CssClass="TextBox" ValidationGroup="Input" Width="68px" />                        
                    <asp:Label ID="lbDigit" runat="server" Text=""/>
                </td>           
                
                <td></td>
                <td></td>
                <td>
                   
                </td>
            </tr>
            <tr>
                <td>Total</td>
                <td>:</td>
                <td colspan="4">
                    <table>
                        <tr style="background-color:Silver;text-align:center">
                            <td>Curr</td>
                            <td>Base Forex</td>
                            <td>VAT %</td>
                            <td>VAT Forex</td>
                            <td>Total Forex</td>
                            
                        </tr>
                        <tr>
                            <td><asp:TextBox ID="tbCurr" runat="server" CssClass="TextBoxR" Enabled="false" Width="90px" /></td>
                            <td><asp:TextBox ID="tbTotalForex" runat="server" CssClass="TextBoxR" Enabled="false" Width="120px" /></td>
                            <td><asp:TextBox ID="tbPPN" runat="server" CssClass="TextBox" ValidationGroup="Input" width="40px" /></td>
                            <td><asp:TextBox ID="tbPPNForex" runat="server" CssClass="TextBoxR" Enabled="false" Width="90px" /></td>
                            <td><asp:TextBox ID="tbTotalInvoice" runat="server" CssClass="TextBoxR" Enabled="false" Width="120px" /></td>
                            
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>Remark</td>
                <td>:</td>
                <td colspan="4">
                    <asp:TextBox ID="tbRemark" runat="server" CssClass="TextBox" ValidationGroup="Input" Width="225px" />                              
                </td>
            </tr>
        </table>
       
        
         
       
        <br />
        <br />      
      <asp:Menu
            ID="Menu1" runat="server" CssClass = "Menu"        
            StaticMenuItemStyle-CssClass = "MenuItem"
            StaticSelectedStyle-CssClass = "MenuSelect"        
            Orientation="Horizontal"
            ItemWrap = "False"
            StaticEnableDefaultPopOutImage="False">            
            <Items>
                <asp:MenuItem Text="Detail" Value="0"></asp:MenuItem>                   
                
            </Items>            
        </asp:Menu>
      <asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex="0">
           <asp:View ID="Tab1" runat="server">  
        <%--<div style="font-size:medium; color:Blue;">
            Detail</div>
        <hr style="color:Blue" />--%>
        <asp:Panel ID="pnlDt" runat="server">
            <%--<asp:Button ID="btnAddDt" runat="server" class="bitbtn btnadd" Text="Add" 
                ValidationGroup="Input" />--%>
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
                                <asp:Button ID="btnEdit" runat="server" class="bitbtn btnedit" CommandName="Edit" Text="Edit" />
                                <asp:Button ID="btnDelete" runat="server" class="bitbtndt btndelete" CommandName="Delete" 
                                    OnClientClick="return confirm('Sure to delete this data?');" Text="Delete" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="ItemNo" HeaderStyle-Width="50px" HeaderText="Item No" />
                        <asp:BoundField DataField="JenisBarang" HeaderStyle-Width="50px" HeaderText="Description" />
                        <asp:BoundField DataField="TahunTanam" HeaderStyle-Width="80px" HeaderText="Tahun Tanam" />
                        <asp:BoundField DataField="TahunTanamName" HeaderStyle-Width="80px" HeaderText="Tahun Tanam Name" />
                        <asp:BoundField DataField="NettoWeight" DataFormatString="{0:#,##0.####}" HeaderStyle-Width="80px" HeaderText="Netto Weight" ItemStyle-HorizontalAlign="Right" />
                        <asp:BoundField DataField="PriceForex" DataFormatString="{0:#,##0.########}" HeaderStyle-Width="80px" HeaderText="Price Forex" ItemStyle-HorizontalAlign="Right" />
                        <asp:BoundField DataField="AmountForex" HeaderStyle-Width="80px" HeaderText="Amount Forex" DataFormatString="{0:#,##0.########}" ItemStyle-HorizontalAlign="Right" />
                        
                    </Columns>
                </asp:GridView>
            </div>
           </asp:Panel>
        <asp:Panel ID="pnlEditDt" runat="server" Visible="false">
            <table>
            <tr>
                    <td>No</td>
                    <td>:</td>
                    <td><asp:TextBox ID="tbNo" runat="server" CssClass="TextBoxR" Enabled="false"  Width="20px" />
                       
                    </td>
                </tr>
                <tr>
                    <td>Desctiption</td>
                    <td>:</td>
                    <td>
                        <asp:TextBox ID="tbDescription" runat="server" CssClass="TextBox" Width="200px"/>
                        
                    </td>
                </tr>
                <tr>
                    <td>Tahun Tanam</td>
                    <td>:</td>
                    <td><asp:TextBox ID="tbStatusTanam" runat="server" CssClass="TextBoxR" Enabled="false" Width="130px" />
                    </td>
                </tr>
                
               <tr>
            <td>Amount</td>
            <td>:</td>
            <td>
                <table>
                    <tr style="background-color:Silver;text-align:center">
                        <td>Netto Weight</td>                        
                        <td>Price</td>
                        <td>Amount</td>
                    </tr>
                    <tr>
                        <td><asp:TextBox ID="tbNetto" ValidationGroup="Input" runat="server" Width="120px" CssClass="TextBox" /></td>
                        <td><asp:TextBox ID="tbPriceForex" ValidationGroup="Input" runat="server" Width="120px" CssClass="TextBox" AutoPostBack="True"/></td>                        
                        <td><asp:TextBox ID="tbAmountForex" ValidationGroup="Input" runat="server" Width="120px" CssClass="TextBoxR"/></td>
                    </tr>
                </table>
            </td>                
        </tr> 
          
                
            </table>
            <br />
            <asp:Button ID="btnSaveDt" runat="server" class="bitbtndt btnsave" Text="Save" />
            &nbsp;
            <asp:Button ID="btnCancelDt" runat="server" class="bitbtndt btncancel" Text="Cancel" />
        </asp:Panel>
        </asp:View>           
                   
        </asp:MultiView>     
       <br />          
        <br />
        <asp:Button ID="btnSaveAll" runat="server" class="bitbtndt btnsave" 
            Text="Save &amp; New" ValidationGroup="Input" Width="97px" />
        &nbsp;    
        <asp:Button ID="btnSaveTrans" runat="server" class="bitbtndt btnsave" Text="Save" ValidationGroup="Input"/> &nbsp;    
        <asp:Button ID="btnBack" runat="server" class="bitbtndt btncancel" Text="Cancel" ValidationGroup="Input"/>  &nbsp;
        <asp:Button ID="btnHome" runat="server" class="bitbtndt btncancel" Text="Home"/>   
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
