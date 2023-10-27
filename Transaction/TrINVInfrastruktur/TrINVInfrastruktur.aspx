<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TrINVInfrastruktur.aspx.vb" Inherits="INVInfrastruktur" %>
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


                document.getElementById("tbppnValue").value = (parseFloat(document.getElementById("tbTotalBAP").value.replace(/\$|\,/g, "")) * parseFloat(document.getElementById("tbppn").value.replace(/\$|\,/g, ""))) / 100;
                
                var pph = document.getElementById("tbpph").value

                document.getElementById("tbPphValue").value = (parseFloat(document.getElementById("tbTotalBAP").value.replace(/\$|\,/g, ""))) * pph / 100;
 


                // document.getElementById("tbppn").value = (parseFloat(document.getElementById("tbppnValue").value.replace(/\$|\,/g,"")) / parseFloat(document.getElementById("tbTotalBAP").value.replace(/\$|\,/g,""))) * 100;

                var type = document.getElementById("tbType").value

                if (type == "-") {
                    document.getElementById("tbTotalAmount").value = parseFloat(document.getElementById("tbTotalBAP").value.replace(/\$|\,/g, "")) + parseFloat(Math.floor(document.getElementById("tbppnValue").value.replace(/\$|\,/g, ""))) - parseFloat(Math.floor(document.getElementById("tbPphValue").value.replace(/\$|\,/g, "")));
                  
                } else {
                    document.getElementById("tbTotalAmount").value = parseFloat(document.getElementById("tbTotalBAP").value.replace(/\$|\,/g, "")) + parseFloat(Math.floor(document.getElementById("tbppnValue").value.replace(/\$|\,/g, ""))) + parseFloat(Math.floor(document.getElementById("tbPphValue").value.replace(/\$|\,/g, "")));
                }



                document.getElementById("tbTotalAmount").value = setdigit(document.getElementById("tbTotalAmount").value.replace(/\$|\,/g, ""), '<%=ViewState("DigitHome")%>');
                document.getElementById("tbppnValue").value = setdigit(Math.floor(document.getElementById("tbppnValue").value.replace(/\$|\,/g, "")), '<%=ViewState("DigitHome")%>');
                document.getElementById("tbPphValue").value = setdigit(Math.floor(document.getElementById("tbPphValue").value.replace(/\$|\,/g, "")), '<%=ViewState("DigitHome")%>');

                document.getElementById("tbTotalBAP").value = setdigit(document.getElementById("tbTotalBAP").value.replace(/\$|\,/g, ""), '<%=ViewState("DigitHome")%>');

                document.getElementById("tbppn").value = setdigit(document.getElementById("tbppn").value.replace(/\$|\,/g, ""), '<%=ViewState("DigitHome")%>');

                document.getElementById("tbpph").value = setdigit(document.getElementById("tbpph").value.replace(/\$|\,/g, ""), '<%=ViewState("DigitHome")%>');


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
    <div class="H1">Invoice Infrastruktur</div>
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
                            <asp:ListItem Value="Supplier_Code">Supplier</asp:ListItem>
                            <asp:ListItem Value="SuppName">Supplier Name</asp:ListItem>
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
                            <asp:ListItem Value="Supplier_Code">Supplier</asp:ListItem>
                            <asp:ListItem Value="SuppName">Supplier Name</asp:ListItem>
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
                 <asp:BoundField DataField="TransNmbr" HeaderStyle-Width="120px" SortExpression="Nmbr" HeaderText="Payment No"></asp:BoundField>                  
                      <asp:BoundField DataField="Status" SortExpression="Status" HeaderText="Status"></asp:BoundField>                  
                      <asp:BoundField DataField="TransDate" dataformatstring="{0:dd MMM yyyy}" htmlencode="true" HeaderStyle-Width="80px" SortExpression="TransDate" ItemStyle-wrap="true" HeaderText="Payment Date" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" > </asp:BoundField>
                      <asp:BoundField DataField="SuppCode" HeaderStyle-Width="200px" SortExpression="SuppCode" HeaderText="Supplier"></asp:BoundField>
                      <asp:BoundField DataField="Supplier_Name" HeaderStyle-Width="200px" SortExpression="Supplier_Name" HeaderText="Supplier Name"></asp:BoundField>
                      <asp:BoundField DataField="PPHName" HeaderStyle-Width="150px" SortExpression="PPHName" HeaderText="PPH Name"></asp:BoundField>
                      <asp:BoundField DataField="TotalBAP" DataFormatString="{0:#,##0.00}" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="80px" SortExpression="TotalBAP" HeaderText="Total BAP(Rp)"></asp:BoundField>
                      <asp:BoundField DataField="PPN" DataFormatString="{0:#,##0.00}" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="50px" SortExpression="Ppn" HeaderText="PPN (%)"></asp:BoundField>
                      <asp:BoundField DataField="PPNValue" DataFormatString="{0:#,##0.00}" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="50px" SortExpression="PPnValue" HeaderText="PPN Value (Rp)"></asp:BoundField>
                      <asp:BoundField DataField="PPH" DataFormatString="{0:#,##0.00}" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="80px" SortExpression="PPh" HeaderText="PPH(%)"></asp:BoundField>
                      <asp:BoundField DataField="PPHValue" DataFormatString="{0:#,##0.00}" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="80px" SortExpression="PphValue" HeaderText="PPH Value (Rp)"></asp:BoundField>
                      <asp:BoundField DataField="TotalAmount" DataFormatString="{0:#,##0.00}" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="80px" SortExpression="TotalAmount" HeaderText="Amount (Rp)"></asp:BoundField>
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
                    <asp:MenuItem Text="Form Input Invoice" Value="0"></asp:MenuItem>
                    <asp:MenuItem Text="Upload Dokumen Invoice" Value="1"></asp:MenuItem>
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
            <td>Invoice Date</td>
                   <td>:</td>
                   <td>
                   <BDP:BasicDatePicker ID="tbInvoiceDate" runat="server"  DateFormat="dd MMM yyyy" 
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
                                    
                <td>No Invoice Supplier</td>
                <td>:</td>
                <td><asp:TextBox CssClass="TextBox" runat="server" ValidationGroup="Input"  MaxLength = "30" ID="tbSuppInv" Width="225px" Enabled="True"/>        
                </td>
                        
        </tr>
        
        <tr>
            <td>Supplier<asp:LinkButton ID="lbSupp" Visible="false" ValidationGroup="Input"  runat="server" Text="Supplier"/></td>
            <td>:</td>
            <td>
                <asp:TextBox CssClass="TextBox" ValidationGroup="Input" runat="server" ID="tbSuppCode" Width="100px" AutoPostBack="true" />
                <asp:TextBox CssClass="TextBox" runat="server" ID="tbSuppName" Enabled="false" Width="190px"/>
                <asp:TextBox CssClass="TextBox" runat="server" ID="tbSuppType" Visible="false" Enabled="false" Width="190px"/>
                <asp:Button Class="btngo" ID="btnSupp" Text="..." runat="server" ValidationGroup="Input" />                                  
            </td>
         
         
         <td>No Faktur Pajak</td>
                <td>:</td>
                <td><asp:TextBox CssClass="TextBox" ValidationGroup="Input"  MaxLength = "30"  runat="server" ID="tbFakturPajak" Width="225px" Enabled="True"/>        
                </td>                              
        </tr> 
        
        <tr>
        <td>Pph</td>
                        <td>:</td>
                        <td>
                        <asp:DropDownList CssClass="DropDownList" Width="212px" ValidationGroup="Input" Visible="True" runat="server" ID="ddlpph" AutoPostBack="True" />
                        <asp:TextBox runat="server" ValidationGroup="Input" ID="tbType" CssClass="TextBox" Width="10px" Enabled = "false"/> 
                        </td>
        <td>Ppn Date</td>
                   <td>:</td>
                   <td>
                   <BDP:BasicDatePicker ID="tbPpnDate" runat="server"  DateFormat="dd MMM yyyy" 
                                    ReadOnly = "true" ValidationGroup="Input"
                                    ButtonImageHeight="19px" ButtonImageWidth="20px" 
                                    DisplayType="TextBoxAndImage" 
                                    TextBoxStyle-CssClass="TextDate" AutoPostBack="True" 
                                    ShowNoneButton="False"><TextBoxStyle CssClass="TextDate" Width="205px" /></BDP:BasicDatePicker> </td>                 
        </tr> 
        
        <tr>
            <td><asp:LinkButton ID="lbTerm" ValidationGroup="Input" runat="server" Text="Term"/></td>
            <td>:</td>
            <td><asp:DropDownList CssClass="DropDownList" ValidationGroup="Input" Width="142px" runat="server" ID="ddlTerm" AutoPostBack="true" /> 
                <BDP:BasicDatePicker ID="tbDueDate" runat="server" DateFormat="dd MMM yyyy" 
                        ReadOnly = "true" Enabled="false"
                        ButtonImageHeight="19px" ButtonImageWidth="20px" 
                        DisplayType="TextBox" 
                        TextBoxStyle-CssClass="TextDate" AutoPostBack="True" 
                        ShowNoneButton="False"><TextBoxStyle CssClass="TextDate" /></BDP:BasicDatePicker>
            </td> 
            
         <td>BAP External</td>
                <td>:</td>
                <td><asp:TextBox CssClass="TextBox" ValidationGroup="Input"  MaxLength = "30"  runat="server" ID="tbBapExt" Width="225px" Enabled="True"/>        
                </td>
      
        </tr>     
                  
        <tr>
        
        <td><asp:LinkButton ID="lbCurr" ValidationGroup="Input" runat="server" Text="Currency"/></td>
                <td>:</td>
                <td><asp:DropDownList ID="ddlCurr" ValidationGroup="Input" AutoPostBack="true" Width="142px" runat="server" CssClass="DropDownList" />                                                                   
                 Rate :  <asp:TextBox ID="tbRate" ValidationGroup="Input" runat="server" CssClass="TextBox" Width="50px" />
                </td>
                
        <td>Dokumen Lain</td>
                <td>:</td>
                <td><asp:TextBox CssClass="TextBox" ValidationGroup="Input"  MaxLength = "30"  runat="server" ID="tbDokumenLain" Width="225px" Enabled="True"/>        
                </td> 
                
        </tr>         
        
        
         <tr>
                            <td>Nominal</td>
                            <td>:</td>
                            <td colspan="7">
                                <table>
                                    <tr style="background-color:Silver;text-align:center; border-radius:30px;">
                                        <td>Total BAP</td>
                                        <td>PPN %</td>
                                        <td>PPN Value</td>
                                        <td>PPH %</td>
                                        <td>PPH Value</td>
                                        <td>Total Amount </td>
                                    </tr>
                                    <tr>
                                        <td><asp:TextBox ID="tbTotalBAP" ValidationGroup="Input" runat="server" CssClass="TextBox" width="120px"/></td>
                                        <td><asp:TextBox ID="tbppn" ValidationGroup="Input" runat="server" CssClass="TextBox" width="50px"/></td>
                                        <td><asp:TextBox ID="tbppnValue"  ValidationGroup="Input" runat="server" CssClass="TextBox" Width="120px"  /></td>
                                        <td><asp:TextBox ID="tbpph"  ValidationGroup="Input" runat="server" Enabled = "True" CssClass="TextBox" Width="50px"/></td> 
                                        <td><asp:TextBox ID="tbPphValue"  ValidationGroup="Input" runat="server" Enabled = "True" CssClass="TextBox" Width="120px"/></td>  
                                        <td><asp:TextBox ID="tbTotalAmount"  ValidationGroup="Input" runat="server"  CssClass="TextBoxR" Width="120px"/></td>                          
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
                <asp:MenuItem Text="Detail Invoice" Value="0"></asp:MenuItem>
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
            <asp:Button ID="btnGetBAP" runat="server" class="bitbtn btnsearch" Text="Get BAP" validationgroup="Input"/>
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
                                  <asp:Button class="bitbtndt btnedit" runat="server" ID="btnView" Text="Detail BAP" Width="100px" CommandName="View" CommandArgument='<%# Container.DataItemIndex %>'/>
                                 </ItemTemplate>
                        </asp:TemplateField>
                        
                                    <asp:BoundField DataField="BAP_No" HeaderStyle-Width="150px" HeaderText="BAP No">
                                        <HeaderStyle Width="150px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="SPK_No" HeaderStyle-Width="150px" HeaderText="SPK No">
                                        <HeaderStyle Width="150px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="NilaiBAP" DataFormatString="{0:#,##0.00}" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="80px" HeaderText="Biaya BAP (Rp)"></asp:BoundField>
                                    <asp:BoundField DataField="Remark" HeaderStyle-Width="255px" HeaderText="Remark">
                                        <HeaderStyle Width="255px" />
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
                                <td>
                                    Nomor BAP
                                </td>
                                <td>
                                    :
                                </td>
                                <td >
                                    <asp:TextBox ID="tbBAPNo" runat="server" Enabled = "false" CssClass="TextBox"
                                        MaxLength="30" Width="225px" />                                   
                                    <asp:Button ID="btnBAP" runat="server" Class="btngo" Text="..." Visible = "false" ValidationGroup="Input" />
                                </td>
                            </tr>
                            
                            <tr>
                                <td>
                                    Nomor SPK
                                </td>
                                <td>
                                    :
                                </td>
                                <td >
                                    <asp:TextBox ID="tbSPKNo" runat="server" Enabled ="false" AutoPostBack="true" CssClass="TextBox"
                                        MaxLength="30" Width="225px" />
                                </td>
                            </tr>
                            
                            <tr>
                                <td>
                                    Nilai BAP
                                </td>
                                <td>
                                    :
                                </td>
                                <td colspan="4">
                                    <asp:TextBox ID="tbNilaiBAP" Enabled = "false" runat="server" CssClass="TextBox"
                                     Width="225px" />
                                </td>
                            </tr>
                            
                             <tr>
                        <td>Remark </td>
                        <td>:</td>
                        <td><asp:TextBox runat="server" ID="tbRemarkdt" CssClass="TextBox" Width="365px" 
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
                        <td>
                            BAP Number</td>
                        <td>
                            :</td>
                        <td>
                            <asp:Label ID="lblBAPNumber" runat="server" Text="BAP Number" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                           SPK Number</td>
                        <td>
                            :</td>
                        <td>
                            <asp:Label ID="lblSPKNumber" runat="server" />
                        </td>
                    </tr>
                           
                </table>
                
                <hr />
                <asp:Panel ID="pnlDt2" runat="server">
                    <br />
                    <asp:Button class="bitbtndt btnadd" runat="server" ID="btnAddDt2" Text="Add" ValidationGroup="Input" />
                    <asp:Button class="bitbtndt btnback" runat="server" ID="btnBackDt2ke1" Text="Back"/>
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
                                    <asp:BoundField DataField="UraianPekerjaan" HeaderStyle-Width="250px" HeaderText="Uraian Pekerjaan" />                           
                                    <asp:BoundField DataField="Luas"  ItemStyle-HorizontalAlign="Right"  HeaderStyle-Width="100px" DataFormatString="{0:#,##0.00}" HeaderText="Luas" />
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
                    <asp:Button class="bitbtndt btnadd" runat="server" ID="btnAddDt2Ke2" Text="Add" ValidationGroup="Input" />
                    <asp:Button class="bitbtndt btnback" runat="server" ID="btnBackDt2ke2" Text="Back" />
                  </asp:Panel>
                <asp:Panel ID="pnlEditDt2" runat="server" Visible="false">
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
                                    <tr style="background-color:Silver; text-align:center; border-radius:30px;">
                                        <td>Luas</td>
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
