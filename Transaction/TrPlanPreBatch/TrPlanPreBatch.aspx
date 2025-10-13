<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TrPlanPreBatch.aspx.vb" Inherits="Transaction_TrPlanPreBatch" %>
<%@ Register Assembly="FastReport" Namespace="FastReport.Web" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="BasicFrame.WebControls.BasicDatePicker" Namespace="BasicFrame.WebControls"
    TagPrefix="BDP" %>
<%@ Register assembly="CrystalDecisions.Web, Version=10.5.3700.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<script runat="server">

</script>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Sickness Record</title>
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
        document.getElementById("tbAmount").value = setdigit(document.getElementById("tbAmount").value.replace(/\$|\,/g,""),'<%=ViewState("DigitQty")%>');
        document.getElementById("tbTAmount").value = setdigit(document.getElementById("tbTAmount").value.replace(/\$|\,/g, ""), '<%=ViewState("DigitQty")%>');
        document.getElementById("tbTDepr").value = setdigit(document.getElementById("tbTDepr").value.replace(/\$|\,/g, ""), '<%=ViewState("DigitQty")%>');
        document.getElementById("tbTBalance").value = setdigit(document.getElementById("tbTBalance").value.replace(/\$|\,/g, ""), '<%=ViewState("DigitQty")%>');        
        
        
    } catch (err) {
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
           function deletetrans()
        {
            try
            {
                
                 var result = confirm("Sure Delete Transaction ?");
                if (result){
                    document.getElementById("HiddenRemarkDelete").value = "true";
                } else {
                    document.getElementById("HiddenRemarkDelete").value = "false";
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
    <style type="text/css">
        .style1
        {
            height: 23px;
        }
        .style2
        {
            height: 22px;
        }
    </style>
    </head>

<body>     
    <form id="form1" runat="server">
     <div class="Content">
    <div class="H1">Persiapan Planning</div>
     <hr style="color:Blue" />        
    <asp:Panel runat="server" id="PnlHd"> 
      <table>
        <tr>
            <td style="width:100px;text-align:right">Quick Search :</td>            
            <td><asp:TextBox CssClass="TextBox" runat="server" ID ="tbFilter"/> 
                  <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlField" >
                      <asp:ListItem Value="TransNmbr" Selected="True">Reference</asp:ListItem>
                      <%--<asp:ListItem Value="dbo.FormatDate(TransDate)">Date</asp:ListItem>--%>
                      <asp:ListItem>Status</asp:ListItem>
                      <asp:ListItem Value="WoNo">Master Plan</asp:ListItem>
                      <asp:ListItem Value="CheckBy">Division By</asp:ListItem>
                      <asp:ListItem Value="Remark">Year</asp:ListItem>
                      <asp:ListItem Value="Remark">Start Week</asp:ListItem> 
                      <asp:ListItem Value="Remark">End Week</asp:ListItem> 
                      <asp:ListItem Value="Remark">Remark</asp:ListItem>   
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
                      <%--<asp:ListItem Value="dbo.FormatDate(TransDate)">Date</asp:ListItem>--%>
                      <asp:ListItem>Status</asp:ListItem>
                      <asp:ListItem Value="MasterPlan">Master Plan</asp:ListItem>
                      <asp:ListItem Value="Division">Division By</asp:ListItem>
                      <asp:ListItem Value="Year">Year</asp:ListItem>
                      <asp:ListItem Value="StartWeek">Start Week</asp:ListItem> 
                      <asp:ListItem Value="EndWeek">End Week</asp:ListItem> 
                      <asp:ListItem Value="Remark">Remark</asp:ListItem>                
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
                          <asp:Button class="btngo" runat="server" ID="BtnGo" Text="G" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" CommandName="Go"  />                                                 
                      </ItemTemplate>
                      <HeaderStyle Width="110px" />
                  </asp:TemplateField>                     
                  <asp:BoundField DataField="TransNmbr" SortExpression="TransNmbr" 
                      HeaderText="Reference"></asp:BoundField>                  
                  <asp:BoundField DataField="Status" HeaderText="Status" SortExpression="Status"></asp:BoundField> 
                  <asp:BoundField DataField="TransDate" HtmlEncode="true" DataFormatString="{0:dd MMM yyyy}" HeaderText="Date" SortExpression="TransDate"></asp:BoundField>           
                  <asp:BoundField DataField="MasterPlanNo"  HeaderText="Master Plan" sortExpression="MasterPlanNo">
                      <HeaderStyle Width="100px" />
                  </asp:BoundField>    
                  
                  <asp:BoundField DataField="DivisionName"  HeaderText="Division" sortExpression="Division">
                      <HeaderStyle Width="100px" />
                  </asp:BoundField> 
                  
                  <asp:BoundField DataField="Year"  HeaderText="Year" sortExpression="Year">
                      <HeaderStyle Width="100px" />
                  </asp:BoundField>
                  
                  <asp:BoundField DataField="StartWeek"  HeaderText="Start Week" sortExpression="StartWeek">
                      <HeaderStyle Width="50px" />
                  </asp:BoundField>
                  
                   <asp:BoundField DataField="EndWeek"  HeaderText="End Week" sortExpression="EndWeek">
                      <HeaderStyle Width="50px" />
                  </asp:BoundField>
                              
                  <asp:BoundField DataField="Remark" HeaderStyle-Width="200px" HeaderText="Remark">
                      <HeaderStyle Width="200px" />
                  </asp:BoundField>
              </Columns>
          </asp:GridView>
          </div>
            <asp:Panel runat="server" ID ="pnlNav" Visible="false">
            <asp:Button class="bitbtn btnadd" runat="server" ID="btnAdd2" Text="Add" />	     
            &nbsp &nbsp &nbsp  

            <asp:DropDownList ID="ddlCommand2" CssClass="DropDownList" runat="server"/>
            <asp:Button class="btngo" runat="server" ID="btnGo2" Text="G"  />          
                        
            </asp:Panel>
    </asp:Panel>    
    <asp:Panel runat="server" ID="pnlInput" Visible="false">
      <table>
        <tr>
            <td>Reference</td>
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
        <%--<tr>
            <td>
            <asp:ImageButton ID="btnGetDt" ValidationGroup="Input" runat="server"  
                    ImageUrl="../../Image/btnGetDataOn.png"
                    onmouseover="this.src='../../Image/btnGetDataOff.png';"
                    onmouseout="this.src='../../Image/btnGetDataOn.png';"
                    ImageAlign="AbsBottom" />             
            </td>            
        </tr>--%>
          <tr>
              <td>
                  Master Plan No</td>
              <td>
                  :</td>
              <td >
                  <asp:TextBox ID="tbMasterPland" runat="server" AutoPostBack="true" CssClass="TextBox" 
                      ValidationGroup="Input" Width="151px" />
                  <asp:Button ID="btnPlan" runat="server" Class="btngo" Text="..." 
                      ValidationGroup="Input" />
              </td>
             
          </tr>
          <tr>
              <td>
                  Division</td>
              <td>
                  :</td>
              <td>
                  <asp:Dropdownlist ID="ddlDivision" runat="server" CssClass="TextBox" 
                      ValidationGroup="Input" Width="131px" Height="20px" />
              </td>
          </tr>
          <tr>
              <td>
                  Year</td>
              <td>
                  :</td>
              <td>
                  <asp:DropDownList ID="ddlYear" runat="server" CssClass="TextBox" 
                      Height="20px" ValidationGroup="Input" Width="84px" />
                  <asp:Label ID="Label6" runat="server" ForeColor="Red" Text="*" />
              </td>
          </tr>
          <tr>
              <td>
                  Start Week</td>
              <td>
                  :</td>
              <td>
                  <asp:DropDownList ID="ddlStartWeek" runat="server" CssClass="TextBox" AutoPostBack = "True" 
                      Height="20px" ValidationGroup="Input" Width="84px" />
                  <asp:Label ID="Label8" runat="server" ForeColor="Red" Text="*" />
                  &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; XPeriod&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; :&nbsp;
                  <asp:TextBox ID="tbXPeriod"  ValidationGroup="Input" runat="server" CssClass="TextBox" 
                      Width="80px" value = "1" />
                  <asp:Label ID="Label10" runat="server" ForeColor="Blue" Text="Times" />
              </td>
          </tr>
          <tr>
              <td>
                  End Week</td>
              <td>
                  :</td> 
              <td>
                  <asp:TextBox ID="tbEndWeek"  ValidationGroup="Input" runat="server" CssClass="TextBox" Enabled="false"
                      Width="80px" />
                  <asp:Label ID="Label7" runat="server" ForeColor="Red" Text="*" />
                  &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Range Period&nbsp;&nbsp;&nbsp; :&nbsp;
                  <asp:TextBox ID="tbRange"  ValidationGroup="Input" runat="server" CssClass="TextBox" 
                      Width="80px" value = "2" />
                  <asp:Label ID="Label11" runat="server" ForeColor="Blue" Text="Week" />
              </td>
          </tr>
          <tr>
              <td>
                  Qty In Period</td>
              <td>
                  :</td>
              <td>
                  <asp:TextBox ID="tbQtyPeriod"  ValidationGroup="Input" runat="server" CssClass="TextBox"
                      Width="113px" />
                  &nbsp;<asp:Label ID="Label9" runat="server" ForeColor="Blue" Text="Pkk" />
                  &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Po No&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; :&nbsp;
                  <asp:TextBox ID="tbPoNo"  ValidationGroup="Input" runat="server" CssClass="TextBox"  
                      Width="155px" />
              </td>
          </tr>
        <tr>
              <td>Remark</td>
              <td>:</td>
              <td>
                  <asp:TextBox ID="tbRemark" runat="server" CssClass="TextBox" 
                      ValidationGroup="Input" Width="402px" />
                  &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
                  &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                  <asp:Button ID="btnGenerate" runat="server" class="bitbtndt btnsavenew" 
                      Text="Generate" Height="18px" Width="112px" />
                  &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
              </td>
          </tr>
          
      </table>  
      
      <br />      
      <div style="font-size:medium; color:Blue;">Detail</div>
      <hr style="color:Blue" />  
        <asp:Panel runat="server" ID="pnlDt">
        	
        <asp:Button class="bitbtn btnadd" runat="server" ID="btnAddDt" Text="Add" ValidationGroup="Input" Enabled = "False" />	     
                 
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
                               <asp:Button class="bitbtn btnedit" runat="server" ID="btnEdit" Text="Edit" CommandName="Edit" />																						 											
						       <asp:Button class="bitbtn btndelete" runat="server" ID="btnDelete" Text="Delete" CommandName="Delete"  OnClientClick="return confirm('Sure to delete this data?');" />																						 													     
                            </ItemTemplate>
                            <EditItemTemplate>
                            		<asp:Button class="bitbtn btnsave" runat="server" ID="btnUpdate" Text="Save" CommandName="Update" />																						 																											
									
									<asp:Button class="bitbtn btncancel" runat="server" ID="btnCancel" Text="Cancel" CommandName="Cancel" />																						 													
                                
                            </EditItemTemplate>
                        </asp:TemplateField>
                        
                        <%--<asp:BoundField DataField="JobPlant" HeaderStyle-Width="100px" HeaderText="JobPlant" SortExpression="JobPlant1" >
                            <HeaderStyle Width="100px" />
                        </asp:BoundField>--%>
                        
                        <asp:BoundField DataField="JobPlant" HeaderStyle-Width="100px" HeaderText="Job Plant" SortExpression="JobPlant" >
                            <HeaderStyle Width="100px" />
                        </asp:BoundField>
                        
                        <asp:BoundField DataField="JobName" HeaderText="Job Name" HeaderStyle-Width="100px" SortExpression="BlocakName" >
                            <HeaderStyle Width="200px" />
                        </asp:BoundField>
                        
                        <asp:BoundField 
                            DataField="RotasiNo" HeaderText="Rotation" HeaderStyle-Width="100px"  SortExpression="Rotation" 
                            ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center" >
                            <FooterStyle HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>
                        
                        <asp:BoundField 
                            DataField="TeamName" HeaderText="Team" HeaderStyle-Width="200px"  SortExpression="Team" 
                            ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center" >
                            <FooterStyle HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>
                        
                        <asp:BoundField 
                            DataField="StartDate" HtmlEncode="true" DataFormatString="{0:dd MMM yyyy}" 
                            HeaderText="Start Date" SortExpression="StartDate" HeaderStyle-Width="120px" >
                            <FooterStyle HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Center" /> 
                        </asp:BoundField>
                        
                        
                        
                        <asp:BoundField 
                            DataField="EndDate" HtmlEncode="true" DataFormatString="{0:dd MMM yyyy}" 
                            HeaderText="End Date" SortExpression="EndDate" HeaderStyle-Width="120px" > 
                            <FooterStyle HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Center" /> 
                        </asp:BoundField>
                        
                      
                        <asp:BoundField DataField="Qty" HeaderText="Qty" DataFormatString="{0:#,##0.##}" HeaderStyle-Width="100px" SortExpression="Qty" >
                            <HeaderStyle Width="100px" />
                            <FooterStyle HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Center" /> 
                        </asp:BoundField>
                        
                        <asp:BoundField 
                            DataField="Unit" HeaderText="Unit" HeaderStyle-Width="100px"  SortExpression="Unit" 
                            ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center" >
                            <FooterStyle HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>
                        
                        <asp:BoundField 
                            DataField="WorkDay" HeaderText="Work Day" HeaderStyle-Width="100px"  SortExpression="WorkDay" 
                            ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center" >
                            <FooterStyle HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>
                        
                        <asp:BoundField DataField="Capacity" HeaderText="Capacity"  DataFormatString="{0:#,##0.##}" HeaderStyle-Width="100px" SortExpression="Capacity" >
                            <HeaderStyle Width="100px"  />
                            <FooterStyle HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Center" /> 
                        </asp:BoundField>
                        
                        <asp:BoundField 
                            DataField="Person" HeaderText="Person" HeaderStyle-Width="100px"  SortExpression="Person" 
                            ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center" >
                            <FooterStyle HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>  
                        
                        
                                          
                        <%--<asp:BoundField DataField="Remark" HeaderStyle-Width="200px" HeaderText="Remark" >
                            <HeaderStyle Width="200px" />
                        </asp:BoundField>--%>
                        
                    </Columns>
                </asp:GridView>
          </div>   
          <asp:Button class="bitbtn btnadd" runat="server" ID="btnAddDt2" Text="Add" ValidationGroup="Input" />	     
          
            <br />
          
       </asp:Panel>             
       <asp:Panel runat="server" ID="pnlEditDt" Visible="false">
       <table width="100%">
            <tr>
                <td style="width:60%">                
                    <table>
                        <tr>
                            <td>
                                Job</td>
                            <td>:</td>
                            <td>
                                <asp:TextBox ID="tbJob" runat="server" AutoPostBack="true" 
                                    CssClass="TextBox" ValidationGroup="Input" Width="107px" />
                                <%--<asp:TextBox ID="tbName" runat="server" CssClass="TextBoxR" Enabled="False" 
                                    EnableTheming="True" ReadOnly="True" Width="126px" />
                                <asp:Button Class="btngo" ID="btnBlock" Text="..." runat="server" 
                                    ValidationGroup="Input" />  --%>                
                             
                            </td>
                            <td>
                            </td>
                        </tr>
                        
                        <tr>
                            <td class="style1">
                                Job Name</td>
                            <td class="style1">
                                :</td>
                            <td class="style1">
                                <asp:TextBox ID="tbJobName" runat="server" AutoPostBack="true" 
                                    CssClass="TextBox" ValidationGroup="Input" Width="220px" />
                            </td>
                        </tr>
                        
                        <tr>
                            <td class="style1">
                                Rotation</td>
                            <td class="style1">
                                :</td>
                            <td class="style1">
                                <asp:TextBox ID="tbRotation" runat="server" AutoPostBack="true" 
                                    CssClass="TextBox" ValidationGroup="Input" Width="110px" />
                            </td>
                        </tr>
                        <tr>
                        <td>Team</td>
                        <td>:</td>
                        <td>
                            <asp:DropDownList ID="ddlTeam" runat="server" CssClass="TextBox" 
                                ValidationGroup="Input" Width="219px" Height="18px" />
                                <asp:TextBox ID="tbTeamName" runat="server" AutoPostBack="true" 
                                CssClass="TextBox" Visible="false" ValidationGroup="Input" Width="220px" />
                        </td>
                        </tr>
                        
                        <tr>
                            <td class="style1">
                                Start Date</td>
                            <td class="style1">
                                :</td>
                            <td class="style1">
                                <BDP:BasicDatePicker ID="tbStartDate" runat="server" AutoPostBack="True" 
                                    ButtonImageHeight="19px" ButtonImageWidth="20px" DateFormat="dd MMM yyyy" 
                                    DisplayType="TextBoxAndImage" ReadOnly="true" ShowNoneButton="False" 
                                    TextBoxStyle-CssClass="TextDate" ValidationGroup="Input">
                                    <TextBoxStyle CssClass="TextDate" />
                                </BDP:BasicDatePicker>
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; End Date:&nbsp;&nbsp;
                                <BDP:BasicDatePicker ID="tbEndDate" runat="server" AutoPostBack="True" 
                                    ButtonImageHeight="19px" ButtonImageWidth="20px" DateFormat="dd MMM yyyy" 
                                    DisplayType="TextBoxAndImage" ReadOnly="true" ShowNoneButton="False" 
                                    TextBoxStyle-CssClass="TextDate" ValidationGroup="Input">
                                    <TextBoxStyle CssClass="TextDate" />
                                </BDP:BasicDatePicker>
                            </td>
                        </tr>
                        
                         <tr>
                            <td class="style1">
                                Qty</td>
                            <td class="style1">
                                :</td>
                            <td class="style1">
                                <asp:TextBox ID="tbQty" runat="server" AutoPostBack="true" 
                                    CssClass="TextBox" ValidationGroup="Input" Width="110px" />
                            </td>
                        </tr>
                        <tr>
                            <td class="style2">
                                Unit</td>
                            <td class="style2">
                                :</td>
                            <td class="style2">
                                <asp:TextBox ID="tbUnit" AutoPostBack = "true" runat="server" CssClass="TextBox" 
                                    ValidationGroup="Input" Width="110px" />
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Capacity&nbsp; :&nbsp;
                                <asp:TextBox ID="tbCapacity" runat="server" AutoPostBack="true" 
                                    CssClass="TextBox" ValidationGroup="Input" Width="86px" />
                            </td>
                        </tr>
                        <tr>
                            <td class="style2">
                                WorkDay</td>
                            <td class="style2">
                                :</td>
                            <td class="style2">
                                <asp:TextBox ID="tbWorkDay" runat="server" AutoPostBack="true" 
                                    CssClass="TextBox" ValidationGroup="Input" Width="110px" />
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Person&nbsp; :&nbsp;
                                <asp:TextBox ID="tbPerson" runat="server" 
                                    CssClass="TextBox" ValidationGroup="Input" Width="86px" />
                            </td>
                        </tr>
                    </table>
                </td>
                <td style="vertical-align:top;width:40%">
                	&nbsp;</td>
            </tr>
       </table>
            <br />
            <asp:Button class="bitbtn btnsave" runat="server" ID="btnSaveDt" Text="Save" CommandName="Update" />																				 																											
		    <asp:Button class="bitbtn btncancel" runat="server" ID="btnCancelDt" Text="Cancel" CommandName="Cancel" />																						 													
 
            <br />
       </asp:Panel> 
       <br />    
       
       
       <asp:Button ID="btnSaveAll" runat="server" class="bitbtndt btnsavenew" Text="Save & New" validationgroup="Input" Width = "90"/>									
       <asp:Button ID="btnSaveTrans" runat="server" class="bitbtndt btnsave" Text="Save" validationgroup="Input"/>									
       <asp:Button ID="btnBack" runat="server" class="bitbtndt btncancel" Text="Cancel" validationgroup="Input"/>									
       <asp:Button ID="btnHome" runat="server" class="bitbtndt btnback" Text="Home" />									                                             
       
     
        &nbsp;									                                             
       
     
    </asp:Panel>
    <asp:Panel runat="server" ID="pnlPrint" Visible="false">
    <cr:crystalreportviewer ID="CrystalReportViewer1" runat="server" AutoDataBind="True"
               Height="1036px" Width="928px" />
   <%-- <cc1:WebReport ID="WebReport1" runat="server" AutoHeight="True" 
            AutoWidth="True" Width="100%" Height = "100%" 
            ShowRefreshButton="False" />--%>
      <br />             
    </asp:Panel>               
    </div>   
    <asp:Label runat ="server" ID="lbStatus" ForeColor="Red"/>
    <asp:HiddenField ID="HiddenRemarkClose" runat="server" />
    <asp:HiddenField ID="HiddenRemarkDelete" runat="server" />    
    </form>                            
    </body>
</html>
