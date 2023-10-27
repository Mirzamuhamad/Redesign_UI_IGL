<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TrLandPurchaseAgremt.aspx.vb" Inherits="Transaction_TrLandPurchaseAgremt_TrLandPurchaseAgremt" %>

<%@ Register Assembly="BasicFrame.WebControls.BasicDatePicker" Namespace="BasicFrame.WebControls" TagPrefix="BDP" %>
    
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">   
    <title>Untitled Page</title>    
    <link type="text/css" rel="stylesheet" href="../../Styles/Style.css" /> 
    <script type="text/javascript" src="../../Function/OpenDlg.js" ></script>
    <script type="text/javascript" src="../../Function/Function.js" ></script> 
    <script type="text/javascript" src="../../JQuery/quicksearch.js"></script>
    <script type="text/javascript" src="../../JQuery/jquery.min.js"></script>
    <script type="text/javascript" src="../../JQuery/jquery-ui.js"></script>     
    <script type="text/javascript" src="../../JQuery/circularProgressBar.min.js"></script>  
   <!-- <script type="text/javascript" src="../../JQuery/FathGrid.js"></script> -->
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
      
      function setformatforhd(prmchange) {
        try {
           document.getElementById("tbPelunasan").value = (parseFloat(document.getElementById("tbHrgTanah").value.replace(/\$|\,/g, "")) - parseFloat(document.getElementById("tbPembayaran").value.replace(/\$|\,/g, ""))); 
                
           document.getElementById("tbHrgTanah").value = setdigit(document.getElementById("tbHrgTanah").value.replace(/\$|\,/g, ""), '<%=ViewState("DigitCurr")%>');
           document.getElementById("tbPembayaran").value = setdigit(document.getElementById("tbPembayaran").value.replace(/\$|\,/g, ""), '<%=ViewState("DigitCurr")%>');
           document.getElementById("tbPelunasan").value = setdigit(document.getElementById("tbPelunasan").value.replace(/\$|\,/g, ""), '<%=ViewState("DigitCurr")%>');
        } catch (err) {
            alert(err.description);
        }
      }  
   
      //-----------------------------------------------------------------------------//
      function OpenPopup() {
          var left = (screen.width - 600) / 2; //370
          var top = (screen.height - 600) / 2;
          var winOpen = window.open("../../earchDlgV.Aspx", "", 'toolbar=no, location=no, directories=no, status=no, menubar=no, scrollbars=yes, resizable=yes, copyhistory=no, width=' + 650 + ', height=' + 600 + ', top=' + top + ', left=' + left);
          //winOpen.reload(); 
          Opener.Location.reload(false);
          return false;
      }  
                  
      function setformat()
        {
        try
         {         
         var TrainingCost = document.getElementById("tbTrainingCost").value.replace(/\$|\,/g,"");                           
         document.getElementById("tbTrainingCost").value = setdigit(TrainingCost,'<%=Viewstate("DigitCurr")%>');
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
        
        function postback()
        {
            __doPostBack('','');
        }
        
        //-------------------------------------------------------------------------------------------//
        var $dlg = null;
        var dlg_options = {
            title: "List Document", width: 550, height: 500, modal: true
        };

        function ShowPopupDocument() {
            //open dialog and save dialog jquery object to $dlg
            $dlg = $("#dlgDocument").dialog(dlg_options);
            // reposition dialog back inside the form in its container
            $dlg.parent().appendTo($("#dlgContainer"));
        }

        /* $(function() {
        $("[id*=btnGetData]").click(function() {
        ShowPopupDocument();
        return false;
        });
        }); 
        function ShowPopupDocument() {
        $("#dlgDocument").dialog({
        title: "List Document",
        width: 550, height: 500, modal: true
        });
        }
        $(document).ready(function() {
        $("#dlgDocument tr").click(function(event) {
        var IDDocument = $(this).find("td:nth-child(2)").html();
        var strDocumentName = $(this).find("td:nth-child(3)").html();
        tbSuppCode.value = IDDocument
        tbSuppName.value = strDocumentName
        });
        }); */
        function ClosePopupDocument() {
            $('#dlgDocument').dialog('close')
            $(this).dialog('close');
        }
//--------------------------------------------------------------------//
      /*  $(function() {
            $("[id*=btnNoLandSurvey]").click(function() {
                ShowPopupListNoLS();
                return false;
            });
        }); */
        
        function ShowPopupListNoLS() {
            $("#dlgListNoLS").dialog({
                title: "List No. Land Survey",
                width: 650, height: 500, modal: true
            });
        }
        $(document).ready(function() {
           $("#dlgListNoLS tr").click(function(event) {
               // var IDKegiatan = $(this).find("td:nth-child(1)").html();
               // var strKegiatanName = $(this).find("td:nth-child(6)").html();
                tbNoLandSurvey.value = $(this).find("td:nth-child(1)").html();
                tbHrgM2.value = $(this).find("td:nth-child(3)").html();
                tbLuas.value = $(this).find("td:nth-child(4)").html();
                tbHrgTanah.value = $(this).find("td:nth-child(5)").html();
                tbSellerName.value = $(this).find("td:nth-child(6)").html();
                tbPembeli.value = $(this).find("td:nth-child(7)").html();
                tbSellerCode.value = $(this).find("td:nth-child(8)").html();
            });
            ClosePopupListNoLS();
        });
        function ClosePopupListNoLS() {
            $('#dlgListNoLS').dialog('close')
            $(this).dialog('close');
        }
//----------------------------------------------------------------------------//
        $(function() {
            $("[id*=btnWakilPembeli]").click(function() {
                ShowPopupPembeli();
                return false;
            });
        });

        function ShowPopupPembeli() {
            $("#dlgWakilPembeli").dialog({
                title: "List Wakil Pembeli",
                width: 650, height: 500, buttons: {Close: function() {$(this).dialog('close');} },
                modal: true
            });
        }
        $(document).ready(function() {
        $("#dlgWakilPembeli tr").click(function(event) {
            //var IDArea = $(this).find("td:nth-child(1)").html();
            //var strAreaName = $(this).find("td:nth-child(2)").html();
            tbKodeWakilPembeli.value = $(this).find("td:nth-child(1)").html();
            tbNamaWakilPembeli.value = $(this).find("td:nth-child(2)").html();
            });
            ClosePopupPembeli();
        });
        function ClosePopupPembeli() {
            $('#dlgWakilPembeli').dialog('close')
            $(this).dialog('close');
        }

//----------------------------------------------------------------------------//  
        function SearchGrid(txtSearch, grd)
       {
         if ($("[id *=" + txtSearch + " ]").val() != "") {
         $("[id *=" + grd + " ]").children
         ('tbody').children('tr').each(function () {
             $(this).show();
          });
         $("[id *=" + grd + " ]").children
         ('tbody').children('tr').each(function () {
            var match = false;
            $(this).children('td').each(function () {
               if ($(this).text().toUpperCase().indexOf($("[id *=" + 
               txtSearch + " ]").val().toUpperCase()) > -1) {
                    match = true;
                    return false;
                }
            });
            if (match) {
                $(this).show();
                $(this).children('th').show();
            }
            else {
                $(this).hide();
                $(this).children('th').show();
            }
          });

          $("[id *=" + grd + " ]").children('tbody').
                children('tr').each(function (index) {
            if (index == 0)
                $(this).show();
          });
         }
         else {
          $("[id *=" + grd + " ]").children('tbody').
                children('tr').each(function () {
            $(this).show();
          });
         }
       }  
       
       function SearchGVNoLS() {
         $(document).on("click",function(){
          SearchGrid('<%=txtSearchListNoLS.ClientID%>','<%=GVListNoLS.ClientID%>');
         }); 
       }
         
       function SearchGVPembeli() {
         $(document).on("click",function(){
          SearchGrid('<%=txtSearchPembeli.ClientID%>','<%=GVPembeli.ClientID%>');
         }); 
       }  
//----------------------------------------------------------------------------//        
      /*  function GetSelected() {                       
            //Reference the GridView.
            var griddoc = document.getElementById("<%=GVDocument.ClientID%>");
            var griddata = document.getElementById("<%=GridDt.ClientID%>");

            //Reference the CheckBoxes in GridView.
            var checkBoxes = griddoc.getElementsByTagName("input");
            //var rowdata = griddata.rows[0].getElementsByTagName("input"); //"select", "text"
            //var rowdata = griddata.getElementsByTagName("input");

            //Loop through the CheckBoxes.
            for (var i = 0; i < checkBoxes.length; i++) {
                if (checkBoxes[i].checked) {
                    //for (var j = 1; j < rowdata.length; j++) {
                    var row = checkBoxes[i].parentNode.parentNode;
                    //var rowData = row.parentNode.parentNode;
                    //var rowIndex = rowData[i].parentNode.parentNode;
                    //j = i + 1;
                    //rowdata = $("[id*=GVDocument] tr:last-child").clone(true);
                    //tbApplfileNo.value = row.cells[1].innerHTML;
                    //tbBrokerName.value = row.cells[2].innerHTML;
                    //row.cells[0].innerHTML = row.cells[1].innerHTML;
                    //griddata.rows[i + 2].cells[i + 1].innerHTML = row.cells[i + 1].innerHTML;
                    //griddata.rows[1].cells[0].innerText = row.cells[1].innerHTML;
                    //griddata.rows[1].cells[0].innerHTML = row.cells[1].innerHTML;
                    //griddata.rows[1].cells[1].innerHTML = row.cells[2].innerHTML;
                    //$("[id*=GVDocument]").append(rowdata);
                    griddata.rows[i + 1].cells[0].innerHTML = i+1;
                    griddata.rows[i + 1].cells[1].innerHTML = row.cells[1].innerHTML;
                    griddata.rows[i + 1].cells[2].innerHTML = row.cells[2].innerHTML;

                    //griddata.rowIndex[i].cells[1].innerHTML = row.cells[1].innerHTML;                    
                    //}
                }
                else {
                    griddata.rows[i].cells[0].innerHTML = "";
                    griddata.rows[i].cells[1].innerHTML = "";
                    griddata.rows[i].cells[2].innerHTML = "";
                }
            }
            ClosePopupDocument();
            return false;
        } */
        /*-------------------------------------------------------------------------------------------*/       
        $(function() {
            $('.search_textbox').each(function(i) {
              $(this).quicksearch("[id*=GVKegiatan] tr:not(:has(th))", {
                    'testQuery': function(query, txt, row) {
                     return $(row).children(":eq(" + i + ")").text().toLowerCase().indexOf(query[0].toLowerCase()) != -1;
                    }
                });
            });
        });
        function CheckAll(Checkbox) {
            var GVHeaderCheckbox = document.getElementById("<%=GVDocument.ClientID %>");
            for (i = 1; i < GVHeaderCheckbox.rows.length; i++) {
                GVHeaderCheckbox.rows[i].cells[0].getElementsByTagName("INPUT")[0].checked = Checkbox.checked;
            }
        }
        function CheckUploadFile() {
            var sFileLength = $("#fupSignRecv")[0].files.length;
            if (sFileLength === 0) {
                //alert("No file Pdf selected.");
                if (confirm("Document file (.Pdf) not yet uploaded. Are you sure upload file ?")) {
                    return false;
                } else {
                    return;
                    ProgressCircle();
                }
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
                
        function CheckSizeDoc(input) {
          const fileSize = input.files[0].size / 1024 / 1024; 
          if (fileSize > 3.5) {
            alert('Files document cannot be larger than 3.5 MB');
            document.getElementById("fupSignRecv").value = null;
            return false;
          } else { 
             var filename = document.getElementById('fupSignRecv').value;
             tbApplfileNo.value = filename.split("\\").pop();
             return;
          }
        }
        
        //-----------------------------------------------------------------------------//
        function GetSelectedRecords() {
            $(function() {
                var temprow = $('[id*=GridDt] table tbody').find(".emptyTd");

                $('[id*=checkAll]').change(function() {
                    var checkbox = $(this);
                    if ($(this).is(":checked")) {
                        var row = $(this).parent().closest('tr');
                        var temp = $(row).clone(true);
                        $('[id*=GVDocument] tbody').append(temp);
                        $('[id*=GridDt] table tbody').find(".emptyTd").remove();
                        $(row).find("td:first").remove();
                        $('[id*=GridDt] table tbody').append(row);
                    }
                    else {
                        $('[id*=GridDt] table tbody tr').each(function() {
                            if ($(this).find("td:first").html() == $(checkbox).parent().closest('tr').find("td:nth-child(2)").html()) {
                                $(this).remove();
                                if ($('[id*=GridDt] table tbody').has('td').length == 0) {
                                    $('[id*=GridDt] table tbody').append(temprow);
                                }
                            }
                        });
                    }
                });
            });
            ClosePopupDocument();
            return false;
        } 
         
    </script>
     <link type="text/css" rel="stylesheet" href="../../Styles/circularprogress.css" />       
     <link type="text/css" rel="stylesheet" href="../../Styles/jquery-ui.css" /> 

    
    <style type="text/css">
    .posisicircle { position: relative; top: 100px;  left: 550px; }
     /* .container .box .chart 
     { position: relative; width: 100%; height: 100%; text-align: center; font-size: 40px;
      line-height: 160px; height: 160px; color: #fff;
     } */
    </style> 
    
    
</head>
<body>
  <form id="form1" runat="server" >
    <div class="Content">
    <div class="H1">Perjanjian Pembelian Tanah</div>
     <hr style="color:Blue" />        
    <asp:Panel runat="server" id="PnlHd"> 
      <table>
        <tr>
            <td style="width:100px;text-align:right">Quick Search :</td>         
            <td><asp:TextBox CssClass="TextBox" runat="server" ID ="tbFilter"/> 
                  <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlField" >     
                    <asp:ListItem Selected="True" Value="TransNmbr">No.Perjanjian</asp:ListItem>
                    <asp:ListItem Value="dbo.FormatDate(TransDate)">Tgl.Perjanjian</asp:ListItem>
                    <asp:ListItem Value="Revisi">Revisi</asp:ListItem>
                    <asp:ListItem Value="Status">Status</asp:ListItem>
                    <asp:ListItem Value="LSNo">No. Land Survey</asp:ListItem>
                    <asp:ListItem Value="SellName">Pemilik</asp:ListItem>
                    <asp:ListItem Value="NamaPembeli">Pembeli</asp:ListItem>
                    <asp:ListItem Value="NamaWakilPenjual">Penjual</asp:ListItem>
                    <asp:ListItem Value="Remark">Remark</asp:ListItem>
                  </asp:DropDownList>
                  <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlRange"></asp:DropDownList>
                  <asp:Button class="bitbtn btnsearch" runat="server" ID="btnSearch" Text="Search" />
                  <asp:Button class="btngo" runat="server" ID="btnExpand" Text="..."/>                  
            </td>
            <td>
                <%-- <asp:LinkButton ID="LbAdvSearch" runat="server" Text="Advanced Search" />--%>
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
                    <asp:ListItem Selected="True" Value="TransNmbr">No.Perjanjian</asp:ListItem>
                    <asp:ListItem Value="dbo.FormatDate(TransDate)">Tgl.Perjanjian</asp:ListItem>
                    <asp:ListItem Value="Revisi">Revisi</asp:ListItem>
                    <asp:ListItem Value="Status">Status</asp:ListItem>
                    <asp:ListItem Value="LSNo">No. Land Survey</asp:ListItem>
                    <asp:ListItem Value="SellName">Pemilik</asp:ListItem>
                    <asp:ListItem Value="NamaPembeli">Pembeli</asp:ListItem>
                    <asp:ListItem Value="NamaWakilPenjual">Penjual</asp:ListItem>
                    <asp:ListItem Value="Remark">Remark</asp:ListItem>
              </asp:DropDownList>
          </td>              
        </tr>        
      </table>      
      </asp:Panel>
          <br />
           <asp:Button ID="BtnAdd" runat="server" class="bitbtn btnadd" Text="Add"/>
          <%--  <br/>&nbsp;	 OnClientClick="ProgressCircle(this);"--%>
           &nbsp &nbsp &nbsp   
            <asp:DropDownList CssClass="DropDownList" ID="ddlCommand" runat="server" Visible="false"/>
            <asp:Button class="btngo" runat="server" ID="BtnGo" Text="G" Visible="False"/>
          <br/>&nbsp;
         <div style="border:0px  solid; width:100%; height:100%; overflow:auto;">          
            <asp:GridView ID="GridView1" runat="server" AllowPaging="True" AllowSorting="true" CssClass="Grid" AutoGenerateColumns="false"> 
              <HeaderStyle CssClass="GridHeader" Wrap = "false"></HeaderStyle>
			  <RowStyle CssClass="GridItem" Wrap="false" />
			  <AlternatingRowStyle CssClass="GridAltItem"/>
			  <PagerStyle CssClass="GridPager" />
              <Columns>              
                  <asp:TemplateField>
                      <HeaderTemplate>
                          <asp:CheckBox ID="cbSelectHd" runat="server" AutoPostBack="true" oncheckedchanged="cbSelectHd_CheckedChanged" />
                      </HeaderTemplate>
                      <ItemTemplate>
                          <asp:CheckBox ID="cbSelect" runat="server" />
                      </ItemTemplate>
                  </asp:TemplateField>
                  <asp:TemplateField HeaderStyle-Width="110px" HeaderText="Action">
                      <ItemTemplate>
                          <asp:DropDownList CssClass="DropDownList" ID="ddl" runat="server">
                              <asp:ListItem Selected="True" Text="View" />
                              <asp:ListItem Text="Edit" />
                              <asp:ListItem Text="Revisi" />
                              <asp:ListItem Text="Print" /> 
                          </asp:DropDownList>
                          <asp:Button class="btngo" runat="server" ID="BtnGo" Text="G" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" CommandName="Go" />
                      </ItemTemplate>
                      <HeaderStyle Width="110px" />
                  </asp:TemplateField>            
                  <asp:BoundField DataField="TransNmbr" HeaderStyle-Width="120px" SortExpression="Nmbr" HeaderText="No.Trans"></asp:BoundField>  
                  <asp:BoundField DataField="Revisi" ItemStyle-HorizontalAlign="Center" SortExpression="Revisi" HeaderText="Revisi"></asp:BoundField>                 
                  <asp:BoundField DataField="Status" ItemStyle-HorizontalAlign="Center" SortExpression="Status" HeaderText="Status"></asp:BoundField>                                                    
                  <asp:BoundField DataField="TransDate" dataformatstring="{0:dd MMM yyyy}" htmlencode="true" HeaderStyle-Width="80px" SortExpression="TransDate" HeaderText="Date"></asp:BoundField>
                  <asp:BoundField DataField="LSNo" HeaderStyle-Width="80px" HeaderText="No. LS"></asp:BoundField>
                  <asp:BoundField DataField="SellName" HeaderStyle-Width="200px" HeaderText="Pemilik Tanah"></asp:BoundField>
                  <asp:BoundField DataField="Pembayaran" DataFormatString="{0:#,##0.00}" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="80px" HeaderText="Pembayaran 1"></asp:BoundField>
                  <asp:BoundField DataField="PayPelunasan" DataFormatString="{0:#,##0.00}" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="80px" HeaderText="Pembayaran 2"></asp:BoundField>
                  <asp:BoundField DataField="HargaPerM" DataFormatString="{0:#,##0.00}" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="80px" HeaderText="Harga/M2"></asp:BoundField>
                  <asp:BoundField DataField="Luas" DataFormatString="{0:#,##0.00}" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="80px" HeaderText="Luas (M2)"></asp:BoundField>
                  <asp:BoundField DataField="HargaTanah" DataFormatString="{0:#,##0.00}" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="80px" HeaderText="Harga Tanah"></asp:BoundField>
                  <asp:BoundField DataField="NamaPembeli" HeaderStyle-Width="200px" HeaderText="Pembeli"></asp:BoundField>
                  <asp:BoundField DataField="NamaWakilPenjual" HeaderStyle-Width="200px" HeaderText="Penjual"></asp:BoundField>
                  <asp:BoundField DataField="AddrWakilPenjual" HeaderStyle-Width="200px" HeaderText="Alamat Penjual"></asp:BoundField>
                  <asp:BoundField DataField="NoTelpWakilPenjual" HeaderStyle-Width="200px" HeaderText="No. Tlp Penjual"></asp:BoundField>
                  <asp:BoundField DataField="Remark" HeaderStyle-Width="250px" HeaderText="Remark"></asp:BoundField>
              </Columns>
            </asp:GridView>
          </div>
            <br/>&nbsp;
            <asp:Panel runat="server" ID ="pnlNav" Visible="false">
            <asp:Button ID="btnAdd2" runat="server" class="bitbtn btnadd" Text="Add"/>	
            &nbsp &nbsp &nbsp
            <asp:DropDownList ID="ddlCommand2" runat="server" CssClass="DropDownList"/>
            <asp:Button class="btngo" runat="server" ID="btnGo2" Text="G"/>                
            </asp:Panel>          
            <br/>&nbsp;</asp:Panel>    
    <asp:Panel runat="server" ID="pnlInput" Visible="false">
      <table>
        <tr>
            <td style="width: 90px">No. Transaksi</td>
            <td>:</td>
            <td>
              <asp:TextBox ID="tbCode" runat="server" CssClass="TextBoxR" Width="200px" Enabled="False" />
              <asp:Label ID="Label3" runat="server" Visible="False" Text =" Rev : "></asp:Label>
              <asp:Label ID="lbRevisi" runat="server" Visible="False" ></asp:Label>
            </td> 
            
            <td style="width: 30px"></td> 
            <td>Penyelesaian Dokumen</td>
            <td>:</td>
            <td>              
              <asp:TextBox ID="tbDokumen" runat="server" AutoPostBack="true" CssClass="TextBox" Width="100px" ValidationGroup="Input" />
               / Hari
               
               <BDP:BasicDatePicker ID="tbDateDokumen" runat="server" DateFormat="dd MMM yyyy" 
                        ReadOnly = "true" ValidationGroup="Input"
                        ButtonImageHeight="19px" ButtonImageWidth="20px" 
                        DisplayType="TextBoxAndImage" 
                        TextBoxStyle-CssClass="TextDate" 
                        ShowNoneButton="False"><TextBoxStyle CssClass="TextDate" />
            </BDP:BasicDatePicker>  
            </td>
             
            <td style="width: 30px"></td> 
        </tr> 
        <tr>
            <td>Tgl. Transaksi</td>
            <td>:</td>
            <td>
            <BDP:BasicDatePicker ID="tbDate" runat="server" AutoPostBack = "true" DateFormat="dd MMM yyyy" 
                        ReadOnly = "true" ValidationGroup="Input"
                        ButtonImageHeight="19px" ButtonImageWidth="20px" 
                        DisplayType="TextBoxAndImage" 
                        TextBoxStyle-CssClass="TextDate" 
                        ShowNoneButton="False"><TextBoxStyle CssClass="TextDate" />
            </BDP:BasicDatePicker>                
            </td>
            <td style="width: 10px"></td> 
            <td>Rencana Bayar</td>
            <td>:</td>
            <td>
              <asp:DropDownList ID="ddlPembayaran" runat="server" CssClass="DropDownList" Height="30px" Width="90px" >
              <asp:ListItem Value="B" Selected="True">Bank</asp:ListItem>
              <asp:ListItem Value="G">Giro</asp:ListItem>                                          
              <asp:ListItem Value="T">Transfer</asp:ListItem>                                          
              <asp:ListItem Value="C">Cash</asp:ListItem>                                          
              </asp:DropDownList>
            </td>              
            
            <%--<td style="width: 30px"></td> 
            <td>Term
              <asp:LinkButton ID="lbSeller" runat="server" ValidationGroup="Input" Text="Pemilik Tanah/Seller"/>
            </td>
            <td>:</td>
            <td>
              <asp:DropDownList ID="ddlTerm" runat="server" CssClass="DropDownList" ValidationGroup="Input" Width="150px" 
              Height="30px" AutoPostBack="true"  />             

              <BDP:BasicDatePicker ID="tbDueDate" runat="server" DateFormat="dd MMM yyyy" 
                 ReadOnly = "true" ValidationGroup="Input"
                 ButtonImageHeight="19px" ButtonImageWidth="20px" 
                 DisplayType="TextBoxAndImage" 
                 TextBoxStyle-CssClass="TextDate" 
                 ShowNoneButton="False"><TextBoxStyle CssClass="TextDate" />
              </BDP:BasicDatePicker>                                                                      
            </td> --%>          
        </tr>
        <tr>
            <td>Land Survey No</td>
            <td>:</td>
            <td style="width: 360px">              
              <asp:TextBox ID="tbNoLandSurvey" runat="server" CssClass="TextBox" Width="240px" ValidationGroup="Input" />
              <asp:Button ID="btnNoLandSurvey" runat="server" Class="btngo" Text="v" ValidationGroup="Input" />             
            </td>                            
            <td style="width: 30px"></td> 
            <td>Nama Pembeli</td>
            <td>:</td>
            <td>              
              <asp:TextBox ID="tbPembeli" runat="server" CssClass="TextBox" Width="325px" ValidationGroup="Input" />
            </td>                            
            
            <%--<td>Type Transaksi</td>
            <td>:</td>
            <td> 
              <asp:DropDownList ID="ddlType" runat="server" CssClass="DropDownList" ValidationGroup="Input" Height="30px" Width="90px" >  
              <asp:ListItem Value="TJ" Selected="True">Tanda Jadi</asp:ListItem>
              <asp:ListItem Value="PL">Pelunasan</asp:ListItem>                              
              </asp:DropDownList>           
            </td> --%>
        </tr>
        <tr>
            <td style="width: 90px">Pemilik Tanah</td>
            <td>:</td>
            <td>              
              <asp:TextBox ID="tbSellerCode" runat="server" Visible="True" CssClass="TextBox" Width="50px" ValidationGroup="Input" />
              <asp:TextBox ID="tbSellerName" runat="server" CssClass="TextBox" Width="212px" ValidationGroup="Input" /><%--Width="273px" --%>                         
            </td> 
            <td style="width: 30px"></td>            
            <td><asp:LinkButton ID="lbWakilPembeli" runat="server" ValidationGroup="Input" Text="Wakil Pembeli"/></td>
            <td>:</td>
            <td>
              <asp:TextBox ID="tbKodeWakilPembeli" runat="server" Visible="True" CssClass="TextBox" Width="50px" ValidationGroup="Input" />
              <asp:TextBox ID="tbNamaWakilPembeli" runat="server" CssClass="TextBox" Width="230px" ValidationGroup="Input" /> 
              <asp:Button ID="btnWakilPembeli" runat="server" Class="btngo" Text="v" ValidationGroup="Input" />                                    
              <%--<asp:DropDownList ID="ddlWakilPembeli" runat="server" CssClass="DropDownList" Height="30px" Width="247px" >
              </asp:DropDownList>--%>
            </td>                                 
        </tr>
        <tr>
            <td>Total</td>
            <td>:</td>
            <td style="width: 360px"> 
                <table>
                     <tr style="background-color:Silver;text-align:center">
                       <td><asp:Label ID = "lbHrgM2" runat="server" CssClass="TextBox" Text = "Harga/M2"/></td>
                       <td><asp:Label ID = "lbLuas" runat="server" CssClass="TextBox" Text = "Luas (M2)"/></td>
                       <td><asp:Label ID = "lbHrgTanah" runat="server" CssClass="TextBox" Text = "Harga Tanah"/></td>
                     </tr>
                     <tr>
                        <td><asp:TextBox ID="tbHrgM2" runat="server" CssClass="TextBox" Width="90px" Enabled="false" ValidationGroup="Input" /></td>
                        <td><asp:TextBox ID="tbLuas" runat="server" CssClass="TextBox" Width="60px" Enabled="false" ValidationGroup="Input" /></td> 
                        <td><asp:TextBox ID="tbHrgTanah" runat="server" CssClass="TextBox" Width="100px" Enabled="false" ValidationGroup="Input" /></td> 
                     </tr>
                </table>
            </td>                                    
            <td style="width: 30px"></td>  
            <td>Wakil Penjual</td>
            <td>:</td>
            <td style="width: 360px"> 
                <table>
                     <tr style="background-color:Silver;text-align:center">
                       <td><asp:Label ID = "Label1" runat="server" CssClass="TextBox" Text = "Nama"/></td>
                       <td><asp:Label ID = "Label2" runat="server" CssClass="TextBox" Text = "No. KTP"/></td>
                       <td><asp:Label ID = "Label4" runat="server" CssClass="TextBox" Text = "Alamat"/></td>
                       <td><asp:Label ID = "Label5" runat="server" CssClass="TextBox" Text = "No. Tlp"/></td>
                     </tr>
                     <tr>
                        <td><asp:TextBox ID="tbNamaWakilPenjual" runat="server" CssClass="TextBox" Width="100px" Enabled="false" ValidationGroup="Input" /></td>
                        <td><asp:TextBox ID="tbNoKTPWakilPenjual" runat="server" CssClass="TextBox" Width="100px" Enabled="false" ValidationGroup="Input" /></td> 
                        <td><asp:TextBox ID="tbAddrWakilPenjual" runat="server" CssClass="TextBox" Width="100px" Enabled="false" ValidationGroup="Input" /></td> 
                        <td><asp:TextBox ID="tbNoTlpWakilPenjual" runat="server" CssClass="TextBox" Width="100px" Enabled="false" ValidationGroup="Input" /></td> 
                     </tr>
                </table>
            </td>                                                
        </tr>
        <tr>
            <td>Pembayaran 1</td>
            <td>:</td>
            <td><asp:TextBox ID="tbPembayaran" CssClass="TextBox" runat="server" Width="162px" ValidationGroup="Input" /></td>      
            <td style="width: 30px"></td>             
        </tr>         
        <tr>
            <td>Pembayaran 2</td>
            <td>:</td>
            <td><asp:TextBox ID="tbPelunasan" CssClass="TextBox" runat="server" Width="162px" ValidationGroup="Input" />
            </td>                     
            <td style="width: 30px"></td> 
            <td></td>
            <td></td>
            <td></td>      
        </tr> 
        <tr>
           <td>Remark</td>
           <td>:</td>
           <td><asp:TextBox ID="tbRemark" runat="server" ValidationGroup="Input"  CssClass="TextBoxMulti" Width="280px"
            MaxLength="255" TextMode="MultiLine" /></td>                                                                                               
        </tr>                       
      </table>        
      <br /> 

        <asp:Button ID="btnSaveAll" runat="server" class="bitbtn btnsave" Text="Save & New" ValidationGroup="Input" OnClientClick="return CheckUploadFile(this);" Width="97px"/> 
        &nbsp;    
        <asp:Button ID="btnSaveTrans" runat="server" class="bitbtn btnsave" Text="Save" ValidationGroup="Input" OnClientClick="return CheckUploadFile(this);"/> 
        &nbsp;    
        <asp:Button ID="btnBack" runat="server" class="bitbtn btncancel" Text="Cancel" ValidationGroup="Input"/>  
        &nbsp;
        <asp:Button ID="btnHome" runat="server" class="btngo" Text="Home" Width="48px"/>  
           
      <%--<div style="font-size:medium; color:White;">Detail</div>      
      <hr style="color:White" /> --%> 
              <asp:Panel runat="server" ID="PnlDt">
              <%--<asp:Button class="bitbtndt btnadd" runat="server" ID="btnAdddt" Text="Add" Visible="false" ValidationGroup="Input" />&nbsp;  OnClientClick="settext(this.value);" --%>         
              <%--<asp:Button ID="btnGetData" runat="server" class="bitbtn btngo" ValidationGroup="Input" onchange="FileOnChange(this)" 
                     Text="Get Data" Visible="false" Width="75px"/> 
              <asp:Button ID="btnDeleteAll" runat="server" class="bitbtn btndelete" ValidationGroup="Input" OnClientClick="gfg_Run()"
                     Text="Delete All" Visible="false" Width="90px"/>--%> 
                                        
              <br/>&nbsp;
                <div style="border:0px  solid; width:100%; height:100%; overflow:auto;">
                    <asp:GridView ID="GridDt" runat="server" AllowSorting="true" CssClass="Grid" AutoGenerateColumns="false" ShowFooter="False">
                        <HeaderStyle CssClass="GridHeader" Wrap = "false" ></HeaderStyle>
                        <RowStyle CssClass="GridItem" Wrap="false" />
                        <AlternatingRowStyle CssClass="GridAltItem" />
                        <PagerStyle CssClass="GridPager" />
                        <Columns>
                            <asp:TemplateField HeaderText="Action">
                               <ItemTemplate>
                               <asp:Button ID="btnEdit" runat="server" class="bitbtn btnedit" Text="Edit" CommandName="Edit"/>
                               <asp:Button ID="btnDelete" runat="server" class="bitbtn btndelete" Text="Delete" OnClientClick="return confirm('Sure to delete this data?');" CommandName="Delete"/>						                                      
                               </ItemTemplate>
                            </asp:TemplateField>                            
                            <%--<asp:BoundField DataField="ItemNo" HeaderStyle-Width="30px" HeaderText="ItemNo" ItemStyle-horizontalAlign ="Left" />--%>   
                            <asp:BoundField DataField="DokCode" HeaderStyle-Width="80px" HeaderText="Doc Code" ItemStyle-horizontalAlign ="Left"  />   
                            <asp:BoundField DataField="DokName" HeaderStyle-Width="200px" HeaderText="Doc Name" ItemStyle-horizontalAlign ="Left" />
                            <asp:BoundField DataField="Remark" HeaderStyle-Width="100px" HeaderText="Remark" ItemStyle-horizontalAlign ="Left" />                                
                        </Columns>
                    </asp:GridView>
              </div>  
              <br/>&nbsp;               
              <%--<asp:Button class="bitbtndt btnadd" runat="server" ID="btnAddDtke2" Text="Add" Visible="false" ValidationGroup="Input" />--%></asp:Panel> 
              <asp:Panel runat="server" ID="pnlEditDt" Visible="false">
                <table style="width: 681px"> 
                <%--    <tr>
                      <td>Category</td>
                      <td>:</td>
                      <td>
                         <asp:DropDownList ID="ddlCategory" runat="server" CssClass="DropDownList" ValidationGroup="Input" Width="200px" 
                         OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged" AutoPostBack="True" >                                
                         </asp:DropDownList>&nbsp;&nbsp; 
                         <asp:Label ID="lbGetCategoryCode" runat="server" Text="" Visible="False"/>                            
                      </td>                   
                   </tr>
                   <tr>
                      <td>Sub Category</td>
                      <td>:</td>
                      <td>
                                                     
                      </td>                   
                   </tr> --%>                  
                   <tr>
                      <td>Doc Code</td>
                      <td>:</td>
                      <td><asp:TextBox CssClass="TextBox" ValidationGroup="Input" runat="server" 
                              ID="tbDocCode" Width="60px" AutoPostBack="True" />
                          <asp:TextBox CssClass="TextBoxR" ValidationGroup="Input" runat="server" 
                              ID="tbDocName" Width="350px" />
                          <asp:Button class="btngo" runat="server" ID="btnDocument" Text="..."/>
                      </td>
                   </tr>     
                  <%-- <tr>
                      <td>Unit</td>
                      <td>:</td>
                      <td>
                           <asp:DropDownList ID="ddlUnit" runat="server" CssClass="DropDownList" ValidationGroup="Input" Width="80px" >                                
                           </asp:DropDownList>                            
                      </td>
                   </tr> --%>  
                   <tr>
                      <td>Remark</td>
                      <td>:</td>
                      <td>
                          <asp:TextBox ID="tbRemarkDt" runat="server" CssClass="TextBoxMulti" MaxLength="255" TextMode="MultiLine" ValidationGroup="Input" Width="300px" />
                      </td>
                   </tr>
                </table>
                <br />           
                <asp:Button ID="btnSaveDt" runat="server" class="bitbtn btnsave" Text="Save"/> &nbsp;         
                <asp:Button ID="btnCancelDt" runat="server" class="bitbtn btncancel" Text="Cancel"/> 
           </asp:Panel> 
       <br />      
        
          
    </asp:Panel>        
    </div>
    
    <div id="dlgContainer">
      <div id="dlgDocument" style="display:none;"> <%--style="display:none;" --%>
        <%--Search : <asp:TextBox ID="txtSearchDocument" runat="server" />--%>
        <asp:GridView ID="GVDocument" runat="server" CssClass="Grid" AutoGenerateColumns="false">
          <HeaderStyle CssClass="GridHeader" Wrap="false" ></HeaderStyle>
          <RowStyle CssClass="GridItem" Wrap = "false" /> <%--Gridview-row--%>
          <AlternatingRowStyle CssClass="GridAltItem"/>
          <PagerStyle CssClass="GridPager" HorizontalAlign="Left" />        
          <Columns>
            <asp:TemplateField>
                <HeaderTemplate>
                  <asp:CheckBox ID="checkAll" runat="server" onclick = "CheckAll(this);" />
                </HeaderTemplate>            
                <ItemTemplate>
                    <asp:CheckBox ID="chkRow" runat="server" onclick = "Check_Click(this)" />
                </ItemTemplate>
            </asp:TemplateField>                      
            <asp:TemplateField HeaderText="" ItemStyle-Width="50" Visible="false" >
                <ItemTemplate>
                    <asp:Label ID="lblDokCode" runat="server" Text='<%# Eval("DokCode") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="DokCode" HeaderText="Document Code" HeaderStyle-Width="80px" /><%-- ItemStyle-CssClass="cssDocument" --%>
            <asp:BoundField DataField="DokName" HeaderText="Document Name" HeaderStyle-Width="300px" />
            
          </Columns>
        </asp:GridView>  
        <asp:Button ID="btnApply" runat="server" class="bitbtn btnapply" Text="Apply" Width="90px" OnClientClick="return GetSelectedRecords(this);"   />
        <%--OnClientClick="return GetSelectedRecords(this);" OnClick="GetSelectedRecords" OnClick="btnItemSave_Click"--%>            
      </div>   
    </div>
      <div id="dlgListNoLS" style="display:none;">
        Search : <asp:TextBox ID="txtSearchListNoLS" runat="server" CssClass="TextBox" Width="200px" /> 
        <asp:Button ID="btnSearchListLSNo" runat="server" class="bitbtn btnapply" Width="90px" Text="Search" OnClientClick="return SearchGVNoLS(this);" /><%--Gridview-row--%>
        <hr />
        <asp:GridView ID="GVListNoLS" runat="server" CssClass="Grid" AutoGenerateColumns="false"  >  
          <HeaderStyle CssClass="GridHeader" Wrap="false" ></HeaderStyle> <%--OnDataBound="OnDataBoundActivity"--%>
          <RowStyle CssClass="GridItem" Wrap = "false" /> 
          <AlternatingRowStyle CssClass="GridAltItem"/>
          <PagerStyle CssClass="GridPager" HorizontalAlign="Left" />
          <Columns>
            <asp:BoundField DataField="TransNmbr" HeaderStyle-Width="80px" HeaderText="No. LS" /><%--ItemStyle-CssClass="cssSupplier" --%>
            <asp:BoundField DataField="TransDate" dataformatstring="{0:dd MMM yyyy}" htmlencode="true" HeaderStyle-Width="80px" SortExpression="TransDate" HeaderText="Date"></asp:BoundField>
            <asp:BoundField DataField="HrgTanah" DataFormatString="{0:#,##0}" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="80px" HeaderText="Harga/M2"></asp:BoundField>
            <asp:BoundField DataField="HrgFix" DataFormatString="{0:#,##0}" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="80px" HeaderText="Luas (M2)"></asp:BoundField>
            <asp:BoundField DataField="TtlHrgTanah" DataFormatString="{0:#,##0}" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="80px" HeaderText="Harga Tanah"></asp:BoundField>
            <asp:BoundField DataField="SellName" HeaderStyle-Width="100px" HeaderText="Pemilik" />            
            <asp:BoundField DataField="Pembeli" HeaderStyle-Width="100px" HeaderText="Pembeli"></asp:BoundField>
            <asp:BoundField DataField="SellCode" HeaderStyle-Width="50px" HeaderText="Kode" />            
            <%--<asp:BoundField DataField="SellName" HeaderStyle-Width="200px" HeaderText="Pemilik Tanah/Seller"></asp:BoundField>
            <asp:BoundField DataField="Pembayaran" DataFormatString="{0:#,##0.00}" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="80px" HeaderText="Pembayaran"></asp:BoundField>
            <asp:BoundField DataField="PayPelunasan" DataFormatString="{0:#,##0.00}" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="80px" HeaderText="Pelunasan"></asp:BoundField>--%>
            
          </Columns>
        </asp:GridView>      
      </div> 

      <div id="dlgWakilPembeli" style="display:none;">        
       Search : <asp:TextBox ID="txtSearchPembeli" CssClass="TextBox" runat="server" Width="200px" />
        <asp:Button ID="btnSearchPembeli" runat="server" class="bitbtn btnapply" Width="90px" Text="Search" OnClientClick="return SearchGVPembeli(this);" />       
        <hr />
        <asp:GridView ID="GVPembeli" runat="server" CssClass="Grid" AutoGenerateColumns="false"> <%-- OnDataBound="OnDataBoundActivity" --%>
        <HeaderStyle CssClass="GridHeader" Wrap="false" ></HeaderStyle>
          <RowStyle CssClass="GridItem" Wrap = "false" /> <%--Gridview-row--%>
          <AlternatingRowStyle CssClass="GridAltItem"/>
          <PagerStyle CssClass="GridPager" HorizontalAlign="Left" />
          <Columns>
            <asp:BoundField HeaderStyle-Width="80px" DataField="TugasCode" HeaderText="Kode" /><%--ItemStyle-CssClass="cssSupplier" --%>
            <asp:BoundField HeaderStyle-Width="200px" DataField="TugasName" HeaderText="Nama" />
            <asp:BoundField HeaderStyle-Width="250px" DataField="Address1" HeaderText="Alamat" />
            <asp:BoundField HeaderStyle-Width="100px" DataField="Phone" HeaderText="Telepon" />            
          </Columns>
        </asp:GridView>      
      </div> 

   
    <asp:Label runat ="server" ID="lbStatus" ForeColor="Red"/>
    <asp:HiddenField ID="HiddenRemarkRevisi" runat="server" />
    <%--<td colspan="4">   style="display:none;" 
    <div id="boxprogress" class="box"  >
      <div class="chart" data-percent="100" ></div>
    </div>--%>
    
    <div class="loading" align="center">
      <%--Loading. Please wait.<br />--%>
      <br />
       <img src="../../Image/loader.gif" alt="" />
    </div>
    
    
  </form>
</body>
</html>
