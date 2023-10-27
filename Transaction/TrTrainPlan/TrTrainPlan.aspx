<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TrTrainPlan.aspx.vb" Inherits="Transaction_TrTrainPlan_TrTrainPlan" %>

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
         var TrainingCost = document.getElementById("tbTrainingCost").value.replace(/\$|\,/g,"");                           
         document.getElementById("tbTrainingCost").value = setdigit(TrainingCost,'<%=Viewstate("DigitCurr")%>');
        }catch (err){
            alert(err.description);
          }      
        } 
        
        function closing()
        {
            try
            {
                var result = prompt("Remark Close", "");
                if (result){
                    document.getElementById("HiddenRemarkClose").value = result;
                } else {
                    document.getElementById("HiddenRemarkClose").value = "False Value";
                }
                postback();
                //document.form1.submit();                
            }catch(err){
                alert(err.description);
            }        
        }
        
        function postback()
        {
            __doPostBack('','');
        }  
    </script>
    
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .style1
        {
        }
        .style6
        {
            width: 100px;
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
    <div class="H1">Training Plan</div>
     <hr style="color:Blue" />        
    <asp:Panel runat="server" id="PnlHd"> 
      <table>
        <tr>
            <td style="width:100px;text-align:right">Quick Search :</td>         
            <td><asp:TextBox CssClass="TextBox" runat="server" ID ="tbFilter"/> 
                  <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlField" >     
                    <asp:ListItem Selected="True" Value="TransNmbr">Plan No</asp:ListItem>
                    <asp:ListItem Value="dbo.FormatDate(TransDate)">Plan Date</asp:ListItem>
                    <asp:ListItem>Status</asp:ListItem>
                    <asp:ListItem Value="FgSchedule">Training Yearly</asp:ListItem>
                    <asp:ListItem Value="Department_Name">Organization</asp:ListItem>
                    <asp:ListItem Value="ScheduleNo">Schedule No</asp:ListItem>
                    <asp:ListItem Value="CourseTitle">Training Name</asp:ListItem>
                    <asp:ListItem Value="TrainingName">Training Type</asp:ListItem>
                    <asp:ListItem Value="TrainingPlace">Training Place</asp:ListItem>
                    <asp:ListItem Value="TrainingLocation">Training Location</asp:ListItem>
                    <asp:ListItem Value="Sasaran">Sasaran</asp:ListItem>
                    <asp:ListItem Value="Materi">Materi</asp:ListItem>
                    <asp:ListItem Value="InstitutionName">Institution</asp:ListItem>
                    <asp:ListItem Value="Instructor">Instructor</asp:ListItem>
                    <asp:ListItem Value="dbo.FormatDate(StartDate)">Start Date</asp:ListItem>
                    <asp:ListItem Value="dbo.FormatDate(EndDate)">End Date</asp:ListItem>
                    <asp:ListItem Value="TrainingCost">Training Cost</asp:ListItem>
                    <asp:ListItem Value="FgSertifikat">Sertification</asp:ListItem>
                    <asp:ListItem Value="QtyParticipant">Qty Participant</asp:ListItem>
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
                    <asp:ListItem Selected="True" Value="TransNmbr">Plan No</asp:ListItem>
                    <asp:ListItem Value="dbo.FormatDate(TransDate)">Plan Date</asp:ListItem>
                    <asp:ListItem>Status</asp:ListItem>
                    <asp:ListItem Value="FgSchedule">Training Yearly</asp:ListItem>
                    <asp:ListItem Value="Department_Name">Organization</asp:ListItem>
                    <asp:ListItem Value="ScheduleNo">Schedule No</asp:ListItem>
                    <asp:ListItem Value="CourseTitle">Training Name</asp:ListItem>
                    <asp:ListItem Value="TrainingName">Training Type</asp:ListItem>
                    <asp:ListItem Value="TrainingPlace">Training Place</asp:ListItem>
                    <asp:ListItem Value="TrainingLocation">Training Location</asp:ListItem>
                    <asp:ListItem Value="Sasaran">Sasaran</asp:ListItem>
                    <asp:ListItem Value="Materi">Materi</asp:ListItem>
                    <asp:ListItem Value="InstitutionName">Institution</asp:ListItem>
                    <asp:ListItem Value="Instructor">Instructor</asp:ListItem>
                    <asp:ListItem Value="dbo.FormatDate(StartDate)">Start Date</asp:ListItem>
                    <asp:ListItem Value="dbo.FormatDate(EndDate)">End Date</asp:ListItem>
                    <asp:ListItem Value="TrainingCost">Training Cost</asp:ListItem>
                    <asp:ListItem Value="FgSertifikat">Sertification</asp:ListItem>
                    <asp:ListItem Value="QtyParticipant">Qty Participant</asp:ListItem>
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
                  <asp:BoundField DataField="TransNmbr" HeaderStyle-Width="120px" SortExpression="Nmbr" HeaderText="Plan No"></asp:BoundField>                  
                  <asp:BoundField DataField="Status" SortExpression="Status" HeaderText="Status"></asp:BoundField>                  
                  <asp:BoundField DataField="TransDate" dataformatstring="{0:dd MMM yyyy}" htmlencode="true" HeaderStyle-Width="80px" SortExpression="TransDate" HeaderText="Plan Date"></asp:BoundField>
                  <asp:BoundField DataField="FgSchedule" HeaderStyle-Width="80px" SortExpression="FgSchedule" HeaderText="Training Yearly"></asp:BoundField>
                  <asp:BoundField DataField="ScheduleNo" HeaderStyle-Width="120px" SortExpression="ScheduleNo" HeaderText="Schedule No"></asp:BoundField>
                  <asp:BoundField DataField="Department_Name" HeaderStyle-Width="180px" SortExpression="Department_Name" HeaderText="Organization"></asp:BoundField>
                  <asp:BoundField DataField="CourseTitle" HeaderStyle-Width="250px" SortExpression="CourseTitle" HeaderText="Training Name"></asp:BoundField>
                  <asp:BoundField DataField="TrainingName" HeaderStyle-Width="180px" SortExpression="TrainingName" HeaderText="Training Type"></asp:BoundField>
                  <asp:BoundField DataField="TrainingPlace" HeaderStyle-Width="120px" SortExpression="TrainingPlace" HeaderText="Training Place"></asp:BoundField>
                  <asp:BoundField DataField="TrainingLocation" HeaderStyle-Width="120px" SortExpression="TrainingLocation" HeaderText="Training Location"></asp:BoundField>
                  <asp:BoundField DataField="Sasaran" HeaderStyle-Width="250px" SortExpression="Sasaran" HeaderText="Sasaran"></asp:BoundField>
                  <asp:BoundField DataField="Materi" HeaderStyle-Width="250px" SortExpression="Materi" HeaderText="Materi"></asp:BoundField>
                  <asp:BoundField DataField="InstitutionName" HeaderStyle-Width="180px" SortExpression="InstitutionName" HeaderText="Institution"></asp:BoundField>
                  <asp:BoundField DataField="Instructor" HeaderStyle-Width="120px" SortExpression="Instructor" HeaderText="Instructor"></asp:BoundField>
                  <asp:BoundField DataField="StartDate" dataformatstring="{0:dd MMM yyyy}" htmlencode="true" HeaderStyle-Width="80px" SortExpression="StartDate" HeaderText="Start Date"></asp:BoundField>
                  <asp:BoundField DataField="EndDate" dataformatstring="{0:dd MMM yyyy}" htmlencode="true" HeaderStyle-Width="80px" SortExpression="EndDate" HeaderText="End Date"></asp:BoundField>
                  <asp:BoundField DataField="TrainingCost" HeaderStyle-Width="80px" SortExpression="TrainingCost" HeaderText="Training Cost"></asp:BoundField>
                  <asp:BoundField DataField="FgSertifikat" HeaderStyle-Width="80px" SortExpression="FgSertifikat" HeaderText="Sertification"></asp:BoundField>
                  <asp:BoundField DataField="QtyParticipant" HeaderStyle-Width="80px" SortExpression="QtyParticipant" HeaderText="Qty Participant"></asp:BoundField>
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
      <table style="width: 90%">
        <tr>
            <td class="style6">Plan No</td>
            <td>:</td>
            <td><asp:TextBox CssClass="TextBoxR" runat="server" ID="tbCode" Width="150px" Enabled="False"/></td>
            
            <td>Plan Date</td>
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
            <td class="style6">Schedule Yearly</td>
            <td>:</td>
            <td><asp:DropDownList runat="server" ValidationGroup="Input" ID="ddlFgSchedule" 
                    CssClass="DropDownList" AutoPostBack="True">
                <asp:ListItem Selected="True">Y</asp:ListItem>
                <asp:ListItem>N</asp:ListItem>
                </asp:DropDownList>            
                <asp:TextBox ID="tbScheduleNo" runat="server" AutoPostBack="True" CssClass="TextBoxR" MaxLength="20" ValidationGroup="Input" Width="137px" />
                <asp:Button ID="btnSchedule" runat="server" class="btngo" Text="..." 
                    ValidationGroup="Input" />
            </td>            
              <td class="style6">Organization</td>
              <td>:</td>
              <td><asp:DropDownList ID="ddlDepartment" runat="server" CssClass="DropDownList" ValidationGroup="Input" />
              </td>              
          </tr>
        <tr>
            <td class="style6">Training Name</td>
            <td>:</td>
            <td ><asp:TextBox ID="tbCourseTitle" runat="server" CssClass="TextBox" 
                    MaxLength="60" ValidationGroup="Input" Width="298px" />
            </td>
              <td class="style6">Training Type</td>
              <td>:</td>
              <td><asp:DropDownList ID="ddlTraining" runat="server" CssClass="DropDownList" ValidationGroup="Input" />
              </td>
        </tr>
        <tr>      
              <td class="style6">Training Place</td>
              <td>:</td>
              <td><asp:DropDownList ID="ddlTrainingPlace" runat="server" CssClass="DropDownList" ValidationGroup="Input">
                              <asp:ListItem>In House</asp:ListItem>
                              <asp:ListItem>Out House</asp:ListItem>
                          </asp:DropDownList>
              </td>          
              <td class="style6">Training Location</td>
              <td>:</td>
              <td><asp:TextBox ID="tbTrainingLocation" runat="server" CssClass="TextBox" MaxLength="60" ValidationGroup="Input" Width="225px" />
              </td>
          </tr>
          <tr>
              <td class="style6">Sasaran</td>
              <td>:</td>
              <td colspan="4"><asp:TextBox ID="tbSasaran" runat="server" CssClass="TextBoxMulti" MaxLength="255" ValidationGroup="Input" Width="488px" TextMode="MultiLine" />
              </td>
          </tr>
          <tr>
              <td class="style6">Materi</td>
              <td>:</td>
              <td colspan="4"><asp:TextBox ID="tbMateri" runat="server" CssClass="TextBoxMulti" MaxLength="255" ValidationGroup="Input" Width="488px" TextMode="MultiLine" />
              </td>
          </tr>
          <tr>
              <td class="style6">Institution</td>
              <td>:</td>
              <td>
                  <asp:DropDownList ID="ddlInstitution" runat="server" CssClass="DropDownList" ValidationGroup="Input" />
              </td>
              <td class="style6">
                  Instructor</td>
              <td>:</td>
              <td ><asp:TextBox ID="tbInstructor" runat="server" CssClass="TextBox" MaxLength="60" ValidationGroup="Input" Width="308px" />
              </td>
          </tr>
          <tr>
              <td class="style6">Start & End Date</td>
              <td>:</td>
              <td><BDP:BasicDatePicker ID="tbStartDate" runat="server" ButtonImageHeight="19px" 
                      ButtonImageWidth="20px" DateFormat="dd MMM yyyy" DisplayType="TextBoxAndImage" 
                      ReadOnly="true" ShowNoneButton="False" TextBoxStyle-CssClass="TextDate" 
                      ValidationGroup="Input"><TextBoxStyle CssClass="TextDate" /></BDP:BasicDatePicker>
                   <BDP:BasicDatePicker ID="tbEndDate" runat="server" ButtonImageHeight="19px" 
                      ButtonImageWidth="20px" DateFormat="dd MMM yyyy" DisplayType="TextBoxAndImage" 
                      ReadOnly="true" ShowNoneButton="False" TextBoxStyle-CssClass="TextDate" 
                      ValidationGroup="Input"><TextBoxStyle CssClass="TextDate" /></BDP:BasicDatePicker>
              </td>          
              <td class="style6">Training Cost&nbsp;(<asp:Label ID="lblCurr" runat="server" Text=""></asp:Label>)
              </td>
              <td>:</td>
              <td><asp:TextBox ID="tbTrainingCost" runat="server" CssClass="TextBox" MaxLength="60" ValidationGroup="Input" Width="101px" />
              </td>
         </tr>
         <tr>     
              <td class="style6">Sertification</td>
              <td>:</td>
              <td><asp:DropDownList ID="ddlFgSertifikat" runat="server" 
                      CssClass="DropDownList" ValidationGroup="Input" >
                  <asp:ListItem Selected="True">Y</asp:ListItem>
                  <asp:ListItem>N</asp:ListItem>
                  </asp:DropDownList>
              </td>          
              <td class="style6">Participant</td>
              <td>:</td>
              <td><asp:TextBox ID="tbQty" runat="server" CssClass="TextBoxR" MaxLength="60" ValidationGroup="Input" Width="57px" /> Person              
              </td>
          </tr>
          <tr>
              <td class="style6">Remark</td>
              <td>:</td>
              <td colspan="2" style="margin-left: 80px">
                  <asp:TextBox ID="tbRemark" runat="server" CssClass="TextBoxMulti" MaxLength="255" 
                      ValidationGroup="Input" Width="360px" TextMode="MultiLine" />
              </td>
              <td colspan="2" style="margin-left: 80px">
                  <asp:Button ID="btnGetData" runat="server" class="bitbtn btngetitem" 
                      Text="Get Data" ValidationGroup="Input" />
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
                                        <asp:Button ID="btnEdit" runat="server" class="bitbtndt btnedit" Text="Edit" CommandName="Edit"/>
                                        <asp:Button ID="btnDelete" runat="server" class="bitbtndt btndelete" Text="Delete" OnClientClick="return confirm('Sure to delete this data?');" CommandName="Delete"/>						                                      
                                      </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate>                               
                                      <asp:Button Class="bitbtndt btngetitem"  ID="btnClosing" runat="server" Text="Closing" CommandArgument='<%# Container.DataItemIndex %>' CommandName="Closing" />                                    
                                </ItemTemplate>
                        </asp:TemplateField> 
                            <asp:BoundField DataField="EmpNumb" HeaderText="Employee No" HeaderStyle-Width="100px" />   
                            <asp:BoundField DataField="Emp_Name" HeaderText="Employee Name" HeaderStyle-Width="260px"/>
                            <asp:BoundField DataField="Job_Title_Name" HeaderText="Job Title" HeaderStyle-Width="200px"/>
                            <asp:BoundField DataField="Remark" HeaderText="Remark" HeaderStyle-Width="300px"/>                            
                        </Columns>
                    </asp:GridView>
              </div>   
              <asp:Button class="bitbtndt btnadd" runat="server" ID="btnAddDtke2" Text="Add" ValidationGroup="Input" />
              </asp:Panel> 
              <asp:Panel runat="server" ID="pnlEditDt" Visible="false">
                <table style="width: 681px">    
                    <tr>
                        <td class="style7">Employee</td>
                        <td>:</td>
                        <td><asp:TextBox CssClass="TextBox" ValidationGroup="Input" 
                                        runat="server" ID="tbEmpNo" MaxLength="15" Width="125px" AutoPostBack="True" />
                                        <asp:TextBox CssClass="TextBoxR" ValidationGroup="Input" 
                                        runat="server" ID="tbEmpName" MaxLength="100" Width="318px" />
                                        <asp:Button class="btngo" runat="server" ID="btnEmp" Text="..."/>
                        </td>
                    </tr>   
                    <tr>
                        <td class="style7">Job Title</td>
                        <td>:</td>
                        <td><asp:DropDownList ValidationGroup="Input" CssClass="DropDownList" runat="server" ID="ddlJobTitle" Enabled = "false"/>
                        </td>
                    </tr>       
                    <tr>
                        <td class="style7">Remark</td>
                        <td>:</td>
                        <td class="style1" style="margin-left: 40px">
                            <asp:TextBox ID="tbRemarkDt" runat="server" CssClass="TextBoxMulti" MaxLength="255" ValidationGroup="Input" Width="445px" TextMode="MultiLine" />
                        </td>
                    </tr>
                </table>
                <br />           
                <asp:Button ID="btnSaveDt" runat="server" class="bitbtndt btnsave" Text="Save"/> &nbsp;         
                <asp:Button ID="btnCancelDt" runat="server" class="bitbtndt btncancel" Text="Cancel"/> 
           </asp:Panel> 
       <br />      
        <asp:Button ID="btnSaveAll" runat="server" class="bitbtndt btnsave" Text="Save & New" ValidationGroup="Input" Width="97px"/> 
        &nbsp;    
        <asp:Button ID="btnSaveTrans" runat="server" class="bitbtndt btnsave" Text="Save" ValidationGroup="Input"/> 
        &nbsp;    
        <asp:Button ID="btnBack" runat="server" class="bitbtndt btncancel" Text="Cancel" ValidationGroup="Input"/>  
        &nbsp;
        <asp:Button ID="btnHome" runat="server" class="btngo" Text="Home" Width="48px"/>    
    </asp:Panel>        
    </div>
   
    <asp:Label runat ="server" ID="lbStatus" ForeColor="Red"/>
    <asp:HiddenField ID="HiddenRemarkClose" runat="server" />
    </form>
</body>
</html>
