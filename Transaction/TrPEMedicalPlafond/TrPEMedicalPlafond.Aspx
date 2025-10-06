<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TrPEMedicalPlafond.Aspx.vb" Inherits="TrPEMedicalPlafond" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<%@ Register Assembly="BasicFrame.WebControls.BasicDatePicker" Namespace="BasicFrame.WebControls"
    TagPrefix="BDP" %>

<%--<%@ Register assembly="CrystalDecisions.Web, Version=12.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>--%>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
    <script src="../../Function/OpenDlg.js" type="text/javascript"></script>
    <script src="../../Function/Function.JS" type="text/javascript"></script>
    <script type="text/javascript"> 
        function opencekpostingdlg() {  
                window.open("../../Transaction/TrPEMedicalPlafond/FormCekPostPR.Aspx","List","scrollbars=yes,resizable=no,width=800,height=400");
                return false;
                          
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
            
             
            var AmountClaim = document.getElementById("tbAmountPlafond").value.replace(/\$|\,/g,""); 
            var AmountToPaid = document.getElementById("tbAmount1Plafond").value.replace(/\$|\,/g,""); 
                        
            
         try
         {                       
            
            document.getElementById("tbAmountPlafond").value = setdigit(AmountClaim,'<%=ViewState("DigitHome")%>');
            document.getElementById("tbAmount1Plafond").value = setdigit(AmountToPaid,'<%=ViewState("DigitHome")%>');
                        
            
            // document.getElementById("tbPrice").value = setdigit(PriceForex,'<%=VIEWSTATE("DigitCurr")%>');
            // document.getElementById("tbAmountForex").value = setdigit(AmountForex,'<%=VIEWSTATE("DigitCurr")%>');
        }catch (err){
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
    <div class="H1">Medical Plafond</div>
     <hr style="color:Blue" />        
    <asp:Panel runat="server" id="PnlHd"> 
      <table>
        <tr>    
            <td style="width:100px;text-align:right">Quick Search :</td>            
            <td><asp:TextBox CssClass="TextBox" runat="server" ID ="tbFilter"/> 
                  <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlField" >
                      <asp:ListItem Selected="True" Value="TransNmbr">Reference</asp:ListItem>                      
                      <asp:ListItem Value="dbo.FormatDate(TransDate)">Date</asp:ListItem>                      
                      <asp:ListItem>Status</asp:ListItem>   
                      <asp:ListItem Value="EffectiveDate">Effective Date</asp:ListItem>                
                      <asp:ListItem Value="Currency">Currency</asp:ListItem>                                                          
                      <asp:ListItem Value="RemarkHd">Remark</asp:ListItem>                                        
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
                      <asp:ListItem Selected="True" Value="TransNmbr">Reference</asp:ListItem>                      
                      <asp:ListItem Value="dbo.FormatDate(TransDate)">Date</asp:ListItem>                      
                      <asp:ListItem>Status</asp:ListItem>   
                      <asp:ListItem Value="EffectiveDate">Effective Date</asp:ListItem>                
                      <asp:ListItem Value="Currency">Currency</asp:ListItem>                                                          
                      <asp:ListItem Value="RemarkHd">Remark</asp:ListItem>                                        
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
         <asp:GridView ID="GridView1" runat="server" AllowPaging="True" AllowSorting="true"
            CssClass="Grid" AutoGenerateColumns="false"> 
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
                          <asp:Button class="btngo" runat="server" ID="BtnGo" Text="G" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" CommandName="Go" />                      </ItemTemplate>
                      <HeaderStyle Width="110px" />
                  </asp:TemplateField>
                  <asp:BoundField DataField="TransNmbr" HeaderStyle-Width="120px" SortExpression="Nmbr" HeaderText="Reference"></asp:BoundField>                  
                  <asp:BoundField DataField="Status" HeaderText="Status"></asp:BoundField>                  
                  <asp:BoundField DataField="TransDate" dataformatstring="{0:dd MMM yyyy}" htmlencode="true" HeaderStyle-Width="80px" SortExpression="TransDate" HeaderText="Date"></asp:BoundField>
                  <asp:BoundField DataField="EffectiveDate" dataformatstring="{0:dd MMM yyyy}" htmlencode="true" HeaderStyle-Width="80px" SortExpression="EffectiveDate" HeaderText="Effective Date"></asp:BoundField>                  
                  <asp:BoundField DataField="Currency" HeaderStyle-Width="200px" SortExpression="Currency" HeaderText="Currency"></asp:BoundField>
                  <asp:BoundField DataField="RemarkHD" HeaderStyle-Width="250px" HeaderText="Remark"></asp:BoundField>                  
              </Columns>
          </asp:GridView>	    
          </div>
            <asp:Panel runat="server" ID ="pnlNav" Visible="false">
            <asp:Button class="bitbtn btnadd" runat="server" ID="btnAdd2" Text="Add" />	            
            &nbsp &nbsp &nbsp
            <asp:DropDownList ID="ddlCommand2" CssClass="DropDownList" runat="server"/>
            <asp:Button class="btngo" runat="server" ID="btnGo2" Text="G" />  
            </asp:Panel>
    </asp:Panel>    
    <asp:Panel runat="server" ID="pnlInput" Visible="false">
      <table>
        <tr>
            <td>No</td>
            <td>:</td>
            <td><asp:TextBox CssClass="TextBox" Enabled ="false" runat="server" ID="tbTransNo" Width="149px"/>
            </td>            
            <td>Date</td>
            <td>:</td>
            <td>
            <BDP:BasicDatePicker ID="tbDate" runat="server" DateFormat="dd MMM yyyy" 
                        ShowNoneButton ="false" ReadOnly = "true" ValidationGroup="Input"
                        ButtonImageHeight="19px" ButtonImageWidth="20px" 
                        DisplayType="TextBoxAndImage" 
                        TextBoxStyle-CssClass="TextDate" AutoPostBack="True"><TextBoxStyle CssClass="TextDate" /></BDP:BasicDatePicker>                
            </td>            
        </tr> 
        <tr>
            <td><asp:LinkButton ID="lbCurrency" ValidationGroup="Input" runat="server" Text="Currency"/></td>
            <td>:</td>
            <td>
                <asp:DropDownList CssClass="DropDownList" ValidationGroup="Input" runat="server" AutoPostBack="True" 
                                        onselectedindexchanged="ddlCurrency_SelectedIndexChanged" ID="ddlCurrency" enabled = "False"  Width="130px" />                                
            </td>                            
            <td>Effective Date</td>
            <td>:</td>
            <td>
            <BDP:BasicDatePicker ID="tbEffectiveDate" runat="server" DateFormat="dd MMM yyyy" 
                        ShowNoneButton ="false" ReadOnly = "true" ValidationGroup="Input"
                        ButtonImageHeight="19px" ButtonImageWidth="20px" 
                        DisplayType="TextBoxAndImage" 
                        TextBoxStyle-CssClass="TextDate" AutoPostBack="True"><TextBoxStyle CssClass="TextDate" /></BDP:BasicDatePicker>                
            </td>            
        </tr>                                  
         
        <tr>
            <td>Remark</td>
            <td>:</td>
            <td colspan="4">
            <asp:TextBox runat="server" MaxLength = "255" ValidationGroup="Input" Width = "360" ID="tbRemark" CssClass="TextBoxMulti" TextMode="MultiLine" />
            </td>
        </tr>
      </table>        
      <br />    

      <div style="font-size:medium; color:Blue;">Detail</div>
      <hr style="color:Blue" />  
        <asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex="0">
           <asp:View ID="Tab1" runat="server">      
        <asp:Panel runat="server" ID="pnlDt">
            <asp:Button class="bitbtn btnadd" runat="server" ID="btnAddDt" Text="Add" ValidationGroup="Input" />	                  
            <div style="border:0px  solid; width:100%; height:100%; overflow:auto;">
                <asp:GridView ID="GridDt" runat="server" AutoGenerateColumns="false" 
                    ShowFooter="True">
                    <HeaderStyle CssClass="GridHeader" />
                    <RowStyle CssClass="GridItem" Wrap="false" />
                    <AlternatingRowStyle CssClass="GridAltItem" />
                    <PagerStyle CssClass="GridPager" />
                    <Columns>
                        <asp:TemplateField HeaderText="Action">
                            <ItemTemplate>
                                <asp:Button ID="btnEdit" runat="server" class="bitbtndt btnedit" Text="Edit" CommandName="Edit"/>
								<asp:Button ID="btnDelete" runat="server" class="bitbtndt btndelete" Text="Delete" OnClientClick="return confirm('Sure to delete this data?');" CommandName="Delete"/>									   
								<asp:Button ID="btnCopy" runat="server" class="bitbtndt btnedit" Text="Copy" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>"  CommandName="Copy"/>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:Button ID="btnUpdate" runat="server" class="bitbtndt btnsave" Text="Save" CommandName="Update"/>									
                                <asp:Button ID="btnCancel" runat="server" class="bitbtndt btncancel" Text="Cancel" CommandName="Cancel"/>									
                            </EditItemTemplate>                                                     
                        </asp:TemplateField>
                        <asp:TemplateField>
                               <ItemTemplate>                                                             
<%--                               <asp:Button Class="bitbtndt btngetitem"  ID="btnClosing" runat="server" Text="Closing"                                             
                                            CommandArgument='<%# Container.DataItemIndex %>'
                                            CommandName="Closing" />                                            
                                    <asp:Button Class="bitbtndt btngetitem" ID="btnDetailPO" runat="server" Text="Detail PO"                                             
                                            CommandArgument='<%# Container.DataItemIndex %>'
                                            CommandName="DetailPO" />--%>                                            
                                </ItemTemplate>
                        </asp:TemplateField>                       
                        <asp:BoundField DataField="Medical" HeaderStyle-Width="80px" HeaderText="Medical Code" />
                        <asp:BoundField DataField="Medical_Name" HeaderStyle-Width="250px"  HeaderText="Medical Name" />                                                
                        <asp:BoundField DataField="JobLevel" HeaderStyle-Width="250px"  HeaderText="Job Level" />                                                
                        <asp:BoundField DataField="FgCheckPlafond" HeaderStyle-Width="100px"  HeaderText="Check Plafond" /> 
                        <asp:BoundField DataField="AmountPlafond" HeaderStyle-Width="250px"  HeaderText="Amount Plafond" /> 
                        <asp:BoundField DataField="FgCheck1Plafond" HeaderStyle-Width="100px"  HeaderText="Check Plafond 1 Claim" /> 
                        <asp:BoundField DataField="Amount1Plafond" HeaderStyle-Width="250px"  HeaderText="Amount Plafond 1 Claim" /> 
                        <asp:BoundField DataField="Remark" HeaderStyle-Width="250px"  HeaderText="Remark" />                                                                        
                    </Columns>
                </asp:GridView>
          </div> 
          
          <asp:Button class="bitbtn btnadd" runat="server" ID="btnAddDt2" Text="Add" ValidationGroup="Input" />	  
           
       </asp:Panel>             
       <asp:Panel runat="server" ID="pnlEditDt" Visible="false">
            <table>
                <tr>
                    <td><asp:LinkButton ID="lbMedical"  runat="server" Text="Medical"/> </td>
                    <td>:</td>
                    <td><asp:TextBox runat="server" ID="tbMedicalCode" CssClass="TextBox" AutoPostBack="true" />
                        <asp:TextBox runat="server"  CssClass="TextBox"
                            ID="tbMedicalName" EnableTheming="True" ReadOnly="True" Enabled="False" 
                            Width="200px"/>
                        <asp:Button Class="btngo" ID="btnMedical" Text="..." runat="server" ValidationGroup="Input" />                                               
                        <asp:TextBox runat="server" ID="tbJobLevel" CssClass="TextBox" AutoPostBack="true" Visible = "false" />
                        <asp:TextBox runat="server" ID="tbPlafond" CssClass="TextBox" AutoPostBack="true" Visible = "false" />                        
                     </td>
                </tr>    
                
                <tr>
                   <td><asp:LinkButton ID="lbJobLevel" ValidationGroup="Input" runat="server" Text="Job Level" /></td>
                   <td>:</td>
                   <td><asp:DropDownList runat="server" CssClass="DropDownList" ID="ddlJobLevel" Width="220px">
                      </asp:DropDownList>
                   </td>
                </tr>   
                <tr>
                  <td>Check Plafond</td>
                  <td>:</td>
                  <td><asp:DropDownList runat="server" CssClass="DropDownList" ID="ddlCheckPlafond" 
                        Width="50px">
                        <asp:ListItem >Y</asp:ListItem>
                        <asp:ListItem >N</asp:ListItem>                                        
                      </asp:DropDownList>	
                      <asp:TextBox runat="server" CssClass="TextBox" ID="tbAmountPlafond" MaxLength="9" 
                        ValidationGroup="Input" />						    
                  </td>
                </tr>                  
                <tr>
                  <td>Check Plafond 1 Claim</td>
                  <td>:</td>
                  <td><asp:DropDownList runat="server" CssClass="DropDownList" ID="ddlCheck1Plafond" 
                        Width="50px">
                        <asp:ListItem >Y</asp:ListItem>
                        <asp:ListItem >N</asp:ListItem>                                        
                      </asp:DropDownList>	
                      <asp:TextBox runat="server" CssClass="TextBox" ID="tbAmount1Plafond" MaxLength="9" 
                        ValidationGroup="Input" />						    
                  </td>
                </tr>    
                
                <tr>
                  <td>Remark</td>
                  <td>:</td>
                  <td><asp:TextBox runat="server" MaxLength = "255" ValidationGroup="Input" Width = "350" ID="tbRemarkDt" CssClass="TextBoxMulti" TextMode="MultiLine" /></td>
                </tr>                           
                       
            </table>
            <br />                     
            <asp:Button ID="btnSaveDt" runat="server" class="bitbtndt btnsave" Text="Save" />									
            <asp:Button ID="btnCancelDt" runat="server" class="bitbtndt btncancel" Text="Cancel" />									
       </asp:Panel> 
       
       <br />     
       </asp:View>           
             
        </asp:MultiView>
     
       	<asp:Button ID="btnSaveAll" runat="server" class="bitbtndt btnsavenew" Text="Save & New" validationgroup="Input" Width = "90"/>									
        <asp:Button ID="btnSaveTrans" runat="server" class="bitbtndt btnsave" Text="Save" validationgroup="Input"/>									
        <asp:Button ID="btnBack" runat="server" class="bitbtndt btncancel" Text="Cancel" validationgroup="Input"/>									
        <asp:Button ID="btnHome" runat="server" class="bitbtndt btnback" Text="Home" />	
    </asp:Panel>
    <asp:Panel runat="server" ID="pnlPrint" Visible="false">
<%--    <cr:crystalreportviewer ID="CrystalReportViewer1" runat="server" AutoDataBind="True"
               Height="1036px" Width="928px" />
--%>    </asp:Panel>
      <br />            
    </div>
   
    <asp:Label runat ="server" ID="lbStatus" ForeColor="Red"/>
    <asp:Label runat ="server" ID="lbPrev" ForeColor="Red" Visible = "False"/>
    
    <asp:HiddenField ID="HiddenRemarkClose" runat="server" />
    </form>
    </body>
</html>
