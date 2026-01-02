<%@ Page Language="VB" AutoEventWireup="false" CodeFile="MsHoliday.aspx.vb" Inherits="MsHoliday_MsHoliday" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<%@ Register Assembly="BasicFrame.WebControls.BasicDatePicker" Namespace="BasicFrame.WebControls"
    TagPrefix="BDP" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Holiday File</title>
    <script src="../../Function/OpenDlg.js" type="text/javascript"></script>
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
 <link href="https://fonts.googleapis.com/css2?family=Muli:wght@300;400;600&display=swap" rel="stylesheet">
</head>
<body>
  <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>    
     <div class="Content">
     <div class="H1">Holiday File</div>
     <hr style="color:Blue" />   
      <table>
        <tr>
            <td style="width:100px;text-align:right" >Quick Search :</td>      
            
            <td><asp:DropDownList ID="ddlYear" Runat="Server"  CssClass="DropDownList"									    
                    OnSelectedIndexChanged =  "ddlYear_SelectedIndexChanged" AutoPostBack ="true"  >
                  </asp:DropDownList>  
                 <asp:TextBox runat="server" ID ="tbFilter" CssClass="TextBox"/> 
                <asp:DropDownList runat="server" ID="ddlField" CssClass="DropDownList" >
                  <asp:ListItem Selected="true" Text="Holiday Date" Value="HolidayDate"></asp:ListItem>
                  <asp:ListItem Text="Holiday Name" Value="HolidayName"></asp:ListItem>        
                  <asp:ListItem Text="Force Leave" Value="FgForceLeave"></asp:ListItem>        
                </asp:DropDownList>                   
            
                    
                <asp:Button class="bitbtn btnsearch" runat="server" ID="btnSearch" Text="Search" />                
                <asp:Button class="btngo" runat="server" ID="btnExpand" Text="..."/>                
                <asp:Button class="bitbtn btnprint" runat="server" ID="btnPrint" Text="Print"/>               
                <asp:Button ID="btnShowPopup" runat="server" Style="display: none" />
                &nbsp &nbsp &nbsp                   
			    
                <asp:Button class="bitbtndt btnGo" runat="server" ID="BtnGenerateTop" Text="Generate" width = "80" />									                                                                                                                                                                                
                            
                <cc1:ModalPopupExtender ID="ModalPopupExtender1" runat="server" 
                    PopupControlID="pnlFind" DropShadow="True" TargetControlID="btnShowPopup"                     
                    BackgroundCssClass="BackgroundStyle" />                           
                                   
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
                  <asp:ListItem Selected="true" Text="Holiday Date" Value="HolidayDate"></asp:ListItem>
                  <asp:ListItem Text="Holiday Name" Value="HolidayName"></asp:ListItem>        
                  <asp:ListItem Text="Force Leave" Value="FgForceLeave"></asp:ListItem>        
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
							<asp:TemplateField HeaderText="Holiday Date" HeaderStyle-Width="100" SortExpression="HolidayDate">
								<Itemtemplate>
									<asp:Label Runat="server" ID="HolidayDate" text='<%# DataBinder.Eval(Container.DataItem, "Holiday_Date") %>'>
									</asp:Label>									
								</Itemtemplate>
								<EditItemTemplate>
									<BDP:BasicDatePicker ID="HolidayDateEdit" runat="server" DateFormat="dd/MMM/yyyy" 
                                      ReadOnly = "true" Enabled="false"
                                      ButtonImageHeight="19px" ButtonImageWidth="20px" 
                                      DisplayType="TextBoxAndImage" 
                                      TextBoxStyle-CssClass="TextDate" AutoPostBack="True" 
                                      ShowNoneButton="False" SelectedValue = '<%# DataBinder.Eval(Container.DataItem, "HolidayDate") %>' >
                                     <TextBoxStyle CssClass="TextDate" />
                                    </BDP:BasicDatePicker>
								</EditItemTemplate>
								<FooterTemplate>
									<BDP:BasicDatePicker ID="HolidayDateAdd" runat="server" DateFormat="dd/MMM/yyyy" 
                                      ReadOnly = "true" ValidationGroup="Input"
                                      ButtonImageHeight="19px" ButtonImageWidth="20px" 
                                      DisplayType="TextBoxAndImage" 
                                      TextBoxStyle-CssClass="TextDate" AutoPostBack="True" 
                                      ShowNoneButton="False" >
                                     <TextBoxStyle CssClass="TextDate" />
                                    </BDP:BasicDatePicker>   
								</FooterTemplate>								
							</asp:TemplateField>							
							
							<asp:TemplateField HeaderText="Holiday Name" HeaderStyle-Width="368" SortExpression="HolidayName">
								<Itemtemplate>
									<asp:Label Runat="server" ID="HolidayName" text='<%# DataBinder.Eval(Container.DataItem, "HolidayName") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:TextBox Runat="server" ID="HolidayNameEdit" MaxLength="30" Width="100%" CssClass="TextBox" Text='<%# DataBinder.Eval(Container.DataItem, "HolidayName") %>'>
									</asp:TextBox>
									<cc1:TextBoxWatermarkExtender ID="HolidayNameEdit_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="HolidayNameEdit" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>									
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="HolidayNameAdd" CssClass="TextBox" MaxLength="30" Runat="Server" Width="100%"/>
									<cc1:TextBoxWatermarkExtender ID="HolidayNameAdd_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="HolidayNameAdd" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>
								</FooterTemplate>
							</asp:TemplateField>	
							
							<asp:TemplateField HeaderText="Force Leave" HeaderStyle-Width="80" SortExpression="FgForceLeave">
								<Itemtemplate>
									<asp:Label Runat="server" Width="30" ID="ForceLeave" text='<%# DataBinder.Eval(Container.DataItem, "FgForceLeave") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:DropDownList ID="ForceLeaveEdit" CssClass="DropDownList" Width="100%" runat="server" SelectedValue='<%# DataBinder.Eval(Container.DataItem, "FgForceLeave") %>'>
									  <asp:ListItem>N</asp:ListItem>
									  <asp:ListItem>Y</asp:ListItem>                                        
									</asp:DropDownList>								    
								</EditItemTemplate>
								<FooterTemplate>
									<asp:DropDownList ID="ForceLeaveAdd" CssClass="DropDownList" Width="100%" runat="server">
									  <asp:ListItem Selected="True">N</asp:ListItem>
									  <asp:ListItem>Y</asp:ListItem>                                        
									</asp:DropDownList>								    
								</FooterTemplate>
							    <HeaderStyle Width="80px" />
                                <ItemStyle HorizontalAlign="Left" />
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
         
         <asp:Panel runat="server" ID="PnlGenerate" Visible="false">         
         <table>
         
                <tr>
                    <td>Year</td>
                    <td>:</td>
                    <td>                                  
                        <asp:DropDownList ID="ddlSelectYear" runat="server" CssClass="DropDownList" /> 
                    </td>
                </tr>         
         
                <tr>
                    <td>Off Day</td>
                    <td>:</td>
                    <td>                                  
                       	<asp:DropDownList ID="ddlOffDay" CssClass="DropDownList" Width="70px" runat="server">
						  <asp:ListItem Selected="True" Value = "1">Minggu</asp:ListItem>
						  <asp:ListItem Value = "2">Senin</asp:ListItem>
						  <asp:ListItem Value = "3">Selasa</asp:ListItem>
						  <asp:ListItem Value = "4">Rabu</asp:ListItem>
						  <asp:ListItem Value = "5">Kamis</asp:ListItem>
						  <asp:ListItem Value = "6">Jumat</asp:ListItem>
						  <asp:ListItem Value = "7">Sabtu</asp:ListItem>						  						  
						</asp:DropDownList>	
                    </td>
                </tr>                
                
                <tr>
                    <td>Description</td>
                    <td>:</td>
                    <td>
                    <asp:TextBox ID="tbDescription" runat="server"  MaxLength="30"
                      CssClass="TextBox" Width="300px" />                                        
                    </td>
                </tr>    
                <tr>
                    <td colspan="3" style="text-align: center">
                       <asp:Button class="btngo" runat="server" ID="btnGenerate" Text="Generate" width = "80" />									                                                                                                                                                                                
                       <asp:Button class="bitbtndt btncancel" runat="server" ID="btnCancel" Text="Cancel" width = "90" />									                                                                                                                                                                                
                        
                    </td>
                </tr>                 
         
         </table>
         </asp:Panel>      
         
        
        
     <BDP:BasicDatePicker ID="HolidayDateTemp" runat="server" DateFormat="dd/MMM/yyyy" 
                             ReadOnly = "true" ValidationGroup="Input"
                             ButtonImageHeight="19px" ButtonImageWidth="20px" 
                             DisplayType="TextBoxAndImage" 
                             TextBoxStyle-CssClass="TextDate" AutoPostBack="True" 
                             ShowNoneButton="False" Visible = "False">
                             <TextBoxStyle CssClass="TextDate" />
     </BDP:BasicDatePicker>    
            

     <asp:Label ID="lstatus" ForeColor="red" runat="server"></asp:Label>      
     
    </div>
    </form>
</body>

</html>
