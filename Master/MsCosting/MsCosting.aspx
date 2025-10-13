<%@ Page Language="VB" AutoEventWireup="false" CodeFile="MsCosting.aspx.vb" Inherits="Master_MsCosting_MsCosting" %>
<%@ Register Assembly="BasicFrame.WebControls.BasicDatePicker" Namespace="BasicFrame.WebControls" TagPrefix="BDP" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<%@ Register Assembly="DevExpress.Web.ASPxGridView.v10.2, Version=10.2.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxGridView" TagPrefix="dx" %>
<%@ Register assembly="DevExpress.Web.ASPxEditors.v10.2" namespace="DevExpress.Web.ASPxEditors" tagprefix="dx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Costing</title>
    
    <%--<script type="text/javascript">
        function OpenPopup() {         
            window.open("../../SearchDlg.Aspx","List","scrollbars=yes,resizable=no,width=500,height=400");        
            return false;
        }
    </script>--%>
    
    <style type="text/css">
        .style2
        {
            width: 79px;
        }
        .style3
        {
            width: 66px;
        }
    </style>
    
   <script src="../../Function/OpenDlg.JS" type="text/javascript"></script>
   <script src="../../Function/Function.JS" type="text/javascript"></script>
   <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
 <link href="https://fonts.googleapis.com/css2?family=Muli:wght@300;400;600&display=swap" rel="stylesheet">
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div class="Content">
     <div class="H1">Costing</div>
     <hr style="color:Blue" />
     
     <asp:Panel id="pnlHd" runat="server">
     
        <%--<asp:Panel runat="server" ID="pnlSearch" Visible="true">
            <table width="100%">
                    <tr>--%>
                        <%--<td style="width:100px">Group By </td>
                        <td>:</td>--%>
                        <%--<td class="style1" style="text-align:right;">Rental :</td>
                        <td>
                            <asp:DropDownList CssClass="DropDownList" ID = "ddlGroupBy" runat = "server" AutoPostBack="true" Visible="false" >
                                <asp:ListItem Selected="True">Method</asp:ListItem>
                                <asp:ListItem>User</asp:ListItem>
                            </asp:DropDownList>                            
                        
                            <asp:TextBox CssClass="TextBox" Id="tbCode" runat="server" Width="75px" AutoPostBack="True"/>
                            <asp:TextBox CssClass="TextBox" ID="tbName" runat="server" Width="150px" Enabled="False" /> 
                            <asp:Button ID="btnSearch" runat="server" class="btngo" Text="..."/>                        
                        </td>
                        <td style="width:50px"></td>
                   </tr>                         
            </table>
        </asp:Panel>--%>
         <table>
             <tr>
                 <td style="width: 100px; text-align: right">
                     Quick Search :
                 </td>
                 <td>
                     <asp:TextBox runat="server" ID="tbFilter" CssClass="TextBox" />
                     <asp:DropDownList runat="server" ID="ddlField" CssClass="DropDownList">
                         <asp:ListItem Selected="true" Text="Costing Code" Value="CostingCode"></asp:ListItem>
                         <asp:ListItem Text="Costing Name" Value="CostingName"></asp:ListItem>
                         <asp:ListItem Value="CostingType">Costing Type</asp:ListItem>
                     </asp:DropDownList>
                     <asp:Button class="bitbtn btnsearch" runat="server" ID="btnSearch" Text="Search" />
                     <asp:Button class="btngo" runat="server" ID="btnExpand" Text="..." />
                     <%--<asp:Button class="bitbtn btnprint" runat="server" ID="btnPrint" Text="Print" />--%>
                 </td>
             </tr>
         </table>
         <asp:Panel runat="server" ID="pnlSearch" Visible="false">
             <table>
                 <tr>
                     <td style="width: 100px; text-align: right">
                         <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlNotasi">
                             <asp:ListItem Selected="true" Text="OR" Value="OR"></asp:ListItem>
                             <asp:ListItem Text="AND" Value="AND"></asp:ListItem>
                         </asp:DropDownList>
                     </td>
                     <td>
                         <asp:TextBox runat="server" ID="tbfilter2" CssClass="TextBox" />
                         <asp:DropDownList runat="server" ID="ddlField2" CssClass="DropDownList">
                             <asp:ListItem Selected="true" Text="Costing Code" Value="CostingCode"></asp:ListItem>
                             <asp:ListItem Text="Costing Name" Value="CostingName"></asp:ListItem>
                             <asp:ListItem Value="CostingType">Costing Type</asp:ListItem>
                         </asp:DropDownList>
                     </td>
                 </tr>
             </table>
         </asp:Panel>
        <br />
        <div style="border:0px  solid; width:100%; height:100%; overflow:auto;">

            <asp:GridView id="DataGrid" runat="server" width = "650"   
            ShowFooter="True" AllowSorting="True" 
            AutoGenerateColumns="False" AllowPaging="True" CssClass="Grid">
						<HeaderStyle CssClass="GridHeader"></HeaderStyle>
						<RowStyle CssClass="GridItem" Wrap="false"/>
						<AlternatingRowStyle CssClass="GridAltItem"/>
						<FooterStyle CssClass="GridFooter" />
						<PagerStyle CssClass="GridPager" HorizontalAlign="Center" />					  
				<EmptyDataTemplate>
				    
				</EmptyDataTemplate>	  
				      <Columns>
				            <asp:TemplateField HeaderText="Costing Code" HeaderStyle-Width="100" SortExpression="CostingCode">
								<Itemtemplate>
									<asp:Label Runat="server" ID="CostingCode" text='<%# DataBinder.Eval(Container.DataItem, "CostingCode") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
								    <asp:Label Runat="server" ID="CostingCodeTemp" text='<%# DataBinder.Eval(Container.DataItem, "CostingCode") %>' Visible="true">
									</asp:Label>
									<%--<asp:TextBox Runat="server" ID="CostingCodeEdit" CssClass="TextBox" MaxLength="5" Width="100%" Text='<%# DataBinder.Eval(Container.DataItem, "CostingCode") %>'>
									</asp:TextBox>--%>
									<%--<cc1:TextBoxWatermarkExtender ID="CostingCodeEdit_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="CostingCodeEdit" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>--%>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="CostingCodeAdd" Runat="Server" CssClass="TextBox" MaxLength="5" Width="100%"/>
									<cc1:TextBoxWatermarkExtender ID="CostingCodeAdd_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="CostingCodeAdd" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>
								</FooterTemplate>
							</asp:TemplateField>
										      
							<asp:TemplateField HeaderText="Costing Name" HeaderStyle-Width="200" SortExpression="CostingName">
								<Itemtemplate>
									<asp:Label Runat="server" ID="CostingName" text='<%# DataBinder.Eval(Container.DataItem, "CostingName") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:TextBox Runat="server" ID="CostingNameEdit" CssClass="TextBox" MaxLength="60" Width="100%" Text='<%# DataBinder.Eval(Container.DataItem, "CostingName") %>'>
									</asp:TextBox>
									<cc1:TextBoxWatermarkExtender ID="CostingNameEdit_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="CostingNameEdit" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="CostingNameAdd" Runat="Server" CssClass="TextBox" MaxLength="60" Width="100%"/>
									<cc1:TextBoxWatermarkExtender ID="CostingNameAdd_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="CostingNameAdd" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>
								</FooterTemplate>
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Costing Type" HeaderStyle-Width="80" ItemStyle-HorizontalAlign="Center" SortExpression="CostingType">
								<Itemtemplate>
									<asp:Label Runat="server" Width="80" ID="CostingType" TEXT='<%# DataBinder.Eval(Container.DataItem, "CostingType") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
                                    <asp:DropDownList ID="CostingTypeEdit" CssClass="DropDownList" Width="100%" runat="server"
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "CostingType") %>'>
                                        <asp:ListItem>Labor Direct</asp:ListItem>
                                        <asp:ListItem>Labor In-Direct</asp:ListItem>
                                        <asp:ListItem>Overhead</asp:ListItem>
                                    </asp:DropDownList>								    
								</EditItemTemplate>
								<FooterTemplate>
									<asp:DropDownList ID="CostingTypeAdd" CssClass="DropDownList" Width="100%" runat="server">									  
									  <asp:ListItem Selected="True">Labor Direct</asp:ListItem>
                                      <asp:ListItem>Labor In-Direct</asp:ListItem>
									  <asp:ListItem>Overhead</asp:ListItem>                                                                    
									</asp:DropDownList>								    
								</FooterTemplate>
                                <ItemStyle HorizontalAlign="Center" />
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Action" headerstyle-width="100" >
								<ItemTemplate>
								    <asp:Button class="bitbtn btnedit" runat="server" ID="btnEdit" Text="Edit" CommandName="Edit" />																						 											
									<asp:Button class="bitbtn btndelete" runat="server" ID="btnDelete" Text="Delete" CommandName="Delete"  OnClientClick="return confirm('Sure to delete this data?');" />
									<asp:Button ID="btnView" runat="server" class="bitbtndt btndetail" Text="Account" CommandName="Account" Width="75px" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" />
								</ItemTemplate>
								<EditItemTemplate>
									<asp:Button class="bitbtn btnsave" runat="server" ID="btnUpdate" Text="Save" CommandName="Update" />																						 																		
									<asp:Button class="bitbtn btncancel" runat="server" ID="btnCancel" Text="Cancel" CommandName="Cancel" />																						 																		
								</EditItemTemplate>
								<FooterTemplate>
								   <asp:Button class="bitbtn btnadd" runat="server" ID="btnAdd" Text="Add" CommandName="Insert" />
								</FooterTemplate>
							</asp:TemplateField>
							
    					</Columns>
            </asp:GridView>

        </div>
    </asp:Panel>
    
    <asp:Panel runat="server" ID="PnlAssign" Visible="false" >          
        <%--<table>
            <tr>
                <td>Rental</td>
                <td>:</td>
                <td>
                    <asp:TextBox CssClass="TextBox" Id="tbRental" runat="server" Width="75px" Enabled="false"/>
                </td>
            </tr>
            <tr>
                <td>Material Type</td>
                <td>:</td>
                <td>
                    <asp:TextBox CssClass="TextBox" Id="TbMaterialType" runat="server" Width="75px" Enabled="false"/>
                </td>
            </tr> 
            <tr>
                <td>
                    <asp:Button ID="btnCancel" runat="server" class="bitbtndt btncancel" Text="Back" CommandName="Cancel"/>
                </td>
            </tr>                          
        </table>--%>
        <table>
            <tr>
                <td class="style2">
                    <asp:Button ID="btnCancel" runat="server" class="bitbtndt btncancel" Text="Back" CommandName="Cancel"/>
                </td>
            </tr>
        </table>
        <br />
        <table width="100%">
            <tr>
                <%--<td style="width: 100px">
                    Group By
                </td>--%>
                <%--<td>
                    :
                </td>--%>
                <td class="style3" style="text-align: left;">
                    Costing Item :
                </td>
                <td>
                    <%--<asp:DropDownList CssClass="DropDownList" ID="ddlGroupBy" runat="server" AutoPostBack="true"
                        Visible="false">
                        <asp:ListItem Selected="True">Method</asp:ListItem>
                        <asp:ListItem>User</asp:ListItem>
                    </asp:DropDownList>--%>
                    <asp:TextBox CssClass="TextBox" ID="tbCode" runat="server" Width="75px" AutoPostBack="false" Enabled="False" />
                    <asp:TextBox CssClass="TextBox" ID="tbName" runat="server" Width="150px" Enabled="False" />
                    <%--<asp:Button ID="btnSearch" runat="server" class="btngo" Text="..."/>--%>
                </td>
                <td style="width: 50px">
                </td>
            </tr>
        </table>
        
        <br />        
            
        <table width="100%">
            <tr>
                <td align="center">
                <asp:Label ID="lblAssign" runat="server" Text="Material Assigned"></asp:Label>
                </td>
                <td>&nbsp</td>
                <td align="center">
                <asp:Label ID="lblAvailable" runat="server" Text="Material Available"></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="width:40%;">
                   <div style="border:0px  solid; width:100%; height:100%; overflow:auto;">                   
                       <dx:ASPxGridView ID="AssignedGrid" runat="server" Width="100%" style="table-layout:fixed;"
                            EmptyDataText="There are no data records to display." KeyFieldName = "CODE"                     
                            AllowPaging="True" DataSourceID="dsAssigned" AutoGenerateColumns="False"><Columns><dx:GridViewCommandColumn ShowSelectCheckbox="True" VisibleIndex="0"><HeaderTemplate><input type="checkbox" onclick="AssignedGrid.SelectAllRowsOnPage (this.checked);" title="Select/Unselect all rows on the page" /></HeaderTemplate><HeaderStyle HorizontalAlign="Center" /></dx:GridViewCommandColumn><dx:GridViewDataColumn FieldName="CODE" VisibleIndex="1" ><Settings AutoFilterCondition="Contains" /></dx:GridViewDataColumn><dx:GridViewDataColumn FieldName="NAME" VisibleIndex="2" ><Settings AutoFilterCondition="Contains" /></dx:GridViewDataColumn></Columns><Settings ShowFilterRow="True" ShowFilterBar="Auto" ShowFilterRowMenu="True" /></dx:ASPxGridView>
                    </div>
                    <asp:SqlDataSource ID="dsAssigned" runat="server" />
                </td>
                <td style="width:3%" align ="center">
                        <asp:Button OnClick="btnAddAll_Click" ID="btnAddAll" ToolTip="Add All"  runat="server" class="btnassign" Text="<<"/>                                                                         
                        <br />
                        <br />
                        <asp:Button OnClick="btnAdd_Click" ID="btnAdd" ToolTip="Add"  runat="server" class="btnassign" Text="<"/>                                                                         
                        <br />
                        <br />
                        <asp:Button OnClick="btnRemove_Click" ID="btnRemove" ToolTip="Remove"  runat="server" class="btnassign" Text=">"/>                                                                         
                        <br />
                        <br />
                        <asp:Button OnClick="btnRemoveAll_Click" ID="btnRemoveAll" ToolTip="Remove All"  runat="server" class="btnassign" Text=">>"/>                                                                                                 
                    </td>
                <td style="width:40%;">
                    <div style="border:0px  solid; width:100%; height:100%; overflow:auto;">
                        <dx:ASPxGridView ID="AvailableGrid" runat="server" Width="100%" style="table-layout:fixed;"
                            EmptyDataText="There are no data records to display." KeyFieldName = "CODE"                     
                            AllowPaging="True" DataSourceID="dsAvailable" AutoGenerateColumns="False"><Columns><dx:GridViewCommandColumn ShowSelectCheckbox="True" VisibleIndex="0"><HeaderTemplate><input type="checkbox" onclick="AvailableGrid.SelectAllRowsOnPage (this.checked);" title="Select/Unselect all rows on the page" /></HeaderTemplate><HeaderStyle HorizontalAlign="Center" /></dx:GridViewCommandColumn><dx:GridViewDataColumn FieldName="CODE" VisibleIndex="1" ><Settings AutoFilterCondition="Contains" /></dx:GridViewDataColumn><dx:GridViewDataColumn FieldName="NAME" VisibleIndex="2" ><Settings AutoFilterCondition="Contains" /></dx:GridViewDataColumn></Columns><Settings ShowFilterRow="True" ShowFilterBar="Auto" ShowFilterRowMenu="True" /></dx:ASPxGridView>   
                    </div>  
                    <asp:SqlDataSource ID="dsAvailable" runat="server" />
                </td>
            </tr>
        </table>
    </asp:Panel>               
        
    <%--<asp:Panel ID="pnlView" runat="server" Visible="false">
        
    </asp:Panel>--%>
  
    <asp:SqlDataSource ID="dsMaterialType" runat="server"       
        SelectCommand="Select MaterialType From MsMaterialType">
    </asp:SqlDataSource>

    <%--<asp:SqlDataSource ID="dsBank" runat="server"       
        SelectCommand="EXEC S_GetBank">
    </asp:SqlDataSource>--%>

    <asp:Label ID="lbstatus" ForeColor="red" runat="server"></asp:Label>

    </div>
    </form>
    </body>
</html>
