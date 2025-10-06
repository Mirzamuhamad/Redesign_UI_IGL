<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TrPrintForm.aspx.vb" Inherits="Transaction_TrPrintForm_TrPrintForm" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
 <link href="https://fonts.googleapis.com/css2?family=Muli:wght@300;400;600&display=swap" rel="stylesheet">
</head>
<body>
    <form id="form1" runat="server">
    <div class="Content">
     <div class="H1">Setup Print Form</div>
     <hr style="color:Blue" />
         <asp:GridView id="DataGrid" runat="server"
            AutoGenerateColumns="False" CssClass="Grid" AllowSorting = "true">
				<HeaderStyle CssClass="GridHeader"></HeaderStyle>
				<RowStyle CssClass="GridItem" Wrap="false"/>
				<AlternatingRowStyle CssClass="GridAltItem"/>
				<FooterStyle CssClass="GridFooter" />
				<PagerStyle CssClass="GridPager" HorizontalAlign="Center" />
		      <Columns>
		        <asp:TemplateField HeaderText="Form" HeaderStyle-Width = "35px" SortExpression ="Form">
					<Itemtemplate>
						<asp:Label Runat="server" ID="Form" text='<%# DataBinder.Eval(Container.DataItem, "Form") %>'>
						</asp:Label>
					</Itemtemplate>
					<EditItemTemplate>
						<asp:Label Runat="server" ID="FormEdit" Text='<%# DataBinder.Eval(Container.DataItem, "Form") %>'>
						</asp:Label>
					</EditItemTemplate>	
				</asp:TemplateField>
				<asp:TemplateField HeaderText="User Title" HeaderStyle-Width = "100px" SortExpression ="UserTitle">
					<Itemtemplate>
						<asp:Label Runat="server" ID="UserTitle" text='<%# DataBinder.Eval(Container.DataItem, "UserTitle") %>'>
						</asp:Label>
					</Itemtemplate>
					<EditItemTemplate>
						<asp:Label Runat="server" ID="UserTitleEdit" text='<%# DataBinder.Eval(Container.DataItem, "UserTitle") %>'>
						</asp:Label>
					</EditItemTemplate>	
				</asp:TemplateField>
		        <asp:TemplateField HeaderText="User Name" HeaderStyle-Width = "150px" SortExpression ="UserName">
					<Itemtemplate>
						<asp:Label Runat="server" ID="UserName" text='<%# DataBinder.Eval(Container.DataItem, "UserName") %>'>
						</asp:Label>
					</Itemtemplate>
					<EditItemTemplate>
						<asp:TextBox Runat="server" ID="UserNameEdit" CssClass="TextBox" Text='<%# DataBinder.Eval(Container.DataItem, "UserName") %>' MaxLength = "100">
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
