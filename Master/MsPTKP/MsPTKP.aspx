<%@ Page Language="VB" AutoEventWireup="false" CodeFile="MsPTKP.aspx.vb" Inherits="Master_MsPTKP_MsPTKP" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
    <script src="../../Function/Function.JS" type="text/javascript"></script>
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
 <link href="https://fonts.googleapis.com/css2?family=Muli:wght@300;400;600&display=swap" rel="stylesheet">
</head>
<body>
    <form id="form1" runat="server">
    <div class="Content">
     <div class="H1">PTKP File</div>
     <hr style="color:Blue" />
         <asp:GridView id="DataGrid" runat="server"
            AutoGenerateColumns="False" CssClass="Grid" AllowSorting = "true">
				<HeaderStyle CssClass="GridHeader"></HeaderStyle>
				<RowStyle CssClass="GridItem" Wrap="false"/>
				<AlternatingRowStyle CssClass="GridAltItem"/>
				<FooterStyle CssClass="GridFooter" />
				<PagerStyle CssClass="GridPager" HorizontalAlign="Center" />
		      <Columns>
		        <asp:TemplateField HeaderText="PTKP Code" HeaderStyle-Width = "35px" SortExpression ="PTKPCode">
					<Itemtemplate>
						<asp:Label Runat="server" ID="PTKPCode" text='<%# DataBinder.Eval(Container.DataItem, "PTKPCode") %>'>
						</asp:Label>
					</Itemtemplate>
					<EditItemTemplate>
						<asp:Label Runat="server" ID="PTKPCodeEdit" Text='<%# DataBinder.Eval(Container.DataItem, "PTKPCode") %>'>
						</asp:Label>
					</EditItemTemplate>	
				</asp:TemplateField>
				<asp:TemplateField HeaderText="PTKP Name" HeaderStyle-Width = "100px" SortExpression ="PTKPName">
					<Itemtemplate>
						<asp:Label Runat="server" ID="PTKPName" text='<%# DataBinder.Eval(Container.DataItem, "PTKPName") %>'>
						</asp:Label>
					</Itemtemplate>
					<EditItemTemplate>
						<asp:Label Runat="server" ID="PTKPNameEdit" text='<%# DataBinder.Eval(Container.DataItem, "PTKPName") %>'>
						</asp:Label>
					</EditItemTemplate>	
				</asp:TemplateField>
		        <asp:TemplateField HeaderText="PTKP Rate" HeaderStyle-Width = "150px" SortExpression ="PTKPRate" ItemStyle-HorizontalAlign="Right" >
					<Itemtemplate>
						<asp:Label Runat="server" ID="PTKPRate" text='<%# DataBinder.Eval(Container.DataItem, "PTKPRateStr") %>'>
						</asp:Label>
					</Itemtemplate>
					<EditItemTemplate>
						<asp:TextBox Runat="server" ID="PTKPRateEdit" CssClass="TextBox" Text='<%# DataBinder.Eval(Container.DataItem, "PTKPRateStr") %>' MaxLength = "100">
						</asp:TextBox>
					</EditItemTemplate>	
				</asp:TemplateField>
				
				<asp:TemplateField HeaderText="Action" >
						<ItemTemplate>		
						    <asp:Button ID="btnEdit" runat="server" class="bitbtndt btnedit" Text="Edit" CommandName="Edit"/>										             
						</ItemTemplate>
						<EditItemTemplate>
						    <asp:Button ID="btnUpdate" runat="server" class="bitbtndt btnsave" Text="Save" CommandName="Update"/>									
                            <asp:Button ID="btnCancel" runat="server" class="bitbtndt btncancel" Text="Cancel" CommandName="Cancel"/>																
						</EditItemTemplate>						
					</asp:TemplateField>							
			</Columns>
        </asp:GridView>  
    </div>
    <br />
    <asp:Label ID="lbStatus" runat="server" ForeColor="Red" />
    </form>
</body>
</html>
