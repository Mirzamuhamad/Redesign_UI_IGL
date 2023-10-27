<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TrSensusPokokPN.aspx.vb" Inherits="Transaction_TrSensusPokokPN_TrSensusPokokPN" %>
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
    </head>

<body>     
    <form id="form1" runat="server">
     <div class="Content">
    <div class="H1">Seleksi Pokok Perbatch</div>
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
                      <asp:ListItem Value="SensusByName">Sensus By</asp:ListItem>
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
                  <asp:ListItem Value="TransNmbr" Selected="True">Reference</asp:ListItem>
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
                                   
                  <asp:BoundField DataField="TransNmbr" HeaderStyle-Width="80px" HeaderText="Trans No" />
                        <asp:BoundField DataField="Status" HeaderStyle-Width="80px" HeaderText="Status" />
                        <asp:BoundField DataField="TransDate" HeaderStyle-Width="80px" DataFormatString="{0:dd MMM yyyy}"   HeaderText="Trans Date" />
                        <asp:BoundField DataField="SensusByName" HeaderText="Sensus By" />                        
                        <asp:BoundField DataField="Remark" HeaderStyle-Width="250px" HeaderText="Remark" />

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
                <asp:TextBox ID="tbRef" runat="server" CssClass="TextBoxR" enabled ="false" />
            </td>
        </tr>
          <tr>
              <td>
                  Date</td>
              <td>
                  :</td>
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
                <asp:GridView ID="GridDt" runat="server" AutoGenerateColumns="False" 
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
                        
                        <asp:BoundField DataField="BatchNo" HeaderStyle-Width="80px" HeaderText="Batch No" />
                        <asp:BoundField DataField="BatchDate" DataFormatString="{0:dd MMM yyyy}" HeaderStyle-Width="80px" HeaderText="Batch Date" />
                        <asp:BoundField DataField="VarietasName" HeaderStyle-Width="150px" HeaderText="Varietas" />
                        <asp:BoundField DataField="Rotasi" HeaderText="Rotasi" />                        
                        <asp:BoundField DataField="BedengName" HeaderText="Bedeng" HeaderStyle-Width="150px"/>
                        <asp:BoundField DataField="QtyBatch" DataFormatString="{0:#,##0}"  HeaderStyle-Width="80px" HeaderText="TRansfer Batch" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right"/>
                        <asp:BoundField DataField="QtyDoubleTone"  DataFormatString="{0:#,##0}" HeaderStyle-Width="80px" HeaderText="Double Tone" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right"/>
                        <asp:BoundField DataField="QtyReject"  DataFormatString="{0:#,##0}" HeaderStyle-Width="80px" HeaderText="Busuk" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right"/>
                        <asp:BoundField DataField="QtyPatah"  DataFormatString="{0:#,##0}" HeaderStyle-Width="80px" HeaderText="Patah" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right"/>  
                        <asp:BoundField DataField="QtySaldo" DataFormatString="{0:#,##0}" HeaderText="Tanam PN" />                                                  
                        <asp:BoundField DataField="Remark" HeaderStyle-Width="250px" HeaderText="Remark" />
                        
                        
                        
                        
                    </Columns>

                </asp:GridView>
          </div>   
          <asp:Button class="bitbtn btnadd" runat="server" ID="btnAddDt2" Text="Add" ValidationGroup="Input" />	     
          
       </asp:Panel>             
       <asp:Panel runat="server" ID="pnlEditDt" Visible="false">
       <table>
                <tr>
                    <td>Batch No</td>
                    <td>:</td>
                    <td>
                        <asp:TextBox runat="server"  CssClass="TextBox"
                            ID="tbBatchName" EnableTheming="True" ReadOnly="True" Enabled="False" 
                            Width="120px"/> 
                        <asp:Button class="btngo" runat="server" ID="btnBatch" Text="..." ValidationGroup="Input"/>            
                    </td>           
                    <td>
                        Batch Date</td>
                    <td>
                        :</td>
                    <td>
                        <BDP:BasicDatePicker ID="tbBatchDate" runat="server" AutoPostBack="True" 
                            ButtonImageHeight="19px" ButtonImageWidth="20px" DateFormat="dd MMM yyyy" 
                            DisplayType="TextBoxAndImage" ReadOnly="true" ShowNoneButton="false" 
                            TextBoxStyle-CssClass="TextDate" ValidationGroup="Input">
                            <TextBoxStyle CssClass="TextDate" />
                        </BDP:BasicDatePicker>
                    </td>   
                     <td>
                        </td>
                    <td>
                        </td>
                    <td>
                       
                    </td> 
                </tr>                                    
                
               
                
                <tr>
                    <td>Variates</td>
                    <td>:</td>
                    <td>
                        <asp:TextBox ID="tbVriates" runat="server" CssClass="TextBox" 
                         EnableTheming="True" ReadOnly="True" Enabled="False" />
                         <asp:TextBox ID="tbVriatesCode" runat="server" CssClass="TextBox" Enabled="False" Visible="False"
                            EnableTheming="True" ReadOnly="True" Width="25px" /> 
                    </td>
                     <td>
                        Rotasi</td>
                    <td>
                        :</td>
                    <td>
                        <asp:TextBox ID="tbRotasi" runat="server" CssClass="TextBox" Width="50px"/>
                    </td>
                     <td>
                        </td>
                    <td>
                        </td>
                    <td>
                       
                    </td>
                </tr>
                   <tr>
                    <td>
                        Bedeng</td>
                    <td>
                        :</td>
                    <td>
                        <asp:DropDownList ID="ddlBedeng" runat="server" AutoPostBack="True" 
                            CssClass="DropDownList" Width="119px" />
                            <asp:TextBox ID="tbBedengName" runat="server" CssClass="TextBox" Visible="False"
                            Enabled="False" EnableTheming="True" ReadOnly="True" Width="51px" />
                    </td>
                </tr>
                <tr><td >Qty</td><td>:</td><td colspan="7">
                <table>
                <tr style="background-color: Silver; text-align: center">
                    <td>Transfer Batch</td>
                    <td>Double Tone</td>
                    <td>Busuk</td>
                    <td>Patah</td>
                    <td>Tanam PN</td>                    
                </tr>
                <tr>
                    <td>
                        <asp:TextBox CssClass="TextBoxR" runat="server" ID="tbTransferBatch" Enabled="False"
                         />                        
                    </td>                   
                    <td><asp:TextBox runat="server" CssClass="TextBox" ID="tbDoubleTone" Width="80px" autopostback="true"/>
                    </td>
                    <td>
                        <asp:TextBox ID="tbBusuk" runat="server" CssClass="TextBox" Width="80px" autopostback="true"/>
                    </td>
                     <td>
                        <asp:TextBox ID="tbPatah" runat="server" CssClass="TextBox" Width="80px" autopostback="true"/>
                    </td>
                    <td>
                        <asp:TextBox ID="tbTanamPN" runat="server" CssClass="TextBoxR" Width="80px" Enabled="False"/>
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
