<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TrPEEmpTerm.aspx.vb" Inherits="Transaction_TrPEEmpTerm_TrPEEmpTerm" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="BasicFrame.WebControls.BasicDatePicker" Namespace="BasicFrame.WebControls"
    TagPrefix="BDP" %>
<%@ Register assembly="CrystalDecisions.Web, Version=10.5.3700.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Employee Term</title>
    <script src="../../Function/OpenDlg.js" type="text/javascript"></script>
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
        
         try
         {           
         document.getElementById("tbSalaryGP").value = setdigit(document.getElementById("tbSalaryGP").value.replace(/\$|\,/g,""),'<%=VIEWSTATE("DigitQty")%>');
        
        }catch (err){
            alert(err.description);
          }      
        }   
    </script>
    
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
</head>
<body>     
    <form id="form1" runat="server">    
    <div class="Content">
    <div class="H1">Employee Term</div>
     <hr style="color:Blue" />        
    <asp:Panel runat="server" id="PnlHd"> 
      <table>
        <tr>
            <td style="width:100px;text-align:right">Quick Search :</td>            
            <td><asp:TextBox CssClass="TextBox" runat="server" ID ="tbFilter"/> 
                  <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlField" >
                      <asp:ListItem Value="TransNmbr" Selected="True">TransNmbr</asp:ListItem>
                      <asp:ListItem Value="dbo.FormatDate(TransDate)">Date</asp:ListItem>
                      <asp:ListItem>Status</asp:ListItem>
                      <asp:ListItem Value="dbo.FormatDate(EffectiveDate)">Effective Date</asp:ListItem>
                      <asp:ListItem Value="TermType">SK Type</asp:ListItem>
                      <asp:ListItem Value="EmpApprName">Emp. Appr. Name</asp:ListItem>
                      <asp:ListItem Value="EmpApprJobTtl">JobTitle Appr Name</asp:ListItem>                      
                      <asp:ListItem Value="Remark">Remark</asp:ListItem>
                    </asp:DropDownList>
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
                      <asp:ListItem Value="TransNmbr" Selected="True">TransNmbr</asp:ListItem>
                      <asp:ListItem Value="dbo.FormatDate(TransDate)">Date</asp:ListItem>
                      <asp:ListItem>Status</asp:ListItem>
                      <asp:ListItem Value="dbo.FormatDate(EffectiveDate)">Effective Date</asp:ListItem>
                      <asp:ListItem Value="TermType">SK Type</asp:ListItem>
                      <asp:ListItem Value="EmpApprName">Emp. Appr. Name</asp:ListItem>
                      <asp:ListItem Value="EmpApprJobTtl">JobTitle Appr Name</asp:ListItem>                      
                      <asp:ListItem Value="Remark">Remark</asp:ListItem>                      
              </asp:DropDownList>
          </td>              
        </tr>        
      </table>      
      </asp:Panel>
          <br />
            <asp:Button class="bitbtn btnadd" runat="server" ID="BtnAdd" Text="Add"  />  
            &nbsp &nbsp &nbsp   
            <asp:DropDownList CssClass="DropDownList" ID="ddlCommand" runat="server" Visible="false"/>
            <asp:Button class="btngo" runat="server" ID="BtnGo" Text="G" Visible="false"/>   
          <br />
          <div style="border:0px  solid; width:100%; height:100%; overflow:auto;">
          <asp:GridView ID="GridView1" runat="server" AllowPaging="True" AllowSorting="True"
            CssClass="Grid" AutoGenerateColumns="False"> 
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
                  <asp:BoundField DataField="TransNmbr" HeaderStyle-Width="120px" SortExpression="Nmbr" HeaderText="Reference"></asp:BoundField>                  
                  <asp:BoundField DataField="Status" HeaderText="Status" SortExpression="Status"></asp:BoundField>                  
                  <asp:BoundField DataField="TransDate" dataformatstring="{0:dd MMM yyyy}" htmlencode="true" HeaderStyle-Width="80px" SortExpression="TransDate" HeaderText="Date"></asp:BoundField>                  
                  <asp:BoundField DataField="EffectiveDate" dataformatstring="{0:dd MMM yyyy}" htmlencode="true" HeaderStyle-Width="80px" SortExpression="EffectiveDate" HeaderText="Effective Date"></asp:BoundField>                  
                  <asp:BoundField DataField="TermType" HeaderText="SK Type" SortExpression="TermType"></asp:BoundField>                  
                  <asp:BoundField DataField="EmpApprName" HeaderText="Emp. Appr. Name" SortExpression="EmpApprName" ></asp:BoundField>
                  <asp:BoundField DataField="EmpApprJobTtl" HeaderText="Job Title Appr. Name" SortExpression="EmpApprJobTtl" ></asp:BoundField>
                  <asp:BoundField DataField="Remark" HeaderStyle-Width="250px" HeaderText="Remark"></asp:BoundField>
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
            <td>Reference</td>
            <td>:</td>
            <td><asp:TextBox CssClass="TextBox" Enabled = "false" runat="server" ID="tbRef" Width="149px"/> 
            </td> 
            <td>Date</td>
            <td>:</td>
            <td>
            <BDP:BasicDatePicker ID="tbDate" runat="server" DateFormat="dd MMM yyyy" 
                        ReadOnly = "true" ValidationGroup="Input"
                        ButtonImageHeight="19px" ButtonImageWidth="20px" 
                        DisplayType="TextBoxAndImage" 
                        TextBoxStyle-CssClass="TextDate" AutoPostBack="True" 
                    ShowNoneButton="False"><TextBoxStyle CssClass="TextDate" /></BDP:BasicDatePicker>                
            </td>               
        </tr>
        
        <tr>      
            <td>Effective Date</td>
            <td>:</td>
            <td><BDP:BasicDatePicker ID="tbEffectiveDate" runat="server" DateFormat="dd MMM yyyy" 
                        ReadOnly = "true" ValidationGroup="Input"
                        ButtonImageHeight="19px" ButtonImageWidth="20px" 
                        DisplayType="TextBoxAndImage" 
                        TextBoxStyle-CssClass="TextDate" 
                    ShowNoneButton="False"><TextBoxStyle CssClass="TextDate" /></BDP:BasicDatePicker>                
                    </td>           
            
            <td>SK Type</td>
            <td>:</td>
            <td><asp:DropDownList ValidationGroup="Input" CssClass="DropDownList" ID="ddlTermType" runat="server" >
                        <asp:ListItem Selected="True">New Employee</asp:ListItem>
                        <asp:ListItem>Pengangkatan</asp:ListItem>
                        <asp:ListItem>Promosi</asp:ListItem>
                        <asp:ListItem>Mutasi</asp:ListItem>
                        <asp:ListItem>Demosi</asp:ListItem>
                    </asp:DropDownList> </td>           
        </tr>
        <tr>
            <td>Approved By</td>
            <td>:</td>
            <td colspan="4"><asp:TextBox runat="server" ID="tbEmpAppr" CssClass="TextBox" AutoPostBack="true" />
                <asp:TextBox runat="server"  CssClass="TextBox" ID="tbEmpApprName" EnableTheming="True" ReadOnly="True" Enabled="False" Width="200px"/>
                <asp:Button ID="btnEmpAppr" runat="server" class="btngo" Text="..."/>
                        
            </td>
        </tr>  
        <tr>
            <td>Job Title</td>
            <td>:</td>
            <td colspan="4"><asp:DropDownList CssClass="DropDownList" Enabled="false" runat="server" ValidationGroup="Input" ID="ddlEmpApprJobTitle" />
            </td>
        </tr>   
        
        <tr>
            <td>Remark</td>
            <td>:</td>
            <td colspan="4">
                <asp:TextBox ID="tbRemark" runat="server" CssClass="TextBox" ValidationGroup="Input" Width="269px" />
            </td>
        </tr>
      </table>  
      
      <br />      
      <div style="font-size:medium; color:Blue;">Detail</div>
      <hr style="color:Blue" />  
        <asp:Panel runat="server" ID="pnlDt">
            <asp:Button class="bitbtndt btnadd" runat="server" ID="btnAdddt" Text="Add" ValidationGroup="Input" />
            <div style="border:0px  solid; width:100%; height:100%; overflow:auto;">
                <asp:GridView ID="GridDt" runat="server" AutoGenerateColumns="False" 
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
                            <EditItemTemplate>
                                <asp:Button ID="btnUpdate" runat="server" class="bitbtndt btnsave" Text="Save" CommandName="Update"/>
                                <asp:Button ID="btnCancel" runat="server" class="bitbtndt btncancel" Text="Cancel" CommandName="Cancel"/>   
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="EmpNumb" HeaderStyle-Width="200px" HeaderText="Employee" SortExpression="EmpNumb" ></asp:BoundField>
                        <asp:BoundField DataField="Emp_Name" HeaderText="Employee Name" HeaderStyle-Width="120px" SortExpression="Emp_Name" ></asp:BoundField>
                        <asp:BoundField DataField="SKCompanyNo" HeaderStyle-Width="200px" HeaderText="SK Company No" SortExpression="SKCompanyNo" ></asp:BoundField>
                        <asp:BoundField DataField="CurrJobTtlName" HeaderText="Curr JobTtl Name" HeaderStyle-Width="120px" SortExpression="CurrJobTtlName" ></asp:BoundField>
                        <asp:BoundField DataField="CurrJobLvlName" HeaderStyle-Width="200px" HeaderText="Curr JobLvl Name" SortExpression="CurrJobLvlName" ></asp:BoundField>
                        <asp:BoundField DataField="CurrEmpStatusName" HeaderText="Curr EmpStatus Name" HeaderStyle-Width="120px" SortExpression="CurrEmpStatusName" ></asp:BoundField>
                        <asp:BoundField DataField="CurrDeptName" HeaderStyle-Width="200px" HeaderText="Curr Organization Name" SortExpression="CurrDeptName" ></asp:BoundField>
                        <asp:BoundField DataField="CurrWorkPlaceName" HeaderText="Curr WorkPlace Name" HeaderStyle-Width="120px" SortExpression="CurrWorkPlaceName" ></asp:BoundField>
                        <asp:BoundField DataField="CurrContractEndDate" HeaderText="Curr Contract EndDate" HeaderStyle-Width="120px" SortExpression="CurrContractEndDate" ></asp:BoundField>
                        <asp:BoundField DataField="NewJobTtlName" HeaderText="New JobTtl Name" HeaderStyle-Width="120px" SortExpression="NewJobTtlName" ></asp:BoundField>
                        <asp:BoundField DataField="NewJobLvlName" HeaderStyle-Width="200px" HeaderText="New JobLvl Name" SortExpression="NewJobLvlName" ></asp:BoundField>
                        <asp:BoundField DataField="NewEmpStatusName" HeaderText="New EmpStatus Name" HeaderStyle-Width="120px" SortExpression="NewEmpStatusName" ></asp:BoundField>
                        <asp:BoundField DataField="NewDeptName" HeaderStyle-Width="200px" HeaderText="New Organization Name" SortExpression="NewDeptName" ></asp:BoundField>
                        <asp:BoundField DataField="NewWorkPlaceName" HeaderText="New WorkPlace Name" HeaderStyle-Width="120px" SortExpression="NewWorkPlaceName" ></asp:BoundField>
                        <asp:BoundField DataField="NewContractEndDate" HeaderText="New Contract EndDate" HeaderStyle-Width="120px" SortExpression="NewContractEndDate" ></asp:BoundField>
                        <asp:BoundField DataField="Appraisal" HeaderStyle-Width="200px" HeaderText="Appraisal" ></asp:BoundField>
                        <asp:BoundField DataField="ChangeReason" HeaderStyle-Width="200px" HeaderText="Change Reason" ></asp:BoundField>
                        <asp:BoundField DataField="Remark" HeaderStyle-Width="200px" HeaderText="Remark" ></asp:BoundField>
                    </Columns>
                </asp:GridView>
          </div>   
          <asp:Button class="bitbtndt btnadd" runat="server" ID="btnAddDt2" Text="Add" ValidationGroup="Input" />  
       </asp:Panel>             
       <asp:Panel runat="server" ID="pnlEditDt" Visible="false">
         <table>
          <tr>
             <td>Employee</td>
             <td>:</td>
             <td colspan="5"><asp:TextBox runat="server" ID="tbEmpCode" CssClass="TextBox" AutoPostBack="true" />
                <asp:TextBox runat="server"  CssClass="TextBox" ID="tbEmpName" EnableTheming="True" ReadOnly="True" Enabled="False" Width="200px"/>
                <asp:Button ID="btnEmp" runat="server" class="btngo" Text="..."/>                        
             </td>
          </tr>
          <tr>
             <td>SK Company No</td>
             <td>:</td>
             <td colspan="5"><asp:TextBox ID="tbSKNo" runat="server" CssClass="TextBox" />
             </td>
          </tr>
          <tr>
             <td>Old Job Title</td>
             <td>:</td>
             <%--<td><asp:TextBox runat="server"  CssClass="TextBox" ID="tbCurrJobTtl" EnableTheming="True" ReadOnly="True" Enabled="False" /></td>--%>
             <td Width="100px"><asp:DropDownList CssClass="DropDownList" runat="server" Enabled="False" ID="ddlCurrJobTtl" Width="300px" /></td>
             <td>New Job Title</td>
             <td>:</td>
             <td><asp:DropDownList CssClass="DropDownList" runat="server" ValidationGroup="Input" ID="ddlNewJobTtl" Width="300px" /></td>
          </tr>
          <tr>
             <td>Old Job Level</td>
             <td>:</td>
             <%--<td><asp:TextBox runat="server"  CssClass="TextBox" ID="tbCurrJobLevel" EnableTheming="True" ReadOnly="True" Enabled="False" /></td>--%>
             <td><asp:DropDownList CssClass="DropDownList" runat="server" Enabled="False" ID="ddlCurrJobLevel" Width="300px"/></td>
             <td>New Job Level</td>
             <td>:</td>
             <td><asp:DropDownList CssClass="DropDownList" ID="ddlNewJobLevel" runat="server" ValidationGroup="Input" Width="300px"/></td>
          </tr>          
          <tr>
             <td>Old Organization</td>
             <td>:</td>             
             <td><asp:DropDownList CssClass="DropDownList" runat="server" Enabled="False" ID="ddlCurrDept" Width="300px"/></td>
             <td>New Organization</td>
             <td>:</td>
             <td><asp:DropDownList CssClass="DropDownList" runat="server" ValidationGroup="Input" ID="ddlNewDept" Width="300px"/></td>
          </tr>          
          <tr>
             <td>Old Emp Status</td>
             <td>:</td>
             <%--<td><asp:TextBox runat="server"  CssClass="TextBox" ID="tbCurrEmpStatus" EnableTheming="True" ReadOnly="True" Enabled="False" /></td>--%>
             <td><asp:DropDownList CssClass="DropDownList" runat="server" Enabled="False" ID="ddlCurrEmpStatus" Width="300px"/></td>
             <td>New Emp Status</td>
             <td>:</td>
             <td><asp:DropDownList CssClass="DropDownList" runat="server" ValidationGroup="Input" ID="ddlNewEmpStatus" AutoPostBack="True" Width="300px"/></td>
          </tr>          
          <tr>
             <td>Old Work Place</td>
             <td>:</td>
             <%--<td><asp:TextBox runat="server"  CssClass="TextBox" ID="tbCurrWorkPlace" EnableTheming="True" ReadOnly="True" Enabled="False" /></td>--%>
             <td><asp:DropDownList CssClass="DropDownList" runat="server" Enabled="False" ID="ddlCurrWorkPlace" Width="300px"/></td>
             <td>New Work Place</td>
             <td>:</td>
             <td><asp:DropDownList CssClass="DropDownList" runat="server" ValidationGroup="Input" ID="ddlNewWorkPlace" Width="300px"/></td>
          </tr> 
          <tr>
                <td>Curr End Date</td>
                <td>:</td>
                <td>
                <BDP:BasicDatePicker ID="tbCurrEndDate" runat="server" DateFormat="dd MMM yyyy" 
                        ReadOnly = "true" ValidationGroup="Input"
                        ButtonImageHeight="19px" ButtonImageWidth="20px" 
                        DisplayType="TextBoxAndImage" 
                        TextBoxStyle-CssClass="TextDate" Enabled ="False" 
                    ShowNoneButton="False"><TextBoxStyle CssClass="TextDate" /></BDP:BasicDatePicker>                
                </td>        
                <td>New End Date</td>
                <td>:</td>
                <td>
                <BDP:BasicDatePicker ID="tbNewEndDate" runat="server" DateFormat="dd MMM yyyy" 
                        ReadOnly = "true" ValidationGroup="Input"
                        ButtonImageHeight="19px" ButtonImageWidth="20px" 
                        DisplayType="TextBoxAndImage" 
                        TextBoxStyle-CssClass="TextDate"  Enabled ="False"
                    ShowNoneButton="False"><TextBoxStyle CssClass="TextDate" /></BDP:BasicDatePicker>                
                </td>        
          </tr>
          <tr>
             <td>Appraisal </td>
             <td>:</td>
             <td colspan="5"><asp:TextBox ID="tbAppraisal" runat="server" CssClass="TextBox" Width="304px" />
             </td>
          </tr>
          <tr>
             <td>Change Reason</td>
             <td>:</td>
             <td colspan="5"><asp:TextBox ID="tbChangeReason" runat="server" CssClass="TextBox" Width="304px" />
             </td>
          </tr>        
          <tr>
             <td>Remark </td>
             <td>:</td>
             <td colspan="5"><asp:TextBox ID="tbRemarkDt" runat="server" CssClass="TextBox" Width="304px" />
             </td>
          </tr>
         </table>
            <br />
            <asp:Button ID="btnSaveDt" runat="server" class="bitbtndt btnsave" Text="Save"/> &nbsp;         
            <asp:Button ID="btnCancelDt" runat="server" class="bitbtndt btncancel" Text="Cancel"/> 
            <br />
       </asp:Panel> 
       <br />
       
       <br />          
       <asp:Button ID="btnSaveAll" runat="server" class="bitbtndt btnsave" 
            Text="Save & New" ValidationGroup="Input" Width="97px"/> &nbsp;    
        <asp:Button ID="btnSaveTrans" runat="server" class="bitbtndt btnsave" Text="Save" ValidationGroup="Input"/> &nbsp;    
        <asp:Button ID="btnBack" runat="server" class="bitbtndt btncancel" Text="Cancel" ValidationGroup="Input"/>  &nbsp;
        <asp:Button ID="btnHome" runat="server" class="bitbtndt btncancel" Text="Home"/>    
    </asp:Panel>
    <asp:Panel runat="server" ID="pnlPrint" Visible="false">
    <cr:crystalreportviewer ID="CrystalReportViewer1" runat="server" AutoDataBind="True"
               Height="1036px" Width="928px" />
    </asp:Panel>
      <br />            
    </div>
   
    <asp:Label runat ="server" ID="lbStatus" ForeColor="Red"/>
   
    </form>
    </body>
</html>
