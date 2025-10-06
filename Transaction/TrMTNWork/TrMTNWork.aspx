<%@ Page MaintainScrollPositionOnPostback ="true" Language="VB" AutoEventWireup="false" CodeFile="TrMTNWork.aspx.vb" Inherits="Transaction_TrMTNWork_TrMTNWork" %>

<%@ Register Assembly="BasicFrame.WebControls.BasicDatePicker" Namespace="BasicFrame.WebControls"
    TagPrefix="BDP" %>
    
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
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
        
        function openprintdlg2ds2()
        {
            var wOpen;
            wOpen = window.open("../../Rpt/PrintForm2.Aspx", "List","scrollbars=yes,resizable=yes,width=500,height=400");            
            wOpen.moveTo(0, 0);
            wOpen.resizeTo(screen.width, screen.height);                        
            return false; 
        }                                      
       
    </script>
    
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
 <link href="https://fonts.googleapis.com/css2?family=Muli:wght@300;400;600&display=swap" rel="stylesheet">
</head>
<body>
    <form id="form1" runat="server">    
    <div class="Content">
    <div class="H1">Maintenance Order</div>
     <hr style="color:Blue" />        
    <asp:Panel runat="server" id="PnlHd"> 
      <table>
        <tr>
            <td style="width:100px;text-align:right">Quick Search :</td>            
            <td><asp:TextBox CssClass="TextBox" runat="server" ID ="tbFilter"/> 
                  <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlField" >                      
                    <asp:ListItem Selected="True" Value="TransNmbr">MO No</asp:ListItem>
                    <asp:ListItem Value="dbo.FormatDate(TransDate)">MO Date</asp:ListItem>
                    <asp:ListItem>Status</asp:ListItem>
                    <asp:ListItem Value="ReffNmbr">Reff. Nmbr</asp:ListItem>
                    <asp:ListItem Value="Dept_Name">Department</asp:ListItem>
                    <asp:ListItem Value="RequestBy">Request By</asp:ListItem>
                    <asp:ListItem Value="dbo.FormatDate(RequestDate)">Request Date</asp:ListItem>
                    <asp:ListItem Value="FgOutSource">Fg Outsource</asp:ListItem>
                    <asp:ListItem Value="OutSourceTo">Outsource To</asp:ListItem>
                    <asp:ListItem Value="Remark">Remark</asp:ListItem>
                    
                  </asp:DropDownList>
                  <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlRange"></asp:DropDownList>
                 
                  <asp:Button class="bitbtn btnsearch" runat="server" ID="btnSearch" Text="Search" />																						 											  
                  <asp:Button class="bitbtn btngo" runat="server" ID="btnExpand" Text="..." />																						 											   
            </td>
            <td>
                <asp:LinkButton ID="LbAdvSearch" runat="server" Text="Advanced Search" />
            </td>
            <td class="style1">&nbsp;</td>
            <td>Show Records :</td>
            <td>
                <asp:DropDownList ID="ddlRow" runat="server" AutoPostBack="True" 
                    CssClass="DropDownList">
                    <asp:ListItem Selected="True" Value="15">Choose One</asp:ListItem>
                    <asp:ListItem>20</asp:ListItem>
                    <asp:ListItem>30</asp:ListItem>
                    <asp:ListItem>40</asp:ListItem>
                    <asp:ListItem>50</asp:ListItem>
                    <asp:ListItem>100</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td>Rows</td> 
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
                    <asp:ListItem Selected="True" Value="TransNmbr">MO No</asp:ListItem>
                    <asp:ListItem Value="dbo.FormatDate(TransDate)">MO Date</asp:ListItem>
                    <asp:ListItem>Status</asp:ListItem>
                    <asp:ListItem Value="ReffNmbr">Reff. Nmbr</asp:ListItem>
                    <asp:ListItem Value="Dept_Name">Department</asp:ListItem>
                    <asp:ListItem Value="RequestBy">Request By</asp:ListItem>
                    <asp:ListItem Value="dbo.FormatDate(RequestDate)">Request Date</asp:ListItem>
                    <asp:ListItem Value="FgOutSource">Fg Outsource</asp:ListItem>
                    <asp:ListItem Value="OutSourceTo">Outsource To</asp:ListItem>
                    <asp:ListItem Value="Remark">Remark</asp:ListItem>    
              </asp:DropDownList>
          </td>  
                     
        </tr>        
      </table>      
      </asp:Panel>
          <br />
            <asp:Button class="bitbtn btnadd" runat="server" ID="BtnAdd" Text="Add" />	  &nbsp &nbsp &nbsp   
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
                              <asp:ListItem Text="Revisi" />
                              <asp:ListItem Text="Print" />
                              <%--<asp:ListItem Text="Print Full" />--%>
                          </asp:DropDownList>
                          <asp:Button class="btngo" runat="server" ID="BtnGo" Text="G" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" CommandName="Go" />                      </ItemTemplate>
                      <HeaderStyle Width="110px" />
                  </asp:TemplateField>
                  <asp:BoundField DataField="TransNmbr" HeaderStyle-Width="120px" SortExpression="Nmbr" HeaderText="MO No"></asp:BoundField>                  
                  <asp:BoundField DataField="Revisi" HeaderText="Revisi"></asp:BoundField>
                  <asp:BoundField DataField="Status" HeaderText="Status"></asp:BoundField>                  
                  <asp:BoundField DataField="TransDate" dataformatstring="{0:dd MMM yyyy}" htmlencode="true" HeaderStyle-Width="80px" SortExpression="TransDate" ItemStyle-wrap="true" HeaderText="MO Date" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" > </asp:BoundField>
                  <asp:BoundField DataField="ReffNmbr" HeaderStyle-Width="200px" SortExpression="ReffNmbr" HeaderText="Reff. Nmbr"></asp:BoundField>
                  <asp:BoundField DataField="Dept_Name" HeaderStyle-Width="150px" SortExpression="Dept_Name" HeaderText="Department"></asp:BoundField>
                  <asp:BoundField DataField="RequestBy" HeaderStyle-Width="80px" SortExpression="RequestBy" HeaderText="Request By"></asp:BoundField>
                  <asp:BoundField DataField="RequestDate" dataformatstring="{0:dd MMM yyyy}" HeaderStyle-Width="80px" SortExpression="RequestDate" HeaderText="Request Date"></asp:BoundField>
                  <asp:BoundField DataField="FgOutSource" HeaderStyle-Width="80px" SortExpression="FgOutSource" HeaderText="Fg OutSource"></asp:BoundField>
                  <asp:BoundField DataField="OutSourceTo" HeaderStyle-Width="80px" SortExpression="OutSourceTo" HeaderText="OutSource To"></asp:BoundField>
                  <asp:BoundField DataField="Remark" HeaderStyle-Width="250px" HeaderText="Remark"></asp:BoundField>
              </Columns>
          </asp:GridView>
          </div>
            <asp:Panel runat="server" ID ="pnlNav" Visible="false">           
            <asp:Button class="bitbtn btnadd" runat="server" ID="btnAdd2" Text="Add" />	            &nbsp &nbsp &nbsp
            <asp:DropDownList ID="ddlCommand2" CssClass="DropDownList" runat="server"/>
            <asp:Button class="btngo" runat="server" ID="btnGo2" Text="G" />                      
            </asp:Panel>
    </asp:Panel>    
    <asp:Panel runat="server" ID="pnlInput" Visible="false">
      <table>
        <tr>
            <td>MO No</td>
            <td>:</td>
            <td><asp:TextBox  CssClass="TextBoxR" runat="server" ID="tbCode" Width="150px" Enabled="False"/>
            <asp:Label runat="server" ID="Label3" Text =" Rev : "></asp:Label>
            <asp:Label runat="server" ID="lbRevisi"></asp:Label>
            </td>
        
            <td>MO Date</td>
            <td>:</td>
            <td>
            <BDP:BasicDatePicker ID="tbDate" runat="server" DateFormat="dd MMM yyyy" 
                        ReadOnly = "true" ValidationGroup="Input"
                        ButtonImageHeight="19px" ButtonImageWidth="20px" 
                        DisplayType="TextBoxAndImage" 
                        TextBoxStyle-CssClass="TextDate" 
                        ShowNoneButton="False"><TextBoxStyle CssClass="TextDate" /></BDP:BasicDatePicker>                
            </td>            
        </tr> 
             
        
        <%--<tr>
            <td>Reff No</td>
            <td>:</td>
            <td colspan="4"><asp:TextBox runat="server" MaxLength = "20" ValidationGroup="Input" ID="tbReffNmbr"  CssClass="TextBox" Width="225px"/></td>
        </tr>--%>
        <tr>            
            <td>Department</td>
            <td>:</td>
            <td colspan="3">
                <asp:DropDownList ID="ddlDept" runat="server" CssClass="DropDownList" ValidationGroup="Input" Width="225px" />
                <asp:Label runat ="server" ID="Label1" Text="*" ForeColor="Red"/>
            </td>
        </tr>
        <tr>
            <td>Request By</td>
            <td>:</td>
            <td colspan="4"><asp:TextBox runat="server" MaxLength = "12" ValidationGroup="Input" ID="tbRequestBy"  CssClass="TextBox" Width="225px"/>
            <asp:Label runat ="server" ID="Label4" Text="*" ForeColor="Red"/>
            </td>
            
        </tr> 
        <tr>
            <td>Request Date</td>
            <td>:</td>
            <td>
            <BDP:BasicDatePicker ID="tbRequestDate" runat="server" DateFormat="dd MMM yyyy" 
                        ReadOnly = "true" ValidationGroup="Input"
                        ButtonImageHeight="19px" ButtonImageWidth="20px" 
                        DisplayType="TextBoxAndImage" 
                        TextBoxStyle-CssClass="TextDate" 
                        ShowNoneButton="False"><TextBoxStyle CssClass="TextDate" /></BDP:BasicDatePicker>                
            </td>            
        </tr> 
        <tr>            
            <td>Outsource</td>
            <td>:</td>
            <td colspan="3">
                <asp:DropDownList ID="ddlOutsource" runat="server" AutoPostBack="true" 
                    CssClass="DropDownList" Enabled="true" ValidationGroup="Input" Width="74px">
                    <asp:ListItem>Y</asp:ListItem>
                    <asp:ListItem Selected="True">N</asp:ListItem>
                </asp:DropDownList>
                <asp:TextBox runat="server" MaxLength = "100" ID="tbOutsource" Enabled="false" CssClass="TextBox" Width="225px"/>
            </td>
        </tr>
        <tr>
            <td>Remark</td>
            <td>:</td>
            <td colspan="4">
                <asp:TextBox ID="tbRemark" runat="server"  ValidationGroup="Input" CssClass="TextBox" MaxLength="255" 
                    TextMode="MultiLine" Width="365px"  />
<%--                <asp:RegularExpressionValidator runat="server" ID="valInput"
                    ControlToValidate="tbRemark"
                    ValidationExpression="^[\s\S]{0,60}$"
                    ErrorMessage="Please enter a maximum of 60 characters"
                    Display="Dynamic">*--%>
                <%--</asp:RegularExpressionValidator>    --%>
            </td>
            <td>               
                <asp:Panel runat="server" ID ="pnlSchedule" Visible=false >
                <table bgcolor=silver>
                <tr>
                    <td></td>
                    <td></td>
                    <td>Year</td>
                    <td>Month</td>
                    <td>Week</td>
                    <td></td>
                    <td>Year</td>
                    <td>Month</td>
                    <td>Week</td>
                    <td></td>
                </tr>
                <tr>                
                    <td>Period</td>
                    <td>:</td>
                    <td><asp:DropDownList runat="server" ID="ddlYear1" CssClass="DropDownList" /></td>
                    <td><asp:DropDownList runat="server" ID="ddlMonth1" CssClass="DropDownList" >
                     <asp:ListItem Selected="true" Text="1" Value="1"></asp:ListItem>
                     <asp:ListItem Text="2" Value="2"></asp:ListItem>
                     <asp:ListItem Text="3" Value="3"></asp:ListItem>
                     <asp:ListItem Text="4" Value="4"></asp:ListItem>
                     <asp:ListItem Text="5" Value="5"></asp:ListItem>
                     <asp:ListItem Text="6" Value="6"></asp:ListItem>
                     <asp:ListItem Text="7" Value="7"></asp:ListItem>
                     <asp:ListItem Text="8" Value="8"></asp:ListItem>
                     <asp:ListItem Text="9" Value="9"></asp:ListItem>
                     <asp:ListItem Text="10" Value="10"></asp:ListItem>
                     <asp:ListItem Text="11" Value="11"></asp:ListItem>
                     <asp:ListItem Text="12" Value="12"></asp:ListItem>
                     </asp:DropDownList> 
                    </td>
                    <td><asp:DropDownList runat="server" ID="ddlWeek1" CssClass="DropDownList" >
                     <asp:ListItem Selected="true" Text="1" Value="1"></asp:ListItem>
                     <asp:ListItem Text="2" Value="2"></asp:ListItem>
                     <asp:ListItem Text="3" Value="3"></asp:ListItem>
                     <asp:ListItem Text="4" Value="4"></asp:ListItem>
                     <asp:ListItem Text="5" Value="5"></asp:ListItem>                     
                     </asp:DropDownList>                    
                    </td>
                    <td>s/d</td>
                    <td><asp:DropDownList runat="server" ID="ddlYear2" CssClass="DropDownList" /></td>
                    <td><asp:DropDownList runat="server" ID="ddlMonth2" CssClass="DropDownList" >
                     <asp:ListItem Selected="true" Text="1" Value="1"></asp:ListItem>
                     <asp:ListItem Text="2" Value="2"></asp:ListItem>
                     <asp:ListItem Text="3" Value="3"></asp:ListItem>
                     <asp:ListItem Text="4" Value="4"></asp:ListItem>
                     <asp:ListItem Text="5" Value="5"></asp:ListItem>
                     <asp:ListItem Text="6" Value="6"></asp:ListItem>
                     <asp:ListItem Text="7" Value="7"></asp:ListItem>
                     <asp:ListItem Text="8" Value="8"></asp:ListItem>
                     <asp:ListItem Text="9" Value="9"></asp:ListItem>
                     <asp:ListItem Text="10" Value="10"></asp:ListItem>
                     <asp:ListItem Text="11" Value="11"></asp:ListItem>
                     <asp:ListItem Text="12" Value="12"></asp:ListItem>
                     </asp:DropDownList> 
                    </td>
                    <td><asp:DropDownList runat="server" ID="ddlWeek2" CssClass="DropDownList" >
                     <asp:ListItem Selected="true" Text="1" Value="1"></asp:ListItem>
                     <asp:ListItem Text="2" Value="2"></asp:ListItem>
                     <asp:ListItem Text="3" Value="3"></asp:ListItem>
                     <asp:ListItem Text="4" Value="4"></asp:ListItem>
                     <asp:ListItem Text="5" Value="5"></asp:ListItem>                     
                     </asp:DropDownList>                    
                    </td> 
                    <td><asp:Button class="bitbtn btngetitem" runat="server" ID="btnGetData" Width="100px" Text="Get Schedule" ValidationGroup="Input"/></td>
                </tr>              
                
                </table>
                </asp:Panel>
            </td>
        </tr>
      </table>  
      
      <br />            
      <hr style="color:Blue" />  
       <asp:Menu
            ID="Menu1"            
            runat="server"
            CssClass = "Menu"        
            StaticMenuItemStyle-CssClass = "MenuItem"
            StaticSelectedStyle-CssClass = "MenuSelect"        
            Orientation="Horizontal"
            ItemWrap = "False"
            StaticEnableDefaultPopOutImage="False">            
            <Items>
                <asp:MenuItem Text="Detail Job" Value="0"></asp:MenuItem>
                <asp:MenuItem Text="Detail Sparepart" Value="1"></asp:MenuItem>
            </Items>            
        </asp:Menu>
        <br />
        <asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex="0">
           <asp:View ID="Tab1" runat="server">
              <asp:Panel runat="server" ID="PnlDt">
                <asp:Button class="bitbtn btnadd" runat="server" ID="btnAdddt" Text="Add" ValidationGroup="Input" />	
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
                                       <asp:Button ID="btnEdit" runat="server" class="bitbtndt btnedit" Text="Edit" CommandName="Edit"/>
								       <asp:Button ID="btnDelete" runat="server" class="bitbtndt btndelete" Text="Delete" OnClientClick="return confirm('Sure to delete this data?');" CommandName="Delete"/>									                                      
                                      </ItemTemplate>
                            </asp:TemplateField>                                
                            <asp:BoundField DataField="ItemNo" HeaderText="No" />
                            <asp:BoundField DataField="MTNItem" HeaderStyle-Width="80px" HeaderText="Maintenance Item" />                                
                            <asp:BoundField DataField="MTN_Item_Name" HeaderStyle-Width="160px" HeaderText="Maintenance Item Name" />                                                                                       
                            <asp:BoundField DataField="Job" HeaderStyle-Width="80px" HeaderText="Job" />                                
                            <asp:BoundField DataField="JobName" HeaderStyle-Width="160px" HeaderText="Job Name" />                     
                            <asp:BoundField DataField="Problem" HeaderStyle-Width="160px" HeaderText="Problem" />                                
                            <asp:BoundField DataField="PIC" HeaderStyle-Width="160px" HeaderText="PIC" />                                
                            <asp:BoundField DataField="Remark" HeaderStyle-Width="200px" HeaderText="Remark" />
                                                            
                        </Columns>
                    </asp:GridView>
              </div>   
              <asp:Button class="bitbtn btnadd" runat="server" ID="btnAddDtke2" Text="Add" ValidationGroup="Input" />	
              </asp:Panel> 
              <asp:Panel runat="server" ID="pnlEditDt" Visible="false">
                <table>              
                    <tr>
                        <td>Item No</td>
                        <td>:</td>
                        <td colspan="4"><asp:Label ID="lbItemNo" runat="server" Text="itemmm" />
                        </td>           
                    </tr>        
                    <tr>
                        <td>Maintenance</td>
                        <td>:</td>
                        <td colspan="4"><asp:TextBox CssClass="TextBox" ValidationGroup="Input" runat="server" ID="tbMTNItemCode" AutoPostBack="true" MaxLength=20 />
                            <asp:TextBox CssClass="TextBox" runat="server" ID="tbMTNItemName" Enabled="false" Width="225px"/>                
                            <asp:Button Class="btngo" ID="btnMTNItem" Text="..." runat="server" ValidationGroup="Input" />                                                 
                            <asp:Label ID="lbItemNo7" runat="server" ForeColor="#FF3300" Text="*" />
                        </td>
                    </tr>
                    <tr>
                        <td>Job</td>
                        <td>:</td>
                        <td colspan="4"><asp:TextBox CssClass="TextBox" ValidationGroup="Input" runat="server" ID="tbJobCode" MaxLength=5 AutoPostBack="true" />
                            <asp:TextBox CssClass="TextBox" runat="server" ID="tbJobName" Enabled="false" Width="225px"/>                                            
                        </td>
                    </tr>
                    <tr>
                        <td>Problem</td>
                        <td>:</td>
                        <td colspan="4"><asp:TextBox runat="server" MaxLength = "255" ValidationGroup="Input" ID="tbProblem"  CssClass="TextBox" Width="225px"/></td>
                    </tr>
                    
                    <tr>
                        <td>PIC</td>
                        <td>:</td>
                        <td colspan="4"><asp:TextBox runat="server" MaxLength = "60" ValidationGroup="Input" ID="tbPIC"  CssClass="TextBox" Width="225px"/></td>
                    </tr>           
                    
                    <tr>
                        <td>Remark </td>
                        <td>:</td>
                        <td colspan="4"><asp:TextBox runat="server" ID="tbRemarkDt" CssClass="TextBox" Width="365px" 
                                MaxLength="255" TextMode="MultiLine"  />                        
                        </td>
                    </tr>
                </table>
                <br />                     
               
               <asp:Button ID="btnSaveDt" runat="server" class="bitbtndt btnsave" Text="Save" />									
               <asp:Button ID="btnCancelDt" runat="server" class="bitbtndt btncancel" Text="Cancel" />									
           </asp:Panel> 
              
           </asp:View>           
            <asp:View ID="Tab2" runat="server">
                <asp:Panel ID="pnlDt2" runat="server">  
                <asp:Button class="bitbtn btnadd" runat="server" ID="btnAddDt2" Text="Add" ValidationGroup="Input" />	              
                &nbsp &nbsp &nbsp 
                <asp:Button class="bitbtn btnadd" runat="server" ID="btnGenerate" Text="Generate Sparepart" ValidationGroup="Input" />
                
                <div style="border:0px  solid; width:100%; height:100%; overflow:auto;">
                    <asp:GridView ID="GridDt2" runat="server" AutoGenerateColumns="False" 
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
                            
                            <asp:BoundField DataField="Sparepart" HeaderText="Sparepart" />
                            <asp:BoundField DataField="Sparepart_Name" HeaderStyle-Width="150px" HeaderText="Sparepart Name" />
                            <asp:BoundField DataField="Qty" HeaderStyle-Width="150px" HeaderText="Qty" />                                                            
                            <asp:BoundField DataField="Unit" HeaderStyle-Width="120px" HeaderText="Unit" />                                                           
                                                                                 
                        </Columns>
                    </asp:GridView>
              </div>    
              <asp:Button class="bitbtn btnadd" runat="server" ID="btnAddDt2Ke2" Text="Add" ValidationGroup="Input" />	              
   
              </asp:Panel>
              <asp:Panel runat="server" ID="pnlEditDt2" Visible="false">
                <table>              
                    <tr>
                        <td>Sparepart</td>
                        <td>:</td>
                        <td colspan="4"><asp:TextBox CssClass="TextBox" ValidationGroup="Input" runat="server" MaxLength=20 ID="tbSparepartCode" AutoPostBack="true" />
                            <asp:TextBox CssClass="TextBox" runat="server" ID="tbSparepartName" Enabled="false" Width="225px"/>                
                            <asp:Button Class="btngo" ID="btnSparepart" Text="..." runat="server" ValidationGroup="Input" />                                                 
                            <asp:Label ID="Label2" runat="server" ForeColor="#FF3300" Text="*" />
                        </td>
                    </tr>
                    <tr>
                        <td>Qty</td>
                        <td>:</td>
                        <td><asp:TextBox CssClass="TextBox" ValidationGroup="Input" runat="server" ID="tbQty" />                                                        
                        </td>
                        <td>Unit</td>
                        <td>:</td>
                        <td><asp:TextBox CssClass="TextBox" runat="server" ID="tbUnit" Enabled="false" Width="50px"/>                                            
                            
                        </td>
                    </tr>
                    
                </table>
                <br />                     
                <asp:Button ID="btnSaveDt2" runat="server" class="bitbtndt btnsave" Text="Save" />									
                <asp:Button ID="btnCancelDt2" runat="server" class="bitbtndt btncancel" Text="Cancel" />									


           </asp:Panel> 
               
            </asp:View>            
        </asp:MultiView>
    
       <br /> 
                
        <asp:Button ID="btnSaveAll" runat="server" class="bitbtndt btnsavenew" Text="Save & New" validationgroup="Input" Width = "90"/>									
        <asp:Button ID="btnSaveTrans" runat="server" class="bitbtndt btnsave" Text="Save" validationgroup="Input"/>									
        <asp:Button ID="btnBack" runat="server" class="bitbtndt btncancel" Text="Cancel" validationgroup="Input"/>									
        <asp:Button ID="btnHome" runat="server" class="bitbtndt btnback" Text="Home" />									                                                           
            
    </asp:Panel>        
    </div>
   
    <asp:Label runat ="server" ID="lbStatus" ForeColor="Red"/>
    <asp:HiddenField ID="HiddenRemarkRevisi" runat="server" />
   
    </form>
</body>
</html>
