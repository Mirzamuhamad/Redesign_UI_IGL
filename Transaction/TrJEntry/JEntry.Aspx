<%@ Page Language="VB" AutoEventWireup="false" CodeFile="JEntry.Aspx.vb" Inherits="JEntry" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<%@ Register Assembly="BasicFrame.WebControls.BasicDatePicker" Namespace="BasicFrame.WebControls"
    TagPrefix="BDP" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<%@ Register assembly="CrystalDecisions.Web, Version=10.5.3700.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title
    >
    <script src="../../Function/OpenDlg.js" type="text/javascript"></script>
    <script src="../../Function/Function.JS" type="text/javascript"></script>
    <link type="text/css" rel="stylesheet" href="../../Styles/circularprogress.css" /> 
    <script type="text/javascript" src="../../JQuery/jquery.min.js"></script>


    <script type="text/javascript">   
        function createajax()  
        { 
            var xmlhttp;
            if (window.XMLHttpRequest)
            {// code for IE7+, Firefox, Chrome, Opera, Safari
                xmlhttp=new XMLHttpRequest();
            }
            else
            {// code for IE6, IE5
                xmlhttp=new ActiveXObject("Microsoft.XMLHTTP");
            }      
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
        //var Rate = document.getElementById("tbRate");
        //var DrForex = document.getElementById("tbDebitForex");
        //var CrForex = document.getElementById("tbCreditForex");
        
         try
         {           
        //document.getElementById("tbDebitHome").value = Rate.value.replace(",","") * DrForex.value.replace(",","");
        //document.getElementById("tbCreditHome").value = Rate.value.replace(",","") * CrForex.value.replace(",","");        
        document.getElementById("tbNominal").value = setdigit(document.getElementById("tbNominal").value.replace(/\$|\,/g,""),'<%=ViewState("DigitHome")%>');
        document.getElementById("tbRate").value = setdigit(document.getElementById("tbRate").value.replace(/\$|\,/g,""),'<%=ViewState("DigitRate")%>');
        document.getElementById("tbDebitForex").value = setdigit(document.getElementById("tbDebitForex").value.replace(/\$|\,/g,""),'<%=ViewState("DigitCurr")%>');
        document.getElementById("tbCreditForex").value = setdigit(document.getElementById("tbCreditForex").value.replace(/\$|\,/g,""),'<%=ViewState("DigitCurr")%>');
        document.getElementById("tbDebitHome").value = setdigit(document.getElementById("tbDebitHome").value.replace(/\$|\,/g,""),'<%=ViewState("DigitHome")%>');
        document.getElementById("tbCreditHome").value = setdigit(document.getElementById("tbCreditHome").value.replace(/\$|\,/g,""),'<%=ViewState("DigitHome")%>');
        }catch (err){
            alert(err.description);
          }
      }


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
      
        
    </script>
    
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
</head>
<body>     
    <form id="form1" runat="server">    
    <div class="Content">
    <div class="H1">
        <asp:Label ID="lblTitle" runat="server" Text="Jurnal"></asp:Label>
        </div>
     <hr style="color:Blue" />        
    <asp:Panel runat="server" id="PnlHd"> 
      <table>
        <tr>
            <td style="width:100px;text-align:right">Quick Search :</td>            
            <td><asp:TextBox CssClass="TextBox" runat="server" ID ="tbFilter"/> 
                  <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlField" >
                      <asp:ListItem Selected="True">Reference</asp:ListItem>
                      <asp:ListItem Value="dbo.FormatDate(TransDate)">Date</asp:ListItem>                      
                      <asp:ListItem>Status</asp:ListItem>
                      <asp:ListItem Value="Format_Name">Format Journal</asp:ListItem>
                      <asp:ListItem Value="FgReport">Report</asp:ListItem>
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
                  <asp:ListItem Selected="True">Reference</asp:ListItem>
                  <asp:ListItem Value="dbo.FormatDate(TransDate)">Date</asp:ListItem>
                  <asp:ListItem>Status</asp:ListItem>
                  <asp:ListItem Value="Format_Name">Format Journal</asp:ListItem>
                  <asp:ListItem Value="FgReport">Report</asp:ListItem>
                  <asp:ListItem>Remark</asp:ListItem>
              </asp:DropDownList>
          </td>              
        </tr>        
      </table>      
      </asp:Panel>
          <br />
            <asp:Button class="bitbtn btnadd" runat="server" ID="BtnAdd" Text="Add"/>	 
            &nbsp;   
            <asp:DropDownList CssClass="DropDownList" ID="ddlCommand" runat="server" Visible="false"/>
            <asp:Button class="bitbtn btngo" runat="server" ID="BtnGo" Text="G" Visible="false"/>
            
          <br />&nbsp;
          <div style="border:0px  solid; width:100%; height:100%; overflow:auto;">
          <asp:GridView ID="GridView1" runat="server" AllowPaging="True" AllowSorting="true"
            CssClass="Grid" AutoGenerateColumns="false" > 
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
                          <asp:Button class="bitbtn btngo" runat="server" ID="btnGo" Text="G"	 
                          CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" CommandName="Go" />
                      </ItemTemplate>
                      <HeaderStyle Width="110px" />
                  </asp:TemplateField>
                  <asp:BoundField DataField="Reference" HeaderStyle-Width="120px" SortExpression="Nmbr" HeaderText="Reference"></asp:BoundField>                  
                  <asp:BoundField DataField="Status" SortExpression="Status" HeaderText="Status"></asp:BoundField>                  
                  <asp:BoundField DataField="TransDate" HtmlEncode="true" DataFormatString="{0:dd MMM yyyy}" HeaderStyle-Width="80px" SortExpression="TransDate" HeaderText="Date"></asp:BoundField>
                  <asp:BoundField DataField="FgReport" HeaderStyle-Width="50px" SortExpression="Report"  HeaderText="Report"></asp:BoundField>                 
                  <asp:BoundField DataField="TransClass" SortExpression="TransClass" HeaderText="Trans. Class"></asp:BoundField>
                  <asp:BoundField DataField="Format_Name" HeaderStyle-Width="200px" SortExpression="Format_Name" HeaderText="Format Journal"></asp:BoundField>
                  <asp:BoundField DataField="TransNmbr" HeaderStyle-Width="100px" SortExpression="TransNmbr" HeaderText="Trans. No"></asp:BoundField>
                  <asp:BoundField DataField="Debit" DataFormatString="{0:#,##0.00}" HeaderStyle-Width="100px" SortExpression="Debit" HeaderText="Debit" ItemStyle-HorizontalAlign="Right" ></asp:BoundField>
                  <asp:BoundField DataField="Credit" DataFormatString="{0:#,##0.00}" HeaderStyle-Width="100px" SortExpression="Credit" HeaderText="Credit" ItemStyle-HorizontalAlign="Right" ></asp:BoundField>                                  
                  <asp:BoundField DataField="Selisih" DataFormatString="{0:#,##0.00}" HeaderStyle-Width="100px" SortExpression="Selisih" HeaderText="Selisih" ItemStyle-HorizontalAlign="Right" ></asp:BoundField>                                  
                  <asp:BoundField DataField="Remark" HeaderStyle-Width="250px" HeaderText="Remark"></asp:BoundField>
              </Columns>
          </asp:GridView>
          </div>
          <br />
            <asp:Panel runat="server" ID ="pnlNav" Visible="false">
            <asp:Button class="bitbtn btnadd" runat="server" ID="btnAdd2" Text="Add"/>	 
                        
            &nbsp;
            <asp:DropDownList ID="ddlCommand2" CssClass="DropDownList" runat="server"/>
            <asp:Button class="bitbtn btngo" runat="server" ID="btnGo2" Text="G"/>	 
            </asp:Panel>
    </asp:Panel>    
    <asp:Panel runat="server" ID="pnlInput" Visible="false">
      <table>
        <tr>
            <td>Reference</td>
            <td>:</td>
            <td colspan="4"><asp:TextBox CssClass="TextBox" Enabled ="false" runat="server" ID="tbRef" Width="149px"/> <asp:Label runat="server" ID="lbTransClass" Visible="false"/>&nbsp; &nbsp; 
                <asp:Button class="bitbtn btnsearch" runat="server" ID="BtnGetDt" Text="Select" ValidationGroup="Input"/>
            </td>            
        </tr>
        <tr>
            <td>Date</td>
            <td>:</td>
            <td>
            <BDP:BasicDatePicker ID="tbDate" runat="server" DateFormat="dd MMM yyyy" 
                        ShowNoneButton = "false" ReadOnly = "true" ValidationGroup="Input"
                        ButtonImageHeight="19px" ButtonImageWidth="20px" 
                        DisplayType="TextBoxAndImage" 
                        TextBoxStyle-CssClass="TextDate" AutoPostBack="True">
                        <TextBoxStyle CssClass="TextDate" />
             </BDP:BasicDatePicker>                
            </td>            
            <td>
                Report</td>
            <td>
                :</td>
            <td>
                <asp:DropDownList ID="ddlReport" runat="server" AutoPostBack="true" 
                    CssClass="DropDownList" Enabled="true">
                    <asp:ListItem>Y</asp:ListItem>
                    <asp:ListItem>N</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>      
        <tr>
            <td><asp:LinkButton ID="lbFormatJE" ValidationGroup="Input"  runat="server" Text="Format JE"/></td>
            <td>:</td>
            <td colspan="4"><asp:DropDownList runat="server" ValidationGroup="Input" CssClass="DropDownList" ID="ddlJE" Width="200px" AutoPostBack="True"></asp:DropDownList>
                <asp:TextBox CssClass="TextBox" runat="server" ID="tbNominal" Width="100px"/> 
                <asp:Button class="btngo" runat="server" ID="BtnFormatJE" Text="^" ValidationGroup="Input" />	 
                          
            </td>
        </tr>
        <tr>
            <td>Total</td>
            <td>:</td>
            <td colspan="4">
                <table>
                    <tr style="background-color:Silver;text-align:center">
                        <td>Debit</td>
                        <td>Credit</td>
                        <td>Selisih</td>
                    </tr>
                    <tr>
                        <td><asp:TextBox ID="tbDebit" ValidationGroup="Input" runat="server" Width="180px" 
                                CssClass="TextBoxR"/></td>
                        <td><asp:TextBox ID="tbCredit" ValidationGroup="Input" runat="server" Width="180px" 
                                CssClass="TextBoxR"/></td>
                        <td><asp:TextBox ID="tbSelisih" ValidationGroup="Input" runat="server" Width="180px" 
                                CssClass="TextBoxR"/></td>
                        
                    </tr>
                </table>
            </td>                
        </tr>
        <tr>
            <td>Remark</td>
            <td>:</td>
            <td colspan="4"><asp:TextBox runat="server" Width="300px" ValidationGroup="Input" ID="tbRemark" MaxLength="255" CssClass="TextBoxMulti" TextMode="MultiLine" /></td>
        </tr>
      </table>  
      
      <br />      
      <div style="font-size:medium; color:Blue;">Detail</div>
      <hr style="color:Blue" />  
        <asp:Panel runat="server" ID="pnlDt">
           <asp:Button class="bitbtn btnadd" runat="server" ID="btnAddDt" Text="Add" ValidationGroup="Input" />	 
             <div style="border:0px  solid; width:100%; height:100%; overflow:auto;">
                <asp:GridView ID="GridDt" runat="server" AutoGenerateColumns="false" AllowPaging="true" 
                    ShowFooter="False">
                    <HeaderStyle CssClass="GridHeader" />
                    <RowStyle CssClass="GridItem" Wrap="false" />
                    <AlternatingRowStyle CssClass="GridAltItem" />
                    <PagerStyle CssClass="GridPager" />                    
                    <Columns>
                        <asp:TemplateField HeaderText="Action">
                            <ItemTemplate>
                                <asp:Button ID="btnEdit" runat="server" class="bitbtn btnedit" Text="Edit" CommandName="Edit"/>
                                <asp:Button ID="btnDelete" runat="server" class="bitbtndt btndelete" Text="Delete" OnClientClick="return confirm('Sure to delete this data?');" CommandName="Delete"/>						                                      
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:ImageButton ID="btnUpdate" runat="server"  
                                    ImageUrl="../../Image/btnUpdateDtOn.png"
                                    onmouseover="this.src='../../Image/btnUpdateDtOff.png';"
                                    onmouseout="this.src='../../Image/btnUpdateDtOn.png';"
                                    ImageAlign="AbsBottom" CommandName="Update" />   
                                <asp:ImageButton ID="btnCancel" runat="server"  
                                    ImageUrl="../../Image/btnCancelDtOn.png"
                                    onmouseover="this.src='../../Image/btnCancelDtOff.png';"
                                    onmouseout="this.src='../../Image/btnCancelDtOn.png';"
                                    ImageAlign="AbsBottom" CommandName="Cancel" />    
                            </EditItemTemplate>
                            
                        </asp:TemplateField>
                        <asp:BoundField DataField="ItemNo" HeaderText="No" />
                        <asp:BoundField DataField="Account" HeaderStyle-Width="80px" HeaderText="Account" />
                        <asp:BoundField DataField="AccountName" HeaderStyle-Width="250px"  HeaderText="Account Name" />
                        <asp:BoundField DataField="Subled_Name" HeaderStyle-Width="250px" HeaderText="Subled Name" />
                        <asp:BoundField DataField="CostCtrName" HeaderText="Cost Center" />                        
                        <asp:BoundField DataField="Currency" HeaderText="Currency" />
                        <asp:BoundField DataField="DebitForex"  DataFormatString="{0:#,##0.00}"  HeaderStyle-Width="80px" HeaderText="Debit Forex" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right"/>
                        <asp:BoundField DataField="DebitHome"  DataFormatString="{0:#,##0.00}" HeaderStyle-Width="80px" HeaderText="Debit Home" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right"/>
                        <asp:BoundField DataField="CreditForex"  DataFormatString="{0:#,##0.00}" HeaderStyle-Width="80px" HeaderText="Credit Forex" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right"/>
                        <asp:BoundField DataField="CreditHome"  DataFormatString="{0:#,##0.00}" HeaderStyle-Width="80px" HeaderText="Credit Home" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right"/>                                                    
                        <asp:BoundField DataField="Remark" HeaderStyle-Width="250px" HeaderText="Remark" />
                        
                        
                    </Columns>
                </asp:GridView>
          </div>   
          
          <asp:Button class="bitbtn btnadd" runat="server" ID="btnAddDt2" Text="Add" ValidationGroup="Input" />	 
          
       </asp:Panel>             
       <asp:Panel runat="server" ID="pnlEditDt" Visible="false">
            <table>
                <tr>
                    <td>Item No</td>
                    <td>:</td>
                    <td><asp:Label runat="server" ID="lbItemNo" /> </td>
                </tr>
                <tr>
                    <td><asp:LinkButton ID="lbAccount"  runat="server" Text="Account"/> </td>
                    <td>:</td>
                    <td><asp:TextBox runat="server" ID="tbAccCode" CssClass="TextBox" AutoPostBack="true" />
                        <asp:TextBox runat="server"  CssClass="TextBox"
                            ID="tbAccName" EnableTheming="True" ReadOnly="True" Enabled="False" 
                            Width="200px"/> 
                        <asp:Button class="btngo" runat="server" ID="btnAcc" Text="..." ValidationGroup="Input"/>            
                    </td>           
                    <td><asp:TextBox runat="server" Visible = "false" CssClass="TextBox" ID="tbfgSubled"/>
                        <asp:TextBox runat="server" Visible = "false" CssClass="TextBox" ID="tbFgCostCtr"/>
                    </td>         
                </tr>                                    
                
                <tr>
                    <td>Subled</td>
                    <td>:</td>
                    <td><asp:TextBox CssClass="TextBox" runat="server" ID="tbSubled" AutoPostBack="true" />
                        <asp:TextBox CssClass="TextBox" runat="server" 
                            ID="tbSubledName" Enabled="False" Width="200px" />
                        <asp:Button class="btngo" runat="server" ID="btnSubled" Text="..." ValidationGroup="Input"/>            
                    </td>
                </tr>
                <tr>
                    <td><asp:LinkButton ID="lbCostCtr"  runat="server" Text="Cost Ctr"/></td>
                    <td>:</td>
                    <td><asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlCostCtr" 
                            AutoPostBack="True"/></td>
                </tr>
                <tr>
                    <td><asp:LinkButton ID="lbCurr"  runat="server" Text="Currency"/>&nbsp/ Rate </td>
                    <td>:</td>
                    <td><asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlCurr" Enabled="false" AutoPostBack="True"/>
                        <asp:TextBox CssClass="TextBox" runat="server" ID="tbRate" />                                                                                                                       </td>
                </tr>
                <tr>
                    <td>Debit Forex / Home </td>
                    <td>:</td>
                    <td><asp:TextBox runat="server" CssClass="TextBox" ID="tbDebitForex" />
                        <asp:TextBox CssClass="TextBoxR" runat="server" ID="tbDebitHome"/>                        
                     </td>
                </tr>
                <tr>
                    <td>Credit Forex / Home</td>
                    <td>:</td>                    
                    <td><asp:TextBox runat="server" CssClass="TextBox" ID="tbCreditForex" />
                        <asp:TextBox CssClass="TextBoxR" runat="server" ID="tbCreditHome" />                        
                    </td>
                </tr>
                <tr>
                    <td>Remark </td>
                    <td>:</td>
                    <td><asp:TextBox runat="server" Width="300px" ID="tbRemarkDt" MaxLength="255" CssClass="TextBoxMulti" TextMode="MultiLine" />                        
                    </td>
                </tr>
            </table>
            <br />                     
            <asp:Button ID="btnSaveDt" runat="server" class="bitbtndt btnsave" Text="Save"/> &nbsp;         
            <asp:Button ID="btnCancelDt" runat="server" class="bitbtndt btncancel" Text="Cancel"/> 
       </asp:Panel> 
       <br />          
       <asp:Button ID="btnSaveAll" runat="server" class="bitbtndt btnsave" 
            Text="Save & New" ValidationGroup="Input" Width="97px"/> &nbsp;    
        <asp:Button ID="btnSaveTrans" runat="server" class="bitbtndt btnsave" Text="Save" ValidationGroup="Input"/> &nbsp;    
        <asp:Button ID="btnBack" runat="server" class="bitbtndt btncancel" Text="Cancel" ValidationGroup="Input"/>  &nbsp;
        <asp:Button ID="btnHome" runat="server" class="bitbtndt btncancel" Text="Home"/>    
    </asp:Panel>
    <asp:Panel runat="server" ID="pnlPrint" Visible="false">
    <cr:crystalreportviewer ID="CrystalReportViewer1" runat="server" AutoDataBind="True"
               Height="1036px" Width="928px" />
    </asp:Panel>
      <br />            
    </div>
   
    <asp:Label runat ="server" ID="lbStatus" ForeColor="Red"/>
    
       <div class="loading" align="center">
      Loading. Please wait.<br />
      <br />
       <img src="../../Image/loader.gif" alt="" />
    </div>
    </form>
    </body>
</html>
