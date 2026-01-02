<%@ Page Language="VB" AutoEventWireup="False" CodeFile="MsBlock.aspx.vb" Inherits="MsBlock_MsBlock" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<%@ Register Assembly="BasicFrame.WebControls.BasicDatePicker" Namespace="BasicFrame.WebControls"
    TagPrefix="BDP" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Block File</title>
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
     <div class="H1">Block File</div>
     <hr style="color:Blue" />
     <asp:Panel runat="server" ID="pnlHd">
      <table>
        <tr>
            <td style="width:100px;text-align:right" >Quick Search :</td>
            <td><asp:TextBox runat="server" ID ="tbFilter" CssClass="TextBox" /> 
                <asp:DropDownList runat="server" ID="ddlField" CssClass="DropDownList" > 
                    <asp:ListItem Selected="true" Text="Block Code" Value="BlockCode"></asp:ListItem>
                    <asp:ListItem Text="Block Name" Value="BlockName"></asp:ListItem>                   
                    <asp:ListItem Text="Est Start Taman" Value="EstStartPlant" ></asp:ListItem>
                    <asp:ListItem Text="Start Tanam" Value="StartPlant"></asp:ListItem>
                    <asp:ListItem Text="Block No" Value="BlockNo"></asp:ListItem>
                    <asp:ListItem Text="Areal" Value="Areal"></asp:ListItem>
                    <asp:ListItem Text="Luas Area" Value="Area"></asp:ListItem>
                    <asp:ListItem Text="Land Type" Value="LandType"></asp:ListItem>
                    <asp:ListItem Text="Land Scape" Value="LandScape"></asp:ListItem>
                    <asp:ListItem Text="Max Capacity" Value="MaxCap"></asp:ListItem>
                    <asp:ListItem Text="Status Tanam" Value="StatusTanam"></asp:ListItem>
                    <asp:ListItem Text="Qty Tanam" Value="QtyTanam"></asp:ListItem>
                    <asp:ListItem Text="SPH" Value="SPH"></asp:ListItem>
                    <asp:ListItem Text="Panen" Value="FgPanen"></asp:ListItem>
                    <asp:ListItem Text="KSU Block" Value="KSUBlockName"></asp:ListItem>
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
                    <asp:ListItem Selected="true" Text="Block Code" Value="BlockCode"></asp:ListItem>
                    <asp:ListItem Text="Block Name" Value="BlockName"></asp:ListItem>                   
                    <asp:ListItem Text="Est Start Taman" Value="EstStartPlant"></asp:ListItem>
                    <asp:ListItem Text="Start Tanam" Value="StartPlant"></asp:ListItem>
                    <asp:ListItem Text="Block No" Value="BlockNo"></asp:ListItem>
                    <asp:ListItem Text="Areal" Value="Areal"></asp:ListItem>
                    <asp:ListItem Text="Luas Area" Value="Area"></asp:ListItem>
                    <asp:ListItem Text="Land Type" Value="LandType"></asp:ListItem>
                    <asp:ListItem Text="Land Scape" Value="LandScape"></asp:ListItem>
                    <asp:ListItem Text="Max Capacity" Value="MaxCap"></asp:ListItem>
                    <asp:ListItem Text="Status Tanam" Value="StatusTanam"></asp:ListItem>
                    <asp:ListItem Text="Qty Tanam" Value="QtyTanam"></asp:ListItem>
                    <asp:ListItem Text="SPH" Value="SPH"></asp:ListItem>
                    <asp:ListItem Text="Panen" Value="FgPanen"></asp:ListItem>
                    <asp:ListItem Text="KSU Block" Value="KSUBlock"></asp:ListItem>
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
                                          <asp:ListItem Text="Delete" />
                                      </asp:DropDownList>
                                      <asp:Button class="btngo" runat="server" ID="btnGo" Text="G" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" CommandName="Go" />                                      
                                  </ItemTemplate>
                                  <HeaderStyle Width="110px" />
                            </asp:TemplateField>
				            <asp:BoundField DataField="BlockCode" HeaderText="Block Code" HeaderStyle-Width="140" SortExpression="BlockCode"/>
							<asp:BoundField DataField="BlockName" HeaderText="Block Name" HeaderStyle-Width="300" SortExpression="BlockName"/>
							<asp:BoundField DataField="EstStartPlant" HeaderText="Est Start Tanam " SortExpression="EstStartPlant" DataFormatString="{0:dd MMM yyyy}"/>
							<asp:BoundField DataField="StartPlant" HeaderText="Start Tanam" SortExpression="StartPlant" DataFormatString="{0:dd MMM yyyy}"/>	
							<asp:BoundField DataField="BlockNo" HeaderText="Block No" HeaderStyle-Width="200" SortExpression="BlockNo"/>						
							<asp:BoundField DataField="ArealName" HeaderText="Areal" SortExpression="Areal" ItemStyle-HorizontalAlign="Center"/>
							<asp:BoundField DataField="Area" HeaderText="Luas Area" HeaderStyle-Width="120" SortExpression="Area"/>
						    <asp:BoundField DataField="LandTypeName" HeaderText="Land Type" HeaderStyle-Width="300" SortExpression="LandType"/>
						    <asp:BoundField DataField="LandScapeName" HeaderText="Land Scape" HeaderStyle-Width="300" SortExpression="LandScape"/>
							<asp:BoundField DataField="MaxCap" HeaderText="Max Capacity" HeaderStyle-Width="120" SortExpression="MaxCap"/>
							<asp:BoundField DataField="StatusTanam" HeaderText="Status Tanam" HeaderStyle-Width="300" SortExpression="StatusTanam"/>
							<asp:BoundField DataField="QtyTanam" HeaderText="Qtr Taman" HeaderStyle-Width="300" SortExpression="QtyTanam" DataFormatString = "{0:#.##.##}"/>
							<asp:BoundField DataField="SPH" HeaderText="SPH" HeaderStyle-Width="300" SortExpression="SPH" DataFormatString = "{0:#,##0.##}"/>
							<asp:BoundField DataField="FgPanen" HeaderText="Panen" HeaderStyle-Width="70" SortExpression="FgPanen"/>
							<asp:BoundField DataField="KSUBlock" HeaderText="KSU Block" HeaderStyle-Width="70" SortExpression="KSUBlock"/>				
							
    					</Columns>
        </asp:GridView>
        <asp:Button class="bitbtn btnadd" runat="server" ID="btnAdd2" Text="Add" 
             Visible="False" />									
      </asp:Panel>
      <asp:Panel runat="server" ID="pnlInput" Visible="false">
        <table>
            <tr>
                <td>Block Code</td>
                <td>:</td> 
                <td><asp:TextBox runat="server" CssClass="TextBox" ID="tbCode" MaxLength = "5"/>
                <asp:Label ID="Label4" runat="server" ForeColor="Red" Text="*"></asp:Label></td>
            </tr>
            <tr>
                <td>Block Name</td>
                <td>:</td>
                <td><asp:TextBox runat="server" MaxLength="60" CssClass="TextBox" ID="tbName" ValidationGroup="Input" Width="300px"/>
                <asp:Label ID="Label1" runat="server" ForeColor="Red" Text="*"></asp:Label></td>
            </tr>
            <tr>
                <td>Est Start Tanam</td>
                <td>:</td>
                <td> <BDP:BasicDatePicker ID="tbEstStartPlant" runat="server" DateFormat="dd MMM yyyy" 
                            ReadOnly = "true" ValidationGroup="Input"
                            ButtonImageHeight="19px" ButtonImageWidth="20px" 
                            DisplayType="TextBoxAndImage" 
                            TextBoxStyle-CssClass="TextDate" 
                            ShowNoneButton="False"><TextBoxStyle CssClass="TextDate" /></BDP:BasicDatePicker> 
                <asp:Label ID="Label2" runat="server" ForeColor="Red" Text="*"></asp:Label></td>
            </tr>
            <tr>
                <td>Start Tanam</td>
                <td>:</td>
                <td> <BDP:BasicDatePicker ID="tbStartPlant" runat="server" DateFormat="dd MMM yyyy" 
                            ReadOnly = "true" ValidationGroup="Input"
                            ButtonImageHeight="19px" ButtonImageWidth="20px" 
                            DisplayType="TextBoxAndImage" 
                            TextBoxStyle-CssClass="TextDate"
                            ShowNoneButton="False"><TextBoxStyle CssClass="TextDate" /></BDP:BasicDatePicker> 
                </td>
            </tr>
            <tr>
                <td>Block No</td>
                <td>:</td>
                <td><asp:TextBox runat="server" MaxLength="4" CssClass="TextBox" ID="tbBlockNo" ValidationGroup="Input" Width="180px"/>
                <asp:Label ID="Label3" runat="server" ForeColor="Red" Text="*"></asp:Label></td>
            </tr>
            <tr>
                <td>Areal</td>
                <td>:</td>
                <td><asp:DropDownList runat="server" CssClass="DropDownList" ID="ddlAreal" ValidationGroup="Input" MaxLength = "20"
                        Width = "200px" Height="18px">
                    </asp:DropDownList>
               &nbsp;Ha <asp:Label ID="Label5" runat="server" ForeColor="Red" Text="*"></asp:Label></td>
            </tr>           
            <tr>
                <td>Luas Area </td>
                <td>:</td>
                <td><asp:TextBox runat="server" MaxLength="5" CssClass="TextBox" ID="tbArea" ValidationGroup="Input" Width="80px"/>
                <asp:Label ID="Label6" runat="server" ForeColor="Red" Text="*"></asp:Label></td>
            </tr>
            <tr>
                <td>Land Type</td>
                <td>:</td>
                <td><asp:DropDownList runat="server" CssClass="DropDownList" ID="ddlLandType" ValidationGroup="Input" MaxLength = "20"
                        Width = "200px" Height="18px">
                    </asp:DropDownList>
                <asp:Label ID="Label7" runat="server" ForeColor="Red" Text="*"></asp:Label></td>
            </tr>
            <tr>
                <td>Land Scape</td>
                <td>:</td>
                <td><asp:DropDownList runat="server" CssClass="DropDownList" ID="ddlLandScape" ValidationGroup="Input" MaxLength = "20"
                        Width = "200px" Height="18px" >
                    </asp:DropDownList>
               <asp:Label ID="Label8" runat="server" ForeColor="Red" Text="*"></asp:Label> </td>
            </tr>
            <tr>
                <td>Max Capacity </td>
                <td>:</td>
                <td><asp:TextBox runat="server" MaxLength="5" CssClass="TextBox" ID="tbMaxCap" ValidationGroup="Input" Width="80px"/>
                &nbsp;Pokok<asp:Label ID="Label9" runat="server" ForeColor="Red" Text="*"></asp:Label></td>
               </tr>
             <tr>
                <td>Status Tanam </td>
                <td>:</td>
                <td><asp:TextBox runat="server" CssClass="TextBoxR" ID="tbStatusTanam" ValidationGroup="Input" Width="200px" Enabled="False"  ReadOnly="True" /></td>
            </tr>
             <tr>
                <td>Qty Tanam </td>
                <td>:</td>
                <td><asp:TextBox runat="server" CssClass="TextBoxR" ID="tbQtyTanam" ValidationGroup="Input" Width="200px" Enabled="False" ReadOnly="True" /></td>
            </tr>
             <tr>
                <td>SPH </td>
                <td>:</td>
                <td><asp:TextBox runat="server" CssClass="TextBoxR" ID="tbSPH" ValidationGroup="Input" Width="200px" Enabled="False" ReadOnly="True" /></td>
            </tr>
             <tr>
                <td>Panen </td>
                <td>:</td>
                <td><asp:DropDownList runat="server" CssClass="DropDownList" ID="ddlFgPanen" ValidationGroup="Input" MaxLength = "20"
                        Width = "40px" Height="18px" AutoPostBack="true">
                        <asp:ListItem Selected="True">Y</asp:ListItem>
                        <asp:ListItem>N</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
            <td>KSU Block</td>
            <td>:</td>
                <td><asp:DropDownList runat="server" CssClass="DropDownList" ID="ddlKSUBlock" ValidationGroup="Input" MaxLength = "20"
                        Width = "100px" Height="18px">
                    </asp:DropDownList></td>
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
