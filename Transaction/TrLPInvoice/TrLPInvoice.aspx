<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TrLPInvoice.aspx.vb" Inherits="TrLpInvoice " %>
<%@ Register Assembly="FastReport" Namespace="FastReport.Web" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="BasicFrame.WebControls.BasicDatePicker" Namespace="BasicFrame.WebControls"
    TagPrefix="BDP" %>
<%@ Register assembly="CrystalDecisions.Web, Version=10.5.3700.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Land Purchase Invoice</title>
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


function setformathd(prmchange)
        {
         try {
         
                    
         document.getElementById("tbppnValue").value = (parseFloat(document.getElementById("tbdpp").value.replace(/\$|\,/g,"")) * parseFloat(document.getElementById("tbppn").value.replace(/\$|\,/g,""))) / 100;


         document.getElementById("tbPphValue").value = (parseFloat(document.getElementById("tbdpp").value.replace(/\$|\,/g,"")) * parseFloat(document.getElementById("tbpph").value.replace(/\$|\,/g,""))) / 100;


          // document.getElementById("tbppn").value = (parseFloat(document.getElementById("tbppnValue").value.replace(/\$|\,/g,"")) / parseFloat(document.getElementById("tbdpp").value.replace(/\$|\,/g,""))) * 100;

            var type = document.getElementById("tbType").value

            if (type == "-")
            {
                document.getElementById("tbTotalAmount").value = parseFloat(document.getElementById("tbdpp").value.replace(/\$|\,/g,"")) + parseFloat(document.getElementById("tbppnValue").value.replace(/\$|\,/g,"")) - parseFloat(document.getElementById("tbPphValue").value.replace(/\$|\,/g,"")) ; 

                   
            } else {
                document.getElementById("tbTotalAmount").value = parseFloat(document.getElementById("tbdpp").value.replace(/\$|\,/g,"")) + parseFloat(document.getElementById("tbppnValue").value.replace(/\$|\,/g,"")) + parseFloat(document.getElementById("tbPphValue").value.replace(/\$|\,/g,"")) ;                     
            }
            
           

            document.getElementById("tbTotalAmount").value = setdigit(document.getElementById("tbTotalAmount").value.replace(/\$|\,/g,""),'<%=ViewState("DigitCurr")%>'); 
            document.getElementById("tbppnValue").value = setdigit(document.getElementById("tbppnValue").value.replace(/\$|\,/g,""),'<%=ViewState("DigitCurr")%>');
            document.getElementById("tbPphValue").value = setdigit(document.getElementById("tbPphValue").value.replace(/\$|\,/g,""),'<%=ViewState("DigitCurr")%>');
           
            document.getElementById("tbdpp").value = setdigit(document.getElementById("tbdpp").value.replace(/\$|\,/g,""),'<%=ViewState("DigitCurr")%>');

             document.getElementById("tbppn").value = setdigit(document.getElementById("tbppn").value.replace(/\$|\,/g,""),'<%=ViewState("DigitCurr")%>');

              document.getElementById("tbpph").value = setdigit(document.getElementById("tbpph").value.replace(/\$|\,/g,""),'<%=ViewState("DigitCurr")%>');

           
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
        
        const file = document.querySelector('#file');
file.addEventListener('change', (e) => {
  // Get the selected file
  const [file] = e.target.files;
  // Get the file name and size
  const { name: fileName, size } = file;
  // Convert size in bytes to kilo bytes
  const fileSize = (size / 1000).toFixed(2);
  // Set the text content
  const fileNameAndSize = `${fileName} - ${fileSize}KB`;
  document.querySelector('.file-name').textContent = fileNameAndSize;
});
      

    </script>
      <style>
.file {
  opacity: 0;
  width: 0.1px;
  height: 0.1px;
  position: absolute;
}

.file-input label {
  display: block;
  font-family:roboto;
  position: relative;
  width: 120px;
  height: 30px;
  border-radius: 10px;
  background: linear-gradient(40deg,#ff6ec4,#7873f5);
  box-shadow: 0 2px 5px rgba(0, 0, 0, 0.2);
  display: flex;
  font-size:0.75rem;
  align-items: center;
  justify-content: center;
  color: #fff;
  font-weight: bold;
  cursor: pointer;
  transition: transform .2s ease-out;
}

.file-name {
  position: absolute;
  bottom: -6px;
  left: 140px;
  font-size: 0.75rem;
  color: #555;
  width: 600px;
}

input:hover + label,
input:focus + label {
  transform: scale(1.02);
}

/* Adding an outline to the label on focus */
input:focus + label {
  outline: 0px solid #000;
  outline: -webkit-focus-ring-color auto 2px;
}

  </style>
    
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
</head>

<body>     
    <form id="form1" runat="server">


     <div class="Content">
    <div class="H1">Land Purchase Invoice</div>
     <hr style="color:Blue" />        
    <asp:Panel runat="server" id="PnlHd"> 
      <table>
        <tr>
            <td style="width:100px;text-align:right">Quick Search :</td>            
            <td><asp:TextBox CssClass="TextBox" runat="server" ID ="tbFilter"/> 
                  <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlField" >
                      <asp:ListItem Value="Reference" Selected="True">Reference</asp:ListItem>
                      <asp:ListItem Value="dbo.FormatDate(TransDate)">Date</asp:ListItem>
                      <asp:ListItem Value="LandPurchaseNo">Land Purchase</asp:ListItem>
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
                 <asp:ListItem Value="LandPurchaseNo">Land Purchase</asp:ListItem>
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
                              HeaderText="TransNmbr"></asp:BoundField>                  
                          <asp:BoundField DataField="Status" HeaderText="Status" SortExpression="Status"></asp:BoundField>                  
                          <asp:BoundField DataField="TransDate" HtmlEncode="true" DataFormatString="{0:dd MMM yyyy}" HeaderText="Date" SortExpression="TransDate"></asp:BoundField>                  
                          <asp:BoundField DataField="LandPurchaseNo" HeaderStyle-Width="120px" HeaderText="Refference LP/TJ" 
                              SortExpression="LandPurchaseNo">
                              <HeaderStyle Width="120px" />
                          </asp:BoundField>
                          
                          <asp:BoundField DataField="Revisi" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="center" HeaderText="Rev" 
                              SortExpression="Revisi">
                              <HeaderStyle Width="30px" />
                          </asp:BoundField>
                          
                           <asp:BoundField DataField="JenisInv" HeaderStyle-Width="120px"  HeaderText="Type Invoice" 
                              SortExpression="JenisInv">
                              <HeaderStyle Width="120px" />
                          </asp:BoundField>

                           <asp:BoundField DataField="Jenis" HeaderStyle-Width="120px" HeaderText="Jenis Payment" 
                              SortExpression="Jenis">
                              <HeaderStyle Width="120px" />
                          </asp:BoundField>

                          <asp:BoundField DataField="Attn" HeaderStyle-Width="120px" HeaderText="Nama" 
                              SortExpression="Attn">
                              <HeaderStyle Width="120px" />
                          </asp:BoundField>
                          
                          
                          <asp:BoundField DataField="Invoice_No" HeaderStyle-Width="120px" HeaderText="Invoice No" 
                              SortExpression="Invoice_No">
                              <HeaderStyle Width="120px" />
                          </asp:BoundField>

                         <asp:BoundField DataField="InvoiceDate" HtmlEncode="true" DataFormatString="{0:dd MMM yyyy}" HeaderText="Invoice Date" SortExpression="InvoiceDate"></asp:BoundField>

                            <asp:BoundField DataField="NoFaktur" HeaderStyle-Width="120px" HeaderText="No Faktur" 
                            SortExpression="NoFaktur">                 
                            </asp:BoundField> 

                        

                        <asp:BoundField DataField="Dpp" ItemStyle-HorizontalAlign="right" HeaderText="Dpp" DataFormatString="{0:#,##0.##}" SortExpression="Dpp"> 
                                    <HeaderStyle Width="300px" />
                                </asp:BoundField>

                        <asp:BoundField DataField="PPn" ItemStyle-HorizontalAlign="right" HeaderText="Ppn %" DataFormatString="{0:#,##0.##}" SortExpression="PPn"> 
                                    <HeaderStyle Width="300px" />
                                </asp:BoundField>
                        
                        <asp:BoundField DataField="PpnAmount" ItemStyle-HorizontalAlign="right" HeaderText="Ppn Value" DataFormatString="{0:#,##0.##}" SortExpression="PpnAmount"> 
                                    <HeaderStyle Width="300px" />
                                </asp:BoundField>

                        <asp:BoundField DataField="PPh" ItemStyle-HorizontalAlign="right" HeaderText="PPh %" DataFormatString="{0:#,##0.##}" SortExpression="PPh"> 
                                    <HeaderStyle Width="300px" />
                                </asp:BoundField>

                        <asp:BoundField DataField="PPhAmount" ItemStyle-HorizontalAlign="right" HeaderText="PPh Value" DataFormatString="{0:#,##0.##}" SortExpression="PPhAmount"> 
                                    <HeaderStyle Width="300px" />
                                </asp:BoundField>        

                        <asp:BoundField DataField="TotalAmount" ItemStyle-HorizontalAlign="right" HeaderText="Total Amount" DataFormatString="{0:#,##0.##}" SortExpression="TotalAmount"> 
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
         <asp:MultiView ID="MultiView2" runat="server" ActiveViewIndex="0">
       <asp:View ID="TabHd0" runat="server">
           <table>
                            <tr>
                                <td>TransNmbr</td>
                                <td>:</td>
                                <td><asp:TextBox CssClass="TextBoxR" runat="server" ID="tbCode" Width="225px" Enabled="False"/>
                                </td> 
                                
                                
                                </td> 

                                

                                
                               
                            </tr>

                            <tr>

                                 <td>Date</td>
                                <td>:</td>
                                <td>
                                <BDP:BasicDatePicker ID="tbDate" runat="server" DateFormat="dd MMM yyyy" 
                                            ReadOnly = "true" ValidationGroup="Input"
                                            ButtonImageHeight="19px" ButtonImageWidth="20px" 
                                            DisplayType="TextBoxAndImage" 
                                            TextBoxStyle-CssClass="TextDate" AutoPostBack="True" 
                                            ShowNoneButton="False"><TextBoxStyle CssClass="TextDate" /></BDP:BasicDatePicker>   </td>
                                 <td>Date Invoice</td>
                                <td>:</td>
                                <td>
                                <BDP:BasicDatePicker ID="tbDateInvoice" runat="server" DateFormat="dd MMM yyyy" 
                                            ReadOnly = "true" ValidationGroup="Input"
                                            ButtonImageHeight="19px" ButtonImageWidth="20px" 
                                            DisplayType="TextBoxAndImage" 
                                            TextBoxStyle-CssClass="TextDate" AutoPostBack="True" 
                                            ShowNoneButton="False"><TextBoxStyle CssClass="TextDate" /></BDP:BasicDatePicker>                
                                </td> 
                                             
                             
                            </tr>
                            
                            <tr>
                                <td>Type INV</td>
                                <td>:</td>
                                <td>
                                
                                <asp:DropDownList runat="server" CssClass="DropDownList" ID="ddlTypeInv" 
                                                Width="230px" >
                                            <asp:ListItem Selected="True" Value="TJ">Tanda Jadi</asp:ListItem>
                                            <asp:ListItem Value="PL">Pelunasan</asp:ListItem>                                          
                                            </asp:DropDownList> 
                                </td>  
                                
                                <td>No Invoice</td>
                                <td>:</td>
                                <td><asp:TextBox CssClass="TextBox" runat="server" ID="tbInvoice" Width="225px" Enabled="False"/>
                                </td>    
                            </tr>
         
                            
                             

                            <tr>


                               
                                <td>Jenis Payment</td>
                                <td>:</td>          
                                <td><asp:DropDownList CssClass="DropDownList" Width="230px" ValidationGroup="Input" Visible="True" runat="server" ID="ddlJenisPayment" AutoPostBack="True" />
                                  
                                </td>
                               
                                <td>No Faktur</td>
                                <td>:</td>
                                <td>
                                    <asp:TextBox runat="server" ValidationGroup="Input" ID="tbFakturNo" CssClass="TextBox" Width="225px"/>
                                   
                                </td>
                            </tr>
         

                            
                            
                            
                             <tr>
                                 <td>Reff LP/TJ No</td>
                                <td>:</td>
                                <td><asp:TextBox runat="server" ValidationGroup="Input" ID="TbLp" CssClass="TextBox" Width="168px"/>
                                Rev : 
                                <asp:TextBox runat="server" ValidationGroup="Input" ID="tbRevisi" CssClass="TextBox" Width="20px"/>
                                 <asp:Button ID="btnLP"  runat="server" Class="btngo" visible = True Text="..." />
                                </td>
                                
                                
                                <td>Ppn Date</td>
                                <td>:</td>
                                <td>
                                <BDP:BasicDatePicker ID="tbPpndate" runat="server" DateFormat="dd MMM yyyy" 
                                            ReadOnly = "true" ValidationGroup="Input"
                                            ButtonImageHeight="19px" ButtonImageWidth="20px" 
                                            DisplayType="TextBoxAndImage" 
                                            TextBoxStyle-CssClass="TextDate" AutoPostBack="True" 
                                            ShowNoneButton="False"><TextBoxStyle CssClass="TextDate" /></BDP:BasicDatePicker>                
                                </td>  
                                 
                            </tr>

                            
                            <tr>
                                <td>Nama</td>
                                <td>:</td>
                                <td><asp:TextBox runat="server" ValidationGroup="Input" ID="tbNama" CssClass="TextBox" Width="225px"/>
                                <asp:TextBox runat="server" ValidationGroup="Input" ID="tbNameCode" Visible="false" CssClass="TextBox" Width="225px"/>
                                </td>
                                
                                 <td>No Dokumen 1</td>
                                <td>:</td>
                                <td><asp:TextBox runat="server" ValidationGroup="Input" ID="tbNoDok1" CssClass="TextBox" Width="225px"/>
                                </td>
                                
                                  
                
                            </tr>
                            
                            <tr>
                                <td>Pph</td>
                                <td>:</td>
                                <td>
                                <asp:DropDownList CssClass="DropDownList" Width="212px" ValidationGroup="Input" Visible="True" runat="server" ID="ddlpph" AutoPostBack="True" />
                                <asp:TextBox runat="server" ValidationGroup="Input" ID="tbType" CssClass="TextBox" Width="10px" Enabled = "False"/>

                            </td>
                            
                            <td>No Dokumen 2</td>
                                <td>:</td>
                                <td><asp:TextBox runat="server" ValidationGroup="Input" ID="tbNoDok2" CssClass="TextBox" Width="225px"/>
                                </td>
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
                                
                                 
                 
              
                        </tr>     
                                  
                        <tr>
                        
                        <td><asp:LinkButton ID="lbCurr" ValidationGroup="Input" runat="server" Text="Currency"/></td>
                                <td>:</td>
                                <td><asp:DropDownList ID="ddlCurr" ValidationGroup="Input" AutoPostBack="true" Width="142px" runat="server" CssClass="DropDownList" />                                                                   
                                 Rate :  <asp:TextBox ID="tbRate" ValidationGroup="Input" runat="server" Enabled ="false" CssClass="TextBox" Width="50px" />
                                </td>
                                
                        </tr>  
                                    

                            <tr>
                                <td><asp:TextBox runat="server" ValidationGroup="Input" ID="tbDPPCek" Visible = "false" CssClass="TextBox" Width="225px"/>
                                </td>                      
                            </tr>

                             

                             <tr>
                                    <td>Nominal</td>
                                    <td>:</td>
                                    <td colspan="7">
                                        <table>
                                            <tr style="background-color:Silver;text-align:center; border-radius:30px;">
                                                <td>DPP   </td>
                                                <td>PPN %</td>
                                                <td>PPN Value</td>
                                                <td>PPH %</td>
                                                <td>PPH Value</td>
                                                <td>Total Amount </td>
                                            </tr>
                                            <tr>
                                                <td><asp:TextBox ID="tbdpp" ValidationGroup="Input" runat="server" CssClass="TextBox" width="120px"/></td>
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
                                <td><asp:TextBox runat="server" TextMode="MultiLine" ValidationGroup="Input" ID="tbRemark" CssClass="TextBox" Width="302px"/></td>
                            </tr>  

                             
                    </table>
                    </asp:View>
       
       <asp:View ID="TabHd1" runat="server">
            <table>
                <tr>
                <td><asp:Button class="bitbtndt btndelete" OnClientClick="return confirm('Sure to delete this dokumen?');" runat="server" ID="btnClearInv" Width="15px" Text="s" /> Upload Dokumen Satu</td>
                      <td>:</td>
                  <td>
                         
                       <%-- <div class="file-input">
                          <input type="file" id="file" class = "file" />
                          <label for="file">
                            Upload file
                            </br>
                            <p class="file-name"></p>
                          </label>
                        </div>      --%>                    
                        
                        
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
                <td><asp:Button class="bitbtndt btndelete" OnClientClick="return confirm('Sure to delete this dokumen?');" runat="server" ID="btnClearFaktur" Width="15px" Text="s" /> Upload Dokumen Dua</td>
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
               
                
           
            </table>
       </asp:View>
       
    </asp:MultiView>
  



      
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
       <asp:Button ID="btnSaveTrans" runat="server" class="bitbtndt btnsave" Text="Save" OnClientClick="Confirm()" validationgroup="Input"/>									
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
