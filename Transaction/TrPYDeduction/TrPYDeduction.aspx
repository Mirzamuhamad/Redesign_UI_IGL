<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TrPYDeduction.aspx.vb" Inherits="Transaction_TrPYDeduction_TrPYDeduction" %>
<%@ Register Assembly="BasicFrame.WebControls.BasicDatePicker" Namespace="BasicFrame.WebControls"
    TagPrefix="BDP" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Untitled Page</title>
    <script src="../../Function/OpenDlg.js" type="text/javascript"></script>
    <script src="../../Function/Function.js" type="text/javascript"></script>
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
         var AmountForex = document.getElementById("tbAmountForex").value.replace(/\$|\,/g,""); 
         var Rate = document.getElementById("tbRate").value.replace(/\$|\,/g,""); 
                          
         document.getElementById("tbRate").value = setdigit(Rate,'<%=ViewState("DigitRate")%>');
         document.getElementById("tbAmountForex").value = setdigit(AmountForex,'<%=VIEWSTATE("DigitCurr")%>');                 
        }catch (err){
            alert(err.description);
          }      
        }     
    </script>
    
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .style1
        {
        }
        .style6
        {
            width: 90px;
        }
        .style7
        {
            width: 94px;
        }
        </style>
</head>
<body>
    <form id="form1" runat="server">
    <div class="Content">
    <div class="H1">Payroll Deduction</div>
     <hr style="color:Blue" />        
    <asp:Panel runat="server" id="PnlHd"> 
      <table>
        <tr>
            <td style="width:100px;text-align:right">Quick Search :</td>         
            <td><asp:TextBox CssClass="TextBox" runat="server" ID ="tbFilter"/> 
                  <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlField" >     
                    <asp:ListItem Selected="True" Value="TransNmbr">Reference</asp:ListItem>
                    <asp:ListItem Value="dbo.FormatDate(TransDate)">Date</asp:ListItem>
                    <asp:ListItem>Status</asp:ListItem>
                    <asp:ListItem Value="Dept_Name">Organization</asp:ListItem>
                    <asp:ListItem Value="EmpNumb">Employee ID</asp:ListItem>
                    <asp:ListItem Value="EmpName">Employee Name</asp:ListItem>
                    <asp:ListItem Value="Deduction_Name">Deduction</asp:ListItem>
                    <asp:ListItem Value="Remark">Remark</asp:ListItem>
                  </asp:DropDownList>
                  <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlRange"></asp:DropDownList>
                  <asp:Button class="bitbtn btnsearch" runat="server" ID="btnSearch" Text="Search" />
                  <asp:Button class="btngo" runat="server" ID="btnExpand" Text="..."/>                  
            </td>
            <td>
                <asp:LinkButton ID="LbAdvSearch" runat="server" Text="Advanced Search" />
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
                    <asp:ListItem Selected="True" Value="TransNmbr">Reference</asp:ListItem>
                    <asp:ListItem Value="dbo.FormatDate(TransDate)">Date</asp:ListItem>
                    <asp:ListItem>Status</asp:ListItem>
                    <asp:ListItem Value="Dept_Name">Organization</asp:ListItem>
                    <asp:ListItem Value="EmpNumb">Employee ID</asp:ListItem>
                    <asp:ListItem Value="EmpName">Employee Name</asp:ListItem>
                    <asp:ListItem Value="Deduction_Name">Deduction</asp:ListItem>
                    <asp:ListItem Value="Remark">Remark</asp:ListItem>
              </asp:DropDownList>
          </td>              
        </tr>        
      </table>      
      </asp:Panel>
          <br />
           <asp:Button class="bitbtn btnadd" runat="server" ID="BtnAdd" Text="Add"/>	 
            &nbsp &nbsp &nbsp   
            <asp:DropDownList CssClass="DropDownList" ID="ddlCommand" runat="server" Visible="false"/>
            <asp:Button class="btngo" runat="server" ID="BtnGo" Text="G"/>
          <br />
          <div style="border:0px  solid; width:100%; height:100%; overflow:auto;">
          <asp:GridView ID="GridView1" runat="server" AllowPaging="True" AllowSorting="true"
            CssClass="Grid" AutoGenerateColumns="false"> 
              <HeaderStyle CssClass="GridHeader" Wrap = "false"></HeaderStyle>
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
                              <%--<asp:ListItem Text="Print" />--%>
                          </asp:DropDownList>
                          <asp:Button class="btngo" runat="server" ID="BtnGo" Text="G" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" CommandName="Go" />
                      </ItemTemplate>
                      <HeaderStyle Width="110px" />
                  </asp:TemplateField>            
                  <asp:BoundField DataField="TransNmbr" HeaderStyle-Width="120px" SortExpression="Nmbr" HeaderText="Trans No"></asp:BoundField>                  
                  <asp:BoundField DataField="Status" SortExpression="Status" HeaderText="Status"></asp:BoundField>                  
                  <asp:BoundField DataField="TransDate" dataformatstring="{0:dd MMM yyyy}" htmlencode="true" HeaderStyle-Width="80px" SortExpression="TransDate" HeaderText="Trans Date"></asp:BoundField>
                  <asp:BoundField DataField="Dept_Name" HeaderStyle-Width="250px" SortExpression="Dept_Name" HeaderText="Organization"></asp:BoundField>
                  <asp:BoundField DataField="Remark" HeaderStyle-Width="450px" SortExpression="Remark" HeaderText="Remark"></asp:BoundField>
              </Columns>
          </asp:GridView>
          </div>
            <asp:Panel runat="server" ID ="pnlNav" Visible="false">
            <asp:Button class="bitbtn btnadd" runat="server" ID="btnAdd2" Text="Add"/>	
            &nbsp &nbsp &nbsp
            <asp:DropDownList ID="ddlCommand2" CssClass="DropDownList" runat="server"/>
            <asp:Button class="btngo" runat="server" ID="btnGo2" Text="G"/>                
            </asp:Panel>
    </asp:Panel>    
    <asp:Panel runat="server" ID="pnlInput" Visible="false">
      <table style="width: 583px">
        <tr>
            <td class="style6">Reference</td>
            <td>:</td>
            <td><asp:TextBox CssClass="TextBoxR" runat="server" ID="tbCode" Width="150px" Enabled="False"/></td>                                
        </tr>         
        <tr>
            <td class="style6">Date</td>
            <td>:</td>
            <td><BDP:BasicDatePicker ID="tbDate" runat="server" DateFormat="dd MMM yyyy" 
                        ReadOnly = "true" ValidationGroup="Input"
                        ButtonImageHeight="19px" ButtonImageWidth="20px" 
                        DisplayType="TextBoxAndImage" 
                        TextBoxStyle-CssClass="TextDate" 
                        ShowNoneButton="False"><TextBoxStyle CssClass="TextDate" /></BDP:BasicDatePicker>                
            </td>   
        </tr>
          <tr>
              <td class="style6">Organization</td>
              <td>:</td>
              <td>
                  <asp:DropDownList runat="server" ValidationGroup="Input" ID="ddlDepartment" CssClass="DropDownList" Width="200px" />
                &nbsp;<asp:Label ID="lbred" runat="server" ForeColor="Red">*</asp:Label>               
              </td>
          </tr>
          <tr>
              <td class="style6">Remark</td>
              <td>:</td>
              <td><asp:TextBox ID="tbRemark" runat="server" CssClass="TextBox" MaxLength="50" ValidationGroup="Input" Width="225px" />
              </td>
          </tr>
      </table>  
      
      <br />      
      <div style="font-size:medium; color:Blue;">Detail</div>
      <hr style="color:Blue" />  
              <asp:Panel runat="server" ID="PnlDt">
              <asp:Button class="bitbtndt btnadd" runat="server" ID="btnAdddt" Text="Add" ValidationGroup="Input" />
                <div style="border:0px  solid; width:100%; height:100%; overflow:auto;">
                    <asp:GridView ID="GridDt" runat="server" AutoGenerateColumns="false" 
                        ShowFooter="True">
                        <HeaderStyle CssClass="GridHeader" />
                        <RowStyle CssClass="GridItem" Wrap="false" />
                        <AlternatingRowStyle CssClass="GridAltItem" />
                        <PagerStyle CssClass="GridPager" />
                        <Columns>
                            <asp:TemplateField HeaderText="Action">
                                      <ItemTemplate>
                                        <asp:Button ID="btnEdit" runat="server" class="bitbtn btnedit" Text="Edit" CommandName="Edit"/>
                                        <asp:Button ID="btnDelete" runat="server" class="bitbtndt btndelete" Text="Delete" OnClientClick="return confirm('Sure to delete this data?');" CommandName="Delete"/>						                                      
                                      </ItemTemplate>
                            </asp:TemplateField>   
                            <asp:BoundField DataField="ItemNo" HeaderText="No" />
                            <asp:BoundField DataField="EmpNumb" HeaderStyle-Width="120px" HeaderText="Employee" />
                            <asp:BoundField DataField="EmpName" HeaderStyle-Width="250px" HeaderText="Employee Name" />
                            <asp:BoundField DataField="Job_Level_Name" HeaderStyle-Width="180px" HeaderText="Job Level" />
                            <asp:BoundField DataField="Deduction_Name" HeaderStyle-Width="180px" HeaderText="Deduction" />
                            <asp:BoundField DataField="Currency" HeaderStyle-Width="80px" HeaderText="Currency" />
                            <asp:BoundField DataField="ForexRate" HeaderStyle-Width="50px" HeaderText="Rate" />
                            <asp:BoundField DataField="AmountForex" HeaderStyle-Width="100px" HeaderText="Amount Forex"  ItemStyle-HorizontalAlign="Right" />
                            <asp:BoundField DataField="Remark" HeaderStyle-Width="250px" HeaderText="Remark" />
                        </Columns>
                    </asp:GridView>
              </div>   
              <asp:Button class="bitbtndt btnadd" runat="server" ID="btnAddDtke2" Text="Add" ValidationGroup="Input" />
              </asp:Panel> 
              <asp:Panel runat="server" ID="pnlEditDt" Visible="false">
                <table style="width: 583px">             
                    <tr>
                        <td>Item No</td>
                        <td>:</td>
                        <td><asp:Label runat="server" ID="lbItemNo" /> </td>
                    </tr>
                    <tr>
                        <td class="style7">Employee</td>
                        <td>:</td>
                        <td><asp:TextBox CssClass="TextBox" ValidationGroup="Input" runat="server" 
                                ID="tbEmpNo" MaxLength="15" Width="100px" AutoPostBack="True" />
                            <asp:TextBox ID="tbEmpName" runat="server" CssClass="TextBoxR" Enabled="false" MaxLength="60" ValidationGroup="Input" Width="270px" />
                            <asp:Button ID="btnEmp" runat="server" class="btngo" Text="..." />
                        </td>
                    </tr>
                    <tr>                    
                        <td class="style7">Job Level</td>
                        <td>:</td>
                        <td style="margin-left: 40px">
                            <asp:DropDownList ID="ddlJobLevel" runat="server" CssClass="DropDownList" Enabled="false" Height="17px" ValidationGroup="Input" Width="228px">
                            </asp:DropDownList>
                        </td>
                    </tr>                                        
                    <tr>
                        <td class="style7">Deduction</td>
                        <td>:</td>
                        <td style="margin-left: 40px">
                        <asp:DropDownList ID="ddlDeduction" runat="server" CssClass="DropDownList" Height="17px" ValidationGroup="Input" Width="228px">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="style7">Currency</td>
                        <td>:</td>
                        <td style="margin-left: 40px">
                        <asp:DropDownList ID="ddlCurrency" runat="server" CssClass="DropDownList" Height="17px" ValidationGroup="Input" AutoPostBack="true" Width="228px">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="style7">Rate</td>
                        <td>:</td>
                        <td class="style1" style="margin-left: 40px">
                            <asp:TextBox ID="tbRate" runat="server" CssClass="TextBox" Height="18px" ValidationGroup="Input" Width="40px" />
                        </td>
                    </tr>
                    <tr>
                        <td class="style7">Amount</td>
                        <td>:</td>
                        <td class="style1" style="margin-left: 40px">
                            <asp:TextBox ID="tbAmountForex" runat="server" CssClass="TextBox" Height="18px" ValidationGroup="Input" Width="80px" />
                        </td>
                    </tr>
                    <tr>
                        <td class="style7">
                            Remark</td>
                        <td>
                            :</td>
                        <td class="style1" style="margin-left: 40px">
                            <asp:TextBox ID="tbRemarkDt" runat="server" CssClass="TextBox" MaxLength="255" ValidationGroup="Input" Width="445px" />
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
        <asp:Button ID="btnHome" runat="server" class="btngo" Text="Home" Width="48px"/>    
    </asp:Panel>        
    </div>
   
    <asp:Label runat ="server" ID="lbStatus" ForeColor="Red"/>
    </form>
</body>
</html>
