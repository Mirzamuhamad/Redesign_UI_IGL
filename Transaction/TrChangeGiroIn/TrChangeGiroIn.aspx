<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TrChangeGiroIn.aspx.vb" Inherits="TrChangeGiroIn" %>

<%@ Register Assembly="BasicFrame.WebControls.BasicDatePicker" Namespace="BasicFrame.WebControls"
    TagPrefix="BDP" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

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
       
       function setformat()
        {
        try
         {        
          var VReceipt = document.getElementById("tbTotalReceipt").value.replace(/\$|\,/g,"");
          var VGiro = document.getElementById("tbTotalGiro").value.replace(/\$|\,/g,"");
          var VCharge = document.getElementById("tbTotalCharge").value.replace(/\$|\,/g,"");
          
          document.getElementById("tbTotalReceipt").value = setdigit(VReceipt,'<%=ViewState("DigitHome")%>');
          document.getElementById("tbTotalGiro").value = setdigit(VGiro,'<%=ViewState("DigitHome")%>');
          document.getElementById("tbTotalCharge").value = setdigit(VCharge,'<%=ViewState("DigitHome")%>');
        }catch (err){
            alert(err.description);
          }      
        }   
        
       function setformatdt()
        {
        try
         {         
         var AmountForex = document.getElementById("tbAmountForexDt").value.replace(/\$|\,/g,""); 
         var AmountHome = document.getElementById("tbAmountHomeDt").value.replace(/\$|\,/g,""); 
         var Rate = document.getElementById("tbRateDt").value.replace(/\$|\,/g,""); 
                          
         document.getElementById("tbRateDt").value = setdigit(Rate,'<%=ViewState("DigitRate")%>');
         document.getElementById("tbAmountForexDt").value = setdigit(AmountForex,'<%=VIEWSTATE("DigitCurr")%>');
         document.getElementById("tbAmountHomeDt").value = setdigit(AmountHome,'<%=ViewState("DigitHome")%>');
         
        }catch (err){
            alert(err.description);
          }      
        }   

        function setformatdt2()
        {
        try
         {
         var Rate = document.getElementById("tbRateDt2").value.replace(/\$|\,/g,""); 
         var ChargeRate = document.getElementById("tbChargeRateDt2").value.replace(/\$|\,/g,""); 
         var ReceiptForex = document.getElementById("tbReceiptForexDt2").value.replace(/\$|\,/g,""); 
         var ReceiptHome = document.getElementById("tbReceiptHomeDt2").value.replace(/\$|\,/g,""); 
         var ChargeForex = document.getElementById("tbChargeForexDt2").value.replace(/\$|\,/g,""); 
         var ChargeHome = document.getElementById("tbChargeHomeDt2").value.replace(/\$|\,/g,""); 
                          
         document.getElementById("tbRateDt2").value = setdigit(Rate,'<%=ViewState("DigitRate")%>');
         document.getElementById("tbChargeRateDt2").value = setdigit(ChargeRate,'<%=ViewState("DigitRate")%>');
         document.getElementById("tbReceiptForexDt2").value = setdigit(ReceiptForex,'<%=VIEWSTATE("DigitCurrDt")%>');
         document.getElementById("tbReceiptHomeDt2").value = setdigit(ReceiptHome,'<%=ViewState("DigitHome")%>');
         document.getElementById("tbChargeForexDt2").value = setdigit(ChargeForex,'<%=VIEWSTATE("DigitExpenseCurr")%>');
         document.getElementById("tbChargeHomeDt2").value = setdigit(ChargeHome,'<%=ViewState("DigitHome")%>');
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
    <div class="H1">Change Giro Receipt</div>
     <hr style="color:Blue" />        
    <asp:Panel runat="server" id="PnlHd"> 
      <table>
        <tr>
            <td style="width:100px;text-align:right">Quick Search :</td>         
            <%--TransNmbr, TransDate, STATUS, FgReport, UserType, UserCode, UserName, Attn, Remark--%>   
            <td><asp:TextBox CssClass="TextBox" runat="server" ID ="tbFilter"/> 
                  <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlField" >                      
                    <asp:ListItem Selected="True" Value="TransNmbr">Receipt No</asp:ListItem>
                    <asp:ListItem Value="dbo.FormatDate(TransDate)">Receipt Date</asp:ListItem>
                    <asp:ListItem>Status</asp:ListItem>
                    <asp:ListItem Value="FgReport">Report</asp:ListItem>
                    <asp:ListItem Value="UserType">User Type</asp:ListItem>
                    <asp:ListItem Value="UserReceipt">User Receipt</asp:ListItem>                    
                    <asp:ListItem>Attn</asp:ListItem>                                       
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
                    <asp:ListItem Selected="True" Value="TransNmbr">Receipt No</asp:ListItem>
                    <asp:ListItem Value="dbo.FormatDate(TransDate)">Receipt Date</asp:ListItem>
                    <asp:ListItem>Status</asp:ListItem>
                    <asp:ListItem Value="FgReport">Report</asp:ListItem>
                    <asp:ListItem Value="UserType">User Type</asp:ListItem>
                    <asp:ListItem Value="UserReceipt">User Receipt</asp:ListItem>                    
                    <asp:ListItem>Attn</asp:ListItem>                                       
              </asp:DropDownList>
          </td>              
        </tr>        
      </table>      
      </asp:Panel>
          <br />
            <asp:Button class="bitbtn btnadd" runat="server" ID="BtnAdd" Text="Add"/> 
            &nbsp &nbsp &nbsp   
            <asp:DropDownList CssClass="DropDownList" ID="ddlCommand" runat="server" Visible="false"/>
            <asp:Button class="btngo" runat="server" ID="BtnGo" Text="G" Visible="false"/> 
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
                              <asp:ListItem Text="Print" />
                          </asp:DropDownList>
                          <asp:Button class="btngo" runat="server" ID="BtnGo" Text="G" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" CommandName="Go"/> 
                      </ItemTemplate>
                      <HeaderStyle Width="110px" />
                  </asp:TemplateField>
                  <asp:BoundField DataField="TransNmbr" HeaderStyle-Width="120px" SortExpression="Nmbr" HeaderText="Receipt No"></asp:BoundField>                  
                  <asp:BoundField DataField="Status" HeaderText="Status"></asp:BoundField>                  
                  <asp:BoundField DataField="TransDate" HtmlEncode="true" DataFormatString="{0:dd MMM yyyy}" HeaderStyle-Width="80px" SortExpression="TransDate" HeaderText="Receipt Date"></asp:BoundField>
                  <asp:BoundField DataField="FgReport" HeaderStyle-Width="80px" HeaderText="Report"></asp:BoundField>
                  <asp:BoundField DataField="UserType" HeaderStyle-Width="80px" SortExpression="UserType" HeaderText="User Type"></asp:BoundField>
                  <asp:BoundField DataField="UserReceipt" HeaderStyle-Width="80px" SortExpression="UserReceipt" HeaderText="User Receipt"></asp:BoundField>
                  <asp:BoundField DataField="Attn" HeaderStyle-Width="200px" SortExpression="Attn" HeaderText="Attn"></asp:BoundField>
                  <asp:BoundField DataField="TotalReceipt" DataFormatString="{0:#,##0.00}" HeaderStyle-Width="80px" SortExpression="TotalReceipt" HeaderText="Receipt"></asp:BoundField>
                  <asp:BoundField DataField="TotalGiro" DataFormatString="{0:#,##0.00}" HeaderStyle-Width="80px" SortExpression="TotalGiro" HeaderText="Giro"></asp:BoundField>
                  <asp:BoundField DataField="TotalCharge" DataFormatString="{0:#,##0.00}" HeaderStyle-Width="80px" SortExpression="TotalCharge" HeaderText="Charge"></asp:BoundField>
                  <asp:BoundField DataField="Remark" HeaderStyle-Width="250px" HeaderText="Remark"></asp:BoundField>
              </Columns>
          </asp:GridView>
          </div>
            <asp:Panel runat="server" ID ="pnlNav" Visible="false">
            <asp:Button class="bitbtn btnadd" runat="server" ID="btnAdd2" Text="Add"/> 
            &nbsp &nbsp &nbsp
            <asp:DropDownList ID="ddlCommand2" CssClass="DropDownList" runat="server"/>
            <asp:Button class="btngo" runat="server" ID="btnGo2" Text="G"/> 
            </asp:Panel>
    </asp:Panel>    
    <asp:Panel runat="server" ID="pnlInput" Visible="false">
      <table>
        <tr>
            <td>Receipt No</td>
            <td>:</td>
            <td><asp:TextBox CssClass="TextBoxR" runat="server" ID="tbCode" Width="150px" Enabled="False"/></td>
        
            <td>Receipt Date</td>
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
          <%--<tr>
              <td>
                  Report
              </td>
              <td>
                  :
              </td>
              <td colspan="4">
                  <asp:DropDownList AutoPostBack="true" ValidationGroup="Input" CssClass="DropDownList"
                      ID="ddlReport" runat="server">
                      <asp:ListItem Selected="True">Y</asp:ListItem>
                      <asp:ListItem>N</asp:ListItem>
                  </asp:DropDownList>
              </td>
          </tr>--%>          
        <tr>
            <td>User</td>
            <td>:</td>
            <td colspan="4">
                <asp:DropDownList CssClass="DropDownList" ID="ddlUserType" runat="server" 
                    AutoPostBack ="true" ValidationGroup="Input">
                    <asp:ListItem Selected="True">Supplier</asp:ListItem>
                    <asp:ListItem>Customer</asp:ListItem>
                    <asp:ListItem>Common</asp:ListItem>
                </asp:DropDownList>
                <asp:TextBox CssClass="TextBox" ValidationGroup="Input" runat="server" ID="tbUserCode" AutoPostBack="true" />
                <asp:TextBox CssClass="TextBox" runat="server" ID="tbUserName" Enabled="false" Width="225px"/>
                <asp:Button class="btngo" runat="server" ID="btnUser" Text="..." ValidationGroup="Input"/> 
            </td>
        </tr>
        <tr>
            <td>Attn</td>
            <td>:</td>
            <td colspan="4"><asp:TextBox runat="server" ValidationGroup="Input" ID="tbAttn" MaxLength="255" CssClass="TextBox" Width="225px"/></td>
        </tr>
        <tr>
            <td>Total</td>
            <td>:</td>
            <td colspan="4">
                <table>
                    <tr style="background-color:Silver;text-align:center">
                       <td><asp:Label ID = "lbTotPay" runat="server" CssClass="TextBox" Text = "Receipt"/></td>
                       <td><asp:Label ID = "lbTotExpense" runat="server" CssClass="TextBox" Text = "Expense"/></td>
                       <td><asp:Label ID = "lbTotCharge" runat="server" CssClass="TextBox" Text = "Charge"/></td>
                     </tr>
                     <tr>
                        <td><asp:TextBox runat="server" ID="tbTotalReceipt" CssClass="TextBoxR"/></td>
                        <td><asp:TextBox runat="server" ID="tbTotalGiro" CssClass="TextBoxR"/></td>
                        <td><asp:TextBox runat="server" ID="tbTotalCharge" CssClass="TextBoxR"/></td>
                     </tr>
                 </table>
            </td>                         
        </tr> 
        <tr>
            <td>Remark</td>
            <td>:</td>
            <td colspan="4"><asp:TextBox runat="server" ValidationGroup="Input" ID="tbRemark" CssClass="TextBox" Width="225px"/></td>
        </tr>
      </table>  
      
      <br />            
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
                <asp:MenuItem Text="Detail Giro" Value="0"></asp:MenuItem>
                <asp:MenuItem Text="Detail Receipt" Value="1"></asp:MenuItem>
            </Items>            
        </asp:Menu>
        <br />
        <asp:MultiView 
        ID="MultiView1"
        runat="server"
        ActiveViewIndex="0">
           <asp:View ID="Tab1" runat="server">
              <asp:Panel runat="server" ID="PnlDt">
                <asp:Button class="bitbtndt btnadd" runat="server" ID="btnAdddt" Text="Add" 
                      ValidationGroup="Input"/> 
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
                                         <asp:Button class="bitbtndt btnedit" runat="server" ID="btnEdit" Text="Edit" CommandName="Edit" /> 
                                         <asp:Button class="bitbtndt btndelete" runat="server" ID="btnDelete" Text="Delete" CommandName="Delete" OnClientClick="return confirm('Sure to delete this data?');" /> 
                                      </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="OldGiro" HeaderStyle-Width="150px" HeaderText="Old Giro No" />
                            <asp:BoundField DataField="ReceiptDate" HeaderStyle-Width="80px" HeaderText="Receipt Date" />
                            <asp:BoundField DataField="DueDate" HeaderStyle-Width="80px" HeaderText="Due Date" />
                            <asp:BoundField DataField="BankGiroName" HeaderStyle-Width="150px" HeaderText="Bank Giro" />
                            <asp:BoundField DataField="Currency" HeaderStyle-Width="80px" HeaderText="Currency" />
                            <asp:BoundField DataField="ForexRate" HeaderStyle-Width="80px" HeaderText="Forex Rate" />
                            <asp:BoundField DataField="AmountForex" HeaderStyle-Width="80px" HeaderText="Amount Forex" />
                            <asp:BoundField DataField="AmountHome" HeaderStyle-Width="80px" HeaderText="Amount Home" />
                            <asp:BoundField DataField="Remark" HeaderStyle-Width="200px" HeaderText="Remark" />
                        </Columns>
                    </asp:GridView>
              </div>   
              <asp:Button class="bitbtndt btnadd" runat="server" ID="btnAddDtke2" Text="Add" ValidationGroup="Input"/> 
              </asp:Panel> 
              <asp:Panel runat="server" ID="pnlEditDt" Visible="false">
                <table>              
                    <tr>                    
                        <td>Giro No</td>
                        <td>:</td>
                        <td colspan="4">                                
                            <asp:TextBox CssClass="TextBox" runat="server" ID="tbGiroNoDt" Enabled="false" Width="155px"/>
                            <asp:Button class="btngo" runat="server" ID="btnGiroNo" Text="..." ValidationGroup="Input"/>                                           
                        </td>
                    </tr>
                    <tr>
                        <td>Receipt Date </td>
                        <td>:</td>
                        <td colspan="4"><BDP:BasicDatePicker ID="tbReceiptDateDt" runat="server" DateFormat="dd MMM yyyy" 
                             ReadOnly = "true" ValidationGroup="Input" ButtonImageHeight="19px" ButtonImageWidth="20px" 
                             DisplayType="TextBox" TextBoxStyle-CssClass="TextDate"
                             ShowNoneButton="False"> 
                             <TextBoxStyle CssClass="TextDate" />
                             </BDP:BasicDatePicker>
                         </td>
                        
                        <td>Due Date </td>
                        <td>:</td>
                        <td colspan="4"><BDP:BasicDatePicker ID="tbDueDateDt" runat="server" DateFormat="dd MMM yyyy" 
                             ReadOnly = "true" ValidationGroup="Input" ButtonImageHeight="19px" ButtonImageWidth="20px" 
                             DisplayType="TextBox" TextBoxStyle-CssClass="TextDate"
                             ShowNoneButton="False"> 
                             <TextBoxStyle CssClass="TextDate" />
                             </BDP:BasicDatePicker>
                          </td>
                    </tr>   
                    <tr>
                        <td>Bank Giro</td>
                        <td>:</td>
                        <td colspan="4">                                
                            <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlBankGiroDt" Enabled="false" Width="200px" />                            
                        </td>
                    </tr>                    
                    <tr> 
                        <td>Currency</td>
                        <td>:</td>
                        <td>
                            <asp:DropDownList CssClass="DropDownList" ID="ddlCurrDt" runat="server" Enabled="false" Width="60px"/>
                            <asp:TextBox CssClass="TextBox" runat="server" ID="tbRateDt" Enabled="false" Width="65px"/>                                    
                        </td>                       
                    </tr>   
                    <tr> 
                        <td>Nominal</td>
                        <td>:</td>
                        <td>                            
                            <asp:TextBox CssClass="TextBox" runat="server" ID="tbAmountForexDt" Enabled = "false"/>                                    
                            <asp:TextBox CssClass="TextBox" runat="server" ID="tbAmountHomeDt" Enabled = "false"/>                                    
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
                <asp:Button class="bitbtndt btnsave" runat="server" ID="btnSaveDt" Text="Save"/>
                <asp:Button class="bitbtndt btncancel" runat="server" ID="btnCancelDt" Text="Cancel"/>                
           </asp:Panel> 
              
           </asp:View>           
            <asp:View ID="Tab2" runat="server">
                <asp:Panel ID="pnlDt2" runat="server">
                <asp:Button class="bitbtndt btnadd" runat="server" ID="btnAddDt2" Text="Add" ValidationGroup="Input"/>
                <div style="border:0px  solid; width:100%; height:100%; overflow:auto;">
                    <asp:GridView ID="GridDt2" runat="server" AutoGenerateColumns="false" 
                        ShowFooter="True">
                        <HeaderStyle CssClass="GridHeader" />
                        <RowStyle CssClass="GridItem" Wrap="false" />
                        <AlternatingRowStyle CssClass="GridAltItem" />
                        <PagerStyle CssClass="GridPager" />
                        <Columns>
                            <asp:TemplateField HeaderText="Action">
                            <ItemTemplate>
                                <asp:Button class="bitbtndt btnedit" runat="server" ID="btnEdit" Text="Edit" CommandName="Edit"/>
                                <asp:Button class="bitbtndt btndelete" runat="server" ID="btnDelete" Text="Delete" CommandName="Delete" OnClientClick="return confirm('Sure to delete this data?');"/>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:Button class="bitbtndt btnsave" runat="server" ID="btnUpdate" Text="Save" CommandName="Update"/>
                                <asp:Button class="bitbtndt btncancel" runat="server" ID="btnCancel" Text="Cancel" CommandName="Cancel"/>
                            </EditItemTemplate>
                            </asp:TemplateField>                                        
                            <asp:BoundField DataField="ItemNo" HeaderText="No" />
                            <asp:BoundField DataField="ReceiptName" HeaderStyle-Width="150px" HeaderText="Receipt" />
                            <asp:BoundField DataField="DocumentNo" HeaderStyle-Width="150px" HeaderText="Document No" />                            
                            <asp:BoundField DataField="Reference" HeaderStyle-Width="120px" HeaderText="Voucher No" />                            
                            <asp:BoundField DataField="DueDate" HeaderStyle-Width="80px" HeaderText="Due Date" />
                            <asp:BoundField DataField="BankGiroName" HeaderStyle-Width="150px" HeaderText="Bank Giro" />
                            <asp:BoundField DataField="Currency" HeaderStyle-Width="80px" HeaderText="Currency" />
                            <asp:BoundField DataField="ReceiptForex" HeaderStyle-Width="80px" HeaderText="Receipt Forex" />
                            <asp:BoundField DataField="ReceiptHome" HeaderStyle-Width="80px" HeaderText="Receipt Home" />
                            <asp:BoundField DataField="ChargeHome" HeaderStyle-Width="80px" HeaderText="Charge Home" />                            
                            <asp:BoundField DataField="Remark" HeaderText="Remark" />                                                        
                        </Columns>
                    </asp:GridView>
              </div>  
              <asp:Button class="bitbtndt btnadd" runat="server" ID="btnAddDt2Ke2" Text="Add" ValidationGroup="Input"/>
              </asp:Panel>
              <asp:Panel runat="server" ID="pnlEditDt2" Visible="false">
                <table>              
                    <tr>
                        <td>Item No</td>
                        <td>:</td>
                        <td colspan="4"><asp:Label ID="lbItemNoDt2" runat="server" Text="itemmm" />
                        </td>           
                    </tr>        
                    <tr>
                        <td>Receipt Type</td>
                        <td>:</td>
                        <td><asp:DropDownList CssClass="DropDownList" ID="ddlPayTypeDt2" runat="server" Width="190px" AutoPostBack ="true"/>
                            <asp:TextBox CssClass ="TextBox" ID = "tbFgModeDt2" runat ="server" Visible="false" />
                        </td>
                            
                        <td>Receipt Date</td>
                        <td>:</td>
                        <td>
                             <BDP:BasicDatePicker ID="tbReceiptDateDt2" runat="server" DateFormat="dd MMM yyyy" 
                             ReadOnly = "true" ValidationGroup="Input" ButtonImageHeight="19px" ButtonImageWidth="20px" 
                             DisplayType="TextBoxAndImage" TextBoxStyle-CssClass="TextDate"
                             ShowNoneButton="False"> 
                             <TextBoxStyle CssClass="TextDate" />
                             </BDP:BasicDatePicker>
                        </td>
                    </tr>
                    <tr>
                        <td>Document No</td>
                        <td>:</td>
                        <td><asp:TextBox CssClass="TextBox" runat="server" ID="tbDocumentNoDt2" Width="157px"/> </td>
                    <td>Voucher No</td>
                        <td>:</td>
                        <td><asp:TextBox CssClass="TextBox" runat="server" ID="tbVoucherNo" Width="157px"/>                            
                        </td>
                    </tr>
                    <tr>
                        <td>Bank Giro</td>
                        <td>:</td>
                        <td><asp:DropDownList CssClass="DropDownList" ID="ddlBankGiroDt2" runat="server"/></td>
                    
                        <td>Due Date</td>
                        <td>:</td>
                        <td>
                             <BDP:BasicDatePicker ID="tbDueDateDt2" runat="server" DateFormat="dd MMM yyyy" 
                             ReadOnly = "true" ButtonImageHeight="19px" ButtonImageWidth="20px" 
                             DisplayType="TextBoxAndImage" TextBoxStyle-CssClass="TextDate"
                             ShowNoneButton="False"> 
                             <TextBoxStyle CssClass="TextDate" />
                             </BDP:BasicDatePicker>
                        </td>    
                        
                    </tr>   
                    <tr> 
                        <td>Currency</td>
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
                                    <td><asp:DropDownList CssClass="DropDownList" ID="ddlCurrDt2" runat="server" Enabled="false" Width="60px"/></td>
                                    <td><asp:TextBox CssClass="TextBox" runat="server" ID="tbRateDt2" Width="65px"/></td>
                                    <td><asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlChargeCurrDt2" 
                                            Width="60px" AutoPostBack="True"/></td>
                                    <td><asp:TextBox CssClass="TextBox" runat="server" ID="tbChargeRateDt2" Width="65px"/></td>                                    
                                </tr>
                            </table>
                        </td>
                        
                        
                    </tr>   
                    <tr>
                        <td>Nominal</td>
                        <td>:</td>
                        <td colspan="3">
                            <table>
                                <tr style="background-color:Silver;text-align:center">
                                    <td><asp:Label ID="lbPayForex" CssClass="TextBox" runat="server" Text = "Receipt Forex" /></asp:Label></td>
                                    <td><asp:Label ID="lbPayHome" CssClass="TextBox" runat="server" Text = "Receipt Home" /></td>                                    
                                    <td><asp:Label ID="lbChargeForex" CssClass="TextBox" runat="server" Text = "Charge Forex" /></td>
                                    <td><asp:Label ID="lbChargeHome" CssClass="TextBox" runat="server" Text = "Charge Home" /></td>                                
                                </tr>
                                <tr>
                                    <td><asp:TextBox CssClass="TextBox" runat="server" ID="tbReceiptForexDt2" Width="80px"/></td>
                                    <td><asp:TextBox CssClass="TextBoxR" runat="server" ID="tbReceiptHomeDt2" Width="80px"/></td>                                                                        
                                    <td><asp:TextBox CssClass="TextBox" runat="server" ID="tbChargeForexDt2" Width="80px"/></td>
                                    <td><asp:TextBox CssClass="TextBoxR" runat="server" ID="tbChargeHomeDt2" Width="80px"/></td>
                                </tr>
                            </table>
                        </td>
                    </tr>                                   
                    <tr>
                        <td>Remark </td>
                        <td>:</td>
                        <td colspan="4"><asp:TextBox runat="server" ID="tbRemarkDt2" CssClass="TextBox" Width="365px" 
                                MaxLength="255" TextMode="MultiLine" />                        
                        </td>
                    </tr>
                </table>
                <br />   
                <asp:Button class="bitbtndt btnsave" runat="server" ID="btnSaveDt2" Text="Save"/>
                <asp:Button class="bitbtndt btncancel" runat="server" ID="btnCancelDt2" Text="Cancel"/>                
           </asp:Panel> 
               
            </asp:View>            
        </asp:MultiView>
    
       <br />  
       <asp:Button class="bitbtn btnsavenew" runat="server" ID="btnSaveAll" 
            Text="Save & New" ValidationGroup="Input" Width="92px"/>
       <asp:Button class="bitbtn btnsave" runat="server" ID="btnSaveTrans" Text="Save" ValidationGroup="Input"/>
       <asp:Button class="bitbtn btncancel" runat="server" ID="btnBack" Text="Cancel" ValidationGroup="Input"/>
       <asp:Button class="btngo" runat="server" ID="btnHome" Text="Home" Width="38px"/>
    </asp:Panel>        
    </div>
   
    <asp:Label runat ="server" ID="lbStatus" ForeColor="Red"/>
    </form>
</body>
</html>
