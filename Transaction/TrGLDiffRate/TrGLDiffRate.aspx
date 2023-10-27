<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TrGLDiffRate.aspx.vb" Inherits="Transaction_TrGLDiffRate_TrGLDiffRate" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="BasicFrame.WebControls.BasicDatePicker" Namespace="BasicFrame.WebControls"
    TagPrefix="BDP" %>
<%@ Register Assembly="CrystalDecisions.Web, Version=10.5.3700.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Untitled</title>

    <script src="../../Function/OpenDlg.js" type="text/javascript"></script>
    <script src="../../Function/Function.JS" type="text/javascript"></script>
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />

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
         document.getElementById("tbRate").value = setdigit(document.getElementById("tbRate").value.replace(/\$|\,/g,""),'<%=VIEWSTATE("DigitCurr")%>');
        
        }catch (err){
            alert(err.description);
          }      
        }   
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="Content">
        <div class="H1">
            <asp:Label runat="server" ID="lblJudul" Text="Difference Rate" /></div>
        <hr style="color: Blue" />
        <asp:Panel runat="server" ID="PnlHd">
            <table>
                <tr>
                    <td style="width: 100px; text-align: right">
                        Quick Search :
                    </td>
                    <td>
                        <asp:TextBox CssClass="TextBox" runat="server" ID="tbFilter" />
                        <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlField">
                            <asp:ListItem Value="Reference" Selected="True">Reference</asp:ListItem>
                            <asp:ListItem Value="Trans_Date">Date</asp:ListItem>
                            <asp:ListItem>Status</asp:ListItem>
                            <asp:ListItem Value="Diff_Date">Difference Date</asp:ListItem>
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
                                <asp:ListItem Value="Reference" Selected="True">Reference</asp:ListItem>
                                <asp:ListItem Value="Trans_Date">Date</asp:ListItem>
                                <asp:ListItem>Status</asp:ListItem>
                                <asp:ListItem Value="Diff_Date">Difference Date</asp:ListItem>
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
            <asp:Button class="btngo" runat="server" ID="BtnGo" Text="G" />
            <br />
            <div style="border: 0px  solid; width: 100%; height: 100%; overflow: auto;">
                <asp:GridView ID="GridView1" runat="server" AllowPaging="True" AllowSorting="True"
                    CssClass="Grid" AutoGenerateColumns="False">
                    <HeaderStyle CssClass="GridHeader"></HeaderStyle>
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
                        <asp:BoundField DataField="Reference" HeaderStyle-Width="120px" SortExpression="Nmbr"
                            HeaderText="Reference">
                            <HeaderStyle Width="120px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Status" HeaderText="Status" SortExpression="Status"></asp:BoundField>
                        <asp:BoundField DataField="Trans_Date" HeaderStyle-Width="80px" HeaderText="Date"
                            SortExpression="TransDate">
                            <HeaderStyle Width="80px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="DiffDate" HeaderText="Diffrance Date" SortExpression="DiffDate">
                            <HeaderStyle Width="100px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Remark" HeaderStyle-Width="80px" HeaderText="Remark">
                            <HeaderStyle Width="250px" />
                        </asp:BoundField>
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
                        Difference Date
                    </td>
                    <td>
                        :
                    </td>
                    <td>
                        <BDP:BasicDatePicker ID="tbDiffDate" runat="server" AutoPostBack="True" ButtonImageHeight="19px"
                            ButtonImageWidth="20px" DateFormat="dd MMM yyyy" DisplayType="TextBoxAndImage"
                            TextBoxStyle-CssClass="TextDate" ValidationGroup="Input" ShowNoneButton="False">
                            <TextBoxStyle CssClass="TextDate" />
                        </BDP:BasicDatePicker>
                        &nbsp; &nbsp; &nbsp;
                        <asp:Button class="bitbtn btngetitem" runat="server" ID="btnGetDt" Text="Get Item" />
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
                            Width="269px" />
                        &nbsp; &nbsp; &nbsp;
                        <asp:Button class="bitbtn btngetitem" runat="server" ID="btnGetData" Text="Get Currency"
                            Width="100" />
                    </td>
                </tr>
            </table>
            <br />
            <div style="font-size: medium; color: Blue;">
                Detail</div>
            <hr style="color: Blue" />
            <asp:Panel runat="server" ID="pnlDt">
                <asp:Button class="bitbtndt btnadd" runat="server" ID="btnAdddt" Text="Add" ValidationGroup="Input" />
                <div style="border: 0px  solid; width: 100%; height: 100%; overflow: auto;">
                    <asp:GridView ID="GridDt" runat="server" AutoGenerateColumns="False" ShowFooter="True">
                        <HeaderStyle CssClass="GridHeader" />
                        <RowStyle CssClass="GridItem" Wrap="false" />
                        <AlternatingRowStyle CssClass="GridAltItem" />
                        <PagerStyle CssClass="GridPager" />
                        <Columns>
                            <asp:TemplateField HeaderText="Action">
                                <ItemTemplate>
                                    <asp:Button ID="btnEdit" runat="server" class="bitbtndt btnedit" Text="Edit" CommandName="Edit" />
                                    <asp:Button ID="btnDelete" runat="server" class="bitbtndt btndelete" Text="Delete"
                                        OnClientClick="return confirm('Sure to delete this data?');" CommandName="Delete" />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Button ID="btnUpdate" runat="server" class="bitbtndt btnsave" Text="Save" CommandName="Update" />
                                    <asp:Button ID="btnCancel" runat="server" class="bitbtndt btncancel" Text="Cancel"
                                        CommandName="Cancel" />
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Currency" HeaderStyle-Width="200px" HeaderText="Currency"
                                SortExpression="Currency">
                                <HeaderStyle Width="80px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="NewRate" HeaderText="New Rate" HeaderStyle-Width="120px"
                                ItemStyle-HorizontalAlign="Right" SortExpression="NewRate">
                                <HeaderStyle Width="150px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Remark" HeaderStyle-Width="80px" HeaderText="Remark">
                                <HeaderStyle Width="250px" />
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
                            <asp:LinkButton ID="lbCurrency" runat="server" Text="Currency" />
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlCurrency" runat="server" AutoPostBack="True" CssClass="DropDownList" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            New Rate
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                            <asp:TextBox ID="tbRate" runat="server" AutoPostBack="False" CssClass="TextBox" />
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
                            <asp:TextBox ID="tbRemarkDt" runat="server" CssClass="TextBox" Width="304px" />
                        </td>
                    </tr>
                </table>
                <br />
                <asp:Button ID="btnSaveDt" runat="server" class="bitbtndt btnsave" Text="Save" />
                &nbsp;
                <asp:Button ID="btnCancelDt" runat="server" class="bitbtndt btncancel" Text="Cancel" />
                <br />
            </asp:Panel>
            <br />
            <asp:Panel runat="server" ID="PnlDt2">
                <div style="border: 0px  solid; width: 100%; height: 100%; overflow: auto;">
                    <asp:GridView ID="GridDt2" runat="server" AutoGenerateColumns="False" ShowFooter="True">
                        <HeaderStyle CssClass="GridHeader" />
                        <RowStyle CssClass="GridItem" Wrap="false" />
                        <AlternatingRowStyle CssClass="GridAltItem" />
                        <PagerStyle CssClass="GridPager" />
                        <Columns>
                            <asp:BoundField DataField="Currency" HeaderStyle-Width="200px" HeaderText="Currency"
                                SortExpression="Currency">
                                <HeaderStyle Width="80px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Account" HeaderText="Account" HeaderStyle-Width="120px"
                                SortExpression="Account">
                                <HeaderStyle Width="150px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="AccountName" HeaderStyle-Width="80px" HeaderText="Account Name"
                                SortExpression="AccountName">
                                <HeaderStyle Width="250px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="FgSubled" HeaderText="Subled" SortExpression="FgSubled" />
                            <asp:BoundField DataField="SubledName" HeaderText="Subled Name" SortExpression="SubledName" />
                            <asp:BoundField DataField="AmountForex" HeaderText="Amount Forex" SortExpression="AmountForex"
                                ItemStyle-HorizontalAlign="Right" />
                            <asp:BoundField DataField="AmountHome" HeaderText="Amount Home" SortExpression="AmountHome"
                                ItemStyle-HorizontalAlign="Right" />
                            <asp:BoundField DataField="NewAmountHome" HeaderText="New Amount Home" SortExpression="NewAmountHome"
                                ItemStyle-HorizontalAlign="Right" />
                            <asp:BoundField DataField="AmountAdjust" HeaderText="Amount Ajdust" SortExpression="AmountAdjust"
                                ItemStyle-HorizontalAlign="Right" />
                        </Columns>
                    </asp:GridView>
                </div>
            </asp:Panel>
            <br />
            <asp:Button ID="btnSaveAll" runat="server" class="bitbtndt btnsave" Text="Save & New"
                ValidationGroup="Input" Width="97px" />
            &nbsp;
            <asp:Button ID="btnSaveTrans" runat="server" class="bitbtndt btnsave" Text="Save"
                ValidationGroup="Input" />
            &nbsp;
            <asp:Button ID="btnBack" runat="server" class="bitbtndt btncancel" Text="Cancel"
                ValidationGroup="Input" />
            <asp:Button ID="btnHome" runat="server" class="bitbtndt btnback" Text="Home" />
        </asp:Panel>
        <asp:Panel runat="server" ID="pnlPrint" Visible="false">
            <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" AutoDataBind="True"
                Height="1036px" Width="928px" />
        </asp:Panel>
        <br />
    </div>
    <asp:Label runat="server" ID="lbStatus" ForeColor="Red" />
    </form>
</body>
</html>
