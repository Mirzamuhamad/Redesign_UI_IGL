<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TrSPB.aspx.vb" Inherits="Transaction_TrSPB_TrSPB" %>
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
    <div class="H1">S P B</div>
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
                  <asp:BoundField DataField="TPSGrading" HeaderText="TBS (Janjang) Grading"  DataFormatString="{0:#,##0.00}"
                      SortExpression="TPSGrading" />
                  <asp:BoundField DataField="TotalKirim" HeaderText="TBS (Janjang) Kirim"  DataFormatString="{0:#,##0.00}"
                      SortExpression="TotalKirim" />
                  <asp:BoundField DataField="BalanceTBS" HeaderText="TBS (Janjang) Balance"  DataFormatString="{0:#,##0.00}"
                      SortExpression="BalanceTBS" />
                  <asp:BoundField DataField="BrondolanKgs" HeaderStyle-Width="120px" HeaderText="Brondolan Kgs (TPS)"  DataFormatString="{0:#,##0.00}"
                      SortExpression="BrondolanKgs">
                      <HeaderStyle Width="120px" />
                  </asp:BoundField>
                  <asp:BoundField DataField="BrdKirim" HeaderStyle-Width="120px"  DataFormatString="{0:#,##0.00}"
                      HeaderText="Brondolan Kgs Kirim" SortExpression="BrdKirim">
                      <HeaderStyle Width="120px" />
                  </asp:BoundField>
                  <asp:BoundField DataField="BrdGrading" HtmlEncode="true" HeaderText="Brondolan Kgs Grading"  DataFormatString="{0:#,##0.00}"
                      SortExpression="BrdGrading"></asp:BoundField>                  
                  <asp:BoundField DataField="BalanceBrd" HeaderText="Brondolan Kgs Balance"  DataFormatString="{0:#,##0.00}"
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
            <BDP:BasicDatePicker ID="tbDateMuat" runat="server" DateFormat="dd MMM yyyy" 
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
             <BDP:BasicDatePicker ID="tbDate" runat="server" DateFormat="dd MMM yyyy" 
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
                  <asp:DropDownList ID="ddlEstate" runat="server" Width="142px" Height="16px" AutoPostBack="true"
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
                              <asp:TextBox ID="tbTGrading" runat="server" CssClass="TextBox" Height="16px" 
                                  Width="91px" AutoPostBack="True" />
                          </td>
                           <td>
                              <asp:TextBox ID="tbBalanceTBS" runat="server" CssClass="TextBoxR" Width="110px" 
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
                              <asp:TextBox ID="tbbrdTPS" runat="server" CssClass="TextBoxR" Width="110px" 
                                  Enabled="False" />
                          </td>
                          <td>
                              <asp:TextBox ID="tbbrdKirim" runat="server" CssClass="TextBoxR" Width="110px" 
                                  Enabled="False" />
                          </td>
                          <td>
                              <asp:TextBox ID="tbbrdGrading" runat="server" CssClass="TextBox" Height="16px" 
                                  Width="91px" AutoPostBack="True" />
                          </td>
                           <td>
                              <asp:TextBox ID="TbbrdBalance" runat="server" CssClass="TextBoxR" Width="110px" 
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
            <hr style="color: Blue" />
            <div style="font-size:medium; color:Blue;">Detail</div>
            <br>
        <asp:Menu ID="Menu1" runat="server" CssClass="Menu" ItemWrap="False" 
            Orientation="Horizontal" StaticEnableDefaultPopOutImage="False" 
            StaticMenuItemStyle-CssClass="MenuItem" 
            StaticSelectedStyle-CssClass="MenuSelect">
            <StaticSelectedStyle CssClass="MenuSelect" />
            <StaticMenuItemStyle CssClass="MenuItem" />
            <Items>
                <asp:MenuItem Text="Detail SPB" Value="0"></asp:MenuItem>
                <asp:MenuItem Text="SPTBS" Value="1"></asp:MenuItem>
                <%--<asp:MenuItem Text="Schedule Job Detail" Value="3" ></asp:MenuItem>--%>
            </Items>
        </asp:Menu>
        <hr style="color:Blue" />
        <asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex="0">
            <asp:View ID="Tab1" runat="server">
                <asp:Panel ID="pnlDt" runat="server">
                    <asp:Button ID="btnAddDt" runat="server" class="bitbtn btnadd" Text="Add" 
                        ValidationGroup="Input" />
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
                                        <asp:Button ID="btnEdit" runat="server" class="bitbtn btnedit" 
                                            CommandName="Edit" Text="Edit" />
                                        <asp:Button ID="btnDelete" runat="server" class="bitbtn btndelete" 
                                            CommandName="Delete" 
                                            OnClientClick="return confirm('Sure to delete this data?');" Text="Delete" />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Button ID="btnUpdate" runat="server" class="bitbtn btnsave" 
                                            CommandName="Update" Text="Save" />
                                        <asp:Button ID="btnCancel" runat="server" class="bitbtn btncancel" 
                                            CommandName="Cancel" Text="Cancel" />
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:Button ID="btnClosing" runat="server" 
                                            CommandArgument="<%# Container.DataItemIndex %>" CommandName="Closing" 
                                            CssClass="Button" Text="Closing" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="SPBManualNo" HeaderStyle-Width="100px" 
                                    HeaderText="SPB Manual No" SortExpression="SPBManualNo" />
                                <asp:BoundField DataField="TripNo" HeaderStyle-Width="180px" 
                                    HeaderText="TripNo" SortExpression="Trip No" />
                                <asp:BoundField DataField="Customer" FooterStyle-HorizontalAlign="Right" 
                                    HeaderText="Customer" ItemStyle-HorizontalAlign="Right" 
                                    SortExpression="Customer" />
                                <asp:BoundField DataField="CustomerName" HeaderText="Customer Name" 
                                    SortExpression="CustomerName" />
                                <asp:BoundField DataField="CarNo" HeaderText="Car No" SortExpression="CarNo" />
                                <asp:BoundField DataField="CarName" HeaderText="Car Name" 
                                    SortExpression="CarName" />
                                <asp:BoundField DataField="Owner" HeaderText="Pemilik Kapal" 
                                    SortExpression="Owner" />
                                <asp:BoundField DataField="Pengawas" HeaderText="Pengawas" 
                                    SortExpression="Pengawas" />
                                <asp:BoundField DataField="PengawasName" HeaderText="Pengawas Name" 
                                    SortExpression="PengawasName" />
                                <asp:BoundField DataField="DateBerangkat" DataFormatString="{0:dd MMM yyyy}" 
                                    HeaderText="Date Berangkat" HtmlEncode="true" SortExpression="DateBerangkat" />
                                <asp:BoundField DataField="JamBerangkat" HeaderText="JamBerangkat" 
                                    SortExpression="JamBerangkat" />
                                <asp:BoundField DataField="StatusTanam" HeaderText="Status Tanam" 
                                    SortExpression="StatusTanam" />
                                <asp:BoundField DataField="StatusTanamName" HeaderText="Status Tanam Name" 
                                    SortExpression="StatusTanamName" />
                                <asp:BoundField DataField="PriceTPS" DataFormatString="{0:#,##0.00}" 
                                    HeaderText="Price TBS" SortExpression="PriceTPS" />
                                <asp:BoundField DataField="FactorRate" DataFormatString="{0:#,##0.00}" 
                                    FooterStyle-HorizontalAlign="Right" HeaderText="Factor Rate" 
                                    ItemStyle-HorizontalAlign="Right" SortExpression="FactorRate" />
                                <asp:BoundField DataField="BJR" DataFormatString="{0:#,##0.00}" 
                                    FooterStyle-HorizontalAlign="Right" HeaderText="BJR" 
                                    ItemStyle-HorizontalAlign="Right" SortExpression="BJR" />
                                <asp:BoundField DataField="QtyJanjang" DataFormatString="{0:#,##0}" 
                                    FooterStyle-HorizontalAlign="Right" HeaderText="Qty Janjang" 
                                    ItemStyle-HorizontalAlign="Right" SortExpression="QtyJanjang" />
                                <asp:BoundField DataField="QtyBrondolan" DataFormatString="{0:#,##0}" 
                                    FooterStyle-HorizontalAlign="Right" HeaderText="Brondolan (Krg)" 
                                    ItemStyle-HorizontalAlign="Right" SortExpression="QtyBrondolan" />
                            </Columns>
                        </asp:GridView>
                    </div>
                    <asp:Button ID="btnAddDt2" runat="server" class="bitbtn btnadd" Text="Add" 
                        ValidationGroup="Input" />
                </asp:Panel>
                <asp:Panel ID="pnlEditDt" runat="server" Visible="false">
                    <table width="100%">
                        <tr>
                            <td style="width:60%">
                                <table>
                                    <tr>
                                        <td>
                                            SPB Manual
                                        </td>
                                        <td>
                                            :</td>
                                        <td colspan="4">
                                            <asp:TextBox ID="tbSPBManual" runat="server" CssClass="TextBox" Width="200px" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Trip No</td>
                                        <td>
                                            :</td>
                                        <td colspan="4">
                                            <asp:TextBox ID="tbTrip" runat="server" CssClass="TextBox" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Customer</td>
                                        <td>
                                            :</td>
                                        <td colspan="4">
                                            <asp:TextBox ID="tbCustCode" runat="server" AutoPostBack="true" 
                                                CssClass="TextBox" />
                                            <asp:TextBox ID="tbCustName" runat="server" CssClass="TextBox" Enabled="False" 
                                                EnableTheming="True" ReadOnly="True" Width="200px" />
                                            <asp:Button ID="btnCust" runat="server" Class="btngo" Text="..." />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Kapal</td>
                                        <td>
                                            :</td>
                                        <td>
                                            <asp:TextBox ID="tbKapal" runat="server" AutoPostBack="true" 
                                                CssClass="TextBox" />
                                            <asp:TextBox ID="tbKapalName" runat="server" CssClass="TextBox" Enabled="False" 
                                                EnableTheming="True" ReadOnly="True" Width="200px" />
                                            <asp:Button ID="btnCar" runat="server" Class="btngo" Text="..." />
                                        </td>
                                        <td>
                                            Pemilik Kapal</td>
                                        <td>
                                            :</td>
                                        <td>
                                            <asp:TextBox ID="tbOwner" runat="server" CssClass="TextBox" enabled="false" 
                                                Width="200px" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Pengawas</td>
                                        <td>
                                            :</td>
                                        <td colspan="4">
                                            <asp:TextBox ID="tbPengawasCode" runat="server" autoPostBack="true" 
                                                CssClass="TextBox" />
                                            <asp:TextBox ID="tbPengawasName" runat="server" CssClass="TextBox" 
                                                enabled="false" Width="200px" />
                                            <asp:Button ID="btnPengawas" runat="server" Class="btngo" Text="..." />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Tanggal/ Jam Berangkat</td>
                                        <td>
                                            :</td>
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
                                            <asp:DropDownList ID="ddlstatusTanam" runat="server" AutoPostBack="true" 
                                                CssClass="DropDownList" Height="16px" ValidationGroup="Input" Width="142px">
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
                                                        <asp:TextBox ID="tbPriceTPS" runat="server" CssClass="TextBox" enabled="false" 
                                                            Width="110px" />
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="tbFactorRate" runat="server" CssClass="TextBox" 
                                                            enabled="false" Width="110px" />
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="tbBJR" runat="server" CssClass="TextBox" enabled="false" 
                                                            Height="16px" Width="81px" />
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="tbQtyJanjang" runat="server" CssClass="TextBox" Height="16px" 
                                                            ValidationGroup="Input" Width="81px" />
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="tbQtyBrondolan" runat="server" CssClass="TextBox" 
                                                            Height="16px" ValidationGroup="Input" Width="81px" />
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
                    <asp:Button ID="btnSaveDt" runat="server" class="bitbtn btnsave" 
                        CommandName="Update" Text="Save" />
                    <asp:Button ID="btnCancelDt" runat="server" class="bitbtn btncancel" 
                        CommandName="Cancel" Text="Cancel" />
                    <br />
                </asp:Panel>
            </asp:View>
            <asp:View ID="Tab2" runat="server">
                <asp:Panel ID="pnlDt2" runat="server">
                    <asp:Button class="bitbtn btnadd" runat="server" ID="btnAddDt2ke1" Text="Add" ValidationGroup="Input" />
                    &nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:Button ID="btnGetsptbs" runat="server" class="bitbtn btnadd" Text="Get Data" />
                    <div style="border: 0px  solid; width: 100%; height: 100%; overflow: auto;">
                        <asp:GridView ID="GridDt2" runat="server" AutoGenerateColumns="False" 
                            ShowFooter="True">
                            <HeaderStyle CssClass="GridHeader" />
                            <RowStyle CssClass="GridItem" Wrap="false" />
                            <AlternatingRowStyle CssClass="GridAltItem" />
                            <PagerStyle CssClass="GridPager" />
                            <Columns>
                                <asp:TemplateField HeaderText="Action">
                                    <ItemTemplate>
                                        <asp:Button ID="btnEdit" runat="server" class="bitbtndt btnedit" 
                                            CommandName="Edit" Text="Edit" />
                                        <asp:Button ID="btnDelete" runat="server" class="bitbtndt btndelete" 
                                            CommandName="Delete" 
                                            OnClientClick="return confirm('Sure to delete this data?');" Text="Delete" />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Button ID="btnUpdate" runat="server" class="bitbtndt btnsave" 
                                            CommandName="Update" Text="Save" />
                                        <asp:Button ID="btnCancel" runat="server" class="bitbtndt btncancel" 
                                            CommandName="Cancel" Text="Cancel" />
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="SptbsNo" HeaderStyle-Width="150px" 
                                    HeaderText="SPTBS No" SortExpression="SptbsNo">
                                    <HeaderStyle Width="200px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Car_No" HeaderText="Car No" 
                                    SortExpression="Car_No">
                                    <HeaderStyle Width="100px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="CarName" HeaderStyle-Width="150px" 
                                    HeaderText="Car Name" SortExpression="CarName">
                                    <HeaderStyle Width="200px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Operator" HeaderStyle-Width="60px" 
                                    HeaderText="Operator" ItemStyle-HorizontalAlign="Left" 
                                    SortExpression="Operator">
                                    <HeaderStyle Width="150px" />
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:BoundField DataField="AngkutDate" DataFormatString="{0:dd MMM yyyy}" 
                                    HeaderStyle-Width="100px" HeaderText="Angkut Date" SortExpression="AngkutDate">
                                    <HeaderStyle Width="100px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="AngkutTime" HeaderStyle-Width="100px" 
                                    HeaderText="Angkut Time" SortExpression="AngkutTime">
                                    <HeaderStyle Width="100px" />
                                </asp:BoundField>
                            </Columns>
                        </asp:GridView>
                    </div>
                    <asp:Button ID="btnAddDt2ke2" runat="server" class="bitbtn btnadd" Text="Add" 
                        ValidationGroup="Input" />
                </asp:Panel>
                <asp:Panel ID="PnlEditDt2" runat="server" Visible="false">
                    <table>
                        <tr>
                            <td>
                                Sptbs No</td>
                            <td>
                                :
                            </td>
                            <td>
                                <asp:TextBox ID="tbsptbsNo" runat="server" AutoPostBack="true" 
                                    CssClass="TextBox" MaxLength="20" />
                                <asp:Button ID="Btnsptbs" runat="server" Class="btngo" Text="..." 
                                    ValidationGroup="Input" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Car No</td>
                            <td>
                                :</td>
                            <td>
                                <asp:TextBox ID="tbCarno" runat="server" CssClass="TextBox" MaxLength="60" 
                                    Width="80px" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Operator</td>
                            <td>
                                :</td>
                            <td>
                                <asp:TextBox ID="tbOperator" runat="server" CssClass="TextBox" MaxLength="60" 
                                    Width="80px" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Date / Time Angkut</td>
                            <td>
                                :</td>
                            <td>
                                <BDP:BasicDatePicker ID="tbAngkutDate" runat="server" ButtonImageHeight="19px" 
                                    ButtonImageWidth="20px" DateFormat="dd MMM yyyy" DisplayType="TextBoxAndImage" 
                                    ReadOnly="true" ShowNoneButton="False" TextBoxStyle-CssClass="TextDate" 
                                    ValidationGroup="Input">
                                    <TextBoxStyle CssClass="TextDate" />
                                </BDP:BasicDatePicker>
                                /
                                <asp:TextBox ID="tbAngkutTime" runat="server" CssClass="TextBox" MaxLength="60" 
                                    Width="80px" />
                            </td>
                        </tr>
                    </table>
                    <br />
                    <asp:Button ID="btnSaveDt2" runat="server" class="bitbtndt btnsave" Text="Save" />
                    <asp:Button ID="btnCancelDt2" runat="server" class="bitbtndt btncancel" 
                        Text="Cancel" />
                </asp:Panel>
            </asp:View>
        </asp:MultiView>
        <br />
        <asp:Button ID="btnSaveAll" runat="server" class="bitbtndt btnsavenew" 
            Text="Save &amp; New" validationgroup="Input" Width="90" />
        <asp:Button ID="btnSaveTrans" runat="server" class="bitbtndt btnsave" 
            Text="Save" validationgroup="Input" />
        <asp:Button ID="btnBack" runat="server" class="bitbtndt btncancel" 
            Text="Cancel" validationgroup="Input" />
        <asp:Button ID="btnHome" runat="server" class="bitbtndt btnback" Text="Home" />
        <br>
        
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
