<%@ Page Language="VB" AutoEventWireup="false" CodeFile="MsFormatJE.aspx.vb" Inherits="Master_MsFormatJE_MsFormatJE" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Format Journal Entry File</title>
    <script src="../../Function/Function.JS" type="text/javascript"></script>
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
     }          
 
     function OpenPopup() {         
      window.open("../../SearchDlg.Aspx","List","scrollbars=yes,resizable=no,width=500,height=400");        
      return false;
    }    
    </script>
    <%--</script> --%>   
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div class="Content">
     <div class="H1">Format Jurnal Entry File</div>
     <hr style="color:Blue" />
     <asp:Panel id="pnlHd" runat="server">
        <table>
        <tr>
            <td style="width:100px;text-align:right" >Quick Search :</td>
            <td><asp:TextBox runat="server" ID ="tbFilter" CssClass="TextBox"/> 
                <asp:DropDownList runat="server" ID="ddlField" CssClass="DropDownList" >
                  <asp:ListItem Selected="true" Text="Format Jurnal Entry Code" Value="FormatJECode"></asp:ListItem>
                  <asp:ListItem Text="Format Jurnal Entry Name" Value="FormatJEName"></asp:ListItem>        
                  <asp:ListItem Text="Type" Value="Type"></asp:ListItem>        
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
                    <asp:ListItem Selected="true" Text="Format Jurnal Entry Code" Value="FormatJECode"></asp:ListItem>
                    <asp:ListItem Text="Format Jurnal Entry Name" Value="FormatJEName"></asp:ListItem>        
                    <asp:ListItem Text="Type" Value="Type"></asp:ListItem> 
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
							<asp:TemplateField HeaderText="Format Jurnal Entry Code" HeaderStyle-Width="200" SortExpression="FormatJECode">
								<Itemtemplate>
									<asp:Label Runat="server" ID="FormatJECode" text='<%# DataBinder.Eval(Container.DataItem, "FormatJECode") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:Label Runat="server" ID="FormatJECodeEdit" Text='<%# DataBinder.Eval(Container.DataItem, "FormatJECode") %>'>
									</asp:Label>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="FormatJECodeAdd" placeholder="can't blank" CssClass="TextBox" MaxLength="5" Runat="Server" Width="100%"/>
									<%--<cc1:TextBoxWatermarkExtender ID="FormatJECodeAdd_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="FormatJECodeAdd" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>--%>
								</FooterTemplate>
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Format Jurnal Entry Name" HeaderStyle-Width="360" SortExpression="FormatJEName">
								<Itemtemplate>
									<asp:Label Runat="server"  ID="FormatJEName" text='<%# DataBinder.Eval(Container.DataItem, "FormatJEName") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:TextBox Runat="server" CssClass="TextBox" ID="FormatJENameEdit" MaxLength="60" Width="100%" Text='<%# DataBinder.Eval(Container.DataItem, "FormatJEName") %>'>
									</asp:TextBox>
									<cc1:TextBoxWatermarkExtender ID="FormatJENameEdit_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="FormatJENameEdit" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="FormatJENameAdd" placeholder="can't blank" CssClass="TextBox" MaxLength="60" Runat="Server" Width="100%"/>
									<%--<cc1:TextBoxWatermarkExtender ID="FormatJENameAdd_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="FormatJENameAdd" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>--%>
								</FooterTemplate>

<HeaderStyle Width="360px"></HeaderStyle>
							</asp:TemplateField>		
							
							
							<asp:TemplateField HeaderText="Type" SortExpression="Type" HeaderStyle-Width="90">
								<Itemtemplate>
									<asp:Label Runat="server" ID="Type" text='<%# DataBinder.Eval(Container.DataItem, "Type") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:DropDownList CssClass="DropDownList" Runat="server" ID="TypeEdit" Width="100%" Text='<%# DataBinder.Eval(Container.DataItem, "Type") %>'>
									    <asp:ListItem>Percentage</asp:ListItem>
									    <asp:ListItem>Nominal</asp:ListItem>
									</asp:DropDownList>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:DropDownList CssClass="DropDownList" ID="TypeAdd" Runat="Server" Width="100%">
									    <asp:ListItem Selected="True">Percentage</asp:ListItem>
									    <asp:ListItem >Nominal</asp:ListItem>
									</asp:DropDownList>
								</FooterTemplate>
							</asp:TemplateField>	
							
							
							<asp:TemplateField HeaderText="Action" ItemStyle-HorizontalAlign="Center" >
							    <ItemTemplate>
								    <asp:Button ID="btnEdit" runat="server" class="bitbtndt btnedit" Text="Edit" CommandName="Edit"/>
									<asp:Button ID="btndelete" runat="server" class="bitbtndt btndelete" Text="Delete" OnClientClick="return confirm('Sure to delete this data?');" CommandName="Delete"/>									
									<asp:Button ID="BtnDetail" runat="server" class="bitbtndt btnedit" Text="Detail" CommandName="Detail" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>"/>																			
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
     <td Width="65%">
     <asp:Label ID="label1" CssClass="H1" runat="server" Text="Format JE : " />   
     <asp:Label ID="lbFormatJECode" ForeColor="#0092C8" CssClass="H1" Font-Bold="true" runat="server" />
     <asp:Label ID="Label2" Text = " - " ForeColor="#0092C8" CssClass="H1" Font-Bold="true" runat="server" />
     <asp:Label ID="lbFormatJEName" ForeColor="#0092C8" CssClass="H1" Font-Bold="true" runat="server" /><br>
     <asp:Label ID="label5" CssClass="H1" runat="server" Text="Type : " />   
     <asp:Label ID="lbType" ForeColor="#0092C8" CssClass="H1" Font-Bold="true" runat="server" />
     </td>
     <td>
     </td>
     <td Width="180px">
         
     <asp:Label ID="label3" CssClass="H1" runat="server" Text="Total Debit : " /> <br> 
     <asp:Label ID="lbTotalDebit" runat="server" CssClass="H1" Font-Bold="true" 
             ForeColor="#0092C8" /> 
     </td>
     
     <td Width="180px">
     <asp:Label ID="label4" CssClass="H1" runat="server" Text="Total Credit : " />   <br>
     <asp:Label ID="lbTotalCredit" ForeColor="#0092C8" CssClass="H1" Font-Bold="true" runat="server" />
     </td>
     </tr>
     </table>     
     
	 <br />
	 <asp:Button class="bitbtn btnback" runat="server" ID="btnBackDtTop" Text="Back" />
	<%--    				                            <asp:ImageButton ID="btnBackDtTop" runat="server" 
                                                        ImageUrl="../../Image/btnBackon.png"
                                                        onmouseover="this.src='../../Image/btnBackoff.png';"
                                                        onmouseout="this.src='../../Image/btnBackon.png';"
                                                        ImageAlign="AbsBottom" />--%>
	 <br />
	 
	    <div style="border:0px  solid; width:100%; height:100%; overflow:auto;">
	    
            <asp:GridView ID="DataGridDt" runat="server" AllowPaging="True" 
                AllowSorting="True" AutoGenerateColumns="False" ShowFooter="True">
                <HeaderStyle CssClass="GridHeader" wrap="false" />
                <RowStyle CssClass="GridItem" wrap="false" />
                <AlternatingRowStyle CssClass="GridAltItem" />
                <FooterStyle CssClass="GridAltItem" Wrap="false" />
                <PagerStyle CssClass="GridPager" />
                <Columns>
                    <asp:TemplateField HeaderText="No" HeaderStyle-Width="30" SortExpression="ItemNo">
							<Itemtemplate>
							<asp:Label Runat="server" ID="ItemNo" text='<%# DataBinder.Eval(Container.DataItem, "ItemNo") %>'>
							</asp:Label>
							</Itemtemplate>	
							<EditItemTemplate>
                            <asp:Label ID="ItemNoEdit" Runat="server" Maxlength = "4"
                                text='<%# DataBinder.Eval(Container.DataItem, "ItemNo") %>' Width="100%">
                            </asp:Label>
                        </EditItemTemplate>	
                    <HeaderStyle Width="30" />    						
					</asp:TemplateField>	
					
                    <asp:TemplateField HeaderStyle-Width="128" HeaderText="Account" 
                        SortExpression="Account">
                        <Itemtemplate>
                            <asp:Label ID="Account" Runat="server" 
                                text='<%# DataBinder.Eval(Container.DataItem, "account") %>'>
                            </asp:Label>
                        </Itemtemplate>
                        <EditItemTemplate>
                            <%--<asp:Button CssClass="Button" ID="btnAccountEdit" Text="..." runat="server" CommandName="SearchAccountEdit" />--%>
                            <asp:TextBox ID="AccountEdit" runat="server" AutoPostBack="true" Maxlength = "12" 
                                CssClass="TextBox" ontextchanged="tbAccount_TextChanged" 
                                text='<%# DataBinder.Eval(Container.DataItem, "Account") %>' Width="90" />
                            <asp:Button ID="btnAccountEdit" runat="server" class="btngo" Text="..." CommandName="SearchAccountEdit"/>                            
                        </EditItemTemplate>
                        <FooterTemplate>
                            <%--<asp:Button CssClass="Button" ID="btnAccountAdd" Text="..." runat="server" CommandName="SearchAccountAdd" />--%>
                            <asp:TextBox ID="AccountAdd" runat="server" AutoPostBack="true" Text = "" 
                                CssClass="TextBox" ontextchanged="tbAccount_TextChanged" Width="90" MaxLength="12" />
                            <asp:Button ID="btnAccountAdd" runat="server" class="btngo" Text="..." CommandName="SearchAccountAdd"/>                            
                            </FooterTemplate>
                        <HeaderStyle Width="128" />
                    </asp:TemplateField>
                    
                    <asp:TemplateField HeaderStyle-Width="250" HeaderText="Account Name" 
                        SortExpression="AccountName">
                        <Itemtemplate>
                            <asp:Label ID="AccountName" Runat="server" 
                                text='<%# DataBinder.Eval(Container.DataItem, "AccountName") %>'>
                            </asp:Label>
                        </Itemtemplate>
                        <EditItemTemplate>
                            <asp:Label ID="AccountNameEdit" Runat="server" 
                                text='<%# DataBinder.Eval(Container.DataItem, "AccountName") %>' Width="250%">
                            </asp:Label>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:Label ID="AccountNameAdd" Runat="server" Width="250"> </asp:Label>
                        </FooterTemplate>
                        <HeaderStyle Width="250" />                       
                    </asp:TemplateField>   
                    
                    <asp:TemplateField HeaderStyle-Width="30" HeaderText="FgType" Visible="false" 
                        SortExpression="FgType">
                        <Itemtemplate>
                            <asp:Label ID="FgType" Runat="server" text='<%# DataBinder.Eval(Container.DataItem, "FgType") %>' Width="30">
                            </asp:Label>
                        </Itemtemplate>
                        <EditItemTemplate>
                            <asp:Label ID="FgTypeEdit" Runat="server" text='<%# DataBinder.Eval(Container.DataItem, "FgType") %>' Width="100%">
                            </asp:Label>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:Label ID="FgTypeAdd" Runat="server" Width="30"> </asp:Label>
                        </FooterTemplate>
                        <HeaderStyle Width="30px" />
                    </asp:TemplateField>		
                    
                    <asp:TemplateField HeaderText="Cost Center" HeaderStyle-Width="175" SortExpression="CostCtr">
						<Itemtemplate>
							<asp:Label Runat="server" ID="CostCtrName" text='<%# DataBinder.Eval(Container.DataItem, "Cost_Ctr_Name") %>'>
							</asp:Label>
							<asp:Label Runat="server" ID="CostCtr" text='<%# DataBinder.Eval(Container.DataItem, "CostCtr") %>' Visible = "false">
							</asp:Label>
						</Itemtemplate>
						<EditItemTemplate>
							<asp:DropDownList Runat="server" ID="CostCtrEdit" CssClass="DropDownList" Width="175" 
                            SelectedValue='<%# DataBinder.Eval(Container.DataItem, "CostCtr") %>' 
                            DataSourceID="dsCostCtr" DataTextField="Cost_Ctr_Name" DataValueField="Cost_Ctr_Code" AppendDataBoundItems="true">	
                            <asp:ListItem Value="" >Choose One</asp:ListItem>								    
							</asp:DropDownList>
						</EditItemTemplate>
						<FooterTemplate>
							<asp:DropDownList  ID="CostCtrAdd" Runat="Server" Width="175" CssClass="DropDownList"									    
                            DataSourceID="dsCostCtr" DataTextField="Cost_Ctr_Name" DataValueField="Cost_Ctr_Code" AppendDataBoundItems="true">
                            <asp:ListItem Value="">Choose One</asp:ListItem>
							</asp:DropDownList>
						</FooterTemplate>
						<HeaderStyle Width="175" />
					</asp:TemplateField>
            				
					<asp:TemplateField HeaderStyle-Width="30" HeaderText="FgSubled" Visible="false" 
                        SortExpression="FgSubled">
                        <Itemtemplate>
                            <asp:Label ID="FgSubled" Runat="server" 
                                text='<%# DataBinder.Eval(Container.DataItem, "FgSubled") %>' Width="30">
                            </asp:Label>
                        </Itemtemplate>
                        <EditItemTemplate>
                            <asp:Label ID="FgSubledEdit" Runat="server" 
                                text='<%# DataBinder.Eval(Container.DataItem, "FgSubled") %>' Width="100%">
                            </asp:Label>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:Label ID="FgSubledAdd" Runat="server" Width="30"> </asp:Label>
                        </FooterTemplate>
                        <HeaderStyle Width="30px" />
                    </asp:TemplateField>		
                    
                    <asp:TemplateField HeaderStyle-Width="128" HeaderText="Subled" 
                        SortExpression="Subled">
                        <Itemtemplate>
                            <asp:Label ID="Subled" Runat="server" 
                                text='<%# DataBinder.Eval(Container.DataItem, "subled") %>' Width="90">
                            </asp:Label>
                        </Itemtemplate>
                        <EditItemTemplate>
                            <%--<asp:Button CssClass="Button" ID="btnSubledEdit" Text="..." runat="server" CommandName="SearchSubledEdit" />--%>
                            <asp:TextBox ID="SubledEdit" runat="server" AutoPostBack="true" MaxLength="12"
                                CssClass="TextBox" ontextchanged="tbSubled_TextChanged" 
                                text='<%# DataBinder.Eval(Container.DataItem, "Subled") %>' Width="90" />
                            <asp:Button ID="btnSubledEdit" runat="server" class="btngo" Text="..."  CommandName="SearchSubledEdit"/>                                                        
                        </EditItemTemplate>
                        <FooterTemplate>
                            <%--<asp:Button CssClass="Button" ID="btnSubledAdd" Text="..." runat="server" CommandName="SearchSubledAdd" />--%>
                            <asp:TextBox ID="SubLedAdd" runat="server" AutoPostBack="true" Text= ""
                                CssClass="TextBox" ontextchanged="tbSubled_TextChanged" Width="90" MaxLength="12" />
                            <asp:Button ID="btnSubledAdd" runat="server" class="btngo" Text="..." CommandName="SearchSubledAdd"/>                                                                                    
                        </FooterTemplate>
                        <HeaderStyle Width="128" />
                    </asp:TemplateField>     
                                 
                    <asp:TemplateField HeaderStyle-Width="250" HeaderText="Subled Name" 
                        SortExpression="SubledName">
                        <Itemtemplate>
                            <asp:Label ID="SubledName" Runat="server" 
                                text='<%# DataBinder.Eval(Container.DataItem, "SubledName") %>'>
                            </asp:Label>
                        </Itemtemplate>
                        <EditItemTemplate>
                            <asp:Label ID="SubledNameEdit" Runat="server" 
                                text='<%# DataBinder.Eval(Container.DataItem, "SubledName") %>' Width="250">
                            </asp:Label>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:Label ID="SubledNameAdd" Runat="server" Width="250"> </asp:Label>
                        </FooterTemplate>
                        <HeaderStyle Width="250" />
                    </asp:TemplateField>
                    
                    <asp:TemplateField HeaderText="Debit" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Right" SortExpression="Debit">
						       <Itemtemplate >
									<asp:Label Runat="server" ID="Debit" text='<%# DataBinder.Eval(Container.DataItem, "Debit") %>'>
									</asp:Label>
								</Itemtemplate>	
								<EditItemTemplate>
									<asp:TextBox Runat="server" ID="DebitEdit" Width="95" MaxLength="22" CssClass="TextBox" Text='<%# DataBinder.Eval(Container.DataItem, "Debit","{0:#,##}") %>'>
									</asp:TextBox>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="DebitAdd" CssClass="TextBox" Runat="Server" Width="95" MaxLength="22"  Text= "0"/>
								</FooterTemplate>									
					           <HeaderStyle Width="100" />
                               <ItemStyle HorizontalAlign="Right" />
					</asp:TemplateField>
					
					<asp:TemplateField HeaderText="Credit" HeaderStyle-Width="100" ItemStyle-HorizontalAlign="Right" SortExpression="Credit">
						       <Itemtemplate>
									<asp:Label Runat="server" ID="Credit" text='<%# DataBinder.Eval(Container.DataItem, "Credit") %>'>
									</asp:Label>
								</Itemtemplate>	
								<EditItemTemplate>
									<asp:TextBox Runat="server" ID="CreditEdit" Width="95" CssClass="TextBox"  MaxLength="22" Text='<%# DataBinder.Eval(Container.DataItem, "Credit","{0:#,##}") %>'>
									</asp:TextBox>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="CreditAdd" CssClass="TextBox" Runat="Server" Width="95" MaxLength="22"  Text= "0"/>
								</FooterTemplate>									
					           <HeaderStyle Width="100" />
                               <ItemStyle HorizontalAlign="Right" />
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
                            <asp:Button ID="btnDelete0" runat="server" class="bitbtndt btndelete" Text="Delete" OnClientClick="return confirm('Sure to delete this data?');" CommandName="Delete"/>                                                                                                                                           
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
     <asp:SqlDataSource ID="dsCostCtr" runat="server" SelectCommand="SELECT Cost_Ctr_Code, Cost_Ctr_Name FROM VMsCostCtr" ></asp:SqlDataSource>        
     <asp:Label ID="lstatus" ForeColor="red" runat="server"></asp:Label>
    </div>
    </form>
</body>
</html>
