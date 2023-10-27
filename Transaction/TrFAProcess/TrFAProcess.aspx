<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TrFAProcess.aspx.vb" Inherits="Transaction_TrFAProcess_TrFAProcess" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="BasicFrame.WebControls.BasicDatePicker" Namespace="BasicFrame.WebControls"
    TagPrefix="BDP" %>
<%@ Register Assembly="CrystalDecisions.Web, Version=10.5.3700.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Untitled Page</title>

    <script src="../../Function/OpenDlg.js" type="text/javascript"></script>
    <script src="../../Function/Function.JS" type="text/javascript"></script>
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript">         
        function setdigit(nStr, digit)
        {
        try
        {
        var TNstr = parseFloat(nStr);        
        TNstr = TNstr.toFixed(digit);                
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
        //var Rate = document.getElementById("tbRate");
        //var DrForex = document.getElementById("tbDebitForex");
        //var CrForex = document.getElementById("tbCreditForex");
        
         try
         {           
        //document.getElementById("tbDebitHome").value = Rate.value.replace(",","") * DrForex.value.replace(",","");
        //document.getElementById("tbCreditHome").value = Rate.value.replace(",","") * CrForex.value.replace(",","");
        document.getElementById("tbLife").value = setdigit(document.getElementById("tbLife").value.replace(/\$|\,/g,""),'<%=ViewState("DigitHome")%>');
        document.getElementById("tbBalanceAmount").value = setdigit(document.getElementById("tbBalanceAmount").value.replace(/\$|\,/g,""),'<%=ViewState("DigitHome")%>');
        document.getElementById("tbProcess").value = setdigit(document.getElementById("tbProcess").value.replace(/\$|\,/g,""),'<%=ViewState("DigitRate")%>');
        document.getElementById("tbAdjust").value = setdigit(document.getElementById("tbAdjust").value.replace(/\$|\,/g,""),'<%=ViewState("DigitCurr")%>');
        document.getElementById("tbTotal").value = setdigit(document.getElementById("tbTotal").value.replace(/\$|\,/g,""),'<%=ViewState("DigitCurr")%>');
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
            Fixed & Expendable Depreciation</div>
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
                            <asp:ListItem>Year</asp:ListItem>
                            <asp:ListItem>Period</asp:ListItem>
                            <asp:ListItem>Status</asp:ListItem>
                            <asp:ListItem>Remark</asp:ListItem>
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
                                <asp:ListItem>Year</asp:ListItem>
                                <asp:ListItem>Period</asp:ListItem>
                                <asp:ListItem>Status</asp:ListItem>
                                <asp:ListItem>Remark</asp:ListItem>
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
            <br />&nbsp;
            <div style="border: 0px  solid; width: 100%; height: 100%; overflow: auto;">
                <asp:GridView ID="GridView1" runat="server" AllowPaging="True" AllowSorting="False"
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
                        <asp:TemplateField HeaderStyle-Width="100" HeaderText="Action">
                            <ItemTemplate>
                                <asp:DropDownList CssClass="DropDownList" ID="ddl" runat="server">
                                    <asp:ListItem Selected="True" Text="View" />
                                    <asp:ListItem Text="Edit" />
                                    <asp:ListItem Text="Print" />
                                </asp:DropDownList>
                                <asp:Button class="btngo" runat="server" ID="BtnGo" Text="G" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>"
                                    CommandName="Go" />
                            </ItemTemplate> 
                            <HeaderStyle Width="100px" />
                            
                        </asp:TemplateField>
                        <asp:BoundField DataField="Reference" HeaderText="Reference" Visible="False" />
                        <asp:BoundField DataField="Year" HeaderText="Year" SortExpression="Year">
                            <HeaderStyle Width="40px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Period" HeaderText="Period" ItemStyle-HorizontalAlign="Center" SortExpression="Period">
                            <HeaderStyle Width="50px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="PeriodName" HeaderStyle-Width="80px" HeaderText="Month"
                            SortExpression="Period">
                            <HeaderStyle Width="80px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Status" HeaderText="Status" HeaderStyle-Width="50px" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="Remark" HeaderText="Remark">
                            <HeaderStyle Width="250px" />
                        </asp:BoundField>
                    </Columns>
                </asp:GridView>
            </div> <br />&nbsp;
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
                        Year
                    </td>
                    <td>
                        :
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlYear" runat="server" AutoPostBack="True" CssClass="DropDownList"
                            Width="80px" Enabled="false" ValidationGroup="Input">
                        </asp:DropDownList>
                        &nbsp;<asp:TextBox ID="tbRef" runat="server" CssClass="TextBox" ValidationGroup="Input"
                            Visible="False" Width="109px" />
                    </td>
                </tr>
                <tr>
                    <td>
                        Period
                    </td>
                    <td>
                        :
                    </td>
                    <td>
                        <asp:DropDownList runat="server" Enabled="false" CssClass="DropDownList" ID="ddlPeriod"
                            Width="100px" ValidationGroup="Input">
                        </asp:DropDownList>
                        &nbsp;&nbsp;
                        <asp:Button class="btngo" runat="server" ID="btnGetDt" Text="Get Data" ValidationGroup="Input"
                            Width="50px" />
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
                        <asp:TextBox runat="server" ValidationGroup="Input" ID="tbRemark" CssClass="TextBox"
                            Width="300px" />
                    </td>
                </tr>
            </table>
            <br />
            <div style="font-size: medium; color: Blue;">
                Detail</div>
            <hr style="color: Blue" />
            <asp:Panel runat="server" ID="pnlDt">
                <div style="border: 0px  solid; width: 100%; height: 100%; overflow: auto;">
                    <asp:GridView ID="GridDt" runat="server" AutoGenerateColumns="False" ShowFooter="True">
                        <HeaderStyle CssClass="GridHeader" />
                        <RowStyle CssClass="GridItem" />
                        <AlternatingRowStyle CssClass="GridAltItem" />
                        <PagerStyle CssClass="GridPager" />
                        <FooterStyle CssClass="GridAltItem" Wrap="false" />
                        <Columns>
                            <asp:TemplateField HeaderText="Action" ItemStyle-Wrap="false">
                                <ItemTemplate>
                                    <asp:Button class="bitbtndt btnedit" runat="server" ID="btnEdit" Text="Edit" CommandName="Edit" />
                                    <asp:Button class="bitbtndt btndelete" runat="server" ID="btnDelete" Text="Delete"
                                        CommandName="Delete" />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Button class="bitbtndt btnsave" runat="server" ID="btnUpdate" Text="Save" CommandName="Update" />
                                    <asp:Button class="bitbtndt btncancel" runat="server" ID="btnCancel" Text="Cancel"
                                        CommandName="Cancel" />
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="FixedAsset" HeaderText="Fixed Asset Code" ItemStyle-Wrap="false">
                                <HeaderStyle Width="120px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="FixedAssetName" HeaderStyle-Width="300px" HeaderText="Fiexd Asset Name"
                                ItemStyle-Wrap="false">
                                <HeaderStyle Width="300px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Specification" HeaderStyle-Width="300px" HeaderText="Specification"
                                ItemStyle-Wrap="true">
                                <HeaderStyle Width="300px" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="Balance Life month" DataField="BalanceLife" DataFormatString="{0:#,##0.00}"
                                ItemStyle-HorizontalAlign="Right" ItemStyle-Wrap="false" HeaderStyle-Width="90px">
                                <HeaderStyle Width="90px" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="Balance Amount" DataField="BalanceAmount" DataFormatString="{0:#,##0.00}"
                                ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" ItemStyle-Wrap="false">
                                <HeaderStyle Width="90px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="AmountDepr" HeaderText="Depreciation Process" DataFormatString="{0:#,##0.00}"
                                ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" ItemStyle-Wrap="false" />
                            <asp:BoundField DataField="AdjustDepr" DataFormatString="{0:#,##0.00}" ItemStyle-HorizontalAlign="Right"
                                ItemStyle-Wrap="false" FooterStyle-HorizontalAlign="Right" HeaderText="Depreciation Adjust">
                                <HeaderStyle Width="90px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="TotalDepr" HeaderStyle-Width="90px" DataFormatString="{0:#,##0.00}"
                                ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" HeaderText="Depreciation Total" ItemStyle-Wrap="false">
                                <HeaderStyle Width="90px" />
                            </asp:BoundField>
                            <%--<asp:BoundField DataField="Reference" HeaderText="Reference" Visible="False" />
                        <asp:BoundField DataField="Year" HeaderText="Year" Visible="False" />
                        <asp:BoundField DataField="Period" HeaderText="Period" Visible="False" />--%>
                        </Columns>
                    </asp:GridView>
                </div>
            </asp:Panel>
            <asp:Panel runat="server" ID="pnlEditDt" Visible="false">
                <table>
                    <tr>
                        <td>
                            <asp:LinkButton ID="lbFixedAsset" runat="server" Text="Fixed Asset" />
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                            <asp:TextBox ID="tbFACode" runat="server" CssClass="TextBoxR" Enabled="False" />
                            <asp:TextBox ID="tbFAName" runat="server" CssClass="TextBoxR" Enabled="False" EnableTheming="True"
                                ReadOnly="True" Width="300px" />
                            <asp:Button class="btngo" runat="server" ID="btnAcc" Text="..." Visible="False" />
                        </td>
                        <td>
                            <asp:TextBox ID="tbfgSubled" runat="server" CssClass="TextBox" Visible="false" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Specification
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                            <asp:TextBox ID="tbSpecification" runat="server" CssClass="TextBoxR" Enabled="False"
                                EnableTheming="True" ReadOnly="True" TextMode="MultiLine" Width="429px" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Balance Life month
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                            <asp:TextBox CssClass="TextBoxR" runat="server" ID="tbLife" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Balance Amount
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                            <asp:TextBox ID="tbBalanceAmount" runat="server" CssClass="TextBoxR" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Depreciation Process
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                            <asp:TextBox ID="tbProcess" runat="server" CssClass="TextBoxR" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Depreciation Adjust
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                            <asp:TextBox ID="tbAdjust" runat="server" CssClass="TextBox" AutoPostBack="True" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Depreciation Total
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                            <asp:TextBox ID="tbTotal" runat="server" CssClass="TextBoxR" />
                        </td>
                    </tr>
                </table>
                <br />
                <asp:Button class="bitbtndt btnsave" runat="server" ID="btnSaveDt" Text="Save" />
                <asp:Button class="bitbtndt btncancel" runat="server" ID="btnCancelDt" Text="Cancel" />
            </asp:Panel>
            <br />
            <asp:Button class="bitbtndt btnsavenew" runat="server" ID="btnSaveAll" Text="Save & New"
                ValidationGroup="Input" Width="95px" />
            <asp:Button class="bitbtndt btnsave" runat="server" ID="btnSaveTrans" Text="Save"
                ValidationGroup="Input" />
            <asp:Button class="bitbtndt btncancel" runat="server" ID="btnBack" Text="Cancel"
                ValidationGroup="Input" />
            <asp:Button class="btngo" runat="server" ID="btnHome" Text="Home" Width="42px" />
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
