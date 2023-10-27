<%@ Page Language="VB" AutoEventWireup="false" CodeFile="MsBank.aspx.vb" Inherits="Master_MsBank_MsBank" %>
<%--<%@ PreviousPageType VirtualPath="~/Default.aspx" %>--%>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Bank File</title>
    <script src="../../Function/OpenDlg.JS" type="text/javascript"></script>   

    <script src="../../JQuery/jquery-1.9.1.js" type="text/javascript"></script>    
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">     
//        function TT()
//            {
//                try                
//                {   
//                    var lbuser = parent.document.getElementById("hdpinstance").value;                    
//                    document.getElementById("hdinstance").value = lbuser;                                                           
//                } catch(err){
//                    alert(err.description);
//                }        
//            }   
        $(document).ready(function(){
            var lbuser = parent.document.getElementById("hdpinstance").value;                    
            document.getElementById("hdinstance").value = lbuser;                                                           
        });         
     </script> 
</head>
<body>
    <%--<script type="text/javascript">     
        window.onload = TT;  
    </script> --%>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <input type="hidden" id = "hdinstance" runat="server" value="" />    	
    <div class="Content">
     <div class="H1">Bank File</div>
     <hr style="color:Blue" />
      <table>
        <tr>
            <td style="width:100px;text-align:right" >Quick Search :</td>
            <td><asp:TextBox runat="server" ID ="tbFilter" CssClass="TextBox"/> 
                <asp:DropDownList runat="server" ID="ddlField" CssClass="DropDownList" >
                  <asp:ListItem Selected="true" Text="Bank Code" Value="BankCode"></asp:ListItem>
                  <asp:ListItem Text="Bank Name" Value="BankName"></asp:ListItem>        
                    <asp:ListItem Value="FgActive">Active</asp:ListItem>
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
                    <asp:ListItem Selected="true" Text="Bank Code" Value="BankCode"></asp:ListItem>
                    <asp:ListItem Text="Bank Name" Value="BankName"></asp:ListItem> 
                      <asp:ListItem Value="FgActive">Active</asp:ListItem>
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
						<PagerStyle CssClass="GridPager" HorizontalAlign="Center" />
				      <Columns>
							<asp:TemplateField HeaderText="Bank Code" HeaderStyle-Width="100" SortExpression="BankCode">
								<Itemtemplate>
									<asp:Label Runat="server" ID="BankCode" text='<%# DataBinder.Eval(Container.DataItem, "BankCode") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:Label Runat="server" ID="BankCodeEdit" Text='<%# DataBinder.Eval(Container.DataItem, "BankCode") %>'>
									</asp:Label>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="BankCodeAdd" CssClass="TextBox" MaxLength="5" Runat="Server" Width="100%"/>
									<cc1:TextBoxWatermarkExtender ID="BankCodeAdd_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="BankCodeAdd" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>
								</FooterTemplate>
							</asp:TemplateField>
							
							<asp:TemplateField HeaderText="Bank Name" HeaderStyle-Width="350" SortExpression="BankName">
								<Itemtemplate>
									<asp:Label Runat="server" ID="BankName" text='<%# DataBinder.Eval(Container.DataItem, "BankName") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:TextBox Runat="server" CssClass="TextBox" ID="BankNameEdit" MaxLength="60" Width="100%" Text='<%# DataBinder.Eval(Container.DataItem, "BankName") %>'>
									</asp:TextBox>
									<cc1:TextBoxWatermarkExtender ID="BankNameEdit_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="BankNameEdit" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="BankNameAdd" CssClass="TextBox" MaxLength="60" Runat="Server" Width="100%"/>
									<cc1:TextBoxWatermarkExtender ID="BankNameAdd_WtExt" 
                                        runat="server" Enabled="True" TargetControlID="BankNameAdd" 
                                        WatermarkText="can't blank" WatermarkCssClass="Watermarked">
                                     </cc1:TextBoxWatermarkExtender>
								</FooterTemplate>
							</asp:TemplateField>		
							
							<asp:TemplateField HeaderText="Active" SortExpression="FgActive">
								<Itemtemplate>
									<asp:Label Runat="server" ID="FgActive" text='<%# DataBinder.Eval(Container.DataItem, "FgActive") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:DropDownList Runat="server" CssClass="DropDownList" ID="FgActiveEdit" Width="100%" Text='<%# DataBinder.Eval(Container.DataItem, "FgActive") %>'>
									    <asp:ListItem>Y</asp:ListItem>
									    <asp:ListItem>N</asp:ListItem>
									</asp:DropDownList>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:DropDownList ID="FgActiveAdd" Runat="Server" CssClass="DropDownList" Width="100%">
									    <asp:ListItem Selected="True">Y</asp:ListItem>
									    <asp:ListItem>N</asp:ListItem>
									</asp:DropDownList>
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
