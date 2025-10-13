<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TrTransferModule.aspx.vb" Inherits="TrTransferModule" %>
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
 <link href="https://fonts.googleapis.com/css2?family=Muli:wght@300;400;600&display=swap" rel="stylesheet">
    </head>

<body>     
    <form id="form1" runat="server">
     <div class="Content">
    <div class="H1">Transfer Module (PN to PM)</div>
     <hr style="color:Blue" />        
    <asp:Panel runat="server" id="PnlHd"> 
      <table>
        <tr>
            <td style="width:100px;text-align:right">Quick Search :</td>            
            <td><asp:TextBox CssClass="TextBox" runat="server" ID ="tbFilter"/> 
                  <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlField" >
                      <asp:ListItem Selected="True">TransNmbr</asp:ListItem>
                      <asp:ListItem >Work By</asp:ListItem>
                      <asp:ListItem Value="dbo.FormatDate(TransDate)">Date</asp:ListItem>                      
                      <asp:ListItem Value="BatchNo" >Batch No</asp:ListItem>
                      <asp:ListItem >Varietas Name</asp:ListItem>
                      <asp:ListItem >Bedeng Name</asp:ListItem>
                      <asp:ListItem>Qty PN</asp:ListItem>
                      <asp:ListItem>Qty MN</asp:ListItem>
                      <asp:ListItem>Abnormal</asp:ListItem>
                      <asp:ListItem>Repair</asp:ListItem>
                      <asp:ListItem>Saldo</asp:ListItem>
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
                  <asp:ListItem Selected="True">TransNmbr</asp:ListItem>
                      <asp:ListItem >Work By</asp:ListItem>
                      <asp:ListItem Value="dbo.FormatDate(TransDate)">Date</asp:ListItem>                      
                      <asp:ListItem Value="BatchNo" >Batch No</asp:ListItem>
                      <asp:ListItem >Varietas Name</asp:ListItem>
                      <asp:ListItem >Bedeng Name</asp:ListItem>
                      <asp:ListItem>Qty PN</asp:ListItem>
                      <asp:ListItem>Qty MN</asp:ListItem>
                      <asp:ListItem>Abnormal</asp:ListItem>
                      <asp:ListItem>Repair</asp:ListItem>
                      <asp:ListItem>Saldo</asp:ListItem>
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
                  <asp:BoundField DataField="Status" HeaderStyle-Width="50px" 
                      HeaderText="Status" />
                  <asp:BoundField DataField="TransDate" DataFormatString="{0:dd MMM yyyy}" 
                      HeaderStyle-Width="80px" HeaderText="Trans Date" />
                  <asp:BoundField DataField="TeamName" HeaderText="Work By" HeaderStyle-Width="100px" />
                  <asp:BoundField DataField="BatchNo" HeaderText="Batch No" />
                  <asp:BoundField DataField="VarietasName" HeaderText="Varietas" />
                  <asp:BoundField DataField="BedengName" HeaderText="bedeng" />
                  <asp:BoundField DataField="QtyPN" HeaderStyle-Width="50px" 
                      HeaderText="Qty PN" />
                  <asp:BoundField DataField="QtyMN" HeaderStyle-Width="50px" 
                      HeaderText="Qty MN" />
                  <asp:BoundField DataField="QtyReject" HeaderStyle-Width="50px" 
                      HeaderText="Abnormal" />
                  <asp:BoundField DataField="QtyRepair" HeaderStyle-Width="50px" 
                      HeaderText="Repair" />
                  <asp:BoundField DataField="QtySaldo" HeaderStyle-Width="50px" 
                      HeaderText="Saldo" />
                  <asp:BoundField DataField="Remark" HeaderStyle-Width="250px" 
                      HeaderText="Remark" />
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
            <td>Trans No</td>
            <td>:</td>
            <td>
                &nbsp;</td>
            <td>
                <asp:TextBox ID="tbRef" runat="server" CssClass="TextBoxR" enabled="false" />
            </td>
        </tr>
          <tr>
              <td>
                  Date</td>
              <td>
                  :<br />
              </td>
              <td>
                  &nbsp;</td>
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
                  Work By</td>
              <td>
                  :</td>
              <td>
                  &nbsp;</td>
              <td>
                  <asp:DropDownList ID="ddlWork" runat="server" CssClass="DropDownList" 
                      ValidationGroup="Input" Width="200px">
                  </asp:DropDownList>
              </td>
          </tr>
          <tr>
              <td>
                  Batch No</td>
              <td>
                  :</td>
              <td>
                  &nbsp;</td>
              <td>
                  <asp:TextBox ID="tbBatch" runat="server" AutoPostBack="True" 
                      CssClass="DropDownList" Width="119px" />
                  <asp:Button ID="btnBatchHD" runat="server" class="btngo" Text="..." 
                      ValidationGroup="Input" />
              </td>
          </tr>
          <tr>
              <td>
                  Varietas</td>
              <td>
                  :</td>
              <td>
                  &nbsp;</td>
              <td>
                  <asp:TextBox ID="tbVarietas" runat="server" CssClass="TextBox" Enabled="false" 
                      Width="196px" />
                  &nbsp; ID :&nbsp;
                  <asp:TextBox ID="tbVarietasID" runat="server" CssClass="TextBox" 
                      Enabled="false" Width="45px" />
              </td>
          </tr>
          <tr>
              <td>
                  Bedeng</td>
              <td>
                  :</td>
              <td>
                  &nbsp;</td>
              <td>
                  <asp:TextBox ID="tbBedeng" runat="server" CssClass="TextBox" Enabled="false" 
                      Width="196px" />
                  &nbsp; ID :&nbsp;
                  <asp:TextBox ID="tbBedengID" runat="server" CssClass="TextBox" Enabled="false" 
                      Width="45px" />
              </td>
          </tr>
          <tr>
              <td>
                  Qty
              </td>
              <td>
                  :</td>
              <td>
                  &nbsp;</td>
              <td>
                  <table>
                      <tr style="background-color: Silver; text-align: center">
                          <td class="style1">
                              PN</td>
                          <td class="style1">
                              MN</td>
                          <td class="style1">
                              Abnormal</td>
                          <td class="style1">
                              Repair</td>
                          <td class="style1">
                              Saldo</td>
                      </tr>
                      <tr>
                          <td>
                              <asp:TextBox ID="tbPN" runat="server" CssClass="TextBox" Enabled="False" />
                          </td>
                          <td>
                              <asp:TextBox ID="tbMNHd" runat="server" CssClass="TextBox" Enabled="False" 
                                  Width="80px" />
                          </td>
                          <td>
                              <asp:TextBox ID="tbAbnormalHd" runat="server" autopostback="true" Enabled="False" 
                                  CssClass="TextBox" Width="80px" />
                          </td>
                          <td>
                              <asp:TextBox ID="tbRepairHd" runat="server" autopostback="true"  Enabled="False"
                                  CssClass="TextBox" Width="80px" />
                          </td>
                          <td>
                              <asp:TextBox ID="tbSaldo" runat="server" CssClass="TextBox" Enabled="False" 
                                  Width="80px" />
                          </td>
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
                  &nbsp;</td>
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
                            <asp:Button ID="btnDelete" runat="server" class="bitbtndt btndelete" 
                                CommandName="Delete" 
                                OnClientClick="return confirm('Sure to delete this data?');" Text="Delete" />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:ImageButton ID="btnUpdate" runat="server" CommandName="Update" 
                                ImageAlign="AbsBottom" ImageUrl="../../Image/btnUpdateDtOn.png" 
                                onmouseout="this.src='../../Image/btnUpdateDtOn.png';" 
                                onmouseover="this.src='../../Image/btnUpdateDtOff.png';" />
                            <asp:ImageButton ID="btnCancel" runat="server" CommandName="Cancel" 
                                ImageAlign="AbsBottom" ImageUrl="../../Image/btnCancelDtOn.png" 
                                onmouseout="this.src='../../Image/btnCancelDtOn.png';" 
                                onmouseover="this.src='../../Image/btnCancelDtOff.png';" />
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Module" HeaderStyle-Width="80px" 
                        HeaderText="Module" />
                    <asp:BoundField DataField="QtyMax" HeaderStyle-Width="80px" DataFormatString="{0:#,##0.##}"
                        HeaderText="Max Cap" />
                    <asp:BoundField DataField="QtyUse" HeaderStyle-Width="80px" HeaderText="Use" DataFormatString="{0:#,##0.##}" />
                    <asp:BoundField DataField="QtyOK" HeaderStyle-Width="80px" HeaderText="MN" DataFormatString="{0:#,##0.##}" />
                    <asp:BoundField DataField="QtyRepair" HeaderStyle-Width="80px" DataFormatString="{0:#,##0.##}"
                        HeaderText="Repair" />
                    <asp:BoundField DataField="QtySaldo" HeaderStyle-Width="80px" DataFormatString="{0:#,##0.##}"
                        HeaderText="Saldo Cap" />
                    <asp:BoundField DataField="QtyReject" HeaderStyle-Width="80px" DataFormatString="{0:#,##0.##}"
                        HeaderText="Abnormal" />
                    <asp:BoundField DataField="Remark" HeaderStyle-Width="250px" 
                        HeaderText="Remark" />
                </Columns>

                </asp:GridView>
          </div>   
          <asp:Button class="bitbtn btnadd" runat="server" ID="btnAddDt2" Text="Add" ValidationGroup="Input" />	     
          
       </asp:Panel>             
       <asp:Panel runat="server" ID="pnlEditDt" Visible="false">
       <table>
                   <tr>
                    <td>
                        Module</td>
                    <td>
                        :</td>
                    <td>
                        <asp:TextBox ID="tbModule" runat="server" CssClass="TextBox" Enabled="False" 
                            EnableTheming="True" ReadOnly="True" Width="120px" />
                        <asp:Button ID="btnBatch" runat="server" class="btngo" Text="..." 
                            ValidationGroup="Input" />
                    </td>
                </tr>
                   <tr>
                       <td>
                           Module Name</td>
                       <td>
                           &nbsp;</td>
                       <td>
                           <asp:TextBox ID="tbModuleName" runat="server" CssClass="TextBox" Width="196px" />
                       </td>
                   </tr>
                <tr><td >Qty Pokok</td><td>:</td><td>
                <table>
                <tr style="background-color: Silver; text-align: center">
                    <td class="style">Max Cap</td>
                    <td class="style">Use</td>
                    <td class="style">MN</td>
                    <td class="style">Repair</td>                    
                    <td class="style">Saldo Cap</td>
                    <td class="style">Abnormal</td>
                </tr>
                <tr>
                    <td>
                        <asp:TextBox CssClass="TextBox" runat="server" ID="tbMaxCap" 
                            Enabled="False" Width="75px" DataFormatString="{0:#,##0.##}"
                         />                        
                    </td>                   
                    <td><asp:TextBox runat="server" CssClass="TextBox" ID="tbUse" Width="80px" autopostback="true"/>
                    </td>
                    <td>
                        <asp:TextBox ID="tbMN" runat="server" CssClass="TextBox" Width="80px" autopostback="true" Enabled="False"/>
                    </td>
                    <td>
                        <asp:TextBox ID="tbRepair" runat="server" CssClass="TextBoxR" Width="80px" 
                            Enabled="False"/>
                    </td>
                    <td>
                        <asp:TextBox ID="tbSaldoCap" runat="server" CssClass="TextBoxR" Enabled="False" 
                            Width="80px" />
                    </td>
                    <td>
                        <asp:TextBox ID="tbAbnormal" runat="server" CssClass="TextBoxR" Enabled="False" 
                            Width="80px" />
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
            <asp:Button class="bitbtn btnsave" runat="server" ID="btnSaveDt" Text="Save" CommandName="Update" />																						 																											
									
			<asp:Button class="bitbtn btncancel" runat="server" ID="btnCancelDt" Text="Cancel" CommandName="Cancel" />																						 													
 
            <br />
       </asp:Panel> 
       <br />    
       
       
       <asp:Button ID="btnSaveAll" runat="server" class="bitbtndt btnsavenew" Text="Save & New" validationgroup="Input" Width = "90"/>									
       <asp:Button ID="btnSaveTrans" runat="server" class="bitbtndt btnsave" Text="Save" validationgroup="Input"/>									
       <asp:Button ID="btnBack" runat="server" class="bitbtndt btncancel" Text="Cancel" validationgroup="Input"/>									
       <asp:Button ID="btnHome" runat="server" class="bitbtndt btnback" Text="Home" />									                                             
       
     
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
