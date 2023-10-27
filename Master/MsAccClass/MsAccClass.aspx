<%@ Page Language="VB" AutoEventWireup="false" CodeFile="MsAccClass.aspx.vb" Inherits="Master_MsAccClass_MsAccClass" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Account Class File</title>
    <script type="text/javascript">
    function OpenPopup() {         
        window.open("../../SearchDlg.Aspx","List","scrollbars=yes,resizable=no,width=500,height=400");        
        return false;
    }    
    </script>
<link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
 <script src="../../Function/OpenDlg.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">    
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true" >
    </asp:ScriptManager>    
     <div class="Content">
     <div class="H1">Account Class File</div>
     <hr style="color:Blue" />        
                <table>
                     <tr>
                         <td style="width: 100px; text-align: right">
                             Quick Search :
                         </td>
                         <td>
                             <asp:TextBox runat="server" CssClass="TextBox" ID="tbFilter" />
                             <asp:DropDownList runat="server" ID="ddlField" CssClass="DropDownList">
                                 <asp:ListItem Selected="true" Text="Account Class Code" Value="AccClassCode"></asp:ListItem>
                                 <asp:ListItem Text="Account Class Name" Value="AccClassName"></asp:ListItem>
                                 <asp:ListItem Text="Account Sub Group" Value="AccSubGroupName"></asp:ListItem>
                             </asp:DropDownList>
                             <asp:Button class="bitbtn btnsearch" runat="server" ID="btnSearch" Text="Search" />
                             <asp:Button ID="btnExpand" runat="server" class="btngo" Text="..."/>
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
                                     <asp:ListItem Selected="true" Text="Account Class Code" Value="AccClassCode"></asp:ListItem>
                                     <asp:ListItem Text="Account Class Name" Value="AccClassName"></asp:ListItem>
                                     <asp:ListItem Text="Account Sub Group" Value="AccSubGroupName"></asp:ListItem>
                                 </asp:DropDownList>
                             </td>
                         </tr>
                     </table>
                 </asp:Panel>
                 <br />
                              
         
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
                             <asp:TemplateField HeaderText="Account Class Code" SortExpression="AccClassCode">
                                 <ItemTemplate>
                                     <asp:Label runat="server" ID="AccClassCode" Text='<%# DataBinder.Eval(Container.DataItem, "AccClassCode") %>'>
                                     </asp:Label>
                                 </ItemTemplate>
                                 <EditItemTemplate>
                                     <asp:Label runat="server" ID="AccClassCodeEdit" Text='<%# DataBinder.Eval(Container.DataItem, "AccClassCode") %>'
                                         CssClass="TextBox" Width="100%" />
                                 </EditItemTemplate>
                                 <FooterTemplate>
                                     <asp:TextBox runat="server" placeholder="can't blank" ID="AccClassCodeAdd" MaxLength="8" CssClass="TextBox"
                                         Width="90%" />
                                     <%--<cc1:TextBoxWatermarkExtender ID="AccClassCodeAdd_WtExt" runat="server" Enabled="True"
                                         TargetControlID="AccClassCodeAdd" WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>--%>
                                 </FooterTemplate>
                             </asp:TemplateField>
                             <asp:TemplateField HeaderText="Account Class Name" HeaderStyle-Width="368" SortExpression="AccClassName">
                                 <ItemTemplate>
                                     <asp:Label runat="server" ID="AccClassName" Text='<%# DataBinder.Eval(Container.DataItem, "AccClassName") %>'>
                                     </asp:Label>
                                 </ItemTemplate>
                                 <EditItemTemplate>
                                     <asp:TextBox runat="server" ID="AccClassNameEdit" MaxLength="60" Text='<%# DataBinder.Eval(Container.DataItem, "AccClassName") %>'
                                         CssClass="TextBox" Width="100%" />
                                     <cc1:TextBoxWatermarkExtender ID="AccClassNameEdit_WtExt" runat="server" Enabled="True"
                                         TargetControlID="AccClassNameEdit" WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>
                                 </EditItemTemplate>
                                 <FooterTemplate>
                                     <asp:TextBox runat="server" ID="AccClassNameAdd" placeholder="can't blank" MaxLength="60" CssClass="TextBox"
                                         Width="90%" />
                                     <%--<cc1:TextBoxWatermarkExtender ID="AccClassNameAdd_WtExt" runat="server" Enabled="True"
                                         TargetControlID="AccClassNameAdd" WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>--%>
                                 </FooterTemplate>
                             </asp:TemplateField>
                             <asp:TemplateField HeaderText="Account Sub Group" SortExpression="AccSubGroup">
                                 <ItemTemplate>
                                     <asp:Label runat="server" ID="AccSubGroup" Text='<%# DataBinder.Eval(Container.DataItem, "AccSubGroup") %>'>
                                     </asp:Label>
                                 </ItemTemplate>
                                 <EditItemTemplate>
                                     <asp:TextBox runat="server" ID="AccSubGroupEdit" MaxLength="6" Width="70%" CssClass="TextBox"
                                         AutoPostBack="true" OnTextChanged="tbAccSubGroup_TextChanged" Text='<%# DataBinder.Eval(Container.DataItem, "AccSubGroup") %>' />
                                     <cc1:TextBoxWatermarkExtender ID="AccSubGroupEdit_WtExt" runat="server" Enabled="True"
                                         TargetControlID="AccSubGroupEdit" WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>
                                     <asp:Button ID="btnAccSubGroupEdit" runat="server" CommandName="SearchEdit" class="btngo" Text="..."/>
                                     <%--<asp:ImageButton ID="btnAccSubGroupEdit" CommandName="SearchEdit" runat="server"
                                         ImageUrl="../../Image/btndotdton.png" onmouseover="this.src='../../Image/btndotdtoff.png';"
                                         onmouseout="this.src='../../Image/btndotdton.png';" ImageAlign="AbsBottom" />--%>
                                 </EditItemTemplate>
                                 <FooterTemplate>
                                     <asp:TextBox ID="AccSubGroupAdd" placeholder="can't blank" CssClass="TextBox" MaxLength="6" Width="70%" runat="Server"
                                         AutoPostBack="true" OnTextChanged="tbAccSubGroup_TextChanged" />

                                     <asp:Button ID="btnAccSubGroupEdit" runat="server" CommandName="SearchAdd" class="btngo" Text="..."/>
                                     <%--<asp:ImageButton ID="btnAccSubGroupAdd" CommandName="SearchAdd" runat="server" ImageUrl="../../Image/btndotdton.png"
                                         onmouseover="this.src='../../Image/btndotdtoff.png';" onmouseout="this.src='../../Image/btndotdton.png';"
                                         ImageAlign="AbsBottom" />
                                          <cc1:TextBoxWatermarkExtender ID="AccSubGroupAdd_WtExt" runat="server" Enabled="True"
                                         TargetControlID="AccSubGroupAdd" WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender> --%>
                                     
                                 </FooterTemplate>
                             </asp:TemplateField>
                             <asp:TemplateField HeaderText="Account Sub Group Name" HeaderStyle-Width="368" SortExpression="AccSubGroupName">
                                 <ItemTemplate>
                                     <asp:Label runat="server" ID="AccSubGroupName" Text='<%# DataBinder.Eval(Container.DataItem, "AccSubGroupName") %>'>
                                     </asp:Label>
                                 </ItemTemplate>
                                 <EditItemTemplate>
                                     <asp:Label runat="server" ID="AccSubGroupNameEdit" Text='<%# DataBinder.Eval(Container.DataItem, "AccSubGroupName") %>'>
                                     </asp:Label>
                                 </EditItemTemplate>
                                 <FooterTemplate>
                                     <asp:Label style = "color: black;" runat="server" ID="AccSubGroupNameAdd">
                                     </asp:Label>
                                 </FooterTemplate>
                             </asp:TemplateField>                             
                             <asp:TemplateField HeaderText="Action" headerstyle-width="126" >
                                <ItemTemplate>
                                    <asp:Button style = "height: 20px;" ID="btnEdit" runat="server" class="bitbtndt btnedit" Text="Edit" CommandName="Edit"/>
                                    <asp:Button style = "height: 20px;"  ID="btndelete" runat="server" class="bitbtndt btndelete" Text="Delete" OnClientClick="return confirm('Sure to delete this data?');" CommandName="Delete"/>                                 
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Button style = "height: 20px;" ID="btnUpdate" runat="server" class="bitbtndt btnsave" Text="Save" CommandName="Update"/>                                   
                                    <asp:Button style = "height: 20px;" ID="btnCancel" runat="server" class="bitbtndt btncancel" Text="Cancel" CommandName="Cancel"/>                                   
                                </EditItemTemplate>                             
                                <FooterTemplate>
                                    <asp:Button class="bitbtndt btnadd" runat="server" ID="btnAdd" Text="Add" CommandName="Insert"/>                                    
                                </FooterTemplate>

<HeaderStyle Width="126px"></HeaderStyle>
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
