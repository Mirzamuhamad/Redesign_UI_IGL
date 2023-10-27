<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TrTrainIdentifikasi.aspx.vb" Inherits="Transaction_TrTrainIdentifikasi_TrTrainIdentifikasi" %>

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
            width: 100px;
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
    <div class="H1">Training Identification</div>
     <hr style="color:Blue" />        
    <asp:Panel runat="server" id="PnlHd"> 
      <table>
        <tr>
            <td style="width:100px;text-align:right">Quick Search :</td>         
            <td><asp:TextBox CssClass="TextBox" runat="server" ID ="tbFilter"/> 
                  <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlField" >     
                    <asp:ListItem Selected="True" Value="TransNmbr">Train No</asp:ListItem>
                    <asp:ListItem Value="dbo.FormatDate(TransDate)">Train Date</asp:ListItem>
                    <asp:ListItem>Status</asp:ListItem>
                    <asp:ListItem Value="Department_Name">Organization</asp:ListItem>
                    <asp:ListItem Value="Year">Year</asp:ListItem>
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
                    <asp:ListItem Selected="True" Value="TransNmbr">Train No</asp:ListItem>
                    <asp:ListItem Value="dbo.FormatDate(TransDate)">Train Date</asp:ListItem>
                    <asp:ListItem>Status</asp:ListItem>
                    <asp:ListItem Value="Department_Name">Organization</asp:ListItem>
                    <asp:ListItem Value="Year">Year</asp:ListItem>
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
                  <asp:BoundField DataField="TransNmbr" HeaderStyle-Width="120px" SortExpression="Nmbr" HeaderText="Train No"></asp:BoundField>                  
                  <asp:BoundField DataField="Status" SortExpression="Status" HeaderText="Status"></asp:BoundField>                  
                  <asp:BoundField DataField="TransDate" dataformatstring="{0:dd MMM yyyy}" htmlencode="true" HeaderStyle-Width="80px" SortExpression="TransDate" HeaderText="Train Date"></asp:BoundField>
                  <asp:BoundField DataField="Department_Name" HeaderStyle-Width="200px" SortExpression="Department_Name" HeaderText="Organization"></asp:BoundField>
                  <asp:BoundField DataField="Group_Code" HeaderStyle-Width="200px" SortExpression="Group_Code" HeaderText="Department" Visible = "false"></asp:BoundField>
                  <asp:BoundField DataField="Year" HeaderStyle-Width="80px" SortExpression="Year" HeaderText="Year"></asp:BoundField>
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
            <td>Train No</td>
            <td>:</td>
            <td><asp:TextBox CssClass="TextBoxR" runat="server" ID="tbCode" Width="150px" Enabled="False"/></td>
            
            <td>Train Date</td>
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
            <td>Organization</td>
            <td>:</td>
            <td><asp:DropDownList runat="server" ValidationGroup="Input" ID="ddlDepartment" CssClass="DropDownList"/>
            </td>            
            <td>Year</td>
            <td>:</td>
            <td><asp:DropDownList runat="server" ValidationGroup="Input" ID="ddlYear" CssClass="DropDownList" AutoPostBack="True"/>
            </td>
        </tr>
          <tr>
              <td>Remark</td>
              <td>:</td>
              <td colspan="4"><asp:TextBox ID="tbRemark" runat="server" CssClass="TextBoxMulti" TextMode="MultiLine" ValidationGroup="Input" Width="360px" MaxLength="255" />
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
                            <asp:BoundField DataField="CourseTitle" HeaderText="Training Name" />
                            <asp:BoundField DataField="TrainingName" HeaderText="Training Type" />
                            <asp:BoundField DataField="TrainingPlace" HeaderStyle-Width="100px" HeaderText="Training Place" />
                            <asp:BoundField DataField="InstitutionName" HeaderStyle-Width="100px" HeaderText="Institution" />
                            <asp:BoundField DataField="Instructor" HeaderStyle-Width="150px" HeaderText="Instructor" />
                            <asp:BoundField DataField="Currency" HeaderStyle-Width="100px" HeaderText="Currency" />
                            <asp:BoundField DataField="Rate" HeaderStyle-Width="80px" HeaderText="Rate" />
                            <asp:BoundField DataField="CostPerson" HeaderStyle-Width="80px" HeaderText="CostPerson" />
                            <asp:BoundField DataField="Participant" HeaderStyle-Width="100px" HeaderText="Participant" />
                            <asp:BoundField DataField="EstMonth" HeaderStyle-Width="80px" HeaderText="Est Month" />
                            <asp:BoundField DataField="Sasaran" HeaderStyle-Width="200px" HeaderText="Sasaran" />
                            <asp:BoundField DataField="Materi" HeaderStyle-Width="80px" HeaderText="Materi" />
                            <asp:BoundField DataField="Remark" HeaderStyle-Width="200px" HeaderText="Remark" />
                        </Columns>
                    </asp:GridView>
              </div>   
              <asp:Button class="bitbtndt btnadd" runat="server" ID="btnAddDtke2" Text="Add" ValidationGroup="Input" />
              </asp:Panel> 
              <asp:Panel runat="server" ID="pnlEditDt" Visible="false">
                <table style="width: 100%">             
                    <tr>
                        <td>Training Name</td>
                        <td>:</td>
                        <td colspan="4" ><asp:TextBox CssClass="TextBox" ValidationGroup="Input" runat="server" ID="tbCourseTitle" MaxLength="60" Width="373px" />
                        </td>                        
                    </tr>
                    <tr>                    
                        <td>Training Type</td>
                        <td>:</td>
                        <td><asp:DropDownList ValidationGroup="Input" CssClass="DropDownList" runat="server" ID="ddlTrainingType" Width="180px"/>                                
                        </td>                        
                        <td style="width: 100px;">Training Place</td>
                        <td>:</td>
                        <td>
                            <asp:DropDownList ID="ddlTrainingPlace" runat="server" CssClass="DropDownList" 
                                ValidationGroup="Input">
                                <asp:ListItem>In House</asp:ListItem>
                                <asp:ListItem>Out House</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>                    
                        <td>Institution</td>
                        <td>:</td>
                        <td>
                            <asp:DropDownList runat="server" ValidationGroup="Input" ID="ddlInstitution" CssClass="DropDownList" Width="180px"/>
                        </td>                    
                        <td style="width: 100px">Instructor</td>
                        <td>:</td>
                        <td>
                            <asp:TextBox ID="tbInstructor" runat="server" CssClass="TextBox" MaxLength="60" ValidationGroup="Input" Width="280px" />
                        </td>
                    </tr>
                    <tr>
                        <td class="style7">Currency</td>
                        <td>:</td>
                        <td class="style1" style="margin-left: 40px">
                            <asp:DropDownList ID="ddlCurrency" runat="server" CssClass="DropDownList" 
                                ValidationGroup="Input" AutoPostBack="True" />
                            <asp:TextBox ID="tbRate" runat="server" CssClass="TextBox" ValidationGroup="Input" Width="65px" />
                        </td>
                        <td style="width: 120px">Est Cost</td>
                        <td>:</td>
                        <td>
                            <asp:TextBox ID="tbEstCost" runat="server" CssClass="TextBox" MaxLength="60" ValidationGroup="Input" Width="101px" />
                            Est Month :&nbsp 
                            <asp:DropDownList ID="ddlEstMonth" runat="server" CssClass="DropDownList" 
                                ValidationGroup="Input" />
                        </td>
                    </tr>
                    <tr>
                        <td class="style7">Participant</td>
                        <td>:</td>
                        <td colspan="4">
                            <asp:TextBox ID="tbParticipant" runat="server" CssClass="TextBoxMulti" 
                                MaxLength="255" ValidationGroup="Input" TextMode="MultiLine" Width="445px"/>
                        </td>
                    </tr>                    
                    <tr>
                        <td class="style7">
                            Sasaran</td>
                        <td>
                            :</td>
                        <td colspan="4">
                            <asp:TextBox ID="tbSasaran" runat="server" CssClass="TextBoxMulti" MaxLength="255" 
                                ValidationGroup="Input" Width="445px" TextMode="MultiLine" />
                        </td>
                    </tr>
                    <tr>
                        <td class="style7">
                            Materi</td>
                        <td>
                            :</td>
                        <td colspan="4">
                            <asp:TextBox ID="tbMateri" runat="server" CssClass="TextBoxMulti" MaxLength="255" 
                                ValidationGroup="Input" Width="445px" TextMode="MultiLine" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Remark</td>
                        <td>
                            :</td>
                        <td colspan="4">
                            <asp:TextBox ID="tbRemarkDt" runat="server" CssClass="TextBoxMulti" MaxLength="255" 
                                ValidationGroup="Input" Width="445px" TextMode="MultiLine" />
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
