<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TrSTOpnameLotNo.aspx.vb" Inherits="Transaction_TrSTOpnameLotNo_TrSTOpnameLotNo" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="BasicFrame.WebControls.BasicDatePicker" Namespace="BasicFrame.WebControls"
    TagPrefix="BDP" %>
<%@ Register assembly="CrystalDecisions.Web, Version=10.5.3700.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Opname Lot No</title>
    <script type="text/javascript">
    function OpenPopup() {         
        window.open("../../SearchDlg.Aspx","List","scrollbars=yes,resizable=no,width=500,height=400");        
        return false;
    }    
    </script>
    <script src="../../Function/OpenDlg.js" type="text/javascript"></script>
    <script src="../../Function/Function.JS" type="text/javascript"></script>
    <%--<asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>--%>
    
    <script type="text/javascript" src="../../JS/jquerymodal/jquery.min.js"></script>
    <script src="../../JS/jquerymodal/jquery-ui.js" type="text/javascript"></script>
    <link href="../../JS/jquerymodal/jquery-ui.css" rel="stylesheet" type="text/css" />
    
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
    
    </script>
    
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
 <link href="https://fonts.googleapis.com/css2?family=Muli:wght@300;400;600&display=swap" rel="stylesheet">
    </head>
<body>     
    <form id="form1" runat="server">    
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    
    <div class="Content">
    <div class="H1">Opname Lot No</div>
     <hr style="color:Blue" />        
        <asp:Panel runat="server" ID="PnlHd">
            <table>
                <tr>
                    <td style="width: 100px; text-align: right">
                        Quick Search :
                    </td>
                    <td>
                        <asp:TextBox CssClass="TextBox" runat="server" ID="tbFilter" />
                        <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlField">
                            <asp:ListItem Value="TransNmbr" Selected="True">Reference</asp:ListItem>
                            <asp:ListItem Value="dbo.FormatDate(TransDate)">Date</asp:ListItem>
                            <asp:ListItem Value="Status">Status</asp:ListItem>
                            <asp:ListItem Value="dbo.FormatDate(OpnameDate)">Opname Date</asp:ListItem>
                            <asp:ListItem Value="OpnameType">Opname Type</asp:ListItem>
                            <asp:ListItem Value="Remark">Remark</asp:ListItem>
                        </asp:DropDownList>
                        <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlRange">
                        </asp:DropDownList>
                        <asp:Button class="bitbtn btnsearch" runat="server" ID="btnSearch" Text="Search" />
                        <asp:Button class="btngo" runat="server" ID="btnExpand" Text="..." />
                    </td>
                    <td>
                        <asp:LinkButton ID="LbAdvSearch" runat="server" Text="Advanced Search" />
                    </td>
                </tr>
            </table>
            <asp:Panel runat="server" ID="pnlSearch" Visible="false">
                <table>
                    <tr>
                        <td style="width: 100px; text-align: right">
                            <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlNotasi">
                                <asp:ListItem Selected="true" Text="OR" Value="OR"></asp:ListItem>
                                <asp:ListItem Text="AND" Value="AND"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:TextBox runat="server" CssClass="TextBox" ID="tbfilter2" />
                            <asp:DropDownList runat="server" CssClass="DropDownList" ID="ddlField2">
                                <asp:ListItem Value="TransNmbr" Selected="True">Reference</asp:ListItem>
                                <asp:ListItem Value="dbo.FormatDate(TransDate)">Date</asp:ListItem>
                                <asp:ListItem Value="Status">Status</asp:ListItem>
                                <asp:ListItem Value="dbo.FormatDate(OpnameDate)">Opname Date</asp:ListItem>
                                <asp:ListItem Value="Remark">Remark</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <br />
            <asp:Button class="bitbtn btnadd" runat="server" ID="BtnAdd" Text="Add" />
            &nbsp &nbsp &nbsp
            <asp:DropDownList CssClass="DropDownList" ID="ddlCommand" runat="server" Visible="false" />
            <asp:Button class="btngo" runat="server" ID="BtnGo" Text="G" Visible="false" />
            <br />
            <div style="border: 0px  solid; width: 100%; height: 100%; overflow:auto;">
                <asp:GridView ID="GridView1" runat="server" AllowPaging="True" AllowSorting="True"
                    CssClass="Grid" AutoGenerateColumns="False">
                    <HeaderStyle CssClass="GridHeader" Wrap="false"></HeaderStyle>
                    <RowStyle CssClass="GridItem" Wrap="false" />
                    <AlternatingRowStyle CssClass="GridAltItem" />
                    <PagerStyle CssClass="GridPager" />
                    <Columns>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:CheckBox ID="cbSelectHd" runat="server" AutoPostBack="true" OnCheckedChanged="cbSelectHd_CheckedChanged" />
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
                                </asp:DropDownList>
                                <asp:Button class="btngo" runat="server" ID="BtnGo" Text="G" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>"
                                    CommandName="Go" />
                            </ItemTemplate>
                            <HeaderStyle Width="110px" />
                        </asp:TemplateField>
                        <asp:BoundField DataField="TransNmbr" HeaderStyle-Width="120px" SortExpression="Nmbr" HeaderText="Reference">
                            <HeaderStyle Width="120px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Status" HeaderText="Status" SortExpression="Status"></asp:BoundField>
                        <asp:BoundField DataField="TransDate" DataFormatString="{0:dd MMM yyyy}" HtmlEncode="true" HeaderStyle-Width="80px" HeaderText="Date" SortExpression="TransDate">
                            <HeaderStyle Width="80px" />
                        </asp:BoundField>
                        
                        <asp:BoundField DataField="OpnameDate" HeaderText="Opname Date" DataFormatString="{0:dd MMM yyyy}" HtmlEncode="true" HeaderStyle-Width="80px" SortExpression="OpnameDate" />
                        <asp:BoundField DataField="Remark" HeaderText="Remark" SortExpression="Remark" />
                    </Columns>
                </asp:GridView>
            </div>
            <asp:Panel runat="server" ID="pnlNav" Visible="false">
                <asp:Button class="bitbtn btnadd" runat="server" ID="btnAdd2" Text="Add" />
                &nbsp &nbsp &nbsp
                <asp:DropDownList ID="ddlCommand2" CssClass="DropDownList" runat="server" />
                <asp:Button class="btngo" runat="server" ID="btnGo2" Text="G" />
            </asp:Panel>
        </asp:Panel>
        
    <asp:Panel runat="server" ID="pnlInput" Visible="false">
    <asp:Panel runat="server" ID="PnlWOgetdata" Visible="true">
      <table>
        <tr>
            <td>Reference</td>
            <td>:</td>
            <td><asp:TextBox CssClass="TextBox" Enabled = "false" runat="server" ID="tbRef" Width="149px"/> 
            </td>            
            
            <td>Date </td>
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
            <td>Opname Date</td>
            <td>:</td>
            <td>
                <BDP:BasicDatePicker ID="tbOpnameDate" runat="server" AutoPostBack="True" 
                    ButtonImageHeight="19px" ButtonImageWidth="20px" DateFormat="dd MMM yyyy" 
                    DisplayType="TextBoxAndImage" ReadOnly="true" ShowNoneButton="False" 
                    TextBoxStyle-CssClass="TextDate" ValidationGroup="Input"><TextBoxStyle CssClass="TextDate" />
                    </BDP:BasicDatePicker>
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
                  Remark</td>
              <td>
                  :</td>
              <td>
                  <asp:TextBox ID="tbRemark" runat="server" CssClass="TextBox" Height="50px" 
                      MaxLength="255" TextMode="MultiLine" ValidationGroup="Input" Width="352px" />
                  &nbsp;
                  <asp:Button ID="btnGetDt" runat="server" class="btngo" Text="Get Data" 
                      ValidationGroup="Input" Width="56px" />
              </td>
              <td>
              </td>
              <td>
              </td>
              <td>
              </td>
          </tr>
      </table>  
   
     
     <br />      
      <div style="font-size:medium; color:Blue;">Detail</div>
      <hr style="color:Blue" />  
     
   </asp:Panel>           
        <asp:Menu ID="Menu1" runat="server" CssClass="Menu" ItemWrap="False" 
            Orientation="Horizontal" StaticEnableDefaultPopOutImage="False" 
            StaticMenuItemStyle-CssClass="MenuItem" 
            StaticSelectedStyle-CssClass="MenuSelect" Visible="False">
            <StaticSelectedStyle CssClass="MenuSelect" />
            <StaticMenuItemStyle CssClass="MenuItem" />
            <Items>
                <asp:MenuItem Text="Detail " Value="0"></asp:MenuItem>
                
            </Items>
        </asp:Menu>
       <asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex="0">
           <asp:View ID="Tab1" runat="server"> 
        <br />
        <asp:Panel ID="pnlDt" runat="server">
            
            <asp:Button ID="btnAddDt" runat="server" class="bitbtndt btnadd" Text="Add" 
                ValidationGroup="Input" />
            <div style="border:0px  solid; width:100%; height:100%; overflow:auto;">
                <asp:GridView ID="GridDt" runat="server" AllowSorting="True" 
                    AutoGenerateColumns="False" ShowFooter="True">
                    <HeaderStyle CssClass="GridHeader" />
                    <RowStyle CssClass="GridItem" Wrap="false" />
                    <AlternatingRowStyle CssClass="GridAltItem" />
                    <PagerStyle CssClass="GridPager" />
                    <Columns>
                        <%--<asp:TemplateField>
                            <HeaderTemplate>
                                <asp:CheckBox ID="cbSelectDt" runat="server" AutoPostBack="true" 
                                    oncheckedchanged="cbSelectDt_CheckedChanged" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="cbSelect1" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>--%>
                        <asp:TemplateField HeaderText="Action" HeaderStyle-Width="100px">
                            <ItemTemplate>
                                <asp:Button ID="btnEditDt" runat="server" class="bitbtndt btnedit" 
                                    CommandName="Edit" Text="Edit" />
                                <asp:Button ID="btnDeleteDt" runat="server" class="bitbtndt btndelete" 
                                    CommandName="Delete" 
                                    OnClientClick="return confirm('Sure to delete this data?');" Text="Delete" />
                                
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:Button ID="btnUpdateDt" runat="server" class="bitbtndt btnsave" 
                                    CommandName="Update" Text="Save" />
                                <asp:Button ID="btnCancelDt" runat="server" class="bitbtndt btncancel" 
                                    CommandName="Cancel" Text="Cancel" />
                            </EditItemTemplate>
                           
                        </asp:TemplateField>
                         <asp:TemplateField HeaderText="Action">
                            <ItemTemplate>
                                <asp:Button ID="btnDetail" runat="server" class="bitbtn btndetail" Text="Detail" CommandName="Detail" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" />   
                            </ItemTemplate>
                        </asp:TemplateField>
                         <asp:BoundField DataField="Product" HeaderStyle-Width="120px" 
                            HeaderText="Product" SortExpression="Product">
                            <HeaderStyle Width="120px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Product_Name" HeaderStyle-Width="200px" 
                            HeaderText="Product Name" SortExpression="Product_Name">
                            <HeaderStyle Width="200px" />
                        </asp:BoundField>
                        <asp:TemplateField Visible="false">
                          <ItemTemplate>
                                <asp:Label ID="lbWrhs" runat="server" 
                                    text='<%# DataBinder.Eval(Container.DataItem, "Warehouse") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="Warehouse_Name" HeaderStyle-Width="200px" 
                            HeaderText="Warehouse" SortExpression="Warehouse_Name">
                            <HeaderStyle Width="200px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="QtyOnHand" DataFormatString="{0:#,##0.00}" 
                            HeaderStyle-Width="150px" HeaderText="Qty OH" SortExpression="QtyOnHand">
                            <HeaderStyle Width="80px" />
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Unit" HeaderStyle-Width="50px" 
                            HeaderText="Unit" SortExpression="Unit">
                            <HeaderStyle Width="50px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="QtySystemOH" DataFormatString="{0:#,##0.00}" 
                            HeaderText="Qty System OH" ItemStyle-HorizontalAlign="Right" 
                            SortExpression="QtySystemOH">
                            <HeaderStyle Width="80px" />
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:BoundField>
                        <asp:BoundField DataField="QtySystemPackage" DataFormatString="{0:#,##0.00}" 
                            HeaderText="Qty System Package" ItemStyle-HorizontalAlign="Right" 
                            SortExpression="QtySystemPackage">
                            <HeaderStyle Width="80px" />
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:BoundField>
                        <asp:BoundField DataField="QtyActualOH" DataFormatString="{0:#,##0.00}" 
                            HeaderText="Qty Actual OH" ItemStyle-HorizontalAlign="Right" 
                            SortExpression="QtyActualOH">
                            <HeaderStyle Width="80px" />
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:BoundField>
                        <asp:BoundField DataField="QtyActualPackage" DataFormatString="{0:#,##0.00}" 
                            HeaderText="Qty Actual Package" SortExpression="QtyActualPackage">
                            <HeaderStyle Width="80px" />
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Remark" HeaderStyle-Width="80px" HeaderText="Remark">
                            <HeaderStyle Width="250px" />
                        </asp:BoundField>
                    </Columns>
                </asp:GridView>
            </div>
            <asp:Button ID="btnAddDt2" runat="server" class="bitbtndt btnadd" Text="Add" 
                ValidationGroup="Input" />
        </asp:Panel>
       <asp:Panel runat="server" ID="pnlEditDt" Visible="false">
            <table>
                <tr>
                    <td>
                        <asp:LinkButton ID="lbProduct" runat="server" Text="Product" />
                    </td>
                    <td>
                        :
                    </td>
                    <td>
                        <asp:TextBox ID="tbProductCode" runat="server" CssClass="TextBoxR" Width="126px"
                            MaxLength="20" Enabled="False" />
                        <asp:TextBox ID="TbProductName" runat="server" CssClass="TextBoxR" Width="250px" Enabled="false"/>
                        <asp:Button class="btngo" runat="server" ID="btnProduct" Text="..." />
                        <asp:Label ID="Label12" runat="server" ForeColor="#FF3300" Text="*"></asp:Label>
                    </td>
                </tr>
                <tr>
                            <td>
                                <asp:LinkButton ID="lbWrhs2" runat="server" Text="Warehouse" />
                            </td>
                            <td>:</td>
                            <td>
                                <asp:DropDownList ID="ddlWrhs" runat="server" 
                                    CssClass="DropDownList" Height="16px" ValidationGroup="Input" 
                                    Width="200px">
                                </asp:DropDownList>
                            </td>
                        </tr>
                <tr>
                            <td>Qty On Hand </td>
                            <td>:</td>
                            <td>
                                        <asp:TextBox CssClass="TextBoxR" runat="server" ID="tbQtyOH" Width="80px" 
                                                AutoPostBack="True" Enabled="False" />
                                
                                        &nbsp;<asp:Label ID="lblUnit" runat="server" Text="Label"></asp:Label>
                                
                            </td>
                        </tr>
                <tr>
                    <td>
                        Qty System</td>
                    <td>
                        :</td>
                    <td>
                        <table cellspacing="0" cellpadding="0">
                                    <tr style="background-color:Silver;text-align:center">
                                        
                                        <td>
                                            OH</td>
                                        <td>
                                            Package</td>
                                    </tr>
                                    <tr>
                                        <td><asp:TextBox CssClass="TextBoxR" runat="server" ID="tbQtySystemOH" 
                                                Width="80px" Enabled="False"/></td>
                                        <td>
                                            <asp:TextBox ID="tbQtySystemPackages" runat="server" CssClass="TextBoxR" 
                                                Enabled="False" Width="80px" />
                                        </td>
                                    </tr>
                                </table></td>
                </tr>
                <tr>
                    <td>
                        Qty Actual</td>
                    <td>
                        :</td>
                    <td>
                        <table cellpadding="0" cellspacing="0">
                            <tr style="background-color:Silver;text-align:center">
                                <td>
                                    OH
                                    <asp:Label ID="Label3" runat="server" ForeColor="Red">*</asp:Label>
                                </td>
                                <td>
                                    Package</td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:TextBox ID="tbQtyActualOH" runat="server" AutoPostBack="True" 
                                        CssClass="TextBoxR" Width="80px" Enabled="False" />
                                </td>
                                <td>
                                    <asp:TextBox ID="tbQtyActualPackages" runat="server" AutoPostBack="True" 
                                        CssClass="TextBoxR" Width="80px" Enabled="False" />
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
                        :
                    </td>
                    <td>
                        <asp:TextBox ID="tbRemarkDt" runat="server" CssClass="TextBox" Height="50px" 
                            MaxLength="255" TextMode="MultiLine" ValidationGroup="Input" Width="352px" />
                    </td>
                </tr>
            </table>
            <br />
            <asp:Button class="bitbtndt btnsave" runat="server" ID="btnSaveDt" Text="Save"/>                              
            <asp:Button class="bitbtndt btncancel" runat="server" ID="btnCancelDt" Text="Cancel"/>                              
            <br />
       </asp:Panel> 
       
        
        
       
      </asp:View>
        
        <asp:View ID="Tab2" runat="server">
                
                <br />  
          <asp:Panel runat="server" ID="pnlLot">
              Product&nbsp; :
              <asp:Label ID="LabelProduct" runat="server" ForeColor="#0092C8" Text="Label"></asp:Label>
              &nbsp;-
              <asp:Label ID="LabelProductName" runat="server" ForeColor="#0092C8" 
                  Text="Label"></asp:Label>
              &nbsp;Warehouse :
              <asp:Label ID="LabelWrhs" runat="server" ForeColor="#0092C8" Text="Label"></asp:Label>
              &nbsp;-
              <asp:Label ID="LabelWrhsName" runat="server" ForeColor="#0092C8" Text="Label"></asp:Label>
              <br />
            </asp:Panel> 
           <br />       
          <asp:Panel runat="server" id="pnlDt2">    
          <asp:Button ID="btnAddDt4" runat="server" class="bitbtndt btnadd" Text="Add" 
                ValidationGroup="Input" />
                  
          <div style="border:0px  solid; width:100%; height:100%; overflow:auto;">
              <asp:GridView ID="GridDt2" runat="server" AllowPaging="False" 
                  AllowSorting="True" AutoGenerateColumns="False" CssClass="Grid" 
                  ShowFooter="True" Width="823px">
                  <HeaderStyle CssClass="GridHeader" />
                  <RowStyle CssClass="GridItem" Wrap="True" />
                  <AlternatingRowStyle CssClass="GridAltItem" />
                  <FooterStyle CssClass="GridFooter"  />
                  <PagerStyle CssClass="GridPager" />
                  <Columns>
                      <asp:TemplateField HeaderText="Action" HeaderStyle-Width="250px" >
                            <ItemTemplate>
                            <asp:Button ID="btnEditDt2" runat="server" class="bitbtndt btnedit" 
                                    CommandName="Edit" Text="Edit" />
                                
                                <asp:Button ID="btnDeleteDt2" runat="server" class="bitbtndt btndelete" 
                                    CommandName="Delete" 
                                    OnClientClick="return confirm('Sure to delete this data?');" Text="Delete" />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:Button ID="btnUpdateDt2" runat="server" class="bitbtndt btnsave" 
                                    CommandName="Update" Text="Save" />
                                <asp:Button ID="btnCancelDt2" runat="server" class="bitbtndt btncancel" 
                                    CommandName="Cancel" Text="Cancel" />
                            </EditItemTemplate>
                          <%-- <FooterTemplate>
                              <asp:Button ID="btnAddDt2" runat="server" class="bitbtndt btnadd" 
                                    CommandName="Add" Text="Add" />
                          </FooterTemplate>--%>
                        </asp:TemplateField>                                         
                      
                      <asp:TemplateField HeaderStyle-Width="180px" HeaderText="Lot No" 
                          SortExpression="LotNo">
                          <Itemtemplate>
                              <asp:Label  ID="LotNo" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "LotNo") %>' Width="100%" />
                          </Itemtemplate>
                          <EditItemTemplate>
                              <asp:Label ID="LotNoEdit" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "LotNo") %>' Width="100%" />
                          </EditItemTemplate>
                         <%-- <FooterTemplate>
                              
                              <asp:TextBox ID="LotNoAdd" Runat="Server" CssClass="TextBox" 
                                  Width="100%" />
                          </FooterTemplate>--%>
                          <HeaderStyle Width="180px" />
                      </asp:TemplateField>
                      <asp:TemplateField HeaderStyle-Width="80" HeaderText="Qty Lot"  
                          SortExpression="Qty">
			            <ItemStyle HorizontalAlign="Right" />
                          <Itemtemplate>
                              <asp:Label ID="Qty" Runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Qty") %>' Width="100%" />
                              <asp:Label ID="QtyKey" Runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "QtyKey") %>' Visible="False"/>
                              
                          </Itemtemplate>
                          <EditItemTemplate>
                              <asp:Label ID="QtyEdit" Runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Qty") %>' Width="100%" />
                              
                             
                          </EditItemTemplate>
                         <%-- <FooterTemplate>
                              <asp:TextBox ID="QtyAdd" Runat="Server" CssClass="TextBox" 
                                  Width="100%" />
                          </FooterTemplate>
                          <HeaderStyle Width="80px" />--%>
                      </asp:TemplateField>
                      <%--<asp:TemplateField HeaderStyle-Width="80" HeaderText="System Package" 
                          SortExpression="QtyPackageSystem">
                          <Itemtemplate>
                              <asp:Label ID="QtyPackageSystem" Runat="server" 
                                  text='<%# DataBinder.Eval(Container.DataItem, "QtyPackageSystem") %>' />
                              
                          </Itemtemplate>
                          <EditItemTemplate>
                              <asp:Label ID="QtyPackageSystemEdit" Runat="server" 
                                  Text='<%# DataBinder.Eval(Container.DataItem, "QtyPackageSystem") %>' 
                                  Width="100%" />
                              
                          </EditItemTemplate>
                          <FooterTemplate>
                              <asp:Label ID="QtyPackageSystemAdd" Runat="server" text="0" Width="100%" />
                              
                          </FooterTemplate>
                          <HeaderStyle Width="80px" />
                      </asp:TemplateField>--%>
                      <asp:TemplateField HeaderStyle-Width="80" HeaderText="Actual Package" 
                          SortExpression="QtyPackageActual">
			               <ItemStyle HorizontalAlign="Right" />
                          <Itemtemplate>
                              <asp:Label ID="QtyPackageActual" Runat="server" 
                                  Text='<%# DataBinder.Eval(Container.DataItem, "QtyPackageActual") %>' 
                                  Width="100%" />
                              <asp:Label ID="QtyPackageSystem" Runat="server" visible="false"
                                  Text='<%# DataBinder.Eval(Container.DataItem, "QtyPackageSystem") %>' 
                                  Width="100%" />
                          </Itemtemplate>
                          <EditItemTemplate>
                              <asp:TextBox ID="QtyPackageActualEdit" Runat="server" CssClass="TextBox" 
                                  Text='<%# DataBinder.Eval(Container.DataItem, "QtyPackageActual") %>' 
                                  Width="100%">
                              </asp:TextBox>
                              <%-- <cc1:TextBoxWatermarkExtender ID="QQtyPackageActualEdit_WtExt" runat="server" 
                                    Enabled="True" TargetControlID="QtyPackageActualEdit" WatermarkCssClass="Watermarked" 
                                    WatermarkText="can't blank"></cc1:TextBoxWatermarkExtender>--%>
                          </EditItemTemplate>
                       <FooterTemplate>
                             <%-- <asp:TextBox ID="QtyPackageActualAdd" Runat="Server" CssClass="TextBox" 
                                  Width="100%" />--%>
                          </FooterTemplate>
                          <HeaderStyle Width="80px" />
                      </asp:TemplateField>
                      <asp:TemplateField HeaderStyle-Width="100" HeaderText="Expire Date" 
                          SortExpression="ExpireDate">
                          <ItemTemplate>
                              <asp:Label ID="ExpireDate" Runat="server" 
                                  Text='<%# DataBinder.Eval(Container.DataItem, "ExpireDate") %>' 
                                  Width="100%" />
                          </ItemTemplate>
                          <EditItemTemplate>
                               <BDP:BasicDatePicker ID="ExpireDateEdit" runat="server" AutoPostBack="False" 
                                  ButtonImageHeight="19px" ButtonImageWidth="20px" DateFormat="dd/MMM/yyyy" 
                                  DisplayType="TextBoxAndImage" ReadOnly="true" ShowNoneButton="False" 
                                  TextBoxStyle-CssClass="TextDate" ValidationGroup="Input"><TextBoxStyle CssClass="TextDate" /></BDP:BasicDatePicker>
                          </EditItemTemplate>
                         <%-- <FooterTemplate>
                              <BDP:BasicDatePicker ID="ExpireDateAdd" runat="server" AutoPostBack="False" 
                                  SelectedDate='<%# DataBinder.Eval(Container.DataItem, "ExpireDate") %>' 
                                  ButtonImageHeight="19px" ButtonImageWidth="20px" DateFormat="dd/MMM/yyyy" 
                                  DisplayType="TextBoxAndImage" ReadOnly="true" ShowNoneButton="False" 
                                  TextBoxStyle-CssClass="TextDate" ValidationGroup="Input"><TextBoxStyle CssClass="TextDate" /></BDP:BasicDatePicker>
                          </FooterTemplate>--%>
                          
                          <HeaderStyle Width="100px" />
                      </asp:TemplateField>
                      <asp:TemplateField HeaderStyle-Width="180" HeaderText="Pallet No" 
                          SortExpression="PalletNo">
			 <ItemStyle HorizontalAlign="Right" />
                          <Itemtemplate>
                            <asp:Label ID="PalletNo" Runat="server" 
                                  Text='<%# DataBinder.Eval(Container.DataItem, "PalletNo") %>' 
                                  Width="100%" />
                          </Itemtemplate>
                          <EditItemTemplate>
                             <asp:Label ID="PalletNoEdit" Runat="server" 
                                  Text='<%# DataBinder.Eval(Container.DataItem, "PalletNo") %>' 
                                  Width="100%" />
                          </EditItemTemplate>
                         <%-- <FooterTemplate>
                               <asp:TextBox ID="PalletNoAdd" Runat="Server" CssClass="TextBox" 
                                  Width="100%" />
                          </FooterTemplate>--%>
                          <HeaderStyle Width="180px" />
                      </asp:TemplateField>
			<asp:TemplateField HeaderStyle-Width="250" HeaderText="Status" 
                          SortExpression="Status">
                          <Itemtemplate>
                             <asp:Label ID="Status" Runat="server" 
                                  Text='<%# DataBinder.Eval(Container.DataItem, "Status") %>' 
                                  Width="100%" />
                          </Itemtemplate>
                          <EditItemTemplate>
                              <asp:TextBox ID="StatusEdit" Runat="server" CssClass="TextBox" MaxLength="255" 
                                  Text='<%# DataBinder.Eval(Container.DataItem, "Status") %>' Width="100%">
                              </asp:TextBox>
                          </EditItemTemplate>
                         <%-- <FooterTemplate>
                              <asp:TextBox ID="StatusAdd" Runat="Server" CssClass="TextBox" MaxLength="255" 
                                  Width="100%" />
                          </FooterTemplate>--%>
                          <HeaderStyle Width="250px" />
                      </asp:TemplateField>

                      <asp:TemplateField HeaderStyle-Width="250" HeaderText="Remark" 
                          SortExpression="Remark">
                          <Itemtemplate>
                              <asp:Label ID="Remark" Runat="server" 
                                  Text='<%# DataBinder.Eval(Container.DataItem, "Remark") %>' 
                                  Width="100%" />
                          </Itemtemplate>
                          <EditItemTemplate>
                              <asp:TextBox ID="RemarkEdit" Runat="server" CssClass="TextBox" MaxLength="255" 
                                  Text='<%# DataBinder.Eval(Container.DataItem, "Remark") %>' Width="100%">
                              </asp:TextBox>
                          </EditItemTemplate>
                          <%--<FooterTemplate>
                              <asp:TextBox ID="RemarkAdd" Runat="Server" CssClass="TextBox" MaxLength="255" 
                                  Width="100%" />
                          </FooterTemplate>--%>
                          <HeaderStyle Width="250px" />
                      </asp:TemplateField>

                      
                  </Columns>
              </asp:GridView>
              <asp:Button ID="btnAddDt3" runat="server" class="bitbtndt btnadd" Text="Add" 
                  ValidationGroup="Input" />
              <br />
              <br />
              <%--<asp:Button ID="btnOK" runat="server" class="bitbtndt btnsave" Text="Apply" 
                  validationgroup="Input" /> --%>
              &nbsp;<asp:Button ID="btnBackDt2" runat="server" class="bitbtndt btncancel" 
                  Text="Back" validationgroup="Input" />
            
          </asp:Panel>
          <asp:Panel runat="server" ID="PnlEditDt2" Visible="false">
            <table>
                <tr>
                    <td>
                        Lot No
                    </td>
                    <td>
                        :
                    </td>
                    <td>
                        <asp:TextBox ID="tbLotNo" runat="server" CssClass="TextBox" Width="126px"
                            MaxLength="20" />
                        
                    </td>
                </tr>
                <tr>
                            <td>
                                Lot Qty
                            </td>
                            <td>:</td>
                            <td>
                                 <asp:TextBox ID="tbLotQty" runat="server" CssClass="TextBox" Width="126px"
                            MaxLength="10" />
                                <asp:TextBox ID="tbLotQtyKey" runat="server" CssClass="TextBox" Visible="false" />
                            </td>
                        </tr>
                <tr>
                            <td>Actual Packages </td>
                            <td>:</td>
                            <td>
                                        <asp:TextBox CssClass="TextBox" runat="server" ID="tbQtyActual" Width="80px" 
                                                AutoPostBack="False" Enabled="True" />
                                
                                      
                                
                            </td>
                        </tr>
                <tr>
                    <td>
                        Expire Date</td>
                    <td>
                        :</td>
                    <td>
                      <BDP:BasicDatePicker ID="tbExpireDate" runat="server" AutoPostBack="True" 
                    ButtonImageHeight="19px" ButtonImageWidth="20px" DateFormat="dd MMM yyyy" 
                    DisplayType="TextBoxAndImage" ReadOnly="true" ShowNoneButton="False" 
                    TextBoxStyle-CssClass="TextDate" ValidationGroup="Input"><TextBoxStyle CssClass="TextDate" /></BDP:BasicDatePicker>
              </td>
                </tr>
                <tr>
                    <td>
                        Pallet No</td>
                    <td>
                        :</td>
                    <td>
                         <asp:TextBox CssClass="TextBox" runat="server" ID="tbPalletNo" Width="80px" />
                    </td>
                </tr>
                <tr>
                    <td>
                        Status
                    </td>
                    <td>
                        :
                    </td>
                    <td>
                        <asp:TextBox ID="tbStatus" runat="server" CssClass="TextBox" Height="50px" 
                            MaxLength="255" TextMode="MultiLine" ValidationGroup="Input" Width="352px" />
                    </td>
                </tr>
                <tr>
                    <td>
                        Remark
                    </td>
                    <td>
                        :
                    </td>
                    <td>
                        <asp:TextBox ID="tbRemarkDt2" runat="server" CssClass="TextBox" Height="50px" 
                            MaxLength="255" TextMode="MultiLine" ValidationGroup="Input" Width="352px" />
                    </td>
                </tr>
            </table>
            <br />
            <asp:Button class="bitbtndt btnsave" runat="server" ID="btnsavedt2" Text="Save"/>                              
            <asp:Button class="bitbtndt btncancel" runat="server" ID="btncancelDt2" Text="Cancel"/>                              
            <br />
       </asp:Panel>
            </asp:View> 
        </asp:MultiView> 
        <br />          
       <asp:Button class="bitbtn btnsavenew" runat="server" ID="btnSaveAll" 
            Text="Save & New" ValidationGroup="Input" Width="96px"/>                              
       <asp:Button class="bitbtn btnsave" runat="server" ID="btnSaveTrans" Text="Save" ValidationGroup="Input"/>                              
       <asp:Button class="bitbtn btnback" runat="server" ID="btnBack" Text="Cancel" ValidationGroup="Input"/>                              
       <asp:Button class="btngo" runat="server" ID="btnHome" Text="Home" Width="45px"/>    
                                  
    </asp:Panel>
     </div>
    <asp:Panel runat="server" ID="pnlPrint" Visible="false">
    <cr:crystalreportviewer ID="CrystalReportViewer1" runat="server" AutoDataBind="True" Height="1036px" Width="928px" />
    </asp:Panel>
      <br />     
     <asp:SqlDataSource ID="dsWarehouse" runat="server"                                                                                 
           SelectCommand="SELECT WrhsCode, WrhsName FROM MsWarehouse">                                        
        </asp:SqlDataSource>           
    </div>
    <asp:Label runat ="server" ID="lbStatus" ForeColor="Red"/>
    </form>
    </body>
</html>
