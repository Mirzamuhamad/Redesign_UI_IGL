<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Index.aspx.vb" Inherits="Index" %>

    <%--<%@ Register Assembly="obout_EasyMenu_Pro" Namespace="OboutInc.EasyMenu_Pro" TagPrefix="oem" %>--%>
    <%@ Register Assembly="BasicFrame.WebControls.BasicDatePicker" Namespace="BasicFrame.WebControls" TagPrefix="BDP" %>

        <%--<!DOCTYPE html
            PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">--%>
            <!-- <!DOCTYPE html> -->
            <html xmlns="http://www.w3.org/1999/xhtml">

            <head runat="server">

                <title>Irama Gemilang Lestari</title>
                <link rel="icon" type="image/png" href="Image/LogoDP.jpg">
                <meta name="viewport" content="width=device-width, initial-scale=1.0" />
                <!-- <link href="https://fonts.googleapis.com/css2?family=Inter:wght@400;600&display=swap" rel="stylesheet"> -->
                <!-- <link href="https://fonts.googleapis.com/css2?family=Muli:wght@300;400;600&display=swap"                    rel="stylesheet"> -->
                <link href="Styles/MenuTemplates.css" rel="stylesheet" type="text/css" />
                <link href="Styles/Style.css" rel="stylesheet" type="text/css" />
                <link rel="stylesheet" type="text/css" href="sdmenu/sdmenu.css" />
                <link type="text/css" rel="stylesheet" href="Styles/circularprogress.css" />
                <link href="Styles/StyleNew.css" rel="stylesheet" type="text/css" />
                <script type="text/javascript" src="JQuery/jquery.min.js"></script>

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
                            var width = 200;
                            var height = 150;
                            var left = (screen.width / 2) - (width / 2);
                            var top = (screen.height / 4) - (height / 2);

                            window.open(
                                "AccountingPeriod.aspx?KeyId=" + keyId,
                                "List",
                                "scrollbars=yes,resizable=no,width=" + width + ",height=" + height +
                                ",top=" + top + ",left=" + left
                            );
                            // window.open("AccountingPeriod.aspx?KeyId=" + keyId, "List", "scrollbars=yes,resizable=no,width=200,height=150");
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
                    body {
                        /* font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif, 'Roboto'; */
                        font-family: 'Muli', Roboto, "Helvetica Neue", Arial, sans-serif !important;
                    }


                    .AspNet-Menu,
                    .aspNetMenuHorizontal,
                    table td,
                    label,
                    .form-control {
                        /* font-family: 'Inter', 'Segoe UI', sans-serif !important; */
                        font-family: 'Muli', Roboto, "Helvetica Neue", Arial, sans-serif !important;
                    }

                    /* Tampilan menu utama */
                    .MenuBar {
                        background-color: #1e51dc;
                        font-family: 'Muli', Roboto, "Helvetica Neue", Arial, sans-serif !important;
                        font-size: 11pt;
                        height: 35px;
                        /* 🔹 tinggi background menu */
                        line-height: 60px;
                        /* 🔹 biar teks rata vertikal */
                        padding: 0 20px;
                        border-bottom: 3px solid #1e51dc;
                        position: relative;
                        z-index: 10;
                        top: -40px;
                        /* margin-top: 0px; */
                        /* jika mau sedikit turun */
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
                        background-color: #1e51dc;
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

                    /* .MenuBar .aspNet-Menu-Horizontal ul  {
                        display: none !important;
                    } */

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
                        /* 0077b6 color sebelumnya */
                        background-color: #1e51dc;
                        color: white;
                         padding: 3px;
                        /* padding: 2px 12px; */
                        font-weight: bold;
                        font-size: 10pt;
                        text-decoration: none;
                        display: inline-block;
                        transition: background-color 0.3s ease, color 0.3s ease;
                        /* animasi halus */
                    }

                    .staticMenu:hover,
                    .staticHover {
                        background-color: #5a8bff;
                        /* 🔹 warna hover */
                        color: white;
                        border-radius: 5px;
                    }

                    .dynamicMenu {
                        background-color: rgb(70, 70, 70);
                        /* border: 1px solid rgb(70, 70, 70); */
                        /* z-index: 9999; */
                        min-width: 160px;
                        /* padding: 8px 10px; */
                        /* 🔹 beris jarak dalam menu */
                        animation: fadeInSlide 0.7s ease;
                        color: #ebebeb;
                        /* box-shadow: 0 3px 6px rgba(0, 0, 0, 0.25);  */
                        /* 🔹 efek melayang */
                        font-size: 10pt;
                    }





                    /* 🔹 Jika item di dalamnya pakai <a> atau <div> */
                    .dynamicMenu a,
                    .dynamicMenu div {
                        display: block;
                        padding: 6px 10px;
                        color: #ebebeb;
                        text-decoration: none;
                        transition: background 0.2s;
                    }

                    /* untuk menghilangkan tanda segitiga */
                    /* .dynamicMenu img {
                        display: none;
                    } */



                    .dynamicMenu a:hover,
                    .dynamicMenu div:hover {
                        background-color: rgba(255, 255, 255, 0.1);
                        border-radius: 6px;
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

                    /* Hapus sisa padding dari icon bawaan ASP.NET */
                    /* Hilangkan gambar bawaan ASP.NET */
                    .MenuBar img {
                        display: none !important;
                    }

                    /* Atur link menu utama agar pakai flex dan spasi antara teks & icon */
                    /* .MenuBar td[style*="white-space:nowrap"] > a {
    display: flex;
    align-items: center;
    justify-content: space-between; 
} */

                    /* Caret putih dari gambar */
                    .MenuBar td[style*="white-space:nowrap"]>a::after {
                        content: "";
                        display: inline-block;
                        width: 10px;
                        height: 10px;
                        background-image: url('Image/right-arrow-white.png');
                        /* ubah path sesuai lokasi */
                        background-size: contain;
                        background-repeat: no-repeat;
                        background-position: center right;
                        margin-left: 20px;
                        transition: transform 0.8s ease;
                    }

                    /* Efek hover untuk caret */
                    .MenuBar td[style*="white-space:nowrap"]:hover>a::after {
                        transform: rotate(180deg);
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
                        border-top: 6px solid #1e51dc;
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

                    @media (max-width: 1024px) {
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
                            border-bottom: 1px solid #1e51dc;
                        }

                        .responsiveMenu a {
                            display: block;
                            padding: 12px;
                            color: white;
                            text-align: left;
                        }
                    }
                </style>



                <script>
                    window.addEventListener('mouseover', function (e) {
                        if (e.target.tagName === 'TD' || e.target.tagName === 'A') {
                            console.log('Hover:', e.target.innerText);
                        }
                    });
                </script>


                <script type="text/javascript">
                    function hidePanelsAndLoadInFrame(url) {
                        try {

                            var pnlDashboard = document.getElementById('<%= pnlDashboard.ClientID %>');
                            var pnlSearch = document.getElementById('<%= pnlSearch.ClientID %>');
                            var pnlTransfer = document.getElementById('<%= PnlTransfer.ClientID %>');
                            var iframe = document.getElementById('InFrame');
                            var spinner = document.getElementById('loadingSpinner');

                            if (pnlDashboard) pnlDashboard.style.display = 'none';
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


                <style>
                    /* Toggle Button */
                    /* #mobileMenuToggle {
                        display: none;
                        position: absolute;
                        
                        top: 60px;
                       
                        right: 324px;
                        
                        background-color: #333;
                        color: white;
                        padding: 7px 12px;
                        font-size: 14px;
                        cursor: pointer;
                        border-radius: 6px;
                        z-index: 10000;
                       
                        transition: all 0.3s ease;
                        
                    } */
                    #mobileMenuToggle {
                        position: absolute;
                        display: none;
                        /* top:7px; */
                        right: 20px;
                        /* background-color: #333; */
                        color: white;
                        /* font-size: 18px; */
                        cursor: pointer;
                        border-radius: 4px;
                        /* padding: 7px 10px; */
                        z-index: 9999;
                        transition: all 1s ease;
                    }

                    /* Sidebar */
                    #mobileSidebar {
                        display: none;
                        position: fixed;
                        top: 0;
                        left: 0;
                        width: 280px;
                        height: 100%;
                        background-color: #1e1e1e;
                        color: white;
                        padding-left: 5px;
                        padding-right: 5px;
                        z-index: 10000;
                        overflow-y: auto;
                        font-size: 8pt;
                        border-top-right-radius: 25px;
                        border-bottom-right-radius: 25px;
                        box-shadow: 2px 0 8px rgba(0, 0, 0, 0.5);

                    }

                    #mobileSidebarClose {
                        font-size: 28px;
                        cursor: pointer;
                        margin-bottom: 20px;
                        text-align: right;
                        display: block;
                        padding-top: 9px;
                        padding-right: 5px;
                        color: #fff;
                        /* sesuaikan warna */
                        transition: transform 0.8s ease;
                    }



                    #mobileSidebar.active {
                        display: block;
                    }

                    @media (max-width: 1024px) {
                        #mobileMenuToggle {
                            display: block;
                        }

                        #mobileMenuToggle {
                            left: 5px;
                            right: auto;
                            top: 7px;
                        }

                        #menuTop {
                            display: none !important;
                        }
                    }

                    @media (max-width: 1024px) {

                        #menuTop,
                        .AspNet-Menu,
                        .AspNet-Menu-Horizontal,
                        .menu-horizontal,
                        .menu-container,
                        table[id*="menuTop"],
                        table.AspNet-Menu,
                        table.AspNet-Menu-Horizontal {
                            display: none !important;
                            visibility: hidden !important;
                            height: 0 !important;
                            overflow: hidden !important;
                            position: absolute !important;
                            z-index: -1 !important;
                        }
                    }



                    /* Menu List Style */
                    .mobile-menu,
                    .submenu {
                        list-style-type: none;
                        padding-left: 0;
                        margin: 0;
                    }

                    .mobile-menu li {
                        margin-bottom: 4px;
                    }

                    .mobile-menu a,
                    .mobile-menu span {
                        color: white;
                        text-decoration: none;
                        display: block;
                        padding: 5px 12px;
                        border-radius: 6px;
                        background-color: #2d2d2d;
                        transition: background-color 0.2s;
                    }

                    .mobile-menu a:hover,
                    .mobile-menu span:hover {
                        background-color: #444;
                    }

                    .has-submenu>span {
                        cursor: pointer;
                        font-weight: 600;
                        position: relative;
                    }

                    .has-submenu>span::after {
                        float: right;
                        margin-left: 10px;
                        transition: transform 0.2s;
                    }

                    .submenu {
                        display: none;
                        padding-left: 15px;
                        margin-top: 6px;
                        border-left: 2px solid #444;
                        margin-left: 10px;
                    }

                    .submenu a {
                        background-color: #3a3a3a;
                        padding-left: 18px;
                        margin-bottom: 6px;
                    }
                </style>


                <!-- Script di bawah </form> -->

                <!-- menu mobile sebelum ada tambahan loading -->
                <!-- <script>
                    function toggleSidebar() {
                        const sidebar = document.getElementById("mobileSidebar");
                        sidebar.classList.toggle("active");
                    }

                    function toggleSubmenu(el) {
                        const submenu = el.nextElementSibling;
                        if (submenu && submenu.classList.contains("submenu")) {
                            const isOpen = submenu.style.display === "block";
                            submenu.style.display = isOpen ? "none" : "block";
                            el.innerHTML = el.innerHTML.replace(isOpen ? "▾" : "▸", isOpen ? "▸" : "▾");
                        }
                    }

                    function handleMenuClick(event) {
                        var pnlSearch = document.getElementById('<%= pnlSearch.ClientID %>');
                        var pnlTransfer = document.getElementById('<%= PnlTransfer.ClientID %>');
                        var sidebar = document.getElementById("mobileSidebar");

                        if (pnlSearch) pnlSearch.style.display = "none";
                        if (pnlTransfer) pnlTransfer.style.display = "none";
                        if (sidebar) sidebar.classList.remove("active");
                    }

                    // Pasang event listener saat dokumen siap
                    window.addEventListener('DOMContentLoaded', function () {
                        document.querySelectorAll('#mobileSidebar a').forEach(function (a) {
                            a.addEventListener('click', handleMenuClick);
                        });
                    });
                </script> -->

                <!-- menu mobile dengan tambahn loading spinner -->
                <script>
                    // === Fungsi Loading Spinner ===
                    function showLoading() {
                        const spinner = document.getElementById('loadingSpinner');
                        if (spinner) spinner.style.display = 'flex';
                    }

                    function hideLoading() {
                        const spinner = document.getElementById('loadingSpinner');
                        if (spinner) spinner.style.display = 'none';
                    }

                    // === Toggle Sidebar (mobile) ===
                    function toggleSidebar() {
                        const sidebar = document.getElementById("mobileSidebar");
                        sidebar.classList.toggle("active");
                    }

                    // === Toggle Submenu ===
                    function toggleSubmenu(el) {
                        const submenu = el.nextElementSibling;
                        if (submenu && submenu.classList.contains("submenu")) {
                            const isOpen = submenu.style.display === "block";
                            submenu.style.display = isOpen ? "none" : "block";
                            el.innerHTML = el.innerHTML.replace(isOpen ? "▾" : "▸", isOpen ? "▸" : "▾");
                        }
                    }

                    // === Klik Menu ===
                    function handleMenuClick(event) {
                        // Jangan ganggu toggle submenu
                        const el = event.target;
                        if (el && el.classList.contains('has-submenu')) {
                            return; // biarkan toggleSubmenu yang jalan
                        }

                        // Sembunyikan panel search dan transfer
                        var pnlDashboard = document.getElementById('<%= pnlDashboard.ClientID %>');
                        var pnlSearch = document.getElementById('<%= pnlSearch.ClientID %>');
                        var pnlTransfer = document.getElementById('<%= PnlTransfer.ClientID %>');
                        var sidebar = document.getElementById("mobileSidebar");

                        if (pnlDashboard) pnlDashboard.style.display = "none";
                        if (pnlSearch) pnlSearch.style.display = "none";
                        if (pnlTransfer) pnlTransfer.style.display = "none";
                        if (sidebar) sidebar.classList.remove("active");

                        // Tampilkan spinner
                        showLoading();
                    }

                    // === Saat dokumen siap ===
                    window.addEventListener('DOMContentLoaded', function () {
                        // Pasang event click ke semua link menu sidebar
                        document.querySelectorAll('#mobileSidebar a').forEach(function (a) {
                            a.addEventListener('click', handleMenuClick);
                        });

                        // Sembunyikan loading setelah iframe selesai load
                        const iframe = document.getElementById('InFrame');
                        if (iframe) {
                            iframe.addEventListener('load', hideLoading);
                        }
                    });
                </script>



                <style>
                    body,
                    input,
                    button,
                    select,
                    textarea {
                        /* font-family: 'Inter', 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; */
                        font-family: 'Muli', Roboto, "Helvetica Neue", Arial, sans-serif !important;
                        font-size: 14px;
                    }

                    .AspNet-Menu,
                    .aspNetMenuHorizontal,
                    table td,
                    label,
                    .form-control {
                        /* font-family: 'Inter', 'Segoe UI', sans-serif !important; */
                        font-family: 'Muli', Roboto, "Helvetica Neue", Arial, sans-serif !important;
                    }

                    /* === Segitiga Dropdown Putih di Menu ASP.NET === */
                    .MenuBar>li>a {
                        position: relative;
                    }

                    /* Tambahkan segitiga putih hanya di menu yang punya submenu */
                    .MenuBar>li>ul {
                        display: none;
                    }

                    .MenuBar>li:hover>ul {
                        display: block;
                    }

                    /* Gunakan !important agar menang dari CSS ASP.NET default */
                    .MenuBar>li>a::after {
                        content: "\25BE";
                        /* ▼ Segitiga bawah */
                        font-size: 11px;
                        color: #ffffff !important;
                        /* Putih pakai !important */
                        margin-left: 6px;
                        vertical-align: middle;
                        transition: transform 0.3s ease;
                    }

                    /* Muncul hanya di menu yang punya submenu */
                    .MenuBar>li:not(:has(ul))>a::after {
                        content: none !important;
                    }

                    /* Saat hover, segitiga berputar ke atas */
                    .MenuBar>li:hover>a::after {
                        transform: rotate(180deg);
                    }
                </style>


                <style>
                    /* ===== HEADER CONTAINER ===== */
                    #topContent {
                        position: relative;
                        background-color: #444444;
                        color: #f8f9fa;
                        font-family: 'Muli', Roboto, Arial, sans-serif !important;
                    }

                    /* Struktur lama tetap dipakai tapi dirapikan */
                    #topContent table {
                        width: 99.4%;
                        border-collapse: collapse;
                    }

                    #topContent tr {
                        display: flex;
                        align-items: center;
                        justify-content: space-between;
                    }

                    #ImgCompanyLogo {
                        border-radius: 50%;
                        object-fit: cover;
                        height: 23px;
                        width: 23px;
                        display: flex;
                        border: solid 5px #fff;
                    }

                    /* ===== LOGO + COMPANY ===== */
                    #lbCompany {
                        display: flex;
                        align-items: center;
                        gap: 4px;
                        font-size: 14px;
                        font-weight: 500;
                        color: #fff !important;
                    }

                    /* #lbCompany::before {
                        content: "IGL";
                        background-color: #097DBC;
                        border: solid 2px #fff;
                        color: #fff;
                        border-radius: 50%;
                        width: 26px;
                        height: 26px;
                        display: flex;
                        align-items: center;
                        justify-content: center;
                        font-size: 11px;
                        font-weight: 600;
                    } */

                    /* ===== PERIOD SECTION (tengah atas) ===== */
                    .Period {
                        position: absolute;
                        top: 0;
                        left: 50%;
                        transform: translate(-50%, 0);
                        background-color: #ffffff;
                        color: #777777;
                        padding: 3px 30px;
                        font-size: 13px;
                        font-weight: 1500;
                        border-bottom-left-radius: 10px;
                        border-bottom-right-radius: 10px;
                        box-shadow: 0 2px 4px rgba(0, 0, 0, 0.15);
                        z-index: 100;
                    }

                    /* ===== RIGHT SECTION (Hi, Admin + Button) ===== */
                    #topContent td:last-child {
                        text-align: right;
                        white-space: nowrap;
                    }

                    #Label1,
                    #lkbHome,
                    #LinkButton1 {
                        font-size: 10pt !important;
                        color: #777777;
                        font-family: 'Muli', Roboto, Arial, sans-serif !important;
                    }

                    #lbUser {
                        font-size: 10pt !important;
                        font-weight: 600;
                        color: #777777 !important;
                    }

                    /* Tombol lama tapi lebih rapat dan sejajar kanan */
                    #topContent .Home,
                    #topContent .Log-Out {
                        display: inline-block;
                        /* margin-left: 6px; */
                        padding: 5px 0px;
                        font-size: 12px;
                        font-weight: 500;
                        color: #fff;
                        border-radius: 4px;
                    }

                    /* #topContent .Home {
    background-color: #0d6efd;
}

#topContent .Log-Out {
    background-color: #dc3545;
} */

                    /* ===== RESPONSIVE ===== */
                    @media (max-width: 1024px) {
                        #topContent {
                            height: 45px;
                            display: flex;
                            flex-direction: column;
                            justify-content: center;
                            /* posisi vertikal tengah */
                            align-items: center;
                            /* posisi horizontal tengah */
                            text-align: center;
                            padding-bottom: 40px;
                        }

                        #topContent tr {
                            flex-direction: column;
                            align-items: center;
                        }

                        #lbCompany {
                            justify-content: center;
                        }

                        #lbCompany::before {
                            width: 0px;
                            height: 0px;
                        }

                        .Period {
                            position: static;
                            transform: none;
                            padding-top: 30px;
                            padding-bottom: 5px;
                            margin-bottom: 6px;
                            border-radius: 10px;
                            font-size: 9pt;
                            z-index: 200;
                            /* width: 235px; */
                        }

                        #topContent td:last-child {
                            text-align: center;
                        }
                    }

                    /* 🔹 Bungkus utama bisa diatur posisinya */
                    .user-menu-container {
                        width: 100%;
                        padding-top: 5px;
                        padding-right: 5px;
                        position: relative;
                        z-index: 25;
                        /* pastikan di atas object lain */
                    }

                    /* 🔹 Dropdown responsif & fleksibel */
                    .user-dropdown {
                        position: relative;
                    }

                    /* Dropdown tetap di atas semua elemen */
                    .dropdown-menu {
                        z-index: 999999 !important;
                    }

                    /* 🔹 Mobile responsive (atur posisi center / kiri di layar kecil) */
                    @media (max-width: 1024px) {
                        .user-menu-container {
                            justify-content: center !important;
                            /* padding: 8px; */
                        }

                        .user-dropdown button {
                            width: 100%;
                            text-align: center;
                        }


                        /* untuk usuan userdropdowndi Hp */
                        .dropdown-menu {
                            /* left: 0 !important;
                            right: 0 !important;
                            margin: 0 auto; */
                            text-align: center;
                            /* transform: translateX(-100%) !important; */
                        }
                    }

                    /* Hilangkan margin global bawaan halaman ASP.NET */
                    html,
                    body,
                    form {
                        margin: 0 !important;
                        padding: 0 !important;
                        border: none !important;
                    }

                    /* Pastikan area di bawah menu menempel langsung */
                    #ForInFrame {
                        margin-top: 0 !important;
                        padding-top: 0 !important;
                        border: none !important;
                    }

                    /* Geser iframe lebih atas */
                    #InFrame {
                        margin: 0 !important;
                        padding: 0 !important;
                        border: none !important;
                        display: block;
                        width: 100%;
                        height: calc(100vh);
                        position: relative;
                        top: -42px;
                        /* geser sedikit ke atas */
                        z-index: 0;
                    }



                    /* #ForInFrame, #InFrame {
    outline: 1px solid red; 
} */
                </style>

                <link href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.11.3/font/bootstrap-icons.css"
                    rel="stylesheet">
                <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/js/bootstrap.bundle.min.js"></script>

                <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" rel="stylesheet">
                <!-- <link href="https://fonts.googleapis.com/css2?family=Rubik:wght@400;500;600;700&display=swap" rel="stylesheet"> -->


                <script type="text/javascript">
                    // Fungsi untuk menampilkan spinner
                    function showLoading() {
                        document.getElementById("loadingSpinner").style.display = "flex";
                    }

                    // Jalankan fungsi ini saat LinkButton diklik
                    window.addEventListener("load", function () {
                        var homeBtn = document.getElementById("<%= lkbHome.ClientID %>");
                        var logoutBtn = document.getElementById("<%= LinkButton1.ClientID %>");

                        var logoutbutton = document.getElementById("<%= Linkbutton1.ClientID %>")

                        if (homeBtn) {
                            homeBtn.addEventListener("click", function () {
                                showLoading();
                            });
                        }

                        if (logoutBtn) {
                            logoutBtn.addEventListener("click", function () {
                                showLoading();
                            });
                        }
                    });
                </script>


                <!-- Style pembungkus untuk panel dashboard -->
                <style>
                    .scroll-container {
                        position: relative;
                        top: -40px;
                        /* tambahkan satuan px di sini */
                        width: 100%;
                        height: calc(100vh - 60px);
                        overflow-y: auto;
                        overflow-x: auto;
                        background-color: #fff;

                        /* Scrollbar untuk browser modern (Chrome, Edge, Safari) */
                        scrollbar-width: thin;
                        /* Firefox: scrollbar tipis */
                        scrollbar-color: #bdbdbd #f1f1f1;
                        /* Firefox: warna thumb & track */
                    }

                    /* Scrollbar untuk WebKit (Chrome, Edge, Safari) */
                    .scroll-container::-webkit-scrollbar {
                        width: 3px;
                        /* ketebalan scrollbar */
                    }

                    .scroll-container::-webkit-scrollbar-track {
                        background: #f1f1f1;
                        /* warna latar belakang track */
                    }

                    .scroll-container::-webkit-scrollbar-thumb {
                        background-color: #bdbdbd;
                        /* warna thumb */
                        border-radius: 10px;
                        /* biar lembut ujungnya */
                    }

                    .scroll-container::-webkit-scrollbar-thumb:hover {
                        background-color: #9e9e9e;
                        /* sedikit lebih gelap saat hover */
                    }


                    .dashboard-container {
                        display: flex;
                        /* justify-content: space-between; */
                        /* beri jarak di antara dua panel */
                        align-items: flex-start;
                        /* biar sejajar atas */
                        gap: 0px;
                        padding-bottom: 20px;
                        /* jarak antar panel */
                        /* margin-top: 10px; */
                    }

                    /* Biar masing-masing panel punya ukuran proporsional */
                    #pnlSearch,
                    #pnlDateExpired {
                        flex: 1;
                        /* dua-duanya ambil lebar seimbang */

                    }

                    #pnlSearch {
                        /* left: -150px; */
                        max-width: 30%;

                    }

                    #pnlDateExpired {
                        /* left: -150px; */
                        max-width: 50%;
                    }



                    /* Responsif: tumpuk ke bawah jika layar kecil */
                    @media (max-width: 992px) {
                        .dashboard-container {
                            flex-direction: column;
                        }

                        #pnlSearch,
                        #pnlDateExpired,
                        #pnlCIPAdministrasi {
                            max-width: 100%;

                        }

                        #pnlSearch,
                        #pnlDateExpired {
                            padding-top: 20px;
                        }
                    }
                </style>

                <!-- End Style pembungkus untuk panel dashboard------------------------ -->


            </head>


            <body style="margin: 0px; overflow: visible;">

                
    
                <form id="form" runat="server">
                    <asp:ScriptManager ID="ScriptManager1" runat="server" />
                    <!-- Container pembungkus fleksibel -->
                    <div class="user-menu-container d-flex justify-content-end align-items-center flex-wrap">

                        <div class="dropdown user-dropdown">
                            <button class="btn btn-dark btn-sm dropdown-toggle" type="button" id="userMenuButton"
                                data-bs-toggle="dropdown" aria-expanded="false">
                                <i class="bi bi-gear-fill"></i>
                            </button>
                            <ul class="dropdown-menu dropdown-menu-end" aria-labelledby="userMenuButton">
                                <li class="dropdown-item text-center text-info fw-bold">
                                    <i class="bi bi-person-circle"></i>
                                    <asp:Label ID="Label1" runat="server" Text="Hi, " />
                                    <asp:Label ID="lbUser" runat="server" Text="Administrator" />
                                </li>
                                <hr class="dropdown-divider">
                                <li>
                                    <asp:LinkButton CssClass="dropdown-item" ID="lkbHome" runat="server">
                                        <i class="bi bi-house-door-fill"></i> Home
                                    </asp:LinkButton>
                                </li>

                                <li>
                                    <asp:LinkButton CssClass="dropdown-item text-danger" ID="LinkButton1"
                                        runat="server">
                                        <i class="bi bi-box-arrow-right"></i> Log Out
                                    </asp:LinkButton>
                                </li>
                            </ul>
                        </div>

                    </div>


                    <div id="topContent">

                        <div class="offcanvas-body p-10 pt-1 p-lg-10">
                            <hr class="d-lg-none text-white-50">
                            <ul class="navbar-nav flex-row flex-wrap bd-navbar-nav">
                                <li class="nav-item col-6 col-lg-auto"> <a class="nav-link py-2 px-0 px-lg-1"
                                        href="/docs/5.3/getting-started/introduction/">
                                        <asp:Image ID="imgCompanyLogo" runat="server" Style="border:2px solid #ffffff;"
                                            ImageUrl="Image/LogoDP.jpg" />

                                    </a> </li>
                                <li class="nav-item col-6 col-lg-auto"> <a aria-current="true"
                                        class="nav-link py-2 px-0 px-lg-1 active" href="#">
                                        <asp:Label ID="lbCompany" runat="server" />
                                    </a> </li>
                            </ul>

                        </div>

                        <div class="Period">
                            <asp:Label style="font-family: 'Muli', Roboto, Arial, sans-serif !important;" Text="Period"
                                Onclick="OpenAcc(); return false;" id="someID" runat="server" />

                            <asp:Label style="font-family: 'Muli', Roboto, Arial, sans-serif !important;" ID="lbYear"
                                runat="server" />

                        </div>




                        <div id="mobileMenuToggle" class="btn btn-dark" onclick="toggleSidebar()">
                            <i class="bi bi-list"></i>
                        </div>


                    </div>


                    <div id="mobileSidebar">
                        <div id="mobileSidebarClose" onclick="toggleSidebar()"><i class="bi bi-x-circle"></i></div>
                        <div id="mobileMenuContainer">
                            <%= ViewState("MobileMenuHtml") %>
                        </div>
                    </div>


                    <div id="topContent2">
                        <div id="myMenuContainer" runat="server"></div>
                        <asp:Menu ID="menuTop" runat="server" Orientation="Horizontal" RenderingMode="List"
                            StaticDisplayLevels="1" MaximumDynamicDisplayLevels="3"
                            StaticMenuItemStyle-CssClass="staticMenu" StaticHoverStyle-CssClass="staticHover"
                            DynamicMenuItemStyle-CssClass="dynamicMenu" DynamicHoverStyle-CssClass="dynamicHover"
                            CssClass="MenuBar menu-container responsiveMenu">
                        </asp:Menu>

                    </div>
                    
                        
                    <asp:Panel runat="server" ID="pnlDashboard" CssClass="scroll-container">

                       <asp:Panel ID="pnlCard" runat="server" visible="True" CssClass="container-fluid mt-3">

                                <!--  Card animation -->
                            <style>

                                /* Hidden default */
                                .card-animate {
                                    opacity: 0;
                                    transform: translateY(-20px);
                                    transition: all 0.8s ease;
                                }

                                /* After animation */
                                .card-animate.show {
                                    opacity: 1;
                                    transform: translateY(0);
                                }

                            </style>

                            <script>
                                function openMenu(url, menuId) {
                                    var finalUrl = url + "?KeyId=" + '<%= ViewState("KeyId") %>' + "&ContainerId=" + menuId;

                                    // ⬅ langsung panggil fungsi loading + load iframe
                                    hidePanelsAndLoadInFrame(finalUrl);
                                }

                                // Card animation
                                document.addEventListener("DOMContentLoaded", function () {

                                    const cards = document.querySelectorAll(".card-animate");
                                    let delay = 100;

                                    cards.forEach(card => {
                                        setTimeout(() => {
                                            card.classList.add("show");
                                        }, delay);

                                        delay += 150; // Delay antar card (biar muncul satu-satu)
                                    });

                                });
                            </script>

                            <!-- Row Utama -->
                            <div class="row">

                                <!-- === Sidebar kiri (Fast Menu) === -->
                                <div class="col-md-3 col-lg-2 mb-3">
                                    <div class="card shadow-sm border-0 card-animate">
                                        <div
                                            class="card-header bg-light d-flex justify-content-between align-items-center py-2">
                                            <strong>Fast Menu To Be Approve</strong>
                                            <i class="fa fa-bars"></i>
                                        </div>

                                        <style>
                                            .list-group-item:hover {
                                                background:#f0f0f0;
                                                border-radius: 5px;
                                            }
                                        </style>

                                        <div class="card-body p-0 ">
                                            <ul id="ulDashboard" runat="server" class="list-group list-group-flush small"></ul>
                                        </div>
                                    </div>
                                </div>

                                <!-- === Area kanan (card summary + konten utama) === -->
                                <div class="col-md-9 col-lg-10">

                                    <!-- === Card Summary Row === -->
                                    <div class="row g-3">

                                        <div class="col-sm-6 col-md-3">
                                            <div class="card shadow-sm border-0 card-animate">
                                                <div class="card-body p-2">
                                                    <!-- Judul Atas -->
                                                    <h6>Pending Voucher Approval</h6>

                                                    <!-- Teks Info (Kanan Tengah, lebih besar) -->
                                                    <div class="position-absolute card-rubik text-primary">
                                                        /100
                                                    </div>

                                                    <!-- Tombol Bawah -->
                                                    <div class="text-left mt-5">
                                                        <button class="btn btn-Primary btn-sm rounded-pill px-2">Lihat
                                                            Transaksi</button>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>



                                        <div class="col-sm-6 col-md-3">
                                            <div class="card shadow-sm border-0 card-animate">
                                                <div class="card-body p-2">
                                                    <!-- Judul Atas -->
                                                    <h6>Pending AR Receive</h6>

                                                    <!-- Teks Info (Kanan Tengah, lebih besar) -->
                                                    <div class="position-absolute card-rubik text-danger">
                                                        /100
                                                    </div>

                                                    <!-- Tombol Bawah -->
                                                    <div class="text-left mt-5">
                                                        <button class="btn btn-danger btn-sm rounded-pill px-2">Lihat
                                                            Transaksi</button>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="col-sm-6 col-md-3">
                                            <div class="card shadow-sm border-0 card-animate">
                                                <div class="card-body p-2">
                                                    <!-- Judul Atas -->
                                                    <h6>Pending Purchase Order</h6>

                                                    <!-- Teks Info (Kanan Tengah, lebih besar) -->
                                                    <div class="position-absolute card-rubik text-warning">
                                                        /100
                                                    </div>

                                                    <!-- Tombol Bawah -->
                                                    <div class="text-left mt-5">
                                                        <button class="btn btn-warning btn-sm rounded-pill px-2">Lihat
                                                            Transaksi</button>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>


                                         <div class="col-sm-6 col-md-3">
                                            <div class="card shadow-sm border-0 card-animate">
                                                <div class="card-body p-2">
                                                    <!-- Judul Atas -->
                                                    <h6>Pending Supplier Invoice</h6>

                                                    <!-- Teks Info (Kanan Tengah, lebih besar) -->
                                                    <div class="position-absolute card-rubik text-success">
                                                        /100
                                                    </div>

                                                    <!-- Tombol Bawah -->
                                                    <div class="text-left mt-5">
                                                        <button class="btn btn-success btn-sm rounded-pill px-2">Lihat
                                                            Transaksi</button>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>


                                    </div>

                                    <!-- === Area konten utama di bawah === -->
                                    <div class="card shadow-sm border-0 mt-3 card-animate">
                                        <div class="card-body" style="height: 350px;">
                                            <!-- Konten dashboard, grafik, atau iframe bisa diletakkan di sini -->
                                        </div>
                                    </div>

                                </div>
                            </div>

                       </asp:Panel>


                        <!-- Bungkus dua panel dalam 1 container -->
                        <div class="dashboard-container">
                            <asp:Panel runat="server" ID="pnlSearch" Visible="True"
                                style="top: 10px; position: relative; padding-left: 10px; padding-right: 10px;">
                                <table>

                                    <tr>
                                        <td
                                            style=" text-align: center; background-color:#fcfcfc;border-radius: 5px; box-shadow: 0 1px 5px rgba(104, 104, 104, 0.2);">
                                            <asp:Label style="padding:0px;" Height="40" ForeColor="#4a4a4a"
                                                Font-Size="21.5" Font-Bold="true" Text="Approval Dashboard"
                                                runat="server" />
                                        </td>

                                    </tr>

                                    <tr>
                                        <td style="padding: 3px;">

                                        </td>
                                    </tr>
                                    <tr>
                                        <td
                                            style="border: 0px solid #8b8c8c;background-color:#fcfcfc; border-radius:5px; box-shadow: 0 3px 10px rgb(0 0 0 / 0.2); padding:5px;">
                                            <asp:GridView id="DataGrid" runat="server" AllowPaging="True"
                                                CssClass="Grid" AutoGenerateColumns="false" PageSize="5">
                                                <HeaderStyle CssClass="GridHeader" Font-Size="11.5" Font-Bold="true"
                                                    Wrap="false"></HeaderStyle>
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

                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Left"
                                                        HeaderText="Menu Name" HeaderStyle-Width="300">
                                                        <Itemtemplate>
                                                            <asp:Label style="font-size: 14px;" Runat="server"
                                                                ID="MenuName"
                                                                text='<%# DataBinder.Eval(Container.DataItem, "MenuName") %>'>
                                                            </asp:Label>
                                                        </Itemtemplate>

                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="To be Approve"
                                                        HeaderStyle-Width="200" ItemStyle-HorizontalAlign="Center">
                                                        <Itemtemplate>
                                                            <asp:LinkButton ID="btnLink" runat="server" Visible="true"
                                                                CommandName="View"
                                                                Text='<%# DataBinder.Eval(Container.DataItem, "Outstanding") %>'
                                                                CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>"
                                                                Style="font-size:11pt; text-decoration: none; color: #1e51dc; font-weight: 800; ">
                                                            </asp:LinkButton>

                                                            <%-- <asp:Label Runat="server" ID="Outstanding"
                                                                text='<%# DataBinder.Eval(Container.DataItem, "Outstanding") %>'>--%>
                                                                </asp:Label>
                                                        </Itemtemplate>

                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="To be Approve"
                                                        HeaderStyle-Width="200" Visible="false">
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
                                                                    <%# DataBinder.Eval(Container.DataItem, "MenuUrl" )
                                                                        %>
                                                                </a>
                                                            </asp:Label>
                                                        </Itemtemplate>

                                                        </asp:TemplateField>--%>




                                                        <%--<asp:TemplateField HeaderText="Action"
                                                            headerstyle-width="126">
                                                            <ItemTemplate>
                                                                <asp:Button class="bitbtn btnedit" Width="150Px"
                                                                    runat="server" ID="btnLink" Text="Go to Approve"
                                                                    CommandName="View"
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

                            
                            <asp:Panel runat="server" ID="pnlDateExpired" Visible="True"
                                style="top: 10px; position: relative; padding-left: 10px; padding-right: 10px;">                                
                                <asp:UpdatePanel ID="updExpired" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <!-- Auto refresh setiap 10 detik , di tutup dahulu karena kalau di hp jadi flicker-->
                                        <!-- <asp:Timer ID="tmExpired" runat="server" Interval="5000" /> -->

                                            <table>

                                                <tr>
                                                    <td
                                                        style=" text-align: center; background-color:#fcfcfc;border-radius: 5px; box-shadow: 0 1px 5px rgba(104, 104, 104, 0.2);">
                                                        <asp:Label style="padding:0px;" Height="40" ForeColor="#4a4a4a"
                                                            Font-Size="21.5" Font-Bold="true" Text="Licence & Administration Expiration Date" runat="server" />
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <td style="padding: 3px;">
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <td
                                                        style="border: 0px solid #8b8c8c;background-color:#fcfcfc; border-radius:5px; box-shadow: 0 3px 10px rgb(0 0 0 / 0.2); padding:5px;">
                                                        <asp:GridView id="DataGridDateExpired" runat="server" AllowPaging="True"
                                                            CssClass="Grid" AutoGenerateColumns="false" PageSize="5">
                                                            <HeaderStyle CssClass="GridHeader" Font-Size="11.5" Font-Bold="true"
                                                                Wrap="false"></HeaderStyle>
                                                            <RowStyle CssClass="GridItem" Wrap="false" />
                                                            <AlternatingRowStyle CssClass="GridAltItem" />
                                                            <PagerStyle CssClass="GridPager" />
                                                            <EmptyDataTemplate>

                                                            </EmptyDataTemplate>
                                                            <Columns>

                                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left"
                                                                    HeaderText="Kegiatan" HeaderStyle-Width="330">
                                                                    <Itemtemplate>
                                                                        <asp:Label style="font-size: 14px;" Runat="server"
                                                                            ID="lbKegiatanName"
                                                                            text='<%# DataBinder.Eval(Container.DataItem, "KegiatanName") %>'>
                                                                        </asp:Label>
                                                                    </Itemtemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left"
                                                                    HeaderText="No Dokumen" HeaderStyle-Width="220">
                                                                    <Itemtemplate>
                                                                        <asp:Label style="font-size: 14px;" Runat="server"
                                                                            ID="lbNoDokumen"
                                                                            text='<%# DataBinder.Eval(Container.DataItem, "NoDokumen") %>'>
                                                                        </asp:Label>
                                                                    </Itemtemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left"
                                                                    HeaderText="Date" HeaderStyle-Width="100">
                                                                    <Itemtemplate>
                                                                        <asp:Label style="font-size: 14px;" Runat="server"
                                                                            ID="lbEndDate"
                                                                            text='<%# DataBinder.Eval(Container.DataItem, "EndDate") %>'>
                                                                        </asp:Label>
                                                                    </Itemtemplate>
                                                                </asp:TemplateField>


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
                                         </ContentTemplate>
                                    </asp:UpdatePanel> 
                           
                            </asp:Panel>
                           
                        </div>

                        <asp:Panel runat="server" ID="pnlCIPAdministrasi" Visible="True"
                            style="top: 10px; position: relative; padding-left: 10px; padding-right: 10px">
                            <table>

                                <tr>
                                    <td
                                        style=" text-align: center; background-color:#fcfcfc;border-radius: 5px; box-shadow: 0 1px 5px rgba(104, 104, 104, 0.2);">
                                        <asp:Label style="padding:0px;" Height="40" ForeColor="#4a4a4a" Font-Size="21.5"
                                            Font-Bold="true" Text="License & Administration Monitoring" runat="server" />
                                    </td>

                                </tr>

                                <tr>
                                    <td style="padding: 3px;">

                                    </td>
                                </tr>
                                <tr>

                                    <td
                                        style="border: 0px solid #8b8c8c;background-color:#fcfcfc; border-radius:5px; box-shadow: 0 3px 10px rgb(0 0 0 / 0.2); padding:5px;">
                                        <asp:GridView id="DataGridCIP" runat="server" AllowPaging="True" CssClass="Grid"
                                            AutoGenerateColumns="false" PageSize="5">
                                            <HeaderStyle CssClass="GridHeader" Font-Size="11.5" Font-Bold="true"
                                                Wrap="false"></HeaderStyle>
                                            <RowStyle CssClass="GridItem" Wrap="false" />
                                            <AlternatingRowStyle CssClass="GridAltItem" />
                                            <PagerStyle CssClass="GridPager" />
                                            <EmptyDataTemplate>

                                            </EmptyDataTemplate>
                                            <Columns>

                                                <asp:TemplateField HeaderText="No Transaksi" HeaderStyle-Width="110"
                                                    Visible="true">
                                                    <Itemtemplate>
                                                        <asp:Label style="font-size: 14px;" Runat="server"
                                                            ID="NoPenyerahanID"
                                                            text='<%# DataBinder.Eval(Container.DataItem, "TransNmbr") %>'>
                                                        </asp:Label>
                                                    </Itemtemplate>

                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center"
                                                    HeaderText="Kegiatan" HeaderStyle-Width="340">
                                                    <Itemtemplate>
                                                        <asp:Label style="font-size: 14px;" Runat="server" ID="Kegiatan"
                                                            text='<%# DataBinder.Eval(Container.DataItem, "KegiatanName") %>'>
                                                        </asp:Label>
                                                    </Itemtemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center"
                                                    HeaderText="Nama Step" HeaderStyle-Width="300">
                                                    <Itemtemplate>
                                                        <asp:Label style="font-size: 14px;" Runat="server" ID="Step"
                                                            text='<%# DataBinder.Eval(Container.DataItem, "TahapanProses") %>'>
                                                        </asp:Label>
                                                    </Itemtemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Progress (%)" HeaderStyle-Width="200"
                                                    Visible="True">
                                                    <Itemtemplate>
                                                        <asp:Label style="font-size: 14px;" Runat="server"
                                                            ID="ProgressPercen"
                                                            text='<%# DataBinder.Eval(Container.DataItem, "ProgressPercen") %>'>
                                                        </asp:Label>
                                                    </Itemtemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="PIC" HeaderStyle-Width="200"
                                                    Visible="True">
                                                    <Itemtemplate>
                                                        <asp:Label style="font-size: 14px;" Runat="server" ID="PIC"
                                                            text='<%# DataBinder.Eval(Container.DataItem, "PICName") %>'>
                                                        </asp:Label>
                                                    </Itemtemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Date Transaksi" HeaderStyle-Width="200"
                                                    Visible="False">
                                                    <Itemtemplate>
                                                        <asp:Label style="font-size: 14px;" Runat="server"
                                                            ID="lbTransDate"
                                                            text='<%# DataBinder.Eval(Container.DataItem, "TransDate") %>'>
                                                        </asp:Label>
                                                    </Itemtemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Total Hari" HeaderStyle-Width="200"
                                                    Visible="False">
                                                    <Itemtemplate>
                                                        <asp:Label style="font-size: 14px;" Runat="server"
                                                            ID="lbTotalHari"
                                                            text='<%# DataBinder.Eval(Container.DataItem, "TotalHari") %>'>
                                                        </asp:Label>
                                                    </Itemtemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Tanggal Hari Ini" HeaderStyle-Width="200"
                                                    Visible="False">
                                                    <Itemtemplate>
                                                        <asp:Label style="font-size: 14px;" Runat="server" ID="lbharini"
                                                            text='<%# DataBinder.Eval(Container.DataItem, "TanggalHariIni") %>'>
                                                        </asp:Label>
                                                    </Itemtemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Tanggal Hari Ini" HeaderStyle-Width="200"
                                                    Visible="False">
                                                    <Itemtemplate>
                                                        <asp:Label style="font-size: 14px;" Runat="server" ID="ItemNo"
                                                            text='<%# DataBinder.Eval(Container.DataItem, "ItemNo") %>'>
                                                        </asp:Label>
                                                    </Itemtemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Tanggal Hari Ini" HeaderStyle-Width="200"
                                                    Visible="False">
                                                    <Itemtemplate>
                                                        <asp:Label style="font-size: 14px;" Runat="server" ID="ItemNoDt"
                                                            text='<%# DataBinder.Eval(Container.DataItem, "ItemNoDt2") %>'>
                                                        </asp:Label>
                                                    </Itemtemplate>
                                                </asp:TemplateField>


                                                <asp:TemplateField HeaderText="Keterangan Keterlambatan" HeaderStyle-Width="200"
                                                    Visible="false">
                                                    <Itemtemplate>
                                                        <asp:Label style="font-size: 14px;" Runat="server" ID="lbKeterlambatan"
                                                            text='<%# DataBinder.Eval(Container.DataItem, "AlasanKeterlambatan") %>'>
                                                        </asp:Label>
                                                    </Itemtemplate>
                                                </asp:TemplateField>


                                                <asp:TemplateField HeaderText="Fg Permanent" 
                                                    Visible="False">
                                                    <Itemtemplate>
                                                        <asp:Label style="font-size: 14px;" Runat="server" ID="lbfgPermanent"
                                                            text='<%# DataBinder.Eval(Container.DataItem, "FgPermanent") %>'>
                                                        </asp:Label>
                                                    </Itemtemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Action" headerstyle-width="126">
                                                    <ItemTemplate>
                                                        <asp:Button class="bitbtn btnedit" runat="server" ID="btnUpdate"
                                                            Text="Update" CommandName="ViewUpdate"
                                                            CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>"
                                                            CausesValidation="false" UseSubmitBehavior="false" />

                                                        <asp:Button class="bitbtn btnedit" runat="server" ID="btnFinish"
                                                            Text="finish" CommandName="ViewFinish"
                                                            CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>"
                                                            />

                                                        <asp:Button class="bitbtn btnedit" runat="server" ID="btnDetail"
                                                            Text="Detail" CommandName="ViewDetail"
                                                            CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>"
                                                            CausesValidation="false" UseSubmitBehavior="false" />

                                                        <asp:Button class="bitbtn btnedit" runat="server"
                                                            ID="btnVerified" Text="Verified" CommandName="ViewVerified"
                                                            CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>"
                                                            OnClientClick="return confirm('apakah anda yakin akan verified step ini ?');"
                                                            />
                                                    </ItemTemplate>

                                                </asp:TemplateField>

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
                                            ForeColor="#4a4a4a" Font-Size="20" Font-Bold="true"
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

                    </asp:Panel>
                    
                    <asp:Panel runat="server" ID="ForInFrame" Visible="True">
                        <iframe name="InFrame" id="InFrame" style="width:100%;height:89%; border: none; "></iframe>
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
                        <img src="Image/loader.gif" alt="" />
                    </div>

                    <div id="loadingSpinner"
                        style="display:none; position:fixed; z-index:9999; top:0; left:0; width:100vw; height:100vh; background:rgba(0,0,0,0.3); justify-content:center; align-items:center;">
                        <div
                            style="border:6px solid #f3f3f3; border-top:6px solid #1e51dc; border-radius:50%; width:50px; height:50px; animation:spin 0.8s linear infinite;">
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



                        <div class="untukModalCIPAdministrasi">
                            <!--------------------------- untuk modal pnlCIP ---------------------------------- -->
                            <style>
                            
                            </style>

                            <!-- === Modal Custom Detail === -->
                            <div id="customModal" class="custom-modal">
                                <div class="custom-modal-content">
                                    <div class="custom-modal-header">
                                        <h6>Detail Tahapan</h6>
                                        <span class="close-btn" onclick="closeCustomModal()">&times;</span>
                                    </div>
                                    <div class="custom-modal-body">
                                        <table>
                                            <tr>
                                                <td>Tanggal Hari Ini</td>
                                                <td>:</td>
                                                <td>
                                                    <asp:Label ID="lbtglHariIni" runat="server" Text="No penyerahan" />
                                                </td>
                                            </tr>

                                            <tr>
                                                <td></td>
                                            </tr>

                                            <tr>
                                                <td>Detail Transaksi</td>
                                                <td>:</td>
                                                <td>
                                                    <asp:Label ID="lbNopenyerahan" runat="server"
                                                        Text="No penyerahan" />
                                                </td>

                                                <td>&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; Kegiatan</td>
                                                <td>:</td>
                                                <td>
                                                    <asp:Label ID="lbKegiatan" runat="server" Text="Kegiatan" />
                                                </td>

                                                <td>&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                                                </td>
                                                <td>
                                                    <asp:Label ID="lbLamaProsesBerlangsung" runat="server"
                                                        Text="Lama Proses Berlangsung" />
                                                </td>

                                            </tr>
                                            <tr>
                                                <td></td>
                                            </tr>
                                        </table>
                                        <asp:GridView ID="GridDetail" runat="server" CssClass="Grid"
                                            AutoGenerateColumns="false" AllowPaging="false">
                                            <HeaderStyle CssClass="GridHeader" />
                                            <RowStyle CssClass="GridItem" Wrap="false" />
                                            <AlternatingRowStyle CssClass="GridAltItem" />
                                            <Columns>
                                                <asp:BoundField DataField="StepProsess" HeaderText="Step" />
                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center"
                                                    HeaderText="Nama Step" HeaderStyle-Width="400">
                                                    <Itemtemplate>
                                                        <asp:LinkButton ID="btnLinkComment" runat="server"
                                                            Visible="true" CommandName="ViewLogComment"
                                                            Text='<%# DataBinder.Eval(Container.DataItem, "Tahapan") %>'
                                                            CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>"
                                                            Style="font-size:9pt; text-decoration: none; color: #1e51dc; font-weight: 800; ">
                                                        </asp:LinkButton>
                                                    </Itemtemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Nama Menu" Visible="False">
                                                    <ItemTemplate>
                                                        <a href="javascript:void(0) "
                                                            onmouseover='<%# "showMenuTooltip(this, &quot;" & Eval("Tahapan").ToString().Replace("""", "&quot;") & "&quot;, &quot;" & Eval("AlasanKeterlambatan").ToString().Replace("""", "&quot;") & "&quot;)" %>'
                                                            onmouseout="hideMenuTooltip()"
                                                            style="font-size:11pt; text-decoration:none; color:#1e51dc; font-weight:800;">
                                                            <%# Eval("Tahapan") %>
                                                        </a>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:BoundField DataField="Percen" HeaderText="%" />
                                                <asp:BoundField DataField="Biaya1" HeaderStyle-Width="100px"
                                                    ItemStyle-HorizontalAlign="Right" HeaderText="Biaya (A)"
                                                    DataFormatString="{0:#,##0.00}" />
                                                <asp:BoundField DataField="Biaya2" HeaderStyle-Width="100px"
                                                    ItemStyle-HorizontalAlign="Right" HeaderText="Biaya (B)"
                                                    DataFormatString="{0:#,##0.00}" />
                                                <asp:BoundField DataField="TargetWaktu" HeaderStyle-Width="80px"
                                                    ItemStyle-HorizontalAlign="Center" HeaderText="Target Waktu (Hari)"
                                                    DataFormatString="{0:#,##0}" />

                                                <asp:BoundField DataField="TargetTanggal"
                                                    HeaderStyle-HorizontalAlign="Center"
                                                    ItemStyle-HorizontalAlign="Center"
                                                    DataFormatString="{0:dd MMM yyyy}" HtmlEncode="true"
                                                    HeaderStyle-Width="85px" HeaderText="Target Tanggal">
                                                </asp:BoundField>

                                                <asp:BoundField DataField="Realisasi" DataFormatString="{0:dd MMM yyyy}"
                                                    ItemStyle-HorizontalAlign="Center" HtmlEncode="true"
                                                    HeaderStyle-Width="80px" HeaderText="Realisasi">
                                                </asp:BoundField>

                                                <asp:BoundField DataField="SelisihHari" HeaderStyle-Width="80px"
                                                    ItemStyle-HorizontalAlign="Center"
                                                    HeaderText="Variance Thd Target (Hari)"
                                                    DataFormatString="{0:#,##0}" />

                                                <asp:BoundField DataField="TargetTanggalBaru2"
                                                    HeaderStyle-HorizontalAlign="Center"
                                                    ItemStyle-HorizontalAlign="Center"
                                                    DataFormatString="{0:dd MMM yyyy}" HtmlEncode="true"
                                                    HeaderStyle-Width="85px" HeaderText="Target Tanggal Baru">
                                                </asp:BoundField>

                                                <asp:BoundField DataField="QCVerifiedByName" HeaderText="Verified By" />

                                                <asp:BoundField DataField="QcVerifiedDate"
                                                    DataFormatString="{0:dd MMM yyyy - HH:mm}" HtmlEncode="true"
                                                    HeaderStyle-Width="80px" HeaderText="Verified Date">
                                                </asp:BoundField>

                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" Visible="False"
                                                    HeaderText="Nama Step" HeaderStyle-Width="300">
                                                    <Itemtemplate>
                                                        <asp:Label ID="lbLog" runat="server"
                                                            Text='<%# DataBinder.Eval(Container.DataItem, "AlasanKeterlambatan") %>'>
                                                        </asp:Label>
                                                    </Itemtemplate>
                                                </asp:TemplateField>

                                            </Columns>
                                        </asp:GridView>
                                        <!-- TextBox untuk menampilkan ID -->
                                        <asp:TextBox ID="txtComment" runat="server" CssClass="TextBox"
                                            Textmode="MultiLine" Width="500px" height="150" Visible="False"
                                            ReadOnly="True" placeholder="Komentar / ID akan tampil di sini">
                                        </asp:TextBox>
                                    </div>
                                    <!-- <div class="custom-modal-footer">
                                    <button type="button" onclick="closeCustomModal()">Tutup</button>
                                </div> -->
                                </div>
                            </div>

                            <div id="menuTooltip" class="menu-tooltip"></div>
                            <script>
                                // === JS Control ===
                                function showCustomModal() {
                                    document.getElementById("customModal").style.display = "flex";
                                }
                                function closeCustomModal() {
                                    document.getElementById("customModal").style.display = "none";
                                }

                                // Klik di luar modal untuk menutup
                                window.onclick = function (event) {
                                    const modal = document.getElementById("customModal");
                                    if (event.target === modal) {
                                        closeCustomModal();
                                    }

                                    const modal2Update = document.getElementById("customModalUpdate");
                                    if (event.target === modal2Update) {
                                        closeCustomModalUpdate();
                                    }

                                    const modal3Update = document.getElementById("customModalFinish");
                                    if (event.target === modal3Update) {
                                        closeCustomModalFinish();
                                    }

                                    const modal4Update = document.getElementById("customModalFlagY");
                                    if (event.target === modal4Update) {
                                        closeCustomModalFlagY();
                                    }
                                    
                                };
                            </script>

                            <script type="text/javascript">
                                function showMenuTooltip(el, Tahapan, AlasanKeterlambatan) {
                                    console.log("Hover:", Tahapan, AlasanKeterlambatan);
                                    var tooltip = document.getElementById("menuTooltip");
                                    if (!tooltip) {
                                        console.errsor("menuTooltip div not found!");
                                        return;
                                    }
                                    tooltip.innerHTML = "<b>" + Tahapan + "</b><br> " + AlasanKeterlambatan; //Ambil dari alasan keterlambatan dari link grid tahapan yang sekarang di hide

                                    var rect = el.getBoundingClientRect();
                                    tooltip.style.left = (rect.left + window.scrollX + 20) + "px";
                                    tooltip.style.top = (rect.top + window.scrollY - 10) + "px";

                                    tooltip.classList.add("show");
                                }


                                function hideMenuTooltip() {
                                    var tooltip = document.getElementById("menuTooltip");
                                    tooltip.classList.remove("show");
                                }

                            </script>
                            <!--------------------- end modal CIP Detail ------------------------------->

                        </div>


                        <div class="UntukModalUpdate">
                            <!--------------------- Start modal CIP Update ------------------------------->
                            <!-- === Modal Custom Update === -->
                            <div id="customModalUpdate" class="custom-modal">
                                <div class="custom-modal-content">
                                    <div class="custom-modal-header">
                                        <h6>Update Tahapan</h6>
                                        <span class="close-btn" onclick="closeCustomModalUpdate()">&times;</span>
                                    </div>
                                    <div class="custom-modal-body">
                                        <asp:TextBox ID="tbLog" runat="server" Enabled = "False" CssClass="TextBoxR"
                                            TextMode="MultiLine" Width="490px" Height="150px"
                                            placeholder="Masukkan keterangan di sini..." />
                                            <br>
                                            <br>
                                        <asp:TextBox ID="tbKeterangan" MaxLength="500" runat="server" CssClass="TextBox"
                                            Width="490px"
                                            placeholder="Masukkan keterangan di sini..." />
                                    </div>
                                    <div class="custom-modal-footer">
                                        <asp:Button ID="btnUpdateTahapan" runat="server" class="bitbtndt btnsave"
                                            Text="Update Keterangan" width="200" />
                                    </div>
                                </div>
                            </div>

                            <script type="text/javascript">
                                function showCustomModalUpdate() {
                                    document.getElementById('customModalUpdate').style.display = 'flex';
                                }

                                function closeCustomModalUpdate() {
                                    document.getElementById('customModalUpdate').style.display = 'none';
                                }

                                // Klik di luar modal untuk menutup
                                // window.onclick = function (event) {
                                    
                                // };
                            </script>
                            <!--------------------- End modal CIP Update ------------------------------->
                        </div>



                         <div class="UntukModalFinish">
                            <!--------------------- Start modal CIP Finish Flag N ------------------------------->
                            <!-- === Modal Custom Update === -->
                            <div id="customModalFinish" class="custom-modal">
                                <div class="custom-modal-content">

                                    <!-- <div class="custom-modal-header">
                                        <h6>Update Tahapan</h6>
                                        <span class="close-btn" onclick="closeCustomModalFinish()">&times;</span>
                                    </div> -->

                                    <div class="custom-modal-body">
                                        <asp:Label ID="lbNotifFInish" runat="server" style="text-align: center; font-size: 11pt; padding: 0px;" Text="finish" />
                                    </div>

                                    <div class="custom-modal-footer">
                                        <asp:Button ID="btnCancelFinish" style="background-color: rgb(165, 35, 61);" OnClientClick="closeCustomModalFinish()" runat="server" class="bitbtndt btncancel"
                                            Text="Cancel" width="100" />
                                        <asp:Button ID="btnConfirmVerify" runat="server" class="bitbtndt btnsave"
                                            Text="Ok" width="100" />
                                            
                                    </div>
                                </div>
                            </div>
                            
                            <script type="text/javascript">
                                function showCustomModalFinish() {
                                    document.getElementById('customModalFinish').style.display = 'flex';
                                }

                                function closeCustomModalFinish() {
                                    document.getElementById('customModalFinish').style.display = 'none';
                                }

                                // Klik di luar modal untuk menutup
                                // window.onclick = function (event) {
                                    
                                // };
                            </script>
                            <!--------------------- End modal CIP Finish Flag N ------------------------------->
                        </div>


                        <div class="UntukModalFlagY">
                            <!--------------------- Start modal CIP FlagY FLag Y ------------------------------->
                            <!-- === Modal Custom Update === -->
                            <div id="customModalFlagY" class="custom-modal">
                                <div class="custom-modal-content">
                                    <div class="custom-modal-header">
                                        <h6>Finish Step</h6>
                                        <span class="close-btn" onclick="closeCustomModalFlagY()">&times;</span>
                                    </div>
                                    
                                    <div class="custom-modal-body">
                                        <asp:GridView ID="GridDetailFinish" runat="server" CssClass="Grid"
                                            AutoGenerateColumns="false" AllowPaging="false">
                                            <HeaderStyle CssClass="GridHeader" />
                                            <RowStyle CssClass="GridItem" Wrap="false" />
                                            <AlternatingRowStyle CssClass="GridAltItem" />
                                            <Columns>
                                               <asp:TemplateField HeaderText="No"  HeaderStyle-Width="50" >
                                                    <ItemTemplate>                                                       
                                                        <asp:Label Runat="server" Width="20px" ID="lbNo"
										                text='<%# Container.DataItemIndex + 1 %>'>
									                    </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                
                                                <asp:TemplateField HeaderText="Type Ijin">
                                                    <ItemTemplate>
                                                       <asp:DropDownList ID="ddlKegiatan" Runat="Server" Width="220px" height = "23px" CssClass="DropDownList" 								    
                                                            DataSourceID="dsKegiatan" DataTextField="KegiatanName" 
                                                            DataValueField="KegiatanCode" 	>
                                                        </asp:DropDownList>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                               
                                                <asp:TemplateField HeaderText="No Dokumen">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="tbNoDok" runat="server" CssClass="TextBox" Width="220px"/>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Perihal">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="tbPerihal" runat="server" CssClass="TextBox" Width="225px" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Instansi">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="tbInstansi" runat="server" CssClass="TextBox" Width="225px" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Tanggal Terbit">
                                                    <ItemTemplate>
                                                         <BDP:BasicDatePicker ID="tbTglTerbit" runat="server" DateFormat="dd MMM yyyy" ReadOnly="true"
                                                            ValidationGroup="Input" ButtonImageHeight="19px" ButtonImageWidth="20px" DisplayType="TextBox"
                                                            TextBoxStyle-CssClass="TextDate" ShowNoneButton="False">
                                                            <TextBoxStyle CssClass="TextDate" Width="110px" height="23px"/>
                                                        </BDP:BasicDatePicker>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Tanggal Berlaku s/d">
                                                    <ItemTemplate>
                                                       <BDP:BasicDatePicker ID="tbTglExpired" runat="server" DateFormat="dd MMM yyyy" ReadOnly="true"
                                                            ValidationGroup="Input" ButtonImageHeight="19px" ButtonImageWidth="20px" DisplayType="TextBox"
                                                            TextBoxStyle-CssClass="TextDate" ShowNoneButton="False">
                                                            <TextBoxStyle CssClass="TextDate" Width="110px" height="23px"/>
                                                        </BDP:BasicDatePicker>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                            </Columns>
                                        </asp:GridView>
                                    </div>

                                    <div class="custom-modal-footer">
                                        <asp:Button ID="btnCancelFlagY" style="background-color: rgb(165, 35, 61);" OnClientClick="closeCustomModalFlagY()" runat="server" class="bitbtndt btncancel"
                                            Text="Cancel" width="100" />

                                        <asp:Button ID="btnFinishFlagY" runat="server" class="bitbtndt btnsave"
                                            Text="Ok" width="100" />                                            
                                    </div>
                                </div>
                            </div>

                             <asp:SqlDataSource ID="dsKegiatan" runat="server" SelectCommand="SELECT * FROM VMsKegiatan">

                        </asp:SqlDataSource>
                            
                            <script type="text/javascript">
                                function showCustomModalFlagY() {
                                    document.getElementById('customModalFlagY').style.display = 'flex';
                                }

                                function closeCustomModalFlagY() {
                                    document.getElementById('customModalFlagY').style.display = 'none';
                                }

                                // Klik di luar modal untuk menutup
                                // window.onclick = function (event) {                                    
                                // };
                                
                            </script>
                            <!--------------------- End modal CIP Finish Flag Y ------------------------------->
                        </div>


                </form>
            </body>

            </html>