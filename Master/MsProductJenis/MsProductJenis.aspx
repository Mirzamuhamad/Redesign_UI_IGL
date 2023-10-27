<%@ Page Language="VB" AutoEventWireup="false" CodeFile="MsProductJenis.aspx.vb" Inherits="MsProductJenis_MsProductJenis" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Product JEnisFile</title>
    <script src="../../Function/OpenDlg.JS" type="text/javascript"></script> 
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div class="Content">
     <div class="H1"><asp:Label runat="server" ID="lblTitle"></asp:Label></div>
     <hr style="color:Blue" />
      <table>
        <tr>
            <td style="width:100px;text-align:right" >Quick Search :</td>
            <td><asp:TextBox runat="server" ID ="tbFilter" CssClass="TextBox" /> 
                <asp:DropDownList runat="server" ID="ddlField" CssClass="DropDownList" > 
                    <asp:ListItem Selected="true" Text="Jenis Code" Value="JenisCode"></asp:ListItem>
                    <asp:ListItem Text="Jenis Name" Value="JenisName"></asp:ListItem> 
                    <asp:ListItem Text="Group Name" Value="Materi"></asp:ListItem> 
                    <asp:ListItem Text="Jenis No" Value="JenisNo"></asp:ListItem>                    
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
                    <asp:ListItem Selected="true" Text="Jenis Code" Value="JenisCode"></asp:ListItem>
                    <asp:ListItem Text="Jenis Name" Value="JenisName"></asp:ListItem> 
                    <asp:ListItem Text="Materi" Value="Materi"></asp:ListItem>
                    <asp:ListItem Text="Jenis No" Value="JenisNo"></asp:ListItem>                               
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
							<asp:TemplateField HeaderText="Product Jenis Code" HeaderStyle-Width="160" SortExpression="JenisCode">
								<Itemtemplate>
									<asp:Label Runat="server" ID="JenisCode" text='<%# DataBinder.Eval(Container.DataItem, "JenisCode") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:Label Runat="server" ID="JenisCodeEdit" Text='<%# DataBinder.Eval(Container.DataItem, "JenisCode") %>'>
									</asp:Label>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="JenisCodeAdd"  Enabled="false" CssClass="TextBox" MaxLength="6" Runat="Server" Width="100%"/>
									<cc1:TextBoxWatermarkExtender ID="JenisCodeAdd_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="JenisCodeAdd" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>
								</FooterTemplate>
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Product Jenis Name" HeaderStyle-Width="300" SortExpression="JenisName">
								<Itemtemplate>
									<asp:Label Runat="server" ID="JenisName" text='<%# DataBinder.Eval(Container.DataItem, "JenisName") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:TextBox Runat="server" ID="JenisNameEdit" MaxLength="60" CssClass="TextBox" Width="100%" Text='<%# DataBinder.Eval(Container.DataItem, "JenisName") %>'>
									</asp:TextBox>
									<cc1:TextBoxWatermarkExtender ID="JenisNameEdit_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="JenisNameEdit" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="JenisNameAdd" Runat="Server" MaxLength="60" CssClass="TextBox" Width="100%"/>
									<cc1:TextBoxWatermarkExtender ID="JenisNameAdd_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="JenisNameAdd" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>
								</FooterTemplate>
							</asp:TemplateField>		
							
							<asp:TemplateField HeaderText="Product Materi" HeaderStyle-Width="220" SortExpression="MateriName">
								<Itemtemplate>
									<asp:Label Runat="server" ID="Materi" text='<%# DataBinder.Eval(Container.DataItem, "MateriName") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:DropDownList Runat="server" ID="MateriEdit" Enabled="False" CssClass="DropDownList" Width="100%" 
									    AutoPostBack="True" OnSelectedIndexChanged="MateriEdit_SelectedIndexChanged"
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Materi") %>' 
                                        DataSourceID="dsProductMateri" DataTextField="Product_Materi_Name" 
                                        DataValueField="Product_Materi_Code">									    
									</asp:DropDownList>								    
								</EditItemTemplate>
								<FooterTemplate>
									<asp:DropDownList ID="MateriAdd" Runat="Server" Width="100%" CssClass="DropDownList"
									    AutoPostBack="True" OnSelectedIndexChanged="MateriAdd_SelectedIndexChanged"									    
                                        DataSourceID="dsProductMateri" DataTextField="Product_Materi_Name" 
                                        DataValueField="Product_Materi_Code">
									</asp:DropDownList>
								</FooterTemplate>
							</asp:TemplateField>
							<%--OnSelectedIndexChanged="ddlMateri_SelectedIndexChanged" --%>
							<asp:TemplateField HeaderText="Jenis No" HeaderStyle-Width="100" SortExpression="JenisNo">
								<Itemtemplate>
									<asp:Label Runat="server" ID="JenisNo" text='<%# DataBinder.Eval(Container.DataItem, "JenisNo") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:TextBox Runat="server" ID="JenisNoEdit" Enabled="False" MaxLength="2" CssClass="TextBox" Width="100%" AutoPostBack="true" Text='<%# DataBinder.Eval(Container.DataItem, "JenisNo") %>'>
									</asp:TextBox>
									<cc1:TextBoxWatermarkExtender ID="JenisNoEdit_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="JenisNoEdit" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="JenisNoAdd" AutoPostBack="True" OnTextChanged="JenisNoAdd_TextChanged" Runat="Server" MaxLength="2" CssClass="TextBox" Width="100%"/>
									<cc1:TextBoxWatermarkExtender ID="JenisNoAdd_WtExt" 
                                         runat="server" Enabled="True" TargetControlID="JenisNoAdd" 
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
        
        <asp:SqlDataSource ID="dsProductMateri" runat="server"            
           SelectCommand="EXEC S_GetProductMateri">
        </asp:SqlDataSource>
     <asp:Label ID="lstatus" ForeColor="red" runat="server"></asp:Label>        
    </div>
    </form>
</body>
</html>
