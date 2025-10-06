<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TrGLFAContruction.aspx.vb" Inherits="Transaction_TrGLFAContruction_TrGLFAContruction" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="BasicFrame.WebControls.BasicDatePicker" Namespace="BasicFrame.WebControls"
    TagPrefix="BDP" %>

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
        var TotalForex = document.getElementById("tbTotalForex").value.replace(/\$|\,/g,""); 
        TotalExpense = document.getElementById("tbTotalExpense").value.replace(/\$|\,/g,""); 
        document.getElementById("tbTotalForex").value = setdigit(Qty,'<%=ViewState("DigitQty")%>');
        document.getElementById("tbTotalExpense").value = setdigit(Qty,'<%=ViewState("DigitQty")%>');
        
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
    <div class="H1">&nbsp;<asp:Label ID="Labelmenu" runat="server" 
            Text="Construction in Progress Accomplishment"></asp:Label>
            </div>
     <hr style="color:Blue" />        
    <asp:Panel runat="server" id="PnlHd"> 
      <table>
        <tr>
            <td style="width:100px;text-align:right">Quick Search :</td>            
            <td><asp:TextBox CssClass="TextBox" runat="server" ID ="tbFilter"/> 
                  <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlField" >
                      <asp:ListItem Value="TransNmbr" Selected="True">TransNmbr</asp:ListItem>
                      <asp:ListItem Value="Trans_Date">Date</asp:ListItem>
                      <asp:ListItem>Status</asp:ListItem>
                      <asp:ListItem>ContructionIP</asp:ListItem>
                      <asp:ListItem Value="FALocationType">FA Type Location</asp:ListItem>
                      <asp:ListItem Value="FALocationName">FA Location</asp:ListItem>
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
              <asp:DropDownList ID="ddlField2" runat="server" CssClass="DropDownList">
                  <asp:ListItem Selected="True" Value="Reference">TransNmbr</asp:ListItem>
                  <asp:ListItem Value="Trans_Date">Date</asp:ListItem>
                  <asp:ListItem>Status</asp:ListItem>
                  <asp:ListItem>ContructionIP</asp:ListItem>
                  <asp:ListItem Value="FALocationType">FA Type Location</asp:ListItem>
                  <asp:ListItem Value="FALocationName">FA Location</asp:ListItem>
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
            <asp:Button class="btngo" runat="server" ID="BtnGo" Text="G" Visible="false"/>
          <br />
          <div style="border:0px  solid; width:100%; height:100%; overflow:auto;">
          <asp:GridView ID="GridView1" runat="server" AllowPaging="True" AllowSorting="True"
            CssClass="Grid" AutoGenerateColumns="False"> 
              <HeaderStyle CssClass="GridHeader"></HeaderStyle>
						<RowStyle CssClass="GridItem" Wrap="false" />
						<AlternatingRowStyle CssClass="GridAltItem"/>
						<PagerStyle CssClass="GridPager" />
              <Columns>              
                  <%--<asp:BoundField DataField="QCNo" HeaderText="QC No" SortExpression="QCNo" />--%>
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
                          <asp:Button class="btngo" runat="server" ID="BtnGo" Text="G" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" CommandName="Go"/>  
                      </ItemTemplate>
                      <HeaderStyle Width="110px" />
                  </asp:TemplateField>
                  <asp:BoundField DataField="TransNmbr" HeaderStyle-Width="140px" 
                      HeaderText="TransNmbr" SortExpression="Nmbr">
                      <HeaderStyle Width="140px" />
                  </asp:BoundField>
                  <asp:BoundField DataField="Status" HeaderText="Status" />
                  <asp:BoundField DataField="TransDate" dataformatstring="{0:dd MMM yyyy}" htmlencode="true" HeaderStyle-Width="80px" SortExpression="TransDate"
                      HeaderText="Date">
                      <HeaderStyle Width="80px" />
                  </asp:BoundField>
                  <asp:BoundField DataField="ContructionIP" HeaderStyle-Width="102px" 
                      HeaderText="Contruction IP" SortExpression="ContructionIP">
                      <HeaderStyle Width="200px" />
                  </asp:BoundField>
                  <asp:BoundField DataField="FALocationType" 
                      HeaderText="F A Type Location" SortExpression="FALocationType">
                  </asp:BoundField>
                  <asp:BoundField DataField="FALocationName" HeaderText="FA Location" 
                      SortExpression="FALocationName" />
                  <asp:BoundField DataField="Remark" HeaderStyle-Width="200px" 
                      HeaderText="Remark">
                      <HeaderStyle Width="250px" />
                  </asp:BoundField>
              </Columns>
          </asp:GridView>
          </div>
            <asp:Panel runat="server" ID ="pnlNav" Visible="false">
            <asp:Button class="bitbtn btnadd" runat="server" ID="btnAdd2" Text="Add"/>
            &nbsp &nbsp &nbsp
            <asp:DropDownList ID="ddlCommand2" CssClass="DropDownList" runat="server"/>
            <asp:Button class="btngo" runat="server" ID="btnGo2" Text="G"/>
            </asp:Panel>
    </asp:Panel>    
    <asp:Panel runat="server" ID="pnlInput" Visible="false">
      <table>
        <tr>
            <td>No</td>
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
                &nbsp;</td>            
        </tr> 
         
          <tr>
              <td>
                  Contruction IP No</td>
              <td>
                  <asp:Label ID="Label4" runat="server" Text=":"></asp:Label>
              </td>
              <td>
                  <asp:TextBox ID="tbReff" runat="server" CssClass="TextBoxR" Enabled="False" 
                      Width="150px" />
                  <asp:Button class="btngo" runat="server" ID="btnReff" Text="..." ValidationGroup="Input"/>    
                  <asp:Label ID="lbred0" runat="server" ForeColor="Red">*</asp:Label>
              </td>
          </tr>
         
        <tr>
            <td>Fixed Asset</td>
            <td>:</td>
            <td>
                <asp:TextBox CssClass="TextBox" ValidationGroup="Input" runat="server" 
                    ID="tbFA" /> 
                <asp:TextBox CssClass="TextBox" runat="server" ID="tbFAName" Width="312px" 
                    ValidationGroup="Input"/>
                <asp:Label ID="lbred1" runat="server" ForeColor="Red">*</asp:Label>
            </td>                    
        </tr> 
        <tr>
            <td>
                F A Sub Group</td>
            <td>:</td>
            <td>
                <asp:DropDownList ID="ddlFASubGroup" runat="server" CssClass="DropDownList" 
                    ValidationGroup="Input" Width="200px" />
                <asp:Label ID="lbred2" runat="server" ForeColor="Red">*</asp:Label>
            </td>            
            
        </tr> 
          <tr>
              <td>
                  FA&nbsp; Status</td>
              <td>
                  :</td>
              <td>
                  <asp:DropDownList ID="ddlFAStatus" runat="server" CssClass="DropDownList" 
                      ValidationGroup="Input" Width="200px" />
                  <asp:Label ID="lbred4" runat="server" ForeColor="Red">*</asp:Label>
              </td>
              
          </tr>
          <tr>
              <td>
                  FA Location </td>
              <td>
                  :</td>
              <td>
                  <asp:DropDownList ID="ddlType" runat="server" AutoPostBack="true" 
                      CssClass="DropDownList" Height="16px" ValidationGroup="Input" Width="97px">
                      <asp:ListItem>GENERAL</asp:ListItem>
                      <asp:ListItem>CUSTOMER</asp:ListItem>
                      <asp:ListItem>SUPPLIER</asp:ListItem>
                      <asp:ListItem>EMPLOYEE</asp:ListItem>
                  </asp:DropDownList>
                  <asp:TextBox ID="tbLocation" runat="server" AutoPostBack="true" 
                      CssClass="TextBox" OnTextChanged="tbLocation_TextChanged" ValidationGroup="Input" />
                  <asp:TextBox ID="tbLocationName" runat="server" CssClass="TextBoxR" 
                      Enabled="false" EnableTheming="True" Width="219px" />
                  <asp:Button ID="btnLocation" runat="server" class="btngo" Text="..." 
                      ValidationGroup="Input" />
                  <asp:Label ID="lbred3" runat="server" ForeColor="Red">*</asp:Label>
                  &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                  </td>
          </tr>
          <tr>
              <td>
                  Cost Center</td>
              <td>
                  :</td>
              <td>
                  <asp:DropDownList ID="ddlCostCtr" runat="server" CssClass="DropDownList" 
                      ValidationGroup="Input" Width="200px" />
                  <asp:Label ID="lbred5" runat="server" ForeColor="Red">*</asp:Label>
                  <asp:Label ID="lbCurr" runat="server" ForeColor="Red" />
              </td>
          </tr>
          <tr>
              <td>
                  Total</td>
              <td>
                  :</td>
              <td>
                  <table>
                      <tr style="background-color: Silver; text-align: center">
                          <td>
                              Life (Month)</td>
                          <td>
                              Asset</td>
                          <td>
                              Expense</td>
                      </tr>
                      <tr>
                          <td>
                              <asp:TextBox ID="tbLife" runat="server" CssClass="TextBox" 
                                  ValidationGroup="Input" />
                          </td>
                          <td>
                              <asp:TextBox ID="tbTotalForex" runat="server" CssClass="TextBoxR" 
                                  Enabled="False" />
                          </td>
                          <td>
                              <asp:TextBox ID="tbTotalExpense" runat="server" CssClass="TextBoxR" 
                                  Enabled="False" />
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
                  <asp:TextBox ID="tbRemark" runat="server" CssClass="TextBoxMulti" 
                      MaxLength="255" TextMode="MultiLine" ValidationGroup="Input" Width="350px" />                  
              </td>
          </tr>
      </table>  
      
      <br />      
      <div style="font-size:medium; color:Blue;">Detail</div>
      <hr style="color:Blue" />  
        <asp:Panel runat="server" ID="pnlDt">
            <asp:Button class="bitbtndt btnadd" runat="server" ID="btnAddDt" Text="Add" ValidationGroup="Input"/>
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
                                <asp:Button class="bitbtndt btnedit" runat="server" ID="btnEdit" Text="Edit" CommandName="Edit"/>
                                <%--<asp:Button class="bitbtndt btndelete" runat="server" ID="btnDelete" Text="Delete" CommandName="Delete"/>--%>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:Button class="bitbtndt btnsave" runat="server" ID="btnUpdate" Text="Save" CommandName="Update"/>
                                <asp:Button class="bitbtndt btncancel" runat="server" ID="btnCancel" Text="Cancel" CommandName="Cancel"/>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="TransType" HeaderStyle-Width="120px" 
                            HeaderText="Trans Type" >
                            <HeaderStyle Width="120px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Reference" HeaderStyle-Width="250px"  
                            HeaderText="Reference" >
                            <HeaderStyle Width="250px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Date" dataformatstring="{0:dd MMM yyyy}" htmlencode="true" HeaderStyle-Width="80px"  
                            HeaderText="Date" >
                            <HeaderStyle Width="80px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Account" HeaderStyle-Width="150px" 
                            HeaderText="Account" >
                            <HeaderStyle Width="150px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Description" HeaderStyle-Width="150px" 
                            HeaderText="Account Name" >
                            <HeaderStyle Width="150px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="CostCtr" HeaderStyle-Width="120px" 
                            HeaderText="Cost Ctr" >                            
                        </asp:BoundField>
                        <asp:BoundField DataField="Currency" HeaderStyle-Width="150px" 
                            HeaderText="Currency" >
                            <HeaderStyle Width="80px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="ForexRate" DataFormatString="{0:#,##0.##}" HeaderStyle-Width="80px" HeaderText="Rate" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right"/>
                        <asp:BoundField DataField="AmountForex" DataFormatString="{0:#,##0.##}" HeaderStyle-Width="80px" HeaderText="Amount Forex" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right"/>
                        <asp:BoundField DataField="AmountHome" DataFormatString="{0:#,##0.##}" HeaderStyle-Width="80px" HeaderText="Amount Home" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right"/>
                        <asp:BoundField DataField="FgAddValue" HeaderText="Add Value" HeaderStyle-Width="60px" ItemStyle-HorizontalAlign="Center" >
                            <HeaderStyle Width="60px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Remark" HeaderStyle-Width="150px" 
                            HeaderText="Remark" >
                            <HeaderStyle Width="150px" />
                        </asp:BoundField>
                    </Columns>
                </asp:GridView>
          </div> 
               <asp:Button class="bitbtndt btnadd" runat="server" ID="btnAddDt2" Text="Add" ValidationGroup="Input"/>              
       </asp:Panel>             
       <asp:Panel runat="server" ID="pnlEditDt" Visible="false">
            <table>                
                <tr>
                    <td>Trans Type</td>
                    <td>:</td>
                    <td colspan="4">
                        <asp:TextBox ID="tbTransType" runat="server" CssClass="TextBox" Enabled="false" Width="45px" />
                    </td>                               
                </tr>
                <tr>
                    <td>
                        Reference</td>
                    <td>
                        :</td>
                    <td colspan="4">
                        <asp:TextBox ID="tbPONo" runat="server" CssClass="TextBox" Enabled="false" Width="167px" />
                    </td>
                </tr>
                                              
                <tr>
                    <td>
                        Account</td>
                    <td>
                        :</td>
                    <td colspan="4">                     
                        <asp:TextBox ID="tbAccount" runat="server" CssClass="TextBox" Enabled="false"/>
                        <asp:TextBox ID="tbAccountName" runat="server" CssClass="TextBox" Enabled="false" Width="303px" />
                    </td>
                </tr>
                <tr>
                    <td>
                        Cost Ctr</td>
                    <td>
                        :</td>
                    <td colspan="4">                     
                        <asp:TextBox ID="tbCostCtrDt" runat="server" Width="120px" CssClass="TextBox" Enabled="false"/>                        
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label2" runat="server" Text="Date"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="Label3" runat="server" Text=":"></asp:Label>
                    </td>
                    <td colspan="4">
                        <BDP:BasicDatePicker ID="tbRRDate" runat="server" ButtonImageHeight="19px" ButtonImageWidth="20px" DateFormat="dd MMM yyyy" 
                            DisplayType="TextBoxAndImage" ReadOnly="true" ShowNoneButton="False" 
                            TextBoxStyle-CssClass="TextDate" Enabled="false"/>
                        </BDP:BasicDatePicker>
                    </td>                    
                </tr>
                <tr>
                    <td>
                        Currcency</td>
                    <td>
                        :</td>
                    <td colspan="4">
                        <asp:TextBox ID="tbCurr" runat="server" CssClass="TextBox" Width="47px" Enabled="false" />
                        <asp:TextBox ID="tbRate" runat="server" CssClass="TextBox" Enabled="false" Width="80px" />
                    </td>
                </tr>
                <tr>
                    <td>
                        Amount</td>
                    <td>
                        :</td>
                    <td>
                        <table>
                            <tr style="background-color: Silver; text-align: center">
                                <td>
                                    Forex</td>
                                <td>
                                    Home</td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:TextBox ID="tbForex" runat="server" CssClass="TextBox" Enabled="False" />
                                </td>
                                <td>
                                    <asp:TextBox ID="tbHome" runat="server" CssClass="TextBox" Enabled="False" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        Add Value</td>
                    <td>
                        :</td>
                    <td>
                        <asp:DropDownList ID="ddlAddValue" runat="server" 
                            CssClass="DropDownList" Height="16px" ValidationGroup="Input" Width="44px">
                            <asp:ListItem>N</asp:ListItem>
                            <asp:ListItem>Y</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td>
                        &nbsp;</td>
                    <td>
                        &nbsp;</td>
                    <td>
                        &nbsp;</td>
                </tr>
                <tr>
                    <td>
                        Remark
                    </td>
                    <td>
                        :</td>
                    <td colspan="4">
                        <asp:TextBox ID="tbRemarkDt" runat="server" CssClass="TextBoxMulti" 
                            MaxLength="255" TextMode="MultiLine" ValidationGroup="Input" Width="350px" />
                    </td>
                </tr>
            </table>
            <br />  
            <asp:Button class="bitbtndt btnsave" runat="server" ID="btnSaveDt" Text="Save"/>                    
            <asp:Button class="bitbtndt btncancel" runat="server" ID="btnCancelDt" Text="Cancel"/>                    
       </asp:Panel> 
       <br />     
        <asp:Button class="bitbtn btnsavenew" runat="server" ID="btnSaveAll" 
            Text="Save & New" ValidationGroup="Input" Width="91px"/>     
        <asp:Button class="bitbtn btnsave" runat="server" ID="btnSaveTrans" Text="Save" ValidationGroup="Input"/>     
        <asp:Button class="bitbtn btnback" runat="server" ID="btnBack" Text="Cancel" ValidationGroup="Input"/>     
        <asp:Button class="btngo" runat="server" ID="btnHome" Text="Home" Width="44px"/>     
      </asp:Panel>
      <br />            
    </div>
   
    <asp:Label runat ="server" ID="lbStatus" ForeColor="Red"/>
   
    </form>
    </body>
</html>
