<%@ Page Language="VB" AutoEventWireup="false" CodeFile="MsDepartment.aspx.vb" Inherits="Master_MsDepartment_MsDepartment" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Common User File</title>
    <script src="../../Function/OpenDlg.js" type="text/javascript"></script>
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
 <link href="https://fonts.googleapis.com/css2?family=Muli:wght@300;400;600&display=swap" rel="stylesheet">
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div class="Content">
     <div class="H1">Department File</div>
     <hr style="color:Blue" />
      <table>
        <tr>
            <td style="width:100px;text-align:right" >Quick Search :</td>
            <td><asp:TextBox runat="server" ID ="tbFilter" CssClass="TextBox"/> 
                <asp:DropDownList runat="server" ID="ddlField" CssClass="DropDownList" >
                  <asp:ListItem Selected="true" Text="Code" Value="Dept_Code"></asp:ListItem>
                  <asp:ListItem Text="Description" Value="Dept_Name"></asp:ListItem> 
                  <asp:ListItem Text="Group" Value="Group_Name"></asp:ListItem>        
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
                    <asp:ListItem Selected="true" Text="Code" Value="Dept_Code"></asp:ListItem>
                    <asp:ListItem Text="Description" Value="Dept_Name"></asp:ListItem> 
                    <asp:ListItem Text="Group" Value="Group_Name"></asp:ListItem> 
                  </asp:DropDownList>
                
            </td>
        </tr>
     </table>
     </asp:Panel>
      <br />
      <asp:GridView id="DataGrid" runat="server" 
            ShowFooter="True" AllowSorting="True" 
            AutoGenerateColumns="False" AllowPaging="True" CssClass="Grid">
						<HeaderStyle CssClass="GridHeader"></HeaderStyle>
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
							</asp:TemplateField>
							--%>
							<asp:TemplateField HeaderText="Code" HeaderStyle-Width="100" SortExpression="Dept_Code">
								<Itemtemplate>
									<asp:Label Runat="server" ID="DeptCode" text='<%# DataBinder.Eval(Container.DataItem, "Dept_Code") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:Label Runat="server" ID="DeptCodeEdit" Text='<%# DataBinder.Eval(Container.DataItem, "Dept_Code") %>'>
									</asp:Label>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="DeptCodeAdd" CssClass="TextBox" MaxLength="10" Runat="Server" Width="100%"/>
									<cc1:TextBoxWatermarkExtender ID="DeptCodeAdd_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="DeptCodeAdd" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>
								</FooterTemplate>
                                <HeaderStyle Width="100px"></HeaderStyle>
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Description" HeaderStyle-Width="320" SortExpression="Dept_Name">
								<Itemtemplate>
									<asp:Label Runat="server" ID="DeptName" text='<%# DataBinder.Eval(Container.DataItem, "Dept_Name") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:TextBox Runat="server" CssClass="TextBox" ID="DeptNameEdit" MaxLength="60" Width="100%" Text='<%# DataBinder.Eval(Container.DataItem, "Dept_Name") %>'>
									</asp:TextBox>
									<cc1:TextBoxWatermarkExtender ID="DeptNameEdit_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="DeptNameEdit" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="DeptNameAdd" CssClass="TextBox" MaxLength="60" Runat="Server" Width="100%"/>
									<cc1:TextBoxWatermarkExtender ID="DeptNameAdd_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="DeptNameAdd" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>
								</FooterTemplate>
                            <HeaderStyle Width="320px"></HeaderStyle>
							</asp:TemplateField>	
							
							<asp:TemplateField HeaderText="Type" HeaderStyle-Width="50" SortExpression="Group_Code">
								<Itemtemplate>
								    <asp:Label Runat="server" ID="DeptGroupName" text='<%# DataBinder.Eval(Container.DataItem, "Group_Code") %>'>
									</asp:Label>
									
								</Itemtemplate>
								<EditItemTemplate>
									 <asp:DropDownList runat="server" ID="DeptGroupEdit" CssClass="DropDownList" >
                                        <asp:ListItem Selected="true" Text="-" Value=" "></asp:ListItem>
                                        <asp:ListItem Text="Bibit" Value="Bibit"></asp:ListItem> 
                                        <asp:ListItem Text="Land" Value="Land"></asp:ListItem> 
                                        <asp:ListItem Text="TBM" Value="TBM"></asp:ListItem> 
                                    </asp:DropDownList>
								</EditItemTemplate>
								<FooterTemplate>
									 <asp:DropDownList runat="server" ID="DeptGroupAdd" CssClass="DropDownList" >
                                        <asp:ListItem Selected="true" Text="-" Value=" "></asp:ListItem>
                                        <asp:ListItem Text="Bibit" Value="Bibit"></asp:ListItem> 
                                        <asp:ListItem Text="Land" Value="Land"></asp:ListItem> 
                                        <asp:ListItem Text="TBM" Value="TBM"></asp:ListItem>
										<asp:ListItem Text="PKS" Value="PKS"></asp:ListItem> 										
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
       
     <asp:Label ID="lstatus" ForeColor="red" runat="server"></asp:Label>        
    </div>
    </form>
</body>
</html>
