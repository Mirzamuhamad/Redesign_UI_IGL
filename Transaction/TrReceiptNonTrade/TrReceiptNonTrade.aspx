<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TrReceiptNonTrade.aspx.vb" Inherits="TrReceiptNonTrade" %>

<%@ Register Assembly="BasicFrame.WebControls.BasicDatePicker" Namespace="BasicFrame.WebControls"
    TagPrefix="BDP" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Tanda Terima(Bank/Giro)</title>
    <script src="../../Function/OpenDlg.js" type="text/javascript"></script>
    <script src="../../Function/Function.JS" type="text/javascript"></script>
  <link type="text/css" rel="stylesheet" href="../../Styles/circularprogress.css" />
    <script type="text/javascript" src="../../JQuery/jquery.min.js"></script>
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
          var VReceiptF = document.getElementById("tbTotalReceiptForex").value.replace(/\$|\,/g,"");
          var VReceipt = document.getElementById("tbTotalReceipt").value.replace(/\$|\,/g,"");
          var VOther = document.getElementById("tbTotalOther").value.replace(/\$|\,/g,"");
          var VExpense = document.getElementById("tbTotalExpense").value.replace(/\$|\,/g,"");
          var VCharge = document.getElementById("tbTotalCharge").value.replace(/\$|\,/g,"");
          var Selisih = document.getElementById("tbTotalSelisih").value.replace(/\$|\,/g, "");
            if (isNaN(Selisih) == true) {
                Selisih = 0;
            }  
            
          //Selisih = VReceipt - VOther - VExpense - VCharge  
          
          document.getElementById("tbTotalReceiptForex").value = setdigit(VReceiptF,'<%=ViewState("DigitCurr")%>');
          document.getElementById("tbTotalReceipt").value = setdigit(VReceipt,'<%=ViewState("DigitHome")%>');
          document.getElementById("tbTotalOther").value = setdigit(VOther,'<%=ViewState("DigitHome")%>');
          document.getElementById("tbTotalExpense").value = setdigit(VExpense,'<%=ViewState("DigitHome")%>');
          document.getElementById("tbTotalCharge").value = setdigit(VCharge,'<%=ViewState("DigitHome")%>');
          document.getElementById("tbTotalSelisih").value = setdigit(Selisih, '<%=ViewState("DigitHome")%>');
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
         document.getElementById("tbAmountForexDt").value = setdigit(AmountForex,'<%=VIEWSTATE("DigitCurrAcc")%>');
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
         var ReceiptForex = document.getElementById("tbReceiptForexDt2").value.replace(/\$|\,/g,""); 
         var ReceiptHome = document.getElementById("tbReceiptHomeDt2").value.replace(/\$|\,/g,"");          
                          
         document.getElementById("tbRateDt2").value = setdigit(Rate,'<%=ViewState("DigitRate")%>');
         document.getElementById("tbReceiptForexDt2").value = setdigit(ReceiptForex,'<%=VIEWSTATE("DigitCurr")%>');
         document.getElementById("tbReceiptHomeDt2").value = setdigit(ReceiptHome,'<%=ViewState("DigitHome")%>');         
        }catch (err){
            alert(err.description);
          }
      }

      function ProgressCircle() {
          setTimeout(function() {
              var modal = $('<div />');
              modal.addClass("modal");
              $('body').append(modal);
              var loading = $(".loading");
              loading.show();
              var top = Math.max($(window).height() / 2 - loading[0].offsetHeight / 2, 0);
              var left = Math.max($(window).width() / 2 - loading[0].offsetWidth / 2, 0);
              loading.css({ top: top, left: left });
          }, 200);
      }
      $('form').live("submit", function() {
          ProgressCircle();
      });            
        
    </script>
    
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
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
                    <asp:ListItem Value="ReceiptType">Receipt Type </asp:ListItem>
                    <asp:ListItem Value="ReceiptName">Receipt Type Name</asp:ListItem>
                    <asp:ListItem Value="BankGiro">Bank Giro</asp:ListItem>
                    <asp:ListItem Value="BankGiroName">Bank Giro Name</asp:ListItem>                                    
                    <asp:ListItem Value="DocumentNo">Document No</asp:ListItem>
                    <asp:ListItem Value="Voucher_No">Voucher No</asp:ListItem>
                    <asp:ListItem Value="Account">Account</asp:ListItem>
                    <asp:ListItem Value="Accountname">Account Name</asp:ListItem>
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
                    <asp:ListItem Selected="True" Value="TransNmbr">Receipt No</asp:ListItem>
                    <asp:ListItem Value="dbo.FormatDate(TransDate)">Receipt Date</asp:ListItem>
                    <asp:ListItem>Status</asp:ListItem>
                    <asp:ListItem Value="FgReport">Report</asp:ListItem>
                    <asp:ListItem Value="UserType">User Type</asp:ListItem>
                    <asp:ListItem Value="UserReceipt">User Receipt</asp:ListItem>                    
                    <asp:ListItem>Attn</asp:ListItem> 
                    <asp:ListItem Value="ReceiptType">Receipt Type </asp:ListItem>
                    <asp:ListItem Value="ReceiptName">Receipt Type Name</asp:ListItem>
                    <asp:ListItem Value="BankGiro">Bank Giro</asp:ListItem>
                    <asp:ListItem Value="BankGiroName">Bank Giro Name</asp:ListItem>   
                    <asp:ListItem Value="DocumentNo">Document No</asp:ListItem>
                    <asp:ListItem Value="Voucher_No">Voucher No</asp:ListItem>
                    <asp:ListItem Value="Account">Account</asp:ListItem>
                    <asp:ListItem Value="Accountname">Account Name</asp:ListItem>                                   
              </asp:DropDownList>
          </td>              
        </tr>        
      </table>      
      </asp:Panel>
          <br />
            <asp:Button class="bitbtn btnadd" runat="server" ID="BtnAdd" Text="Add" />	           
           
            &nbsp;  
            <asp:DropDownList CssClass="DropDownList" ID="ddlCommand" runat="server" Visible="false"/>
            <asp:Button class="btngo" runat="server" ID="BtnGo" Text="G" Visible="false" />                      
          <br />&nbsp;
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
                              <asp:ListItem Text="Tanda Terima Giro" />
                              <%--<asp:ListItem Text="Print Full" />--%>
                          </asp:DropDownList>
                          <asp:Button class="btngo" runat="server" ID="BtnGo" Text="G" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" CommandName="Go" />
                      </ItemTemplate>
                      <HeaderStyle Width="110px" />
                  </asp:TemplateField>
                  <asp:BoundField DataField="TransNmbr" HeaderStyle-Width="120px" SortExpression="Nmbr" HeaderText="Receipt No"></asp:BoundField>                  
                  <asp:BoundField DataField="Status" SortExpression="Status" HeaderText="Status"></asp:BoundField>                  
                  <asp:BoundField DataField="TransDate" dataformatstring="{0:dd MMM yyyy}" htmlencode="true" HeaderStyle-Width="80px" SortExpression="TransDate" HeaderText="Receipt Date"></asp:BoundField>
                  <%--<asp:BoundField DataField="FgReport" HeaderStyle-Width="80px" HeaderText="Report"></asp:BoundField>--%>
                  <asp:BoundField DataField="UserType" HeaderStyle-Width="80px" SortExpression="UserType" HeaderText="User Type"></asp:BoundField>
                  <asp:BoundField DataField="UserReceipt" HeaderStyle-Width="80px" SortExpression="UserReceipt" HeaderText="User Receipt"></asp:BoundField>
                  <asp:BoundField DataField="Attn" HeaderStyle-Width="200px" SortExpression="Attn" HeaderText="Attn"></asp:BoundField>
                  <asp:BoundField DataField="Currency" HeaderStyle-Width="80px" SortExpression="Currency" HeaderText="Currency" ></asp:BoundField>
                  <asp:BoundField DataField="TotalReceiptForexStr" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="80px" SortExpression="TotalReceiptForex" HeaderText="Receipt Forex"></asp:BoundField>
                  <asp:BoundField DataField="TotalReceipt" DataFormatString="{0:#,##0.00}" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Right" SortExpression="TotalReceipt" HeaderText="Receipt (IDR)"></asp:BoundField>
                  <asp:BoundField DataField="TotalOthers" DataFormatString="{0:#,##0.00}" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Right" SortExpression="TotalOthers" HeaderText="Others (IDR)"></asp:BoundField>
                  <asp:BoundField DataField="TotalExpense" DataFormatString="{0:#,##0.00}" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Right" SortExpression="TotalExpense" HeaderText="Expense (IDR)"></asp:BoundField>
                  <%--<asp:BoundField DataField="TotalCharge" DataFormatString="{0:#,##0.00}" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Right" SortExpression="TotalCharge" HeaderText="Charge"></asp:BoundField>--%>
                  <asp:BoundField DataField="Remark" HeaderStyle-Width="250px" SortExpression="Remark" HeaderText="Remark"></asp:BoundField>
              </Columns>
          </asp:GridView>
          </div>
          <br />
            <asp:Panel runat="server" ID ="pnlNav" Visible="false">
            <asp:Button class="bitbtn btnadd" runat="server" ID="btnAdd2" Text="Add" />	
            
            &nbsp;
            <asp:DropDownList ID="ddlCommand2" CssClass="DropDownList" runat="server"/>
            <asp:Button class="btngo" runat="server" ID="btnGo2" Text="G" />                      
            
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
                <td>Report</td>
                <td>:</td>
                <td colspan="4"><asp:DropDownList AutoPostBack="true" ValidationGroup="Input" CssClass="DropDownList" ID="ddlReport" runat="server" >
                        <asp:ListItem Selected="True">Y</asp:ListItem>
                        <asp:ListItem>N</asp:ListItem>
                    </asp:DropDownList> 
                </td>    
        </tr>--%>          
        <tr>
            <td>User</td>
            <td>:</td>
            <td colspan="4">
                <asp:DropDownList CssClass="DropDownList" ID="ddlUserType" runat="server" AutoPostBack ="true">
                <asp:ListItem Selected="True">Unallocated</asp:ListItem>
                    <asp:ListItem >Supplier</asp:ListItem>
                    <asp:ListItem>Customer</asp:ListItem>
                    <asp:ListItem>Common</asp:ListItem>
                </asp:DropDownList>
                <asp:TextBox CssClass="TextBox" ValidationGroup="Input" runat="server" ID="tbUserCode" AutoPostBack="true" />
                <asp:TextBox CssClass="TextBox" runat="server" ID="tbUserName" Enabled="false" Width="225px"/>
                <asp:Button Class="btngo" ID="btnUser" Text="..." runat="server" ValidationGroup="Input" />                                  
                           
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
                       <td><asp:Label ID="Label1" runat="server" CssClass="TextBox" Text = "Receipt Forex"/></td> 
                       <td><asp:Label ID = "lbTotPay" runat="server" CssClass="TextBox" Text = "Receipt"/></td>
                       <td><asp:Label ID = "lbTotOther" runat="server" CssClass="TextBox" Text = "Other"/></td>
                       <td><asp:Label ID = "lbTotExpense" runat="server" CssClass="TextBox" Text = "Expense"/></td>
                       <td><asp:Label ID = "lbTotCharge" runat="server" CssClass="TextBox" Text = "Charge"/></td>
                       <td><asp:Label ID="lbTotSelisih" runat="server" CssClass="TextBox" Text = "Difference"/></td>
                     </tr>
                     <tr>
                        <td><asp:TextBox runat="server" ID="tbTotalReceiptForex" CssClass="TextBoxR"/></td>
                        <td><asp:TextBox runat="server" ID="tbTotalReceipt" CssClass="TextBoxR"/></td>
                        <td><asp:TextBox runat="server" ID="tbTotalOther" CssClass="TextBoxR"/></td>
                        <td><asp:TextBox runat="server" ID="tbTotalExpense" CssClass="TextBoxR"/></td>
                        <td><asp:TextBox runat="server" ID="tbTotalCharge" CssClass="TextBoxR"/></td>
                        <td><asp:TextBox runat="server" ValidationGroup="Input" ID="tbTotalSelisih" CssClass="TextBoxR"/></td>
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
                <asp:MenuItem Text="Detail Account" Value="0"></asp:MenuItem>
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
              <asp:Button class="bitbtn btnadd" runat="server" ID="btnAdddt" Text="Add" ValidationGroup="Input" />	
                <div style="border:0px  solid; width:100%; height:100%; overflow:auto;">
                    <asp:GridView ID="GridDt" runat="server" AutoGenerateColumns="false" 
                        ShowFooter="False">
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
                            </asp:TemplateField>
                            <asp:BoundField DataField="ItemNo" HeaderText="No" />
                            <asp:BoundField DataField="Account" HeaderText="Account" />
                            <asp:BoundField DataField="AccountName" HeaderStyle-Width="150px" HeaderText="Account Name" />
                            <asp:BoundField DataField="SubledName" HeaderStyle-Width="150px" HeaderText="Subled" />
                            <asp:BoundField DataField="CostCtr" HeaderStyle-Width="80px" HeaderText="Cost Center" />
                            <asp:BoundField DataField="Currency" HeaderStyle-Width="80px" HeaderText="Currency" />
                            <asp:BoundField DataField="ForexRate" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Right" HeaderText="Forex Rate" />
                            <asp:BoundField DataField="AmountForex" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Right"  HeaderText="Amount Forex" />
                            <asp:BoundField DataField="AmountHome" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Right" HeaderText="Amount Home" />
                            <asp:BoundField DataField="Remark" HeaderStyle-Width="200px" HeaderText="Remark" />
                        </Columns>
                    </asp:GridView>
              </div>   
              <asp:Button class="bitbtn btnadd" runat="server" ID="btnAddDtke2" Text="Add" ValidationGroup="Input" />	
              
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
                        <td>Account</td>
                        <td>:</td>
                        <td colspan="4">                                
                            <asp:TextBox CssClass="TextBox" ValidationGroup="Input" runat="server" ID="tbAccountDt" AutoPostBack="true" />
                            <asp:TextBox CssClass="TextBox" runat="server" ID="tbAccountNameDt" Enabled="false" Width="225px"/>
                            <asp:TextBox CssClass="TextBox" runat="server" ID="tbFgType" Visible="false"/>
                            <asp:Button Class="btngo" ID="btnAccount" Text="..." runat="server" ValidationGroup="Input" />                                          
                            
                        </td>
                    </tr>
                    <tr>
                        <td>Subled</td>
                        <td>:</td>
                        <td colspan="4">                                
                            <asp:TextBox runat="server" ID="tbFgSubledDt" visible="false" />
                            <asp:TextBox runat="server" ID="tbFgCostCtr" Visible="false" />                              
                            <asp:TextBox CssClass="TextBox" ValidationGroup="Input" runat="server" ID="tbSubledDt" AutoPostBack="true" />
                            <asp:TextBox CssClass="TextBox" runat="server" ID="tbSubledNameDt" Enabled="false" Width="225px"/>
                            <asp:Button Class="btngo" ID="btnSubled" Text="..." runat="server" ValidationGroup="Input" />                                          
                           
                        </td>
                    </tr>
                    <tr>
                        <td>Cost Center</td>
                        <td>:</td>
                        <td colspan="4"><asp:DropDownList CssClass="DropDownList" ID="ddlCostCenterDt" runat="server"/></td>
                    </tr>   
                    <tr> 
                        <td>Nominal</td>
                        <td>:</td>
                        <td colspan="2">
                        <table>
                            <tr style="background-color:Silver;text-align:center">
                                <td>Currency</td>
                                <td>Rate</td>
                                <td>Amount Forex</td>
                                <td>Amount Home</td>
                            </tr>
                            <tr>
                                <td><asp:DropDownList CssClass="DropDownList" ID="ddlCurrDt" runat="server" Enabled="false" AutoPostBack="true" Width="60px"/></td>    
                                <td><asp:TextBox CssClass="TextBox" runat="server" ID="tbRateDt" Width="65px"/></td>    
                                <td><asp:TextBox CssClass="TextBox" runat="server" ID="tbAmountForexDt"/></td>    
                                <td><asp:TextBox CssClass="TextBoxR" runat="server" ID="tbAmountHomeDt"/></td>                     
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
            <asp:Button ID="btnSaveDt" runat="server" class="bitbtndt btnsave" Text="Save" />									
            <asp:Button ID="btnCancelDt" runat="server" class="bitbtndt btncancel" Text="Cancel" />					
           </asp:Panel> 
              
           </asp:View>           
            <asp:View ID="Tab2" runat="server">
                <asp:Panel ID="pnlDt2" runat="server">      
                <asp:Button class="bitbtn btnadd" runat="server" ID="btnAddDt2" Text="Add" ValidationGroup="Input" />	          
                
                <div style="border:0px  solid; width:100%; height:100%; overflow:auto;">
                    <asp:GridView ID="GridDt2" runat="server" AutoGenerateColumns="false" 
                        ShowFooter="False">
                        <HeaderStyle CssClass="GridHeader" />
                        <RowStyle CssClass="GridItem" Wrap="false" />
                        <AlternatingRowStyle CssClass="GridAltItem" />
                        <PagerStyle CssClass="GridPager" />
                        <Columns>
                            <asp:TemplateField HeaderText="Action"><ItemTemplate>
                            
                            			<asp:Button ID="btnEdit" runat="server" class="bitbtndt btnedit" Text="Edit" CommandName="Edit"/>
								        <asp:Button ID="btnDelete" runat="server" class="bitbtndt btndelete" Text="Delete" OnClientClick="return confirm('Sure to delete this data?');" CommandName="Delete"/>									
                                        
                                        </ItemTemplate><EditItemTemplate>
                                       
                                        <asp:Button ID="btnUpdate" runat="server" class="bitbtndt btnsave" Text="Save" CommandName="Update"/>									
                                        <asp:Button ID="btnCancel" runat="server" class="bitbtndt btncancel" Text="Cancel" CommandName="Cancel"/>									
 
                                        
                                        
                                        </EditItemTemplate></asp:TemplateField>                                        
                            <asp:BoundField DataField="ItemNo" HeaderText="No" />
                            <asp:BoundField DataField="ReceiptName" HeaderStyle-Width="150px" HeaderText="Receipt" />
                            <asp:BoundField DataField="DocumentNo" HeaderStyle-Width="150px" HeaderText="Document No" />                            
                            <asp:BoundField DataField="Reference" HeaderStyle-Width="120px" HeaderText="Voucher No" >                                                            
                            </asp:BoundField>
                            <asp:BoundField DataField="DueDate" HeaderStyle-Width="80px" HeaderText="Due Date" />
                            <asp:BoundField DataField="BankGiroName" HeaderStyle-Width="150px" HeaderText="Bank Giro" />
                            <asp:BoundField DataField="Currency" HeaderStyle-Width="80px" HeaderText="Currency" />
                            <asp:BoundField DataField="ReceiptForex" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Right"  HeaderText="Receipt Forex" />
                            <asp:BoundField DataField="ReceiptHome" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Right" HeaderText="Receipt Home" />                            
                            <asp:BoundField DataField="Remark" HeaderText="Remark" />                                                        
                        </Columns>
                    </asp:GridView>
              </div>    
              
              <asp:Button class="bitbtn btnadd" runat="server" ID="btnAddDt2Ke2" Text="Add" ValidationGroup="Input" />	          
              
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
                            
                        <td></td>
                        <td></td>
                        <td>
                             <BDP:BasicDatePicker ID="tbReceiptDateDt2" runat="server" DateFormat="dd MMM yyyy" 
                             ReadOnly = "true" ValidationGroup="Input" ButtonImageHeight="19px" ButtonImageWidth="20px" 
                             DisplayType="TextBoxAndImage" TextBoxStyle-CssClass="TextDate" Visible="false"
                             ShowNoneButton="False"> 
                             <TextBoxStyle CssClass="TextDate" />
                             </BDP:BasicDatePicker>
                        </td>
                    </tr>
                    <tr>
                        <td>Document No</td>
                        <td>:</td>
                        <td><asp:TextBox CssClass="TextBox" runat="server" ID="tbDocumentNoDt2" Width="157px"/> 
                        <asp:Button Class="btngo" ID="btnDocNo" Text="..." runat="server" />                                  
                        </td>
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
                        <td>Nominal</td>
                        <td>:</td>
                        <td colspan="4">
                            <table>
                                <tr style="background-color:Silver;text-align:center">
                                    <td>Receipt</td>
                                    <td>Rate</td>  
                                    <td><asp:Label ID="lbPayForex" CssClass="TextBox" runat="server" Text = "Receipt Forex" /></asp:Label></td>
                                    <td><asp:Label ID="lbPayHome" CssClass="TextBox" runat="server" Text = "Receipt Home" /></td>                                                                              
                                </tr>
                                <tr>
                                    <td><asp:DropDownList CssClass="DropDownList" ID="ddlCurrDt2" runat="server" Enabled="false" Width="60px"/></td>
                                    <td><asp:TextBox CssClass="TextBox" runat="server" ID="tbRateDt2" Width="65px"/></td> 
                                    <td><asp:TextBox CssClass="TextBox" runat="server" ID="tbReceiptForexDt2" Width="100px"/></td>
                                    <td><asp:TextBox CssClass="TextBoxR" runat="server" ID="tbReceiptHomeDt2" Width="100px"/></td>                                   
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
               
               <asp:Button ID="btnSaveDt2" runat="server" class="bitbtndt btnsave" Text="Save" />									
               <asp:Button ID="btnCancelDt2" runat="server" class="bitbtndt btncancel" Text="Cancel" />									
                   
                
           </asp:Panel> 
               
            </asp:View>            
        </asp:MultiView>
    
       <br />  
      
        <asp:Button ID="btnSaveAll" runat="server" class="bitbtndt btnsavenew" Text="Save & New" validationgroup="Input" Width = "90"/>									
        <asp:Button ID="btnSaveTrans" runat="server" class="bitbtndt btnsave" Text="Save" validationgroup="Input"/>									
        <asp:Button ID="btnBack" runat="server" class="bitbtndt btncancel" Text="Cancel" validationgroup="Input"/>									
        <asp:Button ID="btnHome" runat="server" class="bitbtndt btnback" Text="Home" />									                                                           
         
     
    </asp:Panel>        
    </div>
   
    <asp:Label runat ="server" ID="lbStatus" ForeColor="Red"/>
    
    <div class="loading" align="center">
      <br />
       <img src="../../Image/loader.gif" alt="" />
    </div>
    </form>
</body>
</html>
