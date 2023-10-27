<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TrPesanan.aspx.vb" Inherits="TrPemesanan" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<%@ Register Assembly="BasicFrame.WebControls.BasicDatePicker" Namespace="BasicFrame.WebControls"
    TagPrefix="BDP" %>

<%@ Register assembly="CrystalDecisions.Web, Version=10.5.3700.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Surat Pesanan</title>
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

            try 
            {

                document.getElementById("tbTotDisc").value = parseFloat(document.getElementById("tbTotHarga").value.replace(/\$|\,/g, "")) * parseFloat(document.getElementById("tbDiscHd").value.replace(/\$|\,/g, "")) / 100;
//                
                document.getElementById("tbTotDPP").value = parseFloat(document.getElementById("tbTotHarga").value.replace(/\$|\,/g, "")) - parseFloat(document.getElementById("tbTotDisc").value.replace(/\$|\,/g, ""));

                document.getElementById("tbTotAmount").value = parseFloat(document.getElementById("tbTotDPP").value.replace(/\$|\,/g, "")) + parseFloat(Math.floor(document.getElementById("tbTotPPN").value.replace(/\$|\,/g, "")));
                document.getElementById("tbTotAmount").value = parseFloat(document.getElementById("tbTotAmount").value.replace(/\$|\,/g, "")) - parseFloat(Math.floor(document.getElementById("tbTotPPH").value.replace(/\$|\,/g, "")));
                          

                document.getElementById("tbTotHarga").value = setdigit(document.getElementById("tbTotHarga").value.replace(/\$|\,/g, ""), '<%=ViewState("DigitHome")%>');
                document.getElementById("tbDiscHd").value = setdigit(Math.floor(document.getElementById("tbDiscHd").value.replace(/\$|\,/g, "")), '<%=ViewState("DigitHome")%>');
                document.getElementById("tbTotDisc").value = setdigit(Math.floor(document.getElementById("tbTotDisc").value.replace(/\$|\,/g, "")), '<%=ViewState("DigitHome")%>');
                document.getElementById("tbTotDPP").value = setdigit(document.getElementById("tbTotDPP").value.replace(/\$|\,/g, ""), '<%=ViewState("DigitHome")%>');

                document.getElementById("tbTotPPN").value = setdigit(document.getElementById("tbTotPPN").value.replace(/\$|\,/g, ""), '<%=ViewState("DigitHome")%>');
                document.getElementById("tbTotPPH").value = setdigit(document.getElementById("tbTotPPH").value.replace(/\$|\,/g, ""), '<%=ViewState("DigitHome")%>');
                document.getElementById("tbTotAmount").value = setdigit(document.getElementById("tbTotAmount").value.replace(/\$|\,/g, ""), '<%=ViewState("DigitHome")%>');
          
                

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



        function setformatdt2(change) {
            try 
            {


                document.getElementById("tbPpnValuedt2").value = Math.floor(parseFloat(document.getElementById("tbNominal").value.replace(/\$|\,/g, "")) * parseFloat(document.getElementById("tbPpnDt2").value.replace(/\$|\,/g, "")) / 100);

                document.getElementById("tbPphValueDt2").value = Math.floor(parseFloat(document.getElementById("tbNominal").value.replace(/\$|\,/g, "")) * parseFloat(document.getElementById("tbPphDt2").value.replace(/\$|\,/g, "")) / 100);

                document.getElementById("tbTotalNominal").value = parseFloat(document.getElementById("tbNominal").value.replace(/\$|\,/g, "")) + parseFloat(Math.floor(document.getElementById("tbPpnValuedt2").value.replace(/\$|\,/g, ""))) - parseFloat(Math.floor(document.getElementById("tbPphValueDt2").value.replace(/\$|\,/g, "")));



                document.getElementById("tbPphDt2").value = setdigit(document.getElementById("tbPphDt2").value.replace(/\$|\,/g, ""), '<%=ViewState("DigitHome")%>');
                document.getElementById("tbPpnDt2").value = setdigit(Math.floor(document.getElementById("tbPpnDt2").value.replace(/\$|\,/g, "")), '<%=ViewState("DigitHome")%>');
                document.getElementById("tbNominal").value = setdigit(Math.floor(document.getElementById("tbNominal").value.replace(/\$|\,/g, "")), '<%=ViewState("DigitHome")%>');

                document.getElementById("tbTotalNominal").value = setdigit(document.getElementById("tbTotalNominal").value.replace(/\$|\,/g, ""), '<%=ViewState("DigitHome")%>');

                document.getElementById("tbPpnValuedt2").value = setdigit(document.getElementById("tbPpnValuedt2").value.replace(/\$|\,/g, ""), '<%=ViewState("DigitHome")%>');

                document.getElementById("tbPphValueDt2").value = setdigit(document.getElementById("tbPphValueDt2").value.replace(/\$|\,/g, ""), '<%=ViewState("DigitHome")%>');


            } catch (err) {
                alert(err.description);
            }
        }



        function setformatdtppn2(change) {
            try {


//                document.getElementById("tbPpnValuedt2").value = Math.floor(parseFloat(document.getElementById("tbNominal").value.replace(/\$|\,/g, "")) * parseFloat(document.getElementById("tbPpnDt2").value.replace(/\$|\,/g, "")) / 100);

//                document.getElementById("tbPphValueDt2").value = Math.floor(parseFloat(document.getElementById("tbNominal").value.replace(/\$|\,/g, "")) * parseFloat(document.getElementById("tbPphDt2").value.replace(/\$|\,/g, "")) / 100);

                document.getElementById("tbTotalNominal").value = parseFloat(document.getElementById("tbNominal").value.replace(/\$|\,/g, "")) + parseFloat(Math.floor(document.getElementById("tbPpnValuedt2").value.replace(/\$|\,/g, ""))) - parseFloat(Math.floor(document.getElementById("tbPphValueDt2").value.replace(/\$|\,/g, "")));

                document.getElementById("tbRemarkDt2").focus();

                document.getElementById("tbPphDt2").value = setdigit(document.getElementById("tbPphDt2").value.replace(/\$|\,/g, ""), '<%=ViewState("DigitHome")%>');
                document.getElementById("tbPpnDt2").value = setdigit(Math.floor(document.getElementById("tbPpnDt2").value.replace(/\$|\,/g, "")), '<%=ViewState("DigitHome")%>');
                document.getElementById("tbNominal").value = setdigit(Math.floor(document.getElementById("tbNominal").value.replace(/\$|\,/g, "")), '<%=ViewState("DigitHome")%>');

                document.getElementById("tbTotalNominal").value = setdigit(document.getElementById("tbTotalNominal").value.replace(/\$|\,/g, ""), '<%=ViewState("DigitHome")%>');

                document.getElementById("tbPpnValuedt2").value = setdigit(document.getElementById("tbPpnValuedt2").value.replace(/\$|\,/g, ""), '<%=ViewState("DigitHome")%>');

                document.getElementById("tbPphValueDt2").value = setdigit(document.getElementById("tbPphValueDt2").value.replace(/\$|\,/g, ""), '<%=ViewState("DigitHome")%>');


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



        function Reject() {
            try {
                var result = prompt("Remark Cancel", "");
                if (result) {
                    document.getElementById("HiddenRemarkReject").value = result;
                } else {
                    document.getElementById("HiddenRemarkReject").value = "False Value";
                }
                postback();
                //document.form1.submit();                
            } catch (err) {
                alert(err.description);
            }
        }

        function postback() {
            __doPostBack('', '');
        }
        
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
    <div class="H1">Surat Pesanan</div>
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
                            <asp:ListItem Value="CustomerCode">Customer</asp:ListItem>
                            <asp:ListItem Value="CustomerName">Customer Name</asp:ListItem>
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
                       <asp:ListItem Selected="True" Value="TransNmbr">Refference No</asp:ListItem>
                            <asp:ListItem Value="dbo.FormatDate(TransDate)">Refference Date</asp:ListItem>
                            <asp:ListItem>Status</asp:ListItem>
                            <asp:ListItem Value="CustomerCode">Customer</asp:ListItem>
                            <asp:ListItem Value="CustomerName">Customer Name</asp:ListItem>
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
                              <%--<asp:ListItem Text="Tanda Terima" />--%>
                              <asp:ListItem Text="Kwitansi Global" />
                              <asp:ListItem Text="Cancel" />
                              
                          </asp:DropDownList>
                          <asp:Button class="btngo" runat="server" ID="BtnGo" Text="G" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" CommandName="Go"/> 
                      </ItemTemplate>
                      <HeaderStyle Width="110px" />
                  </asp:TemplateField>
                 <asp:BoundField DataField="TransNmbr" HeaderStyle-Width="120px" SortExpression="Nmbr" HeaderText="No Pesanan"></asp:BoundField>                  
                      <asp:BoundField DataField="Status" SortExpression="Status" HeaderText="Status"></asp:BoundField>                  
                      <asp:BoundField DataField="TransDate" dataformatstring="{0:dd MMM yyyy}" htmlencode="true" HeaderStyle-Width="80px" SortExpression="TransDate" ItemStyle-wrap="true" HeaderText="Pesanan Date" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" > </asp:BoundField>
                      <asp:BoundField DataField="CustomerCode" HeaderStyle-Width="200px" SortExpression="CustomerCode" HeaderText="Customer"></asp:BoundField>
                      <asp:BoundField DataField="CustomerName" HeaderStyle-Width="200px" SortExpression="CustomerName" HeaderText="Customer Name"></asp:BoundField>
                      <asp:BoundField DataField="SPNo" HeaderStyle-Width="150px" SortExpression="SPNo" HeaderText="SP No"></asp:BoundField>
                      <asp:BoundField DataField="TotalHarga" DataFormatString="{0:#,##0.00}" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="80px" SortExpression="TotalHarga" HeaderText="Total Harga"></asp:BoundField>
                      <asp:BoundField DataField="TotalDisc" DataFormatString="{0:#,##0.00}" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="50px" SortExpression="TotalDisc" HeaderText="Total Disc"></asp:BoundField>
                      <asp:BoundField DataField="TotalDPP" DataFormatString="{0:#,##0.00}" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="50px" SortExpression="TotalDPP" HeaderText="Total DPP"></asp:BoundField>
                      <asp:BoundField DataField="TotalPPN" DataFormatString="{0:#,##0.00}" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="80px" SortExpression="TotalPPN" HeaderText="Total PPN"></asp:BoundField>
                      <asp:BoundField DataField="TotalPPH" DataFormatString="{0:#,##0.00}" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="80px" SortExpression="TotalPPH" HeaderText="Total PPH"></asp:BoundField>
                      <asp:BoundField DataField="TotalAmount" DataFormatString="{0:#,##0.00}" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="80px" SortExpression="TotalAmount" HeaderText="Total Amount"></asp:BoundField>
                      <asp:BoundField DataField="Remark" HeaderStyle-Width="250px" SortExpression="Remark" HeaderText="Remark"></asp:BoundField>
                      <asp:BoundField DataField="RemarkReject" HeaderStyle-HorizontalAlign ="Center" HeaderStyle-Width="250px" 
                            HeaderText="Remark Reject" SortExpression="RemarkReject">
                            <HeaderStyle Width="250px" />
                        </asp:BoundField>            
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
                    <asp:MenuItem Text="Form Input Pesanan" Value="0"></asp:MenuItem>
                    <%--Kententuan Kawasan & Kesepakatan Membangun--%> 
                    <asp:MenuItem Text="" Value="1"></asp:MenuItem>
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
            <td>    Date</td>
                   <td>:</td>
                   <td>
                   <BDP:BasicDatePicker ID="tbDate" runat="server"  DateFormat="dd MMM yyyy" 
                                    ReadOnly = "true" ValidationGroup="Input"
                                    ButtonImageHeight="19px" ButtonImageWidth="20px" 
                                    DisplayType="TextBoxAndImage" 
                                    TextBoxStyle-CssClass="TextDate" AutoPostBack="True" 
                                    ShowNoneButton="False"><TextBoxStyle CssClass="TextDate" Width="205px" /></BDP:BasicDatePicker> </td>            
                                  
        </tr>
         <tr>
                   <td>SP No</td>
                   <td>:</td>
                   <td>
                <asp:TextBox CssClass="TextBox" ValidationGroup="Input" runat="server" ID="tbSpNo" Width="225px" AutoPostBack="true" />
                <asp:Button Class="btngo" ID="btnSP" Text="..." runat="server" ValidationGroup="Input" />                                  
                 </td>
                                    
                <td>Fg Sewa</td>
                <td>:</td>
                <td>
               <asp:DropDownList CssClass="DropDownList" ValidationGroup="Input" AutoPostBack = "true" runat="server" Width="230px" ID="ddlFgsewa" >
                                <asp:ListItem Selected="True" >N</asp:ListItem>
                                <asp:ListItem >Y</asp:ListItem>
                      </asp:DropDownList>
                </td>
                        
        </tr>
        
        
        <tr>
            <td><asp:LinkButton ID="lbCust" Visible="True"  ValidationGroup="Input"  runat="server" Text="Pembeli"/></td>
            <td>:</td>
            <td>
                <asp:TextBox CssClass="TextBox" ValidationGroup="Input" runat="server" ID="tbCustomerCode" Width="100px" AutoPostBack="true" />
                <asp:TextBox CssClass="TextBox" runat="server" ID="tbCustomerName" Enabled="false" Width="190px"/>
                <asp:Button Class="btngo" ID="btnCust" Text="..." runat="server" ValidationGroup="Input" />                                  
            </td>
         
         
         <td>Department</td>
                <td>:</td>
                <td><asp:DropDownList CssClass="DropDownList" ValidationGroup="Input" Width="230px" runat="server" ID="ddlDept" AutoPostBack="False" />
                </td>                              
        </tr> 
        
        
         <tr>
            <td>Area<asp:LinkButton ID="lbArea" Visible="false" ValidationGroup="Input"  runat="server" Text="Area"/></td>
            <td>:</td>
            <td>
                <asp:TextBox CssClass="TextBox" ValidationGroup="Input" runat="server" ID="tbArea" Width="100px" AutoPostBack="true" />
                <asp:TextBox CssClass="TextBox" runat="server" ID="tbAreaName" Enabled="false" Width="190px"/>
                <asp:Button Class="btngo" ID="btnArea" Text="..." runat="server" ValidationGroup="Input" />                                  
            </td>
         
         
         <td>Po Customer</td>
                <td>:</td>
                <td><asp:TextBox CssClass="TextBox" ValidationGroup="Input" runat="server" ID="tbPoCust" Width="100px" AutoPostBack="true" />
                </td>                              
        </tr>
        
        <tr>
                            <td>PIC Pembeli</td>
                            <td>:</td>
                            <td colspan="7">
                                <table>
                                    <tr style="background-color:Silver;text-align:center; border-radius:30px;">
                                        <td>Nama</td>
                                        <td>No Telp</td>
                                        <td>Email</td>                                        
                                    </tr>
                                    <tr>
                                        <td><asp:TextBox ID="tbPICName" ValidationGroup="Input" runat="server" CssClass="TextBox" width="150px"/></td>
                                        <td><asp:TextBox ID="tbPICTelp" ValidationGroup="Input" runat="server" CssClass="TextBox" width="150px"/></td>
                                        <td><asp:TextBox ID="tbPICEmail"  ValidationGroup="Input" runat="server" CssClass="TextBox" Width="150px"  /></td>
                                       
                                    </tr>
                                    <tr>
                                        <td><asp:TextBox ID="tbPICName2" ValidationGroup="Input" runat="server" CssClass="TextBox" width="150px"/></td>
                                        <td><asp:TextBox ID="tbPICTelp2" ValidationGroup="Input" runat="server" CssClass="TextBox" width="150px"/></td>
                                        <td><asp:TextBox ID="tbPICEmail2"  ValidationGroup="Input" runat="server" CssClass="TextBox" Width="150px"  /></td>
                                       
                                    </tr>
                                </table>
                            </td>                
                    </tr> 
              
        
        
        
                <tr>
                            <td>Perantara</td>
                            <td>:</td>
                            <td colspan="7">
                                <table>
                                    <tr style="background-color:Silver;text-align:center; border-radius:30px;">
                                        <td>Nama</td>
                                        <td>No Telp</td>
                                        <td>Email</td>                                        
                                    </tr>
                                    <tr>
                                        <td><asp:TextBox ID="tbPeName" ValidationGroup="Input" runat="server" CssClass="TextBox" width="150px"/></td>
                                        <td><asp:TextBox ID="tbPeTelp" ValidationGroup="Input" runat="server" CssClass="TextBox" width="150px"/></td>
                                        <td><asp:TextBox ID="tbPeEmail"  ValidationGroup="Input" runat="server" CssClass="TextBox" Width="150px"  /></td>
                                       
                                    </tr>
                                    
                                    <tr>
                                        <td><asp:TextBox ID="tbPeName2" ValidationGroup="Input" runat="server" CssClass="TextBox" width="150px"/></td>
                                        <td><asp:TextBox ID="tbPeTelp2" ValidationGroup="Input" runat="server" CssClass="TextBox" width="150px"/></td>
                                        <td><asp:TextBox ID="tbPeEmail2"  ValidationGroup="Input" runat="server" CssClass="TextBox" Width="150px"  /></td>
                                       
                                    </tr>
                                </table>
                            </td>                
                    </tr> 
         <tr> 
        
        
           <td><asp:LinkButton ID="lbCurr" ValidationGroup="Input" runat="server" Text="Currency"/></td>
                <td>:</td>
                <td><asp:DropDownList ID="ddlCurr" ValidationGroup="Input" AutoPostBack="true" Width="142px" runat="server" CssClass="DropDownList" />                                                                   
                 Rate :  <asp:TextBox ID="tbRate" ValidationGroup="Input" runat="server" CssClass="TextBox" Width="50px" />
                </td> 
                
                
         <td><asp:LinkButton ID="lbTerm" visible ="false" ValidationGroup="Input" runat="server" Text="Term"/></td>
            <td></td>
            <td><asp:DropDownList visible ="false"  CssClass="DropDownList" ValidationGroup="Input" Width="142px" runat="server" ID="ddlTerm" AutoPostBack="true" /> 
                <BDP:BasicDatePicker visible ="false"  ID="tbDueDate" runat="server" DateFormat="dd MMM yyyy" 
                        ReadOnly = "true" Enabled="false"
                        ButtonImageHeight="19px" ButtonImageWidth="20px" 
                        DisplayType="TextBox" 
                        TextBoxStyle-CssClass="TextDate" AutoPostBack="True" 
                        ShowNoneButton="False"><TextBoxStyle CssClass="TextDate" /></BDP:BasicDatePicker>
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
                                            <td>Total DPP</td> 
                                            <td>Total PPN</td>
                                            <td>Total PPH</td>                                           
                                            
                                            <td>Total Amount</td>
                                        </tr>
                                        
                                        <tr>
                                            <td><asp:TextBox ID="tbTotHarga" ValidationGroup="Input" runat="server" Enabled = "True" CssClass="TextBox" width="120px"/></td>
                                             <td><asp:TextBox ID="tbDiscHd"  ValidationGroup="Input" runat="server" Enabled = "True" CssClass="TextBox" Width="50px"/></td>
                                            <td><asp:TextBox ID="tbTotDisc"  ValidationGroup="Input" runat="server" Enabled = "True" CssClass="TextBox" Width="120px"/></td>
                                             <td><asp:TextBox ID="tbTotDPP"  ValidationGroup="Input" runat="server" Enabled = "True" CssClass="TextBox" Width="120px"/></td>  
                                            
                                            <td><asp:TextBox ID="tbTotPPN"  ValidationGroup="Input" runat="server" Enabled = "True" CssClass="TextBox" Width="120px"/></td>
                                             <td><asp:TextBox ID="tbTotPPH"  ValidationGroup="Input" runat="server" Enabled = "True" CssClass="TextBox" Width="120px"/></td> 
                                             <td><asp:TextBox ID="tbTotAmount"  ValidationGroup="Input" runat="server" Enabled = "True" CssClass="TextBox" Width="120px"/></td> 
                                            
                                        </tr>
                                        
                                        
                                    </table>
                                </td>                
                        </tr>
                        
                        
                            <tr>
                                <td>Pph Final</td>
                                <td>:</td>
                                <td colspan="7">
                                    <table>
                                        <tr style="background-color:Silver;text-align:center; border-radius:30px;">
                                            <td>Pph Final(%)</td>
                                            <td>Pph Final Value</td>                                       
                                            
                                        </tr>
                                        
                                        <tr>
                                        
                                             <td><asp:TextBox ID="tbpphFinal"  ValidationGroup="Input" runat="server" AutoPostBack = "True" CssClass="TextBox" Width="100px"/></td>
                                            <td><asp:TextBox ID="tbPphFinalValue" ValidationGroup="Input" runat="server" AutoPostBack = "True" CssClass="TextBox" width="150px"/></td>
                                        </tr>
                                        
                                        
                                    </table>
                                </td>                
                        </tr>
                        
                        
                        
                        
                        
  
               
       <tr>
                            <td>From Sub Detail</td>
                            <td>:</td>
                            <td colspan="7">
                                <table>
                                    <tr style="background-color:Silver;text-align:center; border-radius:30px;">
                                        <td>Down Payment</td>
                                            <td>Tanda Jadi </td>
                                        <td>Sisa Pembayaran</td>
                                      
                                    </tr>
                                    
                                    
                                    <tr>
                                        <td><asp:TextBox ID="tbDPValue"  ValidationGroup="Input" runat="server" Enabled = "false" CssClass="TextBox" Width="120px"/></td>
                                            <td><asp:TextBox ID="tbTJHd"  ValidationGroup="Input" runat="server" Enabled = "false" CssClass="TextBox" Width="120px"/></td>     
                                        <td><asp:TextBox ID="tbSisaBayarHd"  Enabled = "false" ValidationGroup="Input" runat="server"  CssClass="TextBox" Width="120px"/></td>                                         
                                        
                                    </tr>                                    
                                    
                                </table>
                            </td>                
                    </tr> 
         
         
    
         
         
         <tr>
        
            <td>Rencana Bayar</td>
                <td>:</td>
                <td>
               <asp:DropDownList CssClass="DropDownList" runat="server" Width = "230px" ID="ddlRencanaBayar" >
                                <asp:ListItem Selected="True" >Cash</asp:ListItem>
                                <asp:ListItem >Angsuran</asp:ListItem>
                                <asp:ListItem >KPR</asp:ListItem>
                      </asp:DropDownList>
                </td>
        
            <td>Bank</td>
                <td>:</td>
                <td><asp:TextBox CssClass="TextBox" ValidationGroup="Input"  MaxLength = "30"  runat="server" ID="tbBank" Width="225px" Enabled="True"/>        
                </td>
        </tr>
         
         
         <tr>
         
         <td>Masa Angsuran</td>
                <td>:</td>
                <td><asp:TextBox runat="server" ValidationGroup="Input" ID="tbAngsuran" CssClass="TextBox" Width="225px"/>
                &nbsp Bulan
                </td>
                
               
            <td>Penyelesaian Dokumen</td>
            <td>:</td>
            <td>              
              <asp:TextBox ID="tbDokumen" runat="server" AutoPostBack="true" CssClass="TextBox" Width="105px" ValidationGroup="Input" />
               / Hari
               
               <BDP:BasicDatePicker ID="tbDateDokumen" Enabled = "false" runat="server" DateFormat="dd MMM yyyy" 
                        ReadOnly = "true" ValidationGroup="Input"
                        ButtonImageHeight="19px" ButtonImageWidth="20px" 
                        DisplayType="TextBox" 
                        TextBoxStyle-CssClass="TextDate" 
                        ShowNoneButton="False"><TextBoxStyle CssClass="TextDate" />
            </BDP:BasicDatePicker>  
            </td>
         </tr>
         
         
               <tr>
            <td><asp:LinkButton ID="lbWakilPenjual" runat="server" ValidationGroup="Input" Text="Wakil Penjual"/></td>
            <td>:</td>
            <td>
              <asp:TextBox ID="tbWakilPenjual" Visible="false" runat="server" CssClass="TextBox" Width="50px" ValidationGroup="Input" />
              <asp:TextBox ID="tbNamaWakilPenjual" runat="server" CssClass="TextBox" Width="225px" ValidationGroup="Input" /> 
              <asp:Button ID="btnWakilPenjual" runat="server" Class="btngo" Text="v" ValidationGroup="Input" />                                    

            </td>                                 
        </tr>
         
             
            <td>Remark</td>
            <td>:</td>
            <td><asp:TextBox runat="server" ValidationGroup="Input" ID="tbRemark" CssClass="TextBoxMulti" Width="400px" TextMode="MultiLine" MaxLength="255"/></td>
            
            
            <td>Remark Reject</td>
                        <td>:</td>
                        <td><asp:TextBox runat="server" TextMode="MultiLine" ValidationGroup="Input" Enabled= false ID="tbRemarkReject" CssClass="TextBox" Width="225px"/></td>
                        
        </tr>
      </table>  
        </asp:View>
       
       <asp:View ID="TabHd1" runat="server">
            <table>
                <tr>
                <td><asp:Button class="bitbtndt btndelete" OnClientClick="return confirm('Sure to delete this dokumen?');" runat="server" ID="btnClearInv" Width="15px" Text="s" /> Upload Dokumen Invoice</td>
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
                <td><asp:Button class="bitbtndt btndelete" OnClientClick="return confirm('Sure to delete this dokumen?');" runat="server" ID="btnClearFaktur" Width="15px" Text="s" /> Upload Dokumen Faktur Pajak</td>
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
                <td><asp:Button class="bitbtndt btndelete" OnClientClick="return confirm('Sure to delete this dokumen?');" runat="server" ID="btnClearBAP" Width="15px" Text="s" /> Upload Dokumen BAP External</td>
                        <td>:</td>
                 
                  <td> 
                     <asp:FileUpload runat="server" style="color: White;" accept="application/pdf" ID="FubBAPExt"  /> 
                     <asp:Button ID="btnsaveBAP" CssClass="bitbtndt btnadd" runat="server" Text="View" Style="display: none" />              
                  </td> 
                  <td>        
                    <asp:LinkButton ID="lbBAP" ValidationGroup="Input" runat="server" Text="Not Yet Uploaded"/>
                            <%--<asp:Button ID="btnsaveKtpW" CssClass="bitbtndt btnadd" runat="server"  Style="display: none" Text="View" /> --%>
                  </td>
                </tr>        
                        
                <tr></tr>
                
                <tr>
                <td><asp:Button class="bitbtndt btndelete" OnClientClick="return confirm('Sure to delete this dokumen?');" runat="server" ID="btnClearLain" Width="15px" Text="s" /> Upload Dokumen Lain-Lain</td> 
                        <td>:</td>
                 
                   <td>
                     
                     <asp:FileUpload runat="server" style="color: White;" accept="application/pdf" ID="FubDokLain"  />
                     <asp:Button ID="btnsaveLain" CssClass="bitbtndt btnadd" runat="server" Text="View" Style="display: none" />               
                  </td>  
                  
                  <td>        
                    <asp:LinkButton ID="lbDokLain" ValidationGroup="Input" runat="server" Text="Not Yet Uploaded"/>
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
                <asp:MenuItem Text="Detail Kavling" Value="0"></asp:MenuItem>
                <asp:MenuItem Text="Detail Angsuran" Value="1"></asp:MenuItem>
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
            <div style="border:0px  solid; width:100%; height:100%; overflow:auto;">
                <asp:GridView ID="GridDt" runat="server" AutoGenerateColumns="False" 
                    ShowFooter="False">
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
                                  <asp:Button class="bitbtndt btnedit" runat="server" ID="btnView" Text="Detail Jadwal Pembayaran" Width="160px" CommandName="View" CommandArgument='<%# Container.DataItemIndex %>'/>
                                 </ItemTemplate>
                        </asp:TemplateField>
                        
                            <asp:BoundField DataField="UnitCode" HeaderStyle-Width="100px" HeaderText="Unit Code" /> 
                            <asp:BoundField DataField="UnitName" HeaderStyle-Width="150px" HeaderText="Unit Name" />                           
                            <asp:BoundField DataField="LuasTanah"  ItemStyle-HorizontalAlign="Right"  HeaderStyle-Width="100px" DataFormatString="{0:#,##0.00}" HeaderText="Luas Tanah" />
                            <asp:BoundField DataField="LuasBangunan"  ItemStyle-HorizontalAlign="Right"  HeaderStyle-Width="100px" DataFormatString="{0:#,##0.00}" HeaderText="Luas Bangunan" />
                            <asp:BoundField DataField="ArahKavling" HeaderStyle-Width="150px" HeaderText="Arah Kavling" /> 
                            <asp:BoundField DataField="KtinggianDPL" HeaderText="DPL"  HeaderStyle-Width="50px" />
                            <asp:BoundField DataField="DateSerahKunci" dataformatstring="{0:dd MMM yyyy}" HeaderText="Serah Kunci"  HeaderStyle-Width="100px" />
                            <asp:BoundField DataField="StartSewa" dataformatstring="{0:dd MMM yyyy}" HeaderText="Start Sewa"  HeaderStyle-Width="100px" />
                            <asp:BoundField DataField="EndSewa" dataformatstring="{0:dd MMM yyyy}" HeaderText="End Sewa"  HeaderStyle-Width="100px" />
                            <asp:BoundField DataField="Price"  ItemStyle-HorizontalAlign="Right"  HeaderStyle-Width="100px" DataFormatString="{0:#,##0.00}" HeaderText="Price" />
                            <asp:BoundField DataField="Disc" HeaderText="Disc (%)" HeaderStyle-Width="50px"  />
                            <asp:BoundField DataField="DiscValue"  ItemStyle-HorizontalAlign="Right"  HeaderStyle-Width="100px" DataFormatString="{0:#,##0.00}" HeaderText="Disc Value" />
                            <asp:BoundField DataField="Dpp"  ItemStyle-HorizontalAlign="Right"  HeaderStyle-Width="100px" DataFormatString="{0:#,##0.00}" HeaderText="DPP " />
                            <asp:BoundField DataField="PpnTotal"  ItemStyle-HorizontalAlign="Right"  HeaderStyle-Width="100px" DataFormatString="{0:#,##0.00}" HeaderText="Ppn Total " />
                            <asp:BoundField DataField="PphTotal"  ItemStyle-HorizontalAlign="Right"  HeaderStyle-Width="100px" DataFormatString="{0:#,##0.00}" HeaderText="Pph Total" />
                            <asp:BoundField DataField="AmountTotal"  ItemStyle-HorizontalAlign="Right"  HeaderStyle-Width="100px" DataFormatString="{0:#,##0.00}" HeaderText="Total Harga" />
                            <asp:BoundField DataField="TJ"  ItemStyle-HorizontalAlign="Right"  HeaderStyle-Width="100px" DataFormatString="{0:#,##0.00}" HeaderText="Qty TJ" />
                            <asp:BoundField DataField="DP"  ItemStyle-HorizontalAlign="Right"  HeaderStyle-Width="100px" DataFormatString="{0:#,##0.00}" HeaderText="Qty DP" />
                            <asp:BoundField DataField="SisaTagihan"  ItemStyle-HorizontalAlign="Right"  HeaderStyle-Width="100px" DataFormatString="{0:#,##0.00}" HeaderText="Sisa Pembayaran" />
                            <asp:BoundField DataField="Remark" HeaderStyle-Width="200px" HeaderText="Remark" />
                    </Columns>
                </asp:GridView>
          </div>   
          <asp:Button class="bitbtndt btnadd" runat="server" ID="btnAddDtKe2" Text="Add" ValidationGroup="Input" />
      </asp:Panel>  
               <br />
               <asp:Panel ID="pnlEditDt" runat="server" Visible="false">
                   <table>
                   
                    <tr>                    
                        <td>Unit</td>
                        <td>:</td>
                        <td><asp:TextBox runat="server" ValidationGroup="Input" ID="TbUnit" CssClass="TextBox" Width="50px"/>
                        <asp:TextBox runat="server" ValidationGroup="Input" ID="tbUnitName" CssClass="TextBox" Width="175px"/>
                        <asp:Button Class="btngo" ID="btnUnit" Text="..." runat="server" ValidationGroup="Input" /></td>
                    </tr>
                    
                    <tr>
                       <td>Serah Kunci</td>
                   <td>:</td>
                   <td>
                   <BDP:BasicDatePicker ID="tbSerahKunci" runat="server"  DateFormat="dd MMM yyyy" 
                                    ReadOnly = "true" ValidationGroup="Input"
                                    ButtonImageHeight="19px" ButtonImageWidth="20px" 
                                    DisplayType="TextBoxAndImage" 
                                    TextBoxStyle-CssClass="TextDate" AutoPostBack="False" 
                                    ShowNoneButton="False"><TextBoxStyle CssClass="TextDate" Width="205px" /></BDP:BasicDatePicker> </td>
                    </tr>
                    
                    
                     <tr>
                       <td>Start Sewa</td>
                   <td>:</td>
                   <td>
                   <BDP:BasicDatePicker ID="tbStartSewa" runat="server"  DateFormat="dd MMM yyyy" 
                                    ReadOnly = "true" ValidationGroup="Input"
                                    ButtonImageHeight="19px" ButtonImageWidth="20px" 
                                    DisplayType="TextBoxAndImage" 
                                    TextBoxStyle-CssClass="TextDate" AutoPostBack="False" 
                                    ShowNoneButton="False"><TextBoxStyle CssClass="TextDate" Width="205px" /></BDP:BasicDatePicker> </td>
                     <td>End Sewa</td>
                   <td>:</td>
                   <td>
                   <BDP:BasicDatePicker ID="tbEndSewa" runat="server"  DateFormat="dd MMM yyyy" 
                                    ReadOnly = "true" ValidationGroup="Input"
                                    ButtonImageHeight="19px" ButtonImageWidth="20px" 
                                    DisplayType="TextBoxAndImage" 
                                    TextBoxStyle-CssClass="TextDate" AutoPostBack="False" 
                                    ShowNoneButton="False"><TextBoxStyle CssClass="TextDate" Width="205px" /></BDP:BasicDatePicker> </td>
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
                                        <td>Price /(m2)</td>
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
                            <td>Amount Price</td>
                            <td>:</td>
                            <td colspan="7">
                                <table>
                                    <tr style="background-color:Silver;text-align:center; border-radius:30px;">
                                        <td>Price</td>
                                        <%--<td>Disc</td>
                                        <td>Disc Value</td>
                                        <td>DPP</td>
                                        <td>Ppn Total</td>
                                        <td>Pph Total</td>                                       
                                        <td>Total Price</td>--%>
                                    </tr>
                                    
                                    
                                    <tr>
                                        <td><asp:TextBox ID="tbPrice"  ValidationGroup="Input" runat="server" Enabled = "True" CssClass="TextBox" Width="150px"/></td>                                         
                                        <td><asp:TextBox ID="tbDisc"  ValidationGroup="Input" Visible = "false" runat="server"  CssClass="TextBox" Width="50px"/></td>   
                                        <td><asp:TextBox ID="tbDiscValue"  ValidationGroup="Input" Visible = "false" runat="server" Enabled = "True" CssClass="TextBox" Width="120px"/></td>                                         
                                        <td><asp:TextBox ID="tbDpp"  ValidationGroup="Input" Visible = "false" runat="server"  CssClass="TextBox" Width="120px"/></td>
                                        <td><asp:TextBox ID="tbPpnTotal"  ValidationGroup="Input" Visible = "false" runat="server"  CssClass="TextBox" Width="120px"/></td>
                                        
                                        <td><asp:TextBox ID="tbPphtotal"  ValidationGroup="Input" Visible = "false" runat="server" Enabled = "True" CssClass="TextBox" Width="120px"/></td>                                         
                                        <td><asp:TextBox ID="tbTotalAmount"  ValidationGroup="Input" Visible = "false" runat="server"  CssClass="TextBox" Width="120px"/></td>                       
                                    </tr>
                                    
                                    
                                </table>
                            </td>                
                    </tr> 
                    
                    
                      <tr>
                            <%--<td>From Sub Detail</td>
                            <td>:</td>--%>
                            <td colspan="7">
                                <table>
                                    <tr style="background-color:Silver;text-align:center; border-radius:30px;">
                                        <%--<td>Tanda Jadi</td>
                                        <td>DP</td>
                                        <td>Sisa Pembayaran</td>--%>
                                      
                                    </tr>
                                    
                                    
                                    <tr>
                                        <td><asp:TextBox ID="tbQtyTJ" Visible = "false" ValidationGroup="Input" runat="server"  CssClass="TextBox" Width="120px"/></td>                                         
                                        <td><asp:TextBox ID="tbQtyDP"  Visible = "false"  ValidationGroup="Input" runat="server"  CssClass="TextBox" Width="120px"/></td>   
                                        <td><asp:TextBox ID="tbQtySisaBayar"  Visible = "false" ValidationGroup="Input" runat="server"  CssClass="TextBox" Width="120px"/></td>                                         
                                        
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
                   <asp:Button class="bitbtndt btnsave" runat="server" ID="btnSaveDt"  Text="Save" ValidationGroup="Input" />
                   <asp:Button class="bitbtndt btncancel" runat="server" ID="btnCancelDt" Text="Cancel" ValidationGroup="Input" />
               </asp:Panel>
       </asp:View>           
            <asp:View ID="Tab2" runat="server">     
                <table>
                 <tr>
                        <%--<td>
                            Unit Code</td>
                        <td>
                            :</td>--%>
                        <td>
                            <asp:Label ID="lblUnit" Visible ="False" runat="server" Text="Unit Number" />
                        </td>
                 </tr>
                    
                    
                    <tr>
                        <%--<td>
                           Unit Name</td>
                        <td>
                            :</td>--%>
                        <td>
                            <asp:Label ID="lblUnitName" Visible ="False" runat="server" />
                        </td>
                    </tr>
                           
                </table>
                
                <hr />
                <asp:Panel ID="pnlDt2" runat="server">
                    <br />
                    <asp:Button class="bitbtndt btnadd" runat="server" ID="btnAddDt2" Text="Add" ValidationGroup="Input" />                    
                    <asp:Button class="bitbtndt btnback" runat="server" ID="btnBackDt2ke1" Text="Back"/>
                    <asp:Button class="bitbtndt btnsearch" runat="server" Visible = "false" ID="btnGetAngsuran" Text="Get Angsuran" ValidationGroup="Input" />
                    <div style="border:0px  solid; width:100%; height:100%; overflow:auto;">
                        <asp:GridView ID="GridDt2" runat="server" AutoGenerateColumns="false" 
                            ShowFooter="False">
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
                             <asp:BoundField DataField="ItemNo" HeaderText="No Item" />  
                                    <asp:BoundField DataField="PayDate" dataformatstring="{0:dd MMM yyyy}" HeaderText="Date"  HeaderStyle-Width="100px" />  
                                    <asp:BoundField DataField="TypeName" HeaderStyle-Width="100px" HeaderText="Type" />                      
                                    <asp:BoundField DataField="PayKAv"  ItemStyle-HorizontalAlign="Right"  HeaderStyle-Width="100px" DataFormatString="{0:#,##0.00}" HeaderText="Nominal" />
                                  
                                    <asp:BoundField DataField="Ppn" HeaderText="Ppn (%)"  HeaderStyle-Width="50px" />
                                    <asp:BoundField DataField="PpnValue"  ItemStyle-HorizontalAlign="Right"  HeaderStyle-Width="100px" DataFormatString="{0:#,##0.00}" HeaderText="Ppn Value" />
                                    
                                    <asp:BoundField DataField="Pph" HeaderText="Pph(%)" HeaderStyle-Width="50px"  />
                                    <asp:BoundField DataField="PphValue"  ItemStyle-HorizontalAlign="Right"  HeaderStyle-Width="100px" DataFormatString="{0:#,##0.00}" HeaderText="Pph Value" />
                                    
                                    <asp:BoundField DataField="AmountDt2"  ItemStyle-HorizontalAlign="Right"  HeaderStyle-Width="100px" DataFormatString="{0:#,##0.00}" HeaderText="Total Nominal" />
                                    
<%--                                    <asp:BoundField DataField="BankName" HeaderStyle-Width="100px" HeaderText="Bank" />
                                    
                                    <asp:BoundField DataField="NoGiro" HeaderStyle-Width="100px" HeaderText="Giro" />--%>
                                    
                                    <asp:BoundField DataField="Remark" HeaderStyle-Width="200px" HeaderText="Remark" />
                           
                               
                            </Columns>
                        </asp:GridView>
                    </div>
                    <asp:Button class="bitbtndt btnadd" runat="server" ID="btnAddDt2Ke2" Text="Add" ValidationGroup="Input" />
                    <asp:Button class="bitbtndt btnback" runat="server" ID="btnBackDt2ke2" Text="Back" />
                  </asp:Panel>
                <asp:Panel ID="pnlEditDt2" runat="server" Visible="false">
                    <table>
                    
                    <tr>
                        <td>Item No</td>
                        <td>:</td>
                        <td><asp:Label ID="lbItemNo" runat="server" Text="0" />
                        </td>           
                    </tr> 
                    
                    <tr>
                       <td>Date</td>
                   <td>:</td>
                   <td>
                   <BDP:BasicDatePicker ID="tbTempoDate" runat="server"  DateFormat="dd MMM yyyy" 
                                    ReadOnly = "true" ValidationGroup="Input"
                                    ButtonImageHeight="19px" ButtonImageWidth="20px" 
                                    DisplayType="TextBoxAndImage" 
                                    TextBoxStyle-CssClass="TextDate" AutoPostBack="True" 
                                    ShowNoneButton="False"><TextBoxStyle CssClass="TextDate" Width="205px" /></BDP:BasicDatePicker> </td>
                    </tr> 
                    
                    
                     <tr> 
                     
                       <td>Type</td>
                            <td>:</td>
                            <td><asp:DropDownList ID="ddlType" ValidationGroup="Input" AutoPostBack="true" Width="230px" runat="server" CssClass="DropDownList" /> 
                            <asp:TextBox ID="lblAngsuran"  ValidationGroup="Input" runat="server" style="text-align: center"  CssClass="TextBox" Width="15px"/>
                            <%--<asp:Label ID="lblAngsuran" runat="server" />--%>                                                                  
                            </td>                
                    </tr> 
                    
                    
                    <tr>
                            <td>Pembayaran</td>
                            <td>:</td>
                            <td colspan="7">
                                <table>
                                    <tr style="background-color:Silver;text-align:center; border-radius:30px;">
                                        <td>Nominal</td>
                                        <td>Ppn</td>
                                        <td>PPn Value</td>
                                        <td>Pph Total</td>
                                        <td>Pph Total</td>                                       
                                        <td>Total Nominal</td>
                                    </tr>
                                    
                                    
                                    <tr>
                                        <td><asp:TextBox ID="tbNominal"  ValidationGroup="Input" runat="server" Enabled = "True" CssClass="TextBox" Width="120px"/></td>                                         
                                        <td><asp:TextBox ID="tbPpnDt2"  ValidationGroup="Input" runat="server"  CssClass="TextBox" Width="50px"/></td>   
                                        <td><asp:TextBox ID="tbPpnValuedt2"  ValidationGroup="Input" runat="server" Enabled = "True" CssClass="TextBox" Width="120px"/></td>                                         
                                        <td><asp:TextBox ID="tbPphDt2"  ValidationGroup="Input" runat="server"  CssClass="TextBox" Width="50px"/></td>
                                        <td><asp:TextBox ID="tbPphValueDt2"  ValidationGroup="Input" runat="server" Enabled = "False" CssClass="TextBox" Width="120px"/></td>                                        
                                        <td><asp:TextBox ID="tbTotalNominal"  ValidationGroup="Input" runat="server" Enabled = "True" CssClass="TextBox" Width="120px"/></td>                                         
                                    </tr>                                    
                                    
                                </table>
                            </td>                
                    </tr> 
                    
                  
                    
                    <tr> 
                     <%--
                       <td>Bank</td>
                            <td>:</td>--%>
                            <td><asp:DropDownList ID="ddlBank" Visible = "false" ValidationGroup="Input" AutoPostBack="False" Width="230px" runat="server" CssClass="DropDownList" />                                                                   
                            </td>                
                    </tr> 
                    
                    <tr> 
                     
                       <%--<td>No Giro / Cek</td>
                            <td>:</td>--%>
                            <td><asp:TextBox ID="tbGiroNo" Visible ="False"  ValidationGroup="Input" runat="server" Enabled = "True" CssClass="TextBox" Width="225px"/>                                                                   
                            </td>                
                    </tr> 
                    
                     <tr>
                        <td>Remark </td>
                        <td>:</td>
                        <td><asp:TextBox runat="server" ID="tbRemarkDt2" CssClass="TextBox" Width="365px" 
                                MaxLength="255" TextMode="MultiLine" />                        
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
        <asp:Button class="bitbtn btnsave" runat="server" ID="btnSaveTrans"  Text="Save" ValidationGroup="Input" />
        <%--OnClientClick="Confirm()"--%>
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
    <asp:HiddenField ID="HiddenRemarkReject" runat="server" />
    
    <div class="loading" align="center">
 
      <br />
       <img src="../../Image/loader.gif" alt="" />
    </div>
    </form>
    </body>
</html>
