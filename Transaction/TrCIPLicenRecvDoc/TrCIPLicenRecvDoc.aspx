<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TrCIPLicenRecvDoc.aspx.vb" Inherits="Transaction_TrCIPLicenRecvDoc_TrCIPLicenRecvDoc" %>

<%@ Register Assembly="BasicFrame.WebControls.BasicDatePicker" Namespace="BasicFrame.WebControls" TagPrefix="BDP" %>
    
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">   
    <title>Untitled Page</title>    
    <link type="text/css" rel="stylesheet" href="../../Styles/Style.css" /> 
    <script type="text/javascript" src="../../Function/OpenDlg.js" ></script>
    <script type="text/javascript" src="../../Function/Function.js" ></script> 
    <script type="text/javascript" src="../../JQuery/jquery.min.js"></script>
    <script type="text/javascript" src="../../JQuery/jquery-ui.js"></script> 
    <script type="text/javascript" src="../../JQuery/circularProgressBar.min.js"></script>  
    <%--<script type="text/javascript" src="../../JQuery/jquery.easypiechart.min.js"></script>    
    <script type="text/javascript" src="../../JQuery/presentation.js"></script>
    <script type="text/javascript" src="../../JQuery/circle-progress.js"></script>  
   <script type="text/javascript" src="../../JQuery/ASPSnippets_Pager.min.js" ></script> 
    <script type="text/javascript" src="../../JQuery/quicksearch.js"></script>--%>      
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
/*
      var WshShell = new ActiveXObject("WScript.Sheell");
      function Minimize()
      { WshShell.sendkey("% n") } */
   
      //-----------------------------------------------------------------------------//
      function OpenPopup() {
          var left = (screen.width - 600) / 2; //370
          var top = (screen.height - 600) / 2;
          var winOpen = window.open("../../FindMultiDlg.Aspx", "", 'toolbar=no, location=no, directories=no, status=no, menubar=no, scrollbars=yes, resizable=yes, copyhistory=no, width=' + 600 + ', height=' + 600 + ', top=' + top + ', left=' + left);
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
        
        function settext(val1) {
            //document.getElementById('<%= tbPICName.ClientID %>').value = val1; 
            document.getElementById('<%= lbSignRecv.ClientID %>').value = val1; 
        }

       /* window.pressed = function() {
        var a = document.getElementById('fupSignRecv');
            if (a.value == "") {
                fileLabel.innerHTML = "Choose file";
            }
            else {
                var theSplit = a.value.split('\\');
                fileLabel.innerHTML = theSplit[theSplit.length - 1];
            }
        };

        var el_down = document.getElementById("fupSignRecv");
        function gfg_Run() {
            $('#fupSignRecv').attr('title', '');
            el_down.innerHTML ="Tooltip value has been removed.";
        } */
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
        $(function() {
            $("[id*=btnKegiatan]").click(function() {
                ShowPopupActivity();
                return false;
            });
        });

        function ShowPopupActivity() {
            $("#dlgKegiatan").dialog({
                title: "List Kegiatan",
                width: 550, height: 500, modal: true
            });
        }
        $(document).ready(function() {
           $("#dlgKegiatan tr").click(function(event) {
                var IDKegiatan = $(this).find("td:nth-child(1)").html();
                var strKegiatanName = $(this).find("td:nth-child(2)").html();
                tbKegiatanCode.value = IDKegiatan
                tbKegiatanName.value = strKegiatanName
            });
            ClosePopupKegiatan();
        });
        function ClosePopupKegiatan() {
            $('#dlgKegiatan').dialog('close')
            $(this).dialog('close');
        }
//----------------------------------------------------------------------------//
        $(function() {
            $("[id*=btnArea]").click(function() {
                ShowPopupArea();
                return false;
            });
        });

        function ShowPopupArea() {
            $("#dlgArea").dialog({
                title: "List Area",
                width: 400, height: 500, modal: true
            });
        }
        $(document).ready(function() {
        $("#dlgArea tr").click(function(event) {
            var IDArea = $(this).find("td:nth-child(1)").html();
            var strAreaName = $(this).find("td:nth-child(2)").html();
            tbAreaCode.value = IDArea
            tbAreaName.value = strAreaName
            });
            ClosePopupArea();
        });
        function ClosePopupArea() {
            $('#dlgArea').dialog('close')
            $(this).dialog('close');
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
             return;
          }
        }
        
        function File_OnChange(sender) {
          val = sender.value.split('\\');
          document.getElementById('<%= tbApplfileNo.ClientID %>').value = val[val.length - 1];
        }
//-----------------------------------------------------------------------------//
     /* window.addEventListener("DOMContentLoaded", () => {
        // update circle when range change
        const pie = document.querySelectorAll(".pie");
        const range = document.querySelector('[type="range"]');

        range.addEventListener("input", (e) => {
          pie.forEach((el, index) => {
            const options = {index: index + 1, percent: e.target.value,};
            circle.animationTo(options);
          });
        });  

        // start the animation when the element is in the page view
        const elements = [].slice.call(document.querySelectorAll(".pie"));
        const circle = new CircularProgressBar("pie");
        // circle.initial();
        if ("IntersectionObserver" in window) {
          const config = {root: null, rootMargin: "0px", threshold: 0.75, };

          const ovserver = new IntersectionObserver((entries, observer) => {
            entries.map((entry) => {
              if (entry.isIntersecting && entry.intersectionRatio >= 0.75) {
                circle.initial(entry.target);
                observer.unobserve(entry.target); }
            });
          }, config);
          elements.map((item) => { ovserver.observe(item); });
        } else {
          elements.map((element) => {circle.initial(element); });
        }  
      });   */
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

 <%--<style type="text/css">
        .style3
        {
            width: 497px;
        }
    </style>--%>
    
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
    <div class="H1">CIP License Terima Dokumen</div>
     <hr style="color:Blue" />        
    <asp:Panel runat="server" id="PnlHd"> 
      <table>
        <tr>
            <td style="width:100px;text-align:right">Quick Search :</td>         
            <td><asp:TextBox CssClass="TextBox" runat="server" ID ="tbFilter"/> 
                  <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlField" >     
                    <asp:ListItem Selected="True" Value="TransNmbr">No.Terima Dokumen</asp:ListItem>
                    <asp:ListItem Value="dbo.FormatDate(TransDate)">Tgl.Terima Dokumen</asp:ListItem>
                    <asp:ListItem Value="Status">Status</asp:ListItem>
                    <asp:ListItem Value="ApplfileNo">No. Berkas</asp:ListItem>
                    <asp:ListItem Value="dbo.FormatDate(ApplfileDate)">Tgl. Berkas</asp:ListItem>
                    <%--<asp:ListItem Value="dbo.FormatDate(EndDate)">End Date</asp:ListItem>--%>
                    <asp:ListItem Value="HGBNo">Alas Hak</asp:ListItem>
                    <asp:ListItem Value="AreaCode">Code Area</asp:ListItem>
                    <asp:ListItem Value="PICName">PIC</asp:ListItem>
                    <asp:ListItem Value="BrokerName">Perantara</asp:ListItem>
                    <asp:ListItem Value="RelatedOffcName">Pejabat Terkait</asp:ListItem>
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
                    <asp:ListItem Selected="True" Value="TransNmbr">No.Terima Dokumen</asp:ListItem>
                    <asp:ListItem Value="dbo.FormatDate(TransDate)">Tgl.Terima Dokumen</asp:ListItem>
                    <asp:ListItem Value="Status">Status</asp:ListItem>
                    <asp:ListItem Value="ApplfileNo">No. Berkas</asp:ListItem>
                    <asp:ListItem Value="dbo.FormatDate(ApplfileDate)">Tgl. Berkas</asp:ListItem>
                    <%--<asp:ListItem Value="dbo.FormatDate(EndDate)">End Date</asp:ListItem>--%>
                    <asp:ListItem Value="HGBNo">Alas Hak</asp:ListItem>
                    <asp:ListItem Value="AreaCode">Code Area</asp:ListItem>
                    <asp:ListItem Value="PICName">PIC</asp:ListItem>
                    <asp:ListItem Value="BrokerName">Perantara</asp:ListItem>
                    <asp:ListItem Value="RelatedOffcName">Pejabat Terkait</asp:ListItem>
              </asp:DropDownList>
          </td>              
        </tr>        
      </table>      
      </asp:Panel>
          <br />
           <asp:Button ID="BtnAdd" runat="server" class="bitbtn btnadd" Text="Add"/>
          <%--  <br/>&nbsp;	 OnClientClick="ProgressCircle(this);"--%>
           &nbsp; 
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
                              <%--<asp:ListItem Text="Print" /> --%>
                          </asp:DropDownList>
                          <asp:Button class="btngo" runat="server" ID="BtnGo" Text="G" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" CommandName="Go" />
                      </ItemTemplate>
                      <HeaderStyle Width="110px" />
                  </asp:TemplateField>            
                  <asp:BoundField DataField="TransNmbr" HeaderStyle-Width="120px" SortExpression="Nmbr" HeaderText="No.Terima Dokumen"></asp:BoundField>                  
                  <asp:BoundField DataField="Status" ItemStyle-HorizontalAlign="Center" SortExpression="Status" HeaderText="Status"></asp:BoundField>                  
                  <asp:BoundField DataField="TransDate" dataformatstring="{0:dd MMM yyyy}" htmlencode="true" HeaderStyle-Width="80px" SortExpression="TransDate" HeaderText="Date"></asp:BoundField>
                  <asp:BoundField DataField="ApplfileNo" HeaderStyle-Width="80px" HeaderText="No. Berkas"></asp:BoundField>
                  <asp:BoundField DataField="ApplfileDate" dataformatstring="{0:dd MMM yyyy}" htmlencode="true" HeaderStyle-Width="80px" HeaderText="Tgl. Berkas"></asp:BoundField>
                  <asp:BoundField DataField="KegiatanName" HeaderStyle-Width="200px" HeaderText="Nama Kegiatan"></asp:BoundField>
                  <asp:BoundField DataField="AreaName" HeaderStyle-Width="200px" HeaderText="Nama Area"></asp:BoundField>
                  <asp:BoundField DataField="HGBNo" HeaderStyle-Width="80px" HeaderText="Alas Hak"></asp:BoundField>
                  <asp:BoundField DataField="PICName" HeaderStyle-Width="80px" HeaderText="PIC"></asp:BoundField>
                  <asp:BoundField DataField="BrokerName" HeaderStyle-Width="80px" HeaderText="Perantara"></asp:BoundField>
                  <asp:BoundField DataField="BrokerPhone" HeaderStyle-Width="80px" HeaderText="No. Tlp"></asp:BoundField>
                  <asp:BoundField DataField="RelatedOffcName" HeaderStyle-Width="80px" HeaderText="Pejabat Terkait"></asp:BoundField>
                  <asp:BoundField DataField="RelatedOffcPhone" HeaderStyle-Width="80px" HeaderText="No. Tlp"></asp:BoundField>
                  <asp:BoundField DataField="Remark" HeaderStyle-Width="250px" HeaderText="Remark"></asp:BoundField>
              </Columns>
            </asp:GridView>
          </div>
            <br/>
            <asp:Panel runat="server" ID ="pnlNav" Visible="false">
            <asp:Button ID="btnAdd2" runat="server" class="bitbtn btnadd" Text="Add"/>	
            &nbsp;
            <asp:DropDownList ID="ddlCommand2" runat="server" CssClass="DropDownList"/>
            <asp:Button class="btngo" runat="server" ID="btnGo2" Text="G"/>                
            </asp:Panel>          
            <br/>&nbsp;</asp:Panel>    
    <asp:Panel runat="server" ID="pnlInput" Visible="false">
      <table>
        <tr>
            <td style="width: 70px">No. Terima Dokumen</td>
            <td>:</td>
            <td><asp:TextBox ID="tbCode" runat="server" CssClass="TextBoxR" Width="200px" Enabled="False" /></td> 
            <td style="width: 30px"></td> 
            <td style="width: 80px">Upload Tanda Terima</td>
            <td>:</td>
            <td>
               <asp:Button ID="btnDeleteDoc" runat="server" class="bitbtn btndelete" Text="Delete" OnClientClick="return confirm('Sure to delete only document file ?');" />	
               <asp:FileUpload ID="fupSignRecv" style="color: White;" runat="server" accept=".pdf" onchange="CheckSizeDoc(this)" /><%--Visible = "false" onchange="pressed()"   --%>
               <asp:LinkButton ID="lbSignRecv" runat="server" ValidationGroup="Input" Width="200px" Text="" /><%--accept="application/pdf"  onchange="settext(this.value);" --%>         
               </td>
            <%--<td> Text="Not Yet Uploaded" Text="File uploaded successfully"  onchange="File_OnChange(this)"
               <asp:Button ID="btnSignRecv" Text="Upload" runat="server" Style="display: none" /> CssClass="labelMassage"  
               <asp:LinkButton ID="lbSignRecv" ValidationGroup="Input" runat="server" Text=""/>Text="Not Yet Uploaded"         
            </td>--%>
        </tr> 
        <tr>
            <td>Tgl. Terima Dokumen</td>
            <td>:</td>
            <td>
            <BDP:BasicDatePicker ID="tbDate" runat="server" DateFormat="dd MMM yyyy" 
                        ReadOnly = "true" ValidationGroup="Input"
                        ButtonImageHeight="19px" ButtonImageWidth="20px" 
                        DisplayType="TextBoxAndImage" 
                        TextBoxStyle-CssClass="TextDate" 
                        ShowNoneButton="False"><TextBoxStyle CssClass="TextDate" />
            </BDP:BasicDatePicker>                
            </td>
            <td style="width: 30px"></td> 
            <td>No. Berkas Permohonan</td>
            <td>:</td>
            <td><%--<td colspan="4">--%>
                <asp:TextBox ID="tbApplfileNo" runat="server" CssClass="TextBox" ValidationGroup="Input" Width="200px" />
                <%--<asp:Button Class="btngo" ID="btnGetNoLP" Text="..." runat="server" ValidationGroup="Input" />--%>                                  
            </td>
            
        </tr>
        <tr>
            <td><asp:LinkButton ID="lbKegiatan" runat="server" ValidationGroup="Input" Text="Kegiatan"/></td>
            <td>:</td>
            <td style="width: 360px"><asp:TextBox ID="tbKegiatanCode" runat="server" CssClass="TextBox" Width="60px" ValidationGroup="Input" /><%--Enabled="False" --%>
              <asp:TextBox ID="tbKegiatanName" runat="server" CssClass="TextBox" Width="250px" ValidationGroup="Input" />
              <asp:Button ID="btnKegiatan" runat="server" Class="btngo" Text="v" ValidationGroup="Input" />
            </td>                            
            <td style="width: 30px"></td> 
            <td>Tgl. Berkas Permohonan</td>
            <td>:</td>
            <td>
               <BDP:BasicDatePicker ID="tbApplfileDate" runat="server" DateFormat="dd MMM yyyy" 
                        ReadOnly = "true" ValidationGroup="Input"
                        ButtonImageHeight="19px" ButtonImageWidth="20px" 
                        DisplayType="TextBoxAndImage" 
                        TextBoxStyle-CssClass="TextDate" 
                        ShowNoneButton="False"><TextBoxStyle CssClass="TextDate" />
              </BDP:BasicDatePicker>                
            </td>
        </tr>
        <tr>
            <td><asp:LinkButton ID="lbArea" runat="server" ValidationGroup="Input" Text="Area"/></td>
            <td>:</td>
            <td style="width: 360px"><asp:TextBox ID="tbAreaCode" CssClass="TextBox" runat="server" Width="60px" ValidationGroup="Input" /><%--Enabled="False"--%>
              <asp:TextBox ID="tbAreaName" runat="server" CssClass="TextBox" Width="250px" ValidationGroup="Input" />
              <asp:Button ID="btnArea" runat="server" Class="btngo" Text="v" ValidationGroup="Input" />            
            </td>                                    
            <td style="width: 30px"></td>  
            <td>PIC</td>
            <td>:</td>
            <td><asp:TextBox ID="tbPICName" CssClass="TextBox" runat="server" Width="200px" ValidationGroup="Input" /></td>                            
        </tr>
        <tr>
            <td>Alas Hak</td>
            <td>:</td>
            <td><asp:TextBox ID="tbAlasHak" CssClass="TextBox" runat="server" Width="200px" ValidationGroup="Input" /></td>      
            <td style="width: 30px"></td> 
            <td>Perantara</td>
            <td>:</td>
            <td><asp:TextBox ID="tbBrokerName" CssClass="TextBox" runat="server" Width="200px" ValidationGroup="Input" /></td> 
            <td style="width: 10px"></td> 
            <td>No. Telp</td>
            <td>:</td>
            <td><asp:TextBox ID="tbBrokerPhone" CssClass="TextBox" runat="server" Width="90px" ValidationGroup="Input" /></td>                              
        </tr>         
        <tr>
            <td>Remark</td>
            <td>:</td><%--<td colspan="4">--%>
            <td><asp:TextBox ID="tbRemark" runat="server" ValidationGroup="Input"  CssClass="TextBoxMulti" Width="300px"
            MaxLength="255" TextMode="MultiLine" /></td>
            <td style="width: 30px"></td> 
            <td>Pejabat Terkait</td>
            <td>:</td>
            <td><asp:TextBox ID="tbRelatedOffcName" CssClass="TextBox" runat="server" Width="200px" ValidationGroup="Input" /></td>      
            <td style="width: 10px"></td>  
            <td>No. Telp</td>
            <td>:</td>
            <td><asp:TextBox ID="tbRelatedOffcPhone" CssClass="TextBox" runat="server" Width="90px" ValidationGroup="Input" /></td>                                    
        </tr>                        
      </table>        
      <br />      
      <div style="font-size:medium; color:Blue;">Detail</div>
      <hr style="color:Blue" />  
              <asp:Panel runat="server" ID="PnlDt">
              <%--<asp:Button class="bitbtndt btnadd" runat="server" ID="btnAdddt" Text="Add" Visible="false" ValidationGroup="Input" />&nbsp;  OnClientClick="settext(this.value);" --%>         
              <asp:Button ID="btnGetData" runat="server" class="bitbtn btngo" ValidationGroup="Input"  
                     Text="Get Data" Visible="false" Width="75px"/> 
              <%--<asp:Button ID="btnDeleteAll" runat="server" class="bitbtn btndelete" ValidationGroup="Input" OnClientClick="gfg_Run()"
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
                         <asp:DropDownList ID="ddlSubCategory" runat="server" CssClass="DropDownList" ValidationGroup="Input" Width="300px" >                                
                         </asp:DropDownList>                            
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
        <asp:Button ID="btnSaveAll" runat="server" class="bitbtn btnsave" Text="Save & New" ValidationGroup="Input" OnClientClick="return CheckUploadFile(this);" Width="97px"/> 
        &nbsp;    
        <asp:Button ID="btnSaveTrans" runat="server" class="bitbtn btnsave" Text="Save" ValidationGroup="Input" OnClientClick="return CheckUploadFile(this);"/> 
        &nbsp;    
        <asp:Button ID="btnBack" runat="server" class="bitbtn btncancel" Text="Cancel" ValidationGroup="Input"/>  
        &nbsp;
        <asp:Button ID="btnHome" runat="server" class="btngo" Text="Home" Width="48px"/>    
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
      <div id="dlgKegiatan" style="display:none;">
       <%-- Search : <asp:TextBox ID="txtSearchKegiatan" runat="server" /> 
       Search : <asp:TextBox ID="txtSearchKegiatan" CssClass="TextBox" runat="server" Width="200px" />
        <hr />--%>
        <asp:GridView ID="GVKegiatan" runat="server" CssClass="Grid" AutoGenerateColumns="false" > <%-- OnDataBound="OnDataBoundActivity" --%>
        <HeaderStyle CssClass="GridHeader" Wrap="false" ></HeaderStyle>
          <RowStyle CssClass="GridItem" Wrap = "false" /> <%--Gridview-row--%>
          <AlternatingRowStyle CssClass="GridAltItem"/>
          <PagerStyle CssClass="GridPager" HorizontalAlign="Left" />
          <Columns>
            <asp:BoundField HeaderStyle-Width="80px" DataField="KegiatanCode" HeaderText="Kode Kegiatan" /><%--ItemStyle-CssClass="cssSupplier" --%>
            <asp:BoundField HeaderStyle-Width="250px" DataField="KegiatanName" HeaderText="Nama Kegiatan" />
          </Columns>
        </asp:GridView>      
      </div> 

      <div id="dlgArea" style="display:none;">
       <%-- Search : <asp:TextBox ID="txtSearchKegiatan" runat="server" /> 
       Search : <asp:TextBox ID="txtSearchKegiatan" CssClass="TextBox" runat="server" Width="200px" />
        <hr />--%>
        <asp:GridView ID="GVArea" runat="server" CssClass="Grid" AutoGenerateColumns="false"> <%-- OnDataBound="OnDataBoundActivity" --%>
        <HeaderStyle CssClass="GridHeader" Wrap="false" ></HeaderStyle>
          <RowStyle CssClass="GridItem" Wrap = "false" /> <%--Gridview-row--%>
          <AlternatingRowStyle CssClass="GridAltItem"/>
          <PagerStyle CssClass="GridPager" HorizontalAlign="Left" />
          <Columns>
            <asp:BoundField HeaderStyle-Width="80px" DataField="AreaCode" HeaderText="Area Code" /><%--ItemStyle-CssClass="cssSupplier" --%>
            <asp:BoundField HeaderStyle-Width="250px" DataField="AreaName" HeaderText="Area Name" />
          </Columns>
        </asp:GridView>      
      </div> 

   
    <asp:Label runat ="server" ID="lbStatus" ForeColor="Red"/>
    <%--<td colspan="4">   style="display:none;" 
    <div id="boxprogress" class="box"  >
      <div class="chart" data-percent="100" ></div>
    </div>--%>
  
    <div class="posisicircle" runat="server" >
      <div class="pie" data-pie='{ "speed": 30, "percent": 100, "colorSlice": "#DD2C00", "colorCircle": "#f1f1f1", "round": true }'></div>
    </div> 
    
    <input type="range" value="100" min="0" max="100" step="1" style="display:none;" />
    
    <div class="loading" align="center">
    
      <br />
       <img src="../../Image/loader.gif" alt="" />
    </div>
  </form>
</body>
</html>
