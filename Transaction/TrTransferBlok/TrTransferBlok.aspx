<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TrTransferBlok.aspx.vb" Inherits="Transaction_TrTransferBlok" %>
<%@ Register Assembly="FastReport" Namespace="FastReport.Web" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="BasicFrame.WebControls.BasicDatePicker" Namespace="BasicFrame.WebControls"
    TagPrefix="BDP" %>
<%@ Register assembly="CrystalDecisions.Web, Version=10.5.3700.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<script runat="server">

</script>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Transfer Block</title>
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
        document.getElementById("tbAmount").value = setdigit(document.getElementById("tbAmount").value.replace(/\$|\,/g,""),'<%=ViewState("DigitQty")%>');
        document.getElementById("tbTAmount").value = setdigit(document.getElementById("tbTAmount").value.replace(/\$|\,/g, ""), '<%=ViewState("DigitQty")%>');
        document.getElementById("tbTDepr").value = setdigit(document.getElementById("tbTDepr").value.replace(/\$|\,/g, ""), '<%=ViewState("DigitQty")%>');
        document.getElementById("tbTBalance").value = setdigit(document.getElementById("tbTBalance").value.replace(/\$|\,/g, ""), '<%=ViewState("DigitQty")%>');        
        
        
    } catch (err) {
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
    </head>

<body>     
    <form id="form1" runat="server">
     <div class="Content">
    <div class="H1">Transfer Block</div>
     <hr style="color:Blue" />        
    <asp:Panel runat="server" id="PnlHd"> 
      <table>
        <tr>
            <td style="width:100px;text-align:right">Quick Search :</td>            
            <td><asp:TextBox CssClass="TextBox" runat="server" ID ="tbFilter"/> 
                  <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlField" >
                      <asp:ListItem Value="TransNmbr" Selected="True">Reference</asp:ListItem>
                      <asp:ListItem Value="dbo.FormatDate(TransDate)">Date</asp:ListItem>
                      <asp:ListItem>Status</asp:ListItem>
                      <asp:ListItem Value="Type">Type</asp:ListItem>
                      <asp:ListItem Value="Division">Division</asp:ListItem>
                      <asp:ListItem Value="CarNo">CarNo</asp:ListItem>
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
                  <asp:ListItem Value="TransNmbr" Selected="True">Reference</asp:ListItem>
                      <asp:ListItem Value="dbo.FormatDate(TransDate)">Date</asp:ListItem>
                      <asp:ListItem>Status</asp:ListItem>
                      <asp:ListItem Value="Type">Type</asp:ListItem>
                      <asp:ListItem Value="Division">Division</asp:ListItem>
                      <asp:ListItem Value="CarNo">CarNo</asp:ListItem>
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
                  <asp:BoundField DataField="CarNo"  HeaderText="Car No" sortExpression="CarNo">
                      <HeaderStyle Width="100px" />
                  </asp:BoundField> 
                  
                  <asp:BoundField DataField="CarName"  HeaderText="Car Name" sortExpression="CarName">
                      <HeaderStyle Width="100px" />
                  </asp:BoundField>  
                  
                  <asp:BoundField DataField="Type"  HeaderText="Type" sortExpression="Type">
                      <HeaderStyle Width="100px" />
                  </asp:BoundField>       
                  
                  <asp:BoundField DataField="QtyLangsir"  HeaderText="Qty Langsir" sortExpression="QtyLangsir" DataFormatString="{0:#,##0.##}">
                      <HeaderStyle Width="100px"  />
                  </asp:BoundField>
                  
                  <asp:BoundField DataField="QtyReject"  HeaderText="Qty Mati" sortExpression="QtyReject" DataFormatString="{0:#,##0.##}">
                      <HeaderStyle Width="100px" />
                  </asp:BoundField>
                  <asp:BoundField DataField="QtyRetur"  HeaderText="Retur" sortExpression="QtyRetur" DataFormatString="{0:#,##0.##}">
                      <HeaderStyle Width="100px" />
                  </asp:BoundField> 
                  <asp:BoundField DataField="QtyTanam"  HeaderText="Tanam" sortExpression="QtyTanam" DataFormatString="{0:#,##0.##}">
                      <HeaderStyle Width="100px" />
                  </asp:BoundField>       
                  
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
                <asp:Label ID="Label12" runat="server" ForeColor="Red" Text="*" />
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
              <td>
                  Car No</td>
              <td>
                  :</td>
              <td >
                  <asp:TextBox ID="tbCarNo" runat="server" AutoPostBack="true" 
                      CssClass="TextBox" Enabled="False" Width="85px" />
                  <asp:TextBox 
                      ID="tbCarName" runat="server" AutoPostBack="true" CssClass="TextBox" 
                      Enabled = "False" Width="151px" />
                  <asp:Button ID="btnCarNo" runat="server" Class="btngo" Text="..." 
                      ValidationGroup="Input" />
                  <asp:Label ID="Label13" runat="server" ForeColor="Red" Text="*" />
              </td>
             
          </tr>
        <tr>
              <td>Division</td>
              <td>:</td>
              <td>
                  <asp:DropDownList ID="ddlDivisi" runat="server" CssClass="TextBox" 
                      Height="21px" ValidationGroup="Input" Width="205px" />
              </td>
          </tr>
          
          <tr>
              <td>
                  Type</td>
              <td>
                  :</td>
              <td>
                  <asp:DropDownList ID="ddlType" runat="server" CssClass="TextBox"
                      Height="21px" ValidationGroup="Input" Width="138px" >
                              <asp:ListItem Selected="True" Text="Tanam" />
                              <asp:ListItem Text="Sisip" />
                  </asp:DropDownList>
              </td>
          </tr>
          <tr>
              <td>
                  Qty</td>
              <td>
                  :</td>
              <td>
                  <table>
                      <tr style="background-color: Silver; text-align: center">
                          <td>
                              Langsir</td>
                          <td>
                              Mati</td>
                          <td>
                              Retur</td>
                          <td>
                              Tanam</td>
                          <%--<td>
                                    Saldo Cap</td>--%>
                      </tr>
                      <tr>
                          <td>
                              <asp:TextBox ID="tbLangsir" runat="server" CssClass="TextBoxR" Enabled="false" 
                                  Width="65px" />
                          </td>
                          <td>
                              <asp:TextBox ID="tbReject" runat="server" CssClass="TextBoxR" 
                                  Enabled="false" Width="65px" />
                          </td>
                          <td>
                              <asp:TextBox ID="tbRetur" runat="server" CssClass="TextBoxR" Enabled="false" 
                                  Width="65px" />
                          </td>
                          <td>
                              <asp:TextBox ID="tbTanam" runat="server" CssClass="TextBoxR" 
                                  Enabled="false" Width="65px" />
                          </td>
                          <%--<td>
                                    <asp:TextBox ID="tbSaldoCap" runat="server" CssClass="TextBoxR" Enabled="false" 
                                        Width="65px" />
                                </td>--%>
                      </tr>
                  </table>
              </td>
          </tr>
          <tr>
              <td>
                  Remark</td>
              <td>
                  :</td>
              <td>
                  <asp:TextBox ID="tbRemark" runat="server" CssClass="TextBox" 
                      ValidationGroup="Input" Width="505px" />
                  &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
                  &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
                  &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
              </td>
          </tr>
          
      </table>  
      
      <br />      
      <div style="font-size:medium; color:Blue;">Detail</div>
      <hr style="color:Blue" />  
       <asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex="0">
                
                <asp:View ID="Tab2" runat="server">
                    
        <asp:Panel runat="server" ID="pnlDt">
        	
        <asp:Button class="bitbtn btnadd" runat="server" ID="btnAddDt" Text="Add" ValidationGroup="Input" />	     
                 
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
                    <%--<asp:BoundField DataField="JobPlant" HeaderStyle-Width="100px" HeaderText="JobPlant" SortExpression="JobPlant1" >
                            <HeaderStyle Width="100px" />
                        </asp:BoundField>--%>
                    <asp:TemplateField HeaderText="Action">
                        <ItemTemplate>
                            <%--<asp:Button ID="btnView" runat="server" class="bitbtndt btndetail" 
                                CommandArgument="<%# Container.DataItemIndex %>" 
                                CommandName="ViewA" Text="Machine" />
                            <asp:Button ID="btnViewE" runat="server" class="bitbtndt btndetail" 
                                CommandArgument="<%# Container.DataItemIndex %>" 
                                CommandName="ViewE" Text="Equipment" />--%>
                             <asp:Button ID="btnViewM" runat="server" class="bitbtndt btndetail" 
                                                CommandArgument="<%# Container.DataItemIndex %>" 
                                                CommandName="ViewM" Text="Material" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Block" HeaderStyle-Width="100px" 
                        HeaderText="Block" SortExpression="Block">
                    <HeaderStyle Width="100px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="WorkBy" HeaderStyle-Width="100px" 
                        HeaderText="Work By" SortExpression="WorkBy">
                    <HeaderStyle Width="100px" />
                    </asp:BoundField>
                    
                  
                    <asp:BoundField DataField="Mandor" HeaderStyle-Width="100px" 
                        HeaderText="Mandor" SortExpression="Mandor">
                    <HeaderStyle Width="100px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="MandorName" HeaderStyle-Width="100px" 
                        HeaderText="Mandor Name" SortExpression="MandorName">
                    <HeaderStyle Width="100px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="QtyLangsir" DataFormatString="{0:#,##0.##}" 
                        HeaderStyle-Width="100px" HeaderText="Langsir" SortExpression="QtyLangsir">
                    <HeaderStyle Width="100px" />
                    <FooterStyle HorizontalAlign="Center" />
                    <ItemStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:BoundField DataField="QtyMax" DataFormatString="{0:#,##0.##}" 
                        HeaderStyle-Width="100px" HeaderText=" Max Cap" SortExpression="QtyMax">
                    <HeaderStyle Width="100px" />
                    <FooterStyle HorizontalAlign="Center" />
                    <ItemStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:BoundField DataField="QtyUse" DataFormatString="{0:#,##0.##}" 
                        HeaderStyle-Width="100px" HeaderText="Use" SortExpression="QtyUse">
                    <HeaderStyle Width="100px" />
                    <FooterStyle HorizontalAlign="Center" />
                    <ItemStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:BoundField DataField="QtyTanam" DataFormatString="{0:#,##0.##}" 
                        HeaderStyle-Width="100px" HeaderText="Tanam" SortExpression="QtyTanam">
                    <HeaderStyle Width="100px" />
                    <FooterStyle HorizontalAlign="Center" />
                    <ItemStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:BoundField DataField="QtySaldo" DataFormatString="{0:#,##0.##}" 
                        HeaderStyle-Width="100px" HeaderText="Saldo Cap" SortExpression="QtySaldo">
                    <HeaderStyle Width="100px" />
                    <FooterStyle HorizontalAlign="Center" />
                    <ItemStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                    
                    <asp:BoundField DataField="QtyReject" DataFormatString="{0:#,##0.##}" 
                        HeaderStyle-Width="100px" HeaderText="Mati" SortExpression="QtyReject">
                    <HeaderStyle Width="100px" />
                    <FooterStyle HorizontalAlign="Center" />
                    <ItemStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                    
                    <asp:BoundField DataField="QtyRetur" DataFormatString="{0:#,##0.##}" 
                        HeaderStyle-Width="100px" HeaderText="Retur" SortExpression="QtyRetur">
                    <HeaderStyle Width="100px" />
                    <FooterStyle HorizontalAlign="Center" />
                    <ItemStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                    
                    
                    <asp:BoundField DataField="Remark" HeaderStyle-Width="200px" 
                        HeaderText="Remark">
                    <HeaderStyle Width="200px" />
                    </asp:BoundField>
                </Columns>
            </asp:GridView>
            <div style="border:0px  solid; width:100%; height:100%; overflow:auto;">
            </div>
          <asp:Button class="bitbtn btnadd" runat="server" ID="btnAddDt2" Text="Add" ValidationGroup="Input" />	     
          
            <br />
          
       </asp:Panel>             
       <asp:Panel runat="server" ID="pnlEditDt" Visible="false">
       <table width="100%">
            <tr>
                <td style="width:60%">                
                    <table>
                        <tr>
                            <td>
                                Block</td>
                            <td>:</td>
                            <td>
                                <asp:TextBox ID="tbBlock" runat="server" AutoPostBack="true" 
                                    CssClass="TextBox" Width="85px" Enabled="False" />
                                <asp:TextBox ID="tbBlockName"  runat="server" 
                                    AutoPostBack="true" CssClass="TextBox" Visible="true" Enabled="False" Width="151px" />
                                <asp:Button ID="btnBlock" runat="server" 
                                    Class="btngo" Text="..." ValidationGroup="Input" />
                            </td>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Work By</td>
                            <td>:</td>
                            <td>
                                <asp:DropDownList ID="ddlWorkBy" runat="server" CssClass="TextBox" Height="21px" 
                                    ValidationGroup="Input" Width="138px" />
                            </td>
                            <td>
                            </td>
                        </tr>
                        
                        <tr>
                            <td>
                                Mandor</td>
                            <td>:</td>
                            <td>
                                <asp:TextBox ID="tbMandor" runat="server" AutoPostBack="true" 
                                    CssClass="TextBox" Width="85px" Enabled="False" />
                                <asp:TextBox ID="tbMandorName"  runat="server" 
                                    AutoPostBack="true" CssClass="TextBox" Enabled="False" Width="151px" />
                                <asp:Button ID="btnMandor" runat="server" 
                                    Class="btngo" Text="..." ValidationGroup="Input" />
                            </td>
                            <td>
                            </td>
                        </tr>
                        
                        
                        
                        <tr>
                            <td>
                                Qty Pokok</td>
                            <td>
                                :</td>
                            <td colspan="2">
                                <table>
                                    <tr style="background-color: Silver; text-align: center">
                                        <td>
                                            Langsir</td>
                                        <td>
                                            Max Cap</td>
                                        <td>
                                            Use</td>
                                        <td>
                                            Tanam</td>
                                        <td>
                                            Saldo Cap</td>
                                        <td>
                                            Mati</td>
                                            <td>
                                                Retur</td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="tbQtyLangsir" runat="server" CssClass="TextBoxR" Enabled="false" 
                                                Width="65px" />
                                        </td>
                                        <td>
                                            <asp:TextBox ID="tbQtyMax" runat="server" CssClass="TextBoxR" 
                                                Enabled="false" Width="65px" />
                                        </td>
                                        <td>
                                            <asp:TextBox ID="tbQtyUse" runat="server" CssClass="TextBoxR" Enabled="false" 
                                                Width="65px" />
                                        </td>
                                        <td>
                                            <asp:TextBox ID="tbQtyTanam" runat="server" CssClass="TextBoxR" 
                                                Enabled="false" Width="65px" />
                                        </td>
                                        <td>
                                            <asp:TextBox ID="tbQtySaldo" runat="server" CssClass="TextBoxR" 
                                                Enabled="false" Width="65px" />
                                        </td>
                                        <td>
                                            <asp:TextBox ID="tbQtyReject" runat="server" CssClass="TextBoxR" 
                                                Enabled="false" Width="65px" />
                                        </td>
                                        <td>
                                            <asp:TextBox ID="tbQtyRetur" runat="server" CssClass="TextBoxR" 
                                                Enabled="false" Width="65px" />
                                        </td>                                 
                                    </tr>
                                </table>
                                
                          <tr>
                            <td>
                                Remark</td>
                            <td>
                                :</td>
                            <td>
                                <asp:TextBox ID="tbRemarkDt" runat="server" CssClass="TextBox" 
                                    ValidationGroup="Input" Width="505px" />
                            </td>
                         </tr>
                              
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
                </asp:View>
        <br />
          <asp:View ID="Tab3" runat="server">
                        
          <br />
        <asp:Panel ID="pnlDt2" runat="server">
           
            <asp:Button ID="btnAddDt3" runat="server" class="bitbtn btnadd" Text="Add" ValidationGroup="Input" />
                <asp:Button ID="btnBack2" runat="server" class="bitbtn btnadd" Text="Back" 
                ValidationGroup="Input" />
            <p />
             <asp:Label ID="LblCodeM" runat="server" ForeColor="Blue" Text="Block :" ></asp:Label>
          <asp:Label ID="LblCodeMA" runat="server" ForeColor="Blue" Text=""></asp:Label>
                                 <asp:Label ID="LblBlockNameM" runat="server"  ForeColor="Blue" Text="Block Name:"></asp:Label>
                                <asp:Label ID="LblBlockMA" runat="server" ForeColor="Blue" Text=""></asp:Label><br />
          <asp:Label ID="lblWorkByA" runat="server"  ForeColor="Blue" Text="Work By:"></asp:Label>
          <asp:Label ID="lblWorkBy" runat="server" ForeColor="Blue" Text=""></asp:Label>
                                 <br />
           
            <asp:GridView ID="GridDt2" runat="server" AutoGenerateColumns="False" 
                ShowFooter="True">
                <HeaderStyle CssClass="GridHeader" />
                <RowStyle CssClass="GridItem" Wrap="false" />
                <AlternatingRowStyle CssClass="GridAltItem" />
                <PagerStyle CssClass="GridPager" />
                <Columns>
                    <asp:TemplateField HeaderText="Action">
                        <ItemTemplate>
                            <asp:Button ID="btnEdit0" runat="server" class="bitbtn btnedit" 
                                CommandName="Edit" Text="Edit" />
                            <asp:Button ID="btnDelete0" runat="server" class="bitbtn btndelete" 
                                CommandName="Delete" 
                                OnClientClick="return confirm('Sure to delete this data?');" Text="Delete" />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Button ID="btnUpdate0" runat="server" class="bitbtn btnsave" 
                                CommandName="Update" Text="Save" />
                            <asp:Button ID="btnCancel0" runat="server" class="bitbtn btncancel" 
                                CommandName="Cancel" Text="Cancel" />
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <%--<asp:BoundField DataField="JobPlant" HeaderStyle-Width="100px" HeaderText="JobPlant" SortExpression="JobPlant1" >
                            <HeaderStyle Width="100px" />
                        </asp:BoundField>--%>
                    <%--  <asp:BoundField DataField="Block" HeaderStyle-Width="60px" 
                            HeaderText="Block" SortExpression="Block">
                            <HeaderStyle Width="60px" />
                        </asp:BoundField>--%>
                    <asp:BoundField DataField="BatchNo" HeaderStyle-Width="60px" 
                        HeaderText="Batch No" SortExpression="Module">
                    <HeaderStyle Width="60px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Block" HeaderStyle-Width="100px" HeaderText="Block" 
                        SortExpression="Block">
                    <HeaderStyle Width="100px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Type" HeaderStyle-Width="100px" HeaderText="Type" 
                        SortExpression="Type">
                    <HeaderStyle Width="100px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Varietas_Name" HeaderStyle-Width="100px" 
                        HeaderText="Varietas" SortExpression="Varietas_Name">
                    <HeaderStyle Width="100px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Module" HeaderStyle-Width="100px" 
                        HeaderText="Module" SortExpression="Module">
                    <HeaderStyle Width="100px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="SJManualNo" HeaderStyle-Width="100px" 
                        HeaderText="SJ Manual No" SortExpression="SJManualNo">
                    <HeaderStyle Width="100px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="QtyLangsir" DataFormatString="{0:#,##0.##}" 
                        HeaderStyle-Width="100px" HeaderText="Langsir" SortExpression="QtyLangsir">
                    <HeaderStyle Width="100px" />
                    <FooterStyle HorizontalAlign="Center" />
                    <ItemStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:BoundField DataField="QtyTanam" DataFormatString="{0:#,##0.##}" 
                        HeaderStyle-Width="100px" HeaderText="Tanam" SortExpression="QtyTanam">
                    <HeaderStyle Width="100px" />
                    <FooterStyle HorizontalAlign="Center" />
                    <ItemStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:BoundField DataField="QtyReject" DataFormatString="{0:#,##0.##}" 
                        HeaderStyle-Width="100px" HeaderText="Mati" SortExpression="QtyReject">
                    <HeaderStyle Width="100px" />
                    <FooterStyle HorizontalAlign="Center" />
                    <ItemStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:BoundField DataField="QtyRetur" DataFormatString="{0:#,##0.##}" 
                        HeaderStyle-Width="100px" HeaderText="Retur" SortExpression="QtyRetur">
                    <HeaderStyle Width="100px" />
                    <FooterStyle HorizontalAlign="Center" />
                    <ItemStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                    <%--<asp:BoundField 
                            DataField="EndDate" HtmlEncode="true" DataFormatString="{0:dd/mm/yy}" 
                            HeaderText="End Date" SortExpression="EndDate" HeaderStyle-Width="120px" > 
                            <FooterStyle HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Center" /> 
                        </asp:BoundField>--%>
                    <asp:BoundField DataField="Remark" HeaderStyle-Width="200px" 
                        HeaderText="Remark">
                    <HeaderStyle Width="200px" />
                    </asp:BoundField>
                </Columns>
            </asp:GridView>
                <div style="border:0px  solid; width:100%; height:100%; overflow:auto;">
            </div>
            <asp:Button ID="btnAddDt4" runat="server" class="bitbtn btnadd" Text="Add" 
                ValidationGroup="Input" />
            <br />
        </asp:Panel>
        <br />
        <asp:Panel ID="pnlEditDt2" runat="server" Visible="false">
            <table width="100%">
                <tr>
                    <td style="width:60%">
                        <table>
                            <tr>
                                <td>
                                    Batch No</td>
                                <td>
                                    :</td>
                                <td>
                                    <asp:TextBox ID="tbBatchNo" runat="server" AutoPostBack="true" 
                                        CssClass="TextBox" Enabled="False" Width="85px" />
                                    <%--<asp:TextBox ID="tbName" runat="server" CssClass="TextBoxR" Enabled="False" 
                                    EnableTheming="True" ReadOnly="True" Width="126px" />
                                <asp:Button Class="btngo" ID="btnBlock" Text="..." runat="server" 
                                    ValidationGroup="Input" />  --%><asp:Button ID="btnBatch" runat="server" Class="btngo" Text="..." 
                                        ValidationGroup="Input" />
                                    <asp:TextBox ID="tbBlockDt2" runat="server" AutoPostBack="true" 
                                        CssClass="TextBox" Enabled="False" Width="140px" />
                                </td>
                                <td>
                                </td>
                            </tr>
                            
                            <tr>
                                <td>
                                    Type</td>
                                <td>:</td>
                                <td>
                                    <asp:TextBox ID="tbtypeDt" runat="server" AutoPostBack="true" 
                                        CssClass="TextBox" Width="140px" Enabled="False" />
                                   </td>
                                <td>
                              </td>
                             </tr>
                             <tr>
                                <td>
                                    Varietas</td>
                                <td>:</td>
                                <td>
                                    <asp:TextBox ID="tbVarietas" runat="server" AutoPostBack="true" 
                                        CssClass="TextBox" Enabled="False" Width="140px" />
                                   </td>
                                <td>
                              </td>
                             </tr>
                             <tr>
                                <td>
                                    Module</td>
                                <td>:</td>
                                <td>
                                    <asp:TextBox ID="tbModuleDt" runat="server" AutoPostBack="true" 
                                        CssClass="TextBox" Width="140px" Enabled="False" />
                                   </td>
                                <td>
                              </td>
                             </tr>
                             <tr>
                                <td>
                                    SJ Manual No</td>
                                <td>:</td>
                                <td>
                                    <asp:TextBox ID="tbSjManual" runat="server" AutoPostBack="true" 
                                        CssClass="TextBox" Width="140px" Enabled="False" />
                                   </td>
                                <td>
                              </td>
                             </tr>
                        
                            <tr>
                                <td>
                                    Qty Pokok</td>
                                <td>
                                    :</td>
                                <td colspan="2">
                                    <table>
                                        <tr style="background-color: Silver; text-align: center">
                                            <td>
                                                Langsir</td>
                                            <td>
                                                Tanam</td>
                                            <td>
                                                Mati</td>
                                            <td>
                                                Retur</td>
                                            <%--<td>
                                    Saldo Cap</td>--%>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="tblangsirDt2" runat="server" CssClass="TextBox" 
                                                     Width="65px" AutoPostBack = "True" />
                                            </td>
                                            <td>
                                                <asp:TextBox ID="tbTanamDt2" runat="server" CssClass="TextBoxR" Enabled="false" 
                                                    Width="65px" />
                                            </td>
                                            <td>
                                                <asp:TextBox ID="tbQtyRejectDt2" runat="server" CssClass="TextBox" 
                                                     Width="65px" AutoPostBack="true"  />
                                            </td>
                                            <td>
                                                <asp:TextBox ID="tbReturDt2" runat="server" AutoPostBack = "True" CssClass="TextBox" 
                                                    Width="65px" />
                                            </td>
                                            <%--<td>
                                    <asp:TextBox ID="tbSaldoCap" runat="server" CssClass="TextBoxR" Enabled="false" 
                                        Width="65px" />
                                </td>--%>
                                        </tr>
                                    </table>
                                </td>
                                
                                 <tr>
                            <td>
                                Remark</td>
                            <td>
                                :</td>
                            <td>
                                <asp:TextBox ID="tbRemarkDt2" runat="server" CssClass="TextBox" 
                                    ValidationGroup="Input" Width="505px" />
                            </td>
                         </tr>
                              
                            </tr>
                        </table>
                    </td>
                    <td style="vertical-align:top;width:40%">
                        &nbsp;</td>
                </tr>
            </table>
            <br />
            <asp:Button ID="btnSaveDt2" runat="server" class="bitbtn btnsave" 
                CommandName="Update" Text="Save" />
            <asp:Button ID="btnCancelDt2" runat="server" class="bitbtn btncancel" 
                CommandName="Cancel" Text="Cancel" />
            <br />
        </asp:Panel>
         </asp:View>
         </asp:MultiView>
        <br />
       <br />    
       
       
       <asp:Button ID="btnSaveAll" runat="server" class="bitbtndt btnsavenew" Text="Save & New" validationgroup="Input" Width = "90"/>									
       <asp:Button ID="btnSaveTrans" runat="server" class="bitbtndt btnsave" Text="Save" validationgroup="Input"/>									
       <asp:Button ID="btnBack" runat="server" class="bitbtndt btncancel" Text="Cancel" validationgroup="Input"/>									
       <asp:Button ID="btnHome" runat="server" class="bitbtndt btnback" Text="Home" />									                                             
       
     
        &nbsp;									                                             
       
     
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
