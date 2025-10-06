<%@ Page Language="VB" AutoEventWireup="false" CodeFile="MsSuppType.aspx.vb" Inherits="Master_MsSuppType_MsSuppType" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Supplier Type File</title>    
    <script src="../../Function/OpenDlg.js" type="text/javascript"></script>    
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
 <link href="https://fonts.googleapis.com/css2?family=Muli:wght@300;400;600&display=swap" rel="stylesheet">
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div class="Content">
     <div class="H1">Supplier Type File</div>
     <hr style="color:Blue" />
     <asp:Panel id="pnlHd" runat="server">
      <table>
        <tr>
            <td style="text-align:right; width:100px" >Quick Search :
            </td>
            <td><asp:TextBox CssClass="TextBox"  runat="server" ID ="tbFilter"/> 
                <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlField" >
                  <asp:ListItem Selected="true" Text="Supplier Type Code" Value="SuppTypeCode"></asp:ListItem>
                  <asp:ListItem Text="Supplier Type Name" Value="SuppTypeName"></asp:ListItem>
                  <asp:ListItem Text="Group Type" Value="GroupType"></asp:ListItem>  
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
                      <asp:ListItem Selected="true" Text="Supplier Type Code" Value="SuppTypeCode"></asp:ListItem>
                      <asp:ListItem Text="Supplier Type Name" Value="SuppTypeName"></asp:ListItem>
                      <asp:ListItem Text="Group Type" Value="GroupType"></asp:ListItem>
                  </asp:DropDownList>
                
            </td>
        </tr>
     </table>
     </asp:Panel>
      <br />
      <div style="border:0px  solid; width:100%; height:100%; overflow:auto;"/>
      <asp:GridView id="DataGrid" runat="server" ShowFooter="True" AllowSorting="True" 
            AutoGenerateColumns="False" AllowPaging="True" CssClass="Grid">
						<HeaderStyle CssClass="GridHeader" wrap="false"></HeaderStyle>
						<RowStyle CssClass="GridItem" Wrap="false"/>
						<AlternatingRowStyle CssClass="GridAltItem"/>
						<FooterStyle CssClass="GridFooter" />
						<PagerStyle CssClass="GridPager" />
				      <Columns>
							<asp:TemplateField HeaderText="Supplier Type Code" HeaderStyle-Width="70" SortExpression="SuppTypeCode">
								<Itemtemplate>
									<asp:Label Runat="server" Width="70" ID="SuppTypeCode" text='<%# DataBinder.Eval(Container.DataItem, "SuppTypeCode") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:Label Runat="server" ID="SuppTypeCodeEdit" Width="100%" Text='<%# DataBinder.Eval(Container.DataItem, "SuppTypeCode") %>'>
									</asp:Label>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="SuppTypeCodeAdd" CssClass="TextBox" placeholder="can't blank" Width="100%" MaxLength="5" Runat="Server"/>
								    <%--<cc1:TextBoxWatermarkExtender ID="SuppTypeCodeAdd_TextBoxWatermarkExtender" 
                                        runat="server" Enabled="True" TargetControlID="SuppTypeCodeAdd" WatermarkText="can't blank" WatermarkCssClass="Watermarked" >
                                    </cc1:TextBoxWatermarkExtender>--%>
								</FooterTemplate>
							    <HeaderStyle Width="70px" />
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Supplier Type Name" HeaderStyle-Width="280" SortExpression="SuppTypeName">
								<Itemtemplate>
									<asp:Label Runat="server" Width="280" ID="SuppTypeName" text='<%# DataBinder.Eval(Container.DataItem, "SuppTypeName") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:TextBox Runat="server" ID="SuppTypeNameEdit" CssClass="TextBox" Width="100%" Text='<%# DataBinder.Eval(Container.DataItem, "SuppTypeName") %>'>
									</asp:TextBox>
									<%--<cc1:TextBoxWatermarkExtender ID="SuppTypeNameEdit_TextBoxWatermarkExtender" 
                                        runat="server" Enabled="True" TargetControlID="SuppTypeNameEdit" WatermarkText="can't blank" WatermarkCssClass="Watermarked" >
                                    </cc1:TextBoxWatermarkExtender>--%>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="SuppTypeNameAdd" CssClass="TextBox" placeholder="can't blank" MaxLength="60" Width="100%" Runat="Server"/>
									<%--<cc1:TextBoxWatermarkExtender ID="SuppTypeNameAdd_TextBoxWatermarkExtender" 
                                        runat="server" Enabled="True" TargetControlID="SuppTypeNameAdd" WatermarkText="can't blank" WatermarkCssClass="Watermarked" >
                                    </cc1:TextBoxWatermarkExtender>--%>
								</FooterTemplate>
							    <HeaderStyle Width="280px" />
							</asp:TemplateField>								
														
							<asp:TemplateField HeaderText="Group Type" HeaderStyle-Width="30" ItemStyle-HorizontalAlign="Left" SortExpression="GroupType">
								<Itemtemplate>
									<asp:Label Runat="server" Width="30" ID="GroupType" text='<%# DataBinder.Eval(Container.DataItem, "GroupType") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:DropDownList ID="GroupTypeEdit" CssClass="DropDownList" Width="100%" runat="server" SelectedValue='<%# DataBinder.Eval(Container.DataItem, "GroupType") %>'>
										
									  <asp:ListItem>Import</asp:ListItem>
									  <asp:ListItem>Lokal</asp:ListItem>                                        
									    <asp:ListItem>Affiliasi</asp:ListItem>
										<asp:ListItem>Panen</asp:ListItem>
										<asp:ListItem>PKS</asp:ListItem>
									</asp:DropDownList>								    
								</EditItemTemplate>
								<FooterTemplate>
									<asp:DropDownList ID="GroupTypeAdd" CssClass="DropDownList" Width="100%" runat="server">
									  <asp:ListItem>Import</asp:ListItem>
									  <asp:ListItem Selected="True">Lokal</asp:ListItem>                                        
									    <asp:ListItem>Affiliasi</asp:ListItem>
									</asp:DropDownList>								    
								</FooterTemplate>
							    <HeaderStyle Width="30px" />
                                <ItemStyle HorizontalAlign="Left" />
							</asp:TemplateField>								
							
							<asp:TemplateField HeaderText="Action" headerstyle-width="180" >
							    <ItemTemplate>
								    <asp:Button ID="btnEdit" runat="server" class="bitbtndt btnedit" Text="Edit" CommandName="Edit"/>
									<asp:Button ID="btndelete" runat="server" class="bitbtndt btndelete" Text="Delete" OnClientClick="return confirm('Sure to delete this data?');" CommandName="Delete"/>									
									<asp:Button ID="btnView" runat="server" class="bitbtndt btnedit" Text="Account" CommandName="View" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" Width = "65"/>									
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
     <asp:Label ID="label1" CssClass="H1" runat="server" Text="Supplier Type : " />   
     <asp:Label ID="lbGroupTypeCode" ForeColor="Blue" CssClass="H1" Font-Bold="true" runat="server" />
     </asp:Panel>
	 <br />
     <asp:Panel ID="pnlDt" runat="server" Visible = "false">  
     <asp:Button class="bitbtn btnback" runat="server" ID="btnBackDtTop" Text="Back" /> &nbsp;
	 <asp:Button class="bitbtn btnadd" runat="server" ID="btnAdd" Text="Add" />
	 <br />
	 
	    <div style="border:0px  solid; width:100%; height:100%; overflow:auto;">
        <asp:GridView id="DataGridDt" runat="server" AllowSorting="True" 
            AutoGenerateColumns="False" AllowPaging="True" ShowFooter="False" >
						<HeaderStyle CssClass="GridHeader" wrap="false"></HeaderStyle>
						<RowStyle CssClass="GridItem" wrap="false"/>
						<AlternatingRowStyle CssClass="GridAltItem"/>
						<FooterStyle CssClass="GridFooter"/>
						<PagerStyle CssClass="GridPager" />
				      <Columns>
				      
				            <asp:BoundField DataField="CurrCode" HeaderText="Currency" 
                                HeaderStyle-Width="80" SortExpression="CurrCode">
							    <HeaderStyle Width="80px" />
                            </asp:BoundField>
							<asp:BoundField DataField="Accap" HeaderText="Acc. AP" HeaderStyle-Width="128" 
                                SortExpression="Accap">
							    <HeaderStyle Width="128px" />
                            </asp:BoundField>
							<asp:BoundField DataField="AccapName" HeaderText="Acc. AP Name" 
                                HeaderStyle-Width="280" SortExpression="AccapName">
							    <HeaderStyle Width="280px" />
                            </asp:BoundField>
							<asp:BoundField DataField="Accappending" HeaderText="Acc. AP Pending" 
                                HeaderStyle-Width="128" SortExpression="Accappending" >
							    <HeaderStyle Width="128px" />
                            </asp:BoundField>
							<asp:BoundField DataField="AccappendingName" HeaderText="Acc. AP Pending Name" 
                                HeaderStyle-Width="280" SortExpression="AccappendingName">
							    <HeaderStyle Width="280px" />
                            </asp:BoundField>
							<asp:BoundField DataField="AccDebitAP" HeaderText="Acc. Debit AP" 
                                HeaderStyle-Width="128" SortExpression="AccDebitAP">
							    <HeaderStyle Width="128px" />
                            </asp:BoundField>
							<asp:BoundField DataField="AccDebitAPName" HeaderText="Acc. Debit AP Name" 
                                HeaderStyle-Width="280" SortExpression="AccDebitAPName" >						
							    <HeaderStyle Width="280px" />
                            </asp:BoundField>
							<asp:BoundField DataField="AccDP" HeaderText="Acc. DP" HeaderStyle-Width="128" 
                                SortExpression="AccDP" >							
							    <HeaderStyle Width="128px" />
                            </asp:BoundField>
							<asp:BoundField DataField="AccDPName" HeaderText="Acc. DP Name" 
                                HeaderStyle-Width="280" SortExpression="AccDPName" >							
							    <HeaderStyle Width="280px" />
                            </asp:BoundField>
							<asp:BoundField DataField="AccDeposit" HeaderText="Acc. Deposit" 
                                HeaderStyle-Width="128" SortExpression="AccDeposit" >							
							    <HeaderStyle Width="128px" />
                            </asp:BoundField>
							<asp:BoundField DataField="AccDepositName" HeaderText="Acc. Deposit Name" 
                                HeaderStyle-Width="280" SortExpression="AccDepositName" >							
							    <HeaderStyle Width="280px" />
                            </asp:BoundField>
							<asp:BoundField DataField="AccVariantPO" HeaderText="Acc. Variant PO" 
                                HeaderStyle-Width="128" SortExpression="AccVariantPO" >							
							    <HeaderStyle Width="128px" />
                            </asp:BoundField>
							<asp:BoundField DataField="AccVariantPOName" HeaderText="Acc. Variant PO Name" 
                                HeaderStyle-Width="280" SortExpression="AccVariantPOName" >							
							    <HeaderStyle Width="280px" />
                            </asp:BoundField>
							<asp:BoundField DataField="Accppn" HeaderText="Acc. PPN" 
                                HeaderStyle-Width="128" SortExpression="Accppn" >							
							    <HeaderStyle Width="128px" />
                            </asp:BoundField>
							<asp:BoundField DataField="AccPPNName" HeaderText="Acc. PPN Name" 
                                HeaderStyle-Width="280" SortExpression="AccPPNName" >							
							    <HeaderStyle Width="280px" />
                            </asp:BoundField>
							<asp:BoundField DataField="AccFreight" HeaderText="Acc. Freight" 
                                HeaderStyle-Width="128" SortExpression="AccFreight" >							
							    <HeaderStyle Width="128px" />
                            </asp:BoundField>
							<asp:BoundField DataField="AccFreightName" HeaderText="Acc. Freight Name" 
                                HeaderStyle-Width="280" SortExpression="AccFreightName" >							
							    <HeaderStyle Width="280px" />
                            </asp:BoundField>
							<asp:BoundField DataField="AccOther" HeaderText="Acc. Other" 
                                HeaderStyle-Width="128" SortExpression="AccOther" >							
							    <HeaderStyle Width="128px" />
                            </asp:BoundField>
							<asp:BoundField DataField="AccOtherName" HeaderText="Acc. Other Name" 
                                HeaderStyle-Width="280" SortExpression="AccOtherName" >							
							    <HeaderStyle Width="280px" />
                            </asp:BoundField>
							<asp:BoundField DataField="AccPPH" HeaderText="Acc. PPH" 
                                HeaderStyle-Width="128" SortExpression="AccPPH" >							
							    <HeaderStyle Width="128px" />
                            </asp:BoundField>
							<asp:BoundField DataField="AccPPHName" HeaderText="Acc. PPH Name" 
                                HeaderStyle-Width="280" SortExpression="AccPPHName" >							
							    <HeaderStyle Width="280px" />
                            </asp:BoundField>
							<asp:BoundField DataField="AccDisc" HeaderText="Acc. Disc" 
                                HeaderStyle-Width="128" SortExpression="AccDisc" >	
							    <HeaderStyle Width="128px" />
                            </asp:BoundField>
							<asp:BoundField DataField="AccDiscName" HeaderText="Acc. Disc Name" 
                                HeaderStyle-Width="280" SortExpression="AccDiscName" >						
							    <HeaderStyle Width="280px" />
                            </asp:BoundField>
							<asp:TemplateField HeaderText="Action" HeaderStyle-Width="126px" ItemStyle-Wrap="false">
							    <ItemTemplate>
								    <asp:Button ID="btnEdit" runat="server" class="bitbtndt btnedit" Text="Edit" CommandName="Edit"/>
									<asp:Button ID="btndelete" runat="server" class="bitbtndt btndelete" Text="Delete" OnClientClick="return confirm('Sure to delete this data?');" CommandName="Delete"/>									
								</ItemTemplate>								
							    <HeaderStyle Width="126px" />
                                <ItemStyle Wrap="False" />
							</asp:TemplateField>	
						</Columns>
        </asp:GridView>  
        </div>        
	    <asp:Button class="bitbtn btnback" runat="server" ID="Button2" Text="Back" /> &nbsp;
	    <asp:Button class="bitbtn btnadd" runat="server" ID="btnAdd2" Text="Add" />
	   </asp:Panel> 
	   <asp:Panel runat="server" ID="pnlInputDt" Visible="false">
        <table>
            <tr>
                <td>Currency</td>
                <td>:</td>
                <td><asp:DropDownList runat="server" CssClass="DropDownList" ID="ddlCurrCodeDt" 
                        Width="160px" AutoPostBack="True">
                    </asp:DropDownList></td>
            </tr>
            <tr>
                <td>Acc. AP</td>
                <td>:</td>
                <td><asp:TextBox runat="server" MaxLength="12" CssClass="TextBox" ID="tbAccAP" Width="127px" AutoPostBack="True"/>
                    <asp:TextBox runat="server" MaxLength="60" CssClass="TextBoxR" ID="tbAccAPName" Width="250px" ReadOnly="True"/>
                    <asp:Button class="btngo" runat="server" ID="btnAccAP" Text="..."/> 
                </td>
            </tr>
            <tr>
                <td>Acc. AP Pending</td>
                <td>:</td>
                <td><asp:TextBox runat="server" MaxLength="12" CssClass="TextBox" ID="tbAccAPPending" Width="127px" AutoPostBack="True"/>
                    <asp:TextBox runat="server" MaxLength="60" CssClass="TextBoxR" ID="tbAccAPPendingName" Width="250px" ReadOnly="True"/>
                    <asp:Button class="btngo" runat="server" ID="btnAccAPPending" Text="..."/> 
                </td>
            </tr>
            <tr>
                <td>Acc. Debit AP</td>
                <td>:</td>
                <td><asp:TextBox runat="server" MaxLength="12" CssClass="TextBox" ID="tbAccDebitAP" Width="127px" AutoPostBack="True"/>
                    <asp:TextBox runat="server" MaxLength="60" CssClass="TextBoxR" ID="tbAccDebitAPName" Width="250px" ReadOnly="True"/>
                    <asp:Button class="btngo" runat="server" ID="btnAccDebitAP" Text="..."/> </td>
            </tr>
            <tr>
                <td>Acc. DP</td>
                <td>:</td>
                <td><asp:TextBox runat="server" MaxLength="12" CssClass="TextBox" ID="tbAccDP" Width="127px" AutoPostBack="True"/>
                    <asp:TextBox runat="server" MaxLength="60" CssClass="TextBoxR" ID="tbAccDPName" Width="250px" ReadOnly="True"/>
                    <asp:Button class="btngo" runat="server" ID="btnAccDP" Text="..."/> 
                </td>
            </tr>
            <tr>
                <td>Acc. Deposit</td>
                <td>:</td>
                <td><asp:TextBox runat="server" MaxLength="12" CssClass="TextBox" ID="tbAccDeposit" Width="127px" AutoPostBack="True"/>
                    <asp:TextBox runat="server" MaxLength="60" CssClass="TextBoxR" ID="tbAccDepositName" Width="250px" ReadOnly="True"/>
                    <asp:Button class="btngo" runat="server" ID="btnAccDeposit" Text="..."/> 
                </td>
            </tr>
            <tr>
                <td>Acc. Variant PO</td>
                <td>:</td>
                <td><asp:TextBox runat="server" MaxLength="12" CssClass="TextBox" ID="tbAccVariantPO" Width="127px" AutoPostBack="True"/>
                    <asp:TextBox runat="server" MaxLength="60" CssClass="TextBoxR" ID="tbAccVariantPOName" Width="250px" ReadOnly="True"/>
                    <asp:Button class="btngo" runat="server" ID="btnAccVariantPO" Text="..."/> 
                </td>
            </tr>
            <tr>
                <td>Acc. PPN</td>
                <td>:</td>
                <td><asp:TextBox runat="server" MaxLength="12" CssClass="TextBox" ID="tbAccPPN" Width="127px" AutoPostBack="True"/>
                    <asp:TextBox runat="server" MaxLength="60" CssClass="TextBoxR" ID="tbAccPPNName" Width="250px" ReadOnly="True"/>
                    <asp:Button class="btngo" runat="server" ID="btnAccPPN" Text="..."/> 
                </td>
            </tr>
            <tr>
                <td>Acc. Freight</td>
                <td>:</td>
                <td><asp:TextBox runat="server" MaxLength="12" CssClass="TextBox" ID="tbAccFreight" Width="127px" AutoPostBack="True"/>
                    <asp:TextBox runat="server" MaxLength="60" CssClass="TextBoxR" ID="tbAccFreightName" Width="250px" ReadOnly="True"/>
                    <asp:Button class="btngo" runat="server" ID="btnAccFreight" Text="..."/> 
                </td>
            </tr>
            <tr>
                <td>Acc. Other</td>
                <td>:</td>
                <td><asp:TextBox runat="server" MaxLength="12" CssClass="TextBox" ID="tbAccOther" Width="127px" AutoPostBack="True"/>
                    <asp:TextBox runat="server" MaxLength="60" CssClass="TextBoxR" ID="tbAccOtherName" Width="250px" ReadOnly="True"/>
                    <asp:Button class="btngo" runat="server" ID="btnAccOther" Text="..."/> 
                </td>
            </tr>
            <tr>
                <td>Acc. PPH</td>
                <td>:</td>
                <td><asp:TextBox runat="server" MaxLength="12" CssClass="TextBox" ID="tbAccPPH" Width="127px" AutoPostBack="True"/>
                    <asp:TextBox runat="server" MaxLength="60" CssClass="TextBoxR" ID="tbAccPPHName" Width="250px" ReadOnly="True"/>
                    <asp:Button class="btngo" runat="server" ID="btnAccPPH" Text="..."/> 
                </td>
            </tr>
            <tr>
                <td>Acc. Disc</td>
                <td>:</td>
                <td><asp:TextBox runat="server" MaxLength="12" CssClass="TextBox" ID="tbAccDisc" Width="127px" AutoPostBack="True"/>
                    <asp:TextBox runat="server" MaxLength="60" CssClass="TextBoxR" ID="tbAccDiscName" Width="250px" ReadOnly="True"/>
                    <asp:Button class="btngo" runat="server" ID="btnAccDisc" Text="..."/> 
                </td>
            </tr>
            <tr>
                <td colspan="3" align="center">
                <asp:Button ID="BtnSave" runat="server" class="bitbtndt btnsave" Text="Save" CommandName="Update"/>&nbsp;									
                <asp:Button ID="btnCancel" runat="server" class="bitbtndt btncancel" Text="Cancel" CommandName="Cancel"/>&nbsp;									                                    
                <asp:Button ID="btnReset" runat="server" class="bitbtndt btncancel" Text="Reset" CommandName="Cancel"/>&nbsp;                     
                </td>
            </tr>
        </table>
      </asp:Panel>                
    </div>    
        <asp:SqlDataSource ID="dsCurrency" runat="server" SelectCommand="EXEC S_GetCurrency">                                        
        </asp:SqlDataSource>
        
    </form>
    <asp:Label ID="lstatus" ForeColor="red" runat="server"></asp:Label>
</body>
</html>
