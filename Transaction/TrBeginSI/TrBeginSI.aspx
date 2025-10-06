<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TrBeginSI.aspx.vb" Inherits="Transaction_TrBeginSI_TrBeginSI" %>

<%@ Register Assembly="BasicFrame.WebControls.BasicDatePicker" Namespace="BasicFrame.WebControls"
    TagPrefix="BDP" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Supplier Begin</title>
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
            var _tempbaseforex = document.getElementById("tbBaseForex").value.replace(/\$|\,/g,"");
            var _tempppn = document.getElementById("tbPPN").value.replace(/\$|\,/g,"");
            var _tempppnforex = document.getElementById("tbPPNForex").value.replace(/\$|\,/g,"");
            var _temptotalforex = document.getElementById("tbTotalForex").value.replace(/\$|\,/g,"");
            var _tempRate = parseFloat(document.getElementById("tbRate").value.replace(/\$|\,/g,""));
            var _tempPPnRate = parseFloat(document.getElementById("tbPPNRate").value.replace(/\$|\,/g,""));
            if(isNaN(_tempbaseforex) == true)
            {
               _tempbaseforex = 0;
            }            
            if(isNaN(_tempppn) == true)
            {
               _tempppn = 0;
           }     
                        
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
    
</head>
<body>
    <form id="form1" runat="server">
    <div class="Content">
    <div class="H1">SUPPLIER BEGIN</div>
     <hr style="color:Blue" />        
    <asp:Panel runat="server" id="PnlHd"> 
      <table>
        <tr>
            <td style="width:100px;text-align:right">Quick Search :</td>            
            <td><asp:TextBox CssClass="TextBox" runat="server" ID ="tbFilter"/> 
                  <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlField" >
                      <asp:ListItem Value="InvoiceNo" Selected="True">Invoice No</asp:ListItem>
                      <asp:ListItem Value="InvoiceDate">Invoice Date</asp:ListItem>
                      <asp:ListItem Value="Status">Status</asp:ListItem>                      
                      <asp:ListItem Value="FgReport">Report</asp:ListItem>
                      <asp:ListItem Value="SuppCode">Supplier Code</asp:ListItem>
                      <asp:ListItem Value="SuppName">Supplier Name</asp:ListItem>
                      <asp:ListItem Value="Terms">Terms</asp:ListItem>
                      <asp:ListItem Value="DueDate">Due Date</asp:ListItem>
                      <asp:ListItem Value="Currency">Currency</asp:ListItem>
                      <asp:ListItem Value="CostCtrName">Cost Center</asp:ListItem>
                      <asp:ListItem Value="Remark">Remark</asp:ListItem>
                      <asp:ListItem Value="PPNNo">PPN No</asp:ListItem>
                    </asp:DropDownList>
                   
                  <asp:Button class="bitbtn btnsearch" runat="server" ID="btnSearch" Text="Search" />																						 											  
                  <asp:Button class="bitbtn btngo" runat="server" ID="btnExpand" Text="..." />																						 											                     
                    

                    
            </td>
            <td>
                <asp:LinkButton ID="lkbAdvanceSearch" runat="server" Text="Advanced Search" />
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
                      <asp:ListItem Value="InvoiceNo" Selected="True">Invoice No</asp:ListItem>
                      <asp:ListItem Value="InvoiceDate">Invoice Date</asp:ListItem>
                      <asp:ListItem Value="Status">Status</asp:ListItem>                      
                      <asp:ListItem Value="FgReport">Report</asp:ListItem>
                      <asp:ListItem Value="SuppCode">Supplier Code</asp:ListItem>
                      <asp:ListItem Value="SuppName">Supplier Name</asp:ListItem>
                      <asp:ListItem Value="Terms">Terms</asp:ListItem>
                      <asp:ListItem Value="DueDate">Due Date</asp:ListItem>
                      <asp:ListItem Value="Currency">Currency</asp:ListItem>
                      <asp:ListItem Value="CostCtrName">Cost Center</asp:ListItem>                     
                      <asp:ListItem Value="Remark">Remark</asp:ListItem>
                      <asp:ListItem Value="PPNNo">PPN No</asp:ListItem>
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
      <asp:GridView ID="GridView1" runat="server" AllowPaging="True" AllowSorting="true" 
           AutoGenerateColumns="false" CssClass="Grid"> 
              <HeaderStyle CssClass="GridHeader" ></HeaderStyle>
						<RowStyle CssClass="GridItem" Wrap="false"/>
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
                          <asp:DropDownList CssClass="DropDownList" ID="ddl" runat="server">
                              <asp:ListItem Selected="True" Text="View" />
                              <asp:ListItem Text="Edit" />
                              <asp:ListItem Text="Delete" />
                              <%--<asp:ListItem Text="Print" />--%>
                          </asp:DropDownList>
                          <asp:Button class="btngo" runat="server" ID="BtnGoDt" Text="G" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" CommandName="Go"  />   
                      </ItemTemplate>                       
                  </asp:TemplateField>
                  
                  <asp:BoundField HeaderText="Invoice No" DataField="InvoiceNo" HeaderStyle-Width="320px" SortExpression="InvoiceNo" /> 
                  <asp:BoundField HeaderText="Status" DataField="Status" HeaderStyle-Width="10px" SortExpression="Status" /> 
                  <asp:BoundField HeaderText="Invoice Date" DataField="Invoice_Date" HeaderStyle-Width="80px" SortExpression="InvoiceDate" /> 
				  <asp:BoundField HeaderText="Report" DataField="FgReport" HeaderStyle-Width="10px" SortExpression="FgReport" />   
				  <asp:BoundField HeaderText="Supplier" DataField="Supplier" HeaderStyle-Width="250px" SortExpression="Supplier" /> 
				  <asp:BoundField HeaderText="Due Date" DataField="Due_Date" HeaderStyle-Width="80px" SortExpression="DueDate" />   
				  <asp:BoundField HeaderText="Cost Center" DataField="CostCtrName" HeaderStyle-Width="80px" SortExpression="CostCtrName" />   
				  <asp:BoundField HeaderText="Currency" DataField="Currency" HeaderStyle-Width="15px" SortExpression="Currency" />   
				  <asp:BoundField HeaderText="Base Forex" DataField="BaseForex" HeaderStyle-Width="80px" SortExpression="BaseForex" ItemStyle-HorizontalAlign ="Right"   />   
				  <asp:BoundField HeaderText="PPn" DataField="PPn" HeaderStyle-Width="50px" SortExpression="PPn" ItemStyle-HorizontalAlign ="Right" />   
				  <asp:BoundField HeaderText="PPn Forex" DataField="PPnForex" HeaderStyle-Width="80px" SortExpression="PPnForex" ItemStyle-HorizontalAlign ="Right" />   
				  <asp:BoundField HeaderText="Total Forex" DataField="TotalForex" HeaderStyle-Width="80px" SortExpression="TotalForex" ItemStyle-HorizontalAlign ="Right" />     				  
				  <asp:BoundField HeaderText="Remark" DataField="Remark" HeaderStyle-Width="200px" SortExpression="Remark" ItemStyle-HorizontalAlign ="Right" />     
									
                <%--<asp:TemplateField HeaderText="Invoice No" HeaderStyle-Width="120px" SortExpression="InvoiceNo">	
                      <Itemtemplate>
							<asp:Label Runat="server" ID="LbPrimaryKey" text='<%# DataBinder.Eval(Container.DataItem, "InvoiceNo") %>'>
							</asp:Label>
						</Itemtemplate>
				</asp:TemplateField>
				<asp:TemplateField HeaderText="Status" HeaderStyle-Width="10px" SortExpression="Status">
						<Itemtemplate>
							<asp:Label Runat="server" ID="lbStatus" text='<%# DataBinder.Eval(Container.DataItem, "Status") %>'>
							</asp:Label>
						</Itemtemplate>                    
				</asp:TemplateField>
				<asp:TemplateField HeaderText="Invoice Date" HeaderStyle-HorizontalAlign="Center"  HeaderStyle-Width="80px" SortExpression="TransDate">
						<Itemtemplate>
							<asp:Label Runat="server" ID="lbTransDate" text='<%# DataBinder.Eval(Container.DataItem, "TransDate") %>'>
							</asp:Label>
						</Itemtemplate>                    
				</asp:TemplateField>
				<asp:TemplateField HeaderText="Report" HeaderStyle-Width="10px" SortExpression="FgReport">
						<Itemtemplate>
							<asp:Label Runat="server" ID="lbFgReport" text='<%# DataBinder.Eval(Container.DataItem, "FgReport") %>'>
							</asp:Label>
						</Itemtemplate>                    
				</asp:TemplateField>
				<asp:TemplateField HeaderText="Supplier" HeaderStyle-Width="250px" SortExpression="SuppCode">
						<Itemtemplate>
							<asp:Label Runat="server" ID="lbSupplier" text='<%# DataBinder.Eval(Container.DataItem, "SuppCode")+" "+DataBinder.Eval(Container.DataItem, "SuppName")%>'>
							</asp:Label>
						</Itemtemplate>                    
				</asp:TemplateField>
				<asp:TemplateField HeaderText="Due Date" HeaderStyle-Width="15px" SortExpression="DueDate">
						<Itemtemplate>
							<asp:Label Runat="server" ID="lbDueDate" text='<%# DataBinder.Eval(Container.DataItem, "DueDate") %>'>
							</asp:Label>
						</Itemtemplate>                    
				</asp:TemplateField>
				<asp:TemplateField HeaderText="Currency" HeaderStyle-Width="15px" SortExpression="CurrCode">
						<Itemtemplate>
							<asp:Label Runat="server" ID="lbCurrCode" text='<%# DataBinder.Eval(Container.DataItem, "CurrCode") %>'>
							</asp:Label>
						</Itemtemplate>                    
				</asp:TemplateField>
				<asp:TemplateField HeaderText="Base Forex" HeaderStyle-Width="100px" SortExpression="BaseForex">
						<Itemtemplate>
							<asp:Label Runat="server" ID="lbBaseForex" text='<%# DataBinder.Eval(Container.DataItem, "BaseForex") %>'>
							</asp:Label>
						</Itemtemplate>                    
				</asp:TemplateField>
				<asp:TemplateField HeaderText="PPn Forex" HeaderStyle-Width="100px" SortExpression="PPnForex">
						<Itemtemplate>
							<asp:Label Runat="server" ID="lbPPnForex" text='<%# DataBinder.Eval(Container.DataItem, "PPnForex") %>'>
							</asp:Label>
						</Itemtemplate>                    
				</asp:TemplateField>
				<asp:TemplateField HeaderText="Total Forex" HeaderStyle-Width="100px" SortExpression="TotalForex">
						<Itemtemplate>
							<asp:Label Runat="server" ID="lbTotalForex" text='<%# DataBinder.Eval(Container.DataItem, "TotalForex") %>'>
							</asp:Label>
						</Itemtemplate>                    
				</asp:TemplateField>--%>
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
                <td>Invoice No</td>
                <td>:</td>
                <td><asp:TextBox ID="tbInvoiceNo" MaxLength="20" ValidationGroup="Input" Width = "225px" runat="server" CssClass ="TextBox" />
                    
                    
                </td>
                
                <td>Invoice Date</td>
                <td>:</td>
                <td>
                    <BDP:BasicDatePicker ID="tbTransDate" runat="server" DateFormat="dd MMM yyyy" 
                        ReadOnly = "true" ValidationGroup="Input" ShowNoneButton = "True"
                        ButtonImageHeight="19px" ButtonImageWidth="20px" 
                        DisplayType="TextBoxAndImage" 
                        TextBoxStyle-CssClass="TextDate" AutoPostBack="True"><TextBoxStyle Width = "225px"  CssClass="TextDate" /></BDP:BasicDatePicker>
                </td>
            </tr>
            <tr>
                <td>Type</td>
                <td>:</td>
                <td><asp:DropDownList ID ="ddlType" ValidationGroup="Input" runat ="server" CssClass="DropDownList" Width="230px">                                    
                        <asp:ListItem Selected="True">SI</asp:ListItem>
                        <asp:ListItem>DN</asp:ListItem>
                        <asp:ListItem>CN</asp:ListItem>
                    </asp:DropDownList> 
                </td>                 
                <%--<td>Report</td>
                <td>:</td>
                <td><asp:DropDownList AutoPostBack="true" ValidationGroup="Input" CssClass="DropDownList" ID="ddlReport" runat="server" >
                        <asp:ListItem Selected="True">Y</asp:ListItem>
                        <asp:ListItem>N</asp:ListItem>
                    </asp:DropDownList> 
                </td>--%>                 
            </tr>                            
              
             <tr>
                <td>Supplier</td>
                <td>:</td>
                <td colspan="4">
                <asp:TextBox ID="tbSuppCode" ValidationGroup="Input" AutoPostBack="true" Width="100" runat="server" CssClass="TextBox" />
                <asp:TextBox ID="tbSuppName" Width="175" runat="server" 
                        ReadOnly="true" CssClass="TextBoxR" />
                <asp:Button Class="btngo" ID="btnSupp" Text="..." runat="server" ValidationGroup="Input" />                                          
                
                <%--<asp:Button ID="btntemp" CssClass="ButtonR" OnClientClick="return true;" runat="server" />               --%>
                
                </td>                
             </tr>
             <tr>
                <td>Term</td>
                <td>:</td>
                <td><asp:TextBox ID="tbTerm" ValidationGroup="Input" AutoPostBack="true" runat="server" CssClass="TextBox" Width="190px" />
                    &nbsp days
                </td>
                <td>Due Date</td>    
                <td>:</td>    
                <td>
                    <BDP:BasicDatePicker ID="tbDueDate" ValidationGroup="Input" runat="server" DateFormat="dd MMM yyyy" ShowNoneButton = "false" ButtonImageHeight="19px" ButtonImageWidth="20px" 
                       DisplayType="TextBoxAndImage" ReadOnly="true" TextBoxStyle-CssClass="TextDate" > <TextBoxStyle Width = "225px"  CssClass="TextDate" /></BDP:BasicDatePicker>
                 </td>                
             </tr>
             
             <tr>
                <td>
                    <asp:LinkButton ID="lbCurr" runat="server" Text="Currency" />
                 </td>
                <td>:</td>
                <td>
                    <asp:DropDownList ID="ddlCurr" runat="server" AutoPostBack="true" 
                        CssClass="DropDownList" ValidationGroup="Input" Width="80px" />
                    <asp:TextBox ID="tbRate" runat="server" CssClass="TextBox" 
                        ValidationGroup="Input" Width="80px" />
                </td>                
                 <td>
                     Supp PO No</td>
                 <td>
                     :</td>
                 <td>
                     <asp:TextBox ID="tbSuppPONo" runat="server" Width = "225" CssClass="TextBox" 
                         ValidationGroup="Input" />
                 </td>
             </tr>     
             <tr>
                <td>Cost Center</td>
                <td>:</td>
                <td colspan="8">
                    <asp:DropDownList ID="ddlCostCtr" runat="server" CssClass="DropDownList" 
                        ValidationGroup="Input" Width="230px">
                    </asp:DropDownList>
                </td>
          
             </tr>   
             <tr>
                <td>PPN</td>
                <td>:</td>
                <td colspan="4">
                    <table>
                        <tr style="background-color:Silver;text-align:center">
                            <td>No</td>
                            <td>Date</td>
                            <td>Rate</td>
                        </tr>
                        <tr>
                            <td><asp:TextBox ID="tbPPNNo" ValidationGroup="Input" runat="server" 
                                    CssClass="TextBox" /></td>
                            <td>
                                <BDP:BasicDatePicker ID="tbPPNDate" runat="server" ButtonImageHeight="19px" 
                                    ButtonImageWidth="20px" DateFormat="dd MMM yyyy" DisplayType="TextBoxAndImage" 
                                    ReadOnly="true" TextBoxStyle-CssClass="TextDate" ValidationGroup="Input" 
                                    AutoPostBack="True">
                                    <TextBoxStyle CssClass="TextDate" />
                                </BDP:BasicDatePicker>
                            </td>
                            <td><asp:TextBox ID="tbPPNRate" runat="server" CssClass="TextBox" 
                                    ValidationGroup="Input" Width="80px"/></td>
                        </tr>
                    </table>
                </td>                
             </tr>
             
             <tr>
                <td>Amount</td>
                <td>:</td>
                <td colspan="4">
                    <table>
                        <tr style="background-color:Silver;text-align:center">
                            <td>
                                Base Forex</td>
                            <td>
                                PPN %</td>
                            <td>
                                PPN Forex</td>
                            <td>
                                Total Forex</td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="tbBaseForex" runat="server" CssClass="TextBox" 
                                    ValidationGroup="Input" AutoPostBack="True" />
                            </td>
                            <td>
                                <asp:TextBox ID="tbPPN" runat="server" CssClass="TextBox" 
                                    ValidationGroup="Input" Width="50px" AutoPostBack="True" />
                            </td>
                            <td>
                                <asp:TextBox ID="tbPPNForex" runat="server" CssClass="TextBoxR" />
                            </td>
                            <td>
                                <asp:TextBox ID="tbTotalForex" runat="server" CssClass="TextBoxR" />
                            </td>
                        </tr>
                    </table>
                 </td>
             </tr>  
             <tr>
                <td>
        			Remark</td>
                 <td>
                     :</td>
                 <td colspan="2">
                     <asp:TextBox ID="tbRemark" runat="server" CssClass="TextBoxMulti" 
                         MaxLength="255" TextMode="MultiLine" ValidationGroup="Input" Width="358px" />
                 </td>
             </tr>
            <tr>
                <td colspan="6">
                    <asp:Button ID="btnSaveNew" runat="server" class="bitbtndt btnsavenew" 
                        Text="Save &amp; New" validationgroup="Input" Width="90px" />
                    <asp:Button ID="btnSave" runat="server" class="bitbtndt btnsave" Text="Save" 
                        validationgroup="Input" />
                    <asp:Button ID="btnCancel" runat="server" class="bitbtndt btncancel" 
                        Text="Cancel" validationgroup="Input" />
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
