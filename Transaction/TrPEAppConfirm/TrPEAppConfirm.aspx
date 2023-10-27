<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TrPEAppConfirm.aspx.vb" Inherits="Transaction_TrPEAppConfirm_TrPEAppConfirm" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="BasicFrame.WebControls.BasicDatePicker" Namespace="BasicFrame.WebControls"
    TagPrefix="BDP" %>
<%@ Register assembly="CrystalDecisions.Web, Version=10.5.3700.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Candidate Confirm</title>
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
         document.getElementById("tbSalaryGP").value = setdigit(document.getElementById("tbSalaryGP").value.replace(/\$|\,/g,""),'<%=VIEWSTATE("DigitCurr")%>');
        
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
    <div class="H1">Candidate Confirm</div>
     <hr style="color:Blue" />        
    <asp:Panel runat="server" id="PnlHd"> 
      <table>
        <tr>
            <td style="width:100px;text-align:right">Quick Search :</td>            
            <td><asp:TextBox CssClass="TextBox" runat="server" ID ="tbFilter"/> 
                  <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlField" >
                     <asp:ListItem Selected="True" Value="TransNmbr">Confirm No</asp:ListItem>
                     <asp:ListItem Value="dbo.FormatDate(TransDate)">Confirm Date</asp:ListItem>
                     <asp:ListItem>Status</asp:ListItem>
                     <asp:ListItem Value="RequestNo">Request No</asp:ListItem>
                     <asp:ListItem Value="JobTitle">JobTitle Code</asp:ListItem>
                     <asp:ListItem Value="JobTitle_Name">JobTitle Name</asp:ListItem>                                        
                     <asp:ListItem Value="EmpStatus">EmpStatus Code</asp:ListItem>                    
                     <asp:ListItem Value="QtyMale">Qty Male</asp:ListItem>
                     <asp:ListItem Value="QtyFemale">Qty Female</asp:ListItem>
                     <asp:ListItem Value="QtyTotal">Qty Total</asp:ListItem>
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
                     <asp:ListItem Selected="True" Value="TransNmbr">Confirm No</asp:ListItem>
                     <asp:ListItem Value="dbo.FormatDate(TransDate)">Confirm Date</asp:ListItem>
                     <asp:ListItem>Status</asp:ListItem>
                     <asp:ListItem Value="RequestNo">Request No</asp:ListItem>
                     <asp:ListItem Value="JobTitle">JobTitle Code</asp:ListItem>
                     <asp:ListItem Value="JobTitle_Name">JobTitle Name</asp:ListItem>                                        
                     <asp:ListItem Value="EmpStatus">EmpStatus Code</asp:ListItem>                    
                     <asp:ListItem Value="QtyMale">Qty Male</asp:ListItem>
                     <asp:ListItem Value="QtyFemale">Qty Female</asp:ListItem>
                     <asp:ListItem Value="QtyTotal">Qty Total</asp:ListItem>
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
                  <asp:BoundField DataField="TransNmbr" HeaderStyle-Width="120px" SortExpression="Nmbr" HeaderText="Candidate No"></asp:BoundField>                  
                  <asp:BoundField DataField="Status" HeaderText="Status"></asp:BoundField>                  
                  <asp:BoundField DataField="TransDate" dataformatstring="{0:dd MMM yyyy}" htmlencode="true" HeaderStyle-Width="80px" SortExpression="TransDate" HeaderText="Candidate Date"></asp:BoundField>
                  <asp:BoundField DataField="RequestNo" HeaderStyle-Width="80px" SortExpression="RequestNo" HeaderText="Request No"></asp:BoundField>
                  <asp:BoundField DataField="JobTitle" HeaderStyle-Width="80px" SortExpression="JobTitle" HeaderText="JobTitle Code"></asp:BoundField>
                  <asp:BoundField DataField="JobTitle_Name" HeaderStyle-Width="200px" SortExpression="JobTitle_Name" HeaderText="JobTitle Name"></asp:BoundField>
                  <asp:BoundField DataField="EmpStatus" HeaderStyle-Width="80px" SortExpression="EmpStatus" HeaderText="EmpStatus Code"></asp:BoundField>
                  <asp:BoundField DataField="EmpStatus_Name" HeaderStyle-Width="200px" SortExpression="EmpStatus_Name" HeaderText="EmpStatus Name"></asp:BoundField>
                  <asp:BoundField DataField="QtyMale" HeaderText="Qty Male" SortExpression="QtyMale" ></asp:BoundField>
                  <asp:BoundField DataField="QtyFemale" HeaderText="Qty Female" SortExpression="QtyFemale" ></asp:BoundField>
                  <asp:BoundField DataField="QtyTotal" HeaderText="Qty Total" SortExpression="Qty Total" ></asp:BoundField>
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
            <td>Confirm No</td>
            <td>:</td>
            <td><asp:TextBox CssClass="TextBox" Enabled = "false" runat="server" ID="tbRef" Width="149px"/> 
            </td>            
        </tr>
        <tr>
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
            <td>Request No</td>
            <td>:</td>
            <td><asp:TextBox CssClass="TextBox" Enabled ="false" runat="server" ID="tbRequestNo" MaxLength="20" Width="150px" />
                <asp:Button Class="btngo" ID="btnRequestNo" Text="..." runat="server" ValidationGroup="Input" />
            </td> 
             
        </tr>
        <tr>
            <td>Job Title</td>
             <td>:</td>
             <td>                            
                 <asp:DropDownList CssClass="DropDownList" ID="ddlJobTitle" runat="server" Enabled ="false"/>
             </td>    
        </tr>
        
        <tr> 
             <td>Emp. Status</td>
             <td>:</td>
             <td colspan="4">                            
                 <asp:DropDownList CssClass="DropDownList" ID="ddlEmpStatus" runat="server" Enabled ="false"/>
             </td>                        
        </tr>
        <tr> 
                        <td>Qty</td>
                        <td>:</td>
                        <td colspan="4">
                            <table>
                                <tr style="background-color:Silver;text-align:center">
                                    <td>Male</td>
                                    <td>Female</td>
                                    <td>Total</td>
                                </tr>                             
                                <tr>
                                    <td><asp:TextBox CssClass="TextBoxR" runat="server" ID="tbQtyMale" Width="65px"/></td>                                    
                                    <td><asp:TextBox CssClass="TextBoxR" runat="server" ID="tbQtyFemale" Width="65px"/></td>                                    
                                    <td><asp:TextBox CssClass="TextBoxR" runat="server" ID="tbQtyTotal" Width="65px"/></td>                                    
                                </tr>
                            </table>
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
                        <asp:BoundField DataField="CandidateNo" HeaderStyle-Width="200px" HeaderText="Candidate No" SortExpression="CandidateNo" ></asp:BoundField>
                        <asp:BoundField DataField="Candidate_Name" HeaderText="Candidate Name" HeaderStyle-Width="200px" SortExpression="Candidate_Name" ></asp:BoundField>
                        <asp:BoundField DataField="Gender" HeaderText="Gender" HeaderStyle-Width="100px" SortExpression="Gender" ></asp:BoundField>
                        <asp:BoundField DataField="FgRecruitment" HeaderStyle-Width="80px" HeaderText="Recruit" SortExpression="FgRecruitment" ></asp:BoundField>                        
                    </Columns>
                </asp:GridView>
          </div>   
          <asp:Button class="bitbtndt btnadd" runat="server" ID="btnAddDt2" Text="Add" ValidationGroup="Input" />  
       </asp:Panel>             
       <asp:Panel runat="server" ID="pnlEditDt" Visible="false">
         <table>
          <tr>
                 <td>Candidate</td>
                 <td>:</td>
                 <td colspan="5"><asp:TextBox runat="server" ID="tbEmpCode" CssClass="TextBox" AutoPostBack="true" />
                    <asp:TextBox runat="server"  CssClass="TextBox" ID="tbEmpName" EnableTheming="True" ReadOnly="True" Enabled="False" Width="200px"/>
                    <asp:Button ID="btnEmp" runat="server" class="btngo" Text="..."/>                        
                 </td>
          </tr>
          <tr>
                <td>Gender</td>
                <td>:</td>
                <td><asp:TextBox runat="server"  CssClass="TextBox" ID="tbGender" EnableTheming="True" ReadOnly="True" Enabled="False" Width="80px"/> 
                </td>           
          </tr>
          <tr>
                <td>Recruit</td>
                <td>:</td>
                <td><asp:DropDownList ValidationGroup="Input" CssClass="DropDownList" ID="ddlFgRecruit" runat="server" >
                            <asp:ListItem Selected="True">Y</asp:ListItem>
                            <asp:ListItem>N</asp:ListItem>
                        </asp:DropDownList> 
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
        <asp:Button ID="btnHome" runat="server" class="bitbtndt btncancel" Text="Cancel"/>    
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
