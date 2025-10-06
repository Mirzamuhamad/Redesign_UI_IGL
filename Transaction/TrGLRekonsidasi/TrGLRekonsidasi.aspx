<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TrGLRekonsidasi.aspx.vb" Inherits="Transaction_TrGLRekonsidasi_TrGLRekonsidasi" %>

<%@ Register Assembly="BasicFrame.WebControls.BasicDatePicker" Namespace="BasicFrame.WebControls"
    TagPrefix="BDP" %>
<%@ Register assembly="FastReport" namespace="FastReport.Web" tagprefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Audit</title>
    <script src="../../Function/OpenDlg.js" type="text/javascript"></script>
    <script src="../../Function/Function.JS" type="text/javascript"></script>
    <script type="text/javascript">
        function setdigit(nStr, digit) {
            try {
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
            } catch (err) {
                alert(err.description);
            }
        }

        function setformat() {

            try {
                document.getElementById("tbOldLimit").value = setdigit(document.getElementById("tbOldLimit").value.replace(/\$|\,/g, ""), '<%=VIEWSTATE("DigitCurr")%>');
                document.getElementById("tbNewLimit").value = setdigit(document.getElementById("tbNewLimit").value.replace(/\$|\,/g, ""), '<%=VIEWSTATE("DigitCurr")%>');
                document.getElementById("tbOldUsed").value = setdigit(document.getElementById("tbOldUsed").value.replace(/\$|\,/g, ""), '<%=VIEWSTATE("DigitCurr")%>');
                document.getElementById("tbSaldo").value = setdigit(document.getElementById("tbSaldo").value.replace(/\$|\,/g, ""), '<%=VIEWSTATE("DigitCurr")%>');
            } catch (err) {
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
    <div class="H1"><b style="mso-bidi-font-weight:normal">
        <span style="font-size:12.0pt;font-family:&quot;Times New Roman&quot;;mso-fareast-font-family:
&quot;Times New Roman&quot;;mso-ansi-language:EN-US;mso-fareast-language:EN-US;
mso-bidi-language:AR-SA">Audit </span></b></div>
     <hr style="color:Blue" />        
    <asp:Panel runat="server" id="PnlHd"> 
      <table>
        <tr>
            <td style="width:100px;text-align:right">Quick Search :</td>            
            <td><asp:TextBox CssClass="TextBox" runat="server" ID ="tbFilter"/> 
                  <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlField" >
                      <asp:ListItem Value="RekNo" Selected="True">Rek No</asp:ListItem>
                      <asp:ListItem Value="dbo.FormatDate(BankDate)">Bank Date</asp:ListItem>
                      <asp:ListItem Value="TransClass">Type Transaction</asp:ListItem>
                      <asp:ListItem Value="Reference">Reference</asp:ListItem>
                      <asp:ListItem Value="Remark">Remark</asp:ListItem>                      
                      <asp:ListItem Value="SubLed">Subled</asp:ListItem>
                      <asp:ListItem Value="SubLedName">Subled Name</asp:ListItem>
                      <asp:ListItem Value="ItemNo">No</asp:ListItem>
                      <asp:ListItem Value="Currency">Currency</asp:ListItem>
                      <asp:ListItem Value="ForexRate">Rate</asp:ListItem>
                      <asp:ListItem Value="DebitForex">Debit</asp:ListItem>
                      <asp:ListItem Value="CreditForex">Credit</asp:ListItem>
                      <asp:ListItem Value="Detail">Detail</asp:ListItem>
                    </asp:DropDownList>
                  <asp:Button class="bitbtn btnsearch btnsearch" runat="server" ID="btnSearch" Text="Search" />
                <asp:Button class="btngo btngo" runat="server" ID="btnExpand" Text="..."/>
            </td>
            <td>
                <asp:LinkButton ID="LbAdvSearch" runat="server" Text="Advanced Search" Visible ="false" />
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
                  <asp:ListItem Selected="True" Value="RekNo">Rek No</asp:ListItem>
                      <asp:ListItem Value="RekNo" Selected="True">Rek No</asp:ListItem>
                      <asp:ListItem Value="dbo.FormatDate(BankDate)">Bank Date</asp:ListItem>
                      <asp:ListItem Value="TransClass">Type Transaction</asp:ListItem>
                      <asp:ListItem Value="Reference">Reference</asp:ListItem>
                      <asp:ListItem Value="Remark">Remark</asp:ListItem>                      
                      <asp:ListItem Value="SubLed">Subled</asp:ListItem>
                      <asp:ListItem Value="SubLedName">Subled Name</asp:ListItem>
                      <asp:ListItem Value="ItemNo">No</asp:ListItem>
                      <asp:ListItem Value="Currency">Currency</asp:ListItem>
                      <asp:ListItem Value="ForexRate">Rate</asp:ListItem>
                      <asp:ListItem Value="DebitForex">Debit</asp:ListItem>
                      <asp:ListItem Value="CreditForex">Credit</asp:ListItem>
                      <asp:ListItem Value="Detail">Detail</asp:ListItem>
                  </asp:DropDownList>
          </td>              
        </tr>        
      </table>      
      </asp:Panel>
          <br />
            <table>
        <tr>
            <td>Period</td>
            <td>:</td>
            <td>
            <BDP:BasicDatePicker ID="tbDate" runat="server" DateFormat="dd MMM yyyy" 
                        ShowNoneButton = "false" ReadOnly = "true" ValidationGroup="Input"
                        ButtonImageHeight="19px" ButtonImageWidth="20px" 
                        DisplayType="TextBoxAndImage" 
                        TextBoxStyle-CssClass="TextDate" AutoPostBack="True">
                        <TextBoxStyle CssClass="TextDate" />
             </BDP:BasicDatePicker>                
            </td>            
            <td>
                <BDP:BasicDatePicker ID="tbDateE" runat="server" AutoPostBack="True" 
                    ButtonImageHeight="19px" ButtonImageWidth="20px" DateFormat="dd MMM yyyy" 
                    DisplayType="TextBoxAndImage" ReadOnly="true" ShowNoneButton="false" 
                    TextBoxStyle-CssClass="TextDate" ValidationGroup="Input">
                    <TextBoxStyle CssClass="TextDate" />
                </BDP:BasicDatePicker>
            </td>
            <td>
                &nbsp;</td>
            <td>
                <asp:Button ID="BtnGetDt" runat="server" class="bitbtn btnsearch" 
                OnClientClick="return confirm('Sure want to clear data changed?');" 
                    Text="Reset" Width="57px" />
            </td>
        </tr>      
        <tr>
            <td><asp:LinkButton ID="lbFormatJE" ValidationGroup="Input"  runat="server" 
                    Text="Account"/></td>
            <td>:</td>
            <td>
                <asp:TextBox ID="tbAccCode" runat="server" AutoPostBack="true" 
                    CssClass="TextBox" />
                          
            </td>
            <td >
                <asp:TextBox ID="tbAccName" runat="server" CssClass="TextBoxR" Enabled="False" 
                    EnableTheming="True" ReadOnly="True" Width="200px" />
                <asp:Button ID="btnAcc" runat="server" class="btngo" Text="..." 
                    ValidationGroup="Input" />
            </td>
            <td >
                &nbsp;</td>
            <td>
                <asp:Button ID="btnSaveTrans" runat="server" class="bitbtndt btnsave" 
                    Text="Apply" ValidationGroup="Input" />
            </td>
        </tr>
      </table>
          <br />
          <br />
          
          <div style="border:0px  solid; width:100%; height:100%; overflow:auto;">
              <asp:GridView ID="GridView1" runat="server" AllowPaging="true" AllowSorting="true"
                  AutoGenerateColumns="false" ShowFooter="True">
                  <HeaderStyle CssClass="GridHeader" />
                  <RowStyle CssClass="GridItem" Wrap="false"/>
                  <AlternatingRowStyle CssClass="GridAltItem" />
                  <PagerStyle CssClass="GridPager" />
                  <Columns>
                      <asp:TemplateField HeaderText="Rek No" SortExpression="RekNo">
                             <ItemTemplate >
                              <asp:TextBox Runat="server" CssClass="TextBox" ID="RekNo" MaxLength="60" Width="95%" Text='<%# DataBinder.Eval(Container.DataItem, "RekNo") %>'>
							  </asp:TextBox>
                              <%--<asp:Label Runat="server" ID="RekNoAdd" text='<%# DataBinder.Eval(Container.DataItem, "RekNo") %>'>
								</asp:Label>--%>
                          </ItemTemplate>                         
                           <EditItemTemplate>
                              <asp:Button ID="btnSave" runat="server" class="bitbtn btnsavedt" 
                                  CommandName="Save" Text="Save" />
                              <asp:Button ID="btnCancel" runat="server" class="bitbtn btncanceldt" 
                                  CommandName="Cancel" Text="Cancel" />
                           </EditItemTemplate>
                      
<HeaderStyle Width="1000px"></HeaderStyle>      
                      </asp:TemplateField>
                       
                     <%-- <asp:BoundField DataField="RekNo" HeaderText="Rek. No." />--%>
                      <asp:BoundField DataField="TransDate" HeaderStyle-Width="80px" SortExpression="TransDate"
                          HeaderText="Date" DataFormatString="{0:dd MMM yyyy}" >
                          <HeaderStyle Width="80px" />
                      </asp:BoundField>
                      <asp:BoundField DataField="TransClass" HeaderStyle-Width="250px" SortExpression="TransClass"
                          HeaderText="Type">
                          <HeaderStyle Width="250px" />
                      </asp:BoundField>
                      <asp:BoundField DataField="Reference" HeaderStyle-Width="250px" SortExpression="Reference"
                          HeaderText="Reference">
                          <HeaderStyle Width="250px" />
                      </asp:BoundField>
                      <asp:BoundField DataField="Remark" HeaderText="Remark" SortExpression="Remark"/>
                      <asp:BoundField DataField="SubLed" HeaderText="SubLed" />
                      <asp:BoundField DataField="SubLedName" HeaderStyle-Width="80px" 
                          HeaderText="SubLed Name">
                          <HeaderStyle Width="80px" />
                      </asp:BoundField>
                      <asp:BoundField DataField="ItemNo" HeaderStyle-Width="80px" 
                          HeaderText="No.">
                          <HeaderStyle Width="80px" />
                      </asp:BoundField>
                      <asp:BoundField DataField="CurrCode" HeaderStyle-Width="80px" 
                          HeaderText="Curr.">
                          <HeaderStyle Width="80px" />
                      </asp:BoundField>
                      <asp:BoundField DataField="ForexRate" HeaderStyle-Width="80px" 
                          HeaderText="Rate" DataFormatString="{0:#,##0.##}" ItemStyle-HorizontalAlign="Right" >
                          <HeaderStyle Width="80px" />
                      </asp:BoundField>
                      <asp:BoundField DataField="DebitForex" HeaderStyle-Width="250px" 
                          HeaderText="Debit" DataFormatString="{0:#,##0.##}" ItemStyle-HorizontalAlign="Right" > 
                          <HeaderStyle Width="250px" />
                      </asp:BoundField>
                       <asp:BoundField DataField="CreditForex" HeaderStyle-Width="250px" 
                          HeaderText="Credit" DataFormatString="{0:#,##0.##}" ItemStyle-HorizontalAlign="Right" >
                          <HeaderStyle Width="250px" />
                      </asp:BoundField>
                       <asp:BoundField DataField="Detail" HeaderStyle-Width="250px" 
                          HeaderText="Detail">
                          <HeaderStyle Width="250px" />
                      </asp:BoundField>
                  </Columns>
              </asp:GridView>
          </div>
            <asp:Panel runat="server" ID ="pnlNav" Visible="false">
            &nbsp &nbsp &nbsp<br />
                <br />
        </asp:Panel>
    </asp:Panel>    
      <br />            
    </div>
   
    <asp:Label runat ="server" ID="lbStatus" ForeColor="Red"/>
   
    </form>
    </body>
</html>
