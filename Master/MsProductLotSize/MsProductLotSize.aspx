    <%@ Page Language="VB" AutoEventWireup="false" CodeFile="MsProductLotSize.aspx.vb" Inherits="Master_MsProductLotSize_MsProductLotSize" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Product Type File</title>
    <script src="../../Function/Function.JS" type="text/javascript"></script>
    <script src="../../Function/OpenDlg.js" type="text/javascript"></script>
    <script type="text/javascript">
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
     <div class="H1"><asp:Label runat="server" ID="lblTitle"></asp:Label></div>
     <hr style="color:Blue" />
     <asp:Panel runat="server" ID="pnlCopy" Visible="False">
              <table width="100%">
                <tr>
                    <td style="width:100px">From Product</td>
                    <td>:</td>
                    <td>                    
                        <asp:TextBox CssClass="TextBox" Id="tbFromCode" runat="server" Width="100px" AutoPostBack="True" />
                        <asp:TextBox CssClass="TextBox" ID="tbFromName" runat="server" Width="250px" Enabled="False" /> 
                        <asp:Button ID="btnSearchFrom" runat="server" class="btngo" Text="..."/>                                                 
                    </td>
                </tr>
                <tr>
                    <td>To Product</td>
                    <td>:</td>
                    <td>
                        <asp:TextBox Id="tbToCode" CssClass="TextBox" runat="server" Width="100px" AutoPostBack="True"/>
                        <asp:TextBox ID="tbToName" CssClass="TextBox" runat="server" Width="250px" Enabled="False" /> 
                        <asp:Button ID="btnSearchTo" runat="server" class="btngo" Text="..."/>                                                 
                    </td>
                </tr>
                <tr>
                    <td>Method</td>
                    <td>:</td>
                    <td><asp:DropDownList CssClass="DropDownList" ID = "ddlGroupByCopy" runat = "server" AutoPostBack="true" >
                            <asp:ListItem Selected="True">Additional</asp:ListItem>
                            <asp:ListItem>Overwrite</asp:ListItem>
                        </asp:DropDownList> </td>
                </tr>      
                <tr>
                    <td colspan="3">
                        <asp:Button class="bitbtn btncopy" runat="server" ID="btnCopy" Text="Copy" />
                        <asp:Button class="bitbtn btncancel" runat="server" ID="btnCancel" Text="Cancel" />                        
                             
                    </td>                                   
                </tr>
                </table>
            </asp:Panel>
     <asp:Panel id="pnlHd" runat="server">
      <table>
        <tr>
            <td style="text-align:right; width:100px" >Product :
                </td>
            <td><asp:TextBox CssClass="TextBox" Visible="false" runat="server" ID ="tbFilter"/> 
                <asp:DropDownList CssClass="DropDownList" Visible="false" runat="server" ID="ddlField" >
                  <asp:ListItem Selected="true" Text="Product Type Code" Value="ProductTypeCode"></asp:ListItem>
                  <asp:ListItem Text="Product Type Name" Value="ProductTypeName"></asp:ListItem>
                  <asp:ListItem Text="Product Category" Value="ProductCategory"></asp:ListItem>
                  <asp:ListItem Text="Stock" Value="FgStock"></asp:ListItem>
                </asp:DropDownList>     
                <asp:Button class="bitbtn btnsearch" Visible="false" runat="server" ID="btnSearch" Text="Search" />
                <asp:Button class="btngo" Visible="false" runat="server" ID="btnExpand" Text="..."/>
                
                <asp:TextBox CssClass="TextBox" runat="server" ID ="tbProductCode" AutoPostBack="True"/> 
                <asp:TextBox CssClass="TextBox" runat="server" ID ="tbProductName" MaxLength="60" Width="255px" Enabled="False" />
                <asp:Button class="btngo" runat="server" ID="btnProduct" Text="..."/>
                <asp:Button class="bitbtn btncopyto" runat="server" ID="btnCopyTo" Text="Copy To" />
                <asp:Button class="bitbtn btnprint" runat="server" ID="btnPrint" Text="Print"/>
            </td>
        </tr>
     </table>
     
     <asp:Panel runat="server" ID="pnlSearch" Visible="false">
     <table>   
        <tr>
            <td style="width:100px;text-align:right"><asp:DropDownList runat="server" ID="ddlNotasi" >
                    <asp:ListItem Selected="true" Text="OR" Value="OR"></asp:ListItem>
                    <asp:ListItem Text="AND" Value="AND"></asp:ListItem>         
                </asp:DropDownList>
            </td>
            <td><asp:TextBox CssClass="Button" runat="server" ID ="tbfilter2"/> 
                  <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlField2" >
                      <asp:ListItem Selected="true" Text="Product Type Code" Value="ProductTypeCode"></asp:ListItem>
                      <asp:ListItem Text="Product Type Name" Value="ProductTypeName"></asp:ListItem>
                      <asp:ListItem Text="Product Category" Value="ProductCategory"></asp:ListItem>
                      <asp:ListItem Text="Stock" Value="FgStock"></asp:ListItem>
                   </asp:DropDownList>
                
            </td>
        </tr>
     </table>
     </asp:Panel>
      <br />
     <%-- <div style="border:0px  solid; width:100%; height:100%; overflow:auto;">--%>
      <asp:GridView id="DataGrid" runat="server" ShowFooter="True" AllowSorting="True" 
            AutoGenerateColumns="False" AllowPaging="True" CssClass="Grid">
						<HeaderStyle CssClass="GridHeader" wrap="false"></HeaderStyle>
						<RowStyle CssClass="GridItem" Wrap="false"/>
						<AlternatingRowStyle CssClass="GridAltItem"/>
						<FooterStyle CssClass="GridFooter" />
						<PagerStyle CssClass="GridPager" />
				      <Columns>
																			
							
							<asp:TemplateField HeaderText="Day" HeaderStyle-Width="30" ItemStyle-HorizontalAlign="Center" SortExpression="Day">
								<Itemtemplate>
									<asp:Label Runat="server" Width="30" ID="Day" text='<%# DataBinder.Eval(Container.DataItem, "Day") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:Label Runat="server" Width="30" ID="DayEdit" text='<%# DataBinder.Eval(Container.DataItem, "Day") %>'>
									</asp:Label>								    
								</EditItemTemplate>
								<FooterTemplate>
									<asp:DropDownList ID="DayAdd" CssClass="DropDownList" Width="100%" runat="server">
									  <asp:ListItem Selected="True">1</asp:ListItem>
									  <asp:ListItem>2</asp:ListItem>
									  <asp:ListItem>3</asp:ListItem>
									  <asp:ListItem>4</asp:ListItem>
									  <asp:ListItem>5</asp:ListItem>
									  <asp:ListItem>6</asp:ListItem>
									  <asp:ListItem>7</asp:ListItem>
									</asp:DropDownList>								    
								</FooterTemplate>
							    <HeaderStyle Width="30px" />
                                <ItemStyle HorizontalAlign="Center" />
							</asp:TemplateField>	
							
							<asp:TemplateField HeaderText="Shift" HeaderStyle-Width="210" SortExpression="Shift">
								<Itemtemplate>
									<asp:Label Runat="server" ID="Shift" text='<%# DataBinder.Eval(Container.DataItem, "Shift") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:Label Runat="server" ID="ShiftEdit" text='<%# DataBinder.Eval(Container.DataItem, "Shift") %>'>
									</asp:Label>							    
								</EditItemTemplate>
								<FooterTemplate>
									<asp:DropDownList ID="ShiftAdd" Runat="Server" Width="100%" CssClass="DropDownList"									    
                                        DataSourceID="dsShift" DataTextField="ShiftName" 
                                        DataValueField="ShiftCode">
									</asp:DropDownList>
								</FooterTemplate>
							    <HeaderStyle Width="210px" />
							</asp:TemplateField>
														
							<asp:TemplateField HeaderText="Lot Qty" HeaderStyle-Width="70" SortExpression="LotQty">
								<Itemtemplate>
									<asp:Label Runat="server" Width="70" ID="LotQty" text='<%# DataBinder.Eval(Container.DataItem, "LotQty") %>'>
									</asp:Label>
								</Itemtemplate>
								<EditItemTemplate>
									<asp:TextBox Runat="server" ID="LotQtyEdit" Width="100%" MaxLength="5" Text='<%# DataBinder.Eval(Container.DataItem, "LotQty") %>'>
									</asp:TextBox>
								</EditItemTemplate>
								<FooterTemplate>
									<asp:TextBox ID="LotQtyAdd" CssClass="TextBox" Width="100%" MaxLength="5" Runat="Server"/>								   
								</FooterTemplate>
							    <HeaderStyle Width="70px" />
							</asp:TemplateField>
										
							<asp:TemplateField HeaderText="Action" headerstyle-width="100" >
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
							    <HeaderStyle Width="100px" />
							</asp:TemplateField>							
    					</Columns>
        </asp:GridView>
            </asp:Panel>
      
    </div>    
    <asp:SqlDataSource ID="dsShift" runat="server" 
          SelectCommand="EXEC S_GetShift">
    </asp:SqlDataSource>
    <%--<asp:SqlDataSource ID="dsWrhsType" runat="server" 
      SelectCommand="EXEC S_GetWrhsType">
    </asp:SqlDataSource>--%>
    <asp:Label ID="lstatus" ForeColor="red" runat="server"></asp:Label>
    </form>
    </body>
</html>