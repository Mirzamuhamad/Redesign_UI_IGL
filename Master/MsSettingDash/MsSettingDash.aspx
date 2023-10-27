<%@ Page Language="VB" AutoEventWireup="false" CodeFile="MsSettingDash.aspx.vb" Inherits="Master_Setting" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Setting Dashboard</title>
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
                  <asp:ListItem Selected="true" Text=" UserName" Value="UserName"></asp:ListItem>
                  <asp:ListItem Text="Menu Name" Value="MenuName"></asp:ListItem>        
                </asp:DropDownList>     
                <asp:Button class="bitbtn btnsearch" runat="server" ID="btnSearch" Text="Search" />
                <asp:Button class="btngo" runat="server" ID="btnExpand" Visible = "false" Text="..."/>
                &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                <asp:Button class="bitbtn btnadd" runat="server" ID="btnPrint" Width = "150" Text="Add Menu"/>
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
            <td><asp:TextBox runat="server" ID ="tbfilter2" CssClass="TextBox" /> 
                  <asp:DropDownList runat="server" ID="ddlField2" CssClass="DropDownList" >
                      <asp:ListItem Selected="true" Text=" UserName" Value="UserName"></asp:ListItem>
                  <asp:ListItem Text="Menu Name" Value="MenuName"></asp:ListItem>       
                  </asp:DropDownList>
                
            </td>
        </tr>
     </table>
     </asp:Panel>
      <br />
      
      <br />
      <asp:GridView id="DataGrid" runat="server" ShowFooter="True" AllowSorting="True" 
            AutoGenerateColumns="False" AllowPaging="True" CssClass="Grid">
						<HeaderStyle CssClass="GridHeader"></HeaderStyle>
						<RowStyle CssClass="GridItem" Wrap="false"/>
						<AlternatingRowStyle CssClass="GridAltItem"/>
						<FooterStyle CssClass="GridFooter" />
						<PagerStyle CssClass="GridPager" />
				      <Columns>
				      <asp:TemplateField HeaderText="User ID" HeaderStyle-Width="150" SortExpression="UserID" Visible = "false">
								<Itemtemplate>  
									<asp:Label Runat="server" ID="USerID" text='<%# DataBinder.Eval(Container.DataItem, " UserID") %>'>
									</asp:Label>
								</Itemtemplate>
								
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Menu ID" HeaderStyle-Width="150" SortExpression="MenuID" Visible = "false">
								<Itemtemplate>
									<asp:Label Runat="server" ID="MenuId" text='<%# DataBinder.Eval(Container.DataItem, " MenuID") %>'>
									</asp:Label>
								</Itemtemplate>
								
							</asp:TemplateField>
							<asp:TemplateField HeaderText="User Name" HeaderStyle-Width="150" SortExpression="UserName">
								<Itemtemplate>
									<asp:Label Runat="server" ID="UserName" text='<%# DataBinder.Eval(Container.DataItem, " UserName") %>'>
									</asp:Label>
								</Itemtemplate>
								
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Menu Name" HeaderStyle-Width="350" SortExpression="MenuName">
								<Itemtemplate>
									<asp:Label Runat="server" ID="MenuName" text='<%# DataBinder.Eval(Container.DataItem, "MenuName") %>'>
									</asp:Label>
								</Itemtemplate>
								
							</asp:TemplateField>
							
							
							
							
							
							<asp:TemplateField HeaderText="Action" headerstyle-width="126" >
							    <ItemTemplate>
								
									<asp:Button ID="btndelete" runat="server" class="bitbtndt btndelete" Text="Drop Menu"  Width = "125" CommandName="Delete"/>									
								</ItemTemplate>
								
							</asp:TemplateField>
							
    					</Columns>
        </asp:GridView>
        
     <asp:Label ID="lstatus" ForeColor="red" runat="server"></asp:Label>
    </div>
    </form>
</body>
</html>
