<%@ Page Language="VB" AutoEventWireup="false" CodeFile="MsVehicle.aspx.vb" Inherits="MsVehicle_MsVehicle" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="BasicFrame.WebControls.BasicDatePicker" Namespace="BasicFrame.WebControls"
    TagPrefix="BDP" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Vehicle File</title>
    <script src="../../Function/OpenDlg.js" type="text/javascript"></script>
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
 <link href="https://fonts.googleapis.com/css2?family=Muli:wght@300;400;600&display=swap" rel="stylesheet">
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div class="Content">
     <div class="H1">Vehicle File</div>
     <hr style="color:Blue"/>
      <table>
        <tr>
            <td style="width:100px;text-align:right" >Quick Search :</td>
            <td><asp:TextBox runat="server" ID ="tbFilter" CssClass="TextBox"/> 
                <asp:DropDownList runat="server" ID="ddlField" CssClass="DropDownList" >
                  <asp:ListItem Selected="true" Text="Vehicle Code" Value="VehicleCode"></asp:ListItem>
                  <asp:ListItem Text="Vehicle Name" Value="VehicleName"></asp:ListItem>        
                  <asp:ListItem Text="Vehicle Type" Value="VehicleTypeName"></asp:ListItem>        
                  <asp:ListItem Text="Belongs To" Value="BelongsTo"></asp:ListItem>    
                  <asp:ListItem Text="Vehicle Size" Value="VehicleSize"></asp:ListItem>                                                
                  <asp:ListItem Text="Supplier" Value="SuppName"></asp:ListItem>       
                  <asp:ListItem Text="Expire Date" Value="ExpireDate"></asp:ListItem>                        
                  <asp:ListItem Text="Active" Value="FgActive"></asp:ListItem>                                       
                                     
                </asp:DropDownList>     
                <%--<asp:Button runat="server" ID="btnSearch" Text="Search" CssClass="Button" />--%>
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
                  <asp:ListItem Selected="true" Text="Vehicle Code" Value="VehicleCode"></asp:ListItem>
                  <asp:ListItem Text="Vehicle Name" Value="VehicleName"></asp:ListItem>        
                  <asp:ListItem Text="Vehicle Type" Value="VehicleTypeName"></asp:ListItem>        
                  <asp:ListItem Text="Belongs To" Value="BelongsTo"></asp:ListItem>    
                  <asp:ListItem Text="Vehicle Size" Value="VehicleSize"></asp:ListItem>                                                
                  <asp:ListItem Text="Supplier" Value="SuppName"></asp:ListItem>       
                  <asp:ListItem Text="Expire Date" Value="ExpireDate"></asp:ListItem>                        
                  <asp:ListItem Text="Active" Value="FgActive"></asp:ListItem>                   
               </asp:DropDownList>
                
            </td>
        </tr>
     </table>
     </asp:Panel>
      <br />
      <asp:GridView id="DataGrid" runat="server"  
            ShowFooter="True" AllowSorting="True" 
            AutoGenerateColumns="False" AllowPaging="True" CssClass="Grid" width = "1300px">
						<HeaderStyle CssClass="GridHeader" Wrap = "True"></HeaderStyle>
						<RowStyle CssClass="GridItem" Wrap="True"/>
						<AlternatingRowStyle CssClass="GridAltItem"/>
						<FooterStyle CssClass="GridFooter" />
						<PagerStyle CssClass="GridPager" HorizontalAlign="Center" />					  
				<EmptyDataTemplate>
				    
				</EmptyDataTemplate>	  
				      <Columns>				      
							<asp:TemplateField HeaderText="Vehicle Code"  HeaderStyle-Width="100" SortExpression="VehicleCode">
								<Itemtemplate>
									<asp:Label Runat="server" ID="VehicleCode" text='<%# DataBinder.Eval(Container.DataItem, "VehicleCode") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:Label Runat="server" ID="VehicleCodeEdit" Text='<%# DataBinder.Eval(Container.DataItem, "VehicleCode") %>'>
									</asp:Label>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="VehicleCodeAdd" CssClass="TextBox" MaxLength="12" Runat="Server" Width="100%"/>
									<cc1:TextBoxWatermarkExtender ID="VehicleCodeAdd_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="VehicleCodeAdd" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>
								</FooterTemplate>								
                           <ControlStyle Width="100px" />         
                           <HeaderStyle Width="100px"></HeaderStyle>
							</asp:TemplateField>							
							
							<asp:TemplateField HeaderText="Vehicle Name" ItemStyle-Wrap="True" HeaderStyle-Width="700" SortExpression="VehicleName">
								<Itemtemplate>
									<asp:Label Runat="server" ID="VehicleName" text='<%# DataBinder.Eval(Container.DataItem, "VehicleName") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:TextBox Runat="server" ID="VehicleNameEdit" MaxLength="60" Width="100%" CssClass="TextBox" Text='<%# DataBinder.Eval(Container.DataItem, "VehicleName") %>'>
									</asp:TextBox>
									<cc1:TextBoxWatermarkExtender ID="VehicleNameEdit_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="VehicleNameEdit" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>									
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="VehicleNameAdd" CssClass="TextBox" MaxLength="60" Runat="Server" Width="100%"/>
									<cc1:TextBoxWatermarkExtender ID="VehicleNameAdd_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="VehicleNameAdd" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>
								</FooterTemplate>
							    <ControlStyle Width="200px" />
                            <HeaderStyle Width="700px"></HeaderStyle>
							</asp:TemplateField>	
							
							
							<asp:TemplateField HeaderText="Vehicle Type" HeaderStyle-Width="210" SortExpression="VehicleTypeName">
								<Itemtemplate>
									<asp:Label Runat="server" ID="VehicleType" text='<%# DataBinder.Eval(Container.DataItem, "VehicleTypeName") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:DropDownList Runat="server" ID="ddlVehicleTypeEdit" MaxLength="5" Width="100%" CssClass="DropDownList" 
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "VehicleType") %>' 
                                        DataSourceID="dsVehicleType" DataTextField="VehicleTypeName" 
                                        DataValueField="VehicleTypeCode">
									</asp:DropDownList>								    
								</EditItemTemplate>
								<FooterTemplate>
									<asp:DropDownList ID="ddlVehicleTypeAdd" Runat="Server" Width="100%" MaxLength="5" CssClass="DropDownList"									    
                                        DataSourceID="dsVehicleType" DataTextField="VehicleTypeName" 
                                        DataValueField="VehicleTypeCode">
									</asp:DropDownList>
								</FooterTemplate>
                                <ControlStyle Width="150px" />
                               <HeaderStyle Width="300px"></HeaderStyle>
							</asp:TemplateField>
						
							
							<asp:TemplateField HeaderText="Belongs To" HeaderStyle-Width="80" ItemStyle-HorizontalAlign="Center" SortExpression="BelongsTo">
								<Itemtemplate>
									<asp:Label Runat="server" Width="80" ID="BelongsTo" TEXT='<%# DataBinder.Eval(Container.DataItem, "BelongsTo") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:DropDownList ID="BelongsToEdit" CssClass="DropDownList" Width="100%" MaxLength="10" runat="server" AutoPostBack = "true"
									 OnSelectedIndexChanged="ddlBelongstoedit_SelectedIndexChanged" SelectedValue='<%# DataBinder.Eval(Container.DataItem, "BelongsTo") %>'>
									  <asp:ListItem>Perusahaan</asp:ListItem>
									  <asp:ListItem>Supplier</asp:ListItem>                                        									  
								</asp:DropDownList>								    
								</EditItemTemplate>
								<FooterTemplate>
									<asp:DropDownList ID="BelongsToAdd" CssClass="DropDownList" Width="100%" MaxLength="10" runat="server" AutoPostBack = "true"
									 OnSelectedIndexChanged="ddlBelongstoadd_SelectedIndexChanged" >
									  <asp:ListItem Selected="True">Perusahaan</asp:ListItem>
									  <asp:ListItem>Supplier</asp:ListItem>                                        									  
									</asp:DropDownList>								    
								</FooterTemplate>
								<ControlStyle Width="100px" />
							    <HeaderStyle Width="80px" />
                                <ItemStyle HorizontalAlign="Left" />
							</asp:TemplateField>										
							
							
							<asp:TemplateField HeaderText="Size" HeaderStyle-Width="80" ItemStyle-HorizontalAlign="Center" SortExpression="VehicleSize">
								<Itemtemplate>
									<asp:Label Runat="server" Width="80" ID="Size" TEXT='<%# DataBinder.Eval(Container.DataItem, "VehicleSize") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:DropDownList ID="SizeEdit" CssClass="DropDownList" Width="100%" runat="server" SelectedValue='<%# DataBinder.Eval(Container.DataItem, "VehicleSize") %>'>
									  <asp:ListItem>Big</asp:ListItem>
									  <asp:ListItem>Small</asp:ListItem>                                        									  
								</asp:DropDownList>								    
								</EditItemTemplate>
								<FooterTemplate>
									<asp:DropDownList ID="SizeAdd" CssClass="DropDownList" Width="100%" runat="server">
									  <asp:ListItem Selected="True">Big</asp:ListItem>
									  <asp:ListItem>Small</asp:ListItem>                                        									  
									</asp:DropDownList>								    
								</FooterTemplate>
								<ControlStyle Width="60px" />
							    <HeaderStyle Width="80px" />
                                <ItemStyle HorizontalAlign="Left" />                                
							</asp:TemplateField>	
							
							<asp:TemplateField HeaderText="Supplier" HeaderStyle-Width="210" SortExpression="SuppName">
								<Itemtemplate>
									<asp:Label Runat="server" ID="Supplier" text='<%# DataBinder.Eval(Container.DataItem, "SuppName") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:DropDownList Runat="server" ID="ddlSupplierEdit" Width="100%" CssClass="DropDownList"                                         
                                        DataSourceID="dsSupplier" DataTextField="SuppName" 
                                        DataValueField="SuppCode">
									</asp:DropDownList>								    
								</EditItemTemplate>
								<FooterTemplate>
									<asp:DropDownList ID="ddlSupplierAdd" Runat="Server" Width="100%" CssClass="DropDownList"									    
                                        DataSourceID="dsSupplier" DataTextField="SuppName" 
                                        DataValueField="SuppCode">
									</asp:DropDownList>
								</FooterTemplate>
								<ControlStyle Width="150px" />
                               <HeaderStyle Width="210px"></HeaderStyle>
							</asp:TemplateField>																						
							
						<asp:TemplateField HeaderText="Expire Date" HeaderStyle-Width="80" ItemStyle-HorizontalAlign="Center" SortExpression="ExpireDate">
								<Itemtemplate>
									<asp:Label Runat="server" Width="80" ID="ExpireDate" TEXT='<%# DataBinder.Eval(Container.DataItem, "ExpireDate","{0:dd MMM yyyy}") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
	                            <BDP:BasicDatePicker ID="tbDateEdit" runat="server" DateFormat="dd MMM yyyy" 
                                    SelectedValue='<%# DataBinder.Eval(Container.DataItem, "ExpireDate") %>' 
                                    AutoPostBack = "true"	                            
                                    ReadOnly = "true" ShowNoneButton="false"
                                    ButtonImageHeight="19px" ButtonImageWidth="20px" 
                                    DisplayType="TextBoxAndImage" 
                                    TextBoxStyle-CssClass="TextDate"><TextBoxStyle CssClass="TextDate" />
                                 </BDP:BasicDatePicker>                
              
								</EditItemTemplate>
								<FooterTemplate>
								  <BDP:BasicDatePicker ID="tbDateAdd" runat="server" DateFormat="dd/MM/yyyy" 
                                    SelectedValue='<%# DataBinder.Eval(Container.DataItem, "ExpireDate") %>' 	                            
                                    AutoPostBack = "true"	                            
                                    ReadOnly = "true" ShowNoneButton="false"
                                    ButtonImageHeight="19px" ButtonImageWidth="20px" 
                                    DisplayType="TextBoxAndImage" 
                                    TextBoxStyle-CssClass="TextDate"><TextBoxStyle CssClass="TextDate" />
                                 </BDP:BasicDatePicker>                
              
								</FooterTemplate>
							    <HeaderStyle Width="80px" />
                                <ItemStyle HorizontalAlign="Center" />
							</asp:TemplateField>		
							
						
						
						   <asp:TemplateField HeaderText="Active" HeaderStyle-Width="80" ItemStyle-HorizontalAlign="Center" SortExpression="FgActive">
								<Itemtemplate>
									<asp:Label Runat="server" Width="80" ID="Active" TEXT='<%# DataBinder.Eval(Container.DataItem, "FgActive") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:DropDownList ID="ActiveEdit" CssClass="DropDownList" Width="100%" runat="server" SelectedValue='<%# DataBinder.Eval(Container.DataItem, "FgActive") %>'>
									  <asp:ListItem>Y</asp:ListItem>
									  <asp:ListItem>N</asp:ListItem>                                        									  
								</asp:DropDownList>								    
								</EditItemTemplate>
								<FooterTemplate>
									<asp:DropDownList ID="ActiveAdd" CssClass="DropDownList" Width="100%" runat="server">
									  <asp:ListItem Selected="True">Y</asp:ListItem>
									  <asp:ListItem>N</asp:ListItem>                                        									  
									</asp:DropDownList>								    
								</FooterTemplate>
							    <HeaderStyle Width="80px" />
                                <ItemStyle HorizontalAlign="Center" />
							</asp:TemplateField>		
							
								
					
							<asp:TemplateField HeaderText="Action" headerstyle-width="600" >
								<ItemTemplate>
							        <asp:Button class="bitbtn btnedit" runat="server" ID="btnEdit" Text="Edit" CommandName="Edit" />																						 											
									
									<asp:Button class="bitbtn btndelete" runat="server" ID="btnDelete" Text="Delete" CommandName="Delete"  OnClientClick="return confirm('Sure to delete this data?');" />																						 																		
									
								</ItemTemplate>
								<EditItemTemplate>
								    <asp:Button class="bitbtn btnsave" runat="server" ID="btnUpdate" Text="Save" CommandName="Update" />																						 																																		
									<asp:Button class="bitbtn btncancel" runat="server" ID="btnCancel" Text="Cancel" CommandName="Cancel" />																						 																		
								</EditItemTemplate>
								<FooterTemplate>
									<asp:Button class="bitbtn btnadd" runat="server" ID="btnAdd" Text="Add" CommandName="Insert" />																						 																		
								</FooterTemplate>
                              <HeaderStyle Width="600"></HeaderStyle>
							</asp:TemplateField>
							
    					</Columns>
        </asp:GridView>
        <asp:SqlDataSource ID="dsVehicleType" runat="server"                                                                                 
           SelectCommand="SELECT VehicleTypeCode, VehicleTypeName FROM MsVehicleType">                                        
        </asp:SqlDataSource>        
        <asp:SqlDataSource ID="dsSupplier" runat="server"                                                                                 
           SelectCommand="SELECT SuppCode, SuppName FROM MsSupplier">                                        
        </asp:SqlDataSource>        
        
     <asp:Label ID="lstatus" ForeColor="red" runat="server"></asp:Label>
    </div>
    </form>
</body>

</html>
