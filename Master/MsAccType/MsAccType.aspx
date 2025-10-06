<%@ Page Language="VB" AutoEventWireup="false" CodeFile="MsAccType.aspx.vb" Inherits="Execute_Master_MsAccType_MsAccType" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Account Type File</title>    
    <script src="../../Function/OpenDlg.JS" type="text/javascript"></script>     
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
 <link href="https://fonts.googleapis.com/css2?family=Muli:wght@300;400;600&display=swap" rel="stylesheet">
</head>

<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true">
    </asp:ScriptManager>
    <div class="Content">
     <div class="H1">Account Type File</div>
     <hr style="color:Blue" />
      <table>
        <tr>
            <td style="width:100px;text-align:right" >Quick Search :</td>
            <td><asp:TextBox CssClass="TextBox" runat="server" ID ="tbFilter"/> 
                <asp:DropDownList runat="server" ID="ddlField" CssClass="DropDownList" >
                  <asp:ListItem Selected="true" Text="Account Type Code" Value="AccTypeCode"></asp:ListItem>
                  <asp:ListItem Text="Account Type Name" Value="AccTypeName"></asp:ListItem>        
                    <asp:ListItem Value="FgType">Type</asp:ListItem>
                    <asp:ListItem Text="Position" Value="Position"></asp:ListItem>        
                    <asp:ListItem Text="PL Summary" Value="FgPLSummary"></asp:ListItem>        
                </asp:DropDownList>     
                <asp:Button class="bitbtn btnsearch" runat="server" ID="btnSearch" Text="Search" />
                <asp:Button class="btngo" runat="server" ID="btnExpand" Text="..."/>
                <asp:Button class="bitbtn btnprint" runat="server" ID="btnPrint" Text="Print"/>
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
            <td><asp:TextBox CssClass="TextBox" runat="server" ID ="tbfilter2"/> 
                  <asp:DropDownList runat="server" ID="ddlField2" CssClass="DropDownList" >
                    <asp:ListItem Selected="true" Text="Account Type Code" Value="AccTypeCode"></asp:ListItem>
                    <asp:ListItem Text="Account Type Name" Value="AccTypeName"></asp:ListItem>        
                    <asp:ListItem Value="FgType">Type</asp:ListItem>        
                      <asp:ListItem Text="Position" Value="Position"></asp:ListItem>
                      <asp:ListItem Text="PL Summary" Value="FgPLSummary"></asp:ListItem>        
                  </asp:DropDownList>
                
            </td>
        </tr>
     </table>
     </asp:Panel>
      <br />
      <asp:GridView id="DataGrid" runat="server" ShowFooter="True" AllowSorting="True" 
            AutoGenerateColumns="False" AllowPaging="True" CssClass="Grid">
						<HeaderStyle CssClass="GridHeader" Wrap="false"></HeaderStyle>
						<RowStyle CssClass="GridItem" Wrap="false"/>
						<AlternatingRowStyle CssClass="GridAltItem"/>
						<FooterStyle CssClass="GridFooter" />
						<PagerStyle CssClass="GridPager" HorizontalAlign="Center" />
				      <Columns>
							<asp:TemplateField HeaderText="Account Type Code" SortExpression="AccTypeCode">
								<Itemtemplate>
									<asp:Label Runat="server" ID="AccTypeCode" text='<%# DataBinder.Eval(Container.DataItem, "AccTypeCode") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:Label Runat="server" ID="AccTypeCodeEdit" Text='<%# DataBinder.Eval(Container.DataItem, "AccTypeCode") %>'>
									</asp:Label>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="AccTypeCodeAdd" placeholder="can't blank" CssClass="TextBox" MaxLength="2" Runat="Server" Width="90%"/>
									<%--<cc1:TextBoxWatermarkExtender ID="AccTypeCodeAdd_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="AccTypeCodeAdd" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender> --%>
								</FooterTemplate>
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Account Type Name" HeaderStyle-Width="350" SortExpression="AccTypeName">
								<Itemtemplate>
									<asp:Label Runat="server" ID="AccTypeName" text='<%# DataBinder.Eval(Container.DataItem, "AccTypeName") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:TextBox Runat="server" CssClass="TextBox" ID="AccTypeNameEdit" MaxLength="60" Width="100%" Text='<%# DataBinder.Eval(Container.DataItem, "AccTypeName") %>'>
									</asp:TextBox>
									<cc1:TextBoxWatermarkExtender ID="AccTypeNameEdit_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="AccTypeNameEdit" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="AccTypeNameAdd" placeholder="can't blank" CssClass="TextBox" MaxLength="60" Runat="Server" Width="90%"/>
									<%--<cc1:TextBoxWatermarkExtender ID="AccTypeNameAdd_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="AccTypeNameAdd" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>--%>
								</FooterTemplate>

                        <HeaderStyle Width="350px"></HeaderStyle>
							</asp:TemplateField>		
							
							<asp:TemplateField HeaderText="Type" SortExpression="FgType">
								<Itemtemplate>
									<asp:Label Runat="server" ID="FgType" text='<%# DataBinder.Eval(Container.DataItem, "FgType") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:DropDownList CssClass="DropDownList" Runat="server" ID="FgTypeEdit" 
                                        Width="100%" text='<%# DataBinder.Eval(Container.DataItem, "FgType") %>' 
                                        AutoPostBack="True" onselectedindexchanged="FgTypeEdit_SelectedIndexChanged">
									  <asp:ListItem >BS</asp:ListItem>
									    <asp:ListItem>PL</asp:ListItem>  
									</asp:DropDownList>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:DropDownList CssClass="DropDownList" ID="FgTypeAdd" Runat="Server" 
                                        Width="100%" AutoPostBack="True" 
                                        onselectedindexchanged="FgTypeAdd_SelectedIndexChanged" >
									    <asp:ListItem Selected="True">BS</asp:ListItem>
									    <asp:ListItem>PL</asp:ListItem>
									</asp:DropDownList>
								</FooterTemplate>
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Position" SortExpression="Position">
								<Itemtemplate>
									<asp:Label Runat="server" ID="Position" text='<%# DataBinder.Eval(Container.DataItem, "Position") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:DropDownList CssClass="DropDownList" Runat="server" ID="PositionEdit" Width="100%"  > 
									 
									</asp:DropDownList>
									
								</EditItemTemplate>
								<FooterTemplate>
									<asp:DropDownList CssClass="DropDownList" ID="PositionAdd" Runat="Server" Width="100%">
									    <asp:ListItem Selected="True">AKTIVA</asp:ListItem>
									    <asp:ListItem>PASIVA</asp:ListItem>
									</asp:DropDownList>
								</FooterTemplate>
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="PL Summary" SortExpression="FgPLSummary">
								<Itemtemplate>
									<asp:Label Runat="server" ID="PLSummary" text='<%# DataBinder.Eval(Container.DataItem, "FgPLSummary") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:DropDownList CssClass="DropDownList" Runat="server" ID="FgPLSummaryEdit" Width="100%" >
									 
									</asp:DropDownList>
									
								</EditItemTemplate>
								<FooterTemplate>
									<asp:DropDownList CssClass="DropDownList" ID="FgPLSummaryAdd" Runat="Server" Width="100%">
									    <asp:ListItem Selected="True">Y</asp:ListItem>
									    <asp:ListItem>N</asp:ListItem>
									</asp:DropDownList>
								</FooterTemplate>
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Action" headerstyle-width="126" >
								<ItemTemplate>
								    <asp:Button style = "height: 20px;" ID="btnEdit" runat="server" class="bitbtndt btnedit" Text="Edit" CommandName="Edit"/>
									<asp:Button style = "height: 20px;"  ID="btndelete" runat="server" class="bitbtndt btndelete" Text="Delete" OnClientClick="return confirm('Sure to delete this data?');" CommandName="Delete"/>									
								</ItemTemplate>
								<EditItemTemplate>
									<asp:Button style = "height: 20px;" ID="btnUpdate" runat="server" class="bitbtndt btnsave" Text="Save" CommandName="Update"/>									
                                    <asp:Button style = "height: 20px;" ID="btnCancel" runat="server" class="bitbtndt btncancel" Text="Cancel" CommandName="Cancel"/>									
								</EditItemTemplate>								
								<FooterTemplate>
									<asp:Button class="bitbtndt btnadd" runat="server" ID="btnAdd" Text="Add" CommandName="Insert"/>									
								</FooterTemplate>

<HeaderStyle Width="126px"></HeaderStyle>
							</asp:TemplateField>
							
    					</Columns>
        </asp:GridView>
        
     <asp:Label ID="lstatus" ForeColor="red" runat="server"></asp:Label>        
    </div>
    </form>
</body>
</html>
