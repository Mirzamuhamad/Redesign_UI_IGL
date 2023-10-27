<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TrFAFromStock.aspx.vb" Inherits="Transaction_TrFAFromStock_TrFAFromStock" %>

<%@ Register Assembly="BasicFrame.WebControls.BasicDatePicker" Namespace="BasicFrame.WebControls"
    TagPrefix="BDP" %>

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
       
       function setformat()
        {
        try
         {        
          var Qty = document.getElementById("tbQty").value.replace(/\$|\,/g,""); 
          var AmountForex = document.getElementById("tbAmountForexDt").value.replace(/\$|\,/g,""); 
          var Rate = document.getElementById("tbRateDt").value.replace(/\$|\,/g,""); 
          var LifeMonth = parseFloat(document.getElementById("tbLifeMonth").value.replace(/\$|\,/g,"")); 
         document.getElementById("tbRateDt").value = setdigit(Rate,'<%=ViewState("DigitRate")%>');
         document.getElementById("tbAmountForexDt").value = setdigit(AmountForex,'<%=VIEWSTATE("DigitCurr")%>');        
         document.getElementById("tbQty").value = setdigit(Qty,'<%=ViewState("DigitQty")%>');
         document.getElementById("tbLifeMonth").value = setdigit(LifeMonth,'<%=ViewState("DigitQty")%>');

        }catch (err){
            alert(err.description);
          }      
        }   
        
       function setformatdt()
        {
        try
         {         
         
         
         if(isNaN(LifeMonth) == true)
         {
           LifeMonth = 0;
         }
         if(isNaN(LifeDepr) == true)
         {
           LifeDepr = 0;
         }                
         
         AmountDepr =  (LifeDepr/LifeMonth) * AmountHome   
         
         if(isNaN(AmountDepr) == true)
         {
           AmountDepr = 0;
         }      
                                   
         
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
    <div class="H1"><asp:Label runat="server" ID="lbTitle"></asp:Label></div>
     <hr style="color:Blue" />        
    <asp:Panel runat="server" id="PnlHd"> 
      <table>
        <tr>
            <td style="width:100px;text-align:right">Quick Search :</td>         
            <%--TransNmbr, TransDate, STATUS, FgReport, UserType, UserCode, UserName, Attn, Remark--%>   
            <td><asp:TextBox CssClass="TextBox" runat="server" ID ="tbFilter"/> 
                  <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlField" >                      
                    <asp:ListItem Selected="True" Value="TransNmbr">Reference</asp:ListItem>
                    <asp:ListItem Value="dbo.FormatDate(TransDate)">Date</asp:ListItem>
                    <asp:ListItem>Status</asp:ListItem>
                    <asp:ListItem Value="Operator">Operator</asp:ListItem>                    
                      <asp:ListItem Value="Wrhs_Name">Warehouse</asp:ListItem>
                      <asp:ListItem>Remark</asp:ListItem>
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
                    <asp:ListItem Selected="True" Value="TransNmbr">Reference</asp:ListItem>
                    <asp:ListItem Value="dbo.FormatDate(TransDate)">Date</asp:ListItem>
                    <asp:ListItem>Status</asp:ListItem>
                    <asp:ListItem Value="Wrhs_Name">Warehouse</asp:ListItem>                                       
                    <asp:ListItem Value="Operator">Operator</asp:ListItem>
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
            <asp:Button class="btngo" runat="server" ID="BtnGo" Text="G" Visible="false"/>                 
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
                              <%--<asp:ListItem Text="Print" />--%>
                          </asp:DropDownList>
                          <asp:Button class="btngo" runat="server" ID="BtnGo" Text="G" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" CommandName="Go" />                 
                      </ItemTemplate>
                      <HeaderStyle Width="110px" />
                  </asp:TemplateField>
                  <asp:BoundField DataField="TransNmbr" HeaderStyle-Width="120px" 
                      SortExpression="Nmbr" HeaderText="Reference">
                      <HeaderStyle Width="120px" />
                  </asp:BoundField>                  
                  <asp:BoundField DataField="Status" HeaderText="Status"></asp:BoundField>                  
                  <asp:BoundField DataField="TransDate" dataformatstring="{0:dd MMM yyyy}" htmlencode="true" HeaderStyle-Width="80px" SortExpression="TransDate" HeaderText="Date"></asp:BoundField>                  
                  <asp:BoundField DataField="Wrhs_Name" HeaderStyle-Width="180px" 
                      HeaderText="Warehouse" SortExpression="Wrhs_Name">
                      <HeaderStyle Width="180px" />
                  </asp:BoundField>
                  <asp:BoundField DataField="Subled" HeaderStyle-Width="180px" HeaderText="Subled" SortExpression="Subled">
                      <HeaderStyle Width="180px" />
                  </asp:BoundField>
                  <asp:BoundField DataField="Subled_Name" HeaderStyle-Width="180px" HeaderText="Subled Name" SortExpression="Subled_Name">
                      <HeaderStyle Width="180px" />
                  </asp:BoundField>
                  <asp:BoundField DataField="Operator" HeaderStyle-Width="80px" SortExpression="Operator" HeaderText="Operator">
                      <HeaderStyle Width="80px" />
                  </asp:BoundField>
                  <asp:BoundField DataField="Remark" HeaderStyle-Width="250px" HeaderText="Remark">
                      <HeaderStyle Width="250px" />
                  </asp:BoundField>
                  
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
            <td>Transfer No</td>
            <td>:</td>
            <td><asp:TextBox CssClass="TextBoxR" runat="server" ID="tbCode" Width="150px" Enabled="False"/></td>                            
        </tr>                     
        <tr>
            <td>Transfer Date</td>
            <td>:</td>
            <td>
            <BDP:BasicDatePicker ID="tbDate" runat="server" DateFormat="dd MMM yyyy" 
                        ReadOnly = "true" ValidationGroup="Input"
                        ButtonImageHeight="19px" ButtonImageWidth="20px" 
                        DisplayType="TextBoxAndImage" 
                        TextBoxStyle-CssClass="TextDate" 
                        ShowNoneButton="False"><TextBoxStyle CssClass="TextDate" /></BDP:BasicDatePicker>                
            </td>   
        </tr>
        <tr>
              <td>Warehouse</td>
              <td>:</td>
              <td>
                  <asp:DropDownList ID="ddlWrhs" runat="server" Width="150px" AutoPostBack="true" CssClass="DropDownList" ValidationGroup="Input" ></asp:DropDownList>
                  <asp:TextBox ID="tbFgSubled" runat="server" AutoPostBack="true" 
                      CssClass="TextBox" Visible="False" />
              </td>
          </tr>
          <tr>
              <td>Subled</td>
              <td>:</td>
              <td>
                  <asp:TextBox ID="tbSubled" runat="server" CssClass="TextBox" 
                      AutoPostBack="True" />
                  <asp:TextBox ID="tbSubledName" runat="server" CssClass="TextBoxR" Enabled="False" Width="200px" />
                  <asp:Button class="btngo" runat="server" ID="btnSubled" Text="..." ValidationGroup ="Input" />                 
              </td>
          </tr>  
          <tr>
              <td>
                  Operator</td>
              <td>
                  :</td>
              <td>
                  <asp:TextBox ID="tbOperator" runat="server" CssClass="TextBox" MaxLength="255" ValidationGroup="Input" Width="225px" />
              </td>
          </tr>
          <tr>
              <td>
                  Remark</td>
              <td>
                  :</td>
              <td>
                  <asp:TextBox ID="tbRemark" runat="server" CssClass="TextBox" ValidationGroup="Input" Width="225px" />
              </td>
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
                <asp:MenuItem Text="Detail Fixed Asset" Value="0"></asp:MenuItem>
                <%--<asp:MenuItem Text="Detail FA Location" Value="1"></asp:MenuItem>--%>
            </Items>            
        </asp:Menu>
        <br />
        <asp:MultiView 
        ID="MultiView1"
        runat="server"
        ActiveViewIndex="0">
           <asp:View ID="Tab1" runat="server">
              <asp:Panel runat="server" ID="PnlDt">
                <asp:Button class="bitbtndt btnadd" runat="server" ID="btnAdddt" Text="Add" ValidationGroup ="Input" />                 
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
                                        <asp:Button class="bitbtndt btnedit" runat="server" ID="btnEdit" Text="Edit" CommandName="Edit"/>                 
                                        <asp:Button class="bitbtndt btndelete" runat="server" ID="btnDelete" Text="Delete" CommandName="Delete" OnClientClick="return confirm('Sure to delete this data?');"/>                 
                                      </ItemTemplate>
                            </asp:TemplateField>                            
                            <asp:BoundField DataField="FACode" HeaderStyle-Width="100px" 
                                HeaderText="Asset ID" >
                                <HeaderStyle Width="90px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="FAName" HeaderStyle-Width="100px" 
                                HeaderText="Asset Name" >                            
                                <HeaderStyle Width="160px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Product" 
                                HeaderText="Product" >
                            </asp:BoundField>
                            <asp:BoundField DataField="Product_Name" HeaderText="Proudct Name" />
                            <asp:BoundField DataField="Location" HeaderText="Location" />
                            <asp:BoundField DataField="Qty" HeaderText="Qty" />
                            <asp:BoundField DataField="Unit" HeaderText="Unit" />
                            <asp:BoundField DataField="FAStatusName" HeaderStyle-Width="100px" 
                                HeaderText="Asset Status">
                                <HeaderStyle Width="100px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="FAOwner" HeaderText="Owner" />
                            <asp:BoundField DataField="FASubGroupName" HeaderStyle-Width="100px" 
                                HeaderText="Asset Sub Group" >
                                <HeaderStyle Width="100px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="FALocationType" 
                                HeaderText="Location Type" >
                            </asp:BoundField>
                            <asp:BoundField DataField="FALocationCode" HeaderText="Location Code" />
                            <asp:BoundField DataField="FA_Location_Name" HeaderText="Location Name" />
                            <asp:BoundField DataField="CostCtrName" HeaderStyle-Width="100px" 
                                HeaderText="Cost Centre">
                                <HeaderStyle Width="100px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="LifeMonth" HeaderStyle-Width="10px" 
                                HeaderText="Life Month">
                                <HeaderStyle Width="100px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Currency" HeaderText="Curr" />
                            <asp:BoundField DataField="AmountForex" HeaderStyle-Width="100px" 
                                HeaderText="Amount" >
                                <HeaderStyle Width="100px" />
                            </asp:BoundField>
                        </Columns>
                    </asp:GridView>
              </div>   
              <asp:Button class="bitbtndt btnadd" runat="server" ID="btnAddDtke2" Text="Add" ValidationGroup="Input"/>                 
              </asp:Panel> 
              <asp:Panel runat="server" ID="pnlEditDt" Visible="false">
                <table>    
                    <tr>
                        <td>Product</td>
                        <td>:</td>
                        <td colspan="4">                                
                            <asp:TextBox CssClass="TextBox" ValidationGroup="Input" runat="server" ID="tbProduct" Width="135px" AutoPostBack="true" />
                            <asp:TextBox CssClass="TextBox" runat="server" ID="tbProductName" Enabled="false" Width="250px"/>
                            <asp:Button class="btngo" runat="server" ID="btnProduct" Text="..." ValidationGroup="Input"/>                 
                        </td>
                    </tr>
                    <tr>
                        <td>Location</td>
                        <td>:</td>
                        <td><asp:DropDownList CssClass="DropDownList" ID="ddlLocation" runat="server" 
                                Height="16px" Width="135px"/></td>
                        <td>Qty</td>
                        <td>:</td>
                        <td>
                            <asp:TextBox ID="tbQty" runat="server" CssClass="TextBox" Width="50px" />
                            <asp:TextBox ID="tbUnit" CssClass="TextBox" runat="server" Enabled="false" Width="50px"/>
                        </td> 
                    </tr>   
                    <tr>                    
                        <td>Asset ID & Name</td>
                        <td>:</td>
                        <td colspan="4">                                
                            <asp:TextBox CssClass="TextBox" ValidationGroup="Input" runat="server" ID="tbFA" Width="135px" />                                                    
                            <asp:TextBox CssClass="TextBox" runat="server" ID="tbFAName" Width="250px"/>                            
                        </td>
                    </tr>
                    <tr>
                        <td>Asset Status</td>
                        <td>:</td>
                        <td><asp:DropDownList CssClass="DropDownList" ID="ddlFAStatus" runat="server"/></td>
                        <td>Asset Owner</td>
                        <td>:</td>
                        <td>
                            <asp:DropDownList Enabled="false" CssClass="DropDownList" ID="ddlFAOwner" runat="server" >
                                <asp:ListItem Selected="True">Y</asp:ListItem>
                                <asp:ListItem>N</asp:ListItem>
                            </asp:DropDownList> 
                        </td> 
                    </tr>                       
                    <tr>
                        <td>Asset Sub Group</td>
                        <td>:</td>
                        <td colspan="4">                                
                            <asp:TextBox CssClass="TextBox" ValidationGroup="Input" runat="server" ID="tbFASubGroup" AutoPostBack="true" />
                            <asp:TextBox CssClass="TextBox" runat="server" ID="tbFASubGroupName" Enabled="false" Width="225px"/>
                            <asp:Button class="btngo" runat="server" ID="btnFASubGroup" Text="..." ValidationGroup="Input"/>                 
                        </td>
                    </tr>                    
                    <tr>
                        <td>Asset Location</td>
                        <td>:</td>
                        <td colspan="4">
                        <asp:DropDownList CssClass="DropDownList" ID="ddlFALocType" runat="server" AutoPostBack ="true">
                            <asp:ListItem Selected="True">GENERAL</asp:ListItem>
                            <asp:ListItem>CUSTOMER</asp:ListItem>
                            <asp:ListItem>SUPPLIER</asp:ListItem>
                            <asp:ListItem>EMPLOYEE</asp:ListItem>
                        </asp:DropDownList>
                        <asp:TextBox CssClass="TextBox" ValidationGroup="Input" runat="server" ID="tbFALocCode" AutoPostBack="true" />
                        <asp:TextBox CssClass="TextBox" runat="server" ID="tbFALocName" Enabled="false" Width="225px"/>
                        <asp:Button class="btngo" runat="server" ID="btnFALoc" Text="..." ValidationGroup="Input"/>                 
                        </td>
                    </tr> 
                    <tr>
                        <td>Cost Center</td>
                        <td>:</td>
                        <td><asp:DropDownList CssClass="DropDownList" ID="ddlCostCenterDt" runat="server"  Width="150px"/></td>
                        <td>Process</td>
                        <td>:</td>
                        <td>
                            <asp:DropDownList ID="ddlFAProcess" runat="server" 
                                CssClass="DropDownList" Enabled="false" >
                                <asp:ListItem Selected="True">Y</asp:ListItem>
                                <asp:ListItem>N</asp:ListItem>
                            </asp:DropDownList>
                        </td>            
                    </tr>   
                    <tr> 
                        <td>Currency</td>
                        <td>:</td>
                        <td>
                            <asp:DropDownList CssClass="DropDownList" ID="ddlCurrDt" runat="server" AutoPostBack=true Width="60px"/>
                            <asp:TextBox CssClass="TextBox" runat="server" ID="tbRateDt" Width="65px"/>                                    
                        </td>                       
                    </tr>  
                    <tr> 
                        <td>Life Month</td>
                        <td>:</td>
                        <td><asp:TextBox CssClass="TextBoxR" runat="server" ID="tbLifeMonth"/></td>
                    </tr>           
                    <tr> 
                        <td>Amount</td>
                        <td>:</td>
                        <td><asp:TextBox CssClass="TextBox" runat="server" ID="tbAmountForexDt"/></td>                                    
                      </tr>                      
                </table>
                <br /> 
                <asp:Button class="bitbtndt btnsave" runat="server" ID="btnSaveDt" Text="Save"/>                 
                <asp:Button class="bitbtndt btncancel" runat="server" ID="btnCancelDt" Text="Cancel"/>                 
           </asp:Panel> 
              
           </asp:View>           
        </asp:MultiView>
    
       <br />  
        <asp:Button class="bitbtn btnsavenew" runat="server" ID="btnSaveAll" 
            Text="Save & New" ValidationGroup="Input" Width="95px"/>                 
        <asp:Button class="bitbtn btnsave" runat="server" ID="btnSaveTrans" Text="Save" ValidationGroup="Input"/>                 
        <asp:Button class="bitbtn btnback" runat="server" ID="btnBack" Text="Back" ValidationGroup="Input"/>                 
        <asp:Button class="btngo" runat="server" ID="btnHome" Text="Home" Width="50px"/>                 
     </asp:Panel>        
    </div>
   
    <asp:Label runat ="server" ID="lbStatus" ForeColor="Red"/>
    </form>
</body>
</html>
