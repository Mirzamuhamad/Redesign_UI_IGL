<%@ Page Language="VB" AutoEventWireup="false" CodeFile="MsSeller.aspx.vb" Inherits="MsSeller_MsSeller" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Seller File</title>
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
     <div class="H1">Seller File</div>
     <hr style="color:Blue" />
     <asp:Panel runat="server" ID="pnlHd">
      <table>
        <tr>
            <td style="width:100px;text-align:right" >Quick Search :</td>
            <td><asp:TextBox runat="server" ID ="tbFilter" CssClass="TextBox" /> 
                <asp:DropDownList runat="server" ID="ddlField" CssClass="DropDownList" > 
                    <asp:ListItem Selected="true" Text="Seller Code" Value="SellCode"></asp:ListItem>
                    <asp:ListItem Text="Seller Name" Value="SellName"></asp:ListItem>                   
                    <asp:ListItem Text="Gender" Value="Gender"></asp:ListItem>
                    <asp:ListItem Text="Type ID" Value="TypeID"></asp:ListItem>
                    <asp:ListItem Text="Seller ID" Value="SellID"></asp:ListItem>
                    <asp:ListItem Text="Address 1" Value="Address1"></asp:ListItem>
                    <asp:ListItem Text="Address 2" Value="Address2"></asp:ListItem>
                    <asp:ListItem Text="City" Value="City"></asp:ListItem>
                    <asp:ListItem Text="ZipCode" Value="ZipCode"></asp:ListItem>
                    <asp:ListItem Text="Phone" Value="Phone"></asp:ListItem>
                    <asp:ListItem Text="Email" Value="Email"></asp:ListItem>
                    <asp:ListItem Text="NPWP" Value="NPWP"></asp:ListItem>
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
                    <asp:ListItem Selected="true" Text="Seller Code" Value="SellCode"></asp:ListItem>
                    <asp:ListItem Text="Seller Name" Value="SellName"></asp:ListItem>                   
                    <asp:ListItem Text="Gender" Value="Gender"></asp:ListItem>
                    <asp:ListItem Text="Type ID" Value="TypeID"></asp:ListItem>
                    <asp:ListItem Text="Seller ID" Value="SellID"></asp:ListItem>
                    <asp:ListItem Text="Address 1" Value="Address1"></asp:ListItem>
                    <asp:ListItem Text="Address 2" Value="Address2"></asp:ListItem>
                    <asp:ListItem Text="City" Value="City"></asp:ListItem>
                    <asp:ListItem Text="ZipCode" Value="ZipCode"></asp:ListItem>
                    <asp:ListItem Text="Phone" Value="Phone"></asp:ListItem>
                    <asp:ListItem Text="Email" Value="Email"></asp:ListItem>
                    <asp:ListItem Text="NPWP" Value="NPWP"></asp:ListItem>
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
				            <asp:BoundField DataField="SellCode" HeaderText="Seller Code" HeaderStyle-Width="140" SortExpression="SellCode"/>
							<asp:BoundField DataField="SellName" HeaderText="Seller Name" HeaderStyle-Width="300" SortExpression="SellName"/>
                            <asp:BoundField DataField="Gender" HeaderText="Gender" SortExpression="Gender"/>
							<asp:BoundField DataField="TypeID" HeaderText="Type ID " SortExpression="TypeID"/>	
                            <asp:BoundField DataField="SellID" HeaderText="ID No" SortExpression="SellID"/>
                            <asp:BoundField DataField="NoKK" HeaderText="KK No " SortExpression="NoKK"/>
                            <asp:BoundField DataField="Address1" HeaderText="Address" SortExpression="Address1"/> 
                            <asp:BoundField DataField="Address2" HeaderText="Address 2" SortExpression="Address2"/> 
                            <asp:BoundField DataField="Desa" HeaderText="Desa" SortExpression="Desa"/>  
                            <asp:BoundField DataField="Kec" HeaderText="Kecamatan" SortExpression="Kec"/>  
                            <asp:BoundField DataField="Kab" HeaderText="Kabupaten" SortExpression="Kab"/>   
                            <asp:BoundField DataField="City" HeaderText="City" SortExpression="City"/>  
                            <asp:BoundField DataField="ZipCode" HeaderText="ZipCode" SortExpression="ZipCode"/>  
                            <asp:BoundField DataField="Phone" HeaderText="Phone" SortExpression="Phone"/>  
                            <asp:BoundField DataField="Email" HeaderText="Email" SortExpression="Email"/>  
                            <asp:BoundField DataField="NPWP" HeaderText="NPWP" SortExpression="NPWP"/>  		
							
    					</Columns>
        </asp:GridView>
        <asp:Button class="bitbtn btnadd" runat="server" ID="btnAdd2" Text="Add" 
             Visible="False" />									
      </asp:Panel>
      <asp:Panel runat="server" ID="pnlInput" Visible="false">
        <table>
            <tr>
                <td>Seller Code</td>
                <td>:</td> 
                <td><asp:TextBox runat="server" CssClass="TextBox" AutoPostBack="true" ID="tbCode" MaxLength = "5"/></td>
            </tr>
            <tr>
                <td>Seller Name</td>
                <td>:</td>
                <td><asp:TextBox runat="server" MaxLength="60" CssClass="TextBox" ID="tbName" ValidationGroup="Input" Width="300px"/></td>
            </tr>
            <tr>
                <td>Gender</td>
                <td>:</td>
                <td><asp:DropDownList runat="server" CssClass="DropDownList" Width="306px" ID="ddlGender" ValidationGroup="Input">
                    <asp:ListItem Selected="True"></asp:ListItem>
                    <asp:ListItem>Laki - Laki</asp:ListItem>
                    <asp:ListItem>Perempuan</asp:ListItem>
                    </asp:DropDownList> 
                </td>
             </tr>

             <tr>
                <td>Type ID</td>
                <td>:</td>
                <td><asp:DropDownList runat="server" CssClass="DropDownList" Width="306px" ID="ddlTypeID" ValidationGroup="Input">
                    <asp:ListItem Selected="True">KTP</asp:ListItem>
                    <asp:ListItem>SIM C</asp:ListItem>
                    <asp:ListItem>SIM A</asp:ListItem>
                    <asp:ListItem>SIM B</asp:ListItem>
                    <asp:ListItem>Passport</asp:ListItem>
                    </asp:DropDownList> 
                </td>
             </tr>

            <tr>
                <td>ID No</td>
                <td>:</td>
                <td><asp:TextBox runat="server" MaxLength="60" CssClass="TextBox" ID="tbSellerID" ValidationGroup="Input" Width="300px"/></td>
            </tr>

            <tr>
                <td>KK No</td>
                <td>:</td>
                <td><asp:TextBox runat="server" MaxLength="60" CssClass="TextBox" ID="TbKk" ValidationGroup="Input" Width="300px"/></td>
            </tr>

            <tr>
                <td>Address</td>
                <td>:</td>
                <td><asp:TextBox runat="server" MaxLength="200" CssClass="TextBox" ID="tbAddress" ValidationGroup="Input" Width="300px"/></td>
            </tr>

             <tr>
                <td>Address 2</td>
                <td>:</td>
                <td><asp:TextBox runat="server" MaxLength="200" CssClass="TextBox" ID="tbAddress2" ValidationGroup="Input" Width="300px"/></td>
            </tr>

            <tr>
                <td>Desa</td>
                <td>:</td>
                <td><asp:TextBox runat="server" MaxLength="60" CssClass="TextBox" ID="tbDesa" ValidationGroup="Input" Width="300px"/></td>
            </tr>

            <tr>
                <td>Kecamatan</td>
                <td>:</td>
                <td><asp:TextBox runat="server" MaxLength="60" CssClass="TextBox" ID="tbKec" ValidationGroup="Input" Width="300px"/></td>
            </tr>

            <tr>
                <td>Kabupaten</td>
                <td>:</td>
                <td><asp:TextBox runat="server" MaxLength="60" CssClass="TextBox" ID="TbKab" ValidationGroup="Input" Width="300px"/></td>
            </tr>

            <tr>
                <td>Kota</td>
                <td>:</td>
                <td><asp:TextBox runat="server" MaxLength="60" CssClass="TextBox" ID="tbCity" ValidationGroup="Input" Width="300px"/></td>
            </tr>

            <tr>
                <td>ZipCode</td>
                <td>:</td>
                <td><asp:TextBox runat="server" MaxLength="5" CssClass="TextBox" ID="tbZipCode" ValidationGroup="Input" Width="300px"/></td>
            </tr>

            <tr>
                <td>Phone</td>
                <td>:</td>
                <td><asp:TextBox runat="server" MaxLength="13" CssClass="TextBox" ID="tbPhone" ValidationGroup="Input" Width="300px"/></td>
            </tr>

            <tr>
                <td>Email</td>
                <td>:</td>
                <td><asp:TextBox runat="server" MaxLength="60" CssClass="TextBox" ID="tbEmail" ValidationGroup="Input" Width="300px"/></td>
            </tr>

            <tr>
                <td>NPWP</td>
                <td>:</td>
                <td><asp:TextBox runat="server" MaxLength="20" CssClass="TextBox" ID="tbNpwp" ValidationGroup="Input" Width="300px"/></td>
            </tr>
            <tr>

                        </td> 
                        <td>INVOICE Reg</td>
                        <td>:</td>
                        <td><asp:DropDownList CssClass="DropDownList"  ValidationGroup="Input" Width="50px" runat="server" ID="ddlFgPT" >
                             <asp:ListItem Selected="True">Y</asp:ListItem>                                        
                                        <asp:ListItem>N</asp:ListItem>
                            </asp:DropDownList></td>
            </tr>

            
        </table>
                    <br>
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
               
      </asp:Panel>              
     <asp:Label ID="lstatus" ForeColor="red" runat="server"></asp:Label>        
    </div>
    </form>
</body>
</html>
