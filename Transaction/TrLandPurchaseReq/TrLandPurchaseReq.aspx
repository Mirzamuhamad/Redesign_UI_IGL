<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TrLandPurchaseReq.aspx.vb" Inherits="Transaction_TrLandPurchaseReq_TrLandPurchaseReq" %>

<%@ Register Assembly="BasicFrame.WebControls.BasicDatePicker" Namespace="BasicFrame.WebControls"
    TagPrefix="BDP" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Land Survey</title>

    <script src="../../Function/OpenDlg.js" type="text/javascript"></script>

    <script src="../../Function/Function.JS" type="text/javascript"></script>
    <script type="text/javascript">

     function OpenPopup() {
         window.open("../../SeaDlg.Aspx", "List", "scrollbars=yes,resizable=no,width=500,height=400");
         return false;
     }
     function openprintdlg() {
         var wOpens;
         wOpens = window.open("../../Rpt/PrintForm.Aspx", "List", "scrollbars=yes,resizable=yes,width=500,height=400");
         wOpens.moveTo(0, 0);
         wOpens.resizeTo(screen.width, screen.height);
     }



     function OpenPopup() {
         window.open("../../SearchDlg.Aspx", "List", "scrollbars=yes,resizable=no,width=500,height=400");
         return false;
     }    

     function myPopup() {
            var left = (screen.width - 370) / 2;
            var top = (screen.height - 800) / 2;
             window.open("../../earchDlgV.Aspx", "", 'toolbar=no, location=no, directories=no, status=no, menubar=no, scrollbars=yes, resizable=yes, copyhistory=no, width=' + 370 + ', height=' + 800 + ', top=' + top + ', left=' + left);
             return false;
         }

        

    </script>
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
       
        function setformatdt()
        {
         try
         {  
            var _QtyOutput = parseFloat(document.getElementById("tbQtyM").value.replace(/\$|\,/g,""));
            var _QtyWO = parseFloat(document.getElementById("tbQtyT").value.replace(/\$|\,/g,""));
            var _QtyGood = parseFloat(document.getElementById("tbQtyB").value.replace(/\$|\,/g,""));
//            var _QtyRepair = parseFloat(document.getElementById("tbQtyRepair").value.replace(/\$|\,/g,""));
            var _QtyReject = parseFloat(document.getElementById("tbQtyS").value.replace(/\$|\,/g,""));
            
            
         
            document.getElementById("tbQtyM").value = setdigit(_QtyM,'<%=VIEWSTATE("DigitQty")%>');
            document.getElementById("tbQtyT").value = setdigit(_QtyT,'<%=VIEWSTATE("DigitQty")%>');
            document.getElementById("tbQtyB").value = setdigit(_QtyB,'<%=VIEWSTATE("DigitQty")%>');
            document.getElementById("tbQtyS").value = setdigit(_QtyS,'<%=VIEWSTATE("DigitQty")%>');            
            //alert("test 2");                                                
            
        }catch (err){
            alert(err.description);
          }      
        }   
        

        //      function setformathd(prmchange)
        // {
        //  try {


        //     if (document.getElementById("ddlHitungTotal").value == 'SPPT')
        //   {

        //     document.getElementById("tbTotal").value = parseFloat(document.getElementById("tbSPPT").value.replace(/\$|\,/g,"")) * parseFloat(document.getElementById("tbNilai").value.replace(/\$|\,/g,""));

        //     document.getElementById("tbHrgFix").value =  parseFloat(document.getElementById("tbSPPT").value.replace(/\$|\,/g,""));
                     
        //   } 


        //    if (document.getElementById("ddlHitungTotal").value == 'AJB/SPH/SHM')
        //   {

        //     document.getElementById("tbTotal").value = parseFloat(document.getElementById("tbAjbSphShm").value.replace(/\$|\,/g,"")) * parseFloat(document.getElementById("tbNilai").value.replace(/\$|\,/g,""));

        //     document.getElementById("tbHrgFix").value =  parseFloat(document.getElementById("tbAjbSphShm").value.replace(/\$|\,/g,""));
                     
        //   } 

        //   if (document.getElementById("ddlHitungTotal").value == 'Luas Ukur')
        //   {

        //     document.getElementById("tbTotal").value = parseFloat(document.getElementById("tbLuasUkur").value.replace(/\$|\,/g,"")) * parseFloat(document.getElementById("tbNilai").value.replace(/\$|\,/g,""));

        //     document.getElementById("tbHrgFix").value =  parseFloat(document.getElementById("tbLuasUkur").value.replace(/\$|\,/g,""));
                     
        //   } 
          

        //   document.getElementById("tbLuasUkur").value = setdigit(document.getElementById("tbLuasUkur").value.replace(/\$|\,/g,""),'<%=ViewState("DigitCurr")%>');

        //     document.getElementById("tbAjbSphShm").value = setdigit(document.getElementById("tbAjbSphShm").value.replace(/\$|\,/g,""),'<%=ViewState("DigitCurr")%>'); 

        //     document.getElementById("tbHrgFix").value = setdigit(document.getElementById("tbHrgFix").value.replace(/\$|\,/g,""),'<%=ViewState("DigitCurr")%>'); 
            
        //     document.getElementById("tbSPPT").value = setdigit(document.getElementById("tbSPPT").value.replace(/\$|\,/g,""),'<%=ViewState("DigitCurr")%>');  

        //      document.getElementById("tbTotal").value = setdigit(document.getElementById("tbTotal").value.replace(/\$|\,/g,""),'<%=ViewState("DigitCurr")%>');  

        //       document.getElementById("tbNilai").value = setdigit(document.getElementById("tbNilai").value.replace(/\$|\,/g,""),'<%=ViewState("DigitCurr")%>');            
        // }catch (err){
        //     alert(err.description);
        //   }      
        // } 




         function UploadKtp(fileUploadKtp) {
                if (fileUploadKtp.value != '') {
                    document.getElementById("<%=btnKtp.ClientID %>").click();
                }
            } 

         function UploadKK(fileUploadKK) {
                if (fileUploadKK.value != '') {
                    document.getElementById("<%=btnSaveKK.ClientID %>").click();
                }
            }

        function UploadSPPT(fileUploadSPPT) {
                if (fileUploadSPPT.value != '') {
                    document.getElementById("<%=btnSaveSPPT.ClientID %>").click();
                }
            } 

            function UploadSTTS(fileUploadSTTS) {
                if (fileUploadSTTS.value != '') {
                    document.getElementById("<%=btnSaveSTTS.ClientID %>").click();
                }
            }  

            function UploadTTD(fileUploadTTD) {
                if (fileUploadTTD.value != '') {
                    document.getElementById("<%=btnSaveTTD.ClientID %>").click();
                }
            }  

            function UploadAJB(fileUploadAJB) {
                if (fileUploadAJB.value != '') {
                    document.getElementById("<%=btnSaveAJB.ClientID %>").click();
                }
            }

             function UploadAJB2(fileUploadAJB2) {
                if (fileUploadAJB2.value != '') {
                    document.getElementById("<%=btnSaveAJB2.ClientID %>").click();
                }
            } 

             function UploadSSP(fileUploadSSP) {
                if (fileUploadSSP.value != '') {
                    document.getElementById("<%=btnSaveSSP.ClientID %>").click();
                }
            }  


            function UploadSSD(fileUploadSSD) {
                if (fileUploadSSD.value != '') {
                    document.getElementById("<%=btnSaveSSD.ClientID %>").click();
                }
            }

           
            function UploadSKTS(fileUploadSKTS) {
                if (fileUploadSKTS.value != '') {
                    document.getElementById("<%=btnSaveSKTS.ClientID %>").click();
                }
            }     

            function UploadSPBT(fileUploadSPBT) {
                if (fileUploadSPBT.value != '') {
                    document.getElementById("<%=btnSaveSPBT.ClientID %>").click();
                }
            }  

            function UploadSPKTT(fileUploadSPKTT) {
                if (fileUploadSPKTT.value != '') {
                    document.getElementById("<%=btnSaveSPKTT.ClientID %>").click();
                }
            }  

            function UploadBAM(fileUploadBAM) {
                if (fileUploadBAM.value != '') {
                    document.getElementById("<%=btnSaveBAM.ClientID %>").click();
                }
            }  

            function UploadBAPL(fileUploadBAPL) {
                if (fileUploadBAPL.value != '') {
                    document.getElementById("<%=btnSaveBAPL.ClientID %>").click();
                }
            }  

            function UploadPTPP(fileUploadPTPP) {
                if (fileUploadPTPP.value != '') {
                    document.getElementById("<%=btnSavePTPP.ClientID %>").click();
                }
            } 

             function UploadPTPP2(fileUploadPTPP2) {
                if (fileUploadPTPP2.value != '') {
                    document.getElementById("<%=btnSavePTPP2.ClientID %>").click();
                }
            }  


            function UploadSKRT(fileUploadSKRT) {
                if (fileUploadSKRT.value != '') {
                    document.getElementById("<%=btnSaveSKRT.ClientID %>").click();
                }
            } 


            function UploadSKD(fileUploadSKD) {
                if (fileUploadSKD.value != '') {
                    document.getElementById("<%=btnSaveSKD.ClientID %>").click();
                }
            } 


            function UploadFcGirik(fileUploadFcGirik) {
                if (fileUploadFcGirik.value != '') {
                    document.getElementById("<%=btnSaveFcGirik.ClientID %>").click();
                }
            } 


            function UploadFDP(fileUploadFDP) {
                if (fileUploadFDP.value != '') {
                    document.getElementById("<%=btnSaveFDP.ClientID %>").click();
                }
            } 


            function UploadPatok(fileUploadPatok) {
                if (fileUploadPatok.value != '') {
                    document.getElementById("<%=btnSavePatok.ClientID %>").click();
                }
            } 

            function UploadSporadik(fileUploadSporadik) {
                if (fileUploadSporadik.value != '') {
                    document.getElementById("<%=btnSaveSporadik.ClientID %>").click();
                }
            } 

            function UploadAHU(fileUploadAHU) {
                if (fileUploadAHU.value != '') {
                    document.getElementById("<%=btnSaveAHU.ClientID %>").click();
                }
            } 

            function UploadSejarah(fileUploadSejarah) {
                if (fileUploadSejarah.value != '') {
                    document.getElementById("<%=btnSaveSejarah.ClientID %>").click();
                }
            } 

            function UploadSPJH(fileUploadSPJH) {
                if (fileUploadSPJH.value != '') {
                    document.getElementById("<%=btnSaveSPJH.ClientID %>").click();
                }
            } 

            function UploadLainLain(fileUploadLainLain) {
                if (fileUploadLainLain.value != '') {
                    document.getElementById("<%=btnSaveLainLain.ClientID %>").click();
                }
            } 

            function UploadKTPW(fileUploadKTPW) {
                if (fileUploadKTPW.value != '') {
                    document.getElementById("<%=btnSaveKTPW.ClientID %>").click();
                }
            } 

            function UploadSPW(fileUploadSPW) {
                if (fileUploadSPW.value != '') {
                    document.getElementById("<%=btnSaveSPW.ClientID %>").click();
                }
            } 

            function UploadSPKW(fileUploadSPKW) {
                if (fileUploadSPKW.value != '') {
                    document.getElementById("<%=btnSaveSPKW.ClientID %>").click();
                }
            } 

            function UploadBPJS(fileUploadBPJS) {
                if (fileUploadBPJS.value != '') {
                    document.getElementById("<%=btnSaveBPJS.ClientID %>").click();
                }
            } 


            

            function UploadLainLain2(fileUploadLainLain2) {
                if (fileUploadLainLain2.value != '') {
                    document.getElementById("<%=btnSaveLainLain2.ClientID %>").click();
                }
            } 

          function UploadLainLain3(fileUploadLainLain3) {
                if (fileUploadLainLain3.value != '') {
                    document.getElementById("<%=btnSaveLainLain3.ClientID %>").click();
                }
            } 



             function Reject()
                {
                    try
                    {
                        var result = prompt("Remark Reject", "");
                        if (result){
                            document.getElementById("HiddenRemarkReject").value = result;
                        } else {
                            document.getElementById("HiddenRemarkReject").value = "False Value";
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

    </script>

    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
</head>
<body>


    <form id="form1" runat="server">


    <div class="Content">
        <div class="H1">Land Survey</div>
        <hr style="color: Blue" />
        <asp:Panel runat="server" ID="PnlHd">
            <table>
                <tr>
                    <td style="width: 100px; text-align: right">
                        Quick Search :
                    </td>
                  
                    <td>
                        <asp:TextBox CssClass="TextBox" runat="server" ID="tbFilter" />
                        <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlField">
                            <asp:ListItem Value="TransNmbr" Selected="True">Reference</asp:ListItem>
                              <asp:ListItem>Status</asp:ListItem>
                              <asp:ListItem Value="dbo.FormatDate(TransDate)">Date</asp:ListItem>
                              <asp:ListItem Value="Kohir">Nomor Kohir</asp:ListItem>
                                  <asp:ListItem Value="NoDocSertifikat">Nomor Dokumen</asp:ListItem>
                              <asp:ListItem Value="FgImport">Data Import</asp:ListItem>
                              <asp:ListItem>Remark</asp:ListItem>
                            
                        </asp:DropDownList>
                        <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlRange">
                        </asp:DropDownList>
                        <asp:Button class="bitbtn btnsearch" runat="server" ID="btnSearch" Text="Search" />
                        <asp:Button class="bitbtn btngo" runat="server" ID="btnExpand" Text="..." />
                    </td>
                    <td>
                        <asp:LinkButton ID="LbAdvSearch" runat="server" Text="Advanced Search" />
                        Show Records :
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
                        <td style="width: 100px; text-align: right">
                            <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlNotasi">
                                <asp:ListItem Selected="true" Text="OR" Value="OR"></asp:ListItem>
                                <asp:ListItem Text="AND" Value="AND"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:TextBox runat="server" CssClass="TextBox" ID="tbfilter2" />
                            <asp:DropDownList runat="server" CssClass="DropDownList" ID="ddlField2">
                               <asp:ListItem Value="TransNmbr" Selected="True">Reference</asp:ListItem>
                                  <asp:ListItem>Status</asp:ListItem>
                                  <asp:ListItem Value="dbo.FormatDate(TransDate)">Date</asp:ListItem>
                                  <asp:ListItem Value="Kohir">Nomor Kohir</asp:ListItem>
                                  <asp:ListItem Value="NoDocSertifikat">Nomor Dokumen</asp:ListItem>
                                  <asp:ListItem Value="FgImport">Data Import</asp:ListItem>
                                  <asp:ListItem>Remark</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <br />
            <asp:Button class="bitbtn btnadd" runat="server" ID="BtnAdd" Text="Add" />
            &nbsp &nbsp &nbsp
            <asp:DropDownList CssClass="DropDownList" ID="ddlCommand" runat="server" Visible="false" />
            <asp:Button class="btngo" runat="server" ID="BtnGo" Text="G" Visible="false" />
            <br />
            <div style="border: 0px  solid; width: 100%; height: 100%; overflow: auto;">
                <asp:GridView ID="GridView1" runat="server" AllowPaging="True" AllowSorting="True"
                    CssClass="Grid" AutoGenerateColumns="False">
                    <HeaderStyle CssClass="GridHeader" Wrap="false"></HeaderStyle>
                    <RowStyle CssClass="GridItem" Wrap="false" />
                    <AlternatingRowStyle CssClass="GridAltItem" />
                    <PagerStyle CssClass="GridPager" />
                    <Columns>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:CheckBox ID="cbSelectHd" runat="server" AutoPostBack="true" OnCheckedChanged="cbSelectHd_CheckedChanged" />
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
                                    <asp:ListItem Text="Print Check Doc" />
                                    <asp:ListItem Text="Print Riwayat" />
                                    <asp:ListItem Text="Print Letter C" /> 
                                    <asp:ListItem Text="Reject" />                                   
                                    
                                    <%--<asp:ListItem Text="Print" />--%>
                                </asp:DropDownList>
                                <asp:Button class="btngo" runat="server" ID="BtnGo" Text="G" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>"
                                    CommandName="Go" />
                            </ItemTemplate>
                            <HeaderStyle Width="110px" />
                        </asp:TemplateField>
                        <asp:BoundField DataField="TransNmbr" HeaderStyle-Width="120px" SortExpression="TransNmbr"
                            HeaderText="Request No">
                            <HeaderStyle Width="120px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Status" HeaderText="Status"></asp:BoundField>
                        <asp:BoundField DataField="TransDate" DataFormatString="{0:dd MMM yyyy}" HtmlEncode="true"
                            HeaderStyle-Width="80px" SortExpression="TransDate" 
                            HeaderText="Date">
                            <HeaderStyle Width="80px" />
                        </asp:BoundField>
                      
                        <asp:BoundField DataField="Block" HeaderStyle-Width="200px" SortExpression="Block"
                            HeaderText="Block No">
                            <HeaderStyle Width="200px" />
                        </asp:BoundField>

                        <asp:BoundField DataField="Kohir" HeaderStyle-Width="200px" SortExpression="Kohir"
                            HeaderText="Kohir No">
                            <HeaderStyle Width="200px" />
                        </asp:BoundField>

                        <asp:BoundField DataField="Persil" HeaderStyle-Width="200px" SortExpression="Persil"
                            HeaderText="Persil No">
                            <HeaderStyle Width="200px" />
                        </asp:BoundField>

                        <asp:BoundField DataField="JnsDocSertifikat" HeaderStyle-Width="200px" SortExpression="JnsDocSertifikat"
                            HeaderText="Jenis Dokumen">
                            <HeaderStyle Width="200px" />
                        </asp:BoundField>

                        <asp:BoundField DataField="NoDocSertifikat" HeaderStyle-Width="200px" SortExpression="NoDocSertifikat"
                            HeaderText="No Dokumen">
                            <HeaderStyle Width="200px" />
                        </asp:BoundField>

                        <asp:BoundField DataField="TglTerbit" DataFormatString="{0:dd MMM yyyy}" HtmlEncode="true" HeaderStyle-Width="200px" SortExpression="TglTerbit"
                            HeaderText="Tanggal Terbit">
                            <HeaderStyle Width="200px" />
                        </asp:BoundField>
                        
                        <asp:BoundField DataField="HrgFix" ItemStyle-HorizontalAlign="right" HeaderText="Luas" DataFormatString="{0:#,##0.##}" SortExpression="LuasUkur"> 
                            <HeaderStyle Width="300px" />
                        </asp:BoundField>

                        <asp:BoundField DataField="Remark" HeaderStyle-Width="250px" 
                            HeaderText="Remark" SortExpression="Remark">
                            <HeaderStyle Width="250px" />
                        </asp:BoundField>

                          <asp:BoundField DataField="RemarkReject" HeaderStyle-HorizontalAlign ="Center" HeaderStyle-Width="250px" 
                            HeaderText="Reject" SortExpression="RemarkReject">
                            <HeaderStyle Width="250px" />
                        </asp:BoundField>
                        
                    </Columns>
                </asp:GridView>
            </div>
            <asp:Panel runat="server" ID="pnlNav" Visible="false">
                <asp:Button class="bitbtn btnadd" runat="server" ID="btnAdd2" Text="Add" />
                &nbsp &nbsp &nbsp
                <asp:DropDownList ID="ddlCommand2" CssClass="DropDownList" runat="server" />
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
                    <asp:MenuItem Text="General" Value="0"></asp:MenuItem>
                    <asp:MenuItem Text="Perlengkapan Dokumen" Value="1"></asp:MenuItem>                    
                    <asp:MenuItem Text="Perlengkapan Dokumen 2" Value="2"></asp:MenuItem>
                </Items>
            </asp:Menu>
                    
                </td>
                <td>
                <asp:Button class="bitbtndt btnedit" runat="server" ID="btnGoEdit" Text="Edit Data" /> 
                </td>
            </tr>
        </table>
           

        <br /> 
       

        <asp:MultiView ID="MultiView2" runat="server" ActiveViewIndex="0">
            <asp:View ID="Tab0" runat="server">
                <table>
                    <tr>
                        <td>Offering Survey No</td>
                        <td>:</td>
                        <td><asp:TextBox CssClass="TextBoxR" runat="server" ID="tbCode" Width="225px" Enabled="False"/>
                        </td> 
                         <td>Offering Survey Date</td>
                        <td>:</td>
                        <td>
                        <BDP:BasicDatePicker ID="tbDate" runat="server" DateFormat="dd MMM yyyy" 
                                    ReadOnly = "True" ValidationGroup="Input"
                                    ButtonImageHeight="19px" ButtonImageWidth="20px" 
                                    DisplayType="TextBoxAndImage" 
                                    TextBoxStyle-CssClass="TextDate" AutoPostBack="True" 
                                    ShowNoneButton="False"><TextBoxStyle CssClass="TextDate" /></BDP:BasicDatePicker> </td>
                        
                                  
                        
                    </tr>
                    
                    <tr>
                        <td><asp:LinkButton ID="lbSeller" ValidationGroup="Input" runat="server" Text="Seller"/></td>
                        <td>:</td>
                        
                        <td>
                            <asp:TextBox runat="server" ValidationGroup="Input" ID="tbSeller" CssClass="TextBox" Visible="false" Width="225px"/>
                        <asp:TextBox runat="server" ValidationGroup="Input" ID="tbSellerName" CssClass="TextBox" Enabled="false" Width="192px"/>
                         <asp:Button ID="btnSeller" runat="server" Class="btngo" visible = True Text="v" />
                            <asp:DropDownList CssClass="DropDownList" Width="230px" ValidationGroup="Input" Visible="False" runat="server" ID="ddlseller" AutoPostBack="false" /></td>  
                       

                         <td><asp:LinkButton ID="lbModerator" ValidationGroup="Input" runat="server" Text="Moderator"/></td>
                        <td>:</td>
                        <td>
                            <asp:TextBox runat="server" ValidationGroup="Input" ID="tbModerator" CssClass="TextBox" Visible="false" Width="225px"/>
                        <asp:TextBox runat="server" ValidationGroup="Input" ID="tbModeratorName" CssClass="TextBox" Enabled="false" Width="192px"/>
                         <asp:Button ID="btnModerator" runat="server" Class="btngo" visible = True Text="v" />

                            <asp:DropDownList CssClass="DropDownList" Width="230px" ValidationGroup="Input" runat="server" ID="ddlModerator" Visible="False" AutoPostBack="false" /></td>                  
                    </tr> 

                    <tr>          
                       <td>Nama Pembeli</td>
                        <td>:</td>
                        <td><asp:TextBox runat="server" MaxLength ="100" ValidationGroup="Input" ID="tbNamaPembeli" CssClass="TextBox" Width="225px"/></td>

                    </tr>

                    
                    <tr>
                        <td><asp:LinkButton ID="lbArea" ValidationGroup="Input" runat="server" Text="Area"/></td>
                        <td>:</td>
                        <td>
                            <asp:TextBox runat="server" ValidationGroup="Input" ID="tbArea" CssClass="TextBox" Visible="false" Width="225px"/>
                        <asp:TextBox runat="server" ValidationGroup="Input" ID="tbAreaName" CssClass="TextBox" Enabled="false"  Width="192px"/>
                         <asp:Button ID="btnArea" runat="server" Class="btngo" visible = True Text="v" />

                            <asp:DropDownList CssClass="DropDownList" Width="230px" ValidationGroup="Input" runat="server" ID="ddlArea" Visible="False"  AutoPostBack="false" /></td> 
                       
                       
                       </td> 
                        <td>Land type</td>
                        <td>:</td>
                        <td><asp:DropDownList CssClass="DropDownList"  ValidationGroup="Input" Width="230px" runat="server" ID="ddlLandType" >
                             <asp:ListItem Selected="True">Sporadik</asp:ListItem>                                        
                                        <asp:ListItem>Kavling</asp:ListItem>
                            </asp:DropDownList></td>     
 
                    </tr>    
                    
                    
                    <tr>
                            <td>Dok Sertifikat</td>
                            <td>:</td>
                            <td colspan="7">
                                <table>
                                    <tr style="background-color:Silver;text-align:center; border-radius:30px;">
                                        <td>Jenis Dokumen</td>
                                        <td>Nomor Dokumen</td>
                                        <td>Tanggal Terbit</td>
                                        <td>Masa Berlaku</td>                           
                                    </tr>
                                    <tr>
                                         <td>
                                             <asp:DropDownList ID="ddlJenisDokumen" runat="server" AutoPostBack="true" 
                                                 CssClass="DropDownList" ValidationGroup="Input" Width="200px">
                                                 <asp:ListItem Selected="True">Choose One</asp:ListItem>
                                                 <asp:ListItem>AJB</asp:ListItem>
                                                 <asp:ListItem>SPH</asp:ListItem>
                                                 <asp:ListItem>SHM</asp:ListItem>
                                                 <asp:ListItem>SHGB</asp:ListItem>
                                                 <asp:ListItem>SHGU</asp:ListItem>
                                             </asp:DropDownList>
                                         </td>  
                                         <td><asp:TextBox runat="server" MaxLength ="200" ValidationGroup="Input" ID="tbNoDokumen" CssClass="TextBox" Width="225px"/></td>    
                                         <td>
                                             <BDP:BasicDatePicker ID="tbTglTerbit" runat="server" AutoPostBack="false" 
                                                 ButtonImageHeight="19px" ButtonImageWidth="20px" DateFormat="dd MMM yyyy" 
                                                 DisplayType="TextBoxAndImage" ReadOnly="False" ShowNoneButton="False" 
                                                 TextBoxStyle-CssClass="TextDate" ValidationGroup="Input" Width="225px">
                                                 <TextBoxStyle CssClass="TextDate" />
                                             </BDP:BasicDatePicker>
                                         </td> 
                                         <td>
                                             <BDP:BasicDatePicker ID="tbMasaBerlaku" runat="server" AutoPostBack="false" 
                                                 ButtonImageHeight="19px" ButtonImageWidth="20px" DateFormat="dd MMM yyyy" 
                                                 DisplayType="TextBoxAndImage" ReadOnly="False" Enabled="false" ShowNoneButton="False" 
                                                 TextBoxStyle-CssClass="TextDate" ValidationGroup="Input" Width="225px">
                                                 <TextBoxStyle CssClass="TextDate" />
                                             </BDP:BasicDatePicker>
                                         </td>                                              
                                    </tr>
                                </table>
                            </td>  
                                       
                    </tr> 
                    
                     

                    <tr>
                            <td>Luas</td>
                            <td>:</td>
                            <td colspan="7">
                                <table>
                                    <tr style="background-color:Silver;text-align:center; border-radius:30px;">
                                        <td>SPPT</td>
                                        <td>AJB/SPH/SHM</td>
                                        <td>Luas Ukur</td>
                                    </tr>
                                    <tr>
                                        <td><asp:TextBox ID="tbSPPT" runat="server" CssClass="TextBox" Width="100px" AutoPostBack="true"/></td>
                                        <td><asp:TextBox ID="tbAjbSphShm" ValidationGroup="Input" runat="server" CssClass="TextBox" width="100px" AutoPostBack="true"/></td>
                                        <td><asp:TextBox ID="tbLuasUkur" runat="server" CssClass="TextBox" Width="100px" AutoPostBack="true"/></td>                          
                                    </tr>
                                </table>
                            </td>                
                    </tr>

                    <tr>
                        <td>Nilai Tanah/m<sup>2</sup></td>
                        <td>:</td>
                        <td><asp:TextBox runat="server" ValidationGroup="Input" ID="tbNilai" CssClass="TextBox" Width="213px" AutoPostBack="true"/> x 
                            <asp:DropDownList CssClass="DropDownList"  ValidationGroup="Input" Width="100px" runat="server" ID="ddlHitungTotal" AutoPostBack="true">
                             <asp:ListItem Selected="True">Choose One</asp:ListItem>
                                        <asp:ListItem>SPPT</asp:ListItem>
                                        <asp:ListItem>AJB/SPH/SHM</asp:ListItem>
                                        <asp:ListItem>Luas Ukur</asp:ListItem>
                            </asp:DropDownList>
                            <asp:TextBox runat="server" ValidationGroup="Input" ID="tbHrgFix" Visible="false" CssClass="TextBox" Width="100px" Enabled="false"/>
                        </td>


                        

                        <td>Total</td>
                        <td>:</td>
                        <td><asp:TextBox runat="server" ValidationGroup="Input" ID="tbTotal" CssClass="TextBox" Width="225px" Enabled="false"/></td>

                        
                    </tr> 

                    <tr>
                            <td>Address</td>
                            <td>:</td>
                            <td colspan="7">
                                <table>
                                    <tr style="background-color:Silver;text-align:center; border-radius:30px;">
                                        
                                        <td>Provinsi</td>
                                        <td>Kabupaten</td>
                                        <td>Kecamatan</td>
                                        <td>Desa</td>
                                        <td>Address</td>                           
                                    </tr>
                                    <tr>
                                           
                                         <td><asp:TextBox runat="server" MaxLength ="30" ValidationGroup="Input" ID="tbProvinsi" CssClass="TextBox" Width="150px"/></td>    
                                         <td><asp:TextBox runat="server" MaxLength ="50" ValidationGroup="Input" ID="tbKab" CssClass="TextBox" Width="150px"/></td> 
                                         <td><asp:TextBox runat="server" MaxLength ="50" ValidationGroup="Input" ID="tbKec" CssClass="TextBox" Width="150px"/></td>
                                         <td><asp:TextBox runat="server" MaxLength ="100" ValidationGroup="Input" ID="tbDesa" CssClass="TextBox" Width="150px"/></td>
                                         <td><asp:TextBox runat="server" MaxLength ="200" ValidationGroup="Input" ID="tbAddress" CssClass="TextBox" Width="200px"/></td>                                              
                                    </tr>
                                </table>
                            </td>  
                                       
                    </tr> 
                    
                    <tr>
                    <td>No SPPT/PBB</td>
                        <td>:</td>
                        <td><asp:TextBox runat="server" MaxLength ="100" ValidationGroup="Input" ID="tbSptPbb" CssClass="TextBox" Width="225px"/></td>
                    
                    
                     <td>No Peta Rincik</td>
                        <td>:</td>
                        <td><asp:TextBox runat="server" MaxLength ="100" ValidationGroup="Input" ID="tbPetaRincikNo" CssClass="TextBox" Width="225px"/></td>
                    
                    <tr>
                    
                    <td>No Girik Blok</td>
                        <td>:</td>
                        <td><asp:TextBox runat="server" MaxLength ="100" ValidationGroup="Input" ID="tbBlok" CssClass="TextBox" Width="225px"/></td>
                       
                        <td>No NIB</td>
                        <td>:</td>
                        <td><asp:TextBox runat="server" MaxLength ="100" ValidationGroup="Input" ID="tbBNIB" CssClass="TextBox" Width="225px"/></td>
                          
                    </tr> 
                    
                     <tr>
                         <td>No Girik Kohir</td>
                        <td>:</td>
                        <td><asp:TextBox runat="server" MaxLength ="100" ValidationGroup="Input" ID="tbKohir" CssClass="TextBox" Width="225px"/></td>
                        
                        <td>No Surat Ukur</td>
                        <td>:</td>
                        <td><asp:TextBox runat="server" MaxLength ="100" ValidationGroup="Input" ID="tbNoSuratUkur" CssClass="TextBox" Width="225px"/></td>

                        
                    </tr> 
                    
                     <tr>

                        <td>No Girik Percil</td>
                        <td>:</td>
                        <td><asp:TextBox runat="server" MaxLength ="100" ValidationGroup="Input" ID="tbPercil" CssClass="TextBox" Width="225px"/></td>
                        
                        <td>No Lain-Lain</td>
                        <td>:</td>
                        <td><asp:TextBox runat="server" MaxLength ="225" ValidationGroup="Input" ID="tbNoLainLian" CssClass="TextBox" Width="225px"/></td>

                    </tr>

                    <tr>
                    
                        <td>Remark Reject</td>
                        <td>:</td>
                        <td><asp:TextBox runat="server" TextMode="MultiLine" ValidationGroup="Input" Enabled= false ID="tbRemarkReject" CssClass="TextBox" Width="225px"/></td>
                        
                        <td>Remark</td>
                        <td>:</td>
                        <td><asp:TextBox runat="server" MaxLength ="255" TextMode="MultiLine" ValidationGroup="Input" ID="tbRemark" CssClass="TextBox" Width="405px"/></td>
                    </tr>  
                    
                    

                     <tr>
                     
                      <%--<td>No AJB</td>
                        <td>:</td>--%>
                        <td><asp:TextBox runat="server" ValidationGroup="Input" ID="tbAJB" Visible ="false" CssClass="TextBox" Width="225px"/></td>
                        

                       <%-- <td>No SPH</td>
                        <td>:</td>--%>
                        <td><asp:TextBox runat="server" ValidationGroup="Input" ID="tbSPH" Visible ="false" CssClass="TextBox" Width="225px"/></td>
                    </tr>      
                          

                    <tr>
                        


                       <%-- <td>No SHM</td>
                        <td>:</td>--%>
                        <td><asp:TextBox runat="server" ValidationGroup="Input" ID="tbSHM" Visible ="false"  CssClass="TextBox" Width="225px"/></td>
                        
                        <%--<td>Jenis Dokumen</td>
                        <td>:</td> --%>         
                        <td><asp:DropDownList CssClass="DropDownList" ValidationGroup="Input" Visible ="False" Width="230px" runat="server" ID="ddlJenisDok" >
                             <asp:ListItem Selected="True">AJB</asp:ListItem>
                                        <asp:ListItem>SPH</asp:ListItem>
                                        <asp:ListItem>SHM</asp:ListItem>
                            </asp:DropDownList>
                           <%--&nbsp; &nbsp; &nbsp; &nbsp; No Doc : --%>        
                           <asp:TextBox ID="tbNoDocHD" runat="server" CssClass="TextBox" Visible = "False" Width="105px"/>
                        </td>
                    </tr> 

                     
                </table>
             </asp:View>

            <asp:View ID="Tab1" runat="server">
                <table>

                    <tr>
                        <td>
                        <asp:checkbox id="cbKtp" Enabled="True" runat="server" text=" KTP Penjual" autopostback="False"/>
                        </td>
                        
                        <td> 
                        <asp:FileUpload style="color: White;" runat="server" accept="application/pdf" ID="FupMain"  />
                       <!--  <asp:RegularExpressionValidator ID="FileUpLoadValidator" runat="server" ErrorMessage="Upload PDF files only .!!!"
                            ValidationExpression="^(([a-zA-Z]:)|(\\{2}\w+)\$?)(\\(\w[\w].*))(.pdf|.PDF)$" 
                           ControlToValidate="FupMain"> 
                        </asp:RegularExpressionValidator>  -->
                        <asp:Label ID="lblmassageKTP" runat="server"  CssClass="labelMassage" Visible = "false" Text="File uploaded successfully"></asp:Label>
                        </td> 

                        
        

                        <td>
                            <asp:Button ID="btnKtp" Text="Upload" runat="server" Style="display: none" />   
                           <asp:LinkButton ID="lbKtp" ValidationGroup="Input" runat="server" Text="Not Yet Uploaded"/>        
                        </td>

                        <td>                            
                           <asp:LinkButton ID="btnDelKTP" visible="false" class="bitbtndt btndelete" ForeColor="#e36e6e" ValidationGroup="Input" runat="server" Text="Delete"  OnClientClick="return confirm('Sure to delete this dokumen?');"/>        
                        </td>
                    </tr>

                    <tr>
                        <td>
                        <asp:checkbox id="cbBPJS" Enabled="True" runat="server" text=" BPJS Penjual" autopostback="False"/>
                        </td>
                        
                        <td> 

                        <asp:FileUpload style="color: White;" runat="server" accept="application/pdf" ID="FubBPJS"  />
                       <!--  <asp:RegularExpressionValidator ID="FileUpLoadValidator30" runat="server" ErrorMessage="Upload PDF files only .!!!"
                            ValidationExpression="^(([a-zA-Z]:)|(\\{2}\w+)\$?)(\\(\w[\w].*))(.pdf|.PDF)$" 
                           ControlToValidate="FubBPJS"> 
                        </asp:RegularExpressionValidator>  -->
                        <asp:Label ID="lblmassageBPJS" runat="server"  CssClass="labelMassage" Visible = "false" Text="File uploaded successfully"></asp:Label>
                        </td> 

                        
        

                        <td>
                            <asp:Button ID="btnSaveBPJS" Text="Upload" runat="server" Style="display: none" />   
                            <!-- <asp:HyperLink id="hlKtp" NavigateUrl="~/Image/Dokumen/IGLLPR22020002Invoice.pdf" runat="server" Text="View PDF" Target="_blank" /> -->
                           <asp:LinkButton ID="lbBPJS" ValidationGroup="Input" runat="server" Text="Not Yet Uploaded"/>        
                            <!-- <asp:Button ID="btnsaveKtp" CssClass="bitbtndt btnadd" runat="server"  OnClientClick = "SetTarget();" Text="View" /> 
 -->
                        </td>
                    </tr>

                    <tr>
                        <td>
                        <asp:checkbox id="cbKk" runat="server" text=" KK Permohonan Penjual" autopostback="False"/>
                        </td>
                        
                        <td> 
                        <asp:FileUpload style="color: White;" runat="server" accept="application/pdf" ID="FubKK"  />
                        <!-- <asp:RegularExpressionValidator ID="FileUpLoadValidator2" runat="server" ErrorMessage="Upload PDF files only .!!!"
                            ValidationExpression="^(([a-zA-Z]:)|(\\{2}\w+)\$?)(\\(\w[\w].*))(.pdf|.PDF)$" 
                           ControlToValidate="FubKK"> 
                        </asp:RegularExpressionValidator>  --> 
                         <asp:Label ID="lblmassageKK" CssClass="labelMassage" runat="server"  Visible = "False" Text="File uploaded successfully"></asp:Label>                              
                        
                        </td>            

                        <td>
                             <asp:LinkButton ID="lbKK" ValidationGroup="Input" runat="server" Text="Not Yet Uploaded"/>
                            <asp:Button ID="btnsaveKK" CssClass="bitbtndt btnadd" runat="server" Text="View" Style="display: none" /> 
                        </td>
                    </tr>

                    <tr>
                        <td>
                        <asp:checkbox id="cbSPPT" runat="server" text=" SPPT PBB tanah berjalan" autopostback="False"/>
                        </td>
                        
                        <td> 
                        <asp:FileUpload style="color: White;" runat="server" accept="application/pdf" ID="FubSPPT"  />
                        <!-- <asp:RegularExpressionValidator ID="FileUpLoadValidator3" runat="server" ErrorMessage="Upload PDF files only .!!!"
                            ValidationExpression="^(([a-zA-Z]:)|(\\{2}\w+)\$?)(\\(\w[\w].*))(.pdf|.PDF)$" 
                           ControlToValidate="FubSPPT"> 
                        </asp:RegularExpressionValidator> -->    
                        <asp:Label ID="lblmassageSPPT" CssClass="labelMassage" runat="server"  Visible = "False" Text="File uploaded successfully"></asp:Label>                             
                        
                        </td>            

                        <td>
                             <asp:LinkButton ID="lbSPPT" ValidationGroup="Input" runat="server" Text="Not Yet Uploaded"/>
                            <asp:Button ID="btnsaveSPPT" CssClass="bitbtndt btnadd" runat="server" Text="View" Style="display: none" /> 
                        </td>
                    </tr>

                    <tr>
                        <td>
                        <asp:checkbox id="cbSTTS" runat="server" text=" STTS tanah 10 tahun terkahir berjalan" autopostback="False"/>
                        </td>
                        
                        <td> 
                        <asp:FileUpload style="color: White;" runat="server" accept="application/pdf" ID="FubSTTS"  />
                        <!-- <asp:RegularExpressionValidator ID="FileUpLoadValidator4" runat="server" ErrorMessage="Upload PDF files only .!!!"
                            ValidationExpression="^(([a-zA-Z]:)|(\\{2}\w+)\$?)(\\(\w[\w].*))(.pdf|.PDF)$" 
                           ControlToValidate="FubSTTS"> 
                        </asp:RegularExpressionValidator> -->   
                        <asp:Label ID="lblmassageSTTS" CssClass="labelMassage" runat="server"  Visible = "False" Text="File uploaded successfully"></asp:Label>                                
                        
                        </td>            

                        <td>
                             <asp:LinkButton ID="lbSTTS" ValidationGroup="Input" runat="server" Text="Not Yet Uploaded"/>
                            <asp:Button ID="btnsaveSTTS" CssClass="bitbtndt btnadd" runat="server" Text="View" Style="display: none" /> 
                        </td>
                    </tr>

                    <tr>
                        <td>
                        <asp:checkbox id="cbTTD" runat="server" text=" 2 buah Kuitansi Kosong bermaterai 6,000 dan ber TTD Penjual" autopostback="False"/>
                        </td>
                        
                        <td> 
                        <asp:FileUpload style="color: White;" runat="server" accept="application/pdf" ID="FubTTD"  />
                       <!--  <asp:RegularExpressionValidator ID="FileUpLoadValidator5" runat="server" ErrorMessage="Upload PDF files only .!!!"
                            ValidationExpression="^(([a-zA-Z]:)|(\\{2}\w+)\$?)(\\(\w[\w].*))(.pdf|.PDF)$" 
                           ControlToValidate="FubTTD"> 
                        </asp:RegularExpressionValidator>  -->                               
                        <asp:Label ID="lblmassageTTD" CssClass="labelMassage" runat="server"  Visible = "False" Text="File uploaded successfully"></asp:Label>   
                        </td>            

                        <td>
                            <asp:LinkButton ID="lbTTD" ValidationGroup="Input" runat="server" Text="Not Yet Uploaded"/>
                            <asp:Button ID="btnsaveTTD" CssClass="bitbtndt btnadd" runat="server" Text="View" Style="display: none" /> 
                        </td>
                    </tr>


                    <tr>
                        <td>
                        <asp:checkbox id="cbAJB" runat="server" text=" Asli AJB /SPH/ SHM" autopostback="False"/>
                        </td>
                        
                        <td> 
                        <asp:FileUpload style="color: White;" runat="server" accept="application/pdf" ID="FubAJB"  />
                        <!-- <asp:RegularExpressionValidator ID="FileUpLoadValidator6" runat="server" ErrorMessage="Upload PDF files only .!!!"
                            ValidationExpression="^(([a-zA-Z]:)|(\\{2}\w+)\$?)(\\(\w[\w].*))(.pdf|.PDF)$" 
                           ControlToValidate="FubAJB"> 
                        </asp:RegularExpressionValidator> -->                                
                        <asp:Label ID="lblmassageAJB" CssClass="labelMassage" runat="server"  Visible = "False" Text="File uploaded successfully"></asp:Label>   
                        </td>            

                        <td>

                            <asp:LinkButton ID="lbAJB" ValidationGroup="Input" runat="server" Text="Not Yet Uploaded"/>
                            <asp:Button ID="btnsaveAJB" CssClass="bitbtndt btnadd" runat="server" Style="display: none" Text="View" /> 
                        </td>
                    </tr>

                    <tr>
                        <td>
                        <asp:checkbox id="cbAJB2" runat="server" text=" Asli AJB 2 (kalau nama penjual beda dengan nama dalam girik)" autopostback="False"/>
                        </td>
                        
                        <td> 
                        <asp:FileUpload style="color: White;" runat="server" accept="application/pdf" ID="FubAJB2"  />
                        <!-- <asp:RegularExpressionValidator ID="FileUpLoadValidator7" runat="server" ErrorMessage="Upload PDF files only .!!!"
                            ValidationExpression="^(([a-zA-Z]:)|(\\{2}\w+)\$?)(\\(\w[\w].*))(.pdf|.PDF)$" 
                           ControlToValidate="FubAJB2"> 
                        </asp:RegularExpressionValidator>  -->                               
                        <asp:Label ID="lblmassageAJB2" CssClass="labelMassage" runat="server"  Visible = "False" Text="File uploaded successfully"></asp:Label>  
                        </td>            

                        <td>
                            <asp:LinkButton ID="lbAJB2" ValidationGroup="Input" runat="server" Text="Not Yet Uploaded"/>
                            <asp:Button ID="btnsaveAJB2" CssClass="bitbtndt btnadd" runat="server" Text="View" Style="display: none" /> 
                        </td>
                    </tr>

                    <tr>
                        <td>
                        <asp:checkbox id="cbSSP" runat="server" text=" Bukti Bayar & SSP Validasi (Pajak Penjual)" autopostback="False"/>
                        </td>
                        
                        <td> 
                        <asp:FileUpload style="color: White;" runat="server" accept="application/pdf" ID="FubSSP"  />
                        <!-- <asp:RegularExpressionValidator ID="FileUpLoadValidator8" runat="server" ErrorMessage="Upload PDF files only .!!!"
                            ValidationExpression="^(([a-zA-Z]:)|(\\{2}\w+)\$?)(\\(\w[\w].*))(.pdf|.PDF)$" 
                           ControlToValidate="FubSSP"> 
                        </asp:RegularExpressionValidator>  -->                               
                        <asp:Label ID="lblmassageSSP" CssClass="labelMassage" runat="server"  Visible = "False" Text="File uploaded successfully"></asp:Label>
                        </td>            

                        <td>
                             <asp:LinkButton ID="lbSSP" ValidationGroup="Input" runat="server" Text="Not Yet Uploaded"/>
                            <asp:Button ID="btnsaveSSP" CssClass="bitbtndt btnadd" runat="server" Text="View" Style="display: none" /> 
                        </td>
                    </tr>

                    <tr>
                        <td>
                        <asp:checkbox id="cbSSD" runat="server" text=" Bukti Bayar & SSPD-BPHTB lembar 3 (Pejak Pembeli)" autopostback="False"/>
                        </td>
                        
                        <td> 
                        <asp:FileUpload style="color: White;" runat="server" accept="application/pdf" ID="FubSSD"  />
                        <!-- <asp:RegularExpressionValidator ID="FileUpLoadValidator9" runat="server" ErrorMessage="Upload PDF files only .!!!"
                            ValidationExpression="^(([a-zA-Z]:)|(\\{2}\w+)\$?)(\\(\w[\w].*))(.pdf|.PDF)$" 
                           ControlToValidate="FubSSD"> 
                        </asp:RegularExpressionValidator> -->                                
                        <asp:Label ID="lblmassageSSD" CssClass="labelMassage" runat="server"  Visible = "False" Text="File uploaded successfully"></asp:Label>
                        </td>            

                        <td>
                             <asp:LinkButton ID="lbSSD" ValidationGroup="Input" runat="server" Text="Not Yet Uploaded"/>
                            <asp:Button ID="btnsaveSSD" CssClass="bitbtndt btnadd" runat="server" Text="View" Style="display: none" /> 
                        </td>
                    </tr>
                    <!-- 10 -->
                    <tr>
                        <td>
                        <asp:checkbox id="cbSKTS" runat="server" text=" Surat Keterangan Tidak Sengketa" autopostback="False"/>
                        </td>
                        
                        <td> 
                        <asp:FileUpload style="color: White;" runat="server" accept="application/pdf" ID="FubSKTS"  />
                        <!-- <asp:RegularExpressionValidator ID="FileUpLoadValidator10" runat="server" ErrorMessage="Upload PDF files only .!!!"
                            ValidationExpression="^(([a-zA-Z]:)|(\\{2}\w+)\$?)(\\(\w[\w].*))(.pdf|.PDF)$" 
                           ControlToValidate="FubSKTS"> 
                        </asp:RegularExpressionValidator> -->                                
                        <asp:Label ID="lblmassageSKTS" CssClass="labelMassage" runat="server"  Visible = "False" Text="File uploaded successfully"></asp:Label>
                        </td>            

                        <td>
                             <asp:LinkButton ID="lbSKTS" ValidationGroup="Input" runat="server" Text="Not Yet Uploaded"/>
                            <asp:Button ID="btnsaveSKTS" CssClass="bitbtndt btnadd" runat="server" Text="View" Style="display: none" /> 
                        </td>
                    </tr>



                    <tr>
                        <td>
                        <asp:checkbox id="cbSPBT" runat="server" text=" Surat Pernyataan Batas Tanah, Belum Bersetifikat dan Terima Luas" autopostback="False"/>
                        </td>
                        
                        <td> 
                        <asp:FileUpload style="color: White;" runat="server" accept="application/pdf" ID="FubSPBT"  />
                        <!-- <asp:RegularExpressionValidator ID="FileUpLoadValidator11" runat="server" ErrorMessage="Upload PDF files only .!!!"
                            ValidationExpression="^(([a-zA-Z]:)|(\\{2}\w+)\$?)(\\(\w[\w].*))(.pdf|.PDF)$" 
                           ControlToValidate="FubSPBT"> 
                        </asp:RegularExpressionValidator>  -->                               
                        <asp:Label ID="lblmassageSPBT" CssClass="labelMassage" runat="server"  Visible = "False" Text="File uploaded successfully"></asp:Label>
                        </td>            

                        <td>
                             <asp:LinkButton ID="lbSPBT" ValidationGroup="Input" runat="server" Text="Not Yet Uploaded"/>
                            <asp:Button ID="btnsaveSPBT" CssClass="bitbtndt btnadd" runat="server" Text="View" Style="display: none" /> 
                        </td>
                    </tr>


                     <tr>
                        <td>
                        <asp:checkbox id="cbSPKTT" runat="server" text=" Surat Pernyataan Kebenaran Tanda Tangan / Cap Jempol" autopostback="False"/>
                        </td>
                        
                        <td> 
                        <asp:FileUpload style="color: White;" runat="server" accept="application/pdf" ID="FubSPKTT"  />
                        <!-- <asp:RegularExpressionValidator ID="FileUpLoadValidator12" runat="server" ErrorMessage="Upload PDF files only .!!!"
                            ValidationExpression="^(([a-zA-Z]:)|(\\{2}\w+)\$?)(\\(\w[\w].*))(.pdf|.PDF)$" 
                           ControlToValidate="FubSPKTT"> 
                        </asp:RegularExpressionValidator>                                
                         -->
                          <asp:Label ID="lblmassageSPKTT" CssClass="labelMassage" runat="server"  Visible = "False" Text="File uploaded successfully"></asp:Label>
                        </td>            

                        <td>
                             <asp:LinkButton ID="lbSPKTT" ValidationGroup="Input" runat="server" Text="Not Yet Uploaded"/>
                            <asp:Button ID="btnsaveSPKTT" CssClass="bitbtndt btnadd" runat="server" Text="View" Style="display: none" /> 
                        </td>
                    </tr>

                    <tr>
                        <td>
                        <asp:checkbox id="cbBAM" runat="server" text=" Berita Acara Menghadap" autopostback="False"/>
                        </td>
                        
                        <td> 
                        <asp:FileUpload style="color: White;" runat="server" accept="application/pdf" ID="FubBAM"  />
                        <!-- <asp:RegularExpressionValidator ID="FileUpLoadValidator13" runat="server" ErrorMessage="Upload PDF files only .!!!"
                            ValidationExpression="^(([a-zA-Z]:)|(\\{2}\w+)\$?)(\\(\w[\w].*))(.pdf|.PDF)$" 
                           ControlToValidate="FubBAM"> 
                        </asp:RegularExpressionValidator> --> 
                         <asp:Label ID="lblmassageBAM" CssClass="labelMassage" runat="server"  Visible = "False" Text="File uploaded successfully"></asp:Label>                               
                            
                        </td>            

                        <td>
                             <asp:LinkButton ID="lbBAM" ValidationGroup="Input" runat="server" Text="Not Yet Uploaded"/>
                            <asp:Button ID="btnsaveBAM" CssClass="bitbtndt btnadd" runat="server" Text="View" Style="display: none" /> 
                        </td>
                    </tr>

                    <tr>
                        <td>
                        <asp:checkbox id="cbBAPL" runat="server" text=" Berita Acara Pemeriksaan Lokasi dari Desa" autopostback="False"/>
                        </td>
                        
                        <td> 
                        <asp:FileUpload style="color: White;" runat="server" accept="application/pdf" ID="FubBAPL"  />
                        <!-- <asp:RegularExpressionValidator ID="FileUpLoadValidator14" runat="server" ErrorMessage="Upload PDF files only .!!!"
                            ValidationExpression="^(([a-zA-Z]:)|(\\{2}\w+)\$?)(\\(\w[\w].*))(.pdf|.PDF)$" 
                           ControlToValidate="FubBAPL"> 
                        </asp:RegularExpressionValidator>  -->                               
                            <asp:Label ID="lblmassageBAPL" CssClass="labelMassage" runat="server"  Visible = "False" Text="File uploaded successfully"></asp:Label> 
                        </td>            

                        <td>
                            <asp:LinkButton ID="lbBAPL" ValidationGroup="Input" runat="server" Text="Not Yet Uploaded"/>
                            <asp:Button ID="btnsaveBAPL" CssClass="bitbtndt btnadd" runat="server" Text="View" Style="display: none" /> 
                        </td>
                    </tr>

                    
                     <tr>
                        <td>
                        <asp:checkbox id="cbPTPP" runat="server" text=" Denah / Peta Tanah Pembeli dari Pihak IGL (Pak Hanif)" autopostback="False"/>
                        </td>
                        
                        <td> 
                        <asp:FileUpload style="color: White;" runat="server" accept="application/pdf" ID="FubPTPP"  />
                        <!-- <asp:RegularExpressionValidator ID="FileUpLoadValidator15" runat="server" ErrorMessage="Upload PDF files only .!!!"
                            ValidationExpression="^(([a-zA-Z]:)|(\\{2}\w+)\$?)(\\(\w[\w].*))(.pdf|.PDF)$" 
                           ControlToValidate="FubPTPP"> 
                        </asp:RegularExpressionValidator>  -->                               
                            <asp:Label ID="lblmassagePTPP" CssClass="labelMassage" runat="server"  Visible = "False" Text="File uploaded successfully"></asp:Label>
                        </td>            

                        <td>
                            <asp:LinkButton ID="lbPTPP" ValidationGroup="Input" runat="server" Text="Not Yet Uploaded"/>
                            <asp:Button ID="btnsavePTPP" CssClass="bitbtndt btnadd" runat="server" Text="View" Style="display: none" /> 
                        </td>
                    </tr>

                </table>
             </asp:View>

            

            <asp:View ID="TabW" runat="server">
                <table>

                    <tr>
                        <td>
                        <asp:checkbox id="cbPTPP2" runat="server" text=" Denah / Peta Tanah Pembeli dari Pihak IGL terhadap Ijin Kawasan / Ijin Lokasi (Pak Hanif)" autopostback="False"/>
                        </td>
                        
                        <td> 
                        <asp:FileUpload style="color: White;" runat="server" accept="application/pdf" ID="FubPTPP2"  />
                        <!-- <asp:RegularExpressionValidator ID="FileUpLoadValidator16" runat="server" ErrorMessage="Upload PDF files only .!!!"
                            ValidationExpression="^(([a-zA-Z]:)|(\\{2}\w+)\$?)(\\(\w[\w].*))(.pdf|.PDF)$" 
                           ControlToValidate="FubPTPP2"> 
                        </asp:RegularExpressionValidator>  -->                               
                            <asp:Label ID="lblmassagePTPP2" CssClass="labelMassage" runat="server"  Visible = "False" Text="File uploaded successfully"></asp:Label>
                        </td>            

                        <td>
                            <asp:LinkButton ID="lbPtPP2" ValidationGroup="Input" runat="server" Text="Not Yet Uploaded"/> 
                            <asp:Button ID="btnsavePTPP2" CssClass="bitbtndt btnadd" runat="server" Text="View" Style="display: none" /> 
                        </td>
                    </tr>


                    <tr>
                        <td>
                        <asp:checkbox id="cbSKRT" runat="server" text=" Surat Keterangan Riwayat Tanah dari Desa" autopostback="False"/>
                        </td>
                       
                        <td>  
                        <asp:FileUpload style="color: White;" runat="server" accept="application/pdf" ID="FubSKRT" />
                        <!-- <asp:RegularExpressionValidator ID="FileUpLoadValidator17" runat="server" ErrorMessage="Upload PDF files only .!!!"
                            ValidationExpression="^(([a-zA-Z]:)|(\\{2}\w+)\$?)(\\(\w[\w].*))(.pdf|.PDF)$" 
                           ControlToValidate="FubSKRT"> 
                        </asp:RegularExpressionValidator>  -->                               
                            <asp:Label ID="lblmassageSKRT" CssClass="labelMassage" runat="server"  Visible = "False" Text="File uploaded successfully"></asp:Label>
                        </td>            

                        <td>
                            <asp:LinkButton ID="lbSKRT" ValidationGroup="Input" runat="server" Text="Not Yet Uploaded"/> 
                            <asp:Button ID="btnsaveSKRT" CssClass="bitbtndt btnadd" runat="server" Text="View" Style="display: none" /> 
                        </td>
                    </tr>


                     <tr>
                        <td>
                        <asp:checkbox id="cbSKD" runat="server" text=" Surat Keterangan Desa / Tanah" autopostback="False"/>
                        </td>
                        
                        <td> 
                        <asp:FileUpload style="color: White;" runat="server" accept="application/pdf" ID="FubSKD"  />
                        <!-- <asp:RegularExpressionValidator ID="FileUpLoadValidator18" runat="server" ErrorMessage="Upload PDF files only .!!!"
                            ValidationExpression="^(([a-zA-Z]:)|(\\{2}\w+)\$?)(\\(\w[\w].*))(.pdf|.PDF)$" 
                           ControlToValidate="FubSKD"> 
                        </asp:RegularExpressionValidator> -->                                
                            <asp:Label ID="lblmassageSKD" CssClass="labelMassage" runat="server"  Visible = "False" Text="File uploaded successfully"></asp:Label>
                        </td>            

                        <td>
                            <asp:LinkButton ID="lbSKD" ValidationGroup="Input" runat="server" Text="Not Yet Uploaded"/>
                            <asp:Button ID="btnsaveSKD" CssClass="bitbtndt btnadd" runat="server" Text="View" Style="display: none" /> 
                        </td>
                    </tr>



                    <tr>
                        <td>
                        <asp:checkbox id="cbFCGirik" runat="server" text=" Fc. Girik/Buku C Desa yang dilegalisir Kelurahan" autopostback="False"/>
                        </td>
                        
                        <td> 
                        <asp:FileUpload style="color: White;" runat="server" accept="application/pdf" ID="FubFCGirik"  />
                       <!--  <asp:RegularExpressionValidator ID="FileUpLoadValidator19" runat="server" ErrorMessage="Upload PDF files only .!!!"
                            ValidationExpression="^(([a-zA-Z]:)|(\\{2}\w+)\$?)(\\(\w[\w].*))(.pdf|.PDF)$" 
                           ControlToValidate="FubFCGirik"> 
                        </asp:RegularExpressionValidator> -->                                
                            <asp:Label ID="lblmassageFcGirik" CssClass="labelMassage" runat="server"  Visible = "False" Text="File uploaded successfully"></asp:Label>
                        </td>            

                        <td>
                            <asp:LinkButton ID="lbFCGirik" ValidationGroup="Input" runat="server" Text="Not Yet Uploaded"/>
                            <asp:Button ID="btnsaveFCGirik" CssClass="bitbtndt btnadd" runat="server" Text="View" Style="display: none" /> 
                        </td>
                    </tr>


                    <tr>
                        <td>
                        <asp:checkbox id="cbFDP" runat="server" text=" Foto Dokumentasi Penjual  (Pelunasan)" autopostback="False"/>
                        </td>
                        
                        <td> 
                        <asp:FileUpload style="color: White;" runat="server" accept="application/pdf" ID="FubFDP"  />
                       <!--  <asp:RegularExpressionValidator ID="FileUpLoadValidator20" runat="server" ErrorMessage="Upload PDF files only .!!!"
                            ValidationExpression="^(([a-zA-Z]:)|(\\{2}\w+)\$?)(\\(\w[\w].*))(.pdf|.PDF)$" 
                           ControlToValidate="FubFDP"> 
                        </asp:RegularExpressionValidator>   -->                              
                            <asp:Label ID="lblmassageFDP" CssClass="labelMassage" runat="server"  Visible = "False" Text="File uploaded uccessfully"></asp:Label>
                        </td>            

                        <td>
                            <asp:LinkButton ID="lbFDP" ValidationGroup="Input" runat="server" Text="Not Yet Uploaded"/>
                            <asp:Button ID="btnsaveFDP" CssClass="bitbtndt btnadd" runat="server" Text="View" Style="display: none"  /> 
                        </td>
                    </tr>


                    <tr>
                        <td>
                        <asp:checkbox id="cbPatok" runat="server" text=" Foto Batas Tanah (Patok)" autopostback="False"/>
                        </td>
                        
                        <td> 
                        <asp:FileUpload style="color: White;" runat="server" accept="application/pdf" ID="FubPatok"  />
                        <!-- <asp:RegularExpressionValidator ID="FileUpLoadValidator21" runat="server" ErrorMessage="Upload PDF files only .!!!"
                            ValidationExpression="^(([a-zA-Z]:)|(\\{2}\w+)\$?)(\\(\w[\w].*))(.pdf|.PDF)$" 
                           ControlToValidate="FubPatok"> 
                        </asp:RegularExpressionValidator> -->                                
                            <asp:Label ID="lblmassagePatok" CssClass="labelMassage" runat="server"  Visible = "False" Text="File uploaded uccessfully"></asp:Label>
                        </td>            

                        <td>
                            <asp:LinkButton ID="lbPatok" ValidationGroup="Input" runat="server" Text="Not Yet Uploaded"/>
                            <asp:Button ID="btnsavePatok" CssClass="bitbtndt btnadd" runat="server" Text="View"  Style="display: none"  /> 
                        </td>
                    </tr>

                     <tr>
                        <td>
                        <asp:checkbox id="cbSporadik" runat="server" text=" Surat Penguasaan Fisik Bidang Tanah (Sporadik)" autopostback="False"/>
                        </td>
                        
                        <td> 
                        <asp:FileUpload style="color: White;" runat="server" accept="application/pdf" ID="FubSporadik"  />
                        <!-- <asp:RegularExpressionValidator ID="FileUpLoadValidator22" runat="server" ErrorMessage="Upload PDF files only .!!!"
                            ValidationExpression="^(([a-zA-Z]:)|(\\{2}\w+)\$?)(\\(\w[\w].*))(.pdf|.PDF)$" 
                           ControlToValidate="FubSporadik"> 
                        </asp:RegularExpressionValidator>  -->                               
                             <asp:Label ID="lblmassageSporadik" CssClass="labelMassage" runat="server"  Visible = "False" Text="File uploaded uccessfully"></asp:Label>
                        </td>            

                        <td>
                            <asp:LinkButton ID="lbSporadik" ValidationGroup="Input" runat="server" Text="Not Yet Uploaded"/>
                            <asp:Button ID="btnsaveSporadik" CssClass="bitbtndt btnadd" runat="server" Text="View" Style="display: none" /> 
                        </td>
                    </tr>


                    <tr>
                        <td>
                        <asp:checkbox id="cbAHU" runat="server" text=" Asli Hasil Ukur" autopostback="False"/>
                        </td>
                        
                        <td> 
                        <asp:FileUpload style="color: White;" runat="server" accept="application/pdf" ID="FubAHU"  />
                        <!-- <asp:RegularExpressionValidator ID="FileUpLoadValidator23" runat="server" ErrorMessage="Upload PDF files only .!!!"
                            ValidationExpression="^(([a-zA-Z]:)|(\\{2}\w+)\$?)(\\(\w[\w].*))(.pdf|.PDF)$" 
                           ControlToValidate="FubAHU"> 
                        </asp:RegularExpressionValidator>  -->                               
                            <asp:Label ID="lblmassageAHU" CssClass="labelMassage" runat="server"  Visible = "False" Text="File uploaded uccessfully"></asp:Label>
                        </td>            

                        <td>
                            <asp:LinkButton ID="lbAHU" ValidationGroup="Input" runat="server" Text="Not Yet Uploaded"/>
                            <asp:Button ID="btnsaveAHU" CssClass="bitbtndt btnadd" runat="server" Text="View" Style="display: none" /> 
                        </td>
                    </tr>


                    <tr>
                        <td>
                        <asp:checkbox id="cbSejarah" runat="server" text=" Sejarah / Riwayat Tanah" autopostback="False"/>
                        </td>
                        
                        <td> 
                        <asp:FileUpload style="color: White;" runat="server" accept="application/pdf" ID="FubSejarah"  />
                        <!-- <asp:RegularExpressionValidator ID="FileUpLoadValidator24" runat="server" ErrorMessage="Upload PDF files only .!!!"
                            ValidationExpression="^(([a-zA-Z]:)|(\\{2}\w+)\$?)(\\(\w[\w].*))(.pdf|.PDF)$" 
                           ControlToValidate="FubSejarah"> 
                        </asp:RegularExpressionValidator>  -->                               
                            <asp:Label ID="lblmassageSejarah" CssClass="labelMassage" runat="server"  Visible = "False" Text="File uploaded uccessfully"></asp:Label>
                        </td>            

                        <td>
                            <asp:LinkButton ID="lbSejarah" ValidationGroup="Input" runat="server" Text="Not Yet Uploaded"/>
                            <asp:Button ID="btnsaveSejarah" CssClass="bitbtndt btnadd" runat="server" Text="View" Style="display: none" /> 
                        </td>
                    </tr>


                    <tr>
                        <td>
                        <asp:checkbox id="cbSPJH" runat="server" text=" Surat Pernyataan Jual Hasi/ Seluruhnya" autopostback="False"/>
                        </td>
                        
                        <td> 
                        <asp:FileUpload style="color: White;" runat="server" accept="application/pdf" ID="FubSPJH"  />
                        <!-- <asp:RegularExpressionValidator ID="FileUpLoadValidator25" runat="server" ErrorMessage="Upload PDF files only .!!!"
                            ValidationExpression="^(([a-zA-Z]:)|(\\{2}\w+)\$?)(\\(\w[\w].*))(.pdf|.PDF)$" 
                           ControlToValidate="FubSPJH"> 
                        </asp:RegularExpressionValidator> -->                                
                            <asp:Label ID="lblmassageSPJH" CssClass="labelMassage" runat="server"  Visible = "False" Text="File uploaded uccessfully"></asp:Label>
                        </td>            

                        <td>
                            <asp:LinkButton ID="lbSPJH" ValidationGroup="Input" runat="server" Text="Not Yet Uploaded"/>
                            <asp:Button ID="btnsaveSPJH" CssClass="bitbtndt btnadd" runat="server" Text="View" Style="display: none"/> 
                        </td>
                    </tr>


                    <tr>
                        <td>
                        <asp:checkbox id="cbLainLain" runat="server" autopostback="False"/>
                        <asp:TextBox runat="server" ValidationGroup="Input" ID="tbLainLain" CssClass="TextBox" Height="20px" Width="225px" AutoPostBack = "True"/>
                        </td>
                         
                        
                        <td> 
                        <asp:FileUpload style="color: White;" runat="server" accept="application/pdf" ID="FubLainLain"  />                             
                            <asp:Label ID="lblmassageLainLain" CssClass="labelMassage" runat="server"  Visible = "False" Text="File uploaded uccessfully"></asp:Label>
                        </td>            

                        <td>
                            <asp:LinkButton ID="lbLainLain" ValidationGroup="Input" runat="server" Text="Not Yet Uploaded"/>
                            <asp:Button ID="btnsaveLainLain" CssClass="bitbtndt btnadd" runat="server" Text="View" Style="display: none" /> 
                        </td>
                    </tr>

                    <tr>
                        <td>
                        <asp:checkbox id="cbLainLain2" runat="server" autopostback="False"/>
                        <asp:TextBox runat="server" ValidationGroup="Input" ID="tbLainLain2" CssClass="TextBox" Height="20px" Width="225px" AutoPostBack = "True"/>
                        </td>
                         
                        
                        <td> 
                        <asp:FileUpload style="color: White;" runat="server" accept="application/pdf" ID="FubLainLain2"  />                       
                            <asp:Label ID="lblmassageLainLain2" CssClass="labelMassage" runat="server"  Visible = "False" Text="File uploaded uccessfully"></asp:Label>
                        </td>            

                        <td>
                            <asp:LinkButton ID="lbLainLain2" ValidationGroup="Input" runat="server" Text="Not Yet Uploaded"/>
                            <asp:Button ID="btnsaveLainLain2" CssClass="bitbtndt btnadd" runat="server" Text="View" Style="display: none" /> 
                        </td>
                    </tr>

                    <tr>
                        <td>
                        <asp:checkbox id="cbLainLain3" runat="server" autopostback="False"/>
                        <asp:TextBox runat="server" ValidationGroup="Input" ID="tbLainLain3" CssClass="TextBox" Height="20px" AutoPostBack = "True" Width="225px"/>
                        </td>
                         
                        
                        <td> 
                        <asp:FileUpload style="color: White;" runat="server" accept="application/pdf" ID="FubLainLain3"  />                  
                            <asp:Label ID="lblmassageLainLain3" CssClass="labelMassage" runat="server"  Visible = "False" Text="File uploaded uccessfully"></asp:Label>
                        </td>            

                        <td>
                            <asp:LinkButton ID="lbLainLain3" ValidationGroup="Input" runat="server" Text="Not Yet Uploaded"/>
                            <asp:Button ID="btnsaveLainLain3" CssClass="bitbtndt btnadd" runat="server" Text="View" Style="display: none" /> 
                        </td>
                    </tr>

                    <tr>
                        <td>
                            <h4>Jika Dokumen Tanah Waris</h4>
                        </td>
                    </tr>
                    

                    <tr>
                        <td>
                        <asp:checkbox id="cbKtpW" Enabled="True" runat="server" text=" KTP Waris" autopostback="False"/>
                        </td>
                        
                        <td> 
                        <asp:FileUpload style="color: White;" runat="server" accept="application/pdf" ID="FupKTPW"  />
                         <!-- <asp:RegularExpressionValidator ID="FileUpLoadValidator27" runat="server" ErrorMessage="Upload PDF files only .!!!"
                            ValidationExpression="^(([a-zA-Z]:)|(\\{2}\w+)\$?)(\\(\w[\w].*))(.pdf|.PDF)$" 
                           ControlToValidate="FupKTPW"> 
                        </asp:RegularExpressionValidator> -->                              
                        <asp:Label ID="lblmassageKTPW" CssClass="labelMassage" runat="server"  Visible = "False" Text="File uploaded uccessfully"></asp:Label>
                        </td>            

                        <td>        
                            <asp:LinkButton ID="lbKtpW" ValidationGroup="Input" runat="server" Text="Not Yet Uploaded"/>
                            <asp:Button ID="btnsaveKtpW" CssClass="bitbtndt btnadd" runat="server"  Style="display: none" Text="View" /> 
                        </td>
                        
                    </tr>

                    <tr>
                        <td>
                        <asp:checkbox id="cbSPW" runat="server" text=" Surat Pernyataan Waris" autopostback="False"/>
                        </td>
                        
                        <td> 
                        <asp:FileUpload style="color: White;" runat="server" accept="application/pdf" ID="FubSPW"  />
                        <!-- <asp:RegularExpressionValidator ID="FileUpLoadValidator28" runat="server" ErrorMessage="Upload PDF files only .!!!"
                            ValidationExpression="^(([a-zA-Z]:)|(\\{2}\w+)\$?)(\\(\w[\w].*))(.pdf|.PDF)$" 
                           ControlToValidate="FubSPW"> 
                        </asp:RegularExpressionValidator> -->                                
                        <asp:Label ID="lblmassageSPW" CssClass="labelMassage" runat="server"  Visible = "False" Text="File uploaded uccessfully"></asp:Label>
                        </td>            

                        <td>
                            <asp:LinkButton ID="lbSPW" ValidationGroup="Input" runat="server" Text="Not Yet Uploaded"/>
                            <asp:Button ID="btnsaveSPW" CssClass="bitbtndt btnadd" runat="server" Text="View" Style="display: none" /> 
                        </td>
                    </tr>

                    <tr>
                        <td>
                        <asp:checkbox id="cbSPKW" runat="server" text=" Surat Pernyataan Kuasa Waris" autopostback="False"/>
                        </td>
                        
                        <td> 
                        <asp:FileUpload style="color: White;" runat="server" accept="application/pdf" ID="FubSPKW"  />
                        <!-- <asp:RegularExpressionValidator ID="FileUpLoadValidator29" runat="server" ErrorMessage="Upload PDF files only .!!!"
                            ValidationExpression="^(([a-zA-Z]:)|(\\{2}\w+)\$?)(\\(\w[\w].*))(.pdf|.PDF)$" 
                           ControlToValidate="FubSPKW"> 
                        </asp:RegularExpressionValidator>  -->                               
                        <asp:Label ID="lblmassageSPKW" CssClass="labelMassage" runat="server"  Visible = "False" Text="File uploaded uccessfully"></asp:Label>
                        </td>            

                        <td>
                            <asp:LinkButton ID="lbSPKW" ValidationGroup="Input" runat="server" Text="Not Yet Uploaded"/>
                            <asp:Button ID="btnsaveSPKW" CssClass="bitbtndt btnadd" runat="server" Text="View" Style="display: none" /> 
                        </td>
                    </tr>


                </table>
             </asp:View>

        </asp:MultiView>

            <br />
            <hr style="color: Blue" />

            <asp:Menu ID="Menu1" runat="server" CssClass="Menu" StaticMenuItemStyle-CssClass="MenuItem"
                StaticSelectedStyle-CssClass="MenuSelect" Orientation="Horizontal" ItemWrap="False"
                StaticEnableDefaultPopOutImage="False">
                <StaticSelectedStyle CssClass="MenuSelect" />
                <StaticMenuItemStyle CssClass="MenuItem" />
                <Items>
                    <asp:MenuItem Text="Riwayat Tanah" Value="0" ></asp:MenuItem>
                    <%--<asp:MenuItem Text="Equipment" Value="1" ></asp:MenuItem>--%>
                    <asp:MenuItem Text="Monitoring Letter C" Value="2" ></asp:MenuItem>
                    <%--<asp:MenuItem Text="Schedule Job Detail" Value="3" ></asp:MenuItem>--%>
                </Items>
            </asp:Menu>
            <br />
             
            <asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex="0">
                
                <asp:View ID="Tab2" runat="server">
                    
                    <asp:Panel ID="pnlDt" runat="server">
                    
                        <asp:Button class="bitbtn btnadd" runat="server" ID="btnAddDt" Text="Add" ValidationGroup="Input" />
                       
                        <div style="border: 0px  solid; width: 100%; height: 100%; overflow: auto;">
                            <asp:GridView ID="GridDt" runat="server" AutoGenerateColumns="False" ShowFooter="False">
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
                                     <asp:BoundField DataField="ItemNo" HeaderStyle-Width="150px" HeaderText="Item No"/>                                   
                                    <asp:BoundField DataField="KetKegiatan" HeaderStyle-Width="150px" HeaderText="Ket Kegiatan" />
                                    <asp:BoundField DataField="NoSurat" HeaderStyle-Width="150px" HeaderText="No Surat" />                            
                                     <asp:BoundField DataField="DateSurat" DataFormatString="{0:dd MMM yyyy}" HtmlEncode="true"
                                            HeaderStyle-Width="80px" SortExpression="DateSurat" 
                                            HeaderText="DateSurat">
                                            <HeaderStyle Width="80px" />
                                        </asp:BoundField>
                                    
                                    <asp:BoundField DataField="Luas" DataFormatString="{0:#,##0.##}"  HeaderStyle-Width="150px" HeaderText="Luas" />
                                    <asp:BoundField DataField="NameAwal" HeaderStyle-Width="150px" HeaderText="Atas Nama Awal" />

                                    <asp:BoundField DataField="NameAkhir" HeaderStyle-Width="150px" HeaderText="Atas Nama Akhir" />
                                    <asp:BoundField DataField="Remark" HeaderStyle-Width="150px" HeaderText="Remark" />
                                    
                                </Columns>

                            </asp:GridView>
                        </div>
                        <asp:Button class="bitbtn btnadd" runat="server" ID="btnAddDtKe2" Text="Add" ValidationGroup="Input" />                      
                    </asp:Panel>

                    <asp:Panel runat="server" ID="pnlEditDt" Visible="false">
                        <table>
                            <tr>
                                <td>
                                   Item No</td>
                                <td>:</td>
                                <td colspan="4"> <asp:Label ID="lbItemNo" runat="server" Text="" /> </td>
                            </tr>

                             <tr>
                                <td>Ket Kegiatan</td>
                                <td>:</td>
                                <td><asp:TextBox runat="server" ValidationGroup="Input" ID="tbKetKegiatan" CssClass="TextBox" Width="225px"/></td>
                            </tr> 

                            <tr>
                                <td>No Surat</td>
                                <td>:</td>
                                <td><asp:TextBox runat="server" ValidationGroup="Input" MaxLength="30" ID="tbNoSurat" CssClass="TextBox" Width="225px"/></td>
                            </tr>

                             <tr>
                                <td>Tgl Surat</td>
                                <td>:</td>
                                <td>            
                                    <BDP:BasicDatePicker ID="tbDateSurat" runat="server" DateFormat="dd MMM yyyy" 
                                    ReadOnly = "False" ValidationGroup="Input" Width="225px"
                                    ButtonImageHeight="19px" ButtonImageWidth="20px" 
                                    DisplayType="TextBoxAndImage" 
                                    TextBoxStyle-CssClass="TextDate" AutoPostBack="false" 
                                    ShowNoneButton="False"><TextBoxStyle CssClass="TextDate" /></BDP:BasicDatePicker>            
                                </td>
                            </tr>

                            <tr>
                                <td>Luas</td>
                                <td>:</td>
                                <td><asp:TextBox runat="server" ValidationGroup="Input" ID="tbLuasDt" CssClass="TextBox" Width="225px"/></td>
                            </tr>

                            <tr>
                                <td>Nama Pemilik</td>
                                <td>:</td>
                                <td colspan="7">
                                    <table>
                                        <tr style="background-color:Silver;text-align:center; border-radius:30px;">
                                            <td>Awal</td>
                                            <td>Akhir</td>
                                        </tr>
                                        <tr>
                                            <td><asp:TextBox runat="server" ValidationGroup="Input" ID="tbPemilikAwal" CssClass="TextBox" Width="225px"/></td>
                                             <td><asp:TextBox runat="server" ValidationGroup="Input" ID="tbPemilikAkhir" CssClass="TextBox" Width="225px"/></td>                 
                                        </tr>
                                    </table>
                                </td>                
                            </tr>  

                            <tr>
                                <td>
                                    Remark</td>
                                <td>
                                    :</td>
                                <td colspan="4">
                                    <asp:TextBox ID="tbRemarkDt" runat="server" CssClass="TextBox" MaxLength="255" 
                                        TextMode="MultiLine" Width="365px" />
                                </td>
                            </tr>
                        </table>
                        <br />
                        <asp:Button ID="btnSaveDt" runat="server" class="bitbtndt btnsave" Text="Save" />
                        <asp:Button ID="btnCancelDt" runat="server" class="bitbtndt btncancel" Text="Cancel" />
                    </asp:Panel>
                </asp:View>

                <asp:View ID="Tab3" runat="server">
                        
                    <asp:Panel ID="pnlDt2" runat="server">
                        <asp:Button class="bitbtn btnadd" runat="server" ID="btnAddDt2" Text="Add" ValidationGroup="Input" />
                         
                        <div style="border: 0px  solid; width: 100%; height: 100%; overflow: auto;">
                            <asp:GridView ID="GridDt2" runat="server" AutoGenerateColumns="False" 
                                ShowFooter="True">
                                <HeaderStyle CssClass="GridHeader" />
                                <RowStyle CssClass="GridItem" Wrap="false" />
                                <AlternatingRowStyle CssClass="GridAltItem" />
                                <PagerStyle CssClass="GridPager" />
                                <Columns>
                                    <asp:TemplateField HeaderText="Action"><ItemTemplate><asp:Button ID="btnEdit" runat="server" class="bitbtndt btnedit" 
                                                CommandName="Edit" Text="Edit" /><asp:Button ID="btnDelete" runat="server" class="bitbtndt btndelete" 
                                                CommandName="Delete" 
                                                OnClientClick="return confirm('Sure to delete this data?');" Text="Delete" /></ItemTemplate><EditItemTemplate><asp:Button ID="btnUpdate" runat="server" class="bitbtndt btnsave" 
                                                CommandName="Update" Text="Save" /><asp:Button ID="btnCancel" runat="server" class="bitbtndt btncancel" 
                                                CommandName="Cancel" Text="Cancel" /></EditItemTemplate></asp:TemplateField>
                                    <asp:BoundField DataField="Equipment" HeaderText="Equipment" 
                                        SortExpression="Equipment" ><HeaderStyle Width="150px" /></asp:BoundField>
                                    <asp:BoundField DataField="EquipmentName" HeaderStyle-Width="150px" 
                                        HeaderText="Equipment Name" SortExpression="EquipmentName" ><HeaderStyle Width="200px" /></asp:BoundField>
                                    <asp:BoundField DataField="Qty" HeaderStyle-Width="60px" 
                                        HeaderText="Qty" DataFormatString="{0:#,##0.##}" 
                                        ItemStyle-HorizontalAlign="Right" SortExpression="Qty" ><HeaderStyle Width="60px" /><ItemStyle HorizontalAlign="Right" /></asp:BoundField>
                                    <asp:BoundField DataField="Unit" HeaderStyle-Width="60px" 
                                        HeaderText="Unit"
                                        ItemStyle-HorizontalAlign="Left" SortExpression="Unit" ><HeaderStyle Width="60px" /><ItemStyle HorizontalAlign="Right" /></asp:BoundField>
                                   <asp:BoundField DataField="Remark" HeaderStyle-Width="150px" 
                                        HeaderText="Remark" SortExpression="Remark" ><HeaderStyle Width="200px" /></asp:BoundField>
                                    
                                </Columns>
                            </asp:GridView>
                        </div>
                        <asp:Button ID="btnAddDt2ke2" runat="server" class="bitbtn btnadd" Text="Add" 
                            ValidationGroup="Input" />
                        
                    </asp:Panel>
                    <asp:Panel runat="server" ID="pnlEditDt2" Visible="false">
                        <table>
                            <tr>
                                <td>
                                    Equipment</td>
                                <td>
                                    :
                                </td>
                                <td>
                                    <asp:TextBox ID="tbEquip" runat="server" AutoPostBack="true" CssClass="TextBox" 
                                        MaxLength="20" ValidationGroup="Input" />
                                    <asp:TextBox ID="tbEquipName" runat="server" CssClass="TextBox" 
                                        Enabled="false" MaxLength="60" Width="225px" />
                                    <asp:Button ID="btnEquip" runat="server" Class="btngo" Text="..." 
                                        ValidationGroup="Input" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Qty</td>
                                <td>
                                    :</td>
                                <td>
                                    <asp:TextBox ID="tbQty" runat="server" CssClass="TextBox" Width="65px" />
                                    <asp:Label ID="lblUnit" runat="server" ForeColor="#FF3300" Text="*"></asp:Label>
                                </td>
                                <tr>
                                    <td>
                                        Remark
                                    </td>
                                    <td>
                                        :
                                    </td>
                                    <td>
                                        <asp:TextBox ID="tbremarkEquip" runat="server" CssClass="TextBoxMulti" 
                                            MaxLength="255" TextMode="MultiLine" ValidationGroup="Input" Width="350px" />
                                        &nbsp; &nbsp; &nbsp;
                                    </td>
                                </tr>
                            </tr>
                            </tr>
                        </table>
                        <br />
                        <asp:Button ID="btnSaveDt2" runat="server" class="bitbtndt btnsave" Text="Save" />
                        <asp:Button ID="btnCancelDt2" runat="server" class="bitbtndt btncancel" Text="Cancel" />
                    </asp:Panel>
                </asp:View>

                <asp:View ID="Tab4" runat="server">
               
                    <asp:Panel ID="pnlDt3" runat="server">
                    
                        <asp:Button class="bitbtn btnadd" runat="server" ID="btnAddDt3" Text="Add" ValidationGroup="Input" />
                         
                        <div style="border: 0px  solid; width: 100%; height: 100%; overflow: auto;">
                        
                            <asp:GridView ID="GridDt3" runat="server" AutoGenerateColumns="False" ShowFooter="False">
                                <HeaderStyle CssClass="GridHeader" />
                                <RowStyle CssClass="GridItem" Wrap="false" />
                                <AlternatingRowStyle CssClass="GridAltItem" />
                                <PagerStyle CssClass="GridPager" />
                                <Columns>
                                    <asp:TemplateField HeaderText="Action"><ItemTemplate><asp:Button ID="btnEdit" runat="server" class="bitbtndt btnedit" Text="Edit" CommandName="Edit" /><asp:Button ID="btnDelete" runat="server" class="bitbtndt btndelete" Text="Delete"
                                                OnClientClick="return confirm('Sure to delete this data?');" CommandName="Delete" /></ItemTemplate><EditItemTemplate><asp:Button ID="btnUpdate" runat="server" class="bitbtndt btnsave" Text="Save" CommandName="Update" /><asp:Button ID="btnCancel" runat="server" class="bitbtndt btncancel" Text="Cancel"
                                                CommandName="Cancel" /></EditItemTemplate></asp:TemplateField>
                                                
                                    <asp:TemplateField HeaderText="Detail">
                                        <ItemTemplate>
                                            <asp:Button ID="btnDetailMaterial" runat="server" Class="bitbtndt btndetail" CommandArgument="<%# Container.DataItemIndex %>"
                                                CommandName="DetailMaterial" Text="Detail" Width="70" />
                                        </ItemTemplate>
                                    </asp:TemplateField>   
                                    
                                    <asp:BoundField DataField="NoWl" HeaderStyle-Width="100px" 
                                        HeaderText="No Wl" >
                                        <HeaderStyle Width="100px" />
                                    </asp:BoundField>
                                     <asp:BoundField DataField="NoPercilDt2" HeaderStyle-Width="100px" 
                                        HeaderText="No Percil" >
                                        <HeaderStyle Width="100px" />
                                    </asp:BoundField>
                                    
                                    <asp:BoundField DataField="AwalName" HeaderStyle-Width="150px" 
                                        HeaderText="Nama Awal" >                        
                                        <HeaderStyle Width="150px" />
                                    </asp:BoundField>

                                    <asp:BoundField DataField="LuasAwal" HeaderStyle-Width="80px" 
                                        HeaderText="Luas"  DataFormatString="{0:#,##0.##}">
                                        <HeaderStyle Width="80px" />
                                    </asp:BoundField>
                                    
                                    <asp:BoundField DataField="NoWarisLevel" HeaderStyle-Width="150px" 
                                        HeaderText="No Waris Level" >                        
                                        <HeaderStyle Width="150px" />
                                    </asp:BoundField>

                                    <asp:BoundField DataField="WlLvlName1" HeaderStyle-Width="150px" 
                                        HeaderText="Nama 1" >                        
                                        <HeaderStyle Width="150px" />
                                    </asp:BoundField>

                                    <asp:BoundField DataField="LuasLvlNo1" HeaderStyle-Width="80px" 
                                        HeaderText="Luas 1"  DataFormatString="{0:#,##0.##}">
                                        <HeaderStyle Width="80px" />
                                    </asp:BoundField>  

                                    <asp:BoundField DataField="WlLvlName2" HeaderStyle-Width="150px" 
                                        HeaderText="Nama 2" >                        
                                        <HeaderStyle Width="150px" />
                                    </asp:BoundField>

                                    <asp:BoundField DataField="LuasLvlNo2" HeaderStyle-Width="80px" 
                                        HeaderText="Luas 2"  DataFormatString="{0:#,##0.##}">
                                        <HeaderStyle Width="80px" />
                                    </asp:BoundField>  

                                     <asp:BoundField DataField="WlLvlName3" HeaderStyle-Width="150px" 
                                        HeaderText="Nama 3" >                        
                                        <HeaderStyle Width="150px" />
                                    </asp:BoundField>

                                    <asp:BoundField DataField="LuasLvlNo3" HeaderStyle-Width="80px" 
                                        HeaderText="Luas 3"  DataFormatString="{0:#,##0.##}">
                                        <HeaderStyle Width="80px" />
                                    </asp:BoundField>   


                                    <asp:BoundField DataField="WlLvlName4" HeaderStyle-Width="150px" 
                                        HeaderText="Nama 4" >                        
                                        <HeaderStyle Width="150px" />
                                    </asp:BoundField>

                                    <asp:BoundField DataField="LuasLvlNo4" HeaderStyle-Width="80px" 
                                        HeaderText="Luas 4"  DataFormatString="{0:#,##0.##}">
                                        <HeaderStyle Width="80px" />
                                    </asp:BoundField> 

                                    <asp:BoundField DataField="WlLvlName5" HeaderStyle-Width="150px" 
                                        HeaderText="Nama 5" >                        
                                        <HeaderStyle Width="150px" />
                                    </asp:BoundField>

                                    <asp:BoundField DataField="LuasLvlNo5" HeaderStyle-Width="80px" 
                                        HeaderText="Luas 5"  DataFormatString="{0:#,##0.##}">
                                        <HeaderStyle Width="80px" />
                                    </asp:BoundField>  
 


                                    <asp:BoundField DataField="Remark" HeaderStyle-Width="225px" 
                                        HeaderText="Remark" >
                                        <HeaderStyle Width="100px" />
                                    </asp:BoundField>
                                                                        
                                 
                                </Columns>
                            </asp:GridView>
                        </div>
                        <asp:Button class="bitbtn btnadd" runat="server" ID="btnAddDt3ke2" Text="Add" ValidationGroup="Input" />
                        
                    </asp:Panel>
                    <asp:Panel runat="server" ID="pnlEditDt3" Visible="false">
                    <table>

                                    <tr>
                                        <td>No Wl</td>
                                        <td>:</td>
                                        <td><asp:TextBox runat="server" MaxLength="20" ValidationGroup="Input" ID="tbWlNo" CssClass="TextBox" Width="225px"/></td>
                                    </tr>
                                     <tr>
                                        <td>No Percil</td>
                                        <td>:</td>
                                        <td><asp:TextBox runat="server" MaxLength="20" ValidationGroup="Input" ID="tbPercilNoDt" CssClass="TextBox" Width="225px"/></td>
                                    </tr>
                                    
                                    <tr>
                                        <td>No Waris Level</td>
                                        <td>:</td>
                                        <td><asp:TextBox runat="server" MaxLength="20" ValidationGroup="Input" ID="tbWarisLevelNo" CssClass="TextBox" Width="225px"/></td>
                                    </tr>

                                    <tr>
                                    <td>Pemilik Awal</td>
                                    <td>:</td>
                                    <td colspan="7">
                                        <table>
                                            <tr style="background-color:Silver;text-align:center; border-radius:30px;">
                                                <td>Nama Awal</td>
                                                <td>Luas Awal</td>
                                            </tr>
                                            <tr>
                                                <td><asp:TextBox runat="server" ValidationGroup="Input" ID="tbNameAwal" CssClass="TextBox" Width="225px"/></td>
                                                 <td><asp:TextBox runat="server" ValidationGroup="Input" ID="tbLuasAwal" CssClass="TextBox" Width="225px"/></td>                 
                                            </tr>
                                        </table>
                                    </td>                
                            </tr> 



                            <tr>
                                    <td>Nama &amp; Luas 1</td>
                                    <td>:</td>
                                    <td colspan="7">
                                        <table>
                                            <tr style="background-color:Silver;text-align:center; border-radius:30px;">
                                                <td>Name 1</td>
                                                <td>Luas 1</td>
                                            </tr>
                                            <tr>
                                                <td><asp:TextBox runat="server" ValidationGroup="Input" ID="tbNamelvl1" CssClass="TextBox" Width="225px"/></td>
                                                 <td><asp:TextBox runat="server" ValidationGroup="Input" ID="tbLuaslvl1" CssClass="TextBox" Width="225px"/></td>                 
                                            </tr>
                                        </table>
                                    </td>                
                            </tr> 

                            <tr>
                                    <td>&nbsp;Nama &amp; Luas 2</td>
                                    <td>:</td>
                                    <td colspan="7">
                                        <table>
                                            <tr style="background-color:Silver;text-align:center; border-radius:30px;">
                                                <td>Name 2</td>
                                                <td>Luas 2</td>
                                            </tr>
                                            <tr>
                                                <td><asp:TextBox runat="server" ValidationGroup="Input" ID="tbNamelvl2" CssClass="TextBox" Width="225px"/></td>
                                                 <td><asp:TextBox runat="server" ValidationGroup="Input" ID="tbLuaslvl2" CssClass="TextBox" Width="225px"/></td>                 
                                            </tr>
                                        </table>
                                    </td>                
                            </tr> 

                            <tr>
                                    <td>Nama &amp; Luas 3</td>
                                    <td>:</td>
                                    <td colspan="7">
                                        <table>
                                            <tr style="background-color:Silver;text-align:center; border-radius:30px;">
                                                <td>Name 3</td>
                                                <td>Luas 3</td>
                                            </tr>
                                            <tr>
                                                <td><asp:TextBox runat="server" ValidationGroup="Input" ID="tbNamelvl3" CssClass="TextBox" Width="225px"/></td>
                                                 <td><asp:TextBox runat="server" ValidationGroup="Input" ID="tbLuaslvl3" CssClass="TextBox" Width="225px"/></td>                 
                                            </tr>
                                        </table>
                                    </td>                
                            </tr> 

                            <tr>
                                    <td>Nama &amp; Luas 4</td>
                                    <td>:</td>
                                    <td colspan="7">
                                        <table>
                                            <tr style="background-color:Silver;text-align:center; border-radius:30px;">
                                                <td>Name 1</td>
                                                <td>Luas 1</td>
                                            </tr>
                                            <tr>
                                                <td><asp:TextBox runat="server" ValidationGroup="Input" ID="tbNamelvl4" CssClass="TextBox" Width="225px"/></td>
                                                 <td><asp:TextBox runat="server" ValidationGroup="Input" ID="tbLuaslvl4" CssClass="TextBox" Width="225px"/></td>                 
                                            </tr>
                                        </table>
                                    </td>                
                            </tr> 

                            <tr>
                                    <td>&nbsp;Nama &amp; Luas 5</td>
                                    <td>:</td>
                                    <td colspan="7">
                                        <table>
                                            <tr style="background-color:Silver;text-align:center; border-radius:30px;">
                                                <td>Name 5</td>
                                                <td>Luas 5</td>
                                            </tr>
                                            <tr>
                                                <td><asp:TextBox runat="server" ValidationGroup="Input" ID="tbNamelvl5" CssClass="TextBox" Width="225px"/></td>
                                                 <td><asp:TextBox runat="server" ValidationGroup="Input" ID="tbLuaslvl5" CssClass="TextBox" Width="225px"/></td>                 
                                            </tr>
                                        </table>
                                    </td>                
                            </tr> 

                            <tr>
                                           <td class="style1">
                                               Remark</td>
                                           <td class="style2">
                                               :</td>
                                           <td>
                                               <asp:TextBox ID="tbRemarkdt2" runat="server" CssClass="TextBox" Enabled="True" Height="38px" TextMode="MultiLine" Width="450px" />
                                           </td>
                                       </tr>
                    </table>

                        <br />
                        <asp:Button ID="btnSaveDt3" runat="server" class="bitbtndt btnsave" Text="Save" />
                        <asp:Button ID="btnCancelDt3" runat="server" class="bitbtndt btncancel" Text="Cancel" />
                    </asp:Panel>
                </asp:View>
                
                <asp:View ID="Tab5" runat="server">
                
                 <asp:Panel ID="pnlInfoDt" runat="server">
                    <table>
                            <tr>

                            <td>  <asp:Label ID="lblItem" runat="server" Text="No WL " />  </td>
                             <td>: </td> 
                            <td>  <asp:Label ID="lbNoWl" runat="server" Font-Bold="False" ForeColor="#0092C8"
                            Text="No Wl" />  </td>   
                            </tr> 

                             <tr>
                                    <td>Waris Level No</td>
                                    <td>:</td>
                                    <td colspan="7">
                                        <table>
                                            <tr style="background-color:Silver;text-align:center; border-radius :50px;">
                                                
                                                <td>1</td> 
                                                <td>2</td>
                                                <td>3</td>
                                                <td>4</td>
                                                <td>5</td>
                                            </tr>
                                            <tr>
                                                <td><asp:Label ID="lbwaris1" runat="server" Font-Bold="False" ForeColor="#0092C8" Text="Waris Name 1" /></td> 
                                                <td><asp:Label ID="lbwaris2" runat="server" Font-Bold="False" ForeColor="#0092C8" Text="Waris Name 2" /></td> 
                                                <td><asp:Label ID="lbwaris3" runat="server" Font-Bold="False" ForeColor="#0092C8" Text="Waris Name 3" /></td> 
                                                <td><asp:Label ID="lbwaris4" runat="server" Font-Bold="False" ForeColor="#0092C8" Text="Waris Name 4" /></td> 
                                                <td><asp:Label ID="lbwaris5" runat="server" Font-Bold="False" ForeColor="#0092C8" Text="Waris Name 5" /></td>                 
                                            </tr>

                                            <tr>
                                                <td><asp:TextBox runat="server" ValidationGroup="Input" ID="tbLuas1" Enabled = "False" CssClass="TextBox" Width="120px"/></td>
                                                 <td><asp:TextBox runat="server" ValidationGroup="Input" ID="tbLuas2" Enabled = "False"  CssClass="TextBox" Width="120px"/></td>
                                                 <td><asp:TextBox runat="server" ValidationGroup="Input" ID="tbLuas3" Enabled = "False" CssClass="TextBox" Width="120px"/></td>
                                                 <td><asp:TextBox runat="server" ValidationGroup="Input" ID="tbLuas4" Enabled = "False" CssClass="TextBox" Width="120px"/></td>
                                                 <td><asp:TextBox runat="server" ValidationGroup="Input" ID="tbLuas5" Enabled = "False" CssClass="TextBox" Width="120px"/></td>                 
                                            </tr>
                                        </table>
                                    </td>                
                            </tr>
                    </table>
                        
                                                               
                            
                    </asp:Panel>
                       <br /> 
                    <asp:Panel runat="server" ID="PnlDt4">
                    
                        <asp:Button class="bitbtn btnadd" runat="server" ID="btnAdddt4" Text="Add" Visible="false"
                            ValidationGroup="Input" />
                        
                                
                        &nbsp;
                        <asp:Button ID="btnBackDt" runat="server" class="bitbtndt btnback" Text="Back" 
                            Width="60" />
                            <br />
                            <br/>
                        <div style="border: 0px  solid; width: 100%; height: 100%; overflow: auto;">
                            <asp:GridView ID="GridDt4" runat="server" AutoGenerateColumns="False" 
                                ShowFooter="False">
                                <HeaderStyle CssClass="GridHeader" />
                                <RowStyle CssClass="GridItem" Wrap="false" />
                                <AlternatingRowStyle CssClass="GridAltItem" />
                                <PagerStyle CssClass="GridPager" />
                                <Columns>
                                    <asp:TemplateField HeaderText="Action">
                                        <ItemTemplate>
                                            <asp:Button ID="btnEdit" runat="server" class="bitbtndt btnedit" 
                                                CommandName="Edit" Text="Edit" />
                                            <asp:Button ID="btnDelete" runat="server" class="bitbtndt btndelete" 
                                                CommandName="Delete" 
                                                OnClientClick="return confirm('Sure to delete this data?');" Text="Delete" />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    
                                    <asp:BoundField DataField="NoDok" HeaderStyle-Width="100px" 
                                        HeaderText="No Dokumen" SortExpression="NoDok" >
                                        <HeaderStyle Width="100px" />
                                    </asp:BoundField>

                                    <asp:BoundField DataField="Descript" HeaderStyle-Width="100px" 
                                        HeaderText="Description" SortExpression="Descript">
                                        <HeaderStyle Width="100px" />
                                    </asp:BoundField>

                                        <asp:BoundField DataField="DocDate" DataFormatString="{0:dd MMM yyyy}" HtmlEncode="true"
                                         HeaderStyle-Width="80px" SortExpression="TransDate" 
                                         HeaderText="Dokumen Date">
                                         <HeaderStyle Width="80px" />
                                        </asp:BoundField>


                                    <asp:BoundField DataField="NoAJB" HeaderStyle-Width="50px" 
                                        HeaderText="NoAJB" SortExpression="NoAJB">
                                        <HeaderStyle Width="50px" />
                                    </asp:BoundField>

                                    <asp:BoundField DataField="Name" HeaderStyle-Width="50px" 
                                        HeaderText="Name" SortExpression="Name">
                                        <HeaderStyle Width="50px" />
                                    </asp:BoundField>

                                    <asp:BoundField DataField="Luas" HeaderStyle-Width="80px" HeaderText="Luas" 
                                        SortExpression="Luas" DataFormatString="{0:#,##0.##}" >
                                        <HeaderStyle Width="40px" />
                                    </asp:BoundField>

                                    <asp:BoundField DataField="Sisa" HeaderStyle-Width="80px" HeaderText="Sisa" 
                                        SortExpression="Sisa" DataFormatString="{0:#,##0.##}" >
                                        <HeaderStyle Width="40px" />
                                    </asp:BoundField>

                                    <asp:BoundField DataField="NoAJB2" HeaderStyle-Width="50px" 
                                        HeaderText="NoAJB2" SortExpression="NoAJB2">
                                        <HeaderStyle Width="50px" />
                                    </asp:BoundField>

                                    <asp:BoundField DataField="Name2" HeaderStyle-Width="50px" 
                                        HeaderText="Name2" SortExpression="Name2">
                                        <HeaderStyle Width="50px" />
                                    </asp:BoundField>

                                    <asp:BoundField DataField="Luas2" HeaderStyle-Width="80px" HeaderText="Luas2" 
                                        SortExpression="Luas2" DataFormatString="{0:#,##0.##}" >
                                        <HeaderStyle Width="40px" />
                                    </asp:BoundField>

                                    <asp:BoundField DataField="Sisa2" HeaderStyle-Width="80px" HeaderText="Sisa2" 
                                        SortExpression="Sisa2" DataFormatString="{0:#,##0.##}" >
                                        <HeaderStyle Width="40px" />
                                    </asp:BoundField>

                                    <asp:BoundField DataField="NoAJB3" HeaderStyle-Width="50px" 
                                        HeaderText="NoAJB3" SortExpression="NoAJB3">
                                        <HeaderStyle Width="50px" />
                                    </asp:BoundField>

                                    <asp:BoundField DataField="Name3" HeaderStyle-Width="50px" 
                                        HeaderText="Name3" SortExpression="Name3">
                                        <HeaderStyle Width="50px" />
                                    </asp:BoundField>

                                    <asp:BoundField DataField="Luas3" HeaderStyle-Width="80px" HeaderText="Luas3" 
                                        SortExpression="Luas3" DataFormatString="{0:#,##0.##}" >
                                        <HeaderStyle Width="40px" />
                                    </asp:BoundField>

                                    <asp:BoundField DataField="Sisa3" HeaderStyle-Width="80px" HeaderText="Sisa3" 
                                        SortExpression="Sisa3" DataFormatString="{0:#,##0.##}" >
                                        <HeaderStyle Width="40px" />
                                    </asp:BoundField>

                                     <asp:BoundField DataField="NoAJB4" HeaderStyle-Width="50px" 
                                        HeaderText="NoAJB4" SortExpression="NoAJB4">
                                        <HeaderStyle Width="50px" />
                                    </asp:BoundField>

                                    <asp:BoundField DataField="Name4" HeaderStyle-Width="50px" 
                                        HeaderText="Name4" SortExpression="Name4">
                                        <HeaderStyle Width="50px" />
                                    </asp:BoundField>

                                    <asp:BoundField DataField="Luas4" HeaderStyle-Width="80px" HeaderText="Luas4" 
                                        SortExpression="Luas4" DataFormatString="{0:#,##0.##}" >
                                        <HeaderStyle Width="40px" />
                                    </asp:BoundField>

                                    <asp:BoundField DataField="Sisa4" HeaderStyle-Width="80px" HeaderText="Sisa4" 
                                        SortExpression="Sisa4" DataFormatString="{0:#,##0.##}" >
                                        <HeaderStyle Width="40px" />
                                    </asp:BoundField>

                                     <asp:BoundField DataField="NoAJB5" HeaderStyle-Width="50px" 
                                        HeaderText="NoAJB5" SortExpression="NoAJB5">
                                        <HeaderStyle Width="50px" />
                                    </asp:BoundField>

                                    <asp:BoundField DataField="Name5" HeaderStyle-Width="50px" 
                                        HeaderText="Name5" SortExpression="Name5">
                                        <HeaderStyle Width="50px" />
                                    </asp:BoundField>

                                    <asp:BoundField DataField="Luas5" HeaderStyle-Width="80px" HeaderText="Luas5" 
                                        SortExpression="Luas5" DataFormatString="{0:#,##0.##}" >
                                        <HeaderStyle Width="40px" />
                                    </asp:BoundField>

                                    <asp:BoundField DataField="Sisa5" HeaderStyle-Width="80px" HeaderText="Sisa5" 
                                        SortExpression="Sisa5" DataFormatString="{0:#,##0.##}" >
                                        <HeaderStyle Width="40px" />
                                    </asp:BoundField>

                                    <asp:BoundField DataField="Remark" HeaderStyle-Width="50px" 
                                        HeaderText="Remark" SortExpression="Remark">
                                        <HeaderStyle Width="50px" />
                                    </asp:BoundField>

                                </Columns>
                            </asp:GridView>
                            <br />
                        </div>
                        <asp:Button class="bitbtn btnadd" runat="server" ID="btnAddDt4ke2" Text="Add" Visible="false"
                            ValidationGroup="Input" />
                        
                        &nbsp;
                        <asp:Button ID="btnBackDt2" runat="server" class="bitbtndt btnback" Text="Back" 
                            Width="60" />
                        
                    </asp:Panel>
                    <br />
                    <asp:Panel runat="server" ID="pnlEditDt4" Visible="false">
                        <table>

                                
                

                             <tr>
                                <td>Description</td>
                                <td>:</td>
                                <td colspan="7">
                                    <table>
                                     
                                        <tr>
                                            <td><asp:TextBox runat="server" ValidationGroup="Input" ID="tbDescriptiondt2" CssClass="TextBox" Width="130px"/>
                                        </td>  
                                                           
                                        </tr>
                                    </table>
                                </td>                
                            </tr>



                            <tr>
                                <td>No Dokumen</td>
                                <td>:</td>
                                <td colspan="7">
                                    <table>
                                     
                                        <tr>
                                            <td><asp:TextBox runat="server" MaxLength = "30" ValidationGroup="Input" ID="tbNoDok" CssClass="TextBox" Width="130px"/>
                                        </td>   
                                        <td></td>

                                <td>Dokumen Date</td>
                                <td>:</td>

                                <td>
                                <BDP:BasicDatePicker ID="tbDatedt2" runat="server" Width="130px" DateFormat="dd MMM yyyy" 
                                            ReadOnly = "False" ValidationGroup="Input"
                                            ButtonImageHeight="19px" ButtonImageWidth="20px" 
                                            DisplayType="TextBoxAndImage" 
                                            TextBoxStyle-CssClass="TextDate" AutoPostBack="False" 
                                            ShowNoneButton="False"><TextBoxStyle CssClass="TextDate" /></BDP:BasicDatePicker>                
                                </td>                               
                                        </tr>
                                    </table>
                                </td>                
                            </tr>

                           

                            <tr>
                                <td>Name</td>
                                <td>:</td>
                                <td colspan="7">
                                    <table>
                                     
                                        <tr>
                                            <td><asp:TextBox runat="server" ValidationGroup="Input" ID="tbNameSubDt" CssClass="TextBox" Width="130px"/></td>
                                <td><asp:TextBox runat="server" ValidationGroup="Input" ID="tbNameSubDt2" CssClass="TextBox" Width="130px"/></td>
                                <td><asp:TextBox runat="server" ValidationGroup="Input" ID="tbNameSubDt3" CssClass="TextBox" Width="130px"/></td>
                                <td><asp:TextBox runat="server" ValidationGroup="Input" ID="tbNameSubDt4" CssClass="TextBox" Width="130px"/></td>
                                <td><asp:TextBox runat="server" ValidationGroup="Input" ID="tbNameSubDt5" CssClass="TextBox" Width="130px"/></td>                                   
                                        </tr>
                                    </table>
                                </td>                
                            </tr> 

                            <tr>
                                <td>No AJB</td>
                                <td>:</td>
                                <td colspan="7">
                                    <table>
                                     
                                        <tr>
                                            <td><asp:TextBox runat="server" MaxLength = "50" ValidationGroup="Input" ID="tbAJBSubDt" CssClass="TextBox" Width="130px"/></td>                                  
                                            <td><asp:TextBox runat="server" MaxLength = "50" ValidationGroup="Input" ID="tbAJBSubDt2" CssClass="TextBox" Width="130px"/></td>                                  
                                            <td><asp:TextBox runat="server" MaxLength = "50" ValidationGroup="Input" ID="tbAJBSubDt3" CssClass="TextBox" Width="130px"/></td>                                  
                                            <td><asp:TextBox runat="server" MaxLength = "50" ValidationGroup="Input" ID="tbAJBSubDt4" CssClass="TextBox" Width="130px"/></td>                                  
                                            <td><asp:TextBox runat="server" MaxLength = "50" ValidationGroup="Input" ID="tbAJBSubDt5" CssClass="TextBox" Width="130px"/></td>                                  
                                        </tr>
                                    </table>
                                </td>                
                            </tr> 


                            <tr>
                                <td>Luas Ukur m<sup>2</sup></td>
                                <td>:</td>
                                <td colspan="7">
                                    <table>
                                     
                                        <tr>
                                            <td><asp:TextBox runat="server" ValidationGroup="Input" ID="tbLuasSubDt" CssClass="TextBox" Width="130px"/></td>                                  
                                            <td><asp:TextBox runat="server" ValidationGroup="Input" ID="tbLuasSubDt2" CssClass="TextBox" Width="130px"/></td>                                  
                                            <td><asp:TextBox runat="server" ValidationGroup="Input" ID="tbLuasSubDt3" CssClass="TextBox" Width="130px"/></td>                                  
                                            <td><asp:TextBox runat="server" ValidationGroup="Input" ID="tbLuasSubDt4" CssClass="TextBox" Width="130px"/></td>                                  
                                            <td><asp:TextBox runat="server" ValidationGroup="Input" ID="tbLuasSubDt5" CssClass="TextBox" Width="130px"/></td>                                  
                                        </tr>
                                    </table>
                                </td>                
                            </tr> 


                            <tr>
                                <td>Sisa M<sub>2</sub></td>
                                <td>:</td>
                                <td colspan="7">
                                    <table>
                                     
                                        <tr>
                                            <td><asp:TextBox ID="tbSisaSubDt" ValidationGroup="Input" runat="server" CssClass="TextBox" width="130px"/></td>
                                            <td><asp:TextBox ID="tbSisaSubDt2" ValidationGroup="Input" runat="server" CssClass="TextBox" width="130px"/></td>
                                            <td><asp:TextBox ID="tbSisaSubDt3" ValidationGroup="Input" runat="server" CssClass="TextBox" width="130px"/></td>
                                            <td><asp:TextBox ID="tbSisaSubDt4" ValidationGroup="Input" runat="server" CssClass="TextBox" width="130px"/></td>
                                            <td><asp:TextBox ID="tbSisaSubDt5" ValidationGroup="Input" runat="server" CssClass="TextBox" width="130px"/></td>                                  
                                        </tr>
                                    </table>
                                </td>                
                            </tr> 



                            <tr>
                                <td>Remark</td>
                                <td>:</td>
                                <td><asp:TextBox runat="server" ValidationGroup="Input" ID="tbremarkSubDt" CssClass="TextBox" Width="695px"/></td>
                            </tr> 

                        </table>
                        <br />
                        <asp:Button ID="btnSaveDt4" runat="server" class="bitbtndt btnsave" Text="Save" />
                        <asp:Button ID="btnCancelDt4" runat="server" class="bitbtndt btncancel" Text="Cancel" />
                    </asp:Panel>
                </asp:View>

            </asp:MultiView>
            <br />
            <asp:Button ID="btnSaveAll" runat="server" class="bitbtndt btnsavenew" Text="Save & New"
                ValidationGroup="Input" Width="90" />
            <asp:Button ID="btnSaveTrans" runat="server" class="bitbtndt btnsave" Text="Save"
                ValidationGroup="Input" />
            <asp:Button ID="btnBack" runat="server" class="bitbtndt btncancel" Text="Cancel"
                ValidationGroup="Input" />
            <asp:Button ID="btnHome" runat="server" class="bitbtndt btnback" Text="Home" />
        </asp:Panel>
        
    </div>
   
    <asp:Label runat="server" ID="lbStatus" ForeColor="Red" />
    <asp:HiddenField ID="HiddenRemarkReject" runat="server" />
    </form>
</body>
</html>