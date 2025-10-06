<%@ Page Language="VB" AutoEventWireup="false" CodeFile="MsFAMaintenance.aspx.vb" Inherits="Master_MsFAMaintenance_MsFAMaintenance" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Fixed Asset Maintenance</title>
    <script src="../../Function/OpenDlg.JS" type="text/javascript"></script>
    <script type="text/javascript">    
    function OpenPopup() {         
        window.open("../../SeaDlg.Aspx","List","scrollbars=yes,resizable=no,width=500,height=400");        
        return false;
    }    
    function openprintdlg() {
            var wOpens;
            wOpens = window.open("../../Rpt/PrintForm.Aspx", "List", "scrollbars=yes,resizable=yes,width=500,height=400");
            wOpens.moveTo(0, 0);
            wOpens.resizeTo(screen.width, screen.height);
     )           
 
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
     <div class="H1"><asp:Label runat="server" ID="lblTitle"></asp:Label></div>
     <hr style="color:Blue" />
     <asp:Panel id="pnlHd" runat="server">
        <table>
        <tr>
            <td style="width:100px;text-align:right" >Quick Search :</td>
            <td><asp:TextBox runat="server" ID ="tbFilter" CssClass="TextBox"/> 
                <asp:DropDownList runat="server" ID="ddlField" CssClass="DropDownList" >
                  <asp:ListItem Selected="true" Text="FA Maintenance Code" Value="FAMaintenanceCode"></asp:ListItem>
                  <asp:ListItem Text="FA Maintenance Name" Value="FAMaintenanceName"></asp:ListItem>        
                  <asp:ListItem Text="Add Value" Value="FgAddValue"></asp:ListItem>        
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
                    <asp:ListItem Selected="true" Text="FA Maintenance Code" Value="FAMaintenanceCode"></asp:ListItem>
                    <asp:ListItem Text="FA Maintenance Name" Value="FAMaintenanceName"></asp:ListItem>        
                    <asp:ListItem Text="Add Value" Value="FgAddValue"></asp:ListItem>        
                  </asp:DropDownList>
                
            </td>
        </tr>
     </table>
     </asp:Panel>
      <br />
      <asp:GridView id="DataGrid" runat="server" ShowFooter="True" AllowSorting="True" 
            AutoGenerateColumns="False" AllowPaging="True" CssClass="Grid">
						<HeaderStyle CssClass="GridHeader"></HeaderStyle>
						<RowStyle CssClass="GridItem" Wrap="false"/>
						<AlternatingRowStyle CssClass="GridAltItem"/>
						<FooterStyle CssClass="GridFooter" />
						<PagerStyle CssClass="GridPager" />
				      <Columns>
							<asp:TemplateField HeaderText="Code" SortExpression="FAMaintenanceCode">
								<Itemtemplate>
									<asp:Label Runat="server" ID="FAMaintenanceCode" text='<%# DataBinder.Eval(Container.DataItem, "FAMaintenanceCode") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:Label Runat="server" ID="FAMaintenanceCodeEdit" Text='<%# DataBinder.Eval(Container.DataItem, "FAMaintenanceCode") %>'>
									</asp:Label>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="FAMaintenanceCodeAdd" Placeholder = "can't blank" CssClass="TextBox" MaxLength="5" Runat="Server" Width="100%"/>
									<%--<cc1:TextBoxWatermarkExtender ID="FAMaintenanceCodeAdd_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="FAMaintenanceCodeAdd" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>--%>
								</FooterTemplate>
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="FA Maintenance Name" HeaderStyle-Width="360" SortExpression="FAMaintenanceName">
								<Itemtemplate>
									<asp:Label Runat="server" ID="FAMaintenanceName" text='<%# DataBinder.Eval(Container.DataItem, "FAMaintenanceName") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:TextBox Runat="server" CssClass="TextBox" ID="FAMaintenanceNameEdit" MaxLength="50" Width="100%" Text='<%# DataBinder.Eval(Container.DataItem, "FAMaintenanceName") %>'>
									</asp:TextBox>
									<cc1:TextBoxWatermarkExtender ID="FAMaintenanceNameEdit_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="FAMaintenanceNameEdit" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="FAMaintenanceNameAdd" Placeholder = "can't blank" CssClass="TextBox" MaxLength="50" Runat="Server" Width="100%"/>
									<%--<cc1:TextBoxWatermarkExtender ID="FAMaintenanceNameAdd_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="FAMaintenanceNameAdd" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>--%>
								</FooterTemplate>

                            <HeaderStyle Width="360px"></HeaderStyle>
							</asp:TemplateField>		
							
							
							<asp:TemplateField HeaderText="Value" SortExpression="FgAddValue">
								<Itemtemplate>
									<asp:Label Runat="server" ID="Value" text='<%# DataBinder.Eval(Container.DataItem, "FgAddValue") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:DropDownList CssClass="DropDownList" Runat="server" ID="ValueEdit" Width="100%" Text='<%# DataBinder.Eval(Container.DataItem, "FgAddValue") %>'>
									    <asp:ListItem>N</asp:ListItem>
									    <asp:ListItem>Y</asp:ListItem>
									</asp:DropDownList>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:DropDownList CssClass="DropDownList" ID="ValueAdd" Runat="Server" Width="100%">
									    <asp:ListItem Selected="True">N</asp:ListItem>
									    <asp:ListItem >Y</asp:ListItem>
									</asp:DropDownList>
								</FooterTemplate>
							</asp:TemplateField>	
							
							
							<asp:TemplateField HeaderText="Action" ItemStyle-HorizontalAlign="Center" >
							    <ItemTemplate>
								    <asp:Button ID="btnEdit" runat="server" class="bitbtndt btnedit" Text="Edit" CommandName="Edit"/>
									<asp:Button ID="btndelete" runat="server" class="bitbtndt btndelete" Text="Delete" OnClientClick="return confirm('Sure to delete this data?');" CommandName="Delete"/>									
									<asp:Button ID="BtnDetail" runat="server" class="bitbtndt btndetail" Text="Detail" CommandName="Detail" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>"/>										
									           								
								</ItemTemplate>
								<EditItemTemplate>
									<asp:Button ID="btnUpdate" runat="server" class="bitbtndt btnsave" Text="Save" CommandName="Update"/>									
                                    <asp:Button ID="btnCancel" runat="server" class="bitbtndt btncancel" Text="Cancel" CommandName="Cancel"/>									
								</EditItemTemplate>								
								<FooterTemplate>
									<asp:Button class="bitbtndt btnadd" runat="server" ID="btnAdd" Text="Add" CommandName="Insert"/>									
								</FooterTemplate>

<ItemStyle HorizontalAlign="Center"></ItemStyle>
							</asp:TemplateField>
							
    					</Columns>
        </asp:GridView>                     
     </asp:Panel>     
     <asp:Panel ID="pnlDt" runat="server" Visible = "False">  
     <table>
     <tr>
     <td width="65%">
     <asp:Label ID="lbFormatJECode" CssClass="H1" runat="server" Text="Format JE : " Visible = "false" /> 
     <asp:Label ID="lbFAMaintainCode" ForeColor="Blue" CssClass="H1" Font-Bold="true" runat="server" />
     <asp:Label ID="lbFormatJEName" Text = " - " ForeColor="Blue" CssClass="H1" Font-Bold="true" runat="server" />
     <asp:Label ID="lbFAMaintainName" ForeColor="Blue" CssClass="H1" Font-Bold="true" runat="server" />
     </td>
     </tr>
     </table>     
     
     <br />
	 <asp:Button class="bitbtn btnback" runat="server" ID="btnBackDtTop" Text="Back" />
	
	 <br />
	 
	    <div style="border:0px  solid; width:100%; height:100%; overflow:auto;">
	    
            <asp:GridView ID="DataGridDt" runat="server" AllowPaging="True" 
                AllowSorting="True" AutoGenerateColumns="False" ShowFooter="True">
                <HeaderStyle CssClass="GridHeader" wrap="false" />
                <RowStyle CssClass="GridItem" wrap="false" />
                <AlternatingRowStyle CssClass="GridAltItem" />
                <FooterStyle CssClass="GridFooter" Wrap="false" />
                <PagerStyle CssClass="GridPager" />
                <Columns>
          
                       <asp:TemplateField HeaderText="Currency" HeaderStyle-Width="210" SortExpression="CurrName">
							<Itemtemplate>
			 					<asp:Label Runat="server" ID="Currency" text='<%# DataBinder.Eval(Container.DataItem, "CurrName") %>'>
								</asp:Label>
								<asp:Label Runat="server" ID="CurrencyCode" Visible = "False" text='<%# DataBinder.Eval(Container.DataItem, "Currency") %>'>
								</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
								
								   <asp:Label Runat="server" ID="ddlCurrencyEdit" text='<%# DataBinder.Eval(Container.DataItem, "CurrName") %>'>
								   </asp:Label>
								   <asp:Label Runat="server" ID="ddlCurrencyEdit2" Visible = "False" text='<%# DataBinder.Eval(Container.DataItem, "Currency") %>'>
								   </asp:Label>

									<%--<asp:DropDownList Runat="server" ID="ddlCurrencyEdit" Width="100%" CssClass="DropDownList" 
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Currency") %>' 
                                        DataSourceID="dsCurrency" DataTextField="CurrName" 
                                        DataValueField="CurrCode">
									</asp:DropDownList>--%>								    
								</EditItemTemplate>
								<FooterTemplate>
									<asp:DropDownList ID="ddlCurrencyAdd" Runat="Server" Width="100%" CssClass="DropDownList"									    
                                        DataSourceID="dsCurrency" DataTextField="CurrName" 
                                        DataValueField="CurrCode">
									</asp:DropDownList>
								</FooterTemplate>
								<ControlStyle Width="150px" />
                               <HeaderStyle Width="210px"></HeaderStyle>
							</asp:TemplateField>																						
									
					
					 <asp:TemplateField HeaderStyle-Width="128" HeaderText="Account" 
                        SortExpression="Account">
                        <Itemtemplate>
                            <asp:Label ID="Account" Runat="server" 
                                text='<%# DataBinder.Eval(Container.DataItem, "account") %>' Width="128">
                            </asp:Label>
                        </Itemtemplate>
                        <EditItemTemplate>
                            <%--<asp:Button CssClass="Button" ID="btnAccountEdit" Text="..." runat="server" CommandName="SearchAccountEdit" />--%>
                            <asp:TextBox ID="AccountEdit" runat="server" AutoPostBack="true" 
                                CssClass="TextBox" ontextchanged="tbAccount_TextChanged" 
                                text='<%# DataBinder.Eval(Container.DataItem, "Account") %>' Width="90" MaxLength="12"/>
                            <asp:Button ID="btnAccountEdit" runat="server" class="btngo" Text="..." CommandName="SearchAccountEdit"/>                            
                        </EditItemTemplate>
                        <FooterTemplate>
                            <%--<asp:Button CssClass="Button" ID="btnAccountAdd" Text="..." runat="server" CommandName="SearchAccountAdd" />--%>
                            <asp:TextBox ID="AccountAdd" runat="server" AutoPostBack="true" Text = ""
                                CssClass="TextBox" ontextchanged="tbAccount_TextChanged" Width="90" MaxLength="12" />
                            <asp:Button ID="btnAccountAdd" runat="server" class="btngo" Text="..." CommandName="SearchAccountAdd"/>                            
                            </FooterTemplate>
                        <HeaderStyle Width="128px" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderStyle-Width="280" HeaderText="Account Name" 
                        SortExpression="AccountName">
                        <Itemtemplate>
                            <asp:Label ID="AccountName" Runat="server" 
                                text='<%# DataBinder.Eval(Container.DataItem, "AccountName") %>' Width="280">
                            </asp:Label>
                        </Itemtemplate>
                        <EditItemTemplate>
                            <asp:Label ID="AccountNameEdit" Runat="server" 
                                text='<%# DataBinder.Eval(Container.DataItem, "AccountName") %>' Width="100%">
                            </asp:Label>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:Label ID="AccountNameAdd" Runat="server" Width="280"> </asp:Label>
                        </FooterTemplate>
                        <HeaderStyle Width="280px" />
                        
                    </asp:TemplateField>   
            
							                   
                    
                    <asp:TemplateField HeaderStyle-Width="126px" HeaderText="Action" 
                        ItemStyle-Wrap="false">
                        <EditItemTemplate>
                            <%--<asp:Button CssClass="Button" CommandName="Update" Text="Update" ID="btnUpdate" Runat="server" Width="60" />--%>
                            <asp:Button ID="btnUpdate0" runat="server" class="bitbtndt btnsave" Text="Save"  CommandName="Update"/>                                                        
                            <%--<asp:Button CssClass="Button" CommandName="Cancel" Text="Cancel" ID="btnCancel" Runat="server" Width="50" />--%>
                            <asp:Button ID="btnCancel0" runat="server" class="bitbtndt btnsave" Text="Cancel"  CommandName="Cancel"/>                                                                                    
                        </EditItemTemplate>
                        <FooterTemplate>
                            <%--<asp:Button CssClass="Button" CommandName="Insert" Text="Add" ID="btnAdd" Runat="server" Width="95" />--%>
                            <asp:Button ID="btnAdd0" runat="server" class="bitbtndt btnadd" Text="Add"  CommandName="Insert"/>                                                                                                                
                        </FooterTemplate>
                        <ItemTemplate>
                            <%--<asp:Button CssClass="Button" CommandName="Edit" Text="Edit" ID="btnEdit" Runat="server" Width="50" />--%>
                            <asp:Button ID="btnEdit0" runat="server" class="bitbtndt btnedit" Text="Edit"  CommandName="Edit"/>                                                                                                                
                            <%--<asp:Button CssClass="Button" CommandName="Delete" Text="Delete" ID="btnDelete" OnClientClick="return confirm('Sure to delete this data?');" Runat="server" Width="55" />--%>
                            <asp:Button ID="btnDelete0" runat="server" class="bitbtndt btndelete" Text="Delete"  OnClientClick="return confirm('Sure to delete this data?');" CommandName="Delete"/>                                                                                                                                            
                        </ItemTemplate>
                        <HeaderStyle Width="126px" />
                        <ItemStyle Wrap="False" />
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
	    
        </div>        
	    <asp:Button class="bitbtn btnback" runat="server" ID="btnBack2" Text="Back" />
	       				                            <%--<asp:ImageButton ID="btnBack2" runat="server" 
                                                        ImageUrl="../../Image/btnBackon.png"
                                                        onmouseover="this.src='../../Image/btnBackoff.png';"
                                                        onmouseout="this.src='../../Image/btnBackon.png';"
                                                        ImageAlign="AbsBottom" />--%>
     </asp:Panel>   
        
      <asp:SqlDataSource ID="dsCurrency" runat="server"                                                                                 
           SelectCommand="SELECT CurrCode, CurrName FROM MsCurrency">                                        
      </asp:SqlDataSource>        
          
     <asp:Label ID="lstatus" ForeColor="red" runat="server"></asp:Label>
    </div>
    </form>
</body>
</html>
