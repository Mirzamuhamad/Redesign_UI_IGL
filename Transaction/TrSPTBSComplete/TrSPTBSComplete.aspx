<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TrSPTBSComplete.aspx.vb"
    Inherits="TrSPTBSComplete" EnableEventValidation="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="BasicFrame.WebControls.BasicDatePicker" Namespace="BasicFrame.WebControls"
    TagPrefix="BDP" %>
<%@ Register Assembly="CrystalDecisions.Web, Version=10.5.3700.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
<%@ Register Assembly="DevExpress.Web.ASPxGridView.v10.2, Version=10.2.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxGridView" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v10.2" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>S P T B S Complete</title>

    <script src="../../Function/OpenDlg.js" type="text/javascript"></script>
    <script src="../../Function/Function.JS" type="text/javascript"></script>
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript" src="../../JS/jquerymodal/jquery.min.js"></script>
    <script src="../../JS/jquerymodal/jquery-ui.js" type="text/javascript"></script>
    <link href="../../JS/jquerymodal/jquery-ui.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript">
        window.parent.document.title = "S P T B S Complete";
        
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
    </script>

    </head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div class="Content">
        <div class="H1">
            <table>
                <tr>
                    <td>
                        S P T B S Complete</td>
                    <td style="width: 700">
                        <asp:LinkButton runat="server" ID="btnSPTBS" Text="Goto S P T B S"
                            Width="563px" Style="text-align: right"></asp:LinkButton>
                    </td>
                </tr>
            </table>
        </div>
        <hr style="color: Blue" />
        						
								<table>
                                    <tr>
                                        <td style="width: 70px; text-align: left">
                                            Quick Search :
                                        </td>
                                        <td>
                                            <asp:TextBox CssClass="TextBox" runat="server" ID="tbFilterP" AccessKey="q" />
                                            <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender1" runat="server" Enabled="True"
                                                TargetControlID="tbFilterP" WatermarkText="alt+q" WatermarkCssClass="Watermarked">
                                            </cc1:TextBoxWatermarkExtender>
                                            <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlFieldP">
                                                <asp:ListItem Value="SPTBSNo" Selected="True">SPTBS No</asp:ListItem>
                                                <asp:ListItem Value="dbo.FormatDate(DateAngkut)">Date Angkut</asp:ListItem>
                                                <asp:ListItem Value="CarNo">Car No</asp:ListItem>
                                                <asp:ListItem Value="Normal">SPTBS Normal</asp:ListItem>
                                                <asp:ListItem Value="Abnormal">SPTBS Abnormal</asp:ListItem>
                                                <asp:ListItem Value="Brondolan">SPTBS Brondolan</asp:ListItem>
                                                <asp:ListItem Value="ActNormal">TPH Result Normal</asp:ListItem>
                                                <asp:ListItem Value="ActAbnormal">TPH Result Abnormal</asp:ListItem>
                                                <asp:ListItem Value="ActBrondolan">TPH Result Brondolan</asp:ListItem>
                                                <asp:ListItem Value="DiffNormal">Difference Normal</asp:ListItem>
                                                <asp:ListItem Value="DiffAbNormal">Difference AbNormal</asp:ListItem>
                                                <asp:ListItem Value="DiffBrondolan">Difference Brondolan</asp:ListItem>
                                                <asp:ListItem Value="Remark">Remark</asp:ListItem>
                                            </asp:DropDownList>
                                            <%--<asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlRangeP">
                                            </asp:DropDownList>--%>
                                            <button id="btnSearchP" runat="server" class="bitbtn btnsearch" accesskey="s">
                                                <span style="text-decoration: underline;">S</span>earch</button>
                                            <button id="btnExpandP" runat="server" class="btngo" accesskey=".">
                                                <span style="text-decoration: underline;">...</span></button>
                                        </td>
										
										<td>
											<asp:Label ID="Label1" runat="server" Text="Show Records:" Font-Bold="false"></asp:Label>
										</td>

										<td>
											<asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlShowRecord" AutoPostBack="true">
												<asp:ListItem Selected="True" Value="10">10</asp:ListItem>
												<asp:ListItem Value="100">100</asp:ListItem>
												<asp:ListItem Value="500">500</asp:ListItem>
												<asp:ListItem Value="1000">1000</asp:ListItem>
											</asp:DropDownList>
										</td>
										
										<td>
											<asp:Label ID="Label2" runat="server" Text="Rows" Font-Bold="false"></asp:Label>
										</td>
										
                                    </tr>
									
                                    <tr>
                                        <td colspan="2">
                                            <asp:Panel runat="server" ID="pnlSearchP" Visible="false">
                                                <table>
                                                    <tr>
                                                        <td style="width: 100px; text-align: right">
                                                            <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlNotasiP">
                                                                <asp:ListItem Selected="true" Text="OR" Value="OR"></asp:ListItem>
                                                                <asp:ListItem Text="AND" Value="AND"></asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox runat="server" CssClass="TextBox" ID="tbfilter2P" />
                                                            <asp:DropDownList runat="server" CssClass="DropDownList" ID="ddlField2P">
                                                                <asp:ListItem Value="SPTBSNo" Selected="True">SPTBS No</asp:ListItem>
                                                                <asp:ListItem Value="dbo.FormatDate(DateAngkut)">Date Angkut</asp:ListItem>
                                                                <asp:ListItem Value="CarNo">Car No</asp:ListItem>
                                                                <asp:ListItem Value="Normal">SPTBS Normal</asp:ListItem>
                                                                <asp:ListItem Value="Abnormal">SPTBS Abnormal</asp:ListItem>
                                                                <asp:ListItem Value="Brondolan">SPTBS Brondolan</asp:ListItem>
                                                                <asp:ListItem Value="ActNormal">TPH Result Normal</asp:ListItem>
                                                                <asp:ListItem Value="ActAbnormal">TPH Result Abnormal</asp:ListItem>
                                                                <asp:ListItem Value="ActBrondolan">TPH Result Brondolan</asp:ListItem>
                                                                <asp:ListItem Value="DiffNormal">Difference Normal</asp:ListItem>
                                                                <asp:ListItem Value="DiffAbNormal">Difference AbNormal</asp:ListItem>
                                                                <asp:ListItem Value="DiffBrondolan">Difference Brondolan</asp:ListItem>
                                                                <asp:ListItem Value="Remark">Remark</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                        </td>
                                    </tr>
									
                                </table>
						
						
                            
                                <table>
                                    <tr>
                                        
                                         <td style="width: 40px; text-align: Left">Period :
                                         </td>                     
                                        <td>
                                          <BDP:BasicDatePicker ID="tbSDate" runat="server" DateFormat="dd MMM yyyy" 
                                            ReadOnly = "true" ValidationGroup="Input"
                                            ButtonImageHeight="19px" ButtonImageWidth="20px" 
                                            DisplayType="TextBoxAndImage" 
                                            TextBoxStyle-CssClass="TextDate" AutoPostBack="True" 
                                        ShowNoneButton="False"><TextBoxStyle CssClass="TextDate" /></BDP:BasicDatePicker> 
                                        </td style="width: 35px; text-align: left">
                                         <td>as 
                                         </td> 
                                        <td>
                                        </BDP:BasicDatePicker> 
                                                            <BDP:BasicDatePicker ID="tbEDate" runat="server" DateFormat="dd MMM yyyy" 
                                            ReadOnly = "true" ValidationGroup="Input"
                                            ButtonImageHeight="19px" ButtonImageWidth="20px" 
                                            DisplayType="TextBoxAndImage" 
                                            TextBoxStyle-CssClass="TextDate" AutoPostBack="True" 
                                        ShowNoneButton="False"><TextBoxStyle CssClass="TextDate" /></BDP:BasicDatePicker> 
                                        </td>
                                        <td style="width: 45px; text-alignDivision : right">
                                            Division :
                                        </td>
                                        <td>
                                            <asp:DropDownList CssClass="DropDownList" runat="server" ID="ddlField" AutoPostBack="true">
                                            </asp:DropDownList>
                                        </td>
										
                                        <td>
                                            <%--<asp:Button class="bitbtn btngo" runat="server" ID="btnProcess" Text="Process" Width="69px" />--%>
                                            <button id="btnProcess" runat="server" class="bitbtn btnadd" Text="Complete" accesskey="p" Width="69px" >
                                                <span style="text-decoration: underline;">C</span>omplete</button>
												
                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
											
                                            <button id="btnUnProcess" runat="server" class="bitbtn btnadd" 
                                                Text="Un Complete" accesskey="u" Width="200px" >
                                                <span style="text-decoration: underline;">U</span>nComplete</button>     
                                        </td>
                                    </tr>
                                </table>
		<br/>
								
        <asp:Menu ID="Menu1" runat="server" CssClass="Menu" ItemWrap="True" Orientation="Horizontal"
            StaticEnableDefaultPopOutImage="False" StaticMenuItemStyle-CssClass="MenuItem"
            StaticSelectedStyle-CssClass="MenuSelect">
            <StaticSelectedStyle CssClass="MenuSelect" />
            <StaticMenuItemStyle CssClass="MenuItem" />
            <Items>
                <asp:MenuItem Text="Complete" Value="0"></asp:MenuItem>
                <asp:MenuItem Text="UnComplete" Value="1"></asp:MenuItem>
            </Items>
        </asp:Menu>
        <hr/>
        
        <asp:MultiView ID="MV1" runat="server" ActiveViewIndex="0">
        
            <asp:View ID="TabProcess" runat="server">
                <asp:Panel ID="pnlProcess" runat="server">
                    <table>
                                        <tr style="background-color: Silver; text-align: center">
                                            <td>
                                                SPTBS Normal</td>
                                            <td>
                                                SPTBS Ab Normal</td>
                                            <td>
                                                SPTBS Brd(krg)</td>
                                            
                                            <td>
                                                TPH Normal</td>
                                            <td>
                                                TPH Ab Normal</td>
                                            <td>
                                                TPH Brd(krg)</td>
                                            <td>
                                                Diff Normal</td>
                                            
                                            <td>
                                                Diff Ab Normal</td>   
                                            <td>
                                                Diff Brd(krg)</td>   
                                                 <td>
                                                </td>    
                                           
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="tbNormal" runat="server" CssClass="TextBoxR" Enabled="false" Width="65px" text="0" />
                                            </td>
                                            <td>
                                                <asp:TextBox ID="tbAbNormal" runat="server" CssClass="TextBoxR" Enabled="false" Width="80px" text="0" />
                                            </td>
                                            <td>
                                                <asp:TextBox ID="tbBrd" runat="server" CssClass="TextBoxR" Enabled="false" Width="65px" text="0"/>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="tbNormal2" runat="server" CssClass="TextBoxR" Enabled="false" text="0"
                                                    Width="70px" />
                                            </td>
                                            <td>
                                                <asp:TextBox ID="tbAbNormal2" runat="server" CssClass="TextBoxR" Enabled="false"  Width="70px" text="0"/>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="tbBrd2" runat="server" CssClass="TextBoxR" Enabled="false"  Width="65px" text="0"/>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="tbNormal3" runat="server" CssClass="TextBoxR" Enabled="false"  Width="65px" text="0"/>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="tbAbNormal3" runat="server" CssClass="TextBoxR"  Enabled="false"  text="0"
                                                    Width="70px" />
                                            </td>
                                            <td>
                                                <asp:TextBox ID="tbBrd3" runat="server" CssClass="TextBoxR"  Enabled="false"  text="0"
                                                    Width="70px" />
                                            </td>
                                            <td>
                                                <asp:Button class="bitbtn btngo" width ="70px" runat="server" ID="btnTotalComplete" Text="Total Janjang" />
                                            </td>
                                           
                                            
                                        </tr>
                                    </table>
                    <asp:Panel runat="server" ID="pnlInput">
                        <br />
                        <asp:Panel runat="server" ID="PanelDev">
                            <div style="border: 0px  solid; width: 100%; height: 100%; overflow: auto;">
                                <asp:GridView ID="gvProcess" runat="server" AllowPaging="True" AllowSorting="True"
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
                                        <asp:BoundField DataField="SPTBSNo" HeaderText="SPTBS No" SortExpression="RequestNo" />
                                        <asp:BoundField DataField="DateAngkut" DataFormatString="{0:dd MMM yyyy}" HtmlEncode="true"
                                            HeaderText="Date Angkut" SortExpression="DateAngkut" />
                                        <asp:BoundField DataField="CarNo" HeaderText="Car No" SortExpression="CarNo">
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderText="SPTBS Normal"  >
                                            <ItemTemplate>
                                                <asp:TextBox ID="tbNormalC" CssClass="TextBoxR" style="text-align: right"  Width="50px" runat="server" Enabled="false" text='<%# DataBinder.Eval(Container.DataItem, "Normal") %>'/>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="SPTBS Abnormal" >
                                            <ItemTemplate>
                                                <asp:TextBox ID="tbAbNormalC" CssClass="TextBoxR" style="text-align: right"  Width="50px" Enabled="false" runat="server" text='<%# DataBinder.Eval(Container.DataItem, "Abnormal") %>'/>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="SPTBS Brondolan">
                                            <ItemTemplate>
                                                <asp:TextBox ID="tbBrondolanC" CssClass="TextBoxR" style="text-align: right" Width="50px" Enabled="false" runat="server" text='<%# DataBinder.Eval(Container.DataItem, "Brondolan") %>'/>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="TPH Normal">
                                            <ItemTemplate>
                                                <asp:TextBox ID="tbActNormalC" CssClass="TextBox" style="text-align: right" Width="50px" runat="server" text='<%# DataBinder.Eval(Container.DataItem, "ActNormal") %>'/>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="TPH Abnormal">
                                            <ItemTemplate>
                                                <asp:TextBox ID="tbActAbnormalC" CssClass="TextBox" style="text-align: right" Width="50px" runat="server" text='<%# DataBinder.Eval(Container.DataItem, "ActAbnormal") %>'/>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="TPH Brondolan">
                                            <ItemTemplate>
                                                <asp:TextBox ID="tbActBrondolanC" CssClass="TextBox" style="text-align: right" Width="50px" runat="server" text='<%# DataBinder.Eval(Container.DataItem, "ActBrondolan") %>'/>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                       
                                        <asp:TemplateField HeaderText="Diff Normal">
                                            <ItemTemplate>
                                                <asp:TextBox ID="tbDiffNormalC" CssClass="TextBoxR" style="text-align: right" Width="50px" Enabled="false" runat="server" text='<%# DataBinder.Eval(Container.DataItem, "DiffNormal") %>'/>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Diff AbNormal">
                                            <ItemTemplate>
                                                <asp:TextBox ID="tbDiffAbNormalC" CssClass="TextBoxR" style="text-align: right" Width="50px" Enabled="false" runat="server" text='<%# DataBinder.Eval(Container.DataItem, "DiffAbNormal") %>'/>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Diff Brondolan">
                                            <ItemTemplate>
                                                <asp:TextBox ID="tbDiffBrondolanC" CssClass="TextBoxR" style="text-align: right" Width="50px" Enabled="false" runat="server" text='<%# DataBinder.Eval(Container.DataItem, "DiffBrondolan") %>'/>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Remark">
                                            <ItemTemplate>
                                                <asp:TextBox ID="tbRemarkC" CssClass="TextBox" style="text-align: right" Width="100px" runat="server" text='<%# DataBinder.Eval(Container.DataItem, "Remark") %>'/>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                                                                
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </asp:Panel>
                        <br />
                        <asp:Label runat="server" ID="lbStatusProcess" ForeColor="Red" />
                    </asp:Panel>
                </asp:Panel>
            </asp:View>
            <asp:View ID="TabUnProcess" runat="server">
            
                <asp:Panel ID="pnlUnProcess" runat="server">
                    <table>
                                        <tr style="background-color: Silver; text-align: center">
                                            <td>
                                                SPTBS Normal</td>
                                            <td>
                                                SPTBS Ab Normal</td>
                                            <td>
                                                SPTBS Brd(krg)</td>
                                            
                                            <td>
                                                TPH Normal</td>
                                            <td>
                                                TPH Ab Normal</td>
                                            <td>
                                                TPH Brd(krg)</td>
                                            <td>
                                                Diff Normal</td>
                                            
                                            <td>
                                                Diff Ab Normal</td>   
                                            <td>
                                                Diff Brd(krg)</td>  
                                                <td>
                                                </td>   
                                           
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="tbUnNormal" runat="server" Enabled="false" CssClass="TextBoxR" Width="65px" text="0"/>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="tbUnAbNormal" runat="server" CssClass="TextBoxR" Enabled="false" Width="80px" text="0"/>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="tbUnbrd" runat="server" CssClass="TextBoxR" Enabled="false" Width="65px" text="0"/>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="tbUnNormal2" runat="server"  Enabled="false" CssClass="TextBoxR" text="0"
                                                    Width="70px" />
                                            </td>
                                            <td>
                                                <asp:TextBox ID="tbUnAbNormal2" runat="server"  Enabled="false" CssClass="TextBoxR" Width="70px" text="0"/>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="tbUnbrd2" runat="server"  Enabled="false"  CssClass="TextBoxR" Width="65px" text="0"/>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="tbUnNormal3" runat="server" CssClass="TextBoxR" Enabled="false" Width="65px" text="0"/>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="tbUnAbNormal3" runat="server" CssClass="TextBoxR"  Enabled="false" text="0"
                                                    Width="70px" />
                                            </td>
                                            <td>
                                                <asp:TextBox ID="tbUnbrd3" runat="server" CssClass="TextBoxR"  Enabled="false" text="0"
                                                    Width="70px" />
                                            </td>
                                           <td >
                                               <asp:Button class="bitbtn btngo" width ="70px" runat="server" ID="btnTotalUnComplete" Text="Total Janjang" />
                                            </td>
                                            
                                        </tr>
                                    </table>
                    <asp:Panel runat="server" ID="pnlInputUnProcess">
                        <br />
                        <asp:Panel runat="server" ID="PanelDevUnProcess">
                            <div style="border: 0px  solid; width: 100%; height: 100%; overflow: auto;">
                                
                                <asp:GridView ID="gvUnProcess" runat="server" AllowPaging="True" AllowSorting="True"
                                    CssClass="Grid" AutoGenerateColumns="False">
                                    <HeaderStyle CssClass="GridHeader" Wrap="false"></HeaderStyle>
                                    <RowStyle CssClass="GridItem" Wrap="false" />
                                    <AlternatingRowStyle CssClass="GridAltItem" />
                                    <PagerStyle CssClass="GridPager" />
                                    <Columns>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:CheckBox ID="cbSelectHd" runat="server" AutoPostBack="true" OnCheckedChanged="cbSelectHdUnProcess_CheckedChanged" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="cbSelect" runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        
                                        <asp:BoundField DataField="SPTBSNo" HeaderText="SPTBS No" SortExpression="RequestNo" />
                                        <asp:BoundField DataField="DateAngkut" DataFormatString="{0:dd MMM yyyy}" HtmlEncode="true"
                                            HeaderText="Date Angkut" SortExpression="DateAngkut" />
                                        <asp:BoundField DataField="CarNo" HeaderText="Car No" SortExpression="CarNo">
                                        </asp:BoundField>
                                        <%--<asp:BoundField DataField="Normal" HeaderText="Normal" SortExpression="Normal" />--%>
                                       
                                        <asp:TemplateField HeaderText="SPTBS Normal"  >
                                            <ItemTemplate>
                                                <asp:TextBox ID="tbNormalU" CssClass="TextBoxR" Enabled ="false" style="text-align: right"  Width="50px" runat="server" text='<%# DataBinder.Eval(Container.DataItem, "Normal") %>'/>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="SPTBS Abnormal" >
                                            <ItemTemplate>
                                                <asp:TextBox ID="tbAbNormalU" CssClass="TextBoxR" Enabled ="false" style="text-align: right"  Width="50px" runat="server" text='<%# DataBinder.Eval(Container.DataItem, "Abnormal") %>'/>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="SPTBS Brondolan">
                                            <ItemTemplate>
                                                <asp:TextBox ID="tbBrondolanU" CssClass="TextBoxR" Enabled ="false" style="text-align: right" Width="50px" runat="server" text='<%# DataBinder.Eval(Container.DataItem, "Brondolan") %>'/>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="TPH Normal">
                                            <ItemTemplate>
                                                <asp:TextBox ID="tbActNormalU" CssClass="TextBox" style="text-align: right" Width="50px" runat="server" text='<%# DataBinder.Eval(Container.DataItem, "ActNormal") %>'/>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="TPH Abnormal">
                                            <ItemTemplate>
                                                <asp:TextBox ID="tbActAbnormalU" CssClass="TextBox" style="text-align: right" Width="50px" runat="server" text='<%# DataBinder.Eval(Container.DataItem, "ActAbnormal") %>'/>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="TPH Brondolan">
                                            <ItemTemplate>
                                                <asp:TextBox ID="tbActBrondolanU" CssClass="TextBox" style="text-align: right" Width="50px" runat="server" text='<%# DataBinder.Eval(Container.DataItem, "ActBrondolan") %>'/>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                       
                                        <asp:TemplateField HeaderText="Diff Normal">
                                            <ItemTemplate>
                                                <asp:TextBox ID="tbDiffNormalU" CssClass="TextBoxR" Enabled ="false" style="text-align: right" Width="50px" runat="server" text='<%# DataBinder.Eval(Container.DataItem, "DiffNormal") %>'/>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Diff AbNormal">
                                            <ItemTemplate>
                                                <asp:TextBox ID="tbDiffAbNormalU" CssClass="TextBoxR" Enabled ="false" style="text-align: right" Width="50px" runat="server" text='<%# DataBinder.Eval(Container.DataItem, "DiffAbNormal") %>'/>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Diff Brondolan">
                                            <ItemTemplate>
                                                <asp:TextBox ID="tbDiffBrondolanU" CssClass="TextBoxR" Enabled ="false" style="text-align: right" Width="50px" runat="server" text='<%# DataBinder.Eval(Container.DataItem, "DiffBrondolan") %>'/>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                         <asp:TemplateField HeaderText="Remark">
                                            <ItemTemplate>
                                                <asp:TextBox ID="tbRemark" CssClass="TextBox" style="text-align: right" Width="100px" runat="server" text='<%# DataBinder.Eval(Container.DataItem, "Remark") %>'/>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </asp:Panel>
                        <br />
                        <asp:Label runat="server" ID="lbStatusUnProcess" ForeColor="Red" />
                    </asp:Panel>
                </asp:Panel>
            </asp:View>
        </asp:MultiView>
    </div>
    <asp:Label runat="server" ID="lbStatus" ForeColor="Red" />
    </form>
</body>
</html>
