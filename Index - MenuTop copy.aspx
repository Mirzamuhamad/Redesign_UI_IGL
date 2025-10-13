<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Index.aspx.vb" Inherits="Index" %>

    <%--<%@ Register Assembly="obout_EasyMenu_Pro" Namespace="OboutInc.EasyMenu_Pro" TagPrefix="oem" %>--%>

        <%--<!DOCTYPE html
            PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">--%>
            <!-- <!DOCTYPE html> -->
            <html xmlns="http://www.w3.org/1999/xhtml">

            <head runat="server">

                <title>Irama Gemilang Lestari</title>
                <link href="Styles/MenuTemplates.css" rel="stylesheet" type="text/css" />
                <link href="Styles/Style.css" rel="stylesheet" type="text/css" />
                <link rel="stylesheet" type="text/css" href="sdmenu/sdmenu.css" />
                <link type="text/css" rel="stylesheet" href="Styles/circularprogress.css" />
                <script type="text/javascript" src="JQuery/jquery.min.js"></script>
                <script type="text/javascript" src="sdmenu/sdmenu.js">

                    /***********************************************
                    * Slashdot Menu script- By DimX
                    * Submitted to Dynamic Drive DHTML code library: http://www.dynamicdrive.com
                    * Visit Dynamic Drive at http://www.dynamicdrive.com/ for full source code
                    ***********************************************/
                </script>
                <script type="text/javascript">
                    function OpenAcc() {
                        var _Value = document.getElementById('someID').innerHTML; //innerHTML; //innerText;
                        //alert("*"+_Value+"*");
                        //var iText = theDiv.innerHTML.replace(re,'');
                        var Iindex = _Value.search("Change")
                        //alert(Iindex);
                        //if (_Value == "Change Period :")
                        if (Iindex >= 0) {
                            var keyId = document.getElementById('<%= HiddenKeyId.ClientID %>').value;
                            window.open("AccountingPeriod.aspx?KeyId=" + keyId, "List", "scrollbars=yes,resizable=no,width=200,height=150");
                            return false;
                        }
                    }
                    function updateAcc(taon, bulan) {
                        var browserName = navigator.appName;
                        if (browserName == "Microsoft Internet Explorer") {
                            document.getElementById('lbYear').innerText = bulan + ', ' + taon;
                        }
                        else {
                            document.getElementById('lbYear').textContent = bulan + ', ' + taon;
                        }
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

                <style type="text/css">
                    /* Tampilan menu utama */
                    .MenuBar {
                        background-color: #097dbc;
                        font-family: 'Segoe UI', sans-serif;
                        font-size: 14px;
                        height: auto;
                        padding: 0;
                        border-bottom: 3px solid #0077b6;
                        position: relative;
                        z-index: 10;
                    }

                    /* Efek klik */
                    .MenuBar a:active {
                        transform: scale(0.97);
                        transition: transform 0.1s ease;
                    }



                    /* Menu item statis (level 0) */
                    .StaticMenuItemStyleMB {
                        color: white;
                        padding: 12px 20px;
                        text-decoration: none;
                        display: inline-block;
                        transition: background-color 0.5s ease, transform 0.5s ease;
                    }

                    /* Hover efek */
                    .StaticHoverStyleMB {
                        background-color: #0077b6;
                        color: white !important;
                        transform: translateY(-2px);
                        cursor: pointer;
                    }

                    .StaticSelectedStyleMB {
                        background-color: #005f87;
                        font-weight: bold;
                    }

                    /* Submenu */
                    .DynamicMenuItemStyleMB {
                        background-color: #f5f5f5;
                        color: #f5f5f5;
                        padding: 10px 15px;
                        border-bottom: 1px solid #ddd;
                        border-radius: 10px;
                        /* ✅ Sudut membulat submenu */
                        white-space: nowrap;
                        opacity: 0;
                        transform: translateY(-10px);
                        transition: all 0.3s ease;
                        z-index: 999;
                        pointer-events: none;
                    }

                    /* Hover submenu */
                    .DynamicHoverStyleMB {
                        background-color: #e0f0ff;
                        color: #f6f6f6;
                    }

                    /* Submenu visible on hover */
                    .MenuBar .aspNet-Menu-Vertical ul,
                    .MenuBar .aspNet-Menu-Horizontal ul {
                        display: block !important;
                        opacity: 1 !important;
                        transform: translateY(0) !important;
                        pointer-events: auto;
                    }

                    .StaticMenuItemStyleMB:active,
                    .DynamicMenuItemStyleMB:active {
                        transform: scale(0.97);
                        transition: transform 0.1s ease;
                    }


                    /* Z-index fix */
                    .aspNet-Menu ul {
                        z-index: 9999 !important;
                    }

                    /* Opsional tambahan */
                    .staticMenu {
                        background-color: #097dbc;
                        color: white;
                        padding: 10px;
                        font-weight: bold;
                        text-decoration: none;
                    }

                    .staticHover {
                        background-color: #2391cb;
                        color: white;
                    }

                    .dynamicMenu {
                        background-color: rgb(70, 70, 70);
                        border: 1px solid rgb(70, 70, 70);
                        z-index: 9999;
                        min-width: 160px;
                        padding: 5px;
                        animation: fadeInSlide 0.5s ease;
                        color: #ebebeb;

                    }

                    .dynamicHover {
                        background-color: #6d6d6d;
                    }



                    /* Keyframe untuk animasi fade & slide */
                    @keyframes fadeInSlide {
                        from {
                            opacity: 0;
                            transform: translateY(-8px);
                        }

                        to {
                            opacity: 1;
                            transform: translateY(0);
                        }
                    }

                    /* Style iframe dan layout */
                    html,
                    body {
                        height: 100%;
                        margin: 0;
                        padding: 0;
                        overflow: hidden;
                    }

                    #topContent2,
                    .MenuBar,
                    .menu-container {
                        overflow: visible !important;
                        position: relative;
                        z-index: 1;
                    }

                    #mainContent {
                        width: 100%;
                        height: calc(100% - 100px);
                        /* Sesuaikan tinggi header */
                        position: relative;
                    }

                    iframe {
                        border: none;
                        width: 100%;
                        height: 100%;
                    }
                </style>


                <style>
                    /* Spinner container */
                    #loadingSpinner {
                        display: none;
                        position: fixed;
                        z-index: 99999;
                        top: 0;
                        left: 0;
                        width: 100vw;
                        height: 100vh;
                        background: rgba(0, 0, 0, 0.3);
                        justify-content: center;
                        align-items: center;
                    }

                    /* Spinner */
                    .spinner {
                        border: 6px solid #f3f3f3;
                        border-top: 6px solid #0077b6;
                        border-radius: 50%;
                        width: 50px;
                        height: 50px;
                        animation: spin 0.8s linear infinite;
                    }

                    @keyframes spin {
                        0% {
                            transform: rotate(0deg);
                        }

                        100% {
                            transform: rotate(360deg);
                        }
                    }

                    /* Tombol menu untuk layar kecil */
.menu-toggle {
    display: none;
    font-size: 24px;
    padding: 10px;
    color: white;
    background-color: #003c58;
    cursor: pointer;
}

@media (max-width: 768px) {
    .menu-toggle {
        display: block;
    }

    .responsiveMenu ul {
        display: none;
        flex-direction: column;
        width: 100%;
        background-color: #003c58;
    }

    .responsiveMenu.show ul {
        display: flex !important;
    }

    .responsiveMenu li {
        width: 100%;
        border-bottom: 1px solid #0077b6;
    }

    .responsiveMenu a {
        display: block;
        padding: 12px;
        color: white;
        text-align: left;
    }
}

                </style>








                <script type="text/javascript">
                    /*function resizeImg(width) {
                        document.getElementById("menuTop").style.height = 30;
                        document.getElementById("menuTop").style.width = width;    
                    }
                    window.onresize = resizeImg(document.body.clientWidth);     */

                    // <![CDATA[
                    var myMenu;
                    window.onload = function () {
                        myMenu = new SDMenu("my_menu");
                        myMenu.init();
                    };
                    // ]]>

                </script>

                <script>
                    window.addEventListener('mouseover', function (e) {
                        if (e.target.tagName === 'TD' || e.target.tagName === 'A') {
                            console.log('Hover:', e.target.innerText);
                        }
                    });
                </script>
                <!-- 
<script type="text/javascript">
    function loadMenu(menuId) {
        var keyId = document.getElementById('<%= HiddenKeyId.ClientID %>').value;
        window.location.href = "Index.aspx?KeyId=" + keyId + "&ContainerId=" + menuId;
    }
</script> -->



                <!-- <script type="text/javascript">
    function showLoadingAndLoadUrl(url) {
        var spinner = document.getElementById("loadingSpinner");
        if (spinner) spinner.style.display = "flex";

        var pnlSearch = document.getElementById('<%= pnlSearch.ClientID %>');
        var pnlTransfer = document.getElementById('<%= PnlTransfer.ClientID %>');
        if (pnlSearch) pnlSearch.style.display = "none";
        if (pnlTransfer) pnlTransfer.style.display = "none";

        var iframe = document.getElementById("MainFrame");
       
        if (iframe) {
            try {
                // Force reload by assigning blank first (if needed)
                iframe.src = "about:blank";
                

                // Delay setting target URL
                setTimeout(function () {
                    iframe.src = url;

                    // Optional: remove loading after loaded
                    iframe.onload = function () {
                        spinner.style.display = "none";
                    };
                }, 100);
            } catch (e) {
                console.error("iframe load error:", e);
                spinner.style.display = "none";
            }
        }
    }
</script> -->

                <!-- <script type="text/javascript">
    function hidePanelsAndLoadInFrame(url) {
        try {
            var pnlSearch = document.getElementById('<%= pnlSearch.ClientID %>');
            var pnlTransfer = document.getElementById('<%= PnlTransfer.ClientID %>');
            var iframe = window.frames['InFrame'];

            if (pnlSearch) pnlSearch.style.display = 'none';
            if (pnlTransfer) pnlTransfer.style.display = 'none';

            if (iframe) {
                iframe.location.href = url;
            } else {
                console.warn("iframe 'InFrame' tidak ditemukan");
            }
        } catch (e) {
            console.error("Error hidePanelsAndLoadInFrame:", e);
        }
    }
</script> -->

                <script type="text/javascript">
                    function hidePanelsAndLoadInFrame(url) {
                        try {
                            var pnlSearch = document.getElementById('<%= pnlSearch.ClientID %>');
                            var pnlTransfer = document.getElementById('<%= PnlTransfer.ClientID %>');
                            var iframe = document.getElementById('InFrame');
                            var spinner = document.getElementById('loadingSpinner');

                            if (pnlSearch) pnlSearch.style.display = 'none';
                            if (pnlTransfer) pnlTransfer.style.display = 'none';
                            if (spinner) spinner.style.display = 'flex';

                            if (iframe) {
                                // ⛔ Hapus event lama supaya gak nempel dua kali
                                iframe.onload = null;

                                // ✅ Tambahkan event onload untuk sembunyikan loading
                                iframe.onload = function () {
                                    spinner.style.display = 'none';
                                };

                                // ✅ Gunakan .src agar event onload benar-benar dipicu
                                iframe.src = url;
                            } else {
                                console.warn("iframe 'InFrame' tidak ditemukan");
                                if (spinner) spinner.style.display = 'none';
                            }
                        } catch (e) {
                            console.error("Error hidePanelsAndLoadInFrame:", e);
                            var spinner = document.getElementById('loadingSpinner');
                            if (spinner) spinner.style.display = 'none';
                        }
                    }
                </script>


<script type="text/javascript">
    function toggleMenu() {
        var menu = document.querySelector('.responsiveMenu');
        if (menu.classList.contains('show')) {
            menu.classList.remove('show');
        } else {
            menu.classList.add('show');
        }
    }
</script>














            </head>

            <body style="margin: 0px; overflow: visible;">

                <form id="form" runat="server">

                    <div id="topContent">

                        <table style="width:100%; padding:0,0,0,0; margin:0,0,0,0; background-image:url('Image/.gif')">
                            <tr></tr>

                            <tr>

                                <td style="width:92%">
                                    <table width="100%">
                                        <tr>

                                            <td>
                                                <asp:Label ForeColor="#eeeeee" ID="lbCompany" Font-Names="Arial"
                                                    Font-Size="16" Font-Bold="true" runat="server" />
                                            </td>

                                            <td style="width:40%; text-align:right">
                                                <!-- <asp:image CssClass="" id="imgUpload" bordercolor="Black" 
                                                runat="server" width="150px"  /> -->
                                                <asp:Label ID="Label1" Font-Names="Arial" Font-Size="13pt"
                                                    ForeColor="#eeeeee" runat="server" Text="Hi,  " />
                                                <asp:Label ID="lbUser" Font-Names="Arial" Font-Size="11pt"
                                                    ForeColor="#eeeeee" Font-Bold="true" runat="server" Text="HI" />
                                                &nbsp &nbsp
                                                <asp:LinkButton CssClass="Home" ID="lkbHome" runat="server"
                                                    Font-Names="Roboto"> &nbsp <span>Home</span> </asp:LinkButton> &nbsp
                                                <asp:LinkButton CssClass="Log-Out" ID="LinkButton1" runat="server"
                                                    Font-Names="Roboto"> <span>LogOut</span> </asp:LinkButton>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>

                        <div class="Period">
                            <%--<a style="color:blue; font-family:Arial; font-size:8pt"
                                onclick="OpenAcc(); return false;" id="someID" href="">Period</a>--%>
                                <asp:Label Text="Period" Onclick="OpenAcc(); return false;" id="someID"
                                    runat="server" />
                                <asp:Label ID="lbYear" runat="server" />
                        </div>

                    </div>


                    <div id="topContent2">
                        <div class="menu-toggle" onclick="toggleMenu()">☰</div>

                        <div id="myMenuContainer" runat="server"></div>
                        


                        <asp:Menu ID="menuTop" runat="server"
    Orientation="Horizontal"
    RenderingMode="List"
    StaticDisplayLevels="1"
    MaximumDynamicDisplayLevels="3"
    StaticMenuItemStyle-CssClass="staticMenu"
    StaticHoverStyle-CssClass="staticHover"
    DynamicMenuItemStyle-CssClass="dynamicMenu"
    DynamicHoverStyle-CssClass="dynamicHover"
    CssClass="MenuBar menu-container responsiveMenu">
</asp:Menu>


                    </div>





                    <asp:Panel runat="server" ID="pnlSearch" Visible="True"
                        style="padding-top: 60px;padding-left: 30px;">
                        <table>

                            <tr>
                                <td
                                    style=" text-align: center; background-color:#fcfcfc;border-radius: 5px; box-shadow: 0 1px 5px rgb(0 0 0 / 0.2);">
                                    <asp:Label style="padding:5px;" Height="40" ForeColor="#4a4a4a" Font-Names="Arial"
                                        Font-Size="21.5" Font-Bold="true" Text="Approval Dashboard" runat="server" />
                                </td>

                            </tr>

                            <tr>
                                <td>

                                </td>
                            </tr>
                            <tr>

                                <td
                                    style="border: 0px solid #8b8c8c;background-color:#fcfcfc; border-radius:5px; box-shadow: 0 3px 10px rgb(0 0 0 / 0.2); padding:5px;">
                                    <asp:GridView id="DataGrid" runat="server" AllowPaging="True" CssClass="Grid"
                                        AutoGenerateColumns="false">
                                        <HeaderStyle CssClass="GridHeader" Font-Names="Arial" Font-Size="11.5"
                                            Font-Bold="true" Wrap="false"></HeaderStyle>
                                        <RowStyle CssClass="GridItem" Wrap="false" />
                                        <AlternatingRowStyle CssClass="GridAltItem" />
                                        <PagerStyle CssClass="GridPager" />
                                        <EmptyDataTemplate>

                                        </EmptyDataTemplate>
                                        <Columns>
                                            <asp:TemplateField HeaderText="Menu Name" HeaderStyle-Width="300"
                                                Visible="false">
                                                <Itemtemplate>
                                                    <asp:Label Runat="server" ID="MenuId"
                                                        text='<%# DataBinder.Eval(Container.DataItem, "MenuId") %>'>
                                                    </asp:Label>
                                                </Itemtemplate>

                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="Menu Name"
                                                HeaderStyle-Width="300">
                                                <Itemtemplate>
                                                    <asp:Label style="font-size: 14px;" Runat="server" ID="MenuName"
                                                        text='<%# DataBinder.Eval(Container.DataItem, "MenuName") %>'>
                                                    </asp:Label>
                                                </Itemtemplate>

                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="To be Approve" HeaderStyle-Width="200"
                                                ItemStyle-HorizontalAlign="Center">
                                                <Itemtemplate>
                                                    <asp:LinkButton style="font-size: 14px;" ID="btnLink" runat="server"
                                                        Visible="true" CommandName="View"
                                                        text='<%# DataBinder.Eval(Container.DataItem, "Outstanding") %>'
                                                        CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>">
                                                    </asp:LinkButton>
                                                    <%-- <asp:Label Runat="server" ID="Outstanding"
                                                        text='<%# DataBinder.Eval(Container.DataItem, "Outstanding") %>'>--%>
                                                        </asp:Label>
                                                </Itemtemplate>

                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="To be Approve" HeaderStyle-Width="200"
                                                Visible="false">
                                                <Itemtemplate>
                                                    <asp:Label Runat="server" ID="MenuUrl"
                                                        text='<%# DataBinder.Eval(Container.DataItem, "MenuUrl") %>'>
                                                    </asp:Label>
                                                </Itemtemplate>

                                            </asp:TemplateField>

                                            <%-- <asp:TemplateField HeaderText="Value" HeaderStyle-Width="150">
                                                <Itemtemplate>
                                                    <asp:Label Runat="server" ID="Url">
                                                        <a target='_blank'
                                                            href='<%# DataBinder.Eval(Container.DataItem, "MenuUrl") %>'>
                                                            <%# DataBinder.Eval(Container.DataItem, "MenuUrl" ) %>
                                                        </a>
                                                    </asp:Label>
                                                </Itemtemplate>

                                                </asp:TemplateField>--%>




                                                <%--<asp:TemplateField HeaderText="Action" headerstyle-width="126">
                                                    <ItemTemplate>
                                                        <asp:Button class="bitbtn btnedit" Width="150Px" runat="server"
                                                            ID="btnLink" Text="Go to Approve" CommandName="View"
                                                            CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" />


                                                    </ItemTemplate>

                                                    </asp:TemplateField>--%>

                                        </Columns>
                                    </asp:GridView>
                                </td>
                            </tr>
                        </table>
                        <%--<table>
                            <tr>
                                <td>
                                    <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlNotasi">
                                        <asp:ListItem Selected="true" Text="OR" Value="OR"></asp:ListItem>
                                        <asp:ListItem Text="AND" Value="AND"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:TextBox runat="server" CssClass="TextBox" ID="tbfilter2" />

                                </td>
                            </tr>
                            </table> --%>
                    </asp:Panel>

                    <asp:Panel runat="server" ID="PnlTransfer" Visible="False"
                        style="padding-top: 30px;padding-left: 30px;">
                        <table>

                            <tr>
                                <td
                                    style=" text-align: center; background-color:#fcfcfc;border-radius: 5px; box-shadow: 0 1px 5px rgb(0 0 0 / 0.2);">
                                    <asp:Label ID="Label2" style="padding:5px;" Height="30" Width="355"
                                        ForeColor="#4a4a4a" Font-Names="Arial" Font-Size="20" Font-Bold="true"
                                        Text="Transfer Ending Balance" runat="server" />
                                </td>

                            </tr>

                            <tr>
                                <td>

                                </td>
                            </tr>
                            <tr>

                                <td
                                    style="border: 0px solid #8b8c8c;background-color:#fcfcfc; border-radius:5px; box-shadow: 0 3px 10px rgb(0 0 0 / 0.2); padding:5px;">


                                    <asp:Label ID="label3" runat="server" Text="From This Year : " />
                                    <asp:DropDownList ID="ddlYear" Width="100" ValidationGroup="Input"
                                        AutoPostBack="true" runat="server" CssClass="DropDownList" /> to
                                    <asp:Label ID="lbYearAfter" runat="server" Text="2020" />
                                    </br>
                                    </br>
                                    <asp:Button style=" text-align: center;" class="bitbtn btnapply" runat="server"
                                        Width="348" ID="btnProcess" Text="Process" />

                                </td>
                            </tr>
                        </table>
                    </asp:Panel>

                    <asp:Panel runat="server" ID="ForInFrame" Visible="True"
                        style="padding-top: 0px;padding-left: 0px;">
                        <iframe name="InFrame" id="InFrame"
                            style="width:100%;height:88%;  border: none; padding-top"></iframe>
                    </asp:Panel>



                    <div id="mainContent">
                        <iframe name="MainFrame" id="MainFrame"></iframe>
                    </div>



                    <!-- <asp:Panel runat="server" ID="PKon" Visible="True">  	
        <div id="leftContent">
            <%--<asp:Menu ID="menumodule" runat="server" Orientation="Vertical" Width="100%" Target="MainFrame"  
                        CssClass="MenuBar2" StaticEnableDefaultPopOutImage="False" MaximumDynamicDisplayLevels="5">
                        <StaticMenuStyle CssClass="StaticMenuItemMB2" />                
                        <StaticMenuItemStyle CssClass="StaticMenuItemStyleMB2" />
                        <StaticHoverStyle CssClass="StaticHoverStyleMB2" />  
                        <StaticSelectedStyle CssClass="StaticSelectedStyleMB2" />              
                        <DynamicMenuItemStyle CssClass="DynamicMenuItemStyleMB2" />
                        <DynamicHoverStyle CssClass="DynamicHoverStyleMB2" />
                        </asp:Menu> 
            <asp:PlaceHolder ID="placeHolder1" runat="server">--%>
            <div style="float: left" id="my_menu" class="sdmenu" runat="server">          
            </div>    
            <%--</asp:PlaceHolder> --%>           
        </div>
     </asp:Panel>  -->


                    <div id="footerContent">
                        <%--<asp:Image ID="PoweredByImage" ImageUrl="" runat="server"
                            AlternateText="Powered by ASP.NET!" />--%>
                        <asp:Label runat="server" ID="lStatus"></asp:Label>
                        <asp:HiddenField ID="HiddenKeyId" runat="server" />
                    </div>
                    <div class="loading" align="center">

                        <br />
                        <img src="../../Image/loader.gif" alt="" />
                    </div>

                    <div id="loadingSpinner"
                        style="display:none; position:fixed; z-index:9999; top:0; left:0; width:100vw; height:100vh; background:rgba(0,0,0,0.3); justify-content:center; align-items:center;">
                        <div
                            style="border:6px solid #f3f3f3; border-top:6px solid #0077b6; border-radius:50%; width:50px; height:50px; animation:spin 0.8s linear infinite;">
                        </div>
                    </div>
                    <style>
                        @keyframes spin {
                            0% {
                                transform: rotate(0deg);
                            }

                            100% {
                                transform: rotate(360deg);
                            }
                        }
                    </style>


                    <%--<div style="color:Red; text-align:center">
                        <asp:Label runat="server" ID="lStatus"></asp:Label>
                        </div>--%>
                </form>
            </body>

            </html>