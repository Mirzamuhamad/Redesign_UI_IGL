<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TrBeginGiroIn.Aspx.vb" Inherits="Transaction_TrBeginGiroIn_TrBeginGiroIn" %>

<%@ Register Assembly="BasicFrame.WebControls.BasicDatePicker" Namespace="BasicFrame.WebControls"
    TagPrefix="BDP" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Begin Giro Receipt</title>
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
            var _temptotalforex = parseFloat(document.getElementById("tbTotalForex").value.replace(/\$|\,/g,""));
            var _tempRate = parseFloat(document.getElementById("tbRate").value.replace(/\$|\,/g,""));
            if(isNaN(_temptotalforex) == true)
            {
                _temptotalforex = 0;
            }  
            if(isNaN(_tempRate) == true)
            {
                _tempRate = 0;
            }                   
            document.getElementById("tbTotalForex").value = setdigit(_temptotalforex, '<%=ViewState("DigitCurr")%>');
            document.getElementById("tbRate").value = setdigit(_tempRate,'<%=ViewState("DigitRate")%>');            
            }catch (err){
            alert(err.description);
          }
        }    
    </script>
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
 <link href="https://fonts.googleapis.com/css2?family=Muli:wght@300;400;600&display=swap" rel="stylesheet">
    
</head>
<body>
    <form id="form1" runat="server">
    <div class="Content">
    <div class="H1">BEGIN GIRO RECEIPT</div>
     <hr style="color:Blue" />        
    <asp:Panel runat="server" id="PnlHd"> 
      <table>
        <tr>
            <td style="width:100px;text-align:right">Quick Search :</td>            
            <td><asp:TextBox CssClass="TextBox" runat="server" ID ="tbFilter"/> 
                  <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlField" >
                      <asp:ListItem Value="GiroNo" Selected="True">Giro No</asp:ListItem>
                      <asp:ListItem Value="Status">Status</asp:ListItem>
                      <asp:ListItem Value="UserType">User Type</asp:ListItem>
                      <asp:ListItem Value="UserCode">User Code</asp:ListItem>
                      <asp:ListItem Value="UserName">User Name</asp:ListItem>
                      <asp:ListItem Value="ReceiptNo">Receipt No</asp:ListItem>
                      <asp:ListItem Value="ReceiptDate">Receipt Date</asp:ListItem>
                      <asp:ListItem Value="BankGiro">Bank Giro</asp:ListItem>
                      <asp:ListItem Value="Currency">Currency</asp:ListItem>
                      <asp:ListItem Value="DueDate">Due Date</asp:ListItem>
                      <asp:ListItem Value="Remark">Remark</asp:ListItem>
                    </asp:DropDownList>
                    <asp:Button class="bitbtn btnsearch" runat="server" ID="Button1" Text="Search" />                 
                    <asp:Button class="btngo" runat="server" ID="btnExpand" Text="..."/>                      
            </td>
            <td>
                <asp:LinkButton ID="LinkButton1" runat="server" Text="Advanced Search" />
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
                  <asp:ListItem Value="GiroNo" Selected="True">Giro No</asp:ListItem>                      
                  <asp:ListItem Value="Status">Status</asp:ListItem>
                  <asp:ListItem Value="UserType">User Type</asp:ListItem>
                  <asp:ListItem Value="UserCode">User Code</asp:ListItem>
                  <asp:ListItem Value="UserName">User Name</asp:ListItem>
                  <asp:ListItem Value="ReceiptNo">Receipt No</asp:ListItem>
                  <asp:ListItem Value="ReceiptDate">Receipt Date</asp:ListItem>
                  <asp:ListItem Value="BankGiro">Bank Giro</asp:ListItem>
                  <asp:ListItem Value="Currency">Currency</asp:ListItem>
                  <asp:ListItem Value="DueDate">Due Date</asp:ListItem>
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
            <asp:Button class="btngo" runat="server" ID="BtnGo" Text="G" Visible="false" />
          <br />
      <div style="border:0px  solid; width:100%; height:100%; overflow:auto;">
      <asp:GridView ID="GridView1" runat="server" AllowPaging="True" 
           AutoGenerateColumns="false" CssClass="Grid"> 
              <HeaderStyle CssClass="GridHeader" Wrap="false"></HeaderStyle>
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
                  <asp:TemplateField>
                      <ItemTemplate>
                          <asp:DropDownList CssClass="DropDownList" ID="ddl" runat="server">
                              <asp:ListItem Selected="True" Text="View" />
                              <asp:ListItem Text="Edit" />
                              <asp:ListItem Text="Delete" />
                              <%--<asp:ListItem Text="Print" />--%>
                          </asp:DropDownList>
                          <asp:Button class="btngo" runat="server" ID="BtnGo" Text="G" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" CommandName="Go"/>
                      </ItemTemplate>                       
                  </asp:TemplateField>
                  <asp:BoundField DataField="GiroNo" SortExpression="GiroNo" HeaderText="Giro No" HeaderStyle-Width="150px"></asp:BoundField>
                  <asp:BoundField DataField="Status" SortExpression="Status" HeaderText="Status"></asp:BoundField>
                  <asp:BoundField DataField="UserCodeName" SortExpression="UserCodeName" HeaderText="User Name" HeaderStyle-Width="200px"></asp:BoundField>
                  <asp:BoundField DataField="FgReport" SortExpression="FgReport" HeaderText="Report"></asp:BoundField>
                  <asp:BoundField DataField="ReceiptNo" SortExpression="ReceiptNo" HeaderText="Receipt No"></asp:BoundField>
                  <asp:BoundField DataField="Receipt_Date" SortExpression="ReceiptDate" HeaderText="Receipt Date"></asp:BoundField>
                  <asp:BoundField DataField="BankGiro" SortExpression="BankGiro" HeaderText="Bank Giro"></asp:BoundField>
                  <asp:BoundField DataField="Due_Date" SortExpression="DueDate" HeaderText="Due Date"></asp:BoundField>                  
                  <asp:BoundField DataField="Currency" SortExpression="Currency" HeaderText="Currency"></asp:BoundField>
                  <asp:BoundField DataField="ForexRate" SortExpression="ForexRate" HeaderText="Rate"></asp:BoundField>                  
                  <asp:BoundField DataField="TotalForex" SortExpression="TotalForex" HeaderText="Total Forex"></asp:BoundField>
                  <asp:BoundField DataField="Remark" SortExpression="Remark" HeaderText="Remark" HeaderStyle-Width="250px"></asp:BoundField>
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
     <asp:Panel ID="pnlInput" runat="server" Visible="false">
        <table>
            <tr>
                <td>Giro No</td>
                <td>:</td>
                <td colspan = "4">
                    <asp:TextBox ID="tbGiroNo" ValidationGroup="Input" runat="server" CssClass ="TextBox" />
                    <%--&nbsp Report : &nbsp <asp:DropDownList AutoPostBack="true" ValidationGroup="Input"  CssClass="DropDownList" ID="ddlReport" runat="server" >
                        <asp:ListItem Selected="True">Y</asp:ListItem>
                        <asp:ListItem>N</asp:ListItem>
                    </asp:DropDownList>--%> 
                </td>
            </tr>
            <tr>
                <td>User</td>
                <td>:</td>
                <td colspan="4">
                    <asp:DropDownList ID="ddlUserType" AutoPostBack="true" ValidationGroup="Input" runat="server" CssClass="DropDownList">
                        <asp:ListItem Selected="True">Customer</asp:ListItem>
                        <asp:ListItem>Supplier</asp:ListItem>
                        <asp:ListItem>Common</asp:ListItem>                        
                    </asp:DropDownList> 
                    <asp:TextBox ID="tbUserCode" AutoPostBack="true" ValidationGroup="Input" Width="100" runat="server" CssClass="TextBox" />
                    <asp:TextBox ID="tbUserName" Width="175" ValidationGroup="Input" runat="server" 
                                ReadOnly="true" CssClass="TextBoxR" />
                    <asp:Button class="btngo" runat="server" ID="btnUser" Text="..." validationgroup = "Input"/>                              
                </td>                
             </tr>
             <tr>
                <td>Receipt No</td>
                <td>:</td>
                <td><asp:TextBox ID="tbReceiptNo" ValidationGroup="Input" runat="server" CssClass="TextBox" /></td>
                
                <td>Receipt Date</td>
                <td>:</td>
                <td><BDP:BasicDatePicker ID="tbReceiptDate" ValidationGroup="Input" runat="server" 
                       ShowNoneButton="false" DateFormat="dd MMM yyyy" ButtonImageHeight="19px" ButtonImageWidth="20px" 
                       DisplayType="TextBoxAndImage" ReadOnly="true" 
                        TextBoxStyle-CssClass="TextDate" AutoPostBack="True">
                    <TextBoxStyle CssClass="TextDate" />
                    </BDP:BasicDatePicker>
                </td>                 
             </tr>
             <tr>
                <td>Bank Giro</td>
                <td>:</td>
                <td><asp:DropDownList ID="ddlBankGiro" ValidationGroup="Input" runat="server" CssClass="DropDownList" Width="180px" />                                                                
                </td>                
                <td>Due Date</td>
                <td>:</td>
                <td><BDP:BasicDatePicker ID="tbDueDate" runat="server" DateFormat="dd MMM yyyy" ButtonImageHeight="19px" ButtonImageWidth="20px" 
                       ShowNoneButton="false" DisplayType="TextBoxAndImage" ReadOnly="true" ValidationGroup="Input" TextBoxStyle-CssClass="TextDate">
                    </BDP:BasicDatePicker>
                </td>   
             </tr>
             
             <tr>
                <td><asp:LinkButton ID="lbCurr" runat="server" Text="Currency"/></td>
                <td>:</td>
                <td><asp:DropDownList ID="ddlCurr" ValidationGroup="Input" AutoPostBack="true" runat="server" CssClass="DropDownList" Width="100px" />                                                                   
                    <asp:TextBox ID="tbRate" ValidationGroup="Input" onchange="calculate();" runat="server" CssClass="TextBox" Width="80px" />
                </td>
                <td>Amount Forex</td>
                <td>:</td>
                <td><asp:TextBox ID="tbTotalForex" ValidationGroup="Input" onchange="calculate();" runat="server" CssClass="TextBox" Width="100px" />
                </td>                                              
             </tr>
             <tr>
                <td>Remark</td>
                <td>:</td>
                <td colspan="4"><asp:TextBox ID="tbRemark" ValidationGroup="Input" Width="358px" runat="server" 
                        CssClass="TextBoxMulti" MaxLength="255" TextMode="MultiLine" /></td>
             </tr>  
             <tr>
             <td colspan="6">
                <br />
             </td>
             </tr>
             <tr>
                <td colspan="6">
                    <asp:Button class="bitbtn btnsavenew" runat="server" ID="btnSaveNew" 
                        Text="Save & New" validationgroup = "Input" CommandName="SearchEdit" 
                        Width="91px"/>
                    <asp:Button class="bitbtn btnsave" runat="server" ID="btnSave" Text="Save" validationgroup = "Input" CommandName="SearchEdit"/>
                    <asp:Button class="bitbtn btncancel" runat="server" ID="btnCancel" Text="Cancel" validationgroup = "Input" CommandName="SearchEdit"/>
                    <asp:Button class="btngo" runat="server" ID="btnHome" Text="Home" Width="37px"/>                   
                </td>
             </tr>
        </table>     
     </asp:Panel>          
    </div>   
    <asp:Label ID="lbStatus" runat="server" ForeColor="red" />     
    </form>
</body>
</html>
