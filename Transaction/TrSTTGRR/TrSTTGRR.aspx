<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TrSTTGRR.aspx.vb" Inherits="Transaction_TrSTTGRR_TrSTTGRR" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="BasicFrame.WebControls.BasicDatePicker" Namespace="BasicFrame.WebControls"
    TagPrefix="BDP" %>
<%@ Register assembly="CrystalDecisions.Web, Version=10.5.3700.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Untitled Page</title>
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
        var Qty = document.getElementById("tbQtyOut").value.replace(/\$|\,/g,""); 
        Qtyin = document.getElementById("tbQtyIn").value.replace(/\$|\,/g,""); 
        QtyLoss = document.getElementById("tbQtyLoss").value.replace(/\$|\,/g,""); 
        document.getElementById("tbQtyOut").value = setdigit(Qty,'<%=ViewState("DigitQty")%>');
        document.getElementById("tbQtyIn").value = setdigit(Qtyin,'<%=ViewState("DigitQty")%>');
        document.getElementById("tbQtyLoss").value = setdigit(QtyLoss,'<%=ViewState("DigitQty")%>');
        }catch (err){
            alert(err.description);
          }      
        }   
    </script>
    
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
 <link href="https://fonts.googleapis.com/css2?family=Muli:wght@300;400;600&display=swap" rel="stylesheet">
</head>
<body>     
    <form id="form1" runat="server">    
    <div class="Content">
    <div class="H1">Transfer Good RR</div>
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
                      <asp:ListItem Value="WrhsAreaSrc_Name">Wrhs Area Issue</asp:ListItem>
                      <asp:ListItem Value="Wrhs_Name">Wrhs Issue</asp:ListItem>
                      <asp:ListItem Value="Subled">Issue Subled No</asp:ListItem>
                      <asp:ListItem Value="Subled_Name">Issue Subled Name</asp:ListItem>
                      <asp:ListItem Value="WrhsAreaDest_Name">Wrhs Area Destination</asp:ListItem>
                      <asp:ListItem Value="ReceivedBy">Received By</asp:ListItem>
                      <asp:ListItem>Remark</asp:ListItem>
                  </asp:DropDownList>
                  <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlRange"></asp:DropDownList>
                 
                  <asp:Button class="bitbtn btnsearch" runat="server" ID="btnSearch" Text="Search" />																						 											  
                  <asp:Button class="bitbtn btngo" runat="server" ID="btnExpand" Text="..." />																						 											                    
            </td>
            <td>
                <asp:LinkButton ID="LbAdvSearch" runat="server" Text="Advanced Search" />
                &nbsp &nbsp &nbsp &nbsp
                <asp:Label runat="server" ID="Label1" Text="SPB Gantung : "/>
                <asp:LinkButton runat="server" ID="lbCount" Text="X" ForeColor="#FF6600" Font-Size="Small" />
                <asp:Label runat="server" ID="Label2" Text=" record(s)"/>
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
              <asp:DropDownList ID="ddlField2" runat="server" CssClass="DropDownList">
                  <asp:ListItem Selected="True" Value="TransNmbr">Reference</asp:ListItem>
                  <asp:ListItem Value="dbo.FormatDate(TransDate)">Date</asp:ListItem>
                  <asp:ListItem>Status</asp:ListItem>
                  <asp:ListItem Value="WrhsAreaSrc_Name">Wrhs Area Issue</asp:ListItem>
                  <asp:ListItem Value="Wrhs_Name">Wrhs Issue</asp:ListItem>
                  <asp:ListItem Value="Subled">Issue Subled No</asp:ListItem>
                  <asp:ListItem Value="Subled_Name">Issue Subled Name</asp:ListItem>
                  <asp:ListItem Value="WrhsAreaDest_Name">Wrhs Area Destination</asp:ListItem>
                  <asp:ListItem Value="ReceivedBy">Receiveed By</asp:ListItem>
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
            CssClass="Grid" AutoGenerateColumns="False" > 
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
                          <asp:Button class="btngo" runat="server" ID="BtnGo" Text="G" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" CommandName="Go" />                    
                          
                      </ItemTemplate>
                      <HeaderStyle Width="110px" />
                  </asp:TemplateField>
                  <asp:BoundField DataField="TransNmbr" HeaderStyle-Width="120px" 
                      SortExpression="Nmbr" HeaderText="Reference">
                      <HeaderStyle Width="120px" />
                  </asp:BoundField>                  
                  <asp:BoundField DataField="Status" HeaderText="Status"></asp:BoundField>                  
                  <asp:BoundField DataField="TransDate" HtmlEncode="true" DataFormatString="{0:dd MMM yyyy}" HeaderStyle-Width="80px" SortExpression="TransDate" HeaderText="Date">
                      <HeaderStyle Width="80px" />
                  </asp:BoundField>
                  <asp:BoundField DataField="WrhsAreaSrc_Name" HeaderText="Wrhs Area Issue" 
                      SortExpression="WrhsAreaSrc_Name" />
                  <asp:BoundField DataField="WrhsAreaDest_Name" HeaderStyle-Width="200px" 
                      HeaderText="Wrhs Area Destionation" SortExpression="WrhsAreaDest_Name">
                      <HeaderStyle Width="200px" />
                  </asp:BoundField>
                  <asp:BoundField DataField="Wrhs_Name" HeaderText="Warehouse Issue" 
                      SortExpression="Wrhs_Name" />
                  <asp:BoundField DataField="Subled" HeaderText="Issue Subled" 
                      SortExpression="Subled" />
                  <asp:BoundField DataField="Subled_Name" HeaderStyle-Width="102px" 
                      HeaderText="Issue Subled Name" SortExpression="Subled_Name">
                      <HeaderStyle Width="200px" />
                  </asp:BoundField>
                  <asp:BoundField DataField="ReceivedBy" HeaderStyle-Width="200px" 
                      HeaderText="Received By" SortExpression="ReceivedBy">
                      <HeaderStyle Width="200px" />
                  </asp:BoundField>
                  <asp:BoundField DataField="Remark" HeaderStyle-Width="200px" 
                      HeaderText="Remark">
                      <HeaderStyle Width="250px" />
                  </asp:BoundField>
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
            <td>Reference</td>
            <td>:</td>
            <td><asp:TextBox CssClass="TextBoxR" runat="server" ID="tbReference" Width="150px" Enabled="False"/> </td>            
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
            </td>            
        </tr> 
        <tr>
            <td>
                SJ Transfer</td>
            <td>:</td>
            <td>
                <asp:TextBox ID="tbTGSJ" runat="server" CssClass="TextBoxR" Enabled="False" 
                    Width="150px" />                    
                <asp:Button Class="btngo" ID="btnTGSJ" Text="..." runat="server"  ValidationGroup="Input" />                                          
                
                <asp:Label ID="Label11" runat="server" ForeColor="Red">*</asp:Label>
                
            </td>            
        </tr> 
          <tr>
              <td>
                  <asp:LinkButton ID="lbWrhsArea" runat="server" Text="Wrhs Area Source" 
                      ValidationGroup="Input" />
              </td>
              <td>
                  :</td>
              <td>
                  <asp:DropDownList ID="ddlWrhsArea" runat="server" 
                      CssClass="DropDownList" Width="200px" 
                      Enabled="False" />
                  <asp:Label ID="Label12" runat="server" ForeColor="Red">*</asp:Label>
              </td>
          </tr>
        <tr>
            <td>Wrhs Area Destination</td>
            <td>:</td>
            <td>
                <asp:DropDownList ID="ddlWrhsAreaDest" runat="server" AutoPostBack="True" 
                    CssClass="DropDownList" Enabled="False" Width="200px" />
                <asp:Label ID="Label13" runat="server" ForeColor="Red">*</asp:Label>
            </td>                    
        </tr>  
          <tr>
              <td>
                  <asp:LinkButton ID="lbWarehouse" runat="server" Text="Warehouse Receive" 
                      ValidationGroup="Input" />
              </td>
              <td>
                  :</td>
              <td>
                  <asp:DropDownList ID="ddlWrhs" runat="server" CssClass="DropDownList" 
                      Width="200px" />
                  <asp:Label ID="Label14" runat="server" ForeColor="Red">*</asp:Label>
                  <asp:TextBox ID="tbFgSubLed" runat="server" Visible="false" />
              </td>
          </tr>
        <tr>
            <td>Receive<span lang="IN" style="font-size:12.0pt;font-family:&quot;Times New Roman&quot;; mso-fareast-font-family:&quot;Times New Roman&quot;;mso-ansi-language:IN;mso-fareast-language:
            EN-US;mso-bidi-language:AR-SA"> </span>SubLed</td>
            <td>:</td>
            <td><asp:TextBox CssClass="TextBox" ValidationGroup="Input" runat="server" ID="tbSubLed" AutoPostBack="true" /> 
                <asp:TextBox CssClass="TextBoxR" runat="server" ID="tbSubLedName" enabled="false" Width="225px"/>
                <asp:Button Class="btngo" ID="btnSubLed" Text="..." runat="server"  ValidationGroup="Input" />                                          
                
            </td>                    
        </tr>  
          <tr>
              <td>
                  Receive By</td>
              <td>
                  :</td>
              <td>
                  <asp:TextBox ID="tbReceiveedBy" runat="server" CssClass="TextBox" 
                      ValidationGroup="Input" Width="200px" />
                  &nbsp;&nbsp;&nbsp;&nbsp;
                  <asp:Button Class="bitbtndt btngetitem" ID="btnGetDt" Text="Get Data" runat="server"  ValidationGroup="Input" width = "70px"/>                                          
                  
                  
              </td>
          </tr>
        <tr>
            <td>Remark</td>
            <td>:</td>
            <td><asp:TextBox runat="server" ValidationGroup="Input" ID="tbRemark" CssClass="TextBox" Width="225px"/></td>
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
                               	<asp:Button ID="btnEdit" runat="server" class="bitbtndt btnedit" Text="Edit" CommandName="Edit"/>
								<asp:Button ID="btnDelete" runat="server" class="bitbtndt btndelete" Text="Delete" OnClientClick="return confirm('Sure to delete this data?');" CommandName="Delete"/>									
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:Button ID="btnUpdate" runat="server" class="bitbtndt btnsave" Text="Save" CommandName="Update"/>									
                                <asp:Button ID="btnCancel" runat="server" class="bitbtndt btncancel" Text="Cancel" CommandName="Cancel"/>									
                            </EditItemTemplate>
                            <%--<FooterTemplate>
                            <asp:ImageButton ID="btnAddDt2" ValidationGroup="Input" runat="server"  
                                    ImageUrl="../../Image/btnAddDtOn.png"
                                    onmouseover="this.src='../../Image/btnAddDtOff.png';"
                                    onmouseout="this.src='../../Image/btnAddDtOn.png';"
                                    ImageAlign="AbsBottom" />        
                            </FooterTemplate>--%>                            
                        </asp:TemplateField>
                        <asp:BoundField DataField="Product" HeaderStyle-Width="120px" 
                            HeaderText="Product Code" >
                            <HeaderStyle Width="120px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Product_Name" HeaderStyle-Width="250px"  
                            HeaderText="Product Name" >
                            <HeaderStyle Width="250px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Specification" HeaderText="Specification" HeaderStyle-Width="150px"  
                            SortExpression="Specification" >
                            <HeaderStyle Width="150px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="QtySJ" HeaderStyle-Width="80px" 
                            HeaderText="Qty Out" >
                            <HeaderStyle Width="80px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="QtyRR" HeaderText="Qty In" />
                        <asp:BoundField DataField="QtyLoss" HeaderText="Qty Loss" />
                        <asp:BoundField DataField="Unit" HeaderStyle-Width="80px" HeaderText="Unit" >
                            <HeaderStyle Width="80px" />                            
                        </asp:BoundField>                        
                    </Columns>
                </asp:GridView>
          </div>   
          <asp:Button class="bitbtn btnadd" runat="server" ID="btnAddDt2" Text="Add" ValidationGroup="Input" />	
       </asp:Panel>             
       <asp:Panel runat="server" ID="pnlEditDt" Visible="false">
            <table>                
                <tr>
                    <td><asp:LinkButton ID="lbProduct"  runat="server" Text="Product"/> </td>
                    <td>:</td>
                    <td><asp:TextBox runat="server" ID="tbProductCode" CssClass="TextBox" AutoPostBack="true" />
                        <asp:TextBox runat="server"  CssClass="TextBoxR"                        
                            ID="tbProductName" EnableTheming="True" Enabled="false" Width="200px"/> 
                        <asp:Button Class="btngo" ID="btnProduct" Text= "..." runat="server" />                                                                                    
                        
                        <asp:Label ID="Label7" runat="server" ForeColor="Red">*</asp:Label>
                        
                    </td>                               
                </tr>
                <tr>
                    <td>Specification</td>
                    <td>:</td>
                    <td>
                        <asp:TextBox ID="tbSpecification" runat="server" CssClass="TextBoxR" 
                            Enabled="false" EnableTheming="True" Width="301px" />
                    </td>
                </tr>                        
                <tr>
                    <td>
                        Qty</td>
                    <td>
                        :</td>
                    <td>
                        <table cellpadding="0" cellspacing="0">
                            <tr style="background-color:Silver;text-align:center">
                                <td>
                                    Out
                                    <asp:Label ID="Label5" runat="server" ForeColor="Red">*</asp:Label>
                                </td>
                                
                                <td>
                                    In</td>
                                <td>
                                    Loss</td>
                                <td>
                                    Unit
                                    <asp:Label ID="Label6" runat="server" ForeColor="Red">*</asp:Label>
                                </td>                                
                            </tr>
                            <tr>
                                <td>
                                    <asp:TextBox ID="tbQtyOut" runat="server" AutoPostBack="true" 
                                        CssClass="TextBox" Width="80px" />
                                </td>
                                
                                
                                <td>
                                    <asp:TextBox ID="tbQtyIn" runat="server" AutoPostBack="True" CssClass="TextBox" 
                                        Width="80px" />
                                </td>
                                <td>
                                    <asp:TextBox ID="tbQtyLoss" runat="server" AutoPostBack="True" 
                                        CssClass="TextBoxR" Width="80px" />
                                </td>
                                <td>
                                    <asp:TextBox ID="tbUnit" runat="server" CssClass="TextBox" Enabled="false" 
                                        Width="75px" />
                                </td>                                
                            </tr>
                        </table>
                    </td>
                </tr>                
                <tr>
                    <td>
                        Remark
                    </td>
                    <td>
                        :</td>
                    <td>
                        <asp:TextBox ID="tbRemarkDt" runat="server" CssClass="TextBox" Width="472px" 
                            Height="35px" TextMode="MultiLine" />
                    </td>
                    
                </tr>
            </table>
            <br />                     
            <asp:Button ID="btnSaveDt" runat="server" class="bitbtndt btnsave" Text="Save" />									
            <asp:Button ID="btnCancelDt" runat="server" class="bitbtndt btncancel" Text="Cancel" />									
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
    </asp:Panel>
      <br />            
    </div>
   
    <asp:Label runat ="server" ID="lbStatus" ForeColor="Red"/>
   
    </form>
    </body>
</html>
