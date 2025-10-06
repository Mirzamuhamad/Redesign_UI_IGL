<%@ Page Language="VB" AutoEventWireup="false" CodeFile="MsRecruitment.aspx.vb" Inherits="Master_MsRecruitment_MsRecruitment" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitle</title>
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
     <div class="H1">Recruitment File</div>
     <hr style="color:Blue" />
     <asp:Panel id="pnlHd" runat="server">
        <table>
        <tr>
            <td style="width:100px;text-align:right" >Quick Search :</td>
            <td><asp:TextBox runat="server" ID ="tbFilter" CssClass="TextBox"/> 
                <asp:DropDownList runat="server" ID="ddlField" CssClass="DropDownList" >
                  <asp:ListItem Selected="true" Text="Recruitment Code" Value="RecruitmentCode"></asp:ListItem>
                  <asp:ListItem Text="Recruitment Name" Value="RecruitmentName"></asp:ListItem>        
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
                    <asp:ListItem Selected="true" Text="Recruitment Code" Value="RecruitmentCode"></asp:ListItem>
                    <asp:ListItem Text="Recruitment Name" Value="RecruitmentName"></asp:ListItem>        
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
							<asp:TemplateField HeaderText="Recruitment Code" SortExpression="RecruitmentCode">
								<Itemtemplate>
									<asp:Label Runat="server" ID="RecruitmentCode" text='<%# DataBinder.Eval(Container.DataItem, "RecruitmentCode") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:Label Runat="server" ID="RecruitmentCodeEdit" Text='<%# DataBinder.Eval(Container.DataItem, "RecruitmentCode") %>'>
									</asp:Label>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="RecruitmentCodeAdd" CssClass="TextBox" MaxLength="5" Runat="Server" Width="100%"/>
									<cc1:TextBoxWatermarkExtender ID="RecruitmentCodeAdd_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="RecruitmentCodeAdd" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>
								</FooterTemplate>
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Recruitment Name" HeaderStyle-Width="360" SortExpression="RecruitmentName">
								<Itemtemplate>
									<asp:Label Runat="server" ID="RecruitmentName" text='<%# DataBinder.Eval(Container.DataItem, "RecruitmentName") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:TextBox Runat="server" CssClass="TextBox" ID="RecruitmentNameEdit" MaxLength="50" Width="100%" Text='<%# DataBinder.Eval(Container.DataItem, "RecruitmentName") %>'>
									</asp:TextBox>
									<cc1:TextBoxWatermarkExtender ID="RecruitmentNameEdit_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="RecruitmentNameEdit" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="RecruitmentNameAdd" CssClass="TextBox" MaxLength="50" Runat="Server" Width="100%"/>
									<cc1:TextBoxWatermarkExtender ID="RecruitmentNameAdd_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="RecruitmentNameAdd" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>
								</FooterTemplate>

<HeaderStyle Width="360px"></HeaderStyle>
							</asp:TemplateField>		
							
							<asp:TemplateField HeaderText="Action" ItemStyle-HorizontalAlign="Left" >
							    <ItemTemplate>
								    <asp:Button ID="btnEdit" runat="server" class="bitbtn btnedit" Text="Edit" CommandName="Edit"/>
									<asp:Button ID="btndelete" runat="server" class="bitbtn btndelete" Text="Delete" OnClientClick="return confirm('Sure to delete this data?');" CommandName="Delete"/>									
									<asp:Button ID="BtnDetail" runat="server" class="bitbtn btndetail" Text="Detail" CommandName="Detail" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>"/>										
								</ItemTemplate>
								<EditItemTemplate>
									<asp:Button ID="btnUpdate" runat="server" class="bitbtn btnsave" Text="Save" CommandName="Update"/>									
                                    <asp:Button ID="btnCancel" runat="server" class="bitbtn btncancel" Text="Cancel" CommandName="Cancel"/>									
								</EditItemTemplate>								
								<FooterTemplate>
									<asp:Button class="bitbtn btnadd" runat="server" ID="btnAdd" Text="Add" CommandName="Insert"/>									
								</FooterTemplate>

<ItemStyle HorizontalAlign="Left"></ItemStyle>
							</asp:TemplateField>
							
    					</Columns>
        </asp:GridView>                     
     </asp:Panel>     
	    
     <asp:Panel ID="pnlDt" runat="server" Visible = "False">  
     <table>
     <tr>
     <td Width="65%">
     <asp:Label ID="label1" CssClass="H1" runat="server" Text="Recruitment : " />   
     <asp:Label ID="lbRecruitmentCode" ForeColor="Blue" CssClass="H1" Font-Bold="true" runat="server" />
     <asp:Label ID="Label2" Text = " - " ForeColor="Blue" CssClass="H1" Font-Bold="true" runat="server" />
     <asp:Label ID="lbRecruitmentName" ForeColor="Blue" CssClass="H1" Font-Bold="true" runat="server" />
     </td>
     </tr>
     </table>     
     
	 <br />
	 <asp:Button class="bitbtn btnback" runat="server" ID="btnBackDtTop" Text="Back" />
	 <br />
	 
	    <div style="border:0px  solid; width:100%; height:100%; overflow:auto;">
	    
            <asp:GridView id="DataGridDt" runat="server" ShowFooter="True" AllowSorting="True" 
            AutoGenerateColumns="False" AllowPaging="True">
						<HeaderStyle CssClass="GridHeader" wrap="False"></HeaderStyle>
						<RowStyle CssClass="GridItem" wrap="false"/>
						<AlternatingRowStyle CssClass="GridAltItem"/>
						<FooterStyle CssClass="GridFooter" Wrap="false" />
						<PagerStyle CssClass="GridPager" />
				      <Columns>
				            <asp:TemplateField HeaderText="Grade" SortExpression="Grade" HeaderStyle-Width="50px">
								<Itemtemplate>
									<asp:Label Runat="server" ID="Grade" text='<%# DataBinder.Eval(Container.DataItem, "Grade") %>'>
									</asp:Label>
								</Itemtemplate>		
								<EditItemTemplate>
								    <asp:Label Runat="server" ID="GradeEdit" CssClass="TextBox" Width="100%" Text='<%# DataBinder.Eval(Container.DataItem, "Grade") %>'></asp:Label>	
								</EditItemTemplate>	
								<FooterTemplate>
								   <asp:DropDownList ID="GradeAdd" CssClass="DropDownList" Width="100%" runat="server">
									  <asp:ListItem Selected="True">A</asp:ListItem>
									  <asp:ListItem>B</asp:ListItem>                                        
									  <asp:ListItem>C</asp:ListItem>
									  <asp:ListItem>D</asp:ListItem>                                        
									</asp:DropDownList>		
								</FooterTemplate>
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Range Value" HeaderStyle-Width="80px" SortExpression="RangeValue">
								<Itemtemplate>
									<asp:Label Runat="server" Width="95%" ID="RangeValue" text='<%# DataBinder.Eval(Container.DataItem, "RangeValue") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:TextBox Runat="server" ID="RangeValueEdit" CssClass="TextBox" Width="95%" MaxLength="50" Text='<%# DataBinder.Eval(Container.DataItem, "RangeValue") %>'>
									</asp:TextBox>									
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="RangeValueAdd" CssClass="TextBox" Width="95%" MaxLength="50" Runat="Server"/>
								    <cc1:TextBoxWatermarkExtender ID="RangeValueAdd_TextBoxWatermarkExtender" 
                                        runat="server" Enabled="True" TargetControlID="RangeValueAdd" WatermarkText="can't blank" WatermarkCssClass="Watermarked" >
                                    </cc1:TextBoxWatermarkExtender>
								</FooterTemplate>
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Lulus" SortExpression="FgLulus" HeaderStyle-Width="50px">
								<Itemtemplate>
									<asp:Label Runat="server" ID="FgLulus" text='<%# DataBinder.Eval(Container.DataItem, "FgLulus") %>'>
									</asp:Label>
								</Itemtemplate>		
								<EditItemTemplate>
								    <asp:DropDownList ID="FgLulusEdit" CssClass="DropDownList" Width="100%" runat="server" SelectedValue='<%# DataBinder.Eval(Container.DataItem, "FgLulus") %>'>
									  <asp:ListItem>Y</asp:ListItem>
									  <asp:ListItem>N</asp:ListItem>                                        
								    </asp:DropDownList>		
								</EditItemTemplate>	
								<FooterTemplate>
								   <asp:DropDownList ID="FgLulusAdd" CssClass="DropDownList" Width="100%" runat="server">
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
	    <asp:Button class="bitbtn btnback" runat="server" ID="btnBack2" Text="Back" />
	 </asp:Panel>   
        
     <asp:Label ID="lstatus" ForeColor="red" runat="server"></asp:Label>
    </div>
    </form>
</body>
</html>
