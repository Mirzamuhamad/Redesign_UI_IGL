<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TrWOBAP.aspx.vb" Inherits="TrWOBAP" %>
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
        var Rate = document.getElementById("tbRate").value.replace(/\$|\,/g,"");
        var PPn = document.getElementById("tbPPN").value.replace(/\$|\,/g,"");        
        var BaseForex = document.getElementById("tbBaseForex").value.replace(/\$|\,/g,"");
        var PPnForex = document.getElementById("tbPPNForex").value.replace(/\$|\,/g,"");
        var TotalForex = document.getElementById("tbTotalForex").value.replace(/\$|\,/g,"");
        
        document.getElementById("tbRate").value = setdigit(Rate,'<%=VIEWSTATE("DigitRate")%>');
        document.getElementById("tbPPN").value = setdigit(PPn,'<%=VIEWSTATE("DigitPercent")%>');
        document.getElementById("tbBaseForex").value = setdigit(BaseForex,'<%=VIEWSTATE("DigitCurr")%>');
        document.getElementById("tbPPNForex").value = setdigit(PPnForex,'<%=VIEWSTATE("DigitCurr")%>');
        document.getElementById("tbTotalForex").value = setdigit(TotalForex,'<%=VIEWSTATE("DigitCurr")%>');
        }catch (err){
            alert(err.description);
          }      
        }   
        
        function setformatdt()
        {
        try
         {         
        var Qty = document.getElementById("tbQty").value.replace(/\$|\,/g,""); 
        var QtyWrhs = document.getElementById("tbQtyWrhs").value.replace(/\$|\,/g,"");         
        document.getElementById("tbQty").value = setdigit(Qty,'<%=VIEWSTATE("DigitQty")%>');
        document.getElementById("tbQtyWrhs").value = setdigit(QtyWrhs,'<%=VIEWSTATE("DigitQty")%>');        
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
    <div class="H1">BAP Borongan</div>
     <hr style="color:Blue" />        
    <asp:Panel runat="server" id="PnlHd"> 
      <table>
        <tr>
            <td style="width:100px;text-align:right">Quick Search :</td>            
            <td><asp:TextBox CssClass="TextBox" runat="server" ID ="tbFilter"/> 
                  <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlField" >
                      <asp:ListItem Value="TransNmbr" Selected="True">Reference</asp:ListItem>
                      <asp:ListItem Value="dbo.FormatDate(TransDate)">Date</asp:ListItem>
                      <asp:ListItem Value="Divisi">Divisi</asp:ListItem>
                      <asp:ListItem Value="Job">Job Borongan</asp:ListItem>
                      <asp:ListItem>Status</asp:ListItem>                      
                      <asp:ListItem>Remark</asp:ListItem>
                  </asp:DropDownList>
                  <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlRange"></asp:DropDownList>
                  
                  <asp:Button class="bitbtn btnsearch" runat="server" ID="btnSearch" Text="Search" />																						 											  
                  <asp:Button class="bitbtn btngo" runat="server" ID="btnExpand" Text="..." />																					 											  
            </td>
            <td>
                <asp:LinkButton ID="LbAdvSearch" runat="server" Text="Advanced Search" />
                
                   &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                  
                        <asp:Label ID="Label17" runat="server" Text="Outstanding BAP : " />
                        <asp:LinkButton ID="lbCount" runat="server" Font-Size="Small" 
                            ForeColor="#FF6600" Text="X" />
                        <asp:Label ID="Label" runat="server" Text=" record(s)" />
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
                      <asp:ListItem Value="Divisi">Divisi</asp:ListItem>
                      <asp:ListItem Value="Job">Job Borongan</asp:ListItem>
                      <asp:ListItem>Status</asp:ListItem>                      
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
            <asp:Button class="btngo" runat="server" ID="BtnGo" Text="G" Visible="false" />                                <br />
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
                   <asp:BoundField DataField="TransNmbr" HeaderStyle-Width="120px" SortExpression="Nmbr"
                            HeaderText="Trans No">
                            <HeaderStyle Width="120px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Status" HeaderText="Status"></asp:BoundField>                         
                  <asp:BoundField DataField="TransDate" dataformatstring="{0:dd MMM yyyy}" htmlencode="true" HeaderStyle-Width="80px" SortExpression="TransDate" HeaderText="Date"></asp:BoundField>
                  <asp:BoundField DataField="Type" HeaderStyle-Width="120px" SortExpression="Type" HeaderText="Type"></asp:BoundField>
                  <asp:BoundField DataField="Divisi" HeaderStyle-Width="150px" SortExpression="Divisi" HeaderText="Divisi"></asp:BoundField>
                  <asp:BoundField DataField="DivisionName" HeaderStyle-Width="150px" HeaderText="DivisionName"></asp:BoundField>
                  <asp:BoundField DataField="Team" HeaderStyle-Width="150px" HeaderText="Team"></asp:BoundField>
                  <asp:BoundField DataField="Team_Name" HeaderStyle-Width="150px" HeaderText="Team Name"></asp:BoundField>
                  <asp:BoundField DataField="FgBorongan" HeaderStyle-Width="50px" HeaderText="Job Borongan"></asp:BoundField>
                  <asp:BoundField DataField="Supplier" HeaderStyle-Width="150px" HeaderText="Supplier"></asp:BoundField>
                  <asp:BoundField DataField="Supplier_Name" HeaderStyle-Width="180px" HeaderText="Supplier Name"></asp:BoundField>
                  <asp:BoundField DataField="Remark" HeaderStyle-Width="250px" HeaderText="Remark"></asp:BoundField>
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
            <td>Trans No</td>
            <td>:</td>
            <td><asp:TextBox CssClass="TextBox" runat="server" ID="tbRef" Width="150px" Enabled = "false"  /></td>              
            <%--<asp:Label ID="Label1" runat="server" Text="   Report : "></asp:Label>
            <asp:DropDownList ID ="ddlReport" ValidationGroup="Input" Enabled="true" AutoPostBack="true" runat ="server" CssClass="DropDownList">
                                    <asp:ListItem>Y</asp:ListItem>
                                    <asp:ListItem>N</asp:ListItem>
                                </asp:DropDownList> 
            </td> --%>                   
            <td> Date</td>
            <td>:</td>
            <td>
            <BDP:BasicDatePicker ID="tbTransDate" runat="server" DateFormat="dd MMM yyyy" 
                        ReadOnly = "true" ValidationGroup="Input"
                        ButtonImageHeight="19px" ButtonImageWidth="20px" 
                        DisplayType="TextBoxAndImage" 
                        TextBoxStyle-CssClass="TextDate" AutoPostBack="True" 
                        ShowNoneButton="False">
                        <TextBoxStyle CssClass="TextDate" />
             </BDP:BasicDatePicker>                
            </td>  
            <td>Type</td>
            <td>:</td>    
            <td><asp:TextBox CssClass="TextBox" runat="server" ID="tbTypehd" Width="100px" Enabled = "false">LKM</asp:TextBox>
            </td>      
        </tr> 
        <tr>
            <td>Divisi</td>
            <td>:</td>
            <td><asp:DropDownList ID="ddlDivision" runat="server" Width="250px" ValidationGroup="Input" CssClass="DropDownList"/>                                                
            </td>            
        </tr>
        <tr>
            <td>Work By</td>
            <td>:</td>
            <td colspan="4"><asp:TextBox CssClass="TextBox" ValidationGroup="Input" runat="server" ID="tbWorkbyCode" AutoPostBack="true" />
                <asp:TextBox CssClass="TextBox" runat="server" ID="tbWorkByName" Enabled="false" Width="225px"/>
                <asp:Button Class="btngo" ID="btnWorkBy" Text="..." runat="server" ValidationGroup="Input" />                                               
              
            </td>            
        </tr>
        <tr>
            <td>Job Borongan</td>
            <td>:</td>
            <td ><asp:DropDownList ID="ddlFgBorongan" runat="server" Width="50px" ValidationGroup="Input" CssClass="DropDownList" Enabled = "false">
                 <asp:ListItem>Y</asp:ListItem>
                 <asp:ListItem Selected = "True">N</asp:ListItem>
                 </asp:DropDownList>
             </td>            
        </tr>        
        <tr>
            <td>Kontraktor</td>
            <td>:</td>
             <td colspan="4"><asp:TextBox CssClass="TextBox" ValidationGroup="Input" runat="server" ID="tbKontraktorCode" AutoPostBack="true" />
                <asp:TextBox CssClass="TextBox" runat="server" ID="tbKontraktorName" Enabled="false" Width="225px"/>
                <asp:Button Class="btngo" ID="btnKontraktor" Text="..." runat="server" ValidationGroup="Input" />                                               
              
            </td>          
        </tr>
        
        <tr>
              <td>Amount</td>
              <td>:</td>
              <td colspan="4">
              <table>
                    <tr style="background-color:#F5F5F5;text-align:center">
                        <td>Currency</td> 
                        <td>Total</td> 
                        <tr>
                        <td><asp:TextBox ID="tbCurrency" ValidationGroup="Input" runat="server" 
                                Width="120px" CssClass="TextBox">Rp</asp:TextBox>
                            </td>                        
                        <td><asp:TextBox ID="tbTotalForex" ValidationGroup="Input" runat="server" 
                                Width="120px" CssClass="TextBox">0</asp:TextBox>
                            </td>
                        </tr>
          </tr>
          
               </table>
          </td>
          </tr>
          <tr>
              <td>Remark</td>
              <td>:</td>
              <td colspan="4"><asp:TextBox ID="tbRemark" runat="server" CssClass="TextBoxMulti" 
                        MaxLength="255" TextMode="MultiLine" ValidationGroup="Input" Width="350px" /> </td>
          </tr>
      </table>  
      
      
      
      <br />      
      <div style="font-size:medium; color:Blue;">Detail</div>
      <hr style="color:Blue" />  
        <asp:Panel runat="server" ID="pnlDt">
            <asp:Button class="bitbtn btnadd" runat="server" ID="btnAddDt" Text="Add" ValidationGroup="Input" />	                  
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
                            <EditItemTemplate>
                                <asp:Button ID="btnUpdate" runat="server" class="bitbtndt btnsave" Text="Save" CommandName="Update"/>									
                                <asp:Button ID="btnCancel" runat="server" class="bitbtndt btncancel" Text="Cancel" CommandName="Cancel"/>									
                            </EditItemTemplate>                            
                        </asp:TemplateField>
                        <asp:BoundField DataField="WOResult" HeaderStyle-Width="120px" HeaderText="LKM No" />
                        <asp:BoundField DataField="WOResultDate" DataFormatString="{0:dd MMM yyyy}"  HeaderStyle-Width="180px"  HeaderText="LKM Date" />
                        <asp:BoundField DataField="WONo"  HeaderStyle-Width="120px" HeaderText="Wo No" />
                        <asp:BoundField DataField="Type" HeaderStyle-Width="180px" HeaderText="Type" />                                              
                        <asp:BoundField DataField="DivisiBlok"  HeaderStyle-Width="250px" HeaderText="Divisi Blok" />
                        <asp:BoundField DataField="DivisiBlokName" HeaderStyle-Width="350px" HeaderText="Divisi Blok Name" />
                        <asp:BoundField DataField="Job" HeaderStyle-Width="250px" HeaderText="Job Code" />
                        <asp:BoundField DataField="JobName" HeaderStyle-Width="350px" HeaderText="Job Name" />
                        <asp:BoundField DataField="QtyResult" DataFormatString="{0:#,##0.##}" HeaderStyle-Width="80px" HeaderText="Qty LKM" />
                        <asp:BoundField DataField="QtyDone" DataFormatString="{0:#,##0.##}" HeaderStyle-Width="80px" HeaderText="Qty Done" />
                        <asp:BoundField DataField="Qty" DataFormatString="{0:#,##0.##}" HeaderStyle-Width="80px" HeaderText="Qty BAP" />
                        <asp:BoundField DataField="Unit" HeaderStyle-Width="80px" HeaderText="Unit" />
                        <asp:BoundField DataField="PriceForex" DataFormatString="{0:#,##0.##}"  HeaderStyle-Width="80px" HeaderText="Price" />
                        <asp:BoundField DataField="AmountForex" DataFormatString="{0:#,##0.##}"  HeaderStyle-Width="80px" HeaderText="Amount Forex" />
                        <asp:BoundField DataField="Disc" DataFormatString="{0:#,##0.##}" HeaderStyle-Width="80px" HeaderText="Disc" />
                        <asp:BoundField DataField="DiscForex" DataFormatString="{0:#,##0.##}" HeaderStyle-Width="80px" HeaderText="Disc Forex" />
                        <asp:BoundField DataField="NettoForex" DataFormatString="{0:#,##0.##}"  HeaderStyle-Width="80px" HeaderText="Netto Forex" />
                        <asp:BoundField DataField="Remark" HeaderStyle-Width="400px" HeaderText="Remark" />
                    </Columns>
                </asp:GridView>
          </div>   
          <asp:Button class="bitbtn btnadd" runat="server" ID="btnAddDt2" Text="Add" ValidationGroup="Input" />	  
       </asp:Panel>             
       <asp:Panel runat="server" ID="pnlEditDt" Visible="false">
            <table>                
                <tr>
                    <td>LKM No </td>
                    <td>:</td>
                    <td><asp:TextBox runat="server" ID="tbWOResult" CssClass="TextBox" />
                    <asp:Button Class="btngo" ID="btnWOResult" Text="..." runat="server" ValidationGroup="Input" />
                        
                    </td>                               
                </tr>                   
                <tr>
                    <td>LKM Date</td>
                    <td>:</td>
                    <td><BDP:BasicDatePicker ID="tbWOResultDate" runat="server" DateFormat="dd MMM yyyy" 
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
                    <td>WO No</td>
                    <td>:</td>
                    <td><asp:TextBox CssClass="TextBox" runat="server" ID="tbWONo"/></td>                    
                </tr>
                <tr>
                    <td>Type</td>
                    <td>:</td>
                    <td><asp:TextBox CssClass="TextBox" runat="server" ID="tbType" /></td>
                </tr>
                <tr>
                    <td>Divisi Blok</td>
                    <td>:</td>
                    <td><asp:TextBox CssClass="TextBox" ValidationGroup="Input" runat="server" ID="ddlDivisiBlok" AutoPostBack="true" />
                <asp:TextBox CssClass="TextBox" runat="server" ID="tbDivisiBlokName" Enabled="false" Width="225px"/>
                <asp:Button Class="btngo" ID="btnDivisiBlok" Text="..." runat="server" ValidationGroup="Input" />  </td>
                </tr>  
                <tr>
                    <td>Job</td>
                    <td>:</td>
                    <td><asp:TextBox CssClass="TextBox" ValidationGroup="Input" runat="server" ID="ddlJob" AutoPostBack="true" />
                <asp:TextBox CssClass="TextBox" runat="server" ID="tbJobName" Enabled="false" Width="225px"/>
                <asp:Button Class="btngo" ID="btnJob" Text="..." runat="server" ValidationGroup="Input" />  </td>
                </tr> 
              
                <tr >
                    <td>Qty LKM</td>
                    <td>:</td>
                    <td><asp:TextBox CssClass="TextBox" runat="server" ID="tbQtyResult" Width="54px" />&nbsp;
                    &nbsp;Qty Done &nbsp;:
                    <asp:TextBox CssClass="TextBox" runat="server" ID="tbQtyDone" Width="54px" /> &nbsp;
                    &nbsp;Qty BAP&nbsp;:
                    <asp:TextBox CssClass="TextBox" runat="server" ID="tbQty" Width="54px" /></td>
                </tr>
                <tr>
                    <td>Unit</td>
                    <td>:</td>
                    <td><asp:TextBox CssClass="TextBox" runat="server" ID="tbUnit" Width="54px" /></td>
                </tr>
                <tr>
                    <td>Price</td>
                    <td>:</td>
                    <td><asp:TextBox CssClass="TextBox" runat="server" ID="tbPriceForex" Width="54px" /></td>
                </tr>
                <tr>
                    <td>Amount Forex</td>
                    <td>:</td>
                    <td><asp:TextBox CssClass="TextBox" runat="server" ID="tbAmountForex" Width="54px" /></td>
                </tr>
                <tr>
                    <td>Disc</td>
                    <td>:</td>
                    <td><asp:TextBox CssClass="TextBox" runat="server" ID="tbDisc" Width="54px" /></td>
                </tr>
                <tr>
                    <td>Disc Forex</td>
                    <td>:</td>
                    <td><asp:TextBox CssClass="TextBox" runat="server" ID="tbDiscForex" Width="54px" /></td>
                </tr>
                <tr>
                    <td>Netto Forex</td>
                    <td>:</td>
                    <td><asp:TextBox CssClass="TextBox" runat="server" ID="tbNettoForex" Width="54px" /></td>
                </tr>
                <tr>
                    <td>Remark </td>
                    <td>:</td>
                    <td><asp:TextBox runat="server" MaxLength="255" CssClass="TextBoxMulti" TextMode="MultiLine" ID="tbRemarkDt" Width = "355" /></td>
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
    <%--<asp:Panel runat="server" ID="pnlPrint" Visible="false">
    <cr:crystalreportviewer ID="CrystalReportViewer1" runat="server" AutoDataBind="True"
               Height="1036px" Width="928px" />
    </asp:Panel>--%>
      <br />            
    </div>
   
    <asp:Label runat ="server" ID="lbStatus" ForeColor="Red"/>
   
    </form>
    </body>
</html>
