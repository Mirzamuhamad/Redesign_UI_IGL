<%@ Page Language="VB" AutoEventWireup="false" CodeFile="MsBenefit.aspx.vb" Inherits="MsBenefit_MsBenefit" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Benefit File</title>
    <script src="../../Function/OpenDlg.JS" type="text/javascript"></script>
    <script src="../../Function/Function.JS" type="text/javascript"></script>
    <script type="text/javascript">         
        function setdigit(nStr, digit)
        {
        try
        {
        var TNstr = parseFloat(nStr);        
        if ( parseFloat(digit) >= 0) 
        {     
           TNstr = TNstr.toFixed(digit);                
        } 
        nStr = TNstr;        
	    nStr += '';
	    x = nStr.split('.');
	    x1 = x[0];
	    x2 = x.length > 1 ? '.' + x[1] : '';
	    var rgx = /(\d+)(\d{3})/;
	    while (rgx.test(x1)) {
		    x1 = x1.replace(rgx, '$1' + ',' + '$2');
	    }
	    return x1 + x2;
	    }catch (err){
            alert(err.description);
          }  
        }
    
        function setformat()
        {
        //var Rate = document.getElementById("tbRate");
        //var DrForex = document.getElementById("tbDebitForex");
        //var CrForex = document.getElementById("tbCreditForex");
        
         try
         {           
        document.getElementById("tbAmount").value = setdigit(document.getElementById("tbAmount").value.replace(/\$|\,/g,""),'<%=VIEWSTATE("DigitCurr")%>');
        }catch (err){
            alert(err.description);
          }      
        }   
    </script>
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
 <link href="https://fonts.googleapis.com/css2?family=Muli:wght@300;400;600&display=swap" rel="stylesheet">
    <style type="text/css">
        .style7
        {
            width: 109px;
        }
        </style>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div class="Content">
     <div class="H1">Benefit File</div>
     <hr style="color:Blue" />
     <asp:Panel runat="server" ID="pnlHd">
      <table>
        <tr>
            <td style="width:100px;text-align:right" >Quick Search :</td>
            <td><asp:TextBox runat="server" ID ="tbFilter" CssClass="TextBox" /> 
                <asp:DropDownList runat="server" ID="ddlField" CssClass="DropDownList" > 
                    <asp:ListItem Selected="true" Text="Benefit Code" Value="PayrollCode"></asp:ListItem>
                    <asp:ListItem Text="Benefit Name" Value="PayrollName"></asp:ListItem>                   
                    <asp:ListItem Text="Formula Code" Value="Formula"></asp:ListItem>
                    <asp:ListItem Text="Formula Name" Value="FormulaName"></asp:ListItem>
                    <asp:ListItem Text="Include PPh" Value="FgPPh"></asp:ListItem>
                    <asp:ListItem Text="Amount" Value="Amount"></asp:ListItem>
                    <asp:ListItem Text="Priority" Value="Prioritas"></asp:ListItem>
                    <asp:ListItem Text="Responsibility By" Value="Tertanggung"></asp:ListItem>
                    <asp:ListItem Text="Show In Slip" Value="FgSlip"></asp:ListItem>
                    <asp:ListItem Text="Annual" Value="FgAnnual"></asp:ListItem>
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
                    <asp:ListItem Selected="true" Text="Benefit Code" Value="PayrollCode"></asp:ListItem>
                    <asp:ListItem Text="Benefit Name" Value="PayrollName"></asp:ListItem>                   
                    <asp:ListItem Text="Formula Code" Value="Formula"></asp:ListItem>
                    <asp:ListItem Text="Formula Name" Value="FormulaName"></asp:ListItem>
                    <asp:ListItem Text="Include PPh" Value="FgPPh"></asp:ListItem>
                    <asp:ListItem Text="Amount" Value="Amount"></asp:ListItem>
                    <asp:ListItem Text="Priority" Value="Prioritas"></asp:ListItem>
                    <asp:ListItem Text="Responsibility By" Value="Tertanggung"></asp:ListItem>
                    <asp:ListItem Text="Show In Slip" Value="FgSlip"></asp:ListItem>
                    <asp:ListItem Text="Annual" Value="FgAnnual"></asp:ListItem>
                  </asp:DropDownList>
                
            </td>
        </tr>
     </table>
     </asp:Panel>
      <br />
      <asp:Button class="bitbtn btnadd" runat="server" ID="btnAdd" Text="Add" />									
      <asp:GridView id="DataGrid" runat="server" ShowFooter="True" AllowSorting="True" 
            AutoGenerateColumns="False" AllowPaging="True" CssClass="Grid">
						<HeaderStyle CssClass="GridHeader" Wrap ="false"></HeaderStyle>
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
				            <asp:BoundField DataField="PayrollCode" HeaderText="Benefit Code" HeaderStyle-Width="140" SortExpression="PayrollCode"/>
							<asp:BoundField DataField="PayrollName" HeaderText="Benefit Name" HeaderStyle-Width="300" SortExpression="PayrollName"/>
							<asp:BoundField DataField="Formula" HeaderText="Formula Code" SortExpression="Formula"/>
							<asp:BoundField DataField="FormulaName" HeaderText="Formula Name" SortExpression="FormulaName"/>
							<asp:BoundField DataField="FgPPh" HeaderText="Include PPh" HeaderStyle-Width="100" SortExpression="FgPPh" />							
							<asp:BoundField DataField="Amount" HeaderText="Amount" SortExpression="Amount" ItemStyle-HorizontalAlign ="Right"/>
							<asp:BoundField DataField="Prioritas" HeaderText="Priority" HeaderStyle-Width="80" SortExpression="Prioritas"/>
							<asp:BoundField DataField="Tertanggung" HeaderText="Responsibility By" HeaderStyle-Width="100" SortExpression="Tertanggung"/>																					
							<asp:BoundField DataField="FgSlip" HeaderText="Show In Slip" HeaderStyle-Width="100" SortExpression="FgSlip"/>
							<asp:BoundField DataField="FgAnnual" HeaderText="Annualized Net Income" HeaderStyle-Width="100" SortExpression="FgAnnual"/>							
    					</Columns>
        </asp:GridView>
        <asp:Button class="bitbtn btnadd" runat="server" ID="btnAdd2" Text="Add" />									
      </asp:Panel>
      <asp:Panel runat="server" ID="pnlInput" Visible="false">
        <table style="margin-right: 93px; width: 694px;">
            <tr>
                <td class="style7">Benefit Code</td>
                <td>:</td>
                <td>
                    <asp:Label ID="lblType" runat="server" />
                    <asp:TextBox runat="server" MaxLength="4" CssClass="TextBox" ID="tbNo" 
                        Width="75px" />
                    <asp:TextBox ID="tbNoView" runat="server" CssClass="TextBox" MaxLength="5" 
                        Enabled = "false" Width="75px"/>
                </td>
            </tr>
            <tr>
                <td class="style7">Benefit Name</td>
                <td>:</td>
                <td><asp:TextBox runat="server" MaxLength="50" CssClass="TextBox" ID="tbName" Width="250px"/></td>
            </tr>
            <tr>
                <td class="style7">Formula</td>
                <td>:</td>
                <td><asp:DropDownList runat="server" CssClass="DropDownList" ID="ddlFormula" 
                        Width="227px" Height="16px">                        
                    </asp:DropDownList>                    
                </td>
            </tr>
            <tr>
                <td class="style7">Amount</td>
                <td>:</td>
                <td>
                    <asp:TextBox ID="tbAmount" runat="server" CssClass="TextBox" Width="103px" />
                    <asp:Label ID="lbCurr" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="style7">
                    Priority</td>
                <td>
                    :</td>
                <td>
                    <asp:TextBox ID="tbPriority" runat="server" AutoPostBack="True" 
                        CssClass="TextBox" MaxLength="2" Width="51px" />
                    <asp:Label ID="lb1" runat="server" Text="[1...99]" />
                </td>
            </tr>
            <tr>
                <td class="style7">
                    Include PPh</td>
                <td>
                    :</td>
                <td>
                    <asp:DropDownList ID="ddlPPh" runat="server" AutoPostBack="true" 
                        CssClass="DropDownList" Height="16px" Width="39px">
                        <asp:ListItem Selected="True">Y</asp:ListItem>
                        <asp:ListItem>N</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="style7">Responsibility By</td>
                <td>:</td>
                <td>
                    <asp:DropDownList ID="ddlResponsibility" runat="server" CssClass="DropDownList" Height="17px" Width="104px" Enabled="false">
                        <asp:ListItem>Employee</asp:ListItem>
                        <asp:ListItem>Company</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="style7">Show In Slip</td>
                <td>:</td>
                <td><asp:DropDownList runat="server" CssClass="DropDownList" ID="ddlSlip">
                        <asp:ListItem Selected="True">Y</asp:ListItem>
                        <asp:ListItem>N</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="style7">Annualized Net Income</td>
                <td>:</td>
                <td><asp:DropDownList ID="ddlAnnual" runat="server" CssClass="DropDownList">
                    <asp:ListItem Selected="True">Y</asp:ListItem>
                    <asp:ListItem>N</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="style7">&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td align="center" colspan="3">
                    <asp:Button ID="BtnSave" runat="server" class="bitbtndt btnsave" 
                        CommandName="Update" Text="Save" />
                    <asp:Button ID="btnCancel" runat="server" class="bitbtndt btncancel" 
                        CommandName="Cancel" Text="Cancel" />
                    <asp:Button ID="btnReset" runat="server" class="bitbtndt btncancel" 
                        CommandName="Cancel" Text="Reset" />
                </td>
            </tr>
        </table>
      </asp:Panel>              
     <asp:Label ID="lstatus" ForeColor="red" runat="server"></asp:Label>        
    </div>
    </form>
</body>
</html>
