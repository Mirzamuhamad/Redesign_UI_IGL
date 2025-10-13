<%@ Page Language="VB" AutoEventWireup="False" CodeFile="MsCar.aspx.vb" Inherits="MsCar_MsCar" %>
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
    <style type="text/css">
        .style1
        {
            width: 415px;
        }
        .style3
        {
            height: 4px;
        }
        .style5
        {
            width: 123px;
        }
        .style6
        {
            width: 78px;
        }
        .style7
        {
            width: 300px;
        }
        .style8
        {
            width: 27px;
        }
        .style9
        {
            width: 201px;
        }
        .style10
        {
            width: 73px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div class="Content">
     <div class="H1">Car File</div>
     <hr style="color:Blue" />
     <asp:Panel runat="server" ID="pnlHd">
      <table>
        <tr>
            <td style="width:100px;text-align:right" >Quick Search :</td>
            <td><asp:TextBox runat="server" ID ="tbFilter" CssClass="TextBox" /> 
                <asp:DropDownList runat="server" ID="ddlField" CssClass="DropDownList" > 
                    <asp:ListItem Selected="true" Text="CarNo" Value="CarNo"></asp:ListItem>
                    <asp:ListItem Text="CarName" Value="CarName"></asp:ListItem>                   
                    <asp:ListItem Text="Merk" Value="Merk"></asp:ListItem>
                    <asp:ListItem Text="Model" Value="Model"></asp:ListItem>
                    <asp:ListItem Text="ManufactYear" Value="ManufactYear"></asp:ListItem>
                    <asp:ListItem Text="Cylinder" Value="Cylinder"></asp:ListItem>
                    <asp:ListItem Text="Color" Value="Color"></asp:ListItem>
                    <asp:ListItem Text="No Mesin" Value="MachineNo"></asp:ListItem>
                    <asp:ListItem Text="No Rangka" Value="RangkaNo"></asp:ListItem>
                    <asp:ListItem Text="BPKB" Value="BpkbNO"></asp:ListItem>
                    <asp:ListItem Text="Comsuption" Value="Comsuption"></asp:ListItem>
                    <asp:ListItem Text="KmP/Ltr" Value="KmPerLtr"></asp:ListItem>
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
                    <asp:ListItem Selected="true" Text="CarNo" Value="CarNo"></asp:ListItem>
                    <asp:ListItem Text="CarName" Value="CarName"></asp:ListItem>                   
                    <asp:ListItem Text="Merk" Value="Merk"></asp:ListItem>
                    <asp:ListItem Text="Model" Value="Model"></asp:ListItem>
                    <asp:ListItem Text="ManufactYear" Value="ManufactYear"></asp:ListItem>
                    <asp:ListItem Text="Cylinder" Value="Cylinder"></asp:ListItem>
                    <asp:ListItem Text="Color" Value="Color"></asp:ListItem>
                    <asp:ListItem Text="No Mesin" Value="MachineNo"></asp:ListItem>
                    <asp:ListItem Text="No Rangka" Value="RangkaNo"></asp:ListItem>
                    <asp:ListItem Text="BPKB" Value="BpkbNO"></asp:ListItem>
                    <asp:ListItem Text="Comsuption" Value="Comsuption"></asp:ListItem>
                    <asp:ListItem Text="KmP/Ltr" Value="KmPerLtr"></asp:ListItem>
  
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
				            <asp:BoundField DataField="CarNo" HeaderText="Car No" HeaderStyle-Width="140" SortExpression="CarNo"/>
							<asp:BoundField DataField="CarName" HeaderText="Car Name" HeaderStyle-Width="300" SortExpression="CarName"/>
							<asp:BoundField DataField="Merk" HeaderText="Merk " SortExpression="Merk" />
							<asp:BoundField DataField="Model" HeaderText="Model" SortExpression="Model" />	
							<asp:BoundField DataField="ManufactYear" HeaderText="Manufact Year" HeaderStyle-Width="200" SortExpression="ManufactYear"/>						
							<asp:BoundField DataField="Cylinder" HeaderText="Cylinder" SortExpression="Cylinder" />
							<asp:BoundField DataField="Color" HeaderText="Luas Area" HeaderStyle-Width="120" SortExpression="Color"/>
						    <asp:BoundField DataField="MachineNo" HeaderText="Machine No" HeaderStyle-Width="300" SortExpression="MachineNo"/>
						    <asp:BoundField DataField="RangkaNo" HeaderText="Rangka No" HeaderStyle-Width="300" SortExpression="RangkaNo"/>
							<asp:BoundField DataField="BpkbNO" HeaderText="BPKB No" HeaderStyle-Width="120" SortExpression="BpkbNO"/>
                            <asp:BoundField DataField="Comsuption" HeaderText="Comsuption" HeaderStyle-Width="300" SortExpression="Comsuption"/>
                            <asp:BoundField DataField="Supplier" HeaderText="Supplier" HeaderStyle-Width="300" SortExpression="Supplier"/>
							<asp:BoundField DataField="KmPerLtr" HeaderText="Km/Ltr" HeaderStyle-Width="300" SortExpression="KmPerLtr" DataFormatString = "{0:#.##.##}"/>
							<asp:BoundField DataField="Length" HeaderText="Lenght" HeaderStyle-Width="300" SortExpression="Length" DataFormatString = "{0:#,##0.##}"/>
							<asp:BoundField DataField="Width" HeaderText="Width" HeaderStyle-Width="70" SortExpression="Width"/>
                            <asp:BoundField DataField="Volume" HeaderText="Volume" HeaderStyle-Width="70" SortExpression="Volume"/>
                            <asp:BoundField DataField="DueSTNK" HeaderText="STNK " SortExpression="DueSTNK" DataFormatString="{0:dd MMM yyyy}"/>
                            <asp:BoundField DataField="DueBPKB" HeaderText="BPKB" SortExpression="DueBPKB" DataFormatString="{0:dd MMM yyyy}"/>
                            <asp:BoundField DataField="DueKir" HeaderText="Kir" SortExpression="DueKir" DataFormatString="{0:dd MMM yyyy}"/>
                            <asp:BoundField DataField="DueIjinTrayek" HeaderText="Iajin Trayek" SortExpression="DueIjinTrayek" DataFormatString="{0:dd MMM yyyy}"/>				
							
    					</Columns>
        </asp:GridView>
        <asp:Button class="bitbtn btnadd" runat="server" ID="btnAdd2" Text="Add" 
             Visible="False" />									
      </asp:Panel>
      <asp:Panel runat="server" ID="pnlInput" Visible="false">
        <table>
            <tr>
                <td>Vehicle</td>
                <td>:</td> 
                <td class="style1">
                    <asp:TextBox runat="server" CssClass="TextBox" ID="tbVehicleCode" 
                        Width="119px"/>
                    &nbsp;&nbsp;&nbsp;
                    <asp:TextBox ID="tbVehicleName" runat="server" CssClass="TextBox" 
                        Width="279px" />
                <asp:Label ID="Label4" runat="server" ForeColor="Red" Text="*"></asp:Label></td>
            </tr>
            <tr>
                <td>Merk</td>
                <td>:</td>
                <td >
                    <table cellpadding="0" cellspacing="0" >
                        <tr>
                            <td >
                                <asp:TextBox ID="tbMerk" runat="server" CssClass="TextBox"
                                    ValidationGroup="Input" Width="180px" />
                            </td>

                            <td>&emsp;&nbsp;&nbsp;</td>
                            <td>
                                Model</td>
                            <td>
                                &emsp;&emsp;: &nbsp;</td>
                            <td >
                                <asp:TextBox ID="tbModel" runat="server" CssClass="TextBox" 
                                    ValidationGroup="Input" Width="151px"  />
                            </td>
                        </tr>
                    </table>
                </td>
                
               
            </tr>
            <tr>
                <td>Manufacture Year</td>
                <td>:</td>
                <td class="style1"> 
                    <table cellpadding="0" cellspacing="0">
                        <tr>
                            <td>
                                <asp:TextBox ID="tbManufacture" runat="server" CssClass="TextBox"  
                                    ValidationGroup="Input" Width="115px" />
                            </td>
                            <td>&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp; </td>
                            <td>
                                Cylinder</td>
                            <td>
                                &emsp; &nbsp;: &nbsp;</td>
                            <td >
                                <asp:TextBox ID="tbCylinder" runat="server" CssClass="TextBox"  
                                    ValidationGroup="Input" Width="151px" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>Color</td>
                <td>:</td>
                <td > 
                    <table cellpadding="0" cellspacing="0" >
                        <tr>
                            <td class="style2">
                                <asp:TextBox ID="tbColor" runat="server" CssClass="TextBox" 
                                    ValidationGroup="Input" />
                            </td>
                            <td>&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp; </td>
                            <td>
                                Type</td>
                            <td>
                                &emsp; &nbsp;&emsp; &nbsp;: &nbsp;</td>
                            <td >
                                <asp:DropDownList ID="ddlType" runat="server" CssClass="DropDownList" 
                                     ValidationGroup="Input" Width="157px"  >
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>Machine No</td>
                <td>:</td>
                <td >
                    <table cellpadding="0" cellspacing="0">
                        <tr>
                            <td>
                                <asp:TextBox ID="tbMachine" runat="server" CssClass="TextBox" 
                                    ValidationGroup="Input" />
                            </td>
                            <td>&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp; </td>
                            <td>
                                Rangka No</td>
                            <td>
                                &nbsp;&nbsp;: &nbsp;</td>
                            <td>
                                <asp:TextBox ID="tbRangka" runat="server" CssClass="TextBox" 
                                    ValidationGroup="Input" Width="151px"   />
                            </td>
                        </tr>
                    </table>
               
            </tr>
            <tr>
                <td>BPKB No</td>
                <td>:</td>
                <td class="style1">
                    <asp:TextBox ID="tbBPKB" runat="server" CssClass="TextBox" 
                        ValidationGroup="Input" Width="180px" />
                    &nbsp;</td>
            </tr>           
            <tr>
                <td>Consumtion</td>
                <td>:</td>
                <td class="style1">
                    <asp:DropDownList ID="ddlConsumtion" runat="server" CssClass="DropDownList" 
                        Height="18px" MaxLength="20" ValidationGroup="Input" Width="200px">
                    </asp:DropDownList>
                <asp:Label ID="Label6" runat="server" ForeColor="Red" Text="*"></asp:Label></td>
            </tr>
            <tr>
                <td>Supplier</td>
                <td>:</td>
                <td class="style1">
                    <asp:TextBox ID="tbSupplier" runat="server" AutoPostBack="true" 
                        CssClass="TextBox"  ValidationGroup="Input" />
                    <asp:TextBox ID="tbSuppliertName" runat="server" CssClass="TextBox" Enabled="false" 
                        MaxLength="60" Width="251px" />
                    <asp:Button ID="btnsupplier" runat="server" Class="btngo" Text="..." 
                        ValidationGroup="Input" />
                    <asp:Label ID="Label12" runat="server" ForeColor="Red" Text="*"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>Amount</td>
                <td>:</td>
                <td class="style1"><table cellpadding="" cellspacing="2">
                    <tr style="background-color:Silver;text-align:center">
                        <td >
                            Km/Ltr</td>
                        <td>
                            Lenght (M)</td>
                        <td >
                            Weidth(M)</td>
                        <td >
                            Height(M)</td>
                             <td >
                                 Volune(M) </td>
                                 
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="tbKmPerLtr" runat="server" CssClass="TextBox" 
                                ValidationGroup="Input" Width="81px" />
                        </td>
                        <td>
                            <asp:TextBox ID="tbLength" runat="server" CssClass="TextBox" 
                                ValidationGroup="Input" Width="81px" />
                        </td>
                        <td>
                            <asp:TextBox ID="tbwidth" runat="server" CssClass="TextBox" 
                                ValidationGroup="Input" Width="81px" />
                        </td>
                        <td>
                            <asp:TextBox ID="tbheight" runat="server" CssClass="TextBox" 
                                ValidationGroup="Input" Width="81px" />
                        </td>
                        <td>
                            <asp:TextBox ID="tbVolume" runat="server" CssClass="TextBox" 
                                ValidationGroup="Input" Width="81px" />
                                
                        </td>
                        <td><asp:Label ID="Labe" runat="server" ForeColor="Red" Text="*"></asp:Label></td>
                        
                    </tr>
                </table>
                    
                </td>
            </tr>
            <tr>
                <td>Driver</td>
                <td>:</td>
                <td class="style1"><asp:TextBox runat="server" MaxLength="5" CssClass="TextBox" 
                        ID="tbDriver" ValidationGroup="Input" Width="222px"/>
                    &nbsp;</td>
               </tr>
             <tr>
                <td>Budget Cost </td>
                <td>:</td>
                <td class="style1"><table cellpadding="0" cellspacing="2">
                    <tr style="background-color:Silver;text-align:center">
                        <td>
                            Maintenance<asp:Label ID="Label8" runat="server" ForeColor="Red" Text="*"></asp:Label></td>
                        <td>
                            Renewal<asp:Label ID="Label9" runat="server" ForeColor="Red" Text="*"></asp:Label></td>
                        <td>
                            People</td>
                        
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="tbMaintenance" runat="server" CssClass="TextBox" 
                                ValidationGroup="Input" Width="81px" />
                        </td>
                        <td>
                            <asp:TextBox ID="tbRenewal" runat="server" CssClass="TextBox" 
                                ValidationGroup="Input" Width="81px" />
                        </td>
                        <td>
                            <asp:TextBox ID="tbPeople" runat="server" CssClass="TextBox" 
                                ValidationGroup="Input" Width="81px" />
                            </td>
                            <td>&nbsp;<asp:Label ID="Label7" runat="server" ForeColor="blue" Text="/ KM"></asp:Label> </td>
                       
                    </tr>
                    </table>
                 </td>
            </tr>
             <tr>
                <td>Due Date</td>
                <td>:</td>
                <td class="style1">
                    <table cellpadding="0" cellspacing="2">
                        <tr style="background-color:Silver;text-align:center">
                            <td>
                                STNK)<asp:Label ID="Label10" runat="server" ForeColor="Red" Text="*"></asp:Label></td>
                            <td>
                                BPKB<asp:Label ID="Label11" runat="server" ForeColor="Red" Text="*"></asp:Label></td>
                            <td>
                                KIR</td>
                            <td>
                                Ijin Trayek</td>
                        </tr>
                        <tr>
                            <td>
                                <BDP:BasicDatePicker ID="tbSTNK" runat="server" ButtonImageHeight="19px" 
                                    ButtonImageWidth="20px" DateFormat="dd MMM yyyy" DisplayType="TextBoxAndImage" 
                                    ReadOnly="true" ShowNoneButton="False" TextBoxStyle-CssClass="TextDate" 
                                    ValidationGroup="Input">
                                    <TextBoxStyle CssClass="TextDate" />
                                </BDP:BasicDatePicker>
                            </td>
                            <td>
                                <BDP:BasicDatePicker ID="tbBPKBDate" runat="server" ButtonImageHeight="19px" 
                                    ButtonImageWidth="20px" DateFormat="dd MMM yyyy" DisplayType="TextBoxAndImage" 
                                    ReadOnly="true" ShowNoneButton="False" TextBoxStyle-CssClass="TextDate" 
                                    ValidationGroup="Input">
                                    <TextBoxStyle CssClass="TextDate" />
                                </BDP:BasicDatePicker>
                            </td>
                            <td>
                                <BDP:BasicDatePicker ID="tbKIR" runat="server" ButtonImageHeight="19px" 
                                    ButtonImageWidth="20px" DateFormat="dd MMM yyyy" DisplayType="TextBoxAndImage" 
                                    ReadOnly="true" ShowNoneButton="False" TextBoxStyle-CssClass="TextDate" 
                                    ValidationGroup="Input">
                                    <TextBoxStyle CssClass="TextDate" />
                                </BDP:BasicDatePicker>
                            </td>
                            <td>
                                <BDP:BasicDatePicker ID="tbIjinTrayek" runat="server" ButtonImageHeight="19px" 
                                    ButtonImageWidth="20px" DateFormat="dd MMM yyyy" DisplayType="TextBoxAndImage" 
                                    ReadOnly="true" ShowNoneButton="False" TextBoxStyle-CssClass="TextDate" 
                                    ValidationGroup="Input">
                                    <TextBoxStyle CssClass="TextDate" />
                                </BDP:BasicDatePicker>
                            </td>
                        </tr>
                    </table>
                 </td>
            </tr>
             <tr>
                <td>Fg Active</td>
                <td>:</td>
                <td class="style1">
                    <table cellpadding="0" cellspacing="0">
                        <tr>
                            <td class="style6">
                                <asp:DropDownList ID="ddlFgActive" runat="server" AutoPostBack="true" 
                                    CssClass="DropDownList" Height="18px" MaxLength="20" ValidationGroup="Input" 
                                    Width="40px">
                                    <asp:ListItem Selected="True">Y</asp:ListItem>
                                    <asp:ListItem>N</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>&emsp;&emsp;</td>
                            <td>
                                Angkut Internal</td>
                            <td>
                                <table cellpadding="0" cellspacing="2">
                                    <tr>
                                        <td>
                                            :</td>
                                        <td>
                                            <asp:DropDownList ID="ddlFgAngkut" runat="server" AutoPostBack="true" 
                                                CssClass="DropDownList" Height="18px" MaxLength="20" ValidationGroup="Input" 
                                                Width="40px">
                                                <asp:ListItem Selected="True">Y</asp:ListItem>
                                                <asp:ListItem>N</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td>&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;</td>
                                        <td >
                                            Kirim External</td>
                                        <td>
                                            :</td>
                                        <td>
                                            <asp:DropDownList ID="ddlFgKirim" runat="server" AutoPostBack="true" 
                                                CssClass="DropDownList" Height="18px" MaxLength="20" ValidationGroup="Input" 
                                                Width="40px">
                                                <asp:ListItem Selected="True">Y</asp:ListItem>
                                                <asp:ListItem>N</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                    
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
