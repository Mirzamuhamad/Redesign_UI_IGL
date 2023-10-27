<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TrIO.aspx.vb" Inherits="Transaction_TrIO_TrIO" %>

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
       function setformat()
        {
        try
         {         
         var TrainingCost = document.getElementById("tbTrainingCost").value.replace(/\$|\,/g,"");                           
         document.getElementById("tbTrainingCost").value = setdigit(TrainingCost,'<%=Viewstate("DigitCurr")%>');
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
    <div class="H1">Internal Order</div>
     <hr style="color:Blue" />        
    <asp:Panel runat="server" id="PnlHd"> 
      <table>
        <tr>
            <td style="width:100px;text-align:right">Quick Search :</td>         
            <td><asp:TextBox CssClass="TextBox" runat="server" ID ="tbFilter"/> 
                  <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlField" >     
                    <asp:ListItem Selected="True" Value="TransNmbr">IO No</asp:ListItem>
                    <asp:ListItem Value="dbo.FormatDate(TransDate)">IO Date</asp:ListItem>
                    <asp:ListItem>Status</asp:ListItem>
                    <%--<asp:ListItem Value="PeriodCode">Period</asp:ListItem>--%>
                    <asp:ListItem Value="dbo.FormatDate(StartDate)">Start Date</asp:ListItem>
                    <asp:ListItem Value="dbo.FormatDate(EndDate)">End Date</asp:ListItem>
                    <asp:ListItem Value="RequestBy">Request By Code</asp:ListItem>
                    <asp:ListItem Value="RequestByName">Request By Name</asp:ListItem>
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
                    <asp:ListItem Selected="True" Value="TransNmbr">IO No</asp:ListItem>
                    <asp:ListItem Value="dbo.FormatDate(TransDate)">IO Date</asp:ListItem>
                    <asp:ListItem>Status</asp:ListItem>
                    <%--<asp:ListItem Value="PeriodCode">Period</asp:ListItem>--%>
                    <asp:ListItem Value="dbo.FormatDate(StartDate)">Start Date</asp:ListItem>
                    <asp:ListItem Value="dbo.FormatDate(EndDate)">End Date</asp:ListItem>
                    <asp:ListItem Value="RequestBy">Request By Code</asp:ListItem>
                    <asp:ListItem Value="RequestByName">Request By Name</asp:ListItem>
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
            <asp:Button class="btngo" runat="server" ID="BtnGo" Text="G" 
            Visible="False"/>
          <br />
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
                          <asp:CheckBox ID="cbSelectHd" runat="server" AutoPostBack="true" oncheckedchanged="cbSelectHd_CheckedChanged" />
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
                  <asp:BoundField DataField="TransNmbr" HeaderStyle-Width="120px" SortExpression="Nmbr" HeaderText="Evaluation No"></asp:BoundField>                  
                  <asp:BoundField DataField="Status" SortExpression="Status" HeaderText="Status"></asp:BoundField>                  
                  <asp:BoundField DataField="TransDate" dataformatstring="{0:dd MMM yyyy}" htmlencode="true" HeaderStyle-Width="80px" SortExpression="TransDate" HeaderText="Evaluation Date"></asp:BoundField>
                  <%--<asp:BoundField DataField="PeriodCode" HeaderStyle-Width="120px" SortExpression="PeriodCode" HeaderText="Period"></asp:BoundField>--%>
                  <asp:BoundField DataField="StartDate" dataformatstring="{0:dd MMM yyyy}" htmlencode="true" HeaderStyle-Width="80px" SortExpression="StartDate" HeaderText="Start Date"></asp:BoundField>
                  <asp:BoundField DataField="EndDate" dataformatstring="{0:dd MMM yyyy}" htmlencode="true" HeaderStyle-Width="80px" SortExpression="EndDate" HeaderText="End Date"></asp:BoundField>
                  <asp:BoundField DataField="RequestBy" HeaderStyle-Width="120px" SortExpression="RequestBy" HeaderText="Request By Code"></asp:BoundField>
                  <asp:BoundField DataField="RequestByName" HeaderStyle-Width="200px" SortExpression="RequestByName" HeaderText="Request By Name"></asp:BoundField>
                  <asp:BoundField DataField="Remark" HeaderStyle-Width="250px" SortExpression="Remark" HeaderText="Remark"></asp:BoundField>            
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
      <table style="width: 677px">
        <tr>
            <td>IO No</td>
            <td>:</td>
            <td><asp:TextBox CssClass="TextBoxR" runat="server" ID="tbCode" Width="150px" Enabled="False"/></td>
            
            <td>IO Date</td>
            <td>:</td>
            <td><BDP:BasicDatePicker ID="tbDate" runat="server" DateFormat="dd MMM yyyy" 
                        ReadOnly = "true" ValidationGroup="Input"
                        ButtonImageHeight="19px" ButtonImageWidth="20px" 
                        DisplayType="TextBoxAndImage" 
                        TextBoxStyle-CssClass="TextDate"                         
                        ShowNoneButton="False"><TextBoxStyle CssClass="TextDate" /></BDP:BasicDatePicker>                
            </td>            
        </tr>         
       <%-- <tr>
            <td>Period</td>
            <td>:</td>
            <td><asp:TextBox ID="tbPeriod" runat="server" CssClass="TextBoxR" MaxLength="8" 
                    ValidationGroup="Input" Width="78px" />
                <asp:Button ID="btnPeriod" runat="server" class="btngo" Text="..." ValidationGroup="Input" />
            </td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
        </tr>  --%>      
        <tr>
            <td>Start Date</td>
            <td>:</td>
            <td>
                <BDP:BasicDatePicker ID="tbStartDate" runat="server" ButtonImageHeight="19px" 
                    ButtonImageWidth="20px" DateFormat="dd MMM yyyy" DisplayType="TextBoxAndImage" 
                    ReadOnly="true" ShowNoneButton="False" AutoPostBack="true"
                    TextBoxStyle-CssClass="TextDate"><TextBoxStyle CssClass="TextDate" /></BDP:BasicDatePicker>
                &nbsp; -&nbsp;
                <BDP:BasicDatePicker ID="tbEndDate" runat="server" ButtonImageHeight="19px" 
                    ButtonImageWidth="20px" DateFormat="dd MMM yyyy" DisplayType="TextBoxAndImage" 
                    ReadOnly="true" ShowNoneButton="False" AutoPostBack="true"
                    TextBoxStyle-CssClass="TextDate"><TextBoxStyle CssClass="TextDate" /></BDP:BasicDatePicker>
            </td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
         </tr>
        <tr>
            <td>Request By</td>
            <td>:</td>
            <td colspan="4">
                <asp:TextBox ID="tbRequestBy" runat="server" CssClass="TextBox" MaxLength="60" ValidationGroup="Input" Width="101px" AutoPostBack = "true" />
                <asp:TextBox ID="tbRequestByName" runat="server" CssClass="TextBoxR" MaxLength="100" ValidationGroup="Input" Width="389px" />
                <asp:Button ID="btnRequestBy" runat="server" class="btngo" Text="..." ValidationGroup="Input" />
            </td>
        </tr>
          <tr>
              <td>Remark</td>
              <td>:</td>
              <td style="margin-left: 80px">
                  <asp:TextBox ID="tbRemarkHd" runat="server" CssClass="TextBox" MaxLength="60" ValidationGroup="Input" Width="359px" />
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
                        ShowFooter="True">
                        <HeaderStyle CssClass="GridHeader" />
                        <RowStyle CssClass="GridItem" Wrap="false" />
                        <AlternatingRowStyle CssClass="GridAltItem" />
                        <PagerStyle CssClass="GridPager" />
                        <Columns>
                            <asp:TemplateField HeaderText="Action">
                                      <ItemTemplate>
                                        <asp:Button ID="btnEdit" runat="server" class="bitbtndt btnedit" Text="Edit" CommandName="Edit"/>
                                        <asp:Button ID="btnDelete" runat="server" class="bitbtndt btndelete" Text="Delete" OnClientClick="return confirm('Sure to delete this data?');" CommandName="Delete"/>						                                      
                                      </ItemTemplate>
                            </asp:TemplateField>                            
                            <asp:TemplateField>
                                <ItemTemplate>
                                  <asp:Button ID="btnClosing" runat="server" class="bitbtn btnclear" Text="Closing" CommandName="Closing" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" />     
                                </ItemTemplate>
                            </asp:TemplateField>                         
                            <asp:BoundField DataField="ProductCode" HeaderText="Product Code" />   
                            <asp:BoundField DataField="Product_Name" HeaderText="Product Name" />
                            <asp:BoundField DataField="Specification" HeaderText="Specification" />
                            <asp:BoundField DataField="Qty" DataFormatString="{0:#,##0.00}" ItemStyle-HorizontalAlign="Right" HeaderText="Qty" />
                            <asp:BoundField DataField="Unit" HeaderText="Unit" />
                            <asp:BoundField DataField="Remark" HeaderText="Remark" />    
                            <asp:BoundField DataField="UserClose" HeaderText="User Close" />
                            <asp:BoundField DataField="DateClose" dataformatstring="{0:dd MMM yyyy}" htmlencode="true" HeaderText="Date Close" />
                            <asp:BoundField DataField="QtyClose" DataFormatString="{0:#,##0.00}" ItemStyle-HorizontalAlign="Right"  HeaderText="Qty Close" />    
                            <asp:BoundField DataField="RemarkClose" HeaderText="Remark Close" />
                            <asp:BoundField DataField="DoneClosing" HeaderText="Done Close" />
                        </Columns>
                    </asp:GridView>
              </div>   
              <asp:Button class="bitbtndt btnadd" runat="server" ID="btnAddDtke2" Text="Add" ValidationGroup="Input" />
              </asp:Panel> 
              <asp:Panel runat="server" ID="pnlEditDt" Visible="false">
                <table style="width: 681px">    
                    <tr>
                        <td>Product</td>
                        <td>:</td>
                        <td><asp:TextBox CssClass="TextBox" ValidationGroup="Input" runat="server" ID="tbProductCode" MaxLength="20" Width="125px" AutoPostBack="True" />
                            <asp:TextBox CssClass="TextBoxR" ValidationGroup="Input" runat="server" ID="tbProductName" MaxLength="60" Width="318px" />
                            <asp:Button class="btngo" runat="server" ID="btnProduct" Text="..."/>
                        </td>
                    </tr>   
                    <tr>
                        <td>Specification</td>
                        <td>:</td>
                        <td>
                            <asp:TextBox ID="tbSpecification" runat="server" CssClass="TextBox" 
                                Enabled="False" Height="42px" MaxLength="255" TextMode="MultiLine" 
                                ValidationGroup="Input" Width="456px" />
                        </td>
                    </tr>       
                    <tr>
                        <td>Qty</td>
                        <td>:</td>
                        <td>
                            <asp:TextBox ID="tbQty" runat="server" CssClass="TextBox" 
                                ValidationGroup="Input" Width="55px" />
                        </td>
                    </tr>       
                    <tr>
                        <td>Unit</td>
                        <td>:</td>
                        <td>
                            <asp:DropDownList ID="ddlUnit" runat="server" CssClass="DropDownList" 
                                Enabled="false" ValidationGroup="Input">
                            </asp:DropDownList>
                        </td>
                    </tr>   
                    <tr>
                        <td>
                            Remark</td>
                        <td>
                            :</td>
                        <td>
                            <asp:TextBox ID="tbRemarkDt" runat="server" CssClass="TextBox" MaxLength="60" 
                                ValidationGroup="Input" Width="456px" />
                        </td>
                    </tr>
                </table>
                <br />           
                <asp:Button ID="btnSaveDt" runat="server" class="bitbtndt btnsave" Text="Save"/> &nbsp;         
                <asp:Button ID="btnCancelDt" runat="server" class="bitbtndt btncancel" Text="Cancel"/> 
           </asp:Panel> 
       <br />      
        <asp:Button ID="btnSaveAll" runat="server" class="bitbtndt btnsave" Text="Save & New" ValidationGroup="Input" Width="97px"/> 
        &nbsp;    
        <asp:Button ID="btnSaveTrans" runat="server" class="bitbtndt btnsave" Text="Save" ValidationGroup="Input"/> 
        &nbsp;    
        <asp:Button ID="btnBack" runat="server" class="bitbtndt btncancel" Text="Cancel" ValidationGroup="Input"/>  
        &nbsp;
        <asp:Button ID="btnHome" runat="server" class="btngo" Text="Home" Width="48px"/>    
    </asp:Panel>        
    </div>
   
    <asp:Label runat ="server" ID="lbStatus" ForeColor="Red"/>
    <asp:HiddenField ID="HiddenRemarkClose" runat="server" />
    </form>
</body>
</html>
