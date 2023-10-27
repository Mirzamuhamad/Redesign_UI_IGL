<%@ Page Language="VB" AutoEventWireup="false" CodeFile="MsBedeng.aspx.vb" Inherits="Master_MsBedeng_MsBedeng" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Bedeng File</title>
    <script src="../../Function/Function.JS" type="text/javascript"></script>
    <script src="../../Function/OpenDlg.JS" type="text/javascript"></script> 
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div class="Content">
     <div class="H1">Bedeng File</div>
     <hr style="color:Blue" />
      <table>
        <tr>
            <td style="width:100px;text-align:right" >Quick Search :</td>
            <td><asp:TextBox CssClass="TextBox" runat="server" ID ="tbFilter"/> 
                <asp:DropDownList runat="server" ID="ddlField" CssClass="DropDownList" >
                  <asp:ListItem Selected="true" Text="Bedeng Code" Value="BedengCode"></asp:ListItem>
                  <asp:ListItem Text="Bedeng Name" Value="BedengName"></asp:ListItem>        
                  <asp:ListItem Value="MaxCap">Max Capacity</asp:ListItem>
                  <asp:ListItem Value="PICName">PIC</asp:ListItem>
                </asp:DropDownList>     
                <%--<asp:Button runat="server" CssClass="Button" ID="btnSearch" Text="Search" />--%>
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
                  <asp:ListItem Selected="true" Text="Bedeng Code" Value="BedengCode"></asp:ListItem>
                  <asp:ListItem Text="Bedeng Name" Value="BedengName"></asp:ListItem>        
                  <asp:ListItem Value="MaxCap">Max Capacity</asp:ListItem>
                  <asp:ListItem Value="PICName">PIC</asp:ListItem>     
                  </asp:DropDownList>
                
            </td>
        </tr>
     </table>
     </asp:Panel>
      <br />
      <div style="border:0px  solid; width:100%; height:100%; overflow:auto;">
      <asp:GridView id="DataGrid" runat="server" width = "880"
            ShowFooter="True" AllowSorting="True" 
            AutoGenerateColumns="False" AllowPaging="True" CssClass="Grid">
						<HeaderStyle CssClass="GridHeader"  wrap="false" > </HeaderStyle>
						<RowStyle CssClass="GridItem" Wrap="False"/>
						<AlternatingRowStyle CssClass="GridAltItem"/>
						<FooterStyle CssClass="GridFooter" />
						<PagerStyle CssClass="GridPager"  />
				      <Columns>
							<asp:TemplateField HeaderText="Bedeng Code" HeaderStyle-Width="110" SortExpression="BedengCode">
								<Itemtemplate>
									<asp:Label Runat="server" ID="BedengCode" text='<%# DataBinder.Eval(Container.DataItem, "BedengCode") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:Label Runat="server" ID="BedengCodeEdit" Text='<%# DataBinder.Eval(Container.DataItem, "BedengCode") %>'>
									</asp:Label>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="BedengCodeAdd" MaxLength="5" CssClass="TextBox" Runat="Server" Width="100%"/>
									<cc1:TextBoxWatermarkExtender ID="BedengCodeAdd_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="BedengCodeAdd" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>
								</FooterTemplate>

                            <HeaderStyle Width="110px"></HeaderStyle>
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Bedeng Name" HeaderStyle-Width="300" SortExpression="BedengName">
								<Itemtemplate>
									<asp:Label Runat="server" ID="BedengName" Width="100%" text='<%# DataBinder.Eval(Container.DataItem, "BedengName") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:TextBox Runat="server" CssClass="TextBox" ID="BedengNameEdit" MaxLength="60" Width="100%" Text='<%# DataBinder.Eval(Container.DataItem, "BedengName") %>'>
									</asp:TextBox>
							
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="BedengNameAdd" CssClass="TextBox" MaxLength="60" Runat="Server" Width="100%"/>
									<cc1:TextBoxWatermarkExtender ID="BedengNameAdd_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="BedengNameAdd" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>
								</FooterTemplate>

                           <HeaderStyle Width="200px"></HeaderStyle>
							</asp:TemplateField>		
							
							<asp:TemplateField HeaderText="PIC" HeaderStyle-Width="128" SortExpression="PIC">
								<Itemtemplate>
									<asp:Label Runat="server" Width="128" ID="PIC" TEXT='<%# DataBinder.Eval(Container.DataItem, "PIC") %>'>
									</asp:Label>
								</Itemtemplate>	
								<EditItemTemplate>
								    <asp:TextBox CssClass="TextBox" runat="server" Width="90px" ID="PICEdit" 
                                        TEXT='<%# DataBinder.Eval(Container.DataItem, "PIC") %>' 
                                        AutoPostBack="true" MaxLength="12"  ontextchanged="tbPIC_TextChanged"/>
                                    <asp:Button class="btngo" runat="server" ID="btnPICEdit" Text="..." CommandName="SearchPICEdit"/>
								</EditItemTemplate>							
								<FooterTemplate>
								    <asp:TextBox CssClass="TextBox" OnTextChanged="tbPIC_TextChanged" runat="server" id="PICAdd" Width="90" 
                                        AutoPostBack="true" />
                                     <asp:Button class="btngo" runat="server" ID="btnPICAdd" Text="..." CommandName="SearchPICAdd"/>                                    
                                     <cc1:TextBoxWatermarkExtender ID="PICAdd_TextBoxWatermarkExtender" 
                                        runat="server" Enabled="True" TargetControlID="PICAdd" 
                                        WatermarkText="[PIC]" WatermarkCssClass="Watermarked">
                                    </cc1:TextBoxWatermarkExtender>
								</FooterTemplate>
							    <HeaderStyle Width="128px" />
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="PIC Name" HeaderStyle-Width="280" SortExpression="PICName">
								<Itemtemplate>
									<asp:Label Runat="server" Width="280" ID="PICtName" TEXT='<%# DataBinder.Eval(Container.DataItem, "PICName") %>'>
									</asp:Label>
								</Itemtemplate>	
								<EditItemTemplate>
								    <asp:Label Runat="server" ID="PICNameEdit" Width="280" TEXT='<%# DataBinder.Eval(Container.DataItem, "PICName") %>'>
									</asp:Label>
								</EditItemTemplate>	
								<FooterTemplate>
                                    <asp:Label ID="PICNameAdd" Runat="server" 
                                        TEXT='<%# DataBinder.Eval(Container.DataItem, "PICName") %>' Width="280px"></asp:Label>
                                </FooterTemplate>
							    <HeaderStyle Width="280px" />
							</asp:TemplateField>	
							
								<asp:TemplateField HeaderText="Max Capacity" HeaderStyle-Width="300" SortExpression="MaxCap">
								<Itemtemplate>
									<asp:Label Runat="server" ID="MaxCap" Width="100%" text='<%# DataBinder.Eval(Container.DataItem, "MaxCap") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:TextBox Runat="server" CssClass="TextBox" ID="MaxCapEdit" MaxLength="60" Width="100%" Text='<%# DataBinder.Eval(Container.DataItem, "MaxCap") %>'>
									</asp:TextBox>
									</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="MaxCapAdd" CssClass="TextBox" MaxLength="60" Runat="Server" Width="100%"/>
									<cc1:TextBoxWatermarkExtender ID="MaxCapAdd_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="MaxCapAdd" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>
								</FooterTemplate>

                           <HeaderStyle Width="200px"></HeaderStyle>
							</asp:TemplateField>		
							
							
							<asp:TemplateField HeaderText="Action" headerstyle-width="600px" >
								<ItemTemplate>
								
								    <asp:Button ID="btnEdit" runat="server" class="bitbtndt btnedit" Text="Edit" CommandName="Edit"/>
									<asp:Button ID="btnDelete" runat="server" class="bitbtndt btndelete" Text="Delete" OnClientClick="return confirm('Sure to delete this data?');" CommandName="Delete"/>									

								</ItemTemplate>
								<EditItemTemplate>
									<%--<asp:Button CssClass="Button" CommandName="Update" Text="Update" ID="btnUpdate" Runat="server" Width="60" />--%>
									<asp:Button ID="btnUpdate" runat="server" class="bitbtndt btnsave" Text="Save" CommandName="Update"/>									
                                    <asp:Button ID="btnCancel" runat="server" class="bitbtndt btncancel" Text="Cancel" CommandName="Cancel"/>									
									
									                
								</EditItemTemplate>
								<FooterTemplate>
								    <asp:Button class="bitbtndt btnadd" runat="server" ID="btnAdd" Text="Add" CommandName="Insert"/>									
									<%--<asp:Button CssClass="Button" CommandName="Insert" Text="Add" ID="btnAdd" Runat="server" Width="95" />--%>
									                
								</FooterTemplate>

<HeaderStyle Width="600px"></HeaderStyle>
							</asp:TemplateField>
							
    					</Columns>
        </asp:GridView>
        </div>
        <asp:SqlDataSource ID="dsAccount" runat="server"                                                                                 
           SelectCommand="SELECT AccountCode, AccountName FROM VMsAccount">                                        
        </asp:SqlDataSource>        
     <asp:Label ID="lstatus" ForeColor="red" runat="server"></asp:Label>        
    </div>
    </form>
</body>
</html>
