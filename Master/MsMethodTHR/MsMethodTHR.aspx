<%@ Page Language="VB" AutoEventWireup="false" CodeFile="MsMethodTHR.aspx.vb" Inherits="MsMethodTHR_MsMethodTHR" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">    
    <title>MethodTHR File</title>
    <script src="../../Function/OpenDlg.js" type="text/javascript"></script>
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
 <link href="https://fonts.googleapis.com/css2?family=Muli:wght@300;400;600&display=swap" rel="stylesheet">
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="SSSS" runat="server">
        <ContentTemplate>
        </ContentTemplate>
    </asp:UpdatePanel>
     <div class="Content">
     <asp:UpdatePanel ID="UpdAJAXMain" runat="server">
        <ContentTemplate>
     <div class="H1">MethodTHR File</div>
     <hr style="color:Blue" />
     
        <%--<asp:UpdatePanel ID="UpdAJAXHd" runat="server">
        <ContentTemplate>      --%>
        <table>
        <tr>
            <td style="width:100px;text-align:right" >Quick Search :</td>
            <td><asp:TextBox runat="server" ID ="tbFilter" CssClass="TextBox"/> 
                <asp:DropDownList runat="server" ID="ddlField" CssClass="DropDownList" >
                  <asp:ListItem Selected="true" Text="Method THR Code" Value="MethodTHRCode"></asp:ListItem>
                  <asp:ListItem Text="Method THR Name" Value="MethodTHRName"></asp:ListItem>        
                </asp:DropDownList>     
                <asp:Button class="bitbtn btnsearch" runat="server" ID="btnSearch" Text="Search" />
                <asp:Button class="btngo" runat="server" ID="btnExpand" Text="..."/>
                <asp:Button class="bitbtn btnprint" runat="server" ID="btnPrint" Text="Print"/>
                <asp:Button ID="btnShowPopup" runat="server" Style="display: none" />
                <cc1:ModalPopupExtender ID="ModalPopupExtender1" runat="server" 
                    PopupControlID="pnlFind" DropShadow="True" TargetControlID="btnShowPopup"                     
                    BackgroundCssClass="BackgroundStyle" />       
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
                  <asp:DropDownList runat="server" ID="ddlField2" CssClass="DropDownList" >
                    <asp:ListItem Selected="true" Text="Method THR Code" Value="MethodTHRCode"></asp:ListItem>
                  <asp:ListItem Text="Method THR Name" Value="MethodTHRName"></asp:ListItem>        
                  </asp:DropDownList>
                
            </td>
        </tr>
     </table>
     </asp:Panel>  
        <%--</ContentTemplate>
        </asp:UpdatePanel>--%>
        <br />
        <%--<asp:UpdatePanel ID="UpdAJAXDt" runat="server">
        <ContentTemplate>   --%>
        <asp:GridView id="DataGrid" runat="server"  
            ShowFooter="True" AllowSorting="True" 
            AutoGenerateColumns="False" AllowPaging="True" CssClass="Grid">
						<HeaderStyle CssClass="GridHeader"></HeaderStyle>
						<RowStyle CssClass="GridItem" Wrap="false"/>
						<AlternatingRowStyle CssClass="GridAltItem"/>
						<FooterStyle CssClass="GridFooter" />
						<PagerStyle CssClass="GridPager" HorizontalAlign="Center" />					  
				<EmptyDataTemplate>
				    
				</EmptyDataTemplate>	  
				      <Columns>				      
							<asp:TemplateField HeaderText="MethodTHR Code" HeaderStyle-Width="100" SortExpression="MethodTHRCode">
								<Itemtemplate>
									<asp:Label Runat="server" ID="MethodTHRCode" text='<%# DataBinder.Eval(Container.DataItem, "MethodTHRCode") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:Label Runat="server" ID="MethodTHRCodeEdit" Width="100%" Text='<%# DataBinder.Eval(Container.DataItem, "MethodTHRCode") %>'>
									</asp:Label>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="MethodTHRCodeAdd" CssClass="TextBox" MaxLength="5" Runat="Server" Width="100%"/>
									<cc1:TextBoxWatermarkExtender ID="MethodTHRCodeAdd_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="MethodTHRCodeAdd" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>
								</FooterTemplate>								
							</asp:TemplateField>							
							
							<asp:TemplateField HeaderText="MethodTHR Name" HeaderStyle-Width="368" SortExpression="MethodTHRName">
								<Itemtemplate>
									<asp:Label Runat="server" ID="MethodTHRName" text='<%# DataBinder.Eval(Container.DataItem, "MethodTHRName") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:TextBox Runat="server" ID="MethodTHRNameEdit" MaxLength="60" Width="100%" CssClass="TextBox" Text='<%# DataBinder.Eval(Container.DataItem, "MethodTHRName") %>'>
									</asp:TextBox>
									<cc1:TextBoxWatermarkExtender ID="MethodTHRNameEdit_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="MethodTHRNameEdit" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>									
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="MethodTHRNameAdd" CssClass="TextBox" MaxLength="60" Runat="Server" Width="100%"/>
									<cc1:TextBoxWatermarkExtender ID="MethodTHRNameAdd_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="MethodTHRNameAdd" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>
								</FooterTemplate>
							</asp:TemplateField>		
							
							<asp:TemplateField HeaderText="Action" headerstyle-width="126" >
							    <ItemTemplate>
								    <asp:Button ID="btnEdit" runat="server" class="bitbtndt btnedit" Text="Edit" CommandName="Edit"/>
									<asp:Button ID="btndelete" runat="server" class="bitbtndt btndelete" Text="Delete" OnClientClick="return confirm('Sure to delete this data?');" CommandName="Delete"/>									
								</ItemTemplate>
								<EditItemTemplate>
									<asp:Button ID="btnUpdate" runat="server" class="bitbtndt btnsave" Text="Save" CommandName="Update"/>									
                                    <asp:Button ID="btnCancel" runat="server" class="bitbtndt btncancel" Text="Cancel" CommandName="Cancel"/>									
								</EditItemTemplate>										
								<FooterTemplate>
									<asp:Button class="bitbtndt btnadd" runat="server" ID="btnAdd" Text="Add" CommandName="Insert"/>									
								</FooterTemplate>
							</asp:TemplateField>
							
    					</Columns>
        </asp:GridView>     
        <%--</ContentTemplate>
        </asp:UpdatePanel>--%>
        <asp:Label ID="lstatus" ForeColor="red" runat="server"></asp:Label>
        <asp:Panel ID="pnlFind" runat="server"> </asp:Panel> 
      </ContentTemplate>
      </asp:UpdatePanel>
    </div>
    </form>
</body>

</html>
