<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TrPEMedicalClaim.Aspx.vb" Inherits="TrPEMedicalClaim" %>
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
                window.open("../../Transaction/TrPEMedicalClaim/FormCekPostPR.Aspx","List","scrollbars=yes,resizable=no,width=800,height=400");
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
    <div class="H1">Medical Claim</div>
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
                      <asp:ListItem Value="Department_Code">Organization</asp:ListItem>                
                      <asp:ListItem Value="Department_Name">Organization Name</asp:ListItem>
                      <asp:ListItem Value="EmpNumb">Employee No.</asp:ListItem>                      
                      <asp:ListItem Value="EmpName">Employee Name</asp:ListItem>                                            
                      <asp:ListItem Value="JobTitleCode">Job Title</asp:ListItem>                                            
                      <asp:ListItem Value="JobTitleName">Job Title Name</asp:ListItem>                                            
                      <asp:ListItem Value="JobLevelCode">Job Level</asp:ListItem>                                            
                      <asp:ListItem Value="JobLevelName">Job Level Name</asp:ListItem>                                            
                      <asp:ListItem Value="MedicalFor">Medical For</asp:ListItem>                                            
                      <asp:ListItem Value="FamilyType">Family Type</asp:ListItem>                                            
                      <asp:ListItem Value="FamilyName">Family Name</asp:ListItem>                                            
                      <asp:ListItem Value="MedicalFor">Medical For</asp:ListItem>                                            
                      <asp:ListItem Value="CurrCode">Currency Code</asp:ListItem>                                            
                      <asp:ListItem Value="RemarkHd">Remark</asp:ListItem>                                        
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
                      <asp:ListItem Value="Department_Code">Organization</asp:ListItem>                
                      <asp:ListItem Value="Department_Name">Organization Name</asp:ListItem>
                      <asp:ListItem Value="EmpNumb">Employee No.</asp:ListItem>                      
                      <asp:ListItem Value="EmpName">Employee Name</asp:ListItem>                                            
                      <asp:ListItem Value="JobTitleCode">Job Title</asp:ListItem>                                            
                      <asp:ListItem Value="JobTitleName">Job Title Name</asp:ListItem>                                            
                      <asp:ListItem Value="JobLevelCode">Job Level</asp:ListItem>                                            
                      <asp:ListItem Value="JobLevelName">Job Level Name</asp:ListItem>                                            
                      <asp:ListItem Value="MedicalFor">Medical For</asp:ListItem>                                            
                      <asp:ListItem Value="FamilyType">Family Type</asp:ListItem>                                            
                      <asp:ListItem Value="FamilyName">Family Name</asp:ListItem>                                            
                      <asp:ListItem Value="MedicalFor">Medical For</asp:ListItem>                                            
                      <asp:ListItem Value="CurrCode">Currency Code</asp:ListItem>                                            
                      <asp:ListItem Value="RemarkHd">Remark</asp:ListItem>                                        
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
                          <asp:Button class="btngo" runat="server" ID="BtnGo" Text="G" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" CommandName="Go" />                      </ItemTemplate>
                      <HeaderStyle Width="110px" />
                  </asp:TemplateField>
                  <asp:BoundField DataField="TransNmbr" HeaderStyle-Width="120px" SortExpression="Nmbr" HeaderText="Reference"></asp:BoundField>                  
                  <asp:BoundField DataField="Status" HeaderText="Status"></asp:BoundField>                  
                  <asp:BoundField DataField="TransDate" dataformatstring="{0:dd MMM yyyy}" htmlencode="true" HeaderStyle-Width="80px" SortExpression="TransDate" HeaderText="Date"></asp:BoundField>
                  <asp:BoundField DataField="Department_Code" HeaderStyle-Width="200px" SortExpression="Department_Code" HeaderText="Organization"></asp:BoundField>
                  <asp:BoundField DataField="Department_Name" HeaderStyle-Width="200px" SortExpression="Department_Name" HeaderText="Organization Name"></asp:BoundField>
                  <asp:BoundField DataField="EmpNumb" HeaderStyle-Width="200px" SortExpression="EmpNumb" HeaderText="Employee No."></asp:BoundField>
                  <asp:BoundField DataField="EmpName" HeaderStyle-Width="200px" SortExpression="EmpName" HeaderText="Employee Name."></asp:BoundField>
                  <asp:BoundField DataField="JobTitleCode" HeaderStyle-Width="200px" SortExpression="JobTitleCode" HeaderText="Job Title"></asp:BoundField>                  
                  <asp:BoundField DataField="JobTitleName" HeaderStyle-Width="200px" SortExpression="JobTitleName" HeaderText="Job Title Name"></asp:BoundField>                  
                  <asp:BoundField DataField="JobLevelCode" HeaderStyle-Width="200px" SortExpression="JobLevelCode" HeaderText="Job Title"></asp:BoundField>                  
                  <asp:BoundField DataField="JobLevelName" HeaderStyle-Width="200px" SortExpression="JobLevelName" HeaderText="Job Title Name"></asp:BoundField>                  
                  <asp:BoundField DataField="MedicalFor" HeaderStyle-Width="200px" SortExpression="MedicalFor" HeaderText="Medical For"></asp:BoundField>                  
                  <asp:BoundField DataField="FamilyType" HeaderStyle-Width="200px" SortExpression="FamilyType" HeaderText="Family Type"></asp:BoundField>                  
                  <asp:BoundField DataField="FamilyName" HeaderStyle-Width="200px" SortExpression="FamilyName" HeaderText="Family Name"></asp:BoundField>                  
                  <asp:BoundField DataField="CurrCode" HeaderStyle-Width="200px" SortExpression="CurrCode" HeaderText="Currency"></asp:BoundField>                  
                  <asp:BoundField DataField="TotalForex" HeaderStyle-Width="200px" SortExpression="TotalForex" HeaderText="Total Forex"></asp:BoundField>                                                     
                  <asp:BoundField DataField="RemarkHD" HeaderStyle-Width="250px" HeaderText="Remark"></asp:BoundField>                  
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
            <td>Claim No</td>
            <td>:</td>
            <td><asp:TextBox CssClass="TextBox" Enabled ="false" runat="server" ID="tbTransNo" Width="149px"/>
            </td>                    
            <td>Claim Date</td>
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
            <td>Organization</td>
            <td>:</td>
            <td><asp:DropDownList runat="server" ValidationGroup="Input" ID="ddlDept" CssClass="DropDownList" Width="200px" AutoPostBack="true" /></td>
            <td>Medical For</td>
            <td>:</td>
            <td>    <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlMedicalFor" AutoPostBack="True" 
                                        onselectedindexchanged="ddlMedicalFor_SelectedIndexChanged"  ValidationGroup="Input">
                      <asp:ListItem Value="Employee"></asp:ListItem>
                      <asp:ListItem Value="Family"></asp:ListItem>                                            
                    </asp:DropDownList> 
            </td>                  
        </tr>            
        <tr>
           <td>
               <asp:LinkButton ID="lbEmployee" ValidationGroup="Input" runat="server" Text="Employee"/>
            </td>
            <td>:</td>
            <td colspan="4">
                <asp:TextBox ID="tbEmployee" runat="server" AutoPostBack="true" MaxLength = "15" 
                    CssClass="TextBox" ValidationGroup = "Input" />
                <asp:TextBox ID="tbEmpName" runat="server" CssClass="TextBoxR" Enabled="false" 
                    EnableTheming="True" Width="200px" />
                <asp:Button Class="btngo" ID="btnEmployee" Text="..." runat="server" ValidationGroup="Input" />                                                       
            </td> 
        </tr>   

        
          
        
       
         <tr>   
          <td> <asp:Label ID="lbEmpStatus" runat="server" Text="Employee Status"/></td>
            <td>:</td>
            <td>     
                <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlEmpStatus" enabled = "False"  Width="200px" />                          
                <asp:TextBox runat="server" ID="TextBox5" Visible="false"/>                
            </td>      
            <td> Gender </td>
            <td>:</td>
            <td>
                <asp:TextBox runat="server" ID="tbJenisKelamin" CssClass="TextBoxR" EnableTheming="True" width = "100" Enabled="false" />                
            </td>                                  
         </tr>  
         
                         
          
         <tr>
            <td><asp:Label ID="lbJobTitle" runat="server" Text="Job Title"/></td>
            <td>:</td>
            <td>
                <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlJobTitle" enabled = "False"  Width="200px" />                
                <asp:TextBox runat="server" ID="TextBox4" Visible="false"/>                
            </td>                            
            <td><asp:Label ID="lbJobLevel" runat="server" Text="Job Level"/></td>
            <td>:</td>
            <td>
                <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlJobLevel" enabled = "False"  Width="200px" />                
                <asp:TextBox runat="server" ID="TextBox1" Visible="false"/>                
            </td>                    
        </tr>      
        
        <tr>
           <td>
               Family Type
            </td>
            <td>:</td>
            <td colspan="4">
                <asp:TextBox ID="tbFamilyType" runat="server" AutoPostBack="true" MaxLength = "15" 
                    CssClass="TextBox" ValidationGroup = "Input" />
                <asp:TextBox ID="tbFamilyName" runat="server" CssClass="TextBoxR" Enabled="false" 
                    EnableTheming="True" ValidationGroup = "Input" Width="200px" />
                <asp:Button Class="btngo" ID="btnFamily" Text="..." runat="server" ValidationGroup="Input" />                                                       
            </td> 
        </tr>  
        <tr>
            <td><asp:LinkButton ID="lbCurrency" ValidationGroup="Input" runat="server" Text="Currency & Rate"/></td>
            <td>:</td>
            <td>
                <asp:DropDownList CssClass="DropDownList" ValidationGroup="Input" runat="server" AutoPostBack="True" 
                                        onselectedindexchanged="ddlCurrency_SelectedIndexChanged" ID="ddlCurrency" enabled = "False"  Width="130px" />                
                <asp:TextBox ID="tbRate" ValidationGroup="Input" runat="server" CssClass="TextBox" Width="60px" />                
            </td>                            
           <td>Total Forex</td>
           <td>:</td>                    
           <td><asp:TextBox runat="server" CssClass="TextBox" ID="tbTotalForex" MaxLength = "20" Enabled="false" />
           </td>
        </tr>                                                                        
        <tr>
            <td>Remark</td>
            <td>:</td>
            <td colspan="4"><asp:TextBox runat="server" MaxLength = "255" ValidationGroup="Input" Width = "350" ID="tbRemark" CssClass="TextBoxMulti" TextMode="MultiLine" /></td>
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
                        <asp:BoundField DataField="Medical" HeaderStyle-Width="80px" HeaderText="Medical Code" />
                        <asp:BoundField DataField="Medical_Name" HeaderStyle-Width="250px"  HeaderText="Medical Name" />
                        <asp:BoundField DataField="ClaimType" HeaderText="Claim Type" />                                                
                        <asp:BoundField DataField="ReceiptNo" HeaderStyle-Width="250px" HeaderText="Receipt No." />                        
                        <asp:BoundField DataField="ReceiptDate" dataformatstring="{0:dd MMM yyyy}" htmlencode="true" HeaderStyle-Width="80px" SortExpression="RequireDate" HeaderText="Receipt Date"></asp:BoundField>                        
                        <asp:BoundField DataField="AmountClaim" HeaderText="Amount Claim" />                                                                        
                        <asp:BoundField DataField="PercentPaid" HeaderText="Percent Paid" />                           
                        <asp:BoundField DataField="AmountPaid" HeaderText="Amount Paid" />                                                                                                                     
                        <asp:BoundField DataField="AmountToBePaid" HeaderStyle-Width="250px" HeaderText="Amount To be Paid" />                                                
                        <asp:BoundField DataField="Remark" HeaderText="Remark" />                                                                          
                        
                    </Columns>
                </asp:GridView>
          </div> 
          
          <asp:Button class="bitbtn btnadd" runat="server" ID="btnAddDt2" Text="Add" ValidationGroup="Input" />	  
           
       </asp:Panel>             
       <asp:Panel runat="server" ID="pnlEditDt" Visible="false">
            <table>
                <tr>
                    <td><asp:LinkButton ID="lbMedical"  runat="server" Text="Medical"/> </td>
                    <td>:</td>
                    <td colspan="4"><asp:TextBox runat="server" ID="tbMedicalCode" CssClass="TextBox" AutoPostBack="true" />
                        <asp:TextBox runat="server"  CssClass="TextBox"
                            ID="tbMedicalName" EnableTheming="True" ReadOnly="True" Enabled="False" 
                            Width="200px"/>
                        <asp:Button Class="btngo" ID="btnMedical" Text="..." runat="server" ValidationGroup="Input" />                                               
                     </td>                     
                </tr>           
                <tr>
                   <td>Claim Type</td>
                    <td>:</td>
                    <td colspan="4"> 
                         <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlClaim" ValidationGroup="Input">
                         <asp:ListItem Value="Internal"></asp:ListItem>
                         <asp:ListItem Value="External"></asp:ListItem>                                            
                         </asp:DropDownList> 
                    </td>          
                 </tr>
                 <tr>                           
                    <td>Receipt No.</td>
                    <td>:</td>
                    <td><asp:TextBox runat="server" CssClass="TextBox" Width ="220" ID="tbReceiptNo" maxlength = "60"/>                                              
                    </td>
                
                    <td>Receipt Date</td>
                    <td>:</td>
                    <td>
                    <BDP:BasicDatePicker ID="ReceiptDate" runat="server" DateFormat="dd MMM yyyy" 
                            ShowNoneButton ="false" ReadOnly = "true" ValidationGroup="Input"
                            ButtonImageHeight="19px" ButtonImageWidth="20px" 
                            DisplayType="TextBoxAndImage" 
                            TextBoxStyle-CssClass="TextDate" AutoPostBack="True"><TextBoxStyle CssClass="TextDate" /></BDP:BasicDatePicker>                
                    </td>            
                </tr>  
                <tr>
                    <td>Amount</td>
                    <td>:</td>
                    <td colspan="4">
                        <table cellspacing="1" cellpadding="1">
                            <tr style="background-color:Silver;text-align:center">
                                <td>Claim</td>
                                <td>Reimbursment</td>
                                <td>To Be Paid</td>
                                <td>Paid</td>                                
                            </tr>
                            <tr>
                                <td><asp:TextBox runat="server" CssClass="TextBox" ID="tbAmountClaim" 
                                        AutoPostBack="True" MaxLength = "9"  />                      
                                </td>                
                                <td><asp:TextBox runat="server" CssClass="TextBox" ID="tbAmountPercent" AutoPostBack="True" width = "50" MaxLength = "9"/> &nbsp %                     
                                </td>
                                <td><asp:TextBox runat="server" CssClass="TextBox" ID="tbAmountToPaid" MaxLength = "20" Enabled="false"/>
                                </td>                
                                <td><asp:TextBox runat="server" CssClass="TextBox" ID="tbAmountPaid" MaxLength = "20" 
                                      AutoPostBack="True" />
                                </td>
                            </tr>
                        </table>
                    </td>        
                </tr>                                                          
                <tr>
                    <td>Remark </td>
                    <td>:</td>
                    <td colspan="4"><asp:TextBox runat="server" ID="tbRemarkDt" CssClass="TextBoxMulti" width = "350" MaxLength = "255" TextMode="MultiLine"/>                        
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
