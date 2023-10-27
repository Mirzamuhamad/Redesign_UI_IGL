<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TrTrainSchedule.aspx.vb" Inherits="Transaction_TrTrainSchedule_TrTrainSchedule" %>

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
    <div class="H1">Training Schedule</div>
     <hr style="color:Blue" />        
    <asp:Panel runat="server" id="PnlHd"> 
      <table>
        <tr>
            <td style="width:100px;text-align:right">Quick Search :</td>         
            <td><asp:TextBox CssClass="TextBox" runat="server" ID ="tbFilter"/> 
                  <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlField" >     
                    <asp:ListItem Selected="True" Value="TransNmbr">Schedule No</asp:ListItem>
                    <asp:ListItem Value="dbo.FormatDate(TransDate)">Schedule Date</asp:ListItem>
                    <asp:ListItem>Status</asp:ListItem>
                    <asp:ListItem Value="Year">Year</asp:ListItem>
                    <asp:ListItem Value="WorkPlaceName">Work Place</asp:ListItem>
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
                    <asp:ListItem Selected="True" Value="TransNmbr">Schedule No</asp:ListItem>
                    <asp:ListItem Value="dbo.FormatDate(TransDate)">Schedule Date</asp:ListItem>
                    <asp:ListItem>Status</asp:ListItem>
                    <asp:ListItem Value="Year">Year</asp:ListItem>
                    <asp:ListItem Value="WorkPlaceName">Work Place</asp:ListItem>
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
                  <asp:BoundField DataField="TransNmbr" HeaderStyle-Width="120px" SortExpression="Nmbr" HeaderText="Schedule No"></asp:BoundField>                  
                  <asp:BoundField DataField="Status" SortExpression="Status" HeaderText="Status"></asp:BoundField>                  
                  <asp:BoundField DataField="TransDate" dataformatstring="{0:dd MMM yyyy}" htmlencode="true" HeaderStyle-Width="80px" SortExpression="TransDate" HeaderText="Schedule Date"></asp:BoundField>
                  <asp:BoundField DataField="Year" HeaderStyle-Width="80px" SortExpression="Year" HeaderText="Year"></asp:BoundField>
                  <asp:BoundField DataField="WorkPlaceName" HeaderStyle-Width="200px" SortExpression="WorkPlaceName" HeaderText="Work Place"></asp:BoundField>
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
            <td class="style6">Schedule No</td>
            <td>:</td>
            <td><asp:TextBox CssClass="TextBoxR" runat="server" ID="tbCode" Width="150px" Enabled="False"/></td>
            
            <td>Schedule Date</td>
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
            <td class="style6">Year</td>
            <td>:</td>
            <td><asp:DropDownList runat="server" ValidationGroup="Input" ID="ddlYear" CssClass="DropDownList" AutoPostBack="True"/>
            </td>        
            <td class="style6">Work Place</td>
            <td>:</td>
            <td><asp:DropDownList runat="server" ValidationGroup="Input" ID="ddlWorkPlace" CssClass="DropDownList" Width="180px"/>
            </td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
        </tr>
        <tr>
            <td class="style6">Remark</td>
            <td>:</td>
            <td style="margin-left: 80px" colspan="4"><asp:TextBox ID="tbRemark" runat="server" CssClass="TextBoxMulti" ValidationGroup="Input" Width="350px" MaxLength="255" TextMode="MultiLine" />
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
                            <asp:BoundField DataField="IdentificateNo" HeaderText="Identification No" />   
                            <asp:BoundField DataField="Department_Name" HeaderText="Organization" />
                            <asp:BoundField DataField="CourseTitle" HeaderText="Training Name" />
                            <asp:BoundField DataField="TrainingName" HeaderText="Training Type" />
                            <asp:BoundField DataField="TrainingPlace" HeaderStyle-Width="100px" HeaderText="Training Place" />
                            <asp:BoundField DataField="InstitutionName" HeaderStyle-Width="100px" HeaderText="Institution" />
                            <asp:BoundField DataField="Instructor" HeaderStyle-Width="150px" HeaderText="Instructor" />
                            <asp:BoundField DataField="Currency" HeaderStyle-Width="100px" HeaderText="Currency" />
                            <asp:BoundField DataField="CostPerson" HeaderStyle-Width="80px" HeaderText="CostPerson" />
                            <asp:BoundField DataField="EstMonth" HeaderStyle-Width="80px" HeaderText="Est Month" />
                            <asp:BoundField DataField="Participant" HeaderStyle-Width="100px" HeaderText="Participant" />
                            <asp:BoundField DataField="Sasaran" HeaderStyle-Width="200px" HeaderText="Sasaran" />
                            <asp:BoundField DataField="Materi" HeaderStyle-Width="80px" HeaderText="Materi" />
                            <asp:BoundField DataField="Prioritas" HeaderStyle-Width="80px" HeaderText="Priority" />
                            <asp:BoundField DataField="Remark" HeaderStyle-Width="200px" HeaderText="Remark" />
                        </Columns>
                    </asp:GridView>
              </div>   
              <asp:Button class="bitbtndt btnadd" runat="server" ID="btnAddDtke2" Text="Add" ValidationGroup="Input" />
              </asp:Panel> 
              <asp:Panel runat="server" ID="pnlEditDt" Visible="false">
                <table style="width: 583px">    
                    <tr>
                        <td class="style7">Identification No</td>
                        <td>:</td>
                        <td><asp:TextBox CssClass="TextBox" ValidationGroup="Input" runat="server" ID="tbNoIden" MaxLength="60" Width="137px" 
                                AutoPostBack="True" />
                        <asp:Button class="btngo" runat="server" ID="btnNoIden" Text="..."/>
                        </td>
                        <td class="style7">Organization</td>
                        <td>:</td>
                        <td><asp:DropDownList ValidationGroup="Input" CssClass="DropDownList" runat="server" ID="ddlDepartment" Enabled = "false"/>
                        </td>
                    </tr>       
                    <tr>
                        <td class="style7">Training Name</td>
                        <td>:</td>
                        <td colspan="4"><asp:TextBox CssClass="TextBoxR" ValidationGroup="Input" runat="server" ID="tbCourseTitle" MaxLength="60" Width="380px" />
                            <asp:Button ID="btnCourse" runat="server" class="btngo" Text="..." />
                        </td>
                    </tr>
                    <tr>                    
                        <td class="style7">Training Type</td>
                        <td>:</td>
                        <td class="style1"><asp:DropDownList ValidationGroup="Input" CssClass="DropDownList" runat="server" ID="ddlTrainingType" Enabled = "false"/>                                
                        </td>
                        <td class="style5">Training Place</td>
                        <td>:</td>
                        <td class="style8">
                            <asp:DropDownList ID="ddlTrainingPlace" runat="server" CssClass="DropDownList" ValidationGroup="Input" Enabled = "false">
                                <asp:ListItem>In House</asp:ListItem>
                                <asp:ListItem>Out House</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>                    
                        <td class="style7">Institution</td>
                        <td>:</td>
                        <td style="margin-left: 40px">
                            <asp:DropDownList runat="server" ValidationGroup="Input" ID="ddlInstitution" CssClass="DropDownList" Enabled = "false" Width="180px" />
                        </td>                    
                        <td class="style7">Instructor</td>
                        <td>:</td>
                        <td style="margin-left: 40px">
                            <asp:TextBox ID="tbInstructor" runat="server" CssClass="TextBoxR" MaxLength="60" ValidationGroup="Input" Width="220px" />
                        </td>
                    </tr>
                    <tr>
                        <td class="style7">Currency</td>
                        <td>:</td>
                        <td class="style1" style="margin-left: 40px">
                            <asp:DropDownList ID="ddlCurrency" runat="server" CssClass="DropDownList" 
                                ValidationGroup="Input" AutoPostBack="True" Enabled = "false"/>
                        </td>
                        <td class="style5" style="margin-left: 40px">Est Cost</td>
                        <td>:</td>
                        <td class="style8">
                            <asp:TextBox ID="tbEstCost" runat="server" CssClass="TextBoxR" MaxLength="60" ValidationGroup="Input" Width="101px" />
                            &nbsp Est Month :
                            <asp:DropDownList ID="ddlEstMonth" runat="server" CssClass="DropDownList" ValidationGroup="Input" Enabled = "false"/>                                                
                         </td>
                    </tr>
                    <tr>
                        <td class="style7">Participant</td>
                        <td>:</td>
                        <td class="style1" style="margin-left: 40px" colspan="4">
                            <asp:TextBox ID="tbParticipant" runat="server" CssClass="TextBoxR" MaxLength="255" ValidationGroup="Input" Width="445px" Height="19px"/>
                        </td>
                    </tr>                        
                    <tr>
                        <td class="style7">Sasaran</td>
                        <td>:</td>
                        <td class="style1" colspan="4" style="margin-left: 40px">
                            <asp:TextBox ID="tbSasaran" runat="server" CssClass="TextBoxMultiR" MaxLength="255" ValidationGroup="Input" Width="445px" TextMode="MultiLine" />
                        </td>
                    </tr>
                    <tr>
                        <td class="style7">Materi</td>
                        <td>:</td>
                        <td class="style1" colspan="4" style="margin-left: 40px">
                            <asp:TextBox ID="tbMateri" runat="server" CssClass="TextBoxMultiR" MaxLength="255" ValidationGroup="Input" Width="445px" TextMode="MultiLine"/>
                        </td>
                    </tr>
                    <tr>
                        <td class="style7">Priority</td>
                        <td>:</td>
                        <td colspan="4"><asp:DropDownList ValidationGroup="Input" CssClass="DropDownList" 
                                runat="server" ID="ddlPrioriry" >
                            <asp:ListItem>A</asp:ListItem>
                            <asp:ListItem>B</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="style7">Remark</td>
                        <td>:</td>
                        <td class="style1" colspan="4" style="margin-left: 40px">
                            <asp:TextBox ID="tbRemarkDt" runat="server" CssClass="TextBoxMulti" MaxLength="255" ValidationGroup="Input" Width="445px" TextMode="MultiLine"/>
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
