<%@ Page Language="VB" AutoEventWireup="false" CodeFile="MsModule.aspx.vb" Inherits="MsModule_MsModule" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Module File</title>
    <script src="../../Function/OpenDlg.js" type="text/javascript"></script>
    <script src="../../Function/Function.JS" type="text/javascript"></script>
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
 <link href="https://fonts.googleapis.com/css2?family=Muli:wght@300;400;600&display=swap" rel="stylesheet">
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div class="Content">
     <div class="H1">Module File</div>
     <hr style="color:Blue" />
     <asp:Panel runat="server" ID="pnlHd">
      <table>
        <tr>
            <td style="width:100px;text-align:right" >Quick Search :</td>
            <td><asp:TextBox runat="server" ID ="tbFilter" CssClass="TextBox" /> 
                <asp:DropDownList runat="server" ID="ddlField" CssClass="DropDownList" > 
                    <asp:ListItem Selected="true" Text="Module Code" Value="ModuleCode"></asp:ListItem>
                    <asp:ListItem Text="Module Name" Value="ModuleName"></asp:ListItem>                   
                    <asp:ListItem Text="Cluster" Value="ClusterName"></asp:ListItem>
                    <asp:ListItem Text="Max Capacity" Value="MaxCap"></asp:ListItem>
                    <asp:ListItem Text="Luas Areal" Value="Area"></asp:ListItem>
                    <asp:ListItem Text="PIC" Value="PICName"></asp:ListItem>
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
                    <asp:ListItem Selected="true" Text="Module Code" Value="ModuleCode"></asp:ListItem>
                    <asp:ListItem Text="Module Name" Value="ModuleName"></asp:ListItem>                   
                    <asp:ListItem Text="Cluster" Value="ClusterName"></asp:ListItem>
                    <asp:ListItem Text="Max Capacity" Value="MaxCap"></asp:ListItem>
                    <asp:ListItem Text="Luas Areal" Value="Area"></asp:ListItem>
                    <asp:ListItem Text="PIC" Value="PICName"></asp:ListItem>
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
				            <asp:BoundField DataField="ModuleCode" HeaderText="Module Code" HeaderStyle-Width="50" SortExpression="ModuleCode"/>
							<asp:BoundField DataField="ModuleName" HeaderText="Module Name" HeaderStyle-Width="200" SortExpression="ModuleName"/>
							<asp:BoundField DataField="ClusterName" HeaderText="Cluster" HeaderStyle-Width="200" SortExpression="ClusterName"/>
							<asp:BoundField DataField="MaxCap" HeaderText="Max Capacity" SortExpression="MaxCap" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:#,##.##}"/>
							<asp:BoundField DataField="Area" HeaderText="Luas Area" SortExpression="Area" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:#,##.##}"/>
							<asp:BoundField DataField="PICName" HeaderText="PIC" SortExpression="PICName" HeaderStyle-Width="200"/>												
							<asp:TemplateField HeaderText="Action">
							    <ItemTemplate>
								    <asp:Button ID="btnEdit" runat="server" class="bitbtndt btnedit" Text="Edit" CommandName="Edit"/>
									<asp:Button ID="btndelete" runat="server" class="bitbtndt btndelete" Text="Delete" OnClientClick="return confirm('Sure to delete this data?');" CommandName="Delete"/>									
								</ItemTemplate>								
							</asp:TemplateField>
    					</Columns>
        </asp:GridView>
        <asp:Button class="bitbtn btnadd" runat="server" ID="btnAdd2" Text="Add" 
             Visible="False" />									
      </asp:Panel>
      <asp:Panel runat="server" ID="pnlInput" Visible="false">
        <table>
            <tr>
                <td>Module Code</td>
                <td>:</td> 
                <td><asp:TextBox runat="server" CssClass="TextBox" ID="tbCode" MaxLength = "5"/></td>
            </tr>
            <tr>
                <td>Module Name</td>
                <td>:</td>
                <td><asp:TextBox runat="server" MaxLength="60" CssClass="TextBox" ID="tbName" Width="250px"/></td>
            </tr>
            <tr>
                <td>Cluster</td>
                <td>:</td>
                <td><asp:DropDownList runat="server" CssClass="DropDownList" ID="ddlCluster" MaxLength = "5"
                        Width = "250px" Height="18px">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>Max Capacity</td>
                <td>:</td>
                <td><asp:TextBox runat="server" CssClass="TextBox" ID="tbMaxCap" MaxLength = "9" Width="49px"/>
                  &nbsp;Pokok</td>
            </tr>
            <tr>
                <td>Luas Area</td>
                <td>:</td>
                <td><asp:TextBox runat="server" CssClass="TextBox" ID="tbArea" MaxLength = "9" Width="49px"/>
                    </td>
            </tr>
            <tr>
                <td>PIC</td>
                <td>:</td>
                <td><asp:TextBox runat="server" CssClass="TextBox" ID="tbPIC" 
                        Width="127px" AutoPostBack="True"/>
                    <asp:TextBox runat="server" MaxLength="60" CssClass="TextBoxR" 
                        ID="tbPIC2" Width="250px" ReadOnly="True"/>
                    <asp:Button class="btngo" runat="server" ID="btnPIC" Text="..."/> 
                    <asp:Label ID="Label4" runat="server" ForeColor="Red" Text="*"></asp:Label>
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
