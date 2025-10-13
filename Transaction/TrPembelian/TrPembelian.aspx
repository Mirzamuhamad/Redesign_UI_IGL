<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TrPembelian.aspx.vb" Inherits="TrPembelian " %>
<%@ Register Assembly="FastReport" Namespace="FastReport.Web" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="BasicFrame.WebControls.BasicDatePicker" Namespace="BasicFrame.WebControls"
    TagPrefix="BDP" %>
<%@ Register assembly="CrystalDecisions.Web, Version=10.5.3700.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Land Purchase</title>
    <script src="../../Function/OpenDlg.js" type="text/javascript"></script>
    <script src="../../Function/Function.JS" type="text/javascript"></script>
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
        document.getElementById("tbQty").value = setdigit(document.getElementById("tbQty").value.replace(/\$|\,/g,""),'<%=ViewState("DigitQty")%>');        
        }catch (err){
            alert(err.description);
          }      
        }   
        function closing()
        {
            try
            {
                var result = prompt("Remark Close", "");
                if (result){
                    document.getElementById("HiddenRemarkClose").value = result;
                } else {
                    document.getElementById("HiddenRemarkClose").value = "False Value";
                }
                postback();
                //document.form1.submit();                
            }catch(err){
                alert(err.description);
            }        
        }
           function deletetrans()
        {
            try
            {
                
                 var result = confirm("Sure Delete Transaction ?");
                if (result){
                    document.getElementById("HiddenRemarkDelete").value = "true";
                } else {
                    document.getElementById("HiddenRemarkDelete").value = "false";
                }
                postback();
                //document.form1.submit();                
            }catch(err){
                alert(err.description);
            }        
        }
        
        function postback()
        {
            __doPostBack('','');
        }   


    function hitungBiaya() {
    
      var tbBiayaNotaris = document.getElementById('tbBiayaNotaris').value.replace(/\$|\,/g, "");
      var tbLuas = document.getElementById('tbLuas').value.replace(/\$|\,/g, "");
      var result = parseInt(tbBiayaNotaris) * parseInt(tbLuas);

      
      if (!isNaN(result)) {
         document.getElementById('tbJumlah').value = result.toFixed(2).replace(/\d(?=(\d{3})+\.)/g, '$&,');
      }


    }

function myPopup() {
            var left = (screen.width - 370) / 2;
            var top = (screen.height - 800) / 2;
             window.open("../../earchDlgV.Aspx", "", 'toolbar=no, location=no, directories=no, status=no, menubar=no, scrollbars=yes, resizable=yes, copyhistory=no, width=' + 370 + ', height=' + 800 + ', top=' + top + ', left=' + left);
             return false;
         }


         function setformathd(prmchange) {
             try {
           
                 var biaya = 60000000;

//                 document.getElementById("tbTotalHrgAkta").value = (parseFloat(document.getElementById("tbHrgPerm2Akta").value.replace(/\$|\,/g, "")) * parseFloat(document.getElementById("tbLuas").value.replace(/\$|\,/g, "")));
//                 
//                     if (document.getElementById("tbTotalHrgAkta").value > biaya) {

//                         document.getElementById("tbBPHTB").value = (parseFloat(document.getElementById("tbTotalHrgAkta").value.replace(/\$|\,/g, "")) - biaya) * 5 / 100;
//                        
//                     } else {
//                     document.getElementById("tbBPHTB").value = 0 ;
//                     }


                 document.getElementById("tbBiayaSurvey").value = setdigit(document.getElementById("tbBiayaSurvey").value.replace(/\$|\,/g, ""), '<%=ViewState("DigitCurr")%>');
                 document.getElementById("tbBiayaLainLian").value = setdigit(document.getElementById("tbBiayaLainLian").value.replace(/\$|\,/g, ""), '<%=ViewState("DigitCurr")%>');
                 document.getElementById("tbBiayaModerator").value = setdigit(document.getElementById("tbBiayaModerator").value.replace(/\$|\,/g, ""), '<%=ViewState("DigitCurr")%>');
                 document.getElementById("tbTotalHrgAkta").value = setdigit(document.getElementById("tbTotalHrgAkta").value.replace(/\$|\,/g, ""), '<%=ViewState("DigitCurr")%>');
                 document.getElementById("tbHrgPerm2Akta").value = setdigit(document.getElementById("tbHrgPerm2Akta").value.replace(/\$|\,/g, ""), '<%=ViewState("DigitCurr")%>');
                 document.getElementById("tbBPHTB").value = setdigit(document.getElementById("tbBPHTB").value.replace(/\$|\,/g, ""), '<%=ViewState("DigitCurr")%>');
                 document.getElementById("tbLuas").value = setdigit(document.getElementById("tbLuas").value.replace(/\$|\,/g, ""), '<%=ViewState("DigitCurr")%>');


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
    <div class="H1">Land Purchase Order</div>
     <hr style="color:Blue" />        
    <asp:Panel runat="server" id="PnlHd"> 
      <table>
        <tr>
            <td style="width:100px;text-align:right">Quick Search :</td>            
            <td><asp:TextBox CssClass="TextBox" runat="server" ID ="tbFilter"/> 
                  <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlField" >
                      <asp:ListItem Value="Reference" Selected="True">Reference</asp:ListItem>
                      <asp:ListItem Value="dbo.FormatDate(TransDate)">Date</asp:ListItem>
                      <asp:ListItem Value="LosNo">Land Survey No</asp:ListItem>
                      <asp:ListItem>Status</asp:ListItem>

                      <asp:ListItem Value="Remark">Remark</asp:ListItem>  
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
                  <asp:ListItem Value="Reference" Selected="True">Reference</asp:ListItem>
                  <asp:ListItem Value="dbo.FormatDate(TransDate)">Date</asp:ListItem>
                  <asp:ListItem Value="LosNo">Land Survey No</asp:ListItem>
                  <asp:ListItem>Status</asp:ListItem>            
                
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
            <table>
              <tr>
                  <td>
                    <asp:GridView ID="GridView1" runat="server" AllowPaging="True" AllowSorting="True"
                        CssClass="Grid" AutoGenerateColumns="False"> 
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
                                  <asp:Button class="btngo" runat="server" ID="BtnGo" Text="G" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" CommandName="Go"  />                                                 
                              </ItemTemplate>
                              <HeaderStyle Width="110px" />
                          </asp:TemplateField>                     
                          <asp:BoundField DataField="Reference" SortExpression="Reference" 
                              HeaderText="Reference"></asp:BoundField>                  
                          <asp:BoundField DataField="Status" HeaderText="Status" SortExpression="Status"></asp:BoundField>                  
                          <asp:BoundField DataField="TransDate" HtmlEncode="true" DataFormatString="{0:dd MMM yyyy}" HeaderText="Date" SortExpression="TransDate"></asp:BoundField>                  
                          <asp:BoundField DataField="LosNo" HeaderStyle-Width="120px" HeaderText="Los No" 
                              SortExpression="LosNo">
                              <HeaderStyle Width="120px" />
                          </asp:BoundField>
                          
                           <asp:BoundField DataField="TJNo" HeaderStyle-Width="120px" HeaderText="TJ No" 
                              SortExpression="TJNo">
                              <HeaderStyle Width="120px" />
                          </asp:BoundField>
                          
                           <asp:BoundField DataField="Revisi" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="center" HeaderText="Rev" 
                              SortExpression="Revisi">
                              <HeaderStyle Width="30px" />
                          </asp:BoundField>
                          
                          <asp:BoundField DataField="AreaNAme" HeaderStyle-Width="120px" HeaderText="Area" 
                              SortExpression="AreaNAme">
                              <HeaderStyle Width="120px" />
                          </asp:BoundField>
                          
                           <asp:BoundField DataField="JenisDoc" HeaderStyle-Width="120px" HeaderText="Jenis Dokumen" 
                              SortExpression="JenisDoc">
                              <HeaderStyle Width="120px" />
                          </asp:BoundField>
                          <asp:BoundField DataField="PBBNo" HeaderStyle-Width="120px" HeaderText="SPPT" 
                              SortExpression="PBBNo">
                              <HeaderStyle Width="120px" />
                          </asp:BoundField>

                          <asp:BoundField DataField="PemilikAwal" HeaderStyle-Width="120px" HeaderText="Pemilik Awal " 
                          SortExpression="PemilikAwal"> 
                          <HeaderStyle Width="120px" />                
                            </asp:BoundField>

                            <asp:BoundField DataField="PemilikAkhir" HeaderStyle-Width="120px" HeaderText="Pemilik Akhir" 
                            SortExpression="PemilikAkhir">                 
                            </asp:BoundField> 

                        <asp:BoundField DataField="LuasUkur" ItemStyle-HorizontalAlign="right" HeaderText="Luas" DataFormatString="{0:#,##0.##}" SortExpression="LuasUkur"> 
                                    <HeaderStyle Width="300px" />
                                </asp:BoundField>

                        <asp:BoundField DataField="HrgTanah" ItemStyle-HorizontalAlign="right" HeaderText="Harga Per m2" DataFormatString="{0:#,##0.##}" SortExpression="HrgTanah"> 
                                    <HeaderStyle Width="300px" />
                                </asp:BoundField>

                        <asp:BoundField DataField="TtlHrgTanah" ItemStyle-HorizontalAlign="right" HeaderText="Jumlah" DataFormatString="{0:#,##0.##}" SortExpression="TtlHrgTanah"> 
                                    <HeaderStyle Width="300px" />
                                </asp:BoundField>        

                       
                        <asp:BoundField DataField="Remark" HeaderStyle-Width="200px" HeaderText="Remark" SortExpression="Remark"> 
                            <HeaderStyle Width="200px"  />
                          </asp:BoundField>
                      </Columns>
                    </asp:GridView> 
                  </td>
              </tr>
          </table> 

        </div>

            
            <asp:Panel runat="server" ID ="pnlNav" Visible="false">
            <asp:Button class="bitbtn btnadd" runat="server" ID="btnAdd2" Text="Add" />	     
            &nbsp &nbsp &nbsp  

            <asp:DropDownList ID="ddlCommand2" CssClass="DropDownList" runat="server"/>
            <asp:Button class="btngo" runat="server" ID="btnGo2" Text="G"  />          
            <br />          
            </asp:Panel>
    </asp:Panel>

    <asp:Panel runat="server" ID="pnlInput" Visible="false">
   <table>
                    <tr>
                        <td>Purchase No</td>
                        <td>:</td>
                        <td><asp:TextBox CssClass="TextBoxR" runat="server" ID="tbCode" Width="225px" Enabled="False"/>
                        </td>            
                        
                    </tr>
                    
                     <tr>
                        <td>Tanda Jadi No</td>
                        <td>:</td>
                        <td>                         
                        <asp:TextBox runat="server" ValidationGroup="Input" AutoPostBack = "true" ID="tbTjNo" CssClass="TextBox" Enabled="False"  Width="168px"/>
                        Rev : <asp:TextBox style="Text-align:center;" runat="server" ValidationGroup="Input" ID="tbRev" CssClass="TextBox" Enabled="False"  Width="20px"/>
                         <asp:Button ID="btnTJ" runat="server" Class="btngo" visible = True Text="v" />
                         
                         <td>Purchase Date</td>
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
                        <td>Land Survey No</td>
                        <td>:</td>
                        <td><asp:TextBox runat="server" ValidationGroup="Input" ID="TbLos" CssClass="TextBox" Width="225px"/>
                         <asp:Button ID="btnLos" runat="server" Class="btngo" visible = "false" Text="..." 
                                Width="26px" />
                        </td>
                        
                       
                        <td>Area</td>
                        <td>:</td>
                        <td>
                            <asp:TextBox runat="server" ValidationGroup="Input" ID="tbArea" Visible ="false" CssClass="TextBox" Width="225px"/>
                            <asp:TextBox runat="server" ValidationGroup="Input" ID="tbAreaName"  CssClass="TextBox" Width="225px"/>
                        </td>
                    </tr>
 

                    
                    </tr>    
                    
                    <tr>                        
                        <td>
                            <asp:TextBox runat="server" ValidationGroup="Input" ID="tbPenjual" Visible  ="false" CssClass="TextBox" Width="225px"/>
                            <asp:TextBox runat="server" ValidationGroup="Input" ID="tbSellName" Visible  ="false" CssClass="TextBox" Width="225px"/>
                            <asp:TextBox runat="server" ValidationGroup="Input" ID="tbPembeli" Visible  ="false" CssClass="TextBox" Width="225px"/>
                        </td>                         
                    </tr>

                    
                    <tr>
                        <td>Pemilik Awal</td>
                        <td>:</td>
                        <td><asp:TextBox runat="server" ValidationGroup="Input" ID="tbPemilikAwal" CssClass="TextBox" Width="225px"/>
                        </td>

                        <td>Pemilik Akhir</td>
                        <td>:</td>          
                        <td><asp:TextBox runat="server" ValidationGroup="Input" ID="tbPemilikAkhir" CssClass="TextBox" Width="225px"/></td> 
                    </tr>

                     <tr>
                        <td>Jenis Dokumen</td>
                        <td>:</td>
                        <td><asp:TextBox runat="server" ValidationGroup="Input" ID="tbJenisDoc" CssClass="TextBox" Width="225px"/></td>

                        <td>No Dokumen</td>
                        <td>:</td>          
                        <td><asp:TextBox runat="server" ValidationGroup="Input" ID="tbNoDoc" CssClass="TextBox" Width="225px"/></td> 
                    </tr>
                    
                    

                    <tr>
                            <td>Luas/Harga</td>
                            <td>:</td>
                            <td colspan="7">
                                <table>
                                    <tr style="background-color:Silver;text-align:center; border-radius:30px;">
                                        <td>Luas M<sup>2</sup>   </td>
                                        <td>Harga /M<sup>2</sup> Transaksi</td>
                                        <td>Jumlah</td>
                                        <td>Harga /M<sup>2</sup> Akta</td>
                                        <td>Jumlah Akta</td>
                                    </tr>
                                    <tr>
                                        <td><asp:TextBox ID="tbLuas" runat="server" CssClass="TextBox" Width="80px"/></td>
                                        <td><asp:TextBox ID="tbHrgPerm2" ValidationGroup="Input" runat="server" CssClass="TextBox" width="105px"/></td>
                                        <td><asp:TextBox ID="tbTotalHarga" runat="server" CssClass="TextBox" Width="150px"/></td> 
                                        <td><asp:TextBox ID="tbHrgPerm2Akta" ValidationGroup="Input" AutoPostBack = "true" runat="server" CssClass="TextBox" width="100px"/></td>
                                        <td><asp:TextBox ID="tbTotalHrgAkta" runat="server" CssClass="TextBox" Width="150px"/></td>                          
                                    </tr>
                                </table>
                            </td>                
                    </tr>
                    
                     <tr>
                            <td></td>
                            <td>:</td>
                            <td colspan="7">
                                <table>
                                    <tr style="background-color:Silver;text-align:center; border-radius:30px;">
                                        <td>Bayar Berapa Kali<asp:LinkButton ID="lbTerm" Visible="false" ValidationGroup="Input" runat="server" Text="Term"/></td>
                                        <td>Pembayaran</td>
                                        <td>Tanda Jadi</td>
                                        <td>Pelunasan</td>
                                    </tr>
                                    <tr>
                                    
                                        
                                        <td>
                                        <asp:TextBox ID="tbPayTerm" ValidationGroup="Input" Enabled ="True"  runat="server" CssClass="TextBox" Width="150px"/> 
                                        
                                        <asp:DropDownList CssClass="DropDownList"  ValidationGroup="Input" Width="150px" Visible="false" runat="server" ID="ddlTerm" AutoPostBack="true" /> 
                                            <BDP:BasicDatePicker ID="tbDueDate"  runat="server" Visible = "false" DateFormat="dd MMM yyyy" 
                                                    ReadOnly = "true" Enabled="false"
                                                    ButtonImageHeight="19px" ButtonImageWidth="20px" 
                                                    DisplayType="TextBox" 
                                                    TextBoxStyle-CssClass="TextDate" AutoPostBack="True" 
                                                    ShowNoneButton="False"><TextBoxStyle CssClass="TextDate" /></BDP:BasicDatePicker>
                                        </td> 
                                        
                                        <td>
                                            <asp:DropDownList runat="server" CssClass="DropDownList" ID="ddlPembayaran" 
                                                Width="160px" >
                                            <asp:ListItem Selected="True" Value="T">Transfer</asp:ListItem>
                                            <asp:ListItem Value="C">Cash</asp:ListItem>
                                            <asp:ListItem Value="K">Cek</asp:ListItem>
                                            <asp:ListItem Value="G">Giro</asp:ListItem>                                          
                                            </asp:DropDownList>
                                        </td>
                                        
                                        <td><asp:TextBox ID="tbTandaJadi" ValidationGroup="Input" Enabled ="false"  runat="server" CssClass="TextBox" Width="150px"/></td> 
                                        <td><asp:TextBox ID="tbPelunasan" ValidationGroup="Input" Enabled = "false" runat="server" CssClass="TextBox" width="150px"/></td>
                                                        
                                    </tr>
                                </table>
                            </td>                
                    </tr>

                       
                    <tr>
                        <td>Moderator</td>
                        <td>:</td>
                        <td>
                            <asp:TextBox runat="server" ValidationGroup="Input" ID="tbModerator" Visible="false" CssClass="TextBox" Width="225px"/>
                            <asp:TextBox runat="server" ValidationGroup="Input" ID="tbModeName"  CssClass="TextBox" Width="225px"/>
                        </td>
                         <td>No SPPT/PBB</td>
                        <td>:</td>
                        <td><asp:TextBox runat="server" ValidationGroup="Input" ID="tbSPPT" CssClass="TextBox" Width="225px"/></td>
                    </tr>
                    <tr>
                        <td><asp:LinkButton ID="lbNotaris" ValidationGroup="Input" runat="server" Text="Notaris/PPAT"/></td>
                        <td>:</td>
                        <td>
                            <asp:TextBox runat="server" ValidationGroup="Input" ID="tbNotaris" CssClass="TextBox" Visible="false" Width="225px"/>
                        <asp:TextBox runat="server" ValidationGroup="Input" ID="tbNotarisName" CssClass="TextBox" Enabled="false"  Width="192px"/>
                         <asp:Button ID="btnNotaris" runat="server" Class="btngo" visible = True Text="v" />
                    </tr>

                     <tr>
                            <td>Notaris / PPAT</td>
                            <td>:</td>
                            <td colspan="7">
                                <table>
                                    <tr style="background-color:Silver;text-align:center; border-radius:30px;">
                                        <td>Jenis dokumen   </td>
                                        <td>Nomor Dokumen </td>
                                        <td>Biaya/m<sup>2</sup> </td>
                                        <td>Jumlah Biaya </td>
                                    </tr>
                                    <tr>
                                        <td><asp:DropDownList CssClass="DropDownList" ValidationGroup="Input" Width="150px" runat="server" ID="ddlJenisDokNotaris" >
                                             <asp:ListItem Selected="True">AJB</asp:ListItem>
                                                        <asp:ListItem>SPH</asp:ListItem>
                                                        <asp:ListItem>SHM</asp:ListItem>
                                                        <asp:ListItem>SHGB</asp:ListItem>
                                                        <asp:ListItem>SHGU</asp:ListItem>
                                            </asp:DropDownList>         
                                        </td>  
                                        <td><asp:TextBox ID="tbNoDocNotaris" ValidationGroup="Input" runat="server" CssClass="TextBox" width="150px"/></td>
                                        <td><asp:TextBox ID="tbBiayaNotaris" runat="server" CssClass="TextBox" Width="150px" onkeyup="hitungBiaya();" /></td>
                                        <td><asp:TextBox ID="tbJumlah" runat="server" Enabled = "False" CssClass="TextBox" Width="150px"/></td>                          
                                    </tr>
                                </table>
                            </td>                
                    </tr>

                     <tr>
                        
                        </td>
                         <td>Biaya Survey</td>
                        <td>:</td>
                        <td><asp:TextBox runat="server" ValidationGroup="Input" ID="tbBiayaSurvey" CssClass="TextBox" Width="225px"/></td>
                         <td>Biaya Moderator</td>
                        <td>:</td>
                        <td><asp:TextBox runat="server" ValidationGroup="Input" ID="tbBiayaModerator" CssClass="TextBox" Width="225px"/></td>
                    </tr>

                    <tr>
                            <td>BPHTB</td>
                            <td>:</td>
                            
                            <td><asp:TextBox ID="tbBPHTB" ValidationGroup="Input" runat="server" CssClass="TextBox" width="225px"/></td>
                            <asp:DropDownList CssClass="DropDownList" ValidationGroup="Input" Visible ="false" Width="70px" runat="server" ID="ddlFgBPHTB" >
                                             <asp:ListItem Selected="True">Y</asp:ListItem>
                                                        <asp:ListItem>N</asp:ListItem>
                                            </asp:DropDownList>         
                                         
                           <%-- <td>
                                <table>
                                    <tr style="background-color:Silver;text-align:center; border-radius:30px;">
                                        <td>Biaya BPHTB</td>
                                        <td>Inv BPHTB</td>                                       
                                    </tr>
                                    <tr>
                                        <td><asp:TextBox ID="tbBPHTB" ValidationGroup="Input" runat="server" CssClass="TextBox" width="150px"/></td>
                                        <td><asp:DropDownList CssClass="DropDownList" ValidationGroup="Input" Visible ="false" Width="70px" runat="server" ID="ddlFgBPHTB" >
                                             <asp:ListItem Selected="True">Y</asp:ListItem>
                                                        <asp:ListItem>N</asp:ListItem>
                                            </asp:DropDownList>         
                                        </td>  
                                       
                                        
                                    </tr>
                                </table>
                            </td> --%> 


                        </td>
                        <td>Biaya Lain-Lain</td>
                        <td>:</td>
                        <td><asp:TextBox ID="tbBiayaLainLian" ValidationGroup="Input" runat="server" CssClass="TextBox" width="225px"/></td>
                                        <td><asp:DropDownList CssClass="DropDownList" ValidationGroup="Input" Visible ="false"  Width="70px" runat="server" ID="ddlFgOther" >
                                             <asp:ListItem Selected="True">Y</asp:ListItem>
                                                        <asp:ListItem>N</asp:ListItem>
                                            </asp:DropDownList>         
                                        </td> 
                            <%--<td>
                                <table>
                                    <tr style="background-color:Silver;text-align:center; border-radius:30px;">
                                        <td>Biaya Lain Lain</td>
                                        <td>Inv Lain-Lain</td>                                       
                                    </tr>
                                    <tr>
                                         <td><asp:TextBox ID="tbBiayaLainLian" ValidationGroup="Input" runat="server" CssClass="TextBox" width="150px"/></td>
                                        <td><asp:DropDownList CssClass="DropDownList" ValidationGroup="Input" Visible ="false"  Width="70px" runat="server" ID="ddlFgOther" >
                                             <asp:ListItem Selected="True">Y</asp:ListItem>
                                                        <asp:ListItem>N</asp:ListItem>
                                            </asp:DropDownList>         
                                        </td>  
                                       
                                        
                                    </tr>
                                </table>
                            </td> --%> 
                        
                    </tr> 
 
                    
                     <tr>
                        
                        <td>Remark</td>
                        <td>:</td>
                        <td><asp:TextBox runat="server" MaxLength ="225" TextMode="MultiLine" ValidationGroup="Input" ID="tbRemark" CssClass="TextBox" Width="302px"/></td>
                    </tr>  

                     
                </table>



      
    <%--  <br />      
      <div style="font-size:medium; color:Blue;">Detail</div>
      <hr style="color:Blue" />  
        <asp:Panel runat="server" ID="pnlDt">
        <asp:Button class="bitbtn btnadd" runat="server" ID="btnAddDt" Text="Add" ValidationGroup="Input" />	     
                 
            <div style="border:0px  solid; width:100%; height:100%; overflow:auto;">
                <asp:GridView ID="GridDt" runat="server" AutoGenerateColumns="False" 
                    ShowFooter="True">
                    <HeaderStyle CssClass="GridHeader" />
                    <RowStyle CssClass="GridItem" Wrap="false" />
                    <AlternatingRowStyle CssClass="GridAltItem" />
                    <PagerStyle CssClass="GridPager" />
                    <Columns>
                        <asp:TemplateField HeaderText="Action">
                            <ItemTemplate>
                               <asp:Button class="bitbtn btnedit" runat="server" ID="btnEdit" Text="Edit" CommandName="Edit" />																						 											
						       <asp:Button class="bitbtn btndelete" runat="server" ID="btnDelete" Text="Delete" CommandName="Delete"  OnClientClick="return confirm('Sure to delete this data?');" />																						 													     
                            </ItemTemplate>
                            <EditItemTemplate>
                            		<asp:Button class="bitbtn btnsave" runat="server" ID="btnUpdate" Text="Save" CommandName="Update" />																						 																											
									
									<asp:Button class="bitbtn btncancel" runat="server" ID="btnCancel" Text="Cancel" CommandName="Cancel" />																						 													
                                
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="ItemNo" HeaderStyle-Width="100px" HeaderText="Item" SortExpression="ItemNo" ></asp:BoundField>
                        <asp:BoundField DataField="Timbang1" HeaderText="Timbang 1" SortExpression="Timbang1" DataFormatString="{0:#,##0.00}"></asp:BoundField>
                        <asp:BoundField DataField="Timbang2" HeaderText="Timbang 2" SortExpression="Timbang2" DataFormatString="{0:#,##0.00}"></asp:BoundField>
                        <asp:BoundField DataField="Netto1" HeaderText="Netto1" SortExpression="Netto1" DataFormatString="{0:#,##0.00}"></asp:BoundField>
                        <asp:BoundField DataField="Potongan" HeaderText="Potongan" SortExpression="Potongan" DataFormatString="{0:#,##0.00}"></asp:BoundField>                        
                        <asp:BoundField DataField="Netto2" HeaderText="Netto2" SortExpression="Netto2" DataFormatString="{0:#,##0.00}"></asp:BoundField>                                               
                        <asp:BoundField DataField="Remark" HeaderStyle-Width="80px" HeaderText="Remark" ></asp:BoundField>
                        
                    </Columns>
                </asp:GridView>
          </div>   
          <asp:Button class="bitbtn btnadd" runat="server" ID="btnAddDt2" Text="Add" ValidationGroup="Input" />	     
          
       </asp:Panel>             
       <asp:Panel runat="server" ID="pnlEditDt" Visible="false">
       <table width="100%">
            <tr>
                <td style="width:60%">                
                    <table>
                        <tr>
                            <td>Item No</td>
                            <td>:</td>
                            <td><asp:Label ID="lbItemNo" runat="server" Text="itemmm" />
                            </td>           
                       </tr> 
                        <tr>
                            <td>
                                Nominal</td>
                            <td>
                                &nbsp;</td>
                            <td>
                                <table cellpadding="0" cellspacing="0">
                                    <tr style="background-color:Silver;text-align:center">
                                        <td>
                                            Timbang 1</td>
                                        <td>
                                            Timbang 2</td>
                                        <td>
                                            Netto 1</td>
                                        <td>
                                            Potongan </td>
                                        <td>
                                            Netto 2</td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="tbTimbang1" runat="server" CssClass="TextBox" 
                                                ValidationGroup="Input" Width="91px" />
                                        </td>
                                        <td>
                                            <asp:TextBox ID="tbTimbang2" runat="server" CssClass="TextBox" 
                                                ValidationGroup="Input" Width="91px" />
                                        </td>

                                        <td>
                                            <asp:TextBox ID="tbNetto1" runat="server" CssClass="TextBox" Height="16px" 
                                                ValidationGroup="Input" Width="91px" />
                                        </td>

                                        <td>
                                            <asp:TextBox ID="tbPotongan" runat="server" CssClass="TextBox" Height="16px" 
                                                ValidationGroup="Input" Width="91px" />
                                        </td>
                                        
                                        <td>
                                            <asp:TextBox ID="tbNetto2" runat="server" CssClass="TextBox" Height="16px" 
                                                ValidationGroup="Input" Width="91px" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Remark
                            </td>
                            <td>
                                :</td>
                            <td>
                                <asp:TextBox ID="tbRemarkDt" runat="server" CssClass="TextBox" Width="304px" 
                                    Height="31px" />
                            </td>
                        </tr>
                    </table>
                </td>
                <td style="vertical-align:top;width:40%">
                	&nbsp;</td>
            </tr>
       </table>
            <br />
            <asp:Button class="bitbtn btnsave" runat="server" ID="btnSaveDt" Text="Save" CommandName="Update" />																						 																											
									
			<asp:Button class="bitbtn btncancel" runat="server" ID="btnCancelDt" Text="Cancel" CommandName="Cancel" />																						 													
 
            <br />
       </asp:Panel> --%>
       <br />    
       
       
       <asp:Button ID="btnSaveAll" runat="server" class="bitbtndt btnsavenew" Text="Save & New" validationgroup="Input" Width = "90"/>									
       <asp:Button ID="btnSaveTrans" runat="server" class="bitbtndt btnsave" Text="Save" validationgroup="Input"/>									
       <asp:Button ID="btnBack" runat="server" class="bitbtndt btncancel" Text="Cancel" validationgroup="Input"/>									
       <asp:Button ID="btnHome" runat="server" class="bitbtndt btnback" Text="Home" />									                                             
       
     
    </asp:Panel>
    <asp:Panel runat="server" ID="pnlPrint" Visible="false">
    <cr:crystalreportviewer ID="CrystalReportViewer1" runat="server" AutoDataBind="True"
               Height="1036px" Width="928px" />
   <%-- <cc1:WebReport ID="WebReport1" runat="server" AutoHeight="True" 
            AutoWidth="True" Width="100%" Height = "100%" 
            ShowRefreshButton="False" />--%>
      <br />             
    </asp:Panel>               
    </div>   
    <asp:Label runat ="server" ID="lbStatus" ForeColor="Red"/>
    <asp:HiddenField ID="HiddenRemarkClose" runat="server" />
    <asp:HiddenField ID="HiddenRemarkDelete" runat="server" /> 
    
      <div class="loading" align="center">
      <br />
       <img src="../../Image/loader.gif" alt="" />
    </div>
       
    </form>                            
    </body>
</html>
