<%@ Page Language="VB" AutoEventWireup="false" CodeFile="MsPanen.aspx.vb" Inherits="MsPanen_MsPanen" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Panen File</title>
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
     <div class="H1">Panen File</div>
     <hr style="color:Blue" />
     <asp:Panel runat="server" ID="pnlHd">
      <table>
        <tr>
            <td style="width:100px;text-align:right" >Quick Search :</td>
            <td><asp:TextBox runat="server" ID ="tbFilter" CssClass="TextBox" /> 
                <asp:DropDownList runat="server" ID="ddlField" CssClass="DropDownList" > 
                    <asp:ListItem Selected="true" Text="Panen Code" Value="PanenCode"></asp:ListItem>
                    <asp:ListItem Text="Panen Name" Value="PanenName"></asp:ListItem>                   
                    <asp:ListItem Text="Status Tanam" Value="StatusTanam"></asp:ListItem>
                    <asp:ListItem Text="Start BJR" Value="StartBJR"></asp:ListItem>
                    <asp:ListItem Text="End BJR" Value="EndBJR"></asp:ListItem>
                    <asp:ListItem Text="HK Per Basis" Value="HKPerBasis"></asp:ListItem>
                    <asp:ListItem Text="Normal Janjang" Value="EkuivalenABnormal"></asp:ListItem>
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
                    <asp:ListItem Selected="true" Text="Panen Code" Value="PanenCode"></asp:ListItem>
                    <asp:ListItem Text="Panen Name" Value="PanenName"></asp:ListItem>                   
                    <asp:ListItem Text="Status Tanam" Value="StatusTanam"></asp:ListItem>
                    <asp:ListItem Text="Start BJR" Value="StartBJR"></asp:ListItem>
                    <asp:ListItem Text="End BJR" Value="EndBJR"></asp:ListItem>
                    <asp:ListItem Text="HK PerBasis" Value="HKPerBasis"></asp:ListItem>
                    <asp:ListItem Text="Normal Janjang" Value="EkuivalenABnormal"></asp:ListItem>
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
				            <asp:BoundField DataField="PanenCode" HeaderText="Panen Code" HeaderStyle-Width="140" SortExpression="PanenCode"/>
							<asp:BoundField DataField="PanenName" HeaderText="Panen Name" HeaderStyle-Width="300" SortExpression="PanenName"/>
							<asp:BoundField DataField="StatusTanam" HeaderText="Status Tanam" SortExpression="StatusTanam"/>
							<asp:BoundField DataField="StatusTanam2" HeaderText="Status Tanam" SortExpression="StatusTanam2"/>
							<asp:BoundField DataField="StartBJR" HeaderText="Start BJR" SortExpression="StartBJR" ItemStyle-HorizontalAlign="Right"/>
							<asp:BoundField DataField="EndBJR" HeaderText="End BJR" SortExpression="EndBJR" ItemStyle-HorizontalAlign="Right"/>
							<asp:BoundField DataField="HKPerBasis" HeaderText="HK Per Basis" SortExpression="HKPerBasis" ItemStyle-HorizontalAlign="Center" DataFormatString="{0:#,##.##}"/>
							<asp:BoundField DataField="EkuivalenABnormal" HeaderText="Normal Janjang" SortExpression="EkuivalenABnormal" ItemStyle-HorizontalAlign="Center" DataFormatString="{0:#,##.##}"/>
							
							<asp:BoundField DataField="JanjangperBasis" HeaderText="Janjang PerBasis" SortExpression="JanjangperBasis" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:#,##.##}"/>
							<asp:BoundField DataField="KrgPerBasis" HeaderText="Karung PerBasis" SortExpression="KrgPerBasis" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:#,##0.##}"/>
							<asp:BoundField DataField="JanjangperBasisHt" HeaderText="Janjang Basis Hitam" SortExpression="JanjangperBasisHt" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:#,##0.##}"/>
							<asp:BoundField DataField="KrgPerBasisHt" HeaderText="Karung PerBasis Hitam" SortExpression="KrgPerBasisHt" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:#,##0.##}"/>
							
							<asp:BoundField DataField="PriceNBPerJanjang" HeaderText="Price Non PerJanjang " SortExpression="PriceNBPerJanjang" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:#,##.##}"/>
							<asp:BoundField DataField="PriceNBPerKrg" HeaderText="Price NB Perkarung" SortExpression="PriceNBPerKrg" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:#,##.##}"/>
							<asp:BoundField DataField="PriceNBPerJanjangHt" HeaderText="Price NB Per Janjang Hitam" SortExpression="PriceNBPerJanjangHt" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:#,##.##}"/>
							<asp:BoundField DataField="PriceNBPerKrgHt" HeaderText="Price NB PerKarung Hitam" SortExpression="PriceNBPerKrgHt" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:#,##.##}"/>
							
							<asp:BoundField DataField="PriceUPPerJanjang" HeaderText="Price Over Janjang" SortExpression="PriceUPPerJanjang" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:#,##.##}"/>
							<asp:BoundField DataField="PriceUPPerKrg" HeaderText="Price Over Janjang Hitam" SortExpression="PriceUPPerKrg" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:#,##.##}"/>
							<asp:BoundField DataField="PriceUPPerJanjangHt" HeaderText="Price Over Janjang Hitam" SortExpression="PriceUPPerJanjangHt" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:#,##.##}"/>
							<asp:BoundField DataField="PriceUPPerKrgHt" HeaderText="Price Over PerKarung Hitam" SortExpression="PriceUPPerKrgHt" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:#,##.##}"/>
							
							<asp:BoundField DataField="PremiBasis" HeaderText="Premi Basis" SortExpression="PremiBasis" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:#,##.##}"/>
							<asp:BoundField DataField="PremiBasisKrg" HeaderText="PremiBasis Karung" SortExpression="PremiBasisKrg" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:#,##0.##}"/>
							<asp:BoundField DataField="PremiBasisHt" HeaderText="Premi Basis Hitam" SortExpression="PremiBasisHt" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:#,##0.##}"/>
							<asp:BoundField DataField="PremiBasisKrgHt" HeaderText="Premi Basis Karung Hitam" SortExpression="PremiBasisKrgHt" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:#,##0.##}"/>
																		
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
                <td>Panen Code</td>
                <td>:</td> 
                <td><asp:TextBox runat="server" CssClass="TextBox" ID="tbCode" MaxLength = "5"/></td>
            </tr>
            <tr>
                <td>Panen Name</td>
                <td>:</td>
                <td><asp:TextBox runat="server" MaxLength="60" CssClass="TextBox" ID="tbName" Width="250px"/></td>
            </tr>
            <tr>
                <td>Status Tanam </td>
                <td>:</td>
                <td><asp:DropDownList runat="server" CssClass="DropDownList" ID="ddlStatusTanam" MaxLength = "5"
                        Width = "180px" Height="18px">
                    </asp:DropDownList>S/D<asp:DropDownList runat="server" CssClass="DropDownList" ID="ddlStatusTanam2" MaxLength = "5"
                        Width = "180px" Height="18px"> </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>Start BJR</td>
                <td>:</td>
                <td><asp:TextBox runat="server" CssClass="TextBox" ID="tbStartBJR" MaxLength = "9" Width="49px"/>
                  &nbsp;kgs</td>
            </tr>
            <tr>
                <td>End BJR</td>
                <td>:</td>
                <td><asp:TextBox runat="server" CssClass="TextBox" ID="tbEndBJR" MaxLength = "9" Width="49px"/>
                    &nbsp;kgs</td>
            </tr>
            <tr>
                <td>HK Per Basis</td>
                <td>:</td>
                <td><asp:TextBox runat="server" CssClass="TextBox" ID="tbHKPerBasis" MaxLength = "9" Width="70px"/>
                  &nbsp;HK</td>
            </tr>
             <tr>
                <td>1 Normal janjang</td>
                <td>:</td>
                <td><asp:TextBox runat="server" CssClass="TextBox" ID="tbEkuivalenABnormal" MaxLength = "9" Width="70px"/>
                  &nbsp;AbNormal Janjang</td>
            </tr>
            <tr>
            <td></td>
            <td></td>
            <td>
                <table>
                    <tr style="background-color:#F5F5F5;text-align:center">
                        <td></td>
                        <td>TBS</td>                        
                        <td>Beondolan</td>
                        <td>TBS Hitam</td>
                        <td>Brondolan Hitam</td>                                                
                        
                    </tr>
                    <tr><td>1 Baris</td>
                        <td><asp:TextBox ID="tbJanjangPerBasis" ValidationGroup="Input" runat="server" Width="120px" CssClass="TextBox"/></td>                        
                        <td><asp:TextBox ID="tbKrgPerBasis" ValidationGroup="Input" runat="server" Width="120px" CssClass="TextBox"/></td>
                        <td><asp:TextBox ID="tbJanjangPerBasisHt" ValidationGroup="Input" runat="server" Width="120px" CssClass="TextBox"/></td>
                        <td><asp:TextBox ID="tbKrgPerBasisHt" ValidationGroup="Input" runat="server" Width="120px" CssClass="TextBox"/></td>                                                
               
                    </tr>
                    <tr><td>Price Non Basis</td>
                        <td><asp:TextBox ID="tbPriceNBPerJanjang" ValidationGroup="Input" runat="server" Width="120px" CssClass="TextBox"/></td>                        
                        <td><asp:TextBox ID="tbPriceNBPerKrg" ValidationGroup="Input" runat="server" Width="120px" CssClass="TextBox"/></td>
                        <td><asp:TextBox ID="tbPriceNBPerJanjangHt" ValidationGroup="Input" runat="server" Width="120px" CssClass="TextBox"/></td>
                        <td><asp:TextBox ID="tbPriceNBPerKrgHt" ValidationGroup="Input" runat="server" Width="120px" CssClass="TextBox"/></td>                                                
                        
                    </tr>
                    <tr><td>Price Over Basis</td>
                        <td><asp:TextBox ID="tbPriceUPPerJanjang" ValidationGroup="Input" runat="server" Width="120px" CssClass="TextBox"/></td>                        
                        <td><asp:TextBox ID="tbPriceUPPerKrg" ValidationGroup="Input" runat="server" Width="120px" CssClass="TextBox"/></td>
                        <td><asp:TextBox ID="tbPriceUPPerJanjangHt" ValidationGroup="Input" runat="server" Width="120px" CssClass="TextBox"/></td>
                        <td><asp:TextBox ID="tbPriceUPPerKrgHt" ValidationGroup="Input" runat="server" Width="120px" CssClass="TextBox"/></td>                                                
                        
                    </tr>
                    <tr><td>Premi Basis</td>
                        <td><asp:TextBox ID="tbPremiBasis" ValidationGroup="Input" runat="server" Width="120px" CssClass="TextBox"/></td>                        
                        <td><asp:TextBox ID="tbPremiBasisKrg" ValidationGroup="Input" runat="server" Width="120px" CssClass="TextBox"/></td>
                        <td><asp:TextBox ID="tbPremiBasisHt" ValidationGroup="Input" runat="server" Width="120px" CssClass="TextBox"/></td>
                        <td><asp:TextBox ID="tbPremiBasisKrgHt" ValidationGroup="Input" runat="server" Width="120px" CssClass="TextBox"/></td>                                                
                        
                    </tr>
                </table>
            </td>                
        </tr>
             

            <tr>
                <td align="center" colspan="3">
                    <asp:Button ID="BtnSave" runat="server" class="bitbtndt btnsave" 
                        CommandName="Update" Text="Save" Height="20px" Width="66px" />
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
