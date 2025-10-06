<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TrARRate.Aspx.vb" Inherits="TrARRate" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<%@ Register Assembly="BasicFrame.WebControls.BasicDatePicker" Namespace="BasicFrame.WebControls"
    TagPrefix="BDP" %>

<%@ Register assembly="CrystalDecisions.Web, Version=10.5.3700.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>

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
    
        function setformat()
        {
        try
         {                 
        var Rate = document.getElementById("tbRate").value.replace(/\$|\,/g,"");                        
        document.getElementById("tbRate").value = setdigit(Rate,'<%=ViewState("DigitRate")%>');        
        }catch (err){
            alert(err.description);
          }      
        }   
        
        function setformatdt()
        {
        try
         {         
        var RateF = document.getElementById("tbForexRate").value.replace(/\$|\,/g,""); 
        var AmountF = document.getElementById("tbAmountForex").value.replace(/\$|\,/g,"");
        var AmountH = document.getElementById("tbAmountHome").value.replace(/\$|\,/g,""); 
        var NewRate = document.getElementById("tbRate").value.replace(/\$|\,/g,""); 
        var NewAmountH = document.getElementById("tbNewAmountHome").value.replace(/\$|\,/g,""); 
        var Adjust = document.getElementById("tbAdjust").value.replace(/\$|\,/g,""); 
        var FgValue = document.getElementById("tbFgValue").value.replace(/\$|\,/g,""); 
                                     
                
        document.getElementById("tbForexRate").value = setdigit(RateF,'<%=ViewState("DigitRate")%>');
        document.getElementById("tbAmountForex").value = setdigit(AmountF,'<%=VIEWSTATE("DigitCurr")%>');
        document.getElementById("tbAmountHome").value = setdigit(AmountH,'<%=VIEWSTATE("DigitCurr")%>');
        document.getElementById("tbNewAmountHome").value = setdigit(NewAmountH,'<%=VIEWSTATE("DigitCurr")%>');
        document.getElementById("tbAdjust").value = setdigit(Adjust,'<%=VIEWSTATE("DigitCurr")%>');
                
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
    <div class="H1">Difference Rate A/R</div>
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
                      <asp:ListItem Value="CostCtr_Name">Cost Center</asp:ListItem>
                      <asp:ListItem Value="Currency">Currency</asp:ListItem>
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
                  <asp:ListItem Value="TransNmbr" Selected="True">Reference</asp:ListItem>
                      <asp:ListItem Value="dbo.FormatDate(TransDate)">Date</asp:ListItem>                      
                      <asp:ListItem>Status</asp:ListItem>
                      <asp:ListItem Value="CostCtr_Name">Cost Center</asp:ListItem>
                      <asp:ListItem Value="Currency">Currency</asp:ListItem>
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
            <asp:Button class="btngo" runat="server" ID="BtnGo" Text="G"/>
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
                          <asp:ImageButton ID="BtnGo" runat="server"  
                            ImageUrl="../../Image/btnGoDtOn.png"
                            onmouseover="this.src='../../Image/btnGoDtOff.png';"
                            onmouseout="this.src='../../Image/btnGoDtOn.png';"
                            ImageAlign="AbsBottom" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" CommandName="Go" />
                      </ItemTemplate>
                      <HeaderStyle Width="110px" />
                  </asp:TemplateField>
                  <asp:BoundField DataField="TransNmbr" HeaderStyle-Width="120px" SortExpression="Nmbr" HeaderText="Reference"></asp:BoundField>                  
                  <asp:BoundField DataField="Status" HeaderText="Status"></asp:BoundField>                  
                  <asp:BoundField DataField="TransDate" dataformatstring="{0:dd MMM yyyy}" htmlencode="true" HeaderStyle-Width="80px" SortExpression="TransDate" HeaderText="Date"></asp:BoundField>                  
                  <asp:BoundField DataField="Currency" HeaderText="Curr"></asp:BoundField>
                  <asp:BoundField DataField="NewRate" DataFormatString="{0:#,##0.####}" HeaderStyle-Width="80px" HeaderText="New Rate"></asp:BoundField>                  
                  <asp:BoundField DataField="Cost_Ctr_Name" HeaderStyle-Width="180px" SortExpression="Cost_Ctr_Name" HeaderText="Cost Center"></asp:BoundField>                  
                  <asp:BoundField DataField="Remark" HeaderStyle-Width="200px" HeaderText="Remark"></asp:BoundField>                  
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
            <td>Reference</td>
            <td>:</td>
            <td><asp:TextBox CssClass="TextBoxR" runat="server" ID="tbReference" Width="150px" Enabled="False"/> </td>            
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
                        ShowNoneButton="False">
                        <TextBoxStyle CssClass="TextDate" />
             </BDP:BasicDatePicker>                
            </td>            
        </tr>      
        <tr>
                <td>Report</td>
                <td>:</td>
                <td><asp:DropDownList AutoPostBack="true" ValidationGroup="Input" CssClass="DropDownList" ID="ddlReport" runat="server" >
                        <asp:ListItem Selected="True">Y</asp:ListItem>
                        <asp:ListItem>N</asp:ListItem>
                    </asp:DropDownList> 
                </td>    
        </tr>                                 
        <tr>
            <td>Currency</td>
            <td>:</td>
            <td><asp:DropDownList ID="ddlCurr" ValidationGroup="Input" AutoPostBack="true" runat="server" CssClass="DropDownList" />                                                                                   
            </td>
        </tr>  
        <tr>
            <td>New Rate</td>
            <td>:</td>
            <td><asp:TextBox ID="tbRate" ValidationGroup="Input" runat="server" CssClass="TextBox" Width="60px" />
              <asp:Button class="bitbtn btngetitem" runat="server" ID="btnGetDt" Text="Get Item" />
            </td>
         </tr>
        <tr>
            <td>Cost Ctr</td>
            <td>:</td>
            <td><asp:DropDownList CssClass="DropDownList" ValidationGroup="Input" runat="server" ID="ddlCostCtr" AutoPostBack="true" Width="200px" />
            </td>                    
        </tr>
        <tr>
            <td>Remark</td>
            <td>:</td>
            <td><asp:TextBox runat="server" ID="tbRemark" CssClass="TextBox" Width="225px"/></td>
        </tr>
      </table>  
      
      <br />      
      <div style="font-size:medium; color:Blue;">Detail</div>
      <hr style="color:Blue" />  
        <asp:Panel runat="server" ID="pnlDt">
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
                                <asp:ImageButton ID="btnEdit" runat="server"  
                                    ImageUrl="../../Image/btnEditDtOn.png"
                                    onmouseover="this.src='../../Image/btnEditDtOff.png';"
                                    onmouseout="this.src='../../Image/btnEditDtOn.png';"
                                    ImageAlign="AbsBottom" CommandName="Edit" />   
                                <asp:ImageButton ID="btnDelete" runat="server"  
                                    ImageUrl="../../Image/btnDeleteDtOn.png"
                                    onmouseover="this.src='../../Image/btnDeleteDtOff.png';"
                                    onmouseout="this.src='../../Image/btnDeleteDtOn.png';"
                                    ImageAlign="AbsBottom" CommandName="Delete" />     
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:ImageButton ID="btnUpdate" runat="server"  
                                    ImageUrl="../../Image/btnUpdateDtOn.png"
                                    onmouseover="this.src='../../Image/btnUpdateDtOff.png';"
                                    onmouseout="this.src='../../Image/btnUpdateDtOn.png';"
                                    ImageAlign="AbsBottom" CommandName="Update" />   
                                <asp:ImageButton ID="btnCancel" runat="server"  
                                    ImageUrl="../../Image/btnCancelDtOn.png"
                                    onmouseover="this.src='../../Image/btnCancelDtOff.png';"
                                    onmouseout="this.src='../../Image/btnCancelDtOn.png';"
                                    ImageAlign="AbsBottom" CommandName="Cancel" />    
                            </EditItemTemplate>
                            
                        </asp:TemplateField>
                        <asp:BoundField DataField="InvoiceNo" HeaderStyle-Width="120px" HeaderText="Invoice No" />
                        <asp:BoundField DataField="InvoiceDate" HeaderStyle-Width="100px"  HeaderText="Invoice Date" />                          
                        <asp:BoundField DataField="Customer_Name" HeaderStyle-Width="150px"  HeaderText="Customer" />
                        <asp:BoundField DataField="ForexRate" HeaderStyle-Width="80px" HeaderText="Rate" />
                        <asp:BoundField DataField="AmountForex" HeaderStyle-Width="80px" HeaderText="Amount Forex" />                                                                        
                        <asp:BoundField DataField="AmountHome" HeaderStyle-Width="80px" HeaderText="Amount Home" />  
                        <asp:BoundField DataField="NewAmountHome" HeaderStyle-Width="100px" HeaderText="New Amount Home" />  
                        <asp:BoundField DataField="AdjustHome" HeaderStyle-Width="100px" HeaderText="AdjustHome" />                          
                     </Columns>
                </asp:GridView>
          </div>   
          <asp:Button class="bitbtndt btnadd" runat="server" ID="btnAddDt2" Text="Add" ValidationGroup="Input" />  
       </asp:Panel>             
       <asp:Panel runat="server" ID="pnlEditDt" Visible="false">
            <table>                
                <tr>
                    <td>Invoice No</td>
                    <td>:</td>
                    <td><asp:TextBox runat="server" ID="tbInvNo" Enabled="false" CssClass="TextBox" AutoPostBack="true" />
                    <asp:Button ID="btnInvNo" runat="server" class="btngo" Text="..."/>
                 </tr>
                 <tr>
                    <td>Invoice Date</td>
                    <td>:</td>
                    <td><BDP:BasicDatePicker ID="tbInvDate" runat="server" DateFormat="dd MMM yyyy" 
                            ReadOnly = "true" ValidationGroup="Input" Enabled = "False"
                            ButtonImageHeight="19px" ButtonImageWidth="20px" 
                            DisplayType="TextBox" 
                            TextBoxStyle-CssClass="TextDate" AutoPostBack="True" 
                            ShowNoneButton="False">
                            <%--<TextBoxStyle CssClass="TextDate" />--%>
                        </BDP:BasicDatePicker> 
                    </td>                               
                </tr>                                                                
                <tr>
                    <td>Customer</td>
                    <td>:</td>
                    <td><asp:TextBox runat="server" ID="tbCustCode" CssClass="TextBox" Enabled="false" AutoPostBack="true" />
                        <asp:TextBox runat="server"  CssClass="TextBox" ID="tbCustName" Enabled="false" Width="200px"/> 
                        <asp:TextBox ID="tbFgValue" Visible="false" runat="server" CssClass="TextBox" />
                     </td>
                </tr>                
                <tr>
                    <td>Forex Rate</td>
                    <td>:</td>
                    <td><asp:TextBox CssClass="TextBox" Enabled="false" runat="server" ID="tbForexRate" /> </td>
                </tr>
                <tr>
                    <td>Amount Forex</td>
                    <td>:</td>
                    <td><asp:TextBox ID="tbAmountForex" Enabled="false" runat="server" CssClass="TextBox" /></td>                    
                </tr>  
                <tr>
                    <td><asp:Label ID = "lbAmountHome" runat="server" CssClass="TextBox" Text = "Amount Home"/></td>
                    <td>:</td>
                    <td><asp:TextBox ID="tbAmountHome" runat="server" CssClass="TextBoxR" /></td>                    
                </tr>                
                <tr>
                    <td><asp:Label ID = "lbNewAmountHome" runat="server" CssClass="TextBox" Text = "New Amount Home"/></td>
                    <td>:</td>
                    <td><asp:TextBox CssClass="TextBoxR" runat="server" ID="tbNewAmountHome" /> </td>
                </tr>
                <tr>
                    <td><asp:Label ID = "lbAdjust" runat="server" CssClass="TextBox" Text = "Adjust Home"/></td>
                    <td>:</td>
                    <td><asp:TextBox ID="tbAdjust" runat="server" CssClass="TextBoxR" /></td>                    
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
    <asp:Panel runat="server" ID="pnlPrint" Visible="false">
    <cr:crystalreportviewer ID="CrystalReportViewer1" runat="server" AutoDataBind="True"
               Height="1036px" Width="928px" />
    </asp:Panel>
      <br />            
    </div>
   
    <asp:Label runat ="server" ID="lbStatus" ForeColor="Red"/>
   
    </form>
    </body>
</html>
