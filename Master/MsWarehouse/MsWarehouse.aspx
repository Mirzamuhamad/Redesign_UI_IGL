<%@ Page Language="VB" AutoEventWireup="false" CodeFile="MsWarehouse.aspx.vb" Inherits="MsWarehouse_MsWarehouse" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Warehouse File</title>
    <script src="../../Function/OpenDlg.JS" type="text/javascript"></script>
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
 <link href="https://fonts.googleapis.com/css2?family=Muli:wght@300;400;600&display=swap" rel="stylesheet">
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div class="Content">
     <div class="H1">Warehouse File</div>
     <hr style="color:Blue" />
     <asp:Panel runat="server" ID="pnlHd">
      <table>
        <tr>
            <td style="width:100px;text-align:right" >Quick Search :</td>
            <td><asp:TextBox runat="server" ID ="tbFilter" CssClass="TextBox" /> 
                <asp:DropDownList runat="server" ID="ddlField" CssClass="DropDownList" > 
                    <asp:ListItem Selected="true" Text="Warehouse Code" Value="WrhsCode"></asp:ListItem>
                    <asp:ListItem Text="Warehouse Name" Value="WrhsName"></asp:ListItem>
                    <asp:ListItem Text="Warehouse Group" Value="WrhsGroupName"></asp:ListItem>                   
                    <asp:ListItem Text="Warehouse Area" Value="WrhsAreaName"></asp:ListItem> 
                    <asp:ListItem Text="Warehouse Type" Value="WrhsType"></asp:ListItem>                     
                    <asp:ListItem Text="Warehouse Condition" Value="WrhsCondition"></asp:ListItem> 
                    <asp:ListItem Text="Contact Person" Value="A.ContactPerson"></asp:ListItem>
                    <asp:ListItem Text="Work Center" Value="WorkCtr"></asp:ListItem>
                    <asp:ListItem Text="Work Center Name" Value="W.WorkCtrName"></asp:ListItem>
                    <asp:ListItem Text="Active" Value="FgActive"></asp:ListItem>
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
                    <asp:ListItem Selected="true" Text=" Warehouse Code" Value="WrhsCode"></asp:ListItem>
                    <asp:ListItem Text="Warehouse Name" Value="WrhsName"></asp:ListItem>
                    <asp:ListItem Text="Warehouse Group" Value="WrhsGroupName"></asp:ListItem>                   
                    <asp:ListItem Text="Warehouse Area" Value="WrhsAreaName"></asp:ListItem> 
                    <asp:ListItem Text="Warehouse Type" Value="WrhsType"></asp:ListItem>                     
                    <asp:ListItem Text="Warehouse Condition" Value="WrhsCondition"></asp:ListItem> 
                    <asp:ListItem Text="Contact Person" Value="A.ContactPerson"></asp:ListItem>
                    <asp:ListItem Text="Work Center" Value="WorkCtr"></asp:ListItem>
                    <asp:ListItem Text="Work Center Name" Value="W.WorkCtrName"></asp:ListItem>
                    <asp:ListItem Text="Active" Value="FgActive"></asp:ListItem>
                  </asp:DropDownList>
                
            </td>
        </tr>
     </table>
     </asp:Panel>
      <br />
      <asp:Button class="bitbtn btnadd" runat="server" ID="btnAdd" Text="Add" />									
      <asp:GridView id="DataGrid" runat="server" ShowFooter="True" AllowSorting="True" 
            AutoGenerateColumns="False" AllowPaging="True" CssClass="Grid">
						<HeaderStyle CssClass="GridHeader" Wrap = "false"></HeaderStyle>
						<RowStyle CssClass="GridItem" wrap="false" />
						<AlternatingRowStyle CssClass="GridAltItem"/>
						<FooterStyle CssClass="GridFooter" />
						<PagerStyle CssClass="GridPager" HorizontalAlign="Left" />
				      <Columns>
				      <asp:TemplateField HeaderStyle-Width="110">
                            <ItemTemplate>
                                <asp:DropDownList CssClass="DropDownList" ID="ddl" runat="server">
                                <asp:ListItem Selected="True" Text="View"/>
                                <asp:ListItem Text="Edit" />
                                <asp:ListItem Text ="Delete"/>
                                <asp:ListItem Text="User"/>
                                <asp:ListItem Text="Location"/>
                                </asp:DropDownList>
                                <asp:Button class="btngo" runat="server" ID="btnGO" Text="G" CommandName="Go" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>"/>                               
                            </ItemTemplate>
                      <HeaderStyle Width="110px" />
                      </asp:TemplateField>
				            <asp:BoundField DataField="WrhsCode" HeaderText=" Warehouse Code" HeaderStyle-Width="140" SortExpression="WrhsCode"/>
							<asp:BoundField DataField="WrhsName" HeaderText=" Warehouse Name" HeaderStyle-Width="300" SortExpression="WrhsName"/>
							<asp:BoundField DataField="wrhsgroupName" HeaderText=" Warehouse Group Name" HeaderStyle-Width="220" SortExpression="wrhsgroupName"/>
							<asp:BoundField DataField="WrhsAreaName" HeaderText="Warehouse Area Name" HeaderStyle-Width="220" SortExpression="wrhsAreaName" />						
							<asp:BoundField DataField="WrhsType" HeaderText="Warehouse Type" HeaderStyle-Width="220" SortExpression="WrhsType" />							
							<asp:BoundField DataField="WrhsCondition" HeaderText="Warehouse Condition" HeaderStyle-Width="220" SortExpression="WrhsCondition"/>
							<asp:BoundField DataField="ContactPerson" HeaderText="Contact Person" HeaderStyle-Width="220" SortExpression="ContactPerson"/>
							<asp:BoundField DataField="WorkCtrName" HeaderText="Work Center Name" HeaderStyle-Width="220" SortExpression="WorkCtrName"/>
							<asp:BoundField DataField="FgActive" HeaderText="Active" SortExpression="FgActive"/>
							<asp:BoundField DataField="FgCustomer" HeaderText="Customer" SortExpression="FgCustomer"/>
							<asp:BoundField DataField="FgBuffer" HeaderText="Buffer" SortExpression="FgBuffer"/>
							<asp:BoundField DataField="FgBlock" HeaderText="Block" SortExpression="FgBlock"/>
							<asp:BoundField DataField="FgSuplier" HeaderText="Suplier" SortExpression="FgSuplier"/>																					
							<%--<asp:TemplateField HeaderText="Action" ItemStyle-Wrap ="false" >
							    <ItemTemplate>
								    <asp:Button ID="btnEdit" runat="server" class="bitbtndt btnedit" Text="Edit" CommandName="Edit"/>
									<asp:Button ID="btndelete" runat="server" class="bitbtndt btndelete" Text="Delete" OnClientClick="return confirm('Sure to delete this data?');" CommandName="Delete"/>									
									<asp:Button CommandName="User" Text="User" CssClass="bitbtndt btndetail" ID="btnUser" Runat="server" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>"/>
                                    <asp:Button CommandName="Location" Text="Location" CssClass="bitbtndt btndetail" ID="btnLocation" Runat="server" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" Width = "65px"/>
								</ItemTemplate>								
							</asp:TemplateField>--%>
    					</Columns>
        </asp:GridView>
        <asp:Button class="bitbtn btnadd" runat="server" ID="btnAdd2" Text="Add" />									
      </asp:Panel>
      <asp:Panel runat="server" ID="pnlInput" Visible="false">
        <table>
            <tr>
                <td>Warehouse Code</td>
                <td>:</td>
                <td><asp:TextBox runat="server" MaxLength="10" CssClass="TextBox" ID="tbCode" /></td>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td>Warehouse Name</td>
                <td>:</td>
                <td><asp:TextBox runat="server" MaxLength="60" CssClass="TextBox" ID="tbName" 
                        Width="228px"/></td>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td>Warehouse Group</td>
                <td>:</td>
                <td><asp:DropDownList runat="server" CssClass="DropDownList" ID="ddlWrhsGroup" Width="160px">                        
                    </asp:DropDownList>                    
                </td>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td>Warehouse Area</td>
                <td>:</td>
                <td><asp:DropDownList runat="server" CssClass="DropDownList" ID="ddlWrhsArea" Width="160px">                    
                    </asp:DropDownList>
                </td>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td>Warehouse Type</td>
                <td>:</td>
                <td><asp:DropDownList runat="server" CssClass="DropDownList" ID="ddlWrhsType" AutoPostBack="true" Width="160px">
                    </asp:DropDownList>
                </td>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td>Warehouse Class</td>
                <td>:</td>
                    <td>
                        <asp:DropDownList runat="server" CssClass="DropDownList" ID="ddlWrhsClass" Width="160px">  
                             <asp:ListItem Selected="True">GOOD</asp:ListItem> 
                             <asp:ListItem>BAD</asp:ListItem> 
                             <asp:ListItem >WO</asp:ListItem> 
                             <asp:ListItem >LAPANGAN</asp:ListItem> 
                             <asp:ListItem >REMAIN</asp:ListItem> 
                             <asp:ListItem >BUFFER</asp:ListItem>                       
                        </asp:DropDownList>                    
                    </td>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td>Contact Person</td>
                <td>:</td>
                <td><asp:TextBox runat="server" MaxLength="60" CssClass="TextBox" 
                        ID="tbContactPerson" Width="230px"/></td>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td>Work Center</td>
                <td>:</td>
                <td><asp:DropDownList runat="server" CssClass="DropDownList" ID="ddlWorkCtr" Width="160px">                        
                    </asp:DropDownList>                    
                </td>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td>Customer</td>
                <td>:</td>
                <td><asp:DropDownList runat="server" CssClass="DropDownList" ID="ddlCustomer">
                        <asp:ListItem >Y</asp:ListItem>
                        <asp:ListItem Selected="True" >N</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td>
                    Buffer</td>
                <td>
                    :</td>
                <td>
                    <asp:DropDownList ID="ddlBuffer" runat="server" CssClass="DropDownList">
                        <asp:ListItem >Y</asp:ListItem>
                        <asp:ListItem Selected="True">N</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td>
                    <table>
                        <tr>
                            <td>
                                Suplier</td>
                            <td>
                                :</td>
                            <td>
                                <asp:DropDownList ID="ddlSuplier" runat="server" CssClass="DropDownList">
                                    <asp:ListItem >Y</asp:ListItem>
                                    <asp:ListItem Selected="True">N</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    Block</td>
                <td>
                    :</td>
                <td>
                    <asp:DropDownList ID="ddlBlock" runat="server" CssClass="DropDownList">
                        <asp:ListItem >Y</asp:ListItem>
                        <asp:ListItem Selected="True">N</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td>
                    <table>
                        <tr>
                            <td>
                                Active &nbsp;
                            </td>
                            <td>
                                :</td>
                            <td>
                                <asp:DropDownList ID="ddlActive" runat="server" CssClass="DropDownList">
                                    <asp:ListItem Selected="True">Y</asp:ListItem>
                                    <asp:ListItem>N</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td colspan="3" align="center">
                <asp:Button ID="BtnSave" runat="server" class="bitbtndt btnsave" Text="Save" CommandName="Update"/>									
                <asp:Button ID="btnReset" runat="server" class="bitbtndt btncancel" Text="Reset" CommandName="Cancel"/>                     
                <asp:Button ID="btnCancel" runat="server" class="bitbtndt btncancel" Text="Back" CommandName="Cancel"/>									                                    
                </td>
                <td align="center">
                    &nbsp;</td>
            </tr>
        </table>
      </asp:Panel>              
     <asp:Label ID="lstatus" ForeColor="red" runat="server"></asp:Label>        
    </div>
    </form>
</body>
</html>
