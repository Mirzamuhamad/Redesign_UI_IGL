<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TrBeginningWIP.aspx.vb" Inherits="Transaction_TrBeginningWIP_TrBeginningWIP" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="BasicFrame.WebControls.BasicDatePicker" Namespace="BasicFrame.WebControls"
    TagPrefix="BDP" %>
<%@ Register assembly="CrystalDecisions.Web, Version=10.5.3700.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Beginning WIP</title>
    <script src="../../Function/OpenDlg.js" type="text/javascript"></script>
    <script src="../../Function/Function.JS" type="text/javascript"></script>
    
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
    
        function setformat()
        {
        
         try
         {           
        document.getElementById("tbOldPrice").value = setdigit(document.getElementById("tbOldPrice").value.replace(/\$|\,/g,""),'<%=VIEWSTATE("DigitCurr")%>');
        document.getElementById("tbNewPrice").value = setdigit(document.getElementById("tbNewPrice").value.replace(/\$|\,/g,""),'<%=VIEWSTATE("DigitCurr")%>');
//        document.getElementById("tbAdjustPercent").value = setdigit(document.getElementById("tbAdjustPercent").value.replace(/\$|\,/g,""),'<%=VIEWSTATE("DigitCurr")%>');
        }catch (err){
            alert(err.description);
          }      
        }
        
        $(document).ready(function(){
            $("#tbTime").mask("'00:00");
        });
        
        function hitung(prmchange)
        {
            var tbQty = document.getElementById("tbQty");
            var tbPrice = document.getElementById("tbPrice");
            var tbTotal = document.getElementById("tbTotal");
            
            if (tbQty.value == "")
            {
                tbQty.value == "0";
            }
            if (tbPrice.value == "")
            {
                tbPrice.value == "0";
            }
            
            tbTotal.value = parseFloat(tbQty.value.replace(/,/g,"")) * parseFloat(tbPrice.value.replace(/,/g,""))
        }  
    </script>
    
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
</head>
<body>     
    <form id="form1" runat="server">    
    <div class="Content">
    <div class="H1">Beginning WIP</div>
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
                                    <%--<asp:ListItem Text="Print" />--%>
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
        <table>
            <tr>
                <td>
                    Reference
                </td>
                <td>
                    :
                </td>
                <td>
                    <asp:TextBox CssClass="TextBox" Enabled="false" runat="server" ID="tbRef" Width="149px" />
                </td>
            </tr>
            <tr>
                <td>
                    Date
                </td>
                <td>
                    :
                </td>
                <td>
                    <BDP:BasicDatePicker ID="tbDate" runat="server" DateFormat="dd MMM yyyy" ReadOnly="true"
                        ValidationGroup="Input" ButtonImageHeight="19px" ButtonImageWidth="20px" DisplayType="TextBoxAndImage"
                        TextBoxStyle-CssClass="TextDate" AutoPostBack="True" ShowNoneButton="False">
                        <TextBoxStyle CssClass="TextDate" />
                    </BDP:BasicDatePicker>
                </td>
            </tr>
            <tr>
                <td>
                    Warehouse
                </td>
                <td>
                    :
                </td>
                <td>
                    <asp:DropDownList CssClass="DropDownList" ValidationGroup="Input" runat="server"
                        ID="ddlWarehouse">
                    </asp:DropDownList>
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
                    <asp:TextBox ID="tbRemark" runat="server" CssClass="TextBox" ValidationGroup="Input"
                        Width="352px" MaxLength="255" Height="50px" TextMode="MultiLine" />
                </td>
            </tr>
        </table>  
      
      <br />      
      <div style="font-size:medium; color:Blue;">Detail</div>
      <hr style="color:Blue" />  
        <asp:Panel runat="server" ID="pnlDt">
            <table>
            <tr>
            <td>            
            <%--<asp:Panel ID="Panel1" runat="server" BorderColor="Black" BorderStyle="Solid" 
                    Height="100%" Width="224px">
            &nbsp;<asp:Label ID="Label1" runat="server" Font-Bold="True" ForeColor="Blue" Text="Set Adjust"></asp:Label>
            <asp:DropDownList ID="ddlAdjustType" runat="server" AutoPostBack="False" 
                    CssClass="DropDownList" Height="17px" Width="36px" >
                <asp:ListItem>+</asp:ListItem>
                <asp:ListItem>-</asp:ListItem>
                </asp:DropDownList>
            <asp:TextBox ID="tbAdjPercent" runat="server" AutoPostBack="False" 
                    CssClass="TextBox" Width="54px" /> %
            <asp:Button class="btngo" runat="server" ID="btnProcess" Text="Process" ValidationGroup="Input" Width="49px"/>       
            </asp:Panel>--%>
            </td>
            </tr>
            </table>
            <asp:Button class="bitbtndt btnadd" runat="server" ID="btnAddDt" Text="Add" ValidationGroup="Input"/>       
            <div style="border:0px  solid; width:100%; height:100%; overflow:auto;">
                <asp:GridView ID="GridDt" runat="server" AutoGenerateColumns="False" 
                    ShowFooter="True">
                    <HeaderStyle CssClass="GridHeader" />
                    <RowStyle CssClass="GridItem" Wrap="false" />
                    <AlternatingRowStyle CssClass="GridAltItem" />
                    <PagerStyle CssClass="GridPager" />
                    <Columns>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:CheckBox ID="cbSelectHd" runat="server" AutoPostBack="true" oncheckedchanged="cbSelectHd_CheckedChanged1" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="cbSelect" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Action">
                            <ItemTemplate>
                                <asp:Button class="bitbtndt btnedit" runat="server" ID="btnEdit" Text="Edit" CommandName="Edit"/>       
                                <asp:Button class="bitbtndt btndelete" runat="server" ID="btnDelete" Text="Delete" CommandName="Delete"/>       
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:Button class="bitbtndt btnsave" runat="server" ID="btnUpdate" Text="Save" CommandName="Update"/>       
                                <asp:Button class="bitbtndt  btncancel" runat="server" ID="btnCancel" Text="Cancel" CommandName="Cancel"/>       
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <%--<asp:BoundField DataField="TransNmbr" HeaderStyle-Width="50px" HeaderText="Reference">
                        <HeaderStyle Width="50px" />
                        </asp:BoundField>--%>
                        <asp:BoundField DataField="WIP" HeaderStyle-Width="100px" HeaderText="WIP">
                        <HeaderStyle Width="100px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="WIPName" HeaderStyle-Width="200px" HeaderText="WIP Name">
                        <HeaderStyle Width="200px" />
                        </asp:BoundField>
                        
                        <asp:BoundField DataField="ProductFG" HeaderStyle-Width="100px" HeaderText="Product">
                        <HeaderStyle Width="100px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="ProductName" HeaderStyle-Width="200px" HeaderText="Product Name">
                        <HeaderStyle Width="200px" />
                        </asp:BoundField>
                        
                        <asp:BoundField DataField="Qty" HeaderText="Qty" ItemStyle-HorizontalAlign = "Right">
                        <HeaderStyle Width="100px" />
                        </asp:BoundField >
                        <asp:BoundField DataField="Price" HeaderText="Price" ItemStyle-HorizontalAlign = "Right">
                        <HeaderStyle Width="100px" />
                        </asp:BoundField >
                        <asp:BoundField DataField="Total" HeaderText="Total" ItemStyle-HorizontalAlign = "Right">
                        <HeaderStyle Width="100px" />
                        </asp:BoundField >
                        <asp:BoundField DataField="Remark"  HeaderStyle-Width="200px" HeaderText="Remark" >
                        <HeaderStyle Width="200px" />
                        </asp:BoundField>
                    </Columns>
                </asp:GridView>
          </div>   
          <asp:Button class="bitbtndt btnadd" runat="server" ID="btnAddDt2" Text="Add" ValidationGroup="Input" />       
       </asp:Panel>             
        <asp:Panel runat="server" ID="pnlEditDt" Visible="false">
            <table>
                <tr>
                    <td>
                        <asp:LinkButton ID="lbWIP" runat="server" Text="WIP" />
                    </td>
                    <td>
                        :
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlWIP" runat="server" CssClass="DropDownList" Height="17px" AutoPostBack="true">
                        </asp:DropDownList>
                        <asp:Label ID="Label1" runat="server" ForeColor="#FF3300" Text="*"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        Product
                    </td>
                    <td>
                        :
                    </td>
                    <td>
                        <asp:TextBox ID="tbProductCode" runat="server" CssClass="TextBox" MaxLength="20"
                            ValidationGroup="Input" Width="126px" AutoPostBack="true" />
                        <asp:TextBox ID="tbProductName" runat="server" CssClass="TextBoxR" ValidationGroup="Input"
                            Width="250px" Enabled="false"/>
                        <asp:Button ID="btnProduct" runat="server" class="bitbtn btngo" Text="..." ValidationGroup="Input" />
                    </td>
                </tr>
                <tr>
                    <td>
                        Qty
                    </td>
                    <td>
                        :
                    </td>
                    <td>
                        <asp:TextBox ID="tbQty" runat="server" CssClass="TextBox" AutoPostBack="false" />
                        <asp:Label ID="Label2" runat="server" ForeColor="#FF3300" Text="*"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        Price
                    </td>
                    <td>
                        :
                    </td>
                    <td>
                        <asp:TextBox ID="tbPrice" runat="server" CssClass="TextBox" AutoPostBack="false" />
                        <asp:Label ID="Label3" runat="server" ForeColor="#FF3300" Text="*"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        Total
                    </td>
                    <td>
                        :
                    </td>
                    <td>
                        <asp:TextBox ID="tbTotal" runat="server" CssClass="TextBoxR" AutoPostBack="false" />
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
                        <asp:TextBox ID="tbRemarkDt" runat="server" CssClass="TextBox" ValidationGroup="Input"
                            Width="352px" MaxLength="255" Height="50px" TextMode="MultiLine" />
                    </td>
                </tr>
            </table>
            <br />
            <asp:Button class="bitbtndt btnsave" runat="server" ID="btnSaveDt" Text="Save" />
            <asp:Button class="bitbtndt btncancel" runat="server" ID="btnCancelDt" Text="Cancel" />
            <br />
        </asp:Panel> 
       <br />          
       <asp:Button class="bitbtn btnsavenew" runat="server" ID="btnSaveAll" 
            Text="Save & New" ValidationGroup="Input" Width="96px"/>                              
       <asp:Button class="bitbtn btnsave" runat="server" ID="btnSaveTrans" Text="Save" ValidationGroup="Input"/>                              
       <asp:Button class="bitbtn btnback" runat="server" ID="btnBack" Text="Cancel" ValidationGroup="Input"/>                              
       <asp:Button class="btngo" runat="server" ID="btnHome" Text="Home" Width="45px"/>                              
    </asp:Panel>
    <asp:Panel runat="server" ID="pnlPrint" Visible="false">
    <cr:crystalreportviewer ID="CrystalReportViewer1" runat="server" AutoDataBind="True" Height="1036px" Width="928px" />
    </asp:Panel>
      <br />            
    </div>
    <asp:Label runat ="server" ID="lbStatus" ForeColor="Red"/>
    </form>
    </body>
</html>
