<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TrRDSampleConfirm.aspx.vb" Inherits="Transaction_TrRDSampleConfirm_TrRDSampleConfirm" %>

<%@ Register Assembly="BasicFrame.WebControls.BasicDatePicker" Namespace="BasicFrame.WebControls"
    TagPrefix="BDP" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Customer Begin</title>
    <script src="../../Function/OpenDlg.js" type="text/javascript"></script>
    <script src="../../Function/Function.JS" type="text/javascript"></script>
    <script type="text/javascript">
        function setdigit(nStr, digit)
        {
        try
        {
        var TNstr = parseFloat(nStr);        
        TNstr = TNstr.toFixed(digit);                
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
        
        function setformat(){   
          try
          { 
            var _tempbaseforex = parseFloat(document.getElementById("tbBaseForex").value.replace(/\$|\,/g,""));
            var _tempppn = parseFloat(document.getElementById("tbPPN").value.replace(/\$|\,/g,""));
            var _tempppnforex = parseFloat(document.getElementById("tbPPNForex").value.replace(/\$|\,/g,""));
            var _temptotalforex = parseFloat(document.getElementById("tbTotalForex").value.replace(/\$|\,/g,""));
            var _tempRate = parseFloat(document.getElementById("tbRate").value.replace(/\$|\,/g,""));
            var _tempPPnRate = parseFloat(document.getElementById("tbPPNRate").value.replace(/\$|\,/g,""));
            
            document.getElementById("tbPPN").value = setdigit(_tempppn, '<%=ViewState("DigitPercent")%>');
            document.getElementById("tbBaseForex").value = setdigit(_tempbaseforex, '<%=ViewState("DigitCurr")%>');            
            document.getElementById("tbPPNForex").value = setdigit(_tempppnforex, '<%=ViewState("DigitCurr")%>');
            document.getElementById("tbTotalForex").value = setdigit(_temptotalforex, '<%=ViewState("DigitCurr")%>');
            document.getElementById("tbRate").value = setdigit(_tempRate,'<%=ViewState("DigitRate")%>');
            document.getElementById("tbPPNRate").value = setdigit(_tempPPnRate,'<%=ViewState("DigitRate")%>');            
          }catch (err){
            alert(err.description);
          }
        }
    </script>
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
 <link href="https://fonts.googleapis.com/css2?family=Muli:wght@300;400;600&display=swap" rel="stylesheet">
    
    <style type="text/css">
        .style1
        {
            width: 3px;
        }
        .style2
        {
            width: 295px;
        }
    </style>
    
</head>
<body>
    <form id="form1" runat="server">
    <div class="Content">
    <div class="H1">Sample Confirmation</div>
     <hr style="color:Blue" />        
    <asp:Panel runat="server" id="PnlHd"> 
      <table>
        <tr>
            <td style="width:100px;text-align:right">Quick Search :</td>            
            <td><asp:TextBox CssClass="TextBox" runat="server" ID ="tbFilter"/> 
                  <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlField" >
                      <asp:ListItem Value="TransNmbr" Selected="True">Sample No</asp:ListItem>
                      <asp:ListItem Value="SampleDate">Sample Date</asp:ListItem>
                      <asp:ListItem Value="Status">Status</asp:ListItem>
                      <asp:ListItem Value="SampleName">Sample</asp:ListItem>
                      <asp:ListItem Value="Customer_Name">Customer</asp:ListItem> 
                      <asp:ListItem Value="SampleReqNo">Sample Request No</asp:ListItem>
                      <asp:ListItem Value="dbo.FormatDate(StartPlan)">Start Planning</asp:ListItem>
                      <asp:ListItem Value="dbo.FormatDate(EndPlan)">End Planning</asp:ListItem>
                      <asp:ListItem Value="dbo.FormatDate(DeliveryDate)">Delivery Date</asp:ListItem>
                      <asp:ListItem Value="Remark">Remark</asp:ListItem>                                           
                    </asp:DropDownList>
                  <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlRange"></asp:DropDownList>
                  <asp:Button class="bitbtn btnsearch" runat="server" ID="btnSearch" Text="Search" />																						 											  
                  <asp:Button class="bitbtn btngo" runat="server" ID="btnExpand" Text="..." />																						 											   
            </td>
            <td>
                <asp:LinkButton ID="lkbAdvanceSearch" runat="server" Text="Advanced Search" />
            </td>
            <td style="width:100px; text-align: right;">
                Show Records:
            </td>
            <td>
                <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlShowRecord" AutoPostBack="true">
                    <asp:ListItem Selected="True" Value="15">Choose One</asp:ListItem>
                    <asp:ListItem Value="20">20</asp:ListItem>
                    <asp:ListItem Value="30">30</asp:ListItem>
                    <asp:ListItem Value="40">40</asp:ListItem>
                    <asp:ListItem Value="50">50</asp:ListItem>
                    <asp:ListItem Value="100">100</asp:ListItem>
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
                  <asp:ListItem Value="TransNmbr" Selected="True">Sample No</asp:ListItem>
                      <asp:ListItem Value="SampleDate">Sample Date</asp:ListItem>
                      <asp:ListItem Value="Status">Status</asp:ListItem>
                      <asp:ListItem Value="SampleName">Sample</asp:ListItem>
                      <asp:ListItem Value="Customer_Name">Customer</asp:ListItem> 
                      <asp:ListItem Value="SampleReqNo">Sample Request No</asp:ListItem>
                      <asp:ListItem Value="dbo.FormatDate(StartPlan)">Start Planning</asp:ListItem>
                      <asp:ListItem Value="dbo.FormatDate(EndPlan)">End Planning</asp:ListItem>
                      <asp:ListItem Value="dbo.FormatDate(DeliveryDate)">Delivery Date</asp:ListItem>
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
      <div style="border:0px  solid; height:100%; overflow:auto;">
          <asp:GridView ID="GridView1" runat="server" AllowPaging="True" 
              AllowSorting="true" AutoGenerateColumns="false" CssClass="Grid" 
              PageSize="15">
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
                  <asp:TemplateField ItemStyle-Width="80px">
                      <ItemTemplate>
                          <asp:DropDownList ID="ddl" runat="server" CssClass="DropDownList">
                              <asp:ListItem Selected="True" Text="View" />
                              <asp:ListItem Text="Edit" />
                              <%--<asp:ListItem Text="Delete" />--%>
                              <asp:ListItem Text="Print" />
                          </asp:DropDownList>
                          <asp:Button class="btngo" runat="server" ID="BtnGoDt" Text="G" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" CommandName="Go"  />   
                             
                          
                      </ItemTemplate>
                      <ItemStyle Width="80px" />
                  </asp:TemplateField>
                  <asp:BoundField DataField="TransNmbr" HeaderStyle-Width="180px" 
                      HeaderText="Sample No" SortExpression="Nmbr" >
                      <HeaderStyle Width="180px" />
                  </asp:BoundField>
                  <asp:BoundField DataField="Status" HeaderStyle-Width="10px" HeaderText="Status" 
                      SortExpression="Status" >
                      <HeaderStyle Width="10px" />
                  </asp:BoundField>
                  <asp:BoundField DataField="UserType" HeaderStyle-Width="180px" 
                      HeaderText="User Type" SortExpression="UserType" >
                      <HeaderStyle Width="180px" />
                  </asp:BoundField>
                  <asp:BoundField DataField="TransDate" dataformatstring="{0:dd MMM yyyy}" 
                      htmlencode="true" HeaderStyle-Width="80px" HeaderText="Date" 
                      SortExpression="TransDate" >                  
                      <HeaderStyle Width="80px" />
                  </asp:BoundField>
                  <asp:BoundField DataField="SampleDate" dataformatstring="{0:dd MMM yyyy}" 
                      htmlencode="true" HeaderStyle-Width="80px" HeaderText="Sample Date" 
                      SortExpression="SampleDate" >                  
                      <HeaderStyle Width="80px" />
                  </asp:BoundField>
                  <asp:BoundField DataField="SampleName" HeaderStyle-Width="160px" 
                      HeaderText="Sample" SortExpression="Sample" >
                      <HeaderStyle Width="160px" />
                  </asp:BoundField>
                  <asp:BoundField DataField="Customer_Name" HeaderStyle-Width="220px" 
                      HeaderText="Customer" SortExpression="Customer_Name" >
                      <HeaderStyle Width="220px" />
                  </asp:BoundField>
                  <asp:BoundField DataField="SampleReqNo" HeaderStyle-Width="150px" 
                      HeaderText="Sample Request" SortExpression="SampleReqNo" >
                      <HeaderStyle Width="150px" />
                  </asp:BoundField>
                  <asp:BoundField DataField="StartPlan" dataformatstring="{0:dd MMM yyyy}" 
                      htmlencode="true" HeaderStyle-Width="80px" HeaderText="Start Plan" 
                      SortExpression="StartPlan" >
                      <HeaderStyle Width="80px" />
                  </asp:BoundField>
                  <asp:BoundField DataField="EndPlan" dataformatstring="{0:dd MMM yyyy}" 
                      htmlencode="true" HeaderStyle-Width="80px" HeaderText="End Plan" 
                      SortExpression="EndPlan" >
                      <HeaderStyle Width="80px" />
                  </asp:BoundField>
                  <asp:BoundField DataField="DeliveryDate" dataformatstring="{0:dd MMM yyyy}" 
                      htmlencode="true" HeaderStyle-Width="80px" HeaderText="Delivery Date" 
                      SortExpression="DeliveryDate" >
                      <HeaderStyle Width="80px" />
                  </asp:BoundField>
                  <asp:BoundField DataField="Remark" HeaderStyle-Width="200px" 
                      HeaderText="Remark" SortExpression="Remark" >
                  
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
     <asp:Panel ID="pnlInput" DefaultButton="btnSave" runat="server" Visible="false">
        <table>
            <tr>
                <td>Sample No</td>
                <td class="style1">:</td>
                <td><asp:TextBox ID="tbTransNmbr" MaxLength="20" Enabled="false" runat="server" CssClass ="TextBox" />                    
                </td>                
                <td>Date</td>
                <td>:</td>
                <td class="style2">
                    <BDP:BasicDatePicker ID="tbTransDate" runat="server" DateFormat="dd MMM yyyy" 
                        ShowNoneButton = "false" ReadOnly = "true" ValidationGroup="Input"
                        ButtonImageHeight="19px" ButtonImageWidth="20px" 
                        DisplayType="TextBoxAndImage" 
                        TextBoxStyle-CssClass="TextDate"><TextBoxStyle CssClass="TextDate" /></BDP:BasicDatePicker>
                </td>
            </tr>                
             <tr>
                <td>User</td>
                <td class="style1">:</td>
                <td colspan="4">
                <asp:DropDownList CssClass="DropDownList" ID="ddlUserType" runat="server" AutoPostBack ="true">
                    <asp:ListItem Selected="True">Customer</asp:ListItem>
                    <asp:ListItem>Common</asp:ListItem>
                </asp:DropDownList>
                <asp:TextBox ID="tbCustCode" ValidationGroup="Input" AutoPostBack="true" Width="100" runat="server" CssClass="TextBox" />
                <asp:TextBox ID="tbCustName" Width="327px" runat="server" ReadOnly="true" CssClass="TextBoxR" />
                <asp:Button Class="btngo" ID="btnCust" Text="..." runat="server" ValidationGroup="Input" />                  
                </td>                
             </tr>
             <tr>
                <td>Sample</td>
                <td class="style1">:</td>
                <td colspan="4">
                <asp:TextBox ID="tbSampleCode" ValidationGroup="Input" AutoPostBack="true" Width="100" runat="server" CssClass="TextBox" />
                <asp:TextBox ID="tbSampleName" Width="408px" runat="server" 
                        ReadOnly="true" CssClass="TextBoxR" />                        
                <asp:Button Class="btngo" ID="btnSample" Text="..." runat="server" ValidationGroup="Input" />                                          
                </td>                                
             </tr>
             <tr>
                <td>Sample Request No</td>
                <td class="style1">:</td>
                <td><asp:TextBox ID="tbReqNo" ValidationGroup="Input"
                        runat="server" CssClass="TextBox" Width="200px" /></td>
                <td>Sample Date</td>
                <td>:</td>
                <td class="style2">
                    <BDP:BasicDatePicker ID="tbSampleDate" runat="server" DateFormat="dd MMM yyyy" 
                        ShowNoneButton = "false" ReadOnly = "true" ValidationGroup="Input"
                        ButtonImageHeight="19px" ButtonImageWidth="20px" 
                        DisplayType="TextBoxAndImage" 
                        TextBoxStyle-CssClass="TextDate"><TextBoxStyle CssClass="TextDate" /></BDP:BasicDatePicker>
                </td>        
             </tr>             
             <tr>
                <td>Qty Sheet / Pack</td>
                <td class="style1">:</td>
                <td><asp:TextBox ID="tbQtySheet" ValidationGroup="Input" runat="server" CssClass="TextBox" Width="60px" /> /
                    <asp:DropDownList ID="ddlUnit" ValidationGroup="Input" Runat="server" CssClass="DropDownList" Width="120px"/>
                </td>    
                <td>Qty Sample</td>
                 <td>:</td>
                 <td class="style2">
                    <asp:TextBox ID="tbQtySample" ValidationGroup="Input" runat="server" CssClass="TextBox" Width="60px"/> /
                    <asp:DropDownList ID="ddlUnit2" ValidationGroup="Input" Runat="server" CssClass="DropDownList" Width="120px"/> 
                 </td>                 
             </tr>
             <tr>
                <td>Start Planning</td>
                <td class="style1">:</td>
                <td>
                    <BDP:BasicDatePicker ID="tbStartPlan" ValidationGroup="Input" runat="server" DateFormat="dd MMM yyyy" ButtonImageHeight="19px" ButtonImageWidth="20px" 
                       ShowNoneButton = "false" DisplayType="TextBoxAndImage" TextBoxStyle-CssClass="TextDate"></BDP:BasicDatePicker>
                </td>
                <td>End Planning</td>
                <td>:</td>
                <td class="style2"><BDP:BasicDatePicker ID="tbEndPlan" ValidationGroup="Input" runat="server" DateFormat="dd MMM yyyy" ButtonImageHeight="19px" ButtonImageWidth="20px" 
                       ShowNoneButton = "false" DisplayType="TextBoxAndImage" TextBoxStyle-CssClass="TextDate"></BDP:BasicDatePicker>
                </td>
             </tr>                          
             <tr>
                <td>Delivery Date</td>
                <td class="style1">:</td>
                <td>
                    <BDP:BasicDatePicker ID="tbDeliveryDate" ValidationGroup="Input" runat="server" DateFormat="dd MMM yyyy" ButtonImageHeight="19px" ButtonImageWidth="20px" 
                       ShowNoneButton = "false" DisplayType="TextBoxAndImage" ReadOnly="true" TextBoxStyle-CssClass="TextDate"></BDP:BasicDatePicker>
                </td>
                <td>Available Material</td>
                <td class="style1">:</td>
                <td class="style2">
                    <asp:DropDownList ID="ddlMaterialAvailable" ValidationGroup="Input" Runat="server" CssClass="DropDownList" >
                        <asp:ListItem Selected="True">Available</asp:ListItem>
                        <asp:ListItem>Not Available</asp:ListItem>
                    </asp:DropDownList>
                </td>
             </tr>                          
             <tr>
                <td>Remark</td>
                <td class="style1">:</td>
                <td colspan="4"><asp:TextBox ID="tbRemark" ValidationGroup="Input" Width="553px" runat="server" 
                        CssClass="TextBoxMulti" MaxLength="255" TextMode="MultiLine" 
                        Height="94px" /></td>
             </tr>  
             <tr>
             <td colspan="6">
        			<asp:Button ID="btnSaveNew" runat="server" class="bitbtndt btnsavenew" Text="Save & New" validationgroup="Input" Width = "90"/>									
                    <asp:Button ID="btnSave" runat="server" class="bitbtndt btnsave" Text="Save" validationgroup="Input"/>									
                    <asp:Button ID="btnCancel" runat="server" class="bitbtndt btncancel" Text="Cancel" validationgroup="Input"/>									
                    <asp:Button ID="btnHome" runat="server" class="bitbtndt btnback" Text="Home" />									                                                           
                </td>
             </tr>
        </table>     
     </asp:Panel>          
    </div>   
    <asp:Label ID="lbStatus" runat="server" ForeColor="red" />     
    </form>
</body>
</html>
