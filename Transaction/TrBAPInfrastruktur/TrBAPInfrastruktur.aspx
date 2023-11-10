<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TrBAPInfrastruktur.aspx.vb" Inherits="BAPInfrastruktur" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<%@ Register Assembly="BasicFrame.WebControls.BasicDatePicker" Namespace="BasicFrame.WebControls"
    TagPrefix="BDP" %>

<%@ Register assembly="CrystalDecisions.Web, Version=10.5.3700.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>BAP Infrastruktur</title>
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

              var bapValue = document.getElementById("tbBAPPersen").value
              
              if (bapValue == 0) {
              document.getElementById("tbBAP").value = 0
              document.getElementById("tbBAPnowPersen").value = 0
              document.getElementById("tbBAPnow").value = 0

              document.getElementById("tbSisaBAP").value = (parseFloat(document.getElementById("tbBiaya").value.replace(/\$|\,/g, "")) - parseFloat(document.getElementById("tbBAPSebelum").value.replace(/\$|\,/g, "")));

              
              } else{
              
              document.getElementById("tbBAP").value = parseFloat(document.getElementById("tbBiaya").value.replace(/\$|\,/g, "")) * parseFloat(document.getElementById("tbBAPPersen").value.replace(/\$|\,/g, "")) / 100;
              document.getElementById("tbBAPnowPersen").value = (parseFloat(document.getElementById("tbBAPPersen").value.replace(/\$|\,/g, "")) - parseFloat(document.getElementById("tbBAPSebelumPersen").value.replace(/\$|\,/g, ""))) ;
              document.getElementById("tbBAPnow").value = (parseFloat(document.getElementById("tbBAP").value.replace(/\$|\,/g, "")) - parseFloat(document.getElementById("tbBAPSebelum").value.replace(/\$|\,/g, "")));

              document.getElementById("tbSisaBAP").value = (parseFloat(document.getElementById("tbBiaya").value.replace(/\$|\,/g, "")) - parseFloat(document.getElementById("tbBAP").value.replace(/\$|\,/g, "")));

              }
               
              
              document.getElementById("tbBAP").value = setdigit(document.getElementById("tbBAP").value.replace(/\$|\,/g, ""), '<%=ViewState("DigitCurr")%>');
              document.getElementById("tbBAPnowPersen").value = setdigit(document.getElementById("tbBAPnowPersen").value.replace(/\$|\,/g, ""), '<%=ViewState("DigitCurr")%>');
              document.getElementById("tbBAPnow").value = setdigit(document.getElementById("tbBAPnow").value.replace(/\$|\,/g, ""), '<%=ViewState("DigitCurr")%>');
              document.getElementById("tbBAPPersen").value = setdigit(document.getElementById("tbBAPPersen").value.replace(/\$|\,/g, ""), '<%=ViewState("DigitCurr")%>');
              document.getElementById("tbBAPSebelumPersen").value = setdigit(document.getElementById("tbBAPSebelumPersen").value.replace(/\$|\,/g, ""), '<%=ViewState("DigitCurr")%>');
              document.getElementById("tbBAPSebelum").value = setdigit(document.getElementById("tbBAPSebelum").value.replace(/\$|\,/g, ""), '<%=ViewState("DigitCurr")%>');
              document.getElementById("tbBiaya").value = setdigit(document.getElementById("tbBiaya").value.replace(/\$|\,/g, ""), '<%=ViewState("DigitCurr")%>');

              document.getElementById("tbSisaBAP").value = setdigit(document.getElementById("tbSisaBAP").value.replace(/\$|\,/g, ""), '<%=ViewState("DigitCurr")%>');
  

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
    <div class="H1">BAP Infrastruktur</div>
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
                      <asp:ListItem Value="SuppCode">Kontraktor</asp:ListItem>
                      <asp:ListItem Value="Supplier_Name">Kontraktor Name</asp:ListItem>
                      <asp:ListItem Value="No_SPK">No SPK</asp:ListItem>
                      <asp:ListItem>Remark</asp:ListItem>
                  </asp:DropDownList>
                  <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlRange"></asp:DropDownList>

                  <asp:Button class="bitbtn btnsearch" runat="server" ID="btnSearch" Text="Search" />																						 											  
                  <asp:Button class="bitbtn btngo" runat="server" ID="btnExpand" Text="..." />																						 											  
                 
                 
                
            </td>
            <td>
                <asp:LinkButton ID="LbAdvSearch" runat="server" Text="Advanced Search" />
                 &nbsp &nbsp &nbsp &nbsp
                <asp:Label runat="server" ID="Label1" Text="Outstanding SPK: "/>
                <asp:LinkButton runat="server" ID="lbCount" Text="X"  Font-Size="Small" />
                <asp:Label runat="server" ID="Label2" Text=" record(s)"/>
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
                  <asp:ListItem Selected="True" Value="TransNmbr">Reference</asp:ListItem>
                  <asp:ListItem>Status</asp:ListItem>
                  <asp:ListItem Value="dbo.FormatDate(TransDate)">Date</asp:ListItem>
                      <asp:ListItem Value="TransNmbr" Selected="True">Reference</asp:ListItem>
                      <asp:ListItem>Status</asp:ListItem>
                      <asp:ListItem Value="dbo.FormatDate(TransDate)">Date</asp:ListItem>
                      <asp:ListItem Value="SuppCode">Kontraktor</asp:ListItem>
                      <asp:ListItem Value="Supplier_Name">Kontraktor Name</asp:ListItem>
                      <asp:ListItem Value="No_SPK">No SPK</asp:ListItem>
                      <asp:ListItem>Remark</asp:ListItem>
                   
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
                              <asp:ListItem Text="Print" />
                          </asp:DropDownList>
                          
                          <asp:Button class="btngo" runat="server" ID="BtnGo" Text="G" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" CommandName="Go" />
                          
                      </ItemTemplate>
                      <HeaderStyle Width="110px" />
                  </asp:TemplateField>
                  <asp:BoundField DataField="TransNmbr" HeaderStyle-Width="120px" SortExpression="Nmbr" HeaderText="Reference"></asp:BoundField>                  
                  <asp:BoundField DataField="Status" HeaderText="Status"></asp:BoundField>                  
                  <asp:BoundField DataField="TransDate" dataformatstring="{0:dd MMM yyyy}" htmlencode="true" HeaderStyle-Width="80px" SortExpression="TransDate" HeaderText="Date"></asp:BoundField>
                  <asp:BoundField DataField="Supplier_Name" HeaderStyle-Width="150px" SortExpression="Supplier_Name" HeaderText="Supplier"></asp:BoundField>
                  <asp:BoundField DataField="No_SPK" HeaderStyle-Width="100px" SortExpression="No_SPK" HeaderText="No SPK"></asp:BoundField>
                  <asp:BoundField DataField="TotalBAP" DataFormatString="{0:#,##0.00}" ItemStyle-HorizontalAlign="Right"  HeaderStyle-Width="80px" SortExpression="TotalBAP" HeaderText="Total BAP"></asp:BoundField>
                  <asp:BoundField DataField="Remark" HeaderStyle-Width="120px" SortExpression="Remark" HeaderText="Remark"></asp:BoundField>
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
            <td>No BAP</td>
            <td>:</td>
            <td><asp:TextBox CssClass="TextBoxR" runat="server" ID="tbRef" Width="225px" Enabled="False"/>        
            </td>     
        </tr>
        
        
        <tr>
        <td>BAP Date</td>
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
       
             <td>No SPK</td>
            <td>:</td>
            <td><asp:TextBox runat="server" ValidationGroup="Input" ID="tbNoSPK" CssClass="TextBox" Width="225px"/>
            <asp:Button Class="btngo" ID="BtnNoSPK" Text="..." runat="server" ValidationGroup="Input" /></td>
                    
        </tr>
 
        <tr>
            <td>Kontraktor<asp:LinkButton ID="lbSupp" Visible="false" ValidationGroup="Input"  runat="server" Text="Supplier"/></td>
            <td>:</td>
            <td>
                <asp:TextBox CssClass="TextBox" ValidationGroup="Input" runat="server" ID="tbSuppCode" Width="100px" AutoPostBack="true" />
                <asp:TextBox CssClass="TextBox" runat="server" ID="tbSuppName" Enabled="false" Width="190px"/>
                <%--<asp:TextBox CssClass="TextBox" runat="server" ID="tbSuppType" Visible="false" Enabled="false" Width="190px"/>--%>
                <asp:Button Class="btngo" ID="btnSupp" Text="..." runat="server" ValidationGroup="Input" />                                  
            </td>            
        </tr>
        
        
        <tr>
       
             <td>Paket Pekerjaan</td>
            <td>:</td>
            <td><asp:TextBox runat="server" ValidationGroup="Input" MaxLength="225" TextMode="MultiLine" ID="tbPaketPekerjaan" CssClass="TextBox" Width="225px"/>
                    
        </tr>

        
                    <tr>
                            <td>Total Nilai Project</td>
                            <td>:</td>
                            <td colspan="7">
                                <table>
                                    <tr style="background-color:Silver;text-align:center; border-radius:30px;">
                                        <td>Total Nilai Project</td>                                        
                                        <td>Total BAP s/d Saat ini </td>
                                        <td>Total Sebelumnya</td>
                                        <td>Total Bayar BAP</td>
                                        <td>Total Sisa BAP</td>
                                    </tr>
                                    
                                    <tr>
                                        <td><asp:TextBox ID="tbTotNilai" ValidationGroup="Input" runat="server" Enabled = "False" CssClass="TextBox" width="120px"/></td>
                                        <td><asp:TextBox ID="tbBAPsdSaatIni"  ValidationGroup="Input" runat="server" Enabled = "False" CssClass="TextBox" Width="120px"/></td>
                                         <td><asp:TextBox ID="tbBAPSebelumnya"  ValidationGroup="Input" runat="server" Enabled = "false" CssClass="TextBox" Width="120px"/></td>  
                                        <td><asp:TextBox ID="tbTotBayarBAP"  ValidationGroup="Input" runat="server" Enabled = "False" CssClass="TextBox" Width="120px"/></td>
                                         <td><asp:TextBox ID="tbTotSisaBAP"  ValidationGroup="Input" runat="server" Enabled = "false" CssClass="TextBox" Width="120px"/></td> 
                                        
                                    </tr>
                                </table>
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
                <asp:MenuItem Text="Detail BAP Infrastruktur" Value="1"></asp:MenuItem>                   
            </Items>            
        </asp:Menu>
        <hr /> 
      
        <asp:Panel runat="server" ID="pnlDt">
            <asp:Button class="bitbtn btnadd" runat="server" ID="btnAddDt" Text="Add" ValidationGroup="Input" />
            	
            
            <div style="border:0px  solid; width:100%; height:100%; overflow:auto;">
                <asp:GridView ID="GridDt" runat="server" AutoGenerateColumns="false" 
                    ShowFooter="False"  Wrap="false" >
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
                            <asp:BoundField DataField="UraianPekerjaan" HeaderStyle-Width="250px" HeaderText="Uraian Pekerjaan" />                           
                            <asp:BoundField DataField="Luas"  ItemStyle-HorizontalAlign="Right"  HeaderStyle-Width="100px" DataFormatString="{0:#,##0.00}" HeaderText="Volume" />
                            <asp:BoundField DataField="Biaya"  ItemStyle-HorizontalAlign="Right"  HeaderStyle-Width="100px" DataFormatString="{0:#,##0.00}" HeaderText="Nilai Project" />
                            <asp:BoundField DataField="BAPPersen" HeaderText="%"  HeaderStyle-Width="50px" />
                            <asp:BoundField DataField="BAP"  ItemStyle-HorizontalAlign="Right"  HeaderStyle-Width="100px" DataFormatString="{0:#,##0.00}" HeaderText="BAP s/d Saat Ini" />
                            
                            <asp:BoundField DataField="BAPSebelumPersen" HeaderText="%" HeaderStyle-Width="50px"  />
                            <asp:BoundField DataField="BAPSebelum"  ItemStyle-HorizontalAlign="Right"  HeaderStyle-Width="100px" DataFormatString="{0:#,##0.00}" HeaderText="BAP Sebelumnya" />
                            
                            <asp:BoundField DataField="TagihanBAPPersen" HeaderText="%" HeaderStyle-Width="50px"/>
                            <asp:BoundField DataField="TagihanBAP"  ItemStyle-HorizontalAlign="Right"  HeaderStyle-Width="100px" DataFormatString="{0:#,##0.00}" HeaderText="BAP Saat Ini" />
                            <asp:BoundField DataField="SisaBAP"  ItemStyle-HorizontalAlign="Right"  HeaderStyle-Width="100px" DataFormatString="{0:#,##0.00}" HeaderText="Sisa BAP" />
                            
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
                        <td>Uraian Pekerjaan</td>
                        <td>:</td>
                        <td>                             
                            <asp:TextBox CssClass="TextBox" runat="server" ID="tbUraian" Enabled="true" Width="225px"/>                                          
                        </td>
                    </tr>
                    
                    
                    <tr>
                            <td>Nilai Project</td>
                            <td>:</td>
                            <td colspan="7">
                                <table>
                                    <tr style="background-color:Silver;text-align:center; border-radius:30px;">
                                        <td>Volume</td>
                                        <td>Nilai Project</td>
                                    </tr>
                                    <tr>

                                        <td><asp:TextBox ID="tbLuas"  ValidationGroup="Input" runat="server" Enabled = "True" CssClass="TextBox" Width="50px"/></td>                                         
                                        <td><asp:TextBox ID="tbBiaya"  ValidationGroup="Input" runat="server"  CssClass="TextBox" Width="120px"/></td>                          
                                    </tr>
                                </table>
                            </td>                
                    </tr>
                    
                    <tr>
                            <td>Nilai BAP</td>
                            <td>:</td>
                            <td colspan="7">
                                <table>
                                    <tr style="background-color:Silver;text-align:center; border-radius:30px;">
                                        <td>%</td>
                                        <td>BAP s/d Saat Ini</td>
                                        <td>%</td>
                                        <td>BAP Sebelumnya</td>                                        
                                        <td>%</td>
                                        <td>BAP Saat Ini</td>
                                        <td>Sisa</td>
                                    </tr>
                                    
                                    
                                    <tr>
                                        <td><asp:TextBox ID="tbBAPPersen"  ValidationGroup="Input" runat="server" Enabled = "True" CssClass="TextBox" Width="50px"/></td>                                         
                                        <td><asp:TextBox ID="tbBAP"  ValidationGroup="Input" runat="server"  CssClass="TextBox" Width="120px"/></td>   
                                        <td><asp:TextBox ID="tbBAPSebelumPersen"  ValidationGroup="Input" runat="server" Enabled = "True" CssClass="TextBox" Width="50px"/></td>                                         
                                        <td><asp:TextBox ID="tbBAPSebelum"  ValidationGroup="Input" runat="server"  CssClass="TextBox" Width="120px"/></td>
                                        
                                        <td><asp:TextBox ID="tbBAPnowPersen"  ValidationGroup="Input" runat="server" Enabled = "True" CssClass="TextBox" Width="50px"/></td>                                         
                                        <td><asp:TextBox ID="tbBAPnow"  ValidationGroup="Input" runat="server"  CssClass="TextBox" Width="120px"/></td> 
                                        
                                        <td><asp:TextBox ID="tbSisaBAP"  ValidationGroup="Input" runat="server"  CssClass="TextBox" Width="120px"/></td>                       
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
