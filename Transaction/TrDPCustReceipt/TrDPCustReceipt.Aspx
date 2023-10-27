<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TrDPCustReceipt.Aspx.vb" Inherits="TrDPCustReceipt" %>
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
            //var _tempPPnRate = parseFloat(document.getElementById("tbPPNRate").value.replace(/\$|\,/g,""));
            
            _tempppnforex = _tempbaseforex*_tempppn/100;
            _temptotalforex = _tempbaseforex*_tempppn/100 + _tempbaseforex;
                        
            document.getElementById("tbPPN").value = setdigit(_tempppn, '<%=ViewState("DigitPercent")%>');
            document.getElementById("tbBaseForex").value = setdigit(_tempbaseforex, '<%=ViewState("DigitCurr")%>');            
            document.getElementById("tbPPNForex").value = setdigit(_tempppnforex, '<%=ViewState("DigitCurr")%>');
            document.getElementById("tbTotalForex").value = setdigit(_temptotalforex, '<%=ViewState("DigitCurr")%>');
            document.getElementById("tbRate").value = setdigit(_tempRate,'<%=ViewState("DigitRate")%>');
            //document.getElementById("tbPPNRate").value = setdigit(_tempPPnRate,'<%=ViewState("DigitRate")%>');                       
          }catch (err){
            alert(err.description);
          }
        }
        function setformatDt(){   
          try
          { 
            var _tempRate2 = parseFloat(document.getElementById("tbRate2").value.replace(/\$|\,/g,""));
            var _tempChargeRate = parseFloat(document.getElementById("tbChargeRate").value.replace(/\$|\,/g,""));
            var _tempReceiptForex = parseFloat(document.getElementById("tbReceiptForex").value.replace(/\$|\,/g,""));
            var _tempReceiptHome = parseFloat(document.getElementById("tbReceiptHome").value.replace(/\$|\,/g,""));
            
            
            document.getElementById("tbReceiptForex").value = setdigit(_tempbaseforex, '<%=ViewState("DigitCurr")%>');            
            document.getElementById("tbReceiptHome").value = setdigit(_tempppnforex, '<%=ViewState("DigitCurr")%>');
            document.getElementById("tbRate2").value = setdigit(_tempRate,'<%=ViewState("DigitRate")%>');
            document.getElementById("tbChargeRate").value = setdigit(_tempChargeRate,'<%=ViewState("DigitRate")%>');
          }catch (err){
            alert(err.description);
          }
        }        
    </script>
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
    
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
    <div class="H1"><asp:Label runat="server" ID="lbTitle"></asp:Label></div>
     <hr style="color:Blue" />        
    <asp:Panel runat="server" id="PnlHd"> 
      <table>
        <tr>
            <td style="width:100px;text-align:right">Quick Search :</td>            
            <td><asp:TextBox CssClass="TextBox" runat="server" ID ="tbFilter"/> 
                  <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlField" >
                      <asp:ListItem Value="TransNmbr" Selected="True">DP No</asp:ListItem>
                      <asp:ListItem Value="dbo.FormatDate(TransDate)">DP Date</asp:ListItem>
                      <asp:ListItem Value="FgReport">Report</asp:ListItem>
                      <asp:ListItem Value="Status">Status</asp:ListItem>
                      <asp:ListItem Value="Customer_Name">Customer</asp:ListItem>    
                      <asp:ListItem Value="DPListNo">DP Inv No</asp:ListItem>                          
                      <asp:ListItem Value="Attn">Attention</asp:ListItem>    
                      <asp:ListItem Value="SONo">SO No</asp:ListItem>    
                      <asp:ListItem Value="PPNNo">PPN No</asp:ListItem>    
                      <asp:ListItem Value="dbo.FormatDate(PPnDate)">PPN Date</asp:ListItem>    
                      <asp:ListItem Value="PPNRate">PPN Rate</asp:ListItem>    
                      <asp:ListItem Value="Currency">Currency</asp:ListItem>    
                      <asp:ListItem Value="ForexRate">Rate</asp:ListItem>    
                      <asp:ListItem Value="BaseForex">Base Forex</asp:ListItem>    
                      <asp:ListItem Value="PPN">PPN</asp:ListItem>    
                      <asp:ListItem Value="PPNForex">PPN Forex</asp:ListItem>    
                      <asp:ListItem Value="TotalForex">Total Forex</asp:ListItem>    
                      <asp:ListItem Value="Remark">Remark</asp:ListItem>     
                      <asp:ListItem Value="Reference">Voucher No</asp:ListItem>                                                                  
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
            <td><asp:TextBox CssClass="TextBox" runat="server" ID ="tbFilter2"/> 
                  <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlField2" >
                      <asp:ListItem Value="TransNmbr" Selected="True">DP No</asp:ListItem>
                      <asp:ListItem Value="dbo.FormatDate(TransDate)">DP Date</asp:ListItem>
                      <asp:ListItem Value="FgReport">Report</asp:ListItem>
                      <asp:ListItem Value="Status">Status</asp:ListItem>
                      <asp:ListItem Value="Customer_Name">Customer</asp:ListItem>    
                      <asp:ListItem Value="DPListNo">DP Inv No</asp:ListItem>                          
                      <asp:ListItem Value="Attn">Attention</asp:ListItem>    
                      <asp:ListItem Value="SONo">SO No</asp:ListItem>    
                      <asp:ListItem Value="PPNNo">PPN No</asp:ListItem>    
                      <asp:ListItem Value="dbo.FormatDate(PPnDate)">PPN Date</asp:ListItem>    
                      <asp:ListItem Value="PPNRate">PPN Rate</asp:ListItem>    
                      <asp:ListItem Value="Currency">Currency</asp:ListItem>    
                      <asp:ListItem Value="ForexRate">Rate</asp:ListItem>    
                      <asp:ListItem Value="BaseForex">Base Forex</asp:ListItem>    
                      <asp:ListItem Value="PPN">PPN</asp:ListItem>    
                      <asp:ListItem Value="PPNForex">PPN Forex</asp:ListItem>    
                      <asp:ListItem Value="TotalForex">Total Forex</asp:ListItem>    
                      <asp:ListItem Value="Remark">Remark</asp:ListItem> 
                      <asp:ListItem Value="Reference">Voucher No</asp:ListItem>                                                                     
                    </asp:DropDownList>
          </td>              
        </tr>        
      </table>      
      </asp:Panel>
          <br />
            <asp:Button class="bitbtn btnadd" runat="server" ID="BtnAdd" Text="Add" />  
            &nbsp &nbsp &nbsp   
            <asp:DropDownList CssClass="DropDownList" ID="ddlCommand" runat="server" Visible="false"/>
            <asp:Button class="btngo" runat="server" ID="BtnGo" visible=false Text="G"/>       
          <br />
          <div style="border:0px  solid; width:100%; height:100%; overflow:auto;">
          </div>
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
                              <asp:ListItem Text="Print" />
                          </asp:DropDownList>
                          <asp:Button class="btngo" runat="server" ID="BtnGo" Text="G" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" CommandName="Go" />
                          
                      </ItemTemplate>
                      <HeaderStyle Width="110px" />
                  </asp:TemplateField>
                    <asp:BoundField DataField="TransNmbr" HeaderStyle-Width="320px" 
                        HeaderText="DP No" SortExpression="Nmbr" />
                    <asp:BoundField DataField="Status" HeaderStyle-Width="10px" HeaderText="Status" 
                        SortExpression="Status" />
                    <asp:BoundField DataField="TransDate" HtmlEncode="true" DataFormatString="{0:dd MMM yyyy}" HeaderStyle-Width="80px" 
                        HeaderText="Date" SortExpression="TransDate" />
                    <asp:BoundField DataField="FgReport" HeaderStyle-Width="10px" 
                        HeaderText="Report" SortExpression="FgReport" />
                    <asp:BoundField DataField="Customer_Name" HeaderStyle-Width="250px" 
                        HeaderText="Customer" SortExpression="Customer_Name" />
                    <asp:BoundField DataField="DPListNo" HeaderStyle-Width="250px" HeaderText="DP Inv No." SortExpression="DPListNo" />
                    <asp:BoundField DataField="Attn" HeaderStyle-Width="250px" HeaderText="Attention" SortExpression="Attn" />
                    <asp:BoundField DataField="SONo" HeaderStyle-Width="150px" HeaderText="PO No" SortExpression="SONo" />
                    <%--<asp:BoundField DataField="PPNNo" HeaderStyle-Width="150px" HeaderText="PPN No" SortExpression="PPNNo" />
                    <asp:BoundField DataField="PPNDate" HtmlEncode="true" DataFormatString="{0:dd MMM yyyy}" HeaderStyle-Width="80px" HeaderText="PPN Date" SortExpression="PPNDate" />
                    <asp:BoundField DataField="PPNRate" DataFormatString="{0:#,##0.####}" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="80px" HeaderText="PPN Rate" SortExpression="PPNRate" />--%>
                    <asp:BoundField DataField="Currency" HeaderStyle-Width="15px" HeaderText="Currency" SortExpression="Currency" />
                    <asp:BoundField DataField="BaseForex" DataFormatString="{0:#,##0.00}" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="80px" HeaderText="Base Forex" SortExpression="BaseForex" />
                    <asp:BoundField DataField="PPn" DataFormatString="{0:#,##0.####}" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="50px" HeaderText="PPn" SortExpression="PPn" />
                    <asp:BoundField DataField="PPnForex" DataFormatString="{0:#,##0.00}" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="80px" HeaderText="PPn Forex" SortExpression="PPnForex" />
                    <asp:BoundField DataField="TotalForex" HeaderStyle-Width="80px" HeaderText="Total Forex" DataFormatString="{0:#,##0.00}" ItemStyle-HorizontalAlign="Right" SortExpression="TotalForex" />
                    <asp:BoundField DataField="Remark" HeaderStyle-Width="200px" HeaderText="Remark" SortExpression="Remark" />                  
              </Columns>
          </asp:GridView>
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
                <td>DP No</td>
                <td class="style1">:</td>
                <td><asp:TextBox ID="tbTransNo" ValidationGroup="Input" runat="server" 
                        CssClass ="TextBoxR" Enabled="False" ReadOnly="True" Width="150px" /></td>
                
                <td>Date</td>
                <td>:</td>
                <td>
                    <BDP:BasicDatePicker ID="tbTransDate" runat="server" DateFormat="dd MMM yyyy" 
                        ShowNoneButton = "false" ReadOnly = "true" ValidationGroup="Input"
                        ButtonImageHeight="19px" ButtonImageWidth="20px" 
                        DisplayType="TextBoxAndImage" 
                        TextBoxStyle-CssClass="TextDate" AutoPostBack="True"><TextBoxStyle CssClass="TextDate" /></BDP:BasicDatePicker>
                </td>
            </tr>
          <%--<tr>
              <td>
                  Fg Report
              </td>
              <td class="style1">
                  :
              </td>
              <td>
                  <asp:DropDownList AutoPostBack="true" ValidationGroup="Input" CssClass="DropDownList"
                      ID="ddlReport" runat="server">
                      <asp:ListItem Selected="True">Y</asp:ListItem>
                      <asp:ListItem>N</asp:ListItem>
                  </asp:DropDownList>
              </td>
          </tr>--%>
             <tr>
                <td>Customer</td>
                <td class="style1">:</td>
                <td colspan="4">
                <asp:TextBox ID="tbCustCode" ValidationGroup="Input" AutoPostBack="true" Width="100" runat="server" CssClass="TextBox" />
                <asp:TextBox ID="tbCustName" Width="175" runat="server" 
                        ReadOnly="true" CssClass="TextBoxR" />
                <asp:Button class="btngo" runat="server" ID="btnCust" Text="..."/>
                <asp:Label ID="Label1" runat="server" ForeColor="#FF3300" Text="*"></asp:Label>                    
                </td>         
                
             </tr>
             <tr>
                <td>Attention</td>
                <td class="style1">:</td>
                <td><asp:TextBox ID="tbAttn" ValidationGroup="Input"
                        runat="server" CssClass="TextBox" Width="150" /></td>
             </tr>             
             <tr>
                <td>DP Inv No.</td>
                <td class="style1">:</td>
                <td><asp:TextBox ID="tbDPList" ValidationGroup="Input"
                        runat="server" CssClass="TextBoxR" Width="150" Enabled="False" 
                        ReadOnly="True" />
                <asp:Button class="btngo" runat="server" ID="btnDPList" Text="..."/>
                <asp:Label ID="Label11" runat="server" ForeColor="#FF3300" Text="*"></asp:Label>
                </td>
             </tr>                          
             <tr>
                <td>SO No</td>
                <td class="style1">:</td>
                <td><asp:TextBox ID="tbSONo" ValidationGroup="Input"
                        runat="server" CssClass="TextBoxR" Width="150" Enabled="False" 
                        ReadOnly="True" /></td>
             </tr>                                       
             <tr>
                <td><asp:LinkButton ID="lbCurr"  runat="server" Text="Currency"/></td>
                <td class="style1">:</td>
                <td><asp:DropDownList ID="ddlCurr" Enabled = False runat="server" CssClass="DropDownList" />
                </td>
                <td>Forex Rate</td>
                <td>:</td>
                <td><asp:TextBox ID="tbRate" ValidationGroup="Input" runat="server" CssClass="TextBox" /></td>
             </tr>
             <tr>
                <%--<td>PPN</td>
                <td class="style1">:</td>--%>
                <td colspan="4">
                    <table>
                        <%--<tr style="background-color:Silver;text-align:center">
                            <td>No</td>
                            <td>Date</td>
                            <td>Rate</td>
                        </tr>--%>
                        <tr>
                            <td><asp:TextBox id="tbPPNNo"  visible="false" ValidationGroup="Input" runat="server" CssClass="TextBox"/></td>
                            <td>                                
                                <BDP:BasicDatePicker ID="tbPPNDate"  visible="false" ValidationGroup="Input" runat="server" DateFormat="dd MMM yyyy" ButtonImageHeight="19px" ButtonImageWidth="20px" 
                                    DisplayType="TextBoxAndImage" ReadOnly="true" TextBoxStyle-CssClass="TextDate"></BDP:BasicDatePicker>
                            </td>
                            <td><asp:TextBox ID="tbPPNRate"  visible="false" ValidationGroup="Input" runat="server" CssClass="TextBox" /></td>
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
                            <td>Base Forex</td>
                            <td>PPN %</td>
                            <td>PPN Forex</td>
                            <td>Total Forex</td>
                        </tr>
                        <tr>
                            <td><asp:TextBox ID="tbBaseForex" runat="server" CssClass="TextBoxR"/></td>
                            <td><asp:TextBox ID="tbPPN" ValidationGroup="Input" runat="server" CssClass="TextBox" /></td>
                            <td><asp:TextBox ID="tbPPNForex" runat="server" CssClass="TextBoxR"/></td>
                            <td><asp:TextBox ID="tbTotalForex" runat="server" CssClass="TextBoxR" /></td>
                        </tr>
                    </table>
                </td>                
             </tr>
             <tr>
                <td>Remark</td>
                <td class="style1">:</td>
                <td colspan="4"><asp:TextBox ID="tbRemark" ValidationGroup="Input" Width="358px" runat="server" 
                        CssClass="TextBox" /></td>
             </tr>  
      </table>  
      
      <br />      
      <div style="font-size:medium; color:Blue;">Detail</div>
      <hr style="color:Blue" />  
        <asp:Panel runat="server" ID="pnlDt">
            <asp:Button class="bitbtndt btnadd" runat="server" ID="btnAdddt" Text="Add" ValidationGroup="Input" />
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
                                <asp:Button ID="btnEdit" runat="server" class="bitbtndt btnedit" Text="Edit" CommandName="Edit" />
                                    <asp:Button ID="btnDelete" runat="server" class="bitbtndt btndelete" Text="Delete"
                                        OnClientClick="return confirm('Sure to delete this data?');" CommandName="Delete" />   
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:Button ID="btnUpdate" runat="server" class="bitbtndt btnsave" Text="Update" CommandName="Update"/>
                                <asp:Button ID="btnCancel" runat="server" class="bitbtndt btncancel" Text="Cancel" CommandName="Cancel"/>      
                            </EditItemTemplate>
                                                    
                        </asp:TemplateField>                       
                        <asp:BoundField DataField="ItemNo" HeaderStyle-Width="80px" HeaderText="No" >
                            <HeaderStyle Width="80px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="ReceiptType" HeaderStyle-Width="80px"  
                            HeaderText="Receipt Type Code" >
                            <HeaderStyle Width="200px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="ReceiptTypeName" HeaderStyle-Width="200px"  
                            HeaderText="Receipt Type Name" >                        
                            <HeaderStyle Width="200px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="ReceiptDate" HeaderText="Receipt Date" Visible ="false" />                        
                        <asp:BoundField DataField="DocumentNo" HeaderText="Document No" />                                                
                        <asp:BoundField DataField="Reference" HeaderText="Voucher No" HeaderStyle-Width="120px"  />                                                
                        <asp:BoundField DataField="BankGiroName" HeaderText="Bank Receipt" />
                        <asp:BoundField DataField="DueDate" HeaderText="Due Date" />
                        <asp:BoundField DataField="Currency" HeaderText="Currency" />                           
                        <asp:BoundField DataField="ForexRate" ItemStyle-HorizontalAlign="Right" HeaderText="Rate" />    
                        <asp:BoundField DataField="ReceiptForex" ItemStyle-HorizontalAlign="Right" HeaderText="Receipt Forex" />    
                        <asp:BoundField DataField="ReceiptHome" ItemStyle-HorizontalAlign="Right" HeaderText="Receipt Home" />
                        <asp:BoundField DataField="ChargeHome" ItemStyle-HorizontalAlign="Right" HeaderText="Charge Home" />
                        <asp:BoundField DataField="DPHome" ItemStyle-HorizontalAlign="Right" HeaderText="DP Home" />                            
                        <asp:BoundField DataField="Remark" HeaderText="Remark" 
                            HeaderStyle-Width="250px" >                            
                            <HeaderStyle Width="250px" />
                        </asp:BoundField>
                        
                    </Columns>
                </asp:GridView>
          </div>   
          <asp:Button class="bitbtndt btnadd" runat="server" ID="btnAddDt2" Text="Add" ValidationGroup="Input" />  
       </asp:Panel>             
       <asp:Panel runat="server" ID="pnlEditDt" Visible="false">
                <table>              
                    <tr>
                        <td>Item No</td>
                        <td>:</td>
                        <td colspan="4"><asp:Label ID="lbItemNo" runat="server" Text="itemmm" />
                        </td>           
                    </tr>        
                    <tr>
                        <td>Receipt Type</td>
                        <td>:</td>
                        <td><asp:DropDownList CssClass="DropDownList" ID="ddlReceiptType" runat="server" Width="190px" AutoPostBack ="true"/>
                            <asp:TextBox CssClass ="TextBox" ID = "tbFgMode" runat ="server" Visible="false" />
                        </td>
                            
                        <td></td>
                        <td></td>
                        <td>
                             <BDP:BasicDatePicker ID="tbReceiptDate" runat="server" DateFormat="dd MMM yyyy" 
                             ReadOnly = "true" ValidationGroup="Input" ButtonImageHeight="19px" ButtonImageWidth="20px" 
                             DisplayType="TextBoxAndImage" TextBoxStyle-CssClass="TextDate" Visible="false"
                             ShowNoneButton="False"><TextBoxStyle CssClass="TextDate" /></BDP:BasicDatePicker>
                        </td>
                    </tr>
                    <tr>
                        <td>Document No</td>
                        <td>:</td>
                        <td><asp:TextBox CssClass="TextBox" runat="server" ID="tbDocNo" Width="157px"/> 
                        </td>
                        <td>Voucher No</td>
                        <td>:</td>
                        <td><asp:TextBox CssClass="TextBox" runat="server" ID="tbVoucherNo" Width="157px"/>                            
                        </td>
                    </tr>
                    <tr>
                        <td>Bank Receipt</td>
                        <td>:</td>
                        <td><asp:DropDownList CssClass="DropDownList" ID="ddlBankGiro" runat="server"/></td>
                    
                        <td>Due Date</td>
                        <td>:</td>
                        <td>
                             <BDP:BasicDatePicker ID="tbDueDate" runat="server" DateFormat="dd MMM yyyy" 
                             ReadOnly = "true" ButtonImageHeight="19px" ButtonImageWidth="20px" 
                             DisplayType="TextBoxAndImage" TextBoxStyle-CssClass="TextDate"
                             ShowNoneButton="False"><TextBoxStyle CssClass="TextDate" /></BDP:BasicDatePicker>
                        </td>    
                        
                    </tr>   
                    <tr> 
                        <td><asp:LinkButton ID="lbCurrency2"  runat="server" Text="Currency"/></td>

                        <td>:</td>
                        <td colspan="4">
                            <table>
                                <tr style="background-color:Silver;text-align:center">
                                    <td>Receipt</td>
                                    <td>Rate</td>
                                    <td>Charge</td>
                                    <td>Rate</td>                                
                                </tr>
                                <tr>
                                    <td><asp:DropDownList CssClass="DropDownList" ID="ddlCurr2" runat="server" 
                                            Enabled="false" Width="60px"/></td>
                                    <td><asp:TextBox CssClass="TextBox" runat="server" ID="tbRate2" Width="65px"/></td>
                                    <td><asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlChargeCurr" 
                                            Width="60px" AutoPostBack="True"/></td>
                                    <td><asp:TextBox CssClass="TextBox" runat="server" ID="tbChargeRate" Width="65px"/></td>                                    
                                </tr>
                            </table>
                        </td>
                        
                        
                    </tr>   
                    <tr>
                        <td>Nominal</td>
                        <td>:</td>
                        <td colspan="4">
                            <table>
                                <tr style="background-color:Silver;text-align:center">
                                    <td><asp:Label ID="lbPayForex" runat="server" CssClass="TextBox" Text = "Receipt Forex"/></td>
                                    <td><asp:Label ID="lbPayHome" runat="server" CssClass="TextBox" Text = "Receipt Home"/></td>   
                                    <td><asp:Label ID="lbChargeForex" runat="server" CssClass="TextBox" Text = "Charge Forex"/></td>
                                    <td><asp:Label ID="lbChargeHome" runat="server" CssClass="TextBox" Text = "Charge Home"/></td>
                                    <td><asp:Label ID="lbDPForex" runat="server" CssClass="TextBox" Text = "DP Forex"/></td>
                                    <td><asp:Label ID="lbDPHome" runat="server" CssClass="TextBox" Text = "DP Home"/></td>                                                                                                         
                                </tr>
                                <tr>
                                    <td><asp:TextBox CssClass="TextBox" runat="server" ID="tbReceiptForex" Width="80px" 
                                            AutoPostBack="True"/></td>                                    
                                    <td><asp:TextBox CssClass="TextBoxR" runat="server" ID="tbReceiptHome" Width="80px"/></td>
                                    <td><asp:TextBox CssClass="TextBox" runat="server" ID="tbChargeForex" Width="80px" 
                                            AutoPostBack="True"/></td>
                                    <td><asp:TextBox CssClass="TextBoxR" runat="server" ID="tbChargeHome" Width="80px"/></td>
                                    <td><asp:TextBox CssClass="TextBoxR" runat="server" ID="tbDPForex" Width="80px"/></td>
                                    <td><asp:TextBox CssClass="TextBoxR" runat="server" ID="tbDPHome" Width="80px"/></td>                                                                                                            
                                </tr>
                            </table>
                        </td>
                    </tr>                                   
                    <tr>
                        <td>Remark </td>
                        <td>:</td>
                        <td colspan="4"><asp:TextBox runat="server" ID="tbRemarkDt" CssClass="TextBox" Width="365px" 
                                MaxLength="255" TextMode="MultiLine" />                        
                        </td>
                    </tr>
                </table>
            <br />                     
            <asp:Button ID="btnSaveDt" runat="server" class="bitbtndt btnsave" Text="Save"/> &nbsp;         
            <asp:Button ID="btnCancelDt" runat="server" class="bitbtndt btncancel" Text="Cancel"/> 
       </asp:Panel> 
       <br />          
       <asp:Button ID="btnSaveAll" runat="server" class="bitbtndt btnsave" Text="Save & New" ValidationGroup="Input" Width="97px"/> &nbsp;    
                    <asp:Button ID="btnSaveTrans" runat="server" class="bitbtndt btnsave" Text="Save" ValidationGroup="Input"/> &nbsp;    
                    <asp:Button ID="btnBack" runat="server" class="bitbtndt btncancel" Text="Cancel" ValidationGroup="Input"/>  &nbsp;
                    <asp:Button ID="btnHome" runat="server" class="bitbtndt btncancel" Text="Cancel"/>    
    </asp:Panel>
    <asp:Panel runat="server" ID="pnlPrint" Visible="false">
    </asp:Panel>
      <br />            
    </div>
   
    <asp:Label runat ="server" ID="lbStatus" ForeColor="Red"/>
   
    </form>
    </body>
</html>
