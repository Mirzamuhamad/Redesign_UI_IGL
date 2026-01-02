<%@ Page Language="VB" AutoEventWireup="false" CodeFile="MsPuasa.aspx.vb" Inherits="MsPuasa_MsPuasa" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<%@ Register Assembly="BasicFrame.WebControls.BasicDatePicker" Namespace="BasicFrame.WebControls"
    TagPrefix="BDP" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitle</title>
    <script src="../../Function/OpenDlg.js" type="text/javascript"></script>
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
 <link href="https://fonts.googleapis.com/css2?family=Muli:wght@300;400;600&display=swap" rel="stylesheet">
</head>
<body>
  <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>    
     <div class="Content">
     <div class="H1">Setting Puasa</div>
     <hr style="color:Blue" />   
     
             
      <table>
        <tr>
            <td style="width:100px;text-align:right" >Quick Search :</td>      
            
            <td><asp:TextBox runat="server" ID ="tbFilter" CssClass="TextBox"/> 
                <asp:DropDownList runat="server" ID="ddlField" CssClass="DropDownList" >
                  <asp:ListItem Selected="true" Text="Year" Value="Year"></asp:ListItem>
                  <asp:ListItem Text="Start Date #1" Value="dbo.FormatDate(Start1Date)"></asp:ListItem>        
                  <asp:ListItem Text="End Date #1" Value="dbo.FormatDate(End1Date)"></asp:ListItem>        
                  <asp:ListItem Text="Start Date #2" Value="dbo.FormatDate(Start2Date)"></asp:ListItem>        
                  <asp:ListItem Text="End Date #2" Value="dbo.FormatDate(End2Date)"></asp:ListItem>        
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
            <td style="width:160px;text-align:right">
            <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlNotasi" >
                    <asp:ListItem Selected="true" Text="OR" Value="OR"></asp:ListItem>
                    <asp:ListItem Text="AND" Value="AND"></asp:ListItem>         
                </asp:DropDownList>
            </td>
            <td><asp:TextBox runat="server" ID ="tbfilter2" CssClass="TextBox"/> 
                  <asp:DropDownList runat="server" ID="ddlField2" CssClass="DropDownList" >
                  <asp:ListItem Selected="true" Text="Year" Value="Year"></asp:ListItem>
                  <asp:ListItem Text="Start Date #1" Value="dbo.FormatDate(Start1Date)"></asp:ListItem>        
                  <asp:ListItem Text="End Date #1" Value="dbo.FormatDate(End1Date)"></asp:ListItem>        
                  <asp:ListItem Text="Start Date #2" Value="dbo.FormatDate(Start2Date)"></asp:ListItem>        
                  <asp:ListItem Text="End Date #2" Value="dbo.FormatDate(End2Date)"></asp:ListItem>        
                  </asp:DropDownList>
                
            </td>
        </tr>
     </table>
     </asp:Panel>        
     
      <br />
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
				            <asp:TemplateField HeaderStyle-Width="100" HeaderText="Year" SortExpression="Year">
                                <Itemtemplate>
                                    <asp:Label ID="Year" Runat="server" text='<%# DataBinder.Eval(Container.DataItem, "Year")%>'> </asp:Label>
                                </Itemtemplate>
                                <EditItemTemplate>
									<asp:DropDownList Runat="server" ID="YearEdit" Enabled ="false" Width="100%" CssClass="DropDownList"
                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "Year") %>'
                                        DataSourceID="dsYear" DataTextField="Year" 
                                        DataValueField="Year">
									</asp:DropDownList>													
								</EditItemTemplate>
								<FooterTemplate>
									<asp:DropDownList ID="YearAdd" Runat="Server" Width="100%" CssClass="DropDownList"
                                        DataSourceID="dsYear" DataTextField="Year"
                                        DataValueField="Year">
									</asp:DropDownList>									
								</FooterTemplate>								
                                <HeaderStyle Width="100px" />
                            </asp:TemplateField>	
									      
							<asp:TemplateField HeaderText="Start Date #1" HeaderStyle-Width="100" SortExpression="Start1Date">
								<Itemtemplate>
									<asp:Label Runat="server" ID="Start1Date" text='<%# DataBinder.Eval(Container.DataItem, "Start1Date") %>' Visible = "false">
									</asp:Label>
									<asp:Label Runat="server" ID="Start1" text='<%# DataBinder.Eval(Container.DataItem, "Start1_Date") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<BDP:BasicDatePicker ID="Start1DateEdit" runat="server" DateFormat="dd/MMM/yyyy" 
                                      ReadOnly = "true" ValidationGroup="Input"
                                      ButtonImageHeight="19px" ButtonImageWidth="20px" 
                                      DisplayType="TextBoxAndImage" 
                                      TextBoxStyle-CssClass="TextDate" AutoPostBack = "True" 
                                      ShowNoneButton="False" SelectedValue = '<%# DataBinder.Eval(Container.DataItem, "Start1Date") %>' >
                                    <TextBoxStyle CssClass="TextDate" />
                                    </BDP:BasicDatePicker>                
                               	</EditItemTemplate>
								<FooterTemplate>
									<BDP:BasicDatePicker ID="Start1DateAdd" runat="server" DateFormat="dd/MMM/yyyy" 
                                      ReadOnly = "true" ValidationGroup="Input"
                                      ButtonImageHeight="19px" ButtonImageWidth="20px" 
                                      DisplayType="TextBoxAndImage" 
                                      TextBoxStyle-CssClass="TextDate" AutoPostBack="True" 
                                      ShowNoneButton="False" >
                                     <TextBoxStyle CssClass="TextDate" />
                                    </BDP:BasicDatePicker>   
								</FooterTemplate>								
							</asp:TemplateField>							
							
							<asp:TemplateField HeaderText="End Date #1" HeaderStyle-Width="100" SortExpression="End1Date">
								<Itemtemplate>
									<asp:Label Runat="server" ID="End1Date" text='<%# DataBinder.Eval(Container.DataItem, "End1Date") %>' Visible = "false">
									</asp:Label>
									<asp:Label Runat="server" ID="End1" text='<%# DataBinder.Eval(Container.DataItem, "End1_Date") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<BDP:BasicDatePicker ID="End1DateEdit" runat="server" DateFormat="dd/MMM/yyyy" 
                                      ReadOnly = "true" ValidationGroup="Input"
                                      ButtonImageHeight="19px" ButtonImageWidth="20px" 
                                      DisplayType="TextBoxAndImage" 
                                      TextBoxStyle-CssClass="TextDate" AutoPostBack = "True" 
                                      ShowNoneButton="False" SelectedValue = '<%# DataBinder.Eval(Container.DataItem, "End1Date") %>' >
                                    <TextBoxStyle CssClass="TextDate" />
                                    </BDP:BasicDatePicker>                
                               	</EditItemTemplate>
								<FooterTemplate>
									<BDP:BasicDatePicker ID="End1DateAdd" runat="server" DateFormat="dd/MMM/yyyy" 
                                      ReadOnly = "true" ValidationGroup="Input"
                                      ButtonImageHeight="19px" ButtonImageWidth="20px" 
                                      DisplayType="TextBoxAndImage" 
                                      TextBoxStyle-CssClass="TextDate" AutoPostBack="True" 
                                      ShowNoneButton="False" >
                                     <TextBoxStyle CssClass="TextDate" />
                                    </BDP:BasicDatePicker>   
								</FooterTemplate>								
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Start Date #2" HeaderStyle-Width="100" SortExpression="Start2Date">
								<Itemtemplate>
									<asp:Label Runat="server" ID="Start2Date" text='<%# DataBinder.Eval(Container.DataItem, "Start2Date") %>' Visible = "false">
									</asp:Label>
									<asp:Label Runat="server" ID="Start2" text='<%# DataBinder.Eval(Container.DataItem, "Start2_Date") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<BDP:BasicDatePicker ID="Start2DateEdit" runat="server" DateFormat="dd/MMM/yyyy" 
                                      ReadOnly = "true" ValidationGroup="Input"
                                      ButtonImageHeight="19px" ButtonImageWidth="20px" 
                                      DisplayType="TextBoxAndImage" 
                                      TextBoxStyle-CssClass="TextDate" AutoPostBack = "True" 
                                      ShowNoneButton="False" SelectedValue = '<%# DataBinder.Eval(Container.DataItem, "Start2Date") %>' >
                                    <TextBoxStyle CssClass="TextDate" />
                                    </BDP:BasicDatePicker>                
                               	</EditItemTemplate>
								<FooterTemplate>
									<BDP:BasicDatePicker ID="Start2DateAdd" runat="server" DateFormat="dd/MMM/yyyy" 
                                      ReadOnly = "true" ValidationGroup="Input"
                                      ButtonImageHeight="19px" ButtonImageWidth="20px" 
                                      DisplayType="TextBoxAndImage" 
                                      TextBoxStyle-CssClass="TextDate" AutoPostBack="True" 
                                      ShowNoneButton="False" >
                                     <TextBoxStyle CssClass="TextDate" />
                                    </BDP:BasicDatePicker>   
								</FooterTemplate>								
							</asp:TemplateField>							
							
							<asp:TemplateField HeaderText="End Date #2" HeaderStyle-Width="100" SortExpression="End2Date">
								<Itemtemplate>
									<asp:Label Runat="server" ID="End2Date" text='<%# DataBinder.Eval(Container.DataItem, "End2Date") %>' Visible = "false">
									</asp:Label>
									<asp:Label Runat="server" ID="End2" text='<%# DataBinder.Eval(Container.DataItem, "End2_Date") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<BDP:BasicDatePicker ID="End2DateEdit" runat="server" DateFormat="dd/MMM/yyyy" 
                                      ReadOnly = "true" ValidationGroup="Input"
                                      ButtonImageHeight="19px" ButtonImageWidth="20px" 
                                      DisplayType="TextBoxAndImage" 
                                      TextBoxStyle-CssClass="TextDate" AutoPostBack = "True" 
                                      ShowNoneButton="False" SelectedValue = '<%# DataBinder.Eval(Container.DataItem, "End2Date") %>' >
                                    <TextBoxStyle CssClass="TextDate" />
                                    </BDP:BasicDatePicker>                
                               	</EditItemTemplate>
								<FooterTemplate>
									<BDP:BasicDatePicker ID="End2DateAdd" runat="server" DateFormat="dd/MMM/yyyy" 
                                      ReadOnly = "true" ValidationGroup="Input"
                                      ButtonImageHeight="19px" ButtonImageWidth="20px" 
                                      DisplayType="TextBoxAndImage" 
                                      TextBoxStyle-CssClass="TextDate" AutoPostBack="True" 
                                      ShowNoneButton="False" >
                                     <TextBoxStyle CssClass="TextDate" />
                                    </BDP:BasicDatePicker>   
								</FooterTemplate>								
							</asp:TemplateField>				
							
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
									<asp:Button class="bitbtn btnadd" runat="server" ID="btnAdd" Text="Add" CommandName="Insert" />																						 																		
								</FooterTemplate>
							</asp:TemplateField>
							
    					</Columns>
        </asp:GridView>
         <asp:SqlDataSource ID="dsYear" runat="server" SelectCommand="EXEC S_GetYear">
       </asp:SqlDataSource> 
     <asp:Label ID="lstatus" ForeColor="red" runat="server"></asp:Label>      
     
    </div>
    </form>
</body>

</html>
