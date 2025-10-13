<%@ Page Language="VB" AutoEventWireup="false" CodeFile="MsJobPlant.aspx.vb" Inherits="MsJobPlant_MsJobPlant" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Job Plantation File</title>
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
     <div class="H1">Job Plantation File</div>
     <hr style="color:Blue" />
     <asp:Panel runat="server" ID="pnlHd">
      <table>
        <tr>
            <td style="width:100px;text-align:right" >Quick Search :</td>
            <td><asp:TextBox runat="server" ID ="tbFilter" CssClass="TextBox" /> 
                <asp:DropDownList runat="server" ID="ddlField" CssClass="DropDownList" > 
                    <asp:ListItem Selected="true" Text="Job Code" Value="JobCode"></asp:ListItem>
                    <asp:ListItem Text="Job Name" Value="JobName"></asp:ListItem>                   
                    <asp:ListItem Text="Level Plantation" Value="LevelPlantName"></asp:ListItem>
                    <asp:ListItem Text="Job Gruop" Value="JobGroupPlantName"></asp:ListItem>
                    <asp:ListItem Text="Job No" Value="JobNo"></asp:ListItem>
                    <asp:ListItem Text="Unit Areal" Value="UnitAreal"></asp:ListItem>
                    <asp:ListItem Text="Unit Norma" Value="Unit"></asp:ListItem>
                    <asp:ListItem Text="Unit Dosis" Value="UnitConvert"></asp:ListItem>
                    <asp:ListItem Text="Acc Expense" Value="AccExpense"></asp:ListItem>
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
                    <asp:ListItem Selected="true" Text="Job Code" Value="JobCode"></asp:ListItem>
                    <asp:ListItem Text="Job Name" Value="JobName"></asp:ListItem>                   
                    <asp:ListItem Text="Level Plantation" Value="LevelPlantName"></asp:ListItem>
                    <asp:ListItem Text="Job Gruop" Value="JobGroupPlantName"></asp:ListItem>
                    <asp:ListItem Text="Job No" Value="JobNo"></asp:ListItem>
                    <asp:ListItem Text="Unit Areal" Value="UnitAreal"></asp:ListItem>
                    <asp:ListItem Text="Unit Norma" Value="Unit"></asp:ListItem>
                    <asp:ListItem Text="Unit Dosis" Value="UnitConvert"></asp:ListItem>
                    <asp:ListItem Text="Acc Expense" Value="AccExpense"></asp:ListItem>
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
				            <asp:TemplateField HeaderStyle-Width="110" HeaderText="Action">
                                  <ItemTemplate>
                                      <asp:DropDownList CssClass="DropDownList" ID="ddl" runat="server">
                                          <asp:ListItem Selected="True" Text="View" />
                                          <asp:ListItem Text="Edit" />
                                          <asp:ListItem Text="Non Active" />
                                          <asp:ListItem Text="Delete" />
                                      </asp:DropDownList>
                                      <asp:Button class="btngo" runat="server" ID="btnGo" Text="G" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" CommandName="Go" />                                      
                                  </ItemTemplate>
                                  <HeaderStyle Width="110px" />
                            </asp:TemplateField>
				            <asp:BoundField DataField="JobCode" HeaderText="Job Code" HeaderStyle-Width="140" SortExpression="JobCode"/>
							<asp:BoundField DataField="JobName" HeaderText="Job Name" HeaderStyle-Width="300" SortExpression="JobName"/>
							<asp:BoundField DataField="LevelPlantName" HeaderText="Level Plantation " SortExpression="LevelPlantName"/>
							<asp:BoundField DataField="JobGroupPlantName" HeaderText="Job Group" SortExpression="JobGroupPlantName"/>							
							<asp:BoundField DataField="Unit" HeaderText="Unit Norma" SortExpression="Unit" ItemStyle-HorizontalAlign="Center"/>
							<asp:BoundField DataField="AccExpenseName" HeaderText="Acc Expanse" SortExpression="AccExpenseName"/>
							<asp:BoundField DataField="AccAssetName" HeaderText="Acc Asset" SortExpression="AccAssetName"/>
							<asp:BoundField DataField="FgActive" HeaderText="Active" HeaderStyle-Width="20" SortExpression="FgActive"/>				
							
    					</Columns>
        </asp:GridView>
        <asp:Button class="bitbtn btnadd" runat="server" ID="btnAdd2" Text="Add" 
             Visible="False" />									
      </asp:Panel>
      <asp:Panel runat="server" ID="pnlInput" Visible="false">
        <table>
            <tr>
                <td>Job Code</td>
                <td>:</td> 
                <td><asp:TextBox runat="server" CssClass="TextBox" ID="tbCode" MaxLength = "5"/></td>
            </tr>
            <tr>
                <td>Job Name</td>
                <td>:</td>
                <td><asp:TextBox runat="server" MaxLength="60" CssClass="TextBox" ID="tbName" ValidationGroup="Input" Width="300px"/></td>
            </tr>
            <tr>
                <td>Level Plantation</td>
                <td>:</td>
                <td><asp:DropDownList runat="server" CssClass="DropDownList" ID="ddlLevelPlant" ValidationGroup="Input" MaxLength = "3"
                        Width = "250px" Height="18px">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>Job Group</td>
                <td>:</td>
                <td><asp:DropDownList runat="server" CssClass="DropDownList" ID="ddlJobGroupPlant" ValidationGroup="Input" MaxLength = "3"
                        Width = "250px" Height="18px">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>Job No</td>
                <td>:</td>
                <td><asp:TextBox runat="server" MaxLength="5" CssClass="TextBox" ID="tbJobNo" ValidationGroup="Input" Width="80px"/></td>
            </tr>
            <tr>
                <td>Unit Areal</td>
                <td>:</td>
                <td><asp:DropDownList runat="server" CssClass="DropDownList" ID="ddlUnitAreal" ValidationGroup="Input" MaxLength = "5"
                        Width = "100px" Height="18px">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>Unit Norma</td>
                <td>:</td>
                <td><asp:DropDownList runat="server" CssClass="DropDownList" ID="ddlUnit" ValidationGroup="Input" MaxLength = "5"
                        Width = "100px" Height="18px" AutoPostBack="true">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>Unit Dosis</td>
                <td>:</td>
                <td><asp:DropDownList runat="server" CssClass="DropDownList" ID="ddlUnitConvert" ValidationGroup="Input" MaxLength = "5"
                        Width = "100px" Height="18px">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>Acc Expanse</td>
                <td>:</td>
                <td><asp:TextBox runat="server" CssClass="TextBox" ID="tbAccExpense" ValidationGroup="Input" 
                        Width="127px" AutoPostBack="True"/>
                    <asp:TextBox runat="server" MaxLength="12" CssClass="TextBoxR" 
                        ID="tbAccExpenseName" Width="250px" ReadOnly="True"/>
                    <asp:Button class="btngo" runat="server" ID="btnAccExpense" Text="..." ValidationGroup="Input" /> 
                    <asp:Label ID="Label1" runat="server" ForeColor="Red" Text="*"></asp:Label>
                </td>
            </tr>
             <tr>
                <td>Acc Asset</td>
                <td>:</td>
                <td><asp:TextBox runat="server" CssClass="TextBox" ID="tbAccAsset" ValidationGroup="Input" 
                        Width="127px" AutoPostBack="True"/>
                    <asp:TextBox runat="server" MaxLength="12" CssClass="TextBoxR" 
                        ID="tbAccAssetName" Width="250px" ReadOnly="True"/>
                    <asp:Button class="btngo" runat="server" ID="btnAccAsset" Text="..." ValidationGroup="Input" /> 
                    <asp:Label ID="Label2" runat="server" ForeColor="Red" Text="*"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>CIP</td>
                <td>:</td>
                <td><asp:DropDownList runat="server" CssClass="DropDownList" ID="ddlFgCIP" ValidationGroup="Input" 
                        Width="80px">
                    <asp:ListItem>Y</asp:ListItem>
                    <asp:ListItem Selected="True">N</asp:ListItem>
                    </asp:DropDownList> 
                </td>
             </tr>
              <tr>       
                <td>
                    Used Material
                </td>    
                <td>:</td>                    
                <td>
                    <asp:DropDownList runat="server" CssClass="DropDownList" ID="ddlFgUsedMaterial" ValidationGroup="Input" 
                        Width="80px">
                    <asp:ListItem>Y</asp:ListItem>
                    <asp:ListItem Selected="True">N</asp:ListItem>
                    </asp:DropDownList>     
                </td>
            </tr>
             <tr>       
                <td>
                    Used Machine
                </td>    
                <td>:</td>                    
                <td>
                    <asp:DropDownList runat="server" CssClass="DropDownList" ID="ddlFgUsedMachine" ValidationGroup="Input" 
                        Width="80px">
                    <asp:ListItem>Y</asp:ListItem>
                    <asp:ListItem Selected="True">N</asp:ListItem>
                    </asp:DropDownList>     
                </td>
            </tr>
            <tr>       
                <td>
                    Active
                </td>    
                <td>:</td>                    
                <td>
                    <asp:DropDownList runat="server" CssClass="DropDownList" ID="ddlActive" ValidationGroup="Input" 
                        Width="80px">
                    <asp:ListItem Selected="True">Y</asp:ListItem>
                    <asp:ListItem>N</asp:ListItem>
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
                    <asp:Button ID="btnHome" runat="server" class="bitbtndt btnback" Text="Home" />									                                                           
                </td>
            </tr>
        </table>
      </asp:Panel>              
     <asp:Label ID="lstatus" ForeColor="red" runat="server"></asp:Label>        
    </div>
    </form>
</body>
</html>
