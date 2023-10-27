    <%@ Page Language="VB" AutoEventWireup="false" CodeFile="MsShift.aspx.vb" Inherits="Master_MsShift_MsShift" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Shift File</title>
    <script src="../../Function/OpenDlg.js" type="text/javascript"></script>
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
     <div class="H1">Shift File</div>
     <hr style="color:Blue" />
     <asp:Panel id="pnlHd" runat="server">
      <table>
        <tr>
            <td style="text-align:right; width:100px" >Quick Search :
                </td>
            <td><asp:TextBox CssClass="TextBox"  runat="server" ID ="tbFilter"/> 
                <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlField" >
                  <asp:ListItem Selected="true" Text="Shift Code" Value="ShiftCode"></asp:ListItem>
                  <asp:ListItem Text="Shift Name" Value="ShiftName"></asp:ListItem>
                  <asp:ListItem Text="Shift Group" Value="ShiftGroup"></asp:ListItem>
                  <asp:ListItem Text="Off" Value="FgOff"></asp:ListItem>
                  <asp:ListItem Text="Active" Value="FgActive"></asp:ListItem>
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
                      <asp:ListItem Selected="true" Text="Shift Code" Value="ShiftCode"></asp:ListItem>
                      <asp:ListItem Text="Shift Name" Value="ShiftName"></asp:ListItem>
                      <asp:ListItem Text="Shift Group" Value="ShiftGroup"></asp:ListItem>
                      <asp:ListItem Text="Off" Value="FgOff"></asp:ListItem>
                      <asp:ListItem Text="Active" Value="FgActive"></asp:ListItem>
                   </asp:DropDownList>
                
            </td>
        </tr>
     </table>
     </asp:Panel>
      <br />
     <%-- <div style="border:0px  solid; width:100%; height:100%; overflow:auto;">--%>
      <asp:GridView id="DataGrid" runat="server" ShowFooter="True" AllowSorting="True" 
            AutoGenerateColumns="False" AllowPaging="True" CssClass="Grid">
						<HeaderStyle CssClass="GridHeader" wrap="false"></HeaderStyle>
						<RowStyle CssClass="GridItem" Wrap="false"/>
						<AlternatingRowStyle CssClass="GridAltItem"/>
						<FooterStyle CssClass="GridFooter" />
						<PagerStyle CssClass="GridPager" />
				      <Columns>
							<asp:TemplateField HeaderText="Shift Code" HeaderStyle-Width="70" SortExpression="ShiftCode">
								<Itemtemplate>
									<asp:Label Runat="server" Width="70" ID="ShiftCode" text='<%# DataBinder.Eval(Container.DataItem, "ShiftCode") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:Label Runat="server" ID="ShiftCodeEdit" MaxLength="5" Width="100%" Text='<%# DataBinder.Eval(Container.DataItem, "ShiftCode") %>'>
									</asp:Label>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="ShiftCodeAdd" CssClass="TextBox" Width="100%" MaxLength="5" Runat="Server"/>
								   <cc1:TextBoxWatermarkExtender ID="ShiftCodeAdd_TextBoxWatermarkExtender" 
                                        runat="server" Enabled="True" TargetControlID="ShiftCodeAdd" WatermarkText="can't blank" WatermarkCssClass="Watermarked" >
                                   </cc1:TextBoxWatermarkExtender>
								</FooterTemplate>
							    <HeaderStyle Width="70px" />
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Shift Name" HeaderStyle-Width="280" SortExpression="ShiftName">
								<Itemtemplate>
									<asp:Label Runat="server" Width="280" ID="ShiftName" text='<%# DataBinder.Eval(Container.DataItem, "ShiftName") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:TextBox Runat="server" ID="ShiftNameEdit" MaxLength="60" CssClass="TextBox" Width="100%" Text='<%# DataBinder.Eval(Container.DataItem, "ShiftName") %>'>
									</asp:TextBox>0									
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="ShiftNameAdd" CssClass="TextBox" Width="100%" MaxLength="60" Runat="Server"/>
									<cc1:TextBoxWatermarkExtender ID="ShiftNameAdd_TextBoxWatermarkExtender" 
                                        runat="server" Enabled="True" TargetControlID="ShiftNameAdd" WatermarkText="can't blank" WatermarkCssClass="Watermarked" >
                                    </cc1:TextBoxWatermarkExtender>
								</FooterTemplate>
							    <HeaderStyle Width="280px" />
							</asp:TemplateField>	
							
							<asp:TemplateField HeaderText="Shift Group" HeaderStyle-Width="210" SortExpression="ShiftGroup">
								<Itemtemplate>
									<asp:Label Runat="server" ID="ShiftGroup" text='<%# DataBinder.Eval(Container.DataItem, "ShiftGrpName") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:DropDownList Runat="server" ID="ShiftGroupEdit" Width="100%" 
                                        CssClass="DropDownList" AutoPostBack="True" SelectedValue='<%# DataBinder.Eval(Container.DataItem, "ShiftGroup") %>'
                                        DataSourceID="dsShiftGroup" DataTextField="ShiftGrpName" 
                                        DataValueField="ShiftGrpCode">
                                        
									</asp:DropDownList>								    
								</EditItemTemplate>
								<FooterTemplate>
									<asp:DropDownList ID="ShiftGroupAdd" Runat="Server" Width="100%" 
                                        CssClass="DropDownList" DataSourceID="dsShiftGroup" DataTextField="ShiftGrpName" 
                                        DataValueField="ShiftGrpCode"> 
                                        
									</asp:DropDownList>
								</FooterTemplate>
							    <HeaderStyle Width="210px" />
							</asp:TemplateField>
													
							<asp:TemplateField HeaderText="Off" HeaderStyle-Width="30" ItemStyle-HorizontalAlign="Center" SortExpression="FgOff">
								<Itemtemplate>
									<asp:Label Runat="server" Width="30" ID="FgOff" text='<%# DataBinder.Eval(Container.DataItem, "FgOff") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:DropDownList ID="FgOffEdit" CssClass="DropDownList" Width="100%" runat="server" SelectedValue='<%# DataBinder.Eval(Container.DataItem, "FgOff") %>'>
									  <asp:ListItem>Y</asp:ListItem>
									  <asp:ListItem>N</asp:ListItem>                                        
									</asp:DropDownList>								    
								</EditItemTemplate>
								<FooterTemplate>
									<asp:DropDownList ID="FgOffAdd" CssClass="DropDownList" Width="100%" runat="server">
									  <asp:ListItem>Y</asp:ListItem>
									  <asp:ListItem Selected="True">N</asp:ListItem>                                        
									</asp:DropDownList>								    
								</FooterTemplate>
							    <HeaderStyle Width="30px" />
                                <ItemStyle HorizontalAlign="Center" />
							</asp:TemplateField>	
							
							<asp:TemplateField HeaderText="Active" HeaderStyle-Width="30" ItemStyle-HorizontalAlign="Center" SortExpression="FgActive">
								<Itemtemplate>
									<asp:Label Runat="server" Width="30" ID="FgActive" text='<%# DataBinder.Eval(Container.DataItem, "FgActive") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:DropDownList ID="FgActiveEdit" CssClass="DropDownList" Width="100%" runat="server" SelectedValue='<%# DataBinder.Eval(Container.DataItem, "FgActive") %>'>
									  <asp:ListItem>Y</asp:ListItem>
									  <asp:ListItem>N</asp:ListItem>                                        
									</asp:DropDownList>								    
								</EditItemTemplate>
								<FooterTemplate>
									<asp:DropDownList ID="FgActiveAdd" CssClass="DropDownList" Width="100%" runat="server">
									  <asp:ListItem Selected="True">Y</asp:ListItem>
									  <asp:ListItem>N</asp:ListItem>                                        
									</asp:DropDownList>								    
								</FooterTemplate>
							    <HeaderStyle Width="30px" />
                                <ItemStyle HorizontalAlign="Center" />
							</asp:TemplateField>	
										
							<asp:TemplateField HeaderText="Action" headerstyle-width="180" >
							    <ItemTemplate>
								    <asp:Button ID="btnEdit" runat="server" class="bitbtndt btnedit" Text="Edit" CommandName="Edit"/>
									<asp:Button ID="btndelete" runat="server" class="bitbtndt btndelete" Text="Delete" OnClientClick="return confirm('Sure to delete this data?');" CommandName="Delete"/>									
									<asp:Button ID="btnView" Width="70px" runat="server" class="bitbtndt btndetail" Text="Detail" CommandName="View" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>"/>
								</ItemTemplate>
								<EditItemTemplate>
									<asp:Button ID="btnUpdate" runat="server" class="bitbtndt btnsave" Text="Save" CommandName="Update"/>									
                                    <asp:Button ID="btnCancel" runat="server" class="bitbtndt btncancel" Text="Cancel" CommandName="Cancel"/>									
								</EditItemTemplate>
								<FooterTemplate>
									<asp:Button class="bitbtndt btnadd" runat="server" ID="btnAdd" Text="Add" CommandName="Insert"/>									
								</FooterTemplate>
							    <HeaderStyle Width="180px" />
							</asp:TemplateField>							
    					</Columns>
        </asp:GridView>
            </asp:Panel>
     <asp:Panel ID="pnlDt" runat="server" Visible = "false">  
     <asp:Label ID="label1" CssClass="H1" runat="server" Text="Shift : " />   
     <asp:Label ID="lbShiftCode" ForeColor="Blue" CssClass="H1" Font-Bold="true" runat="server" />
	 <br />
	 <asp:Button class="bitbtn btnback" width="50px" runat="server" ID="btnBackDtTop" Text="Back" />
	 <asp:Button class="bitbtndt btnadd" width="50px" runat="server" ID="btnAddDt" Text="Add" CommandName="Insert"/>    
	 <div style="border:0px  solid; width:100%; height:100%; overflow:auto;">
	        
            <asp:GridView ID="DataGridDt" runat="server" AllowPaging="True" 
                AllowSorting="True" AutoGenerateColumns="False" ShowFooter="False">
                <HeaderStyle CssClass="GridHeader" wrap="false" />
                <RowStyle CssClass="GridItem" wrap="false" />
                <AlternatingRowStyle CssClass="GridAltItem" />
                <%--<FooterStyle CssClass="GridFooter" Wrap="false" />--%>
                <PagerStyle CssClass="GridPager" />
                <Columns>
                <asp:TemplateField HeaderStyle-Width="110">
                            <ItemTemplate>
                                <asp:DropDownList CssClass="DropDownList" ID="ddl" runat="server">
                                <asp:ListItem Selected="True" Text="Edit" />
                                <asp:ListItem>Copy New</asp:ListItem>
                                <asp:ListItem>Delete</asp:ListItem>
                                </asp:DropDownList>
                                <asp:Button class="btngo" runat="server" ID="btnExpand" Text="G" CommandName="Go" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>"/>
                            </ItemTemplate>
                            <HeaderStyle Width="110px" />
                        </asp:TemplateField>
                    <asp:BoundField DataField="Day" SortExpression="Day" HeaderText="Day" />
                    <asp:BoundField DataField="ShiftIn" SortExpression="ShiftIn" HeaderText="Shift In" />
                    <asp:BoundField DataField="ShiftOut" SortExpression="ShiftOut" HeaderText="Shift Out" />
                    <asp:BoundField DataField="IdleIn1" SortExpression="IdleIn1" HeaderText="Idle In 1" />
                    <asp:BoundField DataField="IdleOut1" SortExpression="IdleOut1" HeaderText="Idle Out 1" />
                    <asp:BoundField DataField="IdleIn2" SortExpression="IdleIn2" HeaderText="Idle In 2" />                    
                    <asp:BoundField DataField="IdleOut2" SortExpression="IdleOut2" HeaderText="Idle Out 2" />
                    <asp:BoundField DataField="IdleIn3" SortExpression="IdleIn3" HeaderText="Idle In 3" />
                    <asp:BoundField DataField="IdleOut3" SortExpression="IdleOut3" HeaderText="Idle Out 3" />
                    <%--<asp:BoundField DataField="UserId" SortExpression="UserId" HeaderText="User Id" />
                    <asp:BoundField DataField="UserDate" SortExpression="UserDate" HeaderText="User Date" />--%>
                    
                                        
                    <asp:TemplateField HeaderStyle-Width="126px" HeaderText="Action" Visible="false"
                        ItemStyle-Wrap="false">
                        <EditItemTemplate>
                            <asp:Button ID="btnUpdate0" runat="server" class="bitbtndt btnsave" Text="Save" CommandName="Update"/>									
                            <asp:Button ID="btnCancel0" runat="server" class="bitbtndt btncancel" Text="Cancel" CommandName="Cancel"/>									
                        </EditItemTemplate>
                        <%--<FooterTemplate>
                            <asp:Button class="bitbtndt btnadd" runat="server" ID="btnAdd0" Text="Add" CommandName="Insert"/>								
                        </FooterTemplate>--%>
                        <ItemTemplate>
                            <asp:Button ID="btnEdit0" runat="server" class="bitbtndt btnedit" Text="Edit" CommandName="Edit"/>
							<asp:Button ID="btnDelete0" runat="server" class="bitbtndt btndelete" Text="Delete" OnClientClick="return confirm('Sure to delete this data?');" CommandName="Delete"/>									
                        </ItemTemplate>
                        <HeaderStyle Width="126px" />
                        <ItemStyle Wrap="False" />
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
	    
        </div>        
	    
	    <asp:Button class="bitbtn btnback" runat="server" width="50px" ID="btnBack2" Text="Back" />
	    <asp:Button class="bitbtndt btnadd" width="50px" runat="server" ID="btnAddDt2" Text="Add" CommandName="Insert"/>    
     </asp:Panel> 
     <asp:Panel ID="pnlInputDt" runat="server" Visible="false">
        <table>
           
            <tr>
                <td>Day</td>
                <td>:</td>
                <td>
                <asp:DropDownList ID="ddlDay" runat="server" CssClass="DropDownList"/>                   
                </td>
            </tr>                  
            <tr>
                <td>Shift In</td>
                <td>:</td>
                <td>
                    <asp:TextBox ID="tbShiftIn" CssClass="TextBox" MaxLength="5" runat="server" />                    
                </td>
            </tr> 
            <tr>
                <td>Shift Out</td>
                <td>:</td>
                <td>
                    <asp:TextBox ID="tbShiftOut" CssClass="TextBox" MaxLength="5" runat="server" />                    
                </td>
            </tr>    
            <tr>
                <td>Idle In 1</td>
                <td>:</td>
                <td>
                    <asp:TextBox ID="tbIdleIn1" CssClass="TextBox" MaxLength="5" runat="server" />
                </td>
            </tr>    
            <tr>
                <td>Idle Out 1</td>
                <td>:</td>
                <td>
                    <asp:TextBox ID="tbIdleOut1" CssClass="TextBox" MaxLength="5" runat="server" />
                </td>
            </tr>                                                    
            <tr>
                <td>Idle In 2</td>
                <td>:</td>
                <td>
                    <asp:TextBox ID="tbIdleIn2" CssClass="TextBox" MaxLength="5" runat="server" />
                </td>
            </tr> 
            <tr>
                <td>Idle Out 2</td>
                <td>:</td>
                <td>
                    <asp:TextBox ID="tbIdleOut2" CssClass="TextBox" MaxLength="5" runat="server" />
                </td>
            </tr> 
            <tr>
                <td>Idle In 3</td>
                <td>:</td>
                <td>
                    <asp:TextBox ID="tbIdleIn3" CssClass="TextBox" MaxLength="5" runat="server" />                    
                </td>
            </tr> 
            <tr>
                <td>Idle Out 3</td>
                <td>:</td>
                <td>
                    <asp:TextBox ID="tbIdleOut3" CssClass="TextBox" MaxLength="5" runat="server" />
                </td>
            </tr> 
                               
            <tr>
                <td>
                    <asp:Button ID="btnCancel" runat="server" class="bitbtndt btncancel" Text="Back" CommandName="Cancel"/>
                </td>
                <td align="center" colspan="3">
                    <asp:Button ID="btnSave" runat="server" class="bitbtndt btnsave" Text="Save" />									
                    <asp:Button ID="btnReset" runat="server" class="bitbtndt btncancel" Text="Reset" CommandName="Reset"/>       
                </td>
            </tr>
        </table>
     </asp:Panel>  
    </div>    
    <asp:SqlDataSource ID="dsShiftGroup" runat="server" 
          SelectCommand="EXEC S_GetMsShiftGroup">
    </asp:SqlDataSource>
    <%--<asp:SqlDataSource ID="dsWrhsType" runat="server" 
      SelectCommand="EXEC S_GetWrhsType">
    </asp:SqlDataSource>--%>
    <asp:Label ID="lstatus" ForeColor="red" runat="server"></asp:Label>
    </form>
    </body>
</html>