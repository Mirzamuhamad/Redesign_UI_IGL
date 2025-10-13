<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TrSptIn.aspx.vb" Inherits="Transaction_TrSptIn_TrSptIn" %>
<%@ Register Assembly="FastReport" Namespace="FastReport.Web" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="BasicFrame.WebControls.BasicDatePicker" Namespace="BasicFrame.WebControls"
    TagPrefix="BDP" %>
<%@ Register assembly="CrystalDecisions.Web, Version=10.5.3700.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>SPT-In (di produksi PKS)</title>
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
    <div class="H1">SPT - IN</div>
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
                      <asp:ListItem Value="dbo.FormatDate(DateAngkut)">Angkut Date</asp:ListItem>
                       <asp:ListItem Value="JamAngkut">Jam</asp:ListItem>
                      <asp:ListItem Value="CarNo">Car No</asp:ListItem>
                      <asp:ListItem Value="Operator">Operator</asp:ListItem>
                      <asp:ListItem Value="TPH">Tujuan</asp:ListItem>
                      <asp:ListItem Value="DivisionName">Division</asp:ListItem>
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
                  <asp:ListItem Value="dbo.FormatDate(DateAngkut)">Angkut Date</asp:ListItem>
                       <asp:ListItem Value="JamAngkut">Jam</asp:ListItem>
                      <asp:ListItem Value="CarNo">Car No</asp:ListItem>
                      <asp:ListItem Value="Operator">Operator</asp:ListItem>
                      <asp:ListItem Value="TPH">Tujuan</asp:ListItem>
                      <asp:ListItem Value="DivisionName">Division</asp:ListItem>
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
                  <asp:BoundField DataField="CarNo" HeaderStyle-Width="120px" HeaderText="Car No" 
                      SortExpression="CarNo">
                      <HeaderStyle Width="120px" />
                  </asp:BoundField>
                  <asp:BoundField DataField="CarName" HeaderStyle-Width="120px" 
                      HeaderText="Car Name" SortExpression="CarName">
                      <HeaderStyle Width="120px" />
                  </asp:BoundField>
                  <asp:BoundField DataField="Operator" HtmlEncode="true" HeaderText="Operator" 
                      SortExpression="Operator"></asp:BoundField>                  
                  <asp:BoundField DataField="OperatorName" HeaderStyle-Width="180px" 
                      HeaderText="Operator Name" SortExpression="OperatorName">
                      <HeaderStyle Width="180px" />
                  </asp:BoundField>
                  <asp:BoundField DataField="TPH" HeaderStyle-Width="120px" 
                      HeaderText="Tujuan" SortExpression="TPH">
                      <HeaderStyle Width="120px" />
                  </asp:BoundField>
                  <asp:BoundField DataField="Division" HeaderText="Division" 
                      SortExpression="Division" />
                  <asp:BoundField DataField="DivisionName" HeaderText="Division Name" 
                      SortExpression="DivisionName" />
                  <asp:BoundField DataField="DateAngkut" HeaderText="Angkut Date"  HtmlEncode="true" DataFormatString="{0:dd MMM yyyy}" 
                      SortExpression="DateAngkut" />
                  <asp:BoundField DataField="JamAngkut" HeaderText="Jam" 
                      SortExpression="JamAngkut" />
                  <asp:BoundField DataField="FgTransit" HeaderText="Transit" 
                      SortExpression="FgTransit" />
                  <asp:BoundField DataField="CarNoTransit" HeaderText="Car No Transit" 
                      SortExpression="CarNoTransit" />
                  <asp:BoundField DataField="CarTransitName" HeaderText="Car No Transit Name" 
                      SortExpression="CarTransitName" />
                  <asp:BoundField DataField="OperatorTransit" HeaderText="OperatorTransit" 
                      SortExpression="OperatorTransit" />
                  <asp:BoundField DataField="OperatorTransitName" HeaderText="Operator Transit" 
                      SortExpression="OperatorTransitName" />
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
            <td colspan="4"><asp:TextBox CssClass="TextBox" Enabled = "false" runat="server" ID="tbRef" Width="149px"/> 
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
            <td colspan="4">
            <BDP:BasicDatePicker ID="tbDate" runat="server" DateFormat="dd MMM yyyy" 
                        ReadOnly = "true" ValidationGroup="Input" 
                        ButtonImageHeight="19px" ButtonImageWidth="20px" 
                        DisplayType="TextBoxAndImage" 
                        TextBoxStyle-CssClass="TextDate" AutoPostBack="True" 
                    ShowNoneButton="False"><TextBoxStyle CssClass="TextDate" /></BDP:BasicDatePicker>                
            </td>                        
            <td>
                Fg Transit</td>
            <td>
                :</td>
            <td>
                <asp:DropDownList ID="ddlTransit" runat="server" CssClass="DropDownList" 
                    ValidationGroup="Input" AutoPostBack="True">
                    <asp:ListItem>Y</asp:ListItem>
                    <asp:ListItem Selected="True">N</asp:ListItem>
                </asp:DropDownList>
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
              <td>Car No</td>
              <td>:</td>
              <td colspan="4">
                  <asp:TextBox ID="tbCar" runat="server" CssClass="TextBox" />
                  <asp:TextBox ID="tbCarName" runat="server" CssClass="TextBoxR" Enabled="False" 
                      Width="200px" />
                  <asp:Button ID="btnCar" runat="server" Class="btngo" Text="..." />
              </td>
              <td>
                  Car Transit</td>
              <td>
                  :</td>
              <td>
                  <asp:TextBox ID="tbCarT" runat="server" CssClass="TextBox" />
                  <asp:TextBox ID="tbCarNameT" runat="server" CssClass="TextBoxR" Enabled="False" 
                      Width="200px" />
                  <asp:Button ID="btnCarT" runat="server" Class="btngo" Text="..." />
              </td>
          </tr>
          <tr>
              <td>
                  Operator</td>
              <td>
                  :</td>
              <td colspan="4">
                  <asp:TextBox ID="tbOp" runat="server" CssClass="TextBox" />
                  <asp:TextBox ID="tbOpName" runat="server" CssClass="TextBoxR" Enabled="False" 
                      Width="200px" />
                  <asp:Button ID="btnOP" runat="server" Class="btngo" Text="..." />
              </td>
              <td>
                  Operator Transit</td>
              <td>
                  :</td>
              <td>
                  <asp:TextBox ID="tbOpT" runat="server" CssClass="TextBox" />
                  <asp:TextBox ID="tbOPNameT" runat="server" CssClass="TextBoxR" Enabled="False" 
                      Width="200px" />
                  <asp:Button ID="btnOPT" runat="server" Class="btngo" Text="..." />
              </td>
          </tr>

          <tr>
            <td>BM</td>
            <td>:</td>
            <td colspan="4">
                <asp:TextBox ID="tbBM" runat="server" CssClass="TextBox" />
                <asp:TextBox ID="tbBMName" runat="server" CssClass="TextBoxR" Enabled="False" 
                    Width="200px" />
                <asp:Button ID="btnBM" runat="server" Class="btngo" Text="..." />
            </td>
            <td>
                BM Transit</td>
            <td>
                :</td>
            <td>
                <asp:TextBox ID="tbBMT" runat="server" CssClass="TextBox" />
                <asp:TextBox ID="tbBMTName" runat="server" CssClass="TextBoxR" Enabled="False" 
                    Width="200px" />
                <asp:Button ID="btnBMT" runat="server" Class="btngo" Text="..." />
            </td>
        </tr>

          <tr>
              <td>
                  Tujuan</td>
              <td>
                  :</td>
              <td>
                  <asp:DropDownList ID="ddlTPH" runat="server" 
                      CssClass="DropDownList" ValidationGroup="Input">
                  </asp:DropDownList>
                  <asp:Label ID="Label2" runat="server" ForeColor="Red">*</asp:Label>
              </td>
              <td>
                  Division</td>
              <td>
                  :</td>
              <td>
                  <asp:DropDownList ID="ddlDivision" Enabled="False" runat="server" CssClass="DropDownList" 
                      ValidationGroup="Input" AutoPostBack="True">
                  </asp:DropDownList>
              </td>
                          
            
           </tr>
           <%--<tr>
               
                <td>
                    Sumber Buah</td>
                <td>
                    :</td>
                <td>
                    <asp:DropDownList ID="ddlSumberBuah" runat="server" 
                        CssClass="DropDownList" ValidationGroup="Input">
                    </asp:DropDownList>
                    <asp:Label ID="SumberBuah" runat="server" ForeColor="Red">*</asp:Label>
                </td>
                </tr>--%>

          <tr>
              <td>
                  Angkut Date / Jam</td>
              <td>
                  :</td>
              <td colspan="4">
                  <BDP:BasicDatePicker ID="tbAngkutDate" runat="server" ButtonImageHeight="19px" 
                      ButtonImageWidth="20px" DateFormat="dd MMM yyyy" DisplayType="TextBoxAndImage" 
                      ReadOnly="true" ShowNoneButton="False" TextBoxStyle-CssClass="TextDate" 
                      ValidationGroup="Input">
                      <TextBoxStyle CssClass="TextDate" />
                  </BDP:BasicDatePicker>
                  /&nbsp;<asp:TextBox ID="tbJam" runat="server" CssClass="TextBox" MaxLength="5" 
                      Width="60px" />
                  <asp:Label ID="Label3" runat="server" ForeColor="Red">*</asp:Label>
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
              <td colspan="4">
                  <asp:TextBox ID="tbRemark" runat="server" CssClass="TextBox" 
                      ValidationGroup="Input" Width="250px" />
              </td>

              <td>Scan QrCode</td>
              <td>:</td>
              <td colspan="4">
                  <asp:TextBox ID="tbScan" autoPostback="true" Enabled="False" runat="server" CssClass="TextBox" 
                      ValidationGroup="Input" Width="250px" />
              </td>

              <td>
                <asp:TextBox ID="tbSuppCek" Enabled="False"  runat="server" CssClass="TextBox" 
                    ValidationGroup="Input" Width="50px" visible ="False" />
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
      <hr style="color:Blue" />  
       <asp:Menu
            ID="Menu1"            
            runat="server"
            CssClass = "Menu"        
            StaticMenuItemStyle-CssClass = "MenuItem"
            StaticSelectedStyle-CssClass = "MenuSelect"        
            Orientation="Horizontal"
            ItemWrap = "False"
            StaticEnableDefaultPopOutImage="False">            
            <Items>
                <asp:MenuItem Text="Detail Colector" Value="0"></asp:MenuItem>
                <asp:MenuItem Text="Detail SPBL" Value="1"></asp:MenuItem>                   
            </Items>            
        </asp:Menu>
        <br />
        <asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex="0">
            <asp:View ID="Tab1" runat="server">
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
                                    <asp:BoundField DataField="WorkBy" HeaderStyle-Width="100px" HeaderText="TK Panen" SortExpression="WorkBy" ></asp:BoundField>
                                    <asp:BoundField DataField="WorkByName" HeaderText="TK Panen Name" HeaderStyle-Width="180px" SortExpression="WorkByName" ></asp:BoundField>
                                    <asp:BoundField DataField="Person" HeaderText="Person" SortExpression="Person" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right"></asp:BoundField>                        
                                    <asp:BoundField DataField="Blok" HeaderText="Blok Code" SortExpression="Blok" ></asp:BoundField>
                                    <asp:BoundField DataField="BlokName" HeaderText="Blok Name" SortExpression="BlokName" ></asp:BoundField>
                                    <asp:BoundField DataField="PanenHK" HeaderText="Hektar Panen" SortExpression="PanenHK" DataFormatString="{0:#,##0.00}"></asp:BoundField>
                                    <asp:BoundField DataField="Ancak" HeaderText="TPH" SortExpression="Ancak"></asp:BoundField>
                                    <asp:BoundField DataField="SPTBSManual" HeaderText="SPTBS Manual" SortExpression="SPTBSManual"></asp:BoundField>
                                    <asp:BoundField DataField="SPTBSDate" HeaderText="SPTBS Date"  HtmlEncode="true" DataFormatString="{0:dd MMM yyyy}" SortExpression="SPTBSDate"></asp:BoundField>
                                    <asp:BoundField DataField="JamSPTBS" HeaderText="SPTBS Jam" SortExpression="JamSPTBS"></asp:BoundField>
                                    <asp:BoundField DataField="FgHariHitam" HeaderText="Hari Hitam" SortExpression="FgHariHitam"></asp:BoundField>
                                    <asp:BoundField DataField="Normal" HeaderText="Normal" SortExpression="Normal" DataFormatString="{0:#,##0}" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right"></asp:BoundField>                        
                                    <asp:BoundField DataField="ABNormal" HeaderText="AbNormal" SortExpression="AbNormal" DataFormatString="{0:#,##0}" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right"></asp:BoundField>                        
                                    <asp:BoundField DataField="Brondolan" HeaderText="Brondolan" SortExpression="Brondolan" DataFormatString="{0:#,##0}" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right"></asp:BoundField>                        
                                    <asp:BoundField DataField="Weight" HeaderText="Weight" SortExpression="Weight" DataFormatString="{0:#,##0}" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right"></asp:BoundField>
                                    <asp:BoundField DataField="Remark" HeaderStyle-Width="80px" HeaderText="Remark" ></asp:BoundField>
                                    
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
                                            <asp:LinkButton ID="lbTKPanen" runat="server" Text="TK Panen" />
                                        </td>
                                        <td>:</td>
                                        <td colspan="4">
                                            <asp:TextBox ID="tbCode" runat="server" AutoPostBack="true" 
                                                CssClass="TextBox" />
                                            <asp:TextBox ID="tbName" runat="server" CssClass="TextBox" Enabled="False" 
                                                EnableTheming="True" ReadOnly="True" Width="200px" />
                                            <asp:Button Class="btngo" ID="btnTKPanen" Text="..." runat="server" />                  
                                        
                                        </td>
                                    </tr>
                                    <tr>
                                        <td >Person</td>
                                        <td >:</td>
                                        <td colspan="4"><asp:TextBox ID="tbPerson" runat="server" CssClass="TextBox" /></td>
                                    </tr>                
                                    <tr>
                                        <td>Block</td>
                                        <td>:</td>
                                        <td colspan="4">
                                            <asp:DropDownList ID="ddlBlock" runat="server" CssClass="DropDownList" 
                                                Width="200px">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>                
                                    <tr>
                                        <td>
                                            Hektar Panen</td>
                                        <td>
                                            :</td>
                                        <td>
                                            <asp:TextBox ID="tbPanenHK" runat="server" CssClass="TextBox" Width="49px" />
                                        </td>
                                        <td>
                                            TPH</td>
                                        <td>
                                            :</td>
                                        <td>
                                            <asp:TextBox ID="tbTPH" runat="server" CssClass="TextBox"  Width="180px" MaxLength="60" />
                                            <asp:Label ID="Label4" runat="server" ForeColor="Red">*</asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            SPTBS Manual</td>
                                        <td>:</td>
                                        <td colspan="4">
                                            <asp:TextBox ID="tbSPTBManual" runat="server" CssClass="TextBox" />
                                        </td>
                                    </tr>
                                    <tr>
                                    <td>SPTBS Date/ Jam</td>
                                    <td>:</td>
                                    <td>
                                        <BDP:BasicDatePicker ID="tbSPTBSDate" runat="server" ButtonImageHeight="19px" 
                                            ButtonImageWidth="20px" AutoPostBack="True"  DateFormat="dd MMM yyyy" DisplayType="TextBoxAndImage" 
                                            ReadOnly="true" ShowNoneButton="False" TextBoxStyle-CssClass="TextDate" 
                                            ValidationGroup="Input">
                                            <TextBoxStyle CssClass="TextDate" />
                                        </BDP:BasicDatePicker>
                                        /
                                        <asp:TextBox ID="tbJamSPTBS" runat="server" CssClass="TextBox" MaxLength="5" 
                                            Width="56px" />
                                        <asp:Label ID="Label1" runat="server" ForeColor="Red">*</asp:Label>
                                    </td>
                                        <td>
                                            Hari Hitam</td>
                                        <td>
                                            :</td>
                                        <td>
                                            <asp:DropDownList ID="ddlFgHariHitam" runat="server" CssClass="DropDownList" 
                                                Width="42px" Height="16px">
                                                <asp:ListItem>Y</asp:ListItem>
                                                <asp:ListItem Selected="True">N</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Nominal</td>
                                        <td>
                                            &nbsp;</td>
                                        <td colspan="4">
                                            <table cellpadding="0" cellspacing="0">
                                                <tr style="background-color:Silver;text-align:center">
                                                    <td>
                                                        Normal (TBS)</td>
                                                    <td>
                                                        Abnormal (TBS)</td>
                                                    <td>
                                                        Brondolan (Krg)</td>

                                                        <td>
                                                            Weight (Kg)</td>
                                                        
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:TextBox ID="tbNormal" runat="server" CssClass="TextBox" 
                                                            ValidationGroup="Input" Width="110px" />
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="tbAbnormal" runat="server" CssClass="TextBox" 
                                                            ValidationGroup="Input" Width="110px" />
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="tbBrondolan" runat="server" CssClass="TextBox" Height="16px" 
                                                            ValidationGroup="Input" Width="81px" />
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="tbWeightDt" runat="server" CssClass="TextBox" Height="16px" 
                                                            ValidationGroup="Input" Width="81px" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Remark
                                        </td>
                                        <td>
                                            :</td>
                                        <td colspan="4">
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
            </asp:View>


            <asp:View ID ="Tab2" runat="server">
                <div style="font-size:medium; color:Blue;">Detail SPBL</div>
                <hr style="color:Blue" />  
                    <asp:Panel runat="server" ID="pnlDt2">
                        <asp:Button class="bitbtn btnadd" runat="server" ID="btnAddDtKe1" Text="Add" ValidationGroup="Input" />	     
                            
                        <div style="border:0px  solid; width:100%; height:100%; overflow:auto;">
                            <asp:GridView ID="GridDt2" runat="server" AutoGenerateColumns="False" 
                                ShowFooter="True">
                                <HeaderStyle CssClass="GridHeader" />
                                <RowStyle CssClass="GridItem" Wrap="false" />
                                <AlternatingRowStyle CssClass="GridAltItem" />
                                <PagerStyle CssClass="GridPager" />
                                <Columns>
                                    <asp:TemplateField HeaderText="Action">
                                        <ItemTemplate>
                                            <asp:Button class="bitbtn btnedit" runat="server" ID="btnEdit2" Text="Edit" CommandName="Edit" />																						 											
                                            <asp:Button class="bitbtn btndelete" runat="server" ID="btnDelete2" Text="Delete" CommandName="Delete"  OnClientClick="return confirm('Sure to delete this data?');" />																						 													     
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                                <asp:Button class="bitbtn btnsave" runat="server" ID="btnUpdate2" Text="Save" CommandName="Update" />																						 																											
                                                
                                                <asp:Button class="bitbtn btncancel" runat="server" ID="btnCancel2" Text="Cancel" CommandName="Cancel" />																						 													
                                            
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Button ID="btnClosing2" runat="server" CssClass="Button" Text="Closing"                                             
                                                        CommandArgument='<%# Container.DataItemIndex %>'
                                                        CommandName="Closing" />
                                                        <%--CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>"--%>
                                            </ItemTemplate>
                                    </asp:TemplateField>     
                                    <asp:BoundField DataField="SuppCode" HeaderStyle-Width="100px" HeaderText="Supplier" SortExpression="SuppCode" ></asp:BoundField>
                                    <asp:BoundField DataField="SupplierName" HeaderText="Supplier Name" HeaderStyle-Width="180px" SortExpression="SupplierName" ></asp:BoundField>
									
                                    <asp:BoundField DataField="Person" HeaderText="Person" SortExpression="Person" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right"></asp:BoundField>                        
                                    <asp:BoundField DataField="PanenHK" HeaderText="Hektar Panen" SortExpression="PanenHK" DataFormatString="{0:#,##0.00}"></asp:BoundField>
                                    <asp:BoundField DataField="Ancak" HeaderText="TPH" SortExpression="Ancak"></asp:BoundField>
                                    <asp:BoundField DataField="SPBLManual" HeaderText="SPBL Manual" SortExpression="SPBLManual"></asp:BoundField>
                                    <asp:BoundField DataField="SPBLDate" HeaderText="SPTBS Date"  HtmlEncode="true" DataFormatString="{0:dd MMM yyyy}" SortExpression="SPBLDate"></asp:BoundField>
                                    <asp:BoundField DataField="JamSPBL" HeaderText="SPBL Jam" SortExpression="JamSPBL"></asp:BoundField>
                                    <asp:BoundField DataField="Normal" HeaderText="TBS Normal" SortExpression="Normal" DataFormatString="{0:#,##0}" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right"></asp:BoundField>                        
                                    <asp:BoundField DataField="AbNormal" HeaderText="TBS AbNormal" SortExpression="Abnormal" DataFormatString="{0:#,##0}" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right"></asp:BoundField>                        
                                    <asp:BoundField DataField="Brondolan" HeaderText="Brondolan" SortExpression="Brondolan" DataFormatString="{0:#,##0}" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right"></asp:BoundField>
                                    <asp:BoundField DataField="Weight" HeaderText="Weight" SortExpression="Weight" DataFormatString="{0:#,##0}" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right"></asp:BoundField>                        
                                    <asp:BoundField DataField="VehicleType" HeaderStyle-Width="100px" HeaderText="IKP" SortExpression="VehicleType" ></asp:BoundField>
                                    <asp:BoundField DataField="VehicleName" HeaderText="IKP Name" HeaderStyle-Width="180px" SortExpression="VehicleName" ></asp:BoundField>
									<asp:BoundField DataField="Remark" HeaderStyle-Width="80px" HeaderText="Remark" ></asp:BoundField>
                                </Columns>
                            </asp:GridView>
                            </div>   
                            
                        <asp:Button class="bitbtn btnadd" runat="server" ID="btnAddDtKe2" Text="Add" ValidationGroup="Input" />	     
                    
                    </asp:Panel>             
                    <asp:Panel runat="server" ID="pnlEditDt2" Visible="false">
                 <table width="100%">
                      <tr>
                          <td style="width:60%">                
                              <table>
                                  <tr>
                                      <td>
                                          <asp:LinkButton ID="Supplier" runat="server" Text="Supplier" />
                                      </td>
                                      <td>:</td>
                                      <td colspan="4">
                                          <asp:TextBox ID="tbSuppcode" runat="server" AutoPostBack="true" 
                                              CssClass="TextBox" />
                                          <asp:TextBox ID="tbSuppName" runat="server" CssClass="TextBox" Enabled="False" 
                                              EnableTheming="True" ReadOnly="True" Width="200px" />
                                          <asp:Button Class="btngo" ID="btnSupplier" Text="..." runat="server" />                  
                                       
                                      </td>
                                  </tr>
                                  <tr>
                                      
                                    <%-- <td visible="False">Person</td>
                                    <td visible="False">:</td> --%>
                                    <td  colspan="4"><asp:TextBox visible="False" ID="tbPersonDt2" runat="server" CssClass="TextBox" /></td>
                                  </tr>                
                                  <tr>
                                      <td>Vechile</td>
                                      <td>:</td>
                                      <td colspan="4">
                                          <asp:DropDownList ID="ddlVechile" runat="server" CssClass="DropDownList" 
                                              Width="200px">
                                          </asp:DropDownList>
                                      </td>
                                  </tr>                
                                  <tr>
                                    <td>
                                        TPH</td>
                                    <td>
                                        :</td>
                                    <td>
                                        <asp:TextBox ID="tbTPHDt2" runat="server" CssClass="TextBox"  Width="180px" MaxLength="60" />
                                         <asp:Label ID="Label4Dt2" runat="server" ForeColor="Red">*</asp:Label>
                                    </td>
                                    <%--<td visible="False">
                                          Hektar Panen</td>
                                      <td visible="False">
                                          :</td>--%>
                                      <td >
                                          <asp:TextBox ID="tbPanenHKDt2" visible="False" runat="server" CssClass="TextBox" Width="49px" />
                                      </td>
                                      
                                  </tr>
                                  <tr>
                                      <td>
                                          SPTBS Manual</td>
                                      <td>:</td>
                                      <td colspan="4">
                                          <asp:TextBox ID="tbSPBLManual" runat="server" CssClass="TextBox" />
                                      </td>
                                  </tr>
                                  <tr>
                                    <%--<td visible="False">SPTBS Date/ Jam</td>
                                  <td visible="False"> :</td> --%>
                                  <td >
                                      <BDP:BasicDatePicker visible="False" ID="tbSPBLDate" runat="server" ButtonImageHeight="19px" 
                                          ButtonImageWidth="20px" DateFormat="dd MMM yyyy" DisplayType="TextBoxAndImage" 
                                          ReadOnly="true" ShowNoneButton="False" TextBoxStyle-CssClass="TextDate" 
                                          ValidationGroup="Input">
                                          <TextBoxStyle CssClass="TextDate" />
                                      </BDP:BasicDatePicker>
                                      
                                      <asp:TextBox ID="tbJamSPBL" visible="False" runat="server" CssClass="TextBox" MaxLength="5" 
                                          Width="56px" />
                                      <asp:Label ID="Label1Dt2" visible="False" runat="server" ForeColor="Red">*</asp:Label>
                                  </td>
                                  <%--<td>
                                          Hari Hitam</td>
                                      <td>
                                          :</td> --%>
                                      <td>
                                          <asp:DropDownList ID="ddlFgHariHitamDt2" visible="False" runat="server" CssClass="DropDownList" 
                                              Width="42px" Height="16px">
                                              <asp:ListItem>Y</asp:ListItem>
                                              <asp:ListItem Selected="True">N</asp:ListItem>
                                          </asp:DropDownList>
                                      </td>
                                  </tr>
                                  <tr>
                                      <td>
                                          Nominal</td>
                                      <td>
                                          &nbsp;</td>
                                      <td colspan="4">
                                          <table cellpadding="0" cellspacing="0">
                                              <tr style="background-color:Silver;text-align:center">
                                                  <td>
                                                      Normal (TBS)</td>
                                                  <td>
                                                      Abnormal (TBS)</td>
                                                  <td>
                                                      Brondolan (Krg)</td>
                                                  <td>
                                                        Weight (Kg) </td>
                                              </tr>
                                              <tr>
                                                  <td>
                                                      <asp:TextBox ID="tbNormalDt2" runat="server" CssClass="TextBox" 
                                                          ValidationGroup="Input" Width="110px" />
                                                  </td>
                                                  <td>
                                                      <asp:TextBox ID="tbAbnormalDt2" runat="server" CssClass="TextBox" 
                                                          ValidationGroup="Input" Width="110px" />
                                                  </td>
                                                  <td>
                                                      <asp:TextBox ID="tbBrondolanDt2" runat="server" CssClass="TextBox" Height="16px" 
                                                          ValidationGroup="Input" Width="81px" />
                                                  </td>
                                                  <td>
                                                    <asp:TextBox ID="tbWeight" runat="server" CssClass="TextBox" Height="16px" 
                                                        ValidationGroup="Input" Width="81px" />
                                                </td>
                                              </tr>
                                          </table>
                                      </td>
                                  </tr>
                                  <tr>
                                      <td>
                                          Remark
                                      </td>
                                      <td>
                                          :</td>
                                      <td colspan="4">
                                          <asp:TextBox ID="tbRemarkDt2" runat="server" CssClass="TextBox" Width="405" />
                                      </td>
                                  </tr>
                              </table>
                          </td>
                          <td style="vertical-align:top;width:40%">
                              &nbsp;</td>
                      </tr>
                 </table>
                      <br />
                      <asp:Button class="bitbtn btnsave" runat="server" ID="btnSaveDt2" Text="Save" CommandName="Update" />																						 																											
                                              
                      <asp:Button class="bitbtn btncancel" runat="server" ID="btnCancelDt2" Text="Cancel" CommandName="Cancel" />																						 													
           
                      <br />
                 </asp:Panel> 
                 <br />  
                   
            </asp:View>
        </asp:MultiView>
      
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
	<br/>
    <asp:Label runat ="server" ID="lbStatus"   ForeColor="Red"/>
    <asp:HiddenField ID="HiddenRemarkClose" runat="server" />
    <asp:HiddenField ID="HiddenRemarkDelete" runat="server" />    
    </form>                            
    </body>
</html>
