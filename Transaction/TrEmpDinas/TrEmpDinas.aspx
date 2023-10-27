<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TrEmpDinas.aspx.vb" Inherits="Transaction_TrEmpDinas_TrEmpDinas" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="BasicFrame.WebControls.BasicDatePicker" Namespace="BasicFrame.WebControls"
    TagPrefix="BDP" %>
<%@ Register assembly="CrystalDecisions.Web, Version=10.5.3700.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>

<%@ Register assembly="FastReport" namespace="FastReport.Web" tagprefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Employee Dinas</title>
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
        var Qty = document.getElementById("tbQty").value.replace(/\$|\,/g,""); 
//        var QtyRR = document.getElementById("tbQtyRR").value.replace(/\$|\,/g,""); 
        var QtyOrder = document.getElementById("tbQtyOrder").value.replace(/\$|\,/g,""); 
        document.getElementById("tbQty").value = setdigit(Qty,'<%=ViewState("DigitQty")%>');
//        document.getElementById("tbQtyRR").value = setdigit(QtyRR,'<%=ViewState("DigitQty")%>');
        document.getElementById("tbQtyOrder").value = setdigit(QtyOrder,'<%=ViewState("DigitQty")%>');
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
    <div class="H1">Surat Dinas</div>
     <hr style="color:Blue" />        
    <asp:Panel runat="server" id="PnlHd"> 
      <table>
        <tr>
            <td style="width:100px;text-align:right">Quick Search :</td>            
            <td><asp:TextBox CssClass="TextBox" runat="server" ID ="tbFilter"/> 
                  <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlField" >
                      <asp:ListItem Value="TransNmbr" Selected="True">Reference</asp:ListItem>
                      <asp:ListItem Value="dbo.FormatDate(TransDate)">Date</asp:ListItem>
                      <asp:ListItem>Status</asp:ListItem>
                      <asp:ListItem Value="DinasType" >Duty Type</asp:ListItem>
                      <asp:ListItem Value="CarRequest" >Request Car</asp:ListItem>
                      <asp:ListItem Value="CarNo">Car No</asp:ListItem>
                      <asp:ListItem Value="Driver" >Driver</asp:ListItem> 
                      <asp:ListItem >Remark</asp:ListItem>                       
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
                  <asp:ListItem Value="TransNmbr" Selected="True">Reference</asp:ListItem>
                      <asp:ListItem Value="dbo.FormatDate(TransDate)">Date</asp:ListItem>
                      <asp:ListItem>Status</asp:ListItem>
                      <asp:ListItem Value="DinasType">Duty Type</asp:ListItem>
                      <asp:ListItem Value="CarRequest">Request Car</asp:ListItem>
                      <asp:ListItem Value="CarNo">Car No</asp:ListItem>
                      <asp:ListItem Value="Driver">Driver</asp:ListItem> 
                      <asp:ListItem >Remark</asp:ListItem>                     
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
              <asp:GridView ID="GridView1" runat="server" AllowPaging="True" 
                  AllowSorting="True" AutoGenerateColumns="False" CssClass="Grid">
                  <HeaderStyle CssClass="GridHeader" />
                  <RowStyle CssClass="GridItem" Wrap="false" />
                  <AlternatingRowStyle CssClass="GridAltItem" />
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
                              <asp:DropDownList ID="ddl" runat="server" CssClass="DropDownList">
                                  <asp:ListItem Selected="True" Text="View" />
                                  <asp:ListItem Text="Edit" />
                                  <asp:ListItem Text="Print" />
                                  <asp:ListItem Text="Complete" />
                              </asp:DropDownList>                              
                              <asp:Button class="btngo" runat="server" ID="BtnGo" Text="G" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" CommandName="Go" />
                          </ItemTemplate>
                          <HeaderStyle Width="110px" />
                      </asp:TemplateField>
                      <asp:BoundField DataField="TransNmbr" HeaderStyle-Width="120px" 
                          HeaderText="Reference" SortExpression="Nmbr">
                          <HeaderStyle Width="120px" />
                      </asp:BoundField>
                      <asp:BoundField DataField="Status" HeaderText="Status" />
                      <asp:BoundField DataField="TransDate" HtmlEncode="true" DataFormatString="{0:dd MMM yyyy}" HeaderStyle-Width="80px" 
                          HeaderText="Date" SortExpression="TransDate">
                          <HeaderStyle Width="80px" />
                      </asp:BoundField>
                      <asp:BoundField DataField="StartDate" HtmlEncode="true" DataFormatString="{0:dd MMM yyyy}" HeaderText="Start Date" 
                          SortExpression="StartDate" />    
                      <asp:BoundField DataField="StartHour" HeaderText="Start Time" 
                          SortExpression="StartHour" />    
                      <asp:BoundField DataField="EndDate" HtmlEncode="true" DataFormatString="{0:dd MMM yyyy}" HeaderText="EndDate" 
                          SortExpression="EndDate" />    
                      <asp:BoundField DataField="EndHour" HeaderText="End Time" SortExpression="EndHour" />                            
                      <asp:BoundField DataField="DinasType" HeaderText="Duty Type" 
                          SortExpression="DinasType" />        
                     <asp:BoundField DataField="CarRequest" HeaderText="Request Car" 
                          SortExpression="CarRequest" />        
                     <asp:BoundField DataField="CarNo" HeaderText="Car No." 
                          SortExpression="CarNo" />             
                     <asp:BoundField DataField="Driver" HeaderText="Driver" 
                          SortExpression="Driver" />  
                      <asp:BoundField DataField="Remark" HeaderText="Remark" 
                          SortExpression="Remark" />                       
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
            <td>Duty No</td>
            <td>:</td>
            <td><asp:TextBox CssClass="TextBoxR" runat="server" ID="tbReference" Width="150px" Enabled="False"/> </td>            
            <td>Duty Date</td>
            <td>:</td>
            <td>
            <BDP:BasicDatePicker ID="tbDate" runat="server" DateFormat="dd MMM yyyy" 
                        ReadOnly = "true" ValidationGroup="Input"
                        ButtonImageHeight="19px" ButtonImageWidth="20px" 
                        DisplayType="TextBoxAndImage" 
                        TextBoxStyle-CssClass="TextDate" AutoPostBack="True" 
                        ShowNoneButton="False"><TextBoxStyle CssClass="TextDate" />
            </BDP:BasicDatePicker>                                                 
            <asp:Label ID="Label1" runat = "server" Text = "*" ForeColor = "Red"  /> 
            </td>            
        </tr> 
        <tr>
              <td>
                  Start Date</td>
              <td>
                  :</td>
              <td>
                  <BDP:BasicDatePicker ID="tbStartDate" runat="server" AutoPostBack="True" 
                      ButtonImageHeight="19px" ButtonImageWidth="20px" DateFormat="dd MMM yyyy" 
                      DisplayType="TextBoxAndImage" ReadOnly="true" ShowNoneButton="False" 
                      TextBoxStyle-CssClass="TextDate" ValidationGroup="Input"><TextBoxStyle CssClass="TextDate" /></BDP:BasicDatePicker>
                      &nbsp Time : &nbsp
                      <asp:TextBox ID="tbStartTime" runat="server" CssClass="TextBox" MaxLength="5" 
                      ValidationGroup="Input" Width="53px" />
                   <asp:Label ID="Label3" runat = "server" Text = "*" ForeColor = "Red"  />    
              </td>          
              <td>
                  End Date</td>
              <td>
                  :</td>
              <td>
                  <BDP:BasicDatePicker ID="tbEndDate" runat="server" AutoPostBack="True" 
                      ButtonImageHeight="19px" ButtonImageWidth="20px" DateFormat="dd MMM yyyy" 
                      DisplayType="TextBoxAndImage" ReadOnly="true" ShowNoneButton="False" 
                      TextBoxStyle-CssClass="TextDate" ValidationGroup="Input"><TextBoxStyle CssClass="TextDate" /></BDP:BasicDatePicker>
                  <asp:Label ID="Label4" runat = "server" Text = "*" ForeColor = "Red"  />     
                  &nbsp Time : &nbsp    
                  <asp:TextBox ID="tbEndTime" runat="server" CssClass="TextBox" MaxLength="5" 
                      ValidationGroup="Input" Width="53px" />
                  <asp:Label ID="Label5" runat = "server" Text = "*" ForeColor = "Red"  />      
                      
              </td>
          </tr>
          <tr>   
             <td>
                  Duty Type</td>
              <td>
                  :</td>    
                 
             <td>
               <asp:DropDownList ID="ddlDinasType" runat="server" ValidationGroup = "Input" CssClass="DropDownList" Width="200px">
               <asp:ListItem>Dalam Kota</asp:ListItem>
               <asp:ListItem>Luar Kota</asp:ListItem>
               <asp:ListItem>Luar Negeri</asp:ListItem>
               </asp:DropDownList>
               <asp:Label ID="Label6" runat = "server" Text = "*" ForeColor = "Red"  /> 
             </td>              
              <td>
                  Request Car</td>
              <td>
                  :</td>                 
                                       
             <td>
               <asp:DropDownList ID="ddlCarRequest" runat="server" CssClass="DropDownList" ValidationGroup="Input"
                Height="16px" Width="44px" OnSelectedIndexChanged = "ddlCarRequest_SelectedIndexChanged" AutoPostBack = "true" >
               <asp:ListItem>N</asp:ListItem>
               <asp:ListItem>Y</asp:ListItem>               
               </asp:DropDownList>
               <asp:Label ID="Label7" runat = "server" Text = "*" ForeColor = "Red"  /> 
             </td>              
          </tr>
          <tr>
              <td>Car No.</td>
              <td>:</td>
              <td><asp:TextBox runat="server" ValidationGroup="Input" ID="tbCarNo" CssClass="TextBox" MaxLength = "60" Width="250px"/></td>
              <td>Driver</td>
              <td>:</td>
              <td><asp:TextBox runat="server" ValidationGroup="Input" ID="tbDriver" CssClass="TextBox" MaxLength = "60" Width="250px"/></td>
          </tr>          
          <tr>
              <td>Remark</td>
              <td>:</td>
              <td colspan="4"><asp:TextBox runat="server" ValidationGroup="Input" ID="tbRemark"  CssClass="TextBoxMulti" MaxLength = "255" Width="360px" TextMode="MultiLine"/></td>
          </tr>
      </table>        
        
        <br />
        <asp:Menu ID="Menu1" runat="server" CssClass="Menu" ItemWrap="False" 
            Orientation="Horizontal" StaticEnableDefaultPopOutImage="False" 
            StaticMenuItemStyle-CssClass="MenuItem" 
            StaticSelectedStyle-CssClass="MenuSelect">
            <StaticSelectedStyle CssClass="MenuSelect" />
            <StaticMenuItemStyle CssClass="MenuItem" />
            <Items>
                <asp:MenuItem Text="Detail Employee" Value="0"></asp:MenuItem>
                <asp:MenuItem Text="Detail Job List" Value="1"></asp:MenuItem>
                <asp:MenuItem Text="Detail Budget" Value="2"></asp:MenuItem>                                
            </Items>
      </asp:Menu>        
    
        
        
         <asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex="0">
            <asp:View ID="Tab1" runat="server">
            <br />
               
                <asp:Panel ID="pnlDt" runat="server">
                    <asp:Button class="bitbtndt btnadd" runat="server" ID="btnadddt" Text="Add" CommandName = 'Insert'/>									                                
                   
                    <div style="border:0px  solid; width:100%; height:220px; overflow:auto;">
                        <asp:GridView ID="GridDt" runat="server" AutoGenerateColumns="False" 
                            ShowFooter="True">
                            <HeaderStyle CssClass="GridHeader" />
                            <RowStyle CssClass="GridItem" Wrap="false" />
                            <AlternatingRowStyle CssClass="GridAltItem" />
                            <PagerStyle CssClass="GridPager" />
                            <Columns>
                                <asp:TemplateField HeaderText="Action">
                                    <ItemTemplate>
                                        <asp:Button class="bitbtndt btnedit" runat="server" ID="btnEdit0" Text="Edit" CommandNAme="Edit"  />									                                
                                        <asp:Button class="bitbtndt btndelete" runat="server" ID="btnDelete0" Text="Delete" CommandNAme="Delete" OnClientClick="return confirm('Sure to delete this data?');"  />									                                                                                                                
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Button class="bitbtndt btnupdate" runat="server" ID="btnUpdate0" Text="Save" CommandNAme="Update"  />									                                                                        
                                        <asp:Button class="bitbtndt btncancel" runat="server" ID="btnCancel0" Text="Cancel" CommandNAme="Cancel"  />									                                                                                                                
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:Button class="bitbtndt btnadd" runat="server" ID="btnadddt" OnClick = "btnAddDt_Click" Text="Add" CommandName="Insert"  />									                                                                                                                                                        
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="EmpNumb" HeaderText="Employee No." HeaderStyle-Width="100px" />
                                <asp:BoundField DataField="Emp_Name" HeaderText="Employee Name" HeaderStyle-Width="260px"/>
                                <asp:BoundField DataField="Job_Title" HeaderText="Job Title" HeaderStyle-Width="180px"/>
                                <asp:BoundField DataField="Department_Name" HeaderText="Organization" HeaderStyle-Width="180px"/>                                
                                <asp:BoundField DataField="Destination" HeaderText="Destination" HeaderStyle-Width="250px" />                                
                            </Columns>
                        </asp:GridView>
                    </div>
                    <br />
                </asp:Panel>
                <asp:Panel ID="pnlEditDt" runat="server" Visible="false">
                        <table>
                           <tr>
                                <td>
                                    Employee </td>
                                <td>
                                    :</td>
                                <td>
                                   <asp:TextBox ID="tbEmployee" runat="server" AutoPostBack="true" MaxLength = "15" 
                                     CssClass="TextBox" ValidationGroup = "Input" />
                                   <asp:TextBox ID="tbEmpName" runat="server" CssClass="TextBoxR" Enabled="false" 
                                     EnableTheming="True" Width="220px" />
                                    <asp:Button Class="btngo" ID="btnEmployee" Text="..." runat="server" ValidationGroup="Input" />                                                       
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Job Title</td>
                                <td>
                                    :</td>
                                <td>
                                   <asp:TextBox ID="tbJobTitle" runat="server" CssClass="TextBoxR" Enabled="false" 
                                     EnableTheming="True" Width="200px" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                   Organization</td>
                                <td>
                                    :</td>
                                <td>
                                    <asp:TextBox ID="tbDepartment" runat="server" CssClass="TextBoxR" Enabled="false" 
                                     EnableTheming="True" Width="200px" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                   Destination</td>
                                <td>
                                    :</td>
                                <td>
                                    <asp:TextBox ID="tbDestination" runat="server" CssClass="TextBoxMulti" 
                                    Width="360px" TextMode="MultiLine" ValidationGroup = "Input"/>
                                </td>
                            </tr>

                            <tr>
                                <td colspan="3" >
                                    <asp:Button class="bitbtndt btnsave" runat="server" ID="btnSaveDt" Text="Save" />									                                                                                                                                                        
                                    <asp:Button class="bitbtndt btncancel" runat="server" ID="btnCancelDt" Text="Cancel" />									                                                                                                                                                                                                                                
                                </td>
                            </tr>
                        </table>
                        <br />                        
                 </asp:Panel>
            </asp:View>
            
            <asp:View ID="Tab2" runat="server">
               <br />
               <asp:Panel ID="PnlJobList" runat="server">
                                <asp:Button class="bitbtndt btnadd" runat="server" ID="btnAddJobList" Text="Add" />									                                                                                                                                                                                                                                
                                <div style="border:0px  solid; width:100%; height:220px; overflow:auto;">
                                    <asp:GridView ID="GridDtJob" runat="server" AutoGenerateColumns="False" 
                                        ShowFooter="True">
                                        <HeaderStyle CssClass="GridHeader" />
                                        <RowStyle CssClass="GridItem" Wrap="false" />
                                        <AlternatingRowStyle CssClass="GridAltItem" />
                                        <PagerStyle CssClass="GridPager" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="Action">
                                                <ItemTemplate>
                                                    <asp:Button class="bitbtndt btnedit" runat="server" ID="btnEdit1" Text="Edit" CommandNAme="Edit"  />									                                                                                                                                                                                                                                                                                    
                                                    <asp:Button class="bitbtndt btndelete" runat="server" ID="btnDelete1" Text="Delete" CommandNAme="Delete" OnClientClick="return confirm('Sure to delete this data?');"  />									                                                                                                                                                                                                                                                                                    
                                                    
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:Button class="bitbtndt btnupdate" runat="server" ID="btnUpdate1" Text="cancel" CommandNAme="Update"  />									                                                                                                                                                                                                                                                                                                                                        
                                                    <asp:Button class="bitbtndt btncancel" runat="server" ID="btnCancel1" Text="Cancel" CommandNAme="Cancel"  />									                                                                                                                                                                                                                                                                                                                                                                                            
                                                </EditItemTemplate>
                                                <FooterTemplate>                                                    
                                                    <asp:Button class="bitbtndt btnadd" runat="server" ID="btnAdd2" Text="Add" OnClick = "btnAddJobList_Click" CommandNAme="Insert"  />									                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                        
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="ItemNo" HeaderText="No." />
                                            <asp:BoundField DataField="JobList" HeaderText="Job List" HeaderStyle-Width="250px" />
                                            <asp:BoundField DataField="Target" HeaderText="Target" HeaderStyle-Width="250px" />
                                            <asp:BoundField DataField="Remark" HeaderText="Remark" HeaderStyle-Width="250px"/>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                                
                                <br />
                            </asp:Panel> 
                            <asp:Panel ID="PnlEditJobList" runat="server" Visible="false">
                                    <table>
                                       <tr>
                                           <td>
                                             Item No.</td>
                                           <td>
                                             :</td>
                                           <td>
                                            <asp:TextBox ID="tbItemNo" runat="server" CssClass="TextBoxR" Enabled="false" 
                                              EnableTheming="True" Width="200px" />
                                           </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Job List</td>
                                            <td>
                                                :</td>
                                            <td>
                                                <asp:TextBox ID="tbJobList" runat="server" MaxLength="255"
                                                    CssClass="TextBoxMulti" Width="380px" TextMode="MultiLine" />
                                            </td>
                                        </tr>
                                         <tr>
                                            <td>
                                                Target</td>
                                            <td>
                                                :</td>
                                            <td>
                                                <asp:TextBox ID="tbTarget" runat="server" MaxLength="255"
                                                    CssClass="TextBoxMulti" Width="380px" TextMode="MultiLine" />
                                            </td>
                                        </tr>
                                          <tr>
                                            <td>
                                                Remark</td>
                                            <td>
                                                :</td>
                                            <td>
                                                <asp:TextBox ID="tbRemarkJob" runat="server" MaxLength="255"
                                                    CssClass="TextBoxMulti" Width="380px" TextMode="MultiLine" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="3" >
                                               <asp:Button class="bitbtndt btnsave" runat="server" ID="btnSaveJob" Text="Save" />									                                                                                                                                                        
                                               <asp:Button class="bitbtndt btncancel" runat="server" ID="btnCanceljob" Text="Cancel" />									                                                                                                                                                                                                                                                                            
                                                
                                            </td>
                                        </tr>
                                    </table>
                                    <br />
                        </asp:Panel>     
            </asp:View>
            <asp:View ID="Tab3" runat="server">
                <br />
                <asp:Panel ID="PnlBudget" runat="server">
                                <asp:Button class="bitbtndt btnadd" runat="server" ID="btnAddBudget" Text="Add" />									                                                                                                                                                                                                                                                                
                                <div style="border:0px  solid; width:100%; height:220px; overflow:auto;">
                                    <asp:GridView ID="GridDtBudget" runat="server" AutoGenerateColumns="False" 
                                        ShowFooter="True">
                                        <HeaderStyle CssClass="GridHeader" />
                                        <RowStyle CssClass="GridItem" Wrap="false" />
                                        <AlternatingRowStyle CssClass="GridAltItem" />
                                        <PagerStyle CssClass="GridPager" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="Action">
                                                <ItemTemplate>
                                                    <asp:Button class="bitbtndt btnedit" runat="server" ID="btnEdit2" Text="Edit" CommandNAme="Edit"  />									                                                                                                                                                                                                                                                                                                                                        
                                                 
                                                    <asp:Button class="bitbtndt btndelete" runat="server" ID="btnDelete2" Text="Delete" CommandNAme="Delete" OnClientClick="return confirm('Sure to delete this data?');" />									                                                                                                                                                                                                                                                                                                                                                                                            
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:Button class="bitbtndt btnupdate" runat="server" ID="btnUpdate2" Text="cancel" CommandNAme="Update"  />									                                                                                                                                                                                                                                                                                    
                                                    
                                                    <asp:Button class="bitbtndt btncancel" runat="server" ID="btnCancel2" Text="cancel" CommandNAme="Cancel"  />									                                                                                                                                                                                                                                                                                                                                        
                                                </EditItemTemplate>
                                                <FooterTemplate>
                                                    
                                                    <asp:Button class="bitbtndt btnadd" runat="server" ID="btnAdd5" Text="Add" OnClick = "btnAddBudget_Click" CommandNAme="insert"  />									                                                                                                                                                                                                                                                                                                                                                                                        
                                                 </FooterTemplate>
                                                    
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="BudgetCode" HeaderText="Budget" HeaderStyle-Width="80px" />
                                            <asp:BoundField DataField="BudgetName" HeaderText="Budget Name" HeaderStyle-Width="180px"/>
                                            <asp:BoundField DataField="Description" HeaderText="Description" HeaderStyle-Width="220px"/>
                                            <asp:BoundField DataField="QtyPlan" HeaderText="Plan Qty" HeaderStyle-Width="60px"/>
                                            <asp:BoundField DataField="PricePlan" HeaderText="Plan Price" HeaderStyle-Width="60px"/>
                                            <asp:BoundField DataField="TotalPlan" HeaderText="Plan Total" HeaderStyle-Width="60px"/>
                                            <asp:BoundField DataField="QtyAct" HeaderText="Actual Qty" HeaderStyle-Width="60px"/>
                                            <asp:BoundField DataField="PriceAct" HeaderText="Actual Price" HeaderStyle-Width="60px"/>
                                            <asp:BoundField DataField="TotalAct" HeaderText="Actual Total" HeaderStyle-Width="60px"/>                                            
                                        </Columns>
                                    </asp:GridView>
                                    <br />
                                </div>
                                
                                <br />
                            </asp:Panel>
                            <asp:Panel ID="PnlEditBudget" runat="server" Visible="false">
                                    <table>
                                      <tr>
                                       <td>
                                           Budget </td>
                                         <td>
                                           :</td>
                                         <td>
                                           <asp:TextBox ID="tbBudget" runat="server" AutoPostBack="true" MaxLength = "5" 
                                            CssClass="TextBox" ValidationGroup = "Input" />
                                            <asp:TextBox ID="tbBudgetName" runat="server" CssClass="TextBoxR" Enabled="false" 
                                             EnableTheming="True" Width="200px" />
                                             <asp:Button Class="btngo" ID="btnBudget" Text="..." runat="server" ValidationGroup="Input" />                                                       
                                           </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Description</td>
                                            <td>
                                                :</td>
                                            <td>
                                                <asp:TextBox ID="tbDescription" runat="server" CssClass="TextBoxMulti" MaxLength="255"
                                                    Width="380px" TextMode="MultiLine" />
                                            </td>
                                        </tr>                                                                          
                                        <tr>
                                            <td>
                                                Plan Qty</td>
                                            <td>
                                                :</td>
                                            <td>
                                                <asp:TextBox ID="tbPlanQty" runat="server" CssClass="TextBox" MaxLength="20"
                                                    Width="71px" AutoPostBack = "true" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Plan Price</td>
                                            <td>
                                                :</td>
                                            <td>
                                                <asp:TextBox ID="tbPlanPrice" runat="server" CssClass="TextBox" MaxLength="20"
                                                    Width="71px" AutoPostBack = "true"/>
                                            </td>
                                        </tr>
                                        
                                        <tr>
                                            <td>
                                                Plan Total</td>
                                            <td>
                                                :</td>
                                            <td>
                                                <asp:TextBox ID="tbPlanTotal" runat="server" CssClass="TextBox" MaxLength="20"
                                                    Width="120px" enabled = "False"/>
                                            </td>
                                        </tr>

                                        
                                        <tr>
                                            <td colspan="3">
                                                <asp:Button class="bitbtndt btnsave" runat="server" ID="btnSaveBudget" Text="Save" />									                                                                                                                                                        
                                                <asp:Button class="bitbtndt btncancel" runat="server" ID="btnCancelBudget" Text="Cancel" />									                                                                                                                                                                                                                                                                                                                           
                                                
                                            </td>
                                        </tr>
                                    </table>
                                    <br />
                                    
                                    </asp:Panel>
            </asp:View>
            <asp:View ID="Tab4" runat="server">
            
            </asp:View>
           <asp:View ID="Tab5" runat="server">
            </asp:View>
        </asp:MultiView>
      <asp:Button ID="btnSaveAll" runat="server" class="bitbtndt btnsavenew" Text="Save & New" validationgroup="Input" Width = "90"/>									
        <asp:Button ID="btnSaveTrans" runat="server" class="bitbtndt btnsave" Text="Save" validationgroup="Input"/>									
        <asp:Button ID="btnBack" runat="server" class="bitbtndt btncancel" Text="Cancel" validationgroup="Input"/>									
        <asp:Button ID="btnHome" runat="server" class="bitbtndt btnback" Text="Home" /> 
        <br />      
    </asp:Panel>
    <asp:Panel runat="server" ID="PnlCompleteProcess" Visible="False">
            <table>
              <tr>
              <td>Date </td>
              <td>:</td>
              <td>
                 <BDP:BasicDatePicker ID="tbDateTrans" runat="server" DateFormat="dd MMM yyyy" 
                        ReadOnly = "true" ValidationGroup="Input"
                        ButtonImageHeight="19px" ButtonImageWidth="20px" 
                        DisplayType="TextBoxAndImage" 
                        TextBoxStyle-CssClass="TextDate" AutoPostBack="True" enabled = "False"
                        ShowNoneButton="False"><TextBoxStyle CssClass="TextDate" /></BDP:BasicDatePicker>                
                 </td>            
              </tr>
              <tr>
              <td>Complete Date </td>
              <td>:</td>
              <td>
                 <BDP:BasicDatePicker ID="dpCompDate" runat="server" DateFormat="dd MMM yyyy" 
                        ReadOnly = "true" ValidationGroup="Input"
                        ButtonImageHeight="19px" ButtonImageWidth="20px" 
                        DisplayType="TextBoxAndImage" 
                        TextBoxStyle-CssClass="TextDate" AutoPostBack="True" 
                        ShowNoneButton="False"><TextBoxStyle CssClass="TextDate" /></BDP:BasicDatePicker>                
                 </td>            
              </tr> 
              <tr>
                 <td>
                     Surat Dinas No</td>
                 <td>
                     :</td>
                 <td>
                    <asp:TextBox ID="tbCompSuratDinas" runat="server" CssClass="TextBoxR" Enabled="false" 
                     EnableTheming="True" Width="200px" />
                  </td>
              </tr>
              <tr>
                 <td>Detail Job List</td>                 
               </tr>
              <tr>
                 <td>Job List </td>
                 <td>:</td>
                 <td><asp:TextBox ID="tbCompJobList" runat="server" CssClass="TextBoxR" Enabled="false" 
                     EnableTheming="True" Width="200px" />
                  </td>
              </tr>
              <tr>
                 <td>Target </td>
                 <td>:</td>
                 <td><asp:TextBox ID="tbCompTarget" runat="server" CssClass="TextBoxR" Enabled="false" 
                     EnableTheming="True" Width="200px" />
                  </td>
              </tr>   
              <tr>
                 <td>Realisasi</td>
                 <td>:</td>
                <td><asp:TextBox ID="tbCompReal" runat="server" CssClass="TextBox" MaxLength="9" Width="199px" />
              </td>
              </tr>
              
               <tr>
                 <td>
                     Detail Budget
                 </td>                 
               </tr>
              <tr>
                 <td>Budget Name</td>
                 <td>:</td>
                 <td><asp:TextBox ID="tbCompBudget" runat="server" CssClass="TextBoxR" Enabled="false" 
                     EnableTheming="True" Width="200px" />
                  </td>
              </tr>
              <tr>
                 <td>
                    Actual Qty</td>
                 <td>
                    :</td>
                <td>
                <asp:TextBox ID="tbCompActQty" runat="server" CssClass="TextBox" MaxLength="9"
                     Width="71px" AutoPostBack = "true" />
              </td>
              </tr>
              <tr>
                  <td>
                    Actual Price</td>
                  <td>
                    :</td>
                  <td>
                   <asp:TextBox ID="tbCompActPrice" runat="server" CssClass="TextBox" MaxLength="9"
                    Width="71px"  AutoPostBack = "true" />
                  </td>
              </tr>                                      
              <tr>
                 <td>
                    Actual Total</td>
                 <td>
                  :</td>
                 <td>
                    <asp:TextBox ID="tbCompActTotal" runat="server" CssClass="TextBox" MaxLength="13"
                     Width="120px" enabled = "false"/>
                 </td>
             </tr>           
              <tr>
                    <td colspan="3" style="text-align: center">
                        <asp:Button class="bitbtndt btngo" Width= "80" runat="server" ID="btnCompleteProcess" Text="Complete" />									                                                                                                                                                        
                        <asp:Button class="bitbtndt btncancel" runat="server" ID="btnCancelProcess" Text="Cancel" />									                                                                                                                                                                                                                                                                                                                                                                           
                        
                    </td>
                </tr>
            </table>
            <br />
                    
            <br />
            <br />
       </asp:Panel>
    <asp:Panel runat="server" ID="pnlPrint" Visible="false">
    <cr:crystalreportviewer ID="CrystalReportViewer1" runat="server" AutoDataBind="True"
               Height="1036px" Width="928px" />
        <div>
            <cc1:WebReport ID="WebReport1" runat="server" AutoHeight="True" 
                AutoWidth="True" Height="100%" Width="100%" />
        </div>
    </asp:Panel>
      <br />            
    </div>
   
    <asp:Label runat ="server" ID="lbStatus" ForeColor="Red"/>
   
    </form>
    </body>
</html>
