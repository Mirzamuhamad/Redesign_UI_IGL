<%@ Page Language="VB" AutoEventWireup="false" CodeFile="MsFormula.aspx.vb" Inherits="Master_MsFormula_MsFormula" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Formula File</title>
    <script src="../../Function/Function.JS" type="text/javascript"></script>
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
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div class="Content">
     <div class="H1">Formula File</div>
     <hr style="color:Blue" />
     <asp:Panel id="pnlHd" runat="server">
      <table>
        <tr>
            <td style="text-align:right; width:100px" >Quick Search :
            </td>
            <td><asp:TextBox CssClass="TextBox"  runat="server" ID ="tbFilter"/> 
                <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlField" >
                  <asp:ListItem Selected="true" Text="Formula Code" Value="FormulaCode"></asp:ListItem>
                  <asp:ListItem Text="Formula Name" Value="FormulaName"></asp:ListItem>
                  <asp:ListItem Text="Values" Value="FormulaValues"></asp:ListItem>  
                  <asp:ListItem Text="Formula Desc" Value="FormulaDesc"></asp:ListItem>
                  
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
            <td><asp:TextBox CssClass="Button" runat="server" ID ="tbfilter2"/> 
                  <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlField2" >
                    <asp:ListItem Selected="true" Text="Formula Code" Value="FormulaCode"></asp:ListItem>
                    <asp:ListItem Text="Formula Name" Value="FormulaName"></asp:ListItem>
                    <asp:ListItem Text="Values" Value="FormulaValues"></asp:ListItem>  
                    <asp:ListItem Text="Formula Desc" Value="FormulaDesc"></asp:ListItem>
                  </asp:DropDownList>
                
            </td>
        </tr>
     </table>
     </asp:Panel>
      <br />
      <div style="border:0px  solid; width:100%; height:100%; overflow:auto;">
      <asp:GridView id="DataGrid"  runat="server" ShowFooter="True" AllowSorting="True" 
            AutoGenerateColumns="False" AllowPaging="True" CssClass="Grid">
						<HeaderStyle CssClass="GridHeader" wrap="false"></HeaderStyle>
						<RowStyle CssClass="GridItem" Wrap="False" />
						<AlternatingRowStyle CssClass="GridAltItem"/>
						<FooterStyle CssClass="GridFooter" />
						<PagerStyle CssClass="GridPager" />
				      <Columns>
							<asp:TemplateField HeaderText="Formula Code" HeaderStyle-Width="70" SortExpression="FormulaCode">
								<Itemtemplate>
									<asp:Label Runat="server" Width="70" ID="FormulaCode" text='<%# DataBinder.Eval(Container.DataItem, "FormulaCode") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:Label Runat="server" ID="FormulaCodeEdit" Width="100%" Text='<%# DataBinder.Eval(Container.DataItem, "FormulaCode") %>'>
									</asp:Label>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="FormulaCodeAdd" CssClass="TextBox" Width="100%" MaxLength="5" Runat="Server"/>
								    <cc1:TextBoxWatermarkExtender ID="FormulaCodeAdd_TextBoxWatermarkExtender" 
                                        runat="server" Enabled="True" TargetControlID="FormulaCodeAdd" WatermarkText="can't blank" WatermarkCssClass="Watermarked" >
                                    </cc1:TextBoxWatermarkExtender>
								</FooterTemplate>
							    <HeaderStyle Width="70px" />
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Formula Name" HeaderStyle-Width="250px" SortExpression="FormulaName">
								<Itemtemplate>
									<asp:Label Runat="server" Width="250px" ID="FormulaName" text='<%# DataBinder.Eval(Container.DataItem, "FormulaName") %>' >
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:TextBox Runat="server" ID="FormulaNameEdit" MaxLength="50" CssClass="TextBox" Width="100%" Text='<%# DataBinder.Eval(Container.DataItem, "FormulaName") %>'>
									</asp:TextBox>
									<%--<cc1:TextBoxWatermarkExtender ID="TermNameEdit_TextBoxWatermarkExtender" 
                                        runat="server" Enabled="True" TargetControlID="TermNameEdit" WatermarkText="can't blank" WatermarkCssClass="Watermarked" >
                                    </cc1:TextBoxWatermarkExtender>--%>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="FormulaNameAdd" CssClass="TextBox" Width="100%" MaxLength="50" Runat="Server"/>
									<cc1:TextBoxWatermarkExtender ID="FormulaNameAdd_TextBoxWatermarkExtender" 
                                        runat="server" Enabled="True" TargetControlID="FormulaNameAdd" WatermarkText="can't blank" WatermarkCssClass="Watermarked" >
                                    </cc1:TextBoxWatermarkExtender>
								</FooterTemplate>
							    <HeaderStyle Width="250Px" />
							</asp:TemplateField>	
							
							<asp:TemplateField HeaderText="Values" ItemStyle-HorizontalAlign = "Right" HeaderStyle-Width="110px" SortExpression="FormulaValues">
								<Itemtemplate>
									<asp:Label Runat="server" Width="110px" ID="Values" TEXT='<%# DataBinder.Eval(Container.DataItem, "FormulaValues") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:TextBox Runat="server" ID="ValuesEdit" Maxlength = "13" CssClass="TextBox" Width="100%" TEXT='<%# DataBinder.Eval(Container.DataItem, "FormulaValues") %>'>
									</asp:TextBox>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="ValuesAdd" CssClass="TextBox" Width="100%" MaxLength="13" 
                                        Runat="Server">0</asp:TextBox>
								    <cc1:TextBoxWatermarkExtender ID="ValuesAdd_TextBoxWatermarkExtender" 
                                        runat="server" Enabled="True" TargetControlID="ValuesAdd" WatermarkText="can't blank" WatermarkCssClass="Watermarked" >
                                    </cc1:TextBoxWatermarkExtender>
								</FooterTemplate>
							    <HeaderStyle Width="110px" />
							</asp:TemplateField>			
							
				           <asp:TemplateField HeaderText="Description" HeaderStyle-Width="250" SortExpression="FormulaDesc">
								<Itemtemplate>
									<asp:Label Runat="server" Width="200" ID="FormulaDesc" text='<%# DataBinder.Eval(Container.DataItem, "FormulaDesc") %>' >
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:TextBox Runat="server" ID="FormulaDescEdit" CssClass="TextBox" MaxLength="100" Width="100%" Text='<%# DataBinder.Eval(Container.DataItem, "FormulaDesc") %>'>
									</asp:TextBox>
									
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="FormulaDescAdd" CssClass="TextBox" Width="100%" MaxLength="100" Runat="Server"/>
									<cc1:TextBoxWatermarkExtender ID="FormulaDescAdd_TextBoxWatermarkExtender" 
                                        runat="server" Enabled="True" TargetControlID="FormulaDescAdd" WatermarkText="can't blank" WatermarkCssClass="Watermarked" >
                                    </cc1:TextBoxWatermarkExtender>
								</FooterTemplate>
							    <HeaderStyle Width="250px" />
							</asp:TemplateField>	
							
							<asp:TemplateField HeaderText="Action" headerstyle-width="170">
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
								<HeaderStyle Width="170" />
							</asp:TemplateField>
							
    					</Columns>
        </asp:GridView>
      </div>
     </asp:Panel>   
        
  


    <asp:Label ID="lstatus" ForeColor="red" runat="server"></asp:Label>

    </form>
    </body>
</html>
