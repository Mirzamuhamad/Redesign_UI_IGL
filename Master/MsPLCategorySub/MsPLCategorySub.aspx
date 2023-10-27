<%@ Page Language="VB" AutoEventWireup="false" CodeFile="MsPLCategorySub.aspx.vb" Inherits="Master_MsPLCategorySub_MsPLCategorySub" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>PL -  Cost Center File</title>
    <script type="text/javascript">
    function OpenPopup() {         
        window.open("../../SearchDlg.Aspx","List","scrollbars=yes,resizable=no,width=500,height=400");        
        return false;
    }    
    </script>
<link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">    
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true" >
    </asp:ScriptManager>    
     <div class="Content">
     <div class="H1">PL - Cost Center File</div>
     <hr style="color:Blue" />        
                <table>
                     <tr>
                         <td style="width: 100px; text-align: right">
                             Quick Search :
                         </td>
                         <td>
                             <asp:TextBox runat="server" CssClass="TextBox" ID="tbFilter" />
                             <asp:DropDownList runat="server" ID="ddlField" CssClass="DropDownList">
                                 <asp:ListItem Selected="true" Text="Code" Value="PLCategorySubCode"></asp:ListItem>
                                 <asp:ListItem Text="Description" Value="PLCategorySubName"></asp:ListItem>
                                 <asp:ListItem Text="Category Code" Value="PLCategoryCode"></asp:ListItem>
                                 <asp:ListItem Text="Category Name" Value="PLCategoryName"></asp:ListItem>
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
                             <td style="width: 100px; text-align: right">
                                 <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlNotasi">
                                     <asp:ListItem Selected="true" Text="OR" Value="OR"></asp:ListItem>
                                     <asp:ListItem Text="AND" Value="AND"></asp:ListItem>
                                 </asp:DropDownList>
                             </td>
                             <td>
                                 <asp:TextBox runat="server" ID="tbfilter2" CssClass="TextBox" />
                                 <asp:DropDownList runat="server" ID="ddlField2" CssClass="DropDownList">
                                     <asp:ListItem Selected="true" Text="Code" Value="PLCategorySubCode"></asp:ListItem>
                                     <asp:ListItem Text="Description" Value="PLCategorySubName"></asp:ListItem>
                                     <asp:ListItem Text="Category Code" Value="PLCategoryCode"></asp:ListItem>
                                     <asp:ListItem Text="Category Name" Value="PLCategoryName"></asp:ListItem>
                                 </asp:DropDownList>
                             </td>
                         </tr>
                     </table>
                 </asp:Panel>
                 <br />
                              
         <%--<asp:UpdatePanel ID="UpdatePanel1" runat="server">
             <ContentTemplate>--%>
                 <div style="border: 0px  solid; width: 100%; height: 100%; overflow: auto;">
                     <asp:GridView ID="DataGrid" runat="server" AllowSorting="True" ShowFooter="true"
                         AutoGenerateColumns="False" AllowPaging="True" CssClass="Grid">
                         <HeaderStyle CssClass="GridHeader" Wrap="false"></HeaderStyle>
                         <FooterStyle Wrap="false" />
                         <RowStyle CssClass="GridItem" Wrap="false" />
                         <AlternatingRowStyle CssClass="GridAltItem" />
                         <FooterStyle CssClass="GridFooter" />
                         <PagerStyle CssClass="GridPager" Wrap="false" HorizontalAlign="Center" />
                         <Columns>
                             <asp:TemplateField HeaderText="Code" SortExpression="PLCategorySubCode">
                                 <ItemTemplate>
                                     <asp:Label runat="server" ID="PLCategorySubCode" Text='<%# DataBinder.Eval(Container.DataItem, "PLCategorySubCode") %>'>
                                     </asp:Label>
                                 </ItemTemplate>
                                 <EditItemTemplate>
                                     <asp:Label runat="server" ID="PLCategorySubCodeEdit" Text='<%# DataBinder.Eval(Container.DataItem, "PLCategorySubCode") %>'
                                         CssClass="TextBox" Width="100%" />
                                 </EditItemTemplate>
                                 <FooterTemplate>
                                     <asp:TextBox runat="server" ID="PLCategorySubCodeAdd" MaxLength="5" CssClass="TextBox"
                                         Width="100%" />
                                     <cc1:TextBoxWatermarkExtender ID="PLCategorySubCodeAdd_WtExt" runat="server" Enabled="True"
                                         TargetControlID="PLCategorySubCodeAdd" WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>
                                 </FooterTemplate>
                             </asp:TemplateField>
                             
                             <asp:TemplateField HeaderText="Description" HeaderStyle-Width="368" SortExpression="PLCategorySubName">
                                 <ItemTemplate>
                                     <asp:Label runat="server" ID="PLCategorySubName" Text='<%# DataBinder.Eval(Container.DataItem, "PLCategorySubName") %>'>
                                     </asp:Label>
                                 </ItemTemplate>
                                 <EditItemTemplate>
                                     <asp:TextBox runat="server" ID="PLCategorySubNameEdit" MaxLength="60" Text='<%# DataBinder.Eval(Container.DataItem, "PLCategorySubName") %>'
                                         CssClass="TextBox" Width="100%" />
                                     <cc1:TextBoxWatermarkExtender ID="PLCategorySubNameEdit_WtExt" runat="server" Enabled="True"
                                         TargetControlID="PLCategorySubNameEdit" WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>
                                 </EditItemTemplate>
                                 <FooterTemplate>
                                     <asp:TextBox runat="server" ID="PLCategorySubNameAdd" MaxLength="60" CssClass="TextBox"
                                         Width="100%" />
                                     <cc1:TextBoxWatermarkExtender ID="PLCategorySubNameAdd_WtExt" runat="server" Enabled="True"
                                         TargetControlID="PLCategorySubNameAdd" WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>
                                 </FooterTemplate>
                             </asp:TemplateField>
                             
                             <asp:TemplateField HeaderText="Category Code" SortExpression="PLCategoryCode">
                                 <ItemTemplate>
                                     <asp:Label runat="server" ID="PLCategoryCode" Text='<%# DataBinder.Eval(Container.DataItem, "PLCategoryCode") %>'>
                                     </asp:Label>
                                 </ItemTemplate>
                                 <EditItemTemplate>
                                     <asp:TextBox runat="server" ID="PLCategoryCodeEdit" MaxLength="5" Width="70%" CssClass="TextBox"
                                         AutoPostBack="true" OnTextChanged="tbPLCategoryCode_TextChanged" Text='<%# DataBinder.Eval(Container.DataItem, "PLCategoryCode") %>' />
                                     <cc1:TextBoxWatermarkExtender ID="PLCategoryCodeEdit_WtExt" runat="server" Enabled="True"
                                         TargetControlID="PLCategoryCodeEdit" WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>
                                     <asp:Button class="btngo" runat="server" ID="btnPLCategoryCodeEdit" Text="..." CommandName="SearchEdit"/>
                                 </EditItemTemplate>
                                 <FooterTemplate>
                                     <asp:TextBox ID="PLCategoryCodeAdd" CssClass="TextBox" MaxLength="5" Width="70%" runat="Server"
                                         AutoPostBack="true" OnTextChanged="tbPLCategoryCode_TextChanged" />
                                     <cc1:TextBoxWatermarkExtender ID="PLCategoryCodeAdd_WtExt" runat="server" Enabled="True"
                                         TargetControlID="PLCategoryCodeAdd" WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>
                                     <asp:Button class="btngo" runat="server" ID="btnPLCategoryCodeAdd" Text="..." CommandName="SearchAdd"/>
                                 </FooterTemplate>
                             </asp:TemplateField>
                             
                             <asp:TemplateField HeaderText="Category Name" HeaderStyle-Width="368" SortExpression="PLCategoryName">
                                 <ItemTemplate>
                                     <asp:Label runat="server" ID="PLCategoryName" Text='<%# DataBinder.Eval(Container.DataItem, "PLCategoryName") %>'>
                                     </asp:Label>
                                 </ItemTemplate>
                                 <EditItemTemplate>
                                     <asp:Label runat="server" ID="PLCategoryNameEdit" Text='<%# DataBinder.Eval(Container.DataItem, "PLCategoryName") %>'>
                                     </asp:Label>
                                 </EditItemTemplate>
                                 <FooterTemplate>
                                     <asp:Label runat="server" ID="PLCategoryNameAdd">
                                     </asp:Label>
                                 </FooterTemplate>
                             </asp:TemplateField>
                             
                             <asp:TemplateField HeaderText="Action" HeaderStyle-Width="126">
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
                 </div>
             <%--</ContentTemplate>
             <Triggers>                  
                 <asp:AsyncPostBackTrigger ControlID="btnSearch"/>
             </Triggers>
         </asp:UpdatePanel>--%>
         <asp:Label ID="lstatus" ForeColor="red" runat="server"></asp:Label>                 
    </div>
    </form>
</body>
</html>
