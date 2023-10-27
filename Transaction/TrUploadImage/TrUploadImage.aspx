<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TrUploadImage.aspx.vb" Inherits="TrUploadImage_TrUploadImage" %>

<%@ Register Assembly="DevExpress.Web.v10.2, Version=10.2.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPopupControl" TagPrefix="dx" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<%@ Register assembly="DevExpress.Web.ASPxEditors.v10.2, Version=10.2.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxEditors" tagprefix="dx" %>
<%@ Register TagPrefix="MsgBoxDlg" TagName="MsgBoxCtrl" Src="~/UserControl/MsgBox.ascx" %>
<%@ Register TagPrefix="WaitDlg" TagName="WaitCtrl" Src="~/UserControl/Waiting.ascx" %>
<%--<%@ Register TagPrefix="FindDlg" TagName="FindDlgCtrl" Src="~/UserControl/FindDlg1.ascx" %>--%>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<meta charset="utf-8">
    <title>Upload Image</title>
    <script src="../../Function/OpenDlg.JS" type="text/javascript"></script>         
	<link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
</head>
<body>
<form id="form1" runat="server">    
    <asp:ScriptManager ID="ScriptManager1" runat="server">        
        
    </asp:ScriptManager>
    <div class="Content">
        <div class="H1">Upload Image</div>
        <hr style="color:Blue" />
        <asp:Label runat="server" ID="lbGroup" Font-Bold="true"></asp:Label>   
        <asp:Label runat="server" ID="Label1" Text = " : " Font-Bold="true"></asp:Label>     
        <asp:Label runat="server" ID="lbCode" ForeColor="Red" Font-Bold="true"></asp:Label>    
        <asp:Label runat="server" ID="Label2" Text = " - " ForeColor="Red" Font-Bold="true"></asp:Label>     
        <asp:Label runat="server" ID="lbName" ForeColor="Red" Font-Bold="true"></asp:Label>
        <br />    
        <br />
        <asp:FileUpload runat="server" ID="FupMain"  />
        <asp:RegularExpressionValidator ID="FileUpLoadValidator" runat="server" ErrorMessage="Image Files Only (.jpg, .bmp, .png, .gif)"
            ValidationExpression="^(([a-zA-Z]:)|(\\{2}\w+)\$?)(\\(\w[\w].*))(.jpg|.JPG|.gif|.GIF|.jpeg|.JPEG|.bmp|.BMP|.png|.PNG)$" 
           ControlToValidate="FupMain"> 
        </asp:RegularExpressionValidator>
        <br />
        <br />
        <asp:Button ID="btnUpload" CssClass="bitbtndt btnsave" runat="server" Text="Upload" />
        <asp:Button ID="btnDelete" CssClass="bitbtndt btndelete" runat="server" Text="Delete" />
        <asp:Button ID="btnDownload" CssClass="bitbtndt btnadd" runat="server" Text="Download" />
        <asp:Button ID="btnopen" CssClass="bitbtndt btnadd" runat="server" Text="Open Pdf" />
        <asp:Image  runat="server" ID="imgviewer" />
        <asp:Label runat="server" ID="lbImage"></asp:Label>
        <asp:GridView id="GrdImage" runat="server" 
            ShowHeader = "false" PagerSettings-Position="TopAndBottom" ShowFooter="False" AllowSorting="False" 
            AutoGenerateColumns="False" AllowPaging="True" PageSize="1" CssClass="Grid">
		    <HeaderStyle CssClass="GridHeader"></HeaderStyle>
			<RowStyle CssClass="GridItem" Wrap="false"/>
			<AlternatingRowStyle CssClass="GridAltItem"/>
			<FooterStyle CssClass="GridFooter" />
			<PagerStyle CssClass="GridPager" Wrap="false" HorizontalAlign="Left"/>
			<Columns>            
			 <asp:ImageField DataImageUrlField="PictureURL"></asp:ImageField>            
            </Columns>
        </asp:GridView>                
        <asp:Label ID="lstatus" ForeColor="red" runat="server"></asp:Label>           
    </div>     
</form>    
</body>
</html>
