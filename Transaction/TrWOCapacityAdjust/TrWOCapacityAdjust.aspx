<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TrWOCapacityAdjust.aspx.vb" Inherits="Transaction_TrWOCapacityAdjust" %>
<%@ Register Assembly="FastReport" Namespace="FastReport.Web" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="BasicFrame.WebControls.BasicDatePicker" Namespace="BasicFrame.WebControls"
    TagPrefix="BDP" %>
<%@ Register assembly="CrystalDecisions.Web, Version=10.5.3700.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Sickness Record</title>
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
    <style type="text/css">
        .style1
        {
            height: 23px;
        }
    </style>
    </head>

<body>     
    <form id="form1" runat="server">
     <div class="Content">
    <div class="H1">WO Capacity Adjust</div>
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
                      <asp:ListItem Value="WoNo">Wo No</asp:ListItem>
                      <asp:ListItem Value="CheckBy">Check By</asp:ListItem>
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
                      <asp:ListItem Value="WoNo">Wo No</asp:ListItem>
                      <asp:ListItem Value="CheckBy">Check By</asp:ListItem>
                      <asp:ListItem Value="Remark">Remark</asp:ListItem>                
              </asp:DropDownList>
          </td>              
        </tr>        
      </table>      
      </asp:Panel>
          <br />
            <asp:Button class="bitbtn btnadd" runat="server" ID="BtnAdd" Text="Add"  />	     
            &nbsp &nbsp &nbsp                 
            <asp:DropDownList CssClass="DropDownList" ID="ddlCommand" runat="server" Visible="false"/>
            <asp:Button class="btngo" runat="server" ID="BtnGo" Text="G" Visible="false" />     
          <br />
          <div style="border:0px  solid; width:100%; height:100%; overflow:auto;">
          <asp:GridView ID="GridView1" runat="server" AllowPaging="True" AllowSorting="True"
            CssClass="Grid" AutoGenerateColumns="False" Height="16px"> 
              <HeaderStyle CssClass="GridHeader"></HeaderStyle>
						<RowStyle CssClass="GridItem" Wrap="false" />
						<AlternatingRowStyle CssClass="GridAltItem"/>
						<PagerStyle CssClass="GridPager" />
              <Columns>              
                  <asp:TemplateField Visible = "False" >
                      <HeaderTemplate>
                          <asp:CheckBox Visible = "False" ID="cbSelectHd" runat="server" AutoPostBack="true" 
                          oncheckedchanged="cbSelectHd_CheckedChanged" />
                      </HeaderTemplate>
                      <ItemTemplate>
                          <asp:CheckBox Visible = "False" ID="cbSelect" runat="server" />
                      </ItemTemplate>
                  </asp:TemplateField>
                  <asp:TemplateField HeaderStyle-Width="110">
                      <ItemTemplate>
                          <asp:DropDownList CssClass="DropDownList" ID="ddl" runat="server">
                              <asp:ListItem Selected="True" Text="View" />
                              <asp:ListItem Text="Edit" />
                              
                          </asp:DropDownList>
                          <asp:Button class="btngo" runat="server" ID="BtnGo" Text="G" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" CommandName="Go"  />                                                 
                      </ItemTemplate>
                      <HeaderStyle Width="110px" />
                  </asp:TemplateField>                     
                  <asp:BoundField DataField="TransNmbr" SortExpression="TransNmbr" 
                      HeaderText="No"></asp:BoundField>                  
                                    
                  <asp:BoundField DataField="TransDate" HtmlEncode="true" DataFormatString="{0:dd MMM yyyy}" HeaderText="Date" SortExpression="TransDate"></asp:BoundField>                  
                  <asp:BoundField DataField="Status" HeaderText="Status" SortExpression="Status"></asp:BoundField>
                  <asp:BoundField DataField="WorkByName"  HeaderText="Work By" sortExpression="WorkByName">
                      <HeaderStyle Width="120px" />
                  </asp:BoundField> 
                  
                  <asp:BoundField DataField="ReffType"  HeaderText="Ref Type" sortExpression="RefType">
                    <HeaderStyle Width="120px" />
                  </asp:BoundField>
                  
                  <asp:BoundField DataField="Reference"  HeaderText="Reference" sortExpression="Reference">
                    <HeaderStyle Width="120px" />
                  </asp:BoundField>
                  
                  <asp:BoundField DataField="JobPlantName"  HeaderText="Job" sortExpression="Job">
                    <HeaderStyle Width="150px" />
                  </asp:BoundField>
                  
                  <asp:BoundField DataField="FgBorongan"  HeaderText="Kontraktor" sortExpression="Kontraktor">
                    <HeaderStyle Width="120px" />
                  </asp:BoundField>
                                 
                  <asp:BoundField DataField="Qty" DataFormatString="{0:#,##0.##}" HeaderStyle-Width="100px" 
                      HeaderText="Qty" SortExpression="Qty">
                      <HeaderStyle Width="100px" />
                  </asp:BoundField>
                  <asp:BoundField DataField="EstStartWeek" HeaderStyle-Width="100px" HeaderText="StartWeek">
                      <HeaderStyle Width="100px" />
                  </asp:BoundField>
                  
                  <asp:BoundField DataField="EstEndWeek" HeaderStyle-Width="100px" HeaderText="EndWeek">
                      <HeaderStyle Width="100px" />
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
                <asp:Label runat ="server" ID="Label1" ForeColor="Red" Text="*"/>
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
                  Divisi</td>
              <td>
                  :</td>
              <td>
                  <asp:DropDownList ID="ddlDivisi" runat="server" CssClass="TextBox" 
                      ValidationGroup="Input" Width="130px" />
                  <asp:Label ID="Label6" runat="server" ForeColor="Red" Text="*" />
              </td>
          </tr>
          <tr>
              <td>
                  WorkBy</td>
              <td>
                  :</td>
              <td >
                  <asp:DropDownList ID="ddlWorkBy" runat="server" CssClass="TextBox" 
                      ValidationGroup="Input" Width="130px" />
                  <asp:Label ID="Label5" runat="server" ForeColor="Red" Text="*" />
                  &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Person&nbsp; :&nbsp;
                  <asp:TextBox ID="tbPerson" runat="server" CssClass="TextBox" 
                      ValidationGroup="Input" Width="64px" />
              </td>
             
          </tr>
        <tr>
              <td>Job</td>
              <td>:</td>
              <td>
                  <asp:TextBox ID="tbJobCode" runat="server" CssClass="TextBox" 
                      ValidationGroup="Input" Width="128px" />
                  <asp:TextBox ID="tbJobName" runat="server" CssClass="TextBox" 
                      ValidationGroup="Input" Width="152px" />
                  &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Kapasitas :&nbsp;&nbsp;
                  <asp:TextBox ID="tbKapasitas" runat="server" CssClass="TextBox" 
                      ValidationGroup="Input" Width="85px" />
                  &nbsp;
                   <asp:Button ID="btnApply" runat="server" class="bitbtn btnsave" Text="Apply" 
                      />
              </td>
          </tr>
          
      </table>  
      
      <br />      
      <div style="font-size:medium; color:Blue;">Detail</div>
      <hr style="color:Blue" />  
        <asp:Panel runat="server" ID="pnlDt">
        <asp:Button class="bitbtn btnadd" runat="server" ID="btnAddDt" Text="Add" ValidationGroup="Input" />	     
                 
            <div style="border:0px  solid; width:100%; height:100%; overflow:auto;">
                <asp:GridView ID="GridDt" runat="server" AutoGenerateColumns="False" 
                    ShowFooter="True" Height="16px">
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
                        <asp:TemplateField  >
                          <HeaderTemplate>
                                  <asp:CheckBox  ID="cbSelect" runat="server" AutoPostBack="true" 
                                  oncheckedchanged="cbSelectDt_CheckedChanged" />
                           </HeaderTemplate>
                           <ItemTemplate>
                                  <asp:CheckBox  ID="cbSelect" runat="server" />
                           </ItemTemplate>
                        </asp:TemplateField>
                        
                        <asp:TemplateField  HeaderText="Type" HeaderStyle-Width="100px" SortExpression="Type" >
                           <Itemtemplate>
									<asp:Label  Runat="server" ID="lbType" text='<%# DataBinder.Eval(Container.DataItem, "type") %>' >
									
									</asp:Label>
								</Itemtemplate>
                        </asp:TemplateField>
                        
                        <asp:TemplateField  HeaderText="Divisi Blok" HeaderStyle-Width="100px" SortExpression="Blok" >
                           <Itemtemplate>
									<asp:Label  Runat="server" ID="lbBlok" text='<%# DataBinder.Eval(Container.DataItem, "DivisiBlok") %>' >
									
									</asp:Label>
								</Itemtemplate>
                        </asp:TemplateField>
                        
                        <asp:BoundField DataField="DivisiBlokName" HeaderStyle-Width="100px" HeaderText="Name" SortExpression="Name" >
                            <HeaderStyle Width="100px" />
                        </asp:BoundField>
                        
                        <asp:BoundField DataField="StatusTanam" HeaderText="Status Tanam" HeaderStyle-Width="100px" SortExpression="StatusTanam" >
                            <HeaderStyle Width="100px" />
                        </asp:BoundField>
                        
                        <asp:BoundField 
                            DataField="Percentage" HeaderText="Percentage" DataFormatString="{0:#,##0.##}" HeaderStyle-Width="100px"  SortExpression="Percentage" 
                            ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center" >
                            <FooterStyle HorizontalAlign="Center" />
                            <HeaderStyle Width="100px" />
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>
                        
                        <asp:BoundField DataField="QtyTotal" DataFormatString="{0:#,##0.##}" HeaderText="Qty Total" HeaderStyle-Width="100px" SortExpression="QtyTotal" >
                            <HeaderStyle Width="100px" />
                            <FooterStyle HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Center" /> 
                        </asp:BoundField>
                        
                        <asp:BoundField DataField="QtyCancel" DataFormatString="{0:#,##0.##}" HeaderText="Qty WO" HeaderStyle-Width="100px" SortExpression="QtyCancel" >
                            <HeaderStyle Width="100px" />
                            <FooterStyle HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Center" /> 
                        </asp:BoundField>
                        
                        <asp:TemplateField  HeaderText="Kapasitas HK" HeaderStyle-Width="100px" SortExpression="NormaHK" >
                           <Itemtemplate>
									<asp:TextBox OnTextChanged="tbKapasitas_TextChanged"  Runat="server" ID="TbKapasitas" text='<%# DataBinder.Eval(Container.DataItem, "NormaHK") %>' AutoPostBack="True">
									
									</asp:TextBox>
								</Itemtemplate>
                        </asp:TemplateField>
                        
                        <asp:TemplateField  HeaderText="Norma HK" HeaderStyle-Width="100px" SortExpression="NormaHk" >
                           <Itemtemplate>
									<asp:Label DataFormatString="{0:#,##0.##}" Runat="server" ID="lbNormaHk" text='<%# DataBinder.Eval(Container.DataItem, "NormaHk") %>' >
									
									</asp:Label>
								</Itemtemplate>
                        </asp:TemplateField>
                        
                        <asp:BoundField DataField="WorkDay" HeaderText="WorkDay" HeaderStyle-Width="100px" SortExpression="WorkDay" >
                            <HeaderStyle Width="100px" />
                            <FooterStyle HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Center" /> 
                        </asp:BoundField>
                        
                        <asp:BoundField DataField="TargetHK" DataFormatString="{0:#,##0.##}" HeaderText="Target HK" HeaderStyle-Width="100px" SortExpression="TargetHK" >
                            <HeaderStyle Width="100px" />
                            <FooterStyle HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Center" /> 
                        </asp:BoundField>
                        
                        <asp:BoundField 
                            DataField="StartDate" HtmlEncode="true" DataFormatString="{0:dd/mm/yy}" 
                            HeaderText="Start Date" SortExpression="StartDate" HeaderStyle-Width="120px" >
                            <FooterStyle HorizontalAlign="Center" />
                            <HeaderStyle Width="120px" />
                            <ItemStyle HorizontalAlign="Center" /> 
                        </asp:BoundField>
                        
                        <asp:BoundField 
                            DataField="EndDate" HtmlEncode="true" DataFormatString="{0:dd/mm/yy}" 
                            HeaderText="End Date" SortExpression="EndDate" HeaderStyle-Width="120px" > 
                            <FooterStyle HorizontalAlign="Center" />
                            <HeaderStyle Width="120px" />
                            <ItemStyle HorizontalAlign="Center" /> 
                        </asp:BoundField>
                        
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
                                Block Code</td>
                            <td>:</td>
                            <td>
                                <asp:TextBox ID="tbCode" runat="server" AutoPostBack="true" 
                                    CssClass="TextBox" ValidationGroup="Input" Width="108px" />
                                <asp:TextBox ID="tbName" runat="server" CssClass="TextBoxR" Enabled="False" 
                                    EnableTheming="True" ReadOnly="True" Width="126px" />
                                <asp:Button Class="btngo" ID="btnBlock" Text="..." runat="server" 
                                    ValidationGroup="Input" />                  
                             
                            </td>
                            <td>
                            </td>
                        </tr>
                        <tr>
                        <td>Propose Week</td>
                        <td>:</td>
                        <td>
                            <asp:DropDownList ID="ddlWeek" runat="server" AutoPostBack = "true" CssClass="TextBox" 
                                ValidationGroup="Input" Width="130px" />
                        </td>
                        </tr>
                        <tr>
                            <td>
                                Date</td>
                            <td>
                                :</td>
                            <td>
                                <table>
                                    <tr style="background-color: Silver; text-align: center">
                                        <td>
                                            Start Date</td>
                                        <td>
                                            End Date</td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="tbStartDate" runat="server" CssClass="TextBoxR" Enabled="False" 
                                                Width="80px" />
                                        </td>
                                        <td>
                                            <asp:TextBox ID="tbEndDate" runat="server" CssClass="TextBoxR" Enabled="False" 
                                                Width="80px" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="style1">
                                Density Panen</td>
                            <td class="style1">
                                :</td>
                            <td class="style1">
                                <asp:TextBox ID="tbDensity" AutoPostBack = "true" runat="server" CssClass="TextBox" 
                                    ValidationGroup="Input" Width="110px" />
                            </td>
                        </tr>
                        <tr>
                            <td class="style1">
                                Areal</td>
                            <td class="style1">
                                :</td>
                            <td class="style1">
                                <asp:TextBox ID="tbAreal" AutoPostBack = "true" runat="server" CssClass="TextBox" 
                                    ValidationGroup="Input" Width="110px" />
                            </td>
                        </tr>
                        <tr>
                            <td class="style1">
                                Berat TBS</td>
                            <td class="style1">
                                :</td>
                            <td class="style1">
                                <table>
                                    <tr style="background-color: Silver; text-align: center">
                                        <td>
                                            Start Weight</td>
                                        <td>
                                            End Weight</td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="tbStartWeight" AutoPostBack = "true" runat="server" CssClass="TextBoxR" Enabled="true" 
                                                Width="80px" />
                                        </td>
                                        <td>
                                            <asp:TextBox ID="tbEndWeight" AutoPostBack = "true" runat="server" CssClass="TextBoxR" Enabled="true" 
                                                Width="80px" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="style1">
                                Est. Tonase PAnen</td>
                            <td class="style1">
                                :</td>
                            <td class="style1">
                                <table>
                                    <tr style="background-color: Silver; text-align: center">
                                        <td>
                                            Tot Start Weight</td>
                                        <td>
                                            Tot End Weight
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="tbTotStartWeight" runat="server" CssClass="TextBoxR" Enabled="False" 
                                                Width="80px" />
                                        </td>
                                        <td>
                                            <asp:TextBox ID="tbTotEndWeight" runat="server" CssClass="TextBoxR" Enabled="False" 
                                                Width="80px" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <br />
                                Remark </td>
                            <td>
                                :</td>
                            <td>
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
       
       
       <asp:Button ID="btnSaveAll" runat="server" class="bitbtndt btnsavenew" Text="Save & New" validationgroup="Input" Width = "90"/>									
       <asp:Button ID="btnSaveTrans" runat="server" class="bitbtndt btnsave" Text="Save" validationgroup="Input"/>									
       <asp:Button ID="btnBack" runat="server" class="bitbtndt btncancel" Text="Cancel" validationgroup="Input"/>									
       <asp:Button ID="btnHome" runat="server" class="bitbtndt btnback" Text="Home" />									                                             
       
     
        <asp:Button ID="btnSaveEdit" runat="server" class="bitbtn btnsave" Text="Save" 
            ValidationGroup="Input" />
       
     
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
