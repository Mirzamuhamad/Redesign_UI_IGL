<%@ Page Language="VB" AutoEventWireup="false" CodeFile="MsMaintenancePattern.aspx.vb" Inherits="Master_MsMaintenancePattern_MsMaintenancePattern" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitle</title>
    <script src="../../Function/Function.JS" type="text/javascript"></script>
    <script src="../../Function/OpenDlg.js" type="text/javascript"></script>
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
 <link href="https://fonts.googleapis.com/css2?family=Muli:wght@300;400;600&display=swap" rel="stylesheet">
    
    <script type="text/javascript">   
               
        function PressWeek()
        {
            var _result = false;

            if(event.keyCode==110 || event.keyCode==8 || event.keyCode == 9 || event.keyCode==37 || event.keyCode==39 || event.keyCode==188 || ( event.keyCode >= 49 && event.keyCode <= 53 ))
            {
                _result = true;
            }
            else
            {
                _result = false;
            }
            
            return _result;
        }  
        
        function PressMonth()
        {
            var _result = false;

            if(event.keyCode==110 || event.keyCode==8 || event.keyCode == 9 || event.keyCode==37 || event.keyCode==39 || event.keyCode==188 || ( event.keyCode >= 48 && event.keyCode <= 57 ))
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
    <link href="../../JS/jquerymodal/jquery-ui.css" rel="stylesheet" type="text/css" />
    
    
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div class="Content">
     <div class="H1">Maintenance Pattern File</div>
     <hr style="color:Blue" />
      <table>
        <tr>
            <td style="width:100px;text-align:right" >Quick Search :</td>
            <td><asp:TextBox runat="server" ID ="tbFilter" CssClass="TextBox"/> 
                <asp:DropDownList runat="server" ID="ddlField" CssClass="DropDownList" >
                  <asp:ListItem Selected="true" Text="Pattern" Value="PatternCode"></asp:ListItem>
                    <asp:ListItem Text="Pattern Name" Value="PatternName"></asp:ListItem>                   
                    <asp:ListItem Text="Pattern Type" Value="PatternType"></asp:ListItem>
                    <asp:ListItem Text="Range Month" Value="XMonth"></asp:ListItem>
                    <asp:ListItem Text="@ Month" Value="EveryMonth"></asp:ListItem>
                    <%--<asp:ListItem Text="Maintenance Section" Value="MTNSectionName"></asp:ListItem>--%>
                    <asp:ListItem Text="@ Week" Value="EveryWeek"></asp:ListItem>       
                </asp:DropDownList>     
                <asp:Button class="bitbtn btnsearch" runat="server" ID="btnSearch" Text="Search" />
                <asp:Button class="btngo" runat="server" ID="btnExpand" Text="..."/>
                <asp:Button class="bitbtn btnprint" runat="server" ID="btnPrint" Text="Print"/>
            </td>
            <td class="style1">&nbsp;</td>
            <td>Show Records :</td>
            <td>
                <asp:DropDownList ID="ddlRow" runat="server" AutoPostBack="True" 
                    CssClass="DropDownList">
                    <asp:ListItem Selected="True" Value="15">Choose One</asp:ListItem>
                    <asp:ListItem>20</asp:ListItem>
                    <asp:ListItem>30</asp:ListItem>
                    <asp:ListItem>40</asp:ListItem>
                    <asp:ListItem>50</asp:ListItem>
                    <asp:ListItem>100</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td>Rows</td>
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
                    <asp:ListItem Selected="true" Text="Pattern" Value="PatternCode"></asp:ListItem>
                    <asp:ListItem Text="Pattern Name" Value="PatternName"></asp:ListItem>                   
                    <asp:ListItem Text="Pattern Type" Value="PatternType"></asp:ListItem>
                    <asp:ListItem Text="Range Month" Value="XMonth"></asp:ListItem>
                    <asp:ListItem Text="@ Month" Value="EveryMonth"></asp:ListItem>
                    <%--<asp:ListItem Text="Maintenance Section" Value="MTNSectionName"></asp:ListItem>--%>
                    <asp:ListItem Text="@ Week" Value="EveryWeek"></asp:ListItem>         
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
							<asp:TemplateField HeaderText="Pattern" HeaderStyle-Width="100" SortExpression="PatternCode">
								<Itemtemplate>
									<asp:Label Runat="server" ID="PatternCode" text='<%# DataBinder.Eval(Container.DataItem, "PatternCode") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:Label Runat="server" ID="PatternCodeEdit" Text='<%# DataBinder.Eval(Container.DataItem, "PatternCode") %>'>
									</asp:Label>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="PatternCodeAdd" CssClass="TextBox" MaxLength="3" Runat="Server" Width="100%"/>
									<cc1:TextBoxWatermarkExtender ID="PatternCodeAdd_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="PatternCodeAdd" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>
								</FooterTemplate>

                                <HeaderStyle Width="100px"></HeaderStyle>
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Pattern Name" HeaderStyle-Width="320" SortExpression="PatternName">
								<Itemtemplate>
									<asp:Label Runat="server" ID="PatternName" text='<%# DataBinder.Eval(Container.DataItem, "PatternName") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:TextBox Runat="server" CssClass="TextBox" ID="PatternNameEdit" MaxLength="60" Width="100%" Text='<%# DataBinder.Eval(Container.DataItem, "PatternName") %>'>
									</asp:TextBox>
									<cc1:TextBoxWatermarkExtender ID="PatternNameEdit_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="PatternNameEdit" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="PatternNameAdd" CssClass="TextBox" MaxLength="60" Runat="Server" Width="100%"/>
									<cc1:TextBoxWatermarkExtender ID="PatternNameAdd_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="PatternNameAdd" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>
								</FooterTemplate>

                                <HeaderStyle Width="320px"></HeaderStyle>
							</asp:TemplateField>		
							
							<asp:TemplateField HeaderText="Pattern Type" HeaderStyle-Width="80" ItemStyle-HorizontalAlign="Center" SortExpression="PatternType">
								<Itemtemplate>
									<asp:Label Runat="server" Width="80" ID="PatternType" TEXT='<%# DataBinder.Eval(Container.DataItem, "PatternType") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:DropDownList ID="PatternTypeEdit" CssClass="DropDownList" Width="100%" MaxLength="10" runat="server" AutoPostBack = "true"
									 OnSelectedIndexChanged="ddlPatternTypeEdit_SelectedIndexChanged" SelectedValue='<%# DataBinder.Eval(Container.DataItem, "PatternType") %>'>
									  <asp:ListItem>Interval</asp:ListItem>
									  <asp:ListItem>Every</asp:ListItem>                                        									  
								</asp:DropDownList>								    
								</EditItemTemplate>
								<FooterTemplate>
									<asp:DropDownList ID="PatternTypeAdd" CssClass="DropDownList" Width="100%" MaxLength="10" runat="server" AutoPostBack = "true"
									 OnSelectedIndexChanged="ddlPatternTypeadd_SelectedIndexChanged" >
									  <asp:ListItem Selected="True">Interval</asp:ListItem>
									  <asp:ListItem>Every</asp:ListItem>                                        									  
									</asp:DropDownList>								    
								</FooterTemplate>
								<ControlStyle Width="100px" />
							    <HeaderStyle Width="80px" />
                                <ItemStyle HorizontalAlign="Left" />
							</asp:TemplateField>	
							
							<asp:TemplateField HeaderText="Range Month" HeaderStyle-Width="320" SortExpression="XMonth">
								<Itemtemplate>
									<asp:Label Runat="server" ID="XMonth" text='<%# DataBinder.Eval(Container.DataItem, "XMonth") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:TextBox Runat="server" CssClass="TextBox" ID="tbXMonthEdit" MaxLength="60" Width="100%" Text='<%# DataBinder.Eval(Container.DataItem, "XMonth") %>'>
									</asp:TextBox>
									<cc1:TextBoxWatermarkExtender ID="XMonthEdit_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="tbXMonthEdit" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="tbXMonthAdd" CssClass="TextBox" MaxLength="60" Runat="Server" Width="100%"/>
									<cc1:TextBoxWatermarkExtender ID="XMonthAdd_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="tbXMonthAdd" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>
								</FooterTemplate>

                                <HeaderStyle Width="320px"></HeaderStyle>
							</asp:TemplateField>							
							
							<asp:TemplateField HeaderText="@Month [1..12],[1..12]" HeaderStyle-Width="320" SortExpression="EveryMonth">
								<Itemtemplate>
									<asp:Label Runat="server" ID="EveryMonth" text='<%# DataBinder.Eval(Container.DataItem, "EveryMonth") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:TextBox Runat="server" CssClass="TextBox" ID="tbEveryMonthEdit" MaxLength="60" Width="100%" Text='<%# DataBinder.Eval(Container.DataItem, "EveryMonth") %>'>
									</asp:TextBox>
									<%--<cc1:TextBoxWatermarkExtender ID="EveryMonthEdit_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="tbEveryMonthEdit" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>--%>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="tbEveryMonthAdd" CssClass="TextBox" MaxLength="60" Runat="Server" Width="100%"/>
									<%--<cc1:TextBoxWatermarkExtender ID="EveryMonthAdd_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="tbEveryMonthAdd" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>--%>
								</FooterTemplate>

                                <HeaderStyle Width="320px"></HeaderStyle>
							</asp:TemplateField>						
							
							<asp:TemplateField HeaderText="@Week [1..5],[1..5]" HeaderStyle-Width="320" SortExpression="EveryWeek">
								<Itemtemplate>
									<asp:Label Runat="server" ID="EveryWeek" text='<%# DataBinder.Eval(Container.DataItem, "EveryWeek") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:TextBox Runat="server" CssClass="TextBox" ID="EveryWeekEdit" MaxLength="60" Width="100%" Text='<%# DataBinder.Eval(Container.DataItem, "EveryWeek") %>'>
									</asp:TextBox>
									<%--<cc1:TextBoxWatermarkExtender ID="EveryWeekEdit_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="EveryWeekEdit" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>--%>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="EveryWeekAdd" CssClass="TextBox" MaxLength="60" Runat="Server" Width="100%"/>
									<%--<cc1:TextBoxWatermarkExtender ID="EveryWeekAdd_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="EveryWeekAdd" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>--%>
								</FooterTemplate>

                                <HeaderStyle Width="320px"></HeaderStyle>
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
        <%--<asp:SqlDataSource ID="dsProductType" runat="server" SelectCommand="EXEC S_GetProductType">
        </asp:SqlDataSource>--%>

     <asp:Label ID="lstatus" ForeColor="red" runat="server"></asp:Label>        
    </div>
    </form>
</body>
</html>
