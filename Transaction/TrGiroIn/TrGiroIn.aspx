<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TrGiroIn.aspx.vb" Inherits="Transaction_TrGiroIn_TrGiroIn" %>

<%@ Register Assembly="BasicFrame.WebControls.BasicDatePicker" Namespace="BasicFrame.WebControls"
    TagPrefix="BDP" %>
    

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
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
    
        function setformatsetor()
        {
        try
         {        
          var bangcas = document.getElementById("tbBankChargeSetor").value.replace(/\$|\,/g,"");          
          document.getElementById("tbBankChargeSetor").value = setdigit(bangcas,'<%=ViewState("DigitHome")%>');
        }catch (err){
            alert(err.description);
          }      
        }  
        function setformatdrawn()
        {
        try
         {        
          var bangcas = document.getElementById("tbBankChargeDrawn").value.replace(/\$|\,/g,"");          
          document.getElementById("tbBankChargeDrawn").value = setdigit(bangcas,'<%=ViewState("DigitHome")%>');
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
    <div class="H1"><asp:Label runat="server" ID="lblTitle"></asp:Label></div>
    <hr style="color:Blue" />        
    <asp:Panel runat="server" id="PnlHd"> 
      <table>
        <tr>
            <td style="width:100px;text-align:right">Quick Search :</td>            
            <td><asp:TextBox CssClass="TextBox" runat="server" ID ="tbFilter"/>             
                  <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlField" >                      
                    <asp:ListItem Selected="True" Value="GiroNo">Giro No</asp:ListItem>
                    <asp:ListItem>Status</asp:ListItem>
                    <asp:ListItem Value="FgReport">Report</asp:ListItem>
                    <asp:ListItem Value="Usertype">User type</asp:ListItem>                    
                    <asp:ListItem Value="UserCode">User Code</asp:ListItem>
                    <asp:ListItem Value="UserName">User Name</asp:ListItem>
                    <asp:ListItem Value="Reference">Voucher No</asp:ListItem>
                    <asp:ListItem Value="ReceiptNo">Receipt No</asp:ListItem>
                    <asp:ListItem Value="dbo.FormatDate(ReceiptDate)">Receipt Date</asp:ListItem>
                    <asp:ListItem Value="BankGiroName">Bank Giro</asp:ListItem>
                    <asp:ListItem>Currency</asp:ListItem>                    
                    <asp:ListItem Value="dbo.FormatDate(DueDate)">Due Date</asp:ListItem>
                    <asp:ListItem>Remark</asp:ListItem>
                  </asp:DropDownList>            
                  <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlRange"></asp:DropDownList>
                  <asp:Button class="bitbtn btnsearch" runat="server" ID="btnSearch" Text="Search" />                 
                  <asp:Button class="btngo" runat="server" ID="btnExpand" Text="..."/> 
            </td>
            <td>&nbsp;</td>
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
                    <asp:ListItem Selected="True" Value="GiroNo">Giro No</asp:ListItem>
                    <asp:ListItem>Status</asp:ListItem>
                    <asp:ListItem Value="FgReport">Report</asp:ListItem>
                    <asp:ListItem Value="Usertype">User type</asp:ListItem>                    
                    <asp:ListItem Value="UserCode">User Code</asp:ListItem>
                    <asp:ListItem Value="UserName">User Name</asp:ListItem>
                    <asp:ListItem Value="Reference">Voucher No</asp:ListItem>
                    <asp:ListItem Value="ReceiptNo">Receipt No</asp:ListItem>
                    <asp:ListItem Value="dbo.FormatDate(ReceiptDate)">Receipt Date</asp:ListItem>
                    <asp:ListItem Value="BankGiroName">Bank Giro</asp:ListItem>
                    <asp:ListItem>Currency</asp:ListItem>                    
                    <asp:ListItem Value="dbo.FormatDate(DueDate)">Due Date</asp:ListItem>
                    <asp:ListItem>Remark</asp:ListItem>                                   
              </asp:DropDownList>
          </td>              
        </tr>        
      </table>      
      </asp:Panel>
          <br />            
            <asp:DropDownList CssClass="DropDownList" ID="ddlCommand" runat="server" Visible="false"/>
            <asp:Button class="btngo" runat="server" ID="BtnGo" Text="G" Visible="false" />                 
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
                          </asp:DropDownList>
                          <asp:Button class="btngo" runat="server" ID="BtnGo" Text="G" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" CommandName="Go" />                 
                      </ItemTemplate>
                      <HeaderStyle Width="110px" />
                  </asp:TemplateField>
                  <asp:BoundField DataField="GiroNo" HeaderStyle-Width="120px" SortExpression="GiroNo" HeaderText="Giro No"></asp:BoundField>                  
                  <asp:BoundField DataField="Status" HeaderText="Status"></asp:BoundField>                  
                  <asp:BoundField DataField="FgReport" HeaderStyle-Width="80px" HeaderText="Report"></asp:BoundField>
                  <asp:BoundField DataField="Usertype" HeaderStyle-Width="80px" SortExpression="Usertype" HeaderText="User Type"></asp:BoundField>
                  <asp:BoundField DataField="UserCode" HeaderStyle-Width="80px" SortExpression="UserCode" HeaderText="User Code"></asp:BoundField>
                  <asp:BoundField DataField="UserName" HeaderStyle-Width="150px" SortExpression="UserName" HeaderText="User Name"></asp:BoundField>
                  <asp:BoundField DataField="ReceiptNo" HeaderStyle-Width="120px" SortExpression="ReceiptNo" HeaderText="Receipt No"></asp:BoundField>
                  <asp:BoundField DataField="ReceiptDate" HeaderStyle-Width="80px" HtmlEncode="true" DataFormatString="{0:dd MMM yyyy}" SortExpression="ReceiptDate" HeaderText="Receipt Date"></asp:BoundField>
                  <asp:BoundField DataField="BankGiroName" HeaderStyle-Width="150px" SortExpression="BankGiroName" HeaderText="Bank Giro"></asp:BoundField>
                  <asp:BoundField DataField="Reference" HeaderStyle-Width="120px" 
                      SortExpression="Reference" HeaderText="Voucher Bank">
                      <HeaderStyle Width="120px" />
                  </asp:BoundField>
                  <asp:BoundField DataField="Currency" HeaderStyle-Width="80px" SortExpression="Currency" HeaderText="Currency"></asp:BoundField>
                  <asp:BoundField DataField="AmountForex" DataFormatString="{0:#,##0.00}" HeaderStyle-Width="80px" SortExpression="AmountForex" ItemStyle-HorizontalAlign="Right" HeaderText="Amount"></asp:BoundField>
                  <asp:BoundField DataField="DueDate" HtmlEncode="true" DataFormatString="{0:dd MMM yyyy}" HeaderStyle-Width="80px" SortExpression="DueDate" HeaderText="Due Date"></asp:BoundField>
                  <asp:BoundField DataField="Remark" SortExpression="Remark"  HeaderStyle-Width="250px" HeaderText="Remark"></asp:BoundField>
                  <asp:TemplateField Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lbBankReceipt" runat="server" text='<%# DataBinder.Eval(Container.DataItem, "BankReceipt") %>' />
                                <asp:Label ID="lbBankSetor" runat="server" text='<%# DataBinder.Eval(Container.DataItem, "BankSetor") %>' />
                                <asp:Label ID="lbBuktiNo" runat="server" text='<%# DataBinder.Eval(Container.DataItem, "BuktiBankNo") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>   
              </Columns>
          </asp:GridView>
          </div>
            <asp:Panel runat="server" ID ="pnlNav" Visible="false">            
            <asp:DropDownList ID="ddlCommand2" CssClass="DropDownList" runat="server"/>
            <asp:Button class="btngo" runat="server" ID="btnGo2" Text="G"/>                 
            </asp:Panel>
    </asp:Panel>   
    <asp:Panel ID="PnlDt" runat="server" Visible="false">
    <asp:Panel ID="pnlInfo" runat="server" Visible="false">
            <fieldset style="width:90%">
                <legend>Info Giro</legend>                
                <table>
                    <tr>
                        <td>Giro No</td>
                        <td>:</td>
                        <td><asp:Label ID="lbGiroNo" runat="server" /></td>
                    </tr>
                    <tr>
                        <td>Status</td>
                        <td>:</td>
                        <td><asp:Label ID="lbInfoStatus" runat="server" /></td>
                    </tr>
                    <tr>
                        <td>Report</td>
                        <td>:</td>
                        <td><asp:Label ID="lbReport" runat="server" /></td>
                    </tr>
                    <tr>
                        <td>User Type</td>
                        <td>:</td>
                        <td><asp:Label ID="lbUserype" runat="server" /></td>
                    </tr>
                    <tr>
                        <td>User</td>
                        <td>:</td>
                        <td><asp:Label ID="lbUserCode" runat="server" /> -
                            <asp:Label ID="lbUserName" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>Receipt No</td>
                        <td>:</td>
                        <td><asp:Label ID="lbReceiptNo" runat="server" /></td>
                    </tr>
                    <tr>
                        <td>Receipt Date</td>
                        <td>:</td>
                        <td><asp:Label ID="lbReceiptDate" runat="server" /></td>
                    </tr>
                    <tr>
                        <td>Bank Giro</td>
                        <td>:</td>
                        <td><asp:Label ID="lbBankGiroName" runat="server" /></td>
                    </tr>
                    <tr>
                        <td>Curr - Rate</td>
                        <td>:</td>
                        <td><asp:Label ID="lbCurr" runat="server" /> -
                            <asp:Label ID="lbRate" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>Due Date</td>
                        <td>:</td>
                        <td><asp:Label ID="lbDueDate" runat="server" /></td>
                    </tr>
                    <tr>
                        <td>Remark</td>
                        <td>:</td>
                        <td><asp:Label ID="lbRemark" runat="server" /></td>
                    </tr>
                    <tr>
                        <td colspan="3" align="center">
                            <asp:Button class="bitbtn btnsave" runat="server" ID="ImageButton1" Text="Save" ValidationGroup="Input"/>                 
                            <asp:Button class="bitbtn btncancel" runat="server" ID="ImageButton2" Text="Cancel" ValidationGroup="Input"/>                 
                        </td>
                    </tr>
                </table>
            </fieldset></asp:Panel>
         <asp:Panel ID="PnlGiroSelect" runat = "server" Visible="false">
         <div style="border:0px  solid; width:100%; height:100%; overflow:auto;">
          <asp:GridView ID="GridGiro" runat="server" AllowPaging="True" AllowSorting="false"
            CssClass="Grid" AutoGenerateColumns="false"> 
              <HeaderStyle CssClass="GridHeader"></HeaderStyle>
						<RowStyle CssClass="GridItem" Wrap="false" />
						<AlternatingRowStyle CssClass="GridAltItem"/>
						<PagerStyle CssClass="GridPager" />
              <Columns>              
                  <asp:BoundField DataField="GiroNo" HeaderStyle-Width="120px" SortExpression="GiroNo" HeaderText="Giro No"></asp:BoundField>                  
                  <asp:BoundField DataField="Status" HeaderText="Status"></asp:BoundField>                  
                  <asp:BoundField DataField="FgReport" HeaderStyle-Width="80px" HeaderText="Report"></asp:BoundField>
                  <asp:BoundField DataField="Usertype" HeaderStyle-Width="80px" SortExpression="Usertype" HeaderText="User Type"></asp:BoundField>
                  <asp:BoundField DataField="UserCode" HeaderStyle-Width="80px" SortExpression="UserCode" HeaderText="User Code"></asp:BoundField>
                  <asp:BoundField DataField="UserName" HeaderStyle-Width="150px" SortExpression="UserName" HeaderText="User Name"></asp:BoundField>
                  <%--<asp:BoundField DataField="ReceiptNo" HeaderStyle-Width="120px" SortExpression="ReceiptNo" HeaderText="Receipt No"></asp:BoundField>--%>
                  <asp:BoundField DataField="ReceiptDate" HtmlEncode="true" DataFormatString="{0:dd MMM yyyy}" HeaderStyle-Width="80px" SortExpression="ReceiptDate" HeaderText="Receipt Date"></asp:BoundField>
                  <asp:BoundField DataField="BankGiroName" HeaderStyle-Width="150px" SortExpression="BankGiroName" HeaderText="Bank Giro"></asp:BoundField>
                  <asp:BoundField DataField="Currency" HeaderStyle-Width="80px" SortExpression="Currency" HeaderText="Currency"></asp:BoundField>
                  <asp:BoundField DataField="AmountForex" DataFormatString="{0:#,##0.00}" HeaderStyle-Width="80px" SortExpression="AmountForex" ItemStyle-HorizontalAlign="Right" HeaderText="Amount"></asp:BoundField>
                  <asp:BoundField DataField="DueDate" HtmlEncode="true" DataFormatString="{0:dd MMM yyyy}" HeaderStyle-Width="80px" SortExpression="DueDate" HeaderText="Due Date"></asp:BoundField>
                  <%--<asp:BoundField DataField="Remark" SortExpression="Remark"  HeaderStyle-Width="250px" HeaderText="Remark"></asp:BoundField>--%>
              </Columns>
          </asp:GridView>
          </div>
         </asp:Panel> 
        <asp:Panel ID="pnlSetor" runat="server" Visible="false">
            <fieldset style="width:90%">
                <legend>Setor</legend>
                <br />
                <table>
                    <tr>
                        <td>Date To Bank</td>
                        <td>:</td>
                        <td><BDP:BasicDatePicker ID="tbDateToBank" runat="server" DateFormat="dd MMM yyyy" 
                            ReadOnly = "true" ValidationGroup="Input"
                            ButtonImageHeight="19px" ButtonImageWidth="20px" 
                            DisplayType="TextBoxAndImage" 
                            TextBoxStyle-CssClass="TextDate" 
                            ShowNoneButton="False"><TextBoxStyle CssClass="TextDate" /></BDP:BasicDatePicker>
                        </td>
                    </tr>
                    <tr>
                        <td>Bank Receipt</td>
                        <td>:</td>
                        <td><asp:DropDownList ID="ddlBankReceiptSetor" ValidationGroup="Input" AutoPostBack ="true" Width = "200px"  CssClass="DropDownList" runat="server" /></td>
                    </tr>
                    <tr>
                        <td>Bank Setor</td>
                        <td>:</td>
                        <td><asp:DropDownList ID="ddlBankSetorSetor" ValidationGroup="Input" Width = "200px" CssClass="DropDownList" runat="server" /></td>
                    </tr>
                    <tr>
                        <td>Bukti Terima Bank</td>
                        <td>:</td>
                        <td><asp:TextBox runat="server" ID="tbBuktiBankNoSetor" ValidationGroup="Input" CssClass="TextBox" /></td>
                    </tr>
                    <tr>
                        <td>Bank Charge</td>
                        <td>:</td>
                        <td><asp:TextBox runat="server" ID="tbBankChargeSetor" ValidationGroup="Input" CssClass="TextBox"/></td>
                    </tr>
                    <tr>
                        <td colspan="3" align="center">
                            <asp:Button class="bitbtn btnsave" runat="server" ID="btnSaveSetor" Text="Save" ValidationGroup="Input"/>                 
                            <asp:Button class="bitbtn btncancel" runat="server" ID="btnCancelSetor" Text="Cancel" ValidationGroup="Input"/>                 
                        </td>
                    </tr>
                </table>
            </fieldset></asp:Panel>
        <asp:Panel ID="pnlDrawn" runat = "server" Visible="false">
            <fieldset style="width:90%">
                <legend>Drawn</legend>
                <br />
                <table>
                    <tr>
                        <td>Voucher Bank</td>
                        <td>:</td>
                        <td><asp:TextBox runat="server" ID="tbVoucherNmbr" Width="150px" Enabled="True" 
                                CssClass="TextBox" ValidationGroup="Input"/></td>
                    </tr>
                    <tr>                    
                        <td>Date To Drawn</td>
                        <td>:</td>
                        <td><BDP:BasicDatePicker ID="tbDateToDrawn" runat="server" DateFormat="dd MMM yyyy" 
                            ReadOnly = "true" ValidationGroup="Input" AutoPostBack="true"
                            ButtonImageHeight="19px" ButtonImageWidth="20px" 
                            DisplayType="TextBoxAndImage" 
                            TextBoxStyle-CssClass="TextDate" 
                            ShowNoneButton="False"><TextBoxStyle CssClass="TextDate" /></BDP:BasicDatePicker>
                        </td>
                    </tr>
                    <tr>
                        <td>Bank Receipt</td>
                        <td>:</td>
                        <td><asp:DropDownList ID="ddlBankReceiptDrawn" ValidationGroup="Input" AutoPostBack="true" Width = "200px" CssClass="DropDownList" runat="server" /></td>
                    </tr>
                    <tr>
                        <td>Bank Setor</td>
                        <td>:</td>
                        <td><asp:DropDownList ID="ddlBankSetorDrawn" ValidationGroup="Input" Width = "200px" CssClass="DropDownList" runat="server" /></td>
                    </tr>
                    <tr>
                        <td>Bukti Terima Bank</td>
                        <td>:</td>
                        <td><asp:TextBox runat="server" ID="tbBuktiBankNoDrawn" ValidationGroup="Input" CssClass="TextBox"/></td>
                    </tr>
                    <tr>
                        <td>Bank Charge</td>
                        <td>:</td>
                        <td><asp:TextBox runat="server" ID="tbBankChargeDrawn" ValidationGroup="Input" CssClass="TextBox"/></td>
                    </tr>
                    <tr>
                        <td>Remark</td>
                        <td>:</td>
                        <td><asp:TextBox runat="server" ID="tbRemarkDrawn" ValidationGroup="Input" MaxLength="255" Width="250px" CssClass="TextBox"/></td>
                    </tr>
                    <tr>
                        <td colspan="3" align="center">
                            <asp:Button class="bitbtn btnsave" runat="server" ID="btnSaveDrawn" Text="Save" ValidationGroup="Input"/>                 
                            <asp:Button class="bitbtn btncancel" runat="server" ID="btnCancelDrawn" Text="Cancel" ValidationGroup="Input"/>                 
                        </td>
                    </tr>
                </table>
            </fieldset></asp:Panel>     
        <asp:Panel runat="server" ID="pnlCancel" Visible="false">
            <fieldset style="width:90%">
                <legend>Cancel</legend>
                <br />
                <table>
                    <tr>
                        <td>Date Cancel</td>
                        <td>:</td>
                        <td><BDP:BasicDatePicker ID="tbDateCancel" runat="server" DateFormat="dd MMM yyyy" 
                            ReadOnly = "true" ValidationGroup="Input"
                            ButtonImageHeight="19px" ButtonImageWidth="20px" 
                            DisplayType="TextBoxAndImage" 
                            TextBoxStyle-CssClass="TextDate" 
                            ShowNoneButton="False"><TextBoxStyle CssClass="TextDate" /></BDP:BasicDatePicker>
                        </td>
                    </tr>
                    <tr>
                        <td>Reason</td>
                        <td>:</td>
                        <td><asp:TextBox runat="server" ValidationGroup="Input" ID="tbCancelReason" MaxLength="255" Width="250px" CssClass="TextBox"/></td>
                    </tr>
                    <tr>
                        <td colspan="3" align="center">
                            <asp:Button class="bitbtn btnsave" runat="server" ID="btnSaveCancel" Text="Save" ValidationGroup="Input"/>                 
                            <asp:Button class="bitbtn btncancel" runat="server" ID="btnCancelCancel" Text="Cancel" ValidationGroup="Input"/>                 
                        </td>
                    </tr>
                </table>
            </fieldset></asp:Panel>
            <asp:Button class="btngo" runat="server" ID="btnHome" Text="Home" 
            Width="49px" />                 
    </asp:Panel>    
    </div>
    <asp:Label ID="lbStatus" runat="server" ForeColor="Red" />
    </form>
</body>
</html>
