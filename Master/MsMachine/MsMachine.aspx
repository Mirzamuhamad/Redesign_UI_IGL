<%@ Page Language="VB" AutoEventWireup="false" CodeFile="MsMachine.aspx.vb" Inherits="MsMachine_MsMachine" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Machine File</title>
    <script src="../../Function/OpenDlg.JS" type="text/javascript"></script>            
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
 <link href="https://fonts.googleapis.com/css2?family=Muli:wght@300;400;600&display=swap" rel="stylesheet">
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div class="Content">
     <div class="H1">Machine File</div>
     <hr style="color:Blue" />
           <table>
        <tr>
            <td style="width:100px;text-align:right" >Quick Search :</td>
            <td><asp:TextBox runat="server" ID ="tbFilter" CssClass="TextBox" /> 
                <asp:DropDownList runat="server" ID="ddlField" CssClass="DropDownList" >
                  <asp:ListItem Selected="true" Text="Machine Code" Value="Machine_Code"></asp:ListItem>
                  <asp:ListItem Text="Machine Name" Value = "Machine_Name"></asp:ListItem>        
                  <asp:ListItem Text="Machine Group Code" Value = "MachineGroup_Code"></asp:ListItem>        
                  <asp:ListItem Text="Machine Group Name" Value = "MachineGroup_Name"></asp:ListItem>        
                  <asp:ListItem Text="Specification" Value = "Specification"></asp:ListItem>        
                  <asp:ListItem Text="Unit" Value = "Unit"></asp:ListItem>        
                  <asp:ListItem Text="LoadFactor" Value = "LoadFactor"></asp:ListItem>                          
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
                  <asp:ListItem Selected="true" Text="Machine Code" Value="Machine_Code"></asp:ListItem>
                  <asp:ListItem Text="Machine Name" Value = "Machine_Name"></asp:ListItem>        
                  <asp:ListItem Text="Machine Group Code" Value = "MachineGroup_Code"></asp:ListItem>        
                  <asp:ListItem Text="Machine Group Name" Value = "MachineGroup_Name"></asp:ListItem>        
                  <asp:ListItem Text="Specification" Value = "Specification"></asp:ListItem>        
                  <asp:ListItem Text="Unit" Value = "Unit"></asp:ListItem>        
                  <asp:ListItem Text="LoadFactor" Value = "LoadFactor"></asp:ListItem>                          
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
							<asp:TemplateField HeaderText="Machine Code" HeaderStyle-Width="120" SortExpression="Machine_Code">
								<Itemtemplate>
									<asp:Label Runat="server" ID="MachineCode" text='<%# DataBinder.Eval(Container.DataItem, "Machine_Code") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:Label Runat="server" ID="MachineCodeEdit"  Text='<%# DataBinder.Eval(Container.DataItem, "Machine_Code") %>'>
									</asp:Label>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="MachineCodeAdd" CssClass="TextBox" MaxLength="5" Runat="Server" Width="100%"/>
									<cc1:TextBoxWatermarkExtender ID="MachineCodeAdd_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="MachineCodeAdd" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>
								</FooterTemplate>
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Machine Name" HeaderStyle-Width="800" SortExpression="Machine_Name">
								<Itemtemplate>
									<asp:Label Runat="server" ID="MachineName" text='<%# DataBinder.Eval(Container.DataItem, "Machine_Name") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:TextBox Runat="server" CssClass="TextBox" ID="MachineNameEdit" MaxLength="60" Width="100%" Text='<%# DataBinder.Eval(Container.DataItem, "Machine_Name") %>'>
									</asp:TextBox>
									<cc1:TextBoxWatermarkExtender ID="MachineNameEdit_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="MachineNameEdit" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="MachineNameAdd" CssClass="TextBox" Runat="Server" MaxLength="60" Width="100%"/>
									<cc1:TextBoxWatermarkExtender ID="MachineNameAdd_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="MachineNameAdd" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>
								</FooterTemplate>
							</asp:TemplateField>		
							
							<asp:TemplateField HeaderText="Machine Group" HeaderStyle-Width="550" 
                                SortExpression="MachineGroup_Code">
								<Itemtemplate>
									<asp:Label Runat="server" ID="MachineGroupCode" text='<%# DataBinder.Eval(Container.DataItem, "MachineGroup_Code") %>'>
									</asp:Label>
								</Itemtemplate>	
								<EditItemTemplate>
									<asp:TextBox Runat="server" ID="MachineGroupEdit" MaxLength="5" Width="65%" CssClass="TextBox" 
									Text='<%# DataBinder.Eval(Container.DataItem, "MachineGroup_Code") %>'
									ontextchanged="MachineGroupAdd_TextChanged" AutoPostBack="true"/>
									<cc1:TextBoxWatermarkExtender ID="MachineGroupEdit_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="MachineGroupEdit" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                    </cc1:TextBoxWatermarkExtender>		
                                    <asp:Button class="bitbtn btngo" runat="server" ID="btnMachineGroupEdit" CommandName ="btnMachineGroupEdit" Text="..." />							
									
								</EditItemTemplate>
								<FooterTemplate>								
									<asp:TextBox ID="MachineGroupAdd" CssClass="TextBox" MaxLength="5" Width="65%" 
									ontextchanged="MachineGroupAdd_TextChanged" AutoPostBack="true" Runat="Server" />
									<cc1:TextBoxWatermarkExtender ID="MachineGroupAdd_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="MachineGroupAdd" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                    </cc1:TextBoxWatermarkExtender>			
                                    <asp:Button class="bitbtn btngo" runat="server" ID="btnMachineGroupAdd" CommandName ="btnMachineGroupAdd" Text="..."  />																						
								</FooterTemplate>															
							</asp:TemplateField>							  			
							
							<asp:TemplateField HeaderText="Machine Group Name" HeaderStyle-Width="800" SortExpression="MachineGroup_Name">
								<Itemtemplate>
									<asp:Label Runat="server" ID="MachineGroupName" text='<%# DataBinder.Eval(Container.DataItem, "MachineGroup_Name") %>'>
									</asp:Label>
								</Itemtemplate>	
								<EditItemTemplate>
									<asp:Label Runat="server" ID="MachineGroupNameEdit" Text='<%# DataBinder.Eval(Container.DataItem, "MachineGroup_Name") %>'>
									</asp:Label>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:Label Runat="server" ID="MachineGroupNameAdd">
									</asp:Label>
								</FooterTemplate>							
							</asp:TemplateField>
							
        					<asp:TemplateField HeaderText="Specification" HeaderStyle-Width="800" SortExpression="Specification">
								<Itemtemplate>
									<asp:Label Runat="server" ID="Specification" text='<%# DataBinder.Eval(Container.DataItem, "Specification") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:TextBox Runat="server" CssClass="TextBox" ID="SpecificationEdit" MaxLength="60" Width="100%" Text='<%# DataBinder.Eval(Container.DataItem, "Specification") %>'>
									</asp:TextBox>
									<cc1:TextBoxWatermarkExtender ID="SpecificationEdit_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="SpecificationEdit" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="SpecificationAdd" CssClass="TextBox" Runat="Server" MaxLength="60" Width="100%"/>
									<cc1:TextBoxWatermarkExtender ID="SpecificationAdd_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="SpecificationAdd" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>
								</FooterTemplate>
							</asp:TemplateField>	
							
						     <asp:TemplateField HeaderText="Unit" HeaderStyle-Width="500" SortExpression="Unit">
								<Itemtemplate>
									<asp:Label Runat="server" ID="Unit" text='<%# DataBinder.Eval(Container.DataItem, "Unit_Name") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>														
            						<asp:DropDownList Runat="server" ID="UnitEdit" Width="100" CssClass="DropDownList"
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Unit") %>'
                                        DataSourceID="dsUnit" DataTextField="Unit_Name" 
                                        DataValueField="Unit_Code">
									</asp:DropDownList>													
								</EditItemTemplate>
								<FooterTemplate>
									<asp:DropDownList ID="UnitAdd" Runat="Server" Width="100" CssClass="DropDownList"
                                        DataSourceID="dsUnit" DataTextField="Unit_Name" 
                                        DataValueField="Unit_Code">
									</asp:DropDownList>																											
								</FooterTemplate>
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Load Factor" HeaderStyle-Width="50" SortExpression="LoadFactor">
								<Itemtemplate>
									<asp:Label Runat="server" ID="LoadFactor" text='<%# DataBinder.Eval(Container.DataItem, "LoadFactor") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:Label Runat="server" ID="LoadFactorEdit" text='<%# DataBinder.Eval(Container.DataItem, "LoadFactor") %>'>
									</asp:Label>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:Label Runat="server" ID="LoadFactorAdd" text="0">
									</asp:Label>
								</FooterTemplate>
							</asp:TemplateField>		

							
							<asp:TemplateField HeaderText="Action" headerstyle-width="126" >
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
							</asp:TemplateField>
							
    					</Columns>
        </asp:GridView>
        
        <asp:SqlDataSource ID="dsUnit" runat="server"                                                                                 
               SelectCommand="EXEC S_GetUnit">                                        
        </asp:SqlDataSource>

        
     <asp:Label ID="lstatus" ForeColor="red" runat="server"></asp:Label>
    </div>
    </form>
</body>
</html>
