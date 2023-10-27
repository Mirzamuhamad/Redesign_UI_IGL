<%@ Page Language="VB" AutoEventWireup="false" CodeFile="MsLeaves.aspx.vb" Inherits="MsLeaves_MsLeaves" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitle</title>
    <script src="../../Function/OpenDlg.js" type="text/javascript"></script>
    <script src="../../Function/Function.JS" type="text/javascript"></script>
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div class="Content">
     <div class="H1">Leaves & Permission File</div>
     <hr style="color:Blue" />
     <asp:Panel runat="server" ID="pnlHd">
      <table>
        <tr>
            <td style="wid  th:100px;text-align:right" >Quick Search :</td>
            <td><asp:TextBox runat="server" ID ="tbFilter" CssClass="TextBox" /> 
                <asp:DropDownList runat="server" ID="ddlField" CssClass="DropDownList" > 
                    <asp:ListItem Selected="true" Text="Leaves Code" Value="LeaveCode"></asp:ListItem>
                    <asp:ListItem Text="Leaves Name" Value="LeaveName"></asp:ListItem>                   
                    <asp:ListItem Text="Gender" Value="Gender"></asp:ListItem>
                    <asp:ListItem Text="Dispensasi" Value="Dispensasi"></asp:ListItem>
                    <asp:ListItem Text="Max Taken" Value="MaxTaken"></asp:ListItem>
                    <asp:ListItem Text="Recuring Taken" Value="FgRecuring"></asp:ListItem>
                    <asp:ListItem Text="Max Recuring" Value="MaxRecuring"></asp:ListItem>
                    <asp:ListItem Text="Lead Time" Value="LeadTime"></asp:ListItem>
                    <asp:ListItem Text="Include Holiday" Value="FgHoliday"></asp:ListItem>
                    <asp:ListItem Text="Absence Status Code" Value="AbsStatus"></asp:ListItem>
                    <asp:ListItem Text="Absence Status Name" Value="AbsStatusName"></asp:ListItem>
                    <asp:ListItem Text="Leave Type" Value="LeaveTypeName"></asp:ListItem>
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
                    <asp:ListItem Selected="true" Text="Leaves Code" Value="LeaveCode"></asp:ListItem>
                    <asp:ListItem Text="Leaves Name" Value="LeaveName"></asp:ListItem>                   
                    <asp:ListItem Text="Gender" Value="Gender"></asp:ListItem>
                    <asp:ListItem Text="Dispensasi" Value="Dispensasi"></asp:ListItem>
                    <asp:ListItem Text="Max Taken" Value="MaxTaken"></asp:ListItem>
                    <asp:ListItem Text="Recuring Taken" Value="FgRecuring"></asp:ListItem>
                    <asp:ListItem Text="Max Recuring" Value="MaxRecuring"></asp:ListItem>
                    <asp:ListItem Text="Lead Time" Value="LeadTime"></asp:ListItem>
                    <asp:ListItem Text="Include Holiday" Value="FgHoliday"></asp:ListItem>
                    <asp:ListItem Text="Absence Status Code" Value="AbsStatus"></asp:ListItem>
                    <asp:ListItem Text="Absence Status Name" Value="AbsStatusName"></asp:ListItem>
                    <asp:ListItem Text="Leave Type" Value="LeaveTypeName"></asp:ListItem>
                  </asp:DropDownList>
            </td>
        </tr>
     </table>
     </asp:Panel>
      <br />
      <asp:Button class="bitbtn btnadd" runat="server" ID="btnAdd" Text="Add" />									
      <asp:GridView id="DataGrid" runat="server" ShowFooter="True" AllowSorting="True" 
            AutoGenerateColumns="False" AllowPaging="True" CssClass="Grid">
						<HeaderStyle CssClass="GridHeader" Wrap="false"  ></HeaderStyle>
						<RowStyle CssClass="GridItem" wrap="false" />
						<AlternatingRowStyle CssClass="GridAltItem"/>
						<FooterStyle CssClass="GridFooter" />
						<PagerStyle CssClass="GridPager" HorizontalAlign="Left" />
				      <Columns>
				            <asp:BoundField DataField="LeaveCode" HeaderText="Leaves Code" HeaderStyle-Width="140" SortExpression="LeaveCode"/>
							<asp:BoundField DataField="LeaveName" HeaderText="Leaves Name" HeaderStyle-Width="300" SortExpression="LeaveName"/>
							<asp:BoundField DataField="Gender" HeaderText="Gender" SortExpression="Gender"/>
							<asp:BoundField DataField="Dispensasi" HeaderText="Dispensasi" SortExpression="Dispensasi" ItemStyle-HorizontalAlign="Right"/>
							<asp:BoundField DataField="MaxTaken" HeaderText="Max Taken" SortExpression="MaxTaken" ItemStyle-HorizontalAlign="Right"/>
							<asp:BoundField DataField="FgRecuring" HeaderText="Recuring Taken" SortExpression="FgRecuring" ItemStyle-HorizontalAlign="Center"/>
							<asp:BoundField DataField="MaxRecuring" HeaderText="Max Recuring" SortExpression="MaxRecuring" ItemStyle-HorizontalAlign="Right"/>						
							<asp:BoundField DataField="LeadTime" HeaderText="Lead Time" SortExpression="LeadTime" ItemStyle-HorizontalAlign="Right"/>							
							<asp:BoundField DataField="FgHoliday" HeaderText="Include Holiday" SortExpression="FgHoliday" ItemStyle-HorizontalAlign="Center"/>							
							<asp:BoundField DataField="AbsStatus" HeaderText="Absence Status" SortExpression="AbsStatus" />							
							<asp:BoundField DataField="AbsStatusName" HeaderText="Absence Status Name" SortExpression="AbsStatusName" />				
							<asp:BoundField DataField="LeaveType" HeaderText="Leave Type Code" SortExpression="LeaveType" />										
							<asp:BoundField DataField="LeaveTypeName" HeaderText="Leave Type Name" SortExpression="LeaveTypeName" />							
							<asp:TemplateField HeaderText="Action">
							    <ItemTemplate>
								    <asp:Button ID="btnEdit" runat="server" class="bitbtndt btnedit" Text="Edit" CommandName="Edit"/>
									<asp:Button ID="btndelete" runat="server" class="bitbtndt btndelete" Text="Delete" OnClientClick="return confirm('Sure to delete this data?');" CommandName="Delete"/>									
								</ItemTemplate>								
							</asp:TemplateField>
    					</Columns>
        </asp:GridView>
        <asp:Button class="bitbtn btnadd" runat="server" ID="btnAdd2" Text="Add" />									
      </asp:Panel>
      <asp:Panel runat="server" ID="pnlInput" Visible="false">
        <table>
            <tr>
                <td>Leaves Code</td>
                <td>:</td> 
                <td><asp:TextBox runat="server" CssClass="TextBox" ID="tbCode" MaxLength = "5"/></td>
            </tr>
            <tr>
                <td>Leaves Name</td>
                <td>:</td>
                <td><asp:TextBox runat="server" MaxLength="60" CssClass="TextBox" ID="tbName" Width="250px"/></td>
            </tr>
            <tr>
                <td>Gender</td>
                <td>:</td>
                <td><asp:DropDownList runat="server" CssClass="DropDownList" ID="ddlGender" Width = "71px" Height="18px">
                    <asp:ListItem>Female</asp:ListItem>
                    <asp:ListItem>Male</asp:ListItem>
                    <asp:ListItem>All</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>Dispensasi</td>
                <td>:</td>
                <td><asp:TextBox runat="server" CssClass="TextBox" ID="tbDispensasi" Width="49px"/>
                    &nbsp;Days</td>
            </tr>
            <tr>
                <td>Max Taken</td>
                <td>:</td>
                <td><asp:TextBox runat="server" CssClass="TextBox" ID="tbMaxTaken" Width="49px"/>
                    &nbsp;Days</td>
            </tr>
            <tr>
                <td>Recuring Taken</td>
                <td>:</td>
                <td><asp:DropDownList runat="server" CssClass="DropDownList" ID="ddlFgRecuring" AutoPostBack = "true" OnSelectedIndexChanged = "ddlFgRecuring_SelectedIndexChanged"
                        Width = "35px" Height="18px">
                    <asp:ListItem>Y</asp:ListItem>
                    <asp:ListItem>N</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>Max Recuring</td>
                <td>:</td>
                <td><asp:TextBox runat="server" CssClass="TextBox" ID="tbMaxRecuring" Width="49px"/></td>
            </tr>
            <tr>
                <td>Lead Time</td>
                <td>:</td>
                <td><asp:TextBox runat="server" CssClass="TextBox" ID="tbLeadTime" Width="49px"/>
                    &nbsp;Days</td>
            </tr>
            <tr>
                <td>Include Holiday</td>
                <td>:</td>
                <td><asp:DropDownList runat="server" CssClass="DropDownList" ID="ddlFgHoliday" 
                        Width = "35px" Height="18px">
                    <asp:ListItem>Y</asp:ListItem>
                    <asp:ListItem>N</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>Absence Status</td>
                <td>:</td>
                <td><asp:DropDownList runat="server" CssClass="DropDownList" ID="ddlAbsStatus" 
                        Width="200px">
                    <asp:ListItem Selected = "True">Choose One</asp:ListItem></asp:DropDownList> 
                </td>
            </tr>
            <tr>
                <td>Leave Type</td>
                <td>:</td>
                <td><asp:DropDownList ID="ddlLeaveType" runat="server" CssClass="DropDownList" Width="200px">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td align="center" colspan="3">
                    <asp:Button ID="BtnSave" runat="server" class="bitbtndt btnsave" 
                        CommandName="Update" Text="Save" />
                    &nbsp;
                    <asp:Button ID="btnCancel" runat="server" class="bitbtndt btncancel" 
                        CommandName="Cancel" Text="Cancel" />
                    &nbsp;
                    <asp:Button ID="btnReset" runat="server" class="bitbtndt btncancel" 
                        CommandName="Cancel" Text="Reset" />
                    &nbsp;
                </td>
            </tr>
        </table>
      </asp:Panel>              
     <asp:Label ID="lstatus" ForeColor="red" runat="server"></asp:Label>        
    </div>
    </form>
</body>
</html>
