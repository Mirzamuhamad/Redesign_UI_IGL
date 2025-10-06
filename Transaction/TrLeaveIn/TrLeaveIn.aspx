<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TrLeaveIn.aspx.vb" Inherits="Transaction_TrLeaveIn_TrLeaveIn" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="BasicFrame.WebControls.BasicDatePicker" Namespace="BasicFrame.WebControls"
    TagPrefix="BDP" %>

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
       function setformat()
        {
        try
         {
             var QtyLeave = document.getElementById("tbLeave").value.replace(/\$|\,/g, "");
             document.getElementById("tbLeave").value = setdigit(QtyLeave, '<%=Viewstate("DigitQty")%>');
        }catch (err){
            alert(err.description);
          }      
        }   
    </script>
    
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
 <link href="https://fonts.googleapis.com/css2?family=Muli:wght@300;400;600&display=swap" rel="stylesheet">
    <style type="text/css">
        .style1
        {
        }
        .style5
        {
            width: 85px;
        }
        .style6
        {
            width: 90px;
        }
        .style7
        {
            width: 94px;
        }
        .style8
        {
            width: 131px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div class="Content">
    <div class="H1"><asp:Label runat="server" ID="lbTitle"></asp:Label></div>
     <hr style="color:Blue" />        
    <asp:Panel runat="server" id="PnlHd"> 
      <table>
        <tr>
            <td style="width:100px;text-align:right">Quick Search :</td>         
            <td><asp:TextBox CssClass="TextBox" runat="server" ID ="tbFilter"/> 
                  <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlField" >     
                    <asp:ListItem Selected="True" Value="TransNmbr">Trans No</asp:ListItem>
                    <asp:ListItem Value="dbo.FormatDate(TransDate)">Trans Date</asp:ListItem>
                    <asp:ListItem>Status</asp:ListItem>
                    <asp:ListItem Value="ProcessType">Process Type</asp:ListItem>
                    <asp:ListItem Value="Year">Year</asp:ListItem>
                    <asp:ListItem Value="Period">Period</asp:ListItem>
                    <asp:ListItem Value="QtyAdd">Add Leave</asp:ListItem>
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
                    <asp:ListItem Selected="True" Value="TransNmbr">Trans No</asp:ListItem>
                    <asp:ListItem Value="dbo.FormatDate(TransDate)">Trans Date</asp:ListItem>
                    <asp:ListItem>Status</asp:ListItem>
                    <asp:ListItem Value="ProcessType">Process Type</asp:ListItem>
                    <asp:ListItem Value="Year">Year</asp:ListItem>
                    <asp:ListItem Value="Period">Period</asp:ListItem>
                    <asp:ListItem Value="QtyAdd">Add Leave</asp:ListItem>
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
            <asp:Button class="btngo" runat="server" ID="BtnGo" Text="G" 
            Visible="False"/>
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
                  <asp:BoundField DataField="TransNmbr" HeaderStyle-Width="120px" SortExpression="Nmbr" HeaderText="Trans No"></asp:BoundField>                  
                  <asp:BoundField DataField="Status" SortExpression="Status" HeaderText="Status"></asp:BoundField>                  
                  <asp:BoundField DataField="TransDate" dataformatstring="{0:dd MMM yyyy}" htmlencode="true" HeaderStyle-Width="80px" SortExpression="TransDate" HeaderText="Trans Date"></asp:BoundField>
                  <asp:BoundField DataField="ProcessType" HeaderStyle-Width="100px" SortExpression="ProcessType" HeaderText="Process Type"></asp:BoundField>
                  <asp:BoundField DataField="Year" HeaderStyle-Width="80px" SortExpression="Year" HeaderText="Year"></asp:BoundField>
                  <asp:BoundField DataField="Period" HeaderStyle-Width="80px" SortExpression="Period" HeaderText="Period"></asp:BoundField>
                  <asp:BoundField DataField="QtyAdd" HeaderStyle-Width="80px" SortExpression="QtyAdd" HeaderText="Add Leave"></asp:BoundField>
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
      <table>
        <tr>
            <td class="style6">No</td>
            <td>:</td>
            <td><asp:TextBox CssClass="TextBoxR" runat="server" ID="tbCode" Width="150px" Enabled="False"/></td>            
            <td>Date</td>
            <td>:</td>
            <td><BDP:BasicDatePicker ID="tbDate" runat="server" DateFormat="dd MMM yyyy" 
                        ReadOnly = "true" ValidationGroup="Input"
                        ButtonImageHeight="19px" ButtonImageWidth="20px" 
                        DisplayType="TextBoxAndImage" 
                        TextBoxStyle-CssClass="TextDate" 
                        ShowNoneButton="False"><TextBoxStyle CssClass="TextDate" /></BDP:BasicDatePicker>                
                <asp:TextBox ID="tbProcessType" runat="server" 
                    CssClass="TextBoxR" Width="90px" MaxLength="12" 
                    Visible="False" />        
            </td>            
        </tr>        
        <tr>
            <td class="style6">Period Leave</td>
            <td>:</td>
            <td><asp:DropDownList runat="server" ValidationGroup="Input" ID="ddlYear" CssClass="DropDownList" AutoPostBack="True"/>
                 <asp:DropDownList runat="server" ID="ddlPeriod" CssClass="DropDownList"/>                
            </td>        
            <td>Add Leave</td>
            <td>:</td>
            <td><asp:TextBox ID="tbAddQty" runat="server" CssClass="TextBox" ValidationGroup="Input" Width="50px" MaxLength="20" /> days &nbsp &nbsp
            <asp:Button class="bitbtn btngo" ValidationGroup="Input" runat="server" 
                    ID="btnGetData" Text="Get All" Visible="false" Width="74px"/>
            </td>
        </tr>         
        <tr>
            <td>Remark</td>
            <td>:</td>
            <td colspan="4" style="margin-left: 80px"><asp:TextBox ID="tbRemark" runat="server" CssClass="TextBoxMulti" ValidationGroup="Input" Width="360px" MaxLength="255" TextMode="MultiLine" />
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
                            <asp:BoundField DataField="EmpNumb" HeaderText="Employee No" HeaderStyle-Width="100px" />   
                            <asp:BoundField DataField="EmpName" HeaderText="Employee Name" HeaderStyle-Width="220px"/>
                            <asp:BoundField DataField="JobTitleName" HeaderText="Job Title" HeaderStyle-Width="180px"/>
                            <asp:BoundField DataField="HireDate" dataformatstring="{0:dd MMM yyyy}" htmlencode="true" HeaderText="Hire Date" HeaderStyle-Width="80px"/>
                            <asp:BoundField DataField="WorkYear" HeaderStyle-Width="50px" HeaderText="Work Time Year" />
                            <asp:BoundField DataField="WorkMonth" HeaderStyle-Width="50px" HeaderText="Work Time Month" />
                            <asp:BoundField DataField="StartDate" dataformatstring="{0:dd MMM yyyy}" htmlencode="true" HeaderStyle-Width="80px" HeaderText="Start Date" />
                            <asp:BoundField DataField="EndDate" dataformatstring="{0:dd MMM yyyy}" htmlencode="true" HeaderStyle-Width="80px" HeaderText="End Date" />
                            <asp:BoundField DataField="QtyLeave" HeaderStyle-Width="80px" HeaderText="Leave (days)" />
                            <asp:BoundField DataField="Remark" HeaderText="Remark" HeaderStyle-Width="250px"/>
                        </Columns>
                    </asp:GridView>
              </div>   
              <asp:Button class="bitbtndt btnadd" runat="server" ID="btnAddDtke2" Text="Add" ValidationGroup="Input" />
              </asp:Panel> 
              <asp:Panel runat="server" ID="pnlEditDt" Visible="false">
                <table style="width: 90%">    
                    <tr>
                        <td class="style7">Employee</td>
                        <td>:</td>
                        <td><asp:TextBox CssClass="TextBox" ValidationGroup="Input" runat="server" ID="tbEmpNumb" MaxLength="12" Width="100px" 
                                AutoPostBack="True" />
                                <asp:TextBox CssClass="TextBoxR" runat="server" ID="tbEmpName" MaxLength="60" Width="180px" />
                                <asp:Button class="btngo" runat="server" ID="btnEmp" Text="..."/>
                        </td>                        
                    </tr>   
                    <tr>
                        <td class="style7">Job Title</td>
                        <td>:</td>
                        <td><asp:DropDownList ValidationGroup="Input" CssClass="DropDownList" 
                                runat="server" ID="ddlJobTitle" Enabled = "false" Height="16px" Width="294px"/>
                        </td>
                    </tr>       
                    <tr>
                        <td class="style7">Hire Date</td>
                        <td>:</td>
                        <td><BDP:BasicDatePicker ID="tbHireDate" runat="server" DateFormat="dd MMM yyyy" 
                        ReadOnly = "true" Enabled="false"
                        ButtonImageHeight="19px" ButtonImageWidth="20px" 
                        DisplayType="TextBoxAndImage" 
                        TextBoxStyle-CssClass="TextDate" 
                        ShowNoneButton="False"><TextBoxStyle CssClass="TextDate" /></BDP:BasicDatePicker> 
                        </td>                    
                  </tr>
                  <tr>      
                        <td class="style7">Work Time</td>
                        <td>:</td>
                        <td class="style1">
                            <asp:TextBox ID="tbWorkYear" runat="server" CssClass="TextBoxR" Enabled="False" 
                                MaxLength="8" ValidationGroup="Input" Width="101px" />&nbsp Year &nbsp 
                                <asp:TextBox ID="tbWorkMonth" runat="server" CssClass="TextBoxR" 
                                Enabled="False" MaxLength="8" ValidationGroup="Input" Width="101px" />&nbsp Month
                        </td>
                    </tr>
                    <tr>                    
                        <td class="style7">Leave (days)</td>
                        <td>:</td>
                        <td class="style1" style="margin-left: 40px">
                            <asp:TextBox ID="tbLeave" runat="server" CssClass="TextBox" MaxLength="8" ValidationGroup="Input" Width="101px" />
                        </td>
                        </tr>
                        <tr>
                        <td class="style7">Effective Date</td>
                        <td>:</td>
                        <td style="margin-left: 40px">
                            <BDP:BasicDatePicker ID="tbStartDate" runat="server" DateFormat="dd MMM yyyy" 
                        ReadOnly = "true" ValidationGroup="Input"
                        ButtonImageHeight="19px" ButtonImageWidth="20px" 
                        DisplayType="TextBoxAndImage" 
                        TextBoxStyle-CssClass="TextDate" 
                        ShowNoneButton="False"><TextBoxStyle CssClass="TextDate" /></BDP:BasicDatePicker>                         
                        &nbsp s/d &nbsp
                            <BDP:BasicDatePicker ID="tbEndDate" runat="server" DateFormat="dd MMM yyyy" 
                        ReadOnly = "true" ValidationGroup="Input"
                        ButtonImageHeight="19px" ButtonImageWidth="20px" 
                        DisplayType="TextBoxAndImage" 
                        TextBoxStyle-CssClass="TextDate" 
                        ShowNoneButton="False"><TextBoxStyle CssClass="TextDate" /></BDP:BasicDatePicker> 
                        </td>
                    
                                                
                    </tr>
                    <tr>
                        <td class="style7">Remark</td>
                        <td>:</td>
                        <td class="style1" style="margin-left: 40px">
                            <asp:TextBox ID="tbRemarkDt" runat="server" CssClass="TextBoxMulti" MaxLength="255" ValidationGroup="Input" TextMode="MultiLine" Width="445px" />
                        </td>
                    </tr>
                </table>
                <br />           
                <asp:Button ID="btnSaveDt" runat="server" class="bitbtndt btnsave" Text="Save"/> &nbsp;         
                <asp:Button ID="btnCancelDt" runat="server" class="bitbtndt btncancel" Text="Cancel"/> 
           </asp:Panel> 
       <br />      
        <asp:Button ID="btnSaveAll" runat="server" class="bitbtndt btnsave" Text="Save & New" ValidationGroup="Input" Width="97px"/> &nbsp;    
        <asp:Button ID="btnSaveTrans" runat="server" class="bitbtndt btnsave" Text="Save" ValidationGroup="Input"/> &nbsp;    
        <asp:Button ID="btnBack" runat="server" class="bitbtndt btncancel" Text="Cancel" ValidationGroup="Input"/>  &nbsp;
        <asp:Button ID="btnHome" runat="server" class="btngo" Text="Home" Width="48px"/>    
    </asp:Panel>        
    </div>
   
    <asp:Label runat ="server" ID="lbStatus" ForeColor="Red"/>
    </form>
</body>
</html>
