<%@ Page Language="VB" AutoEventWireup="false" CodeFile="MsDivisi.aspx.vb" Inherits="Master_MsDivisi_MsDivisi" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Payment Type File</title>
    <script type="text/javascript">
    function OpenPopup() {         
        window.open("../../SearchDlg.Aspx","List","scrollbars=yes,resizable=no,width=500,height=400");        
        return false;
    }    
    </script>
   <script src="../../Function/OpenDlg.JS" type="text/javascript"></script> 
   <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div class="Content">
     <div class="H1"><asp:Label runat="server" ID="lblTitle"></asp:Label></div>
     <hr style="color:Blue" />
     <asp:Panel id="pnlHd" runat="server">
      <table>
        <tr>
            <td style="text-align:right; width:100px" >Quick Search :
            </td>
            <td><asp:TextBox CssClass="TextBox"  runat="server" ID ="tbFilter"/> 
                <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlField" >
                      <asp:ListItem Selected="true" Text="Division Code" Value="DivisionCode"></asp:ListItem>
                      <asp:ListItem Text="Division Name" Value="DivisionName"></asp:ListItem>
                      <asp:ListItem Text="Estate" Value="EstateName"></asp:ListItem>
                      <asp:ListItem Text="Area" Value="Area"></asp:ListItem>
                      <asp:ListItem Text="Manager" Value="EmpManagerName">   </asp:ListItem>  
                      <asp:ListItem Text="Kepala Tata Usaha" Value="EmpKTUName"></asp:ListItem>  
                      <asp:ListItem Text="Asisten" Value="EmpAsistenName"></asp:ListItem>  
                      <asp:ListItem Text="Askep" Value="EmpAskepName"></asp:ListItem> 
                      <asp:ListItem Text="Mandor" Value="EmpMandorName"></asp:ListItem> 
                      <asp:ListItem Text="Auditor" Value="EmpAuditorName"></asp:ListItem>                                            
                      <asp:ListItem Text="Warehouse" Value="EmpWarehouseName"></asp:ListItem> 
                      <asp:ListItem Text="PPIC" Value="EmpPPICName"></asp:ListItem> 
                      <asp:ListItem Text="Accounting" Value="EmpAccountingName"></asp:ListItem>                       
                      <asp:ListItem Text="Krani" Value="EmpKraniName"></asp:ListItem>                       
                      <asp:ListItem Text="Askep Produksi" Value="EmpAskepProduksiName"></asp:ListItem>                       
                      <asp:ListItem Text="Batch" Value="FgBatch"></asp:ListItem>                       
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
            <td style="width:100px;text-align:right"><asp:DropDownList runat="server" ID="ddlNotasi" >
                    <asp:ListItem Selected="true" Text="OR" Value="OR"></asp:ListItem>
                    <asp:ListItem Text="AND" Value="AND"></asp:ListItem>         
                </asp:DropDownList>
            </td>
            <td><asp:TextBox CssClass="Button" runat="server" ID ="tbfilter2"/> 
                  <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlField2" >
                      <asp:ListItem Selected="true" Text="Division Code" Value="DivisionCode"></asp:ListItem>
                      <asp:ListItem Text="Division Name" Value="DivisionName"></asp:ListItem>
                      <asp:ListItem Text="Estate" Value="EstateName"></asp:ListItem>
                      <asp:ListItem Text="Area" Value="Area"></asp:ListItem>
                      <asp:ListItem Text="Manager" Value="EmpManagerName">   </asp:ListItem>  
                      <asp:ListItem Text="Kepala Tata Usaha" Value="EmpKTUName"></asp:ListItem>  
                      <asp:ListItem Text="Asisten" Value="EmpAsistenName"></asp:ListItem>  
                      <asp:ListItem Text="Askep" Value="EmpAskepName"></asp:ListItem> 
                      <asp:ListItem Text="Mandor" Value="EmpMandorName"></asp:ListItem> 
                      <asp:ListItem Text="Auditor" Value="EmpAuditorName"></asp:ListItem>                                            
                      <asp:ListItem Text="Warehouse" Value="EmpWarehouseName"></asp:ListItem> 
                      <asp:ListItem Text="PPIC" Value="EmpPPICName"></asp:ListItem> 
                      <asp:ListItem Text="Accounting" Value="EmpAccountingName"></asp:ListItem>                       
                      <asp:ListItem Text="Krani" Value="EmpKraniName"></asp:ListItem>                       
                      <asp:ListItem Text="Askep Produksi" Value="EmpAskepProduksiName"></asp:ListItem>                                        
                      <asp:ListItem Text="Batch" Value="FGBatch"></asp:ListItem>                                           
                 </asp:DropDownList>               
            </td>
        </tr>
     </table>
     </asp:Panel>
      <br />
      <div style="border:0px  solid; width:100%; height:100%; overflow:auto;">
      <asp:Button class="bitbtn btnadd" runat="server" ID="btnAdd" Text="Add" />	        
          <asp:GridView ID="DataGrid" runat="server" AllowPaging="True" 
              AllowSorting="True" AutoGenerateColumns="False" CssClass="Grid" 
              ShowFooter="True">
              <HeaderStyle CssClass="GridHeader" wrap="False" />
              <RowStyle CssClass="GridItem" Wrap="false" />
              <AlternatingRowStyle CssClass="GridAltItem" />
              <FooterStyle CssClass="GridFooter" />
              <PagerStyle CssClass="GridPager" />
              <Columns>
                  <asp:TemplateField HeaderStyle-Width="110">
                      <ItemTemplate>
                          <asp:DropDownList ID="ddl" runat="server" CssClass="DropDownList">
                              <asp:ListItem Selected="True" Text="View" />
                              <asp:ListItem Text="Edit" />
                              <asp:ListItem Text="Delete" />                              
                          </asp:DropDownList>
                          <asp:Button ID="btnGO" runat="server" class="btngo" 
                              CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" CommandName="Go" 
                              Text="G" />
                      </ItemTemplate>
                      <HeaderStyle Width="110px" />
                  </asp:TemplateField>
                  <asp:BoundField DataField="DivisionCode" HeaderStyle-Width="170px" 
                      HeaderText="Division Code" SortExpression="DivisionCode">
                      <HeaderStyle Width="170px" />
                  </asp:BoundField>
                  <asp:BoundField DataField="DivisionName" HeaderStyle-Width="200px" 
                      HeaderText="Division Name" SortExpression="DivisionName">
                      <HeaderStyle Width="200px" />
                  </asp:BoundField>
                  <asp:BoundField DataField="EstateName" HeaderStyle-Width="200px" 
                      HeaderText="Estate" SortExpression="EstateName">
                      <HeaderStyle Width="150px" />
                  </asp:BoundField>
                  <asp:BoundField DataField="Area" HeaderStyle-Width="200px" 
                      HeaderText="Area" SortExpression="Area">
                      <HeaderStyle Width="200px" />
                  </asp:BoundField>                  
                  <asp:BoundField DataField="FgBatch" HeaderStyle-Width="50px" 
                      HeaderText="Batch" SortExpression="FgBatch">
                      <HeaderStyle Width="50px" />
                  </asp:BoundField>
                  <asp:BoundField DataField="FgActive" HeaderStyle-Width="50px" 
                      HeaderText="Active" SortExpression="FgActive">
                      <HeaderStyle Width="50px" />
                  </asp:BoundField>
                  
                
                  <%--<asp:TemplateField HeaderText="Action" HeaderStyle-Width="200px">
							    <ItemTemplate>
								    <asp:Button ID="btnEdit" runat="server" class="bitbtndt btnedit" Text="Edit" CommandName="Edit"/>
									<asp:Button ID="btndelete" runat="server" class="bitbtndt btndelete" Text="Delete" OnClientClick="return confirm('Sure to delete this data?');" CommandName="Delete"/>									
									<asp:Button ID="btnView" runat="server" class="bitbtndt btndetail" Text="Detail" CommandName="View" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>"/>
									<asp:Button ID="btnAssign" runat="server" class="bitbtndt btndetail" Text="User" CommandName="Assign" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>"/>   
								</ItemTemplate>								
							    <HeaderStyle Width="200px" />
							</asp:TemplateField>	--%>
              </Columns>
          </asp:GridView>
      </div>
    </asp:Panel>
     <asp:Panel runat="server" ID="pnlInput" Visible="false">
        <table>
            <tr>
                <td>Division Code</td>
                <td>:</td>
                <td><asp:TextBox runat="server" CssClass="TextBox" ID="tbDivisionCode" MaxLength="5" 
                        ValidationGroup="Input" /></td>
            </tr>
            <tr>
                <td>Division Name</td>
                <td>:</td>
                <td><asp:TextBox runat="server" MaxLength="60" CssClass="TextBox" ID="tbDivisionName" 
                        Width="250px" ValidationGroup="Input"/></td>
            </tr>
            
            <tr>
                <td>Estate</td>
                <td>:</td>
                <td><asp:DropDownList runat="server" CssClass="DropDownList" ID="ddlestate" 
                        Width="250px" Enabled = "false">
                    <asp:ListItem Selected = "True">Choose One</asp:ListItem></asp:DropDownList></td>
            </tr>
          
            <tr>
                <td>Area</td>
                <td>:</td>
                <td><asp:TextBox runat="server" MaxLength="30" CssClass="TextBox" Enabled = "False" 
                        ID="tbarea" Width="90px" EnableTheming="False" ReadOnly="True"/></td>
            </tr>
            <tr>
                <td>Manager</td>
                <td>:</td>
                <td><asp:TextBox runat="server" MaxLength="12" CssClass="TextBox" ID="tbmanager" AutoPostBack=true 
                        Width="100px"/>
                    <asp:TextBox runat="server" CssClass="TextBox" ID="tbmanagerName" Enabled="false"
                        Width="250px"/>    
                    <asp:Button ID="btnmanager" runat="server" class="btngo" Text="..."/>                        
                </td>
            </tr>
            <tr>
                <td>Kepala Tata Usaha</td>
                <td>:</td>
                <td><asp:TextBox runat="server" MaxLength="12" CssClass="TextBox" ID="tbKTU" AutoPostBack=true
                        Width="100px"/>
                     <asp:TextBox runat="server" CssClass="TextBox" ID="tbKTUName" Enabled="false"
                        Width="250px"/>   
                     <asp:Button ID="btnKTU" runat="server" class="btngo" Text="..."/>                       
                 </td>
            </tr>
            <tr>
                <td>Asisten</td>
                <td>:</td>
                <td><asp:TextBox runat="server" MaxLength="12" CssClass="TextBox" ID="tbasisten" AutoPostBack=true
                        Width="100px"/>
                    <asp:TextBox runat="server" CssClass="TextBox" ID="tbasistenname" Enabled="false"
                        Width="250px"/>   
                    <asp:Button ID="btnasisten" runat="server" class="btngo" Text="..."/>                         
                </td>
            </tr>
            <tr>
                <td>As-Kep</td>
                <td>:</td>
                <td><asp:TextBox runat="server" MaxLength="12" CssClass="TextBox" ID="tbaskep" AutoPostBack=true
                    Width="100px"/>
                    <asp:TextBox runat="server" CssClass="TextBox" ID="tbaskepname" Enabled="false"
                        Width="250px"/>
                    <asp:Button ID="btnaskep" runat="server" class="btngo" Text="..."/>                        
                </td>
            </tr>
            <tr>
                <td>Mandor</td>
                <td>:</td>
                <td><asp:TextBox runat="server" MaxLength="12" CssClass="TextBox" ID="tbmandor" AutoPostBack=true
                    Width="100px"/>
                    <asp:TextBox runat="server" CssClass="TextBox" ID="tbmandorname" Enabled="false"
                        Width="250px"/>
                    <asp:Button ID="btnmandor" runat="server" class="btngo" Text="..."/>                        
                </td>
            </tr>
            <tr>
                <td>Auditor</td>
                <td>:</td>
                <td><asp:TextBox runat="server" MaxLength="12" CssClass="TextBox" ID="tbauditor" AutoPostBack=true
                    Width="100px"/>
                    <asp:TextBox runat="server" CssClass="TextBox" ID="tbauditorName" Enabled="false"
                        Width="250px"/>
                    <asp:Button ID="btnauditor" runat="server" class="btngo" Text="..."/>                        
                </td>
            </tr>
            <tr>
                <td>Warehouse</td>
                <td>:</td>
                <td><asp:TextBox runat="server" MaxLength="12" CssClass="TextBox" ID="tbwarehouse" AutoPostBack=true
                    Width="100px"/>
                    <asp:TextBox runat="server" CssClass="TextBox" ID="tbwarehouseName" Enabled="false"
                        Width="250px"/>
                    <asp:Button ID="btnwarehouse" runat="server" class="btngo" Text="..."/>                        
                </td>
            </tr>
            <tr>
                <td>PPIC</td>
                <td>:</td>
                <td><asp:TextBox runat="server" MaxLength="12" CssClass="TextBox" ID="tbPPIC" AutoPostBack=true
                    Width="100px"/>
                    <asp:TextBox runat="server" CssClass="TextBox" ID="tbPPICName" Enabled="false"
                        Width="250px"/>
                    <asp:Button ID="btnPPIC" runat="server" class="btngo" Text="..."/>                        
                </td>
            </tr>
            <tr>
                <td>Accounting</td>
                <td>:</td>
                <td><asp:TextBox runat="server" MaxLength="12" CssClass="TextBox" ID="tbaccounting" AutoPostBack=true
                    Width="100px"/>
                    <asp:TextBox runat="server" CssClass="TextBox" ID="tbaccountingName" Enabled="false"
                        Width="250px"/>
                    <asp:Button ID="btnaccounting" runat="server" class="btngo" Text="..."/>                        
                </td>
            </tr>
            <tr>
                <td>Krani</td>
                <td>:</td>
                <td><asp:TextBox runat="server" MaxLength="12" CssClass="TextBox" ID="tbkrani" AutoPostBack=true
                    Width="100px"/>
                    <asp:TextBox runat="server" CssClass="TextBox" ID="tbkraniName" Enabled="false"
                        Width="250px"/>
                    <asp:Button ID="btnkrani" runat="server" class="btngo" Text="..."/>                        
                </td>
            </tr>
            <tr>
                <td>As-Kep Produksi</td>
                <td>:</td>
                <td><asp:TextBox runat="server" MaxLength="12" CssClass="TextBox" ID="tbaskepproduksi" AutoPostBack=true
                    Width="100px"/>
                    <asp:TextBox runat="server" CssClass="TextBox" ID="tbaskepproduksiName" Enabled="false"
                        Width="250px"/>
                    <asp:Button ID="btnaskepproduksi" runat="server" class="btngo" Text="..."/>                        
                </td>
            </tr>
            <tr>
                <td>Batch</td>
                <td>:</td>
                <td><asp:DropDownList runat="server" CssClass="DropDownList" ID="ddlfgbatch" 
                        Width="80px">
                    <asp:ListItem>Y</asp:ListItem>
                    <asp:ListItem Selected="True">N</asp:ListItem>
                    </asp:DropDownList>                    
                </td>                    
            </tr>
            <tr>
                <td>Active</td>
                <td>:</td>
                <td><asp:DropDownList runat="server" CssClass="DropDownList" ID="ddlActive" 
                        Width="80px">
                    <asp:ListItem>Y</asp:ListItem>
                    <asp:ListItem>N</asp:ListItem>
                    </asp:DropDownList>                    
                </td>                    
            </tr>
            <tr>
                <td colspan="3" align="center">
                <asp:Button ID="BtnSave" runat="server" class="bitbtndt btnsave" Text="Save" CommandName="Update" ValidationGroup="Input"/>&nbsp;									
                <asp:Button ID="btnReset" runat="server" class="bitbtndt btncancel" Text="Reset" CommandName="Cancel" ValidationGroup="Input"/>&nbsp;                     
                <asp:Button ID="btnCancel" runat="server" class="bitbtndt btncancel" Text="Back" CommandName="Cancel"/>&nbsp;									                                    
                </td>
            </tr>
        </table>
      </asp:Panel>         
     </div>   
    <asp:Label ID="lstatus" ForeColor="red" runat="server"></asp:Label>

    </form>
    </body>
</html>
