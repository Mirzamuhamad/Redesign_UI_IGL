<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TrMKCustLimit.aspx.vb" Inherits="Transaction_TrMKCustLimit_TrMKCustLimit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="BasicFrame.WebControls.BasicDatePicker" Namespace="BasicFrame.WebControls"
    TagPrefix="BDP" %>
<%@ Register assembly="CrystalDecisions.Web, Version=10.5.3700.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Adjust Customer Limit</title>
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
        document.getElementById("tbOldLimit").value = setdigit(document.getElementById("tbOldLimit").value.replace(/\$|\,/g,""),'<%=VIEWSTATE("DigitCurr")%>');
        document.getElementById("tbNewLimit").value = setdigit(document.getElementById("tbNewLimit").value.replace(/\$|\,/g,""),'<%=VIEWSTATE("DigitCurr")%>');
        document.getElementById("tbOldUsed").value = setdigit(document.getElementById("tbOldUsed").value.replace(/\$|\,/g,""),'<%=VIEWSTATE("DigitCurr")%>');
        document.getElementById("tbSaldo").value = setdigit(document.getElementById("tbSaldo").value.replace(/\$|\,/g,""),'<%=VIEWSTATE("DigitCurr")%>');
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
    <div class="H1"><b style="mso-bidi-font-weight:normal">
        <span style="font-size:12.0pt;font-family:&quot;Times New Roman&quot;;mso-fareast-font-family:
&quot;Times New Roman&quot;;mso-ansi-language:EN-US;mso-fareast-language:EN-US;
mso-bidi-language:AR-SA">Adjust Customer Limit </span></b></div>
     <hr style="color:Blue" />        
    <asp:Panel runat="server" id="PnlHd"> 
      <table>
        <tr>
            <td style="width:100px;text-align:right">Quick Search :</td>            
            <td><asp:TextBox CssClass="TextBox" runat="server" ID ="tbFilter"/> 
                  <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlField" >
                      <asp:ListItem Value="Reference" Selected="True">Reference</asp:ListItem>
                      <asp:ListItem Value="Trans_Date">Date</asp:ListItem>
                      <asp:ListItem>Status</asp:ListItem>
                      <asp:ListItem Value="Effective_Date">EffectiveDate</asp:ListItem>
                      <asp:ListItem Value="Remark">Remark</asp:ListItem>                      
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
                  <asp:ListItem Value="Reference" Selected="True">Reference</asp:ListItem>
                  <asp:ListItem Value="Trans_Date">Date</asp:ListItem>
                  <asp:ListItem>Status</asp:ListItem>
                  <asp:ListItem Value="Effective_Date">EffectiveDate</asp:ListItem>
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
            <asp:Button class="btngo" runat="server" ID="BtnGo" Text="G" Visible="false"/>          
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
                          <asp:Button class="btngo" runat="server" ID="BtnGo" Text="G" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" CommandName="Go"/>
                      </ItemTemplate>
                      <HeaderStyle Width="110px" />
                  </asp:TemplateField>                     
                  <asp:BoundField DataField="Reference" HeaderStyle-Width="120px" SortExpression="Nmbr" HeaderText="Reference"><HeaderStyle Width="120px" /></asp:BoundField>                  
                  <asp:BoundField DataField="Status" HeaderText="Status" SortExpression="Status"></asp:BoundField>                  
                  <asp:BoundField DataField="Trans_Date" HeaderStyle-Width="80px" HeaderText="Date" SortExpression="TransDate"><HeaderStyle Width="80px" /></asp:BoundField>
                  <asp:BoundField DataField="Effective_Date" HeaderText="Effective Date" SortExpression="EffectiveDate" ><HeaderStyle Width="80px" /></asp:BoundField>
                  <asp:BoundField DataField="Remark" HeaderStyle-Width="80px" HeaderText="Remark"><HeaderStyle Width="250px" /></asp:BoundField>
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
        <tr>
            <td>Effective Date</td>
            <td>:</td>
            <td>
                <BDP:BasicDatePicker ID="tbEffectiveDate" runat="server" AutoPostBack="True" 
                    ButtonImageHeight="19px" ButtonImageWidth="20px" DateFormat="dd MMM yyyy" 
                    DisplayType="TextBoxAndImage" TextBoxStyle-CssClass="TextDate" 
                    ValidationGroup="Input" ShowNoneButton="False"><TextBoxStyle CssClass="TextDate" /></BDP:BasicDatePicker>
                    &nbsp; &nbsp; &nbsp; 
                <asp:Button class="btngo" runat="server" ID="btnGetDt" Text="Get Data" 
                    ValidationGroup="Input" Width="52px"/>
            </td>
            
        </tr>
          <tr>
              <td>Remark</td>
              <td>:</td>
              <td><asp:TextBox ID="tbRemark" runat="server" CssClass="TextBox" ValidationGroup="Input" Width="269px" />
              </td>
          </tr>
      </table>  
      
      <br />      
      <div style="font-size:medium; color:Blue;">Detail</div>
      <hr style="color:Blue" />  
        <asp:Panel runat="server" ID="pnlDt">
            <asp:Button class="bitbtndt btnadd" runat="server" ID="btnAddDt" Text="Add" ValidationGroup="Input"/>
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
                                <asp:Button class="bitbtndt btnedit" runat="server" ID="btnEdit" Text="Edit" CommandName="Edit"/>
                                <asp:Button class="bitbtndt btndelete" runat="server" ID="btnDelete" Text="Delete" CommandName="Delete"/>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:Button class="bitbtndt btnsave" runat="server" ID="btnUpdate" Text="Save" CommandName="Update"/>
                                <asp:Button class="bitbtndt btncancel" runat="server" ID="btnCancel" Text="Save" CommandName="Cancel"/>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="CustomerName" HeaderStyle-Width="200px" HeaderText="Customer" >                            
                        </asp:BoundField>   
                        <asp:TemplateField Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lbCustomer" runat="server" text='<%# DataBinder.Eval(Container.DataItem, "Customer") %>' />
                            </ItemTemplate>                            
                        </asp:TemplateField>                     
                        <asp:BoundField DataField="Currency" HeaderText="Currency" />
                        <asp:BoundField DataField="OldLimit" HeaderText="Old Limit" >
                            <HeaderStyle Width="80px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="NewLimit" HeaderStyle-Width="80px" HeaderText="New Limit" >
                            <HeaderStyle Width="80px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="UseLimit" HeaderText="Old Used" />
                        <asp:BoundField DataField="SaldoLimit" HeaderText="Saldo Limit" />
                        <asp:BoundField DataField="Remark" HeaderStyle-Width="80px" HeaderText="Remark" >
                            <HeaderStyle Width="250px" />
                        </asp:BoundField>
                    </Columns>
                </asp:GridView>
          </div>   
          <asp:Button class="bitbtndt btnadd" runat="server" ID="btnAddDt2" Text="Add" ValidationGroup="Input"/>                 
        </asp:Panel>             
       <asp:Panel runat="server" ID="pnlEditDt" Visible="false">
            <table>
                <tr>
                    <td>
                        <asp:LinkButton ID="lbCustomer" runat="server" Text="Customer" />
                    </td>
                    <td>:</td>
                    <td><asp:TextBox ID="tbCustCode" runat="server" AutoPostBack="true" CssClass="TextBox" />
                        <asp:TextBox ID="tbCustName" runat="server" CssClass="TextBox" Enabled="False" EnableTheming="True" ReadOnly="True" Width="200px" />
                        <asp:Button class="btngo" runat="server" ID="btnCust" Text="..." ValidationGroup="Input"/>                 
                    </td>
                </tr>
                <tr>
                    <td><asp:LinkButton ID="lbCurrency" runat="server" Text="Currency" ValidationGroup="Input" /></td>
                    <td>&nbsp;</td>                    
                    <td><asp:DropDownList ID="ddlCurrency" runat="server" AutoPostBack="True" CssClass="DropDownList" Height="16px" ValidationGroup="Input" Width="69px">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>Old Limit</td>
                    <td>:</td>
                    <td><asp:TextBox ID="tbOldLimit" runat="server" CssClass="TextBoxR" AutoPostBack="True" />
                    </td>
                </tr>
                <tr>
                    <td>New Limit</td>
                    <td>:</td>
                    <td><asp:TextBox ID="tbNewLimit" runat="server" CssClass="TextBox" AutoPostBack="True" />
                    </td>
                </tr>
                <tr>
                    <td>Old Used</td>
                    <td>&nbsp;</td>
                    <td><asp:TextBox ID="tbOldUsed" runat="server" CssClass="TextBoxR" AutoPostBack="True" />
                    </td>
                </tr>
                <tr>
                    <td>Saldo Limit</td>
                    <td>&nbsp;</td>
                    <td><asp:TextBox ID="tbSaldo" runat="server" CssClass="TextBoxR" />
                    </td>
                </tr>
                <tr>
                    <td>Remark</td>
                    <td>:</td>
                    <td><asp:TextBox ID="tbRemarkDt" runat="server" CssClass="TextBox" Width="152px" /></td>
                </tr>
            </table>
            <br />
            <asp:Button class="bitbtndt btnsave" runat="server" ID="btnSaveDt" Text="Save" ValidationGroup="Input"/>                 
            <asp:Button class="bitbtndt btncancel" runat="server" ID="btnCancelDt" Text="Cancel" ValidationGroup="Input"/>                 
            <br />
       </asp:Panel> 
       <br />
       <asp:Button class="bitbtn btnsavenew" runat="server" ID="btnSaveAll" 
            Text="Save & New" ValidationGroup="Input" Width="89px"/>                 
       <asp:Button class="bitbtn btnsave" runat="server" ID="btnSaveTrans" Text="Save" 
            ValidationGroup="Input" Height="18px" Width="59px"/>                 
       <asp:Button class="bitbtn btnback" runat="server" ID="btnBack" Text="Cancel" 
            ValidationGroup="Input" Width="64px"/>                 
       <asp:Button class="btngo" runat="server" ID="btnHome" Text="Home" Width="44px"/>                 
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
