<%@ Page Language="VB" AutoEventWireup="false" CodeFile="MsPattern.aspx.vb" Inherits="Master_MsPattern_MsPattern" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Pattern File</title>
    <script src="../../Function/OpenDlg.JS" type="text/javascript"></script>            
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">         
    
    function PressShift()
    {
        var _result = false;

        if(event.keyCode==8 || event.keyCode == 88 || ( event.keyCode >= 65 && event.keyCode <= 70 ) || ( event.keyCode >= 97 && event.keyCode <= 102 ))
        {
            _result = true;
        }
        else
        {
            _result = false;
        }
        
        return _result;
    }
</script>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div class="Content">
     <div class="H1">Pattern File</div>
     <hr style="color:Blue" />
           <table>
        <tr>
            <td style="width:100px;text-align:right" >Quick Search :</td>
            <td><asp:TextBox runat="server" ID ="tbFilter" CssClass="TextBox" /> 
                <asp:DropDownList runat="server" ID="ddlField" CssClass="DropDownList" >
                  <asp:ListItem Selected="true" Text="Pattern Code" Value="PatternCode"></asp:ListItem>
                  <asp:ListItem Text="Pattern Name" Value="PatternName"></asp:ListItem>        
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
            <td><asp:TextBox runat="server" ID ="tbfilter2" CssClass="TextBox"/> 
                  <asp:DropDownList runat="server" ID="ddlField2" CssClass="DropDownList" >
                    <asp:ListItem Selected="true" Text="Pattern Code" Value="PatternCode"></asp:ListItem>
                    <asp:ListItem Text="Pattern Name" Value="PatternName"></asp:ListItem> 
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
						<PagerStyle CssClass="GridPager" />
				      <Columns>
							<asp:TemplateField HeaderText="Pattern Code" HeaderStyle-Width="50" SortExpression="PatternCode">
								<Itemtemplate>
									<asp:Label Runat="server" ID="PatternCode" text='<%# DataBinder.Eval(Container.DataItem, "PatternCode") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:Label Runat="server" ID="PatternCodeEdit" Text='<%# DataBinder.Eval(Container.DataItem, "PatternCode") %>'>
									</asp:Label>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="PatternCodeAdd" CssClass="TextBox" MaxLength="5" Runat="Server" Width="100%"/>
									<cc1:TextBoxWatermarkExtender ID="PatternCodeAdd_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="PatternCodeAdd" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>
								</FooterTemplate>
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Pattern Name" HeaderStyle-Width="150" SortExpression="PatternName">
								<Itemtemplate>
									<asp:Label Runat="server" ID="PatternName" text='<%# DataBinder.Eval(Container.DataItem, "PatternName") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:TextBox Runat="server" CssClass="TextBox" ID="PatternNameEdit" MaxLength="100" Width="100%" Text='<%# DataBinder.Eval(Container.DataItem, "PatternName") %>'>
									</asp:TextBox>
									<cc1:TextBoxWatermarkExtender ID="PatternNameEdit_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="PatternNameEdit" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="PatternNameAdd" CssClass="TextBox" Runat="Server" MaxLength="100" Width="100%"/>
									<cc1:TextBoxWatermarkExtender ID="PatternNameAdd_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="PatternNameAdd" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>
								</FooterTemplate>
							</asp:TemplateField>		
							
							<asp:TemplateField HeaderText="Pattern Formula ( A-F & X )" HeaderStyle-Width="150" SortExpression="PatternShift">
								<Itemtemplate>
									<asp:Label Runat="server" ID="PatternShift" text='<%# DataBinder.Eval(Container.DataItem, "PatternShift") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:TextBox Runat="server" CssClass="TextBox" ID="PatternShiftEdit" MaxLength="50" Width="100%" Text='<%# DataBinder.Eval(Container.DataItem, "PatternShift") %>'>
									</asp:TextBox>
									<cc1:TextBoxWatermarkExtender ID="PatternShiftEdit_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="PatternShiftEdit" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="PatternShiftAdd" CssClass="TextBox" Runat="Server" MaxLength="50" Width="100%"/>
									<cc1:TextBoxWatermarkExtender ID="PatternShiftAdd_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="PatternShiftAdd" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>
								</FooterTemplate>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Shift A" HeaderStyle-Width="85" SortExpression="ShiftA">
								<Itemtemplate>
									<asp:Label Runat="server" ID="ShiftA" text='<%# DataBinder.Eval(Container.DataItem, "ShiftAName") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:DropDownList Runat="server" ID="ShiftAEdit" Width="100%" CssClass="DropDownList"
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "ShiftA") %>' 
                                        DataSourceID="dsShift" DataTextField="ShiftName" 
                                        DataValueField="ShiftCode">
									    
									</asp:DropDownList>								    
								</EditItemTemplate>
								<FooterTemplate>
									<asp:DropDownList ID="ShiftAAdd" Runat="Server" Width="100%" CssClass="DropDownList"									    
                                        DataSourceID="dsShift" DataTextField="ShiftName" 
                                        DataValueField="ShiftCode">
									</asp:DropDownList>
								</FooterTemplate>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Shift B" HeaderStyle-Width="85" SortExpression="ShiftB">
								<Itemtemplate>
									<asp:Label Runat="server" ID="ShiftB" text='<%# DataBinder.Eval(Container.DataItem, "ShiftBName") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:DropDownList Runat="server" ID="ShiftBEdit" Width="100%" CssClass="DropDownList"
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "ShiftB") %>' 
                                        DataSourceID="dsShift" DataTextField="ShiftName" 
                                        DataValueField="ShiftCode">
									    
									</asp:DropDownList>								    
								</EditItemTemplate>
								<FooterTemplate>
									<asp:DropDownList ID="ShiftBAdd" Runat="Server" Width="100%" CssClass="DropDownList"									    
                                        DataSourceID="dsShift" DataTextField="ShiftName" 
                                        DataValueField="ShiftCode">
									</asp:DropDownList>
								</FooterTemplate>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Shift C" HeaderStyle-Width="85" SortExpression="ShiftC">
								<Itemtemplate>
									<asp:Label Runat="server" ID="ShiftC" text='<%# DataBinder.Eval(Container.DataItem, "ShiftCName") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:DropDownList Runat="server" ID="ShiftCEdit" Width="100%" CssClass="DropDownList"
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "ShiftC") %>' 
                                        DataSourceID="dsShift" DataTextField="ShiftName" 
                                        DataValueField="ShiftCode">
									    
									</asp:DropDownList>								    
								</EditItemTemplate>
								<FooterTemplate>
									<asp:DropDownList ID="ShiftCAdd" Runat="Server" Width="100%" CssClass="DropDownList"									    
                                        DataSourceID="dsShift" DataTextField="ShiftName" 
                                        DataValueField="ShiftCode">
									</asp:DropDownList>
								</FooterTemplate>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Shift D" HeaderStyle-Width="85" SortExpression="ShiftD">
								<Itemtemplate>
									<asp:Label Runat="server" ID="ShiftD" text='<%# DataBinder.Eval(Container.DataItem, "ShiftDName") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:DropDownList Runat="server" ID="ShiftDEdit" Width="100%" CssClass="DropDownList"
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "ShiftD") %>' 
                                        DataSourceID="dsShift" DataTextField="ShiftName" 
                                        DataValueField="ShiftCode">
									    
									</asp:DropDownList>								    
								</EditItemTemplate>
								<FooterTemplate>
									<asp:DropDownList ID="ShiftDAdd" Runat="Server" Width="100%" CssClass="DropDownList"									    
                                        DataSourceID="dsShift" DataTextField="ShiftName" 
                                        DataValueField="ShiftCode">
									</asp:DropDownList>
								</FooterTemplate>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Shift E" HeaderStyle-Width="85" SortExpression="ShiftE">
								<Itemtemplate>
									<asp:Label Runat="server" ID="ShiftE" text='<%# DataBinder.Eval(Container.DataItem, "ShiftEName") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:DropDownList Runat="server" ID="ShiftEEdit" Width="100%" CssClass="DropDownList"
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "ShiftE") %>' 
                                        DataSourceID="dsShift" DataTextField="ShiftName" 
                                        DataValueField="ShiftCode">
									    
									</asp:DropDownList>								    
								</EditItemTemplate>
								<FooterTemplate>
									<asp:DropDownList ID="ShiftEAdd" Runat="Server" Width="100%" CssClass="DropDownList"									    
                                        DataSourceID="dsShift" DataTextField="ShiftName" 
                                        DataValueField="ShiftCode">
									</asp:DropDownList>
								</FooterTemplate>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Shift F" HeaderStyle-Width="85" SortExpression="ShiftF">
								<Itemtemplate>
									<asp:Label Runat="server" ID="ShiftF" text='<%# DataBinder.Eval(Container.DataItem, "ShiftFName") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:DropDownList Runat="server" ID="ShiftFEdit" Width="100%" CssClass="DropDownList"
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "ShiftF") %>' 
                                        DataSourceID="dsShift" DataTextField="ShiftName" 
                                        DataValueField="ShiftCode">
									    
									</asp:DropDownList>								    
								</EditItemTemplate>
								<FooterTemplate>
									<asp:DropDownList ID="ShiftFAdd" Runat="Server" Width="100%" CssClass="DropDownList"									    
                                        DataSourceID="dsShift" DataTextField="ShiftName" 
                                        DataValueField="ShiftCode">
									</asp:DropDownList>
								</FooterTemplate>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Shift X" HeaderStyle-Width="85" SortExpression="ShiftX">
								<Itemtemplate>
									<asp:Label Runat="server" ID="ShiftX" text='<%# DataBinder.Eval(Container.DataItem, "ShiftXName") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:DropDownList Runat="server" ID="ShiftXEdit" Width="100%" CssClass="DropDownList"
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "ShiftX") %>' 
                                        DataSourceID="dsShift" DataTextField="ShiftName" 
                                        DataValueField="ShiftCode">
									    
									</asp:DropDownList>								    
								</EditItemTemplate>
								<FooterTemplate>
									<asp:DropDownList ID="ShiftXAdd" Runat="Server" Width="100%" CssClass="DropDownList"									    
                                        DataSourceID="dsShift" DataTextField="ShiftName" 
                                        DataValueField="ShiftCode">
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
							</asp:TemplateField>
							
    					</Columns>
        </asp:GridView>
        <asp:SqlDataSource ID="dsShift" runat="server"                 
                SelectCommand="EXEC S_GetShift">
        </asp:SqlDataSource>
     <asp:Label ID="lstatus" ForeColor="red" runat="server"></asp:Label>
    </div>
    </form>
</body>
</html>
