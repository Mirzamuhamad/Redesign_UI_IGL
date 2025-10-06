<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TrBeginCIPPN.aspx.vb" Inherits="Transaction_TrBeginCIPPN_TrBeginCIPPN" %>

<%@ Register Assembly="BasicFrame.WebControls.BasicDatePicker" Namespace="BasicFrame.WebControls"
    TagPrefix="BDP" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Customer Begin (PPN)</title>
    <script src="../../Function/OpenDlg.js" type="text/javascript"></script>
    <script src="../../Function/Function.JS" type="text/javascript"></script>
    <script type="text/javascript">
        function setdigit(nStr, digit)
        {
        try
        {
        var TNstr = parseFloat(nStr);        
        TNstr = TNstr.toFixed(digit);                
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
        
        function setformat(){   
          try
          { 
            var _tempbaseforex = parseFloat(document.getElementById("tbBaseForex").value.replace(/\$|\,/g,""));
            var _tempppn = parseFloat(document.getElementById("tbPPN").value.replace(/\$|\,/g,""));
            var _tempppnforex = parseFloat(document.getElementById("tbPPNForex").value.replace(/\$|\,/g,""));
            var _temptotalforex = parseFloat(document.getElementById("tbTotalForex").value.replace(/\$|\,/g,""));
            var _tempRate = parseFloat(document.getElementById("tbRate").value.replace(/\$|\,/g,""));
            var _tempPPnRate = parseFloat(document.getElementById("tbPPNRate").value.replace(/\$|\,/g,""));
            
            document.getElementById("tbPPN").value = setdigit(_tempppn, '<%=ViewState("DigitPercent")%>');
            document.getElementById("tbBaseForex").value = setdigit(_tempbaseforex, '<%=ViewState("DigitCurr")%>');
            document.getElementById("tbPPNForex").value = setdigit(_tempppnforex, '<%=ViewState("DigitCurr")%>');
            document.getElementById("tbTotalForex").value = setdigit(_temptotalforex, '<%=ViewState("DigitCurr")%>');
            document.getElementById("tbRate").value = setdigit(_tempRate,'<%=ViewState("DigitRate")%>');
            document.getElementById("tbPPNRate").value = setdigit(_tempPPnRate,'<%=ViewState("DigitRate")%>');            
          }catch (err){
            alert(err.description);
          }
        }
    </script>
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
 <link href="https://fonts.googleapis.com/css2?family=Muli:wght@300;400;600&display=swap" rel="stylesheet">
    
    <style type="text/css">
        .style1
        {
            width: 3px;
        }
    </style>
    
</head>
<body>
    <form id="form1" runat="server">
    <div class="Content">
    <div class="H1">CUSTOMER BEGIN (PPN)</div>
     <hr style="color:Blue" />        
    <asp:Panel runat="server" id="PnlHd"> 
      <table>
        <tr>
            <td style="width:100px;text-align:right">Quick Search :</td>            
            <td><asp:TextBox CssClass="TextBox" runat="server" ID ="tbFilter"/> 
                  <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlField" >
                      <asp:ListItem Value="InvoiceNo" Selected="True">Invoice No</asp:ListItem>
                      <asp:ListItem Value="InvoiceDate">Invoice Date</asp:ListItem>
                      <asp:ListItem Value="Status">Status</asp:ListItem>
                      <asp:ListItem Value="FgReport">Report</asp:ListItem>
                      <asp:ListItem Value="Customer">Customer</asp:ListItem> 
                      <asp:ListItem Value="Bill_To">Bill To</asp:ListItem>
                      <asp:ListItem Value="Due_Date">Due Date</asp:ListItem>
                      <asp:ListItem Value="CostCtrName">Cost Center</asp:ListItem>
                      <asp:ListItem Value="Currency">Currency</asp:ListItem>
                      <asp:ListItem Value="Remark">Remark</asp:ListItem>                                           
                    </asp:DropDownList>
                  
                  <asp:Button class="bitbtn btnsearch" runat="server" ID="btnSearch" Text="Search" />																						 											  
                  <asp:Button class="bitbtn btngo" runat="server" ID="btnExpand" Text="..." />																						 											  
                  
                  
            </td>
            <td>
                <asp:LinkButton ID="lkbAdvanceSearch" runat="server" Text="Advanced Search" />
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
                  <asp:ListItem Value="InvoiceNo" Selected="True">Invoice No</asp:ListItem>
                      <asp:ListItem Value="InvoiceDate">Invoice Date</asp:ListItem>
                      <asp:ListItem Value="Status">Status</asp:ListItem>
                      <asp:ListItem Value="FgReport">Report</asp:ListItem>                      
                      <asp:ListItem Value="Customer">Customer</asp:ListItem>                      
                      <asp:ListItem Value="Bill_To">Bill To</asp:ListItem>
                      <asp:ListItem Value="Due_Date">Due Date</asp:ListItem>
                      <asp:ListItem Value="CostCtrName">Cost Center</asp:ListItem>
                      <asp:ListItem Value="Currency">Currency</asp:ListItem>
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
      <div style="border:0px  solid; height:100%; overflow:auto;">
          <asp:GridView ID="GridView1" runat="server" AllowPaging="True" 
              AllowSorting="true" AutoGenerateColumns="false" CssClass="Grid">
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
                  <asp:TemplateField ItemStyle-Width="80px">
                      <ItemTemplate>
                          <asp:DropDownList ID="ddl" runat="server" CssClass="DropDownList">
                              <asp:ListItem Selected="True" Text="View" />
                              <asp:ListItem Text="Edit" />
                              <asp:ListItem Text="Delete" />
                              <%--<asp:ListItem Text="Print" />--%>
                          </asp:DropDownList>
                          <asp:Button class="btngo" runat="server" ID="BtnGoDt" Text="G" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" CommandName="Go"  />   
                             
                          
                      </ItemTemplate>
                  </asp:TemplateField>
                  <asp:BoundField DataField="InvoiceNo" HeaderStyle-Width="320px" HeaderText="Invoice No" SortExpression="InvoiceNo" />
                  <asp:BoundField DataField="Status" HeaderStyle-Width="10px" HeaderText="Status" SortExpression="Status" />
                  <asp:BoundField DataField="Invoice_Date" HeaderStyle-Width="80px" HeaderText="Invoice Date" SortExpression="InvoiceDate" />                  
                  <asp:BoundField DataField="FgReport" HeaderStyle-Width="10px" HeaderText="Report" SortExpression="FgReport" />
                  <asp:BoundField DataField="Customer" HeaderStyle-Width="250px" HeaderText="Customer" SortExpression="Customer" />
                  <asp:BoundField DataField="Bill_To" HeaderStyle-Width="250px" HeaderText="Bill To" SortExpression="Bill_To" />
                  <asp:BoundField DataField="Due_Date" HeaderStyle-Width="80px" HeaderText="Due Date" SortExpression="DueDate" />
                  <asp:BoundField DataField="CostCtrName" HeaderStyle-Width="200px" HeaderText="Cost Center" SortExpression="CostCtrName" />
                  <asp:BoundField DataField="Currency" HeaderStyle-Width="15px" HeaderText="Currency" SortExpression="Currency" />
                  <asp:BoundField DataField="BaseForex" HeaderStyle-Width="80px" HeaderText="Base Forex" SortExpression="BaseForex" ItemStyle-HorizontalAlign = "Right" />
                  <asp:BoundField DataField="PPn" HeaderStyle-Width="50px" HeaderText="PPn" SortExpression="PPn" ItemStyle-HorizontalAlign = "Right"/>
                  <asp:BoundField DataField="PPnForex" HeaderStyle-Width="80px" HeaderText="PPn Forex" SortExpression="PPnForex" ItemStyle-HorizontalAlign = "Right" />
                  <asp:BoundField DataField="TotalForex" HeaderStyle-Width="80px" HeaderText="Total Forex" SortExpression="TotalForex" ItemStyle-HorizontalAlign = "Right" />
                  <asp:BoundField DataField="Remark" HeaderStyle-Width="200px" HeaderText="Remark" SortExpression="Remark" />
                  
              </Columns>
          </asp:GridView>
          </div>
            <asp:Panel runat="server" ID ="pnlNav" Visible="false">
            <asp:Button class="bitbtn btnadd" runat="server" ID="btnAdd2" Text="Add" />	
            &nbsp &nbsp &nbsp
            <asp:DropDownList ID="ddlCommand2" CssClass="DropDownList" runat="server"/>
            <asp:Button class="btngo" runat="server" ID="btnGo2" Text="G"  />                      
            

     </asp:Panel>    
     </asp:Panel>  
     <asp:Panel ID="pnlInput" DefaultButton="btnSave" runat="server" Visible="false">
        <table>
            <tr>
                <td>Invoice No</td>
                <td class="style1">:</td>
                <td><asp:TextBox ID="tbInvoiceNo" MaxLength="20" ValidationGroup="Input" runat="server" CssClass ="TextBox" />
                    <%--&nbsp Report : &nbsp
                    <asp:DropDownList AutoPostBack="true" ValidationGroup="Input" CssClass="DropDownList" ID="ddlReport" runat="server" >
                        <asp:ListItem Selected="True">Y</asp:ListItem>
                        <asp:ListItem>N</asp:ListItem>
                    </asp:DropDownList>--%>
                </td>
                
                <td>Invoice Date</td>
                <td>:</td>
                <td>
                    <BDP:BasicDatePicker ID="tbTransDate" runat="server" DateFormat="dd MMM yyyy" 
                        ShowNoneButton = "false" ReadOnly = "true" ValidationGroup="Input"
                        ButtonImageHeight="19px" ButtonImageWidth="20px" 
                        DisplayType="TextBoxAndImage" 
                        TextBoxStyle-CssClass="TextDate" AutoPostBack="True"><TextBoxStyle CssClass="TextDate" /></BDP:BasicDatePicker>
                </td>
            </tr>                
             <tr>
                <td>Customer</td>
                <td class="style1">:</td>
                <td colspan="4">
                <asp:TextBox ID="tbCustCode" ValidationGroup="Input" AutoPostBack="true" Width="100" runat="server" CssClass="TextBox" />
                <asp:TextBox ID="tbCustName" Width="175" runat="server" 
                        ReadOnly="true" CssClass="TextBoxR" />
                <asp:Button Class="btngo" ID="btnCust" Text="..." runat="server" ValidationGroup="Input" />                  

                <%--<asp:Button ID="btntemp" CssClass="ButtonR" OnClientClick="return true;" runat="server" />               --%>
                
                </td>                
             </tr>
             <tr>
                <td>Bill To</td>
                <td class="style1">:</td>
                <td colspan="4">
                <asp:TextBox ID="tbBillCode" ValidationGroup="Input" AutoPostBack="true" Width="100" runat="server" CssClass="TextBox" />
                <asp:TextBox ID="tbBillName" Width="175" runat="server" 
                        ReadOnly="true" CssClass="TextBoxR" />                        
                <asp:Button Class="btngo" ID="btnBillTo" Text="..." runat="server" ValidationGroup="Input" />                                          
                
                
                <%--<asp:Button ID="btntemp" CssClass="ButtonR" OnClientClick="return true;" runat="server" />               --%>
                
                </td>                                
             </tr>
             <tr>
                <td>Attention</td>
                <td class="style1">:</td>
                <td><asp:TextBox ID="tbAttn" ValidationGroup="Input"
                        runat="server" CssClass="TextBox" Width="200px" /></td>
                <td>Cust PO No</td>
                <td>:</td>
                <td>
                    <asp:TextBox ID="tbcustPONo" runat="server" CssClass="TextBox" Width="130px"
                        ValidationGroup="Input" />
                </td>        
             </tr>             
             <tr>
                <td>Term</td>
                <td class="style1">:</td>
                <td><asp:TextBox ID="tbTerm" ValidationGroup="Input" AutoPostBack="true" runat="server" CssClass="TextBox" /> &nbsp days</td>
                 <td>Due Date</td>
                 <td>:</td>
                 <td><BDP:BasicDatePicker ID="tbDueDate" ValidationGroup="Input" runat="server" DateFormat="dd MMM yyyy" ButtonImageHeight="19px" ButtonImageWidth="20px" 
                       ShowNoneButton = "false" DisplayType="TextBoxAndImage" ReadOnly="true" TextBoxStyle-CssClass="TextDate"></BDP:BasicDatePicker>
                 </td>                 
             </tr>
             <tr>
                <td>Cost Center</td>
                <td class="style1">:</td>
                <td colspan="4">
                    <asp:DropDownList ID="ddlCostCtr" runat="server" CssClass="DropDownList" 
                        Height="16px" ValidationGroup="Input" Width="280px">
                    </asp:DropDownList>
             </tr>
             <tr>
                <td>
                    <asp:LinkButton ID="lbCurr" runat="server" Text="Currency" />
                 </td>
                <td class="style1">:</td>
                <td>
                    <asp:DropDownList ID="ddlCurr" runat="server" AutoPostBack="true" 
                        CssClass="DropDownList" ValidationGroup="Input" />
                    <asp:TextBox ID="tbRate" runat="server" CssClass="TextBox" 
                        style="text-align:right;" ValidationGroup="Input" Width="80px" />
                    </t>
                    <td>
                        Type</td>
                    <td class="style1">
                        :</td>
                    <td>
                        <asp:DropDownList ID="ddlType" runat="server" CssClass="DropDownList" 
                            ValidationGroup="Input" Width="80px">
                            <asp:ListItem>CI</asp:ListItem>
                            <asp:ListItem>CN</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </td>                
             </tr>        
             <tr>
                <td>PPN</td>
                <td class="style1">:</td>
                <td colspan="4">
                    <table>
                        <tr style="background-color:Silver;text-align:center">
                            <td>No</td>
                            <td>Date</td>
                            <td>Rate</td>
                        </tr>
                        <tr>
                            <td><asp:TextBox ID="tbPPNNo" ValidationGroup="Input" runat="server" 
                                    CssClass="TextBox" /></td>
                            <td>
                                <BDP:BasicDatePicker ID="tbPPNDate" runat="server" ButtonImageHeight="19px" 
                                    ButtonImageWidth="20px" DateFormat="dd MMM yyyy" DisplayType="TextBoxAndImage" 
                                    ReadOnly="true" TextBoxStyle-CssClass="TextDate" ValidationGroup="Input" 
                                    AutoPostBack="True">
                                    <TextBoxStyle CssClass="TextDate" />
                                </BDP:BasicDatePicker>
                            </td>
                            <td><asp:TextBox ID="tbPPNRate" runat="server" CssClass="TextBox" 
                                    style="text-align:right;" ValidationGroup="Input" Width="80px"/></td>
                        </tr>
                    </table>
                </td>                
             </tr>             
             <tr>
                <td>Amount</td>
                <td class="style1">:</td>
                <td colspan="4">
                    <table>
                        <tr style="background-color:Silver;text-align:center">
                            <td>
                                Base Forex</td>
                            <td>
                                PPN %</td>
                            <td>
                                PPN Forex</td>
                            <td>
                                Total Forex</td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="tbBaseForex" runat="server" CssClass="TextBox" 
                                    style="text-align:right;" ValidationGroup="Input" AutoPostBack="True" />
                            </td>
                            <td>
                                <asp:TextBox ID="tbPPN" runat="server" CssClass="TextBox" 
                                    style="text-align:right;" ValidationGroup="Input" Width="50px" 
                                    AutoPostBack="True" />
                            </td>
                            <td>
                                <asp:TextBox ID="tbPPNForex" runat="server" CssClass="TextBoxR" 
                                    style="text-align:right;" />
                            </td>
                            <td>
                                <asp:TextBox ID="tbTotalForex" runat="server" CssClass="TextBoxR" 
                                    style="text-align:right;" />
                            </td>
                        </tr>
                    </table>
                 </td>
             </tr>  
             <tr>
             <td>
        			Remark</td>
                 <td class="style1">
                     :</td>
                 <td colspan="4">
                     <asp:TextBox ID="tbRemark" runat="server" CssClass="TextBoxMulti" 
                         MaxLength="255" TextMode="MultiLine" ValidationGroup="Input" Width="358px" />
                 </td>
             </tr>
            <tr>
                <td colspan="6">
                    <asp:Button ID="btnSaveNew" runat="server" class="bitbtndt btnsavenew" 
                        Text="Save &amp; New" validationgroup="Input" Width="90" />
                    <asp:Button ID="btnSave" runat="server" class="bitbtndt btnsave" Text="Save" 
                        validationgroup="Input" />
                    <asp:Button ID="btnCancel" runat="server" class="bitbtndt btncancel" 
                        Text="Cancel" validationgroup="Input" />
                    <asp:Button ID="btnHome" runat="server" class="bitbtndt btnback" Text="Home" />
                </td>
            </tr>
        </table>     
     </asp:Panel>          
    </div>   
    <asp:Label ID="lbStatus" runat="server" ForeColor="red" />     
    </form>
</body>
</html>
