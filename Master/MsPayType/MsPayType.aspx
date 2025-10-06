<%@ Page Language="VB" AutoEventWireup="false" CodeFile="MsPayType.aspx.vb" Inherits="Master_MsPayType_MsPayType" %>

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
 <link href="https://fonts.googleapis.com/css2?family=Muli:wght@300;400;600&display=swap" rel="stylesheet">
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div class="Content">
     <div class="H1">Payment Type File</div>
     <hr style="color:Blue" />
     <asp:Panel id="pnlHd" runat="server">
      <table>
        <tr>
            <td style="text-align:right; width:100px" >Quick Search :
            </td>
            <td><asp:TextBox CssClass="TextBox"  runat="server" ID ="tbFilter"/> 
                <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlField" >
                  <asp:ListItem Selected="true" Text="Payment Type Code" Value="PayCode"></asp:ListItem>
                  <asp:ListItem Text="Payment Type Name" Value="PayName"></asp:ListItem>
                      <asp:ListItem Text="Account" Value="Account"></asp:ListItem>
                      <asp:ListItem Text="Account Name" Value="AccountName"></asp:ListItem>
                      <asp:ListItem Text="Currency" Value="CurrCode"></asp:ListItem>  
                      <asp:ListItem Text="Mode" Value="FgModeName"></asp:ListItem>  
                      <asp:ListItem Text="Type" Value="FgTypeName"></asp:ListItem>  
                      <asp:ListItem Text="Bank" Value="BankName"></asp:ListItem> 
                      <asp:ListItem Text="Voucher No In" Value="VoucherNo"></asp:ListItem> 
                      <asp:ListItem Text="Voucher No Out" Value="VoucherNoOut"></asp:ListItem> 
                      <asp:ListItem Text="No Rekening" Value="NoRekening"></asp:ListItem> 
                      <asp:ListItem Text="Rekening Name" Value="NamaRekening"></asp:ListItem>                                            
                      <asp:ListItem Text="Swift Code" Value="SwiftCode"></asp:ListItem> 
                      <asp:ListItem Text="Bank Branch" Value="BankBranch"></asp:ListItem> 
                      <asp:ListItem Text="Bank Address" Value="BankAddr"></asp:ListItem>                       
                      <asp:ListItem Text="Bank Phone" Value="BankPhone"></asp:ListItem>                       
                      <asp:ListItem Text="Bank Fax" Value="BankFax"></asp:ListItem>                       
                      <asp:ListItem Text="Contact Person" Value="ContactPerson"></asp:ListItem>                       
                      <asp:ListItem Text="Contact Address" Value="ContactAddr"></asp:ListItem>                       
                      <asp:ListItem Text="Contact Phone" Value="ContactPhone"></asp:ListItem>
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
                      <asp:ListItem Selected="true" Text="Payment Type Code" Value="PayCode"></asp:ListItem>
                      <asp:ListItem Text="Payment Type Name" Value="PayName"></asp:ListItem>
                      <asp:ListItem Text="Account" Value="Account"></asp:ListItem>
                      <asp:ListItem Text="Account Name" Value="AccountName"></asp:ListItem>
                      <asp:ListItem Text="Currency" Value="CurrCode"></asp:ListItem>  
                      <asp:ListItem Text="Mode" Value="FgModeName"></asp:ListItem>  
                      <asp:ListItem Text="Type" Value="FgTypeName"></asp:ListItem>  
                      <asp:ListItem Text="Bank" Value="BankName"></asp:ListItem> 
                      <asp:ListItem Text="Voucher No In" Value="VoucherNo"></asp:ListItem> 
                      <asp:ListItem Text="Voucher No Out" Value="VoucherNoOut"></asp:ListItem> 
                      <asp:ListItem Text="No Rekening" Value="NoRekening"></asp:ListItem> 
                      <asp:ListItem Text="Rekening Name" Value="NamaRekening"></asp:ListItem>                                            
                      <asp:ListItem Text="Swift Code" Value="SwiftCode"></asp:ListItem> 
                      <asp:ListItem Text="Bank Branch" Value="BankBranch"></asp:ListItem> 
                      <asp:ListItem Text="Bank Address" Value="BankAddr"></asp:ListItem>                       
                      <asp:ListItem Text="Bank Phone" Value="BankPhone"></asp:ListItem>                       
                      <asp:ListItem Text="Bank Fax" Value="BankFax"></asp:ListItem>                       
                      <asp:ListItem Text="Contact Person" Value="ContactPerson"></asp:ListItem>                       
                      <asp:ListItem Text="Contact Address" Value="ContactAddr"></asp:ListItem>                       
                      <asp:ListItem Text="Contact Phone" Value="ContactPhone"></asp:ListItem>                       
                 </asp:DropDownList>               
            </td>
        </tr>
     </table>
     </asp:Panel>
      <br />
      <div style="border:0px  solid; width:100%; height:100%; overflow:auto;">
      <asp:Button class="bitbtn btnadd" runat="server" ID="btnAdd" Text="Add" />	
      <asp:GridView id="DataGrid" runat="server" ShowFooter="True" AllowSorting="True" 
            AutoGenerateColumns="False" AllowPaging="True" CssClass="Grid">
						<HeaderStyle CssClass="GridHeader" wrap="False"></HeaderStyle>
						<RowStyle CssClass="GridItem" Wrap = "false"/>
						<AlternatingRowStyle CssClass="GridAltItem"/>
						<FooterStyle CssClass="GridFooter" />
						<PagerStyle CssClass="GridPager" />
				      <Columns>
				      <asp:TemplateField HeaderStyle-Width="110">
                            <ItemTemplate>
                                <asp:DropDownList CssClass="DropDownList" ID="ddl" runat="server">
                                <asp:ListItem Selected="True" Text="View"/>
                                <asp:ListItem Text="Edit" />
                                <asp:ListItem Text ="Delete"/>
                                <asp:ListItem Text="Detail"/>
                                <asp:ListItem Text="User"/>
                                </asp:DropDownList>
                                <asp:Button class="btngo" runat="server" ID="btnGO" Text="G" CommandName="Go" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>"/>                               
                            </ItemTemplate>
                      <HeaderStyle Width="110px" />
                      </asp:TemplateField>
				            <asp:BoundField DataField="PayCode" HeaderText="Payment Type Code" HeaderStyle-Width="150px" SortExpression="PayCode"><HeaderStyle Width="150px" /></asp:BoundField>
							<asp:BoundField DataField="PayName" HeaderText="Payment Type Name" HeaderStyle-Width="200px" SortExpression="PayName"><HeaderStyle Width="200px" /></asp:BoundField>
							<asp:BoundField DataField="Account" HeaderText="Account" HeaderStyle-Width="150px" SortExpression="Account"><HeaderStyle Width="150px" /></asp:BoundField>
							<asp:BoundField DataField="AccountName" HeaderText="Account Name" HeaderStyle-Width="200px" SortExpression="AccountName"><HeaderStyle Width="200px" /></asp:BoundField>
							<asp:BoundField DataField="CurrCode" HeaderText="Currency" HeaderStyle-Width="30px" SortExpression="CurrCode" ><HeaderStyle Width="30px" /></asp:BoundField>
							<asp:BoundField DataField="FgModeName" HeaderText="Mode" HeaderStyle-Width="80px" SortExpression="FgModeName"><HeaderStyle Width="80px" /></asp:BoundField>
							<asp:BoundField DataField="FgTypeName" HeaderText="Type" HeaderStyle-Width="80px" SortExpression="FgTypeName"><HeaderStyle Width="80px" /></asp:BoundField>
							<asp:BoundField DataField="BankName" HeaderText="Bank" HeaderStyle-Width="150px" SortExpression="BankName" ><HeaderStyle Width="150px" /></asp:BoundField>
							<asp:BoundField DataField="VoucherNo" HeaderText="Voucher No In" HeaderStyle-Width="150px" SortExpression="VoucherNo" ><HeaderStyle Width="150px" /></asp:BoundField>
							<asp:BoundField DataField="VoucherNoOut" HeaderText="Voucher No Out" HeaderStyle-Width="150px" SortExpression="VoucherNoOut" ><HeaderStyle Width="150px" /></asp:BoundField>
							<asp:BoundField DataField="NoRekening" HeaderText="No Rekening" HeaderStyle-Width="200px" SortExpression="NoRekening" ><HeaderStyle Width="200px" /></asp:BoundField>
							<asp:BoundField DataField="NamaRekening" HeaderText="Rekening Name" HeaderStyle-Width="200px" SortExpression="NamaRekening" ><HeaderStyle Width="200px" /></asp:BoundField>
							<asp:BoundField DataField="SwiftCode" HeaderText="Swift Code" HeaderStyle-Width="150px" SortExpression="SwiftCode" ><HeaderStyle Width="150px" /></asp:BoundField>
							<asp:BoundField DataField="BankBranch" HeaderText="Bank Branch" HeaderStyle-Width="150px" SortExpression="BankBranch" ><HeaderStyle Width="150px" /></asp:BoundField>
							<asp:BoundField DataField="BankAddr" HeaderText="Bank Address" HeaderStyle-Width="200px" SortExpression="BankAddr" ><HeaderStyle Width="200px" /></asp:BoundField>
							<asp:BoundField DataField="BankPhone" HeaderText="Bank Phone" HeaderStyle-Width="150px" SortExpression="BankPhone" ><HeaderStyle Width="150px" /></asp:BoundField>
							<asp:BoundField DataField="BankFax" HeaderText="Bank Fax" HeaderStyle-Width="150px" SortExpression="BankFax" ><HeaderStyle Width="150px" /></asp:BoundField>
							<asp:BoundField DataField="ContactPerson" HeaderText="Contac Person" HeaderStyle-Width="150px" SortExpression="ContactPerson" ><HeaderStyle Width="150px" /></asp:BoundField>
							<asp:BoundField DataField="ContactAddr" HeaderText="Contact Address" HeaderStyle-Width="200px" SortExpression="ContactAddr" ><HeaderStyle Width="200px" /></asp:BoundField>
							<asp:BoundField DataField="ContactPhone" HeaderText="Contact Phone" HeaderStyle-Width="150px" SortExpression="ContactPhone" ><HeaderStyle Width="150px" /></asp:BoundField>
							<asp:BoundField DataField="CostCtrName" HeaderText="Cost Ctr" HeaderStyle-Width="150px" SortExpression="CostCtrName" ><HeaderStyle Width="150px" /></asp:BoundField>
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
        <asp:Panel runat="server" ID="PnelNav" Visible="false">
        <asp:Button class="bitbtn btnadd" runat="server" ID="btnAdd2" Text="Add" />
        </asp:Panel>	
      </div>
    </asp:Panel>
     <asp:Panel runat="server" ID="pnlInput" Visible="false">
        <table>
            <tr>
                <td>Payment Type Code</td>
                <td>:</td>
                <td><asp:TextBox runat="server" CssClass="TextBox" ID="tbPayCode" MaxLength="5" 
                        ValidationGroup="Input" /></td>
            </tr>
            <tr>
                <td>Payment Type Name</td>
                <td>:</td>
                <td><asp:TextBox runat="server" MaxLength="60" CssClass="TextBox" ID="tbPayName" 
                        Width="250px" ValidationGroup="Input"/></td>
            </tr>
            <tr>
                <td>Account</td>
                <td>:</td>
                <td><asp:TextBox runat="server" MaxLength="12" CssClass="TextBox" ID="tbAccount" 
                        Width="127px" AutoPostBack="True" ValidationGroup="Input"/>
                    <asp:TextBox runat="server" MaxLength="60" CssClass="TextBoxR" ID="tbAccName" Width="250px" ReadOnly="True"/>
                    <asp:Button class="btngo" runat="server" ID="btnAcc" Text="..."/> 
                </td>
            </tr>
            <tr>
                <td>Currency</td>
                <td>:</td>
                <td><asp:DropDownList runat="server" CssClass="DropDownList" ID="ddlCurrCode" Width="160px" Enabled = "false">
                    <asp:ListItem Selected = "True">Choose One</asp:ListItem></asp:DropDownList></td>
            </tr>
            <tr>
                <td>Mode</td>
                <td>:</td>
                <td><asp:DropDownList runat="server" CssClass="DropDownList" ID="ddlFgmode" 
                        Width="160px" AutoPostBack ="true" >
                    <asp:ListItem Value="K">Kas</asp:ListItem>
                    <asp:ListItem Selected="True" Value="B">Bank</asp:ListItem>
                    <asp:ListItem Value="G">Giro</asp:ListItem>
                    <asp:ListItem Value="D">DP</asp:ListItem>
                    <%--<asp:ListItem Value="C">Charge</asp:ListItem>
                    <asp:ListItem Value="C">CN</asp:ListItem>
                    <asp:ListItem Value="N">DN</asp:ListItem>--%>
                    <asp:ListItem Value="I">Income</asp:ListItem>
                    <asp:ListItem Value="E">Expense</asp:ListItem>
                    <asp:ListItem Value="P">PB</asp:ListItem>
                    <asp:ListItem Value="O">Other</asp:ListItem>
                    
                    </asp:DropDownList>
                    </td> 
            </tr>
            <tr>
                <td>Type</td>
                <td>:</td>
                <td><asp:DropDownList runat="server" CssClass="DropDownList" ID="ddlType" 
                        Width="160px">
                    <asp:ListItem Value="P">Payment</asp:ListItem>
                    <asp:ListItem Value="R">Receipt</asp:ListItem>
                    <asp:ListItem Selected="True" Value="A">All</asp:ListItem>
                    </asp:DropDownList>							    
                </td>
            </tr>
            <tr>
                <td>Bank</td>
                <td>:</td>
                <td><asp:DropDownList runat="server" CssClass="DropDownList" ID="ddlBank" Width="160px">
                    <asp:ListItem Selected = "True">Choose One</asp:ListItem></asp:DropDownList></td>
            </tr>
            <tr>
                <td>Voucher No In/ Voucher No Out</td>
                <td>:</td>
                <td>
                    <asp:TextBox ID="tbVoucherNo" runat="server" CssClass="TextBox" MaxLength="30" 
                        Width="160px" /> / <asp:TextBox ID="tbVoucherNoOut" runat="server" CssClass="TextBox" MaxLength="30" 
                        Width="160px" />
                </td>
            </tr>
            <tr>
                <td>No Rekening</td>
                <td>:</td>
                <td><asp:TextBox runat="server" MaxLength="30" CssClass="TextBox" ID="tbNorek" 
                        Width="160px"/></td>
            </tr>
            <tr>
                <td>Rekening Name</td>
                <td>:</td>
                <td><asp:TextBox runat="server" MaxLength="60" CssClass="TextBox" 
                        ID="tbNamaRekening" Width="250px"/></td>
            </tr>
            <tr>
                <td>Swift Code</td>
                <td>:</td>
                <td><asp:TextBox runat="server" MaxLength="30" CssClass="TextBox" ID="tbSwift" 
                        Width="160px"/></td>
            </tr>
            <tr>
                <td>Bank Branch</td>
                <td>:</td>
                <td><asp:TextBox runat="server" MaxLength="60" CssClass="TextBox" ID="tbBankBranch" 
                        Width="250px"/></td>
            </tr>
            <tr>
                <td>Bank Address</td>
                <td>:</td>
                <td><asp:TextBox runat="server" MaxLength="100" CssClass="TextBox" ID="tbBankAddr" 
                        Width="280px"/></td>
            </tr>
            <tr>
                <td>Bank Phone</td>
                <td>:</td>
                <td><asp:TextBox runat="server" MaxLength="30" CssClass="TextBox" ID="tbBankPhone" 
                        Width="160px"/></td>
            </tr>
            <tr>
                <td>Bank Fax</td>
                <td>:</td>
                <td><asp:TextBox runat="server" MaxLength="30" CssClass="TextBox" ID="tbBankFax" 
                        Width="160px"/></td>
            </tr>
            <tr>
                <td>Contact Person</td>
                <td>:</td>
                <td><asp:TextBox runat="server" MaxLength="60" CssClass="TextBox" ID="tbCP" 
                        Width="250px"/></td>
            </tr>
            <tr>
                <td>Contact Address</td>
                <td>:</td>
                <td><asp:TextBox runat="server" MaxLength="100" CssClass="TextBox" ID="tbCPAddress" 
                        Width="280px"/></td>
            </tr>
            <tr>
                <td>Contact Phone</td>
                <td>:</td>
                <td>
                    <asp:TextBox ID="tbCPPhone" runat="server" CssClass="TextBox" MaxLength="30" 
                        Width="160px" />
                </td>                    
            </tr>
            <tr>
                <td>
                    Cost Ctr</td>
                <td>
                    :</td>
                <td>
                    <asp:DropDownList ID="ddlCostCtr" runat="server" CssClass="DropDownList" 
                        Width="160px" />
                </td>
            </tr>
            <tr>
                <td align="center" colspan="3">
                    <asp:Button ID="BtnSave" runat="server" class="bitbtndt btnsave" 
                        CommandName="Update" Text="Save" ValidationGroup="Input" />
                    &nbsp;
                    <asp:Button ID="btnReset" runat="server" class="bitbtndt btncancel" 
                        CommandName="Cancel" Text="Reset" ValidationGroup="Input" />
                    &nbsp;
                    <asp:Button ID="btnCancel" runat="server" class="bitbtndt btncancel" 
                        CommandName="Cancel" Text="Back" />
                    &nbsp;
                </td>
            </tr>
        </table>
      </asp:Panel>         
     <asp:Panel ID="pnlDt" runat="server" Visible = "false">  
     <asp:Label ID="label1" CssClass="H1" runat="server" Text="Payment Type : " />   
     <asp:Label ID="lbPayType" ForeColor="Blue" CssClass="H1" Font-Bold="true" runat="server" />
	 <br />
	<asp:Button class="bitbtn btnback" runat="server" ID="btnBackDtTop" Text="Back" />
	<br />
	 
	    <div style="border:0px  solid; width:100%; height:100%; overflow:auto;">
        <asp:GridView id="DataGridDt" runat="server" ShowFooter="True" AllowSorting="True" 
            AutoGenerateColumns="False" AllowPaging="True">
						<HeaderStyle CssClass="GridHeader" wrap="false"></HeaderStyle>
						<RowStyle CssClass="GridItem" wrap="false"/>
						<AlternatingRowStyle CssClass="GridAltItem"/>
						<FooterStyle CssClass="GridFooter" Wrap="false" />
						<PagerStyle CssClass="GridPager" />
				      <Columns>			            
						
							<asp:TemplateField HeaderText="Currency" HeaderStyle-Width="80" SortExpression="Currency">
								<Itemtemplate>
									<asp:Label Runat="server" Width="80" ID="CurrCodeDt" text='<%# DataBinder.Eval(Container.DataItem, "Currency") %>'>
									</asp:Label>
								</Itemtemplate>			
								<EditItemTemplate>
								    <asp:Label runat="server" ID="CurrCodeDtEdit" text='<%# DataBinder.Eval(Container.DataItem, "Currency") %>' />
								</EditItemTemplate>	
								<FooterTemplate>								    
								    <asp:DropDownList ID="CurrCodeDtAdd" CssClass="DropDownList" runat="server" DataSourceID="dsCurrency" Width="100%" 
                                        DataTextField="Currency" DataValueField="Currency" AutoPostBack="true"
                                        onselectedindexchanged="CurrCodeDtAdd_SelectedIndexChanged">
                                    </asp:DropDownList>								    
								</FooterTemplate>											
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Acc. Bank Charge" HeaderStyle-Width="128" SortExpression="AccBankCharge">
								<Itemtemplate>
									<asp:Label Runat="server" Width="128" ID="AccBankCharge" TEXT='<%# DataBinder.Eval(Container.DataItem, "AccBankCharge") %>'>
									</asp:Label>
								</Itemtemplate>	
								<EditItemTemplate>
								    <asp:TextBox CssClass="TextBox" runat="server" Width="90" ID="AccBankChargeEdit" 
                                        TEXT='<%# DataBinder.Eval(Container.DataItem, "AccBankCharge") %>' 
                                        AutoPostBack="true" ontextchanged="tbAccBankCharge_TextChanged"/>
                                    <asp:Button class="btngo" runat="server" ID="btnAccBankChargeEdit" Text="..." CommandName="SearchBankChargeEdit"/>                               
								</EditItemTemplate>							
								<FooterTemplate>
								    <asp:TextBox CssClass="TextBox" OnTextChanged="tbAccBankCharge_TextChanged" runat="server" id="AccBankChargeAdd" Width="90" 
                                        AutoPostBack="true" />
                                     <asp:Button class="btngo" runat="server" ID="btnAccBankChargeAdd" Text="..." CommandName="SearchBankChargeAdd"/>
                                     <cc1:TextBoxWatermarkExtender ID="AccBankChargeAdd_TextBoxWatermarkExtender" 
                                        runat="server" Enabled="True" TargetControlID="AccBankChargeAdd" 
                                        WatermarkText="[Acc Bank Charge]" WatermarkCssClass="Watermarked">
                                    </cc1:TextBoxWatermarkExtender>
								</FooterTemplate>
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Acc. Bank Charge Name" HeaderStyle-Width="280" SortExpression="AccBankChargeName">
								<Itemtemplate>
									<asp:Label Runat="server" Width="280" ID="AccBankChargeName" TEXT='<%# DataBinder.Eval(Container.DataItem, "AccBankChargeName") %>'>
									</asp:Label>
								</Itemtemplate>	
								<EditItemTemplate>
								    <asp:Label Runat="server" ID="AccBankChargeNameEdit" Width="100%" TEXT='<%# DataBinder.Eval(Container.DataItem, "AccBankChargeName") %>'>
									</asp:Label>
								</EditItemTemplate>	
								<FooterTemplate>
								    <asp:Label Runat="server" ID="AccBankChargeNameAdd" Width="280" >
									</asp:Label>
								</FooterTemplate>								
							</asp:TemplateField>	
							
							<asp:TemplateField HeaderText="Expense Charge" HeaderStyle-Width="70" SortExpression="ExpenseCharge" ItemStyle-HorizontalAlign = "Right">
								<Itemtemplate>
									<asp:Label Runat="server" Width="70" ID="ExpenseCharge" TEXT='<%# DataBinder.Eval(Container.DataItem, "ExpenseCharge") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:TextBox Runat="server" ID="ExpenseChargeEdit" CssClass="TextBox" Width="90%" Text='<%# DataBinder.Eval(Container.DataItem, "ExpenseCharge") %>'>
									</asp:TextBox>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="ExpenseChargeAdd" CssClass="TextBox" Width="90%" Runat="Server"/>
								    <cc1:TextBoxWatermarkExtender ID="ExpenseChargeAdd_TextBoxWatermarkExtender" 
                                        runat="server" Enabled="True" TargetControlID="ExpenseChargeAdd" WatermarkText="can't blank" WatermarkCssClass="Watermarked" >
                                    </cc1:TextBoxWatermarkExtender>
								</FooterTemplate>
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Default" HeaderStyle-Width="30" ItemStyle-HorizontalAlign="Center" SortExpression="FgDefault">
								<Itemtemplate>
									<asp:Label Runat="server" Width="30" ID="Default" TEXT='<%# DataBinder.Eval(Container.DataItem, "FgDefault") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:DropDownList ID="DefaultEdit" CssClass="DropDownList" Width="100%" runat="server" SelectedValue='<%# DataBinder.Eval(Container.DataItem, "FgDefault") %>'>
									  <asp:ListItem>Y</asp:ListItem>
									  <asp:ListItem>N</asp:ListItem>                                         
									</asp:DropDownList>								    
								</EditItemTemplate>
								<FooterTemplate>
									<asp:DropDownList ID="DefaultAdd" CssClass="DropDownList" Width="100%" runat="server">
									  <asp:ListItem Selected="True">Y</asp:ListItem>
									  <asp:ListItem>N</asp:ListItem>                                        
									</asp:DropDownList>								    
								</FooterTemplate>
							</asp:TemplateField>
						       				  																	
																																						
							<asp:TemplateField HeaderText="Action" HeaderStyle-Width="126px" ItemStyle-Wrap="false">
								<ItemTemplate>
								    <asp:Button ID="btnEdit" runat="server" class="bitbtndt btnedit" Text="Edit" CommandName="Edit"/>
							        <asp:Button ID="btnDelete" runat="server" class="bitbtndt btndelete" Text="Delete" OnClientClick="return confirm('Sure to delete this data?');" CommandName="Delete"/>									
                        	    </ItemTemplate>																
								<EditItemTemplate>
								    <asp:Button ID="btnUpdate" runat="server" class="bitbtndt btnsave" Text="Save" CommandName="Update"/>									
                                    <asp:Button ID="btnCancel" runat="server" class="bitbtndt btncancel" Text="Cancel" CommandName="Cancel"/>								
								</EditItemTemplate>
								<FooterTemplate>
									<asp:Button class="bitbtndt btnadd" runat="server" ID="btnAdd" Text="Add" CommandName="Insert"/>
								</FooterTemplate>
							</asp:TemplateField>							
    					</Columns>
        </asp:GridView>  
        </div>        
	    <asp:Button class="bitbtn btnback" runat="server" ID="Button2" Text="Back" />
     </asp:Panel>   
     </div>   
  
    <asp:SqlDataSource ID="dsCurrency" runat="server"       
      SelectCommand="EXEC S_GetCurrency">
    </asp:SqlDataSource>

    <asp:SqlDataSource ID="dsBank" runat="server"       
      SelectCommand="EXEC S_GetBank">
    </asp:SqlDataSource>

    <asp:Label ID="lstatus" ForeColor="red" runat="server"></asp:Label>

    </form>
    </body>
</html>
