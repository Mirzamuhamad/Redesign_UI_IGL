<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TrBuktiOutMaterial.aspx.vb" Inherits="TrBuktiOutMaterial" %>

<%@ Register Assembly="BasicFrame.WebControls.BasicDatePicker" Namespace="BasicFrame.WebControls"
    TagPrefix="BDP" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Bukti Material Keluar</title>

    <script src="../../Function/OpenDlg.js" type="text/javascript"></script>
    <script src="../../Function/Function.JS" type="text/javascript"></script>
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
 <link href="https://fonts.googleapis.com/css2?family=Muli:wght@300;400;600&display=swap" rel="stylesheet">
       <link type="text/css" rel="stylesheet" href="../../Styles/circularprogress.css" />
    <script type="text/javascript" src="../../JQuery/jquery.min.js"></script>



    <script type="text/javascript">


        function ProgressCircle() {
            setTimeout(function() {
                var modal = $('<div />');
                modal.addClass("modal");
                $('body').append(modal);
                var loading = $(".loading");
                loading.show();
                var top = Math.max($(window).height() / 2 - loading[0].offsetHeight / 2, 0);
                var left = Math.max($(window).width() / 2 - loading[0].offsetWidth / 2, 0);
                loading.css({ top: top, left: left });
            }, 200);
        }
        $('form').live("submit", function() {
            ProgressCircle();
        });
      
      
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
          var VPayment = document.getElementById("tbTotalPayment").value.replace(/\$|\,/g,"");
          var VExpense = document.getElementById("tbTotalExpense").value.replace(/\$|\,/g,"");
          var VCharge = document.getElementById("tbTotalCharge").value.replace(/\$|\,/g,"");
          
          document.getElementById("tbTotalPayment").value = setdigit(VPayment,'<%=ViewState("DigitHome")%>');
          document.getElementById("tbTotalExpense").value = setdigit(VExpense,'<%=ViewState("DigitHome")%>');
          document.getElementById("tbTotalCharge").value = setdigit(VCharge,'<%=ViewState("DigitHome")%>');
        }catch (err){
            alert(err.description);
          }      
        }   
        
       function setformatdt()
        {
        try
         {         
         var AmountForex = document.getElementById("tbAmountForexDt").value.replace(/\$|\,/g,""); 
         var AmountHome = document.getElementById("tbAmountHomeDt").value.replace(/\$|\,/g,""); 
         var Rate = document.getElementById("tbRateDt").value.replace(/\$|\,/g,""); 
         
         var LifeMonth = parseFloat(document.getElementById("tbLifeMonth").value.replace(/\$|\,/g,"")); 
         var LifeDepr = parseFloat(document.getElementById("tbLifeProcessDepr").value.replace(/\$|\,/g,"")); 
         var AmountDepr = parseFloat(document.getElementById("tbAmountProcessDepr").value.replace(/\$|\,/g,"")); 
         var PriceForex = parseFloat(document.getElementById("tbPriceForex").value.replace(/\$|\,/g,"")); 
         
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
                                   
         document.getElementById("tbRateDt").value = setdigit(Rate,'<%=ViewState("DigitCurr")%>');
         document.getElementById("tbAmountForexDt").value = setdigit(AmountForex,'<%=VIEWSTATE("DigitCurr")%>');
         document.getElementById("tbAmountHomeDt").value = setdigit(AmountHome,'<%=ViewState("DigitHome")%>');
         
         document.getElementById("tbLifeMonth").value = setdigit(LifeMonth,'<%=ViewState("DigitQty")%>');
         document.getElementById("tbLifeProcessDepr").value = setdigit(LifeDepr,'<%=ViewState("DigitQty")%>');
         document.getElementById("tbAmountProcessDepr").value = setdigit(AmountDepr,'<%=ViewState("DigitHome")%>');
         
        }catch (err){
            alert(err.description);
          }      
        }   

        function setformatdt2()
        {
        try
         {
         
        }catch (err){
            alert(err.description);
          }
      }
      function OpenPopup() {
          var left = (screen.width - 600) / 2; //370
          var top = (screen.height - 600) / 2;
          var winOpen = window.open("../../earchDlgV.Aspx", "", 'toolbar=no, location=no, directories=no, status=no, menubar=no, scrollbars=yes, resizable=yes, copyhistory=no, width=' + 650 + ', height=' + 600 + ', top=' + top + ', left=' + left);
          //winOpen.reload(); 
          Opener.Location.reload(false);
          return false;
      }  
               
        
    </script>

 <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
 <link href="https://fonts.googleapis.com/css2?family=Muli:wght@300;400;600&display=swap" rel="stylesheet">
</head>
<body>
    <form id="form1" runat="server">
    <div class="Content">
        <div class="H1">Bukti Material Keluar</div>
        <hr style="color: Blue" />
        <asp:Panel runat="server" ID="PnlHd">
            <table>
                <tr>
                    <td style="width: 100px; text-align: right">
                        Quick Search :
                    </td>
                    <%--TransNmbr, TransDate, STATUS, FgReport, UserType, UserCode, UserName, Attn, Remark--%>
                    <td>
                        <asp:TextBox CssClass="TextBox" runat="server" ID="tbFilter" />
                        <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlField">
                            <asp:ListItem Selected="True" Value="TransNmbr">Reference</asp:ListItem>
                            <asp:ListItem Value="dbo.FormatDate(TransDate)">Date</asp:ListItem>
                            <asp:ListItem>Status</asp:ListItem>
                            <asp:ListItem Value="WoServiceNo">Wo Service No</asp:ListItem>
                            <asp:ListItem Value="Remark">Remark</asp:ListItem>
                        
                        </asp:DropDownList>
                        <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlRange">
                        </asp:DropDownList>
                        <asp:Button class="bitbtn btnsearch" runat="server" ID="btnSearch" Text="Search" />
                        <asp:Button class="btngo" runat="server" ID="btnExpand" Text="..." />
                    </td>
                    <td>
                        <asp:LinkButton ID="LbAdvSearch" runat="server" Text="Advanced Search" />
                    </td>
                </tr>
            </table>
            <asp:Panel runat="server" ID="pnlSearch" Visible="false">
                <table>
                    <tr>
                        <td style="width: 100px; text-align: right">
                            <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlNotasi">
                                <asp:ListItem Selected="true" Text="OR" Value="OR"></asp:ListItem>
                                <asp:ListItem Text="AND" Value="AND"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:TextBox runat="server" CssClass="TextBox" ID="tbfilter2" />
                            <asp:DropDownList runat="server" CssClass="DropDownList" ID="ddlField2">
                                <asp:ListItem Selected="True" Value="TransNmbr">Reference</asp:ListItem>
                                <asp:ListItem Value="dbo.FormatDate(TransDate)">Date</asp:ListItem>
                                <asp:ListItem>Status</asp:ListItem>
                            <asp:ListItem Value="WoServiceNo">Wo Service No</asp:ListItem>
                            <asp:ListItem Value="Remark">Remark</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <br />
            <asp:Button class="bitbtn btnadd" runat="server" ID="BtnAdd" Text="Add" />
            &nbsp;
            <asp:DropDownList CssClass="DropDownList" ID="ddlCommand" runat="server" Visible="false" />
            <asp:Button class="btngo" runat="server" ID="BtnGo" Text="G" Visible="false" />
            <br />&nbsp;
            <div style="border: 0px  solid; width: 100%; height: 100%; overflow: auto;">
                <asp:GridView ID="GridView1" runat="server" AllowPaging="True" AllowSorting="true"
                    CssClass="Grid" AutoGenerateColumns="false">
                    <HeaderStyle CssClass="GridHeader"></HeaderStyle>
                    <RowStyle CssClass="GridItem" Wrap="false" />
                    <AlternatingRowStyle CssClass="GridAltItem" />
                    <PagerStyle CssClass="GridPager" />
                    <Columns>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:CheckBox ID="cbSelectHd" runat="server" AutoPostBack="true" OnCheckedChanged="cbSelectHd_CheckedChanged" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="cbSelect" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-Width="100" HeaderText="Action">
                            <ItemTemplate>
                                <asp:DropDownList CssClass="DropDownList" ID="ddl" runat="server" Width="60px"> 
                                    <asp:ListItem Selected="True" Text="View" />
                                    <asp:ListItem Text="Edit" />
                                    <asp:ListItem Text="Print" />
                                </asp:DropDownList>
                                <asp:Button class="btngo" runat="server" ID="BtnGo" Text="G" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>"
                                    CommandName="Go" />
                            </ItemTemplate>
                            <HeaderStyle Width="100px" />
                        </asp:TemplateField>
                        <asp:BoundField DataField="TransNmbr" HeaderStyle-Width="120px" SortExpression="Nmbr"
                            HeaderText="No Mutasi"></asp:BoundField>
                        <asp:BoundField DataField="Status" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="60px" HeaderText="Status"></asp:BoundField>
                        <asp:BoundField DataField="TransDate" DataFormatString="{0:dd MMM yyyy}" HtmlEncode="true"
                            HeaderStyle-Width="80px" SortExpression="TransDate" HeaderText="Date"></asp:BoundField>
                            
                        <asp:BoundField DataField="WoServiceNo" HeaderStyle-Width="120px" SortExpression="AreaCode" HeaderText="WO Service No"></asp:BoundField>
                        <asp:BoundField DataField="WrhsCode" HeaderStyle-Width="100px" SortExpression="WrhsCode" HeaderText="Wrhs Code"></asp:BoundField>
                        <asp:BoundField DataField="Wrhs_Name" HeaderStyle-Width="150px" SortExpression="Wrhs_Name" HeaderText="Wrhs Name"></asp:BoundField>
                        <asp:BoundField DataField="LocationName" HeaderStyle-Width="200px" SortExpression="LocationName" HeaderText="Location Name "></asp:BoundField>
                        <asp:BoundField DataField="Remark" HeaderStyle-Width="250px" HeaderText="Remark">
                        </asp:BoundField>
                    </Columns>
                </asp:GridView>
            </div>
            <br/>
            <br />
            <asp:Panel runat="server" ID="pnlNav" Visible="false">
                <asp:Button class="bitbtn btnadd" runat="server" ID="btnAdd2" Text="Add" />
                &nbsp;
                <asp:DropDownList ID="ddlCommand2" CssClass="DropDownList" runat="server" />
                <asp:Button class="btngo" runat="server" ID="btnGo2" Text="G" />
            </asp:Panel>
        </asp:Panel>
        <asp:Panel runat="server" ID="pnlInput" Visible="false">
            <table>
                <tr>
                    <td>TransNo</td>
                    <td>:</td> 
                    <td width="250px">
                        <asp:TextBox CssClass="TextBoxR" runat="server" ID="tbCode" Width="150px" Enabled="False" />
                    </td>
                    
                    <td>TransDate</td>
                    <td>:</td>
                    <td>
                        <BDP:BasicDatePicker ID="tbDate" runat="server" DateFormat="dd MMM yyyy" ReadOnly="true"
                            ValidationGroup="Input" ButtonImageHeight="19px" ButtonImageWidth="20px" DisplayType="TextBoxAndImage"
                            TextBoxStyle-CssClass="TextDate" ShowNoneButton="False">
                            <TextBoxStyle CssClass="TextDate" />
                        </BDP:BasicDatePicker>
                    </td>
                    
                </tr>
                
    
                <tr>                    
                        <td>WO Service No</td>
                        <td>:</td>
                        <td><asp:TextBox runat="server" Enabled = "false" ValidationGroup="Input" ID="tbWoService" CssClass="TextBox" Width="225px"/>
                        <asp:Button Class="btngo" ID="btnWoService" Text="..." runat="server" ValidationGroup="Input" /></td>
                </tr>
                
                <tr>
              
              <td>
                      Warehause </td>
                  <td>
                      :</td>
                  <td>
                      <asp:DropDownList ID="ddlWrhs" runat="server" AutoPostBack="true" 
                          CssClass="DropDownList" ValidationGroup="Input" Width="230px">
                      </asp:DropDownList>
                  </td> 
              </tr>
              
                <tr>
              
                  <td>
                          Location </td>
                      <td>
                          :</td>
                      <td>
                          <asp:DropDownList ID="ddlLocation" runat="server" AutoPostBack="false" 
                              CssClass="DropDownList" ValidationGroup="Input" Width="230px">
                          </asp:DropDownList>
                      </td> 
                  </tr>
              
                
                <tr>
                    <td>Remark</td>
                    <td>:</td>
                    <td>
                        <asp:TextBox runat="server" ValidationGroup="Input" ID="tbRemark" CssClass="TextBox" Width="300px" />
                        <br />
                    </td>
                    
                </tr>
                <tr>
                </tr>
                <br />
                 <tr>
                    <td>
                    </td>
                    <td>
                    </td>
                    <td>
                   
                    <asp:Panel runat="server" ID="PnlInfo" Visible="false" Height="100%" Width="100%">
                    <asp:Label ID="lbInfo" runat="server" Font-Bold="true" Visible="true" Text="Info Product Need to Transfer "></asp:Label>
                        <asp:GridView ID="GridInfo" runat="server" AutoGenerateColumns="true" 
                            ShowFooter="False" Visible="true">
                            <HeaderStyle CssClass="GridHeader" />
                            <RowStyle CssClass="GridItem" Wrap="false" />
                            <AlternatingRowStyle CssClass="GridAltItem" />
                            <PagerStyle CssClass="GridPager" />
                            <Columns>
                                <%--<asp:BoundField DataField="ProductCode" HeaderStyle-Width="120px" 
                                    HeaderText="Product" />
                                <asp:BoundField DataField="Qty" HeaderStyle-Width="70px" HeaderText="Qty" />--%>
                            </Columns>
                        </asp:GridView>                     
                    </asp:Panel>  
                    </td>                   
                    
                </tr>
                
                
                
                
            </table>
            
            <br />
           
            <asp:Menu ID="Menu1" runat="server" CssClass="Menu" StaticMenuItemStyle-CssClass="MenuItem"
                StaticSelectedStyle-CssClass="MenuSelect" Orientation="Horizontal" ItemWrap="False"
                StaticEnableDefaultPopOutImage="False">
                <Items>
                    <%--<asp:MenuItem Text="Detail Transaction" Value="0"></asp:MenuItem>
                    <asp:MenuItem Text="Detail No Payment" Value="1"></asp:MenuItem>
                    <asp:MenuItem Text="Detail No Payment" Value="2"></asp:MenuItem>--%>
                </Items>
            </asp:Menu>
              <br />
              <div style="font-size:medium; ">Detail Product TRM</div>
             <hr />
            <asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex="0">
                <asp:View ID="Tab1" runat="server">
                    <asp:Panel runat="server" ID="PnlDt">
                        <asp:Button class="bitbtndt btnadd" runat="server" ID="btnAdddt" Text="Add" ValidationGroup="Input" /><br/>&nbsp;
                        <div style="border: 0px  solid; width: 100%; height: 100%; overflow: auto;">
                            <asp:GridView ID="GridDt" runat="server" AutoGenerateColumns="false" ShowFooter="False">
                                <HeaderStyle CssClass="GridHeader" />
                                <RowStyle CssClass="GridItem" Wrap="false" />
                                <AlternatingRowStyle CssClass="GridAltItem" />
                                <PagerStyle CssClass="GridPager" />
                                <Columns>
                                    <asp:TemplateField HeaderText="Action">
                                        <ItemTemplate>
                                            <asp:Button ID="btnEdit" runat="server" class="bitbtndt btnedit" Text="Edit" CommandName="Edit" />
                                            <asp:Button ID="btnDelete" runat="server" class="bitbtndt btndelete" Text="Delete" 
                                                OnClientClick="return confirm('Sure to delete this data?');" CommandName="Delete" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField Visible = "false">
                                        <ItemTemplate>
                                            <asp:Button ID="btnView" runat="server" class="bitbtndt btnedit" Text="Detail Dokumen" Width="120px"
                                                CommandName="View" CommandArgument='<%# Container.DataItemIndex %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    
                                    <asp:BoundField DataField="TrmNo" HeaderStyle-Width="120px" 
                                            HeaderText="TRM No" SortExpression="TrmNo" >                            
                                            <HeaderStyle Width="120px" />
                                        </asp:BoundField>
                                    
                                        <asp:BoundField DataField="ProductCode" HeaderStyle-Width="120px" 
                                            HeaderText="Product Code" SortExpression = "ProductCode" >                            
                                            <HeaderStyle Width="120px" />
                                        </asp:BoundField>
                                        
                                        <asp:BoundField DataField="Product_Name" HeaderText="Product Name" HeaderStyle-Width="120px" 
                                            SortExpression="ProductName" >
                                            <HeaderStyle Width="200px" />
                                        </asp:BoundField>
                                        
                                        <asp:BoundField DataField="WrhsSrcName" HeaderText="Wrhs Src" HeaderStyle-Width="120px" 
                                            SortExpression="WrhsSrcName" >
                                            <HeaderStyle Width="200px" />
                                        </asp:BoundField>
                                        
                                        <asp:BoundField DataField="LocSrcName" HeaderText="Location Src" HeaderStyle-Width="120px" 
                                            SortExpression="LocSrcName" >
                                            <HeaderStyle Width="200px" />
                                        </asp:BoundField>
                                        
                                        <asp:BoundField DataField="WrhsDestName" HeaderText="Wrhs Dest" HeaderStyle-Width="120px" 
                                            SortExpression="WrhsDestName" >
                                            <HeaderStyle Width="200px" />
                                        </asp:BoundField>
                                        
                                        <asp:BoundField DataField="LocDestName" HeaderText="Location Dest" HeaderStyle-Width="120px" 
                                            SortExpression="LocDestName" >
                                            <HeaderStyle Width="200px" />
                                        </asp:BoundField>                                       
                                                                                                             
                                        <asp:BoundField DataField="Qty" HeaderText="Qty" DataFormatString="{0:#,##0.##}"  ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" 
                                            SortExpression="Qty" >
                                            <HeaderStyle Width="80px" />
                                        </asp:BoundField>
                                        
                                        <asp:BoundField DataField="Unit" HeaderText="Unit" SortExpression="Unit" />                                                                        
                                        <asp:BoundField DataField="Remark" HeaderStyle-Width="80px" 
                                            HeaderText="Remark" >
                                            <HeaderStyle Width="250px" />
                                        </asp:BoundField>
                                        
                                </Columns>
                            </asp:GridView>
                        </div> <br/>
            
                        <asp:Button class="bitbtndt btnadd" runat="server" ID="btnAddDtke2" Text="Add" ValidationGroup="Input" />
                        
                        
                    </asp:Panel>
                    
                    
                    <asp:Panel runat="server" ID="pnlEditDt" Visible="false">
                        <table>
                            
                                <tr>
                                    <td>
                                        TRM No 
                                    </td>
                                    
                                    <td>:</td>
                                    <td>
                                        <asp:TextBox ID="tbTrmNo" runat="server" CssClass="TextBox" Width="150px" />
                                            
                                            <asp:Button Class="btngo" ID="btnTrm" Text= "..." runat="server" />                                                        
                                    
                                    </td>
                                   
                                </tr> 
                                
                                <tr>
                                    <td>
                                        Product 
                                    </td>
                                    
                                    <td>:</td>
                                    <td>
                                        <asp:TextBox ID="tbProdSrcCode" runat="server" AutoPostBack="true" 
                                            CssClass="TextBox" Width="150px" />
                                    </td>
                                    <td>        
                                        <asp:TextBox ID="tbProdSrcName" runat="server" CssClass="TextBoxR" Enabled="False" 
                                            EnableTheming="True" ReadOnly="True" Width="200px" />
                                    </td>
                                    <td>  
                                    
                                        <asp:Button Class="btngo" ID="btnProdSrc" Text= "..." runat="server" />                                                        
                                        <asp:TextBox ID="tbLevelProduct" runat="server" CssClass="TextBox" 
                                            Visible="False" Width="80px" />
                                    </td>
                                </tr> 
                                
                                <tr>
                                    <td>
                                        Qty</td>
                                    <td>:</td>
                                    <td>
                                        <asp:TextBox ID="tbQtySrc" runat="server" 
                                        CssClass="TextBox" Width="80px" />
                                    </td>
                                                       
                                </tr>
                                
                                <tr>
                                <td>
                                        Unit</td>
                                    <td>:</td>
                                    <td>
                                        <asp:TextBox ID="tbUnitSrc" runat="server" CssClass="TextBox" Enabled="false" 
                                        Width="80px" />
                                    </td>
                                            
                                </tr>
                                
                                 <tr>
              
                                  <td>
                                          Warehause Src </td>
                                      <td>
                                          :</td>
                                      <td>
                                          <asp:DropDownList ID="ddlWrhsSrc" runat="server" AutoPostBack="False" 
                                              CssClass="DropDownList" ValidationGroup="Input" Width="100px">
                                          </asp:DropDownList>
                                      </td> 
                                  </tr>
                                
                                <tr>
                                    <td>
                                        Location Src</td>
                                    <td>:</td>
                                    <td colspan="2">
                                        <asp:DropDownList ID="ddlLocationSrc" runat="server" AutoPostBack="False" 
                                            CssClass="DropDownList" Width="150px" />
                                      
                                    </td> 
                                    
                                </tr>
                                
                                 <tr>
              
                                  <td>
                                          Warehouse Dest </td>
                                      <td>
                                          :</td>
                                      <td>
                                          <asp:DropDownList ID="ddlWrhsDest" runat="server" AutoPostBack="False" 
                                              CssClass="DropDownList" ValidationGroup="Input" Width="100px">
                                          </asp:DropDownList>
                                      </td> 
                                  </tr
                                
                                <tr>
                                    <td>
                                        Location Dest</td>
                                    <td>
                                        :</td>
                                    <td>
                                        <asp:DropDownList ID="ddlLocationDest" runat="server" AutoPostBack="False" 
                                            CssClass="DropDownList" Width="150px" />
                                    </td>
                                   
                                </tr> 
                                 
                                          
                                <tr>
                                    <td>
                                        Remark</td>
                                    <td>
                                        :</td>
                                    <td colspan="2">
                                        <asp:TextBox ID="tbRemarkDt" runat="server" AutoPostBack="False" 
                                            CssClass="TextBox" Height="41px" MaxLength="60" TextMode="MultiLine" 
                                            Width="354px" />
                                    </td>
                                    
                                     
                                </tr>
                                
                          
                        </table>                        
                       
                        <br />
                        <asp:Button class="bitbtndt btnsave" runat="server" ID="btnSaveDt" Text="Save" />
                        <asp:Button class="bitbtndt btncancel" runat="server" ID="btnCancelDt" Text="Cancel" />
                    </asp:Panel>
                    
                    
                    <br />
                    <br />
                    <div style="font-size:medium; ">Summary Product TRM</div>
                    <hr />
                    
                    <asp:Panel ID="pnlDt2" runat="server">
                        
                        <asp:Button class="bitbtndt btnadd" runat="server" ID="btnAddDt2" Text="Add" Visible = "false"  />
                        <asp:Button class="bitbtndt btnback" runat="server" ID="btnBackDt2ke1" Visible = "false" Text="Back" /> <br/>&nbsp;
                        <div style="border: 0px  solid; width: 100%; height: 100%; overflow: auto;">
                            <asp:GridView ID="GridDt2" runat="server" AutoGenerateColumns="false" ShowFooter="False">
                                <HeaderStyle CssClass="GridHeader" />
                                <RowStyle CssClass="GridItem" Wrap="false" />
                                <AlternatingRowStyle CssClass="GridAltItem" />
                                <PagerStyle CssClass="GridPager" />
                                
                                <Columns>
                                    <asp:TemplateField HeaderStyle-Width="110" HeaderText="Action" >
                                        <ItemTemplate>
                                            <asp:Button class="bitbtndt btnedit" runat="server" ID="btnEdit" Text="Edit" CommandName="Edit" />
                                            <asp:Button Visible = "false" class="bitbtndt btndelete" runat="server" ID="btnDelete" Text="Delete"  
                                                CommandName="Delete" OnClientClick="return confirm('Sure to delete this data?');" />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:Button class="bitbtndt btnsave" runat="server" ID="btnUpdate" Text="Save" CommandName="Update" />
                                            <asp:Button class="bitbtndt btnsave" runat="server" ID="btnCancel" Text="Cancel"  
                                                CommandName="Cancel" />
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="ProductCode" HeaderStyle-Width="150px" HeaderText="Product" />
                                    <asp:BoundField DataField="Product_Name" HeaderStyle-Width="90px" HeaderText="Product Name" />                                  
                                    <asp:BoundField DataField="Qty" DataFormatString="{0:#,##0.00}" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Right" HeaderText="Qty" />
                                    <asp:BoundField DataField="Unit" HeaderStyle-Width="80px" HeaderText="Unit" />
                                    
                                    <%--<asp:BoundField DataField="ProductRusakCode" HeaderStyle-Width="90px" HeaderText="Product Rusak" /> 
                                    <asp:BoundField DataField="ProductRusakName" HeaderStyle-Width="150px" HeaderText="Product Rusak Name" />--%>                                  
                                    <asp:BoundField DataField="QtyRusak" DataFormatString="{0:#,##0.00}" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Right" HeaderText="Qty Rusak" />
                                    <%--<asp:BoundField DataField="UnitRusak" HeaderStyle-Width="150px" HeaderText="Unit Rusak" /> --%>                                     
                                    <asp:BoundField DataField="Remark" HeaderStyle-Width="150px" HeaderText="Remark" />
                                </Columns>
                            </asp:GridView>
                        </div> 
                        <br/>
                        <asp:Button class="bitbtndt btnadd" runat="server" ID="btnAddDt2Ke2" Text="Add" Visible = "false"   />
                        <asp:Button class="bitbtndt btnback" runat="server" Visible = "false" ID="btnBackDt2ke2" Text="Back" />
                    </asp:Panel>
                    
                    
                    <asp:Panel runat="server" ID="pnlEditDt2" Visible="false">
                        <table>
                                <tr>
                                    <td>
                                        Product Material
                                    </td>
                                    
                                    <td>:</td>
                                    <td>
                                        <asp:TextBox ID="tbProductCodeDt" runat="server" Enabled="False"  AutoPostBack="true" 
                                            CssClass="TextBox" Width="150px" />
                                            <asp:TextBox ID="tbProductNameDt" runat="server" CssClass="TextBoxR" Enabled="False" 
                                            EnableTheming="True" ReadOnly="True" Width="200px" />
                                            <asp:Button Class="btngo" Visible="False"  ID="btnProductDt" Text= "..." runat="server" />     
                                    </td>                                   
                                </tr> 
                                
                                <tr>
                                    <td>
                                        Qty</td>
                                    <td>:</td>
                                    <td>
                                        <asp:TextBox ID="tbQtyDt" ValidationGroup="Input" Enabled="False"  runat="server" CssClass="TextBox" Width="80px" />
                                    </td>
                                    
                                      <td> Unit</td>
                                    <td>:</td>
                                    <td>
                                        <asp:TextBox ID="tbUnitDt" runat="server" Enabled="False"  CssClass="TextBox"
                                        Width="80px" />
                                    </td>
                                                       
                                </tr>
                                
                               
                                
                                
                                
                                <tr>
                                
                                <td>
                                        Wrhs Src</td>
                                    <td>
                                        :</td>
                                    <td>
                                        <asp:DropDownList ID="ddlWrhsSrcDt" runat="server" Enabled="False"  AutoPostBack="False" 
                                            CssClass="DropDownList" Width="150px" />
                                    </td>
                                    
                                    <td>
                                        Location Src</td>
                                    <td>:</td>
                                    <td colspan="2">
                                        <asp:DropDownList ID="ddlLocationSrcDt" runat="server" Enabled="False"  AutoPostBack="False" 
                                            CssClass="DropDownList" Width="150px" />
                                      
                                    </td> 
                                    
                                </tr>
                                
                                 <tr>
                                    <td>
                                        Wrhs Dest</td>
                                    <td>
                                        :</td>
                                    <td>
                                        <asp:DropDownList ID="ddlWrhsDestDt" runat="server" Enabled="False"  AutoPostBack="False" 
                                            CssClass="DropDownList" Width="150px" />
                                    </td>
                                    
                                    <td>
                                        Location Dest</td>
                                    <td>
                                        :</td>
                                    <td>
                                        <asp:DropDownList ID="ddlLocationDestDt" runat="server" Enabled="False"  AutoPostBack="False" 
                                            CssClass="DropDownList" Width="150px" />
                                    </td>
                                   
                                </tr> 
                               
                                <tr>
                                    <%--<td>
                                        Product Rusak
                                    </td>
                                    
                                    <td>:</td>--%>
                                    <td>
                                        <asp:TextBox ID="tbProductRusak" runat="server" Visible="False"  AutoPostBack="true" 
                                            CssClass="TextBox" Width="150px" />
                                            <asp:TextBox ID="tbProductRusakName" runat="server" CssClass="TextBoxR" Visible="False" 
                                            EnableTheming="True" ReadOnly="True" Width="200px" />
                                            <asp:Button Class="btngo" ID="btnProductRusak" Text= "..." Visible="False"  runat="server" />     
                                    </td>                                   
                                </tr> 
                                
                                <tr>
                                    <td>
                                        Qty Rusak</td>
                                    <td>:</td>
                                    <td>
                                        <asp:TextBox ID="tbQtyRusak" runat="server" CssClass="TextBox" Width="80px" />
                                    </td>
                                    
                                    <%--<td>
                                        Unit</td>
                                    <td>:</td>--%>
                                    <td>
                                        <asp:TextBox ID="tbUnitRusak" runat="server" CssClass="TextBox" Visible="false" 
                                        Width="80px" />
                                    </td>
                                                       
                                </tr>
                                
                                          
                                <tr>
                                    <td>
                                        Remark</td>
                                    <td>
                                        :</td>
                                    <td colspan="2">
                                        <asp:TextBox ID="tbRemarkDt2" runat="server" AutoPostBack="False" 
                                            CssClass="TextBox" Height="41px" MaxLength="60" TextMode="MultiLine" 
                                            Width="354px" />
                                    </td>
                                </tr>
                        </table>
                        <br />
                        <asp:Button class="bitbtndt btnsave" runat="server" ID="btnSaveDt2" Text="Save" />
                        <asp:Button class="bitbtndt btncancel" runat="server" ID="btnCancelDt2" Text="Cancel" />
                    </asp:Panel>
                </asp:View>
                
                
                <asp:View ID="Tab2" runat="server">
                    <table>
                        <tr>
                            <td>Jenis Mutasi</td>
                            <td>:</td>
                            <td>
                                <asp:Label ID="lbMutasi" runat="server" Text="Fixed Asset" />
                            </td>  
                             
                            <td>&nbsp; &nbsp;  &nbsp; &nbsp; &nbsp;  &nbsp; Jenis Dokumen</td>
                            <td>:</td>
                            <td>
                                <asp:Label ID="lblJenisDokumen" runat="server" Text="JenisDokumen" />
                            </td>  
                                                       
                        </tr>
                        
                        <tr>
                            <td>No Dokumen</td>
                            <td>:</td>
                            <td>
                                <asp:Label ID="lbNoDokumen" runat="server" />
                            </td>
                            
                        </tr>
                        <tr>
                        <td>Luas</td>
                            <td>:</td>
                            <td>
                                <asp:Label ID="lbLuas" runat="server" Text="Fixed Asset" /> 
                            </td>
                        </tr>
                    </table>
                    
                    <hr style="color: Blue" />
                    
                </asp:View>
                
                
                <asp:View ID="Tab3" runat="server">
               
                    <asp:Panel ID="pnlDt3" runat="server">
                    
                        <asp:Button class="bitbtn btnadd" runat="server" ID="btnAddDt3" Text="Add" ValidationGroup="Input" />
                        <asp:Button Class="bitbtndt btnsearch" ID="btnGetDt" Text="Get Data" runat="server" Visible="false" ValidationGroup="Input" />                                 	
             
                         
                        <div style="border: 0px  solid; width: 100%; height: 100%; overflow: auto;">
                        
                            <asp:GridView ID="GridDt3" runat="server" AutoGenerateColumns="False" ShowFooter="False">
                                <HeaderStyle CssClass="GridHeader" />
                                <RowStyle CssClass="GridItem" Wrap="false" />
                                <AlternatingRowStyle CssClass="GridAltItem" />
                                <PagerStyle CssClass="GridPager" />
                                <Columns>
                                    <asp:TemplateField HeaderText="Action"><ItemTemplate><asp:Button ID="btnEdit" runat="server" class="bitbtndt btnedit" Text="Edit" CommandName="Edit" /><asp:Button ID="btnDelete" runat="server" class="bitbtndt btndelete" Text="Delete"
                                                OnClientClick="return confirm('Sure to delete this data?');" CommandName="Delete" /></ItemTemplate><EditItemTemplate><asp:Button ID="btnUpdate" runat="server" class="bitbtndt btnsave" Text="Save" CommandName="Update" /><asp:Button ID="btnCancel" runat="server" class="bitbtndt btncancel" Text="Cancel"
                                                CommandName="Cancel" /></EditItemTemplate></asp:TemplateField>
                                                
                                  
                                    <asp:BoundField DataField="PaymentNo" HeaderStyle-Width="150px" 
                                        HeaderText="Payment No" SortExpression="PaymentNo" ><HeaderStyle Width="150px" />
                                    </asp:BoundField>
                                    
                                    <asp:BoundField DataField="Remark" HeaderStyle-Width="150px" 
                                        HeaderText="Remark" SortExpression="Remark" ><HeaderStyle Width="200px" />
                                    </asp:BoundField>
                                 
                                </Columns>
                            </asp:GridView>
                        </div>
                        <asp:Button class="bitbtn btnadd" runat="server" ID="btnAddDt3ke2" Text="Add" ValidationGroup="Input" />
                        
                    </asp:Panel>
                    <asp:Panel runat="server" ID="pnlEditDt3" Visible="false">
                        <table>
                            <tr>
                                <td>
                                    No Payment</td>
                                <td> : </td>
                                <td>
                                    <asp:TextBox ID="tbPaymentNoDt" Width = "225px" runat="server" Enabled ="True"
                                        CssClass="TextBox"  />
                                        <asp:Button Class="btngo" ID="btnPaymentNo" Text="..." runat="server" />
                                 </td>
                            </tr>
                            
                            <tr>
                                <td>
                                    Remark</td>
                                <td> : </td>
                                <td>
                                    <asp:TextBox ID="tbRemarkdt3" Width = "225px" TextMode = "MultiLine" runat="server" Enabled ="True"
                                        CssClass="TextBox"  />
                                 </td>
                            </tr>
                            
                            
                       
                        </table>
                        <br />
                        <asp:Button ID="btnSaveDt3" runat="server" class="bitbtndt btnsave" Text="Save" />
                        <asp:Button ID="btnCancelDt3" runat="server" class="bitbtndt btncancel" Text="Cancel" />
                    </asp:Panel>
                </asp:View>
            </asp:MultiView>
            <br />
            <asp:Button class="bitbtndt btnsavenew" runat="server" ID="btnSaveAll" Text="Save & New"
                ValidationGroup="Input" Width="100px" />
            <asp:Button class="bitbtndt btnsave" runat="server" ID="btnSaveTrans" Text="Save"
                ValidationGroup="Input" />
            <asp:Button class="bitbtndt btnback " runat="server" ID="btnBack" Text="Cancel" ValidationGroup="Input" />
            <asp:Button class="bitbtndt btnback" runat="server" ID="btnHome" Text="Home" />
        </asp:Panel>
    </div>
    <asp:Label runat="server" ID="lbStatus" ForeColor="Red" />
    
        <div class="loading" align="center">
      <br />
       <img src="../../Image/loader.gif" alt="" />
    </div>
    </form>
</body>
</html>
