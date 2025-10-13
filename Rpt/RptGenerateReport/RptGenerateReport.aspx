<%@ Page Language="VB" AutoEventWireup="false" CodeFile="RptGenerateReport.aspx.vb" Inherits="Rpt_RptGenerateReport_RptGenerateReport" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<%@ Register Assembly="BasicFrame.WebControls.BasicDatePicker" Namespace="BasicFrame.WebControls"
    TagPrefix="BDP" %>

<%@ Register assembly="CrystalDecisions.Web, Version=10.5.3700.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Untitled Page</title>
    <script src="../../Function/OpenDlg.js" type="text/javascript"></script>
    <script src="../../Function/Function.JS" type="text/javascript"></script>
    
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
 <link href="https://fonts.googleapis.com/css2?family=Muli:wght@300;400;600&display=swap" rel="stylesheet">
</head>
<body>     
    <form id="form1" runat="server">    
    <div class="Content">
    <div class="H1">
                        <asp:Label ID="lbJudul" runat="server" />
                    </div>
     <hr style="color:Blue; width: 1147px;" />        
    <asp:Panel runat="server" ID="pnlInput" Visible="True">
      <table>
        <tr>
            <td>Report</td>
            <td>:</td>
            <td><asp:TextBox CssClass="TextBox" ValidationGroup="Input" runat="server" 
                    ID="tbReport" AutoPostBack="true" Width="86px" /> 
                <asp:TextBox CssClass="TextBox" runat="server" ID="tbReportName" 
                    enabled="false" Width="225px"/>
                <asp:ImageButton ID="btnReport" runat="server" ValidationGroup="Input" 
                         ImageUrl="../../Image/btnDot2on.png"
                         onmouseover="this.src='../../Image/btnDot2off.png';"
                         onmouseout="this.src='../../Image/btnDot2on.png';"
                         ImageAlign="AbsBottom" />               
                <asp:TextBox ID="tbTable" runat="server" CssClass="TextBox" enabled="false" 
                    Visible="False" Width="108px" />
                <asp:DropDownList ID="ddlColumnType" runat="server" AutoPostBack="False" 
                            CssClass="DropDownList" Visible="false"/>
            </td>                    
        </tr>  
        <tr>
            <td>Template</td>
            <td>:</td>
            <td><asp:TextBox runat="server" ValidationGroup="Input" ID="tbTemp" AutoPostBack="true"
                    CssClass="TextBox" Width="86px"/> <asp:TextBox ID="tbTempName" 
                    runat="server" CssClass="TextBox" enabled="false" AutoPostBack="true" Width="225px" />
                <asp:ImageButton ID="btnTemp" runat="server" ImageAlign="AbsBottom" 
                    ImageUrl="../../Image/btnDot2on.png" 
                    onmouseout="this.src='../../Image/btnDot2on.png';" 
                    onmouseover="this.src='../../Image/btnDot2off.png';" ValidationGroup="Input" />
                &nbsp; &nbsp; 
                <asp:ImageButton ID="btnGetDt" runat="server" ValidationGroup="Input" 
                         ImageUrl="../../Image/btnGetDataon.png"
                         onmouseover="this.src='../../Image/btnGetDataoff.png';"
                         onmouseout="this.src='../../Image/btnGetDataon.png';"
                         ImageAlign="AbsBottom" />               
                <asp:ImageButton ID="btnSaveTrans" runat="server" ImageAlign="AbsBottom" 
                    ImageUrl="../../Image/btnSaveOn.png" 
                    onmouseout="this.src='../../Image/btnSaveOn.png';" 
                    onmouseover="this.src='../../Image/btnSaveOff.png';" ValidationGroup="Input" />
                <asp:ImageButton ID="btnUser" runat="server" ImageAlign="AbsBottom" 
                    ImageUrl="../../Image/btnUserDtOn.png" 
                    onmouseout="this.src='../../Image/btnUserDtOn.png';" 
                    onmouseover="this.src='../../Image/btnUserDtOff.png';" ValidationGroup="Input" />
            </td>
        </tr>                        
          <tr>
              <td colspan="3">
                  <asp:Panel ID="Panel1" runat="server" GroupingText="Criteria">
                      <table>
                          <tr>
                              <td>
                              </td>
                              <td>
                              </td>
                              <td>
                                  <asp:Label ID="lbAColumn1" runat="server" Text="Label" Visible="False"></asp:Label>
                                  &nbsp;<asp:Label ID="lbAColumn1T" runat="server" Text="Label" Visible="False"></asp:Label>
                              </td>
                              <td>
                              </td>
                              <td>
                              </td>
                              <td>
                                  &nbsp;</td>
                              <td>
                                  <asp:Label ID="lbAColumn2" runat="server" Text="Label" Visible="False"></asp:Label>
                                  &nbsp;<asp:Label ID="lbAColumn2T" runat="server" Text="Label" Visible="False"></asp:Label>
                              </td>
                              <td>
                                  &nbsp;</td>
                              <td>
                              </td>
                              <td>
                              </td>
                              <td>
                                  <asp:Label ID="lbAColumn3" runat="server" Text="Label" Visible="False"></asp:Label>
                                  <asp:Label ID="lbAColumn3T" runat="server" Text="Label" Visible="False"></asp:Label>
                              </td>
                          </tr>
                          <tr>
                              <td>
                                  <asp:DropDownList ID="ddlAColumn1" runat="server" AutoPostBack="true" 
                                      CssClass="DropDownList" Height="16px" Width="130px" Enabled="False" />
                              </td>
                              <td>
                                  <asp:DropDownList ID="ddlAOperator1" runat="server" AutoPostBack="false" 
                                      CssClass="DropDownList" Height="16px" ValidationGroup="Input" Width="55px" 
                                      Enabled="False">
                                      <asp:ListItem>=</asp:ListItem>
                                      <asp:ListItem>&gt;=</asp:ListItem>
                                      <asp:ListItem>&lt;=</asp:ListItem>
                                      <asp:ListItem>LIKE</asp:ListItem>
                                      <asp:ListItem>NOT LIKE</asp:ListItem>
                                  </asp:DropDownList>
                              </td>
                              <td>
                                  <asp:TextBox ID="tbAValue1" runat="server" CssClass="TextBox" AutoPostBack="true" 
                                      ValidationGroup="Input" Width="86px" Enabled="False" />
                              </td>
                              <td>
                                  <asp:DropDownList ID="ddlALogic2" runat="server" AutoPostBack="true" 
                                      CssClass="DropDownList" Height="16px" ValidationGroup="Input" Width="55px" 
                                      Enabled="False">
                                      <asp:ListItem Value="">-</asp:ListItem>
                                      <asp:ListItem>AND</asp:ListItem>
                                      <asp:ListItem>OR</asp:ListItem>
                                  </asp:DropDownList>
                              </td>
                              <td>
                                  <asp:DropDownList ID="ddlAColumn2" runat="server" AutoPostBack="true" 
                                      CssClass="DropDownList" Height="16px" Width="130px" Enabled="False" />
                              </td>
                              <td>
                                  <asp:DropDownList ID="ddlAOperator2" runat="server" AutoPostBack="false" 
                                      CssClass="DropDownList" Height="16px" ValidationGroup="Input" Width="55px" 
                                      Enabled="False">
                                      <asp:ListItem>=</asp:ListItem>
                                      <asp:ListItem>&gt;=</asp:ListItem>
                                      <asp:ListItem>&lt;=</asp:ListItem>
                                      <asp:ListItem>LIKE</asp:ListItem>
                                      <asp:ListItem>NOT LIKE</asp:ListItem>
                                  </asp:DropDownList>
                              </td>
                              <td>
                                  <asp:TextBox ID="tbAValue2" runat="server" CssClass="TextBox" AutoPostBack="true"
                                      ValidationGroup="Input" Width="86px" Enabled="False" />
                              </td>
                              <td>
                                  <asp:DropDownList ID="ddlALogic3" runat="server" AutoPostBack="true" 
                                      CssClass="DropDownList" Height="16px" ValidationGroup="Input" Width="55px" Enabled="False">
                                      <asp:ListItem Value="">-</asp:ListItem>
                                      <asp:ListItem>AND</asp:ListItem>
                                      <asp:ListItem>OR</asp:ListItem>
                                  </asp:DropDownList>
                              </td>
                              <td>
                                  <asp:DropDownList ID="ddlAColumn3" runat="server" AutoPostBack="true" 
                                      CssClass="DropDownList" Height="16px" Width="130px" Enabled="False" />
                              </td>
                              <td>
                                  <asp:DropDownList ID="ddlAOperator3" runat="server" AutoPostBack="false" 
                                      CssClass="DropDownList" Height="16px" ValidationGroup="Input" Width="55px" 
                                      Enabled="False">
                                      <asp:ListItem>=</asp:ListItem>
                                      <asp:ListItem>&gt;=</asp:ListItem>
                                      <asp:ListItem>&lt;=</asp:ListItem>
                                      <asp:ListItem>LIKE</asp:ListItem>
                                      <asp:ListItem>NOT LIKE</asp:ListItem>
                                  </asp:DropDownList>
                              </td>
                              <td>
                                  <asp:TextBox ID="tbAValue3" runat="server" CssClass="TextBox" AutoPostBack="true"
                                      ValidationGroup="Input" Width="86px" Enabled="False" />
                              </td>
                          </tr>
                          <tr>
                              <td>
                                  <asp:DropDownList ID="ddlBLogic1" runat="server" AutoPostBack="true" 
                                      CssClass="DropDownList" Height="16px" ValidationGroup="Input" Width="55px" 
                                      Enabled="False">
                                      <asp:ListItem Value="">-</asp:ListItem>
                                      <asp:ListItem>AND</asp:ListItem>
                                      <asp:ListItem>OR</asp:ListItem>
                                  </asp:DropDownList>
                              </td>
                              <td>
                              </td>
                              <td>
                                  <asp:Label ID="lbBColumn1" runat="server" Text="Label" Visible="False"></asp:Label>
                                  &nbsp;<asp:Label ID="lbBColumn1T" runat="server" Text="Label" Visible="False"></asp:Label>
                              </td>
                              <td>
                              </td>
                              <td>
                              </td>
                              <td>
                                  &nbsp;</td>
                              <td>
                                  <asp:Label ID="lbBColumn2" runat="server" Text="Label" Visible="False"></asp:Label>
                                  &nbsp;<asp:Label ID="lbBColumn2T" runat="server" Text="Label" Visible="False"></asp:Label>
                              </td>
                              <td>
                                  &nbsp;</td>
                              <td>
                              </td>
                              <td>
                              </td>
                              <td>
                                  <asp:Label ID="lbBColumn3" runat="server" Text="Label" Visible="False"></asp:Label>
                                  &nbsp;<asp:Label ID="lbBColumn3T" runat="server" Text="Label" Visible="False"></asp:Label>
                              </td>
                          </tr>
                          <tr>
                              <td>
                                  <asp:DropDownList ID="ddlBColumn1" runat="server" AutoPostBack="true" 
                                      CssClass="DropDownList" Height="16px" Width="130px" Enabled="False" />
                              </td>
                              <td>
                                  <asp:DropDownList ID="ddlBOperator1" runat="server" AutoPostBack="false" 
                                      CssClass="DropDownList" Height="16px" ValidationGroup="Input" Width="55px" 
                                      Enabled="False">
                                      <asp:ListItem>=</asp:ListItem>
                                      <asp:ListItem>&gt;=</asp:ListItem>
                                      <asp:ListItem>&gt;=</asp:ListItem>
                                      <asp:ListItem>LIKE</asp:ListItem>
                                      <asp:ListItem>NOT LIKE</asp:ListItem>
                                  </asp:DropDownList>
                              </td>
                              <td>
                                  <asp:TextBox ID="tbBValue1" runat="server" CssClass="TextBox" AutoPostBack="true"
                                      ValidationGroup="Input" Width="86px" Enabled="False" />
                              </td>
                              <td>
                                  <asp:DropDownList ID="ddlBLogic2" runat="server" AutoPostBack="true" 
                                      CssClass="DropDownList" Height="16px" ValidationGroup="Input" Width="55px" 
                                      Enabled="False">
                                      <asp:ListItem Value="">-</asp:ListItem>
                                      <asp:ListItem>AND</asp:ListItem>
                                      <asp:ListItem>OR</asp:ListItem>
                                  </asp:DropDownList>
                              </td>
                              <td>
                                  <asp:DropDownList ID="ddlBColumn2" runat="server" AutoPostBack="true" 
                                      CssClass="DropDownList" Height="16px" Width="130px" Enabled="False" />
                              </td>
                              <td>
                                  <asp:DropDownList ID="ddlBOperator2" runat="server" AutoPostBack="false" 
                                      CssClass="DropDownList" Height="16px" ValidationGroup="Input" Width="55px" 
                                      Enabled="False">
                                      <asp:ListItem>=</asp:ListItem>
                                      <asp:ListItem>&gt;=</asp:ListItem>
                                      <asp:ListItem>&lt;=</asp:ListItem>
                                      <asp:ListItem>LIKE</asp:ListItem>
                                      <asp:ListItem>NOT LIKE</asp:ListItem>
                                  </asp:DropDownList>
                              </td>
                              <td>
                                  <asp:TextBox ID="tbBValue2" runat="server" CssClass="TextBox" AutoPostBack="true"
                                      ValidationGroup="Input" Width="86px" Enabled="False" />
                              </td>
                              <td>
                                  <asp:DropDownList ID="ddlBLogic3" runat="server" AutoPostBack="true" 
                                      CssClass="DropDownList" Height="16px" ValidationGroup="Input" Width="55px" 
                                      Enabled="False">
                                      <asp:ListItem Value="">-</asp:ListItem>
                                      <asp:ListItem>AND</asp:ListItem>
                                      <asp:ListItem>OR</asp:ListItem>
                                  </asp:DropDownList>
                              </td>
                              <td>
                                  <asp:DropDownList ID="ddlBColumn3" runat="server" AutoPostBack="true" 
                                      CssClass="DropDownList" Height="16px" Width="130px" Enabled="False" />
                              </td>
                              <td>
                                  <asp:DropDownList ID="ddlBOperator3" runat="server" AutoPostBack="false" 
                                      CssClass="DropDownList" Height="16px" ValidationGroup="Input" Width="55px" 
                                      Enabled="False">
                                      <asp:ListItem>=</asp:ListItem>
                                      <asp:ListItem>&gt;=</asp:ListItem>
                                      <asp:ListItem>&lt;=</asp:ListItem>
                                      <asp:ListItem>LIKE</asp:ListItem>
                                      <asp:ListItem>NOT LIKE</asp:ListItem>
                                  </asp:DropDownList>
                              </td>
                              <td>
                                  <asp:TextBox ID="tbBValue3" runat="server" CssClass="TextBox" AutoPostBack="true"
                                      ValidationGroup="Input" Width="86px" Enabled="False" />
                              </td>
                          </tr>
                          <tr>
                              <td>
                                  <asp:DropDownList ID="ddlCLogic1" runat="server" AutoPostBack="true" 
                                      CssClass="DropDownList" Height="16px" ValidationGroup="Input" Width="55px" 
                                      Enabled="False">
                                      <asp:ListItem Value="">-</asp:ListItem>
                                      <asp:ListItem>AND</asp:ListItem>
                                      <asp:ListItem>OR</asp:ListItem>
                                  </asp:DropDownList>
                              </td>
                              <td>
                              </td>
                              <td>
                                  <asp:Label ID="lbCColumn1" runat="server" Text="Label" Visible="False"></asp:Label>
                                  &nbsp;<asp:Label ID="lbCColumn1T" runat="server" Text="Label" Visible="False"></asp:Label>
                              </td>
                              <td>
                              </td>
                              <td>
                              </td>
                              <td>
                                  &nbsp;</td>
                              <td>
                                  <asp:Label ID="lbCColumn2" runat="server" Text="Label" Visible="False"></asp:Label>
                                  &nbsp;<asp:Label ID="lbCColumn2T" runat="server" Text="Label" Visible="False"></asp:Label>
                              </td>
                              <td>
                                  &nbsp;</td>
                              <td>
                              </td>
                              <td>
                              </td>
                              <td>
                                  <asp:Label ID="lbCColumn3" runat="server" Text="Label" Visible="False"></asp:Label>
                                  <asp:Label ID="lbCColumn3T" runat="server" Text="Label" Visible="False"></asp:Label>
                              </td>
                          </tr>
                          <tr>
                              <td>
                                  <asp:DropDownList ID="ddlCColumn1" runat="server" AutoPostBack="true" 
                                      CssClass="DropDownList" Height="16px" Width="130px" Enabled="False" />
                              </td>
                              <td>
                                  <asp:DropDownList ID="ddlCOperator1" runat="server" AutoPostBack="false" 
                                      CssClass="DropDownList" Height="16px" ValidationGroup="Input" Width="55px" 
                                      Enabled="False">
                                      <asp:ListItem>=</asp:ListItem>
                                      <asp:ListItem>&gt;=</asp:ListItem>
                                      <asp:ListItem>&lt;=</asp:ListItem>
                                      <asp:ListItem>LIKE</asp:ListItem>
                                      <asp:ListItem>NOT LIKE</asp:ListItem>
                                  </asp:DropDownList>
                              </td>
                              <td>
                                  <asp:TextBox ID="tbCValue1" runat="server" CssClass="TextBox" AutoPostBack="true"
                                      ValidationGroup="Input" Width="86px" Enabled="False" />
                              </td>
                              <td>
                                  <asp:DropDownList ID="ddlCLogic2" runat="server" AutoPostBack="true" 
                                      CssClass="DropDownList" Height="16px" ValidationGroup="Input" Width="55px" 
                                      Enabled="False">
                                      <asp:ListItem Value="">-</asp:ListItem>
                                      <asp:ListItem>AND</asp:ListItem>
                                      <asp:ListItem>OR</asp:ListItem>
                                  </asp:DropDownList>
                              </td>
                              <td>
                                  <asp:DropDownList ID="ddlCColumn2" runat="server" AutoPostBack="true" 
                                      CssClass="DropDownList" Height="16px" Width="130px" Enabled="False" />
                              </td>
                              <td>
                                  <asp:DropDownList ID="ddlCOperator2" runat="server" AutoPostBack="false" 
                                      CssClass="DropDownList" Height="16px" ValidationGroup="Input" Width="55px" 
                                      Enabled="False">
                                      <asp:ListItem>=</asp:ListItem>
                                      <asp:ListItem>&gt;=</asp:ListItem>
                                      <asp:ListItem>&lt;=</asp:ListItem>
                                      <asp:ListItem>LIKE</asp:ListItem>
                                      <asp:ListItem>NOT LIKE</asp:ListItem>
                                  </asp:DropDownList>
                              </td>
                              <td>
                                  <asp:TextBox ID="tbCValue2" runat="server" CssClass="TextBox" AutoPostBack="true"
                                      ValidationGroup="Input" Width="86px" Enabled="False" />
                              </td>
                              <td>
                                  <asp:DropDownList ID="ddlCLogic3" runat="server" AutoPostBack="true" 
                                      CssClass="DropDownList" Height="16px" ValidationGroup="Input" Width="55px" 
                                      Enabled="False">
                                      <asp:ListItem Value="">-</asp:ListItem>
                                      <asp:ListItem>AND</asp:ListItem>
                                      <asp:ListItem>OR</asp:ListItem>
                                  </asp:DropDownList>
                              </td>
                              <td>
                                  <asp:DropDownList ID="ddlCColumn3" runat="server" AutoPostBack="true" 
                                      CssClass="DropDownList" Height="16px" Width="130px" Enabled="False" />
                              </td>
                              <td>
                                  <asp:DropDownList ID="ddlCOperator3" runat="server" AutoPostBack="false" 
                                      CssClass="DropDownList" Height="16px" ValidationGroup="Input" Width="55px" 
                                      Enabled="False">
                                      <asp:ListItem>=</asp:ListItem>
                                      <asp:ListItem>&gt;=</asp:ListItem>
                                      <asp:ListItem>&lt;=</asp:ListItem>
                                      <asp:ListItem>LIKE</asp:ListItem>
                                      <asp:ListItem>NOT LIKE</asp:ListItem>
                                  </asp:DropDownList>
                              </td>
                              <td>
                                  <asp:TextBox ID="tbCValue3" runat="server" CssClass="TextBox" AutoPostBack="true"
                                      ValidationGroup="Input" Width="86px" Enabled="False" />
                              </td>
                          </tr>
                      </table>
                  </asp:Panel>
              </td>
          </tr>
          <tr>
              <td colspan=3>
                  Type : 
                  <asp:DropDownList ID="ddlType" runat="server" AutoPostBack="true" 
                      CssClass="DropDownList" Height="16px" ValidationGroup="Input" Width="80px">
                      <asp:ListItem>Common</asp:ListItem>
                      <asp:ListItem>Crosstab</asp:ListItem>
                  </asp:DropDownList> &nbsp &nbsp
                  Column & Result : 
                  <asp:DropDownList ID="ddlTabColumn" runat="server" CssClass="DropDownList" Height="16px" Width="130px">                      
                  </asp:DropDownList>
                  <asp:DropDownList ID="ddlTabResult" runat="server" CssClass="DropDownList" Height="16px" Width="130px">                      
                  </asp:DropDownList>
              </td>
          </tr>
      </table>  
      
      <br />      
      <div style="font-size:medium; color:Blue;"></div>
        <asp:Panel runat="server" ID="pnlDt">
            <asp:ImageButton ID="btnAddDt" ValidationGroup="Input" runat="server"  
                    ImageUrl="../../Image/btnAddDtOn.png"
                    onmouseover="this.src='../../Image/btnAddDtOff.png';"
                    onmouseout="this.src='../../Image/btnAddDtOn.png';"
                    ImageAlign="AbsBottom" />                   
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
                                <asp:ImageButton ID="btnEdit" runat="server" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>"  
                                    ImageUrl="../../Image/btnEditDtOn.png"
                                    onmouseover="this.src='../../Image/btnEditDtOff.png';"
                                    onmouseout="this.src='../../Image/btnEditDtOn.png';"
                                    ImageAlign="AbsBottom" CommandName="Edit" />   
                                <asp:ImageButton ID="btnDelete" runat="server" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>"  
                                    ImageUrl="../../Image/btnDeleteDtOn.png"
                                    onmouseover="this.src='../../Image/btnDeleteDtOff.png';"
                                    onmouseout="this.src='../../Image/btnDeleteDtOn.png';"
                                    ImageAlign="AbsBottom" CommandName="Delete" />     
                                <asp:ImageButton ID="btnUp" runat="server" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>"  
                                    ImageUrl="../../Image/btnUpOn.png"
                                    onmouseover="this.src='../../Image/btnUpOff.png';"
                                    onmouseout="this.src='../../Image/btnUpOn.png';"
                                    ImageAlign="AbsBottom" CommandName="Up" />     
                                <asp:ImageButton ID="btnDown" runat="server" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>"  
                                    ImageUrl="../../Image/btnDownOn.png"
                                    onmouseover="this.src='../../Image/btnDownOff.png';"
                                    onmouseout="this.src='../../Image/btnDownOn.png';"
                                    ImageAlign="AbsBottom" CommandName="Down" />                                
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
                            <%--<FooterTemplate>
                            <asp:ImageButton ID="btnAddDt2" ValidationGroup="Input" runat="server"  
                                    ImageUrl="../../Image/btnAddDtOn.png"
                                    onmouseover="this.src='../../Image/btnAddDtOff.png';"
                                    onmouseout="this.src='../../Image/btnAddDtOn.png';"
                                    ImageAlign="AbsBottom" />        
                            </FooterTemplate>--%>                            
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="No" SortExpression="ItemNo" DataField="ItemNo" />
                        <asp:BoundField DataField="ColumnField" HeaderStyle-Width="200px" 
                            HeaderText="Column" >
                            <HeaderStyle Width="200px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="ColumnAlias" HeaderStyle-Width="250px"  
                            HeaderText="Alias" >
                            <HeaderStyle Width="250px" />
                        </asp:BoundField>
                        <asp:TemplateField Visible="false">
<%--                            <ItemTemplate>
                                <asp:Label ID="lbLocation" runat="server" text='<%# DataBinder.Eval(Container.DataItem, "ColumnField") %>' />
                            </ItemTemplate>
--%>                        </asp:TemplateField>   
                        <asp:BoundField DataField="FgDisplay" HeaderText="Display" />
                        <asp:BoundField DataField="SortOrder" HeaderText="Sort Order" />
                        <asp:BoundField DataField="SortType" HeaderStyle-Width="80px"  
                            HeaderText="Sort Type" >
                            <HeaderStyle Width="8px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Type" HeaderStyle-Width="100px" HeaderText="Type" >
                            <HeaderStyle Width="100px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="ColumnFunct" HeaderStyle-Width="80px" 
                            HeaderText="Function" >                                                
                            <HeaderStyle Width="80px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Compare" HeaderStyle-Width="100px" HeaderText="Compare Column" >
                            <HeaderStyle Width="100px" />
                        </asp:BoundField>                        
                    </Columns>
                </asp:GridView>
          </div>   
          <asp:ImageButton ID="btnAddDt2" ValidationGroup="Input" runat="server"  
                 ImageUrl="../../Image/btnAddDtOn.png"
                 onmouseover="this.src='../../Image/btnAddDtOff.png';"
                 onmouseout="this.src='../../Image/btnAddDtOn.png';"
                 ImageAlign="AbsBottom" />                   
       </asp:Panel>             
       <asp:Panel runat="server" ID="pnlEditDt" Visible="false">
            <table>                
                <tr>
                    <td>Item No</td>
                    <td>:</td>
                    <td>
                        <asp:Label ID="lbItemNo" runat="server" />
                    </td>                               
                </tr>
                <tr>
                    <td>
                        Columns</td>
                    <td>:</td>
                    <td>
                        <asp:DropDownList ID="ddlColumn" runat="server" AutoPostBack="True" 
                            CssClass="DropDownList" Height="16px" Width="165px" />
                    </td>
                </tr>                                                    
                <tr>
                    <td>Alias</td>
                    <td>&nbsp;</td>
                    <td>
                        <asp:TextBox ID="tbAlias" runat="server" CssClass="TextBox" 
                            EnableTheming="True" Width="305px" />
                    </td>
                </tr>
                <tr>
                    <td>Display</td>
                    <td>:</td>
                    <td>
                        <asp:DropDownList ID="ddlDisplay" runat="server" AutoPostBack="True" 
                            CssClass="DropDownList" Height="16px" Width="52px">
                            <asp:ListItem Selected="True">N</asp:ListItem>
                            <asp:ListItem>Y</asp:ListItem>
                        </asp:DropDownList>
                    </td>                    
                </tr>
                <tr>
                    <td>
                        Sort Order</td>
                    <td>
                        :</td>
                    <td>
                        <asp:DropDownList ID="ddlSortOrder" runat="server" AutoPostBack="True" 
                            CssClass="DropDownList" Height="16px" Width="52px">
                            <asp:ListItem>0</asp:ListItem>
                            <asp:ListItem>1</asp:ListItem>
                            <asp:ListItem>2</asp:ListItem>
                            <asp:ListItem>3</asp:ListItem>
                            <asp:ListItem>4</asp:ListItem>
                            <asp:ListItem>5</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>Sort Type</td>
                    <td>:</td>
                    <td>
                        <asp:DropDownList ID="ddlSortType" runat="server" 
                            CssClass="DropDownList" Height="16px" Width="75px">
                            <asp:ListItem>ASC</asp:ListItem>
                            <asp:ListItem>DESC</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>              
                <tr>
                    <td>Type</td>
                    <td>:</td>                    
                    <td><asp:TextBox runat="server" CssClass="TextBoxR" ID="tbType" Width="122px" 
                            Enabled="false"/></td>
                </tr>  
                <tr>
                    <td>
                        Function
                    </td>
                    <td>
                        :</td>
                    <td>
                        <asp:DropDownList ID="ddlFunction" runat="server" 
                            CssClass="DropDownList" Height="16px" Width="75px">
                            <asp:ListItem Value=""> - </asp:ListItem>
                            <asp:ListItem>AVG</asp:ListItem>
                            <asp:ListItem>MAX</asp:ListItem>
                            <asp:ListItem>MIN</asp:ListItem>
                            <asp:ListItem>SUM</asp:ListItem>                            
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        Compare With Column
                    </td>
                    <td>
                        :</td>
                    <td>
                        <asp:DropDownList ID="ddlCompare" runat="server" 
                            CssClass="DropDownList" Height="16px" Width="75px" AutoPostBack="True">
                            <asp:ListItem Value=""> Choose One </asp:ListItem>
                            <asp:ListItem>></asp:ListItem>
                            <asp:ListItem><</asp:ListItem>
                            <asp:ListItem>>=</asp:ListItem>                            
                            <asp:ListItem><=</asp:ListItem>                            
                            <asp:ListItem><></asp:ListItem>                            
                        </asp:DropDownList>
                        <asp:DropDownList ID="ddlColumnCompare" runat="server" 
                            CssClass="DropDownList" Height="16px" Width="130px">                            
                        </asp:DropDownList>
                    </td>
                </tr>
            </table>
            <br />                     
            <asp:ImageButton ID="btnSaveDt" runat="server"  
                 ImageUrl="../../Image/btnSaveOn.png"
                 onmouseover="this.src='../../Image/btnSaveOff.png';"
                 onmouseout="this.src='../../Image/btnSaveOn.png';"
                 ImageAlign="AbsBottom" />
            <asp:ImageButton ID="btnCancelDt" runat="server"  
                 ImageUrl="../../Image/btnCancelOn.png"
                 onmouseover="this.src='../../Image/btnCancelOff.png';"
                 onmouseout="this.src='../../Image/btnCancelOn.png';"
                 ImageAlign="AbsBottom" />
       </asp:Panel> 
       <br />    
             
    </asp:Panel>
        <asp:ImageButton ID="btnPreview" runat="server"  
                    ImageUrl="../../Image/btnPreviewOn.png"
                    onmouseover="this.src='../../Image/btnPreviewOff.png';"
                    onmouseout="this.src='../../Image/btnPreviewOn.png';"
                    ImageAlign="AbsBottom" />        
            <asp:Button ID="btnExport" runat="server" Text="Export" />
    <asp:Panel runat="server" ID="pnlPrint" Visible="false" Height="50%">
        <div style="border-style: solid; border-color: inherit; border-width: 0px; width:100%; height:237px; overflow:auto;">
           <asp:GridView ID="GridResult" runat="server" AutoGenerateColumns="True" 
                AllowPaging="True" AllowSorting="true" ShowFooter="False" GridLines="Both" > <%--Height="79px" PageSize="20" Width="724px"--%>
               <HeaderStyle CssClass="GridHeader" />
               <RowStyle CssClass="GridItem" Wrap="false" />
               <AlternatingRowStyle CssClass="GridAltItem" />
               <PagerStyle CssClass="GridPager" />
           </asp:GridView>
           <asp:GridView ID="GridExport" runat="server" AutoGenerateColumns="True" 
                Visible="true" ShowFooter="False" GridLines="None" Height="79px" PageSize="20" Width="724px">
               <HeaderStyle CssClass="GridHeader" />
               <RowStyle CssClass="GridItem" Wrap="false" />
               <AlternatingRowStyle CssClass="GridAltItem" />
               <PagerStyle CssClass="GridPager" />
           </asp:GridView>
       </div>   
       
    </asp:Panel>
      <br />            
    </div>  
    <asp:Label runat ="server" ID="lbStatus" ForeColor="Red"/>
   
   
    </form>
    </body>
</html>
