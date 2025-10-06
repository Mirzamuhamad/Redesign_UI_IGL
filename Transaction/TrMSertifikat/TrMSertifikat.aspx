<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TrMSertifikat.aspx.vb" Inherits="TrMSertifikat" %>

<%@ Register Assembly="BasicFrame.WebControls.BasicDatePicker" Namespace="BasicFrame.WebControls"
    TagPrefix="BDP" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Mutasi sertifikat</title>

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

      function OpenPopupRef() {
                        var left = (screen.width - 700) / 2; //370
                        var top = (screen.height - 700) / 2;
                        window.open("../../earchDlgV.Aspx", "", 'toolbar=no, location=no, directories=no, status=no, menubar=no, scrollbars=yes, resizable=yes, copyhistory=no, width=' + 800 + ', height=' + 600 + ', top=' + top + ', left=' + left);
                        return false;
                    }
               
        
    </script>

 <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
 <link href="https://fonts.googleapis.com/css2?family=Muli:wght@300;400;600&display=swap" rel="stylesheet">
</head>
<body>
    <form id="form1" runat="server">
    <div class="Content">
        <div class="H1">Mutasi Sertifikat</div>
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
                            <asp:ListItem Value="AreaCode">Area</asp:ListItem>
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
                            <asp:ListItem Value="AreaCode">Area</asp:ListItem>
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
                        <asp:BoundField DataField="TransNmbr" HeaderStyle-Width="120px" SortExpression="TransNmbr"
                            HeaderText="No Mutasi"></asp:BoundField>
                        <asp:BoundField DataField="Status" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="60px" HeaderText="Status"></asp:BoundField>
                        <asp:BoundField DataField="TransDate" DataFormatString="{0:dd MMM yyyy}" HtmlEncode="true"
                            HeaderStyle-Width="80px" SortExpression="TransDate" HeaderText="Date"></asp:BoundField>
                            
                        <asp:BoundField DataField="AreaCode" HeaderStyle-Width="80px" SortExpression="AreaCode" HeaderText="Area Code"></asp:BoundField>
                        <asp:BoundField DataField="AreaName" HeaderStyle-Width="80px" SortExpression="AreaName" HeaderText="Area Name"></asp:BoundField>
                        <asp:BoundField DataField="Remark" HeaderStyle-Width="250px" HeaderText="Remark">
                        </asp:BoundField>
                    </Columns>
                </asp:GridView>
            </div>
            <br/>&nbsp; 
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
                    <td>No Mutasi</td>
                    <td>:</td> 
                    <td width="250px">
                        <asp:TextBox CssClass="TextBoxR" runat="server" ID="tbCode" Width="150px" Enabled="False" />
                    </td>
                    <td>Date Mutasi</td>
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
                        <td>Area</td>
                        <td>:</td>
                        <td><asp:TextBox runat="server" ValidationGroup="Input" ID="tbArea" CssClass="TextBox" Width="50px"/>
                        <asp:TextBox runat="server" ValidationGroup="Input" ID="tbAreaName" CssClass="TextBox" Width="175px"/>
                        <asp:Button Class="btngo" ID="btnArea" Text="..." runat="server" ValidationGroup="Input" /></td>
                </tr>
                
                <tr>
                    <td>Remark</td>
                    <td>:</td>
                    <td>
                        <asp:TextBox runat="server" ValidationGroup="Input" ID="tbRemark" CssClass="TextBox" Width="300px" />
                    </td>
                </tr>
            </table>
            
            <br />
           
            <asp:Menu ID="Menu1" runat="server" CssClass="Menu" StaticMenuItemStyle-CssClass="MenuItem"
                StaticSelectedStyle-CssClass="MenuSelect" Orientation="Horizontal" ItemWrap="False"
                StaticEnableDefaultPopOutImage="False">
                <Items>
                    <asp:MenuItem Text="Detail Transaction" Value="0"></asp:MenuItem>
                    <%--<asp:MenuItem Text="Detail No Payment" Value="1"></asp:MenuItem>--%>
                    <%-- <asp:MenuItem Text="Detail No Payment"  Value="2"></asp:MenuItem>--%>
                </Items>
            </asp:Menu>
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
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:Button ID="btnView" runat="server" class="bitbtndt btnedit" Text="Detail Dokumen" Width="120px"
                                                CommandName="View" CommandArgument='<%# Container.DataItemIndex %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="ItemNo" HeaderStyle-Width="100px" HeaderText="No Item" />          
                                    <asp:BoundField DataField="Reference" HeaderStyle-Width="120px" HeaderText="Reference" />                                  
                                    <asp:BoundField DataField="JenisMutasi" HeaderStyle-Width="120px" HeaderText="Jenis Mutasi" />
                                    <asp:BoundField DataField="JenisDokumen" HeaderStyle-Width="100px" HeaderText="Jenis Dokumen" />
                                    <asp:BoundField DataField="NoDokumen" HeaderStyle-Width="100px" HeaderText="No Dokumen" />
                                    <%--<asp:BoundField DataField="NoPayment" HeaderStyle-Width="120px" HeaderText="No Payment" />--%>
                                   
                                    <asp:BoundField DataField="Luas" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Right"
                                        HeaderText="Luas" />
                                        <asp:BoundField DataField="Nilai" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Right"
                                        HeaderText="Nilai" />
                                        <asp:BoundField DataField="Seller" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Right"
                                        HeaderText="Seller" Visible = "false"/>
                                        <asp:BoundField DataField="SellerName" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Right"
                                        HeaderText="SellerName" />
                                    <asp:BoundField DataField="UnitName" HeaderStyle-Width="150px" HeaderText="Object" />    
                                    <asp:BoundField DataField="SisaLuas" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Right"
                                        HeaderText="Sisa Luas" />
                                        <asp:BoundField DataField="Nama" HeaderStyle-Width="150px" HeaderText="Nama Akhir" />
                                    <asp:BoundField DataField="Info" HeaderStyle-Width="150px" HeaderText="Dokumen Info" />   
                                    <asp:BoundField DataField="Remark" HeaderStyle-Width="200px" HeaderText="Remark" /> 
                                </Columns>
                            </asp:GridView>
                        </div> <br/>
            
                        <asp:Button class="bitbtndt btnadd" runat="server" ID="btnAddDtke2" Text="Add" ValidationGroup="Input" />
                    </asp:Panel>
                    <asp:Panel runat="server" ID="pnlEditDt" Visible="false">
                        <table>
                             <tr>
                                <td>Item No</td>
                                <td>:</td>
                                <td><asp:Label ID="lbItemNo" runat="server" Text="Item" />
                                </td>           
                             </tr> 
                             
                            <tr>                    
                                <%--<td>Payment No</td>
                                <td>:</td>--%>
                                <td>                             
                                    <asp:TextBox CssClass="TextBox" runat="server" ID="tbPaymentNo"  Visible ="false" Width="225px"/> 
                                     <asp:TextBox runat="server" ID="tbFgValueDt2" Visible="false" />
                                       <%-- <asp:Label ID="lbItemNo5" runat="server" Text="*" />   --%>                             
                                </td>
                            </tr>

                            <tr>
                                <td>Reference</td>
                                <td>:</td>
                                <td>

                                    <asp:TextBox ID="tbReference" runat="server" CssClass="TextBox"
                                        ValidationGroup="Input" Width="190px" AutoPostBack="false" />
                                    <asp:Button ID="btnReference" runat="server" Class="btngo" Text="v"
                                        ValidationGroup="Input" />
                                </td>
                            </tr>
                    
                            <tr>
                                <td class="style3">Jenis Mutasi</td>
                                <td class="style4">:</td>
                                <td class="style4">
                                <asp:DropDownList CssClass="DropDownList" AutoPostBack = "true" Width="230px"  runat="server" ID="ddlJenisMutasi">
                                        <asp:ListItem Selected="True" >Peningkatan</asp:ListItem>
                                        <asp:ListItem >Penggabungan</asp:ListItem>
                                        <asp:ListItem >Pemecahan</asp:ListItem>
                                        <asp:ListItem >Penurunan</asp:ListItem>
                                        <asp:ListItem >Balik Nama</asp:ListItem>                        
                                </asp:DropDownList>
                                </td>
                            </tr>
                            
                            
                            <tr>
                                <td class="style2">Jenis Dokumen</td>
                                <td>:</td>
                                <td>
                                <asp:DropDownList CssClass="DropDownList" runat="server" Width="230px" ID="ddlJenisDokumen">
                                        <asp:ListItem Selected="True" >SHM</asp:ListItem>                                        
                                        <asp:ListItem >SPH</asp:ListItem>
                                        <asp:ListItem >SHGB</asp:ListItem>
                                        <asp:ListItem >SHGU</asp:ListItem>                        
                                </asp:DropDownList>
                                </td>
                            </tr>

                            <tr>
                                <td >No Dokumen</td>
                                <td>:</td>
                                <td>
                                    <asp:TextBox ID="tbNoDok" runat="server" CssClass="TextBox" MaxLength="255" 
                                        Width="225px" />
                                </td>
                            </tr>
                            
                            <tr>
                                <td >Luas</td>
                                <td>:</td>
                                <td>
                                    <asp:TextBox ID="tbLuas" runat="server" CssClass="TextBox" MaxLength="255" 
                                        Width="225px" />
                                </td>
                            </tr>

                            <tr>
                                <td>Nilai</td>
                                <td>:</td>
                                <td><asp:TextBox CssClass="TextBox" Width = 225 runat="server" ID="tbNilai" /></td>
                            </tr>
                            <tr>
                                <td>Seller</td>
                                <td>:</td>
                                <td><asp:TextBox CssClass="TextBox" Width = 225 runat="server" ID="tbsellerName" /></td>
                                <td><asp:TextBox CssClass="TextBox" Width = 225 runat="server" ID="tbSeller" Visible = "false"/></td>
                                
                            </tr>
                            
                            <tr>
                                <td>Object</td>
                                <td>:</td>
                                <td>
                                    <asp:TextBox CssClass="TextBox" ValidationGroup="Input" Width="50px" runat="server" ID="tbObject"
                                        AutoPostBack="true" />
                                    <asp:TextBox CssClass="TextBox" runat="server" ID="tbObjectName" Enabled="false" Width="165px" />
                                    <asp:Button class="btngo" runat="server" ID="btnObject" Text="..." ValidationGroup="Input" />
                                </td>
                            </tr>
                            
                            <tr>
                                <td > Sisa Luas</td>
                                <td>:</td>
                                <td>
                                    <asp:TextBox ID="tbSisaLuas" runat="server" CssClass="TextBox" MaxLength="255" 
                                        Width="225px" />
                                </td>
                            </tr>
                            
                            <tr>
                                <td>Nama Akhir</td>
                                <td>:</td>
                                <td><asp:TextBox CssClass="TextBox" Width = 225 runat="server" ID="tbNamaAkhir" /></td>
                            </tr>
                            
                            <tr>
                                <td > Info Dokumen</td>
                                <td>:</td>
                                <td>
                                    <asp:TextBox ID="tbInfo" runat="server" CssClass="TextBox" MaxLength="255" 
                                        Width="225px" />
                                </td>
                            </tr>
                            
                            <tr>
                                <td >Remark</td>
                                <td>:</td>
                                <td>
                                    <asp:TextBox ID="tbRemarkDt" runat="server" CssClass="TextBox" Textmode = "MultiLine"  
                                        Width="400px" />
                                </td>
                            </tr>

                            <tr>
                                <td>
                                    <asp:TextBox runat="server" ID="tbSumberObject" CssClass="TextBox" Visible = "False" 
                                        />
                                        <asp:TextBox runat="server" ID="tbNoSppt" CssClass="TextBox" Visible = "False" 
                                        />
                                </td>
                            </tr>

                          
                        </table>                        
                       
                        <br />
                        <asp:Button class="bitbtndt btnsave" runat="server" ID="btnSaveDt" Text="Save" />
                        <asp:Button class="bitbtndt btncancel" runat="server" ID="btnCancelDt" Text="Cancel" />
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
                            <td>&nbsp; &nbsp;  &nbsp; &nbsp; &nbsp;  &nbsp; Reference</td>
                            <td>:</td>
                            <td>
                                <asp:Label ID="lbReference" runat="server" Text="Reference" />
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
                    
                    <hr style="color: rgb(183, 183, 183)" />
                    <asp:Button Class="bitbtndt btnsearch" ID="btnGetreference" Text="Get Data" runat="server" Visible="false" ValidationGroup="Input" />                                 	
             
                    <asp:Panel ID="pnlDt2" runat="server">
                        <br />
                        <asp:Button class="bitbtndt btnadd" runat="server" ID="btnAddDt2" Text="Add" ValidationGroup="Input" />
                        <asp:Button class="bitbtndt btnback" runat="server" ID="btnBackDt2ke1" Text="Back" /> <br/>&nbsp;

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
                                            <asp:Button class="bitbtndt btndelete" runat="server" ID="btnDelete" Text="Delete"  
                                                CommandName="Delete" OnClientClick="return confirm('Sure to delete this data?');" />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:Button class="bitbtndt btnsave" runat="server" ID="btnUpdate" Text="Save" CommandName="Update" />
                                            <asp:Button class="bitbtndt btnsave" runat="server" ID="btnCancel" Text="Cancel"  
                                                CommandName="Cancel" />
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="NoDokDt2" HeaderStyle-Width="150px" HeaderText="No Dokumen" />
                                    <asp:BoundField DataField="JenisDokDt2" HeaderStyle-Width="90px" HeaderText="Jenis Dokumen" />
                                    <asp:BoundField DataField="UnitName" HeaderStyle-Width="150px" HeaderText="Object" />
                                    <asp:BoundField DataField="Luas" DataFormatString="{0:#,##0.00}" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Right" HeaderText="Luas" />
                                    <asp:BoundField DataField="Nama" HeaderStyle-Width="150px" HeaderText="Nama Awal" />
                                    <asp:BoundField DataField="Remark" HeaderStyle-Width="150px" HeaderText="Remark" />
                                </Columns>
                            </asp:GridView>
                        </div> <br/>&nbsp;
                        <asp:Button class="bitbtndt btnadd" runat="server" ID="btnAddDt2Ke2" Text="Add" ValidationGroup="Input" />
                        <asp:Button class="bitbtndt btnback" runat="server" ID="btnBackDt2ke2" Text="Back" />
                    </asp:Panel>
                    <asp:Panel runat="server" ID="pnlEditDt2" Visible="false">
                        <table>
                            <tr>
                                <td>No Dokumen</td>
                                <td>:</td>
                                <td><asp:TextBox CssClass="TextBox" Width = 225 runat="server" ID="tbDokumenDt" /></td>
                            </tr>
                            
                             <tr>
                                <td class="style2">Jenis Dokumen</td>
                                <td>:</td>
                                <td>
                                <asp:DropDownList CssClass="DropDownList" runat="server" Width="230px" ID="ddlJenisDokDt2">
                                        <asp:ListItem Selected="True" >SHM</asp:ListItem>                                        
                                        <asp:ListItem >SPH</asp:ListItem>
                                        <asp:ListItem >SHGB</asp:ListItem>
                                        <asp:ListItem >SHGU</asp:ListItem>  
                                        <asp:ListItem >AJB</asp:ListItem>                        
                                </asp:DropDownList>
                                </td>
                            </tr>
                            
                            
                            <tr>
                                <td>Luas</td>
                                <td>:</td>
                                <td><asp:TextBox CssClass="TextBox" Width = 225 runat="server" ID="tbLuasDt2" /></td>
                            </tr>
                            
                            
                            
                            <tr>
                                <td>Object</td>
                                <td>:</td>
                                <td>
                                    <asp:TextBox CssClass="TextBox" ValidationGroup="Input" Width="50px" runat="server" ID="tbUnit"
                                        AutoPostBack="true" />
                                    <asp:TextBox CssClass="TextBox" runat="server" ID="tbUnitName" Enabled="false" Width="175px" />
                                    <asp:Button class="btngo" runat="server" ID="btnUnit" Text="..." ValidationGroup="Input" />
                                </td>
                            </tr>
                            
                            <tr>
                                <td>Nama Awal</td>
                                <td>:</td>
                                <td><asp:TextBox CssClass="TextBox" Width = 225 runat="server" ID="tbNamaAwal" /></td>
                            </tr>
                            
                            
                            <tr>
                                <td>Remark</td>
                                <td>:</td>
                                <td>
                                    <asp:TextBox runat="server" ID="tbRemarkDt2" CssClass="TextBox" Width="365px" MaxLength="255"
                                        TextMode="MultiLine" />
                                </td>
                            </tr>

                            

                        </table>
                        <br />
                        <asp:Button class="bitbtndt btnsave" runat="server" ID="btnSaveDt2" Text="Save" />
                        <asp:Button class="bitbtndt btncancel" runat="server" ID="btnCancelDt2" Text="Cancel" />
                    </asp:Panel>
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
