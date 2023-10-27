<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TrEmpReprimand.aspx.vb" Inherits="Transaction_TrEmpReprimand_TrEmpReprimand" %>

<%@ Register Assembly="BasicFrame.WebControls.BasicDatePicker" Namespace="BasicFrame.WebControls"
    TagPrefix="BDP" %>
    
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
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
       function setformat()
        {
        try
         {         
         var EstCost = document.getElementById("tbEstCost").value.replace(/\$|\,/g,"");                           
         document.getElementById("tbEstCost").value = setdigit(EstCost,'<%=Viewstate("DigitCurr")%>');
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
    <div class="H1">Employee Indiscipliner</div>
     <hr style="color:Blue" />        
    <asp:Panel runat="server" id="PnlHd"> 
      <table>
        <tr>
            <td style="width:100px;text-align:right">Quick Search :</td>         
            <td><asp:TextBox CssClass="TextBox" runat="server" ID ="tbFilter"/> 
                  <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlField" >     
                    <asp:ListItem Selected="True" Value="TransNmbr">Indiscipliner No</asp:ListItem>
                    <asp:ListItem Value="dbo.FormatDate(TransDate)">Indiscipliner Date</asp:ListItem>
                    <asp:ListItem>Status</asp:ListItem>
                    <asp:ListItem Value="dbo.FormatDate(EffectiveDate)">Effective Date</asp:ListItem>
                    <asp:ListItem Value="IndisciplinerType">Indiscipliner Type</asp:ListItem>
                    <asp:ListItem Value="IndisciplinerStatus">Indiscipliner Status</asp:ListItem>
                    <asp:ListItem Value="MasaBerlaku">Masa Berlaku</asp:ListItem>
                    <asp:ListItem Value="EmpAppr1">Employee Approval 1</asp:ListItem>
                    <asp:ListItem Value="EmpNameAppr1">Employee Name Approval 1</asp:ListItem>
                    <asp:ListItem Value="JbtNameAppr1">Job Title</asp:ListItem>
                    <asp:ListItem Value="Perjanjian">Perjanjian Kerja Bersama</asp:ListItem>
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
                    <asp:ListItem Selected="True" Value="TransNmbr">Indiscipliner No</asp:ListItem>
                    <asp:ListItem Value="dbo.FormatDate(TransDate)">Indiscipliner Date</asp:ListItem>
                    <asp:ListItem>Status</asp:ListItem>
                    <asp:ListItem Value="dbo.FormatDate(EffectiveDate)">Effective Date</asp:ListItem>
                    <asp:ListItem Value="IndisciplinerType">Indiscipliner Type</asp:ListItem>
                    <asp:ListItem Value="IndisciplinerStatus">Indiscipliner Status</asp:ListItem>
                    <asp:ListItem Value="MasaBerlaku">Masa Berlaku</asp:ListItem>
                    <asp:ListItem Value="EmpAppr1">Employee Approval 1</asp:ListItem>
                    <asp:ListItem Value="EmpNameAppr1">Employee Name Approval 1</asp:ListItem>
                    <asp:ListItem Value="JbtNameAppr1">Job Title</asp:ListItem>
                    <asp:ListItem Value="Perjanjian">Perjanjian Kerja Bersama</asp:ListItem>
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
                              <asp:ListItem Text="Print" />
                          </asp:DropDownList>
                          <asp:Button class="btngo" runat="server" ID="BtnGo" Text="G" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" CommandName="Go" />
                      </ItemTemplate>
                      <HeaderStyle Width="110px" />
                  </asp:TemplateField>            
                  <asp:BoundField DataField="TransNmbr" HeaderStyle-Width="120px" SortExpression="Nmbr" HeaderText="Indiscipliner No"></asp:BoundField>                  
                  <asp:BoundField DataField="Status" SortExpression="Status" HeaderText="Status"></asp:BoundField>                  
                  <asp:BoundField DataField="TransDate" dataformatstring="{0:dd MMM yyyy}" htmlencode="true" HeaderStyle-Width="80px" SortExpression="TransDate" HeaderText="Indiscipliner Date"></asp:BoundField>
                  <asp:BoundField DataField="EffectiveDate" dataformatstring="{0:dd MMM yyyy}" HeaderStyle-Width="80px" SortExpression="EffectiveDate" HeaderText="Effective Date"></asp:BoundField>
                  <asp:BoundField DataField="IndisciplinerType" HeaderStyle-Width="100px" SortExpression="IndisciplinerType" HeaderText="Indiscipliner Type"></asp:BoundField>
                  <asp:BoundField DataField="IndisciplinerStatus" HeaderStyle-Width="100px" SortExpression="IndisciplinerStatus" HeaderText="Indiscipliner Status"></asp:BoundField>
                  <asp:BoundField DataField="MasaBerlaku" HeaderStyle-Width="80px" SortExpression="MasaBerlaku" HeaderText="Masa Berlaku (Month)"></asp:BoundField>
                  <asp:BoundField DataField="EmpAppr1" HeaderStyle-Width="100px" SortExpression="EmpAppr1" HeaderText="Employee Approval 1"></asp:BoundField>
                  <asp:BoundField DataField="EmpNameAppr1" HeaderStyle-Width="250px" SortExpression="EmpNameAppr1" HeaderText="Employee Name Approval 1"></asp:BoundField>
                  <asp:BoundField DataField="JbtNameAppr1" HeaderStyle-Width="200px" SortExpression="JbtNameAppr1" HeaderText="Job Title"></asp:BoundField>
                  <asp:BoundField DataField="Perjanjian" HeaderStyle-Width="250px" SortExpression="Perjanjian" HeaderText="Perjanjian Kerja Bersama"></asp:BoundField>
                  <asp:BoundField DataField="Remark" HeaderStyle-Width="250px" SortExpression="Remark" HeaderText="Remark"></asp:BoundField>
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
            <td class="style6">Indiscipliner No</td>
            <td>:</td>
            <td><asp:TextBox CssClass="TextBoxR" runat="server" ID="tbCode" Width="150px" Enabled="False"/></td>
            
            <td>Indiscipliner Date</td>
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
            <td class="style6">Effective Date</td>
            <td>:</td>
            <td><BDP:BasicDatePicker ID="tbEffectiveDate" runat="server" DateFormat="dd MMM yyyy" 
                        ReadOnly = "true" ValidationGroup="Input"
                        ButtonImageHeight="19px" ButtonImageWidth="20px" 
                        DisplayType="TextBoxAndImage" 
                        TextBoxStyle-CssClass="TextDate" 
                        ShowNoneButton="False"><TextBoxStyle CssClass="TextDate" /></BDP:BasicDatePicker>
            </td>
            <td class="style6">Masa Berlaku</td>
              <td>:</td>
              <td ><asp:TextBox ID="tbMasaBerlaku" runat="server" CssClass="TextBox" MaxLength="60" ValidationGroup="Input" Width="35px" />
                  &nbsp;Month</td>
        </tr>
        <tr>        
            <td class="style6">Indiscipliner Type</td>
            <td>:</td>
            <td><asp:DropDownList runat="server" ValidationGroup="Input" ID="ddlIndisType" 
                    CssClass="DropDownList" Width="120px">
                <asp:ListItem>Teguran</asp:ListItem>
                <asp:ListItem>SP 1</asp:ListItem>
                <asp:ListItem>SP 2</asp:ListItem>
                <asp:ListItem>SP 3</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td>Indiscipliner Status</td>
            <td>:</td>
            <td><asp:DropDownList runat="server" ValidationGroup="Input" ID="ddlIndisStatus" CssClass="DropDownList" Width="80px">
                <asp:ListItem>Lisan</asp:ListItem>
                <asp:ListItem>Tertulis</asp:ListItem>
                </asp:DropDownList>    
            </td>   
        </tr>
          <tr>
              <td class="style6">Emp Approval 1</td>
              <td>:</td>
              <td colspan="4">
                  <asp:TextBox ID="tbEmpAppr1" runat="server" CssClass="TextBox" MaxLength="15" 
                      ValidationGroup="Input" Width="99px" AutoPostBack="True" />
                  <asp:TextBox ID="tbEmpNameAppr1" runat="server" CssClass="TextBoxR" 
                      MaxLength="100" ValidationGroup="Input" Width="262px" ReadOnly="True"/>
                  <asp:Button ID="btnEmpAppr1" runat="server" class="btngo" Text="..." 
                      ValidationGroup="Input" />
              </td>
          </tr>
          <tr>
              <td class="style6">Job Title Approval 1</td>
              <td>:</td>
              <td colspan="4">
                  <asp:DropDownList ID="ddlJbtAppr1" runat="server" CssClass="DropDownList" 
                      ValidationGroup="Input" Width="250px" Enabled="False">
                  </asp:DropDownList>
              </td>
          </tr>
          <tr>
              <td class="style6">Perjanjian Kerja Bersama</td>
              <td>:</td>
              <td colspan="4"><asp:TextBox ID="tbKerjaBersama" runat="server" CssClass="TextBoxMulti" MaxLength="255" ValidationGroup="Input" Width="380px" TextMode="MultiLine" />
              </td>
          </tr>
          <tr>
              <td class="style6">Remark</td>
              <td>:</td>
              <td colspan="4"><asp:TextBox ID="tbRemark" runat="server" CssClass="TextBoxMulti" MaxLength="255" ValidationGroup="Input" Width="380px" TextMode="MultiLine"/>
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
                            <asp:BoundField DataField="EmpNumb" HeaderText="Employee" HeaderStyle-Width="100px"/>
                            <asp:BoundField DataField="Emp_Name" HeaderText="Employee Name" HeaderStyle-Width="220px"/>
                            <asp:BoundField DataField="DeptName" HeaderStyle-Width="150px" HeaderText="Organization" />
                            <asp:BoundField DataField="JobTitleName" HeaderStyle-Width="150px" HeaderText="Job Title" />
                            <asp:BoundField DataField="IndisciplinerInfo" HeaderStyle-Width="250px" HeaderText="Indiscipliner Info" />
                            <asp:BoundField DataField="StatementEmp" HeaderStyle-Width="250px" HeaderText="Statement Employee" />
                            <asp:BoundField DataField="StatementAppr" HeaderStyle-Width="250px" HeaderText="Statement Approval" />
                            <asp:BoundField DataField="EmpAppr2" HeaderStyle-Width="100px" HeaderText="Emp Approval 2" />
                            <asp:BoundField DataField="EmpNameAppr2" HeaderStyle-Width="220px" HeaderText="Emp Name Approval 2" />
                            <asp:BoundField DataField="JbtNameAppr2" HeaderStyle-Width="150px" HeaderText="Job Title" />
                            <asp:BoundField DataField="Remark" HeaderStyle-Width="250px" HeaderText="Remark" />
                        </Columns>
                    </asp:GridView>
              </div>   
              <asp:Button class="bitbtndt btnadd" runat="server" ID="btnAddDtke2" Text="Add" ValidationGroup="Input" />
              </asp:Panel> 
              <asp:Panel runat="server" ID="pnlEditDt" Visible="false">
                <table style="width: 583px">             
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
                        <td class="style7">Organization</td>
                        <td>:</td>
                        <td class="style1">
                            <asp:DropDownList ID="ddlDepartment" runat="server" CssClass="DropDownList" Enabled="false" Height="17px" ValidationGroup="Input" Width="228px">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>                    
                        <td class="style7">Job Title</td>
                        <td>:</td>
                        <td style="margin-left: 40px">
                            <asp:DropDownList ID="ddlJobTitle" runat="server" CssClass="DropDownList" Enabled="false" Height="17px" ValidationGroup="Input" Width="228px">
                            </asp:DropDownList>
                        </td>
                    </tr>                    
                    <tr>
                        <td class="style7">Indiscipliner Info</td>
                        <td>:</td>
                        <td class="style1" style="margin-left: 40px">
                            <asp:TextBox ID="tbIndisInfo" runat="server" CssClass="TextBoxMulti" MaxLength="255" ValidationGroup="Input" Width="419px" TextMode="MultiLine" />
                        </td>
                    </tr>
                    <tr>
                        <td class="style7">Statement Emp</td>
                        <td>:</td>
                        <td class="style1" style="margin-left: 40px">
                            <asp:TextBox ID="tbStatementEmp" runat="server" CssClass="TextBoxMulti" MaxLength="255" ValidationGroup="Input" Width="419px" TextMode="MultiLine"/>
                        </td>
                    </tr>
                    <tr>
                        <td class="style7">Statement Appr</td>
                        <td>:</td>
                        <td class="style1" style="margin-left: 40px">
                            <asp:TextBox ID="tbStatementAppr" runat="server" CssClass="TextBoxMulti" MaxLength="255" ValidationGroup="Input" Width="419px" TextMode="MultiLine"/>
                        </td>
                    </tr>
                    <tr>
                        <td class="style7">Emp Approval 2</td>
                        <td>:</td>
                        <td class="style1" style="margin-left: 40px">
                            <asp:TextBox ID="tbEmpAppr2" runat="server" CssClass="TextBox" MaxLength="15" 
                                ValidationGroup="Input" Width="100px" AutoPostBack="True" />
                            <asp:TextBox ID="tbEmpNameAppr2" runat="server" CssClass="TextBoxR" Enabled="false" MaxLength="60" ValidationGroup="Input" Width="270px" />
                            <asp:Button ID="btnEmpAppr2" runat="server" class="btngo" Text="..." />
                        </td>
                    </tr>
                    <tr>
                        <td class="style7">Job Title Approval 2</td>
                        <td>:</td>
                        <td class="style1" style="margin-left: 40px">
                            <asp:DropDownList ID="ddlJbtAppr2" runat="server" CssClass="DropDownList" Enabled="false" Height="17px" ValidationGroup="Input" Width="228px">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="style7">
                            Remark</td>
                        <td>
                            :</td>
                        <td class="style1" style="margin-left: 40px">
                            <asp:TextBox ID="tbRemarkDt" runat="server" CssClass="TextBoxMulti" MaxLength="255" ValidationGroup="Input" Width="419px" TextMode="MultiLine"/>
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
