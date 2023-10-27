<%@ Page Language="VB" AutoEventWireup="false" CodeFile="MsPriceBMOperator.aspx.vb" Inherits="MsPriceBMOperator" %>
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
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div class="Content">
     <div class="H1">Price Bm & Operator</div>
     <hr style="color:Blue" />
     <asp:Panel runat="server" ID="pnlHd">
      <table>
        <tr>
            <td style="width:100px;text-align:right" >Quick Search :</td>
            <td><asp:TextBox runat="server" ID ="tbFilter" CssClass="TextBox" /> 
                <asp:DropDownList runat="server" ID="ddlField" CssClass="DropDownList" > 
                    <asp:ListItem Selected="true" Text="Division Code" Value="Divisi"></asp:ListItem>
                    <asp:ListItem Text="DivisionName" Value="DivisionName"></asp:ListItem>                   
                    <asp:ListItem Value="dbo.FormatDate(EffectiveDate)">Effective Date</asp:ListItem>
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
                    <asp:ListItem Selected="true" Text="Division Code" Value="Divisi"></asp:ListItem>
                    <asp:ListItem Text="DivisionName" Value="DivisionName"></asp:ListItem>                   
                    <asp:ListItem Value="dbo.FormatDate(EffectiveDate)">Effective Date</asp:ListItem>
                    
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

				            <asp:BoundField DataField="Divisi" HeaderText="Division" HeaderStyle-Width="30" SortExpression="Divisi"/>
							<asp:BoundField DataField="DivisionName" HeaderText="Division Name" HeaderStyle-Width="120" SortExpression="DivisionName"/>
							<asp:BoundField DataField="EffectiveDate" dataformatstring="{0:dd MMM yyyy}" htmlencode="true" HeaderStyle-Width="80px" SortExpression="EffectiveDate" HeaderText="Effective Date"></asp:BoundField>
							
							<asp:BoundField DataField="BmJonder_TPHtoLT" DataFormatString="{0:#,##0.00}" HeaderStyle-Width="80px" SortExpression="BmJonder_TPHtoLT" ItemStyle-HorizontalAlign="Right" HeaderText="BM Jonder TPH to LT"></asp:BoundField>
							<asp:BoundField DataField="BmJonder_TPHtoPKS" DataFormatString="{0:#,##0.00}" HeaderStyle-Width="80px" SortExpression="BmJonder_TPHtoPKS" ItemStyle-HorizontalAlign="Right" HeaderText="BM Jonder TPH to PKS"></asp:BoundField>
							<asp:BoundField DataField="BmDt_LTtoPKS" DataFormatString="{0:#,##0.00}" HeaderStyle-Width="80px" SortExpression="BmDt_LTtoPKS" ItemStyle-HorizontalAlign="Right" HeaderText="BM Dt LT to PKS"></asp:BoundField>
							<asp:BoundField DataField="BmDt_TPHtoPKS" DataFormatString="{0:#,##0.00}" HeaderStyle-Width="80px" SortExpression="BmDt_TPHtoPKS" ItemStyle-HorizontalAlign="Right" HeaderText="BM Dt TPH to PKS"></asp:BoundField>
							
							<asp:BoundField DataField="OprJonder_TPHtoLT" DataFormatString="{0:#,##0.00}" HeaderStyle-Width="80px" SortExpression="OprJonder_TPHtoLT" ItemStyle-HorizontalAlign="Right" HeaderText="Opr Jonder TPH to LT"></asp:BoundField>
							<asp:BoundField DataField="OprJonder_TPHtoPKS" DataFormatString="{0:#,##0.00}" HeaderStyle-Width="80px" SortExpression="OprJonder_TPHtoPKS" ItemStyle-HorizontalAlign="Right" HeaderText="Opr Jonder TPH to PKS"></asp:BoundField>
							<asp:BoundField DataField="OprDt_LTtoPKS" DataFormatString="{0:#,##0.00}" HeaderStyle-Width="80px" SortExpression="OprDt_LTtoPKS" ItemStyle-HorizontalAlign="Right" HeaderText="Opr Dt LT to PKS"></asp:BoundField>
							<asp:BoundField DataField="OprDt_TPHtoPKS" DataFormatString="{0:#,##0.00}" HeaderStyle-Width="80px" SortExpression="OprDt_TPHtoPKS" ItemStyle-HorizontalAlign="Right" HeaderText="Opr Dt TPH to PKS"></asp:BoundField>
							
							
    					</Columns>
        </asp:GridView>
        <asp:Button class="bitbtn btnadd" runat="server" ID="btnAdd2" Text="Add" Visible="false" />									
      </asp:Panel>
      <asp:Panel runat="server" ID="pnlInput" Visible="false">
        <table>
		
		
			<tr>
                <td>Division</td>
                <td>:</td>
                <td><asp:DropDownList runat="server" CssClass="DropDownList" ID="ddldivision" Width = "250px" Height="20px">
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
                                BM Jonder</td>
                            <td>
                                : </td>
                            <td>
                                <table cellpadding="0" cellspacing="0">
                                    <tr style="background-color:Silver;text-align:center">
                                        <td>
                                            TPH to LT</td>
                                        <td>
                                            TPH to PKS</td>                                    
                                       
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="tbTphToLTJ" runat="server" CssClass="TextBox" 
                                                ValidationGroup="Input" Height="16px" Width="91px" />
                                        </td>
                                        <td>
                                            <asp:TextBox ID="tbTphToPksJ" runat="server" CssClass="TextBox" 
                                                ValidationGroup="Input" Height="16px" Width="91px" />
                                        </td>
                                      
                                    </tr>
                                </table>
                            </td>
							
							<td>
                                BM Dt</td>
                            <td>
                                : </td>
                            <td>
                                <table cellpadding="0" cellspacing="0">
                                    <tr style="background-color:Silver;text-align:center">
                                        <td>
                                           LT to PKS</td>
                                        <td>
                                            TPH to PKS</td>                                    
                                       
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="tbLtToPksDt" runat="server" CssClass="TextBox" 
                                                ValidationGroup="Input" Height="16px" Width="91px" />
                                        </td>
                                        <td>
                                            <asp:TextBox ID="tbTphToPksDt" runat="server" CssClass="TextBox" 
                                                ValidationGroup="Input" Height="16px" Width="91px" />
                                        </td>
                                      
                                    </tr>
                                </table>
                            </td>
							
                        </tr>
						
						<tr>
                            <td>
                                Operator Jonder</td>
                            <td>
                                : </td>
                            <td>
                                <table cellpadding="0" cellspacing="0">
                                    <tr style="background-color:Silver;text-align:center">
                                        <td>
                                            TPH to LT</td>
                                        <td>
                                            TPH to PKS</td>                                    
                                       
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="tbOPTphToLT" runat="server" CssClass="TextBox" 
                                                ValidationGroup="Input" Height="16px" Width="91px" />
                                        </td>
                                        <td>
                                            <asp:TextBox ID="tbOPTphToPks" runat="server" CssClass="TextBox" 
                                                ValidationGroup="Input" Height="16px" Width="91px" />
                                        </td>
                                      
                                    </tr>
                                </table>
                            </td>
							
							
							<td>
                                Operator DT </td>
                            <td>
                                : </td>
                            <td>
                                <table cellpadding="0" cellspacing="0">
                                    <tr style="background-color:Silver;text-align:center">
                                        <td>
                                           LT to PKS</td>
                                        <td>
                                            TPH to PKS</td>                                    
                                       
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="tbOPLTtoPksDt" runat="server" CssClass="TextBox" 
                                                ValidationGroup="Input" Height="16px" Width="91px" />
                                        </td>
                                        <td>
                                            <asp:TextBox ID="tbOPTphToPKSDt" runat="server" CssClass="TextBox" 
                                                ValidationGroup="Input" Height="16px" Width="91px" />
                                        </td>
                                      
                                    </tr>
                                </table>
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
