<%@ Page Language="VB" AutoEventWireup="false" CodeFile="MsSampling.aspx.vb" Inherits="Execute_Master_MsSampling_MsSampling" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Sampling Size File</title>
    <script src="../../Function/Function.JS" type="text/javascript"></script> 
    <script src="../../Function/OpenDlg.JS" type="text/javascript"></script>     
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
 <link href="https://fonts.googleapis.com/css2?family=Muli:wght@300;400;600&display=swap" rel="stylesheet">
</head>

<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true">
    </asp:ScriptManager>
    <div class="Content">
     <div class="H1">Sampling Size File</div>
     <hr style="color:Blue" />
      <table>
        <tr>
            <td style="width:100px;text-align:right" >Quick Search :</td>
            <td><asp:TextBox CssClass="Button" runat="server" ID ="tbFilter"/> 
                <asp:DropDownList runat="server" ID="ddlField" CssClass="DropDownList" >
                  <asp:ListItem Selected="true" Text="Sampling Code" Value="SamplingCode"></asp:ListItem>                  
                    <asp:ListItem Value="SamplingType">Sampling Type</asp:ListItem>                    
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
            <td><asp:TextBox CssClass="TextBox" runat="server" ID ="tbfilter2"/> 
                  <asp:DropDownList runat="server" ID="ddlField2" CssClass="DropDownList" >
                    <asp:ListItem Selected="true" Text="Sampling Code" Value="SamplingCode"></asp:ListItem>                  
                    <asp:ListItem Value="SamplingType">Sampling Type</asp:ListItem>                    
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
						<PagerStyle CssClass="GridPager" HorizontalAlign="Center" />
				      <Columns>
							<asp:TemplateField HeaderText="Sampling Code" HeaderStyle-Width="60px" SortExpression="SamplingCode">
								<Itemtemplate>
									<asp:Label Width="100%" Runat="server" ID="SamplingCode" text='<%# DataBinder.Eval(Container.DataItem, "SamplingCode") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:Label Width="100%"  Runat="server" ID="SamplingCodeEdit" Text='<%# DataBinder.Eval(Container.DataItem, "SamplingCode") %>'>
									</asp:Label>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="SamplingCodeAdd" CssClass="TextBox" MaxLength="10" Runat="Server" Width="100%"/>
									<cc1:TextBoxWatermarkExtender ID="SamplingCodeAdd_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="SamplingCodeAdd" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>
								</FooterTemplate>
							</asp:TemplateField>							
							
							<asp:TemplateField HeaderText="Sampling Type" HeaderStyle-Width="80" SortExpression="SamplingType">
								<Itemtemplate>
									<asp:Label Runat="server" ID="SamplingType" text='<%# DataBinder.Eval(Container.DataItem, "SamplingType") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:DropDownList CssClass="DropDownList" Runat="server" ID="SamplingTypeEdit" 
                                        Width="100%" text='<%# DataBinder.Eval(Container.DataItem, "SamplingType") %>' 
                                        AutoPostBack="True" onselectedindexchanged="SamplingTypeEdit_SelectedIndexChanged">
									    <asp:ListItem >Range</asp:ListItem>
									    <asp:ListItem>Percentage</asp:ListItem>  
									</asp:DropDownList>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:DropDownList CssClass="DropDownList" ID="SamplingTypeAdd" Runat="Server" 
                                        Width="100%" AutoPostBack="True" 
                                        onselectedindexchanged="SamplingTypeAdd_SelectedIndexChanged" >
									    <asp:ListItem Selected="True">Range</asp:ListItem>
									    <asp:ListItem>Percentage</asp:ListItem>
									</asp:DropDownList>
								</FooterTemplate>
							</asp:TemplateField>							
							<asp:TemplateField HeaderText="Range Start" HeaderStyle-Width="50" SortExpression="RangeStart">
								<Itemtemplate>
									<asp:Label Runat="server" ID="RangeStart" text='<%# DataBinder.Eval(Container.DataItem, "RangeStart") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:TextBox Runat="server" CssClass="TextBox" ID="RangeStartEdit" MaxLength="30" Width="100%" Text='<%# DataBinder.Eval(Container.DataItem, "RangeStart") %>'>
									</asp:TextBox>									
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="RangeStartAdd" CssClass="TextBox" MaxLength="30" Runat="Server" Width="100%" Text="0"/>									
								</FooterTemplate>
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Range End" HeaderStyle-Width="50" SortExpression="RangeEnd">
								<Itemtemplate>
									<asp:Label Runat="server" ID="RangeEnd" text='<%# DataBinder.Eval(Container.DataItem, "RangeEnd") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:TextBox Runat="server" CssClass="TextBox" ID="RangeEndEdit" MaxLength="30" Width="100%" Text='<%# DataBinder.Eval(Container.DataItem, "RangeEnd") %>'>
									</asp:TextBox>									
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="RangeEndAdd" CssClass="TextBox" MaxLength="30" Runat="Server" Width="100%" Text="0"/>									
								</FooterTemplate>
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Qty SS" HeaderStyle-Width="50" SortExpression="Qty">
								<Itemtemplate>
									<asp:Label Runat="server" ID="Qty" text='<%# DataBinder.Eval(Container.DataItem, "Qty") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:TextBox Runat="server" CssClass="TextBox" ID="QtyEdit" MaxLength="30" Width="100%" Text='<%# DataBinder.Eval(Container.DataItem, "Qty") %>'>
									</asp:TextBox>									
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="QtyAdd" CssClass="TextBox" MaxLength="30" Runat="Server" Width="100%" Text="0"/>									
								</FooterTemplate>
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Percent SS" HeaderStyle-Width="50" SortExpression="Qty">
								<Itemtemplate>
									<asp:Label Runat="server" ID="Percentage" text='<%# DataBinder.Eval(Container.DataItem, "Percentage") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:TextBox Runat="server" CssClass="TextBox" ID="PercentageEdit" MaxLength="30" Width="100%" Text='<%# DataBinder.Eval(Container.DataItem, "Percentage") %>'>
									</asp:TextBox>									
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="PercentageAdd" CssClass="TextBox" MaxLength="30" Runat="Server" Width="100%" Text="0"/>									
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
