<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TrBeginLicenseAdm.aspx.vb" Inherits="BeginLicenseAdm" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<%@ Register Assembly="BasicFrame.WebControls.BasicDatePicker" Namespace="BasicFrame.WebControls"
    TagPrefix="BDP" %>

<%@ Register assembly="CrystalDecisions.Web, Version=10.5.3700.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Invoice Infrastruktur</title>
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

        function setformathd(prmchange) {
            try {



                document.getElementById("tbOriginalAmount").value = setdigit(document.getElementById("tbOriginalAmount").value.replace(/\$|\,/g, ""), '<%=ViewState("DigitHome")%>');
                document.getElementById("tbAmountUnallocated").value = setdigit(Math.floor(document.getElementById("tbAmountUnallocated").value.replace(/\$|\,/g, "")), '<%=ViewState("DigitHome")%>');
 

            } catch (err) {
                alert(err.description);
            }
        }


        function UploadInvoice(fileUploadInvoice) {
            if (fileUploadInvoice.value != '') {
                document.getElementById("<%=btnSaveINV.ClientID %>").click();
            }
        }

        function UploadFaktur(fileUploadFaktur) {
            if (fileUploadFaktur.value != '') {
                document.getElementById("<%=btnSaveFaktur.ClientID %>").click();
            }
        }

        function UploadBAP(fileUploadBAP) {
            if (fileUploadBAP.value != '') {
                document.getElementById("<%=btnSaveBAP.ClientID %>").click();
            }
        }

        function UploadLain(fileUploadLain) {
            if (fileUploadLain.value != '') {
                document.getElementById("<%=btnSaveLain.ClientID %>").click();
            }
        }


        function Confirm() {
            var confirm_value = document.createElement("INPUT");
            confirm_value.type = "hidden";
            confirm_value.name = "confirm_value";
            if (confirm("Do you want to upload the dokument ? \n\nClick (OK) to Upload or Click (CANCEL) to Save Data")) {
                confirm_value.value = "Yes";
            } else {
                confirm_value.value = "No";
            }
            document.forms[0].appendChild(confirm_value);
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
    <div class="H1">Begin License & ADM</div>
     <hr style="color:Blue" />        
    <asp:Panel runat="server" id="PnlHd"> 
      <table>
        <tr>
            <td style="width:100px;text-align:right">Quick Search :</td>            
            <td><asp:TextBox CssClass="TextBox" runat="server" ID ="tbFilter"/> 
                  <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlField" >
                       <asp:ListItem Selected="True" Value="TransNmbr">Refference No</asp:ListItem>
                            <asp:ListItem Value="dbo.FormatDate(TransDate)">Refference Date</asp:ListItem>
                            <asp:ListItem>Status</asp:ListItem>
                            <asp:ListItem Value="KegiatanCode">Kagiatan</asp:ListItem>
                            <asp:ListItem Value="KegiatanName">Kagiatan Name</asp:ListItem>
                            <asp:ListItem Value="Area">Area</asp:ListItem>
                            <asp:ListItem Value="AreaName">Area Name</asp:ListItem>
                            <asp:ListItem Value="NoBrks_Permohonan"> No Permohonan</asp:ListItem>
                            <asp:ListItem Value="AlasHak">Alas Hak</asp:ListItem>
                            <asp:ListItem>Remark</asp:ListItem>
                  </asp:DropDownList>
                  <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlRange"></asp:DropDownList>
                  <asp:Button class="bitbtn btnsearch" runat="server" ID="btnSearch" Text="Search" />                 
                  <asp:Button class="btngo" runat="server" ID="btnExpand" Text="..."/>             
            </td>
            
            
            <td>
                <asp:LinkButton ID="LbAdvSearch" runat="server" Text="Advanced Search" />
                        &nbsp &nbsp &nbsp Show :
                        <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlShowRecord" AutoPostBack="true">
                            <asp:ListItem Selected="True" Value="15">Choose One</asp:ListItem>
                            <asp:ListItem Value="20">20</asp:ListItem>
                            <asp:ListItem Value="30">30</asp:ListItem>
                            <asp:ListItem Value="40">40</asp:ListItem>
                            <asp:ListItem Value="50">50</asp:ListItem>
                            <asp:ListItem Value="100">100</asp:ListItem>
                        </asp:DropDownList>
                        Rows
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
                       <asp:ListItem Selected="True" Value="TransNmbr">Refference No</asp:ListItem>
                            <asp:ListItem Value="dbo.FormatDate(TransDate)">Refference Date</asp:ListItem>
                           <asp:ListItem>Status</asp:ListItem>
                            <asp:ListItem Value="KegiatanCode">Kagiatan</asp:ListItem>
                            <asp:ListItem Value="KegiatanName">Kagiatan Name</asp:ListItem>
                            <asp:ListItem Value="Area">Area</asp:ListItem>
                            <asp:ListItem Value="AreaName">Area Name</asp:ListItem>
                            <asp:ListItem Value="NoBrks_Permohonan"> No Permohonan</asp:ListItem>
                            <asp:ListItem Value="AlasHak">Alas Hak</asp:ListItem>
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
            CssClass="Grid" AutoGenerateColumns="false"  > 
              <HeaderStyle CssClass="GridHeader" Wrap="false" ></HeaderStyle>
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
                 <asp:BoundField DataField="TransNmbr" HeaderStyle-Width="120px" SortExpression="Nmbr" HeaderText="Refference"></asp:BoundField>                  
                      <asp:BoundField DataField="Status" SortExpression="Status" HeaderText="Status"></asp:BoundField>                  
                      <asp:BoundField DataField="TransDate" dataformatstring="{0:dd MMM yyyy}" htmlencode="true" HeaderStyle-Width="80px" SortExpression="TransDate" ItemStyle-wrap="true" HeaderText="Trans Date" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" > </asp:BoundField>
                      <asp:BoundField DataField="KegiatanCode" HeaderStyle-Width="200px" SortExpression="SuppCode" HeaderText="Kegiatan"></asp:BoundField>
                      <asp:BoundField DataField="KegiatanName" HeaderStyle-Width="200px" SortExpression="Supplier_Name" HeaderText="Kegiatan Name"></asp:BoundField>
                       <asp:BoundField DataField="AreaCode" HeaderStyle-Width="200px" SortExpression="SuppCode" HeaderText="Area Code"></asp:BoundField>
                      <asp:BoundField DataField="AreaName" HeaderStyle-Width="200px" SortExpression="Supplier_Name" HeaderText="Area Name"></asp:BoundField>
                      <asp:BoundField DataField="OriginalAmount" DataFormatString="{0:#,##0.00}" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="80px" SortExpression="OriginalAmount" HeaderText="Original Amount (Rp)"></asp:BoundField>
                      <asp:BoundField DataField="AllocAmount" DataFormatString="{0:#,##0.00}" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="80px" SortExpression="Unallocated Amount" HeaderText="Unallocated Amount (Rp)"></asp:BoundField>
                      <asp:BoundField DataField="Remark" HeaderStyle-Width="250px" SortExpression="Remark" HeaderText="Remark"></asp:BoundField>
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
                <td>
                     <asp:Menu ID="Menu2" runat="server" CssClass="Menu" ItemWrap="False" 
                Orientation="Horizontal" StaticEnableDefaultPopOutImage="False" 
                StaticMenuItemStyle-CssClass="MenuItem" 
                StaticSelectedStyle-CssClass="MenuSelect">
                <StaticSelectedStyle CssClass="MenuSelect" />
                <StaticMenuItemStyle CssClass="MenuItem" />
                <Items>
                    <asp:MenuItem Text="General Input" Value="0"></asp:MenuItem>
                    <asp:MenuItem Text="Upload Dokumen" Value="1"></asp:MenuItem>
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
             <td>Reference No</td>
                        <td>:</td>
                        <td><asp:TextBox CssClass="TextBoxR" runat="server" ID="tbCode" Width="225px" Enabled="False"/>
                        </td>  
            <td>Tanggal Registrasi</td>
                   <td>:</td>
                   <td>
                   <BDP:BasicDatePicker ID="tbDateRegistrasi" runat="server"  DateFormat="dd MMM yyyy" 
                                    ReadOnly = "true" ValidationGroup="Input"
                                    ButtonImageHeight="19px" ButtonImageWidth="20px" 
                                    DisplayType="TextBoxAndImage" 
                                    TextBoxStyle-CssClass="TextDate" AutoPostBack="True" 
                                    ShowNoneButton="False"><TextBoxStyle CssClass="TextDate" Width="205px" /></BDP:BasicDatePicker> </td>            
                                  
        </tr>
         <tr>
                   <td>Refference Date</td>
                   <td>:</td>
                   <td>
                   <BDP:BasicDatePicker ID="tbDate" runat="server"  DateFormat="dd MMM yyyy" 
                                    ReadOnly = "true" ValidationGroup="Input"
                                    ButtonImageHeight="19px" ButtonImageWidth="20px" 
                                    DisplayType="TextBoxAndImage" 
                                    TextBoxStyle-CssClass="TextDate" AutoPostBack="True" 
                                    ShowNoneButton="False"><TextBoxStyle CssClass="TextDate" Width="205px" /></BDP:BasicDatePicker> </td>
                                    
                <td>No Berkas Permohonan</td>
                <td>:</td>
                <td><asp:TextBox CssClass="TextBox" runat="server" ValidationGroup="Input"  MaxLength = "30" ID="tbNoPermohonan" Width="225px" Enabled="True"/>        
                </td>
                        
        </tr>
        
        <tr>
            <td><asp:LinkButton ID="lbKegiatan" ValidationGroup="Input" runat="server" Text="Kegiatan"/></td>
            <td>:</td>
            <td>
                <asp:TextBox CssClass="TextBox" ValidationGroup="Input" runat="server" ID="tbKegiatanCode" Visible = "false" Width="100px" AutoPostBack="true" />
                <asp:TextBox CssClass="TextBox" runat="server" ID="tbKegiatanName" Enabled="false" Width="225Px"/>
                <asp:Button Class="btngo" ID="btnKegiatan" Text="..." runat="server" ValidationGroup="Input" />                                  
            </td>
            
            
         
         
         <td>Tgl Berkas Permohonan</td>
                   <td>:</td>
                   <td>
                   <BDP:BasicDatePicker ID="tbDatePermohonan" runat="server"  DateFormat="dd MMM yyyy" 
                                    ReadOnly = "true" ValidationGroup="Input"
                                    ButtonImageHeight="19px" ButtonImageWidth="20px" 
                                    DisplayType="TextBoxAndImage" 
                                    TextBoxStyle-CssClass="TextDate" AutoPostBack="True" 
                                    ShowNoneButton="False"><TextBoxStyle CssClass="TextDate" Width="205px" /></BDP:BasicDatePicker> </td>                              
        </tr>
        
          <tr>
            <td><asp:LinkButton ID="lbArea" ValidationGroup="Input" runat="server" Text="Area"/></td>
            <td>:</td>
            <td>
                <asp:TextBox CssClass="TextBox" ValidationGroup="Input" runat="server" ID="tbAreaCode" Visible = "false" Width="100px" AutoPostBack="true" />
                <asp:TextBox CssClass="TextBox" runat="server" ID="tbAreaName" Enabled="false" Width="225Px"/>
                <asp:Button Class="btngo" ID="btnArea" Text="..." runat="server" ValidationGroup="Input" />                                  
            </td>
            
            
         
         
         <td>Alas HAK</td>
            <td>:</td>
            <td><asp:TextBox ID="tbAlasHak" ValidationGroup="Input" runat="server" CssClass="TextBox" width="225Px"/></td>
                                       
        </tr>
        
           <tr>
            <td>Pejabat Terkait</td>
            <td>:</td>
            <td><asp:TextBox ID="tbPejabat" ValidationGroup="Input" runat="server" CssClass="TextBox" width="225Px"/></td>
            
            
         
         
         <td>Perantara</td>
                <td>:</td>
                <td><asp:TextBox ID="tbPerantara" ValidationGroup="Input" runat="server" CssClass="TextBox" width="225Px"/></td>
                                                  
        </tr> 
        
        

        
        <tr>
            <td>No Telp Pejabat Terkait</td>
            <td>:</td>
            <td><asp:TextBox ID="tbTelpPejabat" ValidationGroup="Input" runat="server" CssClass="TextBox" width="225Px"/></td>                         
            
            
         
         
         <td>No Telp Perantara</td>
                <td>:</td>
                <td><asp:TextBox ID="tbTelpPerantara" ValidationGroup="Input" runat="server" CssClass="TextBox" width="225Px"/></td>                                
        </tr>
        
          <tr>
            <td>PIC</td>
            <td>:</td>
            <td><asp:TextBox ID="tbPIC" ValidationGroup="Input" runat="server" CssClass="TextBox" width="225Px"/></td>                         
                              
        </tr>
        
        
       <tr>

        <td>CIP</td>
                        <td>:</td>
                        <td>
                        <asp:DropDownList ID="ddlCIP" runat="server" Width = "230px" CssClass="DropDownList" ValidationGroup="Input"> <%--AutoPostBack="true"--%>
                          <asp:ListItem Selected="True">Izin</asp:ListItem>
                          <asp:ListItem>Administrasi</asp:ListItem>
                      </asp:DropDownList>
                        </td>
        <td>SPS Date</td>
                   <td>:</td>
                   <td>
                   <BDP:BasicDatePicker ID="tbSPSDate" runat="server"  DateFormat="dd MMM yyyy" 
                                    ReadOnly = "true" ValidationGroup="Input"
                                    ButtonImageHeight="19px" ButtonImageWidth="20px" 
                                    DisplayType="TextBoxAndImage" 
                                    TextBoxStyle-CssClass="TextDate" AutoPostBack="True" 
                                    ShowNoneButton="False"><TextBoxStyle CssClass="TextDate" Width="205px" /></BDP:BasicDatePicker> </td>                 
        </tr> 
        
        
        
        
        <tr>
            <td>No Dokumen 1</td>                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                     
            <td>:</td>
            <td><asp:TextBox ID="tbNoDok1" ValidationGroup="Input" runat="server" CssClass="TextBox" width="225Px"/></td>                         
            
            
         
         
         <td>No Dokumen 2</td>
                <td>:</td>
                <td><asp:TextBox ID="tbNoDok2" ValidationGroup="Input" runat="server" CssClass="TextBox" width="225Px"/></td>                                
        </tr>
        
        
         <tr>
                            <td>Nominal</td>
                            <td>:</td>
                            <td colspan="7">
                                <table>
                                    <tr style="background-color:Silver;text-align:center; border-radius:30px;">
                                        <td><asp:LinkButton ID="lbCurr" ValidationGroup="Input" runat="server" Text="Currency"/></td>
                                        <td>Original Amount</td>
                                        <td>Unallocated Amoount</td>
                                    </tr>
                                    <tr>
                                     
                                        <td><asp:DropDownList ID="ddlCurr" ValidationGroup="Input" AutoPostBack="true" Width="142px" runat="server" CssClass="DropDownList" />                                                                   
                                         Rate :  <asp:TextBox ID="tbRate" ValidationGroup="Input" runat="server" CssClass="TextBox" Width="50px" />
                                        </td>
                                        <td><asp:TextBox ID="tbOriginalAmount" ValidationGroup="Input" runat="server" CssClass="TextBox" width="170px"/></td>
                                        <td><asp:TextBox ID="tbAmountUnallocated" ValidationGroup="Input" runat="server" CssClass="TextBox" width="170px"/></td>
                                       
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
        </asp:View>
       
       <asp:View ID="TabHd1" runat="server">
            <table>
                <tr>
                <td><asp:Button class="bitbtndt btndelete" OnClientClick="return confirm('Sure to delete this dokumen?');" runat="server" ID="btnClearInv" Width="15px" Text="s" /> Upload Dokumen 1</td>
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
                <tr></tr>
                <tr>
                <td><asp:Button class="bitbtndt btndelete" OnClientClick="return confirm('Sure to delete this dokumen?');" runat="server" ID="btnClearFaktur" Width="15px" Text="s" /> Upload Dokumen 2</td>
                        <td>:</td>
                  <td> 
                     <asp:FileUpload runat="server" style="color: White;" accept="application/pdf" ID="FubFaktur"  />    
                     <asp:Button ID="btnsaveFaktur" CssClass="bitbtndt btnadd" runat="server" Text="View" Style="display: none" />        
                  </td> 
                  <td>        
                    <asp:LinkButton ID="lbFaktur" ValidationGroup="Input" runat="server" Text="Not Yet Uploaded"/>
                            <%--<asp:Button ID="btnsaveKtpW" CssClass="bitbtndt btnadd" runat="server"  Style="display: none" Text="View" /> --%>
                  </td>
                </tr>
                <tr></tr>
                <tr> 
                <td><asp:Button class="bitbtndt btndelete" Visible = "false"  OnClientClick="return confirm('Sure to delete this dokumen?');" runat="server" ID="btnClearBAP" Width="15px" Text="s" /></td>
                       <%-- <td>:</td>--%>
                 
                  <td> 
                     <asp:FileUpload runat="server" Visible = "false"  style="color: White;" accept="application/pdf" ID="FubBAPExt"  /> 
                     <asp:Button ID="btnsaveBAP" Visible = "false"  CssClass="bitbtndt btnadd" runat="server" Text="View" Style="display: none" />              
                  </td> 
                  <td>        
                    <asp:LinkButton ID="lbBAP" Visible = "false"  ValidationGroup="Input" runat="server" Text="Not Yet Uploaded"/>
                            <%--<asp:Button ID="btnsaveKtpW" CssClass="bitbtndt btnadd" runat="server"  Style="display: none" Text="View" /> --%>
                  </td>
                </tr>        
                        
                <tr></tr>
                
                <tr>
                <td><asp:Button class="bitbtndt btndelete" Visible = "false" OnClientClick="return confirm('Sure to delete this dokumen?');" runat="server" ID="btnClearLain" Width="15px" Text="s" /> </td> 
                        <%--<td>:</td>--%>
                 
                   <td>
                     
                     <asp:FileUpload runat="server" style="color: White;" Visible = "false" accept="application/pdf" ID="FubDokLain"  />
                     <asp:Button ID="btnsaveLain" CssClass="bitbtndt btnadd" runat="server" Visible = "false"  Text="View" Style="display: none" />               
                  </td>  
                  
                  <td>        
                    <asp:LinkButton ID="lbDokLain" Visible = "false" ValidationGroup="Input" runat="server" Text="Not Yet Uploaded"/>
                            <%--<asp:Button ID="btnsaveKtpW" CssClass="bitbtndt btnadd" runat="server"  Style="display: none" Text="View" /> --%>
                  </td>            
                </tr>
            </table>
       </asp:View>
       
    </asp:MultiView>
  
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
                <asp:MenuItem Text="Detail Item" Value="0"></asp:MenuItem>
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
            <%--<asp:Button ID="btnGetBAP" runat="server" class="bitbtn btnsearch" Text="Get BAP" validationgroup="Input"/>--%>
            <div style="border:0px  solid; width:100%; height:100%; overflow:auto;">
                <asp:GridView ID="GridDt" runat="server" AutoGenerateColumns="False" 
                    ShowFooter="False">
                    <HeaderStyle CssClass="GridHeader" />
                    <RowStyle CssClass="GridItem" Wrap="false" />
                    <AlternatingRowStyle CssClass="GridAltItem" />
                    <PagerStyle CssClass="GridPager" />
                     <Columns>
                            <asp:TemplateField HeaderText="Action"><ItemTemplate><asp:Button ID="btnEdit" runat="server" class="bitbtndt btnedit" Text="Edit" CommandName="Edit"/><asp:Button ID="btnDelete" runat="server" class="bitbtndt btndelete" Text="Delete" OnClientClick="return confirm('Sure to delete this data?');" CommandName="Delete"/></ItemTemplate></asp:TemplateField>
                            <asp:BoundField DataField="ItemNo" ItemStyle-HorizontalAlign="Right" HeaderText="Item No" /> <%--DataFormatString="{0:#}" --%>
                            <asp:BoundField DataField="JobName" HeaderStyle-Width="100px" HeaderText="Uraian Pekerjaan" ItemStyle-HorizontalAlign="Left" />
                            <asp:BoundField DataField="Qty" DataFormatString="{0:#,##0}" HeaderStyle-Width="40px" HeaderText="Jumlah" ItemStyle-HorizontalAlign="Right" />
                            <asp:BoundField DataField="UnitCode" HeaderStyle-Width="50px" HeaderText="Satuan" ItemStyle-HorizontalAlign="Left" />
                            <asp:BoundField DataField="PriceForex" DataFormatString="{0:#,##0}" HeaderStyle-Width="60px" HeaderText="Harga/Sat" ItemStyle-HorizontalAlign="Right" />
                            <asp:BoundField DataField="LocationName" HeaderStyle-Width="100px" HeaderText="Tempat Pengajuan" ItemStyle-HorizontalAlign="Left" />                            
                            <asp:BoundField DataField="BeaForex" DataFormatString="{0:#,##0}" HeaderStyle-Width="90px" ItemStyle-HorizontalAlign="Right" HeaderText="Biaya" />
                            <asp:BoundField DataField="Remark" HeaderStyle-Width="300px" HeaderText="Remark" ItemStyle-HorizontalAlign="Left" />
                            
                        </Columns>
                </asp:GridView>
          </div>   
          <asp:Button class="bitbtndt btnadd" runat="server" ID="btnAddDtKe2" Text="Add" ValidationGroup="Input" />
      </asp:Panel>  
               <br />
               <asp:Panel ID="pnlEditDt" runat="server" Visible="false">
                   <table>
                        <tr>
                        <td>Item No</td>
                        <td>:</td>
                        <td><asp:Label ID="lbItemNo" runat="server" Text="item" /> <%--<td colspan="4" --%>
                        </td>           
                    </tr>   
                    <tr> 
                        <td>Uraian Pekerjaan</td>
                        <td>:</td>
                        <td>
                          <asp:TextBox runat="server" ID="tbJobName" CssClass="TextBox" Width="365px" MaxLength="255" ValidationGroup="Input" AutoPostBack="False" />                         
                      
                    </tr>                       
                    <tr> 
                        <td>Jumlah</td>
                        <td>:</td>
                        <td>                            
                            <asp:TextBox ID="tbQty" runat="server" CssClass="TextBox" Width="50px" ValidationGroup="Input" AutoPostBack="False"/>                                    
                                                            
                        </td>                        
                    </tr>
                    <tr>
                        <td>Satuan</td>
                        <td>:</td>
                        <td><asp:DropDownList ID="ddlUnit" runat="server" CssClass="DropDownList" Width="100px"/></td>
                    </tr>
                    <tr> 
                        <td>Harga per Unit</td>
                        <td>:</td>
                        <td>                            
                            <asp:TextBox ID="tbPriceForex" runat="server" CssClass="TextBox" Width="100px" ValidationGroup="Input" AutoPostBack="False"/>                                    
                        </td>                        
                    </tr>                                        
                    <tr> 
                        <td>Tempat Pengajuan</td>
                        <td>:</td>
                        <td>                            
                            <%--<asp:TextBox ID="tbLocationCode" runat="server" CssClass="TextBox" Width="80px" ValidationGroup="Input" AutoPostBack="true"/>--%>                                    
                            <asp:TextBox ID="tbLocationName" runat="server" CssClass="TextBox" Width="250px" ValidationGroup="Input"/>   
                            <%--<asp:Button ID="btnLocation" runat="server" Class="btngo" Text="..." ValidationGroup="Input" />--%>                                                             
                        </td>                        
                    </tr>                    
                    <tr>
                        <td>Biaya</td>
                        <td>:</td>
                        <td>                                
                            <asp:TextBox ID="tbTotalBiaya" runat="server" CssClass="TextBox" Width="100px" ValidationGroup="Input" AutoPostBack="False" />
                                                      
                        </td>
                    </tr>                         
                 
                    <tr>
                        <td>Remark</td>
                        <td>:</td>
                        <td><asp:TextBox runat="server" ID="tbRemarkDt" CssClass="TextBox" Width="365px" 
                                MaxLength="255" TextMode="MultiLine" />                        
                        </td>
                    </tr>    
                   </table>
                   <br />   
                   <asp:Button class="bitbtndt btnsave" runat="server" ID="btnSaveDt"  Text="Save" ValidationGroup="Input" />
                   <asp:Button class="bitbtndt btncancel" runat="server" ID="btnCancelDt" Text="Cancel" ValidationGroup="Input" />
               </asp:Panel>
             </asp:View>           
            
        </asp:MultiView>
    
       <br />    
        <asp:Button class="bitbtn btnsavenew" runat="server" ID="btnSaveAll" 
            Text="Save & New" ValidationGroup="Input" Width="103px" />
        <asp:Button class="bitbtn btnsave" runat="server" ID="btnSaveTrans" OnClientClick="Confirm()" Text="Save" ValidationGroup="Input" />
        <asp:Button class="bitbtn btnback" runat="server" ID="btnBack" Text="Cancel" ValidationGroup="Input" />
        <asp:Button class="bitbtndt btnback" runat="server" ID="btnHome" Text="Home" />
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
