<%@ Page Language="VB" AutoEventWireup="false" CodeFile="MsPricePremiOperator.aspx.vb" Inherits="MsPriceBMOperator" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<%@ Register Assembly="BasicFrame.WebControls.BasicDatePicker" Namespace="BasicFrame.WebControls"
    TagPrefix="BDP" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Price Bm & Operator</title>
    <script src="../../Function/OpenDlg.js" type="text/javascript"></script>
    <script src="../../Function/Function.JS" type="text/javascript"></script>
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
 <link href="https://fonts.googleapis.com/css2?family=Muli:wght@300;400;600&display=swap" rel="stylesheet">
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div class="Content">
     <div class="H1"> Price Premi Operator (Komersil)</div>
     <hr style="color:Blue" />
     <asp:Panel runat="server" ID="pnlHd">
      <table>
        <tr>
            <td style="width:100px;text-align:right" >Quick Search :</td>
            <td><asp:TextBox runat="server" ID ="tbFilter" CssClass="TextBox" /> 
                <asp:DropDownList runat="server" ID="ddlField" CssClass="DropDownList" > 
                    <asp:ListItem Selected="true" Text="Tph Start" Value="TphStart"></asp:ListItem>
                    <asp:ListItem Value="dbo.FormatDate(EffectiveDate)">Effective Date</asp:ListItem>
                    <asp:ListItem  Text="Product" Value="Product"></asp:ListItem>
                    <asp:ListItem  Text="Product Name" Value="ProductName"></asp:ListItem>                    
                    <asp:ListItem  Text="Tph Finish" Value="TphFinish"></asp:ListItem>
                    <asp:ListItem Text="Hari Hitam" Value="FgHariHitam"></asp:ListItem>                   
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
                    <asp:ListItem Selected="true" Text="Tph Start" Value="TphStart"></asp:ListItem>
                    <asp:ListItem Value="dbo.FormatDate(EffectiveDate)">Effective Date</asp:ListItem>
                    <asp:ListItem  Text="Product" Value="Product"></asp:ListItem>
                    <asp:ListItem  Text="Product Name" Value="ProductName"></asp:ListItem>                    
                    <asp:ListItem  Text="Tph Finish" Value="TphFinish"></asp:ListItem>
                    <asp:ListItem Text="Hari Hitam" Value="FgHariHitam"></asp:ListItem> 
                  </asp:DropDownList>
            </td>
        </tr>
     </table>
     </asp:Panel>
      <br />
      <asp:Button class="bitbtn btnadd" runat="server" ID="btnAdd" Text="Add" />

      <asp:GridView id="DataGrid" runat="server" ShowFooter="True" AllowSorting="True" 
            AutoGenerateColumns="False" AllowPaging="True" CssClass="Grid">
						<HeaderStyle CssClass="GridHeader" Wrap="false"  ></HeaderStyle>
						<RowStyle CssClass="GridItem" wrap="false" />
						<AlternatingRowStyle CssClass="GridAltItem"/>
						<FooterStyle CssClass="GridFooter" />
						<PagerStyle CssClass="GridPager" HorizontalAlign="Left" />
				        <Columns>

					  <asp:TemplateField HeaderText="Action">
							    <ItemTemplate>
								    <asp:Button ID="btnEdit" runat="server" class="bitbtndt btnedit" Text="Edit" CommandName="Edit"/>
									<asp:Button ID="btndelete" runat="server" class="bitbtndt btndelete" Text="Delete" OnClientClick="return confirm('Sure to delete this data?');" CommandName="Delete"/>									
								</ItemTemplate>								
							</asp:TemplateField>

				            <asp:BoundField DataField="TphStart" HeaderText="TPH Start" HeaderStyle-Width="30" SortExpression="TphStart"/>
			
							<asp:BoundField DataField="EffectiveDate" dataformatstring="{0:dd MMM yyyy}" htmlencode="true" HeaderStyle-Width="80px" SortExpression="EffectiveDate" HeaderText="Effective Date"></asp:BoundField>

                            <asp:BoundField DataField="ProductCode" HeaderText="Product" HeaderStyle-Width="30" SortExpression="ProductCode"/>

                            <asp:BoundField DataField="ProductName" HeaderText="ProductName" HeaderStyle-Width="30" SortExpression="ProductName"/>


                             <asp:BoundField DataField="TphFinish" HeaderText="TPH Finish" HeaderStyle-Width="30" SortExpression="TphFinish"/>

							<asp:BoundField DataField="Price" DataFormatString="{0:#,##0.00}" HeaderStyle-Width="80px" SortExpression="tbPriceFgN" ItemStyle-HorizontalAlign="Right" HeaderText="Price"></asp:BoundField>

                            <asp:BoundField DataField="FgHariHitam" HeaderText="Hari Hitam" HeaderStyle-Width="30" SortExpression="FgHariHitam"/>

    					</Columns>
        </asp:GridView>
        <asp:Button class="bitbtn btnadd" runat="server" ID="btnAdd2" Text="Add" Visible="false" />									
      </asp:Panel>
      <asp:Panel runat="server" ID="pnlInput" Visible="false">
        <table>
		
		
			<tr>
                <td>Division</td>
                <td>:</td>
                <td><asp:DropDownList runat="server" CssClass="DropDownList" ID="ddldivision" Width = "100px" Height="20px">
                    <asp:ListItem Selected = "True">Choose One</asp:ListItem></asp:DropDownList>
                </td>
            </tr>
			
            <tr>
                  <td>Effective Date</td>
                <td>:</td>
                <td><BDP:BasicDatePicker ID="tbDate" runat="server" AutoPostBack="True" 
                        ButtonImageHeight="19px" ButtonImageWidth="20px" DateFormat="dd MMM yyyy" 
                        DisplayType="TextBoxAndImage" ReadOnly="true" ShowNoneButton="False" 
                        TextBoxStyle-CssClass="TextDate" ValidationGroup="Input">
                        <TextBoxStyle CssClass="TextDate" />
                    </BDP:BasicDatePicker>
                </td>
            </tr>

            <tr>
                <td>
                    <asp:LinkButton ID="lbProduct" runat="server" Text="Product" />
                </td>
                <td>:</td>
                <td colspan="4">
                <asp:TextBox ID="tbProduct" runat="server" AutoPostBack="true" 
                    CssClass="TextBox" />
                    <asp:TextBox ID="tbProductName" runat="server" CssClass="TextBox" Enabled="False" 
                    EnableTheming="True" ReadOnly="True" Width="200px" />
                    <asp:Button Class="btngo" ID="btnProduct" Text="..." runat="server" />                  
                                        
                </td>
            </tr>

            <tr>
                <td>Tujuan</td>
                <td>:</td>
                <td><asp:DropDownList runat="server" CssClass="DropDownList" ID="ddlTujuan" Width = "100px" Height="20px">
                    <asp:ListItem Selected = "True">Choose One</asp:ListItem></asp:DropDownList>
                </td>
            </tr>

            <tr>
                <td>Hari Hitam</td>
                <td>:</td>
                <td><asp:DropDownList runat="server" CssClass="DropDownList" AutoPostBack="True" ID="ddlFgHitam" Width = "50px" >
                    <asp:ListItem Selected = "True">N</asp:ListItem>
                    <asp:ListItem >Y</asp:ListItem></asp:DropDownList>
                </td>
            </tr>

            <tr>
                <td>
                    Premi Operator
                </td>
                <td>:</td>
                <td >
                <asp:TextBox ID="tbPriceFgN" runat="server" CssClass="TextBox" 
                ValidationGroup="Input" Width="91px" />

                <asp:TextBox ID="tbPriceFgY" runat="server" CssClass="TextBox" 
                ValidationGroup="Input" Width="91px"  />                   
                </td>
               
            </tr>
			
						
            <tr>
                <td align="center" colspan="3">
                    <asp:Button ID="BtnSave" runat="server" class="bitbtndt btnsave" 
                        CommandName="Update" Text="Save" />
                    &nbsp;
                    <asp:Button ID="btnCancel" runat="server" class="bitbtndt btncancel" 
                        CommandName="Cancel" Text="Cancel" />
                    &nbsp;
                    <asp:Button ID="btnReset" runat="server" class="bitbtndt btncancel" 
                        CommandName="Cancel" Text="Reset" />
                    &nbsp;
                </td>
            </tr>
        </table>
      </asp:Panel>              
     <asp:Label ID="lstatus" ForeColor="red" runat="server"></asp:Label>        
    </div>
    </form>
</body>
</html>
