<%@ Page Language="VB" AutoEventWireup="false" CodeFile="MsCompetence.aspx.vb" Inherits="Master_MsCompetence_MsCompetence" %>
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
 <link href="https://fonts.googleapis.com/css2?family=Muli:wght@300;400;600&display=swap" rel="stylesheet">
    </head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div class="Content">
     <div class="H1">Competence File</div>
     <hr style="color:Blue" />
     <asp:Panel id="pnlHd" runat="server">
        <table>
        <tr>
            <td style="width:100px;text-align:right" >Quick Search :</td>
            <td><asp:TextBox runat="server" ID ="tbFilter" CssClass="TextBox"/> 
                <asp:DropDownList runat="server" ID="ddlField" CssClass="DropDownList" >
                  <asp:ListItem Selected="true" Text="Competence Code" Value="CompetenceCode"></asp:ListItem>
                  <asp:ListItem Text="Competence Name" Value="CompetenceName"></asp:ListItem>        
                  <asp:ListItem Text="Description1" Value="Description1"></asp:ListItem>        
                  <asp:ListItem Text="Description2" Value="Description2"></asp:ListItem>        
                  <asp:ListItem Text="Type" Value="Type"></asp:ListItem>        
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
                    <asp:ListItem Selected="true" Text="Competence Code" Value="CompetenceCode"></asp:ListItem>
                    <asp:ListItem Text="Competence Name" Value="CompetenceName"></asp:ListItem>        
                    <asp:ListItem Text="Description1" Value="Description1"></asp:ListItem>        
                    <asp:ListItem Text="Description2" Value="Description2"></asp:ListItem>        
                    <asp:ListItem Text="Type" Value="Type"></asp:ListItem>        
                  </asp:DropDownList>
                
            </td>
        </tr>
     </table>
     </asp:Panel>
      <br />
        <asp:Button class="bitbtn btnadd" runat="server" ID="btnAdd" Text="Add"/>&nbsp &nbsp &nbsp
      <br />
      <asp:GridView id="DataGrid" runat="server" ShowFooter="True" AllowSorting="True" 
            AutoGenerateColumns="False" AllowPaging="True" CssClass="Grid">
						<HeaderStyle CssClass="GridHeader" Wrap = "false"></HeaderStyle>
						<RowStyle CssClass="GridItem" Wrap="true"/>
						<AlternatingRowStyle CssClass="GridAltItem"/>
						<FooterStyle CssClass="GridFooter" />
						<PagerStyle CssClass="GridPager" />
				      <Columns>
				            <asp:TemplateField HeaderStyle-Width="130" HeaderText="Action" ItemStyle-Wrap = "false">
                                <ItemTemplate>
                                   <asp:DropDownList CssClass="DropDownList" ID="ddl" runat="server">
                                   <asp:ListItem Selected="True" Text="View" />
                                   <asp:ListItem Text="Edit" />
                                   <asp:ListItem>Delete</asp:ListItem> 
                                   <asp:ListItem Text="Detail" />                  
                                   </asp:DropDownList>
                                   <asp:Button class="btngo" runat="server" ID="btnGo" Text="G" CommandName="Go" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>"/>
                                </ItemTemplate>
                            <HeaderStyle Width="130" />
                            </asp:TemplateField>
				          
							<asp:TemplateField HeaderText="Competence Code" SortExpression="CompetenceCode">
								<Itemtemplate>
									<asp:Label Runat="server" ID="CompetenceCode" text='<%# DataBinder.Eval(Container.DataItem, "CompetenceCode") %>'>
									</asp:Label>
								</Itemtemplate>
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Competence Name" HeaderStyle-Width="250px" SortExpression="CompetenceName">
								<Itemtemplate>
									<asp:Label Runat="server" ID="CompetenceName" text='<%# DataBinder.Eval(Container.DataItem, "CompetenceName") %>'>
									</asp:Label>
								</Itemtemplate>
							<HeaderStyle Width="250px"></HeaderStyle>
							</asp:TemplateField>	
							
							<asp:TemplateField HeaderText="Description1" HeaderStyle-Width="350" SortExpression="Description1">
								<Itemtemplate>
									<asp:Label Runat="server" ID="Description1" width="350" text='<%# DataBinder.Eval(Container.DataItem, "Description1") %>'>
									</asp:Label>
								</Itemtemplate>
							</asp:TemplateField>		
							
							<asp:TemplateField HeaderText="Description2" HeaderStyle-Width="350" SortExpression="Description2">
								<Itemtemplate>
									<asp:Label Runat="server" ID="Description2" width="350" text='<%# DataBinder.Eval(Container.DataItem, "Description2") %>'>
									</asp:Label>
								</Itemtemplate>
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Type" HeaderStyle-Width="80" SortExpression="Type">
								<Itemtemplate>
									<asp:Label Runat="server" ID="Type" width="80" text='<%# DataBinder.Eval(Container.DataItem, "Type") %>'>
									</asp:Label>
								</Itemtemplate>
							</asp:TemplateField>		
					</Columns>
        </asp:GridView>
        <asp:Button class="bitbtn btnadd" runat="server" ID="btnAdd2" Text="Add" 
             Visible="False" /> &nbsp &nbsp &nbsp                   
     </asp:Panel>   
     
     <asp:Panel runat="server" ID="pnlView" Visible="false">
      <table style="width: 585px">
        <tr>
            <td>Competence Code</td>
            <td>:</td>
            <td><asp:TextBox CssClass="TextBox" runat="server" ID="tbCode" MaxLength="5" 
                    Width="142px"/> &nbsp; &nbsp; </td>            
        </tr>
        <tr>
            <td>Competence Name</td>
            <td>:</td>
            <td><asp:TextBox CssClass="TextBox" runat="server" ID="tbName" MaxLength="60" 
                    Width="420px"/> &nbsp; &nbsp; </td>            
        </tr>      
        <tr>
            <td>Description 1</td>
            <td>:</td>
            <td><asp:TextBox CssClass="TextBox" runat="server" ID="tbDescription1" 
                    MaxLength="255" Width="418px" Height="64px" TextMode="MultiLine"/> &nbsp; &nbsp; </td>            
        </tr>
        <tr>
            <td>Description 2</td>
            <td>:</td>
            <td><asp:TextBox CssClass="TextBox" runat="server" ID="tbDescription2" 
                    MaxLength="255" Width="418px" Height="62px" TextMode="MultiLine"/> &nbsp; &nbsp; </td>            
        </tr>            
        <tr>
            <td>Type</td>
            <td>:</td>
            <td><asp:DropDownList ID="ddlType" runat="server" AutoPostBack="True" 
                    CssClass="DropDownList" Height="16px" Width="71px">
                <asp:ListItem>UMUM</asp:ListItem>
                <asp:ListItem>KHUSUS</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
           <td>&nbsp;</td>
           <td>&nbsp;</td>
           <td>
           <br />
           <asp:Button ID="btnSaveHd" runat="server" class="bitbtndt btnsave" Text="Save" />									
           <asp:Button ID="btnCancelHd" runat="server" class="bitbtndt btncancel" Text="Cancel" CommandName="Cancel"/>									                                    
           <asp:Button ID="btnReset" runat="server" class="bitbtndt btncancel" Text="Reset" CommandName="Reset"/>       
           </td>
          </tr>
      </table>  
      
      <asp:Button class="bitbtn btnback" runat="server" ID="btnBack" Text="Back" />
      <br />
      <br />
      </asp:Panel>  
	    
     <asp:Panel ID="pnlDt" runat="server" Visible = "False">  
     <table>
     <tr>
     <td>
     <asp:Label ID="label1" CssClass="H1" runat="server" Text="Competence : " />   
     <asp:Label ID="lbCompetenceCode" ForeColor="Blue" CssClass="H1" Font-Bold="true" runat="server" />
     <asp:Label ID="Label2" Text = " - " ForeColor="Blue" CssClass="H1" Font-Bold="true" runat="server" />
     <asp:Label ID="lbCompetenceName" ForeColor="Blue" CssClass="H1" Font-Bold="true" runat="server" />
     </td>
     </tr>
     </table>     
     
	 <br />
	 <asp:Button class="bitbtn btnback" runat="server" ID="btnBackDtTop" Text="Back" />
	 <br />
	 
	    <div style="border:0px  solid; width:100%; height:100%; overflow:auto;">
	    
            <asp:GridView id="DataGridDt" runat="server" ShowFooter="True" AllowSorting="True" 
            AutoGenerateColumns="False" AllowPaging="True">
						<HeaderStyle CssClass="GridHeader" wrap="False"></HeaderStyle>
						<RowStyle CssClass="GridItem" wrap="true"/>
						<AlternatingRowStyle CssClass="GridAltItem"/>
						<FooterStyle CssClass="GridFooter" Wrap="false" />
						<PagerStyle CssClass="GridPager" />
				      <Columns>
				            <asp:TemplateField HeaderText="No" SortExpression="ItemNo" >
								<Itemtemplate>
									<asp:Label Runat="server" ID="ItemNo" text='<%# DataBinder.Eval(Container.DataItem, "ItemNo") %>'>
									</asp:Label>
								</Itemtemplate>		
								<EditItemTemplate>
								    <asp:Label Runat="server" ID="ItemNoEdit" CssClass="TextBox" Text='<%# DataBinder.Eval(Container.DataItem, "ItemNo") %>'></asp:Label>	
								</EditItemTemplate>	
								<FooterTemplate>
								    <asp:Label Runat="server" ID="ItemNoAdd" CssClass="TextBox" Text=""></asp:Label>	
								</FooterTemplate>
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Description1" HeaderStyle-Width="350" SortExpression="Description1">
								<Itemtemplate>
									<asp:Label Runat="server" ID="Description1" Width = "350" text='<%# DataBinder.Eval(Container.DataItem, "Description1") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:TextBox Runat="server" ID="Description1Edit" CssClass="TextBox" Width="100%" MaxLength="255" Text='<%# DataBinder.Eval(Container.DataItem, "Description1") %>'>
									</asp:TextBox>									
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="Description1Add" CssClass="TextBox" Width="100%" MaxLength="255" Runat="Server"/>
								    <cc1:TextBoxWatermarkExtender ID="Description1Add_TextBoxWatermarkExtender" 
                                        runat="server" Enabled="True" TargetControlID="Description1Add" WatermarkText="can't blank" WatermarkCssClass="Watermarked" >
                                    </cc1:TextBoxWatermarkExtender>
								</FooterTemplate>
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Description2" HeaderStyle-Width="350" SortExpression="Description2">
								<Itemtemplate>
									<asp:Label Runat="server" ID="Description2" Width = "350" text='<%# DataBinder.Eval(Container.DataItem, "Description2") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:TextBox Runat="server" ID="Description2Edit" CssClass="TextBox" Width="100%" MaxLength="255" Text='<%# DataBinder.Eval(Container.DataItem, "Description2") %>'>
									</asp:TextBox>									
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="Description2Add" CssClass="TextBox" Width="100%" MaxLength="255" Runat="Server"/>
								    <cc1:TextBoxWatermarkExtender ID="Description2Add_TextBoxWatermarkExtender" 
                                        runat="server" Enabled="True" TargetControlID="Description2Add" WatermarkText="can't blank" WatermarkCssClass="Watermarked" >
                                    </cc1:TextBoxWatermarkExtender>
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
        
     <asp:Label ID="lstatus" ForeColor="red" runat="server"></asp:Label>
    </div>
    </form>
</body>
</html>
