<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TrGLBudget.aspx.vb" Inherits="Transaction_TrGLBudget_TrGLBudget" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Untitled Page</title>
    <script src="../../Function/OpenDlg.js" type="text/javascript"></script>
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
                                                      
                     
            document.getElementById("tbAmountPrev").value = setdigit(document.getElementById("tbAmountPrev").value.replace(/\$|\,/g,""),'<%=VIEWSTATE("DigitQty")%>');
            document.getElementById("tbActualPrev").value = setdigit(document.getElementById("tbActualPrev").value.replace(/\$|\,/g,""),'<%=VIEWSTATE("DigitQty")%>');
            document.getElementById("tbAmount").value = setdigit(document.getElementById("tbAmount").value.replace(/\$|\,/g,""),'<%=VIEWSTATE("DigitQty")%>');
            
            
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
        <div class="H1">Budget</div>
        <hr style="color:Blue" />
        <asp:Panel runat="server" id="PnlHd"> 
      <table>
        <tr>
            <td style="width:100px;text-align:right">Quick Search :</td>            
            <td><asp:TextBox CssClass="TextBox" runat="server" ID ="tbFilter"/> 
                  <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlField" >
                      <asp:ListItem Selected="True">Year</asp:ListItem>
                      <asp:ListItem>Period</asp:ListItem>
                      <asp:ListItem>Revisi</asp:ListItem>
                      <asp:ListItem>Status</asp:ListItem>
                      <asp:ListItem>Remark</asp:ListItem>                      
                    </asp:DropDownList>
                  <asp:Button class="bitbtn btnsearch" runat="server" ID="btnSearch" Text="Search" />                  
                  <asp:Button class="btngo" runat="server" ID="btnExpand" Text="..."/>  
            </td>
            <td>
                &nbsp;
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
                  <asp:ListItem Selected="True">Year</asp:ListItem>
                  <asp:ListItem>Period</asp:ListItem>
                  <asp:ListItem>Revisi</asp:ListItem>                  
                  <asp:ListItem>Status</asp:ListItem>
                  <asp:ListItem>Remark</asp:ListItem>
              </asp:DropDownList>
          </td>              
        </tr>        
      </table>      
      </asp:Panel>
          <br />
            <asp:Button class="bitbtn btnadd" runat="server" ID="BtnAdd" Text="Add" />
            
            &nbsp &nbsp &nbsp   
            <asp:DropDownList CssClass="DropDownList" ID="ddlCommand" runat="server" Visible="false"/>
            <asp:Button class="btngo" Visible="false" runat="server" ID="BtnGo" Text="G"/>
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
                              <asp:ListItem Text="Edit"/>
                              <asp:ListItem Text="Revisi"/>
                              <asp:ListItem Text="Print"/>
                          </asp:DropDownList>
                          <asp:Button class="btngo" runat="server" ID="BtnGo" Text="G" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" CommandName="Go" />
                      </ItemTemplate>
                      <HeaderStyle Width="110px" />
                  </asp:TemplateField>
                  <asp:BoundField DataField="Year" SortExpression="Year" HeaderText="Year"></asp:BoundField>
                  <asp:BoundField DataField="Period" SortExpression="Period" HeaderText="Period"></asp:BoundField>
                  <asp:BoundField DataField="Status" HeaderText="Status"></asp:BoundField>                  
                  <asp:BoundField DataField="Revisi" HeaderText="Revisi"></asp:BoundField>
                  <asp:BoundField DataField="Remark" HeaderStyle-Width="300px" SortExpression="Remark" HeaderText="Remark"></asp:BoundField>
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
            <td>Year</td>
            <td>:</td>
            <td><asp:DropDownList CssClass="DropDownList" runat="server" ValidationGroup="Input" ID="ddlYear" /></td>           
            
            <td>Period</td>
            <td>:</td>
            <td><asp:DropDownList CssClass="DropDownList" runat="server" ValidationGroup="Input" ID="ddlPeriod" /></td>           
            
            <td>Revisi</td>
            <td>:</td>
            <td><asp:Label runat="server" ID="lbRevisi"></asp:Label></td>
        </tr>              
        
        <tr>
            <td>Remark</td>
            <td>:</td>
            <td colspan="4"><asp:TextBox Width = "365px" runat="server" ValidationGroup="Input" 
                    ID="tbRemark" MaxLength="255" CssClass="TextBoxMulti" TextMode="MultiLine" />                    
                    <asp:Button class="bitbtn btngetitem" runat="server" ID="btnGetData" Text="Get Item" />
                    
            </td>
        </tr>
      </table>  
      
      <br />      
      <div style="font-size:medium; color:Blue;">Detail</div>
      <hr style="color:Blue" />  
        <asp:Panel runat="server" ID="pnlDt">
            <asp:Button class="bitbtndt btnadd" runat="server" ID="btnAdddt" Text="Add" ValidationGroup="Input" />                 
            <div style="border:0px  solid; overflow:auto;">
                <asp:GridView ID="GridDt" runat="server" AutoGenerateColumns="false" 
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
                        </asp:TemplateField>
                                               
                        <asp:BoundField DataField="Account" HeaderStyle-Width="120px" HeaderText="Account" />
                        <asp:BoundField DataField="Description" HeaderText="Description" HeaderStyle-Width="200px" />
                        <asp:BoundField DataField="CostCtrName" HeaderStyle-Width="200px" HeaderText="Cost Ctr Name" />
                        <asp:BoundField DataField="Currency" HeaderStyle-Width="80px" HeaderText="Currency" />
                        <asp:BoundField DataField="AmountPrev" HeaderStyle-Width="100px" HeaderText="Previous Amount" />
                        <asp:BoundField DataField="ActualPrev" HeaderStyle-Width="100px" HeaderText="Previous Actual" />
                        <asp:BoundField DataField="Amount" HeaderStyle-Width="100px" HeaderText="Amount" ItemStyle-HorizontalAlign="Right"/>
                        
                    </Columns>
                </asp:GridView>
          </div>   
          <asp:Button class="bitbtndt btnadd" runat="server" ID="btnAddDt2" Text="Add" ValidationGroup="Input" />                  
       </asp:Panel>             
       <asp:Panel runat="server" DefaultButton="btnSaveDt" ID="pnlEditDt" Visible="false">
            <table>              
                <tr>
                    <td>Account</td>
                    <td>:</td>
                    <td><asp:TextBox runat="server" ID="tbAccount" CssClass="TextBox" AutoPostBack="true" />
                        <asp:TextBox runat="server"  CssClass="TextBox"
                            ID="tbDescription" EnableTheming="True" ReadOnly="True" Enabled="False" 
                            Width="200px"/>
                        <asp:Button ID="btnAccount" runat="server" class="btngo" Text="..."/>
                        
                    </td>
                </tr>     
                <tr>
                    <td>Cost Ctr</td>
                    <td>:</td>
                    <td><asp:DropDownList CssClass="DropDownList" Enabled="false"  runat="server" ValidationGroup="Input" ID="ddlCostCtr"/></td>                    
                </tr>                                    
                <tr>
                    <td>Currency</td>
                    <td>:</td>
                    <td><asp:DropDownList CssClass="DropDownList" Enabled="false" runat="server" ValidationGroup="Input" ID="ddlCurrency"/></td>                    
                </tr>                
                <tr>
                    <td>Budget</td>
                    <td>:</td>
                    <td>
                        <table>
                            <tr style="background-color:Silver;text-align:center">
                                <td>Previous Budget</td>
                                <td>Previous Amount</td>
                                <td>Current Amount</td>
                                                             
                            </tr>
                            <tr>
                                <td><asp:TextBox CssClass="TextBox" Enabled=False runat="server" ID="tbAmountPrev"/></td>
                                <td><asp:TextBox CssClass="TextBox" Enabled=False runat="server" ID="tbActualPrev"/></td>
                                <td><asp:TextBox CssClass="TextBox" runat="server" ID="tbAmount"/></td>                                
                            </tr>
                        </table>                    
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
        <asp:Button ID="btnHome" runat="server" class="bitbtndt btncancel" Text="Cancel"/> 
    </asp:Panel>
    
    </div>     
    <asp:Label runat ="server" ID="lbStatus" ForeColor="Red"/>
    </form>
</body>
</html>
