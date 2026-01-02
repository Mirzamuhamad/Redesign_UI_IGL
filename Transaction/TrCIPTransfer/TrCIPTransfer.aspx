<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TrCIPTransfer.aspx.vb" Inherits="TrCIPTransfer" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<%@ Register Assembly="BasicFrame.WebControls.BasicDatePicker" Namespace="BasicFrame.WebControls"
    TagPrefix="BDP" %>

<%@ Register assembly="CrystalDecisions.Web, Version=10.5.3700.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Transfer CIP to Fixed Asset</title>
    <script src="../../Function/OpenDlg.js" type="text/javascript"></script>
    <script src="../../Function/Function.JS" type="text/javascript"></script>
    <link type="text/css" rel="stylesheet" href="../../Styles/circularprogress.css" /> 
    <script type="text/javascript" src="../../JQuery/jquery.min.js"></script>


    
    <script type="text/javascript">

        function myPopup() {
            var left = (screen.width - 370) / 2;
            var top = (screen.height - 800) / 2;
            window.open("../../earchDlgV.Aspx", "", 'toolbar=no, location=no, directories=no, status=no, menubar=no, scrollbars=yes, resizable=yes, copyhistory=no, width=' + 370 + ', height=' + 800 + ', top=' + top + ', left=' + left);
            return false;
        }
                
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
        var PpnRate = document.getElementById("tbPpnRate").value.replace(/\$|\,/g,"");
        var Rate = document.getElementById("tbRate").value.replace(/\$|\,/g,"");
        var PPn = document.getElementById("tbPPN").value.replace(/\$|\,/g,"");        
        var BaseForex = document.getElementById("tbBaseForex").value.replace(/\$|\,/g,"");
        var Disc = document.getElementById("tbDisc").value.replace(/\$|\,/g,"");
        var DiscForex = document.getElementById("tbDiscForex").value.replace(/\$|\,/g,"");
        var PPnForex = document.getElementById("tbPPNForex").value.replace(/\$|\,/g,"");
        var TotalForex = document.getElementById("tbTotalForex").value.replace(/\$|\,/g,"");
        
        document.getElementById("tbPpnRate").value = setdigit(PpnRate,'<%=ViewState("DigitRate")%>');
        document.getElementById("tbRate").value = setdigit(Rate,'<%=ViewState("DigitRate")%>');
        document.getElementById("tbPPN").value = setdigit(PPn,'<%=ViewState("DigitPercent")%>');
        document.getElementById("tbBaseForex").value = setdigit(BaseForex,'<%=VIEWSTATE("DigitCurr")%>');
        document.getElementById("tbDisc").value = setdigit(Disc,'<%=ViewState("DigitPercent")%>');
        document.getElementById("tbDiscForex").value = setdigit(DiscForex,'<%=VIEWSTATE("DigitCurr")%>');
        document.getElementById("tbPPNForex").value = setdigit(PPnForex,'<%=VIEWSTATE("DigitCurr")%>');
        document.getElementById("tbTotalForex").value = setdigit(TotalForex,'<%=VIEWSTATE("DigitCurr")%>');
        }catch (err){
            alert(err.description);
          }
      }


      function setformatfordt(prmchange) {
          try {

               document.getElementById("tbPpnValue").value = (parseFloat(document.getElementById("tbTotalBiaya").value.replace(/\$|\,/g, "")) * parseFloat(document.getElementById("tbPpn").value.replace(/\$|\,/g, ""))) / 100;
              document.getElementById("tbpphValue").value = (parseFloat(document.getElementById("tbTotalBiaya").value.replace(/\$|\,/g, "")) * parseFloat(document.getElementById("tbPph").value.replace(/\$|\,/g, ""))) / 100;
              document.getElementById("tbTotalAmount").value = parseFloat(document.getElementById("tbTotalBiaya").value.replace(/\$|\,/g, "")) + parseFloat(document.getElementById("tbPpnValue").value.replace(/\$|\,/g, "")) - parseFloat(document.getElementById("tbpphValue").value.replace(/\$|\,/g, ""));


              document.getElementById("tbTotalAmount").value = setdigit(document.getElementById("tbTotalAmount").value.replace(/\$|\,/g, ""), '<%=ViewState("DigitCurr")%>');
              document.getElementById("tbPpnValue").value = setdigit(document.getElementById("tbPpnValue").value.replace(/\$|\,/g, ""), '<%=ViewState("DigitCurr")%>');
              document.getElementById("tbpphValue").value = setdigit(document.getElementById("tbpphValue").value.replace(/\$|\,/g, ""), '<%=ViewState("DigitCurr")%>');
              document.getElementById("tbTotalBiaya").value = setdigit(document.getElementById("tbTotalBiaya").value.replace(/\$|\,/g, ""), '<%=ViewState("DigitCurr")%>');
              document.getElementById("tbPpn").value = setdigit(document.getElementById("tbPpn").value.replace(/\$|\,/g, ""), '<%=ViewState("DigitCurr")%>');
              document.getElementById("tbPph").value = setdigit(document.getElementById("tbPph").value.replace(/\$|\,/g, ""), '<%=ViewState("DigitCurr")%>');
              document.getElementById("tbOriginalAmount").value = setdigit(document.getElementById("tbOriginalAmount").value.replace(/\$|\,/g, ""), '<%=ViewState("DigitCurr")%>');
  

          } catch (err) {
              alert(err.description);
          }
      } 

                       
       function setformatdt()
        {
        try
         {         
         var Qty = document.getElementById("tbQty").value.replace(/\$|\,/g,""); 
         var PriceForex = document.getElementById("tbPrice").value.replace(/\$|\,/g,"");
         var AmountForex = document.getElementById("tbAmountForex").value.replace(/\$|\,/g,"");        
        
         document.getElementById("tbQty").value = setdigit(Qty,'<%=ViewState("DigitQty")%>');
         document.getElementById("tbPrice").value = setdigit(PriceForex,'<%=VIEWSTATE("DigitCurr")%>');
         document.getElementById("tbAmountForex").value = setdigit(AmountForex,'<%=VIEWSTATE("DigitCurr")%>');
        }catch (err){
            alert(err.description);
          }
      }

      function setformathd(prmchange) {
          try {


              document.getElementById("tbBiaya").value = (parseFloat(document.getElementById("tbLuas").value.replace(/\$|\,/g, "")) * parseFloat(document.getElementById("tbBiayaSatuan").value.replace(/\$|\,/g, "")));

              document.getElementById("tbBAPValue").value = (parseFloat(document.getElementById("tbBiaya").value.replace(/\$|\,/g, "")) * parseFloat(document.getElementById("tbBAP").value.replace(/\$|\,/g, ""))) / 100;
              document.getElementById("tbSisaBiaya").value = parseFloat(document.getElementById("tbBiaya").value.replace(/\$|\,/g, "")) - parseFloat(document.getElementById("tbBAPValue").value.replace(/\$|\,/g, ""));

 





              document.getElementById("tbLuas").value = setdigit(document.getElementById("tbLuas").value.replace(/\$|\,/g, ""), '<%=ViewState("DigitHome")%>');

              document.getElementById("tbBiayaSatuan").value = setdigit(document.getElementById("tbBiayaSatuan").value.replace(/\$|\,/g, ""), '<%=ViewState("DigitHome")%>');

              document.getElementById("tbBiaya").value = setdigit(document.getElementById("tbBiaya").value.replace(/\$|\,/g, ""), '<%=ViewState("DigitHome")%>');

              document.getElementById("tbBAP").value = setdigit(document.getElementById("tbBAP").value.replace(/\$|\,/g, ""), '<%=ViewState("DigitHome")%>');

              document.getElementById("tbBAPValue").value = setdigit(document.getElementById("tbBAPValue").value.replace(/\$|\,/g, ""), '<%=ViewState("DigitHome")%>');

              document.getElementById("tbSisaBiaya").value = setdigit(document.getElementById("tbSisaBiaya").value.replace(/\$|\,/g, ""), '<%=ViewState("DigitHome")%>');


          } catch (err) {
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
 <link href="https://fonts.googleapis.com/css2?family=Muli:wght@300;400;600&display=swap" rel="stylesheet">
</head>
<body>     
    <form id="form1" runat="server">    
    <div class="Content">
    <div class="H1">Transfer CIP to Fixed Asset</div>
     <hr />        
     <asp:Panel runat="server" id="PnlHd"> 
      <table>
        <tr>
            <td style="width:100px;text-align:right">Quick Search :</td>            
            <td><asp:TextBox CssClass="TextBox" runat="server" ID ="tbFilter"/> 
                  <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlField" >
                      <asp:ListItem Value="TransNmbr" Selected="True">Reference</asp:ListItem>
                      <asp:ListItem>Status</asp:ListItem>
                      <asp:ListItem Value="dbo.FormatDate(TransDate)">Date</asp:ListItem>
                      <asp:ListItem Value="AreaName">Area</asp:ListItem>
                      <asp:ListItem Value="KavlingName">Kavling</asp:ListItem> 
                      <asp:ListItem>Remark</asp:ListItem>
                  </asp:DropDownList>
                  <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlRange"></asp:DropDownList>

                  <asp:Button class="bitbtn btnsearch" runat="server" ID="btnSearch" Text="Search" />																						 											  
                  <asp:Button class="bitbtn btngo" runat="server" ID="btnExpand" Text="..." />																						 											  
                 
                 
                
            </td>
            <td>
                <asp:LinkButton ID="LbAdvSearch" runat="server" Text="Advanced Search" />
                 &nbsp &nbsp &nbsp
                <%--<asp:Label runat="server" ID="Label1" Text="Outstanding Penawaran : "/>--%>
                <asp:LinkButton runat="server" Visible = "false" ID="lbCount" Text="X"  Font-Size="Small" />
               <%-- <asp:Label runat="server" ID="Label2" Text=" record(s)"/>--%>
              
                        <asp:DropDownList CssClass="DropDownList" Visible = "false" runat="server" ID="ddlShowRecord" AutoPostBack="true">
                            <asp:ListItem Selected="True" Value="15">Choose One</asp:ListItem>
                            <asp:ListItem Value="20">20</asp:ListItem>
                            <asp:ListItem Value="30">30</asp:ListItem>
                            <asp:ListItem Value="40">40</asp:ListItem>
                            <asp:ListItem Value="50">50</asp:ListItem>
                            <asp:ListItem Value="100">100</asp:ListItem>
                        </asp:DropDownList>
                        
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
                      <asp:ListItem Value="TransNmbr" Selected="True">Reference</asp:ListItem>
                      <asp:ListItem>Status</asp:ListItem>
                      <asp:ListItem Value="dbo.FormatDate(TransDate)">Date</asp:ListItem>
                      <asp:ListItem Value="AreaName">Area</asp:ListItem>
                      <asp:ListItem Value="KavlingName">Kavling</asp:ListItem> 
                      <asp:ListItem>Remark</asp:ListItem>
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
          <div style="border:0px  solid; width:100%; height:100%; overflow:auto;">
          <asp:GridView ID="GridView1" runat="server" AllowPaging="True" AllowSorting="true"
            CssClass="Grid" AutoGenerateColumns="false"> 
              <HeaderStyle CssClass="GridHeader" Wrap="false"></HeaderStyle>
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
                              <%--<asp:ListItem Text="Print" />--%>
                          </asp:DropDownList>                          
                          <asp:Button class="btngo" runat="server" ID="BtnGo" Text="G" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" CommandName="Go" />
                          
                      </ItemTemplate>
                      <HeaderStyle Width="110px" />
                  </asp:TemplateField>
                  <asp:BoundField DataField="TransNmbr" HeaderStyle-Width="150px" SortExpression="Nmbr" HeaderText="Reference"></asp:BoundField>                  
                  <asp:BoundField DataField="Status" ItemStyle-HorizontalAlign="Center" HeaderText="Status"></asp:BoundField>                  
                  <asp:BoundField DataField="TransDate" ItemStyle-HorizontalAlign="Center" dataformatstring="{0:dd MMM yyyy}" htmlencode="true" HeaderStyle-Width="200px" SortExpression="TransDate" HeaderText="Date"></asp:BoundField>
                  <asp:BoundField DataField="Remark" HeaderStyle-Width="400px" SortExpression="Remark" HeaderText="Remark"></asp:BoundField>
                  </Columns>
          </asp:GridView>
          </div>
            <asp:Panel runat="server" ID ="pnlNav" Visible="false">
            <asp:Button class="bitbtn btnadd" runat="server" ID="btnAdd2" Text="Add" />	
            
            &nbsp &nbsp &nbsp
            <asp:DropDownList ID="ddlCommand2" CssClass="DropDownList" runat="server"/>
            
            <asp:Button class="btngo" runat="server" ID="btnGo2" Text="G" />                      
          </asp:Panel> 
     </asp:Panel>        
     
 
    <asp:Panel runat="server" ID="pnlInput" Visible="false">
      <table>
        <tr>
            <td>No Transaksi</td>
            <td>:</td>
            <td><asp:TextBox CssClass="TextBoxR" runat="server" ID="tbRef" Width="225px" Enabled="False"/>        
            </td>    
        </tr>
        
        <tr>
        <td>Laporan Date</td>
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
            <td>Remark</td>
            <td>:</td>
            <td><asp:TextBox runat="server" ValidationGroup="Input" ID="tbRemark" CssClass="TextBoxMulti" Width="400px" TextMode="MultiLine" MaxLength="255"/></td>
            
        </tr>
      </table> 
      
      <br />
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
                <%-- <asp:MenuItem Text="Detail Invoice" Value="0"></asp:MenuItem> --%>
                <asp:MenuItem Text="Detail" Value="1"></asp:MenuItem>                   
            </Items>            
      </asp:Menu>
      
              <hr /> 
      
        <asp:Panel runat="server" ID="pnlDt">
            <asp:Button class="bitbtn btnadd" runat="server" ID="btnAddDt" Text="Add" ValidationGroup="Input" />	

            <div style="border:0px  solid; width:100%; height:100%; overflow:auto;">
                <asp:GridView ID="GridDt" runat="server" AutoGenerateColumns="false" 
                    ShowFooter="False"  Wrap="true">
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
                            
                            <EditItemTemplate>
                            	<asp:Button ID="btnUpdate" runat="server" class="bitbtndt btnsave" Text="Save" CommandName="Update"/>									
                                <asp:Button ID="btnCancel" runat="server" class="bitbtndt btncancel" Text="Cancel" CommandName="Cancel"/>									
                                
                            </EditItemTemplate>
                         
                        </asp:TemplateField>
                            <asp:BoundField DataField="ItemNo" HeaderText="No Item" />
                            <asp:BoundField DataField="KavlingName" HeaderStyle-Width="250px" HeaderText="Kavling Name" />                           
                            <asp:BoundField DataField="AreaName" HeaderStyle-Width="250px" HeaderText="Area Name" /> 
                            <asp:BoundField DataField="Qty" HeaderStyle-Width="80px" HeaderText="Qty" />
                            <asp:BoundField DataField="Luas"  ItemStyle-HorizontalAlign="Right"  HeaderStyle-Width="90px" DataFormatString="{0:#,##0.00}" HeaderText="Luas" />
                            <asp:BoundField DataField="Amount"  ItemStyle-HorizontalAlign="Right" Visible = "true" HeaderStyle-Width="90px" DataFormatString="{0:#,##0.00}" HeaderText="Amount" />
                            <asp:BoundField DataField="Remark" HeaderStyle-Width="200px" HeaderText="Remark" />
                    </Columns>
                </asp:GridView>
          </div>   
          <asp:Button class="bitbtn btnadd" runat="server" ID="btnAddDt2" Text="Add" ValidationGroup="Input" />	
          
                
       </asp:Panel>       
       <asp:Panel runat="server" ID="pnlEditDt" Visible="false">
            <table>                                
                
                <tr>
                        <td>Item No</td>
                        <td>:</td>
                        <td><asp:Label ID="lbItemNo" runat="server" Text="Item" />
                        </td>           
                    </tr>    
                    
                    <tr>
                                <td>Area</td>
                                <td>:</td>
                                <td>
                                <asp:TextBox CssClass="TextBox" Width = "225" runat="server" ID="tbArea" Visible = "false"/>
                                <asp:TextBox CssClass="TextBox" Width = "225" runat="server" ID="tbareaName" />                                
                                <asp:Button class="btngo" runat="server" ID="btnArea" Text="..." ValidationGroup="Input" /> 
                                </td>
                            </tr>
                       
                     
                    <tr>
                                <td>Kavling</td>
                                <td>:</td>
                                <td><asp:TextBox CssClass="TextBox" Width = "225" runat="server" ID="tbKavling" Visible = "false"/>
                                <asp:TextBox CssClass="TextBox" Width = "225" runat="server" ID="tbKavlingName" />                             
                                <asp:Button class="btngo" runat="server" ID="btnKavling" Text="..." ValidationGroup="Input" /> 
                                </td>
                            </tr>
                            
                            
                            
                    
                    
                    <tr>
                            <td>Kavling</td>
                            <td>:</td>
                            <td colspan="0">
                                <table>
                                    <tr style="background-color:Silver;text-align:center; border-radius:30px;">
                                        <td>Qty</td>
                                        <td>Luas</td>
                                        <td>Amount</td>
                                    </tr>
                                    <tr>
                                         
                                        <td><asp:TextBox ID="tbQty"  ValidationGroup="Input" runat="server" Enabled = "True" CssClass="TextBox" Width="120px"/></td>
                                        <td><asp:TextBox ID="tbLuas"  ValidationGroup="Input" runat="server"  CssClass="TextBox" Width="120px"/></td>                                         
                                        <td><asp:TextBox ID="tbAmount"  ValidationGroup="Input" runat="server"  CssClass="TextBox" Width="120px"/></td>
                                                                  
                                    </tr>
                                </table>
                            </td>                
                    </tr> 
                    
                    
                    <tr>
                        <td>Remark </td>
                        <td>:</td>
                        <td><asp:TextBox runat="server" ID="tbRemarkDt" CssClass="TextBox" Width="365px" 
                                MaxLength="255" TextMode="MultiLine" />                        
                        </td>
                    </tr>
                                      
            </table>
            <br />    
            
    		<asp:Button ID="btnSaveDt" runat="server" class="bitbtndt btnsave" Text="Save" />									
            <asp:Button ID="btnCancelDt" runat="server" class="bitbtndt btncancel" Text="Cancel" />									
                 
            
       </asp:Panel> 
       <br />   
       
		<asp:Button ID="btnSaveAll" runat="server" class="bitbtndt btnsavenew" Text="Save & New" validationgroup="Input" Width = "90"/>									
        <asp:Button ID="btnSaveTrans" runat="server" class="bitbtndt btnsave" Text="Save" validationgroup="Input"/>									
        <asp:Button ID="btnBack" runat="server" class="bitbtndt btncancel" Text="Cancel" validationgroup="Input"/>									
        <asp:Button ID="btnHome" runat="server" class="bitbtndt btnback" Text="Home" />									                                                           
       
      
    </asp:Panel>
    <asp:Panel runat="server" ID="pnlPrint" Visible="false">
    <cr:crystalreportviewer ID="CrystalReportViewer1" runat="server" AutoDataBind="True"
               Height="1036px" Width="928px" />
    </asp:Panel>
      <br />            
    </div>
   
    <asp:Label runat ="server" ID="lbStatus" ForeColor="Red"/>
    
    <div class="loading" align="center">
      <br />
       <img src="../../Image/loader.gif" alt="" />
    </div>
   
    </form>
    </body>
</html>
