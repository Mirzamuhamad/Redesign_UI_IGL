<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TrSetupConfiguration.aspx.vb" Inherits="Transaction_TrSetupConfiguration_TrSetupConfiguration" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Religion File</title>
    <script src="../../Function/Function.JS" type="text/javascript"></script>
    <script src="../../Function/OpenDlg.js" type="text/javascript"></script>
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
 <link href="https://fonts.googleapis.com/css2?family=Muli:wght@300;400;600&display=swap" rel="stylesheet">
    
    <script type="text/javascript">
    
//        $(document).ready(function(){
//        
//            $('#ValueEdit').keyup(function () {
//                if (this.value != this.value.replace(/[^0-9\.]/g, '')) {
//                   this.value = this.value.replace(/[^0-9\.]/g, '');
//                }
//            });
//        
//        });
        
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div class="Content">
    <div class="H1">Setup Configuration</div>
    <hr style="color:Blue" />
    
    <table>
        <tr>
            <td style="width:100px;text-align:right" >Group :</td>
            <td> 
                <asp:DropDownList runat="server" ID="ddlGroup" CssClass="DropDownList" AutoPostBack="true" >
                  <%--<asp:ListItem Selected="true" Text="Religion Code" Value="ReligionCode"></asp:ListItem>--%>
                  <%--<asp:ListItem Text="Religion Name" Value="ReligionName"></asp:ListItem>--%>        
                </asp:DropDownList>                    
                <%--<asp:Button ID="btnShowPopup" runat="server" Style="display: none" />--%>                        
            </td>
        </tr>
     </table>
     
     <br />
     <asp:UpdatePanel ID="updPnl" runat="server" UpdateMode="Always">
     <ContentTemplate>
     
     <asp:GridView id="DataGrid" runat="server"  
            ShowFooter="True" AllowSorting="True" 
            AutoGenerateColumns="False" AllowPaging="True" CssClass="Grid">
						<HeaderStyle CssClass="GridHeader"></HeaderStyle>
						<RowStyle CssClass="GridItem" Wrap="false"/>
						<AlternatingRowStyle CssClass="GridAltItem"/>
						<FooterStyle CssClass="GridFooter" />
						<PagerStyle CssClass="GridPager" HorizontalAlign="Center" />					  
				<EmptyDataTemplate>
				    
				</EmptyDataTemplate>	  
				      <Columns>				      
							<asp:TemplateField HeaderText="Code" HeaderStyle-Width="100" Visible="false" >
								<Itemtemplate>
									<asp:Label Runat="server" ID="Code" text='<%# DataBinder.Eval(Container.DataItem, "SetCode") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:Label Runat="server" ID="CodeEdit" Text='<%# DataBinder.Eval(Container.DataItem, "SetCode") %>'>
									</asp:Label>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="CodeAdd" CssClass="TextBox" MaxLength="5" Runat="Server" Width="100%"/>
									<cc1:TextBoxWatermarkExtender ID="CodeAdd_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="CodeAdd" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>
								</FooterTemplate>								
							</asp:TemplateField>							
							
							<asp:TemplateField HeaderText="Description" HeaderStyle-Width="300">
								<Itemtemplate>
									<asp:Label Runat="server" ID="Description" text='<%# DataBinder.Eval(Container.DataItem, "setdescription") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:TextBox Runat="server" ID="DescriptionEdit" MaxLength="60" Width="100%" CssClass="TextBox" Text='<%# DataBinder.Eval(Container.DataItem, "setdescription") %>'>
									</asp:TextBox>
									<cc1:TextBoxWatermarkExtender ID="DescriptionEdit_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="DescriptionEdit" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>									
								</EditItemTemplate>
								<%--<FooterTemplate>
									<asp:TextBox ID="DescriptionAdd" CssClass="TextBox" MaxLength="60" Runat="Server" Width="100%"/>
									<cc1:TextBoxWatermarkExtender ID="DescriptionAdd_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="DescriptionAdd" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>
								</FooterTemplate>--%>
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Value" HeaderStyle-Width="150">
								<Itemtemplate>
									<asp:Label Runat="server" ID="Value" text='<%# DataBinder.Eval(Container.DataItem, "setvalue") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
								    <asp:DropDownList CssClass="DropDownList" Width="100%" ID="ddlValueEdit" runat="server"
								        
								        DataSourceID="dsAbsStatus" DataTextField="AbsStatusName" 
                                        DataValueField="AbsStatusCode">
								    </asp:DropDownList>
								    
								    <%--SelectedValue='<%# DataBinder.Eval(Container.DataItem, "setvalue") %>'--%>
									
									<asp:TextBox Runat="server" ID="ValueEdit" MaxLength="60" Width="100%" CssClass="TextBox" AutoPostBack="true" Text='<%# DataBinder.Eval(Container.DataItem, "setvalue") %>'>
									</asp:TextBox>
									
									<%--OnTextChanged="TextBox1_TextChanged"--%>
									
									<cc1:TextBoxWatermarkExtender ID="ValueEdit_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="ValueEdit" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>									
								</EditItemTemplate>
								<%--<FooterTemplate>
									<asp:TextBox ID="ValueAdd" CssClass="TextBox" MaxLength="60" Runat="Server" Width="100%"/>
									<cc1:TextBoxWatermarkExtender ID="ValueAdd_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="ValueAdd" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>
								</FooterTemplate>--%>
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="remark" HeaderStyle-Width="150" Visible="false" >
								<Itemtemplate>
									<asp:Label Runat="server" ID="remark" text='<%# DataBinder.Eval(Container.DataItem, "setremark") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:TextBox Runat="server" ID="remarkEdit" MaxLength="60" Width="100%" CssClass="TextBox" Text='<%# DataBinder.Eval(Container.DataItem, "setremark") %>'>
									</asp:TextBox>
									<cc1:TextBoxWatermarkExtender ID="remarkEdit_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="remarkEdit" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>									
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="remarkAdd" CssClass="TextBox" MaxLength="60" Runat="Server" Width="100%"/>
									<cc1:TextBoxWatermarkExtender ID="remarkAdd_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="remarkAdd" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>
								</FooterTemplate>
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="table" HeaderStyle-Width="150" Visible="false" >
								<Itemtemplate>
									<asp:Label Runat="server" ID="table" text='<%# DataBinder.Eval(Container.DataItem, "settable") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:TextBox Runat="server" ID="tableEdit" MaxLength="60" Width="100%" CssClass="TextBox" Text='<%# DataBinder.Eval(Container.DataItem, "settable") %>'>
									</asp:TextBox>
									<cc1:TextBoxWatermarkExtender ID="tableEdit_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="tableEdit" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>									
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="tableAdd" CssClass="TextBox" MaxLength="60" Runat="Server" Width="100%"/>
									<cc1:TextBoxWatermarkExtender ID="tableAdd_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="tableAdd" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>
								</FooterTemplate>
							</asp:TemplateField>
							
							<%--<asp:BoundField HeaderText="remark" DataField="setremark" Visible="true" />--%>
							<%--<asp:BoundField HeaderText="table" DataField="settable" Visible="true" />--%>
							
							<asp:TemplateField HeaderText="Action" headerstyle-width="126" >
								<ItemTemplate>
								    <asp:Button class="bitbtn btnedit" runat="server" ID="btnEdit" Text="Edit" CommandName="Edit" />																						 											
									
									<asp:Button class="bitbtn btndelete" runat="server" ID="btnDelete" Text="Delete" CommandName="Delete"  OnClientClick="return confirm('Sure to delete this data?');" />																						 																		
								
								</ItemTemplate>
								<EditItemTemplate>
									<asp:Button class="bitbtn btnsave" runat="server" ID="btnUpdate" Text="Save" CommandName="Update" />																						 																		
									
									
									<asp:Button class="bitbtn btncancel" runat="server" ID="btnCancel" Text="Cancel" CommandName="Cancel" />																						 																		
									
								</EditItemTemplate>
								<FooterTemplate>
								   <%--<asp:Button class="bitbtn btnadd" runat="server" ID="btnAdd" Text="Add" CommandName="Insert" />--%>																						 																		
									
								</FooterTemplate>
							</asp:TemplateField>
							
    					</Columns>
        </asp:GridView>
        
        </ContentTemplate>
        </asp:UpdatePanel>
        
        <asp:SqlDataSource ID="dsAbsStatus" runat="server"                 
                SelectCommand="select AbsStatusCode,AbsStatusName from VMsAbsStatus">
        </asp:SqlDataSource>
        <asp:Label ID="lstatus" ForeColor="red" runat="server"></asp:Label>
    </div>
    </form>
</body>
</html>
