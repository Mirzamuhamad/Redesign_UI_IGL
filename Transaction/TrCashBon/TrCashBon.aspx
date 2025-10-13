<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TrCashBon.aspx.vb" Inherits="Transaction_TrCashBon_TrCashBon" %>

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
       function setformatdt()
        {
        try
         {         
         var Amount = document.getElementById("tbAmountForex").value.replace(/\$|\,/g,"");                           
         var TotalForex = document.getElementById("tbTotalForex").value.replace(/\$|\,/g,"");                           
         document.getElementById("tbAmountForex").value = setdigit(Amount,'<%=Viewstate("DigitCurr")%>');
         document.getElementById("tbTotalForex").value = setdigit(TotalForex,'<%=Viewstate("DigitCurr")%>');
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
    <div class="H1">Cash Advance Realization</div>
     <hr style="color:Blue" />        
    <asp:Panel runat="server" id="PnlHd"> 
      <table>
        <tr>
            <td style="width:100px;text-align:right">Quick Search :</td>         
            <td><asp:TextBox CssClass="TextBox" runat="server" ID ="tbFilter"/> 
                  <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlField" >     
                    <asp:ListItem Selected="True" Value="TransNmbr">Cash Bon No</asp:ListItem>
                    <asp:ListItem Value="dbo.FormatDate(TransDate)">Cash Bon Date</asp:ListItem>
                    <asp:ListItem>Status</asp:ListItem>
                    <asp:ListItem>Currency</asp:ListItem>
                    <asp:ListItem Value="PettyNo">Petty No</asp:ListItem>
                    <asp:ListItem>Remark</asp:ListItem>  
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
                    <asp:ListItem Selected="True" Value="TransNmbr">Cash Bon No</asp:ListItem>
                    <asp:ListItem Value="dbo.FormatDate(TransDate)">Cash Bon Date</asp:ListItem>
                    <asp:ListItem>Status</asp:ListItem>
                    <asp:ListItem>Currency</asp:ListItem>
                    <asp:ListItem Value="PettyNo">Petty No</asp:ListItem>
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
                          <asp:Button class="btngo" runat="server" ID="BtnGoDt" Text="G" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" CommandName="Go"  />                         </ItemTemplate>
                      <HeaderStyle Width="110px" />
                  </asp:TemplateField>            
                  <asp:BoundField DataField="TransNmbr" HeaderStyle-Width="120px" SortExpression="Nmbr" HeaderText="Cash Bon No"></asp:BoundField>                  
                  <asp:BoundField DataField="Status" SortExpression="Status" HeaderText="Status"></asp:BoundField>                  
                  <asp:BoundField DataField="TransDate" dataformatstring="{0:dd MMM yyyy}" htmlencode="true" HeaderStyle-Width="80px" SortExpression="TransDate" HeaderText="Cash Bon Date"></asp:BoundField>
                  <asp:BoundField DataField="PettyNo" HeaderStyle-Width="80px" SortExpression="PettyNo" HeaderText="Petty No"></asp:BoundField>
                  <asp:BoundField DataField="Currency" HeaderStyle-Width="80px" SortExpression="Currency" HeaderText="Currency"></asp:BoundField>
                  <asp:BoundField DataField="ForexRate" DataFormatString="{0:#,##0.####}" HeaderStyle-Width="80px" SortExpression="ForexRate" HeaderText="Forex Rate"></asp:BoundField>
                  <asp:BoundField DataField="TotalForex" DataFormatString="{0:#,##0.00}" HeaderStyle-Width="80px" HeaderText="TotalForex" SortExpression="Total Forex"></asp:BoundField>
                  <asp:BoundField DataField="Remark" HeaderStyle-Width="250px" SortExpression="Remark" HeaderText="Remark"></asp:BoundField>
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
            <td>Cash Bon No</td>
            <td>:</td>
            <td><asp:TextBox CssClass="TextBoxR" runat="server" ID="tbCode" Enabled="False"/>
            &nbsp
            Cash Bon Date :
            <BDP:BasicDatePicker ID="tbDate" runat="server" DateFormat="dd MMM yyyy" 
                        ReadOnly = "true" ValidationGroup="Input"
                        ButtonImageHeight="19px" ButtonImageWidth="20px" 
                        DisplayType="TextBoxAndImage" 
                        TextBoxStyle-CssClass="TextDate" 
                        ShowNoneButton="False"><TextBoxStyle CssClass="TextDate" /></BDP:BasicDatePicker>                
            </td>            
        </tr>   
        <tr>
            <td>Petty No</td>
            <td>:</td>
            <td><asp:TextBox CssClass="TextBox" runat="server" ID="tbPettyNo" Enabled="False"/>
                <asp:Button Class="btngo" ID="btnPettyNo" Text="..." runat="server" ValidationGroup="Input" />                                                          
                &nbsp                
                <asp:TextBox CssClass="TextBox" runat="server" ID="tbFgSubledHd" Width="20px" Visible="False"/>
            </td>           
        </tr>
        <tr>
            <td>Account</td>
            <td>:</td>
            <td><asp:TextBox CssClass="TextBox" runat="server" ID="tbAccCodeHd" Enabled="False"/>
                <asp:TextBox CssClass="TextBox" runat="server" ID="tbAccNameHd" Width="200px" Enabled="False"/>
            </td>
        </tr>
        <tr>
            <td>Subled</td>
            <td>:</td>
            <td><asp:TextBox CssClass="TextBox" runat="server" ID="tbSubledCodeHd" Enabled="False"/>
                <asp:TextBox CssClass="TextBox" runat="server" ID="tbSubledNameHd" Width="200px" Enabled="False"/>
            </td>
        </tr>
        <tr>
            <td>Currency</td>
            <td>:</td>
            <td>
                <asp:DropDownList CssClass="DropDownList" ID="ddlCurr" runat="server" AutoPostBack="true" Width="60px" Enabled="false"/>
                <asp:TextBox CssClass="TextBox" runat="server" ID="tbRate" Width="65px" 
                    ValidationGroup="Input"/>
            </td>
        </tr>
        <tr>
            <td>Total Forex</td>
            <td>:</td>
            <td>
                <asp:TextBox CssClass="TextBoxR" Enabled ="false" runat="server" ID="tbTotalForex"/>
            </td>                        
        </tr>
        <tr>        
            <td>Remark</td>
            <td>:</td>
            <td><asp:TextBox runat="server" ValidationGroup="Input" ID="tbRemark" CssClass="TextBoxMulti" TextMode="MultiLine" Width="355px"/></td>
        </tr>
      </table>  
      
      <br />      
      <div style="font-size:medium; color:Blue;">Detail</div>
      <hr style="color:Blue" />  
              <asp:Panel runat="server" ID="PnlDt">
                <asp:Button class="bitbtn btnadd" runat="server" ID="btnAdddt" Text="Add" ValidationGroup="Input" />	     
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
                                      
                                        <asp:Button class="bitbtn btnedit" runat="server" ID="btnEdit" Text="Edit" CommandName="Edit" />																						 											
									
									    <asp:Button class="bitbtn btndelete" runat="server" ID="btnDelete" Text="Delete" CommandName="Delete"  OnClientClick="return confirm('Sure to delete this data?');" />																						 																		
                                                                               
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
              <asp:Button class="bitbtn btnadd" runat="server" ID="btnAddDtke2" Text="Add"  ValidationGroup="Input"/>	     
              
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
                            <asp:Button Class="btngo" ID="btnAccount" Text="..." runat="server" ValidationGroup="Input" />                                                          
                            
                            
                        </td>
                    </tr>
                    <tr>                    
                        <td>Subled</td>
                        <td>:</td>
                        <td colspan="4"> 
                            <asp:TextBox runat="server" ID="tbFgSubled" Visible="false" />                               
                            <asp:TextBox CssClass="TextBox" ValidationGroup="Input" runat="server" ID="tbSubled" AutoPostBack="true" />
                            <asp:TextBox CssClass="TextBox" runat="server" ID="tbSubledName" Enabled="false" Width="225px"/>
                            <asp:Button Class="btngo" ID="btnSubled" Text="..." runat="server" ValidationGroup="Input" />                                                          
                            
                        </td>
                    </tr>                    
                    <tr>
                    <%--CostCtr, CostCtrName, AmountForex, Remark--%>
                        <td>Cost Center</td>
                        <td>:</td>
                        <td><asp:TextBox runat="server" ID="tbFgCostCtr" Visible="false"/>                                                              
                            <asp:DropDownList ValidationGroup="Input" CssClass="DropDownList" runat="server" ID="ddlCostCenter" />
                        </td>
                        
                        <td>Amount Forex</td>
                        <td>:</td>
                        <td><asp:TextBox ValidationGroup="Input" CssClass="TextBox" runat="server" ID="tbAmountForex"/></td>
                    </tr>                       
                    <tr>
                        <td>Remark </td>
                        <td>:</td>
                        <td colspan="4"><asp:TextBox runat="server" ID="tbRemarkDt" CssClass="TextBox" Width="365px" 
                                MaxLength="255" TextMode="MultiLine" />                        
                        </td>
                    </tr>
                </table>
                <br />               
        		<asp:Button class="bitbtn btnsave" runat="server" ID="btnSaveDt" Text="Save" CommandName="Update" />																						 																		
		    	<asp:Button class="bitbtn btncancel" runat="server" ID="btnCancelDt" Text="Cancel" CommandName="Cancel" />																						 																		
                      
           </asp:Panel> 
       <br />          
       
    	<asp:Button ID="btnSaveAll" runat="server" class="bitbtndt btnsavenew" Text="Save All" validationgroup="Input" Width="90px"/>									
        <asp:Button ID="btnSaveTrans" runat="server" class="bitbtndt btnsave" Text="Save" validationgroup="Input"/> 									
        <asp:Button ID="btnBack" runat="server" class="bitbtndt btncancel" Text="Cancel" validationgroup="Input"/> 								
        <asp:Button ID="btnHome" runat="server" class="bitbtndt btnback" Text="Home" />										                                                           

       
    </asp:Panel>        
    </div>
   
    <asp:Label runat ="server" ID="lbStatus" ForeColor="Red"/>
    </form>
</body>
</html>
