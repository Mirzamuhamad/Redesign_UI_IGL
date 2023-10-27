<%@ Page Language="VB" AutoEventWireup="false" CodeFile="MsMachineGroup.aspx.vb" Inherits="MsMachineGroup_MsMachineGroup" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Medical Group File</title>
    <script src="../../Function/Function.JS" type="text/javascript"></script>
    <script src="../../Function/OpenDlg.JS" type="text/javascript"></script>            
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div class="Content">
     <div class="H1">Machine Group File</div>
     <hr style="color:Blue" />
           <table>
        <tr>
            <td style="width:100px;text-align:right" >Quick Search :</td>
            <td><asp:TextBox runat="server" ID ="tbFilter" CssClass="TextBox" /> 
                <asp:DropDownList runat="server" ID="ddlField" CssClass="DropDownList" >
                  <asp:ListItem Selected="true" Text="Machine Group Code" Value="MachineGrpCode"></asp:ListItem>
                  <asp:ListItem Text="Machine Group Name" Value = "MachineGrpName"></asp:ListItem>        
                  <asp:ListItem Text="Work Center" Value = "WorkCtr_Code"></asp:ListItem>        
                  <asp:ListItem Text="Work Center Name" Value = "WorkCtr_Name"></asp:ListItem>        
                  <asp:ListItem Text="Qty Machine" Value = "QtyMachine"></asp:ListItem>        
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
                    <asp:ListItem Selected="true" Text="Machine Group Code" Value="MachineGrpCode"></asp:ListItem>
                    <asp:ListItem Text="Machine Group Name" Value = "MachineGrpName"></asp:ListItem>        
                    <asp:ListItem Text="Work Center" Value = "WorkCtr_Code"></asp:ListItem>        
                    <asp:ListItem Text="Work Center Name" Value = "WorkCtr_Name"></asp:ListItem>        
                    <asp:ListItem Text="Qty Machine" Value = "QtyMachine"></asp:ListItem>        
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
							<asp:TemplateField HeaderText="Machine Group Code" HeaderStyle-Width="120" SortExpression="MachineGrpCode">
								<Itemtemplate>
									<asp:Label Runat="server" ID="MachineGrpCode" text='<%# DataBinder.Eval(Container.DataItem, "MachineGrpCode") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:Label Runat="server" ID="MachineGrpCodeEdit"  Text='<%# DataBinder.Eval(Container.DataItem, "MachineGrpCode") %>'>
									</asp:Label>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="MachineGrpCodeAdd" CssClass="TextBox" MaxLength="5" Runat="Server" Width="100%"/>
									<cc1:TextBoxWatermarkExtender ID="MachineGrpCodeAdd_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="MachineGrpCodeAdd" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>
								</FooterTemplate>
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Machine Group Name" HeaderStyle-Width="300" SortExpression="MachineGrpName">
								<Itemtemplate>
									<asp:Label Runat="server" ID="MachineGrpName" text='<%# DataBinder.Eval(Container.DataItem, "MachineGrpName") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:TextBox Runat="server" CssClass="TextBox" ID="MachineGrpNameEdit" MaxLength="60" Width="100%" Text='<%# DataBinder.Eval(Container.DataItem, "MachineGrpName") %>'>
									</asp:TextBox>
									<cc1:TextBoxWatermarkExtender ID="MachineGrpNameEdit_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="MachineGrpNameEdit" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="MachineGrpNameAdd" CssClass="TextBox" Runat="Server" MaxLength="60" Width="100%"/>
									<cc1:TextBoxWatermarkExtender ID="MachineGrpNameAdd_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="MachineGrpNameAdd" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>
								</FooterTemplate>
							</asp:TemplateField>		
							
							<asp:TemplateField HeaderText="Work Center" HeaderStyle-Width="140" 
                                SortExpression="WorkCtr_Code">
								<Itemtemplate>
									<asp:Label Runat="server" ID="WorkCtr" text='<%# DataBinder.Eval(Container.DataItem, "WorkCtr_Code") %>'>
									</asp:Label>
								</Itemtemplate>	
								<EditItemTemplate>
									<asp:TextBox Runat="server" ID="WorkCtrEdit" MaxLength="5" Width="80%" CssClass="TextBox" 
									Text='<%# DataBinder.Eval(Container.DataItem, "WorkCtr_Code") %>'
									ontextchanged="WorkCtrAdd_TextChanged" AutoPostBack="true"/>
									<cc1:TextBoxWatermarkExtender ID="WorkCtrEdit_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="WorkCtrEdit" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                    </cc1:TextBoxWatermarkExtender>		
                                    <asp:Button class="btngo" runat="server" ID="btnWorkCtrEdit" CommandName ="btnWorkCtrEdit" Text="..." />
								</EditItemTemplate>
								<FooterTemplate>								
									<asp:TextBox ID="WorkCtrAdd" CssClass="TextBox" MaxLength="5" Width="80%" 
									ontextchanged="WorkCtrAdd_TextChanged" AutoPostBack="true" Runat="Server" />
									<cc1:TextBoxWatermarkExtender ID="WorkCtrAdd_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="WorkCtrAdd" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                    </cc1:TextBoxWatermarkExtender>			
                                    <asp:Button class="btngo" runat="server" ID="btnWorkCtrAdd" CommandName ="btnWorkCtrAdd" Text="..."  />																						
								</FooterTemplate>															
							</asp:TemplateField>							  			
							
							<asp:TemplateField HeaderText="Work Center Name" HeaderStyle-Width="300" SortExpression="WorkCtr_Name">
								<Itemtemplate>
									<asp:Label Runat="server" ID="WorkCtrName" text='<%# DataBinder.Eval(Container.DataItem, "WorkCtr_Name") %>'>
									</asp:Label>
								</Itemtemplate>	
								<EditItemTemplate>
									<asp:Label Runat="server" ID="WorkCtrNameEdit" Text='<%# DataBinder.Eval(Container.DataItem, "WorkCtr_Name") %>'>
									</asp:Label>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:Label Runat="server" ID="WorkCtrNameAdd">
									</asp:Label>
								</FooterTemplate>							
							</asp:TemplateField>

							
							<asp:TemplateField HeaderText="Qty Machine" HeaderStyle-Width="50" SortExpression="QtyMachine">
								<Itemtemplate>
									<asp:Label Runat="server" ID="QtyMachine" text='<%# DataBinder.Eval(Container.DataItem, "QtyMachine") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:TextBox Runat="server" CssClass="TextBox" ID="QtyMachineEdit" MaxLength="4" Width="100%" Text='<%# DataBinder.Eval(Container.DataItem, "QtyMachine") %>'>
									</asp:TextBox>
									<cc1:TextBoxWatermarkExtender ID="QtyMachineEdit_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="QtyMachineEdit" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="QtyMachineAdd" CssClass="TextBox" Runat="Server" MaxLength="4" Width="100%"/>
									<cc1:TextBoxWatermarkExtender ID="QtyMachineAdd_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="QtyMachineAdd" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>
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
        
     <asp:Label ID="lstatus" ForeColor="red" runat="server"></asp:Label>
    </div>
    </form>
</body>
</html>
