<%@ Page Language="VB" AutoEventWireup="false" CodeFile="MsKegiatan.aspx.vb" Inherits="Master_Kegiatan" %>
	<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
		<!DOCTYPE html
			PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

		<html xmlns="http://www.w3.org/1999/xhtml">

		<head runat="server">
			<title>Kegiatan File</title>
			<script src="../../Function/OpenDlg.js" type="text/javascript"></script>
			<link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
		</head>

		<body>
			<form id="form1" runat="server">
				<asp:ScriptManager ID="ScriptManager1" runat="server">
				</asp:ScriptManager>
				<div class="Content">
					<div class="H1">Kegiatan File</div>
					<hr CssClass="Hr" />
					<table>
						<tr>
							<td style="width:100px;text-align:left">Quick Search :</td>
							<td>
								<asp:TextBox runat="server" ID="tbFilter" CssClass="TextBox" />
								<asp:DropDownList runat="server" ID="ddlField" CssClass="DropDownList">
									<asp:ListItem Selected="true" Text="Code" Value="KegiatanCode"></asp:ListItem>
									<asp:ListItem Text="Name" Value="KegiatanName"></asp:ListItem>
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
										<asp:ListItem Selected="true" Text="Code" Value="KegiatanCode"></asp:ListItem>
										<asp:ListItem Text="Description" Value="KegiatanName"></asp:ListItem>

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
							<asp:TemplateField HeaderText="KegiatanCode" HeaderStyle-Width="100"
								SortExpression="KegiatanCode">
								<Itemtemplate>
									<asp:Label Runat="server" ID="KegiatanCode"
										text='<%# DataBinder.Eval(Container.DataItem, "KegiatanCode") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:Label Runat="server" ID="KegiatanCode"
										text='<%# DataBinder.Eval(Container.DataItem, "KegiatanCode") %>'>
									</asp:Label>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox Kegiatanholder="can't blank" ID="KegiatanCodeAdd" CssClass="TextBox"
										MaxLength="5" Runat="Server" Width="100%" />

								</FooterTemplate>

								<HeaderStyle Width="100px"></HeaderStyle>
							</asp:TemplateField>

							<asp:TemplateField HeaderText="KegiatanName " HeaderStyle-Width="320"
								SortExpression="KegiatanName">
								<Itemtemplate>
									<asp:Label Runat="server" ID="KegiatanName"
										text='<%# DataBinder.Eval(Container.DataItem, "KegiatanName") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:TextBox Runat="server" CssClass="TextBox" ID="KegiatanNameEdit" MaxLength="40"
										Width="100%" Text='<%# DataBinder.Eval(Container.DataItem, "KegiatanName") %>'>
									</asp:TextBox>
									<cc1:TextBoxWatermarkExtender ID="KegiatanNameEdit_WtExt" runat="server"
										Enabled="True" TargetControlID="KegiatanNameEdit" WatermarkText="can't blank"
										WatermarkCssClass="Watermarked">
									</cc1:TextBoxWatermarkExtender>
								</EditItemTemplate>

								<FooterTemplate>
									<asp:TextBox Kegiatanholder="can't blank" ID="KegiatanNameAdd" CssClass="TextBox"
										MaxLength="100" Runat="Server" Width="100%">
									</asp:TextBox>
								</FooterTemplate>


								<HeaderStyle Width="320"></HeaderStyle>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Masa Berlaku" HeaderStyle-Width="150"
								ItemStyle-HorizontalAlign="Center" SortExpression="FgMasaBerlaku">
								<Itemtemplate>
									<asp:Label Runat="server" Width="150" ID="MasaBerlaku"
										text='<%# DataBinder.Eval(Container.DataItem, "FgMasaBerlaku") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:DropDownList ID="MasaBerlakuEdit" CssClass="DropDownList" Width="100%"
										runat="server"
										SelectedValue='<%# DataBinder.Eval(Container.DataItem, "FgMasaBerlaku") %>'>
										<asp:ListItem>Y</asp:ListItem>
										<asp:ListItem Selected="True">N</asp:ListItem>
									</asp:DropDownList>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:DropDownList ID="MasaBerlakuAdd" CssClass="DropDownList" Width="100%"
										runat="server">
										<asp:ListItem >Y</asp:ListItem>
										<asp:ListItem Selected="True">N</asp:ListItem>
									</asp:DropDownList>
								</FooterTemplate>
								<HeaderStyle Width="30px" />
								<ItemStyle HorizontalAlign="Center" />
							</asp:TemplateField>					
							


							<asp:TemplateField HeaderText="Dashboard Reminder " HeaderStyle-Width="150"
								SortExpression="DashboardReminder">
								<Itemtemplate>
									<asp:Label Runat="server" ID="DashboardReminder"
										text='<%# DataBinder.Eval(Container.DataItem, "DashboardReminder") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:TextBox runat="server" CssClass="TextBox" ID="DashboardReminderEdit"
										MaxLength="40" Width="100%"
										Text='<%# DataBinder.Eval(Container.DataItem, "DashboardReminder") %>'
										onkeypress="return hanyaAngka(event)">
									</asp:TextBox>

									<cc1:TextBoxWatermarkExtender ID="DashboardReminderEdit_WtExt" runat="server"
										Enabled="True" TargetControlID="DashboardReminderEdit"
										WatermarkText="can't blank" WatermarkCssClass="Watermarked">
									</cc1:TextBoxWatermarkExtender>
								</EditItemTemplate>

								<FooterTemplate>
									<asp:TextBox ID="DashboardReminderAdd" CssClass="TextBox" MaxLength="100"
										runat="server" Width="100%" onkeypress="return hanyaAngka(event)">
									</asp:TextBox>
								</FooterTemplate>
								<HeaderStyle Width="320"></HeaderStyle>
							</asp:TemplateField>
							

							<asp:TemplateField HeaderText="Free of charge" HeaderStyle-Width="150"
								ItemStyle-HorizontalAlign="Center" SortExpression="FgInv">
								<Itemtemplate>
									<asp:Label Runat="server" Width="150" ID="Inv"
										text='<%# DataBinder.Eval(Container.DataItem, "FgInv") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:DropDownList ID="InvEdit" CssClass="DropDownList" Width="100%" runat="server"
										SelectedValue='<%# DataBinder.Eval(Container.DataItem, "FgInv") %>'>
										<asp:ListItem Selected="True">Y</asp:ListItem>
										<asp:ListItem >N</asp:ListItem>
									</asp:DropDownList>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:DropDownList ID="InvAdd" CssClass="DropDownList" Width="100%" runat="server">
										<asp:ListItem Selected="True">Y</asp:ListItem>
										<asp:ListItem >N</asp:ListItem>
									</asp:DropDownList>
								</FooterTemplate>
								<HeaderStyle Width="30px" />
								<ItemStyle HorizontalAlign="Center" />
							</asp:TemplateField>							
							
							
							<asp:TemplateField HeaderText="Type Ijin" HeaderStyle-Width="100"
								ItemStyle-HorizontalAlign="Center" SortExpression="TypeIjin">
								<Itemtemplate>
									<asp:Label Runat="server" Width="100" ID="TypeIjin"
										text='<%# DataBinder.Eval(Container.DataItem, "IjinName") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:DropDownList Runat="server" ID="TypeIjinEdit" CssClass="DropDownList" Width="100%" 
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "TypeIjin") %>' 
                                        DataSourceID="dsTypeIjin" DataTextField="IjinName" 
                                        DataValueField="IjinType">	
                                        <asp:ListItem Value="">Choose One</asp:ListItem>								    
									</asp:DropDownList>								    
								</EditItemTemplate>
								<FooterTemplate>
									<asp:DropDownList ID="TypeIjinAdd" Runat="Server" Width="100%" CssClass="DropDownList"									    
                                        DataSourceID="dsTypeIjin" DataTextField="IjinName" 
                                        DataValueField="IjinType">
									</asp:DropDownList>
								</FooterTemplate>
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