<%@ Page Language="VB" AutoEventWireup="false" CodeFile="FormComplete.Aspx.vb" Inherits="FormComplete" %>

<%@ Register Assembly="FastReport" Namespace="FastReport.Web" TagPrefix="cc1" %>

<%@ Register Assembly="BasicFrame.WebControls.BasicDatePicker" Namespace="BasicFrame.WebControls"
    TagPrefix="BDP" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Print Form</title>
    <link href="../../Styles/Style.css" rel="stylesheet" type="text/css" />
 <link href="https://fonts.googleapis.com/css2?family=Muli:wght@300;400;600&display=swap" rel="stylesheet">    
    <style type="text/css">
        .style1
        {
            width: 409px;
        }
        .style8
        {
            width: 618px;
        }
        .style9
        {
            width: 84px;
        }
        .style10
        {
            width: 3px;
        }
        .style11
        {
            width: 250px;
        }
        </style>
</head>
<body>
    <form id="form1" runat="server">
    <div class="Content"> <%--nambahin ini doank--%>
        <asp:Panel runat="server" ID="pnlDt">
        <table style="background-color: #1085C7; color: #FFFFFF;">
            <tr> 
                <td class="style1">
                <div style="border:0px  solid; width:100%; height:100%; overflow:auto;">
                    Complete
                    <asp:Label ID="lbTitle" runat="server">Employee Leave</asp:Label>
                    </div>   
                </td>         
            </tr>
        </table>
            <asp:Panel ID="pnlEditDt" runat="server">
                <table class="style8">
                    <tr>
                        <td class="style9">
                            Trans No</td>
                        <td class="style10">
                            &nbsp;</td>
                        <td colspan="3">
                            <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>
                        </td>
                        <td rowspan="4">
                            <asp:Panel ID="PnlInfo" runat="server" Height="100%" Visible="false" 
                                Width="100%">
                                <asp:Label ID="lbInfo" runat="server" Font-Bold="True" ForeColor="Blue" 
                                    Text="Info Leave :"></asp:Label>
                                <br />
                                <div style="border:0px  solid; width:100%; height:100%; overflow:auto;">
                                    <asp:GridView ID="GridInfo" runat="server" AutoGenerateColumns="False" 
                                        ShowFooter="True">
                                        <HeaderStyle CssClass="GridHeader" />
                                        <RowStyle CssClass="GridItem" Wrap="false" />
                                        <AlternatingRowStyle CssClass="GridAltItem" />
                                        <PagerStyle CssClass="GridPager" />
                                        <Columns>
                                            <asp:BoundField DataField="EmpNumb" HeaderStyle-Width="120px" 
                                                HeaderText="Employee">
                                                <HeaderStyle Width="120px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="StrQtyCurrent" DataFormatString="{0:#,##0.##}" 
                                                HeaderStyle-Width="70px" HeaderText="Qty Current">
                                                <HeaderStyle Width="70px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="StrQtyRequest" DataFormatString="{0:#,##0.##}" 
                                                HeaderStyle-Width="70px" HeaderText="Qty Request">
                                                <HeaderStyle Width="70px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="StrQtyBalance" DataFormatString="{0:#,##0.##}" 
                                                HeaderText="Qty Balance" />
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td class="style9">
                            Employee</td>
                        <td class="style10">
                            :</td>
                        <td colspan="3">
                            <asp:TextBox ID="tbEmpNumb" runat="server" CssClass="TextBoxR" Enabled="False" 
                                MaxLength="12" ValidationGroup="Input" Width="100px" />
                            <asp:TextBox ID="tbEmpName" runat="server" CssClass="TextBoxR" Enabled="False" 
                                MaxLength="60" Width="337px" />
                        </td>
                    </tr>
                    <tr>
                        <td class="style9">
                            Leave Date</td>
                        <td class="style10">
                            :</td>
                        <td colspan="3">
                            <BDP:BasicDatePicker ID="tbHireDate" runat="server" ButtonImageHeight="19px" 
                                ButtonImageWidth="20px" DateFormat="dd MMM yyyy" DisplayType="TextBoxAndImage" 
                                Enabled="False" ReadOnly="true" ShowNoneButton="False" 
                                TextBoxStyle-CssClass="TextDate" ValidationGroup="Input"><TextBoxStyle CssClass="TextDate" /></BDP:BasicDatePicker>
                        </td>
                    </tr>
                    <tr>
                        <td class="style9">
                            Leave Type</td>
                        <td class="style10">
                            :</td>
                        <td class="style11">
                            <asp:TextBox ID="tbLeaveType" runat="server" CssClass="TextBox" Enabled="False" 
                                MaxLength="60" Width="337px" />
                        </td>
                        <td class="style11">
                            &nbsp;</td>
                        <td class="style10">
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td class="style9">
                            Date</td>
                        <td class="style10">
                            :</td>
                        <td colspan="3">
                            <table cellpadding="0" cellspacing="0">
                                <tr style="background-color:Silver;text-align:center">
                                    <td>
                                        Less 1 Day</td>
                                    <td>
                                        Start Date</td>
                                    <td>
                                        Start Time</td>
                                    <td>
                                        End Date</td>
                                    <td>
                                        End Time</td>
                                    <td>
                                        &nbsp;</td>
                                    <td>
                                        &nbsp;</td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:DropDownList ID="ddlFgLess1Day" runat="server" AutoPostBack="True" 
                                            CssClass="DropDownList" Height="16px" ValidationGroup="Input" Width="50px">
                                            <asp:ListItem>Y</asp:ListItem>
                                            <asp:ListItem Selected="True">N</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <BDP:BasicDatePicker ID="tbStartDate" runat="server" ButtonImageHeight="19px" 
                                            ButtonImageWidth="20px" DateFormat="dd MMM yyyy" DisplayType="TextBoxAndImage" 
                                            ReadOnly="true" ShowNoneButton="False" TextBoxStyle-CssClass="TextDate" 
                                            ValidationGroup="Input"><TextBoxStyle CssClass="TextDate" /></BDP:BasicDatePicker>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="tbStartTime" runat="server" CssClass="TextBox" 
                                            ValidationGroup="Input" Width="83px" AutoPostBack="True" />
                                    </td>
                                    <td>
                                        <BDP:BasicDatePicker ID="tbEndDate" runat="server" AutoPostBack="True" 
                                            ButtonImageHeight="19px" ButtonImageWidth="20px" DateFormat="dd MMM yyyy" 
                                            DisplayType="TextBoxAndImage" ReadOnly="true" ShowNoneButton="False" 
                                            TextBoxStyle-CssClass="TextDate" ValidationGroup="Input"><TextBoxStyle CssClass="TextDate" /></BDP:BasicDatePicker>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="tbEndTime" runat="server" AutoPostBack="True" 
                                            CssClass="TextBox" ValidationGroup="Input" Width="83px" />
                                    </td>
                                    <td>
                                        &nbsp;</td>
                                    <td>
                                        &nbsp;</td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="style9">
                            Qty (days)</td>
                        <td class="style10">
                            :</td>
                        <td colspan="3">
                            <table cellpadding="0" cellspacing="0">
                                <tr style="background-color:Silver;text-align:center">
                                    <td>
                                        Total</td>
                                    <td>
                                        Holiday</td>
                                    <td>
                                        Dispensasi</td>
                                    <td>
                                        Taken</td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="tbTotal" runat="server" CssClass="TextBoxR" Enabled="false" 
                                            ValidationGroup="Input" Width="80px" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="tbHoliday" runat="server" CssClass="TextBoxR" Enabled="False" 
                                            ValidationGroup="Input" Width="83px" />
                                    </td>
                                    <td style="font-weight: 700">
                                        <asp:TextBox ID="tbDispensasi" runat="server" CssClass="TextBoxR" 
                                            Enabled="false" ValidationGroup="Input" Width="80px" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="tbTaken" runat="server" CssClass="TextBoxR" Enabled="false" 
                                            ValidationGroup="Input" Width="80px" />
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="style9">
                            Remark</td>
                        <td class="style10">
                            :</td>
                        <td class="style1" colspan="3" style="margin-left: 40px">
                            <asp:TextBox ID="tbReason" runat="server" CssClass="TextBox" MaxLength="255" 
                                ValidationGroup="Input" Width="445px" />
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td class="style9">
                            &nbsp;</td>
                        <td class="style10">
                            &nbsp;</td>
                        <td class="style1" colspan="3" style="margin-left: 40px">
                            &nbsp;</td>
                    </tr>
                </table>
                &nbsp;<asp:GridView ID="GridDt" runat="server" AutoGenerateColumns="False" 
                    ShowFooter="True">
                    <HeaderStyle CssClass="GridHeader" />
                    <RowStyle CssClass="GridItem" Wrap="false" />
                    <AlternatingRowStyle CssClass="GridAltItem" />
                    <PagerStyle CssClass="GridPager" />
                    <Columns>
                        <%--                            <asp:BoundField DataField="SubSectionName" HeaderText="Sub Section" HeaderStyle-Width="150px" />--%>
                        <asp:BoundField DataField="LeaveDate" 
                            HeaderText="Leave Date">
                            <HeaderStyle Width="70px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="FgLeaves" HeaderStyle-Width="200px" 
                            HeaderText="Leaves">
                            <HeaderStyle Width="150px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Remark" HeaderStyle-Width="70px" 
                            HeaderText="Remark" htmlencode="true">
                            <HeaderStyle Width="50px" />
                        </asp:BoundField>
                    </Columns>
                </asp:GridView>
            </asp:Panel>
            <asp:Button ID="btnOK2" runat="server" class="bitbtn btnok" Text="OK" Width="59px" />
            <asp:Button ID="btnCancel2" runat="server" class="bitbtn btncancel" Text="Cancel" Width="59px" />
            <asp:Button ID="btnBack2" runat="server" class="bitbtn btnback" Text="Back" 
                Width="59px" />           
       </asp:Panel>          
    <asp:Label ID="lbstatus" ForeColor="red" runat="server"></asp:Label>        
    </div>
    </form>
</body>
</html>
