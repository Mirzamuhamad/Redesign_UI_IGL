<%@ Page Language="VB" AutoEventWireup="false" CodeFile="MsCostAlokasi.aspx.vb" Inherits="Master_CostAlokasi" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Cost Alokasi</title>
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
     <div class="H1">Cost Alokasi</div>
     <hr style="color:Blue" />        
                <table>
                     <tr>
                         <td style="width: 100px; text-align: right">
                             Quick Search :
                         </td>
                         <td>
                             <asp:TextBox runat="server" CssClass="TextBox" ID="tbFilter" />
                             <asp:DropDownList runat="server" ID="ddlField" CssClass="DropDownList">
                                 <asp:ListItem Selected="true" Text="Alokasi Code" Value="CostAlokasiCode"></asp:ListItem>
                                 <asp:ListItem Text="AlokasiName" Value="CostAlokasiName"></asp:ListItem>
                                 <asp:ListItem Text="Account" Value="Account"></asp:ListItem>
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
                                    <asp:ListItem Selected="true" Text="Alokasi Code" Value="CostAlokasiCode"></asp:ListItem>
                                 <asp:ListItem Text="AlokasiName" Value="CostAlokasiName"></asp:ListItem>
                                 <asp:ListItem Text="Account" Value="Account"></asp:ListItem>
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
                             <asp:TemplateField HeaderText="Cost Alokasi Code" SortExpression="CostAlokasiCode">
                                 <ItemTemplate>
                                     <asp:Label runat="server" ID="CostAlokasiCode" Text='<%# DataBinder.Eval(Container.DataItem, "CostAlokasiCode") %>'>
                                     </asp:Label>
                                 </ItemTemplate>
                                 <EditItemTemplate>
                                     <asp:Label runat="server" ID="CostAlokasiCodeEdit" Text='<%# DataBinder.Eval(Container.DataItem, "CostAlokasiCode") %>'
                                         CssClass="TextBox" Width="100%" />
                                 </EditItemTemplate>
                                 <FooterTemplate>
                                     <asp:TextBox runat="server" placeholder="can't blank" ID="CostAlokasiCodeAdd" MaxLength="8" CssClass="TextBox"
                                         Width="90%" />
                                     <%--<cc1:TextBoxWatermarkExtender ID="CostAlokasiCodeAdd_WtExt" runat="server" Enabled="True"
                                         TargetControlID="CostAlokasiCodeAdd" WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>--%>
                                 </FooterTemplate>
                             </asp:TemplateField>
                             <asp:TemplateField HeaderText="Cost Alokasi Name" HeaderStyle-Width="368" SortExpression="CostAlokasiName">
                                 <ItemTemplate>
                                     <asp:Label runat="server" ID="CostAlokasiName" Text='<%# DataBinder.Eval(Container.DataItem, "CostAlokasiName") %>'>
                                     </asp:Label>
                                 </ItemTemplate>
                                 <EditItemTemplate>
                                     <asp:TextBox runat="server" ID="CostAlokasiNameEdit" MaxLength="60" Text='<%# DataBinder.Eval(Container.DataItem, "CostAlokasiName") %>'
                                         CssClass="TextBox" Width="100%" />
                                     <cc1:TextBoxWatermarkExtender ID="CostAlokasiNameEdit_WtExt" runat="server" Enabled="True"
                                         TargetControlID="CostAlokasiNameEdit" WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>
                                 </EditItemTemplate>
                                 <FooterTemplate>
                                     <asp:TextBox runat="server" ID="CostAlokasiNameAdd" placeholder="can't blank" MaxLength="60" CssClass="TextBox"
                                         Width="90%" />
                                     <%--<cc1:TextBoxWatermarkExtender ID="CostAlokasiNameAdd_WtExt" runat="server" Enabled="True"
                                         TargetControlID="CostAlokasiNameAdd" WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>--%>
                                 </FooterTemplate>
                             </asp:TemplateField>
                             <asp:TemplateField HeaderText="Account" HeaderStyle-Width="200" SortExpression="Account">
                                 <ItemTemplate>
                                     <asp:Label runat="server" ID="Account" Text='<%# DataBinder.Eval(Container.DataItem, "Account") %>'>
                                     </asp:Label>
                                 </ItemTemplate>
                                 <EditItemTemplate>
                                     <asp:TextBox runat="server" ID="AccountEdit" MaxLength="6" Width="100%" CssClass="TextBox"
                                         AutoPostBack="true" OnTextChanged="tbAccount_TextChanged" Text='<%# DataBinder.Eval(Container.DataItem, "Account") %>' />
                                     <cc1:TextBoxWatermarkExtender ID="AccountEdit_WtExt" runat="server" Enabled="True"
                                         TargetControlID="AccountEdit" WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>
                                     <asp:Button ID="btnAccountEdit" runat="server" CommandName="SearchEdit" class="btngo" Text="..."/>
                                     <%--<asp:ImageButton ID="btnAccountEdit" CommandName="SearchEdit" runat="server"
                                         ImageUrl="../../Image/btndotdton.png" onmouseover="this.src='../../Image/btndotdtoff.png';"
                                         onmouseout="this.src='../../Image/btndotdton.png';" ImageAlign="AbsBottom" />--%>
                                 </EditItemTemplate>
                                 <FooterTemplate>
                                     <asp:TextBox ID="AccountAdd" placeholder="can't blank" CssClass="TextBox" MaxLength="12" Width="70%" runat="Server"
                                         AutoPostBack="true" OnTextChanged="tbAccount_TextChanged" />

                                     <asp:Button ID="btnAccountEdit" runat="server" CommandName="SearchAdd" class="btngo" Text="..."/>
                                     <%--<asp:ImageButton ID="btnAccountAdd" CommandName="SearchAdd" runat="server" ImageUrl="../../Image/btndotdton.png"
                                         onmouseover="this.src='../../Image/btndotdtoff.png';" onmouseout="this.src='../../Image/btndotdton.png';"
                                         ImageAlign="AbsBottom" />
                                          <cc1:TextBoxWatermarkExtender ID="AccountAdd_WtExt" runat="server" Enabled="True"
                                         TargetControlID="AccountAdd" WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender> --%>
                                     
                                 </FooterTemplate>
                             </asp:TemplateField>
                             <asp:TemplateField HeaderText="Account Name" HeaderStyle-Width="368" SortExpression="AccountName">
                                 <ItemTemplate>
                                     <asp:Label runat="server" ID="AccountName" Text='<%# DataBinder.Eval(Container.DataItem, "AccountName") %>'>
                                     </asp:Label>
                                 </ItemTemplate>
                                 <EditItemTemplate>
                                     <asp:Label runat="server" ID="AccountNameEdit" Text='<%# DataBinder.Eval(Container.DataItem, "AccountName") %>'>
                                     </asp:Label>
                                 </EditItemTemplate>
                                 <FooterTemplate>
                                     <asp:Label style = "color: black;" runat="server" ID="AccountNameAdd">
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
