<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TrFARevaluation.aspx.vb" Inherits="Transaction_TrFARevaluation_TrFARevaluation" %>

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
         var AmountBalNDA = parseFloat(document.getElementById("tbAmountBalNDA").value.replace(/\$|\,/g,""));          
         var AmountBalDA = parseFloat(document.getElementById("tbAmountBalDA").value.replace(/\$|\,/g,"")); 
         var AmountBal = parseFloat(document.getElementById("tbAmountBal").value.replace(/\$|\,/g,"")); 
         var AmountNewNDA = parseFloat(document.getElementById("tbAmountNewNDA").value.replace(/\$|\,/g,""));
         var AmountNewDA = parseFloat(document.getElementById("tbAmountNewDA").value.replace(/\$|\,/g,""));
         var AmountNew = parseFloat(document.getElementById("tbAmountNew").value.replace(/\$|\,/g,"")); 
         var AmountRev = parseFloat(document.getElementById("tbAmountRev").value.replace(/\$|\,/g,"")); 
                  
         var LifeBal = parseFloat(document.getElementById("tbLifeBal").value.replace(/\$|\,/g,"")); 
         var LifeNew = parseFloat(document.getElementById("tbLifeNew").value.replace(/\$|\,/g,"")); 
         var LifeRev = parseFloat(document.getElementById("tbLifeRev").value.replace(/\$|\,/g,""));          
         
         if(isNaN(LifeNew) == true)
         {
           LifeNew = 0;
         }
         if(isNaN(LifeRev) == true)
         {
           LifeRev = 0;
         }   
         if(isNaN(LifeBal) == true)
         {
           LifeBal = 0;
         }                  
         
         LifeRev = LifeNew - LifeBal 
         
         if(isNaN(AmountBalNDA) == true)
         {
           AmountBalNDA = 0;
         }
         if(isNaN(AmountBalDA) == true)
         {
           AmountBalDA = 0;
         }
         if(isNaN(AmountBal) == true)
         {
           AmountBal = 0;
         }     
         
         AmountBal = AmountBalNDA + AmountBalDA 
           
         if(isNaN(AmountNewNDA) == true)
         {
           AmountNewNDA = 0;
         } 
         if(isNaN(AmountNewDA) == true)
         {
           AmountNewDA = 0;
         }                
         if(isNaN(AmountNew) == true)
         {
           AmountNew = 0;
         }   
         
         AmountNew = AmountNewNDA + AmountNewDA 
         
         if(isNaN(AmountRev) == true)
         {
           AmountRev = 0;
         }    
         
         AmountRev = AmountNew - AmountBal 
                              
         document.getElementById("tbAmountBalNDA").value = setdigit(AmountBalNDA,'<%=ViewState("DigitHome")%>');
         document.getElementById("tbAmountBalDA").value = setdigit(AmountBalDA,'<%=ViewState("DigitHome")%>');
         document.getElementById("tbAmountBal").value = setdigit(AmountBal,'<%=ViewState("DigitHome")%>');
         document.getElementById("tbAmountNewNDA").value = setdigit(AmountNewNDA,'<%=ViewState("DigitHome")%>');
         document.getElementById("tbAmountNewDA").value = setdigit(AmountNewDA,'<%=ViewState("DigitHome")%>');
         document.getElementById("tbAmountNew").value = setdigit(AmountNew,'<%=ViewState("DigitHome")%>');
         document.getElementById("tbAmountRev").value = setdigit(AmountRev,'<%=ViewState("DigitHome")%>');
         
         document.getElementById("tbLifeBal").value = setdigit(LifeBal,'<%=ViewState("DigitQty")%>');
         document.getElementById("tbLifeNew").value = setdigit(LifeNew,'<%=ViewState("DigitQty")%>');
         document.getElementById("tbLifeRev").value = setdigit(LifeRev,'<%=ViewState("DigitQty")%>');
         
         
        }catch (err){
            alert(err.description);
          }      
        }   

        
        
    </script>
    
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .style1
        {
            width: 91px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div class="Content">
    <div class="H1">FA Revaluation</div>
     <hr style="color:Blue" />        
    <asp:Panel runat="server" id="PnlHd"> 
      <table>
        <tr>
            <td style="width:100px;text-align:right">Quick Search :</td>         
            <%--TransNmbr, TransDate, STATUS, FgReport, UserType, UserCode, UserName, Attn, Remark--%>   
            <td><asp:TextBox CssClass="TextBox" runat="server" ID ="tbFilter"/> 
                  <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlField" >                      
                    <asp:ListItem Selected="True" Value="TransNmbr">Reference</asp:ListItem>
                    <asp:ListItem Value="dbo.FormatDate(TransDate)">Date</asp:ListItem>
                    <asp:ListItem>Status</asp:ListItem>
                    <asp:ListItem Value="Operator">Operator</asp:ListItem> 
                    <asp:ListItem Value="Remark">Remark</asp:ListItem>                   
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
                    <asp:ListItem Selected="True" Value="TransNmbr">Reference</asp:ListItem>
                    <asp:ListItem Value="dbo.FormatDate(TransDate)">Date</asp:ListItem>
                    <asp:ListItem>Status</asp:ListItem>
                    <asp:ListItem Value="Operator">Operator</asp:ListItem>
                    <asp:ListItem Value="Remark">Remark</asp:ListItem>
              </asp:DropDownList>
          </td>              
        </tr>        
      </table>      
      </asp:Panel>
          <br />
            <asp:Button class="bitbtn btnadd" runat="server" ID="BtnAdd" Text="Add"/>    
            &nbsp &nbsp &nbsp   
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
                              <asp:ListItem Text="Edit" />
                              <asp:ListItem Text="Print" />
                          </asp:DropDownList>
                          <asp:Button class="btngo" runat="server" ID="BtnGo" Text="G" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" CommandName="Go"/>    
                      </ItemTemplate>
                      <HeaderStyle Width="110px" />
                  </asp:TemplateField>
                  <asp:BoundField DataField="TransNmbr" HeaderStyle-Width="120px" SortExpression="Nmbr" HeaderText="Trans. No"></asp:BoundField>                  
                  <asp:BoundField DataField="Status" HeaderText="Status"></asp:BoundField>                  
                  <asp:BoundField DataField="TransDate" dataformatstring="{0:dd MMM yyyy}" htmlencode="true" HeaderStyle-Width="80px" SortExpression="TransDate" HeaderText="Date"></asp:BoundField>
                  <asp:BoundField DataField="Operator" HeaderStyle-Width="80px" SortExpression="Operator" HeaderText="Operator"></asp:BoundField>
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
            <td>Transfer No</td>
            <td>:</td>
            <td><asp:TextBox CssClass="TextBoxR" runat="server" ID="tbCode" Width="150px" Enabled="False"/></td>                            
        </tr>                     
        <tr>
            <td>Transfer Date</td>
            <td>:</td>
            <td>
            <BDP:BasicDatePicker ID="tbDate" runat="server" DateFormat="dd MMM yyyy" 
                        ReadOnly = "true" ValidationGroup="Input"
                        ButtonImageHeight="19px" ButtonImageWidth="20px" 
                        DisplayType="TextBoxAndImage" 
                        TextBoxStyle-CssClass="TextDate" 
                        ShowNoneButton="False"><TextBoxStyle CssClass="TextDate" /></BDP:BasicDatePicker>                
            </td>  
            <td>Fg Report</td>
            <td>:</td>
            <td><asp:DropDownList AutoPostBack="true" ValidationGroup="Input" CssClass="DropDownList" ID="ddlFgReport" runat="server" >
            <asp:ListItem Selected="True">Y</asp:ListItem>
            <asp:ListItem>N</asp:ListItem>
            </asp:DropDownList> 
            </td>  
        </tr>
        <tr>
            <td>Operator</td>
            <td>:</td>
            <td colspan="4"><asp:TextBox runat="server" ValidationGroup="Input" ID="tbOperator" MaxLength="255" CssClass="TextBox" Width="225px"/></td>
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
                <asp:MenuItem Text="Detail Fixed Asset" Value="0"></asp:MenuItem>
             </Items>            
        </asp:Menu>
        <br />
        <asp:MultiView 
        ID="MultiView1"
        runat="server"
        ActiveViewIndex="0">
           <asp:View ID="Tab1" runat="server">
              <asp:Panel runat="server" ID="PnlDt">
                  <asp:Button class="bitbtndt btnadd" runat="server" ID="btnAdddt" Text="Add" ValidationGroup="Input"/>    
                  <div style="border:0px  solid; width:100%; height:100%; overflow:auto;">
                    <asp:GridView ID="GridDt" runat="server" AutoGenerateColumns="false" 
                        ShowFooter="True">
                        <HeaderStyle CssClass="GridHeader" />
                        <RowStyle CssClass="GridItem" Wrap="false" />
                        <AlternatingRowStyle CssClass="GridAltItem" />
                        <PagerStyle CssClass="GridPager" />
                        <Columns>
                            <asp:TemplateField HeaderText="Action"><ItemTemplate><asp:Button class="bitbtndt btnedit" runat="server" ID="btnEdit" Text="Edit" CommandName="Edit"/><asp:Button class="bitbtndt btndelete" runat="server" ID="btnDelete" Text="Delete" CommandName="Delete" OnClientClick="return confirm('Sure to delete this data?');"  /></ItemTemplate></asp:TemplateField>                                                        
                            <asp:BoundField DataField="FixedAsset" HeaderStyle-Width="100px" HeaderText="Fixed Asset Code" />
                            <asp:BoundField DataField="FA_Name" HeaderStyle-Width="100px" HeaderText="Fixed Asset Name" />                                                                                  
                            <asp:BoundField DataField="BalanceLife" HeaderStyle-Width="100px" HeaderText="Balance Life" />
                            <asp:BoundField DataField="BalanceNDA" HeaderStyle-Width="100px" HeaderText="Balance Amount NDA" />
                            <asp:BoundField DataField="BalanceDA" HeaderStyle-Width="100px" HeaderText="Balance Amount DA" />
                            <asp:BoundField DataField="BalanceAmount" HeaderStyle-Width="100px" HeaderText="Balance Amount" />
                            <asp:BoundField DataField="FgRemove" HeaderStyle-Width="80px" HeaderText="Remove" />
                            <asp:BoundField DataField="NewLife" HeaderStyle-Width="100px" HeaderText="New Life" />
                            <asp:BoundField DataField="NewAmountNDA" HeaderStyle-Width="100px" HeaderText="New Amount NDA" />
                            <asp:BoundField DataField="NewAmountDA" HeaderStyle-Width="100px" HeaderText="New Amount DA" />
                            <asp:BoundField DataField="NewAmount" HeaderStyle-Width="100px" HeaderText="New Amount" />
                            <asp:BoundField DataField="RevLife" HeaderStyle-Width="100px" HeaderText="Revaluation Life" />
                            <asp:BoundField DataField="RevAmount" HeaderStyle-Width="100px" HeaderText="Revaluation Amount" />
                            <asp:BoundField DataField="Remark" HeaderStyle-Width="100px" HeaderText="Remark" />
                          </Columns>
                    </asp:GridView>
              </div>  
              <asp:Button class="bitbtndt btnadd" runat="server" ID="btnAddDtke2" Text="Add" ValidationGroup="Input"/>    
              </asp:Panel> 
              <asp:Panel runat="server" ID="pnlEditDt" Visible="false" Width="1079px">
                <table>                                                                
                    <tr>
                        <td class="style1">Fixed Asset</td>
                        <td>:</td>
                        <td colspan="4">                                
                            <asp:TextBox CssClass="TextBox" ValidationGroup="Input" runat="server" ID="tbFACode" AutoPostBack="true" OnTextChanged = "tbFACode_TextChanged"/>
                            <asp:TextBox CssClass="TextBox" runat="server" ID="tbFAName" Enabled="false" Width="225px"/>
                            <asp:Button class="btngo" runat="server" ID="btnFA" Text="..." ValidationGroup="Input"/>    
                        </td>
                    </tr>
                    <tr>                        
                        <td class="style1">Fg Remove</td>
                        <td>:</td>
                        <td><asp:DropDownList AutoPostBack="true" ValidationGroup="Input" CssClass="DropDownList" ID="ddlFgRemove" runat="server" >
                        <asp:ListItem Selected="True">Y</asp:ListItem>
                        <asp:ListItem>N</asp:ListItem>
                        </asp:DropDownList> 
                        </td> 
                    </tr>                                           
                    <tr> 
                        <td class="style1">Life</td>
                        <td>:</td>
                        <td colspan="4">
                            <table>
                            <tr style="background-color:Silver;text-align:center">
                                <td>Balance</td>
                                <td>New</td>
                                <td>Revaluation</td>
                            </tr>
                            <tr>
                                <td><asp:TextBox CssClass="TextBoxR" runat="server" ID="tbLifeBal"/></td>                                    
                                <td><asp:TextBox CssClass="TextBox" runat="server" ID="tbLifeNew"/></td>
                                <td><asp:TextBox CssClass="TextBoxR" runat="server" ID="tbLifeRev"/></td>                                    
                            </tr>
                            </table>
                        </td>                                                              
                    </tr>           
                    <tr> 
                        <td class="style1">Amount</td>
                        <td>:</td>
                        <td colspan="4">
                            <table>
                            <tr style="background-color:Silver;text-align:center">
                                <td>Balance NDA</td>
                                <td>Balance DA</td>
                                <td>Balance Total</td>
                                <td>New NDA</td>
                                <td>New DA</td>
                                <td>New Total</td>
                                <td>Revaluation</td>
                            </tr>
                            <tr>
                                <td><asp:TextBox CssClass="TextBoxR" runat="server" ID="tbAmountBalNDA"/></td>                                    
                                <td><asp:TextBox CssClass="TextBoxR" runat="server" ID="tbAmountBalDA"/></td>
                                <td><asp:TextBox CssClass="TextBoxR" runat="server" ID="tbAmountBal"/></td>                                    
                                <td><asp:TextBox CssClass="TextBox" runat="server" ID="tbAmountNewNDA"/></td>
                                <td><asp:TextBox CssClass="TextBox" runat="server" ID="tbAmountNewDA"/></td>
                                <td><asp:TextBox CssClass="TextBoxR" runat="server" ID="tbAmountNew"/></td>
                                <td><asp:TextBox CssClass="TextBoxR" runat="server" ID="tbAmountRev"/></td>                                    
                            </tr>
                            </table>
                        </td>                                                              
                    </tr>                          
                    <tr>      
                        <td class="style1">Remark</td>
                        <td>:</td>
                        <td><asp:TextBox CssClass="TextBox" runat="server" ID="tbRemarkDt" Width="225px"/></td>              
                    </tr>
                </table>
                <br />  
                <asp:Button class="bitbtndt btnsave" runat="server" ID="btnSaveDt" Text="Save"/>    
                <asp:Button class="bitbtndt btncancel" runat="server" ID="btnCancelDt" Text="Cancel"/>    
           </asp:Panel> 
              
           </asp:View>           
            
        </asp:MultiView>
    
       <br />    
        <asp:Button class="bitbtn btnsavenew" runat="server" ID="btnSaveAll" 
            Text="Save & New" ValidationGroup="Input" Width="103px"/>    
        <asp:Button class="bitbtn btnsave" runat="server" ID="btnSaveTrans" Text="Save" ValidationGroup="Input"/>    
        <asp:Button class="bitbtn btncancel" runat="server" ID="btnBack" Text="Cancel" ValidationGroup="Input"/>    
        <asp:Button class="btngo" runat="server" ID="btnHome" Text="Home" Width="42px"/>    
      </asp:Panel>        
    </div>
   
    <asp:Label runat ="server" ID="lbStatus" ForeColor="Red"/>
    </form>
</body>
</html>
