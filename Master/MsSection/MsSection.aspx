<%@ Page Language="VB" AutoEventWireup="false" CodeFile="MsSection.aspx.vb" Inherits="Master_MsSection_MsSection" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>UnTitle</title>
    <script src="../../Function/OpenDlg.js" type="text/javascript"></script>
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
 <link href="https://fonts.googleapis.com/css2?family=Muli:wght@300;400;600&display=swap" rel="stylesheet">
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div class="Content">
     <div class="H1">Section File</div>
     <hr style="color:Blue" />
      <table>
        <tr>
            <td style="width:100px;text-align:right" >Quick Search :</td>
            <td><asp:TextBox runat="server" ID ="tbFilter" CssClass="TextBox"/> 
                <asp:DropDownList runat="server" ID="ddlField" CssClass="DropDownList" >
                  <asp:ListItem Selected="true" Text="Code" Value="DeptCode"></asp:ListItem>
                  <asp:ListItem Text="Description" Value="DeptName"></asp:ListItem> 
                  <asp:ListItem Text="Group" Value="Dept_Name"></asp:ListItem>        
                  <asp:ListItem Text="Driver" Value="FgDriver"></asp:ListItem>                  
                  <asp:ListItem Text="Direct Type" Value="DirectType"></asp:ListItem>
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
            <td><asp:TextBox runat="server" CssClass="TextBox ID ="tbfilter2"/> 
                  <asp:DropDownList runat="server" ID="ddlField2" CssClass="DropDownList" >
                    <asp:ListItem Selected="true" Text="Code" Value="DeptCode"></asp:ListItem>
                    <asp:ListItem Text="Description" Value="DeptName"></asp:ListItem> 
                    <asp:ListItem Text="Group" Value="Dept_Name"></asp:ListItem> 
                    <asp:ListItem Text="Driver" Value="FgDriver"></asp:ListItem>                  
                  <asp:ListItem Text="Direct Type" Value="DirectType"></asp:ListItem>
                  </asp:DropDownList>
                
            </td>
        </tr>
     </table>
     </asp:Panel>
      <br />
      <asp:GridView id="DataGrid" runat="server" 
            ShowFooter="True" AllowSorting="True" 
            AutoGenerateColumns="False" AllowPaging="True" CssClass="Grid">
						<HeaderStyle CssClass="GridHeader" Wrap = "false"></HeaderStyle>
						<RowStyle CssClass="GridItem" Wrap="false"/>
						<AlternatingRowStyle CssClass="GridAltItem"/>
						<FooterStyle CssClass="GridFooter" />
						<PagerStyle CssClass="GridPager" />
				      <Columns>
				            <%--<asp:TemplateField HeaderText="ItemNo" HeaderStyle-Width="100" SortExpression="ItemNo" Visible = "false">
								<Itemtemplate>
									<asp:Label Runat="server" ID="ItemNo" text='<%# DataBinder.Eval(Container.DataItem, "ItemNo") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:Label Runat="server" ID="ItemNoEdit" Text='<%# DataBinder.Eval(Container.DataItem, "ItemNo") %>'>
									</asp:Label>
								</EditItemTemplate>
								<FooterTemplate>
								    <asp:Label Runat="server" ID="ItemNoAdd" Text='<%# DataBinder.Eval(Container.DataItem, "ItemNo") %>'>
									</asp:Label>
								</FooterTemplate>
                                <HeaderStyle Width="100px"></HeaderStyle>
							</asp:TemplateField>--%>
							
							<asp:TemplateField HeaderText="Code" HeaderStyle-Width="100" SortExpression="DeptCode">
								<Itemtemplate>
									<asp:Label Runat="server" ID="SecCode" text='<%# DataBinder.Eval(Container.DataItem, "DeptCode") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:Label Runat="server" ID="SecCodeEdit" Text='<%# DataBinder.Eval(Container.DataItem, "DeptCode") %>'>
									</asp:Label>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="SecCodeAdd" CssClass="TextBox" MaxLength="10" Runat="Server" Width="100%"/>
									<cc1:TextBoxWatermarkExtender ID="SecCodeAdd_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="SecCodeAdd" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>
								</FooterTemplate>
                                <HeaderStyle Width="100px"></HeaderStyle>
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Description" HeaderStyle-Width="300" SortExpression="DeptName">
								<Itemtemplate>
									<asp:Label Runat="server" ID="SecName" text='<%# DataBinder.Eval(Container.DataItem, "DeptName") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:TextBox Runat="server" CssClass="TextBox" ID="secNameEdit" MaxLength="60" Width="100%" Text='<%# DataBinder.Eval(Container.DataItem, "DeptName") %>'>
									</asp:TextBox>
									<cc1:TextBoxWatermarkExtender ID="SecNameEdit_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="SecNameEdit" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="SecNameAdd" CssClass="TextBox" MaxLength="60" Runat="Server" Width="100%"/>
									<cc1:TextBoxWatermarkExtender ID="SecNameAdd_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="SecNameAdd" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>
								</FooterTemplate>
                            <HeaderStyle Width="300px"></HeaderStyle>
							</asp:TemplateField>	
							
							<asp:TemplateField HeaderText="Group" HeaderStyle-Width="220" SortExpression="DeptGroup">
								<Itemtemplate>
								    <asp:Label Runat="server" ID="SecGroupName" text='<%# DataBinder.Eval(Container.DataItem, "Dept_Name") %>'>
									</asp:Label>
									<asp:Label Runat="server" ID="SecGroup" text='<%# DataBinder.Eval(Container.DataItem, "DeptGroup") %>' Visible = "false">
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:DropDownList Runat="server" ID="SecGroupEdit" CssClass="DropDownList" Width="100%" 
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "DeptGroup") %>' 
                                        DataSourceID="dsGetGroup" DataTextField="Dept_Name" 
                                        DataValueField="Dept_Code">									    
									</asp:DropDownList>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:DropDownList ID="SecGroupAdd" Runat="Server" Width="100%" CssClass="DropDownList"									    
                                        DataSourceID="dsGetGroup" DataTextField="Dept_Name" 
                                        DataValueField="Dept_Code">
									</asp:DropDownList>
								</FooterTemplate>
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Driver" HeaderStyle-Width="30" ItemStyle-HorizontalAlign="Center" SortExpression="FgDriver">
								<Itemtemplate>
									<asp:Label Runat="server" Width="30" ID="FgDriver" text='<%# DataBinder.Eval(Container.DataItem, "FgDriver") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:DropDownList ID="FgDriverEdit" CssClass="DropDownList" Width="100%" runat="server" SelectedValue='<%# DataBinder.Eval(Container.DataItem, "FgDriver") %>'>
									  <asp:ListItem>Y</asp:ListItem>
									  <asp:ListItem>N</asp:ListItem>                                        
									</asp:DropDownList>								    
								</EditItemTemplate>
								<FooterTemplate>
									<asp:DropDownList ID="FgDriverAdd" CssClass="DropDownList" Width="100%" runat="server">
									  <asp:ListItem >Y</asp:ListItem>
									  <asp:ListItem Selected="True" >N</asp:ListItem>                                        
									</asp:DropDownList>								    
								</FooterTemplate>
							    <HeaderStyle Width="30px" />
                                <ItemStyle HorizontalAlign="Center" />
							</asp:TemplateField>		
							
							<asp:TemplateField HeaderText="Direct Type" HeaderStyle-Width="100" SortExpression="DirectType">
								<Itemtemplate>
									<asp:Label Runat="server" Width="30" ID="DirectType" text='<%# DataBinder.Eval(Container.DataItem, "DirectType") %>' ItemStyle-HorizontalAlign="Left">
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:DropDownList ID="DirectTypeEdit" CssClass="DropDownList" Width="100%" runat="server" SelectedValue='<%# DataBinder.Eval(Container.DataItem, "DirectType") %>'>
									  <asp:ListItem>Direct</asp:ListItem>
									  <asp:ListItem>Semi Direct</asp:ListItem>                                        
									  <asp:ListItem>InDirect</asp:ListItem>
									</asp:DropDownList>								    
								</EditItemTemplate>
								<FooterTemplate>
									<asp:DropDownList ID="DirectTypeAdd" CssClass="DropDownList" Width="100%" runat="server">
									  <asp:ListItem Selected = "True">Direct</asp:ListItem>
									  <asp:ListItem>Semi Direct</asp:ListItem>                                        
									  <asp:ListItem>InDirect</asp:ListItem>
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

                            <HeaderStyle Width="126px"></HeaderStyle>
							</asp:TemplateField>
							
    					</Columns>
        </asp:GridView> 
        <asp:SqlDataSource ID="dsGetGroup" runat="server" SelectCommand="SELECT Dept_Code, Dept_Name FROM VMsDepartment" ></asp:SqlDataSource>       
     <asp:Label ID="lstatus" ForeColor="red" runat="server"></asp:Label>        
    </div>
    </form>
</body>
</html>
