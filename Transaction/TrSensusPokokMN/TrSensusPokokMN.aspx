<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TrSensusPokokMN.aspx.vb" Inherits="Transaction_TrSensusPokokMN" %>
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
    <title>Seleksi Pokok Per Module</title>
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
 <link href="https://fonts.googleapis.com/css2?family=Muli:wght@300;400;600&display=swap" rel="stylesheet">
    </head>

<body>     
    <form id="form1" runat="server">
     <div class="Content">
    <div class="H1">Seleksi Pokok Permodule</div>
     <hr style="color:Blue" />        
    <asp:Panel runat="server" id="PnlHd"> 
      <table>
        <tr>
            <td style="width:100px;text-align:right">Quick Search :</td>            
            <td><asp:TextBox CssClass="TextBox" runat="server" ID ="tbFilter"/> 
                  <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlField" >
                      <asp:ListItem Value="TransNmbr" Selected="True">Reference</asp:ListItem>
                      <%--<asp:ListItem Value="dbo.FormatDate(TransDate)">Date</asp:ListItem>--%>
                      <asp:ListItem>Status</asp:ListItem>
                      <asp:ListItem Value="SensusBy">Sensus By</asp:ListItem>
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
                      <asp:ListItem Value="SensusBy">Sensus By</asp:ListItem>
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
                  <asp:BoundField DataField="SensusBy"  HeaderText="Sensus By" sortExpression="SensusBy">
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
                  Sensus By</td>
              <td>
                  :</td>
              <td >
                  <asp:DropDownList ID="ddlSensusBy" runat="server" CssClass="TextBox" 
                      Height="21px" ValidationGroup="Input" Width="205px" />
              </td>
             
          </tr>
        <tr>
              <td>Remark</td>
              <td>:</td>
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
                    <asp:BoundField DataField="Module" HeaderStyle-Width="100px" 
                        HeaderText="Module" SortExpression="Module">
                    <HeaderStyle Width="100px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="ModuleName" HeaderStyle-Width="100px" 
                        HeaderText="Module Name" SortExpression="ModuleName">
                    <HeaderStyle Width="100px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="QtyMax" DataFormatString="{0:#,##0.##}" 
                        HeaderStyle-Width="100px" HeaderText="Qty Max" SortExpression="QtyMax">
                    <HeaderStyle Width="100px" />
                    <FooterStyle HorizontalAlign="Center" />
                    <ItemStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:BoundField DataField="QtyModule" DataFormatString="{0:#,##0.##}" 
                        HeaderStyle-Width="100px" HeaderText=" Qty Module" SortExpression="QtyModule">
                    <HeaderStyle Width="100px" />
                    <FooterStyle HorizontalAlign="Center" />
                    <ItemStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:BoundField DataField="QtyOk" DataFormatString="{0:#,##0.##}" 
                        HeaderStyle-Width="100px" HeaderText="OK" SortExpression="QtyOk">
                    <HeaderStyle Width="100px" />
                    <FooterStyle HorizontalAlign="Center" />
                    <ItemStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:BoundField DataField="QtyReject" DataFormatString="{0:#,##0.##}" 
                        HeaderStyle-Width="100px" HeaderText="Abnormal" SortExpression="QtyReject">
                    <HeaderStyle Width="100px" />
                    <FooterStyle HorizontalAlign="Center" />
                    <ItemStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:BoundField DataField="QtyRepair" DataFormatString="{0:#,##0.##}" 
                        HeaderStyle-Width="100px" HeaderText="Repair" SortExpression="QtyRepair">
                    <HeaderStyle Width="100px" />
                    <FooterStyle HorizontalAlign="Center" />
                    <ItemStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                    
                    <%--<asp:BoundField DataField="QtyRepair" DataFormatString="{0:#,##0.##}" 
                        HeaderStyle-Width="100px" HeaderText="OK" SortExpression="QtyRepair">
                    <HeaderStyle Width="100px" />
                    <FooterStyle HorizontalAlign="Center" />
                    <ItemStyle HorizontalAlign="Center" />
                    </asp:BoundField>--%>
                    
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
                                Setatus Tanam</td>
                            <td>:</td>
                            <td>
                                <asp:TextBox ID="tbModule" runat="server" AutoPostBack="true" 
                                    CssClass="TextBox" Width="85px" Enabled="False" />
                                <%--<asp:TextBox ID="tbName" runat="server" CssClass="TextBoxR" Enabled="False" 
                                    EnableTheming="True" ReadOnly="True" Width="126px" />
                                <asp:Button Class="btngo" ID="btnBlock" Text="..." runat="server" 
                                    ValidationGroup="Input" />  --%><asp:TextBox ID="tbModuleName" Visible="False" runat="server" 
                                    AutoPostBack="true" CssClass="TextBox" Enabled="False" Width="151px" />
                                <asp:Button ID="btnModule" runat="server" 
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
                                            Max</td>
                                        <td>
                                            Module</td>
                                        <td>
                                            Ok</td>
                                        <td>
                                            Abnormal</td>
                                        <td>
                                            Repair</td>
                                        <%--<td>
                                    Saldo Cap</td>--%>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="tbQtyMax" runat="server" CssClass="TextBoxR" Enabled="false" 
                                                Width="65px" />
                                        </td>
                                        <td>
                                            <asp:TextBox ID="tbQtyModule" runat="server" CssClass="TextBoxR" 
                                                Enabled="false" Width="65px" />
                                        </td>
                                        <td>
                                            <asp:TextBox ID="tbQtyOk" runat="server" CssClass="TextBoxR" Enabled="false" 
                                                Width="65px" />
                                        </td>
                                        <td>
                                            <asp:TextBox ID="tbQtyReject" runat="server" CssClass="TextBoxR" 
                                                Enabled="false" Width="65px" />
                                        </td>
                                        <td>
                                            <asp:TextBox ID="tbQtyRepair" runat="server" CssClass="TextBoxR" 
                                                Enabled="false" Width="65px" />
                                        </td>
                                        <%--<td>
                                    <asp:TextBox ID="tbSaldoCap" runat="server" CssClass="TextBoxR" Enabled="false" 
                                        Width="65px" />
                                </td>--%>
                                    </tr>
                                </table>
                            
                    
                    
                                <td>
                                    Remark</td>
                                <td>
                                    :</td>
                                <td>
                                    <asp:TextBox ID="tbRemarkDt" runat="server" CssClass="TextBox" 
                                        ValidationGroup="Input" Width="505px" />
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
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
        <asp:Panel ID="pnlDt2" runat="server">
            <asp:Button ID="btnAddDt3" runat="server" class="bitbtn btnadd" Text="Add" 
                ValidationGroup="Input" />
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
                        
                        <asp:BoundField DataField="BatchNo" HeaderStyle-Width="60px" 
                            HeaderText="Batch No" SortExpression="Module">
                            <HeaderStyle Width="60px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Module" HeaderStyle-Width="100px" 
                            HeaderText="Module" SortExpression="Module">
                            <HeaderStyle Width="100px" />
                        </asp:BoundField>
                        
                        <asp:BoundField DataField="ModuleName" HeaderStyle-Width="100px" 
                            HeaderText="Module Name" SortExpression="ModuleName">
                            <HeaderStyle Width="100px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="QtyModule" DataFormatString="{0:#,##0.##}" 
                            HeaderStyle-Width="100px" HeaderText="Module" SortExpression="QtyModule">
                            <HeaderStyle Width="100px" />
                            <FooterStyle HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:BoundField DataField="QtyOk" DataFormatString="{0:#,##0.##}" 
                            HeaderStyle-Width="100px" HeaderText="OK" SortExpression="QtyOk">
                            <HeaderStyle Width="100px" />
                            <FooterStyle HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:BoundField DataField="QtyReject" DataFormatString="{0:#,##0.##}" 
                            HeaderStyle-Width="100px" HeaderText="Abnormal" SortExpression="QtyReject">
                            <HeaderStyle Width="100px" />
                            <FooterStyle HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:BoundField DataField="QtyRepair" DataFormatString="{0:#,##0.##}" 
                            HeaderStyle-Width="100px" HeaderText="Repair" SortExpression="QtyRepair">
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
                                    ValidationGroup="Input" />  --%><asp:TextBox ID="tbBatchName" Visible="False" runat="server" 
                                        AutoPostBack="true" CssClass="TextBox" Enabled="False" Width="151px" />
                                    <asp:Button ID="btnBatch" runat="server" Class="btngo" Text="..." 
                                        ValidationGroup="Input" />
                                </td>
                                <td>
                                </td>
                            </tr>
                            
                            <tr>
                            <td>
                                Setatus Tanam</td>
                            <td>:</td>
                            <td>
                                <asp:TextBox ID="tbModuleDt" runat="server" AutoPostBack="true" 
                                    CssClass="TextBox" Width="85px" Enabled="False" />
                                <%--<asp:TextBox ID="tbName" runat="server" CssClass="TextBoxR" Enabled="False" 
                                    EnableTheming="True" ReadOnly="True" Width="126px" />
                                <asp:Button Class="btngo" ID="btnBlock" Text="..." runat="server" 
                                    ValidationGroup="Input" />  --%><asp:Button ID="btnModuleDt" runat="server" 
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
                                                Module</td>
                                            <td>
                                                Ok</td>
                                            <td>
                                                Abnormal</td>
                                            <td>
                                                Repair</td>
                                            <%--<td>
                                    Saldo Cap</td>--%>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="tbQtyModuleDt" runat="server" CssClass="TextBoxR" 
                                                    Enabled="false" Width="65px" />
                                            </td>
                                            <td>
                                                <asp:TextBox ID="tbQtyOkDt" runat="server" CssClass="TextBoxR" Enabled="false" 
                                                    Width="65px" />
                                            </td>
                                            <td>
                                                <asp:TextBox ID="tbQtyRejectDt" runat="server" CssClass="TextBoxR" 
                                                     Width="65px" AutoPostBack="true" />
                                            </td>
                                            <td>
                                                <asp:TextBox ID="tbQtyRepairDt" runat="server" CssClass="TextBoxR" 
                                                    Enabled="false" Width="65px" />
                                            </td>
                                            <%--<td>
                                    <asp:TextBox ID="tbSaldoCap" runat="server" CssClass="TextBoxR" Enabled="false" 
                                        Width="65px" />
                                </td>--%>
                                        </tr>
                                    </table>
                                </td>
                                <%--<tr>
                            <td class="style1">
                                Rotation</td>
                            <td class="style1">
                                :</td>
                            <td class="style1">
                                <asp:TextBox ID="tbRotation" runat="server" AutoPostBack="true" 
                                    CssClass="TextBox" ValidationGroup="Input" Width="110px" />
                            </td>
                        </tr>--%>
                                <%--<tr>
                            <td class="style1">
                                Week</td>
                            <td class="style1">
                                :</td>
                            <td class="style1">
                                <asp:TextBox ID="tbWeek" runat="server" AutoPostBack="true" 
                                    CssClass="TextBox" ValidationGroup="Input" Width="110px" />
                            </td>
                        </tr>--%>
                                <tr>
                                    <td>
                                        Remark</td>
                                    <td>
                                        :</td>
                                    <td>
                                        <asp:TextBox ID="tbRemarkDt2" runat="server" CssClass="TextBox" 
                                            ValidationGroup="Input" Width="505px" />
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
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
