<%@ Page Language="VB" AutoEventWireup="false" CodeFile="MsMachineLoadFactor.aspx.vb" Inherits="MsMachineLoadFactor_MsMachineLoadFactor" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Machine Load Factor FOH File</title>
    <script src="../../Function/Function.JS" type="text/javascript"></script>
    <script src="../../Function/OpenDlg.JS" type="text/javascript"></script>            
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
 <link href="https://fonts.googleapis.com/css2?family=Muli:wght@300;400;600&display=swap" rel="stylesheet">
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div class="Content">
     <div class="H1">Machine Load Factor FOH File</div>
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
                &nbsp &nbsp &nbsp &nbsp
                <asp:Label ID="lb1" runat="server" Text = "Setting Load Factor in selected row : "> </asp:Label>
                <asp:TextBox ID="tbLoadFactor" runat="server" CssClass="TextBox" MaxLength="15" Width="50px" /> 
                <asp:Button class="bitbtn btngo" runat="server" ID="btnApply" Text="G"/>
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
				            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:CheckBox ID="cbSelectHd" runat="server" AutoPostBack="true" 
                                        oncheckedchanged="cbSelectHd_CheckedChanged1" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox ID="cbSelect" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>  
				      
							<asp:TemplateField HeaderText="Machine Code" HeaderStyle-Width="120" SortExpression="Machine_Code">
								<Itemtemplate>
									<asp:Label Runat="server" ID="MachineCode" text='<%# DataBinder.Eval(Container.DataItem, "Machine_Code") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:Label Runat="server" ID="MachineCodeEdit"  Text='<%# DataBinder.Eval(Container.DataItem, "Machine_Code") %>'>
									</asp:Label>
								</EditItemTemplate>
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Machine Name" HeaderStyle-Width="800" SortExpression="Machine_Name">
								<Itemtemplate>
									<asp:Label Runat="server" ID="MachineName" text='<%# DataBinder.Eval(Container.DataItem, "Machine_Name") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:Label Runat="server" ID="MachineNameEdit" text='<%# DataBinder.Eval(Container.DataItem, "Machine_Name") %>'>
									</asp:Label>
								</EditItemTemplate>
							</asp:TemplateField>		
							
							<asp:TemplateField HeaderText="Machine Group" HeaderStyle-Width="550" 
                                SortExpression="MachineGroup_Code">
								<Itemtemplate>
									<asp:Label Runat="server" ID="MachineGroupCode" text='<%# DataBinder.Eval(Container.DataItem, "MachineGroup_Code") %>'>
									</asp:Label>
								</Itemtemplate>	
								<EditItemTemplate>
									<asp:Label Runat="server" ID="MachineGroupCodeEdit" text='<%# DataBinder.Eval(Container.DataItem, "MachineGroup_Code") %>'>
									</asp:Label>								
								</EditItemTemplate>								
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
							</asp:TemplateField>
							
        					<asp:TemplateField HeaderText="Specification" HeaderStyle-Width="800" SortExpression="Specification">
								<Itemtemplate>
									<asp:Label Runat="server" ID="Specification" text='<%# DataBinder.Eval(Container.DataItem, "Specification") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
    								<asp:Label Runat="server" ID="SpecificationEdit" text='<%# DataBinder.Eval(Container.DataItem, "Specification") %>'>
									</asp:Label>
								</EditItemTemplate>
							</asp:TemplateField>	
							
						     <asp:TemplateField HeaderText="Unit" HeaderStyle-Width="500" SortExpression="Unit">
								<Itemtemplate>
									<asp:Label Runat="server" ID="Unit" text='<%# DataBinder.Eval(Container.DataItem, "Unit_Name") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>														
									<asp:Label Runat="server" ID="UnitEdit" text='<%# DataBinder.Eval(Container.DataItem, "Unit_Name") %>'>
									</asp:Label>								
								</EditItemTemplate>
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Load Factor" HeaderStyle-Width="300" SortExpression="LoadFactor">
								<Itemtemplate>
									<asp:Label Runat="server" ID="LoadFactor" text='<%# DataBinder.Eval(Container.DataItem, "LoadFactor") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:TextBox Runat="server" CssClass="TextBox" ID="LoadFactorEdit" MaxLength="9" Width="80" Text='<%# DataBinder.Eval(Container.DataItem, "LoadFactor") %>'>
									</asp:TextBox>									
								</EditItemTemplate>
							</asp:TemplateField>		

							
							<asp:TemplateField HeaderText="Action" headerstyle-width="126" >
								<ItemTemplate>
								    <asp:Button ID="btnEdit" runat="server" class="bitbtndt btnedit" Text="Edit" CommandName="Edit"/>									
								</ItemTemplate>
								<EditItemTemplate>
									<asp:Button ID="btnUpdate" runat="server" class="bitbtndt btnsave" Text="Save" CommandName="Update"/>									
                                    <asp:Button ID="btnCancel" runat="server" class="bitbtndt btncancel" Text="Cancel" CommandName="Cancel"/>									
								</EditItemTemplate>
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
