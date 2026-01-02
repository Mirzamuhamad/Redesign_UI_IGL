<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TrTEstForm.aspx.vb"
    Inherits="Transaction_TrLandPurchaseReq_TrLandPurchaseReq" %>

    <%@ Register Assembly="BasicFrame.WebControls.BasicDatePicker" Namespace="BasicFrame.WebControls" TagPrefix="BDP" %>

        <!DOCTYPE html
            PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
        <html xmlns="http://www.w3.org/1999/xhtml">

        <head runat="server">
            <title>Land Survey</title>

             <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.5.0/css/all.min.css" />
            <link rel="stylesheet" href="https://code.jquery.com/ui/1.13.2/themes/base/jquery-ui.css" />
            <!-- <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" /> -->
            <link href="https://fonts.googleapis.com/css2?family=Muli:wght@300;400;600&display=swap" rel="stylesheet">
            <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" rel="stylesheet" />
            <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/js/bootstrap.bundle.min.js"></script>
            <link href="../../Styles/StyleNew.css" rel="stylesheet" type="text/css" />

            <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
            <script src="../../Function/Function.JS" type="text/javascript"></script>
            
            <script src="../../Function/OpenDlg.js" type="text/javascript"></script>
            

            <script type="text/javascript">

                function OpenPopup() {
                    window.open("../../SeaDlg.Aspx", "List", "scrollbars=yes,resizable=no,width=500,height=400");
                    return false;
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
                function setdigit(nStr, digit) {
                    try {
                        var TNstr = parseFloat(nStr);
                        if (parseFloat(digit) >= 0) {
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
                    } catch (err) {
                        alert(err.description);
                    }
                }

                function setformatdt() {
                    try {
                        var _QtyOutput = parseFloat(document.getElementById("tbQtyM").value.replace(/\$|\,/g, ""));
                        var _QtyWO = parseFloat(document.getElementById("tbQtyT").value.replace(/\$|\,/g, ""));
                        var _QtyGood = parseFloat(document.getElementById("tbQtyB").value.replace(/\$|\,/g, ""));
                        //            var _QtyRepair = parseFloat(document.getElementById("tbQtyRepair").value.replace(/\$|\,/g,""));
                        var _QtyReject = parseFloat(document.getElementById("tbQtyS").value.replace(/\$|\,/g, ""));



                        document.getElementById("tbQtyM").value = setdigit(_QtyM, '<%=VIEWSTATE("DigitQty")%>');
                        document.getElementById("tbQtyT").value = setdigit(_QtyT, '<%=VIEWSTATE("DigitQty")%>');
                        document.getElementById("tbQtyB").value = setdigit(_QtyB, '<%=VIEWSTATE("DigitQty")%>');
                        document.getElementById("tbQtyS").value = setdigit(_QtyS, '<%=VIEWSTATE("DigitQty")%>');
                        //alert("test 2");                                                

                    } catch (err) {
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



                function Reject() {
                    try {
                        var result = prompt("Remark Reject", "");
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

           


        </head>

        <body>
            <form id="form1" runat="server">
                <asp:ScriptManager ID="ScriptManager1" runat="server">
                </asp:ScriptManager>
                <div class="Content">

                    <!-- embed hiper kompress pdf -->
                    <!-- <div id="avepdf-container-id">
                        <script type="text/javascript" src="https://avepdf.com/api/js/embedwidgets.js"></script>
                            <script type="text/javascript">
                                loadAvePDFWidget('692b96ed-b91c-4a1d-bf53-ba8cf411e8a0', 'auto', 'hyper-compress-pdf', 'avepdf-container-id');
                            </script>
                        </div> -->

                    <div class="card bg-white sticky-top shadow-sm border-0 rounded-0">
                            <div class="card-body py-2">
                                <h6 class="mb-0">Sample Form Transaksi Boostrap</h6>
                                <!-- <label class="form-label">Sample Form Transaksi Boostrap</label> -->
                            </div>
                        </div>
                    <!-- <hr style="color: Blue" /> -->
                    <asp:Panel runat="server" ID="PnlHd">
                        <div class="container-fluid mt-2">
                            <div class="row mb-2 align-items-center">
                                <!-- Kolom kiri: filter & tombol -->
                                <div class="col d-flex gap-2 flex-wrap">

                                    <asp:DropDownList CssClass="form-select form-select-sm w-auto" ID="ddlCommand"
                                        runat="server" Visible="false" />
                                    <button id="BtnGo" runat="server"
                                        class="btn btn-success btn-sm icon-btn btn-icon-check" Visible="false">Go
                                    </button>


                                    <!-- Textbox dengan placeholder -->
                                    <asp:TextBox runat="server" ID="tbFilter"
                                        CssClass="form-control form-control-sm w-auto" placeholder="Cari data...">
                                    </asp:TextBox>

                                    <!-- Dropdown -->
                                    <asp:DropDownList runat="server" ID="ddlField"
                                        CssClass="form-select form-select-sm w-auto">
                                        <asp:ListItem Value="TransNmbr" Selected="True">Reference</asp:ListItem>
                                        <asp:ListItem>Status</asp:ListItem>
                                        <asp:ListItem Value="dbo.FormatDate(TransDate)">Date</asp:ListItem>
                                        <asp:ListItem Value="Kohir">Nomor Kohir</asp:ListItem>
                                        <asp:ListItem Value="NoDocSertifikat">Nomor Dokumen</asp:ListItem>
                                        <asp:ListItem Value="FgImport">Data Import</asp:ListItem>
                                        <asp:ListItem>Remark</asp:ListItem>
                                    </asp:DropDownList>

                                    <asp:DropDownList CssClass="form-select form-select-sm w-auto" runat="server"
                                        ID="ddlRange">
                                    </asp:DropDownList>

                                    <!-- Tombol dengan ikon -->
                                    <button runat="server" id="btnSearch"
                                        class="btn btn-primary btn-sm icon-btn btn-icon-search">
                                        Search
                                    </button>

                                    <asp:LinkButton runat="server" ID="btnExpand" CssClass="btn btn-secondary btn-sm">
                                        <i class="fa fa-ellipsis-h"></i>
                                    </asp:LinkButton>

                                    <asp:LinkButton ID="LbAdvSearch" runat="server" CssClass="btn btn-secondary btn-sm"
                                        Text="Advanced Search" />
                                </div>

                                <!-- Kolom kanan: tombol Add -->
                                <div class="col-auto d-flex justify-content-end">
                                    <asp:LinkButton ID="BtnAdd" runat="server" CssClass="btn btn-primary btn-sm px-5">
                                        <i class="fa fa-plus"></i> Add
                                    </asp:LinkButton>

                                    <%--<asp:LinkButton ID="BtnAdd" runat="server"
                                        CssClass="btn btn-primary btn-sm px-5" Text="Add">
                                        </asp:LinkButton>--%>
                                </div>
                            </div>
                        </div>

                        <asp:Panel runat="server" ID="pnlSearch" Visible="false">
                            <div class="container-fluid">
                                <div class="row mb-2">
                                    <div class="col d-flex gap-2 flex-wrap">
                                        <!-- Dropdown Notasi -->
                                        <asp:DropDownList CssClass="form-select form-select-sm w-auto" runat="server"
                                            ID="ddlNotasi">
                                            <asp:ListItem Selected="true" Text="OR" Value="OR"></asp:ListItem>
                                            <asp:ListItem Text="AND" Value="AND"></asp:ListItem>
                                        </asp:DropDownList>

                                        <!-- Textbox dengan placeholder -->
                                        <asp:TextBox runat="server" ID="tbfilter2"
                                            CssClass="form-control form-control-sm w-auto" placeholder="Cari data...">
                                        </asp:TextBox>

                                        <!-- Dropdown Field -->
                                        <asp:DropDownList runat="server" ID="ddlField2"
                                            CssClass="form-select form-select-sm w-auto">
                                            <asp:ListItem Value="TransNmbr" Selected="True">Reference</asp:ListItem>
                                            <asp:ListItem>Status</asp:ListItem>
                                            <asp:ListItem Value="dbo.FormatDate(TransDate)">Date</asp:ListItem>
                                            <asp:ListItem Value="Kohir">Nomor Kohir</asp:ListItem>
                                            <asp:ListItem Value="NoDocSertifikat">Nomor Dokumen</asp:ListItem>
                                            <asp:ListItem Value="FgImport">Data Import</asp:ListItem>
                                            <asp:ListItem>Remark</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>

                        </asp:Panel>


                        <div class="container-fluid mt-0">
                            <div class="card shadow-sm">
                                <div class="card-body p-2">
                                    <!-- wrapper biar tabel bisa scroll kanan & vertikal -->
                                    <div class="table-responsive" style="max-height:100vh; overflow-y:auto;">
                                        <asp:GridView ID="GridView1" runat="server" ShowFooter="false"
                                            AllowSorting="True" AutoGenerateColumns="False" AllowPaging="True"
                                            PageSize="10"
                                            CssClass="table table-striped table-bordered table-hover table-sm  text-nowrap table-soft-text">
                                            <HeaderStyle CssClass="table-soft-dark text-white sticky-top" />
                                            <FooterStyle CssClass="table-light" />
                                            <PagerStyle CssClass="grid-pager text-center bg-light"
                                                HorizontalAlign="Left" />

                                            <Columns>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        <asp:CheckBox ID="cbSelectHd" runat="server" AutoPostBack="true"
                                                            OnCheckedChanged="cbSelectHd_CheckedChanged" />
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="cbSelect" runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Action">

                                                    <ItemTemplate>
                                                        <asp:DropDownList
                                                            CssClass="form-select form-select-sm d-inline w-auto"
                                                            ID="ddl" runat="server">
                                                            <asp:ListItem Selected="True" Text="View" />
                                                            <asp:ListItem Text="Edit" />
                                                            <asp:ListItem Text="Print Check Doc" />
                                                            <asp:ListItem Text="Print Riwayat" />
                                                            <asp:ListItem Text="Print Letter C" />
                                                            <asp:ListItem Text="Reject" />

                                                            <%--<asp:ListItem Text="Print" />--%>
                                                        </asp:DropDownList>
                                                        <asp:Button CssClass="btn btn-primary btn-sm ms-1"
                                                            runat="server" ID="btnGo" Text="Go"
                                                            CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>"
                                                            CommandName="Go" />
                                                    </ItemTemplate>
                                                    <HeaderStyle Width="110px" />
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="TransNmbr" HeaderStyle-Width="120px"
                                                    SortExpression="TransNmbr" HeaderText="Request No">
                                                    <HeaderStyle Width="120px" />
                                                </asp:BoundField>

                                                <asp:TemplateField HeaderText="Status">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblStatus" runat="server"
                                                            Text='<%# Eval("Status") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="TransDate" DataFormatString="{0:dd MMM yyyy}"
                                                    HtmlEncode="true" HeaderStyle-Width="80px"
                                                    SortExpression="TransDate" HeaderText="Date">
                                                    <HeaderStyle Width="80px" />
                                                </asp:BoundField>

                                                <asp:BoundField DataField="Block" HeaderStyle-Width="200px"
                                                    SortExpression="Block" HeaderText="Block No">
                                                    <HeaderStyle Width="200px" />
                                                </asp:BoundField>

                                                <asp:BoundField DataField="Kohir" HeaderStyle-Width="200px"
                                                    SortExpression="Kohir" HeaderText="Kohir No">
                                                    <HeaderStyle Width="200px" />
                                                </asp:BoundField>

                                                <asp:BoundField DataField="Persil" HeaderStyle-Width="200px"
                                                    SortExpression="Persil" HeaderText="Persil No">
                                                    <HeaderStyle Width="200px" />
                                                </asp:BoundField>

                                                <asp:BoundField DataField="JnsDocSertifikat" HeaderStyle-Width="200px"
                                                    SortExpression="JnsDocSertifikat" HeaderText="Jenis Dokumen">
                                                    <HeaderStyle Width="200px" />
                                                </asp:BoundField>

                                                <asp:BoundField DataField="NoDocSertifikat" HeaderStyle-Width="200px"
                                                    SortExpression="NoDocSertifikat" HeaderText="No Dokumen">
                                                    <HeaderStyle Width="200px" />
                                                </asp:BoundField>

                                                <asp:BoundField DataField="TglTerbit" DataFormatString="{0:dd MMM yyyy}"
                                                    HtmlEncode="true" HeaderStyle-Width="200px"
                                                    SortExpression="TglTerbit" HeaderText="Tanggal Terbit">
                                                    <HeaderStyle Width="200px" />
                                                </asp:BoundField>

                                                <asp:BoundField DataField="HrgFix" ItemStyle-HorizontalAlign="right"
                                                    HeaderText="Luas" DataFormatString="{0:#,##0.##}"
                                                    SortExpression="LuasUkur">
                                                    <HeaderStyle Width="300px" />
                                                </asp:BoundField>

                                                <asp:BoundField DataField="Remark" HeaderStyle-Width="250px"
                                                    HeaderText="Remark" SortExpression="Remark">
                                                    <HeaderStyle Width="250px" />
                                                </asp:BoundField>

                                                <asp:BoundField DataField="RemarkReject"
                                                    HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="250px"
                                                    HeaderText="Reject" SortExpression="RemarkReject">
                                                    <HeaderStyle Width="250px" />
                                                </asp:BoundField>

                                            </Columns>
                                        </asp:GridView>
                                    </div>

                                    <div class="d-flex justify-content-between align-items-center mt-2">
                                        <label class="me-2 ">Rows Page:</label>
                                        <asp:DropDownList ID="ddlPageSize" runat="server" AutoPostBack="True"
                                            CssClass="form-select form-select-sm form-select-sm w-auto">
                                            <asp:ListItem Text="10" Value="10" Selected="True" />
                                            <asp:ListItem Text="25" Value="25" />
                                            <asp:ListItem Text="50" Value="50" />
                                            <asp:ListItem Text="100" Value="100" />
                                        </asp:DropDownList>
                                    </div>

                                </div>
                            </div>
                        </div>

                        <asp:Panel runat="server" ID="pnlNav" Visible="false">
                            <asp:Button class="bitbtn btnadd" runat="server" ID="btnAdd2" Text="Add" />
                            &nbsp &nbsp &nbsp
                            <asp:DropDownList ID="ddlCommand2" CssClass="DropDownList" runat="server" />
                            <asp:Button class="btngo" runat="server" ID="btnGo2" Text="G" />
                        </asp:Panel>
                    </asp:Panel>

                    <asp:Panel runat="server" ID="pnlInput" Visible="false">
                        <div class="container-fluid mt-1">
                            <div class="row align-items-center mb-1">
                                <div class="col-md-8">
                                    <asp:Menu ID="Menu2" runat="server" Orientation="Horizontal"
                                        StaticEnableDefaultPopOutImage="False" StaticMenuItemStyle-CssClass="MenuItem"
                                        StaticSelectedStyle-CssClass="MenuSelect">
                                        <Items>
                                            <asp:MenuItem Text="General" Value="0"></asp:MenuItem>
                                            <asp:MenuItem Text="Perlengkapan Dokumen" Value="1"></asp:MenuItem>
                                            <asp:MenuItem Text="Perlengkapan Dokumen 2" Value="2"></asp:MenuItem>
                                        </Items>
                                    </asp:Menu>
                                </div>
                                <div class="col-md-4 text-end">
                                    <asp:Button CssClass="btn btn-primary btn-sm btn-icon btnedit" runat="server"
                                        ID="btnGoEdit" Text="Edit Data" />
                                </div>

                                <div class="container-fluid mt-0">
                                    <hr class="my-1 border border-secondary" />
                                </div>

                                <asp:MultiView ID="MultiView2" runat="server" ActiveViewIndex="0">
                                    <asp:View ID="Tab0" runat="server">
                                        <div class="container-fluid mt-2">
                                            <div class="row mb-1">
                                                <!-- Offering Survey No -->
                                                <div class="col-md-3">
                                                    <label class="form-label">Offering Survey No</label>
                                                    <asp:TextBox ID="tbCode" runat="server"
                                                        CssClass="form-control form-control-sm"></asp:TextBox>
                                                </div>

                                                <!-- Offering Survey Date -->
                                                <div class="col-md-3">
                                                    <label class="form-label">Offering Survey Date</label>
                                                    <BDP:BasicDatePicker ID="tbDate" runat="server"
                                                        DateFormat="dd MMM yyyy" ReadOnly="True" ValidationGroup="Input"
                                                        ButtonImageHeight="19px" ButtonImageWidth="20px"
                                                        DisplayType="TextBox" AutoPostBack="True"
                                                        ShowNoneButton="False">
                                                        <TextBoxStyle CssClass="form-control form-control-sm" />
                                                    </BDP:BasicDatePicker>
                                                </div>

                                                <div class="col-md-3">
                                                    <label class="form-label">
                                                        <asp:LinkButton ID="lbSeller" runat="server"
                                                            ValidationGroup="Input" Text="Seller"
                                                            CssClass="text-decoration-none" />
                                                    </label>
                                                    <div class="row g-1">
                                                        <div class="col-md-10">
                                                            <asp:TextBox ID="tbSeller" runat="server"
                                                                CssClass="form-control form-control-sm" />
                                                            <asp:TextBox ID="tbSellerName" runat="server"
                                                                CssClass="form-control form-control-sm mt-1"
                                                                Visible="false" />
                                                        </div>
                                                        <div class="col-md-2 d-flex align-items-end">
                                                            <asp:Button ID="btnSeller" runat="server"
                                                                CssClass="btn btn-primary btn-sm w-100" Text="v" />
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="col-md-3">
                                                    <label class="form-label">
                                                        <asp:LinkButton ID="lbModerator" runat="server"
                                                            ValidationGroup="Input" Text="Moderator"
                                                            CssClass="text-decoration-none" />
                                                    </label>
                                                    <div class="row g-1">
                                                        <div class="col-md-10">
                                                            <asp:TextBox ID="tbModerator" runat="server"
                                                                CssClass="form-control form-control-sm" />
                                                            <asp:TextBox ID="tbModeratorName" runat="server"
                                                                CssClass="form-control form-control-sm mt-1"
                                                                Visible="false" />
                                                        </div>
                                                        <div class="col-md-2 d-flex align-items-end">
                                                            <asp:Button ID="btnModerator" runat="server"
                                                                CssClass="btn btn-primary btn-sm w-100" Text="v" />
                                                        </div>

                                                    </div>
                                                </div>


                                            </div>


                                            <div class="row mb-1">
                                                <!-- Offering Survey No -->
                                                <div class="col-md-4">
                                                    <label class="form-label">Nama Pembeli</label>
                                                    <asp:TextBox ID="tbNamaPembeli" runat="server"
                                                        CssClass="form-control form-control-sm"></asp:TextBox>
                                                </div>


                                                <div class="col-md-4">
                                                    <label class="form-label">
                                                        <asp:LinkButton ID="lbArea" runat="server"
                                                            ValidationGroup="Input" Text="Area"
                                                            CssClass="text-decoration-none" />
                                                    </label>
                                                    <div class="row g-1">
                                                        <div class="col-md-10">
                                                            <asp:TextBox ID="tbArea" runat="server"
                                                                CssClass="form-control form-control-sm" />
                                                            <asp:TextBox ID="tbAreaName" runat="server"
                                                                CssClass="form-control form-control-sm mt-1"
                                                                Visible="false" />
                                                        </div>
                                                        <div class="col-md-2 d-flex align-items-end">
                                                            <asp:Button ID="btnArea" runat="server"
                                                                CssClass="btn btn-primary btn-sm w-100" Text="v" />
                                                        </div>

                                                    </div>
                                                </div>

                                                <div class="col-md-4">
                                                    <label class="form-label">Land Type</label>
                                                    <asp:DropDownList ID="ddlLandType" runat="server"
                                                        CssClass="form-select form-select-sm">
                                                        <asp:ListItem Selected="True">Sporadik</asp:ListItem>
                                                        <asp:ListItem>Kavling</asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                            </div>


                                            <div class="row mb-1">
                                                <div class="col-md-3">
                                                    <label class="form-label">Jenis Dokumen</label>
                                                    <asp:DropDownList ID="ddlJenisDokumen" runat="server"
                                                        CssClass="form-select form-select-sm">
                                                        <asp:ListItem Selected="True">Choose One</asp:ListItem>
                                                        <asp:ListItem>AJB</asp:ListItem>
                                                        <asp:ListItem>SPH</asp:ListItem>
                                                        <asp:ListItem>SHM</asp:ListItem>
                                                        <asp:ListItem>SHGB</asp:ListItem>
                                                        <asp:ListItem>SHGU</asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>

                                                <div class="col-md-3">
                                                    <label class="form-label">Nomor Dokumen</label>
                                                    <asp:TextBox ID="tbNoDokumen" runat="server"
                                                        CssClass="form-control form-control-sm"></asp:TextBox>
                                                </div>

                                                <div class="col-md-3">
                                                    <label class="form-label">Tanggal Terbit</label>
                                                    <BDP:BasicDatePicker ID="tbTglTerbit" runat="server"
                                                        DateFormat="dd MMM yyyy" ReadOnly="True" ValidationGroup="Input"
                                                        ButtonImageHeight="19px" ButtonImageWidth="20px"
                                                        DisplayType="TextBox" AutoPostBack="True"
                                                        ShowNoneButton="False">
                                                        <TextBoxStyle CssClass="form-control form-control-sm" />
                                                    </BDP:BasicDatePicker>
                                                </div>

                                                <div class="col-md-3">
                                                    <label class="form-label">Masa Berlaku</label>
                                                    <BDP:BasicDatePicker ID="tbMasaBerlaku" runat="server"
                                                        DateFormat="dd MMM yyyy" ReadOnly="True" ValidationGroup="Input"
                                                        ButtonImageHeight="19px" ButtonImageWidth="20px"
                                                        DisplayType="TextBox" AutoPostBack="True"
                                                        ShowNoneButton="False">
                                                        <TextBoxStyle CssClass="form-control form-control-sm" />
                                                    </BDP:BasicDatePicker>
                                                </div>

                                            </div>


                                            <div class="row mb-1">


                                                <div class="col-md-2">
                                                    <label class="form-label">Luas SPPT</label>
                                                    <asp:TextBox ID="tbSPPT" runat="server"
                                                        CssClass="form-control form-control-sm"></asp:TextBox>
                                                </div>

                                                <div class="col-md-2">
                                                    <label class="form-label">Luas Ukur<sup>2</sup></label>
                                                    <asp:TextBox ID="tbAjbSphShm" runat="server"
                                                        CssClass="form-control form-control-sm"></asp:TextBox>
                                                </div>

                                                <div class="col-md-2">
                                                    <label class="form-label">Luas Ukur<sup>2</sup></label>
                                                    <asp:TextBox ID="tbLuasUkur" runat="server"
                                                        CssClass="form-control form-control-sm"></asp:TextBox>
                                                </div>

                                                <div class="col-md-2">
                                                    <label class="form-label">Nilai Tanah/m<sup>2</sup></label>
                                                    <asp:TextBox ID="tbNilai" runat="server"
                                                        CssClass="form-control form-control-sm"></asp:TextBox>
                                                </div>


                                                <div class="col-md-2">
                                                    <label class="form-label">Luas AJB/SPH/SHM</label>
                                                    <asp:DropDownList ID="ddlHitungTotal" runat="server"
                                                        CssClass="form-select form-select-sm" AutoPostBack="true">
                                                        <asp:ListItem Selected="True">Choose One</asp:ListItem>
                                                        <asp:ListItem>SPPT</asp:ListItem>
                                                        <asp:ListItem>AJB/SPH/SHM</asp:ListItem>
                                                        <asp:ListItem>Luas Ukur</asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>

                                                <div class="col-md-2">
                                                    <label class="form-label">Total Nilai(Rp)</label>
                                                    <asp:TextBox ID="tbTotal" runat="server"
                                                        CssClass="form-control form-control-sm"></asp:TextBox>
                                                    <asp:TextBox runat="server" ValidationGroup="Input" ID="tbHrgFix"
                                                        Visible="false" CssClass="TextBox" Width="100px"
                                                        Enabled="false" />
                                                </div>




                                            </div>

                                            <div class="row mb-1">
                                                <div class="col-md-2">
                                                    <label class="form-label">Provinsi</label>
                                                    <asp:TextBox ID="tbProvinsi" runat="server"
                                                        CssClass="form-control form-control-sm"></asp:TextBox>
                                                </div>

                                                <div class="col-md-2">
                                                    <label class="form-label">Kabupaten</label>
                                                    <asp:TextBox ID="tbKab" runat="server"
                                                        CssClass="form-control form-control-sm"></asp:TextBox>
                                                </div>


                                                <div class="col-md-2">
                                                    <label class="form-label">Kecamatan</label>
                                                    <asp:TextBox ID="tbKec" runat="server"
                                                        CssClass="form-control form-control-sm"></asp:TextBox>
                                                </div>

                                                <div class="col-md-2">
                                                    <label class="form-label">Desa</label>
                                                    <asp:TextBox ID="tbDesa" runat="server"
                                                        CssClass="form-control form-control-sm"></asp:TextBox>
                                                </div>

                                                <div class="col-md-4">
                                                    <label class="form-label">Addresss</label>
                                                    <asp:TextBox ID="tbAddress" runat="server"
                                                        CssClass="form-control form-control-sm"></asp:TextBox>
                                                </div>
                                            </div>



                                            <div class="row mb-1">
                                                <div class="col-md-3">
                                                    <label class="form-label">No Sppt/Pbb</label>
                                                    <asp:TextBox ID="tbSptPbb" runat="server"
                                                        CssClass="form-control form-control-sm"></asp:TextBox>
                                                </div>

                                                <div class="col-md-3">
                                                    <label class="form-label">No Peta Rincik</label>
                                                    <asp:TextBox ID="tbPetaRincikNo" runat="server"
                                                        CssClass="form-control form-control-sm"></asp:TextBox>
                                                </div>


                                                <div class="col-md-3">
                                                    <label class="form-label">No Girik Blok</label>
                                                    <asp:TextBox ID="tbBlok" runat="server"
                                                        CssClass="form-control form-control-sm"></asp:TextBox>
                                                </div>

                                                <div class="col-md-3">
                                                    <label class="form-label">No NIB</label>
                                                    <asp:TextBox ID="tbBNIB" runat="server"
                                                        CssClass="form-control form-control-sm"></asp:TextBox>
                                                </div>

                                            </div>

                                            <div class="row mb-1">
                                                <div class="col-md-3">
                                                    <label class="form-label">No Girik Kohir</label>
                                                    <asp:TextBox ID="tbKohir" runat="server"
                                                        CssClass="form-control form-control-sm"></asp:TextBox>
                                                </div>

                                                <div class="col-md-3">
                                                    <label class="form-label">No Surat Ukur</label>
                                                    <asp:TextBox ID="tbNoSuratUkur" runat="server"
                                                        CssClass="form-control form-control-sm"></asp:TextBox>
                                                </div>


                                                <div class="col-md-3">
                                                    <label class="form-label">No Girik Percil</label>
                                                    <asp:TextBox ID="tbPercil" runat="server"
                                                        CssClass="form-control form-control-sm"></asp:TextBox>
                                                </div>

                                                <div class="col-md-3">
                                                    <label class="form-label">No Lain-Lain</label>
                                                    <asp:TextBox ID="tbNoLainLian" runat="server"
                                                        CssClass="form-control form-control-sm"></asp:TextBox>
                                                </div>

                                            </div>

                                            <div class="row mb-1">
                                                <div class="col-md-6">
                                                    <label class="form-label">Remark Reject</label>
                                                    <asp:TextBox ID="tbRemarkReject" enabled="false" runat="server"
                                                        CssClass="form-control form-control-sm"></asp:TextBox>
                                                </div>

                                                <div class="col-md-6">
                                                    <label class="form-label">Remark</label>
                                                    <asp:TextBox ID="tbRemark" runat="server"
                                                        CssClass="form-control form-control-sm"></asp:TextBox>
                                                </div>
                                            </div>


                                            <table>
                                                <td>

                                                    <asp:DropDownList ID="ddlseller" runat="server"
                                                        CssClass="form-select form-select-sm" Visible="false"
                                                        AutoPostBack="false" />


                                                    <asp:DropDownList CssClass="DropDownList" Width="230px"
                                                        ValidationGroup="Input" runat="server" ID="ddlModerator"
                                                        Visible="False" AutoPostBack="false" />

                                                    <asp:DropDownList CssClass="DropDownList" Width="230px"
                                                        ValidationGroup="Input" runat="server" ID="ddlArea"
                                                        Visible="False" AutoPostBack="false" />
                                                </td>
                                                </tr>
                                                <tr>

                                                    <%--<td>No AJB</td>
                                                        <td>:</td>--%>
                                                        <td>
                                                            <asp:TextBox runat="server" ValidationGroup="Input"
                                                                ID="tbAJB" Visible="false" CssClass="TextBox"
                                                                Width="225px" />
                                                        </td>


                                                        <%-- <td>No SPH</td>
                                                            <td>:</td>--%>
                                                            <td>
                                                                <asp:TextBox runat="server" ValidationGroup="Input"
                                                                    ID="tbSPH" Visible="false" CssClass="TextBox"
                                                                    Width="225px" />
                                                            </td>



                                                <tr>



                                                    <%-- <td>No SHM</td>
                                                        <td>:</td>--%>
                                                        <td>
                                                            <asp:TextBox runat="server" ValidationGroup="Input"
                                                                ID="tbSHM" Visible="false" CssClass="TextBox"
                                                                Width="225px" />
                                                        </td>

                                                        <%--<td>Jenis Dokumen</td>
                                                            <td>:</td> --%>
                                                            <td>
                                                                <asp:DropDownList CssClass="DropDownList"
                                                                    ValidationGroup="Input" Visible="False"
                                                                    Width="230px" runat="server" ID="ddlJenisDok">
                                                                    <asp:ListItem Selected="True">AJB</asp:ListItem>
                                                                    <asp:ListItem>SPH</asp:ListItem>
                                                                    <asp:ListItem>SHM</asp:ListItem>
                                                                </asp:DropDownList>
                                                                <%--&nbsp; &nbsp; &nbsp; &nbsp; No Doc : --%>
                                                                    <asp:TextBox ID="tbNoDocHD" runat="server"
                                                                        CssClass="TextBox" Visible="False"
                                                                        Width="105px" />
                                                            </td>
                                                </tr>


                                            </table>
                                        </div>

                            </div>

                            </asp:View>

                            <asp:View ID="Tab1" runat="server">
                                <div class="container-fluid mt-1">
                                    <table>

                                        <tr>
                                            <td>
                                                <asp:checkbox id="cbKtp" Enabled="True" runat="server"
                                                    text=" KTP Penjual" autopostback="False" />
                                            </td>

                                            <td>
                                                <asp:FileUpload style="color: White;" runat="server"
                                                    accept="application/pdf" ID="FupMain" />
                                                <!--  <asp:RegularExpressionValidator ID="FileUpLoadValidator" runat="server" ErrorMessage="Upload PDF files only .!!!"
                                                    ValidationExpression="^(([a-zA-Z]:)|(\\{2}\w+)\$?)(\\(\w[\w].*))(.pdf|.PDF)$" 
                                                ControlToValidate="FupMain"> 
                                                </asp:RegularExpressionValidator>  -->
                                                <asp:Label ID="lblmassageKTP" runat="server" CssClass="labelMassage"
                                                    Visible="false" Text="File uploaded successfully"></asp:Label>
                                            </td>




                                            <td>
                                                <asp:Button ID="btnKtp" Text="Upload" runat="server"
                                                    Style="display: none" />
                                                <asp:LinkButton ID="lbKtp" ValidationGroup="Input" runat="server"
                                                    Text="Not Yet Uploaded" />
                                            </td>

                                            <td>
                                                <asp:LinkButton ID="btnDelKTP" visible="false"
                                                    class="bitbtndt btndelete" ForeColor="#e36e6e"
                                                    ValidationGroup="Input" runat="server" Text="Delete"
                                                    OnClientClick="return confirm('Sure to delete this dokumen?');" />
                                            </td>
                                        </tr>

                                        <tr>
                                            <td>
                                                <asp:checkbox id="cbBPJS" Enabled="True" runat="server"
                                                    text=" BPJS Penjual" autopostback="False" />
                                            </td>

                                            <td>

                                                <asp:FileUpload style="color: White;" runat="server"
                                                    accept="application/pdf" ID="FubBPJS" />
                                                <!--  <asp:RegularExpressionValidator ID="FileUpLoadValidator30" runat="server" ErrorMessage="Upload PDF files only .!!!"
                            ValidationExpression="^(([a-zA-Z]:)|(\\{2}\w+)\$?)(\\(\w[\w].*))(.pdf|.PDF)$" 
                           ControlToValidate="FubBPJS"> 
                        </asp:RegularExpressionValidator>  -->
                                                <asp:Label ID="lblmassageBPJS" runat="server" CssClass="labelMassage"
                                                    Visible="false" Text="File uploaded successfully"></asp:Label>
                                            </td>




                                            <td>
                                                <asp:Button ID="btnSaveBPJS" Text="Upload" runat="server"
                                                    Style="display: none" />
                                                <!-- <asp:HyperLink id="hlKtp" NavigateUrl="~/Image/Dokumen/IGLLPR22020002Invoice.pdf" runat="server" Text="View PDF" Target="_blank" /> -->
                                                <asp:LinkButton ID="lbBPJS" ValidationGroup="Input" runat="server"
                                                    Text="Not Yet Uploaded" />
                                                <!-- <asp:Button ID="btnsaveKtp" CssClass="bitbtndt btnadd" runat="server"  OnClientClick = "SetTarget();" Text="View" /> 
                                                                        -->
                                            </td>
                                        </tr>

                                        <tr>
                                            <td>
                                                <asp:checkbox id="cbKk" runat="server" text=" KK Permohonan Penjual"
                                                    autopostback="False" />
                                            </td>

                                            <td>
                                                <asp:FileUpload style="color: White;" runat="server"
                                                    accept="application/pdf" ID="FubKK" />
                                                <!-- <asp:RegularExpressionValidator ID="FileUpLoadValidator2" runat="server" ErrorMessage="Upload PDF files only .!!!"
                            ValidationExpression="^(([a-zA-Z]:)|(\\{2}\w+)\$?)(\\(\w[\w].*))(.pdf|.PDF)$" 
                           ControlToValidate="FubKK"> 
                        </asp:RegularExpressionValidator>  -->
                                                <asp:Label ID="lblmassageKK" CssClass="labelMassage" runat="server"
                                                    Visible="False" Text="File uploaded successfully"></asp:Label>

                                            </td>

                                            <td>
                                                <asp:LinkButton ID="lbKK" ValidationGroup="Input" runat="server"
                                                    Text="Not Yet Uploaded" />
                                                <asp:Button ID="btnsaveKK" CssClass="bitbtndt btnadd" runat="server"
                                                    Text="View" Style="display: none" />
                                            </td>
                                        </tr>

                                        <tr>
                                            <td>
                                                <asp:checkbox id="cbSPPT" runat="server" text=" SPPT PBB tanah berjalan"
                                                    autopostback="False" />
                                            </td>

                                            <td>
                                                <asp:FileUpload style="color: White;" runat="server"
                                                    accept="application/pdf" ID="FubSPPT" />
                                                <!-- <asp:RegularExpressionValidator ID="FileUpLoadValidator3" runat="server" ErrorMessage="Upload PDF files only .!!!"
                            ValidationExpression="^(([a-zA-Z]:)|(\\{2}\w+)\$?)(\\(\w[\w].*))(.pdf|.PDF)$" 
                           ControlToValidate="FubSPPT"> 
                        </asp:RegularExpressionValidator> -->
                                                <asp:Label ID="lblmassageSPPT" CssClass="labelMassage" runat="server"
                                                    Visible="False" Text="File uploaded successfully"></asp:Label>

                                            </td>

                                            <td>
                                                <asp:LinkButton ID="lbSPPT" ValidationGroup="Input" runat="server"
                                                    Text="Not Yet Uploaded" />
                                                <asp:Button ID="btnsaveSPPT" CssClass="bitbtndt btnadd" runat="server"
                                                    Text="View" Style="display: none" />
                                            </td>
                                        </tr>

                                        <tr>
                                            <td>
                                                <asp:checkbox id="cbSTTS" runat="server"
                                                    text=" STTS tanah 10 tahun terkahir berjalan"
                                                    autopostback="False" />
                                            </td>

                                            <td>
                                                <asp:FileUpload style="color: White;" runat="server"
                                                    accept="application/pdf" ID="FubSTTS" />
                                                <!-- <asp:RegularExpressionValidator ID="FileUpLoadValidator4" runat="server" ErrorMessage="Upload PDF files only .!!!"
                            ValidationExpression="^(([a-zA-Z]:)|(\\{2}\w+)\$?)(\\(\w[\w].*))(.pdf|.PDF)$" 
                           ControlToValidate="FubSTTS"> 
                        </asp:RegularExpressionValidator> -->
                                                <asp:Label ID="lblmassageSTTS" CssClass="labelMassage" runat="server"
                                                    Visible="False" Text="File uploaded successfully"></asp:Label>

                                            </td>

                                            <td>
                                                <asp:LinkButton ID="lbSTTS" ValidationGroup="Input" runat="server"
                                                    Text="Not Yet Uploaded" />
                                                <asp:Button ID="btnsaveSTTS" CssClass="bitbtndt btnadd" runat="server"
                                                    Text="View" Style="display: none" />
                                            </td>
                                        </tr>

                                        <tr>
                                            <td>
                                                <asp:checkbox id="cbTTD" runat="server"
                                                    text=" 2 buah Kuitansi Kosong bermaterai 6,000 dan ber TTD Penjual"
                                                    autopostback="False" />
                                            </td>

                                            <td>
                                                <asp:FileUpload style="color: White;" runat="server"
                                                    accept="application/pdf" ID="FubTTD" />
                                                <!--  <asp:RegularExpressionValidator ID="FileUpLoadValidator5" runat="server" ErrorMessage="Upload PDF files only .!!!"
                            ValidationExpression="^(([a-zA-Z]:)|(\\{2}\w+)\$?)(\\(\w[\w].*))(.pdf|.PDF)$" 
                           ControlToValidate="FubTTD"> 
                        </asp:RegularExpressionValidator>  -->
                                                <asp:Label ID="lblmassageTTD" CssClass="labelMassage" runat="server"
                                                    Visible="False" Text="File uploaded successfully"></asp:Label>
                                            </td>

                                            <td>
                                                <asp:LinkButton ID="lbTTD" ValidationGroup="Input" runat="server"
                                                    Text="Not Yet Uploaded" />
                                                <asp:Button ID="btnsaveTTD" CssClass="bitbtndt btnadd" runat="server"
                                                    Text="View" Style="display: none" />
                                            </td>
                                        </tr>


                                        <tr>
                                            <td>
                                                <asp:checkbox id="cbAJB" runat="server" text=" Asli AJB /SPH/ SHM"
                                                    autopostback="False" />
                                            </td>

                                            <td>
                                                <asp:FileUpload style="color: White;" runat="server"
                                                    accept="application/pdf" ID="FubAJB" />
                                                <!-- <asp:RegularExpressionValidator ID="FileUpLoadValidator6" runat="server" ErrorMessage="Upload PDF files only .!!!"
                            ValidationExpression="^(([a-zA-Z]:)|(\\{2}\w+)\$?)(\\(\w[\w].*))(.pdf|.PDF)$" 
                           ControlToValidate="FubAJB"> 
                        </asp:RegularExpressionValidator> -->
                                                <asp:Label ID="lblmassageAJB" CssClass="labelMassage" runat="server"
                                                    Visible="False" Text="File uploaded successfully"></asp:Label>
                                            </td>

                                            <td>

                                                <asp:LinkButton ID="lbAJB" ValidationGroup="Input" runat="server"
                                                    Text="Not Yet Uploaded" />
                                                <asp:Button ID="btnsaveAJB" CssClass="bitbtndt btnadd" runat="server"
                                                    Style="display: none" Text="View" />
                                            </td>
                                        </tr>

                                        <tr>
                                            <td>
                                                <asp:checkbox id="cbAJB2" runat="server"
                                                    text=" Asli AJB 2 (kalau nama penjual beda dengan nama dalam girik)"
                                                    autopostback="False" />
                                            </td>

                                            <td>
                                                <asp:FileUpload style="color: White;" runat="server"
                                                    accept="application/pdf" ID="FubAJB2" />
                                                <!-- <asp:RegularExpressionValidator ID="FileUpLoadValidator7" runat="server" ErrorMessage="Upload PDF files only .!!!"
                            ValidationExpression="^(([a-zA-Z]:)|(\\{2}\w+)\$?)(\\(\w[\w].*))(.pdf|.PDF)$" 
                           ControlToValidate="FubAJB2"> 
                        </asp:RegularExpressionValidator>  -->
                                                <asp:Label ID="lblmassageAJB2" CssClass="labelMassage" runat="server"
                                                    Visible="False" Text="File uploaded successfully"></asp:Label>
                                            </td>

                                            <td>
                                                <asp:LinkButton ID="lbAJB2" ValidationGroup="Input" runat="server"
                                                    Text="Not Yet Uploaded" />
                                                <asp:Button ID="btnsaveAJB2" CssClass="bitbtndt btnadd" runat="server"
                                                    Text="View" Style="display: none" />
                                            </td>
                                        </tr>

                                        <tr>
                                            <td>
                                                <asp:checkbox id="cbSSP" runat="server"
                                                    text=" Bukti Bayar & SSP Validasi (Pajak Penjual)"
                                                    autopostback="False" />
                                            </td>

                                            <td>
                                                <asp:FileUpload style="color: White;" runat="server"
                                                    accept="application/pdf" ID="FubSSP" />
                                                <!-- <asp:RegularExpressionValidator ID="FileUpLoadValidator8" runat="server" ErrorMessage="Upload PDF files only .!!!"
                            ValidationExpression="^(([a-zA-Z]:)|(\\{2}\w+)\$?)(\\(\w[\w].*))(.pdf|.PDF)$" 
                           ControlToValidate="FubSSP"> 
                        </asp:RegularExpressionValidator>  -->
                                                <asp:Label ID="lblmassageSSP" CssClass="labelMassage" runat="server"
                                                    Visible="False" Text="File uploaded successfully"></asp:Label>
                                            </td>

                                            <td>
                                                <asp:LinkButton ID="lbSSP" ValidationGroup="Input" runat="server"
                                                    Text="Not Yet Uploaded" />
                                                <asp:Button ID="btnsaveSSP" CssClass="bitbtndt btnadd" runat="server"
                                                    Text="View" Style="display: none" />
                                            </td>
                                        </tr>

                                        <tr>
                                            <td>
                                                <asp:checkbox id="cbSSD" runat="server"
                                                    text=" Bukti Bayar & SSPD-BPHTB lembar 3 (Pejak Pembeli)"
                                                    autopostback="False" />
                                            </td>

                                            <td>
                                                <asp:FileUpload style="color: White;" runat="server"
                                                    accept="application/pdf" ID="FubSSD" />
                                                <!-- <asp:RegularExpressionValidator ID="FileUpLoadValidator9" runat="server" ErrorMessage="Upload PDF files only .!!!"
                            ValidationExpression="^(([a-zA-Z]:)|(\\{2}\w+)\$?)(\\(\w[\w].*))(.pdf|.PDF)$" 
                           ControlToValidate="FubSSD"> 
                        </asp:RegularExpressionValidator> -->
                                                <asp:Label ID="lblmassageSSD" CssClass="labelMassage" runat="server"
                                                    Visible="False" Text="File uploaded successfully"></asp:Label>
                                            </td>

                                            <td>
                                                <asp:LinkButton ID="lbSSD" ValidationGroup="Input" runat="server"
                                                    Text="Not Yet Uploaded" />
                                                <asp:Button ID="btnsaveSSD" CssClass="bitbtndt btnadd" runat="server"
                                                    Text="View" Style="display: none" />
                                            </td>
                                        </tr>
                                        <!-- 10 -->
                                        <tr>
                                            <td>
                                                <asp:checkbox id="cbSKTS" runat="server"
                                                    text=" Surat Keterangan Tidak Sengketa" autopostback="False" />
                                            </td>

                                            <td>
                                                <asp:FileUpload style="color: White;" runat="server"
                                                    accept="application/pdf" ID="FubSKTS" />
                                                <!-- <asp:RegularExpressionValidator ID="FileUpLoadValidator10" runat="server" ErrorMessage="Upload PDF files only .!!!"
                            ValidationExpression="^(([a-zA-Z]:)|(\\{2}\w+)\$?)(\\(\w[\w].*))(.pdf|.PDF)$" 
                           ControlToValidate="FubSKTS"> 
                        </asp:RegularExpressionValidator> -->
                                                <asp:Label ID="lblmassageSKTS" CssClass="labelMassage" runat="server"
                                                    Visible="False" Text="File uploaded successfully"></asp:Label>
                                            </td>

                                            <td>
                                                <asp:LinkButton ID="lbSKTS" ValidationGroup="Input" runat="server"
                                                    Text="Not Yet Uploaded" />
                                                <asp:Button ID="btnsaveSKTS" CssClass="bitbtndt btnadd" runat="server"
                                                    Text="View" Style="display: none" />
                                            </td>
                                        </tr>



                                        <tr>
                                            <td>
                                                <asp:checkbox id="cbSPBT" runat="server"
                                                    text=" Surat Pernyataan Batas Tanah, Belum Bersetifikat dan Terima Luas"
                                                    autopostback="False" />
                                            </td>

                                            <td>
                                                <asp:FileUpload style="color: White;" runat="server"
                                                    accept="application/pdf" ID="FubSPBT" />
                                                <!-- <asp:RegularExpressionValidator ID="FileUpLoadValidator11" runat="server" ErrorMessage="Upload PDF files only .!!!"
                            ValidationExpression="^(([a-zA-Z]:)|(\\{2}\w+)\$?)(\\(\w[\w].*))(.pdf|.PDF)$" 
                           ControlToValidate="FubSPBT"> 
                        </asp:RegularExpressionValidator>  -->
                                                <asp:Label ID="lblmassageSPBT" CssClass="labelMassage" runat="server"
                                                    Visible="False" Text="File uploaded successfully"></asp:Label>
                                            </td>

                                            <td>
                                                <asp:LinkButton ID="lbSPBT" ValidationGroup="Input" runat="server"
                                                    Text="Not Yet Uploaded" />
                                                <asp:Button ID="btnsaveSPBT" CssClass="bitbtndt btnadd" runat="server"
                                                    Text="View" Style="display: none" />
                                            </td>
                                        </tr>


                                        <tr>
                                            <td>
                                                <asp:checkbox id="cbSPKTT" runat="server"
                                                    text=" Surat Pernyataan Kebenaran Tanda Tangan / Cap Jempol"
                                                    autopostback="False" />
                                            </td>

                                            <td>
                                                <asp:FileUpload style="color: White;" runat="server"
                                                    accept="application/pdf" ID="FubSPKTT" />
                                                <!-- <asp:RegularExpressionValidator ID="FileUpLoadValidator12" runat="server" ErrorMessage="Upload PDF files only .!!!"
                            ValidationExpression="^(([a-zA-Z]:)|(\\{2}\w+)\$?)(\\(\w[\w].*))(.pdf|.PDF)$" 
                           ControlToValidate="FubSPKTT"> 
                        </asp:RegularExpressionValidator>                                
                         -->
                                                <asp:Label ID="lblmassageSPKTT" CssClass="labelMassage" runat="server"
                                                    Visible="False" Text="File uploaded successfully"></asp:Label>
                                            </td>

                                            <td>
                                                <asp:LinkButton ID="lbSPKTT" ValidationGroup="Input" runat="server"
                                                    Text="Not Yet Uploaded" />
                                                <asp:Button ID="btnsaveSPKTT" CssClass="bitbtndt btnadd" runat="server"
                                                    Text="View" Style="display: none" />
                                            </td>
                                        </tr>

                                        <tr>
                                            <td>
                                                <asp:checkbox id="cbBAM" runat="server" text=" Berita Acara Menghadap"
                                                    autopostback="False" />
                                            </td>

                                            <td>
                                                <asp:FileUpload style="color: White;" runat="server"
                                                    accept="application/pdf" ID="FubBAM" />
                                                <!-- <asp:RegularExpressionValidator ID="FileUpLoadValidator13" runat="server" ErrorMessage="Upload PDF files only .!!!"
                            ValidationExpression="^(([a-zA-Z]:)|(\\{2}\w+)\$?)(\\(\w[\w].*))(.pdf|.PDF)$" 
                           ControlToValidate="FubBAM"> 
                        </asp:RegularExpressionValidator> -->
                                                <asp:Label ID="lblmassageBAM" CssClass="labelMassage" runat="server"
                                                    Visible="False" Text="File uploaded successfully"></asp:Label>

                                            </td>

                                            <td>
                                                <asp:LinkButton ID="lbBAM" ValidationGroup="Input" runat="server"
                                                    Text="Not Yet Uploaded" />
                                                <asp:Button ID="btnsaveBAM" CssClass="bitbtndt btnadd" runat="server"
                                                    Text="View" Style="display: none" />
                                            </td>
                                        </tr>

                                        <tr>
                                            <td>
                                                <asp:checkbox id="cbBAPL" runat="server"
                                                    text=" Berita Acara Pemeriksaan Lokasi dari Desa"
                                                    autopostback="False" />
                                            </td>

                                            <td>
                                                <asp:FileUpload style="color: White;" runat="server"
                                                    accept="application/pdf" ID="FubBAPL" />
                                                <!-- <asp:RegularExpressionValidator ID="FileUpLoadValidator14" runat="server" ErrorMessage="Upload PDF files only .!!!"
                            ValidationExpression="^(([a-zA-Z]:)|(\\{2}\w+)\$?)(\\(\w[\w].*))(.pdf|.PDF)$" 
                           ControlToValidate="FubBAPL"> 
                        </asp:RegularExpressionValidator>  -->
                                                <asp:Label ID="lblmassageBAPL" CssClass="labelMassage" runat="server"
                                                    Visible="False" Text="File uploaded successfully"></asp:Label>
                                            </td>

                                            <td>
                                                <asp:LinkButton ID="lbBAPL" ValidationGroup="Input" runat="server"
                                                    Text="Not Yet Uploaded" />
                                                <asp:Button ID="btnsaveBAPL" CssClass="bitbtndt btnadd" runat="server"
                                                    Text="View" Style="display: none" />
                                            </td>
                                        </tr>


                                        <tr>
                                            <td>
                                                <asp:checkbox id="cbPTPP" runat="server"
                                                    text=" Denah / Peta Tanah Pembeli dari Pihak IGL (Pak Hanif)"
                                                    autopostback="False" />
                                            </td>

                                            <td>
                                                <asp:FileUpload style="color: White;" runat="server"
                                                    accept="application/pdf" ID="FubPTPP" />
                                                <!-- <asp:RegularExpressionValidator ID="FileUpLoadValidator15" runat="server" ErrorMessage="Upload PDF files only .!!!"
                            ValidationExpression="^(([a-zA-Z]:)|(\\{2}\w+)\$?)(\\(\w[\w].*))(.pdf|.PDF)$" 
                           ControlToValidate="FubPTPP"> 
                        </asp:RegularExpressionValidator>  -->
                                                <asp:Label ID="lblmassagePTPP" CssClass="labelMassage" runat="server"
                                                    Visible="False" Text="File uploaded successfully"></asp:Label>
                                            </td>

                                            <td>
                                                <asp:LinkButton ID="lbPTPP" ValidationGroup="Input" runat="server"
                                                    Text="Not Yet Uploaded" />
                                                <asp:Button ID="btnsavePTPP" CssClass="bitbtndt btnadd" runat="server"
                                                    Text="View" Style="display: none" />
                                            </td>
                                        </tr>

                                    </table>
                                </div>
                            </asp:View>



                            <asp:View ID="TabW" runat="server">
                                <div class="container-fluid mt-1">
                                    <table>

                                        <tr>
                                            <td>
                                                <asp:checkbox id="cbPTPP2" runat="server"
                                                    text=" Denah / Peta Tanah Pembeli dari Pihak IGL terhadap Ijin Kawasan / Ijin Lokasi (Pak Hanif)"
                                                    autopostback="False" />
                                            </td>

                                            <td>
                                                <asp:FileUpload style="color: White;" runat="server"
                                                    accept="application/pdf" ID="FubPTPP2" />
                                                <!-- <asp:RegularExpressionValidator ID="FileUpLoadValidator16" runat="server" ErrorMessage="Upload PDF files only .!!!"
                            ValidationExpression="^(([a-zA-Z]:)|(\\{2}\w+)\$?)(\\(\w[\w].*))(.pdf|.PDF)$" 
                           ControlToValidate="FubPTPP2"> 
                        </asp:RegularExpressionValidator>  -->
                                                <asp:Label ID="lblmassagePTPP2" CssClass="labelMassage" runat="server"
                                                    Visible="False" Text="File uploaded successfully"></asp:Label>
                                            </td>

                                            <td>
                                                <asp:LinkButton ID="lbPtPP2" ValidationGroup="Input" runat="server"
                                                    Text="Not Yet Uploaded" />
                                                <asp:Button ID="btnsavePTPP2" CssClass="bitbtndt btnadd" runat="server"
                                                    Text="View" Style="display: none" />
                                            </td>
                                        </tr>


                                        <tr>
                                            <td>
                                                <asp:checkbox id="cbSKRT" runat="server"
                                                    text=" Surat Keterangan Riwayat Tanah dari Desa"
                                                    autopostback="False" />
                                            </td>

                                            <td>
                                                <asp:FileUpload style="color: White;" runat="server"
                                                    accept="application/pdf" ID="FubSKRT" />
                                                <!-- <asp:RegularExpressionValidator ID="FileUpLoadValidator17" runat="server" ErrorMessage="Upload PDF files only .!!!"
                            ValidationExpression="^(([a-zA-Z]:)|(\\{2}\w+)\$?)(\\(\w[\w].*))(.pdf|.PDF)$" 
                           ControlToValidate="FubSKRT"> 
                        </asp:RegularExpressionValidator>  -->
                                                <asp:Label ID="lblmassageSKRT" CssClass="labelMassage" runat="server"
                                                    Visible="False" Text="File uploaded successfully"></asp:Label>
                                            </td>

                                            <td>
                                                <asp:LinkButton ID="lbSKRT" ValidationGroup="Input" runat="server"
                                                    Text="Not Yet Uploaded" />
                                                <asp:Button ID="btnsaveSKRT" CssClass="bitbtndt btnadd" runat="server"
                                                    Text="View" Style="display: none" />
                                            </td>
                                        </tr>


                                        <tr>
                                            <td>
                                                <asp:checkbox id="cbSKD" runat="server"
                                                    text=" Surat Keterangan Desa / Tanah" autopostback="False" />
                                            </td>

                                            <td>
                                                <asp:FileUpload style="color: White;" runat="server"
                                                    accept="application/pdf" ID="FubSKD" />
                                                <!-- <asp:RegularExpressionValidator ID="FileUpLoadValidator18" runat="server" ErrorMessage="Upload PDF files only .!!!"
                            ValidationExpression="^(([a-zA-Z]:)|(\\{2}\w+)\$?)(\\(\w[\w].*))(.pdf|.PDF)$" 
                           ControlToValidate="FubSKD"> 
                        </asp:RegularExpressionValidator> -->
                                                <asp:Label ID="lblmassageSKD" CssClass="labelMassage" runat="server"
                                                    Visible="False" Text="File uploaded successfully"></asp:Label>
                                            </td>

                                            <td>
                                                <asp:LinkButton ID="lbSKD" ValidationGroup="Input" runat="server"
                                                    Text="Not Yet Uploaded" />
                                                <asp:Button ID="btnsaveSKD" CssClass="bitbtndt btnadd" runat="server"
                                                    Text="View" Style="display: none" />
                                            </td>
                                        </tr>



                                        <tr>
                                            <td>
                                                <asp:checkbox id="cbFCGirik" runat="server"
                                                    text=" Fc. Girik/Buku C Desa yang dilegalisir Kelurahan"
                                                    autopostback="False" />
                                            </td>

                                            <td>
                                                <asp:FileUpload style="color: White;" runat="server"
                                                    accept="application/pdf" ID="FubFCGirik" />
                                                <!--  <asp:RegularExpressionValidator ID="FileUpLoadValidator19" runat="server" ErrorMessage="Upload PDF files only .!!!"
                            ValidationExpression="^(([a-zA-Z]:)|(\\{2}\w+)\$?)(\\(\w[\w].*))(.pdf|.PDF)$" 
                           ControlToValidate="FubFCGirik"> 
                        </asp:RegularExpressionValidator> -->
                                                <asp:Label ID="lblmassageFcGirik" CssClass="labelMassage" runat="server"
                                                    Visible="False" Text="File uploaded successfully"></asp:Label>
                                            </td>

                                            <td>
                                                <asp:LinkButton ID="lbFCGirik" ValidationGroup="Input" runat="server"
                                                    Text="Not Yet Uploaded" />
                                                <asp:Button ID="btnsaveFCGirik" CssClass="bitbtndt btnadd"
                                                    runat="server" Text="View" Style="display: none" />
                                            </td>
                                        </tr>


                                        <tr>
                                            <td>
                                                <asp:checkbox id="cbFDP" runat="server"
                                                    text=" Foto Dokumentasi Penjual  (Pelunasan)"
                                                    autopostback="False" />
                                            </td>

                                            <td>
                                                <asp:FileUpload style="color: White;" runat="server"
                                                    accept="application/pdf" ID="FubFDP" />
                                                <!--  <asp:RegularExpressionValidator ID="FileUpLoadValidator20" runat="server" ErrorMessage="Upload PDF files only .!!!"
                            ValidationExpression="^(([a-zA-Z]:)|(\\{2}\w+)\$?)(\\(\w[\w].*))(.pdf|.PDF)$" 
                           ControlToValidate="FubFDP"> 
                        </asp:RegularExpressionValidator>   -->
                                                <asp:Label ID="lblmassageFDP" CssClass="labelMassage" runat="server"
                                                    Visible="False" Text="File uploaded uccessfully"></asp:Label>
                                            </td>

                                            <td>
                                                <asp:LinkButton ID="lbFDP" ValidationGroup="Input" runat="server"
                                                    Text="Not Yet Uploaded" />
                                                <asp:Button ID="btnsaveFDP" CssClass="bitbtndt btnadd" runat="server"
                                                    Text="View" Style="display: none" />
                                            </td>
                                        </tr>


                                        <tr>
                                            <td>
                                                <asp:checkbox id="cbPatok" runat="server"
                                                    text=" Foto Batas Tanah (Patok)" autopostback="False" />
                                            </td>

                                            <td>
                                                <asp:FileUpload style="color: White;" runat="server"
                                                    accept="application/pdf" ID="FubPatok" />
                                                <!-- <asp:RegularExpressionValidator ID="FileUpLoadValidator21" runat="server" ErrorMessage="Upload PDF files only .!!!"
                            ValidationExpression="^(([a-zA-Z]:)|(\\{2}\w+)\$?)(\\(\w[\w].*))(.pdf|.PDF)$" 
                           ControlToValidate="FubPatok"> 
                        </asp:RegularExpressionValidator> -->
                                                <asp:Label ID="lblmassagePatok" CssClass="labelMassage" runat="server"
                                                    Visible="False" Text="File uploaded uccessfully">
                                                </asp:Label>
                                            </td>

                                            <td>
                                                <asp:LinkButton ID="lbPatok" ValidationGroup="Input" runat="server"
                                                    Text="Not Yet Uploaded" />
                                                <asp:Button ID="btnsavePatok" CssClass="bitbtndt btnadd" runat="server"
                                                    Text="View" Style="display: none" />
                                            </td>
                                        </tr>

                                        <tr>
                                            <td>
                                                <asp:checkbox id="cbSporadik" runat="server"
                                                    text=" Surat Penguasaan Fisik Bidang Tanah (Sporadik)"
                                                    autopostback="False" />
                                            </td>

                                            <td>
                                                <asp:FileUpload style="color: White;" runat="server"
                                                    accept="application/pdf" ID="FubSporadik" />
                                                <!-- <asp:RegularExpressionValidator ID="FileUpLoadValidator22" runat="server" ErrorMessage="Upload PDF files only .!!!"
                            ValidationExpression="^(([a-zA-Z]:)|(\\{2}\w+)\$?)(\\(\w[\w].*))(.pdf|.PDF)$" 
                           ControlToValidate="FubSporadik"> 
                        </asp:RegularExpressionValidator>  -->
                                                <asp:Label ID="lblmassageSporadik" CssClass="labelMassage"
                                                    runat="server" Visible="False" Text="File uploaded uccessfully">
                                                </asp:Label>
                                            </td>

                                            <td>
                                                <asp:LinkButton ID="lbSporadik" ValidationGroup="Input" runat="server"
                                                    Text="Not Yet Uploaded" />
                                                <asp:Button ID="btnsaveSporadik" CssClass="bitbtndt btnadd"
                                                    runat="server" Text="View" Style="display: none" />
                                            </td>
                                        </tr>


                                        <tr>
                                            <td>
                                                <asp:checkbox id="cbAHU" runat="server" text=" Asli Hasil Ukur"
                                                    autopostback="False" />
                                            </td>

                                            <td>
                                                <asp:FileUpload style="color: White;" runat="server"
                                                    accept="application/pdf" ID="FubAHU" />
                                                <!-- <asp:RegularExpressionValidator ID="FileUpLoadValidator23" runat="server" ErrorMessage="Upload PDF files only .!!!"
                            ValidationExpression="^(([a-zA-Z]:)|(\\{2}\w+)\$?)(\\(\w[\w].*))(.pdf|.PDF)$" 
                           ControlToValidate="FubAHU"> 
                        </asp:RegularExpressionValidator>  -->
                                                <asp:Label ID="lblmassageAHU" CssClass="labelMassage" runat="server"
                                                    Visible="False" Text="File uploaded uccessfully"></asp:Label>
                                            </td>

                                            <td>
                                                <asp:LinkButton ID="lbAHU" ValidationGroup="Input" runat="server"
                                                    Text="Not Yet Uploaded" />
                                                <asp:Button ID="btnsaveAHU" CssClass="bitbtndt btnadd" runat="server"
                                                    Text="View" Style="display: none" />
                                            </td>
                                        </tr>


                                        <tr>
                                            <td>
                                                <asp:checkbox id="cbSejarah" runat="server"
                                                    text=" Sejarah / Riwayat Tanah" autopostback="False" />
                                            </td>

                                            <td>
                                                <asp:FileUpload style="color: White;" runat="server"
                                                    accept="application/pdf" ID="FubSejarah" />
                                                <!-- <asp:RegularExpressionValidator ID="FileUpLoadValidator24" runat="server" ErrorMessage="Upload PDF files only .!!!"
                            ValidationExpression="^(([a-zA-Z]:)|(\\{2}\w+)\$?)(\\(\w[\w].*))(.pdf|.PDF)$" 
                           ControlToValidate="FubSejarah"> 
                        </asp:RegularExpressionValidator>  -->
                                                <asp:Label ID="lblmassageSejarah" CssClass="labelMassage" runat="server"
                                                    Visible="False" Text="File uploaded uccessfully">
                                                </asp:Label>
                                            </td>

                                            <td>
                                                <asp:LinkButton ID="lbSejarah" ValidationGroup="Input" runat="server"
                                                    Text="Not Yet Uploaded" />
                                                <asp:Button ID="btnsaveSejarah" CssClass="bitbtndt btnadd"
                                                    runat="server" Text="View" Style="display: none" />
                                            </td>
                                        </tr>


                                        <tr>
                                            <td>
                                                <asp:checkbox id="cbSPJH" runat="server"
                                                    text=" Surat Pernyataan Jual Hasi/ Seluruhnya"
                                                    autopostback="False" />
                                            </td>

                                            <td>
                                                <asp:FileUpload style="color: White;" runat="server"
                                                    accept="application/pdf" ID="FubSPJH" />
                                                <!-- <asp:RegularExpressionValidator ID="FileUpLoadValidator25" runat="server" ErrorMessage="Upload PDF files only .!!!"
                            ValidationExpression="^(([a-zA-Z]:)|(\\{2}\w+)\$?)(\\(\w[\w].*))(.pdf|.PDF)$" 
                           ControlToValidate="FubSPJH"> 
                        </asp:RegularExpressionValidator> -->
                                                <asp:Label ID="lblmassageSPJH" CssClass="labelMassage" runat="server"
                                                    Visible="False" Text="File uploaded uccessfully">
                                                </asp:Label>
                                            </td>

                                            <td>
                                                <asp:LinkButton ID="lbSPJH" ValidationGroup="Input" runat="server"
                                                    Text="Not Yet Uploaded" />
                                                <asp:Button ID="btnsaveSPJH" CssClass="bitbtndt btnadd" runat="server"
                                                    Text="View" Style="display: none" />
                                            </td>
                                        </tr>


                                        <tr>
                                            <td>
                                                <asp:checkbox id="cbLainLain" runat="server" autopostback="False" />
                                                <asp:TextBox runat="server" ValidationGroup="Input" ID="tbLainLain"
                                                    CssClass="TextBox" Height="20px" Width="225px"
                                                    AutoPostBack="True" />
                                            </td>


                                            <td>
                                                <asp:FileUpload style="color: White;" runat="server"
                                                    accept="application/pdf" ID="FubLainLain" />
                                                <asp:Label ID="lblmassageLainLain" CssClass="labelMassage"
                                                    runat="server" Visible="False" Text="File uploaded uccessfully">
                                                </asp:Label>
                                            </td>

                                            <td>
                                                <asp:LinkButton ID="lbLainLain" ValidationGroup="Input" runat="server"
                                                    Text="Not Yet Uploaded" />
                                                <asp:Button ID="btnsaveLainLain" CssClass="bitbtndt btnadd"
                                                    runat="server" Text="View" Style="display: none" />
                                            </td>
                                        </tr>

                                        <tr>
                                            <td>
                                                <asp:checkbox id="cbLainLain2" runat="server" autopostback="False" />
                                                <asp:TextBox runat="server" ValidationGroup="Input" ID="tbLainLain2"
                                                    CssClass="TextBox" Height="20px" Width="225px"
                                                    AutoPostBack="True" />
                                            </td>


                                            <td>
                                                <asp:FileUpload style="color: White;" runat="server"
                                                    accept="application/pdf" ID="FubLainLain2" />
                                                <asp:Label ID="lblmassageLainLain2" CssClass="labelMassage"
                                                    runat="server" Visible="False" Text="File uploaded uccessfully">
                                                </asp:Label>
                                            </td>

                                            <td>
                                                <asp:LinkButton ID="lbLainLain2" ValidationGroup="Input" runat="server"
                                                    Text="Not Yet Uploaded" />
                                                <asp:Button ID="btnsaveLainLain2" CssClass="bitbtndt btnadd"
                                                    runat="server" Text="View" Style="display: none" />
                                            </td>
                                        </tr>

                                        <tr>
                                            <td>
                                                <asp:checkbox id="cbLainLain3" runat="server" autopostback="False" />
                                                <asp:TextBox runat="server" ValidationGroup="Input" ID="tbLainLain3"
                                                    CssClass="TextBox" Height="20px" AutoPostBack="True"
                                                    Width="225px" />
                                            </td>


                                            <td>
                                                <asp:FileUpload style="color: White;" runat="server"
                                                    accept="application/pdf" ID="FubLainLain3" />
                                                <asp:Label ID="lblmassageLainLain3" CssClass="labelMassage"
                                                    runat="server" Visible="False" Text="File uploaded uccessfully">
                                                </asp:Label>
                                            </td>

                                            <td>
                                                <asp:LinkButton ID="lbLainLain3" ValidationGroup="Input" runat="server"
                                                    Text="Not Yet Uploaded" />
                                                <asp:Button ID="btnsaveLainLain3" CssClass="bitbtndt btnadd"
                                                    runat="server" Text="View" Style="display: none" />
                                            </td>
                                        </tr>

                                        <tr>
                                            <td>
                                                <h4>Jika Dokumen Tanah Waris</h4>
                                            </td>
                                        </tr>


                                        <tr>
                                            <td>
                                                <asp:checkbox id="cbKtpW" Enabled="True" runat="server"
                                                    text=" KTP Waris" autopostback="False" />
                                            </td>

                                            <td>
                                                <asp:FileUpload style="color: White;" runat="server"
                                                    accept="application/pdf" ID="FupKTPW" />
                                                <!-- <asp:RegularExpressionValidator ID="FileUpLoadValidator27" runat="server" ErrorMessage="Upload PDF files only .!!!"
                            ValidationExpression="^(([a-zA-Z]:)|(\\{2}\w+)\$?)(\\(\w[\w].*))(.pdf|.PDF)$" 
                           ControlToValidate="FupKTPW"> 
                        </asp:RegularExpressionValidator> -->
                                                <asp:Label ID="lblmassageKTPW" CssClass="labelMassage" runat="server"
                                                    Visible="False" Text="File uploaded uccessfully">
                                                </asp:Label>
                                            </td>

                                            <td>
                                                <asp:LinkButton ID="lbKtpW" ValidationGroup="Input" runat="server"
                                                    Text="Not Yet Uploaded" />
                                                <asp:Button ID="btnsaveKtpW" CssClass="bitbtndt btnadd" runat="server"
                                                    Style="display: none" Text="View" />
                                            </td>

                                        </tr>

                                        <tr>
                                            <td>
                                                <asp:checkbox id="cbSPW" runat="server" text=" Surat Pernyataan Waris"
                                                    autopostback="False" />
                                            </td>

                                            <td>
                                                <asp:FileUpload style="color: White;" runat="server"
                                                    accept="application/pdf" ID="FubSPW" />
                                                <!-- <asp:RegularExpressionValidator ID="FileUpLoadValidator28" runat="server" ErrorMessage="Upload PDF files only .!!!"
                            ValidationExpression="^(([a-zA-Z]:)|(\\{2}\w+)\$?)(\\(\w[\w].*))(.pdf|.PDF)$" 
                           ControlToValidate="FubSPW"> 
                        </asp:RegularExpressionValidator> -->
                                                <asp:Label ID="lblmassageSPW" CssClass="labelMassage" runat="server"
                                                    Visible="False" Text="File uploaded uccessfully"></asp:Label>
                                            </td>

                                            <td>
                                                <asp:LinkButton ID="lbSPW" ValidationGroup="Input" runat="server"
                                                    Text="Not Yet Uploaded" />
                                                <asp:Button ID="btnsaveSPW" CssClass="bitbtndt btnadd" runat="server"
                                                    Text="View" Style="display: none" />
                                            </td>
                                        </tr>

                                        <tr>
                                            <td>
                                                <asp:checkbox id="cbSPKW" runat="server"
                                                    text=" Surat Pernyataan Kuasa Waris" autopostback="False" />
                                            </td>

                                            <td>
                                                <asp:FileUpload style="color: White;" runat="server"
                                                    accept="application/pdf" ID="FubSPKW" />
                                                <!-- <asp:RegularExpressionValidator ID="FileUpLoadValidator29" runat="server" ErrorMessage="Upload PDF files only .!!!"
                            ValidationExpression="^(([a-zA-Z]:)|(\\{2}\w+)\$?)(\\(\w[\w].*))(.pdf|.PDF)$" 
                           ControlToValidate="FubSPKW"> 
                        </asp:RegularExpressionValidator>  -->
                                                <asp:Label ID="lblmassageSPKW" CssClass="labelMassage" runat="server"
                                                    Visible="False" Text="File uploaded uccessfully">
                                                </asp:Label>
                                            </td>

                                            <td>
                                                <asp:LinkButton ID="lbSPKW" ValidationGroup="Input" runat="server"
                                                    Text="Not Yet Uploaded" />
                                                <asp:Button ID="btnsaveSPKW" CssClass="bitbtndt btnadd" runat="server"
                                                    Text="View" Style="display: none" />
                                            </td>
                                        </tr>


                                    </table>
                                </div>
                            </asp:View>

                            </asp:MultiView>
                            <div class="row align-items-center mb-1">
                                <div class="col-md-12">
                                    <div class="menu-tabs-wrapper mt-0 pt-0">
                                        <asp:Menu ID="Menu1" runat="server" Orientation="Horizontal"
                                            StaticEnableDefaultPopOutImage="False"
                                            StaticMenuItemStyle-CssClass="MenuItem"
                                            StaticSelectedStyle-CssClass="MenuSelect">
                                            <Items>
                                                <asp:MenuItem Text="Detail Riwayat Tanah" Value="0"></asp:MenuItem>
                                                <asp:MenuItem Text="Detail Monitoring Letter C" Value="2">
                                                </asp:MenuItem>
                                                <%--<asp:MenuItem Text="Equipment" Value="1"></asp:MenuItem>--%>
                                                    <%--<asp:MenuItem Text="Schedule Job Detail" Value="3">
                                                        </asp:MenuItem>
                                                        --%>
                                            </Items>
                                        </asp:Menu>




                                    </div>
                                </div>
                            </div>

                            <asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex="0">

                                <asp:View ID="Tab2" runat="server">

                                    <asp:Panel ID="pnlDt" runat="server">
                                        <div class="card shadow-sm">
                                            <div class="card-body p-1">
                                                <div class="row mb-0">
                                                    <div class="col-md-12 mb-1 text-end">
                                                        <asp:Button ID="BtnAddDt" runat="server"
                                                            text="Tambah Data Riwayat Tanah" ValidationGroup="Input"
                                                            CssClass="btn btn-primary btn-sm w-100 btn-icon btnadd">

                                                        </asp:Button>
                                                    </div>
                                                </div>

                                                <!-- wrapper biar tabel bisa scroll kanan & vertikal -->
                                                <div class="table-responsive"
                                                    style="max-height:600px; overflow-y:auto;">
                                                    <asp:GridView ID="GridDt" runat="server" ShowFooter="false"
                                                        AllowSorting="True" AutoGenerateColumns="False"
                                                        AllowPaging="True" PageSize="10"
                                                        CssClass="table table-striped table-bordered table-hover table-sm  text-nowrap table-soft-text">
                                                        <HeaderStyle CssClass="table-soft-dark text-white sticky-top" />
                                                        <FooterStyle CssClass="table-light" />
                                                        <PagerStyle CssClass="grid-pager text-center bg-light"
                                                            HorizontalAlign="Left" />

                                                        <Columns>
                                                            <asp:TemplateField HeaderText="Action">
                                                                <ItemTemplate>


                                                                    <asp:LinkButton ID="btnEdit" runat="server"
                                                                        CssClass="btn btn-primary btn-sm "
                                                                        CommandName="Edit">
                                                                        <i class="fa fa-edit me-1"></i> Edit
                                                                    </asp:LinkButton>

                                                                    <asp:LinkButton ID="btnDelete" runat="server"
                                                                        CssClass="btn btn-danger btn-sm "
                                                                        CommandName="delete"
                                                                        OnClientClick="return confirm('Sure to delete this data?');">
                                                                        <i class="fa fa-trash me-1"></i> Delete
                                                                    </asp:LinkButton>
                                                                </ItemTemplate>
                                                                <EditItemTemplate>
                                                                    <asp:Button class="bitbtndt btnsave" runat="server"
                                                                        ID="btnUpdate" Text="Save"
                                                                        CommandName="Update" />
                                                                    <asp:Button class="bitbtndt btncancel"
                                                                        runat="server" ID="btnCancel" Text="Cancel"
                                                                        CommandName="Cancel" />
                                                                </EditItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="ItemNo" HeaderStyle-Width="150px"
                                                                HeaderText="Item No" />
                                                            <asp:BoundField DataField="KetKegiatan"
                                                                HeaderStyle-Width="150px" HeaderText="Ket Kegiatan" />
                                                            <asp:BoundField DataField="NoSurat"
                                                                HeaderStyle-Width="150px" HeaderText="No Surat" />
                                                            <asp:BoundField DataField="DateSurat"
                                                                DataFormatString="{0:dd MMM yyyy}" HtmlEncode="true"
                                                                HeaderStyle-Width="80px" SortExpression="DateSurat"
                                                                HeaderText="DateSurat">
                                                                <HeaderStyle Width="80px" />
                                                            </asp:BoundField>

                                                            <asp:BoundField DataField="Luas"
                                                                DataFormatString="{0:#,##0.##}"
                                                                HeaderStyle-Width="150px" HeaderText="Luas" />
                                                            <asp:BoundField DataField="NameAwal"
                                                                HeaderStyle-Width="150px" HeaderText="Atas Nama Awal" />

                                                            <asp:BoundField DataField="NameAkhir"
                                                                HeaderStyle-Width="150px"
                                                                HeaderText="Atas Nama Akhir" />
                                                            <asp:BoundField DataField="Remark" HeaderStyle-Width="150px"
                                                                HeaderText="Remark" />

                                                        </Columns>

                                                    </asp:GridView>
                                                </div>

                                                <!-- </div> -->
                                                <asp:Button class="bitbtn btnadd" visible="false" runat="server"
                                                    ID="btnAddDtKe2" Text="Add" ValidationGroup="Input" />
                                            </div>
                                        </div>
                                    </asp:Panel>

                                    <asp:Panel runat="server" ID="pnlEditDt" Visible="false">
                                        <div class="card shadow-sm">
                                            <div class="card-body p-1">

                                                <div class="container-fluid mt-2">
                                                    <div class="row mb-1">
                                                        <!-- Offering Survey No -->
                                                        <div class="col-md-3">
                                                            <label class="form-label">Keterangan Kegiatan Item No :
                                                                <asp:Label class="form-label" ID="lbItemNo"
                                                                    runat="server" Text="" />
                                                            </label>
                                                            <asp:TextBox ID="tbKetKegiatan" runat="server"
                                                                CssClass="form-control form-control-sm"></asp:TextBox>
                                                        </div>

                                                        <div class="col-md-3">
                                                            <label class="form-label">No Surat</label>
                                                            <asp:TextBox ID="tbNoSurat" runat="server"
                                                                CssClass="form-control form-control-sm"></asp:TextBox>
                                                        </div>

                                                        <div class="col-md-3">
                                                            <label class="form-label">Tanggal Surat</label>
                                                            <BDP:BasicDatePicker ID="tbDateSurat" runat="server"
                                                                DateFormat="dd MMM yyyy" ReadOnly="True"
                                                                ValidationGroup="Input" ButtonImageHeight="19px"
                                                                ButtonImageWidth="20px" DisplayType="TextBox"
                                                                AutoPostBack="True" ShowNoneButton="False">
                                                                <TextBoxStyle CssClass="form-control form-control-sm" />
                                                            </BDP:BasicDatePicker>
                                                        </div>


                                                        <div class="col-md-3">
                                                            <label class="form-label">Luas</label>
                                                            <asp:TextBox ID="tbLuasDt" runat="server"
                                                                CssClass="form-control form-control-sm"></asp:TextBox>
                                                        </div>
                                                    </div>

                                                    <div class="row mb-1">
                                                        <!-- Offering Survey No -->
                                                        <div class="col-md-3">
                                                            <label class="form-label">Nama Pemilik Awal</label>
                                                            <asp:TextBox ID="tbPemilikAwal" runat="server"
                                                                CssClass="form-control form-control-sm"></asp:TextBox>
                                                        </div>

                                                        <div class="col-md-3">
                                                            <label class="form-label">Nama Pemilik Akhir</label>
                                                            <asp:TextBox ID="tbPemilikAkhir" runat="server"
                                                                CssClass="form-control form-control-sm"></asp:TextBox>
                                                        </div>

                                                        <div class="col-md-6">
                                                            <label class="form-label">Remark</label>
                                                            <asp:TextBox ID="tbRemarkDt" runat="server"
                                                                CssClass="form-control form-control-sm"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <br>

                                                    <div class="row-mb-1">

                                                        <div class="col-md-12 text-end">
                                                            <asp:LinkButton ID="btnSaveDt" runat="server"
                                                                CssClass="btn btn-primary btn-sm">
                                                                <i class="fa fa-edit me-1"></i> Save
                                                            </asp:LinkButton>

                                                            <asp:LinkButton ID="btnCancelDt" runat="server"
                                                                CssClass="btn btn-danger btn-sm">
                                                                <i class="fa fa-arrow-left me-1"></i> Cancel
                                                            </asp:LinkButton>
                                                        </div>

                                                    </div>

                                                </div>
                                                <br>
                                            </div>
                                        </div>
                                    </asp:Panel>

                                </asp:View>

                                <asp:View ID="Tab3" runat="server">

                                    <asp:Panel ID="pnlDt2" runat="server">
                                        <asp:Button class="bitbtn btnadd" runat="server" ID="btnAddDt2" Text="Add"
                                            ValidationGroup="Input" />

                                        <div style="border: 0px  solid; width: 100%; height: 100%; overflow: auto;">
                                            <asp:GridView ID="GridDt2" runat="server" AutoGenerateColumns="False"
                                                ShowFooter="True">
                                                <HeaderStyle CssClass="GridHeader" />
                                                <RowStyle CssClass="GridItem" Wrap="false" />
                                                <AlternatingRowStyle CssClass="GridAltItem" />
                                                <PagerStyle CssClass="GridPager" />
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Action">
                                                        <ItemTemplate>
                                                            <asp:Button ID="btnEdit" runat="server"
                                                                class="bitbtndt btnedit" CommandName="Edit"
                                                                Text="Edit" />
                                                            <asp:Button ID="btnDelete" runat="server"
                                                                class="bitbtndt btndelete" CommandName="Delete"
                                                                OnClientClick="return confirm('Sure to delete this data?');"
                                                                Text="Delete" />
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <asp:Button ID="btnUpdate" runat="server"
                                                                class="bitbtndt btnsave" CommandName="Update"
                                                                Text="Save" />
                                                            <asp:Button ID="btnCancel" runat="server"
                                                                class="bitbtndt btncancel" CommandName="Cancel"
                                                                Text="Cancel" />
                                                        </EditItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="Equipment" HeaderText="Equipment"
                                                        SortExpression="Equipment">
                                                        <HeaderStyle Width="150px" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="EquipmentName" HeaderStyle-Width="150px"
                                                        HeaderText="Equipment Name" SortExpression="EquipmentName">
                                                        <HeaderStyle Width="200px" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="Qty" HeaderStyle-Width="60px"
                                                        HeaderText="Qty" DataFormatString="{0:#,##0.##}"
                                                        ItemStyle-HorizontalAlign="Right" SortExpression="Qty">
                                                        <HeaderStyle Width="60px" />
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="Unit" HeaderStyle-Width="60px"
                                                        HeaderText="Unit" ItemStyle-HorizontalAlign="Left"
                                                        SortExpression="Unit">
                                                        <HeaderStyle Width="60px" />
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="Remark" HeaderStyle-Width="150px"
                                                        HeaderText="Remark" SortExpression="Remark">
                                                        <HeaderStyle Width="200px" />
                                                    </asp:BoundField>

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
                                                    <asp:TextBox ID="tbEquip" runat="server" AutoPostBack="true"
                                                        CssClass="TextBox" MaxLength="20" ValidationGroup="Input" />
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
                                                    <asp:TextBox ID="tbQty" runat="server" CssClass="TextBox"
                                                        Width="65px" />
                                                    <asp:Label ID="lblUnit" runat="server" ForeColor="#FF3300" Text="*">
                                                    </asp:Label>
                                                </td>
                                            <tr>
                                                <td>
                                                    Remark
                                                </td>
                                                <td>
                                                    :
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="tbremarkEquip" runat="server"
                                                        CssClass="TextBoxMulti" MaxLength="255" TextMode="MultiLine"
                                                        ValidationGroup="Input" Width="350px" />
                                                    &nbsp; &nbsp; &nbsp;
                                                </td>
                                            </tr>
                                            </tr>
                                            </tr>
                                        </table>
                                        <br />
                                        <asp:Button ID="btnSaveDt2" runat="server" class="bitbtndt btnsave"
                                            Text="Save" />
                                        <asp:Button ID="btnCancelDt2" runat="server" class="bitbtndt btncancel"
                                            Text="Cancel" />
                                    </asp:Panel>
                                </asp:View>

                                <asp:View ID="Tab4" runat="server">
                                    <asp:Panel ID="pnlDt3" runat="server">
                                        <div class="card shadow-sm">
                                            <div class="card-body p-1">
                                                <div class="row mb-0">
                                                    <div class="col-md-12 mb-1 text-end">
                                                        <asp:Button ID="BtnAddDt3" runat="server"
                                                            text="Tambah Data Detail Monitoring" ValidationGroup="Input"
                                                            CssClass="btn btn-primary btn-sm px-10 btn-icon btnadd w-100">

                                                        </asp:Button>
                                                    </div>
                                                </div>

                                                <div class="table-responsive"
                                                    style="max-height:600px; overflow-y:auto;">
                                                    <asp:GridView ID="GridDt3" runat="server" ShowFooter="false"
                                                        AllowSorting="True" AutoGenerateColumns="False"
                                                        AllowPaging="True" PageSize="10"
                                                        CssClass="table table-striped table-bordered table-hover table-sm  text-nowrap table-soft-text">
                                                        <HeaderStyle CssClass="table-soft-dark text-white sticky-top" />
                                                        <FooterStyle CssClass="table-light" />
                                                        <PagerStyle CssClass="grid-pager text-center bg-light"
                                                            HorizontalAlign="Left" />
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="Action">
                                                                <ItemTemplate>

                                                                    <asp:LinkButton ID="btnEdit" runat="server"
                                                                        CssClass="btn btn-primary btn-sm "
                                                                        CommandName="Edit">
                                                                        <i class="fa fa-edit me-1"></i> Edit
                                                                    </asp:LinkButton>

                                                                    <asp:LinkButton ID="btnDelete" runat="server"
                                                                        CssClass="btn btn-danger btn-sm "
                                                                        CommandName="delete"
                                                                        OnClientClick="return confirm('Sure to delete this data?');">
                                                                        <i class="fa fa-trash me-1"></i> Delete
                                                                    </asp:LinkButton>

                                                                </ItemTemplate>
                                                                <EditItemTemplate>
                                                                    <asp:Button ID="btnUpdate" runat="server"
                                                                        class="bitbtndt btnsave" Text="Save"
                                                                        CommandName="Update" />
                                                                    <asp:Button ID="btnCancel" runat="server"
                                                                        class="bitbtndt btncancel" Text="Cancel"
                                                                        CommandName="Cancel" />
                                                                </EditItemTemplate>
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="Detail">
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="btnDetailMaterial"
                                                                        runat="server" CssClass="btn btn-info btn-sm "
                                                                        CommandArgument="<%# Container.DataItemIndex %>"
                                                                        CommandName="DetailMaterial">
                                                                        <i class="fa fa-circle me-1"></i> Detail Letter
                                                                        C C
                                                                    </asp:LinkButton>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>

                                                            <asp:BoundField DataField="NoWl" HeaderStyle-Width="100px"
                                                                HeaderText="No Wl">
                                                                <HeaderStyle Width="100px" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="NoPercilDt2"
                                                                HeaderStyle-Width="100px" HeaderText="No Percil">
                                                                <HeaderStyle Width="100px" />
                                                            </asp:BoundField>

                                                            <asp:BoundField DataField="AwalName"
                                                                HeaderStyle-Width="150px" HeaderText="Nama Awal">
                                                                <HeaderStyle Width="150px" />
                                                            </asp:BoundField>

                                                            <asp:BoundField DataField="LuasAwal"
                                                                HeaderStyle-Width="80px" HeaderText="Luas"
                                                                DataFormatString="{0:#,##0.##}">
                                                                <HeaderStyle Width="80px" />
                                                            </asp:BoundField>

                                                            <asp:BoundField DataField="NoWarisLevel"
                                                                HeaderStyle-Width="150px" HeaderText="No Waris Level">
                                                                <HeaderStyle Width="150px" />
                                                            </asp:BoundField>

                                                            <asp:BoundField DataField="WlLvlName1"
                                                                HeaderStyle-Width="150px" HeaderText="Nama 1">
                                                                <HeaderStyle Width="150px" />
                                                            </asp:BoundField>

                                                            <asp:BoundField DataField="LuasLvlNo1"
                                                                HeaderStyle-Width="80px" HeaderText="Luas 1"
                                                                DataFormatString="{0:#,##0.##}">
                                                                <HeaderStyle Width="80px" />
                                                            </asp:BoundField>

                                                            <asp:BoundField DataField="WlLvlName2"
                                                                HeaderStyle-Width="150px" HeaderText="Nama 2">
                                                                <HeaderStyle Width="150px" />
                                                            </asp:BoundField>

                                                            <asp:BoundField DataField="LuasLvlNo2"
                                                                HeaderStyle-Width="80px" HeaderText="Luas 2"
                                                                DataFormatString="{0:#,##0.##}">
                                                                <HeaderStyle Width="80px" />
                                                            </asp:BoundField>

                                                            <asp:BoundField DataField="WlLvlName3"
                                                                HeaderStyle-Width="150px" HeaderText="Nama 3">
                                                                <HeaderStyle Width="150px" />
                                                            </asp:BoundField>

                                                            <asp:BoundField DataField="LuasLvlNo3"
                                                                HeaderStyle-Width="80px" HeaderText="Luas 3"
                                                                DataFormatString="{0:#,##0.##}">
                                                                <HeaderStyle Width="80px" />
                                                            </asp:BoundField>


                                                            <asp:BoundField DataField="WlLvlName4"
                                                                HeaderStyle-Width="150px" HeaderText="Nama 4">
                                                                <HeaderStyle Width="150px" />
                                                            </asp:BoundField>

                                                            <asp:BoundField DataField="LuasLvlNo4"
                                                                HeaderStyle-Width="80px" HeaderText="Luas 4"
                                                                DataFormatString="{0:#,##0.##}">
                                                                <HeaderStyle Width="80px" />
                                                            </asp:BoundField>

                                                            <asp:BoundField DataField="WlLvlName5"
                                                                HeaderStyle-Width="150px" HeaderText="Nama 5">
                                                                <HeaderStyle Width="150px" />
                                                            </asp:BoundField>

                                                            <asp:BoundField DataField="LuasLvlNo5"
                                                                HeaderStyle-Width="80px" HeaderText="Luas 5"
                                                                DataFormatString="{0:#,##0.##}">
                                                                <HeaderStyle Width="80px" />
                                                            </asp:BoundField>



                                                            <asp:BoundField DataField="Remark" HeaderStyle-Width="225px"
                                                                HeaderText="Remark">
                                                                <HeaderStyle Width="100px" />
                                                            </asp:BoundField>


                                                        </Columns>
                                                    </asp:GridView>
                                                </div>
                                                <asp:Button class="bitbtn btnadd" runat="server" ID="btnAddDt3ke2"
                                                    Text="Add" ValidationGroup="Input" />
                                            </div>
                                        </div>
                                    </asp:Panel>

                                    <asp:Panel runat="server" ID="pnlEditDt3" Visible="false">
                                        <div class="card shadow-sm">
                                            <div class="card-body p-1">

                                                <div class="container-fluid mt-2">
                                                    <div class="row mb-1">
                                                        <!-- Offering Survey No -->
                                                        <div class="col-md-4">
                                                            <label class="form-label">No Wl
                                                            </label>
                                                            <asp:TextBox ID="tbWlNo" runat="server"
                                                                CssClass="form-control form-control-sm"></asp:TextBox>
                                                        </div>

                                                        <div class="col-md-4">
                                                            <label class="form-label">No Percil
                                                            </label>
                                                            <asp:TextBox ID="tbPercilNoDt" runat="server"
                                                                CssClass="form-control form-control-sm"></asp:TextBox>
                                                        </div>

                                                        <div class="col-md-4">
                                                            <label class="form-label">No Waris Level
                                                            </label>
                                                            <asp:TextBox ID="tbWarisLevelNo" runat="server"
                                                                CssClass="form-control form-control-sm"></asp:TextBox>
                                                        </div>
                                                    </div>


                                                    <!-- Nama Section -->
                                                    <div class="row mb-1">
                                                        <div class="col-md-2">
                                                            <label class="form-label">Nama Pemilik Awal
                                                            </label>
                                                            <asp:TextBox ID="tbNameAwal" runat="server"
                                                                CssClass="form-control form-control-sm"></asp:TextBox>
                                                        </div>

                                                        <div class="col-md-2">
                                                            <label class="form-label">Nama 1
                                                            </label>
                                                            <asp:TextBox ID="tbNamelvl1" runat="server"
                                                                CssClass="form-control form-control-sm"></asp:TextBox>
                                                        </div>

                                                        <div class="col-md-2">
                                                            <label class="form-label">Nama 2
                                                            </label>
                                                            <asp:TextBox ID="tbNamelvl2" runat="server"
                                                                CssClass="form-control form-control-sm"></asp:TextBox>
                                                        </div>

                                                        <div class="col-md-2">
                                                            <label class="form-label">Nama 3
                                                            </label>
                                                            <asp:TextBox ID="tbNamelvl3" runat="server"
                                                                CssClass="form-control form-control-sm"></asp:TextBox>
                                                        </div>

                                                        <div class="col-md-2">
                                                            <label class="form-label">Nama 4
                                                            </label>
                                                            <asp:TextBox ID="tbNamelvl4" runat="server"
                                                                CssClass="form-control form-control-sm"></asp:TextBox>
                                                        </div>

                                                        <div class="col-md-2">
                                                            <label class="form-label">Nama 5
                                                            </label>
                                                            <asp:TextBox ID="tbNamelvl5" runat="server"
                                                                CssClass="form-control form-control-sm"></asp:TextBox>
                                                        </div>
                                                    </div>

                                                    <!-- Luas Section -->

                                                    <div class="row mb-1">
                                                        <div class="col-md-2">
                                                            <label class="form-label">Luas Pemilik Awal
                                                            </label>
                                                            <asp:TextBox ID="tbLuasAwal" runat="server"
                                                                CssClass="form-control form-control-sm"></asp:TextBox>
                                                        </div>

                                                        <div class="col-md-2">
                                                            <label class="form-label">Luas 1
                                                            </label>
                                                            <asp:TextBox ID="tbLuaslvl1" runat="server"
                                                                CssClass="form-control form-control-sm"></asp:TextBox>
                                                        </div>

                                                        <div class="col-md-2">
                                                            <label class="form-label">Luas 2
                                                            </label>
                                                            <asp:TextBox ID="tbLuaslvl2" runat="server"
                                                                CssClass="form-control form-control-sm"></asp:TextBox>
                                                        </div>

                                                        <div class="col-md-2">
                                                            <label class="form-label">Luas 3
                                                            </label>
                                                            <asp:TextBox ID="tbLuaslvl3" runat="server"
                                                                CssClass="form-control form-control-sm"></asp:TextBox>
                                                        </div>

                                                        <div class="col-md-2">
                                                            <label class="form-label">Luas 4
                                                            </label>
                                                            <asp:TextBox ID="tbLuaslvl4" runat="server"
                                                                CssClass="form-control form-control-sm"></asp:TextBox>
                                                        </div>

                                                        <div class="col-md-2">
                                                            <label class="form-label">Luas 5
                                                            </label>
                                                            <asp:TextBox ID="tbLuaslvl5" runat="server"
                                                                CssClass="form-control form-control-sm"></asp:TextBox>
                                                        </div>
                                                    </div>

                                                    <div class="row mb-3">
                                                        <div class="col-md-12">
                                                            <label class="form-label">Remark
                                                            </label>
                                                            <asp:TextBox ID="tbRemarkdt2" runat="server"
                                                                CssClass="form-control form-control-sm"></asp:TextBox>
                                                        </div>
                                                    </div>

                                                    <div class="row mb-1 text-end">
                                                        <div class="col-md-12">
                                                            <asp:LinkButton ID="btnSaveDt3" runat="server"
                                                                CssClass="btn btn-primary btn-sm ">
                                                                <i class="fa fa-edit me-1"></i> Save
                                                            </asp:LinkButton>

                                                            <asp:LinkButton ID="btnCancelDt3" runat="server"
                                                                CssClass="btn btn-danger btn-sm ">
                                                                <i class="fa fa-arrow-left me-1"></i> Cancel
                                                            </asp:LinkButton>
                                                        </div>
                                                    </div>

                                                </div>


                                                <br />
                                            </div>
                                        </div>
                                    </asp:Panel>

                                </asp:View>


                                <asp:View ID="Tab5" runat="server">
                                    <div class="card shadow-sm">
                                        <div class="card-body p-1">
                                            <asp:Panel ID="pnlInfoDt" runat="server">

                                                <div class="row mb-1">
                                                    <div class="col-md-2">
                                                        <asp:Label ID="lblItem" class="form-label" runat="server"
                                                            Text="No WL " />
                                                        <asp:Label ID="lbNoWl"
                                                            class="form-control form-control-sm border-primary rounded"
                                                            runat="server" Font-Bold="False" ForeColor="#0092C8"
                                                            Text="No Wl" />
                                                    </div>

                                                    <div class="col-md-2">
                                                        <asp:Label class="form-label" runat="server"
                                                            Text="Waris Level No 1" />
                                                        <div class="input-group input-group-sm mb-2">
                                                            <span class="input-group-text">
                                                                <asp:Label ID="lbwaris1" runat="server"
                                                                    Font-Bold="False" Text="Waris Name 5" />
                                                            </span>
                                                            <asp:TextBox runat="server" ValidationGroup="Input"
                                                                ID="tbLuas1" Enabled="False"
                                                                CssClass="form-control form-control-sm border" />
                                                        </div>
                                                    </div>

                                                    <div class="col-md-2">
                                                        <asp:Label class="form-label" runat="server"
                                                            Text="Waris Level No 2" />
                                                        <div class="input-group input-group-sm mb-2">
                                                            <span class="input-group-text">
                                                                <asp:Label ID="lbwaris2" runat="server"
                                                                    Font-Bold="False" Text="Waris Name 5" />
                                                            </span>
                                                            <asp:TextBox runat="server" ValidationGroup="Input"
                                                                ID="tbLuas2" Enabled="False"
                                                                CssClass="form-control form-control-sm border" />
                                                        </div>
                                                    </div>

                                                    <div class="col-md-2">
                                                        <asp:Label class="form-label" runat="server"
                                                            Text="Waris Level No 3" />
                                                        <div class="input-group input-group-sm mb-2">
                                                            <span class="input-group-text">
                                                                <asp:Label ID="lbwaris3" runat="server"
                                                                    Font-Bold="False" Text="Waris Name 5" />
                                                            </span>
                                                            <asp:TextBox runat="server" ValidationGroup="Input"
                                                                ID="tbLuas3" Enabled="False"
                                                                CssClass="form-control form-control-sm border" />
                                                        </div>
                                                    </div>

                                                    <div class="col-md-2">
                                                        <asp:Label class="form-label" runat="server"
                                                            Text="Waris Level No 4" />
                                                        <div class="input-group input-group-sm mb-2">
                                                            <span class="input-group-text">
                                                                <asp:Label ID="lbwaris4" runat="server"
                                                                    Font-Bold="False" Text="Waris Name 5" />
                                                            </span>
                                                            <asp:TextBox runat="server" ValidationGroup="Input"
                                                                ID="tbLuas4" Enabled="False"
                                                                CssClass="form-control form-control-sm border" />
                                                        </div>
                                                    </div>

                                                    <div class="col-md-2">
                                                        <asp:Label class="form-label" runat="server"
                                                            Text="Waris Level No 5" />
                                                        <div class="input-group input-group-sm mb-2">
                                                            <span class="input-group-text">
                                                                <asp:Label ID="lbwaris5" runat="server"
                                                                    Font-Bold="False" Text="Waris Name 5" />
                                                            </span>
                                                            <asp:TextBox runat="server" ValidationGroup="Input"
                                                                ID="tbLuas5" Enabled="False"
                                                                CssClass="form-control form-control-sm border" />
                                                        </div>

                                                    </div>
                                                </div>

                                            </asp:Panel>
                                        </div>
                                    </div>
                                    <br>
                                    <div class="card shadow-sm">
                                        <div class="card-body p-1">
                                            <asp:Panel runat="server" ID="PnlDt4">


                                                <div class="row mb-1">
                                                    <div class="col-md-8 mb-0 text-end">
                                                        <asp:Button ID="btnAdddt4" runat="server"
                                                            text="Tambah Data Detail Monitoring Letter C"
                                                            ValidationGroup="Input"
                                                            CssClass="btn btn-primary btn-sm px-10 btn-icon btnadd w-100">

                                                        </asp:Button>
                                                    </div>
                                                    <div class="col-md-4 mb-0 text-end">
                                                        <asp:Button ID="btnBackDt" runat="server" text="Back"
                                                            ValidationGroup="Input"
                                                            CssClass="btn btn-secondary btn-sm px-10 btn-icon btnback w-100">

                                                        </asp:Button>
                                                    </div>
                                                </div>


                                                <div class="table-responsive"
                                                    style="max-height:600px; overflow-y:auto;">
                                                    <asp:GridView ID="GridDt4" runat="server" ShowFooter="false"
                                                        AllowSorting="True" AutoGenerateColumns="False"
                                                        AllowPaging="True" PageSize="10"
                                                        CssClass="table table-striped table-bordered table-hover table-sm  text-nowrap table-soft-text">
                                                        <HeaderStyle CssClass="table-soft-dark text-white sticky-top" />
                                                        <FooterStyle CssClass="table-light" />
                                                        <PagerStyle CssClass="grid-pager text-center bg-light"
                                                            HorizontalAlign="Left" />
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="Action">
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="btnEdit" runat="server"
                                                                        CssClass="btn btn-primary btn-sm "
                                                                        CommandName="Edit">
                                                                        <i class="fa fa-edit me-1"></i> Edit
                                                                    </asp:LinkButton>

                                                                    <asp:LinkButton ID="btnDelete" runat="server"
                                                                        CssClass="btn btn-danger btn-sm "
                                                                        CommandName="delete"
                                                                        OnClientClick="return confirm('Sure to delete this data?');">
                                                                        <i class="fa fa-trash me-1"></i> Delete
                                                                    </asp:LinkButton>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>


                                                            <asp:BoundField DataField="NoDok" HeaderStyle-Width="100px"
                                                                HeaderText="No Dokumen" SortExpression="NoDok">
                                                                <HeaderStyle Width="100px" />
                                                            </asp:BoundField>

                                                            <asp:BoundField DataField="Descript"
                                                                HeaderStyle-Width="100px" HeaderText="Description"
                                                                SortExpression="Descript">
                                                                <HeaderStyle Width="100px" />
                                                            </asp:BoundField>

                                                            <asp:BoundField DataField="DocDate"
                                                                DataFormatString="{0:dd MMM yyyy}" HtmlEncode="true"
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

                                                            <asp:BoundField DataField="Luas" HeaderStyle-Width="80px"
                                                                HeaderText="Luas" SortExpression="Luas"
                                                                DataFormatString="{0:#,##0.##}">
                                                                <HeaderStyle Width="40px" />
                                                            </asp:BoundField>

                                                            <asp:BoundField DataField="Sisa" HeaderStyle-Width="80px"
                                                                HeaderText="Sisa" SortExpression="Sisa"
                                                                DataFormatString="{0:#,##0.##}">
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

                                                            <asp:BoundField DataField="Luas2" HeaderStyle-Width="80px"
                                                                HeaderText="Luas2" SortExpression="Luas2"
                                                                DataFormatString="{0:#,##0.##}">
                                                                <HeaderStyle Width="40px" />
                                                            </asp:BoundField>

                                                            <asp:BoundField DataField="Sisa2" HeaderStyle-Width="80px"
                                                                HeaderText="Sisa2" SortExpression="Sisa2"
                                                                DataFormatString="{0:#,##0.##}">
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

                                                            <asp:BoundField DataField="Luas3" HeaderStyle-Width="80px"
                                                                HeaderText="Luas3" SortExpression="Luas3"
                                                                DataFormatString="{0:#,##0.##}">
                                                                <HeaderStyle Width="40px" />
                                                            </asp:BoundField>

                                                            <asp:BoundField DataField="Sisa3" HeaderStyle-Width="80px"
                                                                HeaderText="Sisa3" SortExpression="Sisa3"
                                                                DataFormatString="{0:#,##0.##}">
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

                                                            <asp:BoundField DataField="Luas4" HeaderStyle-Width="80px"
                                                                HeaderText="Luas4" SortExpression="Luas4"
                                                                DataFormatString="{0:#,##0.##}">
                                                                <HeaderStyle Width="40px" />
                                                            </asp:BoundField>

                                                            <asp:BoundField DataField="Sisa4" HeaderStyle-Width="80px"
                                                                HeaderText="Sisa4" SortExpression="Sisa4"
                                                                DataFormatString="{0:#,##0.##}">
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

                                                            <asp:BoundField DataField="Luas5" HeaderStyle-Width="80px"
                                                                HeaderText="Luas5" SortExpression="Luas5"
                                                                DataFormatString="{0:#,##0.##}">
                                                                <HeaderStyle Width="40px" />
                                                            </asp:BoundField>

                                                            <asp:BoundField DataField="Sisa5" HeaderStyle-Width="80px"
                                                                HeaderText="Sisa5" SortExpression="Sisa5"
                                                                DataFormatString="{0:#,##0.##}">
                                                                <HeaderStyle Width="40px" />
                                                            </asp:BoundField>

                                                            <asp:BoundField DataField="Remark" HeaderStyle-Width="50px"
                                                                HeaderText="Remark" SortExpression="Remark">
                                                                <HeaderStyle Width="50px" />
                                                            </asp:BoundField>

                                                        </Columns>
                                                    </asp:GridView>
                                                </div>
                                                <br>
                                                <div class="row mb-0">
                                                    <div class="col-md-8 mb-0 text-end">
                                                        <asp:Button ID="btnAddDt4ke2" runat="server"
                                                            text="Tambah Data Detail Monitoring Letter C"
                                                            ValidationGroup="Input"
                                                            CssClass="btn btn-primary btn-sm px-10 btn-icon btnadd w-100">

                                                        </asp:Button>
                                                    </div>
                                                    <div class="col-md-4 mb-0 text-end">
                                                        <asp:Button ID="btnBackDt2" runat="server" text="Back"
                                                            ValidationGroup="Input"
                                                            CssClass="btn btn-secondary btn-sm px-10 btn-icon btnback w-100">

                                                        </asp:Button>
                                                    </div>
                                                </div>


                                            </asp:Panel>
                                            <asp:Panel runat="server" ID="pnlEditDt4" Visible="false">
                                                <table>




                                                    <tr>
                                                        <td>Description</td>
                                                        <td>:</td>
                                                        <td colspan="7">
                                                            <table>

                                                                <tr>
                                                                    <td>
                                                                        <asp:TextBox runat="server"
                                                                            ValidationGroup="Input"
                                                                            ID="tbDescriptiondt2" CssClass="TextBox"
                                                                            Width="130px" />
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
                                                                    <td>
                                                                        <asp:TextBox runat="server" MaxLength="30"
                                                                            ValidationGroup="Input" ID="tbNoDok"
                                                                            CssClass="TextBox" Width="130px" />
                                                                    </td>
                                                                    <td></td>

                                                                    <td>Dokumen Date</td>
                                                                    <td>:</td>

                                                                    <td>
                                                                        <BDP:BasicDatePicker ID="tbDatedt2"
                                                                            runat="server" Width="130px"
                                                                            DateFormat="dd MMM yyyy" ReadOnly="False"
                                                                            ValidationGroup="Input"
                                                                            ButtonImageHeight="19px"
                                                                            ButtonImageWidth="20px"
                                                                            DisplayType="TextBoxAndImage"
                                                                            TextBoxStyle-CssClass="TextDate"
                                                                            AutoPostBack="False" ShowNoneButton="False">
                                                                            <TextBoxStyle CssClass="TextDate" />
                                                                        </BDP:BasicDatePicker>
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
                                                                    <td>
                                                                        <asp:TextBox runat="server"
                                                                            ValidationGroup="Input" ID="tbNameSubDt"
                                                                            CssClass="TextBox" Width="130px" />
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox runat="server"
                                                                            ValidationGroup="Input" ID="tbNameSubDt2"
                                                                            CssClass="TextBox" Width="130px" />
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox runat="server"
                                                                            ValidationGroup="Input" ID="tbNameSubDt3"
                                                                            CssClass="TextBox" Width="130px" />
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox runat="server"
                                                                            ValidationGroup="Input" ID="tbNameSubDt4"
                                                                            CssClass="TextBox" Width="130px" />
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox runat="server"
                                                                            ValidationGroup="Input" ID="tbNameSubDt5"
                                                                            CssClass="TextBox" Width="130px" />
                                                                    </td>
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
                                                                    <td>
                                                                        <asp:TextBox runat="server" MaxLength="50"
                                                                            ValidationGroup="Input" ID="tbAJBSubDt"
                                                                            CssClass="TextBox" Width="130px" />
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox runat="server" MaxLength="50"
                                                                            ValidationGroup="Input" ID="tbAJBSubDt2"
                                                                            CssClass="TextBox" Width="130px" />
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox runat="server" MaxLength="50"
                                                                            ValidationGroup="Input" ID="tbAJBSubDt3"
                                                                            CssClass="TextBox" Width="130px" />
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox runat="server" MaxLength="50"
                                                                            ValidationGroup="Input" ID="tbAJBSubDt4"
                                                                            CssClass="TextBox" Width="130px" />
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox runat="server" MaxLength="50"
                                                                            ValidationGroup="Input" ID="tbAJBSubDt5"
                                                                            CssClass="TextBox" Width="130px" />
                                                                    </td>
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
                                                                    <td>
                                                                        <asp:TextBox runat="server"
                                                                            ValidationGroup="Input" ID="tbLuasSubDt"
                                                                            CssClass="TextBox" Width="130px" />
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox runat="server"
                                                                            ValidationGroup="Input" ID="tbLuasSubDt2"
                                                                            CssClass="TextBox" Width="130px" />
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox runat="server"
                                                                            ValidationGroup="Input" ID="tbLuasSubDt3"
                                                                            CssClass="TextBox" Width="130px" />
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox runat="server"
                                                                            ValidationGroup="Input" ID="tbLuasSubDt4"
                                                                            CssClass="TextBox" Width="130px" />
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox runat="server"
                                                                            ValidationGroup="Input" ID="tbLuasSubDt5"
                                                                            CssClass="TextBox" Width="130px" />
                                                                    </td>
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
                                                                    <td>
                                                                        <asp:TextBox ID="tbSisaSubDt"
                                                                            ValidationGroup="Input" runat="server"
                                                                            CssClass="TextBox" width="130px" />
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="tbSisaSubDt2"
                                                                            ValidationGroup="Input" runat="server"
                                                                            CssClass="TextBox" width="130px" />
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="tbSisaSubDt3"
                                                                            ValidationGroup="Input" runat="server"
                                                                            CssClass="TextBox" width="130px" />
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="tbSisaSubDt4"
                                                                            ValidationGroup="Input" runat="server"
                                                                            CssClass="TextBox" width="130px" />
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="tbSisaSubDt5"
                                                                            ValidationGroup="Input" runat="server"
                                                                            CssClass="TextBox" width="130px" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>



                                                    <tr>
                                                        <td>Remark</td>
                                                        <td>:</td>
                                                        <td>
                                                            <asp:TextBox runat="server" ValidationGroup="Input"
                                                                ID="tbremarkSubDt" CssClass="TextBox" Width="695px" />
                                                        </td>
                                                    </tr>

                                                </table>
                                                <br />
                                                <asp:Button ID="btnSaveDt4" runat="server" class="bitbtndt btnsave"
                                                    Text="Save" />
                                                <asp:Button ID="btnCancelDt4" runat="server" class="bitbtndt btncancel"
                                                    Text="Cancel" />
                                            </asp:Panel>
                                        </div>
                                    </div>
                                </asp:View>

                            </asp:MultiView>
                            <br />
                            <!-- <div class="footer-action text-center"> -->


                        </div>
                        <!-- <div class="footer-action text-center"> -->
                        <div class="container-fluid mt-1">

                            <asp:Button ID="btnSaveAll" runat="server"
                                class="btn btn-success btn-sm btn-icon btnsavenew m-1" Text="Save & New"
                                ValidationGroup="Input" />

                            <asp:Button ID="btnSaveTrans" runat="server"
                                class="btn btn-primary btn-sm btn-icon btnsave m-1" Text="Save"
                                ValidationGroup="Input" />

                            <asp:Button ID="btnBack" runat="server"
                                class="btn btn-secondary btn-sm btn-icon btncancel m-1" Text="Cancel"
                                ValidationGroup="Input" />

                            <asp:Button ID="btnHome" runat="server" ValidationGroup="Input"
                                class="btn btn-info btn-sm btn-icon btnhome m-1" Text="Home" />

                        </div>

                        <!-- </div> -->
                    </asp:Panel>




                </div>



                <!-- Modal Popup Status -->
                <div class="modal fade" id="statusModal" tabindex="-1" aria-labelledby="statusModalLabel"
                    aria-hidden="true">
                    <div class="modal-dialog modal-dialog-centered">
                        <div class="modal-content border-0 shadow">
                            <!-- <div class="modal-header  py-2">
                                <h6 class="modal-title mb-0" id="statusModalLabel">
                                    <i class="fa fa-info-circle me-2"></i>Alert Status
                                </h6>
                                <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal"
                                    aria-label="Close"></button>
                            </div> -->

                            <div class="modal-body text-center" id="statusModalBody">
                                <div class="container-fluid">
                                    <asp:Label runat="server" ID="lbStatus"
                                        CssClass="badge d-block text-center fs-6 p-2" EnableViewState="false" />
                                </div>

                            </div>

                            <!-- <div class="modal-footer justify-content-center py-2">
                                <button type="button" class="btn btn-primary btn-sm" data-bs-dismiss="modal">OK</button>
                            </div> -->

                        </div>
                    </div>
                </div>

                <asp:HiddenField ID="HiddenRemarkReject" runat="server" />

                <div class="loading-form">
                    <div class="spinner-border text-primary spinner-style" role="status">
                        <!-- <span class="visually-hidden">Loading...</span> -->
                    </div>
                </div>



            </form>
        </body>

        </html>