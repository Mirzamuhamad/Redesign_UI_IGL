<%@ Page Language="VB" AutoEventWireup="true" CodeFile="index2.aspx.vb" Inherits="index2" %>

<%--<%@ Register Assembly="obout_EasyMenu_Pro" Namespace="OboutInc.EasyMenu_Pro" TagPrefix="oem" %>--%>

<%--<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">--%>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Asia Pacific Fortuna Sari</title>

    <script src="AjaxSplitter/VwdCmsSplitterBar.js" type="text/javascript"></script>
    <link href="Styles/MenuTemplates.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        function OpenAcc() {                             
                var _Value = document.getElementById('someID').innerHTML; //innerHTML; //innerText;
                //alert("*"+_Value+"*");
                //var iText = theDiv.innerHTML.replace(re,'');
                var Iindex2 = _Value.search("Change")
                //alert(Iindex2);
                //if (_Value == "Change Period :")
                if (Iindex2 >= 0)
                {
                window.open("AccountingPeriod.aspx?KeyId="+'<%=Viewstate("KeyId")%>',"List","scrollbars=yes,resizable=no,width=200,height=150");        
                return false;
                }
        }   
        function updateAcc(taon, bulan)
        {            
            var browserName=navigator.appName;
            if (browserName=="Microsoft Internet Explorer")
             {
              document.getElementById('lbYear').innerText = bulan + ', ' + taon;
             }
             else
             {
              document.getElementById('lbYear').textContent = bulan + ', ' + taon;
             }            
        }
    </script>
    
    <link rel="stylesheet" type="text/css" href="sdmenu/sdmenu.css" />
	<script type="text/javascript" src="sdmenu/sdmenu.js">
		/***********************************************
		* Slashdot Menu script- By DimX
		* Submitted to Dynamic Drive DHTML code library: http://www.dynamicdrive.com
		* Visit Dynamic Drive at http://www.dynamicdrive.com/ for full source code
		***********************************************/
	</script>
	<script type="text/javascript">
	/*function resizeImg(width) {
        document.getElementById("menuTop").style.height = 30;
        document.getElementById("menuTop").style.width = width;    
    }
	window.onresize = resizeImg(document.body.clientWidth);     */ 

	// <![CDATA[
	var myMenu;
	    window.onload = function() {
		myMenu = new SDMenu("my_menu");
		myMenu.init();			
	};
	// ]]>
	</script>	
</head>
<body>
    <form id="form1" runat="server" >
    <div id="topContent">
    <table style="width:100%; padding:0,0,0,0; margin:0,0,0,0; background-image:url('Image/bg.gif')">
    <tr>
        <td style="width:6%"><asp:Image ID="Image1" ImageUrl="Image/Logo.Jpg" ImageAlign="Top" Height="72px" Width="72px" runat="server"/></td>
		<td style="width:92%">
			<table width="100%">
				<tr style="height:20px">
					<td><asp:Label ID="lbCompany" Font-Names="Verdana" Font-Size="12" Font-Bold="true"  runat="server" /></td>
				</tr>
				<tr style="height:25px">
					<td></td>
				</tr>
				<tr>				      
					<td>
					    <table width="100%">
					        <tr>
					            <td style="width:35%">
					                <%--<a style="color:blue; font-family:Verdana; font-size:8pt" onclick="OpenAcc(); return false;" id="someID" href="">Period</a>--%>
					                <asp:Label Font-Bold="true" ForeColor="Blue" Font-Names="Verdana" Font-Size="9" Text="Period" Onclick="OpenAcc(); return false;" id="someID" runat="server"/>
					                <asp:Label Font-Names="Verdana" Font-Size="9" ID="lbYear" runat="server" />
					            </td>
					            <td style="width:5%"></td>
					            <td style="width:10%"></td>
					            <td style="width:10%"></td>
					            <td style="width:40%; text-align:right"><asp:Label ID="Label1" Font-Names="Verdana" Font-Size="9pt" runat="server" Text="Welcome " /><asp:Label ID="lbUser" Font-Names="Verdana" Font-Size="9pt" ForeColor="red" Font-Bold="true" runat="server" Text="Welcome" />&nbsp |<asp:LinkButton ID="lkbHome" runat="server" Font-Names="Verdana" Font-Size="9pt" ForeColor="Blue" Font-Bold="true">&nbsp Home &nbsp</asp:LinkButton>|<asp:LinkButton ID="LinkButton1" runat="server" Font-Names="Verdana" Font-Size="9pt" ForeColor="Blue" Font-Bold="true">&nbsp Log Out &nbsp</asp:LinkButton></td>
					        </tr>
					    </table>
					
					
                            
                    </td>
				</tr>
			</table>
        </td>		
    </tr>
    </table>
    </div>
    <div id="topContent2">
        <asp:PlaceHolder ID="placeHolder2" runat="server">         
        <asp:Menu ID="menuTop" runat="server" Orientation="Horizontal"  EnableViewState="true" ItemWrap="true"  
                    CssClass="MenuBar" StaticEnableDefaultPopOutImage="False" MaximumDynamicDisplayLevels="5">
                    <StaticMenuStyle CssClass="StaticMenuItemMB" />                
                    <StaticMenuItemStyle CssClass="StaticMenuItemStyleMB" />
                    <StaticHoverStyle CssClass="StaticHoverStyleMB" />  
                    <StaticSelectedStyle CssClass="StaticSelectedStyleMB" />              
                    <DynamicMenuItemStyle CssClass="DynamicMenuItemStyleMB" />
                    <DynamicHoverStyle CssClass="DynamicHoverStyleMB" />                 
                    </asp:Menu>
         </asp:PlaceHolder>     
    </div>
    
   <table border="0" style="width:100%;height:100%;border:solid 1px black;" 
			cellpadding="0" cellspacing="0">
			<tr style="height:100%;">
				<td id="tdLeft" style="width:200px; height:100%;background-color:gainsboro;border-right:solid 1px black;">
				    <asp:PlaceHolder ID="placeHolder1" runat="server">
					<div id="my_menu" class="sdmenu" runat="server" style="width:100%;height:100%;overflow:auto;padding:0px;margin:0px;">
					</div>
					<%--<div style="float: left" id="my_menu" class="sdmenu" runat="server">          
                    </div>    --%>
                    </asp:PlaceHolder>  
					
				</td>
				<td style="width:5px;height:100%;"></td>
				<td id="tdRight" align="left" valign="top" style="height:100%;"> 					
					<iframe id="MainFrame" runat="server" enableviewstate="false" 
					scrolling="auto" frameborder="no" 
					style="width:100%;height:100%;"></iframe>
					
					<%--<iframe name="MainFrame" id="MainFrame" style="width:100%;height:85%"></iframe>--%>
                    
				</td>		
			</tr>
		</table>
    <div id="footerContent">
       <%--<asp:Image ID="PoweredByImage" ImageUrl="" runat="server" AlternateText="Powered by ASP.NET!" />--%>
       <asp:Label runat="server" ID="lStatus"></asp:Label>
    </div>
		
			<VwdCms:SplitterBar runat="server" 
				ID="vwdSplitter" 
				LeftResizeTargets="tdLeft" 
				DynamicResizing="false"
				MinWidth="120" 
				MaxWidth="200"
				style="background-image:url(vsplitter.gif);	 background-position:center center;	 background-repeat:no-repeat;	 border-left:solid 1px black;	 border-right:solid 1px black;" 				
				IFrameHiding="DoNotHide"				
				OnResizeStart="splitterOnResizeStart">
				</VwdCms:SplitterBar>
    
   <script language="javascript" type="text/javascript">

	function splitterOnResizeStart(splitterBar)
	{
		
		
			// first hide the IFrame, then display the DIV
			var ifrm = document.getElementById("MainFrame");
//			if ( ifrm )
//			{
//				ifrm.style.display = "none";
//			}		
//			var div = document.getElementById("divRight6");
//			if ( div )
//			{
//				div.style.display = "block";
//			}
		
	}
	function splitterOnResizeComplete(splitterBar, width)
	{
		// Arguments:
		// splitterBar is a VwdCmsSplitterBar object
		// width is a string like "250px

		// ** the actual div element that you see on the 
		// screen can be accessed by using splitterBar.splitterBar
		// or by document.getElementById('splitterBarID')
		// where 'splitterBarID' is the ID of the server control
		// in your ASPX code.

		// do any other work that needs to happen when the 
		// splitter resizing is complete. this is a good place to handle 
		// any complex resizing rules, etc.

		// make sure the width is in number format
		if (typeof width == "string")
		{
			width = new Number(width.replace("px",""));
		}
		
        // first hide the DIV, then display the IFrame
//			var div = document.getElementById("divRight6");
//			if ( div )
//			{
//				div.style.display = "none";
//			}
			var ifrm = document.getElementById("MainFrame");
			if ( ifrm )
			{
				ifrm.style.display = "block";
			}				
	}		
	function splitterOnResize(splitterBar, width)
	{
		// Arguments:
		// splitterBar is a VwdCmsSplitterBar object
		// width is a string like "250px

		// ** the actual div element that you see on the 
		// screen can be accessed by using splitterBar.splitterBar
		// or by document.getElementById('splitterBarID')
		// where 'splitterBarID' is the ID of the server control
		// in your ASPX code.

		// do any other work that needs to happen when the 
		// splitter resizes. this is a good place to handle 
		// any complex resizing rules, etc.

		// make sure the width is in number format
		if (typeof width == "string")
		{
			width = new Number(width.replace("px",""));
		}
		
		// check to see what splitterBar fired this event
//		if ( splitterBar && splitterBar.id == "???" )
//		{
//			
//		}
	}

// -->
</script> 
    

    <%--<div style="color:Red; text-align:center"><asp:Label runat="server" ID="lStatus"></asp:Label></div>--%>
    </form>    
</body>
</html>
