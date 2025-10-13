<%@ Page Language="VB" AutoEventWireup="false" CodeFile="MsTerm.aspx.vb" Inherits="Master_MsTerm_MsTerm" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Term File</title>
    <script src="../../Function/Function.JS" type="text/javascript"></script>
    <script src="../../Function/OpenDlg.JS" type="text/javascript"></script> 
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
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div class="Content">
     <div class="H1">Term File</div>
     <hr style="color:Blue" />
     <asp:Panel id="pnlHd" runat="server">
      <table>
        <tr>
            <td style="text-align:right; width:100px" >Quick Search :
            </td>
            <td><asp:TextBox CssClass="TextBox"  runat="server" ID ="tbFilter"/> 
                <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlField" >
                  <asp:ListItem Selected="true" Text="Term Code" Value="Term_Code"></asp:ListItem>
                  <asp:ListItem Text="Term Name" Value="Term_Name"></asp:ListItem>
                  <asp:ListItem Text="Periode" Value="XPeriod"></asp:ListItem>  
                  <asp:ListItem Text="Range every Period" Value="TypeRange"></asp:ListItem>  
                  <asp:ListItem Text="Range Period" Value="xRange"></asp:ListItem>
                  <asp:ListItem Text="Cash Before Delivery" Value="FgCBD"></asp:ListItem>    
                  
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
                      <asp:ListItem Selected="true" Text="Term Code" Value="Term_Code"></asp:ListItem>
                      <asp:ListItem Text="Term Name" Value="Term_Name"></asp:ListItem>
                      <asp:ListItem Text="Periode" Value="XPeriod"></asp:ListItem>  
                      <asp:ListItem Text="Range every Period" Value="TypeRange"></asp:ListItem> 
                      <asp:ListItem Text="Range Period" Value="xRange"></asp:ListItem>
                      <asp:ListItem Text="Cash Before Delivery" Value="FgCBD"></asp:ListItem>   
                  </asp:DropDownList>
                
            </td>
        </tr>
     </table>
     </asp:Panel>
      <br />
      <div style="border:0px  solid; width:100%; height:100%; overflow:auto;">
      <asp:GridView id="DataGrid" runat="server" ShowFooter="True" AllowSorting="True" 
            AutoGenerateColumns="False" AllowPaging="True" CssClass="Grid" Width = "100%" >
						<HeaderStyle CssClass="GridHeader" wrap="True"></HeaderStyle>
						<RowStyle CssClass="GridItem" Wrap="True" />
						<AlternatingRowStyle CssClass="GridAltItem"/>
						<FooterStyle CssClass="GridFooter" />
						<PagerStyle CssClass="GridPager" />
				      <Columns>
							<asp:TemplateField HeaderText="Term Code" HeaderStyle-Width="0" SortExpression="Term_Code">
								<Itemtemplate>
									<asp:Label Runat="server" Width="70" ID="TermCode" text='<%# DataBinder.Eval(Container.DataItem, "Term_Code") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:Label Runat="server" ID="TermCodeEdit" Width="100%" Text='<%# DataBinder.Eval(Container.DataItem, "Term_Code") %>'>
									</asp:Label>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="TermCodeAdd" Placeholder="can't blank" CssClass="TextBox" Width="100%" MaxLength="10" Runat="Server"/>
								    <%--<cc1:TextBoxWatermarkExtender ID="TermCodeAdd_TextBoxWatermarkExtender" 
                                        runat="server" Enabled="True" TargetControlID="TermCodeAdd" WatermarkText="can't blank" WatermarkCssClass="Watermarked" >
                                    </cc1:TextBoxWatermarkExtender>--%>
								</FooterTemplate>
							    <HeaderStyle Width="70px" />
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Term Name" HeaderStyle-Width="140px" SortExpression="Term_Name">
								<Itemtemplate>
									<asp:Label Runat="server" Width="140px" ID="TermName" text='<%# DataBinder.Eval(Container.DataItem, "Term_Name") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:TextBox Runat="server" ID="TermNameEdit" CssClass="TextBox" Width="100%" MaxLength="60" Text='<%# DataBinder.Eval(Container.DataItem, "Term_Name") %>'>
									</asp:TextBox>
									<%--<cc1:TextBoxWatermarkExtender ID="TermNameEdit_TextBoxWatermarkExtender" 
                                        runat="server" Enabled="True" TargetControlID="TermNameEdit" WatermarkText="can't blank" WatermarkCssClass="Watermarked" >
                                    </cc1:TextBoxWatermarkExtender>--%>
								</EditItemTemplate>
								<FooterTemplate>
									
									<asp:TextBox ID="TermNameAdd" Placeholder="can't blank"  CssClass="TextBox" Width="100%" MaxLength="60" 
                                        Runat="Server"></asp:TextBox>
									<%--<cc1:TextBoxWatermarkExtender ID="TermNameAdd_TextBoxWatermarkExtender" 
                                        runat="server" Enabled="True" TargetControlID="TermNameAdd" WatermarkText="can't blank" WatermarkCssClass="Watermarked" >
                                    </cc1:TextBoxWatermarkExtender>--%>
								</FooterTemplate>
							    <HeaderStyle Width="140px" />
							</asp:TemplateField>	
							
							<asp:TemplateField HeaderText="XPeriod" ItemStyle-HorizontalAlign = "Right"  HeaderStyle-Width="15" SortExpression="XPeriod">
								<Itemtemplate>
									<asp:Label Runat="server" Width="15" ID="XPeriod" TEXT='<%# DataBinder.Eval(Container.DataItem, "XPeriod") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:TextBox Runat="server" ID="XPeriodEdit" MaxLength="9" CssClass="TextBox" Width="100%" TEXT='<%# DataBinder.Eval(Container.DataItem, "XPeriod") %>'>
									</asp:TextBox>
								</EditItemTemplate>

								<FooterTemplate>
									<asp:TextBox ID="XPeriodAdd" CssClass="TextBox" Width="100%" MaxLength="9" 
                                        Runat="Server">1</asp:TextBox>
								    <cc1:TextBoxWatermarkExtender ID="XPeriodAdd_TextBoxWatermarkExtender" 
                                        runat="server" Enabled="True" TargetControlID="XPeriodAdd" WatermarkText="can't blank" WatermarkCssClass="Watermarked" >
                                    </cc1:TextBoxWatermarkExtender>
								</FooterTemplate>
							    <HeaderStyle Width="15px" />
							</asp:TemplateField>			
							
				            <asp:TemplateField HeaderText="Range every Period" HeaderStyle-Width="80" ItemStyle-HorizontalAlign="Center" SortExpression="TypeRange">
								<Itemtemplate>
									<asp:Label Runat="server" Width="80" ID="TypeRange" TEXT='<%# DataBinder.Eval(Container.DataItem, "TypeRange") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:DropDownList ID="TypeRangeEdit" CssClass="DropDownList" Width="100%" runat="server" SelectedValue='<%# DataBinder.Eval(Container.DataItem, "TypeRange") %>'>
									  <asp:ListItem>day</asp:ListItem>
									  <asp:ListItem>week</asp:ListItem>                                        
									  <asp:ListItem>month</asp:ListItem>                                        
								</asp:DropDownList>								    
								</EditItemTemplate>
								<FooterTemplate>
									<asp:DropDownList ID="TypeRangeAdd" CssClass="DropDownList" Width="100%" runat="server">
									  <asp:ListItem Selected="True">day</asp:ListItem>
									  <asp:ListItem>week</asp:ListItem>                                        
									  <asp:ListItem>month</asp:ListItem>                               
									</asp:DropDownList>								    
								</FooterTemplate>
							    <HeaderStyle Width="80px" />
                                <ItemStyle HorizontalAlign="Center" />
							</asp:TemplateField>				
							
							<asp:TemplateField HeaderText="Range Period" ItemStyle-HorizontalAlign = "Right"  HeaderStyle-Width="70" SortExpression="XRange">
								<Itemtemplate>
									<asp:Label Runat="server" Width="70" ID="XRange" TEXT='<%# DataBinder.Eval(Container.DataItem, "XRange") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:TextBox Runat="server" ID="XRangeEdit" CssClass="TextBox" Width="100%" MaxLength="9"  TEXT='<%# DataBinder.Eval(Container.DataItem, "XRange") %>'>
									</asp:TextBox>
								</EditItemTemplate>
								<FooterTemplate>
								    <asp:TextBox ID="XRangeAdd" CssClass="TextBox" Width="100%" MaxLength="9" 
                                        Runat="Server">1</asp:TextBox>						    
									
								    <cc1:TextBoxWatermarkExtender ID="XRangeAdd_TextBoxWatermarkExtender" 
                                        runat="server" Enabled="True" TargetControlID="XRangeAdd" WatermarkText="can't blank" WatermarkCssClass="Watermarked" >
                                    </cc1:TextBoxWatermarkExtender>
								</FooterTemplate>
							    <HeaderStyle Width="70px" />
							</asp:TemplateField>
							
		                    <asp:TemplateField HeaderText="Cash Before Delivery" HeaderStyle-Width="40" ItemStyle-HorizontalAlign="Center" SortExpression="FgCBD">
								<Itemtemplate>
									<asp:Label Runat="server" Width="40" ID="FgCBD" TEXT='<%# DataBinder.Eval(Container.DataItem, "FgCBD") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:DropDownList ID="FgCBDEdit" CssClass="DropDownList" Width="100%" runat="server" SelectedValue='<%# DataBinder.Eval(Container.DataItem, "FgCBD") %>'>
									  <asp:ListItem>Y</asp:ListItem>
									  <asp:ListItem>N</asp:ListItem>                                                                            
								</asp:DropDownList>								    
								</EditItemTemplate>
								<FooterTemplate>
									<asp:DropDownList ID="FgCBDAdd" CssClass="DropDownList" Width="100%" runat="server">
									  <asp:ListItem Selected="True">Y</asp:ListItem>                                     
									  <asp:ListItem>N</asp:ListItem>                               
									</asp:DropDownList>								    
								</FooterTemplate>
							    <HeaderStyle Width="40px" />
                                <ItemStyle HorizontalAlign="Center" />
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Action" headerstyle-width="260">
							    <ItemTemplate>
								    <asp:Button ID="btnEdit" runat="server" class="bitbtndt btnedit" Text="Edit" CommandName="Edit"/>
									<asp:Button ID="btndelete" runat="server" class="bitbtndt btndelete" Text="Delete" OnClientClick="return confirm('Sure to delete this data?');" CommandName="Delete"/>									
									<asp:Button ID="btnView" runat="server" class="bitbtndt btnedit" Text="Detail" CommandName="View" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>"/>									
								</ItemTemplate>
								<EditItemTemplate>
									<asp:Button ID="btnUpdate" runat="server" class="bitbtndt btnsave" Text="Save" CommandName="Update"/>									
                                    <asp:Button ID="btnCancel" runat="server" class="bitbtndt btncancel" Text="Cancel" CommandName="Cancel"/>									
								</EditItemTemplate>
								<FooterTemplate>
									<asp:Button class="bitbtndt btnadd" runat="server" ID="btnAdd" Text="Add" CommandName="Insert"/>									
								</FooterTemplate>
								<HeaderStyle Width="180" />
							</asp:TemplateField>
							
    					</Columns>
        </asp:GridView>
      </div>
            </asp:Panel>
     <asp:Panel ID="pnlDt" runat="server" Visible = "false">  
     <asp:Label ID="label1" CssClass="H1" runat="server" Text="Term : " />   
     <asp:Label ID="lbTerm" ForeColor="Blue" CssClass="H1" Font-Bold="True" 
             runat="server" />
     <asp:Label ID="Label5" ForeColor="Blue" CssClass="H1" Font-Bold="True" Text=" - "
             runat="server" />
     <asp:Label ID="lbTermName" ForeColor="Blue" CssClass="H1" Font-Bold="True" 
             runat="server" />
     &nbsp &nbsp &nbsp &nbsp
     <asp:Label ID="label2" CssClass="H1" runat="server" Text="Total Period : " />   
     <asp:Label ID="lbPeriod" ForeColor="Blue" CssClass="H1" Font-Bold="True" 
             runat="server" />
     &nbsp &nbsp &nbsp &nbsp
     <asp:Label ID="label4" CssClass="H1" runat="server" Text="Range : " />  
     <asp:Label ID="lbXRange" ForeColor="Blue" CssClass="H1" Font-Bold="True" 
             runat="server" /> 
     <asp:Label ID="Label3" ForeColor="Blue" CssClass="H1" Font-Bold="True" Text=" - "
             runat="server" />
     <asp:Label ID="lbTypeRange" ForeColor="Blue" CssClass="H1" Font-Bold="True" 
             runat="server" />    
    <asp:Label ID="lbFgCBD" ForeColor="Blue" CssClass="H1" Font-Bold="True" 
             runat="server" />           
	 <br />
	 <asp:Button class="bitbtn btnback" runat="server" ID="btnBackDtTop" Text="Back" />
	 <asp:Button class="bitbtn btnapply" runat="server" ID="btnApply" Text="Apply" />
	 <asp:Button class="bitbtn btnsearch" runat="server" ID="btnGet" Text="Get Data" />
	 
	    				                         <%--   <asp:ImageButton ID="btnBackDtTop" runat="server" 
                                                        ImageUrl="../../Image/btnBackon.png"
                                                        onmouseover="this.src='../../Image/btnBackoff.png';"
                                                        onmouseout="this.src='../../Image/btnBackon.png';"
                                                        ImageAlign="AbsBottom" />
	     				                            <asp:ImageButton ID="btnApply" runat="server" 
                                                        ImageUrl="../../Image/btnApplyOn.png"
                                                        onmouseover="this.src='../../Image/btnApplyOff.png';"
                                                        onmouseout="this.src='../../Image/btnApplyOn.png';"
                                                        ImageAlign="AbsBottom" />
	                                                 <asp:ImageButton ID="btnGet" runat="server" 
                                                        ImageUrl="../../Image/btnGetDataon.png"
                                                        onmouseover="this.src='../../Image/btnGetDataoff.png';"
                                                        onmouseout="this.src='../../Image/btnGetDataon.png';"
                                                        ImageAlign="AbsBottom" />--%>
	 <br />
	 
	    <div style="border:0px  solid; width:100%; height:100%; overflow:auto;">
            <asp:GridView ID="DataGridDt" runat="server" AutoGenerateColumns="False" 
                CssClass="Grid">
                <HeaderStyle CssClass="GridHeader" />
                <RowStyle CssClass="GridItem" />
                <AlternatingRowStyle CssClass="GridAltItem" />
                <PagerStyle CssClass="GridPager" />
                <Columns>
                    <asp:TemplateField HeaderText="Period" ItemStyle-HorizontalAlign = "Right" >
                        <Itemtemplate>
                            <asp:Label ID="Period" Runat="server" 
                                text='<%# DataBinder.Eval(Container.DataItem, "Period") %>' />
                        </Itemtemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Type Range" SortExpression="TypeRange">
                        <ItemTemplate>
                            <asp:Label ID="TypeRange" Runat="server" 
                                text='<%# DataBinder.Eval(Container.DataItem, "TypeRange") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="X Range 1" ItemStyle-HorizontalAlign = "Right" SortExpression="XRange">
                        <ItemTemplate>                             
                            <asp:TextBox ID="XRange" runat="server" CssClass="TextBox" style="text-align:right;"
                                text='<%# DataBinder.Eval(Container.DataItem, "Xrange") %>' />                        
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Percentage (%)" ItemStyle-HorizontalAlign = "Right"  SortExpression="Percentage" >
                        <Itemtemplate>
                            <asp:TextBox ID="Percentage" runat="server" CssClass="TextBox" style="text-align:right;"
                                text='<%# DataBinder.Eval(Container.DataItem, "Percentage") %>' />
                        </Itemtemplate>
                        <ItemTemplate>
                            <asp:TextBox ID="Percentage" runat="server" CssClass="TextBox" style="text-align:right;"
                                text='<%# DataBinder.Eval(Container.DataItem, "Percentage") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="PPN (%)" ItemStyle-HorizontalAlign = "Right" SortExpression="PPN">
                        <Itemtemplate>
                            <asp:TextBox ID="PPN" runat="server" CssClass="TextBox" style="text-align:right;"
                                text='<%# DataBinder.Eval(Container.DataItem, "PPN") %>' />
                        </Itemtemplate>
                       <ItemTemplate>
                            <asp:TextBox ID="PPN" runat="server" CssClass="TextBox" style="text-align:right;"
                                text='<%# DataBinder.Eval(Container.DataItem, "PPN") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <br />
        </div>        
	    <asp:Button class="bitbtn btnback" runat="server" ID="Button2" Text="Back" />
	                         <%--<asp:ImageButton ID="Button2" runat="server" CommandName="Cancel" 
                                                        ImageUrl="../../Image/btnBackon.png"
                                                        onmouseover="this.src='../../Image/btnBackoff.png';"
                                                        onmouseout="this.src='../../Image/btnBackon.png';"
                                                        ImageAlign="AbsBottom" />--%>

     </asp:Panel>   
     </div>   
  


    <asp:Label ID="lstatus" ForeColor="red" runat="server"></asp:Label>

    </form>
    </body>
</html>
