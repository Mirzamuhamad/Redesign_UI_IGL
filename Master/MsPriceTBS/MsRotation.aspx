    <%@ Page Language="VB" AutoEventWireup="false" CodeFile="MsRotation.aspx.vb" Inherits="Master_MsRotation_MsRotation" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Rotation File</title>
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
     <div class="H1">Rotation File</div>
     <hr style="color:Blue" />
     <asp:Panel id="pnlHd" runat="server">
      <table>
        <tr>
            <td style="text-align:right; width:100px" >Quick Search :
                </td>
            <td><asp:TextBox CssClass="TextBox"  runat="server" ID ="tbFilter"/> 
                <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlField" >
                  <asp:ListItem Selected="true" Text="Rotation Code" Value="RotationCode"></asp:ListItem>
                  <asp:ListItem Text="Rotation Name" Value="RotationName"></asp:ListItem>
                  <asp:ListItem Text="Rotation" Value="Rotation"></asp:ListItem>
                  
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
                      <asp:ListItem Selected="true" Text="Rotation Code" Value="RotationCode"></asp:ListItem>
                      <asp:ListItem Text="Rotation Name" Value="RotationName"></asp:ListItem>
                      <asp:ListItem Text="Rotation" Value="Rotation"></asp:ListItem>
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
							<asp:TemplateField HeaderText="Rotation Code" HeaderStyle-Width="70" SortExpression="RotationCode">
								<Itemtemplate>
									<asp:Label Runat="server" Width="70" ID="RotationCode" text='<%# DataBinder.Eval(Container.DataItem, "RotationCode") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:Label Runat="server" ID="RotationCodeEdit" MaxLength="5" Width="100%" Text='<%# DataBinder.Eval(Container.DataItem, "RotationCode") %>'>
									</asp:Label>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="RotationCodeAdd" CssClass="TextBox" Width="100%" MaxLength="5" Runat="Server"/>
								   
								</FooterTemplate>
							    <HeaderStyle Width="70px" />
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Rotation Name" HeaderStyle-Width="280" SortExpression="RotationName">
								<Itemtemplate>
									<asp:Label Runat="server" Width="280" ID="RotationName" text='<%# DataBinder.Eval(Container.DataItem, "RotationName") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:TextBox Runat="server" ID="RotationNameEdit" MaxLength="60" CssClass="TextBox" Width="100%" Text='<%# DataBinder.Eval(Container.DataItem, "RotationName") %>'>
									</asp:TextBox>									
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="RotationNameAdd" CssClass="TextBox" Width="100%" MaxLength="60" Runat="Server"/>
									<cc1:TextBoxWatermarkExtender ID="RotationNameAdd_TextBoxWatermarkExtender" 
                                        runat="server" Enabled="True" TargetControlID="RotationNameAdd" WatermarkText="can't blank" WatermarkCssClass="Watermarked" >
                                    </cc1:TextBoxWatermarkExtender>
								</FooterTemplate>
							    <HeaderStyle Width="280px" />
							</asp:TemplateField>	
							
							<asp:TemplateField HeaderText="Rotation" HeaderStyle-Width="80" SortExpression="Rotation" >
								<Itemtemplate>
									<asp:Label Runat="server" ID="Rotation" text='<%# DataBinder.Eval(Container.DataItem, "Rotation") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:Label Runat="server" ID="RotationEdit" text='<%# DataBinder.Eval(Container.DataItem, "Rotation") %>'>
									</asp:Label>								
								</EditItemTemplate>	
								<FooterTemplate>
									<asp:Label Runat="server" ID="RotationAdd" >
									</asp:Label>
								</FooterTemplate>
							    <HeaderStyle Width="80px" />
							</asp:TemplateField>
										
							<asp:TemplateField HeaderText="Action" headerstyle-width="180" >
							    <ItemTemplate>
								    <asp:Button ID="btnEdit" runat="server" class="bitbtndt btnedit" Text="Edit" CommandName="Edit"/>
									<asp:Button ID="btndelete" runat="server" class="bitbtndt btndelete" Text="Delete" OnClientClick="return confirm('Sure to delete this data?');" CommandName="Delete"/>									
									<asp:Button ID="btnView" Width="70px" runat="server" class="bitbtndt btndetail" Text="Detail" CommandName="View" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>"/>
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
     <asp:Panel ID="pnlDt" runat="server" Visible = "false">  
     <asp:Label ID="label1" CssClass="H1" runat="server" Text="Rotation : " />   
     <asp:Label ID="lbRotationCode" ForeColor="Blue" CssClass="H1" Font-Bold="true" runat="server" />
	     &nbsp;
         <asp:Label ID="label2" runat="server" CssClass="H1" Text="Percentage : " />
         <asp:Label ID="lbTotalPercentage" runat="server" CssClass="H1" Font-Bold="True" 
             ForeColor="Blue" />
	 <br />
	 <asp:Button class="bitbtn btnback" width="50px" runat="server" ID="btnBackDtTop" Text="Back" />
	 <asp:Button class="bitbtndt btnadd" width="50px" runat="server" ID="btnAddDt" Text="Add" CommandName="Insert"/>    
	 <div style="border:0px  solid; width:100%; height:100%; overflow:auto;">
	        
            <asp:GridView ID="DataGridDt" runat="server" AllowPaging="True" 
                AllowSorting="True" AutoGenerateColumns="False" ShowFooter="False">
                <HeaderStyle CssClass="GridHeader" wrap="false" />
                <RowStyle CssClass="GridItem" wrap="false" />
                <AlternatingRowStyle CssClass="GridAltItem" />
                <%--<FooterStyle CssClass="GridFooter" Wrap="false" />--%>
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
                    <asp:BoundField DataField="Month" SortExpression="Month" HeaderText="Month" />
                    <asp:BoundField DataField="Percentage" SortExpression="Percentage" HeaderText="Percentage" />
                                        
                    <asp:TemplateField HeaderStyle-Width="50px" HeaderText="Action" Visible="false"
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
                <td>Month</td>
                <td>:</td>
                <td>
                    <asp:TextBox ID="tbMonth" CssClass="TextBox" MaxLength="5" runat="server" />                    
                </td>
            </tr> 
            <tr>
                <td>Percentage</td>
                <td>:</td>
                <td>
                    <asp:TextBox ID="tbPercentage" CssClass="TextBox" MaxLength="5" runat="server" />                    
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
   
    <%--<FooterStyle CssClass="GridFooter" Wrap="false" />--%>
    <asp:Label ID="lstatus" ForeColor="red" runat="server"></asp:Label>
    </form>
    </body>
</html>