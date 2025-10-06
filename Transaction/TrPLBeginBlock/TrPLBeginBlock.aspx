<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TrPLBeginBlock.aspx.vb" Inherits="Transaction_TrPLBeginBlock_TrPLBeginBlock" %>
<%@ Register Assembly="FastReport" Namespace="FastReport.Web" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="BasicFrame.WebControls.BasicDatePicker" Namespace="BasicFrame.WebControls"
    TagPrefix="BDP" %>
<%@ Register assembly="CrystalDecisions.Web, Version=10.5.3700.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Beginning Block</title>
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
        document.getElementById("tbAmount").value = setdigit(document.getElementById("tbAmount").value.replace(/\$|\,/g,""),'<%=ViewState("DigitQty")%>');
        document.getElementById("tbTAmount").value = setdigit(document.getElementById("tbTAmount").value.replace(/\$|\,/g, ""), '<%=ViewState("DigitQty")%>');
        document.getElementById("tbTDepr").value = setdigit(document.getElementById("tbTDepr").value.replace(/\$|\,/g, ""), '<%=ViewState("DigitQty")%>');
        document.getElementById("tbTBalance").value = setdigit(document.getElementById("tbTBalance").value.replace(/\$|\,/g, ""), '<%=ViewState("DigitQty")%>');        
        
        
    } catch (err) {
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
           function deletetrans()
        {
            try
            {
                
                 var result = confirm("Sure Delete Transaction ?");
                if (result){
                    document.getElementById("HiddenRemarkDelete").value = "true";
                } else {
                    document.getElementById("HiddenRemarkDelete").value = "false";
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
 <link href="https://fonts.googleapis.com/css2?family=Muli:wght@300;400;600&display=swap" rel="stylesheet">
    <style type="text/css">
        .style1
        {
            height: 16px;
        }
    </style>
</head>

<body>     
    <form id="form1" runat="server">
     <div class="Content">
    <div class="H1">Beginning Block</div>
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
                      <asp:ListItem Value="Block">Block</asp:ListItem>
                      <asp:ListItem Value="BlockName">Block Name</asp:ListItem>
                      <asp:ListItem Value="QtyTanam">QtyTanam</asp:ListItem>
                      <asp:ListItem Value="BatchNo">Batch No</asp:ListItem>
                      <asp:ListItem Value="VarietasName">Varietas</asp:ListItem>
                      <asp:ListItem Value="TotalNilai">Amount</asp:ListItem>
                      <asp:ListItem Value="TotalDepr">Depresiation</asp:ListItem>
                      <asp:ListItem Value="Balance">Balance</asp:ListItem>
                      <asp:ListItem Value="Remark">Remark</asp:ListItem>  
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
                  <asp:ListItem Value="Reference" Selected="True">Reference</asp:ListItem>
                  <asp:ListItem Value="dbo.FormatDate(TransDate)">Date</asp:ListItem>
                  <asp:ListItem>Status</asp:ListItem>
                   <asp:ListItem Value="Block">Block</asp:ListItem>
                      <asp:ListItem Value="BlockName">Block Name</asp:ListItem>
                      <asp:ListItem Value="QtyTanam">QtyTanam</asp:ListItem>
                      <asp:ListItem Value="BatchNo">Batch No</asp:ListItem>
                      <asp:ListItem Value="VarietasName">Varietas</asp:ListItem>
                      <asp:ListItem Value="TotalNilai">Amount</asp:ListItem>
                      <asp:ListItem Value="TotalDepr">Depresiation</asp:ListItem>
                      <asp:ListItem Value="Balance">Balance</asp:ListItem>
                      <asp:ListItem Value="Remark">Remark</asp:ListItem>                
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
                              
                          </asp:DropDownList>
                          <asp:Button class="btngo" runat="server" ID="BtnGo" Text="G" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" CommandName="Go"  />                                                 
                      </ItemTemplate>
                      <HeaderStyle Width="110px" />
                  </asp:TemplateField>                     
                  <asp:BoundField DataField="TransNmbr" SortExpression="TransNmbr" 
                      HeaderText="Reference"></asp:BoundField>                  
                  <asp:BoundField DataField="Status" HeaderText="Status" SortExpression="Status"></asp:BoundField>                  
                  <asp:BoundField DataField="TransDate" HtmlEncode="true" DataFormatString="{0:dd MMM yyyy}" HeaderText="Date" SortExpression="TransDate"></asp:BoundField>                  
                  <asp:BoundField DataField="Block" HeaderStyle-Width="120px" HeaderText="Block" 
                      SortExpression="Block">
                      <HeaderStyle Width="120px" />
                  </asp:BoundField>
                  <asp:BoundField DataField="BlockName" HeaderStyle-Width="120px" 
                      HeaderText="Block Name" SortExpression="BlockName">
                      <HeaderStyle Width="120px" />
                  </asp:BoundField>
                  <asp:BoundField DataField="QtyTanam" HtmlEncode="true" HeaderText="Qty Tanam" 
                      SortExpression="QtyTanam" DataFormatString="{0:#,##0.##}"></asp:BoundField>                  
                  <asp:BoundField DataField="BatchNo" HeaderStyle-Width="180px" 
                      HeaderText="Batch No" SortExpression="BatchNo">
                      <HeaderStyle Width="180px" />
                  </asp:BoundField>
                  <asp:BoundField DataField="VarietasName" HeaderStyle-Width="120px" 
                      HeaderText="Varietas " SortExpression="VarietasName">
                      <HeaderStyle Width="120px" />
                  </asp:BoundField>
                  <asp:BoundField DataField="TotalNilai" HeaderText="Amount" 
                      SortExpression="TotalNilai" HtmlEncode="true" 
                      DataFormatString="{0:#,##0.##}" />
                  <asp:BoundField DataField="TotalDepr" HeaderText="Depresiation" 
                      SortExpression="TotalDepr" DataFormatString="{0:#,##0.##}" />
                  <asp:BoundField DataField="Balance" HeaderText="Balance" 
                      SortExpression="Balance" DataFormatString="{0:#,##0.##}" />
                  <asp:BoundField DataField="Remark" HeaderStyle-Width="200px" HeaderText="Remark">
                      <HeaderStyle Width="200px" />
                  </asp:BoundField>
                  <asp:BoundField DataField="FgStatusTM" HeaderText="Status TM" 
                      SortExpression="FgStatusTM" />
                  <asp:BoundField DataField="StartDepr" HeaderText="Start Depr" 
                      SortExpression="StartDepr" DataFormatString="{0:dd MMM yyyy}" />
                  <asp:BoundField DataField="LifeDepr" HeaderText="Life Depr" 
                      SortExpression="LifeDepr" DataFormatString="{0:#,##0.##}" />
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
            <td>Reference</td>
            <td>:</td>
            <td><asp:TextBox CssClass="TextBox" Enabled = "false" runat="server" ID="tbRef" Width="149px"/> 
            </td>            
            <td>
                Status TM</td>
            <td>
                :</td>
            <td>
                <asp:TextBox ID="tbStatusTM" runat="server" CssClass="TextBoxR" Enabled="False" 
                    MaxLength="5" Width="60px" />
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
            <td>
                Start Depresiation</td>
            <td>
                :</td>
            <td>
                <BDP:BasicDatePicker ID="tbStartDepr" runat="server" ButtonImageHeight="19px" 
                    ButtonImageWidth="20px" DateFormat="dd MMM yyyy" DisplayType="TextBoxAndImage" 
                    Enabled="False" ReadOnly="true" ShowNoneButton="False" 
                    TextBoxStyle-CssClass="TextDate">
                    <TextBoxStyle CssClass="TextDate" />
                </BDP:BasicDatePicker>
            </td>
        </tr>      
        <%--<tr>
            <td>
            <asp:ImageButton ID="btnGetDt" ValidationGroup="Input" runat="server"  
                    ImageUrl="../../Image/btnGetDataOn.png"
                    onmouseover="this.src='../../Image/btnGetDataOff.png';"
                    onmouseout="this.src='../../Image/btnGetDataOn.png';"
                    ImageAlign="AbsBottom" />             
            </td>            
        </tr>--%>
        <tr>
              <td class="style1">Block</td>
              <td class="style1">:</td>
              <td class="style1">
                  <asp:TextBox ID="tbBlock" runat="server" CssClass="TextBox" 
                      ValidationGroup="Input" />
                  <asp:TextBox ID="tbBlockName" runat="server" CssClass="TextBoxR" Enabled="False" 
                      Width="200px" />
                  <asp:Button ID="btnBlock" runat="server" Class="btngo" Text="..." 
                      ValidationGroup="Input" />
              </td>
              <td class="style1">
                  Life Depresiation</td>
              <td class="style1">
                  :</td>
              <td class="style1">
                  <asp:TextBox ID="tblife" runat="server" CssClass="TextBoxR" Enabled="False" 
                      MaxLength="5" Width="60px" />
                  months</td>
          </tr>
          <tr>
              <td>
                  Qty Tanam</td>
              <td>
                  :</td>
              <td>
                  <asp:TextBox ID="tbQtyTanam" runat="server" CssClass="TextBox" MaxLength="5" 
                      Width="60px" ValidationGroup="Input" />
              </td>
              <td>
                  &nbsp;</td>
              <td>
                  &nbsp;</td>
              <td>
                  &nbsp;</td>
          </tr>
          <tr>
              <td>
                  Batch No / Varietas</td>
              <td>
                  :</td>
              <td >
                  <asp:TextBox ID="tbBatchNo" runat="server" CssClass="TextBox" 
                      ValidationGroup="Input" />
                  <asp:TextBox ID="tbVarietas" runat="server" CssClass="TextBoxR" Enabled="False" 
                      Width="200px" />
                  <asp:Button ID="btnBatchNo" runat="server" Class="btngo" Text="..." 
                      ValidationGroup="Input" />
              </td>
             
              <td>
                  &nbsp;</td>
              <td>
                  &nbsp;</td>
              <td>
                  &nbsp;</td>
             
          </tr>
          <tr>
              <td>
                  Total</td>
              <td>
                  &nbsp;</td>
              <td>
                  <table cellpadding="0" cellspacing="0">
                      <tr style="background-color:Silver;text-align:center">
                          <td>
                              Amount</td>
                          <td>
                              Depresiation</td>
                          <td>
                              Balance</td>
                      </tr>
                      <tr>
                          <td>
                              <asp:TextBox ID="tbTAmount" runat="server" CssClass="TextBoxR" Width="110px" 
                                  Enabled="False" />
                          </td>
                          <td>
                              <asp:TextBox ID="tbTDepr" runat="server" CssClass="TextBoxR" Width="110px" 
                                  Enabled="False" />
                          </td>
                          <td>
                              <asp:TextBox ID="tbTBalance" runat="server" CssClass="TextBoxR" Height="16px" 
                                  Width="91px" Enabled="False" />
                          </td>
                      </tr>
                  </table>
              </td>
             
              <td>
                  &nbsp;</td>
              <td>
                  &nbsp;</td>
              <td>
                  &nbsp;</td>
             
          </tr>
        <tr>
              <td>Remark</td>
              <td>:</td>
              <td>
                  <asp:TextBox ID="tbRemark" runat="server" CssClass="TextBox" 
                      ValidationGroup="Input" Width="250px" />
              </td>
              <td>
                  &nbsp;</td>
              <td>
                  &nbsp;</td>
              <td>
                  &nbsp;</td>
          </tr>
          
      </table>  
      
      <br />      
      <div style="font-size:medium; color:Blue;">Detail</div>
      <hr style="color:Blue" />  
        <asp:Panel runat="server" ID="pnlDt">
        <asp:Button class="bitbtn btnadd" runat="server" ID="btnAddDt" Text="Add" ValidationGroup="Input" />	     
                 
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
                               <asp:Button class="bitbtn btnedit" runat="server" ID="btnEdit" Text="Edit" CommandName="Edit" />																						 											
						       <asp:Button class="bitbtn btndelete" runat="server" ID="btnDelete" Text="Delete" CommandName="Delete"  OnClientClick="return confirm('Sure to delete this data?');" />																						 													     
                            </ItemTemplate>
                            <EditItemTemplate>
                            		<asp:Button class="bitbtn btnsave" runat="server" ID="btnUpdate" Text="Save" CommandName="Update" />																						 																											
									
									<asp:Button class="bitbtn btncancel" runat="server" ID="btnCancel" Text="Cancel" CommandName="Cancel" />																						 													
                                
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:Button ID="btnClosing" runat="server" CssClass="Button" Text="Closing"                                             
                                            CommandArgument='<%# Container.DataItemIndex %>'
                                            CommandName="Closing" />
                                            <%--CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>"--%>
                                </ItemTemplate>
                        </asp:TemplateField>     
                        <asp:BoundField DataField="Job" HeaderStyle-Width="100px" HeaderText="Job" SortExpression="Job" >
                            <HeaderStyle Width="100px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="JobName" HeaderText="Job Name" HeaderStyle-Width="180px" SortExpression="JobName" >
                            <HeaderStyle Width="180px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Amount" HeaderText="Amount" SortExpression="Amount" 
                            ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" 
                            DataFormatString="{0:#,##0.##}">
                            <FooterStyle HorizontalAlign="Right" />
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:BoundField>                        
                        <asp:BoundField DataField="Remark" HeaderStyle-Width="80px" HeaderText="Remark" >
                            <HeaderStyle Width="80px" />
                        </asp:BoundField>
                        
                    </Columns>
                </asp:GridView>
          </div>   
          <asp:Button class="bitbtn btnadd" runat="server" ID="btnAddDt2" Text="Add" ValidationGroup="Input" />	     
          
       </asp:Panel>             
       <asp:Panel runat="server" ID="pnlEditDt" Visible="false">
       <table width="100%">
            <tr>
                <td style="width:60%">                
                    <table>
                        <tr>
                            <td>
                                <asp:LinkButton ID="lbTKPanen" runat="server" Text="Job" />
                            </td>
                            <td>:</td>
                            <td>
                                <asp:TextBox ID="tbCode" runat="server" AutoPostBack="true" 
                                    CssClass="TextBox" ValidationGroup="Input" />
                                <asp:TextBox ID="tbName" runat="server" CssClass="TextBoxR" Enabled="False" 
                                    EnableTheming="True" ReadOnly="True" Width="200px" />
                                <asp:Button Class="btngo" ID="btnJob" Text="..." runat="server" 
                                    ValidationGroup="Input" />                  
                             
                            </td>
                        </tr>
                        <tr>
                        <td>Amount</td>
                        <td>&nbsp;</td>
                        <td>
                            <asp:TextBox ID="tbAmount" runat="server" CssClass="TextBox" 
                                ValidationGroup="Input" Width="110px" />
                        </td>
                        </tr>
                        <tr>
                            <td>
                                Remark </td>
                            <td>
                                :</td>
                            <td>
                                <asp:TextBox ID="tbRemarkDt" runat="server" CssClass="TextBox" Width="304px" />
                            </td>
                        </tr>
                    </table>
                </td>
                <td style="vertical-align:top;width:40%">
                	&nbsp;</td>
            </tr>
       </table>
            <br />
            <asp:Button class="bitbtn btnsave" runat="server" ID="btnSaveDt" Text="Save" CommandName="Update" />																						 																											
									
			<asp:Button class="bitbtn btncancel" runat="server" ID="btnCancelDt" Text="Cancel" CommandName="Cancel" />																						 													
 
            <br />
       </asp:Panel> 
       <br />    
       
       
       <asp:Button ID="btnSaveAll" runat="server" class="bitbtndt btnsavenew" Text="Save & New" validationgroup="Input" Width = "90"/>									
       <asp:Button ID="btnSaveTrans" runat="server" class="bitbtndt btnsave" Text="Save" validationgroup="Input"/>									
       <asp:Button ID="btnBack" runat="server" class="bitbtndt btncancel" Text="Cancel" validationgroup="Input"/>									
       <asp:Button ID="btnHome" runat="server" class="bitbtndt btnback" Text="Home" />									                                             
       
     
    </asp:Panel>
    <asp:Panel runat="server" ID="pnlPrint" Visible="false">
    <cr:crystalreportviewer ID="CrystalReportViewer1" runat="server" AutoDataBind="True"
               Height="1036px" Width="928px" />
   <%-- <cc1:WebReport ID="WebReport1" runat="server" AutoHeight="True" 
            AutoWidth="True" Width="100%" Height = "100%" 
            ShowRefreshButton="False" />--%>
      <br />             
    </asp:Panel>               
    </div>   
    <asp:Label runat ="server" ID="lbStatus" ForeColor="Red"/>
    <asp:HiddenField ID="HiddenRemarkClose" runat="server" />
    <asp:HiddenField ID="HiddenRemarkDelete" runat="server" />    
    </form>                            
    </body>
</html>
