<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TrPDMR.Aspx.vb" Inherits="TrPDMR" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<%@ Register Assembly="BasicFrame.WebControls.BasicDatePicker" Namespace="BasicFrame.WebControls"
    TagPrefix="BDP" %>

<%--<%@ Register assembly="CrystalDecisions.Web, Version=12.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>--%>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
    <script src="../../Function/OpenDlg.js" type="text/javascript"></script>
    <script src="../../Function/Function.JS" type="text/javascript"></script>
    <script type="text/javascript"> 
        function openprintdlgs()
        {
            var wOpens;
            wOpens = window.open("../../Rpt/PrintForm2.Aspx", "List","scrollbars=yes,resizable=yes,width=500,height=400");            
            wOpens.moveTo(0, 0);
            wOpens.resizeTo(screen.width, screen.height);                        
            return false; 
        }   
        
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
            var Qty = document.getElementById("tbQtyreq").value.replace(/\$|\,/g,""); 
            var QtyWrhs = document.getElementById("tbQtyWrhs").value.replace(/\$|\,/g,""); 
//             var PriceForex = document.getElementById("tbPrice").value.replace(/\$|\,/g,"");
//             var AmountForex = document.getElementById("tbAmountForex").value.replace(/\$|\,/g,"");                
         try
         {                       
            document.getElementById("tbQtyreq").value = setdigit(Qty,'<%=ViewState("DigitQty")%>');
            document.getElementById("tbQtyWrhs").value = setdigit(QtyWrhs,'<%=ViewState("DigitQty")%>');
            // document.getElementById("tbPrice").value = setdigit(PriceForex,'<%=VIEWSTATE("DigitCurr")%>');
            // document.getElementById("tbAmountForex").value = setdigit(AmountForex,'<%=VIEWSTATE("DigitCurr")%>');
        }catch (err){
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
        .style4
        {
            width: 72px;
        }
        .style6
        {
            width: 3px;
        }
        .style7
        {
            width: 84px;
        }
        .style8
        {
            width: 201px;
        }
    </style>
</head>
<body>     
    <form id="form1" runat="server">    
    <div class="Content">
    <div class="H1">Material Request</div>
     <hr style="color:Blue" />        
    <asp:Panel runat="server" id="PnlHd"> 
      <table>
        <tr>
            <td style="width:100px;text-align:right">Quick Search :</td>            
            <td><asp:TextBox CssClass="TextBox" runat="server" ID ="tbFilter"/> 
                  <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlField" >
                      <asp:ListItem Selected="True" Value="TransNmbr">MR No</asp:ListItem>                      
                      <asp:ListItem Value="dbo.FormatDate(TransDate)">MR Date</asp:ListItem>
                      <asp:ListItem>Status</asp:ListItem>                   
                      <asp:ListItem Value="MRType">MR Type</asp:ListItem>
                      <asp:ListItem Value="TTReturNo">Return Mtr No</asp:ListItem>
                      <asp:ListItem Value="WorkCtrName">Work Center</asp:ListItem>
                      <asp:ListItem Value="WrhsName">Warehouse</asp:ListItem>
                      <asp:ListItem>Remark</asp:ListItem>
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
                      <asp:ListItem Selected="True" Value="TransNmbr">MR No</asp:ListItem>                      
                      <asp:ListItem Value="dbo.FormatDate(TransDate)">MR Date</asp:ListItem>
                      <asp:ListItem>Status</asp:ListItem>                   
                      <asp:ListItem Value="MRType">MR Type</asp:ListItem>
                      <asp:ListItem Value="TTReturNo">Return Mtr No</asp:ListItem>
                      <asp:ListItem Value="WorkCtrName">Work Center</asp:ListItem>
                      <asp:ListItem Value="WrhsName">Warehouse</asp:ListItem>
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
                          <asp:Button class="btngo" runat="server" ID="BtnGo" Text="G" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" CommandName="Go" />                      </ItemTemplate>
                      <HeaderStyle Width="110px" />
                  </asp:TemplateField>
                  <asp:BoundField DataField="TransNmbr" HeaderStyle-Width="120px" SortExpression="Nmbr" HeaderText="MR No"></asp:BoundField>                  
                  <asp:BoundField DataField="Status" HeaderText="Status"></asp:BoundField>                  
                  <asp:BoundField DataField="TransDate" dataformatstring="{0:dd MMM yyyy}" htmlencode="true" HeaderStyle-Width="80px" SortExpression="TransDate" HeaderText="MR Date"></asp:BoundField>
                  <asp:BoundField DataField="MRType" HeaderStyle-Width="80px" SortExpression="MRType" HeaderText="MR Type"></asp:BoundField>
                  <asp:BoundField DataField="TTReturNo" HeaderStyle-Width="120px" SortExpression="TTReturNo" HeaderText="Return Mtr No"></asp:BoundField>
                  <asp:BoundField DataField="WorkCtrName" HeaderStyle-Width="180px" SortExpression="WorkCtrName" HeaderText="Work Center"></asp:BoundField>
                  <asp:BoundField DataField="WrhsName" HeaderStyle-Width="180px" SortExpression="WrhsName" HeaderText="Warehouse"></asp:BoundField>                  
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
      <table style="width: 616px">
        <tr>
            <td>MR No</td>
            <td>:</td>
            <td><asp:TextBox CssClass="TextBox" Enabled ="false" runat="server" ID="tbTransNo" Width="149px"/>
                <asp:Label ID="label15" runat="server" Font-Underline="False" 
                    ForeColor="#FF3300" Text="*" />
            </td>            
        </tr>
        <tr>
            <td>MR Date</td>
            <td>:</td>
            <td>
            <BDP:BasicDatePicker ID="tbDate" runat="server" DateFormat="dd MMM yyyy" 
                        ShowNoneButton ="false" ReadOnly = "true" ValidationGroup="Input"
                        ButtonImageHeight="19px" ButtonImageWidth="20px" 
                        DisplayType="TextBoxAndImage" 
                        TextBoxStyle-CssClass="TextDate" AutoPostBack="True"><TextBoxStyle CssClass="TextDate" /></BDP:BasicDatePicker>                
                <asp:Label ID="label16" runat="server" Font-Underline="False" 
                    ForeColor="#FF3300" Text="*" />
            </td>            
        </tr>     
        
        <tr>
            <td>MR Type</td>
            <td>:</td>
            <td>
                <asp:DropDownList ID="ddlMRType" runat="server" CssClass="DropDownList" 
                    Height="16px" ValidationGroup="Input" Width="100px" AutoPostBack="True">
                    <asp:ListItem>BIASA</asp:ListItem>
                    <asp:ListItem>PENGGANTIAN</asp:ListItem>
                </asp:DropDownList>
                <asp:Label ID="label17" runat="server" Font-Underline="False" 
                    ForeColor="#FF3300" Text="*" />
            </td>                  
        </tr>   
                
        <tr>
            <td>Return Mtr No</td>
            <td>:</td>
            <td><asp:TextBox ID="tbReturNo" runat="server" MaxLength = "20" CssClass="TextBoxR" 
                    Width="124px" AutoPostBack = "true" Enabled="False"/>
                <asp:Button ID="btnReturNo" runat="server" class="bitbtn btngo" Text="..." />
                <asp:Label ID="label18" runat="server" Font-Underline="False" 
                    ForeColor="#FF3300" Text="*" />
            </td>
        </tr>                                                 
        <tr>
             <td><asp:LinkButton ID="lbWorkCenter" runat="server" Text="Work Center" />
             </td>
             <td>:</td>
             <td>
                 <asp:DropDownList ID="ddlWorkCtr" runat="server" CssClass="DropDownList" ValidationGroup="Input" Width="224px" Height="16px" />
                 <asp:Label ID="label19" runat="server" Font-Underline="False" 
                     ForeColor="#FF3300" Text="*" />
             </td>
        </tr>
        <tr>
             <td>Warehouse</td>
             <td>:</td>
             <td>
                <asp:DropDownList ID="ddlWarehouse" runat="server" CssClass="DropDownList" ValidationGroup="Input" Width="224px" Height="16px" />
                 <asp:Label ID="label20" runat="server" Font-Underline="False" 
                     ForeColor="#FF3300" Text="*" />
             </td>
          </tr>
        <tr>
            <td>Remark</td>
            <td>:</td>
            <td>
                <asp:TextBox ID="tbRemark" runat="server" CssClass="TextBox" MaxLength="255" TextMode="MultiLine" Width="430px" ValidationGroup="Input" />
            </td>
        </tr>
      </table>  
      
      <br />      
      <div style="font-size:medium; color:Blue;">Detail</div>
      <hr style="color:Blue" />  
       <%--<asp:Menu
            ID="Menu1"            
            runat="server"
            CssClass = "Menu"        
            StaticMenuItemStyle-CssClass = "MenuItem"
            StaticSelectedStyle-CssClass = "MenuSelect"        
            Orientation="Horizontal"
            ItemWrap = "False"
            StaticEnableDefaultPopOutImage="False">            
            <Items>
                <asp:MenuItem Text="Detail" Value="0"></asp:MenuItem>                   
            </Items>            
        </asp:Menu>
        <br />--%>
        <asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex="0">
           <asp:View ID="Tab1" runat="server">      
        <asp:Panel runat="server" ID="pnlDt">
            <asp:Button class="bitbtn btnadd" runat="server" ID="btnAddDt" Text="Add" ValidationGroup="Input" />	                  
            <br /><br />
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
                                <asp:Button ID="btnEdit" runat="server" class="bitbtndt btnedit" CommandName="Edit" Text="Edit" />
                                <asp:Button ID="btnDelete" runat="server" class="bitbtndt btndelete" CommandName="Delete" 
                                    OnClientClick="return confirm('Sure to delete this data?');" Text="Delete" />
                            </ItemTemplate>
                         </asp:TemplateField>
                         <asp:TemplateField HeaderText="Detail">
                            <ItemTemplate>
                                <asp:Button ID="btnDetailMaterial" runat="server" Class="bitbtndt btngetitem" CommandArgument="<%# Container.DataItemIndex %>" CommandName="DetailAlternate" Text="Detail" Width="70"/>    
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="WONo" HeaderStyle-Width="80px" HeaderText="WONo" />
                        <asp:BoundField DataField="ShiftName" HeaderStyle-Width="150px" HeaderText="Shift" />
                        <asp:BoundField DataField="StartDate" dataformatstring="{0:dd MMM yyyy}" htmlencode="true" HeaderStyle-Width="80px" HeaderText="Start Date"></asp:BoundField>
                        <asp:BoundField DataField="StartTime" HeaderStyle-Width="80px" HeaderText="Start Time"/>
                        <asp:BoundField DataField="EndDate" dataformatstring="{0:dd MMM yyyy}" htmlencode="true" HeaderStyle-Width="80px" HeaderText="End Date"></asp:BoundField>
                        <asp:BoundField DataField="EndTime" HeaderStyle-Width="80px" HeaderText="End Time"/>
                        <asp:BoundField DataField="Product" HeaderStyle-Width="80px" HeaderText="Product Code"/>
                        <asp:BoundField DataField="ProductName" HeaderStyle-Width="150px" HeaderText="Product Name"/>
                        <asp:BoundField DataField="Qty" HeaderStyle-Width="80px" HeaderText="Qty"/>
                        <asp:BoundField DataField="Unit" HeaderStyle-Width="80px" HeaderText="Unit"/>
                        <asp:BoundField DataField="BOMNo" HeaderStyle-Width="80px" HeaderText="BOM No"/>
                    </Columns>
                </asp:GridView>
            </div>
          <br />
          <asp:Button class="bitbtn btnadd" runat="server" ID="btnAddDt2" Text="Add" ValidationGroup="Input" />	  
           
       </asp:Panel>             
       <asp:Panel runat="server" ID="pnlEditDt" Visible="false">
            <table style="width: 673px">
                <tr>
                    <td class="style7">WO NO</td>
                    <td class="style6">:</td>
                    <td colspan="4"><asp:TextBox runat="server" ID="tbWONo" CssClass="TextBoxR" 
                            AutoPostBack="true" MaxLength = "20" Width="124px" Enabled="False"/>
                        <asp:Button Class="btngo" ID="btnWONo" Text="..." runat="server" ValidationGroup="Input" />                                               
                        <asp:Label ID="label10" runat="server" Font-Underline="False" 
                            ForeColor="#FF3300" Text="*" />
                    </td>    
                </tr>                                                    
                <tr>
                    <td class="style7">Shift</td>
                    <td class="style6">:</td>
                    <td colspan="4"><asp:DropDownList ID="ddlShift" runat="server" 
                            CssClass="DropDownList" Enabled="False"/>
                    </td>
                </tr>                                                                          
                <tr>
                    <td class="style7">Start Date</td>
                    <td class="style6">:</td>                    
                    <td class="style8">
                        <BDP:BasicDatePicker ID="tbStartDate" runat="server" 
                            ButtonImageHeight="19px" ButtonImageWidth="20px" DateFormat="dd MMM yyyy" 
                            DisplayType="TextBoxAndImage" ReadOnly="True" ShowNoneButton="false" 
                            TextBoxStyle-CssClass="TextDate" EnableViewState="False"><TextBoxStyle CssClass="TextDate" /></BDP:BasicDatePicker>
                    </td>
                    <td align = "right">Start Time</td>
                    <td class="style6">:</td>
                    <td><asp:TextBox ID="tbStartTime" runat="server" CssClass="TextBoxR" MaxLength="5" 
                            Width="64px" Enabled="False" />
                    </td>
                </tr>                                                                
                <tr>
                    <td class="style7">End Date</td>
                    <td class="style6">:</td>
                    <td class="style8">
                        <BDP:BasicDatePicker ID="tbEndDate" runat="server" 
                            ButtonImageHeight="19px" ButtonImageWidth="20px" DateFormat="dd MMM yyyy" 
                            DisplayType="TextBoxAndImage" ReadOnly="true" ShowNoneButton="false" 
                            TextBoxStyle-CssClass="TextDate" EnableViewState="False"><TextBoxStyle CssClass="TextDate" /></BDP:BasicDatePicker>
                    </td>
                    <td align="right" class="style4">End Time</td>
                    <td class="style6">:</td>
                    <td><asp:TextBox ID="tbEndTime" runat="server" CssClass="TextBoxR" MaxLength="5" 
                            Width="64px" Enabled="False" />
                    </td>
                </tr>                                                          
                <tr>
                    <td class="style7">Product</td>
                    <td class="style6">:</td>
                    <td colspan="4">
                        <asp:TextBox ID="tbProductCode" runat="server" AutoPostBack="true" CssClass="TextBox" MaxLength="20" Width="124px" />
                        <asp:TextBox ID="tbProductName" runat="server" CssClass="TextBoxR" 
                            Width="281px" Enabled="False" />
                        <asp:Button ID="btnProduct" runat="server" Class="btngo" Text="..." ValidationGroup="Input" />
                        <asp:Label ID="label21" runat="server" Font-Underline="False" 
                            ForeColor="#FF3300" Text="*" />
                    </td>
                </tr>
                <tr>
                    <td class="style7">Qty</td>
                    <td class="style6">:</td>
                    <td colspan="4"><asp:TextBox ID="tbQty" runat="server" CssClass="TextBox" Width="64px" />
                        <asp:Label ID="lbUnit" runat="server" Font-Underline="False" ForeColor="#FF3300" Text="lbUnit" />
                        <asp:Label ID="label22" runat="server" Font-Underline="False" ForeColor="#FF3300" Text="*" />
                    </td>
                </tr>
                <tr>
                    <td class="style7">BOM</td>
                    <td class="style6">:</td>
                    <td colspan="4">
                        <asp:TextBox ID="tbBOMNo" runat="server" AutoPostBack="true" 
                            CssClass="TextBoxR" MaxLength="20" Width="124px" Enabled="False" />
                    </td>
                </tr>
            </table>
            <br />                     
            <asp:Button ID="btnSaveDt" runat="server" class="bitbtndt btnsave" Text="Save" />									
            <asp:Button ID="btnCancelDt" runat="server" class="bitbtndt btncancel" Text="Cancel" />									
       </asp:Panel> 
       <br />     
       </asp:View>           
       <asp:View ID="Tab3" runat="server">
       <asp:Panel ID="pnlInfo" runat="server">
            <asp:Label ID="Label5" runat="server" Text="WO | Product :" />
            <asp:Label ID="lbProductCodeDt2" runat="server" Font-Bold="true" ForeColor="Blue" Text="ProductCode" />
            <asp:Label ID="lbProductNameDt2" runat="server" Font-Bold="true" ForeColor="Blue" Text="ProductName" />
            <asp:Label ID="lbProduct" runat="server" Font-Bold="True" ForeColor="Blue" 
                Text="lbProduct" Visible="False" />
            <asp:Label ID="lbBom" runat="server" Font-Bold="True" ForeColor="Blue" 
                Text="lbom" Visible="False" />
            <asp:Label ID="lbWO" runat="server" Font-Bold="True" ForeColor="Blue" 
                Text="lbWO" Visible="False" />
            <asp:Label ID="lbQty" runat="server" Font-Bold="True" ForeColor="Blue" 
                Text="lbWO" Visible="False" />
        </asp:Panel>
        <br />
        <asp:Panel ID="PnlDetail2" runat="server">
        <br />
                <asp:Button ID="btnAddMaterial" runat="server" class="bitbtndt btnadd" Text="Add" />									
                    &nbsp;<asp:Button ID="btnBackMaterial" runat="server" class="bitbtndt btnback" Text="Back" Width="60" /> <br /><br />
                    
                    <div style="border:0px  solid; width:100%; height:100%; overflow:auto;">
                        <asp:GridView ID="GridDetail2" runat="server" AutoGenerateColumns="false" ShowFooter="True">
                            <HeaderStyle CssClass="GridHeader" />
                            <RowStyle CssClass="GridItem" Wrap="false" />
                            <AlternatingRowStyle CssClass="GridAltItem" />
                            <PagerStyle CssClass="GridPager" />
                            <Columns>
                                <asp:TemplateField HeaderText="Action">
                                    <ItemTemplate>
                                        <asp:Button ID="btnEdit" runat="server" class="bitbtn btnedit" CommandName="Edit" Text="Edit" />
                                        <asp:Button ID="btnDelete" runat="server" class="bitbtn btndelete" CommandName="Delete" Text="Delete" />
                                    </ItemTemplate>                                    
                                </asp:TemplateField>
                                <asp:BoundField DataField="Material" HeaderStyle-Width="150px" HeaderText="Material Code" />
                                <asp:BoundField DataField="MaterialName" HeaderStyle-Width="250px" HeaderText="Material Name" />
                                <asp:BoundField DataField="Specification" HeaderStyle-Width="250px" HeaderText="Specification" />
                                <asp:BoundField DataField="Qty" HeaderStyle-Width="80px" HeaderText="Qty" />
                                <asp:BoundField DataField="Unit" HeaderStyle-Width="80px" HeaderText="Unit" />                                
                                <asp:BoundField DataField="Remark" HeaderStyle-Width="250px" HeaderText="Remark" />                                                                
                            </Columns>
       </asp:GridView>
       </div>
       <br />
       <asp:Button ID="btnAddDt2Ke2" runat="server" class="bitbtndt btnadd" Text="Add" />
       &nbsp;<asp:Button ID="btnbackDt3" runat="server" class="bitbtndt btnback" Text="Back" Width="60" />
                
       </asp:Panel>
       &nbsp;<asp:Panel ID="pnlEditMaterial" runat="server" Visible="false">
             <table style="width: 675px">
                 <tr>
                     <td>Material</td>
                     <td>:</td>
                     <td>
                         <asp:TextBox ID="tbMaterialCode" runat="server" AutoPostBack="true" CssClass="TextBox" MaxLength="20" Width="124px" />
                         <asp:TextBox ID="tbMaterialName" runat="server" CssClass="TextBoxR" 
                             Width="281px" Enabled="False" />
                         <asp:Button ID="btnMaterial" runat="server" Class="btngo" Text="..." ValidationGroup="Input" />
                         <asp:Label ID="label13" runat="server" Font-Underline="False" ForeColor="#FF3300" Text="*" />
                     </td>
                 </tr>
                 <tr>
                     <td>Specification</td>
                     <td>:</td>
                     <td>
                         <asp:TextBox ID="tbSpecMaterial" runat="server" CssClass="TextBox" 
                             Enabled="False" Height="44px" MaxLength="255" TextMode="MultiLine" 
                             Width="434px" />
                     </td>
                 </tr>
                 <tr>
                      <td>Qty</td>
                      <td>:</td>
                      <td><asp:TextBox ID="tbQtyDt2" runat="server" CssClass="TextBox" MaxLength="20" 
                              Width="43px" AutoPostBack="true" />
                          <asp:Label ID="lbUnitDt2" runat="server" Font-Underline="False" 
                              ForeColor="#FF3300" Text="LbUnit" />
                          <asp:Label ID="label14" runat="server" Font-Underline="False" 
                              ForeColor="#FF3300" Text="*" />
                      </td>
                 </tr>
                 <tr>
                     <td>
                         Remark</td>
                     <td>
                         :</td>
                     <td>
                         <asp:TextBox ID="tbRemarkDt2" runat="server" CssClass="TextBox" MaxLength="255" 
                             TextMode="MultiLine" Width="434px" />
                     </td>
                 </tr>
             </table>
             <br />
             <asp:Button ID="btnSaveDt2" runat="server" class="bitbtndt btnsave" Text="Save" validationgroup="Input" />
             <asp:Button ID="btnCancelDt2" runat="server" class="bitbtndt btncancel" Text="Cancel" />
                       <br />
                   </asp:Panel>
               
            </asp:View>          
        </asp:MultiView>
      <br />
       	<asp:Button ID="btnSaveAll" runat="server" class="bitbtndt btnsavenew" Text="Save & New" validationgroup="Input" Width = "90"/>									
        <asp:Button ID="btnSaveTrans" runat="server" class="bitbtndt btnsave" Text="Save" validationgroup="Input"/>									
        <asp:Button ID="btnBack" runat="server" class="bitbtndt btncancel" Text="Cancel" validationgroup="Input"/>									
        <asp:Button ID="btnHome" runat="server" class="bitbtndt btnback" Text="Home" />	
    </asp:Panel>
    <asp:Panel runat="server" ID="pnlPrint" Visible="false">
    </asp:Panel>
      <br />            
    </div>
   
    <asp:Label runat ="server" ID="lbStatus" ForeColor="Red"/>
    </form>
    </body>
</html>
