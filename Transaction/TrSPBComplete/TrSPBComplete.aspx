<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TrSPBComplete.aspx.vb" Inherits="Transaction_TrSPB_TrSPB" %>
<%@ Register Assembly="FastReport" Namespace="FastReport.Web" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="BasicFrame.WebControls.BasicDatePicker" Namespace="BasicFrame.WebControls"
    TagPrefix="BDP" %>
<%@ Register assembly="CrystalDecisions.Web, Version=10.5.3700.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>S P B</title>
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
        document.getElementById("tbQty").value = setdigit(document.getElementById("tbQty").value.replace(/\$|\,/g,""),'<%=ViewState("DigitQty")%>');        
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
</head>

<body>     
    <form id="form1" runat="server">
     <div class="Content">
    <div class="H1">S P B Complete</div>
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
                      <asp:ListItem Value="dbo.FormatDate(InputDate)">Date Muat</asp:ListItem>
                       <asp:ListItem Value="BrondolanKgs">Brondolan Kgs</asp:ListItem>
                      <asp:ListItem Value="BrdKirim">Brd Kirim</asp:ListItem>
                      <asp:ListItem Value="BrdGrading">Brd Grading</asp:ListItem>
                      <asp:ListItem Value="TPSBruto">TPS Bruto</asp:ListItem>
                      <asp:ListItem Value="TPSReal">TPS Real</asp:ListItem>
                      <asp:ListItem Value="TPSGrading">TPS Grading</asp:ListItem>
                      <asp:ListItem Value="TPSNetto">TPS Netto</asp:ListItem>
                      <asp:ListItem Value="TotalKirim">Total Kirim</asp:ListItem>
                      <asp:ListItem Value="TotalKirim">Total Kirim</asp:ListItem>
                      <asp:ListItem Value="BalanceTBS">Balance TBS</asp:ListItem>
                      <asp:ListItem Value="BalanceBrd">Balance Brd</asp:ListItem>
                      <asp:ListItem Value="EstateName">Estate</asp:ListItem>
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
                 <asp:ListItem Value="dbo.FormatDate(InputDate)">Date Muat</asp:ListItem>
                       <asp:ListItem Value="BrondolanKgs">Brondolan Kgs</asp:ListItem>
                      <asp:ListItem Value="BrdKirim">Brd Kirim</asp:ListItem>
                      <asp:ListItem Value="BrdGrading">Brd Grading</asp:ListItem>
                      <asp:ListItem Value="TPSBruto">TPS Bruto</asp:ListItem>
                      <asp:ListItem Value="TPSReal">TPS Real</asp:ListItem>
                      <asp:ListItem Value="TPSGrading">TPS Grading</asp:ListItem>
                      <asp:ListItem Value="TPSNetto">TPS Netto</asp:ListItem>
                      <asp:ListItem Value="TotalKirim">Total Kirim</asp:ListItem>
                      <asp:ListItem Value="BalanceTBS">Balance TBS</asp:ListItem>
                      <asp:ListItem Value="BalanceBrd">Balance Brd</asp:ListItem>
                      <asp:ListItem Value="EstateName">Estate</asp:ListItem>
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
                 <%-- <asp:TemplateField>
                      <HeaderTemplate>
                          <asp:CheckBox ID="cbSelectHd" runat="server" AutoPostBack="true"
                          oncheckedchanged="cbSelectHd_CheckedChanged" />
                      </HeaderTemplate>
                      <ItemTemplate>
                          <asp:CheckBox ID="cbSelect" runat="server" />
                      </ItemTemplate>
                  </asp:TemplateField>--%>
                  <asp:TemplateField HeaderStyle-Width="110">
                      <ItemTemplate>
                          <asp:DropDownList CssClass="DropDownList" ID="ddl" runat="server">
                              <asp:ListItem Selected="True" Text="View" />
                              <%--<asp:ListItem Text="Edit" />--%>
                              <asp:ListItem Text="Print" />
                          </asp:DropDownList>
                          <asp:Button class="btngo" runat="server" ID="BtnGo" Text="G" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" CommandName="Go"  />                                                 
                      </ItemTemplate>
                      <HeaderStyle Width="110px" />
                  </asp:TemplateField>                     
                  <asp:BoundField DataField="TransNmbr" SortExpression="TransNmbr" 
                      HeaderText="Reference"></asp:BoundField>                  
                  <asp:BoundField DataField="Status" HeaderText="Status" SortExpression="Status"></asp:BoundField>                  
                  <asp:BoundField DataField="TransDate" HtmlEncode="true" DataFormatString="{0:dd MMM yyyy}" HeaderText="Date" SortExpression="TransDate"></asp:BoundField>                  
                  <asp:BoundField DataField="InputDate" HtmlEncode="true" DataFormatString="{0:dd MMM yyyy}" HeaderText="Date Muat" SortExpression="InputDate"></asp:BoundField>                  
                  <asp:BoundField DataField="TPSreal" HeaderStyle-Width="120px" 
                      HeaderText="TPS" SortExpression="TBS (Janjang) TPS">
                      <HeaderStyle Width="120px" />
                  </asp:BoundField>
                  <asp:BoundField DataField="TPSGrading" HeaderText="TBS (Janjang) Grading" 
                      SortExpression="TPSGrading" />
                  <asp:BoundField DataField="TotalKirim" HeaderText="TBS (Janjang) Kirim" 
                      SortExpression="TotalKirim" />
                  <asp:BoundField DataField="BalanceTBS" HeaderText="TBS (Janjang) Balance" 
                      SortExpression="BalanceTBS" />
                  <asp:BoundField DataField="BrondolanKgs" HeaderStyle-Width="120px" HeaderText="Brondolan Kgs (TPS)" 
                      SortExpression="BrondolanKgs">
                      <HeaderStyle Width="120px" />
                  </asp:BoundField>
                  <asp:BoundField DataField="BrdKirim" HeaderStyle-Width="120px" 
                      HeaderText="Brondolan Kgs Kirim" SortExpression="BrdKirim">
                      <HeaderStyle Width="120px" />
                  </asp:BoundField>
                  <asp:BoundField DataField="BrdGrading" HtmlEncode="true" HeaderText="Brondolan Kgs Grading" 
                      SortExpression="BrdGrading"></asp:BoundField>                  
                  <asp:BoundField DataField="BalanceBrd" HeaderText="Brondolan Kgs Balance" 
                      SortExpression="BalanceBrd" />
                  <asp:BoundField DataField="Remark" HeaderStyle-Width="200px" HeaderText="Remark">
                      <HeaderStyle Width="200px" />
                  </asp:BoundField>
                 
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
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
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
            Date Muat
              </td>
            <td>
               :</td>
            <td>
             <BDP:BasicDatePicker ID="tbDateMuat" runat="server" DateFormat="dd MMM yyyy" 
                        ReadOnly = "true" ValidationGroup="Input"
                        ButtonImageHeight="19px" ButtonImageWidth="20px" 
                        DisplayType="TextBoxAndImage" 
                        TextBoxStyle-CssClass="TextDate" AutoPostBack="True" 
                    ShowNoneButton="False"><TextBoxStyle CssClass="TextDate" /></BDP:BasicDatePicker> 
            </td>
        </tr>      
       
        <tr>
              <td>Estate</td>
              <td>:</td>
              <td>
                  <asp:DropDownList ID="ddlEstate" runat="server" Width="142px" Height="16px"
                      CssClass="DropDownList" ValidationGroup="Input">
                  </asp:DropDownList>
                  <asp:Label ID="Label2" runat="server" ForeColor="Red">*</asp:Label>
              </td>
              <td>
                 </td>
              <td>
                 </td>
              <td>
               
              </td>
          </tr>
          
        <tr>
              <td>TBS (janjang)</td>
              <td>:</td>
              <td>
                  <table cellpadding="0" cellspacing="0">
                      <tr style="background-color:Silver;text-align:center">
                          <td>
                              TPS</td>
                          <td>
                              Kirim</td>
                          <td>
                              Grading</td>
                          <td>
                              Balance</td>
                          
                      </tr>
                      <tr>
                          <td>
                              <asp:TextBox ID="tbTTPS" runat="server" CssClass="TextBoxR" Width="110px" 
                                  Enabled="False" />
                          </td>
                          <td>
                              <asp:TextBox ID="tbTKirim" runat="server" CssClass="TextBoxR" Width="110px" 
                                  Enabled="False" />
                          </td>
                          <td>
                              <asp:TextBox ID="tbTGrading" runat="server" CssClass="TextBoxR" Height="16px" 
                                  Width="91px" Enabled="False" />
                          </td>
                           <td>
                              <asp:TextBox ID="TbBalance" runat="server" CssClass="TextBoxR" Width="110px" 
                                  Enabled="False" />
                          </td>
                          
                          </td>
                      </tr>
                  </table>
              </td>
              <td>
                 </td>
              <td>
                 </td>
              <td>
               
              </td>
          </tr>
          <tr>
              <td>Brondolan (krg)</td>
              <td>:</td>
              <td>
                  <table cellpadding="0" cellspacing="0">
                      <tr>
                          <td>
                              <asp:TextBox ID="tbbTPS" runat="server" CssClass="TextBoxR" Width="110px" 
                                  Enabled="False" />
                          </td>
                          <td>
                              <asp:TextBox ID="tbbTKirim" runat="server" CssClass="TextBoxR" Width="110px" 
                                  Enabled="False" />
                          </td>
                          <td>
                              <asp:TextBox ID="tbbTGrading" runat="server" CssClass="TextBoxR" Height="16px" 
                                  Width="91px" Enabled="False" />
                          </td>
                           <td>
                              <asp:TextBox ID="TbbBalance" runat="server" CssClass="TextBoxR" Width="110px" 
                                  Enabled="False" />
                          </td>
                          
                      </tr>
                  </table>
              </td>
              <td>
                 </td>
              <td>
                 </td>
              <td>
               
              </td>
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
                                    <asp:Button ID="btnClosing" runat="server" CssClass="Button" Text="Complete"                                             
                                            CommandArgument='<%# Container.DataItemIndex %>'
                                            CommandName="Complete" />
                                            <%--CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>"--%>
                                </ItemTemplate>
                        </asp:TemplateField>     
                        <asp:BoundField DataField="SPBManualNo" HeaderStyle-Width="100px" HeaderText="SPB Manual No" SortExpression="SPBManualNo" ></asp:BoundField>
                        <asp:BoundField DataField="TripNo" HeaderText="TripNo" HeaderStyle-Width="180px" SortExpression="Trip No" ></asp:BoundField>
                        <asp:BoundField DataField="Customer" HeaderText="Customer" SortExpression="Customer" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right"></asp:BoundField>                        
                        <asp:BoundField DataField="CustomerName" HeaderText="Customer Name" SortExpression="CustomerName" ></asp:BoundField>
                        <asp:BoundField DataField="CarNo" HeaderText="Car No" SortExpression="CarNo"></asp:BoundField>
                        <asp:BoundField DataField="CarName" HeaderText="Car Name" SortExpression="CarName"></asp:BoundField>
                        <asp:BoundField DataField="Owner" HeaderText="Pemilik Kapal" SortExpression="Owner"></asp:BoundField>
                        <asp:BoundField DataField="Pengawas" HeaderText="Pengawas" SortExpression="Pengawas"></asp:BoundField>
                        <asp:BoundField DataField="PengawasName" HeaderText="Pengawas Name" SortExpression="PengawasName"></asp:BoundField>
                        <asp:BoundField DataField="DateBerangkat" HeaderText="Date Berangkat"  HtmlEncode="true" DataFormatString="{0:dd MMM yyyy}" SortExpression="DateBerangkat"></asp:BoundField>
                        <asp:BoundField DataField="JamBerangkat" HeaderText="JamBerangkat" SortExpression="JamBerangkat"></asp:BoundField>
                        <asp:BoundField DataField="StatusTanam" HeaderText="Status Tanam" SortExpression="StatusTanam"></asp:BoundField>
                        <asp:BoundField DataField="StatusTanamName" HeaderText="Status Tanam Name" SortExpression="StatusTanamName"></asp:BoundField>
                        <asp:BoundField DataField="PriceTPS" HeaderText="Price TBS" SortExpression="PriceTPS" DataFormatString="{0:#,##0.00}"></asp:BoundField>
                        <asp:BoundField DataField="FactorRate" HeaderText="Factor Rate" SortExpression="FactorRate" DataFormatString="{0:#,##0.00}" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right"></asp:BoundField>                        
                        <asp:BoundField DataField="BJR" HeaderText="BJR" SortExpression="BJR" DataFormatString="{0:#,##0.00}" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right"></asp:BoundField>                        
                        <asp:BoundField DataField="QtyJanjang" HeaderText="Qty Janjang" SortExpression="QtyJanjang" DataFormatString="{0:#,##0.00}" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right"></asp:BoundField>                        
                        <asp:BoundField DataField="QtyBrondolan" HeaderText="Brondolan (Krg)" SortExpression="QtyBrondolan" DataFormatString="{0:#,##0.00}" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right"></asp:BoundField>                        
                         <asp:BoundField DataField="DoneComplete" HeaderStyle-Width="20px" HeaderText="Complete">
                      <HeaderStyle Width="20px" />
                  </asp:BoundField>
                  <asp:BoundField DataField="UserComplete" HeaderStyle-Width="80px" HeaderText="UserComplete">
                      <HeaderStyle Width="80px" />
                  </asp:BoundField>
                  <asp:BoundField DataField="DateComplete" HtmlEncode="true" DataFormatString="{0:dd MMM yyyy}" HeaderStyle-Width="50px" HeaderText="DateComplete">
                      <HeaderStyle Width="50px" />
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
                                SPB Manual
                            </td>
                            <td>:</td>
                            <td colspan="4">
                                <asp:TextBox ID="tbSPBManual" runat="server" CssClass="TextBox" Width="200px" />
                                
                            </td>
                        </tr>
                        <tr>
                            <td>Trip No</td>
                            <td>:</td>
                            <td colspan="4"><asp:TextBox ID="tbTrip" runat="server" CssClass="TextBox" /></td>
                        </tr>                
                        <tr>
                            <td>Customer</td>
                            <td>:</td>
                            <td colspan="4">
                                <asp:TextBox ID="tbCustCode" runat="server" CssClass="TextBox" AutoPostBack ="true" />
                                <asp:TextBox ID="tbCustName" runat="server" CssClass="TextBox" Enabled="False" 
                                    EnableTheming="True" ReadOnly="True" Width="200px" />
                                <asp:Button Class="btngo" ID="btnCust" Text="..." runat="server" />                  
                             
                            </td>
                        </tr>                
                        <tr>
                            <td>
                                Kapal</td>
                            <td>
                                :</td>
                            <td>
                               <asp:TextBox ID="tbKapal" runat="server" CssClass="TextBox" AutoPostBack ="true" />
                                <asp:TextBox ID="tbKapalName" runat="server" CssClass="TextBox" Enabled="False" 
                                    EnableTheming="True" ReadOnly="True" Width="200px" />
                                <asp:Button Class="btngo" ID="btnCar" Text="..." runat="server" /> 
                            </td>
                            <td>
                                Pemilik Kapal</td>
                            <td>
                                :</td>
                            <td>
                                <asp:TextBox ID="tbOwner" runat="server" CssClass="TextBox" Width="200px" enabled ="false"/>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Pengawas</td>
                            <td>:</td>
                            <td colspan="4">
                                <asp:TextBox ID="tbPengawasCode" runat="server" CssClass="TextBox" autoPostBack ="true"  />
                                <asp:TextBox ID="tbPengawasName" runat="server" CssClass="TextBox" Width="200px" enabled ="false" />
                                <asp:Button Class="btngo" ID="btnPengawas" Text="..." runat="server" /> 
                            </td>
                        </tr>
                        <tr>
                        <td>Tanggal/ Jam Berangkat</td>
                        <td>:</td>
                        <td>
                            <BDP:BasicDatePicker ID="tbSPBDate" runat="server" ButtonImageHeight="19px" 
                                ButtonImageWidth="20px" DateFormat="dd MMM yyyy" DisplayType="TextBoxAndImage" 
                                ReadOnly="true" ShowNoneButton="False" TextBoxStyle-CssClass="TextDate" 
                                ValidationGroup="Input">
                                <TextBoxStyle CssClass="TextDate" />
                            </BDP:BasicDatePicker>
                            /
                            <asp:TextBox ID="tbJamSPB" runat="server" CssClass="TextBox" MaxLength="5" 
                                Width="56px" />
                            <asp:Label ID="Label1" runat="server" ForeColor="Red">*</asp:Label>
                        </td>
                            <td>
                                Status Tanam</td>
                            <td>
                                :</td>
                            <td>
                                <asp:DropDownList ID="ddlstatusTanam" runat="server" CssClass="DropDownList" ValidationGroup="Input" AutoPostBack ="true"
                                    Width="142px" Height="16px">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Price TBS</td>
                            <td>
                                &nbsp;</td>
                            <td colspan="4">
                                <table cellpadding="0" cellspacing="0">
                                    <tr style="background-color:Silver;text-align:center">
                                        <td>
                                            Price TBS</td>
                                        <td>
                                           Factor Rate</td>
                                        <td>
                                            BJR</td>
                                         <td>
                                           Qty Janjang</td>    
                                        <td>
                                           Brondolan (Krg)</td>       
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="tbPriceTPS" runat="server" CssClass="TextBox" 
                                                 enabled="false"  Width="110px" />
                                        </td>
                                        <td>
                                            <asp:TextBox ID="tbFactorRate" runat="server" CssClass="TextBox" 
                                                 enabled="false"  Width="110px" />
                                        </td>
                                        <td>
                                            <asp:TextBox ID="tbBJR" runat="server" CssClass="TextBox" Height="16px" 
                                                enabled="false" Width="81px" />
                                        </td>
                                        <td>
                                            <asp:TextBox ID="tbQtyJanjang" runat="server" CssClass="TextBox" Height="16px" 
                                                ValidationGroup="Input" Width="81px" />
                                        </td>
                                        <td>
                                            <asp:TextBox ID="tbQtyBrondolan" runat="server" CssClass="TextBox" Height="16px" 
                                                ValidationGroup="Input" Width="81px" />
                                        </td>
                                    </tr>
                                </table>
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
        <br />
        
       <br />    
       
       
       <asp:Button ID="btnSaveAll" runat="server" class="bitbtndt btnsavenew" Text="Save & New" validationgroup="Input" Width = "90"/>									
       <asp:Button ID="btnSaveTrans" runat="server" class="bitbtndt btnsave" Text="Save" validationgroup="Input"/>									
       <asp:Button ID="btnBack" runat="server" class="bitbtndt btncancel" Text="Cancel" validationgroup="Input"/>									
       <asp:Button ID="btnHome" runat="server" class="bitbtndt btnback" Text="Home" />									                                             
       
     
    </asp:Panel>
    
    <asp:Panel ID="pnlClosing" runat="server" Visible="false">
            <table width="100%">
                <tr>
                    <td style="width:60%">
                        <table style="width: 697px">
                            <tr>
                                <td>
                                    Date
                                </td>
                                <td>
                                    :</td>
                                <td>
                                    <BDP:BasicDatePicker ID="tbCDate" runat="server" ButtonImageHeight="19px" 
                                        ButtonImageWidth="20px" DateFormat="dd MMM yyyy" DisplayType="TextBoxAndImage" 
                                        ReadOnly="true" Enabled = "False" ShowNoneButton="False" TextBoxStyle-CssClass="TextDate" 
                                        ValidationGroup="Input">
                                        <TextBoxStyle CssClass="TextDate" />
                                    </BDP:BasicDatePicker>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Date Muat
                                </td>
                                <td>
                                    :</td>
                                <td>
                                    <BDP:BasicDatePicker ID="tbCDateMuat" runat="server" ButtonImageHeight="19px" 
                                        ButtonImageWidth="20px" Enabled = "False" DateFormat="dd MMM yyyy" DisplayType="TextBoxAndImage" 
                                        ReadOnly="true" ShowNoneButton="False" TextBoxStyle-CssClass="TextDate" 
                                        ValidationGroup="Input">
                                        <TextBoxStyle CssClass="TextDate" />
                                    </BDP:BasicDatePicker>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    ComplateDate
                                </td>
                                <td>
                                    :</td>
                                <td>
                                    <BDP:BasicDatePicker ID="tbCComplateDate" runat="server" ButtonImageHeight="19px" 
                                        ButtonImageWidth="20px" DateFormat="dd MMM yyyy" DisplayType="TextBoxAndImage" 
                                         ShowNoneButton="False" TextBoxStyle-CssClass="TextDate" 
                                        ValidationGroup="Input">
                                        <TextBoxStyle CssClass="TextDate" />
                                    </BDP:BasicDatePicker>
                                    &nbsp;User Complete&nbsp; :&nbsp;
                                    <asp:TextBox ID="tbCUserComplate" runat="server" CssClass="TextBox" Enabled ="False" />
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    SPB Manual&nbsp; No</td>
                                <td>
                                    :</td>
                                <td>
                                    <asp:TextBox ID="tbCSpbManual" Enabled ="False" runat="server" CssClass="TextBox" 
                                        Width="200px" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Trip No</td>
                                <td>
                                    :</td>
                                <td>
                                    <asp:TextBox ID="tbCTripNo" Enabled ="False" runat="server" CssClass="TextBox" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Car No</td>
                                <td>
                                    :</td>
                                <td>
                                    <asp:TextBox ID="tbCCarNo" Enabled ="False" runat="server" AutoPostBack="true" 
                                        CssClass="TextBox" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Customer</td>
                                <td>
                                    :</td>
                                <td>
                                    <asp:TextBox ID="tbCCustomer" Enabled ="False" runat="server" 
                                        CssClass="TextBox" /> 
                                    &nbsp;<asp:TextBox ID="tbCCustomerName" runat="server" Enabled ="False"
                                        CssClass="TextBox" Width="150px" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    TBS (Jenjang)</td>
                                <td>
                                    :</td>
                                <td>
                                    <asp:TextBox ID="tbCTBS" Enabled ="False" runat="server" autoPostBack="true" 
                                        CssClass="TextBox" Width="106px" />
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Brondolan(Krg)&nbsp; :&nbsp;
                                    <asp:TextBox ID="tbCBrondolan" Enabled ="False" runat="server" CssClass="TextBox" 
                                        Width="124px" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Tanggal Berangkat</td>
                                <td>
                                    :</td>
                                <td>
                                    <BDP:BasicDatePicker ID="tbCTglBerangkat" Enabled ="False" runat="server" ButtonImageHeight="19px" 
                                        ButtonImageWidth="20px" DateFormat="dd MMM yyyy" DisplayType="TextBoxAndImage" 
                                        ReadOnly="true" ShowNoneButton="False" TextBoxStyle-CssClass="TextDate" 
                                        ValidationGroup="Input">
                                        <TextBoxStyle CssClass="TextDate" />
                                    </BDP:BasicDatePicker>
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Jam Berangkat :
                                    <asp:TextBox ID="tbCJamBerangkat" Enabled ="False" runat="server" CssClass="TextBox" MaxLength="5" 
                                        Width="56px" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Tanggal Tiba</td>
                                <td>
                                    :</td>
                                <td>
                                    <BDP:BasicDatePicker ID="tbCTiba" runat="server" ButtonImageHeight="19px" 
                                        ButtonImageWidth="20px" DateFormat="dd MMM yyyy" DisplayType="TextBoxAndImage" 
                                        ReadOnly="true" ShowNoneButton="False" TextBoxStyle-CssClass="TextDate" 
                                        ValidationGroup="Input">
                                        <TextBoxStyle CssClass="TextDate" />
                                    </BDP:BasicDatePicker>
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Jam Tiba:
                                    <asp:TextBox ID="tbCJamTiba" runat="server" CssClass="TextBox" MaxLength="5" 
                                        Width="56px" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Tanggal Bongkar</td>
                                <td>
                                    :</td>
                                <td>
                                    <BDP:BasicDatePicker ID="tbCtglBongkar" runat="server" ButtonImageHeight="19px" 
                                        ButtonImageWidth="20px" DateFormat="dd MMM yyyy" DisplayType="TextBoxAndImage" 
                                        ReadOnly="true" ShowNoneButton="False" TextBoxStyle-CssClass="TextDate" 
                                        ValidationGroup="Input">
                                        <TextBoxStyle CssClass="TextDate" />
                                    </BDP:BasicDatePicker>
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Jam Bongkar :
                                    <asp:TextBox ID="tbCJamBongkar" runat="server" CssClass="TextBox" MaxLength="5" 
                                        Width="56px" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Tps</td>
                                <td>
                                    &nbsp;</td>
                                <td>
                                    <table cellpadding="0" cellspacing="0">
                                        <tr style="background-color:Silver;text-align:center">
                                            <td>
                                                Status Tanam</td>
                                            <td>
                                                Price</td>
                                            <td>
                                                +/-</td>
                                            <td>
                                                %</td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="tbCStatusTanam" runat="server" CssClass="TextBox" 
                                                    enabled="false" Width="179px" 
                                                     />
                                            </td>
                                            <td>
                                                <asp:TextBox ID="tbCPrice" runat="server" CssClass="TextBox" 
                                                    enabled="false" Width="84px"  />
                                                </td>
                                            <td>
                                                <asp:TextBox ID="tbCMinPles" runat="server" CssClass="TextBox" enabled="false" Width="86px" 
                                                     />
                                            </td>
                                            <td>
                                                <asp:TextBox ID="tbCPersen" runat="server" CssClass="TextBox" enabled="false" 
                                                    Width="86px"  />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Pabrik</td>
                                <td>
                                    &nbsp;</td>
                                <td>
                                    <table cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td>
                                                <asp:DropDownList ID="ddlPabrik" runat="server" CssClass="TextBox" 
                                                    Height="22px" Width="183px" AutoPostBack="True"  />
                                            </td>
                                            <td>
                                                <asp:TextBox ID="tbCPabrik" runat="server" CssClass="TextBox" 
                                                    Width="83px" AutoPostBack="True" />
                                            </td>
                                            <td>
                                                <asp:TextBox ID="tbCttlPlesMin" runat="server" CssClass="TextBox" enabled="false" Width="86px" 
                                                     />
                                                </td>
                                            <td>
                                                <asp:TextBox ID="tbCttlPersen" runat="server" CssClass="TextBox" enabled="false" 
                                                    Width="86px" />
                                            </td>
                                            <td>
                                                &nbsp;</td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Actual</td>
                                <td>
                                    &nbsp;</td>
                                <td>
                                    <table cellpadding="0" cellspacing="0">
                                        <tr style="background-color:Silver;text-align:center">
                                            <td>
                                                Bruto (Kg)</td>
                                            <td>
                                                Bruto BJR</td>
                                            <td>
                                                Grading (%)</td>
                                            <td>
                                                Grading (Kg)</td>
                                            <td>
                                                Netto (Kg)</td>
                                            <td>
                                                Netto (Bjr)</td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="tbCBrutoKg" runat="server" AutoPostBack="True" CssClass="TextBox"
                                                    Width="86px" />
                                            </td>
                                            <td>
                                                <asp:TextBox ID="tbCBrutoBJR" runat="server" CssClass="TextBox" 
                                                    enabled="false" Width="86px" />
                                            </td>
                                            <td>
                                                <asp:TextBox ID="tbCGrading" runat="server" CssClass="TextBox" AutoPostBack="True"
                                                    Width="86px" />
                                            </td>
                                            <td>
                                                <asp:TextBox ID="tbCGradingKg" runat="server" CssClass="TextBox" AutoPostBack="True"
                                                    Width="86px" />
                                            </td>
                                            <td>
                                                <asp:TextBox ID="tbCNettoKg" Enabled="False" runat="server" CssClass="TextBox" Width="86px" 
                                                     />
                                            </td>
                                            <td>
                                                <asp:TextBox ID="tbcNettoBjr" runat="server" CssClass="TextBox" Width="86px" 
                                                    enabled="false" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Remark</td>
                                <td>
                                    &nbsp;</td>
                                <td>
                                    <table cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="tbCRemark" runat="server" CssClass="TextBox" 
                                                    Width="398px" Height="25px" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Total Grading</td>
                                <td>
                                    &nbsp;</td>
                                <td>
                                    <table cellpadding="0" cellspacing="0">
                                        <tr style="background-color:Silver;text-align:center">
                                            <td>
                                                Rp</td>
                                            <td>
                                                %</td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="tbCttlGrading" runat="server" CssClass="TextBox" enabled="false" 
                                                    Width="110px" Height="16px" />
                                            </td>
                                            <td>
                                                <asp:TextBox ID="tbCPersenGrading" runat="server" CssClass="TextBox" 
                                                    enabled="false" Width="110px" Height="16px" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Price</td>
                                <td>
                                    &nbsp;</td>
                                <td>
                                    <table cellpadding="0" cellspacing="0">
                                        <tr style="background-color:Silver;text-align:center">
                                            <td>
                                                Bongkar</td>
                                            <td>
                                                Kirim</td>
                                            <td>
                                                Muat</td>
                                            <td>
                                                Trip</td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="tbCBongkar" runat="server" CssClass="TextBox" 
                                                    Height="16px" Width="93px" />
                                            </td>
                                            <td>
                                                <asp:TextBox ID="tbCKirim" runat="server" CssClass="TextBox" 
                                                     Width="110px" Height="16px" />
                                            </td>
                                            <td>
                                                <asp:TextBox ID="tbCMuat" runat="server" CssClass="TextBox" 
                                                    Height="16px" Width="81px" />
                                            </td>
                                            <td>
                                                <asp:TextBox ID="tbCTrid" runat="server" CssClass="TextBox" Height="16px" 
                                                    ValidationGroup="Input" Width="81px" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Total</td>
                                <td>
                                    &nbsp;</td>
                                <td>
                                    <table cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="tbCttlBongkar" runat="server" CssClass="TextBox" enabled="false" 
                                                    Width="91px" Height="16px" />
                                            </td>
                                            <td>
                                                <asp:TextBox ID="tbCttlKirim" runat="server" CssClass="TextBox" 
                                                    enabled="false" Width="110px" Height="16px" />
                                            </td>
                                            <td>
                                                <asp:TextBox ID="tbCttlMuat" runat="server" CssClass="TextBox" enabled="false" 
                                                    Height="16px" Width="81px" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td style="vertical-align:top;width:40%">
                        &nbsp;</td>
                </tr>
            </table>
            <br />
            <asp:Button ID="btnComplete" runat="server" class="bitbtn btnsave" 
                CommandName="Ok" Text="Yes" />
            <asp:Button ID="btnCacelCom" runat="server" class="bitbtn btncancel" 
                CommandName="Cancel" Text="Cancel" />
            <br />
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
