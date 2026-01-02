<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TrPetty.aspx.vb" Inherits="Transaction_TrPetty_TrPetty" %>

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
 <link href="https://fonts.googleapis.com/css2?family=Muli:wght@300;400;600&display=swap" rel="stylesheet">
</head>
<body>
    <form id="form1" runat="server">
    <div class="Content">
    <div class="H1">Petty</div>
     <hr style="color:Blue" />        
    <asp:Panel runat="server" id="PnlHd"> 
      <table>
        <tr>
            <td style="width:100px;text-align:right">Quick Search :</td>         
            <td><asp:TextBox CssClass="TextBox" runat="server" ID ="tbFilter"/> 
                  <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlField" >     
                    <asp:ListItem Selected="True" Value="TransNmbr">Petty No</asp:ListItem>
                    <asp:ListItem Value="dbo.FormatDate(TransDate)">Petty Date</asp:ListItem>
                    <asp:ListItem>Status</asp:ListItem>
                    <asp:ListItem>Currency</asp:ListItem>
                    <asp:ListItem Value="ForexRate">Rate</asp:ListItem>
                    <asp:ListItem Value="TotalForex">Total Forex</asp:ListItem>
                    <asp:ListItem Value="PayTo">Pay To</asp:ListItem>
                    <asp:ListItem Value="FgReport">Report</asp:ListItem>
                    <asp:ListItem>Remark</asp:ListItem>
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
                    <asp:ListItem Selected="True" Value="TransNmbr">Petty No</asp:ListItem>
                    <asp:ListItem Value="dbo.FormatDate(TransDate)">Petty Date</asp:ListItem>
                    <asp:ListItem>Status</asp:ListItem>
                    <asp:ListItem>Currency</asp:ListItem>
                    <asp:ListItem Value="ForexRate">Rate</asp:ListItem>
                    <asp:ListItem Value="TotalForex">Total Forex</asp:ListItem>
                    <asp:ListItem Value="PayTo">Pay To</asp:ListItem>
                    <asp:ListItem Value="FgReport">Report</asp:ListItem>
                    <asp:ListItem>Remark</asp:ListItem>         
              </asp:DropDownList>
          </td>              
        </tr>        
      </table>      
      </asp:Panel>
          <br />
           <asp:Button class="bitbtn btnadd" runat="server" ID="BtnAdd" Text="Add"/>	 
            &nbsp;  
            <asp:DropDownList CssClass="DropDownList" ID="ddlCommand" runat="server" Visible="false"/>
            <asp:Button class="btngo" runat="server" ID="BtnGo" Text="G"/>
          <br />&nbsp;
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
                  <asp:BoundField DataField="TransNmbr" HeaderStyle-Width="120px" SortExpression="Nmbr" HeaderText="Petty No"></asp:BoundField>                  
                  <asp:BoundField DataField="Status" SortExpression="Status" HeaderText="Status"></asp:BoundField>                  
                  <asp:BoundField DataField="TransDate" dataformatstring="{0:dd MMM yyyy}" htmlencode="true" HeaderStyle-Width="80px" SortExpression="TransDate" HeaderText="Petty Date"></asp:BoundField>
                  <asp:BoundField DataField="PettyType" HeaderStyle-Width="80px" SortExpression="PettyType" HeaderText="Petty Type"></asp:BoundField>
                  <asp:BoundField DataField="PettyName" HeaderStyle-Width="150px" SortExpression="PettyName" HeaderText="Petty Name"></asp:BoundField>
                  <asp:BoundField DataField="Currency" HeaderStyle-Width="80px" SortExpression="Currency" HeaderText="Currency"></asp:BoundField>
                  <asp:BoundField DataField="ForexRate" DataFormatString="{0:#,##0.###}" HeaderStyle-Width="80px" SortExpression="ForexRate" HeaderText="Rate"></asp:BoundField>
                  <asp:BoundField DataField="TotalForex" DataFormatString="{0:#,##0.00}" HeaderStyle-Width="80px" HeaderText="TotalForex" SortExpression="Total Forex"></asp:BoundField>
                  <asp:BoundField DataField="PayTo" HeaderStyle-Width="200px" SortExpression="PayTo" HeaderText="Pay To"></asp:BoundField>
                  <asp:BoundField DataField="FgReport" HeaderStyle-Width="200px" SortExpression="FgReport" HeaderText="Report"></asp:BoundField>
                  <asp:BoundField DataField="Remark" HeaderStyle-Width="250px" SortExpression="Remark" HeaderText="Remark"></asp:BoundField>
              </Columns>
          </asp:GridView>
          </div>
          <br />
            <asp:Panel runat="server" ID ="pnlNav" Visible="false">
            <asp:Button class="bitbtn btnadd" runat="server" ID="btnAdd2" Text="Add"/>	
            &nbsp;
            <asp:DropDownList ID="ddlCommand2" CssClass="DropDownList" runat="server"/>
            <asp:Button class="btngo" runat="server" ID="btnGo2" Text="G"/>                
            </asp:Panel>
    </asp:Panel>    
    <asp:Panel runat="server" ID="pnlInput" Visible="false">
      <table>
        <tr>
            <td>Petty No</td>
            <td>:</td>
            <td><asp:TextBox CssClass="TextBoxR" runat="server" ID="tbCode" Width="150px" Enabled="False"/></td>
            
            <td>Petty Date</td>
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
            <td>Petty</td>
            <td>:</td>
            <td>
                <asp:DropDownList runat="server" ValidationGroup="Input" ID="ddlPetty" Width="250px" 
                    CssClass="DropDownList" AutoPostBack="True"/>
            </td>
            <%--<td>Report</td>
            <td>:</td>
            <td>
                <asp:DropDownList ID="ddlFgReport" runat="server" CssClass="DropDownList" Enabled="False" Height="16px" ValidationGroup="Input" Width="34px">
                    <asp:ListItem>Y</asp:ListItem>
                    <asp:ListItem>N</asp:ListItem>
                </asp:DropDownList>
            </td>--%>
        </tr>
        <tr>
            <td>Currency</td>
            <td>:</td>
            <td>
                <asp:DropDownList CssClass="DropDownList" ID="ddlCurr" ValidationGroup="Input" 
                    runat="server" AutoPostBack="true" Width="60px" Enabled="False"/>
                <asp:TextBox CssClass="TextBox" runat="server" ID="tbRate" ValidationGroup="Input" Width="65px"/>
            </td>
            <td>Total Forex</td>
            <td>:</td>
            <td>
                <asp:TextBox CssClass="TextBoxR" Enabled ="false" runat="server" ID="tbTotalForex"/>
            </td>
            
            
        </tr>
        <tr>        
            <td>Pay To</td>
            <td>:</td>
            <td colspan="4">
                <asp:TextBox ID="tbPayTo" runat="server" CssClass="TextBox" 
                    ValidationGroup="Input" Width="180px" />
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
                        ShowFooter="False">
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
                            <asp:BoundField DataField="ItemNo" HeaderText="No" />
                            <asp:BoundField DataField="Account" HeaderText="Account" />
                            <asp:BoundField DataField="AccountName" HeaderStyle-Width="150px" HeaderText="Account Name" />
                            <asp:BoundField DataField="Subled" HeaderStyle-Width="80px" HeaderText="Subled" />
                            <asp:BoundField DataField="SubledName" HeaderStyle-Width="150px" HeaderText="Subled Name" />
                            <asp:BoundField DataField="CostCtrName" HeaderStyle-Width="150px" HeaderText="Cost Center" />
                            <asp:BoundField DataField="AmountForex" HeaderStyle-Width="80px" HeaderText="Amount Forex" />
                            <asp:BoundField DataField="Remark" HeaderStyle-Width="200px" HeaderText="Remark" />
                        </Columns>
                    </asp:GridView>
              </div>   
              <asp:Button class="bitbtndt btnadd" runat="server" ID="btnAddDtke2" Text="Add" ValidationGroup="Input" />
              </asp:Panel> 
              <asp:Panel runat="server" ID="pnlEditDt" Visible="false">
                <table>             
                    <tr>
                        <td>No</td>
                        <td>:</td>
                        <td colspan="4">
                            <asp:Label ID="lbItemNo" runat="server" Text ="itemmmm noooooooo" />
                        </td>
                    </tr>
                    <tr>                    
                        <td>Account</td>
                        <td>:</td>
                        <td colspan="4">                                
                            <asp:TextBox CssClass="TextBox" ValidationGroup="Input" runat="server" ID="tbAccount" AutoPostBack="true" />
                            <asp:TextBox CssClass="TextBox" runat="server" ID="tbAccountName" Enabled="false" Width="225px"/>
                            <asp:Button class="btngo" runat="server" ID="btnAccount" Text="..." ValidationGroup="Input"/>            
                        </td>
                    </tr>
                    <tr>                    
                        <td>Subled</td>
                        <td>:</td>
                        <td colspan="4"> 
                            <asp:TextBox runat="server" ID="tbFgSubled" Visible="false" /> 
                            <asp:TextBox runat="server" ID="tbFgCostCtr" Visible="false" />                              
                            <asp:TextBox CssClass="TextBox" ValidationGroup="Input" runat="server" ID="tbSubled" AutoPostBack="true" />
                            <asp:TextBox CssClass="TextBox" runat="server" ID="tbSubledName" Enabled="false" Width="225px"/>
                            <asp:Button class="btngo" runat="server" ID="btnSubled" Text="..." ValidationGroup="Input"/>                   
                        </td>
                    </tr>                    
                    <tr>
                    <%--CostCtr, CostCtrName, AmountForex, Remark--%>
                        <td>Cost Center</td>
                        <td>:</td>
                        <td colspan="4">                                
                            <asp:DropDownList ValidationGroup="Input" CssClass="DropDownList" runat="server" ID="ddlCostCenter" Width="180px"/>
                        </td>
                   </tr>
                   <tr>     
                        <td>Amount Forex</td>
                        <td>:</td>
                        <td colspan="4"><asp:TextBox ValidationGroup="Input" CssClass="TextBox" runat="server" Width="100px" ID="tbAmountForex"/>
                          <asp:Label runat="server" ID="lbcurrDt"></asp:Label>
                        </td>
                    </tr>                       
                    <tr>
                        <td>Remark </td>
                        <td>:</td>
                        <td colspan="4"><asp:TextBox runat="server" ID="tbRemarkDt" CssClass="TextBoxMulti" Width="365px" 
                                MaxLength="255" TextMode="MultiLine" />                        
                        </td>
                    </tr>
                </table>
                <br />           
                <asp:Button ID="btnSaveDt" runat="server" class="bitbtndt btnsave" Text="Save"/> &nbsp;         
                <asp:Button ID="btnCancelDt" runat="server" class="bitbtndt btncancel" Text="Cancel"/> 
           </asp:Panel> 
       <br/>    
         
        <asp:Button ID="btnSaveAll" runat="server" class="bitbtndt btnsave" 
            Text="Save & New" ValidationGroup="Input" Width="97px"/> &nbsp;    
        <asp:Button ID="btnSaveTrans" runat="server" class="bitbtndt btnsave" Text="Save" ValidationGroup="Input"/> &nbsp;    
        <asp:Button ID="btnBack" runat="server" class="bitbtndt btncancel" Text="Cancel" ValidationGroup="Input"/>  &nbsp;
        <asp:Button ID="btnHome" runat="server" class="bitbtndt btnback" Text="Home" />    
    </asp:Panel>        
    </div>
   
    <asp:Label runat ="server" ID="lbStatus" ForeColor="Red"/>
    </form>
</body>
</html>
