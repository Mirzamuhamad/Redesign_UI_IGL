<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TrWOPlanStatus.aspx.vb"
    Inherits="Transaction_TrWOPlanStatus_TrWOPlanStatus" EnableEventValidation="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="BasicFrame.WebControls.BasicDatePicker" Namespace="BasicFrame.WebControls"
    TagPrefix="BDP" %>
<%@ Register Assembly="CrystalDecisions.Web, Version=10.5.3700.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>WO Status Approval</title>

    <script src="../../Function/OpenDlg.js" type="text/javascript"></script>
    <script src="../../Function/Function.JS" type="text/javascript"></script>
    <script type="text/javascript" src="../../JS/jquerymodal/jquery.min.js"></script>

    <script src="../../JS/jquerymodal/jquery-ui.js" type="text/javascript"></script>
    <link href="../../JS/jquerymodal/jquery-ui.css" rel="stylesheet" type="text/css" />
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />



    <script type="text/javascript">
        window.parent.document.title = "WO Status Approval";
        
        function setdigit(nStr, digit) {
            try {
                var TNstr = parseFloat(nStr);
                if (parseFloat(digit) >= 0) {
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
            } catch (err) {
                alert(err.description);
            }
        }

        function setformat() {

            try {
                document.getElementById("tbOldPrice").value = setdigit(document.getElementById("tbOldPrice").value.replace(/\$|\,/g, ""), '<%=VIEWSTATE("DigitCurr")%>');
                document.getElementById("tbNewPrice").value = setdigit(document.getElementById("tbNewPrice").value.replace(/\$|\,/g, ""), '<%=VIEWSTATE("DigitCurr")%>');
                //        document.getElementById("tbAdjustPercent").value = setdigit(document.getElementById("tbAdjustPercent").value.replace(/\$|\,/g,""),'<%=VIEWSTATE("DigitCurr")%>');
            } catch (err) {
                alert(err.description);
            }
        }  
        
/*
        function closing()
        {
            try
            {
             
                var result = prompt("Remark Status Approval", "");
                if (result){
                    document.getElementById("HiddenRemarkClose").value = result;
                } else {
                    document.getElementById("HiddenRemarkClose").value = "False Value";
                }
                postback();
                //document.form1.submit();                
            }catch(err){
                  alert(err.description);
            }        
        }  */

        function postback()
        {
            __doPostBack('','');
        }

        
        
    </script>

    <style type="text/css">
        .style1
        {
            width: 392px;
            text-align: right;
        }
        .style3
        {
            width: 25px;
        }
        .style4
        {
            width: 101px;
        }
        .rataKanan
        {
        	text-align:right;
        }
        .style6
        {
            width: 59px;
        }
        .style7
        {
            width: 114px;
        }
    </style>
</head>
<body>

    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div class="Content">
        <div>
            <table>
                <tr>
                    <td>
                        <div class="H1">
                            WO Status Approval
                        <asp:Label runat="server" Font-Bold="True" 
                                ID="lblstatuslevel"></asp:Label>
                    &nbsp;</div>
                    </td>
                    <td class="style4">
                    </td>
                    <td>
                        <asp:Label runat="server" Text="Show Records:" Font-Bold="false"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlShowRecord" AutoPostBack="true">
                            <asp:ListItem Selected="True" Value="10">Choose One</asp:ListItem>
                            <asp:ListItem Value="30">30</asp:ListItem>
                            <asp:ListItem Value="40">40</asp:ListItem>
                            <asp:ListItem Value="50">50</asp:ListItem>
                            <asp:ListItem Value="100">100</asp:ListItem>
                            <asp:ListItem Value="250">250</asp:ListItem>
                            <asp:ListItem Value="500">500</asp:ListItem>
                            <asp:ListItem Value="1000">1000</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="Label1" runat="server" Text="Rows" Font-Bold="false"></asp:Label>
                    </td>
                    <td class="style1">
                       <%--                             <asp:ListItem>CSR</asp:ListItem>
                            <asp:ListItem>CSR Calmic</asp:ListItem>
                            <asp:ListItem>Install Report</asp:ListItem>
                            <asp:ListItem>Install Report Calmic</asp:ListItem>
                            <asp:ListItem>Remove Report</asp:ListItem>
                            <asp:ListItem>Remove Report Calmic</asp:ListItem>
                            <asp:ListItem>BAP</asp:ListItem>
                            <asp:ListItem>BAS</asp:ListItem>
                            <asp:ListItem>Tanda Terima</asp:ListItem>
                            <asp:ListItem>SPE</asp:ListItem>
                            <asp:ListItem>DPF</asp:ListItem>
                            <asp:ListItem>Suspend</asp:ListItem>
                        </asp:DropDownList>--%>
                        <%--<asp:ListItem>CSR</asp:ListItem>--%>
                       <%-- <button id="btnPrint" runat="server" class="bitbtn btnprint" accesskey="p">
                            <span style="text-decoration: underline;">P</span>rint</button>--%>
                    </td>
                </tr>
            </table>
        </div>
        <hr style="color: Blue" />
        <asp:Panel runat="server" ID="PnlHd">
            <table>
                <tr>
                    <td style="width: 70px; text-align: left">
                        Quick Search :
                    </td>
                    <td>
                        <asp:TextBox CssClass="TextBox" runat="server" ID="tbFilter" AccessKey="q" />
                        <cc1:TextBoxWatermarkExtender ID="tbFilter_WtExt" runat="server" Enabled="True" TargetControlID="tbFilter"
                            WatermarkText="alt+q" WatermarkCssClass="Watermarked">
                        </cc1:TextBoxWatermarkExtender>
                        <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlField">
                             <asp:ListItem Value="TransNmbr" Selected="True">WO No</asp:ListItem>
                                <asp:ListItem Value="dbo.FormatDate(TransDate)">WO Date</asp:ListItem>
                                <asp:ListItem Value="ReffType">Reff Type</asp:ListItem>
                                <asp:ListItem Value="Type">Type</asp:ListItem>
                                <asp:ListItem Value="JobPlant">Job Plant</asp:ListItem>
                                <asp:ListItem Value="JobPlantName">Job Plant Name</asp:ListItem>
                                <asp:ListItem Value="Capacity">Capacity</asp:ListItem>
                                <asp:ListItem Value="ST">ST</asp:ListItem>
                                <asp:ListItem Value="QtyBlok">Qty Blok</asp:ListItem>
                                <asp:ListItem Value="DivisiName">Division Name</asp:ListItem>
                                <asp:ListItem Value="BlockName">Block Name</asp:ListItem>
                                <asp:ListItem Value="Luas">Luas</asp:ListItem>
                                <asp:ListItem Value="Qty">Qty</asp:ListItem>
                                <asp:ListItem Value="Unit">Unit</asp:ListItem>
                                <asp:ListItem Value="HK">HK</asp:ListItem>
                                <asp:ListItem Value="WorkBy">Work By</asp:ListItem>
                                <asp:ListItem Value="StatusWrhs">Status Warehouse</asp:ListItem>
                                <asp:ListItem Value="StatusAsisten">Status Asisten</asp:ListItem>
                                <asp:ListItem Value="StatusManager">Status Manager</asp:ListItem>
                        </asp:DropDownList>
                        <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlRange">
                        </asp:DropDownList>
                        <%--<asp:Button class="bitbtn btnsearch" runat="server" ID="btnSearch" Text="Search" />--%>
                        <%--<asp:Button class="btngo" runat="server" ID="btnExpand" Text="..." />--%>
                        <button id="btnSearch" runat="server" class="bitbtn btnsearch" accesskey="s">
                            <span style="text-decoration: underline;">S</span>earch</button>
                        <button id="btnExpand" runat="server" class="btngo" accesskey=".">
                            <span style="text-decoration: underline;">...</span></button>
                    </td>
                    <td>
                        <asp:LinkButton ID="LbAdvSearch" runat="server" Text="Advanced Search" />
                    </td>
                    <td class="style3">
                        &nbsp;</td>
                    <td>
                        <asp:Panel runat="server" ID="pnlChangeTeam" BackColor="LightGray" Visible="false">
                            <table>
                                <tr>
                                    <td>
                                        Wrhs Src</td>
                                    <td>
                                        :
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlwrhsSrc" runat="server" CssClass="DropDownList" 
                                            Enabled="true">
                                        </asp:DropDownList>
                                        
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Wrhs Dest
                                    </td>
                                    <td>
                                        :
                                    </td>
                                    <td>
                                     <asp:DropDownList ID="ddlWrhsDest"  runat="server" CssClass="DropDownList" 
                                            Enabled="true">
                                        </asp:DropDownList>
                                        </td>
                                </tr>
                            </table>
                        </asp:Panel>
                       
                    </td>
                    <td>
                        <asp:Panel runat="server" ID="pnlWrhs" BackColor="LightGray" Visible="false">
                            <table>

                                
                                <%--<asp:Button ID="btnWOLevel" onclick="btnWOLevel_click" OnclientClick="closing();"  runat="server" Class="btngo" Text="" Width="139px"
                            ValidationGroup="Input" />--%>

                            <asp:Button ID="btnWOLevel"  runat="server" Class="btngo" Text="" Width="139px"
                            ValidationGroup="Input" />

                            
                    
                            </table>
                          
                            
                        </asp:Panel> 


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
                                <asp:ListItem Value="TransNmbr" Selected="True">WO No</asp:ListItem>
                                <asp:ListItem Value="dbo.FormatDate(TransDate)">WO Date</asp:ListItem>
                                <asp:ListItem Value="ReffType">Reff Type</asp:ListItem>
                                <asp:ListItem Value="Type">Type</asp:ListItem>
                                <asp:ListItem Value="JobPlant">Job Plant</asp:ListItem>
                                <asp:ListItem Value="JobPlantName">Job Plant Name</asp:ListItem>
                                <asp:ListItem Value="Capacity">Capacity</asp:ListItem>
                                <asp:ListItem Value="ST">ST</asp:ListItem>
                                <asp:ListItem Value="QtyBlok">Qty Blok</asp:ListItem>
                                <asp:ListItem Value="DivisiName">Division Name</asp:ListItem>
                                <asp:ListItem Value="BlockName">Block Name</asp:ListItem>
                                <asp:ListItem Value="Luas">Luas</asp:ListItem>
                                <asp:ListItem Value="Qty">Qty</asp:ListItem>
                                <asp:ListItem Value="Unit">Unit</asp:ListItem>
                                <asp:ListItem Value="HK">HK</asp:ListItem>
                                <asp:ListItem Value="WorkBy">Work By</asp:ListItem>
                                <asp:ListItem Value="StatusWrhs">Status Warehouse</asp:ListItem>
                                <asp:ListItem Value="StatusAsisten">Status Asisten</asp:ListItem>
                                <asp:ListItem Value="StatusManager">Status Manager</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <br />

            <asp:label ID="lbremarkApprov" runat="server"  CssClass="TextBox"  Text ="Remark : " Height="19px" />
            &nbsp;
            <asp:TextBox ID="TbremarkApprov" runat="server"  CssClass="TextBox" Height="19px" Width="480px" />
           <br><br>
            <div style="border: 0px  solid; width: 100%; height: 100%; overflow: auto;">
                <asp:GridView ID="GridViewHd" runat="server" AllowPaging="True" AllowSorting="True"
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
                                <asp:CheckBox ID="cbSelect" runat="server" AutoPostBack="true" OnCheckedChanged="cbSelect_CheckedChanged"/>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <%--<asp:TemplateField HeaderStyle-Width="110">
                            <ItemTemplate>
                                <asp:DropDownList CssClass="DropDownList" ID="ddl" runat="server">
                                    <asp:ListItem Selected="True" Text="View" />
                                    <asp:ListItem Text="Edit" />
                                    <asp:ListItem Text="Print" />
                                </asp:DropDownList>
                                <asp:Button class="btngo" runat="server" ID="BtnGo" Text="G" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>"
                                    CommandName="Go" />
                            </ItemTemplate>
                            <HeaderStyle Width="110px" />
                        </asp:TemplateField>--%>
                        <%--<asp:TemplateField HeaderText="Action">
                            <ItemTemplate>
                                <asp:DropDownList CssClass="DropDownList" ID="ddl" runat="server">
                                    <asp:ListItem Selected="True" Text="Detail" />
                                    <asp:ListItem Text="Cancel" />
                                </asp:DropDownList>
                                <asp:Button class="btngo" runat="server" ID="BtnGo" Text="G" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" CommandName="Go" OnClientClick = "return confirm('Are you sure you want to delete?');"/>
                            </ItemTemplate>
                        </asp:TemplateField>--%>
                        <asp:TemplateField HeaderText="Action">
                            <ItemTemplate>
                                    <asp:DropDownList CssClass="DropDownList" ID="ddl" runat="server">
                                    <asp:ListItem Selected="True" Text="Detail" />
                               </asp:DropDownList>
                                <asp:Button class="btngo" runat="server" ID="BtnGo" Text="G" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>"
                                    CommandName="Go" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="TransNmbr" HeaderText="Trans Nmbr" SortExpression="TransNmbr" />
                        <asp:BoundField DataField="TransDate" DataFormatString="{0:dd MMM yyyy}" HtmlEncode="true"
                            HeaderText="Trans Date" SortExpression="TransDate" />
                        <asp:BoundField DataField="ReffType" HeaderText="ReffType" SortExpression="ReffType">
                        </asp:BoundField>
                        <asp:BoundField DataField="JobPlant" HeaderText="Job Plant" SortExpression="JobPlant" />
                        <asp:BoundField DataField="Capacity" HeaderText="Capacity" SortExpression="Capacity" />
                        <asp:BoundField DataField="ST" HeaderText="ST" SortExpression="ST" />
                        <asp:BoundField DataField="QtyBlok" HeaderText="Qty Blok" SortExpression="QtyBlok" DataFormatString="{0:#,##0.00}" ItemStyle-HorizontalAlign="Right"/>
                        <asp:BoundField DataField="DivisiName" HeaderText="Divisi Name" SortExpression="DivisiName" />
                        <asp:BoundField DataField="BlockName" HeaderText="Block Name" SortExpression="BlockName" />
                       <asp:BoundField DataField="Luas" HeaderText="Luas" SortExpression="Luas" DataFormatString="{0:#,##0.00}" ItemStyle-HorizontalAlign="Right">
                        </asp:BoundField>
                        <asp:BoundField DataField="Qty" HeaderText="Qty" SortExpression="Qty" DataFormatString="{0:#,##0.00}" ItemStyle-HorizontalAlign="Right"/>
                        <asp:BoundField DataField="Unit" HeaderText="Unit" SortExpression="Unit" />
                        <asp:BoundField DataField="HK" HeaderText="HK" SortExpression="HK" DataFormatString="{0:#,##0.00}" ItemStyle-HorizontalAlign="Right"/>
                        <asp:BoundField DataField="WorkBy" HeaderText="Work By" SortExpression="WorkBy" />
                        <asp:BoundField DataField="StatusWrhs" HeaderText="Status Wrhs" SortExpression="StatusWrhs" />
                        <asp:BoundField DataField="StatusAsisten" HeaderText="Status Asisten" SortExpression="StatusAsisten" />
                       <asp:BoundField DataField="StatusManager" HeaderText="Status Manager" SortExpression="StatusManager" />
                       
                       <%-- <asp:TemplateField HeaderText="Team" Visible="true">
                            <ItemTemplate>
                                <asp:Label runat="server" ID="Team" Text='<%# DataBinder.Eval(Container.DataItem, "Team") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>--%>
                    </Columns>
                </asp:GridView>
            </div>
        </asp:Panel>
        <asp:Panel runat="server" ID="pnlDetail" Visible="false">
            <button id="btnBack" runat="server" class="bitbtndt btncancel" accesskey="b">
                <span style="text-decoration: underline;">B</span>ack</button>
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <table ><tr>
            <td><asp:Label ID="Label2" runat="server" Text="WO No" Font-Bold="true" Font-Size="Medium"></asp:Label></td>
            <td>:</td>
            <td><asp:Label ID="lblWono" runat="server" Text="" Font-Bold="true" Font-Size="Medium"></asp:Label></td>
            
            <td><asp:Label ID="Label3" runat="server" Text="WO Date" Font-Bold="true" Font-Size="Medium"></asp:Label></td>
            <td>:</td>
            <td class="style7"><asp:Label ID="lblWODate" runat="server" Text="" Font-Bold="true" Font-Size="Medium"></asp:Label></td>
            <td></td>
            <td></td>
            <td class="style6"></td>
                <td rowspan="3">
                    <asp:Panel runat="server" ID="pnlwrhs1" BackColor="LightGray" Visible="false">
                            <table>
                                <tr>
                                    <td>
                                        Wrhs Src</td>
                                    <td>
                                        :
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlwrhs" runat="server" CssClass="DropDownList" 
                                            Enabled="true">
                                        </asp:DropDownList>
                                        
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Wrhs Dest
                                    </td>
                                    <td>
                                        :
                                    </td>
                                    <td>
                                     <asp:DropDownList ID="ddlwrhsdest2"  runat="server" CssClass="DropDownList" 
                                            Enabled="true">
                                        </asp:DropDownList>
                                        </td>
                                </tr>
                            </table>
                        </asp:Panel></td>
            </tr><tr>
            <td><asp:Label ID="Label4" runat="server" Text="Work By" Font-Bold="true" Font-Size="Medium"></asp:Label></td>
            <td>:</td>
            <td><asp:Label ID="lblWorkBy" runat="server" Text="" Font-Bold="true" Font-Size="Medium"></asp:Label></td>
            
            <td><asp:Label ID="Label6" runat="server" Text="Reference" Font-Bold="true" Font-Size="Medium"></asp:Label></td>
            <td>:</td>
            <td class="style7"><asp:Label ID="lblReff" runat="server" Text="" Font-Bold="true" Font-Size="Medium"></asp:Label>
               
                    </td>
                    <td colspan ="2"> <asp:Button ID="btnpost" runat="server" Class="btngo" Text="" width="135px"
                    ValidationGroup="Input" /></td>
                    <td class="style6"> </td>
            </tr><tr>
            <td><asp:Label ID="Label5" runat="server" Text="Job Plant" Font-Bold="true" Font-Size="Medium"></asp:Label></td>
            <td>:</td>
            <td><asp:Label ID="lblJobPlant" runat="server" Text="" Font-Bold="true" Font-Size="Medium"></asp:Label></asp:Label></td>
            
            <td><asp:Label ID="Label9" runat="server" Text="Qty/Unit" Font-Bold="true" Font-Size="Medium"></asp:Label></td>
            <td>:</td>
            <td class="style7"><asp:Label ID="lblQty" runat="server" Text="" Font-Bold="true" Font-Size="Medium"></asp:Label>
            <asp:Label ID="lblUnit" runat="server" Text="" Font-Bold="true" Font-Size="Medium"></asp:Label></td>
            <td><asp:Label ID="lblStatusW" runat="server" Text="" Font-Bold="false" Font-Size="Medium" Enabled="false"></asp:Label></td>
            <td><asp:Label ID="lblStatusA" runat="server" Font-Bold="False" Font-Size="Medium" 
                    Enabled="False" Visible="True"></asp:Label></td>
            <td class="style6"><asp:Label ID="lblStatusN" runat="server" Font-Bold="False" Font-Size="Medium" 
                    Enabled="False" Visible="True"></asp:Label></td>
            </tr>
            
            </table>
            <asp:label ID="lbremarkApprovDt" Font-Bold="true" Font-Size="Medium" runat="server"  CssClass="TextBox"  Text ="Remark Approval :" Height="19px"  />
            &nbsp;
            <asp:TextBox ID="TbremarkApprovDt" runat="server"  CssClass="TextBox" Width="427px" />
          <br>
            <br />
            <div style="border: 0px  solid; width: 100%; height: 100%; overflow: auto;">
                <div style="border-style: solid; border-color: inherit; border-width: 0px; width: 100%; height: 185px; overflow: auto;">
                    <asp:GridView ID="GridDt" runat="server" AllowPaging="false" AllowSorting="True"
                        CssClass="Grid" AutoGenerateColumns="False">
                        <HeaderStyle CssClass="GridHeader" Wrap="false"></HeaderStyle>
                        <RowStyle CssClass="GridItem" Wrap="false" />
                        <AlternatingRowStyle CssClass="GridAltItem" />
                        <PagerStyle CssClass="GridPager" />
                        <Columns>
                        <asp:BoundField DataField="StatusLevel" HeaderText="" SortExpression="" />
                        <asp:BoundField DataField="StatusLevel" HeaderText="Status Level" SortExpression="StatusLevel" />
                        <asp:BoundField DataField="WOName" HeaderText="Status Level Name" SortExpression="WOName">
                        </asp:BoundField>
                        
                        <asp:BoundField DataField="DateApproval" DataFormatString="{0:dd MMM yyyy}" HtmlEncode="true"
                            HeaderText="Date Approval" SortExpression="DateApproval" />
                        <asp:BoundField DataField="UserApproval" HeaderText="User Approval" SortExpression="UserApproval" />
                        <asp:BoundField DataField="Remark" HeaderText="Remark" SortExpression="Remark" />
                        <asp:BoundField DataField="UserDate" DataFormatString="{0:dd MMM yyyy}" HtmlEncode="true" HeaderText="User Date" SortExpression="UserDate"/>
                        <asp:BoundField DataField="UserId" HeaderText="User Id" SortExpression="UserId" />
                        <asp:BoundField DataField="WrhsSrcName" HeaderText="Wrhs Source" SortExpression="WrhsSrcName" />
                        <asp:BoundField DataField="WrhsDestName" HeaderText="Wrhs Destination" SortExpression="WrhsDestName" />
                        
                        </Columns>
                    </asp:GridView>
                </div>
                <br />
                <asp:Label ID="lblMI" runat="server" Text="Detail Blok" Font-Bold="true" Font-Size="Medium"></asp:Label>
                <div style="border-style: solid; border-color: inherit; border-width: 0px; width: 100%; height: 193px; overflow: auto;">
                    <asp:GridView ID="GridDtMI" runat="server" AllowPaging="false" AllowSorting="True"
                        CssClass="Grid" AutoGenerateColumns="False">
                        <HeaderStyle CssClass="GridHeader" Wrap="false"></HeaderStyle>
                        <RowStyle CssClass="GridItem" Wrap="false" />
                        <AlternatingRowStyle CssClass="GridAltItem" />
                        <PagerStyle CssClass="GridPager" />
                        <Columns>
                        <asp:BoundField DataField="Type" HeaderText="Type" SortExpression="Type" />
                            <asp:BoundField DataField="DivisiBlok" HeaderText="DivisiBlok" SortExpression="DivisiBlok"></asp:BoundField>
                            <asp:BoundField DataField="DivisiBlokName" HeaderText="DivisiBlok Name" SortExpression="DivisiBlokName"></asp:BoundField>
                            <asp:BoundField DataField="StartDate" HeaderText="Start Date" SortExpression="StartDate" DataFormatString="{0:dd MMM yyyy}" HtmlEncode="true" />
                            <asp:BoundField DataField="EndDate" HeaderText="End Date" SortExpression="EndDate" DataFormatString="{0:dd MMM yyyy}" HtmlEncode="true" />
                            <asp:BoundField DataField="Qty" HeaderText="Qty" SortExpression="Qty" DataFormatString="{0:#,##0.00}" ItemStyle-HorizontalAlign="Right" >
                            </asp:BoundField>
                            <asp:BoundField DataField="NormaHK" HeaderText="Capacity HK" SortExpression="NormaHK" DataFormatString="{0:#,##0.00}" ItemStyle-HorizontalAlign="Right"/>
                            
                            
                           <%-- <asp:TemplateField HeaderText="Qty Convert" ItemStyle-HorizontalAlign="Right">
                                <ItemTemplate>
                                    <asp:Label ID="lblQtyConvertView" runat="server" Visible="true" CssClass="rataKanan" />
                                    <asp:Label ID="lblQtyConvert" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "QtyConvert") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>--%>
                        </Columns>
                    </asp:GridView>
                </div>
                <br />
                <asp:Label ID="lblSPE" runat="server" Text="Detail Material" Font-Bold="true" Font-Size="Medium"></asp:Label>
                <div style="border: 0px  solid; width: 100%; height: 300px; overflow: auto;">
                    <asp:GridView ID="GridDtSPE" runat="server" AllowPaging="false" AllowSorting="True"
                        CssClass="Grid" AutoGenerateColumns="False">
                        <HeaderStyle CssClass="GridHeader" Wrap="false"></HeaderStyle>
                        <RowStyle CssClass="GridItem" Wrap="false" />
                        <AlternatingRowStyle CssClass="GridAltItem" />
                        <PagerStyle CssClass="GridPager" />
                        <Columns>
                            <asp:BoundField DataField="Material" HeaderText="Material" SortExpression="Material" />
                            <asp:BoundField DataField="MaterialName" HeaderText="MaterialName" SortExpression="MaterialName"></asp:BoundField>
                            <asp:BoundField DataField="Specification" HeaderText="Specification" SortExpression="Specification"></asp:BoundField>
                            <asp:BoundField DataField="Qty" HeaderText="Qty" SortExpression="Qty" DataFormatString="{0:#,##0.00}" ItemStyle-HorizontalAlign="Right"  />
                            <asp:BoundField DataField="Unit" HeaderText="Unit" SortExpression="Unit" />
                            <asp:BoundField DataField="QtyOH" HeaderText="Qty On Hand RR" SortExpression="QtyOH" DataFormatString="{0:#,##0.00}" ItemStyle-HorizontalAlign="Right">
                            </asp:BoundField>
                            <asp:BoundField DataField="QtyOHWO" HeaderText="Qty On Hand WO" SortExpression="QtyOHWO" DataFormatString="{0:#,##0.00}" ItemStyle-HorizontalAlign="Right" />
                            <%--
                            <asp:TemplateField HeaderText="Qty Convert" ItemStyle-HorizontalAlign="Right">
                                <ItemTemplate>
                                    <asp:Label ID="lblQtyConvertView" runat="server" Visible="true" CssClass="rataKanan" />
                                    <asp:Label ID="lblQtyConvert" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "QtyConvert") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>--%>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </asp:Panel>
    </div>
    <asp:Label runat="server" ID="lbStatus" ForeColor="Red" />
    <asp:HiddenField ID="HiddenRemarkClose" runat="server" />
    </form>
</body>
</html>
