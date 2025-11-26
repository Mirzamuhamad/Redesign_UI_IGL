<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TrFASales.aspx.vb" Inherits="TrFASales" %>
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
        
        
        function BasePPnOtherTotal(_prmBaseForex, _prmPPn, _prmPPnForex, _prmTotalForex)
        {
        try
        {
        var _tempBaseForex = parseFloat(_prmBaseForex.value.replace(/\$|\,/g,""));
        if(isNaN(_tempBaseForex) == true)
        {
           _tempBaseForex = 0;
           _prmBaseForex.value = addCommas(_tempBaseForex);
        }        
        
        var _tempPPn = parseFloat(_prmPPn.value.replace(/\$|\,/g,""));
        if(isNaN(_tempPPn) == true)
        {
           _tempPPn = 0;
           _prmPPn.value = addCommas(_tempPPn);
        }
               
        
        var _tempPPnForex = (_tempBaseForex * _tempPPn) / 100.00;
        var _tempTotalForex = _tempBaseForex + _tempPPnForex;         
        
        
        _prmPPnForex.value = addCommas(_tempPPnForex);
        
        _prmTotalForex.value = addCommas(_tempTotalForex);  
                       
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
        
        document.getElementById("tbRate").value = setdigit(Rate,'<%=ViewState("DigitRate")%>');
        document.getElementById("tbPPN").value = setdigit(PPn,'<%=ViewState("DigitPercent")%>');
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
//         var qty = document.getElementById("tbQty").value.replace(/\$|\,/g,"");
        //var AmountF = document.getElementById("tbAmountBuy").value.replace(/\$|\,/g,"");
//         var AmountB = document.getElementById("tbAmountForex").value.replace(/\$|\,/g,"");
//         var AmountH = document.getElementById("tbAmountHome").value.replace(/\$|\,/g,"");
//         var AmountC = document.getElementById("tbAmountCurrent").value.replace(/\$|\,/g,"");         
//                                    
//                                   
//         document.getElementById("tbQty").value = setdigit(qty,'<%=ViewState("DigitQty")%>');                  
//         document.getElementById("tbAmountBuy").value = setdigit(AmountB,'<%=ViewState("DigitCurr")%>');
 //        document.getElementById("tbAmountForex").value = setdigit(AmountF,'<%=ViewState("DigitCurr")%>');
 //        document.getElementById("tbAmountHome").value = setdigit(AmountH,'<%=ViewState("DigitCurr")%>');
//         document.getElementById("tbAmountCurrent").value = setdigit(AmountC,'<%=ViewState("DigitCurr")%>');
//                  
         

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
            width: 65px;
        }
        .style2
        {
            width: 3px;
        }
    </style>
</head>
<body>     
    <form id="form1" runat="server">    
    <div class="Content">
    <div class="H1">Fixed Asset Sales</div>
     <hr style="color:Blue" />        
    <asp:Panel runat="server" id="PnlHd"> 
      <table>
        <tr>
            <td style="width:100px;text-align:right">Quick Search :</td>            
            <td><asp:TextBox CssClass="TextBox" runat="server" ID ="tbFilter"/> 
                  <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlField" >
                      <asp:ListItem Value="TransNmbr" Selected="True">Reference</asp:ListItem>
                      <asp:ListItem>Status</asp:ListItem>
                      <asp:ListItem Value="dbo.FormatDate(TransDate)">Date</asp:ListItem>
                      <asp:ListItem Value="UserType">User Type</asp:ListItem>
                      <asp:ListItem Value="User_Name">User Name</asp:ListItem>
                      <asp:ListItem Value="Attn">Attn</asp:ListItem>                    
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
                      <asp:ListItem Value="TransNmbr" Selected="True">Reference</asp:ListItem>
                      <asp:ListItem>Status</asp:ListItem>
                      <asp:ListItem Value="dbo.FormatDate(TransDate)">Date</asp:ListItem>
                      <asp:ListItem Value="UserType">User Type</asp:ListItem>
                      <asp:ListItem Value="User_Name">User Name</asp:ListItem>
                      <asp:ListItem Value="Attn">Attn</asp:ListItem>
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
                  <asp:BoundField DataField="TransNmbr" HeaderStyle-Width="120px" SortExpression="Nmbr" HeaderText="Reference"></asp:BoundField>                  
                  <asp:BoundField DataField="Status" HeaderText="Status"></asp:BoundField>                  
                  <asp:BoundField DataField="TransDate" dataformatstring="{0:dd MMM yyyy}" htmlencode="true" HeaderStyle-Width="80px" SortExpression="TransDate" HeaderText="Date"></asp:BoundField>
                  <asp:BoundField DataField="UserType" HeaderStyle-Width="200px" SortExpression="UserType" HeaderText="User Type"></asp:BoundField>
                  <asp:BoundField DataField="User_Name" HeaderStyle-Width="150px" SortExpression="User_Name" HeaderText="User Name"></asp:BoundField>
                  <asp:BoundField DataField="Attn" HeaderStyle-Width="120px" SortExpression="Attn" HeaderText="Attn"></asp:BoundField>                  
                  <asp:BoundField DataField="Currency" HeaderStyle-Width="50px" SortExpression="Currency" HeaderText="Currency"></asp:BoundField>
                  <asp:BoundField DataField="BaseForex" HeaderStyle-Width="80px" SortExpression="BaseForex" HeaderText="Base Forex"></asp:BoundField>
                  <asp:BoundField DataField="PPnForex" HeaderStyle-Width="80px" SortExpression="PPnForex" HeaderText="PPn Forex"></asp:BoundField>
                  <asp:BoundField DataField="TotalForex" HeaderStyle-Width="80px" SortExpression="TotalForex" HeaderText="Total Forex"></asp:BoundField>
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
            <td>FA Sales No</td>
            <td>:</td>
            <td><asp:TextBox CssClass="TextBoxR" runat="server" ID="tbCode" Width="150px" Enabled="False"/>             
            <asp:DropDownList AutoPostBack="true" ValidationGroup="Input" CssClass="DropDownList" ID="ddlReport" runat="server" >
                        <asp:ListItem Selected="True">Y</asp:ListItem>
                        <asp:ListItem>N</asp:ListItem>
                    </asp:DropDownList> 
            </td>            
        </tr>
        <tr>
            <td>FA Sales Date</td>
            <td>:</td>
            <td>
            <BDP:BasicDatePicker ID="tbDate" runat="server" DateFormat="dd MMM yyyy" 
                        ReadOnly = "true" ValidationGroup="Input"
                        ButtonImageHeight="19px" ButtonImageWidth="20px" 
                        DisplayType="TextBoxAndImage" 
                        TextBoxStyle-CssClass="TextDate" AutoPostBack="True" 
                        ShowNoneButton="False"><TextBoxStyle CssClass="TextDate" /></BDP:BasicDatePicker>                
            </td>            
        </tr>          
                  
        <tr>
            <td>User</td>
            <td>:</td>
            <td><asp:DropDownList CssClass="DropDownList" ID="ddlUserType" runat="server" AutoPostBack ="true">
                            <asp:ListItem Selected="True">CUSTOMER</asp:ListItem>
                            <asp:ListItem>SUPPLIER</asp:ListItem>
                </asp:DropDownList>
                <asp:TextBox CssClass="TextBox" ValidationGroup="Input" runat="server" ID="tbUserCode" AutoPostBack="true" />
                <asp:TextBox CssClass="TextBox" runat="server" ID="tbUserName" Enabled="false" Width="225px"/>
                <asp:Button class="btngo" runat="server" ID="btnUser" Text="..." ValidationGroup="Input" />
            </td>
        </tr>
        <tr>
            <td>Attn</td>
            <td>:</td>
            <td><asp:TextBox runat="server" ValidationGroup="Input" ID="tbAttn" CssClass="TextBox" Width="225px"/></td>
        </tr>
        <tr>
            <td>Payment</td>
            <td>:</td>
            <td><asp:DropDownList CssClass="DropDownList" ID="ddlPayWith" runat="server" AutoPostBack ="true">
                            <asp:ListItem Selected="True">Cash</asp:ListItem>
                            <asp:ListItem>Non Cash</asp:ListItem>
                </asp:DropDownList>
                <asp:TextBox CssClass="TextBox" ValidationGroup="Input" runat="server" ID="tbPayCode" AutoPostBack="true" />
                <asp:TextBox CssClass="TextBox" runat="server" ID="tbPayName" Enabled="false" Width="225px"/>
                <asp:Button class="btngo" runat="server" ID="btnPayment" Text="..." ValidationGroup="Input" />
            </td>
        </tr>
        <tr>
            <td><asp:LinkButton ID="lbTerm" ValidationGroup="Input" runat="server" Text="Term"/></td>
            <td>:</td>
            <td><asp:DropDownList CssClass="DropDownList" ValidationGroup="Input" runat="server" ID="ddlTerm" AutoPostBack="true" /> 
                <BDP:BasicDatePicker ID="tbDueDate" runat="server" DateFormat="dd MMM yyyy" 
                        ReadOnly = "true" Enabled="false"
                        ButtonImageHeight="19px" ButtonImageWidth="20px" 
                        DisplayType="TextBox" 
                        TextBoxStyle-CssClass="TextDate" AutoPostBack="True" 
                        ShowNoneButton="False"><TextBoxStyle CssClass="TextDate" /></BDP:BasicDatePicker>
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
                 <td>
                 <asp:TextBox runat="server" ValidationGroup="Input" ID="tbPPnNo" CssClass="TextBox" Width="150px"/>
                 </td>
                 <td>
                 <BDP:BasicDatePicker ID="tbPPndate" runat="server" DateFormat="dd MMM yyyy" 
                            ReadOnly = "true" ValidationGroup="Input"
                            ButtonImageHeight="19px" ButtonImageWidth="20px" 
                            DisplayType="TextBoxAndImage" 
                            TextBoxStyle-CssClass="TextDate" AutoPostBack="True" ShowNoneButton="False"><TextBoxStyle CssClass="TextDate" /></BDP:BasicDatePicker>                
                 </td>
                 <td>
                 <asp:TextBox runat="server" ValidationGroup="Input" ID="tbPpnRate" CssClass="TextBox" Width="68px"/>
                 </td>
                 </tr>
                 </table>
            </td>                         
        </tr> 
        <tr>
                <td><asp:LinkButton ID="lbCurr" ValidationGroup="Input" runat="server" Text="Currency"/></td>
                <td>:</td>
                <td><asp:DropDownList ID="ddlCurr" ValidationGroup="Input" AutoPostBack="true" runat="server" CssClass="DropDownList" />                                                                   
                &nbsp Rate : &nbsp <asp:TextBox ID="tbRate" ValidationGroup="Input" runat="server" CssClass="TextBox" Width="68px" />
                </td>
        </tr>  
        <tr>
                <td>Total</td>
                <td>:</td>
                <td colspan="7">
                    <table>
                        <tr style="background-color:Silver;text-align:center">
                            <td>Base Forex</td>
                            <td>PPN %</td>
                            <td>PPN Forex</td>
                            <td>Total Forex</td>
                        </tr>
                        <tr>
                            <td><asp:TextBox ID="tbBaseForex" runat="server" CssClass="TextBoxR" Width="90px"/></td>
                            <td><asp:TextBox ID="tbPPN" ValidationGroup="Input" runat="server" CssClass="TextBox" width="40px"/></td>
                            <td><asp:TextBox ID="tbPPNForex" runat="server" CssClass="TextBoxR" Width="90px"/></td>
                            <td><asp:TextBox ID="tbTotalForex" runat="server" CssClass="TextBoxR" Width="90px"/></td>
                        </tr>
                    </table>
                </td>                
        </tr>           
        <tr>
            <td>Remark</td>
            <td>:</td>
            <td><asp:TextBox runat="server" ValidationGroup="Input" ID="tbRemark" CssClass="TextBox" Width="225px"/></td>
        </tr>
      </table>  
      
      <br />      
      <div style="font-size:medium; color:Blue;"></div>
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
                <%--<asp:MenuItem Text="Detail FA Location" Value="1"></asp:MenuItem>--%>
            </Items>            
        </asp:Menu>
        <br />
        <asp:MultiView 
        ID="MultiView1"
        runat="server"
        ActiveViewIndex="0">
           <asp:View ID="Tab1" runat="server">
        <asp:Panel runat="server" ID="pnlDt">
            <asp:Button class="bitbtndt btnadd" runat="server" ID="btnAddDt" Text="Add" ValidationGroup="Input" />
            <br />
            <br />
            <div style="border:0px  solid; width:100%; height:100%; overflow:auto;">
                <asp:GridView ID="GridDt" runat="server" AutoGenerateColumns="False" 
                    ShowFooter="false">
                    <HeaderStyle CssClass="GridHeader" />
                    <RowStyle CssClass="GridItem" Wrap="false" />
                    <AlternatingRowStyle CssClass="GridAltItem" />
                    <PagerStyle CssClass="GridPager" />
                    <Columns>
                        <asp:TemplateField HeaderText="Action">
                            <ItemTemplate>
                                <asp:Button class="bitbtndt btnedit" runat="server" ID="btnEdit" Text="Edit" CommandName="Edit"  />
                                <asp:Button class="bitbtndt btndelete" runat="server" ID="btnDelete" Text="Delete" CommandName="Delete" visible = "True"/>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:Button class="bitbtndt btnsave" runat="server" ID="btnUpdate" Text="Save" CommandName="Update"/>
                                <asp:Button class="bitbtndt btncancel" runat="server" ID="btnCancel" Text="Cancel" CommandName="Cancel"/>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                                 <ItemTemplate>
                                  <asp:Button class="bitbtndt btndetail" runat="server" ID="btnView" Text="Detail" CommandName="View" CommandArgument='<%# Container.DataItemIndex %>'/>
                                 </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="FixedAsset" HeaderStyle-Width="100px" 
                            HeaderText="FA Code" >
                            <HeaderStyle Width="100px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="FA_Name" HeaderStyle-Width="150px" 
                            HeaderText="FA Name" >                        
                            <HeaderStyle Width="150px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Specification" HeaderStyle-Width="150px" 
                            HeaderText="Specification" >                        
                            <HeaderStyle Width="150px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Cost_Ctr_Name" HeaderStyle-Width="150px" 
                            HeaderText="Cost Center">
                            <HeaderStyle Width="150px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Qty" HeaderStyle-Width="80px" HeaderText="Qty" >                                                 
                            <HeaderStyle Width="80px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="AmountForex" HeaderStyle-Width="80px" 
                            ItemStyle-HorizontalAlign="Right" HeaderText="Amount Forex" >                                                    
                            <HeaderStyle Width="80px" />
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:BoundField>
                        <asp:BoundField DataField="AmountHome" HeaderStyle-Width="80px" 
                            ItemStyle-HorizontalAlign="Right" HeaderText="Amount Home" >
                            <HeaderStyle Width="80px" />
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:BoundField>
                    </Columns>
                </asp:GridView>
          </div>   
          <asp:Button class="bitbtndt btnadd" runat="server" ID="btnAddDtKe2" Text="Add" ValidationGroup="Input" />
      </asp:Panel>  
               <br />
               <asp:Panel ID="pnlEditDt" runat="server" Visible="false">
                   <table>
                       <tr>
                           <td class="style1">
                               Fixed Asset</td>
                           <td class="style2">
                               :</td>
                           <td>
                               <asp:TextBox ID="tbFACode" runat="server" CssClass="TextBox" 
                                   OnTextChanged = "tbFACode_TextChanged" AutoPostBack="True"/>
                               <asp:TextBox ID="tbFAName" runat="server" CssClass="TextBox" Enabled="false" 
                                   Width="225px" />
                               <asp:Button class="btngo" runat="server" ID="btnFA" Text="..." ValidationGroup="Input" />    
                         </td>
                       </tr>
                       <tr>
                           <td class="style1">
                               Specification</td>
                           <td class="style2">
                               :</td>
                           <td>
                               <asp:TextBox ID="tbSpecification" runat="server" CssClass="TextBox" Enabled="false" Height="38px" TextMode="MultiLine" Width="362px" />
                           </td>
                       </tr>
                       <tr>
                           <td class="style1">
                               Cost Ctr</td>
                           <td class="style2">
                               :</td>
                           <td>
                               <asp:DropDownList ID="ddlCostCtr" runat="server" CssClass="DropDownList" 
                                   Enabled="False" Height="16px" ValidationGroup="Input" Width="129px" />
                           </td>
                       </tr>
                       <tr>
                           <td class="style1">
                               Qty</td>
                           <td class="style2">
                               :</td>
                           <td>
                               &nbsp;<asp:TextBox ID="tbQty" runat="server" CssClass="TextBoxR" Enabled="False" 
                                   Width="68px" />
                           </td>
                       </tr>
                       <tr>
                           <td class="style1">
                               Amount</td>
                           <td class="style2">
                               :</td>
                           <td>
                               <table>
                                   <tr style="background-color:Silver;text-align:center">
                                       <td>
                                           Forex</td>
                                       <td>
                                           Home</td>
                                   </tr>
                                   <tr>
                                       <td>
                                           <asp:TextBox ID="tbAmountForex" runat="server" AutoPostBack="true" 
                                               CssClass="TextBox" />
                                       </td>
                                       <td>
                                           <asp:TextBox ID="tbAmountHome" runat="server" CssClass="TextBoxR" 
                                               Enabled="False" />
                                       </td>
                                   </tr>
                               </table>
                           </td>
                       </tr>
                   </table>
                   <br />
                   <asp:Button class="bitbtndt btnsave" runat="server" ID="btnSaveDt" Text="Save" ValidationGroup="Input" />
                   <asp:Button class="bitbtndt btncancel" runat="server" ID="btnCancelDt" Text="Cancel" ValidationGroup="Input" />
               </asp:Panel>
       </asp:View>           
            <asp:View ID="Tab2" runat="server">     
                <table>
                    <tr>
                        <td>
                            Fixed Asset</td>
                        <td>
                            :</td>
                        <td>
                            <asp:Label ID="lbFADt2" runat="server" Text="Fixed Asset" />
                            &nbsp;
                            <asp:Label ID="lbFANameDt2" runat="server" Text="Fixed Asset Name" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Cost Center</td>
                        <td>
                            :</td>
                        <td>
                            <asp:Label ID="lbCostCtr" runat="server" />
                        </td>
                    </tr>
                </table>
                <hr style="color:Blue" />
                <asp:Panel ID="pnlDt2" runat="server">
                    <br />
                    <asp:Button class="bitbtndt btnadd" runat="server" ID="btnAddDt2" Text="Add" ValidationGroup="Input" />
                    <asp:Button class="bitbtndt btnback" runat="server" ID="btnBackDt2ke1" Text="Back"/>
                    <div style="border:0px  solid; width:100%; height:100%; overflow:auto;">
                        <asp:GridView ID="GridDt2" runat="server" AutoGenerateColumns="false" 
                            ShowFooter="false">
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
                            <asp:BoundField DataField="FALocationType" HeaderStyle-Width="150px" HeaderText="FA Location Type" />
                            <asp:BoundField DataField="FALocationCode" HeaderStyle-Width="150px" HeaderText="FA Location Code" />                            
                            <asp:BoundField DataField="FA_Location_Name" HeaderStyle-Width="150px" HeaderText="FA Location Name" />
                            <asp:BoundField DataField="Qty" HeaderStyle-Width="80px" HeaderText="Qty" />
                            <asp:BoundField DataField="Remark" HeaderStyle-Width="150px" HeaderText="Remark" />
                               
                            </Columns>
                        </asp:GridView>
                    </div>
                    <asp:Button class="bitbtndt btnadd" runat="server" ID="btnAddDt2Ke2" Text="Add" ValidationGroup="Input" />
                    <asp:Button class="bitbtndt btnback" runat="server" ID="btnBackDt2ke2" Text="Back" />
                  </asp:Panel>
                <asp:Panel ID="pnlEditDt2" runat="server" Visible="false">
                    <table>
                        <tr>
                            <td>
                                FA Location</td>
                            <td>
                                :</td>
                            <td colspan="4">
                                <asp:DropDownList ID="ddlFALocType" runat="server" AutoPostBack="true" 
                                    CssClass="DropDownList">
                                    <asp:ListItem Selected="True">GENERAL</asp:ListItem>
                                    <asp:ListItem>CUSTOMER</asp:ListItem>
                                    <asp:ListItem>SUPPLIER</asp:ListItem>
                                    <asp:ListItem>EMPLOYEE</asp:ListItem>
                                </asp:DropDownList>
                                <asp:TextBox ID="tbFALocCode" runat="server" AutoPostBack="true" 
                                    CssClass="TextBox" ValidationGroup="Input" />
                                <asp:TextBox ID="tbFALocName" runat="server" CssClass="TextBox" Enabled="false" 
                                    Width="225px" />
                                <asp:Button class="btngo" runat="server" ID="btnFALoc" Text="..." ValidationGroup="Input" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Qty</td>
                            <td>
                                :</td>
                            <td>
                                <asp:TextBox ID="tbQtyDt2" runat="server" CssClass="TextBox" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Remark</td>
                            <td>
                                :</td>
                            <td colspan="4">
                                <asp:TextBox ID="tbRemarkDt2" runat="server" CssClass="TextBox" MaxLength="255" 
                                    TextMode="MultiLine" Width="365px" />
                            </td>
                        </tr>
                    </table>
                    <br />
                    <asp:Button class="bitbtndt btnsave" runat="server" ID="btnSaveDt2" Text="Save" ValidationGroup="Input" />
                    <asp:Button class="bitbtndt btncancel" runat="server" ID="btnCancelDt2" Text="Cancel" ValidationGroup="Input" />
                </asp:Panel>
       </asp:View>            
        </asp:MultiView>
    
       <br />    
        <asp:Button class="bitbtn btnsavenew" runat="server" ID="btnSaveAll" 
            Text="Save & New" ValidationGroup="Input" Width="103px" />
        <asp:Button class="bitbtn btnsave" runat="server" ID="btnSaveTrans" Text="Save" ValidationGroup="Input" />
        <asp:Button class="bitbtn btnback" runat="server" ID="btnBack" Text="Cancel" ValidationGroup="Input" />
        <asp:Button class="bitbtn btnback" runat="server" ID="btnHome" Text="Home" 
            />
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
