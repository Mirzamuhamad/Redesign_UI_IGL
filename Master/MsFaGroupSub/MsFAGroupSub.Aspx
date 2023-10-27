<%@ Page Language="VB" AutoEventWireup="false" CodeFile="MsFAGroupSub.Aspx.vb" Inherits="Master_MsFaGroupSub_MsFAGroupSub" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>FA Sub Group File</title>
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
     <div class="H1"><asp:Label runat="server" ID="lblTitle"></asp:Label></div>
     <hr style="color:Blue" />
     <asp:Panel id="pnlHd" runat="server">
      <table>
        <tr>
            <td style="text-align:right; width:100px" >Quick Search :
            </td>
            <td><asp:TextBox CssClass="TextBox"  runat="server" ID ="tbFilter"/> 
                <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlField" >
                  <asp:ListItem Selected="true" Text="FA Sub Group Code" Value="FASubGrpCode"></asp:ListItem>
                  <asp:ListItem Text="FA Sub Group Name" Value="FASubGrpName"></asp:ListItem>
                  <asp:ListItem Text="FA Group" Value="FAGroupName"></asp:ListItem>
                  <asp:ListItem Text="Fg Moving" Value="FgMoving"></asp:ListItem>
                  <asp:ListItem Text="Fg Process" Value="FgProcess"></asp:ListItem>                  
                  <asp:ListItem Text="Percent NDA" Value="PercentNDA"></asp:ListItem>
                  <asp:ListItem Text="Life Month" Value="LifeMonth"></asp:ListItem>
                  <asp:ListItem Text="Method Depr." Value="MethodDepr"></asp:ListItem>                  
                  <asp:ListItem Text="Expendable" Value="FgExpendable"></asp:ListItem>                                    
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
            <td><asp:TextBox CssClass="Button" runat="server" ID ="tbfilter2"/> 
                  <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlField2" >
                      <asp:ListItem Selected="true" Text="FA Sub Group Code" Value="FASubGrpCode"></asp:ListItem>
                      <asp:ListItem Text="FA Sub Group Name" Value="FASubGrpName"></asp:ListItem>
                      <asp:ListItem Text="FA Group" Value="FAGroupName"></asp:ListItem>
                      <asp:ListItem Text="Fg Moving" Value="FgMoving"></asp:ListItem>
                      <asp:ListItem Text="Fg Process" Value="FgProcess"></asp:ListItem>
                      <asp:ListItem Text="Percent NDA" Value="PercentNDA"></asp:ListItem>
                      <asp:ListItem Text="Life Month" Value="LifeMonth"></asp:ListItem>
                      <asp:ListItem Text="Method Depr." Value="MethodDepr"></asp:ListItem>                  
                      <asp:ListItem Text="Expendable" Value="FgExpendable"></asp:ListItem>                                    

                  </asp:DropDownList>
                
            </td>
        </tr>
     </table>
     </asp:Panel>
      <br />
      <div style="border:0px  solid; width:100%; height:100%; overflow:auto;">
      <asp:GridView width = 1800 id="DataGrid" runat="server" ShowFooter="True" AllowSorting="True" 
            AutoGenerateColumns="False" AllowPaging="True" CssClass="Grid">
						<HeaderStyle CssClass="GridHeader" wrap="false"></HeaderStyle>
						<RowStyle CssClass="GridItem" Wrap="false"/>
						<AlternatingRowStyle CssClass="GridAltItem"/>
						<FooterStyle CssClass="GridFooter" />
						<PagerStyle CssClass="GridPager" />
				      <Columns>
							<asp:TemplateField HeaderText="FA Sub Group Code" HeaderStyle-Width="50" SortExpression="FASubGrpCode">
								<Itemtemplate>
									<asp:Label Runat="server" Width="50" ID="FASubGrpCode" text='<%# DataBinder.Eval(Container.DataItem, "FASubGrpCode") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:Label Runat="server" ID="FASubGrpCodeEdit" Width="100%" Text='<%# DataBinder.Eval(Container.DataItem, "FASubGrpCode") %>'>
									</asp:Label>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="FASubGrpCodeAdd" placeholder ="can't blank"  CssClass="TextBox" Width="100%" MaxLength="5" Runat="Server"/>
								   <%-- <cc1:TextBoxWatermarkExtender ID="FASubGrpCodeAdd_TextBoxWatermarkExtender" 
                                        runat="server" Enabled="True" TargetControlID="FASubGrpCodeAdd" WatermarkText="can't blank" WatermarkCssClass="Watermarked" >
                                    </cc1:TextBoxWatermarkExtender>--%>
								</FooterTemplate>
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="FA Sub Group Name" HeaderStyle-Width="280" SortExpression="FASubGrpName">
								<Itemtemplate>
									<asp:Label Runat="server" Width="280" ID="FASubGrpName" text='<%# DataBinder.Eval(Container.DataItem, "FASubGrpName") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:TextBox Runat="server" ID="FASubGrpNameEdit" CssClass="TextBox" Width="100%"  MaxLength="60" Text='<%# DataBinder.Eval(Container.DataItem, "FASubGrpName") %>'>
									</asp:TextBox>
									<%--<cc1:TextBoxWatermarkExtender ID="FASubGrpNameEdit_TextBoxWatermarkExtender" 
                                        runat="server" Enabled="True" TargetControlID="FASubGrpNameEdit" WatermarkText="can't blank" WatermarkCssClass="Watermarked" >
                                    </cc1:TextBoxWatermarkExtender>--%>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="FASubGrpNameAdd" placeholder ="can't blank"  CssClass="TextBox" Width="100%" MaxLength="60"  Runat="Server"/>
									<%--<cc1:TextBoxWatermarkExtender ID="FASubGrpNameAdd_TextBoxWatermarkExtender" 
                                        runat="server" Enabled="True" TargetControlID="FASubGrpNameAdd" WatermarkText="can't blank" WatermarkCssClass="Watermarked" >
                                    </cc1:TextBoxWatermarkExtender>--%>
								</FooterTemplate>
							</asp:TemplateField>	
							
							<asp:TemplateField HeaderText="FA Group" HeaderStyle-Width="220" SortExpression="FAGroup">
								<Itemtemplate>
									<asp:Label Runat="server" Width="220" ID="FAGroup" text='<%# DataBinder.Eval(Container.DataItem, "FAGroupName") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:DropDownList CssClass="DropDownList" ID="FAGroupEdit" Width="100%" runat="server" 
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "FAGroup") %>' 
                                        DataSourceID="dsFAGroup" DataTextField="FAGroupName" 
                                        DataValueField="FAGroupCode">									
									</asp:DropDownList>								    
								</EditItemTemplate>
								<FooterTemplate>
									<asp:DropDownList ID="FAGroupAdd" CssClass="DropDownList" Width="100%" runat="server"                                         
                                        DataSourceID="dsFAGroup" DataTextField="FAGroupName" 
                                        DataValueField="FAGroupCode">									
									</asp:DropDownList>
								</FooterTemplate>
							</asp:TemplateField>	
							
							<asp:TemplateField HeaderText="Moving" HeaderStyle-Width="5px" ItemStyle-HorizontalAlign="Center" SortExpression="FgMoving">
								<Itemtemplate>
									<asp:Label Runat="server" Width="10" ID="FgMoving" text='<%# DataBinder.Eval(Container.DataItem, "FgMoving") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:DropDownList ID="FgMovingEdit" CssClass="DropDownList" Width="100%" runat="server" SelectedValue='<%# DataBinder.Eval(Container.DataItem, "FgMoving") %>'>
									  <asp:ListItem>Y</asp:ListItem>
									  <asp:ListItem>N</asp:ListItem>                                        
									</asp:DropDownList>								    
								</EditItemTemplate>
								<FooterTemplate>
									<asp:DropDownList ID="FgMovingAdd" CssClass="DropDownList" Width="100%" runat="server">
									  <asp:ListItem Selected="True">Y</asp:ListItem>
									  <asp:ListItem>N</asp:ListItem>                                        
									</asp:DropDownList>								    
								</FooterTemplate>
							</asp:TemplateField>	
							
							<asp:TemplateField HeaderText="Process" HeaderStyle-Width="20" ItemStyle-HorizontalAlign="Center" SortExpression="FgProcess">
								<Itemtemplate>
									<asp:Label Runat="server" Width="20" ID="FgProcess" text='<%# DataBinder.Eval(Container.DataItem, "FgProcess") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:DropDownList ID="FgProcessEdit" CssClass="DropDownList" Width="100%" runat="server" SelectedValue='<%# DataBinder.Eval(Container.DataItem, "FgProcess") %>'>
									  <asp:ListItem>Y</asp:ListItem>
									  <asp:ListItem>N</asp:ListItem>                                        
									</asp:DropDownList>								    
								</EditItemTemplate>
								<FooterTemplate>
									<asp:DropDownList ID="FgProcessAdd" CssClass="DropDownList" Width="100%" runat="server">
									  <asp:ListItem Selected="True">Y</asp:ListItem>
									  <asp:ListItem>N</asp:ListItem>                                        
									</asp:DropDownList>								    
								</FooterTemplate>
							</asp:TemplateField>
							
							
							<asp:TemplateField HeaderText="NDA (%)" ItemStyle-HorizontalAlign = "Right" HeaderStyle-Width="70" SortExpression="PercentNDA">
								<Itemtemplate>
									<asp:Label Runat="server" Width="70" ID="PercentNDA" text='<%# DataBinder.Eval(Container.DataItem, "PercentNDA") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>	
								   <asp:TextBox Runat="server" ID="PercentNDAEdit" CssClass="TextBox" Width="100%"  MaxLength="9" Text='<%# DataBinder.Eval(Container.DataItem, "PercentNDA") %>'>
									</asp:TextBox>															   
									
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="PercentNDAAdd" CssClass="TextBox" Maxlength = "20" Text="0" Width="100%" Runat="Server"/>								    
								</FooterTemplate>
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Life (month)" ItemStyle-HorizontalAlign = "Right" HeaderStyle-Width="50" SortExpression="LifeMonth">
								<Itemtemplate>
									<asp:Label Runat="server" Width="50" ID="LifeMonth" text='<%# DataBinder.Eval(Container.DataItem, "LifeMonth") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
								 <asp:TextBox Runat="server" ID="LifeMonthEdit" CssClass="TextBox" Width="100%"  MaxLength="4" Text='<%# DataBinder.Eval(Container.DataItem, "LifeMonth") %>'>
									</asp:TextBox>
								
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="LifeMonthAdd" CssClass="TextBox" Width="100%" Maxlength = "4" Runat="Server"/>								    
								</FooterTemplate>
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Method Depr." HeaderStyle-Width="70" SortExpression="MethodDepr">
								<Itemtemplate>
									<asp:Label Runat="server" Width="70" ID="MethodDepr" text='<%# DataBinder.Eval(Container.DataItem, "MethodDepr") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:DropDownList ID="MethodDeprEdit" CssClass="DropDownList" Width="100%" Maxlength = "20" runat="server" SelectedValue='<%# DataBinder.Eval(Container.DataItem, "MethodDepr") %>'>
									  <asp:ListItem>Straight Line</asp:ListItem>
									  <asp:ListItem>Double Declining</asp:ListItem>                                        
									</asp:DropDownList>								    
								</EditItemTemplate>
								<FooterTemplate>
									<asp:DropDownList ID="MethodDeprAdd" CssClass="DropDownList" Width="100%" Maxlength = "20" runat="server">
									  <asp:ListItem Selected="True">Straight Line</asp:ListItem>
									  <asp:ListItem>Double Declining</asp:ListItem>                                        
									</asp:DropDownList>								    
								</FooterTemplate>
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Expendable" HeaderStyle-Width="25" ItemStyle-HorizontalAlign="Center" SortExpression="FgExpendable">
								<Itemtemplate>
									<asp:Label Runat="server" Width="25" ID="FgExpendable" text='<%# DataBinder.Eval(Container.DataItem, "FgExpendable") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:DropDownList ID="FgExpendableEdit" CssClass="DropDownList" Width="100%" runat="server" SelectedValue='<%# DataBinder.Eval(Container.DataItem, "FgExpendable") %>'>
									  <asp:ListItem>N</asp:ListItem>
									  <asp:ListItem>Y</asp:ListItem>                                        
									</asp:DropDownList>								    
								</EditItemTemplate>
								<FooterTemplate>
									<asp:DropDownList ID="FgExpendableAdd" CssClass="DropDownList" Width="100%" runat="server">
									  <asp:ListItem Selected="True">N</asp:ListItem>
									  <asp:ListItem>Y</asp:ListItem>                                        
									</asp:DropDownList>								    
								</FooterTemplate>
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Action" headerstyle-width="100" >
								<ItemTemplate>
								    <asp:Button ID="btnEdit" runat="server" class="bitbtndt btnedit" Text="Edit" CommandName="Edit"/>
									<asp:Button ID="btnDelete" runat="server" class="bitbtndt btndelete" Text="Delete" OnClientClick="return confirm('Sure to delete this data?');" CommandName="Delete"/>									
									<asp:Button ID="btnView" runat="server" class="bitbtndt btnedit" Text="Detail" CommandName="View" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>"/>
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
     </asp:Panel>
     <asp:Panel ID="pnlDt" runat="server" Visible = "false">  
     <asp:Label ID="label1" CssClass="H1" runat="server" Text="FA Sub Group : " />   
     <asp:Label ID="lbFASubGroupCode" ForeColor="Blue" CssClass="H1" Font-Bold="true" runat="server" />
	 <br />
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
							<asp:TemplateField HeaderText="Currency" HeaderStyle-Width="80" SortExpression="CurrCode">
								<Itemtemplate>
									<asp:Label Runat="server" Width="80" ID="CurrCode" text='<%# DataBinder.Eval(Container.DataItem, "CurrCode") %>'>
									</asp:Label>
								</Itemtemplate>			
								<EditItemTemplate>
								    <asp:Label runat="server" ID="CurrCodeEdit" text='<%# DataBinder.Eval(Container.DataItem, "CurrCode") %>' />
								</EditItemTemplate>	
								<FooterTemplate>								    
								    <asp:DropDownList ID="CurrCodeAdd" CssClass="DropDownList" runat="server" DataSourceID="dsCurrency" Width="100%" 
                                        DataTextField="Currency" DataValueField="Currency" AutoPostBack="true"
                                        onselectedindexchanged="CurrCodeAdd_SelectedIndexChanged">
                                    </asp:DropDownList>								    
								</FooterTemplate>											
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Acc. FA" HeaderStyle-Width="128" SortExpression="AccFA">
								<Itemtemplate>
									<asp:Label Runat="server" Width="128" ID="AccFA" text='<%# DataBinder.Eval(Container.DataItem, "AccFA") %>'>
									</asp:Label>
								</Itemtemplate>	
								<EditItemTemplate>
								    <asp:TextBox CssClass="TextBox" runat="server" Width="90" ID="AccFAEdit" text='<%# DataBinder.Eval(Container.DataItem, "AccFA") %>' 
                                        AutoPostBack="true" ontextchanged="tbAccFA_TextChanged"/>
                                    <asp:Button class="btngo" runat="server" ID="btnAccFAEdit" CommandName="SearchFAEdit" Text="..."/>      
								</EditItemTemplate>							
								<FooterTemplate>
								    <asp:TextBox CssClass="TextBox" OnTextChanged="tbAccFA_TextChanged" runat="server" id="AccFAAdd" Width="90" AutoPostBack="true" />
                                    <asp:Button class="btngo" runat="server" ID="btnAccFAAdd" CommandName="SearchFAAdd" Text="..."/>
                                </FooterTemplate>
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Acc. FA Name" HeaderStyle-Width="280" SortExpression="AccFAName">
								<Itemtemplate>
									<asp:Label Runat="server" Width="280" ID="AccFAName" text='<%# DataBinder.Eval(Container.DataItem, "AccFAName") %>'>
									</asp:Label>
								</Itemtemplate>	
								<EditItemTemplate>
								    <asp:Label Runat="server" ID="AccFANameEdit" Width="100%" text='<%# DataBinder.Eval(Container.DataItem, "AccFAName") %>'>
									</asp:Label>
								</EditItemTemplate>	
								<FooterTemplate>
								    <asp:Label Runat="server" ID="AccFANameAdd" Width="280" >
									</asp:Label>
								</FooterTemplate>								
							</asp:TemplateField>	
							
							<asp:TemplateField HeaderText="Acc. Depreciation" HeaderStyle-Width="128" SortExpression="AccDepr">
								<Itemtemplate>
									<asp:Label Runat="server" ID="AccDepr" Width ="128" text='<%# DataBinder.Eval(Container.DataItem, "AccDepr") %>'>
									</asp:Label>
								</Itemtemplate>	
								<EditItemTemplate>
								    <asp:TextBox CssClass="TextBox" runat="server" ID="AccDeprEdit" Width="90" 
                                        text='<%# DataBinder.Eval(Container.DataItem, "AccDepr") %>' 
                                        AutoPostBack="true" ontextchanged="tbAccDepr_TextChanged"/>
                                    <asp:Button class="btngo" runat="server" ID="BtnAccDeprEdit" CommandName="SearchDeprEdit" Text="..."/>
								</EditItemTemplate>							
								<FooterTemplate>
								    <asp:TextBox CssClass="TextBox" OnTextChanged="tbAccDepr_TextChanged" runat="server" id="AccDeprAdd" Width="90" AutoPostBack="true" />
                                    <asp:Button class="btngo" runat="server" ID="btnAccDeprAdd" CommandName="SearchDeprAdd" Text="..."/>                                       
								</FooterTemplate>							
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Acc. Depreciation Name" HeaderStyle-Width="280" SortExpression="AccDeprName">
								<Itemtemplate>
									<asp:Label Runat="server" Width="280" ID="AccDeprName" text='<%# DataBinder.Eval(Container.DataItem, "AccDeprName") %>'></asp:Label>
								</Itemtemplate>	
								<EditItemTemplate>
								    <asp:Label Runat="server" ID="AccDeprNameEdit" text='<%# DataBinder.Eval(Container.DataItem, "AccDeprName") %>'></asp:Label>
								</EditItemTemplate>	
								<FooterTemplate>
								    <asp:Label Runat="server" ID="AccDeprNameAdd" Width="280"></asp:Label>
								</FooterTemplate>								
							</asp:TemplateField>	
							
							<asp:TemplateField HeaderText="Acc. Akum. Depr." HeaderStyle-Width="128" SortExpression="AccAkumDepr">
								<Itemtemplate>
									<asp:Label Runat="server" ID="AccAkumDepr" Width ="128" text='<%# DataBinder.Eval(Container.DataItem, "AccAkumDepr") %>'>
									</asp:Label>
								</Itemtemplate>	
								<EditItemTemplate>
								    <asp:TextBox CssClass="TextBox" runat="server" ID="AccAkumDeprEdit" Width="90" text='<%# DataBinder.Eval(Container.DataItem, "AccAkumDepr") %>' 
                                        AutoPostBack="true" ontextchanged="tbAccAkumDepr_TextChanged"/>
                                    <asp:Button class="btngo" runat="server" ID="BtnAccAkumDeprEdit" CommandName="SearchAkumDeprEdit" Text="..."/>
								</EditItemTemplate>							
								<FooterTemplate>
								    <asp:TextBox CssClass="TextBox" OnTextChanged="tbAccAkumDepr_TextChanged" runat="server" id="AccAkumDeprAdd" Width="90" 
                                        AutoPostBack="true" />
                                    <asp:Button class="btngo" runat="server" ID="btnAccAkumDeprAdd" CommandName="SearchAkumDeprAdd" Text="..."/>
								</FooterTemplate>									
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Acc. Akum. Depr. Name" HeaderStyle-Width="280" SortExpression="AccAkumDeprName">
								<Itemtemplate>
									<asp:Label Runat="server" ID="AccAkumDeprName" Width="280" text='<%# DataBinder.Eval(Container.DataItem, "AccAkumDeprName") %>'></asp:Label>
								</Itemtemplate>		
								<EditItemTemplate>
								    <asp:Label Runat="server" ID="AccAkumDeprNameEdit" text='<%# DataBinder.Eval(Container.DataItem, "AccAkumDeprName") %>'></asp:Label>
								</EditItemTemplate>	
								<FooterTemplate>
								    <asp:Label Runat="server" ID="AccAkumDeprNameAdd" Width="280"></asp:Label>
								</FooterTemplate>						
							</asp:TemplateField>	
							
							<asp:TemplateField HeaderText="Acc. Sales" HeaderStyle-Width="128" SortExpression="AccSales">
								<Itemtemplate>
									<asp:Label Runat="server" ID="AccSales" Width ="128" text='<%# DataBinder.Eval(Container.DataItem, "AccSales") %>'>
									</asp:Label>
								</Itemtemplate>	
								<EditItemTemplate>
								    <asp:TextBox CssClass="TextBox" runat="server" ID="AccSalesEdit" Width="90" text='<%# DataBinder.Eval(Container.DataItem, "AccSales") %>' AutoPostBack="true" ontextchanged="tbAccSales_TextChanged"/>
                                    <asp:Button class="btngo" runat="server" ID="BtnAccSalesEdit" CommandName="SearchSalesEdit" Text="..." CommandArgument='<%# Container.DataItemIndex %>'/>
                                </EditItemTemplate>							
								<FooterTemplate>
								    <asp:TextBox CssClass="TextBox" OnTextChanged="tbAccSales_TextChanged" runat="server" id="AccSalesAdd" Width="90" AutoPostBack="true" />
								    <asp:Button class="btngo" runat="server" ID="btnAccSalesAdd" CommandName="SearchSalesAdd" Text="..." CommandArgument='<%# Container.DataItemIndex %>'/>
								</FooterTemplate>								
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Acc. Sales Name" HeaderStyle-Width="280" SortExpression="AccSalesName">
								<Itemtemplate>
									<asp:Label Runat="server" ID="AccSalesName" Width="280" text='<%# DataBinder.Eval(Container.DataItem, "AccSalesName") %>'>
									</asp:Label>
								</Itemtemplate>			
								<EditItemTemplate>
								    <asp:Label Runat="server" ID="AccSalesNameEdit" text='<%# DataBinder.Eval(Container.DataItem, "AccSalesName") %>'>
									</asp:Label>
								</EditItemTemplate>	
								<FooterTemplate>
								    <asp:Label Runat="server" ID="AccSalesNameAdd" Width="280">
									</asp:Label>
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
       <asp:Button class="bitbtn btnback" runat="server" ID="button2" Text="Back" />
     </asp:Panel>   
    </div>    
    <asp:SqlDataSource ID="dsFAGroup" runat="server"       
      SelectCommand="EXEC S_GetFAGroup">
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="dsCurrency" runat="server"          
      SelectCommand="EXEC S_GetCurrency">
    </asp:SqlDataSource>
    </form>
    <asp:Label ID="lstatus" ForeColor="red" runat="server"></asp:Label>
</body>
</html>
