<%@ Page Language="VB" AutoEventWireup="false" CodeFile="MsMethod.aspx.vb" Inherits="Master_MsMethod_MsMethod" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Method File</title>
    <script src="../../Function/Function.JS" type="text/javascript"></script>
    <script src="../../Function/OpenDlg.JS" type="text/javascript"></script> 
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
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div class="Content">
     <div class="H1">Method File</div>
     <hr style="color:Blue" />
     <asp:Panel id="pnlHd" runat="server">
      <table>
        <tr>
            <td style="text-align:right; width:100px" >Quick Search :
            </td>
            <td><asp:TextBox CssClass="TextBox"  runat="server" ID ="tbFilter"/> 
                <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlField" >
                  <asp:ListItem Selected="true" Text="Method Code" Value="MethodCode"></asp:ListItem>
                  <asp:ListItem Text="Method Name" Value="MethodName"></asp:ListItem>
                  <asp:ListItem Text="Method Range" Value="MethodRange"></asp:ListItem>  
                  <asp:ListItem Text="Range Day" Value="Range Day"></asp:ListItem>  
                  <asp:ListItem Text="1 Year =" Value="xPeriod"></asp:ListItem>  
                  
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
                      <asp:ListItem Selected="true" Text="Method Code" Value="MethodCode"></asp:ListItem>
                      <asp:ListItem Text="Method Name" Value="MethodName"></asp:ListItem>
                      <asp:ListItem Text="Method Name" Value="MethodName"></asp:ListItem>
                        <asp:ListItem Text="Method Range" Value="MethodRange"></asp:ListItem>  
                        <asp:ListItem Text="Range Day" Value="Range Day"></asp:ListItem>  
                        <asp:ListItem Text="1 Year =" Value="xPeriod"></asp:ListItem>  
                  
                  </asp:DropDownList>
                
            </td>
        </tr>
     </table>
     </asp:Panel>
      <br />
      <div style="border:0px  solid; width:100%; height:100%; overflow:auto;">
            <asp:GridView  id="DataGrid" runat="server" ShowFooter="True" AllowSorting="True" 
            AutoGenerateColumns="False" AllowPaging="True" CssClass="Grid">
						<HeaderStyle CssClass="GridHeader" Wrap ="false"></HeaderStyle>
						<RowStyle CssClass="GridItem" Wrap="True"/>
						<AlternatingRowStyle CssClass="GridAltItem" Wrap="True"/>
						<FooterStyle CssClass="GridFooter" />
						<PagerStyle CssClass="GridPager" />
				      <Columns>
							<asp:TemplateField HeaderText="Method Code" HeaderStyle-Width="50px" SortExpression="MethodCode">
								<Itemtemplate>
									<asp:Label Runat="server" ID="MethodCode" text='<%# DataBinder.Eval(Container.DataItem, "MethodCode") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:Label Runat="server" ID="MethodCodeEdit" Width="100%" Text='<%# DataBinder.Eval(Container.DataItem, "MethodCode") %>'>
									</asp:Label>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="MethodCodeAdd" CssClass="TextBox" Width="100%" MaxLength="3" Runat="Server"/>
								    <cc1:TextBoxWaTermarkExtender ID="MethodCodeAdd_TextBoxWatermarkExtender" 
                                        runat="server" Enabled="True" TargetControlID="MethodCodeAdd" WatermarkText="can't blank" WatermarkCssClass="Watermarked" >
                                    </cc1:TextBoxWaTermarkExtender>
								</FooterTemplate>
							    <HeaderStyle Width="50px" />
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Method Name" HeaderStyle-Width="300" SortExpression="MethodName" ItemStyle-Wrap = "true">
								<Itemtemplate>
									<asp:Label Runat="server" Width="280" ID="MethodName" text='<%# DataBinder.Eval(Container.DataItem, "MethodName") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:TextBox Runat="server" ID="MethodNameEdit" CssClass="TextBox" Width="100%" MaxLength = "50" Text='<%# DataBinder.Eval(Container.DataItem, "MethodName") %>'>
									</asp:TextBox>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="MethodNameAdd" CssClass="TextBox" Width="100%" MaxLength="50" Runat="Server"/>
									<cc1:TextBoxWatermarkExtender ID="MethodNameAdd_TextBoxWatermarkExtender" 
                                        runat="server" Enabled="True" TargetControlID="MethodNameAdd" WatermarkText="can't blank" WatermarkCssClass="Watermarked" >
                                    </cc1:TextBoxWatermarkExtender>
								</FooterTemplate>
							    <HeaderStyle Width="300px" />
							</asp:TemplateField>	
							
							<asp:TemplateField HeaderText="Method Range" HeaderStyle-Width="100" ItemStyle-HorizontalAlign="Center" SortExpression="MethodRange">
								<Itemtemplate>
									<asp:Label Runat="server" ID="MethodRange" TEXT='<%# DataBinder.Eval(Container.DataItem, "MethodRange") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:DropDownList ID="MethodRangeEdit" CssClass="DropDownList" Width="100%" runat="server" SelectedValue='<%# DataBinder.Eval(Container.DataItem, "MethodRange") %>'>
									  <asp:ListItem>One Month</asp:ListItem>
									  <asp:ListItem>Half Month</asp:ListItem>                                        
									  <asp:ListItem>One Week</asp:ListItem>                               
									  <asp:ListItem>One Day</asp:ListItem>                                                                       
								</asp:DropDownList>								    
								</EditItemTemplate>
								<FooterTemplate>
									<asp:DropDownList ID="MethodRangeAdd" CssClass="DropDownList" Width="100%" runat="server">
									  <asp:ListItem Selected="True">One Month</asp:ListItem>
									  <asp:ListItem>Half Month</asp:ListItem>                                        
									  <asp:ListItem>One Week</asp:ListItem>                               
									  <asp:ListItem>One Day</asp:ListItem>                               
									</asp:DropDownList>								    
								</FooterTemplate>
                                <HeaderStyle Width="100px" />
                                <ItemStyle HorizontalAlign="Center" />
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Working Day" HeaderStyle-Width="80" SortExpression="RangeDay">
								<Itemtemplate>
									<asp:Label Runat="server" ID="RangeDay" TEXT='<%# DataBinder.Eval(Container.DataItem, "RangeDay") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:TextBox Runat="server" ID="RangeDayEdit" CssClass="TextBox" Width="100%" TEXT='<%# DataBinder.Eval(Container.DataItem, "RangeDay") %>'>
									</asp:TextBox>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="RangeDayAdd" CssClass="TextBox" Width="100%" MaxLength="10" 
                                        Runat="Server"></asp:TextBox>
								    <cc1:TextBoxWatermarkExtender ID="RangeDayAdd_TextBoxWatermarkExtender" 
                                        runat="server" Enabled="True" TargetControlID="RangeDayAdd" WatermarkText="can't blank" WatermarkCssClass="Watermarked" >
                                    </cc1:TextBoxWatermarkExtender>
								</FooterTemplate>
							    <HeaderStyle Width="80px" />
							</asp:TemplateField>			
							
							<asp:TemplateField HeaderText="1 Year = " HeaderStyle-Width="70" SortExpression="xPeriod">
								<Itemtemplate>
									<asp:Label Runat="server" Width="70" ID="xPeriod" TEXT='<%# DataBinder.Eval(Container.DataItem, "xPeriod") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:TextBox Runat="server" ID="xPeriodEdit" CssClass="TextBox" Width="100%" TEXT='<%# DataBinder.Eval(Container.DataItem, "xPeriod") %>'>
									</asp:TextBox>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="xPeriodAdd" CssClass="TextBox" Width="100%" MaxLength="10" Runat="Server"/>
								    <cc1:TextBoxWatermarkExtender ID="xPeriodAdd_TextBoxWatermarkExtender" 
                                        runat="server" Enabled="True" TargetControlID="xPeriodAdd" WatermarkText="can't blank" WatermarkCssClass="Watermarked" >
                                    </cc1:TextBoxWatermarkExtender>
								</FooterTemplate>
							    <HeaderStyle Width="70px" />
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Action" headerstyle-width="120" ItemStyle-Wrap = "false">
							    <ItemTemplate>
								    <asp:Button ID="btnEdit" runat="server" class="bitbtndt btnedit" Text="Edit" CommandName="Edit"/>
									<asp:Button ID="btndelete" runat="server" class="bitbtndt btndelete" Text="Delete" OnClientClick="return confirm('Sure to delete this data?');" CommandName="Delete"/>									
									<asp:Button ID="btnAssUser" runat="server" class="bitbtndt btndetail" Text="User" CommandName="AssUser" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" Width = "55" />									
								</ItemTemplate>
								<EditItemTemplate>
									<asp:Button ID="btnUpdate" runat="server" class="bitbtndt btnsave" Text="Save" CommandName="Update"/>									
                                    <asp:Button ID="btnCancel" runat="server" class="bitbtndt btncancel" Text="Cancel" CommandName="Cancel"/>									
								</EditItemTemplate>
								<FooterTemplate>
									<asp:Button class="bitbtndt btnadd" runat="server" ID="btnAdd" Text="Add" CommandName="Insert"/>									
								</FooterTemplate>
							    <HeaderStyle Width="120px" />
                                <ItemStyle Wrap="false" />
							</asp:TemplateField>
							
    					</Columns>
        </asp:GridView>
      </div>
            </asp:Panel>
     </div>   
  


    <asp:Label ID="lstatus" ForeColor="red" runat="server"></asp:Label>

    </form>
    </body>
</html>
