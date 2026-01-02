    <%@ Page Language="VB" AutoEventWireup="false" CodeFile="MsProductType.aspx.vb" Inherits="Master_MsProductType_MsProductType" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Product Type File</title>
    <script src="../../Function/OpenDlg.js" type="text/javascript"></script>
    <script type="text/javascript">
       function OpenPopup() {         
        window.open("../../SearchDlg.Aspx","List","scrollbars=yes,resizable=no,width=500,height=400");        
        return false;
    }    
    </script>
    
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
 <link href="https://fonts.googleapis.com/css2?family=Muli:wght@300;400;600&display=swap" rel="stylesheet">
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div class="Content">
     <div class="H1"><asp:Label runat="server" ID="lblTitle" Text="Product Type File"></asp:Label></div>
     <hr style="color:Blue" />
     <asp:Panel id="pnlHd" runat="server">
      <table>
        <tr>
            <td style="text-align:right; width:100px" >Quick Search :
                </td>
            <td><asp:TextBox CssClass="TextBox"  runat="server" ID ="tbFilter"/> 
                <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlField" >
                  <asp:ListItem Text="Product Jenis" Value="JenisName"></asp:ListItem>
                  <asp:ListItem Selected="true" Text="Product Type Code" Value="ProductType"></asp:ListItem>
                  <asp:ListItem Text="Product Type Name" Value="TypeName"></asp:ListItem>
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
                     <asp:ListItem Text="Product Jenis" Value="JenisName"></asp:ListItem>
                  <asp:ListItem Selected="true" Text="Product Type Code" Value="ProductType"></asp:ListItem>
                  <asp:ListItem Text="Product Type Name" Value="TypeName"></asp:ListItem>
                   </asp:DropDownList>
                
            </td>
        </tr>
     </table>
     </asp:Panel>
      <br />
     <%-- <div style="border:0px  solid; width:100%; height:100%; overflow:auto;">--%>
      <asp:GridView id="DataGrid" runat="server" ShowFooter="True" AllowSorting="True" 
            AutoGenerateColumns="False" AllowPaging="True" CssClass="Grid">
						<HeaderStyle CssClass="GridHeader" wrap="false"></HeaderStyle>
						<RowStyle CssClass="GridItem" Wrap="false"/>
						<AlternatingRowStyle CssClass="GridAltItem"/>
						<FooterStyle CssClass="GridFooter" />
						<PagerStyle CssClass="GridPager" />
				      <Columns>
				      <asp:TemplateField HeaderText="Product Jenis" HeaderStyle-Width="210" SortExpression="ProductJenis">
								<%--<Itemtemplate>
									<asp:DropDownList ID="ProductJenis" CssClass="DropDownList" Width="100%" runat="server" Enabled ="false" 
									SelectedValue='<%# DataBinder.Eval(Container.DataItem, "ProductJenis") %>'
									  DataSourceID="dsProductJenis" DataTextField="Product_Jenis_Name" 
                                        DataValueField="Product_Jenis_Code"> 
                                        </asp:DropDownList>	 
								</Itemtemplate>--%>
								<Itemtemplate>
									<asp:Label Runat="server" Width="70" ID="ProductJenis" text='<%# DataBinder.Eval(Container.DataItem, "JenisName") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:DropDownList ID="ProductJenisEdit" CssClass="DropDownList" Width="100%" runat="server" Enabled ="false" 
									SelectedValue='<%# DataBinder.Eval(Container.DataItem, "ProductJenis") %>'
									  DataSourceID="dsProductJenis" DataTextField="Product_Jenis_Name" 
                                        DataValueField="Product_Jenis_Code"> 
                                        </asp:DropDownList>	 
								</EditItemTemplate>
								<FooterTemplate>
									<asp:DropDownList ID="ProductJenisAdd" CssClass="DropDownList" Width="100%" runat="server"
									
									   DataSourceID="dsProductJenis" DataTextField="Product_Jenis_Name" 
                                        DataValueField="Product_Jenis_Code">                               
									</asp:DropDownList>	
								</FooterTemplate>
							    <HeaderStyle Width="210px" />
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Product Type Code" HeaderStyle-Width="70" SortExpression="ProductType">
								<Itemtemplate>
									<asp:Label Runat="server" Width="70" ID="ProductType" text='<%# DataBinder.Eval(Container.DataItem, "ProductType") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:Label Runat="server" ID="ProductTypeEdit" Width="100%" Text='<%# DataBinder.Eval(Container.DataItem, "ProductType") %>'>
									</asp:Label>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="ProductTypeAdd" CssClass="TextBox" Width="100%" MaxLength="5" Runat="Server"/>
								   <cc1:TextBoxWatermarkExtender ID="ProductTypeAdd_TextBoxWatermarkExtender" 
                                        runat="server" Enabled="True" TargetControlID="ProductTypeAdd" WatermarkText="can't blank" WatermarkCssClass="Watermarked" >
                                   </cc1:TextBoxWatermarkExtender>
								</FooterTemplate>
							    <HeaderStyle Width="70px" />
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Product Type Name" HeaderStyle-Width="280" SortExpression="TypeName">
								<Itemtemplate>
									<asp:Label Runat="server" Width="280" ID="TypeName" text='<%# DataBinder.Eval(Container.DataItem, "TypeName") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:TextBox Runat="server" ID="TypeNameEdit" CssClass="TextBox" Width="100%" Text='<%# DataBinder.Eval(Container.DataItem, "TypeName") %>'>
									</asp:TextBox>
									
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="TypeNameAdd" CssClass="TextBox" Width="100%" MaxLength="60" Runat="Server"/>
									<cc1:TextBoxWatermarkExtender ID="TypeNameAdd_TextBoxWatermarkExtender" 
                                        runat="server" Enabled="True" TargetControlID="TypeNameAdd" WatermarkText="can't blank" WatermarkCssClass="Watermarked" >
                                    </cc1:TextBoxWatermarkExtender>
								</FooterTemplate>
							    <HeaderStyle Width="280px" />
							</asp:TemplateField>	
							
							
														
								
										
							<asp:TemplateField HeaderText="Action" headerstyle-width="180" >
							    <ItemTemplate>
								    <asp:Button ID="btnEdit" runat="server" class="bitbtndt btnedit" Text="Edit" CommandName="Edit"/>
									<asp:Button ID="btndelete" runat="server" class="bitbtndt btndelete" Text="Delete" OnClientClick="return confirm('Sure to delete this data?');" CommandName="Delete"/>									
									<asp:Button ID="btnView" Width="70px" runat="server" class="bitbtndt btndetail" Text="Account" CommandName="View" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>"/>
								</ItemTemplate>
								<EditItemTemplate>
									<asp:Button ID="btnUpdate" runat="server" class="bitbtndt btnsave" Text="Save" CommandName="Update"/>									
                                    <asp:Button ID="btnCancel" runat="server" class="bitbtndt btncancel" Text="Cancel" CommandName="Cancel"/>									
								</EditItemTemplate>
								<FooterTemplate>
									<asp:Button class="bitbtndt btnadd" runat="server" ID="btnAdd" Text="Add" CommandName="Insert"/>									
								</FooterTemplate>
							    <HeaderStyle Width="180px" />
							</asp:TemplateField>							
    					</Columns>
        </asp:GridView>
            </asp:Panel>
     <br />
     <asp:Panel ID="PanelInfo" runat="server" Visible = "false">         
     <asp:Label ID="label1" CssClass="H1" runat="server" Text="Product Type : " />   
     <asp:Label ID="lbProductType" ForeColor="Blue" CssClass="H1" Font-Bold="true" runat="server" />
     </asp:Panel>
	 <br />
     <asp:Panel ID="pnlDt" runat="server" Visible = "false">  
     <asp:Button class="bitbtn btnback" width="50px" runat="server" ID="btnBackDtTop" Text="Back" />
	 <asp:Button class="bitbtndt btnadd" width="50px" runat="server" ID="btnAddDt" Text="Add" CommandName="Insert"/>    
	 <div style="border:0px  solid; width:100%; height:100%; overflow:auto;">
	        
            <asp:GridView ID="DataGridDt" runat="server" AllowPaging="True" 
                AllowSorting="True" AutoGenerateColumns="False" ShowFooter="False">
                <HeaderStyle CssClass="GridHeader" wrap="false" />
                <RowStyle CssClass="GridItem" wrap="false" />
                <AlternatingRowStyle CssClass="GridAltItem" />
                <FooterStyle CssClass="GridFooter" Wrap="false" />
                <PagerStyle CssClass="GridPager" />
                <Columns>
                <asp:TemplateField HeaderStyle-Width="110">
                            <ItemTemplate>
                                <asp:DropDownList CssClass="DropDownList" ID="ddl" runat="server">
                                <asp:ListItem Selected="True" Text="Edit" />
                                <asp:ListItem>Delete</asp:ListItem>                              
                                </asp:DropDownList>
                                <asp:Button class="btngo" runat="server" ID="btnExpand" Text="G" CommandName="Go" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>"/>
                            </ItemTemplate>
                            <HeaderStyle Width="110px" />
                        </asp:TemplateField>
                    <asp:BoundField DataField="WrhsType" SortExpression="WrhsType" HeaderText="Wrhs Type" />
                    <asp:BoundField DataField="AccInvent" SortExpression="AccInvent" HeaderText="Acc. Invent" />
                    <asp:BoundField DataField="AccInventName" SortExpression="AccInventName" HeaderText="Acc. Invent Name" />
                    <asp:BoundField DataField="AccCogs" SortExpression="AccCogs" HeaderText="Acc. Cogs" />
                    <asp:BoundField DataField="AccCogsName" SortExpression="AccCogsName" HeaderText="Acc. Cogs Name" />
                    <asp:BoundField DataField="AccTransitSJ" SortExpression="AccTransitSJ" HeaderText="Acc. Transit SJ" />                    
                    <asp:BoundField DataField="AccTransitSJName" SortExpression="AccTransitSJName" HeaderText="Acc. Transit SJ Name" />
                    <asp:BoundField DataField="AccSales" SortExpression="AccSales" HeaderText="Acc. Sales" />
                    <asp:BoundField DataField="AccSalesName" SortExpression="AccSalesName" HeaderText="Acc. Sales Name" />
                    <asp:BoundField DataField="AccDisc" SortExpression="AccDisc" HeaderText="Acc. Disc" />
                    <asp:BoundField DataField="AccDiscName" SortExpression="AccDiscName" HeaderText="Acc. Disc Name" />
                    <asp:BoundField DataField="AccTransitWrhs" SortExpression="AccTransitWrhs" HeaderText="Acc. Transit Wrhs" />
                    <asp:BoundField DataField="AccTransitWrhsName" SortExpression="AccTransitWrhsName" HeaderText="Acc. Transit Wrhs Name" />
                    <asp:BoundField DataField="AccTransitPRetur" SortExpression="AccTransitPRetur" HeaderText="Acc. Transit PRetur" />
                    <asp:BoundField DataField="AccTransitPReturName" SortExpression="AccTransitPReturName" HeaderText="Acc. Transit PRetur Name" />
                    <asp:BoundField DataField="AccTransitSRetur" SortExpression="AccTransitSRetur" HeaderText="Acc. Transit SRetur" />
                    <asp:BoundField DataField="AccTransitSReturName" SortExpression="AccTransitSReturName" HeaderText="Acc. Transit SRetur Name" />                    
                    <asp:BoundField DataField="AccPReturn" SortExpression="AccPReturn" HeaderText="Acc. PReturn" />
                    <asp:BoundField DataField="AccPReturnName" SortExpression="AccPReturnName" HeaderText="Acc. PReturn Name" />
                    <asp:BoundField DataField="AccSReturn" SortExpression="AccSReturn" HeaderText="Acc. SReturn" />
                    <asp:BoundField DataField="AccSReturnName" SortExpression="AccSReturnName" HeaderText="Acc. SReturn Name" />
                                                     
                    <asp:BoundField DataField="AccSTCAdjust" SortExpression="AccSTCAdjust" HeaderText="Acc. Adjust (+)" />
                    <asp:BoundField DataField="AccSTCAdjustName" SortExpression="AccSTCAdjustName" HeaderText="Acc. Adjust (+) Name" />
                    <asp:BoundField DataField="AccSTCLost" SortExpression="AccSTCLost" HeaderText="Acc. Adjust (-)" />
                    <asp:BoundField DataField="AccSTCLostName" SortExpression="AccSTCLostName" HeaderText="Acc. Adjust (-) Name" />
                    <asp:BoundField DataField="AccSampleExps" SortExpression="AccSampleExps" HeaderText="Acc. Sample Exps" />
                    <asp:BoundField DataField="AccSampleExpsName" SortExpression="AccSampleExpsName" HeaderText="Acc. Sample Exps Name" />
                    
                    <asp:BoundField DataField="AccWIPLabor" SortExpression="AccWIPLabor" HeaderText="Acc. WIP Labor" />
                    <asp:BoundField DataField="AccWIPLaborName" SortExpression="AccWIPLaborName" HeaderText="Acc. WIP Labor Name" />
                    <asp:BoundField DataField="AccWIPLabor2" SortExpression="AccWIPLabor2" HeaderText="Acc. WIP Labor 2" />
                    <asp:BoundField DataField="AccWIPLabor2Name" SortExpression="AccWIPLabor2Name" HeaderText="Acc. WIP Labor 2 Name" />
                    <asp:BoundField DataField="AccWIPFOH" SortExpression="AccWIPFOH" HeaderText="Acc. WIP FOH" />
                    <asp:BoundField DataField="AccWIPFOHName" SortExpression="AccWIPFOHName" HeaderText="Acc. WIP FOH Name" />
                                        
                    <asp:TemplateField HeaderStyle-Width="126px" HeaderText="Action" Visible="false"
                        ItemStyle-Wrap="false">
                        <EditItemTemplate>
                            <asp:Button ID="btnUpdate0" runat="server" class="bitbtndt btnsave" Text="Save" CommandName="Update"/>									
                            <asp:Button ID="btnCancel0" runat="server" class="bitbtndt btncancel" Text="Cancel" CommandName="Cancel"/>									
                        </EditItemTemplate>
                        <%--<FooterTemplate>
                            <asp:Button class="bitbtndt btnadd" runat="server" ID="btnAdd0" Text="Add" CommandName="Insert"/>								
                        </FooterTemplate>--%>
                        <ItemTemplate>
                            <asp:Button ID="btnEdit0" runat="server" class="bitbtndt btnedit" Text="Edit" CommandName="Edit"/>
							<asp:Button ID="btnDelete0" runat="server" class="bitbtndt btndelete" Text="Delete" OnClientClick="return confirm('Sure to delete this data?');" CommandName="Delete"/>									
                        </ItemTemplate>
                        <HeaderStyle Width="126px" />
                        <ItemStyle Wrap="False" />
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
	    
        </div>        
	    
	    <asp:Button class="bitbtn btnback" runat="server" width="50px" ID="btnBack2" Text="Back" />
	    <asp:Button class="bitbtndt btnadd" width="50px" runat="server" ID="btnAddDt2" Text="Add" CommandName="Insert"/>    
     </asp:Panel> 
     <asp:Panel ID="pnlInputDt" runat="server" Visible="false">
        <table>
           
            <tr>
                <td>Warehouse Type</td>
                <td>:</td>
                <td>
                <asp:DropDownList ID="ddlWrhsType" runat="server" CssClass="DropDownList"/>                   
                </td>
            </tr>                  
            <tr>
                <td>Acc. Invent</td>
                <td>:</td>
                <td>
                    <asp:TextBox ID="tbAccInvent" CssClass="TextBox" MaxLength="15" runat="server" AutoPostBack="True" />
                    <asp:TextBox ID="tbAccInventName" CssClass="TextBox" MaxLength="60" Width=280 Enabled="False" runat="server" />
                    <asp:Button ID="btnAccInvent" runat="server" class="btngo" Text="..."/>
                </td>
            </tr> 
            <tr>
                <td>Acc. COGS</td>
                <td>:</td>
                <td>
                    <asp:TextBox ID="tbAccCOGS" CssClass="TextBox" MaxLength="15" runat="server" AutoPostBack="True" />
                    <asp:TextBox ID="tbAccCOGSName" CssClass="TextBox" MaxLength="60" Width=280 Enabled="False" runat="server" />
                    <asp:Button ID="btnAccCOGS" runat="server" class="btngo" Text="..."/>
                </td>
            </tr>    
            <tr>
                <td>Acc. Transit SJ</td>
                <td>:</td>
                <td>
                    <asp:TextBox ID="tbAccTransitSJ" CssClass="TextBox" MaxLength="15" runat="server" AutoPostBack="True" />
                    <asp:TextBox ID="tbAccTransitSJName" CssClass="TextBox" MaxLength="60" Width=280 Enabled="False" runat="server" />
                    <asp:Button ID="btnAccTransitSJ" runat="server" class="btngo" Text="..."/>
                </td>
            </tr>    
            <tr>
                <td>Acc. Sales</td>
                <td>:</td>
                <td>
                    <asp:TextBox ID="tbAccSales" CssClass="TextBox" MaxLength="15" runat="server" AutoPostBack="True" />
                    <asp:TextBox ID="tbAccSalesName" CssClass="TextBox" MaxLength="60" Width=280 Enabled="False" runat="server" />
                    <asp:Button ID="btnAccSales" runat="server" class="btngo" Text="..."/>
                </td>
            </tr>   
            <tr>
                <td>Acc. Disc</td>
                <td>:</td>
                <td>
                    <asp:TextBox ID="tbAccDisc" CssClass="TextBox" MaxLength="15" runat="server" AutoPostBack="True" />
                    <asp:TextBox ID="tbAccDiscName" CssClass="TextBox" MaxLength="60" Width=280 Enabled="False" runat="server" />
                    <asp:Button ID="btnAccDisc" runat="server" class="btngo" Text="..."/>
                </td>
            </tr>                                                 
            <tr>
                <td>Acc. Transit Wrhs</td>
                <td>:</td>
                <td>
                    <asp:TextBox ID="tbAccTransitWrhs" CssClass="TextBox" MaxLength="15" runat="server" AutoPostBack="True" />
                    <asp:TextBox ID="tbAccTransitWrhsname" CssClass="TextBox" MaxLength="60" Width=280 Enabled="False" runat="server" />
                    <asp:Button ID="btnAccTransitWrhs" runat="server" class="btngo" Text="..."/>
                </td>
            </tr> 
            <tr>
                <td>Acc. Transit PRetur</td>
                <td>:</td>
                <td>
                    <asp:TextBox ID="tbAccTransitPRetur" CssClass="TextBox" MaxLength="15" runat="server" AutoPostBack="True" />
                    <asp:TextBox ID="tbAccTransitPReturName" CssClass="TextBox" MaxLength="60" Width=280 Enabled="False" runat="server" />
                    <asp:Button ID="btnAccTransitPRetur" runat="server" class="btngo" Text="..."/>
                </td>
            </tr> 
            <tr>
                <td>Acc. Transit SRetur</td>
                <td>:</td>
                <td>
                    <asp:TextBox ID="tbAccTransitSRetur" CssClass="TextBox" MaxLength="15" runat="server" AutoPostBack="True" />
                    <asp:TextBox ID="tbAccTransitSReturname" CssClass="TextBox" MaxLength="60" Width=280 Enabled="False" runat="server" />
                    <asp:Button ID="btnAccTransitSRetur" runat="server" class="btngo" Text="..."/>
                </td>
            </tr> 
            <tr>
                <td>Acc. Purchase Retur</td>
                <td>:</td>
                <td>
                    <asp:TextBox ID="tbAccPReturn" CssClass="TextBox" MaxLength="15" runat="server" AutoPostBack="True" />
                    <asp:TextBox ID="tbAccPReturnName" CssClass="TextBox" MaxLength="60" Width=280 Enabled="False" runat="server" />
                    <asp:Button ID="btnAccPReturn" runat="server" class="btngo" Text="..."/>
                </td>
            </tr> 
            <tr>
                <td>Acc. Sales Retur</td>
                <td>:</td>
                <td>
                    <asp:TextBox ID="tbAccSReturn" CssClass="TextBox" MaxLength="15" runat="server" AutoPostBack="True" />
                    <asp:TextBox ID="tbAccSReturnName" CssClass="TextBox" MaxLength="60" Width=280 Enabled="False" runat="server" />
                    <asp:Button ID="btnAccSReturn" runat="server" class="btngo" Text="..."/>
                </td>
            </tr>  
            <tr>
                <td>Acc. Adjust (+)</td>
                <td>:</td>
                <td>
                    <asp:TextBox ID="tbAccSTCAdjust" CssClass="TextBox" MaxLength="15" runat="server" AutoPostBack="True" />
                    <asp:TextBox ID="tbAccSTCAdjustName" CssClass="TextBox" MaxLength="60" Width=280 Enabled="False" runat="server" />
                    <asp:Button ID="btnAccSTCAdjust" runat="server" class="btngo" Text="..."/>
                </td>
            </tr> 
            <tr>
                <td>Acc. Adjust (-)</td>
                <td>:</td>
                <td>
                    <asp:TextBox ID="tbAccSTCLost" CssClass="TextBox" MaxLength="15" runat="server" AutoPostBack="True" />
                    <asp:TextBox ID="tbAccSTCLostName" CssClass="TextBox" MaxLength="60" Width=280 Enabled="False" runat="server" />
                    <asp:Button ID="btnAccSTCLost" runat="server" class="btngo" Text="..."/>
                </td>
            </tr> 
            <tr>
                <td>Acc. Sample Exps</td>
                <td>:</td>
                <td>
                    <asp:TextBox ID="tbAccSampleExps" CssClass="TextBox" MaxLength="15" runat="server" AutoPostBack="True" />
                    <asp:TextBox ID="tbAccSampleExpsName" CssClass="TextBox" MaxLength="60" Width=280 Enabled="False" runat="server" />
                    <asp:Button ID="btnAccSampleExps" runat="server" class="btngo" Text="..."/>
                </td>
            </tr> 
            <tr>
                <td>Acc. WIP Labor</td>
                <td>:</td>
                <td>
                    <asp:TextBox ID="tbAccWIPLabor" CssClass="TextBox" MaxLength="15" runat="server" AutoPostBack="True" />
                    <asp:TextBox ID="tbAccWIPLaborName" CssClass="TextBox" MaxLength="60" Width=280 Enabled="False" runat="server" />
                    <asp:Button ID="btnAccWIPLabor" runat="server" class="btngo" Text="..."/>
                </td>
            </tr> 
            <tr>
                <td>Acc. WIP Labor 2</td>
                <td>:</td>
                <td>
                    <asp:TextBox ID="tbAccWIPLabor2" CssClass="TextBox" MaxLength="15" runat="server" AutoPostBack="True" />
                    <asp:TextBox ID="tbAccWIPLabor2Name" CssClass="TextBox" MaxLength="60" Width=280 Enabled="False" runat="server" />
                    <asp:Button ID="btnAccWIPLabor2" runat="server" class="btngo" Text="..."/>
                </td>
            </tr> 
            <tr>
                <td>Acc. WIP FOH</td>
                <td>:</td>
                <td>
                    <asp:TextBox ID="tbAccWIPFOH" CssClass="TextBox" MaxLength="15" runat="server" AutoPostBack="True" />
                    <asp:TextBox ID="tbAccWIPFOHName" CssClass="TextBox" MaxLength="60" Width=280 Enabled="False" runat="server" />
                    <asp:Button ID="btnAccWIPFOH" runat="server" class="btngo" Text="..."/>
                </td>
            </tr> 
            
                   
            <tr>
                <td>
                    <asp:Button ID="btnCancel" runat="server" class="bitbtndt btncancel" Text="Back" CommandName="Cancel"/>
                </td>
                <td align="center" colspan="2">
                    <asp:Button ID="btnSave" runat="server" class="bitbtndt btnsave" Text="Save" />									
                    <asp:Button ID="btnReset" runat="server" class="bitbtndt btncancel" Text="Reset" CommandName="Reset"/>       
                </td>
            </tr>
        </table>
     </asp:Panel>  
    </div>    
    <asp:SqlDataSource ID="dsProductJenis" runat="server" 
          SelectCommand="EXEC S_GetProductJenis">
    </asp:SqlDataSource>
   
    <asp:Label ID="lstatus" ForeColor="red" runat="server"></asp:Label>
    </form>
    </body>
</html>