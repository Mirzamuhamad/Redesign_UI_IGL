<%@ Page Language="VB" AutoEventWireup="false" CodeFile="MsFormDoc.aspx.vb" Inherits="Transaction_MsFormDoc_MsFormDoc" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div class="Content">
     <div class="H1">ISO Form No</div>
     <hr style="color:Blue" />
         <asp:GridView id="DataGrid" runat="server"
            AutoGenerateColumns="False" CssClass="Grid" AllowSorting="True" >
				<HeaderStyle CssClass="GridHeader"></HeaderStyle>
				<RowStyle CssClass="GridItem" Wrap="false"/>
				<AlternatingRowStyle CssClass="GridAltItem"/>
				<FooterStyle CssClass="GridFooter" />
				<PagerStyle CssClass="GridPager" HorizontalAlign="Center" />
		      <Columns>
		      
		        <asp:TemplateField HeaderText="Form Code" SortExpression="FormCode"  >
					<Itemtemplate>
						<asp:Label Runat="server" ID="Form" text='<%# DataBinder.Eval(Container.DataItem, "FormCode") %>'>
						</asp:Label>
					</Itemtemplate>
					<EditItemTemplate>
						<asp:Label Runat="server" ID="FormEdit" Text='<%# DataBinder.Eval(Container.DataItem, "FormCode") %>'>
						</asp:Label>
					</EditItemTemplate>	
				</asp:TemplateField>
				
				<asp:TemplateField HeaderText="Description" SortExpression="FormName" HeaderStyle-Width="250" >
					<Itemtemplate>
						<asp:Label Runat="server" ID="Description" text='<%# DataBinder.Eval(Container.DataItem, "FormName") %>'>
						</asp:Label>
					</Itemtemplate>
					<EditItemTemplate>
						<asp:Label Runat="server" ID="DescriptionEdit" text='<%# DataBinder.Eval(Container.DataItem, "FormName") %>'>
						</asp:Label>
					</EditItemTemplate>	
				</asp:TemplateField>
				
		        <asp:TemplateField HeaderText="ISO DOC. No" SortExpression="ISODocNo" HeaderStyle-Width="150" >
					<Itemtemplate>
						<asp:Label Runat="server" ID="ISODocNo" text='<%# DataBinder.Eval(Container.DataItem, "ISODocNo") %>'>
						</asp:Label>
					</Itemtemplate>
					<EditItemTemplate>
						<asp:TextBox Runat="server" ID="ISODocNoEdit" CssClass="TextBox" Text='<%# DataBinder.Eval(Container.DataItem, "ISODocNo") %>'>
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
