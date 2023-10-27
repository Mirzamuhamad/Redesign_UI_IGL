<%@ Page Language="VB" AutoEventWireup="False" CodeFile="MsMemberPlasma.aspx.vb" Inherits="MsMemberPlasma_MsMemberPlasma" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<%@ Register Assembly="BasicFrame.WebControls.BasicDatePicker" Namespace="BasicFrame.WebControls"
    TagPrefix="BDP" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Member Plasma</title>
    <script src="../../Function/OpenDlg.js" type="text/javascript"></script>
    <script src="../../Function/Function.JS" type="text/javascript"></script>
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div class="Content">
     <div class="H1">Member Plasma File</div>
     <hr style="color:Blue" />
     <asp:Panel runat="server" ID="pnlHd">
      <table>
        <tr>
            <td style="width:100px;text-align:right" >Quick Search :</td>
            <td><asp:TextBox runat="server" ID ="tbFilter" CssClass="TextBox" /> 
                <asp:DropDownList runat="server" ID="ddlField" CssClass="DropDownList" > 
                    <asp:ListItem Selected="true" Text="Member ID" Value="MemberID"></asp:ListItem>
                    <asp:ListItem Text="Mamber Name" Value="MemberName"></asp:ListItem>                   
                    <asp:ListItem Text="KTP No" Value="KTPNo"></asp:ListItem>
                    <asp:ListItem Text="KK No" Value="KKNo"></asp:ListItem>
                    <asp:ListItem Text="Village" Value="Village"></asp:ListItem>
                    <asp:ListItem Text="Estate" Value="Estate"></asp:ListItem>
                    <asp:ListItem Text="Luas Area" Value="Area"></asp:ListItem>
                    <asp:ListItem Text="Start Join Date" Value="StartJoin" ></asp:ListItem>
                    <asp:ListItem Text="End Join Date" Value="EndJoin"></asp:ListItem>
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
                   <asp:ListItem Selected="true" Text="Member ID" Value="MemberID"></asp:ListItem>
                    <asp:ListItem Text="Mamber Name" Value="MemberName"></asp:ListItem>                   
                    <asp:ListItem Text="KTP No" Value="KTPNo"></asp:ListItem>
                    <asp:ListItem Text="KK No" Value="KKNo"></asp:ListItem>
                    <asp:ListItem Text="Village" Value="Village"></asp:ListItem>
                    <asp:ListItem Text="Estate" Value="Estate"></asp:ListItem>
                    <asp:ListItem Text="Luas Area" Value="Area"></asp:ListItem>
                    <asp:ListItem Text="Start Join Date" Value="StartJoin"></asp:ListItem>
                    <asp:ListItem Text="End Join Date" Value="EndJoin"></asp:ListItem>
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
				            <asp:BoundField DataField="MemberID" HeaderText="Member ID" HeaderStyle-Width="140" SortExpression="MemberID"/>
							<asp:BoundField DataField="MemberName" HeaderText="MemberName" HeaderStyle-Width="300" SortExpression="MemberName"/>
							<asp:BoundField DataField="KTPNo" HeaderText="KTP No" HeaderStyle-Width="200" SortExpression="KTPNo"/>
							<asp:BoundField DataField="KKNo" HeaderText="KK No" HeaderStyle-Width="200" SortExpression="KKNo"/>						
							<asp:BoundField DataField="VillageName" HeaderText="Village" SortExpression="VillageName" ItemStyle-HorizontalAlign="Center"/>
							<asp:BoundField DataField="EstateName" HeaderText="Estate" SortExpression="EstateName" ItemStyle-HorizontalAlign="Center"/>
							<asp:BoundField DataField="Area" HeaderText="Luas" HeaderStyle-Width="300" SortExpression="Area" DataFormatString ="{0:#,##.##}"/>
							<asp:BoundField DataField="StartJoin" HeaderText="Start Join Date " HeaderStyle-Width="150" SortExpression="StartJoin" DataFormatString = "{0:dd MMM yyyy}"/>
							<asp:BoundField DataField="EndJoin" HeaderText="End Join Date" HeaderStyle-Width="150" SortExpression="StartJoin" DataFormatString = "{0:dd MMM yyyy}"/>					
							<asp:BoundField DataField="FgInti" HeaderText="Fg Inti" SortExpression="FgInti" ItemStyle-HorizontalAlign="Center"/>
    					</Columns>
        </asp:GridView>
        <asp:Button class="bitbtn btnadd" runat="server" ID="btnAdd2" Text="Add" 
             Visible="False" />									
      </asp:Panel>
      <asp:Panel runat="server" ID="pnlInput" Visible="false">
        <table>
            <tr>
                <td>Member ID</td>
                <td>:</td> 
                <td><asp:TextBox runat="server" CssClass="TextBox" ID="tbMemberID" MaxLength = "50"/>
                <asp:Label ID="Label" runat="server" ForeColor="Red" Text="*"></asp:Label></td>
            </tr>
            <tr>
                <td>Member Name</td>
                <td>:</td>
                <td><asp:TextBox runat="server" MaxLength="60" CssClass="TextBox" ID="tbMemberName" ValidationGroup="Input" Width="300px"/>
                <asp:Label ID="Label2" runat="server" ForeColor="Red" Text="*"></asp:Label></td>
            </tr>
            <tr>
                <td>KTP No</td>
                <td>:</td>
                <td><asp:TextBox runat="server" MaxLength="20" CssClass="TextBox" ID="tbKTPNo" ValidationGroup="Input" Width="180px"/>
                <asp:Label ID="label3" runat="server" ForeColor="Red" Text="*"></asp:Label></td>
            </tr>
            <tr>
                <td>
                    KK No</td>
                <td>
                    :</td>
                <td>
                    <asp:TextBox ID="tbKKNo" runat="server" CssClass="TextBox" MaxLength="20" 
                        ValidationGroup="Input" Width="180px" />
                    <asp:Label ID="label4" runat="server" ForeColor="Red" Text="*"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>Village</td>
                <td>:</td>
                <td>
                    <asp:DropDownList ID="ddlVillage" runat="server" CssClass="DropDownList" 
                        Height="18px" MaxLength="20" ValidationGroup="Input" Width="200px">
                    </asp:DropDownList>
                <asp:Label ID="LAbel5" runat="server" ForeColor="Red" Text="*"></asp:Label></td>
            </tr>
            <tr>
                <td>Estate</td>
                <td>:</td>
                <td><asp:DropDownList runat="server" CssClass="DropDownList" ID="ddlEstate" ValidationGroup="Input"
                        Width = "200px" Height="18px" >
                    </asp:DropDownList>
               <asp:Label ID="Label6" runat="server" ForeColor="Red" Text="*"></asp:Label> </td>
            </tr>
			
			 <tr>
				<td>
				Supplier	</td>
				<td>
					:</td>
				<td>
					<asp:TextBox ID="tbSupplier" runat="server" CssClass="TextBox" />
					<asp:TextBox ID="tbSupplierName" runat="server" CssClass="TextBoxR" Enabled="False" 
						Width="150px" />
					<asp:Button ID="btnSupplier" runat="server" Class="btngo" Text="..." />
				</td>
			</tr>
            <tr>
                <td>Luas</td>
                <td>:</td>
                <td><asp:TextBox runat="server" MaxLength="5" CssClass="TextBox" ID="tbLuas" ValidationGroup="Input" Width="80px"/>
                    &nbsp;Ha<asp:Label ID="Label7" runat="server" ForeColor="Red" Text="*"></asp:Label></td>
               </tr>
             <tr>
                 <td>
                     Start Join Date</td>
                 <td>
                     :</td>
                 <td>
                     <BDP:BasicDatePicker ID="tbStartJoin" runat="server" 
                         ButtonImageHeight="19px" ButtonImageWidth="20px" DateFormat="dd MMM yyyy" 
                         DisplayType="TextBoxAndImage" ReadOnly="true" ShowNoneButton="False" 
                         TextBoxStyle-CssClass="TextDate" ValidationGroup="Input">
                         <TextBoxStyle CssClass="TextDate" />
                     </BDP:BasicDatePicker>
                     <asp:Label ID="Label8" runat="server" ForeColor="Red" Text="*"></asp:Label>
                 </td>
            </tr>
            <tr>
                <td>
                    End Join Date</td>
                <td>
                    :</td>
                <td>
                    <BDP:BasicDatePicker ID="tbEndJoin" runat="server" ButtonImageHeight="19px" 
                        ButtonImageWidth="20px" DateFormat="dd MMM yyyy" DisplayType="TextBoxAndImage" 
                        ReadOnly="true" ShowNoneButton="False" TextBoxStyle-CssClass="TextDate" 
                        ValidationGroup="Input">
                        <TextBoxStyle CssClass="TextDate" />
                    </BDP:BasicDatePicker>
                    <asp:Label ID="Label9" runat="server" ForeColor="Red" Text="*"></asp:Label>
                </td>
            </tr>
            

            <tr>
                <td>
                    Fg Inti</td>
                <td>
                    :</td>
                <td>
                    <asp:DropDownList ID="ddlFgInti" runat="server" CssClass="DropDownList" 
                        Height="16px" ValidationGroup="Input" Width="44px">
                        <asp:ListItem Selected="true" Text="N"></asp:ListItem>
                        <asp:ListItem Text="Y"></asp:ListItem>
                    </asp:DropDownList>
                    <asp:Label ID="Label10" runat="server" ForeColor="Red" Text="*"></asp:Label>
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
