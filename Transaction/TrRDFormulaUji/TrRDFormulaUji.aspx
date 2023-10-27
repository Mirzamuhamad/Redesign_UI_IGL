<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TrRDFormulaUji.aspx.vb" Inherits="Transaction_TrRDFormulaUji_TrRDFormulaUji" %>

<%@ Register Assembly="BasicFrame.WebControls.BasicDatePicker" Namespace="BasicFrame.WebControls"
    TagPrefix="BDP" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

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
            var Lebar = parseFloat(document.getElementById("tbWidth").value.replace(/\$|\,/g,""));
            var Panjang = parseFloat(document.getElementById("tbLength").value.replace(/\$|\,/g,""));
            var Berat = parseFloat(document.getElementById("tbWeight").value.replace(/\$|\,/g,""));
            var QtySheet = parseFloat(document.getElementById("tbQtySheet").value.replace(/\$|\,/g,""));
            var Ratio = parseFloat(document.getElementById("tbRatio").value.replace(/\$|\,/g,""));
            if(isNaN(Lebar) == true)
            {
                Lebar = 0;
            }  
            if(isNaN(Panjang) == true)
            {
                Panjang = 0;
            }
            if(isNaN(Berat) == true)
            {
                Berat = 0;
            }
            if(isNaN(QtySheet) == true)
            {
                QtySheet = 0;
            }
            if(isNaN(Ratio) == true)
            {
                Ratio = 0;
            }
            var QtyFormulasi = (Lebar * Panjang * Berat * QtySheet * Ratio)/1000000;
            document.getElementById("tbWidth").value = setdigit(Lebar,'<%=VIEWSTATE("DigitQty")%>')                                               
            document.getElementById("tbLength").value = setdigit(Panjang,'<%=VIEWSTATE("DigitQty")%>')                                               
            document.getElementById("tbWeight").value = setdigit(Berat,'<%=VIEWSTATE("DigitQty")%>')                                               
            document.getElementById("tbQtySheet").value = setdigit(QtySheet,'<%=VIEWSTATE("DigitQty")%>')                                               
            document.getElementById("tbRatio").value = setdigit(Ratio,'<%=VIEWSTATE("DigitQty")%>')                                               
            document.getElementById("tbQtyFormulasi").value = setdigit(QtyFormulasi,'<%=VIEWSTATE("DigitQty")%>')                                                                                   
        }catch (err){
            alert(err.description);
          }      
        }   
        
        function setformatdt()
        {
         try
         {  
            document.getElementById("tbQtyDt3").value = setdigit(document.getElementById("tbQtyDt3").value.replace(/\$|\,/g,""),'<%=VIEWSTATE("DigitQty")%>');
                                                            
            
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
    <div class="H1">Formulir Uji Stabilitas & Kompabilitas</div>
     <hr style="color:Blue" />        
    <asp:Panel runat="server" id="PnlHd"> 
      <table>
        <tr>
            <td style="width:100px;text-align:right">Quick Search :</td>         
            <%--TransNmbr, TransDate, STATUS, FgReport, UserType, UserCode, UserName, Attn, Remark--%>   
            <td><asp:TextBox CssClass="TextBox" runat="server" ID ="tbFilter"/> 
                  <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlField" >                      
                    <asp:ListItem Selected="True" Value="TransNmbr">Formula Uji No</asp:ListItem>
                    <asp:ListItem Value="dbo.FormatDate(TransDate)">Formula Uji Date</asp:ListItem>
                    <asp:ListItem>Status</asp:ListItem>
                    <asp:ListItem Value="SampleName">Sample</asp:ListItem>                    
                    <asp:ListItem Value="Customer">Customer</asp:ListItem>
                    <asp:ListItem Value="CustomerName">Customer Name</asp:ListItem>
                    <asp:ListItem Value="FormulaTrialNo">Formula Trial No</asp:ListItem>
                    <asp:ListItem Value="Location">Location</asp:ListItem>
                    <asp:ListItem Value="dbo.FormatDate(StartDate)">Start Date</asp:ListItem>
                    <asp:ListItem Value="dbo.FormatDate(EndDate)">End Date</asp:ListItem>
                    <asp:ListItem Value="Remark">Remark</asp:ListItem>
                  </asp:DropDownList>
                  <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlRange"></asp:DropDownList>                 
                  <asp:Button class="bitbtn btnsearch" runat="server" ID="btnSearch" Text="Search" />																						 											  
                  <asp:Button class="bitbtn btngo" runat="server" ID="btnExpand" Text="..." />																						 											  
            </td>
            <td>
                <asp:LinkButton ID="LbAdvSearch" runat="server" Text="Advanced Search" />
            </td>
            <td style="width:100px; text-align: right;">
                Show Records:
            </td>
            <td>
                <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlShowRecord" AutoPostBack="true">
                    <asp:ListItem Selected="True" Value="15">Choose One</asp:ListItem>
                    <asp:ListItem Value="20">20</asp:ListItem>
                    <asp:ListItem Value="30">30</asp:ListItem>
                    <asp:ListItem Value="40">40</asp:ListItem>
                    <asp:ListItem Value="50">50</asp:ListItem>
                    <asp:ListItem Value="100">100</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td>Rows</td>
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
                    <asp:ListItem Selected="True" Value="TransNmbr">Formula Uji No</asp:ListItem>
                    <asp:ListItem Value="dbo.FormatDate(TransDate)">Formula Uji Date</asp:ListItem>
                    <asp:ListItem>Status</asp:ListItem>
                    <asp:ListItem Value="SampleName">Sample</asp:ListItem>                    
                    <asp:ListItem Value="Customer">Customer</asp:ListItem>
                    <asp:ListItem Value="CustomerName">Customer Name</asp:ListItem>
                    <asp:ListItem Value="FormulaTrialNo">Formula Trial No</asp:ListItem>
                    <asp:ListItem Value="Location">Location</asp:ListItem>
                    <asp:ListItem Value="dbo.FormatDate(StartDate)">Start Date</asp:ListItem>
                    <asp:ListItem Value="dbo.FormatDate(EndDate)">End Date</asp:ListItem>
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
                          <asp:Button class="btngo" runat="server" ID="BtnGo" Text="G" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" CommandName="Go" />                
                       </ItemTemplate>
                      <HeaderStyle Width="110px" />
                  </asp:TemplateField>
                  <asp:BoundField DataField="TransNmbr" HeaderStyle-Width="120px" SortExpression="Nmbr" HeaderText="Report No"></asp:BoundField>                  
                  <asp:BoundField DataField="Status" HeaderText="Status"></asp:BoundField>
                  <asp:BoundField DataField="UserType" HeaderStyle-Width="180px" HeaderText="User Type" SortExpression="UserType" />                
                  <asp:BoundField DataField="TransDate" dataformatstring="{0:dd MMM yyyy}" htmlencode="true" HeaderStyle-Width="80px" SortExpression="TransDate" HeaderText="Report Date"></asp:BoundField>
                  <asp:BoundField DataField="SampleName" HeaderStyle-Width="250px" SortExpression="SampleName" HeaderText="Sample"></asp:BoundField>
                  <asp:BoundField DataField="CustomerName" HeaderStyle-Width="250px" SortExpression="CustomerName" HeaderText="Customer"></asp:BoundField>
                  <asp:BoundField DataField="FormulaTrialNo" HeaderStyle-Width="150px" SortExpression="FormulaTrialNo" HeaderText="Trial No"></asp:BoundField>
                  <asp:BoundField DataField="StartDate" dataformatstring="{0:dd MMM yyyy}" htmlencode="true" HeaderStyle-Width="80px" SortExpression="StartDate" HeaderText="Start Date"></asp:BoundField>
                  <asp:BoundField DataField="EndDate" dataformatstring="{0:dd MMM yyyy}" htmlencode="true" HeaderStyle-Width="80px" SortExpression="EndDate" HeaderText="End Date"></asp:BoundField>
                  <asp:BoundField DataField="Location" HeaderStyle-Width="250px" SortExpression="Location" HeaderText="Location"></asp:BoundField>
                  <asp:BoundField DataField="Remark" HeaderStyle-Width="250px" SortExpression="Remark" HeaderText="Remark"></asp:BoundField>                  
              </Columns>
          </asp:GridView>
          </div>
            <asp:Panel runat="server" ID ="pnlNav" Visible="false">
            <asp:Button class="bitbtn btnadd" runat="server" ID="btnAdd2" Text="Add" />	  
            &nbsp &nbsp &nbsp
            <asp:DropDownList ID="ddlCommand2" CssClass="DropDownList" runat="server"/>
            <asp:Button class="btngo" runat="server" ID="btnGo2" Text="G" />                 
            </asp:Panel>
    </asp:Panel>    
    <asp:Panel runat="server" ID="pnlInput" Visible="false">
      <table>
        <tr>
            <td>Formula Uji No</td>
            <td>:</td>
            <td><asp:TextBox CssClass="TextBoxR" runat="server" ID="tbCode" Width="150px" Enabled="False"/></td>
            <td>Formula Uji Date</td>
            <td>:</td>
            <td>
            <BDP:BasicDatePicker ID="tbDate" runat="server" DateFormat="dd MMM yyyy" 
                        ReadOnly = "true" ValidationGroup="Input"
                        ButtonImageHeight="19px" ButtonImageWidth="20px" 
                        DisplayType="TextBoxAndImage" TextBoxStyle-CssClass="TextDate" 
                        ShowNoneButton="False"><TextBoxStyle CssClass="TextDate" /></BDP:BasicDatePicker>                
            </td>            
        </tr>         
        <tr>
            <td>Sample</td>
            <td>:</td>
            <td colspan="4">
                <asp:TextBox CssClass="TextBox" runat="server" ID="tbSample" AutoPostBack="true" ValidationGroup="Input"  />
                <asp:TextBox CssClass="TextBox" runat="server" ID="tbSampleName" 
                    Enabled="false" MaxLength="60" Width="306px"/>
                <asp:Button Class="btngo" ID="btnSample" Text="..." runat="server" ValidationGroup="Input" />
                <asp:Label ID="Label2" runat="server" ForeColor="#FF3300" Text="*"></asp:Label>
            </td>
        </tr>        
        <tr>
            <td>User</td>
            <td>:</td>
            <td colspan="4">
                <asp:DropDownList CssClass="DropDownList" ID="ddlUserType" runat="server" AutoPostBack="true">
                    <asp:ListItem Selected="True">Customer</asp:ListItem>
                    <asp:ListItem>Common</asp:ListItem>
                </asp:DropDownList>
                <asp:TextBox CssClass="TextBox" ValidationGroup="Input" runat="server" ID="tbCustomer" MaxLength="12" AutoPostBack="true" />
                <asp:TextBox CssClass="TextBox" runat="server" ID="tbCustomerName" Enabled="false" MaxLength="60" Width="225px"/>
                <asp:Button Class="btngo" ID="btnCustomer" Text="..." runat="server" ValidationGroup="Input" />
                <asp:Label ID="Label10" runat="server" ForeColor="#FF3300" Text="*"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>Formulir Trial No</td>
            <td>:</td>
            <td>
                <asp:TextBox CssClass="TextBox" Enabled="false" runat="server" ID="tbTrialNo" />
                <asp:Button Class="btngo" ID="btnTrialNo" Text="..." runat="server" ValidationGroup="Input" />
                <asp:Label ID="Label4" runat="server" ForeColor="#FF3300" Text="*"></asp:Label>
            </td>
            <td colspan="3">
                <asp:TextBox CssClass="TextBox" Id="tbXtimeHd" runat="server" Width="30px" ValidationGroup="Input"  />
                            <asp:DropDownList CssClass="DropDownList" ID="ddlTimeTypeHd" runat="server" Width="100px" ValidationGroup="Input" > 
                                <asp:ListItem>Week</asp:ListItem>
                                <asp:ListItem>Month</asp:ListItem>
                            </asp:DropDownList>
                <asp:Button Class="btngo" ID="btnGetData" Text="Get Data" Width="60px" runat="server" ValidationGroup="Input" />
            </td>
        </tr>
        <tr>
            <td>Start Date</td>
            <td>:</td>
            <td>
            <BDP:BasicDatePicker ID="tbStartDate" runat="server" DateFormat="dd MMM yyyy" 
                        ReadOnly = "true" ValidationGroup="Input"
                        ButtonImageHeight="19px" ButtonImageWidth="20px" 
                        DisplayType="TextBoxAndImage" TextBoxStyle-CssClass="TextDate" 
                        ShowNoneButton="False"><TextBoxStyle CssClass="TextDate" /></BDP:BasicDatePicker>                
            <asp:Label ID="Label1" runat="server" ForeColor="#FF3300" Text="*"></asp:Label>            
            </td>  
            <td>End Date</td>
            <td>:</td>
            <td>
            <BDP:BasicDatePicker ID="tbEndDate" runat="server" DateFormat="dd MMM yyyy" 
                        ReadOnly = "true" ValidationGroup="Input"
                        ButtonImageHeight="19px" ButtonImageWidth="20px" 
                        DisplayType="TextBoxAndImage" TextBoxStyle-CssClass="TextDate" 
                        ShowNoneButton="False"><TextBoxStyle CssClass="TextDate" /></BDP:BasicDatePicker>                
            <asp:Label ID="Label3" runat="server" ForeColor="#FF3300" Text="*"></asp:Label>            
            </td>  
        </tr>
        <tr>
              <td>Location</td>
              <td>:</td>
              <td colspan="4"><asp:TextBox ID="tbLocation" runat="server" CssClass="TextBox" ValidationGroup="Input" Width="380px"/></td>
          </tr>
        <tr>
              <td>Remark</td>
              <td>:</td>
              <td colspan="4"><asp:TextBox ID="tbRemark" runat="server" CssClass="TextBoxMulti" 
                      ValidationGroup="Input" Width="474px" TextMode="MultiLine" Height="72px" /></td>
          </tr>
        
      </table>  
      
      <br />      
      <hr style="color:Blue" />         
        <br />
              <asp:Panel runat="server" ID="PnlDt">
                <asp:Button class="bitbtn btnadd" runat="server" ID="btnAdddt" Text="Add" Visible="false" ValidationGroup="Input" />	
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
                            <asp:BoundField DataField="XTime" HeaderStyle-Width="60px" HeaderText="Time" />
                            <asp:BoundField DataField="TimeType" HeaderStyle-Width="60px" HeaderText="Time Type" />
                            <asp:BoundField DataField="Criteria" HeaderStyle-Width="60px" HeaderText="Criteria" />
                            <asp:BoundField DataField="CriteriaName" HeaderStyle-Width="280px" HeaderText="Criteria Name" />
                            <asp:BoundField DataField="Standard" HeaderStyle-Width="160px" HeaderText="Standard" />
                            <asp:BoundField DataField="Result" HeaderStyle-Width="100px" HeaderText="Result" />
                            <asp:BoundField DataField="Conclusion" HeaderStyle-Width="100px" HeaderText="Conclusion" />   
                            <asp:BoundField DataField="WeightStart" DataFormatString="{0:#,##0.##}" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="80px" HeaderText="Weight Start (gr)" />   
                            <asp:BoundField DataField="WeightEnd" DataFormatString="{0:#,##0.##}" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="80px" HeaderText="Weight End (gr)" />   
                            <asp:BoundField DataField="Resultgr" DataFormatString="{0:#,##0.##}" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="80px" HeaderText="Result (gr)" />   
                            <asp:BoundField DataField="Resultpercen" DataFormatString="{0:#,##0.##}" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="80px" HeaderText="Result (%)" />   
                            <asp:BoundField DataField="Remark" HeaderStyle-Width="200px" HeaderText="Remark" />   
                        </Columns>
                    </asp:GridView>
              </div>   
              <asp:Button class="bitbtn btnadd" runat="server" ID="btnAddDtke2" Text="Add" Visible="false" ValidationGroup="Input" />	
              </asp:Panel> 
              <asp:Panel runat="server" ID="pnlEditDt" Visible="false">
                <table> 
                    <tr>
                        <td>Time</td>
                        <td>:</td>
                        <td>
                            <asp:TextBox CssClass="TextBox" Id="tbXTime" runat="server" Width="30px" />
                            <asp:DropDownList CssClass="DropDownList" ID="ddlTimeType" runat="server" Width="100px" >
                                <asp:ListItem>Day</asp:ListItem>
                                <asp:ListItem>Week</asp:ListItem>
                                <asp:ListItem>Month</asp:ListItem>
                            </asp:DropDownList>
                            <asp:Label ID="Label5" runat="server" ForeColor="#FF3300" Text="*"></asp:Label>
                         </td>   
                    </tr>  
                    <tr>
                        <td>Criteria</td>
                        <td>:</td>
                        <td>
                            <asp:TextBox CssClass="TextBox" Id="tbCriteria" runat="server" AutoPostBack="True" Width="60px" />
                            <asp:TextBox CssClass="TextBox" ID="tbCriteriaName" runat="server" Width="280px" Enabled="False" /> 
                            <asp:Button ID="btnCriteria" runat="server" class="btngo" Text="..."/> 
                            <asp:Label ID="Label9" runat="server" ForeColor="#FF3300" Text="*"></asp:Label>
                         </td>   
                    </tr>   
                      
                    <tr>
                        <td>Standard</td>
                        <td>:</td>
                        <td><asp:TextBox ID="tbStandard" runat="server" CssClass="TextBox" Width="350px" /></td>
                     </tr> 
                     <tr>
                        <td>Result</td>
                        <td>:</td>
                        <td><asp:TextBox ID="tbResult" runat="server" CssClass="TextBox" Width="350px" /></td>
                     </tr> 
                     <tr>
                        <td>Conclusion</td>
                        <td>:</td>
                        <td><asp:DropDownList ID="ddlConclusion" runat="server" CssClass="TextBox" Width="180px" >
                                <asp:ListItem>OK</asp:ListItem>
                                <asp:ListItem>NOT OK</asp:ListItem>                                
                            </asp:DropDownList>
                        </td>
                     </tr> 
                     <tr>
                        <td>Weight (gr)</td>
                        <td>:</td>
                        <td>
                            <table cellpadding="0" cellspacing="0">
                                <tr style="background-color:Silver;text-align:center">
                                    <td>
                                        Start</td>
                                    <td>
                                        End</td>
                                    <td>
                                        Result (gr)</td>
                                    <td>
                                        Result (%)</td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="tbStart" runat="server" AutoPostBack="true" CssClass="TextBox" 
                                            Width="80px" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="tbEnd" runat="server" CssClass="TextBox" Width="80px" 
                                            AutoPostBack="True" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="tbResultgr" runat="server" AutoPostBack="true" 
                                            CssClass="TextBoxR" Width="80px" Enabled="False" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="tbResultpercen" runat="server" CssClass="TextBoxR" 
                                            Width="80px" Enabled="False" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                     </tr>
                     <tr>
                        <td>Remark</td>
                        <td>:</td>
                        <td>
                            <asp:TextBox ID="tbRemarkDt" runat="server" CssClass="TextBoxMulti" 
                                TextMode="MultiLine" Width="350px" />
                        </td>                
                    </tr>                        
                </table>
                <br />                     
                <asp:Button ID="btnSaveDt" runat="server" class="bitbtndt btnsave" Text="Save" />									
                <asp:Button ID="btnCancelDt" runat="server" class="bitbtndt btncancel" Text="Cancel" />									
           </asp:Panel> 
       <br />          
		<asp:Button ID="btnSaveAll" runat="server" class="bitbtndt btnsavenew" Text="Save & New" validationgroup="Input" Width = "90"/>									
        <asp:Button ID="btnSaveTrans" runat="server" class="bitbtndt btnsave" Text="Save" validationgroup="Input"/>									
        <asp:Button ID="btnBack" runat="server" class="bitbtndt btncancel" Text="Cancel" validationgroup="Input"/>									
        <asp:Button ID="btnHome" runat="server" class="bitbtndt btnback" Text="Home" />									                                                           
    </asp:Panel>        
    </div>
   
    <asp:Label runat ="server" ID="lbStatus" ForeColor="Red"/>
    </form>
</body>
</html>
