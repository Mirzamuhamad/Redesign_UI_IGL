<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TrTemplate.aspx.vb" Inherits="TrTemplate" %>

<%@ Register Assembly="BasicFrame.WebControls.BasicDatePicker" Namespace="BasicFrame.WebControls"
    TagPrefix="BDP" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Master Template</title>

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
        <div class="H1">Master Template</div>
        <hr/>
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
                            <asp:ListItem Value="Remark">Nama Template</asp:ListItem>                        
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
                        <asp:TemplateField Visible ="false">
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
                                    <asp:ListItem Text="Delete" />
                                </asp:DropDownList>
                                <asp:Button class="btngo" runat="server" ID="BtnGo" Text="G" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>"
                                    CommandName="Go" />
                            </ItemTemplate>
                            <HeaderStyle Width="100px" />
                        </asp:TemplateField>
                        <asp:BoundField DataField="TransNmbr" HeaderStyle-Width="120px" SortExpression="TransNmbr"
                            HeaderText="No Template"></asp:BoundField>
                        <asp:BoundField DataField="Status" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="60px" HeaderText="Active"></asp:BoundField>
                            <asp:BoundField DataField="TransDate" DataFormatString="{0:dd MMM yyyy}" HtmlEncode="true"
                                HeaderStyle-Width="80px" SortExpression="TransDate" HeaderText="Date"></asp:BoundField>                          
                        <asp:BoundField DataField="Remark" HeaderStyle-Width="250px" HeaderText="Nama Template">
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
                    <td>No Template</td>
                    <td>:</td> 
                    <td width="250px">
                        <asp:TextBox CssClass="TextBoxR" runat="server" ID="tbCode" Width="150px" Enabled="False" />
                    </td>
                    <td>Date Template</td>
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
                    <td>Nama Template</td>
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
                    <asp:MenuItem Text="Detail Tahapan" Value="0"></asp:MenuItem>
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
                                            <asp:Button ID="btnView" runat="server" class="bitbtndt btnedit" Text="Sub Tahapan" Width="120px"
                                                CommandName="View" CommandArgument='<%# Container.DataItemIndex %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="ItemNo" HeaderStyle-Width="100px" HeaderText="No Item" ItemStyle-HorizontalAlign="Right"/>          
                                    <asp:BoundField DataField="Tahapan" HeaderStyle-Width="120px" HeaderText="Tahapan" />                                  
                                    <asp:BoundField DataField="Percen" HeaderStyle-Width="120px" HeaderText="Percen" ItemStyle-HorizontalAlign="Right" />
                                    <asp:BoundField DataField="TargetWaktu" HeaderStyle-Width="100px" HeaderText="Target Waktu" ItemStyle-HorizontalAlign="Right"/>
                                    <asp:BoundField DataField="Biaya1" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Right"
                                        HeaderText="Biaya1" DataFormatString="{0:#,##0.00}"/>
                                        <asp:BoundField DataField="Biaya2" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Right"
                                        HeaderText="Biaya2" DataFormatString="{0:#,##0.00}"/>
                                        <asp:BoundField DataField="PIC" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Right"
                                        HeaderText="PIC" Visible = "false"/>
                                        <asp:BoundField DataField="PICName" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Right"
                                        HeaderText="PICName" />
                                    <asp:BoundField DataField="SpvName1" HeaderStyle-Width="150px" HeaderText="SPV 1" />    
                                    <asp:BoundField DataField="SpvName2" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Right"
                                        HeaderText="SPV 2" />
                                        <asp:BoundField DataField="DireksiName" HeaderStyle-Width="150px" HeaderText="Direksi" />
                                    <asp:BoundField DataField="QcVerifiedName" HeaderStyle-Width="150px" HeaderText="Qc Verified" /> 
                                    <asp:BoundField DataField="FgPermanent" HeaderStyle-Width="150px" HeaderText="Exp Date Update" ItemStyle-HorizontalAlign="center" />   
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

                                    <asp:TextBox ID="tbTahapan" runat="server" CssClass="TextBox"
                                        ValidationGroup="Input" Width="225px" AutoPostBack="false" />
                                </td>
                            </tr>
                            <tr>
                                <td>Percen</td>
                                <td>:</td>
                                <td>

                                    <asp:TextBox ID="tbPercen" runat="server" CssClass="TextBox"
                                        ValidationGroup="Input" Width="225px"  />
                                </td>
                            </tr> 
                            <tr>
                                <td>Target waktu</td>
                                <td>:</td>
                                <td>

                                    <asp:TextBox ID="tbTargetWaktu" runat="server" CssClass="TextBox"
                                        ValidationGroup="Input" Width="225px"  />
                                </td>
                            </tr> 
                            <tr>
                                <td>Biaya 1</td>
                                <td>:</td>
                                <td>

                                    <asp:TextBox ID="tbBiaya1" runat="server" CssClass="TextBox"
                                        ValidationGroup="Input" Width="225px"  />
                                </td>
                            </tr>
                            <tr>
                                <td>Biaya 2</td>
                                <td>:</td>
                                <td>

                                    <asp:TextBox ID="tbBiaya2" runat="server" CssClass="TextBox"
                                        ValidationGroup="Input" Width="225px"  />
                                </td>
                            </tr> 
                            
                            <tr>
                                <td>PIC</td>
                                <td>:</td>
                                <td><asp:TextBox CssClass="TextBox" Width = 225 runat="server" ID="tbPICName" />
                                
                                <asp:TextBox CssClass="TextBox" Width = 225 runat="server" ID="tbPIC" Visible = "false"/>
                                <asp:Button class="btngo" runat="server" ID="btnPIC" Text="..." ValidationGroup="Input" />
                                </td>
                                
                            </tr>
                            
                             <tr>
                                <td>SPV 1</td>
                                <td>:</td>
                                <td><asp:TextBox CssClass="TextBox" Width = 225 runat="server" ID="tbSpv1Name" />
                                
                                <asp:TextBox CssClass="TextBox" Width = 225 runat="server" ID="tbSpv1" Visible = "false"/>
                                <asp:Button class="btngo" runat="server" ID="btnspv1" Text="..." ValidationGroup="Input" />
                                </td>
                                
                            </tr>
                            
                            <tr>
                                <td>SPV 2</td>
                                <td>:</td>
                                <td><asp:TextBox CssClass="TextBox" Width = 225 runat="server" ID="tbSpv2Name" />
                                
                                <asp:TextBox CssClass="TextBox" Width = 225 runat="server" ID="tbSpv2" Visible = "false"/>
                                <asp:Button class="btngo" runat="server" ID="btnSpv2" Text="..." ValidationGroup="Input" />
                                </td>
                                
                            </tr>
                            
                             <tr>
                                <td>Direksi</td>
                                <td>:</td>
                                <td><asp:TextBox CssClass="TextBox" Width = 225 runat="server" ID="tbDireksiName" />
                               
                                <asp:TextBox CssClass="TextBox" Width = 225 runat="server" ID="tbDireksi" Visible = "false"/>
                                <asp:Button class="btngo" runat="server" ID="btnDireksi" Text="..." ValidationGroup="Input" />
                                </td>
                                
                            </tr>
                            
                            <tr>
                                <td>Qc Verified</td>
                                <td>:</td>
                                <td><asp:TextBox CssClass="TextBox" Width = 225 runat="server" ID="tbQcName" />
                                
                                <asp:TextBox CssClass="TextBox" Width = 225 runat="server" ID="tbQc" Visible = "false"/>
                                <asp:Button class="btngo" runat="server" ID="btnQc" Text="..." ValidationGroup="Input" />
                                </td>                                
                            </tr>
                            
                             <tr>
                                <td class="style2">Exp Date Update</td>
                                <td>:</td>
                                <td>
                                <asp:DropDownList CssClass="DropDownList" runat="server" Width="230px" ID="ddlExtDateUpdate">
                                        <asp:ListItem  >Y</asp:ListItem>                                        
                                        <asp:ListItem Selected="True">N</asp:ListItem>                        
                                </asp:DropDownList>
                                </td>
                            </tr>
                           
                            
                            <tr>
                                <td >Remark</td>
                                <td>:</td>
                                <td>
                                    <asp:TextBox ID="tbRemarkDt" runat="server" CssClass="TextBox" Textmode = "MultiLine"  
                                        Width="500px" />
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
                            <td>Tahapan No</td>
                            <td>:</td>
                            <td>
                                <asp:Label ID="lbItem" runat="server" Text="Fixed Asset" />
                            </td>  
                             
                            <td>&nbsp; &nbsp;  &nbsp; &nbsp; &nbsp;  &nbsp; Tahapan</td>
                            <td>:</td>
                            <td>
                                <asp:Label ID="lbTahapan" runat="server" Text="JenisDokumen" />
                            </td>  
                                                       
                        </tr>
                        
                        
                    </table>
                    
                    <hr style="color: rgb(183, 183, 183)" />
                    
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
                                   <asp:BoundField DataField="ItemNoDt2" HeaderStyle-Width="100px" HeaderText="No Item" />          
                                    <asp:BoundField DataField="Tahapan" HeaderStyle-Width="120px" HeaderText="Tahapan" />                                  
                                    <asp:BoundField DataField="Percen" HeaderStyle-Width="120px" HeaderText="Percen" />
                                    <asp:BoundField DataField="TargetWaktu" HeaderStyle-Width="100px" HeaderText="Target Waktu" />
                                    <asp:BoundField DataField="Biaya1" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Right"
                                        HeaderText="Biaya1" DataFormatString="{0:#,##0.00}"/>
                                        <asp:BoundField DataField="Biaya2" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Right"
                                        HeaderText="Biaya2" DataFormatString="{0:#,##0.00}"/>
                                        <asp:BoundField DataField="PIC" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Right"
                                        HeaderText="PIC" Visible = "false"/>
                                        <asp:BoundField DataField="PICName" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Right"
                                        HeaderText="PICName" />
                                    <asp:BoundField DataField="SpvName1" HeaderStyle-Width="150px" HeaderText="SPV 1" />    
                                    <asp:BoundField DataField="SpvName2" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Right"
                                        HeaderText="SPV 2" />
                                        <asp:BoundField DataField="DireksiName" HeaderStyle-Width="150px" HeaderText="Direksi" />
                                    <asp:BoundField DataField="QcVerifiedName" HeaderStyle-Width="150px" HeaderText="Qc Verified" />
                                    <asp:BoundField DataField="FgPermanent" HeaderStyle-Width="150px" HeaderText="Exp Date Update" ItemStyle-HorizontalAlign="center" />   
                                    <asp:BoundField DataField="Remark" HeaderStyle-Width="200px" HeaderText="Remark" /> 
                                </Columns>
                            </asp:GridView>
                        </div> <br/>&nbsp;
                        <asp:Button class="bitbtndt btnadd" runat="server" ID="btnAddDt2Ke2" Text="Add" ValidationGroup="Input" />
                        <asp:Button class="bitbtndt btnback" runat="server" ID="btnBackDt2ke2" Text="Back" />
                    </asp:Panel>
                    <asp:Panel runat="server" ID="pnlEditDt2" Visible="false">
                        <table>
                            
                             <tr>
                                <td>Item No Dt</td>
                                <td>:</td>
                                <td><asp:Label ID="lbItemNoDt2" runat="server" Text="Item" />
                                </td>           
                             </tr> 
                             
                        

                            <tr>
                                <td>Reference</td>
                                <td>:</td>
                                <td>

                                    <asp:TextBox ID="tbTahapanDt2" runat="server" CssClass="TextBox"
                                        ValidationGroup="Input" Width="225px" AutoPostBack="false" />
                                </td>
                            </tr>
                            <tr>
                                <td>Percen</td>
                                <td>:</td>
                                <td>

                                    <asp:TextBox ID="tbPercenDt2" runat="server" CssClass="TextBox"
                                        ValidationGroup="Input" Width="225px"  />
                                </td>
                            </tr> 
                            <tr>
                                <td>Target waktu</td>
                                <td>:</td>
                                <td>

                                    <asp:TextBox ID="tbTargetWaktuDt2" runat="server" CssClass="TextBox"
                                        ValidationGroup="Input" Width="225px"  />
                                </td>
                            </tr> 
                            <tr>
                                <td>Biaya 1</td>
                                <td>:</td>
                                <td>

                                    <asp:TextBox ID="tbBiaya1Dt2" runat="server" CssClass="TextBox"
                                        ValidationGroup="Input" Width="225px"  />
                                </td>
                            </tr>
                            <tr>
                                <td>Biaya 2</td>
                                <td>:</td>
                                <td>

                                    <asp:TextBox ID="tbBiaya2Dt2" runat="server" CssClass="TextBox"
                                        ValidationGroup="Input" Width="225px"  />
                                </td>
                            </tr> 
                            
                            <tr>
                                <td>PIC</td>
                                <td>:</td>
                                <td><asp:TextBox CssClass="TextBox" Width = 225 runat="server" ID="tbPICNameDt2" />
                                
                                <asp:TextBox CssClass="TextBox" Width = 225 runat="server" ID="tbPICDt2" Visible = "false"/>
                                <asp:Button class="btngo" runat="server" ID="btnPICDt2" Text="..." ValidationGroup="Input" />
                                </td>
                                
                            </tr>
                            
                             <tr>
                                <td>SPV 1</td>
                                <td>:</td>
                                <td><asp:TextBox CssClass="TextBox" Width = 225 runat="server" ID="tbSpv1NameDt2" />
                                
                                <asp:TextBox CssClass="TextBox" Width = 225 runat="server" ID="tbSpv1Dt2" Visible = "false"/>
                                <asp:Button class="btngo" runat="server" ID="btnspv1Dt2" Text="..." ValidationGroup="Input" />
                                </td>
                                
                            </tr>
                            
                            <tr>
                                <td>SPV 2</td>
                                <td>:</td>
                                <td><asp:TextBox CssClass="TextBox" Width = 225 runat="server" ID="tbSpv2NameDt2" />
                                
                                <asp:TextBox CssClass="TextBox" Width = 225 runat="server" ID="tbSpv2Dt2" Visible = "false"/>
                                <asp:Button class="btngo" runat="server" ID="btnSpv2Dt2" Text="..." ValidationGroup="Input" />
                                </td>
                                
                            </tr>
                            
                             <tr>
                                <td>Direksi</td>
                                <td>:</td>
                                <td><asp:TextBox CssClass="TextBox" Width = 225 runat="server" ID="tbDireksiNameDt2" />
                                
                                <asp:TextBox CssClass="TextBox" Width = 225 runat="server" ID="tbDireksiDt2" Visible = "false"/>
                                <asp:Button class="btngo" runat="server" ID="btnDireksiDt2" Text="..." ValidationGroup="Input" />
                                </td>
                                
                            </tr>
                            
                            <tr>
                                <td>Qc Verified</td>
                                <td>:</td>
                                <td><asp:TextBox CssClass="TextBox" Width = 225 runat="server" ID="tbQcNameDt2" />
                                
                                <asp:TextBox CssClass="TextBox" Width = 225 runat="server" ID="tbQcDt2" Visible = "false"/>
                                <asp:Button class="btngo" runat="server" ID="btnQcDt2" Text="..." ValidationGroup="Input" />
                                </td>                                
                            </tr>
                            
                             <tr>
                                <td class="style2">Exp Date Update</td>
                                <td>:</td>
                                <td>
                                <asp:DropDownList CssClass="DropDownList" runat="server" Width="230px" ID="ddlExtDateUpdateDt2">
                                        <asp:ListItem>Y</asp:ListItem>                                        
                                        <asp:ListItem Selected="True">N</asp:ListItem>                        
                                </asp:DropDownList>
                                </td>
                            </tr>
                           
                            
                            <tr>
                                <td >Remark</td>
                                <td>:</td>
                                <td>
                                    <asp:TextBox ID="tbRemarkDtDt2" runat="server" CssClass="TextBox" Textmode = "MultiLine"  
                                        Width="400px" />
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
