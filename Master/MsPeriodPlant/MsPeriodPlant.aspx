<%@ Page Language="VB" AutoEventWireup="false" CodeFile="MsPeriodPlant.aspx.vb" Inherits="MsLeaves_MsLeaves" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Status Tanam File</title>
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
     <div class="H1">Status Tanam File</div>
     <hr style="color:Blue" />
     <asp:Panel runat="server" ID="pnlHd">
      <table>
        <tr>
            <td style="width:100px;text-align:right" >Quick Search :</td>
            <td><asp:TextBox runat="server" ID ="tbFilter" CssClass="TextBox" /> 
                <asp:DropDownList runat="server" ID="ddlField" CssClass="DropDownList" > 
                    <asp:ListItem Selected="true" Text="Plantation Period Code" Value="PlantPeriodCode"></asp:ListItem>
                    <asp:ListItem Text="Plantation Period Name" Value="PlantPeriodName"></asp:ListItem>                   
                    <asp:ListItem Text="Level" Value="LevelPlantName"></asp:ListItem>
                    <asp:ListItem Text="Start Range" Value="StartRange"></asp:ListItem>
                    <asp:ListItem Text="End Range" Value="EndRange"></asp:ListItem>
                    <asp:ListItem Text="Start BJR" Value="StartBJR"></asp:ListItem>
                    <asp:ListItem Text="End BJR" Value="EndBJR"></asp:ListItem>
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
                    <asp:ListItem Selected="true" Text="Plantation Period Code" Value="PlantPeriodCode"></asp:ListItem>
                    <asp:ListItem Text="Plantation Period Name" Value="PlantPeriodName"></asp:ListItem>                   
                    <asp:ListItem Text="Level" Value="LevelPlantName"></asp:ListItem>
                    <asp:ListItem Text="Start Range" Value="StartRange"></asp:ListItem>
                    <asp:ListItem Text="End Range" Value="EndRange"></asp:ListItem>
                    <asp:ListItem Text="Start BJR" Value="StartBJR"></asp:ListItem>
                    <asp:ListItem Text="End BJR" Value="EndBJR"></asp:ListItem>
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
				            <asp:BoundField DataField="PlantPeriodCode" HeaderText="Plantation Period Code" HeaderStyle-Width="30" SortExpression="PlantPeriodCode"/>
							<asp:BoundField DataField="PlantPeriodName" HeaderText="Plantation Period  Name" HeaderStyle-Width="120" SortExpression="PlantPeriodName"/>
							<asp:BoundField DataField="LevelPlant" HeaderText="Level" HeaderStyle-Width="120" SortExpression="LevelPlant" Visible="false"/>
							<asp:BoundField DataField="LevelPlantName" HeaderText="Level" HeaderStyle-Width="120" SortExpression="LevelPlantName"/>
							<asp:BoundField DataField="StartRange" HeaderText="Start Range" SortExpression="StartRange" ItemStyle-HorizontalAlign="Right"/>
							<asp:BoundField DataField="EndRange" HeaderText="End Range" SortExpression="EndRange" ItemStyle-HorizontalAlign="Right"/>
							<asp:BoundField DataField="StartBJR" HeaderText="Start BJR" SortExpression="StartBJR" ItemStyle-HorizontalAlign="Center"/>
							<asp:BoundField DataField="EndBJR" HeaderText="End BJR" SortExpression="EndBJR" ItemStyle-HorizontalAlign="Right"/>						
							<asp:BoundField DataField="FactorRate" HeaderText="Factor Rate" SortExpression="FactorRate" ItemStyle-HorizontalAlign="Right"/>
							<asp:BoundField DataField="PremiKrgPerJJg" HeaderText="Premi Krg/JJG" SortExpression="PremiKrgPerJJg" ItemStyle-HorizontalAlign="Right"/>													
							<asp:TemplateField HeaderText="Action">
							    <ItemTemplate>
								    <asp:Button ID="btnEdit" runat="server" class="bitbtndt btnedit" Text="Edit" CommandName="Edit"/>
									<asp:Button ID="btndelete" runat="server" class="bitbtndt btndelete" Text="Delete" OnClientClick="return confirm('Sure to delete this data?');" CommandName="Delete"/>									
								</ItemTemplate>								
							</asp:TemplateField>
    					</Columns>
        </asp:GridView>
        <asp:Button class="bitbtn btnadd" runat="server" ID="btnAdd2" Text="Add" Visible="false" />									
      </asp:Panel>
      <asp:Panel runat="server" ID="pnlInput" Visible="false">
        <table>
            <tr>
                <td>Plantation Period Code</td>
                <td>:</td> 
                <td><asp:TextBox runat="server" CssClass="TextBox" ID="tbCode" MaxLength = "5"/></td>
            </tr>
            <tr>
                <td>Plantation Period Name</td>
                <td>:</td>
                <td><asp:TextBox runat="server" MaxLength="60" CssClass="TextBox" ID="tbName" Width="250px"/></td>
            </tr>
            <tr>
                <td>Level Plantation</td>
                <td>:</td>
                <td><asp:DropDownList runat="server" CssClass="DropDownList" ID="ddlLevelPlant" Width = "250px" Height="18px">
                    <asp:ListItem Selected = "True">Choose One</asp:ListItem></asp:DropDownList>
                </td>
            </tr>
            
            <tr>
                <td>Range Period</td>
                <td>:</td>
                <td>
                <asp:DropDownList runat="server" CssClass="DropDownList" ID="ddlRangeType" Width = "62px" Height="18px">
                    <asp:ListItem>Month</asp:ListItem>
                    <%--<asp:ListItem>Day</asp:ListItem>--%>
                    <asp:ListItem>Year</asp:ListItem>
                    </asp:DropDownList>
                    <asp:TextBox runat="server" CssClass="TextBox" ID="tbStartRange" Width="60px"/>    
                    <asp:TextBox runat="server" CssClass="TextBox" ID="tbEndRange" Width="60px"/>
                </td>
            </tr>            
            <tr>
                <td>Range BJR</td>
                <td>:</td>
                <td><asp:TextBox runat="server" CssClass="TextBox" ID="tbStartBJR" Width="60px"/>
                    &nbsp;-&nbsp;
                    <asp:TextBox runat="server" CssClass="TextBox" ID="tbEndBJR" Width="60px"/>
                    &nbsp;(Kgs)
                </td>
            </tr>
            <tr>
                <td>Factor Rate</td>
                <td>:</td> 
                <td><asp:TextBox runat="server" CssClass="TextBox" ID="tbFactorRate" Width = "60px"/></td>
            </tr>
            
            <tr>
                <td>Premi Krg/jjg</td>
                <td>:</td> 
                <td><asp:TextBox runat="server" CssClass="TextBox" ID="tbPremiKrgPerJJg" Width = "60px"/></td>
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
