<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TrFATransfer.aspx.vb" Inherits="Transaction_TrFATransfer_TrFATransfer" %>

<%@ Register Assembly="BasicFrame.WebControls.BasicDatePicker" Namespace="BasicFrame.WebControls"
    TagPrefix="BDP" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>

    <script src="../../Function/OpenDlg.js" type="text/javascript"></script>
    <script src="../../Function/Function.JS" type="text/javascript"></script>
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
       <link type="text/css" rel="stylesheet" href="../../Styles/circularprogress.css" />
    <script type="text/javascript" src="../../JQuery/jquery.min.js"></script>



    <script type="text/javascript">


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
          var VPayment = document.getElementById("tbTotalPayment").value.replace(/\$|\,/g,"");
          var VExpense = document.getElementById("tbTotalExpense").value.replace(/\$|\,/g,"");
          var VCharge = document.getElementById("tbTotalCharge").value.replace(/\$|\,/g,"");
          
          document.getElementById("tbTotalPayment").value = setdigit(VPayment,'<%=ViewState("DigitHome")%>');
          document.getElementById("tbTotalExpense").value = setdigit(VExpense,'<%=ViewState("DigitHome")%>');
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
         
         var LifeMonth = parseFloat(document.getElementById("tbLifeMonth").value.replace(/\$|\,/g,"")); 
         var LifeDepr = parseFloat(document.getElementById("tbLifeProcessDepr").value.replace(/\$|\,/g,"")); 
         var AmountDepr = parseFloat(document.getElementById("tbAmountProcessDepr").value.replace(/\$|\,/g,"")); 
         var PriceForex = parseFloat(document.getElementById("tbPriceForex").value.replace(/\$|\,/g,"")); 
         
         if(isNaN(LifeMonth) == true)
         {
           LifeMonth = 0;
         }
         if(isNaN(LifeDepr) == true)
         {
           LifeDepr = 0;
         }                
         
         AmountDepr =  (LifeDepr/LifeMonth) * AmountHome   
         
         if(isNaN(AmountDepr) == true)
         {
           AmountDepr = 0;
         }      
                                   
         document.getElementById("tbRateDt").value = setdigit(Rate,'<%=ViewState("DigitCurr")%>');
         document.getElementById("tbAmountForexDt").value = setdigit(AmountForex,'<%=VIEWSTATE("DigitCurr")%>');
         document.getElementById("tbAmountHomeDt").value = setdigit(AmountHome,'<%=ViewState("DigitHome")%>');
         
         document.getElementById("tbLifeMonth").value = setdigit(LifeMonth,'<%=ViewState("DigitQty")%>');
         document.getElementById("tbLifeProcessDepr").value = setdigit(LifeDepr,'<%=ViewState("DigitQty")%>');
         document.getElementById("tbAmountProcessDepr").value = setdigit(AmountDepr,'<%=ViewState("DigitHome")%>');
         
        }catch (err){
            alert(err.description);
          }      
        }   

        function setformatdt2()
        {
        try
         {
         
        }catch (err){
            alert(err.description);
          }
      }
      function OpenPopup() {
          var left = (screen.width - 600) / 2; //370
          var top = (screen.height - 600) / 2;
          var winOpen = window.open("../../earchDlgV.Aspx", "", 'toolbar=no, location=no, directories=no, status=no, menubar=no, scrollbars=yes, resizable=yes, copyhistory=no, width=' + 650 + ', height=' + 600 + ', top=' + top + ', left=' + left);
          //winOpen.reload(); 
          Opener.Location.reload(false);
          return false;
      }  
               
        
    </script>

    <style type="text/css">
        .style1
        {
            width: 115px;
        }
        .style2
        {
            width: 80px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div class="Content">
        <div class="H1">
            <asp:Label runat="server" ID="lblTitle" /></div>
        <hr style="color: Blue" />
        <asp:Panel runat="server" ID="PnlHd">
            <table>
                <tr>
                    <td style="width: 100px; text-align: right">
                        Quick Search :
                    </td>
                    <%--TransNmbr, TransDate, STATUS, FgReport, UserType, UserCode, UserName, Attn, Remark--%>
                    <td>
                        <asp:TextBox CssClass="TextBox" runat="server" ID="tbFilter" />
                        <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlField">
                            <asp:ListItem Selected="True" Value="TransNmbr">Reference</asp:ListItem>
                            <asp:ListItem Value="dbo.FormatDate(TransDate)">Date</asp:ListItem>
                            <asp:ListItem>Status</asp:ListItem>
                            <asp:ListItem Value="Operator">Operator</asp:ListItem>
                            <asp:ListItem Value="Remark">Remark</asp:ListItem>
                            <asp:ListItem Value="RRNo">RR No</asp:ListItem>
                        </asp:DropDownList>
                        <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlRange">
                        </asp:DropDownList>
                        <asp:Button class="bitbtn btnsearch" runat="server" ID="btnSearch" Text="Search" />
                        <asp:Button class="btngo" runat="server" ID="btnExpand" Text="..." />
                    </td>
                    <td>
                        <asp:LinkButton ID="LbAdvSearch" runat="server" Text="Advanced Search" />
                    </td>
                </tr>
            </table>
            <asp:Panel runat="server" ID="pnlSearch" Visible="false">
                <table>
                    <tr>
                        <td style="width: 100px; text-align: right">
                            <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlNotasi">
                                <asp:ListItem Selected="true" Text="OR" Value="OR"></asp:ListItem>
                                <asp:ListItem Text="AND" Value="AND"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:TextBox runat="server" CssClass="TextBox" ID="tbfilter2" />
                            <asp:DropDownList runat="server" CssClass="DropDownList" ID="ddlField2">
                                <asp:ListItem Selected="True" Value="TransNmbr">Reference</asp:ListItem>
                                <asp:ListItem Value="dbo.FormatDate(TransDate)">Date</asp:ListItem>
                                <asp:ListItem>Status</asp:ListItem>
                                <asp:ListItem Value="Operator">Operator</asp:ListItem>
                                <asp:ListItem Value="Remark">Remark</asp:ListItem>
                                <asp:ListItem Value="RRNo">RR No</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <br />
            <asp:Button class="bitbtn btnadd" runat="server" ID="BtnAdd" Text="Add" />
            <asp:DropDownList CssClass="DropDownList" ID="ddlCommand" runat="server" Visible="false" />
            <asp:Button class="btngo" runat="server" ID="BtnGo" Text="G" Visible="false" />
            <br />&nbsp;
            <div style="border: 0px  solid; width: 100%; height: 100%; overflow: auto;">
                <asp:GridView ID="GridView1" runat="server" AllowPaging="True" AllowSorting="true"
                    CssClass="Grid" AutoGenerateColumns="false">
                    <HeaderStyle CssClass="GridHeader"></HeaderStyle>
                    <RowStyle CssClass="GridItem" Wrap="false" />
                    <AlternatingRowStyle CssClass="GridAltItem" />
                    <PagerStyle CssClass="GridPager" />
                    <Columns>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:CheckBox ID="cbSelectHd" runat="server" AutoPostBack="true" OnCheckedChanged="cbSelectHd_CheckedChanged" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="cbSelect" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-Width="100" HeaderText="Action">
                            <ItemTemplate>
                                <asp:DropDownList CssClass="DropDownList" ID="ddl" runat="server" Width="60px"> 
                                    <asp:ListItem Selected="True" Text="View" />
                                    <asp:ListItem Text="Edit" />
                                    <%--<asp:ListItem Text="Print" />--%>
                                </asp:DropDownList>
                                <asp:Button class="btngo" runat="server" ID="BtnGo" Text="G" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>"
                                    CommandName="Go" />
                            </ItemTemplate>
                            <HeaderStyle Width="100px" />
                        </asp:TemplateField>
                        <asp:BoundField DataField="TransNmbr" HeaderStyle-Width="120px" SortExpression="Nmbr"
                            HeaderText="Registration No"></asp:BoundField>
                        <asp:BoundField DataField="Status" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="60px" HeaderText="Status"></asp:BoundField>
                        <asp:BoundField DataField="TransDate" DataFormatString="{0:dd MMM yyyy}" HtmlEncode="true"
                            HeaderStyle-Width="80px" SortExpression="TransDate" HeaderText="Date"></asp:BoundField>
                            
                        <asp:BoundField DataField="Operator" HeaderStyle-Width="80px" SortExpression="Operator" HeaderText="Operator"></asp:BoundField>
                        <asp:BoundField DataField="Remark" HeaderStyle-Width="250px" HeaderText="Remark">
                        </asp:BoundField>
                    </Columns>
                </asp:GridView>
            </div>
            <br/>&nbsp; 
            <asp:Panel runat="server" ID="pnlNav" Visible="false">
                <asp:Button class="bitbtn btnadd" runat="server" ID="btnAdd2" Text="Add" />
                &nbsp &nbsp &nbsp
                <asp:DropDownList ID="ddlCommand2" CssClass="DropDownList" runat="server" />
                <asp:Button class="btngo" runat="server" ID="btnGo2" Text="G" />
            </asp:Panel>
        </asp:Panel>
        <asp:Panel runat="server" ID="pnlInput" Visible="false">
            <table>
                <tr>
                    <td>Registration No</td>
                    <td>:</td> 
                    <td width="250px">
                        <asp:TextBox CssClass="TextBoxR" runat="server" ID="tbCode" Width="150px" Enabled="False" />
                    </td>
                    <td>Registration Date</td>
                    <td>:</td>
                    <td>
                        <BDP:BasicDatePicker ID="tbDate" runat="server" DateFormat="dd MMM yyyy" ReadOnly="true"
                            ValidationGroup="Input" ButtonImageHeight="19px" ButtonImageWidth="20px" DisplayType="TextBoxAndImage"
                            TextBoxStyle-CssClass="TextDate" ShowNoneButton="False">
                            <TextBoxStyle CssClass="TextDate" />
                        </BDP:BasicDatePicker>
                    </td>
                </tr>
                <tr>
                    <td>Operator</td>
                    <td>:</td>
                    <td>
                        <asp:TextBox runat="server" ValidationGroup="Input" ID="tbOperator" MaxLength="255" CssClass="TextBox" Width="150px" />
                    </td>
                    <td>Remark</td>
                    <td>:</td>
                    <td>
                        <asp:TextBox runat="server" ValidationGroup="Input" ID="tbRemark" CssClass="TextBox" Width="225px" />
                    </td>
                </tr>
            </table>
            <br />
            <hr style="color: Blue" />
            <asp:Menu ID="Menu1" runat="server" CssClass="Menu" StaticMenuItemStyle-CssClass="MenuItem"
                StaticSelectedStyle-CssClass="MenuSelect" Orientation="Horizontal" ItemWrap="False"
                StaticEnableDefaultPopOutImage="False">
                <Items>
                    <asp:MenuItem Text="Detail Fixed Asset" Value="0"></asp:MenuItem>
                    <%--<asp:MenuItem Text="Detail FA Location" Value="1"></asp:MenuItem>--%>
                </Items>
            </asp:Menu>
            <br />
            <asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex="0">
                <asp:View ID="Tab1" runat="server">
                    <asp:Panel runat="server" ID="PnlDt">
                        <asp:Button class="bitbtndt btnadd" runat="server" ID="btnAdddt" Text="Add" ValidationGroup="Input" /><br/>&nbsp;
                        <div style="border: 0px  solid; width: 100%; height: 100%; overflow: auto;">
                            <asp:GridView ID="GridDt" runat="server" AutoGenerateColumns="false" ShowFooter="False">
                                <HeaderStyle CssClass="GridHeader" />
                                <RowStyle CssClass="GridItem" Wrap="false" />
                                <AlternatingRowStyle CssClass="GridAltItem" />
                                <PagerStyle CssClass="GridPager" />
                                <Columns>
                                    <asp:TemplateField HeaderText="Action">
                                        <ItemTemplate>
                                            <asp:Button ID="btnEdit" runat="server" class="bitbtndt btnedit" Text="Edit" Width="60px" Height="30px" CommandName="Edit" />
                                            <asp:Button ID="btnDelete" runat="server" class="bitbtndt btndelete" Text="Delete" Width="60px" Height="30px"
                                                OnClientClick="return confirm('Sure to delete this data?');" CommandName="Delete" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:Button ID="btnView" runat="server" class="bitbtndt btndetail" Text="Detail" Width="60px" Height="30px"
                                                CommandName="View" CommandArgument='<%# Container.DataItemIndex %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="FixedAsset" HeaderStyle-Width="100px" HeaderText="Fixed Asset" />
                                    <asp:BoundField DataField="FAName" HeaderStyle-Width="100px" HeaderText="FA Name" />
                                    <asp:BoundField DataField="Specification" HeaderStyle-Width="100px" HeaderText="Specification" />
                                    <asp:BoundField DataField="FAStatusName" HeaderStyle-Width="100px" HeaderText="FA Status" />
                                    <asp:BoundField DataField="FAOwner" HeaderText="FA Owner" />
                                    <asp:BoundField DataField="FA_SubGrp_Name" HeaderStyle-Width="100px" HeaderText="FA SubGroup" />
                                    <asp:BoundField DataField="RRNo" HeaderStyle-Width="100px" HeaderText="RR No" />
                                    <%--<asp:BoundField DataField="Product" HeaderStyle-Width="100px" HeaderText="Product Code" />
                            <asp:BoundField DataField="ProductName" HeaderStyle-Width="100px" HeaderText="Product Name" />--%>
                                    <asp:BoundField DataField="BuyingDate" DataFormatString="{0:dd MMM yyyy}" HtmlEncode="true"
                                        HeaderStyle-Width="80px" HeaderText="Buying Date" />
                                    <asp:BoundField DataField="StartMonthDepr" HeaderStyle-Width="80px" HeaderText="Start Month Depr" ItemStyle-HorizontalAlign="Center" /> 
                                    <asp:BoundField DataField="StartYearDepr" HeaderStyle-Width="50px" HeaderText="Start Year Depr" />
                                    <asp:BoundField DataField="LifeMonth" HeaderStyle-Width="100px" HeaderText="Life Month" ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="LifeProcessDepr" HeaderStyle-Width="100px" HeaderText="Life Process Depr" ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="Qty" HeaderStyle-Width="100px" HeaderText="Qty" />
                                    <asp:BoundField DataField="Unit" HeaderStyle-Width="100px" HeaderText="Unit" />
                                    <asp:BoundField DataField="Currency" HeaderText="Curr" />
                                    <asp:BoundField DataField="PriceForex" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Right"
                                        HeaderText="Price Forex" />
                                    <asp:BoundField DataField="AmountForex" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Right"
                                        HeaderText="Amount" />
                                    <asp:BoundField DataField="AmountHome" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Right"
                                        HeaderText="Amount Home" />
                                    <asp:BoundField DataField="AmountProcessDepr" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Right"
                                        HeaderText="Amount Process Depr" />
                                    <asp:BoundField DataField="Cost_Ctr_Name" HeaderStyle-Width="100px" HeaderText="Cost Center" />
                                </Columns>
                            </asp:GridView>
                        </div> <br/>&nbsp;
                        &nbsp &nbsp &nbsp
                        <asp:Button class="bitbtndt btnadd" runat="server" ID="btnAddDtke2" Text="Add" ValidationGroup="Input" />
                    </asp:Panel>
                    <asp:Panel runat="server" ID="pnlEditDt" Visible="false">
                        <table>
                            <tr>
                                <td class="style2">Fixed Asset</td>
                                <td>:</td>
                                <td>
                                    <asp:TextBox CssClass="TextBox" MaxLength="20" ValidationGroup="Input" runat="server"
                                        ID="tbFA" Width="125px" />
                                    <asp:TextBox CssClass="TextBox" runat="server" ID="tbFAName" Width="245px" MaxLength="100" />
                                </td>
                            </tr>
                            <%--<tr>                    
                        <td class="style2">FA Name</td>
                        <td>:</td>
                        <td colspan="4">                                
                            
                        </td>
                    </tr>--%>
                            <tr>
                                <td class="style2">Specification</td>
                                <td>:</td>
                                <td>
                                    <asp:TextBox ID="tbSpecFA" runat="server" CssClass="TextBox" MaxLength="255" TextMode="MultiLine"
                                        Width="380px" />
                                </td>
                            </tr>
                            <tr>
                                <td class="style2">FA Status</td>
                                <td>:</td>
                                <td>
                                    <asp:DropDownList ID="ddlFAStatus" runat="server" CssClass="DropDownList" Width="133px" />
                                </td>
                                <td class="style2">FA Owner</td>
                                <td>:</td>
                                <td>
                                    <asp:DropDownList ID="ddlFAOwner" runat="server" CssClass="DropDownList" ValidationGroup="Input">
                                        <asp:ListItem Selected="True">Y</asp:ListItem>
                                        <asp:ListItem>N</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                        </table>
                        <asp:Panel runat="server" ID="PnlRRNoProduct">
                            <table>
                                <tr>
                                    <td class="style2">RR No</td>
                                    <td>:</td>
                                    <td>
                                        <asp:TextBox ID="tbRRNo" runat="server" CssClass="TextBox" Enabled="false" Width="125px" />
                                        <asp:Button class="btngo" runat="server" ID="btnRRNo" Text="..." ValidationGroup="Input" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style2">Product</td>
                                    <td>:</td>
                                    <td>
                                        <asp:TextBox ID="tbProductCode" runat="server" CssClass="TextBox" Enabled="true"
                                            Width="125px" />
                                        <asp:TextBox ID="tbProductName" runat="server" CssClass="TextBox" Enabled="true"
                                            Width="245px" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <table>
                            <tr>
                                <td class="style2">FA Sub Group</td>
                                <td>:</td>
                                <td colspan="4">
                                    <asp:TextBox ID="tbFASubGroup" runat="server" AutoPostBack="true" CssClass="TextBox"
                                        ValidationGroup="Input" Width="125px" />
                                    <asp:TextBox ID="tbFASubGroupName" runat="server" CssClass="TextBox" Enabled="false"
                                        Width="225px" />
                                    <asp:Button class="btngo" runat="server" ID="btnFASubGroup" Text="..." ValidationGroup="Input" />
                                    &nbsp &nbsp
                                    <asp:Label runat="server" ID="lbFA" Text="Max FA : "></asp:Label>
                                    <asp:TextBox runat="server" ID="tbFAMax" CssClass="TextBox" Enabled="false" Width="120px"
                                        Text=""></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="style2">Cost Center</td>                                
                                <td>:</td>
                                <td>
                                    <asp:DropDownList ID="ddlCostCenterDt" runat="server" CssClass="DropDownList" Width="133px" />
                                </td>
                                <td class="style2">Buying Date</td>
                                <td>:</td>
                                <td>
                                    <BDP:BasicDatePicker ID="tbBuyDate" runat="server" ButtonImageHeight="19px" AutoPostBack="true"
                                        ButtonImageWidth="20px" DateFormat="dd MMM yyyy" DisplayType="TextBoxAndImage"
                                        ReadOnly="true" ShowNoneButton="False" TextBoxStyle-CssClass="TextDate" ValidationGroup="Input">
                                        <TextBoxStyle CssClass="TextDate" />
                                    </BDP:BasicDatePicker>
                                </td>
                            </tr>
                            <tr>
                                <td class="style2">Currency</td>
                                <td>:</td>
                                <td>
                                    <asp:DropDownList ID="ddlCurrDt" runat="server" AutoPostBack="true" CssClass="DropDownList"
                                        Width="60px" />
                                    <asp:TextBox ID="tbRateDt" runat="server" CssClass="TextBox" Width="65px" />
                                </td>
                                <td class="style2">Start Depr</td>
                                <td>:</td>
                                <td>
                                    <asp:DropDownList CssClass="DropDownList" ID="ddlDepMonth" runat="server" />
                                    <asp:DropDownList CssClass="DropDownList" ID="ddlDepyear" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td class="style2">Buying</td>
                                <td>:</td>
                                <td colspan="4">
                                    <table>
                                        <tr>
                                            <td style="text-align: center; background-color: Gray">
                                                Qty
                                            </td>
                                            <td style="text-align: center; background-color: Gray">
                                                <asp:Label runat="server" ID="lbUnit" Text="Unit" />
                                            </td>
                                            <td style="text-align: center; background-color: Gray">
                                                Unit Price
                                            </td>
                                            <td style="text-align: center; background-color: Gray">
                                                Total Value
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="tbQty" runat="server" CssClass="TextBox" />
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlUnit" runat="server" CssClass="DropDownList" ValidationGroup="Input">
                                                </asp:DropDownList>
                                            </td>
                                            <td class="style1">
                                                <asp:TextBox ID="tbPriceForex" runat="server" CssClass="TextBox" AutoPostBack="true" />
                                            </td>
                                            <td>
                                                <asp:TextBox ID="tbAmountForexDt" runat="server" CssClass="TextBox" Enabled="false" />
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                                <td colspan="4" rowspan="3">
                                    <table>
                                        <tr>
                                            <td class="style1" style="text-align: center; background-color: Gray">
                                                Life (Month)
                                            </td>
                                            <td style="text-align: center; background-color: Gray">
                                                Amount
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="style1">
                                                <asp:TextBox ID="tbLifeMonth" runat="server" CssClass="TextBoxR" />
                                            </td>
                                            <td>
                                                <asp:TextBox ID="tbAmountHomeDt" runat="server" CssClass="TextBoxR" />
                                            </td>
                                        </tr>
                                    </table>
                                   <asp:Panel runat="server" ID="pnlDepresiasi">
                                     <table>
                                        <tr>
                                            <td class="style1">
                                                <asp:TextBox ID="tbLifeProcessDepr" runat="server" CssClass="TextBox" />
                                            </td>
                                            <td>
                                                <asp:TextBox ID="tbAmountProcessDepr" runat="server" CssClass="TextBox" />
                                            </td>
                                        </tr>
                                     </table>  
                                   </asp:Panel>
                                </td>
                            </tr>
                            <tr>
                                <td>Value</td>
                                <td>:</td>
                            </tr>
                            <asp:Panel runat="server" ID="pnllblDepresiasi">
                              <tr>
                                <td>
                                    <asp:Label runat="server" ID="lbProcessDepr" Text="Depreciation"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="lbProcessDeprOpr" Text=":"></asp:Label>
                                </td>                                
                              </tr>
                            </asp:Panel>
                        </table>
                        <br />
                        <asp:Button class="bitbtndt btnsave" runat="server" ID="btnSaveDt" Text="Save" />
                        <asp:Button class="bitbtndt btncancel" runat="server" ID="btnCancelDt" Text="Cancel" />
                    </asp:Panel>
                </asp:View>
                <asp:View ID="Tab2" runat="server">
                    <table>
                        <tr>
                            <td>Fixed Asset</td>
                            <td>:</td>
                            <td>
                                <asp:Label ID="lbFADt2" runat="server" Text="Fixed Asset" />
                                &nbsp
                                <asp:Label ID="lbFANameDt2" runat="server" Text="Fixed Asset Name" />
                            </td>
                            <td align="right">Cost Center</td>
                            <td>:</td>
                            <td>
                                <asp:Label ID="lbCostCtr" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>Status FA/ FA Owner</td>
                            <td>:</td>
                            <td>
                                <asp:Label ID="lbStatusFA" runat="server" />
                                &nbsp;/
                                <asp:Label ID="lbFAOwner" runat="server" />
                            </td>
                            <td>&nbsp;Buying Date</td>
                            <td>:</td>
                            <td>&nbsp;<asp:Label ID="lbBuyingDate" runat="server" /></td>
                        </tr>
                        <tr>
                            <td>FA Sub Group</td>
                            <td>:</td>
                            <td>
                                <asp:Label ID="lbFASubGroup" runat="server" Text="lbFASubGroup" />
                            </td>
                            <td>&nbsp;Currency</td>
                            <td>:</td>
                            <td><asp:Label ID="lbCurr" runat="server" />&nbsp;</td>
                        </tr>
                         <tr>
                            <td>Qty Asset</td>
                            <td>:</td>
                            <td>
                                <asp:Label ID="lblQty" runat="server"  Text="0" />
                                &nbsp; 
                                <asp:Label ID="lblUnit" runat="server" />
                            </td>
                            
                        </tr>
                        
                    </table>
                    <hr style="color: Blue" />
                    <asp:Panel ID="pnlDt2" runat="server">
                        <br />
                        <asp:Button class="bitbtndt btnadd" runat="server" ID="btnAddDt2" Text="Add" ValidationGroup="Input" />
                        <asp:Button class="bitbtndt btnback" runat="server" ID="btnBackDt2ke1" Text="Back" /> <br/>&nbsp;
                        <div style="border: 0px  solid; width: 100%; height: 100%; overflow: auto;">
                            <asp:GridView ID="GridDt2" runat="server" AutoGenerateColumns="false" ShowFooter="False">
                                <HeaderStyle CssClass="GridHeader" />
                                <RowStyle CssClass="GridItem" Wrap="false" />
                                <AlternatingRowStyle CssClass="GridAltItem" />
                                <PagerStyle CssClass="GridPager" />
                                <Columns>
                                    <asp:TemplateField HeaderStyle-Width="110" HeaderText="Action" >
                                        <ItemTemplate>
                                            <asp:Button class="bitbtndt btnedit" runat="server" ID="btnEdit" Text="Edit" Width="50px" Height="30px" CommandName="Edit" />
                                            <asp:Button class="bitbtndt btndelete" runat="server" ID="btnDelete" Text="Delete" Width="60px" Height="30px"  
                                                CommandName="Delete" OnClientClick="return confirm('Sure to delete this data?');" />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:Button class="bitbtndt btnsave" runat="server" ID="btnUpdate" Text="Save" Width="50px" Height="30px" CommandName="Update" />
                                            <asp:Button class="bitbtndt btnsave" runat="server" ID="btnCancel" Text="Cancel" Width="60px" Height="30px"  
                                                CommandName="Cancel" />
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="FALocationType" HeaderStyle-Width="150px" HeaderText="FA Location Type" />
                                    <asp:BoundField DataField="FALocationCode" HeaderStyle-Width="90px" HeaderText="FA Location Code" />
                                    <asp:BoundField DataField="FA_Location_Name" HeaderStyle-Width="150px" HeaderText="FA Location Name" />
                                    <asp:BoundField DataField="Qty" DataFormatString="{0:#,##0.00}" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Right" HeaderText="Qty" />
                                    <asp:BoundField DataField="Remark" HeaderStyle-Width="150px" HeaderText="Remark" />
                                </Columns>
                            </asp:GridView>
                        </div> <br/>&nbsp;
                        <asp:Button class="bitbtndt btnadd" runat="server" ID="btnAddDt2Ke2" Text="Add" ValidationGroup="Input" />
                        <asp:Button class="bitbtndt btnback" runat="server" ID="btnBackDt2ke2" Text="Back" />
                    </asp:Panel>
                    <asp:Panel runat="server" ID="pnlEditDt2" Visible="false">
                        <table>
                            <tr>
                                <td>FA Location</td>
                                <td>:</td>
                                <td>
                                    <asp:DropDownList CssClass="DropDownList" ID="ddlFALocType" runat="server" AutoPostBack="true">
                                        <asp:ListItem Selected="True">GENERAL</asp:ListItem>
                                        <asp:ListItem>CUSTOMER</asp:ListItem>
                                        <asp:ListItem>SUPPLIER</asp:ListItem>
                                        <asp:ListItem>EMPLOYEE</asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:TextBox CssClass="TextBox" ValidationGroup="Input" runat="server" ID="tbFALocCode"
                                        AutoPostBack="true" />
                                    <asp:TextBox CssClass="TextBox" runat="server" ID="tbFALocName" Enabled="false" Width="225px" />
                                    <asp:Button class="btngo" runat="server" ID="btnFALoc" Text="..." ValidationGroup="Input" />
                                </td>
                            </tr>
                            <tr>
                                <td>Qty</td>
                                <td>:</td>
                                <td><asp:TextBox CssClass="TextBox" runat="server" ID="tbQtyDt2" /></td>
                            </tr>
                            <tr>
                                <td>Remark</td>
                                <td>:</td>
                                <td>
                                    <asp:TextBox runat="server" ID="tbRemarkDt2" CssClass="TextBox" Width="365px" MaxLength="255"
                                        TextMode="MultiLine" />
                                </td>
                            </tr>
                        </table>
                        <br />
                        <asp:Button class="bitbtndt btnsave" runat="server" ID="btnSaveDt2" Text="Save" />
                        <asp:Button class="bitbtndt btncancel" runat="server" ID="btnCancelDt2" Text="Cancel" />
                    </asp:Panel>
                </asp:View>
            </asp:MultiView>
            <br />
            <asp:Button class="bitbtndt btnsavenew" runat="server" ID="btnSaveAll" Text="Save & New"
                ValidationGroup="Input" Width="100px" />
            <asp:Button class="bitbtndt btnsave" runat="server" ID="btnSaveTrans" Text="Save"
                ValidationGroup="Input" />
            <asp:Button class="bitbtndt btnback " runat="server" ID="btnBack" Text="Cancel" ValidationGroup="Input" />
            <asp:Button class="bitbtndt btnback" runat="server" ID="btnHome" Text="Home" />
        </asp:Panel>
    </div>
    <asp:Label runat="server" ID="lbStatus" ForeColor="Red" />
    
        <div class="loading" align="center">
      <br />
       <img src="../../Image/loader.gif" alt="" />
    </div>
    </form>
</body>
</html>
