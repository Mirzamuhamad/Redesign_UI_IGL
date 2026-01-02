<%@ Page Language="VB" AutoEventWireup="false" CodeFile="MsProductStockType.aspx.vb" Inherits="MsProductStockType_MsProductStockType" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Product JEnisFile</title>
    <script src="../../Function/OpenDlg.JS" type="text/javascript"></script> 
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
 <link href="https://fonts.googleapis.com/css2?family=Muli:wght@300;400;600&display=swap" rel="stylesheet">
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div class="Content">
     <div class="H1"><asp:Label runat="server" ID="lblTitle" Text="Product Stock Type File"></asp:Label></div>
     <hr style="color:Blue" />
      <table>
        <tr>
            <td style="width:100px;text-align:right" >Quick Search :</td>
            <td><asp:TextBox runat="server" ID ="tbFilter" CssClass="TextBox" /> 
                <asp:DropDownList runat="server" ID="ddlField" CssClass="DropDownList" > 
                    <asp:ListItem Selected="true" Text="Stock Type Code" Value="StockTypeCode"></asp:ListItem>
                    <asp:ListItem Text="Type Name" Value="StockTypeName"></asp:ListItem> 
                    <asp:ListItem Text="Account" Value="Account"></asp:ListItem> 
                    <asp:ListItem Text="Type" Value="Type"></asp:ListItem>                    
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
            <td><asp:TextBox runat="server" ID ="tbfilter2" CssClass="TextBox"/> 
                  <asp:DropDownList runat="server" ID="ddlField2" CssClass="DropDownList" >
                    <asp:ListItem Selected="true" Text="Stock Type Code" Value="StockTypeCode"></asp:ListItem>
                    <asp:ListItem Text="Type Name" Value="StockTypeName"></asp:ListItem> 
                    <asp:ListItem Text="Account" Value="Account"></asp:ListItem> 
                    <asp:ListItem Text="Type" Value="Type"></asp:ListItem>                               
                  </asp:DropDownList>
                
            </td>
        </tr>
     </table>
     </asp:Panel>
      <br />
      <asp:GridView id="DataGrid" runat="server" ShowFooter="True" AllowSorting="True" 
            AutoGenerateColumns="False" AllowPaging="True" CssClass="Grid">
						<HeaderStyle CssClass="GridHeader"></HeaderStyle>
						<RowStyle CssClass="GridItem" Wrap="false"/>
						<AlternatingRowStyle CssClass="GridAltItem"/>
						<FooterStyle CssClass="GridFooter" />
						<PagerStyle CssClass="GridPager" />
				      <Columns>
							<asp:TemplateField HeaderText="Stock Type Code" HeaderStyle-Width="160" SortExpression="StockTypeCode">
								<Itemtemplate>
									<asp:Label Runat="server" ID="StockTypeCode" text='<%# DataBinder.Eval(Container.DataItem, "StockTypeCode") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:Label Runat="server" ID="StockTypeCodeEdit" Text='<%# DataBinder.Eval(Container.DataItem, "StockTypeCode") %>'>
									</asp:Label>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="StockTypeCodeAdd"  Enabled="true" CssClass="TextBox" MaxLength="6" Runat="Server" Width="100%"/>
									<cc1:TextBoxWatermarkExtender ID="StockTypeCodeAdd_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="StockTypeCodeAdd" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>
								</FooterTemplate>
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Stock Type Name Name" HeaderStyle-Width="300" SortExpression="StockTypeName">
								<Itemtemplate>
									<asp:Label Runat="server" ID="StockTypeName" text='<%# DataBinder.Eval(Container.DataItem, "StockTypeName") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:TextBox Runat="server" ID="StockTypeNameEdit" MaxLength="60" CssClass="TextBox" Width="100%" Text='<%# DataBinder.Eval(Container.DataItem, "StockTypeName") %>'>
									</asp:TextBox>
									<cc1:TextBoxWatermarkExtender ID="StockTypeNameEdit_WtExt" 
                                        runat="server"  TargetControlID="StockTypeNameEdit" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="StockTypeNameAdd" Runat="Server" MaxLength="60" CssClass="TextBox" Width="100%"/>
									<cc1:TextBoxWatermarkExtender ID="StockTypeNameAdd_WtExt" 
                                        runat="server"  TargetControlID="StockTypeNameAdd" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>
								</FooterTemplate>
							</asp:TemplateField>		
							
							<asp:TemplateField HeaderText="Account" HeaderStyle-Width="30%" SortExpression="Account">
								<Itemtemplate>
									<asp:Label Runat="server" ID="Account" text='<%# DataBinder.Eval(Container.DataItem, "Account") %>'>
									</asp:Label>
									<asp:Label Runat="server" ID="Label1" text='<%# DataBinder.Eval(Container.DataItem, "AccountName") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
								    <asp:TextBox Runat="server" ID="AccountEdit" AutoPostBack="true"
								     OnTextChanged="AccountEdit_TextChanged" MaxLength="30" 
								     CssClass="TextBox" Width="45%" Text='<%# DataBinder.Eval(Container.DataItem, "Account") %>'>
									</asp:TextBox>
									  <asp:TextBox ID="AccountNameEdit" Runat="Server" Enabled ="false"
								    MaxLength="60" CssClass="TextBoxR" Width="40%" Text='<%# DataBinder.Eval(Container.DataItem, "AccountName") %>'/> 
								    
									<asp:Button ID="btnAccountEdit" runat="server"  class="btngo" Text="..." CommandName="SearchEdit"/> <%--OnClick="btnAccountAdd_Click"--%>
									<cc1:TextBoxWatermarkExtender ID="AccountEdit_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="AccountEdit" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>
                                     
									<%--<asp:DropDownList Runat="server" ID="AccountEdit" Enabled="False" CssClass="DropDownList" Width="100%" 
									    AutoPostBack="True" OnSelectedIndexChanged="AccountEdit_SelectedIndexChanged"
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Account") %>' 
                                        DataSourceID="dsProductAccount" DataTextField="Product_Account_Name" 
                                        DataValueField="Product_Account_Code">									    
									</asp:DropDownList>--%>								    
								</EditItemTemplate>
								<FooterTemplate>
								    <asp:TextBox ID="AccountAdd" Runat="Server" AutoPostBack="True" 
								    OnTextChanged="AccountAdd_TextChanged" MaxLength="20" CssClass="TextBox" Width="35%"/> 
								      <asp:TextBox ID="AccountNameAdd" Runat="Server" MaxLength="60" CssClass="TextBoxR" Enabled="false" Width="50%"/> 
								    <asp:Button ID="btnAccountAdd" runat="server" class="btngo" Text="..."  CommandName="SearchAdd" />  <%--OnClick="btnAccountAdd_Click"--%>
									<cc1:TextBoxWatermarkExtender ID="AccountAdd_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="StockTypeNameAdd" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>
                                     
									<%--<asp:DropDownList ID="AccountAdd" Runat="Server" Width="100%" CssClass="DropDownList"
									    AutoPostBack="True" OnSelectedIndexChanged="AccountAdd_SelectedIndexChanged"									    
                                        DataSourceID="dsProductAccount" DataTextField="Product_Account_Name" 
                                        DataValueField="Product_Account_Code">
									</asp:DropDownList>--%>
								</FooterTemplate>
							</asp:TemplateField>
							<%--OnSelectedIndexChanged="ddlAccount_SelectedIndexChanged" --%>
							<asp:TemplateField HeaderText="Type" HeaderStyle-Width="100" SortExpression="Type">
								<Itemtemplate>
									<asp:Label Runat="server" ID="Type" text='<%# DataBinder.Eval(Container.DataItem, "Type") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:TextBox Runat="server" ID="TypeEdit" Enabled="False" MaxLength="5" CssClass="TextBox" Width="100%" Text='<%# DataBinder.Eval(Container.DataItem, "Type") %>'>
									</asp:TextBox>
									<cc1:TextBoxWatermarkExtender ID="TypeEdit_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="TypeEdit" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="TypeAdd" Enabled="False" Runat="Server" MaxLength="15" CssClass="TextBox" Width="100%"/>
									<cc1:TextBoxWatermarkExtender ID="TypeAdd_WtExt" 
                                         runat="server" Enabled="True" TargetControlID="TypeAdd" 
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
        
       <%-- <asp:SqlDataSource ID="dsProductAccount" runat="server"            
           SelectCommand="EXEC S_GetProductAccount">
        </asp:SqlDataSource>--%>
     <asp:Label ID="lstatus" ForeColor="red" runat="server"></asp:Label>        
    </div>
    </form>
</body>
</html>
