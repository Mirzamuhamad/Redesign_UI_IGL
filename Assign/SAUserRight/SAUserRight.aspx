<%@ Page Language="VB" AutoEventWireup="false" CodeFile="SAUserRight.aspx.vb" Inherits="Assign_SAUserRight_SAUserRight" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
    <script src="../../Function/OpenDlg.js" type="text/javascript"></script>
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
 <link href="https://fonts.googleapis.com/css2?family=Muli:wght@300;400;600&display=swap" rel="stylesheet">
</head>
<body>
    <form id="form1" runat="server">
    <div class="Content">   
    <div class="H1">Assign User Right</div>
    <hr style="color:Blue" /> 
    
        <table width="100%">
        <tr>
        <td style="width: 60px; text-align: right">User</td>
        <td>:</td>
        <td>
           <asp:TextBox Id="tbCode" CssClass="TextBox" runat="server" Width="75px" AutoPostBack="true"/>
           <asp:TextBox ID="tbName" CssClass="TextBox" runat="server" Width="150px" Enabled="False" /> 
           <asp:Button ID="btnSearch" runat="server" class="btngo" Text="..."/>           
                
                &nbsp &nbsp &nbsp            
                <asp:Label runat="server" ID="lbAction" Text = "Action :"></asp:Label> 
                <asp:DropDownList ID="ddlAction" runat="server" CssClass="DropDownList">
                    <asp:ListItem>Copy To</asp:ListItem>
                    <asp:ListItem>Copy From</asp:ListItem>
                    <asp:ListItem>User Level</asp:ListItem>
                </asp:DropDownList>
              <asp:Button ID="btnAction" runat="server" class="btngo" Text="G"/>                
        </td>
        </tr>
        </table>
        
        <asp:Panel ID="pnlHd" runat="server">                                    
        <table width="100%">
           <tr>
           <td style="width: 60px; text-align: right"></td>
           <td></td>
           <td >
           <asp:DropDownList ID="ddlCommand" runat="server" CssClass="DropDownList">
               <asp:ListItem Value="Expand">Expand All</asp:ListItem>
               <asp:ListItem Value="Collapse">Collapse All</asp:ListItem>
               <asp:ListItem Value="Grant">Grant All</asp:ListItem>
               <asp:ListItem Value="Revoke">Revoke All</asp:ListItem>
           </asp:DropDownList>
           <asp:Button ID="btnGo" runat="server" class="btngo" Text="G"/>                
           <asp:Button class="bitbtn btnapply" runat="server" ID="btnSave" Width="90px" Text="Apply Data"/>                                               
            </td>
            </tr>
            </table>
            <hr />
            <div style="border:0px  solid; width:100%; height:480px; overflow:auto;">
            <table>                        
            <tr>
            <td>
                <asp:TreeView ID="TreeView1" runat="server" ShowCheckBoxes="All"
                    ExpandDepth="8" ShowLines="true" ShowExpandCollapse="true"/>
            </td>
            </tr>
            </table>
            </div>
            <hr />
    </asp:Panel>
    
    
    
    <asp:Panel ID="pnlCopy" runat="server" Visible="false">
            <table width="100%">
            <tr>
            <td style="width: 60px; text-align: right">                
            </td>
            <td></td>
            <td><asp:Label runat="server" ID="lbCopy" Font-Bold="true"></asp:Label> </td>
            </tr>
            <tr>
            <td style="width: 60px; text-align: right">User</td>
            <td>:</td>
            <td>
               <asp:TextBox Id="tbCopyCode" CssClass="TextBox" runat="server" Width="75px" AutoPostBack="true"/>
            <asp:TextBox ID="tbCopyName" CssClass="TextBox" runat="server" Width="150px" Enabled="False" /> 
            <asp:Button ID="btnCopy" runat="server" class="btngo" Text="..."/>                            
            <asp:Button class="bitbtn btnsave" runat="server" ID="btnSaveCopy" Width="90px" Text="Apply Copy"/>                                                               
            <asp:Button class="bitbtn btncancel" runat="server" ID="btnCancelCopy" Text="Cancel" />    
            </td>
            
            
            
            </tr>
            </table>
    </asp:Panel> 
        
       
    <asp:Panel ID="pnlDt" runat="server" Visible="false" Width="1187px">
        <hr />
        
    <asp:Panel ID="pnlLevel" runat="server" Visible="false">
            <table width="100%">
            <tr>
            <td>
            <asp:DropDownList ID="ddlLevel" runat="server" CssClass="DropDownList">
            <asp:ListItem Selected = "True" Value ="All">All</asp:ListItem>
               <asp:ListItem Value="FgInsert">Insert</asp:ListItem>
               <asp:ListItem Value="FgEdit">Edit</asp:ListItem>
               <asp:ListItem Value="FgDelete">Delete</asp:ListItem>
               <asp:ListItem Value="FgGetAppr">Get Approval</asp:ListItem>
               <asp:ListItem Value="FgPost">Post</asp:ListItem>
               <asp:ListItem Value="FgUnPost">Un-Post</asp:ListItem>
               <asp:ListItem Value="FgPrint">Print</asp:ListItem>
               <asp:ListItem Value="FgComplete">Complete</asp:ListItem>
               <asp:ListItem Value="FgClosing">Closing</asp:ListItem>
               <asp:ListItem Value="FgCancel">Cancel</asp:ListItem>
           </asp:DropDownList>
           <asp:DropDownList ID="ddlSelect" runat="server" CssClass="DropDownList">
               <asp:ListItem Selected ="True" >Y</asp:ListItem>
               <asp:ListItem >N</asp:ListItem>
               
           </asp:DropDownList>
           <asp:Button ID="btnLevel" runat="server" class="btngo" Text="G"/>
            </td>
            
            </tr>
            </table>
    </asp:Panel> 
        
        <div style="border-style: solid; border-color: inherit; border-width: 0px; width:100%; height:565px; overflow:auto;">
        <table>            
            <tr>
                <td><asp:gridView id="DataGrid" runat="server" AllowPaging="False" 
                        AutoGenerateColumns="False" CssClass="Grid" PageSize="15">
						<HeaderStyle CssClass="GridHeader"></HeaderStyle>
						<RowStyle CssClass="GridItem" Wrap="false" HorizontalAlign="Center"/>
						<AlternatingRowStyle CssClass="GridAltItem" HorizontalAlign="Center"/>
						<FooterStyle CssClass="GridFooter" />
						<PagerStyle CssClass="GridPager" />
				      <Columns>
							<asp:TemplateField Visible="false">
								<Itemtemplate>
									<asp:Label Runat="server" ID="ContainerId" text='<%# DataBinder.Eval(Container.DataItem, "MenuId") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:Label Runat="server" ID="ContainerIdEdit" Text='<%# DataBinder.Eval(Container.DataItem, "MenuId") %>'>
									</asp:Label>
								</EditItemTemplate>								
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Modul" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="80px" ItemStyle-Width="80px" >
								<Itemtemplate>
									<asp:Label Runat="server" ID="Modul" text='<%# DataBinder.Eval(Container.DataItem, "Modul") %>'/>									
								</Itemtemplate>
								<EditItemTemplate>
									<asp:Label Runat="server" ID="ModulEdit" text='<%# DataBinder.Eval(Container.DataItem, "Modul") %>'/>									
								</EditItemTemplate>								
							</asp:TemplateField>		
							<asp:TemplateField HeaderText="Sub Modul" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="235px" ItemStyle-Width="235px" >
								<Itemtemplate>
									<asp:Label Runat="server" ID="SubModul" text='<%# DataBinder.Eval(Container.DataItem, "SubModul") %>'/>									
								</Itemtemplate>
								<EditItemTemplate>
									<asp:Label Runat="server" ID="SubModulEdit" text='<%# DataBinder.Eval(Container.DataItem, "SubModul") %>'/>									
								</EditItemTemplate>								
							</asp:TemplateField>		
							<asp:TemplateField HeaderText="Menu Name" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="170px" ItemStyle-Width="170px" >
								<Itemtemplate>
									<asp:Label Runat="server" ID="MenuName" text='<%# DataBinder.Eval(Container.DataItem, "MenuName") %>'/>									
								</Itemtemplate>
								<EditItemTemplate>
									<asp:Label Runat="server" ID="MenuNameEdit" text='<%# DataBinder.Eval(Container.DataItem, "MenuName") %>'/>									
								</EditItemTemplate>								
							</asp:TemplateField>		
							
							<asp:TemplateField HeaderText="Insert" >
								<Itemtemplate>
									<asp:Label Runat="server" ID="FgInsert" text='<%# DataBinder.Eval(Container.DataItem, "FgInsert") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:DropDownList CssClass="DropDownList" Runat="server" ID="FgInsertEdit">
									    <asp:ListItem>Y</asp:ListItem>
									    <asp:ListItem>N</asp:ListItem>
									</asp:DropDownList>
								</EditItemTemplate>								
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Edit" >
								<Itemtemplate>
									<asp:Label Runat="server" ID="FgEdit" text='<%# DataBinder.Eval(Container.DataItem, "FgEdit") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:DropDownList CssClass="DropDownList" Runat="server" ID="FgEditEdit">
									    <asp:ListItem>Y</asp:ListItem>
									    <asp:ListItem>N</asp:ListItem>
									</asp:DropDownList>
								</EditItemTemplate>								
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Delete" >
								<Itemtemplate>
									<asp:Label Runat="server" ID="FgDelete" text='<%# DataBinder.Eval(Container.DataItem, "FgDelete") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:DropDownList CssClass="DropDownList" Runat="server" ID="FgDeleteEdit">
									    <asp:ListItem>Y</asp:ListItem>
									    <asp:ListItem>N</asp:ListItem>
									</asp:DropDownList>
								</EditItemTemplate>								
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Get Approval" >
								<Itemtemplate>
									<asp:Label Runat="server" ID="FgGetappr" text='<%# DataBinder.Eval(Container.DataItem, "FgGetappr") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:DropDownList CssClass="DropDownList" Runat="server" ID="FgGetapprEdit">
									    <asp:ListItem>Y</asp:ListItem>
									    <asp:ListItem>N</asp:ListItem>
									</asp:DropDownList>
								</EditItemTemplate>								
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Post" >
								<Itemtemplate>
									<asp:Label Runat="server" ID="FgPost" text='<%# DataBinder.Eval(Container.DataItem, "FgPost") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:DropDownList CssClass="DropDownList" Runat="server" ID="FgPostEdit">
									    <asp:ListItem>Y</asp:ListItem>
									    <asp:ListItem>N</asp:ListItem>
									</asp:DropDownList>
								</EditItemTemplate>								
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Un-Post" >
								<Itemtemplate>
									<asp:Label Runat="server" ID="FgUnPost" text='<%# DataBinder.Eval(Container.DataItem, "FgUnPost") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:DropDownList CssClass="DropDownList" Runat="server" ID="FgUnPostEdit">
									    <asp:ListItem>Y</asp:ListItem>
									    <asp:ListItem>N</asp:ListItem>
									</asp:DropDownList>
								</EditItemTemplate>								
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Print" >
								<Itemtemplate>
									<asp:Label Runat="server" ID="FgPrint" text='<%# DataBinder.Eval(Container.DataItem, "FgPrint") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:DropDownList CssClass="DropDownList" Runat="server" ID="FgPrintEdit">
									    <asp:ListItem>Y</asp:ListItem>
									    <asp:ListItem>N</asp:ListItem>
									</asp:DropDownList>
								</EditItemTemplate>								
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Complete" >
								<ItemTemplate>
									<asp:Label Runat="server" ID="FgComplete" text='<%# DataBinder.Eval(Container.DataItem, "FgComplete") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:DropDownList CssClass="DropDownList" Runat="server" ID="FgCompleteEdit">
									    <asp:ListItem>Y</asp:ListItem>
									    <asp:ListItem>N</asp:ListItem>
									</asp:DropDownList>
								</EditItemTemplate>								
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Closing" >
								<Itemtemplate>
									<asp:Label Runat="server" ID="FgClosing" text='<%# DataBinder.Eval(Container.DataItem, "FgClosing") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:DropDownList CssClass="DropDownList" Runat="server" ID="FgClosingEdit">
									    <asp:ListItem>Y</asp:ListItem>
									    <asp:ListItem>N</asp:ListItem>
									</asp:DropDownList>
								</EditItemTemplate>								
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Cancel" >
								<Itemtemplate>
									<asp:Label Runat="server" ID="FgCancel" text='<%# DataBinder.Eval(Container.DataItem, "FgCancel") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:DropDownList CssClass="DropDownList" Runat="server" ID="FgCancelEdit">
									    <asp:ListItem>Y</asp:ListItem>
									    <asp:ListItem>N</asp:ListItem>
									</asp:DropDownList>
								</EditItemTemplate>								
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Action">
								<ItemTemplate>
					                <asp:ImageButton ID="btnEdit" runat="server" CommandName="Edit"
                                        ImageUrl="../../Image/btnEditDton.png"
                                        onmouseover="this.src='../../Image/btnEditDtoff.png';"
                                        onmouseout="this.src='../../Image/btnEditDton.png';"
                                        ImageAlign="AbsBottom" />
								</ItemTemplate>
								
								<EditItemTemplate>
					                <asp:ImageButton ID="btnUpdate" runat="server"  CommandName="Update" 
                                        ImageUrl="../../Image/btnsaveDton.png"
                                        onmouseover="this.src='../../Image/btnsaveDtOff.png';"
                                        onmouseout="this.src='../../Image/btnsaveDton.png';"
                                        ImageAlign="AbsBottom" />
                                    <asp:ImageButton ID="btnCancel" runat="server" CommandName="Cancel" 
                                        ImageUrl="../../Image/btnCancelDtOn.png"
                                        onmouseover="this.src='../../Image/btnCancelDtOff.png';"
                                        onmouseout="this.src='../../Image/btnCancelDtOn.png';"
                                        ImageAlign="AbsBottom" />
								</EditItemTemplate>
							</asp:TemplateField>							
    					</Columns>    				
                </asp:gridView>
                
                </td>            
            </tr>       
        </table>
        
            
        </div>
        
        <asp:Panel ID="PnlBack" runat="server" Visible="false">
            <table width="100%">
            <tr>
            <td>
            <asp:Button class="bitbtn btnback" runat="server" ID="btnBack" Text="Back" />
            </td>
            
            </tr>
            </table>
    </asp:Panel>
        
        
    </asp:Panel>    
        <br />
     <asp:Label ID="lbStatus" runat="server" ForeColor="Red" />   
    </div>
    </form>
</body>
</html>
