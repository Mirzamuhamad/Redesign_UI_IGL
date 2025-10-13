<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TrPETerminateIn.Aspx.vb" Inherits="TrPETerminateIn" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<%@ Register Assembly="BasicFrame.WebControls.BasicDatePicker" Namespace="BasicFrame.WebControls"
    TagPrefix="BDP" %>

<%--<%@ Register assembly="CrystalDecisions.Web, Version=12.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>--%>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
    <script src="../../Function/OpenDlg.js" type="text/javascript"></script>
    <script src="../../Function/Function.JS" type="text/javascript"></script>
    <script type="text/javascript"> 
        function opencekpostingdlg() {  
                window.open("../../Transaction/TrPETerminateIn/FormCekPostPR.Aspx","List","scrollbars=yes,resizable=no,width=800,height=400");
                return false;
                          
        }           
            
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
            var Qty = document.getElementById("tbQtyreq").value.replace(/\$|\,/g,""); 
            var QtyWrhs = document.getElementById("tbQtyWrhs").value.replace(/\$|\,/g,""); 
            var TotalForex = document.getElementById("tbTotalForex").value.replace(/\$|\,/g,""); 
            var Rate = document.getElementById("tbRate").value.replace(/\$|\,/g,"");
             
            var AmountClaim = document.getElementById("tbAmountClaim").value.replace(/\$|\,/g,""); 
            var AmountPercent = document.getElementById("tbAmountPercent").value.replace(/\$|\,/g,""); 
            var AmountToPaid = document.getElementById("tbAmountToPaid").value.replace(/\$|\,/g,""); 
            var AmountPaid = document.getElementById("tbAmountPaid").value.replace(/\$|\,/g,""); 
            
            
//             var PriceForex = document.getElementById("tbPrice").value.replace(/\$|\,/g,"");
//             var AmountForex = document.getElementById("tbAmountForex").value.replace(/\$|\,/g,"");                
         try
         {                       
            document.getElementById("tbTotalForex").value = setdigit(TotalForex,'<%=ViewState("DigitHome")%>');
            document.getElementById("tbRate").value = setdigit(Rate,'<%=ViewState("DigitHome")%>');
            document.getElementById("tbAmountClaim").value = setdigit(AmountClaim,'<%=ViewState("DigitHome")%>');
            document.getElementById("tbAmountPercent").value = setdigit(AmountPercent,'<%=VIEWSTATE("DigitChargeCurr")%>');            
            document.getElementById("tbAmountToPaid").value = setdigit(AmountToPaid,'<%=ViewState("DigitHome")%>');
            document.getElementById("tbAmountPaid").value = setdigit(AmountPaid,'<%=ViewState("DigitHome")%>');
            
            
            // document.getElementById("tbPrice").value = setdigit(PriceForex,'<%=VIEWSTATE("DigitCurr")%>');
            // document.getElementById("tbAmountForex").value = setdigit(AmountForex,'<%=VIEWSTATE("DigitCurr")%>');
        }catch (err){
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
    <div class="H1">Terminate In</div>
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
                      <asp:ListItem Value="EffectiveDate">Effective Date</asp:ListItem>                
                      <asp:ListItem Value="EmpAppr">Employee Approval</asp:ListItem>                                        
                      <asp:ListItem Value="EmpAppr_Name">Employee Approval Name</asp:ListItem>                                                              
                      <asp:ListItem Value="JobTitleAppr">Job Title Approval</asp:ListItem>                                        
                      <asp:ListItem Value="JobTtlAppr_Name">Job Title Name Approval</asp:ListItem>                                                                                    
                    </asp:DropDownList>
                    <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlRange"></asp:DropDownList>                
                  <asp:Button class="bitbtn btnsearch" runat="server" ID="btnSearch" Text="Search" />																						 											  
                  <asp:Button class="bitbtn btngo" runat="server" ID="btnExpand" Text="..." />																					 											  
      
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
                      <asp:ListItem Value="EffectiveDate">Effective Date</asp:ListItem>                
                      <asp:ListItem Value="EmpAppr">Employee Approval</asp:ListItem>                                        
                      <asp:ListItem Value="EmpAppr_Name">Employee Approval Name</asp:ListItem>                                                              
                      <asp:ListItem Value="JobTitleAppr">Job Title Approval</asp:ListItem>                                        
                      <asp:ListItem Value="JobTtlAppr_Name">Job Title Name Approval</asp:ListItem>                                                                                    
              </asp:DropDownList>
          </td>              
        </tr>        
      </table>      
      </asp:Panel>
          <br />
            <asp:Button class="bitbtn btnadd" runat="server" ID="BtnAdd" Text="Add" />	           
            &nbsp &nbsp &nbsp   
            <asp:DropDownList CssClass="DropDownList" ID="ddlCommand" runat="server" Visible="false"/>
            <asp:Button class="btngo" runat="server" ID="BtnGo" Text="G" Visible="false" />                      
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
                              <%--<asp:ListItem Text="Print" />--%>
                          </asp:DropDownList>
                          <asp:Button class="btngo" runat="server" ID="BtnGo" Text="G" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" CommandName="Go" />                      </ItemTemplate>
                      <HeaderStyle Width="110px" />
                  </asp:TemplateField>
                  <asp:BoundField DataField="TransNmbr" HeaderStyle-Width="120px" SortExpression="Nmbr" HeaderText="Reference"></asp:BoundField>                  
                  <asp:BoundField DataField="Status" HeaderText="Status"></asp:BoundField>                  
                  <asp:BoundField DataField="TransDate" dataformatstring="{0:dd MMM yyyy}" htmlencode="true" HeaderStyle-Width="80px" SortExpression="TransDate" HeaderText="Date"></asp:BoundField>
                  <asp:BoundField DataField="EffectiveDate" dataformatstring="{0:dd MMM yyyy}" htmlencode="true" HeaderStyle-Width="80px" SortExpression="EffectiveDate" HeaderText="Effective Date"></asp:BoundField>                  
                  <asp:BoundField DataField="EmpAppr" HeaderStyle-Width="200px" SortExpression="EmpAppr" HeaderText="Approved By"></asp:BoundField>
                  <asp:BoundField DataField="EmpAppr_Name" HeaderStyle-Width="200px" SortExpression="EmpAppr_Name" HeaderText="Approved By Name"></asp:BoundField>
                  <asp:BoundField DataField="JobTitleAppr" HeaderStyle-Width="250px" SortExpression="JobTitleAppr" HeaderText="Job Title"></asp:BoundField>                  
                  <asp:BoundField DataField="JobTtlAppr_Name" HeaderStyle-Width="250px" SortExpression="JobTtlAppr_Name" HeaderText="Job Titel Name"></asp:BoundField>                  
                  <asp:BoundField DataField="RemarkHd" HeaderStyle-Width="250px" HeaderText="Remark"></asp:BoundField>                  
              </Columns>
          </asp:GridView>	    
          </div>
            <asp:Panel runat="server" ID ="pnlNav" Visible="false">
            <asp:Button class="bitbtn btnadd" runat="server" ID="btnAdd2" Text="Add" />	            
            &nbsp &nbsp &nbsp
            <asp:DropDownList ID="ddlCommand2" CssClass="DropDownList" runat="server"/>
            <asp:Button class="btngo" runat="server" ID="btnGo2" Text="G" />  
            </asp:Panel>
    </asp:Panel>    
    <asp:Panel runat="server" ID="pnlInput" Visible="false">
      <table>
        <tr>
            <td>Terminate Out No</td>
            <td>:</td>
            <td><asp:TextBox CssClass="TextBox" Enabled ="false" runat="server" ID="tbTransNo" Width="149px"/>
            </td>            
        </tr>
        <tr>
            <td>Terminate Out Date</td>
            <td>:</td>
            <td>
            <BDP:BasicDatePicker ID="tbDate" runat="server" DateFormat="dd MMM yyyy" 
                        ShowNoneButton ="false" ReadOnly = "true" ValidationGroup="Input"
                        ButtonImageHeight="19px" ButtonImageWidth="20px" 
                        DisplayType="TextBoxAndImage" 
                        TextBoxStyle-CssClass="TextDate" AutoPostBack="True"><TextBoxStyle CssClass="TextDate" /></BDP:BasicDatePicker>                
            </td>            
        </tr>         
        <tr>
            <td>Effective Date</td>
            <td>:</td>
            <td>
            <BDP:BasicDatePicker ID="tbEffectiveDate" runat="server" DateFormat="dd MMM yyyy" 
                        ShowNoneButton ="false" ReadOnly = "true" ValidationGroup="Input"
                        ButtonImageHeight="19px" ButtonImageWidth="20px" 
                        DisplayType="TextBoxAndImage" 
                        TextBoxStyle-CssClass="TextDate" AutoPostBack="True"><TextBoxStyle CssClass="TextDate" /></BDP:BasicDatePicker>                
            </td>            
        </tr>                                  
       
        <tr>
            <td><asp:LinkButton ID="lbApprovedBy"  runat="server" Text="Approved By"/> </td>
            <td>:</td>
            <td><asp:TextBox runat="server" ID="tbApproved" CssClass="TextBox" AutoPostBack="true" ValidationGroup="Input" />
                <asp:TextBox runat="server"  CssClass="TextBox"
                            ID="tbApprovedName" EnableTheming="True" ReadOnly="True" Enabled="False" 
                            Width="200px"/>
               <asp:Button Class="btngo" ID="btnApproved" Text="..." runat="server" ValidationGroup="Input" />                                                                             
            </td>
        </tr>        
        <tr>
            <td>Job Title</td>
            <td>:</td>
            <td>
            <asp:TextBox runat="server"  CssClass="TextBox"
                            ID="tbJobTitleName" EnableTheming="True" ReadOnly="True" Enabled="False" 
                            Width="200px"/>
            <asp:TextBox runat="server"  CssClass="TextBox"
                            ID="tbJobTitle" EnableTheming="True" ReadOnly="True" Visible="False" 
                            Width="200px"/>                            
            </td>
        </tr>        
        <tr>
            <td>Remark</td>
            <td>:</td>
            <td><asp:TextBox runat="server" MaxLength = "255" ValidationGroup="Input" Width = "350" ID="tbRemark" CssClass="TextBox" /></td>
        </tr>
      </table>        
      <br />    

      <div style="font-size:medium; color:Blue;">Detail</div>
      <hr style="color:Blue" />  
        <asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex="0">
           <asp:View ID="Tab1" runat="server">      
        <asp:Panel runat="server" ID="pnlDt">
            <asp:Button class="bitbtn btnadd" runat="server" ID="btnAddDt" Text="Add" ValidationGroup="Input" />	                  
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
                            <EditItemTemplate>
                                <asp:Button ID="btnUpdate" runat="server" class="bitbtndt btnsave" Text="Save" CommandName="Update"/>									
                                <asp:Button ID="btnCancel" runat="server" class="bitbtndt btncancel" Text="Cancel" CommandName="Cancel"/>									
                            </EditItemTemplate>                                                     
                        </asp:TemplateField>
                        <asp:TemplateField>
                               <ItemTemplate>                                                             
<%--                               <asp:Button Class="bitbtndt btngetitem"  ID="btnClosing" runat="server" Text="Closing"                                             
                                            CommandArgument='<%# Container.DataItemIndex %>'
                                            CommandName="Closing" />                                            
                                    <asp:Button Class="bitbtndt btngetitem" ID="btnDetailPO" runat="server" Text="Detail PO"                                             
                                            CommandArgument='<%# Container.DataItemIndex %>'
                                            CommandName="DetailPO" />--%>                                            
                                </ItemTemplate>
                        </asp:TemplateField>                       
                        <asp:BoundField DataField="EmpNumb" HeaderStyle-Width="80px" HeaderText="Emp. Code" />
                        <asp:BoundField DataField="NewEmpNumb" HeaderStyle-Width="80px" HeaderText="New Emp. Code" />
                        <asp:BoundField DataField="Emp_Name" HeaderStyle-Width="80px" HeaderText="Emp. Name" />                        
                        <asp:BoundField DataField="JobTitle" HeaderStyle-Width="250px"  HeaderText="Job Title" />                                                
                        <asp:BoundField DataField="JobTitle_Name" HeaderStyle-Width="250px"  HeaderText="Job Title Name" />                                                
                        <asp:BoundField DataField="JobLevel" HeaderStyle-Width="250px"  HeaderText="Job Level" />                                                
                        <asp:BoundField DataField="JobLevel_Name" HeaderStyle-Width="250px"  HeaderText="Job Level Name" />                                                                                       
                        <asp:BoundField DataField="EmpStatus" HeaderStyle-Width="250px"  HeaderText="Emp.Status" />                                                
                        <asp:BoundField DataField="EmpStatus_Name" HeaderStyle-Width="250px"  HeaderText="Emp. Status Name" />                                                                                        
                        <asp:BoundField DataField="Department" HeaderStyle-Width="250px"  HeaderText="Organization" />                                                
                        <asp:BoundField DataField="Dept_Name" HeaderStyle-Width="250px"  HeaderText="Organization Name" />                                                                
                        <asp:BoundField DataField="WorkPlace" HeaderStyle-Width="250px"  HeaderText="Work Place" />                                                
                        <asp:BoundField DataField="WorkPlace_Name" HeaderStyle-Width="250px"  HeaderText="Work Place Name" />                                                                
                        <asp:BoundField DataField="End_Date_Contract" HeaderStyle-Width="250px"  HeaderText="Contract End Date" />                                                                                                
                        <asp:BoundField DataField="Remark" HeaderStyle-Width="250px"  HeaderText="Remark" />                                                                        
                    </Columns>
                </asp:GridView>
          </div> 
          
          <asp:Button class="bitbtn btnadd" runat="server" ID="btnAddDt2" Text="Add" ValidationGroup="Input" />	  
           
       </asp:Panel>             
       <asp:Panel runat="server" ID="pnlEditDt" Visible="false">
            <table>
                <tr>
                    <td><asp:LinkButton ID="lbEmployee"  runat="server" Text="Employee"/> </td>
                    <td>:</td>
                    <td><asp:TextBox runat="server" ID="tbEmpCode" CssClass="TextBox" AutoPostBack="true" />
                        <asp:TextBox runat="server"  CssClass="TextBox"
                            ID="tbEmpName" EnableTheming="True" ReadOnly="True" Enabled="False" 
                            Width="200px"/>
                        <asp:Button Class="btngo" ID="btnEmp" Text="..." runat="server" ValidationGroup="Input" />                                               
                     </td>
                </tr>    
                <tr>
                  <td>New Emp. No.</td>
                  <td>:</td>
                  <td><asp:TextBox runat="server" MaxLength = "15" ValidationGroup="Input" Width = "350" ID="tbNewEmpNo" CssClass="TextBox" />
                  </td>
                </tr> 
                
                <tr>
                    <td>Job Title Name</td>
                    <td>:</td>
                    <td>
                    <asp:TextBox runat="server"  CssClass="TextBox"
                            ID="tbJobTitleNameDt" EnableTheming="True" ReadOnly="True" Enabled="False" 
                            Width="300px"/>  
                    <asp:TextBox runat="server" ID="tbJobTitledt" CssClass="TextBox" AutoPostBack="true" Visible = "False" />                                         
                    </td>
                </tr>                  
                <tr>
                    <td>Job Level Name</td>
                    <td>:</td>
                    <td>
                    <asp:TextBox runat="server"  CssClass="TextBox"
                            ID="tbJobLevelName" EnableTheming="True" ReadOnly="True" Enabled="False" 
                            Width="300px"/> 
                    <asp:TextBox runat="server" ID="tbJobLevel" CssClass="TextBox" AutoPostBack="true" Visible = "False" />                                                                                                               
                    </td>
                </tr>                                              
                <tr>
                    <td>Employee Status Name</td>
                    <td>:</td>
                    <td>
                    <asp:TextBox runat="server"  CssClass="TextBox"
                            ID="tbEmpStatusName" EnableTheming="True" ReadOnly="True" Enabled="False" 
                            Width="300px"/>  
                    <asp:TextBox runat="server" ID="tbEmpStatus" CssClass="TextBox" AutoPostBack="true" Visible = "False" />                                                                                                                                                                
                    </td>
                </tr>                                                                                            
                <tr>
                    <td>Organization</td>
                    <td>:</td>
                    <td>
                    <asp:TextBox runat="server"  CssClass="TextBox"
                            ID="tbDeptName" EnableTheming="True" ReadOnly="True" Enabled="False" 
                            Width="300px"/>    
                    <asp:TextBox runat="server" ID="tbDept" CssClass="TextBox" AutoPostBack="true" Visible = "False" />                                                                                                                                                                                                              
                    </td>
                </tr>                             
                <tr>
                    <td>Work Place Name</td>
                    <td>:</td>
                    <td>
                    <asp:TextBox runat="server"  CssClass="TextBox"
                            ID="tbWorkPlaceName" EnableTheming="True" ReadOnly="True" Enabled="False" 
                            Width="300px"/>    
                    <asp:TextBox runat="server" ID="tbWorkPlace" CssClass="TextBox" AutoPostBack="true" Visible = "False" />                                                                                                                                                                                                              
                    </td>
                </tr>                    
                <tr>
                    <td>Contract End Date</td>
                    <td>:</td>
                    <td>
                    <asp:TextBox runat="server"  CssClass="TextBox"
                            ID="tbContractEndDate" EnableTheming="True" ReadOnly="True" Enabled="False" 
                            Width="75px"/>                                           
                    </td>
                </tr>                        

                <tr>
                  <td>Remark</td>
                  <td>:</td>
                  <td><asp:TextBox runat="server" MaxLength = "255" ValidationGroup="Input" Width = "350" ID="tbRemarkDt" CssClass="TextBox" /></td>
                </tr>                           
                       
            </table>
            <br />                     
            <asp:Button ID="btnSaveDt" runat="server" class="bitbtndt btnsave" Text="Save" />									
            <asp:Button ID="btnCancelDt" runat="server" class="bitbtndt btncancel" Text="Cancel" />									
       </asp:Panel> 
       
       <br />     
       </asp:View>           
             
        </asp:MultiView>
     
       	<asp:Button ID="btnSaveAll" runat="server" class="bitbtndt btnsavenew" Text="Save & New" validationgroup="Input" Width = "90"/>									
        <asp:Button ID="btnSaveTrans" runat="server" class="bitbtndt btnsave" Text="Save" validationgroup="Input"/>									
        <asp:Button ID="btnBack" runat="server" class="bitbtndt btncancel" Text="Cancel" validationgroup="Input"/>									
        <asp:Button ID="btnHome" runat="server" class="bitbtndt btnback" Text="Home" />	
    </asp:Panel>
    <asp:Panel runat="server" ID="pnlPrint" Visible="false">
<%--    <cr:crystalreportviewer ID="CrystalReportViewer1" runat="server" AutoDataBind="True"
               Height="1036px" Width="928px" />
--%>    </asp:Panel>
      <br />            
    </div>
   
    <asp:Label runat ="server" ID="lbStatus" ForeColor="Red"/>
    <asp:Label runat ="server" ID="lbPrev" ForeColor="Red" Visible = "False"/>
    
    <asp:HiddenField ID="HiddenRemarkClose" runat="server" />
    </form>
    </body>
</html>
