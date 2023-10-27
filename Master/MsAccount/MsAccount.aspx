<%@ Page Language="VB" AutoEventWireup="false" CodeFile="MsAccount.aspx.vb" Inherits="Master_MsAccount_MsAccount" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Account File</title>
        <script type="text/javascript">
    function OpenPopup() {         
        window.open("../../SearchDlg.Aspx","List","scrollbars=yes,resizable=no,width=500,height=400");        
        return false;
    }    
   </script>
   <script src="../../Function/OpenDlg.JS" type="text/javascript"></script>
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"  EnablePageMethods="true">
    </asp:ScriptManager>
    <div class="Content">
     <div class="H1">Account File</div>
     <hr style="color:Blue" />
     <asp:Panel ID="PnlMain" runat="server">
      <table>
        <tr>
            <td style="width:100px;text-align:right" >Quick Search :</td>
            <td><asp:TextBox runat="server" ID ="tbFilter" CssClass="TextBox"/> 
                <asp:DropDownList runat="server" ID="ddlField" CssClass="DropDownList" >
                  <asp:ListItem Selected="true" Text="Account Code" Value="Account"></asp:ListItem>
                  <asp:ListItem Text="Account Name" Value="AccountName"></asp:ListItem>        
                  <asp:ListItem Text="Account Class" Value="AccClass"></asp:ListItem>        
                  <asp:ListItem Text="Account Class Name" Value="AccClassName"></asp:ListItem>        
                  <asp:ListItem Text="Detail" Value="Detail"></asp:ListItem>        
                  <asp:ListItem Text="Currency" Value="CurrCode"></asp:ListItem>        
                  <asp:ListItem Text="Subled" Value="FgSubled"></asp:ListItem>        
                  <asp:ListItem Text="Normal" Value="FgNormal"></asp:ListItem>                          
                  <asp:ListItem Text="Active" Value="FgActive"></asp:ListItem>                          
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
            <td style="width:100px;text-align:right"><asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlNotasi" >
                    <asp:ListItem Selected="true" Text="OR" Value="OR"></asp:ListItem>
                    <asp:ListItem Text="AND" Value="AND"></asp:ListItem>         
                </asp:DropDownList>
            </td>
            <td><asp:TextBox CssClass="TextBox" runat="server" ID ="tbfilter2"/> 
                  <asp:DropDownList runat="server" ID="ddlField2" CssClass="DropDownList" >
                    <asp:ListItem Selected="true" Text="Account Code" Value="Account"></asp:ListItem>
                    <asp:ListItem Text="Account Name" Value="AccountName"></asp:ListItem>        
                    <asp:ListItem Text="Account Class" Value="AccClass"></asp:ListItem>        
                    <asp:ListItem Text="Account Class Name" Value="AccClassName"></asp:ListItem>        
                    <asp:ListItem Text="Detail" Value="Detail"></asp:ListItem>        
                    <asp:ListItem Text="Currency" Value="CurrCode"></asp:ListItem>        
                    <asp:ListItem Text="Subled" Value="FgSubled"></asp:ListItem>        
                    <asp:ListItem Text="Normal" Value="FgNormal"></asp:ListItem>                            
                    <asp:ListItem Text="Active" Value="FgActive"></asp:ListItem>      
                  </asp:DropDownList>                
            </td>
        </tr>
     </table>
     </asp:Panel>
      <br />
      <%--<asp:Button Text="Add" ID="btnAdd" Runat="server" CssClass="Button" />--%>
      <asp:Button class="bitbtn btnadd" runat="server" ID="btnAdd" Text="Add" />									
      <div style="border:0px  solid; width:100%; height:100%; overflow:auto;">
      <asp:GridView id="GridView1" runat="server" AllowSorting="True" 
            AutoGenerateColumns="False" AllowPaging="True" CssClass="Grid">
						<HeaderStyle Wrap="false" CssClass="GridHeader"></HeaderStyle>
						<RowStyle CssClass="GridItem" Wrap = "false" />
						<AlternatingRowStyle CssClass="GridAltItem"/>
						<PagerStyle CssClass="GridPager" HorizontalAlign="Left" />
				      <Columns>
				            <asp:TemplateField HeaderStyle-Width="110" HeaderText="Action">
                                  <ItemTemplate>
                                      <asp:DropDownList CssClass="DropDownList" ID="ddl" runat="server">
                                          <asp:ListItem Selected="True" Text="View" />
                                          <asp:ListItem Text="Edit" />
                                          <asp:ListItem Text="Non Active" />
                                          <asp:ListItem Text="Delete" />
                                          <asp:ListItem Text="Trans Type" />
                                      </asp:DropDownList>
                                      <asp:Button class="btngo" runat="server" ID="btnGo" Text="G" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" CommandName="Go" />                                      
                                  </ItemTemplate>
                                  <HeaderStyle Width="110px" />
                            </asp:TemplateField>
				            <asp:BoundField DataField="Account" SortExpression="Account" HeaderText="Account" />
				            <asp:BoundField DataField="AccountName" SortExpression="AccountName" HeaderText="Acount Name" />
				            <asp:BoundField DataField="AccClassName" SortExpression="AccClassName" HeaderText="Account Class Name" />
				            <asp:BoundField DataField="CurrCode" SortExpression="CurrCode" HeaderText="Currency" />
				            <asp:BoundField DataField="FgSubled" SortExpression="FgSubled" HeaderText="Subled" />
				            <asp:BoundField DataField="FgNormal" SortExpression="FgNormal" HeaderText="Normal" />
				            <asp:BoundField DataField="FgActive" SortExpression="FgActive" HeaderText="Active" />							
							<%--<asp:TemplateField HeaderText="Action">
							    <ItemTemplate>
								    <asp:Button ID="btnEdit" runat="server" class="bitbtndt btnedit" Text="Edit" CommandName="Edit" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" />
									<asp:Button ID="btndelete" runat="server" class="bitbtndt btndelete" Text="Delete" OnClientClick="return confirm('Sure to delete this data?');" CommandName="Delete" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" />									
									<asp:Button ID="ImageButton1" runat="server" class="bitbtndt btndetail" Text="Trans Type" CommandName="TransType" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" Width = "80" />																		
								</ItemTemplate>								
							</asp:TemplateField>--%>
    					</Columns>
        </asp:GridView>    
        </div>           
        <asp:Button class="bitbtn btnadd" runat="server" ID="btnAdd2" Text="Add" Visible = "false"/>									
      </asp:Panel>
     <asp:Panel ID="pnlInput" runat="server" Visible="false">
        <table>
            <tr>
                <td>Account Code</td>
                <td>:</td>
                <td><asp:TextBox ID="tbCode" CssClass="TextBox" MaxLength="12" runat="server" />
                </td>
            </tr>
            <tr>
                <td>Account Name</td>                
                <td>:</td>
                <td><asp:TextBox ID="tbName" CssClass="TextBox" MaxLength="60" runat="server" ValidationGroup="Input"
                        Width="309px" /></td>
            </tr>
            <tr>
                <td>Account Class</td>
                <td>:</td>
                <td>
                    <asp:TextBox ID="tbAccClass" CssClass="TextBox" runat="server" MaxLength="8" ValidationGroup="Input"
                        AutoPostBack="True" />
                    <asp:TextBox ID="tbAccClassName" CssClass="TextBoxR" runat="server" 
                        Enabled="False" Width="200px" />                    
                    <asp:Button ID="btnAccClass" runat="server" class="btngo" Text="..." ValidationGroup="Input"/>                    
                </td>
            </tr>
            <tr>
                <td>Detail</td>
                <td>:</td>
                <td>
                    <asp:TextBox ID="tbDetail" CssClass="TextBox" runat="server" Width="60px" AutoPostBack="true" ValidationGroup="Input"
                        MaxLength="4" />
                </td>                
            </tr>            
            <tr>
                <td>Currency</td>
                <td>:</td>
                <td>
                    <asp:DropDownList ID="ddlCurr" runat="server" CssClass="DropDownList" ValidationGroup="Input"/>                 
                </td>
            </tr>
            <tr>
                <td>Subled</td>
                <td>:</td>
                <td>
                    <asp:DropDownList ID="ddlSubled" runat="server" CssClass="DropDownList" ValidationGroup="Input"> 
                        <%--<asp:ListItem Selected="True" Value="C">Customer</asp:ListItem>
                        <asp:ListItem Value="S">Supplier</asp:ListItem>
                        <asp:ListItem Value="F">Fixed Asset</asp:ListItem>
                        <asp:ListItem Value="P">Product</asp:ListItem>
                        <asp:ListItem Value="E">Employee</asp:ListItem>
                        <asp:ListItem Value="V">C I P</asp:ListItem>
                        <asp:ListItem Value="N">Non Subled</asp:ListItem>--%>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>Normal</td>
                <td>:</td>
                <td>
                    <asp:DropDownList ID="ddlNormal" runat="server" CssClass="DropDownList" ValidationGroup="Input">
                        <asp:ListItem Selected="True" Value="D">Debit</asp:ListItem>
                        <asp:ListItem Value="C">Credit</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr> 
            <tr>
                <td>Active</td>
                <td>:</td>
                <td>
                    <asp:DropDownList ID="ddlFgActive" runat="server" CssClass="DropDownList" ValidationGroup="Input">
                        <asp:ListItem Selected="True" Value="Y">Y</asp:ListItem>
                        <asp:ListItem Value="N">N</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>    
            
            <tr>
                <td align="center" colspan="3">
                    <asp:Button class="bitbtn btnsave" runat="server" ID="btnSave" Text="Save" />
                    <asp:Button class="bitbtn btncancel" runat="server" ID="btnCancel" Text="Cancel" />
                    <asp:Button class="bitbtn btndelete" runat="server" ID="btnReset" height="26" Text="Reset" />
                    <asp:Button class="bitbtn btnback" runat="server" ID="btnHome"  Text="Home" />
                    <%--	    				      <asp:ImageButton ID="btnSave" runat="server" 
                                                        ImageUrl="../../Image/btnSaveon.png"
                                                        onmouseover="this.src='../../Image/btnSaveoff.png';"
                                                        onmouseout="this.src='../../Image/btnSaveon.png';"
                                                        ImageAlign="AbsBottom" />                    
                                                  <asp:ImageButton ID="btnCancel" runat="server" 
                                                        ImageUrl="../../Image/btnCancelOn.png"
                                                        onmouseover="this.src='../../Image/btnCancelOff.png';"
                                                        onmouseout="this.src='../../Image/btnCancelOn.png';"
                                                        ImageAlign="AbsBottom" />
                    <asp:ImageButton ID="btnReset" runat="server" 
                                                        ImageUrl="../../Image/btnReseton.png"
                                                        onmouseover="this.src='../../Image/btnResetoff.png';"
                                                        onmouseout="this.src='../../Image/btnReseton.png';"
                                                        ImageAlign="AbsBottom" />--%>
                </td>
            </tr>
            
        </table>
     </asp:Panel>   
     <asp:Label ID="lstatus" ForeColor="red" runat="server"></asp:Label>        
    </div>
    </form>
</body>
</html>
