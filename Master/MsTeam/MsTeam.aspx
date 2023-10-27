<%@ Page Language="VB" AutoEventWireup="false" CodeFile="MsTeam.aspx.vb" Inherits="MsTeam_MsTeam" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Team File</title>
    <script src="../../Function/OpenDlg.js" type="text/javascript"></script>
    <script src="../../Function/Function.JS" type="text/javascript"></script>
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div class="Content">
     <div class="H1">Team File</div>
     <hr style="color:Blue" />
     <asp:Panel runat="server" ID="pnlHd">
      <table>
        <tr>
            <td style="width:100px;text-align:right" >Quick Search :</td>
            <td><asp:TextBox runat="server" ID ="tbFilter" CssClass="TextBox" /> 
                <asp:DropDownList runat="server" ID="ddlField" CssClass="DropDownList" > 
                    <asp:ListItem Selected="true" Text="Team Code" Value="TeamCode"></asp:ListItem>
                    <asp:ListItem Text="Team Name" Value="TeamName"></asp:ListItem>                   
                    <asp:ListItem Text="Team Type" Value="TeamType"></asp:ListItem>
                    <asp:ListItem Text="DivType" Value="DivType"></asp:ListItem>
                    <asp:ListItem Text="Supplier" Value="Supplier"></asp:ListItem>
                    <asp:ListItem Text="Division" Value="Division"></asp:ListItem>
                    <asp:ListItem Text="Ketua Team" Value="KetuaTeam"></asp:ListItem>
                    <asp:ListItem Text="Batch" Value="FgBatch"></asp:ListItem>
                    <asp:ListItem Text="Land" Value="FgLand"></asp:ListItem>
                    <asp:ListItem Text="Panen" Value="FgPanen"></asp:ListItem>
                    <asp:ListItem Text="Blok" Value="FgBlok"></asp:ListItem>
                    <asp:ListItem Text="Acvtive" Value="FgActive"></asp:ListItem>
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
                    <asp:ListItem Selected="true" Text="Team Code" Value="TeamCode"></asp:ListItem>
                    <asp:ListItem Text="Team Name" Value="TeamName"></asp:ListItem>                   
                    <asp:ListItem Text="Team Type" Value="TeamType"></asp:ListItem>
                    <asp:ListItem Text="DivType" Value="DivType"></asp:ListItem>
                    <asp:ListItem Text="Supplier" Value="Supplier"></asp:ListItem>
                    <asp:ListItem Text="Division" Value="Division"></asp:ListItem>
                    <asp:ListItem Text="Ketua Team" Value="KetuaTeam"></asp:ListItem>
                    <asp:ListItem Text="Batch" Value="FgBatch"></asp:ListItem>
                    <asp:ListItem Text="Land" Value="FgLand"></asp:ListItem>
                    <asp:ListItem Text="Panen" Value="FgPanen"></asp:ListItem>
                    <asp:ListItem Text="Blok" Value="FgBlok"></asp:ListItem>
                    <asp:ListItem Text="Acvtive" Value="FgActive"></asp:ListItem>
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
				            <asp:BoundField DataField="TeamCode" HeaderText="Team Code" HeaderStyle-Width="140" SortExpression="TeamCode"/>
							<asp:BoundField DataField="TeamName" HeaderText="Team Name" HeaderStyle-Width="300" SortExpression="TeamName"/>
							<asp:BoundField DataField="TeamType" HeaderText="Team Type " SortExpression="TeamType"/>
							<asp:BoundField DataField="SupplierName" HeaderText="Supplier" SortExpression="Supplier"/>							
							<asp:BoundField DataField="DivType" HeaderText="Division Type" SortExpression="DivType" ItemStyle-HorizontalAlign="Center"/>
							<asp:BoundField DataField="DivisionName" HeaderText="Division" SortExpression="Division"/>
							<asp:BoundField DataField="TotalEmp" HeaderText="Total Employee" HeaderStyle-Width="20" SortExpression="TotalEmp"/>
							<asp:BoundField DataField="TotalNonEmp" HeaderText="Total Non Employee" HeaderStyle-Width="20" SortExpression="TotalNonEmp"/>
							<asp:BoundField DataField="TotalMember" HeaderText="TotalMember" HeaderStyle-Width="20" SortExpression="TotalMember"/>
							<asp:BoundField DataField="KetuaTeam" HeaderText="Ketua Team" HeaderStyle-Width="120" SortExpression="KetuaTeam"/>				
							<asp:BoundField DataField="FgActive" HeaderText="Active" HeaderStyle-Width="20" SortExpression="FgActive"/>		
							<asp:BoundField DataField="MaxTotalBottom" HeaderText="MaxTotalBottom" HeaderStyle-Width="20" SortExpression="MaxTotalBottom" DataFormatString ="{0:#,##.##}"/>	
						    <asp:BoundField DataField="MaxTotalTop" HeaderText="MaxTotalTop" HeaderStyle-Width="20" SortExpression="MaxTotalTop" DataFormatString ="{0:#,##.##}"/>	
							<asp:BoundField DataField="MaxHKBottom" HeaderText="MaxHKBottom" HeaderStyle-Width="20" SortExpression="MaxHKBottom" DataFormatString ="{0:#,##.##}"/>	
						    <asp:BoundField DataField="MaxHKTop" HeaderText="MaxHKTop" HeaderStyle-Width="20" SortExpression="MaxHKTop" DataFormatString ="{0:#,##.##}"/>	
							<asp:BoundField DataField="PremiHadir" HeaderText="PremiHadir" HeaderStyle-Width="20" SortExpression="PremiHadir" DataFormatString ="{0:#,##.##}"/>	
							<asp:BoundField DataField="PremiNatura" HeaderText="PremiNatura" HeaderStyle-Width="20" SortExpression="PremiNatura" DataFormatString ="{0:#,##.##}"/>
							<asp:BoundField DataField="PremiOther" HeaderText="PremiOther" HeaderStyle-Width="20" SortExpression="PremiOther" DataFormatString ="{0:#,##.##}"/>	
							<asp:BoundField DataField="FgLand" HeaderText="Land" HeaderStyle-Width="20" SortExpression="FgLand"/>	
							<asp:BoundField DataField="FgBatch" HeaderText="Batch" HeaderStyle-Width="20" SortExpression="FgBatch"/>	
							<asp:BoundField DataField="FgBlock" HeaderText="Block" HeaderStyle-Width="20" SortExpression="FgBlock"/>
							<asp:BoundField DataField="FgPanen" HeaderText="Panen" HeaderStyle-Width="20" SortExpression="FgPanen"/>																							
    					</Columns>
        </asp:GridView>
        <asp:Button class="bitbtn btnadd" runat="server" ID="btnAdd2" Text="Add" 
             Visible="False" />									
      </asp:Panel>
      <asp:Panel runat="server" ID="pnlInput" Visible="false">
        <table>
            <tr>
                <td>Team Code</td>
                <td>:</td> 
                <td><asp:TextBox runat="server" CssClass="TextBox" ID="tbCode" MaxLength = "5"/></td>
            </tr>
            <tr>
                <td>Team Name</td>
                <td>:</td>
                <td><asp:TextBox runat="server" MaxLength="60" CssClass="TextBox" ID="tbName" ValidationGroup="Input" Width="300px"/></td>
            </tr>
            <tr>
                <td>Team Type</td>
                <td>:</td>
                <td><asp:DropDownList runat="server" CssClass="DropDownList" ID="ddlTeamType" ValidationGroup="Input" MaxLength = "3"
                        Width = "250px" Height="18px">
                        <asp:ListItem Selected="True">Borongan</asp:ListItem>
                        <asp:ListItem>Hari</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
             <tr>
                <td>Supplier</td>
                <td>:</td>
                <td><asp:TextBox runat="server" CssClass="TextBox" ID="tbSupplier" ValidationGroup="Input" 
                        Width="127px"/>
                    <asp:TextBox runat="server" MaxLength="12" CssClass="TextBoxR" 
                        ID="tbSupplierName" Width="250px" ReadOnly="True" AutoPostBack = "true"/>
                    <asp:Button class="btngo" runat="server" ID="btnSupplier" Text="..." ValidationGroup="Input" /> 
                </td>
            </tr>
            <tr>
                <td>Division Type</td>
                <td>:</td>
                <td><asp:DropDownList runat="server" CssClass="DropDownList" ID="ddlDivType" ValidationGroup="Input" MaxLength = "3"
                        Width = "250px" Height="18px" AutoPostBack="true">
                        <asp:ListItem Selected ="True"> Division</asp:ListItem> 
                        <asp:ListItem>Estate</asp:ListItem>
                        <asp:ListItem>Areal</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>Division</td>
                <td>:</td>
                <td><asp:DropDownList runat="server" CssClass="DropDownList" ID="ddlDivision" ValidationGroup="Input" MaxLength = "3"
                        Width = "250px" Height="18px">
                    </asp:DropDownList>
                    </td>
            </tr>
            <tr>
                <td>Total Employee</td>
                <td>:</td>
                <td><asp:TextBox runat="server" CssClass="TextBox" ID="tbTotalEmp" MaxLength = "5"/>
                </td>
                <td>Premi Kehadiran</td>
                <td>:</td>
                <td><asp:TextBox runat="server" CssClass="TextBox" ID="tbPremiHadir" MaxLength = "5"/>
                </td>
            </tr>
            <tr>
                <td>Total Non Employee</td>
                <td>:</td>
                <td><asp:TextBox runat="server" CssClass="TextBox" ID="tbTotalNonEmp" MaxLength = "5"/>
                </td>
                <td>Premi Kesehatan</td>
                <td>:</td>
                <td><asp:TextBox runat="server" CssClass="TextBox" ID="tbPremiNatura" MaxLength = "5"/>
                </td>
            </tr>
            <tr>
                <td>Total Member</td>
                <td>:</td>
                <td><asp:TextBox runat="server" CssClass="TextBox" ID="tbTotalMember" MaxLength = "5"/>
                </td>
                <td>Premi Lain-Lain</td>
                <td>:</td>
                <td><asp:TextBox runat="server" CssClass="TextBox" ID="tbPremiOther" MaxLength = "5"/>
                </td>
            </tr>
            <tr>
                <td>Ketua Team</td>
                <td>:</td>
                <td><asp:TextBox runat="server" CssClass="TextBox" ID="tbKetuaTeam" ValidationGroup="Input" 
                        Width="250px"/>
                </td>
            </tr>
             <tr>
                <td>Section</td>
                <td>:</td>
                <td>
                    <asp:CheckBox ID="FgLand" runat="server" Text="Land"/>
                    <asp:CheckBox ID="FgBatch" runat="server" Text="Batch"/>
                    <asp:CheckBox ID="FgBlock" runat="server" Text="Block"/>
                    <asp:CheckBox ID="FgPanen" runat="server" Text="Panen"/>
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
                <td>
                    BPJS
                </td>    
                <td>:</td>                    
                <td>
                    <asp:DropDownList runat="server" CssClass="DropDownList" ID="ddlBpjs" ValidationGroup="Input" 
                        Width="80px">
                    <asp:ListItem Selected="True">N</asp:ListItem>
                    <asp:ListItem>Y</asp:ListItem>
                    </asp:DropDownList>     
                </td>
            </tr>
            <tr>       
                <td>
                    Inap
                </td>    
                <td>:</td>                    
                <td>
                    <asp:DropDownList runat="server" CssClass="DropDownList" ID="ddlInap" ValidationGroup="Input" 
                        Width="80px">
                    <asp:ListItem Selected="True">D</asp:ListItem>
                    <asp:ListItem>L</asp:ListItem>
                    </asp:DropDownList>     
                </td>
            </tr>
            <tr>
                <td>Max WO Bottom</td>
                <td>:</td>
                <td><asp:TextBox runat="server" CssClass="TextBox" ID="tbMaxTotalBottom" MaxLength = "20"/>(Rp)
                </td>
                <td></td>
                <td></td>
                <td><asp:TextBox runat="server" CssClass="TextBox" ID="tbMaxHKBottom" MaxLength = "5"/>HK/HM
                </td>
            </tr>
             <tr>
                <td>Max WO Top</td>
                <td>:</td>
                <td><asp:TextBox runat="server" CssClass="TextBox" ID="tbMaxTotalTop" MaxLength = "20"/>(Rp)
                </td>
                <td></td>
                <td></td>
                <td><asp:TextBox runat="server" CssClass="TextBox" ID="tbMaxHKTop" MaxLength = "5"/>HK/HM
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
