<%@ Page Language="VB" AutoEventWireup="false" CodeFile="MsKavlingPortal.aspx.vb" Inherits="MsKavlingPortal" %>
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
					<div class="H1">Kavling Portal</div>
					<hr CssClass="Hr" />
					<table>
						<tr>
							<td style="width:100px;text-align:left">Quick Search :</td>
							<td>
								<asp:TextBox runat="server" ID="tbFilter" CssClass="TextBox" />
								<asp:DropDownList runat="server" ID="ddlField" CssClass="DropDownList">
									<asp:ListItem Selected="true" Text="Code" Value="KavlingCode"></asp:ListItem>
									<asp:ListItem Text="Name" Value="Blok"></asp:ListItem>
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
										<asp:ListItem Selected="true" Text="Code" Value="KavlingCode"></asp:ListItem>
										<asp:ListItem Text="Description" Value="Blok"></asp:ListItem>

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

							<asp:TemplateField HeaderText="Kavling Id" Visible = "true" HeaderStyle-Width="50"
								SortExpression="KavlingId">
								<Itemtemplate>
									<asp:Label Runat="server" ID="KavlingId"
										text='<%# DataBinder.Eval(Container.DataItem, "KavlingId") %>'>
									</asp:Label>
								</Itemtemplate>							

								<HeaderStyle Width="100px"></HeaderStyle>
							</asp:TemplateField>

							<asp:TemplateField HeaderText="Kavling" HeaderStyle-Width="200"
								SortExpression="KavlingCode">
								<Itemtemplate>
									<asp:Label Runat="server" ID="KavlingCode"
										text='<%# DataBinder.Eval(Container.DataItem, "KavlingCode") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:Label Runat="server" ID="KavlingCode"
										text='<%# DataBinder.Eval(Container.DataItem, "KavlingCode") %>'>
									</asp:Label>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox Kegiatanholder="can't blank" ID="KavlingCodeAdd" CssClass="TextBox"
										MaxLength="10" Runat="Server" Width="100%" />

								</FooterTemplate>

								<HeaderStyle Width="200px"></HeaderStyle>
							</asp:TemplateField>

							<asp:TemplateField HeaderText="Blok " HeaderStyle-Width="200"
								SortExpression="Blok">
								<Itemtemplate>
									<asp:Label Runat="server" ID="Blok"
										text='<%# DataBinder.Eval(Container.DataItem, "Blok") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:TextBox Runat="server" CssClass="TextBox" ID="BlokEdit" MaxLength="40"
										Width="100%" Text='<%# DataBinder.Eval(Container.DataItem, "Blok") %>'>
									</asp:TextBox>
									<cc1:TextBoxWatermarkExtender ID="BlokEdit_WtExt" runat="server"
										Enabled="True" TargetControlID="BlokEdit" WatermarkText="can't blank"
										WatermarkCssClass="Watermarked">
									</cc1:TextBoxWatermarkExtender>
								</EditItemTemplate>

								<FooterTemplate>
									<asp:TextBox Kegiatanholder="can't blank" ID="BlokAdd" CssClass="TextBox"
										MaxLength="100" Runat="Server" Width="100%">
									</asp:TextBox>
								</FooterTemplate>
							</asp:TemplateField>

											
							


							<asp:TemplateField HeaderText="Luas Kavling " HeaderStyle-Width="200"
								SortExpression="Luas">
								<Itemtemplate>
									<asp:Label Runat="server" ID="Luas"
										text='<%# DataBinder.Eval(Container.DataItem, "Luas") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:TextBox runat="server" CssClass="TextBox" ID="LuasEdit"
										MaxLength="40" Width="100%"
										Text='<%# DataBinder.Eval(Container.DataItem, "Luas") %>'
										onkeypress="return hanyaAngka(event)">
									</asp:TextBox>

									<cc1:TextBoxWatermarkExtender ID="LuasEdit_WtExt" runat="server"
										Enabled="True" TargetControlID="LuasEdit"
										WatermarkText="can't blank" WatermarkCssClass="Watermarked">
									</cc1:TextBoxWatermarkExtender>
								</EditItemTemplate>

								<FooterTemplate>
									<asp:TextBox ID="LuasAdd" CssClass="TextBox" MaxLength="100"
										runat="server" Width="100%" onkeypress="return hanyaAngka(event)">
									</asp:TextBox>
								</FooterTemplate>
								<HeaderStyle Width="200"></HeaderStyle>
							</asp:TemplateField>

							
							<asp:TemplateField HeaderText="Status" HeaderStyle-Width="200"
								ItemStyle-HorizontalAlign="Center" SortExpression="Status">
								<Itemtemplate>
									<asp:Label Runat="server" Width="150" ID="Status"
										text='<%# DataBinder.Eval(Container.DataItem, "Status") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:DropDownList ID="StatusEdit" CssClass="DropDownList" Width="100%"
										runat="server"
										SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Status") %>'>
										<asp:ListItem>OCCUPIED</asp:ListItem>
										<asp:ListItem Selected="True">AVAILABLE</asp:ListItem>
									</asp:DropDownList>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:DropDownList ID="StatusAdd" CssClass="DropDownList" Width="100%"
										runat="server">
										<asp:ListItem >OCCUPIED</asp:ListItem>
										<asp:ListItem Selected="True">AVAILABLE</asp:ListItem>
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