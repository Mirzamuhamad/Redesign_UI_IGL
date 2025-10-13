
<%@ Page Language="VB" AutoEventWireup="false" CodeFile="MsUnit.Aspx.vb" Inherits="Master_MsUnit_MsUnit" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Unit File</title>
    <script src="../../Function/OpenDlg.js" type="text/javascript"></script> 
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
 <link href="https://fonts.googleapis.com/css2?family=Muli:wght@300;400;600&display=swap" rel="stylesheet">
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div class="Content">
     <div class="H1">Unit File</div>
     <hr style="color:Blue" />
     <asp:Panel id="pnlHd" runat="server">
      <table>
        <tr>
            <td style="text-align:right; width:100px" >Quick Search :
            </td>
            <td><asp:TextBox CssClass="TextBox"  runat="server" ID ="tbFilter"/> 
                <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlField" >
                  <asp:ListItem Selected="true" Text="Unit Code" Value="UnitCode"></asp:ListItem>
                  <asp:ListItem Text="Unit Name" Value="UnitName"></asp:ListItem>                  
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
            <td style="width:100px;text-align:right"><asp:DropDownList runat="server" ID="ddlNotasi" >
                    <asp:ListItem Selected="true" Text="OR" Value="OR"></asp:ListItem>
                    <asp:ListItem Text="AND" Value="AND"></asp:ListItem>         
                </asp:DropDownList>
            </td>
            <td><asp:TextBox CssClass="Button" runat="server" ID ="tbfilter2"/> 
                  <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlField2" >
                      <asp:ListItem Selected="true" Text="Unit Code" Value="UnitCode"></asp:ListItem>
                      <asp:ListItem Text="Unit Name" Value="UnitName"></asp:ListItem>                      
                  </asp:DropDownList>
                
                <asp:Label ID="lbUnitCode0" runat="server" CssClass="H1" Font-Bold="true" 
                    ForeColor="Blue" />
                
            </td>
        </tr>
     </table>
     </asp:Panel>
      <br />
      <div style="border:0px  solid; width:100%; height:100%; overflow:auto;">
      <asp:GridView id="DataGrid" runat="server" ShowFooter="True" AllowSorting="True" 
            AutoGenerateColumns="False" AllowPaging="True" CssClass="Grid">
						<HeaderStyle CssClass="GridHeader" wrap="false"></HeaderStyle>
						<RowStyle CssClass="GridItem" Wrap="false"/>
						<AlternatingRowStyle CssClass="GridAltItem"/>
						<FooterStyle CssClass="GridFooter" />
						<PagerStyle CssClass="GridPager" />
				      <Columns>
							<asp:TemplateField HeaderText="Unit Code" HeaderStyle-Width="150" SortExpression="UnitCode">
								<Itemtemplate>
									<asp:Label Runat="server" Width="70" ID="UnitCode" text='<%# DataBinder.Eval(Container.DataItem, "UnitCode") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:Label Runat="server" ID="UnitCodeEdit" Width="100%" Text='<%# DataBinder.Eval(Container.DataItem, "UnitCode") %>'>
									</asp:Label>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="UnitCodeAdd" Placeholder = "can't blank" CssClass="TextBox" Width="100%" MaxLength="5" Runat="Server"/>
								    <%--<cc1:TextBoxWatermarkExtender ID="UnitCodeAdd_TextBoxWatermarkExtender" 
                                        runat="server" Enabled="True" TargetControlID="UnitCodeAdd" WatermarkText="can't blank" WatermarkCssClass="Watermarked" >
                                    </cc1:TextBoxWatermarkExtender>--%>
								</FooterTemplate>
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Unit Name" HeaderStyle-Width="350" SortExpression="UnitName">
								<Itemtemplate>
									<asp:Label Runat="server" Width="280" ID="UnitName" text='<%# DataBinder.Eval(Container.DataItem, "UnitName") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:TextBox Runat="server" ID="UnitNameEdit" CssClass="TextBox" Width="100%" Text='<%# DataBinder.Eval(Container.DataItem, "UnitName") %>'>
									</asp:TextBox>
									<%--<cc1:TextBoxWatermarkExtender ID="UnitNameEdit_TextBoxWatermarkExtender" 
                                        runat="server" Enabled="True" TargetControlID="UnitNameEdit" WatermarkText="can't blank" WatermarkCssClass="Watermarked" >
                                    </cc1:TextBoxWatermarkExtender>--%>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="UnitNameAdd" Placeholder = "can't blank" CssClass="TextBox" Width="100%" Runat="Server"/>
								<%--	<cc1:TextBoxWatermarkExtender ID="UnitNameAdd_TextBoxWatermarkExtender" 
                                        runat="server" Enabled="True" TargetControlID="UnitNameAdd" WatermarkText="can't blank" WatermarkCssClass="Watermarked" >
                                    </cc1:TextBoxWatermarkExtender>--%>
								</FooterTemplate>
							</asp:TemplateField>																												
							
							<asp:TemplateField HeaderText="Action" HeaderStyle-Width="126" ItemStyle-Wrap="false">
								<ItemTemplate>
									<asp:Button ID="btnEdit" runat="server" class="bitbtndt btnedit" Text="Edit" CommandName="Edit"/>
									<asp:Button ID="btndelete" runat="server" class="bitbtndt btndelete" Text="Delete" OnClientClick="return confirm('Sure to delete this data?');" CommandName="Delete"/>									
									<asp:Button ID="btnView" runat="server" class="bitbtndt btndetail" Text="Detail" CommandName="View" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>"/>
									<%--<asp:ImageButton ID="btnView" runat="server" CommandName="View" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>"
                                                        ImageUrl="../../Image/btnDetaildton.png"
                                                        onmouseover="this.src='../../Image/btnDetaildtoff.png';"
                                                        onmouseout="this.src='../../Image/btnDetaildton.png';"
                                                        ImageAlign="AbsBottom" />--%>
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
     </asp:Panel>
     <asp:Panel ID="pnlDt" runat="server" Visible = "false">  
     <asp:Label ID="label1" CssClass="H1" runat="server" Text="Unit : " />   
     <asp:Label ID="lbUnitCode" ForeColor="Blue" CssClass="H1" Font-Bold="true" runat="server" />
	 <br />
	 <br />
	 <%--<asp:Button Text="Back" ID="btnBackDtTop" Runat="server" CssClass="Button"/>--%>
	 <asp:Button class="bitbtn btnback" runat="server" ID="btnBackDtTop" Text="Back" />
	 <br />
	 
	    <div style="border:0px  solid; width:100%; height:100%; overflow:auto;">
        <asp:GridView id="DataGridDt" runat="server" ShowFooter="True" AllowSorting="True" 
            AutoGenerateColumns="False" AllowPaging="True">
						<HeaderStyle CssClass="GridHeader" wrap="false"></HeaderStyle>
						<RowStyle CssClass="GridItem" wrap="false"/>
						<AlternatingRowStyle CssClass="GridAltItem"/>
						<FooterStyle CssClass="GridFooter" Wrap="false" />
						<PagerStyle CssClass="GridPager" />
				      <Columns>
				            <asp:TemplateField HeaderText="Operator" SortExpression="Operator">
								<Itemtemplate>
									<asp:Label Runat="server" ID="Operator" text='<%# DataBinder.Eval(Container.DataItem, "Operator") %>'>
									</asp:Label>
								</Itemtemplate>		
								<EditItemTemplate>
								    <asp:DropDownList CssClass="DropDownList" ID="OperatorEdit" text='<%# DataBinder.Eval(Container.DataItem, "Operator") %>' runat="server">
                                        <asp:ListItem>X</asp:ListItem>
                                        <asp:ListItem>/</asp:ListItem>
                                    </asp:DropDownList>
								</EditItemTemplate>	
								<FooterTemplate>
								    <asp:DropDownList CssClass="DropDownList" ID="OperatorAdd" runat="server">
                                        <asp:ListItem>X</asp:ListItem>
                                        <asp:ListItem Selected="True">/</asp:ListItem>
                                    </asp:DropDownList>
								</FooterTemplate>
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Rate" HeaderStyle-Width="70" SortExpression="Rate">
								<Itemtemplate>
									<asp:Label Runat="server" Width="70" ID="Rate" text='<%# DataBinder.Eval(Container.DataItem, "Rate") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<%--<asp:Label Runat="server" ID="RateEdit" Width="90%" Text='<%# DataBinder.Eval(Container.DataItem, "Rate") %>'>--%>
									<%--</asp:Label>--%>
									<asp:TextBox Runat="server" ID="RateEdit" CssClass="TextBox" Width="95%" Text='<%# DataBinder.Eval(Container.DataItem, "Rate") %>'>
									</asp:TextBox>									
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="RateAdd" CssClass="TextBox" Width="90%" MaxLength="5" Runat="Server"/>
								    <cc1:TextBoxWatermarkExtender ID="RateAdd_TextBoxWatermarkExtender" 
                                        runat="server" Enabled="True" TargetControlID="RateAdd" WatermarkText="can't blank" WatermarkCssClass="Watermarked" >
                                    </cc1:TextBoxWatermarkExtender>
								</FooterTemplate>
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Unit Convert" HeaderStyle-Width="80" SortExpression="UnitConvert">
								<Itemtemplate>
									<asp:Label Runat="server" Width="80" ID="UnitConvert" text='<%# DataBinder.Eval(Container.DataItem, "UnitConvert") %>'>
									</asp:Label>
								</Itemtemplate>			
								<EditItemTemplate>
								    <asp:Label runat="server" ID="UnitConvertEdit" text='<%# DataBinder.Eval(Container.DataItem, "UnitConvert") %>' />
								</EditItemTemplate>	
								<FooterTemplate>								    
								    <asp:DropDownList ID="UnitConvertAdd" CssClass="DropDownList" runat="server" DataSourceID="dsUnitConvert" Width="100%" 
                                        DataTextField="Unit_Name" DataValueField="Unit_Code" AutoPostBack="true"
                                        onselectedindexchanged="UnitConvertAdd_SelectedIndexChanged">
                                    </asp:DropDownList>								    
								</FooterTemplate>											
							</asp:TemplateField>
																																																		
							
							<asp:TemplateField HeaderText="Action" HeaderStyle-Width="126px" ItemStyle-Wrap="false">
								<ItemTemplate>
								    <asp:Button ID="btnEdit" runat="server" class="bitbtndt btnedit" Text="Edit" CommandName="Edit"/>
								    <asp:Button ID="btnDelete" runat="server" class="bitbtndt btndelete" Text="Delete" OnClientClick="return confirm('Sure to delete this data?');" CommandName="Delete"/>									
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
	    <%--<asp:Button Text="Back" ID="Button2" Runat="server" CssClass="Button"/>--%>
	    <asp:Button class="bitbtn btnback" runat="server" ID="Button2" Text="Back" />
     </asp:Panel>   
    </div>        
    <asp:SqlDataSource ID="dsUnitConvert" runat="server" 
      SelectCommand="EXEC S_GetUnit">
    </asp:SqlDataSource>
    <asp:Label ID="lstatus" ForeColor="red" runat="server"></asp:Label>
    </form>
    </body>
</html>
