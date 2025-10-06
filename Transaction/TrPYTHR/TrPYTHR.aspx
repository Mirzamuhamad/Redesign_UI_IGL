<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TrPYTHR.aspx.vb" Inherits="Transaction_TrPYTHR_TrPYTHR" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="BasicFrame.WebControls.BasicDatePicker" Namespace="BasicFrame.WebControls"
    TagPrefix="BDP" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Untitled Page</title>
    <script src="../../Function/OpenDlg.js" type="text/javascript"></script>
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
    
        function setformatdt()
        {
         try
         {


             document.getElementById("tbTotalGP").value = setdigit(document.getElementById("tbTotalGP").value.replace(/\$|\,/g, ""), '<%=VIEWSTATE("DigitQty")%>');
            document.getElementById("tbTotalTT").value = setdigit(document.getElementById("tbTotalTT").value.replace(/\$|\,/g, ""), '<%=VIEWSTATE("DigitQty")%>');
            document.getElementById("tbMasa").value = setdigit(document.getElementById("tbMasa").value.replace(/\$|\,/g, ""), '<%=VIEWSTATE("DigitQty")%>');
            document.getElementById("tbFormula").value = setdigit(document.getElementById("tbFormula").value.replace(/\$|\,/g, ""), '<%=VIEWSTATE("DigitQty")%>');
            document.getElementById("tbTHR").value = setdigit(document.getElementById("tbTHR").value.replace(/\$|\,/g, ""), '<%=VIEWSTATE("DigitQty")%>');
            
            
        }catch (err){
            alert(err.description);
          }      
        }
    

    
    </script>    
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
 <link href="https://fonts.googleapis.com/css2?family=Muli:wght@300;400;600&display=swap" rel="stylesheet">
</head>
<body>
    <form id="form1" runat="server">
    <div class="Content">
        <div class="H1">Tunjangan Hari Raya (THR)</div>
        <hr style="color:Blue" />
        <asp:Panel runat="server" id="PnlHd"> 
      <table>
        <tr>
            <td style="width:100px;text-align:right">Quick Search :</td>            
            <td><asp:TextBox CssClass="TextBox" runat="server" ID ="tbFilter"/> 
                  <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlField" >
                      <asp:ListItem Selected="True" Value="Tahun">Year</asp:ListItem>
                      <asp:ListItem Value="dbo.FormatDate(ProcessDate)">Process Date</asp:ListItem>
                      <asp:ListItem Value="MethodTHRName">Method THR</asp:ListItem>                  
                      <asp:ListItem Value="MethodName">Method</asp:ListItem>                  
                      <asp:ListItem Value="EmpNumb">Employee</asp:ListItem>                  
                      <asp:ListItem Value="EmpName">Employee Name</asp:ListItem>                  
                      <asp:ListItem>Status</asp:ListItem>
                          <asp:ListItem>Variable</asp:ListItem>                     
                    </asp:DropDownList>
                  <asp:Button class="bitbtn btnsearch" runat="server" ID="btnSearch" Text="Search" />                  
                  <asp:Button class="btngo" runat="server" ID="btnExpand" Text="..."/>  
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
      </table>
      <asp:Panel runat="server" ID="pnlSearch" Visible="false">
      <table>
        <tr>
          <td style="width:100px;text-align:right">
              <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlNotasi" >
                <asp:ListItem Selected="true" Text="OR" Value="OR"></asp:ListItem>
                <asp:ListItem Text="AND" Value="AND"></asp:ListItem>            
              </asp:DropDownList>
          </td>
          <td>
           
              <asp:TextBox runat="server" CssClass="TextBox" ID ="tbfilter2"/> 
              <asp:DropDownList runat="server" CssClass="DropDownList" ID="ddlField2" >
                  <asp:ListItem Selected="True" Value="Tahun">Year</asp:ListItem>
                  <asp:ListItem Value="dbo.FormatDate(ProcessDate)">Process Date</asp:ListItem>
                  <asp:ListItem Value="MethodTHRName">Method THR</asp:ListItem>                  
                  <asp:ListItem Value="MethodName">Method</asp:ListItem>                  
                  <asp:ListItem Value="EmpNumb">Employee</asp:ListItem>                  
                  <asp:ListItem Value="EmpName">Employee Name</asp:ListItem>                  
                  <asp:ListItem>Status</asp:ListItem>
                  <asp:ListItem>Variable</asp:ListItem>
              </asp:DropDownList>
          </td>              
        </tr>        
      </table>      
      </asp:Panel>
          <br />
            <asp:Button class="bitbtn btnadd" runat="server" ID="BtnAdd" Text="Add" />
            
            &nbsp &nbsp &nbsp   
            <asp:DropDownList CssClass="DropDownList" ID="ddlCommand" runat="server" Visible="false"/>
            <asp:Button class="btngo" Visible="false" runat="server" ID="BtnGo" Text="G"/>
          <br />
          <div style="border:0px  solid; width:100%; height:100%; overflow:auto;">
          <asp:GridView ID="GridView1" runat="server" AllowPaging="True" AllowSorting="true"
            CssClass="Grid" AutoGenerateColumns="false"> 
              <HeaderStyle CssClass="GridHeader"></HeaderStyle>
						<RowStyle CssClass="GridItem" Wrap="false" />
						<AlternatingRowStyle CssClass="GridAltItem"/>
						<PagerStyle CssClass="GridPager" />
              <Columns>              
                  <asp:TemplateField>
                      <HeaderTemplate>
                          <asp:CheckBox ID="cbSelectHd" runat="server" AutoPostBack="true"
                          oncheckedchanged="cbSelectHd_CheckedChanged" />
                      </HeaderTemplate>
                      <ItemTemplate>
                          <asp:CheckBox ID="cbSelect" runat="server" />
                      </ItemTemplate>
                  </asp:TemplateField>
                  <asp:TemplateField HeaderStyle-Width="110">
                      <ItemTemplate>
                          <asp:DropDownList CssClass="DropDownList" ID="ddl" runat="server">
                              <asp:ListItem Selected="True" Text="View" />
                              <asp:ListItem Text="Edit" />
                              <asp:ListItem Text="Print" />
                          </asp:DropDownList>
                          <asp:Button class="btngo" runat="server" ID="BtnGo" Text="G" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" CommandName="Go" />
                      </ItemTemplate>
                      <HeaderStyle Width="110px" />
                  </asp:TemplateField>
                  <asp:BoundField DataField="Tahun" SortExpression="Tahun" HeaderText="Year"></asp:BoundField>
                  <asp:BoundField DataField="ProcessDate" HtmlEncode="true" DataFormatString="{0:dd MMM yyyy}" SortExpression="ProcessDate" HeaderText="Process Date"></asp:BoundField>
                  <asp:BoundField DataField="MethodTHR" SortExpression="MethodTHR" HeaderText="Method THR Code"></asp:BoundField>
                  <asp:BoundField DataField="MethodTHRName" SortExpression="MethodTHRName" HeaderText="Method THR Name"></asp:BoundField>
                  <asp:BoundField DataField="Method" SortExpression="Method" HeaderText="Method Code"></asp:BoundField>
                  <asp:BoundField DataField="MethodName" SortExpression="MethodName" HeaderText="Method Name"></asp:BoundField>
                  <asp:BoundField DataField="Status" HeaderText="Status"></asp:BoundField>                  
                  <asp:BoundField DataField="Variable" DataFormatString="{0:#,##0.##}" ItemStyle-HorizontalAlign="Right" SortExpression="Variable" HeaderText="Variable" />
                  <asp:BoundField DataField="EmpNumb" SortExpression="EmpNumb" HeaderText="Employee No"></asp:BoundField>
                  <asp:BoundField DataField="EmpName" SortExpression="EmpName" HeaderText="Employee Name"></asp:BoundField>
                  
              </Columns>
          </asp:GridView>
          </div>
            <asp:Panel runat="server" ID ="pnlNav" Visible="false">
            <asp:Button class="bitbtn btnadd" runat="server" ID="btnAdd2" Text="Add" />
            &nbsp &nbsp &nbsp
            <asp:DropDownList ID="ddlCommand2" CssClass="DropDownList" runat="server"/>
            <asp:Button class="btngo" runat="server" ID="btnGo2" Text="G"/>  
            </asp:Panel>
    </asp:Panel>    
    <asp:Panel runat="server" ID="pnlInput" Visible="false">
      <table>
        <tr>
            <td>Year</td>
            <td>:</td>
            <td><asp:DropDownList CssClass="DropDownList" runat="server" ValidationGroup="Input" ID="ddlYear" /></td>           
            
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>           
            
        </tr>              
        
        <tr>
            <td>Process Date</td>
            <td>:</td>
            <td >
                <BDP:BasicDatePicker ID="tbProcessDate" runat="server" ButtonImageHeight="19px" 
                    ButtonImageWidth="20px" DateFormat="dd MMM yyyy" DisplayType="TextBoxAndImage" 
                    ReadOnly="true" ShowNoneButton="false" TextBoxStyle-CssClass="TextDate" 
                    ValidationGroup="Input">
                    <TextBoxStyle CssClass="TextDate" />
                </BDP:BasicDatePicker>
                <asp:Label ID="Label4" runat="server" ForeColor="#FF3300" Text="*"></asp:Label>
            </td>
            <td>Variable</td>
            <td>:</td>
            <td >
                <asp:TextBox ID="tbVariable" runat="server" CssClass="TextBox" MaxLength="10" 
                    ValidationGroup="Input" Width="80px" />
                <asp:Label ID="Label7" runat="server" ForeColor="#FF3300" Text="*"></asp:Label>
            </td>
        </tr>
          <tr>
              <td>
                  Method THR</td>
              <td>
                  :</td>
              <td>
                  <asp:DropDownList ID="ddlMethodTHR" runat="server" CssClass="DropDownList" 
                      Height="16px" ValidationGroup="Input" Width="250px" />
                  <asp:Label ID="Label5" runat="server" ForeColor="#FF3300" Text="*"></asp:Label>
              </td>
              <td>
                  Method</td>
              <td>
                  :</td>
              <td>
                  <asp:DropDownList ID="ddlMethod" runat="server" CssClass="DropDownList" 
                      Height="16px" ValidationGroup="Input" Width="250px" />
                  <asp:Label ID="Label6" runat="server" ForeColor="#FF3300" Text="*"></asp:Label>
                  <asp:Button ID="btnGetData" runat="server" class="bitbtn btngetitem" 
                      Text="Calculate THR" />
              </td>
          </tr>
      </table>  
      
      <br />      
      <div style="font-size:medium; color:Blue;">Detail</div>
      <hr style="color:Blue" />  
        <asp:Panel runat="server" ID="pnlDt">
            <asp:Button class="bitbtndt btnadd" runat="server" ID="btnAdddt" Text="Add" ValidationGroup="Input" />                 
            <div style="border:0px  solid; overflow:auto;">
                <asp:GridView ID="GridDt" runat="server" AutoGenerateColumns="false" 
                    ShowFooter="True">
                    <HeaderStyle CssClass="GridHeader" />
                    <RowStyle CssClass="GridItem" Wrap="false" />
                    <AlternatingRowStyle CssClass="GridAltItem" />
                    <PagerStyle CssClass="GridPager" />
                    <Columns>
                        <asp:TemplateField HeaderText="Action">
                            <ItemTemplate>
                                <asp:Button ID="btnEdit" runat="server" class="bitbtndt btnedit" Text="Edit" CommandName="Edit" />
                                <asp:Button ID="btnDelete" runat="server" class="bitbtndt btndelete" Text="Delete"
                                        OnClientClick="return confirm('Sure to delete this data?');" CommandName="Delete" />       
                            </ItemTemplate>                                                                     
                        </asp:TemplateField>
                                               
                        <asp:BoundField DataField="EmpNumb" HeaderStyle-Width="120px" HeaderText="Emp No" />
                        <asp:BoundField DataField="EmpName" HeaderText="Employee Name" HeaderStyle-Width="200px" />
                        <asp:BoundField DataField="HireDate" HtmlEncode="true" DataFormatString="{0:dd MMM yyyy}" SortExpression="HireDate" HeaderText="Hire Date"></asp:BoundField>
                        <asp:BoundField DataField="JobTitle" HeaderText="JobTitle" HeaderStyle-Width="200px" />
                        <asp:BoundField DataField="Currency" HeaderStyle-Width="80px" HeaderText="Currency" />
                        <asp:BoundField DataField="TotalGP" DataFormatString="{0:#,##0.##}" ItemStyle-HorizontalAlign="Right" SortExpression="Variable" HeaderText="Total GP" />
                        <asp:BoundField DataField="TotalTT" DataFormatString="{0:#,##0.##}" ItemStyle-HorizontalAlign="Right" SortExpression="Variable" HeaderText="Total TT" />
                        <asp:BoundField DataField="MasaKerja" DataFormatString="{0:#,##0.##}" ItemStyle-HorizontalAlign="Right" SortExpression="Variable" HeaderText="Masa Kerja" />
                        <asp:BoundField DataField="Formula" DataFormatString="{0:#,##0.##}" ItemStyle-HorizontalAlign="Right" SortExpression="Variable" HeaderText="Formula" />
                        <asp:BoundField DataField="TotalTHR" DataFormatString="{0:#,##0.##}" ItemStyle-HorizontalAlign="Right" SortExpression="Variable" HeaderText="Total THR" />
                        <asp:BoundField DataField="TotalPaid" DataFormatString="{0:#,##0.##}" ItemStyle-HorizontalAlign="Right" SortExpression="Variable" HeaderText="Total Paid" />
                        <asp:BoundField DataField="TotalAdjust" DataFormatString="{0:#,##0.##}" ItemStyle-HorizontalAlign="Right" SortExpression="Variable" HeaderText="Total Adjust" />
                        <asp:BoundField DataField="Method" HeaderStyle-Width="80px" HeaderText="Method" />
                        <asp:BoundField DataField="xPeriod" HeaderStyle-Width="80px" HeaderText="x Period" />
                        <asp:BoundField DataField="ProcessCode" HeaderStyle-Width="80px" HeaderText="Last Salary" />
                    </Columns>
                </asp:GridView>
          </div>   
          <asp:Button class="bitbtndt btnadd" runat="server" ID="btnAddDt2" Text="Add" ValidationGroup="Input" />                  
       </asp:Panel>             
       <asp:Panel runat="server" DefaultButton="btnSaveDt" ID="pnlEditDt" Visible="false">
            <table>              
                <tr>
                    <td>Employee</td>
                    <td>:</td>
                    <td colspan="4"><asp:TextBox runat="server" ID="tbEmp" CssClass="TextBox" AutoPostBack="true" />
                        <asp:TextBox runat="server"  CssClass="TextBox"
                            ID="tbempName" EnableTheming="True" ReadOnly="True" Enabled="False" 
                            Width="200px"/>
                        <asp:Button ID="btnEmp" runat="server" class="btngo" Text="..."/>
                        
                    </td>
                </tr>     
                <tr>
                    <td>Job Title</td>
                    <td>:</td>
                    <td colspan="4"><asp:DropDownList CssClass="DropDownList" Enabled="false"  
                            runat="server" ValidationGroup="Input" ID="ddlJobTitle" Height="16px" 
                            Width="344px"/></td>                    
                </tr>                                    
                <tr>
                    <td>Hire Date</td>
                    <td>:</td>
                    <td>
                        <BDP:BasicDatePicker ID="tbHireDate" runat="server" ButtonImageHeight="19px" 
                            ButtonImageWidth="20px" DateFormat="dd MMM yyyy" DisplayType="TextBoxAndImage" 
                            Enabled="False" ReadOnly="true" ShowNoneButton="false" 
                            TextBoxStyle-CssClass="TextDate" ValidationGroup="Input">
                            <TextBoxStyle CssClass="TextDate" />
                        </BDP:BasicDatePicker>
                    </td>                    
                    <td>
                        Currency</td>
                    <td>
                        :</td>
                    <td>
                        <asp:DropDownList ID="ddlCurr" runat="server" CssClass="DropDownList" 
                            Enabled="false" Height="16px" ValidationGroup="Input" Width="90px" />
                    </td>
                </tr>                
                <tr>
                    <td>Total</td>
                    <td>:</td>
                    <td colspan="4">
                        <table cellspacing="0" cellpadding="0">
                            <tr style="background-color:Silver;text-align:center">
                                <td>Gaji Pokok</td>
                                <td>Tunjangan Tetap</td>
                                <td>Masa Kerja</td>
                                                             
                                <td>
                                    Formula</td>
                                                             
                            </tr>
                            <tr>
                                <td><asp:TextBox CssClass="TextBoxR" Enabled="False" runat="server" ID="tbTotalGP"/></td>
                                <td><asp:TextBox CssClass="TextBoxR" Enabled="false"  runat="server" 
                                        ID="tbTotalTT"/></td>
                                <td><asp:TextBox CssClass="TextBoxR" runat="server" ID="tbMasa" Enabled="False"/></td>                                
                                <td>
                                    <asp:TextBox ID="tbFormula" runat="server" CssClass="TextBoxR" Enabled="False"/>
                                </td>
                            </tr>
                        </table>                    
                    </td>
                </tr>
                                    
                <tr>
                    <td>
                        &nbsp;</td>
                    <td>
                        &nbsp;</td>
                    <td colspan="4">
                        <table cellspacing="0" cellpadding="0">
                            <tr style="background-color:Silver;text-align:center">
                                <td>
                                    THR</td>
                                <td>
                                    Paid</td>
                                <td>
                                    Adjust</td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:TextBox ID="tbTHR" runat="server" CssClass="TextBoxR" Enabled="False" />
                                </td>
                                <td>
                                    <asp:TextBox ID="tbPaid" runat="server" CssClass="TextBox" />
                                </td>
                                <td>
                                    <asp:TextBox ID="tbadjust" runat="server" CssClass="TextBoxR" Enabled="False" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        Method</td>
                    <td>
                        :</td>
                    <td colspan="4">
                        <asp:DropDownList ID="ddlM" runat="server" CssClass="DropDownList" 
                            Enabled="False" Height="17px" ValidationGroup="Input" Width="204px" />
                    </td>
                </tr>
                <tr>
                    <td>
                        X Period</td>
                    <td>
                        :</td>
                    <td colspan="4">
                        <asp:TextBox ID="tbXperiod" runat="server" CssClass="TextBoxR" Enabled="False" />
                    </td>
                </tr>
                <tr>
                    <td>
                        Last Salary</td>
                    <td>
                        :</td>
                    <td colspan="4">
                        <asp:TextBox ID="tbLastSalary" runat="server" CssClass="TextBoxR" 
                            Enabled="False" />
                    </td>
                </tr>
                                    
            </table>
            <br />                     
            <asp:Button ID="btnSaveDt" runat="server" class="bitbtndt btnsave" Text="Save"/> &nbsp;         
            <asp:Button ID="btnCancelDt" runat="server" class="bitbtndt btncancel" Text="Cancel"/> 
       </asp:Panel> 
       
       <br />          
       <asp:Button ID="btnSaveAll" runat="server" class="bitbtndt btnsave" 
            Text="Save & New" ValidationGroup="Input" Width="97px"/> &nbsp;    
        <asp:Button ID="btnSaveTrans" runat="server" class="bitbtndt btnsave" Text="Save" ValidationGroup="Input"/> &nbsp;    
        <asp:Button ID="btnBack" runat="server" class="bitbtndt btncancel" Text="Cancel" ValidationGroup="Input"/>  &nbsp;
        <asp:Button class="btngo" runat="server" ID="btnHome" Text="Home" Width="45px"/>  
    </asp:Panel>
    
    </div>     
    <asp:Label runat ="server" ID="lbStatus" ForeColor="Red"/>
    </form>
</body>
</html>
