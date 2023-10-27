<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Testing.aspx.vb" Inherits="Execute_Master_Testing_Testing" %>

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
    <title>Account Group File</title>
    <script src="../../Function/OpenDlg.JS" type="text/javascript"></script>         

    <link rel="stylesheet" href="../../JQuery/CSS/jquery.ui.all.css">
    <script src="../../JQuery/jquery-1.9.1.js" type="text/javascript"></script>
    <script src="../../JQuery/ui/jquery.ui.core.js" type="text/javascript"></script>

    <script src="../../JQuery/ui/jquery.ui.widget.js" type="text/javascript"></script>
    <script src="../../JQuery/ui/jquery.ui.button.js" type="text/javascript"></script>
    <script src="../../JQuery/ui/jquery.ui.position.js" type="text/javascript"></script>
    <script src="../../JQuery/ui/jquery.ui.menu.js" type="text/javascript"></script>
    <script src="../../JQuery/ui/jquery.ui.autocomplete.js" type="text/javascript"></script>
    <script src="../../JQuery/ui/jquery.ui.tooltip.js" type="text/javascript"></script>

    <style type="text/css">
        .cpHeader
        {
            color: white;
            background-color: #719DDB;
            font: bold 11px auto "Trebuchet MS", Verdana;
            font-size: 12px;
            cursor: pointer;
            width:20px;
            top:auto;
            height:90%;
            padding: 0px;  
            position: absolute;
            /*overflow: hidden;*/
        }
       
        .cpBody
        {
            background-color: Gray;
            position: absolute;
            font: normal 11px auto Verdana, Arial;
            border: 1px gray;               
            left:30px;
            width:300px;
            top:auto;
            height:90%;            
            padding: 0px;
            padding-top: 7px;
            overflow:hidden;
        }      
        
        .cpContent
        {
            background-color: #DCE4F9;
            position: static;
            font: normal 11px auto Verdana, Arial;
            border: 1px gray;    
            height:90%;                                               
            margin-left:30px;
            width:900px;
            /*left:auto;*/
            top:auto;            
            padding: 4px;            
            padding-top: 7px;
        }
    </style>
	<link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
</head>
<body>
<form id="form1" runat="server">    
    <asp:ScriptManager ID="ScriptManager1" runat="server">        
        <%--<Services>            
        <asp:ServiceReference Path="~/App_WebReferences/Timbangan/Reference.svcmap" />         
      </Services>--%>
    </asp:ScriptManager>
    
    <asp:UpdatePanel ID="UpMain" runat="server" >
    <ContentTemplate>           
        <asp:UpdatePanel ID="Up1" runat="server">
        <ContentTemplate>
        <asp:Button runat="server" ID="btnMsg" Text = "Msg"/>
        <asp:Button runat="server" ID="btnLoading" Text = "Loading"/>
          <%--<asp:Button runat="server" ID="btn1" Text = "Panel 1"/>
          <asp:Button runat="server" ID="btn2" Text = "Panel 1 & 2"/>
          <asp:Button runat="server" ID="btn3" Text = "Panel Out"/>--%>          
          <asp:Button runat="server" ID="btnFind" Text = "Search" Style="display: none"/>
          <%--<asp:Button runat="server" ID="btnFind2" Text = "Search 2"/>
          <asp:Button runat="server" ID="btnFinddx" Text = "Search dx"/>          --%>
          <asp:Label runat="server" ID="lbl01"></asp:Label>          
          <cc1:ModalPopupExtender ID="ModalPopupExtender1" runat="server" 
            PopupControlID="pnlFind" DropShadow="True" TargetControlID="btnFind" 
            CancelControlID="btnBatal" 
            BackgroundCssClass="BackgroundStyle" />       
               
            <%--<dx:ASPxPopupControl ID="ASPxPopupControl1" runat="server" ContentUrl="../../SearchDlg.Aspx"  > 
            </dx:ASPxPopupControl>--%>
            <MsgBoxDlg:MsgBoxCtrl ID="MsgBoxOpen" runat = "server"/>
            <asp:Timer ID="Timer1" runat="server" Interval="2000" Enabled="false" />
            <WaitDlg:WaitCtrl ID="WaitOpen" runat = "server"/>
        </ContentTemplate>    
        </asp:UpdatePanel>
        <%--<asp:UpdatePanel ID="Up2" runat="server">
        <ContentTemplate>
          <asp:Button runat="server" ID="btn1b" Text = "Panel 2"/>
          <asp:Button runat="server" ID="btn2b" Text = "Panel 1 & 2"/>
          <asp:Label runat="server" ID="lbl02"></asp:Label>
        </ContentTemplate>    
        </asp:UpdatePanel>--%>
        <%--<asp:Label runat="server" ID="lbl03"></asp:Label>   --%>
        <asp:Panel ID="pnlFind" runat="server" BackColor="#ffffff" 
            BorderColor="#dadada" BorderStyle="Solid" BorderWidth="4px" 
            Height="190px" Width="350px" Font-Names="Arial" Font-Size="10pt">
            <asp:Label ID="lblFind" runat="server"></asp:Label>
           <table cellpadding="10" cellspacing="0" style="width: 100%">              
              <tr>
                 <td>
                    <asp:RadioButtonList ID="RadioButtonList1" runat="server">
                       <asp:ListItem>Java</asp:ListItem>
                       <asp:ListItem>VB.NET</asp:ListItem>
                       <asp:ListItem>C#</asp:ListItem>
                    </asp:RadioButtonList>
                 </td>
              </tr>
              <tr>
                 <td>
                    <asp:Button ID="btnSubmit" runat="server" Text="Submit" />
                    <asp:Button ID="btnBatal" runat="server" Text="Cancel" />
                 </td>
              </tr>
           </table>
        </asp:Panel>                  
        <%--<FindDlg:FindDlgCtrl ID="FindDlgCity" Visible="false" OnLoginStatus="FindDlgCity_LoginStatus" runat = "server"/>      --%>
        
      </ContentTemplate>        
    </asp:UpdatePanel>   
    
    
    <%--<br />
    
	<table>
	<tr>
	<td>	
	<asp:DropDownList runat="server" ID="combobox2">
                  <asp:ListItem Selected="true" Text="Account Group Code" Value="A.AccGroupCode"></asp:ListItem>
                  <asp:ListItem Text="Account Group Name" Value="A.AccGroupName"></asp:ListItem>        
                    <asp:ListItem Value="B.AccTypeName">Account Type</asp:ListItem>
                </asp:DropDownList>     
                <input id="Button1" type="button" value="Get Products" 
                    onclick="Button1_onclick()" />
                <asp:Label runat="server" ID ="label1" ></asp:Label>
    </td>
	</tr>            	    
	<tr>
	<td>
	<asp:DropDownList ID="AccTypeTest" Runat="Server" 
       DataSourceID="dsAccType" DataTextField="AccTypeName" 
       DataValueField="AccTypeCode">
	</asp:DropDownList>			
	&nbsp &nbsp &nbsp &nbsp
	<asp:Button runat="server" ID="btntest" Text="Klik Hasil" />
	<asp:Label runat="server" ID="lb01" ></asp:Label>
	</td>
	</tr>    
	</table>	--%>			    
    
    
    <div class="Content">
     <div class="H1">Account Group File</div>
     <hr style="color:Blue" />
     <asp:UpdatePanel ID="UpdAJAXMain" runat="server">
        <ContentTemplate>
        <asp:UpdatePanel ID="UpdAJAXHd" runat="server">
        <ContentTemplate>        
        <table>
        <tr>
            <td style="width:100px;text-align:right" >Quick Search :</td>
            <td><asp:TextBox runat="server" CssClass="TextBox" ID ="tbFilter"/> 
                <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlField" >
                  <asp:ListItem Selected="true" Text="Account Group Code" Value="A.AccGroupCode"></asp:ListItem>
                  <asp:ListItem Text="Account Group Name" Value="A.AccGroupName"></asp:ListItem>        
                    <asp:ListItem Value="B.AccTypeName">Account Type</asp:ListItem>
                </asp:DropDownList>     
                <asp:ImageButton ID="btnSearch" runat="server"  
                    ImageUrl="../../Image/btnsearchon.png"
                    onmouseover="this.src='../../Image/btnsearchoff.png';"
                    onmouseout="this.src='../../Image/btnsearchon.png';"
                    ImageAlign="AbsBottom" />                
               <asp:ImageButton ID="btnExpand" runat="server"  
                    ImageUrl="../../Image/btndownon.png"
                    onmouseover="this.src='../../Image/btndownoff.png';"
                    onmouseout="this.src='../../Image/btndownon.png';"
                    ImageAlign="AbsBottom" />
                <asp:ImageButton ID="btnPrint" runat="server"  
                    ImageUrl="../../Image/btnPrinton.png"
                    onmouseover="this.src='../../Image/btnPrintoff.png';"
                    onmouseout="this.src='../../Image/btnPrinton.png';"
                    ImageAlign="AbsBottom" />
                 <asp:Button runat="server" ID="btnFindDlg" Text = "Search Dlg" autopostback="true"/>
            </td>
        </tr>
     </table>
     <asp:Panel runat="server" ID="pnlSearch" Visible="false">
     <table>   
        <tr>
            <td style="width:100px;text-align:right"><asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlNotasi" >
                    <asp:ListItem Selected="true" Text="OR" Value="OR"></asp:ListItem>
                    <asp:ListItem Text="AND" Value="AND"></asp:ListItem>         
                </asp:DropDownList>
            </td>
            <td><asp:TextBox runat="server" ID ="tbfilter2" CssClass="TextBox"/> 
                  <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlField2" >
                    <asp:ListItem Selected="true" Text="Account Group Code" Value="A.AccGroupCode"></asp:ListItem>
                    <asp:ListItem Text="Account Group Name" Value="A.AccGroupName"></asp:ListItem> 
                      <asp:ListItem Value="B.AccTypeName">Account Type</asp:ListItem>
                  </asp:DropDownList>
                
            </td>
        </tr>
     </table>
     </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
    <br />
        <asp:UpdatePanel ID="UpdAJAXDt" runat="server">
        <ContentTemplate>
            <asp:GridView id="DataGrid" runat="server" 
            ShowFooter="True" AllowSorting="True" 
            AutoGenerateColumns="False" AllowPaging="True"  CssClass="Grid">
						<HeaderStyle CssClass="GridHeader"></HeaderStyle>
						<RowStyle CssClass="GridItem" Wrap="false"/>
						<AlternatingRowStyle CssClass="GridAltItem"/>
						<FooterStyle CssClass="GridFooter" />
						<PagerStyle CssClass="GridPager" Wrap="false" HorizontalAlign="Center"/>
				      <Columns>
							<asp:TemplateField HeaderText="Account Group Code" HeaderStyle-Width="110" SortExpression="AccGroupCode">
								<Itemtemplate>
									<asp:Label Runat="server" ID="AccGroupCode" text='<%# DataBinder.Eval(Container.DataItem, "AccGroupCode") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:Label Runat="server" ID="AccGroupCodeEdit" Text='<%# DataBinder.Eval(Container.DataItem, "AccGroupCode") %>'>
									</asp:Label>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="AccGroupCodeAdd" CssClass="TextBox" MaxLength="4" Runat="Server" Width="100%"/>
									<cc1:TextBoxWatermarkExtender ID="AccGroupCodeAdd_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="AccGroupCodeAdd" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>
								</FooterTemplate>
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Account Group Name" HeaderStyle-Width="330" SortExpression="AccGroupName">
								<Itemtemplate>
									<asp:Label Runat="server" ID="AccGroupName" text='<%# DataBinder.Eval(Container.DataItem, "AccGroupName") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:TextBox Runat="server" CssClass="TextBox" MaxLength="60" ID="AccGroupNameEdit" Width="100%" Text='<%# DataBinder.Eval(Container.DataItem, "AccGroupName") %>'>
									</asp:TextBox>
									<cc1:TextBoxWatermarkExtender ID="AccGroupNameEdit_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="AccGroupNameEdit" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>									
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="AccGroupNameAdd" CssClass="TextBox" MaxLength="60" Runat="Server" Width="100%"/>
									<cc1:TextBoxWatermarkExtender ID="AccGroupNameAdd_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="AccGroupNameAdd" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>
								</FooterTemplate>
							</asp:TemplateField>		
							
							<asp:TemplateField HeaderText="Account Type" HeaderStyle-Width="160" SortExpression="AccTypeName">
								<Itemtemplate>
									<asp:Label Runat="server" ID="AccType" text='<%# DataBinder.Eval(Container.DataItem, "AccTypeName") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:DropDownList Runat="server" ID="AccTypeEdit" Width="100%" CssClass="DropDownList"
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "AccType") %>'
                                        DataSourceID="dsAccType" DataTextField="AccTypeName" 
                                        DataValueField="AccTypeCode">
									</asp:DropDownList>													
								</EditItemTemplate>
								<FooterTemplate>
								    <asp:DropDownList ID="AccTypeAdd" Runat="Server" Width="100%" CssClass="DropDownList"
                                        DataSourceID="dsAccType" DataTextField="AccTypeName" 
                                        DataValueField="AccTypeCode">
									</asp:DropDownList>																		
								</FooterTemplate>
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Action" headerstyle-width="126" >
								<ItemTemplate>
									                <asp:ImageButton ID="btnEdit" runat="server" CommandName="Edit"
                                                        ImageUrl="../../Image/btnEditDton.png"
                                                        onmouseover="this.src='../../Image/btnEditDtoff.png';"
                                                        onmouseout="this.src='../../Image/btnEditDton.png';"
                                                        ImageAlign="AbsBottom" />
                                                    <asp:ImageButton ID="btnDelete" runat="server" OnClientClick="return confirm('Sure to delete this data?');" CommandName="Delete"
                                                        ImageUrl="../../Image/btnDeleteDton.png"
                                                        onmouseover="this.src='../../Image/btnDeleteDtoff.png';"
                                                        onmouseout="this.src='../../Image/btnDeleteDton.png';"
                                                        ImageAlign="AbsBottom" />
								</ItemTemplate>
								<EditItemTemplate>
									                <asp:ImageButton ID="btnUpdate" runat="server"  CommandName="Update" 
                                                        ImageUrl="../../Image/btnsaveDton.png"
                                                        onmouseover="this.src='../../Image/btnsaveDtOff.png';"
                                                        onmouseout="this.src='../../Image/btnsaveDton.png';"
                                                        ImageAlign="AbsBottom" />
                                                    <asp:ImageButton ID="btnCancel" runat="server" CommandName="Cancel" 
                                                        ImageUrl="../../Image/btnCancelDtOn.png"
                                                        onmouseover="this.src='../../Image/btnCancelDtOff.png';"
                                                        onmouseout="this.src='../../Image/btnCancelDtOn.png';"
                                                        ImageAlign="AbsBottom" />
								</EditItemTemplate>
								<FooterTemplate>
									                <asp:ImageButton ID="btnAdd" runat="server"  CommandName="Insert" 
                                                        ImageUrl="../../Image/btnAddDtOn.png"
                                                        onmouseover="this.src='../../Image/btnAddDtOff.png';"
                                                        onmouseout="this.src='../../Image/btnAddDtOn.png';"
                                                        ImageAlign="AbsBottom" />
								</FooterTemplate>
							</asp:TemplateField>
							
    					</Columns>
            </asp:GridView>
            
            <asp:SqlDataSource ID="dsAccType" runat="server"                                                                                 
                                            SelectCommand="EXEC S_GetAccType">                                        
            </asp:SqlDataSource>
        </ContentTemplate>
        </asp:UpdatePanel>
        <asp:Label ID="lstatus" ForeColor="red" runat="server"></asp:Label>           
    </ContentTemplate>
    </asp:UpdatePanel>
    
    
    <br />
    <hr />
     <asp:Panel ID="pHeader" runat="server" ScrollBars="None" CssClass="cpHeader">
                <asp:Image id="image1" runat="server" ImageAlign="AbsMiddle" BackColor="#719DDB"/>
                <asp:Label ID="lblText" runat="server" />                
     </asp:Panel>
     <asp:Panel ID="pBody" runat="server" ScrollBars="None" CssClass="cpBody">
        <asp:Label runat="server" ID="lbtest1" Text="Test 1"></asp:Label>
        <asp:Label runat="server" ID="Label1" Text="Test 2"></asp:Label>
        <asp:Label runat="server" ID="Label2" Text="Test 3"></asp:Label>
        <asp:Label runat="server" ID="Label3" Text="Test 4"></asp:Label>
        </asp:Panel>     
        <asp:Panel runat="server" ID="pnlContent" CssClass="cpContent">
        <asp:Label runat="server" ID="lbCt1" Text="Content 1 Content 1 Content 1"></asp:Label>
        <br />
        <asp:Label runat="server" ID="lbCt2" Text="Content 2 Content 2 Content 2"></asp:Label>
        <br />
        <asp:Label runat="server" ID="lbCt3" Text="Content 3 Content 3 Content 3"></asp:Label>
        </asp:Panel>     
        <%--<div runat="server" CssClass="cpContent">
        <asp:Label runat="server" ID="lbCt1" Text="Content 1 Content 1 Content 1"></asp:Label>
        <br />
        <asp:Label runat="server" ID="lbCt2" Text="Content 2 Content 2 Content 2"></asp:Label>
        <br />
        <asp:Label runat="server" ID="lbCt3" Text="Content 3 Content 3 Content 3"></asp:Label>
        </div>  --%>
        
            
    <cc1:CollapsiblePanelExtender ID="cpe" runat="Server"
        TargetControlID="pBody"
        CollapsedSize="0"
        ExpandedSize="300"        
        Collapsed="True"
        ExpandControlID="pHeader"
        CollapseControlID="pHeader"
        AutoCollapse="False"
        AutoExpand="False"
        ScrollContents="false"                 
        TextLabelID="lblText"        
        CollapsedText=""
        ExpandedText="" 
        ImageControlID="image1"
        ExpandedImage="../../Image/PLUS.JPG"
        CollapsedImage="../../Image/MINUS.BMP"
        ExpandDirection="Horizontal" />    
        
    <%--<br />
    <hr />
            <asp:FileUpload runat="server" ID="FupMain" />
        <asp:Button ID="btnUpload" runat="server" Text="Upload" />
        <asp:Button ID="btnDelete" runat="server" Text="Delete" />
        <asp:Button ID="btnDownload" runat="server" Text="Download" />
        <asp:Image  runat="server" ID="imgviewer" />
        <asp:Label runat="server" ID="lbImage"></asp:Label>
        <asp:GridView id="GrdImage" runat="server" 
            ShowHeader = "false" PagerSettings-Position="TopAndBottom" ShowFooter="False" AllowSorting="False" 
            AutoGenerateColumns="False" AllowPaging="True" PageSize="1" CssClass="Grid">
		    <HeaderStyle CssClass="GridHeader"></HeaderStyle>
			<RowStyle CssClass="GridItem" Wrap="false"/>
			<AlternatingRowStyle CssClass="GridAltItem"/>
			<FooterStyle CssClass="GridFooter" />
			<PagerStyle CssClass="GridPager" Wrap="false" HorizontalAlign="Center"/>
			<Columns>            
			<asp:ImageField DataImageUrlField="PictureURL"></asp:ImageField>
            
            </Columns>
        </asp:GridView>
        
        <br />--%>
    </div>     
</form>    
</body>
</html>
