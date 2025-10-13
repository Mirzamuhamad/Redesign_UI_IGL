<%@ Page Language="VB" AutoEventWireup="false" CodeFile="MsPayrollAll.aspx.vb" Inherits="Assign_MsPayrollAll_MsPayrollAll" %>
<%@ Register Assembly="DevExpress.Web.ASPxGridView.v10.2, Version=10.2.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxGridView" TagPrefix="dx" %>
<%@ Register assembly="DevExpress.Web.ASPxEditors.v10.2" namespace="DevExpress.Web.ASPxEditors" tagprefix="dx" %>
<%@ Register Assembly="BasicFrame.WebControls.BasicDatePicker" Namespace="BasicFrame.WebControls"
    TagPrefix="BDP" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Untitled Page</title>
    <script src="../../Function/Function.JS" type="text/javascript"></script>
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
    <div class="Content">   
    <div class="H1">Payroll Slip All</div>
    <hr style="color:Blue" /> 
        <br />                     
        
              <table width="100%">
                <tr>
                    <td style="width:100px">Payroll</td>
                    <td>:</td>
                    <td>
                        <asp:TextBox CssClass="TextBox" Id="tbCode" runat="server" Width="120px" AutoPostBack="True"/>
                        <asp:TextBox CssClass="TextBox" ID="tbName" runat="server" Width="200px" Enabled="False" /> 
                        <asp:Button ID="btnSearch" runat="server" class="btngo" Text="..."/> 
                        <asp:Button class="bitbtn btnprint" runat="server" ID="btnPrint" Text="Print" />            
                        <asp:Button class="bitbtn btnadd" runat="server" ID="btnNewSlip" Text="New Slip" Visible="false"/>                                                                                 
                    </td>
                    
                </tr>                 
              </table>  
               <asp:Panel runat="server" ID="PnlAssign">          
               <div style="border:0px  solid; width:100%; height:100%; overflow:auto;">
                    <asp:GridView id="DataGrid" runat="server" ShowFooter="True" AllowSorting="True" 
                            AutoGenerateColumns="False" AllowPaging="True" CssClass="Grid">
						<HeaderStyle CssClass="GridHeader" wrap="false"></HeaderStyle>
						<RowStyle CssClass="GridItem" Wrap="false"/>
						<AlternatingRowStyle CssClass="GridAltItem"/>
						<FooterStyle CssClass="GridFooter" />
						<PagerStyle CssClass="GridPager" />
				      <Columns>
							<asp:TemplateField HeaderText="Eff. Date" HeaderStyle-Width="50" SortExpression="StartDate">
								<Itemtemplate>									
									<asp:Label Runat="server" Width="100%"  ID="EffDate" text='<%# DataBinder.Eval(Container.DataItem, "Start_Date") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
								    <BDP:BasicDatePicker ID="EffDateEdit" runat="server" DateFormat="dd MMM yyyy" 
                                        ReadOnly = "false" ValidationGroup="Input" Width="100%" 
                                        ButtonImageHeight="19px" ButtonImageWidth="20px" 
                                        DisplayType="TextBoxAndImage" SelectedValue = '<%# DataBinder.Eval(Container.DataItem, "StartDate") %>' 
                                        TextBoxStyle-CssClass="TextDate" 
                                        ShowNoneButton="False"><TextBoxStyle CssClass="TextDate" /></BDP:BasicDatePicker> 
									<%--<asp:Label Runat="server" ID="EffDateEdit" MaxLength="60" CssClass="TextBox" Width="100%" Text='<%# DataBinder.Eval(Container.DataItem, "StartDate") %>'>
									</asp:Label>--%>								
								</EditItemTemplate>
								<FooterTemplate>
									<BDP:BasicDatePicker ID="EffDateAdd" runat="server" DateFormat="dd/MM/yyyy" 
                                      ReadOnly = "true" Width="100%" 
                                      ButtonImageHeight="19px" ButtonImageWidth="20px" 
                                      DisplayType="TextBoxAndImage" 
                                      TextBoxStyle-CssClass="TextDate" 
                                      ShowNoneButton="False" >
                                     <TextBoxStyle CssClass="TextDate" />
                                    </BDP:BasicDatePicker>   									
								</FooterTemplate>							    
							</asp:TemplateField>
							
							
							<asp:TemplateField HeaderText="Currency" HeaderStyle-Width="80" SortExpression="Currency">
								<Itemtemplate>
									<asp:Label Runat="server" ID="Currency" Width="100%"  text='<%# DataBinder.Eval(Container.DataItem, "Currency") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:DropDownList Runat="server" ID="CurrencyEdit" Width="100%" 
                                        CssClass="DropDownList" AutoPostBack="True" SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Currency") %>'
                                        DataSourceID="dsCurrency" DataTextField="Currency_Name" 
                                        DataValueField="Currency">                                        
									</asp:DropDownList>								    
								</EditItemTemplate>
								<FooterTemplate>
									<asp:DropDownList ID="CurrencyAdd" Runat="Server" Width="100%" 
                                        CssClass="DropDownList" DataSourceID="dsCurrency" DataTextField="Currency_Name" 
                                        DataValueField="Currency">                                         
									</asp:DropDownList>
								</FooterTemplate>							    
							</asp:TemplateField>																						
										
							<asp:TemplateField HeaderText="Amount" HeaderStyle-Width="70" SortExpression="Amount">
								<Itemtemplate>
									<asp:Label Runat="server" Width="100%"  ID="Amount" text='<%# DataBinder.Eval(Container.DataItem, "Amount") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
								     <asp:TextBox ID="AmountEdit" CssClass="TextBox" Width="100%" Text='<%# DataBinder.Eval(Container.DataItem, "Amount") %>' Runat="Server"/>								   
									
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="AmountAdd" CssClass="TextBox" Width="100%" MaxLength="5" Runat="Server"/>								   
								</FooterTemplate>
							    <HeaderStyle Width="70px" />
							</asp:TemplateField>	
										
                            <asp:TemplateField HeaderText="Formula" HeaderStyle-Width="120" SortExpression="FormulaName">
								<Itemtemplate>
									<asp:Label Runat="server" ID="Formula" Width="100%" text='<%# DataBinder.Eval(Container.DataItem, "FormulaName") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:DropDownList Runat="server" ID="FormulaEdit" Width="100%" 
                                        CssClass="DropDownList" AutoPostBack="True" SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Formula") %>'
                                        DataSourceID="dsFormula" DataTextField="FormulaName" 
                                        DataValueField="FormulaCode">                                        
									</asp:DropDownList>								    
								</EditItemTemplate>
								<FooterTemplate>
									<asp:DropDownList ID="FormulaAdd" Runat="Server" Width="100%" 
                                        CssClass="DropDownList" DataSourceID="dsFormula" DataTextField="FormulaName" 
                                        DataValueField="FormulaCode">                                         
									</asp:DropDownList>
								</FooterTemplate>
							    <HeaderStyle Width="120px" />
							</asp:TemplateField>										
							
							<asp:TemplateField HeaderText="Remark" HeaderStyle-Width="200" SortExpression="Remark">
								<Itemtemplate>
									<asp:Label Runat="server" Width="100%" ID="Remark" text='<%# DataBinder.Eval(Container.DataItem, "Remark") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:TextBox ID="RemarkEdit" CssClass="TextBox" Width="100%" MaxLength="255" Text='<%# DataBinder.Eval(Container.DataItem, "Remark") %>' Runat="Server"/>								   																		
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="RemarkAdd" CssClass="TextBox" Width="100%" MaxLength="255" Runat="Server"/>								   
								</FooterTemplate>
							    <HeaderStyle Width="200px" />
							</asp:TemplateField>			
										
							<asp:TemplateField HeaderText="Action" headerstyle-width="180" >
							    <ItemTemplate>
								    <asp:Button ID="btnEdit" runat="server" class="bitbtndt btnedit" Text="Edit" CommandName="Edit"/>
									<asp:Button ID="btndelete" runat="server" class="bitbtndt btndelete" Text="Delete" OnClientClick="return confirm('Sure to delete this data?');" CommandName="Delete"/>																		
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
        <asp:Panel ID="pnlNewSlip" runat="server" Visible ="false">
            <table width="100%">
                <tr>
                    <td style="width:100px">Effective Date</td>
                    <td>:</td>
                    <td>                                           
                        <BDP:BasicDatePicker ID="tbEffDate" runat="server" DateFormat="dd MMM yyyy" 
                        ShowNoneButton = "false" ReadOnly = "true" ValidationGroup="Input"
                        ButtonImageHeight="19px" ButtonImageWidth="20px" 
                        DisplayType="TextBoxAndImage" 
                        TextBoxStyle-CssClass="TextDate"><TextBoxStyle CssClass="TextDate" /></BDP:BasicDatePicker>
                    </td>
                </tr>
                <tr>
                    <td style="width:100px">Currency</td>
                    <td>:</td>
                    <td>
                        <asp:DropDownList CssClass="DropDownList" ID = "ddlCurr" runat = "server" Width="120px" ></asp:DropDownList>                            
                    </td>
                </tr> 
                <tr>
                    <td style="width:100px">Amount</td>
                    <td>:</td>
                    <td>
                        <asp:TextBox CssClass="TextBox" Id="tbAmount" runat="server" Width="120px" />
                    </td>
                </tr>    
                <tr>
                    <td style="width:100px">Formula</td>
                    <td>:</td>
                    <td>
                        <asp:DropDownList CssClass="DropDownList" ID = "ddlFormula" runat = "server" Width="250px" ></asp:DropDownList>                            
                    </td>
                </tr> 
                <tr>
                    <td style="width:100px">Remark</td>
                    <td>:</td>
                    <td>
                        <asp:TextBox CssClass="TextBoxMulti" MaxLength="255" TextMode="MultiLine" Id="tbRemark" runat="server" Width="350px" />
                    </td>
                </tr>  
                <tr>
                    <td>
                    <asp:Button class="bitbtn btnadd" runat="server" ID="btnOKSlip" Text="OK" />
                        <asp:Button class="bitbtn btncancel" runat="server" ID="btnCancelSlip" Text="Cancel" />                                                     
                    </td>
                </tr> 
            </table>            
        </asp:Panel>        
    </div>
    <asp:SqlDataSource ID="dsCurrency" runat="server" 
          SelectCommand="EXEC S_GetCurrency">
    </asp:SqlDataSource>
    
    <asp:SqlDataSource ID="dsFormula" runat="server" 
          SelectCommand="EXEC S_GetFormula">
    </asp:SqlDataSource>
    <asp:Label ID="lbstatus" runat="server" ForeColor="Red"/>
    </form>
</body>
</html>
