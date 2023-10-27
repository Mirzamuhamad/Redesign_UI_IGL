<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TrPLSensusPokokTBM.Aspx.vb" Inherits="TrPLSensusPokokTBM" %>
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
                      <asp:ListItem Selected="True">TransNmbr</asp:ListItem>
                      <asp:ListItem >Rotasi</asp:ListItem>
                      <asp:ListItem Value="dbo.FormatDate(TransDate)">Date</asp:ListItem>                      
                      <asp:ListItem>Status</asp:ListItem>
                      <asp:ListItem Value="SensusByName">Sensus By</asp:ListItem>
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
                  <asp:ListItem Selected="True">TransNmbr</asp:ListItem>
                  <asp:ListItem >Rotasi</asp:ListItem>
                  <asp:ListItem Value="dbo.FormatDate(TransDate)">Date</asp:ListItem>
                  <asp:ListItem>Status</asp:ListItem>
                  <asp:ListItem Value="SensusByName">Sensus By</asp:ListItem>
                  <asp:ListItem>Remark</asp:ListItem>
              </asp:DropDownList>
          </td>              
        </tr>        
      </table>      
      </asp:Panel>
          <br />
            <asp:Button class="bitbtn btnadd" runat="server" ID="BtnAdd" Text="Add"/>	 
            &nbsp &nbsp &nbsp   
            <asp:DropDownList CssClass="DropDownList" ID="ddlCommand" runat="server" Visible="false"/>
            <asp:Button class="bitbtn btngo" runat="server" ID="BtnGo" Text="G" Visible="false"/>
            
          <br />
            
          <br />
          <div style="border:0px  solid; width:100%; height:100%; overflow:auto;">
              <asp:GridView ID="GridView1" runat="server" AllowPaging="True" 
                  AllowSorting="true" AutoGenerateColumns="false" CssClass="Grid">
                  <HeaderStyle CssClass="GridHeader" />
                  <RowStyle CssClass="GridItem" Wrap="false" />
                  <AlternatingRowStyle CssClass="GridAltItem" />
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
                              <asp:DropDownList ID="ddl" runat="server" CssClass="DropDownList">
                                  <asp:ListItem Selected="True" Text="View" />
                                  <asp:ListItem Text="Edit" />
                                  <asp:ListItem Text="Print" />
                              </asp:DropDownList>
                              <asp:Button ID="btnGo" runat="server" class="bitbtn btngo" 
                                  CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" CommandName="Go" 
                                  Text="G" />
                          </ItemTemplate>
                          <HeaderStyle Width="110px" />
                      </asp:TemplateField>
                      <asp:BoundField DataField="TransNmbr" HeaderStyle-Width="80px" 
                          HeaderText="Trans No" />
                      <asp:BoundField DataField="Status" HeaderStyle-Width="80px" 
                          HeaderText="Status" />
                      <asp:BoundField DataField="TransDate" DataFormatString="{0:dd MMM yyyy}" 
                          HeaderStyle-Width="80px" HeaderText="Trans Date" />
                      <asp:BoundField DataField="RotasiNo" HeaderText="Rotasi" />
                      <asp:BoundField DataField="SensusByName" HeaderText="Sensus By" />
                      <asp:BoundField DataField="Remark" HeaderStyle-Width="250px" 
                          HeaderText="Remark" />
                  </Columns>
              </asp:GridView>
          </div>
            <asp:Panel runat="server" ID ="pnlNav" Visible="false">
            <asp:Button class="bitbtn btnadd" runat="server" ID="btnAdd2" Text="Add"/>	 
                        
            &nbsp &nbsp &nbsp
            <asp:DropDownList ID="ddlCommand2" CssClass="DropDownList" runat="server"/>
            <asp:Button class="bitbtn btngo" runat="server" ID="btnGo2" Text="G"/>	 
            </asp:Panel>
    </asp:Panel>    
    <asp:Panel runat="server" ID="pnlInput" Visible="false">
      <table>
        <tr>
            <td>Trans No</td>
            <td>:</td>
            <td>
                <asp:TextBox ID="tbRef" runat="server" CssClass="TextBoxR" enabled ="false" />
            </td>
        </tr>
          <tr>
              <td>
                  Date</td>
              <td>
                  :<br />
              </td>
              <td>
                  <BDP:BasicDatePicker ID="tbDate" runat="server" AutoPostBack="True" 
                      ButtonImageHeight="19px" ButtonImageWidth="20px" DateFormat="dd MMM yyyy" 
                      DisplayType="TextBoxAndImage" ReadOnly="true" ShowNoneButton="false" 
                      TextBoxStyle-CssClass="TextDate" ValidationGroup="Input">
                      <TextBoxStyle CssClass="TextDate" />
                  </BDP:BasicDatePicker>
              </td>
          </tr>
        <tr>
              <td>
                  Rotasi</td>
              <td>
                  :</td>
              <td>
                  <asp:TextBox ID="tbRotasi" runat="server" CssClass="TextBox" Width="89px" />
              </td>
          </tr>
        <tr>
            <td>Sensus By</td>
            <td>:</td>
            <td>
                <asp:DropDownList ID="ddlSensus" runat="server"
                    CssClass="DropDownList" ValidationGroup="Input" Width="200px">
                </asp:DropDownList>
            </td>
        </tr>
          <tr>
              <td>
                  Remark</td>
              <td>
                  :</td>
              <td>
                  <asp:TextBox ID="tbRemark" runat="server" CssClass="TextBoxMulti" 
                      MaxLength="255" TextMode="MultiLine" ValidationGroup="Input" Width="300px" />
              </td>
          </tr>
      </table>  
      
      <br />      
      <div style="font-size:medium; color:Blue;">Detail</div>
      <hr style="color:Blue" />  
        <asp:Panel runat="server" ID="pnlDt">
           <asp:Button class="bitbtn btnadd" runat="server" ID="btnAddDt" Text="Add" ValidationGroup="Input" />	 
             <div style="border:0px  solid; width:100%; height:100%; overflow:auto;">
                <asp:GridView ID="GridDt" runat="server" AutoGenerateColumns="false" AllowPaging="true" 
                    ShowFooter="True">
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
                        
                        <asp:BoundField DataField="Block" HeaderStyle-Width="80px" HeaderText="Block" />
                        <asp:BoundField DataField="QtyMax" HeaderStyle-Width="80px" HeaderText="Last Sensus" />
                        <asp:BoundField DataField="QtyOK" HeaderStyle-Width="80px" HeaderText="Ok" />
                        <asp:BoundField DataField="QtySaldo" HeaderStyle-Width="80px" HeaderText="Saldo Cap" />                        
                        <asp:BoundField DataField="QtyAdjust" HeaderText="Adjust" HeaderStyle-Width="80px"/>
                        <asp:BoundField DataField="LuasLastSensus" HeaderText="Last Sensus" HeaderStyle-Width="80px"/>
                        <asp:BoundField DataField="LuasTanam" HeaderText="SDHI" HeaderStyle-Width="80px"/>
                        <asp:BoundField DataField="LuasAdjust" HeaderText="Luas" HeaderStyle-Width="80px"/>                                                
                        <asp:BoundField DataField="Remark" HeaderStyle-Width="250px" HeaderText="Remark" />
                        
                        
                        
                        
                    </Columns>
                </asp:GridView>
          </div>   
          
          <asp:Button class="bitbtn btnadd" runat="server" ID="btnAddDt2" Text="Add" ValidationGroup="Input" />	 
          
       </asp:Panel>             
       <asp:Panel runat="server" ID="pnlEditDt" Visible="false">
            <table>
                   <tr>
                    <td>
                        Block</td>
                    <td>
                        :</td>
                    <td>
                        <asp:DropDownList ID="ddlBlock" runat="server" AutoPostBack="True" 
                            CssClass="DropDownList" Width="119px" />
                    </td>
                </tr>
                <tr><td >Qty Tanam</td><td>:</td><td>
                <table>
                <tr style="background-color: Silver; text-align: center">
                    <td>Last Sensus</td>
                    <td>Ok</td>
                    <td>Saldo Cap</td>
                    <td>Adjust</td>                    
                </tr>
                <tr>
                    <td>
                        <asp:TextBox CssClass="TextBox" runat="server" ID="tbLastSensus" Enabled="False"
                         />                        
                    </td>                   
                    <td><asp:TextBox runat="server" CssClass="TextBox" ID="tbOk" Width="80px" autopostback="true"/>
                    </td>
                    <td>
                        <asp:TextBox ID="tbSaldoCap" runat="server" CssClass="TextBox" Width="80px" autopostback="true" Enabled="False"/>
                    </td>
                    <td>
                        <asp:TextBox ID="tbAdjust" runat="server" CssClass="TextBoxR" Width="80px" 
                            Enabled="False"/>
                    </td>
                </tr>
                
                       
                
                </table>
                </td>
                </tr>
                <tr>
                    <td>
                        Luas Tanam</td>
                    <td>
                        :</td>
                    <td>
                        <table>
                            <tr style="background-color: Silver; text-align: center">
                                <td>
                                    Last Luas Sensus</td>
                                <td>
                                    SDHI</td>
                                <td>
                                    Luas</td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:TextBox ID="tbLsensus" runat="server" autopostback="true" 
                                        CssClass="TextBox" Width="80px" Height="19px" />
                                </td>
                                <td>
                                    <asp:TextBox ID="tbSDHI" runat="server" autopostback="true" 
                                        CssClass="TextBox" Width="80px" Height="19px" />
                                </td>
                                <td>
                                    <asp:TextBox ID="tbLuasAdjust" runat="server" autopostback="true" 
                                        CssClass="TextBox" Width="80px" Enabled="false" />
                                </td>
                            </tr>
                        </table>
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
    
   
    </form>
    </body>
</html>
