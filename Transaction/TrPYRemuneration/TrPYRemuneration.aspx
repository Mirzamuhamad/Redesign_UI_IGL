<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TrPYRemuneration.aspx.vb" Inherits="Transaction_TrPYRemuneration_TrPYRemuneration" %>
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
    
        function setformat(_prmChange)
        {
        
         try
         {      
            var CurrentAmount = parseFloat(document.getElementById("tbCurrentAmount").value.replace(/\$|\,/g,""));
            var NewAmount = parseFloat(document.getElementById("tbNewAmount").value.replace(/\$|\,/g,""));
            var AdjustAmount = parseFloat(document.getElementById("tbAdjustAmount").value.replace(/\$|\,/g,""));
            var AdjustPercent = parseFloat(document.getElementById("tbAdjustPercent").value.replace(/\$|\,/g,""));
            if(isNaN(CurrentAmount) == true)
            {
                CurrentAmount = 0;
            }
            if(isNaN(NewAmount) == true)
            {
                NewAmount = 0;
            }
            if(isNaN(AdjustAmount) == true)
            {
                AdjustAmount = 0;
            }
            if(isNaN(AdjustPercent) == true)
            {
                AdjustPercent = 0;
            }
            if (_prmChange == 'new')
            {
                AdjustAmount = parseFloat(NewAmount) - parseFloat(CurrentAmount);                
                AdjustPercent = (parseFloat(NewAmount) - parseFloat(CurrentAmount))*100.00/parseFloat(CurrentAmount);                
            }
            if (_prmChange == 'adjust')
            {
                NewAmount = parseFloat(AdjustAmount) + parseFloat(CurrentAmount);                
                AdjustPercent = (parseFloat(NewAmount) - parseFloat(CurrentAmount))*100.00/parseFloat(CurrentAmount);                
            }
            if (_prmChange == 'percent')
            {
                AdjustAmount = (parseFloat(AdjustPercent) * parseFloat(CurrentAmount)/100.00);      
                NewAmount = parseFloat(AdjustAmount) + parseFloat(CurrentAmount);                          
            }
            document.getElementById("tbCurrentAmount").value = setdigit(CurrentAmount,-1);
            document.getElementById("tbNewAmount").value = setdigit(NewAmount,-1);
            document.getElementById("tbAdjustAmount").value = setdigit(AdjustAmount,-1);
            document.getElementById("tbAdjustPercent").value = setdigit(AdjustPercent,-1);         
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
    <div class="H1">Payroll Remuneration Salary</div>
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
                      <asp:ListItem Value="dbo.FormatDate(EffectiveDate)">Effective Date</asp:ListItem>
                      <asp:ListItem Value="PayrollName">Payroll</asp:ListItem>
                      <asp:ListItem Value="ItemName">Item Name</asp:ListItem>                      
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
                      <asp:ListItem Value="TransNmbr" Selected="True">Reference</asp:ListItem>
                      <asp:ListItem Value="dbo.FormatDate(TransDate)">Date</asp:ListItem>
                      <asp:ListItem>Status</asp:ListItem>                      
                      <asp:ListItem Value="dbo.FormatDate(EffectiveDate)">Effective Date</asp:ListItem>
                      <asp:ListItem Value="PayrollName">Payroll</asp:ListItem>
                      <asp:ListItem Value="ItemName">Item Name</asp:ListItem>                      
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
                  <asp:BoundField DataField="TransNmbr" HeaderStyle-Width="120px" SortExpression="Nmbr" HeaderText="Reference">
                  <HeaderStyle Width="120px" />
                  </asp:BoundField>                  
                  <asp:BoundField DataField="Status" HeaderText="Status" SortExpression="Status"></asp:BoundField>                  
                  <asp:BoundField DataField="TransDate" dataformatstring="{0:dd MMM yyyy}" htmlencode="true" HeaderStyle-Width="80px" HeaderText="Date" SortExpression="TransDate">
                  <HeaderStyle Width="80px" />
                  </asp:BoundField>
                  <asp:BoundField DataField="EffectiveDate" dataformatstring="{0:dd MMM yyyy}" htmlencode="true" HeaderStyle-Width="100px" HeaderText="Effective Date" SortExpression="EffectiveDate">
                  <HeaderStyle Width="100px" />
                  </asp:BoundField>
                  <asp:BoundField DataField="Remark" HeaderStyle-Width="250px" HeaderText="Remark">
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
            <td><BDP:BasicDatePicker ID="tbEffectiveDate" runat="server" AutoPostBack="True" 
                    ButtonImageHeight="19px" ButtonImageWidth="20px" DateFormat="dd MMM yyyy" 
                    DisplayType="TextBoxAndImage" TextBoxStyle-CssClass="TextDate" 
                    ValidationGroup="Input" ShowNoneButton="False"><TextBoxStyle CssClass="TextDate" /></BDP:BasicDatePicker>
                    &nbsp; &nbsp; &nbsp; 
            </td>            
        </tr>      
        
        <tr>
            <td>Remark</td>
            <td>:</td>
            <td><asp:TextBox ID="tbRemark" runat="server" CssClass="TextBoxMulti" ValidationGroup="Input" Width="350px" TextMode="MultiLine" MaxLength ="255"/>
            <asp:Button class="bitbtn btngetitem" runat="server" ID="btnGetDt" Text="Get Data" ValidationGroup="Input"/>       
            </td>
        </tr>
      </table>  
      
      <br />      
      <div style="font-size:medium; color:Blue;">Detail</div>
      <hr style="color:Blue" />  
        <asp:Panel runat="server" ID="pnlDt">
            <table>
            <tr>
            <td>            
            <asp:Panel ID="Panel1" runat="server" BorderColor="Black" BorderStyle="Solid" 
                    Height="100%" Width="304px">
            &nbsp;<asp:Label ID="Label1" runat="server" Font-Bold="True" ForeColor="Blue" Text="Set Adjust"></asp:Label>
            <asp:DropDownList ID="ddlAdjustType" runat="server" AutoPostBack="False" 
                    CssClass="DropDownList" Height="17px" Width="90px" >
                <asp:ListItem Selected="True">Percentage</asp:ListItem>
                <asp:ListItem>Nominal</asp:ListItem>
                </asp:DropDownList>
            <asp:TextBox ID="tbAdjPercent" runat="server" AutoPostBack="False" 
                    CssClass="TextBox" Width="70px" /> 
            <asp:Button class="btngo" runat="server" ID="btnProcess" Text="Process" ValidationGroup="Input" Width="49px"/>       
            </asp:Panel>
            </td>
            </tr>
            </table>
            <asp:Button class="bitbtndt btnadd" runat="server" ID="btnAddDt" Text="Add" ValidationGroup="Input"/>       
            <div style="border:0px  solid; width:100%; height:100%; overflow:auto;">
                <asp:GridView ID="GridDt" runat="server" AutoGenerateColumns="False" 
                    ShowFooter="True">
                    <HeaderStyle CssClass="GridHeader" />
                    <RowStyle CssClass="GridItem" Wrap="false" />
                    <AlternatingRowStyle CssClass="GridAltItem" />
                    <PagerStyle CssClass="GridPager" />
                    <Columns>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:CheckBox ID="cbSelectHd" runat="server" AutoPostBack="true" oncheckedchanged="cbSelectHd_CheckedChanged1" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="cbSelect" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Action">
                            <ItemTemplate>
                                <asp:Button class="bitbtndt btnedit" runat="server" ID="btnEdit" Text="Edit" CommandName="Edit"/>       
                                <asp:Button class="bitbtndt btndelete" runat="server" ID="btnDelete" Text="Delete" CommandName="Delete"/>       
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:Button class="bitbtndt btnsave" runat="server" ID="btnUpdate" Text="Save" CommandName="Update"/>       
                                <asp:Button class="bitbtndt  btncancel" runat="server" ID="btnCancel" Text="Cancel" CommandName="Cancel"/>       
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="Payroll" HeaderStyle-Width="100px" HeaderText="Payroll">
                        <HeaderStyle Width="100px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="PayrollName" HeaderStyle-Width="200px" HeaderText="Payroll Name">
                        <HeaderStyle Width="200px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="ItemCode" HeaderStyle-Width="80px" HeaderText="ItemCode">
                        <HeaderStyle Width="80px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="ItemName" HeaderStyle-Width="180px" HeaderText="ItemName">
                        <HeaderStyle Width="180px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="LastEffectiveDate"  dataformatstring="{0:dd MMM yyyy}" htmlencode="true" HeaderStyle-Width="80px" HeaderText="Last Effective" >
                        <HeaderStyle Width="80px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Currency"  HeaderStyle-Width="60px" HeaderText="Currency" />                        
                        <asp:BoundField DataField="FormulaName"  HeaderStyle-Width="160px" HeaderText="Formula" />                        
                        <asp:BoundField DataField="CurrentAmount" HeaderText="Current Amount" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign = "Right" HeaderStyle-HorizontalAlign = "Right"/>
                        <asp:BoundField DataField="PercAmount" HeaderText="% Adjust" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign = "Right" HeaderStyle-HorizontalAlign = "Right"/>
                        <asp:BoundField DataField="AdjustAmount" HeaderText="Adjust Amount" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign = "Right" HeaderStyle-HorizontalAlign = "Right"/>
                        <asp:BoundField DataField="NewAmount" HeaderText="New Amount" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign = "Right" HeaderStyle-HorizontalAlign = "Right"/>
                        <asp:BoundField DataField="Remark"  HeaderStyle-Width="260px" HeaderText="Remark" />                        
                    </Columns>
                </asp:GridView>
          </div>   
          <asp:Button class="bitbtndt btnadd" runat="server" ID="btnAddDt2" Text="Add" ValidationGroup="Input" />       
       </asp:Panel>             
       <asp:Panel runat="server" ID="pnlEditDt" Visible="false">
            <table>
            <tr>
                    <td><asp:LinkButton ID="lbPayroll" runat="server" Text="Payroll" /></td>
                    <td>:</td>                    
                    <td><asp:TextBox ID="tbPayroll" runat="server" CssClass="TextBox" Width="100px" 
                            AutoPostBack="True" MaxLength = "5"/>
                        <asp:TextBox ID="tbPayrollName" runat="server" CssClass="TextBoxR" Width="250px"/>
                        <asp:TextBox ID="tbType" runat="server" Visible="false"/>
                        <asp:Button class="btngo" runat="server" ID="BtnPayroll" Text="..." />
                        <asp:Label ID="Label4" runat="server" ForeColor="#FF3300" Text="*"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>Item</td>
                    <td>:</td>
                    <td><asp:TextBox ID="tbItemCode" runat="server" CssClass="TextBox" Width="62px" AutoPostBack="true" MaxLength="15" />
                        <asp:TextBox ID="tbItemName" runat="server" CssClass="TextBoxR" Width="250px"/>
                        <asp:Button class="btngo" runat="server" ID="btnItem" Text="..." />
                        <asp:Label ID="Label5" runat="server" ForeColor="#FF3300" Text="*"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>Last Effective</td>
                    <td>:</td>
                    <td>
                        <BDP:BasicDatePicker ID="tbLastEffective" runat="server" DateFormat="dd MMM yyyy" 
                        ReadOnly = "true" 
                        ButtonImageHeight="19px" ButtonImageWidth="20px" 
                        DisplayType="TextBoxAndImage" 
                        TextBoxStyle-CssClass="TextDate" 
                    ShowNoneButton="False"><TextBoxStyle CssClass="TextDate" /></BDP:BasicDatePicker>                
                    </td>
                </tr>
                <tr>
                <td>Currency</td>
                <td>:</td>
                <td><asp:DropDownList runat="server" 
                        CssClass="DropDownList" ID="ddlCurrency" Width="69px" 
                        Height="16px"></asp:DropDownList>   
                    <asp:Label ID="Label6" runat="server" ForeColor="#FF3300" Text="*"></asp:Label>                 
                </td>
                </tr>
                <tr>
                <td>Formula</td>
                <td>:</td>
                <td><asp:DropDownList runat="server" 
                        CssClass="DropDownList" ID="ddlFormula" Width="180px"  
                        Height="16px"></asp:DropDownList>   
                    <asp:Label ID="Label2" runat="server" ForeColor="#FF3300" Text="*"></asp:Label>                 
                </td>
                </tr>
                <tr>
                    <td>Amount Current</td>
                    <td>:</td>
                    <td><asp:TextBox ID="tbCurrentAmount" runat="server" CssClass="TextBoxR"/>
                    </td>
                </tr>                
                <tr>
                    <td>% Adjust</td>
                    <td>:</td>
                    <td><asp:TextBox ID="tbAdjustPercent" runat="server" CssClass="TextBox" />
                    </td>
                </tr>
                <tr>
                    <td>Amount Adjust</td>
                    <td>:</td>
                    <td><asp:TextBox ID="tbAdjustAmount" runat="server" CssClass="TextBox" />
                    </td>
                </tr>
                <tr>
                    <td>Amount New</td>
                    <td>:</td>
                    <td><asp:TextBox ID="tbNewAmount" runat="server" CssClass="TextBox" />                        
                    </td>
                </tr>           
                <tr>
                    <td>Remark</td>
                    <td>:</td>
                    <td><asp:TextBox ID="tbRemarkDt" runat="server" CssClass="TextBoxMulti" TextMode="MultiLine" Width="350px" MaxLength="255"/>                        
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
       <asp:Button class="bitbtn btnsave" runat="server" ID="btnSaveTrans" Text="Save" ValidationGroup="Input"/>                              
       <asp:Button class="bitbtn btnback" runat="server" ID="btnBack" Text="Cancel" ValidationGroup="Input"/>                              
       <asp:Button class="btngo" runat="server" ID="btnHome" Text="Home" Width="45px"/>                              
    </asp:Panel>
    <asp:Panel runat="server" ID="pnlPrint" Visible="false">
    <cr:crystalreportviewer ID="CrystalReportViewer1" runat="server" AutoDataBind="True" Height="1036px" Width="928px" />
    </asp:Panel>
      <br />            
    </div>
    <asp:Label runat ="server" ID="lbStatus" ForeColor="Red"/>
    </form>
    </body>
</html>
