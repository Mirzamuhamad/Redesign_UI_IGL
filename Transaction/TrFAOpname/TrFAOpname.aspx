<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TrFAOpname.aspx.vb" Inherits="Transaction_TrFAOpname_TrFAOpname" %>

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
                    
        function setformatdt()
        {
         try
         {    
            document.getElementById("tbQtyOpname").value = parseInt(document.getElementById("tbQtyActual").value.replace(/\$|\,/g,"")) - parseInt(document.getElementById("tbQtySystem").value.replace(/\$|\,/g,""));
         
            document.getElementById("tbQtySystem").value = setdigit(document.getElementById("tbQtySystem").value.replace(/\$|\,/g,""),'<%=ViewState("DigitQty")%>');
            document.getElementById("tbQtyActual").value = setdigit(document.getElementById("tbQtyActual").value.replace(/\$|\,/g,""),'<%=ViewState("DigitQty")%>');
            document.getElementById("tbQtyOpname").value = setdigit(document.getElementById("tbQtyOpname").value.replace(/\$|\,/g,""),'<%=ViewState("DigitQty")%>');
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
        <div class="H1">Opname</div>
        <hr style="color:Blue" />
        <asp:Panel runat="server" id="PnlHd"> 
      <table>
        <tr>
            <td style="width:100px;text-align:right">Quick Search :</td>            
            <td><asp:TextBox CssClass="TextBox" runat="server" ID ="tbFilter"/> 
                  <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlField" >
                      <asp:ListItem Value ="TransNmbr" Selected="True">Reference</asp:ListItem>
                      <asp:ListItem Value="dbo.FormatDate(TransDate)">Date</asp:ListItem>
                      <asp:ListItem>Status</asp:ListItem>
                      <asp:ListItem Value="dbo.FormatDate(OpnameDate)">Opname Date</asp:ListItem>
                      <asp:ListItem>Operator</asp:ListItem>
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
                    <asp:ListItem Value ="TransNmbr" Selected="True">Reference</asp:ListItem>
                      <asp:ListItem Value="dbo.FormatDate(TransDate)">Date</asp:ListItem>
                      <asp:ListItem>Status</asp:ListItem>
                      <asp:ListItem Value="dbo.FormatDate(OpnameDate)">Opname Date</asp:ListItem>                      
                      <asp:ListItem>Operator</asp:ListItem>
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
                             <%-- <asp:ListItem Text="Revisi" />--%>
                          </asp:DropDownList>
                          <asp:Button class="btngo" runat="server" ID="BtnGo" Text="G" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" CommandName="Go"/> 
                     </ItemTemplate>
                      <HeaderStyle Width="110px" />
                  </asp:TemplateField>
                  <asp:BoundField DataField="TransNmbr" SortExpression="Nmbr" HeaderText="Opname No"></asp:BoundField>                  
                  <asp:BoundField DataField="Status" HeaderText="Status" SortExpression="Status"></asp:BoundField>                  
                  <asp:BoundField DataField="TransDate" dataformatstring="{0:dd MMM yyyy}" htmlencode="true" HeaderStyle-Width="80px" SortExpression="TransDate" HeaderText="Date"></asp:BoundField>
                  <asp:BoundField DataField="OpnameDate" dataformatstring="{0:dd MMM yyyy}" htmlencode="true" HeaderStyle-Width="80px" SortExpression="OpnameDate" HeaderText="Opname Date"></asp:BoundField>                  
                  <asp:BoundField DataField="Operator" HeaderText="Operator" SortExpression="Operator"></asp:BoundField>
                  <asp:BoundField DataField="Remark" sortExpression="Remark" HeaderText="Remark"></asp:BoundField>
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
            <td>Opname No</td>
            <td>:</td>
            <td><asp:TextBox CssClass="TextBoxR" ValidationGroup="Input" runat="server" ReadOnly="true" ID="tbCode" Width="149px"/>
            </td>           
            
            <td>Date</td>
            <td>:</td>
            <td>
            <BDP:BasicDatePicker ID="tbDate" runat="server" DateFormat="dd MMM yyyy" 
                        ReadOnly = "true" ShowNoneButton="false"
                        ButtonImageHeight="19px" ButtonImageWidth="20px" 
                        DisplayType="TextBoxAndImage" 
                        TextBoxStyle-CssClass="TextDate" ValidationGroup="Input"><TextBoxStyle CssClass="TextDate" /></BDP:BasicDatePicker>                
            </td>            
        </tr>
        <tr>
            <td>Opname Date</td>
            <td>:</td>
            <td>
                <BDP:BasicDatePicker ID="tbOpname" runat="server" DateFormat="dd MMM yyyy" 
                        ReadOnly = "true" ShowNoneButton="false"
                        ButtonImageHeight="19px" ButtonImageWidth="20px" 
                        DisplayType="TextBoxAndImage" 
                        TextBoxStyle-CssClass="TextDate" ValidationGroup="Input"><TextBoxStyle CssClass="TextDate" /></BDP:BasicDatePicker>                
                        
                        <asp:Button class="btngo" runat="server" ID="btnGetDt" Text="Get Data" 
                    ValidationGroup="Input" Width="54px"/>
            </td>
            <td>Operator</td>
            <td>:</td>
            <td>
                <asp:TextBox ID="tbOperator" runat="server" ValidationGroup="Input"
                    CssClass="TextBox" MaxLength="60" />                    
            </td>
        </tr>        
        <tr>
            <td>Remark</td>
            <td>:</td>
            <td colspan="4"><asp:TextBox Width = "365px" runat="server" ValidationGroup="Input" 
                    ID="tbRemark" CssClass="TextBox" MaxLength="255" TextMode="MultiLine" /></td>
        </tr>
      </table>  
      
      <br />      
      <div style="font-size:medium; color:Blue;">Detail</div>
      <hr style="color:Blue" />  
        <asp:Panel runat="server" ID="pnlDt">
            <asp:Button class="bitbtndt btnadd" runat="server" ID="btnAddDt" Text="Add" ValidationGroup="Input"/>               
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
                                <asp:Button class="bitbtndt btnedit" runat="server" ID="btnEdit" Text="Edit" CommandName="Edit"/>
                                <asp:Button class="bitbtndt btndelete" runat="server" ID="btnDelete" Text="Delete" CommandName="Delete"/>
                            </ItemTemplate>
                            <EditItemTemplate>
                                 <asp:Button class="bitbtndt btnsave" runat="server" ID="btnUpdate" Text="Save" CommandName="Update"/>
                                 <asp:Button class="bitbtndt btncancel" runat="server" ID="btnCancel" Text="Cancel" CommandName="Cancel"/>
                            </EditItemTemplate>                                              
                        </asp:TemplateField>
                        <asp:BoundField DataField="FixedAsset" HeaderText="Fixed Asset" />
                        <asp:BoundField DataField="FAName" HeaderText="Fixed Asset Name" />
                        <asp:BoundField DataField="LocationType" HeaderText="Location Type" />
                        <asp:BoundField DataField="LocationCode" HeaderText="Location Code" />
                        <asp:BoundField DataField="LocationName" HeaderText="Location Name" />
                        <asp:BoundField DataField="QtySystem" HeaderStyle-Width="80px" HeaderText="Qty System" />
                        <asp:BoundField DataField="QtyActual" HeaderStyle-Width="80px" HeaderText="Qty Actual" />
                        <asp:BoundField DataField="QtyOpname" HeaderStyle-Width="80px" HeaderText="Qty Opname" />
                        <asp:BoundField DataField="Remark" HeaderStyle-Width="250px" HeaderText="Remark" />
                    </Columns>
                </asp:GridView>
          </div>   
          <asp:Button class="bitbtndt btnadd" runat="server" ID="btnAddDt2" Text="Add" ValidationGroup="Input"/>               
       </asp:Panel>             
       <asp:Panel runat="server" ID="pnlEditDt" Visible="false">
            <table>              
                <tr>
                    <td>Fixed Asset</td>
                    <td>:</td>
                    <td><asp:TextBox runat="server" ID="tbFA" CssClass="TextBox" AutoPostBack="true" />
                        <asp:TextBox runat="server"  CssClass="TextBoxR"
                            ID="tbFAName" EnableTheming="True" ReadOnly="True" Enabled="False" 
                            Width="200px"/> 
                        <asp:Button class="btngo" runat="server" ID="BtnFA" Text="..." />
                    </td>           
                </tr>                                    
                <tr>
                    <td>Location</td>
                    <td>:</td>
                    <td>
                        <asp:DropDownList  CssClass="DropDownList" ID="ddlLocationType" runat="server" 
                            AutoPostBack="True">
                            <asp:ListItem Selected="True">CUSTOMER</asp:ListItem>
                            <asp:ListItem>SUPPLIER</asp:ListItem>
                            <asp:ListItem>GENERAL</asp:ListItem>
                            <asp:ListItem>EMPLOYEE</asp:ListItem>
                        </asp:DropDownList>
                        <asp:TextBox runat="server" ID="tbLocCode" CssClass="TextBox" AutoPostBack="true" />
                        <asp:TextBox runat="server"  CssClass="TextBoxR"
                            ID="tbLocName" EnableTheming="True" ReadOnly="True" Enabled="False" 
                            Width="200px"/> 
                        <asp:Button class="btngo" runat="server" ID="btnLoc" Text="..." />    
                    </td>
                </tr>                
                <tr>
                    <td>Qty</td>
                    <td>:</td>
                    <td>
                        <table>
                            <tr style="background-color:Silver;text-align:center">
                                <td>System</td>
                                <td>Actual</td>
                                <td>Opname</td>
                            </tr>
                            <tr>
                                <td><asp:TextBox ID="tbQtySystem"  runat="server" CssClass="TextBoxR"/></td>
                                <td><asp:TextBox ID="tbQtyActual" runat="server" CssClass="TextBox" /></td>
                                <td><asp:TextBox ID="tbQtyOpname"  runat="server" CssClass="TextBoxR" /></td>
                            </tr>
                        </table>
                    </td>                
                </tr>
                <tr>
                    <td>Remark </td>
                    <td>:</td>
                    <td><asp:TextBox runat="server" ID="tbRemarkDt" CssClass="TextBox" Width="365px" 
                            MaxLength="255" TextMode="MultiLine" />                        
                    </td>
                </tr>
            </table>
            <br />       
            <asp:Button class="bitbtndt btnsave" runat="server" ID="btnSaveDt" Text="Save" />               
            <asp:Button class="bitbtndt btncancel" runat="server" ID="btnCancelDt" Text="Cancel" />               
       </asp:Panel> 
       <br />          
            <asp:Button class="bitbtn btnsavenew" runat="server" ID="btnSaveAll" 
            Text="Save & New" ValidationGroup="Input" Width="89px"/>               
            <asp:Button class="bitbtn btnsave" runat="server" ID="btnSaveTrans" Text="Save" ValidationGroup="Input"/>               
            <asp:Button class="bitbtn btnback" runat="server" ID="btnBack" Text="Cancel" ValidationGroup="Input"/>
            <asp:Button class="btngo" runat="server" ID="btnHome" Text="Home" 
            Height="21px" Width="38px" />
    </asp:Panel>
    </div>                 
    <asp:Label runat ="server" ID="lbStatus" ForeColor="Red"/>
    </form>
</body>
</html>
