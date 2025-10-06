<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TrTrainEvaluasi.aspx.vb" Inherits="Transaction_TrTrainEvaluasi_TrTrainEvaluasi" %>

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
 <link href="https://fonts.googleapis.com/css2?family=Muli:wght@300;400;600&display=swap" rel="stylesheet">
    </head>
<body>
    <form id="form1" runat="server">
    <div class="Content">
    <div class="H1">Training Evaluasi</div>
     <hr style="color:Blue" />        
    <asp:Panel runat="server" id="PnlHd"> 
      <table>
        <tr>
            <td style="width:100px;text-align:right">Quick Search :</td>         
            <td><asp:TextBox CssClass="TextBox" runat="server" ID ="tbFilter"/> 
                  <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlField" >     
                    <asp:ListItem Selected="True" Value="TransNmbr">Evaluation No</asp:ListItem>
                    <asp:ListItem Value="dbo.FormatDate(TransDate)">Evaluation Date</asp:ListItem>
                    <asp:ListItem>Status</asp:ListItem>
                    <asp:ListItem Value="ResultNo">Result No</asp:ListItem>
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
                    <asp:ListItem Value="EvaluasionBy">Evaluation By</asp:ListItem>
                    <asp:ListItem Value="EvaluasionByName">Evaluation Name By</asp:ListItem>
                    <asp:ListItem Value="EvaluasiJobTtlName">Evaluation Job Title</asp:ListItem>
                    <asp:ListItem Value="EvaluationType">Evaluation Type</asp:ListItem>
                    <asp:ListItem Value="Suggestion">Suggestion</asp:ListItem>
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
                    <asp:ListItem Selected="True" Value="TransNmbr">Evaluation No</asp:ListItem>
                    <asp:ListItem Value="dbo.FormatDate(TransDate)">Evaluation Date</asp:ListItem>
                    <asp:ListItem>Status</asp:ListItem>
                    <asp:ListItem Value="ResultNo">Result No</asp:ListItem>
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
                    <asp:ListItem Value="EvaluasionBy">Evaluation By</asp:ListItem>
                    <asp:ListItem Value="EvaluasionByName">Evaluation Name By</asp:ListItem>
                    <asp:ListItem Value="EvaluasiJobTtlName">Evaluation Job Title</asp:ListItem>
                    <asp:ListItem Value="EvaluationType">Evaluation Type</asp:ListItem>
                    <asp:ListItem Value="Suggestion">Suggestion</asp:ListItem>
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
                          <asp:CheckBox ID="cbSelectHd" runat="server" AutoPostBack="true" oncheckedchanged="cbSelectHd_CheckedChanged" />
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
                  <asp:BoundField DataField="TransNmbr" HeaderStyle-Width="120px" SortExpression="Nmbr" HeaderText="Evaluation No"></asp:BoundField>                  
                  <asp:BoundField DataField="Status" SortExpression="Status" HeaderText="Status"></asp:BoundField>                  
                  <asp:BoundField DataField="TransDate" dataformatstring="{0:dd MMM yyyy}" htmlencode="true" HeaderStyle-Width="80px" SortExpression="TransDate" HeaderText="Evaluation Date"></asp:BoundField>
                  <asp:BoundField DataField="ResultNo" HeaderStyle-Width="120px" SortExpression="ResultNo" HeaderText="Result No"></asp:BoundField>
                  <asp:BoundField DataField="CourseTitle" HeaderStyle-Width="200px" SortExpression="CourseTitle" HeaderText="Training Name"></asp:BoundField>
                  <asp:BoundField DataField="TrainingName" HeaderStyle-Width="120px" SortExpression="TrainingName" HeaderText="Training Type"></asp:BoundField>
                  <asp:BoundField DataField="TrainingPlace" HeaderStyle-Width="120px" SortExpression="TrainingPlace" HeaderText="Training Place"></asp:BoundField>
                  <asp:BoundField DataField="TrainingLocation" HeaderStyle-Width="120px" SortExpression="TrainingLocation" HeaderText="Training Location"></asp:BoundField>
                  <asp:BoundField DataField="Sasaran" HeaderStyle-Width="200px" SortExpression="Sasaran" HeaderText="Sasaran"></asp:BoundField>
                  <asp:BoundField DataField="Materi" HeaderStyle-Width="200px" SortExpression="Materi" HeaderText="Materi"></asp:BoundField>
                  <asp:BoundField DataField="InstitutionName" HeaderStyle-Width="120px" SortExpression="InstitutionName" HeaderText="Institution"></asp:BoundField>
                  <asp:BoundField DataField="Instructor" HeaderStyle-Width="120px" SortExpression="Instructor" HeaderText="Instructor"></asp:BoundField>
                  <asp:BoundField DataField="StartDate" dataformatstring="{0:dd MMM yyyy}" htmlencode="true" HeaderStyle-Width="80px" SortExpression="StartDate" HeaderText="Start Date"></asp:BoundField>
                  <asp:BoundField DataField="EndDate" dataformatstring="{0:dd MMM yyyy}" htmlencode="true" HeaderStyle-Width="80px" SortExpression="EndDate" HeaderText="End Date"></asp:BoundField>
                  <asp:BoundField DataField="EvaluasionBy" HeaderStyle-Width="120px" SortExpression="EvaluasionBy" HeaderText="Evaluation By"></asp:BoundField>
                  <asp:BoundField DataField="EvaluasionByName" HeaderStyle-Width="200px" SortExpression="EvaluasionByName" HeaderText="Evaluation Name By"></asp:BoundField>
                  <asp:BoundField DataField="EvaluasiJobTtlName" HeaderStyle-Width="120px" SortExpression="EvaluasiJobTtlName" HeaderText="Evaluation Job Title Name"></asp:BoundField>
                  <asp:BoundField DataField="EvaluationType" HeaderStyle-Width="120px" SortExpression="EvaluationType" HeaderText="Evaluation Type"></asp:BoundField>
                  <asp:BoundField DataField="Suggestion" HeaderStyle-Width="250px" SortExpression="Suggestion" HeaderText="Suggestion"></asp:BoundField>
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
      <table style="width: 677px">
        <tr>
            <td>Evaluation No</td>
            <td>:</td>
            <td><asp:TextBox CssClass="TextBoxR" runat="server" ID="tbCode" Width="150px" Enabled="False"/></td>
            <td>&nbsp &nbsp</td>
            <td>Evaluation Date</td>
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
            <td>Result No</td>
            <td>:</td>
            <td colspan="5"><asp:TextBox ID="tbResultNo" runat="server" AutoPostBack="True" CssClass="TextBoxR" MaxLength="20" ValidationGroup="Input" Width="137px" />
                <asp:Button ID="btnResult" runat="server" class="btngo" Text="..." ValidationGroup="Input" />
            </td>            
        </tr>        
        <tr>
            <td>Training Name</td>
            <td>:</td>
            <td><asp:TextBox ID="tbCourseTitle" runat="server" CssClass="TextBoxR" MaxLength="60" ValidationGroup="Input" Width="280px" />
            </td>   
            <td></td>         
            <td>Training Type</td>
            <td>:</td>
            <td><asp:DropDownList ID="ddlTraining" runat="server" CssClass="DropDownList" Enabled="False" />
            </td>
        </tr>
        <tr>
            <td>Training Place</td>
            <td>:</td>
            <td>
                <asp:DropDownList ID="ddlTrainingPlace" runat="server" CssClass="DropDownList" Enabled="False">
                <asp:ListItem>In House</asp:ListItem>
                <asp:ListItem>Out House</asp:ListItem>
                </asp:DropDownList>
            </td>   
            <td></td>     
            <td>Training Location</td>
            <td>:</td>
            <td><asp:TextBox ID="tbTrainingLocation" runat="server" CssClass="TextBoxR" MaxLength="60" ValidationGroup="Input" Width="225px" />
            </td>
          </tr>
          <tr>
              <td>Sasaran</td>
              <td>:</td>
              <td>
                <asp:TextBox ID="tbSasaran" runat="server" CssClass="TextBoxMultiR" MaxLength="255" ValidationGroup="Input" Width="320px" TextMode="MultiLine"  />
              </td>     
              <td></td>     
              <td>Materi</td>
              <td>:</td>
              <td>
                <asp:TextBox ID="tbMateri" runat="server" CssClass="TextBoxMultiR" MaxLength="255" ValidationGroup="Input" Width="320px" TextMode="MultiLine"  />
              </td>
          </tr>
          <tr>
              <td>Institution</td>
              <td>:</td>
              <td>
                <asp:DropDownList ID="ddlInstitution" runat="server" CssClass="DropDownList" 
                      Enabled = "false" />
              </td>
              <td></td>
              <td>Instructor</td>
              <td>:</td>
              <td>
                  <asp:TextBox ID="tbInstructor" runat="server" CssClass="TextBoxR" MaxLength="60" ValidationGroup="Input" Width="308px" />
              </td>
          </tr>
          <tr>
              <td>Start Date</td>
              <td>:</td>
              <td><BDP:BasicDatePicker ID="tbStartDate" runat="server" ButtonImageHeight="19px" 
                      ButtonImageWidth="20px" DateFormat="dd MMM yyyy" DisplayType="TextBoxAndImage" 
                      ReadOnly="true" ShowNoneButton="False" TextBoxStyle-CssClass="TextDate" 
                      Enabled="False"><TextBoxStyle CssClass="TextDate" /></BDP:BasicDatePicker>
              </td>
              <td></td>
              <td>End Date</td>
              <td>:</td>
              <td>
                  <BDP:BasicDatePicker ID="tbEndDate" runat="server" ButtonImageHeight="19px" 
                      ButtonImageWidth="20px" DateFormat="dd MMM yyyy" DisplayType="TextBoxAndImage" 
                      ReadOnly="true" ShowNoneButton="False" TextBoxStyle-CssClass="TextDate" 
                      Enabled="False"><TextBoxStyle CssClass="TextDate"/></BDP:BasicDatePicker>
              </td>
          </tr>
          <tr>
              <td>Evaluation By</td>
              <td>:</td>
              <td colspan="5">
                <asp:TextBox ID="tbEvaluasionBy" runat="server" CssClass="TextBox" MaxLength="15" ValidationGroup="Input" Width="101px" AutoPostBack ="true"/>
                <asp:TextBox ID="tbEvaluasionByName" runat="server" CssClass="TextBoxR" 
                      MaxLength="100" ValidationGroup="Input" Width="280px" />
                  <asp:Button ID="btnEvaluationBy" runat="server" class="btngo" Text="..." ValidationGroup="Input" />
              </td>
          </tr>
          <tr>
              <td>Job Title</td>
              <td>:</td>
              <td>
                  <asp:DropDownList ID="ddlJobTitleHd" runat="server" CssClass="DropDownList" Height="16px" Width="180px" Enabled = "false" />
              </td>   
              <td></td>       
              <td>Evaluation Type</td>
              <td>:</td>
              <td>
                  <asp:DropDownList ID="ddlEvaluationType" runat="server" CssClass="DropDownList" ValidationGroup="Input">
                      <asp:ListItem Selected="True">Effective</asp:ListItem>
                      <asp:ListItem>Non Effective</asp:ListItem>
                  </asp:DropDownList>
              </td>
          </tr>
          <tr>
              <td>Suggestion</td>
              <td>:</td>
              <td>
                <asp:TextBox ID="tbSuggestion" runat="server" CssClass="TextBoxMulti" MaxLength="255" TextMode="MultiLine" 
                      ValidationGroup="Input" Width="320px"/>
              </td>
               <td></td>
              <td>Remark</td>
              <td>:</td>
              <td>
                  <asp:TextBox ID="tbRemark" runat="server" CssClass="TextBoxMulti" MaxLength="255" TextMode="MultiLine" 
                      ValidationGroup="Input" Width="320px" />
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
                            <asp:BoundField DataField="EmpNumb" HeaderText="Employee No" HeaderStyle-Width="100px" />   
                            <asp:BoundField DataField="Emp_Name" HeaderText="Employee Name" HeaderStyle-Width="250px"/>
                            <asp:BoundField DataField="DeptName" HeaderText="Organization" HeaderStyle-Width="160px"/>
                            <asp:BoundField DataField="Job_Title" HeaderText="Job Title" HeaderStyle-Width="160px"/>                            
                            <asp:BoundField DataField="AfterTraining" HeaderText="After Training" />
                            <asp:BoundField DataField="Conclusion" HeaderText="Conclusion" HeaderStyle-Width="300px"/>
                        </Columns>
                    </asp:GridView>
              </div>   
              <asp:Button class="bitbtndt btnadd" runat="server" ID="btnAddDtke2" Text="Add" ValidationGroup="Input" />
              </asp:Panel> 
              <asp:Panel runat="server" ID="pnlEditDt" Visible="false">
                <table style="width: 681px">    
                    <tr>
                        <td>Employee</td>
                        <td>:</td>
                        <td><asp:TextBox CssClass="TextBox" ValidationGroup="Input" 
                                        runat="server" ID="tbEmpNo" MaxLength="15" Width="125px" AutoPostBack="True" />
                                        <asp:TextBox CssClass="TextBoxR" ValidationGroup="Input" 
                                        runat="server" ID="tbEmpName" MaxLength="100" Width="318px" />
                                        <asp:Button class="btngo" runat="server" ID="btnEmp" Text="..."/>
                        </td>
                    </tr>   
                    <tr>
                        <td>Organization</td>
                        <td>:</td>
                        <td><asp:DropDownList ValidationGroup="Input" CssClass="DropDownList" runat="server" ID="ddlDepartment" Enabled = "false">
                            </asp:DropDownList>
                        </td>
                    </tr>       
                    <tr>
                        <td>Job Title</td>
                        <td>:</td>
                        <td><asp:DropDownList ValidationGroup="Input" CssClass="DropDownList" 
                                runat="server" ID="ddlJobTitleDt" Enabled = "false">
                            </asp:DropDownList>
                        </td>
                    </tr>       
                    <tr>
                        <td>After Training</td>
                        <td>:</td>
                        <td><asp:DropDownList ValidationGroup="Input" CssClass="DropDownList" 
                                runat="server" ID="ddlAfterTraining">
                            <asp:ListItem>1</asp:ListItem>
                            <asp:ListItem>2</asp:ListItem>
                            <asp:ListItem>3</asp:ListItem>
                            <asp:ListItem>4</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>Conclusion</td>
                        <td>:</td>
                        <td>
                            <asp:TextBox ID="tbConclusion" runat="server" CssClass="TextBoxMulti" 
                                MaxLength="255" TextMode="MultiLine" ValidationGroup="Input" Width="456px" />
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
    </form>
</body>
</html>
