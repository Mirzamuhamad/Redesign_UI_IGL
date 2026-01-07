<%@ Page Language="VB" AutoEventWireup="false" CodeFile="MsMenuPortal.aspx.vb" Inherits="MsMenuPortal" %>
	<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
		<!DOCTYPE html
			PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

		<html xmlns="http://www.w3.org/1999/xhtml">

		<head runat="server">
			<title>Kavling Portal</title>
			<script src="../../Function/OpenDlg.js" type="text/javascript"></script>
			<link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
		</head>

		<body>
			<form id="form1" runat="server">
				<asp:ScriptManager ID="ScriptManager1" runat="server">
				</asp:ScriptManager>
				<div class="Content">
					<div class="H1">Menu Portal</div>
					<hr CssClass="Hr" />
					<table>
						<tr>
							<td style="width:100px;text-align:left">Quick Search :</td>
							<td>
								<asp:TextBox runat="server" ID="tbFilter" CssClass="TextBox" />
								<asp:DropDownList runat="server" ID="ddlField" CssClass="DropDownList">
									<asp:ListItem Selected="true" Text="Title" Value="Title"></asp:ListItem>
									<asp:ListItem Text="Category" Value="Category"></asp:ListItem>
								</asp:DropDownList>
								<asp:Button class="bitbtn btnsearch" runat="server" ID="btnSearch" Text="Search" />
								<asp:Button class="btngo" runat="server" ID="btnExpand" Text="..." />
								<asp:Button class="bitbtn btnprint" runat="server" ID="btnPrint" Text="Print" />
							</td>
						</tr>
					</table>
					<asp:Panel runat="server" ID="pnlSearch" Visible="false">
						<table>
							<tr>
								<td style="width:100px;text-align:right">
									<asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlNotasi">
										<asp:ListItem Selected="true" Text="OR" Value="OR"></asp:ListItem>
										<asp:ListItem Text="AND" Value="AND"></asp:ListItem>
									</asp:DropDownList>
								</td>
								<td>
									<asp:TextBox runat="server" ID="tbfilter2" CssClass="TextBox" />
									<asp:DropDownList runat="server" ID="ddlField2" CssClass="DropDownList">
										<asp:ListItem Selected="true" Text="Title" Value="Title"></asp:ListItem>
										<asp:ListItem Text="Category" Value="Category"></asp:ListItem>

									</asp:DropDownList>

								</td>
							</tr>
						</table>
					</asp:Panel>
					<br />
					<asp:GridView id="DataGrid" runat="server" ShowFooter="True" AllowSorting="True"
						AutoGenerateColumns="False" AllowPaging="True" CssClass="Grid">
						<HeaderStyle CssClass="GridHeader"></HeaderStyle>
						<RowStyle CssClass="GridItem" Wrap="false" />
						<AlternatingRowStyle CssClass="GridAltItem" />
						<FooterStyle CssClass="GridFooter" />
						<PagerStyle CssClass="GridPager" />
						<Columns>

							<asp:TemplateField HeaderText="Menu Id" Visible = "False" HeaderStyle-Width="50"
								SortExpression="MenuId">
								<Itemtemplate>
									<asp:Label Runat="server" ID="MenuId"
										text='<%# DataBinder.Eval(Container.DataItem, "MenuId") %>'>
									</asp:Label>
								</Itemtemplate>							

								<HeaderStyle Width="100px"></HeaderStyle>
							</asp:TemplateField>

							<asp:TemplateField HeaderText="Menu Name" HeaderStyle-Width="200"
								SortExpression="Title">
								<Itemtemplate>
									<asp:Label Runat="server" ID="Title"
										text='<%# DataBinder.Eval(Container.DataItem, "Title") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:TextBox Runat="server" CssClass="TextBox" ID="TitleEdit" MaxLength="40"
										Width="100%" Text='<%# DataBinder.Eval(Container.DataItem, "Title") %>'>
									</asp:TextBox>
									<cc1:TextBoxWatermarkExtender ID="TitleEdit_WtExt" runat="server"
										Enabled="True" TargetControlID="TitleEdit" WatermarkText="can't blank"
										WatermarkCssClass="Watermarked">
									</cc1:TextBoxWatermarkExtender>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox Kegiatanholder="can't blank" ID="TitleAdd" CssClass="TextBox"
										MaxLength="10" Runat="Server" Width="100%" />
								</FooterTemplate>

								<HeaderStyle Width="200px"></HeaderStyle>
							</asp:TemplateField>

							<asp:TemplateField HeaderText="Icon Path" HeaderStyle-Width="200"
								SortExpression="IconPath">
								<Itemtemplate>
									<asp:Label Runat="server" ID="IconPath"
										text='<%# DataBinder.Eval(Container.DataItem, "IconPath") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:TextBox Runat="server" CssClass="TextBox" ID="IconPathEdit" MaxLength="40"
										Width="100%" Text='<%# DataBinder.Eval(Container.DataItem, "IconPath") %>'>
									</asp:TextBox>
									<cc1:TextBoxWatermarkExtender ID="IconPathEdit_WtExt" runat="server"
										Enabled="True" TargetControlID="IconPathEdit" WatermarkText="can't blank"
										WatermarkCssClass="Watermarked">
									</cc1:TextBoxWatermarkExtender>

								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox Kegiatanholder="can't blank" ID="IconPathAdd" CssClass="TextBox"
										MaxLength="10" Runat="Server" Width="100%" />
								</FooterTemplate>

								<HeaderStyle Width="200px"></HeaderStyle>
							</asp:TemplateField>


							<asp:TemplateField HeaderText="Url " HeaderStyle-Width="200"
								SortExpression="Url">
								<Itemtemplate>
									<asp:Label Runat="server" ID="Url"
										text='<%# DataBinder.Eval(Container.DataItem, "Url") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:TextBox Runat="server" CssClass="TextBox" ID="UrlEdit" MaxLength="40"
										Width="100%" Text='<%# DataBinder.Eval(Container.DataItem, "Url") %>'>
									</asp:TextBox>
									<cc1:TextBoxWatermarkExtender ID="UrlEdit_WtExt" runat="server"
										Enabled="True" TargetControlID="UrlEdit" WatermarkText="can't blank"
										WatermarkCssClass="Watermarked">
									</cc1:TextBoxWatermarkExtender>
								</EditItemTemplate>

								<FooterTemplate>
									<asp:TextBox Kegiatanholder="can't blank" ID="UrlAdd" CssClass="TextBox"
										MaxLength="100" Runat="Server" Width="100%">
									</asp:TextBox>
								</FooterTemplate>
							</asp:TemplateField>

						
							
							<asp:TemplateField HeaderText="Category" HeaderStyle-Width="200"
								ItemStyle-HorizontalAlign="Center" SortExpression="Category">
								<Itemtemplate>
									<asp:Label Runat="server" Width="150" ID="Category"
										text='<%# DataBinder.Eval(Container.DataItem, "Category") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:DropDownList ID="CategoryEdit" CssClass="DropDownList" Width="100%"
										runat="server"
										SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Category") %>'>
										<asp:ListItem Selected="True">Properti</asp:ListItem>
										<asp:ListItem >Service</asp:ListItem>
										<asp:ListItem >Keamanan</asp:ListItem>
										<asp:ListItem >Informasi</asp:ListItem>
										<asp:ListItem >Lainnya</asp:ListItem>
									</asp:DropDownList>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:DropDownList ID="CategoryAdd" CssClass="DropDownList" Width="100%"
										runat="server">
										<asp:ListItem Selected="True">Properti</asp:ListItem>
										<asp:ListItem >Service</asp:ListItem>
										<asp:ListItem >Keamanan</asp:ListItem>
										<asp:ListItem >Informasi</asp:ListItem>
										<asp:ListItem >Lainnya</asp:ListItem>
										
									</asp:DropDownList>
								</FooterTemplate>
								<HeaderStyle Width="200px" />
								<ItemStyle HorizontalAlign="Center" />
							</asp:TemplateField>	


							<asp:TemplateField HeaderText="Menu Home" HeaderStyle-Width="200"
								ItemStyle-HorizontalAlign="Center" SortExpression="FgIsHome">
								<Itemtemplate>
									<asp:Label Runat="server" Width="150" ID="FgIsHome"
										text='<%# DataBinder.Eval(Container.DataItem, "FgIsHome") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:DropDownList ID="FgIsHomeEdit" CssClass="DropDownList" Width="100%"
										runat="server"
										SelectedValue='<%# DataBinder.Eval(Container.DataItem, "FgIsHome") %>'>
										<asp:ListItem >Y</asp:ListItem>
										<asp:ListItem Selected="True">N</asp:ListItem>
									</asp:DropDownList>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:DropDownList ID="FgIsHomeAdd" CssClass="DropDownList" Width="100%"
										runat="server">
										<asp:ListItem >Y</asp:ListItem>
										<asp:ListItem Selected="True">N</asp:ListItem>
									</asp:DropDownList>
								</FooterTemplate>
								<HeaderStyle Width="200px" />
								<ItemStyle HorizontalAlign="Center" />
							</asp:TemplateField>	
							

							

							<asp:TemplateField HeaderText="Action" headerstyle-width="126">
								<ItemTemplate>
									<asp:Button style="height: 20px;" ID="btnEdit" runat="server"
										class="bitbtndt btnedit" Text="Edit" CommandName="Edit" />
									<asp:Button style="height: 20px;" ID="btndelete" runat="server"
										class="bitbtndt btndelete" Text="Delete"
										OnClientClick="return confirm('Sure to delete this data?');"
										CommandName="Delete" />
								</ItemTemplate>
								<EditItemTemplate>
									<asp:Button style="height: 20px;" ID="btnUpdate" runat="server"
										class="bitbtndt btnsave" Text="Save" CommandName="Update" />
									<asp:Button style="height: 20px;" ID="btnCancel" runat="server"
										class="bitbtndt btncancel" Text="Cancel" CommandName="Cancel" />
								</EditItemTemplate>
								<FooterTemplate>
									<asp:Button class="bitbtndt btnadd" runat="server" ID="btnAdd" Text="Add"
										CommandName="Insert" />
								</FooterTemplate>

								<HeaderStyle Width="126px"></HeaderStyle>
							</asp:TemplateField>

						</Columns>
					</asp:GridView>

                    <asp:SqlDataSource ID="dsTypeIjin" runat="server"            
                       SelectCommand="SELECT * FROM V_TypeIjin">
                    </asp:SqlDataSource>
                    
					<asp:Label ID="lstatus" ForeColor="red" runat="server"></asp:Label>
				</div>


				<script type="text/javascript">
    function hanyaAngka(evt) {
        var charCode = (evt.which) ? evt.which : event.keyCode;
        // Hanya izinkan angka (0–9)
        if (charCode < 48 || charCode > 57) {
            evt.preventDefault();
            return false;
        }
        return true;
    }
</script>

			</form>
		</body>

		</html>