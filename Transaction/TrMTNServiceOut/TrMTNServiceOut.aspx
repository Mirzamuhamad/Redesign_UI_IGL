<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TrMTNServiceOut.aspx.vb" Inherits="TrMTNServiceOut" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<%@ Register Assembly="BasicFrame.WebControls.BasicDatePicker" Namespace="BasicFrame.WebControls"
    TagPrefix="BDP" %>

<%@ Register assembly="CrystalDecisions.Web, Version=10.5.3700.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>BAP Serah Terima Lahan/Kunci</title>
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

        function OpenPopup() {
            var left = (screen.width - 600) / 2; //370
            var top = (screen.height - 600) / 2;
            window.open("../../earchDlgV.Aspx", "", 'toolbar=no, location=no, directories=no, status=no, menubar=no, scrollbars=yes, resizable=yes, copyhistory=no, width=' + 600 + ', height=' + 600 + ', top=' + top + ', left=' + left);
            return false;
        }

        function setformatfordt(prmchange) 
      {
          try 
          {
          
              document.getElementById("tbDiscValue").value = parseFloat(document.getElementById("tbPrice").value.replace(/\$|\,/g, "")) * parseFloat(document.getElementById("tbDisc").value.replace(/\$|\,/g, "")) / 100;

              document.getElementById("tbDpp").value = (parseFloat(document.getElementById("tbPrice").value.replace(/\$|\,/g, "")) - parseFloat(document.getElementById("tbDiscValue").value.replace(/\$|\,/g, "")));
              document.getElementById("tbDPValue").value = (parseFloat(document.getElementById("tbDpp").value.replace(/\$|\,/g, "")) * parseFloat(document.getElementById("tbDP").value.replace(/\$|\,/g, "")) / 100) - parseFloat(document.getElementById("tbTandaJadi").value.replace(/\$|\,/g, ""));

              document.getElementById("tbTotalAmount").value = parseFloat(document.getElementById("tbDpp").value.replace(/\$|\,/g, "")) - parseFloat(document.getElementById("tbDPValue").value.replace(/\$|\,/g, ""));

              
              document.getElementById("tbPrice").value = setdigit(document.getElementById("tbPrice").value.replace(/\$|\,/g, ""), '<%=ViewState("DigitCurr")%>');
              document.getElementById("tbDisc").value = setdigit(document.getElementById("tbDisc").value.replace(/\$|\,/g, ""), '<%=ViewState("DigitCurr")%>');
              document.getElementById("tbDiscValue").value = setdigit(document.getElementById("tbDiscValue").value.replace(/\$|\,/g, ""), '<%=ViewState("DigitCurr")%>');
              document.getElementById("tbDpp").value = setdigit(document.getElementById("tbDpp").value.replace(/\$|\,/g, ""), '<%=ViewState("DigitCurr")%>');
              document.getElementById("tbDP").value = setdigit(document.getElementById("tbDP").value.replace(/\$|\,/g, ""), '<%=ViewState("DigitCurr")%>');
              document.getElementById("tbDPValue").value = setdigit(document.getElementById("tbDPValue").value.replace(/\$|\,/g, ""), '<%=ViewState("DigitCurr")%>');
              document.getElementById("tbTotalAmount").value = setdigit(document.getElementById("tbTotalAmount").value.replace(/\$|\,/g, ""), '<%=ViewState("DigitCurr")%>');

              document.getElementById("tbTandaJadi").value = setdigit(document.getElementById("tbTandaJadi").value.replace(/\$|\,/g, ""), '<%=ViewState("DigitCurr")%>');

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


      function UploadInvoice(fileUploadInvoice) {
          if (fileUploadInvoice.value != '') {
              document.getElementById("<%=btnSaveINV.ClientID %>").click();
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
    <div class="H1">Service Out</div>
     <hr />        
     <asp:Panel runat="server" id="PnlHd"> 
      <table>
        <tr>
            <td style="width:100px;text-align:right">Quick Search :</td>            
            <td><asp:TextBox CssClass="TextBox" runat="server" ID ="tbFilter"/> 
                  <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlField" >
                     <asp:ListItem Value="TransNmbr" Selected="True">No. Service Out</asp:ListItem>
                     <asp:ListItem Value="Status">Status</asp:ListItem>
                     <asp:ListItem Value="dbo.FormatDate(TransDate)">Date</asp:ListItem>
                     <asp:ListItem Value="WOServiceNo">No. WO Service</asp:ListItem>
                     <asp:ListItem Value="SuppName">Nama Supplier</asp:ListItem>
                     <asp:ListItem Value="DeptName">Department</asp:ListItem>
                     <asp:ListItem Value="RequestBy">Diminta Oleh</asp:ListItem>
                     <asp:ListItem Value="Remark">Remark</asp:ListItem>                     
                  </asp:DropDownList>
                  <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlRange"></asp:DropDownList>
                  <asp:Button class="bitbtn btnsearch" runat="server" ID="btnSearch" Text="Search" />																						 											  
                  <asp:Button class="bitbtn btngo" runat="server" ID="btnExpand" Text="..." />																						 											                                                
            </td>            
            <td>
                <%--<asp:LinkButton ID="LbAdvSearch" runat="server" Text="Advanced Search" />
                 &nbsp &nbsp &nbsp &nbsp
                <asp:Label runat="server" ID="Label1" Text="Outstanding SPK: "/>
                <asp:LinkButton runat="server" ID="lbCount" Text="X"  Font-Size="Small" />
                <asp:Label runat="server" ID="Label2" Text=" record(s)"/>--%>
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
                <asp:ListItem Value="TransNmbr" Selected="True">No. Service Out</asp:ListItem>
                <asp:ListItem Value="Status">Status</asp:ListItem>
                <asp:ListItem Value="dbo.FormatDate(TransDate)">Date</asp:ListItem>
                <asp:ListItem Value="WOServiceNo">No. WO Service</asp:ListItem>
                <asp:ListItem Value="SuppName">Nama Supplier</asp:ListItem>
                <asp:ListItem Value="DeptName">Department</asp:ListItem>
                <asp:ListItem Value="RequestBy">Diminta Oleh</asp:ListItem>
                <asp:ListItem Value="Remark">Remark</asp:ListItem>                     
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
          <br/>&nbsp;
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
                  <asp:TemplateField HeaderStyle-Width="110" HeaderText="Action">
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
                  <asp:BoundField DataField="TransNmbr" HeaderStyle-Width="120px" SortExpression="Nmbr" HeaderText="No. Service Out"></asp:BoundField>                  
                  <asp:BoundField DataField="Status" HeaderStyle-Width="60px" ItemStyle-HorizontalAlign="Center" HeaderText="Status" ></asp:BoundField>                  
                  <asp:BoundField DataField="TransDate" dataformatstring="{0:dd MMM yyyy}" htmlencode="true" HeaderStyle-Width="80px" SortExpression="TransDate" HeaderText="Date"></asp:BoundField>
                  <asp:BoundField DataField="WOServiceNo" HeaderStyle-Width="120px" SortExpression="WOServiceNo" HeaderText="No. WO Service"></asp:BoundField>
                  <asp:BoundField DataField="SuppName" HeaderStyle-Width="150px" SortExpression="SuppName" HeaderText="Nama Supplier"></asp:BoundField>
                  <asp:BoundField DataField="DeptName" HeaderStyle-Width="70px" SortExpression="DeptName" HeaderText="Department"></asp:BoundField>
                  <asp:BoundField DataField="RequestBy" HeaderStyle-Width="60px" SortExpression="RequestBy" HeaderText="Diminta Oleh"></asp:BoundField>
                  <asp:BoundField DataField="Remark" HeaderStyle-Width="200px" SortExpression="Remark" HeaderText="Remark"></asp:BoundField>
              </Columns>
          </asp:GridView>
          </div>
          <br/>
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
              <td>
                 <asp:Menu ID="Menu2" runat="server" CssClass="Menu" ItemWrap="False" 
                   Orientation="Horizontal" StaticEnableDefaultPopOutImage="False" 
                   StaticMenuItemStyle-CssClass="MenuItem" 
                   StaticSelectedStyle-CssClass="MenuSelect">
                   <StaticSelectedStyle CssClass="MenuSelect" />
                   <StaticMenuItemStyle CssClass="MenuItem" />
                   <Items>
                     <%--<asp:MenuItem Text="Form Input Service Out" Value="0"></asp:MenuItem>
                     <asp:MenuItem Text="Upload Dokumen Layout" Value="1"></asp:MenuItem> --%>
                   </Items>
                 </asp:Menu>                    
              </td>
                <td>
                   <asp:Button class="bitbtndt btnback" Visible = "false" runat="server" ID="btnGoEdit" Text="Back" /> 
                </td>
            </tr>
       </table>
        
         <br /> 
     <asp:MultiView ID="MultiView2" runat="server" ActiveViewIndex="0">
       <asp:View ID="TabHd0" runat="server">     
          <table>
            <tr>
                <td>Service Out No</td>
                <td>:</td>
                <td><asp:TextBox CssClass="TextBoxR" runat="server" ID="tbCode" Width="220px" Enabled="False"/></td> 
                <!--<td style="width: 10px"></td>-->
                <td>Service Out Date</td>
                <td>:</td>
                <td>    
                    <BDP:BasicDatePicker ID="tbDate" runat="server" DateFormat="dd MMM yyyy" 
                        ReadOnly = "true" ValidationGroup="Input"
                        ButtonImageHeight="19px" ButtonImageWidth="20px" 
                        DisplayType="TextBoxAndImage" 
                        TextBoxStyle-CssClass="TextDate" AutoPostBack="False" 
                        ShowNoneButton="False"><TextBoxStyle CssClass="TextDate" />
                    </BDP:BasicDatePicker>            
                </td>                                                
            </tr> 
            <tr>             
                <td>WO Service No</td>
                <td>:</td>
                <td>
                  <!--<asp:TextBox ID="tbArea" runat="server" CssClass="TextBox" ValidationGroup="Input" Width="50px" />-->
                  <asp:TextBox ID="tbNoWOService" runat="server" CssClass="TextBox"  ValidationGroup="Input" Width="185px" />
                  <asp:Button ID="btnNoWOService" runat="server" Class="btngo" Text="..." ValidationGroup="Input" /> 
                </td>
                <td>RequestBy</td>
                <td>:</td>
                <td>
                   <asp:TextBox ID="tbRequestBy" runat="server" CssClass="TextBox" ValidationGroup="Input" Width="150px" />
                </td>                
            </tr>
            <tr>
                <%--<td>Nama Supplier</td>--%>
                <td><asp:LinkButton ID="lbsupplier" ValidationGroup="Input" runat="server" Text="Nama Supplier"/></td>
                <td>:</td>
                <td>
                   <asp:TextBox ID="tbSupplierCode" runat="server" CssClass="TextBox" ValidationGroup="Input" Width="60px" />
                   <asp:TextBox ID="tbSupplierName" runat="server" CssClass="TextBox" ValidationGroup="Input" Width="150px" />
                   <asp:Button ID="btnSupplier" runat="server" Class="btngo" Text="..." ValidationGroup="Input" /> 
                </td>
                <td>Department</td>
                <td>:</td>
                <td>
                   <asp:TextBox ID="tbDepartCode" runat="server" CssClass="TextBox" ValidationGroup="Input" Width="60px" />
                   <asp:TextBox ID="tbDepartName" runat="server" CssClass="TextBox" ValidationGroup="Input" Width="150px" />
                   <asp:Button ID="btnDepartment" runat="server" Class="btngo" Text="..." ValidationGroup="Input" /> 
                </td>                
            </tr>
            <tr>
               <td>Remark</td>
               <td>:</td>
               <td>
                  <asp:TextBox ID="tbRemark" runat="server" CssClass="TextBoxMulti" 
                   MaxLength="255" TextMode="MultiLine" ValidationGroup="Input" Width="400px" />
               </td>
            </tr>            
          </table>           
       </asp:View>
       
       <asp:View ID="TabHd1" runat="server">
            <table>
                <tr>
                  <td><asp:Button class="bitbtndt btndelete" OnClientClick="return confirm('Sure to delete this dokumen?');" runat="server" ID="btnClearInv" Width="15px" Text="s" />Upload View Layout </td>
                  <td>:</td>                  
                  <td> 
                     <asp:FileUpload runat="server" style="color: White;" accept="application/pdf" ID="FubInv"  />
                     <asp:Button ID="btnsaveINV" CssClass="bitbtndt btnadd" runat="server" Text="View" Style="display: none" />               
                  </td> 
                  <td>        
                    <asp:LinkButton ID="lbDokInv" ValidationGroup="Input" runat="server" Text="Not Yet Uploaded"/>
                            <%--<asp:Button ID="btnsaveKtpW" CssClass="bitbtndt btnadd" runat="server"  Style="display: none" Text="View" /> --%>
                  </td>           
                </tr>               
            </table>
       </asp:View>      
    </asp:MultiView> 
  
      <br />
      <asp:Menu
            ID="Menu1" runat="server" CssClass = "Menu" StaticMenuItemStyle-CssClass = "MenuItem"
            StaticSelectedStyle-CssClass = "MenuSelect" Orientation="Horizontal" ItemWrap = "False"
            StaticEnableDefaultPopOutImage="False">            
            <Items>
                <%-- <asp:MenuItem Text="Detail Invoice" Value="0"></asp:MenuItem> --%>
                <asp:MenuItem Text="Detail Service Out " Value="1"></asp:MenuItem>                   
            </Items>            
      </asp:Menu>
        <hr /> 
      
        <asp:Panel runat="server" ID="pnlDt">
          <asp:Button class="bitbtn btnadd" runat="server" ID="btnAddDt" Text="Add" ValidationGroup="Input" /><br/>&nbsp;            	            
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
                            <asp:BoundField DataField="ItemNo" ItemStyle-HorizontalAlign="Right" HeaderText="No" />
                            <asp:BoundField DataField="ItemName" HeaderStyle-Width="250px" HeaderText="Keterangan" />                           
                            <asp:BoundField DataField="Qty" ItemStyle-HorizontalAlign="Right"  HeaderStyle-Width="60px" DataFormatString="{0:#,##0.00}" HeaderText="Qty" />
                            <asp:BoundField DataField="UnitName" HeaderStyle-Width="60px" HeaderText="Unit" /> 
                            <asp:BoundField DataField="Remark" HeaderStyle-Width="250px" HeaderText="Remark" />
                    </Columns>
                </asp:GridView>
          </div> 
          <br/>&nbsp;  
          <asp:Button class="bitbtn btnadd" runat="server" ID="btnAddDt2" Text="Add" ValidationGroup="Input" />	
                          
       </asp:Panel>       
       <asp:Panel runat="server" ID="pnlEditDt" Visible="false">
            <table>                                                
                <tr>
                   <td>No</td>
                   <td>:</td>
                   <td><asp:Label ID="lbItemNo" runat="server" Text="Item" /></td>           
                </tr>   
                <tr>
                   <td>Keterangan</td>
                   <td>:</td>
                   <td>
                     <asp:TextBox ID="tbItemName" runat="server" CssClass="TextBox" ValidationGroup="Input" Width="250px" />
                   </td>                
                </tr> 
                <tr>
                   <td>Jumlah</td>
                   <td>:</td>
                   <td>
                     <asp:TextBox ID="tbQty" runat="server" CssClass="TextBox" ValidationGroup="Input" Width="50px" />
                   </td>                                
                </tr>                                                                                 
                <tr>                    
                   <td>Satuan</td>
                   <td>:</td>
                   <td>
                     <asp:DropDownList ID="ddlUnit" runat="server" CssClass="DropDownList" Width="100px"/>
                   </td>
                </tr>                                      
                <tr>
                   <td>Remark</td>
                   <td>:</td>
                   <td><asp:TextBox runat="server" ID="tbRemarkDt" CssClass="TextBox" Width="365px" MaxLength="255" TextMode="MultiLine" />                        
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
<!--    <asp:Panel runat="server" ID="pnlPrint" Visible="false">
    <cr:crystalreportviewer ID="CrystalReportViewer1" runat="server" AutoDataBind="True"
               Height="1036px" Width="928px" /> 
    </asp:Panel> -->
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
