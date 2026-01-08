<%@ Page Language="VB" AutoEventWireup="false" CodeFile="MsComplaintUsers.aspx.vb" Inherits="MsComplaintUsers" %>
    <%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
        <!DOCTYPE html
            PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

        <html xmlns="http://www.w3.org/1999/xhtml">

        <head runat="server">
            <title>Seller File</title>
            <script src="../../Function/OpenDlg.js" type="text/javascript"></script>
            <script src="../../Function/Function.JS" type="text/javascript"></script>
            <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
            <link type="text/css" rel="stylesheet" href="../../Styles/circularprogress.css" />
            <link href="https://fonts.googleapis.com/css2?family=Muli:wght@300;400;600&display=swap" rel="stylesheet">
            <script type="text/javascript" src="../../JQuery/jquery.min.js"></script>
            <script type="text/javascript" src="../../JQuery/jquery-ui.js"></script>
            <script type="text/javascript" src="../../JQuery/circularProgressBar.min.js"></script>

            <script type="text/javascript">
                var docs = [];
                var index = 0;

                var scale = 1;
                var translateX = 0;
                var translateY = 0;

                var isDragging = false;
                var startX = 0;
                var startY = 0;

                function openLightbox(data) {
                    docs = data || [];
                    index = 0;

                    resetTransform();

                    document.getElementById("lightbox").style.display = "block";
                    showDoc();
                }

                function closeLightbox() {
                    document.getElementById("lightbox").style.display = "none";
                    document.getElementById("lb-body").innerHTML = "";
                }

                function showDoc() {
                    if (!docs.length) return;

                    resetTransform();

                    var d = docs[index];
                    var body = document.getElementById("lb-body");

                    if (d.type.toLowerCase().indexOf("pdf") !== -1) {
                        body.innerHTML =
                            "<iframe src='" + d.path + "' style='width:100%;height:100%;border:none;'></iframe>";
                        body.style.cursor = "default";
                    } else {
                        body.innerHTML =
                            "<div id='imgWrap' " +
                            "style='width:100%;height:100%;display:flex;align-items:center;justify-content:center;overflow:hidden;'>" +
                            "<img id='lb-img' src='" + d.path + "' " +
                            "style='max-width:90%;max-height:90%;width:auto;height:auto;" +
                            "transform:scale(1);transition:transform .15s ease;' />" +
                            "</div>"
                        body.style.cursor = "grab";

                        attachImageEvents();
                        applyTransform();

                    }

                    updateCounter();
                    updateNav();
                }

                function resetTransform() {
                    scale = 1;
                    translateX = 0;
                    translateY = 0;
                }

                function applyTransform() {
                    var img = document.getElementById("lb-img");
                    if (!img) return;

                    img.style.transform =
                        "translate(" + translateX + "px," + translateY + "px) scale(" + scale + ")";
                }

                /* ================= ZOOM SCROLL ================= */
                document.addEventListener("wheel", function (e) {
                    var img = document.getElementById("lb-img");
                    if (!img) return;

                    e.preventDefault();

                    var delta = e.deltaY > 0 ? -0.1 : 0.1;
                    scale += delta;

                    if (scale < 1) scale = 1;
                    if (scale > 5) scale = 5;

                    applyTransform();
                }, { passive: false });

                /* ================= DRAG IMAGE ================= */
                function attachImageEvents() {
                    var body = document.getElementById("lb-body");

                    /* ==== CLICK & HOLD ==== */
                    body.onmousedown = function (e) {
                        if (scale <= 1) return;        // drag hanya saat zoom

                        isDragging = true;
                        startX = e.clientX - translateX;
                        startY = e.clientY - translateY;

                        body.classList.add("dragging");
                        e.preventDefault();            // ⛔ stop select text
                    };

                    /* ==== MOVE ==== */
                    document.onmousemove = function (e) {
                        if (!isDragging) return;

                        translateX = e.clientX - startX;
                        translateY = e.clientY - startY;
                        applyTransform();
                    };

                    /* ==== RELEASE ==== */
                    document.onmouseup = function () {
                        if (!isDragging) return;

                        isDragging = false;
                        body.classList.remove("dragging");
                    };

                    /* ==== SAFETY (mouse keluar area) ==== */
                    body.onmouseleave = function () {
                        isDragging = false;
                        body.classList.remove("dragging");
                    };
                }
                /* ================= NAVIGATION ================= */

                function updateNav() {
                    document.querySelector(".lb-nav.left").style.display =
                        index > 0 ? "block" : "none";

                    document.querySelector(".lb-nav.right").style.display =
                        index < docs.length - 1 ? "block" : "none";
                }

                function nextDoc() {
                    if (index < docs.length - 1) {
                        index++;
                        showDoc();
                    }
                }

                function prevDoc() {
                    if (index > 0) {
                        index--;
                        showDoc();
                    }
                }

                /* ================= KEYBOARD ================= */
                document.addEventListener("keydown", function (e) {
                    if (document.getElementById("lightbox").style.display !== "block") return;

                    if (e.keyCode === 37) prevDoc(); // ←
                    if (e.keyCode === 39) nextDoc(); // →
                    if (e.keyCode === 27) closeLightbox(); // ESC
                });

                /* ================= COUNTER ================= */
                function updateCounter() {
                    var el = document.getElementById("lb-counter");
                    if (el) el.innerHTML = (index + 1) + " / " + docs.length;
                }


                function ProgressCircle() {
                    setTimeout(function () {
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
                $('form').live("submit", function () {
                    ProgressCircle();
                });

            </script>





        </head>

        <style>
            .status-badge {
                padding: 3px;
                border-radius: 12px;
                font-weight: 600;
                display: inline-block;
                text-align: center;
                min-width: 80px;
            }

            .status-pending {
                background-color: #fff3cd;
                color: #856404;
                border: 1px solid #ffeeba;
            }

            .status-approved {
                background-color: #d4edda;
                color: #155724;
                border: 1px solid #c3e6cb;
            }

            .status-rejected {
                background-color: #f8d7da;
                color: #721c24;
                border: 1px solid #f5c6cb;
            }

            #lightbox {
                position: fixed;
                top: 0;
                left: 0;
                width: 100%;
                height: 100%;
                z-index: 9999;
            }

            .lb-overlay {
                position: absolute;
                width: 100%;
                height: 100%;
                background: rgba(0, 0, 0, 0.7);
            }

            .lb-content {
                position: relative;
                width: 80%;
                height: 85%;
                margin: 3% auto;
                background: #fff;
                padding: 10px;
            }

            .lb-close {
                position: absolute;
                top: 10px;
                right: 15px;
                font-size: 28px;
                cursor: pointer;
            }

            .lb-nav {
                position: absolute;
                top: 50%;
                font-size: 30px;
                background: none;
                border: none;
                cursor: pointer;
            }

            .lb-nav.left {
                left: 10px;
            }

            .lb-nav.right {
                right: 10px;
            }

            #lb-body iframe,
            #lb-body img {
                width: 100%;
                height: 100%;
            }


            /* Lightbox CSS */

            #lightbox {
                position: fixed;
                inset: 0;
                z-index: 9999;
            }

            .lb-overlay {
                position: absolute;
                inset: 0;
                background: rgba(0, 0, 0, 0.75);
            }

            .lb-content {
                position: absolute;
                top: 42%;
                left: 50%;
                width: 90%;
                height: 92%;
                transform: translate(-50%, -50%);
                background: #fff;
                border-radius: 8px;
                overflow: hidden;
                z-index: 10000;
            }

            #lb-body {
                width: 100%;
                height: 100%;
            }

            /* NAV DI LUAR CONTENT */
            .lb-nav {
                position: fixed;
                top: 50%;
                transform: translateY(-50%);
                z-index: 10001;
                font-size: 40px;
                background: none;
                border: none;
                cursor: pointer;
                color: #fff;
            }

            .lb-nav.left {
                left: 20px;
            }

            .lb-nav.right {
                right: 20px;
            }

            .lb-close {
                position: absolute;
                top: 8px;
                right: 12px;
                font-size: 28px;
                cursor: pointer;
                z-index: 10002;
            }

            /* conter dokumen */

            #lb-counter {
                position: absolute;
                bottom: 20px;
                /* ⬅️ di bawah konten */
                left: 50%;
                /* ⬅️ center horizontal */
                transform: translateX(-50%);
                padding: 6px 14px;
                background: rgba(0, 0, 0, 0.6);
                color: #fff;
                font-size: 13px;
                border-radius: 20px;
                z-index: 99999999;
                pointer-events: none;
                /* ⬅️ tidak mengganggu klik */
            }
        </style>



        <body>
            <form id="form1" runat="server">
                <asp:ScriptManager ID="ScriptManager1" runat="server">
                </asp:ScriptManager>
                <div class="Content">
                    <div class="H1">Complaint Users</div>
                    <hr style="color:Blue" />
                    <asp:Panel runat="server" ID="pnlHd">
                        <table>
                            <tr>
                                <td style="width:100px;text-align:right">Quick Search :</td>
                                <td>
                                    <asp:TextBox runat="server" ID="tbFilter" CssClass="TextBox" />
                                    <asp:DropDownList runat="server" ID="ddlField" CssClass="DropDownList">
                                        <asp:ListItem Selected="true" Text="No Tiket" Value="Id">
                                        </asp:ListItem>
                                        <asp:ListItem Text="Status" Value="Status"></asp:ListItem>
                                        <asp:ListItem Text="Users Type" Value="UserType"></asp:ListItem>
                                        <asp:ListItem Text="Nama User" Value="UserName"></asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:Button class="bitbtn btnsearch" runat="server" ID="btnSearch" Text="Search" />
                                    <asp:Button class="btngo" runat="server" ID="btnExpand" Text="..." />
                                    <!-- <asp:Button class="bitbtn btnprint" runat="server" ID="btnPrint" Text="Print" /> -->
                                </td>
                            </tr>
                        </table>
                        <asp:Panel runat="server" ID="pnlSearch" Visible="false">
                            <table>
                                <tr>
                                    <td style="width:100px;text-align:right">
                                        <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlNotasi">
                                            <asp:ListItem Selected="true" Text="OR" Value="OR"></asp:ListItem>
                                            <asp:ListItem Text="AND" Value="AND"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="tbfilter2" CssClass="TextBox" />
                                        <asp:DropDownList runat="server" ID="ddlField2" CssClass="DropDownList">
                                            <asp:ListItem Selected="true" Text="No Tiket" Value="Id">
                                        </asp:ListItem>
                                        <asp:ListItem Text="Status" Value="Status"></asp:ListItem>
                                        <asp:ListItem Text="Users Type" Value="UserType"></asp:ListItem>
                                        <asp:ListItem Text="Nama User" Value="UserName"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <br />
                        <!-- <asp:Button class="bitbtn btnadd" runat="server" ID="btnAdd" Text="Add" /> -->
                        <asp:GridView id="DataGrid" runat="server" ShowFooter="True" AllowSorting="True"
                            AutoGenerateColumns="False" AllowPaging="True" CssClass="Grid">
                            <HeaderStyle CssClass="GridHeader" Wrap="false"></HeaderStyle>
                            <RowStyle CssClass="GridItem" wrap="false" />
                            <AlternatingRowStyle CssClass="GridAltItem" />
                            <FooterStyle CssClass="GridFooter" />
                            <PagerStyle CssClass="GridPager" HorizontalAlign="Left" />
                            <Columns>
                                <asp:TemplateField HeaderStyle-Width="110" HeaderText="Action">
                                    <ItemTemplate>
                                        <asp:DropDownList CssClass="DropDownList" ID="ddl" runat="server">
                                            <asp:ListItem Selected="True" Text="View" />
                                            <asp:ListItem Text="Approve User" />
                                            <asp:ListItem Text="Reject" />

                                        </asp:DropDownList>
                                        <asp:Button class="btngo" runat="server" ID="btnGo" Text="G"
                                            CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>"
                                            CommandName="Go" />
                                    </ItemTemplate>
                                    <HeaderStyle Width="110px" />
                                </asp:TemplateField>



                                <asp:BoundField DataField="Id" HeaderText="No Tiket"
                                    HeaderStyle-Width="140" SortExpression="RequestNumber" />
                                <asp:TemplateField HeaderText="Status" SortExpression="Status">
                                    <ItemTemplate>
                                        <span class='<%# GetStatusCss(Eval("Status")) %>'>
                                            <%# Eval("Status") %>
                                        </span>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Bukti Foto" HeaderStyle-Width="90">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkPreview" runat="server"
                                            Text='<%# String.Format("Bukti Foto: {0}", Eval("PhotoCount")) %>' CommandName="PreviewLightbox"
                                            CommandArgument='<%# Eval("ComplaintId") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>

                               
                                <asp:BoundField DataField="UserType" HeaderText="Role Type" SortExpression="UserType" />
                                 <asp:BoundField DataField="Title" HeaderText="Title" HeaderStyle-Width="200"
                                    SortExpression="Title" />
                                <asp:BoundField DataField="Description" HeaderText="Address"
                                    SortExpression="Description" HeaderStyle-Width="400" />
                                <asp:BoundField DataField="Date" DataFormatString="{0:dd MMM yyyy HH:mm:ss}"
                                    HeaderText="Created At" SortExpression="Date" />


                            </Columns>
                        </asp:GridView>


                        <div id="lightbox" style="display:none;">
                            <div class="lb-overlay" onclick="closeLightbox()"></div>


                            <!-- CONTENT -->
                            <div class="lb-content">
                                <span class="lb-close" onclick="closeLightbox()">×</span>
                                <div id="lb-body"></div>
                            </div>
                            <div id="lb-counter">1 / 1</div>

                            <!-- NAVIGATION (DI LUAR IFRAME AREA) -->
                            <button type="button" class="lb-nav left" onclick="prevDoc(event)">❮</button>
                            <button type="button" class="lb-nav right" onclick="nextDoc(event)">❯</button>

                        </div>


                         <!-- POPUP REJECT -->

                <div class="UntukModalReject">
                    <!--------------------- Start modal CIP Update ------------------------------->
                    <!-- === Modal Custom Update === -->
                    <div id="customModalReject" class="custom-modal" >
                        <div class="custom-modal-content" >
                            <div class="custom-modal-header">
                                <h5>Masukan Alasan Rejact</h5>
                                <span class="close-btn" onclick="closeCustomModalReject()">&times;</span>
                            </div>

                            <div class="custom-modal-body">
                                <asp:TextBox ID="txtRejectReason" runat="server" CssClass="TextBox"
                                    TextMode="MultiLine" Width="490px" Height="150px"
                                    placeholder="Masukkan keterangan di sini..." />
                                <br>
                            </div>

                            <div class="custom-modal-footer">
                                <asp:Button ID="btnRejectOK" class="btn bitbtndt btnsave" runat="server" Text="OK" />

                                <asp:Button ID="btnRejectCancel" class="btn bitbtndt btncancel"
                                    OnClientClick="closeCustomModalReject()" style="background-color: rgb(165, 35, 61);"
                                    runat="server" Text="Cancel" />
                            </div>
                        </div>
                    </div>

                    <script type="text/javascript">
                        function showCustomModalReject() {
                            document.getElementById('customModalReject').style.display = 'flex';
                        }

                        function closeCustomModalReject() {
                            document.getElementById('customModalReject').style.display = 'none';
                        }

                        window.onclick = function (event) {
                            const modal2Update = document.getElementById("customModalReject");
                            if (event.target === modal2Update) {
                                closeCustomModalReject();
                            }
                        }
                    </script>
                    <!--------------------- End modal CIP Update ------------------------------->
                </div>


                        <!-- <asp:Button class="bitbtn btnadd" runat="server" ID="btnAdd2" Text="Add" Visible="False" /> -->
                    </asp:Panel>
                    <asp:Panel runat="server" ID="pnlInput" Visible="false">
                        <table>
                            <tr>
                                <td>No Registrasi</td>
                                <td>:</td>
                                <td>
                                    <asp:TextBox runat="server" CssClass="TextBox" enabled="False" ID="tbCode"
                                        MaxLength="20" />
                                </td>
                            </tr>
                            <tr>
                                <td>Name</td>
                                <td>:</td>
                                <td>
                                    <asp:TextBox runat="server" MaxLength="60" CssClass="TextBox" ID="tbName"
                                        ValidationGroup="Input" Width="300px" />
                                </td>
                            </tr>

                            <tr>
                                <td>Role Type</td>
                                <td>:</td>
                                <td>
                                    <asp:DropDownList runat="server" CssClass="DropDownList" Width="306px"
                                        ID="ddlRoleType" ValidationGroup="Input">
                                        <asp:ListItem Selected="True">OWNER</asp:ListItem>
                                        <asp:ListItem>COMPANY</asp:ListItem>
                                        <asp:ListItem>VENDOR</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>

                            <tr>
                                <td>Deskripsi</td>
                                <td>:</td>
                                <td>
                                    <asp:TextBox runat="server" MaxLength="200" CssClass="TextBox" ID="tbKavlingDesc"
                                        ValidationGroup="Input" Width="300px" />
                                </td>
                            </tr>

                            <tr>
                                <td>Email</td>
                                <td>:</td>
                                <td>
                                    <asp:TextBox runat="server" MaxLength="60" CssClass="TextBox" ID="tbEmail"
                                        ValidationGroup="Input" Width="300px" />
                                </td>
                            </tr>

                        </table>
                        <br>
                        <asp:Button ID="BtnSave" runat="server" class="bitbtndt btnsave" CommandName="Update"
                            Text="Save" />
                        &nbsp;
                        <asp:Button ID="btnCancel" runat="server" class="bitbtndt btncancel" CommandName="Cancel"
                            Text="Cancel" />
                        &nbsp;
                        <asp:Button ID="btnReset" runat="server" class="bitbtndt btncancel" CommandName="Cancel"
                            Text="Reset" />
                        &nbsp;
                        <asp:Button ID="btnHome" runat="server" class="bitbtndt btnback" Text="Home" />

                    </asp:Panel>
                    <asp:Label ID="lstatus" ForeColor="red" runat="server"></asp:Label>
                </div>


               

                <div class="loading" align="center">
                    <br />
                    <img src="../../Image/loader.gif" alt="" />
                </div>




            </form>
        </body>




        </html>