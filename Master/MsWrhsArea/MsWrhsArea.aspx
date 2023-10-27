<%@ Page Language="VB" AutoEventWireup="false" CodeFile="MsWrhsArea.aspx.vb" Inherits="Master_MsWrhsArea_MsWrhsArea" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Warehouse Area File</title>
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
    <script src="../../Function/OpenDlg.JS" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div class="Content">
     <div Class="H1">Warehouse Area File</div>
     <hr style="color:Blue" />
     <asp:Panel ID="PnlMain" runat="server">
      <table>
        <tr>
            <td style="width:100px;text-align:right" >Quick Search :</td>
            <td><asp:TextBox runat="server" CssClass="TextBox" ID ="tbFilter"/> 
                <asp:DropDownList runat="server" ID="ddlField" CssClass="DropDownList" >
                  <asp:ListItem Selected="true" Text="Warehouse Area Code" Value="WrhsAreaCode"></asp:ListItem>
                  <asp:ListItem Text="Warehouse Area Name" Value="WrhsAreaName"></asp:ListItem>        
                    <asp:ListItem Value="Address1">Address 1</asp:ListItem>
                    <asp:ListItem Value="Address2">Address 2</asp:ListItem>
                    <asp:ListItem Value="Cluster">Cluster</asp:ListItem>
                    <asp:ListItem Value="ZipCode">Zip Code</asp:ListItem>
                    <asp:ListItem Value="Phone">Phone</asp:ListItem>
                    <asp:ListItem Value="Fax">Fax</asp:ListItem>
                    <asp:ListItem Value="ContactPerson">Contact Person</asp:ListItem>
                    <asp:ListItem Value="ContactTitle">Contact Title</asp:ListItem>
                    <asp:ListItem Value="ContactPhone">Contact Phone</asp:ListItem>
                    <asp:ListItem Value="ContactEmail">Contact Email</asp:ListItem>
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
            <td><asp:TextBox CssClass="TextBox" runat="server" ID ="tbfilter2"/> 
                  <asp:DropDownList ID="ddlField2" runat="server" CssClass="DropDownList">
                      <asp:ListItem Selected="true" Text="Warehouse Area Code" Value="WrhsAreaCode"></asp:ListItem>
                      <asp:ListItem Text="Warehouse Area Name" Value="WrhsAreaName"></asp:ListItem>
                      <asp:ListItem Value="Address1">Address 1</asp:ListItem>
                      <asp:ListItem Value="Address2">Address 2</asp:ListItem>
                      <asp:ListItem Value="Cluster">Cluster</asp:ListItem>
                      <asp:ListItem Value="ZipCode">Zip Code</asp:ListItem>
                      <asp:ListItem Value="Phone">Phone</asp:ListItem>
                      <asp:ListItem Value="Fax">Fax</asp:ListItem>
                      <asp:ListItem Value="ContactPerson">Contact Person</asp:ListItem>
                      <asp:ListItem Value="ContactTitle">Contact Title</asp:ListItem>
                      <asp:ListItem Value="ContactPhone">Contact Phone</asp:ListItem>
                      <asp:ListItem Value="ContactEmail">Contact Email</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
     </table>
     </asp:Panel>
      <br />
      
      <asp:Button class="bitbtn btnadd" runat="server" ID="btnAdd" Text="Add"/>	
      
      <div style="border:0px  solid; width:100%; height:100%; overflow:auto;">
      <asp:GridView  id="DataGrid" runat="server" ShowFooter="True" AllowSorting="True" 
            AutoGenerateColumns="False" AllowPaging="True" CssClass="Grid">
						<HeaderStyle CssClass="GridHeader" Wrap ="false"></HeaderStyle>
						<RowStyle CssClass="GridItem" Wrap="True"/>
						<AlternatingRowStyle CssClass="GridAltItem" Wrap="True"/>
						<FooterStyle CssClass="GridFooter" />
						<PagerStyle CssClass="GridPager" />
				      <Columns>
							<asp:TemplateField HeaderText="Warehouse Area Code" HeaderStyle-Width="80" SortExpression="WrhsAreaCode">
								<Itemtemplate>
									<asp:Label width="80" Runat="server" ID="WrhsAreaCode" text='<%# DataBinder.Eval(Container.DataItem, "WrhsAreaCode") %>'>
									</asp:Label>
								</Itemtemplate>
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Warehouse Area Name"  HeaderStyle-Width="320" SortExpression="WrhsAreaName">
								<Itemtemplate>
									<asp:Label width="320" Runat="server" ID="WrhsAreaName"  text='<%# DataBinder.Eval(Container.DataItem, "WrhsAreaName") %>'>
									</asp:Label>
								</Itemtemplate>
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Cluster Code"  HeaderStyle-Width="100" SortExpression="Cluster">
								<Itemtemplate>
									<asp:Label width="100" Runat="server" ID="Cluster"  text='<%# DataBinder.Eval(Container.DataItem, "Cluster") %>'>
									</asp:Label>
								</Itemtemplate>
							</asp:TemplateField>	
							
							<%--<asp:TemplateField HeaderText="Cluster Name"  HeaderStyle-Width="100" SortExpression="ClusterName">
								<Itemtemplate>
									<asp:Label width="100" Runat="server" ID="ClusterName"  text='<%# DataBinder.Eval(Container.DataItem, "ClusterName") %>'>
									</asp:Label>
								</Itemtemplate>
							</asp:TemplateField>--%>	
							
							<asp:TemplateField HeaderText="Address 1" HeaderStyle-Width="250" SortExpression="Address1">
								<Itemtemplate>
									<asp:Label width="250" Runat="server" ID="Address1"  text='<%# DataBinder.Eval(Container.DataItem, "Address1") %>'>
									</asp:Label>
								</Itemtemplate>
							</asp:TemplateField>		
							
							<asp:TemplateField HeaderText="Address 2"  HeaderStyle-Width="250" SortExpression="Address2">
								<Itemtemplate>
									<asp:Label width="250" Runat="server" ID="Address2"  text='<%# DataBinder.Eval(Container.DataItem, "Address2") %>'>
									</asp:Label>
								</Itemtemplate>
							</asp:TemplateField>		
							
							<asp:TemplateField HeaderText="Zip Code" HeaderStyle-Width="80" SortExpression="ZipCode">
								<Itemtemplate>
									<asp:Label width="80" Runat="server" ID="ZipCode" text='<%# DataBinder.Eval(Container.DataItem, "ZipCode") %>'>
									</asp:Label>
								</Itemtemplate>
							</asp:TemplateField>		
							
							<asp:TemplateField HeaderText="Phone" HeaderStyle-Width="160" SortExpression="Phone">
								<Itemtemplate>
									<asp:Label width="160" Runat="server" ID="Phone" text='<%# DataBinder.Eval(Container.DataItem, "Phone") %>'>
									</asp:Label>
								</Itemtemplate>
							</asp:TemplateField>		
							
							<asp:TemplateField HeaderText="Fax" HeaderStyle-Width="160" SortExpression="Fax">
								<Itemtemplate>
									<asp:Label width="160" Runat="server" ID="Fax"  text='<%# DataBinder.Eval(Container.DataItem, "Fax") %>'>
									</asp:Label>
								</Itemtemplate>
							</asp:TemplateField>		
						
						    <asp:TemplateField HeaderText="Contact Person" HeaderStyle-Width="200" SortExpression="ContactPerson">
								<Itemtemplate>
									<asp:Label Width="200" Runat="server" ID="ContactPerson"  text='<%# DataBinder.Eval(Container.DataItem, "ContactPerson") %>'>
									</asp:Label>
								</Itemtemplate>
							</asp:TemplateField>		
							
							<asp:TemplateField HeaderText="Contact Title" HeaderStyle-Width="200" SortExpression="ContactTitle">
								<Itemtemplate>
									<asp:Label Width="200" Runat="server" ID="ContactTitle"  text='<%# DataBinder.Eval(Container.DataItem, "ContactTitle") %>'>
									</asp:Label>
								</Itemtemplate>
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Contact Phone" HeaderStyle-Width="160" SortExpression="ContactPhone">
								<Itemtemplate>
									<asp:Label Width="160" Runat="server" ID="ContactPhone"  text='<%# DataBinder.Eval(Container.DataItem, "ContactPhone") %>'>
									</asp:Label>
								</Itemtemplate>
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Contact Email" HeaderStyle-Width="200" SortExpression="ContactEmail">
								<Itemtemplate>
									<asp:Label  width="200" Runat="server" ID="ContactEmail"  text='<%# DataBinder.Eval(Container.DataItem, "ContactEmail") %>'>
									</asp:Label>
								</Itemtemplate>
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Action" headerstyle-width="126" ItemStyle-Wrap = "False">
								<ItemTemplate>
								    <asp:Button ID="btnEdit" runat="server" class="bitbtndt btnedit" Text="Edit" CommandName="Edit"/>
									<asp:Button ID="btndelete" runat="server" class="bitbtndt btndelete" Text="Delete" OnClientClick="return confirm('Sure to delete this data?');" CommandName="Delete"/>									
								</ItemTemplate>
								<EditItemTemplate>
									<asp:Button ID="btnUpdate" runat="server" class="bitbtndt btnsave" Text="Save" CommandName="Update"/>									
                                    <asp:Button ID="btnCancel" runat="server" class="bitbtndt btncancel" Text="Cancel" CommandName="Cancel"/>									
								</EditItemTemplate>
							</asp:TemplateField>

							
    					</Columns>
        </asp:GridView>
        </div>
        <asp:Button class="bitbtn btnadd" runat="server" ID="btnAdd2" Text="Add" />
        </asp:Panel>
        <asp:Panel ID="pnlInput" runat="server" Visible="false">
        <table>
            <tr>
                <td>Warehouse Area Code</td>
                <td>:</td>
                <td><asp:TextBox ID="tbCode" CssClass="TextBox" MaxLength="3" runat="server" Width="100" /></td>                
            </tr>
            <tr>
                <td>Warehouse Area Name</td>
                <td>:</td>
                <td><asp:TextBox ID="tbName" CssClass="TextBox" MaxLength="60" runat="server" Width="350" /></td>
            </tr>
            
            <tr>
                <td>
                    Cluster</td>
                <td>
                    :</td>
                <td>
                    <asp:TextBox ID="tbCluster" runat="server" AutoPostBack="true" CssClass="TextBox" MaxLength="5" 
                        Width="96px" />
                    <asp:TextBox ID="tbClusterName" ReadOnly="True" Enabled="False" runat="server" 
                        CssClass="TextBox" MaxLength="60" 
                        Width="219px" />
                    <asp:Button Class="btngo" ID="btnCluster" Text="..." runat="server" ValidationGroup="Input" />  
                </td>
            </tr>
            
            <tr>
                <td>Address 1</td>
                <td>:</td>
                <td>
                    <asp:TextBox ID="tbAddress1" CssClass="TextBox" MaxLength="100" runat="server" Width="350" /></td>                
            </tr>            
            <tr>
                <td>Address 2</td>
                <td>:</td>
                <td><asp:TextBox ID="tbAddress2" CssClass="TextBox" MaxLength="100" runat="server" Width="350" /></td>
            </tr>
            <tr>
                <td>Zip Code</td>
                <td>:</td>
                <td><asp:TextBox ID="tbZipCode" CssClass="TextBox" runat="server" MaxLength="10" 
                        Width="68px" /></td>
            </tr>
            <tr>
                <td>Phone</td>
                <td>:</td>
                <td><asp:TextBox ID="tbPhone" CssClass="TextBox" MaxLength="30" runat="server" Width="100" /></td>
            </tr>
            <tr>
                <td>Fax</td>
                <td>:</td>
                <td><asp:TextBox ID="tbFax" CssClass="TextBox" runat="server" MaxLength="30" 
                        Width="100px" /></td>
            </tr>
            <tr>
                <td>Contact Person</td>
                <td>:</td>
                <td><asp:TextBox ID="tbCPerson" CssClass="TextBox" runat="server" MaxLength="60" Width="200" /></td>
            </tr>
            <tr>
                <td>Contact Title</td>
                <td>:</td>
                <td><asp:TextBox ID="tbCTitle" CssClass="TextBox" runat="server" MaxLength="60" Width="200" /></td>
            </tr>
            <tr>
                <td>Contact Phone</td>
                <td>:</td>
                <td><asp:TextBox ID="tbCPhone" CssClass="TextBox" MaxLength="30" runat="server" 
                        Width="100px" /></td>
            </tr>
            <tr>
                <td>Contact Email</td>
                <td>:</td>
                <td><asp:TextBox ID="tbCEmail" CssClass="TextBox" MaxLength="60" runat="server" 
                        Width="197px" /></td>
            </tr>
            <tr>
                <td align="center" colspan="3">
                    <asp:Button ID="btnSave" runat="server" class="bitbtndt btnsave" Text="Save" /> &nbsp;
                    <asp:Button ID="btnCancel" runat="server" class="bitbtndt btncancel" Text="Cancel" CommandName="Cancel"/> &nbsp;
                    <asp:Button ID="btnReset" runat="server" class="bitbtndt btncancel" Text="Reset" CommandName="Reset"/>       
                 </td>
            </tr>
        </table>
     </asp:Panel>   
     
     <asp:Label ID="lstatus" ForeColor="red" runat="server"></asp:Label>        
    </div>
    </form>
</body>
</html>
