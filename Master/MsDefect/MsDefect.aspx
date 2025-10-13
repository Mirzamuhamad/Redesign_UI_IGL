<%@ Page Language="VB" AutoEventWireup="false" CodeFile="MsDefect.aspx.vb" Inherits="Master_MsDefect_MsDefect" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Defect File</title>
    <script src="../../Function/OpenDlg.JS" type="text/javascript"></script> 
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
 <link href="https://fonts.googleapis.com/css2?family=Muli:wght@300;400;600&display=swap" rel="stylesheet">
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div class="Content">
     <div class="H1">Defect File</div>
     <hr style="color:Blue" />
      <table>
        <tr>
            <td style="width:100px;text-align:right" >Quick Search :</td>
            <td><asp:TextBox runat="server" ID ="tbFilter" CssClass="TextBox"/> 
                <asp:DropDownList runat="server" ID="ddlField" CssClass="DropDownList" >
                  <asp:ListItem Selected="true" Text="Defect Code" Value="Defect_Code"></asp:ListItem>
                  <asp:ListItem Text="Defect Name" Value="Defect_Name"></asp:ListItem>        
                    <asp:ListItem Value="Standard">Standard</asp:ListItem>
                    <asp:ListItem Value="Defect_Group_Name">Defect Group</asp:ListItem>
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
                    <asp:ListItem Selected="true" Text="Defect Code" Value="Defect_Code"></asp:ListItem>
                  <asp:ListItem Text="Defect Name" Value="Defect_Name"></asp:ListItem>        
                    <asp:ListItem Value="Standard">Standard</asp:ListItem>
                    <asp:ListItem Value="Defect_Group_Name">Defect Group</asp:ListItem>
                  </asp:DropDownList>
                
            </td>
        </tr>
     </table>
     </asp:Panel>
      <br />
      <%--<div style="border:0px  solid; width:100%; height:100%; overflow:auto;">--%>
      <asp:GridView id="DataGrid" runat="server" ShowFooter="True" AllowSorting="True" 
            AutoGenerateColumns="False" AllowPaging="True" CssClass="Grid">
						<HeaderStyle CssClass="GridHeader"></HeaderStyle>
						<RowStyle CssClass="GridItem" wrap="false" />
						<AlternatingRowStyle CssClass="GridAltItem"/>
						<FooterStyle CssClass="GridFooter" />
						<PagerStyle CssClass="GridPager" HorizontalAlign="Center" />
				      <Columns>
							<asp:TemplateField HeaderText="Defect Code" HeaderStyle-Width="110" SortExpression="Defect_Code">
								<Itemtemplate>
									<asp:Label Runat="server" ID="DefectCode" text='<%# DataBinder.Eval(Container.DataItem, "Defect_Code") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:Label Runat="server" ID="DefectCodeEdit" Text='<%# DataBinder.Eval(Container.DataItem, "Defect_Code") %>'>
									</asp:Label>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="DefectCodeAdd" CssClass="TextBox" MaxLength="10" Runat="Server" Width="100%"/>
									<cc1:TextBoxWatermarkExtender ID="DefectCodeAdd_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="DefectCodeAdd" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>
								</FooterTemplate>
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Defect Name" HeaderStyle-Width="320" SortExpression="Defect_Name">
								<Itemtemplate>
									<asp:Label Runat="server" ID="DefectName" text='<%# DataBinder.Eval(Container.DataItem, "Defect_Name") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:TextBox Runat="server" ID="DefectNameEdit" CssClass="TextBox" MaxLength="100" Width="100%" Text='<%# DataBinder.Eval(Container.DataItem, "Defect_Name") %>'>
									</asp:TextBox>
									<cc1:TextBoxWatermarkExtender ID="DefectNameEdit_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="DefectNameEdit" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="DefectNameAdd" Runat="Server" CssClass="TextBox" MaxLength="100" Width="100%"/>
									<cc1:TextBoxWatermarkExtender ID="DefectNameAdd_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="DefectNameAdd" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>
								</FooterTemplate>
							</asp:TemplateField>		
							
							<asp:TemplateField HeaderText="Standard" HeaderStyle-Width="320" SortExpression="Standard">
								<Itemtemplate>
									<asp:Label Runat="server" ID="Standard" text='<%# DataBinder.Eval(Container.DataItem, "Standard") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:TextBox Runat="server" ID="StandardEdit" CssClass="TextBox" MaxLength="100" Width="100%" Text='<%# DataBinder.Eval(Container.DataItem, "Standard") %>'>
									</asp:TextBox>
									<cc1:TextBoxWatermarkExtender ID="StandardEdit_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="StandardEdit" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="StandardAdd" Runat="Server" CssClass="TextBox" MaxLength="100" Width="100%"/>
									<cc1:TextBoxWatermarkExtender ID="StandardAdd_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="StandardAdd" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>
								</FooterTemplate>
							</asp:TemplateField>	
							
							<asp:TemplateField HeaderText="Defect Group" HeaderStyle-Width="250" SortExpression="Defect_Group_Name">
								<Itemtemplate>
									<asp:Label Runat="server" ID="DefectGroup" text='<%# DataBinder.Eval(Container.DataItem, "Defect_Group_Name") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:DropDownList Runat="server" ID="DefectGroupEdit" Width="100%" CssClass="DropDownList" 
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Defect_Group") %>' 
                                        DataSourceID="dsDefectGroup" DataTextField="Defect_Group_Name" 
                                        DataValueField="Defect_Group_Code">									    
									</asp:DropDownList>								    
								</EditItemTemplate>
								<FooterTemplate>
									<asp:DropDownList ID="DefectGroupAdd" Runat="Server" Width="100%" CssClass="DropDownList"									    
                                        DataSourceID="dsDefectGroup" DataTextField="Defect_Group_Name" 
                                        DataValueField="Defect_Group_Code">
									</asp:DropDownList>
								</FooterTemplate>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Action" headerstyle-width="170" >
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
        </asp:gridview>
        
        <asp:SqlDataSource ID="dsDefectGroup" runat="server"                 
                SelectCommand="EXEC S_GetDefectGroup">
        </asp:SqlDataSource>
     <asp:Label ID="lstatus" ForeColor="red" runat="server"></asp:Label>        
    </div>
    </form>
</body>
</html>
