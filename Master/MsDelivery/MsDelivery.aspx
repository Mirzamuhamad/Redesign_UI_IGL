<%@ Page Language="VB" AutoEventWireup="false" CodeFile="MsDelivery.aspx.vb" Inherits="Master_MsDelivery_MsDelivery" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Delivery File</title>
    <script src="../../Function/OpenDlg.JS" type="text/javascript"></script>     
    <script type="text/javascript">
    function OpenPopup() {         
        window.open("../../SearchDlg.Aspx","List","scrollbars=yes,resizable=no,width=500,height=400");        
        return false;
    }    
   </script>
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
 <link href="https://fonts.googleapis.com/css2?family=Muli:wght@300;400;600&display=swap" rel="stylesheet">
</head>
<body>
    <form id="form1" runat="server">
    <div class="Content">
     <div class="H1"><asp:Label runat="server" ID="lblTitle"></asp:Label>
</div>
     <hr style="color:Blue" />
     <asp:Panel ID="PnlMain" runat="server">
      <table>
        <tr>
            <td style="width:100px;text-align:right" >Quick Search :</td>
            <td><asp:TextBox runat="server" ID ="tbFilter" CssClass="TextBox"/> 
                <asp:DropDownList runat="server" ID="ddlField" CssClass="DropDownList" >
                  <asp:ListItem Selected="true" Text="Delivery Code" Value="DeliveryCode"></asp:ListItem>
                  <asp:ListItem Text="Delivery Name" Value="DeliveryName"></asp:ListItem>        
                  <asp:ListItem Text="Address 1" Value="Address1"></asp:ListItem>        
                  <asp:ListItem Text="Address 2" Value="Address2"></asp:ListItem>        
                  <asp:ListItem Text="City" Value="City"></asp:ListItem>        
                  <asp:ListItem Text="Zip Code" Value="ZipCode"></asp:ListItem>        
                  <asp:ListItem Text="Phone" Value="Phone"></asp:ListItem>        
                  <asp:ListItem Text="Fax" Value="Fax"></asp:ListItem>        
                  <asp:ListItem Text="Contact Name" Value="ContactName"></asp:ListItem>        
                  <asp:ListItem Text="Contact Title" Value="ContactTitle"></asp:ListItem>        
                  <asp:ListItem Text="Contact Hp" Value="ContactHp"></asp:ListItem>        
                  <asp:ListItem Text="Warehouse" Value="WrhsName"></asp:ListItem>        
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
                  <asp:DropDownList runat="server" ID="ddlField2" CssClass="DropDownList" >
                    <asp:ListItem Selected="true" Text="Delivery Code" Value="DeliveryCode"></asp:ListItem>
                    <asp:ListItem Text="Delivery Name" Value="DeliveryName"></asp:ListItem>        
                    <asp:ListItem Text="Address 1" Value="Address1"></asp:ListItem>        
                    <asp:ListItem Text="Address 2" Value="Address2"></asp:ListItem>        
                    <asp:ListItem Text="Zip Code" Value="ZipCode"></asp:ListItem>        
                    <asp:ListItem Text="Phone" Value="Phone"></asp:ListItem>        
                    <asp:ListItem Text="Fax" Value="Fax"></asp:ListItem>        
                    <asp:ListItem Text="Warehouse" Value="WrhsName"></asp:ListItem>        
                    <asp:ListItem Text="Contact Name" Value="ContactName"></asp:ListItem>        
                    <asp:ListItem Text="Contact Title" Value="ContactTitle"></asp:ListItem>        
                    <asp:ListItem Text="Contact Hp" Value="ContactHp"></asp:ListItem>        
                  </asp:DropDownList>                
            </td>
        </tr>
     </table>
     </asp:Panel>
      <br />
      <asp:Button class="bitbtn btnadd" runat="server" ID="btnAdd" Text="Add" Visible = "false"/>									
                                                        
      <%--<div style="border:0px  solid; width:100%; height:100%; overflow:auto;">--%>
<%--      <asp:GridView id="DataGrid" runat="server" AllowSorting="True" 
            AutoGenerateColumns="False" AllowPaging="True" ShowFooter="false" CssClass="Grid">
						<HeaderStyle CssClass="GridHeader"></HeaderStyle>
						<RowStyle CssClass="GridItem" Wrap = "True" />
						<AlternatingRowStyle CssClass="GridAltItem"/>						
						<PagerStyle CssClass="GridPager" />--%>
						
	     <asp:GridView id="DataGrid" runat="server" ShowFooter="False" AllowSorting="True" 
            AutoGenerateColumns="False" AllowPaging="True" CssClass="Grid" Width = "1900" >
						<HeaderStyle CssClass="GridHeader"></HeaderStyle>
						<RowStyle CssClass="GridItem" Wrap="True"/>
						<AlternatingRowStyle CssClass="GridAltItem"/>
						<FooterStyle CssClass="GridFooter" />
						<PagerStyle CssClass="GridPager" />
				      <EmptyDataTemplate>
				    
				      </EmptyDataTemplate>	
				      <Columns>
							<asp:TemplateField HeaderText="Delivery Code" HeaderStyle-Width="100" SortExpression="DeliveryCode">
								<Itemtemplate>
									<asp:Label  Runat="server" ID="DeliveryCode" text='<%# DataBinder.Eval(Container.DataItem, "DeliveryCode") %>'>
									</asp:Label>
								</Itemtemplate>														
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Delivery Name" HeaderStyle-Width="250" SortExpression="DeliveryName">
								<Itemtemplate>
									<asp:Label Runat="server" ID="DeliveryName" text='<%# DataBinder.Eval(Container.DataItem, "DeliveryName") %>'>
									</asp:Label>
								</Itemtemplate>														
							</asp:TemplateField>		
							
							<asp:TemplateField HeaderText="Address 1" HeaderStyle-Width="250" SortExpression="Address1">
								<Itemtemplate>
									<asp:Label  Runat="server" ID="Address1" text='<%# DataBinder.Eval(Container.DataItem, "Address1") %>'>
									</asp:Label>
								</Itemtemplate>														
							</asp:TemplateField>		
                            
                            <asp:TemplateField HeaderText="Address 2" HeaderStyle-Width="250" SortExpression="Address2">
								<Itemtemplate>
									<asp:Label Runat="server" ID="Address2" text='<%# DataBinder.Eval(Container.DataItem, "Address2") %>'>
									</asp:Label>
								</Itemtemplate>														
							</asp:TemplateField>		
                            
                            <asp:TemplateField HeaderText="City" HeaderStyle-Width="250" SortExpression="City">
								<Itemtemplate>
									<asp:Label Runat="server" ID="City" text='<%# DataBinder.Eval(Container.DataItem, "City") %>'>
									</asp:Label>
								</Itemtemplate>														
							</asp:TemplateField>		
    
							<asp:TemplateField HeaderText="Zip Code" HeaderStyle-Width="80" SortExpression="ZipCode">
								<Itemtemplate>
									<asp:Label  Runat="server" ID="ZipCode" text='<%# DataBinder.Eval(Container.DataItem, "ZipCode") %>'>
									</asp:Label>
								</Itemtemplate>														
							</asp:TemplateField>		

							<asp:TemplateField HeaderText="Phone" HeaderStyle-Width="150" SortExpression="Phone">
								<Itemtemplate>
									<asp:Label  Runat="server" ID="Phone" text='<%# DataBinder.Eval(Container.DataItem, "Phone") %>'>
									</asp:Label>
								</Itemtemplate>														
							</asp:TemplateField>		

							<asp:TemplateField HeaderText="Fax" HeaderStyle-Width="150" SortExpression="Fax">
								<Itemtemplate>
									<asp:Label  Runat="server" ID="Fax" text='<%# DataBinder.Eval(Container.DataItem, "Fax") %>'>
									</asp:Label>
								</Itemtemplate>														
							</asp:TemplateField>		

							<asp:TemplateField HeaderText="Warehouse" HeaderStyle-Width="100" SortExpression="WrhsName">
								<Itemtemplate>
									<asp:Label  Runat="server" ID="WrhsName" text='<%# DataBinder.Eval(Container.DataItem, "WrhsName") %>'>
									</asp:Label>
								</Itemtemplate>								
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Warehouse" HeaderStyle-Width="150" SortExpression="WrhsCode" Visible="false">
								<Itemtemplate>
									<asp:Label Runat="server" ID="WrhsCode" text='<%# DataBinder.Eval(Container.DataItem, "WrhsCode") %>'>
									</asp:Label>
								</Itemtemplate>								
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Contact Name" HeaderStyle-Width="150" SortExpression="ContactName">
								<Itemtemplate>
									<asp:Label Runat="server" ID="ContactName" text='<%# DataBinder.Eval(Container.DataItem, "ContactName") %>'>
									</asp:Label>
								</Itemtemplate>														
							</asp:TemplateField>		

                           <asp:TemplateField HeaderText="Contact Title" HeaderStyle-Width="150" SortExpression="ContactTitle">
								<Itemtemplate>
									<asp:Label  Runat="server" ID="ContactTitle" text='<%# DataBinder.Eval(Container.DataItem, "ContactTitle") %>'>
									</asp:Label>
								</Itemtemplate>														
							</asp:TemplateField>		

                            <asp:TemplateField HeaderText="Contact Hp" HeaderStyle-Width="150" SortExpression="ContactHp">
								<Itemtemplate>
									<asp:Label  Runat="server" ID="ContactHp" text='<%# DataBinder.Eval(Container.DataItem, "ContactHp") %>'>
									</asp:Label>
								</Itemtemplate>														
							</asp:TemplateField>	

							<asp:TemplateField HeaderText="Action" headerstyle-width="150" >						    <ItemTemplate>
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
        <%--</div>--%>   
        <asp:Button class="bitbtn btnadd" runat="server" ID="btnAdd2" Text="Add" Visible = "false"/>									
      </asp:Panel>
     <asp:Panel ID="pnlInput" runat="server" Visible="false">
        <table>
            <tr>
                <td>Delivery Code</td>
                <td>:</td>
                <td><asp:TextBox ID="tbCode" CssClass="TextBox" MaxLength="10" runat="server" Width="100" /></td>                
            </tr>
            <tr>
                <td>Delivery Name</td>
                <td>:</td>
                <td><asp:TextBox ID="tbName" CssClass="TextBox" MaxLength="60" runat="server" Width="350" /></td>
            </tr>
            <tr>
                <td>Address 1</td>
                <td>:</td>
                <td>
                    <asp:TextBox ID="tbAddress1" CssClass="TextBox" MaxLength="100" runat="server" Width="350" />
                </td>                
            </tr>            
            <tr>
                <td>Address 2</td>
                <td>:</td>
                <td>
                    <asp:TextBox ID="tbAddress2" CssClass="TextBox" MaxLength="100" runat="server" Width="350" />
                </td>
            </tr>
            <tr>
                <td>City</td>
                <td>
                    &nbsp;</td>
                <td>
                    <asp:TextBox ID="tbCity" CssClass="TextBox" runat="server" MaxLength="30" Width="200" />
                </td>
            </tr>
            <tr>
                <td>
                    Zip Code</td>
                <td>
                    :</td>
                <td>
                    <asp:TextBox ID="tbZipCode" CssClass="TextBox" MaxLength="10" runat="server" Width="100" />
                </td>
            </tr>
            <tr>
                <td>
                    Phone</td>
                <td>
                    :</td>
                <td>
                    <asp:TextBox ID="tbPhone" CssClass="TextBox" runat="server" MaxLength="30" Width="200" />
                </td>
            </tr>
            <tr>
                <td>
                    Fax</td>
                <td>
                    :</td>
                <td>
                    <asp:TextBox ID="tbFax" CssClass="TextBox" runat="server" MaxLength="30" Width="200" />
                </td>
            </tr>
            <tr>
                <td>
                    Warehouse </td>
                <td>
                    :</td>
                <td>
                    <asp:TextBox ID="tbWrhs" CssClass="TextBox" runat="server" AutoPostBack="True" MaxLength="10" Width="100" />
                    <asp:TextBox ID="tbWrhsName" CssClass="TextBox" runat="server" Enabled="false" Width="200" />
                    <%--<asp:Button ID="btnSearchWrhs" CssClass="Button" runat="server" Text="Search" />--%>                   
                    
                    <asp:Button Cssclass="btngo" runat="server" ID="btnSearchWrhs" Text="..." />									                                                                                                                                                                                                                                                                                                                
                                                        
                </td>
            </tr>
            <tr>
               <td>
                  <asp:Label ID="Text" runat="server" Width="100" Text = "Contact Detail" Font-Bold = "true"  Font-Size = "11" /> 
               </td>
               <td>
               </td>
               <td>               
               </td>               
            </tr>
            <tr>
                <td>
                    Contact Name</td>
                <td>
                    :</td>
                <td>
                    <asp:TextBox ID="tbContactName" CssClass="TextBox" MaxLength="40" runat="server" Width="250" />
                </td>
            </tr>
            <tr>
                <td>
                    Contact Title</td>
                <td>
                    :</td>
                <td>
                    <asp:TextBox ID="tbContactTitle" CssClass="TextBox" MaxLength="40" runat="server" Width="250" />
                </td>
            </tr>
            <tr>
                <td>
                    Contact Hp</td>
                <td>
                    :</td>
                <td>
                    <asp:TextBox ID="tbContactHp" CssClass="TextBox" runat="server" MaxLength="30" Width="200" />
                </td>
            </tr>
            
            <tr>
                <td align="center" colspan="3">
                   <asp:Button class="bitbtn btnsave" runat="server" ID="btnSave" Text="Save"  />																						 																											
									
					<asp:Button class="bitbtn btncancel" runat="server" ID="btnCancel" Text="Cancel" />																						 																		
					
					<asp:Button class="bitbtn btndelete" Height = "28px" runat="server" ID="btnReset" Text="Reset" />																						 																										
                   
                </td>
            </tr>
        </table>
     </asp:Panel>   
     <asp:Label ID="lstatus" ForeColor="red" runat="server"></asp:Label>        
    </div>
    </form>
</body>
</html>
