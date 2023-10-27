<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TrMTNReport.aspx.vb" Inherits="Transaction_TrMTNReport_TrMTNReport" %>

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
       function setformatdt()
        {
        try
         {         
//         var Amount = document.getElementById("tbAmountForex").value.replace(/\$|\,/g,"");                           
//         document.getElementById("tbAmountForex").value = setdigit(Amount,'<%=Viewstate("DigitCurr")%>');
        document.getElementById("tbTotalForex").value = setdigit(document.getElementById("tbTotalForex").value.replace(/\$|\,/g,""),'<%=VIEWSTATE("DigitCurr")%>');
        document.getElementById("tbAmountForex").value = setdigit(document.getElementById("tbAmountForex").value.replace(/\$|\,/g,""),'<%=VIEWSTATE("DigitCurr")%>');
        }catch (err){
            alert(err.description);
          }      
        }   
    </script>
    
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
     <style type="text/css">
        .style2
        {
            width: 51px;
        }
        .style3
        {
            width: 446px;
        }
        .style5
        {
            width: 112px;
        }
        .style6
        {
            width: 29px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div class="Content">
    <div class="H1">Laporan Hasil Maintenance</div>
     <hr style="color:Blue" />        
    <asp:Panel runat="server" id="PnlHd"> 
      <table>
        <tr>
            <td style="width:100px;text-align:right">Quick Search :</td>         
            <td><asp:TextBox CssClass="TextBox" runat="server" ID ="tbFilter"/> 
                  <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlField" >     
                    <asp:ListItem Selected="True" Value="TransNmbr">LHM No</asp:ListItem>
                    <asp:ListItem Value="dbo.FormatDate(TransDate)">LHM Date</asp:ListItem>
                    <asp:ListItem>Status</asp:ListItem>
                    <asp:ListItem Value="dbo.FormatDate(ReportDate)">Report Date</asp:ListItem>
                    <asp:ListItem Value="WONO">WO No</asp:ListItem>
                    <asp:ListItem Value="Job">Job</asp:ListItem>
                    <asp:ListItem Value="JobName">Job Name</asp:ListItem>
                    <asp:ListItem Value="MTNItem">MTN Item</asp:ListItem>
                    <asp:ListItem Value="MTNItemName">MTN Item Name</asp:ListItem>
                    <asp:ListItem>Remark</asp:ListItem>
                  </asp:DropDownList>
                  <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlRange"></asp:DropDownList>
                  <asp:Button class="bitbtn btnsearch" runat="server" ID="btnSearch" Text="Search" />
                  <asp:Button class="btngo" runat="server" ID="btnExpand" Text="..."/>                  
            </td>
            <td>
                <td class="style5"><asp:LinkButton ID="LbAdvSearch" runat="server" Text="Advanced Search" /></td>
                <td class="style5" align="right">Show Records :</td>
                <td class="style2">
                <asp:DropDownList ID="ddlRow" runat="server" AutoPostBack="True" 
                    CssClass="DropDownList">
                    <asp:ListItem>20</asp:ListItem>
                    <asp:ListItem>30</asp:ListItem>
                    <asp:ListItem>40</asp:ListItem>
                    <asp:ListItem>50</asp:ListItem>
                    <asp:ListItem>100</asp:ListItem>
                    <asp:ListItem>200</asp:ListItem>
                    <asp:ListItem>300</asp:ListItem>
                </asp:DropDownList>
                &nbsp;</td>
            <td class="style6">
                Rows</td>
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
                    <asp:ListItem Selected="True" Value="TransNmbr">LHM No</asp:ListItem>
                    <asp:ListItem Value="dbo.FormatDate(TransDate)">LHM Date</asp:ListItem>
                    <asp:ListItem>Status</asp:ListItem>
                    <asp:ListItem Value="dbo.FormatDate(ReportDate)">Report Date</asp:ListItem>
                    <asp:ListItem Value="WONO">WO No</asp:ListItem>
                    <asp:ListItem Value="Job">Job</asp:ListItem>
                    <asp:ListItem Value="JobName">Job Name</asp:ListItem>
                    <asp:ListItem Value="MTNItem">MTN Item</asp:ListItem>
                    <asp:ListItem Value="MTNItemName">MTN Item Name</asp:ListItem>
                    <asp:ListItem>Remark</asp:ListItem>       
              </asp:DropDownList>
          </td>              
        </tr>        
      </table>      
      </asp:Panel>
          <br />
           <asp:Button class="bitbtn btnadd" runat="server" ID="BtnAdd" Text="Add"/>	 
            &nbsp &nbsp &nbsp   
            <asp:DropDownList CssClass="DropDownList" ID="ddlCommand" runat="server" Visible="false"/>
            <asp:Button class="btngo" runat="server" ID="BtnGo" Text="G"/>
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
                  <asp:BoundField DataField="TransNmbr" HeaderStyle-Width="120px" SortExpression="TransNmbr" HeaderText="LHM No"></asp:BoundField>                  
                  <asp:BoundField DataField="Status" SortExpression="Status" HeaderText="Status"></asp:BoundField>                  
                  <asp:BoundField DataField="TransDate" dataformatstring="{0:dd MMM yyyy}" htmlencode="true" HeaderStyle-Width="80px" SortExpression="TransDate" HeaderText="LHM Date"></asp:BoundField>
                  <asp:BoundField DataField="ReportDate" dataformatstring="{0:dd MMM yyyy}" htmlencode="true" HeaderStyle-Width="80px" SortExpression="ReportDate" HeaderText="Report Date"></asp:BoundField>
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
            <td>LHM No</td>
            <td>:</td>
            <td><asp:TextBox CssClass="TextBoxR" runat="server" ID="tbCode" Width="150px" Enabled="False"/></td>
            
            <td>Date</td>
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
        <%--TransNmbr, TransDate, PettyType, PettyName, Currency, ForexRate, TotalForex, PayTo, Remark--%>
        
        <tr>
            <td>Report Date</td>
            <td>:</td>
            <td>
            <BDP:BasicDatePicker ID="tbReportDate" runat="server" DateFormat="dd MMM yyyy" 
                        ReadOnly = "true" ValidationGroup="Input"
                        ButtonImageHeight="19px" ButtonImageWidth="20px" 
                        DisplayType="TextBoxAndImage" 
                        TextBoxStyle-CssClass="TextDate" 
                        ShowNoneButton="False"><TextBoxStyle CssClass="TextDate" /></BDP:BasicDatePicker>                
                <asp:Button ID="btnGetDt" runat="server" Class="bitbtndt btngetitem" 
                    Text="Get Data" ValidationGroup="Input" Width="70" />
            </td>
            
            
        </tr>
        
          <tr>
              <td>
                  Remark</td>
              <td>
                  :</td>
              <td colspan="4">
                  <asp:TextBox ID="tbRemark" runat="server" CssClass="TextBoxMulti" TextMode="MultiLine"
                      ValidationGroup="Input" Width="355px" />
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
                            <asp:BoundField DataField="WONO" HeaderText="WO No" />
                            <asp:BoundField DataField="ItemNo" HeaderText="No" />
                            <asp:BoundField DataField="MTNItemName" HeaderStyle-Width="150px" HeaderText="MTN Item" />
                            <asp:BoundField DataField="Job" HeaderStyle-Width="80px" HeaderText="Job" />
                            <asp:BoundField DataField="JobName" HeaderStyle-Width="150px" HeaderText="Job Name" />
                            <asp:BoundField DataField="Problem" HeaderStyle-Width="150px" HeaderText="Problem" />
                            <asp:BoundField DataField="Cause" HeaderStyle-Width="150px" HeaderText="Cause" />
                            <asp:BoundField DataField="Recommendation" HeaderStyle-Width="150px" HeaderText="Action" />
                            <asp:BoundField DataField="FgMTNItemWorkNormal" HeaderStyle-Width="50px" HeaderText="Work Normal" />
                            <asp:BoundField DataField="PIC" HeaderStyle-Width="150px" HeaderText="PIC" />
                            
                        </Columns>
                    </asp:GridView>
              </div>   
              <asp:Button class="bitbtndt btnadd" runat="server" ID="btnAddDtke2" Text="Add" ValidationGroup="Input" />
              </asp:Panel> 
              <asp:Panel runat="server" ID="pnlEditDt" Visible="false">
                <table>             
                    <tr>                    
                        <td>WO No</td>
                        <td>:</td>
                        <td>                                
                            <asp:TextBox CssClass="TextBox" runat="server" ID="tbWONO" Enabled="false" Width="225px"/>
                            <asp:Button class="btngo" runat="server" ID="btnWONO" Text="..." ValidationGroup="Input"/>            
                            <asp:Label ID="Label4" runat="server" ForeColor="Red">*</asp:Label>
                        </td>
                    </tr>
                    <tr>                    
                        <td>Item No</td>
                        <td>:</td>
                        <td>
                            <asp:Label ID="lbItemNo" runat="server" Text="itemmmm noooooooo" />
                        </td>
                    </tr>                    
                    <tr>
                        <td>
                            MTN Item</td>
                        <td>
                            :</td>
                        <td>
                            <asp:TextBox CssClass="TextBox" runat="server" ID="tbMtnItem" Enabled="False" 
                                Width="60px" />
                            <asp:TextBox CssClass="TextBox" runat="server" ID="tbMtnItemName" Enabled="false" Width="225px"/>
                        </td>
                    </tr>
                    <tr>
                    <%--CostCtr, CostCtrName, AmountForex, Remark
                        <td>Cost Center</td>
                        <td>:</td>
                        <td colspan="4">                                
                            <asp:DropDownList ValidationGroup="Input" CssClass="DropDownList" runat="server" ID="ddlCostCenter" Width="180px"/>
                        </td>
                   </tr>--%>
                   <tr>     
                        <td>Job</td>
                        <td>:</td>
                        
                        <td>
                        <asp:TextBox CssClass="TextBox" runat="server" ID="tbJob" Enabled="False" 
                                Width="60px"/>
                        <asp:TextBox CssClass="TextBox" runat="server" ID="tbJobName" Enabled="false" Width="225px"/>
                        <%--<asp:Button class="btngo" runat="server" ID="btnJob" Text="..." ValidationGroup="Input"/> 
                            <asp:Label ID="Label6" runat="server" ForeColor="Red">*</asp:Label>--%>
                        </td>
                    </tr>                       
                    <tr>
                        <td>Problem </td>
                        <td>:</td>
                        <td><asp:TextBox runat="server" ID="tbProblem" CssClass="TextBoxMulti" Width="365px" 
                                MaxLength="255" TextMode="MultiLine" />                        
                        </td>
                    </tr>
                    <tr>
                        <td>Cause </td>
                        <td>:</td>
                        <td><asp:TextBox runat="server" ID="tbCause" CssClass="TextBoxMulti" Width="365px" 
                                MaxLength="255" TextMode="MultiLine" />                        
                        </td>
                    </tr>
                    <tr>
                        <td>Action </td>
                        <td>:</td>
                        <td><asp:TextBox runat="server" ID="tbRecommendation" CssClass="TextBoxMulti" Width="365px" 
                                MaxLength="255" TextMode="MultiLine" />                        
                        </td>
                    </tr>
                    <tr>                    
                        <td>Work Normal</td>
                        <td>:</td>
                        <td>                                
                            <asp:DropDownList ID="ddlFgNormal" runat="server" AutoPostBack="True" 
                                CssClass="DropDownList" Height="16px" ValidationGroup="Input" Width="50px">
                                <asp:ListItem>Y</asp:ListItem>
                                <asp:ListItem Selected="True">N</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                        <tr>
                            <td>
                                PIC</td>
                            <td>
                                :</td>
                            <td>
                                <asp:TextBox ID="tbPIC" runat="server" CssClass="TextBox" Width="225px" />
                                <asp:Label ID="Label7" runat="server" ForeColor="Red">*</asp:Label>
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
