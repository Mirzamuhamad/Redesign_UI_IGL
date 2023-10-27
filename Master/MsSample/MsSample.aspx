<%@ Page Language="VB" AutoEventWireup="false" CodeFile="MsSample.aspx.vb" Inherits="Master_MsSample_MsSample" %>
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
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div class="Content">
     <div class="H1">Sample File</div>
     <hr style="color:Blue" />
     <asp:Panel id="pnlHd" runat="server">
        <table>
        <tr>
            <td style="width:100px;text-align:right" >Quick Search :</td>
            <td><asp:TextBox runat="server" ID ="tbFilter" CssClass="TextBox"/> 
                <asp:DropDownList runat="server" ID="ddlField" CssClass="DropDownList" >
                  <asp:ListItem Selected="true" Text="Sample Code" Value="SampleCode"></asp:ListItem>
                  <asp:ListItem Text="Sample Name" Value="SampleName"></asp:ListItem>        
                </asp:DropDownList>     
                <asp:Button class="bitbtn btnsearch" runat="server" ID="btnSearch" Text="Search" />
                <asp:Button class="btngo" runat="server" ID="btnExpand" Text="..."/>
                <asp:Button class="bitbtn btnprint" runat="server" ID="btnPrint" Text="Print"/>
            </td>
            <td>&nbsp;&nbsp;</td>
            <td>Show Data :</td>
            <td><asp:DropDownList ID="ddlRow" runat="server" CssClass="DropDownList" 
                    AutoPostBack="True">
                <asp:ListItem Selected="True">10</asp:ListItem>
                <asp:ListItem>20</asp:ListItem>
                <asp:ListItem>30</asp:ListItem>
                <asp:ListItem>40</asp:ListItem>
                <asp:ListItem>50</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td>Row</td>
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
                    <asp:ListItem Selected="true" Text="Sample Code" Value="SampleCode"></asp:ListItem>
                    <asp:ListItem Text="Sample Name" Value="SampleName"></asp:ListItem>        
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
							<asp:TemplateField HeaderText="Sample Code" SortExpression="SampleCode">
								<Itemtemplate>
									<asp:Label Runat="server" ID="SampleCode" text='<%# DataBinder.Eval(Container.DataItem, "SampleCode") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:Label Runat="server" ID="SampleCodeEdit" Text='<%# DataBinder.Eval(Container.DataItem, "SampleCode") %>'>
									</asp:Label>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="SampleCodeAdd" CssClass="TextBox" MaxLength="20" Runat="Server" Width="100%"/>
									<cc1:TextBoxWatermarkExtender ID="SampleCodeAdd_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="SampleCodeAdd" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>
								</FooterTemplate>
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Sample Name" HeaderStyle-Width="360" SortExpression="SampleName">
								<Itemtemplate>
									<asp:Label Runat="server" ID="SampleName" text='<%# DataBinder.Eval(Container.DataItem, "SampleName") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:TextBox Runat="server" CssClass="TextBox" ID="SampleNameEdit" MaxLength="100" Width="100%" Text='<%# DataBinder.Eval(Container.DataItem, "SampleName") %>'>
									</asp:TextBox>
									<cc1:TextBoxWatermarkExtender ID="SampleNameEdit_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="SampleNameEdit" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="SampleNameAdd" CssClass="TextBox" MaxLength="100" Runat="Server" Width="100%"/>
									<cc1:TextBoxWatermarkExtender ID="SampleNameAdd_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="SampleNameAdd" 
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
									<asp:Button class="bitbtndt btnadd" runat="server" ID="btnAdd" Text="Add" CommandName="Insert"/>									
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
     <asp:Label ID="label1" CssClass="H1" runat="server" Text="Sample : " />   
     <asp:Label ID="lbSampleCode" ForeColor="Blue" CssClass="H1" Font-Bold="true" runat="server" />
     <asp:Label ID="Label2" Text = " - " ForeColor="Blue" CssClass="H1" Font-Bold="true" runat="server" />
     <asp:Label ID="lbSampleName" ForeColor="Blue" CssClass="H1" Font-Bold="true" runat="server" />
     </td>
     </tr>
     </table>     
     
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
				            <asp:TemplateField HeaderText="Criteria Code" SortExpression="CriteriaCode">
								<Itemtemplate>
									<asp:Label Runat="server" ID="CriteriaCode" text='<%# DataBinder.Eval(Container.DataItem, "CriteriaCode") %>'>
									</asp:Label>
								</Itemtemplate>		
								<EditItemTemplate>
								    <asp:Label Runat="server" ID="CriteriaCodeEdit" CssClass="TextBox" Width="98%" Text='<%# DataBinder.Eval(Container.DataItem, "CriteriaCode") %>'></asp:Label>	
								</EditItemTemplate>	
								<FooterTemplate>
								   <asp:TextBox ID="CriteriaCodeAdd" CssClass="TextBox" Width="98%" MaxLength="20" Runat="Server"/>
								    <cc1:TextBoxWatermarkExtender ID="CriteriaCodeAdd_TextBoxWatermarkExtender" 
                                        runat="server" Enabled="True" TargetControlID="CriteriaCodeAdd" WatermarkText="can't blank" WatermarkCssClass="Watermarked" >
                                    </cc1:TextBoxWatermarkExtender>
								</FooterTemplate>
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Criteria Name" HeaderStyle-Width="280px" SortExpression="CriteriaName">
								<Itemtemplate>
									<asp:Label Runat="server" Width="99%" ID="CriteriaName" text='<%# DataBinder.Eval(Container.DataItem, "CriteriaName") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:TextBox Runat="server" ID="CriteriaNameEdit" CssClass="TextBox" Width="99%" Text='<%# DataBinder.Eval(Container.DataItem, "CriteriaName") %>'>
									</asp:TextBox>									
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="CriteriaNameAdd" CssClass="TextBox" Width="99%" MaxLength="100" Runat="Server"/>
								    <cc1:TextBoxWatermarkExtender ID="CriteriaNameAdd_TextBoxWatermarkExtender" 
                                        runat="server" Enabled="True" TargetControlID="CriteriaNameAdd" WatermarkText="can't blank" WatermarkCssClass="Watermarked" >
                                    </cc1:TextBoxWatermarkExtender>
								</FooterTemplate>
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Standard" HeaderStyle-Width="280px" SortExpression="Standard">
								<Itemtemplate>
									<asp:Label Runat="server" Width="99%" ID="Standard" text='<%# DataBinder.Eval(Container.DataItem, "Standard") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:TextBox Runat="server" ID="StandardEdit" CssClass="TextBox" Width="99%" Text='<%# DataBinder.Eval(Container.DataItem, "Standard") %>'>
									</asp:TextBox>									
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="StandardAdd" CssClass="TextBox" Width="99%" MaxLength="100" Runat="Server"/>
								    <cc1:TextBoxWatermarkExtender ID="StandardAdd_TextBoxWatermarkExtender" 
                                        runat="server" Enabled="True" TargetControlID="StandardAdd" WatermarkText="can't blank" WatermarkCssClass="Watermarked" >
                                    </cc1:TextBoxWatermarkExtender>
								</FooterTemplate>
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Param Trial" HeaderStyle-Width="200" SortExpression="ParamTrialName">
								<Itemtemplate>
									<asp:Label Runat="server" ID="ParamTrial" text='<%# DataBinder.Eval(Container.DataItem, "ParamTrialName") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:DropDownList Runat="server" ID="ParamTrialEdit" Width="100%" CssClass="DropDownList" 
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "ParamTrial") %>' 
                                        DataSourceID="dsParamTrial" DataTextField="ParamTrialName" 
                                        DataValueField="ParamTrialCode">
									    
									</asp:DropDownList>								    
								</EditItemTemplate>
								<FooterTemplate>
									<asp:DropDownList ID="ParamTrialAdd" Runat="Server" Width="100%" CssClass="DropDownList"									    
                                        DataSourceID="dsParamTrial" DataTextField="ParamTrialName" 
                                        DataValueField="ParamTrialCode">
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
      <asp:SqlDataSource ID="dsParamTrial" runat="server"                 
                SelectCommand="EXEC S_GetParamTrialChoose">
        </asp:SqlDataSource>   
     <asp:Label ID="lstatus" ForeColor="red" runat="server"></asp:Label>
    </div>
    </form>
</body>
</html>
