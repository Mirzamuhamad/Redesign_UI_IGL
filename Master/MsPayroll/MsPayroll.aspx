<%@ Page Language="VB" AutoEventWireup="false" CodeFile="MsPayroll.aspx.vb" Inherits="MsPayroll_MsPayroll" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Payroll File</title>
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
    <style type="text/css">
        .style4
        {
            width: 3px;
        }
        .style6
        {
            width: 145px;
        }
        .style7
        {
            width: 121px;
        }
        .style8
        {
            width: 95px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div class="Content">
     <div class="H1">Payroll File</div>
     <hr style="color:Blue" />
     <asp:Panel runat="server" ID="pnlHd">
      <table>
        <tr>
            <td style="width:100px;text-align:right" >Quick Search :</td>
            <td><asp:TextBox runat="server" ID ="tbFilter" CssClass="TextBox" /> 
                <asp:DropDownList runat="server" ID="ddlField" CssClass="DropDownList" > 
                    <asp:ListItem Selected="true" Text="Payroll Code" Value="PayrollCode"></asp:ListItem>
                    <asp:ListItem Text="Payroll Name" Value="PayrollName"></asp:ListItem>                   
                    <asp:ListItem Text="Formula Code" Value="Formula"></asp:ListItem>
                    <asp:ListItem Text="Formula Name" Value="FormulaName"></asp:ListItem>
                    <asp:ListItem Text="Formula Desc" Value="FormulaDesc"></asp:ListItem>
                    <asp:ListItem Text="Payroll Type Code" Value="PayrollType"></asp:ListItem>
                    <asp:ListItem Text="Payroll Type Name" Value="PayrollTypeName"></asp:ListItem>
                    <asp:ListItem Text="Include PPh" Value="FgPPh"></asp:ListItem>
                    <asp:ListItem Text="Payroll Group By" Value="GroupBy"></asp:ListItem>
                    <asp:ListItem Text="Priority In Slip" Value="Prioritas"></asp:ListItem>
                    <asp:ListItem Text="Responsibility PPH By" Value="Tertanggung"></asp:ListItem>
                    <asp:ListItem Text="Rapel" Value="FgRapel"></asp:ListItem>
                    <asp:ListItem Text="Show In Slip" Value="FgSlip"></asp:ListItem>                  
                    <asp:ListItem Text="Payroll Value" Value="FgValue"></asp:ListItem>
                    <asp:ListItem Text="Annual" Value="FgAnnual"></asp:ListItem>
                    <asp:ListItem Text="Jamsostek" Value="FgJamsostek"></asp:ListItem>
                    <asp:ListItem Text="Check Absence" Value="FgCheckAbsen"></asp:ListItem>  
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
                    <asp:ListItem Selected="true" Text="Payroll Code" Value="PayrollCode"></asp:ListItem>
                    <asp:ListItem Text="Payroll Name" Value="PayrollName"></asp:ListItem>                   
                    <asp:ListItem Text="Formula Code" Value="Formula"></asp:ListItem>
                    <asp:ListItem Text="Formula Name" Value="FormulaName"></asp:ListItem>
                    <asp:ListItem Text="Formula Desc" Value="FormulaDesc"></asp:ListItem>
                    <asp:ListItem Text="Payroll Type Code" Value="PayrollType"></asp:ListItem>
                    <asp:ListItem Text="Payroll Type Name" Value="PayrollTypeName"></asp:ListItem>
                    <asp:ListItem Text="Include PPh" Value="FgPPh"></asp:ListItem>
                    <asp:ListItem Text="Payroll Group By" Value="GroupBy"></asp:ListItem>
                    <asp:ListItem Text="Priority In Slip" Value="Prioritas"></asp:ListItem>
                    <asp:ListItem Text="Responsibility PPH By" Value="Tertanggung"></asp:ListItem>
                    <asp:ListItem Text="Rapel" Value="FgRapel"></asp:ListItem>
                    <asp:ListItem Text="Show In Slip" Value="FgSlip"></asp:ListItem>                  
                    <asp:ListItem Text="Payroll Value" Value="FgValue"></asp:ListItem>
                    <asp:ListItem Text="Annual" Value="FgAnnual"></asp:ListItem>
                    <asp:ListItem Text="Jamsostek" Value="FgJamsostek"></asp:ListItem>
                    <asp:ListItem Text="Check Absence" Value="FgCheckAbsen"></asp:ListItem>                    
                  </asp:DropDownList>
                
            </td>
        </tr>
     </table>
     </asp:Panel>
      <br />
      <asp:Button class="bitbtn btnadd" runat="server" ID="btnAdd" Text="Add" />									
      <asp:GridView id="DataGrid" runat="server" ShowFooter="True" AllowSorting="True" 
            AutoGenerateColumns="False" AllowPaging="True" CssClass="Grid">
						<HeaderStyle CssClass="GridHeader" Wrap = "false"></HeaderStyle>
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
				            <asp:BoundField DataField="PayrollCode" HeaderText="Payroll Code" HeaderStyle-Width="140" SortExpression="PayrollCode"/>
							<asp:BoundField DataField="PayrollName" HeaderText="Payroll Name" HeaderStyle-Width="300" SortExpression="PayrollName"/>
							<asp:BoundField DataField="Formula" HeaderText="Formula Code" SortExpression="Formula"/>
							<asp:BoundField DataField="FormulaName" HeaderText="Formula Name" SortExpression="FormulaName"/>
							<asp:BoundField DataField="FormulaDesc" HeaderText="Formula Desc" HeaderStyle-Width="220" SortExpression="FormulaDesc"/>
							<asp:BoundField DataField="PayrollType" HeaderText="Payroll Type Code" HeaderStyle-Width="100" SortExpression="PayrollType" />						
							<asp:BoundField DataField="PayrollTypeName" HeaderText="Payroll Type Name" HeaderStyle-Width="140" SortExpression="PayrollTypeName" />						
							<asp:BoundField DataField="FgPPh" HeaderText="Include PPh" HeaderStyle-Width="100" SortExpression="FgPPh" />							
							<%--<asp:BoundField DataField="Amount" HeaderText="Amount" SortExpression="Amount" ItemStyle-HorizontalAlign = "Right"/>--%>
							<asp:BoundField DataField="GroupBy" HeaderText="Payroll Group By" HeaderStyle-Width="150" SortExpression="GroupBy"/>
							<asp:BoundField DataField="Prioritas" HeaderText="Priority In Slip" HeaderStyle-Width="80" SortExpression="Prioritas" ItemStyle-HorizontalAlign = "Right"/>
							<asp:BoundField DataField="Tertanggung" HeaderText="Responsibility PPH By" HeaderStyle-Width="100" SortExpression="Tertanggung"/>																					
							<asp:BoundField DataField="FgRapel" HeaderText="Rapel" HeaderStyle-Width="100" SortExpression="FgRapel"/>
							<asp:BoundField DataField="FgSlip" HeaderText="Show In Slip" HeaderStyle-Width="100" SortExpression="FgSlip"/>
							<asp:BoundField DataField="FgValue" HeaderText="Payroll Value" HeaderStyle-Width="100" SortExpression="FgValue"/>
							<asp:BoundField DataField="FgAnnual" HeaderText="Annual" HeaderStyle-Width="100" SortExpression="FgAnnual"/>
							<asp:BoundField DataField="FgJamsostek" HeaderText="Jamsostek" HeaderStyle-Width="100" SortExpression="FgJamsostek"/>
							<asp:BoundField DataField="FgCheckAbsen" HeaderText="Absence" HeaderStyle-Width="100" SortExpression="FgCheckAbsen"/>							
    					</Columns>
        </asp:GridView>
        <asp:Button class="bitbtn btnadd" runat="server" ID="btnAdd2" Text="Add" />									
      </asp:Panel>
      <asp:Panel runat="server" ID="pnlInput" Visible="false">
        <table style="margin-right: 93px; width: 712px;">
            <tr>
                <td class="style7">Payroll Code</td>
                <td>:</td>
                <td>
                    <asp:Label ID="lblType" runat="server" />
                    <asp:TextBox runat="server" MaxLength="5" CssClass="TextBox" ID="tbNoView" 
                        Enabled = "false" Width="75px"/>
                    <asp:TextBox runat="server" MaxLength="4" CssClass="TextBox" ID="tbNo" 
                        Width="75px" /></td>
                <td class="style8">&nbsp;</td>
                <td class="style4">&nbsp;</td>
                <td class="style6">&nbsp;</td>
            </tr>
            <tr>
                <td class="style7">Payroll Name</td>
                <td>:</td>
                <td><asp:TextBox runat="server" MaxLength="50" CssClass="TextBox" ID="tbName" Width="250px"/></td>
                <td class="style8">&nbsp;</td>
                <td class="style4">&nbsp;</td>
                <td class="style6">&nbsp;</td>
            </tr>
            <tr>
                <td class="style7">Formula</td>
                <td>:</td>
                <td><asp:DropDownList runat="server" CssClass="DropDownList" ID="ddlFormula" 
                        Width="160px" AutoPostBack="True">                        
                    </asp:DropDownList>                    
                </td>
                <td class="style8">&nbsp;</td>
                <td class="style4">&nbsp;</td>
                <td class="style6">&nbsp;</td>
            </tr>
            <tr>
                <td class="style7">Formula Desc</td>
                <td>:</td>
                <td><asp:Label runat="server" ID="lbFormulaDesc"/></td>
                <td class="style8">&nbsp;</td>
                <td class="style4">&nbsp;</td>
                <td class="style6">&nbsp;</td>
            </tr>
            <tr>
                <td class="style7">Payroll Type</td>
                <td>:</td>
                <td><asp:DropDownList runat="server" CssClass="DropDownList" ID="ddlPayrolType" 
                        Width="77px" Height="16px" AutoPostBack="True">                    
                    <asp:ListItem Selected="True">GP</asp:ListItem>
                    <asp:ListItem>TT</asp:ListItem>
                    <asp:ListItem>TTT</asp:ListItem>
                    <asp:ListItem>POT</asp:ListItem>
                    <asp:ListItem>PPH</asp:ListItem>
                    <asp:ListItem>RAPEL</asp:ListItem>
                    <asp:ListItem>PHK</asp:ListItem>
                    </asp:DropDownList>
                    &nbsp;
                    <asp:Label runat="server" ID="lbPayrollTypeName"/>
                </td>
                <td class="style8">&nbsp;</td>
                <td class="style4">&nbsp;</td>
                <td class="style6">&nbsp;</td>
            </tr>
            <tr>
                <td class="style7">Include PPh</td>
                <td>:</td>
                <td><asp:DropDownList runat="server" CssClass="DropDownList" ID="ddlPPh" AutoPostBack="true" Width="39px" Height="16px">
                    <asp:ListItem Selected="True">Y</asp:ListItem>
                    <asp:ListItem>N</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td class="style8">&nbsp;</td>
                <td class="style4">&nbsp;</td>
                <td class="style6">&nbsp;</td>
            </tr>
            <tr>
                <td class="style7">Payroll Group By</td>
                <td>:</td>
                <td><asp:DropDownList runat="server" CssClass="DropDownList" ID="ddlGroupBy" 
                        Width="104px" Height="17px">                        
                    <asp:ListItem>All</asp:ListItem>
                    <asp:ListItem>Employee</asp:ListItem>
                    <asp:ListItem>Job Title</asp:ListItem>
                    <asp:ListItem>Job Level</asp:ListItem>
                    <asp:ListItem>Status Emp</asp:ListItem>
                    <asp:ListItem>Work Place</asp:ListItem>
                    <asp:ListItem>Marital Status</asp:ListItem>
                    </asp:DropDownList>                    
                </td>
                <td class="style8">Priority In Slip</td>
                <td class="style4">:</td>
                <td class="style6">
                    <asp:TextBox ID="tbPriority" runat="server" CssClass="TextBox" 
                        MaxLength="2" Width="51px" AutoPostBack="True" />
                    <asp:Label ID="lb1" runat="server" Text = "[1...99]"/>
                </td>
            </tr>
            <tr>
                <td class="style7">Responsibility PPH By</td>
                <td>:</td>
                <td>
                    <asp:DropDownList ID="ddlResponsibility" runat="server" CssClass="DropDownList" Height="17px" Width="104px">
                        <asp:ListItem>Employee</asp:ListItem>
                        <asp:ListItem>Company</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td class="style8">Rapel</td>
                <td class="style4">:</td>
                <td class="style6"><asp:DropDownList ID="ddlRapel" runat="server" AutoPostBack="true" CssClass="DropDownList" Height="16px" Width="39px">
                        <asp:ListItem Selected="True">Y</asp:ListItem>
                        <asp:ListItem>N</asp:ListItem>
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
                <td class="style8">
                    Payroll Value</td>
                <td class="style4">
                    :</td>
                <td class="style6">
                    <asp:DropDownList ID="ddlValue" runat="server" AutoPostBack="true" 
                        CssClass="DropDownList" Height="16px" Width="39px">
                        <asp:ListItem Selected="True">1</asp:ListItem>
                        <asp:ListItem>-1</asp:ListItem>
                        <asp:ListItem>0</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="style7">Annual</td>
                <td>:</td>
                <td><asp:DropDownList ID="ddlAnnual" runat="server" CssClass="DropDownList">
                    <asp:ListItem Selected="True">Y</asp:ListItem>
                    <asp:ListItem>N</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td class="style8">Jamsostek</td>
                <td class="style4">:</td>
                <td class="style6"><asp:DropDownList ID="ddlJamsostek" runat="server" CssClass="DropDownList">
                    <asp:ListItem Selected="True">Y</asp:ListItem>
                    <asp:ListItem>N</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="style7">Absence</td>
                <td>:</td>
                <td><asp:DropDownList ID="ddlAbsen" runat="server" CssClass="DropDownList">
                        <asp:ListItem Selected="True">Y</asp:ListItem>
                        <asp:ListItem>N</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td class="style8">
                    <asp:Label ID="lbCurr0" runat="server" Visible="False">Amount</asp:Label>
                </td>
                <td class="style4">&nbsp;</td>
                <td class="style6">
                    <asp:TextBox ID="tbAmount" runat="server" CssClass="TextBox" Visible="False" 
                        Width="103px" />
                    <asp:Label ID="lbCurr" runat="server" Visible="False" />
                </td>
            </tr>
            <tr>
                <td class="style7">&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td class="style8">&nbsp;</td>
                <td class="style4">&nbsp;</td>
                <td class="style6">&nbsp;</td>
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
                <td align="center" class="style8">
                    &nbsp;</td>
                <td align="center" class="style4">
                    &nbsp;</td>
                <td align="center" class="style6">
                    &nbsp;</td>
            </tr>
        </table>
      </asp:Panel>              
     <asp:Label ID="lstatus" ForeColor="red" runat="server"></asp:Label>        
    </div>
    </form>
</body>
</html>
