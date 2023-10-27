<%@ Page Language="VB" AutoEventWireup="false" CodeFile="MsCity.aspx.vb" Inherits="Master_MsCity_MsCity" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>City File</title>
    <script src="../../Function/OpenDlg.JS" type="text/javascript"></script> 
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div class="Content">
     <div class="H1">City File</div>
     <hr style="color:Blue" />
      <table>
        <tr>
            <td style="width:100px;text-align:right" >Quick Search :</td>
            <td><asp:TextBox runat="server" ID ="tbFilter" CssClass="TextBox"/> 
                <asp:DropDownList runat="server" ID="ddlField" CssClass="DropDownList" >
                  <asp:ListItem Selected="true" Text="City Code" Value="City_Code"></asp:ListItem>
                  <asp:ListItem Text="City Name" Value="City_Name"></asp:ListItem>        
                    <asp:ListItem Value="Regional_Name">Regional</asp:ListItem>
                    <asp:ListItem Value="Country_Name">Country</asp:ListItem>
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
                    <asp:ListItem Selected="true" Text="City Code" Value="City_Code"></asp:ListItem>
                    <asp:ListItem Text="City Name" Value="City_Name"></asp:ListItem> 
                      <asp:ListItem Value="Regional_Name">Regional</asp:ListItem>
                      <asp:ListItem Value="Country_Name">Country</asp:ListItem>
                  </asp:DropDownList>
                
            </td>
        </tr>
     </table>
     </asp:Panel>
      <br />
      <%--<div style="border:0px  solid; width:100%; height:100%; overflow:auto;">--%>
      <asp:GridView id="DataGrid" runat="server" ShowFooter="True" AllowSorting="True" 
            AutoGenerateColumns="False" AllowPaging="True" CssClass="Grid">
						<HeaderStyle CssClass="GridHeader"></HeaderStyle>
						<RowStyle CssClass="GridItem" wrap="false" />
						<AlternatingRowStyle CssClass="GridAltItem"/>
						<FooterStyle CssClass="GridFooter" />
						<PagerStyle CssClass="GridPager" HorizontalAlign="Center" />
				      <Columns>
							<asp:TemplateField HeaderText="City Code" HeaderStyle-Width="110" SortExpression="City_Code">
								<Itemtemplate>
									<asp:Label Runat="server" ID="CityCode" text='<%# DataBinder.Eval(Container.DataItem, "City_Code") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:Label Runat="server" ID="CityCodeEdit" Text='<%# DataBinder.Eval(Container.DataItem, "City_Code") %>'>
									</asp:Label>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="CityCodeAdd" Placeholder = "can't blank" CssClass="TextBox" MaxLength="5" Runat="Server" Width="100%"/>
									<%--<cc1:TextBoxWatermarkExtender ID="CityCodeAdd_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="CityCodeAdd" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>--%>
								</FooterTemplate>
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="City Name" HeaderStyle-Width="320" SortExpression="City_Name">
								<Itemtemplate>
									<asp:Label Runat="server" ID="CityName" text='<%# DataBinder.Eval(Container.DataItem, "City_Name") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:TextBox Runat="server" ID="CityNameEdit" CssClass="TextBox" MaxLength="60" Width="100%" Text='<%# DataBinder.Eval(Container.DataItem, "City_Name") %>'>
									</asp:TextBox>
									<cc1:TextBoxWatermarkExtender ID="CityNameEdit_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="CityNameEdit" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="CityNameAdd" Placeholder = "can't blank" Runat="Server" CssClass="TextBox" MaxLength="60" Width="100%"/>
									<%--<cc1:TextBoxWatermarkExtender ID="CityNameAdd_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="CityNameAdd" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>--%>
								</FooterTemplate>
							</asp:TemplateField>		
							
							<asp:TemplateField HeaderText="Regional" HeaderStyle-Width="250" SortExpression="Regional_Name">
								<Itemtemplate>
									<asp:Label Runat="server" ID="Regional" text='<%# DataBinder.Eval(Container.DataItem, "Regional_Name") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:DropDownList Runat="server" ID="RegionalEdit" Width="100%" CssClass="DropDownList" 
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Regional_Code") %>' 
                                        DataSourceID="dsRegional" DataTextField="RegionalName" 
                                        DataValueField="RegionalCode">
									    
									</asp:DropDownList>								    
								</EditItemTemplate>
								<FooterTemplate>
									<asp:DropDownList ID="RegionalAdd" Runat="Server" Width="100%" CssClass="DropDownList"									    
                                        DataSourceID="dsRegional" DataTextField="RegionalName" 
                                        DataValueField="RegionalCode">
									</asp:DropDownList>
								</FooterTemplate>
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Country" HeaderStyle-Width="250" SortExpression="CountryName">
								<Itemtemplate>
									<asp:Label Runat="server" ID="Country" text='<%# DataBinder.Eval(Container.DataItem, "CountryName") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:DropDownList Runat="server" ID="CountryEdit" Width="100%" CssClass="DropDownList"
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Country") %>' 
                                        DataSourceID="dsCountry" DataTextField="Country_Name" 
                                        DataValueField="Country_Code">
									    
									</asp:DropDownList>								    
								</EditItemTemplate>
								<FooterTemplate>
									<asp:DropDownList ID="CountryAdd" Runat="Server" Width="100%" CssClass="DropDownList"									    
                                        DataSourceID="dsCountry" DataTextField="Country_Name" 
                                        DataValueField="Country_Code">
									</asp:DropDownList>
								</FooterTemplate>
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Action" headerstyle-width="170" >
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
        </asp:gridview>
        
        <asp:SqlDataSource ID="dsRegional" runat="server"                 
                SelectCommand="EXEC S_GetRegional">
        </asp:SqlDataSource>
        
        <asp:SqlDataSource ID="dsCountry" runat="server"                 
                SelectCommand="EXEC S_GetCountry">
        </asp:SqlDataSource>
                
     <asp:Label ID="lstatus" ForeColor="red" runat="server"></asp:Label>        
    </div>
    </form>
</body>
</html>
