<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TrPEReqNewEmployee.Aspx.vb" Inherits="TrPEReqNewEmployee" %>
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
                window.open("../../Transaction/TrPEReqNewEmployee/FormCekPostPR.Aspx","List","scrollbars=yes,resizable=no,width=800,height=400");
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
</head>
<body>     
    <form id="form1" runat="server">    
    <div class="Content">
    <div class="H1">Request New Employee</div>
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
                      <asp:ListItem Value="RequestType">Request Type</asp:ListItem>                
                      <asp:ListItem Value="Department">Organization Code</asp:ListItem>                
                      <asp:ListItem Value="Dept_Name">Organization Name</asp:ListItem>                
                      <asp:ListItem Value="Reason">Reason</asp:ListItem>                                            
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
                      <asp:ListItem Value="RequestType">Request Type</asp:ListItem>                
                      <asp:ListItem Value="Department">Organization Code</asp:ListItem>                
                      <asp:ListItem Value="Dept_Name">Organization Name</asp:ListItem>                
                      <asp:ListItem Value="Reason">Reason</asp:ListItem>                                         
             
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
                              <asp:ListItem Text="Print" />
                          </asp:DropDownList>
                          <asp:Button class="btngo" runat="server" ID="BtnGo" Text="G" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" CommandName="Go" />     
                      </ItemTemplate>
                      <HeaderStyle Width="110px" />
                  </asp:TemplateField>
                  <asp:BoundField DataField="TransNmbr" HeaderStyle-Width="120px" SortExpression="Nmbr" HeaderText="Reference"></asp:BoundField>                  
                  <asp:BoundField DataField="Status" HeaderText="Status"></asp:BoundField>                  
                  <asp:BoundField DataField="TransDate" dataformatstring="{0:dd MMM yyyy}" htmlencode="true" HeaderStyle-Width="80px" SortExpression="TransDate" HeaderText="Date"></asp:BoundField>
                  <asp:BoundField DataField="RequestType" HeaderStyle-Width="200px" SortExpression="RequestType" HeaderText="Request Type"></asp:BoundField>
                  <asp:BoundField DataField="Department" HeaderStyle-Width="200px" SortExpression="Department" HeaderText="Organization Code"></asp:BoundField>
                  <asp:BoundField DataField="Dept_Name" HeaderStyle-Width="200px" SortExpression="Dept_Name" HeaderText="Organization Name"></asp:BoundField>
                  <asp:BoundField DataField="Reason" HeaderStyle-Width="200px" SortExpression="Reason" HeaderText="Reason"></asp:BoundField>
                  <asp:BoundField DataField="TotalMale" HeaderStyle-Width="20px" SortExpression="TotalMale" ItemStyle-HorizontalAlign ="Right"  HeaderText="Male"></asp:BoundField>                  
                  <asp:BoundField DataField="TotalFemale" HeaderStyle-Width="20px" SortExpression="TotalFemale" ItemStyle-HorizontalAlign ="Right"  HeaderText="Female"></asp:BoundField>                  
                  <asp:BoundField DataField="Total" HeaderStyle-Width="20px" SortExpression="Total" ItemStyle-HorizontalAlign ="Right"  HeaderText="Male/Female"></asp:BoundField>                  
                  <asp:BoundField DataField="Remark" HeaderStyle-Width="250px" HeaderText="Remark"></asp:BoundField>                  
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
            <td>Request No</td>
            <td>:</td>
            <td><asp:TextBox CssClass="TextBox" Enabled ="false" runat="server" ID="tbTransNo" Width="149px"/>
            </td>            
        </tr>
        <tr>
            <td>Request Date</td>
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
            <td>Request Type</td>
            <td>:</td>
            <td>    <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlReqType" AutoPostBack="True" 
                                       ValidationGroup="Input">
                      <asp:ListItem Value="Staff"></asp:ListItem>
                      <asp:ListItem Value="Non Staf"></asp:ListItem>                                            
                    </asp:DropDownList> 
            </td>                  
        </tr>                
        
         <tr>   
          <td> <asp:LinkButton ID="lbDepartment" ValidationGroup="Input" runat="server" Text="Organization"/></td>
            <td>:</td>
            <td>     
                <asp:DropDownList CssClass="DropDownList" ValidationGroup="Input" runat="server" ID="ddlDepartment" enabled = "False"  Width="200px" />                          
                <asp:TextBox runat="server" ID="TextBox5" Visible="false"/>                
            </td>                    
        </tr> 
        <tr>
            <td>Reason</td>
            <td>:</td>
            <td><asp:TextBox runat="server" MaxLength = "255" ValidationGroup="Input" Width = "400" ID="tbReason" CssClass="TextBox" /></td>
        </tr>       
        
        <tr>
            <td>Total Required Employee</td>
            <td>:</td>
            <td>
               <table>
                    <tr style="background-color:Silver;text-align:center">
                    <td>Male</td>
                    <td>Female</td>
                    <td>Male/Female</td>                                   
                     </tr>
                        <tr>
                           <td><asp:TextBox runat="server" ID="tbTotalMale"  style="text-align:right;"  AutopostBack = "True" CssClass="TextBoxR"/></td>
                           <td><asp:TextBox runat="server" ID="tbTotalFemale"  style="text-align:right;"  AutopostBack = "True" CssClass="TextBoxR"/></td>
                           <td><asp:TextBox runat="server" ID="tbTotal"  style="text-align:right;"  AutopostBack = "True" CssClass="TextBoxR"/></td>
                     </tr>
                </table>                        
            </td>
        </tr>                                                    
        <tr>
            <td>Remark</td>
            <td>:</td>
            <td><asp:TextBox runat="server" MaxLength = "50" ValidationGroup="Input" Width = "350" ID="tbRemark" CssClass="TextBox" /></td>
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
                             <asp:Button ID="btnClosing" runat="server" class="bitbtn btnclose" Text="Closing" CommandName="Closing" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" />     
                           </ItemTemplate>                            
                        </asp:TemplateField>                       
                        <asp:BoundField DataField="JobTitle" HeaderStyle-Width="80px" HeaderText="JobTitle" />
                        <asp:BoundField DataField="EmpStatus" HeaderStyle-Width="250px"  HeaderText="Employee Status" />
                        <asp:BoundField DataField="RangeTime" HeaderText="Jangka Waktu Kerja" />                                                                        
                        <asp:BoundField DataField="StartDate" dataformatstring="{0:dd MMM yyyy}" htmlencode="true" HeaderStyle-Width="80px" SortExpression="StartDate" HeaderText="Tanggal Mulai Kerja"></asp:BoundField>                        
                        <asp:BoundField DataField="QtyMale" HeaderStyle-Width="250px" HeaderText="Male" />                                                
                        <asp:BoundField DataField="QtyFemale" HeaderStyle-Width="250px" HeaderText="Female" />                                                
                        <asp:BoundField DataField="QtyTotal" HeaderStyle-Width="250px" HeaderText="Male/Female" />                                                
                        <asp:BoundField DataField="SyaratUmum" HeaderText="Syarat Umum" />                                                                          
                        <asp:BoundField DataField="SyaratKhusus" HeaderText="Syarat Khusus" />                                                                          
                        <asp:BoundField DataField="Responsibility" HeaderText="Responsibility" />                                                                                                                          
                        <asp:BoundField DataField="Remark" HeaderText="Remark" />  
                        <asp:BoundField DataField="FgClosing" HeaderText="Closing" />  
                        <asp:BoundField DataField="UserClose" HeaderText="User Close" />  
                        <asp:BoundField DataField="DateClose" HeaderText="Date Close" />  
                        <asp:BoundField DataField="QtyClose" HeaderText="Qty Close" />  
                        <asp:BoundField DataField="ClosingReason" HeaderText="Closing Reason" />                                                                                              
                        
                    </Columns>
                </asp:GridView>
          </div> 
          
          <asp:Button class="bitbtn btnadd" runat="server" ID="btnAddDt2" Text="Add" ValidationGroup="Input" />	  
           
       </asp:Panel>             
       <asp:Panel runat="server" ID="pnlEditDt" Visible="false">
            <table>
            <tr>   
               <td> <asp:LinkButton ID="lbJobTitle" ValidationGroup="Input" runat="server" Text="Job Title"/></td>
               <td>:</td>
               <td>     
                  <asp:DropDownList CssClass="DropDownList" ValidationGroup="Input" runat="server" ID="ddlJobTitle" enabled = "False"  Width="200px" />                          
                  <asp:TextBox runat="server" ID="tbJobTitle" Visible="false"/>                
              </td>                    
            </tr>                                            
            
            <tr>   
               <td> <asp:LinkButton ID="lbEmpStatus" ValidationGroup="Input" runat="server" Text="Employee Status"/></td>
               <td>:</td>
               <td>     
                  <asp:DropDownList CssClass="DropDownList" ValidationGroup="Input" runat="server" ID="ddlEmpStatus" enabled = "False"  Width="200px" />                          
                  <asp:TextBox runat="server" ID="tbEmpStatus" Visible="false"/>                
              </td>                    
            </tr>                                            
             <tr>
                 <td>Work Period</td>
                 <td>:</td>
                 <td><asp:TextBox runat="server" CssClass="TextBox" Width ="250" ID="tbJangkaWaktu" maxlength = "60"/>                                              
                 </td>
             </tr>
                <tr>
                <td>Start Date</td>
                <td>:</td>
                <td>
                <BDP:BasicDatePicker ID="dpStartDate" runat="server" DateFormat="dd MMM yyyy" 
                        ShowNoneButton ="false" ReadOnly = "true" ValidationGroup="Input"
                        ButtonImageHeight="19px" ButtonImageWidth="20px" 
                        DisplayType="TextBoxAndImage" 
                        TextBoxStyle-CssClass="TextDate" AutoPostBack="True"><TextBoxStyle CssClass="TextDate" /></BDP:BasicDatePicker>                
                </td>            
                </tr>  
                <tr>
                    <td>Required Employee</td>
                    <td>:</td>
                    <td>
                      <table>
                                <tr style="background-color:Silver;text-align:center">
                                    <td>Male</td>
                                    <td>Female</td>
                                    <td>Male/Female</td>                                   
                                </tr>
                                <tr>
                                    <td><asp:TextBox runat="server" ID="tbMale"  style="text-align:right;"  CssClass="TextBox" MaxLength = "4" /></td>
                                    <td><asp:TextBox runat="server" ID="tbFemale"   style="text-align:right;"  CssClass="TextBox" MaxLength = "4" /></td>
                                    <td><asp:TextBox runat="server" ID="tbTotaldt"  style="text-align:right;"  CssClass="TextBox" MaxLength = "4" /></td>
                                </tr>
                       </table>                        
                    </td>
                </tr>                
                <tr>
                    <td>General Requirements</td>
                    <td>:</td>
                    <td><asp:TextBox runat="server" CssClass="TextBox" ID="tbSyaratUmum" 
                             width = "400"  MaxLength = "255"/>                      
                    </td>
                </tr>                                                                          
                <tr>
                    <td>Special Requirements</td>
                    <td>:</td>                    
                    <td><asp:TextBox runat="server" CssClass="TextBox" ID="tbSyaratKhusus" width = "400"  MaxLength = "255" 
                           />
                    </td>
                </tr>                                                                
                <tr>
                    <td>Responsibility</td>
                    <td>:</td>
                    <td><asp:TextBox runat="server" CssClass="TextBox" ID="tbResponsibility" width = "400"  MaxLength = "255" 
                           />
                    </td>
                </tr>                                                          
                <tr>
                    <td>Remark </td>
                    <td>:</td>
                    <td><asp:TextBox runat="server" ID="tbRemarkDt" CssClass="TextBox" width = "250" MaxLength = "60"/>                        
                    </td>
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
