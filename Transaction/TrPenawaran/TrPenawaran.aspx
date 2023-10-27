<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TrPenawaran.aspx.vb" Inherits="Penawaran" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<%@ Register Assembly="BasicFrame.WebControls.BasicDatePicker" Namespace="BasicFrame.WebControls"
    TagPrefix="BDP" %>

<%@ Register assembly="CrystalDecisions.Web, Version=10.5.3700.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Surat Penawaran</title>
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
        
        

        function setformatfordt(prmchange) 
      {
          try 
          {
          
              //document.getElementById("tbTotDisc").value = parseFloat(document.getElementById("tbTotHarga").value.replace(/\$|\,/g, "")) * parseFloat(document.getElementById("tbDiscHd").value.replace(/\$|\,/g, "")) / 100;

              document.getElementById("tbDiscHd").value = parseFloat(document.getElementById("tbTotDisc").value.replace(/\$|\,/g, "")) / parseFloat(document.getElementById("tbTotHarga").value.replace(/\$|\,/g, "")) * 100;

              document.getElementById("tbTotDPP").value = (parseFloat(document.getElementById("tbTotHarga").value.replace(/\$|\,/g, "")) - parseFloat(document.getElementById("tbTotDisc").value.replace(/\$|\,/g, "")));
              document.getElementById("tbTotDP").value = (parseFloat(document.getElementById("tbTotDPP").value.replace(/\$|\,/g, "")) * parseFloat(document.getElementById("tbDpHd").value.replace(/\$|\,/g, "")) / 100) - parseFloat(document.getElementById("tbTotTJ").value.replace(/\$|\,/g, ""));

              document.getElementById("tbTotAmount").value = parseFloat(document.getElementById("tbTotDPP").value.replace(/\$|\,/g, "")) - parseFloat(document.getElementById("tbTotDP").value.replace(/\$|\,/g, ""));

              
              document.getElementById("tbDiscHd").value = setdigit(document.getElementById("tbDiscHd").value.replace(/\$|\,/g, ""), '<%=ViewState("DigitCurr")%>');
              document.getElementById("tbTotDisc").value = setdigit(document.getElementById("tbTotDisc").value.replace(/\$|\,/g, ""), '<%=ViewState("DigitCurr")%>');
              document.getElementById("tbTotDPP").value = setdigit(document.getElementById("tbTotDPP").value.replace(/\$|\,/g, ""), '<%=ViewState("DigitCurr")%>');
              document.getElementById("tbDpHd").value = setdigit(document.getElementById("tbDpHd").value.replace(/\$|\,/g, ""), '<%=ViewState("DigitCurr")%>');
              document.getElementById("tbTotTJ").value = setdigit(document.getElementById("tbTotTJ").value.replace(/\$|\,/g, ""), '<%=ViewState("DigitCurr")%>');
              document.getElementById("tbTotDP").value = setdigit(document.getElementById("tbTotDP").value.replace(/\$|\,/g, ""), '<%=ViewState("DigitCurr")%>');
              
              document.getElementById("tbTotAmount").value = setdigit(document.getElementById("tbTotAmount").value.replace(/\$|\,/g, ""), '<%=ViewState("DigitCurr")%>');

              document.getElementById("tbTotHarga").value = setdigit(document.getElementById("tbTotHarga").value.replace(/\$|\,/g, ""), '<%=ViewState("DigitCurr")%>');

          } catch (err) {
              alert(err.description);
          }
      }


      function setformatdtprice(prmchange) {

          try {


              document.getElementById("tbPrice").value = parseFloat(document.getElementById("tbLuasTanah").value.replace(/\$|\,/g, "")) * parseFloat(document.getElementById("tbPricePerm2").value.replace(/\$|\,/g, ""));

              document.getElementById("tbPrice").value = setdigit(document.getElementById("tbPrice").value.replace(/\$|\,/g, ""), '<%=ViewState("DigitHome")%>');
              document.getElementById("tbLuasTanah").value = setdigit(document.getElementById("tbLuasTanah").value.replace(/\$|\,/g, ""), '<%=ViewState("DigitHome")%>');
              document.getElementById("tbPricePerm2").value = setdigit(document.getElementById("tbPricePerm2").value.replace(/\$|\,/g, ""), '<%=ViewState("DigitHome")%>');

          } catch (err) {
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
    <div class="H1">Surat Penawaran</div>
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
                      <asp:ListItem>Remark</asp:ListItem>
                  </asp:DropDownList>
                  <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlRange"></asp:DropDownList>

                  <asp:Button class="bitbtn btnsearch" runat="server" ID="btnSearch" Text="Search" />																						 											  
                  <asp:Button class="bitbtn btngo" runat="server" ID="btnExpand" Text="..." />																						 											  
                 
                 
                
            </td>
            
            <td>
                <asp:LinkButton ID="LbAdvSearch" runat="server" Text="Advanced Search" />
                 <%--&nbsp &nbsp &nbsp &nbsp
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
                  <asp:ListItem Selected="True" Value="TransNmbr">Reference</asp:ListItem>
                  <asp:ListItem>Status</asp:ListItem>
                  <asp:ListItem Value="dbo.FormatDate(TransDate)">Date</asp:ListItem>
                      <asp:ListItem Value="TransNmbr" Selected="True">Reference</asp:ListItem>
                      <asp:ListItem>Status</asp:ListItem>
                      <asp:ListItem Value="dbo.FormatDate(TransDate)">Date</asp:ListItem>
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
                              <asp:ListItem Text="Print" />
                          </asp:DropDownList>
                          
                          <asp:Button class="btngo" runat="server" ID="BtnGo" Text="G" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" CommandName="Go" />
                          
                      </ItemTemplate>
                      <HeaderStyle Width="110px" />
                  </asp:TemplateField>
                  <asp:BoundField DataField="TransNmbr" HeaderStyle-Width="120px" SortExpression="Nmbr" HeaderText="Reference"></asp:BoundField>                  
                  <asp:BoundField DataField="Status" HeaderText="Status"></asp:BoundField>                  
                  <asp:BoundField DataField="TransDate" dataformatstring="{0:dd MMM yyyy}" htmlencode="true" HeaderStyle-Width="80px" SortExpression="TransDate" HeaderText="Date"></asp:BoundField>
                  <asp:BoundField DataField="NamaCPembeli" HeaderStyle-Width="150px" SortExpression="NamaCPembeli" HeaderText="Calon Pembeli"></asp:BoundField>
                  <asp:BoundField DataField="PhoneCPembeli" HeaderStyle-Width="100px" SortExpression="PhoneCPembeli" HeaderText="Phone"></asp:BoundField>
                  <asp:BoundField DataField="TotalHarga" DataFormatString="{0:#,##0.00}" ItemStyle-HorizontalAlign="Right"  HeaderStyle-Width="80px" SortExpression="TotalHarga" HeaderText="Total Harga"></asp:BoundField>
                  <asp:BoundField DataField="Remark" HeaderStyle-Width="120px" SortExpression="Remark" HeaderText="Remark"></asp:BoundField>
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
                <td>
                     <asp:Menu ID="Menu2" runat="server" CssClass="Menu" ItemWrap="False" 
                Orientation="Horizontal" StaticEnableDefaultPopOutImage="False" 
                StaticMenuItemStyle-CssClass="MenuItem" 
                StaticSelectedStyle-CssClass="MenuSelect">
                <StaticSelectedStyle CssClass="MenuSelect" />
                <StaticMenuItemStyle CssClass="MenuItem" />
                <Items>
                    <asp:MenuItem Text="Form Input Penawaran" Value="0"></asp:MenuItem>
                    <asp:MenuItem Text="Upload Dokumen Layout" Value="1"></asp:MenuItem>
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
                <td>Penawran No</td>
                <td>:</td>
                <td><asp:TextBox CssClass="TextBoxR" runat="server" ID="tbRef" Width="225px" Enabled="False"/>        
                </td>   
                
                <td>Penawran Date</td>
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
             <td>Sewa</td>
                <td>:</td>
                <td>
               <asp:DropDownList CssClass="DropDownList" Width="230" runat="server" ID="ddlFgsewa" >
                                <asp:ListItem Selected="True" >N</asp:ListItem>
                                <asp:ListItem >Y</asp:ListItem>
                      </asp:DropDownList>
                </td>
              
              <td>Masa Berlaku</td>
                <td>:</td>
                <td>    
                    <BDP:BasicDatePicker ID="tbMasaBerlaku" runat="server" DateFormat="dd MMM yyyy" 
                            ReadOnly = "true" ValidationGroup="Input"
                            ButtonImageHeight="19px" ButtonImageWidth="20px" 
                            DisplayType="TextBoxAndImage" 
                            TextBoxStyle-CssClass="TextDate" AutoPostBack="True" 
                            ShowNoneButton="False"><TextBoxStyle CssClass="TextDate" /></BDP:BasicDatePicker>            
                </td>  
                
            <tr>
             
            </tr>
            

            <tr>
           
                 <td>Area</td>
                <td>:</td>
                <td><asp:TextBox runat="server" ValidationGroup="Input" ID="tbArea" CssClass="TextBox" Width="50px"/>
                <asp:TextBox runat="server" ValidationGroup="Input" ID="tbAreaName" CssClass="TextBox" Width="166px"/>
                <asp:Button Class="btngo" ID="btnArea" Text="..." runat="server" ValidationGroup="Input" /></td>
                
                <td>Fasilitas / Akses</td>
                <td>:</td>
                <td><asp:TextBox runat="server" ValidationGroup="Input" ID="tbFasilitas" CssClass="TextBox" Width="225px"/>
                </td>
                        
            </tr>
            
            
            
             <tr>
             
              <td>Nama Calon Pembeli</td>
                <td>:</td>
                <td><asp:TextBox runat="server" ValidationGroup="Input" ID="tbCalonPembeli" CssClass="TextBox" Width="225px"/>
                </td>
           
                 <td>Nama Sales</td>
                <td>:</td>
                <td><asp:TextBox runat="server" ValidationGroup="Input" ID="tbNamaSales" CssClass="TextBox" Width="225px"/>
                </td>
             
            </tr>
            
            
            <tr>
             <td>Alamat</td>
                <td>:</td>
                <td><asp:TextBox runat="server" ValidationGroup="Input" ID="tbAlamat" CssClass="TextBox" Width="225px"/>
                </td>
                
                   
                <td>Email Sales</td>
                <td>:</td>
                <td><asp:TextBox runat="server" ValidationGroup="Input" ID="tbEmailSales" CssClass="TextBox" Width="225px"/>
                </td>
                        
           
                 
                
               
                        
            </tr>
            
            
              
            <tr>       
                
                
                <td>No Telp</td>
                <td>:</td>
                <td><asp:TextBox runat="server" ValidationGroup="Input" ID="tbTlp" CssClass="TextBox" Width="225px"/>
                </td>
                
                <td>No Telp Sales</td>
                <td>:</td>
                <td><asp:TextBox runat="server" ValidationGroup="Input" ID="tbTelpSales" CssClass="TextBox" Width="225px"/>
                </td>
                        
            </tr>
            
            
            <tr>
           
                
                
                <td>Email</td>
                <td>:</td>
                <td><asp:TextBox runat="server" ValidationGroup="Input" ID="tbEmail" CssClass="TextBox" Width="225px"/>
                </td>
                
                 <td>No Hotline IGL</td>
                <td>:</td>
                <td><asp:TextBox runat="server" ValidationGroup="Input" ID="tbNoHotline" CssClass="TextBox" Width="225px"/>
                </td>
                        
            </tr>
     
     
            <tr>
           
                 <td>Perantara 1</td>
                <td>:</td>
                <td><asp:TextBox runat="server" ValidationGroup="Input" ID="tbPerantara1" CssClass="TextBox" Width="225px"/>
                </td>
                
                <td>Perantara 2</td>
                <td>:</td>
                <td><asp:TextBox runat="server" ValidationGroup="Input" ID="tbPerantara2" CssClass="TextBox" Width="225px"/>
                </td>
                        
            </tr>
            
    
          
     
            
            
                        <tr>
                                <td>Price</td>
                                <td>:</td>
                                <td colspan="7">
                                    <table>
                                        <tr style="background-color:Silver;text-align:center; border-radius:30px;">
                                            <td>Total Harga</td> 
                                            <td>Disc(%)</td>                                       
                                            <td>Disc Value</td>
                                            <td>DPP</td>
                                            <td>DP(%)</td>
                                            <td>DP Value</td>
                                            <td>Tanda Jadi</td>                                            
                                            <td>Total Amount</td>
                                        </tr>
                                        
                                        <tr>
                                            <td><asp:TextBox ID="tbTotHarga" ValidationGroup="Input" runat="server"  CssClass="TextBox" width="120px"/></td>
                                            <td><asp:TextBox ID="tbDiscHd"  ValidationGroup="Input" runat="server"  CssClass="TextBox" Width="50px"/></td> 
                                            <td><asp:TextBox ID="tbTotDisc"  ValidationGroup="Input" runat="server"  CssClass="TextBox" Width="120px"/></td>
                                             <td><asp:TextBox ID="tbTotDPP"  ValidationGroup="Input" runat="server" CssClass="TextBox" Width="120px"/></td> 
                                             <td><asp:TextBox ID="tbDpHd"  ValidationGroup="Input" runat="server"  CssClass="TextBox" Width="50px"/></td> 
                                              <td><asp:TextBox ID="tbTotDP"  ValidationGroup="Input" runat="server"  CssClass="TextBox" Width="120px"/></td>  
                                            <td><asp:TextBox ID="tbTotTJ"  ValidationGroup="Input" runat="server"  CssClass="TextBox" Width="120px"/></td>                                            
                                             <td><asp:TextBox ID="tbTotAmount"  ValidationGroup="Input" runat="server"  CssClass="TextBox" Width="120px"/></td> 
                                            
                                        </tr>
                                    </table>
                                </td>                
                        </tr>
            <tr>
                <td>Sistem Pembayaran</td>
                <td>:</td>
                <td>
                <asp:DropDownList CssClass="DropDownList" Width = "230px" runat="server" ID="ddlSistemBayar" >
                                <asp:ListItem Selected="True" >Cash</asp:ListItem>
                                <asp:ListItem >Angsuran</asp:ListItem>
                                <asp:ListItem >KPR</asp:ListItem>
                      </asp:DropDownList>
                <%--<asp:TextBox runat="server" ValidationGroup="Input" ID="tbSistemBayar" CssClass="TextBox" Width="225px"/>--%>
                </td>
                
                
                <td>Masa Angsuran</td>
                <td>:</td>
                <td><asp:TextBox runat="server" ValidationGroup="Input" ID="tbAngsuran" CssClass="TextBox" Width="220px"/>
                
                </td>
                
            </tr>            
          
            
            <tr>
                <td>Remark</td>
                <td>:</td>
                <td><asp:TextBox runat="server" ValidationGroup="Input" ID="tbRemark" CssClass="TextBoxMulti" Width="400px" TextMode="MultiLine" MaxLength="255"/></td>
                
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
                <asp:MenuItem Text="Detail Penawaran" Value="1"></asp:MenuItem>                   
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
                            <asp:BoundField DataField="UnitCode" HeaderStyle-Width="100px" HeaderText="Unit Code" /> 
                            <asp:BoundField DataField="UnitName" HeaderStyle-Width="150px" HeaderText="Unit Name" />                           
                            <asp:BoundField DataField="LuasTanah"  ItemStyle-HorizontalAlign="Right"  HeaderStyle-Width="100px" DataFormatString="{0:#,##0.00}" HeaderText="Luas Tanah" />
                            <asp:BoundField DataField="LuasBangunan"  ItemStyle-HorizontalAlign="Right"  HeaderStyle-Width="100px" DataFormatString="{0:#,##0.00}" HeaderText="Luas Bangunan" />
                            <asp:BoundField DataField="ArahKavling" HeaderStyle-Width="150px" HeaderText="Arah Kavling" /> 
                            <asp:BoundField DataField="KtinggianDPL" HeaderText="DPL"  HeaderStyle-Width="50px" />
                            <asp:BoundField DataField="HargaPer_m2"  ItemStyle-HorizontalAlign="Right"  HeaderStyle-Width="100px" DataFormatString="{0:#,##0.00}" HeaderText="Price/m2" />
                            <asp:BoundField DataField="Price"  ItemStyle-HorizontalAlign="Right"  HeaderStyle-Width="100px" DataFormatString="{0:#,##0.00}" HeaderText="Price" />
                            <%--<asp:BoundField DataField="Disc" HeaderText="%" HeaderStyle-Width="50px"  />
                            <asp:BoundField DataField="DiscValue"  ItemStyle-HorizontalAlign="Right"  HeaderStyle-Width="100px" DataFormatString="{0:#,##0.00}" HeaderText="Disc Value" />
                            <asp:BoundField DataField="Dpp"  ItemStyle-HorizontalAlign="Right"  HeaderStyle-Width="100px" DataFormatString="{0:#,##0.00}" HeaderText="DPP" />
                            <asp:BoundField DataField="TJ"  ItemStyle-HorizontalAlign="Right"  HeaderStyle-Width="100px" DataFormatString="{0:#,##0.00}" HeaderText="Tanda Jadi" />
                            <asp:BoundField DataField="DP" HeaderText="%" HeaderStyle-Width="50px"/>
                            <asp:BoundField DataField="DpValue"  ItemStyle-HorizontalAlign="Right"  HeaderStyle-Width="100px" DataFormatString="{0:#,##0.00}" HeaderText="DP Value" />
                            <asp:BoundField DataField="AmountForex"  ItemStyle-HorizontalAlign="Right"  HeaderStyle-Width="100px" DataFormatString="{0:#,##0.00}" HeaderText="Total Harga" />--%>
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
                        <td>Unit</td>
                        <td>:</td>
                        <td><asp:TextBox runat="server" ValidationGroup="Input" ID="TbUnit" CssClass="TextBox" Width="50px"/>
                        <asp:TextBox runat="server" ValidationGroup="Input" ID="tbUnitName" CssClass="TextBox" Width="175px"/>
                        <asp:Button Class="btngo" ID="btnUnit" Text="..." runat="server" ValidationGroup="Input" /></td>
                    </tr>
                    
                    
                    <tr>
                            <td>Luas</td>
                            <td>:</td>
                            <td colspan="7">
                                <table>
                                    <tr style="background-color:Silver;text-align:center; border-radius:30px;">
                                        <td>Tanah (m2)</td>
                                        <td>Bangunan (m2)</td>
                                        <td>Arah Kavling</td>
                                        <td>DPL (m)</td>
                                        <td>Harga/(m2)</td>
                                    </tr>
                                    <tr>

                                        <td><asp:TextBox ID="tbLuasTanah"  ValidationGroup="Input" runat="server" Enabled = "True" CssClass="TextBox" Width="80px"/></td>                                         
                                        <td><asp:TextBox ID="tbLuasBangunan"  ValidationGroup="Input" runat="server"  CssClass="TextBox" Width="80px"/></td> 
                                        <td><asp:TextBox ID="tbArahKav"  ValidationGroup="Input" runat="server"  CssClass="TextBox" Width="100px"/></td> 
                                        <td><asp:TextBox ID="tbKtinggianDPL"  ValidationGroup="Input" runat="server"  CssClass="TextBox" Width="80px"/></td>                          
                                        <td><asp:TextBox ID="tbPricePerm2"  ValidationGroup="Input" runat="server"  CssClass="TextBox" Width="150px"/></td> 
                                    </tr>
                                </table>
                            </td>                
                    </tr>
                    
                    <tr>
                            <td>Amount</td>
                            <td>:</td>
                            <td colspan="7">
                                <table>
                                    <tr style="background-color:Silver;text-align:center; border-radius:30px;">
                                        <td>Price</td>
                                        <%--<td>Disc</td>
                                        <td>Disc Value</td>
                                        <td>DPP</td>
                                        <td>Tanda Jadi</td>
                                        <td>DP</td>
                                        <td>DP Value</td>                                        
                                        <td>Total Amount</td>--%>
                                    </tr>
                                    
                                    
                                    <tr>
                                        <td><asp:TextBox ID="tbPrice"  ValidationGroup="Input" runat="server" Enabled = "True" CssClass="TextBox" Width="225px"/></td>                                         
                                        <%--<td><asp:TextBox ID="tbDisc"  ValidationGroup="Input" runat="server"  CssClass="TextBox" Width="50px"/></td>   
                                        <td><asp:TextBox ID="tbDiscValue"  ValidationGroup="Input" runat="server" Enabled = "True" CssClass="TextBox" Width="120px"/></td>                                         
                                        <td><asp:TextBox ID="tbDpp"  ValidationGroup="Input" runat="server"  CssClass="TextBox" Width="120px"/></td>
                                        <td><asp:TextBox ID="tbTandaJadi"  ValidationGroup="Input" runat="server"  CssClass="TextBox" Width="120px"/></td>
                                        
                                        <td><asp:TextBox ID="tbDP"  ValidationGroup="Input" runat="server" Enabled = "True" CssClass="TextBox" Width="50px"/></td>                                         
                                        <td><asp:TextBox ID="tbDPValue"  ValidationGroup="Input" runat="server"  CssClass="TextBox" Width="120px"/></td> 
                                        
                                        <td><asp:TextBox ID="tbTotalAmount"  ValidationGroup="Input" runat="server"  CssClass="TextBox" Width="120px"/></td>--%>                       
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
