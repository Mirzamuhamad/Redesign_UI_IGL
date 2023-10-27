<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TrGLFAMove.aspx.vb" Inherits="Transaction_TrGLFAMove_TrGLFAMove" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<%@ Register Assembly="BasicFrame.WebControls.BasicDatePicker" Namespace="BasicFrame.WebControls"
    TagPrefix="BDP" %>
<%@ Register assembly="CrystalDecisions.Web, Version=10.5.3700.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>


<%@ Register assembly="FastReport" namespace="FastReport.Web" tagprefix="cc1" %>


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
        var Qty = document.getElementById("tbQty").value.replace(/\$|\,/g,""); 
        document.getElementById("tbQty").value = setdigit(Qty,'<%=ViewState("DigitQty")%>');
        
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
    <div class="H1">&nbsp;<asp:Label ID="Labelmenu" runat="server" Text="Fixed Asset Moving"></asp:Label>
            </div>
     <hr style="color:Blue" />        
    <asp:Panel runat="server" id="PnlHd"> 
      <table>
        <tr>
            <td style="width:100px;text-align:right">Quick Search :</td>            
            <td><asp:TextBox CssClass="TextBox" runat="server" ID ="tbFilter"/> 
                  <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlField" >
                      <asp:ListItem Value="TransNmbr" Selected="True">Reference</asp:ListItem>
                      <asp:ListItem Value="Trans_Date">Date</asp:ListItem>
                      <asp:ListItem>Status</asp:ListItem>
                      <asp:ListItem>Reference</asp:ListItem>
                      <asp:ListItem Value="FA_Location_Type_Src">FA Type Location Src</asp:ListItem>
                      <asp:ListItem Value="FA_Location_Name_Src">FA Location Src</asp:ListItem>
                      <asp:ListItem Value="FA_Location_Type_Dest">FA Type Location Dest</asp:ListItem>
                      <asp:ListItem Value="Operator">Operator</asp:ListItem>
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
                  <asp:ListItem Selected="True" Value="TransNmbr">Reference</asp:ListItem>
                  <asp:ListItem Value="Trans_Date">Date</asp:ListItem>
                  <asp:ListItem>Status</asp:ListItem>
                  <asp:ListItem>Reference</asp:ListItem>
                  <asp:ListItem Value="FA_Location_Type_Src">FA Type Location Src</asp:ListItem>
                  <asp:ListItem Value="FA_Location_Name_Src">FA Location Src</asp:ListItem>
                  <asp:ListItem Value="FA_Location_Type_Dest">FA Type Location Dest</asp:ListItem>
                  <asp:ListItem Value="Operator">Operator</asp:ListItem>
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
                      HeaderText="Reference" SortExpression="Nmbr">
                      <HeaderStyle Width="140px" />
                  </asp:BoundField>
                  <asp:BoundField DataField="Status" HeaderText="Status" />
                  <asp:BoundField DataField="TransDate" dataformatstring="{0:dd MMM yyyy}" htmlencode="true" HeaderStyle-Width="80px" SortExpression="TransDate"
                      HeaderText="Date">
                      <HeaderStyle Width="80px" />
                  </asp:BoundField>
                  <asp:BoundField DataField="Reference" HeaderStyle-Width="102px" 
                      HeaderText="Reference" SortExpression="Reference">
                      <HeaderStyle Width="200px" />
                  </asp:BoundField>
                  <asp:BoundField DataField="FA_Location_Type_Src" 
                      HeaderText="F A Type Location Src" SortExpression="FA_Location_Type_Src">
                  </asp:BoundField>
                  <asp:BoundField DataField="FA_Location_Name_Src" HeaderText="FA Location Src" 
                      SortExpression="FA_Location_Name_Src" />
                  <asp:BoundField DataField="FA_Location_Type_Dest" HeaderStyle-Width="200px" 
                      HeaderText="FA Type Location Dest" SortExpression="FA_Location_Type_Dest" >
                      <HeaderStyle Width="200px" />
                  </asp:BoundField>
                  <asp:BoundField DataField="FA_Location_Name_Dest" HeaderStyle-Width="200px" 
                      HeaderText="FA Location Dest" SortExpression="FA_Location_Name_Dest">
                      <HeaderStyle Width="200px" />
                  </asp:BoundField>
                  <asp:BoundField DataField="Operator" HeaderText="Operator" 
                      SortExpression="Operator" />
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
                &nbsp;<asp:TextBox ID="tbMoveType" runat="server" Visible="false" />
            </td>            
        </tr> 
         
          <tr>
              <td>
                  <asp:Label ID="Label1" runat="server" Text="Label">Reference</asp:Label>
              </td>
              <td>
                  <asp:Label ID="Label4" runat="server" Text=":"></asp:Label>
              </td>
              <td>
                  <asp:TextBox ID="tbReff" runat="server" CssClass="TextBoxR" Enabled="False" 
                      Width="150px" />
                  <asp:Button class="btngo" runat="server" ID="btnReff" Text="..." ValidationGroup="Input"/>    
              </td>
          </tr>
         
        <tr>
            <td>Source</td>
            <td>:</td>
            <td>
                <asp:DropDownList ID="ddlTypeSrc" runat="server" AutoPostBack="true" 
                    CssClass="DropDownList" Height="16px" ValidationGroup="Input" Width="97px">
                    <asp:ListItem>GENERAL</asp:ListItem>
                    <asp:ListItem>CUSTOMER</asp:ListItem>
                    <asp:ListItem>SUPPLIER</asp:ListItem>
                    <asp:ListItem>EMPLOYEE</asp:ListItem>
                </asp:DropDownList>
                <asp:TextBox CssClass="TextBox" ValidationGroup="Input" runat="server" 
                    ID="tbSource" AutoPostBack="true" OnTextChanged = "tbSource_TextChanged"/> 
                <asp:TextBox CssClass="TextBoxR" runat="server" ID="tbSourceName" 
                    enabled="false" Width="225px"/>
                <asp:Button class="btngo" runat="server" ID="btnSource" Text="..." ValidationGroup="Input"/>    
            </td>                    
        </tr> 
        <tr>
            <td>
                Destination</td>
            <td>&nbsp;</td>
            <td>
                <asp:DropDownList ID="ddlTypeDest" runat="server" AutoPostBack="true" 
                    CssClass="DropDownList" Height="16px" ValidationGroup="Input" Width="97px">
                    <asp:ListItem>GENERAL</asp:ListItem>
                    <asp:ListItem>CUSTOMER</asp:ListItem>
                    <asp:ListItem>SUPPLIER</asp:ListItem>
                    <asp:ListItem>EMPLOYEE</asp:ListItem>
                </asp:DropDownList>
                <asp:TextBox ID="tbDest" runat="server" AutoPostBack="true" OnTextChanged = "tbDest_TextChanged" CssClass="TextBox" ValidationGroup="Input"/>
                <asp:TextBox ID="tbDestName" runat="server" CssClass="TextBoxR" Enabled="false" 
                    EnableTheming="True" Width="225px" />
                <asp:Button class="btngo" runat="server" ID="btnDest" Text="..." ValidationGroup="Input"/>    
            </td>            
        </tr> 
          <tr>
              <td>
                  Operator</td>
              <td>
                  :</td>
              <td>
                  <asp:TextBox ID="tbOperator" runat="server" CssClass="TextBox" 
                      ValidationGroup="Input" Width="200px" />
                  &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                  <asp:Button class="btngo" runat="server" ID="btnGetDt" Text="Get Data" 
                      ValidationGroup="Input" Width="56px"/>
              </td>
          </tr>
          <tr>
              <td>
                  Remark</td>
              <td>
                  :</td>
              <td>
                  <asp:TextBox ID="tbRemark" runat="server" CssClass="TextBox" 
                      ValidationGroup="Input" Width="225px" />
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
                                <asp:Button class="bitbtndt btndelete" runat="server" ID="btnDelete" Text="Delete" CommandName="Delete"/>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:Button class="bitbtndt btnsave" runat="server" ID="btnUpdate" Text="Save" CommandName="Update"/>
                                <asp:Button class="bitbtndt btncancel" runat="server" ID="btnCancel" Text="Cancel" CommandName="Cancel"/>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="FixedAsset" HeaderStyle-Width="120px" 
                            HeaderText="Fixed Asset" >
                            <HeaderStyle Width="120px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="FixedAssetName" HeaderStyle-Width="250px"  
                            HeaderText="Fixed Asset Name" >
                            <HeaderStyle Width="250px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Qty" HeaderText="Qty" HeaderStyle-Width="60px" >
                            <HeaderStyle Width="60px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="DueDate" dataformatstring="{0:dd MMM yyyy}" htmlencode="true" HeaderStyle-Width="80px"  
                            HeaderText="Due Date" >
                            <HeaderStyle Width="80px" />
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
                    <td><asp:LinkButton ID="lbFA"  runat="server" Text="Fixed Asset"/> </td>
                    <td>:</td>
                    <td><asp:TextBox runat="server" ID="tbFACode" CssClass="TextBox" 
                            AutoPostBack="true" OnTextChanged = "tbFACode_TextChanged"/>
                        <asp:TextBox runat="server"  CssClass="TextBoxR"
                            ID="tbFAName" EnableTheming="True" Enabled="false" Width="200px"/> 
                        <asp:Button class="btngo" runat="server" ID="btnFA" Text="..."/>              
                    </td>                               
                </tr>
                <tr>
                    <td>Qty </td>
                    <td>:</td>
                    <td><asp:TextBox ID="tbQty" runat="server" CssClass="TextBox" /></td>                    
                </tr>
                                              
                <tr>
                    <td>
                        <asp:Label ID="Label2" runat="server" Text="Due Date"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="Label3" runat="server" Text=":"></asp:Label>
                    </td>
                    <td>                     
                        <BDP:BasicDatePicker ID="tbDueDate" runat="server" AutoPostBack="True" 
                            ButtonImageHeight="19px" ButtonImageWidth="20px" DateFormat="dd MMM yyyy" 
                            DisplayType="TextBoxAndImage" ReadOnly="true" ShowNoneButton="False" 
                            TextBoxStyle-CssClass="TextDate" ValidationGroup="Input"><TextBoxStyle CssClass="TextDate" /></BDP:BasicDatePicker>
                    </td>
                </tr>
                <tr>
                    <td>Remark </td>
                    <td>:</td>
                    <td><asp:TextBox ID="tbRemarkDt" runat="server" CssClass="TextBox" Width="287px" /></td>                    
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
    <asp:Panel runat="server" ID="pnlPrint" Visible="false">
    <cr:crystalreportviewer ID="CrystalReportViewer1" runat="server" AutoDataBind="True"
               Height="1036px" Width="928px" />
        <br />
        <cc1:WebReport ID="WebReport1" runat="server" AutoHeight="True" 
            AutoWidth="True" Height="100%" Width="100%" />
    </asp:Panel>
      <br />            
    </div>
   
    <asp:Label runat ="server" ID="lbStatus" ForeColor="Red"/>
   
    </form>
    </body>
</html>
