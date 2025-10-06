<%@ Page Language="VB" AutoEventWireup="false" CodeFile="MsMachineMaintenanceJobRM.aspx.vb" Inherits="Master_MsMachineMaintenanceJobRM_MsMachineMaintenanceJobRM" %>
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
    <div class="H1">Maintenance Item Sparepart</div>
    <hr style="color:Blue" /> 
        <br />                     
        
              <table width="100%">
                <tr>
                    <td style="width:100px">Machine</td>
                    <td>:</td>
                    <td>
                        <asp:TextBox CssClass="TextBox" Id="tbCode" runat="server" Width="120px" AutoPostBack="True"/>
                        <asp:TextBox CssClass="TextBox" ID="tbName" runat="server" Width="200px" Enabled="False" /> 
                        <asp:Button ID="btnSearch" runat="server" class="btngo" Text="..."/> 
                        <asp:Button class="bitbtn btnprint" runat="server" ID="btnPrint" Text="Print" />            
                        
                    </td>                    
                </tr>    
                <tr>
                    <td>Maintenance Item</td>
                    <td>:</td>
                    <td><asp:DropDownList CssClass="DropDownList" runat="server" ValidationGroup="Input" ID="ddlMaintenanceItem" AutoPostBack="true" /></td>                                 
                </tr>
                <tr>
                    <td>Job</td>
                    <td>:</td>
                    <td><asp:DropDownList CssClass="DropDownList" runat="server" ValidationGroup="Input" ID="ddlMaintenanceItemJob" AutoPostBack="true" /></td>                                 
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
							<asp:TemplateField HeaderText="Material" HeaderStyle-Width="150" SortExpression="Material">
								<Itemtemplate>
									<asp:Label Runat="server" Width="100%"  ID="Material" text='<%# DataBinder.Eval(Container.DataItem, "Material") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
								     <asp:Label Runat="server" Width="100%"  ID="MaterialEdit" text='<%# DataBinder.Eval(Container.DataItem, "Material") %>'>
									</asp:Label>
								     <%--<asp:TextBox ID="MaterialEdit" ontextchanged="MaterialEdit_TextChanged" AutoPostBack="true" CssClass="TextBox" Width="80%" Text='<%# DataBinder.Eval(Container.DataItem, "Material") %>' Runat="Server"/>								   									
								     <asp:Button ID="btnSearchMaterialEdit" CommandName="SearchMaterialEdit" runat="server" class="btngo" Text="..."/> --%>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="MaterialAdd" ontextchanged="MaterialAdd_TextChanged" AutoPostBack="true" CssClass="TextBox" Width="80%" MaxLength="5" Runat="Server"/>								   
									<asp:Button ID="btnSearchMaterialAdd" CommandName="SearchMaterialAdd" runat="server" class="btngo" Text="..."/> 									
								</FooterTemplate>
							    <HeaderStyle Width="150px" />
							</asp:TemplateField>	
							
							
							<asp:TemplateField HeaderText="Material Name" HeaderStyle-Width="255" SortExpression="Material_Name">
								<Itemtemplate>
									<asp:Label Runat="server" ID="MaterialName" Width="100%"  text='<%# DataBinder.Eval(Container.DataItem, "Material_Name") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:Label Runat="server" ID="MaterialNameEdit" Width="100%"  text='<%# DataBinder.Eval(Container.DataItem, "Material_Name") %>'>
									</asp:Label>													    
								</EditItemTemplate>
								<FooterTemplate>
									<asp:Label Runat="server" ID="MaterialNameAdd" Width="100%"  text='<%# DataBinder.Eval(Container.DataItem, "Material_Name") %>'>
									</asp:Label>
								</FooterTemplate>							    
							</asp:TemplateField>																						
										
							<asp:TemplateField HeaderText="Specification" HeaderStyle-Width="255" SortExpression="Specification">
								<Itemtemplate>
									<asp:Label Runat="server" ID="Specification" Width="100%"  text='<%# DataBinder.Eval(Container.DataItem, "Specification") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:Label Runat="server" ID="SpecificationEdit" Width="100%"  text='<%# DataBinder.Eval(Container.DataItem, "Specification") %>'>
									</asp:Label>													    
								</EditItemTemplate>
								<FooterTemplate>
									<asp:Label Runat="server" ID="SpecificationAdd" Width="100%"  text='<%# DataBinder.Eval(Container.DataItem, "Specification") %>'>
									</asp:Label>
								</FooterTemplate>							    
							</asp:TemplateField>	
										
							<asp:TemplateField HeaderText="Qty" HeaderStyle-Width="70" SortExpression="Qty">
								<Itemtemplate>
									<asp:Label Runat="server" Width="100%" ID="Qty" text='<%# DataBinder.Eval(Container.DataItem, "Qty") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
								     <asp:TextBox ID="QtyEdit" CssClass="TextBox" Width="100%" Text='<%# DataBinder.Eval(Container.DataItem, "Qty") %>' Runat="Server"/>								   									
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="QtyAdd" CssClass="TextBox" Width="100%" MaxLength="5" Runat="Server"/>								   
								</FooterTemplate>
							    <HeaderStyle Width="70px" />
							</asp:TemplateField>	
										
                            <asp:TemplateField HeaderText="Unit" HeaderStyle-Width="140" SortExpression="Unit">
								<Itemtemplate>
									<asp:Label Runat="server" ID="Unit" Width="100%" text='<%# DataBinder.Eval(Container.DataItem, "Unit") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:DropDownList Runat="server" ID="UnitEdit" Width="100%" 
                                        CssClass="DropDownList" SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Unit") %>'
                                        DataSourceID="dsUnit" DataTextField="Unit_Name" 
                                        DataValueField="Unit_Code">                                        
									</asp:DropDownList>								    
								</EditItemTemplate>
								<FooterTemplate>
									<asp:DropDownList ID="UnitAdd" Runat="Server" Width="100%" 
                                        CssClass="DropDownList" DataSourceID="dsUnit" DataTextField="Unit_Name" 
                                        DataValueField="Unit_Code">                                         
									</asp:DropDownList>
								</FooterTemplate>
							    <HeaderStyle Width="140px" />
							</asp:TemplateField>										
															
							<asp:TemplateField HeaderText="Action" headerstyle-width="140" >
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
							    <HeaderStyle Width="140px" />
							</asp:TemplateField>							
    					</Columns>
                        </asp:GridView> 
        </asp:Panel>
        <%--<asp:Panel ID="pnlNewSlip" runat="server" Visible ="false">
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
        </asp:Panel>--%>        
    </div>
    <asp:SqlDataSource ID="dsUnit" runat="server" 
          SelectCommand="EXEC S_GetUnit">
    </asp:SqlDataSource>
    <asp:Label ID="lbstatus" runat="server" ForeColor="Red"/>
    </form>
</body>
</html>
