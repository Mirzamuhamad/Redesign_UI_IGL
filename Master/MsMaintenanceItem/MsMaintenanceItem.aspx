<%@ Page Language="VB" AutoEventWireup="false" CodeFile="MsMaintenanceItem.aspx.vb" Inherits="MsMaintenanceItem_MsMaintenanceItem" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitle</title>
    <script src="../../Function/OpenDlg.JS" type="text/javascript"></script>
    <script src="../../Function/Function.JS" type="text/javascript"></script>
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .style1
        {
            width: 20px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div class="Content">
     <div class="H1">Maintenance Item File</div>
     <hr style="color:Blue" />
     <asp:Panel runat="server" ID="pnlHd">
      <table>
        <tr>
            <td style="wid  th:100px;text-align:right" >Quick Search :</td>
            <td><asp:TextBox runat="server" ID ="tbFilter" CssClass="TextBox" /> 
                <asp:DropDownList runat="server" ID="ddlField" CssClass="DropDownList" > 
                    <asp:ListItem Selected="true" Text="Maintenance Item No" Value="ItemNo"></asp:ListItem>
                    <asp:ListItem Text="Maintenance Item Name" Value="ItemName"></asp:ListItem>                   
                    <%--<asp:ListItem Text="Reff. Code" Value="ReffCode"></asp:ListItem>--%>
                    <asp:ListItem Text="Maintenance Type Code" Value="MaintenanceType"></asp:ListItem>
                    <asp:ListItem Text="Maintenance Type Name" Value="MaintenanceTypeName"></asp:ListItem>
                    <%--<asp:ListItem Text="Maintenance Section" Value="MTNSectionName"></asp:ListItem>--%>
                    <asp:ListItem Text="Account Mtn. Expense" Value="Account"></asp:ListItem>
                    <asp:ListItem Text="Account Mtn. Expense Name" Value="Description"></asp:ListItem>
                    <asp:ListItem Text="Location" Value="Location"></asp:ListItem>
                    <asp:ListItem Text="P I C" Value="PIC"></asp:ListItem>
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
            <td><asp:TextBox runat="server" ID ="tbfilter2" CssClass="TextBox"/> 
                  <asp:DropDownList runat="server" ID="ddlField2" CssClass="DropDownList" >
                    <asp:ListItem Selected="true" Text="Maintenance Item No" Value="ItemNo"></asp:ListItem>
                    <asp:ListItem Text="Maintenance Item Name" Value="ItemName"></asp:ListItem>                   
                    <%--<asp:ListItem Text="Reff. Code" Value="ReffCode"></asp:ListItem>--%>
                    <asp:ListItem Text="Maintenance Type Code" Value="MaintenanceType"></asp:ListItem>
                    <asp:ListItem Text="Maintenance Type Name" Value="MaintenanceTypeName"></asp:ListItem>
                    <%--<asp:ListItem Text="Maintenance Section" Value="MTNSectionName"></asp:ListItem>--%>
                    <asp:ListItem Text="Account Mtn. Expense" Value="Account"></asp:ListItem>
                    <asp:ListItem Text="Account Mtn. Expense Name" Value="Description"></asp:ListItem>
                    <asp:ListItem Text="Location" Value="Location"></asp:ListItem>
                    <asp:ListItem Text="P I C" Value="PIC"></asp:ListItem>
                  </asp:DropDownList>
            </td>
        </tr>
     </table>
     </asp:Panel>
      <br />
      <asp:Button class="bitbtn btnadd" runat="server" ID="btnAdd" Text="Add" />									
      <asp:GridView id="DataGrid" runat="server" ShowFooter="True" AllowSorting="True" 
            AutoGenerateColumns="False" AllowPaging="True" CssClass="Grid">
						<HeaderStyle CssClass="GridHeader" Wrap ="false"></HeaderStyle>
						<RowStyle CssClass="GridItem" wrap="false" />
						<AlternatingRowStyle CssClass="GridAltItem"/>
						<FooterStyle CssClass="GridFooter" />
						<PagerStyle CssClass="GridPager" HorizontalAlign="Left" />
				      <Columns>
				            <asp:BoundField DataField="ItemNo" HeaderText="Maintenance Item No" HeaderStyle-Width="140" SortExpression="ItemNo" ItemStyle-HorizontalAlign = "Left"/>
							<asp:BoundField DataField="ItemName" HeaderText="Maintenance Item Name" HeaderStyle-Width="300" SortExpression="ItemName"/>
							<%--<asp:BoundField DataField="ReffCode" HeaderText="Reff. Code" SortExpression="ReffCode"/>--%>
							<asp:BoundField DataField="MaintenanceType" HeaderText="Maintenance Type Code" HeaderStyle-Width="220" SortExpression="MaintenanceType" />
							<asp:BoundField DataField="MaintenanceTypeName" HeaderText="Maintenance Type Name" HeaderStyle-Width="220" SortExpression="MaintenanceTypeName"/>
							<asp:BoundField DataField="MTNSectionName" HeaderText="Maintenance Section" SortExpression="MTNSectionName"/>
							<asp:BoundField DataField="AccMTNExpense" HeaderText="Account Mtn. Expense" HeaderStyle-Width="220" SortExpression="AccMtnExpense" />						
							<asp:BoundField DataField="Description" HeaderText="Account Mtn. Expense Name" HeaderStyle-Width="220" SortExpression="Description" />							
							<asp:BoundField DataField="Location" HeaderText="Location" HeaderStyle-Width="150" SortExpression="Location" />							
							<asp:BoundField DataField="PIC" HeaderText="P I C" HeaderStyle-Width="150" SortExpression="PIC" />							
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
                <td>Maintenance Item No</td>
                <td>:</td>
                <td><asp:TextBox runat="server" CssClass="TextBox" ID="tbItemNo" MaxLength="10" />
                    <asp:Label ID="Label1" runat="server" Text="*" ForeColor = "Red"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>Maintenance Item Name</td>
                <td>:</td>
                <td><asp:TextBox runat="server" MaxLength="60" CssClass="TextBox" ID="tbItemName" Width="250px" OnTextChanged = "tbItemName_TextChanged"/>
                    <asp:Label ID="Label2" runat="server" ForeColor="Red" Text="*"></asp:Label>
                </td>
            </tr>
            <%--<tr>
                <td>Reff. Code</td>
                <td>:</td>
                <td><asp:TextBox runat="server" MaxLength="60" CssClass="TextBoxR" ID="tbReffCode" 
                        Width="160px" ReadOnly="True"/>
                    <asp:Button class="btngo" runat="server" ID="btnReff" Text="..."/> 
                </td>
            </tr>--%>
            <tr>
                <td>Maintenance Type</td>
                <td>:</td>
                <td><asp:DropDownList runat="server" CssClass="DropDownList" ID="ddlMtnType" 
                        Width="253px" AutoPostBack="True" 
                        OnSelectedIndexChanged = "ddlMtnType_SelectedIndexChanged" Height="16px">
                    <asp:ListItem Selected = "True">Choose One</asp:ListItem></asp:DropDownList>
                    <asp:Label ID="Label3" runat="server" ForeColor="Red" Text="*"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>Maintenance Setion</td>
                <td>:</td>
                <td><asp:TextBox runat="server" MaxLength="60" CssClass="TextBoxR" 
                        ID="tbMtnSection" Width="250px" ReadOnly="True"/></td>
            </tr>
            <tr>
                <td>Account Mtn. Expense</td>
                <td>:</td>
                <td><asp:TextBox runat="server" MaxLength="12" CssClass="TextBox" ID="tbAccount" 
                        Width="127px" AutoPostBack="True"/>
                    <asp:TextBox runat="server" MaxLength="60" CssClass="TextBoxR" 
                        ID="tbAccountName" Width="250px" ReadOnly="True"/>
                    <asp:Button class="btngo" runat="server" ID="btnAcc" Text="..."/> 
                    <asp:Label ID="Label4" runat="server" ForeColor="Red" Text="*"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>Location</td>
                <td>:</td>
                <td><asp:TextBox ID="tbLocation" runat="server" CssClass="TextBox" MaxLength="60" Width="250px" />
                </td>
            </tr>
            <tr>
                <td>P I C</td>
                <td>:</td>
                <td><asp:TextBox ID="tbPIC" runat="server" CssClass="TextBox" MaxLength="60" Width="250px" />
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
