    <%@ Page Language="VB" AutoEventWireup="false" CodeFile="MsProductBentuk.aspx.vb" Inherits="Master_MsProductBentuk_MsProductBentuk" %>
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
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div class="Content">
     <div class="H1"><asp:Label runat="server" ID="lblTitle" Text="Product Bentuk File"></asp:Label></div>
     <hr style="color:Blue" />
     <asp:Panel id="pnlHd" runat="server">
      <table>
        <tr>
            <td style="text-align:right; width:100px" >Quick Search :
                </td>
            <td><asp:TextBox CssClass="TextBox"  runat="server" ID ="tbFilter"/> 
                <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlField" >
                  <asp:ListItem Selected="true" Text="Product Jenis Code" Value="ProductJenis"></asp:ListItem>
                      <asp:ListItem Text="Product Bentuk" Value="ProductBentuk"></asp:ListItem>
                      <asp:ListItem Text="Product Bentuk Name" Value="BentukName"></asp:ListItem>
                      <%--<asp:ListItem Text="Stock" Value="FgStock"></asp:ListItem>
                      <asp:ListItem Text="Stock" Value="FgStock"></asp:ListItem>--%>
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
                      <asp:ListItem Selected="true" Text="Product Jenis Code" Value="ProductJenis"></asp:ListItem>
                      <asp:ListItem Text="Product Bentuk" Value="ProductBentuk"></asp:ListItem>
                      <asp:ListItem Text="Product Bentuk Name" Value="BentukName"></asp:ListItem>
                      <%--<asp:ListItem Text="Stock" Value="FgStock"></asp:ListItem>
                      <asp:ListItem Text="Stock" Value="FgStock"></asp:ListItem>--%>
                   </asp:DropDownList>
                
            </td>
        </tr>
     </table>
     </asp:Panel>
      <asp:GridView id="DataGrid" runat="server" ShowFooter="True" AllowSorting="True" 
            AutoGenerateColumns="False" AllowPaging="True" CssClass="Grid">
						<HeaderStyle CssClass="GridHeader" wrap="false"></HeaderStyle>
						<RowStyle CssClass="GridItem" Wrap="false"/>
						<AlternatingRowStyle CssClass="GridAltItem"/>
						<FooterStyle CssClass="GridFooter" />
						<PagerStyle CssClass="GridPager" />
				      <Columns>
				            <asp:TemplateField HeaderText="Product Jenis Code" HeaderStyle-Width="220" SortExpression="ProductJenis">
								<Itemtemplate>
									<asp:Label Runat="server" ID="ProductJenis" text='<%# DataBinder.Eval(Container.DataItem, "ProductJenis") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:Label Runat="server" ID="ProductJenisEdit" Text='<%# DataBinder.Eval(Container.DataItem, "ProductJenis") %>'>
									</asp:Label>								    
								</EditItemTemplate>
								<FooterTemplate>
								<asp:DropDownList ID="ProductJenisAdd" Runat="Server" Width="100%" CssClass="DropDownList"									    
                                        DataSourceID="dsProductJenis" DataTextField="Product_Jenis_Name" 
                                        DataValueField="Product_Jenis_Code" >                                        
									</asp:DropDownList>
								    
								</FooterTemplate>
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Product Bentuk" HeaderStyle-Width="150" SortExpression="ProductBentuk">
								<Itemtemplate>
									<asp:Label Runat="server" Width="150" ID="ProductBentuk" text='<%# DataBinder.Eval(Container.DataItem, "ProductBentuk") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:Label Runat="server" ID="ProductBentukEdit" Width="100%" Text='<%# DataBinder.Eval(Container.DataItem, "ProductBentuk") %>'>
									</asp:label>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="ProductBentukAdd" CssClass="TextBox" Width="100%" MaxLength="2" Runat="Server"/>
								   <cc1:TextBoxWatermarkExtender ID="ProductBentukAdd_TextBoxWatermarkExtender" 
                                        runat="server" Enabled="True" TargetControlID="ProductBentukAdd" WatermarkText="can't blank" WatermarkCssClass="Watermarked" >
                                   </cc1:TextBoxWatermarkExtender>
								</FooterTemplate>
							    <HeaderStyle Width="70px" />
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="BentukName" HeaderStyle-Width="300" SortExpression="BentukName">
								<Itemtemplate>
									<asp:Label Runat="server" Width="300" ID="BentukName" text='<%# DataBinder.Eval(Container.DataItem, "BentukName") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
								    <asp:TextBox Runat="server" Width="300" ID="BentukNameEdit" text='<%# DataBinder.Eval(Container.DataItem, "BentukName") %>'>
									</asp:TextBox>
									<%--<asp:DropDownList ID="BentukNameEdit" Runat="Server" Width="100%" CssClass="DropDownList" text='<%# DataBinder.Eval(Container.DataItem, "BentukName") %>'								    
                                        DataSourceID="dsProductMateri" DataTextField="Product_Name_Category" 
                                        DataValueField="Product_Type_Code" >                                        
									</asp:DropDownList>--%>
									<%--<cc1:TextBoxWatermarkExtender ID="FASubGrpNameEdit_TextBoxWatermarkExtender" 
                                        runat="server" Enabled="True" TargetControlID="FASubGrpNameEdit" WatermarkText="can't blank" WatermarkCssClass="Watermarked" >
                                    </cc1:TextBoxWatermarkExtender>--%>
								</EditItemTemplate>
								<FooterTemplate>
								   	<asp:TextBox ID="BentukNameAdd" CssClass="TextBox" Width="100%" MaxLength="60" Runat="Server"/>
									<cc1:TextBoxWatermarkExtender ID="BentukNamedd_TextBoxWatermarkExtender" 
                                        runat="server" Enabled="True" TargetControlID="BentukNameAdd" WatermarkText="can't blank" WatermarkCssClass="Watermarked" >
                                    </cc1:TextBoxWatermarkExtender>
								</FooterTemplate>
							    <HeaderStyle Width="130px" />
							</asp:TemplateField>	
							
							
														
							<%--<asp:TemplateField HeaderText="Stock" HeaderStyle-Width="30" ItemStyle-HorizontalAlign="Center" SortExpression="FgStock">
								<Itemtemplate>
									<asp:Label Runat="server" Width="30" ID="FgStock" text='<%# DataBinder.Eval(Container.DataItem, "FgStock") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:DropDownList ID="FgStockEdit" Enabled="False" CssClass="DropDownList" Width="100%" runat="server" SelectedValue='<%# DataBinder.Eval(Container.DataItem, "FgStock") %>'>
									  <asp:ListItem>Y</asp:ListItem>
									  <asp:ListItem>N</asp:ListItem>                                        
									</asp:DropDownList>								    
								</EditItemTemplate>
								<FooterTemplate>
									<asp:DropDownList ID="FgStockAdd" CssClass="DropDownList" Width="100%" runat="server">
									  <asp:ListItem Selected="True">Y</asp:ListItem>
									  <asp:ListItem>N</asp:ListItem>                                        
									</asp:DropDownList>								    
								</FooterTemplate>
							    <HeaderStyle Width="30px" />
                                <ItemStyle HorizontalAlign="Center" />
							</asp:TemplateField>--%>	
										
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
            <br />
         <%-- <div style="border:0px  solid; width:100%; height:100%; overflow:auto;">--%>
            </asp:Panel>
     <br />
     <asp:Panel ID="PanelInfo" runat="server" Visible = "false">         
     <asp:Label ID="label1" CssClass="H1" runat="server" Text="Product Bentuk : " />   
     <asp:Label ID="lbProductBentuk" ForeColor="Blue" CssClass="H1" Font-Bold="true" runat="server"  />
     </asp:Panel>
	 <br />
     <asp:Panel ID="pnlDt" runat="server" Visible = "false">  
     <asp:Button class="bitbtn btnback" width="50px" runat="server" ID="btnBackDtTop" Text="Back" />
	 <asp:Button class="bitbtndt btnadd" width="50px" runat="server" ID="btnAddDt" Text="Add" CommandName="Insert"/>    
	 <div style="border:0px  solid; width:100%; height:100%; overflow:auto;">
	        
            <asp:GridView ID="DataGridDt" runat="server" AllowPaging="True" 
                AllowSorting="True" AutoGenerateColumns="False" ShowFooter="False" 
                Width="1292px">
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
                    <asp:BoundField DataField="AccCOGS" SortExpression="AccCogs" HeaderText="Acc. Cogs" />
                    <asp:BoundField DataField="AccCogsName" SortExpression="AccCogsName" HeaderText="Acc. Cogs Name" />
                    <asp:BoundField DataField="AccTransitSJ" SortExpression="AccTransitSJ" HeaderText="Acc. Transit SJ" />                    
                    <asp:BoundField DataField="AccTransitSJName" SortExpression="AccTransitSJName" HeaderText="Acc. Transit SJ Name" />
                    <asp:BoundField DataField="AccSales" SortExpression="AccSales" HeaderText="Acc. Sales" />
                    <asp:BoundField DataField="AccSalesName" SortExpression="AccSalesName" HeaderText="Acc. Sales Name" />
                    <asp:BoundField DataField="AccTransitWrhs" SortExpression="AccTransitWrhs" HeaderText="Acc. Transit Wrhs" />
                    <asp:BoundField DataField="AccTransitWrhsName" SortExpression="AccTransitWrhsName" HeaderText="Acc. Transit Wrhs Name" />
                    <asp:BoundField DataField="AccTransitReject" SortExpression="AccTransitReject" HeaderText="Acc. Transit Reject" />
                    <asp:BoundField DataField="AccTransitRejectName" SortExpression="AccTransitTRejectName" HeaderText="Acc. Transit Reject Name" />
                    <asp:BoundField DataField="AccSRetur" SortExpression="AccTransitSRetur" HeaderText="Acc. Transit SRetur" />
                    <asp:BoundField DataField="AccSReturName" SortExpression="AccTransitSReturName" HeaderText="Acc. Transit SRetur Name" />                    
                    <asp:BoundField DataField="AccTransitRetur" SortExpression="AccTransitretur" HeaderText="Acc. TRetur" />
                    <asp:BoundField DataField="AccTransitReturName" SortExpression="AccTransitreturName" HeaderText="Acc. TR Name" />
                    <asp:BoundField DataField="AccExpLoss" SortExpression="AccSReturn" HeaderText="Acc. ExpLoss" />
                    <asp:BoundField DataField="AccExpLossName" SortExpression="AccSReturnName" HeaderText="Acc. ExpLoss Name" />

                                                     
                    <asp:TemplateField HeaderStyle-Width="126px" HeaderText="Action" Visible="false"
                        ItemStyle-Wrap="false">
                        <EditItemTemplate>
                            <asp:Button ID="btnUpdate0" runat="server" class="bitbtndt btnsave" Text="Save" CommandName="Update"/>									
                            <asp:Button ID="btnCancel0" runat="server" class="bitbtndt btncancel" Text="Cancel" CommandName="Cancel"/>									
                        </EditItemTemplate>
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
                <td>Acc. S Retur</td>
                <td>:</td>
                <td>
                    <asp:TextBox ID="tbAccSRetur" CssClass="TextBox" MaxLength="15" runat="server" AutoPostBack="True" />
                    <asp:TextBox ID="tbAccSReturName" CssClass="TextBox" MaxLength="60" Width=280 Enabled="False" runat="server" />
                    <asp:Button ID="btnAccSRetur" runat="server" class="btngo" Text="..."/>
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
                <td>Acc. </td>
                <td>:</td>
                <td>
                    <asp:TextBox ID="tbAccTransitWrhs" CssClass="TextBox" MaxLength="15" runat="server" AutoPostBack="True" />
                    <asp:TextBox ID="tbAccTransitWrhsName" CssClass="TextBox" MaxLength="60" Width=280 Enabled="False" runat="server" />
                    <asp:Button ID="btnAccTransitWrhs" runat="server" class="btngo" Text="..."/>
                </td>
            </tr>                                           
            <tr>
                <td>Acc. Transit Reject</td>
                <td>:</td>
                <td>
                    <asp:TextBox ID="tbAccTransitReject" CssClass="TextBox" MaxLength="15" runat="server" AutoPostBack="True" />
                    <asp:TextBox ID="tbAccTransitRejectName" CssClass="TextBox" MaxLength="60" Width=280 Enabled="False" runat="server" />
                    <asp:Button ID="btnAccTransitReject" runat="server" class="btngo" Text="..."/>
                </td>
            </tr> 
            <tr>
                <td>Acc. Transit Retur</td>
                <td>:</td>
                <td>
                    <asp:TextBox ID="tbAccTransitRetur" CssClass="TextBox" MaxLength="15" runat="server" AutoPostBack="True" />
                    <asp:TextBox ID="tbAccTransitReturName" CssClass="TextBox" MaxLength="60" Width=280 Enabled="False" runat="server" />
                    <asp:Button ID="btnAccTransitRetur" runat="server" class="btngo" Text="..."/>
                </td>
            </tr> 
            
            <tr>
                <td>Acc. Exp Loss</td>
                <td>:</td>
                <td>
                    <asp:TextBox ID="tbAccExpLoss" CssClass="TextBox" MaxLength="15" runat="server" AutoPostBack="True" />
                    <asp:TextBox ID="tbAccExpLossName" CssClass="TextBox" MaxLength="60" Width=280 Enabled="False" runat="server" />
                    <asp:Button ID="btnAccExpLoss" runat="server" class="btngo" Text="..."/>
                </td>
            </tr> 
 <%--           <tr>
                <td>Acc. Sales Retur</td>
                <td>:</td>
                <td>
                    <asp:TextBox ID="tbAccSReturn" CssClass="TextBox" MaxLength="15" runat="server" AutoPostBack="True" />
                    <asp:TextBox ID="tbAccSReturnName" CssClass="TextBox" MaxLength="60" Width=280 Enabled="False" runat="server" />
                    <asp:Button ID="btnAccSReturn" runat="server" class="btngo" Text="..."/>
                </td>
            </tr>  --%>
<%--            <tr>
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
            </tr> --%>
<%--            <tr>
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
            </tr>--%> 
            
                   
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
    <asp:SqlDataSource ID="dsProductCategory" runat="server" 
          SelectCommand="EXEC S_GetProductCategory">
    </asp:SqlDataSource>
        <asp:SqlDataSource ID="dsProductJenis" runat="server"            
           SelectCommand="EXEC S_GetProductJenis ">
        </asp:SqlDataSource>
    <%--<asp:SqlDataSource ID="dsWrhsType" runat="server" 
      SelectCommand="EXEC S_GetWrhsType">
    </asp:SqlDataSource>--%>
    <asp:Label ID="lstatus" ForeColor="red" runat="server"></asp:Label>
    </form>
    </body>
</html>