<%@ Page Language="VB" AutoEventWireup="false" CodeFile="MsFAGroup.aspx.vb" Inherits="MsFAGroup_MsFAGroup" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>FA Group File</title>
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
                  <asp:ListItem Selected="true" Text="FA Group Code" Value="FAGroupCode"></asp:ListItem>
                  <asp:ListItem Text="FA Group Name" Value="FAGroupName"></asp:ListItem>
                  <asp:ListItem Text="FA Type" Value="FATypeName"></asp:ListItem>  
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
            <td><asp:TextBox runat="server" ID ="tbfilter2" CssClass="TextBox" /> 
                  <asp:DropDownList runat="server" ID="ddlField2" CssClass="DropDownList" >
                    <asp:ListItem Selected="true" Text="FA Group Code" Value="FAGroupCode"></asp:ListItem>
                  <asp:ListItem Text="FA Group Name" Value="FAGroupName"></asp:ListItem>
                  <asp:ListItem Text="FA Type" Value="FATypeName"></asp:ListItem>          
                  </asp:DropDownList>
                
            </td>
        </tr>
     </table>
     </asp:Panel>
      <br />
      
      <asp:GridView id="DataGrid" runat="server" ShowFooter="True" AllowSorting="True" 
            AutoGenerateColumns="False" AllowPaging="True" CssClass="Grid">
						<HeaderStyle CssClass="GridHeader"></HeaderStyle>
						<RowStyle CssClass="GridItem" Wrap="false"/>
						<AlternatingRowStyle CssClass="GridAltItem"/>
						<FooterStyle CssClass="GridFooter" />
						<PagerStyle CssClass="GridPager" />
				      <Columns>
							<asp:TemplateField HeaderText="FA Group Code" HeaderStyle-Width="150" SortExpression="FAGroupCode">
								<Itemtemplate>
									<asp:Label Runat="server" ID="FAGroupCode" text='<%# DataBinder.Eval(Container.DataItem, "FAGroupCode") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:Label Runat="server" ID="FAGroupCodeEdit" Text='<%# DataBinder.Eval(Container.DataItem, "FAGroupCode") %>'>
									</asp:Label>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="FAGroupCodeAdd" placeholder ="can't blank"  CssClass="TextBox" MaxLength="5" Runat="Server" Width="100%"/>
									<%--<cc1:TextBoxWatermarkExtender ID="FAGroupCodeAdd_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="FAGroupCodeAdd" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>--%>
								</FooterTemplate>
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="FA Group Name" HeaderStyle-Width="350" SortExpression="FAGroupName">
								<Itemtemplate>
									<asp:Label Runat="server" ID="FAGroupName" text='<%# DataBinder.Eval(Container.DataItem, "FAGroupName") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:TextBox Runat="server" CssClass="TextBox" ID="FAGroupNameEdit" MaxLength="60" Width="100%" Text='<%# DataBinder.Eval(Container.DataItem, "FAGroupName") %>'>
									</asp:TextBox>
									<cc1:TextBoxWatermarkExtender ID="FAGroupNameEdit_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="FAGroupNameEdit" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="FAGroupNameAdd" placeholder ="can't blank"  CssClass="TextBox" Runat="Server" MaxLength="60" Width="100%"/>
									<%--<cc1:TextBoxWatermarkExtender ID="FAGroupNameAdd_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="FAGroupNameAdd" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>--%>
								</FooterTemplate>
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="FA Type" HeaderStyle-Width="220" SortExpression="FATypeName">
								<Itemtemplate>
									<asp:Label Runat="server" ID="FAType" text='<%# DataBinder.Eval(Container.DataItem, "FATypeName") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:DropDownList Runat="server" ID="FATypeEdit" CssClass="DropDownList" Width="100%" 
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "FAGroupType") %>' 
                                        DataSourceID="dsFAType" DataTextField="FA_Type_Name" 
                                        DataValueField="FA_Type_Code">									    
									</asp:DropDownList>								    
								</EditItemTemplate>
								<FooterTemplate>
									<asp:DropDownList ID="FATypeAdd" Runat="Server" Width="100%" CssClass="DropDownList"									    
                                        DataSourceID="dsFAType" DataTextField="FA_Type_Name" 
                                        DataValueField="FA_Type_Code">
									</asp:DropDownList>
								</FooterTemplate>
							</asp:TemplateField>		
							
							<asp:TemplateField HeaderText="Action" headerstyle-width="126" >
							    <ItemTemplate>
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
        
        
        <asp:SqlDataSource ID="dsFAType" runat="server"            
           SelectCommand="SELECT FA_Type_Code, FA_Type_Name FROM VMsFAType">
        </asp:SqlDataSource>
        
     <asp:Label ID="lstatus" ForeColor="red" runat="server"></asp:Label>
    </div>
    </form>
</body>
</html>
