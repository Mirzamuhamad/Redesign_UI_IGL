<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TrPRPrice.aspx.vb" Inherits="Transaction_TrPRPrice_TrPRPrice" %>
<%@ Register Assembly="BasicFrame.WebControls.BasicDatePicker" Namespace="BasicFrame.WebControls"
    TagPrefix="BDP" %>
<%@ Register assembly="CrystalDecisions.Web, Version=10.5.3700.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Supplier Quotation</title>
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
        //var Rate = document.getElementById("tbRate");
        //var DrForex = document.getElementById("tbDebitForex");
        //var CrForex = document.getElementById("tbCreditForex");
        
         try
         {           
        document.getElementById("tbOldPrice").value = setdigit(document.getElementById("tbOldPrice").value.replace(/\$|\,/g,""),'<%=VIEWSTATE("DigitCurr")%>');
        document.getElementById("tbNewPrice").value = setdigit(document.getElementById("tbNewPrice").value.replace(/\$|\,/g,""),'<%=VIEWSTATE("DigitCurr")%>');
        document.getElementById("tbMOQ").value = setdigit(document.getElementById("tbMOQ").value.replace(/\$|\,/g,""),'<%=VIEWSTATE("DigitQty")%>');
        document.getElementById("tbLeadTime").value = setdigit(document.getElementById("tbLeadTime").value.replace(/\$|\,/g,""),'<%=VIEWSTATE("DigitQty")%>');
        }catch (err){
            alert(err.description);
          }      
        }   
    </script>
    
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .style1
        {
            width: 1px;
        }
    </style>
</head>
<body>     
    <form id="form1" runat="server">    
    <div class="Content">
    <div class="H1">Supplier Quotation</div>
     <hr style="color:Blue" />        
    <asp:Panel runat="server" id="PnlHd"> 
      <table>
        <tr>
            <td style="width:100px;text-align:right">Quick Search :</td>            
            <td><asp:TextBox CssClass="TextBox" runat="server" ID ="tbFilter"/> 
                  <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlField" >
                      <asp:ListItem Value="Reference" Selected="True">Reference</asp:ListItem>
                      <asp:ListItem Value="dbo.FormatDate(TransDate)">Date</asp:ListItem>
                      <asp:ListItem>Status</asp:ListItem>
                      <asp:ListItem Value="Quotation_No"> Quotation No</asp:ListItem>
                      <asp:ListItem Value="dbo.FormatDate(EffectiveDate)">EffectiveDate</asp:ListItem>
                      <asp:ListItem Value="Supplier_Name">Supplier</asp:ListItem>
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
                  <asp:ListItem Value="dbo.FormatDate(TransDate)">Date</asp:ListItem>
                  <asp:ListItem>Status</asp:ListItem>
                  <asp:ListItem Value="Quotation_No">Quotation No</asp:ListItem>
                  <asp:ListItem Value="dbo.FormatDate(EffectiveDate)">Effective Date</asp:ListItem>
                  <asp:ListItem Value="Supplier_Name">Supplier</asp:ListItem>
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
            <asp:Button class="btngo" Visible="false" runat="server" OnClick = "OnConfirm" OnClientClick = "Confirm()" ID="BtnGo" Text="G"/>  
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
                          <asp:Button class="btngo" runat="server" ID="BtnGo" Text="G" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" CommandName="Go" />
                      </ItemTemplate>
                      <HeaderStyle Width="110px" />
                  </asp:TemplateField>                     
                  <asp:BoundField DataField="Reference" HeaderStyle-Width="120px" 
                      SortExpression="Nmbr" HeaderText="Reference">
                      <HeaderStyle Width="120px" />
                  </asp:BoundField>                  
                  <asp:BoundField DataField="Status" HeaderText="Status" SortExpression="Status"></asp:BoundField>                  
                  <asp:BoundField DataField="TransDate" dataformatstring="{0:dd MMM yyyy}" htmlencode="true" HeaderStyle-Width="80px" 
                      HeaderText="Date" SortExpression="TransDate">
                      <HeaderStyle Width="80px" />
                  </asp:BoundField>
                  <asp:BoundField DataField="EffectiveDate" dataformatstring="{0:dd MMM yyyy}" htmlencode="true" HeaderText="Effective Date" 
                      SortExpression="EffectiveDate" >
                      <HeaderStyle Width="80px" />
                  </asp:BoundField>
                  <asp:BoundField DataField="Quotation_No" HeaderStyle-Width="80px" 
                      HeaderText="Quotation No" SortExpression="Quotation_No">
                      <HeaderStyle Width="80px" />
                  </asp:BoundField>
                  <asp:BoundField DataField="Supplier" HeaderText="Supplier" 
                      SortExpression="Supplier" />
                  <asp:BoundField DataField="Remark" 
                      HeaderText="Remark">
                      <HeaderStyle Width="250px" />
                  </asp:BoundField>
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
            <td style="margin-left: 40px"><asp:TextBox CssClass="TextBox" 
                     runat="server" ID="tbRef" Width="149px" 
                    Enabled="False"/> &nbsp; &nbsp; 
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
                <asp:Label ID="Label1" runat="server" Text="*" ForeColor="#FF3300"></asp:Label>              
            </td>            
        </tr>      
        <tr>
            <td>Effective Date</td>
            <td>:</td>
            <td>
                <BDP:BasicDatePicker ID="tbEffectiveDate" runat="server" 
                    ButtonImageHeight="19px" ButtonImageWidth="20px" DateFormat="dd MMM yyyy" 
                    DisplayType="TextBoxAndImage" ShowNoneButton="False" 
                    TextBoxStyle-CssClass="TextDate" ValidationGroup="Input" 
                    AutoPostBack="True"><TextBoxStyle CssClass="TextDate" /></BDP:BasicDatePicker>
                <asp:Label ID="Label2" runat="server" ForeColor="#FF3300" Text="*"></asp:Label>
            </td>
        </tr>
          <tr>
              <td>
                  Quotation No
              </td>
              <td>
                  :</td>
              <td>
                  <asp:TextBox ID="tbSuppQouNo" runat="server" CssClass="TextBox" 
                      ValidationGroup="Input" Width="149px" />
                  <asp:Label ID="Label3" runat="server" ForeColor="#FF3300" Text="*"></asp:Label>
              </td>
          </tr>
        <tr>
            <td>
                <asp:LinkButton ID="lbSupplier" runat="server" Text="Supplier" 
                    ValidationGroup="Input" />
            </td>
            <td>:</td>
            <td>
                <asp:TextBox ID="tbSupplier" runat="server" CssClass="TextBox" 
                    ValidationGroup="Input" Width="98px" AutoPostBack="True" />
                <asp:TextBox ID="tbSupplierName" runat="server" CssClass="TextBoxR" Width="252px" Enabled="False" />&nbsp;         
                <asp:Button class="btngo" runat="server" ID="BtnSupplier" Text="..." ValidationGroup="Input"/>  
                <asp:Label ID="Label4" runat="server" ForeColor="#FF3300" Text="*"></asp:Label>
&nbsp;</td>
        </tr>
          <tr>
              <td>
                  Remark</td>
              <td>
                  :</td>
              <td>
                  <asp:TextBox ID="tbRemark" runat="server" ValidationGroup="Input" Width="350px" MaxLength="255" CssClass="TextBoxMulti" TextMode="MultiLine" /> &nbsp;
                  <asp:Button class="btngo" runat="server" ID="btnGetDt" Text="Get Data" 
                      ValidationGroup="Input" Width="56px"/>               
              </td>
          </tr>
      </table>  
      
      <br />      
      <div style="font-size:medium; color:Blue;">Detail</div>
      <hr style="color:Blue" />  
        <asp:Panel runat="server" ID="pnlDt">
            <asp:Button class="bitbtndt btnadd" runat="server" ID="btnAddDt" Text="Add" ValidationGroup="Input" />                 
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
                                <asp:Button ID="btnEdit" runat="server" class="bitbtn btnedit" Text="Edit" CommandName="Edit"/>
                                <asp:Button ID="btnDelete" runat="server" class="bitbtndt btndelete" Text="Delete" OnClientClick="return confirm('Sure to delete this data?');" CommandName="Delete"/>	
                            </ItemTemplate>
                            <%--<EditItemTemplate>
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
                            </EditItemTemplate>--%>
                        </asp:TemplateField>
                        <%--baru sampai sini --%>
                        <asp:BoundField DataField="ProductCode" HeaderStyle-Width="80px" HeaderText="Product Code" >
                            <HeaderStyle Width="80px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="ProductName" HeaderStyle-Width="250px"  
                            HeaderText="Product Name" >
                            <HeaderStyle Width="250px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Specification" HeaderText="Specification" />
                        <asp:BoundField DataField="Unit" HeaderText="Unit" />
                        <asp:BoundField DataField="Currency" HeaderText="Currency" 
                            SortExpression="Currency" />
                        <asp:BoundField DataField="OldPrice" HeaderText="Old Price" DataFormatString="{0:#,##0.00}"  ItemStyle-HorizontalAlign="Right" >
                            <HeaderStyle Width="80px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="NewPrice" HeaderStyle-Width="80px" DataFormatString="{0:#,##0.00}"  ItemStyle-HorizontalAlign="Right" 
                            HeaderText="New Price" >
                            <HeaderStyle Width="80px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="MOQ" HeaderStyle-Width="80px" DataFormatString="{0:#,##0.00}"  ItemStyle-HorizontalAlign="Right" 
                            HeaderText="MOQ" >
                            <HeaderStyle Width="80px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="LeadTime" HeaderStyle-Width="80px" DataFormatString="{0:#,##0}"  ItemStyle-HorizontalAlign="Right"
                            HeaderText="Lead Time (Days)" >
                            <HeaderStyle Width="80px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Remark" HeaderStyle-Width="80px" 
                            HeaderText="Remark" >
                            <HeaderStyle Width="250px" />
                        </asp:BoundField>
                    </Columns>
                </asp:GridView>
          </div>   
          <asp:Button class="bitbtndt btnadd" runat="server" ID="btnAddDt2" Text="Add" ValidationGroup="Input" />                     
       </asp:Panel>             
       <asp:Panel runat="server" ID="pnlEditDt" Visible="false">
            <table>
                <tr>
                    <td><asp:LinkButton ID="lbProduct"  runat="server" Text="Product"/></td>
                    <td class="style1">:</td>
                    <td>
                        <asp:TextBox ID="tbProduct" runat="server" CssClass="TextBox" Width="145px" 
                            AutoPostBack="True" />
                        <asp:TextBox ID="tbProductName" runat="server" CssClass="TextBoxR" 
                            Width="197px" Enabled="False" />
                        <asp:Button class="btngo" runat="server" ID="BtnProduct" Text="..." ValidationGroup="Input"/>     
                        <asp:Label ID="Label5" runat="server" ForeColor="#FF3300" Text="*"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>Specification</td>
                    <td class="style1">:</td>
                    <td>
                        <asp:TextBox ID="tbSpecification" runat="server" CssClass="TextBoxR"
                            Width="355px" Height="40px" TextMode="MultiLine" />
                    </td>
                </tr>
                <tr>
                    <td>
                        Unit</td>
                    <td class="style1">
                        :</td>
                    <td>
                        <asp:DropDownList ID="ddlUnit" runat="server" CssClass="DropDownList" Width="110px" AutoPostBack ="true" />
                        <asp:Label ID="Label6" runat="server" ForeColor="#FF3300" Text="*"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:LinkButton ID="lbCurrency" runat="server" Text="Currency" />
                    </td>
                    <td class="style1">
                        :</td>
                    <td>
                        <asp:DropDownList ID="ddlCurrency" runat="server" AutoPostBack="True" 
                            CssClass="DropDownList" Height="16px" ValidationGroup="Input" Width="110px">
                        </asp:DropDownList>
                        <asp:Label ID="Label7" runat="server" ForeColor="#FF3300" Text="*"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>Old Price</td>
                    <td class="style1">:</td>                    
                    <td><asp:TextBox runat="server" CssClass="TextBoxR" ID="tbOldPrice" 
                            AutoPostBack="True" Enabled="False" />
                    </td>
                </tr>
                <tr>
                    <td>New Price</td>
                    <td class="style1">:</td>
                    <td><asp:TextBox ID="tbNewPrice" runat="server" CssClass="TextBox" />
                        <asp:Label ID="Label10" runat="server" ForeColor="#FF3300" Text="*"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>MOQ</td>
                    <td class="style1">:</td>
                    <td><asp:TextBox ID="tbMOQ" runat="server" CssClass="TextBox" />
                        <asp:Label ID="lbMOQ" runat="server"/>
                        <asp:Label ID="Label8" runat="server" ForeColor="#FF3300" Text="*"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>Lead Time</td>
                    <td class="style1">:</td>
                    <td><asp:TextBox ID="tbLeadTime" runat="server" CssClass="TextBox" Width="48px" />&nbsp;Days</td>
                </tr>
                <tr>
                    <td>Remark</td>
                    <td class="style1">:</td>
                    <td><asp:TextBox ID="tbRemarkDt" runat="server" Width="350px" MaxLength="255" CssClass="TextBoxMulti" TextMode="MultiLine" />
                    </td>
                </tr>
            </table>
            <br />
            <asp:Button class="bitbtndt btnsave" runat="server" ID="btnSaveDt" Text="Save"/>
            <asp:Button class="bitbtndt btncancel" runat="server" ID="btnCancelDt" Text="Cancel"/>
            <br />
       </asp:Panel> 
       <br /> 
       <asp:Button class="bitbtn btnsavenew" runat="server" ID="btnSaveAll" 
            Text="Save & New" ValidationGroup="Input" Width="96px"/>
       <asp:Button class="bitbtn btnsave" runat="server" ID="btnSaveTrans" 
            Text="Save" ValidationGroup="Input" Width="64px"/>
       <asp:Button class="bitbtn btncancel" runat="server" ID="btnBack" 
            Text="Cancel" ValidationGroup="Input" Width="73px"/>
       <asp:Button class="btngo" runat="server" ID="btnHome" 
            Text="Home" Width="62px"/>      
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
