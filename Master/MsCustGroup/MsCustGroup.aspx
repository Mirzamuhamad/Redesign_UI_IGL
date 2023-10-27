<%@ Page Language="VB" AutoEventWireup="false" CodeFile="MsCustGroup.aspx.vb" Inherits="Master_MsCustGroup_MsCustGroup" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Customer Group File</title>
    <script src="../../Function/OpenDlg.JS" type="text/javascript"></script>            
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div class="Content">
     <div class="H1">Customer Group File</div>
     <hr style="color:Blue" />
     <asp:Panel id="pnlHd" runat="server">
      <table>
        <tr>
            <td style="text-align:right; width:100px" >Quick Search :
            </td>
            <td><asp:TextBox CssClass="TextBox"  runat="server" ID ="tbFilter"/> 
                <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlField" >
                  <asp:ListItem Selected="true" Text="Customer Group Code" Value="CustGroupCode"></asp:ListItem>
                  <asp:ListItem Text="Customer Group Name" Value="CustGroupName"></asp:ListItem>
                  <asp:ListItem Text="Customer Group Type" Value="CustGroupType"></asp:ListItem>  
                  <asp:ListItem Text="PKP" Value="FgPKP"></asp:ListItem>  
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
          <td style="width:100px;text-align:right">
              <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlNotasi" >
                <asp:ListItem Selected="true" Text="OR" Value="OR"></asp:ListItem>
                <asp:ListItem Text="AND" Value="AND"></asp:ListItem>            
              </asp:DropDownList>
          </td>
            <td><asp:TextBox CssClass="Button" runat="server" ID ="tbfilter2"/> 
                  <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlField2" >
                      <asp:ListItem Selected="true" Text="Customer Group Code" Value="CustGroupCode"></asp:ListItem>
                      <asp:ListItem Text="Customer Group Name" Value="CustGroupName"></asp:ListItem>
                      <asp:ListItem Text="Customer Group Type" Value="CustGroupType"></asp:ListItem>
                      <asp:ListItem Text="PKP" Value="FgPKP"></asp:ListItem>
                  </asp:DropDownList>
                
            </td>
        </tr>
     </table>
     </asp:Panel>
      <br />
      <div style="border:0px  solid; width:100%; height:100%; overflow:auto;">
      
      <asp:GridView id="DataGrid" runat="server" AllowSorting="True" 
            AutoGenerateColumns="False" AllowPaging="True" CssClass="Grid">
						<HeaderStyle CssClass="GridHeader" wrap="False"></HeaderStyle>
						<RowStyle CssClass="GridItem" Wrap="False"/>
						<AlternatingRowStyle CssClass="GridAltItem"/>						
						<PagerStyle CssClass="GridPager" />
						
				      <Columns>
							<asp:TemplateField HeaderText="Cust Grp Code" HeaderStyle-Width="70" SortExpression="CustGroupCode">
								<Itemtemplate>
									<asp:Label Runat="server" Width="70" ID="CustGroupCode" text='<%# DataBinder.Eval(Container.DataItem, "CustGroupCode") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:Label Runat="server" ID="CustGroupCodeEdit" Width="100%" Text='<%# DataBinder.Eval(Container.DataItem, "CustGroupCode") %>'>
									</asp:Label>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="CustGroupCodeAdd" Placeholder = "can't blank" CssClass="TextBox" Width="100%" MaxLength="5" Runat="Server"/>
								    <%--<cc1:TextBoxWatermarkExtender ID="CustGroupCodeAdd_TextBoxWatermarkExtender" 
                                        runat="server" Enabled="True" TargetControlID="CustGroupCodeAdd" WatermarkText="can't blank" WatermarkCssClass="Watermarked" >
                                    </cc1:TextBoxWatermarkExtender>--%>
								</FooterTemplate>
							    <HeaderStyle Width="70px" />
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Customer Group Name" HeaderStyle-Width="300" SortExpression="CustGroupName">
								<Itemtemplate>
									<asp:Label Runat="server" Width="100%" ID="CustGroupName" text='<%# DataBinder.Eval(Container.DataItem, "CustGroupName") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:TextBox Runat="server" ID="CustGroupNameEdit" CssClass="TextBox" MaxLength="60"  Width="100%" Text='<%# DataBinder.Eval(Container.DataItem, "CustGroupName") %>'>
									</asp:TextBox>
									<%--<cc1:TextBoxWatermarkExtender ID="CustGroupNameEdit_TextBoxWatermarkExtender" 
                                        runat="server" Enabled="True" TargetControlID="CustGroupNameEdit" WatermarkText="can't blank" WatermarkCssClass="Watermarked" >
                                    </cc1:TextBoxWatermarkExtender>--%>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="CustGroupNameAdd" Placeholder = "can't blank" CssClass="TextBox" Width="100%" MaxLength="60" Runat="Server" />
									<%--<cc1:TextBoxWatermarkExtender ID="CustGroupNameAdd_TextBoxWatermarkExtender" 
                                        runat="server" Enabled="True" TargetControlID="CustGroupNameAdd" WatermarkText="can't blank" WatermarkCssClass="Watermarked" >
                                    </cc1:TextBoxWatermarkExtender>--%>
								</FooterTemplate>
							</asp:TemplateField>	
							
							<asp:TemplateField HeaderText="Cust Grp Type" HeaderStyle-Width="30" ItemStyle-HorizontalAlign="Center" SortExpression="CustGroupType">
								<Itemtemplate>
									<asp:Label Runat="server" Width="30" ID="CustGroupType" TEXT='<%# DataBinder.Eval(Container.DataItem, "CustGroupType") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:DropDownList ID="CustGroupTypeEdit" CssClass="DropDownList" Width="100%" runat="server" SelectedValue='<%# DataBinder.Eval(Container.DataItem, "CustGroupType") %>'>
									  <asp:ListItem>Lokal</asp:ListItem>
									  <asp:ListItem>Export</asp:ListItem>                                        									  
									</asp:DropDownList>								    
								</EditItemTemplate>
								<FooterTemplate>
									<asp:DropDownList ID="CustGroupTypeAdd" CssClass="DropDownList" Width="100%" runat="server">
									  <asp:ListItem Selected="True">Lokal</asp:ListItem>
									  <asp:ListItem>Export</asp:ListItem>                                        									  
									</asp:DropDownList>								    
								</FooterTemplate>
							    <HeaderStyle Width="30px" />
                                <ItemStyle HorizontalAlign="Center" />
							</asp:TemplateField>								
														
							<asp:TemplateField HeaderText="PKP" HeaderStyle-Width="30" ItemStyle-HorizontalAlign="Center" SortExpression="FgPKP">
								<Itemtemplate>
									<asp:Label Runat="server" Width="30" ID="PKP" text='<%# DataBinder.Eval(Container.DataItem, "FgPKP") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:DropDownList ID="PKPEdit" CssClass="DropDownList" Width="100%" runat="server" SelectedValue='<%# DataBinder.Eval(Container.DataItem, "FgPKP") %>'>
									  <asp:ListItem>Y</asp:ListItem>
									  <asp:ListItem>N</asp:ListItem>                                        
									</asp:DropDownList>								    
								</EditItemTemplate>
								<FooterTemplate>
									<asp:DropDownList ID="PKPAdd" CssClass="DropDownList" Width="100%" runat="server">
									  <asp:ListItem Selected="True">Y</asp:ListItem>
									  <asp:ListItem>N</asp:ListItem>                                        
									</asp:DropDownList>								    
								</FooterTemplate>
							    <HeaderStyle Width="30px" />
                                <ItemStyle HorizontalAlign="Center" />
							</asp:TemplateField>								
							
							<asp:TemplateField HeaderText="Action" headerstyle-width="190" >
							    <ItemTemplate>
								    <asp:Button ID="btnEdit" runat="server" class="bitbtndt btnedit" Text="Edit" CommandName="Edit"/>
									<asp:Button ID="btndelete" runat="server" class="bitbtndt btndelete" Text="Delete" OnClientClick="return confirm('Sure to delete this data?');" CommandName="Delete"/>									
									<asp:Button ID="btnView" runat="server" class="bitbtndt btnedit" Text="Detail" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" CommandName="View" />
									
								</ItemTemplate>
								<EditItemTemplate>
									<asp:Button ID="btnUpdate" runat="server" class="bitbtndt btnsave" Text="Save" CommandName="Update"/>									
                                    <asp:Button ID="btnCancel" runat="server" class="bitbtndt btncancel" Text="Cancel" CommandName="Cancel"/>									
								</EditItemTemplate>
								<FooterTemplate>
									<asp:Button class="bitbtndt btnadd" runat="server" ID="btnAdd" Text="Add" CommandName="Insert"/>									
								</FooterTemplate>
							    <HeaderStyle Width="190px" />
							</asp:TemplateField>							
    					</Columns>
        </asp:GridView>
        </div>
            </asp:Panel>
     <br />       
     <asp:Panel runat="server" ID="PanelInfo" Visible = "false">
     <asp:Label ID="label1" CssClass="H1" runat="server" Text="Customer Group : " />   
     <asp:Label ID="lbCustGrouType" ForeColor="Blue" CssClass="H1" Font-Bold="true" runat="server" />
     <asp:Label ID="lbCustType" ForeColor="white" CssClass="H1" Font-Bold="true" runat="server" />
     </asp:Panel>       
     <br />
     <asp:Panel ID="pnlDt" runat="server" Visible = "false">  
     <asp:Button class="bitbtn btnback" runat="server" ID="btnBackDtTop" Text="Back" />    	 
	 &nbsp &nbsp 
	 <asp:Button class="bitbtn btnadd" runat="server" ID="BtnAdd" Text="Add" />	           
     
	 
	 
	 <br />
	 
	    <div style="border:0px  solid; width:100%; height:100%; overflow:auto;">
        <asp:GridView id="DataGridDt" runat="server" AllowSorting="True" 
            AutoGenerateColumns="False" AllowPaging="True">
						<HeaderStyle CssClass="GridHeader" wrap="false"></HeaderStyle>
						<RowStyle CssClass="GridItem" wrap="false"/>
						<AlternatingRowStyle CssClass="GridAltItem"/>						
						<PagerStyle CssClass="GridPager" />
				      <Columns>
				      
                       <asp:TemplateField HeaderText="Action">
                            <ItemTemplate>
                                 <asp:Button class="bitbtndt btnedit" runat="server" ID="btnEdit0" Text="Edit" CommandNAme="Edit"  />									                                                                        
                                 <asp:Button class="bitbtndt btndelete" runat="server" ID="btnDelete0" Text="Delete" CommandNAme="Delete" OnClientClick="return confirm('Sure to delete this data?');"  />									                                                                                                                
                             </ItemTemplate>
                             <EditItemTemplate>
                                 <asp:Button class="bitbtndt btnupdate" runat="server" ID="btnUpdate0" Text="Save" CommandNAme="Update"  />									                                                                        
                                 <asp:Button class="bitbtndt btncancel" runat="server" ID="btnCancel0" Text="Cancel" CommandNAme="Cancel"  />									                                                                                                                
                             </EditItemTemplate>
                              <FooterTemplate>                            
                                 <asp:Button class="bitbtndt btnadd" runat="server" ID="btnadddt"  Text="Add" CommandName="Insert"  />									                                                                                                                                                        
                              </FooterTemplate>
                        </asp:TemplateField>
                       
                        <asp:BoundField DataField="CurrCode" HeaderText="Currency" />
                        <asp:BoundField DataField="AccAR" HeaderText="Acc. AR" />                  
                        <asp:BoundField DataField="AccARName" HeaderText="Acc. AR Name" />                  
                        <asp:BoundField DataField="AccSJUninvoice" HeaderText="Acc. SJ Uninvoice" />                  
                        <asp:BoundField DataField="AccSJUninvoiceName" HeaderText="Acc. SJ Uninvoice Name" />                  
                        <asp:BoundField DataField="AccDisc" HeaderText="Acc. Disc" />                                    
                        <asp:BoundField DataField="AccDiscName" HeaderText="Acc. Disc Name" 
                         SortExpression="AccDiscName" HeaderStyle-HorizontalAlign ="Center" >
                            <HeaderStyle HorizontalAlign="Center" />
                          </asp:BoundField>
                        <asp:BoundField DataField="AccOther" HeaderText="Acc. Other" 
                         SortExpression="AccOther" />                  
                        <asp:BoundField DataField="AccOtherName" HeaderText="Acc. Other Name" 
                         SortExpression="AccOtherName" />                  
                        <asp:BoundField DataField="AccCreditAR" 
                        HeaderText="Acc. Credit AR" SortExpression="AccCreditAR" ></asp:BoundField>                      
                        <asp:BoundField DataField="AccCreditARName" 
                        HeaderText="Acc. Credit AR Name" SortExpression="AccCreditARName"></asp:BoundField>                           
                        <asp:BoundField DataField="AccDP" HeaderText="Acc. DP" SortExpression="AccDP" />
                        <asp:BoundField DataField="AccDPName" HeaderText="Acc. DP Name" SortExpression="AccDPName" />
                        <asp:BoundField DataField="AccDeposit" HeaderText="Acc. Deposit" SortExpression="AccDeposit" />
                        <asp:BoundField DataField="AccDepositName" HeaderText="Acc. Deposit Name" SortExpression="AccDepositName" />
                        <asp:BoundField DataField="AccPPN" HeaderText="Acc. PPN" SortExpression="AccPPN" />
                        <asp:BoundField DataField="AccPPNName" HeaderText="Acc. PPN Name" SortExpression="AccPPNName" /> 
                        <asp:BoundField DataField="AccPotongan" HeaderText="Acc. Potongan" SortExpression="AccPotongan" />
                        <asp:BoundField DataField="AccPotonganName" HeaderText="Acc. Potongan Name" SortExpression="AccPotonganName" />                              
				   </Columns>
        </asp:GridView>  
        </div>    
	     
	   	 
	   	 <asp:Button class="bitbtn btnback" runat="server" ID="Button2" Text="Back" />    	 
	     &nbsp &nbsp 
	     <asp:Button class="bitbtn btnadd" runat="server" ID="BtnAdd2" Text="Add" />
	     	 
        </asp:Panel>   
        <asp:Panel runat="server" ID="PnlEditDetail" Visible="false">
            <table>
                <tr>
                    <td>Currency</td>
                    <td>:</td>
                    <td>
                                   
                        <asp:DropDownList ID="ddlCurrency" CssClass="DropDownList" runat="server" DataSourceID="dsCurrency" Width="50%" 
                            DataTextField="Currency" DataValueField="Currency" AutoPostBack="true" OnSelectedIndexChanged = "ddlCurrency_SelectedIndexChanged" >
                        </asp:DropDownList>								    

                    </td>
                </tr>           
               
                                              
                <tr>
                    <td>Acc. AR</td>
                    <td>:</td>
                    <td>
                    <asp:TextBox ID="tbAccAR" runat="server" AutoPostBack="True" MaxLength="12"
                      CssClass="TextBox" Width="112px" />
                    <asp:TextBox ID="tbAccARName" runat="server" CssClass="TextBox" 
                       Enabled="False" Width="250px" />
                    <asp:Button class="bitbtndt btngo" runat="server" ID="btnAccAR" Text="..." />									                                                                                                                                                                                                                                                        
                    </td>
                </tr>                
                <tr>
                    <td>Acc. SJ Uninvoice</td>
                    <td>:</td>
                    <td>
                    <asp:TextBox ID="tbAccSJUninvoice" runat="server" AutoPostBack="True" MaxLength="12"
                      CssClass="TextBox" Width="112px" />
                    <asp:TextBox ID="tbAccSJUninvoiceName" runat="server" CssClass="TextBox" 
                       Enabled="False" Width="250px" />
                    <asp:Button class="bitbtndt btngo" runat="server" ID="btnAccSJUninvoice" Text="..." />									                                                                                                                                                                                                                                                        
                    </td>
                </tr>
                <tr>
                   <td>Acc. Disc</td>
                    <td>:</td>
                    <td>
                    <asp:TextBox ID="tbAccDisc" runat="server" AutoPostBack="True" MaxLength="12"
                      CssClass="TextBox" Width="112px" />
                    <asp:TextBox ID="tbAccDiscName" runat="server" CssClass="TextBox" 
                       Enabled="False" Width="250px" />
                    <asp:Button class="bitbtndt btngo" runat="server" ID="btnAccDisc" Text="..." />									                                                                                                                                                                                                                                                        
                    </td>
                </tr>
                <tr>
                   <td>Acc. Other</td>
                    <td>:</td>
                    <td>
                    <asp:TextBox ID="tbAccOther" runat="server" AutoPostBack="True" MaxLength="12"
                      CssClass="TextBox" Width="112px" />
                    <asp:TextBox ID="tbAccOtherName" runat="server" CssClass="TextBox" 
                       Enabled="False" Width="250px" />
                    <asp:Button class="bitbtndt btngo" runat="server" ID="btnAccOther" Text="..." />									                                                                                                                                                                                                                                                        
                    </td>
                </tr>
                <tr>
                   <td>Acc. Credit AR</td>
                    <td>:</td>
                    <td>
                    <asp:TextBox ID="tbAccCreditAR" runat="server" AutoPostBack="True" MaxLength="12"
                      CssClass="TextBox" Width="112px" />
                    <asp:TextBox ID="tbAccCreditARName" runat="server" CssClass="TextBox" 
                       Enabled="False" Width="250px" />
                    <asp:Button class="bitbtndt btngo" runat="server" ID="btnAccCreditAR" Text="..." />									                                                                                                                                                                                                                                                        
                    </td>
                </tr>
                <tr>
                   <td>Acc. DP</td>
                    <td>:</td>
                    <td>
                    <asp:TextBox ID="tbAccDP" runat="server" AutoPostBack="True" MaxLength="12"
                      CssClass="TextBox" Width="112px" />
                    <asp:TextBox ID="tbAccDPName" runat="server" CssClass="TextBox" 
                       Enabled="False" Width="250px" />
                    <asp:Button class="bitbtndt btngo" runat="server" ID="btnAccDP" Text="..." />									                                                                                                                                                                                                                                                        
                    </td>
                </tr>

                <tr>
                   <td>Acc. Deposit</td>
                    <td>:</td>
                    <td>
                    <asp:TextBox ID="tbAccDeposit" runat="server" AutoPostBack="True" MaxLength="12"
                      CssClass="TextBox" Width="112px" />
                    <asp:TextBox ID="tbAccDepositName" runat="server" CssClass="TextBox" 
                       Enabled="False" Width="250px" />
                    <asp:Button class="bitbtndt btngo" runat="server" ID="btnAccDeposit" Text="..." />									                                                                                                                                                                                                                                                        
                    </td>
                </tr>
                
                <tr>
                   <td>Acc. PPN</td>
                    <td>:</td>
                    <td>
                    <asp:TextBox ID="tbAccPPN" runat="server" AutoPostBack="True" MaxLength="12"
                      CssClass="TextBox" Width="112px" />
                    <asp:TextBox ID="tbAccPPNName" runat="server" CssClass="TextBox" 
                       Enabled="False" Width="250px" />
                    <asp:Button class="bitbtndt btngo" runat="server" ID="btnAccPPN" Text="..." />									                                                                                                                                                                                                                                                        
                    </td>
                </tr>
                
                <tr>
                   <td>Acc. Potongan</td>
                    <td>:</td>
                    <td>
                    <asp:TextBox ID="tbAccPotongan" runat="server" AutoPostBack="True" MaxLength="12"
                      CssClass="TextBox" Width="112px" />
                    <asp:TextBox ID="tbAccPotonganName" runat="server" CssClass="TextBox" 
                       Enabled="False" Width="250px" />
                    <asp:Button class="bitbtndt btngo" runat="server" ID="btnAccPotongan" Text="..." />									                                                                                                                                                                                                                                                        
                    </td>
                </tr>

                
                <tr>
                    <td colspan="3" style="text-align: center">
                        <asp:Button class="bitbtndt btnsave" runat="server" ID="btnSavedt" Text="Save" />									                                                                                                                                                        
                        <asp:Button class="bitbtndt btncancel" runat="server" ID="btnCanceldt" Text="Cancel" />									                                                                                                                                                                                                                                                                                                                                                                           
                        
                    </td>
                </tr>
            </table>
            <br />
            <br />
            <br />
       </asp:Panel> 
     
    </div>    
    <asp:SqlDataSource ID="dsPClass" runat="server" 
            SelectCommand="EXEC S_GetProductClass">
    </asp:SqlDataSource>
    
    <asp:SqlDataSource ID="dsCurrency" runat="server" 
            SelectCommand="EXEC S_GetCurrency">
    </asp:SqlDataSource>    
    <asp:Label ID="lstatus" ForeColor="Black" runat="server"></asp:Label>  
    
    
    
    </form>
    </body>
</html>
