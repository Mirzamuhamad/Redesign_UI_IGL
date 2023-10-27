<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TrPYPPHYear.aspx.vb" Inherits="Transaction_TrPYPPHYear_TrPYPPHYear" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="BasicFrame.WebControls.BasicDatePicker" Namespace="BasicFrame.WebControls"
    TagPrefix="BDP" %>
<%@ Register assembly="CrystalDecisions.Web, Version=10.5.3700.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Untitled Page</title>
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
        
        function postback()
        {
            __doPostBack('','');
        }   
    </script>
    
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
</head>
<body>     
    <form id="form1" runat="server">    
    <div class="Content">
    <div class="H1">Process PPH Year</div>
     <hr style="color:Blue" />        
    <asp:Panel runat="server" id="PnlHd"> 
      <table>
        <tr>
            <td style="width:100px;text-align:right">Quick Search :</td>            
            <td><asp:TextBox CssClass="TextBox" runat="server" ID ="tbFilter"/> 
                  <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlField" >
                      <asp:ListItem Value="Year" Selected="True">Year</asp:ListItem>
                      <asp:ListItem>Status</asp:ListItem>
                  </asp:DropDownList>
                  <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlRange" 
                    Visible="False"></asp:DropDownList>
                  <asp:Button class="bitbtn btnsearch" runat="server" ID="btnSearch" Text="Search" />                 
                  <asp:Button class="btngo" runat="server" ID="btnExpand" Text="..."/>       
            </td>
            <td>
                <asp:LinkButton ID="LbAdvSearch" runat="server" Text="Advanced Search" Visible = "false"/>
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
                  <asp:ListItem Value="Year" Selected="True">Year</asp:ListItem>
                  <asp:ListItem>Status</asp:ListItem>
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
                  <asp:BoundField DataField="Year" HeaderStyle-Width="120px" SortExpression="Year" HeaderText="Year"><HeaderStyle Width="120px" /></asp:BoundField>                  
                  <asp:BoundField DataField="Status" HeaderText="Status" SortExpression="Status"></asp:BoundField>                                   
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
    <asp:Panel runat="server" ID="pnlInput" Visible="false" Height="517px">
      <table>
        <tr>
            <td>Year</td>
            <td>:</td>
            <td><asp:DropDownList ID="ddlYear" runat="server" CssClass="DropDownList" ValidationGroup="Input" />
            </td>            
            <td><asp:Button ID="btnGetData" runat="server" class="bitbtn btnsearch" 
                    Text="Process" Width="78px" ValidationGroup="Input" />
            </td>
        </tr>
        <tr>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td colspan="2">&nbsp;</td>
        </tr>
      </table>  
      
      <div style="font-size:medium; color:Blue;">Detail</div>
      <hr style="color:Blue" />  
        <asp:Panel runat="server" ID="pnlDt" Height="425px" style="margin-bottom: 0px">
            <table style="width: 842px">
                <tr>
                    <td>
                        Total PPH This Year :
                        <asp:Label ID="LbTotalPPH" runat="server" Font-Bold="True" ForeColor="Red" 
                            Text="TotalPPH"></asp:Label>
                    </td>
                    <td>
                        PPH Has Paid :
                        <asp:Label ID="LbPPHHAsPaid" runat="server" Font-Bold="True" ForeColor="Red" 
                            Text="PPHHasPaid"></asp:Label>
                    </td>
                    <td>
                        PPH Adjust :
                        <asp:Label ID="lbPPHAdjust" runat="server" Font-Bold="True" ForeColor="Red" 
                            Text="PPHAdjust"></asp:Label>
                    </td>
                </tr>
            </table>
            <asp:Button ID="btnAddDt" runat="server" class="bitbtndt btnadd" Text="Add" 
                Visible="false" />
            <div style="border-style: solid; border-color: inherit; border-width: 0px; width:100%; height:76%; overflow:auto;">
                <asp:GridView ID="GridDt" runat="server" AutoGenerateColumns="False" 
                    ShowFooter="True">
                    <HeaderStyle CssClass="GridHeader" />
                    <RowStyle CssClass="GridItem" Wrap="True" />
                    <AlternatingRowStyle CssClass="GridAltItem" />
                    <PagerStyle CssClass="GridPager" />
                    <Columns>
                        <asp:TemplateField HeaderText="Action"  ItemStyle-Wrap = "false">
                            <ItemTemplate>
                                <asp:Button ID="btnEdit" runat="server" class="bitbtndt btnedit" 
                                    CommandName="Edit" Text="Edit" />
                                <%--<asp:Button ID="btnDelete" runat="server" class="bitbtndt btndelete" 
                                    CommandName="Delete" Text="Delete" />--%>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:Button ID="btnUpdate" runat="server" class="bitbtndt btnsave" 
                                    CommandName="Update" Text="Save" />
                                <asp:Button ID="btnCancel" runat="server" class="bitbtndt  btncancel" 
                                    CommandName="Cancel" Text="Cancel" />
                            </EditItemTemplate>
                            <ItemStyle Wrap="False" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-Width="80" HeaderText="Employee No" 
                            SortExpression="EmpNumb">
                            <Itemtemplate>
                                <asp:Label ID="EmpNumb" Runat="server" 
                                    text='<%# DataBinder.Eval(Container.DataItem, "EmpNumb") %>' Width="80">
                                </asp:Label>
                            </Itemtemplate>
                            <EditItemTemplate>
                                <asp:Label ID="EmpNumbEdit" Runat="server" 
                                    text='<%# DataBinder.Eval(Container.DataItem, "EmpNumb") %>' Width="80">
                                </asp:Label>
                            </EditItemTemplate>
                            <HeaderStyle Width="80px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-Width="200" HeaderText="Employee Name" 
                            SortExpression="EmpName">
                            <Itemtemplate>
                                <asp:Label ID="EmpName" Runat="server" 
                                    text='<%# DataBinder.Eval(Container.DataItem, "EmpName") %>' Width="200">
                                </asp:Label>
                            </Itemtemplate>
                            <EditItemTemplate>
                                <asp:Label ID="EmpNameEdit" Runat="server" 
                                    text='<%# DataBinder.Eval(Container.DataItem, "EmpName") %>' Width="200">
                                </asp:Label>
                            </EditItemTemplate>
                            <HeaderStyle Width="200px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-Width="80" HeaderText="Start Date" 
                            SortExpression="StartDate">
                            <Itemtemplate>
                                <asp:Label ID="StartDate" Runat="server" dataformatstring="{0:dd MMM yyyy}" 
                                    htmlencode="true" 
                                    text='<%# DataBinder.Eval(Container.DataItem, "StartDate") %>' Width="80">
                                </asp:Label>
                            </Itemtemplate>
                            <EditItemTemplate>
                                <asp:Label ID="StartDateEdit" Runat="server" dataformatstring="{0:dd MMM yyyy}" 
                                    htmlencode="true" 
                                    text='<%# DataBinder.Eval(Container.DataItem, "StartDate") %>' Width="80">
                                </asp:Label>
                            </EditItemTemplate>
                            <HeaderStyle Width="80px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-Width="80" HeaderText="End Date" 
                            SortExpression="EndDate">
                            <Itemtemplate>
                                <asp:Label ID="EndDate" Runat="server" dataformatstring="{0:dd MMM yyyy}" 
                                    htmlencode="true" text='<%# DataBinder.Eval(Container.DataItem, "EndDate") %>' 
                                    Width="80"> </asp:Label>
                            </Itemtemplate>
                            <EditItemTemplate>
                                <asp:Label ID="EndDateEdit" Runat="server" dataformatstring="{0:dd MMM yyyy}" 
                                    htmlencode="true" text='<%# DataBinder.Eval(Container.DataItem, "EndDate") %>' 
                                    Width="80"> </asp:Label>
                            </EditItemTemplate>
                            <HeaderStyle Width="80px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-Width="80" HeaderText="Marital Tax" 
                            SortExpression="MaritalTax">
                            <Itemtemplate>
                                <asp:Label ID="MaritalTax" Runat="server" 
                                    text='<%# DataBinder.Eval(Container.DataItem, "MaritalTax") %>' Width="80">
                                </asp:Label>
                            </Itemtemplate>
                            <EditItemTemplate>
                                <asp:Label ID="MaritalTaxEdit" Runat="server" 
                                    text='<%# DataBinder.Eval(Container.DataItem, "MaritalTax") %>' Width="80">
                                </asp:Label>
                            </EditItemTemplate>
                            <HeaderStyle Width="80px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-Width="60" HeaderText="Curr" 
                            SortExpression="CurrCode">
                            <Itemtemplate>
                                <asp:Label ID="CurrCode" Runat="server" 
                                    text='<%# DataBinder.Eval(Container.DataItem, "CurrCode") %>' Width="60">
                                </asp:Label>
                            </Itemtemplate>
                            <EditItemTemplate>
                                <asp:Label ID="CurrCodeEdit" Runat="server" 
                                    text='<%# DataBinder.Eval(Container.DataItem, "CurrCode") %>' Width="60">
                                </asp:Label>
                            </EditItemTemplate>
                            <HeaderStyle Width="60px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-Width="80" HeaderText="Pesangon" 
                            SortExpression="TotalPesangon">
                            <Itemtemplate>
                                <asp:Label ID="TotalPesangon" Runat="server" 
                                    text='<%# DataBinder.Eval(Container.DataItem, "TotalPesangon") %>' Width="80">
                                </asp:Label>
                            </Itemtemplate>
                            <EditItemTemplate>
                                <asp:Label ID="TotalPesangonEdit" Runat="server" 
                                    text='<%# DataBinder.Eval(Container.DataItem, "TotalPesangon") %>' Width="80">
                                </asp:Label>
                            </EditItemTemplate>
                            <HeaderStyle Width="80px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-Width="80" HeaderText="Total Bruto" 
                            SortExpression="TotalBruto">
                            <Itemtemplate>
                                <asp:Label ID="TotalBruto" Runat="server" 
                                    text='<%# DataBinder.Eval(Container.DataItem, "TotalBruto") %>' Width="80">
                                </asp:Label>
                            </Itemtemplate>
                            <EditItemTemplate>
                                <asp:Label ID="TotalBrutoEdit" Runat="server" 
                                    text='<%# DataBinder.Eval(Container.DataItem, "TotalBruto") %>' Width="80">
                                </asp:Label>
                            </EditItemTemplate>
                            <HeaderStyle Width="80px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-Width="80" HeaderText="Iuran Jbt" 
                            SortExpression="TotalIuranJbt">
                            <Itemtemplate>
                                <asp:Label ID="TotalIuranJbt" Runat="server" 
                                    text='<%# DataBinder.Eval(Container.DataItem, "TotalIuranJbt") %>' Width="80">
                                </asp:Label>
                            </Itemtemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="TotalIuranJbtEdit" Runat="server" CssClass="TextBox" 
                                    Text='<%# DataBinder.Eval(Container.DataItem, "TotalIuranJbt") %>' Width="80">
                                </asp:TextBox>
                            </EditItemTemplate>
                            <HeaderStyle Width="80px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-Width="80" HeaderText="Total Premi" 
                            SortExpression="TotalPremi">
                            <Itemtemplate>
                                <asp:Label ID="TotalPremi" Runat="server" 
                                    text='<%# DataBinder.Eval(Container.DataItem, "TotalPremi") %>' Width="80">
                                </asp:Label>
                            </Itemtemplate>
                            <EditItemTemplate>
                                <asp:Label ID="TotalPremiEdit" Runat="server" 
                                    text='<%# DataBinder.Eval(Container.DataItem, "TotalPremi") %>' Width="80">
                                </asp:Label>
                            </EditItemTemplate>
                            <HeaderStyle Width="80px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-Width="80" HeaderText="Total Insentif" 
                            SortExpression="TotalInsentif">
                            <Itemtemplate>
                                <asp:Label ID="TotalInsentif" Runat="server" 
                                    text='<%# DataBinder.Eval(Container.DataItem, "TotalInsentif") %>' Width="80">
                                </asp:Label>
                            </Itemtemplate>
                            <EditItemTemplate>
                                <asp:Label ID="TotalInsentifEdit" Runat="server" 
                                    text='<%# DataBinder.Eval(Container.DataItem, "TotalInsentif") %>' Width="80">
                                </asp:Label>
                            </EditItemTemplate>
                            <HeaderStyle Width="80px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-Width="80" HeaderText="Total THR" 
                            SortExpression="TotalTHR">
                            <Itemtemplate>
                                <asp:Label ID="TotalTHR" Runat="server" 
                                    text='<%# DataBinder.Eval(Container.DataItem, "TotalTHR") %>' Width="80">
                                </asp:Label>
                            </Itemtemplate>
                            <EditItemTemplate>
                                <asp:Label ID="TotalTHREdit" Runat="server" 
                                    text='<%# DataBinder.Eval(Container.DataItem, "TotalTHR") %>' Width="80">
                                </asp:Label>
                            </EditItemTemplate>
                            <HeaderStyle Width="80px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-Width="80" HeaderText="Total Netto" 
                            SortExpression="TotalNetto">
                            <Itemtemplate>
                                <asp:Label ID="TotalNetto" Runat="server" 
                                    text='<%# DataBinder.Eval(Container.DataItem, "TotalNetto") %>' Width="80">
                                </asp:Label>
                            </Itemtemplate>
                            <EditItemTemplate>
                                <asp:Label ID="TotalNettoEdit" Runat="server" 
                                    text='<%# DataBinder.Eval(Container.DataItem, "TotalNetto") %>' Width="80">
                                </asp:Label>
                            </EditItemTemplate>
                            <HeaderStyle Width="80px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-Width="80" HeaderText="Year Bruto" 
                            SortExpression="YearBruto">
                            <Itemtemplate>
                                <asp:Label ID="YearBruto" Runat="server" 
                                    text='<%# DataBinder.Eval(Container.DataItem, "YearBruto") %>' Width="80">
                                </asp:Label>
                            </Itemtemplate>
                            <EditItemTemplate>
                                <asp:Label ID="YearBrutoEdit" Runat="server" 
                                    text='<%# DataBinder.Eval(Container.DataItem, "YearBruto") %>' Width="80">
                                </asp:Label>
                            </EditItemTemplate>
                            <HeaderStyle Width="80px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-Width="80" HeaderText="Year Iuran Jbt" 
                            SortExpression="YearIuranJbt">
                            <Itemtemplate>
                                <asp:Label ID="YearIuranJbt" Runat="server" 
                                    text='<%# DataBinder.Eval(Container.DataItem, "YearIuranJbt") %>' Width="80">
                                </asp:Label>
                            </Itemtemplate>
                            <EditItemTemplate>
                                <asp:Label ID="YearIuranJbtEdit" Runat="server" 
                                    text='<%# DataBinder.Eval(Container.DataItem, "YearIuranJbt") %>' Width="80">
                                </asp:Label>
                            </EditItemTemplate>
                            <HeaderStyle Width="80px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-Width="80" HeaderText="Year Premi" 
                            SortExpression="YearPremi">
                            <Itemtemplate>
                                <asp:Label ID="YearPremi" Runat="server" 
                                    text='<%# DataBinder.Eval(Container.DataItem, "YearPremi") %>' Width="80">
                                </asp:Label>
                            </Itemtemplate>
                            <EditItemTemplate>
                                <asp:Label ID="YearPremiEdit" Runat="server" 
                                    text='<%# DataBinder.Eval(Container.DataItem, "YearPremi") %>' Width="80">
                                </asp:Label>
                            </EditItemTemplate>
                            <HeaderStyle Width="80px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-Width="80" HeaderText="Total Year Insentif" 
                            SortExpression="TotalInsentif">
                            <Itemtemplate>                                
                                <asp:Label ID="TotalYearInsentif" Runat="server" 
                                    text='<%# DataBinder.Eval(Container.DataItem, "TotalInsentif") %>' Width="80">
                                </asp:Label>
                            </Itemtemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="TotalYearInsentifEdit" Runat="server" CssClass="TextBox" 
                                    Text='<%# DataBinder.Eval(Container.DataItem, "TotalInsentif") %>' Width="80">
                                </asp:TextBox>
                            </EditItemTemplate>
                            <HeaderStyle Width="80px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-Width="80" HeaderText="Total Year THR" 
                            SortExpression="TotalTHR">
                            <Itemtemplate>
                                <asp:Label ID="TotalYearTHR" Runat="server" 
                                    text='<%# DataBinder.Eval(Container.DataItem, "TotalTHR") %>' Width="80">
                                </asp:Label>
                            </Itemtemplate>
                            <EditItemTemplate>
                                 <asp:TextBox ID="TotalYearTHREdit" Runat="server" CssClass="TextBox" 
                                    Text='<%# DataBinder.Eval(Container.DataItem, "TotalTHR") %>' Width="80">
                                </asp:TextBox>
                             </EditItemTemplate>
                            <HeaderStyle Width="80px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-Width="80" HeaderText="Year Netto" 
                            SortExpression="YearNetto">
                            <Itemtemplate>
                                <asp:Label ID="YearNetto" Runat="server" 
                                    text='<%# DataBinder.Eval(Container.DataItem, "YearNetto") %>' Width="80">
                                </asp:Label>
                            </Itemtemplate>
                            <EditItemTemplate>
                                <asp:Label ID="YearNettoEdit" Runat="server" 
                                    text='<%# DataBinder.Eval(Container.DataItem, "YearNetto") %>' Width="80">
                                </asp:Label>
                            </EditItemTemplate>
                            <HeaderStyle Width="80px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-Width="80" HeaderText="Year PTKP" 
                            SortExpression="YearPTKP">
                            <Itemtemplate>
                                <asp:Label ID="YearPTKPK" Runat="server" 
                                    text='<%# DataBinder.Eval(Container.DataItem, "YearPTKP") %>' Width="80">
                                </asp:Label>
                            </Itemtemplate>
                            <EditItemTemplate>
                                <asp:Label ID="YearPTKPEdit" Runat="server" 
                                    text='<%# DataBinder.Eval(Container.DataItem, "YearPTKP") %>' Width="80">
                                </asp:Label>
                            </EditItemTemplate>
                            <HeaderStyle Width="80px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-Width="80" HeaderText="Year PKP" 
                            SortExpression="YearPKP">
                            <Itemtemplate>
                                <asp:Label ID="YearPKP" Runat="server" 
                                    text='<%# DataBinder.Eval(Container.DataItem, "YearPKP") %>' Width="80">
                                </asp:Label>
                            </Itemtemplate>
                            <EditItemTemplate>
                                <asp:Label ID="YearPKPEdit" Runat="server" 
                                    text='<%# DataBinder.Eval(Container.DataItem, "YearPKP") %>' Width="80">
                                </asp:Label>
                            </EditItemTemplate>
                            <HeaderStyle Width="80px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-Width="80" HeaderText="Year PPH" 
                            SortExpression="YearPPH">
                            <Itemtemplate>
                                <asp:Label ID="YearPPH" Runat="server" 
                                    text='<%# DataBinder.Eval(Container.DataItem, "YearPPH") %>' Width="80">
                                </asp:Label>
                            </Itemtemplate>
                            <EditItemTemplate>
                                <asp:Label ID="YearPPHEdit" Runat="server" 
                                    text='<%# DataBinder.Eval(Container.DataItem, "YearPPH") %>' Width="80">
                                </asp:Label>
                            </EditItemTemplate>
                            <HeaderStyle Width="80px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-Width="80" HeaderText="PPH Cummulation" 
                            SortExpression="TotalPPH">
                            <Itemtemplate>
                                <asp:Label ID="TotalPPH" Runat="server" 
                                    text='<%# DataBinder.Eval(Container.DataItem, "TotalPPH") %>' Width="80">
                                </asp:Label>
                            </Itemtemplate>
                            <EditItemTemplate>
                                <asp:Label ID="TotalPPHEdit" Runat="server" 
                                    text='<%# DataBinder.Eval(Container.DataItem, "TotalPPH") %>' Width="80">
                                </asp:Label>
                            </EditItemTemplate>
                            <HeaderStyle Width="80px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-Width="80" HeaderText="PPH Pesangon" 
                            SortExpression="PPHPesangon">
                            <Itemtemplate>
                                <asp:Label ID="PPHPesangon" Runat="server" 
                                    text='<%# DataBinder.Eval(Container.DataItem, "PPHPesangon") %>' Width="80">
                                </asp:Label>
                            </Itemtemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="PPHPesangonEdit" Runat="server" CssClass="TextBox" 
                                    Text='<%# DataBinder.Eval(Container.DataItem, "PPHPesangon") %>' Width="80">
                                </asp:TextBox>
                            </EditItemTemplate>
                            <HeaderStyle Width="80px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-Width="80" HeaderText="PPH Has Paid" 
                            SortExpression="PPHHaasPaid">
                            <Itemtemplate>
                                <asp:Label ID="PPHHasPaid" Runat="server" 
                                    text='<%# DataBinder.Eval(Container.DataItem, "PPHHasPaid") %>' Width="80">
                                </asp:Label>
                            </Itemtemplate>
                            <EditItemTemplate>
                                <asp:Label ID="PPHHasPaidEdit" Runat="server" 
                                    text='<%# DataBinder.Eval(Container.DataItem, "PPHHasPaid") %>' Width="80">
                                </asp:Label>
                            </EditItemTemplate>
                            <HeaderStyle Width="80px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-Width="80" HeaderText="PPH Adjust" 
                            SortExpression="PPHAdjust">
                            <Itemtemplate>
                                <asp:Label ID="PPHAdjust" Runat="server" 
                                    text='<%# DataBinder.Eval(Container.DataItem, "PPHAdjust") %>' Width="80">
                                </asp:Label>
                            </Itemtemplate>
                            <EditItemTemplate>
                                <asp:Label ID="PPHAdjustEdit" Runat="server" 
                                    text='<%# DataBinder.Eval(Container.DataItem, "PPHAdjust") %>' Width="80">
                                </asp:Label>
                            </EditItemTemplate>
                            <HeaderStyle Width="80px" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
            <asp:Button ID="btnAddDt2" runat="server" class="bitbtndt btnadd" Text="Add" 
                Visible="false" />
       </asp:Panel>             
       <br />    
       <asp:Button class="bitbtn btnsavenew" runat="server" ID="btnSaveAll" 
            Text="Save & New" ValidationGroup="Input" Width="96px"/>                              
       <asp:Button class="bitbtn btnsave" runat="server" ID="btnSaveTrans" Text="Save" ValidationGroup="Input"/>                              
       <asp:Button class="bitbtn btnback" runat="server" ID="btnBack" Text="Cancel" ValidationGroup="Input"/>                              
       <asp:Button class="btngo" runat="server" ID="btnHome" Text="Home" Width="45px"/>                              
    </asp:Panel>
    <asp:Panel runat="server" ID="pnlPrint" Visible="false">
    <cr:crystalreportviewer ID="CrystalReportViewer1" runat="server" AutoDataBind="True" Height="1036px" Width="928px" />
    </asp:Panel>
      <br />            
    </div>
    <br />
    <asp:Label runat ="server" ID="lbStatus" ForeColor="Red"/>
    <asp:HiddenField ID="HiddenRemarkClose" runat="server" />
    </form>
    </body>
</html>
